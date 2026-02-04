using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class ForgotPasswordRequestPopup : SchoolBase
{

    #region -- CONSTANTS(s) --

    private const string S_SUCCESS = "Your request has been submitted successfully!!!";
    private const string S_FAILED = "Submitting request is failed. Please contact to school admin.";    

    #endregion -- CONSTANTS(s) --

    #region -- EVENT(s) --

    /// <summary>
    /// This event is called on page load & is used to initialize the components.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Initialize();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit the mail to be sent.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Captcha1.ValidateCaptcha(txtVerificationCode.Text);
            if (Captcha1.UserValidated)
            {
                SchoolUserBL oSchoolUserBL = new SchoolUserBL();
                if (!ConfigurationManager.AppSettings["SchoolID"].IsNullOrEmpty())
                {
                    int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                    string sEmails = oSchoolUserBL.GetEmailsForForgotPassword(iSchoolId);
                    if (sEmails.IsNullOrEmpty())
                        sEmails = ConfigurationManager.AppSettings["ForgotPasswordRequestMails"].ToString();

                    SendEmail(sEmails);
                    lblUpdateMessage.Text = S_SUCCESS;
                    ClearFields();                    
                }
                else
                    lblUpdateMessage.Text = S_FAILED;
            }
            else
                lblUpdateMessage.Visible = false;
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = S_FAILED;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());            
        }
    }

    /// <summary>
    /// This event is used to clear all the fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = string.Empty;
            lblUpdateMessage.Text = string.Empty;
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT(s) --

    #region -- PRIVATE METHOD(s) --
    
    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtMobileNo.Text = string.Empty;        
        txtNewMobile.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtBirhtDate.Text = string.Empty;
        rdolstRole.SelectedValue = Constants.S_ZERO;
        txtVerificationCode.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to configure mail details & send mail to the users.
    /// </summary>
    /// <param name="asEmailAddress"></param>
    private void SendEmail(string asEmailAddress)
    {
        string sFrom = txtEmail.Text;
        string sSubject = "Forgot Password Request";
        string sUrl = string.Empty;
        string sId = rdolstRole.SelectedItem.Text == "Student" ? "Registration No :" : "Employee No :";
        
        sUrl = "<table width=100% cellpadding=0 cellspacing=0 border=0>" +
            "<tr><td><font face=arial size=2>Dear Sir/Madam,</font><br><br></td></tr>" +
                   "<tr><td><font face=arial size=2>An user of " + SchoolBase.Settings.SiteName +
                   " has submitted his/her request for " + "new password" + ". Following are details for same.</font></td></tr>" +
                   "<tr><td>&nbsp;</td></tr>" +
                   "<tr>&nbsp;<td>" +
                   "<table width=100% cellpadding=0 cellspacing=0 border=0 bgcolor=f1f7ff > " +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Name :</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtName.Text + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>User Role :</b></td>" +
                   "<td ><font face=verdana size=2 > " + rdolstRole.SelectedItem.Text + "</td>" +
                   "</tr>" +

                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b> " + sId + " </b></td>" +
                    "<td ><font face=verdana size=2 > " + txtRegNo.Text + " </td>" +
                    "</tr>" +

                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Email :</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtEmail.Text + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Birth Date :</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtBirhtDate.Text.ToDateTime().ToString("dd MMM yyyy") + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Old Mobile No :</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtMobileNo.Text + " </td>" +
                    "</tr>" +

                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>New Mobile No :</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtNewMobile.Text + "</td>" +
                   "</tr>" +                   
                    
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Comment :</b></td>" +
                    "<td ><font face=verdana size=2 > " + "My mobile number has been changed due to that I am unable to get the password. Please update my old mobile number with new mobile number & send me the password.".Replace("\r\n", "<BR>") + " </td>" +
                    "</tr>" +
                    "</table>" +
                    "</td></tr>" +
                    "</table>";

        if (!asEmailAddress.IsNullOrEmpty())
            CommonUtility.SendE_Mail(asEmailAddress, Constants.S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN, sSubject, sUrl);            
    }

    /// <summary>
    /// This function is used to initialize controls to their default values.
    /// </summary>
    private void Initialize()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSubmit,btnClose,btnClear });
        base.SetDefaultButton(btnSubmit);        
        valSumError.HeaderText = Utility.Constants.S_VALIDATION_SUMMARY_HEADER;
        txtName.Focus();                
    }
      
    #endregion -- PRIVATE METHOD(s) --    
}