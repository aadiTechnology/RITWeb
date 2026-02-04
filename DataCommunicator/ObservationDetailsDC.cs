using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    public class ObservationDetailsDC
    {
        #region Data MEmber(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        private List<ObservationSkill> mlstSkills;
        private List<ObservationGrade> mlstGrades;
        private List<ObservationParameter> mlstParameters;
         private List<ObservationDetails> mlstObservations;
         private List<ObservationRemarks> mlstRemarks;

        private string msClassName;
        private string msTestName;
        private string msSubjectName;
        private bool mbIsSubmitted;
        private bool mbIsPublished;

        #endregion

        #region Constructor(s)

        public ObservationDetailsDC()
        {
        }

        public ObservationDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)

        public List<ObservationSkill> Skills
        {
            get { return this.mlstSkills; }
        }

        public List<ObservationGrade> Grades
        {
            get { return this.mlstGrades; }
        }

       
        public List<ObservationParameter> Parameters
        {
            get { return this.mlstParameters; }
        }

        public List<ObservationDetails> Observations
        {
            get { return this.mlstObservations; }
        }

        public List<ObservationRemarks> Remarks
        {
            get { return this.mlstRemarks; }
        }


        public string ClassName
        {
            get { return msClassName; }
        }

        public string TestName
        {
            get { return msTestName; }
        }

        public string SubjectName
        {
            get { return msSubjectName; }
        }

        public bool IsSubmitted
        {
            get { return mbIsSubmitted; }
        }

        public bool IsPublished
        {
            get { return mbIsPublished; }
        }
                
        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<StudentBasicDetails> GetObservationDetails(int aiTestId, int aiStdDivId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetObservationDetails"))
                {

                    oSqlDataReader.Read();
                    msClassName = Convert.ToString(oSqlDataReader["ClassName"]);
                    msSubjectName = Convert.ToString(oSqlDataReader["SubjectName"]);
                    msTestName = Convert.ToString(oSqlDataReader["TestName"]);
                    mbIsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]);
                    mbIsPublished = Convert.ToBoolean(oSqlDataReader["IsPublished"]);

                    oSqlDataReader.NextResult();
                    FillSkills(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillGrades(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillObservations(oSqlDataReader);
                    
                    oSqlDataReader.NextResult();
                    FillRemarks(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    return  FillStudentDetails(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to save observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asObservationXml"></param>
        public void Save(int aiTestId, int aiSubjectId, int aiStdDivId, string asObservationXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ObservationXml", asObservationXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveObservationDetails");
            }
        }

        /// <summary>
        /// This method is used to submit observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiStdDivId"></param>
        public void Submit(int aiTestId, int aiSubjectId, int aiStdDivId, int aiIsSubmitted)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", aiIsSubmitted, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitObservationDetails");
            }
        } 

        #endregion

        #region Private Method(s)
        /// <summary>
        /// This method is sued to load student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentBasicDetails> FillStudentDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentBasicDetails> lstStudents = new List<StudentBasicDetails>();
            while (aoSqlDataReader.Read())
            {
                lstStudents.Add
                    (
                        new StudentBasicDetails
                        {
                            EnrolmentNumber = Convert.ToString(aoSqlDataReader["Enrolment_Number"]),
                            RollNo = Convert.ToInt32(aoSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(aoSqlDataReader["StudentName"]),
                            YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["Yearwise_Student_Id"])
                        }
                    );
            }
            return lstStudents;
        }

        /// <summary>
        /// This method is sued to load observation details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillObservations(SqlDataReader aoSqlDataReader)
        {
            this.mlstObservations = new List<ObservationDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstObservations.Add
                    (
                        new ObservationDetails
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            GradeId = Convert.ToInt32(aoSqlDataReader["GradeId"]),
                            ParameterId = Convert.ToInt32(aoSqlDataReader["ParameterId"]),
                            StudentId = Convert.ToInt32(aoSqlDataReader["StudentId"]),
                            Remark = Convert.ToString(aoSqlDataReader["Comment"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is sued to load grade details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillGrades(SqlDataReader aoSqlDataReader)
        {
            this.mlstGrades = new List<ObservationGrade>();
            while (aoSqlDataReader.Read())
            {
                this.mlstGrades.Add
                    (
                        new ObservationGrade
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is sued to load parameter details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillParameters(SqlDataReader aoSqlDataReader)
        {
            this.mlstParameters = new List<ObservationParameter>();
            while (aoSqlDataReader.Read())
            {
                this.mlstParameters.Add
                    (
                        new ObservationParameter
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            SkillId = Convert.ToInt32(aoSqlDataReader["SkillId"]),
                            Parameter = Convert.ToString(aoSqlDataReader["Parameter"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            ControlTypeId = Convert.ToInt32(aoSqlDataReader["ControlTypeId"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is sued to load skill details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSkills(SqlDataReader aoSqlDataReader)
        {
            this.mlstSkills = new List<ObservationSkill>();
            while (aoSqlDataReader.Read())
            {
                this.mlstSkills.Add
                    (
                        new ObservationSkill
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            DisplayOnReport = Convert.ToBoolean(aoSqlDataReader["DisplayOnReport"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"])
                        }
                    );
            }
        }
        
        #endregion

        #region Private Method(s)
        /// <summary>
        /// This method is sued to load student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private void FillRemarks(SqlDataReader aoSqlDataReader)
        {
            mlstRemarks = new List<ObservationRemarks>();
            while (aoSqlDataReader.Read())
            {
                mlstRemarks.Add
                    (
                        new ObservationRemarks
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["SkillId"]),
                            Remarks = Convert.ToString(aoSqlDataReader["Remarks"]),
                        }
                    );
            }
        }

        #endregion
    }
}
