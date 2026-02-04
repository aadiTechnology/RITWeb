// File Name  : AdmissionFormStudentDetails.aspx.cs
// Created By : Amit 
// Date       : 17/11/2009
// Description: This class is used to fill student details on admission form.
//              This screen will be used in Online as well as Manual Admission process.

using BusinessLogic;
using BusinessLogic.Exceptions;
using DocumentFormat.OpenXml.Wordprocessing;
using SchoolEntities;
using SchoolEntities.Admin;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using Utility;
using System.Linq;

public partial class AdmissionFormStudentDetails : SchoolBase
{
    #region " Constants "

    static string msFromUrl = string.Empty;
    const string S_SCREENS_URL = "NewStudentAdmisionsListUI.aspx";
	const string S_NEW_STUDENT_ADMISION_LIST="/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx";
    string sIsOnline = Constants.S_NO;
	string msAmount = string.Empty;
    int? miEnquiryId = 0;
	int miStandardId =0;
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Aadhar Cards\\";
    private const string S_FOLDER_LOCATION_PHOTO = "RITeSchool\\DOWNLOADS\\Admission\\StudentPhoto";
    private const string S_FOLDER_BIRTH_CERTIFICATE = "RITeSchool\\DOWNLOADS\\Admission\\BirthCertificates\\";
    private const string S_FILE_NOT_FOUND = "File does not exists.";
    private const int I_FILE_SIZE_LIMIT = 1048576;
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    bool mbIsEditMode = false;

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to set master page based whether this screen is invoked from 
    /// Online admission process or from Admin/Manual admission process.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);			
			
            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
            if(iSchoolId == Constants.SchoolId.PIONEER.ToInt())
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            else if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && (msFromUrl.Equals("NewStudentAdmisionsListUI.aspx") || msFromUrl.Equals("AdmissionFormParentDetails.aspx")))
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            else if (msFromUrl == S_SCREENS_URL)
                Response.Redirect("../Common/Error.aspx", true);
            else
            {
                if (msFromUrl.Equals("AdmissionFormStudentDetails.aspx"))
                    this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
                else
                {
                    sIsOnline = Constants.S_YES;
                    this.Page.MasterPageFile = "~/RITeSchool/MasterPages/OnlineAdmissionNew.master";
                }
            }
        }
        catch (ThreadAbortException)
        {
            // Do nothing. ASP.NET is redirecting.
            // Always comment this so other developers know why the exception 
            // is being swallowed.
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill all page controls and sets java script properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (miAcademicYearId == Constants.I_ZERO && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
                    Response.Redirect("OnlineAdmissionUI.aspx");

                GetEnquiryID();
                ReadQueryStr();
                SetDefaultValuesToControls();
                FillAllControls();
                SetJavascriptAttributes();
                SetValidationState();
                SetCurrentDate();
                Set10StdStudentDetails();
                CheckWaitingListStatus();
                SetLanguageValidations();
            }
            //else
            //{ 
            //    GetEnquiryID();
            //}

           
           
			miStandardId = ReadQueryString() != 0 ? ReadQueryString() : cmbStd.SelectedValue.ToInt();
				GetMinMaxDate(ViewState["MinMaxDate"] as DataTable, miStandardId);
            
                if (!Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]))
                    SubmissionWizardSteps.EnableFormFee = false;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    private bool CheckWaitingListStatus()
    {
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {
            string sStdName = string.Empty;
            if (cmbStd.Visible || cmbStd.SelectedItem.Text != string.Empty)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            int aiSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
            int aiAcademicYearId = hidAcademicYearId.Value.ToInt();
            int aiUpdatedById = 0;

            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL(aiSchoolId, aiAcademicYearId, aiUpdatedById);
            List<StandardsWaitingList> lstStds = oStudentAdmissionsBL.GetWaitingStandardsList();

           if (lstStds.Any(x => x.StandardName.Equals(sStdName, StringComparison.OrdinalIgnoreCase)))
            {
                if (QueryString["WtListId"] != null && QueryString["WtListId"].ToString() != string.Empty && QueryString["WtListId"].ToString() != "0")
                {
                 
                    DataTable dt = oStudentAdmissionsBL.GetWtStudentDetails(QueryString["WtListId"].ToInt(), hidAcademicYearId.Value.ToInt(), miStandardId);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["IsAdmitted"].ToString().ToLower() == "false")
                        {
                            txtSLastName.Text = dt.Rows[0]["LastName"].ToString();
                            txtSName.Text = dt.Rows[0]["FirstName"].ToString();
                            txtFahterName.Text = dt.Rows[0]["MiddleName"].ToString();
                            txtMobile.Text = dt.Rows[0]["MobileNo"].ToString();

                            txtSLastName.Enabled = false;
                            txtSName.Enabled = false;
                            txtFahterName.Enabled = false;
                            txtMobile.Enabled = false;
                            return true;
                        }
                        else
                        {
                            SetControlState();
                            return false;
                        }
                    }
                    else
                    {
                        SetControlState();
                        return false;
                    }
                }
                else
                {
                    SetControlState();
                    return false;
                }
            }
            else
                return true;
        }
        else
            return true;
    }

    private void SetControlState()
    {       
        Response.Redirect("OnlineAdmissionUI.aspx", true);
    }

    private void GetEnquiryID()
    {
        //String currurl = HttpContext.Current.Request.RawUrl;
        //String querystring = null;
        //int index = currurl.IndexOf('=');
        //if (index >= 0)
        //{
        //    querystring = (index < currurl.Length - 1) ? currurl.Substring(index + 1) : String.Empty;
        //}
        //if (querystring != null)
        //{
        //    string[] Value = IsPostBack == false ? querystring.Split('?') : querystring.Split('%');
        //    miEnquiryId = Convert.ToInt32(Value[0]);
        //    fillAllControlsforEnquiryId(miEnquiryId);
        //}
        if (QueryString["EnquiryId"] != null)
        {
            hidEnquiryId.Value = QueryString["EnquiryId"].ToString();
            fillAllControlsforEnquiryId(hidEnquiryId.Value.ToInt());
        }
    }

    private void fillAllControlsforEnquiryId(int? miEnquiryId)    
    {
        SchoolEnquiryBL oStudentEnquiryBL = new SchoolEnquiryBL();

        miSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

        DataSet ds = oStudentEnquiryBL.GetStudentEnquiryDetails(miEnquiryId, miSchoolId, miAcademicYearId);
        DataTable oDTStudentEnquiryDetails = ds.Tables[0];

        int sStandardId = (oDTStudentEnquiryDetails.Rows[0]["For_std"]).ToInt();
        bool isAdmissionOpenForStd = SchoolEnquiryBL.ChkIfAdmissionOpenforStd(sStandardId, miSchoolId);
        if (!isAdmissionOpenForStd)
        {
            string sQuerystringForFrom = "ErrorMessage=" + "1";
            Response.Redirect("~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx?" + (sQuerystringForFrom), false);
        }
        else
        {
            txtFSurname.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() : string.Empty;
            txtFName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() : string.Empty;
            txtFFatherName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() : string.Empty;
            txtAddress.Text = oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() : string.Empty;
            if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            {
                txtMobile.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() : string.Empty;
                // txtMobile2.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_2"].ToString() : string.Empty;
                txtMobile2.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() : string.Empty;
            }
            else
            {
                txtMobile2.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() : string.Empty;
                txtMobile.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() : string.Empty;
            }
            txtMSurname.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() : string.Empty;
            txtMName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() : string.Empty;
            txtMHName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() : string.Empty;
            txtAddress.Text = oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() : string.Empty;
            txtEmail.Text = oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() : string.Empty;
            txtSLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() : string.Empty;
            txtFahterName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() : string.Empty;
            txtSName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() : string.Empty;
            cmbStd.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["For_std"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["For_std"].ToString() : Constants.S_SELECT;
            cmbYear.SelectedIndex = oDTStudentEnquiryDetails.Rows[0]["Academic_Year_Id"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Academic_Year_Id"].ToInt() : 0;
            DateTime SelectedDate = oDTStudentEnquiryDetails.Rows[0]["DOB"].ToDateTime();
            txtCalDobPopup.Text = SelectedDate.Date.ToString("dd-MMM-yyyy");
            txtSchoolName.Text = oDTStudentEnquiryDetails.Rows[0]["Current_School_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Current_School_Name"].ToString() : string.Empty;           
            rdoMale.Checked = oDTStudentEnquiryDetails.Rows[0]["Gender"].ToString() == "M";
            rdoFemale.Checked = oDTStudentEnquiryDetails.Rows[0]["Gender"].ToString() == "F";

            if (miSchoolId == Constants.SchoolId.SPS.ToInt())
            {
                SubmissionWizardSteps.Visible = false;

                txtPermanentAddress.Text = oDTStudentEnquiryDetails.Rows[0]["PermanentAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["PermanentAddress"].ToString() : string.Empty;
                txtOPhone.Text = oDTStudentEnquiryDetails.Rows[0]["OfficePhoneNo"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["OfficePhoneNo"].ToString() : string.Empty;
                txtRPhone.Text = oDTStudentEnquiryDetails.Rows[0]["ResidencePhoneNo"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["ResidencePhoneNo"].ToString() : string.Empty;
                txtPreviousSchoolAddress.Text = oDTStudentEnquiryDetails.Rows[0]["LastSchoolAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["LastSchoolAddress"].ToString() : string.Empty;
                txtPassportNo.Text = oDTStudentEnquiryDetails.Rows[0]["PassportNo"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["PassportNo"].ToString() : string.Empty;
                txtNationality.Text = oDTStudentEnquiryDetails.Rows[0]["Nationality"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Nationality"].ToString() : string.Empty;
                hidFOccupationId.Value = oDTStudentEnquiryDetails.Rows[0]["FatherOccupationId"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["FatherOccupationId"].ToString() : string.Empty;
                hidMOccupationId.Value = oDTStudentEnquiryDetails.Rows[0]["MotherOccupationId"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MotherOccupationId"].ToString() : string.Empty;
                hidFMobileNo.Value = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() : string.Empty;
                hidMMobileNo.Value = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() : string.Empty;                

                if(txtSLastName.Text != string.Empty)
                    txtSLastName.Enabled = false;
                if (txtSName.Text != string.Empty)
                    txtSName.Enabled = false;
                if (txtFahterName.Text != string.Empty)
                    txtFahterName.Enabled = false;
                if (rdoMale.Checked)
                {
                    rdoMale.Checked = true;
                    rdoMale.Enabled = false;
                    rdoFemale.Enabled = false;
                }
                else
                {
                    rdoFemale.Checked = true;
                    rdoMale.Enabled = false;
                    rdoFemale.Enabled = false;
                }
                if (txtCalDobPopup.Text != string.Empty)
                {
                    txtCalDobPopup.Enabled = false;
                    CalDobPopup.Enabled = false;
                }
                if (txtNationality.Text != string.Empty)
                    txtNationality.Enabled = false;
                if (txtPassportNo.Text != string.Empty)
                    txtPassportNo.Enabled = false;
                if (txtRPhone.Text != string.Empty)
                    txtRPhone.Enabled = false;
                if (txtOPhone.Text != string.Empty)
                    txtOPhone.Enabled = false;
                if (txtFSurname.Text != string.Empty)
                    txtFSurname.Enabled = false;
                if (txtFName.Text != string.Empty)
                    txtFName.Enabled = false;
                if (txtFFatherName.Text != string.Empty)
                    txtFFatherName.Enabled = false;
                if (txtMSurname.Text != string.Empty)
                    txtMSurname.Enabled = false;
                if (txtMName.Text != string.Empty)
                    txtMName.Enabled = false;
                if (txtMHName.Text != string.Empty)
                    txtMHName.Enabled = false;
                if (txtAddress.Text != string.Empty)
                    txtAddress.Enabled = false;
                if (txtSchoolName.Text != string.Empty)
                    txtSchoolName.Enabled = false;
                if (txtPermanentAddress.Text != string.Empty)
                    txtPermanentAddress.Enabled = false;
                if (txtPreviousSchoolAddress.Text != string.Empty)
                    txtPreviousSchoolAddress.Enabled = false;
                if (txtEmail.Text != string.Empty)
                    txtEmail.Enabled = false;
            }
        }
    }

    /// <summary>
    /// This event is used to get respected standard as per selected acadamic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
            StandardCollectionBL oStdCollection = new StandardCollectionBL(iSchoolId, Convert.ToInt32(cmbYear.SelectedValue));
            DataTable oDT = oStdCollection.GetAssociatedStandards();
            ControlUtility.FillDropDownList(oDT, ref cmbStd, "standard_id", "standard_name", string.Empty);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

	protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable oDtMinMaxDate = ViewState["MinMaxDate"] as DataTable;
			GetMinMaxDate(oDtMinMaxDate, cmbStd.SelectedValue.ToInt());
		}
		catch (Exception ex)
		{
			BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

    
   

    /// <summary>
    /// This event is used to save student details at time of student online admission.
    /// And redirect to parent detail page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                CheckWaitingListStatus();

                string sErrorMsg = SaveStudentsDetails();
                string sStandardName = string.Empty;
                //if (cmbStd.Visible)
                sStandardName = cmbStd.SelectedItem.Text;
                //else
                //    sStandardName = lblStdName.Text;

                if (sErrorMsg == string.Empty)
                {
                    int iWtListId = 0;
                    if (QueryString["WtListId"] != null && QueryString["WtListId"].ToString().Trim() != string.Empty)
                        iWtListId = QueryString["WtListId"].ToInt();

                    string sQueryString = "sIsOnline=" + sIsOnline + "&StandardId=" + miStandardId + "&Amount=" + hidAmount.Value + "&EnableAdmissionFormFee=" + Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]) + "&EnquiryId=" + (hidEnquiryId.Value != null ? hidEnquiryId.Value.ToInt() : 0) + "&IsEditMode=" + hidIsEditMode.Value + "&FatherOccupation=" + hidFOccupationId.Value + "&FatherMobileNo=" + hidFMobileNo.Value + "&AcademicYearId=" + hidAcademicYearId.Value + "&StandardName=" + sStandardName + "&WtListId=" + iWtListId;
                    Response.Redirect("~/RITeSchool/Admission/AdmissionFormParentDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString), false);

                }
                else
                {
                    tdErrorMessage.Visible = true;
                    lblError.Visible = true;
                    lblError.Text = sErrorMsg;
                }
            }
        }
        catch (SqlException ex)
        {
            tdErrorMessage.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (ApplicationException ex)
        {
            tdErrorMessage.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void DOB_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv = sender as CustomValidator;
            if (txtCalDobPopup.Text == string.Empty)
            {
                cv.ErrorMessage = "DOB should not be blank.";
                e.IsValid = false;
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParse(txtCalDobPopup.Text, out dt))
                {
                    if (txtCalDobPopup.Text.ToDateTime() >= hidMinBdate.Value.ToDateTime() && txtCalDobPopup.Text.ToDateTime() <= hidMaxBdate.Value.ToDateTime())
                    {
                        e.IsValid = true;
                    }
                    else
                    {
                        cv.ErrorMessage = "DOB should be between " + hidMinBdate.Value.ToDateTime().ToString(Constants.S_DATE_FORMAT) + " and " + hidMaxBdate.Value.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                        e.IsValid = false;
                    }
                }
                else
                {
                    cv.ErrorMessage = "DOB is not in correct format.";
                    e.IsValid = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void BlackListStudent_Validate(object obj, ServerValidateEventArgs e)
    {
        try
        {
            if (txtAadharCardNo.Text.Trim() != string.Empty)
            {
                int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
                StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
                string sLeftDate = oStudentAdmissionsBL.ValidateBlackListStudent(iSchoolId, txtAadharCardNo.Text.Trim());

                CustomValidator cv = obj as CustomValidator;
                if (sLeftDate != string.Empty)
                {
                    cv.ErrorMessage = "You have left this school on " + sLeftDate + ".";
                    e.IsValid = false;
                }
                else
                    e.IsValid = true;
            }
            else
                e.IsValid = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void FatherAadharFile_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;
        if (!flUploadFatherAaadhar.HasFile)
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Please select file for Father's Aadhar Card.";
            return;
        }

        string extension = Path.GetExtension(flUploadFatherAaadhar.FileName).ToLower();
        List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

        if (!allowedExtensions.Contains(extension))
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Please select valid file type for Father's Aadhar Card.";
            return;
        }

        // Max size: 1MB = 1 * 1024 * 1024 bytes
        if (flUploadFatherAaadhar.PostedFile.ContentLength > 1048576)
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Size of Father's Aadhar Card file should not be more than 1 mb.";
            return;
        }

        args.IsValid = true;
    }

    protected void MotherAadharFile_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;
        if (!flUploadMotherAaadhar.HasFile)
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Please select file for Mother's Aadhar Card.";
            return;
        }

        string extension = Path.GetExtension(flUploadMotherAaadhar.FileName).ToLower();
        List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

        if (!allowedExtensions.Contains(extension))
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Please select valid file type for Mother's Aadhar Card.";
            return;
        }

        // Max size: 1MB = 1 * 1024 * 1024 bytes
        if (flUploadMotherAaadhar.PostedFile.ContentLength > 1048576)
        {
            args.IsValid = false;
            oCustomValidator.ErrorMessage = "Size of Mother's Aadhar Card file should not be more than 1 mb.";
            return;
        }

        args.IsValid = true;
    }

    protected void CasteCertFIle_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator oCustomValidator = source as CustomValidator;

        if (flUploadCastCert.FileName != string.Empty)
        {
            string extension = Path.GetExtension(flUploadCastCert.FileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png", ".bmp" };

            if (!allowedExtensions.Contains(extension))
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Please select valid file type for Caste Certificate.";
                return;
            }

            // Max size: 1MB = 1 * 1024 * 1024 bytes
            if (flUploadCastCert.PostedFile.ContentLength > 1048576)
            {
                args.IsValid = false;
                oCustomValidator.ErrorMessage = "Size of Caste Certificate file should not be more than 1 mb.";
                return;
            }
        }

        args.IsValid = true;
    }

    #endregion " Events " 

    #region " Private Methods "

    /// <summary>
    /// This method is used to fill all available page controls.
    /// </summary>
    private void FillAllControls()
    {
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        hidSchoolId.Value = iSchoolId.ToString();

        //if (iSchoolId == Constants.SchoolId.PPSN.ToInt() || iSchoolId == Constants.SchoolId.PPS.ToInt())
        //    lblAadharCard.Text = "Birth Certificate:";
        //else
        //{
            lblAadharCard.Text = "Aadhar Card Scan Copy:";
            cstValidateFileUpload.ErrorMessage = "Please select valid file type for Aadhar Card Scan Copy.";
        //}

        if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            hidShowResidentTypeValidation.Value = Constants.S_YES;
            spnFatherMobileNo.InnerText = "Father's Mobile No.";
            spnMotherMobileNo.InnerText = "Mother's Mobile No.";
            txtMobile2.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            reqMobileNo.ErrorMessage = "Father mobile number should not be blank.";
            cst_MobileNumber.ErrorMessage = "Father mobile number should be of 10 digits.";
            cst_MobileNumber2.ErrorMessage = "Mother Mobile number should be of 10 digits.";
            cstValidateStudentPhoto.Enabled = true;
            trStudentPhoto.Visible = true;
            trStudentPhotoNote.Visible = true;
            reqValNationality.Enabled = true;
            reqValMotherTongue.Enabled = true;

            System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");
            //hidShowAadharCardValidation.Value = Constants.S_YES;
            //txtAadharCardNo.BackColor = oBackColor;
            cmbReligion.BackColor = oBackColor;
            txtAadharCardNo.BackColor = oBackColor;
            txtNationality.BackColor = oBackColor;
            txtMotherTongue.BackColor = oBackColor;
            txtBirthState.BackColor = oBackColor;
            txtBirthCountry.BackColor = oBackColor;
            hidShowreligionValidation.Value = Constants.S_YES;
           
            tdAadharHeader.Visible = false;
            tdAadharData.Visible = false;
            trAadharNote.Visible = false;

            tdSaralNoH.Visible = false;
            tdSaralNoData.Visible = false;
            cstValBirthState.Enabled = true;
            custValBirthCountry.Enabled = true;
          
          
            txtNameOnAadharCard.BackColor = oBackColor;
            reqValNameAsPerAadhar.Enabled = true;
           
            cstSchoolUDISE.Enabled = false;
            CustomValidator6.Enabled = false;
        }
        else
        {
            hidShowResidentTypeValidation.Value = Constants.S_NO;
            cstValidateStudentPhoto.Enabled = false;
            trStudentPhoto.Visible = false;
            trStudentPhotoNote.Visible = false;
            hidShowreligionValidation.Value = Constants.S_NO;
        }

        if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
        {
            System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtAadharCardNo.BackColor = oBackColor;
            txtNameOnAadharCard.BackColor = oBackColor;
            reqValNameAsPerAadhar.Enabled = true;
        }

        hidShowAdmissionCategoryValidation.Value = Constants.S_NO;
        if (iSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            spnLocationHeader.InnerText = "Admission Category";
            hidShowAdmissionCategoryValidation.Value = Constants.S_YES;
            txtMobile2.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            reqMobileNo.ErrorMessage = "Mobile number1 should not be blank.";
            reqmobileno2.ErrorMessage = "Mobile number2 should not be blank.";
            cst_MobileNumber.ErrorMessage = "Mobile number1 should be of 10 digits.";
            ////  cst_MobileNumber2.ErrorMessage = "Mother Mobile3 number should be of 10 digits.";

            System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");
            hidShowFullNameValidation.Value = Constants.S_YES;
           
            txtSLastName.BackColor = oBackColor;
            //txtFahterName.BackColor = oBackColor;
            txtMotherTongue.BackColor = oBackColor;
            txtBirthPlace.BackColor = oBackColor;
            txtAadharCardNo.BackColor = oBackColor;            
            txtLanguageKnown.BackColor = oBackColor;
            cmbStudentBloodGroup.BackColor = oBackColor;
            txtFSurname.BackColor = oBackColor;
            txtMSurname.BackColor = oBackColor;
            txtFFatherName.BackColor = oBackColor;
            txtMHName.BackColor = oBackColor;
            txtNameOnAadharCard.BackColor = oBackColor;

            //txtFFatherName.BackColor = oBackColor;
            //txtMHName.BackColor = oBackColor;
            txtFAge.BackColor = oBackColor;
            txtMAge.BackColor = oBackColor;
            txtRPhone.BackColor = oBackColor;

            txtFahterName.BackColor = oBackColor;
            
            reqValMotherTongue.Enabled = true;

            tdSaralNoData.Visible = false;
            tdSaralNoH.Visible = false;
            reqValLangKnown.Enabled = true;
            reqValBloodGrp.Enabled = true;
            reqValtxtRPhone.Enabled = true;

            cstValMiddleName.Enabled = true;
            reqValFatherFName.Enabled = true;
            reqValMotherHName.Enabled = true;
            custValMobileNo2.Enabled = true;
            reqValNameAsPerAadhar.Enabled = true;

            tdPrfBatchHeader.Visible = true;
            tdPrfBatch.Visible = true;

            reqValMotherAge.Enabled = true;
            reqValtxtFAge.Enabled = true;
            compValFAge.Enabled = true;
            cmpValMAge.Enabled = true;

            trResidenceType.Visible = false;
            trResidenceTypeHeader.Visible = false;

            cstLastName.Enabled = false;
            reqValStudLastName.Enabled = true;

            cstValidateFatherName.Enabled = false;
            reqValFLastName.Enabled = true;
            cstValidateMotherName.Enabled = false;
            reqValmLastName.Enabled = true;
            cstValBirthCertificate.Enabled = true;

            spnAadharNameNote.InnerText = "(Student name as per Birth Certificate / Aadhar Card / Leaving Certificate)";
            spnFatherAadharName.InnerText = "----------------------------As per student Birth Certificate / Aadhar Card / Leaving Certificate----------------------------";
            spnMotherAadharName.InnerText = "----------------------------As per student Birth Certificate / Aadhar Card / Leaving Certificate----------------------------";

            trCasteCert.Visible = true;
            cstValCasteCertFile.Enabled = true;

            tdMotherAadharCard.Visible = true;
            cstValMotherAadharFile.Enabled = true;

            tdAadharCardHeaderMother.Visible = true;
            tdAadharCardHeaderFather.Visible = true;

            tdFatherAadharCard.Visible = true;
            cstValFatherAadharFile.Enabled = true;
            
        }

        if (iSchoolId == Constants.SchoolId.BFS.ToInt() || iSchoolId == Constants.SchoolId.SNS.ToInt() || iSchoolId == Constants.SchoolId.PPS.ToInt() || iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
            hidShowAadharCardValidation.Value = Constants.S_YES;
        else
            hidShowAadharCardValidation.Value = Constants.S_NO;

        if (iSchoolId == Constants.SchoolId.DSK.ToInt() || iSchoolId == Constants.SchoolId.SNS.ToInt())
            colpnlHealthDetails.Visible = true;
        else
            colpnlHealthDetails.Visible = false;

        hidShowBirthValidations.Value = ((iSchoolId == Constants.SchoolId.PPSN.ToInt() || iSchoolId == Constants.SchoolId.BFS.ToInt() || iSchoolId == Constants.SchoolId.SNS.ToInt() || iSchoolId == Constants.SchoolId.PPS.ToInt()) ? Constants.S_YES : Constants.S_NO);
               
        if (iSchoolId == Constants.SchoolId.PPSH.ToInt())
            hidShowValidationForSchool.Value = Constants.S_YES;
        else
            hidShowValidationForSchool.Value = Constants.S_NO;

        if (iSchoolId == Constants.SchoolId.SNS.ToInt() || iSchoolId == Constants.SchoolId.PPSN.ToInt() || iSchoolId == Constants.SchoolId.PPS.ToInt() || iSchoolId == Constants.SchoolId.PPSH.ToInt())
        {   
            txtBirthTaluka.Style.Add("background-color", "#ffffa0");
            txtBirthDistrict.Style.Add("background-color", "#ffffa0");
            txtBirthPlace.Style.Add("background-color", "#ffffa0");
            colpnlStudentAdditionalDetails.Visible = true;
        }       
        else
            colpnlStudentAdditionalDetails.Visible = false;

        //if (iSchoolId == Constants.SchoolId.DPIS.ToInt())
            colpnlStudentAdditionalDetails.Visible = true;

        if (iSchoolId == Constants.SchoolId.PPS.ToInt() || iSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            trAdditional1.Visible = false;
            trAdditional2.Visible = false;
            trAdditional3.Visible = false;
            trAdditional4.Visible = false;
            cstValBirthState.Enabled = true;
            txtBirthState.Style.Add("background-color", "#ffffa0");

            custValBirthCountry.Enabled = true;
            txtBirthCountry.Style.Add("background-color", "#ffffa0");
        }

        if (iSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            reqValBloodGrp.Enabled = true;
            cmbStudentBloodGroup.Style.Add("background-color", "#ffffa0");
            cmbReligion.Style.Add("background-color", "#ffffa0");
            txtLanguageKnown.Style.Add("background-color", "#ffffa0");

            reqValLangKnown.Enabled = true;
            hidShowreligionValidation.Value = Constants.S_YES;
        }

        // Table Indices
        //const int AdmissionMasterData.I_TABLE_ACADAMIC_YEARS = 0;
        //const int AdmissionMasterData.I_TABLE_NEW_ACADAMIC_YEAR_ID = 1;
        //const int AdmissionMasterData.I_TABLE_STANDARDS = 2;
        //const int AdmissionMasterData.I_TABLE_RELIGIONS = 3;
        //const int AdmissionMasterData.I_TABLE_CATAGORIES = 6;
       
        //const int AdmissionMasterData.I_TABLE_MOTHER_DATA = 7;
        //const int AdmissionMasterData.I_TABLE_STUDENT_DETAILS = 8;
        //const int AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS = 12;

        //int AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS = 16;
        //int AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS = 17;

        //int AdmissionMasterData.I_TABLE_BLOOD_GROUPS = 9;
        //int AdmissionMasterData.I_TABLE_RESIDENCE_TYPES = 10;
        //int AdmissionMasterData.I_TABLE_SECOND_LANGUAGE = 11;

        //int AdmissionMasterData.I_TABLE_LOCATION_AREA = 7;
        int iStudentAdmisssionID = Convert.ToInt32(hidStudentAdmisssionID.Value);

        //if (iStudentAdmisssionID == 0)
        //{
        //    AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS = 13;
        //    AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS = 14;
        //}

        //if (iStudentAdmisssionID != 0)
        //{
        //    AdmissionMasterData.I_TABLE_LOCATION_AREA = 11;
        //    AdmissionMasterData.I_TABLE_BLOOD_GROUPS = 13;
        //    AdmissionMasterData.I_TABLE_RESIDENCE_TYPES = 14;
        //    AdmissionMasterData.I_TABLE_SECOND_LANGUAGE = 15;            
        //}

        if (QueryString["IsCurrentYearAdmission"] != null)
        {
            S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_YES;

            string sVal = QueryString["IsCurrentYearAdmission"];
            if (sVal == "False")
                S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_NO;

            hidAcademicYearId.Value = QueryString["AcadmicYearId"];
            if(QueryString["AcadmicYearId"] == null)
                hidAcademicYearId.Value = QueryString["AcademicYearId"];
        }
        else
        {
            S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
            //hidAcademicYearId.Value = miAcademicYearId.ToString();
            if (QueryString["AcademicYearId"] != null && QueryString["AcademicYearId"] != Constants.S_ZERO)
                hidAcademicYearId.Value = QueryString["AcademicYearId"].ToString();
            else
                hidAcademicYearId.Value = miAcademicYearId.ToString();
        }

        DataSet oDataSet = MasterDataCollectionBL.GetAllMasterDataForStudentAdmission(iSchoolId, iStudentAdmisssionID, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR, hidAcademicYearId.Value.ToInt());
        int iNewAcadamicYearID=0;
        if(oDataSet.Tables[AdmissionMasterData.I_TABLE_NEW_ACADAMIC_YEAR_ID].Rows[0][0]!=DBNull.Value)
         iNewAcadamicYearID = Convert.ToInt32(oDataSet.Tables[AdmissionMasterData.I_TABLE_NEW_ACADAMIC_YEAR_ID].Rows[0][0]);

        // Fills Acadamic Year combo and set it to default value.
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_ACADAMIC_YEARS], ref cmbYear, "Academic_Year_ID", "AcademicYear", string.Empty);
        if (iNewAcadamicYearID != 0)
        {
            cmbYear.SelectedValue = iNewAcadamicYearID.ToString();
            cmbYear.Enabled = false;
        }

        // Fill Standard conbo and set to default value.
        if (!IsPostBack)
        {
            ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_STANDARDS], ref cmbStd, "Standard_Id", "Standard_Name", Constants.S_SELECT);
        }
		miStandardId = ReadQueryString();
		ViewState["MinMaxDate"] = oDataSet.Tables[AdmissionMasterData.I_TABLE_STANDARDS];
		
		GetMinMaxDate(oDataSet.Tables[AdmissionMasterData.I_TABLE_STANDARDS], miStandardId);
        
        if (miStandardId != 0)
        {
            cmbStd.SelectedValue = miStandardId.ToString();
            lblStdName.Text = cmbStd.SelectedItem.Text;           
        }

        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_RELIGIONS], ref cmbReligion, "Religion_Id", "Religion_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_CATAGORIES], ref cmbCategory, "Category_Id", "Category_Name", Constants.S_SELECT);
        
        if(!IsPostBack)
		ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_LOCATION_AREA], ref cmbLivingLocation, "LivingLocationId", "LivingLocationName", Constants.S_SELECT);

        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_BLOOD_GROUPS], ref cmbStudentBloodGroup, "Id", "BloodGroup", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_RESIDENCE_TYPES], ref cmbResidenceType, "ResidenceTypeId", "Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_SECOND_LANGUAGE], ref cmbSecondSLanguageSubjectId, "Subject_Id", "Subject_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_THIRD_LANGUAGE], ref cmbThirdLanguage, "Subject_Id", "Subject_Name", Constants.S_SELECT);
       
        // Condition true when need to add sibling admission  
        if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null && moUserRole != Constants.UserRoles.Student)
        {
            txtMotherTongue.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Mother_Tongue"].ToString();
            txtNationality.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Nationality"].ToString();
            cmbReligion.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Religion"].ToString();
            txtCasteAndSubcaste.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Caste_Subcaste"].ToString();

            txtFSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["GuardianLastName"].ToString();
            txtFName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["GuardianFirstName"].ToString();
            txtFFatherName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["GuardianMiddleName"].ToString();
            txtFAge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["GuardianAge"].ToString() == Constants.S_ZERO ? string.Empty : oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["GuardianAge"].ToString();
            txtAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Address"].ToString();
            txtCity.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["City"].ToString();
            txtState.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["State"].ToString();
            txtPincode.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Pincode"].ToString();
            if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            {
                txtMobile.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MobileNumber"].ToString();
                txtMobile2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MobileNumber2"].ToString();
            }
            else
            {
                txtMobile2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MobileNumber"].ToString();
                txtMobile.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MobileNumber2"].ToString();
            }
            txtRPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ResidancePhoneNumber"].ToString();
            txtOPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["OfficePhoneNumber"].ToString();
            txtEmail.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["EmailAddress"].ToString();
        
           

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LivingLocationId"].ToString() != string.Empty)
                cmbLivingLocation.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LivingLocationId"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LivingLocationName"].ToString() != string.Empty)
                txtLivingLocation.Text =  oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LivingLocationName"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ResidenceTypeId"].ToString() != string.Empty)
                cmbResidenceType.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ResidenceTypeId"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["SecondLanguageSubjectId"].ToString() != string.Empty)
                cmbSecondSLanguageSubjectId.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["SecondLanguageSubjectId"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ThirdLanguageSubjectId"].ToString() != string.Empty)
                cmbThirdLanguage.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ThirdLanguageSubjectId"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Blood_Group"].ToString() != string.Empty)
                cmbStudentBloodGroup.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Blood_Group"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PenNo"].ToString() != string.Empty)
                txtPenNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PenNo"].ToString();
            
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PenNo"].ToString() != string.Empty)
                txtApaarId.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ApaarId"].ToString();
            
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Last_Name"].ToString() != string.Empty)
                txtSLastName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Last_Name"].ToString();

            hidStudentPhoto.Value = Constants.S_ZERO;
            if (QueryString["IsEditMode"] != null && QueryString["IsEditMode"].ToString() == Constants.S_ONE)
            {
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["First_Name"].ToString() != string.Empty)
                    txtSName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["First_Name"].ToString();

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["DOB"].ToString() != string.Empty)
                    txtCalDobPopup.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["DOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT);

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["For_Standard"].ToString() != string.Empty)                
                    cmbStd.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["For_Standard"].ToString();                

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PreferenceBatchId"].ToString() != string.Empty)
                    cmbPreferenceBatch.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PreferenceBatchId"].ToString();
                else
                    cmbPreferenceBatch.SelectedValue = Constants.S_ZERO;
                
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["AadharCardScanCopy"].ToString() != string.Empty)
                {
                    btnView1.Visible = true;
                    string sAadharCardFilePath = "..//DOWNLOADS//Aadhar Cards//" + oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["AadharCardScanCopy"].ToString();
                    btnView1.Attributes.Add("onclick", " window.open('" + sAadharCardFilePath + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
                    hidAadharCardScanCopy.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["AadharCardScanCopy"].ToString();
                }

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["BirthCertificateScanCopyFileName"].ToString() != string.Empty)
                {
                    btnViewBirthCert.Visible = true;
                    string sBirthCertFilePath = "..//DOWNLOADS//Admission//BirthCertificates//" + oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["BirthCertificateScanCopyFileName"].ToString();
                    btnViewBirthCert.Attributes.Add("onclick", " window.open('" + sBirthCertFilePath + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
                    hidBirthCertificateScanCopy.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["BirthCertificateScanCopyFileName"].ToString();
                }

                imgPhoto.Visible = true;
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["HasStudentPhoto"].ToString() == "1")
                {
                    imgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentPhotoImage"]);
                    hidStudentPhoto.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["HasStudentPhoto"].ToString();
                }
                else
                {
                    imgPhoto.ImageUrl = "~/RITeSchool/images/Student_BlankPh.jpg";
                }

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["CasteCertScanCopy"] != DBNull.Value && oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["CasteCertScanCopy"].ToString() != string.Empty)
                {
                    imgCastCert.Visible = true;
                    string sCasteCertScanCopy = "..//DOWNLOADS//Admission//CasteCertificate//" + oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["CasteCertScanCopy"].ToString();                    
                    imgCastCert.Attributes.Add("onclick", " window.open('" + sCasteCertScanCopy + "'); return false;");
                    hidCasteCertFileName.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["CasteCertScanCopy"].ToString();
                }

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FatherAadharCardScanCopy"] != DBNull.Value && oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FatherAadharCardScanCopy"].ToString() != string.Empty)
                {
                    imgFatherAadhar.Visible = true;
                    string sCasteCertScanCopy = "..//DOWNLOADS//ParentAadharCards//" + oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FatherAadharCardScanCopy"].ToString();
                    imgFatherAadhar.Attributes.Add("onclick", " window.open('" + sCasteCertScanCopy + "'); return false;");
                    hidFatherAadharCardFileName.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FatherAadharCardScanCopy"].ToString();
                }

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MotherAadharCardScanCopy"] != DBNull.Value && oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MotherAadharCardScanCopy"].ToString() != string.Empty)
                {
                    imgMotherAadhar.Visible = true;
                    string sCasteCertScanCopy = "..//DOWNLOADS//ParentAadharCards//" + oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MotherAadharCardScanCopy"].ToString();                    
                    imgMotherAadhar.Attributes.Add("onclick", " window.open('" + sCasteCertScanCopy + "'); return false;");
                    hidMotherAadharCardFileName.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MotherAadharCardScanCopy"].ToString();
                }
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Middle_Name"].ToString() != string.Empty)
                txtFahterName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Middle_Name"].ToString();           
           
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["BirthPlace"].ToString() != string.Empty)
                txtBirthPlace.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["BirthPlace"].ToString();
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolName"].ToString() != string.Empty)
                txtSchoolName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolName"].ToString();
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolAddress"].ToString() != string.Empty)
                txtPreviousSchoolAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolAddress"].ToString();
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolUDISENo"].ToString() != string.Empty)
                txtPreviousSchoolUDISENo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolUDISENo"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedStd"].ToString() != string.Empty)
                txtLastStd.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedStd"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedBoard"].ToString() != string.Empty)
            {
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedBoard"].ToString() == "OTHERS")
                    rdolstlastSchoolBoard.SelectedValue = "OTHERS";
                else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedBoard"].ToString() == "ICSE")
                    rdolstlastSchoolBoard.SelectedValue = "ICSE";
                else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedBoard"].ToString() == "CBSE")
                    rdolstlastSchoolBoard.SelectedValue = "CBSE";
                else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastCompletedBoard"].ToString() == "SSC")
                    rdolstlastSchoolBoard.SelectedValue = "SSC";
            }            

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Category_Id"].ToString() != string.Empty)
            {
                cmbCategory.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Category_Id"].ToString();
            }
           

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["AadharCardNo"].ToString() != string.Empty)
                txtAadharCardNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["AadharCardNo"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["NameAsPerAadharCard"].ToString() != string.Empty)
                txtNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["NameAsPerAadharCard"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Sex"].ToString() != string.Empty)
            {
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Sex"].ToString() == "M")
                    rdoMale.Checked = true;
                else
                    rdoFemale.Checked = true;
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows.Count > 0)
            {
                txtMSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Last_Name"].ToString();
                txtMName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["First_Name"].ToString();
                txtMHName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Middle_Name"].ToString();
                txtMAge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Age"].ToString() == Constants.S_ZERO ? string.Empty : oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Age"].ToString();
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["SaralNo"].ToString() != string.Empty)
                txtSaralNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["SaralNo"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LanguageKnown"].ToString() != string.Empty)
                txtLanguageKnown.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LanguageKnown"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["EmergancyContact"].ToString() != string.Empty)
                txtEmergancyContact.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["EmergancyContact"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PersonToBeContacted"].ToString() != string.Empty)
                txtPersonToContacted.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PersonToBeContacted"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Relationship"].ToString() != string.Empty)
                txtRelationship.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Relationship"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["OnlyChild"].ToString() != string.Empty)
                rdoOnlyChild.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["OnlyChild"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Minority"].ToString() != string.Empty)
                rdoMinority.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Minority"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows.Count > 0)
            {
                txtHouseNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["House_Plot_Name"].ToString();
                txtLandmark.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["LandMark"].ToString();
                txtSubArea.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["SubAreaName"].ToString();
                txtMainArea.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["MainAreaName"].ToString();
                txtmOffcAddr.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["Mother_Offc_Addr"].ToString();
                txtfoffcAddr.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["Father_Offc_Addr"].ToString();
                txttaluka.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["Taluka"].ToString();
                txtDistrict.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["District"].ToString();
                txtBirthTaluka.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["BirthTaluka"].ToString();
                txtBirthDistrict.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["BirthDistrict"].ToString();
                txtBirthState.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["BirthState"].ToString();
                txtBirthCountry.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_ADMISSION_ADDITONAL_DETAILS].Rows[0]["BirthCountry"].ToString();
            }
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows.Count > Constants.I_ZERO)
            {
                chkConsultation.Checked = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["VisionConclusion"].ToBool();
                chkSpectacles.Checked = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["UseofSpectacles"].ToBool();
                chkDifficulty.Checked = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["AnyDifficulty"].ToBool();
                chkHearinConclusion.Checked = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["HearingConclusion"].ToBool();
                txtMedication.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["MedicationTakenForGeneral"].ToString();
                txtSNSAllergy.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_HEALTH_DETAILS].Rows[0]["AnyAllergy"].ToString();
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows.Count > Constants.I_ZERO)
            {
                txt10Board.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows[0]["BoardName"].ToString();
                txt10RollNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows[0]["BoardRollNo"].ToString();
                txt10Exam.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows[0]["ExamName"].ToString();
                txt10PassingYear.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows[0]["YearOfPassing"].ToString();
                txt10thMaths.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_10th_STD_MARK_DETAILS].Rows[0]["BasicStandardMaths"].ToString();
            }
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt() && (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows.Count > Constants.I_ZERO))
        {
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FinanciallyResposibleFor"].ToString() == "1")
                rdoFRFather.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FinanciallyResposibleFor"].ToString() == "2")
                rdoFRMother.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FinanciallyResposibleFor"].ToString() == "3")
                rdoFRGuardian.Checked = true;

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "2")
                rdoFather.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "3")
                rdoMother.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "4")
                rdoLocalGuardian.Checked = true;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() && oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows.Count > Constants.I_ZERO)
        {
            txtPassportNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PassportNo"].ToString();
            txtDateOfExpiry.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["DateOfPassportExpiry"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
            txtMarriageAnniversary.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["MarriageAnniversaryDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
            txtFamilyIncome.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FamilyIncome"].ToString();
            txtLastSchoolPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LastSchoolPhoneNo"].ToString();

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["IsAdoptedChild"].ToString().ToBool() == true)
                chkIsAdoptedChild.Checked = true;
            else
                chkIsAdoptedChild.Checked = false;

            //txtFinancialResponsible.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FinanciallyResposibleFor"].ToString();

            //if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "1")
            //    rdoBothParent.Checked = true;
            //else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "2")
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "2")
                rdoFather.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "3")
                rdoMother.Checked = true;
            else if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["StudentLivingWith"].ToString() == "4")
                rdoLocalGuardian.Checked = true;

            txtPermanentAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PermanentAddress"].ToString();
            txtFirstPersonalMark.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["FirstPersonalMark"].ToString();
            txtSecondPersonalMark.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["SecondPersonalMark"].ToString();
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
        {
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows.Count > 0 && oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["IsForDayBoarding"].ToString().ToBool() == true)
                chkIsForDayBoarding.Checked = true;
            else
                chkIsForDayBoarding.Checked = false;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() ||
            ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt() ||
            ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt()) 

        {
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows.Count > Constants.I_ZERO)
            {
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["IsSchoolFromOutOfState"].ToInt() == Constants.I_ONE)
                    chkIsSchoolFromOutOfState.Checked = true;

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PreviousSchoolSaralId"].ToString() != string.Empty)
                    txtPreviousSchoolSaralId.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["PreviousSchoolSaralId"].ToString();
            }
        }

    }

    private void SetLanguageValidations()
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        if (iSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");
            List<string> lstStds = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string sStdName;
            if (cmbStd.Visible)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            if (lstStds.Contains(sStdName))
            {
                cmbSecondSLanguageSubjectId.BackColor = oBackColor;
                cmbThirdLanguage.BackColor = oBackColor;

                cstValThirdLanguage.Enabled = true;
                reqValSecondLanguage.Enabled = true;
                reqValThirdLanguage.Enabled = true;

                txtPenNo.BackColor = oBackColor;
                reqValPenNo.Enabled = true;
            }

            if(sStdName == "Nursery")
            {
                cmbPreferenceBatch.BackColor = oBackColor;
                reqValPrfBatch.Enabled = true;
            }
        }
    }

    private void GetMinMaxDate(DataTable aOdtDatatable, int aiStandardId)
    {
        if (aOdtDatatable != null)
        {
            if (aOdtDatatable.Rows.Count > 0)
            {
                var dicMinMaxDOBMap = new Dictionary<string, object>();

                foreach (DataRow row in aOdtDatatable.Rows)
                                                                                                                                                                                                                                                                                                                                                     {
                    dicMinMaxDOBMap.Add(row["Standard_Id"].ToString(),
                                        new
                                        {
                                            min = row.IsNull("DOBMin") ? String.Empty : row["DOBMin"].ToDateTime().ToString("dd-MMM-yyyy"),
                                            max = row.IsNull("DOBMax") ? String.Empty : row["DOBMax"].ToDateTime().ToString("dd-MMM-yyyy")
                                        });
                }

                if (dicMinMaxDOBMap.Count > 0)
                {
                    var jsSerializer = new JavaScriptSerializer();
                    hidMinMaxDOBMap.Value = String.Format("[{0}]", jsSerializer.Serialize(dicMinMaxDOBMap));
                }


                if (aiStandardId != 0)
                {

                    DataRow[] oDrDate = aOdtDatatable.Select("Standard_Id=" + aiStandardId) as DataRow[];
                    if (oDrDate.Length > 0)
                    {
                        if (!oDrDate[0].IsNull("DOBMax"))
                            hidMaxBdate.Value = Convert.ToDateTime(oDrDate[0].ItemArray[2]).ToString("dd-MMM-yyyy");
                        else
                            hidMaxBdate.Value = string.Empty;
                        if (!oDrDate[0].IsNull("DOBMin"))
                            hidMinBdate.Value = Convert.ToDateTime(oDrDate[0].ItemArray[3]).ToString("dd-MMM-yyyy");
                        else
                            hidMinBdate.Value = string.Empty;
                    
                        if (!oDrDate[0].IsNull("RemainingformsCount"))
                            Session["RemainingformsCount"] = oDrDate[0]["RemainingformsCount"].ToInt();
                    }
                    miStandardId = aiStandardId;
                    msAmount = oDrDate[0].ItemArray[4].ToString();

                    hidAmount.Value = oDrDate[0].ItemArray[4].ToString();
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetDefaultValuesToControls()
    {   
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.OWS.ToInt())
            txtEmail.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPSN.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SNS.ToInt())
            txtCity.Text = Constants.S_DEFAULT_CITY;

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPSN.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SNS.ToInt())
            txtState.Text = Constants.S_DEFAULT_STATE;

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPSN.ToInt())
            txtNationality.Text = Constants.S_DEFAULT_NATIONALITY;
        else
            txtNationality.Text = string.Empty;

        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidServerDt.Value = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtSLastName.Focus();
        
        if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null && moUserRole != Constants.UserRoles.Student)
            hidStudentAdmisssionID.Value = Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToString();
        else
            hidStudentAdmisssionID.Value = "0";

        if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && (msFromUrl.Equals("NewStudentAdmisionsListUI.aspx") || msFromUrl.Equals("AdmissionFormParentDetails.aspx")))
        {
            divAdmissionSteps.Visible = false;
            cmbStd.Visible = true;
            cmbStd.Enabled = true;
            cmbStd.Focus();
            lblStdName.Visible = false;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            trSPSResponsible.Visible = true;
            trSPSLivingWith.Visible = true;
            trResidenceType.Visible = false;
            trResidenceTypeHeader.Visible = false;
            trLivingLocation.Visible = false;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
        {
            trSPSPassport.Visible = true;
            trSPSMarriageAnniversary.Visible = true;
            trSPSAdopted.Visible = true;
            trSPSResponsible.Visible = true;
            trSPSLivingWith.Visible = true;
            trSPSPermanentAddress.Visible = true;
            trSPSBirthDetails.Visible = true;
            trPersonalMarks.Visible = true;
            trFirstPersonalMarks.Visible = true;            


            lblAddress.Text = "Present Address : ";

            trResidenceType.Visible = false;
            trResidenceTypeHeader.Visible = false;
            trLivingLocation.Visible = false;
            trPincode.Visible = false;
            tdCity.Visible = false;
            tdtxtCity.Visible = false;
            tdMobileNo.Visible = false;
            tdMobileNo2.Visible = false;
            tdtxtMobileNo.Visible = false;
            tdtxtMobileNo2.Visible = false;
            trLastSchoolBoard.Visible = false;
            trRecognised.Visible = false;
            trPreviousStandard.Visible = false;
            trSPSEmpty.Visible = true;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
            lblResidenceType.Text = "Preference:";

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {
            reqValCaste.Enabled = true;
            reqValFLastName.Enabled = true;
            reqValmLastName.Enabled = true;
            reqValMotherTongue.Enabled = true;
            reqValNationality.Enabled = true;
            reqValStudLastName.Enabled = true;

            txtCasteAndSubcaste.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtFSurname.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtMSurname.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtMotherTongue.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtNationality.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtSLastName.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            txtMobile2.BackColor = System.Drawing.Color.FromName("#FFFFA0");

            tdSaralNoH.Visible = false;
            tdSaralNoData.Visible = false;
            tdLastSchoolStudSaralId.Visible = false;
            tdLastSchoolStudSaralIddata.Visible = false;
            lblLastSchoolStudSaralId.Text = "Previous School Student Saral Id:";

            trResidenceTypeHeader.Visible = false;
            trResidenceType.Visible = false;
            tdAadharData.Visible = false;
            tdAadharHeader.Visible = false;
            trAadharNote.Visible = false;
            cstValidateFileUpload.Enabled = false;
            //trAadharNameNote.Visible = false;
            //trFatherAadharName.Visible = false;
            //trMotherAadharName.Visible = false;

            spnAadharNameNote.InnerText = "(Student name as per Birth Certificate)";
            spnFatherAadharName.InnerText = "---------------------------- As per student Birth Certificate ----------------------------";
            spnMotherAadharName.InnerText = "---------------------------- As per student Birth Certificate ----------------------------";
        }
    }

    private void ReadQueryStr()
    {
        if (QueryString["StudetAdmissionId"] != null)
        {
            int iStudentAdmissionId = QueryString["StudetAdmissionId"].ToInt();
            Session.Add(Constants.S_SESSION_STUDENT_ADMISSION_ID, iStudentAdmissionId);
            hidIsEditMode.Value = Constants.S_YES;            
        }       
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmAction()) return false;");
        hidPPSSchoolId.Value = Constants.SchoolId.PPS.ToInt().ToString();
        hidSNSSchoolId.Value = Constants.SchoolId.SNS.ToInt().ToString();
        hidOWSSchoolId.Value = Constants.SchoolId.OWS.ToInt().ToString();
        hidZLSPSchoolId.Value = Constants.SchoolId.ZLSP.ToInt().ToString();
        hidSchoolIdBFS.Value = Constants.SchoolId.BFS.ToInt().ToString();       
        
        cmbLivingLocation.Attributes.Add("onchange", "SetVisibilityOfLocationTxt(this)");

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            hidShowFullNameValidation.Value = Constants.S_YES;

            string sStandardName = string.Empty;
            if (cmbStd.Visible)
                sStandardName = cmbStd.SelectedItem.Text;
            else
                sStandardName = lblStdName.Text;
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private int ReadQueryString()
    {
		
        if (QueryString.Count > 0 && QueryString["StandardId"] != null)
            return QueryString["StandardId"].ToInt();
	    
		return 0;
    }
        
    /// <summary>
    /// This method is used to save student details.
    /// </summary>
    private string SaveStudentsDetails()
    {
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

        string sLinkName, sBirthCertificateLinkName = string.Empty;
        string sFileUploadErr = CheckIsFileUploaded(out sLinkName, FilUpImg, S_FOLDER_LOCATION);

        if (sFileUploadErr == string.Empty)
            sFileUploadErr = CheckIsFileUploaded(out sBirthCertificateLinkName, flUploadBirthCertificate, S_FOLDER_BIRTH_CERTIFICATE);

        ValidateStudentPhoto();

        if (sFileUploadErr == string.Empty)
        {
            if (iSchoolId == Constants.SchoolId.DSK.ToInt())
                colpnlHealthDetails.Visible = true;
            else
                colpnlHealthDetails.Visible = false;

            if (iSchoolId == Constants.SchoolId.SNS.ToInt())
                colpnlStudentAdditionalDetails.Visible = true;
            else
                colpnlStudentAdditionalDetails.Visible = false;

            int iStudentAdmissionId = 0;
            string sAdmissionXML = GetAdmissionXML(sLinkName, sBirthCertificateLinkName);
            string sAdmissionPArentDetailsXML = GetAdmissionParentDtlsXML();
            string sAdmissionAdditionalDetailsXML = GetAdmissionAddtnlDtlsXML();            
            string sAdmissionHealthDetailsXML = null;
            string sStudent10thStdDetails = null;
            //if (iSchoolId == Constants.SchoolId.DSK.ToInt() || iSchoolId == Constants.SchoolId.SNS.ToInt())
            if (iSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                sAdmissionHealthDetailsXML = GetStudentHealthDetailsXML();
                sStudent10thStdDetails = Get10thStdDetailsXML();
            }

            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
            oStudentAdmissionsBL.AdmissionDetails = sAdmissionXML;
            oStudentAdmissionsBL.AdmissionParentDetails = sAdmissionPArentDetailsXML;
            oStudentAdmissionsBL.AdmissionAdditionalDetails = sAdmissionAdditionalDetailsXML;
            oStudentAdmissionsBL.AmissionHealthDetails = sAdmissionHealthDetailsXML;
            oStudentAdmissionsBL.StudentAdmission10thStandardDetails = sStudent10thStdDetails;

            oStudentAdmissionsBL.IsOnlineAdmission = sIsOnline == "Y" ? true : false;
            bool IsEditMode = false;
            int iStdAdmissionId = Constants.I_ZERO;

            if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null && moUserRole != Constants.UserRoles.Student && QueryString["IsEditMode"] != null && QueryString["IsEditMode"].ToString() == Constants.S_ONE)
            {
                IsEditMode = true;
                iStdAdmissionId = Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToInt();                
            }

            byte[] oStudentPhoto = GetByteArrayFromFileField(flStudentPhoto);

            DataTable oDTStudentDetail = oStudentAdmissionsBL.InsertStudentAdmissions(iSchoolId, hidEnquiryId.Value.ToInt(), IsEditMode, iStdAdmissionId, oStudentPhoto);

            if (oDTStudentDetail != null && oDTStudentDetail.Rows.Count > 0)
            {
                iStudentAdmissionId = System.Convert.ToInt32(oDTStudentDetail.Rows[0][0]);
                //saves new student admission id in session
                Session.Add(Constants.S_SESSION_STUDENT_ADMISSION_ID, iStudentAdmissionId);
            }
            return string.Empty;
        }
        else        
            return sFileUploadErr;        
    }

    private string CheckIsFileUploaded(out string asFileName, FileUpload aoFileUpload, string asFolderPath)
    {
        asFileName = string.Empty;
        if (aoFileUpload.FileName != string.Empty)
        {   
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(aoFileUpload.FileName.ToString());
            if (aoFileUpload.HasFile)
            {
                string sFileName = aoFileUpload.PostedFile.FileName;
                string sFileExtention = System.IO.Path.GetExtension(sFileName);
                string sFileMimeType = aoFileUpload.PostedFile.ContentType;
                int iFileLengthinKb = aoFileUpload.PostedFile.ContentLength / I_FILE_SIZE_LIMIT;

                List<string> lstmatchExtention = new List<string>();
                lstmatchExtention.Add(".jpg");lstmatchExtention.Add(".png");
                lstmatchExtention.Add(".bmp");lstmatchExtention.Add(".jpeg");
                lstmatchExtention.Add(".JPG");lstmatchExtention.Add(".PNG");
                lstmatchExtention.Add(".BMP");lstmatchExtention.Add(".JPEG");
                lstmatchExtention.Add(".pdf"); lstmatchExtention.Add(".PDF");
                List<string> lstmatchMimeType = new List<string>();
                lstmatchMimeType.Add("image/jpg");lstmatchMimeType.Add("image/png");
                lstmatchMimeType.Add("image/bmp");lstmatchMimeType.Add("image/jpeg");
                lstmatchMimeType.Add("iimage/JPG");lstmatchMimeType.Add("image/PNG");
                lstmatchMimeType.Add("image/BMP");lstmatchMimeType.Add("image/JPEG");
                lstmatchMimeType.Add("application/pdf"); lstmatchMimeType.Add("application/PDF");

                if (lstmatchExtention.Contains(sFileExtention) && lstmatchMimeType.Contains(sFileMimeType))
                {
                    if (aoFileUpload.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkPath = sServerPath + asFolderPath + sLinkName;
                        aoFileUpload.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
                else
                    sReturnErrorMsg = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
            }
            return sReturnErrorMsg;
        }        
        return string.Empty;
    }

    private string GetAdmissionAddtnlDtlsXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissionAdditionalDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionAdditionalDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionAdditionalDetail", "");
      
        string sAtrrName = "House_PlotName";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtHouseNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Acedemic_Year_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidAcademicYearId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Landmark";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLandmark.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SubArea_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSubArea.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MainArea_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMainArea.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Office_Addr";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtmOffcAddr.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Office_Addr";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtfoffcAddr.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Taluka";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txttaluka.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "District";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtDistrict.Text;
        oXmlNode.Attributes.Append(attr);

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
        {
            sAtrrName = "BirthTaluka";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtSPSBirthTaluka.Text.Trim();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "BirthDistrict";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtSPSBirthDistrict.Text.Trim();
            oXmlNode.Attributes.Append(attr);
        }
        else
        {
            sAtrrName = "BirthTaluka";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtBirthTaluka.Text;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "BirthDistrict";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtBirthDistrict.Text;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "BirthState";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtBirthState.Text;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "BirthCountry";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtBirthCountry.Text;
            oXmlNode.Attributes.Append(attr);
        }

        sAtrrName = "FeeAreaId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = GetFeeArea(hidEnquiryId.Value.ToInt()).ToString();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    private string GetStudentHealthDetailsXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentHealthDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentHealthDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentHealthDetails", "");

        string sAtrrName = "VisionConclusion";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkConsultation.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "VisionLenses";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkSpectacles.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "HearingDifficulty";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkDifficulty.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "HearingConclussion";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkHearinConclusion.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MedicationTaken";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMedication.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Allergy";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSNSAllergy.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        //string sAtrrName = "InoculationGiven";
        //XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        //var svalue = 0;
        //if (rdoYes.Checked)
        //    svalue = 1;
        //if (rdoNo.Checked)
        //    svalue = 0;
        //attr.Value = svalue.ToString();
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "BloodGroup";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = cmbBloodGroup.SelectedValue;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Vaccination1";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txt1.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Vaccination2";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtii.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Vaccination3";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtiii.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "VaccinationBooster";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtBooster.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Ailnment";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtAilment.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Allergies";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtAllergies.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "FamilyDoctor";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtFamilyDoc.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "ClinicPhone";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtClinic.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "DocMobileNo";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtDocMobile.Text;
        //oXmlNode.Attributes.Append(attr);

        //sAtrrName = "EmergancyConNo";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtCoNoInEmergancy.Text;
        //oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.

        return root.InnerXml;
    }

    private string Get10thStdDetailsXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("Student10thStandardDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Student10thStandardDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Student10thStandardDetails", "");

        string sAtrrName = "BoardName";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txt10Board.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BoardRollNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txt10RollNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BoardExamName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txt10Exam.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BoardYearOfPassing";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txt10PassingYear.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BoardMathematics";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txt10thMaths.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.

        return root.InnerXml;
    }

    private int GetFeeArea(int? miEnquiryId)
    {
        return SchoolEnquiryBL.GetFeeArea(miEnquiryId);
    }

    /// <summary>
    /// This method is used to generate XML format for student admission details.
    /// </summary>
    /// <returns></returns>
    private string GetAdmissionXML(string sLinkName, string asBirthCertificateLinkName)
    {
        const char C_FEMALE = 'F';
        const char C_MALE = 'M';
        const int I_MASTER = 5;
        const int I_MISS = 6;
        const string S_ELEMENT = "element";

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissions");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissions", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmission", "");

        // Student Details
        string sAtrrName = "School_Id";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = ConfigurationManager.AppSettings["SchoolID"];
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Acedemic_Year_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidAcademicYearId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Form_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "For_Standard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbStd.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFahterName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Salutation_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoMale.Checked ? I_MASTER.ToString() : I_MISS.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Sex";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoMale.Checked ? C_MALE.ToString() : C_FEMALE.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Tongue";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherTongue.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DOB";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCalDobPopup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DOBInText";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = CommonUtility.GetDateInWords(Convert.ToDateTime(txtCalDobPopup.Text));
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BirthPlace";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBirthPlace.Text;
        oXmlNode.Attributes.Append(attr);       

        sAtrrName = "Nationality";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtNationality.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Religion";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbReligion.SelectedValue.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Blood_Group";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbStudentBloodGroup.SelectedValue.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PenNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPenNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "ApaarId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtApaarId.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Subject_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbSecondSLanguageSubjectId.SelectedValue.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "ThirdLanguageSubjectId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbThirdLanguage.SelectedValue;
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "Caste_Subcaste";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCasteAndSubcaste.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CasteCertScanCopy";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = GetCastCertFileName();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FatherAadharCardScanCopy";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = GetFatherAadharScanCopyFileName();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MotherAadharCardScanCopy";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = GetMotherAadharScanCopyFileName();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAadharCardNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "NameAsPerAadharCard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtNameOnAadharCard.Text;
        oXmlNode.Attributes.Append(attr);

        if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null && moUserRole != Constants.UserRoles.Student)
        {
            sLinkName = hidAadharCardScanCopy.Value;
            asBirthCertificateLinkName = hidBirthCertificateScanCopy.Value;
        }

        sAtrrName = "AadharCardScanCopy";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = sLinkName;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BirthCertificateScanCopyFileName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = asBirthCertificateLinkName;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PreferenceBatchId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbPreferenceBatch.SelectedValue;
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "Category_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbCategory.SelectedValue.ToString();
        oXmlNode.Attributes.Append(attr);

        // Last School Details
        sAtrrName = "Last_school_name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSchoolName.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastSchoolAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPreviousSchoolAddress.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastSchoolUDISENo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPreviousSchoolUDISENo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Last_Completed_Std";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLastStd.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Last_Completed_Board";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdolstlastSchoolBoard.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Is_Recognised_Board";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdolstIsRecognised.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        // Address Details 
        sAtrrName = "Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAddress.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "City";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCity.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Pincode";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPincode.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "State";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtState.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Residence_Phone_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtRPhone.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Office_Phone_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtOPhone.Text;
        oXmlNode.Attributes.Append(attr);

        if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
        {
            sAtrrName = "Mobile_Number1";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtMobile.Text;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "Mobile_Number2";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtMobile2.Text;
            oXmlNode.Attributes.Append(attr);
        }
        else
        {
            sAtrrName = "Mobile_Number1";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtMobile2.Text;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "Mobile_Number2";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtMobile.Text;
            oXmlNode.Attributes.Append(attr);
        }

        sAtrrName = "Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Guardian_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Guardian_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFFatherName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Guardian_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFSurname.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Guardian_Age";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFAge.Text;
        oXmlNode.Attributes.Append(attr);

         sAtrrName = "Sibling_Student_Admission_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidStudentAdmisssionID.Value;
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "ResidenceTypeId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbResidenceType.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LivingLocationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbLivingLocation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LivingLocationName";
        attr = oDoc.CreateAttribute(sAtrrName);
        if (hidSchoolId.Value.ToInt() == Constants.SchoolId.PPS.ToInt())
            attr.Value = txtLivingLocation.Text;
        else            
            attr.Value = string.Empty;

        sAtrrName = "PassPortNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPassportNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DateOfPassportExpiry";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtDateOfExpiry.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MarriageAnniversary";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMarriageAnniversary.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FamilyIncome";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFamilyIncome.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastSchoolPhoneNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLastSchoolPhone.Text.Trim();
        oXmlNode.Attributes.Append(attr);
       

        sAtrrName = "IsStudentAdopted";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkIsAdoptedChild.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        //sAtrrName = "FinanciallyResponsibleFor";
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtFinancialResponsible.Text.Trim();
        //oXmlNode.Attributes.Append(attr);


        int iFinanciallyResp = Constants.I_ZERO;

        if (rdoFRFather.Checked)
            iFinanciallyResp = 1;
        else if (rdoFRMother.Checked)
            iFinanciallyResp = 2;
        else if (rdoFRGuardian.Checked)
            iFinanciallyResp = 3;

        sAtrrName = "FinanciallyResponsibleFor";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iFinanciallyResp.ToString();
        oXmlNode.Attributes.Append(attr);

        string sStudentLivingWith = Constants.S_ZERO;

        //if (rdoBothParent.Checked)
        //    sStudentLivingWith = Constants.S_ONE;
        if(rdoFather.Checked)
            sStudentLivingWith = Constants.S_TWO;
        else if(rdoMother.Checked)
            sStudentLivingWith = "3";
        else if(rdoLocalGuardian.Checked)
            sStudentLivingWith = "4";

        sAtrrName = "StudentLivingWith";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = sStudentLivingWith;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PermanentAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPermanentAddress.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FirstPersonalMark";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFirstPersonalMark.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SecondPersonalMark";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSecondPersonalMark.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IsForDayBoarding";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = chkIsForDayBoarding.Checked.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IsSchoolFromOutOfState";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Convert.ToString(chkIsSchoolFromOutOfState.Checked ? 1 : 0);
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PreviousSchoolSaralId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPreviousSchoolSaralId.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SaralNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSaralNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LanguageKnown";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLanguageKnown.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OnlyChild";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoOnlyChild.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Minority";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoMinority.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "EmergencyContact";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEmergancyContact.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PersonToBeContacted";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPersonToContacted.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Relationship";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtRelationship.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        oXmlNode.Attributes.Append(attr);
        
        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    private string GetCastCertFileName()
    {
        if (flUploadCastCert.HasFile)
        {
            string sFileName = flUploadCastCert.FileName;
            string sServerPath = Server.MapPath("~");
            string sPath = sServerPath + "\\RITeSchool\\Downloads\\Admission\\CasteCertificate\\" + sFileName;

            if (File.Exists(sPath))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                sPath = sServerPath + "\\RITeSchool\\Downloads\\Admission\\CasteCertificate\\" + sFileName;
            }

            flUploadCastCert.SaveAs(sPath);
            return sFileName;
        }
        else
        {
            return hidCasteCertFileName.Value;
        }
    }

    private string GetFatherAadharScanCopyFileName()
    {
        if (flUploadFatherAaadhar.HasFile)
        {
            string sFileName = flUploadFatherAaadhar.FileName;
            string sServerPath = Server.MapPath("~");
            string sPath = sServerPath + "\\RITeSchool\\Downloads\\ParentAadharCards\\" + sFileName;

            if (File.Exists(sPath))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                sPath = sServerPath + "\\RITeSchool\\Downloads\\ParentAadharCards\\" + sFileName;
            }

            flUploadFatherAaadhar.SaveAs(sPath);
            return sFileName;
        }
        else
        {
            return hidFatherAadharCardFileName.Value;
        }
    }

    private string GetMotherAadharScanCopyFileName()
    {
        int fileCount = Request.Files.Count;

        if (flUploadMotherAaadhar.HasFile)
        {
            string sFileName = flUploadMotherAaadhar.FileName;
            string sServerPath = Server.MapPath("~");
            string sPath = sServerPath + "\\RITeSchool\\Downloads\\ParentAadharCards\\" + sFileName;

            if (File.Exists(sPath))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                sPath = sServerPath + "\\RITeSchool\\Downloads\\ParentAadharCards\\" + sFileName;
            }

            flUploadMotherAaadhar.SaveAs(sPath);
            return sFileName;
        }
        else
        {
            return hidMotherAadharCardFileName.Value;
        }
    }

    /// <summary>
    /// This method is used to generate XML format for mother details.
    /// </summary>
    /// <returns></returns>
    private string GetAdmissionParentDtlsXML()
    {
        const char C_MOTHER = 'M';
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissionParentDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionParentDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionParentDetail", "");

        string sAtrrName = "Father_Or_Mother";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = C_MOTHER.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMHName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMSurname.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Age";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMAge.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidMOccupationId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MobileNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidMMobileNo.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Tongue";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherTongue.Text;
        oXmlNode.Attributes.Append(attr);////-------------------------

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to get previous page name.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
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
    /// This method s used to set validation state.
    /// </summary>
    private void SetValidationState()
    {
        hidShowLastSchoolValidation.Value = Constants.S_ZERO;
        hidShowUDISEValidation.Value = Constants.S_NO;
        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {
            List<string> lstStds = new List<string> {"2","3","4", "5", "6", "7", "8", "9", "10" };
            string sStdName = string.Empty;
            if (cmbStd.Visible)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            if (lstStds.Contains(sStdName))
                hidShowLastSchoolValidation.Value = Constants.S_ONE;

            if (sStdName == "7" || sStdName == "8" || sStdName == "9")
            {
                cstValidateFileUpload.Enabled = false;
            }
        }

        else if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.BFS.ToInt())
        {
            List<string> lstStds = new List<string> { "Junior KG","Senior KG","1","2","3","4", "5", "6", "7", "8", "9", "10" };
            string sStdName = string.Empty;
            if (cmbStd.Visible)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            if (lstStds.Contains(sStdName))
                hidShowLastSchoolValidation.Value = Constants.S_ONE;

            hidShowUDISEValidation.Value = Constants.S_NO;

            List<string> lstStandards = new List<string> {"2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string sStandardName = string.Empty;
            if (cmbStd.Visible)
                sStandardName = cmbStd.SelectedItem.Text;
            else
                sStandardName = lblStdName.Text;

            if(lstStandards.Contains(sStandardName))
                hidShowUDISEValidation.Value = Constants.S_YES;
        }
        else if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSN.ToInt())
        {
            string sStdName = string.Empty;
            List<string> lstStds1 = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10" };            
            if (cmbStd.Visible)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            if (lstStds1.Contains(sStdName))
            {
                hidShowLastSchoolValidation.Value = Constants.S_ONE;

                System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");
                txtPenNo.BackColor = oBackColor;
                reqValPenNo.Enabled = true;
            }
               
            //hidShowUDISEValidation.Value = Constants.S_YES;

            if (sStdName.ToLower() == "nursery")
                hidShowLastStdValidation.Value = Constants.S_NO;
            else
            {
                hidShowLastStdValidation.Value = Constants.S_YES;
                txtLastStd.BackColor = System.Drawing.Color.FromName("#FFFFA0");
                txtPreviousSchoolAddress.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            }

            tdLastSchoolStudSaralId.Visible = false;
            tdLastSchoolStudSaralIddata.Visible = false;            
        }
        else if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
        {
            List<string> lstStds = new List<string> { "Junior KG", "Senior KG", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            
            string sStdName = string.Empty;
            if (cmbStd.Visible)
                sStdName = cmbStd.SelectedItem.Text;
            else
                sStdName = lblStdName.Text;

            if (lstStds.Contains(sStdName))
                hidShowLastSchoolValidation.Value = Constants.S_ONE;

            List<string> lstUDISEStds = new List<string> {"2", "3", "4", "5", "6", "7", "8", "9", "10" };
            if (lstUDISEStds.Contains(sStdName))
            {
                hidShowUDISEValidation.Value = Constants.S_YES;
            }
            else
            {
                cstPreviousSchoolSaral.Enabled = false;
                cstSchoolUDISE.Enabled = false;
                CustomValidator6.Enabled = false;
            }
        }
    }

    private void SetCurrentDate()
    {
        if (SchoolBase.Settings.CompareAgeTillDate.ToString() != string.Empty)
        {
            //var dt = SchoolBase.Settings.CompareAgeTillDate.ToDateTime();
            //DateTime newDT = new DateTime(DateTime.Now.Year,dt.Month,dt.Day);
            //if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
            //    hidCurrentDate.Value = "2023/12/31";
            //else

            SchoolBL oSchoolBL = new SchoolBL();
            Dictionary<int, YearwiseSchoolSettings> dictAllAcademicYearSettings = oSchoolBL.GetSchoolSettings(ConfigurationManager.AppSettings["SchoolID"].ToInt());

            YearwiseSchoolSettings oYearwiseSchoolSettings = dictAllAcademicYearSettings[hidAcademicYearId.Value.ToInt()];
            if (oYearwiseSchoolSettings != null)
                hidCurrentDate.Value = oYearwiseSchoolSettings.CompareAgeTillDate.ToString();
            else
                hidCurrentDate.Value = Settings.CompareAgeTillDate.ToString();
        }
        else
            hidCurrentDate.Value = Convert.ToString(DateTime.Now);
    }

    private void Set10StdStudentDetails()
    {
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            EmergancyDetails.Visible = true;
            List<string> lstStandards = new List<string> { "11 Sci", "12 Sci", "11 Com", "12 Com", "11 Art", "12 Art" };

            string sStandardName = string.Empty;
            if (cmbStd.Visible)
                sStandardName = cmbStd.SelectedItem.Text;
            else
                sStandardName = lblStdName.Text;

            if (lstStandards.Contains(sStandardName))
            {
                trSNS10thStdDetails.Visible = true;
                trLastSchoolBoard.Visible = false;
                hidShow10thStdValidation.Value = Constants.S_YES;
            }
            else
            {
                trSNS10thStdDetails.Visible = false;
                hidShow10thStdValidation.Value = Constants.S_NO;
            }
        }
        else
            EmergancyDetails.Visible = false;
    }

    protected void ValidateStudentPhoto()
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
        {
            string sMessage = ValidatePhotoFile();
            if (sMessage != string.Empty)
            {
                throw new ApplicationException(sMessage);
            }
        }
    }

    private string ValidatePhotoFile()
    {
        int I_HEIGHT_LIMIT = 151;
        int I_WIDTH_LIMIT = 112;
        string sReturnErrorMsg = "";

       // asFileName = string.Empty;

        if (flStudentPhoto.HasFile)
        {
            string sServerPath = Server.MapPath("~");
            string sFileName = flStudentPhoto.FileName;
            string sPath = sServerPath + "\\RITeSchool\\Downloads\\Admission\\StudentPhoto\\" + sFileName;

            if (File.Exists(sPath))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(flStudentPhoto.FileName);
            }

            sServerPath = sServerPath + "\\RITeSchool\\Downloads\\Admission\\StudentPhoto\\" + sFileName;
            
           // asFileName = sFileName;
            
            flStudentPhoto.SaveAs(sServerPath);

            if (File.Exists(sServerPath))
            {
                FileStream oFileStream = new FileStream(sServerPath, FileMode.Open);
                System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
                if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px " + Resources.LocalizedResources.And + " " + I_WIDTH_LIMIT + "px " + Resources.LocalizedResources.respectively;
                }
                else
                {
                    if (oImg.Height > I_HEIGHT_LIMIT)
                    {
                        sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    }
                    if (oImg.Width > I_WIDTH_LIMIT)
                    {
                        sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    }
                }
                oFileStream.Close();
                oImg = null;

            }

            if (File.Exists(sServerPath))
                File.Delete(sServerPath);
        }

        return sReturnErrorMsg;
    }

    #endregion " Private Methods "	        

}
