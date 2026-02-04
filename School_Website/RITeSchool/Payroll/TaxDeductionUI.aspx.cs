// File Name - TaxDeductionUI.aspx.cs
// Creator - Pravin
// Created Date - 
// Description - This class is used to configure investment method.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class TaxDeductionUI : SchoolBase
{   
    #region Data Member(s)

    TaxDeductionBL moTaxDeductionBL;
    IncomeTaxDetailsBL moIncomeTaxDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            AddSortImage(lstvwTaxDeduction, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill tax deduction in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            moTaxDeductionBL = new TaxDeductionBL(miSchoolId, miFinancialYearId, miUserId,miAcademicYearId);
            SetControls();            

            if (!IsPostBack)
            {
                SetDefaultValues();
                FillStaffGroups();
                FillUsers();
                FillQuarters();
                FillTaxDeductionDetails();
                ReadQueryString();
            }

            //Hide back button when query string in empty
            if (hidQueryString.Value.IsNullOrEmpty())
                btnBack.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill users in staff group combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            FillUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up tax deduction details according to selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();           
            FillTaxDeductionDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is  used to save tax deduction configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            TaxDeduction oTaxDeduction = Populate(false);
            moTaxDeductionBL.Save(oTaxDeduction);
            DisplayMessage(BtnSave.Text == Constants.ButtonText.Save.ToString() ? Constants.ItemState.saved : Constants.ItemState.updated, false);
            ResetFields();
            FillTaxDeductionDetails();            
        }
        catch (DuplicateName dn)
        {
            DisplayMessage(dn.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to payroll dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            cmbUser.SelectedValue = Constants.S_ZERO;
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Payroll/IncomeTaxDetailsUI.aspx?" + hidQueryString.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add attribute on delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTaxDeduction_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                if (hidIsPublished.Value == Constants.S_ONE)
                    btnDelete.Enabled = false;                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTaxDeduction_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iTaxDeductionId = Convert.ToInt32(lstvwTaxDeduction.DataKeys[e.Item.DisplayIndex]["Id"]);
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {   
                hidTaxDeductionId.Value = iTaxDeductionId.ToString();
                List<TaxDeduction> lstTaxDeduction = moTaxDeductionBL.GetAll(cmbUser.SelectedValue.ToInt(), hidSortExpression.Value, hidSortDirection.Value);
                TaxDeduction oTaxDeduction = lstTaxDeduction.Where(tds => tds.Id == iTaxDeductionId).FirstOrDefault();
                cmbQuarter.SelectedValue = oTaxDeduction.QuarterId.ToString();
                txtDeposited.Text = oTaxDeduction.TaxDepositedAmount.ToString();
                txtTaxDeductAmt.Text = oTaxDeduction.TaxDeductionAmount.ToString();                
                BtnSave.Text = Constants.ButtonText.Update.ToString();                
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {   
                hidTaxDeductionId.Value = iTaxDeductionId.ToString();
                TaxDeduction oTaxDeduction= Populate(true);
                moTaxDeductionBL.Save(oTaxDeduction);
                FillTaxDeductionDetails();
                ResetFields();
                DisplayMessage(Constants.ItemState.deleted, false);
            }          
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort expression.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTaxDeduction_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;

            RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillTaxDeductionDetails();
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set the controls state depending on publish & unpublish.
    /// </summary>
    private void SetControls()
    {
        if (moIncomeTaxDetailsBL.CheckIsPublished())
        {
            trPublishMessage.Visible = true;
            BtnSave.Enabled = false;
            hidIsPublished.Value = Constants.S_ONE;
        }
        else
        {
            trPublishMessage.Visible = false;
            BtnSave.Enabled = true;
            hidIsPublished.Value = Constants.S_ZERO;
        }
    }

    /// <summary>
    /// This function is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (string.IsNullOrEmpty(Request.QueryString.ToString()))
            return;       

        if (!QueryString["StaffGroupId"].IsNull())
            hidStaffGroupsId.Value = QueryString["StaffGroupId"];

        hidQueryString.Value = Request.QueryString.ToString();

        if (!hidStaffGroupsId.Value.IsNullOrEmpty())
            cmbStaffGroups.SelectedValue = hidStaffGroupsId.Value;

        if (!QueryString["UserId"].IsNullOrEmpty())
        {
            cmbUser.SelectedValue = QueryString["UserId"];
            cmbUser_SelectedIndexChanged(cmbUser, null);
            SetFieldState(QueryString["UserId"].ToInt());
        }
        else
            SetFieldState(0);
    }

    /// <summary>
    /// This method is used to set control state.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetFieldState(int aiUserId)
    {
        if (moIncomeTaxDetailsBL.CheckIsPublished(aiUserId))
        {
            trPublishMessage.Visible = true;
            cmbStaffGroups.Enabled = false;
            cmbUser.Enabled = false;
            BtnSave.Enabled = false;
        }
        else
        {
            trPublishMessage.Visible = false;
            cmbStaffGroups.Enabled = false;
            cmbUser.Enabled = false;
            BtnSave.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to fill up staff group combo box.
    /// </summary>
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        // This is a existing method.
        DataTable dtAllStaffGroups = StaffGroupsBL.GetAll(miSchoolId);
        DataRow[] drStaffGroups = dtAllStaffGroups.Select("SchoolId=" + miSchoolId);
        DataTable dtStaffGroups = dtAllStaffGroups.Clone();
        if (drStaffGroups.Length > 0)
            dtStaffGroups = drStaffGroups.CopyToDataTable();
        ControlUtility.FillDropDownList(dtStaffGroups, ref cmbStaffGroups, "StaffGroupsId", "StaffGroupsName", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to fill up user combo box.
    /// </summary>
    private void FillUsers()
    {
        List<UserBasicDetails> lstUserBasicDetails = moTaxDeductionBL.GetPayrollUsers(Convert.ToInt32(cmbStaffGroups.SelectedValue));
        ListSource.FillDropDownList(lstUserBasicDetails, cmbUser, "StaffName", "UserId", Constants.S_SELECT);
        FillTaxDeductionDetails();
    }

    /// <summary>
    /// This method is used to fill the quarters.
    /// </summary>
    private void FillQuarters()
    {
        List<Quarter> lstQuarters = moTaxDeductionBL.GetAllQuarters();
        ListSource.FillDropDownList(lstQuarters, cmbQuarter, "Name", "Id", Constants.S_SELECT);        
    }

    /// <summary>
    /// This method is used to fill section combo box.
    /// </summary>
    private void FillTaxDeductionDetails()
    {        
        List<TaxDeduction> lstTaxDeductions=moTaxDeductionBL.GetAll(cmbUser.SelectedValue.ToInt(),hidSortExpression.Value,hidSortDirection.Value);
        lstvwTaxDeduction.DataSource=lstTaxDeductions;
        lstvwTaxDeduction.DataBind();       
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { BtnSave, BtnCancel, btnBack });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = "QuarterName";
        cmbStaffGroups.Focus();
        BtnSave.Attributes.Add("onclick", "SetState()");
        txtDeposited.Attributes.Add("onchange", "CheckValue(this)");
        txtTaxDeductAmt.Attributes.Add("onchange", "CheckValue(this)");
        lnkCITDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        cmbQuarter.ClearSelection();
        txtDeposited.Text = string.Empty;
        txtTaxDeductAmt.Text = string.Empty;
        BtnSave.Text = Constants.ButtonText.Save.ToString();
        hidTaxDeductionId.Value = string.Empty;        
    }

    /// <summary>
    /// This method is used to populate object.
    /// </summary>
    private TaxDeduction Populate(bool abIsDeleted)
    {
        TaxDeduction oTaxDeduction = new TaxDeduction
        {
            Id =(hidTaxDeductionId.Value==string.Empty?Constants.I_ZERO:hidTaxDeductionId.Value.ToInt()),
            UserId = cmbUser.SelectedValue.ToInt(),
            QuarterId = cmbQuarter.SelectedValue.ToInt(),
            TaxDeductionAmount = txtTaxDeductAmt.Text==string.Empty?"0.0".ToDecimal():txtTaxDeductAmt.Text.ToDecimal(),
            TaxDepositedAmount = txtDeposited.Text==string.Empty?"0.0".ToDecimal():txtDeposited.Text.ToDecimal(),
            Is_Deleted=abIsDeleted.ToInt()
        };

        return oTaxDeduction;
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Tax Deduction details " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    #endregion
}