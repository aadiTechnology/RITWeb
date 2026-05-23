/*  This Class is used to   
 *  - UI and functional validation of student's basic information.
 *  - insert, update student's basic information.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using DocumentEntity;
using PhotoUploadEntities;
using SchoolAutoSearchService.Service;
using SchoolEntities;
using StudentEntities;
using Utility;
using BusinessLogic.TransportBL;


public partial class StudentUI : SchoolBase
{


    #region Constants

    private const int I_FILE_SIZE_LIMIT = 1048576; // nearly 1 mb
    private const int I_HEIGHT_LIMIT = 151;
    private const int I_WIDTH_LIMIT = 112;
    private const int I_SMS_TEMPLATE_TXT = 2;
    private const int I_SMS_SUBJECT_TXT = 1;
    private const int I_SMS_TYPE = 3;
    private const int I_MASTER = 5;
    private const int I_OTHER_OCCUPATION_ID = 5;
    private const int I_MISS = 6;

    private const char C_FEMALE = 'F';
    private const char C_MALE = 'M';
    private const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    private const string S_REPLACE_URL = "http://";
    const int I_REGNOPOSTFIX_COLUMN_INDEX = 7;
    const string S_DUPLICATE_REG_NO = "RegNoException";
    const string S_DUPLICATE_FORM_NO = "FormNumberException";
    const string S_DUPLICATE_ROLL_NO = "RollNoException";
    const string S_DUPLICATE_STUDENT_UNIQUE_NO = "StudentUniqueNoException";
    const string S_DUPLICATE_GENERAL_REG_NO = "GeneralRegNoException";


    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Aadhar Cards\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Aadhar Cards/";


    private const string S_Family_Photo = "RITeSchool\\DOWNLOADS\\Family Photos\\";
    private const string S_Family_Path = @"../DOWNLOADS/Family Photos/";

    private const string S_CastCertificate_Photo = "RITeSchool\\Downloads\\Admission\\CasteCertificate\\";
    private const string S_CastCertificate_PhotoPath = @"../Downloads/Admission/CasteCertificate/";

    private const string S_Parent_Photo = "RITeSchool\\DOWNLOADS\\Parent Photos\\";
    private const string S_Parent_Path = @"../DOWNLOADS/Parent Photos/";

    private const string S_Parent_AadharCardPhoto = "RITeSchool\\DOWNLOADS\\ParentAadharCards\\";
    private const string S_Parent_AadharCardPath = @"../DOWNLOADS/ParentAadharCards/";

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #endregion Constants

    #region Datamembers

    private StudentBL moStudentBL;
    private string msQueryString;
    private SchoolUserBL moSchoolUserBL;
    private int miCurrentAcademicYearId;
    private StudentAdditionalDetails moStudentAdditionalDetails;
    
    #endregion  Datamembers

    #region Event handler

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            SetAdditionalFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to intialize page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)    
    {
        try
        {
            string s = hidIsSiblingAdded.Value;
            SetCurrentDate();
            if (!IsPostBack)
            {
                RenameHeaders();
                if (miSchoolId == Constants.SchoolId.SNS.ToInt())////
                {
                    colpnlStudentSubjectDetails.Visible = true;
                    FillStreamCombo();
                    FillGroupCombo();  //
                    FillCompulsarySubjects();  //
                }
                SetAcademicYearValue();
                SetStudentIdWithGRNumber();

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
               
                if (CheckPreCondition())
                {
                    RefreshValue();
                    //chkIsStaffKid.Checked = false;
                    //ddlFeeRule.Enabled = false;
                    ddlFeeRule.SelectedIndex = 0;
                    Initialize();
                    fillsiblingslistview();
                    hidAcademicYearStatus.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS]);
                    btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
                    hidServerDate.Value = DateTime.Today.ToString("dd-MMM-yyyy", new CultureInfo("en"));
                    SetAcademicYearDates();
                    string sQueryString = "UserId=" + hidStudentId.Value;
                    ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
                    btnSave.Attributes["onclick"] = "if(!confirmDOB()) return false;";                    
                    btnSaveNext.Attributes["onclick"] = "if(!confirmDOB()) return false;";
                    SetAchievementLink();
                }
                else
                    fillsiblingslistview();

                //ddlSecondLanguage.Attributes.Add("onchange", "ChangeSecondAndThirdLanguage(" + Constants.I_ONE + "); return false;");
                //cmbThirdLanguage.Attributes.Add("onchange", "ChangeSecondAndThirdLanguage(" + Constants.I_TWO + "); return false;");
            }
            if (hidStudentSiblingNames.Value != string.Empty)
            {
                lblStudentSiblingName.Text = hidStudentSiblingNames.Value;
            }
            SetMandatoryFields();
            ApplyMouseHoverEffect(new List<Button> { btnCancel, btnRemovePhoto, btnSave, btnSaveNext, btnAddSiblingDetails, btnClear, btnAddAchievement });
            chkIsStaffKid.Attributes.Add("Onclick", "EnableDisabledCmb()");
            btnClear.Attributes.Add("Onclick", "ResetListViewCheckBoxes()");
            btnSavePopUp.Attributes.Add("Onclick", "if(!ConfirmAction())return false;");
            btnRemovePhoto.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
            SetQueryStringToAdd();
            if (QueryString["NewMode"] != null && QueryString["NewMode"] == Constants.S_YES)
                btnClear.Visible = true;
           
          
            if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            {
                if ((Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null) &&
                 (Convert.ToChar(Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED]) == Constants.C_YES) && (Convert.ToChar(Session[Constants.S_SESSION_IS_FINALYEAR_GENERATED]) == Constants.C_NO))
                    chkSendSMS.Enabled = false;
            }
            

            //// this is because we change value of hindden field value in javascript again replacing value to original
            //// this change required because if we dont change value hidden field hidden field event is not get fired
            //// when we adding same subling subling for next time. 
            if (hidSiblingStudentId.Value != string.Empty)
            {
                var iSublingId = hidSiblingStudentId.Value;
                hidSiblingStudentId.Value = iSublingId.Replace(",,", ",");
            }

            if (!QueryString["IsStudntDtailsScrn"].IsNullOrEmpty() || (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Student).ToString() == Constants.S_NO && moUserRole != Constants.UserRoles.Admin && !MasterDataCollectionBL.IsClassTeacher(miSchoolId, miAcademicYearId, miUserId)))
            {
                btnAddSiblingDetails.Enabled = false;
                btnSave.Enabled = false;                
                btnRemovePhoto.Enabled = false;
                btnAddAchievement.Enabled = false;
            }

            if (SchoolBase.Settings.ShowDayBoardingOptionOnStudentsScreen)
            {
                chkIsDayBoarding.Attributes.Add("Onclick", "CheckIsDayBoardingFeePaid();");
                trIsForDayBoarding.Visible = true;
            }
            else
                trIsForDayBoarding.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// According to New or Edit mode data is updated
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int iTrackingId = 0;
                if (hidMode.Value.Equals("new"))
                    AddStudent();
                else
                    iTrackingId = UpdateStudent();

                if (hidIsConfig.Value != "Y")
                    SaveConfigDetails(Constants.SchoolConfigurations.Student.ToInt());
                SaveDocumentDetails();

                if (iTrackingId != 0)
                    moStudentBL.UpdateStudentTrackingDetails(miSchoolId, miUserId, moStudentBL.StudentId, iTrackingId, miAcademicYearId);

                if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
                {
                    string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
                    string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
                    TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                    oTransferTransportDetailsBL.UpdateRFIDDetails(moStudentBL.UserId);
                }

                RefreshStudentCache();
                if (QueryString["AccessModeFromFee"].IsNullOrEmpty() && QueryString["IsDirectSearch"].IsNullOrEmpty())
                {
                    string sUrl = Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + HidBackUrl.Value;

                    switch (moUserRole)
                    {
                        case Constants.UserRoles.Supervisor:
                        case Constants.UserRoles.Admin:
                            {
                                PopupMaster oMasterPage = (PopupMaster)this.Master;
                                oMasterPage.RedirectToNextPage(sUrl);
                            }
                            break;
                        case Constants.UserRoles.Teacher:
                            if (Boolean.Parse(hidUserHasFullAccess.Value))
                            {
                                PopupMaster oMasterPage = (PopupMaster)this.Master;
                                oMasterPage.RedirectToNextPage(sUrl);
                            }
                            else
                            {
                                if (hidMode.Value == "EDIT")
                                {
                                    string sQueryString = "'?" + HidBackUrl.Value + "'";
                                    Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); window.close();</script>");
                                }
                                else
                                {
                                    PopupMaster oMasterPage = (PopupMaster)this.Master;
                                    oMasterPage.RedirectToNextPage(sUrl);
                                }
                            }
                            break;
                    }
                }
                else
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
            }
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (BusinessLogic.DuplicateUserException oEx)
        {
            //  lblErrorMsg.Text = oresource.gets(oEx.Message.Replace(" ", string.Empty));
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
            // this is to clear session image data captured web cam.
            //this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (DuplicateRegisterNumberExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
            //lblErrorMsg.Text = oEx.Message;
            // this is to clear session image data captured web cam.
            //this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (DuplicateRollNumberExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
            // this is to clear session image data captured web cam.
            //this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }        
        catch (DuplicateGeneralRegisterNumberExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
            
        }
        catch (DuplicateStudentUniqueNoExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));

        }
        catch (ApplicationException oEx)
        {
            lblErrorMsg.Text = oEx.Message;
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (ReferenceExceptions oEx)
        {
            lblErrorMsg.Text = oEx.Message;
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }

    }

    /// <summary>
    /// this for btn remove photo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemovePhoto_Click(object sender, EventArgs e)
    {
        try
        {
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            int iStudentId = Convert.ToInt32(hidStudentId.Value);
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.RemoveStudentPhoto(iStudentId, miSchoolId);
            BindEntryFields(miSchoolId, iStudentId, miCurrentAcademicYearId);
            btnRemovePhoto.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is fired when user clicks on next button.
    /// it adds the student and returns back to itself to let user enter information for next studdent
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveNext_Click(object sender, EventArgs e)
    {
        try
        {

            string str = (Request.Url.ToString());
            AddStudent();
            SaveDocumentDetails();
            RefreshStudentCache();
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            PopupMaster oMasterPage = (PopupMaster)this.Master;
            oMasterPage.RedirectToNextPage(str);
        }
        catch (BusinessLogic.DuplicateUserException oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
        }
        catch (DuplicateRegisterNumberExceptions ex)
        {
            lblErrorMsg.Text = oResourceManager.GetString(ex.Message.Replace(" ", string.Empty));
        }
        catch (ApplicationException oEx)
        {
            lblErrorMsg.Text = oEx.Message;
        }
        catch (ReferenceExceptions oEx)
        {
            lblErrorMsg.Text = oEx.Message;
        }
        catch (DuplicateRollNumberExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
        }
        catch (DuplicateGeneralRegisterNumberExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
        }
        catch (DuplicateStudentUniqueNoExceptions oEx)
        {
            lblErrorMsg.Text = oResourceManager.GetString(oEx.Message.Replace(" ", string.Empty));
        }
        catch (Exception oEx)
        {

            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStaffs();
        }
        catch (Exception oEx)
        {

            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param nam e="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {

        this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        chkNewAddmission.Checked = true;
        chkIsRTEApplicable.Checked = false;
        txtFormNo.Text = string.Empty;
        txtStudentID.Text = string.Empty;
        txtGRNumber.Text = string.Empty;
         txtRegNo.Text = string.Empty;
        txtcalAdmissionDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        txtJoiningDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        txtRollNumber.Text = hidDefaultRollNo.Value.ToString();
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtMotherName.Text = string.Empty;
        txtParentName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtBirthPlace.Text = string.Empty;
        txtNationality.Text = string.Empty;
        txtResPhoneNumber.Text = string.Empty;
        txtCasteAndSubcaste.Text = string.Empty;
        cmbCategory.ClearSelection();
        ListItem oListItem = cmbCategory.Items.FindByText("Not Available");
        oListItem.Selected = true;
        //chkIsStaffKid.Checked = false;
        ddlSecondLanguage.ClearSelection();
        cmbThirdLanguage.ClearSelection();
        ddlUserRole.ClearSelection();
        ddlUserName.ClearSelection();
        imgPhoto.Src = S_DEFAULT_PHOTO;
        txtCalDobPopup.Text = string.Empty; //DateTime.Today.ToString("dd-MMM-yyyy");
        cmbBloodGroup.ClearSelection();
        rdoMale.Checked = true;
        cmbOcupation.ClearSelection();
        txtOtherOccupation.Text = string.Empty;
        txtCity.Text = Constants.S_DEFAULT_CITY;
        txtState.Text = Settings.DefaultStudentState;
        txtPIN.Text = string.Empty;
        txtMobilePhoneNumber.Text = string.Empty;
        txtMobilePhoneNumber2.Text = string.Empty;
        txtOfficeNo.Text = string.Empty;
        txtNeighbourNo.Text = string.Empty;
        txtAdditionalReligion.Text = string.Empty;////////////////////////////////
        txtMotherTongue.Text = string.Empty;
        ddlFeeRule.ClearSelection();
        chkHasSibling.Checked = false;
        txtLastSchoolName.Text = string.Empty;
        txtLastSchoolAddress.Text = string.Empty;
        txtLastStandard.Text = string.Empty;
        txtLastUDISENo.Text = string.Empty;
        rdolstlastSchoolBoard.ClearSelection();
        rdobtnRecognisedYes.Checked = true;
        rdobtnRecognisedNo.Checked = false;
        txtEmail.Text = string.Empty;
        txtAadharCardNo.Text = string.Empty;
        txtNameOnAadharCard.Text = string.Empty;
        hidOldFeeAreaId.Value = Constants.S_ZERO;
        txtSaralNo.Text = string.Empty;
        txtPenNo.Text = string.Empty;
    }

    /// <summary>
    /// this for button cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            if (QueryString["AccessModeFromFee"].IsNullOrEmpty() && QueryString["IsStudntDtailsScrn"].IsNullOrEmpty() && QueryString["IsDirectSearch"].IsNullOrEmpty())
            {
                string sUrl = Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + HidBackUrl.Value;

                if (moUserRole == Constants.UserRoles.Admin
                    || moUserRole == Constants.UserRoles.Supervisor)
                {
                    PopupMaster oMasterPage = (PopupMaster)this.Master;
                    oMasterPage.RedirectToNextPage(sUrl);
                }
                else if (moUserRole == Constants.UserRoles.Teacher)
                {

                    if (Boolean.Parse(hidUserHasFullAccess.Value))
                    {
                        PopupMaster oMasterPage = (PopupMaster)this.Master;
                        oMasterPage.RedirectToNextPage(sUrl);
                    }
                    else
                    {
                        if (hidMode.Value == "EDIT")
                        {
                            string sQueryString = "'?" + HidBackUrl.Value + "'";
                            Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); window.close();</script>");
                        }
                        else
                        {
                            PopupMaster oMasterPage = (PopupMaster)this.Master;
                            oMasterPage.RedirectToNextPage(sUrl);
                        }
                    }
                }
            }
            else
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// this method for ocupation combox changed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbOcupation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbOcupation.SelectedItem.Text.ToUpper().Equals("OTHER"))
            {
                trOtherOccupation.Visible = true;
                txtOtherOccupation.Focus();
            }
            else
            {
                cmbOcupation.Focus();
                trOtherOccupation.Visible = false;
                txtOtherOccupation.Text = string.Empty;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to hid siblings of student id value changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidSiblingStudentId_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (hidOverwrite.Value == Constants.S_YES)
            {
                if (string.IsNullOrEmpty(hidSiblingStudentId.Value))
                {
                    if (IsPostBack && hidMode.Value == Constants.S_NEW_MODE.ToLower())
                    {
                        txtAddress.Text = txtCasteAndSubcaste.Text = txtCity.Text = txtLastName.Text = txtMiddleName.Text = txtMobilePhoneNumber.Text = txtState.Text = txtResPhoneNumber.Text = string.Empty;
                        txtMobilePhoneNumber2.Text = txtMotherName.Text = txtNeighbourNo.Text = txtAdditionalReligion.Text = txtOfficeNo.Text = txtOtherOccupation.Text = txtPIN.Text = txtParentName.Text = txtAadharCardNo.Text = string.Empty;
                        txtNameOnAadharCard.Text = string.Empty;

                        cmbOcupation.ClearSelection();
                        cmbCategory.ClearSelection();
                    }
                }
                else if (!hidSchoolwiseStudentId.Value.IsNullOrEmpty())
                    BindEntryFields(miSchoolId, Convert.ToInt32(hidSchoolwiseStudentId.Value), miAcademicYearId);
            }

        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to bound data to listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguredDocument_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            Label oLblDocName = oCurrentItem.FindControl("lblDocumentName") as Label;
            CheckBox oChkIsSubmitted = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            CheckBox oChkIsApplicable = oCurrentItem.FindControl("chkIsApplicable") as CheckBox;            
            oChkIsApplicable.Attributes["onclick"] = "javascript:SetIsApplicableSatus(this, " + iRowId + " );";
            oChkIsSubmitted.Attributes["onclick"] = "javascript:SetIsSubmittedSatus(this, " + iRowId + " );";
            oChkIsSubmitted.Checked = Convert.ToBoolean(lstvwConfiguredDocument.DataKeys[iRowId]["IsSubmitted"]);
            oChkIsApplicable.Checked = Convert.ToBoolean(lstvwConfiguredDocument.DataKeys[iRowId]["IsApplicable"]);
            LinkButton oLinkButton = e.Item.FindControl("lnkAttachment") as LinkButton;
            string sQueryString = string.Empty;

            HiddenField oHiddenField = e.Item.FindControl("hidIsDocMandatory") as HiddenField;            
            oHiddenField.Value = Convert.ToString(lstvwConfiguredDocument.DataKeys[iRowId]["IsSubmissionMandatory"]);

            if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && oHiddenField.Value == "True")
            {
                HtmlTableCell tdMandatoryDoc = e.Item.FindControl("tdDocumentName") as HtmlTableCell;
                HtmlTableCell tdlnkAttachment = e.Item.FindControl("tdlnkAttachment") as HtmlTableCell;
                HtmlTableCell tdSelect = e.Item.FindControl("tdSelect") as HtmlTableCell;
                HtmlTableCell tdIsApplicable = e.Item.FindControl("tdIsApplicable") as HtmlTableCell;

                if (tdMandatoryDoc != null && tdlnkAttachment != null && tdSelect != null && tdIsApplicable != null)
                {
                    LegendTable.Visible = true;
                    tdMandatoryDoc.BgColor = "#ffffcc";
                    tdlnkAttachment.BgColor = "#ffffcc";
                    tdSelect.BgColor = "#ffffcc";
                    tdIsApplicable.BgColor = "#ffffcc";
                }
            }            

            if (hidMode.Value == "EDIT")
            {
                StudentBL oStudentBL = new StudentBL(hidYearWiseStudentId.Value.ToInt());
                int iStandardwiseDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[e.Item.DisplayIndex]["StandardwiseDocumentId"]);
                sQueryString = "UserId=" + oStudentBL.UserId +
                                      "&DocumentId=" + iStandardwiseDocumentId +
                                    "&DocumentTypeId=" + Constants.DocumentTypes.StudentDocuments.ToInt();

                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                oLinkButton.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
            }
            else
                oLinkButton.Attributes.Add("onclick", "OpenPopup1(); return false;");
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update investment declaration listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void HidItemCount_ValueChanged(object sender, EventArgs e)
    {
        try
        {

            int iStandardwiseDocumentId = 0;
            const int I_DOCUMENT_COUNT = 0;
            const int I_STANDARDWISE_DOCUMENT_ID = 1;
            const int I_USER_ID = 2;
            StudentBL oStudentBL = new StudentBL(hidYearWiseStudentId.Value.ToInt());
            string[] sArrayIds = hidItemCount.Value.Split('$');
            if (sArrayIds[I_DOCUMENT_COUNT] != string.Empty && sArrayIds[I_USER_ID] == oStudentBL.UserId.ToString())
            {
                foreach (ListViewDataItem oCurrentItem in lstvwConfiguredDocument.Items)
                {
                    iStandardwiseDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[oCurrentItem.DisplayIndex]["StandardwiseDocumentId"]);
                    if (iStandardwiseDocumentId == sArrayIds[I_STANDARDWISE_DOCUMENT_ID].ToInt())
                    {
                        LinkButton lnkAttachment = oCurrentItem.FindControl("lnkAttachment") as LinkButton;
                        lnkAttachment.Text = sArrayIds[I_DOCUMENT_COUNT];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to bound data to listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSiblingsDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                (e.Item.FindControl("ChkSelectSiblingsSingle") as CheckBox).Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to delete family photo of perticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteFamilyPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidFamilyImage.Value = string.Empty;
            btnView1.Visible = false;
            imgbtnDelete.Visible = false;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to delete father photo of perticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDeleteFatherPhoto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteFatherPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidFatherPhoto.Value = string.Empty;
            imgDeleteFatherPhoto.Visible = false;
            imgViewFatherPhoto.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to delete mother photo of perticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDeleteMotherPhoto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteMotherPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidMotherPhoto.Value = string.Empty;
            imgDeleteMotherPhoto.Visible = false;
            imgViewMotherPhoto.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to delete Guardian photo of perticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDeleteGuardianPhoto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteGuardianPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidGuardianPhoto.Value = string.Empty;
            imgDeleteGuardianPhoto.Visible = false;
            imgViewGuardianPhoto.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    ///This function is used to delete Caste certificate photo of perticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDeleteCasteCert_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteCasteCertificatePhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidCasteCertImage.Value = string.Empty;
            imgbtnDeleteCasteCert.Visible = false;
            imgbtnViewCasteCert.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///This function is used to delete Mother Aadhar Card photo of perticular student
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDeleteMotherAadharCard_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteMotherAadharPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidMotherAadharCardFileName.Value = string.Empty;
            imgDeleteMotherAadharCard.Visible = false;
            imgViewMotherAadharCard.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///This function is used to delete Father Aadhar Card photo of perticular student
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDeleteFatherAadharCard_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            oStudentBL.DeleteFatherAadharPhoto(hidStudentId.Value.ToInt(), miSchoolId, miUserId);
            hidFatherAadharCardFileName.Value = string.Empty;
            imgDeleteFatherAadharCard.Visible = false;
            imgViewFatherAadharCard.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This function is used to get groups .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)  ////
    {
        
        FillGroupCombo();
        FillCompulsarySubjects();//
    }

    /// <summary>
    /// This function is used to get Compulsary subjects of groups .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
       FillCompulsarySubjects();
    }


    protected void FatherAadharFile_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;
        if (flUploadFatherAaadhar.FileName != string.Empty)
        {

            string extension = Path.GetExtension(flUploadFatherAaadhar.FileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

            if (!allowedExtensions.Contains(extension))
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Please select valid file type for Father's Aadhar Card1.";
                return;
            }

            // Max size: 1MB = 1 * 1024 * 1024 bytes
            if (flUploadFatherAaadhar.PostedFile.ContentLength > 1048576)
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Size of Father's Aadhar Card file should not be more than 1 mb.";
                return;
            }
        }
            args.IsValid = true;
        
    }

    protected void MotherAadharFile_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;
        if (flUploadMotherAaadhar.FileName != string.Empty)
        {
          
            string extension = Path.GetExtension(flUploadMotherAaadhar.FileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

            if (!allowedExtensions.Contains(extension))
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Please select valid file type for Mother's Aadhar Card1.";
                return;
            }

            // Max size: 1MB = 1 * 1024 * 1024 bytes
            if (flUploadMotherAaadhar.PostedFile.ContentLength > 1048576)
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Size of Mother's Aadhar Card file should not be more than 1 mb.";
                return;
            }
         }
            args.IsValid = true;
       }

    protected void CasteCertFIle_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;

        if (fuCastCertificate.FileName != string.Empty)
        {
            string extension = Path.GetExtension(fuCastCertificate.FileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

            if (!allowedExtensions.Contains(extension))
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Please select valid file type for Caste Certificate.";
                return;
            }

            // Max size: 1MB = 1 * 1024 * 1024 bytes
            if (fuCastCertificate.PostedFile.ContentLength > 1048576)
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Size of Caste Certificate file should not be more than 1 mb.";
                return;
            }
        }

        args.IsValid = true;
    }
    #endregion event handler

    #region Private Methods

    /// <summary>
    /// This method initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetAcademicYearDates()
    {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        DataTable oDT;
        oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId, miAcademicYearId, iStandardId);
        if (oDT.Rows.Count > 0)
        {
            hidAcademicStartDate.Value = Convert.ToDateTime(oDT.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy", new CultureInfo("en"));
            hidAcademicEndDate.Value = Convert.ToDateTime(oDT.Rows[0]["EndDate"]).ToString("dd-MMM-yyyy", new CultureInfo("en"));
        }
        else
        {
            hidAcademicStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
            hidAcademicEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
        }
    }

    /// <summary>
    /// This method is used to save student submitted and not submitted document details.
    /// </summary>
    private void SaveDocumentDetails()
    {
        int iYrStudId = Convert.ToInt32(hidYearWiseStudentId.Value);
        int iSchoolId = miSchoolId;
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(iSchoolId);
        oStandardwiseDocumentMasterBL.SaveSubmittedDocuments(GenerateXml(PopulateDocumentDetails()), iYrStudId, miUserId);
    }

    /// <summary>
    /// This method is used to populate document details
    /// </summary>
    /// <returns></returns>
    private List<StudentDocument> PopulateDocumentDetails()
    {
        List<StudentDocument> lstDocumentInfo = new List<StudentDocument>();
        StudentDocument oStudentDocument = null;

        for (int iRowCount = 0; iRowCount < lstvwConfiguredDocument.Items.Count; iRowCount++)
        {
            oStudentDocument = new StudentDocument();
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwConfiguredDocument.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iStudentDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[iRowId]["StudentDocumentId"]);
            int iStandardwiseDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[iRowId]["StandardwiseDocumentId"]);
            CheckBox oChkIsSubmitted = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            CheckBox oChkIsApplicable = oCurrentItem.FindControl("chkIsApplicable") as CheckBox;
            Label oLblDocName = oCurrentItem.FindControl("lblDocumentName") as Label;

            oStudentDocument.StudentDocumentId = iStudentDocumentId;
            oStudentDocument.StandardwiseDocumentId = iStandardwiseDocumentId;
            oStudentDocument.IsSubmitted = oChkIsSubmitted.Checked;
            oStudentDocument.IsApplicable = oChkIsApplicable.Checked;
            lstDocumentInfo.Add(oStudentDocument);

        }
        return lstDocumentInfo;
    }

    /// <summary>
    /// this method sets url for student list page
    /// </summary>
    /// <returns></returns>
    private string GetURL()
    {
        msQueryString = "StandardId=" + hidStandardId.Value
                                     + "&DivisionId=" + hidDivisionId.Value
                                  + "&NewMode=" + Constants.C_YES
                                  + "&ClassId=" + Convert.ToInt32(hidClassId.Value);
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQueryString);
        return Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + sEncrypt;

    }

    /// <summary>
    /// this method is used to add new student
    /// </summary>

    private void AddStudent()
    {
        string sLinkName;
        string sFileUploadErr = CheckIsFileFileUploaded(out sLinkName);

		 string sLinkNameForFamilyPhoto;
         string sFileUploadError = CheckIsFamilyPhotoUploaded(out sLinkNameForFamilyPhoto);
         string sLinkNameForCasteCert = CheckIsCasteCertificateUploaded();

        PopulateUserInformation();
        PopulateStudentStruct(sLinkName, sLinkNameForFamilyPhoto, sLinkNameForCasteCert);
        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && hisShowStreamSection.Value == Constants.S_ONE)////
        {
            PopulateStudentStreamwiseSubjects();
        }
            if (!CheckIfRollNoAllReadyAssigned())
            {
                if (!moStudentBL.isRegisterNoAlreadyPresent())
                {
                     if (!moStudentBL.isGeneralRegisterNoAlreadyPresent())
                      {
                          if (!moStudentBL.isStudentUniqueNoAlreadyPresent())
                          {
                    if (string.IsNullOrEmpty(moStudentBL.sFormNo.Trim()) || !CheckIfFormNoAllReadyAssigned(Constants.I_ZERO))
                    {
                        Int32 iUserID = moSchoolUserBL.InsertSchoolUserDetails();
                        if (iUserID != Constants.I_ZERO)
                        {
                            moStudentBL.UserId = iUserID;
                            moSchoolUserBL.UserId = iUserID;
                            moStudentBL.StudentUserBL = moSchoolUserBL;
                            moStudentBL.sFormNo = txtFormNo.Text;
                            moStudentBL.GRNumber = txtGRNumber.Text.Trim();
                            moStudentBL.StudentUniqueNo = txtStudentID.Text.Trim();
                            if (FileUploadLogo.HasFile)
                                moStudentBL.PhotoFilePathInBinary = GetByteArrayFromFileField(FileUploadLogo);
                            else
                            {
                                if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
                                {
                                    List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
                                    var oImage = lstImageData.Where(lst => lst.UserID == 0).LastOrDefault();
                                    if (!oImage.IsNull())
                                        moStudentBL.PhotoFilePathInBinary = oImage.ImagesData;
                                }
                            }

                            int iStudentId = moStudentBL.InsertStudent(hidSiblingStudentId.Value);
                            StudentBL oTempStudentBL = new StudentBL(iStudentId);
                            int iSchoolwiseStudentId = oTempStudentBL.StudentId;

                            if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                                PopulateAdditionalDetails(iSchoolwiseStudentId);

                            if (SchoolBase.Settings.IsAdditionalFieldsApplicable==true)
                            {
                                moStudentBL.AddStudentAdditionalDetails(miSchoolId, miUserId, moStudentAdditionalDetails);
                                moStudentBL.GenerateTrasnportFeeEntry(miSchoolId, miAcademicYearId, iSchoolwiseStudentId);
                            }

                            if (moStudentBL.IsRTEStudent)
                            {
                                StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                                string sReceiptNumber = oStudentFeeDetailsBL.AddConcessionForRTEStudent(iStudentId, miSchoolId, miAcademicYearId);

                                // Create a fee voucher for the fee concession for RTE student.
                                if (Settings.EnableAccountsModule)
                                {
                                    Accounts oAccounts = new Accounts();
                                    oAccounts.RecordCashPaymentForFeeConcession(iStudentId, sReceiptNumber);
                                }

                            }
                            SendLoginDetailSMS(iStudentId);
                            // 0: Add new student and 1: Edit existinf student(this neede because while adding new student, geeting YearwiseStudentId other wise Schoolwise StudentId
                            if (hidIsOverwriteSiblingDetails.Value == Constants.I_ZERO.ToString())
                            {
                                StringBuilder oStringBuilderForAdd = new StringBuilder();
                                string sMiddleStringInMessageForAdd = ", ";
                                for (int iRowId = 0; iRowId < lstvwSiblingsDetails.Items.Count; iRowId++)
                                {
                                    if ((lstvwSiblingsDetails.Items[iRowId].FindControl("ChkSelectSiblingsSingle") as CheckBox).Checked == true)
                                    {
                                        oStringBuilderForAdd.Append((lstvwSiblingsDetails.DataKeys[iRowId]["CommonFieldId"]).ToInt() + sMiddleStringInMessageForAdd);
                                    }
                                }
                                moStudentBL.OverwriteAllSiblingDetails(iStudentId, 0, oStringBuilderForAdd.ToString());
                            }

                            hidYearWiseStudentId.Value = iStudentId.ToString();
                        }
                    }
                    else
                        throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_FORM_NO);
                }
                else
                    throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_STUDENT_UNIQUE_NO);
            }
            else
                throw new DuplicateRollNumberExceptions(S_DUPLICATE_ROLL_NO);
            }
            else
                    throw new DuplicateStudentUniqueNoExceptions(S_DUPLICATE_REG_NO);
        }
            else
                throw new DuplicateGeneralRegisterNumberExceptions(S_DUPLICATE_GENERAL_REG_NO);
    }


    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFileFileUploaded(out string asFileName)
    {
        asFileName = string.Empty;
        if (fuAadharNumber.FileName != string.Empty)
        {
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(fuAadharNumber.FileName.ToString());
            if (fuAadharNumber.HasFile)
            {
                string sFileName = fuAadharNumber.PostedFile.FileName;
                string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                fuAadharNumber.SaveAs(sLinkPath);
                asFileName = sLinkName;
            }
        }
        if (asFileName == string.Empty)
            asFileName = hidAadharImage.Value;
        return string.Empty;
    }

    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFamilyPhotoUploaded(out string asFamilyFileName)
    {
        asFamilyFileName = string.Empty;
        if (FuFamilyPhoto.FileName != string.Empty)
        {
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkFamilyName = CommonUtility.GetFamilyFileNameForRenaming(FuFamilyPhoto.FileName.ToString());
            if (FuFamilyPhoto.HasFile)
            {
                string sFamilyName = FuFamilyPhoto.PostedFile.FileName;
                string sLinkFamilyPath = sServerPath + S_Family_Photo + sLinkFamilyName;
                FuFamilyPhoto.SaveAs(sLinkFamilyPath);
                asFamilyFileName = sLinkFamilyName;
            }
        }
        if (asFamilyFileName == string.Empty)
            asFamilyFileName = hidFamilyImage.Value;
        return string.Empty;
    }

    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsCasteCertificateUploaded()
    {
       string sCasteCertiFicateFileName = string.Empty;
        if (fuCastCertificate.FileName != string.Empty)
        {
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkCasteCertificateName = CommonUtility.GetFamilyFileNameForRenaming(fuCastCertificate.FileName.ToString());
            if (fuCastCertificate.HasFile)
            {
                string sFamilyName = FuFamilyPhoto.PostedFile.FileName;
                string sLinkCasteCertificatePath = sServerPath + S_CastCertificate_Photo + sLinkCasteCertificateName;
                fuCastCertificate.SaveAs(sLinkCasteCertificatePath);
                sCasteCertiFicateFileName = sLinkCasteCertificateName;
            }
        }
        if (sCasteCertiFicateFileName == string.Empty)
            sCasteCertiFicateFileName = hidCasteCertImage.Value;
        return sCasteCertiFicateFileName;
    }

    /// <summary>
    /// This method is used to send login details sms to parent.
    /// </summary>
    private void SendLoginDetailSMS(int aiStudentId)
    {
        if (chkSendSMS.Checked)
        {
            if (moSchoolUserBL == null)
                moSchoolUserBL = new SchoolUserBL(Convert.ToInt32(hidUserId.Value));
            moStudentBL = new StudentBL(Convert.ToInt32(hidSchoolId.Value), aiStudentId, true);
            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            string sDisplayText = moStudentBL.SalutationName + ' ' + moStudentBL.FirstName + ' ' + moStudentBL.LastName + " (" + moStudentBL.StandardDivisionName + " - " + moStudentBL.EnrolementNo + ")";
            int iSmsID = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsID, miSchoolId);


            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                string sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
                string sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", moSchoolUserBL.Login).Replace("%PASSWORD%", moSchoolUserBL.Password);

                string sTemplateRegistrationId = string.Empty;
                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();
                
                SMS oSms = new SMS();
                oSms.Sender = oSchoolBL.SMSSenderName;
                oSms.SMSText = sLoginDetailsSmsText;
                oSms.DisplayText = sDisplayText;
                oSms.SMSType = oDTSmsTemplate.Rows[0][3].ToInt();
                oSms.SMSTypeId = Constants.SMSTypes.ForgotPasswordDetailSMS.ToInt();
                oSms.TemplateRegistrationId = sTemplateRegistrationId;
                oSms.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                oSms.To.Add(moSchoolUserBL.UserId, txtMobilePhoneNumber.Text);
                if (!string.IsNullOrEmpty(txtMobilePhoneNumber2.Text))
                    oSms.To.Add(moSchoolUserBL.UserId + "sm;", txtMobilePhoneNumber2.Text);
                oSms.Send();
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MobileUrl"]))
                SendMobileDetailsSMS(oSchoolBL, sDisplayText);
        }
    }

    /// <summary>
    /// This method is used to populate Additional details of student.
    /// </summary>
    private void PopulateAdditionalDetails(int aiSchoolWiseStudentId)
    {
        moStudentAdditionalDetails = new StudentAdditionalDetails();
        moStudentAdditionalDetails.StudentStatus = Constants.StudentAdditionalStatus.Admission.ToString();
        moStudentAdditionalDetails.AdmissionAcademicYear = txtAdditionalAdmissionAcademicYear.Text.Trim();
        moStudentAdditionalDetails.AdmissionStandard = txtAdditionalAdmissionStandard.Text.Trim();
        moStudentAdditionalDetails.CurrentAcademicYear = txtAdditionalCurrAcaYear.Text.Trim();
        moStudentAdditionalDetails.CurrentStandard = txtAdditionalCurrStandard.Text.Trim();
        moStudentAdditionalDetails.IsHandicapped = chkAdditionalIsHandicapped.Checked;
        moStudentAdditionalDetails.StubjectNames = txtAdditionalSubjectNames.Text.Trim();

        if (txtFAnnualIncome.Text != string.Empty)
            moStudentAdditionalDetails.FatherAnnualIncome = txtFAnnualIncome.Text.ToDecimal();

        if (txtMAnnualIncome.Text != string.Empty)
            moStudentAdditionalDetails.MotherAnnualIncome = txtMAnnualIncome.Text.ToDecimal();

        if (txtAdditionalPreviousMarksObtained.Text.ToString().Trim() == string.Empty)
            moStudentAdditionalDetails.PreviousYearMarksObtained = Constants.I_ZERO;
        else
            moStudentAdditionalDetails.PreviousYearMarksObtained = txtAdditionalPreviousMarksObtained.Text.ToInt();

        if (txtAdditionalPreviousMarksOutOff.Text.Trim() == string.Empty)
            moStudentAdditionalDetails.PreviousYearMarksOutOff = Constants.I_ZERO;
        else
            moStudentAdditionalDetails.PreviousYearMarksOutOff = txtAdditionalPreviousMarksOutOff.Text.ToInt();

        moStudentAdditionalDetails.PreviousYearOfPassing = txtAdditionalPreviousYearOfPassing.Text.Trim();
        moStudentAdditionalDetails.SchoolwiseStudentId = moStudentBL.StudentId == 0 ? aiSchoolWiseStudentId : moStudentBL.StudentId;
        moStudentAdditionalDetails.Religion = txtAdditionalReligion.Text.Trim();
        moStudentAdditionalDetails.BirthTaluka = txtAdditionalBirthTaluka.Text.Trim();
        moStudentAdditionalDetails.BirthDistrict = txtAdditionalBirthDistrict.Text.Trim();
        moStudentAdditionalDetails.BirthState = txtAdditionalBirthState.Text.Trim();
        moStudentAdditionalDetails.HouseNoPlotNo = txtAdditionalHouseNoPlotNo.Text.Trim();
        moStudentAdditionalDetails.MainArea = txtAdditionalMainArea.Text.Trim();
        moStudentAdditionalDetails.SubareaName = txtAdditionalSubareaName.Text.Trim();
        moStudentAdditionalDetails.Landmark = txtAdditionalLandMark.Text.Trim();
        moStudentAdditionalDetails.Taluka = txtAdditionalTaluka.Text.Trim();
        moStudentAdditionalDetails.District = txtAdditionalDistrict.Text.Trim();

        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            moStudentAdditionalDetails.FeeAreaName = cmbAdditionalFeeAreaName.SelectedValue.ToInt();
        else
            moStudentAdditionalDetails.FeeAreaName = Constants.I_ZERO;

        //moStudentAdditionalDetails.FatherOccupation = txtAdditionalFatherOccupation.Text.Trim();
        moStudentAdditionalDetails.FatherOccupation = string.Empty;
        moStudentAdditionalDetails.FatherQualification = txtAdditionalFatherQualification.Text.Trim();
        moStudentAdditionalDetails.FatherEmail = txtAdditionalFatherEmail.Text.Trim();
        moStudentAdditionalDetails.FatherOfficeName = txtAdditionalFatherOfficeName.Text.Trim();
        moStudentAdditionalDetails.FatherOfficeAddress = txtAdditionalFatherOfficeAddress.Text.Trim();
        moStudentAdditionalDetails.MotherOccupation = txtAdditionalMotherOccupation.Text.Trim();
        moStudentAdditionalDetails.MotherQualification = txtAdditionalMotherQualification.Text.Trim();
        moStudentAdditionalDetails.MotherEmail = txtAdditionalMotherEmail.Text.Trim();
        moStudentAdditionalDetails.MotherOfficeName = txtAdditionalMotherOfficeName.Text.Trim();
        moStudentAdditionalDetails.MotherOfficeAddress = txtAdditionalMotherOfficeAddress.Text.Trim();

        if (txtFatherWeight.Text.Trim() != string.Empty)
            moStudentAdditionalDetails.FatherWeight = txtFatherWeight.Text.ToInt();
        if (txtMotherWeight.Text.Trim() != string.Empty)
            moStudentAdditionalDetails.MotherWeight = txtMotherWeight.Text.ToInt();
        if (txtFatherHeight.Text.Trim() != string.Empty)
            moStudentAdditionalDetails.FatherHeight = txtFatherHeight.Text.ToInt();
        if (txtMotherHeight.Text.Trim() != string.Empty)
            moStudentAdditionalDetails.MotherHeight = txtMotherHeight.Text.ToInt();

        moStudentAdditionalDetails.FatherBloodGroup = txtFatherBloodGroup.Text.Trim();
        moStudentAdditionalDetails.MotherBloodGroup = txtMotherBloodGroup.Text.Trim();
        moStudentAdditionalDetails.FatherAadharCardNo = txtFatherAdharcardNo.Text.Trim();
        moStudentAdditionalDetails.MotherAadharCardNo = txtMotherAadharCardNo.Text.Trim();
        
        if (txtMonthlyIncome.Text.Trim() != string.Empty)
            moStudentAdditionalDetails.FamilyMonthlyIncome = txtMonthlyIncome.Text.ToDecimal();

         moStudentAdditionalDetails.CWSN = txtCWSN.Text.Trim();

        if (txtAdditionalFatherDOB.Text.Trim().ToString() != string.Empty)
            moStudentAdditionalDetails.FatherDOB = Convert.ToDateTime(txtAdditionalFatherDOB.Text.Trim().ToString());
        else
            moStudentAdditionalDetails.FatherDOB = Constants.S_DEFAULT_DATE_5.ToDateTime();

        if (txtAdditionalMotherDOB.Text.Trim().ToString() != string.Empty)
            moStudentAdditionalDetails.MotherDOB = Convert.ToDateTime(txtAdditionalMotherDOB.Text.Trim().ToString());
        else
            moStudentAdditionalDetails.MotherDOB = Constants.S_DEFAULT_DATE_5.ToDateTime();

        moStudentAdditionalDetails.FatherDesignation = txtAdditionalFatherDesignation.Text.Trim();
        moStudentAdditionalDetails.MotherDesignation = txtAdditionalMotherDesignation.Text.Trim();

        string sFatherPhotoName = CheckIsAdditionalPhotosUploaded(FUAdditionalFatherPhoto, Constants.I_ONE);
        string sMotherPhotoName = CheckIsAdditionalPhotosUploaded(fuAdditionalMotherPhoto, Constants.I_TWO);
        string sGuardiansPhotoName = CheckIsAdditionalPhotosUploaded(FUAdditionalGuardianPhoto, Constants.I_THREE);
        moStudentAdditionalDetails.FatherPhoto = sFatherPhotoName;
        moStudentAdditionalDetails.MotherPhoto = sMotherPhotoName;
        moStudentAdditionalDetails.GuardianPhoto = sGuardiansPhotoName;

        string sFatherAadharCardPhotoName = CheckIsAdditionalPhotosUploaded(flUploadFatherAaadhar, Constants.I_FOUR);
        string sMotherAadharCardPhotoName = CheckIsAdditionalPhotosUploaded(flUploadMotherAaadhar, Constants.I_FIVE);
        moStudentAdditionalDetails.FatherAadharCardPhoto = sFatherAadharCardPhotoName;
        moStudentAdditionalDetails.MotherAadharCardPhoto = sMotherAadharCardPhotoName;

        if (txtAdditionalAnniversaryDate.Text.Trim().ToString() != string.Empty)
            moStudentAdditionalDetails.MarriageAnniversaryDate = Convert.ToDateTime(txtAdditionalAnniversaryDate.Text.ToString());
        else
            moStudentAdditionalDetails.MarriageAnniversaryDate = Constants.S_DEFAULT_DATE_5.ToDateTime();

      
        moStudentAdditionalDetails.RelativeName = txtRelativeName.Text.Trim();
        moStudentAdditionalDetails.ResisdenceTypeId = cmbResidenceType.SelectedValue.ToInt();   //////added
        if(FUAdditionalFatherPhoto.HasFile)
            moStudentAdditionalDetails.FatherBinaryPhoto = GetByteArrayFromFileField(FUAdditionalFatherPhoto);
        if (fuAdditionalMotherPhoto.HasFile)
            moStudentAdditionalDetails.MotherBinaryPhoto = GetByteArrayFromFileField(fuAdditionalMotherPhoto);
        if (FUAdditionalGuardianPhoto.HasFile)
            moStudentAdditionalDetails.ParentBinaryPhoto = GetByteArrayFromFileField(FUAdditionalGuardianPhoto);

        moStudentAdditionalDetails.Name1 = txtBName1.Text.Trim();

        if (txtBAge1.Text != string.Empty)
            moStudentAdditionalDetails.Age1 = txtBAge1.Text.Trim().ToInt();

        moStudentAdditionalDetails.Institute1 = txtBInstitution1.Text.Trim();
        moStudentAdditionalDetails.Standard1 = txtBStandard1.Text.Trim();

        moStudentAdditionalDetails.Name2 = txtBName2.Text.Trim();

        if (txtBAge2.Text != string.Empty)
            moStudentAdditionalDetails.Age2 = txtBAge2.Text.Trim().ToInt();

        moStudentAdditionalDetails.Institute2 = txtBInstitution2.Text.Trim();
        moStudentAdditionalDetails.Standard2 = txtBStandard2.Text.Trim();
        moStudentAdditionalDetails.ResisdenceTypeId = cmbResidenceType.SelectedValue.ToInt();
        moStudentAdditionalDetails.RFID = txtRFID.Text.Trim();
        moStudentAdditionalDetails.PenNo = txtPenNo.Text.Trim();
        moStudentAdditionalDetails.ApaarId = txtApaarId.Text.Trim();
        
        //moStudentAdditionalDetails.FatherAnnualIncome = txtFAnnualIncome.Text.ToDecimal();
        //moStudentAdditionalDetails.MotherAnnualIncome = txtMAnnualIncome.Text.ToDecimal();
      
        
    }



    /// <summary>
    /// This method is used to populate user information.
    /// </summary>
    private void PopulateUserInformation()
    {
        DataTable oDataTable = StudentBL.GetNextStudentRollNoAndLogin(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidDivisionId.Value), Convert.ToInt32(hidSchoolId.Value));
        if (miSchoolId == Constants.SchoolId.SVNP.ToInt())
            txtLoginId.Text = txtRegNo.Text;
        else
            txtLoginId.Text = Convert.ToString(oDataTable.Rows[0]["LoginId"]);

        txtRollNumber.Text = Convert.ToString(oDataTable.Rows[0]["RollNo"]);
        hidDefaultRollNo.Value = Convert.ToString(oDataTable.Rows[0]["RollNo"]);

        moSchoolUserBL = new SchoolUserBL();
        if (hidStudentId.Value != Constants.S_EMPTY_STRING)
            moSchoolUserBL.UserId = Convert.ToInt32(hidStudentId.Value);
        moSchoolUserBL.SchoolId = miSchoolId;
        moSchoolUserBL.Login = txtLoginId.Text.Trim();
        Random oRandomNo = new Random((int)DateTime.Now.Ticks);
        moSchoolUserBL.Password = oRandomNo.Next(100000, 999999).ToString();
        moSchoolUserBL.UserRoleId = Convert.ToInt32(Constants.UserRoles.Student);
        moSchoolUserBL.InsertedBy = Convert.ToString(miUserId);
        moSchoolUserBL.Email = txtEmail.Text.Trim();
        moSchoolUserBL.FirstName = "";
        moSchoolUserBL.LastName = "";
        moSchoolUserBL.MiddleName = "";
        moSchoolUserBL.sDOB = DBNull.Value.ToString();
        

        // We need to give this field a default value else, it will cause problems when converting to xml.
        // This is becuase the underlying field is a char, which has a default value of '\0', which is invalid when converting to xml.
        moSchoolUserBL.CanApproveRequisition = Constants.C_NO;
        moSchoolUserBL.CanCreateGeneralRequisition = Constants.C_NO;
        moSchoolUserBL.CanSanctionLeave = Constants.C_NO;

    }
    /// <summary>
    /// This method is used to populate student streamwise subject information.
    /// </summary>
    /// 
    private void PopulateStudentStreamwiseSubjects()
    {
        moStudentBL.Stream = ddlStream.SelectedValue.ToInt();  
        moStudentBL.Group = ddlGroup.SelectedValue.ToInt(); 
        //moStudentBL.CompulsorySubject = lblCompulsarySubjects.Text; 
        moStudentBL.CompulsorySubject = hidCompulsorySubjects.Value;
        moStudentBL.CompitativeExams = GetCompitativeExams(); 
       
        moStudentBL.FirstOptionalSubject = RadioOptionalSubjects.SelectedValue.ToInt();
      //  hidOptionalSubjects.Value = moStudentBL.FirstOptionalSubject.ToString();  /////
        if (ddlStream.SelectedValue == "3")
            moStudentBL.SecondOptionalSubject = RadioOptionalSubjectArts.SelectedValue.ToInt();
        
    }
    /// <summary>
    /// This method is used to get compitative exams for Saving in database.
    /// </summary>
    /// <returns></returns>
    private string GetCompitativeExams()
    {
        StringBuilder sbStaffGroupIds = new StringBuilder();
        string sStaffGroupIds = string.Empty;

        for (int iItemCount = 0; iItemCount < chkCompitativeExams.Items.Count; iItemCount++)
        {
            if (chkCompitativeExams.Items[iItemCount].Selected)
            {
                sbStaffGroupIds = sbStaffGroupIds.Append("," + chkCompitativeExams.Items[iItemCount].Value);

            }
        }

        if (sbStaffGroupIds.ToString().StartsWith(","))
            sStaffGroupIds = sbStaffGroupIds.ToString().Substring(1);

        return sStaffGroupIds;
    }
   
    /// <summary>
    /// This function is used to check whether updating  roll number is free or assigned to other student.
    /// </summary>
    private bool CheckIfRollNoAllReadyAssigned()
    {
        return moStudentBL.CheckIsRollNumberDuplicate(moStudentBL.SchoolId, moStudentBL.YearId, moStudentBL.StandardId, moStudentBL.DivisionId, moStudentBL.RollNo, moStudentBL.StudentId);
    }

    /// <summary>
    /// This function is used to check whether updating  roll number is free or assigned to other student.
    /// </summary>
    private bool CheckIfFormNoAllReadyAssigned(int aiStudentId)
    {
        return moStudentBL.CheckIsRFormNumberDuplicate(moStudentBL.SchoolId, txtFormNo.Text, aiStudentId);
    }

    /// <summary>
    /// this method is used to modify existing student's information 
    /// </summary>
    private int UpdateStudent()
    {
        int iTrackingId = 0;

        string sMsg = string.Empty;
        bool bFlag = false;

        string sLinkName;
        string sFileUploadErr = CheckIsFileFileUploaded(out sLinkName);

        string sLinkNameForFamilyPhoto;
        string sFilePhotoUploadError = CheckIsFamilyPhotoUploaded(out sLinkNameForFamilyPhoto);

        string sLinkNameForCasteCert = CheckIsCasteCertificateUploaded();

        PopulateStudentStruct(sLinkName, sLinkNameForFamilyPhoto, sLinkNameForCasteCert);

        if (TrStreamDetails.Visible)
        {
            PopulateStudentStreamwiseSubjects(); //////
            moStudentBL.UpdateStudentStreamwiseDetails(moStudentBL.StudentId);
        }

            if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                PopulateAdditionalDetails(Constants.I_ZERO);

            if (((hidRuleId.Value != moStudentBL.Rule_Id.ToString())) || (Convert.ToDateTime(hidOldJoiningDate.Value) != moStudentBL.JoiningDate) || (cmbFeeCategory.Visible && Convert.ToString(hidOldFeeCategoryId.Value) != Convert.ToString(cmbFeeCategory.SelectedValue)))
            {
                int iStudentId = Convert.ToInt32(moStudentBL.YearWiseStudentId);
                int iAcademicYrId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);

                // If loginUser Role is not "SuperAdmin" then only verify RI check otherwies not.
                bFlag = true;
                if ((hidIsSuperAdmin.Value == Constants.S_NO && chkIsRTEApplicable.Checked == false && Convert.ToDateTime(hidOldJoiningDate.Value) != Convert.ToDateTime(txtJoiningDate.Text) && Convert.ToDateTime(hidOldJoiningDate.Value).Month != Convert.ToDateTime(txtJoiningDate.Text).Month) || (cmbFeeCategory.SelectedValue != string.Empty && Convert.ToString(hidOldFeeCategoryId.Value) != Convert.ToString(cmbFeeCategory.SelectedValue)))
                    sMsg = moStudentBL.CheckDependenciesForFees(moStudentBL.StudentId, iAcademicYrId);
                else
                    bFlag = false;

            }
            if (sMsg == string.Empty)
            {
                if (!CheckIfRollNoAllReadyAssigned())
                {
                    if (!moStudentBL.isRegisterNoAlreadyPresent())
                    {
                        if (!moStudentBL.isGeneralRegisterNoAlreadyPresent())
                        {
                            if (!moStudentBL.isStudentUniqueNoAlreadyPresent())
                            {
                                if (string.IsNullOrEmpty(moStudentBL.sFormNo.Trim()) || !CheckIfFormNoAllReadyAssigned(moStudentBL.StudentId))
                                {
                                   
                                    moStudentBL.UpdateStudent(Convert.ToDateTime(hidOldJoiningDate.Value), bFlag, out iTrackingId);
                                    if (SchoolBase.Settings.ShowDayBoardingOptionOnStudentsScreen && hidOldIsForDayBoarding.Value != moStudentBL.IsForDayBoarding.ToString())                                    
                                        moStudentBL.DeleteDayBoardingFees(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentId.Value), miUserId);                                       

                                    if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                                    {
                                        moStudentBL.AddStudentAdditionalDetails(miSchoolId, miUserId, moStudentAdditionalDetails);

                                        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                                        {
                                            if(cmbAdditionalFeeAreaName.SelectedValue != hidOldFeeAreaId.Value)
                                                moStudentBL.GenerateTrasnportFeeEntry(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentId.Value));
                                        }
                                        else
                                            moStudentBL.GenerateTrasnportFeeEntry(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentId.Value));
                                    }
                                    SendLoginDetailSMS(moStudentBL.YearWiseStudentId);
                                    // Pass parameter because while adding new student, Id is 0 and after Insering student details, returning new student Id (must pass).
                                    if (hidIsOverwriteSiblingDetails.Value == Constants.I_ZERO.ToString())
                                    {
                                        StringBuilder oStringBuilder = new StringBuilder();
                                        string sMiddleStringInMessage = ", ";
                                        for (int iRowId = 0; iRowId < lstvwSiblingsDetails.Items.Count; iRowId++)
                                        {
                                            if ((lstvwSiblingsDetails.Items[iRowId].FindControl("ChkSelectSiblingsSingle") as CheckBox).Checked == true)
                                            {
                                                oStringBuilder.Append((lstvwSiblingsDetails.DataKeys[iRowId]["CommonFieldId"]).ToInt() + sMiddleStringInMessage);
                                            }
                                        }
                                        moStudentBL.OverwriteAllSiblingDetails(moStudentBL.StudentId, 1, oStringBuilder.ToString());
                                    }

                                    //moStudentBL.UpdateStudentTrackingDetails(miSchoolId, miUserId, moStudentBL.StudentId, iTrackingId, miAcademicYearId);

                                    if (FileUploadLogo.HasFile)
                                    {
                                        Byte[] ImageBinaryData = GetByteArrayFromFileField(FileUploadLogo);
                                        moStudentBL.UpdateStudentPhoto(ImageBinaryData);
                                    }
                                    else
                                    {
                                        if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
                                        {
                                            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
                                            var oImage = lstImageData.Where(lst => lst.UserID == hidStudentId.Value.ToInt()).LastOrDefault();
                                            if (!oImage.IsNull())
                                                moStudentBL.UpdateStudentPhoto(oImage.ImagesData);
                                        }
                                    }
                                }
                                else
                                    throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_FORM_NO);
                            }
                            
                        
                    else
                        throw new DuplicateStudentUniqueNoExceptions(S_DUPLICATE_STUDENT_UNIQUE_NO);
                }
                        else
                            throw new DuplicateGeneralRegisterNumberExceptions(S_DUPLICATE_GENERAL_REG_NO);

                    }
                    else
                        throw new DuplicateRegisterNumberExceptions(S_DUPLICATE_REG_NO);
                }
                        
                        else
                            throw new DuplicateRollNumberExceptions(S_DUPLICATE_ROLL_NO);
                    }
                     
                    
            else
            {
                ddlFeeRule.Enabled = false;
               // chkIsStaffKid.Checked = false;
                ddlFeeRule.SelectedValue = "0";
                throw new ReferenceExceptions(sMsg);
            }

            return iTrackingId;
    }

    /// <summary>
    /// This function is used to populate student structure with the values in entry fields
    /// </summary>
    private void PopulateStudentStruct(string sAadharFileName,string sFamilyPhoto,string sCasteCertificate)
    {
        moStudentBL = new StudentBL(miSchoolId,
                                    miAcademicYearId,
                                    Convert.ToInt32(hidStudentId.Value));
        moStudentBL.InsertedBY = miUserId;
        moStudentBL.StudentId = Convert.ToInt32(hidStudentId.Value);
        moStudentBL.SchoolId = Convert.ToInt32(hidSchoolId.Value);
        moStudentBL.EnrolementNo = txtRegNo.Text.Trim();
        moStudentBL.AadharCardNo = txtAadharCardNo.Text.Trim();
        moStudentBL.StudentNameAadharCard = txtNameOnAadharCard.Text.Trim();
        moStudentBL.AadharCardNumberPhotoCopyName = sAadharFileName;
        moStudentBL.Family_Photo_Copy_Path = sFamilyPhoto;
        moStudentBL.FirstName = txtFirstName.Text.ToTitleCase();
        moStudentBL.MiddleName = txtMiddleName.Text.ToTitleCase();
        moStudentBL.LastName = txtLastName.Text.ToTitleCase();
        moStudentBL.MotherName = txtMotherName.Text.ToTitleCase();
        moStudentBL.sFormNo = txtFormNo.Text;
        moStudentBL.GRNumber = txtGRNumber.Text.Trim();
        moStudentBL.StudentUniqueNo = txtStudentID.Text.Trim();
        moStudentBL.BloodGroup = cmbBloodGroup.SelectedIndex > 0 ? cmbBloodGroup.SelectedValue : null;
        moStudentBL.Address = txtAddress.Text;
        moStudentBL.PaentName = txtParentName.Text.ToTitleCase();
        if (cmbOcupation.SelectedItem.Text.ToUpper() != "OTHER")
            moStudentBL.ParentOcupation = Convert.ToInt32(cmbOcupation.SelectedValue);
        else
        {
            moStudentBL.ParentOcupation = I_OTHER_OCCUPATION_ID;
            moStudentBL.ParentOtherOcupation = txtOtherOccupation.Text.Trim();
        }
        moStudentBL.City = txtCity.Text;
      
        moStudentBL.State = txtState.Text.Trim();
        moStudentBL.CategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
        moStudentBL.CasteAndSubCaste = txtCasteAndSubcaste.Text.Trim();
        moStudentBL.CasteCertificate_Photo_Copy_Path = sCasteCertificate;
        moStudentBL.PinCode = txtPIN.Text;
        moStudentBL.ResidencePhoneNo = txtResPhoneNumber.Text;
        moStudentBL.MobilePhoneNo = txtMobilePhoneNumber.Text;
        moStudentBL.MobilePhoneNo2 = txtMobilePhoneNumber2.Text;
        moStudentBL.MotherTongue = txtMotherTongue.Text.Trim();
        moStudentBL.NeighbourNumber = txtNeighbourNo.Text;
        moStudentBL.Religion = txtAdditionalReligion.Text;////////////////////////////add new
        moStudentBL.AreAdditionalDetailsApplicable = Settings.IsAdditionalFieldsApplicable;///////////////new line add
        moStudentBL.OfficeNumber = txtOfficeNo.Text;
        moStudentBL.Email = txtEmail.Text.Trim();
        moStudentBL.Dob = CalDobPopup.DateValue;
        moStudentBL.BirthPlace = txtBirthPlace.Text.Trim();
        moStudentBL.Nationality = txtNationality.Text.Trim();
        moStudentBL.AdmissionDate = calAdmissionDate.DateValue;
        moStudentBL.JoiningDate = calJoingDate.DateValue;
        moStudentBL.StandardId = Convert.ToInt32(hidStandardId.Value);
        moStudentBL.DivisionId = Convert.ToInt32(hidDivisionId.Value);
        moStudentBL.RollNo = Convert.ToInt32(txtRollNumber.Text);
        moStudentBL.DateOfBirthInText = CommonUtility.GetDateInWords(moStudentBL.Dob);
        moStudentBL.YearId = miAcademicYearId;
        moStudentBL.AcademicYearId = miAcademicYearId;
        moStudentBL.UDISENumber = txtUDISENumber.Text;
        moStudentBL.BoardRegistrationNo = txtBoardRegNo.Text;
        moStudentBL.RFID = txtRFID.Text.Trim();
        moStudentBL.SralNo = txtSaralNo.Text;
        
        if ((Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null) &&
             (Convert.ToChar(Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED]) == Constants.C_YES) && (Convert.ToChar(Session[Constants.S_SESSION_IS_FINALYEAR_GENERATED]) == Constants.C_NO))
            moStudentBL.Is_Dummy_Admission = Constants.C_YES;
        else
            moStudentBL.Is_Dummy_Admission = Constants.C_NO;

        if (rdoFemale.Checked)
        {
            moStudentBL.Sex = C_FEMALE;
            moStudentBL.SalutationId = I_MISS;
        }
        else
        {       
            moStudentBL.Sex = C_MALE;
            moStudentBL.SalutationId = I_MASTER;
        }
        if (FileUploadLogo.HasFile)
        {
            string sFileName = SaveFileOnServer(FileUploadLogo.FileName);
            moStudentBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
        }
        else
            moStudentBL.PhotoFilePath = "";

        moStudentBL.InsertedBY = miUserId;
        moStudentBL.UpdatedBY = miUserId;
        moStudentBL.IsNewStudent = chkNewAddmission.Checked;

        if (Settings.IsRTEApplicable)
        {
            moStudentBL.IsRTEStudent = chkIsRTEApplicable.Checked;
            if (moStudentBL.IsRTEStudent)
            {
                moStudentBL.RTECategoryId = Convert.ToInt32(cmbRTECategory.SelectedValue);
                moStudentBL.RTEFormNo = txtRTEApplicationForm.Text;
                moStudentBL.AnnualIncome = txtAnnualIncome.Text.ToInt();////             
            }
            else
            {
                moStudentBL.RTECategoryId = Constants.I_ZERO;
                moStudentBL.RTEFormNo = string.Empty;
                moStudentBL.AnnualIncome = Constants.I_ZERO;////
            }
        }
        
        if (Settings.IsConcessionApplicable)
        {
            //if (chkIsStaffKid.Checked)
                moStudentBL.Rule_Id = Convert.ToInt32(ddlFeeRule.SelectedValue);
            //else
            //    moStudentBL.Rule_Id = Constants.I_ZERO;
        }
        else
        {
            moStudentBL.Rule_Id = 0;
            trCheckStaffKid.Visible = false;
            trApplicableRule.Visible = false;
        }


        moStudentBL.IsStaffKid = chkIsStaffKid.Checked;

        moStudentBL.SecondLanguageSubjectId = Convert.ToInt32(ddlSecondLanguage.SelectedValue);
        moStudentBL.ThirdLanguageSubjectId = Convert.ToInt32(cmbThirdLanguage.SelectedValue);
        moStudentBL.ParentUserRoleId = Convert.ToInt32(ddlUserRole.SelectedValue);
        moStudentBL.ParentUserId = Convert.ToInt32(ddlUserName.SelectedValue);

        moStudentBL.LastSchoolName = txtLastSchoolName.Text.Trim();
        moStudentBL.LastSchoolAddress = txtLastSchoolAddress.Text.Trim();
        moStudentBL.LastSchoolStandard = txtLastStandard.Text.Trim();
        moStudentBL.LastSchoolUDISENo = txtLastUDISENo.Text.Trim();
        moStudentBL.LastSchoolBoardName = rdolstlastSchoolBoard.SelectedValue;
        moStudentBL.IsRecognised = rdobtnRecognisedYes.Checked;
        moStudentBL.IsRiseAndShine = chkRiseAndShine.Checked;
       
        if (miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
            moStudentBL.AdmissionForId = cmbAdmissionFor.SelectedValue.ToInt();
        else
            moStudentBL.AdmissionForId = Constants.I_ZERO;

        if (hidIsAdditionalInformationAvailable.Value == Constants.S_YES)
        {
            foreach (HtmlTableRow oHtmlTableRow in tblAdditionalInformation.Rows)
            {
                foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
                {
                    foreach (Control oControl in oHtmlTableCell.Controls)
                    {
                        if (oControl is TextBox)
                        {
                            TextBox oTextBox = ((TextBox)oControl);
                            if (oTextBox.ID == "txtHeight")
                                moStudentBL.Height = (oTextBox.Text.Trim() != string.Empty) ? Convert.ToDouble(oTextBox.Text) : 0;
                            if (oTextBox.ID == "txtWeight")
                                moStudentBL.Weight = (oTextBox.Text.Trim() != string.Empty) ? Convert.ToDouble(oTextBox.Text) : 0;
                        }
                    }
                }
            }
        }

        if (SchoolBase.Settings.ShowDayBoardingOptionOnStudentsScreen)
            moStudentBL.IsForDayBoarding = chkIsDayBoarding.Checked;
        else
            moStudentBL.IsForDayBoarding = false;

        int iFeeCategoryId = Constants.I_ZERO;
        if (SchoolBase.Settings.IsAaryanSchool)        
            iFeeCategoryId = cmbFeeCategory.SelectedValue.ToInt();

        moStudentBL.FeeCategoryId = iFeeCategoryId;

        moStudentBL.SralNo = txtSaralNo.Text.Trim();
        moStudentBL.IsOnlyChild = chkIsOnlyChild.Checked;
        moStudentBL.Minority = chkIsMinority.Checked;
        moStudentBL.ResidenceTypeId = cmbResidenceType.SelectedValue.ToInt();
       
       
    }

    /// <summary>
    /// This function is used to ValidateFile
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <returns></returns>
    private string ValidateFile(string asServerFilePath)
    {
        string sReturnErrorMsg = "";
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
            {
                sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px" + " " + Resources.LocalizedResources.And + " " + +I_WIDTH_LIMIT + "px" + " " + Resources.LocalizedResources.respectively + ".";
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > I_HEIGHT_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
                if (oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
            }
            oFileStream.Close();
            oImg = null;

        }
        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal;
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }

    /// <summary>
    /// This method is used save file on server 
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName)
    {
        // Upload the file to the server.
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }

        FileUploadLogo.SaveAs(sServerFilePath);
        string sErrMessage = ValidateFile(sServerFilePath);
        if (sErrMessage.Equals(""))
        {
            // delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidFilePath.Value;
            if (File.Exists(sFileToDelete))
            {
                File.Delete(sFileToDelete);
            }
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
        return sFileName;

    }

    /// <summary>
    /// This function is used to fill entry fields with proper values in edit mode
    /// </summary>
    private void BindEntryFields(Int32 aiSchoolId, Int32 aiStudentId, int aiAcademicYearId)
    {
        

        moStudentBL = new StudentBL(aiSchoolId, aiAcademicYearId, aiStudentId);

        if (!string.IsNullOrEmpty(moStudentBL.AadharCardNumberPhotoCopyName))
        {
            btnView.Visible = true;
            string sNewFileName = S_FOLDER_PATH + moStudentBL.AadharCardNumberPhotoCopyName;
            hidAadharImage.Value = moStudentBL.AadharCardNumberPhotoCopyName;
            btnView.Attributes.Add("onclick", " window.open('" + sNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
        }


        if (!string.IsNullOrEmpty(moStudentBL.Family_Photo_Copy_Path))
        {
            btnView1.Visible = true;
            imgbtnDelete.Visible = true;
            string sFamilyNewFileName = S_Family_Path + moStudentBL.Family_Photo_Copy_Path;
            hidFamilyImage.Value = moStudentBL.Family_Photo_Copy_Path;
            btnView1.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");            
        }

        if (!string.IsNullOrEmpty(moStudentBL.CasteCertificate_Photo_Copy_Path))
        {
            imgbtnViewCasteCert.Visible = true;
            imgbtnDeleteCasteCert.Visible = true;
            string sNewFileName = S_CastCertificate_PhotoPath + moStudentBL.CasteCertificate_Photo_Copy_Path;
            hidCasteCertImage.Value = moStudentBL.CasteCertificate_Photo_Copy_Path;
            imgbtnViewCasteCert.Attributes.Add("onclick", " window.open('" + sNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
        }
        else 
           {
              imgbtnViewCasteCert.Visible = false;
              imgbtnDeleteCasteCert.Visible = false;
           }

        if (hidOverwrite.Value == Constants.S_NO)
        {
            string sFormNo = moStudentBL.GetFormNumber(aiSchoolId, aiStudentId, aiAcademicYearId);
            txtFormNo.Text = sFormNo;
            txtRollNumber.Text = Convert.ToString(moStudentBL.RollNo);
            txtLoginId.Text = moStudentBL.LoginName;
            txtFirstName.Text = moStudentBL.FirstName;
            if (moStudentBL.BloodGroup != null && moStudentBL.BloodGroup.Trim() != string.Empty)
                cmbBloodGroup.SelectedValue = moStudentBL.BloodGroup;
            txtRegNo.Text = moStudentBL.EnrolementNo;
           
            hidOrgRegNo.Value = moStudentBL.EnrolementNo;
            CalDobPopup.DateValue = moStudentBL.Dob;

            lblAge.Text = GetAge();
            
            calAdmissionDate.DateValue = moStudentBL.AdmissionDate;
            calJoingDate.DateValue = moStudentBL.JoiningDate;
            char cFemale = C_FEMALE;
            if (moStudentBL.Sex == C_FEMALE || moStudentBL.Sex.ToString() == cFemale.ToString().ToLower())
                rdoFemale.Checked = true;
            else
                rdoMale.Checked = true;
            hidYearWiseStudentId.Value = moStudentBL.YearWiseStudentId.ToString();
            ddlSecondLanguage.Enabled = ddlSecondLanguage.Items.Count > 1;
            ddlSecondLanguage.SelectedValue = moStudentBL.SecondLanguageSubjectId.ToString();

            cmbThirdLanguage.Enabled = cmbThirdLanguage.Items.Count > 1;
            cmbThirdLanguage.SelectedValue = moStudentBL.ThirdLanguageSubjectId.ToString();

            hidFilePath.Value = moStudentBL.PhotoFilePath;
            if (!moStudentBL.PhotoFilePathInBinary.IsNull())
                HidIsBinaryImage.Value = Convert.ToBase64String(moStudentBL.PhotoFilePathInBinary);

            if (!moStudentBL.PhotoFilePathInBinary.IsNull())
                imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + moStudentBL.UserId;
            else
                imgPhoto.Src = S_DEFAULT_PHOTO;
            chkNewAddmission.Checked = moStudentBL.IsNewStudent;
            chkNewAddmission.Enabled = false;
            chkIsRTEApplicable.Checked = moStudentBL.IsRTEStudent;
            chkIsRTEApplicable.Enabled = false;

            if (hidIsAdditionalInformationAvailable.Value == Constants.S_YES)
            {
                foreach (HtmlTableRow oHtmlTableRow in tblAdditionalInformation.Rows)
                {
                    foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
                    {
                        foreach (Control oControl in oHtmlTableCell.Controls)
                        {
                            if (oControl is TextBox)
                            {
                                TextBox oTextBox = ((TextBox)oControl);
                                if (oTextBox.ID == "txtHeight")
                                    oTextBox.Text = Convert.ToString(moStudentBL.Height);
                                if (oTextBox.ID == "txtWeight")
                                    oTextBox.Text = Convert.ToString(moStudentBL.Weight);
                            }
                        }
                    }
                }
            }

            txtLastSchoolName.Text = moStudentBL.LastSchoolName;
            txtLastSchoolAddress.Text = moStudentBL.LastSchoolAddress;
            txtLastStandard.Text = moStudentBL.LastSchoolStandard;
            txtLastUDISENo.Text = moStudentBL.LastSchoolUDISENo;
            rdolstlastSchoolBoard.SelectedValue = moStudentBL.LastSchoolBoardName;
            rdobtnRecognisedYes.Checked = moStudentBL.IsRecognised;
           
            if (moStudentBL.IsRecognised)
                rdobtnRecognisedYes.Checked = true;
            else
                rdobtnRecognisedNo.Checked = true;
        }
        txtMiddleName.Text = moStudentBL.MiddleName;
        txtLastName.Text = moStudentBL.LastName;
        txtMotherName.Text = moStudentBL.MotherName;
        txtBirthPlace.Text = moStudentBL.BirthPlace;
        txtNationality.Text = moStudentBL.Nationality;
        txtEmail.Text = moStudentBL.Email;
        txtStudentID.Text = moStudentBL.StudentUniqueNo;
        txtGRNumber.Text = moStudentBL.GRNumber;
        if (moUserRole == Constants.UserRoles.Teacher && !Boolean.Parse(hidUserHasFullAccess.Value))
            txtRegNo.ReadOnly = true;
        cmbCategory.SelectedValue = Convert.ToString(moStudentBL.CategoryId);
        int iCasteId = Convert.ToInt32(moStudentBL.CategoryId);
        txtCasteAndSubcaste.Text = moStudentBL.CasteAndSubCaste;
        txtState.Text = moStudentBL.State;
        txtAddress.Text = moStudentBL.Address;
        txtCity.Text = moStudentBL.City;
        txtResPhoneNumber.Text = moStudentBL.ResidencePhoneNo;
        txtMobilePhoneNumber.Text = moStudentBL.MobilePhoneNo;
        txtMobilePhoneNumber2.Text = moStudentBL.MobilePhoneNo2;
        txtMotherTongue.Text = moStudentBL.MotherTongue;
        txtOfficeNo.Text = moStudentBL.OfficeNumber;
        txtNeighbourNo.Text = moStudentBL.NeighbourNumber;
        txtAdditionalReligion.Text = moStudentBL.Religion;/////////////////////new add
        txtParentName.Text = moStudentBL.PaentName;
        txtAadharCardNo.Text = moStudentBL.AadharCardNo;
        txtNameOnAadharCard.Text = moStudentBL.StudentNameAadharCard;
        txtUDISENumber.Text = moStudentBL.UDISENumber;
        txtBoardRegNo.Text = moStudentBL.BoardRegistrationNo;
        hidUserId.Value = Convert.ToString(moStudentBL.UserId);
        btnRemovePhoto.Enabled = HidIsBinaryImage.Value == string.Empty ? false : true;
        
        chkRiseAndShine.Checked = moStudentBL.IsRiseAndShine;

        if (moSchool == Constants.SchoolId.PPS && !moStudentBL.IsPrePrimaryStandard && !string.IsNullOrEmpty(moStudentBL.PrePrimaryEnrolmentNumber))
        {
            trPPRegNo.Visible = true;
            txtPrePrimaryRegNo.Text = moStudentBL.PrePrimaryEnrolmentNumber;
        }

        if (miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
        {
            trAdmissionFor.Visible = true;
            cmbAdmissionFor.SelectedValue = moStudentBL.AdmissionForId.ToString();
            cmbAdmissionFor.Enabled = false;
        }
        else
            trAdmissionFor.Visible = false;

        if (moStudentBL.ParentOcupation != I_OTHER_OCCUPATION_ID)
            cmbOcupation.SelectedValue = moStudentBL.ParentOcupation.ToString();
        else
        {
            ListItem oListItem = cmbOcupation.Items.FindByText("Other");
            cmbOcupation.SelectedValue = oListItem.Value;
            txtOtherOccupation.Text = moStudentBL.ParentOtherOcupation;
            trOtherOccupation.Visible = true;
        }

        if (moStudentBL.PinCode != null)
            txtPIN.Text = moStudentBL.PinCode;


        moStudentBL.InsertedBY = miUserId;
        moStudentBL.UpdatedBY = miUserId;

        trNewAdd.Visible = true;

        hidRuleId.Value = moStudentBL.Rule_Id.ToString();

        if (moStudentBL.IsRTEStudent)
        {
            trRTECatrgory.Style.Add("visibility", "visible");
            trRTECatrgory.Style.Add("display", "");
            cmbRTECategory.SelectedValue = Convert.ToString(moStudentBL.RTECategoryId);
            trRTEFormNo.Style.Add("visibility", "visible");
            trRTEFormNo.Style.Add("display","");
            txtRTEApplicationForm.Text = moStudentBL.RTEFormNo;
            trRTENote.Visible = true;
            cmbRTECategory.Enabled = true;

            if (moStudentBL.RTECategoryId == 2)
            {
                trAmount.Style.Add("visibility", "visible");
                trAmount.Style.Add("display", "");                
            }
        }
        else
            trRTENote.Visible = false;

        if (moStudentBL.AnnualIncome != null)
            txtAnnualIncome.Text = moStudentBL.AnnualIncome.ToString();///for binding 
        else
            txtAnnualIncome.Text = Constants.S_ZERO;

        if (Settings.IsConcessionApplicable)
        {
            //if (moStudentBL.Rule_Id != 0)
            //{
            //    //chkIsStaffKid.Checked = true;
            //    ddlFeeRule.Enabled = true;
            //}
            //else
            //{
            //    //chkIsStaffKid.Checked = false;
            //    //chkIsStaffKid.Enabled = !chkIsRTEApplicable.Checked;
            //    ddlFeeRule.Enabled = false;
                
            //}
            ddlFeeRule.SelectedValue = moStudentBL.Rule_Id.ToString();
        }
        else
        {
            trCheckStaffKid.Visible = false;
            trApplicableRule.Visible = false;
        }
        if (!moStudentBL.StudentSiblingNames.IsNullOrEmpty())
            hidStudentSiblingNames.Value = moStudentBL.StudentSiblingNames;
        else
            hidStudentSiblingNames.Value = string.Empty;

        chkIsStaffKid.Checked = moStudentBL.IsStaffKid;
        
       
        ddlUserRole.SelectedValue = moStudentBL.ParentUserRoleId.ToString();
        ddlUserRole_SelectedIndexChanged(ddlUserRole, null);
        ddlUserName.SelectedValue = moStudentBL.ParentUserId.ToString();
        
        if(SchoolBase.Settings.IsAdditionalFieldsApplicable)
        BindAdditionalDetails(aiSchoolId, aiStudentId);

        if (SchoolBase.Settings.ShowConfirmedByName)
        {
            if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Admin))
            {
                lblConfirmedBy.Visible = true;
                lblConfirmedBy.Text = moStudentBL.ConfirmedByText + "<br />" + moStudentBL.UpdatedByText + "<br />Admitted In : " + moStudentBL.AdmissionStandard
                               + "<br />Residence Type: " + moStudentBL.ResidenceTypeName;
            }
            else
            {
                lblConfirmedBy.Visible = true;
                lblConfirmedBy.Text = moStudentBL.ConfirmedByText + "<br />" + moStudentBL.UpdatedByText + "<br />Admitted In : " + moStudentBL.AdmissionStandard;
            }
        }
        else
        {
            lblConfirmedBy.Visible = false;
            lblConfirmedBy.Visible = true;
            lblConfirmedBy.Text = moStudentBL.UpdatedByText;
        }

        if (SchoolBase.Settings.ShowDayBoardingOptionOnStudentsScreen)
        {
            trIsForDayBoarding.Visible = true;
            chkIsDayBoarding.Checked = moStudentBL.IsForDayBoarding;
            hidOldIsForDayBoarding.Value = moStudentBL.IsForDayBoarding.ToString();
            if (moStudentBL.IsDayBoardingFeePaid)
                hidDPISIsFeePaid.Value = "Y";
            else
                hidDPISIsFeePaid.Value = "N";
        }
        else
        {
            trIsForDayBoarding.Visible = false;
            hidDPISIsFeePaid.Value = "N";
        }

        if (SchoolBase.Settings.IsAaryanSchool)
        {   
            trFeeCategory.Visible = true;
            cmbFeeCategory.SelectedValue = moStudentBL.FeeCategoryId.ToString();
            hidOldFeeCategoryId.Value = moStudentBL.FeeCategoryId.ToString();
        }
        else
            trFeeCategory.Visible = false;

        if (moStudentBL.SralNo != null)
            txtSaralNo.Text = moStudentBL.SralNo.ToString();

        if (moStudentBL.IsOnlyChild != null)
            chkIsOnlyChild.Checked = moStudentBL.IsOnlyChild;

        if (moStudentBL.Minority != null)
            chkIsMinority.Checked = moStudentBL.Minority;

        hisShowStreamSection.Value = Constants.S_ZERO;
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            DataSet ds = moStudentBL.RetriveStudentSubjectInfo(miSchoolId, aiStudentId, miAcademicYearId);
            DataTable dt = ds.Tables[0];

            bool bIsSecondary = ds.Tables[1].Rows[0]["IsSecondary"].ToBool();
            bool bisMidYear = ds.Tables[1].Rows[0]["IsMidYear"].ToBool();
            
            hisShowStreamSection.Value = (bIsSecondary && !bisMidYear ? Constants.S_ONE : Constants.S_ZERO);

            if (bIsSecondary && !bisMidYear)
            {
                TrStreamDetails.Visible = true;
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {  
                    ddlStream.SelectedValue = Convert.ToString(dt.Rows[0]["StreamId"]);
                    FillGroupCombo();
                    ddlGroup.SelectedValue = Convert.ToString(dt.Rows[0]["GroupId"]);
                    FillCompulsarySubjects();
                    lblCompulsarySubjects.Text = Convert.ToString(dt.Rows[0]["CompulsorySubjects"]);
                    // hidCompulsorySubjects.Value = lblCompulsarySubjects.Text; ////
                    if (ddlStream.SelectedValue == "3")
                    {
                        string OptionalSubjects = Convert.ToString(dt.Rows[0]["OptionalSubjects"]);
                        if (OptionalSubjects != string.Empty)
                        {                            
                            string[] soptionalSubjects = OptionalSubjects.Split(',');
                            int iOptSub1 = soptionalSubjects[0].ToInt();
                            int iOptSub2 = soptionalSubjects[1].ToInt();
                            RadioOptionalSubjects.Items.FindByValue(soptionalSubjects[0]).Selected = true;
                            RadioOptionalSubjectArts.Visible = true;

                            RadioOptionalSubjectArts.Items.FindByValue(soptionalSubjects[1]).Selected = true;
                        }
                    }
                    else
                    {
                        ListItem oItem = RadioOptionalSubjects.Items.FindByValue(dt.Rows[0]["OptionalSubjects"].ToString());
                        if (oItem != null)
                            oItem.Selected = true;
                        
                        //RadioOptionalSubjects.Items.FindByValue(dt.Rows[0]["OptionalSubjects"].ToString()).Selected = true;
                    }

                    string CompitativeExam = Convert.ToString(dt.Rows[0]["CompitativeExam"]);
                    string[] sExam = CompitativeExam.Split(',');

                    for (int m = 0; m <= sExam.Length - 1; m++)
                    {
                        for (int i = 0; i <= chkCompitativeExams.Items.Count - 1; i++)
                        {
                            if (chkCompitativeExams.Items[i].Value == sExam[m])
                            {
                                chkCompitativeExams.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is used to return age.
    /// </summary>
    /// <returns></returns>
    private string GetAge()
    {
        DateTime dtCurrentDate = hidCurrentDate.Value.ToDateTime();
        TimeSpan Span = dtCurrentDate - moStudentBL.Dob;
        DateTime Age = DateTime.MinValue + Span;

        int iYear = Age.Year - 1;
        int iMonth = Age.Month - 1;

        if (dtCurrentDate.Month > moStudentBL.Dob.Month && dtCurrentDate.Day < moStudentBL.Dob.Day)
            iMonth = iMonth - 1;
        else if (dtCurrentDate.Month == moStudentBL.Dob.Month && dtCurrentDate.Day < moStudentBL.Dob.Day)
        {
            iMonth = 11;
            iYear = iYear - 1;
        }
        else if (dtCurrentDate.Month < moStudentBL.Dob.Month && dtCurrentDate.Day < moStudentBL.Dob.Day)
            iMonth = iMonth - 1;

        return iYear + " Year(s) " + iMonth + " Month(s) till " + hidCurrentDate.Value.ToDateTime().ToString("dd MMM yyyy");
    }

    /// <summary>
    /// This method is used to bind additional details to controls.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiStudentId"></param>
    private void BindAdditionalDetails(int aiSchoolId, int aiStudentId)
    {
        StudentAdditionalDetails oStudentAdditionalDetails = moStudentBL.GetStudentAdditionalDetails(aiSchoolId, aiStudentId);
        if (oStudentAdditionalDetails != null)
        {
            txtAdditionalAdmissionAcademicYear.Text = oStudentAdditionalDetails.AdmissionAcademicYear;
            txtAdditionalAdmissionStandard.Text = oStudentAdditionalDetails.AdmissionStandard;
            txtAdditionalSubjectNames.Text = oStudentAdditionalDetails.StubjectNames;
            chkAdditionalIsHandicapped.Checked = oStudentAdditionalDetails.IsHandicapped;
            txtAdditionalCurrAcaYear.Text = oStudentAdditionalDetails.CurrentAcademicYear;
            txtAdditionalCurrStandard.Text = oStudentAdditionalDetails.CurrentStandard;
            txtAdditionalPreviousMarksObtained.Text = oStudentAdditionalDetails.PreviousYearMarksObtained.ToString() == Constants.S_ZERO ? string.Empty : oStudentAdditionalDetails.PreviousYearMarksObtained.ToString();
            txtAdditionalPreviousMarksOutOff.Text = oStudentAdditionalDetails.PreviousYearMarksOutOff.ToString() == Constants.S_ZERO ? string.Empty : oStudentAdditionalDetails.PreviousYearMarksOutOff.ToString();
            txtAdditionalPreviousYearOfPassing.Text = oStudentAdditionalDetails.PreviousYearOfPassing;
            txtAdditionalReligion.Text = oStudentAdditionalDetails.Religion;///
            txtAdditionalBirthTaluka.Text = oStudentAdditionalDetails.BirthTaluka;
            txtAdditionalBirthDistrict.Text = oStudentAdditionalDetails.BirthDistrict;
            txtAdditionalBirthState.Text = oStudentAdditionalDetails.BirthState;
            txtAdditionalHouseNoPlotNo.Text = oStudentAdditionalDetails.HouseNoPlotNo;
            txtAdditionalMainArea.Text = oStudentAdditionalDetails.MainArea;
            txtAdditionalSubareaName.Text = oStudentAdditionalDetails.SubareaName;
            txtAdditionalLandMark.Text = oStudentAdditionalDetails.Landmark;
            txtAdditionalTaluka.Text = oStudentAdditionalDetails.Taluka;
            txtAdditionalDistrict.Text = oStudentAdditionalDetails.District;
            
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                cmbAdditionalFeeAreaName.SelectedValue = oStudentAdditionalDetails.FeeAreaName.ToString();
                hidOldFeeAreaId.Value = oStudentAdditionalDetails.FeeAreaName.ToString();
            }

            //txtAdditionalFatherOccupation.Text = oStudentAdditionalDetails.FatherOccupation;
            txtAdditionalFatherQualification.Text = oStudentAdditionalDetails.FatherQualification;
            txtAdditionalFatherEmail.Text = oStudentAdditionalDetails.FatherEmail;
            txtAdditionalFatherOfficeName.Text = oStudentAdditionalDetails.FatherOfficeName;
            txtAdditionalFatherOfficeAddress.Text = oStudentAdditionalDetails.FatherOfficeAddress;
            txtAdditionalMotherOccupation.Text = oStudentAdditionalDetails.MotherOccupation;
            txtAdditionalMotherQualification.Text = oStudentAdditionalDetails.MotherQualification;
            txtAdditionalMotherEmail.Text = oStudentAdditionalDetails.MotherEmail;
            txtAdditionalMotherOfficeName.Text = oStudentAdditionalDetails.MotherOfficeName;
            txtAdditionalMotherOfficeAddress.Text = oStudentAdditionalDetails.MotherOfficeAddress;
            txtAdditionalFatherDesignation.Text = oStudentAdditionalDetails.FatherDesignation;
            txtAdditionalMotherDesignation.Text = oStudentAdditionalDetails.MotherDesignation;


            if (oStudentAdditionalDetails.FatherDOB.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_5)
                txtAdditionalFatherDOB.Text = oStudentAdditionalDetails.FatherDOB.ToString(Constants.S_DATE_FORMAT);
            else
                txtAdditionalFatherDOB.Text = string.Empty;

            if (oStudentAdditionalDetails.MotherDOB.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_5)
                txtAdditionalMotherDOB.Text = oStudentAdditionalDetails.MotherDOB.ToString(Constants.S_DATE_FORMAT);
            else
                txtAdditionalMotherDOB.Text = string.Empty;

            if (oStudentAdditionalDetails.MarriageAnniversaryDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_5)
                txtAdditionalAnniversaryDate.Text = oStudentAdditionalDetails.MarriageAnniversaryDate.ToString(Constants.S_DATE_FORMAT);
            else
                txtAdditionalAnniversaryDate.Text = string.Empty;

            string sFamilyNewFileName = string.Empty;

            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                spFatherAdditional.InnerText = "(Supports only .JPG, .JPEG file type. File size should not exceed 80kb.)";
                spMotherAdditional.InnerText = "(Supports only .JPG, .JPEG file type. File size should not exceed 80kb.)";
                spParentAdditional.InnerText = "(Supports only .JPG, .JPEG file type. File size should not exceed 80kb.)";
            }
            else
            {
                spFatherAdditional.InnerText = "(Supports only .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)";
                spMotherAdditional.InnerText = "(Supports only .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)";
                spParentAdditional.InnerText = "(Supports only .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)";
            }
            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.FatherPhoto)))
            {
                imgViewFatherPhoto.Visible = true;
                imgDeleteFatherPhoto.Visible = true;
                sFamilyNewFileName = S_Parent_Path + oStudentAdditionalDetails.FatherPhoto;
                hidFatherPhoto.Value = oStudentAdditionalDetails.FatherPhoto;
                imgViewFatherPhoto.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                imgViewFatherPhoto.Visible = false;
                imgDeleteFatherPhoto.Visible = false;
            }

            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.MotherPhoto)))
            {
                imgViewMotherPhoto.Visible = true;
                imgDeleteMotherPhoto.Visible = true;
                sFamilyNewFileName = S_Parent_Path + oStudentAdditionalDetails.MotherPhoto;
                hidMotherPhoto.Value = oStudentAdditionalDetails.MotherPhoto;
                imgViewMotherPhoto.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                imgViewMotherPhoto.Visible = false;
                imgDeleteMotherPhoto.Visible = false;
            }

            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.GuardianPhoto)))
            {
                imgViewGuardianPhoto.Visible = true;
                imgDeleteGuardianPhoto.Visible = true;
                sFamilyNewFileName = S_Parent_Path + oStudentAdditionalDetails.GuardianPhoto;
                hidGuardianPhoto.Value = oStudentAdditionalDetails.GuardianPhoto;
                imgViewGuardianPhoto.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                imgViewGuardianPhoto.Visible = false;
                imgDeleteGuardianPhoto.Visible = false;
            }

            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.MotherAadharCardPhoto)))
            {
                imgViewMotherAadharCard.Visible = true;
                imgDeleteMotherAadharCard.Visible = true;
                sFamilyNewFileName = S_Parent_AadharCardPath + oStudentAdditionalDetails.MotherAadharCardPhoto;
                hidMotherAadharCardFileName.Value = oStudentAdditionalDetails.MotherAadharCardPhoto;
                imgViewMotherAadharCard.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                imgViewMotherAadharCard.Visible = false;
                imgDeleteMotherAadharCard.Visible = false;
            }
            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.FatherAadharCardPhoto)))
            {
                imgViewFatherAadharCard.Visible = true;
                imgDeleteFatherAadharCard.Visible = true;
                sFamilyNewFileName = S_Parent_AadharCardPath + oStudentAdditionalDetails.FatherAadharCardPhoto;
                hidFatherAadharCardFileName.Value = oStudentAdditionalDetails.FatherAadharCardPhoto;
                imgViewFatherAadharCard.Attributes.Add("onclick", " window.open('" + sFamilyNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                imgViewFatherAadharCard.Visible = false;
                imgDeleteFatherAadharCard.Visible = false;
            }
           
            string sBirthCertificateFileName = string.Empty;

            if ((!string.IsNullOrEmpty(oStudentAdditionalDetails.BirthCertificateFileName)))
            {
                btnView2.Visible = true;
                sBirthCertificateFileName = "..//DOWNLOADS//Admission//BirthCertificates//" + oStudentAdditionalDetails.BirthCertificateFileName;
                hidBirthCertificatePhoto.Value = oStudentAdditionalDetails.BirthCertificateFileName;
                btnView2.Attributes.Add("onclick", " window.open('" + sBirthCertificateFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
            }
            else
            {
                btnView2.Visible = false;
            }

            txtRelativeName.Text = oStudentAdditionalDetails.RelativeName;
            cmbResidenceType.SelectedValue = oStudentAdditionalDetails.ResisdenceTypeId.ToString();/////////added
            if (oStudentAdditionalDetails.RFID != null)
                txtRFID.Text = oStudentAdditionalDetails.RFID.ToString();

            if (oStudentAdditionalDetails.PenNo != null)
                txtPenNo.Text = oStudentAdditionalDetails.PenNo;

            txtFatherWeight.Text = oStudentAdditionalDetails.FatherWeight.ToString();
            txtMotherWeight.Text = oStudentAdditionalDetails.MotherWeight.ToString();
            txtFatherHeight.Text = oStudentAdditionalDetails.FatherHeight.ToString();
            txtMotherHeight.Text = oStudentAdditionalDetails.MotherHeight.ToString();
            txtFatherAdharcardNo.Text = oStudentAdditionalDetails.FatherAadharCardNo;
            txtMotherAadharCardNo.Text = oStudentAdditionalDetails.MotherAadharCardNo;
            txtFatherBloodGroup.Text = oStudentAdditionalDetails.FatherBloodGroup;
            txtMotherBloodGroup.Text = oStudentAdditionalDetails.MotherBloodGroup;
            txtMonthlyIncome.Text = oStudentAdditionalDetails.FamilyMonthlyIncome.ToString();
            txtCWSN.Text = oStudentAdditionalDetails.CWSN;
            txtFAnnualIncome.Text = oStudentAdditionalDetails.FatherAnnualIncome.ToString();
            txtMAnnualIncome.Text = oStudentAdditionalDetails.MotherAnnualIncome.ToString();

            txtBName1.Text = oStudentAdditionalDetails.Name1;
            txtBAge1.Text = oStudentAdditionalDetails.Age1.ToString();
            txtBInstitution1.Text = oStudentAdditionalDetails.Institute1;
            txtBStandard1.Text = oStudentAdditionalDetails.Standard1;
            txtBName2.Text = oStudentAdditionalDetails.Name2;
            txtBAge2.Text = oStudentAdditionalDetails.Age2.ToString();
            txtBInstitution2.Text = oStudentAdditionalDetails.Institute2;
            txtBStandard2.Text = oStudentAdditionalDetails.Standard2;
            txtApaarId.Text = oStudentAdditionalDetails.ApaarId;
        }
    }

    /// <summary>
    /// This function is used to Fill comboBoxes
    /// </summary>
    private void FillAllComboBoxes()
     {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        StringBuilder oStringBuilderPostfix = new StringBuilder();
        StringBuilder oStringBuilderOrgnl = new StringBuilder();
        DataSet oDsMaster = MasterDataCollectionBL.GetAllMasterDataForStudent(miSchoolId, miCurrentAcademicYearId, iStandardId, Convert.ToInt32(hidDivisionId.Value));
        hidCommonFieldNames.Value = oDsMaster.Tables[Constants.I_FIVE].Rows[0].ItemArray[0].ToString();

        // 1: Category
        ControlUtility.FillDropDownList(oDsMaster.Tables[1], ref cmbCategory,
                                       "Category_Id",
                                       "Category_Name",
                                       "");

        ControlUtility.FillDropDownList(oDsMaster.Tables[0], ref cmbOcupation,
                                      "Ocupation_Id",
                                      "Ocupation_Name",
                                      Constants.S_SELECT);

        if (miSchoolId == Constants.SchoolId.JOS.ToInt())
        {
            ListItem oListItem = cmbOcupation.Items.FindByText("Other");
            if (oListItem != null)
            {
                oListItem.Selected = true;
                cmbOcupation_SelectedIndexChanged(cmbOcupation, null);
                txtFormNo.Focus();
            }
        }

        //// Add Additional Details Father Occupation.
        //ControlUtility.FillDropDownList(oDsMaster.Tables[0], ref cmbAdditionalFatherOccupation,
        //                             "Ocupation_Id",
        //                             "Ocupation_Name",
        //                             Constants.S_SELECT);

        //// Add Additional Details Father Occupation.
        //ControlUtility.FillDropDownList(oDsMaster.Tables[0], ref cmbAdditionalMotherOccupaion,
        //                             "Ocupation_Id",
        //                             "Ocupation_Name",
        //                             Constants.S_SELECT);

        // Add Additional Details Fee Area Names.
        moStudentBL = new StudentBL();

        if(miSchoolId == Constants.SchoolId.SNS.ToInt())
            ListSource.FillDropDownList(moStudentBL.GetFeeAreaNames(miSchoolId), cmbAdditionalFeeAreaName, "FeeAreaName", "FeeAreaNameId", Constants.S_SELECT);

        if (oDsMaster.Tables[2].Rows[0][0] != DBNull.Value)
        {
            hidRegPrefix.Value = Convert.ToString(oDsMaster.Tables[2].Rows[0][0]);
            hidRegPrefixOrgnl.Value = Convert.ToString(oDsMaster.Tables[2].Rows[0][0]);
        }

        if (oDsMaster.Tables[2].Rows[0][1] != DBNull.Value)
            hidAllRegPrefixes.Value = Convert.ToString(oDsMaster.Tables[2].Rows[0][1]);

        string sPrefixTooltip = string.Empty, sPostfixTooltip = string.Empty;
        if (QueryString["NewMode"] != null && QueryString["NewMode"] == Constants.S_YES)
            sPrefixTooltip = "Valid Prefix :" + (hidRegPrefix.Value.Trim() == string.Empty ? "No Prefix" : hidRegPrefix.Value.Trim());
        else
            sPrefixTooltip = "Valid Prefix(s) :" + hidAllRegPrefixes.Value.Replace("NULL", "No Prefix");

        if (oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count > 0)
        {
            for (int i = 0; i < oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows.Count; i++)
            {
                oStringBuilderPostfix.Append(hidRegPostfix.Value + ',' + Convert.ToString(oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows[i][0]));


                oStringBuilderOrgnl.Append(hidRegPostfixOrgnl.Value + ',' + Convert.ToString(oDsMaster.Tables[I_REGNOPOSTFIX_COLUMN_INDEX].Rows[i][0]));

            }
            if (oStringBuilderPostfix.ToString().StartsWith(hidRegPostfix.Value))
                hidRegPostfix.Value = oStringBuilderPostfix.ToString().Substring(1);
            else
                hidRegPostfix.Value = oStringBuilderPostfix.ToString();
            if (oStringBuilderOrgnl.ToString().StartsWith(hidRegPostfix.Value))
                hidRegPostfixOrgnl.Value = oStringBuilderOrgnl.ToString().Substring(1);
            else
                hidRegPostfixOrgnl.Value = oStringBuilderOrgnl.ToString();

            if (hidRegPostfixOrgnl.Value.StartsWith(","))
                hidRegPostfixOrgnl.Value = hidRegPostfixOrgnl.Value.Substring(1);

            if (hidRegPostfixOrgnl.Value.Length > 0)
                sPostfixTooltip = "Valid Postfix(s) :" + hidRegPostfixOrgnl.Value;
        }

        if (sPostfixTooltip != string.Empty)
            imgPrefixes.Attributes.Add("title", sPrefixTooltip + "\n" + sPostfixTooltip);
        else
            imgPrefixes.Attributes.Add("title", sPrefixTooltip);


        if (hidRegPostfix.Value.Contains("/"))
            hidRegPostfix.Value = hidRegPostfix.Value.Replace("/", @"\/");

        if (hidRegPrefix.Value.Contains("/"))
            hidRegPrefix.Value = hidRegPrefix.Value.Replace("/", @"\/");


        if (Settings.IsConcessionApplicable)
        {
            trCheckStaffKid.Visible = true;
            trApplicableRule.Visible = true;
            ControlUtility.FillDropDownList(oDsMaster.Tables[3], ref ddlFeeRule,
                                         "Rule_Id",
                                         "RuleName",
                                         Constants.S_SELECT);
        }
        else
        {
            trCheckStaffKid.Visible = false;
            trApplicableRule.Visible = false;
        }
        
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataRow[] drRoles = oMasterDataCollectionBL.GetRolesWithoutParent();
        DataTable dtUserRoles = drRoles.CopyToDataTable().Select("User_role_Id in (2,6)").CopyToDataTable();

        ControlUtility.FillDropDownList(dtUserRoles, ref ddlUserRole,
                                                                    "User_Role_Id",
                                                                    "User_Role_Name",
                                                                    Constants.S_SELECT);


        ddlUserRole_SelectedIndexChanged(ddlUserRole, null);

        DataRow[] drSecndLang = oDsMaster.Tables[4].Select("LanguageGroupId=" + Constants.LanguageMode.SecondLanguage.ToInt());
        if (drSecndLang.Length > 0)
        {
            ControlUtility.FillDropDownList(drSecndLang.CopyToDataTable(), ref ddlSecondLanguage,
                                                "Subject_Id",
                                                "Subject_Name",
                                                 Constants.S_SELECT);
        }
        else
            ddlSecondLanguage.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });

        ddlSecondLanguage.Enabled = drSecndLang.Length > 1;


        DataRow[] drThirdLang = oDsMaster.Tables[4].Select("LanguageGroupId=" + Constants.LanguageMode.ThirdLanguage.ToInt());
        if (drThirdLang.Length > 0)
        {
            ControlUtility.FillDropDownList(drThirdLang.CopyToDataTable(), ref cmbThirdLanguage,
                                                "Subject_Id",
                                                "Subject_Name",
                                                 Constants.S_SELECT);
        }
        else
        {
            if (drSecndLang.Length > 0)
            {
                ControlUtility.FillDropDownList(drSecndLang.CopyToDataTable(), ref cmbThirdLanguage,
                                               "Subject_Id",
                                               "Subject_Name",
                                                Constants.S_SELECT);
            }
            else
                cmbThirdLanguage.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        }

        cmbThirdLanguage.Enabled = drThirdLang.Length > 1;

        StringBuilder oStringBuilder = new StringBuilder();
        foreach(DataRow dr in oDsMaster.Tables[4].Rows)
        {
            oStringBuilder.Append("$" + dr["Subject_Id"].ToInt() + "," + dr["SubjectGroupId"].ToInt());
        }

        if (oStringBuilder.Length > 0)
            hidSubjectGroupIds.Value = oStringBuilder.ToString().Substring(1);
        else
            hidSubjectGroupIds.Value = string.Empty;


        if (Settings.IsRTEApplicable)
        {
            ControlUtility.FillDropDownList(oDsMaster.Tables[6], ref cmbRTECategory,
                                         "Id",
                                         "CategoryName",
                                         Constants.S_SELECT);


            if (QueryString["NewMode"] != null && QueryString["NewMode"].ToString() == Constants.S_YES && miSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                ListItem oListItem = cmbRTECategory.Items.FindByText("Disadvantage Group / Weaker Section");
                if (oListItem != null)
                    cmbRTECategory.Items.Remove(oListItem);
            }

            txtRTEApplicationForm.Text = moStudentBL.sFormNo;

        }
        else
        {
            cmbRTECategory.Enabled = false;
            cmbRTECategory.SelectedValue = Constants.S_ZERO;
            txtRTEApplicationForm.Enabled = false;
            txtAnnualIncome.Enabled = false;
        }

        if (miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
        {
            trAdmissionFor.Visible = true;
            ControlUtility.FillDropDownList(oDsMaster.Tables[8], ref cmbAdmissionFor,
                                         "Id",
                                         "AdmissionFor",
                                         string.Empty);
            //cmbAdmissionFor.SelectedValue = Constants.S_ONE;
        }
        else
            trAdmissionFor.Visible = false;

        if (SchoolBase.Settings.IsAaryanSchool)
        {
            hidIsAaryanSchool.Value = Constants.S_YES;
            trFeeCategory.Visible = true;
            ControlUtility.FillDropDownList(oDsMaster.Tables[9], ref cmbFeeCategory,
                                         "Id",
                                         "Name",
                                         Constants.S_SELECT);
        }
        else
        {
            hidIsAaryanSchool.Value = Constants.S_NO;
            trFeeCategory.Visible = false;
        }

        oStringBuilder.Clear();

        DataTable dtData = oDsMaster.Tables[Constants.I_FOUR];
        if (dtData.Rows.Count > 0 && miSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            if (dtData.AsEnumerable().Any(s => s.Field<int>("SecondThirdId") != 0))
            {
                dtData.AsEnumerable().Where(s => s.Field<int>("SecondThirdId") == 0).ToList().ForEach
                    (
                        sub =>
                        {
                            var thirdLang = dtData.AsEnumerable().Where(s => s.Field<int>("SubjectGroupId") == sub.Field<int>("SubjectGroupId") && s.Field<int>("Subject_Id") != sub.Field<int>("Subject_Id")).FirstOrDefault();
                            var ss = dtData.AsEnumerable().Where(s => s.Field<int>("LanguageGroupId") == thirdLang.Field<int>("LanguageGroupId") && s.Field<int>("Subject_Id") != thirdLang.Field<int>("Subject_Id")).FirstOrDefault();
                            oStringBuilder.Append("$" + sub.Field<int>("Subject_Id") + "," + ss.Field<int>("Subject_Id"));
                        }
                    );
                hidSectionId.Value = Constants.S_NO;

                if (oStringBuilder.Length > 0)
                    hidLanguageGroupIds.Value = oStringBuilder.ToString().Substring(1);
                else
                    hidLanguageGroupIds.Value = string.Empty;
            }
            else
            {
                var subjects = dtData.AsEnumerable().Select(s => s.Field<int>("Subject_Id")).Distinct().ToList();
                hidLanguageGroupIds.Value = subjects[0] + "," + subjects[1];
                hidSectionId.Value = Constants.S_YES;
            }
        }
        ControlUtility.FillDropDownList(oDsMaster.Tables[10], ref cmbResidenceType,
                                   "ResidenceTypeId",
                                   "Name",
                                   Constants.S_SELECT);

        //ddlSecondLanguage.Attributes.Add("onchange", "if(!ChangeSecondAndThirdLanguage(" + Constants.I_ONE + ") return false;");
        //cmbThirdLanguage.Attributes.Add("onchange", "if(!ChangeSecondAndThirdLanguage(" + Constants.I_TWO + ") return false;");
        
    }

    /// <summary>             
    /// This method is used to fill document which are neet to submit while taking admission listview.
    /// </summary>
    private void FillAdmissionDocumetListView()
    {
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId, miAcademicYearId);

        lstvwConfiguredDocument.DataSource = oStandardwiseDocumentMasterBL.GetAdmissionDocumentDetails(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidStudentId.Value));
        lstvwConfiguredDocument.DataBind();
    }

    /// <summary>
    /// This method is used to Initialize        
    /// </summary>
    private void Initialize()
    {
        btnAddSiblingDetails.Attributes.Add("onclick", "ShowSiblingDetails(); return false;");
        string sStandardName = string.Empty;
        string sDivisionName = string.Empty;
        string sClassName = string.Empty;
       
        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            lblMobilePhoneNo.Text = "Mother Number";
            lblMobilePhoneNo2.Text = "Father Number";
        }

        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            lblMobilePhoneNo.Text = "Father Number";
            lblMobilePhoneNo2.Text = "Mother Number";
        }

        if (moUserRole != Constants.UserRoles.Admin)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student).ToString();

        txtCalDobPopup.Attributes.Add("onchange", "SetDate()");

        txtFormNo.Focus();
        txtNationality.Text = "Indian"; 

        hidIsPPSN.Value = Constants.S_NO;
        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            hidIsPPSN.Value = Constants.S_YES;
            colpnlLastSchoolDetails.Collapsed = false;
            colpnlPhotoGallery.Collapsed = false;
            spnRegNo.Visible = false;
            spnFormNo.Visible = true;            
        }
        else
        {
            spnRegNo.Visible = true;
            spnFormNo.Visible = false;
        }

        FillAllComboBoxes();
       
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        if (QueryString["StudentId"] != null)
        {
            hidStudentId.Value = QueryString["StudentId"];
            trNewAdd.Visible = false;
            trHasSibling.Visible = false;

        }
        else
        {
            trNewAdd.Visible = true;
            chkNewAddmission.Checked = true;
            hidStudentId.Value = Constants.S_ZERO;
            //CalDobPopup.DateValue = DateTime.Today;
            calAdmissionDate.DateValue = DateTime.Today;
            calJoingDate.DateValue = DateTime.Today;
            if (moUserRole == Constants.UserRoles.Admin || Boolean.Parse(hidUserHasFullAccess.Value))
                trHasSibling.Visible = true;
        }

        hidStandardId.Value = QueryString["StandardId"] ?? Constants.S_ZERO;        

        DataTable oDataTableDOB = StudentBL.GetStandardwiseDOBDetails(miSchoolId, hidStandardId.Value.ToInt());
        hidMinDOB.Value = DateTime.MinValue.ToString();
        hidMaxDOB.Value = DateTime.MaxValue.ToString();
        hidDOBConfirmationMsg.Value = string.Empty;

        if (oDataTableDOB.Rows.Count != 0)
        {
            DateTime dtStartDate = Convert.ToDateTime(oDataTableDOB.Rows[0]["START_DATE"]);
            DateTime dtEndDate = Convert.ToDateTime(oDataTableDOB.Rows[0]["END_DATE"]);
            hidStandardName.Value = Convert.ToString(oDataTableDOB.Rows[0]["Standard_Name"]);

            hidMinDOB.Value = dtStartDate.ToString();
            hidMaxDOB.Value = dtEndDate.ToString();

            if (!string.IsNullOrEmpty(dtStartDate.ToShortDateString()) && !string.IsNullOrEmpty(dtEndDate.ToShortDateString()))
            {
                hidDOBConfirmationMsg.Value = "Date of Birth of the student you entered is outside the standard date range for standard " + hidStandardName.Value + " which is " + dtStartDate.ToString(Constants.S_DATE_FORMAT) + " to " + dtEndDate.ToString(Constants.S_DATE_FORMAT) + ". Do you want to continue anyway?";
            }

            if (!string.IsNullOrEmpty(dtStartDate.ToString(Constants.S_DATE_FORMAT)) && string.IsNullOrEmpty(dtEndDate.ToString(Constants.S_DATE_FORMAT)))
            {
                hidDOBConfirmationMsg.Value = "Date of birth should be after " + dtStartDate.ToShortDateString() + ". Do you want to continue anyway?";
            }

            if (string.IsNullOrEmpty(dtStartDate.ToString(Constants.S_DATE_FORMAT)) && !string.IsNullOrEmpty(dtEndDate.ToString(Constants.S_DATE_FORMAT)))
            {
                hidDOBConfirmationMsg.Value = "Date of birth should be before " + dtStartDate.ToString(Constants.S_DATE_FORMAT) + ". Do you want to continue anyway?";
            }
        }


        if (QueryString["abIsExactMatch"] != null)
            hidIsExactMatch.Value = QueryString["abIsExactMatch"];

        if (QueryString["asOperator"] != null)
            hidOperator.Value = QueryString["asOperator"];

        if (QueryString["asPrefix"] != null)
            hidPrefix.Value = QueryString["asPrefix"];

        if (QueryString["asPostfix"] != null)
            hidPostfix.Value = QueryString["asPostfix"];

        if (QueryString["SearchedNumber"] != null)
            hidRegNo.Value = QueryString["SearchedNumber"];

        if (QueryString["Is_SuperAdmin"] != null)
            hidIsSuperAdmin.Value = QueryString["Is_SuperAdmin"];
        // ?? - operator returns the left-hand operand if it is not null, or else it returns the right operand.
        if (QueryString["DivisionId"] != null)
            hidDivisionId.Value = QueryString["DivisionId"] ?? "0";
        if (QueryString["standardName"] != null)
            sStandardName = QueryString["standardName"] ?? "";

        if (QueryString["DivisionName"] != null)
            sDivisionName = QueryString["DivisionName"] ?? "";

        if (QueryString["ClassId"] != null)
            hidClassId.Value = QueryString["ClassId"] ?? Constants.I_ZERO.ToString();

        hidIsConfig.Value = QueryString["Is_Configured"];
        hidSchoolId.Value = Session["I_SCHOOL_ID"].ToString();

        if (QueryString["NewMode"] != null && QueryString["NewMode"] == Constants.S_YES)
        {
            hidMode.Value = "new";
            lblStandard.Text = Resources.LocalizedResources.Class + " : (" + sStandardName + " - " + sDivisionName + ")";
            txtCity.Text = Constants.S_DEFAULT_CITY;
            txtState.Text = Settings.DefaultStudentState;
            imgPhoto.Src = S_DEFAULT_PHOTO;
            DataTable oDataTable = StudentBL.GetNextStudentRollNoAndLogin(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidDivisionId.Value), Convert.ToInt32(hidSchoolId.Value));
            if (moUserRole == Constants.UserRoles.Admin || Boolean.Parse(hidUserHasFullAccess.Value))
                btnAddSiblingDetails.Visible = true;

            if (miSchoolId == Constants.SchoolId.SVNP.ToInt() && Convert.ToString(oDataTable.Rows[0]["LoginId"]) == Constants.S_ZERO)
                txtLoginId.Text = "-";
            else
                txtLoginId.Text = Convert.ToString(oDataTable.Rows[0]["LoginId"]);

            txtRollNumber.Text = Convert.ToString(oDataTable.Rows[0]["RollNo"]);
            txtRegNo.Text = Convert.ToString(oDataTable.Rows[0]["RegistrationNo"]);
            txtRollNumber.Enabled = false;
            spnRollMandatory.Visible = false;
            hidDefaultRollNo.Value = Convert.ToString(oDataTable.Rows[0]["RollNo"]);
            ListItem oListItem = cmbCategory.Items.FindByText("Not Available");
            oListItem.Selected = true;
            if (Settings.IsRTEApplicable)
            {
                trRTE.Visible = true;
                trRTENote.Visible = true;

                if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                    hidRestrictAnnualIncomeForRTE.Value = Constants.S_ONE;
                else
                    hidRestrictAnnualIncomeForRTE.Value = Constants.S_ZERO;

                chkIsRTEApplicable.Attributes.Add("Onclick", "EnableDisabledStaffKid()");
                cmbRTECategory.Attributes.Add("onchange", "EnableDisableCategoryCombo()");
                txtRTEApplicationForm.Text = moStudentBL.RTEFormNo;
                txtAnnualIncome.Text = moStudentBL.AnnualIncome.ToString();
            
            }
            else
            {
                trRTE.Visible = false;
                trRTENote.Visible = false;

            }


            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                if (QueryString["standardName"] != null && QueryString["standardName"].ToString() != string.Empty)
                {
                    string sStdName = QueryString["standardName"].ToString();

                    if (sStdName.Contains("11 ") || sStdName.Contains("12 "))
                    {
                        TrStreamDetails.Visible = true;
                     
                        if (lblStandard.Text.Contains("Sci"))
                            ddlStream.SelectedValue = "1";
                        else if (lblStandard.Text.Contains("Com"))
                            ddlStream.SelectedValue = "2";
                        else if (lblStandard.Text.Contains("Art"))
                            ddlStream.SelectedValue = "3";

                        ddlStream_SelectedIndexChanged(ddlStream, null);
                        if (ddlGroup.Items.Count > 0)
                        {
                            ddlGroup.SelectedIndex = 1;
                            ddlGroup_SelectedIndexChanged(ddlGroup, null);
                        }
                    }
                }
            }

        }
        else
        {
            hidMode.Value = "EDIT";
            if (QueryString["ClassName"] != null)
                sClassName = QueryString["ClassName"];

            BindEntryFields(Convert.ToInt32(hidSchoolId.Value), Convert.ToInt32(hidStudentId.Value), miCurrentAcademicYearId);

            if (QueryString["StudentId"] != null)
                SetIdentitylinkURL();

            lblStandard.Text = Resources.LocalizedResources.Class + ": (" + sClassName + ")";
            btnSaveNext.Visible = false;

            if (moUserRole == Constants.UserRoles.Admin || Boolean.Parse(hidUserHasFullAccess.Value))
                btnAddSiblingDetails.Visible = true;
            string sLeaveMessage = StudentFeeDetailsBL.IsOnLeave(Convert.ToInt32(hidYearWiseStudentId.Value), miSchoolId, miAcademicYearId);
            if (sLeaveMessage != "0")
            {
                lblLeaveMessage.Visible = true;
                lblLeaveMessage.Text = sLeaveMessage;
            }
            else
                lblLeaveMessage.Visible = false;

            CheckExamPublishStatus();
        }
        hidOldJoiningDate.Value = calJoingDate.DateValue.ToString("dd-MMM-yyyy", new CultureInfo("en"));
        CheckIfAttendanceMarked(calJoingDate.DateValue);

        if (!QueryString["IsSchoolLeft"].IsNullOrEmpty() && hidIsSuperAdmin.Value != Constants.S_YES)
            SetControlDisable(false);

        if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && moUserRole == Constants.UserRoles.Teacher)
        {
            btnSave.Enabled = false;
            btnSaveNext.Enabled = false;
        }
        
        if (Settings.IsRTEApplicable)
        {
            chkIsRTEApplicable.Attributes.Add("Onclick", "EnableDisabledStaffKid()");
            cmbRTECategory.Attributes.Add("onchange", "EnableDisableCategoryCombo()");
        }

        FillAdmissionDocumetListView();
    }

    /// <summary>
    /// This method is used to set contol enability when edit Left Student.
    /// </summary>
    /// <param name="abFlag"></param>
    public void SetControlDisable(bool abFlag)
    {
        btnRemovePhoto.Enabled = abFlag;
        btnSave.Enabled = abFlag;
        btnAddSiblingDetails.Enabled = abFlag;
        FileUploadLogo.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used to SetAdditionalFields
    /// </summary>
    private void SetAdditionalFields()
    {
        AdditionalFieldsBL oAdditionalFieldsBL = new AdditionalFieldsBL();
        int iAdditionalFildsCount = oAdditionalFieldsBL.FillAdditionalFields(tblAdditionalInformation, Convert.ToInt32(Constants.Screen.StudentUI));
        tblAdditionalInformation.Visible = colpnlAdditionalInfo.Visible = iAdditionalFildsCount > 0;
        hidIsAdditionalInformationAvailable.Value = (iAdditionalFildsCount > 0) ? Constants.S_YES : Constants.S_NO;
    }

    /// <summary>
    /// This method is used to check if attendance is marked before the given date or not.
    /// </summary>
    /// <returns></returns>
    private void CheckIfAttendanceMarked(DateTime aoDateTime)
    {
        int iDivisionId = Convert.ToInt32(hidDivisionId.Value);
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
        Boolean bMarked = oSchoolWiseAttendanceDetailsBL.CheckIfAttendanceMarked(aoDateTime, iStandardId, iDivisionId);
        hidHasAttendance.Value = bMarked.ToString();
    }

    /// <summary>
    /// This method is used to check precondition for a standard 
    /// ie fee configuration for particular standard is set or not. 
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;

        HidBackUrl.Value = Server.UrlDecode(Request.QueryString.ToString());

        if (QueryString["StandardId"] != null)
            hidStandardId.Value = QueryString["StandardId"];

        if(QueryString["DivisionId"] != null)
            hidDivisionId.Value = QueryString["DivisionId"];



        string sLinks = ReferenceBL.GetStudentUIPreConditionMsg(Convert.ToInt32(hidStandardId.Value));

        if (sLinks.Equals("") || QueryString["FromLeftStudentScreen"] == Constants.S_YES)
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            tblStudentInfo.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to set academic year as per academic year combo box filter.
    /// </summary>
    private void SetAcademicYearValue()
    {
        colpnlStudentAdditionalDetails.Visible = SchoolBase.Settings.IsAdditionalFieldsApplicable;

        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            tdlblFeeArea.Visible = true;
            tdCmbFeeArea.Visible = true;
        }
        else
        {
            tdlblFeeArea.Visible = false;
            tdCmbFeeArea.Visible = false;
        }

        if (QueryString["FromLeftStudentScreen"] == Constants.S_YES)
            miCurrentAcademicYearId = QueryString["AcademicYearId"].ToInt();
        else
            miCurrentAcademicYearId = miAcademicYearId;
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetIdentitylinkURL()
    {
        string sQueryString = "iStandardId=0" + "&iDivisionId=0" + "&iStudentId=" + hidYearWiseStudentId.Value;
        sQueryString = "../Teacher/StudentIdentityCards.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
    }
    /// <summary>
    /// This method is used to set the StudentId & GRNumber
    /// </summary>
    private void SetStudentIdWithGRNumber()
    {
        if (miSchoolId == Constants.SchoolId.JOS.ToInt())
        {
            trStudentId.Visible = true;
            trGRNumber.Visible = true;
        }
        else
        {
            trStudentId.Visible = false;
            trGRNumber.Visible = false;
        }       
    }
    /// <summary>
    /// This method is used to SetQueryString To Add
    /// </summary>
    private void SetQueryStringToAdd()
    {
        StringBuilder sQueryString = new StringBuilder();
        sQueryString.AppendFormat("StandardId={0}", Convert.ToInt32(hidStandardId.Value == string.Empty ? "0" : hidStandardId.Value));
        sQueryString.AppendFormat("&DivisionId={0}", Convert.ToInt32(hidDivisionId.Value == string.Empty ? "0" : hidDivisionId.Value));
        sQueryString.AppendFormat("&StudentId={0}", Convert.ToInt32(hidYearWiseStudentId.Value == string.Empty ? "0" : hidYearWiseStudentId.Value));
        sQueryString.AppendFormat("&abIsExactMatch={0}", hidIsExactMatch.Value);
        if (hidIsExactMatch.Value.ToBool())
            sQueryString.AppendFormat("&RegNo={0}", hidRegNo.Value);
        else
            sQueryString.AppendFormat("&NameOrRegNo={0}", hidRegNo.Value);
        sQueryString.AppendFormat("&asOperator={0}", hidOperator.Value);
        sQueryString.AppendFormat("&asPrefix={0}", hidPrefix.Value);
        sQueryString.AppendFormat("&asPostfix={0}", hidPostfix.Value);
        sQueryString.AppendFormat("&SearchedNumber={0}", hidRegNo.Value);
        sQueryString.AppendFormat("&Is_SuperAdmin={0}", hidIsSuperAdmin.Value);
        if (hidSiblingStudentId.Value != string.Empty)
            sQueryString.AppendFormat("&SiblingStudentId={0}", hidSiblingStudentId.Value);
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
        hidEncryptedString.Value = CommonUtility.EncryptQuerystring(sQueryString.ToString());
    }

    /// <summary>
    /// This method is used to send sms about mobile site.
    /// </summary>
    /// <param name="aoSchoolBL"></param>
    /// <param name="asDisplayText"></param>
    private void SendMobileDetailsSMS(SchoolBL aoSchoolBL, string asDisplayText)
    {
        string sMobileSmsTemplate = string.Empty;
        int iTemplateId = Constants.SMSTemplate.MobileWebsiteDetailsSMS.ToInt();
        string sTemplateRegistrationId = string.Empty; //
        DataTable oDTMobileSMSTemplate = SmsTemplateBL.GetTemplate(iTemplateId, miSchoolId);

        if (oDTMobileSMSTemplate.IsNonEmpty())
        {
            if (oDTMobileSMSTemplate.Rows[0][I_SMS_TEMPLATE_TXT] != DBNull.Value)
                sMobileSmsTemplate = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_TEMPLATE_TXT]);

            if (oDTMobileSMSTemplate.Rows[0][2] != DBNull.Value)
            {
                string sSmsSubject = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_SUBJECT_TXT]);
                if (oDTMobileSMSTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTMobileSMSTemplate.Rows[0]["TemplateRegistrationId"].ToString(); 
                SMS oSms = new SMS();
                oSms.Sender = aoSchoolBL.SMSSenderName;
                oSms.SMSText = sMobileSmsTemplate.Replace("%WEBSITE%", ConfigurationManager.AppSettings["MobileUrl"].Replace(S_REPLACE_URL, string.Empty));
                oSms.DisplayText = asDisplayText;
                oSms.SMSType = oDTMobileSMSTemplate.Rows[0][I_SMS_TYPE].ToInt();
                oSms.School_Name = aoSchoolBL.SchoolName + "::" + sSmsSubject;
                oSms.To.Add(moSchoolUserBL.UserId, txtMobilePhoneNumber.Text);
                oSms.TemplateRegistrationId = sTemplateRegistrationId;
                if (!string.IsNullOrEmpty(txtMobilePhoneNumber2.Text))
                    oSms.To.Add(moSchoolUserBL.UserId + "sm;", txtMobilePhoneNumber2.Text);
                oSms.Send();
            }
        }
    }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache()
    {
        try
        {
            int iYearwiseStudentId;
            Constants.Action oAction;
            if (hidMode.Value == "new")
            {
                iYearwiseStudentId = hidYearWiseStudentId.Value.ToInt();
                oAction = Constants.Action.Insert;
            }
            else
            {
                iYearwiseStudentId = moStudentBL.YearWiseStudentId;
                oAction = Constants.Action.Update;
            }
            List<int> lstYearwiseStudentIds = new List<int>() { iYearwiseStudentId };
            AutoSearchService oAutoSearchService = new AutoSearchService();
            oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, lstYearwiseStudentIds, oAction);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    private void RefreshValue()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidRegNumberBlank.Value = Resources.LocalizedResources.RegNumberBlank;
        hidRegNumberEndWith.Value = Resources.LocalizedResources.RegNumberEndWith;
        hidRegNumberIsNumber.Value = Resources.LocalizedResources.RegNumberIsNumber;
        hidRegNumberFormat.Value = Resources.LocalizedResources.RegNumberFormat;
        hidRegNumberStartWith.Value = Resources.LocalizedResources.RegNumberStartWith;
        hidRegNumberZeroValidation.Value = Resources.LocalizedResources.RegNumberZeroValidation;
        hidAtLeastOneSibling.Value = Resources.LocalizedResources.AtLeastOneSibling;
        hidReplaceStudentWithSibling.Value = Resources.LocalizedResources.ReplaceStudentWithSibling;
        hidRemoveThisPhoto.Value = Resources.LocalizedResources.RemoveThisPhoto;
        hidRTEStudentSelected.Value = Resources.LocalizedResources.RTEStudentSelected;
        hidRTECategorySelected.Value = Resources.LocalizedResources.RTECategorySelected;
        hidInvalidFileFormat.Value = Resources.LocalizedResources.InvalidFileFormat;
        hidSchoolLogo.Value = Resources.LocalizedResources.SchoolLogo;

        hidPinCodeDigit.Value = Resources.LocalizedResources.PinCodeDigit;
        hidPinBlank.Value = Resources.LocalizedResources.PinBlank;
        hidMobileNumberBlank.Value = Resources.LocalizedResources.MobileNumberBlank;
        hidMobileNumber1and2Blank.Value = Resources.LocalizedResources.MobileNumber1and2Validation;
        hidMotherNumberZero.Value = Resources.LocalizedResources.MotherNumberZero;
        hidFatherNumberZero.Value = Resources.LocalizedResources.FatherNumberZero;
        hidMobileNumber1Zero.Value = Resources.LocalizedResources.MobileNumber1Zero;
        hidMobileNumber2Zero.Value = Resources.LocalizedResources.MobileNumber2Zero;
        hidRollNumberBlank.Value = Resources.LocalizedResources.RollNumberBlank;
        hidRollNumberZero.Value = Resources.LocalizedResources.RollNumberZero;
        hidMobileDigit.Value = Resources.LocalizedResources.MobileDigit;
        hidMotherNumberDigit.Value = Resources.LocalizedResources.MotherNumberDigit;
        hidFatherNumberDigit.Value = Resources.LocalizedResources.FatherNumberDigit;
        hidMobileNumber2Digit.Value = Resources.LocalizedResources.MobileNumber2Validation;

        hidAttendanceValidation.Value = Resources.LocalizedResources.AttendanceValidation;
        hidSyatemTillDateAttendance.Value = Resources.LocalizedResources.SyatemTillDateAttendance;
        hidSystemAttendance.Value = Resources.LocalizedResources.AlertMarkAttendance;
        hidDateOfJoiningValidation.Value = Resources.LocalizedResources.DateOfJoiningValidation;
        hidDateOfJoining.Value = Resources.LocalizedResources.DateOfJoining;
        hidDateOfAdmission.Value = Resources.LocalizedResources.DateOfAdmission;
        hidDateOfBirthGreaterValidation.Value = Resources.LocalizedResources.DateOfBirthGreaterValidation;
        hidUploadDocumet.Value = Resources.LocalizedResources.UploadDocument;
        hidChangeDetails.Value = Resources.LocalizedResources.ChangeDetails;
        hidReplaceDetails.Value = Resources.LocalizedResources.ReplaceDetails;
        hidEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        hidAdditionalFatherEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        hidAdditionalMotherEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        hidValMotherName.Value = Resources.LocalizedResources.ValBlankMotherName;
        hidValCaste.Value = Resources.LocalizedResources.ValBlankCaste;
        hidValBirthPlace.Value = Resources.LocalizedResources.ValBlankBirthPlace;
        hidValNationality.Value = Resources.LocalizedResources.ValBlankNationality;
        hidValMotherToungue.Value = Resources.LocalizedResources.ValBlankMotherTongue;
        hidValLastSchoolName.Value = Resources.LocalizedResources.ValBlankLastSchoolName;
    }

    private void fillsiblingslistview()
    {
        StudentSiblingDetailsBL oStudentSiblingDetailsBL = new StudentSiblingDetailsBL();
        List<SiblingInfo> olstStudentSibligInfo = new List<SiblingInfo>();
        olstStudentSibligInfo = oStudentSiblingDetailsBL.GetStudentSiblingData();
        lstvwSiblingsDetails.DataSource = olstStudentSibligInfo;
        lstvwSiblingsDetails.DataBind();
        SetHeaderCheckbox();
    }

    /// <summary>
    /// To check header check box while showing list of Sibling details
    /// </summary>
    private void SetHeaderCheckbox()
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwSiblingsDetails.FindControl("trHeader");
        CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkSelectAllSiblings");
        oCheckBox.Checked = true;
    }

    /// <summary>
    /// This method is sued to set mandatory fields.
    /// </summary>
    private void SetMandatoryFields()
    {
        StudentBL oStudentBL = new StudentBL();
        string sMandatoryFields = oStudentBL.GetStudentMandatoryFields(miSchoolId);
        string[] sArrfields = sMandatoryFields.Split(',');
        hidMandatoryFields.Value = sMandatoryFields;
        foreach (string sField in sArrfields)
        {
            Label oLabel = new Label();
            oLabel.Text = "*";
            oLabel.EnableViewState = true;
            oLabel.CssClass = "ClsMdtStar";

            switch (sField)
            {
                case "txtMotherName": tdMotherName.Controls.Add(oLabel); break;
                case "txtCasteAndSubcaste": tdCastSubcast.Controls.Add(oLabel); break;
                case "txtBirthPlace": tdBirthPlace.Controls.Add(oLabel); break;
                case "txtNationality": tdNationality.Controls.Add(oLabel); break;
                case "txtMotherTongue": tdMotherTongue.Controls.Add(oLabel); break;
                case "txtLastSchoolName": tdLastSchoolName.Controls.Add(oLabel); break;
            }
        }

    }

    private void SetAchievementLink()
    {
        string sQueryString = "SchoolWiseStudentId=" + hidStudentId.Value;
        if (hidStudentId.Value != Constants.S_ZERO)
            btnAddAchievement.Visible = true;
        hidQueryValue.Value = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        btnAddAchievement.Attributes.Add("onclick", "ShowAchievementDetails();return false;");
    }

    /// <summary>
    /// This method is used to set Current Date.
    /// </summary>
    private void SetCurrentDate()
    {
        if (SchoolBase.Settings.CompareAgeTillDate.ToString() != string.Empty)
        {
            //var dt = SchoolBase.Settings.CompareAgeTillDate.ToDateTime();
            //DateTime newDT = new DateTime(DateTime.Now.Year,dt.Month,dt.Day);
            hidCurrentDate.Value = Settings.CompareAgeTillDate.ToString();
        }
        else
            hidCurrentDate.Value = Convert.ToString(DateTime.Now);
    }

    /// <summary>
    /// This method is used to check exam status.
    /// </summary>
    private void CheckExamPublishStatus()
    {
        SecondLanguageBL oSecondLanguageBL = new SecondLanguageBL(miSchoolId, miAcademicYearId);
        if(oSecondLanguageBL.IsAnyExamPublished(hidStandardId.Value.ToInt(), hidDivisionId.Value.ToInt()))
        {
            ddlSecondLanguage.Enabled = false;
            cmbThirdLanguage.Enabled = false;
        }
    }

    private string CheckIsAdditionalPhotosUploaded(FileUpload oFileUploadControl, int iValue)
    {
        if (oFileUploadControl.FileName != string.Empty)
        {
            string sLinkFamilyName = string.Empty;

            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                if (iValue == Constants.I_ONE)
                {
                    if (FUAdditionalFatherPhoto.HasFile)
                        SaveFileOnServer(FUAdditionalFatherPhoto.FileName, FUAdditionalFatherPhoto);
                }
                if (iValue == Constants.I_TWO)
                {
                    if (fuAdditionalMotherPhoto.HasFile)
                        SaveFileOnServer(FUAdditionalFatherPhoto.FileName, fuAdditionalMotherPhoto);
                }
                if (iValue == Constants.I_THREE)
                {
                    if (FUAdditionalGuardianPhoto.HasFile)
                        SaveFileOnServer(FUAdditionalFatherPhoto.FileName, FUAdditionalGuardianPhoto);
                }

                // Father Aadhaar
                if (iValue == Constants.I_FOUR)
                {
                    if (flUploadFatherAaadhar.HasFile)
                    {
                        string sServerPath1 = Server.MapPath("~");
                        if (!sServerPath1.EndsWith("\\")) sServerPath1 += "\\";

                        string sName = "Father_" + CommonUtility.GetFileNameForRenaming(flUploadFatherAaadhar.FileName);
                        string sPath = sServerPath1 + S_Parent_AadharCardPhoto + sName;

                        flUploadFatherAaadhar.SaveAs(sPath);
                        return sName;
                    }

                    return hidFatherAadharCardFileName.Value;
                }
                 // Mother Aadhaar
                if (iValue == Constants.I_FIVE)
                {
                    if (flUploadMotherAaadhar.HasFile)
                    {
                        string sServerPath1 = Server.MapPath("~");
                        if (!sServerPath1.EndsWith("\\")) sServerPath1 += "\\";

                        string sName = "Mother_" + CommonUtility.GetFileNameForRenaming(flUploadMotherAaadhar.FileName);
                        string sPath = sServerPath1 + S_Parent_AadharCardPhoto + sName;

                        flUploadMotherAaadhar.SaveAs(sPath);
                        return sName;
                    }
                   return hidMotherAadharCardFileName.Value;
                }
            }

            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";

            sLinkFamilyName = CommonUtility.GetFileNameForRenaming(oFileUploadControl.FileName.ToString());

            if (oFileUploadControl.HasFile)
            {
                string sFamilyName = oFileUploadControl.PostedFile.FileName;
                string sLinkFamilyPath = sServerPath + S_Parent_Photo + sLinkFamilyName;
                oFileUploadControl.SaveAs(sLinkFamilyPath);
                return sLinkFamilyName;
            }
        }

        if (oFileUploadControl.FileName == string.Empty)
        {
            if (iValue == Constants.I_ONE)
                return hidFatherPhoto.Value;
            else if (iValue == Constants.I_TWO)
                return hidMotherPhoto.Value;
            else if (iValue == Constants.I_THREE)
                return hidGuardianPhoto.Value;
            else if (iValue == Constants.I_FOUR)       
                return hidFatherAadharCardFileName.Value;
            else if (iValue == Constants.I_FIVE)      
                return hidMotherAadharCardFileName.Value;
        }

        return string.Empty;
    }
    /// <summary>
    /// This method is used save file on server 
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName, FileUpload oFileUpload)
    {
        // Upload the file to the server.
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }

        oFileUpload.SaveAs(sServerFilePath);
        string sErrMessage = ValidatePhotoFile(sServerFilePath);
        if (sErrMessage.Equals(""))
        {
            // delete exesting logo
            string sFileToDelete = Server.MapPath(".") + oFileUpload.FileName;
            if (File.Exists(sFileToDelete))
            {
                File.Delete(sFileToDelete);
            }
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
        return sFileName;
    }

    /// <summary>
    /// This function is used to ValidateFile
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <returns></returns>
    private string ValidatePhotoFile(string asServerFilePath)
    {
        string sReturnErrorMsg = "";
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
            {
                sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px " + Resources.LocalizedResources.And + " " + I_WIDTH_LIMIT + "px " + Resources.LocalizedResources.respectively;
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > I_HEIGHT_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
                if (oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
            }
            oFileStream.Close();
            oImg = null;

        }
        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal;
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }
    /// <summary>
    /// This method is used to fill stream combobox.
    /// </summary>
    private void FillStreamCombo()
    {
        
        MasterDataCollectionBL OmasterBl = new MasterDataCollectionBL();
        DataTable dtStream = OmasterBl.GetAllStreams();
        ControlUtility.FillDropDownList(dtStream, ref ddlStream, "Id", "Name", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill stream combobox.
    /// </summary>
    private void FillGroupCombo()
    {
        MasterDataCollectionBL oMasterDataBL = new MasterDataCollectionBL();
        DataTable dtGroups = oMasterDataBL.GetAllGroupsOfStream(ddlStream.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(dtGroups, ref ddlGroup, "Id", "GroupName", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill compulsary subjects of groups .
    /// </summary>
    /// 
    private void FillCompulsarySubjects()
    {
        MasterDataCollectionBL oMasterDataBL = new MasterDataCollectionBL();

        DataSet odataset =oMasterDataBL.GetAllCompulsarySubjects( ddlGroup.SelectedValue.ToInt(), miAcademicYearId);

       if (chkNewAddmission.Checked && miAcademicYearId >= 13 && ddlGroup.SelectedIndex > 0)
        {
            // OPTIONAL SUBJECTS
            DataTable dtOptionalSubjects = odataset.Tables[1];
           // SCIENCE - Hide Computer Science only for Group 2
            if (ddlStream.SelectedValue == "1" && ddlGroup.SelectedValue == "2")
            {
                DataRow[] drSubjects = dtOptionalSubjects.Select("SubjectName = 'Computer Science'");
                foreach (DataRow row in drSubjects)
                {
                    dtOptionalSubjects.Rows.Remove(row);
                }
            }
           // COMMERCE
            if (ddlStream.SelectedValue == "2")
            {
                // Hide Mathematics
                DataRow[] drMaths = dtOptionalSubjects.Select("SubjectName = 'Mathematics'");
                foreach (DataRow row in drMaths)
                {
                    dtOptionalSubjects.Rows.Remove(row);
                }

              }
            dtOptionalSubjects.AcceptChanges();

          // COMPETITIVE EXAMS
            DataTable dtCompetitiveExams = odataset.Tables[2];
          // ARTS STREAM - Hide all exams
            if (ddlStream.SelectedValue == "3")
            {
                dtCompetitiveExams.Rows.Clear();
                dtCompetitiveExams.AcceptChanges();
            }

           // ARTS STREAM
           if (ddlStream.SelectedValue == "3")
            {
                string sSubjects = Convert.ToString(  odataset.Tables[0] .Rows[0]["SubjectDetails"]);
               // Hide History
                sSubjects = sSubjects.Replace(",History", "");
                odataset.Tables[0].Rows[0]["SubjectDetails"] =sSubjects;
            }
        }
        else
        {
           DataTable dtOptionalSubjects =odataset.Tables[1];
              // COMMERCE
            if (ddlStream.SelectedValue == "2" && ddlGroup.SelectedValue == "3")
            {  // Hide Applied Mathematics
                DataRow[] drAppliedMaths = dtOptionalSubjects.Select("SubjectName in ( 'Applied Mathematics','Legal Studies')");

                foreach (DataRow row in drAppliedMaths)
                {
                    dtOptionalSubjects.Rows.Remove(row);
                }
             }

            dtOptionalSubjects.AcceptChanges();
          // COMPETITIVE EXAMS
          DataTable dtCompetitiveExams =odataset.Tables[2];
          DataRow[] drExams = dtCompetitiveExams.Select("ExamName = 'UG Entrance'");
          foreach (DataRow row in drExams)
            {
                dtCompetitiveExams.Rows.Remove(row);
            }

          // ARTS STREAM
          if (ddlStream.SelectedValue == "3")
            {
                string sSubjects = Convert.ToString(odataset.Tables[0].Rows[0]["SubjectDetails"]);
               // Hide Business Studies
                sSubjects = sSubjects.Replace( ",Business Studies", "");

                odataset.Tables[0] .Rows[0]["SubjectDetails"] =  sSubjects;
            }
        }
     
            StudentCompulsarySubjects(odataset.Tables[0]);
            FillOptionalSubjects( odataset.Tables[1]);
            FillCompitativeExams(odataset.Tables[2]);
           FillOptionalSubjectArts( odataset.Tables[3]);
       }
    /// <summary>
    /// This method is used to show optional subjects for Arts stream   .
    /// </summary>
    ///
    private void FillOptionalSubjectArts(DataTable oDataTable)
    {
        RadioOptionalSubjectArts.DataSource = oDataTable;
        RadioOptionalSubjectArts.DataTextField = "Subject_Name";
        RadioOptionalSubjectArts.DataValueField = "SubjectId";
        RadioOptionalSubjectArts.DataBind();
        RadioOptionalSubjectArts.Visible =  true;

    }
    /// <summary>
    /// This method is used to show compulsary subjects  .
    /// </summary>
    /// 
    private void StudentCompulsarySubjects(DataTable oDataTable)
    {
        if (oDataTable.IsNonEmpty())
        {
            DataRow oDataRow = oDataTable.Rows[0];
            lblCompulsarySubjects.Text  = Convert.ToString(oDataRow["SubjectDetails"]);
            hidCompulsorySubjects.Value = lblCompulsarySubjects.Text;/////
        }
    }
    /// <summary>
    /// This method is used to show Optional subjects  .
    /// </summary>
    /// 
    private void FillOptionalSubjects(DataTable oDataTable)
    {
       
            RadioOptionalSubjects.DataSource = oDataTable;
            RadioOptionalSubjects.DataTextField = "SubjectName";
            RadioOptionalSubjects.DataValueField = "SubjectId";
            RadioOptionalSubjects.DataBind();
    }
    /// <summary>
    /// This method is used to show Compitative Exams   .
    /// </summary>
    ///
    private void FillCompitativeExams(DataTable oDataTable)
    {
           chkCompitativeExams.DataSource = oDataTable;
            chkCompitativeExams.DataTextField = "ExamName";
            chkCompitativeExams.DataValueField = "Id";
            chkCompitativeExams.DataBind();
     }

    private void FillStaffs()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        DataTable dtUsers = oSchoolUserBL.GetAllUsers(ddlUserRole.SelectedValue.ToInt(), miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(dtUsers, ddlUserName, "UserName", "UserId", Constants.S_SELECT);
    }

    private void RenameHeaders()
    {
        if (miSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            lblUDISEnumber.Text = "Student Saral ID";
            lblsaralNo.Text = "Student National Code";
        }       
    }
    #endregion Private Methods        
}
