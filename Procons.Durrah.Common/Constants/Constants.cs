﻿namespace Procons.Durrah.Common
{
    public class Constants
    {
        public struct ServiceLayer
        {
            public const string HanaSessionId = "HanaSessionId";
            public const string UserName = "UserName";
            public const string Email = "Email";
            public const string Password = "Password";
            public const string CompanyDB = "CompanyDB";
            public const string CardCode = "CardCode";
        }
        public struct ConfigurationKeys
        {
            public const string SapServer = "SapServer";
            public const string SLDServer = "SLDServer";
            public const string BaseUrl = "BaseUrl";
            public const string UserName = "UserName";
            public const string Password = "Password";
            public const string DatabaseName = "DatabaseName";
            public const string ServiceLayer = "ServiceLayer";
            public const string dbUsername = "dbUsername";
            public const string dbPassword = "dbPassword";
            public const string ResponseUrl = "ResponseUrl";
            public const string ResultUrl = "ResultUrl";
            public const string ErrorUrl = "ErrorUrl";
            public const string CancelUrl = "CancelUrl";
            public const string ApiBase = "ApiBase";
            public const string Alias = "Alias";
            public const string ResourcePath = "ResourcePath";
            public const string SeriesName = "SeriesName";

            public const string MailServer = "MailServer";
            public const string AdminMail = "AdminMail";
            public const string MailUid = "MailUid";
            public const string MailPwd = "MailPwd";
            public const string MailSSl = "MailSSl";
            public const string MailPort = "MailPort";

            public const string GoogleCaptchaUrl = "google:Url";
            public const string GoogleCaptchaSecretKey = "google:SecretKey";
        }
        public struct Resources
        {
            public const string Transaction_Completed = "Transaction_Completed";
            public const string KnetEmailConfirmation = "KnetEmailConfirmation";
            public const string TransactionCancellationBody = "TransactionCancellationBody";
            public const string TransactionErrorBody = "TransactionErrorBody"; 
            public const string Successfull_Transaction = "Successfull_Transaction";
            public const string Failed_Transaction = "Failed_Transaction";
            public const string Transaction_Cancelled = "Transaction_Cancelled";
            public const string ConfirmationSent = "ConfirmationSent";
            public const string DurraEmailConfirmation = "DurraEmailConfirmation";
            public const string ConfirmationBody = "ConfirmationBody";
            public const string PasswordResetBody = "PasswordResetBody";
            public const string DurraPasswordReset = "DurraPasswordReset";

            public const string WorkerStatus_Unavailable = "WorkerStatus_Unavailable";
            public const string WorkerStatus_Available = "WorkerStatus_Available";
            public const string WorkerStatus_Booked = "WorkerStatus_Booked";

            public const string WorkerUpdatedSuccessfully = "WorkerUpdatedSuccessfully";
            public const string WorkerAreadyCreated = "WorkerAreadyCreated";
            public const string WorkerCreatedSuccessfully = "WorkerCreatedSuccessfully";
            public const string ErrorOccured = "ErrorOccured";
            public const string WorkerBooked = "WorkerBooked";
        }
        public struct KNET
        {
            public const string TransactionDate = "TransactionDate";
            public const string ItemCode = "ItemCode";
            public const string TransactionAmount = "TransactionAmount";
            public const string ReferenceID = "ReferenceID";
            public const string PaymentID = "PaymentID";
            public const string MerchantTrackID = "MerchantTrackID";
            public const string CustomerCode = "CustomerCode";
            public const string CustomerName = "CustomerName";
        }

        public struct EmailKeys
        {
            public const string Email = "Email";
            public const string Result = "Result";
            public const string BaseUrl = "BaseUrl";
        }
    }
}
