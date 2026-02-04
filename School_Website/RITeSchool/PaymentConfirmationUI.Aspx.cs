/* File Name :- NetBankingUI.aspx.cs
 * Created By :- shankar
 * Created Date :- 12-Nov-2009
 * Class Description :- This Class is used to display Amount to pay and allow user to select bank for net banking transaction.
 */
/* Modified By : Pravin
 * Date        : 5 Jun 2013
 * Purpose     : To make the online transaction using a single entry in the database.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using COM;
using FeeEntities;
using SchoolEntities.Accounts;
using Utility;
using System.Web.UI.HtmlControls;
using System.Net;
using SchoolEntities;
using System.Collections.Specialized;
using CCA.Util;

public partial class PaymentConfirmationUI : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_TOTAL_FEES = "TotalFees";
	private const string S_BANK_ID = "BankId";
	private const string S_DEFAULT_DATE_2 = "01/01/1900 12:00:00 AM";
	private const string S_DEFAULT_DATE_3 = "1/1/1900 12:00:00 AM";
	private const string S_DEFAULT_DATE_4 = "01-Jan-1900";
	private const string S_DEFAULT_DATE_5 = "01-Jan-1900 12:00 AM";	
	private const string S_PROCESSING_ERROR = "Error occurred while processing your transaction. Please try again.";
	private const string S_ADMISSION = "Admission";
    private const string S_CAUTION_MONEY = "CautionMoney";
    private const string S_INTERNAL_FEE = "InternalFee";
	private const string S_SCHOOWISE_STUDENT_FEE_ID = "Schoolwise_Student_Fee_Id";
	private const string S_LATEFEEREMARKS = "LateFeeRemarks";
	private const string S_LATEFEEAMOUNT = "LateFeeAmount";
	private const string S_DUEDATES = "DueDates";
	private const string S_STUDENTFEE="StudentFee";
	private const string S_FORMFEE="Form Fee :";
	private const string S_FORMFEECOLON="Student Fee :";    
    private const string S_ORIGIONAL_AMOUNT = "OrigionalAmount";    
    private const string S_FORM_NUMBER = "Form_Number";
    private const string CONCESSION_AMOUNT = "Concession Amount";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private decimal miTotalFee;
	private List<BlockedBank> mlstBlockedBanks;
	private string msAmount = string.Empty;
    private int miConcessionAmount = 0;
    private const string S_ERROR_FORM_FEE = "%FEETYPE% should be greater than zero.";
    private const string S_ERROR_TOTAL_FEE = "Total Fee should be greater than or equal to %FEETYPE%.";
    private const string S_ERROR_EMPTY_FEE = "%FEETYPE% and Total Fee amounts should not be empty.";
    private const string S_ERROR_AMOUNT_PAYABLE = "Payable Amount mismatched with total amount, you can't proceed further!!!";
    private const string S_PAYABLE_AMOUNT_MESSAGE = "%INCLUDE% processing charge & GST.";
    private int miGatewayId;
    private bool mbHasBankSelection;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	/// This property is used to expose bank name to the next page.
	/// </summary>
	public string BankName
	{
		get
		{
			return cmbBanks.SelectedItem.Text;
		}
	}

	/// <summary>
	/// This property is used to check the user roll other than student
	/// </summary>
	private bool IsOnlineAdmissionFee
	{
        get { return moUserRole != Constants.UserRoles.Student || (moUserRole == Constants.UserRoles.Student && hidTransactionFrom.Value == S_ADMISSION); }
	}

    private bool IsOnlineInternalFee
    {
        get { return hidTransactionFrom.Value == Constants.OnlineFeeTypes.InternalFee.ToString(); }
    }

    /// <summary>
    /// Following control properties are decalred to access the controls in inherited class to which we are setting caluclated values.
    /// </summary>
    public TextBox TxtFormFees
    {
        get { return txtFormFees; }
        set { txtFormFees = value; }
    }

    public Label LblAmtPerTransaction
    {
        get { return txtAmtPerTransaction; }
        set { txtAmtPerTransaction = value; }
    }

    public HtmlTableRow TrServiceTax
    {
        get { return trServiceTax; }
        set { trServiceTax = value; }
    }

    public HtmlTableCell TrProcessChrges
    {
        get { return trProcessChrges; }
        set { trProcessChrges = value; }
    }

    public bool AllowMultipleGateway
    {
        get
        {
            return false;
            //int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
            //return iSchoolId == Constants.SchoolId.PPSN.ToInt();
        }
    }

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This method is used to handle a page load event and intitalize the page with appropriate maste page.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to handle a page load event and initalize the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			ReadQueryString();
			Response.Buffer = true;
			Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
			Response.Expires = -1;
			Response.Cache.AppendCacheExtension("max-age=0, no-store, must-revalidate");
			if (!IsPostBack)
			{
				valSumNetBankingFees.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                FillPaymentTypes();
                SetGateway();
                SetPaymentTypeState();
				FillBankDropDown();
				SerializeBlocksBanks();
                SetFieldState();
                if (hidTransactionFrom.Value == S_STUDENTFEE || hidTransactionFrom.Value == S_CAUTION_MONEY || hidTransactionFrom.Value == S_INTERNAL_FEE)
				{
					lblForm.Text = S_FORMFEECOLON;
					txtFormFees.Text = miTotalFee.ToString();
					btnProceed.CssClass = "ClsBtnMid";
					//calling base class method
					//ApplyMouseHoverEffect(new List<Button> { btnProceed });
				}
				else
				{
					lblForm.Text = S_FORMFEE;
					txtFormFees.Text = msAmount;
				}

				if (hidTransactionFrom.Value == S_ADMISSION && (msAmount.IsNullOrEmpty() || msAmount == Constants.S_ZERO))
					Response.Redirect("~/RITeSchool/Admission/OnlineAdmissionUI.aspx", false);
                //if (moUserRole == Constants.UserRoles.Student && hidTransactionFrom.Value != S_ADMISSION)
                //    SubmissionWizardSteps.ActiveSteps = 2;                   
                //else
                    SubmissionWizardSteps.ActiveSteps = 1;
			}
            if (moUserRole == Constants.UserRoles.Student && hidTransactionFrom.Value != S_ADMISSION)
				SubmissionWizardSteps.IsStudentFee = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    private void SetFieldState()
    {
        int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        if (miGatewayId == Constants.PaymentGateways.Billdesk.ToInt() || miGatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt())
        {   
            if (iSchoolId == Constants.SchoolId.SNS.ToInt())
                trServiceTax.Visible = false;
        }

        if (iSchoolId == Constants.SchoolId.BFS.ToInt() || (miSchoolId == 0 && iSchoolId == Constants.SchoolId.PPSN.ToInt()))
            trServiceTax.Visible = false;

        //if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
        //{
        //    trBankList.Visible = true;
        //    hlnkBankDetails.Attributes.Add("onclick", string.Format("window.open('{0}', '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=680'); return false;", hlnkBankDetails.NavigateUrl));
        //}
    }

    /// <summary>
    /// This method is used to handle the selected index change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoPaymentTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.Validate();
            
            // Exit function if the validation failed.            
            lblErrorMessage.Text = string.Empty;
            lblErrorMessage.Visible = false;
            cmbBanks.SelectedValue = Constants.S_ZERO;

            if (rdoPaymentTypes.SelectedItem.Text == "Bank Payment")
            {
                lblbankName.InnerText = "Select Bank :";
                //Session[Constants.S_TRANSACTION_PAYMENT_METHOD] = "NB";
            }
            else
            {
                lblbankName.InnerText = "Select Card :";
                //Session[Constants.S_TRANSACTION_PAYMENT_METHOD] = "CD";
            }

            SetGateway();
            FillBankDropDown();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

	/// <summary>
	/// This method is used to handle a bank drop down and selected event change.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbBanks_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			this.Validate();
			// Exit function if the validation failed.
			if (!IsValid)
				return;            
		    
            lblErrorMessage.Text = string.Empty;
		    lblErrorMessage.Visible = false;
			int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
			var oFeeType = Constants.OnlineFeeTypes.AdmissionFee;
            if (hidTransactionFrom.Value == Constants.OnlineFeeTypes.StudentFee.ToString() || hidTransactionFrom.Value == Constants.OnlineFeeTypes.CautionMoney.ToString() || hidTransactionFrom.Value == Constants.OnlineFeeTypes.InternalFee.ToString())
				oFeeType = Constants.OnlineFeeTypes.StudentFee;

            SetGateway();
            DataRow[] oDtRowNetBankingBanks = SchoolwiseBankMasterCollectionBL.GetNetBankingBanksList(iSchoolId, oFeeType.ToInt(), rdoPaymentTypes.SelectedValue.ToInt(),miGatewayId)
																			  .Select("RegisteredBankID=" + "'"+cmbBanks.SelectedValue+"'");
			
			// Exit function if the bank selected by the user is not found.
			if (oDtRowNetBankingBanks.Length <= 0)
				return;
			// DataRow representing the bank selected by the user.
			DataRow dtRow = oDtRowNetBankingBanks[0];
			// If the DataRow repsenting the selected bank is null, Exit the function.
			if (dtRow == null)
				return;
			decimal dFormFees=0;
			decimal dDeductedAmount=0;
			decimal dRound = 0.00499999M;
			// Set service tax info.
			decimal dServiceTaxAmount = Decimal.Round(dtRow["ServiceTaxInPercent"].ToDecimal() + dRound, 2);
			txtServiceTax.Text = Convert.ToString(dServiceTaxAmount) + "%";

            decimal dAmount;
            if (oFeeType == Constants.OnlineFeeTypes.StudentFee || oFeeType == Constants.OnlineFeeTypes.CautionMoney || oFeeType == Constants.OnlineFeeTypes.InternalFee)
                dAmount = miTotalFee;
            else
                dAmount = msAmount.ToDecimal();

			// When the current transaction is for School Fees.         

            if (miGatewayId == Constants.PaymentGateways.TPSL.ToInt())
                {
                    TPSLPGRequestData oTPSLPGRequestData = new TPSLPGRequestData(dAmount, dServiceTaxAmount);
                    if (oFeeType == Constants.OnlineFeeTypes.StudentFee || oFeeType == Constants.OnlineFeeTypes.CautionMoney)
                        oTPSLPGRequestData.CalculateStudentFeeAmounts(dtRow, true, out  dDeductedAmount, out  dFormFees,this);
			        // When the current transaction is for Online Admission fees.
			        else
                        oTPSLPGRequestData.CalculateStudentsAdmissionAmount(dtRow, false, out  dDeductedAmount, out  dFormFees,this);
                }
            else if (miGatewayId == Constants.PaymentGateways.AxisBank.ToInt())
                {
                    AXISPGRequest oAXISPGRequest = new AXISPGRequest();
                    oAXISPGRequest.CalculateFeeAmount(dtRow,out dDeductedAmount, out dFormFees, dServiceTaxAmount,dAmount,this);                    
                }
            else if (miGatewayId == Constants.PaymentGateways.PayU.ToInt())
                {
                    PayUPGRequest oPayUPGRequest = new PayUPGRequest();
                    oPayUPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this); 
                }
			else if(miGatewayId == Constants.PaymentGateways.Atom.ToInt())
            {
                AtomPGRequest oAtomPGRequest = new AtomPGRequest();
                oAtomPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.PayUMoney.ToInt())
            {
                PayUMoneyPGRequest oPayUMoneyPGRequest = new PayUMoneyPGRequest();
                oPayUMoneyPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt())
            {
                AxisBankForAllPGRequest oAxixBankForAllPGRequest = new AxisBankForAllPGRequest();
                oAxixBankForAllPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.EaseBuzz.ToInt())
            {
                EaseBuzzPGRequest oEaseBuzzPGRequest = new EaseBuzzPGRequest();
                oEaseBuzzPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.Billdesk.ToInt())
            {
                BilldeskPGRequest oBilldeskPGRequest = new BilldeskPGRequest();
                oBilldeskPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt())
            {
                BilldeskDYPPGRequest oBilldeskDYPPGRequest = new BilldeskDYPPGRequest();
                oBilldeskDYPPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.CCAvenue.ToInt())
            {
                CCAvenuePGRequest oCCAvenuePGRequest = new CCAvenuePGRequest();
                oCCAvenuePGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }
            else if (miGatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt())
            {
                CCAvenueVPMCPSPGRequest oCCAvenueVPMCPSPGRequest = new CCAvenueVPMCPSPGRequest();
                oCCAvenueVPMCPSPGRequest.CalculateFeeAmount(dtRow, out dDeductedAmount, out dFormFees, dServiceTaxAmount, dAmount, this);
            }            
			txtTotalFees.Text = (dFormFees + Decimal.Round(dDeductedAmount + dRound, 2)).ToString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to proceed to make visible conformation controles
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnProceed_Click(object sender, EventArgs e)
	{
		try
		{
            //if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
            //    trDebitCardNote.Visible = true;
            //else
            //    trDebitCardNote.Visible = false;

            if(((txtFormFees.Text.IsNullOrEmpty() || txtTotalFees.Text.IsNullOrEmpty()) && cmbBanks.Visible) || txtFormFees.Text.IsNullOrEmpty())
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = S_ERROR_EMPTY_FEE.Replace("%FEETYPE%", lblForm.Text.Replace(":", string.Empty).Trim());
                return;
            }
		    if (txtFormFees.Text.ToDecimal() == Constants.I_ZERO.ToDecimal())
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = S_ERROR_FORM_FEE.Replace("%FEETYPE%", lblForm.Text.Replace(":", string.Empty).Trim());
                return;
            }
            else if (cmbBanks.Visible && txtTotalFees.Text.ToDecimal() < ViewState[S_ORIGIONAL_AMOUNT].ToDecimal())
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = S_ERROR_TOTAL_FEE.Replace("%FEETYPE%", lblForm.Text.Replace(":", string.Empty).Trim());
                return;
            }
            else
                lblErrorMessage.Visible = false;
            ReadPaymentData();
			divNetBanking.Visible = false;            
			divConformation.Visible = true;			
			if (!IsOnlineAdmissionFee)
			{
				btnConfirm.CssClass = "ClsBtnMid";
				btnConfirm.Attributes.Add("Onclick", "DisableButtons()");
			}
			else
			{
				SubmissionWizardSteps.IsStudentFee = false;
				SubmissionWizardSteps.ActiveSteps = 4;
				SubmissionWizardSteps.SetImagesForActiveStep();
			}

            btnConfirm_Click(btnConfirm, null);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to  conform the payment
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnConfirm_Click(object sender, EventArgs e)
	{
		try
		{
			// If its an fee payment for Online Admission, fetch the number of forms remaining.
            if(lblAmountPay.Text.IsNullOrEmpty() || lblAmountPay.Text.ToDecimal() == Constants.I_ZERO.ToDecimal() || (lblAmountPay.Text.ToDecimal() != ViewState[S_ORIGIONAL_AMOUNT].ToDecimal()))
            {
                lblConfirmMsg.Visible = true;
                lblConfirmMsg.Text = S_ERROR_AMOUNT_PAYABLE;
                btnConfirm.Enabled = false;
                return;
            }

		    int iRemainingFormCount = Session["RemainingformsCount"].ToInt();
			// If the number of forms remaining are less than or equal to zero, we show an error message.
			// If forms are unlimited, iRemainingFormCount will be -1, in which case we will still proceed.
			if (IsOnlineAdmissionFee && iRemainingFormCount <= 0 && iRemainingFormCount != -1)
			{
				btnConfirm.Enabled = false;
				lblErrorMsg.Visible = true;
				lblErrorMsg.Text = "Online admission Forms are closed.";
				SubmissionWizardSteps.ActiveSteps = 4;
			}
			else
			{
				string sBankId = cmbBanks.SelectedValue;
				string sPayAmt = lblAmountPay.Text;

                string sFeeIds = GetStudentFeeIds();
                SetGateway();

                int iTransactionID = ConfirmTransaction(sBankId, sPayAmt, miConcessionAmount);
                Session["TransactionId"] = iTransactionID.ToString();

                string sReturnURL = HttpContext.Current.Request.Url.AbsoluteUri;
                if (HttpContext.Current.Request.Url.Query.Length > 0)
                    sReturnURL = sReturnURL.Replace(HttpContext.Current.Request.Url.Query, string.Empty);
                if (sReturnURL != string.Empty)
                    sReturnURL = sReturnURL.Substring(0, sReturnURL.LastIndexOf("/") + 1);

                int iScStudentId = 0, iNxtAcYearId = 0;

                if (hidScStudentId.Value.Trim() != string.Empty)
                    iScStudentId = hidScStudentId.Value.ToInt();

                if (hidNxtAcYearId.Value.Trim() != string.Empty)
                    iNxtAcYearId = hidNxtAcYearId.Value.ToInt();

                List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(sFeeIds, iScStudentId, hidSelectedFeeTypes.Value, iNxtAcYearId, IsOnlineInternalFee);
                if (miGatewayId == Constants.PaymentGateways.TPSL.ToInt())
                {
                    // This code will execute for TPSL gateway.
                    TPSLPGRequestData oTPSLPGRequestData = new TPSLPGRequestData(sReturnURL);
                    string strMsg = oTPSLPGRequestData.GetQueryString(IsOnlineAdmissionFee,sPayAmt, iTransactionID, sBankId);
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.TPSL);
                    AddMessageToRequest("TransactionRequestMessage", strMsg);
                    if (!strMsg.IsNullOrEmpty())
                    {
                        string netBankingRequestURL = ConfigurationManager.AppSettings["NetBankingRequestURL"];
                        Response.Redirect(netBankingRequestURL + "?msg=" + strMsg, false);
                    }
                    else
                        throw new ApplicationException("Error occured while processing your transaction. Please try again.");
                }
                else if (miGatewayId == Constants.PaymentGateways.AxisBank.ToInt())
                {
                    // This code will execute for Axis bank gateway.
                    AXISPGRequest oAXISPGRequest = new AXISPGRequest(sReturnURL);
                    PaymentGateWayDetails oPaymentGateWayDetails= lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBank.ToInt()).FirstOrDefault();
                    string sQueryString = oAXISPGRequest.PrepareQueryString(sPayAmt, iTransactionID, sBankId, oPaymentGateWayDetails);
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.AxisBank);
                    if (!sQueryString.IsNullOrEmpty())
                        Response.Redirect(sQueryString, false);
                    else
                        throw new ApplicationException("Error occured while processing your transaction. Please try again.");
                }
                else if (miGatewayId == Constants.PaymentGateways.PayU.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.PayU);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);
                    
                    PayUPGRequest oPayUPGRequest = new PayUPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.PayU.ToInt()).FirstOrDefault();

                    bool bUseAlternate = false;
                    if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Utility.Constants.SchoolId.PPS.ToInt())
                        bUseAlternate = true;

                    Hashtable oHashtable = oPayUPGRequest.PrepareTransactionData(sPayAmt, iTransactionID, sBankId, oPaymentGateWayDetails, bUseAlternate);

                    RedirectAndPOST(this.Page, oPaymentGateWayDetails.NetBankingUrl, oHashtable);                    
                }
                else if (miGatewayId == Constants.PaymentGateways.Atom.ToInt())
                {
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.Atom);

                    bool bIsNextYearFeePayment = false;
                    if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == "Y")
                        bIsNextYearFeePayment = true;


                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true, 0, bIsNextYearFeePayment);

                    AtomPGRequest oAtomPGRequest = new AtomPGRequest(sReturnURL, oStudentNetBankingDetails);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.Atom.ToInt()).FirstOrDefault();
                    oAtomPGRequest.SendPostRequest(Response, oPaymentGateWayDetails.NetBankingUrl, sPayAmt, iTransactionID);
                }
                else if (miGatewayId == Constants.PaymentGateways.PayUMoney.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.PayUMoney);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = string.Empty;
                        //oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;
                    

                    PayUMoneyPGRequest oPayUMoneyPGRequest = new PayUMoneyPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.PayUMoney.ToInt()).FirstOrDefault();
                    Hashtable oHashtable = oPayUMoneyPGRequest.SendRequest(sPayAmt, iTransactionID, oPaymentGateWayDetails);
                   
                    RedirectAndPOST(this.Page, oPaymentGateWayDetails.NetBankingUrl, oHashtable);
                }
                else if (miGatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.AxisBankForAll);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    //string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");

                    string sPaymentFor = string.Empty;
                    if (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        sPaymentFor = "Admission";
                    else if (Session[Constants.S_TRANSACTION_FROM].ToString() == S_CAUTION_MONEY)
                        sPaymentFor = S_CAUTION_MONEY;
                    else if (Session[Constants.S_TRANSACTION_FROM].ToString() == S_INTERNAL_FEE)
                        sPaymentFor = S_INTERNAL_FEE;
                    else
                        sPaymentFor = "StudentFee";

                    AxisBankForAllPGRequest oAxixBankForAllPGRequest = new AxisBankForAllPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()).FirstOrDefault();

                    Hashtable oHashtable = oAxixBankForAllPGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor);
                    RedirectAndPOST(this.Page, oPaymentGateWayDetails.NetBankingUrl, oHashtable);
                }
                else if (miGatewayId == Constants.PaymentGateways.EaseBuzz.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.EaseBuzz);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    EaseBuzzPGRequest oEaseBuzzPGRequest = new EaseBuzzPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.EaseBuzz.ToInt()).FirstOrDefault();

                    Hashtable oHashtable = oEaseBuzzPGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor);
                    RedirectAndPOST(this.Page, oPaymentGateWayDetails.NetBankingUrl + "/pay/secure", oHashtable);
                }
                else if (miGatewayId == Constants.PaymentGateways.Billdesk.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.Billdesk);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    BilldeskPGRequest oBilldeskPGRequest = new BilldeskPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.Billdesk.ToInt()).FirstOrDefault();

                    string sParameters = oBilldeskPGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor);
                    hidParameters.Value = sParameters;

                    //hidRequestUrl.Value = "http://dummyPG.riteschool.com/Default2.aspx" + "?msg=" + sParameters; 
                    
                    hidRequestUrl.Value = oPaymentGateWayDetails.NetBankingUrl + "?msg=" + sParameters;                    
                }
                else if (miGatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.BilldeskDYP);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    BilldeskDYPPGRequest oBilldeskDYPPGRequest = new BilldeskDYPPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt()).FirstOrDefault();

                    hidParameters.Value = oBilldeskDYPPGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor);
                    
                    PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
                    List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.BilldeskDYP);
                    hidRequestUrl.Value = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ReturnURL").FirstOrDefault().Value;
                }
                else if (miGatewayId == Constants.PaymentGateways.CCAvenue.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.CCAvenue);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    CCAvenuePGRequest CCAvenuePGRequest = new CCAvenuePGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenue.ToInt()).FirstOrDefault();

                    hidAccessCode.Value = oPaymentGateWayDetails.AccessCode;
                    hidRequestUrl.Value = oPaymentGateWayDetails.NetBankingUrl;

                    string sGuid = string.Empty;
                    if (Session[Constants.S_SESSION_PAYMENT_RECORD] != null)
                    {
                        Dictionary<string, string> dict = Session[Constants.S_SESSION_PAYMENT_RECORD] as Dictionary<string, string>;
                        sPayAmt = dict["TxnAmt"].ToString();
                        sGuid = dict["TxnGuid"].ToString();
                    }

                    hidParameters.Value = CCAvenuePGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor, sGuid);                    
                }
                else if (miGatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.CCAvenueVPMCPS);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    CCAvenueVPMCPSPGRequest oCCAvenueVPMCPSPGRequest = new CCAvenueVPMCPSPGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone, oStudentNetBankingDetails.RegNoOrFormNo);
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt()).FirstOrDefault();

                    hidAccessCode.Value = oPaymentGateWayDetails.AccessCode;
                    hidRequestUrl.Value = oPaymentGateWayDetails.NetBankingUrl;

                    string sGuid = string.Empty;
                    if (Session[Constants.S_SESSION_PAYMENT_RECORD] != null)
                    {
                        Dictionary<string, string> dict = Session[Constants.S_SESSION_PAYMENT_RECORD] as Dictionary<string, string>;
                        sPayAmt = dict["TxnAmt"].ToString();
                        sGuid = dict["TxnGuid"].ToString();
                    }

                    hidParameters.Value = oCCAvenueVPMCPSPGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor, sGuid);
                }
                else if (miGatewayId == Constants.PaymentGateways.RazorPay.ToInt())
                {
                    // This code will execute for PayU gateway.
                    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.RazorPay);
                    StudentNetBankingDetails oStudentNetBankingDetails;
                    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                    // Here we colllect the student details required to send it to gateway.
                    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                    else
                        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()).FirstOrDefault();

                    string sGuid = string.Empty;
                    if (Session[Constants.S_SESSION_PAYMENT_RECORD] != null)
                    {
                        Dictionary<string, string> dict = Session[Constants.S_SESSION_PAYMENT_RECORD] as Dictionary<string, string>;
                        sPayAmt = dict["TxnAmt"].ToString();
                        sGuid = dict["TxnGuid"].ToString();
                    }

                    string sTransactionFrom = Session[Constants.S_TRANSACTION_FROM].ToString().ToUpper();
                    int iTransactionFor;
                    if (sTransactionFrom == "ADMISSION")
                        iTransactionFor = 1;
                    else if (sTransactionFrom == "CAUTIONMONEY")
                        iTransactionFor = 3;
                    else if (sTransactionFrom == "INTERNALFEE")
                        iTransactionFor = 4;
                    else
                        iTransactionFor = 2;

                    int iAcademicYearId = 0;
                    if (Session["FinalAcademicYearId"] != null)
                        iAcademicYearId = Session["FinalAcademicYearId"].ToInt();
                    else
                        iAcademicYearId = miAcademicYearId;

                    int iStudentId = 0;
                    if (Session["FinalYearStudentId"] != null)
                        iStudentId = Session["FinalYearStudentId"].ToInt();
                    else
                        iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();

                    bool bIsForNextYear = false;
                    if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == Constants.S_YES)
                        bIsForNextYear = true;
                    else
                        bIsForNextYear = false;

                    bool bIsOldAcademicYearPayment = false;
                    if (Session["IsOldAcademicYearPayment"] != null && Session["IsOldAcademicYearPayment"].ToString() == Constants.S_ONE)
                        bIsOldAcademicYearPayment = true;

                    string sQueryString = "Amount=" + sPayAmt + "&TxnId=" + iTransactionID + "&Name=" + oStudentNetBankingDetails.FirstName + "&Email=" + oStudentNetBankingDetails.Email + "&ContactNo=" + oStudentNetBankingDetails.Phone + "&TransactionFor=" + iTransactionFor + "&AcademicYearId=" + iAcademicYearId
                        + "&StudentId=" + iStudentId + "&IsForNextYear=" + bIsForNextYear + "&IsOldAcademicYearPayment=" + bIsOldAcademicYearPayment + "&StudentUserId=" + miUserId + "&SchoolId=" + ConfigurationManager.AppSettings["SchoolID"];

                    PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
                    List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.RazorPay);
                    string sGatewayProcessingWebsite = lstGatewayAdditionalDetails.Where(gt => gt.Name.ToUpper() == "GATEWAY_PROCESSING_WEBSITE").FirstOrDefault().Value;

                    Response.Redirect(sGatewayProcessingWebsite+"?" + CommonUtility.EncryptQuerystring(sQueryString), false);
                }
                //else if (miGatewayId == Constants.PaymentGateways.PhiCommerce.ToInt())
                //{
                //    // This code will execute for PayU gateway.
                //    Session.Add(Constants.S_GATEWAY, Constants.PaymentGateways.PhiCommerce);
                //    StudentNetBankingDetails oStudentNetBankingDetails;
                //    NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();

                //    // Here we colllect the student details required to send it to gateway.
                //    if (!ViewState[S_FORM_NUMBER].IsNull() && ViewState[S_FORM_NUMBER].ToString() != Constants.S_ZERO && Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                //        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(ViewState[S_FORM_NUMBER].ToString(), false, Convert.ToInt32(QueryString["StandardId"]));
                //    else
                //        oStudentNetBankingDetails = oNetBankingPaymentTransactionsBL.GetStudentNetBankingDetails(Session[Constants.S_SESSION_STUDENT_ID].ToString(), true);

                //    if (oStudentNetBankingDetails.Email.Trim() == string.Empty)
                //        oStudentNetBankingDetails.Email = oStudentNetBankingDetails.SchoolEmailAddress;

                //    string sPaymentFor = (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION ? "Admission" : "StudentFee");
                //    PhiCommercePGRequest oPhiCommercePGRequest = new PhiCommercePGRequest(sReturnURL, oStudentNetBankingDetails.FirstName, oStudentNetBankingDetails.Email, oStudentNetBankingDetails.Phone);
                //    PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.PhiCommerce.ToInt()).FirstOrDefault();
                //    Hashtable oHashtable = oPhiCommercePGRequest.SendRequest(sPayAmt, iTransactionID.ToString(), oPaymentGateWayDetails, sPaymentFor);
                //    RedirectAndPOST(this.Page, oPaymentGateWayDetails.NetBankingUrl, oHashtable);
                //}
            }
		}
		catch (ApplicationException ae)
		{
			btnConfirm.Visible = false;
			lblErrorMsg.Visible = true;
			lblErrorMsg.Font.Bold = false;
			lblErrorMsg.Text = ae.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}   

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {   
        string sIds = Constants.S_ZERO;
        if (Settings.EnablePartialFeePaymentForStudentLogin)
        {
            var oStudentFeeIds = new List<string>();
            if (hidSchoolwiseStudentFeeIds.Value != string.Empty)
            {
                string sStudentFeeId = string.Empty;
                string[] StudentFeeDetails = hidSchoolwiseStudentFeeIds.Value.Split(',');
                foreach (string val in StudentFeeDetails)
                {
                    List<string> lstFeeDetails = val.Split('$').ToList();
                    int iStudentFeeId = lstFeeDetails[0].ToInt();
                    sStudentFeeId = sStudentFeeId + "," + iStudentFeeId;
                }

                if (sStudentFeeId.StartsWith(","))
                    sStudentFeeId = sStudentFeeId.Substring(1);

                List<string> SchoolwiseStudentFeeIds = sStudentFeeId.Split(',').ToList();
                Session[S_SCHOOWISE_STUDENT_FEE_ID] = SchoolwiseStudentFeeIds;
            }

            if (Session[S_SCHOOWISE_STUDENT_FEE_ID] != null)
                oStudentFeeIds = Session[S_SCHOOWISE_STUDENT_FEE_ID] as List<string>;

            if (oStudentFeeIds.Count > 0)
            {
                sIds = string.Join(",", oStudentFeeIds);
                if (sIds.StartsWith(","))
                    sIds = sIds.Substring(1);
            }
        }
        else
        {
            var iStudentFeeIds = new List<int>();
            if (Session[S_SCHOOWISE_STUDENT_FEE_ID] != null)
                iStudentFeeIds = Session[S_SCHOOWISE_STUDENT_FEE_ID] as List<int>;

            if (iStudentFeeIds.Count > 0)
            {
                sIds = string.Join(",", iStudentFeeIds);
                if (sIds.StartsWith(","))
                    sIds = sIds.Substring(1);
            }
        }
        return sIds;
    }

    /// <summary>
    /// This function is used to set the gateway id applicable for current school and bank selection.
    /// </summary>
    private void SetGateway()
    {
        string sFeeIds = GetStudentFeeIds();

        int iScStudentId = 0, iNxtAcYearId = 0;

        if (hidScStudentId.Value.Trim() != string.Empty)
            iScStudentId = hidScStudentId.Value.ToInt();

        if (hidNxtAcYearId.Value.Trim() != string.Empty)
            iNxtAcYearId = hidNxtAcYearId.Value.ToInt();

        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(sFeeIds, iScStudentId, hidSelectedFeeTypes.Value, iNxtAcYearId, IsOnlineInternalFee);
        PaymentGateWayDetails oPaymentGateWayDetails=new PaymentGateWayDetails();

        if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.TPSL.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.TPSL.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.TPSL.ToInt()).FirstOrDefault();         
        }
        else if ((lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.AxisBank.ToInt())) && rdoPaymentTypes.SelectedValue.ToInt() == Constants.PaymentMethod.CardPayment.ToInt())
        {
            miGatewayId = Constants.PaymentGateways.AxisBank.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBank.ToInt()).FirstOrDefault();         
        }
        else if ((lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.PayU.ToInt())) && rdoPaymentTypes.SelectedValue.ToInt() == Constants.PaymentMethod.BankPayment.ToInt())
        {
            miGatewayId = Constants.PaymentGateways.PayU.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.PayU.ToInt()).FirstOrDefault();         
        }
        else if ((lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.Atom.ToInt())) && rdoPaymentTypes.SelectedValue.ToInt() == Constants.PaymentMethod.BankPayment.ToInt())
        {
            miGatewayId = Constants.PaymentGateways.Atom.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.Atom.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.PayUMoney.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.PayUMoney.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.PayUMoney.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()) && !AllowMultipleGateway)
        //else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.AxisBankForAll.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.EaseBuzz.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.EaseBuzz.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.EaseBuzz.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.Billdesk.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.Billdesk.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.Billdesk.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.BilldeskDYP.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.CCAvenue.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.CCAvenue.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenue.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt()))
        {
            miGatewayId = Constants.PaymentGateways.CCAvenueVPMCPS.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()) && !AllowMultipleGateway)
        {
            miGatewayId = Constants.PaymentGateways.RazorPay.ToInt();
            oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()).FirstOrDefault();
        }
        else if (lstPaymentGateWayDetails.Exists(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()) && lstPaymentGateWayDetails.Exists(gtw => gtw.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()) && AllowMultipleGateway)
        {
            if (rdoPaymentTypes.SelectedValue == Constants.S_ONE)
            {
                miGatewayId = Constants.PaymentGateways.AxisBankForAll.ToInt();
                oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()).FirstOrDefault();
                hidTerms.Value = CommonUtility.EncryptQuerystring("GatewayName=Axis");
            }
            else
            {
                miGatewayId = Constants.PaymentGateways.RazorPay.ToInt();
                oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()).FirstOrDefault();
                hidTerms.Value = CommonUtility.EncryptQuerystring("GatewayName=RazorPay");
            }
        }

        hidGatewayId.Value = miGatewayId.ToString();
        mbHasBankSelection = oPaymentGateWayDetails.HasBankSelection;
        VisibleHideFields(mbHasBankSelection);
    }

    /// <summary>
    /// This method is used to show and hide bank selection, charges controls.
    /// </summary>
    /// <param name="abFlag"></param>
    private void VisibleHideFields(bool abFlag)
    {
        if (abFlag && cmbBanks.Items.Count == Constants.I_TWO)
            trBankSelection.Visible = false;
        else if (abFlag && cmbBanks.Items.Count > Constants.I_TWO)
            trBankSelection.Visible = true;
        else if(!abFlag)
            trBankSelection.Visible = false;

        if (miGatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt())
        {
            //if (rdoPaymentTypes.SelectedValue == Constants.S_ONE)
            //{
                trBankSelection.Visible = false;
                trTotalAmount.Visible = false;
                trNote.Visible = true;
                abFlag = false;
            //}
            //else
            //{
            //    trBankSelection.Visible = true;
            //    trTotalAmount.Visible = true;
            //    trNote.Visible = false;
            //    txtAmtPerTransaction.Text = "-";
            //}
            //txtTotalFees.Text = txtFormFees.Text;
        }        
        else
        {
            trTotalAmount.Visible = abFlag;
            trNote.Visible = !abFlag;
        }
        
        if (!abFlag)
        {
            NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL=new NetBankingPaymentTransactionsBL();
            oNetBankingPaymentTransactionsBL.GetMinMaxCharges(rdoPaymentTypes.SelectedValue.ToInt(),miGatewayId);
            trServiceTax.Visible = true;

            int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();

            //if (iSchoolId == Constants.SchoolId.DYPV.ToInt() || iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.VPMCPS.ToInt())
            //{
                trProcessingCharges.Visible = false;
                trProcessChrges.Visible = false;
                txtServiceTax.Text = "18%";
            //}
            //else
            //{
            //    trProcessingCharges.Visible = true;
            //    trProcessChrges.Visible = true;
            //    txtServiceTax.Text = oNetBankingPaymentTransactionsBL.ServiceTax;
            //}
            
            txtAmtPerTransaction.Text = oNetBankingPaymentTransactionsBL.MinMaxCharge;
        }
    }

	/// <summary>
	///		This method populate blocked banks details
	/// </summary>
	private void PopulateBlockedBanks(int aiFeeType)
	{
		var oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		List<DisbaleBankDetails> lstDisbaleBankDetails = oSchoolwiseBankMasterBL.GetCurrentlyActiveDisabledBankDetails(ConfigurationManager.AppSettings["SchoolID"].ToInt(), aiFeeType)
																				.Where(bank => bank.RuleStatus == "C")
																				.ToList();
		mlstBlockedBanks = new List<BlockedBank>();
		foreach (DisbaleBankDetails bank in lstDisbaleBankDetails)
		{
			mlstBlockedBanks.Add(new BlockedBank
			{
				BankId = bank.RegBankDetails.NetBankingBankId,
				BankName = bank.RegBankDetails.RegisterdBankName,
				StartDate = bank.StartDateTime.ToString(),
				EndDate = (bank.EndDateTime.ToString() == S_DEFAULT_DATE_2 || bank.EndDateTime.ToString() == S_DEFAULT_DATE_3
								|| bank.EndDateTime.ToString() == S_DEFAULT_DATE_4 || bank.EndDateTime.ToString() == S_DEFAULT_DATE_5) ? Constants.S_ZERO : bank.EndDateTime.ToString()
			});
		}
	}

	/// <summary>
	///	This method is used to Serializes the list of disabled banks in JSON format and saves it to a hidden field.
	/// </summary>
	private void SerializeBlocksBanks()
	{
		if (mlstBlockedBanks.IsNull() || mlstBlockedBanks.Count <= 0)
			return;
		var obj = new Dictionary<string, object>();
		mlstBlockedBanks.ForEach(bank =>
		{
			if (!obj.ContainsKey(bank.BankId.ToString()))
			{
				obj.Add(bank.BankId.ToString(),
						new
						{
							BankName = bank.BankName,
							StartDate = bank.StartDate.ToString(),
							EndDate = bank.EndDate.ToString()
						});
			}

		});
		var jsSerializer = new JavaScriptSerializer();
		hidBlockedBanksJSON.Value = jsSerializer.Serialize(obj);
	}

	/// <summary>
	/// This method is used to read query string parameters.
	/// </summary>
	private void ReadQueryString()
	{
        if (QueryString["TotalAmount"] != null)
        {
            miTotalFee = QueryString["TotalAmount"].ToDecimal();
            ViewState[S_ORIGIONAL_AMOUNT] = QueryString["TotalAmount"].ToDecimal();
        }
        if (QueryString["Amount"] != null)
        {
            msAmount = QueryString["Amount"];            
            ViewState[S_ORIGIONAL_AMOUNT] = QueryString["Amount"].ToDecimal();
        }

        if (QueryString["ConcessionAmount"] != null)
        {
            miConcessionAmount = QueryString["ConcessionAmount"].ToInt();
            ViewState[CONCESSION_AMOUNT] = QueryString["ConcessionAmount"].ToDecimal();
        }

        if (QueryString[S_FORM_NUMBER] != null)
            ViewState[S_FORM_NUMBER] = QueryString[S_FORM_NUMBER].ToString();

        if (QueryString["From"] != null)
        {
            hidTransactionFrom.Value = QueryString["From"];
            Session.Add(Constants.S_TRANSACTION_FROM, hidTransactionFrom.Value);
        }
        else
        {
            hidTransactionFrom.Value = S_ADMISSION;
            Session.Add(Constants.S_TRANSACTION_FROM, hidTransactionFrom.Value);
            this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";
        }

        if (QueryString["IsOldAcademicYearPayment"] != null)
            Session["IsOldAcademicYearPayment"] = QueryString["IsOldAcademicYearPayment"].ToString();
        else
            Session["IsOldAcademicYearPayment"] = null;

        if (QueryString["SchoolwiseStudentFeeIds"] != null && QueryString["SchoolwiseStudentFeeIds"] != string.Empty)
            hidSchoolwiseStudentFeeIds.Value = QueryString["SchoolwiseStudentFeeIds"];
		
		 if (QueryString["AcademicYearId"] != null && QueryString["AcademicYearId"].ToString() != string.Empty)
            hidAcademicYearId.Value = QueryString["AcademicYearId"].ToString();

         if (QueryString["SelectedFeeType"] != null && QueryString["SelectedFeeType"] != string.Empty)
             hidSelectedFeeTypes.Value = QueryString["SelectedFeeType"];
         else
         {
             if (QueryString["From"] != null && QueryString["From"].ToString() == "CautionMoney")
                 hidSelectedFeeTypes.Value = "CautionMoney";
             else
                 hidSelectedFeeTypes.Value = string.Empty;
         }

         if (QueryString["ScStudentId"] != null && QueryString["ScStudentId"] != string.Empty)
             hidScStudentId.Value = QueryString["ScStudentId"];
         else
             hidScStudentId.Value = string.Empty;

         if (QueryString["NxtAcYearId"] != null && QueryString["NxtAcYearId"] != string.Empty)
             hidNxtAcYearId.Value = QueryString["NxtAcYearId"];
         else
             hidNxtAcYearId.Value = string.Empty;
        
	}

	/// <summary>
	/// This method is used to fill bank drop down.
	/// </summary>
	private void FillBankDropDown()
	{
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
		int iFeeType = Constants.OnlineFeeTypes.AdmissionFee.ToInt();
        int iPaymentTypeId = rdoPaymentTypes.SelectedValue.ToInt();
        if (hidTransactionFrom.Value == Constants.OnlineFeeTypes.StudentFee.ToString() || hidTransactionFrom.Value == Constants.OnlineFeeTypes.CautionMoney.ToString())
			iFeeType = Constants.OnlineFeeTypes.StudentFee.ToInt();
        
		DataTable oDtNetBankingBanks = SchoolwiseBankMasterCollectionBL.GetNetBankingBanksList(iSchoolId, iFeeType,iPaymentTypeId,miGatewayId);
		cmbBanks.Bind(oDtNetBankingBanks, "RegisteredBankID", "RegisterdBankName", Constants.S_SELECT);

        if (oDtNetBankingBanks.Rows.Count == 1)
        {
            cmbBanks.SelectedIndex = Constants.I_ONE;
            cmbBanks_SelectedIndexChanged(null, null);            
        }

        VisibleHideFields(mbHasBankSelection);

		PopulateBlockedBanks(iFeeType);
	}

    /// <summary>
    /// This method will be used to fill the bank drop down list.
    /// </summary>
    private void FillPaymentTypes()
    {
        NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();
        List<OnlinePaymentType> lstOnlinePaymentType = new List<OnlinePaymentType>();

        if (AllowMultipleGateway)
        {
            lstOnlinePaymentType.Add(new OnlinePaymentType { Id = 1, Type = "Axis" });            
            lstOnlinePaymentType.Add(new OnlinePaymentType { Id = 2, Type = "RazorPay" });
        }
        else
            lstOnlinePaymentType = oNetBankingPaymentTransactionsBL.GetOnlinePaymentTypes();

        rdoPaymentTypes.DataSource = lstOnlinePaymentType;
        rdoPaymentTypes.DataBind();
        rdoPaymentTypes.Items[0].Selected = true;
    }

    /// <summary>
    /// This method is used to set payment type state.
    /// </summary>
    private void SetPaymentTypeState()
    {   
        //if (miGatewayId == Convert.ToInt32(Constants.PaymentGateways.Atom) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.EaseBuzz) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.AxisBankForAll) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.PayUMoney) || (miGatewayId == Convert.ToInt32(Constants.PaymentGateways.PayU) && (miSchoolId == Constants.SchoolId.SNS.ToInt() || miSchoolId == Constants.SchoolId.DSK.ToInt())))
        if (miGatewayId == Convert.ToInt32(Constants.PaymentGateways.Atom) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.RazorPay) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.CCAvenue) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.CCAvenueVPMCPS) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.AxisBankForAll) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.Billdesk) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.BilldeskDYP) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.EaseBuzz) || miGatewayId == Convert.ToInt32(Constants.PaymentGateways.PayUMoney) || (miGatewayId == Convert.ToInt32(Constants.PaymentGateways.PayU) && (miSchoolId == Constants.SchoolId.SNS.ToInt() || miSchoolId == Constants.SchoolId.DSK.ToInt())))
        {
            if (AllowMultipleGateway)
            {
                trPaymentType.Visible = true;
                Span1.InnerText = "Gateway : ";
            }
            else
                trPaymentType.Visible = false;

            //trPaymentType.Visible = false;
            spnNote.InnerText = "Payment Type and Bank selection will happen at payment gateway.";
            trConfirmationBank.Visible = false;
        }
    }

	/// <summary>
	/// This method is used to read payment data
	/// </summary>
	private void ReadPaymentData()
	{
        lblbankNameVal.Text = cmbBanks.SelectedItem.Text;
        if ((cmbBanks.Visible) || (cmbBanks.Items.Count== Constants.I_TWO && !cmbBanks.Visible))
        {
            lblNote2.Text = S_PAYABLE_AMOUNT_MESSAGE.Replace("%INCLUDE%", "Including"); //"Payable Amount is shown by including processing charge & service tax.";
            lblAmountPay.Text = txtTotalFees.Text;
        }
        else
        {
            lblAmountPay.Text = txtFormFees.Text;
            lblNote2.Text = S_PAYABLE_AMOUNT_MESSAGE.Replace("%INCLUDE%", "Excluding");
            if (rdoPaymentTypes.SelectedValue.ToInt() == Constants.PaymentMethod.BankPayment.ToInt())
                lblbankNameVal.Text = "Net Banking";            
        }    

        hidOrigionalAmt.Value = txtFormFees.Text;
		
	    if (hidTransactionFrom.Value == S_ADMISSION)
			this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";
	}

	/// <summary>
	/// This method is used to insert transaction confirmation.
	/// </summary>
	/// <param name="asBankId"></param>
	/// <param name="asPayAmt"></param>	
	/// <returns></returns>
    private int ConfirmTransaction(string asBankId, string asPayAmt, int aiConcessionAmount)
	{
        int iTransactionID = Constants.I_ZERO;
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction
        {
            PaymentITCParameter = Session.SessionID,
            TransactionAMT = asPayAmt.ToDouble(),
            TransactionBankID = asBankId,
            TransactionStatus = Constants.TransactionStatus.Created,
            GatewayId = miGatewayId,
            ConcessionAmount = aiConcessionAmount
        };

		if (!IsOnlineAdmissionFee)
		{
            string sRemarks = string.Empty;
			int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
			if (Session["FinalAcademicYearId"] != null)
				miAcademicYearId = Session["FinalAcademicYearId"].ToInt();
			if (Session["FinalYearStudentId"] != null)
				iStudentId = Session["FinalYearStudentId"].ToInt();
            if (Session[S_LATEFEEREMARKS] != null)
                sRemarks = (Session[S_LATEFEEREMARKS]).ToString();

			if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == "Y")
                iTransactionID = CreateNextYearFeeTransaction(oNetBankingTransaction, sRemarks);
			else
                iTransactionID = CreateCurrentYearFeeTransaction(oNetBankingTransaction, iStudentId, sRemarks);            
			ClearSessionVariables();
		}
		else
            iTransactionID = CreateAdmissionTransaction(oNetBankingTransaction);
        return iTransactionID;
	}

    /// <summary>
    /// This method is used to create mid-year fee transaction.
    /// </summary>
    /// <param name="aoNetBankingTransaction"></param>
    /// <returns></returns>
    private int CreateNextYearFeeTransaction(NetBankingTransaction aoNetBankingTransaction, string asRemarks)
    {
        int iStudentId = Session["NewStudentID"].ToInt();
        miAcademicYearId = Session["NewAcademicYearID"].ToInt();
        int iStandardId = Session["NewStandardID"].ToInt();
        int iLateFeeAmount = Session[S_LATEFEEAMOUNT].ToInt();
        string sDueDatesFilterXML = GetXMLForDueDates();
        bool bIsForInternalFee = Session["IsInternalFeePayment"].ToString() == "Y" ? true : false;
        string sInternalFeeDetailsIds = Constants.S_ZERO;
        if (bIsForInternalFee)
            sInternalFeeDetailsIds = Session["InternalFeeDetailsId"].ToString();

        var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL(miSchoolId, miAcademicYearId, iStudentId);
        DataTable oDataTable = oNetBankingPaymentTransactionsBL.PayStudentNextYearFeeOnLine(iStandardId, asRemarks, sDueDatesFilterXML, iLateFeeAmount, aoNetBankingTransaction, bIsForInternalFee, sInternalFeeDetailsIds, hidSelectedFeeTypes.Value);

        SetPaymentData(oDataTable);

        return oDataTable.Rows[0][0].ToInt();
    }

    /// <summary>
    /// This method is used create current year fee transaction.
    /// </summary>
    /// <param name="aoNetBankingTransaction"></param>
    /// <param name="aiStudentId"></param>
    /// <returns></returns>
    private int CreateCurrentYearFeeTransaction(NetBankingTransaction aoNetBankingTransaction, int aiStudentId, string asRemarks)
    {
        int iLateFeeAmount = Session[S_LATEFEEAMOUNT].ToInt();
        bool bIsPayFromMobile = Convert.ToBoolean(Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE]);
        string sLateFeeRemark = String.Empty;
        if (iLateFeeAmount != 0)
            sLateFeeRemark = asRemarks.Substring(asRemarks.IndexOf("Late fee for ") + 13);
        string sStudentFeeIdXML = GetStudentFeeIdXML();
        var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL(miSchoolId, miAcademicYearId, aiStudentId);

        bool bIsCautionMoneyPayment = false;
        bool bIsInternalFeePayment = false;

        if (hidTransactionFrom.Value == Constants.OnlineFeeTypes.CautionMoney.ToString())
            bIsCautionMoneyPayment = true;

        if (hidTransactionFrom.Value == Constants.OnlineFeeTypes.InternalFee.ToString())
            bIsInternalFeePayment = true;

        DataTable oDataTable = oNetBankingPaymentTransactionsBL.PayStudentFeeOnLine(iLateFeeAmount, asRemarks, sLateFeeRemark, sStudentFeeIdXML, aoNetBankingTransaction, bIsPayFromMobile, bIsCautionMoneyPayment, bIsInternalFeePayment, hidAcademicYearId.Value.ToInt());

        SetPaymentData(oDataTable);

        return oDataTable.Rows[0][0].ToInt();
    }

    /// <summary>
    /// This method is used to set payment details.
    /// </summary>
    /// <param name="aoDT"></param>
    private void SetPaymentData(DataTable aoDT)
    {
        if (aoDT.Rows.Count > 0)
        {
            Session[Constants.S_SESSION_PAYMENT_RECORD] = null;
            Dictionary<string, string> dictRecord = new Dictionary<string, string>();
            dictRecord.Add("TxnId", aoDT.Rows[0][0].ToString());
            dictRecord.Add("TxnAmt", aoDT.Rows[0]["FinalAmt"] + ".00");
            dictRecord.Add("TxnGuid", aoDT.Rows[0]["Guid"].ToString());
            Session[Constants.S_SESSION_PAYMENT_RECORD] = dictRecord;
        }
    }

    /// <summary>
    /// This method is used to create admission transaction.
    /// </summary>
    /// <param name="aoNetBankingTransaction"></param>
    /// <returns></returns>
    private int CreateAdmissionTransaction(NetBankingTransaction aoNetBankingTransaction)
    {
        int iTransactionID = Constants.I_ZERO;
        aoNetBankingTransaction.PaymentReferenceNumber = Session.SessionID;
        aoNetBankingTransaction.PaymentITCParameter = "From$$" + Convert.ToString(hidTransactionFrom.Value);
        var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();
        int iAdmissionId = Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToInt();
        if (iAdmissionId != 0)
        {
            DataTable dt  = oNetBankingPaymentTransactionsBL.CreateNetBankingTransaction(aoNetBankingTransaction, iAdmissionId);
            iTransactionID = dt.Rows[0][0].ToInt();
            SetPaymentData(dt);
        }

        return iTransactionID;
    }

	/// <summary>
	///	This method used to Clears some values set in the session previously.
	/// </summary>
	private void ClearSessionVariables()
	{	
		Session[S_LATEFEEREMARKS] = null;
		Session[S_LATEFEEAMOUNT] = null;
		Session[S_DUEDATES] = null;
	}

	/// <summary>
	///	This method	Returns StudentFeeId list in XML format.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ApplicationException">Thrown when Schoolwise_Student_Fee_Id is null in the session.</exception>
	private string GetStudentFeeIdXML()
	{
		if (Session[S_SCHOOWISE_STUDENT_FEE_ID] == null)
			throw new ApplicationException(S_PROCESSING_ERROR);
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();
		// Create a root level element.
		XmlElement root = oDoc.CreateElement("StudentFeeId");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentFeeId", String.Empty);

        if (Settings.EnablePartialFeePaymentForStudentLogin)
        {
            string[] oStudentFeeIds = hidSchoolwiseStudentFeeIds.Value.Split(',');
            foreach (string feeDetails in oStudentFeeIds)
            {
                List<string> lstFeeDetails = feeDetails.Split('$').ToList();
                int iStudentFeeId = lstFeeDetails[0].ToInt();
                int iAmount = lstFeeDetails[1].ToInt();

                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentFeeId", String.Empty);

                string sAtrrName = "StudentFeeId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = iStudentFeeId.ToString();
                oXmlNode.Attributes.Append(attr);

                string sAtrrName1 = "Amount";
                XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
                attr1.Value = iAmount.ToString();
                oXmlNode.Attributes.Append(attr1);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
                
            }
        }
        else
        {
            var oStudentFeeIds = Session[S_SCHOOWISE_STUDENT_FEE_ID] as List<int>;
            // Loop through all the list view items.
            foreach (int sStudentFeeId in oStudentFeeIds)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentFeeId", String.Empty);
                string sAtrrName = "StudentFeeId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sStudentFeeId.ToString();
                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);
		// return the string generated.
		return root.InnerXml;
	}

	/// <summary>
	///  This method Generate XML for the Due Dates.
	/// </summary>
	/// <returns></returns>
	private string GetXMLForDueDates()
	{
		const string S_ELEMENT = "element";
		var oDoc = new XmlDocument();
		// Create a root level element.
		XmlElement root = oDoc.CreateElement("DueDates");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DueDates", String.Empty);
		string[] sArrDueDates = Session["DueDates"].ToString().Split(',');
		// Loop through all the list view items.
		foreach (string sDueDate in sArrDueDates)
		{
			// Create root xml element.
			XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "DueDates", String.Empty);
			string sAtrrName = "DueDate";
			XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = sDueDate;
			oXmlNode.Attributes.Append(attr);
			// Add the node to root node.
			oXmlRootNode.AppendChild(oXmlNode);
		}
		// Add the root node to document element. 
		root.AppendChild(oXmlRootNode);
		// return the string generated.
		return root.InnerXml;
	}	

    /// <summary>
    /// POST data and Redirect to the specified url using the specified page.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="destinationUrl"></param>
    /// <param name="data"></param>
    private void RedirectAndPOST(Page aopage, string asdestinationUrl, Hashtable aodata)
    {
        StringBuilder sAllFields = new StringBuilder();
        foreach (DictionaryEntry key in aodata)
        {
            sAllFields.Append("," + key.Key.ToString() + "=" + key.Value.ToString());
         }
        if (sAllFields.ToString().StartsWith(","))
            hidParameters.Value = sAllFields.ToString().Substring(1);                
        hidRequestUrl.Value = asdestinationUrl;      
    }
}

public class BlockedBank
{
	public int BankId { get; set; }
	public string BankName { get; set; }
	public string StartDate { get; set; }
	public string EndDate { get; set; }
}

/// <summary>
/// To compare the strings & generate the MD5 signature.
/// </summary>
class VPCStringComparer : IComparer
{    
    public int Compare(Object aObj, Object aObj1)
    {
        if (aObj == aObj1) return 0;
        if (aObj == null) return -1;
        if (aObj1 == null) return 1;

        // Ensure we have string to compare
        string sStr1 = aObj as string;
        string sStr2 = aObj1 as string;

        // Get the CompareInfo object to use for comparing
        CompareInfo myComparer = CompareInfo.GetCompareInfo("en-US");
        if (sStr1 != null && sStr2 != null)
        {
            // Compare using an Ordinal Comparison.
            return myComparer.Compare(sStr1, sStr2, CompareOptions.Ordinal);
        }
        throw new ArgumentException("should be strings.");
    }
}

/// <summary>
/// This class is used to prepare query string for axis bank gateway.
/// </summary>
public class AXISPGRequest : PaymentConfirmationUI
{
    #region "Data Member(s)"

    private string msRuturnURL;

    #endregion    

    #region "Constructor"

    public AXISPGRequest()
    {
    }

    public AXISPGRequest(string asReturnURL)
    {
        this.msRuturnURL = asReturnURL;
    }
    
    #endregion "Constructor"

    #region "Public Methods"
    public string PrepareQueryString(string asAmount, int aiTransactionId,string asBankId, PaymentGateWayDetails aoPaymentGateWayDetails)
    {
        //msRuturnURL = msRuturnURL + "PaymentStatusUI.Aspx";

        msRuturnURL = "https://processRequest.riteschool.com/process.aspx";

        SortedList oTransactionData = new SortedList(new VPCStringComparer());
        
        // create start of QueryString data
        string sQueryString = aoPaymentGateWayDetails.NetBankingUrl;

        oTransactionData.Add("vpc_Version", aoPaymentGateWayDetails.Version);
        oTransactionData.Add("vpc_Command", aoPaymentGateWayDetails.Command);
        oTransactionData.Add("vpc_AccessCode", aoPaymentGateWayDetails.AccessCode);
        oTransactionData.Add("vpc_MerchTxnRef", aiTransactionId);
        oTransactionData.Add("vpc_Merchant", aoPaymentGateWayDetails.MerchantId);
        oTransactionData.Add("vpc_Amount", Math.Round(asAmount.ToDecimal()*100).ToString());
        oTransactionData.Add("vpc_Locale", aoPaymentGateWayDetails.Locale);
        oTransactionData.Add("vpc_ReturnURL", msRuturnURL);
        oTransactionData.Add("vpc_Currency", "INR");
        oTransactionData.Add("vpc_OrderInfo", "");

        oTransactionData.Add("vpc_AVS_PostCode", asBankId);
        string sHashData = aoPaymentGateWayDetails.Hash;
        string sSeperator = "?";

        // Loop through all the data in the SortedList transaction data
        foreach (DictionaryEntry item in oTransactionData)
        {
            sQueryString += sSeperator + HttpUtility.UrlEncode(item.Key.ToString()) + "=" + HttpUtility.UrlEncode(item.Value.ToString());
            sSeperator = "&";

            if (aoPaymentGateWayDetails.Hash.Length > 0)
                sHashData += item.Value.ToString();
        }

        // Create the MD5 signature if required        
        if (aoPaymentGateWayDetails.Hash.Length > 0)
        {
            // create the signature and add it to the query string
            string sSignature = CreateSHA256Signature(aoPaymentGateWayDetails.Hash, oTransactionData);
            sQueryString += sSeperator + "vpc_SecureHash=" + sSignature + "&vpc_SecureHashType=SHA256";
        }

        return sQueryString;
    }

    private string CreateSHA256Signature(string asHash, SortedList aoTransactionData)
    {
        // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
        // hex decoding eliminates this source of error as it is independent of the character encoding
        // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
        byte[] oConvertedHash = new byte[asHash.Length / 2];
        for (int iIndex = 0; iIndex < asHash.Length / 2; iIndex++)
        {
            oConvertedHash[iIndex] = (byte)Int32.Parse(asHash.Substring(iIndex * 2, 2), System.Globalization.NumberStyles.HexNumber);
        }

        // Build string from collection in preperation to be hashed
        StringBuilder sb = new StringBuilder();
        
        foreach (DictionaryEntry kvp in aoTransactionData)
        {
            if (kvp.Key.ToString().StartsWith("vpc_") || kvp.Key.ToString().StartsWith("user_"))
                sb.Append(kvp.Key.ToString() + "=" + kvp.Value.ToString() + "&");
        }

        // remove trailing & from string
        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        // Create secureHash on string
        string hexHash = string.Empty;
        using (HMACSHA256 hasher = new HMACSHA256(oConvertedHash))
        {
            byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            foreach (byte b in hashValue)
            {
                hexHash += b.ToString("X2");
            }
        }
        return hexHash;
    }

    /// <summary>
    /// This method will be used to calculate fee amount on change on bank.
    /// </summary>
    /// <param name="adtRow"></param>
    /// <param name="dDeductedAmount"></param>
    /// <param name="dFormFees"></param>
    /// <param name="adServiceTaxAmount"></param>
    /// <param name="adTotalAmt"></param>
    /// <param name="aoPaymentConfirmationUI"></param>
    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();       
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }    

    #endregion "Public Methods"

    #endregion -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to create MD5 type signature for given type.
    /// </summary>
    /// <param name="RawData"></param>
    /// <returns></returns>
    private string CreateMD5Signature(string asRawData)
    {
        MD5 oMD5 = MD5CryptoServiceProvider.Create();
        byte[] ArrHashValue = oMD5.ComputeHash(Encoding.ASCII.GetBytes(asRawData));

        string strHex = "";
        foreach (byte b in ArrHashValue)
        {
            strHex += b.ToString("x2");
        }
        return strHex.ToUpper();
    }    
}

/// <summary>
/// This class will be used to prepare query string for TPSL payment gateway.
/// </summary>
public class TPSLPGRequestData : SchoolBase
{
    #region "Consants"

    private const string S_EXTENSION = ".property";    

    #endregion    

    #region "Data Member(s)"

    private string msRuturnURL;
    private Decimal mdTotalAmt;    
    private Decimal mdServiceTaxAmount;

    #endregion    

    #region "Constructor"

    public TPSLPGRequestData(string asReturnURL)
    {
        this.msRuturnURL = asReturnURL;
    }

    public TPSLPGRequestData(Decimal adTotalAmt, Decimal adServiceTaxAmount)
    {
        this.mdTotalAmt = adTotalAmt;
        this.mdServiceTaxAmount = adServiceTaxAmount;        
    }

    #endregion "Constructor"

    #region "Public Methods"

    public string GetQueryString(bool abIsOnlineAdmissionFee, string asAmount, int aiTransactionId, string asBankId)
    {
        string sMerchantId = (abIsOnlineAdmissionFee ? Settings.AdmissionFormSubmerchantID : Settings.StudentFeesSubmerchantID);
        string sPropertyPath = sMerchantId == Settings.AdmissionFormSubmerchantID ? "~/MerchantDetailsAdmission.property" : "~/MerchantDetailsFee.property";       
        string sPath = Server.MapPath(sPropertyPath);
        string[] sArrlines = File.ReadAllLines(@sPath);
        var path = Path.GetTempPath();
        var sFileName = Guid.NewGuid().ToString() + S_EXTENSION;
        string sTempPath = Path.Combine(path, sFileName);
        using (var file = new StreamWriter(@sTempPath))
        {
            foreach (string line in sArrlines)
            {
                if (line.Contains("ResponseUrl") == false)
                    file.WriteLine(line);
                else
                    file.WriteLine("ResponseUrl=" + msRuturnURL + "PaymentStatusUI.Aspx");
            }
        }
        var oCheckSumRequestBean = new CheckSumRequestBean
        {
            MerchantTranId = aiTransactionId.ToString(),
            MarketCode = sMerchantId,
            AccountNo = "1",
            Amt = asAmount,
            BankCode = asBankId,
            PropertyPath = sTempPath
        };
        var oTPSLUtil = new TPSLUtil1();
        return oTPSLUtil.transactionRequestMessage(oCheckSumRequestBean);       
    }

    /// <summary>
    /// This method is used to Calculate Online Student Fee Amount.
    /// </summary>
    /// <param name="adtRow"></param>
    /// <param name="abTax"></param>
    public void CalculateStudentFeeAmounts(DataRow adtRow,bool abTax, out Decimal dDeductedAmount, out Decimal dFormFees, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt;
        decimal dRound = 0.00499999M;
        dFormFee = mdTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        if (adtRow["PerTransactionFeesAmountInPercent"] == DBNull.Value)
        {
            dDeductedAmt = Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2);
            aoPaymentConfirmationUI.LblAmtPerTransaction.Text = " Rs." + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2).ToString();
            dDeductedAmt = dDeductedAmt + Decimal.Round(adtRow["PerTransactionFeesAmountInApppliedPercent"].ToDecimal() + dRound, 2);            
            aoPaymentConfirmationUI.TrServiceTax.Visible = abTax;
            aoPaymentConfirmationUI.TrProcessChrges.Visible = abTax;
        }
        // Else, the transaction charges are in percentage.
        else
        {
            decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInApppliedPercent"].ToDecimal();
            dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
            dDeductedAmt = Decimal.Round(dDeductedAmt + dRound, 2);
            aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";            
            aoPaymentConfirmationUI.TrServiceTax.Visible = abTax;
            aoPaymentConfirmationUI.TrProcessChrges.Visible = abTax;
        }
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    /// <summary>
    /// This Method is used to Calculate Online Admission Amount.
    /// </summary>
    /// <param name="adtRow"></param>
    /// <param name="abTax"></param>
    public void CalculateStudentsAdmissionAmount(DataRow adtRow, bool abTax, out Decimal adDeductedAmount, out Decimal adFormFees, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt;
        decimal dRound = 0.00499999M;
        dFormFee = mdTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        if (adtRow["PerTransactionFormAmountInPercent"] == DBNull.Value)
        {
            dDeductedAmt = Decimal.Round(adtRow["PerTransactionFormAmountInRs"].ToDecimal() + dRound, 2);
            aoPaymentConfirmationUI.LblAmtPerTransaction.Text = " Rs." + Decimal.Round(adtRow["PerTransactionFormAmountInRs"].ToDecimal() + dRound, 2).ToString();
            aoPaymentConfirmationUI.TrServiceTax.Visible = abTax;
            aoPaymentConfirmationUI.TrProcessChrges.Visible = abTax;
            dDeductedAmt = dDeductedAmt + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2) * (mdServiceTaxAmount / 100);
        }
        // Else, the transaction charges are in percentage.
        else
        {
            decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFormAmountInApppliedPercent"].ToDecimal();
            dPerTransactionFeesAmountInApppliedPercent = Decimal.Round(dPerTransactionFeesAmountInApppliedPercent + dRound, 2);
            dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
            dDeductedAmt = Decimal.Round(dDeductedAmt + dRound, 2);
            aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFormAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
            aoPaymentConfirmationUI.TrServiceTax.Visible = !abTax;
            aoPaymentConfirmationUI.TrProcessChrges.Visible = !abTax;
        }
        adDeductedAmount = dDeductedAmt;
        adFormFees = dFormFee;
    }

    #endregion "Public Methods"
}

/// <summary>
/// This class is used to prepare query string for payU bank gateway.
/// </summary>
public class PayUPGRequest : SchoolBase
{
    #region "Data Member(s)"

    private string msRuturnURL;
    private string msFirstName;
    private string msEmail;
    private string msPhone;

    #endregion

    #region "Constructor"

    public PayUPGRequest()
    {
    }

    public PayUPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msRuturnURL = asReturnURL;
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
    }

    #endregion "Constructor"

    #region "Public Methods"

    public Hashtable PrepareTransactionData(string asAmount, int aiTransactionId, string asBankId, PaymentGateWayDetails aoPaymentGateWayDetails, bool abUseAlternate)
    {
        if (abUseAlternate)
            msRuturnURL = "http://processpg.riteschool.com/ProcessPayu.aspx";
        else
            msRuturnURL = msRuturnURL + "PaymentStatusUI.Aspx";

        Hashtable oTransactionData = new Hashtable();
        oTransactionData.Add("key", aoPaymentGateWayDetails.MerchantId);
        oTransactionData.Add("txnid", aiTransactionId.ToString());
        oTransactionData.Add("amount", asAmount);
        oTransactionData.Add("productinfo", Session[Constants.S_TRANSACTION_FROM].ToString());
        oTransactionData.Add("firstname", msFirstName);
        oTransactionData.Add("email", msEmail);
        oTransactionData.Add("phone", msPhone);
        oTransactionData.Add("surl", msRuturnURL);
        oTransactionData.Add("furl", msRuturnURL);
        if (aoPaymentGateWayDetails.HasBankSelection)
        {
            oTransactionData.Add("pg", aoPaymentGateWayDetails.Command);
            oTransactionData.Add("bankcode", asBankId);
        }
        oTransactionData.Add("udf1", "");

        string sHashString = aoPaymentGateWayDetails.MerchantId + "|" + aiTransactionId.ToString() + "|" + asAmount + "|" + Session[Constants.S_TRANSACTION_FROM].ToString() + "|" + msFirstName + "|" + msEmail + "|||||||||||" + aoPaymentGateWayDetails.Hash;

        string sResult = CreateHash(sHashString);
        oTransactionData.Add("hash", sResult);
        return oTransactionData;
    }

    /// <summary>
    /// This method will be used to calculate fee amount on change on bank.
    /// </summary>
    /// <param name="adtRow"></param>
    /// <param name="dDeductedAmount"></param>
    /// <param name="dFormFees"></param>
    /// <param name="adServiceTaxAmount"></param>
    /// <param name="adTotalAmt"></param>
    /// <param name="aoPaymentConfirmationUI"></param>
    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxAmount, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        dDeductedAmt = Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = " Rs." + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2).ToString();
        dDeductedAmt = dDeductedAmt + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2) * (adServiceTaxAmount / 100);
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    #endregion "Public Methods"

    #region -- PRIVATE METHOD(s) --

    private string CreateHash(string asText)
    {

        byte[] Arrmessage = Encoding.UTF8.GetBytes(asText);
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] ArrHashValue;
        SHA512Managed oHashString = new SHA512Managed();
        string sHex = "";
        ArrHashValue = oHashString.ComputeHash(Arrmessage);
        foreach (byte x in ArrHashValue)
        {
            sHex += String.Format("{0:x2}", x);
        }
        return sHex;
    }

    #endregion
}
    /// <summary>
/// This class is used to prepare query string for payU bank gateway.
/// </summary>
public class AtomPGRequest : Page
{
    #region "Data Member(s)"

    private string msRuturnURL;
    private string msFirstName;
    private string msEmail;
    private string msPhone;
    private bool mbIsPrePrimaryStudent;

    #endregion    

    #region "Constructor"

    public AtomPGRequest()
    {
    }

    public AtomPGRequest(string asReturnURL, StudentNetBankingDetails aoStudentNetBankingDetails)
    {
        this.msRuturnURL = asReturnURL;
        this.msFirstName = aoStudentNetBankingDetails.FirstName;
        this.msEmail = aoStudentNetBankingDetails.Email;
        this.msPhone = aoStudentNetBankingDetails.Phone;
        this.mbIsPrePrimaryStudent = aoStudentNetBankingDetails.IsPreprimaryStudent;
    }
    
    #endregion "Constructor"

    public void SendPostRequest(HttpResponse aoHttpResponse, string asNetBankingUrl, string asAmount, int aiTransactionId)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();

        int iAtomCategoryId = Constants.AtomCategories.Primary.ToInt();
        if (this.mbIsPrePrimaryStudent)
            iAtomCategoryId = Constants.AtomCategories.PrePrimary.ToInt();

        List<AtomGatewayDetails> lstGatewayDetails = oPaymentGatewayBL.GetAtomGatewayDetails(iAtomCategoryId);
       
        StringBuilder oQueryString = new StringBuilder();
        lstGatewayDetails.ForEach(gd =>
            {
                string sValue = gd.Value;
                if (gd.Name == "clientcode")
                    sValue = EncodeTo64UTF8(gd.Name);
                oQueryString.Append("&" + gd.Name + "=" + sValue);
            });

        string sQuerystring = string.Empty;
        if (oQueryString.Length > 0)
            sQuerystring = oQueryString.ToString().Substring(1);

        sQuerystring = sQuerystring + "&amt=" + asAmount + "&txnid=" + aiTransactionId + "&date=" + DateTime.Now.ToString("dd/mm/yyyy hh:mm:ss") +"&udf1=" + msFirstName + "&udf2=" + msEmail + "&udf3=" + msPhone;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(asNetBankingUrl);

        request.Method = "POST";
        
        request.ContentType = "application/x-www-form-urlencoded";
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; CK={CVxk71YSfgiE6+6P6ftT7lWzblrdvMbRqavYf/6OcMIH8wfE6iK7TNkcwFAsxeChX7qRAlQhvPWso3KI6Jthvnvls9scl+OnAEhsgv+tuvs=}; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        byte[] byteArray = Encoding.UTF8.GetBytes(sQuerystring);

        request.ContentType = "application/x-www-form-urlencoded";

        request.ContentLength = byteArray.Length;
        request.AllowAutoRedirect = true;

        request.Proxy.Credentials = CredentialCache.DefaultCredentials;

        Stream dataStream = request.GetRequestStream();

        dataStream.Write(byteArray, 0, byteArray.Length);

        dataStream.Close();

        WebResponse response = request.GetResponse();

        XmlDocument objXML = new XmlDocument();

        dataStream = response.GetResponseStream();

        objXML.Load(dataStream);

        string TxnId = objXML.DocumentElement.ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerText;
        string Token = objXML.DocumentElement.ChildNodes[0].ChildNodes[0].ChildNodes[3].InnerText;
        string sState = objXML.DocumentElement.ChildNodes[0].ChildNodes[0].ChildNodes[4].InnerText;

        string txnData = "ttype=" + lstGatewayDetails.Where(gd => gd.Name == "ttype").FirstOrDefault().Value + "&txnStage=" + sState + "&tempTxnId=" + TxnId + "&token=" + Token;

        dataStream.Close();
        response.Close();

        aoHttpResponse.Redirect(asNetBankingUrl+"?" + txnData, false);
    }

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }    

    private string EncodeTo64UTF8(String str)
    {
        byte[] toEncode2Bytes = System.Text.Encoding.UTF8.GetBytes(str);
        string sReturnValue = System.Convert.ToBase64String(toEncode2Bytes);
        return sReturnValue;
    }
}

public class PayUMoneyPGRequest : Page
{
    #region "Data Member(s)"

    private string msReturnURL;
    private string msFirstName;
    private string msEmail;
    private string msPhone;

    #endregion

    #region "Constructor"

    public PayUMoneyPGRequest()
    {
    }

    public PayUMoneyPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msReturnURL = asReturnURL;
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
    }

    #endregion "Constructor"

    /// <summary>
    /// This method will be used to calculate fee amount on change on bank.
    /// </summary>
    /// <param name="adtRow"></param>
    /// <param name="dDeductedAmount"></param>
    /// <param name="dFormFees"></param>
    /// <param name="adServiceTaxAmount"></param>
    /// <param name="adTotalAmt"></param>
    /// <param name="aoPaymentConfirmationUI"></param>
    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxAmount, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        dDeductedAmt = Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = " Rs." + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2).ToString();
        dDeductedAmt = dDeductedAmt + Decimal.Round(adtRow["PerTransactionFeesAmountInRs"].ToDecimal() + dRound, 2) * (adServiceTaxAmount / 100);
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public Hashtable SendRequest(string asAmount, int aiTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails)
    {
        string sURL = string.Empty;
        string sHash = string.Empty;
        string[] sArrHashVarSequence;
        Hashtable data = new Hashtable();

        //msReturnURL = msReturnURL + "PaymentStatusUI.Aspx";
        //msReturnURL = "http://103.132.1.14:8078/processpayumoney.aspx";

        //msReturnURL = "http://processpg.riteschool.com/ProcessPayuMoney.aspx";

        msReturnURL = "https://processpgsns.riteschool.com/ProcessPayuMoney.aspx";
                
        sArrHashVarSequence = aoPaymentGateWayDetails.Sequence.Split('|');
        foreach (string sHashVal in sArrHashVarSequence)
        {
            if (sHashVal == "key")
            {
                sHash = sHash + aoPaymentGateWayDetails.MerchantId;
                sHash = sHash + '|';
            }
            else if (sHashVal == "txnid")
            {
                sHash = sHash + aiTransactionId;
                sHash = sHash + '|';
            }
            else if (sHashVal == "amount")
            {   
                sHash = sHash + Convert.ToDecimal(asAmount).ToString("g29");
                sHash = sHash + '|';
            }
            else if (sHashVal == "firstname")
            {   
                sHash = sHash + this.msFirstName;
                sHash = sHash + '|';
            }
            else if (sHashVal == "productinfo")
            {   
                sHash = sHash + aoPaymentGateWayDetails.ProductInfo;
                sHash = sHash + '|';
            }
            else if (sHashVal == "email")
            {   
                sHash = sHash + this.msEmail;
                sHash = sHash + '|';
            }
            else
            {   
                sHash = sHash + string.Empty;
                sHash = sHash + '|';
            }
        }

        sHash += aoPaymentGateWayDetails.Hash;
        sHash = Generatehash512(sHash).ToLower();
        
        if (!string.IsNullOrEmpty(sHash))
        {    
            data.Add("hash", sHash);
            data.Add("txnid", aiTransactionId);
            data.Add("key", aoPaymentGateWayDetails.MerchantId);

            data.Add("amount", Convert.ToDecimal(asAmount).ToString("g29"));
            data.Add("firstname", msFirstName);
            data.Add("email", this.msEmail);
            data.Add("phone", msPhone);
            data.Add("productinfo", aoPaymentGateWayDetails.ProductInfo);
            
            data.Add("surl", msReturnURL);
            data.Add("furl", msReturnURL);
            data.Add("lastname", string.Empty);
            data.Add("curl", msReturnURL);

            data.Add("address1", string.Empty);
            data.Add("address2", string.Empty);
            data.Add("city", string.Empty);
            data.Add("state", string.Empty);
            data.Add("country", string.Empty);
            data.Add("zipcode", string.Empty);
            data.Add("udf1", string.Empty);
            data.Add("udf2", string.Empty);
            data.Add("udf3", string.Empty);
            data.Add("udf4", string.Empty);
            data.Add("udf5", string.Empty);

            data.Add("pg", string.Empty);
            data.Add("service_provider", "payu_paisa");
        }
        else
        {
            //no hash
        }

        return data;
    }

    public string PreparePOSTForm(string asURL, Hashtable aoHashTable)      // post form
    {   
        string formID = "PostForm1";
     
        StringBuilder strForm = new StringBuilder();
        strForm.Append("<form id=\"" + formID + "\" name=\"" +
                       formID + "\" action=\"" + asURL +
                       "\" method=\"POST\">");

        foreach (DictionaryEntry key in aoHashTable)
        {
            strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                           "\" value=\"" + key.Value + "\">");
        }


        strForm.Append("</form>");
        
        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript'>");
        strScript.Append("var v" + formID + " = document." +
                         formID + ";");
        strScript.Append("v" + formID + ".submit();");
        strScript.Append("</script>");
        
        return strForm.ToString() + strScript.ToString();
    }

    private string Generatehash512(string text)
    {
        byte[] message = Encoding.UTF8.GetBytes(text);

        SHA512Managed hashString = new SHA512Managed();
        string hex = string.Empty;
        byte[] hashValue = hashString.ComputeHash(message);
        foreach (byte val in hashValue)
            hex += String.Format("{0:x2}", val);
        return hex;
    }
}

public class AxisBankForAllPGRequest : Page
{
    #region "Data Member(s)"

    private string msReturnURL;
    private string msFirstName;
    private string msEmail;
    private string msPhone;

    #endregion

    #region "Constructor"

    public AxisBankForAllPGRequest()
    {
    }

    public AxisBankForAllPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msReturnURL = asReturnURL;
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
    }

    #endregion "Constructor"

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public Hashtable SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentFor)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.AxisBankForAll);
        msReturnURL = msReturnURL + "PaymentStatusUI.Aspx";

        string sVER = lstGatewayAdditionalDetails.Where(gt => gt.Name == "VER").FirstOrDefault().Value;
        string sTYP = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TYP").FirstOrDefault().Value;
        string sCNY = lstGatewayAdditionalDetails.Where(gt => gt.Name == "CNY").FirstOrDefault().Value;
        string sRE1 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "RE1").FirstOrDefault().Value;

        string sHashKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "HASH_KEY").FirstOrDefault().Value;
        string sEncryptionKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ENCRYPTION_KEY").FirstOrDefault().Value;

        string sCRN = 1000+ asTransactionId.ToString();

        string sChecksum = sha256_hash(aoPaymentGateWayDetails.MerchantId + asTransactionId + sCRN + asAmount + sHashKey);
        
        string sPPiFields = aoPaymentGateWayDetails.MerchantId + "|" + asTransactionId + "|" + asPaymentFor + "|" + msFirstName + "|" + msEmail + "|" + asAmount;
        AddLog("PPI Fields : " + sPPiFields);
        string sPlainText = "CID=" + aoPaymentGateWayDetails.MerchantId + "&RID=" + asTransactionId + "&CRN=" + sCRN + "&AMT=" + asAmount + "&VER=" + sVER + "&TYP=" + sTYP + "&CNY=" + sCNY + "&RTU=" + msReturnURL + "&PPI=" + sPPiFields + "&RE1=" + sRE1 + "&RE2=" + msFirstName + "&RE3=" + msEmail + "&RE4=" + msPhone + "&RE5=&CKS=" + sChecksum;
        AddLog("Plain Text : " + sPlainText);
        string sEncryptedstring = Encrypt(sPlainText, sEncryptionKey);
        AddLog("Encrypted String : " + sEncryptedstring);

        Hashtable data = new Hashtable();
        data.Add("i", sEncryptedstring);

        return data;
    }

    public static String sha256_hash(String value)
    {
        StringBuilder Sb = new StringBuilder();
        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }
        return Sb.ToString();
    }

    public string Encrypt(string input, string key)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);
        Aes kgen = Aes.Create("AES");
        kgen.Mode = CipherMode.ECB;
        kgen.Key = keyArray;
        ICryptoTransform cTransform = kgen.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    private void AddLog(string asMessage, bool abIsStartingMessage = false)
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if (ConfigurationManager.AppSettings["LogFilePath"] != null && ConfigurationManager.AppSettings["LogFilePath"].ToString() != string.Empty)
        {
            string sPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            var sbContent = new StringBuilder();

            if (abIsStartingMessage)
                sbContent.AppendFormat("{0}{0}", Environment.NewLine, Environment.NewLine);

            sbContent.AppendFormat("School Id    : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            sbContent.AppendFormat("School Id   : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("Message : {0}{1}", asMessage, Environment.NewLine);

            var swFile = new StreamWriter(sPath + "QueryPlainText.log", true);
            swFile.WriteLine("\n" + sbContent);
            swFile.Flush();
            swFile.Close();
        }
    }
}

public class EaseBuzzPGRequest : Page
{
    #region "Data Member(s)"

    private string msReturnURL;
    private string msFirstName;
    private string msEmail;
    private string msPhone;

    #endregion

    #region "Constructor"

    public EaseBuzzPGRequest()
    {
    }

    public EaseBuzzPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msReturnURL = asReturnURL;
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
    }

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public Hashtable SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentFor)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.EaseBuzz);
        msReturnURL = msReturnURL + "PaymentStatusUI.Aspx";

        // Generate transaction ID -> make sure this is unique for all transactions
        Random rnd = new Random();
        string strHash = Easebuzz_Generatehash512(rnd.ToString() + DateTime.Now);
        //txnid = strHash.ToString().Substring(0, 20);
        //txnid = Txnid;

        //string paymentUrl = getURL();
        //// Get configs from web config
        //easebuzz_action_url = paymentUrl + "/pay/secure";

        // generate hash table
        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
        data.Add("txnid", asTransactionId);
        data.Add("key", aoPaymentGateWayDetails.MerchantId);
        //string AmountForm = Convert.ToDecimal(amount.Trim()).ToString("g29");// eliminating trailing zeros
        //amount = amount;
        data.Add("amount", asAmount);
        data.Add("firstname", msFirstName.Trim());
        data.Add("email", msEmail.Trim());
        data.Add("phone", msPhone.Trim());
        data.Add("productinfo", aoPaymentGateWayDetails.ProductInfo.Trim());
        data.Add("surl", msReturnURL.Trim());
        data.Add("furl", msReturnURL.Trim());
        data.Add("udf1", string.Empty);
        data.Add("udf2", string.Empty);
        data.Add("udf3", string.Empty);
        data.Add("udf4", string.Empty);
        data.Add("udf5", string.Empty);

        string hash_string = string.Empty;
        // generate hash
        //string[] hashVarsSeq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10".Split('|'); // spliting hash sequence from config
        string[] hashVarsSeq = aoPaymentGateWayDetails.Sequence.Split('|');
        hash_string = "";
        foreach (string hash_var in hashVarsSeq)
        {
            hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
            hash_string = hash_string + '|';
        }
        hash_string += aoPaymentGateWayDetails.Hash;// appending SALT
        string gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
        data.Add("hash", gen_hash);

        //string strForm = Easebuzz_PreparePOSTForm(easebuzz_action_url, data);
        //return strForm;

        return data;
    }

    // hashcode generation
    public string Easebuzz_Generatehash512(string text)
    {

        byte[] message = Encoding.UTF8.GetBytes(text);

        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;

    }

    #endregion "Constructor"
}


public class BilldeskPGRequest : Page
{
    #region "Data Member(s)"

    private string msFirstName;
    private string msEmail;
    private string msPhone;
    private string msReturnURL;

    #endregion

    #region "Constructor"

    public BilldeskPGRequest()
    {
    }

    public BilldeskPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
        msReturnURL = asReturnURL;
    }

    #endregion "Constructor"

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public string SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentFor)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.Billdesk);
        
        msReturnURL = msReturnURL + "PaymentStatusUI.Aspx";

        const string S_NOT_APPLICABLE = "NA";
        List<string> lstParaeters = new List<string>();

        string sCurrencyType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "CurrencyType").FirstOrDefault().Value;
        string sTypeField1 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TypeField1").FirstOrDefault().Value;
        string sTypeField2 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TypeField2").FirstOrDefault().Value;
        string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ChecksumKey").FirstOrDefault().Value;
        string sSecurityID = lstGatewayAdditionalDetails.Where(gt => gt.Name == "SecurityID").FirstOrDefault().Value;
        string sReturnURL = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ReturnURL").FirstOrDefault().Value;
        string sURLId = lstGatewayAdditionalDetails.Where(gt => gt.Name == "URLId").FirstOrDefault().Value;

        lstParaeters.Add(aoPaymentGateWayDetails.MerchantId);
        lstParaeters.Add(asTransactionId);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(asAmount+".00");
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sCurrencyType);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sTypeField1);
        lstParaeters.Add(sSecurityID);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sTypeField2);
        lstParaeters.Add(aoPaymentGateWayDetails.AccessCode);
        lstParaeters.Add(msPhone);
        lstParaeters.Add(msEmail);

        lstParaeters.Add(sURLId);
        //lstParaeters.Add(S_NOT_APPLICABLE);

        lstParaeters.Add(this.msFirstName);
        //lstParaeters.Add(S_NOT_APPLICABLE);

        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sReturnURL);

        //lstParaeters.Add(msReturnURL);
                

        StringBuilder sb = new StringBuilder();
        lstParaeters.ForEach(prm => sb.Append(prm + "|"));
        sb.Append(GetHMACSHA256(sb.ToString().Substring(0,sb.Length-1), sChecksumKey));

        return sb.ToString();
    }

    private string GetHMACSHA256(string text, string key)
    {
        UTF8Encoding encoder = new UTF8Encoding();

        byte[] hashValue;
        byte[] keybyt = encoder.GetBytes(key);
        byte[] message = encoder.GetBytes(text);

        HMACSHA256 hashString = new HMACSHA256(keybyt);
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex.ToUpper();
    }
}

public class BilldeskDYPPGRequest : Page
{
    #region "Data Member(s)"

    private string msFirstName;
    private string msEmail;
    private string msPhone;
    private string msReturnURL;

    #endregion

    #region "Constructor"

    public BilldeskDYPPGRequest()
    {
    }

    public BilldeskDYPPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
        msReturnURL = asReturnURL;
    }

    #endregion "Constructor"

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public string SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentFor)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.BilldeskDYP);

        const string S_NOT_APPLICABLE = "NA";
        List<string> lstParaeters = new List<string>();

        string sCurrencyType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "CurrencyType").FirstOrDefault().Value;
        string sTypeField1 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TypeField1").FirstOrDefault().Value;
        string sTypeField2 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TypeField2").FirstOrDefault().Value;
        string sTypeField3 = lstGatewayAdditionalDetails.Where(gt => gt.Name == "TypeField3").FirstOrDefault().Value;
        string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ChecksumKey").FirstOrDefault().Value;
        string sSecurityID = lstGatewayAdditionalDetails.Where(gt => gt.Name == "SecurityID").FirstOrDefault().Value;
        string sReturnURL = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ReturnURL").FirstOrDefault().Value;
        string sURLId = lstGatewayAdditionalDetails.Where(gt => gt.Name == "URLId").FirstOrDefault().Value;

        lstParaeters.Add(aoPaymentGateWayDetails.MerchantId);
        lstParaeters.Add(asTransactionId);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(asAmount + ".00");
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sCurrencyType);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sTypeField1);
        lstParaeters.Add(sSecurityID);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(sTypeField2);
        lstParaeters.Add(msEmail);
        lstParaeters.Add(msPhone);        
        lstParaeters.Add(sURLId);        
        lstParaeters.Add(this.msFirstName);
        
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);
        lstParaeters.Add(S_NOT_APPLICABLE);

        lstParaeters.Add(sTypeField3);

        StringBuilder sb = new StringBuilder();
        lstParaeters.ForEach(prm => sb.Append(prm + "|"));
        sb.Append(GetHMACSHA256(sb.ToString().Substring(0, sb.Length - 1), sChecksumKey).ToUpper());

        return sb.ToString();
    }

    public string GetHMACSHA256(string text, string key)
    {
        UTF8Encoding encoder = new UTF8Encoding();

        byte[] hashValue;
        byte[] keybyt = encoder.GetBytes(key);
        byte[] message = encoder.GetBytes(text);

        HMACSHA256 hashString = new HMACSHA256(keybyt);
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;
    }
}

public class CCAvenuePGRequest : Page
{
    #region "Data Member(s)"

    private string msFirstName;
    private string msEmail;
    private string msPhone;
    private string msReturnURL;

    #endregion

    #region "Constructor"

    public CCAvenuePGRequest()
    {
    }

    public CCAvenuePGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
    {
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
        msReturnURL = asReturnURL;
    }

    #endregion "Constructor"

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public string SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentForm, string asGuid)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenue);

        string sCurrencyType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "Currency").FirstOrDefault().Value;
        string sLanguage = lstGatewayAdditionalDetails.Where(gt => gt.Name == "Language").FirstOrDefault().Value;      
        string sReturnURL = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ReturnURL").FirstOrDefault().Value;
        string sEncryptionKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;

        string sFinalString = "merchant_id=" + aoPaymentGateWayDetails.MerchantId + "&" +
                              "order_id=" + asTransactionId + "&" +
                              "currency=" + sCurrencyType + "&" +
                              "amount=" + asAmount + "&" +
                              "redirect_url=" + sReturnURL + "&" +
                              "cancel_url=" + sReturnURL + "&" +
                              "language=" + sLanguage + "&" +
                              "merchant_param1=" + msFirstName + "&" +
                              "merchant_param2=" + msPhone + "&" +
                              "merchant_param3=" + msEmail + "&" +
                              "merchant_param4=" + asGuid;

        CCACrypto ccaCrypto = new CCACrypto();
        return ccaCrypto.Encrypt(sFinalString, sEncryptionKey);
    }   
}

public class CCAvenueVPMCPSPGRequest : Page
{
    #region "Data Member(s)"

    private string msFirstName;
    private string msEmail;
    private string msPhone;
    private string msReturnURL;
    private string msRegNoOrFormNo;

    #endregion

    #region "Constructor"

    public CCAvenueVPMCPSPGRequest()
    {
    }

    public CCAvenueVPMCPSPGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone, string asRegNoOrFormNo)
    {
        this.msFirstName = asFirstName;
        this.msEmail = asEmail;
        this.msPhone = asPhone;
        msReturnURL = asReturnURL;
        this.msRegNoOrFormNo = asRegNoOrFormNo;
    }

    #endregion "Constructor"

    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
    {
        decimal dFormFee;
        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
        decimal dRound = 0.00499999M;
        dFormFee = adTotalAmt;
        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
        // Else, the transaction charges are in percentage.       
        dDeductedAmount = dDeductedAmt;
        dFormFees = dFormFee;
    }

    public string SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentForm, string asGuid)
    {
        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenueVPMCPS);

        string sCurrencyType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "Currency").FirstOrDefault().Value;
        string sLanguage = lstGatewayAdditionalDetails.Where(gt => gt.Name == "Language").FirstOrDefault().Value;
        string sReturnURL = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ReturnURL").FirstOrDefault().Value;

        string sEncryptionKey = string.Empty;
        if (aoPaymentGateWayDetails.ProductInfo == Constants.VPMCPSProductInfo.VPMCPS_PP.ToString())
            sEncryptionKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKeyPP").FirstOrDefault().Value;        
        else
            sEncryptionKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;
        
        string sFinalString = "merchant_id=" + aoPaymentGateWayDetails.MerchantId + "&" +
                              "order_id=" + asTransactionId + "&" +
                              "currency=" + sCurrencyType + "&" +
                              "amount=" + asAmount + "&" +
                              "redirect_url=" + sReturnURL + "&" +
                              "cancel_url=" + sReturnURL + "&" +
                              "language=" + sLanguage + "&" +
                              "merchant_param1=" + msFirstName + "&" +
                              "merchant_param2=" + msPhone + "&" +
                              "merchant_param3=" + msEmail + "&" +
                              "merchant_param4=" + asGuid + "&" +
                              "merchant_param5=" + msRegNoOrFormNo;

        CCACrypto ccaCrypto = new CCACrypto();
        return ccaCrypto.Encrypt(sFinalString, sEncryptionKey);
    }
}

//public class PhiCommercePGRequest : Page
//{
//    #region "Data Member(s)"

//    private string msReturnURL;
//    private string msFirstName;
//    private string msEmail;
//    private string msPhone;

//    #endregion

//    #region "Constructor"

//    public PhiCommercePGRequest()
//    {
//    }

//    public PhiCommercePGRequest(string asReturnURL, string asFirstName, string asEmail, string asPhone)
//    {
//        this.msReturnURL = asReturnURL;
//        this.msFirstName = asFirstName;
//        this.msEmail = asEmail;
//        this.msPhone = asPhone;
//    }

//    #endregion "Constructor"

//    public void CalculateFeeAmount(DataRow adtRow, out Decimal dDeductedAmount, out Decimal dFormFees, Decimal adServiceTaxPercentage, Decimal adTotalAmt, PaymentConfirmationUI aoPaymentConfirmationUI)
//    {
//        decimal dFormFee;
//        decimal dDeductedAmt = 0, dServiceTaxAmount = 0;
//        decimal dRound = 0.00499999M;
//        dFormFee = adTotalAmt;
//        aoPaymentConfirmationUI.TxtFormFees.Text = dFormFee.ToString();
//        decimal dPerTransactionFeesAmountInApppliedPercent = adtRow["PerTransactionFeesAmountInPercent"].ToDecimal();
//        dDeductedAmt = dFormFee * (dPerTransactionFeesAmountInApppliedPercent / 100);
//        dServiceTaxAmount = dDeductedAmt * (adServiceTaxPercentage / 100);
//        dDeductedAmt = Decimal.Round((dDeductedAmt + dServiceTaxAmount) + dRound, 2);
//        aoPaymentConfirmationUI.LblAmtPerTransaction.Text = Decimal.Round(adtRow["PerTransactionFeesAmountInPercent"].ToDecimal() + dRound, 2).ToString() + "%";
//        aoPaymentConfirmationUI.TrServiceTax.Visible = true;
//        aoPaymentConfirmationUI.TrProcessChrges.Visible = true;
//        // Else, the transaction charges are in percentage.       
//        dDeductedAmount = dDeductedAmt;
//        dFormFees = dFormFee;
//    }

//    public Hashtable SendRequest(string asAmount, string asTransactionId, PaymentGateWayDetails aoPaymentGateWayDetails, string asPaymentFor)
//    {
//        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
//        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.PhiCommerce);
//        string sCurrencyCode = lstGatewayAdditionalDetails.Where(gt => gt.Name == "currencyCode").FirstOrDefault().Value;
//        string sPayType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "payType").FirstOrDefault().Value;
//        string sTransactionType = lstGatewayAdditionalDetails.Where(gt => gt.Name == "transactionType").FirstOrDefault().Value;

//        msReturnURL = msReturnURL + "PaymentStatusUI.Aspx";

//        Hashtable data = new Hashtable();

//        data["amount"] = asAmount;
//        data["currencyCode"] = sCurrencyCode;

//        //data["customerEmailId"] = msEmail;
//        //data["customerMobileNo"] = msPhone;
//        //data["customerName"] = msFirstName;

//        data["customerEmailId"] = "sachin.shinde@regulusit.net";
//        data["customerMobileNo"] = "8975688484";
//        data["customerName"] = "RIT";


//        data["merchantID"] = aoPaymentGateWayDetails.MerchantId;
//        data["merchantTxnNo"] = asTransactionId;
//        data["payType"] = sPayType;
//        data["returnURL"] = msReturnURL;
//        data["transactionType"] = sTransactionType;
//        data["txnDate"] = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

//        string sHash = asAmount + sCurrencyCode + "sachin.shinde@regulusit.net" + "8975688484" + "RIT" + aoPaymentGateWayDetails.MerchantId + asTransactionId + sPayType + msReturnURL + sTransactionType + data["txnDate"].ToString();

//        string sHashValue = GetHash(aoPaymentGateWayDetails, sHash);
//        data["secureHash"] = sHashValue;

//        HmacHelper.getsecurehash();

//        return data;
//    }

//    private string GetHash(PaymentGateWayDetails aoPaymentGateWayDetails, string asHash)
//    {
//        string sSecureHash = HmacHelper.GetHashValue(aoPaymentGateWayDetails.Hash, asHash, HMACTypes.HMAC_SHA256);
//        return sSecureHash.ToLower();
//    }

//    public static String sha256_hash(String value)
//    {
//        StringBuilder Sb = new StringBuilder();
//        using (SHA256 hash = SHA256Managed.Create())
//        {
//            Encoding enc = Encoding.UTF8;
//            Byte[] result = hash.ComputeHash(enc.GetBytes(value));
//            foreach (Byte b in result)
//                Sb.Append(b.ToString("x2"));
//        }
//        return Sb.ToString();
//    }

//    public string Encrypt(string input, string key)
//    {
//        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
//        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);
//        Aes kgen = Aes.Create("AES");
//        kgen.Mode = CipherMode.ECB;
//        kgen.Key = keyArray;
//        ICryptoTransform cTransform = kgen.CreateEncryptor();
//        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
//        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//    }

//}