// -----------------------------------------------------------------------
//  FileName	: AttendanceAlertConfigDC.cs
//	Created by	: Pravin
//	Date		: 5 May 2012
//	Description	: This class is used to Adding,Removing Users From Attendance Mail Configuration
// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    using Utility;
    using BookEntities;
    using SchoolEntities.Admin;
    using MasterEntities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttendanceAlertConfigDC
    {

        int miSchoolId;
        int miAcademicYearId;
        int miConfigId;       

        /// <summary>
        /// this is a default constructor.
        /// </summary>
        public AttendanceAlertConfigDC()
        {
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public AttendanceAlertConfigDC(int aiSchoolId, int aiAcademicYearId, int aiConfigId)
        {
            miSchoolId=aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miConfigId = aiConfigId;
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public AttendanceAlertConfigDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;            
        }

        /// <summary>
        /// This method is called for saving the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        /// <returns></returns>
        public int Save(AttendanceAlertConfigDetails olstAttendanceAlertConfigDetails)
        {
            int iCount=-1;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", olstAttendanceAlertConfigDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NoOfDays", olstAttendanceAlertConfigDetails.NoOfDays, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", olstAttendanceAlertConfigDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", miConfigId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_InsertAttendanceAlertConfig"))
                {
                    while (oReader.Read())
                        iCount = oReader[0].ToInt();
                    return iCount;
                }
            }
        }

        /// <summary>
        /// This method is called to delete the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        public void Delete(AttendanceAlertConfigDetails oAttendanceAlertConfigDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSqlStatement = "UPDATE ConfigAttendanceMessageDetails SET Is_Deleted=1," +
                                       " Updated_By_Id=" + oAttendanceAlertConfigDetails.InsertedById + "," +
                                       " Update_Date= N'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                       " WHERE Receiver_User_Id=" + oAttendanceAlertConfigDetails.UserId +
                                       " AND School_Id=" + miSchoolId;
                                       
                oSQLServerDbUtility.ExecuteTransaction(sSqlStatement);
            }
        }

        /// <summary>
        /// This mehotd is used to get the data for selected user.
        /// </summary>
        /// <param name="olstTempConfigDetails"></param>
        /// <returns></returns>
        public AttendanceAlertConfigDetails GetDetails()
        {
            AttendanceAlertConfigDetails olstAttendanceConfigDetails = new AttendanceAlertConfigDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId",miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", miConfigId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAttendanceMessageDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        olstAttendanceConfigDetails.UserId = Convert.ToInt32(oSqlDataReader["Receiver_User_Id"]);
                        olstAttendanceConfigDetails.RoleId = Convert.ToInt32(oSqlDataReader["User_Role_Id"]);
                        olstAttendanceConfigDetails.NoOfDays = Convert.ToInt16(oSqlDataReader["No_Of_Days"]);
                    }
                }
                
                return olstAttendanceConfigDetails;
            }
        }

        /// <summary>
        /// This method is used to get all the attendnace details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<AttendanceAlertConfigDetails> GetAll()
        {
            AttendanceAlertConfigDetails olstAttendanceAlertConfigDetails = new AttendanceAlertConfigDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAttendanceMessageDetails"))
                {
                    GenericClass<AttendanceAlertConfigDetails> oGeneric = new GenericClass<AttendanceAlertConfigDetails>();
                    return oGeneric.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to fill the details on poopup
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<AttendanceAlertDetails> GetMissingAttendanceDetailsForUser(int aiUserId,int aiStandardDivisionId)
        {
            AttendanceAlertDetails oAttendanceAlertDetails=new AttendanceAlertDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                if (aiStandardDivisionId != Constants.I_ZERO)
                    oSQLServerDbUtility.AddParameter("Standard_Division_Id", aiStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMissingAttendanceDetailsForAlert"))
                {
                    GenericClass<AttendanceAlertDetails> oGeneric = new GenericClass<AttendanceAlertDetails>();
                    return oGeneric.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to fill the abset student details (who is absent grater than 7 days) on poopup.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<AbsentStudentDetails> GetAbsentStudentDetailsForPopup(int aiUserId, out bool blIsLinkVisibel)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                blIsLinkVisibel = false;
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAbsetStudentDetailsForPopup"))
                {
                    List<AbsentStudentDetails> lstAbsentStudents = FillAbsentStudentDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                        blIsLinkVisibel = Convert.ToBoolean(oSqlDataReader["IsLinkVisibel"]);
                    
                    return lstAbsentStudents;
                }
            }
        }

        ///// <summary>
        ///// This method is used to get the dates for selected count.
        ///// </summary>
        ///// <param name="aiSchoolId"></param>
        ///// <param name="aiAcademicYearId"></param>
        ///// <param name="aiStandardDivisionId"></param>
        ///// <returns></returns>
        public List<DateTime> GetMissingAttendanceDates(int aiStandardDivisionId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMissingAttendanceDetailsForAlert"))
                {
                    List<DateTime> olstDates = new List<DateTime>();
                    while (oSqlDataReader.Read())
                        olstDates.Add(oSqlDataReader["AcademicDates"].ToDateTime());

                    return olstDates;
                }
            }
        }

        ///// <summary>
        ///// This method is used to get the nonPermenant Teacher details whose joining date is gretter than 1 Year.
        ///// </summary>
        public List<NonPermanentTeacherDetails> GetNonPermanentTeacherDetails()
        {            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetNonPermanantTeacherDetails"))
                    return FillTeacherDetails(oSqlDataReader);                               
            }           
        }

        ///// <summary>
        ///// This method is used to fill TEacher Details.
        ///// </summary>
        private List<NonPermanentTeacherDetails> FillTeacherDetails(SqlDataReader oSqlDataReader)
        {
            List<NonPermanentTeacherDetails> lstNonPermantTeachers = new List<NonPermanentTeacherDetails>();
            while (oSqlDataReader.Read())
            {
                NonPermanentTeacherDetails oNonPermanentTeacherDetails = new NonPermanentTeacherDetails();
                oNonPermanentTeacherDetails.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                oNonPermanentTeacherDetails.TeacherName = Convert.ToString(oSqlDataReader["UserName"]);
                oNonPermanentTeacherDetails.JoiningDate = Convert.ToDateTime(oSqlDataReader["JoiningDate"]);

                lstNonPermantTeachers.Add(oNonPermanentTeacherDetails);
            }
            return lstNonPermantTeachers;
        }

        private List<AbsentStudentDetails> FillAbsentStudentDetails(SqlDataReader oSqlDataReader)
        {
            List<AbsentStudentDetails> lstAbsentStudents = new List<AbsentStudentDetails>();
            while (oSqlDataReader.Read())
            {
                lstAbsentStudents.Add
                    (
                        new AbsentStudentDetails 
                        {
                            EnrolmentNumber = Convert.ToString(oSqlDataReader["EnrolmentNumber"]),
                            RollNo = Convert.ToInt32(oSqlDataReader["RollNo"]),
                            className = Convert.ToString(oSqlDataReader["className"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"])
                        }
                    );
            }
            return lstAbsentStudents;
        }    
    }
}
