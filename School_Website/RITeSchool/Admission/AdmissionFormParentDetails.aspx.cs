// File Name  : AdmissionFormParentDetails.aspx.cs
// Created By : Amit 
// Date       : 17/11/2009
//Description : This class is used to fill parents details at online admission of student.

using System;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;
using System.Collections.Generic;

public partial class AdmissionFormParentDetails : SchoolBase
{
    #region " Events "

    string sIsOnline = Constants.S_NO;
	string msAmount = string.Empty;
	string msStandardId = string.Empty;
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
    int? miEnqId = 0;
    bool mbIsEditMode = false;
    /// <summary>
    /// This event is used to set master page file as per user role.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
		try		
        {
			base.OnPreInit(e);			

			if (QueryString["sIsOnline"] != null)
				sIsOnline = QueryString["sIsOnline"].ToString();
			if (QueryString["StandardId"] != null)
				msStandardId = QueryString["StandardId"].ToString();
			if (QueryString["Amount"] != null)
				msAmount = QueryString["Amount"].ToString();

            int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
            if (iSchoolId == Constants.SchoolId.PIONEER.ToInt())
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            else if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && sIsOnline == Constants.S_NO)
            {
                if (moUserRole == Constants.UserRoles.Admin
                    || moUserRole == Constants.UserRoles.Supervisor
                    || moUserRole == Constants.UserRoles.Teacher)
                    this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            }
            else           
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/OnlineAdmission.master";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

    /// <summary>
    /// This event is used to fill all default controls and sets javascript properties to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			if (QueryString["Amount"] != null)
				msAmount = QueryString["Amount"].ToString();
            if (QueryString["EnquiryId"] != null || QueryString["EnquiryId"].ToInt() != 0)
                miEnqId = QueryString["EnquiryId"].ToInt();
            if (QueryString["IsEditMode"] != null)
            {
                if (QueryString["IsEditMode"] == Constants.S_YES)
                    mbIsEditMode = true;
            }

            if (QueryString["StandardName"] != null)
                hidStandardName.Value = QueryString["StandardName"];

            trAdmissionCoordinator.Visible = sIsOnline == Constants.S_NO;
            
            if (!IsPostBack)
            {
                SetValidationState();
				FillAllDefaultControls();
                SetJavascriptAttributes();

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                {
                    trAdmissionCoordinator.Visible = false;
                    trTwinSelection.Visible = false;
                    aParentConsent.HRef = "../DOWNLOADS/AdmissionForms/Parental Consent Form Rev1.pdf";
                }
            }

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
            {
                //if(!mbIsEditMode)
                trSiblingDetailsForPP.Visible = true;
            }
            else
                trSiblingDetailsForPP.Visible = false;

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
            {
                trSector.Visible = false;
                trFax.Visible = false;

                System.Drawing.Color oBackColor = System.Drawing.Color.FromName("#FFFFA0");

                List<WebControl> lstControls = new List<WebControl> { 
                txtFSurname, txtMSurname, txtFAadharCard, txtFQuali, txtFMotherTounge, txtFLangSpoken, cmbFReligion,
                txtFNationality, txtFIncome,txtFCompany,txtFOrgAddress,txtFOccDetails,txtFOffPhone,txtFEmail,txtMAadharCard,txtMQuali,txtMMotherTounge,txtMLangSpoken,cmbMBloodGroup,cmbMReligion,txtMNationality,cmbMOccupation,
                txtMIncome,txtMCompany,txtMOrgAddress,txtMOccDetails,txtMOffPhone,txtMEmail,txtFNameOnAadharCard,txtMNameOnAadharCard
                };

                foreach (var ctrl in lstControls)
                    ctrl.BackColor = oBackColor;

                List<WebControl> lstValidators = new List<WebControl> { reqValtxtFSurname, reqValtxtMSurname, reqValFAadharCard, reqValFQuali, reqValFMotherTounge, reqValFLangSpoken,
                reqValFReligion, reqValtxtFNationality, reqValtxtFCompany,reqValtxtFOrgAddress, reqValFOccDetails,reqValFOffPhone, reqValtxtFEmail, 
                reqValMAadharCard, reqValMQuali, reqValtxtMMotherTounge, reqValMLangSpoken, reqValMReligion, reqValMNationality, reqValcmbMOccupation, reqValFNameOnAadhar, reqValMNameOnAadhar};
                foreach (var ctrl in lstValidators)
                    ctrl.Enabled = true;

                hidShowAnnualIncomeValidation.Value = Constants.S_YES;
                hidShowMotherRelatedValidaions.Value = Constants.S_YES;

                cmbMOccupation.Attributes.Add("onchange","SetMotherRelatedFields();");

                aParentConsent.HRef = "../DOWNLOADS/AdmissionForms/Consent_Undertaking_medical_and_apaar_form.pdf";

                cmpValFMEmail.Enabled = true;
            }
            else
            {
                hidShowMotherRelatedValidaions.Value = Constants.S_NO;
            }

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
            {
                reqValFAadharCard.Enabled = true;
                reqValMAadharCard.Enabled = true;
                reqValFNameOnAadhar.Enabled = true;
                reqValMNameOnAadhar.Enabled = true;
                
                txtFAadharCard.BackColor = System.Drawing.Color.FromName("#FFFFA0");
                txtMAadharCard.BackColor = System.Drawing.Color.FromName("#FFFFA0");
                txtFNameOnAadharCard.BackColor = System.Drawing.Color.FromName("#FFFFA0");
                txtMNameOnAadharCard.BackColor = System.Drawing.Color.FromName("#FFFFA0");
            }

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
            {
                cmp_valFOcc.Enabled = false;
                cmbFOccupation.BackColor = System.Drawing.Color.White;
            }

