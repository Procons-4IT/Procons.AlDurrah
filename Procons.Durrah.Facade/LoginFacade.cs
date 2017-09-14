namespace Procons.Durrah.Facade
{
    using Microsoft.AspNet.Identity;
    using Procons.Durrah.Common;
    using Procons.Durrah.Main;
    using System.Collections.Generic;
    using System.Web;

    public class LoginFacade : IFacade
    {
        public LoginFacade(ILoggingService _loggingService)
        {
            LoggingService = (LogService)_loggingService;
        }
        LoginProvider provider { get { return new LoginProvider(LoggingService); } }

        public ApplicationUser FindUser(string userName, string password)
        {   
            return provider.FindUser(userName,  password);
        }

        public IdentityResult CreateUser(ApplicationUser user)
        {
            return provider.CreateUser(user);
        }

        public List<Worker> GetWorkers()
        {
            return provider.GetWorkers();
        }

        public bool ResetPassword(string pass,string validationId,string email)
        {
            return provider.ResetPassword(pass, validationId, email);
        }

        public string ResetRequest(string emailAddress)
        {
            return provider.CreatePasswordReset(emailAddress);
        }

        public string GenerateCofnirmationToken(string email)
        {
            return provider.GenerateConfirmationToken(email);
        }

        public bool ConfirmEmail(string validationId,string email)
        {
            return provider.ConfirmEmail(validationId, email);
        }
    }
}
