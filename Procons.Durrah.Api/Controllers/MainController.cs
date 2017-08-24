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
        // GET: Main
        public ActionResult Index()
        {
            B1Facade b1Facade = Factory.DeclareClass<B1Facade>();
            b1Facade.InitializeTables();
            return View();
        }
    }
}