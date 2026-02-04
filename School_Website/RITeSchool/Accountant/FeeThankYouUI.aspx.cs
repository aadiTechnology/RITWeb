using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;
using System.Configuration;

public partial class RITeSchool_Accountant_FeeThankYouUI : SchoolBase
{

	#region -- MEMBER(s) --

	private bool mbStatus;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            // Below condition is used to check payment process from mobile app.
            if (Convert.ToBoolean(HttpContext.Current.Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE]))
            {
                btnClose.Visible = false;
            }
			ReadQueryString();
			lblSiteName.Text = Settings.SiteName;
            if (mbStatus)
            {
                int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
                if (iSchoolId == Constants.SchoolId.DPIS.ToInt())
                {
                    if (QueryString["Amount"] != null && QueryString["Amount"].ToString() != string.Empty && QueryString["TxnId"] != null && QueryString["TxnId"].ToString() != string.Empty)
                        msgText.InnerHtml = "Your fee payment of Rs. " + QueryString["Amount"].ToString() + " is successfully received. Transaction No. : " + QueryString["TxnId"].ToString();
                    else
                        msgText.InnerHtml = "Your fee payment is successfully received.";
                }
                else
                    msgText.InnerHtml = "Your fee payment is successfully received.";

                msgText.InnerHtml += "<br></br>To check the fee status of your child, click on the Close button.";

                pageMessage.Attributes["class"] = "msg-box msg-success";
            }
            else
            {
                pageMessage.Attributes["class"] = "msg-box msg-warning";
                if (QueryString["StatusCode"] != null && QueryString["StatusCode"].ToString() == "002")
                {
                    msgText.InnerHtml = "<p>Your transaction is being processed. </p>If the payment has already been deducted from your bank account, please do not retry. You can safely close this window and check the status after some time.";
                }
                else
                    msgText.InnerHtml = "<p><b>An error occurred while processing your transaction.</b></p>If the amount has been deducted from your bank account, please allow some time for the details to be reflected on the website. If the details do not appear after a reasonable period, please send your transaction details to the Software Coordinator through the Message Center. <br></br>If no amount has been deducted, you may try the transaction again.";

                lblSuccess.ForeColor = System.Drawing.Color.Red;
                lblSuccess.Font.Bold = false;
                lblSuccess.Font.Size = FontUnit.Large;
            }
			//tdStatus.Visible = mbStatus;
			tdThankyou.Visible = mbStatus;
			ApplyMouseHoverEffect(new List<Button> { btnClose });

            if (Session["IsOldAcademicYearPayment"] != null)
                Session[Constants.S_SESSION_DO_REFRESH_PAGE] = 1;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			string sQueryString = SetQueryString();
			ClearSessionVariables();

            if (Session["IsOldAcademicYearPayment"] != null)
            {
                Session[Constants.S_SESSION_DO_REFRESH_PAGE] = 1;
                Session["IsOldAcademicYearPayment"] = null;
                Response.Write(string.Format("<script type=\"text/javascript\">window.opener.location.reload(); window.close(); window.opener.focus();</script>"));
            }
            else
                Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+'?" + sQueryString + "';window.opener.focus(); ");
			Response.Write("window.close();");
			Response.Write("</script>");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	private string SetQueryString()
	{
		string sQueryString = string.Empty;
		if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == "Y")
		{
			sQueryString += "StudentId=" + Session["NewStudentID"].ToString();
			sQueryString += "&Academic_Year_ID=" + Session["NewAcademicYearID"].ToString();
			sQueryString += "&StandardID=" + Session["NewStandardID"].ToString();
			string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
			sQueryString = sEncryptQueryString;
		}
		return sQueryString;
	}

	private void ClearSessionVariables()
	{
		Session["NewStudentID"] = null;
		Session["NewStandardID"] = null;
		Session["NewAcademicYearID"] = null;
		Session["IsForNextYear"] = null;
		Session["FinalAcademicYearId"] = null;
		Session["FinalYearStudentId"] = null;
	}

	private void ReadQueryString()
	{
		mbStatus = !QueryString["TransactionStatus"].IsNull() && QueryString["TransactionStatus"].ToBool();
	}

	#endregion -- PRIVATE METHOD(s) --

}
