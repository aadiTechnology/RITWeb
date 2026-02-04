/* --------------------------------------------------------------------------------
 *	FileName	: FeeVoucherDetailsPopup.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 24-Nov-2011
 *	Description	: This is the code behind file for the Fee Voucher Details screen,
 *				  which shows details about fee payments.
 * --------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using System.Data;

public partial class FeeVoucherDetailsPopup : SchoolBase
{

	#region -- CONSTANT(s) --

	private const int I_MAX_LENGTH = 35;
    private const int I_FIXED_COLUMN_COUNT = 5;
    private const string S_STUDENT_NAME = "Student Name (Reg. No.)";
    private const string S_STUDENT_CLASS = "Class";
    private const string S_ACADEMIC_YEAR = "Academic Year";
    private const string S_PAYMENT_MODE = "Payment Mode";
    private const string S_AMOUNT = "Amount (Rs.)";
    private const string S_TRANSACTIONNUMBER = "Transaction No";
    
	#endregion -- CONSTANT(s) --
	
	#region -- MEMBER(s) --

	private int miVoucherId;
	private int miLedgerId;
	private int miGroupId;
    private int miRowCount;

	private string msLedgerName;
	private string msSerialNo;
    private string msIsInternalFeeVoucher;
    
	private AccountVoucherClient moVoucherClient;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	/// Indicates whether the current the Ledger is a Fee Ledger.
	/// </summary>
	protected bool IsFeeHead
	{
		get { return !(miGroupId == (int)Constants.AccountsGroups.BankAccounts || miGroupId == (int)Constants.AccountsGroups.CashInHand); }
	}

	#endregion -- PROPERTIES --

	#region -- EVENT(s) --

	/// <summary>
	/// This even is used to handle the loading of the Page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				ReadQueryString();
				InitVoucherService();
				DisplayDetails();
				Initialize();
				CloseVoucherService();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to show or hide certain column headers in the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFeeVoucherDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwFeeVoucherDetails.Items.Count > 0)
			{
				var oHtmlTableCell = lstvwFeeVoucherDetails.FindControl("trPayableFor") as HtmlTableCell;
				oHtmlTableCell.Visible = !IsFeeHead;

				oHtmlTableCell = lstvwFeeVoucherDetails.FindControl("trDepositedIn") as HtmlTableCell;
				oHtmlTableCell.Visible = IsFeeHead;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		This event is used to trim the Payable for text incase it exceeds the I_MAX_LENGTH limit.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwFeeVoucherDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem && !IsFeeHead)
			{
				var lblPaybleFor = e.Item.FindControl("lblPayableFor") as Label;
				lblPaybleFor.Truncate(I_MAX_LENGTH, true);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to set the properties of a gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPayments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            TableCellCollection cells = e.Row.Cells;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int iCellCount = 0; iCellCount < I_FIXED_COLUMN_COUNT; iCellCount++)
                {
                    e.Row.Cells[iCellCount].Wrap = false;
                    
                    e.Row.Cells[iCellCount].Style.Add("font-size", "9pt");
                    if (iCellCount == 0)
                    {
                        e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Cells[iCellCount].Style.Add("padding-left", "10px");                      
                    }
                    else
                    {
                        e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[iCellCount].Style.Add("padding", "10px");
                    }
                }

                for (int iCellCount = I_FIXED_COLUMN_COUNT; iCellCount < cells.Count; iCellCount++)
                {
                    e.Row.Cells[iCellCount].Wrap = false;
                    e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Right;                    
                    e.Row.Cells[iCellCount].Style.Add("padding", "10px");                    
                }               

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int iCellCount = 0; iCellCount < I_FIXED_COLUMN_COUNT; iCellCount++)
                {
                    if (iCellCount == 0)
                    {
                        e.Row.Cells[iCellCount].Wrap = false;
                        e.Row.Cells[iCellCount].Style.Add("padding-left", "10px");
                        e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        e.Row.Cells[iCellCount].Wrap = false;
                        e.Row.Cells[iCellCount].Style.Add("padding", "10px");
                        e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                for (int iCellCount = I_FIXED_COLUMN_COUNT; iCellCount < cells.Count; iCellCount++)
                {
                    e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[iCellCount].Style.Add("padding", "10px");
                    //e.Row.Cells[iCellCount].ToolTip = grdPayments.Columns[5].HeaderText;
                }

                if (e.Row.RowIndex == miRowCount-1)
                {
                    for (int iCellCount = 1; iCellCount < cells.Count - 1; iCellCount++)
                        e.Row.Cells[iCellCount].Visible = false;

                    e.Row.CssClass = "ClsBorderPager ClsUnread";                    
                    e.Row.Cells[0].Text = "Total (Rs.) : ";
                    e.Row.Cells[0].CssClass = "ClsUnread";                    
                    e.Row.Cells[0].Style.Add("padding-right", "5px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[0].ColumnSpan = cells.Count - 1;
                    e.Row.Height = Unit.Pixel(10);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdPayments_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int iColumnCount = I_FIXED_COLUMN_COUNT; iColumnCount < grdPayments.HeaderRow.Cells.Count - 1; iColumnCount++)
            {
                string sFeeType = grdPayments.HeaderRow.Cells[iColumnCount].Text.ToString();
                for (int iRowCount = 0; iRowCount < grdPayments.Rows.Count; iRowCount++)
                    grdPayments.Rows[iRowCount].Cells[iColumnCount].ToolTip = sFeeType;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// Reads from the QueryString and stores values into member variables for later usage.
	/// </summary>
	private void ReadQueryString()
	{
	    if (Request.QueryString.Count <= 0)
			return;
	    
		if (!QueryString["VoucherId"].IsNullOrEmpty())
	        miVoucherId = QueryString["VoucherId"].ToInt();
	    
		if (!QueryString["LedgerId"].IsNullOrEmpty())
	        miLedgerId = QueryString["LedgerId"].ToInt();
	    
		if (!QueryString["GroupId"].IsNullOrEmpty())
	        miGroupId = QueryString["GroupId"].ToInt();
	    
		if (!QueryString["LedgerName"].IsNullOrEmpty())
	        msLedgerName = QueryString["LedgerName"];
	    
		if (!QueryString["SerialNo"].IsNullOrEmpty())
	        msSerialNo = QueryString["SerialNo"];

        if (!QueryString["IsInternalFeeVoucher"].IsNullOrEmpty())
            msIsInternalFeeVoucher = QueryString["IsInternalFeeVoucher"].ToString();
    }

	/// <summary>
	/// Displays the Fee voucher details on the screen.
	/// </summary>
	private void DisplayDetails()
    {
        List<FeeReceiptDetails> lstFeeReceiptDetails = new List<FeeReceiptDetails>();
        List<FeeVoucherDetails> lstFee = new List<FeeVoucherDetails>();

        if (msIsInternalFeeVoucher == "1")
           lstFee = moVoucherClient.GetInternalFeeVoucherDetails(miSchoolId, miAcademicYearId, miFinancialYearId, miVoucherId, miLedgerId, ref lstFeeReceiptDetails);
        else
           lstFee = moVoucherClient.GetFeeVoucherDetails(miSchoolId, miAcademicYearId, miFinancialYearId, miVoucherId, miLedgerId, ref lstFeeReceiptDetails);

        lstvwFeeVoucherDetails.DataSource = lstFee;
        lstvwFeeVoucherDetails.DataBind();

        if (lstFee.Count <= 0)
            return;

        trDetails.Visible = true;
        lblFeeParticular.Text = msLedgerName;
        lblSerialNo.Text = msSerialNo;

        decimal dTotal = lstFee.Sum(fee => fee.Amount);

        var lblTotal = lstvwFeeVoucherDetails.FindControl("lblTotal") as Label;
        lblTotal.Text = CommonUtility.FormatCurrency(dTotal);

        if (!IsFeeHead)
        {
            divContainer.Visible = true;
            divFeeVoucherDetails.Visible = false;
            GenerateVoucherDetails(lstFee, lstFeeReceiptDetails, dTotal);
        }
    }

    /// <summary>
    /// This function is used to generate the voucher details dynamically.
    /// </summary>
    /// <param name="alstFee"></param>
    /// <param name="alstFeeReceiptDetails"></param>
    /// <param name="adTotal"></param>
    private void GenerateVoucherDetails(List<FeeVoucherDetails> alstFee,List<FeeReceiptDetails> alstFeeReceiptDetails,decimal adTotal)
    {
        DataTable oDataTable = new DataTable();
        oDataTable.Columns.AddRange(new DataColumn[] { new DataColumn(S_STUDENT_NAME),
                            new DataColumn(S_STUDENT_CLASS),
                            new DataColumn(S_ACADEMIC_YEAR),
                            new DataColumn(S_PAYMENT_MODE),
                            new DataColumn(S_TRANSACTIONNUMBER)
                            });

        var lstFeeTypes = alstFeeReceiptDetails.Select(a => a.FeeType).Distinct().ToList();

        foreach (string sFeeType in lstFeeTypes)
        {
            oDataTable.Columns.Add(sFeeType);
        }

        oDataTable.Columns.Add(S_AMOUNT);

        foreach (FeeVoucherDetails oFeeVoucherDetails in alstFee)
        {
            DataRow oDataRow = oDataTable.NewRow();
            oDataRow[S_STUDENT_NAME] = oFeeVoucherDetails.StudentName+ " ("+ oFeeVoucherDetails.RegNo+")";
            oDataRow[S_STUDENT_CLASS] = oFeeVoucherDetails.Class;
            oDataRow[S_TRANSACTIONNUMBER] = oFeeVoucherDetails.TransactionNumber;
            oDataRow[S_ACADEMIC_YEAR] = oFeeVoucherDetails.AcademicYear;
            oDataRow[S_PAYMENT_MODE] = oFeeVoucherDetails.PaymentMode;              
            oDataRow[S_AMOUNT] = oFeeVoucherDetails.Amount;

            for (int iColIndx = I_FIXED_COLUMN_COUNT; iColIndx < oDataTable.Columns.Count - 1; iColIndx++)
            {
                var oResult = alstFeeReceiptDetails.Where(a => a.ReceiptNumber == oFeeVoucherDetails.ReceiptNumber && a.FeeType == oDataTable.Columns[iColIndx].ColumnName).FirstOrDefault();
                oDataRow[oDataTable.Columns[iColIndx].ColumnName] = oResult == null ? 0 : oResult.Amount;
            }

            oDataTable.Rows.Add(oDataRow);
        }

        DataRow oLastRow = oDataTable.NewRow();
        oLastRow[S_STUDENT_NAME] = string.Empty;
        oLastRow[S_STUDENT_CLASS] = string.Empty;
        oLastRow[S_TRANSACTIONNUMBER] = string.Empty;
        oLastRow[S_ACADEMIC_YEAR] = string.Empty;
        oLastRow[S_PAYMENT_MODE] = string.Empty;
        oLastRow[S_AMOUNT] = CommonUtility.FormatCurrency(adTotal);
        oDataTable.Rows.Add(oLastRow);
        miRowCount = oDataTable.Rows.Count;
        grdPayments.DataSource = oDataTable;
        grdPayments.DataBind();

    }

	/// <summary>
	/// Sets default values for controls.
	/// </summary>
	private void Initialize()
	{
		ApplyMouseHoverEffect(new List<Button> { btnClose });
	}

	/// <summary>
	/// Initializes the AccountVoucher client object.
	/// </summary>
	private void InitVoucherService()
	{
		if (moVoucherClient == null)
			moVoucherClient = new AccountVoucherClient();
	}

	/// <summary>
	/// Disposes off the AccountVoucher client object.
	/// </summary>
	private void CloseVoucherService()
	{
		if (moVoucherClient != null && moVoucherClient.State != CommunicationState.Faulted)
			moVoucherClient.Close();
	}

	#endregion -- PRIVATE METHOD(s) --   
    
}