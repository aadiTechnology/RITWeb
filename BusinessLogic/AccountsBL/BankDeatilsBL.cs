using System.Collections.Generic;
using AccountsEntities;
using DataCommunicator;

namespace BusinessLogic
{
    public class BankDeatilsBL
    {
        #region "Data Member"
        
        BankDetailsDC moBankDetailsDC = null;
        
        public BankAccountDetails BankAccountDetails
        {
            get { return moBankDetailsDC.BankAccountDetails; }
            set { moBankDetailsDC.BankAccountDetails = value; }
        }
        #endregion
        #region "Constructor"

        public BankDeatilsBL(int aiSchoolId, int aiFinancialYearId)
        {
            moBankDetailsDC = new BankDetailsDC(aiSchoolId, aiFinancialYearId);
        }
        #endregion

        #region "Public method"
        /// <summary>
        /// This method is used to get bank names.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        //public static List<BankAccountDetails> GetBankNames(int aiSchoolId)
        //{
        //   return BankDetailsDC.GetBankNames(aiSchoolId);
        //}
        ///// <summary>
        ///// This method is used to save bank details.
        ///// </summary>
        ///// <param name="adOpeninigBal"></param>
        ///// <param name="aiIsDebit"></param>
        //public void save(BankAccountDetails oBankAccountDetails)
        //{
        //    moBankDetailsDC.save(oBankAccountDetails);
        //}
        ///// <summary>
        ///// This method is used to get all bank details to display.
        ///// </summary>
        ///// <param name="asSortExp"></param>
        ///// <param name="asSortDirection"></param>
        ///// <returns></returns>
        //public static List<BankAccountDetails> GetAll(string asSortExp, string asSortDirection, int aiSchoolId, int aiFinancialYearId)
        //{
        //    return BankDetailsDC.GetAll(asSortExp,asSortDirection,aiSchoolId,aiFinancialYearId);
        //}
        ///// <summary>
        ///// This method is used to get bank details for edit.
        ///// </summary>
        ///// <param name="aiLedgerId"></param>
        ///// <returns></returns>
        //public static List<BankAccountDetails> GetBankDetails(int aiLedgerId,int aiSchoolId,int aiFinancialYearId)
        //{
        //  return  BankDetailsDC.GetBankDetails(aiLedgerId,aiSchoolId,aiFinancialYearId);
        //}
        ///// <summary>
        ///// This method is used to delete Bank details.
        ///// </summary>
        ///// <param name="aiLedgerId"></param>
        //public void Delete(int aiLedgerId,int aiUserId)
        //{
        //       // throw new DependencyException("Could not delete group since it has subgroup(s) or group is associted with ledger(s).");
        //    moBankDetailsDC.Delete(aiLedgerId,aiUserId);
        //}
        ///// <summary>
        ///// This method is used to get count of the groups.
        ///// </summary>
        ///// <returns></returns>
        //public bool GetCount()
        //{
        //    return moBankDetailsDC.GetCount();
        //}
        #endregion
    }
}
