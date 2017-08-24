using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Procons.Durrah.Auth;
using Procons.Durrah.Common;
using Procons.Durrah.Facade;
using Procons.Durrah.Providers;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
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

            ConfigureRoutes(RouteTable.Routes);
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
          
            ConfigureWebApi(httpConfig);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            //var AuthFacade = Factory.DeclareClass<AuthenticationManagementFacade>();
            //AuthFacade.CreatePerOwinContext(app);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApplicationOAuthProvider(),
                AccessTokenFormat = new ApplicationJwtFormat("self")
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "self";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            //config.Filters.Add(new ApplicationExceptionFilterAttribute());
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
                name: "Default",
                url: "{*anything}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}