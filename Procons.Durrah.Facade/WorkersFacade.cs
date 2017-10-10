namespace Procons.Durrah.Facade
{
    using Procons.Durrah.Common;
    using Procons.Durrah.Main;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using System.Collections.Generic;
    using System.Net.Http;

    public class WorkersFacade : IFacade
    {
        WorkersProvider provider { get { return new WorkersProvider(); } }

        public bool CreateWorker(Worker worker)
        {
            return provider.CreateWorker(worker);
        }

        public bool UpdateWorker(Worker worker)
        {
            return provider.UpdateWorker(worker);
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

        public double? CreateSalesOrder(Transaction transaction, string cardCode)
        {
            return provider.CreateSalesOrder(transaction, cardCode);
        }

        public Transaction SavePaymentDetails(Transaction trans)
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
            return provider.GetLookupValues<Item>();
        }
        public double GetDownPaymentAmount()
        {
            return provider.GetDownPaymentAmount();
        }

        public string GetItemCodeByPaymentId(string PaymentId)
        {
            return provider.GetItemCodeByPaymentId(PaymentId);
        }

        #region Private Methods



        #endregion
    }
}