using System;
using System.Linq;
using System.Collections.Generic;
using SchoolEntities;
using SchoolEntities.Dashboard;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Data;
using DataCommunicator;
using AccountsEntities;

namespace Dashboard.Service
{
    public class DashboardService : IDashboardService
    {
        #region --Public method(s)--
        /// <summary>
        /// This method is used to get fee widget related data.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public FeeSummary GetFeeSummary(int aiSchoolId, int aiAcademicYearId)
        {
            try
            {
                return StudentFeeDetailsCollectionDC.GetFeeSummary(aiSchoolId, aiAcademicYearId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}", aiSchoolId, aiAcademicYearId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get attendance related data.
        /// </summary>
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asDate"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public AttendanceSummary GetAttendanceSummary(int aiSchoolId, int aiAcademicYearId, string asDate, int aiUserId)
        {
            try
            {
                return AttendanceDetailsDC.GetAttendanceSummary(aiSchoolId, aiAcademicYearId, asDate, aiUserId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, asDate:{2}, aiUserId{3}", aiSchoolId, aiAcademicYearId, asDate, aiUserId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }
        /// <summary>
        /// This method is used to get exam wise and standard wise student performance.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public StandardwiseStudentPerformance GetStandardsPerformanceData(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiStandardId = 0)
        {
            try
            {
                return ProgressReportDC.GetStandardsPerformanceData(aiSchoolId, aiAcademicYearId, aiTestId, aiStandardId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiTestId:{2}, aiStandardId{3}", aiSchoolId, aiAcademicYearId, aiTestId, aiStandardId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get list of exam for selected standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<Exam> GetExamsForSelectedStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            try
            {
                DataTable dt = new DataTable();
                List<Exam> standardExams = new List<Exam>();
                TestCollectionBL oTestCollectionBL = new TestCollectionBL(aiSchoolId, aiAcademicYearId);

                dt = oTestCollectionBL.GetAllTestsForStandard(aiStandardId, true);

                /*Read each row from datatable and fill into the list*/
                foreach (DataRow standardExam in dt.Rows)
                    standardExams.Add(new Exam() { ExamId = Convert.ToInt32(standardExam["SchoolWise_Test_Id"]), ExamName = Convert.ToString(standardExam["SchoolWise_Test_Name"]) });

                return standardExams;
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiStandardId:{2}", aiSchoolId, aiAcademicYearId, aiStandardId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get account widget related data.
        /// </summary>
        /// <returns></returns>
         public InflowOutflowSummary GetAccountInflowOutflowSummary(int aiSchoolId, int aiFinancialYearId)
        {
            try
            {
                InflowOutflowSummary objAccountInflowOutflowDetails =  AccountsDC.GetAccountInflowOutflowSummary(aiSchoolId, aiFinancialYearId, true);
                objAccountInflowOutflowDetails.MaxSalaryAmount = new double[] { objAccountInflowOutflowDetails.MonthwiseInflowAmount.Max(), objAccountInflowOutflowDetails.MonthwiseOutflowAmount.Max() }.Max();
                return objAccountInflowOutflowDetails;
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("parameters -aiSchoolId:{0},aiFinancialYearId:{1}", aiSchoolId, aiFinancialYearId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used get payroll widget related data.
        /// </summary>
        /// <returns></returns>
        public PayrollSummary GetPayrollSummary(int aiSchoolId, int aiYear, int aiFinancialYearId, int aiMonth)
        {
            try
            {
                return SalaryDetailsDC.GetPayrollSummary(aiSchoolId, aiYear, aiFinancialYearId, aiMonth, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("parameters - aiSchoolId:{0}, aiYear:{1}, aiFinancialYearId:{2}, aiMonth:{3}", aiSchoolId, aiYear, aiFinancialYearId, aiMonth);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }
        /// <summary>
        /// This method is used get upcoming staff birthday list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asView"></param>
        /// <returns></returns>
        public List<StaffDetails> GetUpcomingStaffBdayList(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, string asView)
        {
            try
            {
                return SchoolUserDC.GetUpcomingStaffBdayList(aiSchoolId, aiAcademicYearId, aiUserRoleId, asView, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiUserRoleId:{2}, asView{3}", aiSchoolId, aiAcademicYearId, aiUserRoleId, asView);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used get albums to show in photo gallery.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiMonth"></param>
        /// <param name="aiYear"></param>
        /// <param name="abSetPreviousMonth"></param>
        /// <returns></returns>
        public List<PhotoGalley> GetAlbumsList(int aiSchoolId, int aiMonth, int aiYear, bool abSetPreviousMonth,int aiUserId)
        {
            try
            {
                return SchoolUserDC.GetAlbumsList(aiSchoolId, aiMonth, aiYear, aiUserId, abSetPreviousMonth, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiMonth:{1}, aiYear:{2}, abSetPreviousMonth :{3},aiUserId:{0}", aiSchoolId, aiMonth, aiYear, abSetPreviousMonth,aiUserId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get student count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public StudentCountDetails GetStudentCountDetails(int aiSchoolId, int aiAcademicYearId)
        {
            try
            {
                return StudentDC.GetStudentCountDetails(aiSchoolId, aiAcademicYearId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}", aiSchoolId, aiAcademicYearId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get staff count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public StaffCountDetails GetStaffCountDetails(int aiSchoolId, int aiAcademicYearId)
        {
            try
            {
                return SchoolUserDC.GetStaffCountDetails(aiSchoolId, aiAcademicYearId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}", aiSchoolId, aiAcademicYearId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get library count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public LibraryCountDetails GetLibraryCountDetails(int aiSchoolId)
        {
            try
            {
                return BookCollectionDC.GetLibraryCountDetails(aiSchoolId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}", aiSchoolId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get top 10 feedbacks to display on feedback widget.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asDesignationId"></param>
        /// <param name="abIsAccountsCumAdminOfficer"></param>
        /// <returns></returns>
        public List<UserFeedBack> GetUserFeedback(int aiSchoolId, int aiUserRoleId, string asDesignationId, bool abIsAccountsCumAdminOfficer = false)
        {
            try
            {
                return FeedbackDetailsDC.GetUserFeedback(Convert.ToInt32(aiSchoolId), aiUserRoleId, asDesignationId, true, abIsAccountsCumAdminOfficer);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiUserRoleId:{0}, aiSchoolId:{1}", aiUserRoleId, aiSchoolId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }
        
        /// <summary>
        /// This method is used to get unread messages list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReceiverId"></param>
        /// <param name="aiReceiverRoleId"></param>
        /// <returns></returns>
        public UnreadMessageDetails GetUnreadMessageList(int aiSchoolId, int aiAcademicYearId, int aiReceiverId, int aiReceiverRoleId, string asProfilePicUpdDt = "")
        {
            try
            {
                return MessageDetailsCollectionDC.GetUnreadMessageList(aiSchoolId, aiAcademicYearId, aiReceiverId, aiReceiverRoleId, true, asProfilePicUpdDt);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiReceiverId:{2}, aiReceiverRoleId:{3}", aiSchoolId, aiAcademicYearId, aiReceiverId, aiReceiverRoleId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// this method is used used to get upcoming events.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public List<UpcomingEvents> GetUpcomingEvents(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiUserRoleId, string isScreenFullAccess)
        {
            try
            {
                return EventDescriptionDC.GetUpcomingEvents(aiSchoolId, aiAcademicYearId, aiUserId, aiUserRoleId, isScreenFullAccess, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiUserRoleId:{2}, isScreenFullAccess:{3}", aiSchoolId, aiAcademicYearId, aiUserRoleId, isScreenFullAccess);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used for to get attendance details for class teacher.
        /// </summary>
        /// <param name="aischoolid"></param>
        /// <param name="aiuserid"></param>
        /// <param name="aiuserroleid"></param>
        /// <returns></returns>
        public ClasswiseAttendanceSummary GetClasswiseAttendanceSummary(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId = 0)
        {
            try
            {
                return AttendanceDetailsDC.GetClasswiseAttendanceSummary(aiSchoolId, aiAcademicYearId, aiUserId, aiStandardId, true);
            }
            catch (Exception oEx)
            {
                string sMessage = string.Format("Parameters - aiSchoolId:{0}, aiAcademicYearId:{1}, aiTestId:{2}, aiStandardId{3}", aiSchoolId, aiAcademicYearId, aiUserId, aiStandardId);
                ExceptionHandler.WriteExceptionToErrorLog(oEx, System.Reflection.MethodBase.GetCurrentMethod(), sMessage, true);
            }

            return null;
        }

        /// <summary>
        /// This method is used to save error log.
        /// </summary>
        /// <param name="asSchoolId"></param>
        /// <param name="asMessage"></param>
        /// <param name="asBrowserInfo"></param>
        /// <param name="asMethodName"></param>
        /// <param name="asUserId"></param>
        public void LogErrorAtClientSide(string asSchoolId, string asMessage, string asBrowserInfo, string asMethodName, int aiUserId)
        {
            try
            {
                ErrorLogDC.WriteExceptionToErrorLog(asMessage, asMethodName, asBrowserInfo, aiUserId, Convert.ToInt32(asSchoolId), true);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion --Public method(s)--
    }
}