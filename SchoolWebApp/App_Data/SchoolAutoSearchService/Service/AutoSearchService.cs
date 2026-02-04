using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using SchoolEntities;
using Utility;
using BusinessLogic;

namespace SchoolAutoSearchService.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class AutoSearchService : IAutoSearchService
    {
        public List<String> StudentAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStdDivId, bool asShowLeftStudents, bool abIncludeRegNo, bool abShowOnlyLeftStudents)
        {
            return AutoSuggestBL.GetStudentDataForAutoSearch(asSearchText, aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStdDivId, asShowLeftStudents, abIncludeRegNo, abShowOnlyLeftStudents);
        }

        public void RefreshStudentCache(int aiSchoolId, int aiAcademicYearId, List<int> alstYearwiseStudentIds, Constants.Action aoAction)
        {
            AutoSuggestBL.RefreshStudentCache(aiSchoolId, aiAcademicYearId, alstYearwiseStudentIds, aoAction);
        }

        public List<String> StaffAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted)
        {
            return AutoSuggestBL.GetStaffDataForAutoSearch(asSearchText, aiSchoolId, aiAcademicYearId, aiUserRoleId, asShowDeleted);
        }

        public List<String> StaffAutoSearchWithStatus(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted, int aiStatusId)
        {
            return AutoSuggestBL.GetStaffDataForAutoSearch(asSearchText, aiSchoolId, aiAcademicYearId, aiUserRoleId, asShowDeleted, aiStatusId);
        }

        public List<String> GetDataForMessageCenter(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId, bool abShowOnlyCoordinator)
        {
            return AutoSuggestBL.GetDataForMessageCenter(asSearchText, aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId, abShowOnlyCoordinator);
        }
        
        public void RefreshStaffCache(int aiSchoolId, int aiAcademicYearId, List<int> alstUserIds, Constants.Action aoAction)
        {
            AutoSuggestBL.RefreshStaffCache(aiSchoolId, aiAcademicYearId, alstUserIds, aoAction);
        }

        public List<String> UserAutoSearch(string asSearchText, int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, bool asShowDeleted)
        {
            return AutoSuggestBL.GetUserDataForAutoSearch(asSearchText, aiSchoolId, aiAcademicYearId, aiUserRoleId, asShowDeleted);
        }
    }
}
