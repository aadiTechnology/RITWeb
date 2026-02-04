using System;
using System.IO;
using System.Reflection;
using PayrollEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Resources;
using Utility;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// UserBasicDetailsUC is Used to Save and Get Basic details of User.
/// </summary>
public partial class UserBasicDetailsUC : System.Web.UI.UserControl
{
    private int iStaffUserId;
    private bool  bShowGradePayOnStaffProfileScreen;
    private const string S_UPLOAD_FILE_PATH_FOR_PAN = "\\DOWNLOADS\\PAN Attachment\\";
    private const string S_UPLOAD_FILE_PATH_FOR_AADHAR = "\\DOWNLOADS\\Aadhar Cards\\";
    private const int I_FILE_SIZE_LIMIT = 2097152;  // File limit is 1 MB
    private const int I_FILE_SIZE_LIMIT_AADHAR = 3145728;  // File limit is 3 MB
    private const string S_FILE_SIZE_EXCEED_ERROR = "File size should not be greater than 2 MB.";
    private const string S_FILE_SIZE_EXCEED_AADHAR_ERROR = "File size should not be greater than 3 MB.";
    private const string S_FILE_ALREADY_EXISTS = "The given file is already exixts.";


    private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;

    #region Properties    
    //This property is used to set width of the user control according to page size
    public string Width
    {
        set { tdUC.Width = value; }
    }


    public bool HideViewImage
    {
        set { btnDownload.Visible = value; }
    }

    public bool HideDeleteImage
    {
        set { imgBtnDelete.Visible = value; }
    }

    //This property is used to set UserId for saving the details.
    public int StaffUserId
    {
        get { return iStaffUserId; }
        set { iStaffUserId = value; }
    }
    public bool ShowGradePayOnStaffProfileScreen
    {
        get { return bShowGradePayOnStaffProfileScreen; }
        set { bShowGradePayOnStaffProfileScreen = value; }
    }