            if (!Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]))
                SubmissionWizardSteps.EnableFormFee= false;

            hidSchoolId.Value = ConfigurationManager.AppSettings["SchoolID"].ToString();
            hidSNSSchoolId.Value = Constants.SchoolId.SNS.ToInt().ToString();

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
            {
                if (hidStandardName.Value == "9")
                {
                    tr9thSubjectCombination.Visible = true;
                    hidIsSubjectSectionApplicable.Value = "Y";
                }
                else
                {
                    tr9thSubjectCombination.Visible = false;
                    hidIsSubjectSectionApplicable.Value = "N";
                }


                if (hidStandardName.Value != string.Empty)
                {
                    List<string> lstStandards = new List<string> { "11 Sci", "12 Sci", "11 Com", "12 Com", "11 Art", "12 Art" };

                    if (lstStandards.Contains(hidStandardName.Value))
                    {
                        hidIsSubjectSectionApplicable.Value = "Y";
                        trSNSStrimwiseSubjects.Visible = true;                        
                        cmbStream.Attributes.Add("onChange", "ChangeStreamDetails(this)");
                    }
                    else
                        trSNSStrimwiseSubjects.Visible = false;
                }
            }
            else
                trSNSStrimwiseSubjects.Visible = false;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save parent details 
    /// and redirect to online fee submiting form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string sQryString = string.Empty;
            DataTable oDTStudentDetails = SaveStudentParentsDetails();

            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && sIsOnline == Constants.S_NO && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
            {
                if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Admin
                || (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Supervisor
                || (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher)
                {
                    //string sParams = SendSMS(oDTStudentDetails); 
                    //sQryString = CommonUtility.EncryptQuerystring(sParams); It is removed because there is no need to send sms to manually added student.
                    Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = null;
                    Response.Redirect("~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx", false);
                }
            }
            else if (oDTStudentDetails != null && oDTStudentDetails.Columns.Count == 4)
            {
                if (Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]))
                {
                    string sStandardId = QueryString["StandardId"].ToString();
                    string sQueryString = "Amount=" + msAmount + "&Form_Number=" + oDTStudentDetails.Rows[0]["Form_Number"] + "&StandardId=" + sStandardId;
                    Response.Redirect("~/RITeSchool/PaymentConfirmationUI.Aspx?" + CommonUtility.EncryptQuerystring(sQueryString), false);
                }
                else
                {
                    if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
                    {   
                        string sQueryString ="StudentAdmissionId=" + Session[Constants.S_SESSION_STUDENT_ADMISSION_ID];
                        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                        Response.Redirect("~/RITeSchool/Admission/LocalGuardianDetialsUI.aspx?" + sQueryString, false);
                    }
                    else
                    {
                        string sQueryString = "Form_Number=" + oDTStudentDetails.Rows[0]["Form_Number"] + "&Mobile_Number=" + oDTStudentDetails.Rows[0]["MobileNumber"] + "&iAdmissionId=" + Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] + "&EnableAdmissionFormFee=" + Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]) + "&AcademicYearId=" + hidNewAcadamicYearID.Value;
                        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                        SendSMS(oDTStudentDetails.Rows[0]["Form_Number"].ToString(), oDTStudentDetails.Rows[0]["MobileNumber"].ToString());
                        Response.Redirect("~/RITeSchool/Admission/AdmissionThankYouUI.aspx?" + sQueryString, false);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            tdErrorMessage.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to send the SMS to form submitted user.
    /// </summary>
    /// <param name="aCanSendSMS"></param>
    public void SendSMS(string asFormNumber, string asMobileNumber)
    {
        if (!asMobileNumber.IsNullOrEmpty())
        {
            Hashtable oHTUsersMobileNo = new Hashtable();
            //string[] sMobileNumber = asMobileNumber.Split(',');
            string sTemplateName = string.Empty;
            string sSmsText = string.Empty;
            string sTemplateRegistrationId = string.Empty;

            oHTUsersMobileNo[asMobileNumber] = asMobileNumber.Trim();

            //if (sMobileNumber.Length > Constants.I_ONE && !sMobileNumber[1].Trim().IsNullOrEmpty() && sMobileNumber[0].Trim() != sMobileNumber[1].Trim())
            //    oHTUsersMobileNo[aiUserId + "sm;"] = sMobileNumber[1].Trim();

            int iSmsId = Convert.ToInt32(Constants.SMSTemplate.FormReceivedSMS);
			DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, ConfigurationManager.AppSettings["SchoolID"].ToInt());
            if (oDTTemplate.Rows.Count != 0)
            {
                if (oDTTemplate.Rows[0][2] != DBNull.Value)
                {
                    sSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);

                    if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sTemplateName = Convert.ToString(oDTTemplate.Rows[0][1]);
                }
            }

			SchoolBL oSchoolBL = new SchoolBL(ConfigurationManager.AppSettings["SchoolID"].ToInt());

            SMS oSMS = new SMS();
			oSMS.SenderID = oSchoolBL.AdminId;
			oSMS.SenderRoleID = oSchoolBL.AdminUeserRoleId;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sSmsText.Replace("%FORMNUMBER%",asFormNumber);
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sTemplateName;
            oSMS.DisplayText = asMobileNumber;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
			oSMS.SchoolID = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"].ToInt());           
            oSMS.ToManualNumbers= oHTUsersMobileNo;
            oSMS.Send();
            oHTUsersMobileNo.Clear();
        }
    }
   

    /// <summary>
    /// This method is used to save partents and 
    /// redirect to student admission form for submission of sibling.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSiblingSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string sQryString = string.Empty;
            DataTable oDTStudentDetails = SaveStudentParentsDetails();
            if (oDTStudentDetails != null && oDTStudentDetails.Columns.Count == 3)
            {
				//string sParams = SendSMS(oDTStudentDetails);
				//sQryString = CommonUtility.EncryptQuerystring(sParams);
            }
            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && sIsOnline==Constants.S_NO)
            {
                if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor
                || moUserRole == Constants.UserRoles.Teacher)
                {
                    string sQueryString = CommonUtility.EncryptQuerystring("AcademicYearId=" + hidNewAcadamicYearID.Value);
                    Response.Redirect("~/RITeSchool/Admission/AdmissionFormStudentDetails.aspx?" + sQueryString, false);
                }
            }
            else
				Response.Redirect("~/RITeSchool/PaymentConfirmationUI.Aspx", false);
        }
        catch (SqlException ex)
        {
            tdErrorMessage.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisionCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    private void FillDivisionCombo()
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(ConfigurationManager.AppSettings["SchoolID"].ToInt(), hidNewAcadamicYearID.Value.ToInt());
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForAdmissionSibling(cmbStandard.SelectedValue.ToInt());

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used fill all default controls.
    /// </summary>
    private void FillAllDefaultControls()
    {
        // Table Indices
        //const int AdmissionMasterData.I_TABLE_NEW_ACADAMIC_YEAR_ID = 1;
        //const int AdmissionMasterData.I_TABLE_RELIGIONS = 3;
        //const int AdmissionMasterData.I_TABLE_OCCUPATIONS = 4;
        //const int AdmissionMasterData.I_TABLE_EVENTS = 5;
        //const int AdmissionMasterData.I_TABLE_MOTHER_DATA = 7;
        //const int AdmissionMasterData.I_TABLE_STUDENT_DETAILS = 8;
        //const int AdmissionMasterData.I_TABLE_PARENT_DETAILS = 9;
        //const int AdmissionMasterData.I_TABLE_PARENT_IN_EVENTS = 10;
        //const int AdmissionMasterData.I_TABLE_BLOOD_GROUPS = 13;
        //const int AdmissionMasterData.I_TABLE_BROTHER_DETAILS = 18;
        //const int AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS = 19;

        string sAcademicYearId = QueryString["AcademicYearId"].ToString();
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);

        if (iSchoolId == Constants.SchoolId.SNS.ToInt())
            trSNSBrotherDetails1.Visible = true;
        else
            trSNSBrotherDetails1.Visible = false;

        int iStudentAdmissionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);
		S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
        DataSet oDataSet = MasterDataCollectionBL.GetAllMasterDataForStudentAdmission(iSchoolId, iStudentAdmissionId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR, sAcademicYearId.ToInt());
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_RELIGIONS], ref cmbFReligion, "Religion_Id", "Religion_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_RELIGIONS], ref cmbMReligion, "Religion_Id", "Religion_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_OCCUPATIONS], ref cmbFOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_OCCUPATIONS], ref cmbMOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);
        ControlUtility.FillCheckBoxList(oDataSet.Tables[AdmissionMasterData.I_TABLE_EVENTS], ref chklstEvents, "AdmissionParentTeacherAsssociationID", "EventType", false);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_BLOOD_GROUPS], ref cmbFBloodGroup, "Id", "BloodGroup", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[AdmissionMasterData.I_TABLE_BLOOD_GROUPS], ref cmbMBloodGroup, "Id", "BloodGroup", Constants.S_SELECT);

        ListItem oItem = cmbFOccupation.Items.FindByText("House Wife");
        if (oItem != null)
            cmbFOccupation.Items.Remove(oItem);

        if (ConfigurationManager.AppSettings["SchoolId"] == Convert.ToString(Constants.SchoolId.DSK.ToInt()))
        {
            txtFIncome.Visible = true;
            txtMIncome.Visible = true;
            lblAnnualIncome.Visible = true;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
        {
            trSPSParentDOB.Visible = true;
            trSPSParentbloodGroup.Visible = true;
            trSPSParentMobile.Visible = true;
            trSPSParentPanNo.Visible = true;
            trSPSParentOrgAddress.Visible = true;
            trSPSAadharCardNo.Visible = true;

            trPTAAssociation.Visible = false;
            trPTAControls.Visible = false;
            trImportant.Visible = false;
            SubmissionWizardSteps.Visible = false;
            trAssureNotice.Visible = false;
            trParentConsent.Visible = false;
            trAccept.Visible = false;
            trNoAccept.Visible = false;
        }

        hidNewAcadamicYearID.Value = Convert.ToString(oDataSet.Tables[AdmissionMasterData.I_TABLE_NEW_ACADAMIC_YEAR_ID].Rows[0][0]);

        FillStandardCombo();
        FillDivisionCombo();
        if (iStudentAdmissionId != 0)
        {
            hidMParentID.Value = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Student_Admission_Parent_Id"].ToString();
            txtMName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["First_Name"].ToString();
            txtMHName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Middle_Name"].ToString();
            txtMSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Last_Name"].ToString();
            txtMOffPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MOfficePhoneNumber"].ToString();
            txtMEmail.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MEmailAddress"].ToString();
            txtMMotherTounge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Mother_Tongue"].ToString();
            cmbMReligion.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Religion"].ToString();
            cmbMOccupation.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Occupation"].ToString();
            txtMQuali.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Educational_Qualification"].ToString();
            txtMCompany.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Company_Name"].ToString();
            txtMLangSpoken.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["OtherSpokenLanguages"].ToString();
            txtMOccDetails.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MOccupationDetails"].ToString();
            txtMFaxNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MFaxNumber"].ToString();            

            txtFName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["GuardianFirstName"].ToString();
            txtFFatherName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["GuardianMiddleName"].ToString();
            txtFSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["GuardianLastName"].ToString();
            txtFEmail.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["EmailAddress"].ToString();
            txtFOffPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FOfficePhoneNumber"].ToString();
            txtFMotherTounge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Mother_Tongue"].ToString();
            cmbFReligion.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["Religion"].ToString();
            cmbFOccupation.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FatherOccupation"].ToString();
            txtFQuali.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FatherEducational"].ToString();
            txtFCompany.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FatherCompanyName"].ToString();
            txtFLangSpoken.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FOtherSpokenLanguages"].ToString();
            txtFOccDetails.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FOccupationDetails"].ToString();
            txtFFaxNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FFaxNumber"].ToString();

            txtFAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FAadharCardNumber"].ToString();
            txtMAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MAadharCardNumber"].ToString();

            txtFNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FatherNameOnAadharCard"].ToString();
            txtMNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MotherNameOnAadharCard"].ToString();

            txtMIncome.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MAnnualIncome"].ToString();
            txtFIncome.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FAnnualIncome"].ToString();

            txtFNationality.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FNationality"].ToString();
            txtMNationality.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MNationality"].ToString();

            txtFOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FOfficeAddress"].ToString();
            txtMOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MOfficeAddress"].ToString();

            txtFSector.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FSector"].ToString();
            txtMSector.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MSector"].ToString();

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
            {
                if (QueryString["FatherMobileNo"] != null && QueryString["FatherMobileNo"] != Constants.S_ZERO)
                    txtFMobileNo.Text = QueryString["FatherMobileNo"].ToString();
                else
                    txtFMobileNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FMobileNo"].ToString();

                if (QueryString["FatherOccupation"] != null && QueryString["FatherOccupation"] != Constants.S_ZERO)
                    cmbFOccupation.SelectedValue = QueryString["FatherOccupation"].ToString();

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FDOB"].ToString() != string.Empty)
                    txtFDOB.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FDOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT);

                txtFPanNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FPanNo"].ToString();
                txtFAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FAadharCardNumber"].ToString();
                txtFNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FatherNameOnAadharCard"].ToString();
                cmbFBloodGroup.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FBloodGroup"].ToString();
                txtFOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["FOfficeAddress"].ToString();

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MDOB"].ToString() != string.Empty)
                    txtMDOB.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MDOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT);

                txtMPanNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MPanNo"].ToString();
                txtMAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MAadharCardNumber"].ToString();
                txtFNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MotherNameOnAadharCard"].ToString();
                txtMMobileNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MMobileNo"].ToString();
                cmbMBloodGroup.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MBloodGroup"].ToString();
                txtMOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["MOfficeAddress"].ToString();

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
                if (txtFMobileNo.Text != string.Empty)
                    txtFMobileNo.Enabled = false;
                if (txtMMobileNo.Text != string.Empty)
                    txtMMobileNo.Enabled = false;
                if (cmbFOccupation.SelectedValue != Constants.S_ZERO)
                    cmbFOccupation.Enabled = false;
                if (cmbMOccupation.SelectedValue != Constants.S_ZERO)
                    cmbMOccupation.Enabled = false;
            }

            if (mbIsEditMode)
            {
                rdoNoAccept.Checked = false;
                rdoAccept.Checked = true;
                txtAdmissionCoordinator.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["AdmissionCoordinator"].ToString();
                trPTAAssociation.Visible = false;
                trPTAControls.Visible = false;
                trAdmissionCoordinator.Visible = false;
                trAssureNotice.Visible = false;
                trParentConsent.Visible = false;
                trAccept.Visible = false;
                trNoAccept.Visible = false;
                trImportant.Visible = false;
                btnSiblingSubmit.Visible = false;
            }
            else
            {
                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
                {
                    trPTAAssociation.Visible = true;
                    trPTAControls.Visible = true;

                    if (sIsOnline == Constants.S_NO)
                        trAdmissionCoordinator.Visible = true;

                    trAssureNotice.Visible = true;
                    trAccept.Visible = true;
                    trNoAccept.Visible = true;
                    trImportant.Visible = true;

                    if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
                    {
                        trParentConsent.Visible = true;
                        hidShowParentConsentRestriction.Value = Constants.S_YES;
                    }
                }
            }
        }
        if (Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] != null)
        {
            if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt() || ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSN.ToInt() || ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
            {
                chkAddSiblingDetails.Enabled = false;
                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPS.ToInt())
                {
                    var iLivingLocationId = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["LivingLocationId"].ToInt();
                    if (iLivingLocationId == 16) // sibling
                        chkAddSiblingDetails.Checked = true;
                    else
                        chkAddSiblingDetails.Checked = false;
                }
                else if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSN.ToInt() || ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                {
                    chkAddSiblingDetails.Enabled = true;
                    //var iResidenceTypeId = oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["ResidenceTypeId"].ToInt();
                    //if (iResidenceTypeId == 10) // sibling
                    //    chkAddSiblingDetails.Checked = true;
                    //else
                    //    chkAddSiblingDetails.Checked = false;
                }

                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStandardId"].ToString().Trim() != Constants.S_ZERO && oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStandardId"].ToString().Trim() != string.Empty)
                {
                    chkAddSiblingDetails.Checked = true;
                    cmbStandard.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStandardId"].ToString();
                    cmbStandard_SelectedIndexChanged(cmbStandard, null);
                    cmbDivision.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingDivisionId"].ToString();
                    txtSiblingName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStudentName"].ToString();
                }
            }
            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows.Count > Constants.I_ZERO &&  oDataSet.Tables[AdmissionMasterData.I_TABLE_STUDENT_DETAILS].Rows[0]["Sibling_Student_Admission_Id"].ToString() != "0")
            {
                txtFSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Last_Name"].ToString();
                txtFName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["First_Name"].ToString();
                txtFFatherName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Middle_Name"].ToString();
                txtFQuali.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Educational_Qualification"].ToString();
                txtFMotherTounge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Mother_Tongue"].ToString();
                txtFLangSpoken.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["OtherSpokenLanguages"].ToString();
                cmbFReligion.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Religion"].ToString();
                cmbFOccupation.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Occupation"].ToString();
                txtFIncome.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["AnnualIncome"].ToString();
                txtFCompany.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Company_Name"].ToString();
                txtFOccDetails.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Occupation_Details"].ToString();
                txtFOffPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Office_Phone_Number"].ToString();
                txtFEmail.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["EmailAddress"].ToString();
                txtFFaxNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["FaxNumber"].ToString();
                txtFAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["AadharCardNumber"].ToString();

                txtFNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["NameAsPerAadharCard"].ToString();
                
                txtFSector.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Sector"].ToString();
                txtFNationality.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["Nationality"].ToString();
                txtFOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[0]["OfficeAddress"].ToString();
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows.Count > 1)
                {
                    txtMSurname.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Last_Name"].ToString();
                    txtMName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["First_Name"].ToString();
                    txtMHName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Middle_Name"].ToString();
                    txtMQuali.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Educational_Qualification"].ToString();
                    txtMMotherTounge.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Mother_Tongue"].ToString();
                    txtMLangSpoken.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["OtherSpokenLanguages"].ToString();
                    cmbMReligion.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Religion"].ToString();
                    cmbMOccupation.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Occupation"].ToString();
                    txtMIncome.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["AnnualIncome"].ToString();
                    txtMCompany.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Company_Name"].ToString();
                    txtMOccDetails.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Occupation_Details"].ToString();
                    txtMOffPhone.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Office_Phone_Number"].ToString();
                    //txtMEmail.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["EmailAddress"].ToString();
                    txtMFaxNo.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["FaxNumber"].ToString();
                    txtMAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["AadharCardNumber"].ToString();
                    txtMNameOnAadharCard.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["NameAsPerAadharCard"].ToString();
                    txtMSector.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Sector"].ToString();
                    txtMNationality.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["Nationality"].ToString();
                    txtMOrgAddress.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_DETAILS].Rows[1]["OfficeAddress"].ToString();

                    if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                    {
                        if (oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["TwinsSelection"].ToInt() == Constants.I_ONE)
                            chkIsTwins.Checked = true;

                        cmbStandard.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStandardId"].ToString();
                        cmbStandard.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingDivisionId"].ToString();

                        txtSiblingName.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_MOTHER_DATA].Rows[0]["SiblingStudentName"].ToString();
                    }
                }
                if (oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_IN_EVENTS] != null &&
                    oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_IN_EVENTS].Rows.Count > 0)
                {
                    for (int iCount = 0; iCount < oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_IN_EVENTS].Rows.Count; iCount++)
                    {
                        Int32 iChklstCount = Convert.ToInt32(oDataSet.Tables[AdmissionMasterData.I_TABLE_PARENT_IN_EVENTS].Rows[iCount]["AdmissionParentTeacherAsssociationID"]);
                        ListItem oli = chklstEvents.Items.FindByValue(iChklstCount.ToString());
                        oli.Selected = true;
                    }
                }
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows.Count > Constants.I_ZERO)
            {
                txtBName1.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Name1"].ToString();
                txtBAge1.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Age1"].ToString();
                txtBInstitution1.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Institution1"].ToString();
                txtBStandard1.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["StandardName1"].ToString();
                txtBName2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Name2"].ToString();
                txtBAge2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Age2"].ToString();
                txtBInstitution2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["Institution2"].ToString();
                txtBStandard2.Text = oDataSet.Tables[AdmissionMasterData.I_TABLE_BROTHER_DETAILS].Rows[0]["StandardName2"].ToString();
            }

            if (oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows.Count > Constants.I_ZERO)
            {
                cmbStream.SelectedValue = oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows[0]["StreamId"].ToString();
                int iStreamId = oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows[0]["StreamId"].ToInt();
                int iGroupId = oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows[0]["GroupId"].ToInt();
                string sOptionalSubject = oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows[0]["OptionalSubjects"].ToString();
                string sCompitativeExam = oDataSet.Tables[AdmissionMasterData.I_TABLE_STREAMWISE_Subject_DETAILS].Rows[0]["CompitativeExam"].ToString();

                if (hidStandardName.Value == "9")
                {
                    tr9thSubjectCombination.Visible = true;

                    if (sOptionalSubject == "1")
                        rdo9th_Hindi.Checked = true;
                    else if (sOptionalSubject == "2")
                        rdo9th_Marathi.Checked = true;
                    else if (sOptionalSubject == "3")
                        rdo9th_Sanskrit.Checked = true;

                    if (sCompitativeExam == "1")
                        rdo9th_MathsStd.Checked = true;
                    else if (sCompitativeExam == "2")
                        rdo9th_MathsBasic.Checked = true;
                }

                if (iStreamId == Constants.I_ONE)
                {   
                    if (iGroupId == Constants.I_ONE)
                    {
                        rdoStream_SciGroupOne.Checked = true;

                        if (sOptionalSubject == "1")
                            rdoStream_SciGr1PhyEdu.Checked = true;
                        else
                            rdoStream_SciGr1CompSci.Checked = true;

                        if (sCompitativeExam != string.Empty)
                        {
                            string[] sExam = sCompitativeExam.Split(',');
                            int iExam1 = Constants.I_ZERO;
                            int iExam2 = Constants.I_ZERO;
                            if (sExam.Length > Constants.I_ZERO)
                            {
                                iExam1 = sExam[0].ToInt();
                                iExam2 = sExam[1].ToInt();
                            }

                            if (iExam1 != Constants.I_ZERO)
                                chkStream_SciGr1JEE.Checked = true;

                            if (iExam2 != Constants.I_ZERO)
                                chkStream_SciGr1ExtraCo.Checked = true;
                        }
                    }
                    else
                    {
                        rdoStream_SciGroupTwo.Checked = true;

                        if (sOptionalSubject == "1")
                            rdoStream_SciGr2PhyEdu.Checked = true;
                        else
                            rdoStream_SciGr2CompSci.Checked = true;

                        if (sCompitativeExam != string.Empty)
                        {
                            string[] sExam = sCompitativeExam.Split(',');
                            int iExam1 = Constants.I_ZERO;
                            int iExam2 = Constants.I_ZERO;
                            if (sExam.Length > Constants.I_ZERO)
                            {
                                iExam1 = sExam[0].ToInt();
                                iExam2 = sExam[1].ToInt();
                            }

                            if (iExam1 != Constants.I_ZERO)
                                chkStream_SciGr2Neet.Checked = true;

                            if (iExam2 != Constants.I_ZERO)
                                chkStream_SciGr2ExtraCO.Checked = true;
                        }
                    }
                }
                else if (iStreamId == 2) // Commerse
                {
                    if (sOptionalSubject == "1")
                        rdoStream_ComMaths.Checked = true;
                    else
                        rdoStream_ComPhyEdu.Checked = true;

                    if (sCompitativeExam != string.Empty)
                    {
                        string[] sExam = sCompitativeExam.Split(',');
                        int iExam1 = Constants.I_ZERO;
                        int iExam2 = Constants.I_ZERO;
                        if (sExam.Length > Constants.I_ZERO)
                        {
                            iExam1 = sExam[0].ToInt();
                            iExam2 = sExam[1].ToInt();
                        }

                        if (iExam1 != Constants.I_ZERO)
                            chkStream_ComCA.Checked = true;

                        if (iExam2 != Constants.I_ZERO)
                            chkStream_ComExtraCo.Checked = true;
                    }
                }
                else if (iStreamId == 3) //Art
                {
                    if (sOptionalSubject != string.Empty)
                    {
                        string[] sOptionalSub = sOptionalSubject.Split(',');
                        int iOptSub1 = Constants.I_ZERO;
                        int iOptSub2 = Constants.I_ZERO;
                        if (sOptionalSub.Length > Constants.I_ZERO)
                        { 
                            iOptSub1 = sOptionalSub[0].ToInt();
                            iOptSub2 = sOptionalSub[1].ToInt();

                            if (iOptSub1 == 1)
                                rdoStream_ArtLegalSci.Checked = true;
                            else if (iOptSub2 == 2)
                                rdoStream_ArtPhyEdu.Checked = true;

                            if (iOptSub2 == 1)
                                rdoStream_ArtGerman.Checked = true;
                            else if (iOptSub2 == 2)
                                rdoStream_ArtEconomics.Checked = true;
                        }
                    }
                    if (sCompitativeExam != string.Empty)
                    {
                        string[] sExam = sCompitativeExam.Split(',');
                        int iExam1 = Constants.I_ZERO;
                        int iExam2 = Constants.I_ZERO;
                        if (sExam.Length > Constants.I_ZERO)
                        {
                            iExam1 = sExam[0].ToInt();
                            iExam2 = sExam[1].ToInt();
                        }

                        if (iExam1 != Constants.I_ZERO)
                            chkStream_ArtClat.Checked = true;

                        if (iExam2 != Constants.I_ZERO)
                            chkStream_ArtExtraCo.Checked = true;
                    }
                }
                else if (iStreamId == 4)
                {
                    if (sCompitativeExam == "1")
                        rdoStream_AbrodEduNo.Checked = true;
                    else if (sCompitativeExam == "2")
                        rdoStream_AbrodEduYes.Checked = true;
                }
            }
        }
    }

    private void FillStandardCombo()
    {
        var oStandardCollectionBL = new StandardCollectionBL(ConfigurationManager.AppSettings["SchoolID"].ToInt(), hidNewAcadamicYearID.Value.ToInt());
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandardsForSiblingDetails();
        cmbStandard.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && sIsOnline == Constants.S_NO)
        {
            if ((moUserRole == Constants.UserRoles.Admin
                ||moUserRole == Constants.UserRoles.Supervisor
                || moUserRole == Constants.UserRoles.Teacher) && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
            {
                divAdmissionSteps.Visible = false;
                btnSubmit.Text = "Submit";
                if(!mbIsEditMode)
                    btnSiblingSubmit.Visible = true;
            }
            else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
                divAdmissionSteps.Visible = false;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        {
            hidShowAnnualIncomeValidation.Value = Constants.S_YES;
            txtFSurname.Enabled = false;
            txtFName.Enabled = false;
            txtFFatherName.Enabled = false;
            txtMSurname.Enabled = false;
            txtMName.Enabled = false;
            txtMHName.Enabled = false;
        }

        txtFSurname.Focus();
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmAction(this)) return false; else this.style.display = 'none';");
        rdoAccept.Attributes.Add("onclick", "return enabledisablecontrols('" + rdoAccept.ClientID + "');");
        rdoNoAccept.Attributes.Add("onclick", "return enabledisablecontrols('" + rdoNoAccept.ClientID + "');");
    }

    /// <summary>
    /// This method is used to save student parent details.
    /// </summary>
    private DataTable SaveStudentParentsDetails()
    {
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        string sAdmissionParentDetailsXML = GetAdmissionParentDtlsXML();
        string sEventParentTeacherAssoDetailsXML = GetEventParentTeacherAssoDetailsXML();
        string sAdmissionAdditionalDetailsXML = GetAdmissionAddtnlDtlsXML();
        string sStudentBotherAndSisterDetailsXML = GetBrotherSisterDetailsXML();
        string sAdmissionHealthDetailsXML = null;
        string sStudentStreamwiseSubjectDetails = null;
        if (hidIsSubjectSectionApplicable.Value == "Y")
            sStudentStreamwiseSubjectDetails = GetStreamwiseSubjectDetailsXML();
        
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        oStudentAdmissionsBL.AdmissionDetails = sAdmissionParentDetailsXML;
        oStudentAdmissionsBL.AdmissionParentDetails = sEventParentTeacherAssoDetailsXML;
        oStudentAdmissionsBL.AdmissionAdditionalDetails = sAdmissionAdditionalDetailsXML;
        oStudentAdmissionsBL.AmissionHealthDetails = sAdmissionHealthDetailsXML;
        oStudentAdmissionsBL.StudentBrotherAndSisterDetails = sStudentBotherAndSisterDetailsXML;
        oStudentAdmissionsBL.StudentsStreamWiseSubjectDetails = sStudentStreamwiseSubjectDetails;
        oStudentAdmissionsBL.NewStudentAdmissionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);
        if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && sIsOnline == Constants.S_NO)
        {
            if (moUserRole == Constants.UserRoles.Admin
                ||moUserRole== Constants.UserRoles.Supervisor
                || moUserRole == Constants.UserRoles.Teacher)
                oStudentAdmissionsBL.IsOnlineAdmission = false;
        }
        else
            oStudentAdmissionsBL.IsOnlineAdmission = true;
        return oStudentAdmissionsBL.InsertStudentAdmissions(iSchoolId, miEnqId, mbIsEditMode, Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]),null);
    }

    private string GetAdmissionAddtnlDtlsXML()
    {
        const string S_ELEMENT = "element";

        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissionAdditionalDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionAdditionals", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionAdditional", "");

        return root.InnerXml;
    }

    private string GetStreamwiseSubjectDetailsXML()
    {
        const string S_ELEMENT = "element";
        int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iStudentAdmisssionID = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);

        int iStreamId = cmbStream.SelectedValue.ToInt();
        int iGroupId = Constants.I_ZERO;
        string sOptionalSubjectID = string.Empty;
        string sCompitativeExamIds = string.Empty;
        string sCompulsorySubjet = string.Empty;

        if (iStreamId == Constants.I_ONE) // Science Stream
        {
            if (rdoStream_SciGroupOne.Checked)
                iGroupId = 1;
            else
                iGroupId = 2;

            if (iGroupId == 1)
            {
                if (rdoStream_SciGr1PhyEdu.Checked)
                    sOptionalSubjectID = Constants.S_ONE;
                else
                    sOptionalSubjectID = Constants.S_TWO;


                if (chkStream_SciGr1JEE.Checked && chkStream_SciGr1ExtraCo.Checked)
                    sCompitativeExamIds = "1,2";
                else if (chkStream_SciGr1JEE.Checked)
                    sCompitativeExamIds = "1";
                else if (chkStream_SciGr1ExtraCo.Checked)
                    sCompitativeExamIds = "2";

                sCompulsorySubjet = "English, Physics, Chemistry, Maths";
            }
            else if (iGroupId == 2)
            {
                if (rdoStream_SciGr2PhyEdu.Checked)
                    sOptionalSubjectID = Constants.S_ONE;
                else
                    sOptionalSubjectID = Constants.S_TWO;


                if (chkStream_SciGr2Neet.Checked && chkStream_SciGr2ExtraCO.Checked)
                    sCompitativeExamIds = "1,2";
                else if (chkStream_SciGr2Neet.Checked)
                    sCompitativeExamIds = "1";
                else if (chkStream_SciGr2ExtraCO.Checked)
                    sCompitativeExamIds = "2";

                sCompulsorySubjet = "English, Physics, Chemistry, Biology";
            }
        }
        else if (iStreamId == 2) // Commarce Stream
        {
            iGroupId = Constants.I_ONE;

            if (rdoStream_ComMaths.Checked)
                sOptionalSubjectID = Constants.S_ONE;
            else
                sOptionalSubjectID = Constants.S_TWO;


            if (chkStream_ComCA.Checked && chkStream_ComExtraCo.Checked)
                sCompitativeExamIds = "1,2";
            else if (chkStream_ComCA.Checked)
                sCompitativeExamIds = "1";
            else if (chkStream_ComExtraCo.Checked)
                sCompitativeExamIds = "2";

            sCompulsorySubjet = "English, Business Studies, Accounts, Economics";
        }
        else if (iStreamId == 3) // Atrs Stream
        {
            iGroupId = Constants.I_ONE;
            if (rdoStream_ArtLegalSci.Checked && rdoStream_ArtGerman.Checked)
                sOptionalSubjectID = "1,1";
            else if (rdoStream_ArtLegalSci.Checked && rdoStream_ArtEconomics.Checked)
                sOptionalSubjectID = "1,2";
            else if (rdoStream_ArtPhyEdu.Checked && rdoStream_ArtEconomics.Checked)
                sOptionalSubjectID = "2,2";
            else if (rdoStream_ArtPhyEdu.Checked = rdoStream_ArtGerman.Checked)
                sOptionalSubjectID = "2,1";

            if (chkStream_ArtClat.Checked && chkStream_ArtExtraCo.Checked)
                sCompitativeExamIds = "1,2";
            else if (chkStream_ArtClat.Checked)
                sCompitativeExamIds = "1";
            else if (chkStream_ArtExtraCo.Checked)
                sCompitativeExamIds = "2";

            sCompulsorySubjet = "English, History, Psychology";
        }
        else if (iStreamId == 4) // Abroad Education
        {
            iGroupId = Constants.I_ONE;
            sCompulsorySubjet = "SAT";

            if (rdoStream_AbrodEduNo.Checked)
                sCompitativeExamIds = "1";
            else
                sCompitativeExamIds = "2";
        }
        else if (hidStandardName.Value == "9")
        {
            iGroupId = Constants.I_ONE;
            sCompulsorySubjet = "English, Science, SST";

            if (rdo9th_Hindi.Checked)
                sOptionalSubjectID = "1";
            else if (rdo9th_Marathi.Checked)
                sOptionalSubjectID = "2";
            else if (rdo9th_Sanskrit.Checked)
                sOptionalSubjectID = "3";

            if (rdo9th_MathsStd.Checked)
                sCompitativeExamIds = "1";
            else if (rdo9th_MathsBasic.Checked)
                sCompitativeExamIds = "2";
        }


        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StreamWiseSubjectDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StreamWiseSubjectDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StreamWiseSubjectDetails", "");

        string sAtrrName = "StudentAdmissionId";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iStudentAdmisssionID.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SchoolID";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iSchoolId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "StreamId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iStreamId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "GroupId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iGroupId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CompulsorySubjects";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = sCompulsorySubjet;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OptionalSubjectId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = sOptionalSubjectID;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CompitativeExam";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = sCompitativeExamIds.ToString();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentBotherAndSisterDetails", "");

        // return the string generated.
        return root.InnerXml;
    }

    private string GetBrotherSisterDetailsXML()
    {
        const string S_ELEMENT = "element";
        int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iStudentAdmisssionID = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentBotherAndSisterDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentBotherAndSisterDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentBotherAndSisterDetails", "");
       
        string sAtrrName = "StudentAdmissionId";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iStudentAdmisssionID.ToString();
        oXmlNode.Attributes.Append(attr);
        
        sAtrrName = "SchoolID";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iSchoolId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherName1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBName1.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherAge1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBAge1.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "NameOfInstitute1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBInstitution1.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherStandard1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBStandard1.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherName2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBName2.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherAge2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBAge2.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "NameOfInstitute2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBInstitution2.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BrotherStandard2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBStandard2.Text.TrimAll();
        oXmlNode.Attributes.Append(attr);      

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentBotherAndSisterDetails", "");

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to generate XML format of parent's details.
    /// </summary>
    /// <returns></returns>
    private string GetAdmissionParentDtlsXML()
    {
        const char C_FATHER = 'F';
        const char C_MOTHER = 'M';
        const string S_ELEMENT = "element";

        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissionParentDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionParentDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionParentDetail", "");

        string sAtrrName = "Student_Admission_Id";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Or_Mother";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = C_FATHER.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Admission_Parent_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidMParentID.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFFatherName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFSurname.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Educational_Qualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFQuali.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Tongue";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFMotherTounge.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Other_Lang_Spoken";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFLangSpoken.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Religion";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbFReligion.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Nationality";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFNationality.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbFOccupation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AnnualIncome";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFIncome.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Company_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFCompany.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation_Details";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFOccDetails.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Office_Phone_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFOffPhone.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AdmissionCoordinator";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAdmissionCoordinator.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FaxNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFFaxNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Sector";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFSector.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IsDeleted";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "N";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DateOfBirth";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFDOB.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFAadharCard.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "NameAsPerAadharCard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFNameOnAadharCard.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PanNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFPanNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);        

        sAtrrName = "MobileNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFMobileNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BloodGroup";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbFBloodGroup.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OfficeAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFOrgAddress.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionParentDetail", "");

        sAtrrName = "Student_Admission_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Or_Mother";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = C_MOTHER.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Admission_Parent_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidMParentID.Value;
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

        sAtrrName = "Educational_Qualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMQuali.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Tongue";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMMotherTounge.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Other_Lang_Spoken";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMLangSpoken.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Religion";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbMReligion.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Nationality";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMNationality.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbMOccupation.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AnnualIncome";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMIncome.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Company_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMCompany.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation_Details";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMOccDetails.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Office_Phone_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMOffPhone.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FaxNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMFaxNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Sector";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMSector.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AdmissionCoordinator";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAdmissionCoordinator.Text;
        oXmlNode.Attributes.Append(attr);        

        sAtrrName = "IsDeleted";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "N";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DateOfBirth";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMDOB.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMAadharCard.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "NameAsPerAadharCard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMNameOnAadharCard.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PanNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMPanNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MobileNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMMobileNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BloodGroup";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbMBloodGroup.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OfficeAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMOrgAddress.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        int iWtListId = 0;
        if(QueryString["WtListId"] != null && QueryString["WtListId"].ToString().Trim() != string.Empty)
            iWtListId = QueryString["WtListId"].ToInt();

        sAtrrName = "WtListId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iWtListId.ToString();
        oXmlNode.Attributes.Append(attr);

        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if ((iSchoolId == Constants.SchoolId.PPS.ToInt() || iSchoolId == Constants.SchoolId.PPSN.ToInt() || iSchoolId == Constants.SchoolId.PPSH.ToInt()) && chkAddSiblingDetails.Checked)
        {
            sAtrrName = "SiblingStandard";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = cmbStandard.SelectedValue;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "SiblingDivision";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = cmbDivision.SelectedValue;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "SiblingStudentName";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = txtSiblingName.Text.Trim();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "IsTwins";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Convert.ToString(chkIsTwins.Checked ? 1 : 0);
            oXmlNode.Attributes.Append(attr);
        }
        else
        {
            sAtrrName = "SiblingStandard";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Constants.S_ZERO;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "SiblingDivision";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Constants.S_ZERO;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "SiblingStudentName";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = string.Empty;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "IsTwins";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Constants.S_ZERO;
            oXmlNode.Attributes.Append(attr);
        }

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to generate XML for parents
    /// contribution in event in Parent Teacher Association.
    /// </summary>
    /// <returns></returns>
    private string GetEventParentTeacherAssoDetailsXML()
    {
        const string S_ELEMENT = "element";
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iStudentAdmisssionID = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("EventsParentTeacherAssociationDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "EventsParentTeacherAssociationDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "EventsParentTeacherAssociationDetails", "");

        for (int iEventCount = 0; iEventCount < chklstEvents.Items.Count; iEventCount++)
        {
            if (chklstEvents.Items[iEventCount].Selected)
            {
                string sAtrrName = "StudentAdmissionId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = iStudentAdmisssionID.ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "EventType";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = chklstEvents.Items[iEventCount].Value;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "SchoolID";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = iSchoolId.ToString();
                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            oXmlNode = oDoc.CreateNode(S_ELEMENT, "EventsParentTeacherAssociationDetails", "");
        }
        // return the string generated.
        return root.InnerXml;
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
    /// This method is used to send SMS of completion admission form to parents with login and password details. 
    /// </summary>
    /// <param name="aoDTStudentDetails"></param>
    /// <returns></returns>
    private string SendSMS(DataTable aoDTStudentDetails)
    {
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        int iSMSType = 0;
        int iStudentAdmissionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID]);
        if (aoDTStudentDetails != null && aoDTStudentDetails.Rows.Count >0 
                                        && aoDTStudentDetails.Columns.Count == 3)
        {
            Hashtable moManualMobileNo = new Hashtable();
            string sMobileNumber = Convert.ToString(aoDTStudentDetails.Rows[0]["MobileNumber"]);
            string sForm_Number = Convert.ToString(aoDTStudentDetails.Rows[0]["Form_Number"]);
            int iAdminlID = Convert.ToInt32(aoDTStudentDetails.Rows[0]["AdminlID"]);
            moManualMobileNo[sMobileNumber] = sMobileNumber;
			int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"].ToInt());
            int iSmsID = Convert.ToInt32(Constants.SMSTemplate.OnlineAdmissionLoginDetailsSMS);
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsID, iSchoolId);
            if (oDTSmsTemplate.Rows.Count != 0)
            {
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                {
                    sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                    if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
                }
                if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                    iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
            }

            SchoolBL oSchoolBL = new SchoolBL(iSchoolId);
            string sDisplayText = sMobileNumber;
            sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", sForm_Number).Replace("%PASSWORD%", sMobileNumber);
            SMS oSMS = new SMS();
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = iAdminlID;
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSType = iSMSType;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.AcademicYearID = Convert.ToInt32(hidNewAcadamicYearID.Value);
			oSMS.SchoolID = ConfigurationManager.AppSettings["SchoolID"].ToInt();
            oSMS.DisplayText = sDisplayText;
            oSMS.ToManualNumbers = moManualMobileNo;
            oSMS.Send();
            return "iAdmissionId=" + iStudentAdmissionId + "&Form_Number=" + sForm_Number + "&Mobile_Number=" + sMobileNumber;
        }
        return string.Empty;
    }

    /// <summary>
    /// This method is used to set validation state.
    /// </summary>
    private void SetValidationState()
    {
        if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {
            hidValidateAdissionCoordinator.Value = Constants.S_ZERO;

            txtFEmail.Style.Add("background-color", "#ffffa0");
            txtMEmail.Style.Add("background-color", "#ffffa0");
            reqValtxtFEmail.Enabled = true;
            CustomValidator10.Enabled = false;
            reqValMotherEmailAddress.Enabled = true;
        }
        else
        {
            if (sIsOnline == Constants.S_NO)
                hidValidateAdissionCoordinator.Value = Constants.S_ONE;
            else
                hidValidateAdissionCoordinator.Value = Constants.S_ZERO;
        }
    }    

    #endregion  " Private Methods "    
    
}
