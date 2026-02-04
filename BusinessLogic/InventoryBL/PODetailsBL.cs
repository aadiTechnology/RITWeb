using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
   public class PODetailsBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private int miTotalRows;
        private PODetailsDC moPODetails = null; 

        #endregion

        #region Constructor(s)

        public PODetailsBL()
        {
            moPODetails = new PODetailsDC();
        }

        public PODetailsBL(int aiSchoolId, int aiUserId)
        {
            moPODetails = new PODetailsDC(aiSchoolId, aiUserId);
        }

        public PODetailsBL(int aiSchoolId, int aiFinancialYearId, int aiUserId, int aiAcademicYearId)
        {
            moPODetails = new PODetailsDC(aiSchoolId, aiFinancialYearId, aiUserId, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

       /// <summary>
       /// THis method is used to delete particular record.
       /// </summary>
       /// <param name="aiId"></param>
 
       public void Delete(int aiId)
        {
            moPODetails.Delete(aiId);
        }

       /// <summary>
       /// This method is used to non duplicate PONo.
       /// </summary>
       /// <param name="aiId"></param>
       /// <param name="asInvoiceNo"></param>
       /// <returns></returns>
       public bool IsExternalPONoDuplicate(int aiId, string asPONo, bool abIsPO)
        {
            return moPODetails.IsExternalPONoDuplicate(aiId, asPONo, abIsPO);
        }

       /// <summary>
        /// This method is used to return PO description.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
       public List<ExternalPODescription> GetPODescriptions(int aiId)
        {
            return moPODetails.GetPODescriptions(aiId);
        }

       /// <summary>
       /// This method is used to save PO Details.
       /// </summary>
       /// <param name="asXml"></param>
       /// <param name="aoGSTInvoiceDetails"></param>
       public void Save(String asXml, ExternalPODetails aoExternalPODetails)
        {
            if (aoExternalPODetails.StartDate == null)
               aoExternalPODetails.StartDate = string.Empty.ToDateTime();

            if (aoExternalPODetails.EndDate == null)
               aoExternalPODetails.EndDate = string.Empty.ToDateTime();

            if (aoExternalPODetails.AdditionalRemarks == null)
                aoExternalPODetails.AdditionalRemarks = string.Empty;

           moPODetails.Save(asXml, aoExternalPODetails);
        }

       /// <summary>
       /// This method is used to get PO details.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="asFilter"></param>
       /// <param name="SortExpression"></param>
       /// <param name="SortDirection"></param>
       /// <param name="MaximumRows"></param>
       /// <param name="StartRowIndex"></param>
       /// <returns></returns>
       public List<ExternalPODetails> GetAll(int aiSchoolId, string asFilter, int aiIsPO, int aiAcademicYearId, int aiFinancialYearId, int aiStatusId, int aiLoginUserId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (SortExpression == string.Empty)
                SortExpression = "PONo desc";

            if (asFilter == null)
                asFilter = string.Empty;

            bool bIsPO = false;
            if (aiIsPO == 1)
                bIsPO = true;

            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<ExternalPODetails> lstExternalPODetails = moPODetails.GetAll(aiSchoolId, asFilter, bIsPO, aiFinancialYearId, aiStatusId, aiLoginUserId, SortExpression, StartRowIndex, MaximumRows, aiAcademicYearId);

            if (lstExternalPODetails.Count > 0)
                miTotalRows = lstExternalPODetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstExternalPODetails;
        }

       /// <summary>
       /// This method is used to count rows.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="asFilter"></param>
       /// <param name="SortExpression"></param>
       /// <param name="SortDirection"></param>
       /// <param name="MaximumRows"></param>
       /// <param name="StartRowIndex"></param>
       /// <returns></returns>
        public int GetCount(int aiSchoolId, string asFilter, int aiIsPO, int aiAcademicYearId, int aiFinancialYearId, int aiStatusId, int aiLoginUserId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }

       /// <summary>
       /// This method is used to return all PO details.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
        public ExternalPODetails Get(int aiId)
        {
            return moPODetails.Get(aiId);
        }

       /// <summary>
       /// This method is used to fill receiver name dropdown.
       /// </summary>
       /// <returns></returns>
        public List<ReceiverName> GetReceiverName()
        {
            return moPODetails.GetReceiverName();
        }

       /// <summary>
       /// This method is used to fill GSTCategory dropdown.
       /// </summary>
       /// <returns></returns>
        public List<GSTCategory> GetGSTCategory()
        {
            return moPODetails.GetGSTCategory();
        }

       /// <summary>
       /// This method is used to fill Instructions chechboxlist.
       /// </summary>
       /// <returns></returns>
       public POInstructionDetails GetInstructions()
        {
            return moPODetails.GetInstructions();
        }

       /// <summary>
       /// This method is used to get prefixes.
       /// </summary>
       /// <returns></returns>
       public ExternalOrderPrefix GetPrefixes()
       {
           return moPODetails.GetPrefixes();
       }

       /// <summary>
       /// This method is used to send request for approval.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
       public int SendRequestForApproval(int aiId)
       {
          return moPODetails.SendRequestForApproval(aiId);
       }

       /// <summary>
       /// This method is used to approve request.
       /// </summary>
       /// <param name="aiId"></param>
       /// <param name="asComment"></param>
       /// <param name="abIsApproved"></param>
       /// <returns></returns>
       public int ApproveRequest(int aiId, string asComment, bool abIsApproved)
       {
           return moPODetails.ApproveRequest(aiId, asComment, abIsApproved);
       }

       /// <summary>
       /// This method is used to get all payment entries.
       /// </summary>
       /// <param name="aiPoMasterId"></param>
       /// <returns></returns>
       public List<POFeePayment> GetAllPayments(int aiPoMasterId)
       {
           return moPODetails.GetAllPayments(aiPoMasterId, 0);
       }

       /// <summary>
       /// This method is used to save payment details.
       /// </summary>
       /// <param name="aoPOFeePayment"></param>
       public void SavePayment(POFeePayment aoPOFeePayment)
       {
           moPODetails.SavePayment(aoPOFeePayment);
       }


       /// <summary>
       /// This method is used to return payment details.
       /// </summary>
       /// <param name="aiPoMasterId"></param>
       /// <param name="aiId"></param>
       /// <returns></returns>
       public POFeePayment GetPaymentDetails(int aiPoMasterId, int aiId)
       {
           List<POFeePayment> lstPOFeePayment = moPODetails.GetAllPayments(aiPoMasterId, aiId);
           return lstPOFeePayment[0];
       }

       /// <summary>
       /// This method is used to delete payment details.
       /// </summary>
       /// <param name="aiPoMasterId"></param>
       /// <param name="aiId"></param>
       public void DeletePaymentDetails(int aiPoMasterId, int aiId)
       {
           moPODetails.DeletePaymentDetails(aiPoMasterId, aiId);
       }

        #endregion
    }
}
