using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class SchoolwiseBankAccountDetailsBL
    {
        #region "Data Members"
        SchoolwiseBankAccountDetailsDC moSchoolwiseBankAccountDetailsDC;
        #endregion "Data Members"

        #region "Property"
        public SchoolWiseBankAccountDetails SchoolWiseBankAccountDetails
        {
            get { return moSchoolwiseBankAccountDetailsDC.moSchoolWiseBankAccountDetails; }
            set { moSchoolwiseBankAccountDetailsDC.moSchoolWiseBankAccountDetails = value; }
        }
        #endregion "Property"

        #region "Constructors"
        public SchoolwiseBankAccountDetailsBL()
        {
            moSchoolwiseBankAccountDetailsDC = new SchoolwiseBankAccountDetailsDC();
        }

        public SchoolwiseBankAccountDetailsBL(int aiSchoolWiseBankAccountDetailsId)
        {
            moSchoolwiseBankAccountDetailsDC = new SchoolwiseBankAccountDetailsDC(aiSchoolWiseBankAccountDetailsId);
        }
        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to insert the schoolwise bank account details.
        /// </summary>
        public void InsertSchoolwiseBankAccountDetailsBL()
        {
            moSchoolwiseBankAccountDetailsDC.InsertSchoolwiseBankAccountDetails();
        }

        /// <summary>
        /// This method is used to update the schoolwise bank account details.
        /// </summary>
        public void UpdateSchoolwiseBankAccountDetailsBL()
        {
            moSchoolwiseBankAccountDetailsDC.UpdateSchoolwiseBankAccountDetails();
        }

        /// <summary>
        /// This method is used to delete the schoolwise bank account details.
        /// </summary>
        public void DeleteSchoolwiseBankAccountDetailsBL()
        {
            moSchoolwiseBankAccountDetailsDC.DeleteSchoolwiseBankAccountDetails();
        }

        /// <summary>
        /// This method is used to load the schoolwise bank account details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<SchoolWiseBankAccountDetails> GetSchoolwiseBankAccountDetailsBL(int aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moSchoolwiseBankAccountDetailsDC.GetSchoolwiseBankAccountDetails(aiSchoolId, sortExpression, iEndIndex, iStartIndex);
        }

        /// <summary>
        /// This method is used to get total no. of bank account details records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int CountTotalSchoolwiseBankAccountBL(int aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {
            return moSchoolwiseBankAccountDetailsDC.CountTotalSchoolwiseBankAccount(aiSchoolId, sortExpression, maximumRows, startRowIndex);
        }

        /// <summary>
        /// This method is used to check whether bank account details to be inserted are duplicate or not.
        /// </summary>
        /// <returns></returns>
        public int IsBankAccountDuplicateBL()
        {
            return moSchoolwiseBankAccountDetailsDC.IsBankAccountDuplicateDC();
        }

        /// <summary>
        /// This method is used to get schoolwise bank list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<SchoolWiseBankAccountDetails> GetSchoolwiseBankList(int aiSchoolId)
        {
            return moSchoolwiseBankAccountDetailsDC.GetSchoolwiseBankList(aiSchoolId);
        }

        /// <summary>
        /// This method is used to get schoolwise account list for a particular bank.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiBankId"></param>
        /// <returns></returns>
        public List<SchoolWiseBankAccountDetails> GetBankwiseAccountList(int aiSchoolId, int aiBankId)
        {
            return moSchoolwiseBankAccountDetailsDC.GetBankwiseAccountList(aiSchoolId, aiBankId);
        }

        public int GetCountForAssociatedBankAccount()
        {
            return moSchoolwiseBankAccountDetailsDC.GetCountForAssociatedBankAccount();
        }
        #endregion "Public Methods"
    }
}
