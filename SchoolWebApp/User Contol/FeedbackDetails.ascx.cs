using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;

/// <summary>
/// This class is used to create an user control.
/// </summary>
/// 
namespace SchoolWebApp
{
    public partial class FeedbackDetails : System.Web.UI.UserControl
    {
        #region "Const"

        private const string S_SUCCESS_MSG = "Thank you for submitting feedback!!!";
        private const string S_UPDATE_MSG = "Feedback updated successfully !!!";
        private const string FEEDBACK_FOR = "Feedback for";
        private const int FEEDBACK_FOR_SCHOOL = Constants.I_ONE;
        private const int CONCERN = Constants.I_TWO;
        #endregion

        #region "Data Member"

        private string msSiteName = Resources.SchoolSettings.ResourceManager.GetString("SiteName");

        public event EventHandler FillGrid;
        public event EventHandler ClearUserSearch;

        public bool bDisplay { get; set; }

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
                if (lblSchoolName != null)
                    lblSchoolName.Text = Session[Constants.S_SESSION_SCHOOL_NAME].ToString().Trim() + ".";
                if (!IsPostBack)
                {
                    FillFeedbackTypeRadiobuttonList();
                    InitializeControls();
                }
            }
            catch (Exception ex)
            {
                BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            }
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
                Page.Validate();
                if (Page.IsValid)
                {
                    string sMailAddressForSchool = string.Empty;
                    FeedbackDetailsBL oFeedbackDetailsBL = PopulateFeebackBL();
                    if (hidMode.Value == Constants.S_NEW_MODE)
                    {
                        oFeedbackDetailsBL.InsertFeedbackDetails();

                        string sMailAddress = Resources.SchoolSettings.ResourceManager.GetString("EmailAddress");
                        string sMailSubject = "Feedback for " + msSiteName;
                        if (optlstFeedbackFor.SelectedValue.ToString() == "School")
                        {
                            sMailAddressForSchool = oFeedbackDetailsBL.GetMailAddressForSchool(Session[Constants.S_SESSION_SCHOOL_ID].ToInt());
                            if (!sMailAddressForSchool.IsNullOrEmpty())
                            {
                                string[] sArrEmail = sMailAddressForSchool.Split(',');
                                for (int iCount = 0; iCount < sArrEmail.Length; iCount++)
                                {
                                    if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == CONCERN)
                                        CommonUtility.SendMail(sArrEmail[iCount].Trim(), sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Sir/Madam", "school"), null, System.Net.Mail.MailPriority.High);
                                    else
                                        CommonUtility.SendMail(sArrEmail[iCount].Trim(), sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Sir/Madam", "school"), null);
                                }
                            }
                            else if (sMailAddressForSchool.IsNullOrEmpty() && sMailAddress != string.Empty)
                            {

                                if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == CONCERN)
                                    CommonUtility.SendMail(sMailAddress, sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Sir/Madam", "school"), null, System.Net.Mail.MailPriority.High);
                                else
                                    CommonUtility.SendMail(sMailAddress, sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Sir/Madam", "school"), null);

                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(optlstFeedbackType.SelectedValue) == CONCERN)
                                CommonUtility.SendMail(sMailAddress, sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Superadmin", "software"), null, System.Net.Mail.MailPriority.High);
                            else
                                CommonUtility.SendMail(sMailAddress, sMailAddress, sMailSubject, GenerateMailBodyForAdmin("Superadmin", "software"), null);
                        }

                        ClearFeedbackControls();
                        ShowSuccessMsg(true, S_SUCCESS_MSG);
                    }
                    else if (hidMode.Value == Constants.S_EDIT_MODE)
                    {
                        oFeedbackDetailsBL.Feedback_Id = Convert.ToInt32(hidFeedbackId.Value);
                        string sMailAddress = Resources.SchoolSettings.ResourceManager.GetString("EmailAddress");
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
        /// This method is used to initialize the controls.
        /// </summary>
        private void InitializeControls()
        {
            hidMode.Value = Constants.S_NEW_MODE;
            txtName.Focus();
            tblDescription.Visible = bDisplay;
            new Button[] { btnCancel, btnSubmit }.ApplyEffect();
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
            DataTable oDSFeedbacktype = oFeedbackDetailsBL.RetriveFeedbackTypeFromFeedbackTypeMaster();

            optlstFeedbackType.DataSource = oDSFeedbacktype;
            optlstFeedbackType.DataTextField = "Feedback_Type";
            optlstFeedbackType.DataValueField = "Feedback_Type_Id";
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

        /// <summary>
        /// This method is used to get the mail body for Admin.
        /// </summary>
        /// <returns></returns>
        private string GenerateMailBodyForAdmin(string asDear, string asSoftware)
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
                       "<td ><font face=verdana size=2 > " + txtName.Text + "</td>" +
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
                       "<td ><font face=verdana size=2 > " + txtEmail.Text + "</td>" +
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
                        "<td ><font face=verdana size=2 > " + optlstFeedbackType.SelectedItem.Text + " </td>" +
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
                        "<td ><font face=verdana size=2 > " + txtContent.Text.Replace("\r\n", "<BR>") + " </td>" +
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
            if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Student)
                sUserRoleName = ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString() + " / Parent";
            else if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Supervisor)
                sUserRoleName = "Admin Staff";
            else
                sUserRoleName = ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString();
            return sUserRoleName;
        }
        #endregion
    }
}