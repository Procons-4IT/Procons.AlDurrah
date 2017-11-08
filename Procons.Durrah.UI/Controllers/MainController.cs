using Procons.Durrah.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Procons.Durrah.Api.Controllers
{
    public class MainController : Controller
    {
        public MainController()
        {

        }

        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Confirm()
        {
            try
            {
                var paymentID = Request.Form["paymentid"];
                var result = Request.Form["result"];
                var postdate = Request.Form["postdate"];
                var tranid = Request.Form["tranid"];
                var auth = Request.Form["auth"];
                var refr = Request.Form["ref"];
                var trackid = Request.Form["trackid"];
                var cardCode = Request.Form["udf2"];
                var code = Request.Form["udf3"];
                var workerCode = Request.Form["udf4"];
                var amount = Request.Form["udf5"];

                Transaction trans = new Transaction() { PaymentID = paymentID, Result = result, PostDate = postdate, TranID = tranid, Auth = auth, Ref = refr, TrackID = trackid, CardCode = cardCode, Code = code, WorkerCode = workerCode };

                if (result == "CAPTURED")
                {
                    var paymentResult = await CallPayment(trans);
                    if (paymentResult)
                        Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResultUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}&Udf2={cardCode}&Udf3={code}&Udf4={workerCode}&Udf5={amount}");
                    else
                        Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ErrorUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}&Udf2={cardCode}&Udf3={code}&Udf4={workerCode}&Udf5={amount}");
                }
                else if (result == "CANCELED")
                    Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.CancelUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}&Udf2={cardCode}&Udf3={code}&Udf4={workerCode}&Udf5={amount}");
                else
                    Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ErrorUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}&Udf2={cardCode}&Udf3={code}&Udf4={workerCode}&Udf5={amount}");
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return View();
        }

        public async Task<bool> CallPayment(Transaction trans)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsJsonAsync($"{Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ApiBase)}/api/Workers/CreatePayment", trans);
            }
            return response.IsSuccessStatusCode;
        }
    }
}