// File Name  : OtherStaffUI.aspx.cs
// Created By : Deepak
// Date       : 07/11/2009
// Description :This class is used to add ,delete Other Staff member's or modify existing one. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using PhotoUploadEntities;
using SchoolAutoSearchService.Client;
using Utility;

public partial class OtherStaffUI : SchoolBase
{
    #region "CONSTANTS"

    private const int I_FILE_SIZE_LIMIT = 81920; // nearly 80 kb
    private const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    private const string S_COMMAND_REMOVE = "REMOVE";
    private const string S_DEFAULT_SORT_EXP = "Name";
    private const string S_COMMAND_UPDATE = "UPDATESTAFF";

    private const string S_EDIT_MODE = "EDIT";
    private const string S_MODE_NEW = "NEW";
    private const string S_UPDATE = "Update";
    private const string S_SAVE = "Save";
    private const string S_SAVE_MESSAGE = "Staff details saved successfully!!!";
    private const string S_UPDATE_MESSAGE = "Staff details updated successfully!!!";
    private const string S_DELETE_MESSAGE = "Staff details deleted successfully!!!";
    private const string S_USER_CONTROL_WIDTH = "244";
    private RetirementNoticeConfigBL moRetirementNoticeConfigBL;

    #endregion

    #region Data Member(s)

