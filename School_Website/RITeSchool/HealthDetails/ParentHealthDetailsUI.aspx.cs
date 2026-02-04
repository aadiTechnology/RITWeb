/* Class Name - ParentHealthDetailsUI
 * Created By - Vishakha
 * Created On - 20 dec 2023
 * Description - This class is used to store item details.
 */

using System;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities;
using BusinessLogic;
using Utility;
using System.Data;
using BusinessLogic;

public partial class ParentHealthDetailsUI : SchoolBase
{
    #region Data Member(s)

    private ParentHealthDetailsBL moParentHealthDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to show Student name, parent details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moParentHealthDetailsBL = new ParentHealthDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetStudentName();
                SetHealthDetails();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save parent health details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int aiYearwiseStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            string sReturnValue = Populate();
            moParentHealthDetailsBL.Save(aiYearwiseStudentId, sReturnValue);
            SetHealthDetails();
            lblMessage.Text = "Parent health details saved successfully!!!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit parent health details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Submit();
            SetHealthDetails();
            lblMessage.Text = "Parent health details submitted successfully!!!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtFName.Text = string.Empty;
        txtMName.Text = string.Empty;
        txtFDOB.Text = string.Empty;
        txtMDOB.Text = string.Empty;
        txtFAadharCardNo.Text = string.Empty;
        txtMAadharCardNo.Text = string.Empty;
        ddlFBloodGroup.ClearSelection();
        ddlMBloodGroup.ClearSelection();
        txtFHeight.Text = string.Empty;
        txtMHeight.Text = string.Empty;
        txtFWeight.Text = string.Empty;
        txtMWeight.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to populate parent health details.
    /// </summary>
    /// <returns></returns>
    private string Populate()
    {
        List<ParentHealthDetails> lstParentHealthDetails = new List<ParentHealthDetails>();
        ParentHealthDetails o1ParentHealthDetails = new ParentHealthDetails();

        o1ParentHealthDetails.Name = txtFName.Text.Trim();
        o1ParentHealthDetails.DOB = txtFDOB.Text.ToDateTime();
        o1ParentHealthDetails.AadharCardNo = txtFAadharCardNo.Text.ToString();
        o1ParentHealthDetails.BloodGroup = ddlFBloodGroup.Text;
        o1ParentHealthDetails.Height = txtFHeight.Text.ToInt();
        o1ParentHealthDetails.Weight = txtFWeight.Text.ToDecimal();
        o1ParentHealthDetails.FatherOrMother = "F";
        lstParentHealthDetails.Add(o1ParentHealthDetails);

        ParentHealthDetails o2ParentHealthDetails = new ParentHealthDetails();
        o2ParentHealthDetails.Name = txtMName.Text.Trim();
        o2ParentHealthDetails.DOB = txtMDOB.Text.ToDateTime();
        o2ParentHealthDetails.AadharCardNo = txtMAadharCardNo.Text.ToString();
        o2ParentHealthDetails.BloodGroup = ddlMBloodGroup.Text;
        o2ParentHealthDetails.Height = txtMHeight.Text.ToInt();
        o2ParentHealthDetails.Weight = txtMWeight.Text.ToDecimal();
        o2ParentHealthDetails.FatherOrMother = "M";
        lstParentHealthDetails.Add(o2ParentHealthDetails);

        return GenerateXml(lstParentHealthDetails);
    }

    /// <summary>
    /// This method is used to get parent health details.
    /// </summary>
    private void SetHealthDetails()
    {
        int aiYearwiseStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        DataTable dtParentHealthDetails = moParentHealthDetailsBL.GetParentHealthDetails(aiYearwiseStudentId);

        if (dtParentHealthDetails.Rows.Count > Constants.I_ZERO)
        {
            bool bIsSubmitted = dtParentHealthDetails.Rows[0]["IsSubmitted"].ToBool();
            
            if (bIsSubmitted)
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnSubmit.Enabled = true;
            }

            DataRow[] drArrFather = dtParentHealthDetails.Select("FatherOrMother='F'");
            if (drArrFather.Length > 0)
            {
                txtFAadharCardNo.Text = drArrFather[0]["AadharCardNo"].ToString();
                txtFName.Text = drArrFather[0]["Name"].ToString();
                txtFDOB.Text = drArrFather[0]["DOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                ddlFBloodGroup.SelectedValue = drArrFather[0]["BloodGroup"].ToString();
                txtFHeight.Text = drArrFather[0]["Height"].ToString();
                txtFWeight.Text = drArrFather[0]["Weight"].ToString();
            }

            DataRow[] drArrMother = dtParentHealthDetails.Select("FatherOrMother='M'");
            if (drArrMother.Length > 0)
            {
                txtMAadharCardNo.Text = drArrMother[0]["AadharCardNo"].ToString();
                txtMName.Text = drArrMother[0]["Name"].ToString();
                txtMDOB.Text = drArrMother[0]["DOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                ddlMBloodGroup.SelectedValue = drArrMother[0]["BloodGroup"].ToString();
                txtMHeight.Text = drArrMother[0]["Height"].ToString();
                txtMWeight.Text = drArrMother[0]["Weight"].ToString();
            }
        }
        else
            btnSubmit.Enabled = false;
    }

    /// <summary>
    /// This method is used to submit parent details.
    /// </summary>
    private void Submit()
    {
        int aiYearwiseStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        moParentHealthDetailsBL.Submit(aiYearwiseStudentId);
    }

    /// <summary>
    /// This method is used to set student name to label.
    /// </summary>
    public void SetStudentName()
    {
        int aiSchoolwiseStudentId = Session[Constants.S_SESSION_SCHOOLWISE_STUDENT_ID].ToInt();
        StudentBL oStudentBL = new StudentBL();
        lblStudentName.Text = oStudentBL.GetStudentName(aiSchoolwiseStudentId);
    }
    
    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ValErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    #endregion
}