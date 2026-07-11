using DataCommunicator;
using System.Data;
using SchoolEntities;

namespace BusinessLogic
{
    /// <summary>
    /// Business logic class for student mandatory details.
    /// </summary>
    public class StudentMandatoryDetailsBL
    {
        #region Data Member(s)

        private StudentMandatoryDetailsDC moStudentMandatoryDetailsDC;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentMandatoryDetailsBL"/> class.
        /// </summary>
        public StudentMandatoryDetailsBL()
        {
            moStudentMandatoryDetailsDC = new StudentMandatoryDetailsDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentMandatoryDetailsBL"/> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        public StudentMandatoryDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moStudentMandatoryDetailsDC = new StudentMandatoryDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get student mandatory details.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public StudentMandatoryDetails GetStudentMandatoryDetails(int aiYearwiseStudentId)
        {
            return moStudentMandatoryDetailsDC.GetStudentMandatoryDetails(aiYearwiseStudentId);
        }

        /// <summary>
        /// This method is used to save student mandatory details.
        /// </summary>
        /// <param name="aoStudentMandatoryDetails"></param>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public bool SaveStudentMandatoryDetails(StudentMandatoryDetails aoStudentMandatoryDetails, int aiYearwiseStudentId)
        {
            return moStudentMandatoryDetailsDC.SaveStudentMandatoryDetails(aoStudentMandatoryDetails, aiYearwiseStudentId);
        }

        /// <summary>
        /// This method is used to submit student mandatory details.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public bool SubmitStudentMandatoryDetails(int aiYearwiseStudentId)
        {
            return moStudentMandatoryDetailsDC.SubmitStudentMandatoryDetails(aiYearwiseStudentId);
        }

        /// <summary>
        /// This method is used to get transport mode details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTransportModeDetails()
        {
            return moStudentMandatoryDetailsDC.GetTransportModeDetails();
        }

        #endregion
    }
}

