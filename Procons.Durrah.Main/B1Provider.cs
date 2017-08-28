namespace Procons.Durrah.Main
{
    using System;
    using System.Data.Services.Client;

    public class B1Provider : ProviderBase
    {
        public string InitializeTables()
        {
            try
            {
                base.instance.Login();
                base.AddTableSL("WORKERS", "WORKERS", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                base.AddFieldSL("ItemCode", "ItemCode", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Serial", "Serial", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Agent", "Agent", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Age", "Age", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Float.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("BirthDate", "BirthDate", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Date.ToString(), SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Gender", "Gender", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Nationality", "Nationality", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Religion", "Religion", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("MaritalStatus", "MaritalStatus", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Language", "Language", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Photo", "Photo", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 200, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Weight", "Weight", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Numeric.ToString(), SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Height", "Height", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Education", "Education", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Passport", "Passport", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 200, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Video", "Video", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 250, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("PassportNumber", "PassportNumber", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("PassportIssDate", "PassportIssDate", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Date.ToString(), SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("PassportExpDate", "PassportExpDate", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("PassportPoIssue", "PassportPoIssue", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("CivilId", "CivilId", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);
                base.AddFieldSL("Status", "Status", "WORKERS", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), true);

                base.AddFieldSL("PaymentID", "Payment Id", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 100, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("Result", "Payment Result", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("TranID", "Payment TranID", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("Auth", "Payment Auth", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("Ref", "Payment Ref", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("TrackID", "Payment TrackID", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("Status", "Payment Status", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 20, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);
                base.AddFieldSL("Password", "Password", "OCRD", SAPbobsCOM.BoFieldTypes.db_Alpha.ToString(), 50, SAPbobsCOM.BoYesNoEnum.tNO.ToString(), false);

                base.AddUdo(new B1ServiceLayer.SAPB1.UserObjectsMD()
                {
                    TableName = "WORKERS",
                    Code = "WORKERSUDO",
                    Name = "WORKERSUDO"
                });
                instance.CurrentServicelayerInstance.SaveChanges();
                return instance.B1SessionId;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }


        }

    }
}
