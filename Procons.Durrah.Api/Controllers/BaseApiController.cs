namespace Procons.Durrah.Controllers
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using System.Net.Http;
    using System.Web;
    using System.Linq;
    using System.Web.Http;
    using Procons.Durrah.Auth;
    using Procons.Durrah.Models;
    using System.Net;
    using System.IO;
    using Newtonsoft.Json;
    using Procons.Durrah.Common.Models;
    using Procons.Durrah.Common;
    using System.Security.Claims;
    using System;

    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        protected ILoggingService LoggingService { get; set; }
        protected EmailService emailService = new EmailService();

        protected IdentityMessage idMessage = new IdentityMessage();
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }

        public bool GoogleReCaptcha(string gRecaptchaResponse)
        {
            bool captchaResult = false;
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LdxFzAUAAAAAGS5UvbX5fr_fzl7712hlQ9yN4O7"; 
            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(result);
            if (captChaesponse.Success)
            {
                captchaResult = true;
            }
            return captchaResult;
        }

        protected string GetCurrentUserCardCode()
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var cardCode = string.Empty;
            if (claims.Count() != 0)
                cardCode = claims.Where(x => x.Type == Common.Constants.ServiceLayer.CardCode).FirstOrDefault().Value;
            return cardCode;
        }

        protected string GetCurrentUserEmail()
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var email = string.Empty;
            if (claims.Count() != 0)
                email = claims.Where(x => x.Type == Common.Constants.ServiceLayer.Email).FirstOrDefault().Value;
            return email;
        }

        protected T MapField<T>(object o)
        {
            var result = default(T);
            if (o != DBNull.Value)
            {
                if (o.GetType() == typeof(float))
                    result = (T)Convert.ChangeType(float.Parse(o.ToString()), typeof(T));
                else
                    result = (T)Convert.ChangeType(o, typeof(T));
                return result;
            }
            else
                return default(T);
        }
    }
}