    #endregion     

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCombo();
            FillBloodGroups();
        }
        RefreshValues();
    }
    #region Methods

     /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
            hidJoiningDateValidation.Value = Resources.LocalizedResources.JoiningDateValidation;
           hidPermanentDateValidation.Value =Resources.LocalizedResources.PermanentDateValidation;
           hidPermanentJoiningDateValidation.Value=Resources.LocalizedResources.PermanentJoiningDateValidation;
           hidJoiningDateValidation1.Value=Resources.LocalizedResources.JoiningDateValidation1;
           hidResignationDateValidation.Value=Resources.LocalizedResources.ResignationDateValidation;
           hidResignationDateValidation1.Value=Resources.LocalizedResources.ResignationDateValidation1;
           hidResignationDateValidation2.Value = Resources.LocalizedResources.ResignationDateValidation2;
    }

    /// <summary>
    /// This method is used to get user basic details.
    /// </summary>
    /// 

    public void InitializeCombo()
    {
        StaffStatusBL oStaffStatusBL = new StaffStatusBL();
        List<StaffStatusDetails> olstStaffStatusDetails = oStaffStatusBL.GetStaffStatusTypes();
        ListSource.FillDropDownList(olstStaffStatusDetails, cmbStaffStatusType, "StatusName", "StatusId", Constants.S_SELECT);

        List<StaffWorkingStatus> olstWorkingStatus = oStaffStatusBL.GetStaffWorkingStatus();
        ListSource.FillDropDownList(olstWorkingStatus, cmbStaffWorkingStatus, "WorkingStatus", "StatusId", Constants.S_SELECT);
    }
    public void FillBloodGroups()
    {
        moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
        DataTable oDtStandardCollection = UsersStaffGroupsAssociationBL.GetAllBloodGroups();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbBloodGroup, "Id", "BloodGroup", Constants.S_SELECT);
    }
    public void InitializeFields()
    {
       int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
       moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
       UserBasicDetails oUsersBasicDetails = moUsersStaffGroupsAssociationBL.GetUserBasicDetails(iStaffUserId, iSchoolId);
       txtPanNo.Text = oUsersBasicDetails.PanNo;
       txtAadharNo.Text = oUsersBasicDetails.AadharNo;
       txtEmpNo.Text = oUsersBasicDetails.EmployeeNo;
       txtJoiningDate.Text = string.Empty;
       txtPermanentDate.Text = string.Empty;
       txtResignationDate.Text = string.Empty;
       txtTransferDate.Text = string.Empty;
       lblGradepay.Text = oUsersBasicDetails.GradePay.ToString();
       trGrade.Visible = false;
       trGrade.Visible = ShowGradePayOnStaffProfileScreen;
       cmbBloodGroup.SelectedValue = oUsersBasicDetails.BloogGroupId.ToString();  /////
       cmbStaffStatusType.SelectedValue = oUsersBasicDetails.JobTypeId.ToString();

       if (iSchoolId == Constants.SchoolId.SNS.ToInt())
       {
           trIsOnClockHoursBasis.Visible = true;
           chkIsOnCHB.Checked = oUsersBasicDetails.IsOnCHB.ToBool();
       }
       else
           trIsOnClockHoursBasis.Visible = false;

       DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        if(!string.IsNullOrEmpty(oUsersBasicDetails.JoiningDate))
            txtJoiningDate.Text = Convert.ToDateTime(oUsersBasicDetails.JoiningDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        if (!string.IsNullOrEmpty(oUsersBasicDetails.PermanentDate))
            txtPermanentDate.Text = Convert.ToDateTime(oUsersBasicDetails.PermanentDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        if (!string.IsNullOrEmpty(oUsersBasicDetails.ResignationDate))
            txtResignationDate.Text = Convert.ToDateTime(oUsersBasicDetails.ResignationDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        if (!string.IsNullOrEmpty(oUsersBasicDetails.TransferDate))
            txtTransferDate.Text = Convert.ToDateTime(oUsersBasicDetails.TransferDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));

        hidFilePath.Value = oUsersBasicDetails.FilePath;
        if (!string.IsNullOrEmpty(hidFilePath.Value))
        {
            btnDownload.Visible = true;
            string sDestination = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN + hidFilePath.Value;
            if (File.Exists(sDestination))
                btnDownload.Attributes.Add("onclick", "window.open('..//downloads//PAN Attachment//" + hidFilePath.Value + "','_blank'); return false;");
            imgBtnDelete.Visible = true;
        }
        else
        {
            btnDownload.Visible = false;
            imgBtnDelete.Visible = false;
        }

        hidAadharFilePath.Value = oUsersBasicDetails.AadharFileUpload;
        if (!string.IsNullOrEmpty(hidAadharFilePath.Value))
        {
            imgDownloadAadhar.Visible = true;
            string sDestination = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_AADHAR + hidAadharFilePath.Value;
            if (File.Exists(sDestination))
                imgDownloadAadhar.Attributes.Add("onclick", "window.open('..//downloads//Aadhar Cards//" + hidAadharFilePath.Value + "','_blank'); return false;");
            imgBtnDeleteAadhar.Visible = true;
        }
        else
        {
            imgDownloadAadhar.Visible = false;
            imgBtnDeleteAadhar.Visible = false;
        }

        if (oUsersBasicDetails.ResignationDate == null)
        {
            cmbStaffWorkingStatus.SelectedValue = Constants.S_ONE;
            cmbStaffWorkingStatus.Enabled = false;
        }
        else
            cmbStaffWorkingStatus.SelectedValue = oUsersBasicDetails.WorkingStatusId.ToString();
    }

    /// <summary>
    /// This method is used to set user basic details.
    /// </summary>
    public void PopulateUserBasicDetails()
    {  
        UserBasicDetails olstUserBasicDetails=PopulateDetailsList();
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iLeaveSeperaterDay = SchoolBase.Settings.LeaveSeperaterDay ;
        moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
        moUsersStaffGroupsAssociationBL.SaveBasicDetails(olstUserBasicDetails, iAcademicYearId, iLeaveSeperaterDay);                    
    }

    /// <summary>
    /// This method is used to clear the fields.
    /// </summary>
    public void ClearFields()
    {
        txtJoiningDate.Text = string.Empty;
        txtPanNo.Text = string.Empty;
        txtPermanentDate.Text = string.Empty;
        txtResignationDate.Text = string.Empty;
        txtTransferDate.Text = string.Empty;
        hidFilePath.Value = string.Empty;
        imgBtnDelete.Visible = false;
        btnDownload.Visible = false;
        cmbStaffStatusType.ClearSelection();
        lblGradepay.Text = string.Empty;
        chkIsOnCHB.Checked = false;
    }

    /// <summary>
    /// This function is used to validate the profile details before saving it.
    /// </summary>
    public void ValidateProfile()
    {
        UserBasicDetails olstUserBasicDetails=PopulateDetailsList();
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
        moUsersStaffGroupsAssociationBL.ValidateProfileDetails(olstUserBasicDetails, iAcademicYearId);        
    }

    /// <summary>
    ///This is a common function for populating user basic details list. 
    /// </summary>
    public UserBasicDetails PopulateDetailsList()
    {
        bool bIsOnCHB;
        if (Session[Constants.S_SESSION_SCHOOL_ID].ToInt() == Constants.SchoolId.SNS.ToInt())
            bIsOnCHB = chkIsOnCHB.Checked;
        else
            bIsOnCHB = false;

        UserBasicDetails olstUserBasicDetails = new UserBasicDetails
            {
                UserId = iStaffUserId,
                PanNo = txtPanNo.Text.Trim(),
                EmployeeNo=txtEmpNo.Text.Trim(),
                JoiningDate = txtJoiningDate.Text,
                PermanentDate = txtPermanentDate.Text,
                ResignationDate = txtResignationDate.Text,
                TransferDate = txtTransferDate.Text,
                JobTypeId=Convert.ToInt32(cmbStaffStatusType.SelectedValue),
                WorkingStatusId = Convert.ToInt32(cmbStaffWorkingStatus.SelectedValue),
                FilePath = SaveFileOnServer(UploadPAN.FileName),
                AadharNo = txtAadharNo.Text.Trim(),
                AadharFileUpload = SaveAadharFileOnServer(UploadAadhar.FileName),
                SchoolId =
                    Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]),
                InsertedById =
                    Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]),
                IsOnCHB = bIsOnCHB
                , BloogGroupId = Convert.ToInt32(cmbBloodGroup.SelectedValue)
            };
        return olstUserBasicDetails;
    }

    /// <summary>
    /// this is for SaveFileOnServer
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName)
    {
        // Upload the file to the server.
        string sErrMessage = string.Empty;
        string sFolderName = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (UploadPAN.HasFile)
        {
            if (UploadPAN.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                sErrMessage = S_FILE_SIZE_EXCEED_ERROR;
            else if (File.Exists(sServerFilePath))
                sErrMessage = S_FILE_ALREADY_EXISTS;
            else
            {
                sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
                sServerFilePath = sFolderName + sFileName;
                UploadPAN.SaveAs(sServerFilePath);
                hidFilePath.Value = sFileName;
            }
        }
        else if (!hidFilePath.Value.IsNullOrEmpty())
            sFileName = hidFilePath.Value;
        
        if (sErrMessage.Equals("") &&  UploadPAN.HasFile)
        {
            //delete exesting file
            string sFileToDelete = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN + asFileName;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);            
        }

        if (sErrMessage != string.Empty)
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }

        return sFileName;
    }


    /// <summary>
    /// this is for SaveFileOnServer
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveAadharFileOnServer(string asFileName)
    {
        // Upload the file to the server.
        string sErrMessage = string.Empty;
        string sFolderName = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_AADHAR;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (UploadAadhar.HasFile)
        {
            if (UploadAadhar.PostedFile.ContentLength > I_FILE_SIZE_LIMIT_AADHAR)
                sErrMessage = S_FILE_SIZE_EXCEED_AADHAR_ERROR;
            else if (File.Exists(sServerFilePath))
                sErrMessage = S_FILE_ALREADY_EXISTS;
            else
            {
                sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
                sServerFilePath = sFolderName + sFileName;
                UploadAadhar.SaveAs(sServerFilePath);
                hidAadharFilePath.Value = sFileName;
            }
        }
        else if (!hidAadharFilePath.Value.IsNullOrEmpty())
            sFileName = hidAadharFilePath.Value;

        if (sErrMessage.Equals("") && UploadAadhar.HasFile)
        {
            //delete exesting file
            string sFileToDelete = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_AADHAR + asFileName;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
        }

        if (sErrMessage != string.Empty)
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }

        return sFileName;
    }
    #endregion
}