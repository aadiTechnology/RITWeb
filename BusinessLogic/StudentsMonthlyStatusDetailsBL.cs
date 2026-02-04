using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    public class StudentsMonthlyStatusDetailsBL
    {
        #region Data Member(s)

        private StudentsMonthlyStatusDetailsDC moStudentsMonthlyStatusDetailsDC;

        #endregion

        #region Constructor(s)

        public StudentsMonthlyStatusDetailsBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moStudentsMonthlyStatusDetailsDC = new StudentsMonthlyStatusDetailsDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        public void Save(String asXml, int aiCategoryId, int aiMonthId)
        {
            moStudentsMonthlyStatusDetailsDC.Save(asXml, aiCategoryId, aiMonthId);
        }

        public List<StudentsMonthlyStatusDetails> GetAllStudentsListforMonthlyStatus(int aiStandardId, int aiDivisionId, int aiCategoryId, int aiMonthId)
        {
            return moStudentsMonthlyStatusDetailsDC.GetAllStudentsListforMonthlyStatus(aiStandardId, aiDivisionId, aiCategoryId, aiMonthId);
        }

        #endregion
    }
}
