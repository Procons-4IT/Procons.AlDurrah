namespace Procons.Durrah.Facade
{
    using System.Collections.Generic;
    using Procons.Durrah.Common;
    using Procons.Durrah.Main;
    using System.Data;
    using Procons.DataBaseHelper;
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

        public List<Worker> GetAgentWorkers(string agent)
        {
            var workersList = provider.GetWorkers(agent);
            //var workersList = MappingHelper.FillCollection<Worker>(dr);
            return workersList;
        }

        public List<Worker> GetWorkers()
        {
            var workersList = provider.GetWorkers();
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
    }
}
