// Class Name    : RegistrationWizard_Step1.aspx.cs
// Modified By   : Amit
// Modified Date : 25 Sept 2009
// Descrption    : This class is used to get details of school.

using System;
using BusinessLogic;
using Utility;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;
using System.Globalization;

public partial class RegistrationWizard_Step1 :SchoolBase
{
    #region " Constants "

    private const string S_ERROR_MESSAGE_DUPLICATE_SCHOOL_NAME = "School name already exists. Please enter another school name.";
    private const string S_SCREENS_URL = "ScreensUI.aspx";
    private const string S_REG_STEP_1 = "RegistrationWizard_Step1.aspx";
    private const int I_FILE_SIZE_LIMIT = 30750;//nearly 20 kb
    private const int I_HEIGHT_LIMIT = 151;
    private const int I_WIDTH_LIMIT = 250;
    private const string S_UPLOAD_IMAGE_PATH_FOR_ID = "\\images\\IdentityCardImages\\";

    static string msURL = "";

    #endregion " Constants "

    #region " Event "

    /// <summary>
    /// This event is used to set master page file as per user role ie. Super Admin or School Admin.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);			
			
            string sUrl = GetSourceUrl();
            if (IsPostBack)
                sUrl = msURL;

			if (sUrl.Equals(S_SCREENS_URL) || sUrl.Equals(S_REG_STEP_1) && !(moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Admin ||moUserRole==Constants.UserRoles.Teacher ))
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
               ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display school information in edit mode and registered new school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sUrl = string.Empty;
            sUrl = GetSourceUrl();
            if (!IsPostBack)
            {
                msURL = sUrl;
                if (sUrl.Equals(S_SCREENS_URL))
                {
                    hidBackUrl.Value = "../SuperAdmin/ScreensUI.aspx";
                    if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
                        Session.Remove(Constants.S_SESSION_SCHOOL_ID);
                }
                SetDefaultProperties();
                SetAddEditModeDetails();
                SetTermsCheckBoxProperties();
                SetClientScriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }

