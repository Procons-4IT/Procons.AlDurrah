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
    using System.Data.Services.Client;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using SAPB1 = Procons.Durrah.Main.B1ServiceLayer.SAPB1;

    public class WorkersProvider : ProviderBase
    {
        public bool CreateWorkerOld(Worker worker)
        {
            var created = false;
            DataServiceResponse response = null;
            var _serviceInstance = ServiceLayerProvider.GetInstance();
            var _worker = new WORKERS();

            var fileName = CreateAttachment(@"c:\BoyumInstallerLog.txt");
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
            _worker.U_PassportExpDate = MapField<DateTime>( worker.PassportExpDate);
            _worker.U_PassportIssDate = MapField<DateTime>(worker.PassportIssDate);
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
                response = _serviceInstance.CurrentServicelayerInstance.SaveChanges();
                if (response != null)
                    created = true;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return created;
        }
        public bool CreateWorker(Worker worker)
        {
            var created = false;

            try
            {
                var attachmentPath = GetAttachmentPath();
                var passportCopy = worker.Passport != null && worker.Passport != string.Empty ? CreateAttachment(worker.Passport) : worker.Passport;
                var photo = worker.Photo != null && worker.Photo != string.Empty ? CreateAttachment(worker.Photo) : worker.Photo;

                var sCmp = B1Company.GetCompanyService();
                var oGeneralService = sCmp.GetGeneralService("WORKERSUDO");
                var oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData) as GeneralData;

                oGeneralData.SetProperty("Code", worker.PassportNumber);
                oGeneralData.SetProperty("U_Agent", worker.Agent);
                oGeneralData.SetProperty("U_Age", worker.Age);
                oGeneralData.SetProperty("U_BirthDate", worker.BirthDate);
                oGeneralData.SetProperty("U_CivilId", worker.CivilId);
                oGeneralData.SetProperty("U_ItemCode", worker.Code);
                oGeneralData.SetProperty("U_Education", worker.Education);
                oGeneralData.SetProperty("U_Gender", worker.Gender);
                oGeneralData.SetProperty("U_Height", worker.Height);
                oGeneralData.SetProperty("U_Language", worker.Language);
                oGeneralData.SetProperty("U_MaritalStatus", worker.MaritalStatus);
                oGeneralData.SetProperty("U_Nationality", worker.Nationality);
                oGeneralData.SetProperty("U_Passport", string.Concat(attachmentPath, passportCopy));
                oGeneralData.SetProperty("U_PassportPoIssue", worker.PassportIssDate);
                oGeneralData.SetProperty("U_PassportExpDate", worker.PassportExpDate);
                oGeneralData.SetProperty("U_PassportNumber", worker.PassportNumber);
                oGeneralData.SetProperty("U_Photo", string.Concat(attachmentPath, photo));
                oGeneralData.SetProperty("U_Religion", worker.Religion);
                oGeneralData.SetProperty("U_Serial", worker.SerialNumber);
                oGeneralData.SetProperty("U_Status", worker.Status);
                oGeneralData.SetProperty("U_Video", worker.Video);
                oGeneralData.SetProperty("U_Weight", worker.Weight);

                oGeneralService.Add(oGeneralData);
                created = true;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return created;
        }

        public bool UpdateWorker(Worker worker,string cardCode)
        {
            var created = false;

            try
            {
                var attachmentPath = GetAttachmentPath();
                var passportCopy = worker.Passport != null && worker.Passport != string.Empty ? CreateAttachment(worker.Passport) : worker.Passport;
                var photo = worker.Photo != null && worker.Photo != string.Empty ? CreateAttachment(worker.Photo) : worker.Photo;

                var sCmp = B1Company.GetCompanyService();
                var oGeneralService = sCmp.GetGeneralService("WORKERSUDO");

                GeneralDataParams oParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams) as GeneralDataParams;
                oParams.SetProperty("Code", worker.WorkerCode);
                var oGeneralData = oGeneralService.GetByParams(oParams);


                if (oGeneralData != null )
                {
                    if(oGeneralData.GetProperty("U_ItemCode").ToString()== "DW00002")
                    {
                        //oGeneralData.SetProperty("Code", Guid.NewGuid().ToString());
                        oGeneralData.SetProperty("U_Agent", "Wissam");
                        oGeneralData.SetProperty("U_Age", worker.Age);
                        oGeneralData.SetProperty("U_BirthDate", worker.BirthDate);
                        oGeneralData.SetProperty("U_CivilId", worker.CivilId);
                        oGeneralData.SetProperty("U_ItemCode", worker.Code);
                        oGeneralData.SetProperty("U_Education", worker.Education);
                        oGeneralData.SetProperty("U_Gender", worker.Gender);
                        oGeneralData.SetProperty("U_Height", worker.Height);
                        oGeneralData.SetProperty("U_Language", worker.Language);
                        oGeneralData.SetProperty("U_MaritalStatus", worker.MaritalStatus);
                        oGeneralData.SetProperty("U_Nationality", worker.Nationality);
                        oGeneralData.SetProperty("U_Passport", string.Concat(attachmentPath, passportCopy));
                        oGeneralData.SetProperty("U_PassportPoIssue", worker.PassportIssDate);
                        oGeneralData.SetProperty("U_PassportExpDate", worker.PassportExpDate);
                        oGeneralData.SetProperty("U_PassportNumber", worker.PassportNumber);
                        oGeneralData.SetProperty("U_Photo", string.Concat(attachmentPath, photo));
                        oGeneralData.SetProperty("U_Religion", worker.Religion);
                        oGeneralData.SetProperty("U_Serial", worker.SerialNumber);
                        oGeneralData.SetProperty("U_Status", worker.Status);
                        oGeneralData.SetProperty("U_Video", worker.Video);
                        oGeneralData.SetProperty("U_Weight", worker.Weight);

                        oGeneralService.Update(oGeneralData);
                        created = true;
                    }
                }
                else
                {
                    Utilities.LogException($"Agent dont have permissions to update worker: {worker.WorkerCode}");
                    created = false;
                }
                    
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return created;
        }

        public bool DeleteWorker(string id, string cardCode)
        {
            var worker = new WORKERS();
            var deleted = false;
            try
            {
                var instance = ServiceLayerProvider.GetInstance();
                worker = instance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.Code == id && x.U_Agent == cardCode).FirstOrDefault();
                if (worker != null)
                {
                    instance.CurrentServicelayerInstance.DeleteObject(worker);
                    instance.SaveChangesBatch();
                    deleted = true;
                }
            }
            catch (Exception ex)
            {
                instance.CurrentServicelayerInstance.Detach(worker);
                Utilities.LogException(ex);
            }
            return deleted;
        }

        public List<Worker> GetAgentWorkers(string agent)
        {
            var ServiceInstance = ServiceLayerProvider.GetInstance();
            var workers = new List<WORKERS>();

            workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.U_Agent == agent).ToList<WORKERS>();

            List<Worker> workersList = new List<Worker>();
            foreach (var w in workers)
            {
                workersList.Add(
                    new Worker()
                    {
                        Agent = w.U_Agent,
                        Age = MapField<int>(w.U_Age),
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
                        PassportExpDate = MapField<DateTime>(w.U_PassportExpDate).ToShortDateString(),
                        PassportIssDate = MapField<DateTime>(w.U_PassportIssDate).ToShortDateString(),
                        PassportNumber = w.U_PassportNumber,
                        PassportPoIssue = w.U_PassportPoIssue,
                        Photo = w.U_Photo,
                        Religion = w.U_Religion,
                        SerialNumber = w.U_Serial,
                        Status = w.U_Status,
                        Video = w.U_Video,
                        Price = MapField<int>(w.U_Price),
                        Weight = w.U_Weight.ToString()
                    }
                    );
            }
            return workersList;
        }

        public List<Worker> GetWorkers(Catalogue worker)
        {
            List<Worker> workersList = new List<Worker>();
            try
            {
                var exp = GetExpressionSql(worker);
                var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

                var query = new StringBuilder();
                query.Append(@"SELECT ""A"".""Code"",""A"".""Name"",""U_ItemCode"",""U_Serial"",""U_Agent"",""U_Age"",");
                query.Append(@"""U_BirthDate"",""U_Gender"",""D"".""Name"" AS ""U_Nationality"",""D"".""U_NAME"" AS ""U_Nationality_AR"",""R"".""Name"" AS ""U_Religion"",""R"".""U_NAME"" AS ""U_Religion_AR"",");
                query.Append(@"""C"".""Name"" AS ""U_Language"",""C"".""U_NAME"" AS ""U_Language_AR"",""U_Photo"",""U_Weight"",""U_Height"",""E"".""Name"" AS ""U_Education"",""E"".""U_NAME"" AS ""U_Education_AR"",");
                query.Append(@"""U_Passport"",""U_Video"",""U_PassportNumber"",""U_PassportIssDate"",""U_PassportExpDate"",""U_PassportPoIssue"",""U_Price"",""U_CivilId"",""U_Status"",");
                query.Append($@"""B"".""Name"" AS ""U_MaritalStatus"",""B"".""U_NAME"" AS ""U_MaritalStatus_AR""");
                query.Append($@" FROM ""{databaseBame}"".""@WORKERS"" as ""A""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@MARITALSTATUS"" AS ""B"" ON ""A"".""U_MaritalStatus"" = ""B"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@LANGUAGES"" AS ""C"" ON ""A"".""U_Language"" = ""C"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@COUNTRIES"" AS ""D"" ON ""A"".""U_Nationality"" = ""D"".""Code""");

                query.Append($@" INNER JOIN ""{databaseBame}"".""@RELIGION"" AS ""R"" ON ""A"".""U_Religion"" = ""R"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@EDUCATION"" AS ""E"" ON ""A"".""U_Education"" = ""E"".""Code""");
                query.Append($@"WHERE A.""U_Status"" = '1' ");

                query.Append(exp);
                var readerResult = dbHelper.ExecuteQuery(query.ToString());

                while (readerResult.Read())
                {

                    workersList.Add(
                        new Worker()
                        {
                            WorkerCode = MapField<string>(readerResult["Code"]),
                            Agent = MapField<string>(readerResult["U_Agent"]),
                            Age = MapField<int>(readerResult["U_Age"]),
                            BirthDate = MapField<DateTime>(readerResult["U_BirthDate"]).ToShortDateString(),
                            CivilId = MapField<string>(readerResult["U_CivilId"]),
                            Code = MapField<string>(readerResult["U_ItemCode"]),
                            Education = MapField<string>(readerResult[Utilities.GetLocalizedField("U_Education")]),
                            Gender = Utilities.GetResourceValue(MapField<string>(readerResult["U_Gender"])),
                            Height = MapField<string>(readerResult["U_Height"]),
                            Language = MapField<string>(readerResult[Utilities.GetLocalizedField("U_Language")]),
                            MaritalStatus = MapField<string>(readerResult[Utilities.GetLocalizedField("U_MaritalStatus")]),
                            Nationality = MapField<string>(readerResult[Utilities.GetLocalizedField("U_Nationality")]),
                            Passport = MapField<string>(readerResult["U_Passport"]),
                            PassportExpDate = MapField<DateTime>(readerResult["U_PassportExpDate"]).ToShortDateString(),
                            PassportIssDate = MapField<string>(readerResult["U_PassportIssDate"]),
                            PassportNumber = MapField<string>(readerResult["U_PassportNumber"]),
                            PassportPoIssue = MapField<string>(readerResult["U_PassportPoIssue"]),
                            Photo = Utilities.GetUrlFromLocalImage(MapField<string>(readerResult["U_Photo"])),
                            Religion = MapField<string>(readerResult[Utilities.GetLocalizedField("U_Religion")]),
                            SerialNumber = MapField<string>(readerResult["U_Serial"]),
                            Status = MapField<string>(readerResult["U_Status"]),
                            Video = MapField<string>(readerResult["U_Video"]),
                            Price = MapField<float>(readerResult["U_Price"]),
                            Weight = MapField<string>(readerResult["U_Weight"])
                        });
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return workersList;
        }

        public double? CreateSalesOrder(Transaction trans, string cardCode)
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
                salesOrder.U_WorkerID = trans.WorkerCode;
                salesOrder.DocDueDate = DateTime.Now;

                salesOrderLine.ItemCode = trans.Code;
                //dserialNum.SystemSerialNumber = serialDetails.SystemNumber;
                //salesOrderLine.SerialNumbers.Add(dserialNum);
                salesOrder.DocumentLines.Add(salesOrderLine);

                instance.CurrentServicelayerInstance.AddToOrders(salesOrder);
                var worker = instance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.U_Serial == trans.SerialNumber && x.U_ItemCode == trans.Code).FirstOrDefault();
                worker.U_Status = "2";
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
                Utilities.LogException(ex);
            }

            return returnResult;
        }

        public Transaction CreateIncomingPayment(Transaction trans)
        {

            Transaction creationResult = null;
            try
            {
                var paymentAmount = GetDownPaymentAmount();
                base.B1Company.StartTransaction();
                var salesOrder = GetSalesOrder(trans.PaymentID);
                if (salesOrder != null)
                {
                    salesOrder.UserFields.Fields.Item("U_Result").Value = trans.Result;
                    salesOrder.UserFields.Fields.Item("U_Auth").Value = trans.Auth;
                    salesOrder.UserFields.Fields.Item("U_TranID").Value = trans.TranID;
                    salesOrder.UserFields.Fields.Item("U_Ref").Value = trans.Ref;
                    if (salesOrder.Update() != 0)
                    {
                        base.B1Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        var err = base.B1Company.GetLastErrorDescription();
                        throw new Exception(err);
                    }

                    var itemnumber = salesOrder.Lines.LineNum;
                    var itemcode = salesOrder.Lines.ItemCode;

                    //Documents oDoc = base.B1Company.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                    //oDoc.CardCode = salesOrder.CardCode;
                    //oDoc.Lines.BaseEntry = salesOrder.Lines.DocEntry;
                    //oDoc.Lines.BaseLine = salesOrder.Lines.LineNum;
                    //oDoc.Lines.BaseType = 17;
                    //oDoc.Lines.Quantity = 1;
                    //oDoc.Lines.UnitPrice = salesOrder.Lines.UnitPrice;
                    //oDoc.Lines.SerialNumbers.InternalSerialNumber = salesOrder.Lines.SerialNumbers.InternalSerialNumber;
                    //oDoc.Lines.SerialNumbers.ManufacturerSerialNumber = salesOrder.Lines.SerialNumbers.ManufacturerSerialNumber;
                    //oDoc.Lines.SerialNumbers.SystemSerialNumber = salesOrder.Lines.SerialNumbers.SystemSerialNumber;
                    //int RetCode = oDoc.Add();

                    SAPbobsCOM.Documents oDownPay = base.B1Company.GetBusinessObject(BoObjectTypes.oDownPayments) as Documents;

                    oDownPay.CardCode = salesOrder.CardCode;
                    //oDownPay.DownPaymentPercentage = 100
                    oDownPay.DownPaymentType = SAPbobsCOM.DownPaymentTypeEnum.dptInvoice;
                    oDownPay.DocTotal = paymentAmount;
                    oDownPay.Lines.BaseType = 17;
                    oDownPay.Lines.BaseEntry = salesOrder.Lines.DocEntry;
                    oDownPay.Lines.BaseLine = salesOrder.Lines.LineNum;
                    oDownPay.Lines.UnitPrice = salesOrder.Lines.UnitPrice;
                    //oDownPay.Lines.SerialNumbers.InternalSerialNumber = salesOrder.Lines.SerialNumbers.InternalSerialNumber;
                    //oDownPay.Lines.SerialNumbers.ManufacturerSerialNumber = salesOrder.Lines.SerialNumbers.ManufacturerSerialNumber;
                    //oDownPay.Lines.SerialNumbers.SystemSerialNumber = salesOrder.Lines.SerialNumbers.SystemSerialNumber;

                    int RetCode = oDownPay.Add();


                    if (RetCode != 0)
                    {
                        if (base.B1Company.InTransaction)
                            base.B1Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        var err = base.B1Company.GetLastErrorDescription();
                        throw new Exception(err);
                    }
                    else
                    {
                        var InvoiceNo = base.B1Company.GetNewObjectKey();
                        var oPay = base.B1Company.GetBusinessObject(BoObjectTypes.oIncomingPayments) as Payments;
                    
                        trans.Amount = paymentAmount.ToString();
                        oPay.CardCode = salesOrder.CardCode;
                        oPay.Invoices.DocEntry = Convert.ToInt32(InvoiceNo);
                        oPay.Invoices.SumApplied = paymentAmount;
                        oPay.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                        oPay.DocDate = DateTime.Today;
                        //TODO: Replace bank account with cach
                        oPay.TransferSum = paymentAmount;
                        oPay.TransferAccount = "_SYS00000000035";

                        oPay.CashSum = paymentAmount;
                        int RetCode1 = oPay.Add();

                        if (RetCode1 != 0)
                        {
                            base.B1Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                            var err = base.B1Company.GetLastErrorDescription();
                            throw new Exception(err);
                        }
                        else
                        {
                            base.B1Company.EndTransaction(BoWfTransOpt.wf_Commit);
                            creationResult = trans;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return creationResult;
        }

        public Documents GetSalesOrder(string paymentId)
        {
            try
            {
                var salesOrder = base.B1Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
                var records = base.B1Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                records.DoQuery(string.Format("SELECT * FROM \"ORDR\" WHERE \"U_PaymentID\"='{0}'", paymentId));
                salesOrder.Browser.Recordset = records;
                return salesOrder;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return null;
            }
        }

        public List<LookupItem> GetLookupValues<T>()
        {
            var _serviceInstance = ServiceLayerProvider.GetInstance();


            if (typeof(T) == typeof(COUNTRIES))
            {
                //var query = from c in _serviceInstance.CurrentServicelayerInstance.COUNTRIESUDO
                //            select new ListItem(c.Code, c.Name, c.U_NAME);
                //var test = query.ToList();
                var results = _serviceInstance.CurrentServicelayerInstance.COUNTRIESUDO.ToList();
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
            else if (typeof(T) == typeof(Item))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.Items.Select(x => new LookupItem(x.ItemName, x.ItemCode)).ToList();
                return results;
            }
            else
                return null;

            List<LookupItem> GetLookups<Y>(List<Y> list)
            {
                var lookups = new List<LookupItem>();
                foreach (dynamic r in list)
                {
                    if (Utilities.GetCurrentLanguage() == Languages.English.GetDescription())
                        lookups.Add(new LookupItem(r.Name, r.Code));
                    else
                        lookups.Add(new LookupItem(r.U_NAME, r.Code));
                }
                return lookups;
            }

            List<LookupItem> GetLookupsNew<Y>(List<Y> list)
            {
                var lookups = new List<LookupItem>();
                foreach (dynamic r in list)
                {
                    if (Utilities.GetCurrentLanguage() == Languages.English.GetDescription())
                        lookups.Add(new LookupItem(r.Name, r.Code));
                    else
                        lookups.Add(new LookupItem(r.U_NAME, r.Code));
                }
                return lookups;
            }
        }

        public double GetDownPaymentAmount()
        {
            double paymentAmount = 0;
            try
            {
                var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();

                var result = conn.ExecuteQuery($@"SELECT IFNULL(""U_DownPay"",0) AS ""U_DownPay"" FROM ""{base.databaseName}"".""OADM""");
                while (result.Read())
                {
                    paymentAmount = MapField<double>(result["U_DownPay"]);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return paymentAmount;
        }

        public string GetItemCodeByPaymentId(string paymentId)
        {
            string itemCode = string.Empty;
            try
            {
                var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
                StringBuilder query = new StringBuilder($@" SELECT IFNULL(""ItemCode"",'') AS ""ItemCode""");
                query.Append($@" FROM ""{base.databaseName}"".""RDR1"" T INNER JOIN ""{base.databaseName}"".""ORDR"" T1 ON T.""DocEntry"" = T1.""DocEntry""");
                query.Append($@" WHERE ""U_PaymentID"" = '{paymentId}'");

                var result = conn.ExecuteQuery(query.ToString());
                while (result.Read())
                {
                    itemCode = MapField<string>(result["ItemCode"]);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return itemCode;
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

        private string GetExpressionSqlOld(Catalogue wrk)
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

        private string GetExpressionSql(Catalogue wrk)
        {
            StringBuilder queryBuilder = new StringBuilder();
            if (wrk != null)
            {
                if (wrk.Age != null)
                {
                    var boundaries = wrk.Age.Trim().Split('-');
                    queryBuilder.Append($" AND \"U_Age\" BETWEEN {boundaries[0]} AND {boundaries[1]}");
                }
                if (wrk.Gender != null)
                {
                    queryBuilder.Append($" AND \"U_Gender\" = '{wrk.Gender}'");
                }
                if (wrk.Nationality != null)
                {
                    queryBuilder.Append($" AND \"U_Nationality\" = '{wrk.Nationality}'");
                }
                if (wrk.MaritalStatus != null)
                {
                    queryBuilder.Append($" AND \"U_MaritalStatus\" = '{wrk.MaritalStatus}'");
                }
                if (wrk.WorkerType != null)
                {
                    queryBuilder.Append($" AND \"U_ItemCode\" = '{wrk.WorkerType}'");
                }
                if (wrk.Language != null)
                {
                    queryBuilder.Append($" AND \"U_Language\" = '{wrk.Language}'");
                }
                return queryBuilder.ToString();
            }
            else
                return null;
        }

        private string CreateAttachment(string fileName)
        {
            SAPbobsCOM.Attachments2 oATT = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2) as SAPbobsCOM.Attachments2;
            string FileName = fileName;
            oATT.Lines.Add();
            oATT.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
            oATT.Lines.FileExtension = System.IO.Path.GetExtension(FileName).Substring(1);
            oATT.Lines.SourcePath = Path.GetDirectoryName(System.Web.Hosting.HostingEnvironment.MapPath(FileName));// System.IO.Path.GetDirectoryName(FileName);
            oATT.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
            if (oATT.Add() == 0)
                return System.IO.Path.GetFileName(fileName);
            else
            {
                var error = B1Company.GetLastErrorDescription();
                throw new Exception(error);
            }
        }

        private string GetAttachmentPath()
        {
            try
            {
                SAPbobsCOM.Recordset rec = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                rec.DoQuery(@"SELECT IFNULL(""AttachPath"",'') FROM ""OADP""");
                var path = rec.Fields.Item(0).Value.ToString();
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateGoodsReceipt(string itemCode, double totalCost, DateTime taxDate, DateTime refDate, string memo)
        {
            var goodsReceipt = B1Company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry) as Documents;
            var oItem = B1Company.GetBusinessObject(BoObjectTypes.oItems) as Items;
            try
            {
                goodsReceipt.DocType = BoDocumentTypes.dDocument_Items;
                goodsReceipt.DocDate = refDate;
                goodsReceipt.TaxDate = taxDate;
                goodsReceipt.Comments = base.MapField<string>(memo);
                goodsReceipt.Lines.ItemCode = itemCode;
                goodsReceipt.Lines.Quantity = 1;
                goodsReceipt.Lines.SerialNum = "Abx";
                oItem.GetByKey(itemCode);

                //if (oItem.InventoryUOM != uomName)
                //{
                //    goodsReceipt.Lines.UseBaseUnits = SAPbobsCOM.BoYesNoEnum.tNO;
                //    goodsReceipt.Lines.MeasureUnit = uomName;
                //}
                goodsReceipt.Lines.UnitPrice = totalCost;
                //goodsReceipt.Reference2 = reference;

                if (goodsReceipt.Add() != 0)
                {
                    var error = B1Company.GetLastErrorDescription();
                    throw new Exception(error);
                }
                else
                {
                    return goodsReceipt.DocEntry;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return 0;
            }
            finally
            {
                goodsReceipt.ReleaseObject();
                oItem.ReleaseObject();
            }
        }


        #endregion
    }

    public class ListItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_Name { get; set; }

        public ListItem(string code,string english,string arabic)
        {
            Code = code;
            Name = english;
            U_Name = arabic;
        }
    }
}
