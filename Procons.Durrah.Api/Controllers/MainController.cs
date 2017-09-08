using Procons.Durrah.Common;
using Procons.Durrah.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Confirm()
        {
            var paymentID = Request.Form["paymentid"];
            var result = Request.Form["result"];
            var postdate = Request.Form["postdate"];
            var tranid = Request.Form["tranid"];
            var auth = Request.Form["auth"];
            var refr = Request.Form["ref"];
            var trackid = Request.Form["trackid"];
            Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResultUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}");
            return View();
        }
    }
}