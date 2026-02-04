using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class StudentAssessmentBL
    {
        #region Data member(s)

        private StudentAssessmentDC moStudentAssessmentDC;

        #endregion

        #region Constructor(s)

        public StudentAssessmentBL()
        {
            moStudentAssessmentDC = new StudentAssessmentDC();
        }

        public StudentAssessmentBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moStudentAssessmentDC = new StudentAssessmentDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Property(s)
        
        public ButtonStatesForStudentAssessment ButtonStates
        {
            get { return moStudentAssessmentDC.ButtonStates; }
        }

        public List<CategorywiseComment> CategorywiseComments
        {
            get { return moStudentAssessmentDC.CategorywiseComments; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to get students to fill student dropdown.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public DataTable GetStudents(int aiStudentId, int aiCategoryId, int aiAcademicYearId)
        {
            return moStudentAssessmentDC.GetStudents(aiStudentId, aiCategoryId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get test names to fill test name dropdown.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTestNames(int aiAcademicYearId)
        {
            return moStudentAssessmentDC.GetTestNames(aiAcademicYearId);
        }

        public DataTable GetAcademicYear(int aiStudentId)
        {
            return moStudentAssessmentDC.GetAcademicYear(aiStudentId);
        }

        /// <summary>
        /// This method is used to fill listview.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentAssessmentDetails> GetStudentAssessmentDetails(int aiAcademiYearId, int aiStandardId, int aiCategoryId, int aiStudentId, int aiTestId)
        {
            return moStudentAssessmentDC.GetStudentAssessmentDetails(aiAcademiYearId, aiStandardId, aiCategoryId, aiStudentId, aiTestId);
        }

        /// <summary>
        /// This method is used to get list of student fav details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentFavouriteListDetails> GetListOfStudentFavDetails(int aiAcademicYearId, int aiStandardId, int aiStudentId, int aiTestId)
        {
            return moStudentAssessmentDC.GetListOfStudentFavDetails(aiAcademicYearId, aiStandardId, aiStudentId, aiTestId);
        }

        /// <summary>
        /// This method is used to return saved favourite related details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentFavouriteDetails GetAll(int aiStudentId, int aiTestId, int aiAcdemicYearId)
        {
            return moStudentAssessmentDC.GetAll(aiStudentId, aiTestId, aiAcdemicYearId);
        }

        /// <summary>
        /// This method is used to fill listview dropdown.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGrades(int aiAcdemicYearId)
        {
            return moStudentAssessmentDC.GetGrades(aiAcdemicYearId);
        }

        /// <summary>
        /// This method is used to save student assessment details.
        /// </summary>
        /// <param name="asxml"></param>
        /// <param name="aiTestId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="oStudentFavouriteDetails"></param>
        public void Save(String asxml, string asFavListXml, string asCategorywiseCommentXml, int aiAcdemicYearId, int aiTestId, int aiStudentId, StudentFavouriteDetails oStudentFavouriteDetails)
        {
            moStudentAssessmentDC.Save(asxml, asFavListXml, asCategorywiseCommentXml, aiAcdemicYearId, aiTestId, aiStudentId, oStudentFavouriteDetails);
        }

        /// <summary>
        /// This method is used to submit student assessment details.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="abIsSubmitted"></param>
        /// <param name="aiStudentId"></param>
        public void SubmitStudentAssessmentDetails(int aiAcademicYearId, int aiCategoryId, int aiTestId, bool abIsSubmitted, int aiStudentId)
        {
            moStudentAssessmentDC.SubmitStudentAssessmentDetails(aiAcademicYearId, aiCategoryId, aiTestId, abIsSubmitted, aiStudentId);
        }

        public bool AllowSelfAssessmentscreen()
        {
            return moStudentAssessmentDC.AllowSelfAssessmentscreen();
        }

        #endregion
    }
}
