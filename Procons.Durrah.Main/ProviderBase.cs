namespace Procons.Durrah.Main
{
    using Procons.DataBaseHelper;
    using Procons.Durrah.Common;
    using Sap.Data.Hana;
    using SAPbobsCOM;
    using System;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Web;

    public abstract class ProviderBase : ILogger
    {
        private const string CompanySession = "B1CompanySession";
        protected ServiceLayerProvider instance = new ServiceLayerProvider();
        protected ServiceLayerProvider Instance { get { return ServiceLayerProvider.GetInstance(); } }
        public DatabaseHelper<HanaConnection> dbHelper = Factory.DeclareClass<DatabaseHelper<HanaConnection>>();
        public string databaseName = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);
        private Company _oCompany;
        public Company B1Company
        {
            get
            {
                return this.GetInstance();
            }
        }
        private Company ConnectCompany()
        {
            _oCompany = new Company();
            int checkConnected = -1;
            string sapServer = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.SapServer);
            string companyDB = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.DatabaseName);
            string dbUsername = "SYSTEM";
            string dbPassword = "Pr0c0ns41t";
            string sapUsername = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.UserName);
            string sapPassword = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.Password);
            string sapLicense = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.SLDServer);

            _oCompany.Server = sapServer;
            _oCompany.SLDServer = sapLicense;
            _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;


            _oCompany.CompanyDB = companyDB;
            _oCompany.DbUserName = dbUsername;
            _oCompany.DbPassword = dbPassword;
            _oCompany.UserName = sapUsername;
            _oCompany.Password = sapPassword;
            _oCompany.UseTrusted = false;

            if (_oCompany.Connected)
                checkConnected = 0;
            else
            {
                checkConnected = _oCompany.Connect();

                if (checkConnected != 0)
                {
                    var test = _oCompany.GetLastErrorDescription();
                }
            }
            return _oCompany;
        }

        public Company GetInstance()
        {
            Company company = null;

            var currentSession = HttpContext.Current != null ? HttpContext.Current.Session[CompanySession] : null;
            if (currentSession != null)
                company = (Company)currentSession;
            else
            {
                company = ConnectCompany();
                if (HttpContext.Current != null)
                    HttpContext.Current.Session.Add(CompanySession, company);
            }
            return company;
        }

        protected T MapField<T>(object o)
        {
            var result = default(T);

            if (o != DBNull.Value && o != null)
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
            {
                if (typeof(T) == typeof(DateTime))
                    return (T)(object)DateTime.MinValue;
                else if (typeof(T) == typeof(string))
                    return (T)(object)string.Empty;
                else
                    return default(T);
            }

        }

        #region Table Methods

        public void DeleteTable(string tableName)
        {
            var oUserTablesMD = B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables) as SAPbobsCOM.UserTablesMD;
            try
            {
                if (oUserTablesMD.GetByKey(tableName))
                {
                    if (oUserTablesMD.Remove() != 0)
                    {
                        var error = B1Company.GetLastErrorDescription();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oUserTablesMD.ReleaseObject();
            }
        }
        public void AddTable(string tableName, string description, SAPbobsCOM.BoUTBTableType tableType)
        {
            var oUserTablesMD = B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables) as SAPbobsCOM.UserTablesMD;
            try
            {
                if (!oUserTablesMD.GetByKey(tableName))
                {
                    oUserTablesMD.TableName = tableName;
                    oUserTablesMD.TableDescription = description;
                    oUserTablesMD.TableType = tableType;
                    if (oUserTablesMD.Add() != 0)
                    {
                        var error = B1Company.GetLastErrorDescription();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oUserTablesMD.ReleaseObject();
            }
        }

        #endregion

        #region FIELD METHODS

        /// <summary>
        /// A method for adding new field to B1 table
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="description">Field description</param>
        /// <param name="tableName">Table the field will be added to</param>
        /// <param name="fieldType">Field Type</param>
        /// <param name="size">Field size in the database</param>
        /// <param name="subType"></param>
        /// <param name="mandatory"></param>
        /// <param name="addedToUDT">If this field will be added to system table or User defined table</param>
        /// <param name="valiedValue">The default selected value</param>
        /// <param name="validValues">Add the values seperated by comma "," for value and description ex:(Value,Description)</param>
        public void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, Nullable<int> size, SAPbobsCOM.BoYesNoEnum mandatory, SAPbobsCOM.BoFldSubTypes subType, bool addedToUDT, string validValue, params string[] validValues)
        {

            var objUserFieldMD = B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields) as SAPbobsCOM.UserFieldsMD;
            try
            {
                if (addedToUDT)
                    tableName = string.Format("@{0}", tableName);
                if (!IsFieldExists(name, tableName))
                {
                    objUserFieldMD.TableName = tableName;
                    objUserFieldMD.Name = name;
                    objUserFieldMD.Description = description;
                    objUserFieldMD.Type = fieldType;
                    objUserFieldMD.Mandatory = mandatory;

                    if (size == null || size <= 0)
                        size = 50;

                    if (fieldType != SAPbobsCOM.BoFieldTypes.db_Numeric)
                        objUserFieldMD.Size = (int)size;
                    else
                        objUserFieldMD.EditSize = 10;

                    if (fieldType == BoFieldTypes.db_Float && subType == BoFldSubTypes.st_None)
                        objUserFieldMD.SubType = BoFldSubTypes.st_Quantity;
                    else
                        objUserFieldMD.SubType = subType;

                    if (validValue != null)
                        objUserFieldMD.DefaultValue = validValue;

                    if (validValues != null)
                    {
                        foreach (string s in validValues)
                        {
                            var valuesAttributes = s.Split(',');
                            if (valuesAttributes.Length == 2)
                                objUserFieldMD.ValidValues.Description = valuesAttributes[1];
                            objUserFieldMD.ValidValues.Value = valuesAttributes[0];
                            objUserFieldMD.ValidValues.Add();
                        }
                    }

                    if (objUserFieldMD.Add() != 0)
                    {
                        var test = _oCompany.GetLastErrorDescription();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                objUserFieldMD.ReleaseObject();
            }
        }
        /// <summary>
        /// A method for adding new field to B1 table
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="description">Field description</param>
        /// <param name="tableName">Table the field will be added to</param>
        /// <param name="fieldType">Field Type</param>
        /// <param name="size">Field size in the database</param>
        /// <param name="subType"></param>
        /// <param name="mandatory"></param>
        /// <param name="addedToUDT">If this field will be added to system table or User defined table</param>
        /// <param name="valiedValue">The default selected value</param>
        /// <param name="validValues">Add the values seperated by comma "," for value and description ex:(Value,Description)</param>
        public void AddFieldSL(string name, string description, string tableName, string fieldType, Nullable<int> size, string mandatory, string subType, bool addedToUDT, string validValue, params string[] validValues)
        {

            B1ServiceLayer.SAPB1.UserFieldMD userField = new B1ServiceLayer.SAPB1.UserFieldMD();
            try
            {
                if (addedToUDT)
                    tableName = string.Format("@{0}", tableName);
                if (!IsFieldExists(name, tableName))
                {
                    userField.TableName = tableName;
                    userField.Name = name;
                    userField.Description = description;
                    userField.Type = fieldType;
                    userField.Mandatory = mandatory;

                    if (size == null || size <= 0)
                        size = 50;

                    if (fieldType != "db_Numeric")
                        userField.Size = (int)size;
                    else
                        userField.EditSize = 10;

                    if (fieldType == "db_Float" && (subType == "st_None" || subType == null))
                        userField.SubType = "st_Quantity";
                    else
                        userField.SubType = subType;

                    if (validValue != null)
                        userField.DefaultValue = validValue;

                    if (validValues != null)
                    {
                        foreach (string s in validValues)
                        {
                            B1ServiceLayer.SAPB1.ValidValueMD vValue = new B1ServiceLayer.SAPB1.ValidValueMD();
                            var valuesAttributes = s.Split(',');
                            if (valuesAttributes.Length == 2)
                                vValue.Description = valuesAttributes[1];
                            vValue.Value = valuesAttributes[0];
                            userField.ValidValuesMD.Add(vValue);
                        }
                    }

                    instance.CurrentServicelayerInstance.AddToUserFieldsMD(userField);
                    //DataServiceResponse response = instance.CurrentServicelayerInstance.SaveChanges();
                    //if (null != response)
                    //{
                    //    ChangeOperationResponse opRes = (ChangeOperationResponse)response.SingleOrDefault();
                    //    object retField = ((EntityDescriptor)(opRes.Descriptor)).Entity;
                    //}
                }
            }
            catch (Exception ex)
            {
                instance.CurrentServicelayerInstance.Detach(userField);
            }
        }

        public void AddFieldSL(string name, string description, string tableName, string fieldType, Nullable<int> size, string mandatory, string subType, bool addedToUDT)
        {
            AddFieldSL(name, description, tableName, fieldType, size, mandatory, subType, addedToUDT, null);
        }

        public void AddFieldSL(string name, string description, string tableName, string fieldType, string mandatory, string subType, bool addedToUDT)
        {
            AddFieldSL(name, description, tableName, fieldType, null, mandatory, subType, addedToUDT);
        }

        public void AddFieldSL(string name, string description, string tableName, string fieldType, int size, string mandatory, bool addedToUDT)
        {
            AddFieldSL(name, description, tableName, fieldType, size, mandatory, null, addedToUDT);
        }

        public void AddFieldSL(string name, string description, string tableName, string fieldType, string mandatory, bool addedToUDT)
        {
            AddFieldSL(name, description, tableName, fieldType, null, mandatory, null, addedToUDT);
        }

        /// <summary>
        /// A method for adding new field to B1 table
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="description">Field description</param>
        /// <param name="tableName">Table the field will be added to</param>
        /// <param name="fieldType">Field Type</param>
        /// <param name="size">Field size in the database</param>       
        /// <param name="mandatory">bool: if the value is mandatory to be filled</param>
        /// <param name="subType"></param>
        /// <param name="addedToUDT">If this field will be added to system table or User defined table</param>
        public void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, Nullable<int> size, SAPbobsCOM.BoYesNoEnum mandatory, SAPbobsCOM.BoFldSubTypes subType, bool addedToUDT)
        {
            AddField(name, description, tableName, fieldType, size, mandatory, subType, addedToUDT, null);
        }

        /// <summary>
        /// A method for adding new field to B1 table
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="description">Field description</param>
        /// <param name="tableName">Table the field will be added to</param>
        /// <param name="fieldType">Field Type</param>
        /// <param name="size">Field size in the database</param>     
        /// <param name="mandatory">bool: if the value is mandatory to be filled</param>
        /// <param name="subType">Sub field type</param>
        public void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, SAPbobsCOM.BoYesNoEnum mandatory, SAPbobsCOM.BoFldSubTypes subType, bool addedToUDT)
        {
            AddField(name, description, tableName, fieldType, null, mandatory, subType, addedToUDT);
        }

        public void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, int size, SAPbobsCOM.BoYesNoEnum mandatory, bool addedToUDT)
        {
            AddField(name, description, tableName, fieldType, size, mandatory, 0, addedToUDT);
        }

        /// <summary>
        /// A method for adding new field to B1 table
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="description">Field description</param>
        /// <param name="tableName">Table the field will be added to</param>
        /// <param name="fieldType">Field Type</param>
        /// <param name="size">Field size in the database</param>     
        /// <param name="mandatory">bool: if the value is mandatory to be filled</param>
        public void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, SAPbobsCOM.BoYesNoEnum mandatory, bool addedToUDT)
        {
            AddField(name, description, tableName, fieldType, null, mandatory, 0, addedToUDT);
        }


        /// <summary>
        /// Check if the field is already created in a table
        /// </summary>
        /// <param name="fieldName">Field name to be checked</param>
        /// <param name="tableName">table to checked the values in</param>
        /// <returns>bool: return the value if teh field is created or not</returns>
        public bool IsFieldExists(string fieldName, string tableName)
        {
            try
            {
                var result = instance.CurrentServicelayerInstance.UserFieldsMD.Where(x => x.TableName == tableName.ToUpper() && x.Name == fieldName);
                if (result.Count() > 0)
                    return true;
                else
                    return false;
                //var recordsSet = B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;

                //    StringBuilder query = new StringBuilder("SELECT COUNT(\"AliasID\") AS \"Count\" ");
                //    query.Append(string.Format("FROM \"{0}\".\"CUFD\" ", B1Company.CompanyDB));
                //    query.Append("WHERE \"AliasID\" ='{0}' AND \"TableID\" = '{1}'");


                //    recordsSet.DoQuery(string.Format(query.ToString(), fieldName, tableName.ToUpper()));
                //    recordsSet.MoveFirst();
                //    if (Convert.ToInt32(recordsSet.Fields.Item("Count").Value) > 0)
                //        return true;
                //    else
                //        return false;
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                //recordsSet.ReleaseObject();
            }
        }

        public void AddTableSL(string tableName, string description, SAPbobsCOM.BoUTBTableType tableType)
        {
            B1ServiceLayer.SAPB1.UserTablesMD oUserTablesMD = new B1ServiceLayer.SAPB1.UserTablesMD();

            try
            {
                var result = instance.CurrentServicelayerInstance.UserTablesMD.Where(x => x.TableName == tableName);
                if (result.Count() == 0)
                {
                    oUserTablesMD.TableName = tableName;
                    oUserTablesMD.TableDescription = description;
                    oUserTablesMD.TableType = tableType.ToString();
                    instance.CurrentServicelayerInstance.AddToUserTablesMD(oUserTablesMD);
                }
            }
            catch (Exception ex)
            {
                if (instance != null)
                {
                    instance.CurrentServicelayerInstance.Detach(oUserTablesMD);
                }
            }
        }

        public bool AddUdo(string code, string tableName)
        {
            var udo = new B1ServiceLayer.SAPB1.UserObjectsMD()
            {
                TableName = tableName,
                Code = code,
                Name = code
            };
            try
            {
                var slInstance = instance.CurrentServicelayerInstance;
                var result = slInstance.UserObjectsMD.Where(x => x.Code == udo.Name);
                if (result.Count() == 0)
                {
                    slInstance.AddToUserObjectsMD(udo);
                }
            }
            catch (Exception ex)
            {
                instance.CurrentServicelayerInstance.Detach(udo);
                throw ex;
            }
            return true;
        }

        #endregion

    }
}
