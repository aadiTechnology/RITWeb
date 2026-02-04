/* -------------------------------------------------------------------------------
 *	MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *	Author	: Vishal B. Shah
 *	Date	: 25-Jan-2012
 *	Purpose	: Modified to record cleared payments in the Accounts Module.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using AccountsEntities;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using SchoolEntities.StudentFee;
using System.Globalization;
using FeeEntities;
using System.IO;
using System.Web;

/// <summary>
/// Allows user to clear payments receieved from various modes.
/// </summary>
public partial class ClearanceList : SchoolBase
{

    #region -- CONSTANT(s) --

    private const int FILTER_BY_TRANSACTION_ID = 1;
    private const int FILTER_BY_STUDENT_NAME_REG_NO = 2;
    private const int FILTER_BY_TRANSACTION_DATE = 3;
    private const int FILTER_BY_CLEARANCE_DATE = 4;

    private const int I_TPSLTRANSID_COL_INDEX = 3;
    private const int I_CHEQUE_NO_COL_INDEX = 4;
    private const int I_BANK_NAME_COL_INDEX = 5;
    private const int I_RECEIPT_NUMBER_COL_INDEX = 6;
    private const int I_PAYABLE_FOR_COL_INDEX = 8;
    private const int I_TRANS_DATETIME_COL_INDEX = 9;
    private const int I_CHEQUE_DATE_COL_INDEX = 10;
    private const int I_TRANSACTION_NO_COL_INDEX = 11;
    private const int I_PAID_DATE_COL_INDEX = 12;
    private const int I_PAYMENT_DATE_COL_INDEX = 13;
    private const int I_DEPOSIT_BANK_COL_INDEX = 15;

    private const string HIDE_PAGE_NUMBER = "1";

    private const string S_ELEMENT = "element";

    #endregion -- CONSTANT(s) --

    #region -- MEMBER(s) --

    private int miTotalAmount;
    private List<BankAccount> mlstBanks;
    private StudentFeeDetailsBL moStudentFeeDetailsBL;

    #endregion -- MEMBER(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Returns true if the Accounts module is enabled, false otherwise.
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    private bool UpdateInternalFeeinDayBook
    {
        get
        {
            if (moSchool == Constants.SchoolId.PPS)
                return true;
            else
                return optStudentFee.Checked;
        }
    }

    #endregion -- PROPERTIES --

    #region -- EVENT(s) --

    /// <summary>
    /// This event is used to handle loading of the Page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
            CheckFinancialYearStatus();
            if (!Page.IsPostBack)
            {                
                InitializeControls();
                SetClientScriptAttribute();
                btnSave.Style.Add("Visibility", "Hidden");
                btnExport.Style.Add("Visibility", "Hidden");
                FillBankCombo();
                optChequeClearance.Checked = true;
                optStudentFee.Checked = true;
                SetClearanceMode();
                SerializeFinancialYear();
                hidBaseFinancialYearId.Value = miFinancialYearId.ToString();                  
            }
            if (optCashClearance.Checked || optCardClearance.Checked || optElectronicPaymentClearance.Checked)
            {
                cstClearanceDate.EnableClientScript = true;
                cstvalOnlinePayment.EnableClientScript = false;
                cstChequePayment.EnableClientScript = false;
            }
            else if (optOnlineTransactionClearance.Checked)
            {
                cstClearanceDate.EnableClientScript = false;
                cstvalOnlinePayment.EnableClientScript = true;
                cstChequePayment.EnableClientScript = false;
            }
            else if (optChequeClearance.Checked)
            {
                cstClearanceDate.EnableClientScript = false;
                cstvalOnlinePayment.EnableClientScript = false;
                cstChequePayment.EnableClientScript = true;
            }

            // Disable client-size validation scripts if the accounts module is disabled.
            if (!IsAccountsModuleEnabled)
            {
                cstDepositBankValidator.EnableClientScript = false;
                cstAcValidateClearanceDate.EnableClientScript = false;
            }
            if (optInternalFee.Checked)
            {
                trCautionMoney.Visible = false;
            }
            else
            {
                trCautionMoney.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls visibility when the clearance mode changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearance_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            trCautionMoney.Visible = true;
            SetClearanceMode();
            btnShow.Text = "Show";
            EnableDisableControlChecked(true);
            EnableDisableControls(true);
            grdvwClearedCash.DataSource = null;
            grdvwClearedCash.DataBind();
            grdvwClearedCash.Visible = false;
            trTotalRec.Visible = false;
            tblTotalAmount.Visible = false;
            btnSave.Style.Add("Visibility", "Hidden");
            //btnExport.Style.Add("Visibility", "Hidden");
          
            
            if (optCashClearance.Checked || optCardClearance.Checked || optOnlineTransactionClearance.Checked || optElectronicPaymentClearance.Checked )
            {
                tdPaymentBankName.Visible = false;
                tdcmbPaymentBankName.Visible = false;
                cmbClearanceBank.SelectedValue = Constants.S_ZERO;
            }
            if (optChequeClearance.Checked)
            {
                tdPaymentBankName.Visible = true;
                tdcmbPaymentBankName.Visible = true;
            }
            if (optChequeClearance.Checked)
                  trCautionMoney.Visible = Settings.IsCautionMoneyApplicable;


            if (optInternalFee.Checked)
            {
                trCautionMoney.Visible = false;
            }
            else
            {
                trCautionMoney.Visible = true;

            }

            HideRow();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill  grid according to filter and cleare paid cash amount.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text == "Show")
            {
                hidPageNo.Value = HIDE_PAGE_NUMBER;
                grdvwClearedCash.PageIndex = Constants.I_ZERO;
                if (optCashClearance.Checked)
                    FillClearedCashPaymentGrid();
                if (optOnlineTransactionClearance.Checked)
                    FillOnlineTransactionGrid();
                if (optCardClearance.Checked)
                    FillCardPaymentsGrid();
                if (optChequeClearance.Checked)
                    FillChequesGrid();
                if (optElectronicPaymentClearance.Checked)
                    FillElectronicPaymentsGrid();
                btnShow.Text = "Change Input";
                EnableDisableControlChecked(false);
                EnableDisableControls(false);
                HideRow();
            }
            else
            {
                btnShow.Text = "Show";
                EnableDisableControlChecked(true);
                EnableDisableControls(true);
                grdvwClearedCash.DataSource = null;
                grdvwClearedCash.DataBind();
                grdvwClearedCash.Visible = false;
                tblLegend.Visible = false;
                trTotalRec.Visible = false;
                tblTotalAmount.Visible = false;
                lblError.Visible = false;
                HideRow();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export the cheque clearance details in the Excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = null;
            if (optCashClearance.Checked)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.ExportCashPayment, GetCashClearanceFilterString());
            else if (optOnlineTransactionClearance.Checked)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.OnlineTransactionClearanceDetails, GetOnlineTransactionFilterString());
            else if (optCardClearance.Checked)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.CardPaymentDetails, GetCardClearanceFilterString());
            else if (optChequeClearance.Checked)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.ChequeClearanceDetails, GetFilterChequePaymentString());
            else if (optElectronicPaymentClearance.Checked)
                oReportDisplay = new ReportDisplay(Constants.ExportReports.ElectronicPaymentDetails, GetElectronicClearanceFilter());

            oReportDisplay.DisplayReport();
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
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Here we will will display details by checking appropriate condition after changing page.
            // This is existing method. I have added a check for electronic payment mode.

            GridViewRow oPageRow = grdvwClearedCash.BottomPagerRow;
            var oPageNumberList = oPageRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
            grdvwClearedCash.PageIndex = oPageNumberList.SelectedIndex;
            if (optCashClearance.Checked)
                FillClearedCashPaymentGrid();
            else if (optOnlineTransactionClearance.Checked)
                FillOnlineTransactionGrid();
            else if (optCardClearance.Checked)
                FillCardPaymentsGrid();
            else if (optChequeClearance.Checked)
                FillChequesGrid();
            else if (optElectronicPaymentClearance.Checked)
                FillElectronicPaymentsGrid();

            hidPageNo.Value = (oPageNumberList.SelectedIndex + 1).ToString();
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Cash payments which are cleared.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {            
            if (optCashClearance.Checked)
                SaveCashPayments();
            else if (optOnlineTransactionClearance.Checked)
                SaveOnlineTrasactionPayments();
            else if (optCardClearance.Checked)
                SaveCardPayments();
            else if (optChequeClearance.Checked)
                SaveChequePayments();
            else if (optElectronicPaymentClearance.Checked)
                SaveElectronicPayments();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer dropdown list in the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwClearedCash_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                case DataControlRowType.DataRow:
                    if (!IsAccountsModuleEnabled)
                        e.Row.Cells[I_DEPOSIT_BANK_COL_INDEX].Visible = false;
                    if (optCashClearance.Checked || optCardClearance.Checked || optElectronicPaymentClearance.Checked)
                    {
                        e.Row.Cells[I_TPSLTRANSID_COL_INDEX].Visible = false;
                        e.Row.Cells[I_CHEQUE_NO_COL_INDEX].Visible = false;
                        e.Row.Cells[I_BANK_NAME_COL_INDEX].Visible = false;
                        e.Row.Cells[I_TRANS_DATETIME_COL_INDEX].Visible = false;
                        e.Row.Cells[I_CHEQUE_DATE_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAYMENT_DATE_COL_INDEX].Visible = false;
                        if (optElectronicPaymentClearance.Checked || optCardClearance.Checked)
                            e.Row.Cells[I_TRANSACTION_NO_COL_INDEX].Visible = true;
                        else
                            e.Row.Cells[I_TRANSACTION_NO_COL_INDEX].Visible = false;
                    }
                    else if (optOnlineTransactionClearance.Checked)
                    {
                        e.Row.Cells[I_CHEQUE_NO_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAYABLE_FOR_COL_INDEX].Visible = false;
                        e.Row.Cells[I_CHEQUE_DATE_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAID_DATE_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAYMENT_DATE_COL_INDEX].Visible = false;
                        e.Row.Cells[I_TRANSACTION_NO_COL_INDEX].Visible = false;
                    }
                    else if (optChequeClearance.Checked)
                    {
                        e.Row.Cells[I_TPSLTRANSID_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAYABLE_FOR_COL_INDEX].Visible = false;
                        e.Row.Cells[I_TRANS_DATETIME_COL_INDEX].Visible = false;
                        e.Row.Cells[I_PAID_DATE_COL_INDEX].Visible = false;
                        e.Row.Cells[I_TRANSACTION_NO_COL_INDEX].Visible = false;

                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            int iPostDatedChequeId = grdvwClearedCash.DataKeys[e.Row.RowIndex]["PostDated_Cheque_Id"].ToInt();
                            bool bIsReturnPayment = grdvwClearedCash.DataKeys[e.Row.RowIndex]["IsReturnPayment"].ToBool();
                            if (iPostDatedChequeId == 0)
                                e.Row.BackColor = bIsReturnPayment ? Color.LightPink : Color.LightBlue;
                        }
                    }
                    if (IsAccountsModuleEnabled && e.Row.RowType == DataControlRowType.DataRow)
                    {
                        var ddlDepositBankList = e.Row.FindControl("ddlDepositedBankList") as DropDownList;
                        ddlDepositBankList.Bind(GetBankList(), "Id", "Name", Constants.S_SELECT);
                        ddlDepositBankList.SelectedValue = grdvwClearedCash.DataKeys[e.Row.RowIndex]["DepositBankId"].ToString();
                    }
                    if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                        e.Row.Cells[I_RECEIPT_NUMBER_COL_INDEX].Visible = false;
                    break;
                case DataControlRowType.Pager:
                    {
                        GridViewRow oPageRow = e.Row;
                        var oPageList = oPageRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
                        oPageList.Attributes.Add("onchange", string.Format("if(!MessageAboutDate('{0}')){{return false;}}", oPageList.ClientID));
                        var oPageLabel = oPageRow.Cells[0].FindControl("CurrentPageLabel") as Label;
                        for (int i = 0; i < grdvwClearedCash.PageCount; i++)
                        {
                            int iPageumber = i + 1;
                            var oListItem = new ListItem(iPageumber.ToString());
                            if (i == grdvwClearedCash.PageIndex)
                                oListItem.Selected = true;
                            oPageList.Items.Add(oListItem);
                        }
                        if (oPageLabel != null)
                        {
                            int iCurrentPageCount = grdvwClearedCash.PageIndex + 1;
                            oPageLabel.Text = string.Format("Page {0} " + "of" + " {1}", iCurrentPageCount, grdvwClearedCash.PageCount);
                        }
                        DisplayRowDetails();
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set filter of Registration number for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRegNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optRegNoChecked();
            HideControls();
            if (optChequeClearance.Checked)
                tblLegend.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set filter based on Payment Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPaymentDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optPaymentDateChecked();
            HideControls();
            if (optChequeClearance.Checked)
                tblLegend.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set filter based on Clearance Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearanceDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optClearanceDateChecked();
            HideControls();
            if (optChequeClearance.Checked)
                tblLegend.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls visibility when the transaction id radio button is checked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optTransactionNumber_heckedChanged(object sender, EventArgs e)
    {
        try
        {
            OptTrasactionIdCheck();
            HideControls();
            if (optChequeClearance.Checked)
                tblLegend.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set filter of cheque number for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optChequeNumber_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            OptCheckedNumberChecked();
            HideControls();
            if (optChequeClearance.Checked)
                tblLegend.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwClearedCash_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwClearedCash.PageIndex = e.NewPageIndex;
            if (optCashClearance.Checked)
                FillClearedCashPaymentGrid();
            else if (optOnlineTransactionClearance.Checked)
                FillOnlineTransactionGrid();
            else if (optCardClearance.Checked)
                FillCardPaymentsGrid();
            else if (optChequeClearance.Checked)
                FillChequesGrid();
            else if (optElectronicPaymentClearance.Checked)
                FillElectronicPaymentsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export fee details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportFee_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
            List<FeeDetailsToExport> lstFeeDetails = oStudentFeeDetailsBL.GetFeeDetailsToExport(0,0,0);
            string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("ENVELOPE");

            XmlNode oXmlFee = null;

            var studentIds = lstFeeDetails.Select(std => std.StudentId).Distinct().Take(2).ToList();

            XmlNode oHeader = oDoc.CreateNode(S_ELEMENT, "HEADER", string.Empty);

            XmlNode oTallyRequest = oDoc.CreateNode(S_ELEMENT, "TALLYREQUEST", string.Empty);
            oTallyRequest.InnerText = "Import Data";
            oHeader.AppendChild(oTallyRequest);

            root.AppendChild(oHeader);

            XmlNode oBody = oDoc.CreateNode(S_ELEMENT, "BODY", string.Empty);

            XmlNode oImportData = oDoc.CreateNode(S_ELEMENT, "IMPORTDATA", string.Empty);

            XmlNode oReqDesc = oDoc.CreateNode(S_ELEMENT, "REQUESTDESC", string.Empty);

            XmlNode oReportName = oDoc.CreateNode(S_ELEMENT, "REPORTNAME", string.Empty);
            oReportName.InnerText = "Vouchers";
            oReqDesc.AppendChild(oReportName);

            var oStaticVar = oDoc.CreateNode(S_ELEMENT, "STATICVARIABLES", string.Empty);
            oReqDesc.AppendChild(oStaticVar);

            var oCurrentCompany = oDoc.CreateNode(S_ELEMENT, "SVCURRENTCOMPANY", string.Empty);
            oCurrentCompany.InnerText = "Dsk School";
            oStaticVar.AppendChild(oCurrentCompany);

            oImportData.AppendChild(oReqDesc);

            var oReqData = oDoc.CreateNode(S_ELEMENT, "REQUESTDATA", string.Empty);

            //var oTallyMessage = oDoc.CreateNode(S_ELEMENT, "TALLYMESSAGE", string.Empty);

            //AddAttribute("xmlns:UDF", "TallyUDF", oTallyMessage, oDoc);

            int iSerialNo = 1;

            foreach (var iStudentId in studentIds)
            {
                var oTallyMessage = oDoc.CreateNode(S_ELEMENT, "TALLYMESSAGE", string.Empty);

                AddAttribute("xmlns:UDF", "TallyUDF", oTallyMessage, oDoc);

                var oVoucher = oDoc.CreateNode(S_ELEMENT, "VOUCHER", string.Empty);

                AddAttribute("REMOTEID", "7995bf93-5052-4587-9558-abd51caff250-" + iSerialNo, oVoucher, oDoc);
                AddAttribute("VCHKEY", "7995bf93-5052-4587-9558-abd51caff250-0000a301:00000be8", oVoucher, oDoc);
                AddAttribute("VCHTYPE", "Journal", oVoucher, oDoc);
                AddAttribute("ACTION", "Create", oVoucher, oDoc);
                AddAttribute("OBJVIEW", "Accounting Voucher View", oVoucher, oDoc);

                lstFeeDetails.Where(std => std.StudentId == iStudentId && std.ParentId == 0).ToList().ForEach(
                    fee =>
                    {
                        oXmlFee = oDoc.CreateNode(S_ELEMENT, fee.Field, string.Empty);
                        oXmlFee.InnerText = fee.Value;

                        if (fee.Field == "OLDAUDITENTRYIDS.LIST")
                            AddAttribute("TYPE", "Number", oXmlFee, oDoc);

                        if (lstFeeDetails.Any(fd => fd.ParentId == fee.Id))
                            AddNodes(oXmlFee, fee.Id, lstFeeDetails, oDoc, iStudentId);

                        oVoucher.AppendChild(oXmlFee);
                    }
                );

                oTallyMessage.AppendChild(oVoucher);

                oReqData.AppendChild(oTallyMessage);
            }

            //oReqData.AppendChild(oTallyMessage);

            oImportData.AppendChild(oReqData);

            oBody.AppendChild(oImportData);

            root.AppendChild(oBody);

            oDoc.AppendChild(root);

            string sVoucherXMLFilePath = HttpContext.Current.Server.MapPath("..") + "\\DOWNLOADS\\FeeVouchers.xml";

            if (File.Exists(sVoucherXMLFilePath))
                File.Delete(sVoucherXMLFilePath);
            oDoc.Save(sVoucherXMLFilePath);
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=FeeVoucher.xml");
            HttpContext.Current.Response.TransmitFile(sVoucherXMLFilePath);
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// Displays controls based on the clearance mode selected.
    /// </summary>
    private void SetClearanceMode()
    {   
        if (optChequeClearance.Checked)
        {
            trChequeNo.Visible = true;
            trCardtype.Visible = false;
            trGateway.Visible = false;
            trTransactionNumber.Visible = false;
            trORChequeNo.Visible = true;

            optClearanceDate.Checked = false;
            optPaymentDate.Checked = false;
            optRegNo.Checked = false;
            optChequeNumber.Checked = true;
            optTransactionNumber.Checked = false;

            chkIncludeAll.Text = "Include cheques which are cleared.";
            lblPaymentDate.Text = "Payment Start Date: ";
            OptCheckedNumberChecked();
        }
        else if (optCardClearance.Checked)
        {
            trChequeNo.Visible = false;
            trCautionMoney.Visible = false;
            trCardtype.Visible = true;
            trGateway.Visible = false;
            trTransactionNumber.Visible = true;
            trORChequeNo.Visible = false;

            lblCardType.Text = "Swipe Card Type :";
            tblLegend.Visible = false;
            cmbCardType.Items.Clear();
            FillCardTypeCombo();

            optRegNo.Checked = true;
            optClearanceDate.Checked = false;
            optPaymentDate.Checked = false;
            optChequeNumber.Checked = false;
            optTransactionNumber.Checked = false;

            chkCautionMoney.Checked = false;
            chkIncludeAll.Text = "Include Card payments which are cleared.";
            lblPaymentDate.Text = "Payment Start Date: ";
            optRegNoChecked();
        }
        else if (optCashClearance.Checked)
        {
            trChequeNo.Visible = false;
            trCautionMoney.Visible = false;
            trCardtype.Visible = false;
            trGateway.Visible = false;
            trTransactionNumber.Visible = false;
            tblLegend.Visible = false;
            trORChequeNo.Visible = false;

            optRegNo.Checked = true;
            optClearanceDate.Checked = false;
            optPaymentDate.Checked = false;
            optChequeNumber.Checked = false;
            optTransactionNumber.Checked = false;

            chkCautionMoney.Checked = false;
            chkIncludeAll.Text = "Include cash payments which are cleared.";
            lblPaymentDate.Text = "Payment Start Date: ";
            optRegNoChecked();
        }
        else if (optOnlineTransactionClearance.Checked)
        {
            trChequeNo.Visible = false;
            trCautionMoney.Visible = Settings.EnableOnlinePaymentForCautionMoney;
            trCardtype.Visible = false;
            trGateway.Visible = true;
            trTransactionNumber.Visible = true;
            trORChequeNo.Visible = true;

            optClearanceDate.Checked = false;
            optPaymentDate.Checked = false;
            optRegNo.Checked = false;
            optTransactionNumber.Checked = true;
            optChequeNumber.Checked = false;
            FillGatewayCombo();

            tblLegend.Visible = false;
            chkIncludeAll.Text = "Include transaction records which are cleared.";
            lblPaymentDate.Text = "Transaction Start Date: ";
            chkCautionMoney.Checked = false;
            OptTrasactionIdCheck();
        }
        else if (optElectronicPaymentClearance.Checked)
        {

            
            trChequeNo.Visible = false;
           // trCautionMoney.Visible = false;
            trCardtype.Visible = true;
            trGateway.Visible = false;
            trTransactionNumber.Visible = true;
            trORChequeNo.Visible = false;

            lblCardType.Text = "Electronic Payment Type :";
            tblLegend.Visible = false;
            cmbCardType.Items.Clear();
            FillElectronicPaymentTypes();

            optRegNo.Checked = true;
            optClearanceDate.Checked = false;
            optPaymentDate.Checked = false;
            optChequeNumber.Checked = false;
            optTransactionNumber.Checked = false;

            chkCautionMoney.Checked = false;
            chkIncludeAll.Text = "Include Electronic payments which are cleared.";
            lblPaymentDate.Text = "Payment Start Date: ";
            optRegNoChecked();

        }
    }

    /// <summary>
    /// Serializes the FinancialYearMaster entity object to a hidden field.
    /// </summary>
    private void SerializeFinancialYear()
    {
        if (!IsAccountsModuleEnabled)
            return;

        var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
        if (oFinancialYear != null)
        {
            var jsSerializer = new JavaScriptSerializer();
            hidFinancialYearJSON.Value = jsSerializer.Serialize(oFinancialYear);
        }

        if (Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] != null)
            hidCanEditOldFinancialYear.Value = Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR].ToString().ToLower();
    }

    /// <summary>
    /// Popoulates the Card type dropdown list.
    /// </summary>
    private void FillCardTypeCombo()
    {
        var oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable oDT = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(miSchoolId);
        cmbCardType.Bind(oDT, "CardTypeId", "CardType", Constants.S_SELECT_ALL);
    }

    private void FillGatewayCombo()
    {
        var oPaymentGatewayBL = new PaymentGatewayBL();
        DataTable odt = oPaymentGatewayBL.GetPaymentGateway(miSchoolId);
        ddlGateway.Bind(odt, "Id", "PaymentGateway", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// Popoulates the electronic type dropdown list.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbCardType, "Type", "TypeId", Constants.S_SELECT);
    }

    /// <summary>
    /// Populates the grid with online transactions details.
    /// </summary>
    private void FillOnlineTransactionGrid()
    {
        lblError.Visible = false;
        DataSet odsOnlineTransaction = null;
        DataTable odtOnlineTrasaction = null;

        if (optTransactionNumber.Checked)
        {
            odsOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineTransactionDetail(miSchoolId,
                                                                                                miAcademicYearId,
                                                                                                chkIncludeAll.Checked,
                                                                                                txtTransactionIDNumber.Text.Trim(),
                                                                                                String.Empty,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                chkCautionMoney.Checked,
                                                                                                optInternalFee.Checked,
                                                                                                ddlGateway.SelectedValue.ToInt());
            odtOnlineTrasaction = odsOnlineTransaction.Tables[0];
        }
        else if (optRegNo.Checked)
        {
            odsOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineTransactionDetail(miSchoolId,
                                                                                                miAcademicYearId,
                                                                                                chkIncludeAll.Checked,
                                                                                                String.Empty,
                                                                                                txtRegNo.Text.Trim(),
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                chkCautionMoney.Checked,
                                                                                                optInternalFee.Checked,
                                                                                                ddlGateway.SelectedValue.ToInt());
            odtOnlineTrasaction = odsOnlineTransaction.Tables[0];
        }
        else if (optPaymentDate.Checked)
        {
            odsOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineTransactionDetail(miSchoolId,
                                                                                                miAcademicYearId,
                                                                                                chkIncludeAll.Checked,
                                                                                                String.Empty,
                                                                                                String.Empty,
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                txtPaymentStartDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtPaymentStartDate.Text.Trim().ToDateTime(),
                                                                                                txtPaymentEndDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtPaymentEndDate.Text.Trim().ToDateTime(),
                                                                                                chkCautionMoney.Checked,
                                                                                                optInternalFee.Checked,
                                                                                                ddlGateway.SelectedValue.ToInt());
            odtOnlineTrasaction = odsOnlineTransaction.Tables[0];
        }
        else if (optClearanceDate.Checked)
        {
            odsOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineTransactionDetail(miSchoolId,
                                                                                                miAcademicYearId,
                                                                                                chkIncludeAll.Checked,
                                                                                                String.Empty,
                                                                                                String.Empty,
                                                                                                txtClearanceStartDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtClearanceStartDate.Text.Trim().ToDateTime(),
                                                                                                txtClearanceEndDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtClearanceEndDate.Text.Trim().ToDateTime(),
                                                                                                DateTime.MinValue,
                                                                                                DateTime.MinValue,
                                                                                                chkCautionMoney.Checked,
                                                                                                optInternalFee.Checked,
                                                                                                ddlGateway.SelectedValue.ToInt(),
                                                                                                cmbClearanceBank.SelectedValue.ToInt());
            odtOnlineTrasaction = odsOnlineTransaction.Tables[0];
        }
        if (odsOnlineTransaction == null)
            return;

        if (odtOnlineTrasaction != null)
        {
            grdvwClearedCash.Visible = true;
            SetGridViewDateColumnProperties();
            grdvwClearedCash.DataSource = odtOnlineTrasaction.DefaultView;
            grdvwClearedCash.DataBind();
            hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);
        }
        if (Convert.ToString(odsOnlineTransaction.Tables[1].Rows[0][0]) != string.Empty)
        {
            tblTotalAmount.Visible = true;
            int iTotalAmount = odsOnlineTransaction.Tables[1].Rows[0][0].ToInt();
            lblTotalAmount.Text = iTotalAmount.ToString();
        }
        else
            tblTotalAmount.Visible = false;

        if (odtOnlineTrasaction.Rows.Count == 0)
            trTotalRec.Visible = false;
    }

    /// <summary>
    /// This method is used to fill GridView.
    /// </summary>
    private void FillClearedCashPaymentGrid()
    {
        const int FILTER_BY_STUDENT_NAME_REG_NO = 1;
        const int FILTER_BY_PAID_DATE = 2;
        const int FILTER_BY_CLEARANCE_DATE = 3;
        DataTable odtClearedCash = null;
        int iTotalAmount = 0;
        if (optRegNo.Checked)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(txtRegNo.Text.Trim(), string.Empty, string.Empty, chkIncludeAll.Checked, FILTER_BY_STUDENT_NAME_REG_NO, miSchoolId, miAcademicYearId, out iTotalAmount);
        else if (optPaymentDate.Checked)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, txtPaymentStartDate.Text.Trim(), txtPaymentEndDate.Text.Trim(), chkIncludeAll.Checked, FILTER_BY_PAID_DATE, miSchoolId, miAcademicYearId, out iTotalAmount);
        else if (optClearanceDate.Checked)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, txtClearanceStartDate.Text.Trim(), txtClearanceEndDate.Text.Trim(), chkIncludeAll.Checked, FILTER_BY_CLEARANCE_DATE, miSchoolId, miAcademicYearId, out iTotalAmount, Convert.ToInt32(cmbClearanceBank.SelectedValue));

        if (odtClearedCash != null)
        {
            grdvwClearedCash.Visible = true;
            grdvwClearedCash.DataSource = odtClearedCash.DefaultView;
            grdvwClearedCash.DataBind();
            hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = iTotalAmount.ToString();
        }
        if (odtClearedCash.Rows.Count != 0)
            return;
        trTotalRec.Visible = false;
        tblTotalAmount.Visible = false;
    }

    /// <summary>
    /// This method used to fill the grid according to selected filter.
    /// </summary>
    private void FillCardPaymentsGrid()
    {
        var oCashClearanceListBL = new CashClearanceListBL();
        DataTable oDt = oCashClearanceListBL.GetCardPaymentList(miSchoolId, miAcademicYearId, txtTransactionIDNumber.Text, txtRegNo.Text, txtPaymentStartDate.Text, txtPaymentEndDate.Text,
                                                                     txtClearanceStartDate.Text, txtClearanceEndDate.Text, chkIncludeAll.Checked, cmbCardType.SelectedValue.ToInt(), cmbClearanceBank.SelectedValue.ToInt());
        grdvwClearedCash.Visible = true;
        grdvwClearedCash.DataSource = oDt.DefaultView;
        grdvwClearedCash.DataBind();
        hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);

        miTotalAmount = oCashClearanceListBL.CardPaymentsTotalAmount(miSchoolId, miAcademicYearId, txtRegNo.Text, txtPaymentStartDate.Text, txtPaymentEndDate.Text,
                                                                     txtClearanceStartDate.Text, txtClearanceEndDate.Text, chkIncludeAll.Checked, cmbCardType.SelectedValue.ToInt(), cmbClearanceBank.SelectedValue.ToInt());

        if (miTotalAmount != 0)
        {
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = miTotalAmount.ToString();
        }
        else
            tblTotalAmount.Visible = false;
    }

    /// <summary>
    /// This function is used to fill the electronic payments gridview using the filters applied.
    /// </summary>
    private void FillElectronicPaymentsGrid()
    {
        FeeClearanceFilters oFeeClearanceFilters = new FeeClearanceFilters
        {
            TransactionNumber = txtTransactionIDNumber.Text.Trim(),
            RegNo = txtRegNo.Text.Trim(),
            PaymentStartDate = (!txtPaymentStartDate.Text.IsNullOrEmpty() ? txtPaymentStartDate.Text.ToDateTime() : DateTime.MinValue),
            PaymentEndDate = (!txtPaymentEndDate.Text.IsNullOrEmpty() ? txtPaymentEndDate.Text.ToDateTime() : DateTime.MinValue),
            ClearanceStartDate = (!txtClearanceStartDate.Text.IsNullOrEmpty() ? txtClearanceStartDate.Text.ToDateTime() : DateTime.MinValue),
            ClearanceEndDate = (!txtClearanceEndDate.Text.IsNullOrEmpty() ? txtClearanceEndDate.Text.ToDateTime() : DateTime.MinValue),
            TypeId = cmbCardType.SelectedValue.ToInt(),
            IncludeAll = chkIncludeAll.Checked,
            IncludeCautionMoney = chkCautionMoney.Checked,
            DepositedBankId = cmbClearanceBank.SelectedValue.ToInt()
        };

        List<StudentFeeClearanceDetails> lstFeeClearanceDetails = new List<StudentFeeClearanceDetails>();
        lstFeeClearanceDetails = moStudentFeeDetailsBL.GetElectronicPayments(oFeeClearanceFilters,optInternalFee.Checked);

        var oPaymentDetails = from a in lstFeeClearanceDetails
                              select new
                              {
                                  a.StudentElectronicPaymentId,
                                  a.RegNo,
                                  a.StudentName,                                  
                                  Payable_For = a.oStudentPayFeeDetails.Remarks,
                                  Student_Id = a.oStudentPayFeeDetails.StudentId,
                                  Amount = a.oStudentPayFeeDetails.ActualAmount,
                                  Paid_Date = a.oStudentPayFeeDetails.PaymentDate.ToString("dd-MMM-yyyy"),
                                  ClearanceDate = (a.oFeeClearanceFilters.ClearanceStartDate != DateTime.MinValue.Date ? a.oFeeClearanceFilters.ClearanceStartDate.ToString("dd-MMM-yyyy") : string.Empty),
                                  Is_Deleted = Constants.S_NO,
                                  ClassName = a.Class,
                                  DepositBankId = a.oStudentPayFeeDetails.DepositeBankId,
                                  StudentCardPaymentDetailsId = 0,
                                  TPSLTransactionID = 0,
                                  Cheque_Number = 0,
                                  Bank_Name = string.Empty,
                                  TransactionDateTime = DateTime.Now,
                                  Cheque_Date = string.Empty,
                                  a.Receipt_Number,
                                  PostDated_Cheque_Id = Constants.S_ZERO,
                                  Bank_Id = Constants.S_ZERO,
                                  Payment_Cheque_Id = Constants.S_ZERO,
                                  NetBankingPaymentTransactionID = Constants.S_ZERO,
                                  IsReturnPayment = Constants.S_ZERO,
                                  TransactionNumber = a.TransactionNumber.ToString(),
                                  IsCautionMoneyPayment = a.IsCautionMoneyPayment.ToInt()
                              };
        grdvwClearedCash.Visible = true;
        grdvwClearedCash.DataSource = oPaymentDetails.ToList();
        grdvwClearedCash.DataBind();
        hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);

        if (moStudentFeeDetailsBL.TotalAmount > Constants.I_ZERO)
        {
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = moStudentFeeDetailsBL.TotalAmount.ToString();
        }
        else
            tblTotalAmount.Visible = false;
    }

    /// <summary>
    /// This method used to fill the grid according to selected filter.
    /// </summary>
    private void FillChequesGrid()
    {
        lblError.Visible = false;
        DataTable oDTCheques;
        string sRegNo = txtRegNo.Text.Trim();
        string sChequeNo = txtChequeNumber.Text.Trim();
        int iTotalAmount;

        
            if (!sRegNo.IsNullOrEmpty())
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sRegNo, miSchoolId, miAcademicYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, false,  out iTotalAmount, optInternalFee.Checked);
            else if (!sChequeNo.IsNullOrEmpty())
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sChequeNo, miSchoolId, miAcademicYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, true, out iTotalAmount, optInternalFee.Checked);
            else if (!txtPaymentStartDate.Text.Trim().IsNullOrEmpty() || !txtPaymentEndDate.Text.Trim().IsNullOrEmpty() || cmbPaymentBank.SelectedValue != Constants.S_ZERO)
            {
                DateTime odtStartDate = txtPaymentStartDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtPaymentStartDate.Text.ToDateTime();
                DateTime odtToDate = txtPaymentEndDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtPaymentEndDate.Text.ToDateTime();
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtToDate, miSchoolId, miAcademicYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, true, out iTotalAmount, optInternalFee.Checked, cmbPaymentBank.SelectedValue.ToInt());
            }
            else if (!txtClearanceStartDate.Text.Trim().IsNullOrEmpty() || !txtClearanceEndDate.Text.Trim().IsNullOrEmpty() || cmbClearanceBank.SelectedValue != Constants.S_ZERO)
            {
                DateTime odtStartDate = txtClearanceStartDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtClearanceStartDate.Text.ToDateTime();
                DateTime odtEndDate = txtClearanceEndDate.Text.Trim().IsNullOrEmpty() ? DateTime.MinValue : txtClearanceEndDate.Text.ToDateTime();

                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtEndDate, miSchoolId, miAcademicYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, false, out iTotalAmount, optInternalFee.Checked, cmbClearanceBank.SelectedValue.ToInt());
            }            
            else
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(miSchoolId, miAcademicYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, out iTotalAmount, optInternalFee.Checked);
        

        
        if (oDTCheques != null)
        {
            grdvwClearedCash.Visible = true;
            tblLegend.Visible = chkCautionMoney.Checked;
            SetGridViewDateColumnProperties();            
            grdvwClearedCash.DataSource = oDTCheques.DefaultView;
            grdvwClearedCash.DataBind();            

            hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = iTotalAmount.ToString();
        }
        if (oDTCheques.Rows.Count != 0)
            return;
        trTotalRec.Visible = false;
        tblLegend.Visible = false;
        tblTotalAmount.Visible = false;
    }

    /// <summary>
    /// This method is used to display transaction date and clearance date in proper formate.
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        int iPaymentDateIndex = 0;
        if (optOnlineTransactionClearance.Checked)
            iPaymentDateIndex = 8;
        else if (optChequeClearance.Checked)
            iPaymentDateIndex = 13;
        var oPaymentDate = grdvwClearedCash.Columns[iPaymentDateIndex] as BoundField;
        oPaymentDate.HtmlEncode = false;
        oPaymentDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method used to set the value to the label indicating records from the grid.
    /// </summary>
    private void DisplayRowDetails()
    {
        int iRowCount = 0;
        if (optElectronicPaymentClearance.Checked)
            iRowCount = grdvwClearedCash.Rows.Count;
        else
            iRowCount = ((DataView)grdvwClearedCash.DataSource).Count;
        lblStartIndex.Text = Convert.ToString((grdvwClearedCash.PageSize * grdvwClearedCash.PageIndex) + 1);
        lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdvwClearedCash.PageSize) - 1);
        lblTotal.Text = iRowCount.ToString();
        if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
            lblEndIndex.Text = iRowCount.ToString();
        trTotalRec.Visible = iRowCount.ToString() != "0";
        if (lblTotal.Text == String.Empty)
            return;
        trTotalRec.Visible = lblTotal.Text.ToInt() > Constants.I_GRID_PAGE_COUNT;
    }

    /// <summary>
    /// Hides certain controls.
    /// </summary>
    private void HideControls()
    {
        trTotalRec.Visible = false;
        lblError.Visible = false;
    }

    /// <summary>
    /// This method used to enabled or disabled controls.
    /// </summary>
    private void EnableDisableControls(bool abflag)
    {
        optRegNo.Enabled = abflag;
        optPaymentDate.Enabled = abflag;
        optClearanceDate.Enabled = abflag;
        chkIncludeAll.Enabled = abflag;
        optTransactionNumber.Enabled = abflag;
        cmbCardType.Enabled = abflag;
        optChequeNumber.Enabled = abflag;
        chkCautionMoney.Enabled = abflag;
        ddlGateway.Enabled = abflag;

        optCardClearance.Enabled = abflag;
        optCashClearance.Enabled = abflag;
        optOnlineTransactionClearance.Enabled = abflag;
        optChequeClearance.Enabled = abflag;
        optElectronicPaymentClearance.Enabled = abflag;

        optStudentFee.Enabled = abflag;
        optInternalFee.Enabled = abflag;
    }

    /// <summary>
    /// This method used to enabled or disabled radio button controls.
    /// </summary>
    private void EnableDisableControlChecked(bool abFlag)
    {
        if (optRegNo.Checked)
            txtRegNo.Enabled = abFlag;
        else if (optPaymentDate.Checked)
        {
            txtPaymentStartDate.Enabled = abFlag;
            txtPaymentEndDate.Enabled = abFlag;
            cmbPaymentBank.Enabled = abFlag;
        }
        else if (optClearanceDate.Checked)
        {
            txtClearanceStartDate.Enabled = abFlag;
            txtClearanceEndDate.Enabled = abFlag;
            cmbClearanceBank.Enabled = abFlag;
        }
        else if (optTransactionNumber.Checked)
            txtTransactionIDNumber.Enabled = abFlag;       
        else if (optChequeClearance.Checked)
            txtChequeNumber.Enabled = abFlag;
        
    }

    /// <summary>
    /// This method is used to clear texts.
    /// </summary>
    private void ClearTextboxes()
    {
        txtRegNo.Text = string.Empty;
        txtPaymentStartDate.Text = string.Empty;
        txtPaymentEndDate.Text = string.Empty;
        txtClearanceStartDate.Text = string.Empty;
        txtClearanceEndDate.Text = string.Empty;
        txtTransactionIDNumber.Text = string.Empty;
        txtChequeNumber.Text = string.Empty;
    }

    /// <summary>
    /// This method is used set controls when RegNo radio button checked.
    /// </summary>
    private void optRegNoChecked()
    {
        txtRegNo.Focus();
        ClearTextboxes();
        txtRegNo.Enabled = true;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
        txtTransactionIDNumber.Enabled = false;
        cmbClearanceBank.Enabled = false;
        cmbPaymentBank.Enabled = false;
    }

    /// <summary>
    /// This method used to set cheque number filter as well as to enabled or disabled controls according to that.
    /// </summary>
    private void OptCheckedNumberChecked()
    {
        ClearTextboxes();
        txtChequeNumber.Enabled = true;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtRegNo.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
        txtTransactionIDNumber.Enabled = false;
        cmbClearanceBank.Enabled = false;
        cmbPaymentBank.Enabled = false;
    }

    /// <summary>
    /// This method is used set controls when PaymentDate radio button checked.
    /// </summary>
    private void optPaymentDateChecked()
    {
        ClearTextboxes();
       
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = true;
        txtPaymentEndDate.Enabled = true;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
        txtTransactionIDNumber.Enabled = false;
        txtChequeNumber.Enabled = false;
        cmbClearanceBank.SelectedIndex = Constants.I_ZERO;
        cmbClearanceBank.Enabled = false;
        cmbPaymentBank.Enabled = true;
    }

    /// <summary>
    /// This method is used set controls when learanceDate radio button checked.
    /// </summary>
    private void optClearanceDateChecked()
    {
        ClearTextboxes();
       
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = true;
        txtClearanceEndDate.Enabled = true;
        chkIncludeAll.Checked = true;
        txtTransactionIDNumber.Enabled = false;
        txtChequeNumber.Enabled = false;
        cmbClearanceBank.Enabled = true;
        cmbPaymentBank.SelectedIndex = Constants.I_ZERO;
        cmbPaymentBank.Enabled = false;
    }

    /// <summary>
    /// Sets controls visibility when transaction id radio button is clicked.
    /// </summary>
    private void OptTrasactionIdCheck()
    {
        ClearTextboxes();
        txtClearanceEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtTransactionIDNumber.Enabled = true;
        chkIncludeAll.Checked = false;
        txtChequeNumber.Enabled = false;
        cmbClearanceBank.Enabled = false;
        cmbPaymentBank.Enabled = false;
    }

    /// <summary>
    /// This method is used to Initialize controls.
    /// </summary>
    private void InitializeControls()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdvwClearedCash.PageSize = Constants.I_GRID_PAGE_COUNT;
        hidPageNo.Value = HIDE_PAGE_NUMBER;
        hidServerDate.Value = DateTime.Today.ToString();

        if (!Settings.EnabledOnlineFee)
            optOnlineTransactionClearance.Visible = false;

        trCautionMoney.Visible = Settings.IsCautionMoneyApplicable;
    }

    /// <summary>
    /// Creates an XML string for Online transaction details.
    /// </summary>
    /// <returns>An XML string representing the online transaction clearance details.</returns>
    private string GenerateOnlineTransactionXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("OnlineTrasactionInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "OnlineTrasactionInfo", String.Empty);
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[11].FindControl("txtclearance") as TextBox;
            var otxtTSPLTransactionID = grdvwClearedCash.Rows[i].Cells[3].FindControl("txtTSPLTransactionID") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "OnlineTrasactionInfo", String.Empty);

            sAttribute = "NetBankingPaymentTransactionID";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["NetBankingPaymentTransactionID"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "TPSLTransactionID";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtTSPLTransactionID.Text.Trim();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "DepositBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            oXmlRootNode.AppendChild(oXMLNode);
        }
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }

    /// <summary>
    /// This method is used to collect paramters and send it to Stored procedure.
    /// </summary>
    /// <returns>An XML string representing the cash payment clearance details.</returns>
    private string GenerateCashClearanceXML()
    {
        const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 5;
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("ClearedCashInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ClearedCashInfo", String.Empty);
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance") as TextBox;
            var otxtPaidDate = grdvwClearedCash.Rows[i].FindControl("txtPaidDate") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "ClearedCashInfo", String.Empty);

            sAttribute = "Receipt_Number";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Receipt_Number"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "PaidDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtPaidDate.Text.Trim() != String.Empty ? otxtPaidDate.Text.Trim() : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "SchoolId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miSchoolId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "AcademicYearId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miAcademicYearId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Insert_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Inserted_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);
            oXmlRootNode.AppendChild(oXMLNode);

            sAttribute = "DepositBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);
        }
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }

    /// <summary>
    /// This method is used to collect paramters and send it to Stored procedure.
    /// </summary>
    /// <returns>An XML string representing the Card payment clearance details.</returns>
    private string GenerateCardPaymentXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("ClearedCardPaymentInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ClearedCardPaymentInfo", String.Empty);
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[11].FindControl("txtclearance") as TextBox;
            var otxtPaidDate = grdvwClearedCash.Rows[i].FindControl("txtPaidDate") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "ClearedCardPaymentInfo", String.Empty);

            sAttribute = "StudentCardPaymentDetailsId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["StudentCardPaymentDetailsId"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : String.Empty;
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "PaidDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtPaidDate.Text.Trim() != String.Empty ? otxtPaidDate.Text.Trim() : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "SchoolId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miSchoolId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "AcademicYearId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miAcademicYearId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Insert_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Inserted_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);
            oXmlRootNode.AppendChild(oXMLNode);

            sAttribute = "DepositBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);
        }
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }

    /// <summary>
    ///  This XML is used to set the parameters to clear cheque and caution money details.
    /// </summary>
    /// <returns>An XML string representing the caution payment clearance details.</returns>
    private string GenerateCautionMoneyXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement("StudentInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", String.Empty);
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[11].FindControl("txtclearance") as TextBox;
            var otxtChequeNo = grdvwClearedCash.Rows[i].Cells[4].FindControl("txtChequeNo") as TextBox;
            var otxtChequeDate = grdvwClearedCash.Rows[i].Cells[9].FindControl("txtChequeDate") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", String.Empty);

            sAttribute = "BankId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Bank_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "ChequeNo";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtChequeNo.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Student_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Student_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "PostDated_Cheque_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["PostDated_Cheque_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Payment_Cheque_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Payment_Cheque_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Insert_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Inserted_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Cheque_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtChequeDate.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : DBNull.Value.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "DepositBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXmlNode.Attributes.Append(oAttr);

            oXmlRootNode.AppendChild(oXmlNode);
        }
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// Creates an XML string for Cheque payment details.
    /// </summary>
    /// <returns>An XML string representing the cheque payment clearance details.</returns>
    private string GenerateChequePaymentXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("StudentInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", String.Empty);
        // Loop through all the grid rows.
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[11].FindControl("txtclearance") as TextBox;
            var otxtChequeNo = grdvwClearedCash.Rows[i].Cells[4].FindControl("txtChequeNo") as TextBox;
            var otxtChequeDate = grdvwClearedCash.Rows[i].Cells[9].FindControl("txtChequeDate") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", String.Empty);

            sAttribute = "BankId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Bank_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "ChequeNo";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtChequeNo.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Student_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["Student_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "PostDated_Cheque_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["PostDated_Cheque_Id"].ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "AcademicYearId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miAcademicYearId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = DateTime.Now.ToString("dd-MMM-yyyy", new CultureInfo("en"));
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Cheque_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtChequeDate.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "Cheque_Passed_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : DBNull.Value.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "DepositBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXmlNode.Attributes.Append(oAttr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

        }

        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetClientScriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnShow, btnSave, btnExport });
        optRegNo.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optClearanceDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optPaymentDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");

        //btnExportFee.Visible = false;
        //if (miSchoolId == Constants.SchoolId.DSK.ToInt())
        //    btnExportFee.Visible = true;
    }

    /// <summary>
    /// Saves cheque payments selected for clearance.
    /// </summary>
    private void SaveChequePayments()
    {
        if (chkCautionMoney.Checked == false)
        {
            string sXML = GenerateChequePaymentXML();
            var oUpdateStudentPostDatedCheques = new StudentPostDatedChequesBL();
            DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML, optInternalFee.Checked);
            if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
            {
                oUpdateStudentPostDatedCheques.SetChequeClearanceDate(sXML, optInternalFee.Checked);
                lblError.Visible = false;
                lblSuccessMsg.Visible = true;
                lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";

                if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
                    RecordPayment(Constants.PaymentMode.Cheque);

                FillChequesGrid();
            }
            else
            {
                string sMessage = string.Empty;
                for (int i = 0; i < oDTMessage.Rows.Count; i++)
                    sMessage = string.Format("{0}, {1}", sMessage, Convert.ToString(oDTMessage.Rows[i]["Name"]));

                lblError.Visible = true;
                lblSuccessMsg.Visible = false;
                lblError.Text = string.Format("Cheque Number already exists for student(s) {0}.", sMessage.Substring(1, sMessage.Length - 1));
            }
        }
        else
        {
            string sXML = GenerateCautionMoneyXML();
            var oUpdateStudentPostDatedCheques = new StudentPostDatedChequesBL();
            DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML, optInternalFee.Checked);
            if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
            {
                oUpdateStudentPostDatedCheques.SetCautionClearanceDate(sXML);
                lblError.Visible = false;
                lblSuccessMsg.Visible = true;
                lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";

                if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
                    RecordPayment(Constants.PaymentMode.Cheque);

                FillChequesGrid();
            }
            else
            {
                string sMessage = string.Empty;
                for (int i = 0; i < oDTMessage.Rows.Count; i++)
                    sMessage = string.Format("{0}, {1}", sMessage, Convert.ToString(oDTMessage.Rows[i]["Name"]));

                lblError.Visible = true;
                lblSuccessMsg.Visible = false;
                lblError.Text = string.Format("Cheque Number already exists for student(s) {0}.", sMessage.Substring(1, sMessage.Length - 1));
            }
        }
    }

    /// <summary>
    /// Saves card payments selected for clearance.
    /// </summary>
    private void SaveCardPayments()
    {
        string sXML = GenerateCardPaymentXML();
        CashClearanceListBL.UpdateCardPaymentsDetails(sXML);
        lblError.Visible = false;
        lblSuccessMsg.Visible = true;
        lblSuccessMsg.Text = "Swipe Card Clearance data updated successfully !!!";

        if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
            RecordPayment(Constants.PaymentMode.Card);

        FillCardPaymentsGrid();
    }

    /// <summary>
    /// This method is used to save electronic payment details.
    /// </summary>
    private void SaveElectronicPayments()
    {
        if (!chkCautionMoney.Checked)
            moStudentFeeDetailsBL.UpdateElectronicPaymentClearance(GetElectronicDetailsXML(), optInternalFee.Checked);
        else
            moStudentFeeDetailsBL.UpdateElectronicPaymentCautionMoneyClearance(GetElectronicDetailsXML());

        lblError.Visible = false;
        lblSuccessMsg.Visible = true;
        lblSuccessMsg.Text = "Electronic Payment Clearance data updated successfully !!!";

        if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
            RecordPayment(Constants.PaymentMode.Electronic);

        FillElectronicPaymentsGrid();
    }

    /// <summary>
    /// This method is used to generate electronic details xml for updating clearance data.
    /// </summary>
    private string GetElectronicDetailsXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("ElectronicPaymentDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ElectronicPaymentDetails", String.Empty);
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var otxtClearanceDate = grdvwClearedCash.Rows[i].Cells[11].FindControl("txtclearance") as TextBox;
            var otxtPaidDate = grdvwClearedCash.Rows[i].FindControl("txtPaidDate") as TextBox;
            var ddlDepositBankList = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;
            var otxtTransactionNumber = grdvwClearedCash.Rows[i].FindControl("txtTransactionNumber") as TextBox;

            int iIsCautionMoney = grdvwClearedCash.DataKeys[i]["IsCautionMoneyPayment"].ToInt();
            int iStudentId = grdvwClearedCash.DataKeys[i]["Student_Id"].ToInt();

            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "ElectronicPaymentDetails", String.Empty);

            sAttribute = "ElectronicPaymentId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwClearedCash.DataKeys[i]["StudentElectronicPaymentId"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : String.Empty;
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "PaidDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtPaidDate.Text.Trim() != String.Empty ? otxtPaidDate.Text.Trim() : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "DepositedBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositBankList != null ? ddlDepositBankList.SelectedValue : DBNull.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "TransactionNumber";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtTransactionNumber.Text.Trim() != String.Empty ? otxtTransactionNumber.Text.Trim() : String.Empty;
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "IsCautionMoneyPayment";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iIsCautionMoney.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "StudentId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iStudentId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXMLNode);
        }

        // Add the root node to document element.         
        oElement.AppendChild(oXmlRootNode);
        // return the string generated.
        return oElement.InnerXml;
    }

    /// <summary>
    /// Saves online transactions selected for clearance.
    /// </summary>
    private void SaveOnlineTrasactionPayments()
    {
        string sOnlineTrasactionXML = GenerateOnlineTransactionXML();
        var oUpdateNetBankingPaymentTransactions = new NetBankingPaymentTransactionsBL();
        DataTable oDTMessage = NetBankingPaymentTransactionsBL.IsTSPLIDuplicate(sOnlineTrasactionXML);
        if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
        {
            oUpdateNetBankingPaymentTransactions.SetOnlineTransactionDetails(sOnlineTrasactionXML);
            lblError.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = "Online transaction clearance data updated successfully !!!";

            if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
                RecordPayment(Constants.PaymentMode.Online);

            FillOnlineTransactionGrid();
        }
        else
        {
            var msMessage = new StringBuilder();
            for (int i = 0; i < oDTMessage.Rows.Count; i++)
                msMessage.AppendFormat(", {0}", Convert.ToString(oDTMessage.Rows[i]["StudentName"]));
            lblError.Visible = true;
            lblSuccessMsg.Visible = false;
            lblError.Text = string.Format("TPSLTransactionID should not be duplicated for student(s): {0}.", msMessage.Remove(0, 1));
        }
    }

    /// <summary>
    /// Saves cash payments selected for clearance.
    /// </summary>
    private void SaveCashPayments()
    {
        string sXML = GenerateCashClearanceXML();
        var oUpdateStudentClearedCashPayment = new CashClearanceListBL();
        oUpdateStudentClearedCashPayment.UpdateCashClearanceDate(sXML);
        lblError.Visible = false;
        lblSuccessMsg.Visible = true;
        lblSuccessMsg.Text = "Cash Clearance data updated successfully !!!";

        if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
            RecordPayment(Constants.PaymentMode.Cash);

        FillClearedCashPaymentGrid();
    }

        
    /// <summary>
    /// Returns the list of bank accounts configured in the accounts module.
    /// </summary>
    /// <returns>A List of BankAccountDetails entity objects.</returns>
    private List<BankAccount> GetBankList()
    {
        if (mlstBanks == null)
        {
            BankAccountClient oBankClient = null;
            try
            {
                oBankClient = new BankAccountClient();
                oBankClient.Open();
                mlstBanks = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an error fetching Bank details.");
            }
            finally
            {
                if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
                    oBankClient.Close();
            }
        }

        return mlstBanks;
    }

    /// <summary>
    /// Records cleared payment details in the accounts module.
    /// </summary>
    /// <param name="aePaymentMode"></param>
    private void RecordPayment(Constants.PaymentMode aePaymentMode)
    {
        if (UpdateInternalFeeinDayBook)
        {
            AccountVoucherClient oVoucherClient = null;
            try
            {
                oVoucherClient = new AccountVoucherClient();
                oVoucherClient.Open();

                if (optStudentFee.Checked)
                    oVoucherClient.CreateFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, GetXMLFromGrid(aePaymentMode), aePaymentMode);
                else
                    oVoucherClient.CreateInternalFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, GetXMLFromGrid(aePaymentMode), aePaymentMode);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format("Accounts Module : An exception occured while processing {0}.",
                                                                         aePaymentMode == Constants.PaymentMode.Online ? "online transactions" : String.Format("{0} payments", aePaymentMode)));
            }
            finally
            {
                if (oVoucherClient.State != CommunicationState.Faulted)
                    oVoucherClient.Close();
            }
        }
    }

    /// <summary>
    /// Returns details about cleared transactions as an xml string.
    /// </summary>
    /// <param name="aePaymentMode"></param>
    /// <returns>An XML string representing the clearance details for the specified mode of payment.</returns>
    private string GetXMLFromGrid(Constants.PaymentMode aePaymentMode)
    {
        const string S_ELEMENT = "element";

        var oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("ClearanceInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ClearanceInfo", String.Empty);

        // Loop through all the grid rows.
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            var txtClearanceDate = grdvwClearedCash.Rows[i].FindControl("txtclearance") as TextBox;
            var ddlDepositBank = grdvwClearedCash.Rows[i].FindControl("ddlDepositedBankList") as DropDownList;

            bool bIsUncleared = txtClearanceDate.Text.Trim().IsNullOrEmpty();
            bool bIsCautionMoney = false;
            bool bIsReturnPayment = false;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ClearanceInfo", String.Empty);
            XmlAttribute oXmlAttr = oDoc.CreateAttribute("TransId");

            switch (aePaymentMode)
            {
                case Constants.PaymentMode.Cash:
                    oXmlAttr.Value = grdvwClearedCash.DataKeys[i]["Receipt_Number"].ToString();
                    break;
                case Constants.PaymentMode.Cheque:
                    int iPDCId = grdvwClearedCash.DataKeys[i]["PostDated_Cheque_Id"].ToInt();
                    int iPaymentId = grdvwClearedCash.DataKeys[i]["Payment_Cheque_Id"].ToInt();
                    bIsCautionMoney = iPDCId == Constants.I_ZERO && iPaymentId != Constants.I_ZERO;
                    bIsReturnPayment = bIsCautionMoney && grdvwClearedCash.DataKeys[i]["IsReturnPayment"].ToBool();
                    oXmlAttr.Value = (bIsCautionMoney ? iPaymentId : iPDCId).ToString();
                    break;
                case Constants.PaymentMode.Card:
                    oXmlAttr.Value = grdvwClearedCash.DataKeys[i]["StudentCardPaymentDetailsId"].ToString();
                    break;
                case Constants.PaymentMode.Online:
                    int iIsCautionMoney = grdvwClearedCash.DataKeys[i]["IsCautionMoneyPayment"].ToInt();
                    if (iIsCautionMoney == Constants.I_ONE)
                        bIsCautionMoney = true;

                    oXmlAttr.Value = grdvwClearedCash.DataKeys[i]["NetBankingPaymentTransactionID"].ToString();
                    break;
                case Constants.PaymentMode.Electronic:
                    int iIsCautionMoneyPayment = grdvwClearedCash.DataKeys[i]["IsCautionMoneyPayment"].ToInt();
                    if (iIsCautionMoneyPayment == Constants.I_ONE)
                        bIsCautionMoney = true;
                    oXmlAttr.Value = grdvwClearedCash.DataKeys[i]["StudentElectronicPaymentId"].ToString();
                    break;
            }

            oXmlNode.Attributes.Append(oXmlAttr);

            oXmlAttr = oDoc.CreateAttribute("ClearanceDate");
            oXmlAttr.Value = txtClearanceDate.Text.Trim();
            oXmlNode.Attributes.Append(oXmlAttr);

            oXmlAttr = oDoc.CreateAttribute("DepositBankId");
            oXmlAttr.Value = bIsUncleared ? Constants.S_ZERO : ddlDepositBank.SelectedValue;
            oXmlNode.Attributes.Append(oXmlAttr);

            oXmlAttr = oDoc.CreateAttribute("IsCautionMoney");
            oXmlAttr.Value = bIsCautionMoney ? Constants.S_ONE : Constants.S_ZERO;
            oXmlNode.Attributes.Append(oXmlAttr);

            oXmlAttr = oDoc.CreateAttribute("IsReturnPayment");
            oXmlAttr.Value = bIsReturnPayment ? Constants.S_ONE : Constants.S_ZERO;
            oXmlNode.Attributes.Append(oXmlAttr);

            oXmlAttr = oDoc.CreateAttribute("IsElectronicPayment");
            oXmlAttr.Value = optElectronicPaymentClearance.Checked ? Constants.S_ONE : Constants.S_ZERO;
            oXmlNode.Attributes.Append(oXmlAttr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }

        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref cmbPaymentBank, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_ALL);

        cmbClearanceBank.Bind(GetBankList(), "Id", "Name", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to add node.
    /// </summary>
    /// <param name="aoXmlFee"></param>
    /// <param name="aiParentId"></param>
    /// <param name="alstFeeDetails"></param>
    /// <param name="aoDoc"></param>
    /// <param name="aiStudentId"></param>
    private void AddNodes(XmlNode aoXmlFee, int aiParentId, List<FeeDetailsToExport> alstFeeDetails, XmlDocument aoDoc, int aiStudentId)
    {
        alstFeeDetails.Where(fee => fee.ParentId == aiParentId && fee.StudentId == aiStudentId).ToList()
            .ForEach(
            fee =>
            {
                var oXmlFee = aoDoc.CreateNode(S_ELEMENT, fee.Field, string.Empty);
                oXmlFee.InnerText = fee.Value;

                if (fee.Field == "OLDAUDITENTRYIDS.LIST")
                    AddAttribute("TYPE", "Number", oXmlFee, aoDoc);

                if (alstFeeDetails.Any(fd => fd.ParentId == fee.Id))
                    AddNodes(oXmlFee, fee.Id, alstFeeDetails, aoDoc, fee.StudentId);

                aoXmlFee.AppendChild(oXmlFee);
            }
            );
    }

    /// <summary>
    /// This method is used to add attribute.
    /// </summary>
    /// <param name="asName"></param>
    /// <param name="asValue"></param>
    /// <param name="aoXmlNode"></param>
    /// <param name="aoDoc"></param>
    private void AddAttribute(string asName, string asValue, XmlNode aoXmlNode, XmlDocument aoDoc)
    {
        XmlAttribute oXmlAttribute = aoDoc.CreateAttribute(asName);
        oXmlAttribute.Value = asValue;
        aoXmlNode.Attributes.Append(oXmlAttribute);
    }

    #endregion -- PRIVATE METHOD(s) --

    #region -- EXPORT FUNCTIONALITY --

    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string GetFilterChequePaymentString()
    {
        string sSchoolYearFilter = String.Empty;
        string sViewNameSchID = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".iSchoolId}";
        string sViewNameAcdYearId = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".iAcademicYrId}";
        string sViewNameIsChequeClear = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsChequeClear}";
        string sViewNameChequeNo = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".ChequeNo}";
        string sViewNameStartDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".StartDate}";
        string sViewNameEndDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".EndDate}";
        string sViewNameRegNo = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".RegNo}";
        string sViewNameIsChequeClearanceDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsChequeClearanceDate}";
        string sViewNameIsCautionClear = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsCautionClearanceDate}";
        string sViewNameIsInternalFee = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsInternalFee}";
        
        if (optChequeNumber.Checked)
            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}= AND {7}= null  AND {8}= null  AND {9}={10}AND {11}={12}AND {13}={14} AND{15}={16})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameChequeNo,
                                                txtChequeNumber.Text.Trim(),
                                                sViewNameRegNo,
                                                sViewNameStartDate,
                                                sViewNameEndDate,
                                                sViewNameIsChequeClear,
                                                chkIncludeAll.Checked,
                                                sViewNameIsChequeClearanceDate,
                                                optClearanceDate.Checked,
                                                sViewNameIsCautionClear,
                                                chkCautionMoney.Checked,
                                                sViewNameIsInternalFee,
                                                optInternalFee.Checked
                                                );
        else if (optPaymentDate.Checked)
            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7} AND {8}=  AND {9}= AND {10}={11}AND {12}={13}AND {14}={15} AND {16}={17})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameStartDate,
                                                (txtPaymentStartDate.Text.Trim() == string.Empty ? "null" : txtPaymentStartDate.Text.Trim()),
                                                sViewNameEndDate,
                                                (txtPaymentEndDate.Text.Trim() == string.Empty ? "null" : txtPaymentEndDate.Text.Trim()),
                                                sViewNameChequeNo,
                                                sViewNameRegNo,
                                                sViewNameIsChequeClear,
                                                chkIncludeAll.Checked,
                                                sViewNameIsChequeClearanceDate,
                                                optClearanceDate.Checked,
                                                sViewNameIsCautionClear,
                                                chkCautionMoney.Checked,
                                                sViewNameIsInternalFee,
                                                optInternalFee.Checked.ToInt()                                                
                                                );
        else if (optRegNo.Checked)
            sSchoolYearFilter = String.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}= null AND {7}= null AND {8}= AND {9}={10}AND {11}={12}AND {13}={14} AND {15}={16})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameRegNo,
                                                txtRegNo.Text.Trim(),
                                                sViewNameStartDate,
                                                sViewNameEndDate,
                                                sViewNameChequeNo,
                                                sViewNameIsChequeClear,
                                                chkIncludeAll.Checked,
                                                sViewNameIsChequeClearanceDate,
                                                optClearanceDate.Checked,
                                                sViewNameIsCautionClear,
                                                chkCautionMoney.Checked,
                                                sViewNameIsInternalFee,
                                                optInternalFee.Checked.ToInt());
        else if (optClearanceDate.Checked)
            sSchoolYearFilter = String.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7} AND {8}=  AND {9}= AND {10}={11}AND {12}={13}AND {14}={15} AND {16}={17})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameStartDate,
                                                (txtClearanceStartDate.Text.Trim() == string.Empty ? "null" : txtClearanceStartDate.Text.Trim()),
                                                sViewNameEndDate,
                                                (txtClearanceEndDate.Text.Trim() == string.Empty ? "null" : txtClearanceEndDate.Text.Trim()),
                                                sViewNameChequeNo,
                                                sViewNameRegNo,
                                                sViewNameIsChequeClear,
                                                chkIncludeAll.Checked,
                                                sViewNameIsChequeClearanceDate,
                                                optClearanceDate.Checked,
                                                sViewNameIsCautionClear,
                                                chkCautionMoney.Checked,
                                                sViewNameIsInternalFee,
                                                optInternalFee.Checked.ToInt());
        else if (txtRegNo.Text == String.Empty && txtPaymentStartDate.Text == String.Empty &&
                    txtPaymentEndDate.Text == String.Empty && txtClearanceStartDate.Text == String.Empty &&
                        txtPaymentEndDate.Text == String.Empty && txtChequeNumber.Text == String.Empty)
            sSchoolYearFilter = String.Format("({0}={1} AND {2}={3} AND {4}= AND {5}= null AND {6}= null AND {7}= AND {8}={9}AND {10}={11}AND {12}={13} AND {14}={15})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameRegNo,
                                                sViewNameStartDate,
                                                sViewNameEndDate,
                                                sViewNameChequeNo,
                                                sViewNameIsChequeClear,
                                                chkIncludeAll.Checked,
                                                sViewNameIsChequeClearanceDate,
                                                optClearanceDate.Checked,
                                                sViewNameIsCautionClear,
                                                chkCautionMoney.Checked,
                                                sViewNameIsInternalFee,
                                                optInternalFee.Checked.ToInt());

        return sSchoolYearFilter + "@ ";
    }

    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string GetCashClearanceFilterString()
    {
        string sSchoolYearFilter = String.Empty;
        string sViewNameSchID = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".SchoolId}";
        string sViewNameAcdYearId = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".Academic_Year_Id}";
        string sViewNameRegNo = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".RegNo}";
        string sViewNamePaymentStartDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".PaymentStartDate}";
        string sViewNamePaymentEndDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".PaymentEndDate}";
        string sViewNameClearanceStartDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".ClearanceStartDate}";
        string sViewNameClearanceEndDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".ClearanceEndDate}";
        string sViewNameIncldeCheck = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".abIncldeCheck}";

        if (optPaymentDate.Checked)
            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7} AND {8}=null AND {9}=null AND {10}=null AND {11}={12})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNamePaymentStartDate,
                                                txtPaymentStartDate.Text.Trim() == string.Empty ? "null" : txtPaymentStartDate.Text.Trim(),
                                                sViewNamePaymentEndDate,
                                                txtPaymentEndDate.Text.Trim() == string.Empty ? "null" : txtPaymentEndDate.Text.Trim(),
                                                sViewNameRegNo,
                                                sViewNameClearanceStartDate,
                                                sViewNameClearanceEndDate,
                                                sViewNameIncldeCheck,
                                                chkIncludeAll.Checked);
        else if (optRegNo.Checked)
            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}=null AND {7}=null AND {8}=null AND {9}=null AND {10}={11})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameRegNo,
                                                txtRegNo.Text.Trim() == string.Empty ? String.Empty : txtRegNo.Text.Trim(),
                                                sViewNamePaymentStartDate,
                                                sViewNamePaymentEndDate,
                                                sViewNameClearanceStartDate,
                                                sViewNameClearanceEndDate,
                                                sViewNameIncldeCheck,
                                                chkIncludeAll.Checked);
        else if (optClearanceDate.Checked)
            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}=null AND {7}=null AND {8}={9} AND {10}={11} AND {12}={13})",
                                                sViewNameSchID,
                                                miSchoolId,
                                                sViewNameAcdYearId,
                                                miAcademicYearId,
                                                sViewNameRegNo,
                                                txtRegNo.Text.Trim() == string.Empty ? "null" : txtRegNo.Text.Trim(),
                                                sViewNamePaymentStartDate,
                                                sViewNamePaymentEndDate,
                                                sViewNameClearanceStartDate,
                                                txtClearanceStartDate.Text.Trim() == string.Empty ? "null" : txtClearanceStartDate.Text.Trim(),
                                                sViewNameClearanceEndDate,
                                                txtClearanceEndDate.Text.Trim() == string.Empty ? "null" : txtClearanceEndDate.Text.Trim(),
                                                sViewNameIncldeCheck,
                                                chkIncludeAll.Checked);

        return sSchoolYearFilter + "@ ";
    }

    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string GetCardClearanceFilterString()
    {
        //string sFilterString = String.Empty;
        var sbFilterString = new StringBuilder();
        string sViewNameSchoolId = Constants.S_EXPORT_CARDPAYMENTS_USP + ".SchoolId}";
        string sViewNameAcademic_Year_Id = Constants.S_EXPORT_CARDPAYMENTS_USP + ".Academic_Year_Id}";
        string sViewNameTransactionNumber = Constants.S_EXPORT_CARDPAYMENTS_USP + ".TransactionNumber}";
        string sViewNameRegNo = Constants.S_EXPORT_CARDPAYMENTS_USP + ".RegNo}";
        string sViewNamePaymentStartDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".PaymentStartDate}";
        string sViewNamePaymentEndDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".PaymentEndDate}";
        string sViewNameClearanceStartDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".ClearanceStartDate}";
        string sViewNameClearanceEndDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".ClearanceEndDate}";
        string sViewNameIncludeAll = Constants.S_EXPORT_CARDPAYMENTS_USP + ".IncludeAll}";
        string sViewNameCardTypeId = Constants.S_EXPORT_CARDPAYMENTS_USP + ".CardTypeId}";

        const string S_FORMAT_STRING = " AND {0}={1}";

        sbFilterString.AppendFormat("{0}={1}", sViewNameSchoolId, miSchoolId);
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameAcademic_Year_Id, miAcademicYearId);
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameTransactionNumber, txtTransactionIDNumber.Text.Trim().IsNullOrEmpty() ? " null" : txtTransactionIDNumber.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameRegNo, txtRegNo.Text.Trim().IsNullOrEmpty() ? " null" : txtRegNo.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNamePaymentStartDate, txtPaymentStartDate.Text.Trim().IsNullOrEmpty() ? " null" : txtPaymentStartDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNamePaymentEndDate, txtPaymentEndDate.Text.Trim().IsNullOrEmpty() ? " null" : txtPaymentEndDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameClearanceStartDate, txtClearanceStartDate.Text.Trim().IsNullOrEmpty() ? " null" : txtClearanceStartDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameClearanceEndDate, txtClearanceEndDate.Text.Trim().IsNullOrEmpty() ? " null" : txtClearanceEndDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameIncludeAll, chkIncludeAll.Checked ? "1" : "0");
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameCardTypeId, cmbCardType.SelectedValue);

        return "(" + sbFilterString + ")@ ";
    }

    /// <summary>
    /// This method generates the report filter as per the field selection for electronic payments.
    /// </summary>
    /// <returns></returns>
    private string GetElectronicClearanceFilter()
    {
        //string sFilterString = String.Empty;
        var sbFilterString = new StringBuilder();
        string sViewNameSchoolId = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".SchoolId}";
        string sViewNameAcademic_Year_Id = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".Academic_Year_Id}";
        string sViewNameTransactionNumber = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".TransactionNumber}";
        string sViewNameRegNo = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".RegNo}";
        string sViewNamePaymentStartDate = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".PaymentStartDate}";
        string sViewNamePaymentEndDate = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".PaymentEndDate}";
        string sViewNameClearanceStartDate = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".ClearanceStartDate}";
        string sViewNameClearanceEndDate = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".ClearanceEndDate}";
        string sViewNameIncludeAll = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".IncludeAll}";
        string sViewNameTypeId = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".TypeId}";
        string sViewNameDepositeBankId = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".DepositBankId}";
        string sViewNameIncludeCautionMoney = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".IncludeCautionMoney}";//new add
        string sViewNameIsInternalFee = Constants.S_EXPORT_ELECTRONICPAYMENTS_USP + ".IsInternalFee}";//// new add
        const string S_FORMAT_STRING = " AND {0}={1}";

        sbFilterString.AppendFormat("{0}={1}", sViewNameSchoolId, miSchoolId);
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameAcademic_Year_Id, miAcademicYearId);
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameTransactionNumber, txtTransactionIDNumber.Text.Trim().IsNullOrEmpty() ? " null" : txtTransactionIDNumber.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameRegNo, txtRegNo.Text.Trim().IsNullOrEmpty() ? " null" : txtRegNo.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNamePaymentStartDate, txtPaymentStartDate.Text.Trim().IsNullOrEmpty() ? " null" : txtPaymentStartDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNamePaymentEndDate, txtPaymentEndDate.Text.Trim().IsNullOrEmpty() ? " null" : txtPaymentEndDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameClearanceStartDate, txtClearanceStartDate.Text.Trim().IsNullOrEmpty() ? " null" : txtClearanceStartDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameClearanceEndDate, txtClearanceEndDate.Text.Trim().IsNullOrEmpty() ? " null" : txtClearanceEndDate.Text.Trim());
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameIncludeAll, chkIncludeAll.Checked ? "1" : "0");
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameTypeId, cmbCardType.SelectedValue);
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameDepositeBankId, "0");
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameIncludeCautionMoney, chkCautionMoney.Checked ? "1" : "0");
        sbFilterString.AppendFormat(S_FORMAT_STRING, sViewNameIsInternalFee, optInternalFee.Checked ? "1" : "0");

        return "(" + sbFilterString + ")@ ";
    }

    /// <summary>
    /// Returns the filter string for online transactions.
    /// 
    /// </summary>
    /// <returns></returns>
    private string GetOnlineTransactionFilterString()
    {
        int iSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
        int iAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();

        string sDateFilterType = String.Empty;
        string sStartDate = String.Empty;
        string sEndDate = String.Empty;
        string sDepositedBankId = Constants.S_ZERO;

        if (!txtClearanceStartDate.Text.Trim().IsNullOrEmpty() || !txtClearanceEndDate.Text.Trim().IsNullOrEmpty())
        {
            sDateFilterType = "Clearance";
            sStartDate = txtClearanceStartDate.Text.Trim();
            sEndDate = txtClearanceEndDate.Text.Trim();
            sDepositedBankId = cmbClearanceBank.SelectedValue;
        }
        else if (!txtPaymentStartDate.Text.Trim().IsNullOrEmpty() || !txtPaymentEndDate.Text.Trim().IsNullOrEmpty())
        {
            sDateFilterType = "Payment";
            sStartDate = txtPaymentStartDate.Text.Trim();
            sEndDate = txtPaymentEndDate.Text.Trim();
            sDepositedBankId = cmbPaymentBank.SelectedValue;
        }

        return String.Format("({0}.SchoolId{9}={1} AND {0}.AcademicYearId{9}={2} AND {0}.IncludeClearedPayments{9}={3} AND {0}.NetbankinTransactionId{9}={4} AND {0}.StudentNameRegNoFilter{9}={5} AND {0}.DateFilterType{9}={6} AND {0}.StartDate{9}={7} AND {0}.EndDate{9}={8} AND {0}.DepositBankId{9}={10} AND {0}.IncludeCautionMoney{9}={11} AND {0}.IsInternalFee{9}={12} AND {0}.GatewayId={13})@",
                            Constants.S_EXPORT_ONLINETRANSACTIONCLEARANCE_USP,
                            iSchoolId,
                            iAcademicYearId,
                            chkIncludeAll.Checked ? 1 : 0,
                            txtTransactionIDNumber.Text.Trim().IsNullOrEmpty() ? "null" : txtTransactionIDNumber.Text.Trim(),
                            txtRegNo.Text.Trim().IsNullOrEmpty() ? "null" : txtRegNo.Text.Trim(),
                            sDateFilterType.IsNullOrEmpty() ? "null" : sDateFilterType,
                            sStartDate.IsNullOrEmpty() ? "null" : sStartDate,
                            sEndDate.IsNullOrEmpty() ? "null" : sEndDate,
                            "}",
                            sDepositedBankId,
                            chkCautionMoney.Checked ? 1: 0,
                            optInternalFee.Checked ? 1 : 0,
                            ddlGateway.SelectedValue
                            );
    }

    #endregion -- EXPORT FUNCTIONALITY --

    private void CheckFinancialYearStatus()
    {
        if (hidBaseFinancialYearId.Value != string.Empty && hidBaseFinancialYearId.Value.ToInt() != 0 && hidBaseFinancialYearId.Value.ToInt() != miFinancialYearId)
        {
            string sFinancialYearString = CommonUtility.EncryptQuerystring("IsFinancialYearShared=Y&ShowLink=Y");
            Response.Redirect("../Common/Error.aspx?" + sFinancialYearString, true);
        }
    }

    private void HideRow()
    {
        if (moSchool == Constants.SchoolId.PPSN && optOnlineTransactionClearance.Checked)
            trGateway.Visible = true;
        else
            trGateway.Visible = false;
    }
}