/* -------------------------------------------------------------------------------
 *	MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *	Author	: Vishal B. Shah
 *	Date	: 24-Jan-2012
 *	Purpose	: Modified to reflect payments in the Accounts Module.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using System.Globalization;
using CrystalDecisions.Shared;
using SchoolEntities.StudentFee;

public partial class CautionMoneyChequePopUp : SchoolBase
{
	#region -- CONSTANT(s) --

	private const char C_CHEQUE_MODE = 'Q';
	private const char C_CASH_MODE = 'C';
    private const char C_ELECTRONIC_MODE = 'E';
	private const string S_DATE_FORMATE_FEILD = "dd-MMM-yyyy";
	private const string S_RETURN_DATE_FEILD = "Return_Date";
	private const string S_PAYMENT_DATE_FEILD = "Payment_Date";
	private const string S_CHEQUE_DATE_FEILD = "Cheque_Date";
	private const string S_PAYMENT_MODE_FEILD = "Payment_Mode";
	private const string S_REMARK_FEILD = "Remarks";
	private const string S_CHEQUE_NO_FEILD = "Cheque_Number";
	private const string S_ADD_PAID = "AddPaid";
	private const string S_ADD_RETURN = "AddReturn";
	private const string S_EDIT_PAID = "EditPaid";
	private const string S_EDIT_RETURN = "EditReturn";

	#endregion -- CONSTANT(s) --

	#region -- PROPERTIES --

	/// <summary>
	/// Returns true if the Accounts module is enabled, false otherwise.
	/// </summary>
	protected bool IsAccountsModuleEnabled
	{
		get { return Settings.EnableAccountsModule; }
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to set default values.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }

                RefreshValue();
				ReadQueryString();
				FillBankCombo();
                FillElectronicPaymentTypes();
                FillElectronicBankCombo();                
				SetClientScriptAttributes();
				SetChequeDetails();

				if (IsAccountsModuleEnabled)
					SerializeFinancialYearDetails();
				else
					cstAcDateValidator.EnableClientScript = false;
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
			hidServerDate.Value = DateTime.Today.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en"));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save postdated cheque details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			if (cal_Date.IsValid())
			{
				switch (hidMode.Value)
				{
					case S_ADD_PAID:
						AddPaidStudentCautionMoney();
						RecordPayment(false, 0);
						break;
					case S_ADD_RETURN:
						AddReturnStudentCautionMoney();
						RecordReturnPayment(false);
						break;
					case S_EDIT_PAID:
						var oStudentCautionMoneyBL = new StudentCautionMoneyDetailsBL(hidStudentId.Value.ToInt());
						EditPaidStudentCautionMoney();
						RecordPayment(true, oStudentCautionMoneyBL.Amount);
						break;
					case S_EDIT_RETURN:
						EditReturnStudentCautionMoney();
						RecordReturnPayment(true);
						break;
				}

				var obtnSender = sender as Button;
				// If the Save button triggered this event
				if (obtnSender.Text == btnSave.Text)
				{
					hidMode.Value = "New";
					ClearAllControls();
					SetQueryString();
				}
				// If the Save & Print button triggered this event
				else
				{
                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    {
                        DisplayCautionMoneyReport(hidStudentId.Value.ToInt());
                    }
                    else
                    {
                        if (hidPostBackUrl.Value == "~/StudentPayFeeUI.aspx")
                            SetQueryStringToSaveandPrintControl();

                        string sQueryString = CommonUtility.EncryptQuerystring(String.Format("StudentId={0}&CautionMode={1}&StudentRegNo={2}&PageIndex={3}&PostBackUrl={4}",
                                                                                              hidStudentId.Value,
                                                                                              hidCautionMode.Value,
                                                                                              hidStudentRegNo.Value,
                                                                                              hidPageIndex.Value,
                                                                                              hidPostBackUrl.Value));
                        Response.Write(String.Format("<script language='javascript'>window.open('../Accountant/CautionMoneyReciept.aspx?{0}','_new','left=0,top=0,height=500,width=750,status=no,resizable=no,scrollbars=yes')</script>", sQueryString));
                    }
				}
			}
		}
		catch (DuplicateChequeNumber ex)
		{
			lblErrMsg.Visible = true;
			lblErrMsg.Text = ex.Message;
			txtChequeNumber.Focus();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to reset all controls and to cancel current transaction.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ClearAllControls();
			hidMode.Value = "New";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to to close pop up window.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			SetQueryString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to enable controls related to Cheque payment.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optCheque_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
            trRemark.Visible = true;
			EnableDisableChequeControls(true);
            EnableDisableElectronicControls(false);
			if (IsAccountsModuleEnabled && (hidMode.Value == S_ADD_RETURN || hidMode.Value == S_EDIT_RETURN))
			{
				trBankList.Visible = false;
				ddlAcBankMdtStar.Visible = true;                
			}
            
            SetFields();
            txtRemarks.Enabled = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to disable controls related to Cheque payment.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optCash_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (!txtChequeNumber.Text.Equals(string.Empty) || !txtChequeDate.Text.Equals(string.Empty))
				ResetChequeControls();
			EnableDisableChequeControls(false);
            EnableDisableElectronicControls(false);            
			ddlAcBankMdtStar.Visible = false;
            SetFields();
            trRemark.Visible = true;
            txtRemarks.Enabled = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to disable controls related to NEFT & RTGS.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optElectronic_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            trOnlinePayment.Visible = true;
            EnableDisableChequeControls(false);
            EnableDisableElectronicControls(true);
            trRemark.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This is a common method to set the appropriate labels to the controls.
    /// </summary>
    private void SetFields()
    {
        if (lblPaymentDate.Visible && !lblPaymentDate.Text.IsNullOrEmpty())
            lblDate.Text = Resources.LocalizedResources.ReturnDate;
        else
            lblDate.Text = Resources.LocalizedResources.PaymentDate;
    }

	/// <summary>
	/// Serializes the FinancialYearMaster entity object to a hidden field.
	/// </summary>
	private void SerializeFinancialYearDetails()
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
	/// This method is used to checked cash or cheque radio button.
	/// </summary>
	/// <param name="bFlag"></param>
	private void SetCashChequeOptionBtn(bool bFlag)
	{
		optCash.Checked = bFlag;
		optCheque.Checked = !bFlag;
	}

	/// <summary>
	/// This method is used to set cheque detail.
	/// </summary>
	private void SetChequeDetails()
    {
      
        EnableDisableElectronicControls(false);
		var oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
		DataTable oDTCautionMoney = oStudentCautionMoneyDetailsCollectionBL.GetReturnedCautionMoneyDetails(hidStudentId.Value.ToInt());
		DataRow oDataRow = oDTCautionMoney.Rows[0];
		txtDate.Text = DateTime.Today.ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en"));
		lblStudName.Text = oDataRow["FullName"].ToString();
       
		// Amount is returned by school and is opened in edit mode.
		if (hidMode.Value == S_ADD_RETURN || hidMode.Value == S_EDIT_RETURN)
		{
			trPaidDetails.Visible = true;
			hidPaymentDate.Value = lblPaymentDate.Text = oDataRow[S_PAYMENT_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en"));
            txtAmount.Text = oDataRow["Amount"].ToString();

            if (oDataRow["ReturnAmount"] == DBNull.Value || oDataRow["ReturnAmount"].ToInt() == 0)
                txtReturnAmount.Text = oDataRow["Amount"].ToString();
            else
                txtReturnAmount.Text = oDataRow["ReturnAmount"].ToString();

            if (Settings.AllowCautionMoneyAdjustmentInRegularFee)
            {
                trReturnAmount.Visible = true;
                hidAllowCautionMoneyAdjustment.Value = Constants.S_ONE;
            }
            else
                hidAllowCautionMoneyAdjustment.Value = Constants.S_ZERO;

            txtAmount.Enabled = false;            
            lblDate.Text = Resources.LocalizedResources.ReturnDate;
            trPaidByName.Visible = false;
         

			if (oDataRow[S_PAYMENT_MODE_FEILD].ToString() == C_CHEQUE_MODE.ToString())
			{
				lblBankName.Text = oDataRow["Bank_Name"].ToString();
				lblChequeDate.Text = oDataRow[S_CHEQUE_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Cheque_Date"].ToString();
				lblChequeNumber.Text = oDataRow[S_CHEQUE_NO_FEILD].ToString();
				lblPaymmentMode.Text = Resources.LocalizedResources.Cheque;

				ShowHideBankDetails(true);
				SetCashChequeOptionBtn(false);
				EnableDisableChequeControls(true);
                EnableDisableElectronicControls(false);

				if (IsAccountsModuleEnabled)
				{
					trBankList.Visible = false;
					ddlAcBankMdtStar.Visible = true;
					lblAcBankName.Text = Resources.LocalizedResources.BankName + " : ";
				}
			}
            else if (oDataRow[S_PAYMENT_MODE_FEILD].ToString() == C_ELECTRONIC_MODE.ToString())
            {
                lblBankName.Text = oDataRow["Bank_Name"].ToString();
                lblChequeDate.Text = oDataRow["Payment_Date"].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Cheque_Date"].ToString();
                lblChequeNumber.Text = oDataRow["TransactionNumber"].ToString();
                lblBankName.Text = oDataRow["ElectronicPaymentBank"].ToString();
                lblChequeDate.Text = oDataRow["PaymentType"].ToString();
                lblChequeDateHeader.Text = "Type";
                lblPaymmentMode.Text = "Electronic";
            }
            else
            {
                lblPaymmentMode.Text = Resources.LocalizedResources.Cash;
                ShowHideBankDetails(false);
                SetCashChequeOptionBtn(false);
                EnableDisableChequeControls(true);
                EnableDisableElectronicControls(false);
            }


            optElectronic.Visible = true;
         
		}

		switch (hidMode.Value)
		{
			case S_EDIT_RETURN:
				txtDate.Text = oDataRow[S_RETURN_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en"));
				cal_Date.SelectedDate = oDataRow[S_RETURN_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Return_Date"].ToString();
				if (oDataRow["Return_Mode"].ToString() == C_CHEQUE_MODE.ToString())
				{
					txtChequeDate.Text = oDataRow["Return_Cheque_Date"].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Return_Cheque_Date"].ToString();
					cal_ChequeDate.SelectedDate = oDataRow["Return_Cheque_Date"].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Return_Cheque_Date"].ToString();
					txtChequeNumber.Text = oDataRow["Return_Cheque_Number"].ToString();
					txtRemarks.Text = oDataRow["Return_Remarks"].ToString();
					SetBankComboBoxIndex(oDataRow["Return_Bank_Id"].ToString());
					if (IsAccountsModuleEnabled && oDataRow["ReturnDepositBankId"] != DBNull.Value)
						ddlAcBankList.SelectedValue = oDataRow["ReturnDepositBankId"].ToString();
					SetCashChequeOptionBtn(false);
					EnableDisableChequeControls(true);
                    EnableDisableElectronicControls(false);
				}
                else if (oDataRow["Return_Mode"].ToString() == C_ELECTRONIC_MODE.ToString())
                {
                    trRemark.Visible = true;
                    optElectronic.Checked = true;
                    trElectronicTypes.Visible = true;
                    txtTxnNumber.Text = oDataRow["ReturnETxnNo"].ToString();
                    cmbElectronicTypes.SelectedIndex = oDataRow["ReturnETypeId"].ToInt();
                    ddlBankNameCard.SelectedValue = oDataRow["ReturnEBankId"].ToString();
                    ddlAcCardBank.SelectedValue = oDataRow["ReturnEDepositedBankId"].ToString();
                    txtRemarks.Text = oDataRow["ReturnRemark"].ToString();
                    EnableDisableChequeControls(false);
                    EnableDisableElectronicControls(true);
                }
                else
                {
                    txtRemarks.Text = oDataRow["CRemark"].ToString();
                    SetCashChequeOptionBtn(true);
                    EnableDisableChequeControls(false);
                    EnableDisableElectronicControls(false);
                }
				break;
			case S_ADD_PAID:                
				trPaymentDetails.Visible = false;
				trPaidDetails.Visible = false;
				SetCashChequeOptionBtn(false);
				EnableDisableChequeControls(true);
				btnSavePrint.Visible = true;
				break;
			case S_EDIT_PAID:
				btnSavePrint.Visible = true;				
				trPaymentDetails.Visible = false;
				trPaidDetails.Visible = false;
				txtAmount.Text = oDataRow["Amount"].ToString();
				txtDate.Text = oDataRow[S_PAYMENT_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Payment_Date"].ToString();
				cal_Date.SelectedDate = oDataRow[S_PAYMENT_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Payment_Date"].ToString();
                txtPaidByName.Text = oDataRow["PaidByName"].ToString();
				if (oDataRow[S_PAYMENT_MODE_FEILD].ToString() == C_CHEQUE_MODE.ToString())
				{
					txtChequeDate.Text = oDataRow[S_CHEQUE_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Cheque_Date"].ToString();
					cal_ChequeDate.SelectedDate = oDataRow[S_CHEQUE_DATE_FEILD].ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en")); //oDataRow["Cheque_Date"].ToString();
					txtChequeNumber.Text = oDataRow[S_CHEQUE_NO_FEILD].ToString();
					txtRemarks.Text = oDataRow[S_REMARK_FEILD].ToString();
					if (IsAccountsModuleEnabled && oDataRow["PaymentDepositBankId"] != DBNull.Value)
						ddlAcBankList.SelectedValue = oDataRow["PaymentDepositBankId"].ToString();
					SetBankComboBoxIndex(oDataRow["Bank_Id"].ToString());
					SetCashChequeOptionBtn(false);
				}
                else if (oDataRow[S_PAYMENT_MODE_FEILD].ToString() == C_ELECTRONIC_MODE.ToString())
                {
                    trOnlinePayment.Visible = true;
                    optElectronic.Checked = true;
                    trElectronicTypes.Visible = true;
                    txtTxnNumber.Text = oDataRow["TransactionNumber"].ToString();
                    cmbElectronicTypes.SelectedIndex = oDataRow["PaymentTypeId"].ToInt();
                    ddlBankNameCard.SelectedValue = oDataRow["BankId"].ToString();
                    txtRemarks.Text = oDataRow["CRemark"].ToString();
                    EnableDisableChequeControls(false);
                    EnableDisableElectronicControls(true);
                    optCash.Enabled = false;
                    optCheque.Enabled = false;
                    //txtRemarks.Enabled = true
                    trRemark.Visible = true;
                }
                else
                {
                    SetCashChequeOptionBtn(true);
                    EnableDisableChequeControls(false);
                    txtRemarks.Text = oDataRow["CRemark"].ToString();
                }
				break;
		}
	}

	/// <summary>
	/// This method is used to set bank name index box.
	/// </summary>
	/// <param name="asBankId"></param>
	private void SetBankComboBoxIndex(string asBankId)
	{
		ddlBankName.SelectedValue = asBankId;
		int iBankIndex = ddlBankName.SelectedIndex.ToInt();
		ddlBankName.SelectedIndex = iBankIndex;
	}

	/// <summary>
	/// This method is used to show or hide bank cheque details as per mode.
	/// </summary>
	/// <param name="abFlag"></param>
	private void ShowHideBankDetails(bool abFlag)
	{
		trChqDetails.Visible = abFlag;
		trBankDetails.Visible = abFlag;
	}

	/// <summary>
	/// This method is used to add paid student caution money details.
	/// </summary>
	private void AddPaidStudentCautionMoney()
	{
		StudentCautionMoneyDetailsBL oStudentCautionMoneyDetailsBL = PopulateCautionMoneyDetails();
		if (optCheque.Checked)
		{
			StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyChequeDetailsBL = PopulateChequeDetails();
			oStudentCautionMoneyChequeDetailsBL.StudentId = hidStudentId.Value.ToInt();
            oStudentCautionMoneyChequeDetailsBL.StudentCautionMoneyId = hidStudentCautionMoneyId.Value.ToInt();

			int iPaymentChequeId = oStudentCautionMoneyChequeDetailsBL.InsertStudentCautionMoneyChequeDetails();
			oStudentCautionMoneyDetailsBL.Payment_Cheque_Id = iPaymentChequeId;
			oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
		}
        else if (optElectronic.Checked)
        {
            StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyElectronicDetails = PopulateElectronicDetails(false);
            oStudentCautionMoneyElectronicDetails.SchoolwiseStudentId = hidStudentId.Value.ToInt();
            oStudentCautionMoneyElectronicDetails.StudentCautionMoneyId = hidStudentCautionMoneyId.Value.ToInt();

            int iPaymentId = oStudentCautionMoneyElectronicDetails.InsertStudentCautionMoneyElectronicDetails();
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
        }
        else if (optCash.Checked)
        {
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
        }
	}

	/// <summary>
	/// This method is used to add return student caution money details.
	/// </summary>
	private void AddReturnStudentCautionMoney()
	{
		StudentCautionMoneyDetailsBL oStudentCautionMoneyDetailsBL = PopulateCautionMoneyDetails();
		if (optCheque.Checked)
		{
			StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyChequeDetailsBL = PopulateChequeDetails();

			// We update the BankId here because when the Accounts module is enabled, the original bankname dropdownlist is hidden.
			// And in the new list, the value field is the LedgerId instead of the BankId. Hence we need to get the corresponding BankId for the LedgerId.
			if (IsAccountsModuleEnabled)
				oStudentCautionMoneyChequeDetailsBL.Bank_Id = GetBankList().FirstOrDefault(bank => bank.Id == ddlAcBankList.SelectedValue.ToInt()).Bank.Id;

			oStudentCautionMoneyChequeDetailsBL.StudentId = hidStudentId.Value.ToInt();
			//This method is used to check wether cheque no is duplicate or not.
			oStudentCautionMoneyChequeDetailsBL.IsChequeNoDuplicate();

			int iReturnChequeId = oStudentCautionMoneyChequeDetailsBL.InsertStudentCautionMoneyChequeDetails();
			oStudentCautionMoneyDetailsBL.Return_Cheque_Id = iReturnChequeId;
			oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
		}
        else if (optElectronic.Checked)
        {
            StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyElectronicDetails = PopulateElectronicDetails(true);
            oStudentCautionMoneyElectronicDetails.SchoolwiseStudentId = hidStudentId.Value.ToInt();
            int iPaymentId = oStudentCautionMoneyElectronicDetails.InsertStudentCautionMoneyElectronicDetails();
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
        }
        else if (optCash.Checked)
        {
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
        }
	}

	/// <summary>
	/// This method is used to edit paid student caution money details.
	/// </summary>
	private void EditPaidStudentCautionMoney()
	{
		StudentCautionMoneyDetailsBL oStudentCautionMoneyDetailsBL = PopulateCautionMoneyDetails();
		if (optCheque.Checked)
		{
			StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyChequeDetailsBL = PopulateChequeDetails();
			oStudentCautionMoneyChequeDetailsBL.StudentId = hidStudentId.Value.ToInt();

			//This is used for updating Check details if hidPaymentChequeId is not null otherwise insert new record.
			if (hidPaymentChequeId.Value != String.Empty)
			{
				int iStudentCautionMoneyChequeId = hidPaymentChequeId.Value.ToInt();
				oStudentCautionMoneyDetailsBL.Payment_Cheque_Id = iStudentCautionMoneyChequeId;
				oStudentCautionMoneyChequeDetailsBL.Student_Caution_Money_Cheque_Id = iStudentCautionMoneyChequeId;
				oStudentCautionMoneyChequeDetailsBL.UpdateStudentCautionMoneyChequeDetails();
			}
			else
			{
				int iPaymentChequeId = oStudentCautionMoneyChequeDetailsBL.InsertStudentCautionMoneyChequeDetails();
				oStudentCautionMoneyDetailsBL.Payment_Cheque_Id = iPaymentChequeId;
			}
			oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
		}
        else if (optElectronic.Checked)
        {
            StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyElectronicDetails = PopulateElectronicDetails(false);
            if (hidElectronicPaymentId.Value != String.Empty && hidElectronicPaymentId.Value != Constants.S_ZERO)
            {
                oStudentCautionMoneyElectronicDetails.ElePaymentId = hidElectronicPaymentId.Value.ToInt();
                oStudentCautionMoneyElectronicDetails.UpdateStudentCautionMoneyElectronicPaymentDetails();
            }
            else
            {
                int iPaymentId = oStudentCautionMoneyElectronicDetails.InsertStudentCautionMoneyElectronicDetails();
            }
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
        }
        else if (optCash.Checked)
        {
            if (!hidPaymentChequeId.Value.Equals(string.Empty))
            {
                int iStudentCautionMoneyChequeId = hidPaymentChequeId.Value.ToInt();
                var oStudentCautionMoneyChequeDetailsBL = new StudentCautionMoneyChequeDetailsBL();
                oStudentCautionMoneyChequeDetailsBL.DeleteStudentCautionMoneyChequeDetails(iStudentCautionMoneyChequeId);
            }
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyPaidDetails();
        }
	}

	/// <summary>
	/// This method is used to edit return student caution money details.
	/// </summary>
	private void EditReturnStudentCautionMoney()
	{
		StudentCautionMoneyDetailsBL oStudentCautionMoneyDetailsBL = PopulateCautionMoneyDetails();

		if (optCheque.Checked)
		{
			StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyChequeDetailsBL = PopulateChequeDetails();
			oStudentCautionMoneyChequeDetailsBL.StudentId = hidStudentId.Value.ToInt();
            oStudentCautionMoneyChequeDetailsBL.StudentCautionMoneyId = hidStudentCautionMoneyId.Value.ToInt();


			// We update the BankId here because when the Accounts module is enabled, the original bankname dropdownlist is hidden.
			// And in the new list, the value field is the LedgerId instead of the BankId. Hence we need to get the corresponding BankId for the LedgerId.
            //if (IsAccountsModuleEnabled)
            //    oStudentCautionMoneyChequeDetailsBL.Bank_Id = GetBankList().FirstOrDefault(bank => bank.Id == ddlAcBankList.SelectedValue.ToInt()).Bank.Id;

            if (IsAccountsModuleEnabled && !ddlAcBankList.SelectedValue.IsNullOrEmpty())
            {
                BankAccount oBankAccount = GetBankList().FirstOrDefault(bank => bank.Id == ddlAcBankList.SelectedValue.ToInt());
                  if (oBankAccount != null)
                    oStudentCautionMoneyChequeDetailsBL.Bank_Id = oBankAccount.Bank.Id;
            }
			oStudentCautionMoneyChequeDetailsBL.Student_Caution_Money_Cheque_Id = hidReturnChequeId.Value == String.Empty ? Constants.I_ZERO : hidReturnChequeId.Value.ToInt();
			//   This method is used to check is there any duplicate cheque number or not.
			oStudentCautionMoneyChequeDetailsBL.IsChequeNoDuplicate();

			if (hidReturnChequeId.Value != String.Empty)
			{
				int iStudentCautionMoneyChequeId = hidReturnChequeId.Value.ToInt();
				oStudentCautionMoneyDetailsBL.Return_Cheque_Id = iStudentCautionMoneyChequeId;
				oStudentCautionMoneyChequeDetailsBL.UpdateStudentCautionMoneyChequeDetails();
			}
			else
			{
				int iReturnChequeId = oStudentCautionMoneyChequeDetailsBL.InsertStudentCautionMoneyChequeDetails();
				oStudentCautionMoneyDetailsBL.Return_Cheque_Id = iReturnChequeId;
			}
			oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
		}
        else if (optElectronic.Checked)
        {
            StudentCautionMoneyChequeDetailsBL oStudentCautionMoneyElectronicDetails = PopulateElectronicDetails(true);
            oStudentCautionMoneyElectronicDetails.StudentCautionMoneyId = hidStudentCautionMoneyId.Value.ToInt();

            if (hidElectronicPaymentId.Value != String.Empty && hidElectronicPaymentId.Value != Constants.S_ZERO)
            {
                oStudentCautionMoneyElectronicDetails.ElePaymentId = hidElectronicPaymentId.Value.ToInt();
                oStudentCautionMoneyElectronicDetails.UpdateStudentCautionMoneyElectronicPaymentDetails();
            }
            else
            {
                int iPaymentId = oStudentCautionMoneyElectronicDetails.InsertStudentCautionMoneyElectronicDetails();
            }
            oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
        }
		else if (optCash.Checked)
		{
			if (!hidReturnChequeId.Value.Equals(string.Empty))
			{
				int iStudentCautionMoneyChequeId = hidReturnChequeId.Value.ToInt();
				var oStudentCautionMoneyChequeDetailsBL = new StudentCautionMoneyChequeDetailsBL();
				oStudentCautionMoneyChequeDetailsBL.DeleteStudentCautionMoneyChequeDetails(iStudentCautionMoneyChequeId);
			}
			oStudentCautionMoneyDetailsBL.UpdateStudentCautionMoneyReturnDetails();
		}
	}

	/// <summary>
	/// This method is used to enable or disable cheque controls as per cash or cheque radio button.
	/// </summary>
	/// <param name="abIsEnabled"></param>
	private void EnableDisableChequeControls(bool abIsEnabled)
	{
        txtChequeDate.Visible = abIsEnabled;
        cal_ChequeDate.Visible = abIsEnabled;
        txtChequeNumber.Visible = abIsEnabled;
        ddlBankName.Visible = abIsEnabled;
        ddlAcBankList.Visible = abIsEnabled;		
		lblChqNumberErr.Visible = abIsEnabled;
		lblChqDateErr.Visible = abIsEnabled;
		lblBankErr.Visible = abIsEnabled;
        reqChequeDate.Visible = abIsEnabled;
        regChequeDate.Visible = abIsEnabled;
        trChequeNumber.Visible = abIsEnabled;
        trChequeDate.Visible = abIsEnabled;
        trBankList.Visible = abIsEnabled;
        trChequeBankName.Visible = abIsEnabled;
	}

    /// <summary>
    /// This method is used to enable or disable Electronic payment controls as per cash, cheque or Electronic radio button.
    /// </summary>
    /// <param name="abIsEnabled"></param>
    private void EnableDisableElectronicControls(bool abIsEnabled)
    {
        trOnlinePayment.Visible = abIsEnabled;
        //txtTxnNumber.Enabled = abIsEnabled;
        //cmbElectronicTypes.Enabled = abIsEnabled;
        //ddlBankNameCard.Enabled = abIsEnabled;
        //ddlAcCardBank.Enabled = abIsEnabled;
    }

	/// <summary>
	/// This method is used to reset all controls.
	/// </summary>
	private void ClearAllControls()
	{
		txtChequeNumber.Text = string.Empty;
		txtChequeDate.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		txtAmount.Text = string.Empty;
		txtDate.Text = string.Empty;
		lblErrMsg.Visible = false;
		lblErrMsg.Text = string.Empty;
		ddlBankName.SelectedIndex = 0;
	}

	/// <summary>
	/// This method is used to populate StudentPostDatedChequesBL and returns object of same.
	/// </summary>
	/// <returns></returns>
	private StudentCautionMoneyChequeDetailsBL PopulateChequeDetails()
	{
		return new StudentCautionMoneyChequeDetailsBL
					{
						Bank_Id = ddlBankName.SelectedValue.ToInt(),
						Cheque_Date = cal_ChequeDate.DateValue, 
						Cheque_Number = txtChequeNumber.Text,
						DepositBankId = IsAccountsModuleEnabled ? ddlAcBankList.SelectedValue.ToInt() : Constants.I_ZERO,
						Inserted_By_id = miUserId,
						Updated_By_Id = miUserId,
						Update_Date = DateTime.Now,
						Remarks = txtRemarks.Text
					};
	}

    /// <summary>
    /// This method is used to populate StudentPostDatedChequesBL and returns object of same.
    /// </summary>
    /// <returns></returns>
    private StudentCautionMoneyChequeDetailsBL PopulateElectronicDetails(bool abIsReturnRecord)
    {
        return new StudentCautionMoneyChequeDetailsBL
        {
            EleTypeId = cmbElectronicTypes.SelectedValue.ToInt(),
            TxnNumber = txtTxnNumber.Text,
            EleBankId = ddlBankNameCard.SelectedValue.ToInt(),            
          //  EleDepositBankId = IsAccountsModuleEnabled ? ddlAcCardBank.SelectedValue.ToInt() : Constants.I_ZERO,
            EleDepositBankId = IsAccountsModuleEnabled ? (string.IsNullOrEmpty(ddlAcCardBank.SelectedValue) ? 0 : ddlAcCardBank.SelectedValue.ToInt())  : Constants.I_ZERO,
            Remarks = txtRemarks.Text,
            aiSChoolId = miSchoolId,
            InsertedByid = miUserId,
            UpdatedById = miUserId,
            UpdateDate = DateTime.Now,
            InsertDate = DateTime.Now,
            IsReturnRecord = abIsReturnRecord,
        };
    }

	/// <summary>
	/// Creaets a CautionMoney BL object from the controls on the Page.
	/// </summary>
	/// <returns></returns>
	private StudentCautionMoneyDetailsBL PopulateCautionMoneyDetails()  
	{
        int iConcessionAmount = Constants.I_ZERO;
        if (hidMode.Value == S_ADD_PAID)
            iConcessionAmount = txtConcessionAmt.Text.ToInt();
        else
            iConcessionAmount = Constants.I_ZERO;

        int iAmount = txtAmount.Text.ToInt();
       
        if (iConcessionAmount != 0)
            iAmount = iAmount - iConcessionAmount;
        
		var oStudentCautionMoneyDetailsBL = new StudentCautionMoneyDetailsBL
											{
												School_Id = miSchoolId,
												Amount = iAmount,
                                                ConcessionAmount = iConcessionAmount,
												Schoolwise_Student_Id = hidStudentId.Value.ToInt(),
                                                Student_Caution_Money_Id = hidStudentCautionMoneyId.Value.ToInt(),
												Inserted_By_id = miUserId,
												Updated_By_Id = miUserId,
												Update_Date =  DateTime.Now,
                                                PaidByName = txtPaidByName.Text.TrimAll(),
                                                Remarks = txtRemarks.Text.Trim()                                                
											};

		switch (hidMode.Value)
		{
			case S_EDIT_PAID:
			case S_ADD_PAID:
				oStudentCautionMoneyDetailsBL.Paid_By_Student = true;
				oStudentCautionMoneyDetailsBL.Payment_Date = txtDate.Text.ToDateTime();
                if (!optElectronic.Checked)
                    oStudentCautionMoneyDetailsBL.Payment_Mode = optCash.Checked ? C_CASH_MODE.ToString() : C_CHEQUE_MODE.ToString();
                else
                    oStudentCautionMoneyDetailsBL.Payment_Mode = C_ELECTRONIC_MODE.ToString();
                oStudentCautionMoneyDetailsBL.Remarks = optCash.Checked || optElectronic.Checked ? txtRemarks.Text.Trim() : string.Empty;
				break;
			case S_EDIT_RETURN:
			case S_ADD_RETURN:
				oStudentCautionMoneyDetailsBL.Returned_By_School = true;
				oStudentCautionMoneyDetailsBL.Return_Date = txtDate.Text.ToDateTime();

                if (!optElectronic.Checked)
				    oStudentCautionMoneyDetailsBL.Return_Mode = optCash.Checked ? C_CASH_MODE.ToString() : C_CHEQUE_MODE.ToString(); //lblPaymentDate.Text == "Cash" ? C_CASH_MODE.ToString() : C_CHEQUE_MODE.ToString();
                else
                    oStudentCautionMoneyDetailsBL.Return_Mode = C_ELECTRONIC_MODE.ToString();

                oStudentCautionMoneyDetailsBL.Remarks = optCash.Checked || optElectronic.Checked ? txtRemarks.Text.Trim() : string.Empty;

                if (txtReturnAmount.Text != string.Empty)
                    oStudentCautionMoneyDetailsBL.ReturnAmount = txtReturnAmount.Text.ToInt();
                else
                    oStudentCautionMoneyDetailsBL.ReturnAmount = 0;
				break;
		}

		return oStudentCautionMoneyDetailsBL;
	}

	/// <summary>
	/// This method is used to read querystring.
	/// </summary>
	private void ReadQueryString()
	{
		string sEventDateDecrypt = Server.UrlDecode(Request.QueryString.ToString());

		if (sEventDateDecrypt.Equals(string.Empty))
			return;

		if (!QueryString["PostBackUrl"].IsNull())
			hidPostBackUrl.Value = QueryString["PostBackUrl"];

		txtAmount.Text = QueryString["Amount"];
        txtActualAmt.Text = QueryString["Amount"].ToString();
		hidStudentId.Value = QueryString["StudentId"];
        hidStudentCautionMoneyId.Value = QueryString["StudentCautionMoneyId"];

		if (hidPostBackUrl.Value != "~/StudentPayFeeUI.aspx")
		{
			hidPaymentChequeId.Value = QueryString["Payment_Cheque_Id"];

			hidMode.Value = QueryString["Mode"];
			hidCautionMode.Value = QueryString["CautionMode"];

			if (!QueryString["StudentRegNo"].IsNull())
				hidStudentRegNo.Value = !QueryString["StudentRegNo"].IsNullOrEmpty() ? QueryString["StudentRegNo"] : "-9999";

			hidPageIndex.Value = QueryString["PageIndex"];
			string sAdmissionDate = QueryString["AdmissionDate"];
            hidAdmissionDate.Value = sAdmissionDate.ToDateTime().ToString(S_DATE_FORMATE_FEILD, new CultureInfo("en"));
			
			if (!QueryString["Return_Cheque_Id"].IsNull())
				hidReturnChequeId.Value = QueryString["Return_Cheque_Id"];

            if (!QueryString["StudentCautionMoneyId"].IsNull())
                hidStudentCautionMoneyId.Value = QueryString["StudentCautionMoneyId"];
            if (!QueryString["ChequeNo"].IsNull())
				hidChequeNo.Value = !QueryString["ChequeNo"].IsNullOrEmpty() ? QueryString["ChequeNo"] : "-9999";

			if (!QueryString["FromDate"].IsNull())
				hidFromDate.Value = !QueryString["FromDate"].IsNullOrEmpty() ? QueryString["FromDate"] : "-9999";

			if (!QueryString["ToDate"].IsNull())
				hidToDate.Value = !QueryString["ToDate"].IsNullOrEmpty() ? QueryString["ToDate"] : "-9999";

            if (!QueryString["ElectronicPaymentId"].IsNull())
                hidElectronicPaymentId.Value = QueryString["ElectronicPaymentId"].ToString();
            else if (!QueryString["ReturnElectronicPaymentId"].IsNull())
                hidElectronicPaymentId.Value = QueryString["ReturnElectronicPaymentId"].ToString();
		}
		else
		{
			int iStudentId = hidStudentId.Value.ToInt();
			var oStudentCautionMoneyDetailsBL = new StudentCautionMoneyDetailsBL();
			DataTable odt = oStudentCautionMoneyDetailsBL.GetStudentCautionMoneyDetail(iStudentId, miAcademicYearId, miSchoolId);
			if (odt.Rows[0]["Paid_By_Student"].ToInt() == 1)
			{
				hidMode.Value = S_EDIT_PAID;
				hidCautionMode.Value = "CMPaid";
              
			}
			else
			{
				hidMode.Value = S_ADD_PAID;
				hidCautionMode.Value = "CMNotPaid";
			}
		}
	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		if (hidPostBackUrl.Value != "~/StudentPayFeeUI.aspx")
		{
			string sQueryString = string.Format("StudentId={0}&CautionMode={1}&StudentRegNo={2}&PageIndex={3}&ChequeNo={4}&FromDate={5}&ToDate={6}&PostBackUrl={7}",
												 hidStudentId.Value,
												 hidCautionMode.Value,
												 hidStudentRegNo.Value,
												 hidPageIndex.Value,
												 hidChequeNo.Value,
												 hidFromDate.Value,
												 hidToDate.Value,
												 hidPostBackUrl.Value);
			sQueryString = string.Format("'?{0}'", CommonUtility.EncryptQuerystring(sQueryString));
			Response.Write(string.Format("<script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus();</Script> ", sQueryString));
		}
		else
		{
			var oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
			int iYearwiseStudentId = oStudentCautionMoneyDetailsCollectionBL.GetStudentAcademicYearId(hidStudentId.Value.ToInt(), miSchoolId, miAcademicYearId);
			hidYearwiseStudentId.Value = Convert.ToString(iYearwiseStudentId);
			string sQueryString = string.Format("'?{0}'",
												CommonUtility.EncryptQuerystring(string.Format("StudentId={0}",
																								hidYearwiseStudentId.Value)));
			Response.Write(string.Format("<script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.opener.focus(); window.close();</script>", sQueryString));
		}
	}

	/// <summary>
	/// Sets the QueryString when Receipt is to be printed.
	/// </summary>
	private void SetQueryStringToSaveandPrintControl()
	{
		if (hidPostBackUrl.Value != "~/StudentPayFeeUI.aspx")
		{
			string sQueryString = string.Format("StudentId={0}&CautionMode={1}&StudentRegNo={2}&PageIndex={3}&ChequeNo={4}&FromDate={5}&ToDate={6}&PostBackUrl={7}",
												 hidStudentId.Value,
												 hidCautionMode.Value,
												 hidStudentRegNo.Value,
												 hidPageIndex.Value,
												 hidChequeNo.Value,
												 hidFromDate.Value,
												 hidToDate.Value,
												 hidPostBackUrl.Value);
			sQueryString = string.Format("'?{0}'", CommonUtility.EncryptQuerystring(sQueryString));
			Response.Write(string.Format("<script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus();</Script> ", sQueryString));
		}
		else
		{
			var oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
			int iYearwiseStudentId = oStudentCautionMoneyDetailsCollectionBL.GetStudentAcademicYearId(hidStudentId.Value.ToInt(), miSchoolId, miAcademicYearId);
			hidYearwiseStudentId.Value = Convert.ToString(iYearwiseStudentId);
			string sQueryString = string.Format("'?{0}'",
												CommonUtility.EncryptQuerystring(string.Format("StudentId={0}",
																								hidYearwiseStudentId.Value)));
			Response.Write(string.Format("<script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.opener.focus();</script>", sQueryString));
		}
	}

	/// <summary>
	/// This method is used to reset cheque control as per option button click.
	/// </summary>
	private void ResetChequeControls()
	{
		txtChequeNumber.Text = string.Empty;
		txtChequeDate.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		lblErrMsg.Visible = false;
		lblErrMsg.Text = string.Empty;
		ddlBankName.SelectedIndex = 0;
	}

	/// <summary>
	/// This method is used to fill combobox with bank list.
	/// </summary>
	private void FillBankCombo()
	{
		var oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
		ddlBankName.Bind(dtBankList, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);

		// Fill the Banks drop-down list if the Accounts module is enabled.
		if (IsAccountsModuleEnabled)
			ddlAcBankList.Bind(GetBankList(), "Id", "Name", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to set client script javascript variables.
	/// </summary>
	private void SetClientScriptAttributes()
	{
		valChequeData.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
		ApplyMouseHoverEffect(new List<Button> { btnSave, btnSavePrint, btnClose });
        txtActualAmt.Enabled = false;
        if (hidMode.Value == S_ADD_PAID)
        {
            trConcessionAmount.Visible = true;
            trNetAmount.Visible = true;
            
        }
        else
        {
            trConcessionAmount.Visible = false;
            trNetAmount.Visible = false;
        }
	}

	/// <summary>
	/// Returns a List of BankAccountDetails entity objects.
	/// </summary>
	/// <returns></returns>
	private List<BankAccount> GetBankList()
	{
		var olstBanks = new List<BankAccount>();
		if (IsAccountsModuleEnabled)
		{
            BankAccountClient oBankClient = null;
			try
			{
                oBankClient = new BankAccountClient();
				oBankClient.Open();
				olstBanks = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : An exception occured while retreiving Bank list.");
			}
			finally
			{
				if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
					oBankClient.Close();
			}
		}
		return olstBanks;
	}

	/// <summary>
	/// Records the payment in the Accounts module when Caution money is paid or edited.
	/// </summary>
	private void RecordPayment(bool abIsEdit, int aiAmount)
	{
		if (IsAccountsModuleEnabled)
		{
			AccountVoucherClient oVoucherClient = null;
			try
			{
				oVoucherClient = new AccountVoucherClient();
				oVoucherClient.Open();

				// If it's an UPDATE operation, we need to delete the payment first.
				// Later we will record the new payment details according to mode of payment.
				if (abIsEdit)
					oVoucherClient.DeleteCautionMoneyPayment(miSchoolId, miFinancialYearId, hidStudentId.Value.ToInt(), aiAmount, miUserId);

				// We record the payment only if it's a cash payment.
				// (we do not record cheque payments because they will be recorded from the clearance screen.)
				if (optCash.Checked)
					oVoucherClient.RecordCautionMoneyPayment(miSchoolId, miFinancialYearId, hidStudentId.Value.ToInt(), miUserId);
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format(Resources.LocalizedResources.EceptionAccountsModule + "- {0}.",
																		 hidStudentId.Value.ToInt()));
			}
			finally
			{
				if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
					oVoucherClient.Close();
			}
		}
	}

	/// <summary>
	/// Records the return payment in the Accounts module when Caution money is returned.
	/// </summary>
	private void RecordReturnPayment(bool abIsEdit)
	{
		if (IsAccountsModuleEnabled)
		{
			AccountVoucherClient oVoucherClient = null;
			try
			{
				oVoucherClient = new AccountVoucherClient();
				oVoucherClient.Open();

				// If it's an UPDATE operation, we need to delete the return payment first.
				// Later we will record the new return payment details according to mode of payment.
				if (abIsEdit)
					oVoucherClient.DeleteCautionMoneyReturnPayment(miSchoolId, miFinancialYearId, hidStudentId.Value.ToInt(), miUserId);

				// We record the return payment only if it's a cash payment.
				// (we do not record cheque payments because they will be recorded from the clearance screen.)
				if (optCash.Checked)
					oVoucherClient.RecordCautionMoneyReturnPayment(miSchoolId, miFinancialYearId, hidStudentId.Value.ToInt(), miUserId);
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format(Resources.LocalizedResources.EceptionReturnAccountsModule + " - {0}.",
																		 hidStudentId.Value.ToInt()));
			}
			finally
			{
				if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
					oVoucherClient.Close();
			}
		}
	}
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidAmountShouldNotBeBlank.Value = Resources.LocalizedResources.AmountShouldNotBeBlank;
        hidChequeNumberShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeNumberShouldNotBeBlank;
        hidChequeDateShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeDateShouldNotBeBlank;
        hidBankNameShouldBeSelected.Value = Resources.LocalizedResources.BankNameShouldBeSelected;
        hidAmountShouldBeGreaterThanZero.Value = Resources.LocalizedResources.AmountShouldBeGreaterThanZero;
        hidReturnDateShouldNotBeBlank.Value = Resources.LocalizedResources.ReturnDateShouldNotBeBlank;
        hidPaymentDateShouldNotBlank.Value = Resources.LocalizedResources.PaymentDateShouldNotBlank;
        hidReturnDateIsInvalid.Value = Resources.LocalizedResources.ReturnDateIsInvalid;
        hidDepositInBankShouldBeSelected.Value = Resources.LocalizedResources.DepositInBankShouldBeSelected;
        hidPaymentDateIsInvalid.Value = Resources.LocalizedResources.PaymentDateIsInvalid;
        hidshouldNotBeFutureDate.Value = Resources.LocalizedResources.shouldNotBeFutureDate;
        hidReturnDateShouldBeGreaterThanPaymentDate.Value = Resources.LocalizedResources.ReturnDateShouldBeGreaterThanPaymentDate;
        hidPaymentDateShouldBeGreaterThanOrEqualAdmissionDate.Value = Resources.LocalizedResources.PaymentDateShouldBeGreaterThanOrEqualAdmissionDate;
        hidFinancialYearIsClosedAndYouDoNotHaveEditAccess.Value = Resources.LocalizedResources.FinancialYearIsClosedAndYouDoNotHaveEditAccess;
        hidDateShouldBeWithinCurrentFinancialYear.Value = Resources.LocalizedResources.DateShouldBeWithinCurrentFinancialYear;
        hidFrom1April.Value = Resources.LocalizedResources.From1April;
    }

    /// <summary>
    /// This Method is used to get filters for display report.
    /// </summary>
    private string GetCautionMoneyFilterString(int aiStudentId)
    {
        string sRecordSelectionFormula = string.Empty;

        int iStudentId = hidStudentId.Value.ToInt();
        sRecordSelectionFormula = "(usp_GetCautionMoneyRecieptForSNS.SchoolId }=" + miSchoolId + "AND usp_GetCautionMoneyRecieptForSNS.StudentId }=" + aiStudentId + ") @";
        return sRecordSelectionFormula;
    }

    /// <summary>
    /// This Method is used to display receipt of caution money for SNS school.
    /// </summary>
    private void DisplayCautionMoneyReport(int aiStudentId)
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentCautionMoneySNS, GetCautionMoneyFilterString(aiStudentId), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method will be used to fill all the electronic types into the types dropdownlist.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt(), miUserId);
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbElectronicTypes, "Type", "TypeId", Constants.S_SELECT);
    }   

    /// <summary>
    /// This method is used to fill combobox with bank list.
    /// </summary>
    private void FillElectronicBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);        
        ControlUtility.FillDropDownList(dtBankList, ref ddlBankNameCard, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);

        if (IsAccountsModuleEnabled)
        {
            BankAccountClient oBankClient = new BankAccountClient();
            try
            {
                oBankClient.Open();
                List<BankAccount> lstLedgers = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);

                ddlAcCardBank.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);                
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an exception fetching Bank Account details.");
            }
            finally
            {
                if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
                    oBankClient.Close();
            }
        }
    }

	#endregion -- PRIVATE METHOD(s) --    
}