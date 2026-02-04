using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
namespace BusinessLogic
{
    public class StudentExamWiseSubjectMarksDetailsBL
    {

        #region Data Member(s)

        private StudentExamWiseSubjectMarksDetailsDC moStudentExamWiseSubjectMarksDetailsDC;

        #endregion

        #region Constructor(s)

        public StudentExamWiseSubjectMarksDetailsBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moStudentExamWiseSubjectMarksDetailsDC = new StudentExamWiseSubjectMarksDetailsDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// These method returns exams
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>

        public List<StudentExamWiseSubjectMarksDetails> GetExams(int aiStudentId)
        {
            return this.moStudentExamWiseSubjectMarksDetailsDC.GetExams(aiStudentId);
        }

        /// <summary>
        /// these method return subjects and marks according to selected exam
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentExamWiseSubjectMarksDetails> GetAllSubjects(int aiTestId, int aiStudentId)
        {
            return this.moStudentExamWiseSubjectMarksDetailsDC.GetAllSubjects(aiTestId, aiStudentId);
        }

        #endregion
    }
}
