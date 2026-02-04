// Class Name       :- StudentPaidFeeDetailsReportBL
// Purpose          :- This class is used to get students paid fee details for export report..
// Date Of creation :- 02/11/2019
// Author Name      :- Dnyaneshwar Shinde

using System.Collections.Generic;
using DataCommunicator.StudentPaidFeeDetailsReport;
using SchoolEntities.StudentPaidFeeDetails;

namespace BusinessLogic.StudentPaidFeeDetailsReport
{
    public class StudentPaidFeeDetailsReportBL
    {
        #region Data Member(s)

        private StudentPaidFeeDetailsReportDC moStudentPaidFeeDetailsReportDC;

        #endregion

        #region Constructor(s)

        public StudentPaidFeeDetailsReportBL()
        {
            moStudentPaidFeeDetailsReportDC = new StudentPaidFeeDetailsReportDC();
        }

        public StudentPaidFeeDetailsReportBL(int aiSchoolId, int aiAcademicYearId)
        {
            moStudentPaidFeeDetailsReportDC = new StudentPaidFeeDetailsReportDC(aiSchoolId, aiAcademicYearId);
        } 

        #endregion

        #region Property(s)

        public List<PayableForDetails> PayableForDetails
        {
            get { return this.moStudentPaidFeeDetailsReportDC.PayableForDetails; }
        }

        public List<PaidFeeDetails> PaidFeeDetails
        {
            get { return this.moStudentPaidFeeDetailsReportDC.PaidFeeDetails; }
        }

        public List<StudentFeeDetails> StudentFeeDetails
        {
            get { return this.moStudentPaidFeeDetailsReportDC.StudentFeeDetails; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get students paid fee details for report.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <returns></returns>
        public List<StudentDetails> GetStudentPaidFeeDetailsForReport(int aiStandardId, int aiDivisionId, int aiFeeTypeId)
        {
            return moStudentPaidFeeDetailsReportDC.GetStudentPaidFeeDetailsForReport(aiStandardId, aiDivisionId, aiFeeTypeId);
        }
        #endregion
    }
}
