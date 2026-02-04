/* --------------------------------------------------------------------
 *	FileName	: DayBook.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 6-Nov-2011
 *	Description	: This is the code behind file for the DayBook screen,
 *				  where one can view all vouchers created by anyone.
 * --------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using System.Web;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

public partial class DayBook : ExportToExcel
{

	#region -- CONSTANT(s) --

	private const string S_DEFAULT_SORT = "SerialNumber";
	private const string S_SORT_ROW = "SORT_ROW";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountVoucherClient moVoucherClient;

    private int miRowIndex = 5;
    private DatewiseVoucherDetails moDatewiseVoucherDetails;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to handle the loading of a page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				InitVoucherClient();
				Initialize();
			}

			chkDateRange.InputAttributes.CssStyle.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
			chkDateRange.LabelAttributes.CssStyle.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
			chkIncludePending.InputAttributes.CssStyle.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
			chkIncludePending.LabelAttributes.CssStyle.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
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

	/// <summary>
	/// This event is used to add the sort image for the Ledger list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_PreRenderComplete(object sender, EventArgs e)
	{
		try
		{
			// Add Sort Image
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is handled to display vouchers based on the selected date/date range.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ResetSearchControls();
			ReBindDayBook();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		Enables the input controls on the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnChangeInput_Click(object sender, EventArgs e)
	{
		try
		{
			DisableControls(false);
			btnExport.Visible = false;
            btnExportToExcel.Visible = false;
            btnExportDayBookDetails.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnExport_Click(object sender, EventArgs e)
	{
		try
		{
			InitVoucherClient();
			List<Voucher> lstVouchers = moVoucherClient.Export(miSchoolId,
															   miFinancialYearId,
															   txtStartDate.Text.ToDateTime(),
															   !chkDateRange.Checked || txtEndDate.Text.IsNullOrEmpty() ? DateTime.MinValue : txtEndDate.Text.ToDateTime(),
															   chkIncludePending.Checked,
															   Constants.I_ZERO);
			
			if (lstVouchers != null && lstVouchers.Count > 0)
				Accounts.ExportVoucherXML(lstVouchers);
			else
			{
				ResetSearchControls();
				ReBindDayBook();
			}
		}
		catch (ThreadAbortException)
		{
			// This exception is caught here becuase it is generated
			// while exporting the XML file as an attachment.
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

	/// <summary>
	/// Updates the ListView Pager controls when switching to a different page no.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwDayBook);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to specify a value for the adtEndDate param under certain conditions.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void objdsDayBook_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		try
		{
			if (String.IsNullOrEmpty(txtEndDate.Text) || (!chkDateRange.Checked && !String.IsNullOrEmpty(txtEndDate.Text)))
				e.InputParameters["adtEndDate"] = DateTime.MinValue;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Sets a different classname for alternating items in the ListView and also sets values for non-databound items.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwDayBook_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;

				var oStatus = (Constants.RequisitionStatus)lstvwDayBook.DataKeys[oCurrentItem.DisplayIndex]["Status"];
				bool bIsFeeVoucher = lstvwDayBook.DataKeys[oCurrentItem.DisplayIndex]["IsFeeVoucher"].ToBool();
                bool bIsInternalFeeVoucher = lstvwDayBook.DataKeys[oCurrentItem.DisplayIndex]["IsInternalFeeVoucher"].ToBool();

				if (oStatus == Constants.RequisitionStatus.Pending)
					oHTMLCurrentRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightPink");

				if (bIsFeeVoucher)
					oHTMLCurrentRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightBlue");

                if(bIsInternalFeeVoucher)
                    oHTMLCurrentRow.Style.Add(HtmlTextWriterStyle.Color, "Maroon");

				string sVoucherId = lstvwDayBook.DataKeys[oCurrentItem.DisplayIndex]["VoucherId"].ToString();
				var imgbtnView = oCurrentItem.FindControl("imgbtnView") as ImageButton;
				imgbtnView.Attributes["onclick"] = String.Format("window.open('VoucherPopUp.aspx?{0}', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600'); return false;",
                                                                  CommonUtility.EncryptQuerystring(String.Format("ViewMode={0}&VoucherId={1}&SourceStatusId={2}&IsInternalFeeVoucher={3}",
																												  Constants.ViewMode.View.ToInt(),
																												  sVoucherId,
																												  Constants.RequisitionStatus.Pending.ToInt(),
                                                                                                                  (bIsInternalFeeVoucher?1:0))));
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Initializes the DataPager control of the ListView and adds a Sort Image.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwDayBook_DataBound(object sender, EventArgs e)
	{
		try
		{
            if (lstvwDayBook.Items.Count > 0)
            {
                // Initialize the DataPager control
                var oDtPgCount = lstvwDayBook.FindControl("DtPgCount") as DataPager;
                if (!oDtPgCount.IsNull())
                    ControlUtility.FillListViewPagerFooter(lstvwDayBook, oDtPgCount);

                DisableControls(true);
                btnExport.Visible = true;
                btnExportToExcel.Visible = true;
                btnExportDayBookDetails.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
                btnExportToExcel.Visible = false;
                btnExportDayBookDetails.Visible = false;
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Handles sorting in the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwDayBook_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			// This case is to handle a sort command. We have set a custom sort command - 'SORT_ROW' so we can handle sorting ourselves.
			// In such a scenario, the ItemType property is actually EmptyItem, hence we cannot handle this in the previous block.
			if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
			{
				if (hidSortExpression.Value != e.CommandArgument.ToString())
					hidSortDirection.Value = Constants.S_DESCENDING;
				SetSortVariables();
				hidSortExpression.Value = e.CommandArgument.ToString();
				ReBindDayBook();

				var oDtPgDropDown = lstvwDayBook.FindControl("DtPgDropDown") as DataPager;
				if (oDtPgDropDown != null)
					oDtPgDropDown.SetPageProperties(0, oDtPgDropDown.PageSize, true);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to export day book details to excel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            InitVoucherClient();

            List<Voucher> lstVouchers = moVoucherClient.GetVoucherDetailsForDays(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), !chkDateRange.Checked || txtEndDate.Text.IsNullOrEmpty() ? DateTime.MinValue : txtEndDate.Text.ToDateTime(),miUserId);

            if (lstVouchers != null && lstVouchers.Count > 0)
            {
                SetBasicHTTPResponse();

                StringBuilder obj = new StringBuilder();
                obj.Append("<Table width='100%' border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");

                obj.Append(AddHeader());

                lstVouchers.OrderBy(vc => vc.Date).ToList().ForEach(
                    vc =>
                    {
                        obj.Append(AddData(vc));

                        decimal dcAmount = lstVouchers.Where(vcp => vcp.VoucherId == vc.VoucherId).Sum(vcp => vcp.Amount);
                        obj.Append(AddSummaryRow(dcAmount,false));

                        obj.Append(AddBlankRow());
                    }
                    );
                
                decimal dcTotalAmount = lstVouchers.Sum(vc => vc.Amount);
                obj.Append(AddSummaryRow(dcTotalAmount,true));

                obj.Append("</tr>");
                obj.Append("</Table>");
                HttpContext.Current.Response.Write(obj.ToString());
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            else
            {
                ResetSearchControls();
                ReBindDayBook();
            }
        }
        catch (ThreadAbortException)
        {
            // This exception is caught here becuase it is generated
            // while exporting the XML file as an attachment.
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

    /// <summary>
    /// This event is used to export day book details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportDayBookDetails_Click(object sender, EventArgs e)
    {
        try
        {
            InitVoucherClient();

            DateTime dtEndDate;
            if (chkDateRange.Checked)
                dtEndDate = txtEndDate.Text.ToDateTime();
            else
                dtEndDate = txtStartDate.Text.ToDateTime();

            moDatewiseVoucherDetails = moVoucherClient.GetDatewiseVoucherDetails(miSchoolId, miFinancialYearId, txtStartDate.Text.ToDateTime(), dtEndDate);

            string sFileName = "DatewiseVoucherDetails_" + Guid.NewGuid() + ".xlsx";
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

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This function is used to initialize controls to their default values.
	/// </summary>
	private void Initialize()
	{
        ApplyMouseHoverEffect(new List<Button> { btnShow, btnChangeInput, btnExport, btnExportToExcel, btnExportDayBookDetails });
		SetDefaultButton(btnShow);

		valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		txtStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

		hidSortExpression.Value = S_DEFAULT_SORT;
		hidSortDirection.Value = Constants.S_DESCENDING;
	}

	/// <summary>
	/// This function sets the hiddenfield values that are maintained to remember sort direction.
	/// </summary>
	private void SetSortVariables()
	{
		hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

	/// <summary>
	/// This function is used to add a sort image to the ListView.
	/// </summary>
	private void AddSortImage()
	{
		string sSortExpression = hidSortExpression.Value;
		string sSortDirection = hidSortDirection.Value;
		var oHtmlTableHeaderRow = lstvwDayBook.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
	}

	/// <summary>
	/// Rebinds the DayBook ListView to its DataSource.
	/// </summary>
	private void ReBindDayBook()
	{
		lstvwDayBook.Items.Clear();
		lstvwDayBook.DataSourceID = objdsDayBook.ID;
	}

	/// <summary>
	///		Resets hidden fields maintained for sorting and listview pager controls.
	/// </summary>
	private void ResetSearchControls()
	{
		hidSortExpression.Value = S_DEFAULT_SORT;
		hidSortDirection.Value = Constants.S_DESCENDING;

		var dtPgDropDown = lstvwDayBook.FindControl("DtPgDropdown") as DataPager;

		if (dtPgDropDown.IsNull() || dtPgDropDown.Controls.Count <= 0)
			return;

		var ddlCnt = dtPgDropDown.Controls[0].FindControl("ddlCnt") as DropDownList;

		if (ddlCnt.IsNull() || ddlCnt.Items.Count <= 0)
			return;

		ddlCnt.SelectedIndex = 0;
		ControlUtility.SetDataPagerAccordingToPageNo(lstvwDayBook);
	}

	private void DisableControls(bool abDisable)
	{
		txtStartDate.Enabled = !abDisable;
		dtStartDate.Enabled = !abDisable;
		txtEndDate.Enabled = !abDisable;
		dtEndDate.Enabled = !abDisable;
		chkDateRange.Enabled = !abDisable;
		chkIncludePending.Enabled = !abDisable;
		btnShow.Visible = !abDisable;
		btnChangeInput.Visible = abDisable;        
	}

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
    /// This method is used to add data in excel.
    /// </summary>
    /// <param name="vc"></param>
    /// <returns></returns>
    private string AddData(Voucher vc)
    {
        StringBuilder obj = new StringBuilder();
        obj.Append("<TR>");
        obj.Append(AddCell(vc.SerialNumber, "text-align:center;"));
        obj.Append(AddCell(vc.Date.ToString(Constants.S_DATE_FORMAT), "text-align:center;"));
        obj.Append(AddCell(vc.VoucherType.Name, "150px;text-align:center;"));
        obj.Append(AddCell(vc.CreatedBy, "padding-left:5px;"));
        obj.Append(AddCell(vc.VoucherParticulars[0].Ledger.Name, "padding-left:5px;"));

        if (vc.VoucherParticulars[0].IsDebit)
        {
            obj.Append(AddCell(vc.VoucherParticulars[0].Amount.ToString(), "text-align:right;padding-right:5px;"));
            obj.Append(AddCell("0", "text-align:right;padding-right:5px;"));
        }
        else
        {
            obj.Append(AddCell("0", "text-align:right;padding-right:5px;"));
            obj.Append(AddCell(vc.VoucherParticulars[0].Amount.ToString(), "text-align:right;padding-right:5px;"));
        }
                
        obj.Append("</TR>");

        obj.Append(AddVoucherParticulars(vc));

        return obj.ToString();
    }

    /// <summary>
    /// This method is used to add voucher particulars details.
    /// </summary>
    /// <param name="vc"></param>
    /// <returns></returns>
    private string AddVoucherParticulars(Voucher vc)
    {
        StringBuilder obj = new StringBuilder();
        bool bAddData = false;
        if (vc.VoucherParticulars.Count > 1)
        {
            vc.VoucherParticulars.ForEach(vp =>
            {
                if (bAddData)
                {
                    obj.Append("<tr>");
                    obj.Append(AddCell(string.Empty));
                    obj.Append(AddCell(string.Empty));
                    obj.Append(AddCell(string.Empty));
                    obj.Append(AddCell(string.Empty));
                    obj.Append(AddCell(vp.Ledger.Name, "padding-left:5px;"));

                    if (vp.IsDebit)
                    {
                        obj.Append(AddCell(vp.Amount.ToString(), "text-align:right;padding-right:5px;"));
                        obj.Append(AddCell("0", "text-align:right;padding-right:5px;"));
                    }
                    else
                    {
                        obj.Append(AddCell("0", "text-align:right;padding-right:5px;"));
                        obj.Append(AddCell(vp.Amount.ToString(), "text-align:right;padding-right:5px;"));
                    }
                    obj.Append("</tr>");
                }
                else
                    bAddData = true;
            });
        }
        return obj.ToString();
    }

    /// <summary>
    /// This method is used to add blank row.
    /// </summary>
    /// <returns></returns>
    private string AddBlankRow()
    {
        StringBuilder obj = new StringBuilder();
        obj.Append("<tr>");
        obj.Append("</tr>");
        return obj.ToString();
    }

    /// <summary>
    /// This method is used to add summary row.
    /// </summary>
    /// <param name="dcTotalAmount"></param>
    /// <returns></returns>
    private string AddSummaryRow(decimal dcTotalAmount, bool abIsGrandTotal)
    {
        StringBuilder obj = new StringBuilder();
        string sColor = "color:black;";
        if(abIsGrandTotal)
            sColor = "color:Navy;";

        obj.Append("<tr>");
        obj.Append(AddCell(string.Empty));
        obj.Append(AddCell(string.Empty));
        obj.Append(AddCell(string.Empty));
        obj.Append(AddCell(string.Empty));
        obj.Append(AddCell("Total", "padding-left:5px;font-weight:bold;" + sColor + "background-color:gray;"));
        obj.Append(AddCell(dcTotalAmount.ToString(), "text-align:right;padding-right:5px;font-weight:bold;" + sColor + "background-color:gray;"));
        obj.Append(AddCell(dcTotalAmount.ToString(), "text-align:right;padding-right:5px;font-weight:bold;" + sColor + "background-color:gray;"));
        return obj.ToString();
    }

    /// <summary>
    /// This method is used to add headers in excel.
    /// </summary>
    /// <returns></returns>
    private string AddHeader()
    {
        StringBuilder obj = new StringBuilder();
        obj.Append("<TR>");
        obj.Append(AddCell("Sr. No.", "font-weight:bold;width:100px;text-align:center;background-color:Gray;"));
        obj.Append(AddCell("Voucher Date", "font-weight:bold;width:100px;text-align:center;background-color:Gray;"));
        obj.Append(AddCell("Voucher Type", "font-weight:bold;width:100px;text-align:center;background-color:Gray;"));
        obj.Append(AddCell("Created By", "font-weight:bold;width:250px;padding-left:5px;background-color:Gray;"));
        obj.Append(AddCell("Particulars", "font-weight:bold;width:250px;padding-left:5px;background-color:Gray;"));
        obj.Append(AddCell("Debit (Rs.)", "font-weight:bold;width:100px;text-align:right;padding-right:5px;background-color:Gray;"));
        obj.Append(AddCell("Credit (Rs.)", "font-weight:bold;width:100px;text-align:right;padding-right:5px;background-color:Gray;"));
        obj.Append("</TR>");
        return obj.ToString();
    }

    /// <summary>
    /// This method is used to set basic http details.
    /// </summary>
    private void SetBasicHTTPResponse()
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=DayBook.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
    }

    /// <summary>
    /// This method is used to add new cell.
    /// </summary>
    /// <param name="asData"></param>
    /// <param name="asStyle"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="aiRowSpan"></param>
    /// <param name="asControlString"></param>
    /// <returns></returns>
    private string AddCell(string asData, string asStyle = "", int aiColSpan = 1, int aiRowSpan = 1, string asControlString = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        StringBuilder obj = new StringBuilder();
        obj.Append("<TD colspan='" + aiColSpan + "' rowspan='" + aiRowSpan + "'" + sStyle + ">");
        obj.Append(asData);

        if (asControlString != string.Empty)
            obj.Append(asControlString);

        obj.Append("</TD>");
        return obj.ToString();
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

        string sLastCell = base.GetReferenceName(3 + iLedgerCount);

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
        if (chkDateRange.Checked)
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
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 15D, CustomWidth = true });

        int iLedgerCount = moDatewiseVoucherDetails.DatewiseVouchers.Count;

        iLedgerCount = 4 + iLedgerCount;

        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = Convert.ToUInt32(iLedgerCount), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private void AddDataForDatewiseVoucherReport(SheetData aoSheetData1)
    {
        miRowIndex++;

        var oLedgers = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.SortOrder, dv.LedgerName, dv.IsDebit }).Distinct().ToList();

        moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { VoucherType = dv.VoucherType, Date = dv.Date, Particulars = dv.Particulars }).Distinct().OrderBy(dv => dv.Date).ToList().ForEach(dv =>
        {
            Row row = new Row { RowIndex = Convert.ToUInt32(miRowIndex), CustomHeight = true, Height = 15 };
            row.Append(AddCell(dv.Date.ToString(Constants.S_DATE_FORMAT), CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(dv.Particulars, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(dv.VoucherType, CellValues.String, StudentPaidFeeEnum.LeftData));

            var iTotalAmount = moDatewiseVoucherDetails.DatewiseVouchers.Where(vd => vd.Date == dv.Date && vd.IsDebit == false).Sum(vd => vd.Amount);

            row.Append(AddCell(iTotalAmount.ToString() + " Dr", CellValues.String, StudentPaidFeeEnum.RightData));

            oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            ld =>
            {
                var oAmountData = moDatewiseVoucherDetails.DatewiseVouchers.Where(am => am.Date == dv.Date && am.LedgerName == ld.LedgerName).Sum(am => am.Amount);
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

        var oTotalAmount = moDatewiseVoucherDetails.DatewiseVouchers.Where(am=> am.IsDebit).Sum(am => am.Amount);

        row1.Append(AddCell(oTotalAmount.ToString()+" Dr", CellValues.String, StudentPaidFeeEnum.RightData));

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
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
        row.Append(AddCell("Gross Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        var oLedgers = moDatewiseVoucherDetails.DatewiseVouchers.Select(dv => new { dv.SortOrder, dv.LedgerName, dv.IsDebit }).Distinct().ToList();

        oLedgers.OrderByDescending(ld => ld.IsDebit).ThenBy(ld => ld.SortOrder).ToList().ForEach(
            dv =>
            {
                row.Append(AddCell(dv.LedgerName, CellValues.String, StudentPaidFeeEnum.RightHeader));
            });

        aoSheetData1.Append(row);
    }

    #endregion

    #endregion -- PRIVATE METHOD(s) --
}