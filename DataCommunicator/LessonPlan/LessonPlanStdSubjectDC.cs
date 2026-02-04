using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LessonPlanEntities;

namespace DataCommunicator
{
    public class LessonPlanStdSubjectDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public LessonPlanStdSubjectDC()
        {
        }

        public LessonPlanStdSubjectDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return all standard wise subjects.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<LessonPlanStdSubject> GetAllSubjects(int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLessonPlanStdSubjects"))
                {
                    List<LessonPlanStdSubject> lstSubjects = new List<LessonPlanStdSubject>();
                    while (oSqlDataReader.Read())
                    {
                        lstSubjects.Add(
                                new LessonPlanStdSubject
                                {
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    SubjectId = Convert.ToInt32(oSqlDataReader["Subject_Id"]),
                                    SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"])
                                }

                            );
                    }

                    return lstSubjects;
                }
            }
        }

        /// <summary>
        ///  This method is used to save subject details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="asSubjectIds"></param>
        public void Save(int aiStandardId, string asSubjectIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectIds", asSubjectIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLessonPlanStdSubjects");
            }
        } 

        #endregion
    }
}
