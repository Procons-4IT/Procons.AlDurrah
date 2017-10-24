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

        public bool UpdateWorker(Worker worker,string cardCode)
        {
            return provider.UpdateWorker(worker, cardCode);
        }

        public bool DeleteWorker(string id, string cardCode)
        {
            return provider.DeleteWorker(id, cardCode);
        }

        public List<Worker> GetWorkers(Catalogue wrk, string requestUrl)
        {
            var workersList = provider.GetWorkers(wrk, requestUrl);
            return workersList;
        }

        public List<Worker> GetAgentWorkers(string agent,string requestUrl)
        {
            var workersList = provider.GetAgentWorkers(agent, requestUrl);
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
        public List<LookupItem> GetEducationLookups()
        {
            return provider.GetLookupValues<EDUCATION>();
        }
        public List<LookupItem> GetReligionLookups()
        {
            return provider.GetLookupValues<RELIGION>();
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

        public string GetAttachmentsPath()
        {
            return provider.GetAttachmentPath();
        }

        #region Private Methods



        #endregion
    }
}