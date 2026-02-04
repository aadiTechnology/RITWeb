using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class DepositeBankDetailsBL
    {
        #region Data MEmber(s)
        
        private DepositeBankDetailsDC moDepositeBankDetailsDC;
        private int miTotalRows; 

        #endregion

        #region Constructor(s)
        
        public DepositeBankDetailsBL()
        {
            moDepositeBankDetailsDC = new DepositeBankDetailsDC();
        }

        public DepositeBankDetailsBL(int aiSchoolId, int aiUserId)
        {
            moDepositeBankDetailsDC = new DepositeBankDetailsDC(aiSchoolId, aiUserId);
        } 

        #endregion
        
        #region Public Method(s)

        /// <summary>
        /// This method is used to savebank details.
        /// </summary>
        /// <param name="aoDepositeBankDetails"></param>
        public void Save(SchoolEntities.DepositeBankDetails aoDepositeBankDetails)
        {
            moDepositeBankDetailsDC.Save(aoDepositeBankDetails);
        }

        /// <summary>
        /// This method is used to delete bank details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            moDepositeBankDetailsDC.Delete(aiId);
        }

        /// <summary>
        /// This method is used to get bank details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public DepositeBankDetails Get(int aiId)
        {
            return moDepositeBankDetailsDC.Get(aiId);
        }

        /// <summary>
        /// This method is used to get all bank details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDate"></param>
        /// <param name="asChequeNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<DepositeBankDetails> GetAll(int aiSchoolId, string asDate, string asChequeNo, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            if (sortExpression == null || sortExpression == string.Empty)
                sortExpression = "Order By Month Desc";

            int iEndRowIndex = startRowIndex + maximumRows;
            List<DepositeBankDetails> lstDepositeBankDetails = moDepositeBankDetailsDC.GetAll(aiSchoolId, asDate, asChequeNo, sortExpression, startRowIndex, iEndRowIndex);

            if (lstDepositeBankDetails.Count > 0)
                miTotalRows = lstDepositeBankDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstDepositeBankDetails;
        }


        /// <summary>
        /// This method is used to return record count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDate"></param>
        /// <param name="asChequeNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, string asDate, string asChequeNo, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }
        
        /// <summary>
        /// This method is used to validate cheque no.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="asChequeNo"></param>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public bool ValidateChequeNo(int aiId, string asChequeNo, int aiCategoryId)
        {
            return moDepositeBankDetailsDC.ValidateChequeNo(aiId, asChequeNo, aiCategoryId);
        }

        /// <summary>
        /// This method is used to validate month.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public bool ValidateMonth(int aiId, int aiYear, int aiMonthId)
        {
            return moDepositeBankDetailsDC.ValidateMonth(aiId, aiYear, aiMonthId);
        } 

        #endregion
    }
}
