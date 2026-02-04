using DataCommunicator;
using SchoolEntities.StudentFee.FeeReport;

namespace BusinessLogic
{
    public class FeeReportBL
    {
        #region Data Member(s)
        
        private FeeReportDC moFeeReportDC; 

        #endregion

        #region Constructor(s)

        public FeeReportBL()
        {
            moFeeReportDC = new FeeReportDC();
        }

        public FeeReportBL(int aiSchoolId, int aiAcademicYearId)
        {
            moFeeReportDC = new FeeReportDC(aiSchoolId, aiAcademicYearId);
        } 

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to return fee details to export it.
        /// </summary>
        /// <param name="aiStdId"></param>
        /// <param name="aiDivId"></param>
        /// <returns></returns>
        public FeeReport GetFeeDetailsForReport(int aiStdId, int aiDivId)
        {
            return moFeeReportDC.GetFeeDetailsForReport(aiStdId, aiDivId);
        } 

        #endregion
    }
}
