/* File Name    :   TeacherUI.aspx.cs
 * Modified By  :-  Sachin
 * Modified Date:-  25-Sept-2009
 * Purpose      :   This class is used to define teacher details.
 * Modified By  :Rohini
 * Date         :13 Jan 2011
 * Description  :Removed joining date field.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using SchoolBusinessService;
using System.Resources;
using Utility;
using System.Globalization;
using SchoolAutoSearchService.Client;

/// <summary>
/// Thhis class is used to add or edit teacher information.
/// </summary>
public partial class TeacherUI : SchoolBase
{
    #region Constants

    public const string S_DEFAULT_DATE_2 = "01/01/1900 12:00:00 AM";
    public const string S_DEFAULT_DATE_3 = "1/1/1900 12:00:00 AM";
    private const string S_QUALIFICATION_ID = "Qualification_Id";
    private const string S_SPECIALISATION = "Specialization";
    private const string S_QUALIFICATION = "Qualification_Name";
    private const string S_YEAR_OF_PASSING_ID = "Year_of_Passing";
    private const string S_PASSING_UNIVERSITY = "Passing_University";
    private const string S_CLASS_ID = "Class_Id";
    private const string S_CLASS_NAME = "Class_Name";
    private const string S_CHECK_BOX_DIV_SELECT = "ChkBoxDivSelect";
    private const string S_CHECK_BOX_STD_SELECT = "ChkBoxStdSelect";
    private const string S_GRIDVIEW_DATASOURCE = " grdvwEducationDetails_DataSource";
    private const string S_LISTVIEW_EXPDETAILS = "lstvwExpDetails_DataSourceID";
    private const int I_DATAKEY_QUALIFICATION_ID = 0;
    private const int I_SUBJECT_ID_COLUMN_INDEX = 0;
    private const string S_TEACHER_ID = "TeacherId";
    private const string S_USER_ID = "UserId";
    private const string S_QSTR_DESIGNATION_ID = "QualificationID";
    private const string S_QSTR_IS_CONFIG = "Is_Configured";
    private const string S_QSTR_STEP = "Step";    
    private const int I_TBL_TEACHER_INDEX = 0;
    private const int I_TBL_EDUCATION_INDEX = 1;
    private const int I_TBL_USER_INDEX = 4;
    private const int I_TBL_EXPERIENCE_INDEX = 5;
    private const string S_COMMAND_REMOVE = "REMOVE";
    private const string S_COMMAND_UPDATE = "Modify";
    private const string S_EDIT_MODE = "EDIT";
    private const string S_MODE_NEW = "NEW";
    private const string S_STANDARDS = "Standards";
    private const string S_SUBJECTS = "Subjects";
    private const string S_TEXT_UPDATE = "Update";    
    private const string S_SCHOOLNAME = "SchoolName";
    private const string S_JOINING_DATE = "JoiningDate";
    private const string S_LEFT_DATE = "leftDate";
    private const string S_DESIDNATION = "PreviousDesignation";
    private const string S_LAST_SALARY = "Last_Salary";
    private const string S_DURATION = "DurationDays";
    private const string S_JOB_DESCRIPTION = "Job_Description";
    private const string S_REASON_FOR_LEAVING = "Reason_For_Leaving";
    private const string S_ZEROS = "0000";

    #endregion

    #region Data Members

    private string msViewMode;
    private SchoolUserBL moSchoolUserBL;    
    private DataSet moDsTeacherInfo;
    private RetirementNoticeConfigBL moRetirementNoticeConfigBL;

    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #endregion

	#region -- PROPERTIES --

	/// <summary>
	/// Returns true if the Accounts module is enabled, false otherwise
	/// </summary>
	private bool IsAccountsModuleEnabled
	{
		get { return Settings.EnableAccountsModule; }
	}

	#endregion -- PROPERTIES --

    #region Events

    /// <summary>
    /// This method is used to fill all comboxes,set default values to the controls and decrypts the querystring.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            SetFocusBaseOnStep();
            StepSaveButtonVisble();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                
                
                GetRetirementNoticeConfig();
                FillAllComboBoxes();
                SetDefaultValues();
                ReadQuerystring();
                SetViewMode();
                SetJavascriptAttributes();
                SetPersonalDetailsAttributes();
                SetSubjectAssignmentStep();
                GetTeacherName();
                RefreshValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {   
                RefreshValues();
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString(); 
            }
            ucUserBasicDetails.Width = "320";
            
            if (!hidPassword.Value.IsNullOrEmpty())
            {
                txtPasswd.Attributes.Add("value", hidPassword.Value);
                txtConfirmPasswd.Attributes.Add("value", hidPassword.Value);
            }

           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStepSave_Click(object sender, EventArgs e)
    {
        try
        {
            ValidateEmployeeDetails();
            PopulateTeacherDetails();
            SaveTeacherDetails(false);

        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_TWO;
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErrorMsgForStd.Visible = true;
            lblErrorMsgForStd.Text = ex.Message;
            divRIMsg.Visible = true;
            msViewMode = Constants.ViewMode.Edit.ToString();
            FillOrginalStandardGrid();
            FillOriginalSubjectGrid();
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_TWO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save teacher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateTeacherDetails();
            SaveTeacherDetails(false);            
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErrorMsgForStd.Visible = true;
            lblErrorMsgForStd.Text = ex.Message;
            divRIMsg.Visible = true;
            msViewMode = Constants.ViewMode.Edit.ToString();
            FillOrginalStandardGrid();
            FillOriginalSubjectGrid();
        }
        catch (ApplicationException ex)
        {           
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_TWO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add teacher's education details into the gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>     
    protected void btnAddDetails_Click(object sender, EventArgs e)
    {
        try
        {
            hidQualificationId.Value = Convert.ToString(cmbQualification.SelectedValue);
            if (!CheckQualificationIsDuplicate())
            {
                lblDuplicateDetails.Visible = false;
                if (hidSelectedIndex.Value != Constants.S_EMPTY_STRING)
                {
                    //AddEducationDetailsToGrid();
                    UpdateEducationDetailsRow(Convert.ToInt32(hidSelectedIndex.Value));
                    
                }
                else
                    AddEducationDetailsToGrid();
                hidSelectedIndex.Value = null;
                ClearAllControls();
            }

            ClearTextBoxes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to set javascript attributes on each step.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TeacherInfo_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            if (wizard_TeacherInfo.ActiveStep == WizardStep1)
                SetPersonalDetailsAttributes();

            
            if (wizard_TeacherInfo.ActiveStep == WizardStep2)
            {
                StepSaveButtonVisble();
                SetAddressDetailsAttributes();
               BtnPreviousVallidationcause();
               

            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep4)
            {
                if (hidStep.Value != Constants.I_THREE.ToString())
                {
                    StepSaveButtonVisble();
                    ValidateEmployeeDetails();
                }

               SetStandardSubjectDetailsAttributes();
               BtnPreviousVallidationcause();
               

            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep3)
            {
                if (miSchoolId == Constants.SchoolId.SPS.ToInt())
                    trSPSTeacherType.Visible = true;
                else
                    trSPSTeacherType.Visible = false;
                StepSaveButtonVisble();
                SetEducationalDetailsAttributes();
                BtnPreviousVallidationcause();
                
            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep5)
            {
               
                SetUserDetailsAttributes();
                BtnPreviousVallidationcause();
            }
            if (wizard_TeacherInfo.ActiveStep != WizardStep5)
                divRIMsg.Visible = false;
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_TWO;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_TWO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to submit teacher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TeacherInfo_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            PopulateTeacherDetails();
            SaveTeacherDetails(true);
        }
        
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErrorMsgForStd.Visible = true;
            lblErrorMsgForStd.Text = ex.Message;
            divRIMsg.Visible = true;
            msViewMode = Constants.ViewMode.Edit.ToString();
            FillOrginalStandardGrid();
            FillOriginalSubjectGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method used to cancel operaton.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TeacherInfo_CancelButtonClick(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_TEACHER_INFO + "?" + HidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to reset all educational details.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            txtExpMonths.Text = "00";
            txtExpYears.Text = "00";
            txtAchivements.Text = string.Empty;
            ClearAllControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region GridView Events

    /// <summary>
    /// This method is used to bind data to the grid of subject details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (msViewMode == Constants.ViewMode.Edit.ToString())
            {
                if (e.Row.RowIndex >= Constants.I_ZERO)
                {
                    int iRowIndex = Convert.ToInt32(e.Row.RowIndex);
                    if (grdSubjects.DataKeys[iRowIndex]["Teacher_Id"].ToString() != Constants.S_ZERO)
                        ((CheckBox)e.Row.FindControl(S_CHECK_BOX_DIV_SELECT)).Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to edit/delete educational details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwEducationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName.ToUpper())
            {
                case "DELETE_ROW":
                    hidSelectedIndex.Value = null;
                    DeleteEducationDetails(iRowIndex);
                    break;

                case "EDIT_ROW":
                    iRowIndex = Convert.ToInt32(e.CommandArgument);
                    hidSelectedIndex.Value = Convert.ToString(iRowIndex);
                    FillEducationDetailsToEdit(iRowIndex);
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set delete button attribute.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwEducationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ImageButton oImgDelete = (ImageButton)e.Row.FindControl("btnDeleteEducationalDetails");
                oImgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind data to standard gridview rows.
    /// standards.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (msViewMode == Constants.ViewMode.Edit.ToString())
            {
                if (e.Row.RowIndex >= Constants.I_ZERO)
                {
                    int iRowIndex = Convert.ToInt32(e.Row.RowIndex);
                    if (grdStandards.DataKeys[iRowIndex]["Teacher_Id"].ToString() != Constants.S_ZERO)
                        ((CheckBox)e.Row.FindControl(S_CHECK_BOX_STD_SELECT)).Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region List Events

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
                ImageButton oImgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oImgbtnDelete.Attributes.Add("onclick", "if(!DeleteExpDetails()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort grid columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExpDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                int iExpDetailsId = Constants.I_ZERO;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                string sSchoolName = lstvwExpDetails.DataKeys[iListIndex][S_SCHOOLNAME].ToString();
                DateTime odtJoinDate = Convert.ToDateTime(lstvwExpDetails.DataKeys[iListIndex][S_JOINING_DATE]);
                DateTime odtLeftDate = Convert.ToDateTime(lstvwExpDetails.DataKeys[iListIndex][S_LEFT_DATE]);
                hidJoinDate.Value = odtJoinDate.ToString(Constants.S_DATE_FORMAT_MARATHI);
                hidLeftDate.Value = odtLeftDate.ToString(Constants.S_DATE_FORMAT_MARATHI);
                hidExperienceDetailsId.Value = iExpDetailsId.ToString();
                hidSlectedExpIndex.Value = Convert.ToString(iListIndex);
                hidSchoolName.Value = sSchoolName;
                if (e.CommandName == S_COMMAND_REMOVE)
                    DeleteExpDetails(iListIndex);
                else if (e.CommandName == S_COMMAND_UPDATE)
                {
                    btnCancelDetails.Enabled = true;
                    FillControlsForExpDetails(iListIndex);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to add exprience details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!CheckSchoolNameIsDuplicate())
            {
                lblChkDuplicate.Visible = false;

                if (btnAdd.Text == Resources.LocalizedResources.Update)
                    UpdateExpDetailsinGrid(Convert.ToInt32(hidSlectedExpIndex.Value));
                else
                    SaveExperienceDetails();
                ClearTextBoxes();
            }

            btnAdd.Text = Resources.LocalizedResources.AddDetails;
            hidbtnAddText.Value = "AddDetails";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear he textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelDetails_Click(object sender, EventArgs e)
    {
        try
        {
            ClearTextBoxes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private Methods

    /// <summary>
    /// This method is used to validate Employee details.
    /// </summary>
    private void ValidateEmployeeDetails()
    {
      if (!hidUserId.Value.IsNullOrEmpty())
         ucUserBasicDetails.StaffUserId = hidUserId.Value.ToInt();
      ucUserBasicDetails.ValidateProfile();
    }

    /// <summary>
    /// This method is used to populate teacher's details.
    /// </summary>
    private void PopulateTeacherDetails()
    {
        moSchoolUserBL = new SchoolUserBL();
        if (hidUserId.Value != Constants.S_EMPTY_STRING)
        {
            moSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
            moSchoolUserBL.TeacherDetails.TeacherId = Convert.ToInt32(hidTeacherId.Value);
        }

        PopulateTeacherPersonalDetails();
        PopulateTeacherAddressDetails();
        PopulateEducationGridDetails();
        PopulateExperienceListDetails();
        PopulateSubjectDetails();
        PopulateStandardDetails();
        PopulateUserInformation();
        populateUserShiftDetails();
        populateUserWeekendDetails();
    }

    /// <summary>
    ///  This method is used to populate all personal details of teacher.
    /// </summary>
    private void PopulateTeacherPersonalDetails()
    {
        moSchoolUserBL.TeacherDetails.SchoolId = miSchoolId;
        moSchoolUserBL.TeacherDetails.Academic_Year_Id = miAcademicYearId;

        moSchoolUserBL.TeacherDetails.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        moSchoolUserBL.TeacherDetails.TeacherFirstName = txtFirstName.Text.ToTitleCase();
        moSchoolUserBL.TeacherDetails.TeacherMiddleName = txtMiddleName.Text.ToTitleCase();
        moSchoolUserBL.TeacherDetails.TeacherLastName = txtLastName.Text.ToTitleCase();

        if (txtPhoneNumber.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.PhoneNumber = txtPhoneNumber.Text;
        if (txtMobileNumber.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.MobileNumber = txtMobileNumber.Text;

		// We need to give this field a default value else, it will cause problems when converting to xml.
		// This is becuase the underlying field is a char, which has a default value of '\0', which is invalid when converting to xml.
		moSchoolUserBL.TeacherDetails.IsTemporary = Constants.C_NO;

        moSchoolUserBL.TeacherDetails.DateofBirth =Convert.ToDateTime(calendar_DOB.DateValue.ToString(Constants.S_DATE_FORMAT_MARATHI));

        moSchoolUserBL.TeacherDetails.DateofRetirement = calendar_DOB.DateValue.AddYears(hidRetAge.Value.ToInt());
        moSchoolUserBL.TeacherDetails.ReligionId = Convert.ToInt32(cmbReligion.SelectedValue);
        moSchoolUserBL.TeacherDetails.DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue);
        moSchoolUserBL.TeacherDetails.CategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            moSchoolUserBL.TeacherDetails.AssociatedStandardCategory = Convert.ToInt32(cmbTeachingForClass.SelectedValue);
        else
            moSchoolUserBL.TeacherDetails.AssociatedStandardCategory = Constants.I_ZERO;

        moSchoolUserBL.EmergencyContact = txtEmergencyNo.Text;
        if (txtCasteSubCaste.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.CasteSubCaste = txtCasteSubCaste.Text.Trim();
        moSchoolUserBL.TeacherDetails.Nationality = txtNationality.Text;
        if (txtExpYears.Text == Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.ExpInYears = Constants.I_ZERO;
        else
            moSchoolUserBL.TeacherDetails.ExpInYears = Convert.ToInt32(txtExpYears.Text);
        if (txtExpMonths.Text == Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.ExpInMonths = Constants.I_ZERO;
        else
            moSchoolUserBL.TeacherDetails.ExpInMonths = Convert.ToInt32(txtExpMonths.Text);
        if (txtAchivements.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.Achivements = txtAchivements.Text;

        if (miSchoolId == Constants.SchoolId.SPS.ToInt())
            moSchoolUserBL.TeacherDetails.TeacherTypeId = cmbType.SelectedValue.ToInt();
        else
            moSchoolUserBL.TeacherDetails.TeacherTypeId = Constants.I_ZERO;

        moSchoolUserBL.TeacherDetails.InsertedByid = miUserId;
        moSchoolUserBL.TeacherDetails.UpdatedById = miUserId;
    }

    /// <summary>
    /// This method is used to populate local and permanent address details.
    /// </summary>
    private void PopulateTeacherAddressDetails()
    {
        moSchoolUserBL.TeacherDetails.LocalAddress = txtLocalAddress.Text.Trim();
        moSchoolUserBL.TeacherDetails.LocalCity = txtLocalCity.Text.Trim();
        moSchoolUserBL.TeacherDetails.LocalPincode = Convert.ToInt32(txtLocalPincode.Text);
        moSchoolUserBL.TeacherDetails.LocalState = txtState.Text;

        if (chkAddress.Checked == true)
            PopulatePermanentAddressIfIsLocal();
        else
            PopulatePermanentAddressIfIsNotLocal();
    }

    /// <summary>
    /// This method is used to populate Permanent address.
    /// </summary>
    private void PopulatePermanentAddressIfIsLocal()
    {
        moSchoolUserBL.TeacherDetails.IsLocalAddress = Constants.C_YES;
        if (txtLocalAddress.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.PermanentAddress = txtLocalAddress.Text.Trim();
        if (txtLocalCity.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.PermanentCity = txtLocalCity.Text.Trim();
        if (txtLocalPincode.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.PermanentPincode = Convert.ToInt32(txtLocalPincode.Text);
        moSchoolUserBL.TeacherDetails.PermanentState = txtState.Text;
    }

    /// <summary>
    /// This method is used to populate Local address.
    /// </summary>
    private void PopulatePermanentAddressIfIsNotLocal()
    {
        moSchoolUserBL.TeacherDetails.IsLocalAddress = Constants.C_NO;
        moSchoolUserBL.TeacherDetails.PermanentAddress = txtPerAddress.Text.Trim();
        moSchoolUserBL.TeacherDetails.PermanentCity = txtPerCity.Text.Trim();
        if (txtPerPinCode.Text != Constants.S_EMPTY_STRING)
            moSchoolUserBL.TeacherDetails.PermanentPincode = Convert.ToInt32(txtPerPinCode.Text);
        moSchoolUserBL.TeacherDetails.PermanentState = txtPerState.Text;
    }

    /// <summary>
    /// This method is used to retrieve retirement notice config. of teacher.s
    /// </summary>
    private void GetRetirementNoticeConfig()
    {
        moRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
        List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = moRetirementNoticeConfigBL.GetAll();
        int iRetAge = lstRetirementNoticeConfig.Where(obj => obj.UserRole.Id == Constants.UserRoles.Teacher.ToInt()).Select(obj => obj.RetirementAge).FirstOrDefault();
        hidRetirementAge.Value = System.DateTime.Now.AddYears(-1 * iRetAge).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        
        hidRetAge.Value = iRetAge.ToString();
    }

    /// <summary>
    ///  This method is used to populate education details.
    /// </summary>
    private void PopulateEducationGridDetails()
    {
        DataTable oDtEducationGridDetails;
        oDtEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];

        foreach (DataRow oEducationDataRow in oDtEducationGridDetails.Rows)
        {
            TeacherEducationDetailsBL oTeacherEducationDetailsBL = new TeacherEducationDetailsBL();
            if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
                oTeacherEducationDetailsBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);
            oTeacherEducationDetailsBL.QualificationId = Convert.ToInt32(oEducationDataRow[S_QUALIFICATION_ID]);
            oTeacherEducationDetailsBL.Specialization = Convert.ToString(oEducationDataRow[S_SPECIALISATION]);
            oTeacherEducationDetailsBL.YearOfPassingId = Convert.ToInt32(oEducationDataRow[S_YEAR_OF_PASSING_ID]);
            oTeacherEducationDetailsBL.ClassId = Convert.ToInt32(oEducationDataRow[S_CLASS_ID]);
            oTeacherEducationDetailsBL.PassingUniversity = Convert.ToString(oEducationDataRow[S_PASSING_UNIVERSITY]);
            oTeacherEducationDetailsBL.InsertedById = miUserId;
            oTeacherEducationDetailsBL.UpdatedById = miUserId;
            moSchoolUserBL.TeacherDetails.moTeacherEduDetails.Add(oTeacherEducationDetailsBL);
        }
    }

    /// <summary>
    /// This method is used to insert exprience details.
    /// </summary>
    private void PopulateExperienceDetails()
    {
        DataTable oDTEducationGridDetails;
        oDTEducationGridDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];
        ArrayList oArrList = new ArrayList();
        foreach (DataRow oEducationDataRow in oDTEducationGridDetails.Rows)
        {
            SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
            if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
                oSchoolWiseTeacherMasterBL.UserId = Convert.ToInt32(hidTeacherId.Value);
            oSchoolWiseTeacherMasterBL.SchoolId = miSchoolId;
            oSchoolWiseTeacherMasterBL.ExpDetailsId = Constants.I_ZERO;
            oSchoolWiseTeacherMasterBL.SchoolName = Convert.ToString(oEducationDataRow[S_SCHOOLNAME]);
            oSchoolWiseTeacherMasterBL.JoinDate = Convert.ToDateTime(string.Format(Constants.S_DATE_FORMAT_MARATHI,oEducationDataRow[S_JOINING_DATE]),new CultureInfo("en"));
            oSchoolWiseTeacherMasterBL.LeftDate = Convert.ToDateTime(string.Format(Constants.S_DATE_FORMAT_MARATHI, oEducationDataRow[S_LEFT_DATE]), new CultureInfo("en"));
            oSchoolWiseTeacherMasterBL.InsertedByid = miUserId;
            oSchoolWiseTeacherMasterBL.InsertDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oSchoolWiseTeacherMasterBL.UpdateDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            oSchoolWiseTeacherMasterBL.UpdatedById = miUserId;
            oSchoolWiseTeacherMasterBL.PreviousDesignation = Convert.ToString(txtDesignation.Text);
            oSchoolWiseTeacherMasterBL.Last_Salary = Convert.ToDecimal(txtLastSalary.Text);
            oSchoolWiseTeacherMasterBL.DurationDays = Convert.ToString(txtDuration.Text);
            oSchoolWiseTeacherMasterBL.Job_Description = Convert.ToString(txtJobDescription.Text);
            oSchoolWiseTeacherMasterBL.Reason_for_Leaving = Convert.ToString(txtReasonForLeaving.Text);
            if (hidMode.Value == S_EDIT_MODE)
                oSchoolWiseTeacherMasterBL.ExpDetailsId = Convert.ToInt32(hidExperienceDetailsId.Value);

           

            oSchoolWiseTeacherMasterBL.InsertExperienceDetails();
        }
    }

    /// <summary>
    /// This method is used to populate exprience details.
    /// </summary>
    private void PopulateExperienceListDetails()
    {
        DataTable oDtEducationGridDetails;
        oDtEducationGridDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];
        if (oDtEducationGridDetails != null)
        {
            foreach (DataRow oEducationDataRow in oDtEducationGridDetails.Rows)
            {
                TeacherExperienceDetailsBL oTeacherExperienceDetailsBL = new TeacherExperienceDetailsBL();
                if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
                    oTeacherExperienceDetailsBL.User_Id = Convert.ToInt32(hidTeacherId.Value);
                oTeacherExperienceDetailsBL.School_Id = miSchoolId;
                oTeacherExperienceDetailsBL.ExperienceDetailsId = Constants.I_ZERO;
                oTeacherExperienceDetailsBL.SchoolName = Convert.ToString(oEducationDataRow[S_SCHOOLNAME]);
                oTeacherExperienceDetailsBL.JoiningDate = Convert.ToDateTime(oEducationDataRow[S_JOINING_DATE].ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
                oTeacherExperienceDetailsBL.leftDate = Convert.ToDateTime(oEducationDataRow[S_LEFT_DATE].ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
                oTeacherExperienceDetailsBL.Inserted_By_id = miUserId;
                oTeacherExperienceDetailsBL.InsertDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
                oTeacherExperienceDetailsBL.Update_Date = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
                oTeacherExperienceDetailsBL.Updated_By_Id = miUserId;


                if (oEducationDataRow[S_DESIDNATION] != DBNull.Value)
                    oTeacherExperienceDetailsBL.PreviousDesignation = Convert.ToString(oEducationDataRow[S_DESIDNATION]);
                else
                    oTeacherExperienceDetailsBL.PreviousDesignation = string.Empty;

                if (oEducationDataRow[S_LAST_SALARY] != DBNull.Value)
                    oTeacherExperienceDetailsBL.Last_Salary = Convert.ToDecimal(oEducationDataRow[S_LAST_SALARY]);
                else
                    oTeacherExperienceDetailsBL.Last_Salary = 0;

                if (oEducationDataRow[S_DURATION] != DBNull.Value)
                    oTeacherExperienceDetailsBL.DurationDays = Convert.ToString(oEducationDataRow[S_DURATION]);
                else
                    oTeacherExperienceDetailsBL.DurationDays = string.Empty;

                if (oEducationDataRow[S_JOB_DESCRIPTION] != DBNull.Value)
                    oTeacherExperienceDetailsBL.Job_Description = Convert.ToString(oEducationDataRow[S_JOB_DESCRIPTION]);
                else
                    oTeacherExperienceDetailsBL.Job_Description = string.Empty;

                if (oEducationDataRow[S_REASON_FOR_LEAVING] != DBNull.Value)
                    oTeacherExperienceDetailsBL.Reason_for_Leaving = Convert.ToString(oEducationDataRow[S_REASON_FOR_LEAVING]);
                else
                    oTeacherExperienceDetailsBL.Reason_for_Leaving = string.Empty;
                
                if (hidMode.Value == S_EDIT_MODE)
                    oTeacherExperienceDetailsBL.ExperienceDetailsId = Convert.ToInt32(hidExperienceDetailsId.Value);
                moSchoolUserBL.TeacherDetails.moTeacherExperienceDetails.Add(oTeacherExperienceDetailsBL);
            }
        }
    }

    /// <summary>
    /// This method is used to populate Shift details.
    /// </summary>
    private void populateUserShiftDetails()
    {
        UserShiftAssociationBL oUserShiftAssociationBL = new UserShiftAssociationBL();
        oUserShiftAssociationBL.Shiftid = oUserShiftAssociationBL.GetDefaultShift(miSchoolId, miAcademicYearId);
        oUserShiftAssociationBL.SchoolId = miSchoolId;
        oUserShiftAssociationBL.AcademicYearId = miAcademicYearId;
        oUserShiftAssociationBL.IsDeleted = Constants.C_NO;
        oUserShiftAssociationBL.InsertedById = miUserId;
        oUserShiftAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
        moSchoolUserBL.TeacherDetails.moUserShiftAssociation.Add(oUserShiftAssociationBL);
    }

    /// <summary>
    /// This method is used to populate Weekend details.
    /// </summary>
    private void populateUserWeekendDetails()
    {
        UserWeekEndAssociationBL oUserWeekendAssociationBL  = new UserWeekEndAssociationBL();
        List<int> weekendIdList = oUserWeekendAssociationBL.GetWeekendsApplicableforStaff(miSchoolId, miAcademicYearId);
        foreach (int iWeekendId in weekendIdList)
        {
            oUserWeekendAssociationBL.WeekEndId = iWeekendId;
            oUserWeekendAssociationBL.SchoolId = miSchoolId;
            oUserWeekendAssociationBL.AcademicYearId = miAcademicYearId;
            oUserWeekendAssociationBL.IsDeleted = Constants.C_NO;
            oUserWeekendAssociationBL.InsertedById = miUserId;
            oUserWeekendAssociationBL.InsertedDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
            moSchoolUserBL.TeacherDetails.moUserWeekendAssociation.Add(oUserWeekendAssociationBL);
        }
    }

    /// <summary>
    /// This method is used to populate subject details.
    /// </summary>
    private void PopulateSubjectDetails()
    {
        const int I_SUBJECT_NAME_COLUMN_INDEX = Constants.I_ONE;
        const int I_TEACHER_SUBJECT_ID = Constants.I_THREE;
        CheckBox chkDeleteflag;
        for (int iRowCounter = 0; iRowCounter < grdSubjects.Rows.Count; iRowCounter++)
        {
            chkDeleteflag = (CheckBox)grdSubjects.Rows[iRowCounter].FindControl(S_CHECK_BOX_DIV_SELECT);
            if (chkDeleteflag.Checked && hidTeacherId.Value == Constants.S_EMPTY_STRING)
            {
                TeacherSubjectDetailsBL oTeacherSubjectDetailsBL = new TeacherSubjectDetailsBL
                                                                       {
                                                                           SubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_SUBJECT_ID_COLUMN_INDEX]),
                                                                           InsertedById = miUserId,
                                                                           UpdatedById = miUserId
                                                                       };
                if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
                    oTeacherSubjectDetailsBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);
                if (!Convert.ToString(grdSubjects.DataKeys[iRowCounter][I_TEACHER_SUBJECT_ID]).Equals(S_ZEROS))
                    oTeacherSubjectDetailsBL.TeacherSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_TEACHER_SUBJECT_ID]);
                oTeacherSubjectDetailsBL.ConfigurationAction = Constants.Action.Insert;
                moSchoolUserBL.TeacherDetails.moTeacherSubDetails.Add(oTeacherSubjectDetailsBL);
            }

            if (!chkDeleteflag.Checked && !Convert.ToString(grdSubjects.DataKeys[iRowCounter][2]).Equals(S_ZEROS))
            {
                TeacherSubjectDetailsBL oTeacherSubjectDetailsBL = new TeacherSubjectDetailsBL();
                oTeacherSubjectDetailsBL.SubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_SUBJECT_ID_COLUMN_INDEX]);
                oTeacherSubjectDetailsBL.SubjectName = grdSubjects.Rows[iRowCounter].Cells[I_SUBJECT_NAME_COLUMN_INDEX].Text;
                oTeacherSubjectDetailsBL.InsertedById = miUserId;
                oTeacherSubjectDetailsBL.UpdatedById = miUserId;
                oTeacherSubjectDetailsBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);
                oTeacherSubjectDetailsBL.TeacherSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_TEACHER_SUBJECT_ID]);
                oTeacherSubjectDetailsBL.ConfigurationAction = Constants.Action.Delete;
                moSchoolUserBL.TeacherDetails.moTeacherSubDetails.Add(oTeacherSubjectDetailsBL);
            }

            if (chkDeleteflag.Checked && !Convert.ToString(grdSubjects.DataKeys[iRowCounter][2]).Equals(S_ZEROS))
            {
                TeacherSubjectDetailsBL oTeacherSubjectDetailsBL = new TeacherSubjectDetailsBL();
                oTeacherSubjectDetailsBL.SubjectId =
                    Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_SUBJECT_ID_COLUMN_INDEX].ToString());
                oTeacherSubjectDetailsBL.SubjectName = grdSubjects.Rows[iRowCounter].Cells[I_SUBJECT_NAME_COLUMN_INDEX].Text;
                oTeacherSubjectDetailsBL.InsertedById = miUserId;
                oTeacherSubjectDetailsBL.UpdatedById = miUserId;
                oTeacherSubjectDetailsBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);
                oTeacherSubjectDetailsBL.TeacherSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowCounter][I_TEACHER_SUBJECT_ID]);
                oTeacherSubjectDetailsBL.ConfigurationAction = Constants.Action.Update;
                moSchoolUserBL.TeacherDetails.moTeacherSubDetails.Add(oTeacherSubjectDetailsBL);
            }
        }
    }

    /// <summary>
    /// This method is used to populate standard details.
    /// </summary>
    private void PopulateStandardDetails()
    {
        const int I_STANDARD_ID_COLUMN_INDEX = Constants.I_ZERO;
        const int I_STANDARD_NAME_COLUMN_INDEX = Constants.I_ONE;
        const int I_DATAKEY_TEACHER_STANDARD_ID = Constants.I_THREE;
        CheckBox oChkDeleteflag;
        for (int iCount = 0; iCount < grdStandards.Rows.Count; iCount++)
        {
            oChkDeleteflag = (CheckBox)grdStandards.Rows[iCount].FindControl(S_CHECK_BOX_STD_SELECT);
            if (oChkDeleteflag.Checked && hidTeacherId.Value == Constants.S_EMPTY_STRING)
            {
                TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL
                                                                         {
                                                                             StandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_STANDARD_ID_COLUMN_INDEX].ToString()),
                                                                             InsertedById = miUserId,
                                                                             UpdatedById = miUserId
                                                                         };

                if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
                    oTeacherStandardDetailsBL.TeacherId = Convert.ToInt32(hidTeacherId.Value);

                if (!Convert.ToString(grdStandards.DataKeys[iCount][I_DATAKEY_TEACHER_STANDARD_ID]).Equals(S_ZEROS))
                    oTeacherStandardDetailsBL.TeacherStandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_DATAKEY_TEACHER_STANDARD_ID]);

                oTeacherStandardDetailsBL.ConfigurationAction = Constants.Action.Insert;
                moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.Add(oTeacherStandardDetailsBL);
            }

            if (!oChkDeleteflag.Checked && !Convert.ToString(grdStandards.DataKeys[iCount][2]).Equals(S_ZEROS))
            {
                TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL
                                                                         {
                                                                             StandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_STANDARD_ID_COLUMN_INDEX].ToString()),
                                                                             StandardName = grdStandards.Rows[iCount].Cells[I_STANDARD_NAME_COLUMN_INDEX].Text,
                                                                             InsertedById = miUserId,
                                                                             UpdatedById = miUserId,
                                                                             TeacherId = Convert.ToInt32(hidTeacherId.Value),
                                                                             TeacherStandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_DATAKEY_TEACHER_STANDARD_ID]),
                                                                             ConfigurationAction = Constants.Action.Delete
                                                                         };
                moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.Add(oTeacherStandardDetailsBL);
            }

            if (oChkDeleteflag.Checked && !Convert.ToString(grdStandards.DataKeys[iCount][2]).Equals(S_ZEROS))
            {
                TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL
                                                                         {
                                                                             StandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_STANDARD_ID_COLUMN_INDEX].ToString()),
                                                                             StandardName = grdStandards.Rows[iCount].Cells[I_STANDARD_NAME_COLUMN_INDEX].Text,
                                                                             InsertedById = miUserId,
                                                                             UpdatedById = miUserId,
                                                                             TeacherId = Convert.ToInt32(hidTeacherId.Value),
                                                                             TeacherStandardId = Convert.ToInt32(grdStandards.DataKeys[iCount][I_DATAKEY_TEACHER_STANDARD_ID]),
                                                                             ConfigurationAction = Constants.Action.Update
                                                                         };
                moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.Add(oTeacherStandardDetailsBL);
            }
        }
    }

    /// <summary>
    /// This method is used to populate user information.
    /// </summary>
    private void PopulateUserInformation()
    {
        string sPasswrd;
        const string S_DEFAULT_USER_ROLE = "2";
        if (hidUserId.Value != Constants.S_EMPTY_STRING)
            moSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
        moSchoolUserBL.SchoolId = miSchoolId;
        moSchoolUserBL.Email = txtEmail.Text.Trim();
        moSchoolUserBL.Login = txtUserName.Text.Trim();

        moSchoolUserBL.CanApproveRequisition = chkCanApproveRequisitions.Checked ? Constants.C_YES : Constants.C_NO;
        moSchoolUserBL.CanCreateGeneralRequisition = chkCanCreateGeneralRequisition.Checked ? Constants.C_YES : Constants.C_NO;
        moSchoolUserBL.CanSanctionLeave = chkCanSanctionLeave.Checked ? Constants.C_YES : Constants.C_NO;
        moSchoolUserBL.CanReceiveMail = Constants.C_NO;
		moSchoolUserBL.CanApproveVoucher = chkCanApproveVoucher.Checked;
		moSchoolUserBL.CanCreateVoucher = chkCanCreateVoucher.Checked;
		moSchoolUserBL.CanDeleteVoucher = chkCanDeleteVoucher.Checked;
		moSchoolUserBL.CanEditOldFinancialYear = chkCanEditOldFinancialYear.Checked;
        moSchoolUserBL.InternalUser = chkInternalUser.Checked;
        moSchoolUserBL.ShowAllSentSMS = chkShowAllSentSMS.Checked;
        moSchoolUserBL.CanPublishUnpublishExam = chkPublishorUnpublishExam.Checked;
        if (chkCanCreateVoucher.Checked)
			moSchoolUserBL.CanSelfApprove = chkCanSelfApprove.Checked;
		else
			chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");
        string sPasswordOfTeacher;
        if (txtPasswd.Enabled == false)
            sPasswordOfTeacher = hidPasswordOfTeacher.Value;
        else
            sPasswordOfTeacher = txtPasswd.Text;
        moSchoolUserBL.Password = sPasswrd = wizard_TeacherInfo.ActiveStep != WizardStep5 ? hidPassword.Value : sPasswordOfTeacher;
      
        
        moSchoolUserBL.UserRoleId = Convert.ToInt32(S_DEFAULT_USER_ROLE);
        moSchoolUserBL.InsertedBy =Convert.ToString(miUserId);
        moSchoolUserBL.UpdatedBy = Convert.ToString(miUserId);
    }    

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {       
       btnAdd.Text = oResourceManager.GetString(hidbtnAddText.Value.Replace(" ", string.Empty));
       btnAddDetails.Text=oResourceManager.GetString(hidbtnAddDetailsText.Value.Replace(" ",string.Empty));
       valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
       valAddEduDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
       valsumExpDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
       valsumUserDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
       hidEmailNotBlankMessage.Value = Resources.LocalizedResources.EmailShouldNotBlank;
       hidEmailShouldBeValidFormat.Value=Resources.LocalizedResources.EmailShouldBeValidFormat;
       hidUserNameShouldNotBlank.Value=Resources.LocalizedResources.UserNameShouldNotBlank;
       hidUserNameShouldBeOfMinSixChar.Value=Resources.LocalizedResources.UserNameShouldBeOfMinSixChar;
       hidLocalPincodeShouldNotBlank.Value=Resources.LocalizedResources.LocalPincodeShouldNotBlank;
       hidLocalPincodeValidation.Value=Resources.LocalizedResources.LocalPincodeValidation;
       hidYearOfPassingShouldNotBlank.Value=Resources.LocalizedResources.YearOfPassingShouldNotBlank;
       hidYearOfPassingValidation.Value=Resources.LocalizedResources.YearOfPassingValidation;
       hidYearOfPassingInvalid.Value=Resources.LocalizedResources.YearOfPassingInvalid;
       hidYearOfPassingValidation1.Value=Resources.LocalizedResources.YearOfPassingValidation1;
       hidDateOfBirthFutureDate.Value=Resources.LocalizedResources.DateOfBirthFutureDate;
       hidAgeShouldBeLessThan.Value=Resources.LocalizedResources.AgeShouldBeLessThan;
       hidYears.Value=Resources.LocalizedResources.years;       
       hidAgeValidationCondition.Value=Resources.LocalizedResources.AgeValidationCondition;
       hidSubjectValidation.Value=Resources.LocalizedResources.SubjectValidation;
       hidStandardCondition.Value=Resources.LocalizedResources.StandardCondition;
       hidMobileNumberValidation1.Value=Resources.LocalizedResources.MobileNumberValidation1;
       hidMobileDigit.Value=Resources.LocalizedResources.MobileDigit;
       hidNewConfirmSamePwdErrorMsg.Value=Resources.LocalizedResources.NewConfirmSamePwdErrorMsg;
       hidPasswordCondition1.Value=Resources.LocalizedResources.PasswordCondition1;
       hidPasswordConditionErrorMsg.Value=Resources.LocalizedResources.PasswordConditionErrorMsg;
       hidPasswordShouldNotBlank.Value=Resources.LocalizedResources.PasswordShouldNotBlank;
       hidAreYouSureDeleteEducationalDetails.Value=Resources.LocalizedResources.AreYouSureDeleteEducationalDetails;
       hidAreYouSureDeleteExperienceDetails.Value=Resources.LocalizedResources.AreYouSureDeleteExperienceDetails;
       hidLeftDateJoinedDateValidation.Value = Resources.LocalizedResources.LeftDateJoinedDateValidation;
       Button oBtnCancel = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
       oBtnCancel.Text = Resources.LocalizedResources.Close;

    }

    #endregion

    #region Create Datatable and Datarow

    /// <summary>
    /// This method is used to create the datarow and bind that row to the gridview
    /// </summary>
    private void AddEducationDetailsToGrid()
    {
        DataTable oDtEducationDetails;
        if (ViewState[S_GRIDVIEW_DATASOURCE] == null)
            oDtEducationDetails = CreateEducationDetailsTable();
        else
            oDtEducationDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];

        // Once a table has been created,create DataRow.    
        oDtEducationDetails.Rows.Add(AddEducationDetailsToDataRow(oDtEducationDetails.NewRow()));
        DataView oDtItemView = oDtEducationDetails.DefaultView;
        grdvwEducationDetails.DataSource = oDtItemView;
        ViewState[S_GRIDVIEW_DATASOURCE] = oDtEducationDetails;
        grdvwEducationDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow AddEducationDetailsToDataRow(DataRow aoDr)
    {
        DataRow oDrItem;
        int iQualificatonId = Convert.ToInt32(hidQualificationId.Value);
        oDrItem = aoDr;
        //// Then add the new row to the collection.
        oDrItem[S_QUALIFICATION_ID] = iQualificatonId;
        oDrItem[S_QUALIFICATION] = Convert.ToString(cmbQualification.SelectedItem);
        oDrItem[S_SPECIALISATION] = Convert.ToString(txtSpecialization.Text.Trim());
        oDrItem[S_YEAR_OF_PASSING_ID] = Convert.ToInt32(txtYearOfPassing.Text);
        oDrItem[S_PASSING_UNIVERSITY] = txtPassingUniversity.Text.Trim();
        oDrItem[S_CLASS_ID] = Convert.ToInt32(cmbPassingClass.SelectedValue);
        oDrItem[S_CLASS_NAME] = Convert.ToString(cmbPassingClass.SelectedItem);
        return oDrItem;
    }

    /// <summary>
    /// This method is used to create new datatable
    /// </summary>
    /// <returns></returns>
    private DataTable CreateEducationDetailsTable()
    {
        const string S_INT_DATA_TYPE = "System.Int32";
        const string S_STRING_DATA_TYPE = "System.String";

        //// Create a new DataTable for educationa details. 
        DataTable oDtEducationDetails = new DataTable();

        //// Add columns to the Item table.
        AddDataColumnToItemTable(S_INT_DATA_TYPE, "ID", ref oDtEducationDetails, true, true);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_QUALIFICATION_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_QUALIFICATION, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_SPECIALISATION, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_YEAR_OF_PASSING_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_PASSING_UNIVERSITY, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_INT_DATA_TYPE, S_CLASS_ID, ref oDtEducationDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_CLASS_NAME, ref oDtEducationDetails, false);
        return oDtEducationDetails;
    }

    /// <summary>
    /// This method is used to add data columns in datatable.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemTable(string asDataType, string asColumnName, ref DataTable aoDataTable, bool abIsPrimaryKey, bool abIsAutoINcrementedColumn=false)
    {
        DataColumn oDataColumn = new DataColumn { DataType = Type.GetType(asDataType), ColumnName = asColumnName };

        if (abIsAutoINcrementedColumn)
            oDataColumn.AutoIncrement = true;

        aoDataTable.Columns.Add(oDataColumn);

        if (abIsPrimaryKey)
        {
            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = oDataColumn;
            aoDataTable.PrimaryKey = keys;
        }
    }

    #endregion

    #region Helping Methods

    /// <summary>
    /// This method is used to clear contents of all textboxes.
    /// </summary>
    private void ClearTextBoxes()
    {
        txtSchoolname.Text = string.Empty;
        txtjoinedDate.Text = string.Empty;
        txtLeftDate.Text = string.Empty;
        hidMode.Value = S_MODE_NEW;
        chkSendSMS.Checked = false;
        txtDesignation.Text = string.Empty; //
        txtDuration.Text = string.Empty; //
        txtJobDescription.Text = string.Empty;//
        txtReasonForLeaving.Text = string.Empty; //
        txtLastSalary.Text = string.Empty; //
    }

    /// <summary>
    /// This method is used to fill combobox of Localstate,Salutation,Year of passing,
    /// passing class,Caste, userrole and qualification.
    /// </summary>
    private void FillAllComboBoxes()
    {
        DataSet oDsMaster = MasterDataCollectionBL.GetAllMasterData();

        // 0 : Salutation
        ControlUtility.FillDropDownList(oDsMaster.Tables[0], ref cmbSalutation, "Salutation_Id", "Salutation_Name", string.Empty);

        // 1: Category
        ControlUtility.FillDropDownList(oDsMaster.Tables[1], ref cmbCategory, "Category_Id", "Category_Name", Constants.S_SELECT);

        // 2: Designation        
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignation, Constants.UserRoles.Teacher);

        // 3: Religion
        ControlUtility.FillDropDownList(oDsMaster.Tables[3], ref cmbReligion, "Religion_Id", "Religion_Name", Constants.S_SELECT);
        ////4:Qualification
        ControlUtility.FillDropDownList(oDsMaster.Tables[4], ref cmbQualification, "Qualification_Id", "Qualification_Name", Constants.S_SELECT);

        // 5:Passing Class.
        ControlUtility.FillDropDownList(oDsMaster.Tables[5], ref cmbPassingClass, "Class_Id", "Class_Name", string.Empty);

        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            tdTeacherForClass.Visible = true;
            tdTeacherComboForClass.Visible = true;
        }
        else
        {
            tdTeacherForClass.Visible = false;
            tdTeacherComboForClass.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set attributes on user details screen.
    /// </summary>
    private void SetUserDetailsAttributes()
    {
        if (hidUserId.Value == Constants.S_EMPTY_STRING)
        {
            string sLogin = txtFirstName.Text.Trim();
            if (txtLastName.Text != string.Empty)
                sLogin = sLogin + txtLastName.Text.Trim().Substring(0, 1);

            if (sLogin != Constants.S_EMPTY_STRING && sLogin.Length > 19)
                txtUserName.Text = sLogin.Substring(0, 20);
            else
                txtUserName.Text = sLogin.ToString();
        }

        Button oBtnFinish = (Button)wizard_TeacherInfo.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
        oBtnFinish.Attributes.Add("onclick", "javascript:disableControls();");
        ApplyMouseHoverEffect(new List<Button> { oBtnFinish });
        Button oButton = (Button)wizard_TeacherInfo.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton");
        ApplyMouseHoverEffect(new List<Button> { oButton });
        Button oFinishPreviousButton = (Button)wizard_TeacherInfo.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
        
        ApplyMouseHoverEffect(new List<Button> {  oFinishPreviousButton });
    }

    /// <summary>
    /// This method is used to set attributes on educational details screen.
    /// </summary>
    private void SetEducationalDetailsAttributes()
    {
        if (txtMiddleName.Text != string.Empty)
            lblTeacherNameStep3.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text.ToTitleCase() + " " + txtMiddleName.Text.ToTitleCase() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text.ToTitleCase();
        else
            lblTeacherNameStep3.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text.ToTitleCase() + " " + txtLastName.Text.ToTitleCase();
        Button oStepNextButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
        
        Button oStepPreviousButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
        
        Button oCancelButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
        
        ApplyMouseHoverEffect(new List<Button> { oStepNextButton, oStepPreviousButton, oCancelButton });
    }

    /// <summary>
    /// This method is used to set attributes on address details screen.
    /// </summary>
    private void SetAddressDetailsAttributes()
    {
        if (txtMiddleName.Text != string.Empty)
            lblTeacherNameStep2.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text.ToTitleCase() + " " + txtMiddleName.Text.ToTitleCase() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text.ToTitleCase();
        else
            lblTeacherNameStep2.Text = cmbSalutation.SelectedItem +" " +txtFirstName.Text.ToTitleCase()+" " + txtLastName.Text.ToTitleCase();

        Button oStepNextButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
        
        Button oStepPreviousButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
        
        Button oCancelButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
        
        Button oSaveButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("SaveButton");
        
        ApplyMouseHoverEffect(new List<Button> { oStepNextButton, oStepPreviousButton, oCancelButton, oSaveButton });
    }

    /// <summary>
    /// This method is used to set attributes on personal details screen.
    /// </summary>
    private void SetPersonalDetailsAttributes()
    {
        if (wizard_TeacherInfo.FindControl("StartNavigationTemplateContainerID") != null)
        {
            Button oButton = (Button)wizard_TeacherInfo.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            ApplyMouseHoverEffect(new List<Button> { oButton });
            oButton = (Button)wizard_TeacherInfo.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton");
            
            ApplyMouseHoverEffect(new List<Button> { oButton });
        }
    }

    /// <summary>
    /// This method is used to set focus according to step.
    /// in edit mode.
    /// </summary>
    private void SetFocusBaseOnStep()
    {
        if (wizard_TeacherInfo.ActiveStep.Name.Equals("Step 1"))
            txtLocalAddress.Focus();
        else if (wizard_TeacherInfo.ActiveStep.Name.Equals("Step 4"))
            txtEmail.Focus();
    }

    /// <summary>
    /// This method is used to set view mode of the page.
    /// </summary>
    private void SetViewMode()
    {
        if (hidUserId.Value == Constants.S_EMPTY_STRING)
        {
            msViewMode = Constants.ViewMode.New.ToString();
            SetUserDetailsForNewMode();
            ucUserBasicDetails.HideDeleteImage = ucUserBasicDetails.HideViewImage = false;
            EnableDisableFields(true);            
        }
        else
        {
            msViewMode = Constants.ViewMode.Edit.ToString();
            SetUserDetailsForEditMode();
            EnableDisableFields(false);            
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()  //
    {
        chkAddress.Attributes.Add("onclick", "FillPermanentAddress()");
        Button oButton = (Button)wizard_TeacherInfo.FindControl("btnAddDetails");
        if (oButton != null)       
            oButton.Attributes["onclick"] = "ResetUpdateLbl()";       
        Button oButtonAdd = (Button)wizard_TeacherInfo.FindControl("btnAdd");
        if (oButtonAdd != null)       
         oButtonAdd.Attributes["onclick"] = "ResetUpdateLbl()";       
        Button oButtonDel = (Button)wizard_TeacherInfo.FindControl("btnCancelDetails");
        if (oButtonDel != null)       
         oButtonDel.Attributes["onclick"] = "ResetUpdateLbl()";
        ApplyMouseHoverEffect(new List<Button> { oButton,oButtonAdd,oButtonDel});

        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            hidIsPPSNSchool.Value = Constants.S_YES;
            spMandatoryField.Visible = false;
        }
        else
            spMandatoryField.Visible = true;


        //if (miSchoolId == Constants.SchoolId.AaryanBhilarewadi.ToInt())
        if (SchoolBase.Settings.IsAaryanSchool)    //
        {
            ReqJoinDate.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
        }
        else
        {
            ReqJoinDate.Enabled = true;
            RequiredFieldValidator3.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to insert/update user details.
    /// </summary>
    private void SaveTeacherDetails(bool abSendSMS = false) 
    {
        string sMessage = string.Empty;
        if (!CheckIsDuplicateLogin())
        {
            try
            {
                if (moSchoolUserBL.UserId == Constants.I_ZERO)
                {
                    ucUserBasicDetails.StaffUserId = 0;                    
                    InsertAllDetailsOfUserAsTeacher();
                    

                }
                else
                {
                    ucUserBasicDetails.StaffUserId = moSchoolUserBL.UserId;
                    UpdateTeacherDetails();
                    ucUserBasicDetails.PopulateUserBasicDetails();
                    ucEmployeeBasicDetails.StaffUserId = moSchoolUserBL.UserId;   //
                    ucEmployeeBasicDetails.PopulateEmployeeBasicDetails();   //////
                }

                if (hidIsConfig.Value != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Teacher));

                if (abSendSMS && chkSendSMS.Checked)
                    SendSmsToUser(moSchoolUserBL.UserId);
                chkSendSMS.Checked = false;

            string sQuerystring = "UserId=" + hidUserId.Value
                                    + "&TeacherId=" + hidTeacherId.Value
                                    + "&HeadMasterFlag=" + hidHeadMFlag.Value
                                    + "&pIndex=" + hidIndex.Value
                                    +"&pSortExp=" + hidSortExpression.Value
                                    + "&pSortDirc=" + hidSortDirection.Value
                                    + "&QualificationID=" + hidQualificationId.Value
                                    + "&Is_Configured=" + hidIsConfig.Value
                                    + "&UserName=" + sMessage;

                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_TEACHER_INFO + "?" + sEncrypt);
            
			RebuilUserPermissionsCache();

			// Update permission in Session
			Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] = moSchoolUserBL.CanEditOldFinancialYear;
            }
            catch (SqlException ex)
            {
                lblErrorWizard.Text = ex.Message;
                lblErrorWizard.Visible = true;
            }
        }
        else
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = Resources.LocalizedResources.MsgLoginNameExists;
        }
    }

    /// <summary>
    /// This method is used to check that whethet user login is duplicate or not.
    /// </summary>
    private bool CheckIsDuplicateLogin()
    {
        bool bIsDuplicateLogin = moSchoolUserBL.IsUserLoginDuplicate();
        if (bIsDuplicateLogin)
            return true;
        return false;
    }

    /// <summary>
    /// This method is used to insert all details of teacher.
    /// </summary>
    private void InsertAllDetailsOfUserAsTeacher()  /////////new code added
    {
        int iTeacherId = moSchoolUserBL.InsertUserDetailsAsTeacher();
        ucUserBasicDetails.StaffUserId = moSchoolUserBL.UserId;        
        ucUserBasicDetails.PopulateUserBasicDetails();
        ucEmployeeBasicDetails.StaffUserId = moSchoolUserBL.UserId; /////new added
        ucEmployeeBasicDetails.PopulateEmployeeBasicDetails();  /////
        RefreshStaffCache(moSchoolUserBL.UserId, Constants.Action.Insert);
        if (iTeacherId == Constants.I_ZERO)
        {   //// Show error message to user account has not been created successfully.
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Resources.LocalizedResources.UnsuccessfulAccountCreation;
        }
    }

    /// <summary>
    /// This function is used to check the dependancy for standards & subjects.
    /// </summary>
    private void CheckDependanciesForSubAndStd()
    {
        ArrayList oArrMsgForSubStd = new ArrayList();
        IEnumerator oIEnumStd = moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.GetEnumerator();
        while (oIEnumStd.MoveNext())
        {
            TeacherStandardDetailsBL oTeacherStdDetails = (TeacherStandardDetailsBL)oIEnumStd.Current;
            if (oTeacherStdDetails.ConfigurationAction == Constants.Action.Delete)
            {
                string sMsg = SchoolWiseTeacherMasterBL.CheckDependenciesAndGetErrorMessagesForStandard(oTeacherStdDetails);
                if (!sMsg.Equals(string.Empty))
                    oArrMsgForSubStd.Add(sMsg);
            }
        }

        IEnumerator oIEnumSub = moSchoolUserBL.TeacherDetails.moTeacherSubDetails.GetEnumerator();
        while (oIEnumSub.MoveNext())
        {
            TeacherSubjectDetailsBL oTeacherSubDetails = (TeacherSubjectDetailsBL)oIEnumSub.Current;
            if (oTeacherSubDetails.ConfigurationAction == Constants.Action.Delete)
            {
                string sMsg = SchoolWiseTeacherMasterBL.CheckDependenciesAndGetErrorMessagesForSubject(oTeacherSubDetails);
                if (!sMsg.Equals(string.Empty))
                    oArrMsgForSubStd.Add(sMsg);
            }
        }

        if (oArrMsgForSubStd.Count != Constants.I_ZERO)
        {
            string sMsg = string.Empty;
            IEnumerator ie = oArrMsgForSubStd.GetEnumerator();
            while (ie.MoveNext())
                sMsg = sMsg + Convert.ToString(ie.Current) + "<BR>";

            HttpBrowserCapabilities oBrowser = Request.Browser;
            if (oBrowser.Type.StartsWith("Firefox"))
            {
                int iMsgCount = oArrMsgForSubStd.Count;
                int iHeight = iMsgCount * 15;
                divRIMsg.Visible = true;
                divRIMsg.Style.Add(HtmlTextWriterStyle.Height, Convert.ToString(iHeight));
            }
            else
                divRIMsg.Visible = false;
            throw new BusinessLogic.Exceptions.ReferenceExceptions(sMsg);
        }
    }

    /// <summary>
    /// This function is used to delete the standards and subjects.
    /// </summary>
    private void RemoveSubjectsAndStandards()
    {
        IEnumerator oIsubEnum = moSchoolUserBL.TeacherDetails.moTeacherSubDetails.GetEnumerator();
        ArrayList oArrLstSub = new ArrayList();

        while (oIsubEnum.MoveNext())
        {
            TeacherSubjectDetailsBL oTeacherSubDetails = (TeacherSubjectDetailsBL)oIsubEnum.Current;
            if (oTeacherSubDetails.ConfigurationAction == Constants.Action.Delete)
                oArrLstSub.Add(oTeacherSubDetails);
        }

        ArrayList oArrLstStd = new ArrayList();
        IEnumerator oIstdEnum = moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.GetEnumerator();
        while (oIstdEnum.MoveNext())
        {
            TeacherStandardDetailsBL oTeacherStdDetails = (TeacherStandardDetailsBL)oIstdEnum.Current;
            if (oTeacherStdDetails.ConfigurationAction == Constants.Action.Delete)
                oArrLstStd.Add(oTeacherStdDetails);
        }

        if (oArrLstSub.Count > Constants.I_ZERO)
        {
            IEnumerator IeSubject = oArrLstSub.GetEnumerator();
            while (IeSubject.MoveNext())
            {
                TeacherSubjectDetailsBL oTeacherSubDetails = (TeacherSubjectDetailsBL)IeSubject.Current;
                if (oTeacherSubDetails.ConfigurationAction == Constants.Action.Delete)
                    moSchoolUserBL.TeacherDetails.moTeacherSubDetails.Remove(oTeacherSubDetails);
            }
        }

        if (oArrLstStd.Count > Constants.I_ZERO)
        {
            IEnumerator IeStd = oArrLstStd.GetEnumerator();
            while (IeStd.MoveNext())
            {
                TeacherStandardDetailsBL oTeacherStdDetails = (TeacherStandardDetailsBL)IeStd.Current;
                if (oTeacherStdDetails.ConfigurationAction == Constants.Action.Delete)
                    moSchoolUserBL.TeacherDetails.moTeacherStandardDetails.Remove(oTeacherStdDetails);
            }
        }
    }

    /// <summary>
    /// This method is used to update teachers details.
    /// </summary>
    private void UpdateTeacherDetails()
    {
        CheckDependanciesForSubAndStd();
        RemoveSubjectsAndStandards();
        int iTeacherId = moSchoolUserBL.UpdateUserDetailsAsTeacher();
        RefreshStaffCache(moSchoolUserBL.UserId, Constants.Action.Update);
        if (iTeacherId == Constants.I_ZERO)
        {
            //// Show error message to user account has not been created successfully.
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Resources.LocalizedResources.UnsuccessfulAccountCreation;
        }
    }

    /// <summary>
    /// This method is used to fill standard gridview with original details.
    /// </summary>
    private void FillOrginalStandardGrid()
    {
        grdStandards.DataSource = (DataTable)ViewState[S_STANDARDS];
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to fill subject gridview with original details.
    /// </summary>
    private void FillOriginalSubjectGrid()
    {
        grdSubjects.DataSource = (DataTable)ViewState[S_SUBJECTS];
        grdSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to set the default values.
    /// </summary>
    private void SetDefaultValues()
    {        
        hidbtnAddText.Value = "AddDetails";
        hidbtnAddDetailsText.Value = "AddDetails";
        calendar_DOB.DateValue = DateTime.Today;

        txtNationality.Text = Resources.LocalizedResources.Indian;
        txtUserName.ToolTip = Resources.LocalizedResources.ToolTipUserName;
        txtPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        txtConfirmPasswd.ToolTip = Resources.LocalizedResources.PasswordCondition;
        bool bInventoryModuleEnabled = Settings.EnableInventoryModule;
        tdchkCanApproveRequisitions.Visible = bInventoryModuleEnabled;
        tdchkCanCraeteGenerelRequisition.Visible = bInventoryModuleEnabled;
        
        bool bAccountsModuleEnabled = Settings.EnableAccountsModule;
        tdchkCanCreateVoucher.Visible = bAccountsModuleEnabled;
        tdchkCanApproveVoucher.Visible = bAccountsModuleEnabled;
        tdchkCanSelfApprove.Visible = bAccountsModuleEnabled;
		tdCanDeleteVoucher.Visible = bAccountsModuleEnabled;
		tdCanEditFinYear.Visible = bAccountsModuleEnabled;
        chkPublishorUnpublishExam.Checked = Settings.AllowPublishUnpublishExam ;

        if (bAccountsModuleEnabled)
			chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");
        
        trModuleRow1.Visible = bInventoryModuleEnabled || bAccountsModuleEnabled;
        trModuleRow2.Visible = bInventoryModuleEnabled || bAccountsModuleEnabled;

        ListItem oLstHeadmaster = cmbDesignation.Items.FindByText("Principal");
        hidHeadMasterDesgnID.Value = oLstHeadmaster.Value;

        cmbSalutation.Focus();
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        const string S_HEADMASTER_FLAG = "HeadMasterFlag";            
        HidBackUrl.Value = Server.UrlDecode(Request.QueryString.ToString());

        if (QueryString[S_USER_ID] != null)
            hidUserId.Value = QueryString[S_USER_ID];
        if (QueryString[S_TEACHER_ID] != null)
            hidTeacherId.Value = QueryString[S_TEACHER_ID];
        if (QueryString[S_HEADMASTER_FLAG] != null)
            hidHeadMFlag.Value = QueryString[S_HEADMASTER_FLAG];
        if (QueryString[S_QSTR_DESIGNATION_ID] != null)
            hidDesginationId.Value = QueryString[S_QSTR_DESIGNATION_ID];
        if (QueryString[S_QSTR_IS_CONFIG] != null)
            hidIsConfig.Value = QueryString[S_QSTR_IS_CONFIG];
        if (QueryString["pIndex"] != null)
            hidIndex.Value = QueryString["pIndex"];
        if (QueryString["pSortExp"] != null)
            hidSortExpression.Value = QueryString["pSortExp"];
        if (QueryString["pSortDirc"] != null)
            hidSortDirection.Value = QueryString["pSortDirc"];
        if (QueryString[S_QSTR_STEP] != null)
            hidStep.Value = QueryString[S_QSTR_STEP];
    }

    /// <summary>
    /// This method is used to update education details.
    /// </summary>
    /// <param name="aiIndex"></param>
    private void UpdateEducationDetailsRow(int aiIndex)
    {
        int iQualificatonId = Convert.ToInt32(hidQualificationId.Value);
        DataTable oDtEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        DataRow oEditRow = oDtEducationGridDetails.Rows[aiIndex];
        oEditRow[S_QUALIFICATION_ID] = iQualificatonId;
        oEditRow[S_QUALIFICATION] = Convert.ToString(cmbQualification.SelectedItem);
        oEditRow[S_SPECIALISATION] = Convert.ToString(txtSpecialization.Text);
        oEditRow[S_YEAR_OF_PASSING_ID] = Convert.ToInt32(txtYearOfPassing.Text);
        oEditRow[S_PASSING_UNIVERSITY] = txtPassingUniversity.Text.Trim();
        oEditRow[S_CLASS_ID] = Convert.ToInt32(cmbPassingClass.SelectedValue);
        oEditRow[S_CLASS_NAME] = Convert.ToString(cmbPassingClass.SelectedItem);
        grdvwEducationDetails.DataSource = oDtEducationGridDetails;
        ViewState[S_GRIDVIEW_DATASOURCE] = oDtEducationGridDetails;
        btnAddDetails.Text = Resources.LocalizedResources.AddDetails;
        hidbtnAddDetailsText.Value = "AddDetails";
        grdvwEducationDetails.DataBind();
    }

    /// <summary>
    /// This method is used to update exprience details in grid.
    /// </summary>
    /// <param name="aiListIndex"></param>
    private void UpdateExpDetailsinGrid(int aiListIndex)
    {
        DataTable oDtExpListDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];
        DataRow oEditRow = oDtExpListDetails.Rows[aiListIndex];
        oEditRow[S_SCHOOLNAME] = txtSchoolname.Text.Trim();
        oEditRow[S_JOINING_DATE] = Convert.ToDateTime(txtjoinedDate.Text);
        oEditRow[S_LEFT_DATE] = Convert.ToDateTime(txtLeftDate.Text);



        //oDtExpListDetails.PreviousDesignation = Convert.ToString(txtDesignation.Text);
        //oSchoolWiseTeacherMasterBL.Last_Salary = Convert.ToDecimal(txtLastSalary.Text);
        //oSchoolWiseTeacherMasterBL.DurationDays = Convert.ToString(txtDuration.Text);
        //oSchoolWiseTeacherMasterBL.Job_Description = Convert.ToString(txtJobDescription.Text);
        //oSchoolWiseTeacherMasterBL.Reason_for_Leaving = Convert.ToString(txtReasonForLeaving.Text);

        //oDtExpListDetails.Rows[aiListIndex][S_DESIDNATION] = Convert.ToString(txtDesignation.Text);
        //oDtExpListDetails.Rows[aiListIndex][S_LAST_SALARY] = Convert.ToDecimal(txtLastSalary.Text);
        //oDtExpListDetails.Rows[aiListIndex][S_DURATION] = Convert.ToString(txtDuration.Text);
        //oDtExpListDetails.Rows[aiListIndex][S_JOB_DESCRIPTION] = Convert.ToString(txtJobDescription.Text);
        //oDtExpListDetails.Rows[aiListIndex][S_REASON_FOR_LEAVING] = Convert.ToString(txtReasonForLeaving.Text);

        oEditRow["PreviousDesignation"] = Convert.ToString(txtDesignation.Text);
        oEditRow["Last_Salary"] = Convert.ToDecimal(txtLastSalary.Text);
        oEditRow["DurationDays"] = Convert.ToString(txtDuration.Text);
        oEditRow["Job_Description"] = Convert.ToString(txtJobDescription.Text);
        oEditRow["Reason_for_Leaving"] = Convert.ToString(txtReasonForLeaving.Text);



        lstvwExpDetails.DataSource = oDtExpListDetails;
        ViewState[S_LISTVIEW_EXPDETAILS] = oDtExpListDetails;
        lstvwExpDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill subjec t grid.
    /// </summary>
    private void FillSubjectGridView()
    {
        SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtSubjects = oSubjectCollectionBL.GetAssociatedSubjects();
        grdSubjects.DataSource = oDtSubjects.DefaultView;
        grdSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to fill grid with standards.
    /// </summary>
    private void FillStandardGridView()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDsStandards = oStandardCollectionBL.GetAssociatedStandards();
        grdStandards.DataSource = oDsStandards.DefaultView;
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to check that whether the selected qualification is already
    /// exists in the list or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckQualificationIsDuplicate()
    {
        lblDuplicateDetails.Text = Resources.LocalizedResources.QualificationDetailsExists;
        bool bIsDuplicate = false;
        for (int iRowIndex = Constants.I_ZERO; iRowIndex < grdvwEducationDetails.Rows.Count; iRowIndex++)
        {
            if (hidSelectedIndex.Value == Constants.S_EMPTY_STRING)
                bIsDuplicate = CheckIsDuplicateQualificationInNewMode(iRowIndex);
            else
                bIsDuplicate = CheckIsDuplicateQualificationInEditMode(iRowIndex);

            if (bIsDuplicate)
                break;
        }

        return bIsDuplicate;
    }

    /// <summary>
    /// This method is used to check duplicated school name for exprience details in edit mode.
    /// </summary>
    /// <returns></returns>
    private bool CheckSchoolNameIsDuplicate()
    {
        lblChkDuplicate.Text = Resources.LocalizedResources.SchoolNameExists;
        bool bIsDuplicate = false;
        for (int iListRow = Constants.I_ZERO; iListRow < lstvwExpDetails.Items.Count; iListRow++)
        {
            if (hidSlectedExpIndex.Value == Constants.S_EMPTY_STRING)
                bIsDuplicate = CheckIsDuplicateSchoolInNewMode(iListRow);
            else
                bIsDuplicate = CheckIsDuplicateSchoolInEditMode(iListRow);
            if (bIsDuplicate)
                break;
        }

        return bIsDuplicate;
    }

    /// <summary>
    /// This method is used to check is duplicate qualification in new mode.
    /// </summary>
    private bool CheckIsDuplicateQualificationInNewMode(int aiRowNumber)
    {
        if (Convert.ToString(grdvwEducationDetails.DataKeys[aiRowNumber][I_DATAKEY_QUALIFICATION_ID]).Equals(hidQualificationId.Value) && grdvwEducationDetails.Rows[aiRowNumber].Cells[1].Text.ToString() == txtSpecialization.Text.Trim())
        {
            lblDuplicateDetails.Visible = true;
            ClearAllControls();
            return true;
        }

        return false;
    }

    /// <summary>
    /// This method is used to check duplicated school name for exprience details in new mode.
    /// </summary>
    /// <param name="aiRowNumber"></param>
    /// <returns></returns>
    private bool CheckIsDuplicateSchoolInNewMode(int aiRowNumber)
    {
        if (Convert.ToString(lstvwExpDetails.DataKeys[aiRowNumber][S_SCHOOLNAME]).Equals(txtSchoolname.Text))
        {
            lblChkDuplicate.Visible = true;
            ClearTextBoxes();
            return true;
        }

        return false;
    }

    /// <summary>
    ///  This method is used to check is duplicate qualification in edit mode.
    /// </summary>
    private bool CheckIsDuplicateQualificationInEditMode(int aiRowNumber)
    {
        if (Convert.ToString(grdvwEducationDetails.DataKeys[aiRowNumber][I_DATAKEY_QUALIFICATION_ID]).Equals(hidQualificationId.Value)
                                                 && (Convert.ToInt32(hidSelectedIndex.Value) != aiRowNumber && grdvwEducationDetails.Rows[aiRowNumber].Cells[1].Text.ToString() == txtSpecialization.Text.Trim()))
        {
            lblDuplicateDetails.Visible = true;
            ClearAllControls();
            return true;
        }

        return false;
    }

    private bool CheckIsDuplicateSchoolInEditMode(int aiRowNumber)
    {
        if (Convert.ToString(lstvwExpDetails.DataKeys[aiRowNumber][S_SCHOOLNAME]).Equals(txtSchoolname.Text) &&
                                                    (Convert.ToInt32(hidSlectedExpIndex.Value) != aiRowNumber))
        {
            lblChkDuplicate.Visible = true;
            ClearTextBoxes();
            return true;
        }

        return false;
    }

    /// <summary>
    /// This method is used to fill all the control of education details at the time of editing.
    /// </summary>
    /// <param name="aiIndex"></param>
    private void FillEducationDetailsToEdit(int aiIndex)
    {
        string sQualificationId = grdvwEducationDetails.DataKeys[aiIndex][I_DATAKEY_QUALIFICATION_ID].ToString();
        string sSpecilization = grdvwEducationDetails.Rows[aiIndex].Cells[1].Text.ToString();
        DataTable oDtEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        DataColumn[] oArrDatacolumn = new DataColumn[1];
        DataRow oDTRow = oDtEducationGridDetails.NewRow();
        oArrDatacolumn[Constants.I_ZERO] = (DataColumn)oDtEducationGridDetails.Columns["Id"];
        oDtEducationGridDetails.PrimaryKey = oArrDatacolumn;
        //oDTRow = oDtEducationGridDetails.Rows.Find(sQualificationId);
        oDTRow = oDtEducationGridDetails.Select("Qualification_Id=" + sQualificationId + " and Specialization = '" + sSpecilization + "'")[0];
        aiIndex = oDtEducationGridDetails.Rows.IndexOf(oDTRow);

        hidQualificationId.Value = Convert.ToString(oDtEducationGridDetails.Rows[aiIndex][S_QUALIFICATION_ID]);

        cmbQualification.SelectedValue = hidQualificationId.Value;
        txtSpecialization.Text = Convert.ToString(oDtEducationGridDetails.Rows[aiIndex][S_SPECIALISATION]);
        txtYearOfPassing.Text = Convert.ToString(oDtEducationGridDetails.Rows[aiIndex][S_YEAR_OF_PASSING_ID]);
        cmbPassingClass.SelectedValue = Convert.ToString(oDtEducationGridDetails.Rows[aiIndex][S_CLASS_ID]);
        txtPassingUniversity.Text = Convert.ToString(oDtEducationGridDetails.Rows[aiIndex][S_PASSING_UNIVERSITY]);
        btnAddDetails.Text = Resources.LocalizedResources.Update;
        hidbtnAddDetailsText.Value = S_TEXT_UPDATE;
    }

    /// <summary>
    /// This method is used to fill controls for exprience details.
    /// </summary>
    /// <param name="aiListIndex"></param>
    private void FillControlsForExpDetails(int aiListIndex)
    {
        string sSchoolName = lstvwExpDetails.DataKeys[aiListIndex][S_SCHOOLNAME].ToString();
        DataTable oDTExperienceListDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];
        DataColumn[] arrDatacolumn = new DataColumn[1];
        DataRow oDTRow = oDTExperienceListDetails.NewRow();
        arrDatacolumn[Constants.I_ZERO] = (DataColumn)oDTExperienceListDetails.Columns[S_SCHOOLNAME];
        oDTExperienceListDetails.PrimaryKey = arrDatacolumn;
        oDTRow = oDTExperienceListDetails.Rows.Find(sSchoolName);
        txtSchoolname.Text = lstvwExpDetails.DataKeys[aiListIndex][S_SCHOOLNAME].ToString();
        hidSchoolName.Value = txtSchoolname.Text;
        txtjoinedDate.Text = hidJoinDate.Value.ToDateTime().ToString("dd-MMM-yyyy",new CultureInfo("en"));
        txtLeftDate.Text = hidLeftDate.Value.ToDateTime().ToString("dd-MMM-yyyy",new CultureInfo("en"));

        txtDesignation.Text = Convert.ToString(oDTExperienceListDetails.Rows[aiListIndex]["PreviousDesignation"]);
        txtLastSalary.Text = Convert.ToString(oDTExperienceListDetails.Rows[aiListIndex]["Last_Salary"]);
        txtJobDescription.Text = Convert.ToString(oDTExperienceListDetails.Rows[aiListIndex]["Job_Description"]);
        txtReasonForLeaving.Text = Convert.ToString(oDTExperienceListDetails.Rows[aiListIndex]["Reason_for_Leaving"]);
        txtDuration.Text = Convert.ToString(oDTExperienceListDetails.Rows[aiListIndex]["DurationDays"]);
        hidMode.Value = S_EDIT_MODE;
        btnAdd.Text = Resources.LocalizedResources.Update;
        hidbtnAddText.Value = S_TEXT_UPDATE;
    }

    /// <summary>
    /// This method is used to delete education details.    
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void DeleteEducationDetails(int aiRowIndex)
    {
        DataTable oDTEducationGridDetails;
        string sQualificationId = grdvwEducationDetails.DataKeys[aiRowIndex][I_DATAKEY_QUALIFICATION_ID].ToString();
        string sSpecilization = grdvwEducationDetails.Rows[aiRowIndex].Cells[1].Text.ToString();
        oDTEducationGridDetails = (DataTable)ViewState[S_GRIDVIEW_DATASOURCE];
        DataColumn[] arrDatacolumn = new DataColumn[1];
        DataRow oDTRow = oDTEducationGridDetails.NewRow();
        arrDatacolumn[Constants.I_ZERO] = (DataColumn)oDTEducationGridDetails.Columns["Id"];
        oDTEducationGridDetails.PrimaryKey = arrDatacolumn;
        //oDTRow = oDTEducationGridDetails.Rows.Find(sQualification_Id);
        oDTRow = oDTEducationGridDetails.Select("Qualification_Id=" + sQualificationId + " and Specialization = '" + sSpecilization + "'")[0];
        oDTRow.Delete();
        oDTEducationGridDetails.AcceptChanges();
        grdvwEducationDetails.DataSource = oDTEducationGridDetails;
        grdvwEducationDetails.DataBind();
        ViewState[S_GRIDVIEW_DATASOURCE] = oDTEducationGridDetails;
        ClearAllControls();
    }

    /// <summary>
    /// This method is used to delete experience details.
    /// </summary>
    /// <param name="iListIndex"></param>
    private void DeleteExpDetails(int iListIndex)
    {
        DataTable oDTExperienceGridDetails;
        oDTExperienceGridDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];
        string sSchoolName = lstvwExpDetails.DataKeys[iListIndex][S_SCHOOLNAME].ToString();
        DataColumn[] arrDatacolumn = new DataColumn[1];
        DataRow oDTRow = oDTExperienceGridDetails.NewRow();
        arrDatacolumn[Constants.I_ZERO] = (DataColumn)oDTExperienceGridDetails.Columns[S_SCHOOLNAME];
        oDTExperienceGridDetails.PrimaryKey = arrDatacolumn;
        oDTRow = oDTExperienceGridDetails.Rows.Find(sSchoolName);
        oDTRow.Delete();
        oDTExperienceGridDetails.AcceptChanges();
        lstvwExpDetails.DataSource = oDTExperienceGridDetails;
        lstvwExpDetails.DataBind();
        ViewState[S_LISTVIEW_EXPDETAILS] = oDTExperienceGridDetails;
        ClearTextBoxes();
        btnAdd.Text = Resources.LocalizedResources.AddDetails;
        hidbtnAddText.Value = "AddDetails";
    }

    /// <summary>
    /// This method is used to clear the controls of educational details after adding details.
    /// </summary>
    private void ClearAllControls()
    {
        txtPassingUniversity.Text = Constants.S_EMPTY_STRING;
        cmbQualification.ClearSelection();
        txtYearOfPassing.Text = string.Empty;
        txtSpecialization.Text = string.Empty;
        hidbtnAddText.Value = "AddDetails";
        btnAdd.Text = oResourceManager.GetString(hidbtnAddText.Value.Replace(" ", string.Empty));
        cmbPassingClass.ClearSelection();
    }

    /// <summary>
    /// This method is used to set subject assignment step of wizard.
    /// </summary>
    private void SetSubjectAssignmentStep()
    {
        if (hidStep.Value == Constants.I_THREE.ToString())
        {
            wizard_TeacherInfo.ActiveStepIndex = Constants.I_THREE;
            Button oBtnPrev = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
            Button oBtnNext = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
            Button oBtnCancel = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
            Button oBtnSave = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("btnSave");
            Button oFinishButton = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("btnStepSave");
            oFinishButton.Visible = false;

            oBtnPrev.Visible = false;
            oBtnNext.Visible = false;
            oBtnCancel.Text = Resources.LocalizedResources.Close;
            oBtnSave.Visible = true;
            ApplyMouseHoverEffect(new List<Button> {oBtnSave });
        }
    }

    /// <summary>
    /// This method is used to set standard and subject details.
    /// </summary>
    private void SetStandardSubjectDetailsAttributes()
    {
        if (txtMiddleName.Text != string.Empty)
            lblTeacherName.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text.ToTitleCase() + " " + txtMiddleName.Text.ToTitleCase() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text.ToTitleCase();
        else
            lblTeacherName.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text.ToTitleCase() + " " + txtLastName.Text.ToTitleCase();
        Button oStepNextButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
    
        Button oStepPreviousButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
    
        Button oCancelButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
    
        ApplyMouseHoverEffect(new List<Button> { oStepNextButton, oStepPreviousButton, oCancelButton });
    }

    #endregion

    #region Update Mode

    /// <summary>
    /// This method is used to set default values to controls at new mode.
    /// </summary>
    private void SetUserDetailsForNewMode()
    {
        moDsTeacherInfo = SchoolWiseTeacherMasterBL.FetchTeacherStdSubjectDetails(miAcademicYearId, miSchoolId);
        SetTeacherSubjectDetails(0);
        SetTeacherStandardDetails(1);
        ViewState[S_GRIDVIEW_DATASOURCE] = null;
        ViewState[S_LISTVIEW_EXPDETAILS] = null;
        txtLocalCity.Text = Constants.S_DEFAULT_CITY;
        txtState.Text = Constants.S_DEFAULT_STATE;
    }

    /// <summary>
    /// This method is used to set values to the controls of user details at the time of updation.
    /// </summary>
    private void SetUserDetailsForEditMode()
    {
        const int I_TBL_SUBJECT_INDEX = Constants.I_TWO;
        const int I_TBL_STANDARD_INDEX = Constants.I_THREE;
        int iTeacherId = QueryString[S_TEACHER_ID].ToInt();
        int iUserId = QueryString[S_USER_ID].ToInt();        
        moDsTeacherInfo = SchoolWiseTeacherMasterBL.FetchAllTeacherDetails(iTeacherId, miAcademicYearId, miSchoolId, iUserId);
        if (moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows.Count > Constants.I_ZERO)
            SetTeacherPersonalDetails();
        if (moDsTeacherInfo.Tables[I_TBL_EDUCATION_INDEX].Rows.Count > Constants.I_ZERO)
            SetTeacherEducationalDetails();
        if (moDsTeacherInfo.Tables[I_TBL_EXPERIENCE_INDEX].Rows.Count > Constants.I_ZERO)
            SetTeacherExperienceDetails();
        if (moDsTeacherInfo.Tables[I_TBL_SUBJECT_INDEX].Rows.Count > Constants.I_ZERO)
        {
            SetTeacherSubjectDetails(I_TBL_SUBJECT_INDEX);
            ViewState.Add(S_SUBJECTS, moDsTeacherInfo.Tables[I_TBL_SUBJECT_INDEX]);
        }

        if (moDsTeacherInfo.Tables[I_TBL_STANDARD_INDEX].Rows.Count > Constants.I_ZERO)
        {
            SetTeacherStandardDetails(I_TBL_STANDARD_INDEX);
            ViewState.Add(S_STANDARDS, moDsTeacherInfo.Tables[I_TBL_STANDARD_INDEX]);
        }

        if (moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows.Count > Constants.I_ZERO)
            SetTeacherUserDetails();
    }

    /// <summary>
    /// This method is used to set personal details at update mode.
    /// </summary>
    private void SetTeacherPersonalDetails()
    {
        cmbSalutation.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Salutation_Id"]);
        txtFirstName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_First_Name"]);
        txtMiddleName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Middle_Name"]);
        txtLastName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Last_Name"]);
        calendar_DOB.DateValue = Convert.ToDateTime(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Date_of_Birth"]);
        DateTime oDtRetirement = Convert.ToDateTime(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Date_of_Retirement"]);
        lblDateofRetirement.Text = oDtRetirement.ToString("dd-MMM-yyyy");
        txtPhoneNumber.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Phone_Number"]);
        txtMobileNumber.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Mobile_Number"]);
        txtEmergencyNo.Text = moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["EmergencyContactNumber"].ToString();
        chkInternalUser.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["IsInternalUser"]);



        if(miSchoolId == Constants.SchoolId.SNS.ToInt())
            cmbTeachingForClass.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["AssociatedStandardCategory"]);

        if (miSchoolId == Constants.SchoolId.SPS.ToInt())
            cmbType.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["TypeId"]);
        else
            trSPSTeacherType.Visible = false;

        NationalityAndCasteDetails();
        TeacherAddressDetails();
        TeacherExperienceDetails();
    }

    /// <summary>
    /// This method is used to set nationality and caste details to respective controls in update mode.
    /// </summary>
    private void NationalityAndCasteDetails()
    {
        txtNationality.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Nationality"]);
        cmbReligion.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Religion_Id"]);
        cmbCategory.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Category_Id"]);
        cmbDesignation.SelectedValue = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Designation_Id"]);
        txtCasteSubCaste.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Caste_SubCaste"]);
    }

    /// <summary>
    /// This method is used to set the address details to respective controls in edit mode.
    /// </summary>
    private void TeacherAddressDetails()
    {
        txtLocalAddress.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_Address"]);
        txtLocalCity.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_City"]);
        txtState.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_State"]);
        txtLocalPincode.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_Pincode"]);

        string sIsLocalAddress = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Is_LocalAddress"]);

        if (sIsLocalAddress.Equals(Constants.C_YES.ToString()))
            chkAddress.Checked = true;
        else
            chkAddress.Checked = false;

        string sPermanentAddress = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_Address"]);
        if (sPermanentAddress != Constants.S_EMPTY_STRING)
            txtPerAddress.Text = sPermanentAddress;
        string sPermanentCity = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_City"]);
        if (sPermanentCity != Constants.S_EMPTY_STRING)
            txtPerCity.Text = sPermanentCity;
        txtPerState.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_State"]);

        string sPermanentPIN = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_Pincode"]);
        if (!sPermanentPIN.Equals(Constants.S_EMPTY_STRING))
            txtPerPinCode.Text = sPermanentPIN;
    }

    /// <summary>
    /// This method is used to set education details to respective controls in edit mode.
    /// </summary>
    private void SetTeacherEducationalDetails()
    {        
        grdvwEducationDetails.DataSource = moDsTeacherInfo.Tables[I_TBL_EDUCATION_INDEX].DefaultView;
        grdvwEducationDetails.DataBind();
        ViewState[S_GRIDVIEW_DATASOURCE] = moDsTeacherInfo.Tables[I_TBL_EDUCATION_INDEX];
    }

    /// <summary>
    /// This method is used to set teacher education details.
    /// </summary>
    private void SetTeacherExperienceDetails()
    {
        lstvwExpDetails.DataSource = moDsTeacherInfo.Tables[I_TBL_EXPERIENCE_INDEX].DefaultView;
        lstvwExpDetails.DataBind();
        ViewState[S_LISTVIEW_EXPDETAILS] = moDsTeacherInfo.Tables[I_TBL_EXPERIENCE_INDEX];
    }

    /// <summary>
    /// This method is used to set subject details to respective controls in edit mode.
    /// </summary>
    private void SetTeacherSubjectDetails(int aiTblIndex)
    {
        grdSubjects.DataSource = moDsTeacherInfo.Tables[aiTblIndex].DefaultView;
        grdSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to set standard details to respective controls in edit mode.
    /// </summary>
    private void SetTeacherStandardDetails(int aiTblStdIndex)
    {
        grdStandards.DataSource = moDsTeacherInfo.Tables[aiTblStdIndex].DefaultView;
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to set user details to respective control in edit mode.
    /// </summary>
    private void SetTeacherUserDetails()
    {
        txtEmail.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["Email_Address"]);
        txtUserName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["User_Login"]);
        string sPassword = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["User_Password"]);
        chkCanApproveRequisitions.Checked = Convert.ToChar(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanApproveRequisition"]) == Constants.C_YES;
        chkCanCreateGeneralRequisition.Checked = Convert.ToChar(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanCreateGeneralRequisition"]) == Constants.C_YES;
        if (moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanSanctionLeave"] != DBNull.Value)
            chkCanSanctionLeave.Checked = Convert.ToChar(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanSanctionLeave"]) == Constants.C_YES;

		chkCanApproveVoucher.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanApproveVoucher"]);
		chkCanCreateVoucher.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanCreateVoucher"]);
		if (chkCanCreateVoucher.Checked)
        {
			chkCanSelfApprove.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanSelfApprove"]);
			chkCanSelfApprove.InputAttributes.Remove("disabled");
		}
		else
			chkCanSelfApprove.InputAttributes.Add("disabled", "disabled");
        
		chkCanDeleteVoucher.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanDeleteVoucher"]);
		chkCanEditOldFinancialYear.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanEditOldFinancialYear"]);
        chkPublishorUnpublishExam.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["CanPublishUnpublishExam"]);
        sPassword = CommonUtility.GetDecryptedPassword(txtUserName.Text.ToLower(), sPassword);
        hidPassword.Value = sPassword;
        txtPasswd.Attributes.Add("value", sPassword);
        txtConfirmPasswd.Attributes.Add("value", sPassword);
        hidPasswordOfTeacher.Value = sPassword;
        if(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["ShowAllSentSMS"].ToString() != string.Empty)
        chkShowAllSentSMS.Checked = Convert.ToBoolean(moDsTeacherInfo.Tables[I_TBL_USER_INDEX].Rows[0]["ShowAllSentSMS"]);
    }

    /// <summary>
    /// This method is used to fill teacher's experience related details in edit mode.
    /// </summary>
    private void TeacherExperienceDetails()
    {
        txtAchivements.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Achivements"]);
        txtExpYears.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Exprince_In_Years"]);
        txtExpMonths.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Exprince_In_Months"]);


        //SchoolWiseTeacherMasterBL obj = new SchoolWiseTeacherMasterBL();
        //DataSet ds = obj.FetchAllTeacherDetails(aiTeacherId,  aiAcademicYrId, aiSchoolId, aiUserId);
        
        //txtDesignation.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["PreviousDesignation"]);
        //txtLastSalary.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Last_Salary"]);
        //txtJobDescription.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Job_Description"]);
        //txtReasonForLeaving.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Reason_for_Leaving"]);
        //txtDuration.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["DurationDays"]);
    }

    /// <summary>
    /// This method is used to get teacher name
    /// </summary>
    private void GetTeacherName()   /////new ucEmployeeDetails Added for get  details
    {
        if (txtMiddleName.Text != string.Empty)
        {
            lblTeacherName.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtMiddleName.Text.Trim() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text;
            lblTeacherNameStep3.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtMiddleName.Text.Trim() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text;
            lblTeacherNameStep2.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtMiddleName.Text.Trim() + (txtMiddleName.Text.Length > 1 ? " " : ". ") + txtLastName.Text;
        }
        else
        {
            lblTeacherName.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtLastName.Text;
            lblTeacherNameStep3.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtLastName.Text;
            lblTeacherNameStep2.Text = cmbSalutation.SelectedItem + " " + txtFirstName.Text + " " + txtLastName.Text;
        }

        if (hidUserId.Value != string.Empty)
        {
            ucUserBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value);
            ucUserBasicDetails.ShowGradePayOnStaffProfileScreen = Settings.ShowGradePayOnStaffProfileScreen;
            ucUserBasicDetails.InitializeFields();
            ucEmployeeBasicDetails.StaffUserId = Convert.ToInt32(hidUserId.Value); ////
            ucEmployeeBasicDetails.InitializeFields();   //////
        }
    }

    /// <summary>
    /// This method is used to save exprience details.
    /// </summary>
    private void SaveExperienceDetails()
    {
        SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
        if (hidMode.Value != S_EDIT_MODE)
            AddExperienceDetailsToGrid();
        else
            oSchoolWiseTeacherMasterBL.UpdateExperienceDetails();
    }

    /// <summary>
    /// This method is used to add exprience details to grid.
    /// </summary>
    private void AddExperienceDetailsToGrid()
    {
        DataTable oDtExperienceDetails;
        if (ViewState[S_LISTVIEW_EXPDETAILS] == null)
            oDtExperienceDetails = CreateExperienceDetailsTable();
        else
            oDtExperienceDetails = (DataTable)ViewState[S_LISTVIEW_EXPDETAILS];

        if (SchoolBase.Settings.IsAaryanSchool )            ////
        {
           oDtExperienceDetails.Rows.Add(AddExperienceDetailsToDataRow(oDtExperienceDetails.NewRow()));  /////
        } 
        else
        {
            if (txtjoinedDate.Text != string.Empty && txtLeftDate.Text != string.Empty)  ////
                oDtExperienceDetails.Rows.Add(AddExperienceDetailsToDataRow(oDtExperienceDetails.NewRow()));
        }
        DataView oDtItemView = oDtExperienceDetails.DefaultView;
        lstvwExpDetails.DataSource = oDtItemView;
        ViewState[S_LISTVIEW_EXPDETAILS] = oDtExperienceDetails;
        lstvwExpDetails.DataBind();
    }

    /// <summary>
    /// This method is used to create datatable.
    /// </summary>
    /// <returns></returns>
    private DataTable CreateExperienceDetailsTable()
    {
        const string S_STRING_DATE_TYPE = "System.DateTime";
        const string S_STRING_DATA_TYPE = "System.String";

        const string S_DECIMAL_DATA_TYPE = "System.Decimal";
        // Create a new DataTable for educationa details. 
        DataTable oDtExperienceDetails = new DataTable();

        // Add columns to the Item table.
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_SCHOOLNAME, ref oDtExperienceDetails, true);
        AddDataColumnToItemTable(S_STRING_DATE_TYPE, S_JOINING_DATE, ref oDtExperienceDetails, false);
        AddDataColumnToItemTable(S_STRING_DATE_TYPE, S_LEFT_DATE, ref oDtExperienceDetails, false);

        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_DESIDNATION, ref oDtExperienceDetails, false);
        AddDataColumnToItemTable(S_DECIMAL_DATA_TYPE, S_LAST_SALARY, ref oDtExperienceDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_DURATION, ref oDtExperienceDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_JOB_DESCRIPTION, ref oDtExperienceDetails, false);
        AddDataColumnToItemTable(S_STRING_DATA_TYPE, S_REASON_FOR_LEAVING, ref oDtExperienceDetails, false);
        return oDtExperienceDetails;
    }

    /// <summary>
    /// This method is used to set values of control to the datarows of datatable.
    /// </summary>
    private DataRow AddExperienceDetailsToDataRow(DataRow aoDataRow)
    {
        DataRow oDrItem = aoDataRow;
        //// Then add the new row to the collection.

        oDrItem[S_SCHOOLNAME] = txtSchoolname.Text.Trim();
        if (txtjoinedDate.Text == "")                           ////
            oDrItem[S_JOINING_DATE] = S_DEFAULT_DATE_2;               //
        else
        oDrItem[S_JOINING_DATE] = Convert.ToDateTime(txtjoinedDate.Text.Trim());
        if(txtLeftDate.Text == "")                      //
            oDrItem[S_LEFT_DATE] = S_DEFAULT_DATE_2;    //
        else 
        oDrItem[S_LEFT_DATE] = Convert.ToDateTime(txtLeftDate.Text.Trim());
        oDrItem[S_DESIDNATION] = txtDesignation.Text.Trim();
        if (txtLastSalary.Text == "")                         //
            oDrItem[S_LAST_SALARY] = 0;
        else
        oDrItem[S_LAST_SALARY] =  txtLastSalary.Text.Trim(); //
        oDrItem[S_DURATION] = txtDuration.Text.Trim();
        oDrItem[S_JOB_DESCRIPTION] = txtJobDescription.Text.Trim();
        oDrItem[S_REASON_FOR_LEAVING] = txtReasonForLeaving.Text.Trim();

        return oDrItem;
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
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
														  "Accounts Module : There was an error rebuilding User permissions after updating Teacher profile.");
			}
			finally
			{
				if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
					oAccountsBaseClient.Close();
			}
		}
	}

    /// <summary>
    /// This metod is used make vissible step wise save button
    /// </summary>
    private void StepSaveButtonVisble()
    {
     
        HidBackUrl.Value = Server.UrlDecode(Request.QueryString.ToString());
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            hidVisibalStatusCombo.Value = Constants.S_ONE;
        else
            hidVisibalStatusCombo.Value = Constants.S_ZERO;

        if (QueryString[S_TEACHER_ID] != null)
        {
            Button oFinishButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("btnStepSave");
            oFinishButton.Visible = true;
            ApplyMouseHoverEffect(new List<Button> { oFinishButton });
            oFinishButton = (Button)wizard_TeacherInfo.FindControl("StartNavigationTemplateContainerID").FindControl("btnStepSave");
            oFinishButton.Visible = true;
            ApplyMouseHoverEffect(new List<Button> { oFinishButton });
        }
        else
        {
            Button oFinishButton = (Button)wizard_TeacherInfo.FindControl("StartNavigationTemplateContainerID").FindControl("btnStepSave");
            oFinishButton.Visible = false;

            oFinishButton = (Button)wizard_TeacherInfo.FindControl("StepNavigationTemplateContainerID").FindControl("btnStepSave");
            oFinishButton.Visible = false;

        }
    }

    /// <summary>
    ///  This is for Pevious Button Validation
    /// </summary>
    private void BtnPreviousVallidationcause()
    {
        HidBackUrl.Value = Server.UrlDecode(Request.QueryString.ToString());

        if (QueryString[S_TEACHER_ID] != null)
        {
            if (wizard_TeacherInfo.ActiveStep == WizardStep2)
            {

                Button oBtnPrev = (Button)wizard_TeacherInfo.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
                oBtnPrev.ValidationGroup = "Save";
                oBtnPrev.CausesValidation = true;
            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep3)
            {

                Button oBtnPrev = (Button)wizard_TeacherInfo.WizardSteps[2].FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
                oBtnPrev.ValidationGroup = "Save";
                oBtnPrev.CausesValidation = true;
            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep4)
            {
                Button oBtnPrev = (Button)wizard_TeacherInfo.WizardSteps[3].FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
                oBtnPrev.ValidationGroup = "Save";
                oBtnPrev.CausesValidation = true;
             
            }
            if (wizard_TeacherInfo.ActiveStep == WizardStep5)
            {
                Button oBtnPrev = (Button)wizard_TeacherInfo.WizardSteps[4].FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
                oBtnPrev.ValidationGroup = "Save";
                oBtnPrev.CausesValidation = true;
            }
        }
    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId, Constants.Action aoAction)
    {
        try
        {
            List<int> lstUserIds = new List<int>();
            lstUserIds.Add(aiUserId);
            AutoSearchService oAutoSearchService = new AutoSearchService();
            oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, aoAction);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to Enable or Disable the control.
    /// </summary>
    /// <param name="aiFlag"></param>
    private void EnableDisableFields(bool aiFlag)
    {
        txtUserName.Enabled = aiFlag;
        txtPasswd.Enabled = aiFlag;
        txtConfirmPasswd.Enabled = aiFlag;
    }
    #endregion

    //public int aiAcademicYrId { get; set; }

    //public int aiSchoolId { get; set; }

    //public int aiUserId { get; set; }

    //public int aiTeacherId { get; set; }
}