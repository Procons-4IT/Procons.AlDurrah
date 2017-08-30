﻿namespace Procons.Durrah.Main
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
            ServiceLayerProvider loginInstance = new ServiceLayerProvider();
            loginInstance.Login();
            var encryptedPassword = Utilities.Encrypt(password);
            var result = loginInstance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.CardCode == userName & x.U_Password == encryptedPassword).FirstOrDefault();
            if (result != null)
            {
                user = new ApplicationUser()
                {
                    UserName = result.CardCode,
                    EmailConfirmed = true,
                    FirstName = result.CardName,
                    LastName = result.CardName,
                    UserType = result.CardType
                };
            }
            return user;
        }

        public List<Worker> GetWorkers([Optional]string agent)
        {
            var ServiceInstance = ServiceLayerProvider.GetInstance();

            var workers = ServiceInstance.CurrentServicelayerInstance.WORKERSUDO.ToList<WORKERS>();
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

        public AspNet.IdentityResult CreateUser(ApplicationUser user)
        {
            AspNet.IdentityResult identityResult = null;
            var password = Utilities.Encrypt(user.Password);
            var oDoc = base.B1Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners) as SAPbobsCOM.BusinessPartners;
            oDoc.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
            oDoc.Series = GetSeriesCode();
            oDoc.UserFields.Fields.Item("U_Password").Value = password;
            oDoc.UserFields.Fields.Item("U_UserName").Value = user.UserName;
            oDoc.CardName = $"{user.FirstName} {user.LastName}";
            oDoc.EmailAddress = user.Email;
            if (oDoc.Add() != 0)
            {

                var err = base.B1Company.GetLastErrorDescription();
                identityResult = new AspNet.IdentityResult(err);
            }
            else
            {
                identityResult = AspNet.IdentityResult.Success; ;
            }

            //ServiceLayerProvider slInstance = new ServiceLayerProvider();
            //slInstance.Login();

            //var businessPartner = new BusinessPartner();
            //try
            //{
            //    var password = Utilities.Encrypt(user.Password);

            //    businessPartner.CardType = "C";
            //    businessPartner.Series = GetSeriesCode();
            //    businessPartner.CardName = $"{user.FirstName} {user.LastName}";
            //    businessPartner.U_Password = password;
            //    businessPartner.U_UserName = user.UserName;
            //    businessPartner.EmailAddress = user.Email;

            //    //instance.Login();
            //    //ServiceInstance = ServiceLayerProvider.GetInstance();

            //    slInstance.CurrentServicelayerInstance.AddToBusinessPartners(businessPartner);
            //    var result = slInstance.CurrentServicelayerInstance.SaveChanges();

            //    if (result != null)
            //        identityResult = new AspNet.IdentityResult();
            //}
            //catch (Exception ex)
            //{
            //    slInstance.CurrentServicelayerInstance.Detach(businessPartner);
            //    identityResult = new AspNet.IdentityResult(ex.Message);

            //}
            return identityResult;
        }

        private int GetSeriesCode()
        {
            int series = 0;
            var seriesCode = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.SeriesName);
            var result = dbHelper.ExecuteQuery($"SELECT \"Series\" FROM \"{base.databaseName}\".\"NNM1\" WHERE \"SeriesName\" = '{seriesCode}' ");
            while (result.Read())
            {
                int.TryParse(result["Series"].ToString(), out series);
            }
            return series;
        }
    }
}