                //Show cancel button when user redirect to this screen from ScreensUI.aspx otherwise keep it hidden
                if (hidBackUrl.Value != "../SuperAdmin/ScreensUI.aspx" && Session[Constants.S_SESSION_SCHOOL_ID] != null)
                {
                    imgBtnCancel.Visible = false;
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                imgBtnSubmit.Text = Resources.LocalizedResources.Save;
                valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    /// <summary>
    /// This is for button save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Set school details.
            SchoolBL oSchoolDetails = new SchoolBL();
            // If session is null that means the school registration is in process.
            if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
            {
                // Set the school details in the session and redirect to next page.
                Session["O_SCHOOL_DETAILS"] = oSchoolDetails;                
                Response.Redirect("../SuperAdmin/RegistrationWizard_Step2.aspx", false);
            }
            else
            {
                // Update the school details in the database and redirect to next page.
                oSchoolDetails.SchoolId = miSchoolId;
                oSchoolDetails.Address = txtAddress.Text.Trim();    
                if (UploadSign.HasFile)
                {
                    string sFileName = SaveFileOnServerForSign(UploadSign.FileName);
                    oSchoolDetails.SignLogo = S_UPLOAD_IMAGE_PATH_FOR_ID + sFileName;
                    Byte[] ImageBinaryData = base.GetByteArrayFromFileField(UploadSign);
                    oSchoolDetails.UpdatePrincipalSignatureLogo(ImageBinaryData);
                }
                else
                    oSchoolDetails.SignLogo = hidSignPath.Value;

                if (UploadICard.HasFile)
                {
                    string sFileName = SaveFileOnServerForICardLogo(UploadICard.FileName);
                    oSchoolDetails.ICardLogo = S_UPLOAD_IMAGE_PATH_FOR_ID + sFileName;
                    Byte[] ImageBinaryData = base.GetByteArrayFromFileField(UploadICard);
                    oSchoolDetails.UpdateICardLogo(ImageBinaryData); 
                }
                else
                    oSchoolDetails.ICardLogo = hidICardPath.Value;
                oSchoolDetails.UpdatePrincipalSignAndIcardDetails();
                SetAddEditModeDetails();
            }
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save school information in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // Set school details.
            SchoolBL oSchoolDetails = new SchoolBL();
            SetSchoolDetails(ref oSchoolDetails);
            ChangeSessionVariables(oSchoolDetails);
            // If session is null that means the school registration is in process.
            if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
            {
                // Set the school details in the session and redirect to next page.
                Session["O_SCHOOL_DETAILS"] = oSchoolDetails;                
                Response.Redirect("../SuperAdmin/RegistrationWizard_Step2.aspx", false);
            }
            else
            {
                // Update the school details in the database and redirect to next page.
                oSchoolDetails.SchoolId =miSchoolId;
                oSchoolDetails.UpdateSchoolInformation();
                if (UploadLogo.HasFile)
                {    
                    Byte[] ImageBinaryData =base.GetByteArrayFromFileField(UploadLogo);
                    oSchoolDetails.UpdateSchoolLogo(ImageBinaryData);
                }
                Response.Redirect("../Common/ControlPanel.aspx", false);
            }
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go to previous page as user acess.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidBackUrl.Value == "../SuperAdmin/ScreensUI.aspx")
            {
                // Set the school details in the session and redirect to next page.                
                Session[Constants.S_SESSION_SCHOOL_ID] = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
                Response.Redirect(hidBackUrl.Value, false);
            }
            else if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
            {
                //Set the school details in the session and redirect to next page.
                Response.Redirect("Home.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Method "

    /// <summary>
    /// This method is used to check duplicate school name validation.
    /// </summary>
    /// <param name="aoSchoolDetails"></param>
    /// <returns></returns>
    private bool CheckIfDuplicateSchoolName(SchoolBL aoSchoolDetails)
    {
        bool bDuplicateName = false;
        if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
        {
            bDuplicateName = aoSchoolDetails.CheckIfSchoolNameExists();
        }
        else
        {
            if (!txtSchoolName.Text.Trim().Equals(Session[Constants.S_SESSION_SCHOOL_NAME].ToString().Trim()))
                bDuplicateName = aoSchoolDetails.CheckIfSchoolNameExists();
            else
                bDuplicateName = false;
        }
        return bDuplicateName;
    }

    /// <summary>
    /// This method is used to check duplicate SMS sender name.
    /// </summary>
    /// <param name="aoSchoolDetails"></param>
    /// <returns></returns>
    private bool CheckIfDuplicateSenderName(SchoolBL aoSchoolDetails)
    {
        bool bDuplicateName = false;
        if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
        {
            bDuplicateName = aoSchoolDetails.CheckIfSMSSenderNameExists();
        }
        else
        {
            if (!txtSMSSenderName.Text.Trim().Equals(hidSMSSenderName.Value.Trim()))
                bDuplicateName = aoSchoolDetails.CheckIfSMSSenderNameExists();
            else
                bDuplicateName = false;
        }
        return bDuplicateName;
    }


    /// <summary>
    /// This method is used to populate SchoolBL object with school details. 
    /// </summary>
    /// <param name="aoSchoolDetails"></param>
    private void SetSchoolDetails(ref SchoolBL aoSchoolDetails)
    {
        bool bDuplicateName = false;

        aoSchoolDetails.SchoolName = txtSchoolName.Text;
        aoSchoolDetails.SMSSenderName = txtSMSSenderName.Text;
        aoSchoolDetails.SchoolId = miSchoolId;
        bDuplicateName = CheckIfDuplicateSchoolName(aoSchoolDetails);
        if (bDuplicateName)
            throw new Exception(Resources.LocalizedResources.ExceptionSchoolAlreadyExists);

        aoSchoolDetails.Address = txtAddress.Text;
        aoSchoolDetails.City = txtCity.Text;
        aoSchoolDetails.Pincode = txtPIN.Text;
        aoSchoolDetails.RegNo = txtRegNo.Text;
        aoSchoolDetails.StateName = txtState.Text;
        aoSchoolDetails.PhoneNumber = txtCPhone.Text.Trim();
        aoSchoolDetails.PhoneNumber2 = txtPhoneNo2.Text.Trim();
        if (calSinceDate.Text != "")
            aoSchoolDetails.SchoolSinceDate = Convert.ToDateTime(calSinceDate.Text);

        aoSchoolDetails.SchoolOrgnName = txtSchoolOrgn.Text;
        if (Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]) == 0)
        {
            aoSchoolDetails.InsertedBY = "";
            aoSchoolDetails.UpdatedBY = "";
        }
        else
        {
            aoSchoolDetails.InsertedBY = miUserId.ToString();
            aoSchoolDetails.UpdatedBY = miUserId.ToString();
        }
        aoSchoolDetails.UpdateDate = System.DateTime.Now;
        aoSchoolDetails.WebSite = txtWebSite.Text.Trim();
        aoSchoolDetails.FaxNumber = txtFaxNo.Text.Trim();
        aoSchoolDetails.Email = txtEmail.Text.Trim();       
        aoSchoolDetails.FeedbackEmail = txtEmails.Text.Trim();
        aoSchoolDetails.CareerEmails = txtCareerEmails.Text.Trim();
        aoSchoolDetails.ForgotPasswordEmails = txtForgotPassword.Text.Trim();
        aoSchoolDetails.Address1 = txtAddress1.Text.Trim();
        aoSchoolDetails.Address2 = txtAddress2.Text.Trim();
        aoSchoolDetails.AccountNo = txtAccountNo.Text.Trim();
        aoSchoolDetails.PTRegCertificateNo = txtPTRegCertificateNo.Text.Trim();
        aoSchoolDetails.SchoolRecNoPrimary = txtRecognitionNoPri.Text.Trim();
        aoSchoolDetails.SchoolRecNoSecondary = txtRecognitionNoSec.Text.Trim();
        aoSchoolDetails.IndexNo = txtIndexNo.Text.Trim();
        aoSchoolDetails.PanNo = txtPanNo.Text.Trim();
        aoSchoolDetails.TanNo = txtTanNo.Text.Trim();
        aoSchoolDetails.GSTIN = txtGSTIN.Text.Trim();
        aoSchoolDetails.UDISENumber = txtUDISENumber.Text.Trim();
        aoSchoolDetails.Lattitude = txtLattitude.Text.Trim();
        aoSchoolDetails.Longitude = txtLongitude.Text.Trim();

        if (UploadLogo.HasFile)
        {
            SaveFileOnServer();
            aoSchoolDetails.LogoPath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + Constants.S_SCHOOL_LOGO_FILE_NAME;            
        }
        else
            aoSchoolDetails.LogoPath = hidFilePath.Value;
    }
    /// <summary>
    /// this is for SaveFileOnServer
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private void SaveFileOnServer()
    {
        // Upload the file to the server.
        const int I_HEIGHT_LIMIT = 151;
        const int I_WIDTH_LIMIT = 250;
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;

        string sServerFilePath = sFolderName + Constants.S_SCHOOL_LOGO_FILE_NAME;
        if (File.Exists(sServerFilePath))
        {
            File.Delete(sServerFilePath);
        }
        UploadLogo.SaveAs(sServerFilePath);

        string sErrMessage = ValidateFile(sServerFilePath, I_HEIGHT_LIMIT, I_WIDTH_LIMIT,"School Logo");
        if (sErrMessage.Equals(""))
        {
            //delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidFilePath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMsg.Text = sErrMessage;
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
    }

    /// <summary>
    /// This method is used to fill all page controls with school details.
    /// </summary>
    private void DisplaySchoolDetails()
    {
        // This method creates object for the current school id 
        // and assigns the values in the respective controls on the page.
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        txtSchoolName.Text = oSchoolBL.SchoolName;
        txtSMSSenderName.Text = oSchoolBL.SMSSenderName;
        hidSMSSenderName.Value = oSchoolBL.SMSSenderName;
        txtAddress.Text = oSchoolBL.Address;
        txtCity.Text = oSchoolBL.City;
        txtPIN.Text = oSchoolBL.Pincode;
        txtRegNo.Text = oSchoolBL.RegNo;
        txtState.Text = oSchoolBL.StateName;
        txtCPhone.Text = oSchoolBL.PhoneNumber;
        txtPhoneNo2.Text = oSchoolBL.PhoneNumber2;
        cSinceDate.DateValue = oSchoolBL.SchoolSinceDate;
        txtSchoolOrgn.Text = oSchoolBL.SchoolOrgnName;
        txtFaxNo.Text = oSchoolBL.FaxNumber;
        txtWebSite.Text = oSchoolBL.WebSite;
        txtAddress1.Text = oSchoolBL.Address1;
        txtAddress2.Text = oSchoolBL.Address2;
        txtAccountNo.Text = oSchoolBL.AccountNo;
        txtPTRegCertificateNo.Text = oSchoolBL.PTRegCertificateNo;
        txtRecognitionNoPri.Text = oSchoolBL.SchoolRecNoPrimary;
        txtRecognitionNoSec.Text = oSchoolBL.SchoolRecNoSecondary;
        txtIndexNo.Text = oSchoolBL.IndexNo;
        txtEmail.Text = oSchoolBL.Email;
        txtEmails.Text = oSchoolBL.FeedbackEmail;
        txtCareerEmails.Text = oSchoolBL.CareerEmails;
        txtForgotPassword.Text = oSchoolBL.ForgotPasswordEmails;
        txtPanNo.Text = oSchoolBL.PanNo;
        txtTanNo.Text = oSchoolBL.TanNo;
        txtGSTIN.Text = oSchoolBL.GSTIN;
        txtUDISENumber.Text = oSchoolBL.UDISENumber;
        txtLattitude.Text = oSchoolBL.Lattitude;
        txtLongitude.Text = oSchoolBL.Longitude;
        imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.SchoolLogo.ToInt().ToString();
        hidFilePath.Value = oSchoolBL.LogoPath;
        imgLogoICard.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.ICardLogo.ToInt().ToString();
        imgSign.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.SignLogo.ToInt().ToString();
        hidSignPath.Value = oSchoolBL.SignLogo;
        hidICardPath.Value = oSchoolBL.ICardLogo;
    }

    /// <summary>
    /// This method is used to get referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetSourceUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method is used to change 'school name' session variable.
    /// </summary>
    /// <param name="aoSchoolDetails"></param>
    private void ChangeSessionVariables(SchoolBL aoSchoolDetails)
    {
        if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
        {
            Session[Constants.S_SESSION_SCHOOL_NAME] = aoSchoolDetails.SchoolName;
        }
    }

    /// <summary>
    /// This method is used to hide term checkbox if school is allready configured.
    /// </summary>
    private void SetTermsCheckBoxProperties()
    {
        if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
        {
            trTerms.Visible = false;
        }
    }

    ///<Summary>
    ///This method is used to set default properties of a controls.
    ///</Summary>
    private void SetDefaultProperties()
    {
		trSMSSender.Visible = trFeedback.Visible = trCareer.Visible = !Settings.IsMiniSite;
        txtSchoolName.Focus();
        imgBtnSubmit.Attributes["onclick"] = "javascript:DisableButtons(this)";
        btnSave.Attributes["onclick"] = "javascript:VisibleSuccessMsg(this)";
        imgBtnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        lnkTermsandConditions.Attributes.Add("onclick", "window.open('TermsandConditions.aspx' , '_new','scrollbars=yes,status=yes,menubar=no,location=no,resizable=no,top=0,left=0,width=800,height=520'); return false;");

        txtSchoolName.Attributes.Add("onkeypress", "return clickButton(event)");
        txtRegNo.Attributes.Add("onkeypress", "return clickButton(event)");
        txtAddress.Attributes.Add("onkeypress", "return clickButton(event)");
        txtCity.Attributes.Add("onkeypress", "return clickButton(event)");
        txtState.Attributes.Add("onkeypress", "return clickButton(event)");
        txtPIN.Attributes.Add("onkeypress", "return clickButton(event)");
        txtCPhone.Attributes.Add("onkeypress", "return clickButton(event)");
        txtSchoolOrgn.Attributes.Add("onkeypress", "return clickButton(event)");
        txtSMSSenderName.Attributes.Add("onkeypress", "return clickButton(event)");
        calSinceDate.Attributes.Add("onkeypress", "return clickButton(event)");
        lnkSchoolAccountDetails.Attributes.Add("onclick", "window.open('../Admin/SchoolBankAccountDetailsPopUp.aspx" +
                            "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370'); return false;");
		chkTermsAndConditions.Attributes.Add("onclick", "fnChkboxClick();");

    }

    /// <summary>
    /// This method is used to set add new school or edit school information depends on session school id.
    /// </summary>
    private void SetAddEditModeDetails()
    {
        if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
        {
            DisplaySchoolDetails();
            imgBtnSubmit.Text = Resources.LocalizedResources.Save;
            trHeading.Visible = false;
        }
        else
        {
            imgBtnSubmit.Text = Resources.LocalizedResources.Next;
            txtCity.Text = Constants.S_DEFAULT_CITY;
            txtState.Text = Constants.S_DEFAULT_STATE;
        }
        trAccount.Visible = false;
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { imgBtnSubmit, imgBtnCancel, btnSave });
    }

