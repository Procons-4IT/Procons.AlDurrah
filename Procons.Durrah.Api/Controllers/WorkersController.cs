namespace Procons.Durrah.Api.Controllers
{
    using Common;
    using e24PaymentPipeLib;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;
    using System.Linq;
    using Facade;
    using System.Web;
    using Procons.Durrah.Controllers;
    using System.IO;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using System.Drawing.Imaging;

    public class WorkersController : BaseApiController
    {

        B1Facade b1Facade = new B1Facade();
        WorkersFacade workersFacade = new WorkersFacade();

        public IHttpActionResult GetSearchLookups()
        {

            Dictionary<string, List<LookupItem>> result = new Dictionary<string, List<LookupItem>>();
            List<LookupItem> languages = workersFacade.GetLanguagesLookups();
            List<LookupItem> age = new List<LookupItem>() { new LookupItem("18-25", "18-25"), new LookupItem("25-35", "25-35"), new LookupItem("35-45", "35-45"), new LookupItem("45-55", "45-55") };
            List<LookupItem> nationality = workersFacade.GetCountriesLookups();
            List<LookupItem> gender = new List<LookupItem>() { new LookupItem(Utilities.GetResourceValue("M"), "M"), new LookupItem(Utilities.GetResourceValue("F"), "F") };
            List<LookupItem> maritalStatus = workersFacade.GetMaritalStatusLookups();
            List<LookupItem> workerTypes = workersFacade.GetWorkersTypesLookups();
            result.Add("Languages", languages);
            result.Add("Nationality", nationality);
            result.Add("Gender", gender);
            result.Add("MaritalStatus", maritalStatus);
            result.Add("WorkerTypes", workerTypes);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage CallKnetGatewayOld([FromBody]Transaction transaction)
        {
            KnetService.KnetServiceClient knetSvc = new KnetService.KnetServiceClient();
            var returnedTrans = knetSvc.CallKnetGateway(transaction);
            short TransVal;
            string varPaymentID, varPaymentPage, varErrorMsg, varRawResponse;

            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var cardCode = claims.Where(x => x.Type == Constants.ServiceLayer.CardCode).FirstOrDefault().Value;

            transaction.TrackID = (new Random().Next(10000000) + 1).ToString();

            e24PaymentPipeCtlClass payment = new e24PaymentPipeCtlClass();
            payment.Action = "1";
            payment.Amt = workersFacade.GetDownPaymentAmount().ToString();
            payment.Currency = "414";
            payment.Language = "USA";
            payment.ResponseUrl = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResponseUrl);
            payment.ErrorUrl = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ErrorUrl);
            payment.TrackId = transaction.TrackID;
            payment.ResourcePath = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResourcePath);
            payment.Alias = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.Alias); ;

            TransVal = payment.PerformInitTransaction();
            varRawResponse = payment.RawResponse;
            varPaymentID = payment.PaymentId;
            varPaymentPage = payment.PaymentPage;
            varErrorMsg = payment.ErrorMsg;
            transaction.PaymentID = varPaymentID;

            if (TransVal != 0)
            {
                Utilities.LogException(varErrorMsg);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
            }
            else
            {
                var result = workersFacade.CreateSalesOrder(transaction, cardCode);
                if (result == double.MinValue)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, varPaymentPage + "?PaymentID=" + varPaymentID);
            }

        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage CallKnetGateway([FromBody]Transaction transaction)
        {
            Transaction returnedTrans = null;
            IEnumerable<Claim> claims;
            var cardCode = string.Empty;

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


            claims = ((ClaimsIdentity)User.Identity).Claims;
            cardCode = claims.Where(x => x.Type == Constants.ServiceLayer.CardCode).FirstOrDefault().Value;

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

        [Authorize]
        public IHttpActionResult GetDownPaymentAmount()
        {
            workersFacade.GetDownPaymentAmount();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult CreatePayment([FromBody]Transaction payment)
        {
            var paymentAmount = workersFacade.GetDownPaymentAmount().ToString();


            payment.Amount = paymentAmount;
            if (payment.Result == "captured")
            {
                var itemCode = workersFacade.GetItemCodeByPaymentId(payment.PaymentID);

                Utilities.LogException($"Captured Code {payment.Code}");
                var tokens = new Dictionary<string, string>();
                tokens.Add(Constants.KNET.MerchantTrackID, payment.TrackID);
                tokens.Add(Constants.KNET.PaymentID, payment.PaymentID);
                tokens.Add(Constants.KNET.ReferenceID, payment.Ref);
                tokens.Add(Constants.KNET.TransactionAmount, payment.Amount);
                tokens.Add(Constants.KNET.ItemCode, itemCode);

                idMessage.Destination = base.GetCurrentUserEmail();
                idMessage.Subject = Utilities.GetResourceValue(Constants.Resources.Transaction_Completed);
                var messageBody = Utilities.GetResourceValue(Constants.Resources.KnetEmailConfirmation).GetMessageBody(tokens);
                idMessage.Body = messageBody;
                emailService.SendAsync(idMessage);
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
            else if (payment.Result == "canceled")
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
        //[Authorize]
        public IHttpActionResult GetAgentWorkers()
        {
            var cardCode = GetCurrentUserCardCode();
            var workers = workersFacade.GetAgentWorkers(cardCode);
            return Ok(workers);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetWorkers([FromBody]Catalogue worker)
        {
            Uri uri = new Uri(Request.RequestUri.ToString());
            var requestUrl = $"{uri.Scheme}{ Uri.SchemeDelimiter}{uri.Host}:{uri.Port}";
            var workers = workersFacade.GetWorkers(worker, requestUrl);
            return Ok(workers);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddWorker()
        {
            var cardCode = GetCurrentUserCardCode();
            var worker = await SaveFile();
            var result = workersFacade.CreateWorker(worker);
            return Ok(result);
        }

        public async Task<IHttpActionResult> UpdateWorker()
        {
            var cardCode = GetCurrentUserCardCode();
            var worker = await SaveFile();
            var result = workersFacade.UpdateWorker(worker, cardCode);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult DeleteWorker([FromBody]Worker worker)
        {
            var cardCode = GetCurrentUserCardCode();
            var result = workersFacade.DeleteWorker(worker.WorkerCode, cardCode);
            return Ok(result);
        }

        [HttpGet]
        public HttpResponseMessage Image(string path)
        {
            var attachmentsPath = workersFacade.GetAttachmentsPath();
            using (var image = System.Drawing.Image.FromFile($"{attachmentsPath}{path}"))
            {
                byte[] imageBytes = null;
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    imageBytes = ms.ToArray();
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(imageBytes);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    return result;
                }
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
                 {
                     Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                 }
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
                         else
                             worker.Passport = fileRelativePath;
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

        private Worker SaveFileNew()
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
            Task<MultipartFormDataStreamProvider> task = Request.Content.ReadAsMultipartAsync(provider);
            task.RunSynchronously();

            foreach (MultipartFileData item in provider.FileData)
            {
                try
                {
                    string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                    string newFileName = string.Concat(Guid.NewGuid(), Path.GetExtension(name));
                    File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                    Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                    string fileRelativePath = string.Concat("~/UploadedFiles/", newFileName);
                    Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                    if (item.Headers.ContentDisposition.Name.Trim('"').Equals("Photo"))
                        worker.Photo = fileRelativePath;
                    else
                        worker.Passport = fileRelativePath;
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }

            PopulateWorker(provider, ref worker);
            return worker;
        }

        private void PopulateWorker(MultipartFormDataStreamProvider provider, ref Worker worker)
        {
            worker.Age = MapField<int>(provider.FormData["Age"]);
            worker.Agent = GetCurrentUserCardCode();
            worker.BirthDate = MapField<string>(provider.FormData["BirthDate"]);
            worker.CivilId = MapField<string>(provider.FormData["CivilId"]);
            worker.Code = MapField<string>(provider.FormData["Code"]);
            worker.Education = MapField<string>(provider.FormData["Education"]);
            worker.Gender = MapField<string>(provider.FormData["Gender"]);
            worker.Language = MapField<string>(provider.FormData["Language"]);
            worker.MaritalStatus = MapField<string>(provider.FormData["MaritalStatus"]);
            worker.Nationality = MapField<string>(provider.FormData["Nationality"]);
            //worker.Price = MapField<string>(provider.FormData["PassportExpDate"]);
            //worker.Photo= MapField<string>(provider.FormData["PassportExpDate"]);
            //worker.Passport = MapField<string>(provider.FormData["Age"]);
            worker.PassportExpDate = MapField<string>(provider.FormData["PassportExpDate"]);
            worker.PassportIssDate = MapField<string>(provider.FormData["PassportIssDate"]);
            worker.PassportNumber = MapField<string>(provider.FormData["PassportNumber"]);
            worker.Religion = MapField<string>(provider.FormData["Religion"]);
            worker.Status = "1";
            worker.SerialNumber = MapField<string>(provider.FormData["CivilId"]);
            worker.Video = MapField<string>(provider.FormData["Video"]);
            worker.Height = MapField<string>(provider.FormData["Height"]);
            worker.Weight = MapField<string>(provider.FormData["Weight"]);
            worker.WorkerCode = MapField<string>(provider.FormData["WorkerCode"]);
        }

        #endregion
    }
}