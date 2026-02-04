// File Name   : SupportUI.aspx.cs
// Modified by : Amit
// Date        : 25 Sept 2009
// Description : This class is used to get new support idea/problems from user.

using System;
using System.IO;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Configuration;
using SchoolEntities;

public partial class SupportUI : SchoolBase
{

    #region Constants

    private const int I_FILE_SIZE_LIMIT = 204800;// for 200 KB
    private const string S_ERRMSG_FILE_NOT_FOUND = " Please fix the following error(s):" + "<br>" + "<ul><li>" + " File Not Found." + "<br>";
    private const string S_ERRMSG_FILE_SIZE = " Please fix the following error(s):" + "<br>" + "<ul><li>" + " Size of file is too large to upload." + "<br>";
    private const string S_SUCCESS_MSG = "Thank you for submitting support request !!!";

    #endregion

    #region Data Members

    private SupportBL moSupportBL;
    
    #endregion

    #region Events

    /// <summary>
    /// This event is used to set default properties to page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           moSupportBL = new SupportBL(miSchoolId, miAcademicYearId);         
            if (!IsPostBack)
            {
                SetDefaultProperties();
                SetClientSideScriptAttributes();                
            }
            txtEmail.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit support problem.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string sFileName;
            string sServerFilePath;
            string sMailAddressForSchool = string.Empty;
            FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
            if (IsFileUploaded(out sFileName, out sServerFilePath))
            {
                string sAdminMailAddress = ConfigurationManager.AppSettings["FromMailAddress"];
                string sFeedbackEmailAddress = ConfigurationManager.AppSettings["FeedbackEmailAddress"];      
                string sMailSubject = "Support needed to " + Settings.SiteName;
                moSupportBL.Save(Populate(sFileName));

                //Following line is used for getting the feedback email of School from Database.
                sMailAddressForSchool = oFeedbackDetailsBL.GetMailAddressForSchool(Session[Constants.S_SESSION_SCHOOL_ID].ToInt());
                sFeedbackEmailAddress = sFeedbackEmailAddress + "," + sMailAddressForSchool;

                CommonUtility.SendMail(sFeedbackEmailAddress, sAdminMailAddress, sMailSubject, GenerateMailBodyForAdmin(), sServerFilePath);
                trlblMessage.Visible = true;
                lblMessage.Visible = true;
                lblMessage.Text = S_SUCCESS_MSG;
                txtProblem.Text = "";
                txtProbSub.Text = "";
                trlblErrorMsg.Visible = false;
            }
        }
        catch (UploadFileExceptions ex)
        {
            trlblErrorMsg.Visible = true;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.CssClass = "ClsLabel";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            trlblErrorMsg.Visible = true;
            lblErrorMsg.Text = "";
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.CssClass = "ClsLabel";
            lblErrorMsg.Visible = true;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region "Private Methods"

    /// <summary>
    /// This method is used to set default properties to page controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        SchoolUserBL oSchoolUserBL;
        if (moUserRole == Constants.UserRoles.Teacher)
            oSchoolUserBL = new SchoolUserBL(miUserId, miSchoolId, miAcademicYearId, true);
        else
            oSchoolUserBL = new SchoolUserBL(miUserId);
        txtEmail.Text = oSchoolUserBL.Email.Trim();
        txtPhone.Text = oSchoolUserBL.Mobile_Number.Trim();
        trlblErrorMsg.Visible = false;
        lblUser.Text += GetUserRoleName() + ",";
    }

    /// <summary>
    /// This method is used to set java script properties to page controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        btnSubmit.Attributes.Add("Onclick", "VisibleSuccessMsg()");
        ApplyMouseHoverEffect(new List<Button> { btnSubmit }); 
    }

    /// <summary>
    /// This method is used to get the mail body for Admin
    /// </summary>
    /// <returns></returns>
    private string GenerateMailBodyForAdmin()
    {
        string strHTML = string.Empty;        

        strHTML = "<table width=100% cellpadding=0 cellspacing=0 border=0>" +
                   "<tr><td><font face=arial size=2>Dear Superadmin,</font><br><br></td></tr>" +
                   "<tr><td><font face=arial size=2>This mail is to notify that a" +
                   " member has made a request for support. Following are the details of the member.</font></td></tr>" +
                   "<tr><td>&nbsp;</td></tr>" +
                   "<tr>&nbsp;<td>" +
                   "<table width=100% cellpadding=0 cellspacing=0 border=0 bgcolor=f1f7ff > " +

                   //                             *** Do not delete ***
                   //"<tr><td height=1 colspan=3 Style=\"background-image:url(" +
			//Settings.SiteName +
                   //"\\RITeSchool\\images\\Popup_TopImg.jpg);background-repeat: repeat-x; height: 106px;\">" +
                   //"<img src=\"" +
			//Settings.SiteName +
                   //"\\RITeSchool\\images\\Popup_TopImg.jpg\"/></td>" +
                   //"<td height=1 colspan=2 width='100%'>&nbsp;</td>"+
                   //"</tr>" +

                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>User Name :</b></td>" +
                   "<td ><font face=verdana size=2 > " + GetStudentDetails() + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%' >&nbsp;</td>" +
                    "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>User Role :</b></td>" +
                    "<td ><font face=verdana size=2 > " + GetUserRoleName() + "</td>" +
                    "</tr>" +

                    //                             *** Do not delete *** 
                   //"<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   //"<tr >" +
                   //"<td  width='1%' >&nbsp;</td>" +
                   //"<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>School Name :</b></td>" +
                   //"<td ><font face=verdana size=2 > " + txtSchoolName.Text + "</td>" +
                   //"</tr>" +
                   //"<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   //"<tr >" +
                   //"<td  width='1%' >&nbsp;</td>" +
                   //"<td valign=top><font face=verdana size=2 color=#336699><b>Address:</b></td>" +
                   //"<td ><font face=verdana size=2 > " + txtAddress.Text.Trim() + " </td>" +

                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%' >&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Email Address:</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtEmail.Text.Trim() + "</td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Mobile:</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtPhone.Text + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Problem Subject:</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtProbSub.Text + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Problem Description:</b></td>" +
                    "<td ><font face=verdana size=2 > " + txtProblem.Text.Replace("\r\n", "<BR>") + " </td>" +
                    "</tr>" +
                    "</table>" +
                    "</td></tr>" +
                    "</table>";

        return strHTML;
    }

    /// <summary>
    /// This method is used to validate file to upload.
    /// </summary>
    /// <returns></returns>
    private bool IsFileUploaded(out string sFileName, out string sServerFilePath)
    {
        bool bIsValid = true;

        if (File_attatchment.FileName != string.Empty)
        {
            if (File_attatchment.HasFile)
            {
                sFileName = File_attatchment.PostedFile.FileName;
                //string sFolderName = Server.MapPath("..") + Constants.S_SUPPORT_FOLDER_LOCATION;
                string sFolderName = base.BasePath +"\\RITeSchool" + Constants.S_SUPPORT_FOLDER_LOCATION;
                sServerFilePath = sFolderName + sFileName.Substring(sFileName.LastIndexOf("//") + 1);
                File_attatchment.SaveAs(sServerFilePath);
                if (File.Exists(sServerFilePath))
                {
                    FileInfo oFile = new FileInfo(sServerFilePath);
                    if (oFile.Length > I_FILE_SIZE_LIMIT)
                    {
                        trlblErrorMsg.Visible = true;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = S_ERRMSG_FILE_SIZE;
                        bIsValid = false;
                    }
                }
            }
            else
            {
                throw new UploadFileExceptions(S_ERRMSG_FILE_NOT_FOUND);
            }
        }
        else
        {
            sFileName = string.Empty;
            sServerFilePath = string.Empty;
        }
        return bIsValid;
    }

    // <summary>
    /// This method is used to get user roll name.
    /// </summary>
    private string GetUserRoleName()
    {
        string sUserRoleName = string.Empty;
        if (moUserRole == Constants.UserRoles.Student)
            sUserRoleName = moUserRole.ToString() + " / Parent";
        else if (moUserRole== Constants.UserRoles.Supervisor)
            sUserRoleName = "Admin Staff";
        else
            sUserRoleName = moUserRole.ToString();

        return sUserRoleName;
    }

    /// <summary>
    /// This method use to populate Support Details  that use to save support details
    /// </summary>
    /// <returns></returns>
    private SupportDetails Populate(string asFileName)
    {
        SupportDetails oSupportDetails = new SupportDetails
        {

            Description = txtProblem.Text.Trim(),
            EmailAddress = txtEmail.Text.Trim(),
            FileName = asFileName,
            MobileNo = txtPhone.Text.Trim(),
            Subject = txtProbSub.Text.Trim(),
            UserId = miUserId,

        };
        return oSupportDetails;
    }

    /// <summary>
    /// This method use to populate Support Details  that use to save support details
    /// </summary>
    private string GetStudentDetails()
    {
        string sName;
        int iUserRoleId = moUserRole.ToInt();
        return sName = moSupportBL.GetStudentDetails(miUserId, iUserRoleId);
    }
    #endregion " Private Methods "

}