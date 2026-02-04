/* File Name :- OnlineAdmission.master.cs
 * Modified By :- Shankar
 * Modified Date :- 17-Nov-2009
 * Class Description :- This class represents master page for admission process.
*/

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Resources;
using Utility;

public partial class OnlineAdmissionNew : BaseMasterPage
{

	#region -- MEMBER(s) --

	private string msPageUrl = String.Empty;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Collects information abou the Page Request.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
	{
		try
		{
			msPageUrl = Request.AppRelativeCurrentExecutionFilePath;

			if (!Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]).IsNull() && Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]) != Constants.S_ZERO && Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]) != Session[Constants.S_SESSION_USER_ID].ToString())
                Response.Redirect("~/RITeSchool/Common/Error.aspx?" + CommonUtility.EncryptQuerystring("Is_Session_Shared=Y"), true);

			// Check if Admission forms are closed.
			if (Request.AppRelativeCurrentExecutionFilePath.IndexOf("OnlineAdmissionUI.aspx") != -1)
			{
                //var oStudentAdmissionsBL = new StudentAdmissionsBL();
                //DataSet oDDataSet = oStudentAdmissionsBL.GetCurrentAdmissionStatus(ConfigurationManager.AppSettings["SchoolID"].ToInt());
                //if (oDDataSet.Tables.Count <= 0 || oDDataSet.Tables[0].Rows.Count <= 0)
                //    //Server.Transfer("~/RITeSchool/Admission/OnlineAdmissionUI.aspx");
                //    Response.Redirect("~/RITeSchool/Admission/OnlineAdmissionUI.aspx",false);
			}
			
			base.OnInit(e);
		}
		catch (ThreadAbortException)
		{
			// Do nothing. ASP.NET is redirecting.
			// Always comment this so other developers know why the exception 
			// is being swallowed.
		}
		catch (Exception ex)
		{
            //ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	/// <summary>
	/// This event is used to display site updation date.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);
			if (!Session[Constants.S_SESSION_USER_ID].IsNull())
                hidSessionUserId.Value = Session[Constants.S_SESSION_USER_ID].ToString();
			KeepMenuSelected();
			UpdateLogo();
			hidServerDate.Value = DateTime.Now.Date.Year.ToString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	/// <summary>
	/// This event is used to redirect towards login page on click of login link.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void BtnRITLogin_Login(object sender, EventArgs e)
	{
		try
		{
			Response.Redirect(SchoolBase.Settings.SubDomainLoginUrl, false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///  This method is used to display login menu with appropriate menu selected
	/// </summary>
	private void KeepMenuSelected()
	{
		String sPageName = Path.GetFileName(Request.Url.AbsolutePath);
		foreach (MenuItem oParentMenu in AdmissionMenus.Items)
		{
			if (Path.GetFileName(oParentMenu.NavigateUrl) == sPageName)
				oParentMenu.Selected = true;
			else
				foreach (MenuItem oChildMenu in oParentMenu.ChildItems)
					if (Path.GetFileName(oParentMenu.NavigateUrl) == sPageName)
						oChildMenu.Selected = true;
		}
		SetLoginMenu(false);
	}

	private void UpdateLogo()
	{
		if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SS.ToInt())
		{
			schoolLogo.Attributes["style"] = "height: 100px; background: url('../../images/Logo.png') no-repeat scroll center center #fff;";
			schoolLogo.Controls.Clear();
		}
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DSK.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../../images/LogoDSK.jpg') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.JPS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../../images/LogoJPS_Old.png') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.NEMS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../../images/schoollogo.jpg') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.JOS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/JOSlogo.gif') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.EPPS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/eppslogo.png') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.LORDDS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/LORDDS_Logo.png') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.BFS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/schoollogoBFS.jpg') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 130px; background: url('../images/schoollogoSNS.jpg') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PKSC.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/School_LogoPKSC.bmp') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 150px; background: url('../images/School_LogoSPS.bmp') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.MVPS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 140px; background: url('../images/School_LogoMVPS.bmp') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/School_LogoDPIS.png?version=1.0') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.ZLSP.ToInt())
        {
            trSchoolLogo.Visible = false;
            trSchoolLogozlsp.Visible = true;
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PEMS.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/School_Logo_PEMS.bmp?version=1.0') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DYPV.ToInt())
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/School_LogoDYP.jpg?version=1.0') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() >= 150 && ConfigurationManager.AppSettings["SchoolID"].ToInt() <= 158)
        {
            schoolLogo.Attributes["style"] = "height: 100px; background: url('../images/Logos/School_Logo.bmp?version=1.0') no-repeat scroll center center #fff;";
            schoolLogo.Controls.Clear();
        }

        schoolLogo.Visible = ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.FBS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.MCPS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.ZLSP.ToInt();
    }

	/// <summary>
	/// This method is used to display login menu with appropriate text and fee receipt enable or disable depending on seesion existence
	/// </summary>
	public void SetLoginMenu(bool bLogout)
	{
		if (!bLogout && Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] != null && Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
		{
			AdmissionMenus.Items[4].Text = "Logout";
			AdmissionMenus.Items[2].NavigateUrl = String.Format("javascript:openReceipt('{0}');", AdmissionMenus.Items[2].NavigateUrl + "?" + CommonUtility.EncryptQuerystring("iAdmissionId=" + Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]));
			AdmissionMenus.Items[3].NavigateUrl = String.Format("javascript:openReceipt('{0}');", AdmissionMenus.Items[3].NavigateUrl + "?" + CommonUtility.EncryptQuerystring("iAdmissionId=" + Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]));
			AdmissionMenus.Items[1].Enabled = true;
		}
		else
		{
			AdmissionMenus.Items[4].Text = "Log In";
			AdmissionMenus.Items[2].Enabled = false;
			AdmissionMenus.Items[3].Enabled = false;
			AdmissionMenus.Items[1].Enabled = false;
		}
	}

	#endregion -- PRIVATE METHOD(s) --

}
