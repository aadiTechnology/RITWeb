using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    public class AssignSummaryGradesBL
    {
        #region Data Member(s)

        private AssignSummaryGradesDC moAssignSummaryGradesDC;

        #endregion

        #region Constructor(s)

        public AssignSummaryGradesBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moAssignSummaryGradesDC = new AssignSummaryGradesDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Property(s)

        public ButtonStatesforAssignSummaryGrades ButtonStates
        {
            get { return moAssignSummaryGradesDC.ButtonStates; }
        }

        public string TestName
        {
            get { return this.moAssignSummaryGradesDC.TestName; }
        }

        public string SubjectName
        {
            get { return this.moAssignSummaryGradesDC.SubjectName; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get students to fill listview.
        /// </summary>
        /// <param name="aiStddivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<AssignSummaryGradesDetails> GetAll(int aiStddivId, int aiSubjectId, int aiTestId)
        {
            return this.moAssignSummaryGradesDC.GetAll(aiStddivId, aiSubjectId, aiTestId);
        }

        /// <summary>
        /// this method is used to save grade details
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="TestId"></param>
        public void Save(string asXml, int aiSubjectId, int aiTestId)
        {
            moAssignSummaryGradesDC.Save(asXml, aiSubjectId, aiTestId);
        }

        /// <summary>
        /// This method is used to submit and unsubmit student grade details.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="abIsSubmitted"></param>

        public void SubmitSummaryGradeDetails(int aiStandardDivId, int aiSubjectId, int aiTestId, bool abIsSubmitted)
        {
            moAssignSummaryGradesDC.Submit(aiStandardDivId, aiSubjectId, aiTestId, abIsSubmitted);
        }

        #endregion
    }
}
