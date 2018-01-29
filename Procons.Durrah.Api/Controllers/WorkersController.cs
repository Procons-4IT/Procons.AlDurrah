namespace Procons.Durrah.Api.Controllers
{
    using Common;
    using Facade;
    using Procons.Durrah.Common.Enumerators;
    using Procons.Durrah.Common.Models;
    using Procons.Durrah.Controllers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.SessionState;

    public class ParamBody
    {
        public string WorkerType { get; set; }
    }
    public class WorkersController : BaseApiController, IRequiresSessionState
    {

        B1Facade b1Facade = new B1Facade();
        WorkersFacade workersFacade = new WorkersFacade();

        public IHttpActionResult GetSearchLookups()
        {

            Dictionary<string, List<LookupItem>> result = new Dictionary<string, List<LookupItem>>();
            List<LookupItem> languages = workersFacade.GetLanguagesLookups();
            List<LookupItem> age = new List<LookupItem>() { new LookupItem("21-25", "21-25"), new LookupItem("25-35", "25-35"), new LookupItem("35-45", "35-45"), new LookupItem("45-55", "45-55") };
            List<LookupItem> nationality = workersFacade.GetCountriesLookups();
            List<LookupItem> religion = workersFacade.GetReligionLookups();
            List<LookupItem> education = workersFacade.GetEducationLookups();
            List<LookupItem> gender = new List<LookupItem>() { new LookupItem(Utilities.GetResourceValue("M"), "M"), new LookupItem(Utilities.GetResourceValue("F"), "F") };
            List<LookupItem> maritalStatus = workersFacade.GetMaritalStatusLookups();
            List<LookupItem> workerTypes = workersFacade.GetWorkersTypesLookups();
            List<LookupItem> countries = workersFacade.GetAllCountriesLookups();
            result.Add("Languages", languages);
            result.Add("Age", age);
            result.Add("Education", education);
            result.Add("Religion", religion);
            result.Add("Nationality", nationality);
            result.Add("Gender", gender);
            result.Add("MaritalStatus", maritalStatus);
            result.Add("WorkerTypes", workerTypes);
            result.Add("Country", countries);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult GetItemsLookup([FromBody]ParamBody workerType)
        {
            var result = workersFacade.GetItemsByWorkerType(workerType.WorkerType);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage CallKnetGateway([FromBody]Transaction transaction)
        {
            Transaction returnedTrans = null;
            try
            {

                IEnumerable<Claim> claims;
                var cardCode = string.Empty;

                claims = ((ClaimsIdentity)User.Identity).Claims;
                cardCode = claims.Where(x => x.Type == Constants.ServiceLayer.CardCode).FirstOrDefault().Value;
                transaction.CardCode = cardCode;

                var knetSvc = new KnetService.KnetServiceClient();
                transaction.Amount = workersFacade.GetDownPaymentAmount().ToString();
                try
                {
                    returnedTrans = knetSvc.CallKnetGateway(transaction);
                }
                catch (Exception ex)
                {
                    Utilities.LogException(ex);
                }

                knetSvc.Close();

                if (returnedTrans == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
                }
                else
                {
                    transaction.PaymentID = returnedTrans.PaymentID;
                    transaction.TrackID = returnedTrans.TrackID;
                    transaction.TranID = returnedTrans.TranID;
                    var result = workersFacade.CreateSalesOrder(transaction, cardCode);

                    if (result == double.MinValue)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, returnedTrans.PaymentPage + "?PaymentID=" + returnedTrans.PaymentID);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.OK, returnedTrans.PaymentPage + "?PaymentID=" + returnedTrans.PaymentID);
            }

        }

        [Authorize]
        public IHttpActionResult GetDownPaymentAmount()
        {
            workersFacade.GetDownPaymentAmount();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CreatePayment([FromBody]Transaction payment)
        {
            var paymentAmount = workersFacade.GetDownPaymentAmount().ToString();
            var isSalesOrderAvailable = workersFacade.CheckSalesOderAvailability(payment.PaymentID);
            var userEmail = workersFacade.GetEmailAddress(payment.CardCode);

            payment.Amount = paymentAmount;
            if (payment.Result == KnetResults.CAPTURED.GetDescription() && isSalesOrderAvailable)
            {
                var itemCode = workersFacade.GetItemCodeByPaymentId(payment.PaymentID);

                try
                {

                    var tokens = new Dictionary<string, string>();
                    tokens.Add(Constants.KNET.MerchantTrackID, payment.TrackID);
                    tokens.Add(Constants.KNET.PaymentID, payment.PaymentID);
                    tokens.Add(Constants.KNET.ReferenceID, payment.Ref);
                    tokens.Add(Constants.KNET.TransactionAmount, payment.Amount);
                    tokens.Add(Constants.KNET.ItemCode, itemCode);

                    idMessage.Destination = userEmail;
                    idMessage.Subject = Utilities.GetResourceValue(Constants.Resources.Transaction_Completed);
                    var messageBody = Utilities.GetResourceValue(Constants.Resources.KnetEmailConfirmation).GetMessageBody(tokens);
                    idMessage.Body = messageBody;
                    emailService.SendAsync(idMessage);
                }
                catch (Exception ex)
                {
                    Utilities.LogException(ex);
                }

                var result = workersFacade.SavePaymentDetails(payment);

                if (result != null)
                {
                    payment.UDF1 = Utilities.GetResourceValue(Constants.Resources.Successfull_Transaction);
                    return Ok(payment);
                }

                else
                {
                    payment.UDF1 = Utilities.GetResourceValue(Constants.Resources.Failed_Transaction);
                    var paymentString = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                    return InternalServerError(new Exception(paymentString));
                }
            }
            else if (payment.Result == KnetResults.CANCELED.GetDescription())
            {
                payment.UDF1 = Utilities.GetResourceValue(Constants.Resources.Transaction_Cancelled);
                return Ok(payment);
            }
            else
            {
                payment.UDF1 = Utilities.GetResourceValue(Constants.Resources.Failed_Transaction);
                var paymentString = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                return InternalServerError(new Exception(paymentString));
            }
        }

        [HttpPost]
        public IHttpActionResult CancelSalesOrder([FromBody]Transaction payment)
        {
            try
            {
                var paymentAmount = workersFacade.GetDownPaymentAmount().ToString();
                var userEmail = workersFacade.GetEmailAddress(payment.CardCode);

                if (payment.Result == KnetResults.CANCELED.GetDescription() || payment.Result == KnetResults.NOTCAPTURED.GetDescription())
                {
                    workersFacade.CancelSalesOrder(payment);
                    try
                    {
                        var tokens = new Dictionary<string, string>();
                        tokens.Add(Constants.KNET.MerchantTrackID, payment.TrackID);
                        tokens.Add(Constants.KNET.PaymentID, payment.PaymentID);

                        tokens.Add(Constants.KNET.TransactionAmount, paymentAmount + payment.Amount);
                        tokens.Add(Constants.KNET.ItemCode, payment.Code);

                        idMessage.Destination = userEmail;
                        string messageBody = string.Empty;
                        if (payment.Result == KnetResults.CANCELED.GetDescription())
                        {
                            idMessage.Subject = Utilities.GetResourceValue(Constants.Resources.Transaction_Cancelled);
                            messageBody = Utilities.GetResourceValue(Constants.Resources.TransactionCancellationBody).GetMessageBody(tokens);
                        }
                        else
                        {
                            tokens.Add(Constants.KNET.ReferenceID, payment.Ref);
                            idMessage.Subject = Utilities.GetResourceValue(Constants.Resources.Failed_Transaction);
                            messageBody = Utilities.GetResourceValue(Constants.Resources.TransactionErrorBody).GetMessageBody(tokens);
                        }

                        idMessage.Body = messageBody;
                        emailService.SendAsync(idMessage);
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogException(ex);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Authorize]
        public IHttpActionResult GetAgentWorkers()
        {
            var cardCode = GetCurrentUserCardCode();
            var requestUrl = GetMainUrl();
            var workers = workersFacade.GetAgentWorkers(cardCode, requestUrl);
            return Ok(workers);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetWorker(string code)
        {
            var cardCode = GetCurrentUserCardCode();
            var requestUrl = GetMainUrl();
            var workers = workersFacade.GetAgentWorkers(cardCode, requestUrl);
            return Ok(workers);
        }



        [HttpPost]
        [Authorize]
        public IHttpActionResult GetWorkers([FromBody]Catalogue worker)
        {
            var requestUrl = GetMainUrl();
            var workers = workersFacade.GetWorkers(worker, requestUrl, WorkerStatus.Opened);
            return Ok(workers);
        }

        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> AddWorker()
        {
            try
            {
                var cardCode = GetCurrentUserCardCode();
                var worker = await SaveFile();
                var result = workersFacade.CreateWorker(worker);
                if (result)
                    return Ok((Utilities.GetResourceValue(Constants.Resources.WorkerCreatedSuccessfully)));
                else
                    return Ok(Utilities.GetResourceValue(Constants.Resources.WorkerAreadyCreated));
                //return  InternalServerError(new Exception(Utilities.GetResourceValue(Constants.Resources.WorkerAreadyCreated)));
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                //return InternalServerError(new Exception(Utilities.GetResourceValue(Constants.Resources.ErrorOccured)));
                return Ok(Utilities.GetResourceValue(Constants.Resources.ErrorOccured));
            }
        }

        public async Task<IHttpActionResult> UpdateWorker()
        {
            try
            {
                var cardCode = GetCurrentUserCardCode();
                var worker = await UpdateFile();
                var result = workersFacade.UpdateWorker(worker, cardCode);
                if (result)
                    return Ok(Utilities.GetResourceValue(Constants.Resources.WorkerUpdatedSuccessfully));
                else
                    return InternalServerError(new Exception(Utilities.GetResourceValue(Constants.Resources.WorkerAreadyCreated)));
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return InternalServerError(new Exception(Utilities.GetResourceValue(Constants.Resources.ErrorOccured)));
            }

        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteWorker([FromBody]Worker worker)
        {
            var cardCode = GetCurrentUserCardCode();
            var result = workersFacade.DeleteWorker(worker.WorkerCode, cardCode);
            return Ok(result);
        }

        [HttpGet]
        public HttpResponseMessage Image(string path)
        {
            var defaultImagePth = HostingEnvironment.MapPath(@"\Assets\src\app\images\no_photo.png");
            var attachmentsPath = workersFacade.GetAttachmentsPath();
            var fileName = string.Empty;

            using (var stream = new MemoryStream())
            {
                System.IO.FileInfo attachmentFile = null;

                if (System.IO.File.Exists($"{attachmentsPath}{path}"))
                    fileName = $"{attachmentsPath}{path}";
                else
                    fileName = defaultImagePth;

                attachmentFile = new System.IO.FileInfo(fileName);
                using (FileStream fs = File.OpenRead(fileName))
                {
                    fs.CopyTo(stream);
                }
                var mimeType = MimeMapping.GetMimeMapping(attachmentFile.Extension);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                {
                    FileName = attachmentFile.Name
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                return result;
            }
        }

        #region Private Methods

        private async Task<Worker> SaveFile()
        {
            var worker = new Worker();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var provider = new MultipartFormDataStreamProvider(rootPath);
            var task = await Request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(t =>
             {
                 if (t.IsCanceled || t.IsFaulted)
                     Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);

                 foreach (MultipartFileData item in provider.FileData)
                 {
                     try
                     {
                         string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                         var nameWithoutExtention = Path.GetFileNameWithoutExtension(name);
                         string newFileName = $"{nameWithoutExtention}.{Guid.NewGuid()}{Path.GetExtension(name)}";
                         File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                         Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                         string fileRelativePath = string.Concat("~/UploadedFiles/", newFileName);
                         Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                         if (item.Headers.ContentDisposition.Name.Trim('"').Equals("Photo"))
                             worker.Photo = fileRelativePath;
                         else if (item.Headers.ContentDisposition.Name.Trim('"').Equals("Passport"))
                             worker.Passport = fileRelativePath;
                         else
                             worker.License = fileRelativePath;
                     }
                     catch (Exception ex)
                     {
                         string message = ex.Message;
                     }
                 }

                 PopulateWorker(provider, ref worker);

                 return Request.CreateResponse(HttpStatusCode.Created, worker);
             });
            return worker;
        }

        private async Task<Worker> UpdateFile()
        {
            var worker = new Worker();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var provider = new MultipartFormDataStreamProvider(rootPath);
            var task = await Request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);

                foreach (MultipartFileData item in provider.FileData)
                {
                    try
                    {
                        string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                        var nameWithoutExtention = Path.GetFileNameWithoutExtension(name);
                        string newFileName = $"{nameWithoutExtention}.{Guid.NewGuid()}{Path.GetExtension(name)}";
                        File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                        Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                        string fileRelativePath = string.Concat("~/UploadedFiles/", newFileName);
                        Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                        if (item.Headers.ContentDisposition.Name.Trim('"').Equals("Photo"))
                            worker.Photo = fileRelativePath;
                        else if (item.Headers.ContentDisposition.Name.Trim('"').Equals("Passport"))
                            worker.Passport = fileRelativePath;
                        else
                            worker.License = fileRelativePath;
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }

                if (worker.Photo == null)
                    worker.Photo = MapField<string>(provider.FormData["Photo"]);
                if (worker.Passport == null)
                    worker.Passport = MapField<string>(provider.FormData["Passport"]);
                if (worker.License == null)
                    worker.License = MapField<string>(provider.FormData["License"]);

                PopulateWorker(provider, ref worker);

                return Request.CreateResponse(HttpStatusCode.Created, worker);
            });
            return worker;
        }

        private void PopulateWorker(MultipartFormDataStreamProvider provider, ref Worker worker)
        {
            worker.Age = MapField<int>(provider.FormData["Age"]);
            worker.Agent = GetCurrentUserCardCode();
            worker.BirthDate = MapField<string>(provider.FormData["BirthDate"]);
            worker.BirthDate = MapField<string>(provider.FormData["BirthDate"]);
            worker.Mobile = MapField<string>(provider.FormData["Mobile"]);
            worker.CivilId = MapField<string>(provider.FormData["CivilId"]);
            worker.Name = MapField<string>(provider.FormData["Name"]);
            worker.Code = MapField<string>(provider.FormData["Code"]);
            worker.Education = MapField<string>(provider.FormData["Education"]);
            worker.Gender = MapField<string>(provider.FormData["Gender"]);
            worker.Language = MapField<string>(provider.FormData["Language"]);
            worker.MaritalStatus = MapField<string>(provider.FormData["MaritalStatus"]);
            worker.Nationality = MapField<string>(provider.FormData["Nationality"]);
            worker.PassportExpDate = MapField<string>(provider.FormData["PassportExpDate"]);
            worker.PassportIssDate = MapField<string>(provider.FormData["PassportIssDate"]);
            worker.PassportPoIssue = MapField<string>(provider.FormData["PassportPoIssue"]);
            worker.PassportNumber = MapField<string>(provider.FormData["PassportNumber"]);
            worker.Religion = MapField<string>(provider.FormData["Religion"]);
            worker.Status = "1";
            worker.WorkerType = MapField<string>(provider.FormData["WorkerType"]);
            if (!string.IsNullOrEmpty(provider.FormData["Price"]))
                worker.Price = MapField<float>(provider.FormData["Price"]);
            worker.Salary = MapField<float>(provider.FormData["Salary"]);
            worker.SerialNumber = MapField<string>(provider.FormData["CivilId"]);
            worker.Video = MapField<string>(provider.FormData["Video"]);
            worker.Height = MapField<string>(provider.FormData["Height"]);
            worker.Weight = MapField<string>(provider.FormData["Weight"]);
            worker.WorkerCode = MapField<string>(provider.FormData["WorkerCode"]);
            worker.WorkerName = MapField<string>(provider.FormData["WorkerName"]);
            worker.Languages = workersFacade.GetLanguagesById(MapField<string>(provider.FormData["Languages"]));

            worker.Hobbies = MapField<string>(provider.FormData["Hobbies"]);
            worker.Location = MapField<string>(provider.FormData["Location"]);
            string inNew = MapField<string>(provider.FormData["IsNew"]) == "" ? "Y" : MapField<string>(provider.FormData["IsNew"]);
            worker.IsNew = inNew;
            worker.Period = MapField<int>(provider.FormData["Period"]);
            worker.Experiences = Utilities.ConvertJsonStringToObject<ICollection<Experience>>(MapField<string>(provider.FormData["Experiences"]));
        }
        #endregion
    }
}