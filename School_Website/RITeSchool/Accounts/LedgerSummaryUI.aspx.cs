/* -------------------------------------------------------------------------------
 *	FileName	: LedgerSummaryUI.aspx.cs
 *	Author		: Deepak
 *	Date		: 26-Mar-2012
 *	Description	: This is the code behind file for the Ledger Summary screen, which
 *				  is used to display vouchers for the selected ledger.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AccountsEntities;
using SchoolBusinessService;




public partial class LedgerSummaryUI : SchoolBase
{
	#region -- CONSTANT(s) --
    
	private const string S_SORT_EXP = "Date";
	private const string S_SORT_ROW = "SORT_ROW";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountLedgerClient moAccountLedgerClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// 	Sets the MasterPage depending upong the logged in user or request query string.
	/// </summary>
	/// <param name="e"> </param>
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);

			if (Request.QueryString.Count <= 0)
				Page.MasterPageFile = Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null ? "../Superadmin/SuperAdminMasterPage.master" : "../MasterPages/MasterPage.master";
			else
				Page.MasterPageFile = "../MasterPages/PopupMaster.master";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Handles the loading of the Page.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			OpenLedgerServiceObj();			
			if (!IsPostBack)
			{
				FillLedgerCombo();
				Initialize();
				ReadQueryString();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());	
		}
		finally
		{
			CloseLedgerServiceObj();
		}
	}

	/// <summary>
	/// 	This event is used to add the sort image for the Ledger list.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
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
	/// 	Handles the click even of the Show button.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			var pager = lstvwVouchers.FindControl("DtPgCount") as DataPager;
			if (pager != null)
				pager.SetPageProperties(0, pager.MaximumRows, false);
			BindVoucherList();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Handles the click even of the Cancel button.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ddlLedgers.SelectedValue = "0";
			txtStartDate.Text = string.Empty;
			txtEndDate.Text = string.Empty;
			calEndDate.SelectDateText = string.Empty;
			calStartDate.SelectDateText = string.Empty;
			lstvwVouchers.Items.Clear();
			lstvwVouchers.DataSourceID = null;
			lstvwVouchers.Visible = false;
            trLedgerTotal.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Sets the page properties when the user changes the page of the grid.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void ddlLedgers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			var pager = lstvwVouchers.FindControl("DtPgCount") as DataPager;
			if (pager != null)
				pager.SetPageProperties(0, pager.MaximumRows, false);
			BindVoucherList();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Initializes controls in the grid with correct values.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwVouchers_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				var lblPerticular = e.Item.FindControl("lblPerticular") as Label;
				var lblDebit = e.Item.FindControl("lblDebit") as Label;
				var lblCredit = e.Item.FindControl("lblCredit") as Label;
				var oVoucher = oCurrentItem.DataItem as Voucher;
				int iLedgerId = ddlLedgers.SelectedValue.ToInt();
				List<VoucherParticular> lstVoucherParticular = oVoucher.VoucherParticulars.Where(lstPerticular => lstPerticular.Ledger.Id != iLedgerId).ToList();
                if (lstVoucherParticular.Count >= 1)
				{
					lblPerticular.Text = "(as per particulars)";
					var trPerticulersDetails = e.Item.FindControl("trPerticulersDetails") as HtmlTableRow;
					trPerticulersDetails.Visible = true;
					var tdPerticulersDetails = trPerticulersDetails.FindControl("tdPerticulersDetails") as HtmlTableCell;
					var lstvwPerticulersDetails = tdPerticulersDetails.FindControl("lstvwPerticulersDetails") as ListView;
					lstvwPerticulersDetails.DataSource = lstVoucherParticular;
					lstvwPerticulersDetails.DataBind();

					lblDebit.Text  = CommonUtility.FormatCurrency(oVoucher.VoucherParticulars
												  .Where(particular => particular.Ledger.Id == iLedgerId && particular.IsDebit)
												  .Sum(lst => lst.Amount));
					lblCredit.Text = CommonUtility.FormatCurrency(oVoucher.VoucherParticulars
												  .Where(particular => particular.Ledger.Id == iLedgerId && !particular.IsDebit)
												  .Sum(lst => lst.Amount));
				}
				else
					lblPerticular.Text = oVoucher.VoucherParticulars.FirstOrDefault().Ledger.Name;

				string sOnClickAttr = "window.open('VoucherPopUp.aspx?{0}', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600'); return false;";
				var imgbtnView = oCurrentItem.FindControl("imgbtnView") as ImageButton;
				imgbtnView.Attributes["onclick"] = String.Format(sOnClickAttr,
																 CommonUtility.EncryptQuerystring(String.Format("ViewMode={0}&VoucherId={1}&NextApproverDesigId={2}&NextApproverDesigName={3}&SourceStatusId={4}",
																												 Constants.ViewMode.View.ToInt(),
																												 oVoucher.VoucherId,
																												 0,
																												 String.Empty,
																												 0)));
                LinkButton lnkExport = oCurrentItem.FindControl("lbtnExport") as LinkButton;

                lnkExport.Attributes.Add("onclick", "CheckShowParticulares()");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Sets the pager properties of the ListView.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwVouchers_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwVouchers.Items.Count > 0)
			{
				var oDtPgCount = lstvwVouchers.FindControl("DtPgCount") as DataPager;
				ControlUtility.FillListViewPagerFooter(lstvwVouchers, oDtPgCount);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Handles sorting of the items in the ListView.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void lstvwVouchers_ItemCommand(object sender, ListViewCommandEventArgs e)
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
                var oHtmlAnchor = lstvwVouchers.FindControl("lnkToggel") as HtmlAnchor;
                if (hidToggel.Value == Constants.S_ZERO)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Toggel", " $('.clsPerticulars').hide();", true);
                    oHtmlAnchor.InnerText = "Expand All";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Toggel", " $('.clsPerticulars').show();", true);
                    oHtmlAnchor.InnerText = "Collapse All";
                }
			}
            else if (e.CommandName == "EXPORT")
            {
                int iVoucherId = Convert.ToInt32(lstvwVouchers.DataKeys[e.Item.DisplayIndex]["VoucherId"]);

                ExportLedgerSummary(iVoucherId);
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// 	Sets the properties of the DataPager control for the ListView.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"> </param>
	protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwVouchers);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// 	This event is used to export All Ledger summary.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ExportLedgerSummary(Constants.I_ZERO);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// 	This function is used to read the query string passed to the page and set some member variables/hidden fields accordingly.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.Count <= 0)
			return;

		if (!QueryString["LedgerId"].IsNullOrEmpty())
			ddlLedgers.SelectedValue = QueryString["LedgerId"];

		// When we have Month and Year passed in the query string, we set the start and end date of that month.
		if (!QueryString["Month"].IsNullOrEmpty() && !QueryString["Year"].IsNullOrEmpty())
		{
			int iMonth, iYear;
			if (Int32.TryParse(QueryString["Month"], out iMonth) && Int32.TryParse(QueryString["Year"], out iYear))
			{
				var dtStartDate = new DateTime(iYear, iMonth, 1);
				calStartDate.SelectedDate = dtStartDate.ToString("dd-MMM-yyyy");
                var iTotaldays = DateTime.DaysInMonth(iYear, iMonth);
                calEndDate.SelectedDate = new DateTime(iYear, iMonth, iTotaldays).ToString("dd-MMM-yyyy");				
			}
		}
		// Else we set the start and end date as per the From and To query string parametes.
		else
		{
			DateTime dtDateTime;
			
			if (!QueryString["From"].IsNullOrEmpty() && DateTime.TryParse(QueryString["From"], out dtDateTime))
				calStartDate.SelectedDate = dtDateTime.ToString("dd-MMM-yyyy");
			else
				calStartDate.SelectedDate = DateTime.Now.ToString("dd-MMM-yyyy");

			if (!QueryString["To"].IsNullOrEmpty() && DateTime.TryParse(QueryString["To"], out dtDateTime))
				calEndDate.SelectedDate = dtDateTime.ToString("dd-MMM-yyyy");
			else
				calEndDate.SelectedDate = DateTime.Now.ToString("dd-MMM-yyyy");
		}

		BindVoucherList();
	}

	/// <summary>
	/// 	This function is used to add a sort image to the ListView.
	/// </summary>
	private void AddSortImage()
	{
		string sSortExpression = hidSortExpression.Value;
		string sSortDirection = hidSortDirection.Value;
		var oHtmlTableHeaderRow = lstvwVouchers.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
	}

	/// <summary>
	/// 	Initializes default values for certain controls on the page.
	/// </summary>
	private void Initialize()
	{
		ApplyMouseHoverEffect(new List<Button> { btnShow, btnBack, btnCancel, btnExport });
		valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
		if (oFinancialYear != null)
		{
			hidFinancialYrStartDt.Value = oFinancialYear.StartDate.ToString("dd MMM yyyy");
			hidFinancialYrEndDt.Value = oFinancialYear.EndDate.ToString("dd MMM yyyy");
		}

		if (Request.QueryString.Count <= 0)
		{
			btnBack.Text = "Back";

			if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
			{
				btnBack.PostBackUrl = Constants.S_PAGE_SUPERADMIN_DASHBOARD;
				trLedgerSummary.Visible = true;
			}
			else
			{
				btnBack.Visible = false;
				trLedgerSummary.Visible = false;
			}
		}
		else
		{
			btnBack.Text = "Close";
			btnBack.Attributes.Add("onclick", "window.close()");
		}
		// Sort defaults
		hidSortExpression.Value = S_SORT_EXP;
		hidSortDirection.Value = Constants.S_DESCENDING;
	}

	/// <summary>
	/// 	Populates the Ledgers dropdown list with values.
	/// </summary>
	private void FillLedgerCombo()
	{
        List<int> lstLedgerIds = SchoolwiseStudentFeeMasterBL.GetActiveLedgersIds(miSchoolId, miFinancialYearId);
		List<Ledger> lstVoucherStatus = moAccountLedgerClient.AllLedgers(miSchoolId, miFinancialYearId);

        lstVoucherStatus = (from vs in lstVoucherStatus
                            join id in lstLedgerIds
                            on vs.Id equals id
                            select vs).ToList();


		ListSource.FillDropDownList(lstVoucherStatus, ddlLedgers, "Name", "Id", "-- Select --");
	}

	/// <summary>
	/// 	Binds the Voucher ListView to its Datasource.
	/// </summary>
	private void BindVoucherList()
	{
		if (ddlLedgers.SelectedValue != string.Empty && txtStartDate.Text != string.Empty && txtEndDate.Text != string.Empty)
		{
			lstvwVouchers.Visible = true;
			lstvwVouchers.Items.Clear();
			lstvwVouchers.DataSourceID = objdsVouchers.ID;

            trLedgerTotal.Visible = true;
            btnExport.Visible = true;
            SetLedgerTotal();
		}
	}

	/// <summary>
	/// 	This function sets the hiddenfield values that are maintained to remember sort direction.
	/// </summary>
	private void SetSortVariables()
	{
		hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

    /// <summary>
    /// 	Initializes the Accounts base service object.
    /// </summary>
    private void SetLedgerTotal()
    {
        AccountVoucherClient oAccountVoucherClient = new AccountVoucherClient();
        oAccountVoucherClient.Open();

        int iLedgerId = ddlLedgers.SelectedValue.ToInt();

        List<Voucher> lstVouchers = oAccountVoucherClient.GetAllVouchersForLedger(miSchoolId, miAcademicYearId, miFinancialYearId, iLedgerId, txtStartDate.Text, txtEndDate.Text, string.Empty, string.Empty, 0, 1000);

        decimal dcTotalDebit = 0, dcTotalCredit = 0;
        foreach (var oVoucher in lstVouchers)
        {
            List<VoucherParticular> lstVoucherParticular = oVoucher.VoucherParticulars.Where(lstPerticular => lstPerticular.Ledger.Id != iLedgerId).ToList();

            decimal dcDebit = 0, dcCredit = 0;

            if (lstVoucherParticular.Count >= 1)
            {
                dcDebit = oVoucher.VoucherParticulars
                                                      .Where(particular => particular.Ledger.Id == iLedgerId && particular.IsDebit)
                                                      .Sum(lst => lst.Amount);
                dcCredit = oVoucher.VoucherParticulars
                                              .Where(particular => particular.Ledger.Id == iLedgerId && !particular.IsDebit)
                                              .Sum(lst => lst.Amount);
            }

            dcTotalDebit = dcTotalDebit + dcDebit;
            dcTotalCredit = dcTotalCredit + dcCredit;
        }

        lblTotalDebitAmount.Text = CommonUtility.FormatCurrency(dcTotalDebit);
        lblTotalCreditAmount.Text = CommonUtility.FormatCurrency(dcTotalCredit);
    }

	/// <summary>
	/// 	Initializes the Accounts base service object.
	/// </summary>
	private void OpenLedgerServiceObj()
	{
		moAccountLedgerClient = new AccountLedgerClient();
		moAccountLedgerClient.Open();
	}

	/// <summary>
	/// 	Disposes off the Accounts base service object.
	/// </summary>
	private void CloseLedgerServiceObj()
	{
		if (moAccountLedgerClient != null && moAccountLedgerClient.State != CommunicationState.Faulted)
			moAccountLedgerClient.Close();
	}

    /// <summary>
    /// 	This method is used for Export Ledgure Summary.
    /// </summary>
    private void ExportLedgerSummary(int iVoucherId)
    {
        AccountVoucherClient oAccountVoucherClient = new AccountVoucherClient();
        oAccountVoucherClient.Open();

        int iLedgerId = ddlLedgers.SelectedValue.ToInt();

        List<Voucher> lstVouchers = oAccountVoucherClient.GetAllVouchersForLedger(miSchoolId, miAcademicYearId, miFinancialYearId, iLedgerId, txtStartDate.Text, txtEndDate.Text, string.Empty, string.Empty, 0, 1000);

        if (iVoucherId != Constants.I_ZERO)
            lstVouchers = lstVouchers.Where(sa => sa.VoucherId == iVoucherId).ToList();

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-LedgerSummary.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
        HttpContext.Current.Response.Write("<TR>");

        AddHeader("Ledger", "text-align:center; font-weight:bold; font-size:17px;");
        AddHeader("Date", "text-align:center; font-weight:bold; font-size:17px;");
        if(hidIsParticularsDisplay.Value == Constants.S_YES)
            AddHeader("Particulars", "text-align:left; font-weight:bold; font-size:17px;");
        AddHeader("Voucher Type", "text-align:center; font-weight:bold; font-size:17px;");
        AddHeader("Sr. No.", "text-align:center; font-weight:bold; font-size:17px;");
        AddHeader("Debit (Rs.)", "text-align:center; font-weight:bold; font-size:17px;");
        AddHeader("Credit (Rs.)", "text-align:center; font-weight:bold; font-size:17px;");
        HttpContext.Current.Response.Write("</TR>");
        AddLedgerSummaryDetails(lstVouchers, iLedgerId, iVoucherId);
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    /// <summary>
    /// 	This Method is used for Add Ledgure summary Details.
    /// </summary>
    private void AddLedgerSummaryDetails(List<Voucher> lstVouchers, int iLedgerId, int iVoucherId)
    {       
        foreach (var sVoucher in lstVouchers)
        {
            string sDebitAmount = CommonUtility.FormatCurrency(sVoucher.VoucherParticulars
                                                  .Where(particular => particular.Ledger.Id == iLedgerId && particular.IsDebit)
                                                  .Sum(lst => lst.Amount));
            sDebitAmount = sDebitAmount.Replace(",", string.Empty);
            
            string sCreditAmount = CommonUtility.FormatCurrency(sVoucher.VoucherParticulars
                                          .Where(particular => particular.Ledger.Id == iLedgerId && !particular.IsDebit)
                                          .Sum(lst => lst.Amount));
            sCreditAmount = sCreditAmount.Replace(",", string.Empty);

            string sVOucherParticulars = string.Empty;

            List<VoucherParticular> lstVoucherParticular = sVoucher.VoucherParticulars.Where(lstPerticular => lstPerticular.Ledger.Id != iLedgerId).ToList();
            
            foreach (var Vouchers in lstVoucherParticular)
            {
                bool isDebit = Vouchers.IsDebit.ToBool();
                string sDrCR = string.Empty;
                sDrCR = isDebit ? "Dr" : "Cr";

                sVOucherParticulars = sVOucherParticulars + Vouchers.Ledger.Name + " (" + Vouchers.Amount + " " + sDrCR + ")" + "<BR>";
            }            

            HttpContext.Current.Response.Write("<TR>");
            AddTableRows(ddlLedgers.SelectedItem.ToString(),"text-align:left");
            AddTableRows(sVoucher.Date.ToString(Constants.S_DATE_FORMAT), "text-align:left; vertical-align:middle");
            if (hidIsParticularsDisplay.Value == Constants.S_YES)
                AddTableRows(sVOucherParticulars, "text-align:left");
            AddTableRows(sVoucher.VoucherType.Name.ToString(), "text-align:center");
            AddTableRows(sVoucher.SerialNumber.ToString(), "text-align:center");
            AddTableRows(sDebitAmount, "text-align:center");
            AddTableRows(sCreditAmount, "text-align:center");
            HttpContext.Current.Response.Write("</TR>");
        }
    }

    /// <summary>
    /// 	This method is used for Adding the rows in to Table for exporting ledgure Summary.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<TD " + sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }

    /// <summary>
    /// 	This method is used for Adding the row Header in to Table for exporting ledgure Summary.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

	#endregion -- PRIVATE METHOD(s) --   
}