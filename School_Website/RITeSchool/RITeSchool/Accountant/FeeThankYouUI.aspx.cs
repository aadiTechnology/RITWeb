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
                if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.VPMCPS.ToInt())
                {
                    if (QueryString["Amount"] != null && QueryString["Amount"].ToString() != string.Empty && QueryString["TxnId"] != null && QueryString["TxnId"].ToString() != string.Empty)
                        lblSuccess.Text = "Your fee payment of Rs. " + QueryString["Amount"].ToString() + " is successfully received. Transaction No. : " + QueryString["TxnId"].ToString();
                    else
                        lblSuccess.Text = "Your fee payment is successfully received.";
                }
                else
                    lblSuccess.Text = "Your fee payment is successfully received.";
            }
            else
            {
                lblSuccess.Text = "Error occurred while processing your transaction.<BR /><BR />If amount is deducted from your bank account then please wait for an hour and then send transaction details to Software Coordinator with Message Center facility.<BR /><BR />If amount is not deducted then please try again.";
                lblSuccess.ForeColor = System.Drawing.Color.Red;
                lblSuccess.Font.Bold = false;
                lblSuccess.Font.Size = FontUnit.Large;
            }
			tdStatus.Visible = mbStatus;
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
