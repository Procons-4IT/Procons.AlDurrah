
using Common=Procons.Durrah.Common;
namespace Procons.Durrah.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Procons.Durrah.Common;

    //public class EmailService : IIdentityMessageService
    public class EmailService 
    {
        public void SendAsync(IdentityMessage identityMessage)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient();
            message.To.Add(identityMessage.Destination);

            message.From = new System.Net.Mail.MailAddress(Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.AdminMail));

            message.Subject = identityMessage.Subject;
            message.Body = identityMessage.Body;
            message.IsBodyHtml = true;
            SmtpServer.Timeout = 2000000;
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.MailUid), Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.MailPwd));
            SmtpServer.Host = Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.MailServer);
            SmtpServer.Port =Convert.ToInt32( Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.MailPort));
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(message);
        }

       
    }
}
