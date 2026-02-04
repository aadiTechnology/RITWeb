// File Name  : AdmissionFormStudentDetails.aspx.cs
// Created By : Amit 
// Date       : 17/11/2009
// Description: This class is used to fill student details on admission form.
//              This screen will be used in Online as well as Manual Admission process.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class EnquiryForm : SchoolBase
{
    #region " Constants "

    static string msFromUrl = string.Empty;
    const string S_SCREENS_URL = "NewStudentAdmisionsListUI.aspx";
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";   

    #endregion " Constants "

    #region " Propertie's "

    private TextBox TxtCLastName
    {
        get
        {
            if ( ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                return txtSPSLastName;
            else
                return txtSLastName;
        }
        set
        {
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                txtSPSLastName = value;
            else
                txtSLastName = value;
        }
    }

    private TextBox TxtFLastName
    {
        get
        {
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                return txtSPSFLastName;
            else
                return txtFSurname;
        }
        set
        {
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                txtSPSFLastName = value;
            else
                txtFSurname = value;
        }
    }

    private TextBox TxtMLastName
    {
        get
        {
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                return txtSPSMLastName;
            else
                return txtMSurname;
        }
        set
        {
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                txtSPSMLastName = value;
            else
                txtMSurname = value;
        }
    }

    #endregion " Propertie's "

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

            //if (!IsPostBack)
            //    msFromUrl = GetFromPageUrl();

            //if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && (msFromUrl.Equals("NewStudentAdmisionsListUI.aspx") || msFromUrl.Equals("AdmissionFormParentDetails.aspx")))
            //    this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            //else if (msFromUrl == S_SCREENS_URL)
            //    Response.Redirect("../Common/Error.aspx", true);
            //else
            //{
            //    if (msFromUrl.Equals("EnquiryForm.aspx"))
            //        this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            //    else
            //    {   
            //        this.Page.MasterPageFile = "~/RITeSchool/MasterPages/OnlineAdmission.master";
            //    }
            //}

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            if ((moUserRole == Constants.UserRoles.Admin || msFromUrl == S_SCREENS_URL))
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            //else if (msFromUrl == S_SCREENS_URL)
            //    Response.Redirect("../Common/Error.aspx", true);

            if (!msFromUrl.Equals("NewStudentAdmisionsListUI.aspx") && !msFromUrl.Equals("AdmissionFormParentDetails.aspx") && Session.Count <= 1)
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/OnlineAdmission.master";

        }
        catch (ThreadAbortException)
        {
            // Do nothing. ASP.NET is redirecting.
            // Always comment this so other developers know why the exception 
            // is being swallowed.
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
             //   trMotherEmail.Visible = true;
                ReadQueryString();
                ReadQueryString1();
                SetDefaultValuesToControls();
                FillAllControls();
                FillSchoolLocations();
             //   FillAllComboBoxes();
                SetJavascriptAttributes();
                FillReferences();
                DisplayStudentEnquiryDetails(hidEnquiryId.Value.ToInt());                
            }

        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill school enquiry references.
    /// </summary>
    private void FillReferences()
    {  
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        List<EnquiryReference> lstEnquiryReference = oSchoolEnquiryBL.GetAllSchoolReference();
        ListSource.FillCheckBoxList(lstEnquiryReference, chklstReferences, "Name", "Id");
    }
    private void ReadQueryString1()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;
        if (QueryString["Academic_Year_ID"] != null)
            hidNextAcademiYearId.Value = QueryString["Academic_Year_ID"].ToString();

        if (!QueryString["Academic_Year_ID"]. IsNull())
      cmbYear.SelectedValue= QueryString["Academic_Year_ID"].ToString();

    }

    private void ReadQueryString()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["Id"].IsNull())
        {
            hidEnquiryId.Value = QueryString["Id"];
            //cmbStd.SelectedItem.Text = QueryString["StandardName"];
        }

        if (QueryString["AcademicYearId"] != null)
            hidNextAcademiYearId.Value = QueryString["AcademicYearId"].ToString();

        if (QueryString["StatusId"] != null)
            hidStatusId.Value = QueryString["StatusId"].ToString();
    }

    private void DisplayStudentEnquiryDetails(int iStudentEnquiryId)
    {
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();

        DataSet ds = oSchoolEnquiryBL.GetStudentEnquiryDetails(iStudentEnquiryId, miSchoolId, hidNextAcademiYearId.Value.ToInt());
        DataTable oDTStudentEnquiryDetails = ds.Tables[0];

        if (oDTStudentEnquiryDetails.Rows.Count > Constants.I_ZERO)
        {
            if (Convert.ToInt32(oDTStudentEnquiryDetails.Rows[0]["Id"]) != Constants.I_ZERO)
            {
                TxtFLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() : string.Empty;
                txtFName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() : string.Empty;
                txtFFatherName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() : string.Empty;
                txtAddress.Text = oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() : string.Empty;
                txtFatherMob1.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() : string.Empty;
                txtFatherMob2.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_2"].ToString() : string.Empty;
                TxtMLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() : string.Empty;
                txtMName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() : string.Empty;
                txtMHName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() : string.Empty;
                txtAddress.Text = oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() : string.Empty;
                txtEmail.Text = oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() : string.Empty;
                TxtCLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() : string.Empty;
                txtFahterName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() : string.Empty;
                txtSName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() : string.Empty;
                cmbStd.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["For_std"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["For_std"].ToString() : Constants.S_SELECT;
                cmbYear.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["Academic_Year_Id"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Academic_Year_Id"].ToString() : "0";
                DateTime SelectedDate = oDTStudentEnquiryDetails.Rows[0]["DOB"].ToDateTime();
                txtCalDobPopup.Text = SelectedDate.Date.ToString("dd-MMM-yyyy");
                txtSchoolName.Text = oDTStudentEnquiryDetails.Rows[0]["Current_School_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Current_School_Name"].ToString() : string.Empty;
                rdoMale.Checked = oDTStudentEnquiryDetails.Rows[0]["Gender"].ToString() == "M";
                rdoFemale.Checked = oDTStudentEnquiryDetails.Rows[0]["Gender"].ToString() == "F";
                txtMotherMob1.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() : string.Empty;
                txtMotherMob2.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_2"].ToString() : string.Empty;
                TxtSibling.Text = oDTStudentEnquiryDetails.Rows[0]["Sibling_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Sibling_Name"].ToString() : string.Empty;
                txtFrnd.Text = oDTStudentEnquiryDetails.Rows[0]["Friend_Colleague_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Friend_Colleague_Name"].ToString() : string.Empty;
                cmbArea.SelectedItem.Text = oDTStudentEnquiryDetails.Rows[0]["FeeAreaName"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["FeeAreaName"].ToString() : Constants.S_SELECT;
                txtEnqNo.Text = oDTStudentEnquiryDetails.Rows[0]["Enquiry_No"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Enquiry_No"].ToString() : string.Empty;

                txtAadharCardNumber.Text = oDTStudentEnquiryDetails.Rows[0]["AadharCardNumber"].ToString();
                cmbSchoolLocation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["LocationId"].ToString();

                if (SchoolBase.Settings.IsAaryanSchool)
                {
                  //  trSPSAdmissionFor.Visible = true;
                    txtMotherEmail.Text = oDTStudentEnquiryDetails.Rows[0]["MotherEmailAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MotherEmailAddress"].ToString() : string.Empty;
                    ddlPreStandard.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["Pre_std"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Pre_std"].ToString() : Constants.S_SELECT;
                 //   ddlFatherQualification.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["FatherQualificationId"].ToString();
                  //  ddlMotherQualification.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["MotherQualificationId"].ToString();
                    txtMoWhatsup.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_WhatsUp_No"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_WhatsUp_No"].ToString() : string.Empty;
                    txtFoWhatsup.Text = oDTStudentEnquiryDetails.Rows[0]["Father_WhatsUp_No"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_WhatsUp_No"].ToString() : string.Empty;
                 //   txtdate.Text = oDTStudentEnquiryDetails.Rows[0]["date"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["date"].ToString() : string.Empty;

                    txtFQualification.Text = oDTStudentEnquiryDetails.Rows[0]["FatherQualification"].ToString();
                    txtMQualification.Text = oDTStudentEnquiryDetails.Rows[0]["MotherQualification"].ToString();
                    txtLandmarks.Text = oDTStudentEnquiryDetails.Rows[0]["Landmark"].ToString();
                }


                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt())
                {
                    txtMotherEmail.Text = oDTStudentEnquiryDetails.Rows[0]["MotherEmailAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MotherEmailAddress"].ToString() : string.Empty;
                    cmbFatherOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["FatherOccupationId"].ToString();
                    cmbMotherOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["MotherOccupationId"].ToString();
                }

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
                {
                    trSPSEnquirtNationality.Visible = true;
                    trSPSEnquiryPhoneNo.Visible = true;
                    trSPSOccupation.Visible = true;
                    trSPSPermanentAddress.Visible = true;
                    trSPSAdmissionFor.Visible = true;
                    trCategoty.Visible = true;

                    txtNationality.Text = oDTStudentEnquiryDetails.Rows[0]["Nationality"].ToString();
                    txtPassportNo.Text = oDTStudentEnquiryDetails.Rows[0]["PassportNo"].ToString();
                    txtPermanentAddress.Text = oDTStudentEnquiryDetails.Rows[0]["PermanentAddress"].ToString();
                    txtResidencePhone.Text = oDTStudentEnquiryDetails.Rows[0]["ResidencePhoneNo"].ToString();
                    txtOfficePhone.Text = oDTStudentEnquiryDetails.Rows[0]["OfficePhoneNo"].ToString();
                    txtLastSchoolAddress.Text = oDTStudentEnquiryDetails.Rows[0]["LastSchoolAddress"].ToString();

                    cmbFatherOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["FatherOccupationId"].ToString();
                    cmbMotherOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["MotherOccupationId"].ToString();
                    cmbCategory.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["CategoryId"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["CategoryId"].ToString() : Constants.S_SELECT;
                    cmbAdmissionFor.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["AdmissionFor"].ToString();
                }


                DataTable dtReference = ds.Tables[1];
                if (dtReference.Rows.Count > 0)
                {
                    for (int iIndex = 0; iIndex < dtReference.Rows.Count; iIndex++)
                    {
                        for (int iListIndex = 0; iListIndex < chklstReferences.Items.Count; iListIndex++)
                        {
                            if (dtReference.Rows[iIndex][0].ToString() == chklstReferences.Items[iListIndex].Value)
                            {
                                chklstReferences.Items[iListIndex].Selected = true;
                                break;
                            }
                        }
                    }
                }
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
            DataTable oDT = oStdCollection.GetAssociatedStandardsForEnquiry(Constants.I_ZERO);
            ControlUtility.FillDropDownList(oDT, ref cmbStd, "standard_id", "standard_name", string.Empty);
            ControlUtility.FillDropDownList(oDT, ref ddlPreStandard, "standard_id", "standard_name", string.Empty);//////////////
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }
    
    //protected void FillAllComboBoxes()
    //{
    //    DataSet oDsMaster = MasterDataCollectionBL.GetAllMasterData();
    //    ControlUtility.FillDropDownList(oDsMaster.Tables[4], ref ddlFatherQualification, "Qualification_Id", "Qualification_Name", Constants.S_SELECT);
    //    ControlUtility.FillDropDownList(oDsMaster.Tables[4], ref ddlMotherQualification, "Qualification_Id", "Qualification_Name", Constants.S_SELECT);
    //}

    /// <summary>
    /// This event is used to save student details of enquiry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool bIsHuman = base.ValidateCaptcha();        
            if (Page.IsValid && bIsHuman)
            {
                DataTable dtStudEnquiryDetails = new DataTable();
                dtStudEnquiryDetails = SaveStudentsDetails();

                if (SchoolBase.Settings.IsAaryanSchool)
                    SendSMS(dtStudEnquiryDetails);

                if (lblError1.Text != string.Empty)
                {
                    tdErrorMessage.Visible = true;
                }
                else
                {
                    if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == null && (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Admin.ToString() || Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Supervisor.ToString()))
                    {
                        //string sQueryString = "EnquiryId=" + hidDBEnquiryId.Value;
                        //sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                        //Response.Redirect("~/RITeSchool/Admission/AdmissionThankYouUI.aspx?" + sQueryString, false);

                        Response.Redirect("~/RITeSchool/Admission/EnquiryFormThankYouPopup.aspx", false);
                    }
                    else
                    {
                        string sQueryString = CommonUtility.EncryptQuerystring("AcademicYearId=" + hidNextAcademiYearId.Value + "&StatusId=" + hidStatusId.Value);
                        Response.Redirect("~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx?" + sQueryString, false);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            lblError1.Text = ex.Message;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbAdmissionFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
            StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(iSchoolId, Convert.ToInt32(cmbYear.SelectedValue));
            FillStandardComboBox(oStandardCollectionBL, cmbAdmissionFor.SelectedValue.ToInt());
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
            if (txtAadharCardNumber.Text.Trim() != string.Empty)
            {
                int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
                StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
                string sLeftDate = oStudentAdmissionsBL.ValidateBlackListStudent(iSchoolId, txtAadharCardNumber.Text.Trim());

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

    #endregion " Events " 

    #region " Private Methods "

    /// <summary>
    /// This method is used to fill all available page controls.
    /// </summary>
    private void FillAllControls()
    {
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

        // Table Indices
        const int I_TABLE_ACADAMIC_YEARS = 0;
        const int I_TABLE_NEW_ACADAMIC_YEAR_ID = 1;
              
		S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;

        DataSet oDataSet = SchoolEnquiryBL.GetAllMasterDataForStudentEnquiry(iSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);
        int iNewAcadamicYearID=0;
       
        if(oDataSet.Tables[I_TABLE_NEW_ACADAMIC_YEAR_ID].Rows[0][0]!=DBNull.Value)
            iNewAcadamicYearID = Convert.ToInt32(oDataSet.Tables[I_TABLE_NEW_ACADAMIC_YEAR_ID].Rows[0][0]);

        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(iSchoolId, iNewAcadamicYearID);
         //DataTable oDataTableStd = oStandardCollectionBL.GetAssociatedStandardsForEnquiry();
         DataTable oDataTableArea = SchoolEnquiryBL.GetAllAreas(iSchoolId);
         DataTable dtOccupation = SchoolEnquiryBL.GetAllOccupations();
         DataTable dtCategories = SchoolEnquiryBL.GetAllCategoriesForEnquiry();
         DataTable dtAdmissionForm = oStandardCollectionBL.GetAdmissionForCategories();

         // Fills Academic Year combo and set it to default value.
        ControlUtility.FillDropDownList(oDataSet.Tables[I_TABLE_ACADAMIC_YEARS], ref cmbYear, "Academic_Year_ID", "AcademicYear", string.Empty);

        if (hidNextAcademiYearId.Value != "0")
        {
            cmbYear.SelectedValue = hidNextAcademiYearId.Value;            
            cmbYear.Enabled = false;////
        }
        else if (iNewAcadamicYearID != 0)
        {
            cmbYear.SelectedValue = iNewAcadamicYearID.ToString();
            cmbYear.Enabled = false;//////
        }
                
        txtEnqNo.Text = GetNextEnquiryNo(iSchoolId, hidNextAcademiYearId.Value.ToInt());

        // Fill Standard combo and set to default value.
        if (!IsPostBack)
        {
            ControlUtility.FillDropDownList(dtAdmissionForm, ref cmbAdmissionFor, "Id", "AdmissionFor", string.Empty);
            int iAdmissionForId = Constants.I_ZERO;
            if (iSchoolId == Constants.SchoolId.SPS.ToInt() || iSchoolId == Constants.SchoolId.SVP.ToInt() || iSchoolId == Constants.SchoolId.SVNP.ToInt())
                iAdmissionForId = cmbAdmissionFor.SelectedValue.ToInt();

            FillStandardComboBox(oStandardCollectionBL, iAdmissionForId);
            ControlUtility.FillDropDownList(oDataTableArea, ref cmbArea, "FeeAreaNameId", "Fee_AreaName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(dtOccupation, ref cmbFatherOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);
            ControlUtility.FillDropDownList(dtOccupation, ref cmbMotherOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);
            ControlUtility.FillDropDownList(dtCategories, ref cmbCategory, "Category_Id", "Category_Name", Constants.S_SELECT);
            
        }
    }


  

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary>
    private void SetDefaultValuesToControls()
    {
        hidDOB.Value = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        hidDPISSchoolId.Value = Constants.SchoolId.DPIS.ToString();
        trSPSEnquirtNationality.Visible = false;
        trSPSEnquiryPhoneNo.Visible = false;
        trSPSOccupation.Visible = false;
        trSPSPermanentAddress.Visible = false;
        trCategoty.Visible = false;
        tdstd.Visible = false;
        tdstd1.Visible = false;
      //  trMotherEmail.Visible = true;
        tdMoemail.Visible = false;
        tdMoemail1.Visible = false;

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            lblSiblingSchool.Text = "Siblings at Shantiniketan:";
            lblHeardOfSchool.Text = "Heard of Shantiniketan from:";
            txtEmail.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMotherMob1.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
        }
        else if (SchoolBase.Settings.IsAaryanSchool)
        {
            trDisplayArea.Visible = false;
            trlandmark.Visible = true;
            txtEmail.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMoWhatsup.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0"); 
         
            tdMoemail.Visible = true;
            tdMoemail1.Visible = true;
            lblFatherEmail.Text = "Father E-Mail ID :";
            lblHeardOfSchool.Text = "Source:";
            Label5.Text = "Standard";
            spCurrentSchool.InnerText = "Previous School Name";
            trMotherEmail.Visible = true;////comment
            trHeardOfSchool.Visible = true;
            trWhatsup.Visible = true;
            tr1.Visible = true;
            trEnquiryOther.Visible = false;
            tdstd.Visible = true;
            tdstd1.Visible = true;
           trmobile2.Visible = false;

            txtSLastName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtFahterName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMotherMob1.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMoWhatsup.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.OWS.ToInt())
        {
            lblSiblingSchool.Text = "Siblings at Oxford:";
            lblHeardOfSchool.Text = "Heard of Oxford from:";
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt())
        {
            lblSiblingSchool.Text = "Siblings :";
            lblFatherEmail.Text = "Father E-Mail ID :";
            trSPSOccupation.Visible = true;
            trMotherEmail.Visible = true;
            trHeardOfSchool.Visible = true;
            lblHeardOfSchool.Text = "Where did you hear about us?";
            trDisplayArea.Visible = false;
            btnBack.Visible = false;
            lblBranchName.Text = "Branch - Pimple Saudagar";
            trDPISBranch.Visible = true;
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
        {
            cmbAdmissionFor.SelectedValue = Constants.S_ONE;
            trEnquiryOther.Visible = false;
            trHeardOfSchool.Visible = false;

            trSPSEnquirtNationality.Visible = true;
            trSPSEnquiryPhoneNo.Visible = true;
            trSPSOccupation.Visible = true;
            trSPSPermanentAddress.Visible = true;
            trLastSchoolAddress.Visible = true;
            trSPSAdmissionFor.Visible = true;
            trPermanentAddressSameAsPresent.Visible = true;
            trCategoty.Visible = true;

            trDisplayArea.Visible = false;
            trSPSEnquiry.Visible = false;
            lblAddressField.Text = "Present Address  with Pincode :";
            spCurrentSchool.InnerText = "School in which studying :";
            lblEnquiryName.Text = "Registration No :";
            lblEnquiryHeader.Text = "Registration Form";
            lblStudentName.Text = "Name of the Student:";
            lblFatherMobileNo.Text = "Father's Mobile No.:";
            lblMotherMobileNo.Text = "Mother's Mobile No.:";

            txtSLastName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtFahterName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtFSurname.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtFFatherName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMSurname.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMHName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtSchoolName.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");
            txtMotherMob1.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffa0");

            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == null && (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Admin.ToString() || Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Supervisor.ToString()))
                btnBack.Visible = false;
            else
                btnBack.Visible = true;

            chkAddress.Attributes.Add("onclick", "CopyPresentToPermanent()");
        }
        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PEMS.ToInt())
        {
            trEnquiryOther.Visible = false;
            trHeardOfSchool.Visible = false;
        }

        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidServerDate.Value = DateTime.Today.ToString();
        TxtCLastName.Focus();
        hidMaxBdate.Value = (DateTime.Today.AddYears(-3)).ToString("dd-MMM-yyyy");
        hidMinBdate.Value = (DateTime.MinValue.ToString("dd-MMM-yyyy"));

        if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && (msFromUrl.Equals("NewStudentAdmisionsListUI.aspx") || msFromUrl.Equals("AdmissionFormParentDetails.aspx")))
        {
            cmbStd.Visible = true;
            cmbStd.Enabled = true;
            cmbStd.Focus();
            lblStdName.Visible = false;
        }

        hidSchoolId.Value = ConfigurationManager.AppSettings["SchoolID"].ToString();
        hidSNSSchoolId.Value = Constants.SchoolId.SNS.ToInt().ToString();
        hidSPSSchoolId.Value = Constants.SchoolId.SPS.ToInt().ToString();
        hidSVPSchoolId.Value = Constants.SchoolId.SVP.ToInt().ToString();
        hidSVNPSchoolId.Value = Constants.SchoolId.SVNP.ToInt().ToString();
        hidAaryanSchool.Value = SchoolBase.Settings.IsAaryanSchool.ToString();

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
        {
            lblBranchName.Text = "Branch - Ravet";
            trDPISBranch.Visible = true;
        }
    }

    private string GetNextEnquiryNo(int iSchoolId,int iAcademicYearID)
    {
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        string sEnqiryNo = oSchoolEnquiryBL.GetNextEnquiryNo(iSchoolId, iAcademicYearID);
        return sEnqiryNo;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSubmit.Attributes.Add("onclick", "if(!ValidateControls()) return false;");

         string sQueryString = CommonUtility.EncryptQuerystring("AcademicYearId=" + hidNextAcademiYearId.Value + "&StatusId=" + hidStatusId.Value);
         btnBack.PostBackUrl = "~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx?" + sQueryString;

        if (miSchoolId == Constants.SchoolId.OWS.ToInt())
            hidIsMotherNameMandatory.Value = Constants.S_ZERO;
        else
        {
            hidIsMotherNameMandatory.Value = Constants.S_ONE;
            txtMName.Style.Add("background-color", "#ffffa0");
        }

        //if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        //{
        //    txtAadharCardNumber.Style.Add("background-color", "#ffffa0");
        //    hidValidateAadharCard.Value = Constants.S_YES;

        //}
        //else
            hidValidateAadharCard.Value = Constants.S_NO;

        if (SchoolBase.Settings.IsAaryanSchool)
        {
          //  tdSPSLastName.Visible = true;
        }
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
        {
            tdSPSLastName.Visible = true;
            tdSPSlblLastName.Visible = true;
            tdSPSFlastName.Visible = true;
            tdSPSlblFlastName.Visible = true;
            tdSPSMLastName.Visible = true;
            tdSPSlblMLastName.Visible = true;

            tdLastName.Visible = false;
            tdlblLastName.Visible = false;
            tdFlastName.Visible = false;
            tdlblFlastName.Visible = false;
            tdMLastName.Visible = false;
            tdlblMLastName.Visible = false;
        }
        else
        {
            tdLastName.Visible = true;
            tdlblLastName.Visible = true;
            tdFlastName.Visible = true;
            tdlblFlastName.Visible = true;
            tdMLastName.Visible = true;
            tdlblMLastName.Visible = true;

            tdSPSLastName.Visible = false;
            tdSPSlblLastName.Visible = false;
            tdSPSFlastName.Visible = false;
            tdSPSlblFlastName.Visible = false;
            tdSPSMLastName.Visible = false;
            tdSPSlblMLastName.Visible = false;
        }

        if (Settings.IsAaryanSchool)
        {
            CompareValidator2.Enabled = false;
            reqAddress.Enabled = false;
            regAddress.Enabled = false;
            cst_MobileNumber2.Enabled = false;
            //reqMobileNo.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
            cstMotherMobileNoEmpty.Enabled = false;
            reqFName.Enabled = false;
            CustomValidator1.Enabled = false;


            ddlPreStandard.Style.Add("background-color", "white");
            txtAddress.Style.Add("background-color", "white");
            txtMotherEmail.Style.Add("background-color", "white");
            //txtFatherMob1.Style.Add("background-color", "white");
            txtMotherMob1.Style.Add("background-color", "white");
            txtFQualification.Style.Add("background-color", "white");
            txtMQualification.Style.Add("background-color", "white");
            txtFName.Style.Add("background-color", "white");
            txtMName.Style.Add("background-color", "white");
        }

        hidCaptData.Value = base.GetCaptcheHeaderData();
    }

    /// <summary>
    /// This method is used to save student details.
    /// </summary>
    private DataTable SaveStudentsDetails()
    {
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        int iEnquiryId;

        if(hidEnquiryId.Value != string.Empty && hidEnquiryId.Value != Constants.S_ZERO)
             iEnquiryId = Convert.ToInt32(hidEnquiryId.Value);
        else
            iEnquiryId = 0;


        string sEnquiryXML = GetEnquiryXML();
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        string enquiryNo = txtEnqNo.Text.Substring(0, 7);
       
        oSchoolEnquiryBL.EnquiryDetails = sEnquiryXML;
        string sSchoolReferences = GetSchoolReferences();
        int iDBEnquiryId;
        DataTable dtStudentEnquiry = oSchoolEnquiryBL.InsertSchoolEnquiryDetails(iSchoolId, iEnquiryId, sSchoolReferences, out iDBEnquiryId);
        hidDBEnquiryId.Value = iDBEnquiryId.ToString();
        return dtStudentEnquiry;
    }

    /// <summary>
    /// This method is used to return selected school references.
    /// </summary>
    /// <returns></returns>
    private string GetSchoolReferences()
    {
        StringBuilder obj = new StringBuilder();
        for (int iListIndex = 0; iListIndex < chklstReferences.Items.Count; iListIndex++)
        {
            if (chklstReferences.Items[iListIndex].Selected == true)
                obj.Append(","+chklstReferences.Items[iListIndex].Value);
        }

        if (obj.Length > 0)
            return obj.ToString().Substring(1);
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to generate XML format for student enquiry details.
    /// </summary>
    /// <returns></returns>
    private string GetEnquiryXML()
    {
       
        const char C_FEMALE = 'F';
        const char C_MALE = 'M';
        const string S_ELEMENT = "element";
        const int I_MASTER = 5;
        const int I_MISS = 6;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolEnquiry");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolEnquiry", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolEnquiryDetails", "");

        // Student Details
        string sAtrrName = "School_Id";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = ConfigurationManager.AppSettings["SchoolID"];
        oXmlNode.Attributes.Append(attr);

        int iAdmissionFor = Constants.I_ZERO;
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())        
            iAdmissionFor = cmbAdmissionFor.SelectedValue.ToInt();        

        sAtrrName = "Admission_For";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iAdmissionFor.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Enquiry_No";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEnqNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = TxtCLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFahterName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Gender";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoMale.Checked ? C_MALE.ToString() : C_FEMALE.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DOB";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCalDobPopup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Current_school_name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSchoolName.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Acedemic_Year_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbYear.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "For_Standard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbStd.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = TxtMLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMHName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = TxtFLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFFatherName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAddress.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Mobile_Number1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherMob1.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Mobile_Number2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherMob2.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Mobile_Number1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFatherMob1.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Mobile_Number2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFatherMob2.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Sibling_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = TxtSibling.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Friend_Colleague_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFrnd.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Area";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbArea.SelectedIndex.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Salutation_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = rdoMale.Checked ? I_MASTER.ToString() : I_MISS.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbFatherOccupation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbMotherOccupation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbMotherOccupation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Nationality";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtNationality.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PassportNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPassportNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PermanentAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPermanentAddress.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "ResidencePhoneNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtResidencePhone.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OfficePhoneNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtOfficePhone.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastSchoolAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLastSchoolAddress.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MoQualification";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMQualification.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FoQualification";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFQualification.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_WhatsUp_Number";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFoWhatsup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_WhatsUp_Number";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMoWhatsup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Pre_Standard";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = ddlPreStandard.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Landmark";////////////new add
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLandmarks.Text;
        oXmlNode.Attributes.Append(attr);

        //sAtrrName = "Date";////////////new add
        //attr = oDoc.CreateAttribute(sAtrrName);
        //attr.Value = txtdate.Text;
        //oXmlNode.Attributes.Append(attr);

        int iCategoryId = Constants.I_ZERO;

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SVNP.ToInt())
            iCategoryId = cmbCategory.SelectedValue.ToInt();

        sAtrrName = "AadharCardNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAadharCardNumber.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LocationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbSchoolLocation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CategoryId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iCategoryId.ToString();
        oXmlNode.Attributes.Append(attr);

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
    /// This method is used to fill the Standard Combobox.
    /// </summary>
    /// <param name="oStandardCollectionBL"></param>
    private void FillStandardComboBox(StandardCollectionBL oStandardCollectionBL, int iAdmissionForId)
    {
        DataTable oDataTableStd = oStandardCollectionBL.GetAssociatedStandardsForEnquiry(iAdmissionForId);
        ControlUtility.FillDropDownList(oDataTableStd, ref cmbStd, "standard_id", "standard_name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataTableStd, ref ddlPreStandard, "standard_id", "standard_name", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <param name="oDataTable"></param>
    private void SendSMS(DataTable oDataTable)
    {
        string sAdmissionConfirmSMS = string.Empty;
        string sSmsSubject = string.Empty;
        string sTemplateRegistrationId = string.Empty; ////
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

        if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
        {
            int iRowCount = oDataTable.Rows.Count;
            int iSMSType = 0;
            int iSmsId = Constants.I_ZERO;

            iSmsId = Convert.ToInt32(Constants.SMSTemplate.EnquirySubmitSMS);

            DataTable oDTTemplate = SmsTemplateBL.GetTemplate(Constants.SMSTemplate.EnquirySubmitSMS.ToString(), iSchoolId);
            if (oDTTemplate.Rows.Count != 0)
            {
                if (oDTTemplate.Rows[0][2] != DBNull.Value)
                {
                    sAdmissionConfirmSMS = Convert.ToString(oDTTemplate.Rows[0][2]);
                    sSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
                    if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)  ////
                        sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString(); ////
                }

                if (oDTTemplate.Rows[0][3] != DBNull.Value)
                    iSMSType = oDTTemplate.Rows[0][3].ToInt();
            }

            SchoolBL oSchoolBL = new SchoolBL(iSchoolId);
            string sSMSSenderName = oSchoolBL.SMSSenderName;
            Hashtable moManualMobileNo = new Hashtable();

            foreach (DataRow oDR in oDataTable.Rows)
            {   
                string sMobileNo = Convert.ToString(oDR["MobileNo"]);
                moManualMobileNo[sMobileNo.Trim()] = sMobileNo.Trim();
                string sDisplayText = sMobileNo;
                SMS oSMS = new SMS();
                oSMS.Sender = sSMSSenderName;
                oSMS.SMSText = sAdmissionConfirmSMS;
                oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                oSMS.DisplayText = sDisplayText;
                oSMS.SMSType = iSMSType;
                oSMS.SchoolID = iSchoolId;
                oSMS.AcademicYearID = miAcademicYearId;
                oSMS.ToManualNumbers = moManualMobileNo;
                oSMS.TemplateRegistrationId = sTemplateRegistrationId; ////
                oSMS.Send();
            }
        }
    }

    /// <summary>
    /// This method is used to fill school locations.
    /// </summary>
    private void FillSchoolLocations()
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
        {
            trLocation.Visible = true;
            SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
            DataTable dtLocation = oSchoolEnquiryBL.GetSchoolLocations();
            cmbSchoolLocation.Bind(dtLocation, "Id", "Name", Constants.S_SELECT);
            reqcmbLocation.Enabled = true;

            if (iSchoolId == Constants.SchoolId.DPIS.ToInt())
                cmbSchoolLocation.Items.FindByValue("1").Selected = true;
            else
                cmbSchoolLocation.Items.FindByValue("2").Selected = true;

        }
        //else  
        //{
        //    trLocation.Visible =false;
        //    reqcmbLocation.Enabled = false;
        //}

        trLocation.Visible = false;
        reqcmbLocation.Enabled = false;
    }
    
    #endregion " Private Methods "
}
