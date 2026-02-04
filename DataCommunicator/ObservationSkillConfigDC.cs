using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Teacher;

namespace DataCommunicator
{
    public class ObservationSkillConfigDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public ObservationSkillConfigDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }
        #endregion Constructor(s)

        #region Public Method(s)

        /// <summary>
        /// this method is used  fill subject dropdown
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>         
        public List<ObservationSkillConfig> GetAllSubjects(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSubject"))
                    return this.ReadAllSubjects(oSqlDataReader);
            }
        }
        /// <summary>
        /// This method is used to Read all Subject to fill dataset.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<ObservationSkillConfig> ReadAllSubjects(SqlDataReader aoSqlDataReader)
        {
            List<ObservationSkillConfig> lstSubjectsDetails = new List<ObservationSkillConfig>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    ObservationSkillConfig oObservationSkillConfig = new ObservationSkillConfig();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oObservationSkillConfig.SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]);
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oObservationSkillConfig.SubjectName = aoSqlDataReader["Subject_Name"].ToString();

                    lstSubjectsDetails.Add(oObservationSkillConfig);
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectsDetails;
        }
        /// <summary>
        /// This method is used to return all available Skill Details.
        /// </summary>
        /// <param name="aiStandardid"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>

        public List<ObservationSkillConfig> GetAll(int aiStandardid, int aiSubjectId, string asFilter)
        {
            List<ObservationSkillConfig> lstObservationSkillConfig = new List<ObservationSkillConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllObservationConfigSkill"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstObservationSkillConfig.Add(new ObservationSkillConfig
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            Name = oSqlDataReader["Name"].ToString(),
                            SubjectName = oSqlDataReader["Subject_Name"].ToString(),
                            StandardName = oSqlDataReader["Standard_Name"].ToString(),
                            SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]),

                        });
                    }
                }
            }
            return lstObservationSkillConfig;
        }

        /// <summary>
        /// This method is used to Save Skill Details.
        /// </summary>
        /// <param name="oObservationSkillConfig"></param>
        public void Save(ObservationSkillConfig oObservationSkillConfig)
        {
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StandardId", oObservationSkillConfig.StandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SubjectId", oObservationSkillConfig.SubjectId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Skill", oObservationSkillConfig.Skill, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("SortOrder", oObservationSkillConfig.SortOrder, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("ObservationSkillConfigId", oObservationSkillConfig.Id, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveObservationConfigSkill");
                }
            }
        }
        /// <summary>
        /// This method is used to Delete Skill Details.
        /// </summary>
        /// <param name="aiObservationSkillConfig"></param>
        /// <param name="aiConfigId"></param>
        public void Delete(int aiObservationSkillConfig, int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ObservationSkillConfig", aiObservationSkillConfig, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteObservationSkillConfig");
            }
        } 

        #endregion
    }
}