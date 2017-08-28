namespace Procons.Durrah.Main
{
    using Common;
    using DataBaseHelper;
    using ExpressionBuilder.Common;
    using ExpressionBuilder.Generics;
    using ExpressionBuilder.Interfaces;
    using Procons.Durrah.Common.Enumerators;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using Sap.Data.Hana;
    using SAPbobsCOM;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Services.Client;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.InteropServices;
    using System.Web.Http;

    public class WorkersProvider : ProviderBase
    {
        DatabaseHelper<HanaConnection> dbHelper = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
        public bool CreateWorker(Worker worker)
        {
            var created = false;
            var _serviceInstance = ServiceLayerProvider.GetInstance();
            var _worker = new WORKERS();
            _worker.Code = Guid.NewGuid().ToString();
            _worker.U_Agent = worker.Agent;
            _worker.U_Age = Convert.ToInt32(worker.Age);
            _worker.U_BirthDate = Convert.ToDateTime(worker.BirthDate);
            _worker.U_CivilId = worker.CivilId;
            _worker.U_ItemCode = worker.Code;
            _worker.U_Education = worker.Education;
            _worker.U_Gender = worker.Gender;
            _worker.U_Height = worker.Height;
            _worker.U_Language = worker.Language;
            _worker.U_MaritalStatus = worker.MaritalStatus;
            _worker.U_Nationality = worker.Nationality;
            _worker.U_Passport = worker.Passport;
            _worker.U_PassportExpDate = worker.PassportExpDate;
            _worker.U_PassportPoIssue = worker.PassportIssDate;
            _worker.U_PassportNumber = worker.PassportNumber;
            _worker.U_PassportPoIssue = worker.PassportPoIssue;
            _worker.U_Photo = worker.Photo;
            _worker.U_Religion = worker.Religion;
            _worker.U_Serial = worker.SerialNumber;
            _worker.U_Status = worker.Status;
            _worker.U_Video = worker.Video;
            _worker.U_Weight = Convert.ToInt32(worker.Weight);

            try
            {
                _serviceInstance.CurrentServicelayerInstance.AddToWORKERSUDO(_worker);
                DataServiceResponse response = _serviceInstance.CurrentServicelayerInstance.SaveChanges();
                if (response != null)
                    created = true;
            }
            catch (Exception ex)
            {

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


        public List<Worker> GetAgentWorkers([Optional]string agent)
        {

            var ServiceInstance = ServiceLayerProvider.GetInstance();
            var workers = ServiceInstance.GetWorkers();
            List<Worker> workersList = new List<Worker>();
            foreach (var w in workers)
            {
                workersList.Add(
                    new Worker()
                    {
                        Agent = w.U_Agent,
                        BirthDate = w.U_BirthDate.ToString(),
                        CivilId = w.U_CivilId,
                        Code = w.U_ItemCode,
                        Education = w.U_Education,
                        Gender = w.U_Gender,
                        Height = w.U_Height,
                        Language = w.U_Language,
                        MaritalStatus = w.U_MaritalStatus,
                        Nationality = w.U_Nationality,
                        Passport = w.U_Passport,
                        PassportExpDate = w.U_PassportExpDate,
                        PassportIssDate = w.U_PassportPoIssue,
                        PassportNumber = w.U_PassportNumber,
                        PassportPoIssue = w.U_PassportPoIssue,
                        Photo = w.U_Photo,
                        Religion = w.U_Religion,
                        SerialNumber = w.U_Serial,
                        Status = w.U_Status,
                        Video = w.U_Video,
                        Weight = w.U_Weight.ToString()
                    }
                    );
            }
            return workersList;
        }

        public List<Worker> GetWorkers(Worker worker)
        {
            var exp = GetExpression(worker);
            var ServiceInstance = ServiceLayerProvider.GetInstance();
            var workers = new List<WORKERS>();
            if (exp != null)
                workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.Where(exp).ToList<WORKERS>();
            else
                workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.ToList<WORKERS>();

            List<Worker> workersList = new List<Worker>();
            foreach (var w in workers)
            {
                workersList.Add(
                    new Worker()
                    {
                        Agent = w.U_Agent,
                        Age = w.U_Age,
                        BirthDate = w.U_BirthDate.ToString(),
                        CivilId = w.U_CivilId,
                        Code = w.U_ItemCode,
                        Education = w.U_Education,
                        Gender = w.U_Gender,
                        Height = w.U_Height,
                        Language = w.U_Language,
                        MaritalStatus = w.U_MaritalStatus,
                        Nationality = w.U_Nationality,
                        Passport = w.U_Passport,
                        PassportExpDate = w.U_PassportExpDate,
                        PassportIssDate = w.U_PassportPoIssue,
                        PassportNumber = w.U_PassportNumber,
                        PassportPoIssue = w.U_PassportPoIssue,
                        Photo = w.U_Photo,
                        Religion = w.U_Religion,
                        SerialNumber = w.U_Serial,
                        Status = w.U_Status,
                        Video = w.U_Video,
                        Weight = w.U_Weight.ToString()
                    }
                    );
            }
            return workersList;
        }

        public Filter<WORKERS> GetExpression(Worker wrk)
        {
            Filter<WORKERS> filter = new Filter<WORKERS>();
            IFilterStatementConnection statementConection = null;
            //var builder = Factory.DeclareClass<ExpressionBuilder<WORKERS>>();
            //var b1Worker = Factory.DeclareClass<WORKERS>();

            if (wrk != null)
            {


                if (wrk.Age != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_Age", Operation.Between, wrk.Age, wrk.Age);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_Age", Operation.Between, wrk.Age, wrk.Age);
                    }
                }
                if (wrk.Gender != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_Gender", Operation.EqualTo, wrk.Gender);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_Gender", Operation.EqualTo, wrk.Gender);
                    }
                }
                if (wrk.Nationality != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_Nationality", Operation.EqualTo, wrk.Nationality);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_Nationality", Operation.EqualTo, wrk.Nationality);
                    }
                }
                if (wrk.MaritalStatus != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_MaritalStatus", Operation.EqualTo, wrk.MaritalStatus);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_MaritalStatus", Operation.EqualTo, wrk.MaritalStatus);
                    }
                }
                if (wrk.Code != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("Code", Operation.EqualTo, wrk.Code);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("Code", Operation.EqualTo, wrk.Code);
                    }
                }
                if (wrk.Language != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_Language", Operation.EqualTo, wrk.Language);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_Language", Operation.EqualTo, wrk.Language);
                    }
                }
                return filter;
            }
            else
                return null;
        }

        public double? CreateSalesOrder(Transaction trans)
        {
            var instance =  ServiceLayerProvider.GetInstance();
            Document salesOrder = new Document();
            DocumentLine salesOrderLine = new DocumentLine();
            SerialNumber dserialNum = new SerialNumber();

            double? returnResult = 0;
            try
            {
                var serialDetails = instance.CurrentServicelayerInstance.SerialNumberDetails.Where(x => x.SerialNumber == trans.SerialNumber && x.ItemCode == trans.Code).FirstOrDefault();
                salesOrder.CardCode = trans.CardCode;
                salesOrder.U_PaymentID = trans.PaymentID;
                salesOrder.U_Auth = trans.Auth;
                salesOrder.U_TrackID = trans.TrackID;
                salesOrder.U_Ref = trans.Ref;
                salesOrder.DocDueDate = DateTime.Now;

                salesOrderLine.ItemCode = trans.Code;
                dserialNum.SystemSerialNumber = serialDetails.SystemNumber;
                salesOrderLine.SerialNumbers.Add(dserialNum);
                salesOrder.DocumentLines.Add(salesOrderLine);

                instance.CurrentServicelayerInstance.AddToOrders(salesOrder);
                var resultOrder = instance.CurrentServicelayerInstance.SaveChanges();

                if (null != resultOrder)
                {
                    ChangeOperationResponse opRes = (ChangeOperationResponse)resultOrder.SingleOrDefault();
                    object retDoc = ((System.Data.Services.Client.EntityDescriptor)(opRes.Descriptor)).Entity;
                    if (null != retDoc)
                    {
                        returnResult = ((Document)retDoc).DocTotal;
                    }
                    else
                        returnResult = 0;
                }
            }
            catch (Exception ex)
            {
                instance.CurrentServicelayerInstance.Detach(salesOrder);
            }

            return returnResult;
        }

        public bool CreateIncomingPayment(Transaction trans)
        {
            bool creationResult = false;
            try
            {
                //ServiceLayerProvider instance = ServiceLayerProvider.GetInstance();


                base.B1Company.StartTransaction();
                var salesOrder = GetSalesOrder(trans.PaymentID);
                if (salesOrder != null)
                {
                    Documents oDoc = base.B1Company.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                    oDoc.CardCode = salesOrder.CardCode;
                    oDoc.Lines.BaseEntry = salesOrder.Lines.DocEntry;
                    oDoc.Lines.BaseLine = salesOrder.Lines.LineNum;
                    oDoc.Lines.BaseType = 17;
                    oDoc.Lines.Quantity = 1;
                    oDoc.Lines.UnitPrice = salesOrder.Lines.UnitPrice;
                    oDoc.Lines.SerialNumbers.InternalSerialNumber = salesOrder.Lines.SerialNumbers.InternalSerialNumber;
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
                            creationResult = true;
                        }
                    }
                }
                return creationResult;
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
