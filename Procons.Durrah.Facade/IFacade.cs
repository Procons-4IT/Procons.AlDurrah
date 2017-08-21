using Procons.Durrah.Main;

namespace Procons.Durrah.Facade
{

    public abstract class IFacade
    {

        protected Repository<ApplicationContext> RepositoryInstance { get { return new Repository<ApplicationContext>(); } }
    }
}
