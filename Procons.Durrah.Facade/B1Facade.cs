using Procons.Durrah.Common;
using Procons.Durrah.Main;

namespace Procons.Durrah.Facade
{
    public class B1Facade
    {
        B1Provider provider { get { return Factory.DeclareClass<B1Provider>(); } }
        public void InitializeTables()
        {
            provider.InitializeTables();
        }
    }
}
