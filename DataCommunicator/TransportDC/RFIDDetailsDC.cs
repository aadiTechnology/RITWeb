using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Transport;
using Utility;

namespace DataCommunicator.TransportDC
{
    public class RFIDDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public RFIDDetailsDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
        }

        public RFIDDetailsDC()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to save new RFID.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiRFID"></param>
        public void Save(int aiStudentId, string asRFID)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Schoolwise_Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RFID", asRFID, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveNewRFIDDetails");
            }
        }

        /// <summary>
        ///  This method is used to return all searched student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<RFIDDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, string asFilter, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsForUpdateRFID"))
                {
                    List<RFIDDetails> lstSearchedStudent = new List<RFIDDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstSearchedStudent.Add(new RFIDDetails
                        {
                            SchoolWiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolWise_Student_Id"]),
                            ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                            EnrolmentNumber = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            RFID = Convert.ToString(oSqlDataReader["RFID"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]),
                            UserId = Convert.ToInt32(oSqlDataReader["User_Id"])
                        });
                    }
                    return lstSearchedStudent;
                }
            }
        }

        /// <summary>
        /// This method is sued to check RFID duplication.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="asRFID"></param>
        /// <returns></returns>
        public string ValidateRFID(int aiSchoolwiseStudentId, string asRFID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RFID", asRFID, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 500);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateRFID");
                return oSqlParameter.Value.ToString();
            }
        }

        #endregion        
    }
}
