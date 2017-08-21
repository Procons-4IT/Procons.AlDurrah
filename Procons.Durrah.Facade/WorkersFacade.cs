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

        public List<Worker> GetWorkers(string agent)
        {
            var dr = provider.GetWorkers(agent);
            var workersList = MappingHelper.FillCollection<Worker>(dr);
            return workersList;
        }

         public void CreateArInvoice(Transaction trans)
        {
            provider.CreateArInvoice(trans);
        }

        public void SubmitSalesOrder(string paymentId)
        {
            provider.CreateIncomingPayment(paymentId);
        }

        public void SavePaymentDetails(Transaction trans)
        {
            provider.CreateArInvoice(trans);
        }
    }
}
