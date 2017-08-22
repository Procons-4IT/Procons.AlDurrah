namespace Procons.Durrah.Main
{
    using Common;
    using DataBaseHelper;
    using SAPbobsCOM;
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class WorkersProvider : ProviderBase
    {
        DatabaseHelper<SqlConnection> dbHelper = Factory.DeclareClass<DatabaseHelper<SqlConnection>>();
        public bool CreateWorker(Worker worker)
        {
            var conn = Factory.DeclareClass<DatabaseHelper<SqlConnection>>();
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

        public DbDataReader GetWorkers([Optional]string agent)
        {
            DbDataReader result = null;
            var query = string.Empty;// "SELECT * FROM [@Workers] WHERE \"U_Agent\"='{0}'";
            if (agent != null)
                query = "SELECT * FROM [@Workers] WHERE \"U_Agent\"='{0}'";
            else
                query = "SELECT * FROM [@Workers]";
            try
            {
                result = dbHelper.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        public double CreateSalesOrder(Transaction trans)
        {
            base.B1Company.StartTransaction();
              
            var oSales = base.B1Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
            oSales.CardCode = trans.CardCode;
            oSales.UserFields.Fields.Item("U_PaymentID").Value = trans.PaymentID;
            oSales.DocObjectCode = BoObjectTypes.oOrders;
            oSales.Lines.ItemCode = trans.Code;
            oSales.Lines.SerialNumbers.InternalSerialNumber = trans.SerialNumber;
            oSales.Lines.SerialNumbers.ManufacturerSerialNumber = trans.SerialNumber;
            int result = oSales.Add();
            if (result == 0)
            {
                return oSales.DocTotal;
            }
            else
            {
                var err = base.B1Company.GetLastErrorDescription();
                return 0;
            }
        }

        public void CreateIncomingPayment(Transaction trans)
        {
            try
            {
                if (base.B1Company.InTransaction)
                {
                }

                else
                    base.B1Company.StartTransaction();
                var salesOrder = GetSalesOrder(trans.PaymentID);

                Documents oDoc = base.B1Company.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                oDoc.CardCode = salesOrder.CardCode;
                oDoc.Lines.BaseEntry = salesOrder.Lines.DocEntry;
                oDoc.Lines.BaseLine = salesOrder.Lines.LineNum;          
                oDoc.Lines.BaseType = 17;
                oDoc.Lines.Quantity = 1;
                oDoc.Lines.UnitPrice = salesOrder.Lines.UnitPrice;
                oDoc.Lines.SerialNumbers.InternalSerialNumber=  salesOrder.Lines.SerialNumbers.InternalSerialNumber;
                oDoc.Lines.SerialNumbers.ManufacturerSerialNumber = salesOrder.Lines.SerialNumbers.ManufacturerSerialNumber;
                oDoc.Lines.SerialNumbers.SystemSerialNumber = salesOrder.Lines.SerialNumbers.SystemSerialNumber;

                int RetCode = oDoc.Add();
                if (RetCode != 0)
                {
                    if (base.B1Company.InTransaction)
                        base.B1Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    var err = base.B1Company.GetLastErrorDescription();
                }
                else
                {
                    var InvoiceNo = base.B1Company.GetNewObjectKey();
                    var oPay = base.B1Company.GetBusinessObject(BoObjectTypes.oIncomingPayments) as Payments;
                    oPay.CardCode = salesOrder.CardCode;
                    oPay.Invoices.DocEntry = Convert.ToInt32(InvoiceNo);
                    //if (trans.Amount != null)
                    //    oPay.CashSum = oDoc.DocTotal;
                    var test = oDoc.DocTotal;
                    oPay.CashSum = oDoc.Lines.UnitPrice * oDoc.Lines.Quantity;
                    int RetCode1 = oPay.Add();
                    if (RetCode1 != 0)
                    {
                        if (base.B1Company.InTransaction)
                            base.B1Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        var err = base.B1Company.GetLastErrorDescription();
                    }
                    else
                    {
                        if (base.B1Company.InTransaction)
                            base.B1Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Documents GetSalesOrder(string paymentId)
        {
            var salesOrder = base.B1Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
            var records = base.B1Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            records.DoQuery(string.Format("SELECT * FROM \"ORDR\" WHERE \"U_PaymentID\"='{0}'", paymentId));
            salesOrder.Browser.Recordset = records;
            return salesOrder;
        }

    }
}
