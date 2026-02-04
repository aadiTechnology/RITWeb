// Class Name       :- UserDeactivePopUp
// Purpose          :- This class is used to activate and deactivate users and send relative sms to them.
// Date Of creation :- 29 April 2009
// Author Name      :- Deepak

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Collections;
using Utility;
using System.Configuration;

public partial class UserDeactivePopUp : SchoolBase
{
    #region "Data Member"
    public string msSmsSubject = string.Empty;
    #endregion

    #region "Events"

    /// <summary>
    /// This event used to intialize form controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitialiseForm();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
 
    /// <summary>
    /// This event is used lock or unlock user and send respective sms.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        try
        {
            string sMobileNumbers=string.Empty;
            int iConsideredForSMS = Constants.I_ZERO;
            int iRemoveReferances = Constants.I_ZERO;            
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();
            int iUserId = Convert.ToInt32(hidUserId.Value);
            string sFromPage = String.Empty;
            if (Request.QueryString.ToString() != String.Empty)
            {
                if (!QueryString["FromPage"].IsNullOrEmpty())
                    sFromPage = QueryString["FromPage"];
            }

            if (chkSendSms.Checked)
            {
                if (hidUserRoleId.Value.ToInt() == Constants.UserRoles.Student.ToInt())
                    sMobileNumbers = StudentBL.GetStudentMobileNumbers(iUserId,miSchoolId);
            }

            if (chkRemoveReferances.Checked)
                iRemoveReferances = Constants.I_ONE;
			
            if (btnDeactivate.Text == Resources.LocalizedResources.Deactivate )
            {                
                iConsideredForSMS = Convert.ToInt32(hidConfirmSms.Value);
                if (chkSendSms.Checked)
                {
                    if (hidUserRoleId.Value.ToInt() != Constants.UserRoles.Student.ToInt())
                        sMobileNumbers = hidMobileNo.Value.ToString();
                    string sDeactivationReason = hidSmsTemplate.Value.Replace("%REASON%", txtReason.Text.Trim());            
                    SendSMS(sDeactivationReason, msSmsSubject,sMobileNumbers);
                }
                oSchoolUserBL.LockParticularUser(iUserId, miSchoolId, miUserId, txtReason.Text, iConsideredForSMS,hidUserRoleId.Value.ToInt(),iRemoveReferances);
                Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?"
                                + CommonUtility.EncryptQuerystring("UserRoleId=" + hidUserRoleId.Value + "&NameFilter=" + hidNameFilter.Value + "&FromPage=" + sFromPage
                                + "&StandarId=" + hidStandarId.Value + "&DivisionId=" + hidDivisionId.Value + "&UserTypeId=" + hidUserTypeId.Value) + "'" + ";window.close();window.opener.focus(); </Script>");
            }
            else
            {
                iConsideredForSMS = Convert.ToInt32(hidConfirmSms.Value);
                if (chkSendSms.Checked)
                {
                    if (hidUserRoleId.Value.ToInt() != Constants.UserRoles.Student.ToInt())
                        sMobileNumbers = hidMobileNo.Value.ToString();
                    SendSMS(txtReason.Text == "" ? "No Text" : txtReason.Text, msSmsSubject, sMobileNumbers);
                }
                
                oSchoolUserBL.UnLockParticularUser(iUserId, miSchoolId, miUserId, iConsideredForSMS);
                Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + "'?"
                                + CommonUtility.EncryptQuerystring("UserRoleId=" + hidUserRoleId.Value + "&NameFilter=" + hidNameFilter.Value + "&FromPage=" + sFromPage
                                + "&StandarId=" + hidStandarId.Value + "&DivisionId=" + hidDivisionId.Value + "&UserTypeId=" + hidUserTypeId.Value) + "'" + ";window.close();window.opener.focus(); </Script>");

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method event is used to close the window.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'>window.close();</Script>");

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to set defualt activation sms or deactivation reason into textbox
    /// depending on chkSendSms is checked or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSendSms_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Activate mode and checkbox is checked then set defualt activation sms.
            if (chkSendSms.Checked && btnDeactivate.Text == Resources.LocalizedResources.Activate)
            {
                SetActiVationSms();
                EnableValidator(false);
                lblReason.Text = Resources.LocalizedResources.ActivationSMS;
                //txtReason.Enabled = true; ;
            }
            //Activate mode and checkbox is unchecked then set activation sms to empty string.
            else if (!chkSendSms.Checked && btnDeactivate.Text == Resources.LocalizedResources.Activate)
            {
                txtReason.Text = "";
                EnableValidator(false);
                lblReason.Text = Resources.LocalizedResources.ActivationSMS;
                txtReason.Enabled = false;
            }
            //Deactivate mode and checkbox is checked then set defualt Deactivate reason.
            else if (chkSendSms.Checked && btnDeactivate.Text == Resources.LocalizedResources.Deactivate)
            {
                SetDeactivationSms();
                EnableValidator(true);
                lblReason.Text = Resources.LocalizedResources.ResonForDeactivate;
                txtReason.Enabled = true;
            }
            //Deactivate mode and checkbox is unchecked then set Deactivate reason to empty string.
            else if (!chkSendSms.Checked && btnDeactivate.Text == Resources.LocalizedResources.Deactivate)
            {
                txtReason.Text = "";
                EnableValidator(true);
                lblReason.Text = Resources.LocalizedResources.ResonForDeactivate;
                txtReason.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Methods"

    /// <summary>
    /// This method initializes variables.
    /// </summary>
    private void InitialiseForm()
    {
        ReadQuerystring();
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnDeactivate.Attributes.Add("onclick", "ClearErrorLabel()");        
		if (!Settings.IsMiniSite)
			btnDeactivate.Attributes.Add("onclick", "ConfirmSms(" + btnDeactivate.ClientID + ")");
        ApplyMouseHoverEffect(new List<Button> { btnDeactivate, btnCancel });
        if (hidUserRoleId.Value.ToInt() == Constants.UserRoles.TransportStaff.ToInt() || Settings.IsMiniSite)
            trSendSms.Visible = false;
        else
            trSendSms.Visible = true;
    }

    /// <summary>
    /// This method decrypts querystring and set its values to hidden variables.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString.Count > 0)
        {
            if (QueryString["User_Id"] != null)
                hidUserId.Value = QueryString["User_Id"];
            if (QueryString["UserName"] != null)
                hidUserName.Value = QueryString["UserName"];
            if (QueryString["Mobile_Number"] != null)
                hidMobileNo.Value = QueryString["Mobile_Number"];
            if (QueryString["UserRoleId"] != null)
                hidUserRoleId.Value = QueryString["UserRoleId"];
            if (QueryString["NameFilter"] != null)
                hidNameFilter.Value = QueryString["NameFilter"];
            if (QueryString["DivisionId"] != null)
                hidDivisionId.Value = QueryString["DivisionId"];
            if (QueryString["StandarId"] != null)
                hidStandarId.Value = QueryString["StandarId"];
            if (QueryString["Deactivation_Reason"] != null)
                hidDeactivationReason.Value = QueryString["Deactivation_Reason"];
            if (QueryString["UserTypeId"] != null)
                hidUserTypeId.Value = QueryString["UserTypeId"];

             if (QueryString["IsLocked"] != null)
                 if (QueryString["IsLocked"] == Convert.ToString(Constants.C_YES))
                 {
					 trReason.Visible = !Settings.IsMiniSite;
                     trReasonOfDectivation.Visible = true;
                     lblReasonForDeactivation.Text = hidDeactivationReason.Value;
                     btnDeactivate.Text = Resources.LocalizedResources.Activate;
                     lblReason.Text = Resources.LocalizedResources.ActivationSMS;
                     txtReason.Enabled = false;
                     EnableValidator(false);
                     regvalTxtReason.ErrorMessage = Resources.LocalizedResources.ActivationSMSLength;                     
                 }
                 else
                 {
                     trReasonOfDectivation.Visible = false;
                     btnDeactivate.Text = Resources.LocalizedResources.Deactivate;
                     lblReason.Text = Resources.LocalizedResources.ResonForDeactivate;
                     EnableValidator(true);
                     regvalTxtReason.ErrorMessage = Resources.LocalizedResources.valReasonOfDeactivation;
                     txtReason.Enabled = true;
                     if (hidUserRoleId.Value.ToInt() == Constants.I_TWO)
                         trRemoveReferances.Visible = true;
                     else
                         trRemoveReferances.Visible = false;                   
                 }
             lblUserHeading.Text = hidUserName.Value;
        }
    }

