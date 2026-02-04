/* File NAme :- EarningsAndDeductions.aspx.cs
 * Created By:- Sachin
 * Created Date :- 24-Oct-2009
 * Class Description :- This class is used to configure EarningsAnd and Deductions.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Linq;
using PayrollEntities;
using Utility;

public partial class EarningsAndDeductions : SchoolBase
{
    #region Constants

    const string S_DATAKEY_EARNINGS_DEDUCTIONS_ID = "EarningsDeductionsId";
    const string S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID = "OriginalEarningsDeductionsId";
    const string S_DATAKEY_SCHOOL_ID = "SchoolId";
    const string S_SUCCESS_MESSAGE = "Earnings and Deductions has been saved successfully !!!";
    const string S_ADD_FORMULA = "Add Formula";
    const string S_EDIT_FORMULA = "Edit Formula";
    const string S_EDIT_RANGE = "Edit Range";
    const string S_DATAKEY_ISATTENDANCEDEPENDENT = "IsAttendanceDependent";

    #endregion

    #region Members

    private string msEarningDeductionName;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill earnings and deductions gridview and set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {                
                SetJavascriptAttributes();
                FillEarningsDeductionsGrid();
                SetScreenWidth();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on formula link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEarnings_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                EarningsDeductions oEarningsDeductions = (EarningsDeductions)oCurrentItem.DataItem;
                // If the school id is not the default id i.e. -9999 that means the earning-deduction is already assigned
                // to the school. Thus check the checkbox.

                CheckBox ChkSelect = ((CheckBox)oCurrentItem.FindControl("ChkSelect"));
                CheckBox ChkIsAttendanceDependent = ((CheckBox)oCurrentItem.FindControl("ChkIsAttendanceDependent"));
                CheckBox chkIncludeInSalaryDifference = ((CheckBox)oCurrentItem.FindControl("chkIncludeInSalaryDifference"));
                
                ChkSelect.Attributes.Add("onclick", "VisibleFormulaLink('Earning'," + iRowId + ");");

                if (lstvwEarnings.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID ||
                    (lstvwEarnings.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID &&
                    Convert.ToInt32(lstvwEarnings.DataKeys[iRowId][S_DATAKEY_ISATTENDANCEDEPENDENT]) == 1))
                {
                    ChkSelect.Checked = true;
                    ChkIsAttendanceDependent.Visible = true;
                    chkIncludeInSalaryDifference.Visible = true;
                }
                else
                {
                    ChkIsAttendanceDependent.Visible = false;
                    chkIncludeInSalaryDifference.Visible = false;
                }

                if (Convert.ToInt32(lstvwEarnings.DataKeys[iRowId][S_DATAKEY_ISATTENDANCEDEPENDENT]) == 1 && lstvwEarnings.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    ChkIsAttendanceDependent.Checked = true;

                TextBox txtName = (TextBox)(oCurrentItem.FindControl("txtEarningsDeductionsName"));
                txtName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
                TextBox txtShortName = (TextBox)(oCurrentItem.FindControl("txtEarningsDeductionsShortName"));
                txtShortName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");

                if (lstvwEarnings.DataKeys[iRowId]["IsBasic"] != DBNull.Value && Convert.ToBoolean(lstvwEarnings.DataKeys[iRowId]["IsBasic"]) == true)
                {
                    ChkSelect.Checked = true;
                    ChkIsAttendanceDependent.Checked = true;
                    chkIncludeInSalaryDifference.Checked = oEarningsDeductions.IncludeInSalaryDifference;
                    ChkIsAttendanceDependent.Enabled = false;
                    ChkSelect.Enabled = false;                    
                }

                char bHasFormula = Convert.ToChar(oEarningsDeductions.FormulaOrRange);
                LinkButton lnkbtnEditFormula = ((LinkButton)oCurrentItem.FindControl("lnkbtnEditFormula"));
                lnkbtnEditFormula.ToolTip = oEarningsDeductions.Formula;

                if (ChkSelect.Checked && ChkSelect.Enabled && lstvwEarnings.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    ChangeFormulaLinkName(lnkbtnEditFormula, bHasFormula);
                else
                    lnkbtnEditFormula.Visible = false;
                if (ChkSelect.Checked)
                {
                    int iEarningDeductionId = Convert.ToInt32(lstvwEarnings.DataKeys[iRowId][S_DATAKEY_EARNINGS_DEDUCTIONS_ID]);
                    string sEncryptedString = CreateEncryptedQueryString(lnkbtnEditFormula, iEarningDeductionId, txtName, txtShortName);
                    msEarningDeductionName = msEarningDeductionName + "," + txtShortName.Text;
                    lnkbtnEditFormula.Attributes.Add("onclick", "OpenFormulaPopup('" + sEncryptedString + "'," + iRowId + ",'Earning');return false;");
                }
                if (Convert.ToInt32(lstvwEarnings.DataKeys[iRowId][S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID]) >= 1 && (Convert.ToInt32(lstvwEarnings.DataKeys[iRowId][S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID]) <= 9))
                    txtName.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on formula link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDeductions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                EarningsDeductions oEarningsDeductions = (EarningsDeductions)oCurrentItem.DataItem;
                // If the school id is not the default id i.e. -9999 that means the earnings-deductions is already assigned
                // to the school. Thus check the checkbox.
                CheckBox ChkSelect = ((CheckBox)oCurrentItem.FindControl("ChkSelect"));
                ChkSelect.Attributes.Add("onclick", "VisibleFormulaLink('Deduction'," + iRowId + ");");
                CheckBox ChkIsAttendanceDependent = ((CheckBox)oCurrentItem.FindControl("ChkIsAttendanceDependent"));
                TextBox txtName = (TextBox)(oCurrentItem.FindControl("txtEarningsDeductionsName"));
                txtName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");

                CheckBox chkIncludeInSalaryDifference = ((CheckBox)oCurrentItem.FindControl("chkIncludeInSalaryDifference"));

                if (lstvwDeductions.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID ||
                    (lstvwDeductions.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID &&
                    Convert.ToInt32(lstvwDeductions.DataKeys[iRowId][S_DATAKEY_ISATTENDANCEDEPENDENT]) == 1))
                {
                    ChkSelect.Checked = true;
                    ChkIsAttendanceDependent.Visible = true;
                    chkIncludeInSalaryDifference.Visible = true;

                }
                else
                {
                    ChkIsAttendanceDependent.Visible = false;
                    chkIncludeInSalaryDifference.Visible = false;
                }

                if (Convert.ToInt32(lstvwDeductions.DataKeys[iRowId][S_DATAKEY_ISATTENDANCEDEPENDENT]) == 1 && lstvwDeductions.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID && ChkSelect.Checked)
                    ChkIsAttendanceDependent.Checked = true;
                else
                    ChkIsAttendanceDependent.Checked = false;

                if (lstvwDeductions.DataKeys[iRowId]["IsBasic"] != DBNull.Value && Convert.ToBoolean(lstvwDeductions.DataKeys[iRowId]["IsBasic"]) == true)
                {
                    ChkSelect.Checked = true;
                    ChkIsAttendanceDependent.Checked = true;
                    ChkIsAttendanceDependent.Enabled = false;
                    ChkSelect.Enabled = false;
                    chkIncludeInSalaryDifference.Checked = oEarningsDeductions.IncludeInSalaryDifference;
                }

                char bHasFormula = Convert.ToChar(oEarningsDeductions.FormulaOrRange);
                LinkButton lnkbtnEditFormula = ((LinkButton)oCurrentItem.FindControl("lnkbtnEditFormula"));
                lnkbtnEditFormula.ToolTip = oEarningsDeductions.Formula;

                if (ChkSelect.Checked)
                    ChangeFormulaLinkName(lnkbtnEditFormula, bHasFormula);
                else
                    lnkbtnEditFormula.Visible = false;

                TextBox txtShortName = (TextBox)(oCurrentItem.FindControl("txtEarningsDeductionsShortName"));
                txtShortName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
                if (ChkSelect.Checked)
                {
                    int iEarningDeductionId = Convert.ToInt32(lstvwDeductions.DataKeys[iRowId][S_DATAKEY_EARNINGS_DEDUCTIONS_ID]);
                    string sEncryptedString = CreateEncryptedQueryString(lnkbtnEditFormula, iEarningDeductionId, txtName, txtShortName);
                    msEarningDeductionName = msEarningDeductionName + "," + txtShortName.Text;
                    lnkbtnEditFormula.Attributes.Add("onclick", "OpenFormulaPopup('" + sEncryptedString + "'," + iRowId + ",'Deduction');return false;");
                }

                if (Convert.ToInt32(lstvwDeductions.DataKeys[iRowId][S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID]) >= 15 && (Convert.ToInt32(lstvwDeductions.DataKeys[iRowId][S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID]) <= 19))
                    txtName.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save earning dductions and add entry into configuration table if it already not exist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sShortNames = GetShortNames();
            string sMessage = EarningsDeductionsBL.ValidateShortName(miSchoolId, miAcademicYearId, sShortNames, true);

            if (string.IsNullOrEmpty(sMessage))
            {
                Save();
                if (QueryString["Is_Configured"] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.EarningsAndDeductions));

                lblMessage.Visible = true;
                lblMessage.Text = S_SUCCESS_MESSAGE;
                lblErr.Text = string.Empty;
                FillEarningsDeductionsGrid();
            }
            else
                lblErr.Text = sMessage;
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = ex.Message;
            lblMessage.Text = string.Empty;
            FillEarningsDeductionsGrid();
        }
        catch (SqlException ex)
        {
            lblErr.Text = ex.Message;
            lblMessage.Text = string.Empty;
            FillEarningsDeductionsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method

    private void SetScreenWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str);
            
            iWidth = iWidth / 100 * 40;
            divEarnings.Style.Add("width", iWidth.ToString() + "px !important");
            divDeductions.Style.Add("width", iWidth.ToString() + "px !important");
        }
        else
        {
            divEarnings.Style.Add("width", Convert.ToString(1024) + "px !important");
            divDeductions.Style.Add("width", Convert.ToString(1024) + "px !important");
        }
    }

    /// <summary>
    /// This method is used to fill Earnings and Deductions gridview.
    /// </summary>
    private void FillEarningsDeductionsGrid()
    {   
        List<EarningsDeductions> lstEarningsDeductions = EarningsDeductionsBL.GetAll(miSchoolId);
        FillEarningsGrid(lstEarningsDeductions);
        FillDeductionGrid(lstEarningsDeductions);
    }

    /// <summary>
    /// This method is used to fill earning grid.
    /// </summary>
    /// <param name="aoDTEarnings"></param>
    private void FillEarningsGrid(List<EarningsDeductions> alstEarningsDeductions)
    {
        var oEarnings = alstEarningsDeductions.Where(ed => ed.IsEarning);                    
        lstvwEarnings.DataSource = oEarnings;
        lstvwEarnings.DataBind();

        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwEarnings.FindControl("trHeader");
        CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkAll");
        oCheckBox.Focus();
    }

    /// <summary>
    /// This method is used to fill deduction grid.
    /// </summary>
    /// <param name="oDTEarningsDeductions"></param>
    private void FillDeductionGrid(List<EarningsDeductions> alstEarningsDeductions)
    {
        var oEarnings = alstEarningsDeductions.Where(ed => !ed.IsEarning);                    
        lstvwDeductions.DataSource = oEarnings;
        lstvwDeductions.DataBind();
    }

    /// <summary>
    /// This method is used to set javascript attributes and postback url to cancel button.
    /// </summary>
    protected void SetJavascriptAttributes()
    {        
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        BtnSave.Attributes.Add("onclick", "if(!CheckSelectedEarningDeductions()) return false;");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel });        
    }

    /// <summary>
    /// This method is used to save Earnings and Deductions into database.
    /// </summary>
    protected void Save()
    {   
        List<EarningsDeductions> oEarningsCollection = GetCollection(lstvwEarnings, true);
        List<EarningsDeductions> oDeductionCollection = GetCollection(lstvwDeductions, false);

        // Update database with the configured earning deductions.
        if (oEarningsCollection.Count > 0)
        {
            EarningsDeductionsBL oEarningsDeductionsBL = new EarningsDeductionsBL();
            oEarningsDeductionsBL.Update(oEarningsCollection, oDeductionCollection, miAcademicYearId, miSchoolId);
        }
    }

    /// <summary>
    /// This method is used to to add objects into list and return it.
    /// </summary>
    /// <param name="oEarningsDeductions"></param>
    /// <returns></returns>
    private List<EarningsDeductions> GetCollection(ListView alstvwEarningsDeduction, bool abIsEarnings)
    {
        bool bIsChanged;
        CheckBox chkSelect;
        List<EarningsDeductions> lstEarningsDeductions = new List<EarningsDeductions>();        
        for (int iItemIndex = 0; iItemIndex < alstvwEarningsDeduction.Items.Count; iItemIndex++)
        {
            bIsChanged = true;
            ListViewDataItem oCurrentItem = (ListViewDataItem)alstvwEarningsDeduction.Items[iItemIndex];
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
            chkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelect");
            EarningsDeductions oEarningsDeduction = PopulateEarningsDeductions(oCurrentItem, abIsEarnings);

            // Check if new entry is being inserted.
            // I.e. If the checkbox is checked and the school id is -9999 then it is the new entry being
            // introduced.
            if (chkSelect.Checked && alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
            {
                oEarningsDeduction.OriginalEarningsDeductionsId = Convert.ToInt32(alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_EARNINGS_DEDUCTIONS_ID]);
                oEarningsDeduction.Action = Constants.Action.Insert;                
            }

            // Check if existing name is being updated.
            // I.e. If the checkbox is checked and the school is not -9999 then update the existing name.
            else if (chkSelect.Checked &&
                    alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                oEarningsDeduction.OriginalEarningsDeductionsId = Convert.ToInt32(alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_ORIGINAL_EARNINGS_DEDUCTIONS_ID]);
                oEarningsDeduction.Action = Constants.Action.Update;                
                oEarningsDeduction.EarningsDeductionsId = Convert.ToInt32(alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_EARNINGS_DEDUCTIONS_ID].ToString());                
            }

            // Check if existing category is being removed.
            // I.e. If the checkbox is NOT checked and the school id is not -9999. 
            // In such case need to check if any of the related data is entered for the unchecked category then
            // the warning message should be given to user.
            else if (!chkSelect.Checked && alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                oEarningsDeduction.Action = Constants.Action.Delete;                
                oEarningsDeduction.EarningsDeductionsId = Convert.ToInt32(alstvwEarningsDeduction.DataKeys[iItemIndex][S_DATAKEY_EARNINGS_DEDUCTIONS_ID]);                
            }
            else
                bIsChanged = false;

            if (bIsChanged)
                lstEarningsDeductions.Add(oEarningsDeduction);

        }
        return lstEarningsDeductions;
    }

    /// <summary>
    /// This method is used to populate earnings deductions entity.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    /// <param name="oEarningsDeductionsEntity"></param>
    /// <param name="abIsEarnings"></param>
    private EarningsDeductions PopulateEarningsDeductions(ListViewDataItem aoCurrentItem, bool abIsEarnings)
    {
        int iRowId = Convert.ToInt32(aoCurrentItem.DisplayIndex);
        TextBox txtEarningsDeductionsName = (TextBox)aoCurrentItem.FindControl("txtEarningsDeductionsName");
        TextBox txtShortName = (TextBox)aoCurrentItem.FindControl("txtEarningsDeductionsShortName");
        CheckBox chkIsAttendanceDependent = (CheckBox)aoCurrentItem.FindControl("ChkIsAttendanceDependent");
        CheckBox chkIncludeInSalaryDifference = (CheckBox)aoCurrentItem.FindControl("chkIncludeInSalaryDifference");

        bool bHasFormula = false;
        LinkButton lnkbtnEditFormula = ((LinkButton)aoCurrentItem.FindControl("lnkbtnEditFormula"));
        if (lnkbtnEditFormula.Text == S_EDIT_FORMULA || lnkbtnEditFormula.Text == S_EDIT_RANGE)
            bHasFormula = true;

        EarningsDeductions EarningsDeductions = new EarningsDeductions();

        EarningsDeductions.EarningsDeductionsName = txtEarningsDeductionsName.Text.Trim();
        EarningsDeductions.ShortName = txtShortName.Text.Trim();
        //if (oEarningsDeductionsStruct.eAction != Constants.Action.Delete)
        //{
            EarningsDeductions.IsEarning = abIsEarnings;
            EarningsDeductions.IsAttendanceDependent = chkIsAttendanceDependent.Checked; ;
            EarningsDeductions.IncludeInSalaryDifference = chkIncludeInSalaryDifference.Checked;
            EarningsDeductions.HasFormula = bHasFormula;

            if (lstvwEarnings.DataKeys[iRowId]["IsBasic"] != DBNull.Value &&
                lstvwEarnings.DataKeys[iRowId]["IsBasic"].ToString() != "False" &&
                abIsEarnings)
                EarningsDeductions.IsBasic = true;
        //}
        EarningsDeductions.SchoolId = miSchoolId;
        EarningsDeductions.InsertedById = miUserId;
        EarningsDeductions.UpdatedById = miUserId;

        return EarningsDeductions;
    }

    /// <summary>
    /// This method is used to change link name.
    /// </summary>
    /// <param name="lnkbtnEditFormula"></param>
    /// <param name="bHasFormula"></param>
    private void ChangeFormulaLinkName(LinkButton lnkbtnEditFormula, char bHasFormula)
    {
        lnkbtnEditFormula.Visible = true;
        if (bHasFormula == 'F')
            lnkbtnEditFormula.Text = S_EDIT_FORMULA;
        else if (bHasFormula == 'R')
            lnkbtnEditFormula.Text = S_EDIT_RANGE;
        else
            lnkbtnEditFormula.Text = S_ADD_FORMULA;
    }

    /// <summary>
    /// This method is used to create encrypted query string.
    /// </summary>
    /// <param name="lnkbtnEditFormula"></param>
    /// <param name="iEarningDeductionId"></param>
    /// <param name="txtName"></param>
    /// <param name="txtShortName"></param>
    /// <returns></returns>
    private string CreateEncryptedQueryString(LinkButton lnkbtnEditFormula, int iEarningDeductionId, TextBox txtName, TextBox txtShortName)
    {
        string sMode = "NEW FORMULA";
        if (lnkbtnEditFormula.Text == S_EDIT_FORMULA)
            sMode = "EDIT FORMULA";
        else if (lnkbtnEditFormula.Text == S_EDIT_RANGE)
            sMode = "EDIT RANGE";
        string sQueryString = "EarningDeductionId=" + iEarningDeductionId +
                              "&EarningDeductionName=" + txtName.Text + " (" + txtShortName.Text + ")" +
                              "&Mode=" + sMode;
        string sEncryptedString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        return sEncryptedString;
    }

    /// <summary>
    /// This method is used to return short name.
    /// </summary>
    /// <returns></returns>
    private string GetShortNames()
    {
        string sShortNames = GetShortNames(lstvwEarnings);
        if (!string.IsNullOrEmpty(sShortNames))
            sShortNames = sShortNames + "," + GetShortNames(lstvwDeductions);
        else
            sShortNames = GetShortNames(lstvwDeductions);
        return sShortNames;
    }

    /// <summary>
    /// This method is used to return short names.
    /// </summary>
    /// <param name="lsttvwEarningsDeductions"></param>
    /// <returns></returns>
    private string GetShortNames(ListView lsttvwEarningsDeductions)
    {
        string sShortName = string.Empty;
        foreach (ListViewItem oCurrentitem in lsttvwEarningsDeductions.Items)
        {
            CheckBox oCheckBox = (CheckBox)oCurrentitem.FindControl("ChkSelect");
            if (oCheckBox.Checked == true)
            {
                TextBox txtShortName = (TextBox)oCurrentitem.FindControl("txtEarningsDeductionsShortName");
                sShortName = sShortName + "," + txtShortName.Text.Trim();
            }
        }

        if (!string.IsNullOrEmpty(sShortName))
            sShortName = sShortName.Substring(1);
        return sShortName;
    }

    #endregion
}
