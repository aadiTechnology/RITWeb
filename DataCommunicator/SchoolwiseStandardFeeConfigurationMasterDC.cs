// File Name       : SchoolwiseStandardFeeConfigurationMasterDC
// Purpose         : This class is used to manage SchoolwiseStandardFeeConfigurationMaster details.
// Date Of creation: 07/02/2008
// Author Name     : Anugandha

using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class SchoolwiseStandardFeeConfigurationMasterDC : DataCommunicatorBaseDC
    {
        #region Data Members

        private SchoolwiseStandardFeeConfigurationMasterStruct moSchoolwiseStandardFeeConfigurationMasterStruct;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationMasterDC()
        {
        }

        public SchoolwiseStandardFeeConfigurationMasterDC(int miSchoolwiseStandardFeeConfigurationId)
        {
            LoadSchoolwiseStandardFeeConfigurationMasterDetails(miSchoolwiseStandardFeeConfigurationId);
        }
        public SchoolwiseStandardFeeConfigurationMasterDC(int aiStandardId, int aiFeeypeId)
        {
            LoadSchoolwiseStandardFeeConfigurationMasterDetails(aiStandardId, aiFeeypeId);
        }
        /// <summary>
        /// This function is used to fetch the SchoolwiseStandardFeeConfigurationMaster Details 
        /// </summary>
        /// <param name="miSchoolwiseStandardFeeConfigurationId"></param>
        /// <returns>SqlDataReader</returns>
        private string FetchSchoolwiseStandardFeeConfigurationMasterDetailsFromDatabase(int aiStandardId, int aiFeeypeId)
        {
            string sSelectStatement = " SELECT  " +
            "Schoolwise_Standard_Fee_Configuration_Id" +
            ",Fee_Type_Id" +
            ",OldStudent_TotalFees" +
            ",NewStudent_TotalFees" +
            ",Standard_Id" +
            ",School_Id" +
            ",academic_Year_Id" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Standard_Fee_Configuration_Master" +
            " WHERE Standard_Id=" + aiStandardId +
            " AND Fee_Type_Id=" + aiFeeypeId;
            return sSelectStatement;
        }
        private void LoadSchoolwiseStandardFeeConfigurationMasterDetails(int aiStandardId, int aiFeeypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardFeeConfigurationMasterDetailsFromDatabase(aiStandardId, aiFeeypeId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Schoolwise_Standard_Fee_Configuration_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId = Convert.ToInt32(oDR["Schoolwise_Standard_Fee_Configuration_Id"]);
                            if (oDR["Fee_Type_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId = Convert.ToInt32(oDR["Fee_Type_Id"]);
                            if (oDR["OldStudent_TotalFees"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld = Convert.ToInt32(oDR["OldStudent_TotalFees"]);
                            if (oDR["NewStudent_TotalFees"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew = Convert.ToInt32(oDR["NewStudent_TotalFees"]);
                            if (oDR["Standard_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["academic_Year_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                        }
                    }
                }
            }
        }
        #endregion

        #region Structure

        public struct SchoolwiseStandardFeeConfigurationMasterStruct
        {

            public int miSchoolwiseStandardFeeConfigurationId;

            public int miFeeTypeId;

            public double miTotalFeesForOld;

            public double miTotalFeesForNew;

            public int miStandardId;

            public int miSchoolId;

            public int miacademicYearId;

            public string msIsDeleted;

            public System.DateTime mdtInsertDate;

            public string msInsertedByid;

            public System.DateTime mdtUpdateDate;

            public string msUpdatedById;

            public int miAmountForNewStudent;

            public int miAmountForOldStudent;

            public System.DateTime mdDueDate;

            public bool mbIsStudentPayFee;
        }

        #endregion

        #region Properties

        public SchoolwiseStandardFeeConfigurationMasterStruct SchoolwiseStandardFeeConfigurationMasterStructDetails
        {
            get
            {
                return moSchoolwiseStandardFeeConfigurationMasterStruct;
            }
            set
            {
                moSchoolwiseStandardFeeConfigurationMasterStruct = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This function is used to insert the SchoolwiseStandardFeeConfigurationMaster Details
        /// </summary>
        /// <returns></returns>
        public string InsertSchoolwiseStandardFeeConfigurationMaster()
        {

            string sInsertStatement = " INSERT " +
                                   " INTO " +
                                   " Schoolwise_Standard_Fee_Configuration_Master(" +
                                   " Fee_Type_Id " +
                                   " ,OldStudent_TotalFee" +
                                   " ,NewStudent_TotalFee" +
                                   " ,Standard_Id" +
                                   " ,School_Id" +
                                   " ,academic_Year_Id" +
                                   " ,Inserted_By_id" +
                                   " ) " +
                                   " VALUES(" +
                                   "  " + moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId +
                                   " , " + moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld +
                                   " , " + moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew +
                                   " , " + moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId +
                                   " , " + moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId +
                                   " , " + moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId +
                                   " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid, false) + "' " +
                                   " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to execute all statements in collection.
        /// </summary>
        /// <param name="aoArrayListInsertStatement"></param>
        public void UpdateFeeSubTypeRecords(ArrayList aoArrayListInsertStatement)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatement.ToArray(typeof(string)));
        }


        /// <summary>
        /// This function is used to update the SchoolwiseStandardFeeConfigurationMaster Details 
        /// </summary>
        public string UpdateSchoolwiseStandardFeeConfigurationMaster()
        {
            string sUpdateStatement = " UPDATE Schoolwise_Standard_Fee_Configuration_Master " +
                                        " SET " +
                                            "OldStudent_TotalFee= " + moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld +", "+
                                            " NewStudent_TotalFee= " + moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew + ", "+
                                            "Update_Date = dbo.GetLocalDate(DEFAULT)," +
                                            "Updated_By_Id = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid, false) + "' " +
                                        " WHERE " +
                                            " Is_Deleted=N'" + Constants.C_NO + "'" +
                                            " AND Schoolwise_Standard_Fee_Configuration_Id=" + moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId;
            return sUpdateStatement;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function is used to load the SchoolwiseStandardFeeConfigurationMaster Details
        /// </summary>
        /// <param name="miSchoolwiseStandardFeeConfigurationId"></param>
        private void LoadSchoolwiseStandardFeeConfigurationMasterDetails(int miSchoolwiseStandardFeeConfigurationId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardFeeConfigurationMasterDetailsFromDatabase(miSchoolwiseStandardFeeConfigurationId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Schoolwise_Standard_Fee_Configuration_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolwiseStandardFeeConfigurationId = Convert.ToInt32(oDR["Schoolwise_Standard_Fee_Configuration_Id"]);
                            if (oDR["Fee_Type_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miFeeTypeId = Convert.ToInt32(oDR["Fee_Type_Id"]);
                            if (oDR["OldStudent_TotalFees"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForOld = Convert.ToInt32(oDR["OldStudent_TotalFees"]);
                            if (oDR["NewStudent_TotalFees"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miTotalFeesForNew = Convert.ToInt32(oDR["NewStudent_TotalFees"]);
                            if (oDR["Standard_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["academic_Year_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeConfigurationMasterStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function is used to fetch the SchoolwiseStandardFeeConfigurationMaster Details 
        /// </summary>
        /// <param name="miSchoolwiseStandardFeeConfigurationId"></param>
        /// <returns>SqlDataReader</returns>
        private string FetchSchoolwiseStandardFeeConfigurationMasterDetailsFromDatabase(int miSchoolwiseStandardFeeConfigurationId)
        {
            string sSelectStatement = " SELECT  " +
            "Schoolwise_Standard_Fee_Configuration_Id" +
            ",Fee_Type_Id" +
            ",Total_Fees" +
            ",Standard_Id" +
            ",School_Id" +
            ",academic_Year_Id" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Standard_Fee_Configuration_Master" +
            " WHERE Schoolwise_Standard_Fee_Configuration_Id=" + miSchoolwiseStandardFeeConfigurationId;
            return sSelectStatement;
        }
        #endregion
    }

    public class SchoolwiseStandardFeeConfigurationMasterCollectionDC
    {
        #region Data Members

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeConfigurationMasterCollectionDC()
        {

        }

        public SchoolwiseStandardFeeConfigurationMasterCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all records from Schoolwise_Standard_Fee_Type_Master.
        /// </summary>
        /// <returns>dataset</returns>    
        public Int32 GetConfiguredFeeTypes(int aiStandardId)
        {
            string sQuery = "SELECT " +
                            " Count(*) " +
                            "FROM " +
                                " Schoolwise_Standard_Fee_Configuration_Master " +
                            " WHERE " +
                                " School_Id = N'" + miSchoolId + "'" +
                                " AND academic_Year_Id = " + miAcademicYearId +
                                " AND Standard_Id = N'" + aiStandardId + "'" +
                                " AND Is_Deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);

        }

        /// <summary>
        /// This method is used to get records from Schoolwise_Standard_Fee_Configuration_Master.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns>DataSet</returns>

        public DataSet GetConfiguredStandardFee(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_getFeeSubTypeDetails");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetStdFeeConfigurationDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardFeeTypeAssociation");
            }
        }
        #endregion
    }
}