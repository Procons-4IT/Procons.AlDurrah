namespace Procons.Durrah.Facade
{
    using System.Collections.Generic;
    using Procons.Durrah.Common;
    using Procons.Durrah.Main;
    using System.Data;
    using Procons.DataBaseHelper;
    using System.Linq.Expressions;
    using Procons.Durrah.Common.Enumerators;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;

    public class WorkersFacade : IFacade
    {
        WorkersProvider provider { get { return Factory.DeclareClass<WorkersProvider>(); } }

        public bool CreateWorker(Worker worker)
        {
            return provider.CreateWorker(worker);
        }

        public bool DeleteWorker(string id)
        {
            return provider.DeleteWorker(id);
        }

        public List<Worker> GetWorkers(Catalogue wrk)
        {
            var workersList = provider.GetWorkers(wrk);
            //var workersList = MappingHelper.FillCollection<Worker>(dr);
            return workersList;
        }

        public double? CreateSalesOrder(Transaction transaction)
        {
            return provider.CreateSalesOrder(transaction);
        }

        public bool SavePaymentDetails(Transaction trans)
        {
           return provider.CreateIncomingPayment(trans);
        }

       public List<LookupItem> GetLanguagesLookups()
        {
            return provider.GetLookupValues<LANGUAGES>();
        }
        public List<LookupItem> GetMaritalStatusLookups()
        {
            return provider.GetLookupValues<MARITALSTATUS>();
        }
        public List<LookupItem> GetCountriesLookups()
        {
            return provider.GetLookupValues<COUNTRIES>();
        }
        public List<LookupItem> GetWorkersTypesLookups()
        {
            return provider.GetLookupValues<WORKERTYPES>();
        }
    }
}
