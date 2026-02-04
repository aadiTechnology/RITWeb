// File Name       : SchoolwiseStandardFeeTypeMasterDC
// Purpose         : This class is used to manage SchoolwiseStandardFeeTypeMaster details.
// Date Of creation: 06/02/2008
// Author Name     : Anugandha 

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{

    public class SchoolwiseStandardFeeTypeMasterDC : DataCommunicatorBaseDC
    {

        #region Data member

        private SchoolwiseStandardFeeTypeMasterStruct moSchoolwiseStandardFeeTypeMasterStruct;

        #endregion

        #region Constructor

        public SchoolwiseStandardFeeTypeMasterDC()
        {
        }

        public SchoolwiseStandardFeeTypeMasterDC(int miSchoolWiseStandardFeeTypeId)
        {
            LoadSchoolwiseStandardFeeTypeMasterDetails(miSchoolWiseStandardFeeTypeId);
        }
        public SchoolwiseStandardFeeTypeMasterDC(int aiStandardId, int aiFeeTypeId)
        {
            LoadSchoolwiseStandardFeeTypeMasterDetails(aiStandardId, aiFeeTypeId);
        }

        #endregion

        #region Structure

        public struct SchoolwiseStandardFeeTypeMasterStruct
        {
            public int miSchoolWiseStandardFeeTypeId;
            public int miStandardId;
            public string msStandardFeeTypeName;
            public int miFeeTypeId;
            public int iInterval;
            public int miSchoolId;
            public int miacademicYearId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public string msInsertedByid;
            public DateTime mdtUpdateDate;
            public string msUpdatedById;
        }

        #endregion

        #region Properties

        public SchoolwiseStandardFeeTypeMasterStruct SchoolwiseStandardFeeTypeMasterStructDetails
        {
            get
            {
                return moSchoolwiseStandardFeeTypeMasterStruct;
            }
            set
            {
                moSchoolwiseStandardFeeTypeMasterStruct = value;
            }
        }

        #endregion

        #region Public Methods
        #region new

        /// <summary>
        /// This function is used to load the SchoolwiseStandardFeeTypeMaster Details 
        /// </summary>
        /// <param name="miSchoolWiseStandardFeeTypeId"></param>
        private void LoadSchoolwiseStandardFeeTypeMasterDetails(int aiStandardId, int aiFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardFeeTypeMasterDetailsFromDatabase(aiStandardId, aiFeeTypeId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                FillStruct(oDR);
              
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oDR"></param>
        private void FillStruct(SqlDataReader oDR)
        {
            if (oDR != null)
            {
                while (oDR.Read())
                {
                    if (oDR["SchoolWise_Standard_FeeType_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId = Convert.ToInt32(oDR["SchoolWise_Standard_FeeType_Id"]);
                    if (oDR["Standard_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                    if (oDR["Fee_Type_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId = Convert.ToInt32(oDR["Fee_Type_Id"]);
                    if (oDR["School_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                    if (oDR["academic_Year_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                    if (oDR["Is_Deleted"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                    if (oDR["Insert_Date"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                    if (oDR["Inserted_By_id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                    if (oDR["Update_Date"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                    if (oDR["Updated_By_Id"] != DBNull.Value)
                        moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                }
            }
        }
        /// <summary>
        /// This function is used to fetch the SchoolwiseStandardFeeTypeMaster Details
        /// </summary>
        /// <param name="miSchoolWiseStandardFeeTypeId"></param>
        /// <returns>SqlDataReader</returns>
        private string FetchSchoolwiseStandardFeeTypeMasterDetailsFromDatabase(int aiStandardId, int aiFeeTypeId)
        {
            string sSelectStatement = " SELECT  " +
                                        "SchoolWise_Standard_FeeType_Id" +
                                        ",Standard_Id" +
                                        ",Fee_Type_Id" +
                                        ",School_Id" +
                                        ",academic_Year_Id" +
                                        ",Is_Deleted" +
                                        ",Insert_Date" +
                                        ",Inserted_By_id" +
                                        ",Update_Date" +
                                        ",Updated_By_Id" +
                                        " FROM " +
                                        " Schoolwise_Standard_FeeType_Master" +
                                        " WHERE " +
                                        " Standard_Id=" + aiStandardId +
                                        " AND Fee_Type_Id=" + aiFeeTypeId;
            return sSelectStatement;
            
        }
        #endregion

        /// <summary>
        /// This function is used to insert the SchoolwiseStandardFeeTypeMaster Details
        /// </summary>
        /// <returns></returns>
        public string InsertSchoolwiseStandardFeeTypeMaster()
        {
            string sInsertStatement = " INSERT INTO " +
                                      " Schoolwise_Standard_FeeType_Master " +
                                      " (" +
                                      " Standard_Id" +
                                      " ,Fee_Type_Id" +
                                      " ,Interval" +
                                      " ,School_Id" +
                                      " ,academic_Year_Id" +
                                      " ,Inserted_By_id" +
                                      " ) " +
                                      " VALUES " +
                                      " (" +
                                         " " + moSchoolwiseStandardFeeTypeMasterStruct.miStandardId +
                                         " , " + moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId +
                                         " , " + moSchoolwiseStandardFeeTypeMasterStruct.iInterval +
                                         " , " + moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId +
                                         " , " + moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId +
                                         " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid, false) + "' " +
                                      " )";

            return sInsertStatement;
        }


        /// <summary>
        /// This function is used to update the SchoolwiseStandardFeeTypeMaster Details
        /// </summary>
        public void UpdateSchoolwiseStandardFeeTypeMaster()
        {
            string sUpdateStatement = " UPDATE " +
                                      " Schoolwise_Standard_FeeType_Master " +
                                      " SET " +
                                      "Standard_Id= " + moSchoolwiseStandardFeeTypeMasterStruct.miStandardId +
                                      ",Fee_Type_Id= " + moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId +
                                      ",School_Id= " + moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId +
                                      ",academic_Year_Id= " + moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId +
                                      ",Is_Deleted= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeTypeMasterStruct.msIsDeleted, false) + "' " +
                                      ",Insert_Date= N'" + moSchoolwiseStandardFeeTypeMasterStruct.mdtInsertDate + "' " +
                                      ",Inserted_By_id= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid, false) + "' " +
                                      ",Update_Date= N'" + moSchoolwiseStandardFeeTypeMasterStruct.mdtUpdateDate + "' " +
                                      ",Updated_By_Id= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById, false) + "' " +
                                      "" +
                                        " WHERE " +
                                        " SchoolWise_Standard_FeeType_Id=" + moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This function is used to updtae the SchoolwiseStandardFeeTypeMaster Details 
        /// </summary>
        public string UpdateStandardFeeTypeMaster()
        {
            string sUpdateStatement = "UPDATE " +
                                      " Schoolwise_Standard_FeeType_Master " +
                                        " SET " +
                                      "Interval= " + moSchoolwiseStandardFeeTypeMasterStruct.iInterval +
                                      ",Update_Date= dbo.GetLocalDate(DEFAULT)" +
                                      ",Updated_By_Id= " + moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById+ 
                                      "" +
                                        " WHERE " +
                                        " SchoolWise_Standard_FeeType_Id=" + moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId +
                                            " AND Fee_Type_Id = " + moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId + 
                                            " AND is_deleted = N'" + Constants.C_NO + "'"; ;

            return sUpdateStatement;
        }

        /// <summary>
        /// This function is used to delete the SchoolwiseStandardFeeTypeMaster Details 
        /// </summary>
        public string DeleteSchoolwiseStandardFeeTypeMaster()
        {
            string sDeleteStatement = "DELETE FROM Schoolwise_Standard_FeeType_Master " +
                                       " WHERE " +
                                        " SchoolWise_Standard_FeeType_Id=" + moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId +
                                         " AND Standard_Id  = " + moSchoolwiseStandardFeeTypeMasterStruct.miStandardId +
                                            " AND Fee_Type_Id = " + moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId + 
                                            " AND is_deleted = N'" + Constants.C_NO + "'"; ;                                     

            return sDeleteStatement;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function is used to load the SchoolwiseStandardFeeTypeMaster Details 
        /// </summary>
        /// <param name="miSchoolWiseStandardFeeTypeId"></param>
        private void LoadSchoolwiseStandardFeeTypeMasterDetails(int miSchoolWiseStandardFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardFeeTypeMasterDetailsFromDatabase(miSchoolWiseStandardFeeTypeId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["SchoolWise_Standard_FeeType_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.miSchoolWiseStandardFeeTypeId = Convert.ToInt32(oDR["SchoolWise_Standard_FeeType_Id"]);
                            if (oDR["Standard_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                            if (oDR["Fee_Type_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.miFeeTypeId = Convert.ToInt32(oDR["Fee_Type_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["academic_Year_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseStandardFeeTypeMasterStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function is used to fetch the SchoolwiseStandardFeeTypeMaster Details
        /// </summary>
        /// <param name="miSchoolWiseStandardFeeTypeId"></param>
        /// <returns>SqlDataReader</returns>
        private string  FetchSchoolwiseStandardFeeTypeMasterDetailsFromDatabase(int miSchoolWiseStandardFeeTypeId)
        {
            string sSelectStatement = " SELECT  " +
                                        "SchoolWise_Standard_FeeType_Id" +
                                        ",Standard_Id" +
                                        ",Fee_Type_Id" +
                                        ",School_Id" +
                                        ",academic_Year_Id" +
                                        ",Is_Deleted" +
                                        ",Insert_Date" +
                                        ",Inserted_By_id" +
                                        ",Update_Date" +
                                        ",Updated_By_Id" +
                                        " FROM " +
                                        " Schoolwise_Standard_FeeType_Master" +
                                        " WHERE " +
                                        " SchoolWise_Standard_FeeType_Id=" + miSchoolWiseStandardFeeTypeId;
            return sSelectStatement;
        }

        #endregion

    }

    public class SchoolwiseStandardFeeTypeMasterCollectionDC
    {

        #region Data Members

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        #endregion

        #region Constructors

        public SchoolwiseStandardFeeTypeMasterCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public SchoolwiseStandardFeeTypeMasterCollectionDC(int aiSchoolId, int aiAcademicYearId)
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
        public DataSet GetAllFeeTypesForStandard(int aiStandardId)
        {

            StringBuilder oSB = new StringBuilder();
            oSB.Append("SELECT " +
                            " Fee_Type_Id " +
                            " , Fee_Type " +
                            " , SchoolWise_Standard_FeeType_Id " +
                            " , Original_Fee_Type_Id " +
                            "FROM " +
                                " vw_Standard_FeeType " +
                            " WHERE " +
                                " School_Id = " + miSchoolId +
                                " AND Standard_Id = " + aiStandardId +
                                " AND academic_Year_Id = " + miAcademicYearId +
                                " AND Is_Deleted = N'" + Constants.C_NO + "';");
            oSB.Append("SELECT Total_Fees, SchoolWise_Standard_FeeType_Id FROM  vw_standard_FeeType_Config " +
                      " WHERE   " +
                      " School_Id = " + miSchoolId +
                                " AND Standard_Id = " + aiStandardId +
                                " AND academic_Year_Id = " + miAcademicYearId +
                                " AND Is_Deleted = N'" + Constants.C_NO + "';");

            oSB.Append("SELECT " +
                         " Late_Fee " +
                         " , Due_Date1 " +
                         " , Due_Date2 " +
                         " , SchoolWise_Standard_FeeType_Id " +
                         " , Day " +
                         " , Interval " +
                       " FROM  vw_LateFeeConfiguration " +
                       " WHERE   " +
                       " School_Id = " + miSchoolId +
                                 " AND Standard_Id = " + aiStandardId +
                                 " AND academic_Year_Id = " + miAcademicYearId + ";");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(oSB.ToString());

        }

        public DataSet GetStdExamAssociation()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardFeesAssociation");
            }
        }
        #endregion
    }

}
