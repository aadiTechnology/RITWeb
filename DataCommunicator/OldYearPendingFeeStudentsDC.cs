using System;
using System.Collections.Generic;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class OldYearPendingFeeStudentsDC
    {
        #region " Data Members "

        public int miSchoolId;
        public int miAcademicYearId;
        public int miUpdatedById;

        #endregion
        #region " Constructor "

        public OldYearPendingFeeStudentsDC() { }

        public OldYearPendingFeeStudentsDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }
        #endregion

        public OldYearPendingFeeReport GetOldYearPendingFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiStandardId, int aiDivisionId, int aiFromYear, int aiToYear, int aiIncludeLateFee)
        {
            OldYearPendingFeeReport oOldYearPendingFeeReport = new OldYearPendingFeeReport();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FromYear", aiFromYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ToYear", aiToYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeLateFee", aiIncludeLateFee, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOldPendingFeeDetailsForAllYears"))
                {
                    oOldYearPendingFeeReport.OldYearPendingFeeStudents = new List<OldYearPendingFeeStudent>();
                    while (oSqlDataReader.Read())
                    {
                        oOldYearPendingFeeReport.OldYearPendingFeeStudents.Add(
                            new OldYearPendingFeeStudent
                            {
                                YearWiseStudentId = Convert.ToInt32(oSqlDataReader["Yearwise_Student_Id"]),
                                RegNo = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                                Class = Convert.ToString(oSqlDataReader["ClassName"]),
                                RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                                StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                                MobileNo = Convert.ToString(oSqlDataReader["Mobile_Number"]),
                                OriginalStandardId = Convert.ToInt32(oSqlDataReader["Original_Standard_Id"]),
                                OriginalDivisionId = Convert.ToInt32(oSqlDataReader["Original_Division_Id"])
                            });

                    }


                    if (oSqlDataReader.NextResult())
                    {
                        oOldYearPendingFeeReport.PendingFees = new List<OldYearPendingFee>();
                        while (oSqlDataReader.Read())
                        {
                            oOldYearPendingFeeReport.PendingFees.Add(
                                new OldYearPendingFee
                                {
                                    //OldYearPendingFeeAmount oPendingAmount = new OldYearPendingFeeAmount();
                                    StudentId = oSqlDataReader["SchoolWise_Student_Id"].ToInt(),
                                    AcademicYearId = oSqlDataReader["Academic_Year_Id"].ToInt(),
                                    AcademicYear = oSqlDataReader["Academic_Year_Name"].ToString(),
                                    Amount = oSqlDataReader["Amount"].ToInt()

                                });
                        }

                    }


                    return oOldYearPendingFeeReport;
                }
            }
        }
    }
}
