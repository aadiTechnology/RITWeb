/* -----------------------------------------------------------------------------------
 *	FileName	: LedgerMasterUI.aspx.cs
 *	Author		: Vishal B. Shah
 *	Date		: 4-Oct-2011
 *	Description	: This is the code behind file for the Ledgers screen,
 *				  which is used to create and manage ledgers for the accounts module.
 * -----------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class LedgerMasterUI : SchoolBase
{

	#region -- MEMBER(s) --

	// Message strings
	private const string S_INSERT_MESSAGE = "Ledger saved successfully!!!";
	private const string S_UPDATE_MESSAGE = "Ledger updated successfully!!!";
	private const string S_DELETE_MESSAGE = "Ledger deleted successfully!!!";
	private const string S_INSERT_ERROR_MESSAGE = "Failed to save Ledger.";
	private const string S_UPDATE_ERROR_MESSAGE = "Failed to update Ledger.";
	private const string S_DELETE_ERROR_MESSAGE = "Failed to delete Ledger.";
    private const string S_UPLOAD_FILE_PATH_FOR_PAN = "\\DOWNLOADS\\PAN Attachment\\";
    private const string S_FILE_SIZE_EXCEED_ERROR = "File size should not be greater than 1 MB.";
    private const string S_FILE_ALREADY_EXISTS = "The given file is already exixts.";
    private const string S_BUTTON_TEXT_UPDATE = "Update";
    private const string S_BUTTON_TEXT_SAVE = "Save";
    private const string S_VIEWSTATE_NAME = "Groups";
    private const int I_FILE_SIZE_LIMIT = 1048576;  // File limit is 1 MB
    
	// ListView Commands
	private const string S_EDIT_ROW = "EDIT_ROW";
	private const string S_DELETE_ROW = "DELETE_ROW";
	private const string S_SORT_ROW = "SORT_ROW";
	private const string S_LEDGER_NAME = "%LEDGERNAME%";

	// Default Sort Expression for the Ledger Table
	private const string S_DEFAULT_SORT_EXP = "LedgerMaster.Name";

	// DataKey string constants
	private const string S_ISDEBIT = "IsDebit";

	// For ObjectDatasource
	private AccountLedgerClient moAccountLedgerClient;
	private AccountGroupClient moAccountGroupClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT(s) --

	/// <summary>
	/// This event is handled to perform default taks the first time the page is loaded.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				InitializeGroupServiceObj();
				FillGroupDropDownList();
				Initialize();
			}
            if (hidLedgerId.Value.IsNullOrEmpty() || hidLedgerId.Value == "0")
                btnSave.Text = S_BUTTON_TEXT_SAVE;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseGroupServiceObj();
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
	/// This event is used to update the ListView pager controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwLedgerDetails);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to Save new ledger details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			InitializeServiceObj();
            bool bIsUpdate = btnSave.Text == S_BUTTON_TEXT_UPDATE;

			Ledger oLedger = LoadLedgerDetails();

			string sMessage;

            if (bIsUpdate)
            {
                sMessage = moAccountLedgerClient.UpdateLedger(oLedger);
                if (String.IsNullOrEmpty(sMessage))
                {
                    ShowUpdateMessage(S_UPDATE_MESSAGE);
                    ReBindLedgerList();
                    ResetControls();
                    if (!IsConfigured())
                        SaveConfigDetails(Constants.SchoolConfigurations.Ledgers.ToInt());
                }
                else
                    ShowErrorMessage(sMessage);
            }
            else
            {
                sMessage = moAccountLedgerClient.SaveLedger(oLedger);
                if (String.IsNullOrEmpty(sMessage))
                {
                    ShowUpdateMessage(S_INSERT_MESSAGE);
                    ReBindLedgerList();
                    ResetControls();
                    if (!IsConfigured())
                        SaveConfigDetails(Constants.SchoolConfigurations.Ledgers.ToInt());
                }
                else
                    ShowErrorMessage(sMessage);
            }
		}
        catch (ApplicationException ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
		catch (Exception ex)
		{
            ShowErrorMessage(btnSave.Text == S_BUTTON_TEXT_UPDATE ? S_UPDATE_ERROR_MESSAGE : S_INSERT_ERROR_MESSAGE);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObj();
		}
	}

	/// <summary>
	/// This event is used to set a diff class for alternating rows in the ListView and
	/// hide action buttons for System defined items in the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwLedgerDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;

				// Set a diff class for alternate rows
				if (oCurrentItem.DisplayIndex % 2 == 1)
				{
					var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
					oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
				}

				// If the current item is a System Defined item or its GroupId == miBankAccountGroupId, remove the edit and delete buttons.
				bool bIsSystemDefined = lstvwLedgerDetails.DataKeys[oCurrentItem.DisplayIndex]["IsSystemDefined"].ToBool();
				var oGroup = lstvwLedgerDetails.DataKeys[oCurrentItem.DisplayIndex]["Group"] as Group;
                var sFilePath = lstvwLedgerDetails.DataKeys[oCurrentItem.DisplayIndex]["FilePath"] as string;
			    var btnDownload = oCurrentItem.FindControl("btnDownload") as ImageButton;
				if (bIsSystemDefined || oGroup.Id == Constants.AccountsGroups.BankAccounts.ToInt())
				{
					var imgbtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
					if (oGroup.OriginalGroup.Id == Constants.AccountsGroups.BankAccounts.ToInt())
					{
						imgbtnEdit.Visible = false;
						imgbtnEdit.Style["visibility"] = "hidden";
						imgbtnEdit.CommandArgument = String.Empty;
					}
					var imgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
					//imgbtnDelete.Visible = false;
					imgbtnDelete.Style["visibility"] = "hidden";
					imgbtnDelete.CommandArgument = String.Empty;
				}
                
                if(sFilePath.IsNullOrEmpty())
                {
                    if(!btnDownload.IsNull())
                        btnDownload.Visible = false;
                }
                else
                {
                    string sDestination = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN + sFilePath;
                    if (File.Exists(sDestination))
                        btnDownload.Attributes.Add("onclick", "window.open('..//downloads//PAN Attachment//" + sFilePath + "','_blank'); return false;");
                }
			}
			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill the Pager controls in the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwLedgerDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwLedgerDetails.Items.Count > 0)
			{
				// Initialize the DataPager control
				var oDtPgCount = lstvwLedgerDetails.FindControl("DtPgCount") as DataPager;
				ControlUtility.FillListViewPagerFooter(lstvwLedgerDetails, oDtPgCount);
			}
			else
				DeleteConfigDetails(Constants.SchoolConfigurations.Ledgers.ToInt());
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to handle the commands generated by the ListView.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwLedgerDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		string sErrorMessage = string.Empty;
		try
		{
			InitializeServiceObj();
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iLedgerId = lstvwLedgerDetails.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToInt();
				switch (e.CommandName)
				{
					case S_EDIT_ROW:
						sErrorMessage = S_UPDATE_ERROR_MESSAGE;
						PopulateLedgerDetails(oCurrentItem);
                        btnSave.Text = S_BUTTON_TEXT_UPDATE;
						break;
					case S_DELETE_ROW:
						sErrorMessage = S_DELETE_ERROR_MESSAGE;
						string sMessage = moAccountLedgerClient.DeleteLedger(miSchoolId, iLedgerId, miUserId);
						string sLedgerName = (oCurrentItem.FindControl("lblName") as Label).Text;
                        string sFilePath = lstvwLedgerDetails.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
                        if(!sFilePath.IsNullOrEmpty())
                        {
                            string sFileToDelete = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN + sFilePath;
                            if (File.Exists(sFileToDelete))
                                File.Delete(sFileToDelete);    
                        }
                        
						if (String.IsNullOrEmpty(sMessage))
						{
							ShowUpdateMessage(S_DELETE_MESSAGE);
							ReBindLedgerList();
							ResetControls();
						}
						else
							ShowErrorMessage(sMessage.Replace(S_LEDGER_NAME, sLedgerName));
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
				ReBindLedgerList();

				var oDtPgDropDown = lstvwLedgerDetails.FindControl("DtPgDropDown") as DataPager;
				if (oDtPgDropDown != null)
					oDtPgDropDown.SetPageProperties(0, oDtPgDropDown.PageSize, true);
			}
		}
		catch (Exception ex)
		{
			ShowErrorMessage(sErrorMessage);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObj();
		}
	}

	#endregion -- EVENT(s) --

	#region -- PROTECTED METHOD(s) --

	/// <summary>
	/// This function is used to return a string representing the double value in Indian currency format.
	/// It is called from the expression binding syntax in the page markup.
	/// </summary>
	/// <param name="adOpeningBal"></param>
	/// <param name="abIsDebit"></param>
	/// <returns></returns>
	protected string GetOpeningBalText(object adOpeningBal, object abIsDebit)
	{
		try
		{
			var cultureInfo = new CultureInfo("hi-IN")
						{
							NumberFormat = { CurrencySymbol = String.Empty }
						};
			// Do not remove following line.
			//cultureInfo.NumberFormat.CurrencySymbol = "\x20B9"; // This sets the currency symbol as the new rupee symbol, but this is browser dependent (requires unicode 6). It doesn't work in IE.
			return String.Format("{0} {1}", adOpeningBal.ToDecimal().ToString("C2", cultureInfo), abIsDebit.ToBool() ? "DR" : "CR");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}

		return String.Empty;
	}

    protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ViewState[S_VIEWSTATE_NAME].IsNull())
            {
                List<Group> lstGroup = ViewState[S_VIEWSTATE_NAME] as List<Group>;
                var bIsPanRequired = lstGroup.Where(a => a.Id == ddlGroupList.SelectedItem.Value.ToInt()).Select(a => a.IsPANDetailsRequired).FirstOrDefault();
                SetVisibilityForPanCard(bIsPanRequired);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- PROTECTED METHOD(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This is a common method to set the appropriate controls depends on the condition.
    /// </summary>
    /// <param name="abIsPanRequired"></param>
    private void SetVisibilityForPanCard(bool abIsPanRequired)
    {
        trPan.Visible = abIsPanRequired;
        trUpload.Visible = abIsPanRequired;
        trUploadNote.Visible = abIsPanRequired;
        if (!abIsPanRequired)
        {
            txtPanNo.Text = string.Empty;
            hidFilePath.Value = string.Empty;
        }
        if (hidLedgerId.Value.IsNullOrEmpty() || hidLedgerId.Value == "0")
            btnSave.Text = S_BUTTON_TEXT_SAVE;
    }

    /// <summary>
    /// this is for SaveFileOnServer
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName)
    {
        // Upload the file to the server.
        string sErrMessage = string.Empty;
        string sFolderName = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        bool bIsUpdate = btnSave.Text == S_BUTTON_TEXT_UPDATE;

        if (UploadFile.HasFile)
        {
            if (UploadFile.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                sErrMessage = S_FILE_SIZE_EXCEED_ERROR;
            else if (File.Exists(sServerFilePath))
                sErrMessage = S_FILE_ALREADY_EXISTS;
            else
            {
                sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
                sServerFilePath = sFolderName + sFileName;
                UploadFile.SaveAs(sServerFilePath);
            }
        }
        else if(!hidFilePath.Value.IsNullOrEmpty() && bIsUpdate)
            sFileName = hidFilePath.Value;
        if (sErrMessage.Equals("") && !hidFilePath.Value.IsNullOrEmpty() && bIsUpdate && UploadFile.HasFile)
        {
            //delete exesting file
            string sFileToDelete = Server.MapPath("..") + S_UPLOAD_FILE_PATH_FOR_PAN + hidFilePath.Value;
            if (File.Exists(sFileToDelete))
                File.Delete(sFileToDelete);
            lblErrorMessage.Text = sErrMessage;
        }

        if (sErrMessage!=string.Empty)
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }

        return sFileName;
    }

	/// <summary>
	/// This function is used to fill the Groups Dropdown list.
	/// </summary>
	private void FillGroupDropDownList()
	{
		List<Group> lstGroups = moAccountGroupClient.GetGroupsForLedgers(miSchoolId, miFinancialYearId);
		ListSource.FillDropDownList(lstGroups, ddlGroupList, "Name", "Id", Constants.S_SELECT);
        ViewState[S_VIEWSTATE_NAME] = lstGroups;
	}

	/// <summary>
	/// This function is used to set defaults for controls on page load.
	/// </summary>
	private void Initialize()
	{
		ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
		btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Accounts_Related));
		valsumErrorMessages.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		txtLedgerName.Focus();

		if (String.IsNullOrEmpty(hidSortExpression.Value))
			hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		if (String.IsNullOrEmpty(hidSortDirection.Value))
			hidSortDirection.Value = Constants.S_ASCENDING;
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
		var oHtmlTableHeaderRow = lstvwLedgerDetails.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, sSortExpression, sSortDirection);
	}

	/// <summary>
	/// This function is used to set the Update message on the screen and hide the error message.
	/// </summary>
	/// <param name="asMessage"></param>
	private void ShowUpdateMessage(string asMessage)
	{
		lblUpateMessage.Text = asMessage;
		lblUpateMessage.Visible = true;
		lblErrorMessage.Visible = false;
	}

	/// <summary>
	/// This function is used to set the error message on the screen and hide the update message.
	/// </summary>
	/// <param name="asMessage"></param>
	private void ShowErrorMessage(string asMessage)
	{
		lblErrorMessage.Text = asMessage;
		lblErrorMessage.Visible = true;
		lblUpateMessage.Visible = false;
	}

	/// <summary>
	/// This function is used to populate the input fields on page with values of the Ledger being edited.
	/// </summary>
	/// <param name="aoListItem">The ListView item which raised the edit command.</param>
	private void PopulateLedgerDetails(ListViewDataItem aoListItem)
	{
		hidLedgerId.Value = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["Id"].ToString();

		var lblLedgerName = aoListItem.FindControl("lblName") as Label;
		txtLedgerName.Text = lblLedgerName.Text;

		var oGroup = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["Group"] as Group;
		ddlGroupList.SelectedValue = oGroup.Id.ToString();

		var lblOpeningBal = aoListItem.FindControl("lblOpeningBal") as Label;
		txtOpeningBal.Text = lblOpeningBal.Text.Replace(" DR", String.Empty)
											   .Replace(" CR", String.Empty)
											   .Replace(",", String.Empty)
											   .Trim();

		var lblBudget = aoListItem.FindControl("lblBudget") as Label;
		txtBudget.Text = lblBudget.Text.Trim().Replace(",", string.Empty);

		bool bIsDebit = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex][S_ISDEBIT].ToBool();
		ddlDebitCredit.SelectedIndex = bIsDebit ? 1 : 0;

        hidFilePath.Value = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["FilePath"].ToString();
        string sPanNo = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["PanNo"].ToString();
        bool IsPanApplicable = lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["IsPanApplicable"].ToBool();
        if (!hidFilePath.Value.IsNullOrEmpty())
            txtPanNo.Text = sPanNo;
        SetVisibilityForPanCard(IsPanApplicable);    
        
		// Do not delete following code.
		// If the OriginalLedgerId > 0, it means that the ledger being populated is an old ledger, hence it's opening balance should not be editable.	
		//if (Convert.ToInt32(lstvwLedgerDetails.DataKeys[aoListItem.DisplayIndex]["OriginalId"]) <= 0) return;
		//txtOpeningBal.Enabled = false;
		//ddlDebitCredit.Enabled = false;
	}

	/// <summary>
	/// This function is used to load ledger details from the input fields on the screen.
	/// </summary>
	/// <returns>A LedgerMaster object representing the ledger details.</returns>
	private Ledger LoadLedgerDetails()
	{
        return new Ledger
				{
					Id				= btnSave.Text == S_BUTTON_TEXT_UPDATE ? hidLedgerId.Value.ToInt() : Constants.I_ZERO,
					Name			= txtLedgerName.Text.Trim(),
					Group			= new Group { Id = ddlGroupList.SelectedValue.ToInt() },
					OpeningBalance  = txtOpeningBal.Text.Trim().IsNullOrEmpty() ? Constants.I_ZERO : txtOpeningBal.Text.Trim().ToDecimal(),
					Budget			= txtBudget.Text.Trim().IsNullOrEmpty() ? Constants.I_ZERO : txtBudget.Text.Trim().ToDecimal(),
					IsDebit			= !txtOpeningBal.Text.Trim().IsNullOrEmpty() && ddlDebitCredit.SelectedValue.ToInt() == Constants.TransactionType.Debit.ToInt(),
					SchoolId		= miSchoolId,
					FinancialYearId = miFinancialYearId,
					InsertedById	= miUserId,
					UpdatedById		= miUserId,
                    PanNo           = txtPanNo.Text.Trim(),
                    FilePath        = SaveFileOnServer(UploadFile.FileName)
				};
	}

	/// <summary>
	/// This function rebinds the Ledger List to the ObjectDatasource.
	/// </summary>
	private void ReBindLedgerList()
	{
		// It's necessary to clear the items before binding because Items.Count property remains the same (old count)
		// if the new datasource that is being bound is null or returns 0 items.
		lstvwLedgerDetails.Items.Clear();
		lstvwLedgerDetails.DataSourceID = objdsLedgerList.ID;
	}

	/// <summary>
	/// This function is used to reset controls on the page.
	/// </summary>
	private void ResetControls()
	{
		// Input controls
		txtLedgerName.Text = String.Empty;
		ddlGroupList.SelectedValue = Constants.S_ZERO;
		txtOpeningBal.Text = String.Empty;
		ddlDebitCredit.SelectedIndex = 0;
		txtBudget.Text = string.Empty;
	    txtPanNo.Text = string.Empty;
	    trPan.Visible = false;
	    trUpload.Visible = false;
	    trUploadNote.Visible = false;
		// Reset visibility of buttons
        btnSave.Text = S_BUTTON_TEXT_SAVE;

		// Hidden fields
		hidLedgerId.Value = String.Empty;
	    hidFilePath.Value = string.Empty;
		// Set focus
		txtLedgerName.Focus();
	}

	/// <summary>
	///		Determines from the QueryString if the screen is configured.
	/// </summary>
	/// <returns>true if Configured, false otherwise.</returns>
	private bool IsConfigured()
	{
		return !QueryString[Constants.S_IS_CONFIGURED].IsNull() && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
	}

	/// <summary>
	/// Initializes the Ledger service object.
	/// </summary>
	private void InitializeServiceObj()
	{
		moAccountLedgerClient = new AccountLedgerClient();
		moAccountLedgerClient.Open();
	}

	/// <summary>
	/// Disposes off the Ledger service object.
	/// </summary>
	private void CloseServiceObj()
	{
		if (moAccountLedgerClient != null && moAccountLedgerClient.State != CommunicationState.Faulted)
			moAccountLedgerClient.Close();
	}

	/// <summary>
	/// Initializes the Group service object.
	/// </summary>
	private void InitializeGroupServiceObj()
	{
		moAccountGroupClient = new AccountGroupClient();
		moAccountGroupClient.Open();
	}

	/// <summary>
	/// Disposes off the Group service object.
	/// </summary>
	private void CloseGroupServiceObj()
	{
		if (moAccountGroupClient != null && moAccountGroupClient.State != CommunicationState.Faulted)
			moAccountGroupClient.Close();
	}

	#endregion -- PRIVATE METHOD(s) --
   
}