using System;
using System.Data;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web.UI.WebControls;

public partial class ResetFeeReceiptPopUp : SchoolBase
{
    #region DataMember

    private StudentFeeDetailsBL moStudentFeeDetailsBL;

    #endregion

    #region Page Events

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentFeeDetailsBL = new StudentFeeDetailsBL();
            if (!IsPostBack)
            {
                FillFeeTypeCombo();
                FillAccountHeaderCombo();
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                SetResetOptionList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event  is used to set the Reset For Receipt Nos.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbResetFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbResetFor.SelectedValue.ToInt() == Constants.I_ZERO)
            {
                cmbFeeType.Enabled = true;
                cmbFeeType.SelectedValue = Constants.S_ZERO;
                cmbAccountHeader.SelectedValue = Constants.S_ZERO;
                cmbAccountHeader.Enabled = false;
                spMandatory.Visible = true;
                FillFeeTypeCombo();
            }
            else if (cmbResetFor.SelectedValue.ToInt() == Constants.I_ONE)
            {
                cmbFeeType.SelectedValue = Constants.S_ZERO;
                cmbFeeType.Enabled = false;
                spMandatory.Visible = false;
                FillAccountHeaderCombo();
            }
            else
            {
                cmbFeeType.SelectedValue = Constants.S_ZERO;
                cmbFeeType.Enabled = false;
                spMandatory.Visible = false;
                FillAccountHeaderCombo();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event  is used to set the Fee Type  Events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillAccountHeaderCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This Event is used to Reset Receipt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtFromDate = DateTime.MinValue;

            if (moSchool != Constants.SchoolId.VPMCPS)
            {
                dtFromDate = txtStartDate.Text.ToDateTime();
            }

            moStudentFeeDetailsBL.ResetReceiptNumber(miSchoolId, miAcademicYearId, cmbAccountHeader.SelectedValue.ToInt(), dtFromDate, cmbOrderBy.SelectedValue.ToInt(), cmbResetFor.SelectedValue.ToInt());
            lblMessage.Visible = true;
            lblMessage.Text = "Receipt number reset Successfully !!!";
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// This method is used to fill FeeType combo box.
    /// </summary>
    private void FillFeeTypeCombo()
    {
        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataTable dtStdFeeType = oStudentFeeDetailsBL.GetStandardFeeType(miSchoolId, miAcademicYearId, 0);
        cmbFeeType.Bind(dtStdFeeType, "SchoolWise_Standard_FeeType_Id", "Fee_Type", Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to Fill Account Header ComboBox.
    /// </summary>
    private void FillAccountHeaderCombo()
    {
        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataTable dtAccountHeader = oStudentFeeDetailsBL.GetAllAccountHeaderCombo(miSchoolId, miAcademicYearId, cmbFeeType.SelectedValue.ToInt(), cmbResetFor.SelectedValue.ToInt());
        cmbAccountHeader.Bind(dtAccountHeader, "AccountHeaderId", "AccountHeaderName", Constants.S_SELECT);

        if (dtAccountHeader.Rows.Count == 1)
        {
            cmbAccountHeader.SelectedIndex = 1;
            cmbAccountHeader.Enabled = false;
        }
        else
        {
            cmbAccountHeader.SelectedIndex = 0;
            cmbAccountHeader.Enabled = true;
        }
    }
        
    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtStartDate.Text = string.Empty;
        cmbFeeType.SelectedValue = Constants.S_ZERO;
        cmbAccountHeader.SelectedValue = Constants.S_ZERO;
        cmbOrderBy.SelectedValue = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set fields as per VPMCPS school.
    /// </summary>
    private void SetResetOptionList()
    {
        if (moSchool == Constants.SchoolId.VPMCPS)
        {
            var oItem = cmbResetFor.Items.FindByValue("2");
            if (oItem != null)
                cmbResetFor.Items.Remove(oItem);

            // Hide From Date, Fee Type, Account Header, Order By fields for VPMCPS
            trFromDate.Visible = false;
            trFeeType.Visible = false;
            trAccountHeader.Visible = false;
            trOrderBy.Visible = false;

            // Disable validators for hidden fields
            reqDate.Enabled = false;
            regAccountHeader.Enabled = false;
            cstValidateFeeTypes.Enabled = false;
        }
    }

    #endregion
}