/* -------------------------------------------------------------------------------
 *	FileName	: VoucherListUI.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 17-Oct-2011
 *	Description	: This is the code behind file for the Vouchers screen, which
 *				  is used to create and manage vouchers for the accounts module.
 * ------------------
 *  MODIFICATION LOG
 * ------------------
 *  Author		: Vishal B. Shah
 *  Date		: 28-Jan-2012
 *  Purpose		: Added a note to inform user if he has access to Ledgers
 *				  configuration screen.
 * ------------------
 *  Author		: Vishal B. Shah
 *  Date		: 13-Feb-2012
 *  Purpose		: Added new entry in the Status drop down list - 'Actioned by me'.
 *				  It shows all vouchers that have been actioned by the currently
 *				  logged in user.
 * ------------------
 *	Author		: Vishal B. Shah
 *	Date		: 13-Mar-2012
 *	Purpsoe		: Disable 'Add' button when financial year is closed and user
 *				  does not have Old financial year edit access.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class VoucherListUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_DB_COLUMN_STATUS_ID = "Id";
	private const string S_DB_COLUMN_STATUS_NAME = "Name";

	private const string S_SORT_EXP = "SerialNumber";
	private const string S_WAITINGFORAPPROVAL_SORT_EXP = "Default";

	private const string S_SORT_ROW = "SORT_ROW";
	private const string S_DELETE_ROW = "DELETE_ROW";
	private const string S_SUBMIT_ROW = "SUBMIT_ROW";

	private const string S_UPDATE_MSG = "Voucher deleted successfully!!!";
	private const string S_UPDATE_ERROR_MSG = "Failed to delete voucher.";
	private const string S_APPROVAL_MSG = "Voucher submitted for approval successfully!!!";
	private const string S_APPROVAL_ERROR_MSG = "Failed to submit voucher for approval.";

	private const string S_FINANCIAL_YEAR_CLOSE_MSG = "The financial year is closed and you do not have edit access.";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private AccountsBaseClient moAccountsBaseClient;
	private AccountVoucherClient moAccountVoucherClient;

	private FinancialYear moFinancialYear;
	private UserPermissions moUserPermissions;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	/// Gets the Status selected in the dropdown list.
	/// </summary>
	private Constants.RequisitionStatus Status
	{
		get
		{
			return (Constants.RequisitionStatus)Enum.Parse(typeof(Constants.RequisitionStatus), ddlStatus.SelectedValue);
		}
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to initialize controls & member variables
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			OpenBaseServiceObj();
			SetMemberVariables();
			if (!IsPostBack)
			{
				OpenVoucherServiceObj();
				ReadQueryString();
				ProcessPermissions();
				FillStatusDropDownList();
				DisplayVouchers();
				Initialize();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseVoucherServiceObj();
			CloseBaseServiceObj();
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
	/// Rebinds the VoucherList on Status change.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			SetDefaultSortVariables();
			ReBindVoucherList();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Sets the properties of the DataPager control for the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
	/// Initializes values for non-databound controls in each row.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwVouchers_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;

				// Hide the created by column if Status is not Waiting for my Approval
				if (Status != Constants.RequisitionStatus.Waiting_For_My_Approval &&
					Status != Constants.RequisitionStatus.Actioned_By_Me)
				{
					var cell = oCurrentItem.FindControl("tdCreatedBy") as HtmlTableCell;
					cell.Visible = false;
					if (Status != Constants.RequisitionStatus.Pending)
					{
						cell = oCurrentItem.FindControl("tdIsSubmitted") as HtmlTableCell;
						cell.Visible = false;
						cell = oCurrentItem.FindControl("tdNextApprover") as HtmlTableCell;
						cell.Visible = false;
					}
				}
				else
				{
					var cell = oCurrentItem.FindControl("tdIsSubmitted") as HtmlTableCell;
					cell.Visible = Status == Constants.RequisitionStatus.Pending;
				}
					

				// Set attributes for action buttons
                string sVoucherId = "";
                if (lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["VoucherId"] != null && lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["VoucherId"].ToString() != string.Empty)
					sVoucherId= lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["VoucherId"].ToString();
                
				string sNextApproverDesigId = "";
                if (lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["NextApproverDesigId"] != null && lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["NextApproverDesigId"].ToString() != string.Empty)
					sNextApproverDesigId = lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["NextApproverDesigId"].ToString();
				
				var lblNextApprover = oCurrentItem.FindControl("lblNextApprover") as Label;
				string sOnClickAttr = "window.open('VoucherPopUp.aspx?{0}', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600'); return false;";

				var imgbtnView = oCurrentItem.FindControl("imgbtnView") as ImageButton;
				imgbtnView.Attributes["onclick"] = String.Format(sOnClickAttr, CommonUtility.EncryptQuerystring(String.Format("ViewMode={0}&VoucherId={1}&NextApproverDesigId={2}&NextApproverDesigName={3}&SourceStatusId={4}",
																															   Constants.ViewMode.View.ToInt(),
																															   sVoucherId,
																															   sNextApproverDesigId,
																															   lblNextApprover.Text,
																															   Status.ToInt())));

				var imgbtnEdit = oCurrentItem.FindControl("imgbtnEdit") as ImageButton;
				var imgbtnDelete = oCurrentItem.FindControl("imgbtnDelete") as ImageButton;

				bool bIsSubmitted = lstvwVouchers.DataKeys[oCurrentItem.DisplayIndex]["IsSubmitted"].ToBool();
				if (!bIsSubmitted)
				{
					var obtnSubmit = oCurrentItem.FindControl("btnSubmit") as Button;
					ApplyMouseHoverEffect(new List<Button> { obtnSubmit });
				}

				if (Status == Constants.RequisitionStatus.Denied || (Status != Constants.RequisitionStatus.Waiting_For_My_Approval && bIsSubmitted))
				{
					imgbtnEdit.Style["visibility"] = "hidden";
					imgbtnDelete.Style["visibility"] = "hidden";
					imgbtnDelete.CommandArgument = String.Empty;
				}
				else
				{
					imgbtnEdit.Attributes["onclick"] = String.Format(sOnClickAttr, CommonUtility.EncryptQuerystring(String.Format("ViewMode={0}&VoucherId={1}&SourceStatusId={2}",
																																   Constants.ViewMode.Edit.ToInt(),
																																   sVoucherId,
																																   Status.ToInt())));
					imgbtnDelete.Attributes["onclick"] = "if(!WarnOnDelete()){return false;}";
				}

				// Temporary till we have a new screen for rights management.
				imgbtnDelete.Style["visibility"] = "hidden";
				imgbtnDelete.CommandArgument = String.Empty;
				if (Status == Constants.RequisitionStatus.Pending && !bIsSubmitted)
				{
					imgbtnDelete.Style["visibility"] = "visible";
					imgbtnDelete.CommandArgument = S_DELETE_ROW;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Initializes the DataPager control and sets visibility of certain columns based on the Status property.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwVouchers_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwVouchers.Items.Count > 0)
			{
				var row = lstvwVouchers.FindControl("trHeader") as HtmlTableRow;
				var cellCreatedBy = row.FindControl("thCreatedBy") as HtmlTableCell;
				var cellNextApprover = row.FindControl("thNextApprover") as HtmlTableCell;
				var cellIsSubmitted = row.FindControl("thIsSubmitted") as HtmlTableCell;
				if (Status != Constants.RequisitionStatus.Waiting_For_My_Approval &&
					Status != Constants.RequisitionStatus.Actioned_By_Me)
				{
					cellCreatedBy.Visible = false;
					cellNextApprover.Visible = Status == Constants.RequisitionStatus.Pending;
					cellIsSubmitted.Visible = Status == Constants.RequisitionStatus.Pending;
				}
				else
				{
					cellCreatedBy.Visible = true;
					cellNextApprover.Visible = true;
					cellIsSubmitted.Visible = false;
				}

				// Initialize the DataPager control
				var oDtPgCount = lstvwVouchers.FindControl("DtPgCount") as DataPager;
				ControlUtility.FillListViewPagerFooter(lstvwVouchers, oDtPgCount);

				ImageButton  imgbtnEdit;
				bool bCondition = moFinancialYear.IsClosed && !moUserPermissions.CanEditOldFinancialYear;
				foreach (var item in lstvwVouchers.Items)
				{
					imgbtnEdit = item.FindControl("imgbtnEdit") as ImageButton;
					if (moUserPermissions.CanDeleteVoucher)
					{
						if (bCondition)
						{
							imgbtnEdit.Attributes["onclick"] = String.Format("alert('{0}'); return false;", S_FINANCIAL_YEAR_CLOSE_MSG);
						}
					}
					else
					{
						if (bCondition)
							imgbtnEdit.Attributes["onclick"] = String.Format("alert('{0}'); return false;", S_FINANCIAL_YEAR_CLOSE_MSG);
					
					}
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Handles any commands fired from the ListView, such as delete & sort.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwVouchers_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			OpenVoucherServiceObj();
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var item = e.Item as ListViewDataItem;
				var oVoucherDetails = new Voucher
					{
						SchoolId		= miSchoolId,
						FinancialYearId = miFinancialYearId,
						VoucherId		= lstvwVouchers.DataKeys[item.DisplayIndex]["VoucherId"].ToInt(),
						UpdatedById		= miUserId,
						Status			= Constants.RequisitionStatus.Pending // This is initialized for the sake of serialization only.
					};
				
				switch (e.CommandName)
				{
					case S_DELETE_ROW:
						{
							if (moAccountVoucherClient.DeleteVoucher(oVoucherDetails))
							{
								SetMessage(S_UPDATE_MSG, false);
								ReBindVoucherList();
							}
							else
								SetMessage(S_UPDATE_ERROR_MSG, true);
						}
						break;
					case S_SUBMIT_ROW:
						{
							if (moAccountVoucherClient.SubmitVoucherForApproval(oVoucherDetails))
							{
								SetMessage(S_APPROVAL_MSG, false);
								ReBindVoucherList();
							}
							else
								SetMessage(S_APPROVAL_ERROR_MSG, true);
						}
						break;
				}
			}
			// This case is to handle a sort command. We have set a custom sort command - 'SORT_ROW' so we can handle sorting ourselves.
			// In such a scenario, the ItemType property is actually EmptyItem, hence we cannot handle this in the previous block.
			else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
			{
				if (hidSortExpression.Value != e.CommandArgument.ToString())
					hidSortDirection.Value = Constants.S_DESCENDING;
				SetSortVariables();
				hidSortExpression.Value = e.CommandArgument.ToString();
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

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This function is used to get info from the Session and save it in member variables for later use.
	/// </summary>
	private void SetMemberVariables()
	{
		InitializeMemberVariables();

		if (Session[Constants.S_SESSION_FINANCIAL_YEAR] != null)
			moFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;

		moUserPermissions = moAccountsBaseClient.GetUserPermissions(miSchoolId, miUserId);

        hidUserAccess.Value = Constants.S_ZERO;
        if (moUserPermissions.CanSelfApprove || moUserPermissions.IsApprovalConfigured)
            hidUserAccess.Value = Constants.S_ONE;
	}

	/// <summary>
	/// This function is used to read the query string passed to the page and set some member variables/hidden fields accordingly.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.Count <= 0)
			return;
		
		if (!QueryString["StatusId"].IsNullOrEmpty())
			hidStatusId.Value = QueryString["StatusId"];
	}

	/// <summary>
	/// Processes UserPermissions and performs actions based on various permissions.
	/// </summary>
	private void ProcessPermissions()
	{
		if (!moUserPermissions.CanCreateVoucher && !moUserPermissions.CanApproveVoucher)
			lstvwVouchers.DataSourceID = null;
        
            btnAdd.Attributes.Add("onclick", "if(!CheckConfiguration()) {return false;}");
				
		if (moUserPermissions.CanCreateVoucher)
		{
			//btnAdd.Attributes["onclick"] = "window.open('VoucherPopUp.aspx', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600'); return false;";
			SetLedgerAccessMessage();
		}
		else
			btnAdd.Visible = false;

	}

	/// <summary>
	/// This function is used to populate the Status DropDown List.
	/// </summary>
	private void FillStatusDropDownList()
	{
		List<VoucherStatus> lstVoucherStatus = moAccountVoucherClient.GetVoucherStatus(miSchoolId,miUserId);
		ListSource.FillDropDownList(lstVoucherStatus, ddlStatus, S_DB_COLUMN_STATUS_NAME, S_DB_COLUMN_STATUS_ID, String.Empty);
		if (lstVoucherStatus.Count > 0 && !String.IsNullOrEmpty(hidStatusId.Value))
			ddlStatus.SelectedValue = hidStatusId.Value;
	}

	private void DisplayVouchers()
	{
		if (ddlStatus.Items.Count > 0)
			ReBindVoucherList();
		else
		{
			divAccessMessage.Visible = true;
			mainUpdatePanel.Visible = false;
			btnAdd.Visible = false;
			tblLedgerAccessNotice.Visible = false;
		}
	}

	/// <summary>
	/// Initializes default values for certain controls on the page.
	/// </summary>
	private void Initialize()
	{
		ApplyMouseHoverEffect(new List<Button> { btnAdd });

		// Sort defaults
		SetDefaultSortVariables();
	}

	/// <summary>
	/// This function sets the hiddenfield values that are maintained to remember sort direction.
	/// </summary>
	private void SetSortVariables()
	{
		hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

	/// <summary>
	///		Sets the default values for hidden variables which maintain sorting.
	/// </summary>
	private void SetDefaultSortVariables()
	{
		hidSortExpression.Value = !ddlStatus.SelectedValue.IsNullOrEmpty() && ddlStatus.SelectedValue.ToInt() == Constants.RequisitionStatus.Waiting_For_My_Approval.ToInt() ? S_WAITINGFORAPPROVAL_SORT_EXP : S_SORT_EXP;
		hidSortDirection.Value = Constants.S_DESCENDING;
	}

	/// <summary>
	/// This function is used to add a sort image to the ListView.
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
	/// Re binds the Voucher ListView to its Datasource.
	/// </summary>
	private void ReBindVoucherList()
	{
		lstvwVouchers.Items.Clear();
		lstvwVouchers.DataSourceID = objdsVouchers.ID;
		lstvwVouchers.DataBind();
	}

	/// <summary>
	/// Sets a message to be displayed to the user.
	/// </summary>
	/// <param name="asMessage">A string representing the Message content.</param>
	/// <param name="abIsError">A bool indicating if the message is an error message.</param>
	private void SetMessage(string asMessage, bool abIsError)
	{
		lblMessage.Text = asMessage;
		lblMessage.Visible = true;
		if (abIsError)
		{
			lblMessage.ForeColor = Color.Red;
		}
		else
		{
			lblMessage.ForeColor = Color.Blue;
			lblMessage.Font.Bold = true;
		}
	}

	/// <summary>
	/// Initializes the Voucher service object.
	/// </summary>
	private void OpenVoucherServiceObj()
	{
		moAccountVoucherClient = new AccountVoucherClient();
		moAccountVoucherClient.Open();
	}

	/// <summary>
	/// Disposes off the Voucher service object.
	/// </summary>
	private void CloseVoucherServiceObj()
	{
		if (moAccountVoucherClient != null && moAccountVoucherClient.State != CommunicationState.Faulted)
			moAccountVoucherClient.Close();
	}

	/// <summary>
	/// Initializes the Accounts base service object.
	/// </summary>
	private void OpenBaseServiceObj()
	{
		moAccountsBaseClient = new AccountsBaseClient();
		moAccountsBaseClient.Open();
	}

	/// <summary>
	/// Disposes off the Accounts base service object.
	/// </summary>
	private void CloseBaseServiceObj()
	{
		if (moAccountsBaseClient != null && moAccountsBaseClient.State != CommunicationState.Faulted)
			moAccountsBaseClient.Close();
	}

	/// <summary>
	/// Sets the Ledger access message according to the logged in user.
	/// </summary>
	private void SetLedgerAccessMessage()
	{
		bool bHasAccess = !(moUserRole != Constants.UserRoles.Admin && !CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Ledgers));
		tblLedgerAccessNotice.Visible = !bHasAccess;
		trLedgerLink.Visible = bHasAccess;
	}

	#endregion -- PRIVATE METHOD(s) --

}