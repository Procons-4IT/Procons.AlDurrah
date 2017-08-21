namespace Procons.Durrah.Main
{
    using Common;
    using DataBaseHelper;
    using SAPbobsCOM;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class WorkersProvider : ProviderBase
    {
        DatabaseHelper<SqlConnection> dbHelper = Factory.DeclareClass<DatabaseHelper<SqlConnection>>();
        public bool CreateWorker(Worker worker)
        {
            var conn = Factory.DeclareClass<DataBaseHelper.DatabaseHelper<SqlConnection>>();
            SqlTransaction transaction = null;
            var created = false;
            try
            {
                transaction = conn.Connection.BeginTransaction();

                //TODO: Create worker then create Goods Reciept PO, Check is worker available and update
                transaction.Commit();
                created = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return created;
        }

        public bool DeleteWorker(string id)
        {
            var conn = Factory.DeclareClass<DataBaseHelper.DatabaseHelper<SqlConnection>>();
            SqlTransaction transaction = null;
            var created = false;
            try
            {
                transaction = conn.Connection.BeginTransaction();
                //TODO: DELETE WORKER, GOODS ISSUE HAS TO BE CREATED 
                transaction.Commit();
                created = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return created;
        }

        public DbDataReader GetWorkers(string agent)
        {
            DbDataReader result = null;
            try
            {
                result = dbHelper.ExecuteQuery("SELECT * FROM [@Workers] WHERE \"U_Agent\"='{0}'");
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        public bool CreateArInvoice(Transaction trans)
        {
            base.B1Company.StartTransaction();
            var salesOrder = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices) as Documents;
            salesOrder.CardCode = trans.CardCode;
            salesOrder.UserFields.Fields.Item("U_PaymentID").Value = trans.PaymentID;
            //CONTINUE PAYMENT UDF's
            salesOrder.DocObjectCode = BoObjectTypes.oOrders;
            salesOrder.Lines.ItemCode = trans.Code;
            salesOrder.Lines.SerialNum = trans.SerialNumber;
           
            int result = salesOrder.Add();
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateIncomingPayment(string paymentId)
        {
            //TODO: SUBMIT B1 SALES ORDER AND UPDATE WORKER STATUS
        }

    }
}
