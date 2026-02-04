/* File Name :- AdminProfileUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to manipulate the admin profile details.
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using PhotoUploadEntities;
using SchoolAutoSearchService.Client;
using SchoolBusinessService;
using SchoolEntities.Admin;
using Utility;

public partial class AdminProfileUI : SchoolBase
{
    #region -- CONSTANT(s) --

    const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    private RetirementNoticeConfigBL moRetirementNoticeConfigBL;
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #endregion -- CONSTANT(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Returns true if the Accounts module is enabled, false otherwise
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    #endregion -- PROPERTIES --

    #region -- EVENT(s) --

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortDirection.Value == string.Empty)
                hidSortDirection.Value = Constants.S_ASCENDING;

            AddSortImage(lstvwAdminDetails, "Name", hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to initialize controls,set javascript attributes and fill admin details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                RefreshValue();
                GetRetirementNoticeConfig();
                InitializeControls();                
                FillAdminDetails();
                SetJavascriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                    InitializeControls();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                txtUserName.ToolTip = Resources.LocalizedResources.ToolTipUserName;
                txtPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
                txtConfirmPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
                valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
            }
            EnableDisableFields(true);
            ucUserBasicDetails.Width = "380";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to upadate the admin details .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserBL oSchoolUserBL = PopulateAdminDetails();
            if (moUserRole == Constants.UserRoles.Admin)
            {
                string sSalutation = cmbSalutation.SelectedItem.Text;
                string sLoginUserName = sSalutation + " " + oSchoolUserBL.FirstName;
                Session[Constants.S_SESSION_USER_NAME] = sLoginUserName;
            }
            ucUserBasicDetails.StaffUserId = oSchoolUserBL.UserId;
            ucUserBasicDetails.ValidateProfile();         
            ucUserBasicDetails.PopulateUserBasicDetails();            
            ucUserBasicDetails.InitializeFields();
            lblUpdateSucess.Visible = true;
            FillAdminDetails();
            lblUpdateSucess.Text = (hidUserId.Value == Constants.S_ZERO ? "<b>" + Resources.LocalizedResources.msgProfileSaveMessage + "</b>" : "<b>" + Resources.LocalizedResources.UpdateMsgProfile + "</b>");
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidIsPhotoCaptured.Value = "N";
            if (chkSendSMS.Checked)
                SendSmsToUser(oSchoolUserBL.UserId);
            chkSendSMS.Checked = false;
            ClearFields();
            RefreshStaffCache(oSchoolUserBL.UserId);
        }
        catch (SqlException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (DuplicateUserException ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Text = oResourceManager.GetString(ex.Message.Replace(" ", string.Empty));
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            // this is to clear session image data captured web cam.
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
            hidIsPhotoCaptured.Value = "N";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAdminDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Admin oAdmin = e.Item.DataItem as Admin;
                Label lblDOB = e.Item.FindControl("lblDOB") as Label;
                lblDOB.Text = (oAdmin.DOB.ToString(Constants.S_DATE_FORMAT) == "01-Jan-0001" ? string.Empty : oAdmin.DOB.ToString(Constants.S_DATE_FORMAT));

                ImageButton imgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                int iDesignationId = lstvwAdminDetails.DataKeys[e.Item.DisplayIndex]["DesignationId"].ToInt();
                if (iDesignationId == Convert.ToInt32(Constants.AdminDesignations.ChiefAdministratorOfficer))
                {
                    imgBtnDelete.Visible = false;

                    var tr = e.Item.FindControl("trRow") as HtmlTableRow;
                    tr.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightBlue");
                }
                else
                    imgBtnDelete.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel current operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update and delete existing profile.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAdminDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iUserId = lstvwAdminDetails.DataKeys[e.Item.DisplayIndex]["UserId"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    InitializeFields(iUserId);
                    btnSave.Text = Resources.LocalizedResources.Update;
                    EnableDisableFields(false);
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    SchoolUserBL oSchoolUserBL = new SchoolUserBL();
                    oSchoolUserBL.DeleteAdminDetails(miSchoolId, iUserId, miUserId);

                    if (hidUserId.Value == iUserId.ToString())
                        ClearFields();

                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = "<B>" + Resources.LocalizedResources.msgProfileDeleteMessage + "</B>";

                    FillAdminDetails();
                }
            }
        }
        catch (SqlException sqlex)
        {
            lblErrorMsg.Text = sqlex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAdminDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortDirection.Value == string.Empty || hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillAdminDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion  -- EVENT(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload aFileUploadLogo)
    {
        const int I_HEIGHT_LIMIT = 151;
        const int I_WIDTH_LIMIT = 112;
        string sFileName = aFileUploadLogo.FileName;
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
            oImg = null;
        }

        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > Constants.I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal; ;
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }

    /// <summary>
    /// This method is used to retrieve retirement notice config. of admin
    /// </summary>
    private void GetRetirementNoticeConfig()
    {
        moRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
        List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = moRetirementNoticeConfigBL.GetAll();
        int iRetAge = lstRetirementNoticeConfig.Where(obj => obj.UserRole.Id == Constants.UserRoles.Admin.ToInt()).Select(obj => obj.RetirementAge).FirstOrDefault();
        hidRetirementAge.Value = System.DateTime.Now.AddYears(-1 * iRetAge).ToString("dd-MMM-yyyy");
        hidRetAge.Value = iRetAge.ToString();

    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { BtnCancel, btnSave });
        btnSave.Attributes["onclick"] = "javascript:DisableButtons(this)";
        string sQueryString = "UserId=" + miUserId;
        ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
        ucUserBasicDetails.HideViewImage = false;
        ucUserBasicDetails.HideDeleteImage = false;
    }

    /// <summary>
    /// This method is used to extract admin details from school user. 
    /// </summary>
    private void FillAdminDetails()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        List<Admin> lstAdmins = oSchoolUserBL.GetAllAdmins(miSchoolId);

        if (hidSortDirection.Value == string.Empty || hidSortDirection.Value == Constants.S_ASCENDING)
            lstAdmins = lstAdmins.OrderBy(adm => adm.FullName).ToList();
        else
            lstAdmins = lstAdmins.OrderByDescending(adm => adm.FullName).ToList();

        lstvwAdminDetails.DataSource = lstAdmins;
        lstvwAdminDetails.DataBind();
    }

    private void InitializeFields(int aiUserId)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        List<Admin> lstAdmins = oSchoolUserBL.GetAllAdmins(miSchoolId, aiUserId);
        Admin oAdmin = lstAdmins[0];

        if (oAdmin != null)
        {
            hidUserId.Value = aiUserId.ToString();
            tblUsername.Visible = true;
            txtFirstName.Text = oAdmin.FirstName;
            txtMiddleName.Text = oAdmin.MiddleName;
            txtLastName.Text = oAdmin.LastName;
            txtAddress.Text = oAdmin.Address;
            txtMobileNo.Text = oAdmin.MobileNumber;
            txtEmergencyNo.Text = oAdmin.EmergencyContact;
            txtEmail.Text = oAdmin.Email;
            txtUserName.Text = oAdmin.Login;
            chkCanApproveRequisitions.Checked = oAdmin.CanApproveRequisition;
            chkCanCreateGeneralRequisition.Checked = oAdmin.CanCreateGeneralRequisition;
            chkCanSanctionLeave.Checked = oAdmin.CanSanctionLeave;
            chkCanApproveVoucher.Checked = oAdmin.CanApproveVoucher;
            chkCanCreateVoucher.Checked = oAdmin.CanCreateVoucher;
            chkPublishorUnpublishExam.Checked = oAdmin.CanPublishUnpublishExam;
            if (chkCanCreateVoucher.Checked)
            {
                chkCanSelfApprove.Checked = oAdmin.CanSelfApprove;
                chkCanSelfApprove.InputAttributes.Remove("disabled");
            }

            chkCanDeleteVoucher.Checked = oAdmin.CanDeleteVoucher;
            chkCanEditOldFinancialYear.Checked = oAdmin.CanEditOldFinancialYear;
            chkShowAllSentSMS.Checked = oAdmin.ShowAllSentSMS;
            cmbSalutation.SelectedValue = Convert.ToString(oAdmin.SalutationId);
            cmbDesignations.SelectedValue = Convert.ToString(oAdmin.DesignationId);
            if (oAdmin.DOB != DateTime.MinValue)
            {
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();

                txtDOB.Text = oAdmin.DOB.ToString("dd-MMM-yyyy", new CultureInfo("en"));
            }
            else
                txtDOB.Text = string.Empty;

            hidFilePath.Value = oAdmin.PhotoFilePath;
            if (!oAdmin.BinaryPhotoImage.IsNull())
                imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + oAdmin.UserId;
            else
                imgPhoto.Src = S_DEFAULT_PHOTO;

        }
        txtPasswd.Attributes.Add("value", oAdmin.Password);
        txtConfirmPasswd.Attributes.Add("value", oAdmin.Password);
        ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value);
        ucUserBasicDetails.ShowGradePayOnStaffProfileScreen = Settings.ShowGradePayOnStaffProfileScreen;
        ucUserBasicDetails.InitializeFields();
    }

    /// <summary>
    /// This method is used to set the updated values of admin profile details.
    /// </summary>
    /// <returns></returns>
    private SchoolUserBL PopulateAdminDetails()
    {
        // Create the user role's object for the available values.
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.FirstName = txtFirstName.Text.ToTitleCase();
        oSchoolUserBL.MiddleName = txtMiddleName.Text.ToTitleCase();
        oSchoolUserBL.LastName = txtLastName.Text.ToTitleCase();
        oSchoolUserBL.Address = txtAddress.Text.Trim();
        oSchoolUserBL.Mobile_Number = txtMobileNo.Text.Trim();
        oSchoolUserBL.EmergencyContact = txtEmergencyNo.Text.Trim();
        oSchoolUserBL.Email = txtEmail.Text.Trim();
        oSchoolUserBL.Login = txtUserName.Text.Trim();
        oSchoolUserBL.Password = txtPasswd.Text;
        oSchoolUserBL.UserRoleId = (cmbDesignations.SelectedValue.ToInt() == Convert.ToInt32(Constants.AdminDesignations.ChiefAdministratorOfficer) ? Convert.ToInt32(Constants.UserRoles.Admin) : Convert.ToInt32(Constants.UserRoles.ExAdmin));        
        oSchoolUserBL.UpdatedBy = miUserId.ToString();
        oSchoolUserBL.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        oSchoolUserBL.DesignationId = Convert.ToInt32(cmbDesignations.SelectedValue);
        oSchoolUserBL.UpdatedDate = System.DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        if (chkCanApproveRequisitions.Checked)
            oSchoolUserBL.CanApproveRequisition = Constants.C_YES;
        else
            oSchoolUserBL.CanApproveRequisition = Constants.C_NO;

        if (chkCanCreateGeneralRequisition.Checked)
            oSchoolUserBL.CanCreateGeneralRequisition = Constants.C_YES;
        else
            oSchoolUserBL.CanCreateGeneralRequisition = Constants.C_NO;
        if (chkCanSanctionLeave.Checked)
            oSchoolUserBL.CanSanctionLeave = Constants.C_YES;
        else
            oSchoolUserBL.CanSanctionLeave = Constants.C_NO;

        oSchoolUserBL.CanCreateVoucher = chkCanCreateVoucher.Checked;
        oSchoolUserBL.CanApproveVoucher = chkCanApproveVoucher.Checked;
        oSchoolUserBL.CanPublishUnpublishExam = chkPublishorUnpublishExam.Checked;

        // If the user can create a voucher, we set the appropriate self approve flag
        if (chkCanCreateVoucher.Checked)
            oSchoolUserBL.CanSelfApprove = chkCanSelfApprove.Checked;

        // else, we disable the self approve checkbox, since he can not self approve if he does not have create rights.
        else
            chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");

        oSchoolUserBL.CanDeleteVoucher = chkCanDeleteVoucher.Checked;
        oSchoolUserBL.CanEditOldFinancialYear = chkCanEditOldFinancialYear.Checked;
        oSchoolUserBL.ShowAllSentSMS = chkShowAllSentSMS.Checked;
        oSchoolUserBL.CanReceiveMail = Constants.C_NO;



        if (txtDOB.Text != string.Empty)
            oSchoolUserBL.sDOB = Convert.ToDateTime(txtDOB.Text).ToString(Constants.S_DATE_FORMAT_MARATHI);
        else
            oSchoolUserBL.sDOB = string.Empty;
        oSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
        oSchoolUserBL.SchoolId = miSchoolId;
        string sFileName = string.Empty;
        if (UploadPhoto.HasFile)
        {
            sFileName = SaveFileOnServer(UploadPhoto);
            oSchoolUserBL.PhotoFilePath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            Byte[] ImageBinaryData = GetByteArrayFromFileField(UploadPhoto);
            oSchoolUserBL.BinaryPhotoImage = ImageBinaryData;
        }
        else if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
        {
            List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
            var oImage = lstImageData.Where(lst => lst.UserID == hidUserId.Value.ToInt()).LastOrDefault();
            if (!oImage.IsNull())
                oSchoolUserBL.BinaryPhotoImage = oImage.ImagesData;
        }
        else
        {

            Byte[] ImageBinaryData = { };
            oSchoolUserBL.PhotoFilePath = string.Empty;
            oSchoolUserBL.BinaryPhotoImage = ImageBinaryData;
        }

        if (hidUserId.Value == string.Empty || hidUserId.Value == Constants.S_ZERO)
            oSchoolUserBL.UserId = oSchoolUserBL.InsertSchoolUserDetails();
        else
            oSchoolUserBL.UpdateSchoolUser();

        if (!oSchoolUserBL.PhotoFilePath.IsNullOrEmpty())
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + oSchoolUserBL.UserId;

        // Rebuild the UserPermissions Cache in the SchoolBusinessService
        RebuilUserPermissionsCache();

        // Update permission in Session
        Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] = oSchoolUserBL.CanEditOldFinancialYear;

        return oSchoolUserBL;
    }

    /// <summary>
    /// This method is used to initialize controls and variables.
    /// </summary>
    private void InitializeControls()
    {
        txtUserName.ToolTip = Constants.S_LOGIN_TOOL_TIP;
        txtPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        txtConfirmPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);

        FillDesignationCombo();

        trchkCanApproveRequisitions.Visible = Settings.EnableInventoryModule;
        trchkCanCraeteGenerelRequisition.Visible = Settings.EnableInventoryModule;

        trAccountsRow1.Visible = IsAccountsModuleEnabled;
        trAccountsRow2.Visible = IsAccountsModuleEnabled;
        trAccountsRow3.Visible = IsAccountsModuleEnabled;
        trAccountsRow4.Visible = IsAccountsModuleEnabled;
        trAccountsRow5.Visible = IsAccountsModuleEnabled;

        if (IsAccountsModuleEnabled)
            chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");

        hidServerDate.Value = Convert.ToString(DateTime.Today);
        cmbSalutation.Focus();

        ucUserBasicDetails.ShowGradePayOnStaffProfileScreen = Settings.ShowGradePayOnStaffProfileScreen;
        ucUserBasicDetails.InitializeFields();
    }

    /// <summary>
    /// This method is used to fill up designation combo box.
    /// </summary>
    private void FillDesignationCombo()
    {
        DesignationMasterBL oDesignationMasterBL = new DesignationMasterBL();
        List<MasterEntities.DesignationMaster> lstDesignations = oDesignationMasterBL.GetAll();
        lstDesignations = lstDesignations.Where(dgn => dgn.UserRoleId == Constants.UserRoles.Admin.ToInt() || dgn.UserRoleId == Constants.UserRoles.ExAdmin.ToInt()).ToList();
        ListSource.FillDropDownList(lstDesignations, cmbDesignations, "Designation", "DesignationId", Constants.S_SELECT);
    }

    /// <summary>
    /// Rebuilds the User Permissions Cache in the SchoolBusinessService, if the Accounts module is enabled.
    /// </summary>
    private void RebuilUserPermissionsCache()
    {
        // If the Accounts module is enabled, rebuild the user permissions cache.
        if (IsAccountsModuleEnabled)
        {
            AccountsBaseClient oAccountsBaseClient = new AccountsBaseClient();
            try
            {
                oAccountsBaseClient.Open();
                oAccountsBaseClient.RebuildUserPermissions(miSchoolId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an error rebuilding User permissions after updating Admin profile.");
            }
            finally
            {
                if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
                    oAccountsBaseClient.Close();
            }
        }
    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId)
    {
        List<int> lstUserIds = new List<int>();
        lstUserIds.Add(aiUserId);
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, Constants.Action.Update);
    }

    private void RefreshValue()
    {
        hidAgeShouldBeLessThan.Value = Resources.LocalizedResources.AgeShouldBeLessThan;
        hidyears.Value = Resources.LocalizedResources.year1;
        hidAgeValidationCondition.Value = Resources.LocalizedResources.AgeValidationCondition;
        hidInvalidFileFormat.Value = Resources.LocalizedResources.InvalidFileFormat;
        hidDateOfBirthFutureDate.Value = Resources.LocalizedResources.DateOfBirthFutureDate;
        hidvalLegthOfAddress.Value = Resources.LocalizedResources.valLegthOfAddress;
        hidAddressBlank.Value = Resources.LocalizedResources.AddressBlank;
        hidEmailShouldNotBlank.Value = Resources.LocalizedResources.EmailShouldNotBlank;
        hidEmailValidation.Value = Resources.LocalizedResources.EmailValidation;
        hidValForPassword.Value = Resources.LocalizedResources.ValForPassword;
        hidValPasswordLengh.Value = Resources.LocalizedResources.ValPasswordLengh;
        hidNoteForPasswordCombination.Value = Resources.LocalizedResources.NoteForPasswordCombination;
        hidvalConfirmPassword.Value = Resources.LocalizedResources.valConfirmPassword;
        hidValUserNameBlank.Value = Resources.LocalizedResources.ValUserNameBlank;
        hidvalUserNameLength.Value = Resources.LocalizedResources.valUserNameLength;
        hidMobileNoVal.Value = Resources.LocalizedResources.MobileNoVal;
        hidMobileDigit.Value = Resources.LocalizedResources.MobileDigit;
        hidMobileNumberBlank.Value = Resources.LocalizedResources.MobileNumberBlank;
        hidconfirmDelete.Value = Resources.LocalizedResources.AlertDeleterecord;
    }


    /// <summary>
    /// This method is used to clear all fields and selections.
    /// </summary>
    private void ClearFields()
    {
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtEmergencyNo.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtUserName.Text = string.Empty;
        chkCanApproveRequisitions.Checked = false;
        chkCanCreateGeneralRequisition.Checked = false;
        chkCanSanctionLeave.Checked = false;
        chkCanApproveVoucher.Checked = false;
        chkCanCreateVoucher.Checked = false;
        chkPublishorUnpublishExam.Checked = false;

        chkCanSelfApprove.Checked = false;
        chkCanSelfApprove.InputAttributes.Remove("disabled");

        chkCanDeleteVoucher.Checked = false;
        chkCanEditOldFinancialYear.Checked = false;
        chkShowAllSentSMS.Checked = false;
        cmbSalutation.ClearSelection();
        cmbDesignations.ClearSelection();
        txtDOB.Text = string.Empty;
        hidFilePath.Value = string.Empty;
        imgPhoto.Src = S_DEFAULT_PHOTO;

        txtPasswd.Attributes.Add("value", string.Empty);
        txtConfirmPasswd.Attributes.Add("value", string.Empty);
        ucUserBasicDetails.StaffUserId = 0;
        ucUserBasicDetails.ClearFields();

        hidUserId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
    }

    /// <summary>
    /// This method is used to enable the UserName and Password Control.
    /// </summary>
    private void EnableDisableFields(bool bFlag)
    {
        txtUserName.Enabled = bFlag;
        txtPasswd.Enabled = bFlag;
        txtConfirmPasswd.Enabled = bFlag;       
    }
    #endregion -- PRIVATE METHOD(s) --
}