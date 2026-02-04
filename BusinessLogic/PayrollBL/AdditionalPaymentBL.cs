/* File Name - AdditionalPaymentBL.cs
 * Created By - Sachin
 * Created Date - 29 Oct 2013
 * Description - This class is used to manage additional payment details. 
 */
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class AdditionalPaymentBL
    {
        #region Data Member(s)
        
        AdditionalPaymentDC moAdditionalPaymentDC; 

        #endregion

        #region Constructor(s)
        
        public AdditionalPaymentBL()
        {
            moAdditionalPaymentDC = new AdditionalPaymentDC();
        }

        public AdditionalPaymentBL(int aiSchoolId, int aiFinancialYearId, int aiUpdatedById)
        {
            moAdditionalPaymentDC = new AdditionalPaymentDC(aiSchoolId, aiFinancialYearId, aiUpdatedById);
        }

        #endregion

        #region Method(s)
        
        /// <summary>
        /// This method is used to return all additional payment details according to given filter.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<AdditionalPaymentDetails> GetAll(int aiSchoolId, int aiFinancialYearId, string asFilter, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            if (sortExpression == string.Empty)
                sortExpression = "PaymentDate Desc, OriginalStaffGroupsId asc, DesignationId asc, FirstName  asc, MiddleName asc, LastName asc";

            if (sortExpression.Contains("UserName"))
            {
                if (sortExpression.Contains(" DESC"))
                    sortDirection = "Desc";
                else
                    sortDirection = "Asc";

                sortExpression = "OriginalStaffGroupsId " + sortDirection + ", DesignationId " + sortDirection + ", FirstName  " + sortDirection + ", MiddleName " + sortDirection + ", LastName " + sortDirection;
            }
            
            if (asFilter == null)
                asFilter = string.Empty;

            maximumRows = startRowIndex + Constants.I_GRID_PAGE_COUNT;
            return moAdditionalPaymentDC.GetAll(aiSchoolId, aiFinancialYearId, asFilter, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// This method is used to count additional payments.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiFinancialYearId, string asFilter, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            if (asFilter == null)
                asFilter = string.Empty;

            return moAdditionalPaymentDC.Count(aiSchoolId, aiFinancialYearId, asFilter);
        }

        /// <summary>
        /// This method is used to return all additional payments.
        /// </summary>
        /// <returns></returns>
        public List<AdditionalPaymentDetails> GetAll()
        {  
            return moAdditionalPaymentDC.GetAll();
        }

        /// <summary>
        /// This method is used to return additional payment object.
        /// </summary>
        /// <param name="aiPaymentId"></param>
        /// <returns></returns>
        public AdditionalPaymentDetails Get(int aiPaymentId)
        {
            return moAdditionalPaymentDC.Get(aiPaymentId);
        }

        /// <summary>
        /// This method is used to delete additional payment according to given id.
        /// </summary>
        /// <param name="aiPaymentId"></param>
        public void Delete(int aiPaymentId)
        {
            moAdditionalPaymentDC.Delete(aiPaymentId);
        }

        /// <summary>
        /// This method is used to save additional payment details.
        /// </summary>
        /// <param name="asPaymentXml"></param>
        public void Save(string asPaymentXml)
        {
            moAdditionalPaymentDC.Save(asPaymentXml);
        } 

        #endregion
    }
}
