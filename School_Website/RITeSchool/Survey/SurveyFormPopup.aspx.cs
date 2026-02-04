/* File Name-  SurveyFormPopup.aspx.cs
 * Creator - Sachin
 * Created Date - 31-Oct-2015
 * Description - This screen is used to add/edit registration details.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Data.SqlClient;

public partial class SurveyFormPopup : SchoolBase
{
    #region Data Member(s)
    
    private SurveyStudentBL moSurveyStudentBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up school, standard and category combo boxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSurveyStudentBL = new SurveyStudentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillAllEntities();
                FillSurveyStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save registration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string sRegNo = Save();
            if ((hidId.Value == Constants.S_ZERO && chkIsInterested.Checked == true) || (hidId.Value != Constants.S_ZERO && chkIsInterested.Checked == true && hidIsIterestedForConpetition.Value == Constants.S_ZERO))
                SendSms(sRegNo);
            ClosePopup();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save registration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveAndContinue_Click(object sender, EventArgs e)
    {
        try
        {
            string sRegNo = Save();
            if ((hidId.Value == Constants.S_ZERO && chkIsInterested.Checked == true) || (hidId.Value != Constants.S_ZERO && chkIsInterested.Checked == true && hidIsIterestedForConpetition.Value == Constants.S_ZERO))
                SendSms(sRegNo);
            ClearFields();
            base.DisplayMessage("Registration details saved successfully !!!", false, tdMessage);
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up entity combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidSchool_OnValueChanged(object sender, EventArgs e)
    {
        List<Standard> lstStandards = moSurveyStudentBL.GetAllEntities();
        string sValue = cmbSchool.SelectedValue;
        ListSource.FillDropDownList(moSurveyStudentBL.Surveyschools, cmbSchool, "Name", "Id", Constants.S_SELECT);
        cmbSchool.SelectedValue = sValue;
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set values to controls.
    /// </summary>
    private void FillSurveyStudentDetails()
    {
        if (QueryString["Id"] != null && QueryString["Id"] != string.Empty && QueryString["Id"].ToInt() != 0)
        {
            int iId = QueryString["Id"].ToInt();
            SurveyStudentDetails oSurveyStudentDetails = moSurveyStudentBL.Get(iId);

            if (oSurveyStudentDetails.Id != 0)
            {
                lblRegNo.Text = oSurveyStudentDetails.RegNo;
                txtName.Text = oSurveyStudentDetails.Name;
                txtMobile1.Text = oSurveyStudentDetails.MobileNo1;
                txtMobile2.Text = oSurveyStudentDetails.MobileNo2;
                txtAddress.Text = oSurveyStudentDetails.Address;
                cmbCategory.SelectedValue = oSurveyStudentDetails.CategoryId.ToString();
                cmbSchool.SelectedValue = oSurveyStudentDetails.SurveySchoolId.ToString();
                cmbStandard.SelectedValue = oSurveyStudentDetails.StandardId.ToString();
                hidId.Value = oSurveyStudentDetails.Id.ToString();
                if (oSurveyStudentDetails.GenderId == 1)
                    optMale.Checked = true;
                else
                    optFemale.Checked = false;
                hidIsIterestedForConpetition.Value = oSurveyStudentDetails.IsInterested.ToInt().ToString(); ;
                if (oSurveyStudentDetails.IsInterested == 1)
                    chkIsInterested.Checked = true;
                else
                    chkIsInterested.Checked = false;
            }
        }
    }

    /// <summary>
    /// This method is used to fill all entities.
    /// </summary>
    private void FillAllEntities()
    {
        List<Standard> lstStandards = moSurveyStudentBL.GetAllEntities();
        ListSource.FillDropDownList(lstStandards, cmbStandard, "Name", "Id", "None");
        ListSource.FillDropDownList(moSurveyStudentBL.Surveyschools, cmbSchool, "Name", "Id", Constants.S_SELECT);
        ListSource.FillDropDownList(moSurveyStudentBL.SurveyStudentCategories, cmbCategory, "Name", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is sued to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnSaveAndContinue });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        optMale.Checked = true;
        txtName.Focus();
    }

    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <param name="asRegNo"></param>
    private void SendSms(string asRegNo)
    {
        string sSendSMS = ConfigurationManager.AppSettings["SendSMS"].ToString();

        if (sSendSMS == Constants.S_YES)
        {
            txtName.Text = txtName.Text.Trim();
            string sFirstName = txtName.Text.Substring(0, txtName.Text.IndexOf(' ') + 1).Trim();
            string sSubject = "DrawingCompetitionRegistrationSMS";
            string sText = "Dear " + sFirstName + ", Thank you for registration in Drawing Competition. Your registration no. is " + asRegNo + " - Jaywant Public School - Sanaswadi.";

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            Hashtable oHTUsersMobileNo = new Hashtable();

            if (txtMobile1.Text != string.Empty)
                oHTUsersMobileNo[txtMobile1.Text] = txtMobile1.Text;
            if (txtMobile2.Text != string.Empty)
            {
                oHTUsersMobileNo[txtMobile2.Text] = txtMobile2.Text;
            }

            SMS oSMS = new SMS();
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;

            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = oSchoolBL.AdminId;

            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSubject;
            oSMS.SMSText = sText;
            oSMS.AcademicYearID = miAcademicYearId;
            oSMS.SchoolID = miSchoolId;
            oSMS.DisplayText = txtName.Text.Trim();
            oSMS.ToManualNumbers = oHTUsersMobileNo;

            oSMS.Send();

            oHTUsersMobileNo.Clear();
        }
    }

    /// <summary>
    /// This method is used to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = "SurveyFormDetailsUI.aspx?";
        hidQueryString.Value = sQuerystring;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "RefreshBaseScreen();", true);
    }

    /// <summary>
    /// This method is used to save registration details.
    /// </summary>
    /// <returns></returns>
    private string Save()
    {
        SurveyStudentDetails oSurveyStudentDetails = new SurveyStudentDetails
        {
            CategoryId = cmbCategory.SelectedValue.ToInt(),
            Id = hidId.Value.ToInt(),
            MobileNo1 = txtMobile1.Text,
            MobileNo2 = txtMobile2.Text,
            Name = txtName.Text.Trim(),
            SurveySchoolId = cmbSchool.SelectedValue.ToInt(),
            StandardId = cmbStandard.SelectedValue.ToInt(),
            GenderId = (optMale.Checked ? 1 : 2),
            IsInterested = (chkIsInterested.Checked ? 1 : 0),
            Address = txtAddress.Text.Trim()
        };

        return moSurveyStudentBL.Save(oSurveyStudentDetails);
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        lblRegNo.Text = "-";
        txtName.Text = string.Empty;
        txtMobile1.Text = string.Empty;
        txtMobile2.Text = string.Empty;
        cmbCategory.ClearSelection();
        cmbSchool.ClearSelection();
        cmbStandard.ClearSelection();
        hidId.Value = Constants.S_ZERO;
        optMale.Checked = true;
        txtName.Focus();
        chkIsInterested.Checked = false;
        txtAddress.Text = string.Empty;
    } 

    #endregion
}