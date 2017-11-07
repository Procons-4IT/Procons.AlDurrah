using Procons.Durrah.Common;
using Procons.Durrah.Facade;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
            var cardCode = Request.Form["udf2"];
            var code = Request.Form["udf3"];
            var workerCode = Request.Form["udf4"];

            Response.Write($"REDIRECT={Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResultUrl)}?PaymentID={paymentID}&Result={ result }&PostDate={ postdate }&TranID={ tranid }&Auth={ auth }&Ref={refr }&TrackID={trackid}&Udf2={cardCode}&Udf3={code}&Udf4={workerCode}");
            return View();
        }

        public ActionResult Image(string path)
        {
            var image = System.Drawing.Image.FromFile(path);
            byte[] fileBytes = null;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                fileBytes = ms.ToArray();
                return base.File(fileBytes, "image/png");
            }
        }
    }
}