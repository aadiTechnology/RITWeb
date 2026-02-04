using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using PayrollEntities;

namespace BusinessLogic
{
    public class PaymentGroupBL
    {
        #region Data Member(s)
        
        PaymentGroupDC moPaymentGroupDC; 

        #endregion

        #region Constructor

        public PaymentGroupBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moPaymentGroupDC = new PaymentGroupDC(aiSchoolId, aiUpdatedById);
        }
 
        #endregion

        #region Public Method(s)
        
        /// <summary>
        ///  method is used to return all payment groups.
        /// </summary>
        /// <returns></returns>
        public List<PaymentGroup> GetAll()
        {
            return this.moPaymentGroupDC.GetAll();
        }

        /// <summary>
        /// This method is used to return payment group according to given payment group id.
        /// </summary>
        /// <param name="aiPaymentGroupId"></param>
        /// <returns></returns>
        public PaymentGroup Get(int aiPaymentGroupId)
        {
            return this.moPaymentGroupDC.Get(aiPaymentGroupId);
        }

        /// <summary>
        /// This method is used to save payment group details.
        /// </summary>
        /// <param name="aoSalaryParameter"></param>
        public void Save(int aiGroupId, string asName, string asParameterXml)
        {
            this.moPaymentGroupDC.Save(aiGroupId, asName, asParameterXml);
        }

        /// <summary>
        /// This method is used to delete payment group according to given payment group id.
        /// </summary>
        /// <param name="aiPaymentGroupId"></param>
        public void Delete(int aiPaymentGroupId)
        {
            this.moPaymentGroupDC.Delete(aiPaymentGroupId);
        } 

        #endregion
    }
}
