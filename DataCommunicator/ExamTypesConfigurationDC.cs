// File Name    : ExamTypesConfigurationDC.cs
// Created By   : Yogesh
// Crested Date : 23-May-2015 

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using SchoolEntities.Admin;
    using Utility;

    public class ExamTypesConfigurationDC
    {
        #region Data Member(s)
        private int iSchoolId;
        private int iAcademicYearId;
        private int iUserId;
        #endregion

        #region Constructor(s)
        public ExamTypesConfigurationDC()
        {
        }
        public ExamTypesConfigurationDC(int miUserId)
        {
            this.iUserId = miUserId;
        }

        public ExamTypesConfigurationDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.iSchoolId = aiSchoolId;
            this.iAcademicYearId = aiAcademicYearId;
        }
        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to Get all SubjectwiseExamTypes.
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<SubjectwiseExamTypeDetails> GetAll(int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSubjectiwiseExamTypeConfiguration"))
                    return this.ReadAllExamTypes(oSqlDataReader);
            }
        }


        /// <summary>
        /// This method is used to get all test type from table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public List<SubjectwiseExamTypeDetails> GetAllTestType(int aiTestTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestTypeId", aiTestTypeId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTestType"))
                    return this.ReadAllTestTypes(oSqlDataReader);
            }
        }
        /// <summary>
        /// This method is used to assign value to the property that are retrieve from database
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<SubjectwiseExamTypeDetails> ReadAllTestTypes(SqlDataReader aoSqlDataReader)
        {
            List<SubjectwiseExamTypeDetails> lstSubjectwiseExamTypeDetails = new List<SubjectwiseExamTypeDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    SubjectwiseExamTypeDetails oSubjectwiseExamTypeDetails = new SubjectwiseExamTypeDetails();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.TestTypeId = Convert.ToInt32(aoSqlDataReader["TestType_Id"]);
                    if (aoSqlDataReader["TestType_Name"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.TestTypeName = aoSqlDataReader["TestType_Name"].ToString();
                    if (aoSqlDataReader["Sort_Order"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.SortOrder = aoSqlDataReader["Sort_Order"].ToInt();
                    lstSubjectwiseExamTypeDetails.Add(oSubjectwiseExamTypeDetails);
                    if (aoSqlDataReader["ConsiderExamStatus"].Equals(1))
                        oSubjectwiseExamTypeDetails.ConsiderExamStatus = true;
                    else
                        oSubjectwiseExamTypeDetails.ConsiderExamStatus = false;
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectwiseExamTypeDetails;
        }
        /// <summary>
        /// This method is used to select single test type
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public SubjectwiseExamTypeDetails GetTestType(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestTypeId", aiId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTestType"))
                    return this.ReadTestTypes(oSqlDataReader);
            }
        }
        /// <summary>
        /// This method is used to assign values to the property that are retrive from database
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public SubjectwiseExamTypeDetails ReadTestTypes(SqlDataReader aoSqlDataReader)
        {
            SubjectwiseExamTypeDetails lstSubjectwiseExamTypeDetails = new SubjectwiseExamTypeDetails();
            if (aoSqlDataReader != null)
            {
                if (aoSqlDataReader.Read())
                {
                    //SubjectwiseExamTypeDetails oSubjectwiseExamTypeDetails = new SubjectwiseExamTypeDetails();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        lstSubjectwiseExamTypeDetails.TestTypeId = Convert.ToInt32(aoSqlDataReader["TestType_Id"]);
                    if (aoSqlDataReader["TestType_Name"] != DBNull.Value)
                        lstSubjectwiseExamTypeDetails.TestTypeName = aoSqlDataReader["TestType_Name"].ToString();
                    if (aoSqlDataReader["Sort_Order"] != DBNull.Value)
                        lstSubjectwiseExamTypeDetails.SortOrder = aoSqlDataReader["Sort_Order"].ToInt();
                    if (aoSqlDataReader["ConsiderExamStatus"].Equals(1))
                        lstSubjectwiseExamTypeDetails.ConsiderExamStatus = true;
                    else
                        lstSubjectwiseExamTypeDetails.ConsiderExamStatus = false;
                    //lstSubjectwiseExamTypeDetails.TestTypeId = oSubjectwiseExamTypeDetails.TestTypeId;
                    //lstSubjectwiseExamTypeDetails.TestTypeName = oSubjectwiseExamTypeDetails.TestTypeName;
                    //lstSubjectwiseExamTypeDetails.ConsiderExamStatus = oSubjectwiseExamTypeDetails.ConsiderExamStatus;
                    //lstSubjectwiseExamTypeDetails.SortOrder = oSubjectwiseExamTypeDetails.SortOrder;
                }
                aoSqlDataReader.Close();
            }

            return lstSubjectwiseExamTypeDetails;
        }
        /// <summary>
        /// This method is used to delete test type
        /// </summary>
        /// <param name="aiTestTypeId"></param>
        public void Delete(int aiTestTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestTypeId", aiTestTypeId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UpdatedById", iUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteTestType");
            }
        }

        /// <summary>
        /// This method is used to save test type in table
        /// </summary>
        /// <param name="aiTestTypeid"></param>
        /// <param name="asTestTypeName"></param>
        /// <param name="aiExamTypeStatus"></param>
        /// <param name="aisSortOrder"></param>
        public void SaveTestType(int aiTestTypeid, string asTestTypeName, bool abExamTypeStatus, int aisSortOrder)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string ExamStatus;
                oSQLServerDbUtility.AddParameter("TestTypeId", aiTestTypeid, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TestTypeName", asTestTypeName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", aisSortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", iUserId, SqlDbType.Int);
                if (abExamTypeStatus == true)
                    ExamStatus = "Y";
                else
                    ExamStatus = "N";
                oSQLServerDbUtility.AddParameter("ConsiderExamStatus", ExamStatus, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTestType");
            }

        }
        /// <summary>
        /// This method is used to save Exam Type Configuration
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <param name="asTestTypeIdsForInsert"></param>
        /// <param name="asTestTypeIdsForDelete"></param>


        public void Save(int aiSubjectId, string asTestTypeIdsForInsert, string asTestTypeIdsForDelete)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestTypeIdsForInsert", asTestTypeIdsForInsert, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TestTypeIdsForDelete", asTestTypeIdsForDelete, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OriginalConfigId", Constants.SchoolConfigurations.ExamTypes, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_SaveExamTypesConfiguration");
            }
        }

        /// <summary>
        /// This method is used to Read all Exam type from table.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<SubjectwiseExamTypeDetails> ReadAllExamTypes(SqlDataReader aoSqlDataReader)
        {
            List<SubjectwiseExamTypeDetails> lstSubjectwiseExamTypeDetails = new List<SubjectwiseExamTypeDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    SubjectwiseExamTypeDetails oSubjectwiseExamTypeDetails = new SubjectwiseExamTypeDetails();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.TestTypeId = Convert.ToInt32(aoSqlDataReader["TestType_Id"]);
                    if (aoSqlDataReader["TestType_Name"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.TestTypeName = aoSqlDataReader["TestType_Name"].ToString();
                    if (aoSqlDataReader["Flag"] != DBNull.Value)
                        oSubjectwiseExamTypeDetails.Flag = Convert.ToInt32(aoSqlDataReader["Flag"]);
                    lstSubjectwiseExamTypeDetails.Add(oSubjectwiseExamTypeDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectwiseExamTypeDetails;
        }

        /// <summary>
        /// This method is used to get all yearwise subject types.
        /// </summary>
        /// <returns></returns>
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetYearwiseSubjectList"))
                    return this.ReadAllSubjects(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get all yearwise subject types.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetYearwiseSubjectList"))
                    return this.ReadAllSubjects(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to Read all Subject to fill dataset.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<YearWiseSubjectsDetails> ReadAllSubjects(SqlDataReader aoSqlDataReader)
        {
            List<YearWiseSubjectsDetails> lstSubjectsDetails = new List<YearWiseSubjectsDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    YearWiseSubjectsDetails oYearWiseSubjectsDetails = new YearWiseSubjectsDetails();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]);
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectName = aoSqlDataReader["Subject_Name"].ToString();

                    lstSubjectsDetails.Add(oYearWiseSubjectsDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectsDetails;
        }
        #endregion
    }
}