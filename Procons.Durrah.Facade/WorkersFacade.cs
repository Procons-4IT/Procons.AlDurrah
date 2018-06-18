namespace Procons.Durrah.Facade
{
    using Procons.Durrah.Common;
    using Procons.Durrah.Common.Enumerators;
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

        public List<Worker> GetWorkers(Catalogue wrk, string requestUrl, WorkerStatus status)
        {
            var workersList = provider.GetWorkers(wrk, requestUrl, status);
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

        public string GetEmailAddress(string cardCode)
        {
            return provider.GetEmailAddress(cardCode);
        }
        public ServiceLayerProvider GetServiceInstance()
        {
          return  provider.GetServiceInstance();
        }
        public List<List<LookupItem>> GetAllLookupsForEmployeeDataSearch()
        {
            var _serviceInstance = GetServiceInstance();
            List<List<LookupItem>> AllLookups = new List<List<LookupItem>>();
            var LANGUAGESItem = provider.GetLookupValues<LANGUAGES>(_serviceInstance);
            var COUNTRIESItem = provider.GetLookupValues<COUNTRIES>(_serviceInstance);
            var RELIGIONItem = provider.GetLookupValues<RELIGION>(_serviceInstance);
            var EDUCATIONItem = provider.GetLookupValues<EDUCATION>(_serviceInstance);
            var MARITALSTATUSItem = provider.GetLookupValues<MARITALSTATUS>(_serviceInstance);
            var WorkersTypesItem = provider.GetLookupValues<Item>(_serviceInstance);
            var CountryItem = provider.GetLookupValues<Country>(_serviceInstance);
            AllLookups.Add(LANGUAGESItem);
            AllLookups.Add(COUNTRIESItem);
            AllLookups.Add(RELIGIONItem);
            AllLookups.Add(EDUCATIONItem);
            AllLookups.Add(MARITALSTATUSItem);
            AllLookups.Add(WorkersTypesItem);
            AllLookups.Add(CountryItem);
            return AllLookups;
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
        public List<LookupItem> GetAllCountriesLookups()
        {
            return provider.GetLookupValues<Country>();
        }
        public List<LookupItem> GetItemsByWorkerType(string workerType)
        {
            return provider.GetItemsByWorkerType(workerType);
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

        public bool CheckSalesOderAvailability(string paymentId)
        {
            return provider.CheckSalesOrderAvailability(paymentId);
        }

        public List<LookupItem> GetLanguagesById(string isonStringIds)
        {
            return provider.GetLanguagesById(isonStringIds);
        }

        public void CancelSalesOrder(Transaction payment)
        {
            provider.CancelSalesOrder( payment);
        }

        #region Private Methods



        #endregion
    }
}