using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    public class HealthDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId, miAcademicYearId, miUpdatedByid;        

        #endregion

        #region Constructor(s)

        public HealthDetailsDC()
        {             
        }

        public HealthDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedByid = aiUpdatedById;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get student details for Health.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<HealthDetails> GetAllStudentDetails(int aiStandardId, int aiDivisionId)
        {
            List<HealthDetails> lstHealthDetails = new List<HealthDetails>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsForHealth"))
                {
                    lstHealthDetails = FillStudentDetails(oSqlDataReader);
                }

            }
            return lstHealthDetails;           
        }

        /// <summary>
        /// This method is used to get students health details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentHealthDetails> GetStudentHealthDetails(int aiStudentId)
        {
            List<StudentHealthDetails> lstStudentHealthDetails = new List<StudentHealthDetails>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentHealthDetails"))
                {
                    lstStudentHealthDetails = FillStudentHealthDetails(oSqlDataReader);
                }
            }

            return lstStudentHealthDetails;
        }

        /// <summary>
        /// This method is used to save student health details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="asHealthDetails"></param>
        /// <param name="aiUpdatedById"></param>
        public void SaveStudentHealthDetails(int aiStudentId, string asHealthDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HealthDetailsxml", asHealthDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedByid, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentHealthDetails");
            }
        }

        /// <summary>
        /// This method is used to submit students health details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiIsPublish"></param>
        public void SubmitStudentHealthDetails(int aiStudentId, int aiIsPublish)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", aiIsPublish, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentHealthDetails");
            }
        }

        /// <summary>
        /// This method is used to get the student details of selected class for import health details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public List<ImportHealthDetails> GetStudentDetailsForImport(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, string sFilter, string asSortExpression, int startRowIndex, int iEndIndex)
        {
            List<ImportHealthDetails> lstImportHealthDetails = new List<ImportHealthDetails>();

            if (sFilter != string.Empty && sFilter != null)
                sFilter = "AND (vwBSD.Enrolment_Number = '" + sFilter + "' OR vwBSD.StudentName LIKE " + "  '%" + sFilter + "%' )";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", " ORDER BY " + asSortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsForImportHealthDetails"))
                {
                    lstImportHealthDetails = FillStudentDetailsForImport(oSqlDataReader);
                }
            }

            return lstImportHealthDetails;
        }

        /// <summary>
        /// This method is used to save Students health details through import.
        /// </summary>
        /// <param name="aiUpdatedById"></param>
        /// <param name="asStudentHealthDetails"></param>
        public void InsertMultipalStudentHealthDetails(int aiUpdatedById, string asStudentHealthDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentHealthDetailsxml", asStudentHealthDetails, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertMultipalStudentsHealthDetails");
            }
        }

        public List<SiblingStudentDetails> GetSiblingStudentDetails()
        {
            List<SiblingStudentDetails> mlstSiblingStudentDetails = new List<SiblingStudentDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSiblingDetailsForImportHealth"))
                {
                    while (oSqlDataReader.Read())
                    {
                        SiblingStudentDetails oSiblingStudentDetails = new SiblingStudentDetails();
                        oSiblingStudentDetails.YearwiseStudentId = Convert.ToInt32(oSqlDataReader["FirstStudentId"]);
                        oSiblingStudentDetails.SiblingStudentId = Convert.ToInt32(oSqlDataReader["SecondStudentId"]);
                        oSiblingStudentDetails.EnrolmentNumber = Convert.ToString(oSqlDataReader["FirstEnrolmentNo"]);
                        oSiblingStudentDetails.SiblingEnrolmentNumber = Convert.ToString(oSqlDataReader["SecondEnrolmentNo"]);

                        mlstSiblingStudentDetails.Add(oSiblingStudentDetails);
                    }
                }
            }
            return mlstSiblingStudentDetails;
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used for fill the student details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<HealthDetails> FillStudentDetails(SqlDataReader oSqlDataReader)
        {
            List<HealthDetails> lstHealthDetails = new List<HealthDetails>();
            while (oSqlDataReader.Read())
            {
                HealthDetails oHealthDetails = new HealthDetails();
                oHealthDetails.RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]);
                oHealthDetails.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                oHealthDetails.Status = Convert.ToInt32(oSqlDataReader["Status"]);
                oHealthDetails.StudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]);
                oHealthDetails.IsSubmited = Convert.ToInt32(oSqlDataReader["IsSubmited"]);
                oHealthDetails.IsLeft = Convert.ToInt32(oSqlDataReader["IsLeft"]);

                lstHealthDetails.Add(oHealthDetails);
            }
            return lstHealthDetails;
        }

        /// <summary>
        /// This method is used to fill students health details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentHealthDetails> FillStudentHealthDetails(SqlDataReader oSqlDataReader)
        {
            List<StudentHealthDetails> lstStudentHealthDetails = new List<StudentHealthDetails>();
            while (oSqlDataReader.Read())
            {
                StudentHealthDetails oStudentHealthDetails = new StudentHealthDetails();
                oStudentHealthDetails.StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]);
                oStudentHealthDetails.RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]);
                oStudentHealthDetails.EnrolmentNo = Convert.ToString(oSqlDataReader["Enrolment_Number"]);
                oStudentHealthDetails.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                oStudentHealthDetails.ClassName = Convert.ToString(oSqlDataReader["className"]);
                oStudentHealthDetails.ComponentId = Convert.ToInt32(oSqlDataReader["ComponentId"]);
                oStudentHealthDetails.Component = Convert.ToString(oSqlDataReader["Components"]);
                oStudentHealthDetails.ParameterId = Convert.ToInt32(oSqlDataReader["ParameterId"]);
                oStudentHealthDetails.Parameter = Convert.ToString(oSqlDataReader["Parameter"]);
                oStudentHealthDetails.Answer = Convert.ToString(oSqlDataReader["Answer"]);
                oStudentHealthDetails.SubmitStatus = Convert.ToBoolean(oSqlDataReader["SubmitStatus"]);
                oStudentHealthDetails.IsDataSaved = Convert.ToInt32(oSqlDataReader["IsSaved"]);

                lstStudentHealthDetails.Add(oStudentHealthDetails);
            }
            return lstStudentHealthDetails;
        }

        /// <summary>
        /// This method is used to fill students details for Import.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<ImportHealthDetails> FillStudentDetailsForImport(SqlDataReader oSqlDataReader)
        {
            List<ImportHealthDetails> lstImportHealthDetails = new List<ImportHealthDetails>();
            while (oSqlDataReader.Read())
            {
                ImportHealthDetails oImportHealthDetails = new ImportHealthDetails();
                oImportHealthDetails.TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]);
                oImportHealthDetails.StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]);
                oImportHealthDetails.RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]);
                oImportHealthDetails.EnrolmentNo = Convert.ToString(oSqlDataReader["Enrolment_Number"]);
                oImportHealthDetails.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                oImportHealthDetails.ClassName = Convert.ToString(oSqlDataReader["className"]);
                if (oSqlDataReader["FatherAdharcardNo"] != DBNull.Value)
                    oImportHealthDetails.FatherAadharCardNo = Convert.ToString(oSqlDataReader["FatherAdharcardNo"]);
                if (oSqlDataReader["MotherAadharCardNo"] != DBNull.Value)
                    oImportHealthDetails.MotherAadharCardNo = Convert.ToString(oSqlDataReader["MotherAadharCardNo"]);
                if (oSqlDataReader["FamilyMonthlyIncome"] != DBNull.Value)
                    oImportHealthDetails.FamilyMonthlyIncome = Convert.ToDecimal(oSqlDataReader["FamilyMonthlyIncome"]);

                lstImportHealthDetails.Add(oImportHealthDetails);
            }
            return lstImportHealthDetails;
        }

        #endregion
    }
}
