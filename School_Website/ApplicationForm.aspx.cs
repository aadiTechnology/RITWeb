using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using SchoolEntities;
using DataCommunicator;
using CareerEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Configuration;


public partial class ApplicationForm: SchoolBase
{
    #region "Data Member and Constants"

    string msSiteName = Settings.SiteName;
	string msSchoolName = ConfigurationManager.AppSettings["SchoolName"];
    string msLocation = Settings.Location;
    string msServerFilePath;
    string msFileName = string.Empty;

    const int I_FILE_SIZE_LIMIT = 204800;// for 200 KB 
    const string S_FILE_NOT_FOUND = "File Not Found";
    const string S_FOLDER_NAME = "\\Resume\\";


    #endregion "Data Member and Constants"

    #region "Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			if (!IsPostBack)			
				txtName.Focus();			
			
			ApplyMouseHoverEffect(new List<Button> { btnSubmit, btnClear });
			valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

        try
        {
            ResetControls();
            lblSuccessful.Visible =  lblFileError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Constants.B_ACTIVITY_LOGGING = false;
            Captcha1.ValidateCaptcha(txtVerificationCode.Text);
            if (Captcha1.UserValidated)
            {               
                if (IsFileUploaded())
                {
                    
                    CareerDetailsBL oCareerDetailsBL = new CareerDetailsBL();
                    oCareerDetailsBL.CareerDetails = PopulateObjects();
                    oCareerDetailsBL.Save();
					SchoolBL oSchoolBL = new SchoolBL(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));
					string sMailAddress = ConfigurationManager.AppSettings["EmailAddress"];
					string sFromMailAddress = ConfigurationManager.AppSettings["FromMailAddress"];
                    string sMailSubject = "Career detail Request " + msSiteName + " - " + txtPosition.Text;
                    string sMailSubjectforCandidate = "Auto Reply: Application To " + msSchoolName + (msLocation.IsNullOrEmpty() ? "" : " - " + msLocation);
                    string sCareerEmails = oSchoolBL.CareerEmails;
                    string sMailAdressofCandidate = txtEmail.Text.Trim();
                    if (!sCareerEmails.IsNullOrEmpty())
                    {
                        string[] arrMailAddress = sCareerEmails.Split(',');
                        foreach (string sAddress in arrMailAddress)
                        {
                            CommonUtility.SendMail(sAddress, sFromMailAddress, sMailSubject, GenerateMailBodyForAdmin(), msServerFilePath);
                        }
                    }
                    else
                        CommonUtility.SendMail(sMailAddress, sFromMailAddress, sMailSubject, GenerateMailBodyForAdmin(), msServerFilePath);

                    //This Method is used to send email to the candidate.
                    CommonUtility.SendMail(sMailAdressofCandidate, sFromMailAddress, sMailSubjectforCandidate, GenerateMailBodyForCandidate(), null);

                    ResetControls();
                    lblSuccessful.Visible = true;
                    lblSuccessful.Text = "Resume is submitted successfully !!!";
                }
                else
                {
                    lblFileError.Visible = true;
                    lblFileError.Text = "File size should not be more than 200 KB.";
                    lblSuccessful.Visible = false;
                }
            }
            else
            {                
                lblSuccessful.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            Constants.B_ACTIVITY_LOGGING = true;
        }

    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to set the reset the control value to blank.
    /// </summary>
    /// <returns></returns>
    private void ResetControls()
    {
        txtName.Focus();
        txtName.Text = string.Empty;
        cal_DOB.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtYearOfExperience.Text = string.Empty;
        txtPosition.Text = string.Empty;
        txtLastOrganisation.Text = string.Empty;
        txtVerificationCode.Text = string.Empty;
        txtAreaOfSpecialization.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to check the file size.
    /// </summary>
    /// <returns></returns>
    private bool IsFileUploaded()
    {
        bool bIsValid = true;

        if (File_attatchment.FileName != string.Empty)
        {
            if (File_attatchment.HasFile)
            {
                string sFolderName = Server.MapPath(".") + S_FOLDER_NAME;
                msFileName = CommonUtility.GetFileNameForRenaming(File_attatchment.FileName);
                msServerFilePath = sFolderName + msFileName;
                FileInfo oFile = new FileInfo(msServerFilePath);
                if (File_attatchment.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                    bIsValid = false;
                else
                    File_attatchment.SaveAs(msServerFilePath);
            }
            else
            {
                throw new UploadFileExceptions(S_FILE_NOT_FOUND);
            }
        }
        return bIsValid;
    }
   
    /// <summary>
    /// This method is used to get the mail body for Admin.
    /// </summary>
    /// <returns></returns>
    private string GenerateMailBodyForAdmin()
    {
        string strHTML = "";
        string sAddress = (txtAddress.Text != string.Empty) ? txtAddress.Text : string.Empty;
        string sMobNo = (txtMobileNo.Text != string.Empty) ? txtMobileNo.Text : string.Empty;
        strHTML = "<table width=100% cellpadding=0 cellspacing=0 border=0>" +
                   "<tr><td><font face=arial size=2>Dear Career Admin,</font><br><br></td></tr>" +
                   "<tr><td><font face=arial size=2>An applicant has submitted resume on " + msSiteName +
                   " .Following are the details.</font></td></tr>" +
                   "<tr><td>&nbsp;</td></tr>" +
                   "<tr>&nbsp;<td>" +
                   "<table width=100% cellpadding=0 cellspacing=0 border=0 bgcolor=f1f7ff > " +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Applicant Name :</b></td>" +
                   "<td ><font face=verdana size=2 >  " + txtName.Text + " </td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Date of Birth :</b></td>" +
                   "<td ><font face=verdana size=2 >  " +  cal_DOB.Text + " </td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Address :</b></td>" +
                   "<td ><font face=verdana size=2 > " + sAddress + "</td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Email :</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtEmail.Text + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Mobile Number  :</b></td>" +
                   "<td ><font face=verdana size=2 > " + sMobNo + "</td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Years of Experience :</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtYearOfExperience.Text + "</td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Position Applied For:</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtPosition.Text + "</td>" +
                   "</tr>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Last Organization Worked For:</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtLastOrganisation.Text + "</td>" +
                   "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                   "<tr >" +
                   "<td  width='1%' >&nbsp;</td>" +
                   "<td width='20%' valign=top><font face=verdana size=2 color=#336699><b>Area of Specialization:</b></td>" +
                   "<td ><font face=verdana size=2 > " + txtAreaOfSpecialization.Text + "</td>" +
                   "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "<tr >" +
                    "<td  width='1%'>&nbsp;</td>" +
                    "<td valign=top><font face=verdana size=2 color=#336699><b>Resume Submit Date:</b></td>" +
                    "<td ><font face=verdana size=2 > " + DateTime.Now.ToString("dd MMM yyyy") + " </td>" +
                    "</tr>" +
                    "<tr ><td bgcolor=white height=1 colspan=3></td></tr>" +
                    "</table>" +
                    "</td></tr>" +
                    "</table>";
        return strHTML;
    }

    /// <summary>
    /// This method is used to get the mail body for Candidate.
    /// </summary>
    /// <returns></returns>
    private string GenerateMailBodyForCandidate()
    {
        string strHTML = "";        

        strHTML = "<table width=100% cellpadding=0 cellspacing=0 border=0>" +
                   "<tr><td><font face=arial size=2>Dear Candidate,</font><br><br></td></tr>" +
                   "<tr><td><font face=arial size=2>Thanks for applying to " + msSchoolName + (msLocation.IsNullOrEmpty() ? "" : " - " + msLocation) +
                   " for the post of " + txtPosition.Text + ". We are in receipt of your resume. In case your profile matches our requirements, we will revert back to you in due course.</font></td></tr>" +
                   "<tr><td><font face=arial size=2>This is an auto-generated email and need not be replied back.</font><br></td></tr>" +
                   "<tr>&nbsp;<td>" +
                   "<tr><td><font face=arial size=2>Best wishes,</font><br></td></tr>" +
                   "<tr><td><font face=arial size=2>" + msSchoolName + (msLocation.IsNullOrEmpty() ? "" : " - " + msLocation) + "</font><br></td></tr>" +
                   "<tr><td><font face=arial size=2>" + msSiteName + "</font></td></tr>" + 
                    "</td></tr>" +
                    "</table>";
        return strHTML;
    }

    /// <summary>
    /// This method is used to initialize the class members.
    /// </summary>
    /// <returns></returns>
    private CareerDetailsInfo PopulateObjects()
    {
        CareerDetailsInfo oStudentInfo = new CareerDetailsInfo
        {
            Name = txtName.Text.ToString().Trim(),
            DOB = Convert.ToDateTime(cal_DOB.Text),
            Address = txtAddress.Text.ToString().Trim(),
            Email = txtEmail.Text.ToString().Trim(),
            MobileNo = txtMobileNo.Text.ToString().Trim(),
            YearOfExperience = txtYearOfExperience.Text.ToString().Trim() != string.Empty ? Convert.ToDecimal(txtYearOfExperience.Text.ToString().Trim()) : 0,
            Post = txtPosition.Text.ToString().Trim(),
            LastOrganisationName = txtLastOrganisation.Text.ToString().Trim(),
            AreaOfSpecialization = txtAreaOfSpecialization.Text.ToString().Trim(),
            Resume = msFileName,
		    IsActive = true
        };
        return oStudentInfo;
    }

    #endregion "Private Methods"
}