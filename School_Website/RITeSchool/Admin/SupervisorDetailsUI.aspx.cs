// File Name  : SupervisorDetailsUI.aspx.cs
// Created By : Ashish
// Date       : 05/12/2008
////Description : This class is used to add new Supervisor or modify existing one. 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SchoolEntities;
using PayrollEntities;
using Utility;
using System.Web;
using PhotoUploadEntities;
using SchoolAutoSearchService.Service;
using System.Resources;
using System.Globalization;
using System.Threading;

public partial class SupervisorDetailsUI : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    private const string S_IS_CONFIGURED = "Is_Configured";
    private const string S_USER_ID = "UserId";
    private const string S_SUPERVISOR_ID = "SupervisorId";
    private const string S_BACK_PAGE_URL = "~/Admin/SupervisorUserListUI.aspx";
    private const string S_SCREEN_LEVEL = "Screen_Level";
    private const string S_REPORT_NAME_FIELD = "Report_Display_Name";
    private const string S_REPORT_ID_FIELD = "Report_Id";
    private const string S_ACCOUNT = "Account";
    private const string S_QUALIFICATION_ID = "Qualification_Id";
    private const string S_SPECIALISATION = "Specialization";
    private const string S_QUALIFICATION = "Qualification_Name";
    private const string S_YEAR_OF_PASSING_ID = "Year_of_Passing";
    private const string S_PASSING_UNIVERSITY = "Passing_University";
    private const string S_CLASS_ID = "Class_Id";
    private const string S_CLASS_NAME = "Class_Name";
    private const string S_JOINING_DATE = "JoiningDate";
    private const string S_LEFT_DATE = "leftDate";
    private const string S_GRIDVIEW_DATASOURCE = " grdvwEducationDetails_DataSource";
    private const string S_LISTVIEW_EXPDETAILS = "lstvwExpDetails_DataSourceID";
    private const int I_DATAKEY_QUALIFICATION_ID = 0;
    private const string S_COMMAND_REMOVE = "REMOVE";
    private const string S_COMMAND_UPDATE = "Modify";
    private const string S_EDIT_MODE = "EDIT";
    private const string S_MODE_NEW = "NEW";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_TEXT_SAVE = "Save";
    private const string S_SCHOOLNAME = "SchoolName";
    private RetirementNoticeConfigBL moRetirementNoticeConfigBL;
    private OtherStaffBL moOtherStaffBL;
    private SchoolWiseSupervisorMasterBL moSchoolWiseSupervisorMasterBL;
    private int miOtherStaffUserId;
    private const string S_BASIC_DETAILS_MESSAGE = "Basic details of user should be added first.";
    private const string S_EXPERIANCE_DETAILS_SAVE_MESSAGE = "Experience details for user saved successfully !!!";
    private const string S_EDUCATION_DETAILS_SAVE_MESSAGE = "Educational details for user saved successfully !!!";
    private const string S_ADDITIONAL_DETAILS_SAVE_MESSAGE = "Additional details for user saved successfully !!!";
    private const string S_EXPERIANCE_DETAILS_DELETE_MESSAGE = "Experience details for user deleted successfully !!!";
    private const string S_EDUCATION_DETAILS_DELETE_MESSAGE = "Educational details for user deleted successfully !!!";
    private const string S_EXPERIANCE_DETAILS_UPDATE_MESSAGE = "Experience details for user updated successfully !!!";
    private const string S_EDUCATION_DETAILS_UPDATE_MESSAGE = "Educational details for user updated successfully !!!";
    private const string S_FILE_SIZE_ERROR_MESSAGE = "File size should not be greater than 5 MB.";
    private const int I_FILE_SIZE_LIMIT = 5242880; // nearly 5 mb
    private const string S_DRIVER_LICENSE_FOLDER_LOCATION = "\\DOWNLOADS\\TransportModule\\LicenseDocuments\\";
    ////Table Indices
    private const int I_TBL_SCHOOL_MENUS = 0;
    private const string S_REPORTS_TABLE = "Reports";    
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion -- CONSTANT(s) --

    #region -- PROPERTIES --

    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }
    #endregion -- PROPERTIES --

    #region EVENT(s)

    /// <summary>
    /// This event is used to fill salutation combo as well to set default properties at add/edit mode.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moOtherStaffBL = new OtherStaffBL(miSchoolId, miUserId);
            moSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
            if (!IsPostBack)
            {
                GetRetirementNoticeConfig();
                SetDefaultProperties();
                FillSalutationComboBox();
                FillDesignationCombobox();
                FillAllComboBoxes();
                SetAddEditModeDetails();
                FillScreenAccessDetails();
                HideShowControls();
                HideControls();
                GetAllAdditionalDetails();
                GetAllEducationalDetails();
                FillExperianceListview();
                SetClientScriptAttributes();
                ucUserBasicDetails.Width = "450";
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                SetDefaultProperties();
            }
            SetControlInEditMode();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save or update Supervisor Details.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void imgBtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidTransportStaff.Value == Convert.ToString(Constants.UserRoles.TransportStaff))
            {
                if (hidMode.Value == "EDIT")
                {
                    ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidTransportStaffID.Value);
                    ucUserBasicDetails.ValidateProfile();
                }
                else
                {
                    ucUserBasicDetails.StaffUserId = 0;
                    ucUserBasicDetails.ValidateProfile();
                }

                lblErrorMsg.Text = string.Empty;
                SaveTransportStaffDetails();

                ucUserBasicDetails.StaffUserId = hidUserId.Value.ToInt();
                ucUserBasicDetails.PopulateUserBasicDetails();
                ucEmployeeBasicDetails.StaffUserId = hidUserId.Value.ToInt();  //
                ucEmployeeBasicDetails.PopulateEmployeeBasicDetails();   //////
                if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.TransportStaff));
            }
            else if (hidOtherStaff.Value == Convert.ToString(Constants.UserRoles.OtherStaff))
            {
                if (hidUserId.Value == Constants.S_ZERO)
                    hidMode.Value = Constants.S_NEW_MODE;
                else
                    hidMode.Value = Constants.S_EDIT_MODE;

                if (hidMode.Value == Constants.S_EDIT_MODE)
                {
                    ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value);
                    ucUserBasicDetails.ValidateProfile();
                    hidBasicDetailUserId.Value = hidUserId.Value;
                }
                else
                {
                    ucUserBasicDetails.StaffUserId = 0;
                    ucUserBasicDetails.ValidateProfile();
                }
                SaveOtherStaffDetails();
                if (QueryString["Is_Configured"] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.OtherStaff));

                if (chkSendSMS.Checked)
                    SendSmsToOtherStaff(miOtherStaffUserId);
                chkSendSMS.Checked = false;

                //ClearFields();
                ucUserBasicDetails.StaffUserId = miOtherStaffUserId;
                ucUserBasicDetails.PopulateUserBasicDetails();
                ucEmployeeBasicDetails.StaffUserId = miOtherStaffUserId;  //
                ucEmployeeBasicDetails.PopulateEmployeeBasicDetails();   //////
                //ucUserBasicDetails.ClearFields();
                // this is to clear session image data captured web cam.
                this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
                EnableDisableFields(true);
            }
            else
            {
                int iUserId = 0;
                //Check the mode of the form. 
                ////If user id from the hidden field is blank then add the user in the system.              
                if (hidSupervisorId.Value == string.Empty)
                {
                    ucUserBasicDetails.StaffUserId = 0;
                    ucUserBasicDetails.ValidateProfile();
                    iUserId = AddSupervisor();
                    lblUpdateSucess.Text = Resources.LocalizedResources.UpdateMsgForProfile;
                }
                else
                {
                    ucUserBasicDetails.StaffUserId = hidUserId.Value.ToInt();
                    ucUserBasicDetails.ValidateProfile();
                    iUserId = UpdateSupervisor();
                    lblUpdateSucess.Text = Resources.LocalizedResources.UpdateMsgForProfile;
                    SetControlInEditMode();
                }

                lblUpdateSucess.Visible = true;
                ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value);
                ucUserBasicDetails.PopulateUserBasicDetails();                
                ucUserBasicDetails.InitializeFields();
                ucEmployeeBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value); /////new added
                ucEmployeeBasicDetails.PopulateEmployeeBasicDetails();  /////
                ucEmployeeBasicDetails.InitializeFields();   //////
                RebuilUserPermissionsCache();
                DataSet oDataSet = GetScreenAccessDetails();
                FillSchoolMenus(oDataSet.Tables[I_TBL_SCHOOL_MENUS]);
                FillScreenAccessDetails();
                // this is to clear session image data captured web cam.
                this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
                RefreshStaffCache(iUserId, Constants.Action.Insert);
            }
        }
        catch (SqlException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblUpdateSucess.Visible = false;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (DuplicateUserException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblErrorMsg.Text = oResourceManager.GetString(ex.Message.Replace(" ", string.Empty));
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This method is used to navigate back to SupervisorUserList.aspx page.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// 	This method is used to fill child listview.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void lstvwReportFolders_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            var oCurrentItem = e.Item as ListViewDataItem;
            var chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            chkSelect.Attributes.Add("onclick", String.Format("SelectUnSelectChilds('{0}', this);", oCurrentItem.DisplayIndex));
            var oHtmlTableRow = oCurrentItem.FindControl("trReports") as HtmlTableRow;
            var oHtmlTableCell = oHtmlTableRow.FindControl("tdReports") as HtmlTableCell;
            var lstvwReports = oHtmlTableCell.FindControl("lstvwReports") as ListView;
            DataTable dtReports = null;
            if (ViewState[S_REPORTS_TABLE] != null)
                dtReports = ViewState[S_REPORTS_TABLE] as DataTable;
            int iReportFolderId = lstvwReportFolders.DataKeys[oCurrentItem.DisplayIndex]["Report_Folder_Id"].ToInt();
            DataRow[] oDatarows = dtReports.Select("Report_Folder_Id = " + iReportFolderId);
            DataTable dtReportDetails = new DataTable();
            if (oDatarows.Length > 0)
                dtReportDetails = oDatarows.CopyToDataTable();
            lstvwReports.DataSource = dtReportDetails;
            lstvwReports.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to disable column.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void lstvwReports_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            var oCurrentItem = e.Item as ListViewDataItem;
            var oDataRowView = oCurrentItem.DataItem as DataRowView;
            var chkReportName = oCurrentItem.FindControl("chkReportName") as CheckBox;
            char cHasAccess = Convert.ToChar((sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["HasAccess"]);
            chkReportName.Checked = cHasAccess == 'Y';
            bool bHasFullAccess = (sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["HasFullAccess"].ToBool();
            bool bIsViewAvailable = (sender as ListView).DataKeys[oCurrentItem.DisplayIndex]["IsViewAvailable"].ToBool();
            var chkHasFullAccess = oCurrentItem.FindControl("chkHasFullAccess") as CheckBox;
            chkHasFullAccess.Checked = bHasFullAccess;
            chkHasFullAccess.Enabled = bIsViewAvailable;
            var oImg = e.Item.FindControl("imgPhotoUpload") as Image;
            if (oImg != null)
                oImg.ImageUrl = oDataRowView["PhotofilePath"].ToString().Trim().Equals(string.Empty) ? Constants.S_UPLOAD_IMAGE_STATUS_BLANK_PHOTO : Constants.S_UPLOAD_IMAGE_STATUS_TRUE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This for designation comb box selected index change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<int> lstDesignationIds = new List<int>();
            if (ViewState[S_ACCOUNT] != null)
                lstDesignationIds = ViewState[S_ACCOUNT] as List<int>;
            if (lstDesignationIds.FindAll(s => s == cmbDesignation.SelectedValue.ToInt()).Any())
            {
                chkCanEditOldFinancialYear.Enabled = true;
                chkFinancialYearChangeApplicable.Enabled = true;
                chkCanDeleteVoucher.Enabled = true;
            }
            else
            {
                chkCanDeleteVoucher.Checked = chkCanEditOldFinancialYear.Checked = chkFinancialYearChangeApplicable.Checked = false;
                chkCanEditOldFinancialYear.Enabled = false;
                chkFinancialYearChangeApplicable.Enabled = false;
                chkCanDeleteVoucher.Enabled = false;
            }
            HideShowControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add onclick attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                StaffAdditionalDetails oStaffAdditionalDetails = e.Item.DataItem as StaffAdditionalDetails;
                Label lblJoinDate = e.Item.FindControl("lblJoinDate") as Label;
                Label lblLeftDate = e.Item.FindControl("lblLeftDate") as Label;

                if (oStaffAdditionalDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblJoinDate.Text = oStaffAdditionalDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
                else
                    lblJoinDate.Text = "-";

                if (oStaffAdditionalDetails.LeftDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblLeftDate.Text = oStaffAdditionalDetails.LeftDate.ToString(Constants.S_DATE_FORMAT);
                else
                    lblLeftDate.Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used Command for Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iExperianceId = Convert.ToInt32(lstvwExpDetails.DataKeys[e.Item.DisplayIndex]["ExperianceId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSaveExperiance.Text = "UPDATE";
                    StaffAdditionalDetails oStaffAdditionalDetails = moSchoolWiseSupervisorMasterBL.GetExperianceDetails(iExperianceId);
                    hidExperienceDetailsId.Value = iExperianceId.ToString();
                    if (oStaffAdditionalDetails.OrganisationName != string.Empty)
                        txtSchoolname.Text = oStaffAdditionalDetails.OrganisationName;
                    else
                        txtSchoolname.Text = string.Empty;
                    if (oStaffAdditionalDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtjoinedDate.Text = oStaffAdditionalDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
                    else
                        txtjoinedDate.Text = string.Empty;
                    if (oStaffAdditionalDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtLeftDate.Text = oStaffAdditionalDetails.LeftDate.ToString(Constants.S_DATE_FORMAT);
                    else
                        txtLeftDate.Text = string.Empty;
                    if(oStaffAdditionalDetails.PreviousDesignation != string.Empty)   //////
                    txtDesignation.Text = oStaffAdditionalDetails.PreviousDesignation;
                    if (oStaffAdditionalDetails.JobDescription != string.Empty)   //////
                        txtJobDescription.Text = oStaffAdditionalDetails.JobDescription;
                    if (oStaffAdditionalDetails.LastSalary != Constants.I_ZERO)   //////
                        txtLastSalary.Text = Convert.ToInt32( oStaffAdditionalDetails.LastSalary).ToString();
                    if (oStaffAdditionalDetails.Duration != Constants.I_ZERO)   //////
                        txtDuration.Text = Convert.ToInt32( oStaffAdditionalDetails.Duration).ToString();
                    if (oStaffAdditionalDetails.ReasonForLeaving != string.Empty)   //////
                         txtReasonForLeaving.Text = oStaffAdditionalDetails.ReasonForLeaving;
                    if (oStaffAdditionalDetails.Achievement != string.Empty)   //////
                        txtAchivements.Text = oStaffAdditionalDetails.Achievement; 
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSchoolWiseSupervisorMasterBL.DeleteExperianceDetails(iExperianceId, hidUserId.Value.ToInt(), miUserId);
                    FillExperianceListview();
                    lblSuccessMsg.Text = S_EXPERIANCE_DETAILS_DELETE_MESSAGE;                    
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save additional details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveAdditionalDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This click event for saving the Educational Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEducationSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidUserId.Value != Constants.S_ZERO)
            {
                SaveEducationalDetails();
                ClearEducationalControl();
                GetAllEducationalDetails();
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This click event is used for the Saving Experiance Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSaveExperiance_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidUserId.Value != Constants.S_ZERO)
            {
                SaveExperianceDetails();
                ClearExperianceControls();
                FillExperianceListview();
            }
            else
                lblSuccessMsg.Text = S_BASIC_DETAILS_MESSAGE;                
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Additional details button clear.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            cmbBloodGroup.ClearSelection();
            cmbMartialStatus.ClearSelection();
            cmbReligion.ClearSelection();
            cmbCategory.ClearSelection();
            txtAadharNumber.Text = string.Empty;
            txtCast.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for Educational Details Cancel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEclear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEducationalControl();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Experiance clear button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExpClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearExperianceControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Back button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidOtherStaff.Value == Convert.ToString(Constants.UserRoles.OtherStaff))
            {
                Response.Redirect("~/RITeSchool/Payroll/OtherStaffUI.aspx");
            }
            else if (hidTransportStaff.Value == Convert.ToString(Constants.UserRoles.TransportStaff))
            {
                Response.Redirect("~/RITeSchool/Transport/TransportStaffUI.aspx");
            }
            else
                this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            RedirectToBackPage();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used Command for Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEducationalDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iEducationDetailsId = Convert.ToInt32(lstvwEducationalDetails.DataKeys[e.Item.DisplayIndex]["EducationId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnEducationSave.Text = S_TEXT_UPDATE;
                    StaffAdditionalDetails oStaffAdditionalDetails = moSchoolWiseSupervisorMasterBL.GetEducationalDetails(iEducationDetailsId);
                    hidEducationId.Value = iEducationDetailsId.ToString();
                    cmbQualification.SelectedValue = oStaffAdditionalDetails.QualificationId.ToString();
                    txtSpecialization.Text = oStaffAdditionalDetails.Specialization;
                    txtYearOfPassing.Text = oStaffAdditionalDetails.YearOfPassing;
                    cmbPassingClass.SelectedValue = oStaffAdditionalDetails.ClassId.ToString();
                    txtPassingUniversity.Text = oStaffAdditionalDetails.University;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSchoolWiseSupervisorMasterBL.DeleteEducationDetails(iEducationDetailsId, hidUserId.Value.ToInt(), miUserId);
                    GetAllEducationalDetails();
                    lblEducationMessage.Text = S_EDUCATION_DETAILS_DELETE_MESSAGE;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Item Data Bound for Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEducationalDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ImageButton imgDeleteEducation = e.Item.FindControl("btnDelete") as ImageButton;
            imgDeleteEducation.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for item Deleting in Experiance Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    { }

    /// <summary>
    /// This event is used for item Editing in Experiance Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    { }

    /// <summary>
    /// This event is used for selected index change in Experiance Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_SelectedIndexChanged(object sender, EventArgs e)
    { }

    /// <summary>
    /// This event is used for item Deleting in Educational Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEducationalDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    { }

    /// <summary>
    /// This event is used for item Editing in Educational Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEducationalDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    { }

    /// <summary>
    /// This event is used for selected index changed in Educational Details Listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEducationalDetails_SelectedIndexChanged(object sender, EventArgs e)
    { }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    //This method is used for the hide the controls as per the USERID Role.
    private void HideControls()
    {
        if (hidOtherStaff.Value == Convert.ToString(Constants.UserRoles.OtherStaff))
        {
            FillSalutationComboBoxOtherStaff();
            FillDesignationComboboxOtherStaff();
            if (hidOtherStaffID.Value != "")
                FillControlsForOtherStaffUpdate(Convert.ToInt32(hidOtherStaffID.Value));
            tracademic.Visible = false;
            trchkCanApproveRequisitions.Visible = false;
            trchkCanCraeteGenerelRequisition.Visible = false;
            trchkCanSanctionLeave.Visible = false;
            trPublishorUnpublish.Visible = false;
            trAccountsRow0.Visible = false;
            trAccountsRow1.Visible = false;
            trAccountsRow2.Visible = false;
            trAccountsRow3.Visible = false;
            trAccountsRow4.Visible = false;
            trAccountsRow5.Visible = false;
            trInternalUser.Visible = false;
            trSMSAllow.Visible = false;
            trCollapsSubjectmenu.Visible = false;
        }
        else if (hidTransportStaff.Value == Convert.ToString(Constants.UserRoles.TransportStaff))
        {
            FillSalutationComboBoxTransportStaff();
            FillDesignationComboboxTransportStaff();
            if (hidUserId.Value != Constants.S_ZERO)
                FillControlsForTransportStaffUpdate(Convert.ToInt32(hidTransportStaffID.Value));
            tracademic.Visible = false;
            trchkCanApproveRequisitions.Visible = false;
            trchkCanCraeteGenerelRequisition.Visible = false;
            trchkCanSanctionLeave.Visible = false;
            trPublishorUnpublish.Visible = false;
            trAccountsRow0.Visible = false;
            trAccountsRow1.Visible = false;
            trAccountsRow2.Visible = false;
            trAccountsRow3.Visible = false;
            trAccountsRow4.Visible = false;
            trAccountsRow5.Visible = false;
            trInternalUser.Visible = false;
            trSMSAllow.Visible = false;
            trSMS.Visible = false;
            trusername.Visible = false;
            trPassword.Visible = false;
            trConfirmPassword.Visible = false;
            trNotePassword.Visible = false;            
            trCollapsSubjectmenu.Visible = false;
            trMail.Visible = false;
        }
        else
        {

            tracademic.Visible = true;
            trchkCanApproveRequisitions.Visible = true;
            trchkCanCraeteGenerelRequisition.Visible = true;
            trchkCanSanctionLeave.Visible = true;
            trPublishorUnpublish.Visible = true;
            trAccountsRow0.Visible = true;
            trAccountsRow1.Visible = true;
            trAccountsRow2.Visible = true;
            trAccountsRow3.Visible = true;
            trAccountsRow4.Visible = true;
            trAccountsRow5.Visible = true;
            trInternalUser.Visible = true;
            trSMSAllow.Visible = true;
            trSMS.Visible = true;
        }
    }

    /// <summary>
    /// This method is Used For Fill all Comboboxes.
    /// </summary>
    private void FillAllComboBoxes()
    {
        DataSet oDsMaster = MasterDataCollectionBL.GetAllMasterData();
        // 1: Category
        ControlUtility.FillDropDownList(oDsMaster.Tables[1], ref cmbCategory, "Category_Id", "Category_Name", Constants.S_SELECT);
        // 3: Religion
        ControlUtility.FillDropDownList(oDsMaster.Tables[3], ref cmbReligion, "Religion_Id", "Religion_Name", Constants.S_SELECT);
        ////4:Qualification
        ControlUtility.FillDropDownList(oDsMaster.Tables[4], ref cmbQualification, "Qualification_Id", "Qualification_Name", Constants.S_SELECT);
        // 5:Passing Class.
        ControlUtility.FillDropDownList(oDsMaster.Tables[5], ref cmbPassingClass, "Class_Id", "Class_Name", string.Empty);
        ////7:BloodGroup
        ControlUtility.FillDropDownList(oDsMaster.Tables[7], ref cmbBloodGroup, "Id", "BloodGroup", Constants.S_SELECT);
        ///8:MaritialStatus
        ControlUtility.FillDropDownList(oDsMaster.Tables[8], ref cmbMartialStatus, "Id", "MaritalStatus", Constants.S_SELECT);
    }

    /// <summary>
    /// 	This method is used to fill grid which provide access level.
    /// </summary>
    private void FillScreenAccessDetails()
    {
        DataSet oDataSet = GetScreenAccessDetails();
        FillSchoolMenus(oDataSet.Tables[I_TBL_SCHOOL_MENUS]);
        FillReportFolderNode(oDataSet);
        ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value);
        if (hidTransportStaff.Value != "TransportStaff")
            ucUserBasicDetails.ShowGradePayOnStaffProfileScreen = Settings.ShowGradePayOnStaffProfileScreen;
        ucUserBasicDetails.InitializeFields();
        ucEmployeeBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value); ////
        ucEmployeeBasicDetails.InitializeFields();   //////
    }

    private void FillSchoolMenus(DataTable oDTSchoolMenus)
    {
        grdAccessConfiguration.DataSource = FilterRestrictedScreens(oDTSchoolMenus);
        grdAccessConfiguration.DataBind();
    }

    /// <summary>
    /// 	This method is used to fill designation combobox.
    /// </summary>
    private void FillDesignationCombobox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignation, Constants.UserRoles.Supervisor);
    }

    /// <summary>
    /// Filters the passed DataSet and removes all restricted screens if the current user does not have access to it.
    /// </summary>
    /// <param name="adtScreenAccessDetails"></param>
    /// <returns></returns>
    private DataTable FilterRestrictedScreens(DataTable adtScreenAccessDetails)
    {
        if (adtScreenAccessDetails.IsNull() || adtScreenAccessDetails.Rows.Count == 0)
            return adtScreenAccessDetails;
        var oSchoolwiseSupervisorMaster = new SchoolWiseSupervisorMasterBL();
        List<SchoolModule> lstModules = oSchoolwiseSupervisorMaster.GetRestrictedModulesForUser(miUserId);
        if (lstModules.IsNull() || lstModules.Count == 0)
            return adtScreenAccessDetails;
        lstModules.ForEach(m => adtScreenAccessDetails.Select(String.Format("SchoolModulesId = {0}", m.Id))
                                                      .ToList()
                                                      .ForEach(r => r.Delete()));

        return adtScreenAccessDetails;
    }

    /// <summary>
    /// This method is used to populate supervisor object.
    /// </summary>
    private SchoolWiseSupervisorMasterBL CreateSupervisorObject()  ////
    {
        return new SchoolWiseSupervisorMasterBL
                    {
                        School_Id = miSchoolId,
                        AcademicYearId = miAcademicYearId,
                        Salutation_Id = cmbSalutation.SelectedValue.ToInt(),
                        Supervisor_First_Name = txtFirstName.Text.ToTitleCase(),
                        Supervisor_Middle_Name = txtMiddleName.Text.ToTitleCase(),
                        Supervisor_Last_Name = txtLastName.Text.ToTitleCase(),
                        Mobile_Number = txtMobileNo.Text,
                        Designation_Id = cmbDesignation.SelectedValue.ToInt(),
                        Inserted_By_id = miUserId,
                        Updated_By_Id = miUserId,
                        Update_Date = DateTime.Now,
                        IsAcademicYrApplicable = chkAcademicApplicable.Checked ? Constants.C_YES : Constants.C_NO,
                        IsFinancialYearApplicable = chkFinancialYearChangeApplicable.Checked,
                         PresentAddress = txtpresentAddress.Text,
                        City = txtLocalCity.Text,
                        State = txtState.Text,
                        Pincode = (txtLocalPincode.Text.Trim()!= string.Empty? txtLocalPincode.Text.ToInt():0)
                    };
    }

    /// <summary>
    /// Create the user role's object for the available values.
    /// </summary>
    /// <returns> SchoolUserBL </returns>
    private SchoolUserBL CreateSchooUserObject()
    {
        var oSupervisorBL = new SchoolUserBL
                                {
                                    Email = txtEmail.Text.Trim(),
                                    Login = txtUserName.Text.Trim(),
                                    Password = (txtPasswd.Enabled == true) ? txtPasswd.Text : hidPassword.Value,
                                    Address = txtAddress.Text.Trim(),
                                    EmergencyContact = txtEmergencyNo.Text.Trim(),
                                    SalutationId = cmbSalutation.SelectedValue.ToInt(),
                                    UserRoleId = Constants.UserRoles.Supervisor.ToInt(),
                                    SchoolId = miSchoolId,
                                    UpdatedBy = Convert.ToString(miUserId),
                                    InsertedBy = Convert.ToString(miUserId),
                                    InternalUser = chkInternalUser.Checked,
                                    UpdatedDate = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI),
                                    CanApproveRequisition = chkCanApproveRequisitions.Checked ? Constants.C_YES : Constants.C_NO,
                                    CanCreateGeneralRequisition = chkCanCreateGeneralRequisition.Checked ? Constants.C_YES : Constants.C_NO,
                                    CanSanctionLeave = chkCanSanctionLeave.Checked ? Constants.C_YES : Constants.C_NO,
                                    CanApproveVoucher = chkCanApproveVoucher.Checked,
                                    CanCreateVoucher = chkCanCreateVoucher.Checked,
                                    CanPublishUnpublishExam = chkPublishorUnpublishExam.Checked,
                                    ShowAllSentSMS = chkShowAllSentSMS.Checked,

                                };
        if (chkCanCreateVoucher.Checked)
        {
            oSupervisorBL.CanSelfApprove = chkCanSelfApprove.Checked;
            chkCanSelfApprove.InputAttributes.Remove("disabled");
        }
        else
            chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");

        oSupervisorBL.CanDeleteVoucher = chkCanDeleteVoucher.Checked;
        oSupervisorBL.CanEditOldFinancialYear = chkCanEditOldFinancialYear.Checked;

        oSupervisorBL.FirstName = string.Empty;
        oSupervisorBL.LastName = string.Empty;
        oSupervisorBL.MiddleName = string.Empty;
        oSupervisorBL.sDOB = txtDOB.Text != Constants.S_EMPTY_STRING ? Convert.ToDateTime(txtDOB.Text).ToString(Constants.S_DATE_FORMAT_MARATHI) : string.Empty;

        // This code used to set password in new admin staff mode,
        // otherwise password automaticaly clears. 
        txtPasswd.Attributes.Add("value", oSupervisorBL.Password);
        txtConfirmPasswd.Attributes.Add("value", oSupervisorBL.Password);

        // Update permission in Session
        Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] = oSupervisorBL.CanEditOldFinancialYear;

        return oSupervisorBL;
    }

    /// <summary>
    /// This method is used to decrypt given querystring
    /// </summary>
    private void ReadeQuerystring()
    {
        if (QueryString[S_USER_ID] != null)
            hidUserId.Value = QueryString[S_USER_ID];

        hidSupervisorId.Value = QueryString[S_SUPERVISOR_ID] ?? string.Empty;
        hidIsConfig.Value = QueryString[S_IS_CONFIGURED] ?? string.Empty;

        if (QueryString["User_Role_Id"] != null)
            hidOtherStaff.Value = QueryString["User_Role_Id"];
        if (QueryString["User_Role_Id"] != null)
        {
            hidTransportStaff.Value = QueryString["User_Role_Id"];
            hidOtherStaffID.Value = QueryString["OtherStaffId"];
        }
        if (QueryString["TransportStaffID"] != null)
            hidTransportStaffID.Value = QueryString["TransportStaffID"];

    }

    /// <summary>
    /// 	This method is used to redirect page to the source page(Back page).
    /// </summary>
    private void RedirectToBackPage()
    {
        string sQuerystring = S_IS_CONFIGURED + "=" + hidIsConfig.Value;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
        string sRedirectUrl = S_BACK_PAGE_URL + "?" + sEncrypt;
        var oMasterPage = Master as MasterPage;
        oMasterPage.RedirectToNextPage(sRedirectUrl);
    }

    /// <summary>
    /// This method is used to retrieve retirement notice config. of admin staff.
    /// </summary>
    private void GetRetirementNoticeConfig()
    {
        moRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
        List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = moRetirementNoticeConfigBL.GetAll();
        int iRetAge = lstRetirementNoticeConfig.Where(obj => obj.UserRole.Id == Constants.UserRoles.Supervisor.ToInt()).Select(obj => obj.RetirementAge).FirstOrDefault();
        hidRetirementAge.Value = System.DateTime.Now.AddYears(-1 * iRetAge).ToString("dd-MMM-yyyy");
        hidRetAge.Value = iRetAge.ToString();
    }

    /// <summary>
    /// 	This method is used to add supervisor details.
    /// </summary>
    private int AddSupervisor()
    {
        SchoolUserBL oSchoolUserBL = CreateSchooUserObject();
        int iUserId = oSchoolUserBL.InsertSchoolUserDetails();
        hidUserId.Value = iUserId.ToString();

        if (iUserId == Constants.I_ZERO)
            return 0;

        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorBL = CreateSupervisorObject();
        if (UploadPhoto.HasFile)
        {
            string sFileName = SaveFileOnServer(UploadPhoto);
            oSchoolWiseSupervisorBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            Byte[] imageBinaryData = base.GetByteArrayFromFileField(UploadPhoto);
            oSchoolWiseSupervisorBL.BinaryFormatPhoto = imageBinaryData;
        }
        else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
        {
            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
            var oImage = lstImageData.Where(lst => lst.UserID == 0).LastOrDefault();
            if (!oImage.IsNull())
            {
                oSchoolWiseSupervisorBL.BinaryFormatPhoto = oImage.ImagesData;
                oSchoolUserBL.BinaryPhotoImage = oImage.ImagesData;
            }
        }
        else
        {
            Byte[] ImageBinaryData = { };
            oSchoolWiseSupervisorBL.PhotoFilePath = string.Empty;
            oSchoolUserBL.BinaryPhotoImage = ImageBinaryData;
        }

        oSchoolWiseSupervisorBL.User_Id = iUserId;
        int iSupervisorId = oSchoolWiseSupervisorBL.InsertSchoolWiseSupervisorMaster();
        hidSupervisorId.Value = iSupervisorId.ToString();

        InsertShiftDetailsForAdminStaff();
        InsertWeekendDetailsForAdminStaff();

        string sReportsAccessId = GetSelectedReports();

        oSchoolWiseSupervisorBL.AddSupervisorScreens(iUserId, miUserId, hidScreenAccess.Value, sReportsAccessId);
        if (hidIsConfig.Value != "Y")
            SaveConfigDetails(Constants.SchoolConfigurations.AdminStaffConfig.ToInt());

        if (!oSchoolWiseSupervisorBL.BinaryFormatPhoto.IsNull())
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + hidUserId.Value;
        else
            imgPhoto.Src = S_DEFAULT_PHOTO;

        if (chkSendSMS.Checked)
            SendSmsToUser(iUserId);
        chkSendSMS.Checked = false;

        return iUserId;
    }


    /// <summary>
    /// This method is used to save TransportStaff details.
    /// </summary>
    private void SaveTransportStaffDetails()
    {
        TransportStaffBL oTransportStaffBL = CreateTransportStaffObject();
        if (flDocument.HasFile && hidFileUpload.Value != string.Empty)
        {
            if (chkRenew.Checked)
                oTransportStaffBL.IsRenewMode = "Y";
            else
                oTransportStaffBL.IsRenewMode = "N";
        }
        else
        {
            oTransportStaffBL.IsRenewMode = "N";
        }

        string sFileName;
        Byte[] ImageBinaryData = { };
        if (UploadPhoto.HasFile)
        {
            sFileName = SaveFileOnServer(UploadPhoto);
            ImageBinaryData = GetByteArrayFromFileField(UploadPhoto);
            oTransportStaffBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            oTransportStaffBL.BinaryPhotoImage = ImageBinaryData;
        }
        else
            oTransportStaffBL.PhotoFilePath = string.Empty;

        string asFileName;
        if (SaveFileToServer(out asFileName))
        {
            if (txtLicenseExpiryDate.Text != string.Empty)
                oTransportStaffBL.mdtDriverLicenseExpiryDate = txtLicenseExpiryDate.Text.ToDateTime();
            else
                oTransportStaffBL.mdtDriverLicenseExpiryDate = Constants.S_DEFAULT_DATE_2.ToDateTime();

            oTransportStaffBL.DriverBatch = txtDriverBatch.Text;
            oTransportStaffBL.DocumentFileName = asFileName;
            oTransportStaffBL.TransportStaffFieldId = hidTransportStaffFields.Value.ToInt();

            int iTransportStaffId = 0;
            if (hidMode.Value != Constants.S_EDIT_MODE)
            {
                oTransportStaffBL.Insert(out iTransportStaffId);
                hidUserId.Value = iTransportStaffId.ToString();
            }
            else
            {
                oTransportStaffBL.TransportStaffId = Convert.ToInt32(hidTransportStaffID.Value);
                oTransportStaffBL.EmergencyContact = txtEmergencyNo.Text.Trim();

                oTransportStaffBL.UpdateStaff();
            }

            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Transport Staff Details Saved Successfully!!";
            hidMode.Value = Constants.S_NEW_MODE;

            if (flDocument.HasFile)
            {
                btnFile.Visible = true;
                string sPath = "../downloads/TransportModule/LicenseDocuments/" + oTransportStaffBL.DocumentFileName;
                btnFile.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                hidFileUpload.Value = oTransportStaffBL.DocumentFileName;
            }

            hidLicensceExpDate.Value = oTransportStaffBL.mdtDriverLicenseExpiryDate.ToString(Constants.S_DATE_FORMAT);

            HideShowControls();
        }
    }

    /// <summary>
    /// This method is used to create transport staff object.
    /// </summary>
    /// <returns>TransportStaffBL</returns>
    private TransportStaffBL CreateTransportStaffObject()
    {
        TransportStaffBL oTransportStaffBL = new TransportStaffBL();
        oTransportStaffBL.SchoolId = miSchoolId;
        oTransportStaffBL.AcademicYearId = miAcademicYearId;
        oTransportStaffBL.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        oTransportStaffBL.FirstName = txtFirstName.Text.ToTitleCase();
        oTransportStaffBL.MiddleName = txtMiddleName.Text.ToTitleCase();
        oTransportStaffBL.LastName = txtLastName.Text.ToTitleCase();
        oTransportStaffBL.MobileNo = txtMobileNo.Text.Trim();
        oTransportStaffBL.Address = txtAddress.Text.Trim();
        oTransportStaffBL.EmergencyContact = txtEmergencyNo.Text.Trim();
        oTransportStaffBL.DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue);
        oTransportStaffBL.InsertedById = miUserId;
        oTransportStaffBL.DOB = !string.IsNullOrEmpty(txtDOB.Text) ? txtDOB.Text.ToDateTime() : Constants.S_DEFAULT_DATE_2.ToDateTime();
        if (hidMode.Value == Constants.S_EDIT_MODE)
            oTransportStaffBL.UserId = Convert.ToInt32(hidUserId.Value);
        return oTransportStaffBL;
    }


    /// <summary>
    /// This Method is used to save OTHERStaff details.
    /// </summary>
    private void SaveOtherStaffDetails()
    {
        moOtherStaffBL.OtherStaff = CreateOtherStaffObject();
        string sFileName;
        byte[] oImageBinaryData = { };
        if (UploadPhoto.HasFile)
        {
            sFileName = SaveFileOnServer(UploadPhoto);
            oImageBinaryData = GetByteArrayFromFileField(UploadPhoto);
            moOtherStaffBL.OtherStaff.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            moOtherStaffBL.OtherStaff.BinaryFormatPhoto = oImageBinaryData;
        }
        else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES && Session[Constants.S_SESSION_IS_BUTTON_CLOSE] != null)
        {
            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
            if (hidBasicDetailUserId.Value == "")
            {
                var oImage = lstImageData.Where(lst => lst.UserID == 0).LastOrDefault();
                if (!oImage.IsNull())
                {
                    oImageBinaryData = oImage.ImagesData;
                    moOtherStaffBL.OtherStaff.BinaryFormatPhoto = oImage.ImagesData;
                }
            }
            else
            {
                var oImage1 = lstImageData.Where(lst => lst.UserID == hidBasicDetailUserId.Value.ToInt()).LastOrDefault();
                if (!oImage1.IsNull())
                {
                    oImageBinaryData = oImage1.ImagesData;
                    moOtherStaffBL.OtherStaff.BinaryFormatPhoto = oImage1.ImagesData;
                }
            }
        }
        else
            moOtherStaffBL.OtherStaff.PhotoFilePath = string.Empty;
        string sUserName = txtUserName.Text.Trim();
        string sPassword;
        if (txtPasswd.Enabled == true)
            sPassword = txtPasswd.Text;
        else
            sPassword = hidPassword.Value;
        string sIsLocked = Settings.EnableOtherStaffLogin ? Constants.S_NO : Constants.S_YES;

        SchoolUserBL oSchoolUserBLObj = new SchoolUserBL();
        oSchoolUserBLObj.Login = sUserName;

        oSchoolUserBLObj.UserId = 0;
        if (hidBasicDetailUserId.Value.Trim() != string.Empty)
            oSchoolUserBLObj.UserId = Convert.ToInt32(hidBasicDetailUserId.Value);

        oSchoolUserBLObj.SchoolId = miSchoolId;
        if (oSchoolUserBLObj.IsUserLoginDuplicate())
            throw new DuplicateUserException(Resources.LocalizedResources.DuplicateUserName);
        if (hidMode.Value != Constants.S_EDIT_MODE)
        {
            miOtherStaffUserId = moOtherStaffBL.Insert(sUserName, sPassword, sIsLocked);
            hidBasicDetailUserId.Value = miOtherStaffUserId.ToString();
            hidUserId.Value = miOtherStaffUserId.ToString();
            InsertShiftDetails();
            InsertWeekendDetails();
        }
        else
        {
            SchoolUserBL oSchoolUserBL = CreateSchoolUserOtherStaffObject();
            oSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
            moOtherStaffBL.OtherStaff.OtherStaffId = Convert.ToInt32(hidOtherStaffID.Value);
            oSchoolUserBL.UpdateOtherStaffSchoolUser(oImageBinaryData, moOtherStaffBL.OtherStaff.OtherStaffId, moOtherStaffBL.OtherStaff.PhotoFilePath, sUserName, sPassword);
            if (hidBasicDetailUserId.Value != string.Empty || hidBasicDetailUserId.Value != Constants.S_ZERO)
                miOtherStaffUserId = Convert.ToInt32(hidBasicDetailUserId.Value);
        }
        if (imgBtnSubmit.Text == Resources.LocalizedResources.Save)
            lblUpdateSucess.Text = Resources.LocalizedResources.OtherStaffSave;
        else
        {
            lblUpdateSucess.Text = Resources.LocalizedResources.OtherStaffUpdate;
            imgBtnSubmit.Text = Resources.LocalizedResources.Save;
        }

        lblUpdateSucess.Visible = true;
        lblErrorMsg.Visible = false;
    }

    /// <summary>
    /// This method is used to create other staff object.
    /// </summary>
    /// <returns>OtherStaff</returns>
    private OtherStaff CreateOtherStaffObject()
    {
        return new OtherStaff
        {
            SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue),
            FirstName = txtFirstName.Text.ToTitleCase(),
            MiddleName = txtMiddleName.Text.ToTitleCase(),
            LastName = txtLastName.Text.ToTitleCase(),
            Address = txtAddress.Text.Trim(),
            MobileNo = txtMobileNo.Text.Trim(),
            EmergencyNo = txtEmergencyNo.Text.Trim(),
            EmailId = txtEmail.Text.Trim(),
            DateOfBirth = txtDOB.Text != Constants.S_EMPTY_STRING ? Convert.ToDateTime(txtDOB.Text).ToString("MM/dd/yyyy") : Constants.S_EMPTY_STRING,
            DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue),
        };
    }

    /// <summary>
    /// This method is used to insert shift details.
    /// </summary>
    private void InsertShiftDetails()
    {
        UserShiftAssociationBL oUserShiftAssociationBL = new UserShiftAssociationBL();
        int shiftId = oUserShiftAssociationBL.GetDefaultShift(miSchoolId, miAcademicYearId);
        if (shiftId != 0)
        {
            oUserShiftAssociationBL.Shiftid = shiftId;
            oUserShiftAssociationBL.SchoolId = miSchoolId;
            oUserShiftAssociationBL.UserId = Convert.ToInt32(hidBasicDetailUserId.Value);
            oUserShiftAssociationBL.AcademicYearId = miAcademicYearId;
            oUserShiftAssociationBL.IsDeleted = Constants.C_NO;
            oUserShiftAssociationBL.InsertedById = miUserId;
            oUserShiftAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oUserShiftAssociationBL.InsertShiftAssociationDetailsForOtherAndAdminStaff();
        }
    }

    /// <summary>
    /// This method is used to insert weekend details.
    /// </summary>
    private void InsertWeekendDetails()
    {
        UserWeekEndAssociationBL oUserWeekendAssociationBL = new UserWeekEndAssociationBL();
        List<int> weekendIdList = oUserWeekendAssociationBL.GetWeekendsApplicableforStaff(miSchoolId, miAcademicYearId);
        foreach (int iWeekendId in weekendIdList)
        {
            oUserWeekendAssociationBL.WeekEndId = iWeekendId;
            oUserWeekendAssociationBL.SchoolId = miSchoolId;
            oUserWeekendAssociationBL.UserId = Convert.ToInt32(hidBasicDetailUserId.Value);
            oUserWeekendAssociationBL.AcademicYearId = miAcademicYearId;
            oUserWeekendAssociationBL.IsDeleted = Constants.C_NO;
            oUserWeekendAssociationBL.InsertedById = miUserId;
            oUserWeekendAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oUserWeekendAssociationBL.InsertWeekendAssociationDetailsForOtherAndAdminStaff();
        }
    }

    /// <summary>
    /// Create the user role's object for the available values.
    /// </summary>
    /// <returns>SchoolUserBL</returns>
    private SchoolUserBL CreateSchoolUserOtherStaffObject()
    {
        SchoolUserBL oOtherStaffBL = new SchoolUserBL
        {
            Email = txtEmail.Text.Trim(),
            Mobile_Number = txtMobileNo.Text.Trim(),
            DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue),
            SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue),
            UserRoleId = Convert.ToInt32(Constants.UserRoles.OtherStaff),
            SchoolId = miSchoolId,
            UpdatedBy = miUserId.ToString(),
            InsertedBy = miUserId.ToString(),
            UpdatedDate = System.DateTime.Today.ToString("MM/dd/yyyy"),
            FirstName = txtFirstName.Text.ToTitleCase(),
            MiddleName = txtMiddleName.Text.ToTitleCase(),
            LastName = txtLastName.Text.ToTitleCase(),
            Address = txtAddress.Text.Trim(),
            EmergencyContact = txtEmergencyNo.Text.Trim(),
            sDOB = txtDOB.Text != Constants.S_EMPTY_STRING ? Convert.ToDateTime(txtDOB.Text).ToString("MM/dd/yyyy") : string.Empty
        };
        return oOtherStaffBL;
    }

    // <summary>
    /// This method is used to send sms.
    /// </summary>
    /// <param name="asMessage"></param>
    public void SendSmsToOtherStaff(int aiUserId)
    {
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(aiUserId);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
        int iSMSType = 0;
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
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

        DataTable oDataTable = SchoolUserCollectionBL.GetPasswordRecoveryDetails(oSchoolUserBL.UserId, miSchoolId);

        if (oDataTable.Rows.Count > 0)
        {
            SMS oSMS = new SMS();
            oSMS.SchoolID = oSchoolBL.SchoolId;
            oSMS.AcademicYearID = Convert.ToInt32(oDataTable.Rows[0]["Academic_Year_ID"]);
            oSMS.SenderID = Convert.ToInt32(oDataTable.Rows[0]["AdminUserId"]);
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + " :: Forgot Password";
            oSMS.DisplayText = txtUserName.Text; //Convert.ToString(oDataTable.Rows[0]["UserName"]);
            oSMS.SMSType = iSMSType;
            oSMS.To.Add(oSchoolUserBL.UserId, txtMobileNo.Text);
            oSMS.Send();
        }
    }


    /// <summary>
    /// This method is used to upload the file to the server. DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload aofileUploadLogo)
    {
        const int I_HEIGHT_LIMIT = 151;
        const int I_WIDTH_LIMIT = 112;
        string sFileName = aofileUploadLogo.FileName;
        string asFileName = sFileName;
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;
        string sServerFilePath = sFolderName + sFileName;
        if (File.Exists(sServerFilePath))
            asFileName = CommonUtility.GetFileNameForRenaming(sFileName);
        sServerFilePath = sFolderName + asFileName;
        UploadPhoto.SaveAs(sServerFilePath);
        string sErrorMsg = ValidateFile(sServerFilePath, I_HEIGHT_LIMIT, I_WIDTH_LIMIT, sFileName);
        if (sErrorMsg.Equals(string.Empty))
        {
            ////delete exesting logo
            string sFileToDelete = Server.MapPath(".") + hidFilePath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMsg.Text = sErrorMsg;
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrorMsg);
        }
        return sFileName;
    }

    /// <summary>
    /// This method is used to validate uploaded file.
    /// </summary>
    /// <param name="asServerFilePath"> </param>
    /// <param name="aiHeight"> </param>
    /// <param name="aiWidth"> </param>
    /// <param name="asFileName"> </param>
    /// <returns> </returns>
    private string ValidateFile(string asServerFilePath, int aiHeight, int aiWidth, string asFileName)
    {
        string sReturnErrorMsg = string.Empty;
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            var oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > aiHeight && oImg.Width > aiWidth)
            {
                sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + aiHeight + "px" + Resources.LocalizedResources.And + aiWidth + "px" + Resources.LocalizedResources.respectively;
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > aiHeight)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + aiHeight + "px.";
                    bIsValid = false;
                }

                if (oImg.Width > aiWidth)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + aiWidth + "px.";
                    bIsValid = false;
                }
            }
            oFileStream.Close();
        }
        var oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > Constants.I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal;
        }
        return sReturnErrorMsg;
    }

    /// <summary>
    /// This method is used to update supervisor details.
    /// </summary>
    private int UpdateSupervisor()
    {
        SchoolUserBL oSchoolUserBL = CreateSchooUserObject();
        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorBL = CreateSupervisorObject();
        Byte[] imageBinaryData = null;
        if (UploadPhoto.HasFile)
        {
            string sFileName = SaveFileOnServer(UploadPhoto);
            imageBinaryData = base.GetByteArrayFromFileField(UploadPhoto);
            oSchoolWiseSupervisorBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            oSchoolWiseSupervisorBL.BinaryFormatPhoto = imageBinaryData;
        }
        else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
        {
            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
            var oImage = lstImageData.Where(lst => lst.UserID == hidUserId.Value.ToInt()).LastOrDefault();
            if (!oImage.IsNull())
            {
                oSchoolWiseSupervisorBL.BinaryFormatPhoto = oImage.ImagesData;
                oSchoolUserBL.BinaryPhotoImage = oImage.ImagesData;
            }
        }
        else
            oSchoolWiseSupervisorBL.PhotoFilePath = string.Empty;
        oSchoolUserBL.UserId = hidUserId.Value.ToInt();
        oSchoolUserBL.UpdateSchoolUser();
        oSchoolWiseSupervisorBL.User_Id = hidUserId.Value.ToInt();
        oSchoolWiseSupervisorBL.Supervisor_Id = hidSupervisorId.Value.ToInt();
        oSchoolWiseSupervisorBL.UpdateSchoolWiseSupervisorMaster();
        string sReportsAccessId = GetSelectedReports();
        int iInsertById = miUserId;
        oSchoolWiseSupervisorBL.AddSupervisorScreens(hidUserId.Value.ToInt(), iInsertById, hidScreenAccess.Value, sReportsAccessId);
        if (!oSchoolWiseSupervisorBL.BinaryFormatPhoto.IsNull())
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + hidUserId.Value;
        else if (imgPhoto.Src.IsNullOrEmpty())
            imgPhoto.Src = S_DEFAULT_PHOTO;

        if (chkSendSMS.Checked)
            SendSmsToUser(oSchoolUserBL.UserId);
        chkSendSMS.Checked = false;
        return oSchoolUserBL.UserId;
    }

    /// <summary>
    /// This method is used to get Quamma sapareted selected report Ids.
    /// </summary>
    /// <returns> </returns>
    private string GetSelectedReports()
    {
        const string S_ELEMENT = "element";
        var oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("ReportAccess");
        XmlNode oRootNode = oDoc.CreateNode(S_ELEMENT, "ReportAccess", string.Empty);
        foreach (ListViewDataItem oCurrentFolder in lstvwReportFolders.Items)
        {
            var oHtmlTableRow = oCurrentFolder.FindControl("trReports") as HtmlTableRow;
            var oHtmlTableCell = oHtmlTableRow.FindControl("tdReports") as HtmlTableCell;
            var lstvwReports = oHtmlTableCell.FindControl("lstvwReports") as ListView;

            foreach (ListViewDataItem oCurrentReport in lstvwReports.Items)
            {
                var chkReportName = oCurrentReport.FindControl("chkReportName") as CheckBox;
                var chkHasFullAccess = oCurrentReport.FindControl("chkHasFullAccess") as CheckBox;
                if (!chkReportName.Checked)
                    continue;

                XmlNode oNode = oDoc.CreateNode(S_ELEMENT, "ReportAccess", string.Empty);
                int iReportId = lstvwReports.DataKeys[oCurrentReport.DisplayIndex]["Report_Id"].ToInt();
                XmlAttribute attr = oDoc.CreateAttribute("Report_Id");
                attr.Value = iReportId.ToString();
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("HasFullAccess");
                attr.Value = chkHasFullAccess.Checked ? "1" : "0";
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("IsViewAvailable");
                attr.Value = lstvwReports.DataKeys[oCurrentReport.DisplayIndex]["IsViewAvailable"].ToBool() ? "1" : "0";
                oNode.Attributes.Append(attr);
                oRootNode.AppendChild(oNode);
            }
        }
        root.AppendChild(oRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to add shift details for AdminStaff.
    /// </summary>
    private void InsertShiftDetailsForAdminStaff()
    {
        UserShiftAssociationBL oUserShiftAssociationBL = new UserShiftAssociationBL();
        int shiftId = oUserShiftAssociationBL.GetDefaultShift(miSchoolId, miAcademicYearId);
        if (shiftId != 0)
        {
            oUserShiftAssociationBL.Shiftid = shiftId;
            oUserShiftAssociationBL.SchoolId = miSchoolId;
            oUserShiftAssociationBL.UserId = Convert.ToInt32(hidSupervisorId.Value);
            oUserShiftAssociationBL.AcademicYearId = miAcademicYearId;
            oUserShiftAssociationBL.IsDeleted = Constants.C_NO;
            oUserShiftAssociationBL.InsertedById = miUserId;
            oUserShiftAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oUserShiftAssociationBL.InsertShiftAssociationDetailsForOtherAndAdminStaff();
        }
    }

    /// <summary>
    /// This method is used to add weekend details for AdminStaff.
    /// </summary>
    private void InsertWeekendDetailsForAdminStaff()
    {
        UserWeekEndAssociationBL oUserWeekendAssociationBL = new UserWeekEndAssociationBL();
        List<int> weekendIdList = oUserWeekendAssociationBL.GetWeekendsApplicableforStaff(miSchoolId, miAcademicYearId);
        foreach (int iWeekendId in weekendIdList)
        {
            oUserWeekendAssociationBL.WeekEndId = iWeekendId;
            oUserWeekendAssociationBL.SchoolId = miSchoolId;
            oUserWeekendAssociationBL.UserId = Convert.ToInt32(hidSupervisorId.Value);
            oUserWeekendAssociationBL.AcademicYearId = miAcademicYearId;
            oUserWeekendAssociationBL.IsDeleted = Constants.C_NO;
            oUserWeekendAssociationBL.InsertedById = miUserId;
            oUserWeekendAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oUserWeekendAssociationBL.InsertWeekendAssociationDetailsForOtherAndAdminStaff();
        }
    }

    /// <summary>
    /// This method is used to update supervisor allow screens. 1. Delete screen id physically from the database. 2. Then add(insert) the screen id in the database.
    /// </summary>
    private void UpdateSupervisorScreen()
    {
        var oSchoolWiseSupervisorBL = new SchoolWiseSupervisorMasterBL();
        int iUserId = hidUserId.Value.ToInt();
        oSchoolWiseSupervisorBL.DeleteSupervisorAllowScreen(iUserId);
    }

    /// <summary>
    /// This method is used to fill all details of supervisor in edit mode and also set current node.
    /// </summary>
    private void SetAddEditModeDetails()
    {
        int iUserRoleId = moUserRole.ToInt();
        ReadeQuerystring();
        var oMasterPage = Master as MasterPage;
        if (!hidSupervisorId.Value.Equals(Constants.S_EMPTY_STRING))
        {
            FillSupervisorDetails();
            //oMasterPage.SetCurrentNodeText("Edit " + Constants.S_SUPERVISOR_ROLE_NAME, iUserRoleId, miSchoolId);
            ucUserBasicDetails.HideViewImage = true;
        }
        else
        {
            ucUserBasicDetails.HideViewImage = false;
            //oMasterPage.SetCurrentNodeText("Add " + Constants.S_SUPERVISOR_ROLE_NAME, iUserRoleId, miSchoolId);
        }

        if (hidTransportStaff.Value == Convert.ToString(Constants.UserRoles.TransportStaff))
        {
            if (hidTransportStaffID.Value == "0")
                oMasterPage.SetCurrentNodeText("Add Transport Staff", iUserRoleId, miSchoolId);
            else
                oMasterPage.SetCurrentNodeText("Edit Transport Staff", iUserRoleId, miSchoolId);
        }
        else if (hidOtherStaff.Value == Convert.ToString(Constants.UserRoles.OtherStaff))
        {
            if (hidUserId.Value == "0")
                oMasterPage.SetCurrentNodeText("Add Other Staff", iUserRoleId, miSchoolId);
            else
                oMasterPage.SetCurrentNodeText("Edit Other Staff", iUserRoleId, miSchoolId);
        }
        else
        {
            if (!hidSupervisorId.Value.Equals(Constants.S_EMPTY_STRING))
                oMasterPage.SetCurrentNodeText("Edit " + Constants.S_SUPERVISOR_ROLE_NAME, iUserRoleId, miSchoolId);
            else
                oMasterPage.SetCurrentNodeText("Add " + Constants.S_SUPERVISOR_ROLE_NAME, iUserRoleId, miSchoolId);
        }

                //imgBtnSubmit.Attributes.Add("onClick", "ResetErrorMsgLbl();");
        string sQueryString = "UserId=" + hidUserId.Value;
        ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
    }

    /// <summary>
    /// This method is used to get supervisor allow path ids.
    /// </summary>
    /// <returns> </returns>
    private DataSet GetScreenAccessDetails()
    {
        var oSchoolWiseSupervisorBL = new SchoolWiseSupervisorMasterBL();
        DataSet oDsScreenId = oSchoolWiseSupervisorBL.GetScreenAccessDetails(hidUserId.Value.ToInt(), miUserId, false);
        return oDsScreenId;
    }

    /// <summary>
    /// This method is used to set javascript attribures.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        if (cmbDesignation.SelectedValue == "60" || cmbDesignation.SelectedValue == "70" || cmbDesignation.SelectedValue == "55" || cmbDesignation.SelectedValue == "65")
        {
            chkCanEditOldFinancialYear.Enabled = true;
            chkFinancialYearChangeApplicable.Enabled = true;
            chkCanDeleteVoucher.Enabled = true;
        }
        else
        {
            chkCanEditOldFinancialYear.Enabled = false;
            chkFinancialYearChangeApplicable.Enabled = false;
            chkCanDeleteVoucher.Enabled = false;
        }

        ApplyMouseHoverEffect(new List<Button> { imgBtnCancel, imgBtnSubmit });
        imgBtnSubmit.Attributes.Add("onclick", "CalculateAccess();DisableButtons(this);ResetErrorMsgLbl();");
        txtConfirmPasswd.Attributes.Add("onkeypress", "return clickButton(event)");
        txtEmail.Attributes.Add("onkeypress", "return clickButton(event)");
        txtFirstName.Attributes.Add("onkeypress", "return clickButton(event)");
        txtLastName.Attributes.Add("onkeypress", "return clickButton(event)");
        txtUserName.Attributes.Add("onkeypress", "return clickButton(event)");
        txtMiddleName.Attributes.Add("onkeypress", "return clickButton(event)");
        txtMobileNo.Attributes.Add("onkeypress", "return clickButton(event)");
        txtPasswd.Attributes.Add("onkeypress", "return clickButton(event)");
        HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = imgBtnSubmit.UniqueID;
    }

    /// <summary>
    /// This method is used to fill salutation combo box.
    /// </summary>
    private void FillSalutationComboBox()
    {
        var oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to fill salutation combo for other staff.
    /// </summary>
    private void FillSalutationComboBoxOtherStaff()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to fill designation combo for other staff.
    /// </summary>
    private void FillDesignationComboboxOtherStaff()
    {
        int iUserRoleId = Convert.ToInt32(Constants.UserRoles.OtherStaff);
        DataTable oDataTable = SchoolWiseSupervisorMasterBL.GetSupervisorDesignations(iUserRoleId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbDesignation, "Teacher_Designation_Id", "Teacher_Designation_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill salutation combo for Transport Staff.
    /// </summary>
    private void FillSalutationComboBoxTransportStaff()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to fill designation combo for Transport Staff.
    /// </summary>
    private void FillDesignationComboboxTransportStaff()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignation, Constants.UserRoles.TransportStaff);
    }

    /// <summary>
    /// This method is used to set default property on page load.
    /// </summary>
    private void SetDefaultProperties()
    {
        cmbSalutation.Focus();
        txtUserName.ToolTip = Resources.LocalizedResources.ToolTipUserName;
        txtPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        txtConfirmPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        ValSumAdditionalError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumExperianceDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumEducationalDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        trchkCanApproveRequisitions.Visible = Settings.EnableInventoryModule;
        trchkCanCraeteGenerelRequisition.Visible = Settings.EnableInventoryModule;
        trchkCanSanctionLeave.Visible = Settings.EnableInventoryModule;
        bool bAccountsModuleEnabled = Settings.EnableAccountsModule;
        trAccountsRow1.Visible = bAccountsModuleEnabled;
        trAccountsRow2.Visible = bAccountsModuleEnabled;
        trAccountsRow3.Visible = bAccountsModuleEnabled;
        trAccountsRow4.Visible = bAccountsModuleEnabled;
        trAccountsRow5.Visible = bAccountsModuleEnabled;
        chkPublishorUnpublishExam.Checked = Settings.AllowPublishUnpublishExam;
        if (bAccountsModuleEnabled)
            chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");
        trAccountsRow0.Visible = Settings.EnableAccountsModule;
        hidServerDate.Value = Convert.ToString(DateTime.Today);
        hidCurrentDate.Value = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to display selected Supervisor's information.
    /// </summary>
    private void FillSupervisorDetails()   ////
    {
        var oSchoolUserBL = new SchoolUserBL(hidUserId.Value.ToInt());
        var oSchoolWiseSupervisorBL = new SchoolWiseSupervisorMasterBL(hidSupervisorId.Value.ToInt());
        tblUsername.Visible = true;
        txtFirstName.Text = oSchoolWiseSupervisorBL.Supervisor_First_Name;
        txtMiddleName.Text = oSchoolWiseSupervisorBL.Supervisor_Middle_Name;
        txtLastName.Text = oSchoolWiseSupervisorBL.Supervisor_Last_Name;
        txtEmail.Text = oSchoolUserBL.Email;
        txtAddress.Text = oSchoolUserBL.Address;
        chkCanApproveRequisitions.Checked = oSchoolUserBL.CanApproveRequisition == Constants.C_YES;
        chkCanCreateGeneralRequisition.Checked = oSchoolUserBL.CanCreateGeneralRequisition == Constants.C_YES;
        chkCanSanctionLeave.Checked = oSchoolUserBL.CanSanctionLeave == Constants.C_YES;
        chkCanApproveVoucher.Checked = oSchoolUserBL.CanApproveVoucher;
        chkCanCreateVoucher.Checked = oSchoolUserBL.CanCreateVoucher;
        chkInternalUser.Checked = oSchoolUserBL.InternalUser;
        chkPublishorUnpublishExam.Checked = oSchoolUserBL.CanPublishUnpublishExam;
        if (chkCanCreateVoucher.Checked)
        {
            chkCanSelfApprove.Checked = oSchoolUserBL.CanSelfApprove;
            chkCanSelfApprove.InputAttributes.Remove("disabled");
        }
        hidPassword.Value = oSchoolUserBL.Password;
        chkCanDeleteVoucher.Checked = oSchoolUserBL.CanDeleteVoucher;
        chkCanEditOldFinancialYear.Checked = oSchoolUserBL.CanEditOldFinancialYear;
        cmbDesignation.SelectedValue = oSchoolWiseSupervisorBL.Designation_Id.ToString();
        txtUserName.Text = oSchoolUserBL.Login;
        txtPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        txtConfirmPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        txtMobileNo.Text = oSchoolWiseSupervisorBL.Mobile_Number;
        txtEmergencyNo.Text = oSchoolUserBL.EmergencyContact;
        cmbSalutation.SelectedValue = Convert.ToString(oSchoolWiseSupervisorBL.Salutation_Id);
        oSchoolWiseSupervisorBL.User_Id = oSchoolUserBL.UserId;
        chkAcademicApplicable.Checked = oSchoolWiseSupervisorBL.IsAcademicYrApplicable == Constants.C_YES;
        oSchoolWiseSupervisorBL.IsAcademicYrApplicable = Constants.C_YES;
        chkFinancialYearChangeApplicable.Checked = oSchoolWiseSupervisorBL.IsFinancialYearApplicable;
        txtpresentAddress.Text = oSchoolWiseSupervisorBL.PresentAddress; /////
        txtLocalPincode.Text = (oSchoolWiseSupervisorBL.Pincode == 0 ? string.Empty : oSchoolWiseSupervisorBL.Pincode.ToString());
        txtLocalCity.Text = oSchoolWiseSupervisorBL.City;  /////
        txtState.Text = oSchoolWiseSupervisorBL.State; ////
        if (oSchoolUserBL.sDOB != Constants.S_EMPTY_STRING && oSchoolUserBL.sDOB != Constants.S_DEFAULT_DATE
            && oSchoolUserBL.sDOB != Constants.S_DEFAULT_DATE_2 && oSchoolUserBL.sDOB != Constants.S_DEFAULT_DATE_3
            && oSchoolUserBL.sDOB != Constants.S_DEFAULT_DATE_4)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();

            txtDOB.Text = Convert.ToDateTime(oSchoolUserBL.sDOB).ToString("dd-MMM-yyyy", new CultureInfo("en"));
        }
        else
            txtDOB.Text = string.Empty;

        if (!oSchoolWiseSupervisorBL.BinaryFormatPhoto.IsNull())
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + hidUserId.Value;
        else
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + hidUserId.Value;
    }

    /// <summary>
    /// This method is used to fill tree view with navigation path name.
    /// </summary>
    /// <param name="aoDataSet"> </param>
    private void FillReportFolderNode(DataSet aoDataSet)
    {
        // Table Indices
        int I_TBL_REPORT_FOLDER_NAME = 1;
        int I_TBL_REPORT_NAME = 2;
        ViewState[S_REPORTS_TABLE] = aoDataSet.Tables[I_TBL_REPORT_NAME];
        DataTable aoDTReportFolders = aoDataSet.Tables[I_TBL_REPORT_FOLDER_NAME];
        lstvwReportFolders.DataSource = aoDTReportFolders;
        lstvwReportFolders.DataBind();
        foreach (ListViewDataItem oCurrentFolder in lstvwReportFolders.Items)
        {
            char sHasAccess = Convert.ToChar(lstvwReportFolders.DataKeys[oCurrentFolder.DisplayIndex]["HasAccess"]);
            if (sHasAccess != 'Y')
                continue;
            var chkRepFolderName = oCurrentFolder.FindControl("ChkSelect") as CheckBox;
            chkRepFolderName.Checked = true;
        }
    }

    /// <summary>
    /// Rebuilds the User Permissions Cache in the SchoolBusinessService, if the Accounts module is enabled.
    /// </summary>
    private void RebuilUserPermissionsCache()
    {
        // If the Accounts module is enabled, rebuild the user permissions cache.
        if (IsAccountsModuleEnabled)
        {
            AccountsBaseClient oAccountsBaseClient = null;
            oAccountsBaseClient = new AccountsBaseClient();
            oAccountsBaseClient.Open();
            oAccountsBaseClient.RebuildUserPermissions(miSchoolId);

            if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
                oAccountsBaseClient.Close();

        }
    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId, Constants.Action aoAction)
    {
        List<int> lstUserIds = new List<int>();
        lstUserIds.Add(aiUserId);
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, aoAction);
    }

    /// <summary>
    /// This method is used to get Adminstaff details of Account scrren access permission.
    /// </summary>
    private void GetAccountDesination()
    {
        DesignationMasterBL oDesignationMasterBL = new DesignationMasterBL();
        List<int> olstInt = oDesignationMasterBL.GetAccountDesignations();
        ViewState[S_ACCOUNT] = olstInt;
    }

    /// <summary>
    /// This method is used to Refresh The Values.
    /// </summary>
    private void RefreshValue()
    {
        hidAgeValidationCondition.Value = Resources.LocalizedResources.AgeValidationCondition;
        hidInvalidFileFormat.Value = Resources.LocalizedResources.InvalidFileFormat;
        hidDateOfBirthFutureDate.Value = Resources.LocalizedResources.DateOfBirthFutureDate;
        hidAddressBlank.Value = Resources.LocalizedResources.AddressBlank;
        hidvalLegthOfAddress.Value = Resources.LocalizedResources.valLegthOfAddress;
        hidMobileNoVal.Value = Resources.LocalizedResources.MobileNoVal;
        hidMobileDigit.Value = Resources.LocalizedResources.MobileDigit;
        hidValUserNameBlank.Value = Resources.LocalizedResources.ValUserNameBlank;
        hidvalUserNameLength.Value = Resources.LocalizedResources.valUserNameLength;
        hidvalConfirmPassword.Value = Resources.LocalizedResources.valConfirmPassword;
        hidNoteForPasswordCombination.Value = Resources.LocalizedResources.NoteForPasswordCombination;
        hidValPasswordLengh.Value = Resources.LocalizedResources.ValPasswordLengh;
        hidValForPassword.Value = Resources.LocalizedResources.ValForPassword;
        hidEmailShouldNotBlank.Value = Resources.LocalizedResources.EmailShouldNotBlank;
        hidEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        hidAgeShouldBeLessThan.Value = Resources.LocalizedResources.AgeShouldBeLessThan;
        hidyears.Value = Resources.LocalizedResources.year1;
    }

    /// <summary>
    /// This method is used to take decision about to display control in edit mode.
    /// </summary>
    public void SetControlInEditMode()
    {
        if (QueryString[S_USER_ID] != null)
            EnableDisableFields(false);
        else
            EnableDisableFields(true);
    }

    /// <summary>
    /// This method is used to enable or disable the fields.
    /// </summary>
    /// <param name="abFlag"></param>
    private void EnableDisableFields(bool abFlag)
    {
        txtUserName.Enabled = abFlag;
        txtPasswd.Enabled = abFlag;
        txtConfirmPasswd.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used fill controls for the OtherStaff Update.
    /// </summary>
    /// <param name="aiOtherStaffId"></param>
    private void FillControlsForOtherStaffUpdate(int aiOtherStaffId)
    {
        ClearFields();
        lblUpdateSucess.Text = string.Empty;
        OtherStaff oOtherStaff = moOtherStaffBL.Get(aiOtherStaffId);
        hidUserId.Value = oOtherStaff.UserId.ToString();
        hidOtherStaffID.Value = aiOtherStaffId.ToString();
        cmbSalutation.SelectedValue = oOtherStaff.SalutationId.ToString();
        txtFirstName.Text = oOtherStaff.FirstName.ToString();
        txtMiddleName.Text = oOtherStaff.MiddleName.ToString();
        txtLastName.Text = oOtherStaff.LastName.ToString();
        if (oOtherStaff.Address != null)
            txtAddress.Text = oOtherStaff.Address.ToString();
        txtDOB.Text = DateCultureConversion(Convert.ToString(oOtherStaff.DateOfBirth), string.Empty, CultureInfo.CurrentCulture.ToString());
        txtMobileNo.Text = Convert.ToString(oOtherStaff.MobileNo);
        txtEmergencyNo.Text = oOtherStaff.EmergencyNo;
        txtEmail.Text = oOtherStaff.EmailId;
        cmbDesignation.SelectedValue = oOtherStaff.DesignationId.ToString();
        hidOtherStaffID.Value = oOtherStaff.OtherStaffId.ToString();
        hidFilePath.Value = oOtherStaff.PhotoFilePath;

        if (!oOtherStaff.BinaryFormatPhoto.IsNull())
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + hidBasicDetailUserId.Value;
        else
            imgPhoto.Src = S_DEFAULT_PHOTO;
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(oOtherStaff.UserId);
        txtUserName.Text = oSchoolUserBL.Login;
        txtPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        txtConfirmPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        hidPassword.Value = oSchoolUserBL.Password;
        EnableDisableFields(false);
    }

    /// <summary>
    /// This method is used for the Clear fields of other staff.
    /// </summary>
    private void ClearFields()
    {
        cmbSalutation.SelectedValue = Constants.I_ONE.ToString();
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtDOB.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtEmergencyNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtEmail.Text = string.Empty;
        cmbDesignation.SelectedValue = Constants.S_ZERO;
        cmbSalutation.Focus();
        imgPhoto.Src = S_DEFAULT_PHOTO;
        txtUserName.Text = string.Empty;
        txtPasswd.Attributes.Add("value", string.Empty);
        txtConfirmPasswd.Attributes.Add("value", string.Empty);
        chkSendSMS.Checked = false;
        txtLicenseExpiryDate.Text = string.Empty;
        txtDriverBatch.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set controls to update staff details.
    /// </summary>
    /// <param name="aiTransportStaffId"></param>
    private void FillControlsForTransportStaffUpdate(int aiTransportStaffId)
    {
        lblUpdateSucess.Text = string.Empty;
        TransportStaffBL oTransportStaffBL = new TransportStaffBL(aiTransportStaffId, miSchoolId, miAcademicYearId);
        cmbSalutation.SelectedValue = oTransportStaffBL.SalutationId.ToString();
        txtFirstName.Text = oTransportStaffBL.FirstName.ToString();
        txtMiddleName.Text = oTransportStaffBL.MiddleName.ToString();
        txtLastName.Text = oTransportStaffBL.LastName.ToString();
        if (oTransportStaffBL.Address != null)
            txtAddress.Text = oTransportStaffBL.Address.ToString();
        else
            txtAddress.Text = string.Empty;
        if (oTransportStaffBL.DOB.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_5)
            txtDOB.Text = oTransportStaffBL.DOB.ToString(Constants.S_DATE_FORMAT);
        else
            txtDOB.Text = string.Empty;
        txtMobileNo.Text = Convert.ToString(oTransportStaffBL.MobileNo);
        txtEmergencyNo.Text = oTransportStaffBL.EmergencyContact;
        cmbDesignation.SelectedValue = oTransportStaffBL.DesignationId.ToString();
        if (cmbDesignation.SelectedItem.Text == "Driver" || cmbDesignation.SelectedItem.Text == "Transport - Driver")
            SetLicenseDetailVisibility(true);
        else
            SetLicenseDetailVisibility(false);

        if (oTransportStaffBL.DocumentFileName != null && oTransportStaffBL.DocumentFileName != string.Empty)
        {
            btnFile.Visible = true;
            string sPath = "../downloads/TransportModule/LicenseDocuments/" + oTransportStaffBL.DocumentFileName;
            btnFile.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

            hidFileUpload.Value = oTransportStaffBL.DocumentFileName;
            hidLicensceExpDate.Value = oTransportStaffBL.mdtDriverLicenseExpiryDate.ToString(Constants.S_DATE_FORMAT);
        }
        else
            btnFile.Visible = false;

        if (oTransportStaffBL.mdtDriverLicenseExpiryDate != DateTime.MinValue && oTransportStaffBL.mdtDriverLicenseExpiryDate.Date != Constants.S_DEFAULT_DATE_2.ToDateTime().Date)
            txtLicenseExpiryDate.Text = oTransportStaffBL.mdtDriverLicenseExpiryDate.ToString(Constants.S_DATE_FORMAT);

        txtDriverBatch.Text = oTransportStaffBL.DriverBatch;
        hidTransportStaffFields.Value = oTransportStaffBL.TransportStaffFieldId.ToString();
        hidTransportStaffID.Value = oTransportStaffBL.TransportStaffId.ToString();
        hidMode.Value = Constants.S_EDIT_MODE;
        hidFilePath.Value = oTransportStaffBL.PhotoFilePath;
        string sFile = ".." + hidFilePath.Value;
        string sServerFilePath = Server.MapPath("..") + hidFilePath.Value;
        hidUserId.Value = oTransportStaffBL.UserId.ToString();
        if (File.Exists(sServerFilePath))
            imgPhoto.Src = sFile;
        else
            imgPhoto.Src = S_DEFAULT_PHOTO;
    }

    /// <summary>
    /// This method is used for Save Additional details of the staff.
    /// </summary>
    private void SaveAdditionalDetails()
    {
        if (hidUserId.Value != Constants.S_ZERO)
        {
            int aiUserId = hidUserId.Value.ToInt();
            int aiReligionId = Convert.ToInt32(cmbReligion.SelectedValue);
            int aiCategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
            string asAadharNumber = txtAadharNumber.Text;
            int aiBloodgroup = Convert.ToInt32(cmbBloodGroup.SelectedValue);
            int aiMaritialStatus = Convert.ToInt32(cmbMartialStatus.SelectedValue);
            string asCast = txtCast.Text;
            moSchoolWiseSupervisorMasterBL.InsertStaffAdditionalDetails(miSchoolId, miUserId, miAcademicYearId, aiUserId, aiReligionId, aiCategoryId, asAadharNumber, aiBloodgroup, aiMaritialStatus, asCast);
            lblAdditionalMessage.Text = S_ADDITIONAL_DETAILS_SAVE_MESSAGE;
        }        
    }

    /// <summary>
    /// This method is used for the get return the additional detial of all staffs.
    /// </summary>
    private void GetAllAdditionalDetails()
    {
        int aiUserId = hidUserId.Value.ToInt();
        StaffAdditionalDetails oStaffAdditionalDetails = moSchoolWiseSupervisorMasterBL.GetAllAdditionalDetails(miSchoolId, aiUserId);
        cmbBloodGroup.SelectedValue = oStaffAdditionalDetails.BloodGroupId.ToString();
        cmbMartialStatus.SelectedValue = oStaffAdditionalDetails.MaritialStatusId.ToString();
        cmbReligion.SelectedValue = oStaffAdditionalDetails.ReligionId.ToString();
        cmbCategory.SelectedValue = oStaffAdditionalDetails.CategoryId.ToString();
        if (oStaffAdditionalDetails.AadharNumber != null)
            txtAadharNumber.Text = oStaffAdditionalDetails.AadharNumber.ToString();
        if (oStaffAdditionalDetails.Cast != null)
            txtCast.Text = oStaffAdditionalDetails.Cast.ToString();
    }

    /// <summary>
    /// This method is used For Saving Educational Details.
    /// </summary>
    private void SaveEducationalDetails()
    {
        int aiQualificationId = Convert.ToInt32(cmbQualification.SelectedValue);
        string asSpecialization = txtSpecialization.Text;
        string asYearOfPassing = txtYearOfPassing.Text;
        int aiClassId = Convert.ToInt32(cmbPassingClass.SelectedValue);
        string asUniversity = txtPassingUniversity.Text;
        moSchoolWiseSupervisorMasterBL.InsertEducationalDetails(miUserId, hidUserId.Value.ToInt(), aiQualificationId, asSpecialization, asYearOfPassing, aiClassId, asUniversity, hidEducationId.Value.ToInt());
        if (btnEducationSave.Text == S_TEXT_UPDATE)
            lblEducationMessage.Text = S_EDUCATION_DETAILS_UPDATE_MESSAGE;
        else
            lblEducationMessage.Text = S_EDUCATION_DETAILS_SAVE_MESSAGE;
        btnEducationSave.Text = S_TEXT_SAVE;
    }

    /// <summary>
    /// This method is used For Get Educational Details.
    /// </summary>
    private void GetAllEducationalDetails()
    {
        StaffAdditionalDetails oStaffAdditionalDetails = new StaffAdditionalDetails();
        List<StaffAdditionalDetails> lstStaffAdditionalDetails = moSchoolWiseSupervisorMasterBL.GetAllEducationalDetails(hidUserId.Value.ToInt());
        lstvwEducationalDetails.DataSource = lstStaffAdditionalDetails;
        lstvwEducationalDetails.DataBind();
    }

    /// <summary>
    /// This method is Used For saving Experiance Details of the staffs.
    /// </summary>
    private void SaveExperianceDetails()   ////
    {
        string asOrganisationName = txtSchoolname.Text;
        DateTime dtJoiningDate = Convert.ToDateTime(txtjoinedDate.Text);
        DateTime dtLeftDate = Convert.ToDateTime(txtLeftDate.Text);
        string designation = txtDesignation.Text;//
        int LastSalary = Convert.ToInt32(txtLastSalary.Text); //
        string duration = txtDuration.Text; //
        string JobDescription = txtJobDescription.Text; //
        string ReasonForLeaving = txtReasonForLeaving.Text;//
        string Achievement = txtAchivements.Text;    ////Achievment
        moSchoolWiseSupervisorMasterBL.InsertExperianceDetails(miUserId, hidUserId.Value.ToInt(), asOrganisationName, dtJoiningDate, dtLeftDate, hidExperienceDetailsId.Value.ToInt(), designation, LastSalary, duration, JobDescription, ReasonForLeaving, Achievement); //////
        if (btnSaveExperiance.Text == S_TEXT_UPDATE.ToString().ToUpper())
            lblSuccessMsg.Text = S_EXPERIANCE_DETAILS_UPDATE_MESSAGE;            
        else
            lblSuccessMsg.Text = S_EXPERIANCE_DETAILS_SAVE_MESSAGE;            
    }

    /// <summary>
    /// This method is Used For Get All Experiance Details of the staffs.
    /// </summary>
    private void FillExperianceListview()
    {
        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        List<StaffAdditionalDetails> lstStaffExperianceDetails = oSchoolWiseSupervisorMasterBL.GetAllExperianceDetails(hidUserId.Value.ToInt());
        if (lstStaffExperianceDetails.Count > Constants.I_ZERO)
        {
            lstvwExpDetails.Visible = true;
            lstvwExpDetails.DataSource = lstStaffExperianceDetails;
            lstvwExpDetails.DataBind();
        }
        else
            lstvwExpDetails.Visible = false;
    }

    /// <summary>
    /// This method is Used to cleare Experiance Details Related all the fields.
    /// </summary>
    private void ClearExperianceControls()
    {
        txtAchivements.Text = string.Empty;
        txtSchoolname.Text = string.Empty;
        txtjoinedDate.Text = string.Empty;
        txtLeftDate.Text = string.Empty;
        txtExpYears.Text = "00";
        txtExpMonths.Text = "00";
        hidExperienceDetailsId.Value = Constants.S_ZERO;
        btnSaveExperiance.Text = S_TEXT_SAVE;
        txtDesignation.Text = string.Empty;  //
        txtDuration.Text = string.Empty;//
        txtLastSalary.Text = string.Empty; //
        txtJobDescription.Text = string.Empty; //
        txtReasonForLeaving.Text = string.Empty; //
    }

    /// <summary>
    /// This method is Used to cleare Education Details Related all the fields.
    /// </summary>
    private void ClearEducationalControl()
    {
        cmbQualification.SelectedIndex = Constants.I_ZERO;
        txtSpecialization.Text = string.Empty;
        txtYearOfPassing.Text = string.Empty;
        cmbPassingClass.SelectedIndex = Constants.I_ZERO;
        txtPassingUniversity.Text = string.Empty;
        btnEducationSave.Text = S_TEXT_SAVE;
        hidEducationId.Value = Constants.S_ZERO;
    }

    private void HideShowControls()
    {
        if (cmbDesignation.SelectedItem.Text == "Driver" || cmbDesignation.SelectedItem.Text == "Transport - Driver")
            SetLicenseDetailVisibility(true);
        else
            SetLicenseDetailVisibility(false);
    }

    private bool SaveFileToServer(out string asFileName)
    {
        if (flDocument.HasFile)
        {
            if (flDocument.FileContent.Length > I_FILE_SIZE_LIMIT)
            {
                asFileName = flDocument.FileName;
                return false;
            }

            string sFileName = flDocument.FileName;
            string sRenamedFileName = sFileName;
            string sFolderName = Server.MapPath("..") + S_DRIVER_LICENSE_FOLDER_LOCATION;
            string sServerFilePath = sFolderName + sFileName;
            asFileName = sFileName;

            if (File.Exists(sServerFilePath))
            {
                sRenamedFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                asFileName = sRenamedFileName;
            }

            sServerFilePath = sFolderName + sRenamedFileName;
            flDocument.SaveAs(sServerFilePath);
        }
        else
            asFileName = hidFileUpload.Value;
        return true;
    }

    private void SetLicenseDetailVisibility(bool abShow)
    {
        trLicenseAttachment.Visible = abShow;
        trExpiryDate.Visible = abShow;
        trDriverBatch.Visible = abShow;
        trLicenseAttachmentNote.Visible = abShow;
        trLicenseRenew.Visible = @abShow;
    }

    #endregion --PRIVATE METHOD(s)--
}