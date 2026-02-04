/* File Name :- eSchoolLogin.aspx.cs
 * Created By :- shankar
 * Created Date :- 12-Nov-2009
 * Class Description :- This class is used to authenticate user of online admission process details.
*/
using System;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;

public partial class OnlineAdmissionlLoginUI : SchoolBase
{
    #region Event

    /// <summary>
    /// This event is used to set default button property and check whether login is allowed or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = null;
                Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = null;
                Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] = null;
                Session.Abandon();
                OnlineAdmission oOnlineAdmission = (OnlineAdmission)this.Master;
                oOnlineAdmission.SetLoginMenu(true);
                AdmissionLogin.FailureText = "";                
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
				SchoolBL oSchoolBL = new SchoolBL(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));
				hlnkEmail.NavigateUrl = "mailto:" + oSchoolBL.Email;
				hlnkEmail.Text = oSchoolBL.Email;
            }
            Button oButton = (Button)this.FindControl("ctl00$ContentPlaceHolder1$Login1$LoginButton");
            SetDefaultButton(oButton);
			Char cIsLoginAllowd = Convert.ToChar( ConfigurationManager.AppSettings["IsLoginAllowd"]) ;
                if (cIsLoginAllowd.Equals(Constants.C_NO))
                {
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring("IsErrorMsg=1");
                    oButton.Attributes.Add("onclick", "window.open('./LoginMsgPopUp.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=400,height=60'); return false;");
                }
            
            TextBox otxtUserName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$Login1$UserName");
            if (otxtUserName !=null)
                otxtUserName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for authentication.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
			Char cIsLoginAllowd = Convert.ToChar(ConfigurationManager.AppSettings["SchoolID"]);
            if (cIsLoginAllowd.Equals(Constants.C_YES))
            {
                string sLogin = AdmissionLogin.UserName.Trim();
                string sPassword = AdmissionLogin.Password;
				int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
                UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, sLogin, sPassword);
                if (oUserAuthentication.ValidUser)
                {
                    oUserAuthentication.UpdateAdmissionLoginSession();
                    Server.Transfer("OnlineAdmissionDashBoardUI.aspx", false);
                }
                else
                    AdmissionLogin.FailureText = "You are not authenticated user.";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion
}
