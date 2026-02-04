using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using SchoolEntities;
using Utility;
using System.Configuration;
/// <summary>
/// This class is used to create an user control.
/// </summary>
public partial class FeedbackDetails : UserControlBase
{
    #region "Constants"

    private const string S_SUCCESS_MSG = "Thank you for submitting feedback !!!";
    private const string S_UPDATE_MSG = "Feedback updated successfully !!!";
    private const string FEEDBACK_FOR = "Feedback for";
    private const string SIRMADAM = "Sir/Madam";
    private const string ADMIN = "Admin";
    private const string SOFTWARE = "Software";
    #endregion

    #region "Data Member"

    private string msSiteName = SchoolBase.Settings.SiteName;
    private string msFirstEmailAddress = string.Empty;
    private List<FeedbackTemplate> mlstFeedbackTemplates = null;
    private List<FeedbackType> mlstFeedbackTypes = null;
    public event EventHandler FillGrid;
    public event EventHandler ClearUserSearch;  
 
    public bool bDisplay { get; set; }

    #endregion

    #region enums

    public enum FeedbackSubTypes
    {
        GENERAL = 1,
        CONCERN = 2,
        TESTIMONIAL = 3
    }

    public enum FeedbackForType
    {
        FEEDBACK_FOR_SCHOOL = 1,
        FEEDBACK_FOR_SOFTWARE = 2
    }

    #endregion

    #region "Events"
    /// <summary>
    /// This method is used to fill feedback type radiobutton list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetQuerryString();
            this.InitializeMemberVariables();
            if (lblSchoolName != null)
                lblSchoolName.Text = Session[Constants.S_SESSION_SCHOOL_NAME].ToString().Trim() + ".";
            if (!IsPostBack)
            {
                FillFeedbackTypeRadiobuttonList();
                InitializeControls();
                GetFeedbackURL();
                GetLoginUserRole();
                SetAttributesAccordingtoUserRole();              
            }
            else
                mlstFeedbackTemplates = ViewState["FeedbackTemplates"] as List<FeedbackTemplate>;           
            
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// To set validation property according login role and feedback type
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void optlstFeedbackFor_SelectedIndexChanged(object sender, EventArgs e)
    {   
    }
    

    /// <summary>
    /// This method is used to submit the feedback.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if ((HidIsStudentLogin.Value == Constants.S_ONE) && (optlstFeedbackFor.SelectedValue == SOFTWARE))
            {
                string sMailSubject = "Feedback for " + SOFTWARE;
                string sMailAddresses = ConfigurationManager.AppSettings["EmailAddress"];
                string sAdminEmailAddress= ConfigurationManager.AppSettings["FromMailAddress"];
                string sTemplate = GenerateMailBodyForAdmin(ADMIN, SOFTWARE, true);
                CommonUtility.SendMail(sMailAddresses, sAdminEmailAddress, sMailSubject, sTemplate, null);
            }            
            else
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    string sMailAddressForSchool = string.Empty;
                    FeedbackDetailsBL oFeedbackDetailsBL = PopulateFeebackBL();
                    if (hidMode.Value == Constants.S_NEW_MODE)
                    {
                        oFeedbackDetailsBL.InsertFeedbackDetails();

                        string sAdminEmailAddress = ConfigurationManager.AppSettings["FromMailAddress"];                       
                        string sMailAddresses = ConfigurationManager.AppSettings["FeedbackEmailAddress"];

                        string sMailSubject = "Feedback for " + msSiteName;
                        sMailAddressForSchool = oFeedbackDetailsBL.GetMailAddressForSchool(Session[Constants.S_SESSION_SCHOOL_ID].ToInt());
                        if (optlstFeedbackFor.SelectedValue.ToString() == "School")
                        {                            
                            string sTemplate = GenerateMailBodyForAdmin(ADMIN, "School", false);
                            if (!sMailAddressForSchool.IsNullOrEmpty())
                            {
                                string[] sArrEmail = sMailAddressForSchool.Split(',');
                                msFirstEmailAddress = sArrEmail[Constants.I_ZERO].Trim();

                                for (int iCount = 0; iCount < sArrEmail.Length; iCount++)
                                {
                                    if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == FeedbackSubTypes.CONCERN.ToInt())
                                        CommonUtility.SendMail(sArrEmail[iCount].Trim(), sAdminEmailAddress, sMailSubject, sTemplate, null, System.Net.Mail.MailPriority.High);
                                    else
                                        CommonUtility.SendMail(sArrEmail[iCount].Trim(), sAdminEmailAddress, sMailSubject, sTemplate, null);
                                }

                            }
                            if (sAdminEmailAddress != string.Empty)
                            {
                                if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == FeedbackSubTypes.CONCERN.ToInt())
                                    CommonUtility.SendMail(sMailAddresses, sAdminEmailAddress, sMailSubject, sTemplate, null, System.Net.Mail.MailPriority.High);
                                else
                                    CommonUtility.SendMail(sMailAddresses, sAdminEmailAddress, sMailSubject, sTemplate, null);
                            }

