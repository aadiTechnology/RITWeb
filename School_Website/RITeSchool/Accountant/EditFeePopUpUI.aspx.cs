/* -------------------------------------------------------------------------------
 *	MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *	Author	: Vishal B. Shah
 *	Date	: 14-Jan-2012
 *	Purpose	: Modified to record details in the Accounts Module.
 * -----------
 *	Author	: Vishal B. Shah
 *	Date	: 10-Feb-2012
 *	Purpose	: For Card & Cheque payments, IsDirectlyDeposited param is sent
 *			  explicitly as 'N'. Reason being that otherwise, they show up on the
 *			  payment clearance screen when searching for cash payments. The USP
 *			  for fetching cash payments for clearance gets all the payments which
 *			  have the Is_Directly_Deposited flag as 'Y'. Hence it needs to be 'N'
 *			  for payments other than cash.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Xml;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using FeeEntities;
using SchoolBusinessService;
using Utility;

public partial class EditFeePopUpUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_SELECT_CHECKBOX = "ChkSelect";
	private const string S_STUDENT = "Student";
	private const string S_ELEMENT = "element";

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
	/// This event is used to handle the loading of the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
        {            
            ReadQueryString();
            if (!IsPostBack)
            {
                FillAllCombos();
                InitializeForm();
                FillFeeDetailsList();
                SetJavaScriptAttributes();
                chkSelectStudentFee_Checked(sender, e);

                cal_ChequeDate.DateValue = DateTime.Now;

                if (IsAccountsModuleEnabled)
                    SerializeFinancialYearDetails();
                else
                    cstAcDateValidator.EnableClientScript = false;
            }

			txtPaymentDate.ReadOnly=true;
			txtDate.ReadOnly=true;
			txtChequeDate.ReadOnly=true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set the state of the checkbox for each fee type in the grid & to save their feeids to a hidden field
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvStudentFeeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			string sFeeIds = String.Empty;
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iRowIndex = oCurrentItem.DisplayIndex;
				var chkSelect = oCurrentItem.FindControl(S_SELECT_CHECKBOX) as CheckBox;
				if (lstvStudentFeeDetails.DataKeys[iRowIndex]["FeeMode"].ToString() != "Debit")
					chkSelect.Checked = true;
				if (chkSelect.Checked)
				{
					int iStudentFeeId = lstvStudentFeeDetails.DataKeys[iRowIndex]["Schoolwise_Student_Fee_Id"].ToInt();
					sFeeIds += String.Format("{0},", iStudentFeeId);
                    hidStudentFeeIds.Value = sFeeIds.Substring(0, sFeeIds.LastIndexOf(","));                    
				}
                if (!IsPostBack)
                    hidPaidFeeIds.Value += sFeeIds;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to handle the check changed event of each fee type in the grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkSelectStudentFee_Checked(object sender, EventArgs e)
	{
		try
		{
			int iAmt = 0;
			string sLateFeeRemarks = String.Empty;
			int iLateFee = 0;
			string sRemarks = String.Empty;
			string sFeeIds = String.Empty;
            string sDistribution = String.Empty;
			        
			for (int iRowCount = 0; iRowCount < lstvStudentFeeDetails.Items.Count; iRowCount++)
			{
				ListViewDataItem oCurrentItem = lstvStudentFeeDetails.Items[iRowCount];
				var chkFeeId = oCurrentItem.FindControl(S_SELECT_CHECKBOX) as CheckBox;
				if (chkFeeId.Checked)
				{
					var oPaidAmt = oCurrentItem.FindControl("lblPaidAmt") as Label;
					var oAmtPayable = oCurrentItem.FindControl("lblAmtPayable") as Label;
					var oAmtPayableFor = oCurrentItem.FindControl("lblPayableFor") as Label;
					var oAmtLateFee = oCurrentItem.FindControl("lblLateFeeAmt") as Label;
					int iStudentFeeId = lstvStudentFeeDetails.DataKeys[iRowCount]["Schoolwise_Student_Fee_Id"].ToInt();                    
                    iAmt = iAmt + oPaidAmt.Text.ToInt() + oAmtPayable.Text.ToInt();
					if (!sRemarks.Contains(((Label)oCurrentItem.FindControl("lblPayableFor")).Text))
						sRemarks = string.Format("{0},{1} ({2} - Rs. {3} /-) ", sRemarks, ((Label)oCurrentItem.FindControl("lblPayableFor")).Text, ((Label)oCurrentItem.FindControl("lblFeeType")).Text, oPaidAmt.Text.ToInt() + oAmtPayable.Text.ToInt());
					sFeeIds += String.Format("{0},", iStudentFeeId);

                    if (oAmtLateFee.Text != Constants.S_ZERO)
                    {
                        sLateFeeRemarks = sLateFeeRemarks + "," + oAmtPayableFor.Text;
                        iLateFee = iLateFee + oAmtLateFee.Text.ToInt();
                        sDistribution=sDistribution+"+"+oAmtLateFee.Text;
                    }                    
				}
			}                      
            
            txtLateFeeAmt.Text = iLateFee.ToString();
			bool bIsMaxFeeApplicable = Settings.IsMaxFeeApplicable;
            if (bIsMaxFeeApplicable)
            {
                trNote.Visible = true;
                int iMaxFee = Settings.MaxFee ;
                lblVerifyNote.Text = Settings.MaxFeeNote;
                if (iMaxFee < txtLateFeeAmt.Text.ToInt() && sDistribution != string.Empty)
                {
                    lblDistribution.Text = "(Actual Late Fee: " + sDistribution.Substring(1) + ")";
                    txtLateFeeAmt.Text = iMaxFee.ToString();
                }
                else
                    lblDistribution.Text = string.Empty;
            }
            else            
                lblDistribution.Visible = false;
            
            if (hidPaidFeeIds.Value == sFeeIds && hidPaymentDete.Value==cal_PaymentDate.DateValue.ToShortDateString())
            {
                txtLateFeeAmt.Text = hidLateFeeAmtPaid.Value;
            }
		   
			if (sFeeIds != string.Empty)
				hidStudentFeeIds.Value = sFeeIds.Substring(0, sFeeIds.LastIndexOf(","));

			hidLateFeeDesc.Value = hidLateFeeRemark.Value;

			if (sLateFeeRemarks != string.Empty)
				hidLateFeeDesc.Value = "," + sLateFeeRemarks.Substring(1, (sLateFeeRemarks.Length - 1));

			txtPayableAmt.Text = iAmt.ToString();
			
			int iConcessionAmt = 0;
			if (!txtConcessionAmt.Text.Trim().IsNullOrEmpty())
				iConcessionAmt = txtConcessionAmt.Text.Trim().ToInt();

			txtActualAmt.Text = (iAmt + txtLateFeeAmt.Text.ToInt() - iConcessionAmt).ToString();	

			if (hidLateFeeDesc.Value.StartsWith(","))
				hidLateFeeDesc.Value = hidLateFeeDesc.Value.Substring(1);

			if (sRemarks.StartsWith(","))
				sRemarks = sRemarks.Substring(1);
			txtRemarks.Text = string.Format("Amount paid for {0}", sRemarks);

            if (txtConcessionAmt.Text != Constants.S_ZERO)
				txtRemarks.Text += string.Format(" with  Concession Fee (Concession Fee - Rs. {0}/-) ", txtConcessionAmt.Text);

			if (iLateFee != Constants.I_ZERO && (iLateFee + hidLateFeeAmtPaid.Value.ToInt()) != 0)
				txtRemarks.Text += string.Format("& Late fee for {0} (Rs. {1}/-)", hidLateFeeDesc.Value, txtLateFeeAmt.Text);

            hidEditedfeeIds.Value = sFeeIds;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This method is used to handle event when we change payment date
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cal_PaymentDate_SelectionChanged(object sender, EventArgs e)
    {
        FillFeeDetailsList();
        SetLateFees();       
        chkSelectStudentFee_Checked(sender, e);
    }

	/// <summary>
	/// This event is used to fill the PayableFor dropdown list according to the fee type selected.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlFeeType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			cmbPayableFor.Items.Clear();
			int iStdFeeTypeId = this.cmbFeeType.SelectedValue.ToInt();
			cmbPayableFor.Items.Add(new ListItem(Constants.S_SELECT, "0"));
			string sFeeIds = String.Empty;
			for (int iRowCount = 0; iRowCount < lstvStudentFeeDetails.Items.Count; iRowCount++)
			{
				ListViewDataItem oCurrentItem = lstvStudentFeeDetails.Items[iRowCount];
				int iStudentFeeId = lstvStudentFeeDetails.DataKeys[iRowCount]["Schoolwise_Student_Fee_Id"].ToInt();
				var chkFeeId = oCurrentItem.FindControl(S_SELECT_CHECKBOX) as CheckBox;
				if (chkFeeId.Checked)
					sFeeIds += String.Format("{0},", iStudentFeeId);
			}
            hidStudentFeeIds.Value = sFeeIds.Substring(0, sFeeIds.LastIndexOf(","));

			int iAmtToBePaid = (this.hidAmtToBePaid.Value != String.Empty ? hidAmtToBePaid.Value : txtAmtToBePaid.Text).ToInt();

			List<PayableForDetails> lstPayableForDetails = EditStudentPaidFeeBL.GetPayableForDetails(miSchoolId, miAcademicYearId, iStdFeeTypeId, hidStudentId.Value.ToInt(), hidStudentFeeIds.Value, txtActualAmt.Text.ToInt() >= iAmtToBePaid);
			lstPayableForDetails.ForEach(feeType => cmbPayableFor.Items.Add(new ListItem(feeType.PayableFor, feeType.PayableFor)));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to close pop up and refresh parent window.
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
	/// This event is used to pay the fee.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnPay_Click(object sender, EventArgs e)
	{
		try
		{
			const string S_CASH = "By Cash";
			const string S_CHEQUE = "By Cheque";
			const string S_CARD = "By Card";
			const string S_ERR_MSG = "Cheque Number already exists for this student. Please enter another cheque number.";
			const string S_ERR_MSG_SWAPNUMBER = "Swipe Number already exists for this student. Please enter another swipe number.";
			char cDirectlyPaid = chkDirectlyPaid.Checked ? Constants.C_YES : Constants.C_NO;
			string sReceiptNo = txtReceiptNo.Text.TrimStart('0');
			int iStudentId = hidStudentId.Value.ToInt();
			int iAmtToBePaid = txtAmtToBePaid.Text.ToInt();
			int iActualAmt = txtActualAmt.Text.ToInt();
			DateTime dtPaymentDate = cal_PaymentDate.DateValue;
			string sRemarks = txtRemarks.Text;
			int iConcessionAmt = txtConcessionAmt.Text.ToInt();
			var oStudentChequeDetails = new StudentPostDatedChequesBL();
			int iInsertedById = miUserId;
			int iUpdatedById = miUserId;
			string sStudentFeeList = GetXMLForStudentFeeIds();
			int iLateFeeAmt = txtLateFeeAmt.Text.Trim().ToInt();
			int iBankId = cmbBankNameDirectlyPaid.SelectedValue.ToInt();
			int iReceiptNo = sReceiptNo.ToInt();
			string sChallanNo = String.Empty;

			string sStudentFeeIdsXML = String.Empty;

			// We get the FeeVoucher particulars for the given Student and ReceiptNo.
			// This needs to be performed now (before fee being delete in the db) because after deletion,
			// it is difficult to get the correct particulars (since there could be multiple deleted entries).
			if (IsAccountsModuleEnabled)
				sStudentFeeIdsXML = GetFeeDetailsXML(iStudentId, sReceiptNo);

			// When the Amount paid is equal to the Actual Amount payable.
			if (iAmtToBePaid == iActualAmt)
			{
				switch (hidPaymentType.Value)
				{
					// Cash payments.
					case S_CASH:
						int iDepositBankId = iBankId;

						if (chkDirectlyPaid.Checked)
							sChallanNo = txtChallanNo.Text.Trim();
						
						if (IsAccountsModuleEnabled && chkDirectlyPaid.Checked)
							iBankId = GetBankIdForLedger(iDepositBankId);

						EditStudentPaidFeeBL.PayStudentFee(miSchoolId, miAcademicYearId, iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, dtPaymentDate, iConcessionAmt,
														   cDirectlyPaid, iBankId, iDepositBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById, sChallanNo);
						if (IsAccountsModuleEnabled)
						{
							DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);

							// If Accounts Module is enabled and the payment was NOT directly paid into the bank.
							if (!chkDirectlyPaid.Checked)
								RecordCashPayment(iStudentId, sReceiptNo);
						}
						break;
					// Card payments.
					case S_CARD:
						if (!oStudentChequeDetails.IsSwapNoDuplicate(txtSwapNumber.Text, iStudentId, iReceiptNo, miAcademicYearId))
						{
							// IsDirecltyDeposited param is sent explicitly as 'N'. The reason being that otherwise it shows up on the Payment Clearance screen when searching for Cash payments.
							// The USP for Cash payments feteches those payments which have the Is_Directly_Deposited flat as 'Y'.
							EditStudentPaidFeeBL.PayStudentFeeWithCard(iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, GetXMLForCardDetails(), dtPaymentDate,
																	   iConcessionAmt, Constants.C_NO, iBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById);

							if (IsAccountsModuleEnabled)
								DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);
						}
						else
						{
							lblErrorMsg.Visible = true;
							lblErrorMsg.Text = S_ERR_MSG_SWAPNUMBER;
							txtSwapNumber.Focus();
						}
						break;
					// Cheque payments.
					case S_CHEQUE:
						if (!oStudentChequeDetails.IsChequeNoDuplicate(txtChequeNumber.Text, iStudentId, iReceiptNo, miAcademicYearId))
						{
							// IsDirecltyDeposited param is sent explicitly as 'N'. The reason being that otherwise it shows up on the Payment Clearance screen when searching for Cash payments.
							// The USP for Cash payments feteches those payments which have the Is_Directly_Deposited flat as 'Y'.
							EditStudentPaidFeeBL.PayStudentFeeWithCheque(iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, GetXMLForChequeDetails(), dtPaymentDate, iConcessionAmt,
																		 Constants.C_NO, iBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById);

							if (IsAccountsModuleEnabled)
								DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);
						}
						else
						{
							lblErrorMsg.Visible = true;
							lblErrorMsg.Text = S_ERR_MSG;
							txtChequeNumber.Focus();
						}
						break;
				}
				SendSMS(EditStudentPaidFeeBL.CanSendSMS.ToBool(), EditStudentPaidFeeBL.MobileNumber, EditStudentPaidFeeBL.UserId,EditStudentPaidFeeBL.Designation);
			}
			// When Amount paid it NOT equal to Actual amount payable.
			else
			{
				// If a new Fee type was chosen, we need to create a ledger for it!
				if (IsAccountsModuleEnabled && rdoFeeType.SelectedItem.Text == "New Fee Type")
					CreateLedgerForNewFeeType(txtFeeType.Text.Trim());

				string sCreditDetails = GetXMLForCreditDetails();
				
				switch (hidPaymentType.Value)
				{
					// Cash payments.
					case S_CASH:
						int iDepositBankId = iBankId;

						if (chkDirectlyPaid.Checked)
							sChallanNo = txtChallanNo.Text.Trim();

						if (IsAccountsModuleEnabled && chkDirectlyPaid.Checked)
							iBankId = GetBankIdForLedger(iDepositBankId);

						EditStudentPaidFeeBL.PayStudentFee(miSchoolId, miAcademicYearId, iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, sCreditDetails, dtPaymentDate, iConcessionAmt,
														   cDirectlyPaid, iBankId, iDepositBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById, sChallanNo);
						if (IsAccountsModuleEnabled)
						{
							DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);
					
							// If Accounts Module is enabled and the payment was NOT directly paid into the bank.
							if (!chkDirectlyPaid.Checked)
								RecordCashPayment(iStudentId, sReceiptNo);
						}
						break;
					// Card payments.
					case S_CARD:
						if (!oStudentChequeDetails.IsSwapNoDuplicate(txtSwapNumber.Text, iStudentId, iReceiptNo, miAcademicYearId))
						{
							// IsDirecltyDeposited param is sent explicitly as 'N'. The reason being that otherwise it shows up on the Payment Clearance screen when searching for Cash payments.
							// The USP for Cash payments feteches those payments which have the Is_Directly_Deposited flat as 'Y'.
							EditStudentPaidFeeBL.PayStudentFeeWithCard(iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, sCreditDetails, GetXMLForCardDetails(), dtPaymentDate,
																	   iConcessionAmt, Constants.C_NO, iBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById);

							if (IsAccountsModuleEnabled)
								DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);
						}
						else
						{
							lblErrorMsg.Visible = true;
							lblErrorMsg.Text = S_ERR_MSG_SWAPNUMBER;
							txtSwapNumber.Focus();
						}
						break;
					// Cheque payments.
					case S_CHEQUE:
						if (!oStudentChequeDetails.IsChequeNoDuplicate(txtChequeNumber.Text, iStudentId, iReceiptNo, miAcademicYearId))
						{
							// IsDirecltyDeposited param is sent explicitly as 'N'. The reason being that otherwise it shows up on the Payment Clearance screen when searching for Cash payments.
							// The USP for Cash payments feteches those payments which have the Is_Directly_Deposited flat as 'Y'.
							EditStudentPaidFeeBL.PayStudentFeeWithCheque(iAmtToBePaid, iActualAmt, iStudentId, sStudentFeeList, sRemarks, sCreditDetails, GetXMLForChequeDetails(), dtPaymentDate,
																		 iConcessionAmt, Constants.C_NO, iBankId, iReceiptNo, iLateFeeAmt, hidLateFeeDesc.Value, iInsertedById, iUpdatedById);

							if (IsAccountsModuleEnabled)
								DeleteFeeVoucher(iStudentId, sReceiptNo, sStudentFeeIdsXML);
						}
						else
						{
							lblErrorMsg.Visible = true;
							lblErrorMsg.Text = S_ERR_MSG;
							txtChequeNumber.Focus();
						}
						break;
				}
				SendSMS(EditStudentPaidFeeBL.CanSendSMS.ToBool(), EditStudentPaidFeeBL.MobileNumber, EditStudentPaidFeeBL.UserId, EditStudentPaidFeeBL.Designation);
			}
			SetQueryString();
		}
		catch(DuplicateName dnex)
		{
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = dnex.Message;
			txtChallanNo.Focus();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// Sets late fees as  per payment date
    /// </summary>
    private void SetLateFees()
    {
        DateTime oPaymentDate = cal_PaymentDate.DateValue;
        for (int iRowCount = 0; iRowCount < lstvStudentFeeDetails.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = lstvStudentFeeDetails.Items[iRowCount];
            var oAmtLateFee = oCurrentItem.FindControl("lblLateFeeAmt") as Label;
            DateTime oDueDate = lstvStudentFeeDetails.DataKeys[iRowCount]["DueDate"].ToDateTime();
            var chkFeeId = oCurrentItem.FindControl(S_SELECT_CHECKBOX) as CheckBox;
            string sFeeId = lstvStudentFeeDetails.DataKeys[iRowCount]["Schoolwise_Student_Fee_Id"].ToString();
            if (ContainsfeeId(hidEditedfeeIds.Value,sFeeId))
                chkFeeId.Checked = true;
            else
                chkFeeId.Checked = false;
            if (chkFeeId.Checked)
            {
                if (oDueDate >= oPaymentDate)
                    oAmtLateFee.Text = Constants.S_ZERO;
                else
                    if (lstvStudentFeeDetails.DataKeys[iRowCount]["OriginalLateFee"].ToString() != Constants.S_ZERO)
                        oAmtLateFee.Text = lstvStudentFeeDetails.DataKeys[iRowCount]["OriginalLateFee"].ToString();
                    else
                        oAmtLateFee.Text = lstvStudentFeeDetails.DataKeys[iRowCount]["LateFee"].ToString();
            }
            else
            {
                oAmtLateFee.Text = oDueDate >= oPaymentDate ? Constants.S_ZERO : lstvStudentFeeDetails.DataKeys[iRowCount]["OriginalLateFee"].ToString();
            }
        }
    }

/// <summary>
    /// This method is used to send the SMS to Activated user.
    /// </summary>
    /// <param name="aCanSendSMS"></param>
    public void SendSMS(bool aCanSendSMS, string asMobileNumber, int aiUserId,string asDesignation)
    {
        Hashtable oHTUsersMobileNo = new Hashtable();
        if (aCanSendSMS && !asMobileNumber.IsNullOrEmpty())        
        {             
            string[] sMobileNumber = asMobileNumber.Split(',');            
            string sTemplateName = string.Empty;
            string sSmsText = string.Empty;
            string sTemplateRegistrationId = string.Empty;

            oHTUsersMobileNo[aiUserId] = sMobileNumber[0].Trim();

            if (sMobileNumber.Length > Constants.I_ONE && !sMobileNumber[1].Trim().IsNullOrEmpty() && sMobileNumber[0].Trim() != sMobileNumber[1].Trim())
                oHTUsersMobileNo[aiUserId + "sm;"] = sMobileNumber[1].Trim();

            int iSmsId = Convert.ToInt32(Constants.SMSTemplate.FeeDefaulterActivationSMS);
            DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDTTemplate.Rows.Count != 0)
            {
                if (oDTTemplate.Rows[0][2] != DBNull.Value)
                {
                    sSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);

                    if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sTemplateName = Convert.ToString(oDTTemplate.Rows[0][1]);
                }
            }

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);            
            SMS oSMS = new SMS();
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sTemplateName;
            oSMS.DisplayText = hidStudentName.Value.ToString() + ' ' + asDesignation;
            oSMS.SchoolID = miSchoolId;
            oSMS.AcademicYearID = miAcademicYearId;
            oSMS.To = oHTUsersMobileNo;
            int iCount = oSMS.Send();
            oHTUsersMobileNo.Clear();            
              
        }
    }
    private bool ContainsfeeId(string sEditedFeeIds, string sfeeId)
    {
        sEditedFeeIds = "," + sEditedFeeIds;
        return (sEditedFeeIds.IndexOf("," + sfeeId + ",") != -1);       
    }

	/// <summary>
	/// Initializes member variables from the QueryString.
	/// </summary>
	private void ReadQueryString()
	{
		if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
			return;
		
		if (!QueryString["StudentId"].IsNull())
			hidStudentId.Value = QueryString["StudentId"];
			
		if (!QueryString["ReceiptNo"].IsNull())
			hidReciptNo.Value = QueryString["ReceiptNo"];
			
		if (!QueryString["StandardId"].IsNull())
			hidStandardId.Value = QueryString["StandardId"];
		if (!QueryString["StudentName"].IsNull())
            hidStudentName.Value = QueryString["StudentName"].ToString();
	}

	/// <summary>
	/// Initializes fields on the page.
	/// </summary>
	private void InitializeForm()
	{
		hidServerDate.Value = Convert.ToString(DateTime.Today);
		cal_PaymentDate.DateValue = DateTime.Today;
		cal_CDate.DateValue = DateTime.Today;
		FeeDetails oFeeDetails = EditStudentPaidFeeBL.GetAllPaidFeeDetails(hidStudentId.Value.ToInt(), hidReciptNo.Value.ToInt(), miSchoolId, miAcademicYearId);
		int iReceiptMinimumDigits = Settings.ReceiptMinimumDigits;
		txtReceiptNo.Text = oFeeDetails.Receipt_Number.Length >= iReceiptMinimumDigits ? oFeeDetails.Receipt_Number : oFeeDetails.Receipt_Number.PadLeft(iReceiptMinimumDigits, '0');
		txtPayableAmt.Text = oFeeDetails.AmtPaid.ToString();
		txtAmtToBePaid.Text = (oFeeDetails.AmtPaid + oFeeDetails.LateFee - oFeeDetails.ConcessionAmt).ToString();
		txtActualAmt.Text = (oFeeDetails.AmtPaid + oFeeDetails.LateFee - oFeeDetails.ConcessionAmt).ToString();
		txtChequeNumber.Text = oFeeDetails.Cheque_Number;
		chkDirectlyPaid.Checked = oFeeDetails.IsDirectlyDeposited;
		if (!oFeeDetails.ChallanNo.IsNullOrEmpty())
			txtChallanNo.Text = oFeeDetails.ChallanNo;
		cmbBankName.SelectedValue = Convert.ToString(oFeeDetails.Bank_Id);
		cmbBankNameDirectlyPaid.SelectedValue = Convert.ToString(oFeeDetails.Bank_Id);
		if (IsAccountsModuleEnabled)
		{
			if (oFeeDetails.PaymentMode == "By Card")
				ddlAcCardBank.SelectedValue = oFeeDetails.DepositBankId.ToString();
			if (oFeeDetails.PaymentMode == "By Cheque")
				ddlAcChqBank.SelectedValue = oFeeDetails.DepositBankId.ToString();
			if (oFeeDetails.IsDirectlyDeposited)
				cmbBankNameDirectlyPaid.SelectedValue = oFeeDetails.DepositBankId.ToString();
		}
		txtRemarks.Text = oFeeDetails.Remarks;
		hidPaymentMode.Value = oFeeDetails.PaymentMode;
		hidTotalAmtToPay.Value = oFeeDetails.TotalAmtToPay.ToString();
		txtLateFeeAmt.Text = oFeeDetails.LateFee.ToString();
		txtConcessionAmt.Text = oFeeDetails.ConcessionAmt.ToString();
		hidLateFeeAmtPaid.Value = txtLateFeeAmt.Text;
		hidLateFeeRemark.Value = oFeeDetails.LateFeeRemark;
		cal_PaymentDate.DateValue = oFeeDetails.Paid_Date;
        hidPaymentDete.Value = oFeeDetails.Paid_Date.ToShortDateString();
		cal_CDate.DateValue = oFeeDetails.Cheque_Date;
		if (hidPaymentMode.Value == "By Card")
		{
			CardPaymentDetails oCardPaymentDetails = EditStudentPaidFeeBL.GetAllPaidFeeDetailsByCard(hidStudentId.Value.ToInt(), hidReciptNo.Value.ToInt(), miSchoolId, miAcademicYearId);
			txtSwapNumber.Text = oCardPaymentDetails.Swap_Number;
			cmbBankNameCard.SelectedValue = Convert.ToString(oCardPaymentDetails.Bank_Id);
			cmbCardType.SelectedValue = Convert.ToString(oCardPaymentDetails.CardTypeId);
		}        
	}

	/// <summary>
	/// Serializes the FinancialYearMaster entity object to a hidden field.
	/// </summary>
	private void SerializeFinancialYearDetails()
	{
		if (!IsAccountsModuleEnabled) return;

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
	/// Sets Javascript attributes for certain elements on the page.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnClose, btnPay, btnPayAndPrint });
		chkDirectlyPaid.Attributes["onclick"] = "javascript:EnableControlsDirectlyPaid();";
		valErrMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
	}

	/// <summary>
	/// Initializes all Dropdown lists on the page.
	/// </summary>
	private void FillAllCombos()
	{
		FillAllBankCombos();
		FillCardTypeCombo();
		FillFeeTypeCombo();
	}

	/// <summary>
	/// Fills the FeeType dropdown list.
	/// </summary>
	private void FillFeeTypeCombo()
	{
		int iStandardId = hidStandardId.Value.ToInt();
		cmbFeeType.Items.Add(new ListItem(Constants.S_SELECT, "0"));
		List<FeeTypeDetails> lstCardTypeDetails = EditStudentPaidFeeBL.GetFeeTypeDetails(miSchoolId, miAcademicYearId, iStandardId);
		lstCardTypeDetails.ForEach(ft => cmbFeeType.Items.Add(new ListItem(ft.Fee_Type, ft.SchoolWise_Standard_FeeType_Id.ToString())));
		cmbPayableFor.Items.Add(new ListItem(Constants.S_SELECT));
	}

	/// <summary>
	/// Populates the FeeDetails grid.
	/// </summary>
	private void FillFeeDetailsList()
	{
		List<FeeDetails> lstFeeDetails = EditStudentPaidFeeBL.GetAll(hidStudentId.Value.ToInt(), hidReciptNo.Value.ToInt(), cal_PaymentDate.DateValue, miSchoolId, miAcademicYearId);
		lstvStudentFeeDetails.DataSource = lstFeeDetails;
		lstvStudentFeeDetails.DataBind();
	}

	/// <summary>
	/// Fills the CardType dropdown list.
	/// </summary>
	private void FillCardTypeCombo()
	{
		cmbCardType.Items.Add(new ListItem(Constants.S_SELECT, "0"));
		List<CardTypeDetails> lstCardTypeDetails = SchoolwiseBankMasterBL.GetCardTypeDetails(miSchoolId);
		lstCardTypeDetails.ForEach(ct => cmbCardType.Items.Add(new ListItem(ct.CardType, ct.CardTypeId.ToString())));
	}

	/// <summary>
	/// Fills the Bank dropdown lists.
	/// </summary>
	private void FillAllBankCombos()
	{
		List<BankDetails> lstBankDetails = SchoolwiseBankMasterBL.GetBankDetails(miSchoolId);
		ListSource.FillDropDownList(lstBankDetails, cmbBankName, "Bank_Name", "Schoolwise_Bank_Id", Constants.S_SELECT);
		ListSource.FillDropDownList(lstBankDetails, cmbBankNameCard, "Bank_Name", "Schoolwise_Bank_Id", Constants.S_SELECT);

		if (IsAccountsModuleEnabled)
		{
            BankAccountClient oBankClient = null;
			try
			{
                oBankClient = new BankAccountClient();
				oBankClient.Open();
				List<BankAccount> lstLedgers = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
				ListSource.FillDropDownList(lstLedgers, ddlAcChqBank, "Name", "Id", Constants.S_SELECT);
				ListSource.FillDropDownList(lstLedgers, ddlAcCardBank, "Name", "Id", Constants.S_SELECT);
				ListSource.FillDropDownList(lstLedgers, cmbBankNameDirectlyPaid, "Name", "Id", Constants.S_SELECT);

				lblBankName.Text = "Deposit in Bank :";
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

		if (cmbBankNameDirectlyPaid.Items.Count != 0)
			return;
		
		ListSource.FillDropDownList(lstBankDetails, cmbBankNameDirectlyPaid, "Bank_Name", "Schoolwise_Bank_Id", Constants.S_SELECT);
		lblBankName.Text = "Bank Name :";
	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		if (lblErrorMsg.Text == String.Empty)
		{
			string sQueryString = "StudentId=" + hidStudentId.Value;
			string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
			sQueryString = "'?" + sEncryptQueryString + "'";
			Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");
			Response.Write("window.close();");
			Response.Write("</script>");
		}
	}

	/// <summary>
	/// Creates an XML for Cheque details.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForChequeDetails()
	{
		var oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("ChequeDetailsList");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ChequeDetailsList", String.Empty);

		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, String.Empty);

		string sAttrName = "DueDate";
		XmlAttribute oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtDate.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "ChequeNo";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtChequeNumber.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "BankId";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = cmbBankName.SelectedValue;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "ChequeRemarks";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtChequeRemarks.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "Is_PDC";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = Constants.C_NO.ToString();
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "ChequeAmt";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtActualAmt.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "DepositBankId";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = IsAccountsModuleEnabled ? ddlAcChqBank.SelectedValue : Constants.S_ZERO;
		oXmlNode.Attributes.Append(oXmlAttr);

		oXmlRootNode.AppendChild(oXmlNode);

		root.AppendChild(oXmlRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// Creates an XML for Card details.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForCardDetails()
	{
		var oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("CardDetailsList");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "CardDetailsList", String.Empty);

		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, String.Empty);

		string sAttrName = "SwapNo";
		XmlAttribute oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtSwapNumber.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "CardTypeId";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = cmbCardType.SelectedValue;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "BankId";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = cmbBankNameCard.SelectedValue;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "CardAmt";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = txtActualAmt.Text;
		oXmlNode.Attributes.Append(oXmlAttr);

		sAttrName = "DepositBankId";
		oXmlAttr = oDoc.CreateAttribute(sAttrName);
		oXmlAttr.Value = IsAccountsModuleEnabled ? ddlAcCardBank.SelectedValue : Constants.S_ZERO;
		oXmlNode.Attributes.Append(oXmlAttr);

		oXmlRootNode.AppendChild(oXmlNode);

		root.AppendChild(oXmlRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// Creates an XML for Credit details.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForCreditDetails()
	{
		var oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement("CreditDetailsList");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "CreditDetailsList", String.Empty);
		if (txtChequeDate.Text == String.Empty)
			txtChequeDate.Text = DateTime.Today.ToString();
		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, String.Empty);

		string sAtrrName1 = "ChequeDate";
		XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
		attr1.Value = txtChequeDate.Text;
		oXmlNode.Attributes.Append(attr1);

		string sAtrrName2 = "PayableFor";
		XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
		attr2.Value = cmbPayableFor.SelectedIndex > 0 ? cmbPayableFor.SelectedItem.Text : txtPayableFor.Text;
		oXmlNode.Attributes.Append(attr2);

		string sAtrrName3 = "FeeType";
		XmlAttribute attr3 = oDoc.CreateAttribute(sAtrrName3);
		attr3.Value = cmbFeeType.SelectedIndex > 0 ? cmbFeeType.SelectedItem.Text : txtFeeType.Text;
		oXmlNode.Attributes.Append(attr3);

		string sAtrrName4 = "Remarks";
		XmlAttribute attr4 = oDoc.CreateAttribute(sAtrrName4);
		attr4.Value = txtAddRemarks.Text;
		oXmlNode.Attributes.Append(attr4);

		string sAtrrName5 = "Std_Fee_Type_id";
		XmlAttribute attr5 = oDoc.CreateAttribute(sAtrrName5);
		attr5.Value = cmbFeeType.SelectedIndex > 0 ? cmbFeeType.SelectedValue : null;
		oXmlNode.Attributes.Append(attr5);

		oXmlRootNode.AppendChild(oXmlNode);

		root.AppendChild(oXmlRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// Creates an XML for Student Fee Details.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForStudentFeeIds()
	{
		const string S_STUDENT_FEE_ID = "Student_Fee_Id";
		const string S_CHECK_BOX_FEE = "chkSelect";
		const string S_STUDENT_FEE_LIST = "StudentFeeList";
		var oDoc = new XmlDocument();
		XmlElement root = oDoc.CreateElement(S_STUDENT_FEE_LIST);
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT_FEE_LIST, String.Empty);

		for (int iCnt = 0; iCnt < lstvStudentFeeDetails.Items.Count; iCnt++)
		{
			var chkFeeId = lstvStudentFeeDetails.Items[iCnt].FindControl(S_CHECK_BOX_FEE) as CheckBox;
			
			if (!chkFeeId.Checked)
				continue;
			
			int iStudentFeeId = lstvStudentFeeDetails.DataKeys[iCnt]["Schoolwise_Student_Fee_Id"].ToInt();
			XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, String.Empty);

			string sAtrrName = S_STUDENT_FEE_ID;
			XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = iStudentFeeId.ToString();
			oXmlNode.Attributes.Append(attr);
			oXmlRootNode.AppendChild(oXmlNode);
		}

		root.AppendChild(oXmlRootNode);
		return root.InnerXml;
	}

	/// <summary>
	/// Creates a ledger for a New Fee type specified at the time of fee payment.
	/// </summary>
	/// <param name="asFeeType"></param>
	private void CreateLedgerForNewFeeType(string asFeeType)
	{
		AccountLedgerClient oLedgerClient = null;
		try
		{
			oLedgerClient = new AccountLedgerClient();
			oLedgerClient.Open();
			oLedgerClient.CreateLedgerForNewFeeType(miSchoolId, miFinancialYearId, asFeeType, miUserId);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
													  String.Format("Accounts Module : An exception occured while creating a Ledger for new fee type : {0}", asFeeType));
		}
		finally
		{
			if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
				oLedgerClient.Close();
		}
	}

	/// <summary>
	/// Creates an XML string of the Fee payment particulars.
	/// </summary>
	/// <param name="aiStudentId"></param>
	/// <param name="asReceiptNo"></param>
	/// <returns></returns>
	private string GetFeeDetailsXML(int aiStudentId, string asReceiptNo)
	{
		string sStudentFeeIdsXML = String.Empty;
		AccountVoucherClient oVoucherClient = null;
		try
		{
			oVoucherClient = new AccountVoucherClient();
			oVoucherClient.Open();
			List<FeeVoucherParticulars> lstFeeParticulars = oVoucherClient.GetFeePaymentParticulars(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo);
			sStudentFeeIdsXML = CommonUtility.GetXMLForList(lstFeeParticulars);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
													  String.Format("Accounts Module : An exception occured while getting FeeVoucher particulars. StudentId : {0}. ReceiptNo : {1}",
																	aiStudentId,
																	asReceiptNo));
		}
		finally
		{
			if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
				oVoucherClient.Close();
		}
		return sStudentFeeIdsXML;
	}

	/// <summary>
	/// Gets the BankId for the specified Ledger.
	/// </summary>
	/// <param name="aiLedgerId"></param>
	/// <returns></returns>
	private int GetBankIdForLedger(int aiLedgerId)
	{
		int iBankId = aiLedgerId;
        BankAccountClient oBankClient = null;
		try
		{
            oBankClient = new BankAccountClient();
			oBankClient.Open();
			iBankId = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId).Find(bank => bank.Id == aiLedgerId).Bank.Id;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : An exception occured while fetching Bank Id for selected Bank.");
		}
		finally
		{
			if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
				oBankClient.Close();
		}
		return iBankId;
	}

	/// <summary>
	/// Deletes the payments details from FeeVoucher for the given StudentId and ReceiptNumber.
	/// </summary>
	/// <param name="aiStudentId"></param>
	/// <param name="asReceiptNo"></param>
	/// <param name="asStudentFeeIdsXML"></param>
	private void DeleteFeeVoucher(int aiStudentId, string asReceiptNo, string asStudentFeeIdsXML)
	{
		// If a fee is edited and then saved, the old payment details for the given ReceiptNumber are all deleted and then the new payment details are recorded.
		// It is possible that fee payment is already recorded in the Accounts module. Hence we need to delete those details and then save the new ones.
		// So now we delete the payment details for the given student id and receipt number from the accounts books.
		// If the payment is not recorded already, the following action will do nothing.
		AccountVoucherClient oVoucherClient = null;
		try
		{
			oVoucherClient = new AccountVoucherClient();
			oVoucherClient.Open();
			oVoucherClient.DeleteFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo, asStudentFeeIdsXML, miUserId,true);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
														String.Format("Account Module : An exception occured while deleting fee details at the time of fee editing. Student Id : {0}, ReceiptNo : {1}",
																		aiStudentId,
																		asReceiptNo));
		}
		finally
		{
			if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
				oVoucherClient.Close();
		}
	}

	/// <summary>
	/// Records the Fee payment in the Accounts module.
	/// </summary>
	/// <param name="aiStudentId"></param>
	/// <param name="asReceiptNo"></param>
	private void RecordCashPayment(int aiStudentId, string asReceiptNo)
	{
		// Create a fee voucher for the fees paid by the student
		AccountVoucherClient oVoucherClient = null;
		try
		{
			oVoucherClient = new AccountVoucherClient();
			oVoucherClient.Open();
			oVoucherClient.CreateFeeVoucherForCashPayment(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo, miUserId);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
													  String.Format("Accounts Module : An exception occured while recording a fee payment. StudentId : {0}. ReceiptNo : {1}",
																	aiStudentId,
																	asReceiptNo));
		}
		finally
		{
			if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
				oVoucherClient.Close();
		}
	}

	#endregion -- PRIVATE METHOD(s) --

}