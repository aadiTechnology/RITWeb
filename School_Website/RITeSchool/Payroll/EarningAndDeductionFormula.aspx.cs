/* File Name :- EarningAndDeductionFormula.aspx.cs
 * Created By :- Sachin
 * Created Date :- 3 Nov 2009
 * Class Description :- This class is used to define formula(e)/range(s) of earnings and deductions.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class EarningAndDeductionFormulaUI : SchoolBase
{
    #region Constants

    private const int I_EARNINGS_DEDUCTIONS_TABLE_INDEX = 0;
    private const int I_FORMULA_TABLE_INDEX = 1;
    private const int I_RANGE_TABLE_INDEX = 0;
    private const int I_EARNING_DEDUCTION = 2;
   
    private const string S_RANGE_TABLE = "RangeTable";
    private const string S_FORMULA_TABLE = "Formula_Table";
   
    private const string S_EDIT_FORMULA = "EDIT FORMULA";
    private const string S_NEW_FORMULA = "NEW FORMULA";
   
    private const string S_NEW_RANGE = "NEW RANGE";
    private const string S_EDIT_RANGE = "EDIT RANGE";
   
    private const string S_FORMULA = "FORMULA";
    private const string S_RANGE = "RANGE";
    private const string S_SAVE = "SAVE";
    private const string S_DETAILS = "DETAILS";
   
    private const string S_AMOUNT_EMPTY_MESSAGE = "Amount should not be empty.";
    private const string S_FORMULA_RECURSIVE_MESSAGE = "Formula should not be recursive.";
    private const string S_FORMULA_VALID_MESSAGE = "Formula should be valid.";
    private const string S_AMOUNT_SAVE_MESSAGE = "Amount range has been saved successfully !!!";
    private const string S_RANGE_DELETE_MESSAGE = "Amount range has been deleted successfully !!!";
    private const string S_FORMULA_SAVE_MESSAGE = "Formula has been saved successfully !!!";
    private const string S_FORMULA_DELETE_MESSAGE = "Formula has been deleted successfully !!!";
    
    #endregion

    #region Data Members

    private DataSet moDSEDFormulaDetails;
    private bool mbDisplayCheckbox = true;
    private EarningDeductionFormulaBL moEarningDeductionFormulaBL;
    private AmountRangeBL moAmountRangeBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to decrypt query string, fill Earning - Deduction combobox and set view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moEarningDeductionFormulaBL = new EarningDeductionFormulaBL(miSchoolId, miAcademicYearId, miUserId);
            moAmountRangeBL = new AmountRangeBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQuerystring();
                GetFormulaDetails();
                SetView();
                InitializeFields();
                SetJavascriptAttributes();
            }

            InitializeFields();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save formula/range.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveFormulaOrRange();
        }
        catch (DuplicateName ex)
        {
            lblError.Text = ex.Message;
            lblMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display formula view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optFormula_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            HideRangeControls(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill ranges into list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRange_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            HideRangeControls(false);
            FillAmountRanges();
            if (hidMode.Value == S_NEW_FORMULA && lstvwFormula.Items.Count == 0)
                hidMode.Value = S_NEW_RANGE;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete formula/ranges.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteFormula_Click(object sender, EventArgs e)
    {
        try
        {   
            int iAmountRangeId = Convert.ToInt32(hidAmountRangeId.Value);
            int iFormulaId = Convert.ToInt32(hidFormulaId.Value);
            int iEarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value);
            moEarningDeductionFormulaBL.DeleteFormulaAndRange(iFormulaId, iAmountRangeId, iEarningsDeductionsId);

            if (hidMode.Value == S_EDIT_RANGE)
            {
                FillEmptyAmountRangeGrid();
                chkIsDefault.Enabled = true;
                chkIsDefault.Checked = false;
            }

            ResetFields();            
            GetFormulaDetails();
            hidFormulaId.Value = Constants.S_ZERO;
            
            if (optFormula.Checked)
            {
                hidMode.Value = S_NEW_FORMULA;
                lblMessage.Text = S_FORMULA_DELETE_MESSAGE;
            }
            else
            {
                hidMode.Value = S_NEW_RANGE;
                lblMessage.Text = S_RANGE_DELETE_MESSAGE;
            }

            btnDeleteFormula.Enabled = false;
        }
        catch (SqlException ex)
        {
            trMessage.Visible = true;
            lblError.Text = ex.Message;
            lblMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields and set new mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            hidMode.Value = string.Empty;
            txtFormulaName.Focus();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set column attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFormula_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            bool bIsDefault = Convert.ToBoolean(lstvwFormula.DataKeys[oCurrentItem.DisplayIndex]["IsDefault"]);

            ImageButton imgbtnDelete = oCurrentItem.FindControl("imgbtnDelete") as ImageButton;
            imgbtnDelete.Attributes["onclick"] = "if(!ConfirmDelete()) return false;";

            // change backgound color of default formula/range.
            if (bIsDefault)
            {
                HtmlTableRow oHtmlTableRow = e.Item.FindControl("Tr2") as HtmlTableRow;
                if (oHtmlTableRow != null)
                    oHtmlTableRow.Style.Add("background-color", "LightSkyBlue");
                imgbtnDelete.Visible = false;
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit/delete formula/range.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFormula_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iFormulaRangeId = Convert.ToInt32(e.CommandArgument);            
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
                EditFormulaAndRange(iFormulaRangeId, e);
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                DeleteFormulaAndRange(iFormulaRangeId);
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblError.Text = ex.Message;
            lblMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region Formula Events

    /// <summary>
    /// This method is used to add earning/deduction into formula textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddEarningDeduction_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbEarningDeductions.SelectedValue != Constants.S_ZERO)
            {
                txtFormula.Text = txtFormula.Text + cmbEarningDeductions.SelectedItem.Text;
                hidFormula.Value = hidFormula.Value + "," + cmbEarningDeductions.SelectedItem.Text;
                hidFormulaValue.Value = hidFormulaValue.Value + ",'" + cmbEarningDeductions.SelectedValue + "'";
                cmbEarningDeductions.SelectedValue = Constants.S_ZERO;
            }

            btnAddEarningDeduction.Enabled = false;
            btnRollBackFormula.Enabled = true;
            lblError.Visible = false;
            cmbOperators.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear formula fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtFormula.Text = string.Empty;
            txtFormulaValue.Text = string.Empty;
            cmbEarningDeductions.SelectedValue = Constants.S_ZERO;
            cmbOperators.SelectedValue = Constants.S_ZERO;
            hidFormulaValue.Value = string.Empty;
            hidFormula.Value = string.Empty;
            btnRollBackFormula.Enabled = false;
            txtFormulaName.Focus();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add operators into formula textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddOperator_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbOperators.SelectedValue != Constants.S_ZERO)
            {
                txtFormula.Text = txtFormula.Text + cmbOperators.SelectedItem.Text;
                hidFormula.Value = hidFormula.Value + "," + cmbOperators.SelectedItem.Text;
                hidFormulaValue.Value = hidFormulaValue.Value + "," + cmbOperators.SelectedValue;
                cmbOperators.SelectedValue = Constants.S_ZERO;
            }

            btnAddOperator.Enabled = false;
            btnRollBackFormula.Enabled = true;
            lblError.Visible = false;
            txtFormulaValue.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to rollback changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRollBackFormula_Click(object sender, EventArgs e)
    {
        try
        {   
            FormatFormulaText();
            FormatFormulaValue();
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add value into formula textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddConstant_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFormulaValue.Text.Trim() != string.Empty)
            {
                txtFormula.Text = txtFormula.Text + txtFormulaValue.Text.Trim();
                hidFormula.Value = hidFormula.Value + "," + txtFormulaValue.Text.Trim();
                hidFormulaValue.Value = hidFormulaValue.Value + "," + txtFormulaValue.Text.Trim();
                txtFormulaValue.Text = string.Empty;
                btnRollBackFormula.Enabled = true;
            }    
        
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear formula.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidMode.Value == S_EDIT_RANGE)
            {
                hidMode.Value = S_NEW_RANGE;
                txtFormulaName.Text = string.Empty;
                chkIsDefault.Enabled = true;
                hidOldRangeName.Value = string.Empty;
                hidRangeId.Value = Constants.S_ZERO;
                hidDefaultRange.Value = Constants.S_NO;
                chkIsDefault.Checked = false;                
            }
            else
                ResetFields();

            FillEmptyAmountRangeGrid();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to disable add button according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbEarningDeductions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbEarningDeductions.SelectedValue == Constants.S_ZERO)
                btnAddEarningDeduction.Enabled = false;
            else
            {              
                btnAddEarningDeduction.Enabled = true;
                cmbEarningDeductions.Focus();
                if (lstvwFormula.Items.Count <= 0)
                {
                    lstvwFormula.Visible = false;
                    btnDeleteFormula.Visible = false;
                    trLegend.Visible = false;
                }
                else
                    btnDeleteFormula.Visible = true;
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to disable add button according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbOperators_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbOperators.SelectedValue == Constants.S_ZERO)
                btnAddOperator.Enabled = false;
            else
            {
                btnAddOperator.Enabled = true;
                cmbOperators.Focus();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Range Events

    /// <summary>
    /// This event is used to save amount ranges.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSaveMonthAmount_Click(object sender, EventArgs e)
    {
        try
        {
            moAmountRangeBL.AmountRange = PopulateAmountRanges(sender);
            moAmountRangeBL.InsertMonthwiseAmount();
            DisableListview(false);
            HideButtons(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel saving of amount ranges.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancelMonthAmount_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            oButton.Parent.Parent.Visible = false;
            DisableListview(false);
            HideButtons(false);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on link button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAmountRange_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

                // If the school id is not the default id i.e. -9999 that means the range is already assigned
                // to the school. Thus check the checkbox.
                int iAmountRangeId = Convert.ToInt32(lstvwAmountRange.DataKeys[iRowId]["AmountRangeId"]);
                bool bHasRange = Convert.ToBoolean(oDataRowView["HasRange"]);

                CheckBox ChkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelect");
                ChkSelect.Attributes.Add("onclick", "VisibleDetailsLink('Earning'," + iRowId + ");");

                LinkButton lnkBtnDetails = (LinkButton)oCurrentItem.FindControl("lnkBtnDetails");
                lnkBtnDetails.Attributes.Add("onclick", "if(!ActivateLink(" + iRowId + "))return false;");

                Button BtnSaveRange = (Button)oCurrentItem.FindControl("BtnSaveRange");
                
                Button BtnSaveMonthAmount = (Button)oCurrentItem.FindControl("BtnSaveMonthAmount");
                Button BtnCancelMonthAmount = (Button)oCurrentItem.FindControl("BtnCancelMonthAmount");

                ApplyMouseHoverEffect(new List<Button> { BtnSaveRange, BtnSaveMonthAmount, BtnCancelMonthAmount });
                
                if (bHasRange)
                {
                    mbDisplayCheckbox = true;
                    ChkSelect.Checked = true;
                    lnkBtnDetails.Visible = true;
                    DisableControls(oCurrentItem, false);
                }
                else
                {
                    if (mbDisplayCheckbox)
                    {
                        ChkSelect.Visible = true;
                        mbDisplayCheckbox = false;
                    }
                    else
                        ChkSelect.Visible = false;
                    DisableControls(oCurrentItem, true);
                    lnkBtnDetails.Visible = false;
                }

                if (ChkSelect.Checked)
                    BtnSaveRange.Enabled = true;
                else
                    BtnSaveRange.Enabled = false;

                bool bIsDefault = Convert.ToBoolean(oDataRowView["IsDefault"]);

                chkIsDefault.Checked = bIsDefault;
                chkIsDefault.Enabled = !bIsDefault;

                BtnSaveRange.Attributes.Add("onclick", "if(!ValidateAmountRange(this," + iRowId + ",'" + ChkSelect.Checked + "')) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display monthwise amount list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAmountRange_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_DETAILS)
                ShowMonthwiseMountListview(e);
            if (e.CommandName == S_SAVE)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                if (ViewState[S_FORMULA_TABLE] != null)
                {
                    int iAmountRangeId = Convert.ToInt32(lstvwAmountRange.DataKeys[iRowId]["AmountRangeId"]);
                    if (!IsDependentFormula(iAmountRangeId))
                    {
                        CheckBox ChkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
                        if (iAmountRangeId != 0 && ChkSelect.Checked == false)
                        {
                            moAmountRangeBL.Delete(iAmountRangeId);
                            FillAmountRanges();
                            lblMessage.Text = S_RANGE_DELETE_MESSAGE;
                        }
                        else
                        {
                            IsDuplicateRangeName();
                            SaveAmountRange(iAmountRangeId);
                            txtFormula.Text = string.Empty;
                            btnDeleteFormula.Enabled = true;
                        }
                    }
                }

                hidMode.Value = S_EDIT_RANGE;                
            }
        }
        catch (DuplicateName ex)
        {
            lblError.Text = ex.Message;
            lblMessage.Text = string.Empty;
        }
        catch (SqlException ex)
        {
            lblError.Text = ex.Message;
            lblMessage.Text = string.Empty;
            FillAmountRanges();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to clear formula and reset controls.
    /// </summary>
    private void ResetFields()
    {
        txtFormula.Text = string.Empty;
        txtFormulaValue.Text = string.Empty;
        txtFormulaName.Text = string.Empty;
        chkIsDefault.Enabled = true;
        chkIsDefault.Checked = false;
        cmbEarningDeductions.SelectedValue = Constants.S_ZERO;
        cmbOperators.SelectedValue = Constants.S_ZERO;
        hidFormulaValue.Value = string.Empty;
        hidFormula.Value = string.Empty;
        btnRollBackFormula.Enabled = false;
        hidFormulaId.Value = Constants.S_ZERO;
        if (optFormula.Checked)
            hidMode.Value = S_NEW_FORMULA;
        else
            hidMode.Value = S_NEW_RANGE;
    
        txtFormulaName.Focus();
    }

    /// <summary>
    /// This method is used to get formula details.
    /// </summary>
    private void GetFormulaDetails()
    {
        EarningsDeductionsBL oEarningsDeductionsBL = new EarningsDeductionsBL();
        int iEarningDeductionId = Convert.ToInt32(hidEarningsDeductionsId.Value);
        
        moDSEDFormulaDetails = oEarningsDeductionsBL.GetFormulaDetails(miSchoolId, miAcademicYearId, iEarningDeductionId);
        if (moDSEDFormulaDetails != null && moDSEDFormulaDetails.Tables.Count > 0)
        {
            DataTable oDTEarningsDeductions = moDSEDFormulaDetails.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX];
            if (oDTEarningsDeductions.IsNonEmpty())
                ControlUtility.FillDropDownList(oDTEarningsDeductions, ref cmbEarningDeductions, "EarningsDeductionsId", "EarningsDeductionsName", Constants.S_SELECT);
            ViewState.Add(S_FORMULA_TABLE, moDSEDFormulaDetails.Tables[I_FORMULA_TABLE_INDEX]);            
            FillFormulaGrid(moDSEDFormulaDetails.Tables[I_FORMULA_TABLE_INDEX]);

            if (moDSEDFormulaDetails.Tables[I_EARNING_DEDUCTION].Rows.Count > 0 && !moDSEDFormulaDetails.Tables[I_EARNING_DEDUCTION].Rows[0]["Id"].ToString().IsNullOrEmpty() && (hidMode.Value == S_NEW_RANGE || hidMode.Value == S_NEW_FORMULA))
                trAlertMessage.Visible = true; 
            else
                trAlertMessage.Visible = false; 
        }        
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        hidEarningsDeductionsId.Value = QueryString["EarningDeductionId"];
        lblFormulaField.Text = QueryString["EarningDeductionName"];
        hidMode.Value = QueryString["Mode"];
    }

    /// <summary>
    /// This method is used to save amount range.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    private void SaveAmountRange(ListViewDataItem aoCurrentItem)
    {
        int iRowId = Convert.ToInt32(aoCurrentItem.DisplayIndex);
        int iAmountRangeId = Convert.ToInt32(lstvwAmountRange.DataKeys[iRowId]["AmountRangeId"]);

        TextBox txtFromAmount = (TextBox)aoCurrentItem.FindControl("txtFromAmount");
        TextBox txtUptoAmount = (TextBox)aoCurrentItem.FindControl("txtUptoAmount");
        TextBox txtAmount = (TextBox)aoCurrentItem.FindControl("txtAmount");
        CheckBox ChkSelect = (CheckBox)aoCurrentItem.FindControl("ChkSelect");

        moAmountRangeBL.AmountRange = new AmountRange
        {
            FromAmount = Convert.ToDecimal(txtFromAmount.Text),
            UptoAmount = Convert.ToDecimal(txtUptoAmount.Text),
            Amount = Convert.ToDecimal(txtAmount.Text),
            EarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value),           
            UpdateMonthwiseAmount = Convert.ToChar(hidUpdateMonthwiseAmount.Value),
            Is_Deleted = ChkSelect.Checked ? 0 : 1
        };

        DataTable oDataTable = moAmountRangeBL.InsertRangeRow(iAmountRangeId);
        lstvwAmountRange.DataSource = oDataTable;
        lstvwAmountRange.DataBind();
        hidMode.Value = "EDIT RANGE";
    }

    /// <summary>
    /// This method is used to display monthwise amount list view.
    /// </summary>    
    /// <param name="aiAmountRangeId"></param>
    private void ShowMonthwiseMountListview(ListViewCommandEventArgs e)
    {
        ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
        int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
        int iAmountRangeId = Convert.ToInt32(lstvwAmountRange.DataKeys[iRowId]["AmountRangeId"]);
        TextBox txtUptoAmount = (TextBox)oCurrentItem.FindControl("txtUptoAmount");
        if (txtUptoAmount.Text.Trim() != string.Empty)
        {
            hidUptoAmount.Value = txtUptoAmount.Text.Trim();            
            hidAmountRangeId.Value = iAmountRangeId.ToString();
            HideOtherMonthViews(iRowId);

            DataTable oDataTable = moAmountRangeBL.GetMonthwiseAmount(iAmountRangeId);
            System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = e.Item.FindControl("trlstvwRange") as System.Web.UI.HtmlControls.HtmlTableRow;
            System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdlstvwRange") as System.Web.UI.HtmlControls.HtmlTableCell;
            ListView olstvwRange = oHtmlTableCell.FindControl("lstvwRange") as ListView;
            olstvwRange.DataSource = oDataTable;
            olstvwRange.DataBind();
            oHtmlTableRow.Visible = true;
            HideButtons(true);
            DisableListview(true);
        }
        else
            lblError.Text = S_AMOUNT_EMPTY_MESSAGE;
    }

    /// <summary>
    /// This method is used to set view.
    /// </summary>
    private void SetView()
    {
        if (hidMode.Value == S_EDIT_FORMULA)
            SetFormulaFields();
        else if (hidMode.Value == S_EDIT_RANGE)
            FillAmountRanges();
        else
        {
            if (cmbEarningDeductions.Items.Count == 0)
            {
                optRange.Checked = true;
                HideRangeControls(false);
                hidDefaultRange.Value = Constants.S_YES;
                FillAmountRanges();
            }
            else
            {
                optFormula.Checked = true;
                btnRollBackFormula.Enabled = false;
                HideRangeControls(true);
            }
        }
    }

    /// <summary>
    /// This method is used to initialize fields.
    /// </summary>
    private void InitializeFields()
    {
        valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSummaryMonthwise.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumAmountRange.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumFormulaValue.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        
        if (hidMode.Value.Contains("EDIT"))
            btnDeleteFormula.Enabled = true;
        else
            btnDeleteFormula.Enabled = false;

        txtFormulaName.Focus();
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        List<Button> buttons = new List<Button>
                        {
                            BtnSave, BtnClose, btnCancel, btnClear,
                            btnAddFormulaValue, btnAddEarningDeduction,
                            btnAddOperator, btnRollBackFormula,
                            btnDeleteFormula
                        };
        ApplyMouseHoverEffect(buttons);

        BtnClose.Attributes["onclick"] = "window.opener.location.href = window.opener.location.href;window.close();window.opener.focus();";
        btnDeleteFormula.Attributes["onclick"] = "if(!Confermation()) return false;";
        BtnSave.Attributes["onclick"] = "if(!CheckSelectedAmountRange(this)) return false;";        
        btnClear.Attributes["onclick"] = "DoPostback(this)";
        btnCancel.Attributes["onclick"] = "DoPostback(this)";
        cmbEarningDeductions.Attributes["onchange"] = "DoPostback(this)";
        cmbOperators.Attributes["onchange"] = "DoPostback(this)";
    }

    /// <summary>
    /// This event is used to display range view.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideRangeControls(bool abAction)
    {
        try
        {
            trFormula.Visible = abAction;
            btnClear.Visible = abAction;
            trRange.Visible = !abAction;
            BtnSave.Visible = abAction;
            HideButtons(false);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save formula/Amount ranges.
    /// </summary>
    private void SaveFormulaOrRange()
    {
        bool bResult = false;
        IsDuplicateFormulaName();        
        if (optFormula.Checked)
            bResult = SaveFormula();
        if (bResult)
        {
            ResetFields();
            hidFormulaId.Value = Constants.S_ZERO;
            hidMode.Value = S_NEW_FORMULA;
        }        
    }

    /// <summary>
    /// This method is used to check duplicate formula name.
    /// </summary>
    private void IsDuplicateFormulaName()
    {
        EarningsDeductionsFormulae oEarningsDeductionsFormula = new EarningsDeductionsFormulae
        {   
            FormulaName = txtFormulaName.Text.Trim(),
            FormulaId = Convert.ToInt32(hidFormulaId.Value),
            EarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value)
        };
        moEarningDeductionFormulaBL.EarningsDeductionsFormula = oEarningsDeductionsFormula;
        moEarningDeductionFormulaBL.IsDuplicateFormulaName();
    }

    /// <summary>
    /// This method is used to check duplicate range name.
    /// </summary>
    private void IsDuplicateRangeName()
    {
        moAmountRangeBL.AmountRange = new AmountRange
        {
            SchoolId = miSchoolId,
            RangeName = txtFormulaName.Text.Trim(),
            RangeId = Convert.ToInt32(hidRangeId.Value),
            EarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value)
        };
        moAmountRangeBL.IsDuplicateRangeName();
    }

    /// <summary>
    /// This method is used to disable controls.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    /// <param name="abAction"></param>
    private void DisableControls(ListViewDataItem aoCurrentItem, bool abAction)
    {
        TextBox txtFromAmount = (TextBox)aoCurrentItem.FindControl("txtFromAmount");
        TextBox txtUptoAmount = (TextBox)aoCurrentItem.FindControl("txtUptoAmount");
        TextBox txtAmount = (TextBox)aoCurrentItem.FindControl("txtAmount");
        txtFromAmount.Enabled = !abAction;
        txtUptoAmount.Enabled = !abAction;
        txtAmount.Enabled = !abAction;
    }

    /// <summary>
    /// This method is used to check default formula.
    /// </summary>
    /// <param name="abIsDefault"></param>
    private void CheckDefaultFormula(bool abIsDefault)
    {
        chkIsDefault.Enabled = true;
        chkIsDefault.Checked = false;
        hidIsDefault.Value = Constants.S_ZERO;
        if (abIsDefault)
        {
            chkIsDefault.Checked = true;
            chkIsDefault.Enabled = false;
            hidIsDefault.Value = Constants.S_ONE;
        }
    }

    /// <summary>
    /// This method is used to edit range.
    /// </summary>
    /// <param name="aoDTRangeDetails"></param>
    /// <param name="aoCurrentItem"></param>
    private void EditRange(DataTable aoDTRangeDetails, ListViewDataItem aoCurrentItem)
    {
        if (aoDTRangeDetails.IsNonEmpty())
        {            
            int iDisplayIndex = aoCurrentItem.DisplayIndex;

            Label lblRangeName = aoCurrentItem.FindControl("lblFormulaName") as Label;
            string sRangeName = lblRangeName.Text;

            DataTable oDataTable = null;
            if (aoDTRangeDetails.Select("RangeName ='" + StringUtility.ReplaceSingleQuoteInString(sRangeName, false) + "'").Length > 0)
            {
                oDataTable = aoDTRangeDetails.Select("RangeName ='" + StringUtility.ReplaceSingleQuoteInString(sRangeName, false) + "'").CopyToDataTable();
                FillAmountRangesTable(oDataTable);
                if (oDataTable.Rows.Count > 0)
                {
                    bool bIsDefault = Convert.ToBoolean(oDataTable.Rows[0]["IsDefault"]);
                    hidDefaultRange.Value = bIsDefault ? Constants.S_YES : Constants.S_NO;
                    CheckDefaultFormula(bIsDefault);
                }

                lstvwAmountRange.DataSource = oDataTable;
                lstvwAmountRange.DataBind();
            }
        }
    }

    /// <summary>
    /// This method is used to edit formula.
    /// </summary>
    /// <param name="aoDTFormulaDetails"></param>
    /// <param name="aiFormulaId"></param>
    private void EditFormula(DataTable aoDTFormulaDetails, int aiFormulaId)
    {
        if (aoDTFormulaDetails.IsNonEmpty())
        {
            DataRow[] oDRFormulaDetails = aoDTFormulaDetails.Select("FormulaId = " + aiFormulaId);
            if (oDRFormulaDetails.Length > 0)
            {
                hidMode.Value = S_EDIT_FORMULA;
                hidFormula.Value = txtFormula.Text = Convert.ToString(oDRFormulaDetails[0]["FormulaValue"]);
                hidFormulaValue.Value = Convert.ToString(oDRFormulaDetails[0]["Formula"]).Replace("''", "'");
                hidFormulaId.Value = Convert.ToString(oDRFormulaDetails[0]["FormulaId"]);
                txtFormulaName.Text = Convert.ToString(oDRFormulaDetails[0]["FormulaName"]);
                bool bIsDefault = Convert.ToBoolean(oDRFormulaDetails[0]["IsDefault"]);
                CheckDefaultFormula(bIsDefault);
            }
        }
    }

    /// <summary>
    /// This method is used to delete formula and range.
    /// </summary>
    /// <param name="aiFormulaRangeId"></param>
    private void DeleteFormulaAndRange(int aiFormulaRangeId)
    {
        if (hidMode.Value.Contains(S_FORMULA))
        {   
            moEarningDeductionFormulaBL.Delete(aiFormulaRangeId);
            GetFormulaDetails();
            if (hidFormulaId.Value == aiFormulaRangeId.ToString())
                ResetFields();
            lblMessage.Text = S_FORMULA_DELETE_MESSAGE;
        }
        else if (hidMode.Value.Contains(S_RANGE))
        {   
            string sRangeName = txtFormulaName.Text;
            moAmountRangeBL.DeleteAmountRange(aiFormulaRangeId);            
            
            FillAmountRanges();
            FillEmptyAmountRangeGrid();
            txtFormulaName.Text = string.Empty;
            
            lblMessage.Text = S_RANGE_DELETE_MESSAGE;
        }        
    }

    /// <summary>
    /// This method is used to edit formula and range.
    /// </summary>
    /// <param name="iFormulaRangeId"></param>
    /// <param name="e"></param>
    private void EditFormulaAndRange(int iFormulaRangeId, ListViewCommandEventArgs e)
    {
        if (hidMode.Value.Contains(S_FORMULA))
        {
            DataTable oDTFormulaDetails = (DataTable)ViewState[S_FORMULA_TABLE];
            EditFormula(oDTFormulaDetails, iFormulaRangeId);
            GetFormulaDetails();
        }
        else if (hidMode.Value.Contains(S_RANGE))
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            DataTable oDTRangeDetails = (DataTable)ViewState[S_RANGE_TABLE];
            EditRange(oDTRangeDetails, oCurrentItem);
        }
    }

    /// <summary>
    /// This method is used to format formula value.
    /// </summary>
    private void FormatFormulaValue()
    {
        string sFormulaValue = hidFormulaValue.Value;
        if (sFormulaValue.Length != 0)
        {
            int iLength = sFormulaValue.LastIndexOf(',');
            if (iLength > 0)
                hidFormulaValue.Value = sFormulaValue.Substring(0, iLength);
            else
            {
                btnRollBackFormula.Enabled = false;
                hidFormulaValue.Value = string.Empty;
            }
        }
        else
            btnRollBackFormula.Enabled = false;
    }

    /// <summary>
    /// This method is used to format formula text.
    /// </summary>
    private void FormatFormulaText()
    {
        string sFormulaText = hidFormula.Value;
        if (sFormulaText.Length != 0)
        {
            int iLength = sFormulaText.LastIndexOf(',');
            if (iLength != -1)
            {
                hidFormula.Value = sFormulaText.Substring(0, iLength);
                txtFormula.Text = hidFormula.Value.Replace(',', ' ');
            }
            else
                btnRollBackFormula.Enabled = false;
        }
        else
            btnRollBackFormula.Enabled = false;
    }

    #region Formula Methods

    /// <summary>
    /// This method is used to fill formula.
    /// </summary>
    private void SetFormulaFields()
    {
        optFormula.Checked = true;
        btnRollBackFormula.Enabled = false;
        HideRangeControls(true);
        if (moDSEDFormulaDetails != null && moDSEDFormulaDetails.Tables.Count > 0)
        {
            DataTable oDTFormulaDetails = moDSEDFormulaDetails.Tables[I_FORMULA_TABLE_INDEX];
            if (oDTFormulaDetails.IsNonEmpty())
            {
                int iEarningDeductionId = Convert.ToInt32(hidEarningsDeductionsId.Value);
                
                var oFormulaDetails = from formula in oDTFormulaDetails.AsEnumerable()
                                        where formula.Field<int>("EarningsDeductionsId") == iEarningDeductionId
                                        && formula.Field<bool>("IsDefault") == true
                                        select new
                                        {
                                            FormulaValue = Convert.ToString(formula.Field<string>("FormulaValue")),
                                            Formula = Convert.ToString(formula.Field<string>("Formula")),
                                            FormulaId = Convert.ToInt32(formula.Field<int>("FormulaId")),
                                            FormulaName = Convert.ToString(formula.Field<string>("FormulaName"))
                                        };
                
                if (oFormulaDetails.Count() > 0)
                {
                    var oFormulaDetail = oFormulaDetails.First();
                    hidFormula.Value = txtFormula.Text = oFormulaDetail.FormulaValue;
                    hidFormulaValue.Value = oFormulaDetail.Formula.Replace("''", "'");
                    hidFormulaId.Value = oFormulaDetail.FormulaId.ToString();
                    txtFormulaName.Text = oFormulaDetail.FormulaName;
                    hidIsDefault.Value = Constants.S_ONE;
                    chkIsDefault.Checked = true;
                    chkIsDefault.Enabled = false;                    
                }
            }
        }
    }

    /// <summary>
    /// This method is used to fill formula/range list view.
    /// </summary>
    /// <param name="aoDTFormulaDetails"></param>
    private void FillFormulaGrid(DataTable aoDTFormulaDetails)
    {
        if (aoDTFormulaDetails.IsNonEmpty())
        {
            DataTable oDtFormulaDetails = null;
            int iEarningDeductionId = Convert.ToInt32(hidEarningsDeductionsId.Value);
            if (aoDTFormulaDetails.Select("EarningsDeductionsId = " + iEarningDeductionId).Length > 0)
            {
                oDtFormulaDetails = aoDTFormulaDetails.Select("EarningsDeductionsId = " + iEarningDeductionId).CopyToDataTable();
                SetFields(true);
            }
            else
            {
                SetFields(false);
                chkIsDefault.Checked = true;
                chkIsDefault.Enabled = false;
            }

            lstvwFormula.DataSource = oDtFormulaDetails;
            lstvwFormula.DataBind();
            if (oDtFormulaDetails == null || lstvwFormula.Items.Count == 0)
            {
                lstvwFormula.Visible = false;
                btnDeleteFormula.Visible = false;
            }
            else
            {
                lstvwFormula.Visible = true;
                btnDeleteFormula.Visible = true;
            }
        }
        else
        {
            lstvwFormula.DataSource = null;
            lstvwFormula.DataBind();
            lstvwFormula.Visible = false;
            btnDeleteFormula.Visible = false;
            trLegend.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set values to fields.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetFields(bool abAction)
    {
        trLegend.Visible = abAction;
        btnDeleteFormula.Enabled = abAction;
        hidIsDefault.Value = abAction ? Constants.S_ZERO : Constants.S_ONE;
    }

    /// <summary>
    /// This method is used to save formula.
    /// </summary>
    /// <returns></returns>
    private bool SaveFormula()
    {
        string sFormula = ValidateFormula();
        string sEarningsDeductions = CheckRecursion(sFormula);
        if (sFormula != string.Empty)
        {
            if (!sEarningsDeductions.Contains("Recursive"))
            {
                if (hidChildIds.Value.Length > 1)
                    hidChildIds.Value = hidChildIds.Value.Substring(1);

                EarningsDeductionsFormulae oEarningsDeductionsFormula = new EarningsDeductionsFormulae
                {
                    EarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value),
                    Formula = sFormula,                    
                    ChildIds = hidChildIds.Value,
                    IsDefault = chkIsDefault.Checked,
                    FormulaName = txtFormulaName.Text.Trim()
                };

                if (lstvwFormula.Items.Count == 0)
                    oEarningsDeductionsFormula.IsDefault = true;

                moEarningDeductionFormulaBL.EarningsDeductionsFormula = oEarningsDeductionsFormula;
                if (hidMode.Value == S_EDIT_FORMULA)
                {
                    moEarningDeductionFormulaBL.EarningsDeductionsFormula.FormulaId = Convert.ToInt32(hidFormulaId.Value);
                    moEarningDeductionFormulaBL.Update();
                }
                else
                    moEarningDeductionFormulaBL.Insert();
                GetFormulaDetails();
                hidMode.Value = S_NEW_FORMULA;
                btnDeleteFormula.Enabled = true;
                lblError.Text = string.Empty;
                lblMessage.Text = S_FORMULA_SAVE_MESSAGE;
                return true;
            }
            else
            {
                lblError.Text = S_FORMULA_RECURSIVE_MESSAGE;
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// This method is used to check association.
    /// </summary>
    /// <param name="asFormula"></param>
    /// <returns></returns>
    private bool CheckAssociation(string asFormula)
    {
        asFormula = asFormula.Replace("'", "!");
        string[] sfields = asFormula.Split('!');
        string sIdList = string.Empty;
        foreach (string str in sfields)
        {
            if (str.Contains("'"))
                sIdList = sIdList + "," + str.Replace("'", string.Empty);
        }

        if (sIdList != string.Empty)
            return moEarningDeductionFormulaBL.AreConfigured(sIdList);
        
        return false;
    }

    /// <summary>
    /// This method is used to check recursion.
    /// </summary>
    /// <param name="asFormula"></param>
    /// <returns></returns>
    private string CheckRecursion(string asFormula)
    {
        string sEarningsDeductions = string.Empty;
        int iEarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value);
        DataTable oDataTable = (DataTable)ViewState[S_FORMULA_TABLE];
        return GetFormula(oDataTable, iEarningsDeductionsId, asFormula);
    }

    /// <summary>
    /// This method is used to retrieve formula from database.
    /// </summary>
    /// <param name="aoDataTable"></param>
    /// <param name="aiEarningDeductionId"></param>
    /// <param name="asFormula"></param>
    /// <returns></returns>
    private string GetFormula(DataTable aoDataTable, int aiEarningDeductionId, string asFormula)
    {
        int iEarnDeductId;
        string sFormula = string.Empty;
        DataRow[] oDataRow = aoDataTable.Select("Formula LIKE '%''" + aiEarningDeductionId + "''%' AND EarningsDeductionsId <> " + aiEarningDeductionId);
        if (oDataRow.Length > 0)
        {
            foreach (DataRow oDR in oDataRow)
            {
                sFormula = oDR.ItemArray[2].ToString();
                iEarnDeductId = Convert.ToInt32(oDR.ItemArray[1]);

                if (asFormula.Contains("'"+iEarnDeductId.ToString()+"'"))
                {
                    sFormula = "Recursive";
                    break;
                }
                else
                    sFormula = GetFormula(aoDataTable, iEarnDeductId, asFormula);
            }

            return sFormula;
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to validate formula.
    /// </summary>
    /// <returns></returns>
    private string ValidateFormula()
    {
        string sFormula = string.Empty;
        if (hidFormulaValue.Value != string.Empty)
        {
            if (hidFormulaValue.Value.StartsWith(","))
                sFormula = hidFormulaValue.Value.Substring(1);
            else
                sFormula = hidFormulaValue.Value;
        }

        if (IsInvalidFormula(sFormula))
        {
            lblError.Text = S_FORMULA_VALID_MESSAGE;
            lblMessage.Text = string.Empty;
            return string.Empty;
        }
        else
            return hidFormulaValue.Value;
    }

    /// <summary>
    /// This method is used to check whether current formula is valid? 
    /// </summary>
    /// <param name="sFormula"></param>
    /// <returns></returns>
    private bool IsInvalidFormula(string sFormula)
    {
        bool bIsOperator = false;
        string[] sFields = sFormula.Split(',');
        int iFieldLength = sFields.Length;
        int iLoopCounter = GetFormulaLength(sFormula, ref bIsOperator);

        if (iFieldLength == iLoopCounter)
        {
            if (!sFormula.StartsWith("'"))
                sFormula = "'" + sFormula;
            sFormula = sFormula.Replace("%", "/100");
            sFormula = sFormula.Replace("'", string.Empty).Replace(",", string.Empty);

            MathsExpressionParser oMathsExpressionParser = new MathsExpressionParser();
            if (oMathsExpressionParser.Evaluate(sFormula))
            {
                lblError.Text = string.Empty;
                lblMessage.Text = string.Empty;
                if (hidFormulaValue.Value.StartsWith(","))
                    hidFormulaValue.Value = hidFormulaValue.Value.Substring(1);
                bIsOperator = false;
            }
            else
                bIsOperator = true;
        }

        return bIsOperator;
    }

    /// <summary>
    /// This formula is used to return formula length to validate formula.
    /// </summary>
    /// <param name="asFormula"></param>
    /// <param name="abIsOperator"></param>
    /// <returns></returns>
    private int GetFormulaLength(string asFormula, ref bool abIsOperator)
    {
        string sOperators = "+,-,*,/,%";
        string sParenthesis = "(,)";

        string[] sFields = asFormula.Split(',');
        int iFieldLength = sFields.Length;
        int iLoopCounter = 0;
        hidChildIds.Value = string.Empty;
        string sPrevsField = string.Empty;
        foreach (string sField in sFields)
        {
            if (sField.Contains("'"))
            {
                hidChildIds.Value = hidChildIds.Value + "," + sField.Replace("'", string.Empty);
                if (abIsOperator)
                    break;
                else if (!abIsOperator && !sOperators.Contains(sPrevsField) && !sParenthesis.Contains(sPrevsField))
                {
                    abIsOperator = true;
                    break;
                }

                abIsOperator = true;
            }
            else
            {
                if (abIsOperator && !sOperators.Contains(sField) && !sParenthesis.Contains(sField))
                {
                    abIsOperator = true;
                    break;
                }
                
                if (sField == ")")
                    abIsOperator = true;
                else
                    abIsOperator = false;
                sPrevsField = sField;
            }

            iLoopCounter++;
        }

        return iLoopCounter;
    }

    #endregion

    #region Range Methods

    /// <summary>
    /// This method is used to populate amount ranges.
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    private AmountRange PopulateAmountRanges(object sender)
    {   
        Button oButton = sender as Button;
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)oButton.Parent.Parent;       
        AmountRange oAmountRange = new AmountRange
        {
            AmountRangeId = Convert.ToInt32(hidAmountRangeId.Value),
            MonthXml = GenerateMonthXml(oHtmlTableRow)
        };

        oButton.Parent.Parent.Visible = false;
        return oAmountRange;
    }

    /// <summary>
    /// This method is used to save amount ranges.
    /// </summary>
    /// <returns></returns>
    private bool SaveAmountRange(int aiAmountRangeId)
    {
        moAmountRangeBL.AmountRange = new AmountRange
        {
            EarningsDeductionsId = Convert.ToInt32(hidEarningsDeductionsId.Value),            
            UpdateMonthwiseAmount = Convert.ToChar(hidUpdateMonthwiseAmount.Value),
            AmountRangeXml = GenerateAmoutRangeXml(aiAmountRangeId),
            RangeName = txtFormulaName.Text.Trim()
        };
        moAmountRangeBL.Insert();
        FillAmountRanges();
        lblError.Text = string.Empty;
        lblMessage.Text = S_AMOUNT_SAVE_MESSAGE;
        hidMode.Value = S_EDIT_RANGE;
        return true;
    }

    /// <summary>
    /// This method is used to generate xml string of selected amount ranges.
    /// </summary>
    /// <returns></returns>
    private string GenerateAmoutRangeXml(int aiAmountRangeId)
    {
        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("AmountRange");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "AmountRange", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwAmountRange.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwAmountRange.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            int iAmountRangeId = Convert.ToInt32(lstvwAmountRange.DataKeys[iRowCount]["AmountRangeId"]);

            CheckBox ChkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelect");
            if (ChkSelect.Checked && iAmountRangeId == aiAmountRangeId)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "AmountRange", string.Empty);

                sAttribute = "AmountRangeId";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iAmountRangeId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "FromAmount";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtFromAmount = (TextBox)oCurrentItem.FindControl("txtFromAmount");
                decimal iFromAmount = Convert.ToDecimal(txtFromAmount.Text.Trim());
                attr.Value = iFromAmount.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "UptoAmount";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtUptoAmount = (TextBox)oCurrentItem.FindControl("txtUptoAmount");
                decimal iUptoAmount = Convert.ToDecimal(txtUptoAmount.Text.Trim());
                attr.Value = iUptoAmount.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Amount";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtAmount = (TextBox)oCurrentItem.FindControl("txtAmount");
                attr.Value = Convert.ToDecimal(txtAmount.Text.Trim()).ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Is_Deleted";
                attr = oDoc.CreateAttribute(sAttribute);
                if (ChkSelect.Checked)
                    attr.Value = Constants.S_NO;
                else
                    attr.Value = Constants.S_YES;
                oXmlNode.Attributes.Append(attr);

                sAttribute = "RangeName";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = txtFormulaName.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "IsDefault";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = chkIsDefault.Checked ? Constants.S_ONE : Constants.S_ZERO;
                oXmlNode.Attributes.Append(attr);

                sAttribute = "OldRangeName";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = hidOldRangeName.Value;
                oXmlNode.Attributes.Append(attr);

                sAttribute = "RangeId";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = hidRangeId.Value;
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This methos is used to generate xml string of selected month wise amounts.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <returns></returns>
    private string GenerateMonthXml(HtmlTableRow aoHtmlTableRow)
    {
        ListView lstvwMonthRange = (ListView)aoHtmlTableRow.FindControl("lstvwRange");
        int iItemCount = lstvwMonthRange.Items.Count;
        string sAttribute;

        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("MonthRange");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "MonthRange", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < iItemCount; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwMonthRange.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "MonthRange", string.Empty);

            sAttribute = "MonthwiseAmountId";
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            int iMonthwiseAmountId = Convert.ToInt32(lstvwMonthRange.DataKeys[iRowCount]["MonthwiseAmountId"]);
            attr.Value = iMonthwiseAmountId.ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "MonthId";
            attr = oDoc.CreateAttribute(sAttribute);
            int iMonthId = Convert.ToInt32(lstvwMonthRange.DataKeys[iRowCount]["MonthID"]);
            attr.Value = iMonthId.ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "Amount";
            attr = oDoc.CreateAttribute(sAttribute);
            TextBox txtAmount = (TextBox)oCurrentItem.FindControl("txtAmount");
            attr.Value = Convert.ToDecimal(txtAmount.Text.Trim()).ToString();
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to fill amount ranges.
    /// </summary>
    private void FillAmountRanges()
    {
        optRange.Checked = true;
        HideRangeControls(false);        
        int iEarningDeductionId = Convert.ToInt32(hidEarningsDeductionsId.Value);
        DataSet oDSAmountRangeTable = moAmountRangeBL.GetAmountRanges(iEarningDeductionId);         
        SetAmountRanges(oDSAmountRangeTable);        
    }

    /// <summary>
    /// This method is used to set amount ranges.
    /// </summary>
    /// <param name="aoDSAmountRanges"></param>
    private void SetAmountRanges(DataSet aoDSAmountRanges)
    {
        if (aoDSAmountRanges != null)
        {
            DataTable oDTRanges = aoDSAmountRanges.Tables[I_RANGE_TABLE_INDEX];
            ViewState[S_RANGE_TABLE] = oDTRanges;
            string sCondition = string.Empty;
            sCondition = "IsDefault=1";            
            if (oDTRanges.IsNonEmpty())
            {
                // hide formula/range grid if selected ED is professional tax.
                if (oDTRanges.Rows[0]["OriginalEarningsDeductionsId"].ToString() == Constants.I_PROFESSIONL_TAX)
                {
                    lstvwFormula.Visible = false;
                    btnCancel.Visible = false;                    
                    trLegend.Visible = false;
                }
               
                // if selected range is not default range.
                if (hidDefaultRange.Value == Constants.S_NO)
                {
                    if (hidRangeId.Value == Constants.S_ZERO)
                        sCondition = "RangeId=" + oDTRanges.Rows[oDTRanges.Rows.Count - 1]["RangeId"];
                    else
                        sCondition = "RangeId=" + hidRangeId.Value;
                }
                
                if (oDTRanges.Select(sCondition) != null)
                {
                    DataTable oDTAmountRanges = null;
                    if (oDTRanges.Select(sCondition).Length > 0)
                    {
                        oDTAmountRanges = oDTRanges.Select(sCondition).CopyToDataTable();

                        if (oDTRanges.Rows[0]["OriginalEarningsDeductionsId"].ToString() != Constants.I_PROFESSIONL_TAX)
                            FillRangeGrid(aoDSAmountRanges);

                        FillAmountRangesTable(oDTAmountRanges);

                        // Fill amount range listview and select checkbox is it is default.
                        lstvwAmountRange.DataSource = oDTAmountRanges;
                        lstvwAmountRange.DataBind();
                        if (hidDefaultRange.Value == Constants.S_YES)
                        {
                            chkIsDefault.Enabled = false;
                            chkIsDefault.Checked = true;
                        }
                    }
                    else
                        FillRangeGrid(aoDSAmountRanges);
                }
            }
            else
            {
                // Set empty grid.
                FillEmptyAmountRangeGrid();
                chkIsDefault.Enabled = false;
                chkIsDefault.Checked = true;           
            }
        }
    }

    /// <summary>
    /// Create a data table with default values and set it list view.
    /// </summary>
    private void FillEmptyAmountRangeGrid()
    {
        DataTable oDataTable = new DataTable();
        string[] sColumns = {
                                "AmountRangeId", "EarningsDeductionsId", "FromAmount", 
                                "UptoAmount", "Amount", "HasRange", "RangeName", 
                                "IsDefault", "RangeId"
                            };
        oDataTable.AddColumns(sColumns);

        int iCount = 0;
        while (iCount < 10)
        {
            DataRow oDataRow = oDataTable.NewRow();
            oDataRow["AmountRangeId"] = 0;
            oDataRow["EarningsDeductionsId"] = hidEarningsDeductionsId.Value;
            oDataRow["FromAmount"] = 0;
            oDataRow["UptoAmount"] = 0;
            oDataRow["Amount"] = 0;
            oDataRow["HasRange"] = false;
            oDataRow["RangeName"] = string.Empty;
            oDataRow["IsDefault"] = false;
            oDataRow["RangeId"] = 0;
            oDataTable.Rows.Add(oDataRow);
            iCount++;
        }

        lstvwAmountRange.DataSource = oDataTable;
        lstvwAmountRange.DataBind();        
    }

    /// <summary>
    /// This method is used to fill range list view.
    /// </summary>
    /// <param name="aoDSAmountRanges"></param>
    /// <returns></returns>
    private DataTable FillRangeGrid(DataSet aoDSAmountRanges)
    {
        const int I_RANGES = 1;
        DataTable oDataTable = new DataTable();
        string[] sColumns = { "FormulaId", "FormulaName", "FormulaValue", "IsDefault", "EarningsDeductionsId" };
        oDataTable.AddColumns(sColumns);
        if (aoDSAmountRanges != null)
        {   
            DataTable oDTRangeNames = aoDSAmountRanges.Tables[I_RANGES];
            string sUptoAmount;
            string sAmount;
            string sRange;
            string sFromAmount;

            // Get all the ranges.
            foreach (DataRow oDRRangeNames in oDTRangeNames.Rows)
            {
                sUptoAmount = string.Empty;
                sAmount = string.Empty;
                sRange = string.Empty;
                DataRow oDataRow = null;
                sFromAmount = string.Empty;

                // Get range details of current range.
                var amountRanges = from amountRange in aoDSAmountRanges.Tables[I_RANGE_TABLE_INDEX].AsEnumerable()
                                    where amountRange.Field<int>("RangeId") == Convert.ToInt32(oDRRangeNames["RangeId"])
                                    select new
                                    {
                                        FromAmount = amountRange["FromAmount"].ToString(),
                                        UptoAmount = amountRange["UptoAmount"].ToString(),
                                        Amount = amountRange["Amount"].ToString()
                                    };

                if (amountRanges.Count() > 0)
                {                    
                    oDataRow = oDataTable.NewRow();
                    foreach (var amountRange in amountRanges)
                    {
                        sFromAmount = amountRange.FromAmount;
                        sUptoAmount = amountRange.UptoAmount;
                        sAmount = amountRange.Amount;
                        sRange = sRange + sFromAmount + " - " + sUptoAmount + " -> " + sAmount + "<BR />";
                    }

                    oDataRow["EarningsDeductionsId"] = hidEarningsDeductionsId.Value;
                    oDataRow["FormulaName"] = oDRRangeNames["RangeName"];
                    oDataRow["FormulaId"] = oDRRangeNames["RangeId"];
                    oDataRow["IsDefault"] = oDRRangeNames["IsDefault"];                    
                    oDataRow["FormulaValue"] = sRange;
                    oDataTable.Rows.Add(oDataRow);
                }
            }
        }

        FillFormulaGrid(oDataTable);
        return oDataTable;
    }

    /// <summary>
    /// This method is used to add default values into data table if row count is less than 10 then remaining rows will fill with 0's.
    /// </summary>
    /// <param name="aoDTAmountRanges"></param>
    private void FillAmountRangesTable(DataTable aoDTAmountRanges)
    {
        DataRow oDataRow = null;
        string sRangeName = string.Empty;
        bool bIsDefault = false;
        int iRowCount = 0;

        if (aoDTAmountRanges != null)
            iRowCount = aoDTAmountRanges.Rows.Count;

        if (iRowCount > 0)
        {
            sRangeName = aoDTAmountRanges.Rows[0]["RangeName"].ToString();
            bIsDefault = Convert.ToBoolean(aoDTAmountRanges.Rows[0]["IsDefault"]);
            txtFormulaName.Text = sRangeName;
            chkIsDefault.Checked = bIsDefault;
            hidOldRangeName.Value = sRangeName;
            hidRangeId.Value = aoDTAmountRanges.Rows[0]["RangeId"].ToString();
        }

        while (iRowCount < 10)
        {
            oDataRow = aoDTAmountRanges.NewRow();
            oDataRow["AmountRangeId"] = 0;
            oDataRow["EarningsDeductionsId"] = hidEarningsDeductionsId.Value;
            oDataRow["FromAmount"] = 0;
            oDataRow["UptoAmount"] = 0;
            oDataRow["Amount"] = 0;
            oDataRow["HasRange"] = 0;
            oDataRow["RangeName"] = sRangeName;
            oDataRow["IsDefault"] = bIsDefault;
            aoDTAmountRanges.Rows.Add(oDataRow);
            iRowCount++;
        }
    }

    /// <summary>
    /// This method is used to hide all the visible list view except selected.
    /// </summary>
    /// <param name="aiRowId"></param>
    private void HideOtherMonthViews(int aiRowId)
    {
        int iItemCount = lstvwAmountRange.Items.Count;
        int iRowId;
        foreach (ListViewDataItem oCurrentItem in lstvwAmountRange.Items)
        {
            iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            if (iRowId != aiRowId)
            {
                System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trlstvwRange") as System.Web.UI.HtmlControls.HtmlTableRow;
                oHtmlTableRow.Visible = false;
            }
        }
    }

    /// <summary>
    /// This method is used to check whether current ED is used in formula.
    /// </summary>
    /// <param name="aiAmountRangeId"></param>
    /// <returns></returns>
    private bool IsDependentFormula(int aiAmountRangeId)
    {
        bool bHasDependentFormula = false;
        DataTable oDTFormula = (DataTable)ViewState[S_FORMULA_TABLE];
        foreach (DataRow row in oDTFormula.Rows)
        {
            string sFormula = Convert.ToString(row["Formula"]);
            if (sFormula.Contains("'" + hidEarningsDeductionsId.Value + "'"))
                bHasDependentFormula = true;
        }

        if (bHasDependentFormula == true)
        {
            lblError.Text = lblFormulaField.Text + " is already used in formula, so you can not add range to it.";
            btnDeleteFormula.Enabled = false;
        }

        return bHasDependentFormula;
    }

    /// <summary>
    /// This method is used to hide buttons.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideButtons(bool abAction)
    {
        BtnClose.Visible = !abAction;
    }

    /// <summary>
    /// This method is used to disable list view.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisableListview(bool abAction)
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwAmountRange.FindControl("trHeader");
        CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkAll");
        oCheckBox.Enabled = !abAction;
        int iRowIndex = 0;
        int iItemCount = lstvwAmountRange.Items.Count;
        foreach (ListViewDataItem oItem in lstvwAmountRange.Items)
        {
            iRowIndex = oItem.DisplayIndex;
            CheckBox chkSelect = (CheckBox)oItem.FindControl("chkSelect");
            TextBox txtFromAmount = (TextBox)oItem.FindControl("txtFromAmount");
            TextBox txtUptoAmount = (TextBox)oItem.FindControl("txtUptoAmount");
            TextBox txtAmount = (TextBox)oItem.FindControl("txtAmount");
            Button btnSaveRange = (Button)oItem.FindControl("BtnSaveRange");

            chkSelect.Enabled = !abAction;

            if (!abAction && !chkSelect.Checked)
                abAction = true;
            txtFromAmount.Enabled = !abAction;
            txtUptoAmount.Enabled = !abAction;
            txtAmount.Enabled = !abAction;
            btnSaveRange.Enabled = !abAction;            
        }
    }

    #endregion

    #endregion   
}