                            if (!sMailAddressForSchool.IsNullOrEmpty())
                                SetFeedbackType(txtEmail.Text.ToString(), msFirstEmailAddress, sMailSubject, sTemplate, FeedbackForType.FEEDBACK_FOR_SCHOOL.ToInt(), Convert.ToInt32(optlstFeedbackType.SelectedValue));
                            else
                                SetFeedbackType(txtEmail.Text.ToString(), sAdminEmailAddress, sMailSubject, sTemplate, FeedbackForType.FEEDBACK_FOR_SCHOOL.ToInt(), Convert.ToInt32(optlstFeedbackType.SelectedValue));
                        }
                        else
                        {
                            string sTemplate = GenerateMailBodyForAdmin(ADMIN, SOFTWARE, false);
                            sMailAddresses = sMailAddresses + "," + sMailAddressForSchool;

                            if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == FeedbackSubTypes.CONCERN.ToInt())
                                CommonUtility.SendMail(sMailAddresses, sAdminEmailAddress, sMailSubject, sTemplate, null, System.Net.Mail.MailPriority.High);
                            else
                                CommonUtility.SendMail(sMailAddresses, sAdminEmailAddress, sMailSubject, sTemplate, null);
                            SetFeedbackType(txtEmail.Text.ToString(), sAdminEmailAddress, sMailSubject, sTemplate, FeedbackForType.FEEDBACK_FOR_SOFTWARE.ToInt(), Convert.ToInt32(optlstFeedbackType.SelectedValue));
                        }

                        ClearFeedbackControls();
                        ShowSuccessMsg(true, S_SUCCESS_MSG);
                    }
                    else if (hidMode.Value == Constants.S_EDIT_MODE)
                    {
                        oFeedbackDetailsBL.Feedback_Id = Convert.ToInt32(hidFeedbackId.Value);
                        string sMailAddress = ConfigurationManager.AppSettings["EmailAddress"];
                        string sMailSubject = "Feedback for " + msSiteName;
                        oFeedbackDetailsBL.SaveSelectedFeedback(CommonUtility.GenerateXml(oFeedbackDetailsBL), 1);
                        ClearFeedbackControls();
                        hidMode.Value = Constants.S_NEW_MODE;
                        ShowSuccessMsg(true, S_UPDATE_MSG);
                    }

                    if (FillGrid != null)
                    {
                        FillGrid(sender, e);
                    }
                }
            }            
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to clear the control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFeedbackControls();
            if (ClearUserSearch != null)
                ClearUserSearch(sender, e);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region "Public method"

    /// <summary>
    /// This method is used to fill controls in edit mode.
    /// </summary>
    /// <param name="aiFeedbackId"></param>
    /// <param name="aiSchoolId"></param>
    public void FillControls(int aiFeedbackId, int aiSchoolId)
    {
        FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
        oFeedbackDetailsBL.GetFeedbackToEdit(aiFeedbackId, aiSchoolId);
        optlstFeedbackFor.SelectedValue = oFeedbackDetailsBL.FeedbackFor;
        optlstFeedbackType.SelectedValue = oFeedbackDetailsBL.Feedback_Type_Id.ToString();
        txtContent.Text = oFeedbackDetailsBL.FeedbackDescription;
        txtEmail.Text = oFeedbackDetailsBL.Email;
        txtName.Text = oFeedbackDetailsBL.UserName;
        hidMode.Value = Constants.S_EDIT_MODE;
        hidFeedbackId.Value = aiFeedbackId.ToString();
    }

    #endregion

    #region "Private Member"

    /// <summary>
    /// This is used to select correct method from SendReplyToMail().
    /// </summary>
    /// <param name="asMailAddressTo"></param>
    /// <param name="asMailAddress"></param>
    /// <param name="asMailSubject"></param>
    /// <param name="asTemplate"></param>
    /// <param name="aiFeedbackFor"></param>
    /// <param name="aiFeedbackType"></param>
    private void SetFeedbackType(string asMailAddressTo, string asMailAddress, string asMailSubject, string asTemplate, int aiFeedbackFor, int aiFeedbackType)
    {
        if (aiFeedbackType == FeedbackSubTypes.CONCERN.ToInt())
            SendReplyToMail(asMailAddressTo, asMailAddress, asMailSubject, asTemplate, aiFeedbackFor, FeedbackSubTypes.CONCERN.ToInt(), 1);
        else if (aiFeedbackType == FeedbackSubTypes.GENERAL.ToInt())
            SendReplyToMail(asMailAddressTo, asMailAddress, asMailSubject, asTemplate, aiFeedbackFor, FeedbackSubTypes.GENERAL.ToInt(), 0);
        else if (aiFeedbackType == FeedbackSubTypes.TESTIMONIAL.ToInt())
            SendReplyToMail(asMailAddressTo, asMailAddress, asMailSubject, asTemplate, aiFeedbackFor, FeedbackSubTypes.TESTIMONIAL.ToInt(), 0);
    }

    /// <summary>
    /// This method is a common to reply all types of mails.
    /// </summary>
    /// <param name="asToMail"></param>
    /// <param name="asFromMail"></param>
    /// <param name="asSubject"></param>
    /// <param name="asMailMatter"></param>
    /// <param name="asFileName"></param>
    /// <param name="asMailPriority"></param>
    private void SendReplyToMail(string asToMail, string asFromMail, string asSubject, string asMailMatter, int aiFeedBackFor, int aiFeedBackType, int aiMailPriority)
    {

        var oTemplate = mlstFeedbackTemplates.Where(feedback => feedback.FeedbackFor == aiFeedBackFor && feedback.FeedbackTypeId == aiFeedBackType);
        string sTemplate = string.Empty;
        if (oTemplate.Count() > Constants.I_ZERO)
        {
            sTemplate = oTemplate.FirstOrDefault().Name;
            string sGenerateMailBodyForUser = GenerateMailBodyForUser(txtName.Text, sTemplate, aiFeedBackFor);

            //This is to send mails with high priority.                
            if (aiMailPriority == Constants.I_ONE)
                CommonUtility.SendMail(asToMail, asFromMail, "RE:" + asSubject, sGenerateMailBodyForUser, null, System.Net.Mail.MailPriority.High);
            else
                //This is to send mails with low priority.
                CommonUtility.SendMail(asToMail, asFromMail, "RE:" + asSubject, sGenerateMailBodyForUser, null);
        }
    }

    /// <summary>
    /// This method is used to initialize the controls.
    /// </summary>
    private void InitializeControls()
    {
        hidMode.Value = Constants.S_NEW_MODE;
        txtName.Focus();
        tblDescription.Visible = bDisplay;
        new Button[] { btnCancel, btnSubmit, btnClose }.ApplyEffect();
    }

    /// <summary>
    /// This method is used to show the success msg.
    /// </summary>
    /// <param name="abFlag"></param>
    /// <param name="asMsg"></param>
    private void ShowSuccessMsg(bool abFlag, string asMsg)
    {
        lblMessage.Visible = abFlag;
        lblMessage.Text = asMsg;
    }

    /// <summary>
    /// This method is used to get data table to fill radio button in Feedback type.
    /// </summary>
    /// <returns></returns> 
    private void FillFeedbackTypeRadiobuttonList()
    {
        FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
        mlstFeedbackTemplates = oFeedbackDetailsBL.RetriveFeedbackTypeFromFeedbackTypeMaster();
        ViewState["FeedbackTemplates"] = mlstFeedbackTemplates;
        mlstFeedbackTypes = oFeedbackDetailsBL.FeedbackTypes;
       
            optlstFeedbackType.DataSource = mlstFeedbackTypes;
            optlstFeedbackType.DataTextField = "Type";
            optlstFeedbackType.DataValueField = "Id";
            optlstFeedbackType.DataBind();

            if (optlstFeedbackType.Items.Count > Constants.I_ZERO)
                optlstFeedbackType.Items[0].Selected = true;        
    }

    /// <summary>
    /// This method is used to populate Feed back information.
    /// </summary>
    private FeedbackDetailsBL PopulateFeebackBL()
    {
        FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
        oFeedbackDetailsBL.School_Id = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        oFeedbackDetailsBL.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oFeedbackDetailsBL.Feedback_Type_Id = Convert.ToInt32(optlstFeedbackType.SelectedValue);
        oFeedbackDetailsBL.FeedbackFor = optlstFeedbackFor.SelectedValue;
        oFeedbackDetailsBL.FeedbackDescription = txtContent.Text.Trim();
        oFeedbackDetailsBL.User_Id = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oFeedbackDetailsBL.UserName = txtName.Text;
        oFeedbackDetailsBL.Email = txtEmail.Text.Trim();
        return oFeedbackDetailsBL;
    }


    private string GenerateMailBodyForUser(string asDear, string asSoftware, int aiFeedBackFor)
    {

        string sStr = string.Empty;
        if (aiFeedBackFor == FeedbackForType.FEEDBACK_FOR_SCHOOL.ToInt())
        {
            if (!SchoolBase.Settings.Location.ToString().IsNullOrEmpty())
                sStr = "<p><font face=arial size=2>Dear " + asDear + ",<br /><br />" + asSoftware + "&nbsp;<br /><br />Regards, <br/>" + Session[Constants.S_SESSION_SCHOOL_NAME].ToString() + " - " + SchoolBase.Settings.Location.ToString() + "<br/><br/>This is an auto-generated email and need not be replied back.</font></p>";
            else
                sStr = "<p><font face=arial size=2>Dear " + asDear + ",<br /><br />" + asSoftware + "&nbsp;<br /><br />Regards, <br/>" + Session[Constants.S_SESSION_SCHOOL_NAME].ToString() + "<br/><br/>This is an auto-generated email and need not be replied back.</font></p>";
        }
        else
            sStr = "<p><font face=arial size=2>Dear " + asDear + ",<br /><br />" + asSoftware + "&nbsp;<br /><br />Regards, <br/> Software Co-ordinator<br/><br/>This is an auto-generated email and need not be replied back.</font></p>";

        return sStr;

        //string sStr1="<p><br />Dear AAA,<br /><br />Text<br /><br />Thanks &amp; Regards,<br />School&nbsp;</p>"
    }

    /// <summary>
    /// This method is used to get the mail body for Admin.
    /// </summary>
    /// <returns></returns>
    private string GenerateMailBodyForAdmin(string asDear, string asSoftware, bool aiFlag)
    {
        string strHTML = string.Empty;
        
        strHTML = "<table width=100% cellpadding=0 cellspacing=0 border=0>" +
            "<tr><td><font face=arial size=2>Dear " + asDear + ",</font><br><br></td></tr>" +
                   "<tr><td><font face=arial size=2>An user of " + msSiteName +
                   " has submitted his/her feedback for " + asSoftware + ". Following are details for same.</font></td></tr>" +
                   "<tr><td>&nbsp;</td></tr>" +
                   "<tr>&nbsp;<td>" +
                   "<table width=100% cellpadding=0 cellspacing=0 border=0 bgcolor=f1f7ff > " +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>User Name :</b></td>" +
                   "<td ><font face=verdana size=2 > " + (aiFlag ?  Session[Constants.S_SESSION_USER_FULLNAME].ToString() : txtName.Text) + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>User Role :</b></td>" +
                   "<td ><font face=verdana size=2 > " + GetUserRoleName() + "</td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Email :</b></td>" +
                   "<td ><font face=verdana size=2 > " + (aiFlag ? "-" : txtEmail.Text) + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Feedback Date:</b></td>" +
                    "<td ><font face=verdana size=2 > " + DateTime.Now.ToString("dd MMM yyyy") + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Feedback Type:</b></td>" +
                    "<td ><font face=verdana size=2 > " + (aiFlag ? "-" : optlstFeedbackType.SelectedItem.Text) + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Feedback For:</b></td>" +
                    "<td ><font face=verdana size=2 > " + optlstFeedbackFor.SelectedItem.Text + " </td>" +                     
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Comments:</b></td>" +
                    "<td ><font face=verdana size=2 > " + (aiFlag ? "-" : txtContent.Text.Replace("\r\n", "<BR>")) + " </td>" +                     
                    "</tr>" +
                    "</table>" +
                    "</td></tr>" +
                    "</table>";

        return strHTML;
    }   

    /// <summary>
    /// This method is used to clear/default all control respect feedback.
    /// </summary>
    public void ClearFeedbackControls()
    {
        txtContent.Text = string.Empty;
        txtEmail.Text = string.Empty;
        optlstFeedbackFor.SelectedIndex = Constants.I_ZERO;
        optlstFeedbackType.SelectedIndex = Constants.I_ZERO;
        txtName.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to fill all control.
    /// </summary>
    private string GetUserRoleName()
    {
        string sUserRoleName;
        if (moUserRole == Constants.UserRoles.Student)
            sUserRoleName = moUserRole.ToString() + " / Parent";
        else if (moUserRole == Constants.UserRoles.Supervisor)
            sUserRoleName = "Admin Staff";
        else
            sUserRoleName = moUserRole.ToString();
        return sUserRoleName;
    }

    /// <summary>
    /// To get Software Feedback URL frol database. 
    /// </summary>
    private void GetFeedbackURL()
    {
        hidSoftwareFeedbackURL.Value = SchoolBase.Settings.SoftwareFeedbackLink;      
    }

    /// <summary>
    /// This Methode is used to check Login User Role and assign value to hidden variable.
    /// </summary>
    private void GetLoginUserRole()
    {
        if (moUserRole != Constants.UserRoles.Student)
            HidIsStudentLogin.Value = Constants.S_ONE;       
    }

    /// <summary>
    /// To set Attributes according to usr role.
    /// </summary>
    private void SetAttributesAccordingtoUserRole()
    {
        optlstFeedbackFor.Attributes["onclick"] = "SetFeedbackViewAsPerRole()";
        if (moUserRole != Constants.UserRoles.Student)
            btnSubmit.Attributes["onclick"] = "OpenSoftwareFeedbackPopUp();";
    }

    /// <summary>
    /// To get querry string.
    /// </summary>
    private void GetQuerryString()
    {
        if (Request.QueryString.ToString().Length > 1)
            btnClose.Visible = false;
    }
   
    #endregion    
}