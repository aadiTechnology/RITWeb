using System.Collections.Generic;
using DataCommunicator;

namespace BusinessLogic
{
    public class LessonPlanStdSubjectBL
    {
        #region Data Member(s)
        
        private LessonPlanStdSubjectDC moLessonPlanStdSubjectDC; 

        #endregion

        #region Constructor(s)
        
        public LessonPlanStdSubjectBL()
        {
            this.moLessonPlanStdSubjectDC = new LessonPlanStdSubjectDC();
        }

        public LessonPlanStdSubjectBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moLessonPlanStdSubjectDC = new LessonPlanStdSubjectDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return all standard wise subjects.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<LessonPlanEntities.LessonPlanStdSubject> GetAllSubjects(int aiStandardId)
        {
            return this.moLessonPlanStdSubjectDC.GetAllSubjects(aiStandardId);
        }

        /// <summary>
        /// This method is used to save subject details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="asSubjectIds"></param>
        public void Save(int aiStandardId, string asSubjectIds)
        {
            this.moLessonPlanStdSubjectDC.Save(aiStandardId, asSubjectIds);
        } 

        #endregion
    }
}
