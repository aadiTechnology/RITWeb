using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using SchoolEntities;
using Utility;

namespace SchoolAutoSearchService.Service
{
    [ServiceContract(Namespace="http://www.riteschool.com")]
    public interface IAutoSearchService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "StudentAutoSearch", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> StudentAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStdDivId, bool asShowLeftStudents, bool abIncludeRegNo, bool abShowOnlyLeftStudents);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        void RefreshStudentCache(int aiSchoolId, int aiAcademicYearId, List<int> alstYearwiseStudentIds, Constants.Action aoAction);

        [OperationContract]
        [WebInvoke(UriTemplate = "StaffAutoSearch", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> StaffAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        void RefreshStaffCache(int aiSchoolId, int aiAcademicYearId, List<int> alstUserIds, Constants.Action aoAction);

        [OperationContract]
        [WebInvoke(UriTemplate = "UserAutoSearch", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> UserAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted);

        [OperationContract]
        [WebInvoke(UriTemplate = "StaffAutoSearchWithStatus", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> StaffAutoSearchWithStatus(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted, int aiStatusId);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetDataForMessageCenter", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> GetDataForMessageCenter(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId, bool abShowOnlyCoordinator);
    }
}
