using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;
using Procons.Durrah.Common;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

[assembly: OwinStartup(typeof(Procons.Durrah.Startup))]
namespace Procons.Durrah
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);//USED TO ENABLE SESSION

            HttpConfiguration httpConfig = new HttpConfiguration();
            RouteCollection routes = new RouteCollection();

            ConfigureWebApi(httpConfig);

            ConfigureRoutes(RouteTable.Routes);
      
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }



        private void ConfigureWebApi(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ILoggingService, LogService>(new HierarchicalLifetimeManager());

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                                        name: "DefaultApi",
                                        routeTemplate: "api/{controller}/{action}/{id}",
                                        defaults: new { id = RouteParameter.Optional }
                                        );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private void ConfigureRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                    name: "Confirm",
                    url: "{controller}.mvc/{action}",
                    defaults: new { controller = "Main", action = "Confirm", id = UrlParameter.Optional }
    );
            routes.MapRoute(
                    name: "Default",
                    url: "{*anything}",
                    defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}