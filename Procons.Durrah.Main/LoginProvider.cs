namespace Procons.Durrah.Main
{
    using AspNet = Microsoft.AspNet.Identity;
    using Procons.Durrah.Common;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Runtime.InteropServices;
    public class LoginProvider : ProviderBase
    {
        public ApplicationUser FindUser(string userName, string password)
        {
            ApplicationUser user = null;
            try
            {
                ServiceLayerProvider loginInstance = new ServiceLayerProvider();
                loginInstance.Login();
                var encryptedPassword = Utilities.Encrypt(password);
                var result = loginInstance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.U_UserName == userName & x.U_Password == encryptedPassword).FirstOrDefault();
                if (result != null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = result.U_UserName,
                        EmailConfirmed = true,
                        FirstName = result.CardName,
                        LastName = result.CardName,
                        Email = result.EmailAddress,
                        UserType = result.CardType,
                        CardCode = result.CardCode
                    };
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }
 
            return user;
        }

        public List<Worker> GetWorkers([Optional]string agent)
        {
            List<Worker> workersList = new List<Worker>();
            try
            {
                var ServiceInstance = ServiceLayerProvider.GetInstance();

                var workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.ToList<WORKERS>();

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
            }
            catch(Exception ex)
            {
                Utilities.LogException(ex);
            }       
            return workersList;
        }

        public AspNet.IdentityResult CreateUser(ApplicationUser user)
        {
            AspNet.IdentityResult identityResult = null;
            var password = Utilities.Encrypt(user.Password);
            var oDoc = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners) as SAPbobsCOM.BusinessPartners;
            var oRecords = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
            try
            {
                oRecords.DoQuery($@"SELECT ""CardCode"" FROM OCRD WHERE ""U_UserName"" = '{user.UserName}' OR ""E_Mail"" = '{user.Email}'");
                if (oRecords.RecordCount == 0)
                {
                    oDoc.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                    oDoc.Series = GetSeriesCode();
                    oDoc.UserFields.Fields.Item("U_Password").Value = password;
                    oDoc.UserFields.Fields.Item("U_UserName").Value = user.UserName;
                    oDoc.EmailAddress = user.Email;
                    oDoc.CardName = $"{user.FirstName} {user.LastName}";
                    oDoc.EmailAddress = user.Email;
                    if (oDoc.Add() != 0)
                    {
                        var err = base.B1Company.GetLastErrorDescription();
                        identityResult = new AspNet.IdentityResult(err);
                    }
                    else
                        identityResult = AspNet.IdentityResult.Success; ;
                }
                else
                    identityResult = new AspNet.IdentityResult("Username or Email are already in use!");
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                identityResult = new AspNet.IdentityResult(ex.Message);
            }
            finally
            {
                oRecords.ReleaseObject();
                oDoc.ReleaseObject();
            }

            return identityResult;
        }

        public string GenerateConfirmationToken(string email)
        {
            try
            {
                var random = Utilities.RandomString();
                var ServiceInstance = ServiceLayerProvider.GetInstance();
                var password = new EMAILCONFIRMATION();
                password.Code = random;
                password.Name = email;
                ServiceInstance.CurrentServicelayerInstance.AddToEMAILCONFIRMATION(password);
                ServiceInstance.CurrentServicelayerInstance.SaveChanges();
                return random;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return string.Empty;
                
            }
        }

        public bool ConfirmEmail(string validationId, string email)
        {
            var confirmed = false;
            try
            {
                var ServiceInstance = ServiceLayerProvider.GetInstance();

                var result = ServiceInstance.CurrentServicelayerInstance.EMAILCONFIRMATION.Where(x => x.Code == validationId && x.Name == email).FirstOrDefault();
                if (result != null)
                {
                    var user = ServiceInstance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.EmailAddress == email).FirstOrDefault();
                    if (user != null)
                    {
                        user.U_Confirmed = "Y";
                        ServiceInstance.CurrentServicelayerInstance.UpdateObject(user);
                        var commit = ServiceInstance.CurrentServicelayerInstance.SaveChanges();
                        if (commit != null)
                            confirmed = true;
                    }
                }

            }
            catch (Exception ex)
            {
                confirmed = false;
            }
            return confirmed;
        }

        public string CreatePasswordReset(string email)
        {
            try
            {
                var random = Utilities.RandomString();
                var ServiceInstance = ServiceLayerProvider.GetInstance();
                var password = new PASSRESET();
                password.Code = random;
                password.Name = email;
                ServiceInstance.CurrentServicelayerInstance.AddToPASSRESET(password);
                ServiceInstance.CurrentServicelayerInstance.SaveChanges();
                return random;
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return string.Empty;
            }
        }

        public bool ResetPassword(string newPassword, string randomId, string email)
        {
            var creationResult = false;
            try
            {
                var ServiceInstance = ServiceLayerProvider.GetInstance();
                var searchPassword = Utilities.Encrypt(newPassword);
                var result = ServiceInstance.CurrentServicelayerInstance.PASSRESET.Where(x => x.Code == randomId.ToUpper() && x.Name == email).FirstOrDefault();
                if (result != null)
                {
                    var user = ServiceInstance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.EmailAddress == email).FirstOrDefault();
                    if (user != null)
                    {
                        user.U_Password = Utilities.Encrypt(newPassword);
                        ServiceInstance.CurrentServicelayerInstance.UpdateObject(user);
                        var commit = ServiceInstance.CurrentServicelayerInstance.SaveChanges();
                        if (commit != null)
                            creationResult = true;
                    }
                }

            }
            catch (Exception ex)
            {
                creationResult = false;
                Utilities.LogException(ex);
            }
            return creationResult;
        }

        private int GetSeriesCode()
        {
            int series = 0;
            try
            {
                var seriesCode = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.SeriesName);
                var result = dbHelper.ExecuteQuery($"SELECT \"Series\" FROM \"{base.databaseName}\".\"NNM1\" WHERE \"SeriesName\" = '{seriesCode}' ");
                while (result.Read())
                {
                    int.TryParse(result["Series"].ToString(), out series);
                }
            }
            catch(Exception ex)
            {
                Utilities.LogException(ex);
            }

            return series;
        }
    }
}

