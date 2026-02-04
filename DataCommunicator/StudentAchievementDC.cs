// Class Name       :- StudentAchievementDC
// Purpose          :- This class is used to manage student Achievement details.
// Date Of creation :- 17/11/2015
// Author Name      :-


using System;
using System.Collections.Generic;
using Utility;
using SchoolEntities;
using System.Data.SqlClient;
using System.Data;
namespace DataCommunicator
{
    public class StudentAchievementDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public StudentAchievementDC() { }
        public StudentAchievementDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get student All Achievement details.
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="miStudentId"></param>
        /// <returns></returns>

        public List<StudentAchievement> GetAll(int miStudentId,int aiNoteCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NoteCategoryId", aiNoteCategoryId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsAllAchievementDetails"))
                    return FillStudnetAchievement(oSqlDataReader);
            }
        }

        private List<StudentAchievement> FillStudnetAchievement(SqlDataReader aoSqlDataReader)
        {
            List<StudentAchievement> lstStudentAchievement = new List<StudentAchievement>();
            while (aoSqlDataReader.Read())
            {
                StudentAchievement oStudentAchievement = new StudentAchievement();
                oStudentAchievement.AchievementId = Convert.ToInt32(aoSqlDataReader["AchievementId"]);
                oStudentAchievement.StudentClass = Convert.ToString(aoSqlDataReader["ClassName"]);
                oStudentAchievement.AchievementDate = Convert.ToDateTime(aoSqlDataReader["AchievementDate"]);
                oStudentAchievement.Description = Convert.ToString(aoSqlDataReader["Description"]);
                if (aoSqlDataReader["Attachment"] != null)
                    oStudentAchievement.Attachment = Convert.ToString(aoSqlDataReader["Attachment"]);
                lstStudentAchievement.Add(oStudentAchievement);
            }
            return lstStudentAchievement;
        }

        /// <summary>
        /// This method is used to get Achievement Detail.
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="miStudentId"></param>
        ///  <param name="miAchievementId"></param>
        ///   <param name="miUpdatedById"></param>
        /// <returns></returns>

        public StudentAchievement Get(int aiAchievementId, int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentAchievement oStudentAchievement = new StudentAchievement();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AchievementId", aiAchievementId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentAchievementDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oStudentAchievement.AchievementId = Convert.ToInt32(oSqlDataReader["AchievementID"]);
                        oStudentAchievement.AchievementDate = Convert.ToDateTime(oSqlDataReader["AchievementDate"]);
                        oStudentAchievement.Description = Convert.ToString(oSqlDataReader["Description"]);
                        oStudentAchievement.Attachment = Convert.ToString(oSqlDataReader["Attachment"]);
                    }
                }
                return oStudentAchievement;
            }
        }

        /// <summary>
        /// This method is used to save Achievement details.
        /// </summary>
        /// <param name="aoStudentAchievement"></param>

        public void Save(StudentAchievement aoStudentAchievement)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AchievementId", aoStudentAchievement.AchievementId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aoStudentAchievement.StudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AchievementDate", aoStudentAchievement.AchievementDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Description", aoStudentAchievement.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Attachment", aoStudentAchievement.Attachment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("NoteCategory", aoStudentAchievement.NoteCategoryId, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentAchievementDetails");
            }
        }

        /// <summary>
        /// This method is used to delete Achievement details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="aiAchievementId"></param>
        /// <param name="miUpdatedById"></param>

        public void Delete(int aiAchievementId, int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentAchievement oStudentAchievement = new StudentAchievement();
                oSQLServerDbUtility.AddParameter("AchievementId", aiAchievementId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStudentAchievementDetails");
            }
        }

        /// <summary>
        /// This method is used to get name & Registration Number of a student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="miSchoolId"></param>

        public StudentAchievement GetStudentDetails(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentAchievement oStudentAchievement = new StudentAchievement();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentNameForAchievementControl"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oStudentAchievement.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                        oStudentAchievement.RegistrationNo = Convert.ToString(oSqlDataReader["RegistrationNo"]);
                    }
                }
                return oStudentAchievement;
            }
        }
        /// <summary>
        /// this method is used to save achievement deatils
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="asImageXml"></param>
        public void SaveAchievementDetails(string asXml, string asImageXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AchievementDetailsXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ImageXml", asImageXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveAchievementDetails");
            }
        }
        /// <summary>
        /// this method is used to get achievement details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<AchievementDetails> GetAchievementDetails(int aiSchoolId, out List<Images> alstImagePath, int aiAchievementId)
        {
            List<AchievementDetails> lstAchievementDetails = new List<AchievementDetails>();

            alstImagePath  = new List<Images>();
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AchievementId", aiAchievementId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAchievementDetails "))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            AchievementDetails oAchievementDetails = new AchievementDetails() 
                            {
                                Id = Convert.ToInt16(oSqlDataReader["Id"]),
                                AchievementTitle = oSqlDataReader["Title"].ToString(),
                                Description = oSqlDataReader["Description"].ToString(),
                                IsSelected = Convert.ToBoolean(oSqlDataReader["IsSelected"]),
                                PhotoCount = Convert.ToInt16(oSqlDataReader["PhotoCount"]),
                            };

                            lstAchievementDetails.Add(oAchievementDetails);
                        }
                            
                        if (oSqlDataReader.NextResult())
                            {
                                while (oSqlDataReader.Read())
                                {
                                    Images oImages = new Images
                                    {
                                        achievementId = Convert.ToInt32(oSqlDataReader["AchievementId"]),
                                        ImagePath = oSqlDataReader["ImagePath"].ToString(),
                                        Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                        FieldIndex = Convert.ToInt32(oSqlDataReader["FieldIndex"])
                                    };

                                    alstImagePath.Add(oImages);
                             };
                        }
                    }
                }
            }

          return lstAchievementDetails;
        }

        /// <summary>
        /// this method is used to delete achievement deatils.
        /// </summary>
        /// <param name="aiAchievementId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteAchievementDetails(int aiAchievementId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AchievementId", aiAchievementId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteAchievementDetails");
            }

        }

        /// <summary>
        /// This method is used to return note categories.
        /// </summary>
        /// <returns></returns>
        public DataTable GetNoteCategories()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetNoteCategories]");
            }
        }

        #endregion
    }
}
