/* File Name = SalaryDetailsUI.aspx.cs
 * Created Date - 
 * Modified Date  -24 Dec 2010
 * Created by - Sachin
 * Class Description - This class is defined to manage salary details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Linq;
using System.Text;

public partial class SalaryDetailsUI : ExportDataTable
{
    #region Constants

    const int I_SALARY_DETAILS = 0;
    const int I_UNPUBLSH_STATUS = 1;

    const int I_DISPLAY_CONTROLS_COLUMN_INDEX = 2;
    const int I_LEAVE_DEDUCTED_LENGTH = 2;
    const int I_EARNINGS_DEDUCTIONS_LENGTH = 4;
    const int I_NAME_COLUMN_INDEX = 8;
    const int I_DESIGNATION_COLUMN_INDEX = 9;

    const int I_START_CELL_INDEX = 7;
    
    const string S_ATTENDANCE = "AT";
    const string S_LEAVE = "LV";
    const string S_EARNINGS_DEDUCTIONS = "ED";

    const string S_SAVE_MESSAGE = "Salary details has been saved successfully !!!";
    const string S_UNPUBLISH_MESSAGE = "Salary details has been unpublished successfully !!!";
    const string S_ERROR_MESSAGE = "Failed to save salary details.";
    const string S_NEGATIVE_LEAVE_BALANCE_MESSAGE = "Leave balance should not be negative (marked in red), please re-configure yearwise leaves of respective user.";

    const string S_EMPTY_TABLE = "TemporaryTable";
    const string S_GROSS_SALARY_DIFFERENCE = "Gross Salary Difference";
    const string S_SALARY_DIFFERENCE = "Salary Difference";
    const string S_SALARY_DIFFERENCE_PF = "Salary Difference of Deduction";

    const string S_COMMAND_SAVE = "SAVE";
    const string S_BUTTON_BACK = "Back";

    string msColumnNumbers = "0,2,3,4,5,6,7,";

    const string S_SALARY_ENTITY_LIST = "SchoolEntityList";
    const string S_FORM_NO_16 = "Income Tax Details";

    const int I_CACHE_TIMEOUT = 1200;
    const string S_ISDELETED = "IsDeleted";
    const string S_INCOME_TAX = "I.T.";
    
    #endregion

    #region Data Members

    int miTotalPages = 0;
    int miTotalRecords = 0;    
    string msMonthList = string.Empty;
    bool mbReloadRequired;
    bool mbDisplaySaveButton;
    bool mbShowConfigMessage;    
    bool mbIsPageInit = true;
    bool mbIsExportAll;
    List<string> olstHeaderNames = new List<string>();
    DataTable moTempDataTable;
    SalaryDetailsBL oSalaryDetails;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill salary details listview.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            string sIsSaveClick = Convert.ToString(Request.Params[hidIsSaveButtonClick.ClientID.Replace("_", "$")]);
            mbIsPageInit = false;            
            if (sIsSaveClick == Constants.S_YES)
            {
                mbIsPageInit = true;
                LoadSalaryDetails(false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill staff groups,months and years comboboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (CheckPreCondition())
            {
                grdSalaryDetails.PageSize = Constants.I_GRID_PAGE_COUNT;
                hidIsSaveButtonClick.Value = Constants.S_NO;
                hidSelectedPageIndex.Value = Constants.S_ZERO;
                SetScreenWidth();
                if (!IsPostBack)
                {
                    FillComboboxes();
                    SetJavascriptAttributes();
					FillRetirementNoticeDetails();
                    mbIsPageInit = false;
                    ReadQuerystring();
                    LoadSalaryDetails(true);
                    DisplayLeavesConfigMsg();
                    SetDefaultProperties();
                }
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to format cells of grid row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iColumnIndex = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                HideCells(e.Row);
                TableCellCollection cells = e.Row.Cells;
                foreach (TableCell cell in cells)
                {
                    if (!msColumnNumbers.Contains(iColumnIndex + ","))
                        moTempDataTable.Columns.Add(cell.Text.Replace("_", " "));
                    cell.Text = cell.Text.Replace("_", " ");

                    cell.Style.Add("padding-left", "5");
                    cell.Style.Add("padding-right", "5");

                    cell.Wrap = false;
                    olstHeaderNames.Add(cell.Text);
                    switch (iColumnIndex)
                    {
                        case 0: cell.HorizontalAlign = HorizontalAlign.Center; break;
                        case I_NAME_COLUMN_INDEX:
                        case I_DESIGNATION_COLUMN_INDEX: cell.HorizontalAlign = HorizontalAlign.Left; break;
                        default: cell.HorizontalAlign = HorizontalAlign.Right; break;

                    }

                    cell.CssClass = "GridDate";
                    if (!mbIsPageInit && (cell.Text == PayrollConstants.S_TOTAL || cell.Text == PayrollConstants.S_GROSS_SALARY || cell.Text == PayrollConstants.S_TOTAL_DEDUCTION || cell.Text == PayrollConstants.S_NET_SALARY))
                        hidColumnIndexes.Value = hidColumnIndexes.Value + "[" + iColumnIndex + "]";

                    if (cell.Text == S_GROSS_SALARY_DIFFERENCE || cell.Text == S_SALARY_DIFFERENCE_PF || cell.Text == S_SALARY_DIFFERENCE)
                        hidSalaryDiffColumnIndex.Value = hidSalaryDiffColumnIndex.Value == string.Empty ? iColumnIndex.ToString() :  hidSalaryDiffColumnIndex.Value + "," + iColumnIndex.ToString();
                    else if (cell.Text == S_ISDELETED)
                        cell.Visible = false;

                    if (cell.Text == S_INCOME_TAX && hidIsStaticOutput.Value != Constants.S_YES)
                    {
                        // This condition is used to add a link after income tax textbox for claculating and displaying it in textbox.      
                        Label oLabel = new System.Web.UI.WebControls.Label { Text = cell.Text +"&nbsp;" };
               
                        LinkButton oLinkButton = new LinkButton { Text = "Calc", ID = "lnkCalculateAll" };
                        oLinkButton.Attributes.Add("onclick", "CalculateAmountForAll(); return false;");
                        cell.Controls.Add(oLabel);
                        cell.Controls.Add(oLinkButton);                        
                    }

                    iColumnIndex++;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
                FormatGridCells(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save individuals salary.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {   
            if (e.CommandName == S_COMMAND_SAVE)
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL { SalaryDetails = PopulateIndividualSalaryDetailsBL(iRowIndex) };
                oSalaryDetailsBL.InsertIndividualDetails();
                SetFields();
                trSuccessMessage.Visible = true;
            }
        }
        catch (SqlException ex)
        {
            lblErr.Text = ex.Message;
            trSuccessMessage.Visible = true;
        }
        catch (Exception ex)
        {
            trSuccessMessage.Visible = true;
            lblErr.Text = S_ERROR_MESSAGE;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to unpublish published salary of selected month and year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnpublish_Click(object sender, EventArgs e)
    {
        try
        {   
            int iMonthId = Convert.ToInt32(cmbMonths.SelectedValue);
            int iYear = Convert.ToInt32(cmbYear.SelectedValue);
            int iLeaveTransferMonth = Settings.LeaveTransferMonth;
            SalaryDetailsBL.Unpublish(miSchoolId, miAcademicYearId, iMonthId, iYear, miUserId, iLeaveTransferMonth);

            lblMessage.Text = S_UNPUBLISH_MESSAGE;
            trSalaryMessage.Visible = false;

            mbIsPageInit = false;
            LoadSalaryDetails(true);
            DisplayLeavesConfigMsg();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save salary details of all the users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL { SalaryDetails = PopulateSalaryDetails() };
            oSalaryDetailsBL.Save();
            SetFields();
            trSuccessMessage.Visible = true;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to display salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            hidMinRecordsStaffGroupId.Value = cmbStaffGroups.SelectedValue;

            if (PageDropDownList.Items.Count > 0)
                PageDropDownList.SelectedIndex = 0;

            hidSelectedPageIndex.Value = Constants.S_ZERO;
            
            hidSalaryFilter.Value = txtSearch.Text.Trim();
            hidSalaryYear.Value = cmbYear.SelectedValue;
            hidSalaryMonthId.Value = cmbMonths.SelectedValue;
            hidSalaryStaffgroup.Value = cmbStaffGroups.SelectedValue;

            ReFillSalaryDetails();
            SetControlVisibility(false);
            if (grdSalaryDetails.Rows.Count > 1)
            {
                btnUnpublish.Enabled = true;
                btnSave.Enabled = true;
                btnExport.Enabled = true;
                btnExportAll.Enabled = true;
                btnExportEarnings.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reload salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            mbReloadRequired = true;
            btnShow_Click(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ExportDetails();
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
    /// This event is used to export salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportAll_Click(object sender, EventArgs e)
    {
        try
        {
            mbIsExportAll = true;
            ExportDetails();
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
	/// This event is used to refill all staff's retirement notice details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lnlRetirementNotice_Click(object sender, EventArgs e)
	{
		try
		{
			FillRetirementNoticeDetails();
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
    /// This event is used to change page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            hidSelectedPageIndex.Value = PageDropDownList.SelectedValue;
            lblCurrentPage.Text = "Page " + PageDropDownList.SelectedValue + "  of " + PageDropDownList.Items.Count;

            FillSalaryDetails(false);

            SetDivHeight();

            int iLastIndex = Convert.ToInt32(PageDropDownList.SelectedValue) * Constants.I_GRID_PAGE_COUNT;

            lblStartIndex.Text = ((Convert.ToInt32(PageDropDownList.SelectedValue) - 1) * Constants.I_GRID_PAGE_COUNT + 1).ToString();
            lblEndIndex.Text = iLastIndex < miTotalRecords ? iLastIndex.ToString() : miTotalRecords.ToString();
            lblTotalRecords.Text = miTotalRecords.ToString();
        }
        catch (NoRecordFoundException ex)
        {
            lblNoRecordMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to set the ListView controls set the serial no for each row of ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwRetirementDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewItem oCurrentItem = (ListViewItem)e.Item;
                Label lblRemainingDays= (Label)oCurrentItem.FindControl("lblDays");
                int iRemainingDays =lblRemainingDays.Text.ToInt();
                if (iRemainingDays < 0)
                {
                    HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                    oHtmlTableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Pink");
                    
                }
				Label lblSrNo = (Label)oCurrentItem.FindControl("lblSrNo");
				lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to export earning details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportEarnings_Click(object sender, EventArgs e)
    {
        try
        {
            mbIsExportAll = true;
            LoadSalaryDetails(false);

            if (grdSalaryDetails.Rows.Count > 0)
            {
                DataTable oDataTable = null;
                if (ViewState[S_EMPTY_TABLE] != null)
                    oDataTable = (DataTable)ViewState[S_EMPTY_TABLE];

                int iColumnCount = grdSalaryDetails.Rows[0].Cells.Count - 1;
                int iRowCount = grdSalaryDetails.Rows.Count;

                int iNewCellIndex;
                if (iRowCount > 0)
                {
                    List<GrossSalaryDetails> lstSalaryDetails = oSalaryDetails.GetGrossSalary(miSchoolId, cmbMonths.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt());

                    oDataTable.Columns.Add("Change Over Previous Month");

                    for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
                    {
                        iNewCellIndex = 1;
                        DataRow oDataRow = oDataTable.NewRow();
                        oDataRow[0] = iRowIndex + 1;

                        int iUserId = grdSalaryDetails.DataKeys[iRowIndex]["UserId"].ToInt();

                        for (int iCellIndex = 0; iCellIndex < iColumnCount; iCellIndex++)
                        {
                            if (iCellIndex > 7)
                                oDataRow[iNewCellIndex++] = grdSalaryDetails.Rows[iRowIndex].Cells[iCellIndex].Text;
                        }

                        if (lstSalaryDetails.Count > 0)
                        {
                            int iGrossSalary = oDataRow["Gross Salary"].ToInt();
                            var salary = lstSalaryDetails.Where(sd => sd.UserId == iUserId).FirstOrDefault();
                            oDataRow["Change Over Previous Month"] = iGrossSalary - (salary != null ? salary.Amount : 0);
                        }
                        
                        oDataTable.Rows.Add(oDataRow);
                    }
                }

                RemoveSummaryRows(oDataTable);
                RemoveAttendanceDependentEDs(oDataTable);
                RemoveUnnecessaryColumns(oDataTable);
                AddRemoveColumns(oDataTable);
                UpdateSerialNumbers(oDataTable);

                DataRow[] drTotal = oDataTable.Select("Name  = 'Net Total'");
                if (drTotal.Length > 0)
                {
                    int iAmt = oDataTable.AsEnumerable().Where(dt => dt.Field<string>("Name") != "Net Total").Select(dt => dt.Field<string>("Change Over Previous Month").ToInt()).Sum();
                    drTotal[0]["Change Over Previous Month"] = iAmt;
                    drTotal[0]["Sr No"] = string.Empty;

                    DataRow dr1 = oDataTable.NewRow();
                    oDataTable.Rows.InsertAt(dr1, oDataTable.Rows.Count - 1);
                }

                DataRow oDtr1 = oDataTable.NewRow();
                oDataTable.Rows.Add(oDtr1);

                int iTotalRowCount = oDataTable.Rows.Count - 3;
                DataRow oDtr = oDataTable.NewRow();
                oDtr["Name"] = "Total Staff : " + iTotalRowCount;
                oDataTable.Rows.Add(oDtr);

                UpdateHeaders(oDataTable);

                if (oDataTable.IsNonEmpty())
                    ExportToExcel("EarningDetails.xls", oDataTable);
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set fields.
    /// </summary>
    private void SetFields()
    {
        lblMessage.Text = string.Empty;
        mbIsPageInit = false;
        LoadSalaryDetails(true);
        DisplayLeavesConfigMsg();

        lblMessage.Visible = true;
        lblMessage.Text = S_SAVE_MESSAGE;
        hidIsSaveClick.Value = Constants.S_NO;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
	    if (string.IsNullOrEmpty(Request.QueryString.ToString()))
		    return;
	    
		if (QueryString["MonthId"] != null)
	    {
		    cmbMonths.SelectedValue = QueryString["MonthId"];
		    hidSalaryMonthId.Value = cmbMonths.SelectedValue;
	    }
	    if (QueryString["Year"] != null)
	    {
		    cmbYear.SelectedValue = QueryString["Year"];
		    hidSalaryYear.Value = cmbYear.SelectedValue;
	    }
	    if (QueryString["StaffGroupId"] != null)
	    {
		    cmbStaffGroups.SelectedValue = QueryString["StaffGroupId"];
		    hidMinRecordsStaffGroupId.Value = cmbStaffGroups.SelectedValue;
		    hidSalaryStaffgroup.Value = cmbStaffGroups.SelectedValue;
	    }
	    if (QueryString["Filter"] != null)
	    {
		    txtSearch.Text = QueryString["Filter"];
		    hidSalaryFilter.Value = txtSearch.Text;
	    }
	    HidMonthDays.Value = DateTime.DaysInMonth(Convert.ToInt32(cmbYear.SelectedValue), Convert.ToInt32(cmbMonths.SelectedValue)).ToString();
    }

    /// <summary>
    /// This method is used to fill pager dropdownlist.
    /// </summary>
    private void FillPagerDropdown()
    {
        PageDropDownList.Items.Clear();
        if (miTotalPages > 0)
        {
            for (int iPageIndex = 0; iPageIndex < miTotalPages; iPageIndex++)
            {
                // Create a ListItem object to represent a page.
                int pageNumber = iPageIndex + 1;
                ListItem item = new ListItem(pageNumber.ToString());

                //if (iPageIndex == (PageDropDownList.SelectedValue == string.Empty?0: PageDropDownList.SelectedValue.ToInt()))
                
                if (iPageIndex == (hidSelectedPageIndex.Value == string.Empty ? 0 : hidSelectedPageIndex.Value.ToInt()))
                    item.Selected = true;

                // Add the ListItem object to the Items collection of the DropDownList.
                PageDropDownList.Items.Add(item);
            }

            PageDropDownList.SelectedValue = hidSelectedPageIndex.Value;
            lblCurrentPage.Text = "Page " + PageDropDownList.SelectedValue + "  of " + miTotalPages;

            int iLastIndex = Convert.ToInt32(PageDropDownList.SelectedValue) * Constants.I_GRID_PAGE_COUNT;

            lblStartIndex.Text = ((Convert.ToInt32(PageDropDownList.SelectedValue) - 1) * Constants.I_GRID_PAGE_COUNT + 1).ToString();
            lblEndIndex.Text = iLastIndex < miTotalRecords ? iLastIndex.ToString() : miTotalRecords.ToString();
            lblTotalRecords.Text = miTotalRecords.ToString();

            tblPager.Visible = true;
            tblPageDetails.Visible = true;
            if (miTotalPages == 1 || grdSalaryDetails.Rows.Count <= 0)
            {
                tblPager.Visible = false;
                tblPageDetails.Visible = false;
            }
        }
        else
        {
            tblPager.Visible = false;
            tblPageDetails.Visible = false;
        }
    }

	/// <summary>
	/// This method is used to fill up all staff retirement notice details listview.
	/// </summary>
	private void FillRetirementNoticeDetails()
	{
			RetirementNoticeConfigBL oRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId,miFinancialYearId,miAcademicYearId,miUserId);
			List<StaffMemberRetirementNotice> lstStaffRetirementNotice = oRetirementNoticeConfigBL.GetAllStaffsRetirementNotices();
			lstvwRetirementDetails.DataSource = lstStaffRetirementNotice;
			lstvwRetirementDetails.DataBind();
	}

    /// <summary>
    /// This Method is used to get the salary details
    /// </summary>
    private void ExportDetails()
    {
        try
        {
            LoadSalaryDetails(false);

            if (grdSalaryDetails.Rows.Count > 0)
            {
                DataTable oDataTable = null;
                if (ViewState[S_EMPTY_TABLE] != null)
                    oDataTable = (DataTable)ViewState[S_EMPTY_TABLE];

                int iColumnCount = grdSalaryDetails.Rows[0].Cells.Count - 1;
                int iRowCount = grdSalaryDetails.Rows.Count;

                if (oDataTable.Columns.Contains(S_FORM_NO_16))
                    oDataTable.Columns.Remove(S_FORM_NO_16);

                int iNewCellIndex;
                if (iRowCount > 0)
                {
                    for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
                    {
                        iNewCellIndex = 1;
                        DataRow oDataRow = oDataTable.NewRow();
                        oDataRow[0] = iRowIndex + 1;
                        for (int iCellIndex = 0; iCellIndex < iColumnCount; iCellIndex++)
                        {
                            if (iCellIndex > 7)
                                oDataRow[iNewCellIndex++] = grdSalaryDetails.Rows[iRowIndex].Cells[iCellIndex].Text;
                        }
                        oDataTable.Rows.Add(oDataRow);
                    }
                }

                if (oDataTable.Columns.Contains(S_ISDELETED))
                    oDataTable.Columns.Remove(S_ISDELETED);

                if (oDataTable.IsNonEmpty())
                {
                    if (!string.IsNullOrEmpty(hidMonthList.Value))
                    {
                        int iRowIndex = oDataTable.Rows.Count;
                        DataRow oDataRow = oDataTable.NewRow();
                        oDataTable.Rows.InsertAt(oDataRow, iRowIndex);

                        iRowIndex = oDataTable.Rows.Count;
                        oDataRow = oDataTable.NewRow();
                        oDataTable.Rows.InsertAt(oDataRow, iRowIndex);

                        oDataTable.Rows[iRowIndex][0] = "Including Salary Difference of Month(s); ";
                        oDataTable.Rows[iRowIndex][1] = hidMonthList.Value;
                    }
                    ExportToExcel("SalaryDetails.xls", oDataTable);
                }
            }
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
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        string sQueryString = "ActivityId=" + Constants.I_ONE;
        hidActivityQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);

        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumShow.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        ApplyMouseHoverEffect(new List<Button> { btnSave, btnUnpublish, btnExport, btnShow, btnExportAll, btnRefresh, btnClosePopUp, btnExportEarnings });

        hidLeaveTransferMonth.Value = Settings.LeaveTransferMonth.ToString();
        btnUnpublish.Attributes.Add("onclick", "if(!DisplayConfirmation()) return false;");
        lnkUserLeaves.Attributes.Add("onclick", "if(!OpenPopup()) return false;");
        lnkStaffAttendance.Attributes.Add("onclick", "if(!OpenAttendancePopup()) return false;");
        lnkDaywiseStaffLeave.Attributes.Add("onclick", "DatewiseLeavesPopup(); return false;");
        lnkPaymentDetails.Attributes.Add("onclick","OpenPaymentDetailsPopup(); return false;");
        lnkExportStaffLEave.Attributes.Add("onclick", "OpenLeaveExportPopup(); return false;");
        lnkMonthwiseAttendance.Attributes.Add("onclick", "if(!OpenMonthwiseAttendancePopup()) return false;");
        lnkODDetails.Attributes.Add("onclick", "if(!OpenODDetailsPopup()) return false;");
        lnkExcludeFromSalary.Attributes.Add("onclick", "if(!OpenAvtivityDetailsScreen()) return false;");

        btnSave.Attributes.Add("onclick", "SetInitStatus()");

        cmbStaffGroups.Attributes.Add("onchange", "HideButtons()");
        cmbMonths.Attributes.Add("onchange", "HideButtons()");
        cmbYear.Attributes.Add("onchange", "HideButtons()");
        //tdStaffLeave.Visible = Settings.ShowStaffAttendanceMenu;
        //tdStaffLeaveSeparater.Visible = Settings.ShowStaffAttendanceMenu;

        if (SchoolBase.Settings.IsBiometriceEnabled)
        {
            tdStaffInOutDetails.Visible = true;
            lnkStaffInOutDetails.Attributes.Add("onclick", "if(!OpenInOutDetailsPopup()) return false;");
        }
        else
            tdStaffInOutDetails.Visible = false;
    }

    /// <summary>
    /// This method is used to set default properties.
    /// </summary>
    private void SetDefaultProperties()
    {
        System.Web.UI.HtmlControls.HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = btnShow.UniqueID;
    }

    /// <summary>
    /// This method is used to fill all comboboxes.
    /// </summary>
    private void FillComboboxes()
    {   
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);

        // Fill staff group combobox

        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffGroups, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);
        ListSource.FillDropDownList(oSalaryDetailsBL.Months, cmbMonths, "Month", "MonthId", string.Empty);
        ListSource.FillDropDownList(oSalaryDetailsBL.Years, cmbYear, "Year", "Year", string.Empty);

        SetDefaultValues(oSalaryDetailsBL.MinUserStaffGroupId);
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    /// <param name="dataTable"></param>
    private void SetDefaultValues(int aiMinUserStaffGroupId)
    {
        cmbYear.SelectedValue = DateTime.Now.Year.ToString();
        cmbMonths.SelectedValue = DateTime.Now.Month.ToString();
        HidMonthDays.Value = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();

        hidSalaryFilter.Value = string.Empty;
        hidSalaryYear.Value = DateTime.Now.Year.ToString();
        hidSalaryMonthId.Value = DateTime.Now.Month.ToString();
        hidSalaryStaffgroup.Value = aiMinUserStaffGroupId.ToString();


        string sMinRecordsStaffGroupsCount = Constants.S_ZERO;
        if (aiMinUserStaffGroupId != 0)
            sMinRecordsStaffGroupsCount = aiMinUserStaffGroupId.ToString();

        if (cmbStaffGroups.Items.Count > 0)
        {
            cmbStaffGroups.SelectedValue = sMinRecordsStaffGroupsCount;
            hidMinRecordsStaffGroupId.Value = sMinRecordsStaffGroupsCount;
        }
    }

    /// <summary>
    /// This method is used to set querystring.
    /// </summary>
    private void SetQueryString()
    {
        string sQueryString = "MonthId=" + cmbMonths.SelectedValue +
                              "&Year=" + cmbYear.SelectedValue +
                              "&StaffGroupId=" + cmbStaffGroups.SelectedValue +
                              "&IsStaticOutput=" + hidIsStaticOutput.Value +
                              "&Filter=" + txtSearch.Text.Trim();
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }

    /// <summary>
    /// This method is used to check combobox value and fill salary grid according to it.
    /// </summary>
    private void ReFillSalaryDetails()
    {
        if (cmbMonths.SelectedValue != Constants.S_ZERO && cmbYear.SelectedValue != Constants.S_ZERO)
        {
            int iMonthId = Convert.ToInt32(cmbMonths.SelectedValue);
            int iYear = Convert.ToInt32(cmbYear.SelectedValue);
            HidMonthDays.Value = DateTime.DaysInMonth(iYear, iMonthId).ToString();

            trLegend.Visible = true;
            divContainer.Visible = true;
            lblErr.Text = string.Empty;
            trSuccessMessage.Visible = false;

            mbIsPageInit = false;

            int iLastSelectedYear = Convert.ToInt32(hidSelectedYear.Value);
            int iLastSelectedMonth = Convert.ToInt32(hidSelectedMonth.Value);

            bool bReloadGrid = iLastSelectedMonth == iMonthId && iLastSelectedYear == iYear;
            bReloadGrid = mbReloadRequired || !bReloadGrid;

            LoadSalaryDetails(bReloadGrid);
            DisplayLeavesConfigMsg();
            
            mbReloadRequired = false;
        }
        else
        {
            divContainer.Visible = false;
            btnUnpublish.Visible = false;
            trLegend.Visible = false;
            trMessages.Visible = false;
            btnSave.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to fill salary details grid.
    /// </summary>
    private void LoadSalaryDetails(bool abReloadGrid)
    {
        FillSalaryDetails(abReloadGrid);
        FillPagerDropdown();
        SetDivHeight();
    }

    /// <summary>
    /// This method is used to set div height.
    /// </summary>
    private void SetDivHeight()
    {
        if (miTotalRecords > 20)
        {
            if (PageDropDownList.SelectedValue.ToInt() == miTotalPages && miTotalRecords % Constants.I_GRID_PAGE_COUNT != 0)
                divContainer.Style.Add("height", (((miTotalRecords % Constants.I_GRID_PAGE_COUNT) + 2) * 30).ToString() + "px !important");
            else
                divContainer.Style.Add("height", "620px !important");
        }
        else
            divContainer.Style.Add("height", ((miTotalRecords + 2) * 30).ToString() + "px !important");
    }

    /// <summary>
    /// This method is used to fill salary details.
    /// </summary>
    /// <param name="abReloadGrid"></param>
    private void FillSalaryDetails(bool abReloadGrid)
    {
        int iStaffGroupId;
        int iMonthId;
        int iYear;
        string sFilter;
        int iPageIndex = 0;

        moTempDataTable = new DataTable();
        lblMessage.Text = string.Empty;
        GetStaffGroupMonthYear(out iStaffGroupId, out iMonthId, out iYear, out sFilter, out iPageIndex);
        if (mbIsExportAll)
        {
            iStaffGroupId = 0;
            iMonthId =Convert.ToInt32(cmbMonths.SelectedValue);
            iYear =Convert.ToInt32(cmbYear.SelectedValue);
            abReloadGrid = true;
        }
        if (iMonthId != 0 && iYear != 0)
        {
            hidColumnIndexes.Value = string.Empty;
            hidSalaryDiffColumnIndex.Value = string.Empty;
            
            DataTable oDataTable = null;
            hidSelectedPageIndex.Value = iPageIndex.ToString();
            oDataTable = GetSalaryDetails(iMonthId, iYear, iStaffGroupId, abReloadGrid, sFilter, iPageIndex);

            if (mbIsExportAll && miSchoolId == Constants.SchoolId.PPSH.ToInt())
            {
                DataRow[] drRows = oDataTable.Select("StaffGroupId <>'23'");

                if (drRows.Length > 0)
                    oDataTable = drRows.CopyToDataTable();
            }

            if (oDataTable.IsNonEmpty())
                oDataTable.Columns.Add(S_FORM_NO_16);

            grdSalaryDetails.DataSource = oDataTable;
            grdSalaryDetails.DataBind();
            
            if (grdSalaryDetails.Rows.Count > 0 || ( !string.IsNullOrEmpty(hidSalaryFilter.Value) && grdSalaryDetails.Rows.Count > 0))
            {
                SetGridVisibility(true);
                SetControlVisibility(false);                
                hidSelectedStaffGroup.Value = cmbStaffGroups.SelectedValue;

                List<string> lstUserids = oDataTable.AsEnumerable().Where(user => Convert.ToInt32(user.Field<string>("UserId")) >= 0).Select(user => user.Field<string>("UserId")).ToList();
                string sUserIds = string.Join(",", lstUserids);
                string sQueryString = "FormNo16ReportUI.aspx?" + CommonUtility.EncryptQuerystring("UserId=" + sUserIds + "&ShowReport=N&IsForSingle=N");
                hidITQueryString.Value = sQueryString;
            }
            else
            {
                trSalaryMessage.Visible = false;
                SetGridVisibility(false);
                SetControlVisibility(true);
            }

            hidSelectedMonth.Value = cmbMonths.SelectedValue;
            hidSelectedYear.Value = cmbYear.SelectedValue;
            SetQueryString();
        }
        ViewState[S_EMPTY_TABLE] = moTempDataTable;
    }

    /// <summary>
    /// This method is used to hide gridview cells.
    /// </summary>
    /// <param name="aogridViewRow"></param>
    private void HideCells(GridViewRow aogridViewRow)
    {
        for (int iCellIndex = 2; iCellIndex <= I_START_CELL_INDEX; iCellIndex++)
            aogridViewRow.Cells[iCellIndex].Visible = false;
    }

    /// <summary>
    /// This method is used to format cella.
    /// </summary>
    /// <param name="e"></param>
    private void FormatGridCells(GridViewRowEventArgs e)
    {
        HideCells(e.Row);

        int iCellIndex = 0;
        int iColumnIndex = 0;
        int iCellCount = e.Row.Cells.Count - 2;// last column is Form No 16

        int iTempCellIndex = 0;

        string sText = string.Empty;
        string sAttendanceOrLeave = string.Empty;
        TextBox oTextBox;
        TableCellCollection cells = e.Row.Cells;

        bool bIsApplicable = false;
        if (e.Row.Cells[I_DISPLAY_CONTROLS_COLUMN_INDEX].Text == Constants.S_YES && hidIsStaticOutput.Value == Constants.S_NO &&
            e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text != null &&
            HttpUtility.HtmlDecode(e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text).Trim() != string.Empty)
            bIsApplicable = true;

        if (!bIsApplicable && !mbIsPageInit &&
            e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text != null &&
            HttpUtility.HtmlDecode(e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text).Trim() != string.Empty)
            mbShowConfigMessage = true;


        if (bIsApplicable && !mbIsPageInit)
            mbDisplaySaveButton = true;

        Button oButton = (Button)e.Row.FindControl("btnSaveSalary");

        oButton.Visible = bIsApplicable;
        Label lblLeaveBalance = null;
        string sUserName = Server.HtmlDecode(e.Row.Cells[I_NAME_COLUMN_INDEX].Text);
        HyperLink hlnkFormNo16 = new HyperLink { Text = "Open" };
        foreach (TableCell cell in cells)
        {
            oTextBox = new TextBox();
            sText = cell.Text;

            cell.Style.Add("padding-left", "5");
            cell.Style.Add("padding-right", "5");

            string sColumnName = grdSalaryDetails.HeaderRow.Cells[iCellIndex].Text;
            cell.Attributes.Add("title", "User : " + sUserName + " [" + sColumnName + "]");

            string[] sEarningsDeductionsValue = sText.Split('_');

            if (sText.Contains("_") && (sText.Substring(sText.IndexOf("_") + 1, 2) == S_ATTENDANCE || sText.Substring(sText.IndexOf("_") + 1, 2) == S_LEAVE))
            {
                sAttendanceOrLeave = sText.Substring(sText.IndexOf("_") + 1, 2);
                cell.Text = sText.Substring(0, sText.IndexOf("_"));
                cell.HorizontalAlign = HorizontalAlign.Right;

                if ((cell.Text == "0.00" || cell.Text == Constants.S_ZERO || cell.Text == "0.0"))
                {
                    Color oColor = Color.Transparent;
                    switch (sAttendanceOrLeave)
                    {
                        case S_ATTENDANCE: oColor = Color.LightSkyBlue; break;
                        case S_LEAVE: oColor = Color.LightSalmon; break;
                    }
                    cell.BackColor = oColor;
                }
            }
            else if ((sText.Contains("_") && (sEarningsDeductionsValue.Length == 3 ||
                                (sEarningsDeductionsValue.Length == I_EARNINGS_DEDUCTIONS_LENGTH && sText.Substring(sText.LastIndexOf("_") + 1, 1) == Constants.S_ZERO))))
            {
                cell.Text = sText.Substring(0, sText.IndexOf("_"));
                sAttendanceOrLeave = sText.Substring(sText.IndexOf("_") + 1, 2);

                if (bIsApplicable)
                {
                    oTextBox.ID = "txt_" + sText.Substring(sText.IndexOf("_") + 1);
                    SetTextboxProperties(oTextBox, sAttendanceOrLeave, cell.Text, lblLeaveBalance);

                    HiddenField hiddenField = new HiddenField();
                    hiddenField.ID = "hid_" + sText.Substring(sText.IndexOf("_") + 1);
                    hiddenField.Value = oTextBox.Text;

                    lblLeaveBalance = null;
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    cell.Controls.Add(oTextBox);
                    cell.Controls.Add(hiddenField);

                    // This condition is used to add a link after income tax textbox for claculating and displaying it in textbox.
                    if (grdSalaryDetails.HeaderRow.Cells[iCellIndex].Text == S_INCOME_TAX)
                    {
                        int iUserId = Convert.ToInt32(grdSalaryDetails.DataKeys[e.Row.RowIndex]["UserId"]);
                        string sQueryString = "FormNo16ReportUI.aspx?" + CommonUtility.EncryptQuerystring("UserId=" + iUserId + "&ShowReport=N&IsForSingle=Y");
                        LinkButton oLinkButton = new LinkButton { Text = "Calc", ID = "lnk_" + sText.Substring(sText.IndexOf("_") + 1) };
                        oLinkButton.Attributes.Add("onclick", "CalculateAmount(this,'" + sQueryString + "'); return false;");
                        cell.Controls.Add(oLinkButton);
                    }
                }
                else if ((cell.Text == "0.00" || cell.Text == Constants.S_ZERO || cell.Text == "0.0"))
                {
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    Color oColor = Color.Transparent;
                    switch (sAttendanceOrLeave)
                    {
                        case S_ATTENDANCE: oColor = Color.LightSkyBlue; break;
                        case S_LEAVE: oColor = Color.LightSalmon; break;
                        case S_EARNINGS_DEDUCTIONS: oColor = Color.LightPink; break;
                    }
                    cell.BackColor = oColor;
                }
                else
                    cell.HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                if (sColumnName == S_FORM_NO_16)
                {
                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "Center");
                    e.Row.Cells[iCellIndex].Controls.Add(hlnkFormNo16);
                    int iUserId = Convert.ToInt32(grdSalaryDetails.DataKeys[e.Row.RowIndex]["UserId"]);
                    hlnkFormNo16.NavigateUrl = string.Format("javascript:OprnFormNo16Report('{0}');", "FormNo16ReportUI.aspx?" + CommonUtility.EncryptQuerystring("UserId=" + iUserId + "&ShowReport=Y&IsForSingle=Y"));                    
                }

                if (sColumnName == S_ISDELETED)
                {
                    cell.Visible = false;
                    if (cell.Text == Constants.S_ONE)
                        e.Row.ForeColor = Color.Red;
                }

                if (cell.Text.Contains("_LB") && sEarningsDeductionsValue.Length == 2)
                {
                    lblLeaveBalance = new Label();
                    lblLeaveBalance.ID = "lblLB" + iCellIndex;
                    lblLeaveBalance.Text = sText.Substring(0, sText.IndexOf("_"));
                    if (Convert.ToDecimal(lblLeaveBalance.Text) < 0)
                    {
                        cell.ForeColor = Color.Red;
                        cell.Font.Bold = true;
                    }
                    cell.Controls.Add(lblLeaveBalance);
                }

                if (sEarningsDeductionsValue.Length == I_LEAVE_DEDUCTED_LENGTH || sEarningsDeductionsValue.Length == I_EARNINGS_DEDUCTIONS_LENGTH)
                    cell.Text = sText.Substring(0, sText.IndexOf("_"));

                if (iCellIndex == I_NAME_COLUMN_INDEX)
                {
                    Label oLabel = new Label { ID = "lblName", Text = cell.Text };
                    cell.Controls.Add(oLabel);
                }

                if (iCellIndex == 0)
                    cell.HorizontalAlign = HorizontalAlign.Center;
                else if (iCellIndex != I_NAME_COLUMN_INDEX && iCellIndex != I_DESIGNATION_COLUMN_INDEX)
                    cell.HorizontalAlign = HorizontalAlign.Right;

                if (cell.Text == Constants.S_ZERO && grdSalaryDetails.HeaderRow.Cells[iCellIndex].Text != "Unpaid Leaves" && !sText.Contains("_LB")
                    && e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text != null &&
                    HttpUtility.HtmlDecode(e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text).Trim() != string.Empty &&
                    (sText.Contains("_ED") && sText.Substring(sText.LastIndexOf("_") + 1) != "1"))
                    cell.BackColor = Color.LightPink;
            }

            if (!msColumnNumbers.Contains(iCellIndex + ","))
            {
                if (cell.Text == "-1" && !hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
                    cell.Text = string.Empty;
                iTempCellIndex++;
            }

            if (cell.Text == "-1" && !hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
                cell.Text = string.Empty;
            cell.Wrap = false;

            string[] oSalDiffColumns = hidSalaryDiffColumnIndex.Value.Split(',');
            if ((e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text != null && HttpUtility.HtmlDecode(e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text).Trim() != string.Empty) && (oSalDiffColumns.Contains(iColumnIndex.ToString()) && !string.IsNullOrEmpty(cell.Text) && Convert.ToInt32(cell.Text) != 0))
                cell.BackColor = Color.FromName("#E1E1FF");

            if (hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
            {
                cell.Font.Bold = true;
                if (iColumnIndex == iCellCount - 1)
                {
                    cell.BackColor = Color.LightSteelBlue;
                    cell.ForeColor = Color.Maroon;
                }
                else if (e.Row.Cells[I_NAME_COLUMN_INDEX].Text != "Total Total")
                {
                    string str = grdSalaryDetails.HeaderRow.Cells[iCellIndex].Text;
                    cell.BackColor = Color.LightGray;
                    cell.ForeColor = Color.Navy;
                }
            }

            cell.CssClass = "GridDate";
            iCellIndex++;
            iColumnIndex++;
        }

        if (e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text == null || HttpUtility.HtmlDecode(e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text).Trim() == string.Empty)
        {
            e.Row.Font.Bold = true;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            if (e.Row.Cells[I_NAME_COLUMN_INDEX].Text == "Total Total")
            {
                e.Row.Cells[I_NAME_COLUMN_INDEX].Text = "Total";
                e.Row.ForeColor = Color.Maroon;
                e.Row.BackColor = Color.LightSteelBlue;
            }
            else
            {
                e.Row.ForeColor = Color.Navy;
                e.Row.BackColor = Color.LightGray;
                hlnkFormNo16.Visible = false;
            }
            RemoveCells(e);
        }

        int iRowIndex = e.Row.RowIndex;
        Button oBtn = (Button)e.Row.FindControl("btnSaveSalary");
        oBtn.Attributes.Add("onclick", "if(!CheckGridRow(" + iRowIndex + ",true)) return false;");
    }

    /// <summary>
    /// This method is used to set textbox properties.
    /// </summary>
    /// <param name="aoTextBox"></param>
    /// <param name="asAttendanceOrLeave"></param>
    /// <param name="sText"></param>
    private void SetTextboxProperties(TextBox aoTextBox, string asAttendanceOrLeave, string sText, Label lblLeaveBalance)
    {
        
        if (asAttendanceOrLeave != S_ATTENDANCE && asAttendanceOrLeave != S_LEAVE)
        {
            aoTextBox.Width = Unit.Pixel(100);
            aoTextBox.MaxLength = 10;
            aoTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false);");
            aoTextBox.Attributes.Add("onblur", "extractNumber(this,2,false)");
            aoTextBox.Attributes.Add("onkeyup", "extractNumber(this,2,false)");
        }

        aoTextBox.Text = sText;

        aoTextBox.Style.Add("text-align", "right");
        aoTextBox.Style.Add("padding-right", "2px");
        aoTextBox.Attributes.Add("onpaste", "event.returnValue=false");
        aoTextBox.Attributes.Add("ondrop", "event.returnValue=false");
        aoTextBox.Attributes.Add("onfocus", "GetValue(this)");
        aoTextBox.Attributes.Add("onchange", "Validate(this,null,null)");

        string sLeaveBalance;
        if (lblLeaveBalance != null)
            sLeaveBalance = lblLeaveBalance.Text.Trim();
        else
            sLeaveBalance = "-999";

        lblLeaveBalance = null;
        if (sText == "0.00" || sText == Constants.S_ZERO || sText == "0.0")
        {
            Color oColor = Color.Transparent;
            switch (asAttendanceOrLeave)
            {
                case S_ATTENDANCE: oColor = Color.LightSkyBlue; break;
                case S_LEAVE: oColor = Color.LightSalmon; break;
                case S_EARNINGS_DEDUCTIONS: oColor = Color.LightPink; break;
            }
            aoTextBox.BackColor = oColor;
        }
    }

    /// <summary>
    /// This method is used to set values.
    /// </summary>
    /// <param name="iStaffGroupId"></param>
    /// <param name="iMonthId"></param>
    /// <param name="iYear"></param>
    private void GetStaffGroupMonthYear(out int iStaffGroupId, out int iMonthId, out int iYear, out string asFilter, out int aiPageIndex)
    {
        if (string.IsNullOrEmpty(cmbStaffGroups.SelectedValue))
        {
            if (IsPostBack)
            {
                iStaffGroupId = Convert.ToInt32(Request.Params[cmbStaffGroups.ClientID.Replace("_", "$")]);
                iMonthId = Convert.ToInt32(Request.Params[cmbMonths.ClientID.Replace("_", "$")]);
                iYear = Convert.ToInt32(Request.Params[cmbYear.ClientID.Replace("_", "$")]);
                asFilter = Convert.ToString(Request.Params[hidSalaryFilter.ClientID.Replace("_", "$")]);
                aiPageIndex = Convert.ToInt32(Request.Params[PageDropDownList.ClientID.Replace("_", "$")]);
            }
            else
            {
                iStaffGroupId = Convert.ToInt32(hidMinRecordsStaffGroupId.Value);
                iMonthId = DateTime.Now.Month;
                iYear = DateTime.Now.Year;
                asFilter = hidSalaryFilter.Value;
                aiPageIndex = PageDropDownList.SelectedValue.ToInt();
            }
        }
        else
        {
            if (!IsPostBack)
            {
                cmbStaffGroups.SelectedValue = hidMinRecordsStaffGroupId.Value;
                iStaffGroupId = Convert.ToInt32(cmbStaffGroups.SelectedValue);
            }
            else
                iStaffGroupId = Convert.ToInt32(hidSalaryStaffgroup.Value);

            iMonthId = Convert.ToInt32(hidSalaryMonthId.Value);
            iYear = Convert.ToInt32(hidSalaryYear.Value);
            asFilter = hidSalaryFilter.Value;
            aiPageIndex = (PageDropDownList.SelectedValue == string.Empty ? 0 : PageDropDownList.SelectedValue.ToInt());
        }

        if (aiPageIndex == 0)
            aiPageIndex = 1;
    }

    /// <summary>
    /// This method is used to set visibility of controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetGridVisibility(bool abAction)
    {
        divContainer.Visible = abAction;
        btnExport.Visible = abAction;
        btnExportAll.Visible = abAction;
        btnExportEarnings.Visible = abAction;
        lblNoRecordMsg.Visible = !abAction;
        trLegend.Visible = abAction;
        trUserLeaves.Visible = abAction;
        trNote.Visible = abAction;
        btnSave.Visible = false;
        if (mbDisplaySaveButton && grdSalaryDetails.Enabled == true)
            btnSave.Visible = true;
    }

    /// <summary>
    /// This method is used to return datatable of salarydetails.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiMonthId"></param>
    /// <param name="aiYear"></param>
    /// <param name="aiStaffGroupId"></param>
    /// <returns></returns>
    private DataTable GetSalaryDetails(int aiMonthId, int aiYear, int aiStaffGroupId, bool abReloadGrid, string asFilter, int aiPageIndex)
    {
        const int I_MONTH_YEAR_TABLE_INDEX = 2;
        DataSet oDSSalaryDetails = null;
        DataTable oDTSalaryDetails = null;
        trUnpublishSalaryNote.Visible = false;

        hidMonthList.Value = string.Empty;

        oSalaryDetails = new SalaryDetailsBL(miSchoolId, miAcademicYearId);

        oSalaryDetails.CacheTimeout = I_CACHE_TIMEOUT;

        if (abReloadGrid || mbIsPageInit || Cache[S_SALARY_ENTITY_LIST] == null)
        {
            oDSSalaryDetails = oSalaryDetails.GetSalaryDetailsDataset(aiMonthId, aiYear, 0, string.Empty, 0, 9999, mbIsPageInit, true);
            Cache.Insert(S_SALARY_ENTITY_LIST, oDSSalaryDetails, null, DateTime.Now.AddSeconds(I_CACHE_TIMEOUT), System.Web.Caching.Cache.NoSlidingExpiration);
            hidLeaveIntervalMonth.Value = oSalaryDetails.IsLeaveIntervalMonth.ToString();
        }
        else
            oDSSalaryDetails = Cache[S_SALARY_ENTITY_LIST] as DataSet;

        int iStartIndex = (aiPageIndex - 1) * Constants.I_GRID_PAGE_COUNT;
        int iEndIndex = iStartIndex + Constants.I_GRID_PAGE_COUNT;

        if (oDSSalaryDetails != null && oDSSalaryDetails.Tables.Count > 0)
        {
            oDTSalaryDetails = oDSSalaryDetails.Tables[I_SALARY_DETAILS];
            if (oDTSalaryDetails.IsNonEmpty())
            {
                IEnumerable<DataRow> SortedSalaryDetails;
                if (oDTSalaryDetails.Columns.Count != 1)
                {
                    if (aiStaffGroupId != 0)
                    {
                        if (string.IsNullOrEmpty(asFilter))
                        {
                            SortedSalaryDetails = from SalDetails in oDTSalaryDetails.AsEnumerable()
                                                  where Convert.ToInt32(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId
                                                  select SalDetails;
                        }
                        else
                        {
                            SortedSalaryDetails = from SalDetails in oDTSalaryDetails.AsEnumerable()
                                                  where Convert.ToInt32(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId
                                                  && SalDetails.Field<string>("Name").ToLower().Contains(asFilter.ToLower())
                                                  select SalDetails;
                        }
                    }
                    else
                    {
                        SortedSalaryDetails = from SalDetails in oDTSalaryDetails.AsEnumerable()
                                              where SalDetails.Field<string>("Name").ToLower().Contains(asFilter.ToLower())
                                              select SalDetails;
                    }

                    int iTotalRows = 0;
                    foreach (DataRow dr in SortedSalaryDetails)
                    {
                        dr["Sr No"] = ++iTotalRows;
                    }

                    if (iTotalRows != 0)
                    {   
                        SetTotalPages(iTotalRows);
                        miTotalRecords = iTotalRows;
                        if (mbIsExportAll)
                        {
                            iStartIndex = Constants.I_ZERO;
                            iEndIndex = Constants.I_DEFAULT_MAX_VALUE;
                        }
                        SortedSalaryDetails = from SalDetails in SortedSalaryDetails.CopyToDataTable().AsEnumerable()
                                              where Convert.ToInt32(SalDetails.Field<string>("Sr No")) > iStartIndex && Convert.ToInt32(SalDetails.Field<string>("Sr No")) <= iEndIndex
                                              select SalDetails;

                        bool bIsFound = false;
                        foreach (DataRow dr in SortedSalaryDetails)
                        {
                            bIsFound = true;
                            break;
                        }
                        if (bIsFound)
                            oDTSalaryDetails = SortedSalaryDetails.CopyToDataTable();
                        else
                            oDTSalaryDetails = new DataTable();
                    }
                    else
                        oDTSalaryDetails = new DataTable();
                }
            }
        }

        lblSalaryDifferenceMessage.Text = string.Empty;
        trSalaryDifferenceMessage.Visible = false;

        DataTable oDataTable = null;
        if (oDSSalaryDetails != null && oDSSalaryDetails.Tables.Count > 0)
        {
            if (oDSSalaryDetails.Tables.Count > 1)
            {
                if (oDTSalaryDetails.Columns.Count != 1)
                {
                    ViewState.Add("SalaryDetails", oDTSalaryDetails);
                    oDataTable = oDTSalaryDetails;

                    DataTable oDTMonthAndYear = oDSSalaryDetails.Tables[I_MONTH_YEAR_TABLE_INDEX];
                    if (oDTMonthAndYear.IsNonEmpty())
                    {
                        int iMonthId = Convert.ToInt32(oDTMonthAndYear.Rows[0]["MonthId"]) + 1;
                        int iYear = Convert.ToInt32(oDTMonthAndYear.Rows[0]["Year"]);
                        msMonthList = oDTMonthAndYear.Rows[0]["MonthList"].ToString();
                        if (!string.IsNullOrEmpty(msMonthList))
                        {
                            lblSalaryDifferenceMessage.Text = "Including salary difference of month(s): " + msMonthList;
                            trSalaryDifferenceMessage.Visible = true;
                            hidMonthList.Value = msMonthList;
                        }

                        if (iMonthId == 13)
                        {
                            iMonthId = 1;
                            iYear = iYear + 1;
                        }

                        if (!(cmbMonths.SelectedValue == iMonthId.ToString() && cmbYear.SelectedValue == iYear.ToString()) && iYear != 0)
                        {
                            grdSalaryDetails.Enabled = false;
                            btnSave.Visible = false;
                        }
                        else
                        {
                            grdSalaryDetails.Enabled = true;
                            btnSave.Visible = true;
                        }
                    }
                    else
                    {
                        grdSalaryDetails.Enabled = true;
                        btnSave.Visible = true;
                    }

                    trSalaryMessage.Visible = false;
                    btnUnpublish.Visible = false;
                    hidIsStaticOutput.Value = Constants.S_NO;
                }
                else
                {
                    if (oDTSalaryDetails.IsNonEmpty())
                    {
                        grdSalaryDetails.Enabled = true;
                        oDataTable = GetPaidSalaryTable(oDTSalaryDetails, oDataTable);

                        if (oDataTable.IsNonEmpty())
                            oDataTable = GetPaidSalaryDetails(aiStaffGroupId, iStartIndex, iEndIndex, oDataTable);

                        oDataTable.Columns.Add(new DataColumn { ColumnName = S_ISDELETED, DefaultValue = "0" });
                        //oSalaryDetails.SalaryEntityLists.lstStaticSalaryDetails.ForEach
                        //    (
                        //        user =>
                        //        {
                        //            if (user.UserId > 0)
                        //            {
                        //                var dr = oDataTable.AsEnumerable().Where(dt => dt.Field<string>("UserId") == user.UserId.ToString());
                        //                if (dr.Count() > 0)
                        //                    dr.FirstOrDefault()[S_ISDELETED] = user.Is_Deleted;
                        //            }                                    
                        //        }
                        //    );

                        trSalaryMessage.Visible = true;

                        hidIsNextMonthAttendAvail.Value = Constants.S_NO;
                        if (oDSSalaryDetails.Tables[I_UNPUBLSH_STATUS] != null)
                        {
                            DataTable oDTAllowUnpublish = oDSSalaryDetails.Tables[I_UNPUBLSH_STATUS];
                            if (oDTAllowUnpublish != null && oDTAllowUnpublish.Rows.Count > 0 && oDTAllowUnpublish.Rows[0][0] != DBNull.Value)
                            {
                                string sUnpublish = Constants.S_NO;
                                string sIsNextMonthAttendanceAvailable = Constants.S_NO;

                                sUnpublish = oDTAllowUnpublish.Rows[0]["AllowUnpublish"].ToString();
                                sIsNextMonthAttendanceAvailable = oDTAllowUnpublish.Rows[0]["IsNextMonthAttendanceAvailable"].ToString();

                                msMonthList = oDTAllowUnpublish.Rows[0]["MonthList"].ToString();
                                if (!string.IsNullOrEmpty(msMonthList))
                                {
                                    lblSalaryDifferenceMessage.Text = String.Format("Including salary difference of month(s): {0}", msMonthList);
                                    trSalaryDifferenceMessage.Visible = true;
                                    hidMonthList.Value = msMonthList;
                                }

                                if (sUnpublish == Constants.S_YES)
                                {
                                    btnUnpublish.Visible = true;
                                    if (sIsNextMonthAttendanceAvailable == Constants.S_YES)
                                    {
                                        btnUnpublish.Enabled = false;
                                        hidIsNextMonthAttendAvail.Value = Constants.S_YES;
                                        trUnpublishSalaryNote.Visible = true;
                                    }
                                }
                                else
                                    btnUnpublish.Visible = false;

                            }
                        }
                    }
                }
            }
            
            if (oSalaryDetails.IsInvalidLeaveExists)
            {
                trConfigMessage.Visible = true;
                mbShowConfigMessage = true;
                lblConfigMessage.Text = S_NEGATIVE_LEAVE_BALANCE_MESSAGE;
            }
        }
        return oDataTable;
    }

    /// <summary>
    /// This method is used to set total pages.
    /// </summary>
    /// <param name="aiTotalRows"></param>
    private void SetTotalPages(int aiTotalRows)
    {
        if (aiTotalRows == Constants.I_GRID_PAGE_COUNT)
            miTotalPages = 1;
        else if (aiTotalRows % Constants.I_GRID_PAGE_COUNT == 0)
            miTotalPages = aiTotalRows / Constants.I_GRID_PAGE_COUNT;
        else
            miTotalPages = (aiTotalRows / Constants.I_GRID_PAGE_COUNT) + 1;
    }

    /// <summary>
    /// This method is used to return paid salary details.
    /// </summary>
    /// <param name="aiStaffGroupId"></param>
    /// <param name="iStartIndex"></param>
    /// <param name="iEndIndex"></param>
    /// <param name="oDataTable"></param>
    /// <returns></returns>
    private DataTable GetPaidSalaryDetails(int aiStaffGroupId, int iStartIndex, int iEndIndex, DataTable oDataTable)
    {
        IEnumerable<DataRow> SortedSalaryDetails2;
        if (aiStaffGroupId != 0)
        {
            if (string.IsNullOrEmpty(hidSalaryFilter.Value))
            {
                SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                       where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                       select SalDetails;
            }
            else
            {
                SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                       where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                        && SalDetails.Field<string>("Name").ToLower().Contains(hidSalaryFilter.Value.ToLower())
                                       select SalDetails;
            }
        }
        else
        {
            SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                   where SalDetails.Field<string>("Name").ToLower().Contains(hidSalaryFilter.Value.ToLower())
                                   select SalDetails;
        }

        int iTotalRows = 0;
        string sColumnName = "Sr_No";
        if (SortedSalaryDetails2.Any() && SortedSalaryDetails2.CopyToDataTable().Columns.Contains("SrNo"))
            sColumnName = "SrNo";

        foreach (DataRow dr in SortedSalaryDetails2)
        {
            dr[sColumnName] = ++iTotalRows;
            dr["Name"] = dr["Name"].ToString().Replace("''", "'");
        }

        if (iTotalRows != 0)
        {   
            SetTotalPages(iTotalRows);
            miTotalRecords = iTotalRows;
            if (mbIsExportAll)
            {
                iStartIndex = Constants.I_ZERO;
                iEndIndex = Constants.I_DEFAULT_MAX_VALUE;
            }
            SortedSalaryDetails2 = from SalDetails in SortedSalaryDetails2.CopyToDataTable().AsEnumerable()
                                   where Convert.ToInt32(SalDetails.Field<string>(sColumnName)) > iStartIndex && Convert.ToInt32(SalDetails.Field<string>(sColumnName)) <= iEndIndex
                                   select SalDetails;

            bool bIsFound = false;
            foreach (DataRow dr in SortedSalaryDetails2)
            {
                bIsFound = true;
                break;
            }
            if (bIsFound)
                oDataTable = SortedSalaryDetails2.CopyToDataTable();
            else
                oDataTable = new DataTable();
        }
        else
            oDataTable = new DataTable();
        return oDataTable;
    }

    /// <summary>
    /// This method is used to return paid salary details.
    /// </summary>
    /// <param name="oDTSalaryDetails"></param>
    /// <param name="oDataTable"></param>
    /// <returns></returns>
    private DataTable GetPaidSalaryTable(DataTable oDTSalaryDetails, DataTable oDataTable)
    {
        string sXml = oDTSalaryDetails.Rows[0][0].ToString();
        hidIsStaticOutput.Value = Constants.S_YES;
        sXml = sXml.Replace("<SalaryDetails>", string.Empty);
        sXml = sXml.Replace("</SalaryDetails>", string.Empty);
        sXml = sXml.Replace("<SalaryDetails ", "<SalaryDetailsXml ");

        DataSet oDataSet = new DataSet();
        using (System.IO.StringReader oReader = new System.IO.StringReader(sXml))
            oDataSet.ReadXml(oReader);

        oDataTable = oDataSet.Tables[I_SALARY_DETAILS];
        return oDataTable;
    }

    /// <summary>
    /// This method is used to display yearwise leaves configuration message.
    /// </summary>
    private void DisplayLeavesConfigMsg()
    {
        if (mbShowConfigMessage && hidIsStaticOutput.Value == Constants.S_NO)
            trConfigMessage.Visible = true;
        else
            trConfigMessage.Visible = false;
    }

    /// <summary>
    /// This method is used to remove unwanted cells.
    /// </summary>
    /// <param name="e"></param>
    private void RemoveCells(GridViewRowEventArgs e)
    {
        for (int iCellIndex = 1; iCellIndex < e.Row.Cells.Count - 2; iCellIndex++)
        {
            if ((e.Row.Cells[iCellIndex].Text == null ||
                e.Row.Cells[iCellIndex].Text == Constants.S_ZERO ||
                e.Row.Cells[iCellIndex].Text == "0.0") &&
                e.Row.BackColor != Color.LightGray)
                e.Row.Cells[iCellIndex].Text = string.Empty;
        }
        e.Row.Cells[9].Text = string.Empty;
    }

    /// <summary>
    /// This method is used to generate salary details xml according to user.
    /// </summary>
    /// <returns></returns>
    private string GenerateSalaryDetailsXml()
    {
        DataTable oDataTable = (DataTable)ViewState["SalaryDetails"];
        int iRowCount = oDataTable.Rows.Count;

        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SalaryDetailsXml");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetailsXml", string.Empty);

        // Loop through all the grid rows.
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {

            if (oDataTable.Rows[iRowIndex]["DisplayControls"].ToString() == Constants.S_NO)
                continue;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetailsXml", string.Empty);

            sAttribute = "UserId";
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = oDataTable.Rows[iRowIndex]["UserId"].ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "IndividualXml";
            attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = GenerateIndividualXml(iRowIndex);
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
    /// This method is used to set screen width.
    /// </summary>
    private void SetScreenWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str);
            iWidth = iWidth / 100 * 80;
            divContainer.Style.Add("width", iWidth.ToString() + "px !important");
            tblLegent.Style.Add("width", iWidth.ToString() + "px !important");
            tblNote.Style.Add("width", iWidth.ToString() + "px !important");
        }
        else
            divContainer.Style.Add("width", Convert.ToString(1024) + "px !important");
    }

    /// <summary>
    /// This method is used to populate salary details.
    /// </summary>
    /// <returns></returns>
    private SalaryDetails PopulateSalaryDetails()
    {
        SalaryDetails oSalaryDetails = new SalaryDetails
        {
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = miUserId,
            MonthId = Convert.ToInt32(cmbMonths.SelectedValue),
            Year = Convert.ToInt32(cmbYear.SelectedValue),
            SalaryDetailsXml = GenerateSalaryDetailsXml()
        };

        return oSalaryDetails;
    }

    /// <summary>
    /// This method is used to populate individuals salary details.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private SalaryDetails PopulateIndividualSalaryDetailsBL(int aiRowIndex)
    {
        SalaryDetails oSalaryDetails = new SalaryDetails
        {
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = miUserId,
            UserId = Convert.ToInt32(grdSalaryDetails.Rows[aiRowIndex].Cells[5].Text),
            MonthId = Convert.ToInt32(cmbMonths.SelectedValue),
            Year = Convert.ToInt32(cmbYear.SelectedValue),
            IndividualXml = GenerateIndividualXml(aiRowIndex)
        };
        return oSalaryDetails;
    }

    /// <summary>
    /// This method is used to generate idividuals xaml.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private string GenerateIndividualXml(int aiRowIndex)
    {
        const string S_ELEMENT = "element";
        string sControlId = string.Empty;
        string sValues = string.Empty;
        string sType = string.Empty;

        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("SalaryDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetails", string.Empty);

        for (int iColumnnIndex = 0; iColumnnIndex < grdSalaryDetails.Rows[aiRowIndex].Cells.Count - 2; iColumnnIndex++)
        {
            Control oControl = null;
            if (grdSalaryDetails.Rows[aiRowIndex].Cells[iColumnnIndex].Controls.Count > 0)
                oControl = grdSalaryDetails.Rows[aiRowIndex].Cells[iColumnnIndex].Controls[0];
            if (oControl is TextBox && oControl.Visible)
            {
                TextBox txtSalary = (TextBox)oControl;
                sControlId = txtSalary.ID;

                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetails", string.Empty);

                XmlAttribute attr = oDoc.CreateAttribute("Type");
                sValues = sControlId.Substring(sControlId.IndexOf("_") + 1);
                attr.Value = sValues.Substring(0, 2);
                sType = attr.Value;
                oXmlNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("ControlId");
                if (sType == S_ATTENDANCE || sType == S_LEAVE)
                    attr.Value = sValues.Substring(sValues.IndexOf("_") + 1);
                else
                    attr.Value = sValues.Substring(sValues.IndexOf("_") + 1, (sValues.LastIndexOf("_") - sValues.IndexOf("_")) - 1);
                oXmlNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("ControlValue");
                attr.Value = txtSalary.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to set hide buttons.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetControlVisibility(bool abAction)
    {
        if (abAction)
        {
            trSalaryMessage.Visible = false;
            btnUnpublish.Visible = false;
            btnSave.Visible = false;
            trConfigMessage.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.SalaryDetails);

        if (!sLinks.Equals(String.Empty))
        {
            divErr.InnerHtml = sLinks;
            trSalaryDetails.Visible = false;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to update headers.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private static void UpdateHeaders(DataTable aoDataTable)
    {
        for (int iInd = 0; iInd < aoDataTable.Columns.Count; iInd++)
            aoDataTable.Columns[iInd].ColumnName = "<B>" + aoDataTable.Columns[iInd].ColumnName + "</B>";
    }

    /// <summary>
    /// This method is sued to update serial numbers.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private static void UpdateSerialNumbers(DataTable aoDataTable)
    {
        int iSrNo = 1;
        for (int iInd = 0; iInd < aoDataTable.Rows.Count; iInd++)
            aoDataTable.Rows[iInd][0] = iSrNo++;
    }

    /// <summary>
    /// This method is used to remove summary rows.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private static void RemoveSummaryRows(DataTable aoDataTable)
    {
        DataRow[] dr = aoDataTable.Select("Name Like '%Faculty Total%' OR Name Like '%Administrative Staff Total%' OR Name Like '%Consolidated Staff Total%'");
        if (dr.Length > 0)
        {
            for (int iInd = 0; iInd < dr.Length; iInd++)
                dr[iInd].Delete();
        }
    }

    /// <summary>
    /// This method is used to remove unnecessary columns.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private void RemoveUnnecessaryColumns(DataTable aoDataTable)
    {
        int iStartIndex = aoDataTable.Columns.IndexOf("Attendance");
        int iEndIndex = aoDataTable.Columns.IndexOf("Total");

        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            for (int iIndex = iEndIndex; iIndex > iStartIndex; iIndex--)
                aoDataTable.Columns.RemoveAt(iIndex);
        }
        else
        {
            for (int iIndex = iEndIndex-1; iIndex >= iStartIndex; iIndex--)
                aoDataTable.Columns.RemoveAt(iIndex);
        }

        if (aoDataTable.Columns.Contains(S_ISDELETED))
            aoDataTable.Columns.Remove(S_ISDELETED);

        if (aoDataTable.Columns.Contains(S_FORM_NO_16))
            aoDataTable.Columns.Remove(S_FORM_NO_16);
    }

    /// <summary>
    /// This method is used to add /remove columns.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private static void AddRemoveColumns(DataTable aoDataTable)
    {
        if (aoDataTable.Columns.Contains("Total Deduction"))
            aoDataTable.Columns.Remove("Total Deduction");

        if (aoDataTable.Columns.Contains("Net Salary"))
            aoDataTable.Columns.Remove("Net Salary");

        if (aoDataTable.Columns.Contains("Salary Difference of Deduction"))
            aoDataTable.Columns.Remove("Salary Difference of Deduction");

        if (aoDataTable.Columns.Contains("Total"))
            aoDataTable.Columns["Total"].ColumnName = "Attendance";

        aoDataTable.Columns.Add("Reason for Increase / Decrease");
    }

    /// <summary>
    /// This method is used to remove attendance dependent EDs.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private void RemoveAttendanceDependentEDs(DataTable aoDataTable)
    {
        List<EarningsDeductions> lstEarningDeductions = EarningsDeductionsBL.GetAll(miSchoolId);

        lstEarningDeductions = lstEarningDeductions.Where(ed => ed.SchoolId == miSchoolId).ToList();

        lstEarningDeductions.ForEach
            (
                ed =>
                {
                    if (!ed.IsEarning && aoDataTable.Columns.Contains(ed.ShortName))
                        aoDataTable.Columns.Remove(ed.ShortName);

                    if (ed.IsAttendanceDependent && aoDataTable.Columns.Contains(ed.ShortName))
                    {
                        aoDataTable.Columns.Remove(ed.ShortName);
                        aoDataTable.Columns["Leave Deducted " + ed.ShortName].ColumnName = ed.ShortName;
                    }
                }
            );
    }

    #endregion   
    
}