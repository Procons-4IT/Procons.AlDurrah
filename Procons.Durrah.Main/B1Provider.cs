using System;
namespace Procons.Durrah.Main
{
    public class B1Provider : ProviderBase
    {
        public void InitializeTables()
        {
            try
            {
                base.B1Company.StartTransaction();
         
                base.AddTable("Workers", "Workers", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                base.AddField("ItemCode", "ItemCode", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Serial", "Serial", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Agent", "Agent", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("BirthDate", "BirthDate", "Workers", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Gender", "Gender", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Nationality", "Nationality", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Religion", "Religion", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("MaritalStatus", "MaritalStatus", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Language", "Language", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Photo", "Photo", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Weight", "Weight", "Workers", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Height", "Height", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Education", "Education", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Passport", "Passport", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Video", "Video", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 250, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("PassportNumber", "PassportNumber", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("PassportIssDate", "PassportIssDate", "Workers", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("PassportExpDate", "PassportExpDate", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("PassportPoIssue", "PassportPoIssue", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("CivilId", "CivilId", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);
                base.AddField("Status", "Status", "Workers", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, true);

                base.AddField("PaymentID", "Payment Id", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("Result", "Payment Result", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("TranID", "Payment TranID", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("Auth", "Payment Auth", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("Ref", "Payment Ref", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("TrackID", "Payment TrackID", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);
                base.AddField("Status", "Payment Status", "ORDR", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, SAPbobsCOM.BoYesNoEnum.tNO, false);

                base.B1Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }
            catch (Exception ex)
            {
                base.B1Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            }
        }
    }
}
