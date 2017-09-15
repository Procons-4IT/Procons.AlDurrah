using Procons.Durrah.Common;
using Procons.Durrah.Main;

namespace Procons.Durrah.Facade
{
    public class B1Facade : IFacade
    {
        B1Provider provider { get { return new B1Provider(); } }

        public string InitializeTables()
        {
            return provider.InitializeTables();
        }
    }
}