    private int miStaffUserId;
    private OtherStaffBL moOtherStaffBL;
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region "EVENTS"
    /// <summary>
    /// This event is used to fill Salutation combo,Designation combo, existing Staff Members listView 
    /// and to set the default properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {                      
            moOtherStaffBL = new OtherStaffBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {               
                GetRetirementNoticeConfig();
                SetDefaultValues();
                FillSalutationComboBox();
                FillDesignationCombobox();
                FillUserTypesCombo();
                FillExistingStaffListview();
                SetJavascriptAttributes();
                SetQueryString();
                tblUsername.Visible = false;
                RefreshValue();
                btnSave.Text = Resources.LocalizedResources.Save;
                hidbtnvalue.Value = "Save";
                ucUserBasicDetails.Width = S_USER_CONTROL_WIDTH;
                ucUserBasicDetails.HideViewImage = false;
                ucUserBasicDetails.HideDeleteImage = false;
                btnSave.Text = oResourceManager.GetString(hidbtnvalue.Value.Replace(" ", string.Empty)); 
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                SetDefaultValues();                
                btnSave.Text = oResourceManager.GetString(hidbtnvalue.Value.Replace(" ", string.Empty));

                if (lstvwOtherStaff.Items.Count > 0)
                    ControlUtility.FillListViewPagerFooterWithCulture(lstvwOtherStaff, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
                
                //FillExistingStaffListview();
                //btnSave.Text = Resources.LocalizedResources.Update;
            }
            
            lblUpdateSucess.Visible = false;
            hidServerDate.Value = Convert.ToString(DateTime.Now);
            cmbSalutation.Focus(); 
            lblErrorMsg.Visible = false;
			string sQueryString = "UserId=" + "0";
			ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to search Staff.
    /// </summary>
    /// <param name="asError"></param>
    protected void btnSearch_Click(object sender, EventArgs e)  //
    {
        try
        {
            FillExistingStaffListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Adds a sort image to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void Page_PreRenderComplete(object sender, EventArgs e)
	{
        try
		{	
			AddSortImage();
		}
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to add attribute,properties to existing Staff Members list views item control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherStaff_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                var olnkbtnOtherName = e.Item.FindControl("lnkbtnOtherName") as LinkButton;
                Label lblName = e.Item.FindControl("lblName") as Label;
            
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                Image oImg = (Image)e.Item.FindControl("imgPhotoUpload");                
                if (oImg != null)
                {
                    // If photo is uploaded
					if (oDataRowView["BinaryPhotoImage"]==DBNull.Value)
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_BLANK_PHOTO;
                    else
                        oImg.ImageUrl = Constants.S_UPLOAD_IMAGE_STATUS_TRUE;
                }
               
                if (SchoolBase.Settings.IsAaryanSchool)
                {
                    string sQuerystr = "UserId=" + miUserId + "&UserRoleId=" + Constants.UserRoles.OtherStaff.ToInt() + "&IncludeDeactivatUser=1";

                    olnkbtnOtherName.Attributes.Add("onclick", "window.open('/RITeSchool/Admin/EmployeeDetailsReportPopup.aspx?" + Utility.CommonUtility.EncryptQuerystring(sQuerystr)
                                                                        + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700'); return false;");
                    olnkbtnOtherName.Visible = true;
                    lblName.Visible = false;
                }
                else
                {
                    olnkbtnOtherName.Visible = false;
                    lblName.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of existing Staff Members list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherStaff_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwOtherStaff.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwOtherStaff, DtPgCount, Resources.LocalizedResources.PageNo , Resources.LocalizedResources.Of,Resources.LocalizedResources.OutOflst);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit and update the staff details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherStaff_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT.ToString())
            {
                ListViewDataItem ocurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = ocurrentItem.DisplayIndex;
                int iOtherStaffId = Convert.ToInt32(lstvwOtherStaff.DataKeys[iListIndex]["OtherStaffId"]);
                hidBasicDetailUserId.Value = lstvwOtherStaff.DataKeys[iListIndex]["UserId"].ToString();
                hidOtherStaffID.Value = iOtherStaffId.ToString();
                hidUserRoleid.Value =Convert.ToString( Constants.UserRoles.OtherStaff);
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    DeleteOtherStaffDetails(iOtherStaffId, hidBasicDetailUserId.Value.ToInt());
                    ucUserBasicDetails.ClearFields();
                    lblUpdateSucess.Text = Resources.LocalizedResources.OtherStaffDelete;
                    lblUpdateSucess.Visible = true;
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                {
                    //FillControlsForStaffUpdate(iOtherStaffId);
                    //btnSave.Text = Resources.LocalizedResources.Update;
                    //hidbtnvalue.Value = "Update";
                    //ucUserBasicDetails.StaffUserId = Convert.ToInt32(lstvwOtherStaff.DataKeys[iListIndex]["UserId"]);
                    //hidBasicDetailUserId.Value = lstvwOtherStaff.DataKeys[iListIndex]["UserId"].ToString();
                    //ucUserBasicDetails.InitializeFields();
                    //string sQueryString = "UserId=" + hidBasicDetailUserId.Value;
                    //ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
                   string S_PAGE =  "~/Admin/SupervisorDetailsUI.aspx";
                   string sQuerystring = "&User_Role_Id=" + hidUserRoleid.Value + "&OtherStaffId=" + hidOtherStaffID.Value + "&UserId=" + hidBasicDetailUserId.Value;
                   string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
                   string sRedirectUrl = S_PAGE + "?" + sEncrypt;
                   MasterPage oMasterPage = (MasterPage)this.Master;
                   oMasterPage.RedirectToNextPage(sRedirectUrl);
                 }                
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the list view of staff members items by Name,Designation and Mobile No..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherStaff_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise staff list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNoAndCulture(lstvwOtherStaff,Resources.LocalizedResources.PageNo ,Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save OtherStaff details as well as its configuration details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidMode.Value == "EDIT")
            {
                ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidBasicDetailUserId.Value);
                ucUserBasicDetails.ValidateProfile();
            }
            else
            {                
                ucUserBasicDetails.StaffUserId = 0;
                ucUserBasicDetails.ValidateProfile();
            }

            SaveStaffDetails();
            if (QueryString["Is_Configured"] != Constants.S_YES)
               SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.OtherStaff));

            if (chkSendSMS.Checked)
                SendSmsToOtherStaff(miStaffUserId);
            chkSendSMS.Checked = false;

            ClearFields();
            ucUserBasicDetails.StaffUserId = miStaffUserId;
            ucUserBasicDetails.PopulateUserBasicDetails();
            ucUserBasicDetails.ClearFields();         
			// this is to clear session image data captured web cam.
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            EnableDisableFields(true);
        }
        catch (SqlException ex)
        {
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (DuplicateUserException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidBasicDetailUserId.Value = string.Empty;
            ucUserBasicDetails.ClearFields();
            lblErrorMsg.Visible = false;
            ClearFields();
            EnableDisableFields(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    { 
        try
        {
            FillExistingStaffListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
   
    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This Method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Other_User_Related));
       
    }


    /// <summary>
    /// This method sets the QueryString.
    /// </summary>

    private void SetQueryString()
    {
        hidUserRoleid.Value = Convert.ToString(Constants.UserRoles.OtherStaff);
        string sQueryString = "&User_Role_Id=" + hidUserRoleid.Value;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        hidQueryString.Value = sEncrypt;
        btnAdd.Attributes.Add("onclick", "window.open('../Admin/SupervisorDetailsUI.aspx?" + sEncrypt
                                   + "' , '_self'); return false;");
    }
    /// <summary>
    /// This method is used set data source  to ListView
    /// </summary>
    private void FillExistingStaffListview()
    {
        lstvwOtherStaff.DataSourceID = ObjDSOtherStaff.ID;
        lstvwOtherStaff.DataBind();
    }

    /// <summary>
    /// This method is used to retrieve retirement notice config. of admin
    /// </summary>
    private void GetRetirementNoticeConfig()
    {
        moRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
        List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = moRetirementNoticeConfigBL.GetAll();
        int iRetAge = lstRetirementNoticeConfig.Where(obj => obj.UserRole.Id == Constants.UserRoles.OtherStaff.ToInt()).Select(obj => obj.RetirementAge).FirstOrDefault();
        hidRetirementAge.Value = System.DateTime.Now.AddYears(-1 * iRetAge).ToString("dd-MMM-yyyy");
        hidRetAge.Value = iRetAge.ToString();
    }
    /// <summary>
    /// This method is used to fill salutation combo.
    /// </summary>
    private void FillSalutationComboBox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
    }

    /// <summary>
    /// This method is used to fill designation combo.
    /// </summary>
    private void FillDesignationCombobox()
    {
        int iUserRoleId = Convert.ToInt32(Constants.UserRoles.OtherStaff);
        DataTable oDataTable = SchoolWiseSupervisorMasterBL.GetSupervisorDesignations(iUserRoleId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbDesignation, "Teacher_Designation_Id", "Teacher_Designation_Name", Constants.S_SELECT);
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
    /// This Method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        btnSave.Text = Resources.LocalizedResources.Save;
        hidbtnvalue.Value = "Save";
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
        hidMode.Value = S_MODE_NEW;
        imgPhoto.Src = S_DEFAULT_PHOTO;
        txtUserName.Text = string.Empty;
        
        txtPasswd.Attributes.Add("value", string.Empty);
        txtConfirmPasswd.Attributes.Add("value", string.Empty);
        chkSendSMS.Checked = false;
    }
   
    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwOtherStaff.SortDirection.ToString() == "Ascending" || lstvwOtherStaff.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;

        if (lstvwOtherStaff.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwOtherStaff.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwOtherStaff.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This Method is used to delete Staff details.
    /// </summary>
    private void DeleteOtherStaffDetails(int aiOtherStaffId, int aiUserId)
    {   
        moOtherStaffBL.Delete(aiOtherStaffId, aiUserId);
        DataTable oDT = moOtherStaffBL.GetAll();
        if (oDT.Rows.Count == Constants.I_ZERO)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.OtherStaff));
        RefreshStaffCache(hidBasicDetailUserId.Value.ToInt(), Constants.Action.Update);
        FillExistingStaffListview();
        ClearFields();
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = S_DELETE_MESSAGE;
    }

    /// <summary>
    /// This Method is used to update Staff details.
    /// </summary>
    private void FillControlsForStaffUpdate(int aiOtherStaffId)
    {
        ClearFields();
        lblUpdateSucess.Text = string.Empty;        
        OtherStaff oOtherStaff = moOtherStaffBL.Get(aiOtherStaffId);
        hidUserID.Value = oOtherStaff.UserId.ToString();
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
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + hidBasicDetailUserId.Value;     
        else
			imgPhoto.Src = S_DEFAULT_PHOTO;

        hidMode.Value = S_EDIT_MODE;

        SchoolUserBL oSchoolUserBL = new SchoolUserBL(oOtherStaff.UserId);
        txtUserName.Text = oSchoolUserBL.Login;
        txtPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        txtConfirmPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        hidPassword.Value = oSchoolUserBL.Password;
        EnableDisableFields(false);        
    }

    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload aoFileUploadLogo)
    {
        const int I_HEIGHT_LIMIT = 151;
        const int I_WIDTH_LIMIT = 112;
        string sFileName = aoFileUploadLogo.FileName;
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
            // delete existing logo
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
    /// <param name="asServerFilePath"></param>
    /// <param name="aiHeight"></param>
    /// <param name="aiWidth"></param>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string ValidateFile(string asServerFilePath, int aiHeight, int aiWidth, string asFileName)
    {
        string sReturnErrorMsg = string.Empty;
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
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
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + aiHeight + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }

                if (oImg.Width > aiWidth)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + aiWidth + "px." + Resources.LocalizedResources.Greater;
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
    /// This Method is used to save Staff details.
    /// </summary>
    private void SaveStaffDetails()
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
        if(txtPasswd.Enabled==true)
            sPassword=txtPasswd.Text;
        else
        sPassword = hidPassword.Value;
        string sIsLocked = Settings.EnableOtherStaffLogin?Constants.S_NO : Constants.S_YES;

        SchoolUserBL oSchoolUserBLObj = new SchoolUserBL();
        oSchoolUserBLObj.Login = sUserName;

        oSchoolUserBLObj.UserId = 0;
        if (hidBasicDetailUserId.Value.Trim() != string.Empty)
            oSchoolUserBLObj.UserId = Convert.ToInt32(hidBasicDetailUserId.Value);

        oSchoolUserBLObj.SchoolId = miSchoolId;
        if (oSchoolUserBLObj.IsUserLoginDuplicate())
            throw new DuplicateUserException(Resources.LocalizedResources.DuplicateUserName);

        if (hidMode.Value != S_EDIT_MODE)
        {
            miStaffUserId = moOtherStaffBL.Insert(sUserName, sPassword, sIsLocked);
            hidBasicDetailUserId.Value = miStaffUserId.ToString();
            InsertShiftDetails();
            InsertWeekendDetails();
        }
        else
        {
            SchoolUserBL oSchoolUserBL = CreateSchoolUserOtherStaffObject();
            oSchoolUserBL.UserId = Convert.ToInt32(hidUserID.Value);
            moOtherStaffBL.OtherStaff.OtherStaffId = Convert.ToInt32(hidOtherStaffID.Value);
            oSchoolUserBL.UpdateOtherStaffSchoolUser(oImageBinaryData, moOtherStaffBL.OtherStaff.OtherStaffId, moOtherStaffBL.OtherStaff.PhotoFilePath, sUserName, sPassword);
            if (hidBasicDetailUserId.Value != string.Empty || hidBasicDetailUserId.Value != Constants.S_ZERO)
                miStaffUserId = Convert.ToInt32(hidBasicDetailUserId.Value);            
        }

        FillExistingStaffListview();
        if (btnSave.Text == Resources.LocalizedResources.Save)
        {
            lblUpdateSucess.Text = Resources.LocalizedResources.OtherStaffSave;
            RefreshStaffCache(miStaffUserId, Constants.Action.Insert);
        }
        else
        {
            lblUpdateSucess.Text = Resources.LocalizedResources.OtherStaffUpdate;
            btnSave.Text = Resources.LocalizedResources.Save;
            hidbtnvalue.Value = "Save";
            RefreshStaffCache(miStaffUserId, Constants.Action.Update);
        }

        lblUpdateSucess.Visible = true;
        lblErrorMsg.Visible = false;
        hidMode.Value = S_MODE_NEW;
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

    /// <summary>
    /// Create the user role's object for the available values.
    /// </summary>
    /// <returns>SchoolUserBL</returns>
    private SchoolUserBL CreateSchooUserObject()
    {
        SchoolUserBL oOtherStaffBL = new SchoolUserBL
        {
           DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue),
           Email = txtEmail.Text.Trim(),
           SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue),
           UserRoleId = Convert.ToInt32(Constants.UserRoles.OtherStaff),
           SchoolId = miSchoolId,
           UpdatedBy = miUserId.ToString(),
           InsertedBy = miUserId.ToString(),
           UpdatedDate = System.DateTime.Now.ToString("MM/dd/yyyy"),
           FirstName = string.Empty,
           LastName = string.Empty,
           MiddleName = string.Empty,
           sDOB = txtDOB.Text != string.Empty ? txtDOB.Text.Trim() : string.Empty
        };

