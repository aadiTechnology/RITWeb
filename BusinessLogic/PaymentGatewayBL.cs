using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using DataCommunicator;

namespace BusinessLogic
{
    public class PaymentGatewayBL
    {
        PaymentGatewayDC moPaymentGatewayDC;
        public PaymentGatewayBL()
        {
            moPaymentGatewayDC = new PaymentGatewayDC();
        }

        public List<AtomGatewayDetails> GetAtomGatewayDetails(int aiAtomCategoryId)
        {
            return moPaymentGatewayDC.GetAtomGatewayDetails(aiAtomCategoryId);
        }

        public List<GatewayAdditionalDetails> GetGatewayDetails(Utility.Constants.PaymentGateways aoPaymentGateway)
        {
            return moPaymentGatewayDC.GetGatewayDetails(aoPaymentGateway);
        }

        public DataTable GetPaymentGateway(int aiSchoolId)
        {
            return moPaymentGatewayDC.GetPaymentGateway(aiSchoolId);
        }
    }
}
