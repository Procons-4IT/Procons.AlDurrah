namespace Procons.Durrah.Main
{
    using SAPbobsCOM;
    using System;
    using System.Text;
    public abstract class ProviderBase
    {
        private  Company _oCompany;
        public Company B1Company
        {
            get
            {
                return _oCompany == null ? ConnectCompany() : _oCompany;
            }
        }
        private  Company ConnectCompany()
        {
            _oCompany = new Company();
            int checkConnected = -1;
            string sapServer = "saphana:30015";
            string companyDB = "ADSC_LIVE";
            string dbUsername = "SYSTEM";
            string dbPassword = "Pr0c0ns41t";
            string sapUsername = "manager";
            string sapPassword = "1234";
            string dbServerType = "Hana";
            string sapLicense = "saphana:40000";

            _oCompany.Server = sapServer;
            _oCompany.SLDServer = sapLicense;

            switch (dbServerType)
            {
                case "2005": _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005; break;
                case "2008": _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008; break;
                case "2012": _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012; break;
                case "2014": _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014; break;
                default: _oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB; break;
            }

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

        public  void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, int size, SAPbobsCOM.BoYesNoEnum mandatory, bool addedToUDT)
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
        public  void AddField(string name, string description, string tableName, SAPbobsCOM.BoFieldTypes fieldType, SAPbobsCOM.BoYesNoEnum mandatory, bool addedToUDT)
        {
            AddField(name, description, tableName, fieldType, null, mandatory, 0, addedToUDT);
        }


        /// <summary>
        /// Check if the field is already created in a table
        /// </summary>
        /// <param name="fieldName">Field name to be checked</param>
        /// <param name="tableName">table to checked the values in</param>
        /// <returns>bool: return the value if teh field is created or not</returns>
        public  bool IsFieldExists(string fieldName, string tableName)
        {
            var recordsSet = B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
            try
            {
                StringBuilder query = new StringBuilder("SELECT COUNT(\"AliasID\") AS \"Count\" ");
                query.Append(string.Format("FROM \"{0}\".\"CUFD\" ", B1Company.CompanyDB));
                query.Append("WHERE \"AliasID\" ='{0}' AND \"TableID\" = '{1}'");


                recordsSet.DoQuery(string.Format(query.ToString(), fieldName, tableName.ToUpper()));
                recordsSet.MoveFirst();
                if (Convert.ToInt32(recordsSet.Fields.Item("Count").Value) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                recordsSet.ReleaseObject();
            }
        }

        #endregion

    }
}
