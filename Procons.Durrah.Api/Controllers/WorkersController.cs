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

    public class LookupItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public LookupItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
    public class WorkersController : ApiController
    {
        B1Facade b1Facade = Factory.DeclareClass<B1Facade>();
        WorkersFacade workersFacade = Factory.DeclareClass<WorkersFacade>();
        public IHttpActionResult GetSearchLookups()
        {

            Dictionary<string, List<LookupItem>> result = new Dictionary<string, List<LookupItem>>();
            List<LookupItem> languages = new List<LookupItem>() { new LookupItem("Arabic", "1"), new LookupItem("English", "2") };
            List<LookupItem> nationality = new List<LookupItem>() { new LookupItem("India", "1"), new LookupItem("Bengladish", "2") };
            List<LookupItem> gender = new List<LookupItem>() { new LookupItem("Male", "1"), new LookupItem("Female", "2") };
            List<LookupItem> maritalStatus = new List<LookupItem>() { new LookupItem("Married", "1"), new LookupItem("Engaged", "2"), new LookupItem("Divorced", "3") };
            List<LookupItem> workerTypes = new List<LookupItem>() { new LookupItem("Servant", "1"), new LookupItem("Driver", "2") };
            result.Add("Languages", languages);
            result.Add("Nationality", nationality);
            result.Add("Gender", gender);
            result.Add("MaritalStatus", maritalStatus);
            result.Add("WorkerTypes", workerTypes);

            return Ok(result);
        }

        [HttpPost]
        public HttpResponseMessage CallKnetGateway([FromBody]Transaction transaction)
        {
            transaction.TrackID = (new Random().Next(10000000) + 1).ToString();
            e24PaymentPipeCtlClass payment = new e24PaymentPipeCtlClass();
            payment.Action = "1";            // Purchase Transaction
            payment.Amt = transaction.Amount;          // The amount of purchase
            payment.Currency = "414";          // KD Currency
            payment.Language = "USA";         // Payment Page Language
            payment.ResponseUrl = "http://localhost:59822/Response.aspx"; // Your response URL where you will be notified with of transaction response
            payment.ErrorUrl = "http://localhost:59822/Error.aspx"; //
            payment.TrackId = transaction.TrackID; // You should create a new unique track ID for each transaction
            payment.ResourcePath = @"C:\Workspace\Resource\"; // Directory to your resource.cgn ending with \
            payment.Alias = "durra"; // Alias of the plug-in
            payment.Udf1 = "User Defined Field 1";
            payment.Udf2 = "User Defined Field 2";
            payment.Udf3 = "User Defined Field 3";
            payment.Udf4 = "User Defined Field 4";
            payment.Udf5 = "User Defined Field 5";

            //Perform the payment initilization
         
            short TransVal;
            string varPaymentID, varPaymentPage, varErrorMsg, varRawResponse;

            TransVal = payment.PerformInitTransaction();  //return 0 for success otherwise -1 for failure
            varRawResponse = payment.RawResponse;
            varPaymentID = payment.PaymentId;
            varPaymentPage = payment.PaymentPage;
            varErrorMsg = payment.ErrorMsg;
            transaction.PaymentID = varPaymentID;
            
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            if (TransVal != 0)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "/Error");
            }
            else
            {
                workersFacade.CreateSalesOrder(transaction);
                return Request.CreateResponse(HttpStatusCode.OK, varPaymentPage + "?PaymentID=" + varPaymentID);
            }
            
        }

        [HttpPost]
        public HttpResponseMessage CreatePayment([FromBody]Transaction payment)
        {

            var result = workersFacade.SavePaymentDetails(payment);
            if (result)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Transaction failed!!!");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Transaction created successfully!!!");
        }

        [HttpPost]
        public IHttpActionResult GetAgentWorkers([FromBody]Worker worker)
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var cardCode = claims.Where(x => x.Type == worker.Agent).FirstOrDefault().Value;
            workersFacade.GetAgentWorkers(cardCode);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetWorkers([FromBody]Worker worker)
        { 
            var workers = workersFacade.GetWorkers();
            return Ok(workers);
        }

        [HttpPost]
        public IHttpActionResult AddWorker([FromBody]Worker worker)
        {
            var result = workersFacade.CreateWorker(worker);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult DeleteWorker(string code)
        {
            return Ok();
        }
    }
}