// File Name - StudentMandatoryDetailsUI.aspx.cs
// Description - This class is used to save / submit student mandatory details.
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentMandatoryDetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MSG = "Student Details saved successfully!!!";
    private const string S_SUBMIT_MSG = "Student Details submitted successfully!!!";
    private const string S_SESSION_YEARWISE_STUDENT_ID = "YearwiseStudentId";

    #endregion

    #region Data Member(s)

    private StudentMandatoryDetailsBL moStudentMandatoryDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to load page details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentMandatoryDetailsBL = new StudentMandatoryDetailsBL(miSchoolId, miAcademicYearId, miUserId);

            if (!IsPostBack)
            {
                FillBloodGroup();
                FillTransportMode();
                SetJavascriptAttribute();
                GetStudentMandatoryDetails();
         
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle transport mode change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTransportMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //SetTransportModeUI(ddlTransportMode.SelectedValue);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save student mandatory details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int iYearwiseStudentId = GetYearwiseStudentIdFromSession();
                StudentMandatoryDetails oStudentMandatoryDetails = Populate();
                bool bResult = moStudentMandatoryDetailsBL.SaveStudentMandatoryDetails(oStudentMandatoryDetails, iYearwiseStudentId);
                if (bResult)
                {
                    base.DisplayMessage(S_SAVE_MSG, false, tdMessage);
                    GetStudentMandatoryDetails();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit student mandatory details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iYearwiseStudentId = GetYearwiseStudentIdFromSession();
            bool bResult = moStudentMandatoryDetailsBL.SubmitStudentMandatoryDetails(iYearwiseStudentId);
            if (bResult)
            {
                base.DisplayMessage(S_SUBMIT_MSG, false, tdMessage);
                GetStudentMandatoryDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to validate transport fields based on selected transport mode.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTransportValidation_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            string sTransportMode = ddlTransportMode.SelectedValue;

            if (sTransportMode == Constants.S_ONE)
            {
                if (txtRouteNo.Text.Trim() == string.Empty || txtStopName.Text.Trim() == string.Empty)
                {
                    cvTransportValidation.ErrorMessage = "Route No and Stop Name should not be blank for selected Transport Mode.";
                    args.IsValid = false;
                    return;
                }
            }
            else if (sTransportMode == "2")
            {
                if (txtContractorName.Text.Trim() == string.Empty || txtContractorContactNo.Text.Trim() == string.Empty)
                {
                    cvTransportValidation.ErrorMessage = "Contractor Name and Contractor Contact No. should not be blank for selected Transport Mode.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            args.IsValid = false;
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set javascript attributes on controls.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnSubmit });
        lblMessage.Text = string.Empty;
        lblError.Text = string.Empty;
        valSumError.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "Resetmessage();");
    }

    /// <summary>
    /// This method is used to bind blood group dropdown.
    /// </summary>
    private void FillBloodGroup()
    {
        UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
        DataTable oDtStandardCollection = UsersStaffGroupsAssociationBL.GetAllBloodGroups();
        ListSource.FillDropDownList(oDtStandardCollection, ddlBloodGroup, "BloodGroup", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to bind transport mode dropdown.
    /// </summary>
    private void FillTransportMode()
    {
        DataTable oDataTable = moStudentMandatoryDetailsBL.GetTransportModeDetails();
        ListSource.FillDropDownList(oDataTable, ddlTransportMode, "ModeName", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to populate entity from UI controls.
    /// </summary>
    /// <returns></returns>
    private StudentMandatoryDetails Populate()
    {
        StudentMandatoryDetails oStudentMandatoryDetails = new StudentMandatoryDetails();
        oStudentMandatoryDetails.FatherMobileNumber = txtFatherMobileNumber.Text.Trim();
        oStudentMandatoryDetails.MotherMobileNumber = txtMotherMobileNumber.Text.Trim();
        oStudentMandatoryDetails.EmergencyContact = txtEmergencyContact.Text.Trim();
        oStudentMandatoryDetails.BloodGroup = ddlBloodGroup.SelectedItem.Text;

        oStudentMandatoryDetails.TransportMode = ddlTransportMode.SelectedValue.ToInt();

        oStudentMandatoryDetails.ContractorName = string.Empty;
        oStudentMandatoryDetails.ContractorContactNo = string.Empty;
        oStudentMandatoryDetails.RouteNo = string.Empty;
        oStudentMandatoryDetails.StopName = string.Empty;

        if (ddlTransportMode.SelectedValue == Constants.S_ONE)
        {
            oStudentMandatoryDetails.RouteNo = txtRouteNo.Text.Trim();
            oStudentMandatoryDetails.StopName = txtStopName.Text.Trim();
        }
        else if (ddlTransportMode.SelectedValue == Constants.S_TWO)
        {
            oStudentMandatoryDetails.ContractorName = txtContractorName.Text.Trim();
            oStudentMandatoryDetails.ContractorContactNo = txtContractorContactNo.Text.Trim();
        }
        
        return oStudentMandatoryDetails;
    }

    /// <summary>
    /// This method is used to get and bind student mandatory details.
    /// </summary>
    private void GetStudentMandatoryDetails()
    {
        int iYearwiseStudentId = GetYearwiseStudentIdFromSession();
        StudentMandatoryDetails oStudentMandatoryDetails = moStudentMandatoryDetailsBL.GetStudentMandatoryDetails(iYearwiseStudentId);
        if (oStudentMandatoryDetails != null)
        {
            txtFatherMobileNumber.Text = oStudentMandatoryDetails.FatherMobileNumber;
            txtMotherMobileNumber.Text = oStudentMandatoryDetails.MotherMobileNumber;
            txtEmergencyContact.Text = oStudentMandatoryDetails.EmergencyContact;

            ListItem OListItem = ddlBloodGroup.Items.FindByText(oStudentMandatoryDetails.BloodGroup);
            if (OListItem != null)
                OListItem.Selected = true;
            else
                ddlBloodGroup.SelectedValue = Constants.S_ZERO;

            ddlTransportMode.SelectedValue = oStudentMandatoryDetails.TransportMode.ToString();
            txtRouteNo.Text = oStudentMandatoryDetails.RouteNo;
            txtStopName.Text = oStudentMandatoryDetails.StopName;
            txtContractorName.Text = oStudentMandatoryDetails.ContractorName;
            txtContractorContactNo.Text = oStudentMandatoryDetails.ContractorContactNo;

            SetTransportModeUI(ddlTransportMode.SelectedValue);
            SetButtonState(oStudentMandatoryDetails.IsSaved, oStudentMandatoryDetails.IsSubmitted);
        }
        else
        {
            SetTransportModeUI(ddlTransportMode.SelectedValue);
            SetButtonState(false, false);
        }
    }

    /// <summary>
    /// This method is used to get yearwise student id from session.
    /// </summary>
    /// <returns></returns>
    private int GetYearwiseStudentIdFromSession()
    {
        if (Session[Constants.S_SESSION_STUDENT_ID] != null)
            return Session[Constants.S_SESSION_STUDENT_ID].ToInt();

        return Constants.I_ZERO;
    }

    /// <summary>
    /// This method is used to set save and submit button state.
    /// </summary>
    /// <param name="abIsSaved"></param>
    /// <param name="abIsSubmitted"></param>
    private void SetButtonState(bool abIsSaved, bool abIsSubmitted)
    {
        if (abIsSubmitted)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;

            Session[Constants.S_SESSION_ARE_MANDATORY_FIELD_SUBMITTED_BY_STUDENT] = Constants.S_YES;

            return;
        }

        btnSave.Enabled = true;
        btnSubmit.Enabled = abIsSaved;
    }

    /// <summary>
    /// This method is used to show or hide transport fields as per selected transport mode.
    /// </summary>
    /// <param name="asTransportMode"></param>
    private void SetTransportModeUI(string asTransportMode)
    {
        bool bIsRouteStop = asTransportMode == Constants.S_ONE; 
        bool bIsContractor = asTransportMode == "2"; 

        trRouteStop.Style["display"] = bIsRouteStop ? string.Empty : "none";
        trStopName.Style["display"] = bIsRouteStop ? string.Empty : "none";

        trContractor.Style["display"] = bIsContractor ? string.Empty : "none";
        trContractorNo.Style["display"] = bIsContractor ? string.Empty : "none";

        rfvRouteNo.Enabled = bIsRouteStop;
        rfvStopName.Enabled = bIsRouteStop;

        rfvContractorName.Enabled = bIsContractor;
        rfvContractorContactNo.Enabled = bIsContractor;

       
        if (!bIsRouteStop)
        {
            txtRouteNo.Text = string.Empty;
            txtStopName.Text = string.Empty;
        }

        if (!bIsContractor)
        {
            txtContractorName.Text = string.Empty;
            txtContractorContactNo.Text = string.Empty;
        }
    }

    #endregion
}

