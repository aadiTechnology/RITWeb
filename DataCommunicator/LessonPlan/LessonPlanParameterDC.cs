using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LessonPlanEntities;
using System.Data.SqlClient;
using System.Data;
using Utility;

namespace DataCommunicator.LessonPlan
{
   public class LessonPlanParameterDC
    {
        #region "Data Members"

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;
        #endregion "Data Members"

        #region "Constructors"

        public LessonPlanParameterDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion "Constructors"

        #region Method(s)
       /// <summary>
       /// This method is used to get lesson plan category
       /// </summary>
       /// <returns></returns>
       /// 
        public List<LessonPlanCategory> GetCategories()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLessonPlanCategory"))
                    return this.FillCategories(oSqlDataReader);
            }
        }
        public DataTable GetParentLessonPlan(int aiCategoryId)
        {
            string sSelectStatement = " SELECT " +
                                         " Id ," +
                                          " Title " +
                                      " FROM " +
                                         " LessonPlanParameters " +
                                      " WHERE " +
                                      "SchoolId=N'"+ miSchoolId +"' " +
                                        " AND AcademicYearId =N'" + miAcademicYearId + "' " +
                                        " and (ParentParameterId is null or ParentParameterId = 0)" +
                                        " and LessonPlanCategoryId=" +aiCategoryId+
                                         " AND IsDeleted=N'" + Constants.I_ZERO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        /// <summary>
        /// This method is used to fill Lesson plan details
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<LessonPlanCategory> FillCategories(SqlDataReader aoSqlDataReader)
        {
            List<LessonPlanCategory> lstFormTypeDetails = new List<LessonPlanCategory>();
            LessonPlanCategory oLessonPlanParameters = null;
            while (aoSqlDataReader.Read())
            {
                oLessonPlanParameters = new LessonPlanCategory
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"])
                };
                lstFormTypeDetails.Add(oLessonPlanParameters);
            }
            return lstFormTypeDetails;
        }
         /// <summary>
        /// This method is used to save parametere details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
        public void Save(LessonPlanParameters aoLessonPlanParameters)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Title", aoLessonPlanParameters.Title, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", aoLessonPlanParameters.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LessonPlanCategoryId", aoLessonPlanParameters.LessonPlanCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LessonPlanSectionId", aoLessonPlanParameters.LessonPlanSectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectCategoryId", aoLessonPlanParameters.SubjectCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoLessonPlanParameters.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentParameterId", aoLessonPlanParameters.ParentParameterId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLessonPlanParameter");
            }
        }

       /// <summary>
       /// This method is used to delete lesson plan parameter details
       /// </summary>
       /// <param name="aiLessonPlanParameterId"></param>
       /// <param name="aiConfigId"></param>
        public void Delete(int aiLessonPlanParameterId, int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiLessonPlanParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteLessonPlanParameter");
            }
        }


        /// <summary>
        /// This method is used to return all available parameters.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="aiPerformanceParameterId"></param>
        /// <returns></returns>
        public List<LessonPlanParameters> GetAll(int aiCategoryId, int aiSectionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LessonPlanCategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LessonPlanSectionId", aiSectionId, SqlDbType.Int);
               
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLessonPlanParameter"))
                    return this.FillLessonPlanParameters(oSqlDataReader);
            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="SubjectCategoryId"></param>
       /// <returns></returns>
        public List<LessonSubjectCategories> GetSubjectCategories()

        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSubjectCategory"))
                    return this.FillSubjectCategory(oSqlDataReader);
            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
       private List<LessonSubjectCategories> FillSubjectCategory(SqlDataReader aoSqlDataReader)
       {
           List<LessonSubjectCategories> lstSubjectCategory = new List<LessonSubjectCategories>();
           LessonSubjectCategories oLessonSubjectCategories = null;

           while (aoSqlDataReader.Read())
           {
               oLessonSubjectCategories = new LessonSubjectCategories
               {
                   Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                   Name = Convert.ToString(aoSqlDataReader["Name"])
               };
               lstSubjectCategory.Add(oLessonSubjectCategories);
           }
           return lstSubjectCategory;
       }



        /// <summary>
        /// This method is used to fill Lesson Plan parameter entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<LessonPlanParameters> FillLessonPlanParameters(SqlDataReader aoSqlDataReader)
        {
            List<LessonPlanParameters> lstPerformanceParameters = new List<LessonPlanParameters>();
            while (aoSqlDataReader.Read())
            {
                lstPerformanceParameters.Add(new LessonPlanParameters
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Title = Convert.ToString(aoSqlDataReader["Title"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    LessonPlanCategoryId = Convert.ToInt32(aoSqlDataReader["LessonPlanCategoryId"]),
                    SubjectCategoryId = Convert.ToInt32(aoSqlDataReader["SubjectCategoryId"]),
                    SubjectCategoryName = Convert.ToString(aoSqlDataReader["SubjectCategoryName"]),
                    ParentParameterId = Convert.ToInt32(aoSqlDataReader["ParentParameterId"]),
                    ParentParameter = Convert.ToString(aoSqlDataReader["ParentParameter"]),
                 });
            }

            return lstPerformanceParameters;
        }

        /// <summary>
        /// This method is used to submit / un submit parameters of selected lesson plan category.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="abIsSubmit"></param>
        public void Submit(int aiLessonPlanCategoryId,bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("LessonPlanCategoryId", aiLessonPlanCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitLessonPlanParameters");
            }
        }
        #endregion

    }
}
