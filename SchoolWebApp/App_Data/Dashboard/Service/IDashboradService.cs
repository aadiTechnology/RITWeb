using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using SchoolEntities.Dashboard;
using SchoolEntities;
using AccountsEntities;

namespace Dashboard.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDashboardService" in both code and config file together.
    [ServiceContract]
    public interface IDashboardService
    {

        [OperationContract]
        [WebInvoke(UriTemplate = "GetFeeSummary", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FeeSummary GetFeeSummary(int aiSchoolId, int aiAcademicYearId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAttendanceSummary", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        AttendanceSummary GetAttendanceSummary(int aiSchoolId, int aiAcademicYearId, string asDate, int aiUserId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetStandardsPerformanceData", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        StandardwiseStudentPerformance GetStandardsPerformanceData(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiStandardId = 0);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetExamsForSelectedStandard", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Exam> GetExamsForSelectedStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAccountInflowOutflowSummary", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        InflowOutflowSummary GetAccountInflowOutflowSummary(int aiSchoolId, int aiFinancialYearId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetPayrollSummary", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        PayrollSummary GetPayrollSummary(int aiSchoolId, int aiYear, int aiFinancialYearId, int aiMonth);


        [OperationContract]
        [WebInvoke(UriTemplate = "GetUpcomingStaffBdayList", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<StaffDetails> GetUpcomingStaffBdayList(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, string asView);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAlbumsList", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PhotoGalley> GetAlbumsList(int aiSchoolId, int aiMonth, int aiYear, bool abSetPreviousMonth, int aiUserId);
	

        [OperationContract]
        [WebInvoke(UriTemplate = "GetStudentCountDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        StudentCountDetails GetStudentCountDetails(int aiSchoolId, int aiAcademicYearId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetStaffCountDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        StaffCountDetails GetStaffCountDetails(int aiSchoolId, int aiAcademicYearId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetLibraryCountDetails", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        LibraryCountDetails GetLibraryCountDetails(int aiSchoolId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUserFeedback", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<UserFeedBack> GetUserFeedback(int aiSchoolId, int aiUserRoleId, string asDesignationId, bool abIsAccountsCumAdminOfficer = false);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUnreadMessageList", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        UnreadMessageDetails GetUnreadMessageList(int aiSchoolId, int aiAcademicYearId, int aiReceiverId, int aiReceiverRoleId, string asProfilePicUpdDt = "");

        [OperationContract]
        [WebInvoke(UriTemplate = "GetUpcomingEvents", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<UpcomingEvents> GetUpcomingEvents(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiUserRoleId, string isScreenFullAccess);

        [OperationContract]
        [WebInvoke(UriTemplate = "LogErrorAtClientSide", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void LogErrorAtClientSide(string asSchoolId, string asMessage, string asBrowserInfo, string asMethodName, int aiUserId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetClasswiseAttendanceSummary", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ClasswiseAttendanceSummary GetClasswiseAttendanceSummary(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId = 0);
    }
}