using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Data;
using BusinessLogic;
using Utility;
using SchoolEntities;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Xml;
using System.Threading;
using System.Collections.Specialized;
public partial class StudentRegistrationDetails : SchoolBase
{
    #region " Constants "
    
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
    private const string S_UPDATE_MESSAGE = "Admission process details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Admission process details saved successfully !!!";

    #endregion

    #region DataMember

    private SchoolEnquiryBL moSchoolEnquiryBL;

    #endregion

    #region Event(s)

    public int SchoolId
    {
        get
        {
            return (miSchoolId > 0) ? miSchoolId : ConfigurationManager.AppSettings["SchoolId"].ToInt();
        }
    }

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

            string sFromUrl = string.Empty;
            if (!IsPostBack)
                sFromUrl = GetFromPageUrl();

            if (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
            else
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/OnlineAdmissionNew.master";            
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
    /// This event is used to load all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSchoolEnquiryBL = new SchoolEnquiryBL();
            if (!IsPostBack)
            {
                GetIsEnquiryValue();
                ReadQueryString();
                LoadPageControls(sender,e);
                FillControls();
                SetJavascriptAttributes();

                SetStreamDetails();
                SetLastSchoolState();
                SetEnquiryVisibility();
            }
            hidSchoolId.Value = SchoolId.ToString();
            hidSNSSchoolId.Value = Constants.SchoolId.SNS.ToInt().ToString();
         }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save student details in table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                moSchoolEnquiryBL = new SchoolEnquiryBL();

                StudentRegistration oStudentRegistration = Populate();
                string sStudentStreamwiseSubjectDetails = null;
                if (hidIsSubjectSectionApplicable.Value == "Y")
                    sStudentStreamwiseSubjectDetails = GetStreamwiseSubjectDetailsXML();

                int iAccYearId = miAcademicYearId;
                if (hidAcademicYearId.Value != string.Empty)
                    iAccYearId = hidAcademicYearId.Value.ToInt();

                DataTable dtResult = moSchoolEnquiryBL.SaveStudentRegistrationDetails(SchoolId, iAccYearId, CommonUtility.GenerateXml(oStudentRegistration), hidStudentAdmisssionID.Value.ToInt(), miUserId, hidEnquieryId.Value.ToInt(), sStudentStreamwiseSubjectDetails, hidIsEnquiry.Value.ToInt());  ////

