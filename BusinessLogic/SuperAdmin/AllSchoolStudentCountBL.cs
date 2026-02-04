using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
namespace BusinessLogic
{
    public class AllSchoolStudentCountBL
    {
        public List<AcademicYear> GetAllAcademicYears()
        {
            AllSchoolStudentCountDC allSchoolStudentCountDC = new AllSchoolStudentCountDC();
            return allSchoolStudentCountDC.GetAllAcademicYears();
        }

        public List<AllStudentCount> GetStudentsCountList(string asAcademicYear)
        {
            AllSchoolStudentCountDC allSchoolStudentCountDC = new AllSchoolStudentCountDC();
            return allSchoolStudentCountDC.GetStudentsCountList(asAcademicYear);
        }
    }
}