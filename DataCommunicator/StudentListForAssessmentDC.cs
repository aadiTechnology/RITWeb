using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;

namespace DataCommunicator
{
    public class StudentListForAssessmentDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId; 

        #endregion

        #region Constructor(s)
        
        public StudentListForAssessmentDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        public StudentListForAssessmentDC()
        {

        } 

        #endregion

        #region Public Method(s)
        
        public DataTable GetTestNames()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestsForStudent");
            }
        }

        public List<StudentListForAssessment> GetStudentList(int aiStandardId, int aiDivisionId, int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentListForAssessment"))
                {
                    List<StudentListForAssessment> lstStudentListForAssessment = new List<StudentListForAssessment>();
                    while (oSqlDataReader.Read())
                    {
                        lstStudentListForAssessment.Add(new StudentListForAssessment
                        {
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            StudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                            StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]),
                            IsSelfSubmitted = Convert.ToBoolean(oSqlDataReader["IsSelfSubmitted"]),
                            IsPeerSubmitted = Convert.ToBoolean(oSqlDataReader["IsPeerSubmitted"]),
                            IsParentSubmitted = Convert.ToBoolean(oSqlDataReader["IsParentSubmitted"]),
                        });
                    }
                    return lstStudentListForAssessment;
                }
            }
        } 

        #endregion
    }
}
