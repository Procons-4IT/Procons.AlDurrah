namespace Procons.Durrah.Facade
{
    using Procons.Durrah.Common;
    using Procons.Durrah.Main;
    using System.Collections.Generic;
    using System.Web;

    public class LoginFacade : IFacade
    {
        LoginProvider provider { get { return Factory.DeclareClass<LoginProvider>(); } }

        public ApplicationUser FindUser(string userName, string password)
        {   
            return provider.FindUser(userName,  password);
        }

        public List<Worker> GetWorkers()
        {
            return provider.GetWorkers();
        }
    }
}
