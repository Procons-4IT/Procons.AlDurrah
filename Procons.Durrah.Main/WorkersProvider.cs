namespace Procons.Durrah.Main
{
    using Common;
    using DataBaseHelper;
    using ExpressionBuilder.Common;
    using ExpressionBuilder.Generics;
    using ExpressionBuilder.Interfaces;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using Sap.Data.Hana;
    using SAPbobsCOM;
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;


    public class WorkersProvider : ProviderBase
    {
        public bool CreateWorker(Worker worker)
        {
            var created = false;
            var _serviceInstance = ServiceLayerProvider.GetInstance();
            var _worker = new WORKERS();
            _worker.Code = Guid.NewGuid().ToString();
            _worker.U_Agent = worker.Agent;
            _worker.U_Age = worker.Age;
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
            _worker.U_Price = worker.Price;
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

        public List<Worker> GetWorkersOld(Catalogue worker)
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
                        //Agent = w.U_Agent,
                        //Age = w.U_Age,
                        //BirthDate = w.U_BirthDate.ToString(),
                        //CivilId = w.U_CivilId,
                        //Code = w.U_ItemCode,
                        //Education = w.U_Education,
                        //Gender = w.U_Gender,
                        //Height = w.U_Height,
                        //Language = w.U_Language,
                        //MaritalStatus = w.U_MaritalStatus,
                        //Nationality = w.U_Nationality,
                        //Passport = w.U_Passport,
                        //PassportExpDate = w.U_PassportExpDate,
                        //PassportIssDate = w.U_PassportPoIssue,
                        //PassportNumber = w.U_PassportNumber,
                        //PassportPoIssue = w.U_PassportPoIssue,
                        //Photo = w.U_Photo,
                        //Religion = w.U_Religion,
                        //SerialNumber = w.U_Serial,
                        //Status = w.U_Status,
                        //Video = w.U_Video,
                        //Price = w.U_Price,
                        //Weight = w.U_Weight.ToString()
                    }
                    );
            }
            return workersList;
        }

        public List<Worker> GetWorkers(Catalogue worker)
        {
            var exp = GetExpressionSql(worker);
            var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

            List<Worker> workersList = new List<Worker>();
            var query = new StringBuilder("SELECT \"A\".\"Code\",\"A\".\"Name\",\"U_ItemCode\",\"U_Serial\",\"U_Agent\",\"U_Age\",\"U_BirthDate\",\"U_Gender\",\"D\".\"Name\" AS \"U_Nationality\",\"R\".\"Name\" AS \"U_Religion\",\"C\".\"Name\" AS \"U_Language\",\"U_Photo\",\"U_Weight\",\"U_Height\",\"E\".\"Name\" AS \"U_Education\",\"U_Passport\",\"U_Video\",\"U_PassportNumber\",\"U_PassportIssDate\",\"U_PassportExpDate\",\"U_PassportPoIssue\",\"U_Price\",\"U_CivilId\",\"U_Status\",");
            query.Append($"\"B\".\"Name\" AS \"U_MaritalStatus\"");
            query.Append($" FROM \"{databaseBame}\".\"@WORKERS\" as \"A\"");
            query.Append($"INNER JOIN \"{databaseBame}\".\"@MARITALSTATUS\" AS \"B\" ON \"A\".\"U_MaritalStatus\" = \"B\".\"Code\"");
            query.Append($"INNER JOIN \"{databaseBame}\".\"@LANGUAGES\" AS \"C\" ON \"A\".\"U_Language\" = \"C\".\"Code\"");
            query.Append($"INNER JOIN \"{databaseBame}\".\"@COUNTRIES\" AS \"D\" ON \"A\".\"U_Nationality\" = \"D\".\"Code\"");

            query.Append($"INNER JOIN \"{databaseBame}\".\"@RELIGION\" AS \"R\" ON \"A\".\"U_Religion\" = \"R\".\"Code\"");
            query.Append($"INNER JOIN \"{databaseBame}\".\"@EDUCATION\" AS \"E\" ON \"A\".\"U_Education\" = \"E\".\"Code\"");

            query.Append(exp);
            var readerResult = dbHelper.ExecuteQuery(query.ToString());

            while (readerResult.Read())
            {
                workersList.Add(
                    new Worker()
                    {
                        Agent = MapField<string>(readerResult["U_Agent"]),
                        Age = MapField<int>(readerResult["U_Age"]),
                        BirthDate = MapField<string>(readerResult["U_BirthDate"]),
                        CivilId = MapField<string>(readerResult["U_CivilId"]),
                        Code = MapField<string>(readerResult["U_ItemCode"]),
                        Education = MapField<string>(readerResult["U_Education"]),
                        Gender = MapField<string>(readerResult["U_Gender"]),
                        Height = MapField<string>(readerResult["U_Height"]),
                        Language = MapField<string>(readerResult["U_Language"]),
                        MaritalStatus = MapField<string>(readerResult["U_MaritalStatus"]),
                        Nationality = MapField<string>(readerResult["U_Nationality"]),
                        Passport = MapField<string>(readerResult["U_Passport"]),
                        PassportExpDate = MapField<string>(readerResult["U_PassportExpDate"]),
                        PassportIssDate = MapField<string>(readerResult["U_PassportIssDate"]),
                        PassportNumber = MapField<string>(readerResult["U_PassportNumber"]),
                        PassportPoIssue = MapField<string>(readerResult["U_PassportPoIssue"]),
                        Photo = MapField<string>(readerResult["U_Photo"]),
                        Religion = MapField<string>(readerResult["U_Religion"]),
                        SerialNumber = MapField<string>(readerResult["U_Serial"]),
                        Status = MapField<string>(readerResult["U_Status"]),
                        Video = MapField<string>(readerResult["U_Video"]),
                        Price = MapField<float>(readerResult["U_Price"]),
                        Weight = MapField<string>(readerResult["U_Weight"])
                    });
            }
            return workersList;
        }

        public double? CreateSalesOrder(Transaction trans,string cardCode)
        {
            var instance = ServiceLayerProvider.GetInstance();
            Document salesOrder = new Document();
            DocumentLine salesOrderLine = new DocumentLine();
            SerialNumber dserialNum = new SerialNumber();

            double? returnResult = double.MinValue;
            try
            {
                var serialDetails = instance.CurrentServicelayerInstance.SerialNumberDetails.Where(x => x.SerialNumber == trans.SerialNumber && x.ItemCode == trans.Code).FirstOrDefault();
                salesOrder.CardCode = cardCode;
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
                //UPDATE WORKER STATUS
                var worker = instance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.U_Serial == trans.SerialNumber && x.U_ItemCode == trans.Code).FirstOrDefault();
                worker.U_Status = "0";
                instance.CurrentServicelayerInstance.UpdateObject(worker);
               var resultOrder = instance.CurrentServicelayerInstance.SaveChanges();

                if (null != resultOrder)
                {
                    ChangeOperationResponse opRes = (ChangeOperationResponse)resultOrder.FirstOrDefault();
                    object retDoc = ((System.Data.Services.Client.EntityDescriptor)(opRes.Descriptor)).Entity;
                    if (null != retDoc)
                        returnResult = ((Document)retDoc).DocTotal;
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
                        //oPay.CashSum = oDoc.Lines.UnitPrice * oDoc.Lines.Quantity;
                        var paymentAmount = GetDownPaymentAmount();
                        oPay.CashSum = paymentAmount;
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

        public List<LookupItem> GetLookupValues<T>()
        {
            var _serviceInstance = ServiceLayerProvider.GetInstance();


            if (typeof(T) == typeof(COUNTRIES))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.COUNTRIESUDO.ToList<COUNTRIES>();
                return GetLookups<COUNTRIES>(results);
            }
            else if (typeof(T) == typeof(LANGUAGES))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.LANGUAGESUDO.ToList<LANGUAGES>();
                return GetLookups<LANGUAGES>(results);
            }
            else if (typeof(T) == typeof(MARITALSTATUS))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.MARITALSTATUSUDO.ToList<MARITALSTATUS>();
                return GetLookups<MARITALSTATUS>(results);
            }
            else if (typeof(T) == typeof(WORKERTYPES))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.WORKERTYPESUDO.ToList<WORKERTYPES>();
                return GetLookups<WORKERTYPES>(results);
            }
            else
                return null;

            List<LookupItem> GetLookups<Y>(List<Y> list)
            {
                var lookups = new List<LookupItem>();
                foreach (dynamic r in list)
                {
                    lookups.Add(new LookupItem(r.Name, r.Code));
                }
                return lookups;
            }
        }

        public double GetDownPaymentAmount()
        {
            var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
            double paymentAmount = 0;
            var result = conn.ExecuteQuery($@"SELECT IFNULL(""U_DownPay"",0) FROM ""{base.databaseName}"".""OADM""");
            while (result.Read())
            {
                paymentAmount = MapField<double>(result["U_DownPay"]);
            }
            return paymentAmount;
        }

        #region PRIVATE METHODS
        private Filter<WORKERS> GetExpression(Catalogue wrk)
        {
            Filter<WORKERS> filter = new Filter<WORKERS>();
            IFilterStatementConnection statementConection = null;
            if (wrk != null)
            {


                if (wrk.Age != null)
                {
                    var boundaries = wrk.Age.Trim().Split('-');
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("U_Age", Operation.Between, Convert.ToInt32(boundaries[0]), Convert.ToInt32(boundaries[1]));
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("U_Age", Operation.Between, Convert.ToInt32(boundaries[0]), Convert.ToInt32(boundaries[1]));
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
                if (wrk.WorkerType != null)
                {
                    if (filter.Statements.Count() > 0)
                        statementConection = statementConection.And.By("Code", Operation.EqualTo, wrk.WorkerType);
                    else
                    {
                        filter = new Filter<WORKERS>();
                        statementConection = filter.By("Code", Operation.EqualTo, wrk.WorkerType);
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

        private string GetExpressionSql(Catalogue wrk)
        {
            StringBuilder queryBuilder = new StringBuilder();
            if (wrk != null)
            {
                if (wrk.Age != null)
                {
                    var boundaries = wrk.Age.Trim().Split('-');
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_Age\" BETWEEN {boundaries[0]} AND {boundaries[1]}");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_Age\" BETWEEN {boundaries[0]} AND {boundaries[1]}");
                    }
                }
                if (wrk.Gender != null)
                {
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_Gender\" = '{wrk.Gender}'");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_Gender\" = '{wrk.Gender}'");
                    }
                }
                if (wrk.Nationality != null)
                {
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_Nationality\" = '{wrk.Nationality}'");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_Nationality\" = '{wrk.Nationality}'");
                    }
                }
                if (wrk.MaritalStatus != null)
                {
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_MaritalStatus\" = '{wrk.MaritalStatus}'");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_MaritalStatus\" = '{wrk.MaritalStatus}'");
                    }
                }
                if (wrk.WorkerType != null)
                {
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_ItemCode\" = '{wrk.WorkerType}'");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_ItemCode\" = '{wrk.WorkerType}'");
                    }
                }
                if (wrk.Language != null)
                {
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_Language\" = '{wrk.Language}'");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_Language\" = '{wrk.Language}'");
                    }
                }
                return queryBuilder.ToString();
            }
            else
                return null;
        }

        private T MapField<T>(object o)
        {
            var result = default(T);
            if (o != DBNull.Value)
            {
                if (o.GetType() == typeof(HanaDecimal))
                    result = (T)Convert.ChangeType(Convert.ToDecimal(o), typeof(T));
                else if (o.GetType() == typeof(float))
                    result = (T)Convert.ChangeType(float.Parse(o.ToString()), typeof(T));
                else
                    result = (T)Convert.ChangeType(o, typeof(T));
                return result;
            }
            else
                return default(T);
        }

        #endregion
    }
}
