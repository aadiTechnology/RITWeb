using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class StudentListForAssessmentBL
    {
        #region Data MEmber(s)
        
        private StudentListForAssessmentDC moStudentListForAssessmentDC; 

        #endregion

        #region Constructor(s)
        
        public StudentListForAssessmentBL()
        {
            moStudentListForAssessmentDC = new StudentListForAssessmentDC();
        }

        public StudentListForAssessmentBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moStudentListForAssessmentDC = new StudentListForAssessmentDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Public Method(s)
        
        public DataTable GetTestNames()
        {
            return moStudentListForAssessmentDC.GetTestNames();
        }

        public List<StudentListForAssessment> GetStudentList(int aiStandardId, int aiDivisionId, int aiTestId)
        {
            return moStudentListForAssessmentDC.GetStudentList(aiStandardId, aiDivisionId, aiTestId);
        } 

        #endregion
    }
}
