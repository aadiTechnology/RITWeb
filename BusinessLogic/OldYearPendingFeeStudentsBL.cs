using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class OldYearPendingFeeStudentsBL
    {
        #region Data members

        private OldYearPendingFeeStudentsDC moEmployeeDetailsDC;

        #endregion
        #region Constructors


        public OldYearPendingFeeStudentsBL()
        {
            this.moEmployeeDetailsDC = new OldYearPendingFeeStudentsDC();
        }


        public OldYearPendingFeeStudentsBL(int aiSchoolId, int aiAcademicYearId)
        {
            moEmployeeDetailsDC = new OldYearPendingFeeStudentsDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        /// <summary>
        /// This function is used to Get the pending fee details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public OldYearPendingFeeReport GetOldYearPendingFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiStandardId, int aiDivisionId, int aiFromYear, int aiToYear, int aiIncludeLateFee)
        {
            OldYearPendingFeeStudentsDC moEmployeeDetailsDC = new OldYearPendingFeeStudentsDC();
            return moEmployeeDetailsDC.GetOldYearPendingFeeDetails(aiSchoolId, aiAcademicYearId, aiStudentId, aiStandardId, aiDivisionId, aiFromYear, aiToYear, aiIncludeLateFee);
        }
    }
}
