using e24PaymentPipeLib;
using Procons.Durrah.Common;
using System;
using System.ServiceModel.Activation;

namespace Procons.Durrah.Api.Services
{
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class KnetService : IKnetService
    {
        public Transaction CallKnetGateway(Transaction transaction)
        {
            short TransVal;
            string varPaymentID, varErrorMsg, varRawResponse;

            transaction.TrackID = (new Random().Next(10000000) + 1).ToString();
            try
            {
                e24PaymentPipeCtlClass payment = new e24PaymentPipeCtlClass();
                payment.Action = "1";
                payment.Amt = transaction.Amount;
                payment.Currency = "414";
                payment.Language = "USA";
                payment.ResponseUrl = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResponseUrl);
                payment.ErrorUrl = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ErrorUrl);
                payment.TrackId = transaction.TrackID;
                payment.ResourcePath = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.ResourcePath);
                payment.Alias = Utilities.GetConfigurationValue(Constants.ConfigurationKeys.Alias);
                payment.Udf2 = transaction.CardCode;
                payment.Udf3 = transaction.Code;
                payment.Udf4 = transaction.WorkerCode;

                TransVal = payment.PerformInitTransaction();
                varRawResponse = payment.RawResponse;
                varPaymentID = payment.PaymentId;
                transaction.PaymentPage = payment.PaymentPage;
                varErrorMsg = payment.ErrorMsg;
                transaction.PaymentID = varPaymentID;

                if (TransVal != 0)
                {
                    Utilities.LogException(varErrorMsg);
                    return null;
                }
                else
                {
                    return transaction;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
                return null;
            }


        }
    }
}
