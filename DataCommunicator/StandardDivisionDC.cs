using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Utility;
using MasterEntities;
using System.Collections.Generic;

namespace DataCommunicator
{

    public class StandardDivisionMasterDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure
        //structure for SchoolWise_Standard_division_Master
        public struct StandardDivisionStruct
        {
            public int miStandardDivisionId;
            public string msStandardDivisionName;
            public int miAcademicYearId;
            public int miStandardId;
            public int miDivisionId;
            public int miSchoolId;
            public int miInsertedByid;
            public int miUpdatedById;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private StandardDivisionStruct moStandardDivisionStruct;

        #endregion
        #region Properties

        public StandardDivisionStruct StandardDivisionStructDetails
        {

            get { return moStandardDivisionStruct; }
            set { moStandardDivisionStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public StandardDivisionMasterDC()
        {
        }
        public StandardDivisionMasterDC(int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchStandardDivisionMasterDataFromDatabase(aiStandardDivisionId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                LoadStandardDivisionMasterDetails(oDR);
            }
        }
        public StandardDivisionMasterDC(int aiStandardId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchStandardDivisionMasterDataFromDatabase(aiStandardId, aiDivisionId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                LoadStandardDivisionMasterDetails(oDR);
            }

        }
        #endregion

        #region Private Methods

        public void LoadStandardDivisionMasterDetails(SqlDataReader oDR)
        {

            if (oDR != null)
            {
                while (oDR.Read())
                {

                    if (oDR["SchoolWise_Standard_Division_Id"] != DBNull.Value)
                        moStandardDivisionStruct.miStandardDivisionId = Convert.ToInt32(oDR["SchoolWise_Standard_Division_Id"].ToString());
                    if (oDR["standard_id"] != DBNull.Value)
                        moStandardDivisionStruct.miStandardId = Convert.ToInt32(oDR["standard_id"].ToString());
                    if (oDR["division_id"] != DBNull.Value)
                        moStandardDivisionStruct.miDivisionId = Convert.ToInt32(oDR["division_id"].ToString());
                    if (oDR["academic_year_id"] != DBNull.Value)
                        moStandardDivisionStruct.miAcademicYearId = Convert.ToInt32(oDR["academic_year_id"].ToString());
                    if (oDR["school_id"] != DBNull.Value)
                        moStandardDivisionStruct.miSchoolId = Convert.ToInt32(oDR["school_id"].ToString());
                }
                oDR.Close();
            }
        }
        public string FetchStandardDivisionMasterDataFromDatabase(int aiStandardDivisionId)
        {

            string sSelectStatement = " SELECT  " +
                                        " SchoolWise_Standard_Division_Id " +
                                        " , school_id" +
                                        " , standard_id" +
                                        " , division_id" +
                                        " , academic_year_id" +
                                    " FROM  " +
                                        "schoolwise_standard_division_master " +
                                    " WHERE  " +
                                        " SchoolWise_Standard_Division_Id = " + aiStandardDivisionId +
                                        " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        public string FetchStandardDivisionMasterDataFromDatabase(int aiStandardId, int aiDivisionId)
        {

            string sSelectStatement = " SELECT  " +
                                        " SchoolWise_Standard_Division_Id " +
                                        " , school_id" +
                                        " , standard_id" +
                                        " , division_id" +
                                        " , academic_year_id" +
                                    " FROM  " +
                                        "schoolwise_standard_division_master " +
                                    " WHERE  " +
                                        " standard_id = " + aiStandardId +
                                        " AND division_id = " + aiDivisionId +
                                        " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        public DataTable GetStandardDivisionNamesForMessaging(int aiSchoolId, int aiAcademicYearId, int aiTeacherId, int aiTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardDivisions");
            }
        }


        #endregion

        #region Public Methods
       
        public DataTable GetStandardAndDivisionName(int aiSchoolId, int aiStandardDivisionId)
        {
            string sStandardDivisionName = " SELECT " +
                                           " Standard_Master.Standard_Name " +
                                           ", VSD.ClassName AS Division_Name " +
                                         " FROM " +
                                            " Standard_Master " +
                                         " INNER JOIN " +
                                            " SchoolWise_Standard_Division_Master " +
                                         " ON " +
                                            " Standard_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                         " INNER JOIN " +
                                            " Division_Master " +
                                         " ON " +
                                            " Division_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id " +
                                        " INNER JOIN vw_standard_division VSD"+
                                        " ON SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = VSD.SchoolWise_Standard_Division_Id"+
                                         " WHERE " +
                                            " SchoolWise_Standard_Division_Master.School_Id =" + aiSchoolId +
                                            " AND SchoolWise_Standard_Division_Master.Schoolwise_standard_division_Id = " + aiStandardDivisionId +
                                            " AND Division_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                            " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sStandardDivisionName);

        }
        
