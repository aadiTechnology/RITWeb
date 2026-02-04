using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using System.Configuration;

public partial class TermsOfUseForOnlinePayment : SchoolBase
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
                btnBack.Visible = Convert.ToBoolean(HttpContext.Current.Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE]);
                bool bIsAaryanSchool = Settings.IsAaryanSchool;

                int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();

                if (iSchoolId == Constants.SchoolId.DSK.ToInt() || iSchoolId == Constants.SchoolId.SS.ToInt())
				{	
					liSpecific1.Visible = true;
					liSpecific2.Visible = true;
                    liSpecific3.Visible = true;

                    if (iSchoolId == Constants.SchoolId.DSK.ToInt())
                    {
                        lblspacific.Text = "PayU and Aggregator";
                        lblSpecificTeachernameAndContact.Text = "Shilpa ma'am : 020 - 24380047";
                    }
                    else if (iSchoolId == Constants.SchoolId.SS.ToInt())
                    {
                        //lblspacific.Text = "ATOM Technologies";
                        lblspacific.Text = "EaseBuzz and Aggregator";
                        lblSpecificTeachernameAndContact.Text = "Anushka ma'am : 020 - 65320180";
                    }
				}
                else if (iSchoolId == Constants.SchoolId.PKIS.ToInt() || iSchoolId == Constants.SchoolId.BFS.ToInt() || bIsAaryanSchool)
                {
                    lblspacific.Text = "PayUMoney and Aggregator";
                }
                else if (iSchoolId == Constants.SchoolId.SNS.ToInt() || iSchoolId == Constants.SchoolId.DYPV.ToInt())
                {
                    lblspacific.Text = "BillDesk and Aggregator";
                }
                else if (iSchoolId == Constants.SchoolId.DPIS.ToInt())
                {
                    lblspacific.Text = "CCAvenue and Aggregator";
                }
                else if (iSchoolId == Constants.SchoolId.VPMCPS.ToInt())
                {
                    lblspacific.Text = "CCAvenue and Aggregator";
                }
                else if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
                {
                    if (QueryString["GatewayName"] != null && QueryString["GatewayName"].ToString() == "RazorPay")
                        lblspacific.Text = "RazorPay and Aggregator";
                    else
                        lblspacific.Text = "Axis Bank and Aggregator";
                }
                //else if (iSchoolId == Constants.SchoolId.OWS.ToInt())
                //{
                //    lblspacific.Text = "Phi Commerce and Aggregator";
                //}
                else
                    lblspacific.Text = "Axis Bank and Aggregator";
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event used for back to pay fee online from terms and condition page.
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
}