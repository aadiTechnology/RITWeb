// Class Name       :- SchoolwiseStandardFeeConfigurationDetailsDC
// Purpose          :- This class is used to manage SchoolwiseStandardFeeConfigurationDetails details.
// Date Of creation :- 2/7/2008
// Author Name      :- Anugandha


using System;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{


    public class SchoolwiseStandardFeeConfigurationDetailsDC : DataCommunicatorBaseDC
    {
        #region Data Members

        private SchoolwiseStandardFeeConfigurationDetailsStruct moSchoolwiseStandardFeeConfigurationDetailsStruct;

        #endregion

        public SchoolwiseStandardFeeConfigurationDetailsDC()
        {
        }

        public SchoolwiseStandardFeeConfigurationDetailsDC(int miSchoolwiseStandardFeeConfigurationDetailId)
        {
            LoadSchoolwiseStandardFeeConfigurationDetailsDetails(miSchoolwiseStandardFeeConfigurationDetailId);
        }

        public virtual SchoolwiseStandardFeeConfigurationDetailsStruct SchoolwiseStandardFeeConfigurationDetailsStructDetails
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationDetailsStruct;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationDetailsStruct = value;
            }
        }


        // This function is used to insert the SchoolwiseStandardFeeConfigurationDetails Details
        public string InsertSchoolwiseStandardFeeConfigurationDetails()
        {
            string sConfigurationID = "";

            if (moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationId == 0)
                sConfigurationID = Constants.S_LAST_INSERTED_P_KEY;
            else
                sConfigurationID = moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationId.ToString();

            string sInsertStatement = "INSERT INTO Schoolwise_Standard_Fee_Configuration_Details(" +
                                          " Schoolwise_Standard_Fee_Configuration_Id" +
                                          " ,Fee_SubType_Id" +
                                          " ,Fee_AmountOld" +
                                          " ,Fee_AmountNew" +
                                          " ,Standard_Id" +
                                          " ,School_Id" +
                                          " ,academic_Year_Id" +
                                          " ,Inserted_By_id" +
                                          " ) " +
                                      " VALUES(" +
                                         sConfigurationID +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeSubTypeId +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountOld +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountNew +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miStandardId +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolId +
                                         " , " + moSchoolwiseStandardFeeConfigurationDetailsStruct.miacademicYearId +
                                         " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeConfigurationDetailsStruct.msInsertedByid, false) + "' " +

                                      " )";

            return sInsertStatement;
        }

        // This function is used to load the SchoolwiseStandardFeeConfigurationDetails Details
        private void LoadSchoolwiseStandardFeeConfigurationDetailsDetails(int miSchoolwiseStandardFeeConfigurationDetailId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardFeeConfigurationDetailsDetailsFromDatabase(miSchoolwiseStandardFeeConfigurationDetailId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Schoolwise_Standard_Fee_Configuration_Detail_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationDetailId = Convert.ToInt32(oDR["Schoolwise_Standard_Fee_Configuration_Detail_Id"]);
                            if (oDR["Schoolwise_Standard_Fee_Configuration_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolwiseStandardFeeConfigurationId = Convert.ToInt32(oDR["Schoolwise_Standard_Fee_Configuration_Id"]);
                            if (oDR["Fee_SubType_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeSubTypeId = Convert.ToInt32(oDR["Fee_SubType_Id"]);
                            if (oDR["Fee_AmountOld"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountOld = Convert.ToInt32(oDR["Fee_AmountOld"]);
                            if (oDR["Fee_AmountNew"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miFeeAmountNew = Convert.ToInt32(oDR["Fee_AmountNew"]);
                            if (oDR["Standard_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["academic_Year_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationDetailsStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the SchoolwiseStandardFeeConfigurationDetails Details
        private string FetchSchoolwiseStandardFeeConfigurationDetailsDetailsFromDatabase(int miSchoolwiseStandardFeeConfigurationDetailId)
        {
            string sSelectStatement = " SELECT  " +
            "Schoolwise_Standard_Fee_Configuration_Detail_Id" +
            ",Schoolwise_Standard_Fee_Configuration_Id" +
            ",Fee_SubType_Id" +
            ",Fee_AmountOld" +
            ",Fee_AmountNew" +
            ",Standard_Id" +
            ",School_Id" +
            ",academic_Year_Id" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Standard_Fee_Configuration_Details" +
            " WHERE Schoolwise_Standard_Fee_Configuration_Detail_Id=" + miSchoolwiseStandardFeeConfigurationDetailId;
            return sSelectStatement;
        }

        public struct SchoolwiseStandardFeeConfigurationDetailsStruct
        {

            public int miSchoolwiseStandardFeeConfigurationDetailId;

            public int miSchoolwiseStandardFeeConfigurationId;

            public int miFeeSubTypeId;

            public double miFeeAmountNew;

            public double miFeeAmountOld;
            
            public double miTotalFeeAmountOld;

            public double miTotalFeeAmountNew;

            public int miStandardId;

            public int miSchoolId;

            public int miacademicYearId;

            public string msIsDeleted;

            public System.DateTime mdtInsertDate;

            public string msInsertedByid;

            public System.DateTime mdtUpdateDate;

            public string msUpdatedById;
        }
    }

    public class SchoolwiseStandardFeeConfigurationDetailsCollectionDC
    {

        #region Data Members

        int miSchoolId = 0;
        int miAcademicYearId = 0;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationDetailsCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        // This function is used to Fetch the SchoolwiseStandardFeeConfigurationDetails Details

        public static string GetPhysicalDeleteStatement(int aiConfigurationId)
        {
            string sDeleteStatement = "DELETE FROM Schoolwise_Standard_Fee_Configuration_Details " +
                                        " WHERE Schoolwise_Standard_Fee_Configuration_Id = " + aiConfigurationId +
                                        " AND Is_Deleted = N'" + Constants.C_NO + "'";
            return sDeleteStatement;
        }
    }
}
