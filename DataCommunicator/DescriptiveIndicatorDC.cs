using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class DescriptiveIndicatorDC
    {
        #region Data Member(s)

        private int miSchoolId, miAcademicYearId, miUpdatedById;
        public int miStudentCount; 

        #endregion

        #region Constructor(s)
        
        public DescriptiveIndicatorDC()
        {
        }

        public DescriptiveIndicatorDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)



        public DataTable GetAllGradeDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetMarkGradeDetails");
                   
            }
        }
        /// <summary>
        /// This method is used to return descriptive indicator marks and remarks.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public DescriptiveIndicator GetAll(int aiYearwiseStudentId, int aiSkillId, int aiTermId)
        {
            DescriptiveIndicator oDescriptiveIndicator = new DescriptiveIndicator();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentSkillId", aiSkillId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDescriptiveIndicatorMarks"))
                {
                    oSqlDataReader.Read();
                    oDescriptiveIndicator.StudentDetails =
                        new Student
                        {
                            RollNo = oSqlDataReader["Roll_No"].ToInt(),
                            Name = oSqlDataReader["StudentName"].ToString(),
                            ClassName = oSqlDataReader["ClassName"].ToString(),
                            Gender = Convert.ToChar(oSqlDataReader["Gender"]),
                            IsPublished = oSqlDataReader["IsPublished"].ToBool()
                        };

                    oSqlDataReader.NextResult();
                    oDescriptiveIndicator.DescriptiveSkills = GetAllDescriptiveSkills(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    oDescriptiveIndicator.DescriptiveParameters = GetAllDescriptiveParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    oDescriptiveIndicator.StudentwiseDescriptiveObservations = GetAllDescriptiveObservations(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    oDescriptiveIndicator.StudentwiseDescriptiveMarks = GetAllDescriptiveMarks(oSqlDataReader);
                }
            }
            return oDescriptiveIndicator;
        }

        /// <summary>
        /// This method is used to get standardwise Student details for Descriptive indecators..
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<StudentDetailsForDescriptiveIndicators> GetAllStudentDetails(int aiSchoolId, int aiAcademicYearId, int aiStandardDivId, int aiTermId, string asSortExpression, string asSortDirection, int aiStartRowIndex, int aiEndIndex)
        {
            List<StudentDetailsForDescriptiveIndicators> lstStudentDetails = new List<StudentDetailsForDescriptiveIndicators>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpr", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentForDescriptiveIndicators"))
                {
                    lstStudentDetails = FillStudentDetails(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                            miStudentCount = Convert.ToInt32(oSqlDataReader["Count"]);
                    }
                }
            }
            return lstStudentDetails;
        }

        /// <summary>
        /// This method is used to publish descriptive indicators.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="aiPublish"></param>
        public void PublishDescriptiveIndecators(int aiYearwiseStudentId, int aiTermId, int aiPublish, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdandardDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublished", aiPublish, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);                

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_PublishStudentwiseDescriptiveIndecators");
            }
        }

        /// <summary>
        /// This method is used to check publish status.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="aiPublishStatus"></param>
        /// <param name="aiPublished"></param>
        public void CheckPublishStatus(int aiStandardDivId, int aiTermId, out int aiPublishStatus, out int aiPublished)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("PublishStatus", 0, SqlDbType.Int, ParameterDirection.Output);
                SqlParameter aoSqlParameter = oSQLServerDbUtility.AddParameter("Published", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_CheckStandardwisePublishStatus");
                aiPublishStatus = Convert.ToInt32(oSqlParameter.Value);
                aiPublished = Convert.ToInt32(aoSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to return all parent skills.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<DescriptiveSkill> GetAllSections(int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllDescriptiveSkills"))
                {
                    List<DescriptiveSkill> lstSkills = new List<DescriptiveSkill>();
                    while (oSqlDataReader.Read())
                    {
                        lstSkills.Add
                            (
                                new DescriptiveSkill
                                {
                                    Id = oSqlDataReader["Id"].ToInt(),
                                    OutOfMark = oSqlDataReader["OutOfMark"].ToInt(),
                                    ParentSkillId = oSqlDataReader["ParentSkillId"].ToInt(),
                                    Skill = oSqlDataReader["Skill"].ToString(),
                                    SortOrder = oSqlDataReader["SortOrder"].ToInt()
                                }
                            );
                    }
                    return lstSkills;
                }
            }
        }

        /// <summary>
        /// This method is used to save marks and remarks.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="asObservationXml"></param>
        /// <param name="asMarks"></param>
        public void Save(int aiYearwiseStudentId, int aiTermId, string asObservationXml, string asMarks)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ObservationXml", asObservationXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("MarkXml", asMarks, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveDescriptiveIndMarks");
            }
        } 

        #endregion

        #region Private method(s)

        /// <summary>
        /// This method is used to Add results to fill Student Details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private List<StudentDetailsForDescriptiveIndicators> FillStudentDetails(SqlDataReader oSqlDataReader)
        {
            List<StudentDetailsForDescriptiveIndicators> olstStudentDatails = new List<StudentDetailsForDescriptiveIndicators>();
            while (oSqlDataReader.Read())
            {
                StudentDetailsForDescriptiveIndicators oStudentDatailsForDI = new StudentDetailsForDescriptiveIndicators();
                oStudentDatailsForDI.RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]);
                oStudentDatailsForDI.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                oStudentDatailsForDI.YearwiseStudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]);
                oStudentDatailsForDI.StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]);
                oStudentDatailsForDI.EditStatus = Convert.ToInt32(oSqlDataReader["EditStatus"]);
                oStudentDatailsForDI.IsPublished = Convert.ToInt32(oSqlDataReader["IsPublished"]);

                olstStudentDatails.Add(oStudentDatailsForDI);
            }
            return olstStudentDatails;
        }

        /// <summary>
        /// This method is used to return marks.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentwiseDescriptiveMark> GetAllDescriptiveMarks(SqlDataReader aoSqlDataReader)
        {
            List<StudentwiseDescriptiveMark> lstStudentwiseDescriptiveMarks = new List<StudentwiseDescriptiveMark>();
            while (aoSqlDataReader.Read())
            {
                lstStudentwiseDescriptiveMarks.Add
                    (
                        new StudentwiseDescriptiveMark
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            ObservationId = aoSqlDataReader["ObservationId"].ToInt(),
                            ParameterId = aoSqlDataReader["ParameterId"].ToInt(),
                            Mark = aoSqlDataReader["Mark"].ToDecimal(),
                            AssignedGradeId = aoSqlDataReader["AssignedGradeId"].ToInt()
                        }
                    );
            }
            return lstStudentwiseDescriptiveMarks;
        }

        /// <summary>
        /// This method is used to return observations.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentwiseDescriptiveObservation> GetAllDescriptiveObservations(SqlDataReader aoSqlDataReader)
        {
            List<StudentwiseDescriptiveObservation> lstStudentwiseDescriptiveObservations = new List<StudentwiseDescriptiveObservation>();
            while (aoSqlDataReader.Read())
            {
                lstStudentwiseDescriptiveObservations.Add
                    (
                        new StudentwiseDescriptiveObservation
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            SkillId = aoSqlDataReader["SkillId"].ToInt(),
                            Observation = aoSqlDataReader["Observation"].ToString(),
                            YearwiseStudentId = aoSqlDataReader["YearwiseStudentId"].ToInt()
                        }
                    );
            }
            return lstStudentwiseDescriptiveObservations;
        }

        /// <summary>
        /// This method is used to return parameters.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<DescriptiveParameter> GetAllDescriptiveParameters(SqlDataReader aoSqlDataReader)
        {
            List<DescriptiveParameter> lstDescriptiveParameters = new List<DescriptiveParameter>();
            while (aoSqlDataReader.Read())
            {
                lstDescriptiveParameters.Add
                    (
                        new DescriptiveParameter
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            SkillId = aoSqlDataReader["SkillId"].ToInt(),
                            Parameter = aoSqlDataReader["Parameter"].ToString(),
                            SortOrder = aoSqlDataReader["SortOrder"].ToInt()
                        }
                    );
            }
            return lstDescriptiveParameters;
        }

        /// <summary>
        /// This method is used to return skills.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<DescriptiveSkill> GetAllDescriptiveSkills(SqlDataReader aoSqlDataReader)
        {
            List<DescriptiveSkill> lstDescriptiveSkill = new List<DescriptiveSkill>();
            while (aoSqlDataReader.Read())
            {
                lstDescriptiveSkill.Add
                    (
                        new DescriptiveSkill
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            OutOfMark = aoSqlDataReader["OutOfMark"].ToInt(),
                            ParentSkillId = aoSqlDataReader["ParentSkillId"].ToInt(),
                            Skill = aoSqlDataReader["Skill"].ToString(),
                            SortOrder = aoSqlDataReader["SortOrder"].ToInt()
                        }
                    );
            }
            return lstDescriptiveSkill;
        } 

        #endregion
    }
}