    /// <summary>
    /// this is ValidateFile
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <param name="aiHeight"></param>
    /// <param name="aiWidth"></param>
    /// <param name="asFileUploadName"></param>
    /// <returns></returns>
    private string ValidateFile(string asServerFilePath,int aiHeight,int aiWidth,string asFileUploadName)
    {
        string sReturnErrorMsg = "";
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > aiHeight && oImg.Width > aiWidth)
                sReturnErrorMsg = Resources.LocalizedResources.HigthAndWidthFilePhoto + asFileUploadName + Resources.LocalizedResources.HeightAndWidthRespectively + aiHeight + "px" + Resources.LocalizedResources.And + aiWidth + "px" + Resources.LocalizedResources.Respectively1;
            else
            {
                if (oImg.Height > aiHeight)
                    sReturnErrorMsg = Resources.LocalizedResources.HeightOfPhotoFile  + asFileUploadName + Resources.LocalizedResources.ShouldNotExceed1 + aiHeight + "px." + Resources.LocalizedResources.NotExceed;
                if (oImg.Width > aiWidth)
                    sReturnErrorMsg = Resources.LocalizedResources.WidthOfPhotoFile  + asFileUploadName + Resources.LocalizedResources.ShouldNotExceed + aiWidth + "px." + Resources.LocalizedResources.NotExceed;
            }
            oFileStream.Close();
            oImg = null;

        }
        return sReturnErrorMsg;
    }

    private string SaveFileOnServerForSign(string asFileName)
    {
        // Upload the file to the server.
        const int I_HEIGHT_LIMIT = 30;
        const int I_WIDTH_LIMIT = 77;
        string sFolderName = Server.MapPath("..") + S_UPLOAD_IMAGE_PATH_FOR_ID;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName,"Principal Sign");
            sServerFilePath = sFolderName + sFileName;
        }
        UploadSign.SaveAs(sServerFilePath);
        string sErrMessage = ValidateFile(sServerFilePath, I_HEIGHT_LIMIT, I_WIDTH_LIMIT, "Principal's Sign");
        if (sErrMessage.Equals(""))
        {
            //delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidSignPath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMsg.Text = sErrMessage;
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
        return sFileName;

    }

    private string SaveFileOnServerForICardLogo(string asFileName)
    {
        // Upload the file to the server.
        //const int I_HEIGHT_LIMIT = 50;
        //const int I_WIDTH_LIMIT = 72;
        string sFolderName = Server.MapPath("..") + S_UPLOAD_IMAGE_PATH_FOR_ID;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName, "IcardLogo" );
            sServerFilePath = sFolderName + sFileName;
        }
        UploadICard.SaveAs(sServerFilePath);
        string sErrMessage = string.Empty;//ValidateFile(sServerFilePath, I_HEIGHT_LIMIT, I_WIDTH_LIMIT, "ICard Logo");
        if (sErrMessage.Equals(""))
        {
            //delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidICardPath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMsg.Text = sErrMessage;
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
        return sFileName;
    }

    private void RefreshValue()
    {
        hidInvalidFileFormat.Value  = Resources.LocalizedResources.InvalidFileFormat;
        hidValBITMapFileFormat.Value = Resources.LocalizedResources.ValBITMapFileFormat;
        hidSchoolLogoBlank.Value = Resources.LocalizedResources.SchoolLogoBlank;
        hidCareerEmailAddLength.Value = Resources.LocalizedResources.CareerEmailAddLength;
        hidValSinceDate.Value = Resources.LocalizedResources.ValSinceDate;
        hidPinCodeDigit.Value = Resources.LocalizedResources.PinCodeDigit;
        hidSchoolSinceDateFutureDate.Value = Resources.LocalizedResources.SchoolSinceDateFutureDate;
        hidValEmailAddForFeedbackLength.Value = Resources.LocalizedResources.ValEmailAddForFeedbackLength;
        hidValFeedBackEmailAdd.Value = Resources.LocalizedResources.ValFeedBackEmailAdd;
        hidValEmailAddValid.Value = Resources.LocalizedResources.ValEmailAddValid;
        hidAreDuplicated.Value = Resources.LocalizedResources.AreDuplicated;
        hidvalCareerEmailAddress.Value = Resources.LocalizedResources.valCareerEmailAddress;
        hidvalForgotPassEmails.Value = Resources.LocalizedResources.valForgotPassEmails;
        hidForgotPasswordEmailLength.Value = Resources.LocalizedResources.ForgotPasswordEmailLength;
    }

    #endregion
}
