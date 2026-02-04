using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.ProgressReport;

namespace BusinessLogic
{
    public class StudentMarksBL
    {
        #region Data MEmber(s)
        
        private StudentMarksDC moStudentMarksDC; 

        #endregion

        #region Constructor(s)

        public StudentMarksBL()
        {
            moStudentMarksDC = new StudentMarksDC();
        }

        public StudentMarksBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedBYId)
        {
            moStudentMarksDC = new StudentMarksDC(aiSchoolId, aiAcademicYearId, aiUpdatedBYId);
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to return mark details.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public StudentConsolidatedMarkDetails GetAllDetails(int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiTestId, int aiSubjectId)
        {
            return moStudentMarksDC.GetAllDetails(aiAcademicYearId, aiStandardId, aiDivisionId, aiTestId, aiSubjectId);
        }

        /// <summary>
        /// This method is used to return test list.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<Test> GetAllTestsForClassSUbject(int aiAcademicYearId, int aiStdDivId, int aiSubjectId)
        {
            return moStudentMarksDC.GetAllTestsForClassSUbject(aiAcademicYearId, aiStdDivId, aiSubjectId);
        } 

        #endregion
    }
}
