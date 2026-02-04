/*File Name - PaymentParameterBL.cs
 * Created By - Pravin Shinde
 * Created Date - 29 Oct 2013
 * Description - This class is used to manage payroll parameters.
 */
namespace BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataCommunicator;
    using PayrollEntities;

    /// <summary>
    /// This class contains the methods that will be used to manage payroll parameters.Internally it calls the methods from dc file.
    /// </summary>
    public class PaymentParameterBL
    {
        #region Data Member(s)

        private PaymentParameterDC moPaymentParameterDC;

        #endregion

        #region Constructor(s)

        public PaymentParameterBL(int aiSchoolId, int aiInsertedById)
        {
            this.moPaymentParameterDC = new PaymentParameterDC(aiSchoolId, aiInsertedById);
        } 

        #endregion

         #region Public Method(s)

        /// <summary>
        /// This method is used to get all/selected the payment parameters.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<PaymentParameter> GetAll(int aiParameterId)
        {
            return moPaymentParameterDC.GetAll(aiParameterId);
        }        
        
        /// <summary>
        /// This method is used to save/update the existing payment parameter.
        /// </summary>
        /// <param name="aiParameterId"></param>
        /// <param name="asParameter"></param>
        public void Save(int aiParameterId,string asParameter)
        {
            moPaymentParameterDC.Save(aiParameterId, asParameter);
        }

        /// <summary>
        /// This method is used to delete the parameter from the given list view.
        /// </summary>
        /// <param name="aiParameterId"></param>
        public void Delete(int aiParameterId)
        {
            moPaymentParameterDC.Delete(aiParameterId);
        }

        #endregion
    }
}
