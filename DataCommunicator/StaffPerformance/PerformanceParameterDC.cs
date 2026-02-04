/*File Name - PerformanceParameterDC.cs
 * Created Date - 17 Sept 2013
 * Created By - Sachin
 * Description - This class is used to communicate with database for managing performance parameter details.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StaffPerformanceEntity;

namespace DataCommunicator
{
    public class PerformanceParameterDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public PerformanceParameterDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return all available parameters.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="aiPerformanceParameterId"></param>
        /// <returns></returns>
        public List<PerformanceParameter> GetAll(int aiYear, int aiSkillId,int iFormTypeId, int aiPerformanceParameterId )
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", aiSkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PerformanceParameterId", aiPerformanceParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AppraisalFormTypeId", iFormTypeId, SqlDbType.Int);

                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPerformanceParameters"))
                    return this.FillPerformanceParameters(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to save parametere details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
        public void Save(PerformanceParameter aoPerformanceParameter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aoPerformanceParameter.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", aoPerformanceParameter.SkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Title", aoPerformanceParameter.Title, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", aoPerformanceParameter.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AppraisalFormTypeId", aoPerformanceParameter.AppraisalFormTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PerformanceParameterId", aoPerformanceParameter.Id, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePerformanceParameter");
                
            }
        }

        /// <summary>
        /// This method is used to delete parameter details.
        /// </summary>
        /// <param name="aiPerformanceParameterId"></param>
        /// <param name="aiConfigId"></param>
        public void Delete(int aiPerformanceParameterId, int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PerformanceParameterId", aiPerformanceParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePerformanceParameter");
            }
        }

        /// <summary>
        /// This method is used to submit / un submit parameters of selected year and skills.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="abIsSubmit"></param>
        public void Submit(int aiYear, int aiSkillId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", aiSkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitPerformanceParameters");
            }
        } 

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill performance parameter entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<PerformanceParameter> FillPerformanceParameters(SqlDataReader aoSqlDataReader)
        {
            List<PerformanceParameter> lstPerformanceParameters = new List<PerformanceParameter>();
            while (aoSqlDataReader.Read())
            {
                lstPerformanceParameters.Add(new PerformanceParameter
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Title = Convert.ToString(aoSqlDataReader["Title"]),
                    FormType = Convert.ToString(aoSqlDataReader["FormType"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                  AppraisalFormTypeId =   Convert.ToInt32(aoSqlDataReader["AppraisalFormTypeId"]),
					SkillId = Convert.ToInt32(aoSqlDataReader["SkillId"])
                });
            }

            return lstPerformanceParameters;
        } 

        #endregion
    }
}
