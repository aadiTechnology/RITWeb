using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using System.Data;

namespace BusinessLogic
{
    public class SurveyStudentBL
    {
        #region Data Member(s)
        
        private SurveyStudentDC moSurveyStudentDC; 

        #endregion

        #region Constructor(s)
        
        public SurveyStudentBL()
        {
            moSurveyStudentDC = new SurveyStudentDC();
        }

        public SurveyStudentBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moSurveyStudentDC = new SurveyStudentDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Method(s)

        public List<SurveySchool> Surveyschools
        {
            get { return moSurveyStudentDC.Surveyschools; }
        }

        public List<SurveyStudentCategory> SurveyStudentCategories
        {
            get { return moSurveyStudentDC.SurveyStudentCategories; }
        }

        public static List<SurveyStudentDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (sortExpression == string.Empty)
                sortExpression = "RegNo";
            else if (sortExpression.Contains(" asc") || sortExpression.Contains(" desc") || sortExpression.Contains(" ASC") || sortExpression.Contains(" DESC"))
                sortExpression = sortExpression.Replace(" asc", string.Empty).Replace(" desc", string.Empty).Replace(" ASC", string.Empty).Replace(" DESC", string.Empty);

            if (string.IsNullOrEmpty(sortDirection))
                sortDirection = "asc";

            return SurveyStudentDC.GetAll(aiSchoolId, aiAcademicYearId, sortExpression, sortDirection, startRowIndex, iEndIndex);
        }

        public static int Count(int aiSchoolId, int aiAcademicYearId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            return SurveyStudentDC.Count(aiSchoolId, aiAcademicYearId);
        }

        public List<Standard> GetAllEntities()
        {
            return moSurveyStudentDC.GetAllEntities();
        }

        public SurveyStudentDetails Get(int aiId)
        {
            return moSurveyStudentDC.Get(aiId);
        }

        public string Save(SurveyStudentDetails aoSurveyStudentDetails)
        {
            return moSurveyStudentDC.Save(aoSurveyStudentDetails);
        }

        public void Delete(int aiId)
        {
            moSurveyStudentDC.Delete(aiId);
        }

        public DataTable GetStandardList(int aiSchoolId, int aiAcademicYearId)
        {
            return moSurveyStudentDC.GetStandardList(aiSchoolId, aiAcademicYearId);
        }

        public List<SurveyStudentDetails> GetAllStudents(int aiCategoryId, string asStandardList)
        {
            return this.moSurveyStudentDC.GetAllStudents(aiCategoryId, asStandardList);
        }

        #endregion
    }
}