    /// <summary>
    /// This method set default activation sms in text box.
    /// </summary>
    private void SetActiVationSms()
    {
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.UserActivationSMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                txtReason.Text = Convert.ToString(oDTTemplate.Rows[0][2]);

                if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    hidTemplateRegId.Value = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                msSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
                HidSMSTemplateName.Value = msSmsSubject;
            }
        }
    }

    /// <summary>
    ///  /// <summary>
    /// This method set default Deactivatio reason in text box.
    /// </summary>
    /// </summary>
    private void SetDeactivationSms()
    {
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.UserDeactivationSMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
       

        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                hidSmsTemplate.Value = oDTTemplate.Rows[0][2].ToString();
                msSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
                HidSMSTemplateName.Value = msSmsSubject;

                if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    hidTemplateRegId.Value = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();
            }
        }
    }

    /// <summary>
    /// This method used for enable disable validator
    /// </summary>
    /// <param name="abAction"></param>
    private void EnableValidator(bool abAction)
    {
        lblMandatoryFields.Visible = abAction;
        lblStar.Visible = abAction;
        reqValtxtReason.Enabled = abAction;
    }

    /// <summary>
    /// This method used to send sms.
    /// </summary>
    /// <param name="sSmsText"></param>
    /// <param name="sSmsSubject"></param>
    private void SendSMS(string sSmsText, string sSmsSubject,string asMobileNumber)
    {
        int iUserId = Convert.ToInt32(hidUserId.Value);
        Hashtable oHTUsersMobileNo=new Hashtable();

        string[] sArrMobileNumber;
        sArrMobileNumber = asMobileNumber.Split(',');
        oHTUsersMobileNo[iUserId] = sArrMobileNumber[0].Trim(); ;

        if (sArrMobileNumber.Length > Constants.I_ONE && !sArrMobileNumber[1].Trim().IsNullOrEmpty() && sArrMobileNumber[0].Trim() != sArrMobileNumber[1].Trim())
            oHTUsersMobileNo[iUserId + "sm;"] = sArrMobileNumber[1].Trim();

        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);        
        var oSMS = new SMS
        {
            Sender = oSchoolBL.SMSSenderName,
            SMSText = sSmsText,
            School_Name = oSchoolBL.SchoolName + "::" + HidSMSTemplateName.Value,
            DisplayText = hidUserName.Value,
            SchoolID = miSchoolId,
            AcademicYearID = miAcademicYearId,
            SenderID = miUserId,
            TemplateRegistrationId = hidTemplateRegId.Value,
            SenderRoleID = Constants.UserRoles.Admin.ToInt(),
            InsertedByID = miUserId
        };

        oSMS.To = oHTUsersMobileNo;
        oSMS.Send();
        oHTUsersMobileNo.Clear();
    }

    private void RefreshValue()
    {
        hidvalResetAllFields.Value = Resources.LocalizedResources.valResetAllFields;
        hidalertDeactivateUser.Value = Resources.LocalizedResources.alertDeactivateUser;
        hidalertActivateUser.Value = Resources.LocalizedResources.alertActivateUser;
    }
    #endregion
}
