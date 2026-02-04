/* Created By - Sachin
 * Created Date - 6 Feb 2025
 * File Name - ExportVoucherDetailsUI.aspx.cs
 * Description - This class is sued to export voucher details as per selected date range and bank.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SchoolBusinessService;
using Utility;

public partial class ExportVoucherDetailsUI : ExportToExcel
{
    #region -- MEMBER(s) --

    private AccountVoucherClient moVoucherClient;
    private int miRowIndex = 5;
    private DatewiseVoucherDetails moDatewiseVoucherDetails;

    #endregion -- MEMBER(s) --

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill banks and set default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDepositedBanks();
            SetDefaultValues();
        }
    }

    /// <summary>
    /// This event is used to export voucher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            InitVoucherClient();

            moDatewiseVoucherDetails = moVoucherClient.GetVoucherDetailsToExport(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), cmbDepostedBank.SelectedValue.ToInt(), miAcademicYearId);

            string sFileName = "ExportVoucherDetails_" + Guid.NewGuid() + ".xlsx";
            string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                CreateWorkBookForDatewiseVoucher(workbookPart);
            }

            HttpContext.Current.Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            CloseVoucherClient();
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// Initializes the Voucher service object.
    /// </summary>
    private void InitVoucherClient()
    {
        moVoucherClient = new AccountVoucherClient();
        moVoucherClient.Open();
    }

    /// <summary>
    /// Disposes off the Voucher service object.
    /// </summary>
    private void CloseVoucherClient()
    {
        if (moVoucherClient != null && moVoucherClient.State != CommunicationState.Faulted)
            moVoucherClient.Close();
    }

    /// <summary>
    /// This method is sued to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valsum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is sued to fill deposited banks.
    /// </summary>
    private void FillDepositedBanks()
    {
        BankAccountClient oBankClient = null;
        try
        {
            oBankClient = new BankAccountClient();
            oBankClient.Open();
            List<BankAccount> lstBanks = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
            ListSource.FillDropDownList(lstBanks, cmbDepostedBank, "Name", "Id", Constants.S_SELECT);
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

    #region Export Day book details

    private void CreateWorkBookForDatewiseVoucher(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);
        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateReportForDatewiseVoucher(worksheetPart1);
        base.GeneratePartContent(aoPart, "Datewise Vouchers");
    }

    private void GenerateReportForDatewiseVoucher(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);

        SheetData sheetData1 = new SheetData();

        SetWidthForDatewiseVoucherReport(worksheet1);
        AddPageHeaderForDatewiseVoucherReport(sheetData1);
        AddHeaderForDatewiseVoucherReport(sheetData1);
        AddDataForDatewiseVoucherReport(sheetData1);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeCellsForDatewiseVoucherReport());

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private MergeCells MergeCellsForDatewiseVoucherReport()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        var iLedgerCount = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.LedgerName }).Distinct().Count();

        string sLastCell = base.GetReferenceName(5 + iLedgerCount);

        mergeCells1.Append(new MergeCell() { Reference = "A1" + ":" + sLastCell + "1" });
        mergeCells1.Append(new MergeCell() { Reference = "A2" + ":" + sLastCell + "2" });
        mergeCells1.Append(new MergeCell() { Reference = "A3" + ":" + sLastCell + "3" });
        mergeCells1.Append(new MergeCell() { Reference = "A4" + ":" + sLastCell + "4" });

        return mergeCells1;
    }

    private void AddPageHeaderForDatewiseVoucherReport(SheetData aoSheetData1)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miRowIndex - 4), CustomHeight = true, Height = 15 };

        row.Append(AddCell(moDatewiseVoucherDetails.SchoolDetails.SchoolName, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        var oLedgers = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.SortOrder, dv.LedgerName, dv.IsDebit }).Distinct().ToList();

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
            });

        aoSheetData1.Append(row);
        /////////////////
        Row rowAddress = new Row { RowIndex = Convert.ToUInt32(miRowIndex - 3), CustomHeight = true, Height = 15 };

        rowAddress.Append(AddCell(moDatewiseVoucherDetails.SchoolDetails.Address, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowAddress.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowAddress.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowAddress.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                rowAddress.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
            });

        aoSheetData1.Append(rowAddress);
        /////////////////////
        Row rowLedgerTitle = new Row { RowIndex = Convert.ToUInt32(miRowIndex - 2), CustomHeight = true, Height = 15 };

        rowLedgerTitle.Append(AddCell("Ledger Account", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowLedgerTitle.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowLedgerTitle.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowLedgerTitle.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                rowLedgerTitle.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
            });

        aoSheetData1.Append(rowLedgerTitle);
        /////////////////////////
        Row rowDate = new Row { RowIndex = Convert.ToUInt32(miRowIndex - 1), CustomHeight = true, Height = 15 };

        string sDate;
        if (txtStartDate.Text != txtEndDate.Text)
            sDate = txtStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT) + " to " + txtEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
        else
            sDate = txtStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

        rowDate.Append(AddCell(sDate, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowDate.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowDate.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        rowDate.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                rowDate.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
            });

        aoSheetData1.Append(rowDate);

    }

    private void SetWidthForDatewiseVoucherReport(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 18D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 18D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 15D, CustomWidth = true });

        int iLedgerCount = moDatewiseVoucherDetails.DatewiseVouchers.Count;

        iLedgerCount = 4 + iLedgerCount;

        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = Convert.ToUInt32(iLedgerCount), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private void AddDataForDatewiseVoucherReport(SheetData aoSheetData1)
    {
        miRowIndex++;

        var oLedgers = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.SortOrder, dv.LedgerName, dv.IsDebit }).Distinct().ToList();

        moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { VoucherType = dv.VoucherType, Date = dv.Date, Particulars = dv.Particulars, SerialNumber = dv.SerialNumber }).Distinct().OrderBy(dv => dv.Date).ToList().ForEach(dv =>
        {
            Row row = new Row { RowIndex = Convert.ToUInt32(miRowIndex), CustomHeight = true, Height = 15 };
            row.Append(AddCell(dv.Date.ToString(Constants.S_DATE_FORMAT), CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(dv.Particulars, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(dv.VoucherType, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(dv.SerialNumber, CellValues.String, StudentPaidFeeEnum.LeftData));

            var iTotalAmount = moDatewiseVoucherDetails.DatewiseVouchers.Where(vd => vd.Date == dv.Date && vd.IsDebit == false && vd.SerialNumber == dv.SerialNumber).Sum(vd => vd.Amount);

            row.Append(AddCell(iTotalAmount.ToString() + " Dr", CellValues.String, StudentPaidFeeEnum.RightData));

            //oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            oLedgers.OrderBy(ld => ld.SortOrder).ToList().ForEach(
            ld =>
            {
                var oAmountData = moDatewiseVoucherDetails.DatewiseVouchers.Where(am => am.Date == dv.Date && am.LedgerName == ld.LedgerName && am.SerialNumber == dv.SerialNumber).Sum(am => am.Amount);
                if (oAmountData != null)
                    row.Append(AddCell(oAmountData.ToString() + (ld.IsDebit ? " Dr" : " Cr"), CellValues.String, StudentPaidFeeEnum.RightData));
                else
                    row.Append(AddCell(Constants.S_ZERO, CellValues.String, StudentPaidFeeEnum.RightData));
            });

            aoSheetData1.Append(row);
            miRowIndex++;
        });

        Row row1 = new Row { RowIndex = Convert.ToUInt32(miRowIndex), CustomHeight = true, Height = 15 };
        row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
        row1.Append(AddCell("Grand Total", CellValues.String, StudentPaidFeeEnum.LeftData));
        row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));

        var oTotalAmount = moDatewiseVoucherDetails.DatewiseVouchers.Where(am => am.IsDebit).Sum(am => am.Amount);

        row1.Append(AddCell(oTotalAmount.ToString() + " Dr", CellValues.String, StudentPaidFeeEnum.RightData));

        //oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
        oLedgers.OrderBy(ld => ld.SortOrder).ToList().ForEach(
            ld =>
            {
                var oAmountData = moDatewiseVoucherDetails.DatewiseVouchers.Where(am => am.LedgerName == ld.LedgerName).Sum(am => am.Amount);
                if (oAmountData != null)
                    row1.Append(AddCell(oAmountData.ToString() + (ld.IsDebit ? " Dr" : " Cr"), CellValues.String, StudentPaidFeeEnum.RightData));
                else
                    row1.Append(AddCell(Constants.S_ZERO, CellValues.String, StudentPaidFeeEnum.RightData));
            });
        aoSheetData1.Append(row1);
    }

    private void AddHeaderForDatewiseVoucherReport(SheetData aoSheetData1)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miRowIndex), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Date", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Particulars", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Voucher Type", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Serial Number", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Gross Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        var oLedgers = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.SortOrder, dv.LedgerName, dv.IsDebit }).Distinct().ToList();

        //oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
        oLedgers.OrderBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                row.Append(AddCell(dv.LedgerName, CellValues.String, StudentPaidFeeEnum.RightHeader));
            });

        aoSheetData1.Append(row);
    }

    #endregion 

    #endregion
}