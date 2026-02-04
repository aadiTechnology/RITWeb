using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
   public class GSTInvoiceDetailsBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private int miTotalRows;
        private GSTInvoiceDetailsDC moGSTInvoiceDetails = null; 

        #endregion

        #region Constructor(s)

        public GSTInvoiceDetailsBL()
        {
            moGSTInvoiceDetails = new GSTInvoiceDetailsDC();
        }

        public GSTInvoiceDetailsBL(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            moGSTInvoiceDetails = new GSTInvoiceDetailsDC(aiSchoolId, aiUserId, aiAcademicYearId);
        } 

        #endregion

        #region Public Method(s)

       /// <summary>
       /// THis method is used to delete particular record.
       /// </summary>
       /// <param name="aiId"></param>
 
       public void Delete(int aiId)
        {
            moGSTInvoiceDetails.Delete(aiId);
        }

       /// <summary>
       /// This method is used to non duplicate InvoiceNo.
       /// </summary>
       /// <param name="aiId"></param>
       /// <param name="asInvoiceNo"></param>
       /// <returns></returns>
        public bool IsInvoiceNoDuplicate(int aiId, string asInvoiceNo)
        {
            return moGSTInvoiceDetails.IsInvoiceNoDuplicate(aiId, asInvoiceNo);
        }

       /// <summary>
        /// This method is used to return GST description.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
        public List<GSTInvoiceDescription> GetGSTDescriptions(int aiId)
        {
            return moGSTInvoiceDetails.GetGSTDescriptions(aiId);
        }

       /// <summary>
       /// This method is used to save GST Invoice Details.
       /// </summary>
       /// <param name="asXml"></param>
       /// <param name="aoGSTInvoiceDetails"></param>
        public void Save(String asXml, GSTInvoiceDetails aoGSTInvoiceDetails)
        {
            moGSTInvoiceDetails.Save(asXml, aoGSTInvoiceDetails);
        }

       /// <summary>
       /// This method is used to get GST Invoice details.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="asFilter"></param>
       /// <param name="SortExpression"></param>
       /// <param name="SortDirection"></param>
       /// <param name="MaximumRows"></param>
       /// <param name="StartRowIndex"></param>
       /// <returns></returns>
       public List<GSTInvoiceDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (SortExpression == string.Empty)
                SortExpression = "InvoiceNo desc";

            if (asFilter == null)
                asFilter = string.Empty;

            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<GSTInvoiceDetails> lstGSTInvoiceDetails = moGSTInvoiceDetails.GetAll(aiSchoolId, aiAcademicYearId, asFilter, SortExpression, StartRowIndex, MaximumRows);

            if (lstGSTInvoiceDetails.Count > 0)
                miTotalRows = lstGSTInvoiceDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstGSTInvoiceDetails;
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
        public int GetCount(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }

       /// <summary>
       /// This method is used to return all GST Invoice details.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
        public GSTInvoiceDetails Get(int aiId)
        {
            return moGSTInvoiceDetails.Get(aiId);
        }

       /// <summary>
       /// This method is used to fill receiver name dropdown.
       /// </summary>
       /// <returns></returns>
        public List<ReceiverName> GetReceiverName()
        {
            return moGSTInvoiceDetails.GetReceiverName();
        }

       /// <summary>
       /// This method is used to fill GSTCategory dropdown.
       /// </summary>
       /// <returns></returns>
        public List<GSTCategory> GetGSTCategory()
        {
            return moGSTInvoiceDetails.GetGSTCategory();
        } 

        #endregion
   }
}
