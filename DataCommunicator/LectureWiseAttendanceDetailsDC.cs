using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using System.Collections;
using SchoolEntities.Admin;
using SchoolEntities.Dashboard;
using System.Linq;
using SchoolEntities;

namespace DataCommunicator
{
  public  class LectureWiseAttendanceDetailsDC : DataCommunicatorBaseDC
    {
      #region structure

        public struct LectureWiseAttendanceDetailsStruct
        {
            public int miSchoolWiseAttendanceId;
            public int miSchoolId;
            public DateTime mdtAttendanceDate;
            public int miStudentId;
            public string msIsPresent;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miStandardDivisionId;
            public int miAcademicYearId;
            public int miLectureNo;
            public int miSubjectId;
        }

        #endregion

        #region Data members

        private LectureWiseAttendanceDetailsStruct moSchoolWiseAttendanceDetailsStruct;
       // private DayDetails moDayDetails;

        #endregion

        #region Properties

        public LectureWiseAttendanceDetailsStruct SchoolWiseAttendanceDetailsStructDetails
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct = value;
            }
        }

        //public DayDetails DayDetails
        //{
        //    get
        //    {
        //        return moDayDetails;
        //    }
        //}
        #endregion

        #region Constructors

        public LectureWiseAttendanceDetailsDC()
        {
        }
        public LectureWiseAttendanceDetailsDC(int aiId)
        {
            LoadSchoolWiseAttendanceDetailsDetails(aiId);
        }
        #endregion



        /// <summary>
        /// This method is used to mark the student's attendance.
        /// </summary>
        /// <param name="sAttendanceXML"></param>
        public void MarkStudentAttendence(string sAttendanceXML, int LectureNo, int SubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moSchoolWiseAttendanceDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureWiseAttendance", sAttendanceXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Attendance_Date", moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", moSchoolWiseAttendanceDetailsStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureNo", LectureNo, SqlDbType.Int); //
                oSQLServerDbUtility.AddParameter("SubjectId", SubjectId, SqlDbType.Int);  //
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkStudentsLectureWiseAttendance");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="adtTodaysDate"></param>
        /// <returns></returns>
        public static void DeleteSchoolWiseAttendanceDetails(int aiSchoolId, int aiAcademicYearId, string asAttendanceDate, int aiStdDivId, int LectureNo, int SubjectId)
        {
            string sDeleteStatement = " DELETE FROM LectureWise_Attendance_Details " +
                      " WHERE " +
                      " School_Id = " + aiSchoolId +
                      " AND Attendance_Date = N'" + asAttendanceDate + "' " +
                      " AND Academic_Year_Id = " + aiAcademicYearId +
                      " AND Standard_Division_Id = " + aiStdDivId +
                         " AND Lecture_No = " + LectureNo +
                           " AND Subject_Id = " + SubjectId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="adtTodaysDate"></param>
        /// <returns></returns>
        public DataSet FetchAttendenceDetails(int aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardDivisionId, DateTime adtTodaysDate, int LectureNo, int SubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("dateOfAttendence", adtTodaysDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LectureNo", LectureNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", SubjectId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLectureWiseAttendance");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiId"></param>
        public void LoadSchoolWiseAttendanceDetailsDetails(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolWiseAttendanceDetailsDataFromDatabase(aiId);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {

                            if (oDR["SchoolWise_Attendance_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId = Convert.ToInt32(oDR["SchoolWise_Attendance_Id"].ToString());
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                            if (oDR["Attendance_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate = Convert.ToDateTime(oDR["Attendance_Date"].ToString());
                            if (oDR["Student_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miStudentId = Convert.ToInt32(oDR["Student_Id"].ToString());
                            if (oDR["Is_Present"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.msIsPresent = oDR["Is_Present"].ToString();
                            //if (oDR["Is_Deleted"] != DBNull.Value)
                            //    moSchoolWiseAttendanceDetailsStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public string FetchSchoolWiseAttendanceDetailsDataFromDatabase(int aiId)
        {
            string sSelectStatement = " SELECT  " +
                "LectureWise_Attendance_Id" +
                " , School_Id" +
                " , Attendance_Date" +
                " , Student_Id" +
                " , Is_Present" +
              
                " , Insert_Date" +
                " , Inserted_By_Id" +
                " , Update_Date" +
                " , Updated_By_Id" +

            " FROM  " +
                "LectureWise_Attendance_Details " +
            " WHERE " +
                 " LectureWise_Attendance_Id = " + aiId ;
            return sSelectStatement;
        }
    }
}
