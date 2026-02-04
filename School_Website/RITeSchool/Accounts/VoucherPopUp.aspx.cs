/* ---------------------------------------------------------------------------------
 *	FileName	: VoucherPopUp.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 17-Oct-2011
 *	Description	: This is the code behind file for the Voucher List screen,
 *				  which is used to display pending/approved/rejected  etc vouchers.
 * ------------------
 *	MODIFICATION LOG
 * ------------------
 *	Author		: Vishal B. Shah
 *	Date		: 13-Mar-2012
 *	Purpose		: #4486. Voucher Date should be with in current financial year.
 *				  Voucher Type should be disabled in edit mode.
 * ------------------
 *	Author		: Vishal B. Shah
 *	Date		: 6-Apr-2012
 *	Purpose		: #4562. Approval system change. Now the user has to explicitly submit
 *				  the voucher for approval. If he only saves the voucher, it is saved
 *				  as a draft. He can edit and delete it draft vouchers. As soon as it
 *				  is submitted, he can no longer edit or delete them.
 * ------------------
 *	Author		: Vishal B. Shah
 *	Date		: 14-Apr-2012
 *	Purpose		: #4194. Added an icon for Cheque printing.
 * ---------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using System.Web;

public partial class VoucherPopUp : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_DATE_FORMAT = "dd-MMM-yyyy";

	private const string S_ADD_TITLE = "Create Voucher";
	private const string S_EDIT_TITLE = "Edit Voucher Details";
	private const string S_VIEW_TITLE = "Voucher Details";
	private const string S_UPDATE_MSG = "Voucher saved successfully!!!";
	private const string S_ERROR_MSG = "Failed to save voucher details";
	private const string S_SUBMIT_MSG = "Voucher saved and submitted for approval successfully!!!";
	private const string S_SUBMIT_ERROR_MSG = "Failed to save and submit voucher for approval.";
	private const string S_SAVEAPPROVE_MSG = "Voucher saved and approved successfully!!!";
	private const string S_SAVEAPPROVE_ERROR_MSG = "Failed to save and approve voucher.";
	private const string S_REJECT_ERROR_MSG = "Failed to reject voucher.";
	private const string S_APPROVE_ERROR_MSG = "Failed to approve voucher.";

	private const int I_MAX_VOUCHERS = 20;

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountsBaseClient moAccountsBaseClient;
	private AccountVoucherClient moAccountVoucherClient;
	private AccountLedgerClient moAccountLedgerClient;
    private BankAccountClient moBankAccountClient;

	private List<ChequeConfiguration> mlstChqConfigurations;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	/// Indicates if the current mode is AddMode (new or edit).
	/// </summary>
	protected bool IsAddMode
	{
		get { return Mode == Constants.ViewMode.New || Mode == Constants.ViewMode.Edit; }
	}

	/// <summary>
	/// Indicates if the current voucher is a Fee Voucher.
	/// </summary>
	protected bool IsFeeVoucher
	{
		get { return !hidIsFeeVoucher.Value.IsNullOrEmpty() && hidIsFeeVoucher.Value.ToBool(); }
	}

	protected bool PrintCheque { get; set; }

	/// <summary>
	/// Returns the current view mode of the page.
	/// </summary>
	private Constants.ViewMode Mode
	{
		get
		{
			if (!hidPageMode.Value.IsNullOrEmpty())
				return (Constants.ViewMode)hidPageMode.Value.ToInt();
			return Constants.ViewMode.New;
		}
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to handle loading of the page and display controls accordingly.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
				OpenBaseServiceObj();
				OpenVoucherServiceObj();
				OpenLedgerServiceObj();
				ReadQueryString();
				DisplayControlsAsPerMode();
				Initialize();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseBaseServiceObj();
			CloseVoucherServiceObj();
			CloseLedgerServiceObj();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwLedgers_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to control properties of the list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwVoucherDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwVoucherDetails.Items.Count > 0)
			{
				// If Mode is Add or Edit, set the grand total.
				if (IsAddMode)
				{
					var lblDebitTotal = lstvwVoucherDetails.FindControl("lblDebitTotal") as Label;
					var lblCreditTotal = lstvwVoucherDetails.FindControl("lblCreditTotal") as Label;

					lblDebitTotal.Text = lblCreditTotal.Text = "0.00";

					// We need to set the Add/Delete row button attributes, so iterate over the rows & set the right attributes
					if (Mode == Constants.ViewMode.Edit)
					{
						foreach (ListViewDataItem item in lstvwVoucherDetails.Items)
						{
							var hidLedgerId = item.FindControl("hidLedgerId") as HiddenField;
							HtmlControl rowButton;
							if (!hidLedgerId.Value.IsNullOrEmpty() && hidLedgerId.Value.ToInt() > 0)
							{
								rowButton = item.FindControl("rowButton") as HtmlControl;
								rowButton.Attributes["class"] = "removeButton";
								rowButton.Attributes["onclick"] = String.Format("ShiftRows({0})", item.DisplayIndex);
							}
							else
							{
								rowButton = lstvwVoucherDetails.Items[item.DisplayIndex - 1].FindControl("rowButton") as HtmlControl;
								rowButton.Attributes["class"] = "addButton";
								rowButton.Attributes["onclick"] = String.Format("AddRow({0})", item.DisplayIndex - 1);
							}
						}
					}
				}
				// Else(View mode) hide, some header and footer columns.
				else
				{
					var control = lstvwVoucherDetails.FindControl("thToBy") as HtmlControl;
					control.Visible = false;
					if (!PrintCheque)
					{
						control = lstvwVoucherDetails.FindControl("thActionBtn") as HtmlControl;
						control.Visible = IsFeeVoucher;
						control = lstvwVoucherDetails.FindControl("tdActionBtn") as HtmlControl;
						control.Visible = IsFeeVoucher;
					}

					var oHtmlTableCell = lstvwVoucherDetails.FindControl("tdTotal") as HtmlTableCell;
					oHtmlTableCell.ColSpan = 2;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is handled to set control properties for each row based on the view mode & databound object
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
				var oRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
				var ddlToBy = oCurrentItem.FindControl("ddlToBy") as DropDownList;
				var oRowButton = oCurrentItem.FindControl("rowButton") as HtmlControl;

				switch (Mode)
				{
					// If ViewMode is new, the ListView is setup to show only the first two rows.
					case Constants.ViewMode.New:
						{
							bool bIsDebit = ddlVoucherTypes.SelectedItem.Text == Constants.VoucherType.Payment.ToString();
							switch (iDisplayIndex)
							{
								case 0:
									ddlToBy.SelectedValue = (bIsDebit ? Constants.TransactionType.Debit : Constants.TransactionType.Credit).ToInt().ToString();
									ddlToBy.Enabled = false;
									oRowButton.Visible = false;
									break;
								case 1:
								//	bool bIsDebit = ddlVoucherTypes.SelectedValue == Constants.VoucherType.Receipt.ToString();
									ddlToBy.SelectedValue = (bIsDebit ? Constants.TransactionType.Credit : Constants.TransactionType.Debit).ToInt().ToString();
									var txtDebitAmount = oCurrentItem.FindControl("txtDebitAmount") as TextBox;
									var txtCreditAmount = oCurrentItem.FindControl("txtCreditAmount") as TextBox;
									txtDebitAmount.Attributes.CssStyle["visibility"] = String.Empty;
									txtCreditAmount.Attributes.CssStyle["visibility"] = "hidden";
									break;
								default:
									oRow.Attributes.CssStyle["display"] = "none";
									break;
							}
						}

						break;
					// If it's Edit mode, we only show rows that have some value, rest are hidden.
					case Constants.ViewMode.Edit:
						{
							var hidLedgerId = oCurrentItem.FindControl("hidLedgerId") as HiddenField;
							bool bIsDebit = lstvwVoucherDetails.DataKeys[iDisplayIndex]["IsDebit"].ToBool();

							// If the row doesn't have a LedgerId, we hide it
							if (hidLedgerId.Value.IsNullOrEmpty() || hidLedgerId.Value.ToInt() == 0)
								oRow.Attributes.CssStyle["display"] = "none";
							// If it does, we set the correct values for the controls in the row
							else
							{
								ddlToBy.SelectedValue = (bIsDebit ? Constants.TransactionType.Debit : Constants.TransactionType.Credit).ToInt().ToString();

								// Disable To/By ddl in the first row.
								if (iDisplayIndex == 0)
									ddlToBy.Enabled = false;

								decimal dcAmount = lstvwVoucherDetails.DataKeys[iDisplayIndex]["Amount"].ToDecimal();
								if (bIsDebit)
								{
									var txtDebitAmount = oCurrentItem.FindControl("txtDebitAmount") as TextBox;
									txtDebitAmount.Text = dcAmount.ToString("0.00");
									txtDebitAmount.Attributes.CssStyle["visibility"] = String.Empty;
									var txtCreditAmount = oCurrentItem.FindControl("txtCreditAmount") as TextBox;
									txtCreditAmount.Attributes.CssStyle["visibility"] = "hidden";
								}
								else
								{
									var txtCreditAmount = oCurrentItem.FindControl("txtCreditAmount") as TextBox;
									txtCreditAmount.Text = dcAmount.ToString("0.00");
								}

								oRowButton.Attributes["class"] = "removeButton";
								oRowButton.Attributes["title"] = "Remove row";
								oRowButton.Attributes["onclick"] = String.Format("ShiftRows({0})", iDisplayIndex);
							}
						}

						break;
					// If the page is in View mode, we just set the debit/credit amount.
					case Constants.ViewMode.View:
						{
							bool bIsDebit = lstvwVoucherDetails.DataKeys[iDisplayIndex]["IsDebit"].ToBool();
							decimal dAmount = lstvwVoucherDetails.DataKeys[iDisplayIndex]["Amount"].ToDecimal();

							var lblAmount = oCurrentItem.FindControl(bIsDebit ? "lblDebitAmount" : "lblCreditAmount") as Label;
							lblAmount.Text = CommonUtility.FormatCurrency(dAmount); //dAmount.ToString("0.00");

							// If it's a Fee Voucher, we set the onclick attribute of rowbutton
							// to open a new popup displaying fee details of respective ledger
							if (IsFeeVoucher)
							{
								var hidLedgerId = oCurrentItem.FindControl("hidLedgerId") as HiddenField;
								var hidGroupId = oCurrentItem.FindControl("hidGroupId") as HiddenField;
								var lblLedgerName = oCurrentItem.FindControl("lblLedger") as Label;
								oRowButton.Attributes["class"] = "viewButton";
								oRowButton.Attributes["title"] = "View details";
								oRowButton.Attributes["onclick"] = String.Format("window.open('FeeVoucherDetailsPopup.aspx?{0}', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600'); return false;",
                                                                                CommonUtility.EncryptQuerystring(String.Format("VoucherId={0}&GroupId={1}&LedgerId={2}&LedgerName={3}&SerialNo={4}&IsInternalFeeVoucher={5}",
																																hidVoucherId.Value,
																																hidGroupId.Value,
																																hidLedgerId.Value,
																																lblLedgerName.Text,
																																lblSerialNo.Text,
                                                                                                                                hidIsInternalFeeVoucher.Value)));
							}

							btnPrint.Visible = true;
                            btnExport.Visible = true;
                            btnExportToExcel.Visible = true;
                            tblNote.Visible = true;
							btnPrint.Attributes.Add("onclick", "GeneratePrint(); return false;");
							hidQery.Value = CommonUtility.EncryptQuerystring("VoucherId=" + hidVoucherId.Value);

							if (PrintCheque)
							{
								var oLedger = lstvwVoucherDetails.DataKeys[iDisplayIndex]["Ledger"] as BankAccount;
								if (oLedger != null && mlstChqConfigurations.Any(cfg => cfg.Bank.Id == oLedger.Bank.Id))
								{
									oRowButton.Attributes["class"] = "printButton";
									oRowButton.Attributes["title"] = "Print Cheque";
									oRowButton.Attributes["onclick"] = String.Format("ShowPrintDialog({0}, '{1}')", oLedger.Bank.Id, lblAmount.Text.Replace(",", String.Empty));
								}
								else
									oRowButton.Visible = false;
							}
						}

						break;
				}

				if (IsAddMode)
					oRowButton.Attributes["onclick"] = String.Format("AddRow({0})", iDisplayIndex);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save the Voucher details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		bool bSelfApprove = sender.Equals(btnSelfApprove); //btnSender.Text == btnSelfApprove.Text;
		bool bIsSubmitted = sender.Equals(btnSubmit); //btnSender.Text == btnSubmit.Text;
		if (hidSourceStatusId.Value.ToInt() == Constants.RequisitionStatus.Waiting_For_My_Approval.ToInt())
			bIsSubmitted = true;
		try
		{
			OpenVoucherServiceObj();

			var oVoucherDetails = new Voucher
									{
										SchoolId		   = miSchoolId,
										AcademicYearId	   = miAcademicYearId,
										FinancialYearId    = miFinancialYearId,
										VoucherId		   = !hidVoucherId.Value.IsNullOrEmpty() ? hidVoucherId.Value.ToInt() : 0,
										VoucherType		   = new VoucherType { Id = ddlVoucherTypes.SelectedValue.ToInt() },
										Date			   = dtVoucherDate.DateValue,
										Narration		   = txtNarration.Text.Trim(),
										Amount			   = hidTotalAmount.Value.ToDecimal(),
										IsSubmitted		   = bIsSubmitted,
										Status			   = bSelfApprove ? Constants.RequisitionStatus.Approved : Constants.RequisitionStatus.Pending,
										InsertedById	   = miUserId,
										VoucherParticulars = GetVoucherParticulars()
									};

			oVoucherDetails = moAccountVoucherClient.Save(oVoucherDetails);

			if (oVoucherDetails != null)
			{
				ShowUpdateMessage(bSelfApprove ? S_SAVEAPPROVE_MSG : bIsSubmitted ? S_SUBMIT_MSG : S_UPDATE_MSG);
				if (Mode == Constants.ViewMode.New)
					ResetControls();
				else
					DisplayVoucherDetails();

				// If the PageView mode is New or Edit, we set the onclick attribute of the close button to
				// refresh the parent window (VoucherList) and show the appropriate status of the just save voucher.
				if (Mode == Constants.ViewMode.New || Mode == Constants.ViewMode.Edit)
				{
					// If hidSourceStatusId is not empty, we use it's value in the onclick attribute.
					// Else we use the StatusId of the just saved Voucher.
					string sStatusId = hidSourceStatusId.Value.IsNullOrEmpty() || hidSourceStatusId.Value.ToInt() != Constants.RequisitionStatus.Waiting_For_My_Approval.ToInt()
					                   	? oVoucherDetails.Status.ToInt().ToString()
					                   	: hidSourceStatusId.Value;

					btnClose.Attributes.Remove("onclick");
					btnClose.Attributes["onclick"] = String.Format("window.opener.location = window.opener.location.href.replace(window.opener.location.search, '') + '?{0}'; window.close(); window.opener.focus();",
						CommonUtility.EncryptQuerystring(String.Format("StatusId={0}", sStatusId)));
				}
			}
			else
				ShowErrorMessage(bSelfApprove ? S_SAVEAPPROVE_ERROR_MSG : bIsSubmitted ? S_SUBMIT_ERROR_MSG : S_ERROR_MSG);
		}
		catch (Exception ex)
		{
			ShowErrorMessage(bSelfApprove ? S_SAVEAPPROVE_ERROR_MSG : bIsSubmitted ? S_SUBMIT_ERROR_MSG : S_ERROR_MSG);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseVoucherServiceObj();
		}
	}

	/// <summary>
	/// This event is used to perform an action(approve/reject) on the voucher.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnAction_Click(object sender, EventArgs e)
	{
		var btnSender = sender as Button;
		try
		{
			OpenVoucherServiceObj();
			var oStatus = btnSender.Text == btnReject.Text ? Constants.RequisitionStatus.Denied : Constants.RequisitionStatus.Approved;
			var oAction = new VoucherAction
							{
								SchoolId		= miSchoolId,
								AcademicYearId	= miAcademicYearId,
								FinancialYearId = miFinancialYearId,
								Voucher			= new Voucher
													{
														VoucherId	 = hidVoucherId.Value.ToInt(),
														SerialNumber = lblSerialNo.Text,
														CreatedBy	 = lblCreatedBy.Text,
														Date		 = lblDate.Text.ToDateTime(),
														VoucherType  = new VoucherType { Name = lblVoucherType.Text },
														Status		 = oStatus,
														InsertedById = hidInsertedById.Value.ToInt()
													},
								InsertedById	= miUserId,
								Comment			= txtComment.Text.Trim(),
								Status			= oStatus,
								FinalApprove	= oStatus == Constants.RequisitionStatus.Approved && chkFinalApprove.Checked
							};

			if (moAccountVoucherClient.PerformActionOnVoucher(oAction))
				ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ParentReloader", "window.opener.location.reload(); window.close(); window.opener.focus();", true);
			else
				ShowErrorMessage(oStatus == Constants.RequisitionStatus.Approved ? S_APPROVE_ERROR_MSG : S_REJECT_ERROR_MSG);
		}
		catch (Exception ex)
		{
			ShowErrorMessage(btnSender.Text == btnReject.Text ? S_REJECT_ERROR_MSG : S_APPROVE_ERROR_MSG);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseVoucherServiceObj();
		}
	}

	/// <summary>
	/// Handles the Print cheque action.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnPrintCheque_Click(object sender, EventArgs e)
	{
		try
		{
			string sScriptBlock = String.Format("window.open('../Accounts/PrintCheque.aspx?{0}', '_blank', 'scrollbars=yes,menubar=1,resizable=no,top=0,left=0,width=800,height=280');",
												CommonUtility.EncryptQuerystring(String.Format("ConfigId={0}&IsCrossCheque={1}&ChequeDate={2}&PayeeName={3}&Amount={4}",
																								hidChqConfigId.Value,
																								chkCrossCheque.Checked.ToInt(),
																								txtChequeDate.Text,
																								Server.UrlEncode(txtPayeeName.Text.Trim()),
																								hidChqAmount.Value)));
			ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PrintOpener", sScriptBlock, true);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    ///		Exports the current voucher to xml.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            OpenVoucherServiceObj();
			List<Voucher> lstVouchers = moAccountVoucherClient.Export(miSchoolId, miFinancialYearId, DateTime.MinValue, DateTime.MinValue, false, hidVoucherId.Value.ToInt());
			Accounts.ExportVoucherXML(lstVouchers);
        }
		catch (ThreadAbortException)
		{
			// This exception is generated while exporting the xml.
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
    ///		Exports the current voucher to Excel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ExportVoucherDetailsToExcel();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PROTECTED METHOD(s) --

	/// <summary>
	/// Get called from the markup at the time of data binding LedgerPopup ListView.
	/// Returns a classname for Cash ledger or BankAccount ledgers, empty string otherwise.
	/// </summary>
	/// <param name="aiOriginalGroupId"></param>
	/// <returns></returns>
	protected string GetClassForLedger(int aiOriginalGroupId)
	{
		return aiOriginalGroupId == Constants.AccountsGroups.BankAccounts.ToInt() ? " bank" : aiOriginalGroupId == Constants.AccountsGroups.CashInHand.ToInt() ? " cash" : string.Empty;
	}

	#endregion -- PROTECTED METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This function is used to read the query string passed to the page and set some member variables/hidden fields accordingly.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.Count <= 0)
			return;

		if (!QueryString["VoucherId"].IsNullOrEmpty())
		{
			hidVoucherId.Value = QueryString["VoucherId"];
			if (!QueryString["ViewMode"].IsNullOrEmpty())
				hidPageMode.Value = QueryString["ViewMode"];
		}
		
		if (!QueryString["SourceStatusId"].IsNullOrEmpty())
			hidSourceStatusId.Value = QueryString["SourceStatusId"];
		
		if (Mode != Constants.ViewMode.View)
			return;
		
		if (!QueryString["NextApproverDesigId"].IsNullOrEmpty())
			hidNextApproverDesigId.Value = QueryString["NextApproverDesigId"];
		
		if (!QueryString["NextApproverDesigName"].IsNullOrEmpty())
			hidNextApproverDesigName.Value = QueryString["NextApproverDesigName"];

        if (!QueryString["IsInternalFeeVoucher"].IsNullOrEmpty())
            hidIsInternalFeeVoucher.Value = QueryString["IsInternalFeeVoucher"].ToString();
        else
            hidIsInternalFeeVoucher.Value = Constants.S_ZERO;
	}

	/// <summary>
	/// This function is used to display details on the page according to the Page view mode.
	/// </summary>
	private void DisplayControlsAsPerMode()
	{
		UserPermissions oUserPermissions = moAccountsBaseClient.GetUserPermissions(miSchoolId, miUserId);
		
		switch (Mode)
		{
			case Constants.ViewMode.New:
				DisplayControlsForAddMode();

				btnSubmit.Visible = true;
				btnSelfApprove.Visible = oUserPermissions.CanSelfApprove;
				btnReset.Attributes["onclick"] = "ResetControls(false); return false;";

				FillVoucherTypes();
				FillDefaultVouchers();
				BindLedgersPopup();
				SerializeFinancialYearDetails();
				break;
			case Constants.ViewMode.Edit:
				DisplayControlsForAddMode();
				
				FillVoucherTypes();
				ddlVoucherTypes.Enabled = false;
				DisplayVoucherDetails();
				BindLedgersPopup();
				SerializeFinancialYearDetails();

				lblMainTitle.Text = S_EDIT_TITLE;
				btnReset.Attributes["onclick"] = "ResetControls(true); return false;";
				break;
			case Constants.ViewMode.View:
				lblMainTitle.Text = S_VIEW_TITLE;
				trAddRow.Visible = false;
				btnSave.Visible = false;
				btnReset.Visible = false;
				trApprovalBtns.Visible = !hidSourceStatusId.Value.IsNullOrEmpty() && hidSourceStatusId.Value.ToInt() == Constants.RequisitionStatus.Waiting_For_My_Approval.ToInt() && oUserPermissions.CanApproveVoucher;
				trComment.Visible = trApprovalBtns.Visible;
				trMdtStar.Visible = trComment.Visible;
				trVoucherAction.Visible = true;
				lblNarration.Visible = true;
				txtNarration.Visible = false;

				DisplayVoucherDetails();
				DisplayVoucherActions();
				break;
		}
	}

	/// <summary>
	/// This function is used to bind a defualt list of vouchers to the list view when in view mode.
	/// </summary>
	private void FillDefaultVouchers()
	{
		var lstLedgers = new List<VoucherParticular>();
		for (int i = 1; i <= I_MAX_VOUCHERS; i++)
			lstLedgers.Add(new VoucherParticular());

		lstvwVoucherDetails.DataSource = lstLedgers;
		lstvwVoucherDetails.DataBind();
	}

	/// <summary>
	/// This function is used to populate the voucher types radio btn list.
	/// </summary>
	private void FillVoucherTypes()
	{
		ddlVoucherTypes.DataSource = moAccountVoucherClient.GetAllVoucherTypes(miSchoolId, miFinancialYearId, false);
		ddlVoucherTypes.DataTextField = "Name";
		ddlVoucherTypes.DataValueField = "Id";
		ddlVoucherTypes.DataBind();
	}

	/// <summary>
	/// This function is used to bind the Ledgers popup.
	/// </summary>
	private void BindLedgersPopup()
	{
		List<Ledger> lstAllLedgers = moAccountLedgerClient.AllLedgers(miSchoolId, miFinancialYearId);

		lstvwLedgers.DataSource = lstAllLedgers;
		lstvwLedgers.DataBind();
	}

	/// <summary>
	/// Serializes the FinancialYearMaster entity object to a hidden field.
	/// </summary>
	private void SerializeFinancialYearDetails()
	{
		var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
		if (oFinancialYear != null)
		{
			var jsSerializer = new JavaScriptSerializer();
			hidFinancialYearJSON.Value = jsSerializer.Serialize(oFinancialYear);
		}

		if (Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] != null)
			hidCanEditOldFinancialYear.Value = Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR].ToString();
	}

	/// <summary>
	/// This function is used to display ledger details when in new/edit mode.
	/// </summary>
	private void DisplayVoucherDetails()
	{
		Voucher oVoucher = moAccountVoucherClient.GetVoucherDetails(miSchoolId, miFinancialYearId, hidVoucherId.Value.ToInt(), miUserId);
		PrintCheque = oVoucher.Status == Constants.RequisitionStatus.Approved &&
					    !oVoucher.IsFeeVoucher &&
						  oVoucher.VoucherType.Name == Constants.VoucherType.Payment.ToString() &&
						    oVoucher.VoucherParticulars.Any(vp => !vp.IsDebit && vp.Ledger.Group.Id == Constants.AccountsGroups.BankAccounts.ToInt());

		if (Mode == Constants.ViewMode.View)
		{
			// Set Voucher details
			lblVoucherType.Text		= oVoucher.VoucherType.Name;
			lblCreatedBy.Text		= oVoucher.CreatedBy;
			lblDate.Text			= oVoucher.Date.ToString(S_DATE_FORMAT);
			lblSerialNo.Text		= oVoucher.SerialNumber;
			lblNarration.Text		= oVoucher.Narration.IsNullOrEmpty() ? "&nbsp;" : oVoucher.Narration;
			hidInsertedById.Value	= oVoucher.InsertedById.ToString();
			hidCurrentDesigId.Value = oVoucher.CurrentUserDesigId.ToString();
			chkFinalApprove.Visible = oVoucher.IsFinalApprover;
			hidIsFeeVoucher.Value	= oVoucher.IsFeeVoucher.ToString();

			if (PrintCheque)
			{
				GetChequeConfigurations();
				SerializeChqConfigurations(oVoucher.VoucherParticulars);
				txtPayeeName.Text = oVoucher.VoucherParticulars.FirstOrDefault().Ledger.Name;
				txtChequeDate.Text = oVoucher.Date.ToString(S_DATE_FORMAT);
			}
		}
		else
		{
			ddlVoucherTypes.ClearSelection();
			ddlVoucherTypes.SelectedValue = oVoucher.VoucherType.Id.ToString();
			txtVoucherDate.Text = oVoucher.Date.ToString(S_DATE_FORMAT);
			hidTotalAmount.Value = oVoucher.Amount.ToString("0.00");

			if (Mode == Constants.ViewMode.Edit && !oVoucher.IsSubmitted && oVoucher.Status != Constants.RequisitionStatus.Approved)
			{
				btnSubmit.Visible = true;
				ddlVoucherTypes.Enabled = true;
			}
		}

		txtNarration.Text = oVoucher.Narration;
		// When the function is called when ViewMode is 'EDIT', it's possible the user might want to add more details to the Voucher
		// Hence we must make sure there are enough rows in the ListView.
		if (Mode == Constants.ViewMode.Edit && oVoucher.VoucherParticulars.Count < I_MAX_VOUCHERS)
		{
			for (int i = oVoucher.VoucherParticulars.Count; i < I_MAX_VOUCHERS; i++)
				oVoucher.VoucherParticulars.Add(new VoucherParticular());
		}

		lstvwVoucherDetails.DataSource = oVoucher.VoucherParticulars;
		lstvwVoucherDetails.DataBind();

		// Set the Debit/Credit sub-total
		var lblDebitTotal = lstvwVoucherDetails.FindControl("lblDebitTotal") as Label;
		var lblCreditTotal = lstvwVoucherDetails.FindControl("lblCreditTotal") as Label;
		lblDebitTotal.Text = lblCreditTotal.Text = CommonUtility.FormatCurrency(oVoucher.Amount);
	}

	/// <summary>
	/// This function displays any voucher actions taken so far on the current voucher being displayed.
	/// </summary>
	private void DisplayVoucherActions()
	{
		lstvwVoucherAction.DataSource = moAccountVoucherClient.GetVoucherActionDetails(miSchoolId, miFinancialYearId, hidVoucherId.Value.ToInt());
		lstvwVoucherAction.DataBind();
	}

	/// <summary>
	/// This function is used to initialize and set default values for controls.
	/// </summary>
	private void Initialize()
	{
		ApplyMouseHoverEffect(new List<Button> { btnReject, btnApprove, btnClose, btnSave, btnReset, btnPrint, btnExport, btnSubmit, btnCancel, btnPrintCheque, btnSelfApprove, btnSelfApprove, btnExportToExcel });

		valSummarySave.HeaderText = valSummaryApprove.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		if (Mode != Constants.ViewMode.Edit)
			dtVoucherDate.SetDateValue(DateTime.Now);
		if (IsAddMode)
			hidVoucherType.Value = ddlVoucherTypes.SelectedItem.Text;
	}

	/// <summary>
	/// This function sets the control properties when in new/edit mode.
	/// </summary>
	private void DisplayControlsForAddMode()
	{
		lblMainTitle.Text = S_ADD_TITLE;
		trViewRow.Visible = false;
		trApprovalBtns.Visible = false;
		trComment.Visible = false;
		trMdtStar.Visible = trComment.Visible;
	}

	/// <summary>
	/// This function is used to popoulate a list of Voucher Particulars from the user input on page.
	/// </summary>
	/// <returns></returns>
	private List<VoucherParticular> GetVoucherParticulars()
	{
		var lstVoucherParticulars = new List<VoucherParticular>();
		
		foreach (ListViewDataItem item in lstvwVoucherDetails.Items)
		{
			var hidLedgerId = item.FindControl("hidLedgerId") as HiddenField;
			
			if (hidLedgerId.Value.IsNullOrEmpty() || hidLedgerId.Value == "0")
				continue;
			
			int iVoucherParticularsId = lstvwVoucherDetails.DataKeys[item.DisplayIndex]["Id"].ToInt();
			var txtDebitAmount = item.FindControl("txtDebitAmount") as TextBox;
			var ddlToBy = item.FindControl("ddlToBy") as DropDownList;
			decimal amount = 0.00m;
			if (ddlToBy.SelectedIndex == Constants.TransactionType.Debit.ToInt() && !txtDebitAmount.Text.IsNullOrEmpty())
				amount = txtDebitAmount.Text.ToDecimal();
			else if (ddlToBy.SelectedIndex == Constants.TransactionType.Credit.ToInt())
			{
				var txtCreditAmount = item.FindControl("txtCreditAmount") as TextBox;
				if (!txtCreditAmount.Text.IsNullOrEmpty())
					amount = txtCreditAmount.Text.ToDecimal();
			}

			if (amount > 0)
				lstVoucherParticulars.Add(new VoucherParticular
											  {
												  Id	  = iVoucherParticularsId,
												  Ledger  = new Ledger { Id = hidLedgerId.Value.ToInt() },
												  IsDebit = ddlToBy.SelectedIndex == Constants.TransactionType.Debit.ToInt(),
												  Amount  = amount
											  });
		}

		return lstVoucherParticulars;
	}

	/// <summary>
	/// Gets all availabe Chq configurations for the School.
	/// </summary>
	private void GetChequeConfigurations()
	{
		OpenBankServiceObject();
		mlstChqConfigurations = moBankAccountClient.GetChequeConfigurations(miSchoolId);
		CloseBankServiceObject();
	}

	/// <summary>
	/// Serializes the applicable Chq configurations to JSON and saves it to a hidden field.
	/// </summary>
	/// <param name="alstVoucherParticulars"></param>
	private void SerializeChqConfigurations(List<VoucherParticular> alstVoucherParticulars)
	{
		// We get all Ledgers from VoucherParticulars which are a BankAccount.
		var lstBankAccounts = alstVoucherParticulars.Where(vp => vp.Ledger is BankAccount).Select(vp => vp.Ledger as BankAccount).ToList();

		mlstChqConfigurations.RemoveAll(cfg => lstBankAccounts.All(b => b.Bank.Id != cfg.Bank.Id));

		Dictionary<string, object> oDict = (from cfg in mlstChqConfigurations
											group cfg by cfg.Bank.Id into grp
											select grp).ToDictionary(b => b.Key.ToString(),
		                    										 b => (from c in b.ToList() 
		                    										       select new { c.Id, c.Name }) as object);
		var jsSerializer = new JavaScriptSerializer();
		hidChqConfigJSON.Value = String.Format("[{0}]" , jsSerializer.Serialize(oDict));
	}

	/// <summary>
	/// This function is used to display an Update message on the page.
	/// </summary>
	/// <param name="asMessage"></param>
	private void ShowUpdateMessage(string asMessage)
	{
		lblUpateMessage.Text = asMessage;
		lblUpateMessage.Visible = true;
		lblErrorMessage.Visible = false;
	}

	/// <summary>
	/// This function is used to display an error message on the page.
	/// </summary>
	/// <param name="asMessage"></param>
	private void ShowErrorMessage(string asMessage)
	{
		lblErrorMessage.Text = asMessage;
		lblErrorMessage.Visible = true;
		lblUpateMessage.Visible = false;
	}

	/// <summary>
	/// This function is used to reset controls to their default values.
	/// </summary>
	private void ResetControls()
	{
		FillDefaultVouchers();
		ddlVoucherTypes.SelectedIndex = 0;
		dtVoucherDate.SetDateValue(DateTime.Now);
		txtNarration.Text = string.Empty;
		hidVoucherId.Value = string.Empty;
		hidVoucherType.Value = ddlVoucherTypes.SelectedItem.Text;
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

	/// <summary>
	/// This function is used to initialize the Ledger client service obj.
	/// </summary>
	private void OpenLedgerServiceObj()
	{
		moAccountLedgerClient = new AccountLedgerClient();
		moAccountLedgerClient.Open();
	}

	/// <summary>
	/// This function is used to dispose off the Ledger service obj.
	/// </summary>
	private void CloseLedgerServiceObj()
	{
		if (moAccountLedgerClient != null && moAccountLedgerClient.State != CommunicationState.Faulted)
			moAccountLedgerClient.Close();
	}

	/// <summary>
	/// This function is used to initialize the Accounts base service obj.
	/// </summary>
	private void OpenBaseServiceObj()
	{
		moAccountsBaseClient = new AccountsBaseClient();
		moAccountsBaseClient.Open();
	}

	/// <summary>
	/// This function is used to dispose off the Accounts base service obj.
	/// </summary>
	private void CloseBaseServiceObj()
	{
		if (moAccountsBaseClient != null && moAccountsBaseClient.State != CommunicationState.Faulted)
			moAccountsBaseClient.Close();
	}

	/// <summary>
	/// Initializes the AccountsBank service object.
	/// </summary>
	private void OpenBankServiceObject()
	{
        moBankAccountClient = new BankAccountClient();
		moBankAccountClient.Open();
	}
	
	/// <summary>
	/// Disposes the AccountsBank service object.
	/// </summary>
	private void CloseBankServiceObject()
	{
		if (moBankAccountClient != null && moBankAccountClient.State != CommunicationState.Faulted)
			moBankAccountClient.Close();
	}

    /// <summary>
    /// This method is used to export voucher details in Excel format.
    /// </summary>
    private void ExportVoucherDetailsToExcel()
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-Voucher Details.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
        HttpContext.Current.Response.Write("<TR>");

        AddHeader("Serial No.", "align = left");
        AddHeader("Voucher Type", "align = left");
        AddHeader("Date", "align = center");
        AddHeader("Created By", "align = left");
        AddHeader("Particulars", "align = left");
        AddHeader("Debit (Rs.)", "align = right");
        AddHeader("Credit (Rs.)", "align = right");
        HttpContext.Current.Response.Write("</TR>");

        AddVoucherDetails();

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    /// <summary>
    /// 	This method is used for Adding the row Header in to Table for exporting ledgure Summary.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = asStyle;
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

    private void AddVoucherDetails()
    {
        OpenVoucherServiceObj();
        Voucher oVoucher = moAccountVoucherClient.GetVoucherDetails(miSchoolId, miFinancialYearId, hidVoucherId.Value.ToInt(), miUserId);        
        List<VoucherParticular> lstVoucherParticular = new List<VoucherParticular>();

        lstVoucherParticular = oVoucher.VoucherParticulars.ToList();

        int ival = Constants.I_ZERO;

        foreach (var sVoucher in lstVoucherParticular)
        {
            ival = ival + 1;
            bool IsDebit = sVoucher.IsDebit.ToBool();

            HttpContext.Current.Response.Write("<TR>");

            if (ival == Constants.I_ONE)
            {
                AddTableRows(lblSerialNo.Text, "align = left");
                AddTableRows(lblVoucherType.Text, "align = left");
                AddTableRows(lblDate.Text, "align = center");
                AddTableRows(lblCreatedBy.Text, "text-align:left");
            }
            else
            {
                AddTableRows(string.Empty);
                AddTableRows(string.Empty);
                AddTableRows(string.Empty);
                AddTableRows(string.Empty);
            }
            AddTableRows(sVoucher.Ledger.Name, "text-align:left");
            if(IsDebit)
                AddTableRows(sVoucher.Amount.ToString(), "align = right");
            else
                AddTableRows(Constants.S_ZERO, "align = right");

             if(!IsDebit)
                 AddTableRows(sVoucher.Amount.ToString(), "align = right");
             else
                 AddTableRows(Constants.S_ZERO, "align = right");

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("<TR>");
        AddTableRows(string.Empty);
        AddTableRows(string.Empty);
        AddTableRows(string.Empty);
        AddTableRows(string.Empty);
        AddTableRows("Total (Rs.) :", "align = right");
        AddTableRows(oVoucher.Amount.ToString(), "text-align:left");
        AddTableRows(oVoucher.Amount.ToString(), "text-align:left");
        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// 	This method is used for Adding the rows in to Table for exporting ledgure Summary.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle =  asStyle;
        HttpContext.Current.Response.Write("<TD " + sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }
	#endregion -- PRIVATE METHOD(s) --    
}