        return oOtherStaffBL;
    }

    /// <summary>
    /// This Method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidSortDirection.Value = Constants.S_ASCENDING;
        imgPhoto.Src = S_DEFAULT_PHOTO;

        if (!Settings.EnableOtherStaffLogin)
        {
            chkSendSMS.Checked = false;
            trSendSMS.Visible = false;
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

    private void RefreshValue()
    {
        hidInvalidFileFormat.Value = Resources.LocalizedResources.InvalidFileFormat;
        hidDateOfBirthFutureDate.Value = Resources.LocalizedResources.DateOfBirthFutureDate;
        hidMobileDigit.Value = Resources.LocalizedResources.MobileDigit;
        hidMobileNoVal.Value = Resources.LocalizedResources.MobileNoVal;
        hidAreYouSureYouWantToDeleteThisRecords.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        hidAddressBlank.Value = Resources.LocalizedResources.AddressBlank;
        hidvalLegthOfAddress.Value = Resources.LocalizedResources.valLegthOfAddress;
        hidvalAgeLength.Value = Resources.LocalizedResources.AgeValidationCondition;
        hidyears.Value = Resources.LocalizedResources.years;
        hidShouldBeLessThan.Value = Resources.LocalizedResources.AgeShouldBeLessThan;
        hidvalConfirmPassword.Value = Resources.LocalizedResources.valConfirmPassword;        
        hidValPasswordLengh.Value = Resources.LocalizedResources.ValPasswordLengh;
        hidValForPassword.Value = Resources.LocalizedResources.ValForPassword;
        hidValUserNameBlank.Value = Resources.LocalizedResources.ValUserNameBlank;
        hidValUserNameBlank.Value = Resources.LocalizedResources.ValUserNameBlank;
        hidValUserNameBlank.Value = Resources.LocalizedResources.ValUserNameBlank;
        hidvalUserNameLength.Value = Resources.LocalizedResources.valUserNameLength;
        hidvalBlankConfirmPassword.Value = Resources.LocalizedResources.valBlankConfirmPassword;
        hidNoteForPasswordCombination.Value = Resources.LocalizedResources.NoteForPasswordCombination;
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
    /// This method is used to enable or disable the control.
    /// </summary>
    /// <param name="aiFlag"></param>
    private void EnableDisableFields(bool aiFlag)
    {
        txtUserName.Enabled = aiFlag;
        txtPasswd.Enabled = aiFlag;
        txtConfirmPasswd.Enabled = aiFlag;
    }

    /// <summary>
    /// This method is used to Fill User Type Combobox.
    /// </summary>
    private void FillUserTypesCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable dtUSerTypes = oMasterDataCollectionBL.GetAllUserTypes();

        foreach (DataRow dr in dtUSerTypes.Rows)
        {
            if (dr["UserType"].ToString() == "Internal")
                dr.Delete();
        }

        ControlUtility.FillDropDownList(dtUSerTypes, ref ddlUserType,
                                       "UserTypeId",
                                      "UserType", string.Empty);
        ddlUserType.SelectedValue = Constants.S_ONE;
    }
    #endregion
}