        #endregion

    }

    public class StandardDivisionCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        public StandardDivisionCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public StandardDivisionCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public DataSet GetAllStandards()
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT " +
                                   " -9999 as school_id" +
                                   " , original_standard_id " +
                                   " , standard_id " +
                                   " , standard_name " +
                               " FROM " +
                                    " standard_master " +
                               " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id is null " +
                                    " AND standard_id NOT IN " +
                                    " ( " +
                                     " SELECT  " +
                                           " original_standard_id " +
                                       " FROM " +
                                            " standard_master " +
                                       " WHERE " +
                                            " is_deleted = N'" + Constants.C_NO + "'" +
                                            " AND school_id = " + miSchoolId +
                                       " )" +
                               " UNION " +
                                " SELECT  " +
                                   " school_id " +
                                   " , original_standard_id " +
                                   " , standard_id " +
                                   " , standard_name " +
                               " FROM " +
                                    " standard_master " +
                               " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id = " + miSchoolId +
                                " ORDER BY " +
                                     " original_standard_id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
        }

        public DataTable GetAssociatedStandardsDivisions()
        {   
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAssocitedStandardDivision");
            }
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsDivisionsGorTest(int aiTestId)
        {
            string sSelectStatement =  "SELECT DISTINCT "+
                                            " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id"+
                                            " , Standard_Master.Standard_Name"+
                                            " , Division_Master.Division_Name"+
                                            " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name AS StandardDivision"+
                                            " , Standard_Master.Original_Standard_Id"+
                                            " , Division_Master.Original_Division_Id"+
                                        " FROM         SchoolWise_Standard_Division_Master INNER JOIN"+
                                        " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND "+
                                        " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN"+
                                        " Division_Master ON SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id AND "+
                                        " SchoolWise_Standard_Division_Master.School_Id = Division_Master.School_Id AND "+
                                        " SchoolWise_Standard_Division_Master.academic_year_id = Division_Master.academic_year_id INNER JOIN "+
                                        " SchoolWise_StanderedDivision_Test_Master ON "+
                                        " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = SchoolWise_StanderedDivision_Test_Master.Standerd_division_Id"+
                                        " AND SchoolWise_Standard_Division_Master.School_Id = SchoolWise_StanderedDivision_Test_Master.School_id AND "+
                                        " SchoolWise_Standard_Division_Master.academic_year_id = SchoolWise_StanderedDivision_Test_Master.Academic_Year_ID"+
                                    " WHERE     (SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (Standard_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (Division_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                        " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ")" +                                        
                                    " GROUP BY SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id"+
                                        " , Standard_Master.Standard_Name"+
                                        " , Division_Master.Division_Name"+
                                        " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name"+
                                        " , Standard_Master.Original_Standard_Id"+
                                        " , Division_Master.Original_Division_Id"+
                                        " , SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id"+
                                    " HAVING "+
                                        " (SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id = " + aiTestId + ")" +  
                                    " ORDER BY Standard_Master.Original_Standard_Id, Division_Master.Original_Division_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsDivisionsGorTest()
        {
            string sSelectStatement = "SELECT DISTINCT " +
                                            " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id" +
                                            " , Standard_Master.Standard_Name" +
                                            " , Division_Master.Division_Name" +
                                            " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name AS StandardDivision" +
                                            " , Standard_Master.Original_Standard_Id" +
                                            " , Division_Master.Original_Division_Id" +
                                        " FROM         SchoolWise_Standard_Division_Master INNER JOIN" +
                                        " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND " +
                                        " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN" +
                                        " Division_Master ON SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id AND " +
                                        " SchoolWise_Standard_Division_Master.School_Id = Division_Master.School_Id AND " +
                                        " SchoolWise_Standard_Division_Master.academic_year_id = Division_Master.academic_year_id INNER JOIN " +
                                        " SchoolWise_StanderedDivision_Test_Master ON " +
                                        " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = SchoolWise_StanderedDivision_Test_Master.Standerd_division_Id" +
                                        " AND SchoolWise_Standard_Division_Master.School_Id = SchoolWise_StanderedDivision_Test_Master.School_id AND " +
                                        " SchoolWise_Standard_Division_Master.academic_year_id = SchoolWise_StanderedDivision_Test_Master.Academic_Year_ID" +
                                    " WHERE     (SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (Standard_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (Division_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                        " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ")" +
                                        //" AND (Standard_Master.Is_PrePrimary='N')" +
                                //        " AND SchoolWise_Standard_Division_Master.Standard_Id " +
                                //"   NOT IN (SELECT Standard_Id " +
                                //" FROM         StandardsWithOnlyGradesSettings " +
                                //" WHERE     (School_Id = " + miSchoolId + ") " +
                                //    " AND (Academic_Year_Id = " + miAcademicYearId + ") AND (Is_Deleted = 'N')) " +
                                    " GROUP BY SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id" +
                                        " , Standard_Master.Standard_Name" +
                                        " , Division_Master.Division_Name" +
                                        " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name" +
                                        " , Standard_Master.Original_Standard_Id" +
                                        " , Division_Master.Original_Division_Id" +
                                        " , SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id" +
                                    " ORDER BY Standard_Master.Original_Standard_Id, Division_Master.Original_Division_Id";
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsGorTest(int aiTestId)
        {
            string sSelectStatement = "SELECT DISTINCT Standard_Master.Standard_Name" +
                        " , Standard_Master.Original_Standard_Id" +
                        " , SchoolWise_Standard_Division_Master.Standard_Id " +
                        " FROM SchoolWise_Standard_Division_Master INNER JOIN " +
                              " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND " +
                              " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN " +
                              " SchoolWise_StanderedDivision_Test_Master ON " +
                              " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = SchoolWise_StanderedDivision_Test_Master.Standerd_division_Id AND " +
                              " SchoolWise_Standard_Division_Master.School_Id = SchoolWise_StanderedDivision_Test_Master.School_id AND " +
                              " SchoolWise_Standard_Division_Master.academic_year_id = SchoolWise_StanderedDivision_Test_Master.Academic_Year_ID" +
                        " WHERE     (SchoolWise_Standard_Division_Master.Is_Deleted = 'N') " +
                                " AND (Standard_Master.Is_Deleted = 'N') AND " +
                                " (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ")" +
                                " AND SchoolWise_Standard_Division_Master.Standard_Id " +
                                "   NOT IN (SELECT Standard_Id " +
                                " FROM         StandardsWithOnlyGradesSettings " +
                                " WHERE     (School_Id = " + miSchoolId + ") " +
                                    " AND (Academic_Year_Id = " + miAcademicYearId + ") AND (Is_Deleted = 'N')) " +
                        " GROUP BY Standard_Master.Standard_Name" +
                                " , Standard_Master.Standard_Name" +
                                " , Standard_Master.Original_Standard_Id" +
                                " , SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id" +
                                " , SchoolWise_Standard_Division_Master.Standard_Id" +
                        " HAVING      (SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id =  " + aiTestId + ")" +
                        " ORDER BY Standard_Master.Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get configureds standered where atleast one test associated.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsGorTest()
        {
            string sSelectStatement = "SELECT DISTINCT Standard_Master.Standard_Name" +
                        " , Standard_Master.Original_Standard_Id" +
                        " , SchoolWise_Standard_Division_Master.Standard_Id " +
                        " FROM SchoolWise_Standard_Division_Master INNER JOIN " +
                              " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND " +
                              " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN " +
                              " SchoolWise_StanderedDivision_Test_Master ON " +
                              " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = SchoolWise_StanderedDivision_Test_Master.Standerd_division_Id AND " +
                              " SchoolWise_Standard_Division_Master.School_Id = SchoolWise_StanderedDivision_Test_Master.School_id AND " +
                              " SchoolWise_Standard_Division_Master.academic_year_id = SchoolWise_StanderedDivision_Test_Master.Academic_Year_ID" +
                        " WHERE     (SchoolWise_Standard_Division_Master.Is_Deleted = 'N') " +
                                " AND (Standard_Master.Is_Deleted = 'N') AND " +
                                " (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ")" +
								//" AND Standard_Master.Is_PrePrimary='N'" +
                                //" AND SchoolWise_Standard_Division_Master.Standard_Id "+
                                //"   NOT IN (SELECT Standard_Id " +
                                //" FROM         StandardsWithOnlyGradesSettings "+
                                //" WHERE     (School_Id = " + miSchoolId + ") "+
                                //    " AND (Academic_Year_Id = " + miAcademicYearId + ") AND (Is_Deleted = 'N')) " +
                        " GROUP BY Standard_Master.Standard_Name" +
                                " , Standard_Master.Standard_Name" +
                                " , Standard_Master.Original_Standard_Id" +
                                " , SchoolWise_StanderedDivision_Test_Master.SchoolWise_Test_Id" +
                                " , SchoolWise_Standard_Division_Master.Standard_Id" +
                        " ORDER BY Standard_Master.Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnnualResultStandardsDivisions()
        {
            string sSelectStatement = "SELECT DISTINCT "+
                                        " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id "+
                                        " , Standard_Master.Standard_Name "+
                                        " , Division_Master.Division_Name "+
                                        " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name AS StandardDivision "+
                                        " , Standard_Master.Original_Standard_Id "+
                                        " , Division_Master.Original_Division_Id "+
                                    " FROM         SchoolWise_Standard_Division_Master INNER JOIN "+
                                        " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND "+
                                        " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN "+
                                        " Division_Master ON SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id AND "+
                                        " SchoolWise_Standard_Division_Master.School_Id = Division_Master.School_Id AND "+
                                        " SchoolWise_Standard_Division_Master.academic_year_id = Division_Master.academic_year_id INNER JOIN "+
                                        " YearWise_Student_Details ON SchoolWise_Standard_Division_Master.Standard_Id = YearWise_Student_Details.Standard_Id AND "+
                                        " SchoolWise_Standard_Division_Master.Division_Id = YearWise_Student_Details.Division_id AND "+
                                        " SchoolWise_Standard_Division_Master.academic_year_id = YearWise_Student_Details.Academic_Year_ID AND "+
                                        " SchoolWise_Standard_Division_Master.School_Id = YearWise_Student_Details.School_Id INNER JOIN "+
                                        " SchoolWise_StudentResult ON YearWise_Student_Details.YearWise_Student_Id = SchoolWise_StudentResult.Student_Id "+
                                    " WHERE  (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                        " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ")" +                                        
                                        " AND (SchoolWise_Standard_Division_Master.Is_Deleted = 'N') "+
                                        " AND (Standard_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                        " AND (Division_Master.Is_Deleted = N'" + Constants.C_NO + "')" +
                                    " GROUP BY SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id  "+
                                        " , Standard_Master.Standard_Name "+
                                        " , Division_Master.Division_Name "+
                                        " , Standard_Master.Standard_Name + '-' + Division_Master.Division_Name"+
                                        " , Standard_Master.Original_Standard_Id "+
                                        " , Division_Master.Original_Division_Id "+
                                    " ORDER BY Standard_Master.Original_Standard_Id "+
                                        " , Division_Master.Original_Division_Id ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public void UpdateStandards(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public void UpdateStandardDivisions(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public void UpdateStandardDivisionsSubjects(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", miAcademicYearId, SqlDbType.Int);
                 oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateClassForBatch");
     
            }
        }
        public DataSet GetStdDivAssociation()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardDivisionAssociation");
            }
        }

        public List<StandardDivisionMaster> GetStandardDivisionList()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardDivisionList"))
                return SetStdDivDetails(oSqlDataReader);
            }
        }

        private List<StandardDivisionMaster> SetStdDivDetails(SqlDataReader oSqlDataReader)
        {
           List<StandardDivisionMaster> oStandardDivisionMasterlst=new List<StandardDivisionMaster>();
           while (oSqlDataReader.Read())
            {
                StandardDivisionMaster oStandardDivisionMaster =new StandardDivisionMaster
                {
                    StandardId = Convert.ToInt32(oSqlDataReader["StandardId"]),
                    DivisionId= Convert.ToInt32(oSqlDataReader["DivisionId"]),
                    StandardDivisionId= Convert.ToInt32(oSqlDataReader["StandardDivisionId"]),
                };
                oStandardDivisionMasterlst.Add(oStandardDivisionMaster);
            }
           return oStandardDivisionMasterlst;
        }
    }

}
