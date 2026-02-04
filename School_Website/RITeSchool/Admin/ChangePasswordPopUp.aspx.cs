/* File Name :- ChangePasswordPopUp.aspx/cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This Class Is used to change existing user's Password.
*/

using System;
using System.Web;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using Utility;
using System.Resources;
using PushNotificationService;

public partial class ChangeUserpassword : SchoolBase
{

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region Events
    
    /// <summary>
    /// This event is used to initialize controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {                        
            if (!IsPostBack)
            {                
                SetProperties();
                if (hidUserId.Value == Constants.S_EMPTY_STRING)
                {
                    SetUserId();
                    InitializeControls();
                }
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
            }
            txtPasswd.Focus();            
            SetJavascriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    ///<Summary>
    ///This method is used to update user's password.
    ///</Summary>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserBL oSchoolUserBL = CreateAndGetObjectForUserPassword();
            oSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
            oSchoolUserBL.SchoolId = miSchoolId;
            oSchoolUserBL.UpdateSchoolUserPassword();
            hidOldPassword.Value = txtPasswd.Text.ToString();

            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "<b>" + Resources.LocalizedResources.MsgPasswordUpdated + "</b>";
            SendPushNotification(hidUserId.Value.ToString());
        }
        catch (DuplicateUserException ex)
        {
            lblErrorMsg.Text = oResourceManager.GetString(ex.Message.Replace(" ", string.Empty)); ;
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This mehod is used to send Pushnotification when user update  Password from the Change password screen.
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="aoObject"></param>
    public override void SendPushNotification(string asUserId, object aoObject= null)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {
            int[] intArrayUserId = new int[1];
            intArrayUserId[0] = Convert.ToInt32(asUserId);

            pushNotificationClient = new PushNotificationClient();
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_SCHOOLNAME, Convert.ToString(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_NAME]));
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_USERNAME, txtLogin.Text.Trim());
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_PASSWORD, txtPasswd.Text.ToString());
            pushNotificationClient.SendNotification(NotificationMessageHeadings.ForgotPassword, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
            pushNotificationClient.Close();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                pushNotificationClient.Close();
        }
    }

    ///<Summary>
    ///This method is used to cancel the transaction.
    ///</Summary>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'> window.close();window.opener.focus(); </Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set properties of controls.
    /// </summary>
    private void SetProperties()
    {
        txtLogin.Enabled = false;
        txtPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        txtConfirmPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        lblErrorMsg.Visible = false;        
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        BtnSave.Attributes.Add("onclick", "ResetErrLabel()");        
        ApplyMouseHoverEffect(new List<System.Web.UI.WebControls.Button> { BtnSave, BtnCancel });
    }

    ///<Summary>
    ///This method is used to display current user's login name.
    ///</Summary>
    private void InitializeControls()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(Convert.ToInt32(hidUserId.Value));
        if (oSchoolUserBL != null)
        {
            txtLogin.Text = oSchoolUserBL.Login;            
            txtPasswd.Text = Constants.S_EMPTY_STRING;
            txtConfirmPasswd.Text = Constants.S_EMPTY_STRING;
            hidOldPassword.Value = oSchoolUserBL.Password.ToString();
        }
    }

    ///<Summary>
    ///This method is used to initialize object of SchoolUserBL class.
    ///</Summary>
    private SchoolUserBL CreateAndGetObjectForUserPassword()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.Login = txtLogin.Text.Trim();
        oSchoolUserBL.Password = txtPasswd.Text;
        oSchoolUserBL.UpdatedBy =Convert.ToString(miUserId);
        oSchoolUserBL.UpdatedDate = System.DateTime.Now.ToString("MM/dd/yyyy");
        return oSchoolUserBL;
    }

    /// <summary>
    /// This method is used to set user id.
    /// </summary>
    private void SetUserId()
    {
	    if (miSchoolId != 0)
		    hidUserId.Value = QueryString.Count == Constants.I_ZERO ? Convert.ToString(miUserId) : QueryString["User_Id"];
    }

    private void RefreshValue()
    {
        hidValNewPasswordBlank.Value = Resources.LocalizedResources.ValNewPasswordBlank;
        hidPasswordConditionErrorMsg.Value = Resources.LocalizedResources.PasswordConditionErrorMsg;
        hidPasswordCondition1.Value = Resources.LocalizedResources.PasswordCondition1;
        hidConfirmPasswordErrorMessage.Value = Resources.LocalizedResources.ConfirmPasswordErrorMessage;
        hidValNewAndConfirmPassword.Value = Resources.LocalizedResources.ValNewAndConfirmPassword;
    }

    #endregion    
}