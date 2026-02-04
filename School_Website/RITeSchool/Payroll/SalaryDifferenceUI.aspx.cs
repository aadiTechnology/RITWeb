/*
 * File Name :- SalaryDifferenceUI.aspx.cs
 * Created By Id :- Sachin
 * Created Date :- 25-July-2010
 * Class Description :- This class is used to display and save salary difference details.
 * 
 * Modified By  -Sachin
 * Modified Date - 10 August 2012
 * Description - Facility to calculate salary difference aginst selected month.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Text;

public partial class SalaryDifferenceUI : ExportDataTable
{
    #region Data Members

    int miDifferenceRow = 0;
    int miOldConfigRow = 0;
    int miTotalSalaryDifferenceRows = 0;
    int miSrNo = 1;
    int miLateMarkLeaveIndex = 0;
    int miNetSalaryColumnIndex = 0;
    int miSavedSalaryDifferenceColumnIndex = 0;
    int miPaidSalaryDifferenceColumnIndex = 0;
    DataTable moDTCurrentSalary;
    DataTable moDTPaidSalary;
    SalaryDifferenceBL moSalaryDifferenceBL;
    List<string> mlstColumnsNames;

    #endregion

    #region Constants

    private const int I_SAVE_BUTTON_INDEX = 0;
    private const int I_DELETE_BUTTON_INDEX = 1;
    private const int I_GRID_PAGE_COUNT = 7;
    private const int I_PAGE_SIZE = 22;
    private const int I_SALARY_PAID_STATUS_TABLE_INDEX = 0;

    private const string S_LEAVE_DEDUCTED = "Leave Deducted ";
    private const string S_USER_ID = "UserId";
    private const string S_ATTENDANCE_COLUMN = "Attendance";
    private const string S_SAVE_COMMAND = "SAVE";
    private const string S_DELETE_COMMAND = "DELETE";
    private const string S_SALARY_DIFFERENCE_TABLE = "SalaryDifferenceTable";
    private const string S_HOLIDAY_LEAVE_DEDUCTION = "Holiday Leaves";
    private const string S_COLUMN_DETAILS = "SalaryDifferencecolumnDetail";
    private const string S_SALARY_DIFFERENCE_ROW_COLUMN = "IsSalaryDifferenceRow";
    private const string S_EARNING_DEDUCTIONS = "EarningsDeductions";
    private const string S_OPERATION = "%OPERATION%";
    private const string S_OPERATION_MESSAGE = "Salary Difference has been " + S_OPERATION + " successfully !!!";
    private const string S_NO_RECORD_FOUND_MESSAGE = "No Record Found.";
    private const string S_PAID_SALARY_MONTH_MESSAGE = "Salary difference of selected month has been paid in month(s): ";
    private const string S_SALARY_PAID_MESSAGE = "If salary difference of this month is saved then it will be paid in salary payment of month: ";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill month and year comboboxes as well as set screen width, javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultProperties();
                SetControlWidth();
                SetJavascriptAttributes();
                FillComboboxes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show salary difference.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            hidSelectedPageIndex.Value = Constants.S_ZERO;
            grdSalaryDifference.PageIndex = 0;
            FillSalaryDifferenceGrid();
            SetPagerDetails();
            SetConfigQueryString();
        }
        catch (NoRecordFoundException ex)
        {
            lblNoRecordMessage.Text = ex.Message;
            SetView(false);
            btnDelete.Enabled = false;
            SetPagerDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            hidSelectedPageIndex.Value = PageDropDownList.SelectedIndex.ToString();
            grdSalaryDifference.PageIndex = PageDropDownList.SelectedIndex;
            lblCurrentPage.Text = "Page " + PageDropDownList.SelectedIndex + "  of " + PageDropDownList.Items.Count;
            FillSalaryDifferenceGrid();
            SetPagerDetails();
        }
        catch (NoRecordFoundException ex)
        {
            lblNoRecordMessage.Text = ex.Message;
            SetView(false);
            btnDelete.Enabled = false;
            SetPagerDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle row deleting event of salary difference gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDifference_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    /// <summary>
    /// This event is used to format gridview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDifference_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string sColName = string.Empty;
        try
        {
            const string S_COLUMN_INDICES = "2,3,4,5";
            const int I_NAME_COLUMN_INDEX = 6;
            const int I_DESIGNATION_COLUMN_INDEX = 7;

            TableCellCollection oCells = e.Row.Cells;
            int iCellndex = 0;
            string sColumnName = string.Empty;
            string sUserName = string.Empty;

            FormatNetSalaryColumn(e);

            if (e.Row.RowType == DataControlRowType.DataRow)
                SetDeleteButtonState(e);
            foreach (TableCell cell in oCells)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    cell.Style.Add("padding-left", "5");
                    cell.Style.Add("padding-right", "5");

                    if (S_COLUMN_INDICES.Contains(iCellndex.ToString()) || iCellndex == oCells.Count - 4)
                        cell.Visible = false;
                    else
                    {
                        cell.Wrap = false;
                        cell.HorizontalAlign = HorizontalAlign.Left;
                        // Set right allign for all the numeric fields.
                        if (iCellndex == 2 || iCellndex > 8)
                        {
                            cell.VerticalAlign = VerticalAlign.Middle;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                        }
                        else if (iCellndex <= 1)
                        {
                            cell.VerticalAlign = VerticalAlign.Middle;
                            cell.HorizontalAlign = HorizontalAlign.Center;
                        }
                    }

                    if (cell.Text == PayrollConstants.S_LATE_MARK_LEAVES)
                        miLateMarkLeaveIndex = iCellndex;

                    if (cell.Text == PayrollConstants.S_NET_SALARY)
                        miNetSalaryColumnIndex = iCellndex;

                    if (cell.Text == PayrollConstants.S_SAVED_DIFFERENCE)
                        miSavedSalaryDifferenceColumnIndex = iCellndex;

                    if (cell.Text == PayrollConstants.S_PAID_DIFFERENCE)
                        miPaidSalaryDifferenceColumnIndex = iCellndex;

                    if (cell.Text == PayrollConstants.S_GROSS_SALARY || cell.Text == PayrollConstants.S_TOTAL_DEDUCTION || cell.Text == PayrollConstants.S_NET_SALARY || cell.Text == PayrollConstants.S_NET_DIFFERENCE)
                        hidColumnIndexes.Value = hidColumnIndexes.Value + "[" + iCellndex + "]";

                    if (cell.Text == PayrollConstants.S_TOTAL)
                    {
                        cell.Text = "Total Attendance";
                        cell.HorizontalAlign = HorizontalAlign.Right;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Set tooltip.
                    sColumnName = grdSalaryDifference.HeaderRow.Cells[iCellndex].Text;
                    sUserName = Server.HtmlDecode(e.Row.Cells[I_NAME_COLUMN_INDEX].Text).Replace("''", "'");
                    if (miDifferenceRow != 0 && miDifferenceRow % 3 == 0)
                    {
                        if (iCellndex > I_DESIGNATION_COLUMN_INDEX + 1 && iCellndex <= miNetSalaryColumnIndex)
                        {
                            GridViewRow oOldValue = grdSalaryDifference.Rows[e.Row.RowIndex - 2];
                            GridViewRow oNewRow = grdSalaryDifference.Rows[e.Row.RowIndex - 1];

                            sColName = oOldValue.Cells[iCellndex].Text;

                            int iOldValue = Server.HtmlDecode(oOldValue.Cells[iCellndex].Text).Trim() != string.Empty ? Convert.ToInt32(oOldValue.Cells[iCellndex].Text) : 0;
                            int iNewValue = Server.HtmlDecode(oNewRow.Cells[iCellndex].Text).Trim() != string.Empty ? Convert.ToInt32(oNewRow.Cells[iCellndex].Text) : 0;
                            int iCalcDiffValue = Server.HtmlDecode(e.Row.Cells[iCellndex].Text).Trim() != string.Empty ? Convert.ToInt32(e.Row.Cells[iCellndex].Text) : 0;

                            if (iOldValue == -1)
                                iOldValue = 0;
                            if (iNewValue == -1)
                                iNewValue = 0;

                            int iDiffValue = iNewValue - iOldValue;
                            if (iCalcDiffValue != iDiffValue)
                            {
                                cell.ToolTip = iNewValue + " - " + iOldValue + " - " + (iDiffValue - iCalcDiffValue) + " (already saved or paid) = " + iCalcDiffValue;

                                if (sColumnName != PayrollConstants.S_GROSS_SALARY && sColumnName != PayrollConstants.S_TOTAL_DEDUCTION && sColumnName != PayrollConstants.S_NET_SALARY)
                                {
                                    HyperLink oHyperLink = new HyperLink();
                                    oHyperLink.Style.Add("CssClass", "clsLabel class1");
                                    oHyperLink.Text = cell.Text;
                                    oHyperLink.NavigateUrl = "#";
                                    oHyperLink.Attributes.Add("onclick", "return false;");

                                    var EarningDeductionId = moSalaryDifferenceBL.EarningsDeductions.Where(ED => ED.ShortName == sColumnName || S_LEAVE_DEDUCTED + ED.ShortName == sColumnName).Select(ED => ED.EarningsDeductionsId).FirstOrDefault();

                                    string sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                                                          "&Year=" + cmbYear.SelectedValue +
                                                          "&UserId=" + grdSalaryDifference.DataKeys[e.Row.RowIndex]["UserId"].ToString() +
                                                          "&EarningDeductionId=" + EarningDeductionId +
                                                          "&ShowPaid=0" +
                                                          "&BaseMonthId=" + cmbMonthToCompare.SelectedValue +
                                                          "&BaseYear=" + cmbYearToCompare.SelectedValue +
                                                          "&Filter=" + txtSearch.Text.Trim();
                                    oHyperLink.Attributes.Add("onclick", "openDetailsPopup(this,'" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");

                                    cell.Controls.Add(oHyperLink);
                                }
                            }
                            else
                                cell.ToolTip = "User: " + sUserName + "[" + sColumnName + " Difference]";
                        }
                        else
                            cell.ToolTip = "User: " + sUserName + "[" + sColumnName + " Difference]";
                    }
                    else
                        cell.ToolTip = "User: " + sUserName + "[" + sColumnName + "]";

                    // Hide un-necessary column.
                    if (S_COLUMN_INDICES.Contains(iCellndex.ToString()) || iCellndex == oCells.Count - 4)
                        cell.Visible = false;
                    else
                    {
                        cell.Wrap = false;

                        cell.Style.Add("padding-left", "5");
                        cell.Style.Add("padding-right", "5");

                        if (iCellndex == I_NAME_COLUMN_INDEX)
                        {
                            Label lblName = new Label();
                            lblName.Text = Server.HtmlDecode(cell.Text).Replace("''", "'");
                            cell.Controls.Add(lblName);
                            cell.HorizontalAlign = HorizontalAlign.Left;
                        }
                        else if (iCellndex == I_DESIGNATION_COLUMN_INDEX || iCellndex == miLateMarkLeaveIndex)
                            cell.HorizontalAlign = HorizontalAlign.Left;
                        else
                        {
                            // Set forecolor red if value of differenec is less than zero.
                            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(cell.Text).Trim()) && Convert.ToDecimal(cell.Text) < 0 && miDifferenceRow != 0 && miDifferenceRow % 3 == 0)
                                cell.ForeColor = Color.Red;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                        }
                    }

                    if (hidColumnIndexes.Value.Contains("[" + iCellndex + "]"))
                    {
                        if (miDifferenceRow != 0 && miDifferenceRow % 3 == 0)
                        {
                            cell.Font.Bold = true;
                            cell.BackColor = Color.SkyBlue;

                            if (e.Row.Cells[I_NAME_COLUMN_INDEX].Text != "Total Total" && iCellndex != oCells.Count - 3)
                                cell.ForeColor = Color.Navy;

                            if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(cell.Text).Trim()) && Convert.ToDecimal(cell.Text) < 0)
                                cell.ForeColor = Color.Red;

                            new Button[] { e.Row.Cells[I_SAVE_BUTTON_INDEX].Controls[1] as Button, e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1] as Button }.ApplyEffect();
                            ((Button)e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1]).Attributes.Add("onclick", "if(!confirm('Are you sure you want to delete salary difference of this user?')) return false;");
                        }
                        else
                        {
                            e.Row.Cells[I_SAVE_BUTTON_INDEX].Controls[1].Visible = false;
                            e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;
                            if (iCellndex == oCells.Count - 3)
                            {
                                cell.BackColor = Color.LightSteelBlue;
                            }
                            else if (e.Row.Cells[I_NAME_COLUMN_INDEX].Text != "Total Total")
                            {
                                string str = grdSalaryDifference.HeaderRow.Cells[iCellndex].Text;
                                cell.BackColor = Color.LightGray;
                                cell.ForeColor = Color.Navy;
                            }
                        }
                    }

                    if (miNetSalaryColumnIndex != 0 && (miSavedSalaryDifferenceColumnIndex == iCellndex || miPaidSalaryDifferenceColumnIndex == iCellndex))
                    {
                        if (Server.HtmlDecode(cell.Text).Trim() != string.Empty && Convert.ToInt32(cell.Text) != 0 && e.Row.RowIndex != ((DataTable)grdSalaryDifference.DataSource).Rows.Count - 1)
                        {
                            HyperLink oHyperLink = new HyperLink();
                            oHyperLink.Style.Add("CssClass", "clsLabel class1");
                            oHyperLink.Text = cell.Text;
                            oHyperLink.NavigateUrl = "#";

                            string sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                                                  "&Year=" + cmbYear.SelectedValue +
                                                  "&UserId=" + grdSalaryDifference.DataKeys[e.Row.RowIndex]["UserId"].ToString();
                            sQueryString = sQueryString + "&ShowPaid=" + (miSavedSalaryDifferenceColumnIndex == iCellndex ? Constants.S_ZERO : Constants.S_ONE);

                            sQueryString = sQueryString + "&BaseMonthId=" + cmbMonthToCompare.SelectedValue +
                                          "&BaseYear=" + cmbYearToCompare.SelectedValue +
                                          "&Filter=" + txtSearch.Text.Trim();

                            oHyperLink.Attributes.Add("onclick", "openDetailsPopup(this,'" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
                            cell.Controls.Add(oHyperLink);
                        }
                    }
                }
                iCellndex++;
            }

            // Set color to difference row.
            if (miDifferenceRow != 0 && miDifferenceRow % 3 == 0)
            {
                e.Row.BackColor = Color.SkyBlue;
                e.Row.Font.Bold = true;
                miDifferenceRow = 0;
                miOldConfigRow = 0;
                miSrNo++;
                if (e.Row.Cells[I_SAVE_BUTTON_INDEX].Controls.Count > 1 && e.Row.Cells[I_SAVE_BUTTON_INDEX].Controls[1].Visible)
                    miTotalSalaryDifferenceRows++;
            }
            else if (miOldConfigRow != 0 && miOldConfigRow % 2 == 0)
                e.Row.BackColor = Color.Wheat; // Set color for current configuration.

            miOldConfigRow++;
            miDifferenceRow++;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save salary difference.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Sent zero user id to save all the records at a time.
            SaveSalaryDifference(0);
            FillSalaryDifferenceGrid();
        }
        catch (NoRecordFoundException ex)
        {
            lblNoRecordMessage.Text = ex.Message;
            SetView(false);
            btnDelete.Enabled = false;
            SetPagerDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export salary difference details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ShowSalaryDifference(false);

            DataTable oDTTemp = null;
            DataTable oDTSalaryDifference = null;
            if (Session[S_SALARY_DIFFERENCE_TABLE] != null)
            {
                oDTSalaryDifference = ((DataTable)Session[S_SALARY_DIFFERENCE_TABLE]);
                oDTTemp = oDTSalaryDifference.Clone();

                foreach (DataRow oDataRow in oDTSalaryDifference.Rows)
                    oDTTemp.ImportRow(oDataRow);
            }

            DataTable ODataTable = moSalaryDifferenceBL.GetSalaryDifferenceToExport(oDTTemp, hidMonthList.Value);
            ExportToExcel("SalaryDifference.xls", ODataTable);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete saved salary difference of selected month.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
            oSalaryDifferenceBL.StaffBaseDetails = new StaffBaseDetails
            {
                MonthId = Convert.ToInt32(hidSelectedMonth.Value),
                Year = Convert.ToInt32(hidSelectedYear.Value)
            };

            oSalaryDifferenceBL.Delete();

            lblMessage.Text = S_OPERATION_MESSAGE.Replace(S_OPERATION, Constants.S_DELETED.ToLower());
            lblMessage.Visible = true;
            btnDelete.Enabled = false;
            hidIsReadyToPay.Value = Constants.S_NO;

            FillSalaryDifferenceGrid();
        }
        catch (NoRecordFoundException ex)
        {
            lblNoRecordMessage.Text = ex.Message;
            SetView(false);
            btnDelete.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save / delete salary difference of individual user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDifference_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = Convert.ToInt32(e.CommandArgument);
            int iUserId = Convert.ToInt32(grdSalaryDifference.DataKeys[iRowIndex][S_USER_ID]);

            if (e.CommandName == S_SAVE_COMMAND)
            {
                SaveSalaryDifference(iUserId);
                FillSalaryDifferenceGrid();
            }
            else if (e.CommandName == S_DELETE_COMMAND)
            {
                DeleteSalaryDifference(iUserId);
                lblMessage.Text = S_OPERATION_MESSAGE.Replace(S_OPERATION, Constants.S_DELETED.ToLower());
                lblMessage.Visible = true;
                FillSalaryDifferenceGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set link state according to selected month and year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbMonthToCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisableFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to save salary difference of all staff.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        try
        {
            ShowSalaryDifference(false);

            List<string> lstOtherColumns = new List<string> { "IsSalaryDifferenceRow", "Saved Difference", "Net Difference", "Paid Difference", "Sr No", "UserId", "OriginalStaffGroupsId", "StaffGroupId", "Name", "Designation", "Total" };

            if (Session[S_SALARY_DIFFERENCE_TABLE] != null)
            {
                List<string> lstColumns = new List<string>();
                DataTable oDTSalaryDifference = ((DataTable)Session[S_SALARY_DIFFERENCE_TABLE]);
                foreach (DataColumn column in oDTSalaryDifference.Columns)
                    lstColumns.Add(column.Caption);

                StringBuilder obj = new StringBuilder();
                List<EarningsDeductions> lstEarningDeductions = Session[S_EARNING_DEDUCTIONS] as List<EarningsDeductions>;

                IEnumerable<DataRow> drRows = (from sd in oDTSalaryDifference.AsEnumerable()
                                               where sd.Field<string>("IsSalaryDifferenceRow") == Constants.S_ONE
                                               select sd).ToList();
                string sQuote = "&quot;";
                if (drRows.Count() > 0)
                {
                    string sType = string.Empty;
                    foreach (DataRow dr in drRows)
                    {
                        int iUserId = dr["UserId"].ToInt();
                        if (dr["Net Salary"].ToInt() > 0)
                        {
                            obj.Append("<SalaryDifference><SalaryDifference UserId=\"" + iUserId + "\" NetSalary=\"" + dr["Net Salary"].ToInt() + "\" SalaryDifferenceXml=\"&lt;User&gt;");
                            foreach (string sColumnName in lstColumns)
                            {
                                switch (sColumnName)
                                {
                                    case "Total Deduction": sType = "TD"; break;
                                    case "Gross Salary": sType = "GS"; break;
                                    case "Net Salary": sType = "NS"; break;
                                    default: sType = "ED"; break;
                                }

                                if (!lstOtherColumns.Contains(sColumnName))
                                {
                                    int iEarnDeuctId = 0;
                                    if (lstEarningDeductions.Any(ed => ed.ShortName == sColumnName))
                                        iEarnDeuctId = lstEarningDeductions.Where(ed => ed.ShortName == sColumnName).FirstOrDefault().EarningsDeductionsId;
                                    obj.Append("&lt;UsersSalaryDifference Id=" + sQuote + iEarnDeuctId + sQuote + " Type=" + sQuote + sType + sQuote + " Value=" + sQuote + dr[sColumnName].ToString() + sQuote + " /&gt;");
                                }
                            }
                            obj.Append("&lt;/User&gt;\"");
                            obj.Append(" /></SalaryDifference>");
                        }
                    }
                }

                if (obj.Length > 0)
                {
                    SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
                    oSalaryDifferenceBL.StaffBaseDetails = PopulateStaffBaseDeails();
                    oSalaryDifferenceBL.SaveAll(obj.ToString());
                    FillSalaryDifferenceGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to disable fields if month or year is changed.
    /// </summary>
    private void DisableFields()
    {
        int iSelectedMonth = Convert.ToInt32(hidSelectedMonth.Value);
        int iSelectedYear = Convert.ToInt32(hidSelectedYear.Value);
        int iCurrentMonth = Convert.ToInt32(cmbMonths.SelectedValue);
        int iCurrentYear = Convert.ToInt32(cmbYear.SelectedValue);

        int iSelectedBaseMonth = Convert.ToInt32(hidSelectedBaseMonth.Value);
        int iSelectedBaseYear = Convert.ToInt32(hidSelectedBaseYear.Value);
        int iCurrentBaseMonth = Convert.ToInt32(cmbMonthToCompare.SelectedValue);
        int iCurrentBaseYear = Convert.ToInt32(cmbYearToCompare.SelectedValue);

        char cIsReadyToPay = Convert.ToChar(hidIsReadyToPay.Value);

        bool bEnableFields = iSelectedMonth == iCurrentMonth && iSelectedYear == iCurrentYear && iSelectedBaseMonth == iCurrentBaseMonth && iSelectedBaseYear == iCurrentBaseYear;

        btnSave.Enabled = bEnableFields;
        btnSaveAll.Enabled = bEnableFields;

        if (cIsReadyToPay == 'Y')
        {
            btnDelete.Enabled = bEnableFields;
        }
        btnExport.Enabled = bEnableFields;

        tblLinks.Visible = bEnableFields;
    }

    /// <summary>
    /// This method is used to set pager details.
    /// </summary>
    private void SetPagerDetails()
    {
        if (moSalaryDifferenceBL.TotalRecords > I_GRID_PAGE_COUNT)
        {
            int iLastIndex = Convert.ToInt32(PageDropDownList.SelectedValue) * I_GRID_PAGE_COUNT;
            tblPageDetails.Visible = true;
            lblStartIndex.Text = ((Convert.ToInt32(PageDropDownList.SelectedValue) - 1) * I_GRID_PAGE_COUNT + 1).ToString();
            lblEndIndex.Text = iLastIndex < moSalaryDifferenceBL.TotalRecords ? iLastIndex.ToString() : moSalaryDifferenceBL.TotalRecords.ToString();
            lblTotalRecords.Text = moSalaryDifferenceBL.TotalRecords.ToString();
        }
        else
            tblPageDetails.Visible = false;
    }

    /// <summary>
    /// This method is used to save salary difference.
    /// </summary>
    /// <param name="aiUserid"></param>
    private void SaveSalaryDifference(int aiUserid)
    {
        DataTable oDTSalaryDifference = GetSalaryDifferenceForSave();
        SalaryDifferenceBL oSalaryDifferenceBL = PopulateSalaryDifferenceBL();
        oSalaryDifferenceBL.Save(aiUserid, oDTSalaryDifference);
        lblMessage.Text = S_OPERATION_MESSAGE.Replace(S_OPERATION, "Saved");
        lblMessage.Visible = true;
        btnDelete.Enabled = true;
        hidIsReadyToPay.Value = Constants.S_YES;
    }

    /// <summary>
    /// This method is used to populate salary difference details.
    /// </summary>
    /// <returns></returns>
    private SalaryDifferenceBL PopulateSalaryDifferenceBL()
    {
        List<SalaryDifferenceClass> olstSalaryDifferenceClassList;
        List<EarningsDeductions> olstEarningsDeductions;

        olstSalaryDifferenceClassList = new List<SalaryDifferenceClass>();
        if (Session[S_COLUMN_DETAILS] != null)
            olstSalaryDifferenceClassList = Session[S_COLUMN_DETAILS] as List<SalaryDifferenceClass>;

        olstEarningsDeductions = new List<EarningsDeductions>();
        if (Session[S_EARNING_DEDUCTIONS] != null)
            olstEarningsDeductions = Session[S_EARNING_DEDUCTIONS] as List<EarningsDeductions>;

        SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
        oSalaryDifferenceBL.SalaryDifferenceClassList = olstSalaryDifferenceClassList;
        oSalaryDifferenceBL.EarningsDeductionsToSave = olstEarningsDeductions;
        oSalaryDifferenceBL.StaffBaseDetails = PopulateStaffBaseDeails();

        return oSalaryDifferenceBL;
    }

    /// <summary>
    /// This method is used to return salary difference from viewstate for saving.
    /// </summary>
    /// <param name="aolstSalaryDifferenceClassList"></param>
    /// <returns></returns>
    private DataTable GetSalaryDifferenceForSave()
    {
        DataTable aoDTSalaryDifference = null;
        if (Session[S_SALARY_DIFFERENCE_TABLE] != null)
            aoDTSalaryDifference = (DataTable)Session[S_SALARY_DIFFERENCE_TABLE];
        return aoDTSalaryDifference;
    }

    /// <summary>
    /// This method is used to show salary difference.
    /// </summary>
    private void ShowSalaryDifference(bool abFillGrid)
    {
        int iMonthId = Convert.ToInt32(cmbMonths.SelectedValue);
        int iYear = Convert.ToInt32(cmbYear.SelectedValue);
        int iMonthIdToCompare = Convert.ToInt32(cmbMonthToCompare.SelectedValue);
        int iYearToCompare = Convert.ToInt32(cmbYearToCompare.SelectedValue);
        int iPageIndex = grdSalaryDifference.PageIndex;
        int iPageSize = I_PAGE_SIZE / 3;
        string sFilter = txtSearch.Text.Trim();

        DataTable oDTSalaryDifference = null;

        hidRowCount.Value = Constants.S_ZERO;
        hidColumnIndexes.Value = string.Empty;

        if (hidSelectedPageIndex.Value != Constants.S_ZERO)
            iPageIndex = Convert.ToInt32(hidSelectedPageIndex.Value);

        if (!abFillGrid)
        {
            iPageIndex = 0;
            iPageSize = 9999;
            sFilter = string.Empty;
        }

        moSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
        DataSet oDsSalaryDifference = moSalaryDifferenceBL.GetSalaryDifferenceDataset(iMonthId, iYear, sFilter, iPageIndex, iPageSize, iMonthIdToCompare, iYearToCompare);
        //int iTotalRecords = oDsSalaryDifference.Tables[2].Rows.Count;
        DataTable oDTSalaryStatus = oDsSalaryDifference.Tables[I_SALARY_PAID_STATUS_TABLE_INDEX];
        Session[S_COLUMN_DETAILS] = moSalaryDifferenceBL.SalaryDifferenceClassList;
        Session[S_EARNING_DEDUCTIONS] = moSalaryDifferenceBL.EarningsDeductions;

        if (oDTSalaryStatus.IsNonEmpty() && oDTSalaryStatus.Rows[0][0].ToString() == Constants.S_YES)
        {
            btnDelete.Enabled = moSalaryDifferenceBL.IsReadyToPaySalary;
            hidIsReadyToPay.Value = moSalaryDifferenceBL.IsReadyToPaySalary ? Constants.S_YES : Constants.S_NO;
            SetPaidSalaryDifferenceLink(oDTSalaryStatus);
            moDTPaidSalary = moSalaryDifferenceBL.GetPaidSalaryDetails(oDsSalaryDifference, out mlstColumnsNames, out moDTCurrentSalary);
            DataTable oDTSalaryDetails = moSalaryDifferenceBL.GetMergedTable(moDTPaidSalary, moDTCurrentSalary);
            oDTSalaryDifference = moSalaryDifferenceBL.CalculateSalaryDifference(oDTSalaryDetails, Convert.ToInt32(cmbYear.SelectedValue), Convert.ToInt32(cmbMonths.SelectedValue), Convert.ToInt32(cmbYearToCompare.SelectedValue), Convert.ToInt32(cmbMonthToCompare.SelectedValue));
            hidSelectedMonth.Value = cmbMonths.SelectedValue;
            hidSelectedYear.Value = cmbYear.SelectedValue;
            hidSelectedBaseMonth.Value = cmbMonthToCompare.SelectedValue;
            hidSelectedBaseYear.Value = cmbYearToCompare.SelectedValue;
        }

        if (oDTSalaryDifference == null || oDTSalaryDifference.Rows.Count == 0)
        {
            SetView(false);
            btnDelete.Enabled = false;
            grdSalaryDifference.DataSource = null;
            grdSalaryDifference.DataBind();
        }
        else
        {
            SetView(true);
            hidRowCount.Value = grdSalaryDifference.Rows.Count.ToString();
            oDTSalaryDifference.Columns.Add(PayrollConstants.S_SAVED_DIFFERENCE);
            oDTSalaryDifference.Columns.Add(PayrollConstants.S_NET_DIFFERENCE);
            oDTSalaryDifference.Columns.Add(PayrollConstants.S_PAID_DIFFERENCE);

            Session[S_SALARY_DIFFERENCE_TABLE] = oDTSalaryDifference;

            moSalaryDifferenceBL.CalculateNetSalaryDifference(oDTSalaryDifference);

            moSalaryDifferenceBL.CalculateSummaryOfDifference(oDTSalaryDifference, mlstColumnsNames);
            moSalaryDifferenceBL.RemoveSupportingColumns(oDTSalaryDifference);

            if (abFillGrid)
            {
                grdSalaryDifference.DataSource = oDTSalaryDifference;
                grdSalaryDifference.DataBind();
                FillPagerDropdown();
            }
        }
    }

    /// <summary>
    /// This method is used to fill pager dropdownlist.
    /// </summary>
    private void FillPagerDropdown()
    {
        int iPageNumber = 0;
        PageDropDownList.Items.Clear();

        for (int iPageIndex = 0; iPageIndex < moSalaryDifferenceBL.TotalPages; iPageIndex++)
        {
            iPageNumber = iPageIndex + 1;

            ListItem item = new ListItem(iPageNumber.ToString());
            if (iPageIndex == grdSalaryDifference.PageIndex)
                item.Selected = true;

            PageDropDownList.Items.Add(item);
        }

        PageDropDownList.SelectedIndex = Convert.ToInt32(hidSelectedPageIndex.Value);
        lblCurrentPage.Text = "Page " + PageDropDownList.SelectedValue + "  of " + moSalaryDifferenceBL.TotalPages;

        tblPager.Visible = true;
        if (moSalaryDifferenceBL.TotalPages == 1)
            tblPager.Visible = false;
    }

    /// <summary>
    /// This method is used to fill salary difference grid.
    /// </summary>
    private void FillSalaryDifferenceGrid()
    {
        miTotalSalaryDifferenceRows = 0;
        ShowSalaryDifference(true);
        if (grdSalaryDifference.Rows.Count > 0)
        {
            trLegend.Visible = true;
            trGrid.Visible = true;
            tblNote.Visible = true;
            SetStyleForSummaryRow();
        }

        hidSalaryDifferenceCount.Value = miTotalSalaryDifferenceRows.ToString();
        btnSave.Enabled = miTotalSalaryDifferenceRows > 0;
        btnSaveAll.Enabled = miTotalSalaryDifferenceRows > 0;
    }

    /// <summary>
    /// This method is used to set style for summary row.
    /// </summary>
    private void SetStyleForSummaryRow()
    {
        TableCellCollection oCells = grdSalaryDifference.Rows[grdSalaryDifference.Rows.Count - 1].Cells;
        foreach (TableCell cell in oCells)
        {
            cell.BackColor = Color.LightGray;
            cell.Font.Bold = true;
            cell.ForeColor = Color.Navy;
            cell.Font.Size = 10;
        }
    }

    /// <summary>
    /// This method is used to set delete button state.
    /// </summary>
    /// <param name="e"></param>
    private void SetDeleteButtonState(GridViewRowEventArgs e)
    {
        int iUserId = Convert.ToInt32(grdSalaryDifference.DataKeys[e.Row.RowIndex][S_USER_ID]);
        int iItemcount = moSalaryDifferenceBL.StaffBaseDetailsList.Where(user => user.UserId == iUserId).Count();

        if (moSalaryDifferenceBL.IsReadyToPaySalary)
        {
            if (miDifferenceRow != 0 && miDifferenceRow % 3 == 0 && iItemcount > 0)
                e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = true;

            if (iItemcount < 1)
                e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;
        }
        else
            e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetDefaultProperties()
    {
        SetDefaultButton(btnShow);
        ClearSessionData();
    }

    /// <summary>
    /// This method is used to clear session data.
    /// </summary>
    private void ClearSessionData()
    {
        if (Session[S_SALARY_DIFFERENCE_TABLE] != null)
            Session.Remove(S_SALARY_DIFFERENCE_TABLE);

        if (Session[S_COLUMN_DETAILS] != null)
            Session.Remove(S_COLUMN_DETAILS);

        if (Session[S_EARNING_DEDUCTIONS] != null)
            Session.Remove(S_EARNING_DEDUCTIONS);
    }

    /// <summary>
    /// This method is used to set delete button state.
    /// </summary>
    private void SetDeleteButtonState(List<StaffBaseDetails> aoStaffBaseDetailsList)
    {
        if (aoStaffBaseDetailsList != null)
        {
            int iUserId;
            int iItemcount;
            int iDifferenceRow = 1;
            foreach (GridViewRow oGridViewRow in grdSalaryDifference.Rows)
            {
                iUserId = Convert.ToInt32(grdSalaryDifference.DataKeys[oGridViewRow.RowIndex][S_USER_ID]);
                iItemcount = aoStaffBaseDetailsList.Where(user => user.UserId == iUserId).Count();

                if (aoStaffBaseDetailsList.Count > 0)
                {
                    if (iDifferenceRow != 0 && iDifferenceRow % 3 == 0)
                        oGridViewRow.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = true;

                    if (iItemcount < 1)
                        oGridViewRow.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;
                }
                else
                    oGridViewRow.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;

                iDifferenceRow++;
            }
        }
    }

    /// <summary>
    /// This method is used to set query string.
    /// </summary>
    private void SetQueryString()
    {
        string sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                             "&Year=" + cmbYear.SelectedValue +
                             "&MonthName=" + lnkmonthList.Text;

        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }

    /// <summary>
    /// This method is used to set paid salary difference link.
    /// </summary>
    /// <param name="asMonthList"></param>
    private void SetPaidSalaryDifferenceLink(DataTable aoDTSalaryStatus)
    {
        string sMonthList = aoDTSalaryStatus.Rows[0]["MonthList"].ToString();
        string sCurrentSalaryMonth = aoDTSalaryStatus.Rows[0]["CurrentSalaryMonth"].ToString();

        tdMonthListMessage.Visible = false;
        if (!string.IsNullOrEmpty(sMonthList))
        {
            lblMonthList.Text = S_PAID_SALARY_MONTH_MESSAGE;
            lnkmonthList.Text = sMonthList;
            hidMonthList.Value = sMonthList;
            tdMonthListMessage.Visible = true;
        }

        lblCurrentSalaryMonth.Text = string.Empty;
        if (!string.IsNullOrEmpty(sCurrentSalaryMonth))
            lblCurrentSalaryMonth.Text = S_SALARY_PAID_MESSAGE + sCurrentSalaryMonth;

        SetQueryString();
    }

    /// <summary>
    /// This method is used to set empty grid message.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetView(bool abAction)
    {
        lblNoRecordMessage.Visible = !abAction;
        divContainer.Visible = abAction;
        tblNote.Visible = abAction;
        trLegend.Visible = abAction;
        btnExport.Enabled = abAction;
        btnSave.Enabled = abAction;
        btnSaveAll.Enabled = abAction;
        tblLinks.Visible = abAction;
    }

    /// <summary>
    /// This method is used to format net salary column.
    /// </summary>
    /// <param name="e"></param>
    private void FormatNetSalaryColumn(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int iNetSalaryColumnIndex = e.Row.Cells.Count - 2;
            if (e.Row.Cells[iNetSalaryColumnIndex].Text != Constants.S_ZERO)
                e.Row.Cells[iNetSalaryColumnIndex].ForeColor = Color.Maroon;
            else
                e.Row.Cells[I_SAVE_BUTTON_INDEX].Controls[1].Visible = false;

            if (e.Row.Cells[iNetSalaryColumnIndex - 1].Text == Constants.S_ZERO)
                e.Row.Cells[I_DELETE_BUTTON_INDEX].Controls[1].Visible = false;
        }
    }

    /// <summary>
    /// This method is used to fill month and year comboboxes.
    /// </summary>
    private void FillComboboxes()
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);

        ListSource.FillDropDownList(oSalaryDetailsBL.Months, cmbMonths, "Month", "MonthId", string.Empty);
        ListSource.FillDropDownList(oSalaryDetailsBL.Years, cmbYear, "Year", "Year", string.Empty);
        ListSource.FillDropDownList(oSalaryDetailsBL.Months, cmbMonthToCompare, "Month", "MonthId", string.Empty);
        ListSource.FillDropDownList(oSalaryDetailsBL.Years, cmbYearToCompare, "Year", "Year", string.Empty);

        if (Request.QueryString.ToString() == string.Empty)
            SetMonthAndYear(oSalaryDetailsBL);
        else
        {
            cmbMonths.SelectedValue = QueryString["MonthId"];
            cmbYear.SelectedValue = QueryString["Year"];
            cmbMonthToCompare.SelectedValue = QueryString["BaseMonthId"];
            cmbYearToCompare.SelectedValue = QueryString["BaseYearId"];
            txtSearch.Text = QueryString["Filter"];

            //ShowSalaryDifference(true);

            btnShow_Click(btnShow, new EventArgs());
        }
    }

    /// <summary>
    /// This method is used to set month and year.
    /// </summary>
    /// <param name="aoSalaryDetailsBL"></param>
    private void SetMonthAndYear(SalaryDetailsBL aoSalaryDetailsBL)
    {
        int iMonthId = aoSalaryDetailsBL.clsMonthAndYear.MonthId;
        int iYear = aoSalaryDetailsBL.clsMonthAndYear.Year;

        if (aoSalaryDetailsBL.clsMonthAndYear != null)
        {
            if (iMonthId != 0 && iYear != 0)
            {
                cmbMonths.Items.FindByValue(iMonthId.ToString()).Selected = true;
                cmbYear.Items.FindByValue(iYear.ToString()).Selected = true;

            }
        }

        iMonthId += 1;
        if (iMonthId == 13)
        {
            iMonthId = 1;
            iYear += 1;
        }

        cmbMonthToCompare.SelectedValue = iMonthId.ToString();
        cmbYearToCompare.SelectedValue = iYear.ToString();
    }

    /// <summary>
    /// This method is used to set screen width.
    /// </summary>
    private void SetControlWidth()
    {
        const string S_WIDTH = "width";
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str);
            iWidth = iWidth / 100 * 80;
            divContainer.Style.Add(S_WIDTH, iWidth.ToString() + "px !important");
            lblNoRecordMessage.Width = Unit.Pixel(iWidth);
            tblLegent.Style.Add(S_WIDTH, iWidth.ToString() + "px !important");
            tblNote.Style.Add(S_WIDTH, iWidth.ToString() + "px !important");
        }
        else
        {
            divContainer.Style.Add(S_WIDTH, Convert.ToString(1024) + "px !important");
            lblNoRecordMessage.Width = Unit.Pixel(1024);
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnShow, btnExport, btnSave, btnSaveAll, btnDelete });
        lnkmonthList.Attributes["onclick"] = "OpenPaidSalaryDifferencePopup(); return false;";
        btnDelete.Attributes.Add("onclick", "if(!confirm('Are you sure you want to delete salary difference of this month?')) return false;");

        cmbMonths.Attributes.Add("onchange", "HideButtons()");
        cmbYear.Attributes.Add("onchange", "HideButtons()");

        SetConfigQueryString();

        grdSalaryDifference.PageSize = I_PAGE_SIZE;
    }

    private void SetConfigQueryString()
    {
        string sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                              "&Year=" + cmbYear.SelectedValue +
                              "&BaseMonthId=" + cmbMonthToCompare.SelectedValue +
                              "&BaseYear=" + cmbYearToCompare.SelectedValue +
                              "&Filter=" + txtSearch.Text.Trim();

        lnkConfig.Attributes.Add("onclick", "openPopup(this,'" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");

        sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                              "&Year=" + cmbYear.SelectedValue +
                              "&ShowPaid=0" +
                              "&BaseMonthId=" + cmbMonthToCompare.SelectedValue +
                              "&BaseYear=" + cmbYearToCompare.SelectedValue +
                              "&Filter=" + txtSearch.Text.Trim();
        lnkDetails.Attributes.Add("onclick", "openDetailsPopup(this,'" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
    }

    /// <summary>
    /// This method is used to populate salary difference BL.
    /// </summary>
    /// <returns></returns>
    private StaffBaseDetails PopulateStaffBaseDeails()
    {
        StaffBaseDetails oStaffBaseDetails = new StaffBaseDetails
        {
            MonthId = Convert.ToInt32(cmbMonths.SelectedValue),
            Year = Convert.ToInt32(cmbYear.SelectedValue),
            InsertedById = miUserId
        };
        return oStaffBaseDetails;
    }

    /// <summary>
    /// This method is used to delete salary difference of selected user.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void DeleteSalaryDifference(int aiUserId)
    {
        SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, 0, miUserId);
        oSalaryDifferenceBL.StaffBaseDetails = new StaffBaseDetails
        {
            MonthId = Convert.ToInt32(cmbMonths.SelectedValue),
            Year = Convert.ToInt32(cmbYear.SelectedValue),
            UserId = aiUserId,
            InsertedById = miUserId
        };

        oSalaryDifferenceBL.Delete();
    }

    #endregion
}