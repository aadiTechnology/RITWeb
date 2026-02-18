using SchoolEntities;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Utility;

namespace MobileExportService.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMobileReceiptService" in both code and config file together.
    [ServiceContract]
    public interface IMobileExportService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "GetReceiptFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetReceiptFileName(int aiSchoolId, string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, string aiIsRefundFee, int aiStudentId, string asSerialNo);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetAdmissionReceiptFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetAdmissionReceiptFileName(int aiSchoolId, int aiAcademicYearId, int aiAdmissionId);

        //[OperationContract]
        //[WebInvoke(UriTemplate = "GetProgressReportFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string GetProgressReportFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, bool abIsPrimaryReport);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetTermAndFinalProgressReportFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetTermAndFinalProgressReportFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, int aiTermId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetChallanFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetChallanFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, string asPayableFor);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetLessonPlanFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetLessonPlanFileName(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asStartDate, string asEndDate);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetReportFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetReportFileName(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId, int aiReportId, Constants.ExportReports aoExportReports, List<ParameterPair> aoParameterPairs);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetITRFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetITRFileName(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiValueMember, int aiSelectAcademicYearId, int aiCategoryId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetFileNameForSNSChallan", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetFileNameForSNSChallan(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivisionId, int aiSchoolwiseStudentId, int aiFeeTypeId, string asPayableFor, int aiSelectedAcademicYearId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetCautionMoneyReceiptFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetCautionMoneyReceiptFileName(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetInternalFeeReceiptFileName", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetInternalFeeReceiptFileName(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId, string asReceiptNo, int aiInternalFeeDetailsId, bool abIsNextYearPayment, int aiSerialNumber);
    }
}
