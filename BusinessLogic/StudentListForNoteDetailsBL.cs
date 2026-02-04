using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    public class StudentListForNoteDetailsBL
    {
        #region Data Member(s)

        private StudentListForNoteDetailsDC moStudentListForNoteDetailsDC;

        #endregion

        #region Constructor(s)

        public StudentListForNoteDetailsBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moStudentListForNoteDetailsDC = new StudentListForNoteDetailsDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        public List<StudentListForNoteDetails> GetAllStudentList(int aiStandardId, int aiDivisionId)
        {
            return moStudentListForNoteDetailsDC.GetAllStudentList(aiStandardId, aiDivisionId);
        }

        #endregion
    }
}
