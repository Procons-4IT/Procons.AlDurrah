namespace Procons.Durrah.Main
{
    using Procons.Durrah.Common;
    using Procons.Durrah.Main.B1ServiceLayer.SAPB1;
    using System.Collections.Generic;
    using System.Linq;

    using System.Runtime.InteropServices;
    public class LoginProvider
    {
        public ApplicationUser FindUser(string userName, string password)
        {
            var instance = ServiceLayerProvider.GetInstance();
            ApplicationUser user = null;
            var result = instance.CurrentServicelayerInstance.BusinessPartners.Where(x => x.CardCode == userName).FirstOrDefault();
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
    }
}

