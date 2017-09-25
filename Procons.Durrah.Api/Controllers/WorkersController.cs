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
    using System.Collections.Specialized;

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
            List<LookupItem> gender = new List<LookupItem>() { new LookupItem("Male", "M"), new LookupItem("Female", "F") };
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
        public HttpResponseMessage CallKnetGateway([FromBody]Transaction transaction)
        {
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
            payment.Udf1 = "User Defined Field 1";
            payment.Udf2 = "User Defined Field 2";
            payment.Udf3 = "User Defined Field 3";
            payment.Udf4 = "User Defined Field 4";
            payment.Udf5 = "User Defined Field 5";

            TransVal = payment.PerformInitTransaction();
            varRawResponse = payment.RawResponse;
            varPaymentID = payment.PaymentId;
            varPaymentPage = payment.PaymentPage;
            varErrorMsg = payment.ErrorMsg;
            transaction.PaymentID = varPaymentID;

            if (TransVal != 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
            }
            else
            {
                var result = workersFacade.CreateSalesOrder(transaction, cardCode);
                if (result == double.MinValue)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, varPaymentPage + "?PaymentID=" + varPaymentID);
                //return Request.CreateResponse(HttpStatusCode.OK, varPaymentPage + "?PaymentID=" + varPaymentID);
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
        public HttpResponseMessage CreatePayment([FromBody]Transaction payment)
        {
            var result = workersFacade.SavePaymentDetails(payment);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Transaction created successfully!!!");
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Transaction failed!!!");
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetAgentWorkers([FromBody]Worker worker)
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var cardCode = claims.Where(x => x.Type == worker.Agent).FirstOrDefault().Value;
            //workersFacade.GetAgentWorkers(cardCode);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetWorkers([FromBody]Catalogue worker)
        {
            var workers = workersFacade.GetWorkers(worker);
            return Ok(workers);
        }

        [HttpPost]
        public IHttpActionResult AddWorker()
        {
            var cardCode = GetCurrentUserCardCode();
            SaveFile();
            return Ok();
            //var result = workersFacade.CreateWorker(worker);
            //return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteWorker(string code)
        {
            return Ok();
        }

        #region Private Methods
        
        private void SaveFile()
        {
            var worker = new Worker();
            var savedFilePath = new List<string>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string rootPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var provider = new MultipartFormDataStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
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
                            string newFileName = Guid.NewGuid() + Path.GetExtension(name);
                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            string fileRelativePath = "~/UploadedFiles/" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                            savedFilePath.Add(fileFullPath.ToString());
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
                    foreach (var keyv in provider.FormData.AllKeys)
                    {
                        //worker.Age = MapField<int>(provider.FormData["Age"]);
                        //worker.Agent = GetCurrentUserCardCode(); 
                        //worker.BirthDate = MapField<string>(provider.FormData["Age"]);
                        //worker.CivilId = MapField<string>(provider.FormData["CivilId"]);
                        //worker.Code = MapField<string>(provider.FormData["Code"]);
                        //worker.Education = MapField<string>(provider.FormData["Education"]);
                        //worker.Gender = MapField<string>(provider.FormData["Gender"]);
                        //worker.Height = MapField<string>(provider.FormData["Height"]);
                        //worker.Language = MapField<string>(provider.FormData["Language"]);
                        //worker.MaritalStatus = MapField<string>(provider.FormData["MaritalStatus"]);
                        //worker.Nationality = MapField<string>(provider.FormData["Nationality"]);
                        ////worker.Passport = MapField<string>(provider.FormData["Age"]);
                        //worker.PassportExpDate = MapField<string>(provider.FormData["PassportExpDate"]);
                        //worker.PassportIssDate = MapField<string>(provider.FormData["PassportIssDate"]);
                        //worker.PassportNumber = MapField<string>(provider.FormData["PassportNumber"]);
                        //worker.Photo = MapField<string>(provider.FormData["PassportExpDate"]);
                        ////worker.Price = MapField<string>(provider.FormData["PassportExpDate"]);
                        ////worker.Photo= MapField<string>(provider.FormData["PassportExpDate"]);
                        //worker.Religion = MapField<string>(provider.FormData["Religion"]);
                        //worker.SerialNumber = MapField<string>(provider.FormData["PassportExpDate"]);
                        //worker.Status = "1";
                        //worker.SerialNumber = MapField<string>(provider.FormData["CivilId"]);
                        //worker.Video = MapField<string>(provider.FormData["Video"]);
                        //worker.Height = MapField<string>(provider.FormData["Video"]);
                        //worker.Weight= MapField<string>(provider.FormData["Video"]);
                    }

                    PopulateWorker(provider, ref worker);

                    return Request.CreateResponse(HttpStatusCode.Created, savedFilePath);
                });
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
        }

        #endregion
    }
}