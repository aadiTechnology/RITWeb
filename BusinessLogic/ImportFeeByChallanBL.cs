// Class Name       :- ImportFeeByChallanBL
// Purpose          :- This class is used to import challan details.
// Date Of creation :- 5 Jul 2016
// Author Name      :- Yogesh

using DataCommunicator;
namespace BusinessLogic
{
    public class ImportFeeByChallanBL
    {
        #region Data Member(s)
        
        private ImportFeeByChallanDC moImportFeeByChallanDC;
        
        #endregion

        #region Constructor(s)

        public ImportFeeByChallanBL()
        {
            moImportFeeByChallanDC = new ImportFeeByChallanDC();
        }

        public ImportFeeByChallanBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moImportFeeByChallanDC = new ImportFeeByChallanDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        public ImportFeeByChallanBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById, int aiFinancialYearId)
        {
            moImportFeeByChallanDC = new ImportFeeByChallanDC(aiSchoolId, aiAcademicYearId, aiUpdatedById, aiFinancialYearId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to Insert Fee by challan.
        /// </summary>
        /// <param name="asChallanDetails"></param>
        public void InsertFeeByChallan(string asChallanDetails, int aiOriginalFeeTypeId)
        {
            moImportFeeByChallanDC.InsertFeeByChallan(asChallanDetails, aiOriginalFeeTypeId);
        }

        /// <summary>
        /// This method is used to check given challan number is invalid or not.
        /// </summary>
        /// <param name="aiChallanNo"></param>
        /// <returns></returns>
        public bool InvalidChallanNo(int aiChallanNo, int aiOriginalfeeTypeId)
        {
            return moImportFeeByChallanDC.InvalidChallanNo(aiChallanNo, aiOriginalfeeTypeId);
        }

        #endregion
    }
}
