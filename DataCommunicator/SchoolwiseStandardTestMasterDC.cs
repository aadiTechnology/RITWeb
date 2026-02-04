// File Name       :SchoolwiseStandardTestMasterDC.cs
// Purpose         :This class is used to manage SchoolwiseStandardTestMaster details.
// Date Of creation:1/31/2008
// Author Name     :Anugandha

using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{

    public class SchoolwiseStandardTestMasterDC : DataCommunicatorBaseDC
    {

        #region Data Member

        private SchoolwiseStandardTestMasterStruct moSchoolwiseStandardTestMasterStruct;

        #endregion

        #region Structure

        public struct SchoolwiseStandardTestMasterStruct
        {

            public int miSchoolwiseStandardTestId;

            public string msStandardTestName;

            public int miStandardId;

            public int miSchoolWiseTestId;

            public int miSchoolId;

            public int miacademicYearId;

            public string msIsDeleted;

            public System.DateTime mdtInsertDate;

            public string msInsertedByid;

            public System.DateTime mdtUpdateDate;

            public string msUpdatedById;
        }

        #endregion

        #region Constructors

        public SchoolwiseStandardTestMasterDC()
        {
        }

        public SchoolwiseStandardTestMasterDC(int miSchoolwiseStandardTestId)
        {
            LoadSchoolwiseStandardTestMasterDetails(miSchoolwiseStandardTestId);
        }
        public SchoolwiseStandardTestMasterDC(int aiStandardId, int aiExamId)
        {
            LoadSchoolwiseStandardTestMasterDetails(aiStandardId, aiExamId);
        }

        #endregion

        #region Properties

        public SchoolwiseStandardTestMasterStruct SchoolwiseStandardTestMasterStructDetails
        {
            get
            {
                return moSchoolwiseStandardTestMasterStruct;
            }
            set
            {
                moSchoolwiseStandardTestMasterStruct = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to insert records into Schoolwise_Standard_Test_Master.
        /// </summary>
        /// <returns>string</returns>
        public string InsertSchoolwiseStandardTestMaster()
        {
            string sInsertStatement = "INSERT INTO " +
                                        "Schoolwise_Standard_Test_Master(" +
                                        "Standard_Id" +
                                        ",SchoolWise_Test_Id" +
                                        ",School_Id" +
                                        ",academic_Year_Id" +
                                        ",Inserted_By_id" +
                                        ",Updated_By_Id" +
									    ",Sort_Order" +
                                        ")SELECT  " +
                                    " N'" + moSchoolwiseStandardTestMasterStruct.miStandardId + "'" +
                                    " , N'" + moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId + "'" +
                                    " , N'" + moSchoolwiseStandardTestMasterStruct.miSchoolId + "'" +
                                    " , N'" + moSchoolwiseStandardTestMasterStruct.miacademicYearId + "'" +
                                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardTestMasterStruct.msInsertedByid, false) + "'" +
                                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardTestMasterStruct.msUpdatedById, false) + "'" +
                                    " , ISNULL(MAX(Sort_Order),0)+1  " +
                                    " FROM   Schoolwise_Standard_Test_Master";
            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to update records of Schoolwise_Standard_Test_Master.
        /// </summary>
        public void UpdateSchoolwiseStandardTestMaster()
        {
            string sUpdateStatement = " UPDATE Schoolwise_Standard_Test_Master SET " +
                                       " Standard_Id = " + moSchoolwiseStandardTestMasterStruct.miStandardId +
                                       " ,SchoolWise_Test_Id = " + moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId +
                                       " ,School_Id = " + moSchoolwiseStandardTestMasterStruct.miSchoolId +
                                       " ,academic_Year_Id = " + moSchoolwiseStandardTestMasterStruct.miacademicYearId +
                                       " ,Is_Deleted = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardTestMasterStruct.msIsDeleted, false) + "' " +
                                       " ,Insert_Date = N'" + moSchoolwiseStandardTestMasterStruct.mdtInsertDate + "' " +
                                       " ,Inserted_By_id = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardTestMasterStruct.msInsertedByid, false) + "' " +
                                       " ,Update_Date = N'" + moSchoolwiseStandardTestMasterStruct.mdtUpdateDate + "' " +
                                       " ,Updated_By_Id = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardTestMasterStruct.msUpdatedById, false) + "' " +
                                       " " +
                                       " WHERE " +
                                       "Schoolwise_Standard_Test_Id = " + moSchoolwiseStandardTestMasterStruct.miSchoolwiseStandardTestId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete records from Schoolwise_Standard_Test_Master.
        /// </summary>
        /// <returns>string</returns>
        public string DeleteSchoolwiseStandardTestMaster()
        {
            string sDeleteStatement = "";
            sDeleteStatement = " DELETE FROM Schoolwise_Standard_Test_Master " +
                               "  WHERE " +
                                    " Standard_Id  = N'" + moSchoolwiseStandardTestMasterStruct.miStandardId + "'" +
                                    " AND Schoolwise_Test_Id = N'" + moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sDeleteStatement;
        }

        #endregion

        #region Private Methods

        private void LoadSchoolwiseStandardTestMasterDetails(int aiStandardId, int aiExamId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardTestMasterDetailsFromDatabase(aiStandardId, aiExamId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                SetObject(oDR);
                
            }
        }
        /// <summary>
        // This method is used to load the SchoolwiseStandardTestMaster Details
        /// </summary>
        private void LoadSchoolwiseStandardTestMasterDetails(int miSchoolwiseStandardTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardTestMasterDetailsFromDatabase(miSchoolwiseStandardTestId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                SetObject(oDR);
                
            }
        }
        private void SetObject(SqlDataReader oDR)
        {
            if (oDR != null)
            {
                while (oDR.Read())
                {
                    if (oDR["Schoolwise_Standard_Test_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.miSchoolwiseStandardTestId = Convert.ToInt32(oDR["Schoolwise_Standard_Test_Id"]);
                    if (oDR["Standard_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"]);
                    if (oDR["SchoolWise_Test_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.miSchoolWiseTestId = Convert.ToInt32(oDR["SchoolWise_Test_Id"]);
                    if (oDR["School_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                    if (oDR["academic_Year_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.miacademicYearId = Convert.ToInt32(oDR["academic_Year_Id"]);
                    if (oDR["Is_Deleted"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                    if (oDR["Insert_Date"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                    if (oDR["Inserted_By_id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.msInsertedByid = Convert.ToString(oDR["Inserted_By_id"]);
                    if (oDR["Update_Date"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                    if (oDR["Updated_By_Id"] != DBNull.Value)
                        moSchoolwiseStandardTestMasterStruct.msUpdatedById = Convert.ToString(oDR["Updated_By_Id"]);
                }
            }
        }
        /// <summary>
        // This function is used to fetch the SchoolwiseStandardTestMaster Details
        /// </summary>
        /// <returns>SqlDataReader</returns>
        private string FetchSchoolwiseStandardTestMasterDetailsFromDatabase(int miSchoolwiseStandardTestId)
        {
            string sSelectStatement = " SELECT  " +
            "Schoolwise_Standard_Test_Id" +
            ",Standard_Id" +
            ",SchoolWise_Test_Id" +
            ",School_Id" +
            ",academic_Year_Id" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Standard_Test_Master" +
            " WHERE Schoolwise_Standard_Test_Id=" + miSchoolwiseStandardTestId;
            return sSelectStatement;            
        }
        /// <summary>
        // This function is used to fetch the SchoolwiseStandardTestMaster Details
        /// </summary>
        /// <returns>SqlDataReader</returns>
        private string FetchSchoolwiseStandardTestMasterDetailsFromDatabase(int aiStandardId, int aiTeastId)
        {
            string sSelectStatement = " SELECT  " +
            "Schoolwise_Standard_Test_Id" +
            ",Standard_Id" +
            ",SchoolWise_Test_Id" +
            ",School_Id" +
            ",academic_Year_Id" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Standard_Test_Master" +
            " WHERE Standard_Id=" + aiStandardId +
            " AND SchoolWise_Test_Id=" + aiTeastId;
            return sSelectStatement;            
        }

        #endregion


        public static int GetTestCount(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement="SELECT "+
                                    " COUNT(*) "+
                                    " FROM SchoolWise_Test_Master"+
                                    " WHERE Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND Term_Id=2" +
                                    " AND School_Id="+aiSchoolId+
                                    " AND academic_year_id ="+aiAcademicYearId;
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iCount;
        }

        public static int GetGeneratedTestCount(int aiSchoolId, int aiAcademicYearId, int aiStandardDivId)
        {
            string sSelectStatement = "SELECT DISTINCT "+
                                      " COUNT(SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id)"+
                                      " FROM "+
                                      " SchoolWise_StanderedDivision_Test_Master "+
                                      " INNER JOIN SchoolWise_Test_Master on SchoolWise_Test_Master.SchoolWise_Test_Id=SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id "+
                                      " WHERE "+
                                      " Standerd_division_Id="+aiStandardDivId+
                                      " AND Is_Published= N'" + Constants.C_YES + "'" +
                                      " AND  SchoolWise_Test_Master.Term_Id=2"+
                                      " AND SchoolWise_StanderedDivision_Test_Master.academic_year_id="+ aiAcademicYearId+
                                      " AND SchoolWise_StanderedDivision_Test_Master.School_id=" + aiSchoolId;
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iCount;
        }

        public static int IsStandardWithGrade(int aiStandardDivId, int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = "SELECT COUNT(Standard_Id)"+
                                                " FROM SchoolWise_Standard_Division_Master "+
                                                " WHERE SchoolWise_Standard_Division_Id= "+aiStandardDivId+
                                                " AND academic_year_id= " +aiAcademicYearId+
                                                " AND School_Id= "+aiSchoolId+
                                                " AND Standard_Id IN (SELECT Standard_Id "+
                                                                        " FROM "+
                                                                        " StandardsWithOnlyGradesSettings)";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iCount;
        }
    }

    public class SchoolwiseStandardTestMasterCollectionDC
    {

        #region Data Members

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        #endregion

        #region Constructors

        public SchoolwiseStandardTestMasterCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public SchoolwiseStandardTestMasterCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to get all standard names from
        ///  Schoolwise_Standard_Test_Master.
        /// </summary>
        /// <returns>dataset</returns>
        public DataTable GetConfiguredStandardName()
        {
            string sQuery = " SELECT " +
                                " Standard_Master.Standard_Name " +
                                " , Standard_Master.Standard_Id " +
                                " , Standard_Master.Original_Standard_Id " +
                            " FROM " +
                                " Standard_Master " +
                            " INNER JOIN " +
                                " Schoolwise_Standard_Test_Master " +
                            " ON " +
                                " Standard_Master.Standard_Id = Schoolwise_Standard_Test_Master.Standard_Id " +
                            " WHERE " +
                                " Schoolwise_Standard_Test_Master.School_Id = N'" + miSchoolId + "'" +
                                " AND Schoolwise_Standard_Test_Master.academic_Year_Id = " + miAcademicYearId +
                                " AND Schoolwise_Standard_Test_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                            " ORDER BY " +
                                " Standard_Master.Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        /// <summary>
        /// Get all test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForStandard(int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllTestsForStandard");
            }
        }
        public DataSet GetStdExamAssociation()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardExamAssociation");
            }
        }

        /// <summary>
        /// Used to update test order of standard.
        /// </summary>
        /// <param name="ischoolId"></param>
        /// <param name="iAcademicYearId"></param>
        /// <param name="iStandardId"></param>
        /// <param name="sXmlExamOrder"></param>
        public void UpdateExamSortOrder(int ischoolId, int iAcademicYearId, int iStandardId, string sXmlExamOrder)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", ischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_ID", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sXmlExamOrder", sXmlExamOrder, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStandardExamsSortOrder");
            }
        }

        #endregion
    }
}
