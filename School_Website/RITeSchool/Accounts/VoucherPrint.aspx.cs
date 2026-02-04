/* ---------------------------------------------------------------------------------
 *	FileName	: VoucherPrint.aspx.cs
 *	Author		: ViPUl A. JAdhAV
 *	Date		: 17-Oct-2011
 *	Description	: This is the code behind file for the Voucher Print screen,
 *				  which is used to display pending/approved/rejected etc vouchers for printing.
 * ---------------------------------------------------------------------------------
 */

using System;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class VoucherPrint : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_DATE_FORMAT = "dd-MMM-yyyy";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountVoucherClient moAccountVoucherClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT(s) --

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			OpenVoucherServiceObj();
			if (!IsPostBack)
			{
				ReadQueryString();
				DisplayHeader();
				DisplayVoucherDetails();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseVoucherServiceObj();
		}
	}

	/// <summary>
	/// This event is handled to set control properties for each row based on the view mode & databound object.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwVoucherDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iDisplayIndex = oCurrentItem.DisplayIndex;
				bool bIsDebit = lstvwVoucherDetails.DataKeys[iDisplayIndex]["IsDebit"].ToBool();
				decimal dAmount = lstvwVoucherDetails.DataKeys[iDisplayIndex]["Amount"].ToDecimal();

				var lblDebitAmount = oCurrentItem.FindControl("lblDebitAmount") as Label;
				var lblCreditAmount = oCurrentItem.FindControl("lblCreditAmount") as Label;
				if (bIsDebit)
				{
					lblDebitAmount.Text = CommonUtility.FormatCurrency(dAmount);
					lblCreditAmount.Text = "&nbsp;";
				}
				else
				{
					lblCreditAmount.Text = CommonUtility.FormatCurrency(dAmount);
					lblDebitAmount.Text = "&nbsp;";
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT(s) --

	#region -- PRIVATE METHODS(s) --

	/// <summary>
	/// This function is used to display ledger details when in new/edit mode.
	/// </summary>
	private void DisplayVoucherDetails()
	{
		int iVoucherId		 = hidVoucherId.Value.ToInt();
		Voucher oVoucherDetails = moAccountVoucherClient.GetVoucherDetails(miSchoolId, miFinancialYearId, iVoucherId, miUserId);

		// Set Voucher details
		lblVoucherType.Text = oVoucherDetails.VoucherType.Name;
		lblDate.Text		= oVoucherDetails.Date.ToString(S_DATE_FORMAT);
		lblSerialNo.Text	= oVoucherDetails.SerialNumber;
		lblCreaterName.Text=  "Creator : "+oVoucherDetails.CreatedBy;
		lblNarration.Text	= String.IsNullOrEmpty(oVoucherDetails.Narration) ? "&nbsp;" : oVoucherDetails.Narration;

		lstvwVoucherDetails.DataSource = oVoucherDetails.VoucherParticulars;
		lstvwVoucherDetails.DataBind();

		var lblDebitTotal = lstvwVoucherDetails.FindControl("lblDebitTotal") as Label;
		var lblCreditTotal = lstvwVoucherDetails.FindControl("lblCreditTotal") as Label;
		lblDebitTotal.Text = lblCreditTotal.Text = CommonUtility.FormatCurrency(oVoucherDetails.Amount);
	}

	/// <summary>
	/// This method is used to display header of receipt.
	/// </summary>
	private void DisplayHeader()
	{
		var oSchoolBL = new SchoolBL(ConfigurationManager.AppSettings["SchoolID"].ToInt());
		lblRegNo.Text = oSchoolBL.RegNo;
		lblSchoolName.Text = oSchoolBL.SchoolName;
		lblAddress.Text = oSchoolBL.Address;
		lblOrgName.Text = oSchoolBL.SchoolOrgnName;
		lblcity.Text = oSchoolBL.City + " - " + oSchoolBL.Pincode;
		//DateTime oDt = Convert.ToDateTime(oSchoolBL.SchoolSinceDate);
		lblPhone.Text = oSchoolBL.PhoneNumber;
	}

	/// <summary>
	/// This function is used to read the query string passed to the page and set some member variables/hidden fields accordingly.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.Count <= 0)
			return;

		if (!QueryString["VoucherId"].IsNullOrEmpty())
			hidVoucherId.Value = QueryString["VoucherId"];
	}

	/// <summary>
	/// This function is used to initialize the Voucherclient service obj.
	/// </summary>
	private void OpenVoucherServiceObj()
	{
		moAccountVoucherClient = new AccountVoucherClient();
		moAccountVoucherClient.Open();
	}

	/// <summary>
	/// This function is used to dispose the Voucherclient service obj.
	/// </summary>
	private void CloseVoucherServiceObj()
	{
		if (moAccountVoucherClient != null && moAccountVoucherClient.State != CommunicationState.Faulted)
			moAccountVoucherClient.Close();
	}

	#endregion -- PRIVATE METHODS(s) --
}