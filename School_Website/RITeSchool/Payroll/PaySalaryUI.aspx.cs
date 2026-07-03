/* Created By :- Sachin
*  Created Date :- 10-Nov-2009
*  Class Description :- This class is used to publish salary.
 *  
 * Modified By - Sachin
 * Modified Date - 24-May-2012
 * Modification reason - To provide faciity of paging and code cleaning.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using PayrollEntities;
using Utility;
using SchoolEntities.Admin;

public partial class PaySalaryUI : ExportDataTable
{
    #region Constants

    private const string S_EMPTY_TABLE = "S_SALARY_DETAILS";    
    private const int I_NAME_COLUMN_INDEX = 7;
    private const int I_DESIGNATION_COLUMN_INDEX = 8;    
    private const int I_CACHE_TIMEOUT = 1200;
    private const string S_TRUE = "True";
    const string S_ISDELETED = "IsDeleted";
    private SchoolwiseBankAccountDetailsBL moSchoolwiseBankAccountDetailsBL;

    #endregion

    #region Members

    private DataTable moEmptySalaryTable = new DataTable();
    private string msColumnNumbers = "1,2,3,4,5,6,";
    private int miTotalRecords = 0;
    private int miTotalPages = 0;
    private SalaryDetailsBL moSalaryDetailsBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to display month and year,set screen width and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                DisplayMonthAndYear();
                if (!CheckPreCondition())
                    SetPreconditionView(false);
                else
                {
                    SetPreconditionView(true);                    
                    SetScreenWidth();
                }
                SetDefaultValues();
                FillBankCombo();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save salary details of all the users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizSalaryInformation_PublishPuttonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            SaveSalaryDetails(true);
            if (Session[PayrollConstants.S_SALARY_DETAILS] != null)
                Session.Remove(PayrollConstants.S_SALARY_DETAILS);
        }
        catch (SalaryPublishException oSalaryPublishException)
        {
            lblInvalidLeaveMEssage.Text = oSalaryPublishException.Message;
            Button btnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
            btnFinish.Visible = false;
            trInvalidLeaveMessage.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill salary details grid according to selected year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbMonths_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            SetPreconditionMessage(!CheckPTChallanDetailsExists());
            CheckComboValue();            
        }
        catch (SalaryPublishException oSalaryPublishException)
        {
            lblInvalidLeaveMEssage.Text = oSalaryPublishException.Message;
            Button btnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
            btnFinish.Visible = false;
            trInvalidLeaveMessage.Visible = true;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill account no  combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillBankAccountCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to display salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizSalaryInformation_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            if (hidIsCurrentMonth.Value == S_TRUE)
            {   
                HideControls(false);
                FillComboboxes();
            }
            else
                HideControls(true);

            FillSalaryDetailsGrid(true);            
            ViewState.Add(S_EMPTY_TABLE, moEmptySalaryTable);
            DisableControls(false);
            ValidateLeaves();
        }
        catch (SalaryPublishException oSalaryPublishException)
        {
            lblInvalidLeaveMEssage.Text = oSalaryPublishException.Message;
            Button btnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
            btnFinish.Visible = false;
            trInvalidLeaveMessage.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            e.Cancel = true;
        }
    }

    /// <summary>
    /// This event is used to format cells.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPaySalary_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        moEmptySalaryTable.Columns.Add(cell.Text);
                    cell.Wrap = false;

                    cell.Style.Add("padding-left", "5");
                    cell.Style.Add("padding-right", "5");

                    switch (iColumnIndex)
                    {
                        case 0: cell.HorizontalAlign = HorizontalAlign.Center;
                            break;
                        case I_NAME_COLUMN_INDEX:
                        case I_DESIGNATION_COLUMN_INDEX: cell.HorizontalAlign = HorizontalAlign.Left;
                            break;
                        default: cell.HorizontalAlign = HorizontalAlign.Right;
                            break;
                    }

                    cell.CssClass = "GridDate";
                    if (cell.Text == PayrollConstants.S_TOTAL || cell.Text == PayrollConstants.S_GROSS_SALARY || cell.Text == PayrollConstants.S_TOTAL_DEDUCTION || cell.Text == PayrollConstants.S_NET_SALARY)
                        hidColumnIndexes.Value = hidColumnIndexes.Value + "[" + iColumnIndex + "]";

                    if (cell.Text == PayrollConstants.S_SALARY_DIFFERENCE)
                        hidSalaryDifferenceColumnIndex.Value = iColumnIndex.ToString();
                    if (cell.Text == S_ISDELETED)
                        cell.Visible = false;

                    iColumnIndex++;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HideCells(e.Row);
                FormatCells(e);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This button is used to navigate to contol panel on click of cancel button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizSalaryInformation_CancelButtonClick(object sender, EventArgs e)
    {
        try
        {
            if (Session[PayrollConstants.S_SALARY_DETAILS] != null)
                Session.Remove(PayrollConstants.S_SALARY_DETAILS);

            MasterPage oMasterPage = (MasterPage)this.Master;
            if (oMasterPage != null) 
                oMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to navigate back to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizSalaryInformation_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            Button oBtnNext = (Button)wizSalaryInformation.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
            oBtnNext.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizSalaryInformation_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            chkSendSMS.Attributes.Add("onclick", "document.getElementById(_clientchkSendDetailSMS).checked = false;");
            chkSendDetailSMS.Attributes.Add("onclick", "document.getElementById(_clientchkSendSMS).checked = false;");

            if (wizSalaryInformation.ActiveStep == WizardStep1 && wizSalaryInformation.FindControl("StartNavigationTemplateContainerID") != null)
                SetJavascriptAttributes();
            else if (wizSalaryInformation.ActiveStep == WizardStep2)
            {   
                string sSalaryDetailsSmsText = GetSMSTemplate(Convert.ToInt32(Constants.SMSTemplate.SalarySMS));
                string sCurrentMonth = hidMonthName.Value;
                sSalaryDetailsSmsText = sSalaryDetailsSmsText.Replace("%MONTH%", sCurrentMonth).Replace("%AMOUNT%", "<AMOUNT>").Trim().Replace("'", "\\'").Replace("\"", "\\'\\'");
                Button oBtnFinish = (Button)wizSalaryInformation.FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
                oBtnFinish.Attributes.Add("onclick", "javascript:disableControls();");                
                oBtnFinish.Attributes.Add("onclick", "if(!DisplayConfirmation('" + sSalaryDetailsSmsText + "')) return false;");

                Button btnCancel = (Button)wizSalaryInformation.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton");
                if (!btnCancel.IsNull())
                {
                    btnCancel.Attributes["onmouseover"] = "javascript:fnover('" + btnCancel.ClientID + "');";
                    btnCancel.Attributes["onmouseout"] = "javascript:fnout('" + btnCancel.ClientID + "');";
                }

                Button btnPrevious = (Button)wizSalaryInformation.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
                if (!btnPrevious.IsNull())
                {
                    btnPrevious.Attributes["onmouseover"] = "javascript:fnover('" + btnPrevious.ClientID + "');";
                    btnPrevious.Attributes["onmouseout"] = "javascript:fnout('" + btnPrevious.ClientID + "');";
                }

                ApplyMouseHoverEffect(new List<Button> { btnExport, btnSalarySlipPreview, oBtnFinish, btnRefresh, btnCancel, btnPrevious });
            }
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
            DataTable oDataTable = null;
            if (ViewState[S_EMPTY_TABLE] != null)
                oDataTable = (DataTable)ViewState[S_EMPTY_TABLE];

            DataTable oDtTotalSalaryDetails = new DataTable();
            if (Session[PayrollConstants.S_SALARY_DETAILS] != null)
                oDtTotalSalaryDetails = (DataTable)Session[PayrollConstants.S_SALARY_DETAILS];

            if (oDataTable.Columns.Contains(S_ISDELETED))
                oDataTable.Columns.Remove(S_ISDELETED);

            int iColumnCount = oDtTotalSalaryDetails.Columns.Count - 1;
            int iRowCount = oDtTotalSalaryDetails.Rows.Count;
            string sValue = string.Empty;
            int iIndex = 0;
            int iNewCellIndex;
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                iNewCellIndex = 1;
                DataRow oDataRow = oDataTable.NewRow();
                oDataRow[0] = iRowIndex + 1;
                for (int iCellIndex = 0; iCellIndex < iColumnCount; iCellIndex++)
                {
                    if (iCellIndex > 6)
                    {
                        sValue = oDtTotalSalaryDetails.Rows[iRowIndex][iCellIndex].ToString();
                        iIndex = sValue.IndexOf("_");
                        if (iIndex >= 0)
                            sValue = sValue.Substring(0, iIndex);

                        if (sValue == "-1" || sValue == "-1.00")
                            sValue = string.Empty;
                        oDataRow[iNewCellIndex++] = sValue;
                    }
                }

                oDataTable.Rows.Add(oDataRow);
            }

            if (hidMonthList.Value != string.Empty)
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
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to preview salary slip of selected month and year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSalarySlipPreview_Click(object sender, EventArgs e)
    {
        try
        {
            SaveSalaryDetails(false);
            DisplayReport();
        }
        catch (ThreadAbortException)
        {
            DeleteSalary();
        }
        catch (Exception ex)
        {
            DeleteSalary();
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
            wizSalaryInformation_NextButtonClick(sender, null);            
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
            grdPaySalary.PageIndex = PageDropDownList.SelectedIndex;
            lblCurrentPage.Text = "Page " + PageDropDownList.SelectedValue + "  of " + PageDropDownList.Items.Count;
            FillSalaryDetailsGrid(false);

            int iLastIndex = Convert.ToInt32(PageDropDownList.SelectedValue) * Constants.I_GRID_PAGE_COUNT;
            lblStartIndex.Text = ((Convert.ToInt32(PageDropDownList.SelectedValue) - 1) * Constants.I_GRID_PAGE_COUNT + 1).ToString();
            lblEndIndex.Text = iLastIndex < miTotalRecords ? iLastIndex.ToString() : miTotalRecords.ToString();
            lblTotalRecords.Text = miTotalRecords.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set field label according to cheque payment type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optCheque_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabelAccordingToPaymentType(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set field label according to online payment type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optOnline_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabelAccordingToPaymentType(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This mthod is used to check precondition.
    /// </summary>
    private void ValidateLeaves()
    {
        DataTable oDataTable = (DataTable)Session[PayrollConstants.S_SALARY_DETAILS];
        var negativeSalary = from row in oDataTable.AsEnumerable()
                                                          where Convert.ToDecimal(row.Field<string>(PayrollConstants.S_NET_SALARY)) < 0
                                                          select row;
        string sMessage = string.Empty;
        string sConnector = string.Empty;
        if (hidInvalidLeaves.Value != string.Empty && Convert.ToString(hidInvalidLeaves.Value) == "Y")
        {
            sMessage = sMessage + sConnector + "Leave balance should not be negative (marked in red), please re-configure yearwise leaves of respective user to publish salary.";
            sConnector = "<br />";
        }

        if (negativeSalary.Count() > 0)
        {
            sMessage = sMessage + sConnector + "Net Salary of the users should not be negative (marked in red).";
            sConnector = "<br />";
        }

        if (moSalaryDetailsBL.NonLeaveConfiguredUsers.Count > 0)
        {
            string sUsers = string.Join(", ", moSalaryDetailsBL.NonLeaveConfiguredUsers);
            sMessage = sMessage + sConnector + "Leaves are not configured for the user(s) : " + sUsers + ".";
            sConnector = "<br />";
        }

        if (moSalaryDetailsBL.NonAttendanceUsers.Count > 0)
        {
            string sUsers = string.Join(", ", moSalaryDetailsBL.NonAttendanceUsers);
            sMessage = sMessage + sConnector + "Attendance is not marked for the user(s) : " + sUsers + ".";
            sConnector = "<br />";
        }
        
        if (moSalaryDetailsBL.NonEarnDeductConfiguredUsers.Count > 0)
        {
            string sUsers = string.Join(", ", moSalaryDetailsBL.NonEarnDeductConfiguredUsers);
            sMessage = sMessage + sConnector + "Earnings and deductions are not configured for the user(s) : " + sUsers + ".";
        }

        int iDaysOfMonth = DateTime.DaysInMonth(hidYear.Value.ToInt(), hidMonthId.Value.ToInt());

        var oTotalDays = from row in oDataTable.AsEnumerable()
                          where Convert.ToDecimal(row.Field<string>("Total")) > iDaysOfMonth
                          && row.Field<string>("UserId") != "-9999"
                          select row;

        if (oTotalDays != null && oTotalDays.Count() > 0)
        {
            var oUserNames = oTotalDays.AsEnumerable().Select(dr => dr.Field<string>("Name")).ToList();
            string sUserList = string.Join(", ", oUserNames);
            sMessage = sMessage + sConnector + "Total Working Days(Total) should not be more than total days of that month for user(s) : " + sUserList;
            sConnector = "<br />";
        }
        
        if (sMessage != string.Empty)
            throw new SalaryPublishException(sMessage);
    }

    /// <summary>
    /// This method is used to fill bank combobox.
    /// </summary>
    private void FillBankCombo()
    {   
        moSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();        
        List<SchoolWiseBankAccountDetails> lstSchoolWiseBankAccountDetails = moSchoolwiseBankAccountDetailsBL.GetSchoolwiseBankList(miSchoolId);
        ListSource.FillDropDownList(lstSchoolWiseBankAccountDetails, cmbBankName, "BankName", "BankId", Constants.S_SELECT);
        cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
    }

    /// <summary>
    /// This method is used to fill bank account combobox.
    /// </summary>
    private void FillBankAccountCombo()
    {   
        int iBankId = Convert.ToInt32(cmbBankName.SelectedValue);
        if (iBankId != 0)
        {   
            moSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();
            List<SchoolWiseBankAccountDetails> lstSchoolWiseBankAccountDetails = moSchoolwiseBankAccountDetailsBL.GetBankwiseAccountList(miSchoolId, iBankId);
            cmbAccountNo.Items.Clear();
            cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            lstSchoolWiseBankAccountDetails.ForEach(account => cmbAccountNo.Items.Add(new ListItem(Convert.ToString(account.AccountNo), Convert.ToString(account.SchoolWiseBankAccountDetailsId))));
        }
        else
        {
            cmbAccountNo.Items.Clear();
            cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
        }
    }

    /// <summary>
    /// This method is used to fetch salary SMS template.
    /// </summary>
    /// <param name="aiSmsId"></param>
    /// <returns></returns>
    private string GetSMSTemplate(int aiSmsId)
    {
        string sSalaryDetailsSmsText = string.Empty;        
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(aiSmsId, miSchoolId);
        if (oDTTemplate.IsNonEmpty())
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
                sSalaryDetailsSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);
        }

        return sSalaryDetailsSmsText;
    }

    /// <summary>
    /// This method is used to hide controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideControls(bool abAction)
    {
        lblYearAndMonth.Text = abAction ? hidMonthAndYear.Value : string.Empty;
        trCombobox.Visible = hidIsCurrentMonth.Value == S_TRUE;        
        trMonthAndYear.Visible = abAction;
    }

    /// <summary>
    /// This method is used to check values of comboboxes and fill salary grid if all are selected.
    /// </summary>
    private void CheckComboValue()
    {
        Button oBtnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
        if (cmbMonths.SelectedValue != Constants.S_ZERO && cmbYear.SelectedValue != Constants.S_ZERO)
        {
            hidMonthId.Value = cmbMonths.SelectedValue;
            hidYear.Value = cmbYear.SelectedValue;
            HideControlsAccordingToCombo(false, oBtnFinish);
            lblErr.Text = string.Empty;

            FillSalaryDetailsGrid(true);
            ViewState.Add(S_EMPTY_TABLE, moEmptySalaryTable);
            ValidateLeaves();
        }
        else
            HideControlsAccordingToCombo(true, oBtnFinish);
    }

    /// <summary>
    /// This method is used to display month and year of salary.
    /// </summary>
    private void DisplayMonthAndYear()
    {
        DataTable oDTYearAndMonth = null;
        DataTable oDTPDetails = null;

        DataSet oDSYearAndMonth = SalaryDetailsBL.GetSalaryMonthAndYear(miSchoolId, miAcademicYearId);
        if (oDSYearAndMonth != null && oDSYearAndMonth.Tables.Count > 0)
        {
            oDTYearAndMonth = oDSYearAndMonth.Tables[0];
            oDTPDetails = oDSYearAndMonth.Tables[1];
        }

        if (oDTYearAndMonth != null && oDTYearAndMonth.Rows.Count > 0 && oDTYearAndMonth.Rows[0][0] != DBNull.Value)
        {
            hidMonthAndYear.Value = oDTYearAndMonth.Rows[0][0].ToString();
            hidIsCurrentMonth.Value = oDTYearAndMonth.Rows[0]["IsCurrentMonth"].ToString();
            hidMonthId.Value = oDTYearAndMonth.Rows[0]["Month"].ToString();
            hidMonthName.Value = oDTYearAndMonth.Rows[0]["MonthName"].ToString();
            hidYear.Value = oDTYearAndMonth.Rows[0]["Year"].ToString();
            hidIsLeaveAccumulationInterval.Value = oDTYearAndMonth.Rows[0]["IsLeaveAccumulationInterval"].ToString();
        }

        if (oDTPDetails != null && oDTPDetails.Rows.Count > 0 && oDTPDetails.Rows[0][0] != DBNull.Value)
        {
            hidPTDetails.Value = Convert.ToString(oDTPDetails.Rows[0]["PTDetails"]);
            hidIsMidYear.Value = Convert.ToString(oDTPDetails.Rows[0]["IsMidYear"]);
        }
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        int iCount = chklstItems.Items.Count;
        Button oBtnNext = (Button)wizSalaryInformation.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
        chklstItems.Attributes.Add("onclick", "NextEnable(" + iCount + " , '" + oBtnNext.ClientID + "')");        
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumShow.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidLeaveTransferMonth.Value = Settings.LeaveTransferMonth.ToString();
        trLeaveConfigMessage.Visible = hidIsLeaveAccumulationInterval.Value == Constants.S_YES || hidLeaveTransferMonth.Value == hidMonthId.Value;
        SetJavascriptAttributes();
        hidQueryString.Value = CommonUtility.EncryptQuerystring("ReportFolderId=" + Constants.ReportFolders.Payroll.ToInt());
        lnkUserReports.Attributes.Add("onclick", "OpenPopup(); return false;");

        if (miSchoolId == Constants.SchoolId.PPS.ToInt())
        {
            optOnline.Checked = true;
            optOnline_CheckedChanged(optOnline, null);
        }
        else
        {
            optCheque.Checked = true;
            optCheque_CheckedChanged(optCheque, null);
        }        
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        Button oButton = (Button)wizSalaryInformation.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton");
        trChequeNo.Visible = false;
        trButtons.Visible = false;

        if (oButton != null)
        {
            oButton.Attributes["onmouseover"] = "javascript:fnover('" + oButton.ClientID + "');";
            oButton.Attributes["onmouseout"] = "javascript:fnout('" + oButton.ClientID + "');";
        }

        oButton = (Button)wizSalaryInformation.FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
        if (oButton != null)
        {
            oButton.Attributes["onmouseover"] = "javascript:fnover('" + oButton.ClientID + "');";
            oButton.Attributes["onmouseout"] = "javascript:fnout('" + oButton.ClientID + "');";
        }
    }

    /// <summary>
    /// This method is used to hide controls according to selection of comboboxes.
    /// </summary>
    /// <param name="abAction"></param>
    /// <param name="oBtnFinish"></param>
    private void HideControlsAccordingToCombo(bool abAction, Button oBtnFinish)
    {
        divContainer.Visible = !abAction;
        btnExport.Visible = !abAction;
        btnSalarySlipPreview.Visible = !abAction;
        oBtnFinish.Enabled = !abAction;
    }

    /// <summary>
    /// This method is used to fill all month and year comboboxes.
    /// </summary>
    private void FillComboboxes()
    {   
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);

        ListSource.FillDropDownList(oSalaryDetailsBL.Months, cmbMonths, "Month", "MonthId", string.Empty);
        ListSource.FillDropDownList(oSalaryDetailsBL.Years, cmbYear, "Year", "Year", string.Empty);

        if (!string.IsNullOrEmpty(hidMonthId.Value) && !string.IsNullOrEmpty(hidYear.Value))
        {
            cmbYear.FindByValue(hidYear.Value);
            cmbMonths.FindByValue(hidMonthId.Value);
        }
        else
        {
            cmbYear.SelectedValue = DateTime.Now.Year.ToString();
            cmbMonths.SelectedValue = DateTime.Now.Month.ToString();
        }
    }

    /// <summary>
    /// This method is used to fill salary details grid.
    /// </summary>
    private void FillSalaryDetailsGrid(bool abReloadGrid)
    {
        DataTable oDTSalaryDetails = GetSalaryDetails(abReloadGrid);
        if (oDTSalaryDetails.IsNonEmpty())
        {
            DataTable oDataTable = null;
            if (oDTSalaryDetails.Columns.Count != 1)
                oDataTable = oDTSalaryDetails;
            
            grdPaySalary.DataSource = oDataTable;
            grdPaySalary.DataBind();

            if (grdPaySalary.Rows.Count > 0)
            {
                divContainer.Visible = true;
                btnExport.Visible = true;
                btnSalarySlipPreview.Visible = true;
            }
            else
            {
                divContainer.Visible = false;
                btnExport.Visible = false;
            }

            DisplayConfigMessage(false);
            FillPagerDropdown();
        }
        else
            DisplayConfigMessage(true);
    }

    /// <summary>
    /// This method is used to return salary details datatable.
    /// </summary>
    /// <returns></returns>
    private DataTable GetSalaryDetails(bool abReloadGrid)
    {
        const int I_SALARY_DETAILS = 0;
        const int I_NET_SALARY_SUM = 1;
        const int I_MONTHLIST_TABLE_INDEX = 2;

        int iMonthId;
        int iYear;
        if (trCombobox.Visible)
        {
            iMonthId = Convert.ToInt32(cmbMonths.SelectedValue);
            iYear = Convert.ToInt32(cmbYear.SelectedValue);
        }
        else
        {
            iMonthId = Convert.ToInt32(hidMonthId.Value);
            iYear = Convert.ToInt32(hidYear.Value);
        }

        lblMessage.Text = string.Empty;        
        bool bIsCurrentMonth = Convert.ToBoolean(hidIsCurrentMonth.Value);

        moSalaryDetailsBL = new SalaryDetailsBL(miSchoolId, miAcademicYearId);
        moSalaryDetailsBL.CacheTimeout = I_CACHE_TIMEOUT;

        DataSet oDsSalaryDetails;

        if (abReloadGrid || Cache[PayrollConstants.S_SALARY_ENTITY_LIST] == null)
        {
            oDsSalaryDetails = moSalaryDetailsBL.GetSalaryDetailsDataset(iMonthId, iYear, 0, string.Empty, 0, 9999, true, true);
            Cache.Insert(PayrollConstants.S_SALARY_ENTITY_LIST, oDsSalaryDetails, null, DateTime.Now.AddSeconds(I_CACHE_TIMEOUT), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        else
            oDsSalaryDetails = Cache[PayrollConstants.S_SALARY_ENTITY_LIST] as DataSet;

        DataTable oDtSalaryDetails = new DataTable();
        int iStartIndex = grdPaySalary.PageIndex * Constants.I_GRID_PAGE_COUNT;
        int iEndIndex = iStartIndex + Constants.I_GRID_PAGE_COUNT;

        if (oDsSalaryDetails != null && oDsSalaryDetails.Tables.Count > 0)
        {
            oDtSalaryDetails = oDsSalaryDetails.Tables[I_SALARY_DETAILS];
            if (oDtSalaryDetails.IsNonEmpty())
            {
                Session.Add(PayrollConstants.S_SALARY_DETAILS, oDtSalaryDetails);

                int iTotalRows = 0;
                foreach (DataRow dr in oDtSalaryDetails.Rows)
                    dr["Sr No"] = ++iTotalRows;

                SetTotalPages(iTotalRows);

                miTotalRecords = iTotalRows;
                IEnumerable<DataRow> sortedSalaryDetails = from salDetails in oDtSalaryDetails.AsEnumerable()
                                                  where Convert.ToInt32(salDetails.Field<string>("Sr No")) > iStartIndex && Convert.ToInt32(salDetails.Field<string>("Sr No")) <= iEndIndex
                                                  select salDetails;
                oDtSalaryDetails = sortedSalaryDetails.CopyToDataTable();
            }
        }

        DataTable oDtNetSalary = oDsSalaryDetails.Tables[I_NET_SALARY_SUM];

        if (oDtNetSalary.IsNonEmpty())
        {
            txtAmount.Text = oDtNetSalary.Rows[0][0].ToString();
            hidNetSalarySum.Value = txtAmount.Text.Trim();
        }

        hidInvalidLeaves.Value = moSalaryDetailsBL.IsInvalidLeaveExists ? Constants.S_YES : Constants.S_NO;

        DataTable oDtMonthList = oDsSalaryDetails.Tables[I_MONTHLIST_TABLE_INDEX];
        lblSalaryDifferenceMessage.Visible = false;
        lblSalaryDifferenceMessage.Text = string.Empty;
        if (oDtMonthList.IsNonEmpty() && oDtMonthList.Rows[0]["MonthList"].ToString() != string.Empty)
        {
            hidMonthList.Value = oDtMonthList.Rows[0]["MonthList"].ToString();
            lblSalaryDifferenceMessage.Text = "Including salary difference of month(s): " + hidMonthList.Value;
            lblSalaryDifferenceMessage.Visible = true;
        }
        
        return oDtSalaryDetails;
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
    /// This method is used to display configuration message.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisplayConfigMessage(bool abAction)
    {
        trConfigMessage.Visible = abAction;
        trMonthAndYear.Visible = !abAction;
        trWizard.Visible = !abAction;
        trButtons.Visible = !abAction;
        trChequeNo.Visible = true;
    }

    /// <summary>
    /// This method is used to hide gridview cells.
    /// </summary>
    /// <param name="aogridViewRow"></param>
    private void HideCells(GridViewRow aogridViewRow)
    {
        for (int iCellIndex = 1; iCellIndex <= 6; iCellIndex++)
            aogridViewRow.Cells[iCellIndex].Visible = false;
    }

    /// <summary>
    /// This method is used to format cella.
    /// </summary>
    /// <param name="e"></param>
    private void FormatCells(GridViewRowEventArgs e)
    {
        int iCellIndex = 0;
        int iColumnIndex = 0;
        string sText = string.Empty;
        
        int iCellCount = e.Row.Cells.Count - 1; // last column is tocheck deleted user.
        TableCellCollection cells = e.Row.Cells;
        string sUserName = e.Row.Cells[I_NAME_COLUMN_INDEX].Text;
        foreach (TableCell cell in cells)
        {
            sText = cell.Text;

            int iIndex = sText.IndexOf("_");
            if (iIndex >= 0 && iCellIndex != I_NAME_COLUMN_INDEX & iCellIndex != I_DESIGNATION_COLUMN_INDEX)
                cell.Text = sText.Substring(0, iIndex);

            cell.Style.Add("padding-left", "5");
            cell.Style.Add("padding-right", "5");

            string sColumnName = grdPaySalary.HeaderRow.Cells[iCellIndex].Text;
            cell.Attributes.Add("title", "User : " + sUserName + " [" + sColumnName + "]");

            if (!msColumnNumbers.Contains(iCellIndex + ","))
            {
                if (cell.Text == "-1" && !hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
                    cell.Text = string.Empty;
            }

            if (iCellIndex != I_NAME_COLUMN_INDEX && iCellIndex != I_DESIGNATION_COLUMN_INDEX)
                cell.HorizontalAlign = HorizontalAlign.Right;

            if ((cell.Text == "-1.00" || cell.Text == "-1") && !hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
                cell.Text = string.Empty;

            cell.Wrap = false;

            if (grdPaySalary.HeaderRow.Cells[iCellIndex].Text.Contains(PayrollConstants.S_NET_SALARY))
            {
                if (Convert.ToDecimal(e.Row.Cells[iCellIndex].Text) < 0)
                {
                    cell.ForeColor = Color.Red;
                    cell.Font.Bold = true;
                }
            }

            if (sColumnName == S_ISDELETED)
            {
                cell.Visible = false;
                if (cell.Text == "1")
                    e.Row.ForeColor = Color.Red;
            }
            
            if (hidColumnIndexes.Value.Contains("[" + iColumnIndex + "]"))
            {
                cell.Font.Bold = true;
                if (iColumnIndex == iCellCount - 1)
                {
                    cell.BackColor = Color.LightSteelBlue;
                    if (cell.ForeColor != Color.Red)
                        cell.ForeColor = Color.Maroon;
                }
                else if (e.Row.Cells[7].Text != "Total Total")
                {
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
            }

            RemoveCells(ref e);
        }
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
            iWidth = iWidth / 100 * 90;
            divContainer.Style.Add("width", iWidth.ToString() + "px !important");
            tblNote.Width = iWidth.ToString();
        }
        else
            divContainer.Style.Add("width", Convert.ToString(1024) + "px !important");
    }

    /// <summary>
    /// This method isused to remove cells.
    /// </summary>
    /// <param name="e"></param>
    private void RemoveCells(ref GridViewRowEventArgs e)
    {   
        for (int iCellIndex = 1; iCellIndex < e.Row.Cells.Count - 1; iCellIndex++)
        {
            if ((e.Row.Cells[iCellIndex].Text == null ||
                e.Row.Cells[iCellIndex].Text == Constants.S_ZERO ||
                e.Row.Cells[iCellIndex].Text == "0.0") &&
                e.Row.BackColor != Color.LightGray)
                e.Row.Cells[iCellIndex].Text = string.Empty;
        }

        e.Row.Cells[I_DESIGNATION_COLUMN_INDEX].Text = string.Empty;
    }

    /// <summary>
    /// This method is used to save salary details.
    /// </summary>
    /// <param name="mbSendSms"></param>
    private void SaveSalaryDetails(bool mbSendSms)
    {
        moSalaryDetailsBL = new SalaryDetailsBL();
        moSalaryDetailsBL.SalaryDetails = Populate(mbSendSms);

        moSalaryDetailsBL.Insert();
        if (mbSendSms)
        {
            if (chkSendSMS.Checked || chkSendDetailSMS.Checked)
                SendSMS(moSalaryDetailsBL);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
        }
    }

    /// <summary>
    /// This method is used to send salary sms.
    /// </summary>
    /// <param name="oSalaryDetailsBL"></param>
    private void SendSMS(SalaryDetailsBL oSalaryDetailsBL)
    {    
        const int S_SUBJECT = 1;
        const int S_SMS_TEXT = 2;

        string sSalarySMSText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;

        if (oSalaryDetailsBL.UserSalaryDetails.Count > 0)
        {            
            int iSmsId = Convert.ToInt32(chkSendSMS.Checked ? Constants.SMSTemplate.SalarySMS : Constants.SMSTemplate.SalaryDetailsSMS);

            DataTable oDtTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDtTemplate.IsNonEmpty())
            {
                if (oDtTemplate.Rows[0][S_SMS_TEXT] != DBNull.Value)
                {
                    sSalarySMSText = Convert.ToString(oDtTemplate.Rows[0][S_SMS_TEXT]);

                    if (oDtTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDtTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sSmsSubject = Convert.ToString(oDtTemplate.Rows[0][S_SUBJECT]);
                }
            }

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);

            Hashtable moManualMobileNo = new Hashtable();
            List<string> olstSMSText = new List<string>();

            bool bIsNewUser = true;

            List<SMSReceiverDetails> lstSMSDetails = new List<SMSReceiverDetails>();
            foreach (PaidSalaryDetails userDetails in oSalaryDetailsBL.UserSalaryDetails)
            {
                olstSMSText.Clear();
                moManualMobileNo.Clear();

                moManualMobileNo[userDetails.UserId] = userDetails.MobileNo;
                olstSMSText.Add(sSalarySMSText.Replace("%MONTH%", userDetails.Month + " " + userDetails.Year).Replace("%AMOUNT%", userDetails.NetSalary.ToString()));

                var oStaffAttn = oSalaryDetailsBL.StaffAttendanceList.Where(user => user.UserId == userDetails.UserId);

                if (oStaffAttn == null)
                {
                    moManualMobileNo.Clear();
                    continue;
                }

                if (chkSendDetailSMS.Checked)
                {
                    StringBuilder oLeaveDetails = new StringBuilder();
                    string sAttendance = string.Empty;
                    if (oStaffAttn != null)
                    {
                        StaffAttendance oStaffAttendance = oStaffAttn.FirstOrDefault();
                        var oLeaveDetailList = oSalaryDetailsBL.StaffLeaveDetailsList
                                        .Where(staffLeave => staffLeave.StaffAttendanceId == oStaffAttendance.StaffAttendanceId && staffLeave.Days != 0)
                                        .ToList();

                        if (oLeaveDetailList != null && oLeaveDetailList.Count > 0)
                            oLeaveDetailList.ForEach(leave => oLeaveDetails.Append(", " + leave.ShortName + " - " + ((leave.Days * 10) % 10 > 0 ? leave.Days : Convert.ToInt32(leave.Days))));

                        if (oStaffAttendance.PresentDays != 0)
                            sAttendance = "Attendance - " + ((oStaffAttendance.PresentDays * 10) % 10 > 0 ? oStaffAttendance.PresentDays : Convert.ToInt32(oStaffAttendance.PresentDays)) + ", ";

                        if (oLeaveDetails.Length > 0)
                            sAttendance = sAttendance + "Leaves - " + oLeaveDetails.ToString().Substring(2) + ", ";
                        olstSMSText[0] = olstSMSText[0].Replace("%SALARYDETAILS%", sAttendance.Substring(0, sAttendance.Length - 2));
                    }
                    else
                    {
                        moManualMobileNo.Clear();
                        continue;
                    }

                    StringBuilder oSalaryDetails = new StringBuilder();
                    oSalaryDetailsBL.PaidSalaryDetails
                                    .Where(user => user.UserId == userDetails.UserId)
                                    .ToList()
                                    .ForEach(user => oSalaryDetails.Append(", " + user.EarnDeductName + " - " + user.Amount));

                    if (oSalaryDetails.Length > 0)
                        olstSMSText.Add("Salary Details - " + oSalaryDetails.ToString().Substring(2));
                }
                
                SMS oSms = new SMS();
                if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter) == Constants.C_YES)
                {
                    oSms.SenderRoleID = Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
                    oSms.SenderID = miUserId;
                }
                else
                {
                    oSms.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                    oSms.SenderID = userDetails.AdminId;
                }

                bIsNewUser = true;
                foreach (string sSalarySMS in olstSMSText)
                {
                    oSms.InsertedByID = -9999;
                    oSms.Sender = oSchoolBL.SMSSenderName;
                    oSms.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                    oSms.SMSText = sSalarySMS;
                    oSms.AcademicYearID = miAcademicYearId;
                    oSms.SchoolID = miSchoolId;
                    oSms.TemplateRegistrationId = sTemplateRegistrationId;
                    oSms.DisplayText = userDetails.Name;
                    oSms.BlockDBEntry = true;
                    oSms.To = moManualMobileNo;                    
                    oSms.Send(bIsNewUser);
                    bIsNewUser = false;

                    lstSMSDetails.Add
                    (
                        new SMSReceiverDetails
                        {
                            SMS_Text = HttpUtility.HtmlDecode(sSalarySMS).Replace("\\n", "\n"),
                            Display_Text = userDetails.Name,
                            UserId = userDetails.UserId,
                            MobileNo = userDetails.MobileNo
                        }
                    );
                }

                moManualMobileNo.Clear();
            }

            SMSMasterBL moSMSMasterBL = new SMSMasterBL();
            moSMSMasterBL.Sender_Name = oSchoolBL.SMSSenderName;
            moSMSMasterBL.SchoolId = miSchoolId;
            moSMSMasterBL.AcademicYearId = miAcademicYearId;

            if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter) == Constants.C_YES)
            {
                moSMSMasterBL.Sender_User_Role_Id = Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
                moSMSMasterBL.Sender_User_Id = miUserId;
            }
            else
            {
                moSMSMasterBL.Sender_User_Role_Id = Convert.ToInt32(Constants.UserRoles.Admin);
                moSMSMasterBL.Sender_User_Id = oSalaryDetailsBL.UserSalaryDetails[0].AdminId;
            }

            moSMSMasterBL.SaveSMSDetails(base.GenerateXml(lstSMSDetails));
        }
    }
    
    /// <summary>
    /// This method is used to populate SalaryDetailsBL object. 
    /// </summary>
    /// <returns></returns>
    private SalaryDetails Populate(bool abSendSMS)
    {
        SalaryDetails oSalaryDetails = new SalaryDetails();

        oSalaryDetails.SchoolId = miSchoolId;
        oSalaryDetails.AcademicYearId = miAcademicYearId;
        oSalaryDetails.InsertedById = miUserId;

        if (trCombobox.Visible == false)
        {
            oSalaryDetails.MonthId = Convert.ToInt32(hidMonthId.Value);
            oSalaryDetails.Year = Convert.ToInt32(hidYear.Value);
        }
        else
        {
            oSalaryDetails.MonthId = Convert.ToInt32(cmbMonths.SelectedValue);
            oSalaryDetails.Year = Convert.ToInt32(cmbYear.SelectedValue);
        }

        oSalaryDetails.ChequeNo = txtChequeNo.Text.Trim() != string.Empty ? txtChequeNo.Text.Trim() : Constants.S_ZERO;
        oSalaryDetails.ChequeDate = txtChequeDate.Text.Trim() != string.Empty ? Convert.ToDateTime(txtChequeDate.Text) : DateTime.Now.Date;
        oSalaryDetails.ChequeAmount = txtAmount.Text.Trim() != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0;
        oSalaryDetails.StaffGroupsId = 0;

        oSalaryDetails.LeaveTransferMonthId = Settings.LeaveTransferMonth;
        oSalaryDetails.SalaryDetailsXml = GenerateSalaryDetailsXml();
        oSalaryDetails.SalayDifferenceXml = hidMonthList.Value != string.Empty ? GenerateSalaryDifferenceXml() : string.Empty;
        oSalaryDetails.SchoolWiseBankAccountDetailsId = Convert.ToInt32(cmbAccountNo.SelectedIndex) != 0 ? Convert.ToInt32(cmbAccountNo.SelectedValue) : 0;
        oSalaryDetails.IsPreviewDisplayed = !abSendSMS;
        oSalaryDetails.IsOnlineTransaction = optOnline.Checked;
        return oSalaryDetails;
    }

    /// <summary>
    /// This method is used to generate xml of salary details to save.
    /// </summary>
    /// <returns></returns>
    private string GenerateSalaryDetailsXml()
    {
        DataTable oDataTable = new DataTable ();

        if (Session[PayrollConstants.S_SALARY_DETAILS] != null)
            oDataTable = (DataTable)Session[PayrollConstants.S_SALARY_DETAILS];
        else
            throw new SalaryPublishException("Publish failed. Please click on 'Refresh' button and try once again.");

        int iRowCount = oDataTable.Rows.Count;

        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SalaryDetailsXml");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetailsXml", string.Empty);

        string sName = string.Empty;

        // Loop through all the grid rows.
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetailsXml", string.Empty);

            sAttribute = "UserId";
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = oDataTable.Rows[iRowIndex]["UserId"].ToString();
            oXmlNode.Attributes.Append(attr);

            decimal dcNetSalary = Convert.ToDecimal(oDataTable.Rows[iRowIndex][PayrollConstants.S_NET_SALARY]);            
            sAttribute = "NetSalary";
            attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = dcNetSalary.ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "StaffGroupId";
            attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = oDataTable.Rows[iRowIndex]["StaffGroupId"].ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "SalaryXml";
            attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = GenerateXml(oDataTable, iRowIndex);
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
    /// This method is used to generate xml of individuals salary details.
    /// </summary>
    /// <param name="aoDataTable"></param>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private string GenerateXml(DataTable aoDataTable, int aiRowIndex)
    {
        int iColumnCount = aoDataTable.Columns.Count - 1;// last column is to check dleted user.

        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SalaryDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetails", string.Empty);

        string sName = string.Empty;

        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SalaryDetails", string.Empty);
        // Loop through all the grid rows.
        for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
        {
            DataRow oDataRow = aoDataTable.Rows[aiRowIndex];

            sAttribute = aoDataTable.Columns[iColumnIndex].ColumnName;
            sAttribute = sAttribute.Replace(" ", "_");
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            sName = oDataRow[iColumnIndex].ToString().Replace("'", "''");
            attr.Value = sName;
            oXmlNode.Attributes.Append(attr);            
        }

        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to generate salary difference xml.
    /// </summary>
    /// <returns></returns>
    private string GenerateSalaryDifferenceXml()
    {
        string sXml = string.Empty;
        DataTable oDataTable = null;

        if (Session[PayrollConstants.S_SALARY_DETAILS] != null)
            oDataTable = (DataTable)Session[PayrollConstants.S_SALARY_DETAILS];

        if (oDataTable.IsNonEmpty())
        {
            var vSalaryCollection = from SalaryDif in oDataTable.AsEnumerable()
                                   where SalaryDif.Field<string>("UserId") != "-9999"
                                   select new
                                   {
                                       SalaryDifference = SalaryDif.Field<string>("Gross Salary Difference"),
                                       ProvidentFund = SalaryDif.Field<string>("Salary Difference of Deduction"),
                                       UserId = SalaryDif.Field<string>("UserId")
                                   };

            XmlDocument oDoc = new XmlDocument();
            XmlElement oRoot = oDoc.CreateElement("SalaryDifference");
            XmlNode oXmlRootNode = oDoc.CreateNode("element", "SalaryDifference", string.Empty);
            string sSalDifference = string.Empty;
            foreach (var salaryDifference in vSalaryCollection)
            {
                sSalDifference = salaryDifference.SalaryDifference;
                sSalDifference = sSalDifference.Substring(0, sSalDifference.IndexOf("_"));
                int iAmount = Convert.ToInt32(sSalDifference);
                int iPFAmount = Convert.ToInt32(salaryDifference.ProvidentFund.Substring(0, salaryDifference.ProvidentFund.IndexOf("_")));

                if (iAmount != 0)
                {
                    XmlNode oXmlNode = oDoc.CreateNode("element", "SalaryDifference", string.Empty);

                    XmlAttribute attr = oDoc.CreateAttribute("UserId");
                    attr.Value = salaryDifference.UserId;
                    oXmlNode.Attributes.Append(attr);

                    attr = oDoc.CreateAttribute("ProvidentFund");
                    attr.Value = iPFAmount.ToString();
                    oXmlNode.Attributes.Append(attr);

                    attr = oDoc.CreateAttribute("GrossAmount");
                    attr.Value = iAmount.ToString();
                    oXmlNode.Attributes.Append(attr);

                    attr = oDoc.CreateAttribute("NetSalary");
                    attr.Value = (iAmount - iPFAmount).ToString();
                    oXmlNode.Attributes.Append(attr);

                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }

            oRoot.AppendChild(oXmlRootNode);
            sXml = oRoot.InnerXml;
        }

        return sXml;
    }

    /// <summary>
    /// This method is used to disable controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisableControls(bool abAction)
    {   
        btnExport.Enabled = !abAction;
        valSumShow.Visible = abAction;
        btnSalarySlipPreview.Enabled = !abAction;
        Button oBtnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
        if (hidIsMidYear.Value == Constants.S_YES)
            oBtnFinish.Visible = false;
        else
            oBtnFinish.Enabled = !abAction;

        if (hidInvalidLeaves.Value == Constants.S_YES)
        {
            oBtnFinish.Visible = false;
            trInvalidLeaveMessage.Visible = true;
        }
        else
        {
            oBtnFinish.Visible = hidIsMidYear.Value != Constants.S_YES;
            oBtnFinish.Enabled = !abAction;
            trInvalidLeaveMessage.Visible = false;
        }
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
                int iPageNumber = iPageIndex + 1;
                ListItem item = new ListItem(iPageNumber.ToString());

                if (iPageIndex == grdPaySalary.PageIndex)
                    item.Selected = true;

                // Add the ListItem object to the Items collection of the DropDownList.
                PageDropDownList.Items.Add(item);
            }

            PageDropDownList.SelectedIndex = Convert.ToInt32(hidSelectedPageIndex.Value);
            lblCurrentPage.Text = "Page " + PageDropDownList.SelectedValue + "  of " + miTotalPages;

            int iLastIndex = Convert.ToInt32(PageDropDownList.SelectedValue) * Constants.I_GRID_PAGE_COUNT;

            lblStartIndex.Text = ((Convert.ToInt32(PageDropDownList.SelectedValue) - 1) * Constants.I_GRID_PAGE_COUNT + 1).ToString();
            lblEndIndex.Text = iLastIndex < miTotalRecords ? iLastIndex.ToString() : miTotalRecords.ToString();
            lblTotalRecords.Text = miTotalRecords.ToString();

            tblPager.Visible = true;
            tblPageDetails.Visible = true;
            if (miTotalPages == 1 || grdPaySalary.Rows.Count <= 0)
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
    /// This method is used to set precondition view.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetPreconditionView(bool abAction)
    {
        divErr.Visible = !abAction;
        trChequeNo.Visible = abAction;
        trButtons.Visible = abAction;
        trWizard.Visible = abAction;
    }

    /// <summary>
    /// This method is used to check whether P.T. challan details are exist or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckPTChallanDetailsExists()
    {   
        int iMonthId = Convert.ToInt32(cmbMonths.SelectedValue);
        int iYear = Convert.ToInt32(cmbYear.SelectedValue);
        return SalaryDetailsBL.CheckPTChallanDetailsExists(miSchoolId, iMonthId, iYear);
    }

    /// <summary>
    /// This method is used to set precondition message.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetPreconditionMessage(bool abAction)
    {
        lblInvalidLeaveMEssage.Text = string.Empty;
        Button btnFinish = (Button)wizSalaryInformation.WizardSteps[0].FindControl("FinishNavigationTemplateContainerID").FindControl("PublishPutton");
        btnFinish.Visible = !abAction;
        trInvalidLeaveMessage.Visible = abAction;
        hlnkPTChallan.Visible = abAction;
    }

    /// <summary>
    /// This event is used to set field label according to selected payment type.
    /// </summary>
    /// <param name="asAmount"></param>
    /// <param name="asDate"></param>
    /// <param name="asNumber"></param>
    private void SetLabelAccordingToPaymentType(bool abIsOnlinePayment)
    {        
        if (!abIsOnlinePayment)
        {
            lblChequeAmount.Text = "Cheque Amount :";
            lblChequeDate.Text = "Cheque Date :";
            lblChequeNo.Text = "Cheque No.:";
            spnStar.InnerText = "*";
        }
        else
        {
            lblChequeAmount.Text = "Transaction Amount :";
            lblChequeDate.Text = "Transaction Date :";
            lblChequeNo.Text = "Transaction No.:";
            spnStar.InnerText = string.Empty;
        }
    }

    #endregion

    #region Report

    /// <summary>
    /// This method is used to delete salary of selected month and year.
    /// </summary>
    private void DeleteSalary()
    {   
        int iMonthId = Convert.ToInt32(hidMonthId.Value);
        int iYear = Convert.ToInt32(hidYear.Value);
        SalaryDetailsBL.DeleteSalary(miSchoolId, miAcademicYearId, iMonthId, iYear);
    }

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    private void DisplayReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.SalarySlipReport, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to return filter string.m
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {   
        int iMonthId = Convert.ToInt32(hidMonthId.Value);
        int iYear = Convert.ToInt32(hidYear.Value);

        DateTime dtDate = new DateTime(iYear, iMonthId, 1);

        string sRecordSelectionFormula = "(usp_GetSalarySlipDetails.School_Id}=" + miSchoolId + " AND  usp_GetSalarySlipDetails.Academic_Year_Id} =" + miAcademicYearId +
          " AND usp_GetSalarySlipDetails.FromDate}=" + dtDate.ToString(Constants.S_DATE_FORMAT) + " AND usp_GetSalarySlipDetails.StaffGroupsId} = null AND usp_GetSalarySlipDetails.UserId} = null AND usp_GetSalarySlipDetails.LoginUserId} = "+miUserId+" AND  usp_GetSalarySlipDetails.ToDate} =" + dtDate.ToString(Constants.S_DATE_FORMAT) + ")" + "@ ";
        return sRecordSelectionFormula;
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.PaySalary);

        if (hidPTDetails.Value == Constants.S_NO)
        {
            if (sLinks == string.Empty)
                sLinks = "<table class='LblNoRecord' width='100%'  cellpadding='0' cellspacing='0'><tr><td class='ClsConfigText'>Please configure following details for School :</td></tr><tr><td><a class='ClsConfigLink' href=PTChallanDetails.aspx>Professional Tax Challan Details for " + hidMonthAndYear.Value + "</a></td></tr></table>";
            else
            {
                sLinks = sLinks.Replace("</table>", string.Empty);
                sLinks = sLinks + "<tr><td><a class='ClsConfigLink' href=PTChallanDetails.aspx>Professional Tax Challan Details for " + hidMonthAndYear.Value + "</a></td></tr>";
                sLinks = sLinks + "</table>";
            }
        }

        if (!sLinks.Equals(String.Empty))
            divErr.InnerHtml = sLinks;
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    #endregion
}