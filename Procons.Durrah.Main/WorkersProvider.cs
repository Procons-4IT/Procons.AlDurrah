namespace Procons.Durrah.Main
{
    using Common;
    using DataBaseHelper;
    using ExpressionBuilder.Common;
    using ExpressionBuilder.Generics;
    using ExpressionBuilder.Interfaces;
    using Procons.Durrah.Common.Enumerators;
    using Procons.Durrah.Common.Models;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using Sap.Data.Hana;
    using SAPbobsCOM;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class WorkersProvider : ProviderBase
    {
        public bool CreateWorker(Worker worker)
        {
            var created = false;

            if (IsWorkerAvailable(worker.WorkerCode))
                return false;
            var attachmentPath = GetAttachmentPath();
            var passportCopy = worker.Passport != null && worker.Passport != string.Empty ? CreateAttachment(worker.Passport) : worker.Passport;
            var photo = worker.Photo != null && worker.Photo != string.Empty ? CreateAttachment(worker.Photo) : worker.Photo;
            var license = worker.License != null && worker.License != string.Empty ? CreateAttachment(worker.License) : worker.License;

            var sCmp = B1Company.GetCompanyService();
            var oGeneralService = sCmp.GetGeneralService("WORKERSUDO");
            var oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData) as GeneralData;

            var gDataCollection = oGeneralData.Child("WORKERLNGS");
            if (worker.Languages != null)
            {
                foreach (var l in worker.Languages)
                {
                    var newLanguage = gDataCollection.Add();
                    newLanguage.SetProperty("U_NAME", l.Name);
                    newLanguage.SetProperty("U_VALUE", l.Value);
                }
            }


            if (worker.Experiences != null)
            {
                var expDataCollection = oGeneralData.Child("EXPERIENCE");
                foreach (var e in worker.Experiences)
                {
                    var newExperience = expDataCollection.Add();
                    newExperience.SetProperty("U_WorkerID", worker.PassportNumber);
                    if (e.StartDate != null)
                        newExperience.SetProperty("U_StartDate", e.StartDate);
                    if (e.EndDate != null)
                        newExperience.SetProperty("U_EndDate", e.EndDate);
                    newExperience.SetProperty("U_Title", e.Title);
                    newExperience.SetProperty("U_Description", e.Description);
                    newExperience.SetProperty("U_CompanyName", e.CompanyName);
                    newExperience.SetProperty("U_Location", e.Location);
                    newExperience.SetProperty("U_Country", e.Country);
                }
            }

            oGeneralData.SetProperty("Code", worker.PassportNumber);
            oGeneralData.SetProperty("U_WorkerName", worker.WorkerName);
            oGeneralData.SetProperty("U_Agent", worker.Agent);
            oGeneralData.SetProperty("U_Age", worker.Age);
            oGeneralData.SetProperty("U_BirthDate", DateTime.ParseExact(worker.BirthDate, "dd-MM-yyyy", null));
            oGeneralData.SetProperty("U_CivilId", worker.CivilId);
            oGeneralData.SetProperty("U_ItemCode", worker.Code);
            oGeneralData.SetProperty("Name", worker.Name);
            oGeneralData.SetProperty("U_Education", worker.Education);
            oGeneralData.SetProperty("U_Gender", worker.Gender);
            oGeneralData.SetProperty("U_Height", worker.Height);
            oGeneralData.SetProperty("U_Language", worker.Language);
            oGeneralData.SetProperty("U_MaritalStatus", worker.MaritalStatus);
            oGeneralData.SetProperty("U_Nationality", worker.Nationality);
            oGeneralData.SetProperty("U_Passport", string.Concat(attachmentPath, passportCopy));
            oGeneralData.SetProperty("U_PassportPoIssue", worker.PassportPoIssue);
            oGeneralData.SetProperty("U_PassportIssDate", DateTime.ParseExact(worker.PassportIssDate, "dd-MM-yyyy", null));
            oGeneralData.SetProperty("U_PassportExpDate", DateTime.ParseExact(worker.PassportExpDate, "dd-MM-yyyy", null));
            oGeneralData.SetProperty("U_PassportNumber", worker.PassportNumber);
            oGeneralData.SetProperty("U_Photo", string.Concat(attachmentPath, photo));
            oGeneralData.SetProperty("U_License", string.Concat(attachmentPath, license));
            oGeneralData.SetProperty("U_Religion", worker.Religion);
            oGeneralData.SetProperty("U_Serial", worker.SerialNumber);
            oGeneralData.SetProperty("U_Status", worker.Status);
            oGeneralData.SetProperty("U_Video", worker.Video);
            oGeneralData.SetProperty("U_Weight", worker.Weight);
            oGeneralData.SetProperty("U_WorkerType", worker.WorkerType);
            oGeneralData.SetProperty("U_Salary", worker.Salary);
            oGeneralData.SetProperty("U_Price", worker.Price);
            oGeneralData.SetProperty("U_Mobile", worker.Mobile);

            oGeneralData.SetProperty("U_Hobbies", worker.Hobbies);
            oGeneralData.SetProperty("U_Location", worker.Location);
            oGeneralData.SetProperty("U_IsNew", worker.IsNew);
            oGeneralData.SetProperty("U_Period", worker.Period);

            oGeneralService.Add(oGeneralData);
            created = true;

            return created;
        }

        public bool UpdateWorker(Worker worker, string cardCode)
        {
            var created = false;
            Trace.WriteLine($"[PR]Update Starts...");
            var attachmentPath = GetAttachmentPath();
            var passportCopy = worker.Passport != null && worker.Passport != string.Empty && worker.Passport != "0" && worker.Passport != "1" ? CreateAttachment(worker.Passport) : worker.Passport;
            var photo = worker.Photo != null && worker.Photo != string.Empty && worker.Photo != "0" && worker.Photo != "1" ? CreateAttachment(worker.Photo) : worker.Passport;
            var license = worker.License != null && worker.License != string.Empty && worker.License != "0" && worker.License != "1" ? CreateAttachment(worker.License) : worker.License;

            var sCmp = B1Company.GetCompanyService();
            var oGeneralService = sCmp.GetGeneralService("WORKERSUDO");

            GeneralDataParams oParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams) as GeneralDataParams;
            oParams.SetProperty("Code", worker.WorkerCode);
            var oGeneralData = oGeneralService.GetByParams(oParams);


            if (oGeneralData != null)
            {
                List<string> languages = new List<string>();

                var gDataCollection = oGeneralData.Child("WORKERLNGS");
                for (int i = gDataCollection.Count - 1; i >= 0; i--)
                    gDataCollection.Remove(i);
                Trace.WriteLine($"[PR]Chech languages...");
                if (worker.Languages != null)
                {
                    Trace.WriteLine($"[PR] foreach (var l in worker.Languages)...");
                    foreach (var l in worker.Languages)
                    {
                        var newLanguage = gDataCollection.Add();
                        Trace.WriteLine($"[PR] U_NAME: l.Name");
                        newLanguage.SetProperty("U_NAME", l.Name);
                        Trace.WriteLine($"[PR] U_VALUE: l.Value");
                        newLanguage.SetProperty("U_VALUE", l.Value);
                    }
                }

                var expDataCollection = oGeneralData.Child("EXPERIENCE");
                for (int i = expDataCollection.Count - 1; i >= 0; i--)
                    expDataCollection.Remove(i);
                if (worker.Experiences != null)
                {
                    foreach (var e in worker.Experiences)
                    {
                        var newExperience = expDataCollection.Add();
                        newExperience.SetProperty("U_WorkerID", worker.PassportNumber);
                        if(e.StartDate !=null)
                            newExperience.SetProperty("U_StartDate", e.StartDate);
                        if (e.EndDate != null)
                            newExperience.SetProperty("U_EndDate", e.EndDate);
                        newExperience.SetProperty("U_Title", e.Title);
                        newExperience.SetProperty("U_Description", e.Description);
                        newExperience.SetProperty("U_CompanyName", e.CompanyName);
                        newExperience.SetProperty("U_Location", e.Location);
                        newExperience.SetProperty("U_Country", e.Country);
                    }
                }
                Trace.WriteLine($"[PR] Condition Done...");

                if (oGeneralData.GetProperty("U_Status").ToString() == ((int)WorkerStatus.Opened).ToString() && oGeneralData.GetProperty("U_Agent").ToString() == cardCode)
                {
                    Trace.WriteLine($"[PR]U_WorkerName:{worker.WorkerName}");
                    oGeneralData.SetProperty("U_WorkerName", worker.WorkerName);
                    Trace.WriteLine($"[PR]U_Age:{worker.Age}");
                    oGeneralData.SetProperty("U_Age", MapField<int>(worker.Age));
                    Trace.WriteLine($"[PR]U_BirthDate:{worker.BirthDate}");
                    oGeneralData.SetProperty("U_BirthDate", MapField<DateTime>(worker.BirthDate));
                    Trace.WriteLine($"[PR]U_CivilId:{worker.CivilId}");
                    oGeneralData.SetProperty("U_CivilId", MapField<string>(worker.CivilId));
                    Trace.WriteLine($"[PR]U_ItemCode:{worker.Code}");
                    oGeneralData.SetProperty("U_ItemCode", MapField<string>(worker.Code));
                    Trace.WriteLine($"[PR]Name:{worker.Name}");
                    oGeneralData.SetProperty("Name", MapField<string>(worker.Name));
                    Trace.WriteLine($"[PR]U_Education:{worker.Education}");
                    oGeneralData.SetProperty("U_Education", MapField<string>(worker.Education));
                    Trace.WriteLine($"[PR]U_Gender:{worker.Gender}");
                    oGeneralData.SetProperty("U_Gender", MapField<string>(worker.Gender));
                    Trace.WriteLine($"[PR]U_Height:{worker.Height}");
                    oGeneralData.SetProperty("U_Height", MapField<string>(worker.Height));
                    Trace.WriteLine($"[PR]U_Language:{worker.Language}");
                    oGeneralData.SetProperty("U_Language", MapField<string>(worker.Language));
                    Trace.WriteLine($"[PR]U_MaritalStatus:{worker.MaritalStatus}");
                    oGeneralData.SetProperty("U_MaritalStatus", MapField<string>(worker.MaritalStatus));
                    Trace.WriteLine($"[PR]U_Nationality:{worker.Nationality}");
                    oGeneralData.SetProperty("U_Nationality", MapField<string>(worker.Nationality));
                    Trace.WriteLine($"[PR]U_PassportIssDate:{worker.PassportIssDate}");
                    oGeneralData.SetProperty("U_PassportIssDate", MapField<DateTime>(worker.PassportIssDate));
                    Trace.WriteLine($"[PR]U_PassportExpDate:{worker.PassportExpDate}");
                    oGeneralData.SetProperty("U_PassportExpDate", MapField<DateTime>(worker.PassportExpDate));
                    Trace.WriteLine($"[PR]U_PassportPoIssue:{worker.PassportPoIssue}");
                    oGeneralData.SetProperty("U_PassportPoIssue", worker.PassportPoIssue);
                    Trace.WriteLine($"[PR]U_PassportNumber:{worker.PassportNumber}");
                    oGeneralData.SetProperty("U_PassportNumber", MapField<string>(worker.PassportNumber));
                    Trace.WriteLine($"[PR]U_WorkerType:{worker.WorkerType}");
                    oGeneralData.SetProperty("U_WorkerType", worker.WorkerType);
                    Trace.WriteLine($"[PR]U_Salary:{worker.Salary}");
                    oGeneralData.SetProperty("U_Salary", worker.Salary);
                    Trace.WriteLine($"[PR]U_Price:{worker.Price}");
                    oGeneralData.SetProperty("U_Price", worker.Price);
                    Trace.WriteLine($"[PR]U_Mobile:{worker.Mobile}");
                    oGeneralData.SetProperty("U_Mobile", worker.Mobile);

                    Trace.WriteLine($"[PR]U_Hobbies:{worker.Hobbies}");
                    oGeneralData.SetProperty("U_Hobbies", worker.Hobbies);
                    Trace.WriteLine($"[PR]U_Location:{worker.Location}");
                    oGeneralData.SetProperty("U_Location", worker.Location);
                    Trace.WriteLine($"[PR]U_IsNew:{worker.IsNew}");
                    oGeneralData.SetProperty("U_IsNew", worker.IsNew);
                    Trace.WriteLine($"[PR]U_Period:{worker.Period}");
                    oGeneralData.SetProperty("U_Period", worker.Period);


                    if (photo.Equals("0"))
                        oGeneralData.SetProperty("U_Photo", string.Empty);
                    else if (!photo.Equals("1"))
                        oGeneralData.SetProperty("U_Photo", string.Concat(attachmentPath, photo));

                    if (passportCopy.Equals("0"))
                        oGeneralData.SetProperty("U_Passport", string.Empty);
                    else if (!passportCopy.Equals("1"))
                        oGeneralData.SetProperty("U_Passport", MapField<string>(string.Concat(attachmentPath, passportCopy)));

                    if (license.Equals("0"))
                        oGeneralData.SetProperty("U_License", string.Empty);
                    else if (!license.Equals("1"))
                        oGeneralData.SetProperty("U_License", MapField<string>(string.Concat(attachmentPath, license)));

                    oGeneralData.SetProperty("U_Religion", MapField<string>(worker.Religion));
                    oGeneralData.SetProperty("U_Serial", MapField<string>(worker.SerialNumber));
                    oGeneralData.SetProperty("U_Status", MapField<string>(worker.Status));
                    oGeneralData.SetProperty("U_Video", MapField<string>(worker.Video));
                    oGeneralData.SetProperty("U_Weight", MapField<string>(worker.Weight));

                    oGeneralService.Update(oGeneralData);
                    created = true;
                }
                else
                    Utilities.LogException($"[PR]Worker {oGeneralData.GetProperty("Code")} is already {((WorkerStatus)Convert.ToInt16(oGeneralData.GetProperty("U_Status"))).ToString()} or agent {cardCode} dont have the permissions to edit this worker.");
            }
            else
            {
                Utilities.LogException($"[PR]Agent dont have permissions to update worker: {worker.WorkerCode}");
                created = false;
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
                //TODO: REMOVE COMMENT TO ADD CARDCODE FILTER
                //worker = instance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.Code == id && x.U_Agent == cardCode).FirstOrDefault();
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

        public List<Worker> GetWorker(string agent, string code, string requestUrl)
        {
            var ServiceInstance = ServiceLayerProvider.GetInstance();
            var workers = new List<WORKERS>();
            workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.U_Agent == agent && x.Code == code).ToList<WORKERS>();

            var workersList = new List<Worker>();
            foreach (var w in workers)
            {
                workersList.Add(
                    new Worker()
                    {
                        Name = w.Name,
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
                        Passport = Utilities.ConvertImagePathToUrl(requestUrl, w.U_Passport),
                        PassportExpDate = MapField<DateTime>(w.U_PassportExpDate).ToShortDateString(),
                        PassportIssDate = MapField<DateTime>(w.U_PassportIssDate).ToShortDateString(),
                        PassportNumber = w.U_PassportNumber,
                        PassportPoIssue = w.U_PassportPoIssue,
                        Photo = Utilities.ConvertImagePathToUrl(requestUrl, w.U_Photo),
                        Religion = w.U_Religion,
                        SerialNumber = w.U_Serial,
                        Status = w.U_Status,
                        Video = w.U_Video,
                        Price = MapField<int>(w.U_Price),
                        Weight = w.U_Weight.ToString(),
                        Hobbies = w.U_Hobbies,
                        Location = w.U_Location,
                        IsNew = w.U_IsNew,
                        Period = w.U_Period
                    }
                    );
            }
            return workersList;
        }

        public List<Worker> GetWorkers(Catalogue worker, string requestUrl, WorkerStatus? status)
        {

            List<Worker> workersList = new List<Worker>();
            try
            {

                var attachmentPath = GetAttachmentPath();
                var exp = GetExpressionSql(worker);
                var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

                var query = new StringBuilder();
                query.Append(@"SELECT DISTINCT ""A"".""Code"",""A"".""U_WorkerName"",""A"".""Name"",""U_ItemCode"",""U_WorkerType"",""U_Serial"",""U_Agent"",""U_Age"",");
                query.Append(@"""U_BirthDate"",""U_Gender"",""D"".""Name"" AS ""U_Nationality"",""D"".""U_NAME"" AS ""U_Nationality_AR"",""R"".""Name"" AS ""U_Religion"",""R"".""U_NAME"" AS ""U_Religion_AR"",");
                query.Append(@"""U_Photo"",""U_License"", ""U_Weight"",""U_Height"",""E"".""Name"" AS ""U_Education"",""E"".""U_NAME"" AS ""U_Education_AR"",");
                query.Append(@"""U_Passport"",""U_Video"",""U_PassportNumber"",""U_PassportIssDate"",""U_PassportExpDate"",""U_PassportPoIssue"",""U_Price"",""U_Salary"",""U_CivilId"",""U_Status"",");
                query.Append($@"""B"".""Name"" AS ""U_MaritalStatus"",""B"".""U_NAME"" AS ""U_MaritalStatus_AR"", ""U_Hobbies"", ""A"".""U_Location"", ""U_IsNew"", ""U_Period""");
                query.Append($@" FROM ""{databaseBame}"".""@WORKERS"" as ""A""");

                query.Append($@" INNER JOIN ""{databaseBame}"".""@MARITALSTATUS"" AS ""B"" ON ""A"".""U_MaritalStatus"" = ""B"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@COUNTRIES"" AS ""D"" ON ""A"".""U_Nationality"" = ""D"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@RELIGION"" AS ""R"" ON ""A"".""U_Religion"" = ""R"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@EDUCATION"" AS ""E"" ON ""A"".""U_Education"" = ""E"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@WORKERLNGS"" AS ""L"" ON ""L"".""Code"" = ""A"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@EXPERIENCE"" AS ""EX"" ON ""EX"".""Code"" = ""A"".""Code""");

                if (status == null)
                    query.Append($@"WHERE 1 = 1 ");
                else
                    query.Append($@"WHERE A.""U_Status"" = '{((int)status).ToString()}' ");

                query.Append(exp);
                Trace.WriteLine($"[PR]{query.ToString()}");
                //var readerResult = dbHelper.ExecuteQuery(query.ToString());


                dbHelper.OpenConnection();
                var readerResult = dbHelper.Execute(query.ToString());
                List<string> codeList = new List<string>();
                foreach (DataRow drow in readerResult.Rows)
                {
                    var code = MapField<string>(drow["Code"]);
                    codeList.Add(code);
                }
                StringBuilder codeQueryStat = new StringBuilder();
                for (int i = 0; i < codeList.Count; i++) //string item in codeList)
                {
                    if (i == codeList.Count - 1)
                        codeQueryStat.Append("'" + codeList[i] + "'");
                    else
                        codeQueryStat.Append("'"+codeList[i] +"'"+ ',');
                }
                var langagesResult = dbHelper.Execute($@"SELECT ""Code"",""U_VALUE"",""U_NAME"",* FROM ""{databaseBame}"".""@WORKERLNGS"" WHERE ""Code"" in({codeQueryStat.ToString()})");
                var experiencesResult = dbHelper.Execute($@"SELECT * FROM ""{databaseBame}"".""@EXPERIENCE"" WHERE ""Code"" in({codeQueryStat.ToString()})");




                foreach (DataRow drow in readerResult.Rows)
                {
                    var code = MapField<string>(drow["Code"]);
                    //var langagesResult = dbHelper.ExecuteQuery($@"SELECT ""U_VALUE"",""U_NAME"" FROM ""{databaseBame}"".""@WORKERLNGS"" WHERE ""Code""='{code}'");

                    var langagesResult1 = langagesResult.AsEnumerable().Where(x => x.Field<string>("Code") == code);
                    List<LookupItem> languages = new List<LookupItem>();

                    foreach (DataRow langaugeRow in langagesResult1)
                    {
                        var name = MapField<string>(langaugeRow["U_NAME"]);
                        var value = MapField<string>(langaugeRow["U_VALUE"]);
                        languages.Add(new LookupItem(name, value));
                    }

                    //var experiencesResult = dbHelper.ExecuteQuery($@"SELECT * FROM ""{databaseBame}"".""@EXPERIENCE"" WHERE ""Code""='{code}'");

                    var experiencesResult1 = experiencesResult.AsEnumerable().Where(x => x.Field<string>("Code") == code);
                    List<Experience> experiences = new List<Experience>();

                    foreach (DataRow experienceRow in experiencesResult1)
                    {
                        var workerId = MapField<string>(experienceRow["U_WorkerId"]);
                        var startDate = MapField<string>(experienceRow["U_StartDate"]);
                        var endDate = MapField<string>(experienceRow["U_EndDate"]);
                        var title = MapField<string>(experienceRow["U_Title"]);
                        var description = MapField<string>(experienceRow["U_Description"]);
                        var companyName = MapField<string>(experienceRow["U_CompanyName"]);
                        var location = MapField<string>(experienceRow["U_Location"]);
                        var country = MapField<string>(experienceRow["U_Country"]);
                        experiences.Add(new Experience { WorkerID = workerId, StartDate = startDate, EndDate = endDate, Title = title, Description = description, CompanyName = companyName, Country = country, Location = location });
                    }

                    var age = DateTime.Now.Year - MapField<DateTime>(drow["U_BirthDate"]).Year;
                    workersList.Add(
                        new Worker()
                        {
                            Name = MapField<string>(drow["U_WorkerType"]),
                            WorkerCode = MapField<string>(drow["Code"]),
                            WorkerName = MapField<string>(drow["U_WorkerName"]),
                            Agent = MapField<string>(drow["U_Agent"]),
                            Age = age,
                            BirthDate = MapField<DateTime>(drow["U_BirthDate"]).ToShortDateString(),
                            CivilId = MapField<string>(drow["U_CivilId"]),
                            Code = MapField<string>(drow["U_ItemCode"]),
                            Education = MapField<string>(drow[Utilities.GetLocalizedField("U_Education")]),
                            Gender = Utilities.GetResourceValue(MapField<string>(drow["U_Gender"])),
                            Height = MapField<string>(drow["U_Height"]),
                            MaritalStatus = MapField<string>(drow[Utilities.GetLocalizedField("U_MaritalStatus")]),
                            Nationality = MapField<string>(drow[Utilities.GetLocalizedField("U_Nationality")]),
                            Passport = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Passport"])),
                            PassportExpDate = MapField<DateTime>(drow["U_PassportExpDate"]).ToShortDateString(),
                            PassportIssDate = MapField<string>(drow["U_PassportIssDate"]),
                            PassportNumber = MapField<string>(drow["U_PassportNumber"]),
                            PassportPoIssue = MapField<string>(drow["U_PassportPoIssue"]),
                            Photo = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Photo"])),
                            License = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_License"])),
                            Religion = MapField<string>(drow[Utilities.GetLocalizedField("U_Religion")]),
                            SerialNumber = MapField<string>(drow["U_Serial"]),
                            Status = MapField<string>(drow["U_Status"]),
                            Video = MapField<string>(drow["U_Video"]),
                            Price = MapField<float>(drow["U_Price"]),
                            Salary = MapField<float>(drow["U_Salary"]),
                            Weight = MapField<string>(drow["U_Weight"]),
                            WorkerType = MapField<string>(drow["U_WorkerType"]),
                            Languages = languages,

                            Hobbies = MapField<string>(drow["U_Hobbies"]),
                            Location = MapField<string>(drow["U_Location"]),
                            IsNew = MapField<string>(drow["U_IsNew"]),
                            Period = MapField<int>(drow["U_Period"]),
                            Experiences = experiences
                        });
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            finally
            {
                dbHelper.CloseConnection();
            }

            return workersList;
        }

        #region Old Methods
        //public List<Worker> GetWorkers(Catalogue worker, string requestUrl, WorkerStatus? status)
        //{

        //    List<Worker> workersList = new List<Worker>();
        //    try
        //    {

        //        var attachmentPath = GetAttachmentPath();
        //        var exp = GetExpressionSql(worker);
        //        var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

        //        var query = new StringBuilder();
        //        query.Append(@"SELECT ""A"".""Code"",""A"".""U_WorkerName"",""A"".""Name"",""U_ItemCode"",""U_Serial"",""U_Agent"",""U_Age"",");
        //        query.Append(@"""U_BirthDate"",""U_Gender"",""D"".""Name"" AS ""U_Nationality"",""D"".""U_NAME"" AS ""U_Nationality_AR"",""R"".""Name"" AS ""U_Religion"",""R"".""U_NAME"" AS ""U_Religion_AR"",");
        //        query.Append(@"""U_Photo"",""U_License"", ""U_Weight"",""U_Height"",""E"".""Name"" AS ""U_Education"",""E"".""U_NAME"" AS ""U_Education_AR"",");
        //        query.Append(@"""U_Passport"",""U_Video"",""U_PassportNumber"",""U_PassportIssDate"",""U_PassportExpDate"",""U_PassportPoIssue"",""U_Price"",""U_CivilId"",""U_Status"",");
        //        query.Append($@"""B"".""Name"" AS ""U_MaritalStatus"",""B"".""U_NAME"" AS ""U_MaritalStatus_AR""");
        //        query.Append($@" FROM ""{databaseBame}"".""@WORKERS"" as ""A""");

        //        query.Append($@" INNER JOIN ""{databaseBame}"".""@MARITALSTATUS"" AS ""B"" ON ""A"".""U_MaritalStatus"" = ""B"".""Code""");
        //        query.Append($@" INNER JOIN ""{databaseBame}"".""@COUNTRIES"" AS ""D"" ON ""A"".""U_Nationality"" = ""D"".""Code""");
        //        query.Append($@" INNER JOIN ""{databaseBame}"".""@RELIGION"" AS ""R"" ON ""A"".""U_Religion"" = ""R"".""Code""");
        //        query.Append($@" INNER JOIN ""{databaseBame}"".""@EDUCATION"" AS ""E"" ON ""A"".""U_Education"" = ""E"".""Code""");

        //        if (status == null)
        //            query.Append($@"WHERE 1 = 1 ");
        //        else
        //            query.Append($@"WHERE A.""U_Status"" = '{((int)status).ToString()}' ");

        //        query.Append(exp);
        //        var readerResult = dbHelper.ExecuteQuery(query.ToString());

        //        foreach (DataRow drow in readerResult.Rows)
        //        {
        //            var code = MapField<string>(drow["Code"]);
        //            var langagesResult = dbHelper.ExecuteQuery($@"SELECT ""U_VALUE"",""U_NAME"" FROM ""{databaseBame}"".""@WORKERLNGS"" WHERE ""Code""='{code}'");
        //            List<LookupItem> languages = new List<LookupItem>();

        //            foreach (DataRow langaugeRow in langagesResult.Rows)
        //            {
        //                var name = MapField<string>(langaugeRow["U_NAME"]);
        //                var value = MapField<string>(langaugeRow["U_VALUE"]);
        //                languages.Add(new LookupItem(name, value));
        //            }
        //            var age = DateTime.Now.Year - MapField<DateTime>(drow["U_BirthDate"]).Year;
        //            workersList.Add(
        //                new Worker()
        //                {
        //                    Name = MapField<string>(drow["Name"]),
        //                    WorkerCode = MapField<string>(drow["Code"]),
        //                    WorkerName = MapField<string>(drow["U_WorkerName"]),
        //                    Agent = MapField<string>(drow["U_Agent"]),
        //                    Age = age,
        //                    BirthDate = MapField<DateTime>(drow["U_BirthDate"]).ToShortDateString(),
        //                    CivilId = MapField<string>(drow["U_CivilId"]),
        //                    Code = MapField<string>(drow["U_ItemCode"]),
        //                    Education = MapField<string>(drow[Utilities.GetLocalizedField("U_Education")]),
        //                    Gender = Utilities.GetResourceValue(MapField<string>(drow["U_Gender"])),
        //                    Height = MapField<string>(drow["U_Height"]),
        //                    MaritalStatus = MapField<string>(drow[Utilities.GetLocalizedField("U_MaritalStatus")]),
        //                    Nationality = MapField<string>(drow[Utilities.GetLocalizedField("U_Nationality")]),
        //                    Passport = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Passport"])),
        //                    PassportExpDate = MapField<DateTime>(drow["U_PassportExpDate"]).ToShortDateString(),
        //                    PassportIssDate = MapField<string>(drow["U_PassportIssDate"]),
        //                    PassportNumber = MapField<string>(drow["U_PassportNumber"]),
        //                    PassportPoIssue = MapField<string>(drow["U_PassportPoIssue"]),
        //                    Photo = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Photo"])),
        //                    License = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_License"])),
        //                    Religion = MapField<string>(drow[Utilities.GetLocalizedField("U_Religion")]),
        //                    SerialNumber = MapField<string>(drow["U_Serial"]),
        //                    Status = MapField<string>(drow["U_Status"]),
        //                    Video = MapField<string>(drow["U_Video"]),
        //                    Price = MapField<float>(drow["U_Price"]),
        //                    Weight = MapField<string>(drow["U_Weight"]),
        //                    Languages = languages
        //                });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.LogException(ex);
        //    }

        //    return workersList;
        //}

        //public List<LookupItem> GetLookupValues<T>()
        //{
        //    var _serviceInstance = ServiceLayerProvider.GetInstance();


        //    if (typeof(T) == typeof(COUNTRIES))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.COUNTRIESUDO.ToList();
        //        return GetLookups<COUNTRIES>(results);
        //    }
        //    else if (typeof(T) == typeof(LANGUAGES))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.LANGUAGESUDO.ToList<LANGUAGES>();
        //        return GetLookups<LANGUAGES>(results);
        //    }
        //    else if (typeof(T) == typeof(MARITALSTATUS))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.MARITALSTATUSUDO.ToList<MARITALSTATUS>();
        //        return GetLookups<MARITALSTATUS>(results);
        //    }
        //    else if (typeof(T) == typeof(WORKERTYPES))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.WORKERTYPESUDO.ToList<WORKERTYPES>();
        //        return GetLookups<WORKERTYPES>(results);
        //    }
        //    else if (typeof(T) == typeof(Item))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.Items.Select(x => new LookupItem(x.ItemName, x.ItemCode)).ToList();
        //        return results;
        //    }
        //    else if (typeof(T) == typeof(RELIGION))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.RELIGION.ToList();
        //        return GetLookups<RELIGION>(results);
        //    }
        //    else if (typeof(T) == typeof(EDUCATION))
        //    {
        //        var results = _serviceInstance.CurrentServicelayerInstance.EDUCATION.ToList();
        //        return GetLookups<EDUCATION>(results);
        //    }
        //    else
        //        return null;

        //    List<LookupItem> GetLookups<Y>(List<Y> list)
        //    {
        //        var lookups = new List<LookupItem>();
        //        foreach (dynamic r in list)
        //        {
        //            if (Utilities.GetCurrentLanguage() == Languages.English.GetDescription())
        //                lookups.Add(new LookupItem(r.Name, r.Code));
        //            else
        //                lookups.Add(new LookupItem(r.U_NAME, r.Code));
        //        }
        //        return lookups;
        //    }
        //}

        //private string GetExpressionSql(Catalogue wrk)
        //{
        //    StringBuilder queryBuilder = new StringBuilder();
        //    if (wrk != null)
        //    {
        //        if (wrk.Age != null)
        //        {
        //            var boundaries = wrk.Age.Trim().Split('-');
        //            //var startYear = DateTime.Now.Year - int.Parse(boundaries[1]);
        //            //var endYear = DateTime.Now.Year - int.Parse(boundaries[0]);

        //            queryBuilder.Append($" AND  {DateTime.Now.Year} - YEAR(\"U_BirthDate\") BETWEEN {boundaries[0]} AND {boundaries[1]}");
        //        }
        //        if (wrk.Gender != null)
        //        {
        //            queryBuilder.Append($" AND \"U_Gender\" = '{wrk.Gender}'");
        //        }
        //        if (wrk.Nationality != null)
        //        {
        //            queryBuilder.Append($" AND \"U_Nationality\" = '{wrk.Nationality}'");
        //        }
        //        if (wrk.MaritalStatus != null)
        //        {
        //            queryBuilder.Append($" AND \"U_MaritalStatus\" = '{wrk.MaritalStatus}'");
        //        }
        //        if (wrk.WorkerType != null)
        //        {
        //            queryBuilder.Append($" AND \"U_ItemCode\" = '{wrk.WorkerType}'");
        //        }
        //        if (wrk.Language != null)
        //        {
        //            queryBuilder.Append($" AND \"U_Language\" = '{wrk.Language}'");
        //        }
        //        return queryBuilder.ToString();
        //    }
        //    else
        //        return null;
        //}

        public List<LookupItem> GetItemsByWorkerType(string workerType)
        {
            try
            {
                var _serviceInstance = ServiceLayerProvider.GetInstance();
                var results = _serviceInstance.CurrentServicelayerInstance.Items.Where(x => x.U_WorkerType == workerType).Select(x => new LookupItem(x.ItemName, x.ItemCode)).ToList();
                return results;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return new List<LookupItem>();
            }

        }

        #endregion

        public List<Worker> GetAgentWorkers(string agent, string requestUrl)
        {
            List<Worker> workersList = new List<Worker>();
            try
            {
                var attachmentPath = GetAttachmentPath();
                var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

                var query = new StringBuilder();
                query.Append(@"SELECT ""A"".""Code"",""A"".""U_WorkerName"",""A"".""U_Mobile"", ""A"".""Name"",""U_ItemCode"",""U_Serial"",""U_Agent"",""U_Age"",""U_WorkerType"", ""U_WorkerName"", ");
                query.Append(@"""U_BirthDate"",""U_Gender"",""D"".""Code"" AS ""U_NationalityCode"",""D"".""Name"" AS ""U_Nationality"",""D"".""U_NAME"" AS ""U_Nationality_AR"",""R"".""Code"" AS ""U_ReligionCode"",""R"".""Name"" AS ""U_Religion"",""R"".""U_NAME"" AS ""U_Religion_AR"",");
                query.Append(@"""U_Photo"",""U_License"",""U_Weight"",""U_Height"",""E"".""Name"" AS ""U_Education"",""E"".""U_NAME"" AS ""U_Education_AR"",");
                query.Append(@"""U_Passport"",""U_Video"",""U_PassportNumber"",""U_PassportIssDate"",""U_PassportExpDate"",""U_PassportPoIssue"",""U_Price"", ""U_Salary"",""U_CivilId"",""U_Status"",");
                query.Append($@"""B"".""Code"" AS ""U_MaritalStatusCode"",""B"".""Name"" AS ""U_MaritalStatus"",""B"".""U_NAME"" AS ""U_MaritalStatus_AR"", ""U_Hobbies"", ""A"".""U_Location"", ""U_IsNew"", ""U_Period""");
                query.Append($@" FROM ""{databaseBame}"".""@WORKERS"" as ""A""");

                query.Append($@" INNER JOIN ""{databaseBame}"".""@MARITALSTATUS"" AS ""B"" ON ""A"".""U_MaritalStatus"" = ""B"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@COUNTRIES"" AS ""D"" ON ""A"".""U_Nationality"" = ""D"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@RELIGION"" AS ""R"" ON ""A"".""U_Religion"" = ""R"".""Code""");
                query.Append($@" INNER JOIN ""{databaseBame}"".""@EDUCATION"" AS ""E"" ON ""A"".""U_Education"" = ""E"".""Code""");
                query.Append($@" WHERE ""U_Agent""='{agent}'");
                dbHelper.OpenConnection();
                var readerResult = dbHelper.Execute(query.ToString());
                List<string> codeList = new List<string>();
                foreach (DataRow drow in readerResult.Rows)
                { 
                    var code = MapField<string>(drow["Code"]);
                    codeList.Add(code);
                }
                StringBuilder codeQueryStat = new StringBuilder();
                for(int i=0; i<codeList.Count;i++) //string item in codeList)
                {
                    if (i == codeList.Count - 1)
                        codeQueryStat.Append("'" + codeList[i] + "'");
                    else
                        codeQueryStat.Append("'" + codeList[i] + "'" + ',');
                }
                    var langagesResult = dbHelper.Execute($@"SELECT ""Code"",""U_VALUE"",""U_NAME"" FROM ""{databaseBame}"".""@WORKERLNGS"" WHERE ""Code""in({codeQueryStat.ToString()})");
                     var experiencesResult = dbHelper.Execute($@"SELECT * FROM ""{databaseBame}"".""@EXPERIENCE"" WHERE ""Code""in({codeQueryStat.ToString()})");

                foreach (DataRow drow in readerResult.Rows)
                {
                    var code = MapField<string>(drow["Code"]);
                    //var langagesResult = dbHelper.Execute($@"SELECT ""U_VALUE"",""U_NAME"" FROM ""{databaseBame}"".""@WORKERLNGS"" WHERE ""Code""='{code}'");
                    //var langagesResult1 = //langagesResult.Select(@"Code ="+ code);
                   var langagesResult1 = langagesResult.AsEnumerable().Where(x => x.Field<string>("Code") == code) ;
                    List<LookupItem> languages = new List<LookupItem>();

                    foreach (DataRow language in langagesResult1)
                    {
                        var name = MapField<string>(language["U_NAME"]);
                        var value = MapField<string>(language["U_VALUE"]);
                        languages.Add(new LookupItem(name, value));
                    }
                    var experiencesResult1 = experiencesResult.AsEnumerable().Where(x => x.Field<string>("Code") == code) ;

                    //var experiencesResult = dbHelper.Execute($@"SELECT * FROM ""{databaseBame}"".""@EXPERIENCE"" WHERE ""Code""='{code}'"); 
                    List<Experience> experiences = new List<Experience>();

                    foreach (DataRow experienceRow in experiencesResult1)
                    {
                        var workerId = MapField<string>(experienceRow["U_WorkerId"]);
                        var startDate = MapField<string>(experienceRow["U_StartDate"]);
                        var endDate = MapField<string>(experienceRow["U_EndDate"]);
                        var title = MapField<string>(experienceRow["U_Title"]);
                        var description = MapField<string>(experienceRow["U_Description"]);
                        var companyName = MapField<string>(experienceRow["U_CompanyName"]);
                        var location = MapField<string>(experienceRow["U_Location"]);
                        var country = MapField<string>(experienceRow["U_Country"]);
                        experiences.Add(new Experience { WorkerID = workerId, StartDate = startDate, EndDate = endDate, Title = title, Description = description, CompanyName = companyName, Country = country, Location = location });
                    }

                    var age = DateTime.Now.Year - MapField<DateTime>(drow["U_BirthDate"]).Year;
                    workersList.Add(
                        new Worker()
                        {
                            Name = MapField<string>(drow["Name"]),
                            WorkerCode = MapField<string>(drow["Code"]),
                            WorkerName = MapField<string>(drow["U_WorkerName"]),
                            Mobile = MapField<string>(drow["U_Mobile"]),
                            WorkerType = MapField<string>(drow["U_WorkerType"]),
                            Salary = MapField<float>(drow["U_Salary"]),
                            Agent = MapField<string>(drow["U_Agent"]),
                            Age = age,
                            BirthDate = MapField<DateTime>(drow["U_BirthDate"]).ToShortDateString(),
                            CivilId = MapField<string>(drow["U_CivilId"]),
                            Code = MapField<string>(drow["U_ItemCode"]),
                            Education = AddHashSeperator(drow["U_NationalityCode"].ToString(), MapField<string>(drow[Utilities.GetLocalizedField("U_Education")])),
                            Gender = Utilities.GetResourceValue(MapField<string>(drow["U_Gender"])),
                            Height = MapField<string>(drow["U_Height"]),
                            MaritalStatus = AddHashSeperator(drow["U_MaritalStatusCode"].ToString(), MapField<string>(drow[Utilities.GetLocalizedField("U_MaritalStatus")])),
                            Nationality = AddHashSeperator(drow["U_NationalityCode"].ToString(), MapField<string>(drow[Utilities.GetLocalizedField("U_Nationality")])),
                            Passport = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Passport"])),
                            PassportExpDate = MapField<DateTime>(drow["U_PassportExpDate"]).ToShortDateString(),
                            PassportIssDate = MapField<string>(drow["U_PassportIssDate"]),
                            PassportNumber = MapField<string>(drow["U_PassportNumber"]),
                            PassportPoIssue = MapField<string>(drow["U_PassportPoIssue"]),
                            Photo = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_Photo"])),
                            License = Utilities.ConvertImagePathToUrl(requestUrl, MapField<string>(drow["U_License"])),
                            Religion = AddHashSeperator(drow["U_ReligionCode"].ToString(), MapField<string>(drow[Utilities.GetLocalizedField("U_Religion")])),
                            SerialNumber = MapField<string>(drow["U_Serial"]),
                            Status = GetWorkerStatus(MapField<int>(drow["U_Status"])),
                            Video = MapField<string>(drow["U_Video"]),
                            Price = MapField<float>(drow["U_Price"]),
                            Weight = MapField<string>(drow["U_Weight"]),
                            Languages = languages,

                            Hobbies = MapField<string>(drow["U_Hobbies"]),
                            Location = MapField<string>(drow["U_Location"]),
                            IsNew = MapField<string>(drow["U_IsNew"]),
                            Period = MapField<int>(drow["U_Period"]),
                            Experiences = experiences
                        });
                }

            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            finally
            {
                dbHelper.CloseConnection();
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
                Utilities.LogException(ex);
                instance.CurrentServicelayerInstance.Detach(salesOrder);
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

                    SAPbobsCOM.Documents oDownPay = base.B1Company.GetBusinessObject(BoObjectTypes.oDownPayments) as Documents;

                    oDownPay.CardCode = salesOrder.CardCode;
                    oDownPay.DownPaymentType = SAPbobsCOM.DownPaymentTypeEnum.dptInvoice;
                    oDownPay.DocTotal = paymentAmount;
                    oDownPay.Lines.BaseType = 17;
                    oDownPay.Lines.BaseEntry = salesOrder.Lines.DocEntry;
                    oDownPay.Lines.BaseLine = salesOrder.Lines.LineNum;
                    oDownPay.Lines.UnitPrice = salesOrder.Lines.UnitPrice;

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
                        oPay.TransferSum = paymentAmount;
                        oPay.TransferAccount = "_SYS00000000035";

                        //oPay.CashSum = paymentAmount;
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

        //public void CancelSalesOrder(Transaction payment)
        //{
        //    Utilities.LogException("Cancellation Starts");
        //    Utilities.LogException($"Result:{payment.Result}, TrackId:{payment.TrackID},TranId:{payment.TranID },Ref:{payment.Ref}");
        //    Document oSalesOrder = new Document();
        //    WORKERS worker = new WORKERS();
        //    try
        //    {
        //        var instance = ServiceLayerProvider.GetInstance();
        //        oSalesOrder = instance.CurrentServicelayerInstance.Orders.Where(x => x.U_PaymentID == payment.PaymentID).FirstOrDefault();
        //        oSalesOrder.U_Status = payment.Result;
        //        oSalesOrder.U_TrackID = payment.TrackID == null ? string.Empty : payment.TrackID;
        //        oSalesOrder.U_TranID = payment.TranID == null ? string.Empty : payment.TranID;
        //        oSalesOrder.U_Ref = payment.Ref == null ? string.Empty : payment.Ref;

        //        oSalesOrder.Cancelled = SAPbobsCOM.BoYesNoEnum.tYES.ToString();
        //        instance.CurrentServicelayerInstance.UpdateObject(oSalesOrder);
        //        var workerCode = oSalesOrder.U_WorkerID;
        //        worker = instance.CurrentServicelayerInstance.WORKERSUDO.Where(x => x.Code == workerCode).FirstOrDefault();
        //        worker.U_Status = "1";
        //        instance.CurrentServicelayerInstance.UpdateObject(worker);
        //        instance.CurrentServicelayerInstance.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.LogException(ex);
        //        instance.CurrentServicelayerInstance.Detach(oSalesOrder);
        //        instance.CurrentServicelayerInstance.Detach(worker); 
        //    }
        //}

        public void CancelSalesOrder(Transaction payment)
        {
            Utilities.LogException("Cancellation Starts");
            Utilities.LogException($"Result:{payment.Result}, TrackId:{payment.TrackID},TranId:{payment.TranID },Ref:{payment.Ref}");
            Document oSalesOrder = new Document();
            WORKERS worker = new WORKERS();
            try
            {
                var salesOrder = base.B1Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
                var records = base.B1Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                records.DoQuery(string.Format("SELECT * FROM \"ORDR\" WHERE \"U_PaymentID\"='{0}'", payment.PaymentID));
                salesOrder.Browser.Recordset = records;
                salesOrder.UserFields.Fields.Item("U_Status").Value = payment.Result;
                salesOrder.UserFields.Fields.Item("U_TrackID").Value = payment.TrackID == null ? string.Empty : payment.TrackID;
                salesOrder.UserFields.Fields.Item("U_TranID").Value = payment.TranID == null ? string.Empty : payment.TranID;
                salesOrder.UserFields.Fields.Item("U_Ref").Value = payment.Ref == null ? string.Empty : payment.Ref;

                string workerCode = salesOrder.UserFields.Fields.Item("U_WorkerID").Value.ToString();
                records.DoQuery(string.Format($@"UPDATE ""@WORKERS"" SET ""U_Status"" = '1' WHERE ""Code"" = '{workerCode}'"));

                if (salesOrder.Update() != 0)
                    throw new Exception(base.B1Company.GetLastErrorDescription());
                if (salesOrder.Cancel() != 0)
                    throw new Exception(base.B1Company.GetLastErrorDescription());
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
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

        public bool CheckSalesOrderAvailability(string paymentId)
        {
            bool isAvailable = false;
            var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();

            var result = conn.ExecuteQuery($"SELECT COUNT(\"DocEntry\") \"COUNT\" FROM \"{base.databaseName}\".\"ORDR\" WHERE \"U_PaymentID\"='{paymentId}'");
            var count = int.Parse(result.Rows[0]["COUNT"].ToString());
            if (count > 0)
                isAvailable = true;

            return isAvailable;
        }

        public List<LookupItem> GetAllCountries()
        {
            var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

            var result = dbHelper.ExecuteQuery($@"SELECT ""Code"", ""Name"" FROM ""{databaseBame}"".""OCRY""");
            List<LookupItem> countries = new List<LookupItem>();

            foreach (DataRow country in result.Rows)
            {
                var name = MapField<string>(country["Code"]);
                var value = MapField<string>(country["Name"]);
                countries.Add(new LookupItem(name, value));
            }
            return countries;

        }

        public List<LookupItem> GetLookupValues<T>()
        {
            var _serviceInstance = ServiceLayerProvider.GetInstance();


            if (typeof(T) == typeof(COUNTRIES))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.COUNTRIESUDO.ToList();
                return GetLookups<COUNTRIES>(results);
            }
            else if (typeof(T) == typeof(B1ServiceLayer.SAPB1.Country))
            {
                //var results = _serviceInstance.CurrentServicelayerInstance.Countries.ToList();
                //return GetLookups<B1ServiceLayer.SAPB1.Country>(results);
                var databaseBame = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);

                var result = dbHelper.ExecuteQuery($@"SELECT ""Code"", ""Name"" FROM ""{databaseBame}"".""OCRY""");
                List<LookupItem> countries = new List<LookupItem>();

                foreach (DataRow country in result.Rows)
                {
                    var name = MapField<string>(country["Name"]);
                    var value = MapField<string>(country["Name"]);
                    countries.Add(new LookupItem(name, value));
                }
                return countries;
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
            else if (typeof(T) == typeof(Item))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.WORKERTYPESUDO.ToList<WORKERTYPES>();
                return GetLookups<WORKERTYPES>(results);
            }
            else if (typeof(T) == typeof(Item))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.Items.Select(x => new LookupItem(x.ItemCode, x.ItemName)).ToList();
                return results;
            }
            else if (typeof(T) == typeof(RELIGION))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.RELIGION.ToList();
                return GetLookups<RELIGION>(results);
            }
            else if (typeof(T) == typeof(EDUCATION))
            {
                var results = _serviceInstance.CurrentServicelayerInstance.EDUCATION.ToList();
                return GetLookups<EDUCATION>(results);
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
        }

        public double GetDownPaymentAmount()
        {
            double paymentAmount = 0;
            try
            {
                var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();

                var result = conn.ExecuteQuery($@"SELECT IFNULL(""U_DownPay"",0) AS ""U_DownPay"" FROM ""{base.databaseName}"".""OADM""");
                foreach (DataRow drow in result.Rows)
                {
                    paymentAmount = MapField<double>(drow["U_DownPay"]);
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
                foreach (DataRow drow in result.Rows)
                {
                    itemCode = MapField<string>(drow["ItemCode"]);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
            return itemCode;
        }

        public string GetAttachmentPath()
        {
            var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
            try
            {
                var path = string.Empty;

                StringBuilder query = new StringBuilder($@"SELECT IFNULL(""AttachPath"",'') ""AttachPath"" FROM ""{base.databaseName}"".""OADP""");
                var result = conn.ExecuteQuery(query.ToString());
                foreach (DataRow drow in result.Rows)
                {
                    path = MapField<string>(drow["AttachPath"]);
                }
                return path;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return string.Empty;
            }
        }

        public string GetEmailAddress(string cardCode)
        {
            var emailAddress = string.Empty;
            try
            {
                var ServiceInstance = ServiceLayerProvider.GetInstance();
                var user = ServiceInstance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.CardCode == cardCode).FirstOrDefault();
                if (user != null)
                {
                    if (user.EmailAddress != null)
                        emailAddress = user.EmailAddress;
                }
                return emailAddress;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return emailAddress;
            }
        }

        public bool IsWorkerAvailable(string id)
        {
            try
            {
                var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
                var path = string.Empty;

                StringBuilder query = new StringBuilder($@"SELECT ""Code"" FROM ""{base.databaseName}"".""@WORKERS"" WHERE ""Code""='{id}' ");
                var result = conn.ExecuteQuery(query.ToString());
                if (result.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return false;
            }
        }

        public List<LookupItem> GetLanguagesById(string jsonString)
        {
            var lookups = new List<LookupItem>();
            try
            {
                var selectedIds = Utilities.ConvertJsonStringToObject<string[]>(jsonString);
                var ids = String.Join(",", selectedIds);
                var query = $@"SELECT ""Code"",""U_NAME"" FROM ""{base.databaseName}"".""@LANGUAGES"" WHERE ""Code"" IN ({ids})";
                var conn = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
                var result = conn.ExecuteQuery(query);
                foreach (DataRow row in result.Rows)
                {
                    lookups.Add(new LookupItem(
                        MapField<string>(row["U_NAME"]),
                        MapField<string>(row["Code"])));
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }

            return lookups;
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
                    var startYear = DateTime.Now.Year - int.Parse(boundaries[0]);
                    var endYear = DateTime.Now.Year - int.Parse(boundaries[1]);
                    if (queryBuilder.Length > 0)
                        queryBuilder.Append($" AND \"U_BirthDate\" BETWEEN {startYear} AND {endYear}");
                    else
                    {
                        queryBuilder = new StringBuilder("WHERE ");
                        queryBuilder.Append($"\"U_BirthDate\" BETWEEN {startYear} AND {endYear}");
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

                    queryBuilder.Append($" AND  {DateTime.Now.Year} - YEAR(\"U_BirthDate\") BETWEEN {boundaries[0]} AND {boundaries[1]}");
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
                    queryBuilder.Append($" AND \"U_WorkerType\" = '{wrk.WorkerType}'");
                }
                if (wrk.Language != null)
                {
                    queryBuilder.Append($" AND \"L\".\"U_VALUE\" = '{wrk.Language}'");
                }
                if (wrk.Languages != null)
                {
                    var ids = String.Join(",", wrk.Languages);
                    queryBuilder.Append($" AND \"L\".\"U_VALUE\" IN ({ids})");
                }
                if (wrk.Location != null)
                {
                    queryBuilder.Append($" AND UPPER(\"U_Location\") LIKE '%{wrk.Location.ToUpper()}%'");
                }
                if (wrk.Hobbies != null)
                {
                    queryBuilder.Append($" AND UPPER(\"U_Hobbies\") LIKE '%{wrk.Hobbies.ToUpper()}%'");
                }
                if (wrk.IsNew != null && wrk.IsNew.Equals("Y"))
                {
                    queryBuilder.Append($" AND \"U_IsNew\" = 'Y'");
                }
                if (wrk.Period != null && wrk.Period > 0)
                {
                    queryBuilder.Append($" AND \"U_IsNew\" = 'N' AND \"U_Period\" = '{wrk.Period}'");
                }
                if (wrk.YearsOfExperience != null && wrk.YearsOfExperience > 0)
                {
                    queryBuilder.Append($" AND (YEAR(\"EX\".\"U_EndDate\") - YEAR(\"EX\".\"U_StartDate\")) = '{wrk.YearsOfExperience}'");
                }
                if (wrk.Country != null)
                {
                    queryBuilder.Append($" AND (\"EX\".\"U_Country\") = '{wrk.Country}'");
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

        public string GetWorkerStatus(int status)
        {
            try
            {
                return AddHashSeperator(status.ToString(), Utilities.GetResourceValue(((WorkerStatus)status).GetDescription()));
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return string.Empty;
            }
        }

        private string AddHashSeperator(string id, string name)
        {
            return $"{id}#{name}";
        }
        #endregion
    }
}
