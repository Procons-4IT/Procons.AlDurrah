using Procons.Durrah.Common;
using Procons.Durrah.Main;

namespace Procons.Durrah.Facade
{
    public class B1Facade : IFacade
    {
        B1Provider provider { get { return new B1Provider(LoggingService); } }

        public B1Facade()
        {
            LoggingService = new LogService();
        }
        public B1Facade(ILoggingService _loggingService)
        {
            LoggingService = (LogService)_loggingService;
        }
        public string InitializeTables()
        {
            return provider.InitializeTables();
        }
    }
}
