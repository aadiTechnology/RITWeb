using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Teacher;
namespace DataCommunicator
{
    public class ObservationParametersDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public ObservationParametersDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Public Method(s)

        public List<ObservationParameters> GetAll(int aiSkillId, int aiObservationParameterId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", aiSkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ObservationParameterId", aiObservationParameterId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllObservationParameters"))
                    return this.FillObServationParameters(oSqlDataReader);
            }
        }

        public List<ObservationParameters> GetSkills(int aiSchoolId, int aiStandardid, int aiAcademicYearId)
        {
            List<ObservationParameters> lstObservationParameters = new List<ObservationParameters>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetFillSkills]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ObservationParameters oObservationParameters = new ObservationParameters
                        {
                            SkillId = Convert.ToInt32(oSqlDataReader["Id"]),
                            SkillName = oSqlDataReader["Name"].ToString()
                        };
                        lstObservationParameters.Add(oObservationParameters);
                    }
                }
            }
            return lstObservationParameters;
        }

        public void Save(ObservationParameters oObservationParameters)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                // oSQLServerDbUtility.AddParameter("StandardId", oObservationParameters.StandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", oObservationParameters.SkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Parameter", oObservationParameters.Parameter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", oObservationParameters.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ObservationParameterId", oObservationParameters.Id, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveObservationParameters");

            }
        }

        public void Delete(int aiParamterId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParameterId", aiParamterId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteObservationParameters");

            }
        }

        public void Submit(int aiStandardId, int aiSkillId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SkillId", aiSkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitObservationParameters");
            }
        } 

        #endregion

        #region Private Method(s)
        
        private List<ObservationParameters> FillObServationParameters(SqlDataReader aoSqlDataReader)
        {
            List<ObservationParameters> lstObservationParameters = new List<ObservationParameters>();
            while (aoSqlDataReader.Read())
            {
                lstObservationParameters.Add(new ObservationParameters
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Parameter = Convert.ToString(aoSqlDataReader["Parameter"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    SkillId = Convert.ToInt32(aoSqlDataReader["SkillId"])
                });
            }

            return lstObservationParameters;
        } 

        #endregion
    }
}

