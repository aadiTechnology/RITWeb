/*
 * File Name - EarningDeductionPercentagePopup.aspx.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage payment categories.
 */
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class PaymentCategoryBL
    {
        #region Data Member(s)

        private PaymentCategoryDC moPaymentCategoryDC;

        #endregion

        #region Constructor(s)

        public PaymentCategoryBL()
        {
            this.moPaymentCategoryDC = new PaymentCategoryDC();
        }

        public PaymentCategoryBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moPaymentCategoryDC = new PaymentCategoryDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public List<EarningDeductionPercentage> EarningDeductionPercentages
        {
            get { return this.moPaymentCategoryDC.EarningDeductionPercentages; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save category.
        /// </summary>
        /// <param name="aiCategoryID"></param>
        /// <param name="asName"></param>
        /// <param name="asEarnDeductXml"></param>
        public void Save(int aiCategoryID, string asName, string asEarnDeductXml, string asUpdateExistingData)
        {
            this.moPaymentCategoryDC.Save(aiCategoryID, asName, asEarnDeductXml, asUpdateExistingData);
        }

        /// <summary>
        /// This method is used to return details of selected category.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public PaymentCategory Get(int aiCategoryId)
        {
           return this.moPaymentCategoryDC.Get(aiCategoryId);
        }

        /// <summary>
        /// This method is used to delete category.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        public void Delete(int aiCategoryId)
        {
            this.moPaymentCategoryDC.Delete(aiCategoryId);
        }

        /// <summary>
        /// This method is used to return all categories.
        /// </summary>
        /// <returns></returns>
        public List<PaymentCategory> GetAll()
        {
            return this.moPaymentCategoryDC.GetAll();
        }

        #endregion
    }
}
