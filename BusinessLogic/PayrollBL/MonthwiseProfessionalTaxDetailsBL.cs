// Class Name       :- MonthwiseProfessionalTaxDetailsBL
// Purpose          :- This class is used to manage MonthwiseProfessionalTaxDetails details.
// Date Of creation :- 4/5/2010
// Author Name      :- 

using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class MonthwiseProfessionalTaxDetailsBL
    {
        #region Data Member(s)

        private MonthwiseProfessionalTaxDetailsDC moMonthwiseProfessionalTaxDetailsDC; 

        #endregion

        #region Constructor(s)

        public MonthwiseProfessionalTaxDetailsBL()
        {
            this.moMonthwiseProfessionalTaxDetailsDC = new MonthwiseProfessionalTaxDetailsDC();
        }

        public MonthwiseProfessionalTaxDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moMonthwiseProfessionalTaxDetailsDC = new MonthwiseProfessionalTaxDetailsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        } 

        #endregion

        #region Property(s)

        public MonthwiseProfessionalTaxDetails MonthwiseProfessionalTaxDetails
        {
            get { return this.moMonthwiseProfessionalTaxDetailsDC.MonthwiseProfessionalTaxDetails; }
            set { this.moMonthwiseProfessionalTaxDetailsDC.MonthwiseProfessionalTaxDetails = value; }
        }

        #endregion

        #region Method(s)

        public static DataTable GetAllPTChallanDetails(int aiSchoolId, int aiFinancialYearId, string sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return MonthwiseProfessionalTaxDetailsDC.GetAllPTChallanDetails(aiSchoolId, aiFinancialYearId, sortExpression, iEndIndex, startRowIndex);
        }

        public static int CountPTChallanDetails(int aiSchoolId, int aiFinancialYearId, string sortExpression, int maximumRows, int startRowIndex)
        {
            return MonthwiseProfessionalTaxDetailsDC.CountPTChallanDetails(aiSchoolId,aiFinancialYearId, sortExpression, maximumRows, startRowIndex);
        }

        public bool Insert()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.Insert();
        }

        public bool Update()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.Update();
        }

        public void Delete()
        {
            this.moMonthwiseProfessionalTaxDetailsDC.Delete();
        }

        public MonthwiseProfessionalTaxDetails Get(int aiMonthwiseProfessionalTaxDetailsId)
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.Get(aiMonthwiseProfessionalTaxDetailsId);
        }

        public bool IsDuplicate()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.IsDuplicate();
        }

        /// <summary>
        /// This method is used to check CIN No. is duplicate or not.
        /// </summary>
        /// <returns></returns>
        public bool IsCINNoDuplicate()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.IsCINNoDuplicate();
        }
        public DataSet GetBankNameMonthYear()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.GetBankNameMonthYear();
        }

        public bool CheckPrecondition()
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.CheckPrecondition();
        }

        public bool IsSalaryPaid(int aiMonthId, int aiYear)
        {
            return this.moMonthwiseProfessionalTaxDetailsDC.IsSalaryPaid(aiMonthId, aiYear);
        } 

        #endregion
    }
}
