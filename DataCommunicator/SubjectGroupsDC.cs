using System;
using System.Data;
using System.Collections;
using Utility;
using System.Data.SqlClient;
namespace DataCommunicator
{
    /// <summary>
    /// This class performs database related tasks for subject group.
    /// </summary>
    public class SubjectGroupsDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct SubjectGroupsStruct
        {
            public int miGroupId;
            public int miSchoolId;
            public int miParentSubjectId;
            public int miParentGroupId;
            public int miChangedParentSubjectId;
            public int miSubjectId;
            public int miacademicyearId;
        }


        #endregion

        #endregion

        #region DataMembers and properties

        #region Data members

        private SubjectGroupsStruct moSubjectGroupsStruct;

        #endregion

        #region Properties

        public SubjectGroupsStruct SubjectGroupsStructDetails
        {

            get { return moSubjectGroupsStruct; }
            set { moSubjectGroupsStruct = value; }
        }

        #endregion

        #endregion

        #region Constructors

        public SubjectGroupsDC()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to check whether group of this subject is available or not.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSubjectIds"></param>
        /// <returns></returns>
        public static bool IsSubjectGroupAvailable(int aiSchoolId, int aiAcademicYearId, string asSubjectIds, int aiStandardDivisionId)
        {
            string sSelectStatement = "SELECT [dbo].[Udf_IsSubjectGroupAvailable](" + aiSchoolId + "," + aiAcademicYearId + ",N'" + asSubjectIds + "'," + aiStandardDivisionId + ")";
            string sResult;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                sResult = oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
                if (sResult == "Y")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// This method executes transaction to store subject group configuration in database
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>

        public void UpdateSubjectGroups(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method retrives subject names in a group
        /// </summary>
        /// <returns></returns>

        public string RetriveSubjectsForGroup()
        {
            // This method returns string populated with master SubjectGroupss from databse.
            string sReturn = "";
            string sSelectStatement = " SELECT  " +
                "  sg.Subject_Id" +
                " ,  sm.Subject_name" +
                " , sg.group_id" +

            " FROM  " +
                "Subject_master sm , Subject_Groups sg " +
            " WHERE  " +
                 " sm.School_id = N'" + moSubjectGroupsStruct.miSchoolId + "'" +
                " AND sm.is_deleted = N'" + Constants.C_NO + "'" +
                " AND sg.academic_Year_Id = N'" + moSubjectGroupsStruct.miacademicyearId + "'" +
                " AND sm.Subject_Id = sg.Subject_Id " +
                " AND sg.parent_group_id = N'" + moSubjectGroupsStruct.miParentGroupId + "'" +
                " AND sm.School_id = sg.School_id " +
                " ORDER BY sm.Subject_Name";

            DataTable oDTSubjects;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oDTSubjects = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
                if (oDTSubjects.Rows.Count > 0)
                {
                    for (int i = 0; i < oDTSubjects.Rows.Count; i++)
                    {
                        if (sReturn != "")
                        {
                            sReturn = sReturn + ", " + oDTSubjects.Rows[i]["Subject_name"].ToString();
                        }
                        else
                        {
                            sReturn = oDTSubjects.Rows[i]["Subject_name"].ToString();
                        }
                    }
                }
                return sReturn;
            }

        }

        /// <summary>
        /// This method retrives subject ids of subjects in a group
        /// </summary>
        /// <returns></returns>

        public DataTable RetriveSubjectIdsForGroup()
        {
            // This method returns datatable populated with master SubjectGroupss from databse.

            string sSelectStatement;

            sSelectStatement = " SELECT  " +
                                    "  sm.Subject_Id, sm.Subject_Name" +
                              " FROM  " +
                                  "Subject_master sm , Subject_Groups sg " +
                              " WHERE  " +
                                   " sm.School_id = N'" + moSubjectGroupsStruct.miSchoolId + "'" +
                                  " AND sm.is_deleted = N'" + Constants.C_NO + "'" +
                                  " AND sg.academic_Year_Id = N'" + moSubjectGroupsStruct.miacademicyearId + "'" +
                                  " AND sm.Subject_Id = sg.Subject_Id " +
                                  " AND sg.parent_group_id = N'" + moSubjectGroupsStruct.miParentGroupId + "'" +
                                  " AND sm.School_id = sg.School_id " +
                                  " ORDER BY sm.Subject_Name";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        /// <summary>
        /// This method retrives the parent id for the group to be inserted next
        /// </summary>
        /// <returns></returns>

        public static int GetNextParentGroupId()
        {
            string sSelectStatement = " SELECT  " +
                           " CASE WHEN MAX(sg.parent_group_id) IS NULL THEN 1 " +
                           "ELSE max(sg.parent_group_id) + 1 END  as parentGroupId" +
                           " FROM  " +
                               " Subject_Groups sg " +
                           " WHERE  " +
                               "  sg.is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method deletes the records for given subject group
        /// </summary>
        /// <param name="aoArrSubjectGroups">list of groups (ids) to be deleted</param>

        public void DeleteSubjectGroup(ArrayList aoArrSubjectGroups)
        {
            string sIds = "";
            for (int i = 0; i < aoArrSubjectGroups.Count; i++)
            {
                if (sIds != "")
                {
                    sIds = sIds + "," + aoArrSubjectGroups[i].ToString();
                }
                else
                {
                    sIds = aoArrSubjectGroups[i].ToString();
                }
            }
            string sDeleteStament = "DELETE FROM Subject_Groups" +
                                    " WHERE " +
                                    " Parent_Group_Id IN (" + sIds + ")" +
                                    " AND school_id = " + moSubjectGroupsStruct.miSchoolId +
                                    " AND academic_year_id = " + moSubjectGroupsStruct.miacademicyearId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStament);
        }

        public bool IsSubjectGroupPresent()
        {
            bool bIsSubjectGroupPresent = false;
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", moSubjectGroupsStruct.miSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", moSubjectGroupsStruct.miacademicyearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSubjectGroupCount"))
                {
                    while (oSqlDataReader.Read())
                    {
                        bIsSubjectGroupPresent = oSqlDataReader["Count"].ToInt() > 0;
                    }
                }
                return bIsSubjectGroupPresent;
            }
        }
        #endregion
    }

    /// <summary>
    /// This class performs database related tasks for collection of subject-groups
    /// </summary>

    public class SubjectGroupsCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public SubjectGroupsCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        /// <summary>
        /// This method returns datatable populated with master SubjectGroups from databse.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllSubjectGroups(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            // This method returns datatable populated with master SubjectGroupss from databse.
            string sSelectStatement = " SELECT DISTINCT " +
                    "  parent_Subject_id " +
                    " , parent_Group_id " +
                    " , Parent_Subject_Name " +
                    " , academic_year_id" +
                " FROM  " +
                    "vw_SubjectGroup " +
                " WHERE  " +
                    "School_id = " + aiSchoolId +
                    " AND is_deleted = N'" + Constants.C_NO + "'" +
                    " AND academic_Year_Id = N'" + aiAcademicYearId + "'" +
                    " AND Standard_Division_Id = " + aiStandardDivisionId +
                    " ORDER BY Parent_Subject_Name";


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
    }
}
