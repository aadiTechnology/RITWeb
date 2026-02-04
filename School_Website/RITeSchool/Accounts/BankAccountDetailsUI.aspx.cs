/* ----------------------------------------------------------------------------------
 *	FileName	: BankAccountDetailsUI.cs
 *	Author		: Rohini V. Ghule
 *	Date		: 5-Oct-2011
 *	Description : This class is used to add ,edit and remove the bank accounts.
 * ----------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SchoolEntities;
using Utility;

public partial class BankAccountDetailsUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_SAVE_MESSAGE = "Bank account saved successfully !!!";
	private const string S_UPDATE_MESSAGE = "Bank account updated successfully!!!";
	private const string S_DELETE_MESSAGE = "Bank account deleted successfully!!!";
	private const string S_DEFAULT_SORT_EXP = "BankName";
	private const string S_DEBIT = "1";
	private const string S_CREDIT = "2";
	private const string S_UPDATE = "Update";
	private const string S_SAVE = "Save";
	private const string S_BANK_NAME = "%BANKNAME%";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

    private BankAccountClient moBankAccountClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT(s) --

	/// <summary>
	/// This event is used to initialise all controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				OpenServiceObj();
				FillBankCombo();
				InitalizeFields();
				FillBankDetailsListview();
				CloseServiceObject();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
	/// This event is used to save bank account details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			OpenServiceObj();
			if (Save())
			{
				FillBankDetailsListview();
				ClearFields();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObject();
		}
	}

	/// <summary>
	/// This event is used to set a different css class for alternating rows.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBankDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				var oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
				var oImgDeleteBtn = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;                
                var hidIsDefault = oCurrentItem.FindControl("hidIsDefault") as HiddenField;
                var hidIsInternalDefault = oCurrentItem.FindControl("hidIsInternalDefault") as HiddenField;

				bool bIsForOnlineTransactions = lstvwBankDetails.DataKeys[oCurrentItem.DisplayIndex]["IsForOnlineTransactions"].ToBool();                
                
				// Set a diff class for alternate rows
				if (oCurrentItem.DisplayIndex % 2 == 1)
					oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
				
				if (oImgDeleteBtn != null)
					oImgDeleteBtn.Attributes["onclick"] = "if(!ConfirmRemove()){return false;}";

				if (bIsForOnlineTransactions)
				{
					oHTMLCurrentRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightBlue");                    
					
					if (oImgDeleteBtn != null)
					{
						oImgDeleteBtn.Attributes.Remove("onclick");
						oImgDeleteBtn.Attributes["onclick"] = "WarnIsForOnlineBank(); return false;";
					}
				}
                if (hidIsDefault.Value.ToBool() && hidIsInternalDefault.Value.ToBool())
                {
                    var lblSrNo = oCurrentItem.FindControl("lblSrNo") as Label;
                    var lblBankName = oCurrentItem.FindControl("lblBankName") as Label;
                    var lblAlias = oCurrentItem.FindControl("lblAlias") as Label;
                    var lblAcNo = oCurrentItem.FindControl("lblAcNo") as Label;
                    var lblOpeningBalance = oCurrentItem.FindControl("lblOpeningBalance") as Label;

                    lblSrNo.ForeColor = System.Drawing.Color.Olive;
                    lblSrNo.Style.Add("font-weight", "bold");
                    lblBankName.ForeColor = System.Drawing.Color.Olive;
                    lblBankName.Style.Add("font-weight", "bold");
                    lblAlias.ForeColor = System.Drawing.Color.Olive;
                    lblAlias.Style.Add("font-weight", "bold");
                    lblAcNo.ForeColor = System.Drawing.Color.Olive;
                    lblAcNo.Style.Add("font-weight", "bold");
                    lblOpeningBalance.ForeColor = System.Drawing.Color.Olive;
                    lblOpeningBalance.Style.Add("font-weight", "bold");
                }
                else if (hidIsDefault.Value.ToBool())
                {
                    var lblSrNo = oCurrentItem.FindControl("lblSrNo") as Label;
                    var lblBankName = oCurrentItem.FindControl("lblBankName") as Label;
                    var lblAlias = oCurrentItem.FindControl("lblAlias") as Label;
                    var lblAcNo = oCurrentItem.FindControl("lblAcNo") as Label;
                    var lblOpeningBalance = oCurrentItem.FindControl("lblOpeningBalance") as Label;

                    lblSrNo.ForeColor = System.Drawing.Color.Maroon;
                    lblSrNo.Style.Add("font-weight", "bold");
                    lblBankName.ForeColor = System.Drawing.Color.Maroon;
                    lblBankName.Style.Add("font-weight", "bold");
                    lblAlias.ForeColor = System.Drawing.Color.Maroon;
                    lblAlias.Style.Add("font-weight", "bold");
                    lblAcNo.ForeColor = System.Drawing.Color.Maroon;
                    lblAcNo.Style.Add("font-weight", "bold");
                    lblOpeningBalance.ForeColor = System.Drawing.Color.Maroon;
                    lblOpeningBalance.Style.Add("font-weight", "bold");
                }
                else if (hidIsInternalDefault.Value.ToBool())
                {
                    var lblSrNo = oCurrentItem.FindControl("lblSrNo") as Label;
                    var lblBankName = oCurrentItem.FindControl("lblBankName") as Label;
                    var lblAlias = oCurrentItem.FindControl("lblAlias") as Label;
                    var lblAcNo = oCurrentItem.FindControl("lblAcNo") as Label;
                    var lblOpeningBalance = oCurrentItem.FindControl("lblOpeningBalance") as Label;

                    lblSrNo.ForeColor = System.Drawing.Color.Navy;
                    lblSrNo.Style.Add("font-weight", "bold");
                    lblBankName.ForeColor = System.Drawing.Color.Navy;
                    lblBankName.Style.Add("font-weight", "bold");
                    lblAlias.ForeColor = System.Drawing.Color.Navy;
                    lblAlias.Style.Add("font-weight", "bold");
                    lblAcNo.ForeColor = System.Drawing.Color.Navy;
                    lblAcNo.Style.Add("font-weight", "bold");
                    lblOpeningBalance.ForeColor = System.Drawing.Color.Navy;
                    lblOpeningBalance.Style.Add("font-weight", "bold");
                }
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update and remove the bank details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBankDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		string sBankName = string.Empty;
		try
		{
			OpenServiceObj();
			ClearFields();
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowIndex = oCurrentItem.DisplayIndex;

				switch (e.CommandName)
				{
				    case Constants.S_COMMAND_UPDATE:
				        Update(oCurrentItem);
				        break;
				    case Constants.S_COMMAND_REMOVE:
						sBankName = (oCurrentItem.FindControl("lblAlias") as Label).Text;
				        if (sBankName.IsNullOrEmpty())
							sBankName = (oCurrentItem.FindControl("lblBankName") as Label).Text;
				        Delete(iRowIndex);
				        FillBankDetailsListview();
				        ClearFields();
				        break;
				}
			}
			if (e.CommandName == Constants.S_COMMAND_SORT)
				hidSortExpression.Value = e.CommandArgument.ToString();
		}
		catch (FaultException<SchoolBusinessService.DependencyException> ex)
		{
			lblError.Text = ex.Detail.ErrorMessage.Replace(S_BANK_NAME, sBankName);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseServiceObject();
		}
	}

	/// <summary>
	/// This event is used for sorting.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBankDetails_Sorting(object sender, ListViewSortEventArgs e)
	{
		try
		{
			OpenServiceObj();
			hidSortExpression.Value = (e.SortExpression != string.Empty) ? e.SortExpression : S_DEFAULT_SORT_EXP;
			SetSortVariables();
			FillBankDetailsListview();
			CloseServiceObject();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion  -- EVENT(s) --

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
			// Do not remove the following comment.
			//cultureInfo.NumberFormat.CurrencySymbol = "\x20B9"; // This sets the currency symbol as the new rupee symbol, but this is browser dependent (requires unicode 6). It doesn't work in IE.
			return String.Format("{0} {1}", adOpeningBal.ToDecimal().ToString("C2", cultureInfo), abIsDebit.ToBool() ? "Dr" : "Cr");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}

		return String.Empty;
	}

	#endregion -- PROTECTED METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to set javaScript attribute.
	/// </summary>
	private void InitalizeFields()
	{
		cmbBankName.Focus();
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		hidMode.Value = Constants.S_NEW_MODE;
		hidSortDirection.Value = Constants.S_ASCENDING;
		hidRowNo.Value = Constants.I_ZERO.ToString();
		btnBack.PostBackUrl = CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Accounts_Related).ToString();
		btnCancel.Attributes.Add("onclick", "if(ClearControls()) {return false;} ");
        btnSave.Attributes.Add("onclick", "if(!Confirm()) {return false;} ");
		ApplyMouseHoverEffect(new List<Button> { btnBack, btnCancel, btnSave });
	}

	/// <summary>
	/// This method is used to fill bank combobox.
	/// </summary>
	private void FillBankCombo()
	{
		List<Bank> lstBankNames = moBankAccountClient.GetAllBanks(miSchoolId);
		ListSource.FillDropDownList(lstBankNames, cmbBankName, "Name", "Id", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to add sort image.
	/// </summary>
	private void AddSortImage()
	{
		if (string.IsNullOrEmpty(hidSortExpression.Value))
			hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		var oHtmlTableHeaderRow = lstvwBankDetails.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
	}

	/// <summary>
	/// This method is used to set sort direction.
	/// </summary>
	private void SetSortVariables()
	{
	    hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

    /// <summary>
	/// This method is used to clear the controls.
	/// </summary>
	private void ClearFields()
	{
		hidLedgerId.Value = string.Empty;
		hidMode.Value = Constants.S_NEW_MODE;
		cmbBankName.ClearSelection();
		txtAlias.Text = string.Empty;
		txtAcNo.Text = string.Empty;
		txtAmt.Text = string.Empty;
		txtBankAddress.Text = string.Empty;
		chkOnlineTransactions.Checked = false;
		cmbBankName.Enabled = true;
		cmbDebit.ClearSelection();
        chkIsDefault.Checked = false;
        chkIsInternalDefault.Checked = false;
		if (btnSave.Text == S_UPDATE)
			btnSave.Text = S_SAVE;
	}

	/// <summary>
	/// This method is used to save bank details.
	/// </summary>
	private bool Save()
	{
		lblUpdateSucess.Text = string.Empty;
		BankAccount oBankAccount = PopulateBankDetails();
		bool bResult = false;
		string sMessage = moBankAccountClient.SaveBankAccountDetails(oBankAccount);
		if (String.IsNullOrEmpty(sMessage))
		{
			if (!IsConfigured())
				SaveConfigDetails(Constants.SchoolConfigurations.BankAccounts.ToInt());
			hidMode.Value = Constants.S_NEW_MODE;
			lblUpdateSucess.Text = btnSave.Text == S_UPDATE ? S_UPDATE_MESSAGE : S_SAVE_MESSAGE;

			bResult = true;
		}
		else
			lblError.Text = sMessage;
		return bResult;
	}

	/// <summary>
	/// This method is used to populate bank details.
	/// </summary>
	/// <returns></returns>
	private BankAccount PopulateBankDetails()
	{
		return new BankAccount
				{
					Id						= hidMode.Value == Constants.S_EDIT_MODE ? hidLedgerId.Value.ToInt() : 0,
					Name					= txtAlias.Text.Trim().IsNullOrEmpty() ? cmbBankName.SelectedItem.Text : txtAlias.Text.Trim(),
					OpeningBalance			= txtAmt.Text.Trim().IsNullOrEmpty() ? 0 : txtAmt.Text.Trim().ToDecimal(),
					IsDebit					= cmbDebit.SelectedValue == S_DEBIT && !txtAmt.Text.Trim().IsNullOrEmpty() && txtAmt.Text.Trim().ToDecimal() > 0,
					Bank					= new Bank
					       						{
					       							Id	 = cmbBankName.SelectedValue.ToInt(),
													Name = cmbBankName.SelectedItem.Text
					       						},
					Alias					= txtAlias.Text.Trim(),
					AccountNumber			= txtAcNo.Text.Trim(),
					Address					= txtBankAddress.Text.Trim(),
					IsForOnlineTransactions = lstvwBankDetails.Items.Count == 0 || chkOnlineTransactions.Checked,
					SchoolId				= miSchoolId,
					FinancialYearId			= miFinancialYearId,
                    IsDefault = chkIsDefault.Checked,
                    IsInternalDefault = chkIsInternalDefault.Checked
				};
	}

	/// <summary>
	/// This method is used to fill list view.
	/// </summary>
	private void FillBankDetailsListview()
	{
		List<BankAccount> lstBankDetails = moBankAccountClient.GetAllConfiguredBankDetails(hidSortExpression.Value, hidSortDirection.Value, miSchoolId, miFinancialYearId);
		lstvwBankDetails.DataSource = lstBankDetails;
		lstvwBankDetails.DataBind();
	}

	/// <summary>
	/// This method is used to set values to control for update.
	/// </summary>
	private void Update(ListViewDataItem aoCurrentItem)
	{
		hidMode.Value = Constants.S_EDIT_MODE;
		btnSave.Text = S_UPDATE;
		
		hidLedgerId.Value = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["Id"].ToString();

		var lblAcNo = aoCurrentItem.FindControl("lblAcNo") as Label;
		txtAcNo.Text = lblAcNo.Text;

		var oBank = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["Bank"] as Bank;
		cmbBankName.SelectedValue = oBank.Id.ToString();
		
		var lblAlias = aoCurrentItem.FindControl("lblAlias") as Label;
		txtAlias.Text = lblAlias.Text;
		
		txtBankAddress.Text = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["Address"].ToString();
		
		txtAmt.Text = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["OpeningBalance"].ToString();
		cmbDebit.SelectedValue = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["IsDebit"].ToBool() ? S_DEBIT : S_CREDIT;
		
		var oOriginalLedger = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["OriginalLedger"] as Ledger;
		cmbBankName.Enabled = oOriginalLedger.Id != 0;
		
		chkOnlineTransactions.Checked = lstvwBankDetails.DataKeys[aoCurrentItem.DisplayIndex]["IsForOnlineTransactions"].ToBool();
        var hidDefaultBank = aoCurrentItem.FindControl("hidIsDefault") as HiddenField;
        var hidInternalDefaultBank = aoCurrentItem.FindControl("hidIsInternalDefault") as HiddenField;
        if (hidDefaultBank != null)
            chkIsDefault.Checked = hidDefaultBank.Value.ToBool();
        if (hidInternalDefaultBank != null)
            chkIsInternalDefault.Checked = hidInternalDefaultBank.Value.ToBool();

		if (chkOnlineTransactions.Checked)
			chkOnlineTransactions.InputAttributes["onclick"] = "WarnOnOnlineTransactionCheck(); this.checked = true; return false;";
	}

    /// <summary>
    /// This method is used to delete bank account details.
    /// </summary>
    /// <param name="aiRowIndex" />
    private void Delete(int aiRowIndex)
	{
		var iLedger = lstvwBankDetails.DataKeys[aiRowIndex]["Id"].ToInt();
		string sErrorMessage = moBankAccountClient.DeleteBankAccountDetails(iLedger, miUserId, miSchoolId, miFinancialYearId);
		if (string.IsNullOrEmpty(sErrorMessage))
		{
			if (!moBankAccountClient.IsAtleastOneBankExist(miSchoolId, miFinancialYearId))
				DeleteConfigDetails(Constants.SchoolConfigurations.BankAccounts.ToInt());
			lblError.Text = string.Empty;
			lblUpdateSucess.Text = S_DELETE_MESSAGE;
		}
		else
			throw new FaultException<SchoolBusinessService.DependencyException>(new SchoolBusinessService.DependencyException { ErrorMessage = sErrorMessage });
	}

	/// <summary>
	/// This method is used to decrypt query string.
	/// </summary>
	/// <returns></returns>
	private bool IsConfigured()
	{
		return !QueryString[Constants.S_IS_CONFIGURED].IsNull() && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
	}

	/// <summary>
	/// This method is used to initialize service object.
	/// </summary>
	private void OpenServiceObj()
	{
        moBankAccountClient = new BankAccountClient();
		moBankAccountClient.Open();
	}

	/// <summary>
	/// Disposes off the Bank client service object.
	/// </summary>
	private void CloseServiceObject()
	{
		if (moBankAccountClient != null && moBankAccountClient.State != CommunicationState.Faulted)
			moBankAccountClient.Close();
	}

	#endregion -- PRIVATE METHOD(s) --
}