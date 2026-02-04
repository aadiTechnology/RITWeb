using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Configuration;
using SchoolEntities.Accounts;
using Utility;
using System.Web;

public partial class OnlineBankDetails : SchoolBase
{
    #region "Constant(s)"

    private const int I_BANK_DETAILS_TABLE = 0;
    private const int I_CARD_DETAILS_TABLE = 1;
    private const string S_SCHOOWISE_STUDENT_FEE_ID = "Schoolwise_Student_Fee_Id";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			// Below condition is used to check payment process from mobile app.
            if (Convert.ToBoolean(HttpContext.Current.Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE]))
            {
                btnBack.Visible = true;
                btnClose.Visible = false;
            }
            else {
                btnBack.Visible = false;
                btnClose.Visible = true;
            }

            btnClose.Attributes.Add("onclick", "refreshParent()");
	        ApplyMouseHoverEffect(new List<Button> { btnClose });      
            
            int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
            if (iSchoolId == Constants.SchoolId.SNS.ToInt())
                trServiceTaxNote.Visible = false;

            var oStudentFeeDetailsBL = new StudentFeeDetailsBL();

            List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds());
            PaymentGateWayDetails oPaymentGateWayDetails = new PaymentGateWayDetails();

            if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.TPSL.ToInt()))
                lblNote2.Visible = false;
            else
                lblNote1.Visible = false;

            DataSet oDataSet = oStudentFeeDetailsBL.GetBankDetailsForNetBanking(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));

            lstvwBankDetails.DataSource = oDataSet.Tables[I_BANK_DETAILS_TABLE];
            lstvwBankDetails.DataBind();

            if (oDataSet.Tables[I_CARD_DETAILS_TABLE].Rows.Count > 0)
            {
                lstvwCardDetails.DataSource = oDataSet.Tables[I_CARD_DETAILS_TABLE];
                lstvwCardDetails.DataBind();
            }
            else
            {
                trCardGateway.Visible = false;
                trCardDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// This event is used to redirect back to fee online pay from bank details page in mobile app.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(HttpContext.Current.Session[Constants.S_SESSION_MOBILE_PAY_FEE_POSTBACKURL].ToString(), false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = Session[S_SCHOOWISE_STUDENT_FEE_ID] as List<int>;
        string sIds = string.Empty;
        if (oStudentFeeIds != null)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }
}