                if (SchoolId != Constants.SchoolId.SNS.ToInt())
                {
                    if (oStudentRegistration.AdmissionId == 0)
                        base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
                    else
                        base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
                }
                else
                {
                    if (miSchoolId != 0)
                    {
                        string sQueryString = CommonUtility.EncryptQuerystring("AcademicYearId=" + iAccYearId);
                        Response.Redirect("NewStudentAdmisionsListUI.aspx?" + sQueryString, false);
                    }
                    else
                    {
                        //if (dtResult != null && dtResult.Rows.Count > 0)
                        //{
                        //    int iAdmissionId = dtResult.Rows[0]["AdmissionId"].ToInt();
                        //    string sFormNumber = dtResult.Rows[0]["Form_Number"].ToString();
                        //    string sMobileNumber = dtResult.Rows[0]["MobileNumber"].ToString();
                        //    string sQueryString = CommonUtility.EncryptQuerystring("AdmissionId=" + iAdmissionId + "&Form_Number=" + sFormNumber + "&Mobile_Number=" + sMobileNumber + "&IsInternalAdmission=N");

                        //    Response.Redirect("~/RITeSchool/Admission/RegistrationThankYouUI.aspx?" + sQueryString, false);
                        //}

                        string sEnqNo = string.Empty;
                        if(dtResult.Rows.Count > 0 && dtResult.Rows[0]["EnquiryNumString"] != DBNull.Value)
                            sEnqNo = dtResult.Rows[0]["EnquiryNumString"].ToString();

                        string sQueryString = CommonUtility.EncryptQuerystring("EnquiryNo=" + sEnqNo+"&IsEnquiry=1");

                        Response.Redirect("~/RITeSchool/Admission/RegistrationThankYouUI.aspx?" + sQueryString, false);
                    }

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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to change stream combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStream_SelectedIndexChanged(object sender, EventArgs e)  //
    {
        try
        {
            int StreamId = cmbStream.SelectedValue.ToInt();
             //$('[id*=rdoStream_]').prop('checked', false);
             //   $('[id*=chkStream_]').prop('checked', false);

                if (StreamId == 1) {
                   trScienceStream.Attributes.Add("style","display:block");
                     trCommerceStream.Attributes.Add("style","display:none");
                     trArtsStream.Attributes.Add("style","display:none");
                     trAbroadEducation.Attributes.Add("style","display:none");
                    
                }
                else if (StreamId == 2) {
                     trScienceStream.Attributes.Add("style","display:none");
                     trCommerceStream.Attributes.Add("style","display:block");
                     trArtsStream.Attributes.Add("style","display:none");
                     trAbroadEducation.Attributes.Add("style","display:none");
                }
                else if (StreamId == 3) {
                     trScienceStream.Attributes.Add("style","display:none");
                     trCommerceStream.Attributes.Add("style","display:none");
                     trArtsStream.Attributes.Add("style","display:block");
                     trAbroadEducation.Attributes.Add("style","display:none");
                }
                else if (StreamId == 4) {
                     trScienceStream.Attributes.Add("style","display:none");
                     trCommerceStream.Attributes.Add("style","display:none");
                     trArtsStream.Attributes.Add("style","display:none");
                     trAbroadEducation.Attributes.Add("style","display:block");
                }
                else if (StreamId == 0) {
                   
                    trScienceStream.Attributes.Add("style", "display:none");
                    trCommerceStream.Attributes.Add("style", "display:none");
                    trArtsStream.Attributes.Add("style", "display:none");
                    trAbroadEducation.Attributes.Add("style", "display:none");
                }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to change combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStandardName.Value = cmbStd.SelectedItem.Text;
            DataTable oDtMinMaxDate = ViewState["MinMaxDate"] as DataTable;
            GetMinMaxDate(oDtMinMaxDate, cmbStd.SelectedValue.ToInt());

            SetStreamDetails();
            SetAadharCardValidationState();
            SetLastSchoolState();
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

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set Aadhar card validators.
    /// </summary>
    private void SetAadharCardValidationState()
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if (iSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            List<string> lstStd = new List<string> { "PLAY GROUP","NURSERY", "LKG", "UKG", "1" };
            if (!lstStd.Contains(cmbStd.SelectedItem.Text.ToUpper()))
            {
                txtAadharCardNumber.Style.Add("background-color", "#ffffa0");
                hidValidateAadharCard.Value = Constants.S_YES;
            }
            else
            {
                txtAadharCardNumber.Style.Add("background-color", "white");
                hidValidateAadharCard.Value = Constants.S_NO;
            }
        }
        else
        {
            txtAadharCardNumber.Style.Add("background-color", "white");
            hidValidateAadharCard.Value = Constants.S_NO;
        }
    }

    /// <summary>
    /// This method is used to populate student registration details.
    /// </summary>
    /// <returns></returns>
    private StudentRegistration Populate()
    {
        StudentRegistration oStudentRegistration = new StudentRegistration();

        oStudentRegistration.AdmissionId = hidStudentAdmisssionID.Value.ToInt();
      oStudentRegistration.AdmissinAcademicYearId = cmbYear.SelectedValue.ToInt();
        oStudentRegistration.standardId = cmbStd.SelectedValue.ToInt();
        oStudentRegistration.FirstName = txtFirstName.Text.Trim();
        oStudentRegistration.MiddleName = txtMiddleName.Text.Trim();
        oStudentRegistration.LastName = txtLastName.Text.Trim();
        if (rdoMale.Checked)
            oStudentRegistration.Gender = "M";
        else if (rdoFemale.Checked)
            oStudentRegistration.Gender = "F";
        oStudentRegistration.DateOfBirth = txtCalDobPopup.Text.ToDateTime();
        oStudentRegistration.BirthPlace = txtBirthPlace.Text.Trim();
        oStudentRegistration.BirthTaluka = txtBirthTaluka.Text.Trim();
        oStudentRegistration.BirthDistrict = txtBirthDistrict.Text.Trim();
        oStudentRegistration.LastSchoolName = txtSchoolName.Text.Trim();
        oStudentRegistration.HouseName = txtHouseNo.Text.Trim();
        oStudentRegistration.LandMark = txtLandmark.Text.Trim();
        oStudentRegistration.MainArea = txtMainArea.Text.Trim();
        oStudentRegistration.City = txtCity.Text.Trim();
        oStudentRegistration.Taluka = txttaluka.Text.Trim();
        oStudentRegistration.District = txtDistrict.Text.Trim();
        oStudentRegistration.Address = txtAddress.Text.Trim();
        oStudentRegistration.FFirstName = txtFFirstName.Text.Trim();
        oStudentRegistration.FMiddleName = txtFMiddleName.Text.Trim();
        oStudentRegistration.FLastName = txtFLastName.Text.Trim();
        oStudentRegistration.MFirstName = txtMFirstName.Text.Trim();
        oStudentRegistration.MMiddleName = txtMMiddleName.Text.Trim();
        oStudentRegistration.MLastName = txtMLastName.Text.Trim();
        oStudentRegistration.FQualification = txtFQuali.Text.Trim();
        oStudentRegistration.MQualification = txtMQuali.Text.Trim();
        oStudentRegistration.FOccupation = cmbFOccupation.SelectedValue.ToInt();
        oStudentRegistration.MOccupation = cmbMOccupation.SelectedValue.ToInt();
        oStudentRegistration.FOrgAddress = txtFOrgAddress.Text.Trim();
        oStudentRegistration.MOrgAddress = txtMOrgAddress.Text.Trim();
        oStudentRegistration.FPhoneNumber = txtFOffPhone.Text.Trim();
        oStudentRegistration.MPhoneNumber = txtMOffPhone.Text.Trim();
        oStudentRegistration.FMobNumber = txtFMobNo.Text.Trim();
        oStudentRegistration.MMobNumber = txtMMobNo.Text.Trim();
        oStudentRegistration.FEmail = txtFEmail.Text.Trim();
        oStudentRegistration.MEmail = txtMEmail.Text.Trim();

        oStudentRegistration.BName1 = txtBName1.Text.Trim();
        if(txtBAge1.Text.Trim() != string.Empty)
            oStudentRegistration.BAge1 = txtBAge1.Text.Trim().ToInt();
        oStudentRegistration.BInstituteName1 = txtBInstitution1.Text.Trim();
        oStudentRegistration.BStandard1 = txtBStandard1.Text.Trim();

        oStudentRegistration.BName2 = txtBName2.Text.Trim();
        if (txtBAge2.Text.Trim() != string.Empty)
            oStudentRegistration.BAge2 = txtBAge2.Text.Trim().ToInt();
        oStudentRegistration.BInstituteName2 = txtBInstitution2.Text.Trim();
        oStudentRegistration.BStandard2 = txtBStandard2.Text.Trim();
        oStudentRegistration.AadharCardNumber = txtAadharCardNumber.Text;
        if (!string.IsNullOrWhiteSpace(txtManualReceiptNo.Text))
            oStudentRegistration.ManualReceiptNo = Convert.ToInt32(txtManualReceiptNo.Text);
        else
            oStudentRegistration.ManualReceiptNo = 0;
        if (!string.IsNullOrWhiteSpace(txtEnqNo.Text))
            oStudentRegistration.EnquiryNo = txtEnqNo.Text.Trim();
        else
            oStudentRegistration.EnquiryNo = null;
        return oStudentRegistration;
    }
    /// <summary>
    /// This method is used to get student streamwise subject details .
    /// </summary>
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
            else if (rdoStream_ComPhyEdu.Checked)
                sOptionalSubjectID = Constants.S_TWO;   
            else if (rdoStream_ComLeagalStudies.Checked)
                sOptionalSubjectID = Constants.S_THREE; 
        
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

    /// <summary>
    /// This method is used to set stream details.
    /// </summary>
    private void SetStreamDetails()
    {
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
                    //cmbStream.Attributes.Add("onChange", "ChangeStreamDetails(this)");//


                }
                else
                    trSNSStrimwiseSubjects.Visible = false;
            }
        }
        else
            trSNSStrimwiseSubjects.Visible = false;
    }

    /// <summary>
    /// This method is used to fill student details controls.
    /// </summary>
    private void FillControls()
    {
        SchoolEnquiryBL oStudentEnquiryBL = new SchoolEnquiryBL();

        int iAccYearId = miAcademicYearId;
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        int enquiryId = hidEnquieryId.Value.ToInt();
        if (hidAcademicYearId.Value != string.Empty)
            iAccYearId = hidAcademicYearId.Value.ToInt();

        bool bIsNew = enquiryId == 0;

        if (hidIsEnquiry.Value.ToInt() == 1 && bIsNew && SchoolId == Constants.SchoolId.SNS.ToInt())
        {
            txtEnqNo.Text = GetNextEnquiryNo(iSchoolId, hidAcademicYearId.Value.ToInt());
        }

        DataTable oDTStudentEnquiryDetails = oStudentEnquiryBL.GetStudentEnquiryDetailsForRegistration(hidEnquieryId.Value.ToInt(), SchoolId, iAccYearId, hidStudentAdmisssionID.Value.ToInt(), hidIsEnquiry.Value.ToInt());

        if (oDTStudentEnquiryDetails.Rows.Count > 0)
        {
            int sStandardId = (oDTStudentEnquiryDetails.Rows[0]["For_std"]).ToInt();
            cmbStd.SelectedValue = sStandardId.ToString();
            txtEnqNo.Text = (!string.IsNullOrWhiteSpace(oDTStudentEnquiryDetails.Rows[0]["Enquiry_No"].ToString()) && oDTStudentEnquiryDetails.Rows[0]["Enquiry_No"].ToString() != null) ? oDTStudentEnquiryDetails.Rows[0]["Enquiry_No"].ToString() : string.Empty;

            txtFirstName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_First_Name"].ToString() : string.Empty;
            txtMiddleName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Middle_Name"].ToString() : string.Empty;
            txtLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Stu_Last_Name"].ToString() : string.Empty;
            string sGender = oDTStudentEnquiryDetails.Rows[0]["Gender"].ToString().Trim().ToUpper();
            rdoMale.Checked = sGender == "M";
            rdoFemale.Checked = sGender == "F";
            DateTime SelectedDate = oDTStudentEnquiryDetails.Rows[0]["DOB"].ToDateTime();
            txtCalDobPopup.Text = SelectedDate.Date.ToString("dd-MMM-yyyy");
            txtAadharCardNumber.Text = oDTStudentEnquiryDetails.Rows[0]["AadharCardNumber"].ToString();
            txtManualReceiptNo.Text = (!string.IsNullOrWhiteSpace(oDTStudentEnquiryDetails.Rows[0]["ManualReceiptNo"].ToString()) && oDTStudentEnquiryDetails.Rows[0]["ManualReceiptNo"].ToInt() > 0) ? oDTStudentEnquiryDetails.Rows[0]["ManualReceiptNo"].ToString() : string.Empty;
            txtFFirstName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Fst_Name"].ToString() : string.Empty;
            txtFMiddleName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Middle_Name"].ToString() : string.Empty;
            txtFLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Last_Name"].ToString() : string.Empty;
            txtMFirstName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Fst_Name"].ToString() : string.Empty;
            txtMMiddleName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Middle_Name"].ToString() : string.Empty;
            txtMLastName.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Last_Name"].ToString() : string.Empty;

            txtFMobNo.Text = oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Father_Mob_No_1"].ToString() : string.Empty;
            txtMMobNo.Text = oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Mother_Mob_No_1"].ToString() : string.Empty;
            txtFEmail.Text = oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Email_Address"].ToString() : string.Empty;
            txtAddress.Text = oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Address"].ToString() : string.Empty;
            txtSchoolName.Text = oDTStudentEnquiryDetails.Rows[0]["LastSchoolName"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["LastSchoolName"].ToString() : string.Empty;

         //   if (hidEnquieryId.Value == "0")
            {
                txtBirthPlace.Text = oDTStudentEnquiryDetails.Rows[0]["BirthPlace"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["BirthPlace"].ToString() : string.Empty;
                txtBirthTaluka.Text = oDTStudentEnquiryDetails.Rows[0]["BirthTaluka"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["BirthTaluka"].ToString() : string.Empty;
                txtBirthDistrict.Text = oDTStudentEnquiryDetails.Rows[0]["BirthDistrict"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["BirthDistrict"].ToString() : string.Empty;
                txtHouseNo.Text = oDTStudentEnquiryDetails.Rows[0]["House_Plot_Name"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["House_Plot_Name"].ToString() : string.Empty;
                txtLandmark.Text = oDTStudentEnquiryDetails.Rows[0]["LandMark"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["LandMark"].ToString() : string.Empty;
                txtMainArea.Text = oDTStudentEnquiryDetails.Rows[0]["MainAreaName"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MainAreaName"].ToString() : string.Empty;
                txtCity.Text = oDTStudentEnquiryDetails.Rows[0]["City"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["City"].ToString() : string.Empty;
                txttaluka.Text = oDTStudentEnquiryDetails.Rows[0]["Taluka"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Taluka"].ToString() : string.Empty;
                txtDistrict.Text = oDTStudentEnquiryDetails.Rows[0]["District"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["District"].ToString() : string.Empty;

                txtFQuali.Text = oDTStudentEnquiryDetails.Rows[0]["FQualification"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["FQualification"].ToString() : string.Empty;
                txtMQuali.Text = oDTStudentEnquiryDetails.Rows[0]["MQualification"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MQualification"].ToString() : string.Empty;
                cmbFOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["FOccupation"].ToString();
                cmbMOccupation.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["MOccupation"].ToString();
                txtFOrgAddress.Text = oDTStudentEnquiryDetails.Rows[0]["FOfficeAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["FOfficeAddress"].ToString() : string.Empty;
                txtMOrgAddress.Text = oDTStudentEnquiryDetails.Rows[0]["MOfficeAddree"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MOfficeAddree"].ToString() : string.Empty;
                txtFOffPhone.Text = oDTStudentEnquiryDetails.Rows[0]["FPhoneNo"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["FPhoneNo"].ToString() : string.Empty;
                txtMOffPhone.Text = oDTStudentEnquiryDetails.Rows[0]["MPhoneNo"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MPhoneNo"].ToString() : string.Empty;
                txtMEmail.Text = oDTStudentEnquiryDetails.Rows[0]["MEmailAddress"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["MEmailAddress"].ToString() : string.Empty;

                txtBName1.Text = oDTStudentEnquiryDetails.Rows[0]["Name1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Name1"].ToString() : string.Empty;
                txtBAge1.Text = oDTStudentEnquiryDetails.Rows[0]["Age1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Age1"].ToString() : string.Empty;
                txtBInstitution1.Text = oDTStudentEnquiryDetails.Rows[0]["Institution1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Institution1"].ToString() : string.Empty;
                txtBStandard1.Text = oDTStudentEnquiryDetails.Rows[0]["StandardName1"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["StandardName1"].ToString() : string.Empty;

                txtBName2.Text = oDTStudentEnquiryDetails.Rows[0]["Name2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Name2"].ToString() : string.Empty;
                txtBAge2.Text = oDTStudentEnquiryDetails.Rows[0]["Age2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Age2"].ToString() : string.Empty;
                txtBInstitution2.Text = oDTStudentEnquiryDetails.Rows[0]["Institution2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["Institution2"].ToString() : string.Empty;
                txtBStandard2.Text = oDTStudentEnquiryDetails.Rows[0]["StandardName2"].ToString() != null ? oDTStudentEnquiryDetails.Rows[0]["StandardName2"].ToString() : string.Empty;

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt() && (cmbStd.SelectedItem.Text.StartsWith("11 ") || cmbStd.SelectedItem.Text.StartsWith("12 "))) // streamwise optional and compulsory subjects
                {

                    cmbStream.SelectedValue = oDTStudentEnquiryDetails.Rows[0]["StreamId"].ToString();
                    int iStreamId = oDTStudentEnquiryDetails.Rows[0]["StreamId"].ToInt();
                    int iGroupId = oDTStudentEnquiryDetails.Rows[0]["GroupId"].ToInt();
                    string sOptionalSubject = oDTStudentEnquiryDetails.Rows[0]["OptionalSubjects"].ToString();
                    string sCompitativeExam = oDTStudentEnquiryDetails.Rows[0]["CompitativeExam"].ToString();

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
                        trScienceStream.Style.Add("display", "block"); //
                        trArtsStream.Style.Add("display", "none");
                        trCommerceStream.Style.Add("display", "none");

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
                                //if (sExam.Length > Constants.I_ZERO)
                                //{
                                //    iExam1 = sExam[0].ToInt();
                                //    iExam2 = sExam[1].ToInt();
                                //}

                                //if (iExam1 != Constants.I_ZERO)
                                //    chkStream_SciGr1JEE.Checked = true;

                                //if (iExam2 != Constants.I_ZERO)
                                //    chkStream_SciGr1ExtraCo.Checked = true;
                                if (sExam.Length == Constants.I_ONE) //
                                {
                                    if (sExam[0].ToInt() == 1)
                                        chkStream_SciGr1JEE.Checked = true;
                                    else
                                        chkStream_SciGr1ExtraCo.Checked = true;
                                }
                                if (sExam.Length > Constants.I_ONE)  //
                                {
                                    iExam1 = sExam[0].ToInt();
                                    iExam2 = sExam[1].ToInt();
                                    if (iExam1 != Constants.I_ZERO)
                                        chkStream_SciGr1JEE.Checked = true;

                                    if (iExam2 != Constants.I_ZERO)
                                        chkStream_SciGr1ExtraCo.Checked = true;
                                }
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
                                //if (sExam.Length > Constants.I_ZERO)
                                //{
                                //    iExam1 = sExam[0].ToInt();
                                //    iExam2 = sExam[1].ToInt();
                                //}

                                //if (iExam1 != Constants.I_ZERO)
                                //    chkStream_SciGr2Neet.Checked = true;

                                //if (iExam2 != Constants.I_ZERO)
                                //    chkStream_SciGr2ExtraCO.Checked = true;
                                if (sExam.Length == Constants.I_ONE) //
                                {
                                    if (sExam[0].ToInt() == 1)
                                        chkStream_SciGr2Neet.Checked = true;
                                    else
                                        chkStream_SciGr2ExtraCO.Checked = true;
                                }
                                if (sExam.Length > Constants.I_ONE)  //
                                {
                                    iExam1 = sExam[0].ToInt();
                                    iExam2 = sExam[1].ToInt();
                                    if (iExam1 != Constants.I_ZERO)
                                        chkStream_SciGr2Neet.Checked = true;

                                    if (iExam2 != Constants.I_ZERO)
                                        chkStream_SciGr2ExtraCO.Checked = true;
                                }
                            }
                        }
                    }
                    else if (iStreamId == 2) // Commerse
                    {
                        trCommerceStream.Style.Add("display", "block"); //
                        trArtsStream.Style.Add("display", "none");
                        trScienceStream.Style.Add("display", "none");

                        if (sOptionalSubject == "1")
                            rdoStream_ComMaths.Checked = true;
                        else if (sOptionalSubject == "2")
                            rdoStream_ComPhyEdu.Checked = true;
                         else if (sOptionalSubject == "3")
                            rdoStream_ComLeagalStudies.Checked = true;

                        if (sCompitativeExam != string.Empty)
                        {
                            string[] sExam = sCompitativeExam.Split(',');
                            int iExam1 = Constants.I_ZERO;
                            int iExam2 = Constants.I_ZERO;
                            if (sExam.Length == Constants.I_ONE) //
                            {
                                if (sExam[0].ToInt() == 1)
                                    chkStream_ComCA.Checked = true;
                                else
                                    chkStream_ComExtraCo.Checked = true;
                            }
                            if (sExam.Length > Constants.I_ONE)  //
                            {
                                iExam1 = sExam[0].ToInt();
                                iExam2 = sExam[1].ToInt();
                                if (iExam1 != Constants.I_ZERO)
                                    chkStream_ComCA.Checked = true;

                                if (iExam2 != Constants.I_ZERO)
                                    chkStream_ComExtraCo.Checked = true;
                            }


                            //if (iExam1 != Constants.I_ZERO)
                            //    chkStream_ComCA.Checked = true;

                            //if (iExam2 != Constants.I_ZERO)
                            //    chkStream_ComExtraCo.Checked = true;
                        }
                    }
                    else if (iStreamId == 3) //Art
                    {
                        trArtsStream.Style.Add("display", "block"); //
                        trScienceStream.Style.Add("display", "none");
                        trCommerceStream.Style.Add("display", "none");
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
    }

    /// <summary>
    /// This method is used to load page controls.
    /// </summary>
    private void LoadPageControls(object sender, EventArgs e)
    {
        int iStudentAdmisssionID = Convert.ToInt32(hidStudentAdmisssionID.Value);
        int iAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
        S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
        hidServerDt.Value = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        if (miSchoolId == Constants.I_ZERO)
        {
            tdReceiptNo.Visible = false;
            tdReceiptNo1.Visible = false;
        }
        else
        {
            tdReceiptNo.Visible = true;
            tdReceiptNo1.Visible = true; 
        }
        DataSet oDataSet = MasterDataCollectionBL.GetAllMasterDataForStudentRegistration(SchoolId, iAcademicYearId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);

        FillCombobox(oDataSet, sender,e);
    }

    /// <summary>
    /// This method is used to fill combobox.
    /// </summary>
    /// <param name="oDataSet"></param>
    private void FillCombobox(DataSet oDataSet,object sender, EventArgs e)
    {
        ControlUtility.FillDropDownList(oDataSet.Tables[0], ref cmbYear, "Academic_Year_ID", "AcademicYear", string.Empty);
        ControlUtility.FillDropDownList(oDataSet.Tables[2], ref cmbStd, "Standard_Id", "Standard_Name", string.Empty);
        ControlUtility.FillDropDownList(oDataSet.Tables[3], ref cmbFOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDataSet.Tables[3], ref cmbMOccupation, "Ocupation_Id", "Ocupation_Name", Constants.S_SELECT);

        if (cmbYear.Items.Count > 0)
            cmbYear.Items[cmbYear.Items.Count - 1].Selected = true;

        ViewState["MinMaxDate"] = oDataSet.Tables[2];
        if (hidStandardName.Value != string.Empty)
        {
           ListItem obj = cmbStd.Items.FindByText(hidStandardName.Value);
           if (obj!=null)
           {
               obj.Selected=true;
               cmbStd_SelectedIndexChanged(sender, e);
           }
         
        }
    }

    /// <summary>
    /// This Method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["EnquiryId"] != null)
            hidEnquieryId.Value = QueryString["EnquiryId"];
        if (QueryString["StudetAdmissionId"] != null)
            hidStudentAdmisssionID.Value = QueryString["StudetAdmissionId"];
        if (QueryString["AcademicYearId"] != null)
            hidAcademicYearId.Value = QueryString["AcademicYearId"];
        if (QueryString["StatusId"] != null)
            hidStatusId.Value = QueryString["StatusId"];
        if (QueryString["StandardName"] != null)
            hidStandardName.Value = QueryString["StandardName"];
        if (QueryString["IsEnquiry"] != null)
        {
          hidIsEnquiry.Value = QueryString["IsEnquiry"];
        }
    }

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttributes()
    {
        int iAccYearId = miAcademicYearId;
        if (hidAcademicYearId.Value != string.Empty)
            iAccYearId = hidAcademicYearId.Value.ToInt();

        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        string sQueryString = CommonUtility.EncryptQuerystring("AcademicYearId=" + iAccYearId + "&StatusId=" + hidStatusId.Value);
        btnBack.PostBackUrl = "~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx?" + sQueryString;

        //if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SNS.ToInt())
        //{
        //    txtAadharCardNumber.Style.Add("background-color", "#ffffa0");
        //    hidValidateAadharCard.Value = Constants.S_YES;

        //}
        //else
        //    hidValidateAadharCard.Value = Constants.S_NO;
    }

    /// <summary>
    /// This method is used to set min and max date for validation.
    /// </summary>
    /// <param name="aOdtDatatable"></param>
    /// <param name="aiStandardId"></param>
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
                    }                    
                }
            }
        }
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
    /// This method is used to set last school name state.
    /// </summary>
    private void SetLastSchoolState()
    {
        if (SchoolId == Constants.SchoolId.SNS.ToInt())
        {
            string sStdName = cmbStd.SelectedItem.Text;
            if (sStdName == "Play Group" || sStdName == "Nursery" || sStdName == "LKG" || sStdName == "UKG")
            {
                txtSchoolName.BackColor = System.Drawing.Color.White;
                reqLastSchoolName.Enabled = false;
                spnMdtLastSchoolName.Visible = false;
            }
            else
            {
                txtSchoolName.BackColor = System.Drawing.Color.Yellow;
                reqLastSchoolName.Enabled = true;
                spnMdtLastSchoolName.Visible = true;
            }

          }
    }

    /// <summary>
    /// these method is used to set enquiry controls.
    /// </summary>
    private void SetEnquiryVisibility()
    {
        int iIsEnquiry = hidIsEnquiry.Value.ToInt();
        if (iIsEnquiry == 1)
        {
            trEnquiry.Visible = true;
            tdReceiptNo.Visible = false;
            tdReceiptNo1.Visible = false;

            if (miUserId != 0)
                txtEnqNo.Enabled = false;
            else
                trEnquiry.Visible = false;
        }
        else
        {
            trEnquiry.Visible = false;
            tdReceiptNo.Visible = true;
            tdReceiptNo1.Visible = true;
        }
    }
    /// <summary>
    /// these method gives next enquiry no.
    /// </summary>
    /// <param name="iSchoolId"></param>
    /// <param name="iAcademicYearID"></param>
    /// <returns></returns>
    private string GetNextEnquiryNo(int aiSchoolId, int aiAcademicYearID)
    {
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        string sEnqiryNo = oSchoolEnquiryBL.GetNextEnquiryNo(aiSchoolId, aiAcademicYearID);
        return sEnqiryNo;
    }
    /// <summary>
    /// Get the IsEnquiry value
    /// </summary>
    private void GetIsEnquiryValue()
    {
        if (miSchoolId == 0)
        {
            hidIsEnquiry.Value = "1";           
        }
        else
        {
            string sIsEnquiry = QueryString["IsEnquiry"];
            if (!string.IsNullOrEmpty(sIsEnquiry) && sIsEnquiry.ToInt() == 1) 
            {
                hidIsEnquiry.Value = "1";               
            }
            else
            {
                hidIsEnquiry.Value = "0";                
            }
        }
    }

    #endregion   
}