using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SchoolEntities;
using SchoolEntities.Teacher;
using System.Data.SqlClient;
namespace DataCommunicator
{
   public class StudentsMonthlyStatusDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public StudentsMonthlyStatusDetailsDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }
        #endregion Constructor(s)

        #region Public Method(s)

        public void Save(string asXml, int aiCategoryId, int aiMonthId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("StudentsMonthlyStatusDetailsxml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentsMonthlyStatusDetails");
            }
        }

        public List<StudentsMonthlyStatusDetails> GetAllStudentsListforMonthlyStatus(int aiStandardId, int aiDivisionId, int aiCategoryId, int aiMonthId)
        {
            List<StudentsMonthlyStatusDetails> lstStudentsMonthlyStatusDetails = new List<StudentsMonthlyStatusDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentsListforMonthlyStatus"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstStudentsMonthlyStatusDetails.Add(new StudentsMonthlyStatusDetails
                        {
                            EnrollmentNumber = oSqlDataReader["Enrolment_Number"].ToString(),
                            RollNumber = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = oSqlDataReader["StudentName"].ToString(),
                            Remark = oSqlDataReader["Remark"].ToString(),
                            YearWise_Student_Id = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                        });
                    }
                }
            }
            return lstStudentsMonthlyStatusDetails;
        } 

        #endregion
    }
}
