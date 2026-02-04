using System;
using System.Data;
using System.Reflection;
using System.Text;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Configuration;
using PushNotificationService;
using System.Collections.Generic;

public partial class ForgotPassword : System.Web.UI.Page
{                                                                                                        
    /// <summary>
   /// This event is used to set validation summary header. 
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            valSumForgotPass.HeaderText = Utility.Constants.S_VALIDATION_SUMMARY_HEADER;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
   /// This event is used to validate the user and send email/sms of the credentials to the user.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(txtUserName.Text.Trim(), txtMobileNo.Text.Trim(), CalDobPopup.DateValue);
            if (oSchoolUserBL.UserId == 0)
                ShowErrorMessage("Provided details are not valid.");
            else
            {
                SchoolBL oSchoolBL = new SchoolBL(oSchoolUserBL.SchoolId);
                SendPasswordSMS(oSchoolBL, oSchoolUserBL);
            }
        }
        catch (InvalidSqlDateTimeException sqlex)
        {
            ShowErrorMessage(sqlex.Message);
        
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check user's role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtCalDobPopup_TextChanged(object sender, EventArgs e)
    {
        CheckUserRoleAndGetEmailId();
    }

    /// <summary>
   /// This event is used to check user's role.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        CheckUserRoleAndGetEmailId();
    }

    /// <summary>
    /// This event is used to check user's role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        CheckUserRoleAndGetEmailId();
    }  

    /// <summary>
    /// This event is used to check user's role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CalDobPopup_SelectionChanged(object sender, EventArgs e)
    {
        CheckUserRoleAndGetEmailId();
    }
  
    /// <summary>
    /// This method is used to display the error messages.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowErrorMessage(string asMessage)
    {
        lblFailureText.Text = asMessage;
        lblFailureText.ForeColor = System.Drawing.Color.Red;
        trFailureText.Visible = true;
    }

    /// <summary>
    /// This method is used to send sms.
    /// </summary>
    /// <param name="asMessage"></param>
    private void SendPasswordSMS(SchoolBL oSchoolBL, SchoolUserBL oSchoolUserBL)
    {
        string sLoginDetailsSmsText = string.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iSMSType = 0;
        string sTemplateRegistrationId = string.Empty;
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, iSchoolId);
        if (oDTSmsTemplate.Rows.Count != 0)
        {
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", oSchoolUserBL.Login).Replace("%PASSWORD%", oSchoolUserBL.Password);
                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();

        }

        DataTable oDataTable = SchoolUserCollectionBL.GetPasswordRecoveryDetails(oSchoolUserBL.UserId, oSchoolBL.SchoolId);

        if (oDataTable.Rows.Count > 0 && Convert.ToChar(oDataTable.Rows[0]["IsSmsSentInDay"]) == Constants.C_NO)
        {
            SMS oSMS = new SMS();
            oSMS.SchoolID = oSchoolBL.SchoolId;
            oSMS.AcademicYearID = Convert.ToInt32(oDataTable.Rows[0]["Academic_Year_ID"]);
            oSMS.SenderID = Convert.ToInt32(oDataTable.Rows[0]["AdminUserId"]);
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;           
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.School_Name = oSchoolBL.SchoolName + " :: Forgot Password";
            oSMS.DisplayText = Convert.ToString(oDataTable.Rows[0]["UserName"]);
            oSMS.SMSType = iSMSType;
            oSMS.To.Add(oSchoolUserBL.UserId, oSchoolUserBL.Mobile_Number);
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            if (oSchoolUserBL.Mobile_Number2 != string.Empty)
                oSMS.To.Add(oSchoolUserBL.UserId + "sm;", oSchoolUserBL.Mobile_Number2);
            try
            {
                if (!string.IsNullOrEmpty(txtEmailId.Text))
                    SendEmail(sLoginDetailsSmsText, oSchoolBL.SchoolName);
                oSMS.Send();
                lblFailureText.Text = "Login details have been sent to you.";
                //to your registered mobile number.";
                lblFailureText.ForeColor = System.Drawing.Color.Blue;
                trFailureText.Visible = true;
                SendPushNotification(oSchoolBL.SchoolId, oSchoolUserBL);
            }
            catch (System.Net.Mail.SmtpFailedRecipientException)
            {
                lblFailureText.Text = "Please enter valid email id.";
                lblFailureText.ForeColor = System.Drawing.Color.Red;
                trFailureText.Visible = true;
            }
        }
        else
        {
            lblFailureText.Text = "Login details have already been sent to you. Please try after 24 Hrs.";
            lblFailureText.ForeColor = System.Drawing.Color.Blue;
            trFailureText.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to send Pushnotification to the user after make forgot password send Mail and SMS. 
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="oSchoolUserBL"></param>
    public void SendPushNotification(int aiSchoolId, SchoolUserBL oSchoolUserBL)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {
            SchoolBL oSchoolBL = new SchoolBL(aiSchoolId);

            int[] intArrayUserId = new int[1];
            intArrayUserId[0] = oSchoolUserBL.UserId;

            pushNotificationClient = new PushNotificationClient();
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_SCHOOLNAME, oSchoolBL.SchoolName);
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_USERNAME, oSchoolUserBL.Login);
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_PASSWORD, oSchoolUserBL.Password);
            pushNotificationClient.SendNotification(NotificationMessageHeadings.ForgotPassword, aiSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
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

    /// <summary>
    /// This method is used to send email.
    /// </summary>
    /// <param name="asMessage"></param>
    private void SendEmail(string asemailText, string asSchoolName)
    {
        string sToMailAddress = txtEmailId.Text;
		string sFromMailAddress = ConfigurationManager.AppSettings["EmailAddress"];
        string sSubject = "Login details for " + asSchoolName;

        StringBuilder sbContent = new StringBuilder();
        sbContent.AppendFormat("<font size ='2'>{0}<BR/><BR/>", asemailText.Replace(" ", "&nbsp;"));
        sbContent.AppendFormat("This is an auto generated mail from the {0}.<BR/>", SchoolBase.Settings.SiteName);
        sbContent.AppendFormat("Date Time : {0}<BR/></font>", DateTime.Now.ToString());
        CommonUtility.SendE_Mail(sToMailAddress, sFromMailAddress, sSubject, sbContent.ToString());
    }

    /// <summary>
    /// This method is used to check user's role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckUserRoleAndGetEmailId()
    {
        if ((!string.IsNullOrEmpty(txtUserName.Text) || !string.IsNullOrEmpty(txtMobileNo.Text)) && (CalDobPopup.DateValue != null && CalDobPopup.DateValue != DateTime.MinValue))
        {
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(txtUserName.Text.Trim(), txtMobileNo.Text.Trim(), CalDobPopup.DateValue);
            if (oSchoolUserBL != null && oSchoolUserBL.UserRoleId == 3)
            {
                tblemail.Visible = true;
            }
            else
            {
                tblemail.Visible = false;
                txtEmailId.Text = string.Empty;
            }
        }
    }

  
}
