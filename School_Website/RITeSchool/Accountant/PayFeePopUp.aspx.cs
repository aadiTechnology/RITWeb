// File Name  : PayFeePopUp.aspx.cs
// Created By : Anugandha
// Date       : 23 Sep 2008
// Description :This class is used to pay fee. 
// Modified By : Milind
// Date       : 11 Sep 2009
/* -------------------------------------------------------------------------------
 *  MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *	Author	: Vishal B. Shah
 *	Date	: 2-Jan-2012
 *	Purpose	: Modified to reflect payments in Accounts books.
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
 *	Author	: Pravin
 *	Date	: 24-Jun-13
 *	Purpose	: Modified to Merge pay fee and edit fee functionality.Given a facility
 *	          to pay partial fee across each fee payment.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SchoolEntities.StudentFee;
using Utility;
using PushNotificationService;
using PayrollReportingUserEntities;
using SchoolEntities;
using System.IO;

/// <summary>
/// This class is used to Pay and Edit fee payments for differernt modes.
/// </summary>
public partial class PayFeePopUp : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_SMS_TEMPLATE_NAME = "SmsTemplateName";
    private const string S_SMS_TEMPLATE_TEXT = "SmsTemplateText";
    private const string S_PAY_FEE = "Pay";
    private const string S_EDIT_FEE = "Edit";
    private const string S_ERR_MSG = "Cheque Number already exists for this student. Please enter another cheque number.";
    private const string S_ERR_MSG_SWAPNUMBER = "Transaction Number already exists for this student. Please enter another Transaction Number.";    
    private const string S_EDIT_MODE = "1";
    private const string S_PAY_MODE = "0";
    private const string S_CASH_PAYMENT = "Cash";
    private const string S_CHEQUE_PAYMENT = "Cheque";
    private const string S_CARD_PAYMENT = "SwapCard";
    private const string S_PDC_PAYMENT = "PDC";
    private const string S_ELECTRONIC_PAYMENT = "Electronic";
    private const int I_EXISTING_FEE_TYPE = -9998;
    private const int I_NEW_FEE_TYPE = -9999;
    private const int I_CHEQUE_STATUS = 2;
    private const string S_ATTACHMENT_FOLDER_LOCATION = "\\RITeSchool\\UPLOADS\\Fees\\PaymentDocuments\\";

    #endregion -- CONSTANT(s) --

    #region -- DATA MEMBER(s) --

    private string msPayFeeMode = string.Empty;
    private string msFeeAmount = Constants.S_ZERO;
    private bool mbIsLeftStudent;
  
    private StudentFeeDetailsBL moStudentFeeDetailsBL;
    
    #endregion -- DATA MEMBER(s) --

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
    /// This event is used to set default values as well to read querystring.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            if (Request.QueryString.IsNull() || Request.QueryString.ToString() == string.Empty)
                SetQueryString();

            CheckFinancialYearStatus();

            moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, QueryString["StudentId"].ToInt() , miUserId);
            if (!IsPostBack)
            {
                FollPaymentModes();
                CheckLoginUser();                
                SetDefaultDates();
                ReadQueryString();
                FillJVLedgerCombo();
                SetFeePaymentMode();        
                FillBankCombo();                
                GetReceiptNoToUpdate();
                SetAttributes();
                trPDCBank.Visible = IsAccountsModuleEnabled;
                cmbJVLedgers.Visible = IsAccountsModuleEnabled;
                if (IsAccountsModuleEnabled)
                    StoreFinancialYearDetails();
                else
                    cstAcDateValidator.EnableClientScript = false;
                SetPaymentSMSAcknowledgementStatus();
                FillStudentFeeDetails();
                SetConcessionMessage();                
                
            }
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
            SetPostBackElementId(sender as Control);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to pay student's fee.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            SetPostBackElementId(sender as Control);
            var oStudentChequeDetails = new StudentPostDatedChequesBL();
            int iActualAmount = txtActualAmt.Text.ToInt();
            string sReceiptNumber = string.Empty;
            bool IsDuplicate = false;
            StudentPayFeeDetails oStudentPayFeeDetails = PopulateStudentPayFeeDetails();
            string sStudentPayFeeDetailsXml = base.GenerateXml(oStudentPayFeeDetails);

            //Get XML string for extra debit or credit entry.
            string sCreditDetailsXML = GetXMLForCreditDetails();

            if (IsAccountsModuleEnabled && hidPaymentMode.Value == S_EDIT_MODE && hidReceiptNumber.Value != Constants.S_ZERO)
                DeleteVoucher(hidReceiptNumber.Value);



            if (chkFeePayment.SelectedValue == Constants.FeePaymentType.Cash.ToString())
            {

                moStudentFeeDetailsBL.Mode = hidPaymentMode.Value.ToInt();
                sReceiptNumber = moStudentFeeDetailsBL.PayStudentFeeWithCash(sStudentPayFeeDetailsXml, sCreditDetailsXML);                
                if (IsAccountsModuleEnabled && !chkDirectlyPaid.Checked && !sReceiptNumber.IsNullOrEmpty())
                    RecordCashPayment(hidStudentId.Value.ToInt(), sReceiptNumber);


                SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.Cash.ToString(), iActualAmount);                
            }
            else if (chkFeePayment.SelectedValue == Constants.FeePaymentType.JournalVoucher.ToString())
            {
                moStudentFeeDetailsBL.Mode = hidPaymentMode.Value.ToInt();
                sReceiptNumber = moStudentFeeDetailsBL.PayStudentFeeWithJournalVoucher(sStudentPayFeeDetailsXml, sCreditDetailsXML, cmbJVLedgers.SelectedValue.ToInt());
                if (IsAccountsModuleEnabled && !sReceiptNumber.IsNullOrEmpty())
                    RecordCashPayment(hidStudentId.Value.ToInt(), sReceiptNumber);
                
                SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.JournalVoucher.ToString(), iActualAmount);
            }
            else if (chkFeePayment.SelectedValue == Constants.FeePaymentType.Cheque.ToString())
            {
                if (hidPaymentMode.Value != S_EDIT_MODE && oStudentChequeDetails.IsChequeNoDuplicate(txtChequeNumber.Text, hidStudentId.Value.ToInt()))
                    IsDuplicate = true;
                if(!IsDuplicate)
                {
                    string sChequeDetailsXML = GetXMLForChequeDetails();
                    moStudentFeeDetailsBL.PayStudentFeeWithCheque(sStudentPayFeeDetailsXml, sChequeDetailsXML, sCreditDetailsXML);
                    SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.Cheque.ToString(), iActualAmount);                    
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = S_ERR_MSG;
                    txtChequeNumber.Focus();
                    return;
                }
            }
            else if (chkFeePayment.SelectedValue == Constants.FeePaymentType.PDC.ToString() || !tblChequeGrid.Visible)
            {
                string sChequeDetailsXML = GetXMLForChequeDetails();
                moStudentFeeDetailsBL.PayStudentFeeWithPDC(sStudentPayFeeDetailsXml, sChequeDetailsXML, sCreditDetailsXML);                
                SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.PDC.ToString(), iActualAmount);
            }
            else if (chkFeePayment.SelectedValue == Constants.FeePaymentType.SwapCard.ToString())
            {
                if (oStudentChequeDetails.IsSwapNoDuplicate(txtSwapNumber.Text, hidStudentId.Value.ToInt()) && hidPaymentMode.Value != S_EDIT_MODE)
                    IsDuplicate = true;
                if (!IsDuplicate)
                {
                    string sCardDetailsXML = GetXMLForCardDetails();
                    moStudentFeeDetailsBL.PayStudentFeeWithCard(sStudentPayFeeDetailsXml, sCardDetailsXML, sCreditDetailsXML);                    
                    SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.SwapCard.ToString(), iActualAmount);
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = S_ERR_MSG_SWAPNUMBER;
                    txtSwapNumber.Focus();
                    return;
                }
            }
            else if (chkFeePayment.SelectedValue == Constants.FeePaymentType.Electronic.ToString())
            {
               IsDuplicate = PayFeeWithElectronicMode(sStudentPayFeeDetailsXml, sCreditDetailsXML);
               if (!IsDuplicate)
                   return;
            }

            List<CreditDetails> lstCreditDetails = new List<CreditDetails>();
            lstCreditDetails = ViewState["CreditDetails"] as List<CreditDetails>;

            // If a new Fee type was chosen, we need to create a ledger for it!
            if (lstCreditDetails.Count > Constants.I_ZERO)
            {
                string sNewFeeType = lstCreditDetails.Where(fee => fee.IsNewlyAdded == true).Select(feetype => feetype.FeeType).FirstOrDefault();
                if (!sNewFeeType.IsNullOrEmpty())
                {
                    if (IsAccountsModuleEnabled)
                        CreateLedgerForNewFeeType(sNewFeeType.Trim());
                }
            }
			if (!Settings.IsMiniSite)
				SendSMS();
            var btnSender = sender as Button;

            if (btnSender.Text == btnPay.Text)
                SetQueryString();
            else
                PrintReceipt(hidStudentId.Value.ToInt());
        }
        catch (DuplicateName dnex)
        {
            lblErrorMsg.Text = dnex.Message;
            lblErrorMsg.Visible = true;
            txtChallanNo.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set late fee details to respected controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cal_PaymentDate_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            SetPaymentSMSAcknowledgementStatus();
            DateTime dtValidDate = System.DateTime.Now;
            SetPostBackElementId(sender as Control);
            if (DateTime.TryParse(txtPaymentDate.Text, out dtValidDate))
            {
                if (!QueryString["StudentFeeId"].IsNull())
                    hidStudentFeeIds.Value = QueryString["StudentFeeId"];
              
                FillStudentFeeDetails();                
                ResetAllAmounts();
                chkSelectStudentFee_Checked();
            }
            else
                cal_PaymentDate.DateValue = System.DateTime.Now;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to calcualate the actual amounts depending on Late fee and concession fee.
    /// </summary>
    private void ResetAllAmounts()
    {
        int iTotalPayableAmount = 0;
        int iTotalLateAmount = 0;
        txtLateFeeAmt.Text = "0";
        txtConcessionAmt.Text = "0";
        for (int iRowCount = 0; iRowCount < lstvwStudentFee.Items.Count - 2; iRowCount++)
        {
            ListViewDataItem oCurrentItem = lstvwStudentFee.Items[iRowCount];
            var chkFeeId = oCurrentItem.FindControl("chkSelect") as CheckBox;
            if (chkFeeId.Checked)
            {
                var lblAmountPayable = oCurrentItem.FindControl("lblAmountPayable") as Label;
                var lblLateFee = oCurrentItem.FindControl("lblLateFee") as Label;
                if (!lblLateFee.Text.IsNullOrEmpty())
                    iTotalLateAmount = iTotalLateAmount + lblLateFee.Text.ToInt();

                if (!lblAmountPayable.Text.IsNullOrEmpty())
                    iTotalPayableAmount = iTotalPayableAmount + lblAmountPayable.Text.ToInt();
            }
        }

        if (txtConcessionAmt.Text == string.Empty)
            txtConcessionAmt.Text = Constants.S_ZERO;
        if (txtLateFeeAmt.Text == string.Empty)
            txtLateFeeAmt.Text = Constants.S_ZERO;
        txtPayableAmt.Text = iTotalPayableAmount.ToString();
        txtLateFeeAmt.Text = iTotalLateAmount.ToString();
        txtAmtToBePaid.Text = (iTotalPayableAmount + iTotalLateAmount).ToString();
        txtActualAmt.Text = txtAmtToBePaid.Text;
        hidTotalActualAmt.Value = txtActualAmt.Text;
    }

    /// <summary>
    /// This event is used to set late fee details to respected controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtPaymentDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            SetPaymentSMSAcknowledgementStatus();
            DateTime dtValidDate = System.DateTime.Now;
            SetPostBackElementId(sender as Control);
            if (DateTime.TryParse(txtPaymentDate.Text, out dtValidDate))                
                FillStudentFeeDetails();
            else
                cal_PaymentDate.DateValue = System.DateTime.Now;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used on to set the selected fee payment type on the selection of radio button. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkFeePayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetPostBackElementId(sender as Control);
            hidPostbackControl.Value = (sender as Control).ClientID;
            if (chkFeePayment.SelectedValue == Constants.FeePaymentType.Cash.ToString())
                hidPaymentType.Value = Constants.FeePaymentType.Cash.ToInt().ToString();
            if (chkFeePayment.SelectedValue == Constants.FeePaymentType.Cheque.ToString())
                hidPaymentType.Value = Constants.FeePaymentType.Cheque.ToInt().ToString();
            if (chkFeePayment.SelectedValue == Constants.FeePaymentType.JournalVoucher.ToString())
                hidPaymentType.Value = Constants.FeePaymentType.JournalVoucher.ToInt().ToString();
            if (chkFeePayment.SelectedValue == Constants.FeePaymentType.PDC.ToString())
                hidPaymentType.Value = Constants.FeePaymentType.PDC.ToInt().ToString();            
            else
                hidPaymentType.Value = Constants.FeePaymentType.SwapCard.ToInt().ToString();

            SetDefaultBank();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check the checkbox depends on status of the fee type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdPostDatedCheque_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            for (int iCnt = 0; iCnt < grdPostDatedCheque.Rows.Count; iCnt++)
            {
                string sStatus = grdPostDatedCheque.DataKeys[iCnt][I_CHEQUE_STATUS].ToString();
                CheckBox chkPay = (CheckBox)grdPostDatedCheque.Rows[iCnt].Cells[Constants.I_ZERO].FindControl("ChkBoxPay");
                if (sStatus == "Paid")
                    chkPay.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to enable disable listview controls and set javascript to listview buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentFee_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                Label lblFeeType = oCurrentItem.FindControl("lblFeeType") as Label;
                Label lblAmount = oCurrentItem.FindControl("lblAmount") as Label;
                Label lblPaybleFor = oCurrentItem.FindControl("lblPaybleFor") as Label;
                Label lblDueDate = oCurrentItem.FindControl("lblDueDate") as Label;
                Label lblLateFee = oCurrentItem.FindControl("lblLateFee") as Label;
                Label lblAmountPayable = oCurrentItem.FindControl("lblAmountPayable") as Label;
                TextBox txtActualAmount = oCurrentItem.FindControl("txtActualAmount") as TextBox;
                DropDownList cmbFeeType = oCurrentItem.FindControl("cmbFeeType") as DropDownList;
                DropDownList cmbPayableFor = oCurrentItem.FindControl("cmbPayableFor") as DropDownList;
                HiddenField hidPreviousActualAmt = oCurrentItem.FindControl("hidPreviousActualAmt") as HiddenField;
                HiddenField hidConcessionAmount = oCurrentItem.FindControl("hidConcessionAmount") as HiddenField;

                if (Settings.SetParticularFeeRestriction && hidAllowPartialFee.Value != Constants.S_YES)
                    txtActualAmount.Enabled = false;

                int ReceiptNo = lstvwStudentFee.DataKeys[iRowId]["ReceiptNumberOutput"].ToInt();
                chkSelect.Attributes.Add("onclick", "CheckSelected(this,'" + iRowId + "')");
                txtActualAmount.Attributes.Add("onblur", "CalculateActualAmt(this,'" + iRowId + "')");

                int iStudentFeeId = lstvwStudentFee.DataKeys[iRowId]["SchoolwiseStudentFeeId"].ToInt();

                if (iStudentFeeId != I_EXISTING_FEE_TYPE && iStudentFeeId != I_NEW_FEE_TYPE)
                {
                    string[] sArrFeeId;
                    if (hidStudentFeeIds.Value != string.Empty)
                    {
                        sArrFeeId = hidStudentFeeIds.Value.Split(',');
                        for (int iCnt = 0; iCnt <= sArrFeeId.Length - 1; iCnt++)
                        {
                            if (sArrFeeId[iCnt].Trim() == iStudentFeeId.ToString())
                                chkSelect.Checked = true;
                        }
                    }

                    if (ReceiptNo != Constants.I_ZERO)
                    {
                        chkSelect.Checked = true;
                        hidStudentFeeIds.Value = hidStudentFeeIds.Value + "," + iStudentFeeId.ToString();
                    }

                    if (chkSelect.Checked)
                    {
                        txtActualAmount.Text = lblAmount.Text;                        
                        hidPreviousActualAmt.Value = txtActualAmount.Text;
                        hidTotalActualAmt.Value = (hidTotalActualAmt.Value.ToInt() + (lblAmountPayable.Text == Constants.S_ZERO ? lblAmount.Text.ToInt() : lblAmountPayable.Text.ToInt())).ToString();
                        txtConcessionAmt.Text = ((txtConcessionAmt.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(txtConcessionAmt.Text)) + hidConcessionAmount.Value.ToInt()).ToString();
                    }
                    else
                        txtActualAmount.Text = Constants.S_ZERO;

                    if (lblDueDate.Text.ToDateTime() < DateTime.Now && ReceiptNo == Constants.I_ZERO)
                    {
                        var tableRow = oCurrentItem.FindControl("trlstvwRow") as System.Web.UI.HtmlControls.HtmlTableRow;
                        tableRow.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FEEABA");
                        lblFeeType.ForeColor = Color.Red;
                        lblPaybleFor.ForeColor = Color.Red;
                        lblDueDate.ForeColor = Color.Red;
                        lblLateFee.ForeColor = Color.Red;
                        lblAmountPayable.ForeColor = Color.Red;
                        lblAmount.ForeColor = Color.Red;
                    }
                    else
                    {
                        var tableCell = oCurrentItem.FindControl("tdAmountPayable") as System.Web.UI.HtmlControls.HtmlTableCell;
                        tableCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, Color.PowderBlue.Name);
                    }
                }
                else if (iStudentFeeId ==I_NEW_FEE_TYPE )
                {
                    VisibleOrHideControls(oCurrentItem);
                    int iStandardId = Convert.ToInt32(hidStandardId.Value);
                    DataTable dtStdFeeType = moStudentFeeDetailsBL.GetStandardFeeType(miSchoolId, miAcademicYearId, iStandardId);
                    ControlUtility.FillDropDownList(dtStdFeeType, ref cmbFeeType, "SchoolWise_Standard_FeeType_Id", "Fee_Type", Constants.S_SELECT);                    
                    ListItem olstDivision = new ListItem();
                    olstDivision.Text = "--Select--";
                    cmbPayableFor.Items.Add(olstDivision);
                    var tableCell = oCurrentItem.FindControl("tdAmountPayable") as System.Web.UI.HtmlControls.HtmlTableCell;
                    tableCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, Color.PowderBlue.Name);
                }
                else if (iStudentFeeId == I_EXISTING_FEE_TYPE)
                {
                    VisibleOrHideControls(oCurrentItem);
                    var tableCell = oCurrentItem.FindControl("tdAmountPayable") as System.Web.UI.HtmlControls.HtmlTableCell;
                    tableCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, Color.PowderBlue.Name);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This method is used to load the payables for selected fee types.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int iCnt = lstvwStudentFee.Items.Count - 2; iCnt <= lstvwStudentFee.Items.Count - 2; iCnt++)
        {
            SetPostBackElementId(sender as Control);
            hidPostbackControl.Value = (sender as Control).ClientID;
            DropDownList cmbPayableFor = lstvwStudentFee.Items[iCnt].FindControl("cmbPayableFor") as DropDownList;
            DropDownList cmbFeeType = lstvwStudentFee.Items[iCnt].FindControl("cmbFeeType") as DropDownList;
            if (hidStudentFeeIds.Value.EndsWith(","))
                hidStudentFeeIds.Value.Remove(hidStudentFeeIds.Value.Length - 1);
            
            List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = moStudentFeeDetailsBL.GetIntervals(cmbFeeType.SelectedValue.ToInt(), "0", true);
            ListSource.FillDropDownList(lstStudentPaidFeeDetails, cmbPayableFor, "PayableFor", "PayableFor", Constants.S_SELECT);
        }
    }    

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to pay fee with electronic mode. Here we also check for the duplication of Txn no.
    /// </summary>
    /// <param name="asStudentPayFeeDetailsXml"></param>
    /// <param name="asCreditDetailsXML"></param>
    private bool PayFeeWithElectronicMode(string asStudentPayFeeDetailsXml,string asCreditDetailsXML)
    {
        if (!moStudentFeeDetailsBL.IsDuplicateElectronicTxn(txtSwapNumber.Text.Trim(), (Constants.PaymentMode)hidPaymentMode.Value.ToInt()))
        {
            string sElectronicPaymentXML = GetXMLForElectronicPaymentDetails();
            moStudentFeeDetailsBL.PayFeeWithElectronicMode(asStudentPayFeeDetailsXml, sElectronicPaymentXML, asCreditDetailsXML);
            SendFeePaymentAcknowledgementSMS(Constants.FeePaymentType.Electronic.ToString(), txtActualAmt.Text.ToInt());
            return true;
        }
        else
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = S_ERR_MSG_SWAPNUMBER;
            txtSwapNumber.Focus();
            return false;
        }
    }

    /// <summary>
    /// This method is used to Visible and hide controls on page load.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    private void VisibleOrHideControls(ListViewDataItem aoCurrentItem)
    {
        int iRowId = aoCurrentItem.DisplayIndex;
        Label lblFeeType = aoCurrentItem.FindControl("lblFeeType") as Label;
        Label lblAmount = aoCurrentItem.FindControl("lblAmount") as Label;
        Label lblPaybleFor = aoCurrentItem.FindControl("lblPaybleFor") as Label;
        Label lblDueDate = aoCurrentItem.FindControl("lblDueDate") as Label;
        Label lblLateFee = aoCurrentItem.FindControl("lblLateFee") as Label;
        Label lblAmountPayable = aoCurrentItem.FindControl("lblAmountPayable") as Label;
        TextBox txtActualAmount = aoCurrentItem.FindControl("txtActualAmount") as TextBox;
        DropDownList cmbFeeType = aoCurrentItem.FindControl("cmbFeeType") as DropDownList;
        DropDownList cmbPayableFor = aoCurrentItem.FindControl("cmbPayableFor") as DropDownList;
        TextBox txtDueDate = aoCurrentItem.FindControl("txtDueDate") as TextBox;
        RJS.Web.WebControl.PopCalendar calDueDate = aoCurrentItem.FindControl("calDueDate") as RJS.Web.WebControl.PopCalendar;
        int iStudentFeeId = lstvwStudentFee.DataKeys[iRowId]["SchoolwiseStudentFeeId"].ToInt();

        lblFeeType.Visible = false;
        lblPaybleFor.Visible = false;
        lblAmountPayable.Visible = false;
        lblDueDate.Visible = false;
        lblLateFee.Visible = false;
        lblAmount.Visible = false;
        txtDueDate.Visible = true;
        calDueDate.Visible = true;
        txtActualAmount.Enabled = false;
        txtActualAmount.Text = Constants.S_ZERO;

        if (iStudentFeeId == I_NEW_FEE_TYPE)
        {
            cmbFeeType.Visible = true;
            cmbPayableFor.Visible = true;
        }
        else
        {
            TextBox txtNewFeeType = aoCurrentItem.FindControl("txtNewFeeType") as TextBox;
            TextBox txtNewPayableFor = aoCurrentItem.FindControl("txtNewPayableFor") as TextBox;
            cmbFeeType.Visible = false;
            cmbPayableFor.Visible = false;
            txtNewFeeType.Visible = true;
            txtNewPayableFor.Visible = true;
        }

     }

    /// <summary>
    /// This method is used send sms/Notification to fee payment acknowledgement.
    /// </summary>
    /// <param name="asPaymentMode"></param>
    /// <param name="aiAmount"></param>
    private void SendFeePaymentAcknowledgementSMS(string asPaymentMode, int aiAmount)
    {
        //SendPushNotification(hidStudentId.Value.ToString(), txtActualAmt.Text.ToString());
        if (chkPaymentSMSAcknowledgement.Checked && !moStudentFeeDetailsBL.MobileNumber.IsNullOrEmpty())
        {
            Hashtable oHTUsersMobileNo = new Hashtable();
            string[] sArrMobileNumbers = moStudentFeeDetailsBL.MobileNumber.Split(',');
            string sSmsText = string.Empty;
            string sTemplateRegistrationId = string.Empty;
            
            oHTUsersMobileNo[moStudentFeeDetailsBL.FeeDefaulterUserId] = sArrMobileNumbers[0].Trim();
            if (sArrMobileNumbers.Length > Constants.I_ONE && !sArrMobileNumbers[1].Trim().IsNullOrEmpty() && sArrMobileNumbers[0].Trim() != sArrMobileNumbers[1].Trim())
                oHTUsersMobileNo[moStudentFeeDetailsBL.FeeDefaulterUserId + "sm;"] = sArrMobileNumbers[1].Trim();
            string sSmsSubject = string.Empty;
            string sDisplayText = hidStudentName.Value;
           
            int iSmsId = Constants.SMSTemplate.FeePaymentAcknowledgementSMS.ToInt();
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            FeeSMS oFeeSMS = moStudentFeeDetailsBL.GetPayableAmount(hidStudentId.Value.ToInt(), miSchoolId, miAcademicYearId);
           
             if (oDTSmsTemplate.Rows.Count != 0)
            {
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                {
                    string sHSPSMSText = "We have successfully received your fee payment(%TERMNAME%) of Rs. %AMOUNT% through %PAYMENTMODE%. Regards, HIS Pune.";
                    sSmsText = Convert.ToString(oDTSmsTemplate.Rows[Constants.I_ZERO][S_SMS_TEMPLATE_TEXT]);

                    if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    if (asPaymentMode == "Cheque" || asPaymentMode == "PDC")
                    {
                        string sChequePaymentAcknowledgementSMS = Constants.SMSTemplate.ChequePaymentSMS.ToString();
                      
                        DataTable dt = SmsTemplateBL.GetTemplate(sChequePaymentAcknowledgementSMS, miSchoolId);
                        if (dt.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                            sTemplateRegistrationId = dt.Rows[0]["TemplateRegistrationId"].ToString();

                        sSmsText = Convert.ToString(dt.Rows[Constants.I_ZERO][S_SMS_TEMPLATE_TEXT]);

                        //In case of specific school, they want to send balance amount in sms text. So it is managed on setting table flag and added in sms text. 
                        if (SchoolBase.Settings.DisplayBalanceAmountInPaymentAcknowledgementSMS == true)
                            sSmsText = sSmsText.Replace("%PaymentMode%.", asPaymentMode + ". Balance Payment is Rs. " + oFeeSMS.PayableAmount.ToString() + "/-.").Replace("%Amount%", aiAmount.ToString() + "/-") + " *Subject To Cheque Realization.";
                        else if (miSchoolId == Constants.SchoolId.HSP.ToInt())
                            sSmsText = sHSPSMSText.Replace("%PAYMENTMODE%.", asPaymentMode).Replace("%AMOUNT%", aiAmount.ToString() + "/-").Replace("%TERMNAME%", oFeeSMS.Term);
                        else if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                            sSmsText = sSmsText.Replace("%PaymentMode%", asPaymentMode).Replace("%Amount%", aiAmount.ToString() + "/-");
                        else
                            sSmsText = sSmsText.Replace("%PaymentMode%", asPaymentMode).Replace("%Amount%", aiAmount.ToString() + "/-") + " *Subject To Cheque Realization.";
                    }
                    else
                    {
                        //In case of specific school, they want to send balance amount in sms text. So it is managed on setting table flag and added in sms text. 
                        if (SchoolBase.Settings.DisplayBalanceAmountInPaymentAcknowledgementSMS == true)
                            sSmsText = sSmsText.Replace("%PaymentMode%.", asPaymentMode + ". Balance Payment is Rs. " + oFeeSMS.PayableAmount.ToString() + "/-.").Replace("%Amount%", aiAmount.ToString() + "/-");
                        else if (miSchoolId == Constants.SchoolId.HSP.ToInt())
                            sSmsText = sHSPSMSText.Replace("%PAYMENTMODE%.", asPaymentMode).Replace("%AMOUNT%", aiAmount.ToString() + "/-").Replace("%TERMNAME%", oFeeSMS.Term);
                        else if (asPaymentMode == "JournalVoucher")
                            sSmsText = sSmsText.Replace("%PaymentMode%", cmbJVLedgers.SelectedItem.Text+" adjustment").Replace("%Amount%", aiAmount.ToString() + "/-");
                        else
                            sSmsText = sSmsText.Replace("%PaymentMode%", asPaymentMode).Replace("%Amount%", aiAmount.ToString() + "/-");
                       
                    }

                    sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[Constants.I_ZERO][S_SMS_TEMPLATE_NAME]);
                }
            }
            

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            SMS oSMS = new SMS();
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.DisplayText = sDisplayText;
            oSMS.SchoolID = miSchoolId;
            oSMS.AcademicYearID = miAcademicYearId;
            oSMS.To = oHTUsersMobileNo;
            int iCount = oSMS.Send();
            oHTUsersMobileNo.Clear();
        }
    }

    /// <summary>
    /// This mentod is used to set chkPaymentSMSAcknowledgement check box check if date is current otherwise false.
    /// </summary>
    private void SetPaymentSMSAcknowledgementStatus()
    {
        string dtCurrentDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
        if (dtCurrentDate == txtPaymentDate.Text)
            chkPaymentSMSAcknowledgement.Checked = true;
        else
            chkPaymentSMSAcknowledgement.Checked = false;

        if (Settings.IsAaryanSchool)
            chkPaymentSMSAcknowledgement.Checked = false;
    }

    /// <summary>
    /// This is a common function for all fee types which returns the XML to pay and edit fee.
    /// </summary>
    /// <returns></returns>
    private StudentPayFeeDetails PopulateStudentPayFeeDetails()
    {
        List<StudentFeeDetails> lstStudentFeesList = GetSelectedStudentFees();
        StudentLateFeeDetails oStudentLateFeeDetails = InitializeLateFeeDetails();
        StudentPayFeeDetails oStudentPayFeeDetails = InitiallizePayFeeDetails(lstStudentFeesList, oStudentLateFeeDetails);
        string sFeePaymentType = chkFeePayment.SelectedValue;
        if (sFeePaymentType.IsNullOrEmpty())
            sFeePaymentType = S_PDC_PAYMENT;
        switch (sFeePaymentType)
        {
            case S_CASH_PAYMENT :
                moStudentFeeDetailsBL.sChallanNumber = txtChallanNo.Text.Trim();
                int iBankId = ddlBankNameDirectlyPaid.SelectedValue.ToInt();
                if (chkDirectlyPaid.Checked)
                    moStudentFeeDetailsBL.sChallanNumber = txtChallanNo.Text.Trim();

                if (IsAccountsModuleEnabled && chkDirectlyPaid.Checked)
                {
                    iBankId = GetBankIdForLedger(iBankId);
                    oStudentPayFeeDetails.BankId = iBankId;
                }

                oStudentPayFeeDetails.BankId = 0;

                break;

            case S_CHEQUE_PAYMENT :
                oStudentPayFeeDetails.DepositeBankId = string.IsNullOrEmpty(ddlAcChqBank.SelectedValue) || ddlAcChqBank.SelectedValue == Constants.S_ZERO
                                               ? Constants.I_ZERO
                                               : ddlAcChqBank.SelectedValue.ToInt();
                break;

            case S_PDC_PAYMENT :
                oStudentPayFeeDetails.DepositeBankId= string.IsNullOrEmpty(ddlAcPDCBank.SelectedValue) || ddlAcPDCBank.SelectedValue == Constants.S_ZERO
                                                ? Constants.I_ZERO
                                                : ddlAcPDCBank.SelectedValue.ToInt();
                break;
            
            case S_ELECTRONIC_PAYMENT :
            case S_CARD_PAYMENT :
                 oStudentPayFeeDetails.BankId = string.IsNullOrEmpty(ddlBankNameCard.SelectedValue) || ddlBankNameCard.SelectedValue == Constants.S_ZERO
                                                 ? Constants.I_ZERO
                                                 : ddlBankNameCard.SelectedValue.ToInt();
                 oStudentPayFeeDetails.DepositeBankId = string.IsNullOrEmpty(ddlAcCardBank.SelectedValue) || ddlAcCardBank.SelectedValue == Constants.S_ZERO
                                                     ? Constants.I_ZERO
                                                     : ddlAcCardBank.SelectedValue.ToInt();
                break;   
        }               

        return oStudentPayFeeDetails;
    }

    /// <summary>
    /// This function is used to fill the Student's selected fee details which is used to generate the XML.
    /// </summary>
    /// <returns></returns>
    private List<StudentFeeDetails> GetSelectedStudentFees()
    {
        List<StudentFeeDetails> lstStudentFeesList = new List<StudentFeeDetails>();
        lstStudentFeesList = (from currentItem in lstvwStudentFee.Items
                              let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                              let iSchoolwiseStudentFeeId = lstvwStudentFee.DataKeys[currentItem.DisplayIndex]["SchoolwiseStudentFeeId"]
                              let lblAmountPayable = currentItem.FindControl("lblAmountPayable") as Label
                              let txtActualAmount = currentItem.FindControl("txtActualAmount") as TextBox
                              let lblLateFee = currentItem.FindControl("lblLateFee") as Label
                              where chkSelect.Checked && iSchoolwiseStudentFeeId.ToInt() != I_EXISTING_FEE_TYPE && iSchoolwiseStudentFeeId.ToInt() != I_NEW_FEE_TYPE
                              select new StudentFeeDetails
                              {
                                  StudentFeeId = iSchoolwiseStudentFeeId.ToInt(),
                                  PaybleAmount = lblAmountPayable.Text.ToInt(),
                                  ActualAmount = txtActualAmount.Text.ToInt(),
                                  LateFee = lblLateFee.Text.ToInt(),
                              }).ToList();

        return lstStudentFeesList;
    }

    /// <summary>
    /// This function is used to initialize the Late fee details of the student.
    /// </summary>
    /// <returns></returns>
    private StudentLateFeeDetails InitializeLateFeeDetails()
    {
        StudentLateFeeDetails oStudentLateFeeDetails = new StudentLateFeeDetails
        {
            TotalLateFeeAmount = txtLateFeeAmt.Text.IsNullOrEmpty() ? Constants.I_ZERO : txtLateFeeAmt.Text.Trim().ToInt(),
            LateFeeDescription = hidLateFeeDesc.Value
        };
        return oStudentLateFeeDetails;
    }

    /// <summary>
    /// This method is used to initialize all the payfeedetails.
    /// </summary>
    /// <param name="alstStudentFeesList"></param>
    /// <param name="aoStudentLateFeeDetails"></param>
    /// <returns></returns>
    private StudentPayFeeDetails InitiallizePayFeeDetails(List<StudentFeeDetails> alstStudentFeesList, StudentLateFeeDetails aoStudentLateFeeDetails)
    {
        string asFileName = SaveFileToServer();
        StudentPayFeeDetails oStudentPayFeeDetails = new StudentPayFeeDetails
        {
            StudentId = hidStudentId.Value.ToInt(),
            FileName = asFileName,
            AmountToBePaid = txtAmtToBePaid.Text.ToInt(),            
            ActualAmount = hidPDCActualAmount.Value.ToInt(),
            PaymentDate = txtPaymentDate.Text.ToDateTime(),
            ConcessionAmount = txtConcessionAmt.Text == string.Empty ? Constants.I_ZERO : txtConcessionAmt.Text.ToInt(),
            ActualLateFeeAmount = !hidActualLateFeeAmt.Value.IsNullOrEmpty() ? hidActualLateFeeAmt.Value.ToInt() : Constants.I_ZERO,
            ReceiptNumberOutput = string.IsNullOrEmpty(cmbReceiptNo.SelectedValue) || cmbReceiptNo.SelectedValue == Constants.S_ZERO
                                  ? Constants.I_ZERO
                                  : Convert.ToInt32(cmbReceiptNo.SelectedValue.TrimStart('0')),
            IsDirectlyDeposited = chkDirectlyPaid.Checked,
            ChallanNumber = txtChallanNo.Text.Trim(),          
            DepositeBankId = string.IsNullOrEmpty(ddlBankNameDirectlyPaid.SelectedValue) || ddlBankNameDirectlyPaid.SelectedValue == Constants.S_ZERO
                     ? Constants.I_ZERO
                     : ddlBankNameDirectlyPaid.SelectedValue.ToInt(),            

            Remarks = hidRemarks.Value.Length > 2000 ? hidRemarks.Value.Substring(0, 1998) + ".." : hidRemarks.Value,
            AdditionalRemark = txtAdditionalRemark.Text.Trim(),
            lstStudentFeeList = alstStudentFeesList,
            oLateFeeDetails = aoStudentLateFeeDetails,
            FinancialYearId = miFinancialYearId,
            IsCautionMoneyAdjusted = ChkPaymentCautionMoneyAdjusted.Checked
        };
        return oStudentPayFeeDetails;
    }

    /// <summary>
    /// This method returns the extra added fee type for the transaction.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForCreditDetails()
    {
        List<CreditDetails> lstCreditDetails = new List<CreditDetails>();
        foreach (ListViewDataItem oListViewDataItem in lstvwStudentFee.Items)
        {
            CheckBox chkSelect = oListViewDataItem.FindControl("chkSelect") as CheckBox;
            int iSchoolwiseStudentFeeId = lstvwStudentFee.DataKeys[oListViewDataItem.DisplayIndex]["SchoolwiseStudentFeeId"].ToInt();
            CreditDetails oCreditDetails = new CreditDetails();
            if (chkSelect != null && chkSelect.Checked && (iSchoolwiseStudentFeeId == I_EXISTING_FEE_TYPE || iSchoolwiseStudentFeeId == I_NEW_FEE_TYPE))
            {
                TextBox txtActualAmount = oListViewDataItem.FindControl("txtActualAmount") as TextBox;
                TextBox txtDueDate = oListViewDataItem.FindControl("txtDueDate") as TextBox;
                if (txtActualAmount.Text.ToInt() > Constants.I_ZERO)
                {
                    if (iSchoolwiseStudentFeeId == I_NEW_FEE_TYPE)
                    {
                        DropDownList cmbFeeType = oListViewDataItem.FindControl("cmbFeeType") as DropDownList;
                        DropDownList cmbPayableFor = oListViewDataItem.FindControl("cmbPayableFor") as DropDownList;
                        oCreditDetails.FeeType = cmbFeeType.SelectedItem.Text;
                        oCreditDetails.PayableFor = cmbPayableFor.SelectedItem.Text;
                        oCreditDetails.IsNewlyAdded = false;
                    }
                    else
                    {
                        TextBox txtNewFeeType = oListViewDataItem.FindControl("txtNewFeeType") as TextBox;
                        TextBox txtNewPayableFor = oListViewDataItem.FindControl("txtNewPayableFor") as TextBox;
                        oCreditDetails.FeeType = txtNewFeeType.Text.Trim();
                        oCreditDetails.PayableFor = txtNewPayableFor.Text.Trim();
                        oCreditDetails.IsNewlyAdded = true;
                    }

                    oCreditDetails.ChequeDate = txtDueDate.Text.ToDateTime();
                    oCreditDetails.CreditedAmount = txtActualAmount.Text.ToInt();
                    oCreditDetails.StdFeeTypeId = iSchoolwiseStudentFeeId;
                    lstCreditDetails.Add(oCreditDetails);
                }
            }
        }

        ViewState["CreditDetails"] = lstCreditDetails;
        return base.GenerateXml(lstCreditDetails);
    }
   

    /// <summary>
    /// This function is used to set javascript attributes.
    /// </summary>
    private void SetAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnPay, btnPayAndPrint, btnClose });
        chkDirectlyPaid.Attributes["onclick"] = "javascript:EnableControlsDirectlyPaid();";
        valErrMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        string sQueryString = "StudentId=" + hidStudentId.Value;
        string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        hidQueryString.Value = "?" + sEncryptQueryString + string.Empty;
        btnClose.Attributes.Add("onclick", "CloseWindow()");
        txtAmtToBePaid.Attributes.Add("readonly", "readonly");
        hidDefaultFeeType.Value = Settings.DefaultFeeType;        
        if (Settings.IsMaxFeeApplicable)
            hidApplicableMaxLateFee.Value = Settings.MaxFee.ToString();

        if (miSchoolId == Constants.SchoolId.ZLSP.ToInt())
            btnPayAndPrint.Visible = false;

        hidBaseFinancialYearId.Value = miFinancialYearId.ToString();
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        txtConcessionAmt.Text = Constants.S_ZERO;        
        // Set standardid to hidden field taking from querystring.
        if (!QueryString["StandardId"].IsNull())
            hidStandardId.Value = QueryString["StandardId"];

        if (!QueryString["PayBtn"].IsNull())
            msPayFeeMode = QueryString["PayBtn"];

        hidMode.Value = msPayFeeMode;

        if (!QueryString["AmtToBePaid"].IsNull())
            msFeeAmount = QueryString["AmtToBePaid"];

        hidStudentId.Value = QueryString["StudentId"] ?? Constants.S_ZERO;
        hidAccountHeaderId.Value = QueryString["AccountHeaderId"] ?? Constants.S_ZERO;

        if (!QueryString["TotalAmt"].IsNull())
            hidTotalAmount.Value = QueryString["TotalAmt"];

        if (!QueryString["StudentName"].IsNull())
            hidStudentName.Value = QueryString["StudentName"].ToString();
    }

    /// <summary>
    /// This method is used to select payment modes depends on cases.
    /// </summary>
    private void SetFeePaymentMode()
    {
        switch (msPayFeeMode)
        {
            case S_PAY_FEE :
                SetPayFeeMode();
                FillChequeDetailsGrid();              
                SetLateFeeDetails();                
                FillCardTypeCombo();
                FillElectronicPaymentTypes();
                break;

            case S_EDIT_FEE :
                imgbtnView.Visible = true;
                SetEditFeeMode();                
                hidReceiptNumber.Value = QueryString["ReceiptNo"];                
                GetPaymentTypeForReceipt();
                FillChequeDetailsGrid();
                FillCardTypeCombo();
                FillElectronicPaymentTypes();
                break;

            default :
                SetPDCViewMode(msFeeAmount);
                break;
        }        
    }

    /// <summary>
    /// This method is used to get the payment mode for selected transaction on edit.
    /// </summary>
    private void GetPaymentTypeForReceipt()
    {
        int iAccountHeaderId = 0;
        if (QueryString["AccountHeaderId"] != null && QueryString["AccountHeaderId"].ToString() != string.Empty)
            iAccountHeaderId = QueryString["AccountHeaderId"].ToInt();

        Constants.FeePaymentType oPaymentType = moStudentFeeDetailsBL.GetPaymentModeForReceipt(hidReceiptNumber.Value, iAccountHeaderId);
        hidPaymentType.Value = oPaymentType.ToInt().ToString();
        chkFeePayment.Items[oPaymentType.ToInt()].Selected = true;
        txtPaymentDate.Text = moStudentFeeDetailsBL.PaymentDate.ToString("dd-MMM-yyyy");
    }   
    
    /// <summary>
    /// This method is used to set PayFeeMode on page load.
    /// </summary>
    private void SetPayFeeMode()
    {
        tblChequeGrid.Visible = true;
        cal_CDate.DateValue = DateTime.Today;        
        hidPaymentType.Value = Constants.FeePaymentType.Cheque.ToInt().ToString();
        hidPaymentMode.Value = S_PAY_MODE;
        chkFeePayment.SelectedValue = Constants.FeePaymentType.Cheque.ToInt().ToString();
        chkFeePayment.Items[Constants.FeePaymentType.Cheque.ToInt()].Selected = true;
        txtPayableAmt.Text = msFeeAmount;
        txtActualAmt.Text = msFeeAmount;
        txtAmtToBePaid.ReadOnly = true;

        if (!QueryString["StudentFeeId"].IsNull())
            hidStudentFeeIds.Value = QueryString["StudentFeeId"];
    }

    /// <summary>
    /// This method is used to call to set EditFeeMode on page load.
    /// </summary>
    private void SetEditFeeMode()
    {
        tblChequeGrid.Visible = true;
        cal_CDate.DateValue = DateTime.Today;
        hidPaymentMode.Value = S_EDIT_MODE;
    }

    /// <summary>
    /// This method is used to set the view mode if the payment is selected for PDC from button given on base screen.
    /// </summary>
    private void SetPDCViewMode(string asAmountToBePaid)
    {
        if (!QueryString["PDC_Id"].IsNull())
            hidPDCId.Value = QueryString["PDC_Id"];
        hidPDCAmount.Value = asAmountToBePaid;
        hidPaymentType.Value = Constants.FeePaymentType.PDC.ToInt().ToString();
        tblFeesToBePaid.Visible = true;
        txtActualAmt.Text = Constants.S_ZERO;
        txtLateFeeAmt.Text = Constants.S_ZERO;
        txtLateFeeAmt.ReadOnly = true;
        txtPayableAmt.Text = Constants.S_ZERO;
        txtLateFeeAmt.Enabled = false;
        txtActualAmt.ReadOnly = true;
        chkDirectlyPaid.Visible = false;
        trChallanNoRow.Visible = false;
        trBankName.Visible = false;
    }

    /// <summary>
    /// This method is used to send the SMS to Activated user.
    /// </summary>
    public void SendSMS()
    {
        if (moStudentFeeDetailsBL.CanSendSMS.ToBool() && !moStudentFeeDetailsBL.MobileNumber.IsNullOrEmpty())
        {
            Hashtable oHTUsersMobileNo = new Hashtable();
            string[] sArrMobileNumbers = moStudentFeeDetailsBL.MobileNumber.Split(',');
            string sTemplateName = string.Empty;
            string sSmsText = string.Empty;
            string sTemplateRegistrationId = string.Empty;

            oHTUsersMobileNo[moStudentFeeDetailsBL.FeeDefaulterUserId] = sArrMobileNumbers[0].Trim();

            if (sArrMobileNumbers.Length > Constants.I_ONE && !sArrMobileNumbers[1].Trim().IsNullOrEmpty() && sArrMobileNumbers[0].Trim() != sArrMobileNumbers[1].Trim())
                oHTUsersMobileNo[moStudentFeeDetailsBL.FeeDefaulterUserId + "sm;"] = sArrMobileNumbers[1].Trim();

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
            oSMS.DisplayText = hidStudentName.Value + ' ' + moStudentFeeDetailsBL.Designation;
            oSMS.SchoolID = miSchoolId;
            oSMS.AcademicYearID = miAcademicYearId;
            oSMS.To = oHTUsersMobileNo;
            int iCount = oSMS.Send();
            oHTUsersMobileNo.Clear();
        }
    }

    /// <summary>
    /// Serializes the FinancialYearMaster entity object to a hidden field.
    /// </summary>
    private void StoreFinancialYearDetails()
    {
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
    /// Populates the ReceiptNo dropdown list with those receipt nos which have been deleted.
    /// </summary>
    private void GetReceiptNoToUpdate()
    {
        DataTable oDTReceiptNumberToUpdate = moStudentFeeDetailsBL.GetReceiptNoToUpdate(hidPaymentMode.Value.ToInt());
        if (oDTReceiptNumberToUpdate != null && oDTReceiptNumberToUpdate.Rows.Count > 0)
        {
            trReceiptNo.Visible = true;
            int iReceiptMinimumDigits = Settings.ReceiptMinimumDigits;
            foreach (DataRow row in oDTReceiptNumberToUpdate.Rows)
            {
                string sReceiptNumber = row["Receipt_Number"].ToString();
                if (sReceiptNumber.Length < iReceiptMinimumDigits)
                    row["Receipt_Number"] = sReceiptNumber.PadLeft(iReceiptMinimumDigits, '0');
            }

            ControlUtility.FillDropDownList(oDTReceiptNumberToUpdate, ref cmbReceiptNo, "Receipt_Number", "Receipt_Number", Constants.S_SELECT);
            cmbReceiptNo.Focus();
        }
        else
            trReceiptNo.Visible = false;
    }

    /// <summary>
    /// This method is used to fill postdated cheque details grid for particular student.
    /// </summary>
    private void FillChequeDetailsGrid()
    {
        SetGridViewDateColumnProperties();
        int iStudentId = Convert.ToInt32(hidStudentId.Value);
        StudentPostDatedChequesBL oChequeDetails = new StudentPostDatedChequesBL();
        DataSet oDsChequeDetails = oChequeDetails.GetStudentChequeDetails(iStudentId);
        if (hidPaymentMode.Value == S_EDIT_MODE)
            grdPostDatedCheque.DataSource = oDsChequeDetails.Tables[Constants.I_ZERO].DefaultView;
        else
            grdPostDatedCheque.DataSource = oDsChequeDetails.Tables[Constants.I_ONE].DefaultView;
        grdPostDatedCheque.DataBind();
    }

    /// <summary>
    /// This function sets the date format for date column property.
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        const int I_COLUMN_INDEX_CHEQUE_DATE = 2;
        BoundField oChequeDate = (BoundField)grdPostDatedCheque.Columns[I_COLUMN_INDEX_CHEQUE_DATE];
        oChequeDate.HtmlEncode = false;
        oChequeDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }
    
    /// <summary>
    /// This method is used to fill the student debit details in the listview.
    /// </summary>
    private void FillStudentFeeDetails()
    {
        FillStudentFeeListview();
        SetEditFeeDetails();
        if (tblChequeGrid.Visible && !hidPaymentType.Value.IsNullOrEmpty() && hidPaymentType.Value.ToInt() == Constants.FeePaymentType.Cheque.ToInt() && hidPaymentMode.Value == S_EDIT_MODE)
            SetChequeDetails();
        if (!hidPaymentType.Value.IsNullOrEmpty() && hidPaymentType.Value.ToInt() == Constants.FeePaymentType.SwapCard.ToInt() && hidPaymentMode.Value == S_EDIT_MODE)
            SetSwapCardDetails();
        if (!hidPaymentType.Value.IsNullOrEmpty() && hidPaymentType.Value.ToInt() == Constants.FeePaymentType.Electronic.ToInt() && hidPaymentMode.Value == S_EDIT_MODE)
            SetElectronicPaymentDetails();
    }

    /// <summary>
    /// This method is used to set the default bank selection.
    /// </summary>
    private void SetDefaultBank()
    {
        if (hidMode.Value == S_PAY_FEE || hidMode.Value == "GridPay")
        {
            ddlAcCardBank.SelectedValue = hidDefaultBank.Value;
            ddlAcChqBank.SelectedValue = hidDefaultBank.Value;
            ddlBankNameDirectlyPaid.SelectedValue = hidDefaultBank.Value;
            ddlAcPDCBank.SelectedValue = hidDefaultBank.Value;            
        }
    }

    /// <summary>
    /// This method is used to fill student fee listview.
    /// </summary>
    private void FillStudentFeeListview()
    {
        DateTime dtCurrentDate = DateTime.Now;
        if (DateTime.TryParse(txtPaymentDate.Text, out dtCurrentDate))
            dtCurrentDate = txtPaymentDate.Text.ToDateTime();
        List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = moStudentFeeDetailsBL.GetStudentFeeDetails(dtCurrentDate, hidPaymentType.Value.ToInt(), true, hidReceiptNumber.Value.ToInt());
        List<StudentPayFeeDetails> lstStudentPayFeeDetails = moStudentFeeDetailsBL.StudentPayFeeDetails;        
        var oStudentDebitDetails = from Paid in lstStudentPaidFeeDetails
                                   join Pay in lstStudentPayFeeDetails
                                   on Paid.SchoolwiseStudentFeeId equals Pay.SchoolwiseStudentFeeId
                                   select new
                                   {
                                       Paid.SchoolwiseStudentFeeId,
                                       Paid.PayableFor,
                                       Paid.Amount,
                                       Paid.FeeType,
                                       Paid.AmountPayable,
                                       Paid.DebitOrCredit,
                                       Paid.LateFeeAmount,
                                       Paid.SerialNumber,
                                       Paid.StandardwiseFeeTypeId,
                                       Pay.PaymentDate,
                                       Pay.ReceiptNumberOutput,
                                       Paid.ConcessionAmount,
                                       Paid.AccountHeaderId,
                                       Paid.FileName
                                   };

        lstvwStudentFee.DataSource = oStudentDebitDetails;
        lstvwStudentFee.DataBind();
		hidLastChequeBank.Value = moStudentFeeDetailsBL.LastChequeBank.ToString();

        if (moStudentFeeDetailsBL.StudentPayFeeDetails[0].RemainingCautionMoney > 0)
        {
            trCaution.Visible = true;
            hidRemaingCautionMoneyAmount.Value = moStudentFeeDetailsBL.StudentPayFeeDetails[0].RemainingCautionMoney.ToString();
            ChkPaymentCautionMoneyAdjusted.Text = "Adjuste Payment From Caution Money(Rs. " + moStudentFeeDetailsBL.StudentPayFeeDetails[0].RemainingCautionMoney + ")?";
        }
        else
        {
            trCaution.Visible = false;
            hidRemaingCautionMoneyAmount.Value = Constants.S_ZERO;

        }
    }

    /// <summary>
    /// This method is used to set the details to the controls on page load.
    /// </summary>
    private void SetEditFeeDetails()
    {
        EditFeeDetails oEditFeeDetails = moStudentFeeDetailsBL.EditFeeDetails;
        if (!oEditFeeDetails.IsNull())
        {
            if (hidPaymentMode.Value == S_EDIT_MODE)
                FillReceiptsCombo();

            int iPaybleAmount = oEditFeeDetails.Payble == 0 ? oEditFeeDetails.AmountPaid : oEditFeeDetails.Payble;
            int iLateFeeAmount = oEditFeeDetails.PaidLateFee;
            int iConcessionFeeAmount = oEditFeeDetails.Concession;
            txtPayableAmt.Text = oEditFeeDetails.Payble.ToString();
            txtLateFeeAmt.Text = oEditFeeDetails.PaidLateFee.ToString();
            txtConcessionAmt.Text = oEditFeeDetails.Concession.ToString();

            if (string.IsNullOrEmpty(oEditFeeDetails.FileName))
            {
                imgbtnView.Visible = false;
                hidFileUpload.Value = string.Empty;
            }
            else
            {
                imgbtnView.Visible = true;
                string sPath = "../Uploads/Fees/PaymentDocuments/" + oEditFeeDetails.FileName;
                imgbtnView.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                hidFileUpload.Value = oEditFeeDetails.FileName;
            }
            
            trCaution.Visible = oEditFeeDetails.IsCautionMoneyAdjusted;

            txtAmtToBePaid.Text = (iPaybleAmount + iLateFeeAmount - iConcessionFeeAmount).ToString();
            txtActualAmt.Text = (iPaybleAmount + iLateFeeAmount - iConcessionFeeAmount).ToString();
            chkDirectlyPaid.Checked = oEditFeeDetails.oStudentPayFeeDetails.IsDirectlyDeposited;
            if (!hidPaymentType.Value.IsNullOrEmpty() && (hidPaymentType.Value.ToInt() == Constants.FeePaymentType.SwapCard.ToInt() || hidPaymentType.Value.ToInt() == Constants.FeePaymentType.Electronic.ToInt()))
                ddlBankNameCard.SelectedValue = oEditFeeDetails.oStudentPayFeeDetails.BankId.ToString();
            else
                ddlAcPDCBank.SelectedValue = oEditFeeDetails.oStudentPayFeeDetails.DepositeBankId.ToString();

            txtChallanNo.Text = oEditFeeDetails.oStudentPayFeeDetails.ChallanNumber.ToString();
            ddlBankNameDirectlyPaid.SelectedValue = oEditFeeDetails.oStudentPayFeeDetails.DepositeBankId.ToString();            
            txtRemarks.Text = oEditFeeDetails.oStudentPayFeeDetails.Remarks;
            txtAdditionalRemark.Text = oEditFeeDetails.oStudentPayFeeDetails.AdditionalRemark;
            ChkPaymentCautionMoneyAdjusted.Checked = oEditFeeDetails.IsCautionMoneyAdjusted;
            if(!IsPostBack)
                txtPaymentDate.Text = oEditFeeDetails.oStudentPayFeeDetails.PaymentDate.ToString("dd-MMM-yyyy");

            cmbJVLedgers.SelectedValue = oEditFeeDetails.oStudentPayFeeDetails.JournalVoucherLedgerId.ToString();
        }
    }

    /// <summary>
    /// This method is used to set receipt number if any transaction is selected into edit mode.
    /// </summary>
    private void FillReceiptsCombo()
    {
        string sReceiptNumber = hidReceiptNumber.Value;
        if (sReceiptNumber.Length < Settings.ReceiptMinimumDigits)
            sReceiptNumber = sReceiptNumber.PadLeft(Settings.ReceiptMinimumDigits, '0');

        cmbReceiptNo.SelectedValue = sReceiptNumber;
        cmbReceiptNo.Enabled = false;
    }

    /// <summary>
    /// This method is used to set values to controls if user has selected payment type by Cheque in edit fee payment mode.
    /// </summary>
    private void SetChequeDetails()
    {
        List<ChequeDetails> lstChequeDetails = moStudentFeeDetailsBL.ChequeDetails;
        txtChequeNumber.Text = lstChequeDetails[0].ChequeNumber;
        txtDate.Text = lstChequeDetails[0].ChequeDate.ToString("dd-MMM-yyyy");
        ddlBankName.SelectedValue = lstChequeDetails[0].BankId.ToString();
        ddlAcChqBank.SelectedValue = moStudentFeeDetailsBL.DepositedBankId.ToString();
        txtChequeRemarks.Text = lstChequeDetails[0].Remarks;
    }

    /// <summary>
    ///  This method is used to set values to controls if user has selected payment type by Swap in edit fee payment mode.
    /// </summary>
    private void SetSwapCardDetails()
    {
        SwapCardDetails oSwapCardDetails = moStudentFeeDetailsBL.SwapCardDetails;
        txtSwapNumber.Text = oSwapCardDetails.SwapNo;
        ddlCardType.SelectedValue = oSwapCardDetails.CardTypeId.ToString();
        ddlAcCardBank.SelectedValue = moStudentFeeDetailsBL.DepositedBankId.ToString();
    }

    /// <summary>
    ///  This method is used to set values to controls if user has selected payment type by electronic payment in edit fee payment mode.
    /// </summary>
    private void SetElectronicPaymentDetails()
    {
        ElectronicPaymentDetails oElectronicPaymentDetails = moStudentFeeDetailsBL.ElectronicPaymentDetails;
        txtSwapNumber.Text = oElectronicPaymentDetails.TxnNo;
        cmbElectronicTypes.SelectedValue = oElectronicPaymentDetails.oElectronicPaymentType.TypeId.ToString();
        ddlAcCardBank.SelectedValue = moStudentFeeDetailsBL.DepositedBankId.ToString();
    }

    /// <summary>
    /// This method creates an XML for student fee id list.
    /// </summary>
    /// <param name="aiLatefee"></param>
    /// <returns></returns>
    private string GetXMLForStudentFeeIds(int aiLatefee)
    {
        const string S_STUDENT_FEE_ID = "Student_Fee_Id";
        const string S_STUDENT_FEE_LIST = "StudentFeeList";
        const string S_STUDENT = "Student";
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement(S_STUDENT_FEE_LIST);
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT_FEE_LIST, string.Empty);
        int iPDCid = 0;
        if (hidPDCId.Value != string.Empty)
            iPDCid = Convert.ToInt32(hidPDCId.Value);
        if (iPDCid == 0)
        {
            string sStudentFeeIds = hidStudentFeeIds.Value;
            string[] sArrStudentFeeId = sStudentFeeIds.Split(',');
            foreach (string sStudentFeeId in sArrStudentFeeId)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, string.Empty);

                string sAtrrName = S_STUDENT_FEE_ID;
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sStudentFeeId;
                oXmlNode.Attributes.Append(attr);
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to create query string and redirect to base screen.
    /// </summary>
    private void SetQueryString()
    {
        string sQueryString = "StudentId=" + hidStudentId.Value;
        string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "'?" + sEncryptQueryString + "'";
        Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");
        Response.Write("window.close();");
        Response.Write("</script>");        
    }

    /// <summary>
    /// This method creates an XML for cheque details of both PDC and normal cheques.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForChequeDetails()
    {
        List<ChequeDetails> lstChequeDetails = new List<ChequeDetails>();

        if (chkFeePayment.SelectedValue == Constants.FeePaymentType.PDC.ToString())
        {
            for (int iCnt = 0; iCnt < grdPostDatedCheque.Rows.Count; iCnt++)
            {
                CheckBox chkPay = (CheckBox)grdPostDatedCheque.Rows[iCnt].Cells[Constants.I_ZERO].FindControl("ChkBoxPay");
                if (!chkPay.Checked) continue;
                int iPDCId = Convert.ToInt32(grdPostDatedCheque.DataKeys[iCnt][Constants.I_ZERO].ToString());
                ChequeDetails oChequeDetails = new ChequeDetails
                {
                    IsPDC = true,
                    ChequeId = iPDCId
                };

                lstChequeDetails.Add(oChequeDetails);
            }
        }
        else if (!tblChequeGrid.Visible)
        {
            ChequeDetails oChequeDetails = new ChequeDetails
            {
                IsPDC = true,
                ChequeId = hidPDCId.Value.ToInt()
            };

            lstChequeDetails.Add(oChequeDetails);
        }
        else
        {
            ChequeDetails oChequeDetails = new ChequeDetails
            {
                ChequeNumber = txtChequeNumber.Text.Trim(),
                ChequeDate = txtDate.Text.ToDateTime(),
                BankId = ddlBankName.SelectedValue.ToInt(),
                Remarks = txtChequeRemarks.Text.Trim()
            };

            lstChequeDetails.Add(oChequeDetails);
        }

        return base.GenerateXml(lstChequeDetails);
    }

    /// <summary>
    /// Returns an XML string for Card payment details.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForCardDetails()
    {
        SwapCardDetails oSwapCardDetails = new SwapCardDetails
        {
            SwapNo = txtSwapNumber.Text,
            CardTypeId = ddlCardType.SelectedValue.ToInt()
        };
        return base.GenerateXml(oSwapCardDetails);
    }

    /// <summary>
    /// This method is used to get the electronic payment details XML for the selected payment.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForElectronicPaymentDetails()
    {
        ElectronicPaymentDetails oElectronicPayment = new ElectronicPaymentDetails
        {
            TxnNo = txtSwapNumber.Text.TrimAll(),
            oElectronicPaymentType = new ElectronicPaymentType { TypeId = cmbElectronicTypes.SelectedValue.ToInt() }
        };
        return base.GenerateXml(oElectronicPayment);
    }

    /// <summary>
    /// This method initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetDefaultDates()
    {
        hidServerDate.Value = Convert.ToString(DateTime.Today);
        cal_PaymentDate.DateValue = DateTime.Today;
		if (Settings.IsMiniSite)
		{
			trSMS.Visible = false;
			trSMSNote.Visible = false;
		}
        if (Settings.SetParticularFeeRestriction)
            hidParticularFeeRestriction.Value = Constants.S_ONE;
        else
            hidParticularFeeRestriction.Value = Constants.S_ZERO;
             
    }

    /// <summary>
    /// This method is used to fill combobox with bank list.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref ddlBankName, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(dtBankList, ref ddlBankNameCard, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);

        if (IsAccountsModuleEnabled)
        {
            BankAccountClient oBankClient = new BankAccountClient();
            try
            {
                oBankClient.Open();
                List<BankAccount> lstLedgers = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);              

                ddlAcChqBank.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);
                ddlAcCardBank.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);
                ddlAcPDCBank.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);
                ddlBankNameDirectlyPaid.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);
                var BankId = lstLedgers.Where(a => a.IsDefault == true).FirstOrDefault();
                
                if (!BankId.IsNull())
                    hidDefaultBank.Value = BankId.Id.ToString();
                SetDefaultBank();
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

        if (ddlBankNameDirectlyPaid.Items.Count == 0)
        {
            ControlUtility.FillDropDownList(dtBankList, ref ddlBankNameDirectlyPaid, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
            lblBankName.Text = "Bank Name :";
        }
    }

    private void FillJVLedgerCombo()
    {
        if (IsAccountsModuleEnabled)
        {
            AccountLedgerClient oLedgerClient = new AccountLedgerClient();
            try
            {
                oLedgerClient.Open();
                List<Ledger> lstLedgers = oLedgerClient.AllLedgers(miSchoolId, miFinancialYearId);

                lstLedgers = lstLedgers.Where(ldg => ldg.Name.Contains("Caution Money")).ToList();
                                
                cmbJVLedgers.Bind(lstLedgers, "Id", "Name", Constants.S_SELECT);

                cmbJVLedgers.SelectedIndex = 1;
                cmbJVLedgers.Enabled = false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an exception fetching Bank Account details.");
            }
            finally
            {
                if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
                    oLedgerClient.Close();
            }
        }
    }

    /// <summary>
    /// Populates the Card type dropdown list.
    /// </summary>
    private void FillCardTypeCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtCardTypeList = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(miSchoolId);
        ControlUtility.FillDropDownList(dtCardTypeList, ref ddlCardType, "CardTypeId", "CardType", Constants.S_SELECT);
    }

    /// <summary>
    /// This method will be used to fill all the electronic types into the types dropdownlist.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbElectronicTypes, "Type", "TypeId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set late fee details to respected controls when we changed the date from date control.
    /// </summary>
    private void SetLateFeeDetails()
    {
        int iLateFeeAmt = Constants.I_ZERO;
        txtLateFeeAmt.Text = Constants.S_ZERO;
        txtAmtToBePaid.Text = txtPayableAmt.Text;
        string sDistribution = " ";
        string sRemarks = string.Empty;
        // Get XML string for selected studentfeeid.
        string sStudentFeeIdsList = GetXMLForStudentFeeIds(1);
        
        // Get late fee details for selected studentfeeid.
        DataTable dtLateFeeDetails = moStudentFeeDetailsBL.GetLateFeeDetails(sStudentFeeIdsList, cal_PaymentDate.DateValue);        

        hidLateFeeDesc.Value = string.Empty;
        bool bAddedToRemark = false;
        for (int iRowCnt = 0; iRowCnt < dtLateFeeDetails.Rows.Count; iRowCnt++)
        {
            //// If late fee is applicable then
            if (dtLateFeeDetails.Rows[iRowCnt]["StandardFeeTypeId"].ToString() != Constants.S_ZERO && dtLateFeeDetails.Rows[iRowCnt]["StandardFeeTypeId"].ToString() != string.Empty && dtLateFeeDetails.Rows[iRowCnt]["StudentFeeId"].ToString() != Constants.S_ZERO)
            {
                int iTotalLateFeeAmtToBePaid = Convert.ToInt32(dtLateFeeDetails.Rows[iRowCnt]["TotalAmountToBePaid"].ToString());

                int iLateFee = Convert.ToInt32(dtLateFeeDetails.Rows[iRowCnt]["LateFeeAmt"].ToString());
                if (!tblChequeGrid.Visible || hidPaymentType.Value.ToInt() == Constants.FeePaymentType.PDC.ToInt())
                    iLateFee = 0;
                iLateFeeAmt += iTotalLateFeeAmtToBePaid;

                string sDescription = string.Empty;
                sDescription = dtLateFeeDetails.Rows[iRowCnt]["DisplayText"].ToString();
                if (iRowCnt == 0)
                    hidRemarks.Value = "Amount paid for " + sDescription;
                else
                    hidRemarks.Value = hidRemarks.Value + ", " + sDescription;
                if (!sDistribution.IsNullOrEmpty())
                    sDistribution = " " + iTotalLateFeeAmtToBePaid;
                else
                    sDistribution = sDistribution + "+" + iTotalLateFeeAmtToBePaid;

                // For autogenerated remarks.(For selected student fee id).
                if (iLateFee > 0 && iTotalLateFeeAmtToBePaid != 0)
                {
                    if (!bAddedToRemark)
                    {
                        if (Settings.IsMaxFeeApplicable && iTotalLateFeeAmtToBePaid > Settings.MaxFee)
                            sRemarks = sRemarks + ", " + dtLateFeeDetails.Rows[iRowCnt]["PayableFor"] + " (Rs. " + Settings.MaxFee.ToString() + "/-" + ")";
                        else
                            sRemarks = sRemarks + ", " + dtLateFeeDetails.Rows[iRowCnt]["PayableFor"] + " (Rs. " + iTotalLateFeeAmtToBePaid + "/-" + ")";
                        bAddedToRemark = true;
                    }
                    else
                        sRemarks = hidLateFeeDesc.Value + ", " + dtLateFeeDetails.Rows[iRowCnt]["PayableFor"] + " (Rs. " + Settings.MaxFee.ToString() + "/-" + ")";
                    hidLateFeeDesc.Value = hidLateFeeDesc.Value + ", " + dtLateFeeDetails.Rows[iRowCnt]["PayableFor"];
                }
            }
        }

        if (Settings.IsMaxFeeApplicable)
        {
            trNote.Visible = true;
            int iMaxFee = Settings.MaxFee;
            trNote.Visible = true;
            lblVerifyNote.Text = Settings.MaxFeeNote;
            if (iLateFeeAmt > iMaxFee)
                iLateFeeAmt = iMaxFee;
        }

        if (chkFeePayment.SelectedIndex == 2)
        {
            sDistribution = "(" + sDistribution.Substring(1).Trim() + ")";
            lblDistribution.Text = sDistribution;
            hidActualLateFeeAmt.Value = iLateFeeAmt.ToString();
            return;
        }
        
        hidLateFeeDistribution.Value = sDistribution.Substring(1);

        // Some school having max limit for late fee amount. So check this flag from resource file and if latefee amount exceeds this max. amount then set late fee amount to max amount.		
        hidActualLateFeeAmt.Value = iLateFeeAmt.ToString();

        // Set distribution of late fee, remarks, amt. to be paid and actual amt.
        if (sDistribution != string.Empty && iLateFeeAmt != 0)
        {
            sDistribution = "(" + sDistribution.Substring(1).Trim() + ")";
            lblDistribution.Text = sDistribution;
            txtLateFeeAmt.Text = iLateFeeAmt.ToString();

            if (hidLateFeeDesc.Value.Trim().Length > 0)
                hidLateFeeDesc.Value = hidLateFeeDesc.Value.Substring(1);

            if (sRemarks.Trim().Length > 0)
            {
                sRemarks = "Late Fee For" + sRemarks.Substring(1);
                txtRemarks.Text = hidRemarks.Value + " & " + sRemarks;
            }

            int iPayableAmt = Convert.ToInt32(txtPayableAmt.Text);
            txtAmtToBePaid.Text = Convert.ToString(iLateFeeAmt + iPayableAmt);
        }

        txtActualAmt.Text = (txtPayableAmt.Text.ToInt() + txtLateFeeAmt.Text.ToInt() - txtConcessionAmt.Text.ToInt()).ToString();
        if (iLateFeeAmt != 0)
            return;
        txtLateFeeAmt.Text = Constants.S_ZERO;
        lblDistribution.Text = "(0)";
        txtRemarks.Text = hidRemarks.Value;
    }

    /// <summary>
    /// Gets the BankId for the specified Ledger.
    /// </summary>
    /// <param name="aiLedgerId"></param>
    /// <returns></returns>
    private int GetBankIdForLedger(int aiLedgerId)
    {
        int iBankId = aiLedgerId;
        var oBankClient = new BankAccountClient();
        try
        {
            oBankClient.Open();
            iBankId = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId).Find(bank => bank.Id == aiLedgerId).Bank.Id;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : An exception occured while fetching Bank Id for selected Bank.");
        }
        finally
        {
            if (oBankClient.State != CommunicationState.Faulted)
                oBankClient.Close();
        }

        return iBankId;
    }

    /// <summary>
    /// Records the fee payment in the Accounts module.
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="asReceiptNo"></param>
    private void RecordCashPayment(int aiStudentId, string asReceiptNo)
    {
        // Create a fee voucher for the fees paid by the student
        AccountVoucherClient oVoucherClient = new AccountVoucherClient();
        try
        {
            oVoucherClient.Open();
            oVoucherClient.CreateFeeVoucherForCashPayment(miSchoolId, miAcademicYearId, miFinancialYearId, aiStudentId, asReceiptNo, miUserId);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), string.Format("Accounts Module : An exception occured while recording a fee payment. StudentId : {0}. ReceiptNo : {1}", aiStudentId, asReceiptNo));
        }
        finally
        {
            if (oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }
    }

    /// <summary>
    /// Creates a Ledger for a new Fee type specified while paying fee.
    /// </summary>
    /// <param name="asFeeType"></param>
    private void CreateLedgerForNewFeeType(string asFeeType)
    {
        AccountLedgerClient oLedgerClient = new AccountLedgerClient();
        try
        {
            oLedgerClient.Open();
            oLedgerClient.CreateLedgerForNewFeeType(miSchoolId, miFinancialYearId, asFeeType, miUserId);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), string.Format("Accounts Module : An exception occured while creating a Ledger for new fee type : {0}", asFeeType));
        }
        finally
        {
            if (oLedgerClient.State != CommunicationState.Faulted)
                oLedgerClient.Close();
        }
    }

    /// <summary>
    /// This function is used to delete the fee voucher previously created for the given receipt number.
    /// </summary>
    private void DeleteVoucher(string asReceiptNo)
    {        
        string sStudentFeeIdsXML = String.Empty;
        // We get the FeeVoucher particulars for the given Student and ReceiptNo.This needs to be performed now(before fee being delete in the db) becuase after deletion,
        // It is difficult to get the correct particulars (since there could be multiple deleted entries).
        var oVoucherClient = new AccountVoucherClient();
        try
        {
            oVoucherClient.Open();
            List<FeeVoucherParticulars> lstFeeParticulars = oVoucherClient.GetFeePaymentParticulars(miSchoolId, miAcademicYearId, miFinancialYearId, hidStudentId.Value.ToInt(), asReceiptNo);
            sStudentFeeIdsXML = CommonUtility.GetXMLForList(lstFeeParticulars);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while getting FeeVoucher particulars. StudentId : {0}. ReceiptNo : {1}", hidStudentId.Value.ToInt(), asReceiptNo));
        }
        finally
        {
            if (oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }

        // Now we actually delete the previously collected particulars from the FeeVoucher.
        oVoucherClient = new AccountVoucherClient();
        try
        {
            oVoucherClient.Open();
            oVoucherClient.DeleteFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, hidStudentId.Value.ToInt(), asReceiptNo, sStudentFeeIdsXML, miUserId,true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while deleting a fee payment. StudentId : {0}. ReceiptNo : {1}", hidStudentId.Value.ToInt(), asReceiptNo));
        }
        finally
        {
            if (oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }
    }

    /// <summary>
    /// Opens a popup displaying a receipt for the last fee paid.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void PrintReceipt(int aiStudentId)
    {
        string sStudentFeeId = string.Empty;
        var sbStudentFeeIds = new StringBuilder();
        int iHeaderId = 0;
        
        int iItemCount = lstvwStudentFee.Items.Count;
        for (int iCount = 0; iCount < iItemCount; iCount++)
        {
            CheckBox chkSelect = lstvwStudentFee.Items[iCount].FindControl("chkSelect") as CheckBox;
            if (chkSelect != null && chkSelect.Checked)
            {                
                int iSchoolwiseStudentFeeId = lstvwStudentFee.DataKeys[iCount]["SchoolwiseStudentFeeId"].ToInt();
                iHeaderId = lstvwStudentFee.DataKeys[iCount]["AccountHeaderId"].ToInt();
                sStudentFeeId = string.Format("{0},{1}", sStudentFeeId, iSchoolwiseStudentFeeId);
                sbStudentFeeIds.AppendFormat(",{0}", iSchoolwiseStudentFeeId);
            }
        }

        if (sStudentFeeId.StartsWith(","))
            sStudentFeeId = sStudentFeeId.Substring(1);
        hidStudentFeeIds.Value = sStudentFeeId;        
        Response.Write(string.Format("<script language='javascript'>window.opener.location=window.opener.location.pathname+'?{0}';window.opener.focus();</script>", CommonUtility.EncryptQuerystring("StudentId=" + hidStudentId.Value)));

        string sFeeIds = hidStudentFeeIds.Value;
        if (sFeeIds.IndexOf(",") > 0)
        {
            sFeeIds = sFeeIds.Substring(0, sFeeIds.IndexOf(","));
            hidStudentFeeIds.Value = sFeeIds.Substring(sFeeIds.IndexOf(",") + 1);
        }

        int iReceiptNo = !string.IsNullOrEmpty(cmbReceiptNo.SelectedValue) && cmbReceiptNo.SelectedValue != Constants.S_ZERO ?
                         Convert.ToInt32(cmbReceiptNo.SelectedValue.TrimStart('0')) :
                         moStudentFeeDetailsBL.GetReceiptNo(aiStudentId, miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentFeeIds.Value));

        Response.Write(string.Format("<script language='javascript'>window.open('../Accountant/FeesMiniReceipt.aspx?{0}','_new','left=0, top=0, height=400, width=850, status=no, resizable= no, scrollbars= yes')</script>",
                                     CommonUtility.EncryptQuerystring(string.Format("&PostBackUrl=~/PayFeePopUp.aspx&StudentId={0}&ReceiptNo={1}&AccountHeaderId={2}",
                                     hidStudentId.Value,
                                     iReceiptNo, iHeaderId))));
    }

    /// <summary>
    /// Saves the ClientID of the control to a hidden feild.
    /// </summary>
    /// <param name="aoControl"></param>
    private void SetPostBackElementId(Control aoControl)
    {
        if (aoControl != null)
            hidPostBackElementId.Value = aoControl.ClientID;
    }

    /// <summary>
    /// This method is used to generate the remarks for selected fee type and changed amount.
    /// </summary>
    private void chkSelectStudentFee_Checked()
    {
        RemarkDetails oRemarkDetails = new RemarkDetails
                                           {
                                               sLateFeeRemarks = string.Empty,
                                               iLateFee = 0,
                                               sRemarks = string.Empty,
                                               sFeeIds = string.Empty,
                                               sDistribution = string.Empty
                                           };
        oRemarkDetails = GetSelecteFeeDetails(oRemarkDetails);
        SetLateFee(oRemarkDetails);
        GenerateLateFeeDescription(oRemarkDetails);
        SetRemarks(oRemarkDetails);
    }

    /// <summary>
    /// This method is used to set the late fee details as per the Maxlatefee applicable for the school.
    /// </summary>
    /// <param name="aoRemarkDetails"></param>
    private void SetLateFee(RemarkDetails aoRemarkDetails)
    {
        txtLateFeeAmt.Text = aoRemarkDetails.iLateFee.ToString();
        bool bIsMaxFeeApplicable = Settings.IsMaxFeeApplicable;
        if (bIsMaxFeeApplicable)
        {
            lblVerifyNote.Text = Settings.MaxFeeNote;
            SetDistribution(aoRemarkDetails.sDistribution);
        }
        else
            lblDistribution.Visible = false;
    }

    /// <summary>
    /// This is a private method and used to set the late fee distribution of the selected fee types.
    /// </summary>
    /// <param name="asDistribution"></param>
    private void SetDistribution(string asDistribution)
    {
        if (Settings.MaxFee < txtLateFeeAmt.Text.ToInt() && asDistribution != string.Empty)
        {
            lblDistribution.Text = "(Actual Late Fee: " + asDistribution.Substring(1) + ")";
            txtLateFeeAmt.Text = Settings.MaxFee.ToString();
        }
        else
            lblDistribution.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set the actual remark text depending on late fee and concession fee.
    /// </summary>
    /// <param name="aoRemarkDetails"></param>
    private void SetRemarks(RemarkDetails aoRemarkDetails)
    {
        if (aoRemarkDetails.sRemarks.StartsWith(","))
            aoRemarkDetails.sRemarks = aoRemarkDetails.sRemarks.Substring(1);
        txtRemarks.Text = string.Format("Amount paid for {0}", aoRemarkDetails.sRemarks);
        SetConcession();
        if (aoRemarkDetails.iLateFee != Constants.I_ZERO && (aoRemarkDetails.iLateFee + txtLateFeeAmt.Text.ToInt()) != 0)
            txtRemarks.Text += string.Format("& Late fee for {0} (Rs. {1}/-)", hidLateFeeDesc.Value, txtLateFeeAmt.Text);

        hidStudentFeeIds.Value = aoRemarkDetails.sFeeIds;
        hidRemarks.Value = txtRemarks.Text;
    }

    /// <summary>
    /// This is a private method and used to set the concession remark as per the given concession.
    /// </summary>
    private void SetConcession()
    {
        if (txtConcessionAmt.Text != Constants.S_ZERO)
            txtRemarks.Text += string.Format(" with  Concession Fee (Concession Fee - Rs. {0}/-) ", txtConcessionAmt.Text);
    }

    /// <summary>
    /// This method is used to set the late fee distribution as per the selected fees from the fee grid.
    /// </summary>
    /// <param name="aoRemarkDetails"></param>
    private void GenerateLateFeeDescription(RemarkDetails aoRemarkDetails)
    {
        if (aoRemarkDetails.sFeeIds != string.Empty)
            hidStudentFeeIds.Value = aoRemarkDetails.sFeeIds.Substring(0, aoRemarkDetails.sFeeIds.LastIndexOf(","));

        hidLateFeeDesc.Value = hidRemarks.Value;

        if (aoRemarkDetails.sLateFeeRemarks != string.Empty)
            hidLateFeeDesc.Value = "," + aoRemarkDetails.sLateFeeRemarks.Substring(1, aoRemarkDetails.sLateFeeRemarks.Length - 1);

        if (hidLateFeeDesc.Value.StartsWith(","))
            hidLateFeeDesc.Value = hidLateFeeDesc.Value.Substring(1);

    }

    /// <summary>
    /// This method is used to generate the Remarks and Late fee distribution for selected checkbox for the fee type.
    /// </summary>
    /// <param name="oRemarkDetails"></param>
    /// <returns></returns>
    private RemarkDetails GetSelecteFeeDetails(RemarkDetails oRemarkDetails)
    {
        int iAmt = 0;        
        for (int iRowCount = 0; iRowCount < lstvwStudentFee.Items.Count ; iRowCount++)
        {
            ListViewDataItem oCurrentItem = lstvwStudentFee.Items[iRowCount];
            var chkFeeId = oCurrentItem.FindControl("chkSelect") as CheckBox;
            if (chkFeeId.Checked)
            {
                var oActualAmount = oCurrentItem.FindControl("txtActualAmount") as TextBox;
                var oAmtPayable = oActualAmount;
                var oAmtPayableFor = oCurrentItem.FindControl("lblPaybleFor") as Label;
                var oAmtLateFee = oCurrentItem.FindControl("lblLateFee") as Label;
                int iStudentFeeId = lstvwStudentFee.DataKeys[iRowCount]["SchoolwiseStudentFeeId"].ToInt();

                if (iStudentFeeId != I_EXISTING_FEE_TYPE && iStudentFeeId != I_NEW_FEE_TYPE)
                {
                    iAmt = iAmt + oAmtPayable.Text.ToInt();
                    if (!oRemarkDetails.sRemarks.Contains(((Label)oCurrentItem.FindControl("lblPaybleFor")).Text))
                        oRemarkDetails.sRemarks = string.Format("{0},{1} ({2} - Rs. {3} /-) ", oRemarkDetails.sRemarks, ((Label)oCurrentItem.FindControl("lblPaybleFor")).Text, ((Label)oCurrentItem.FindControl("lblFeeType")).Text, oAmtPayable.Text);

                    oRemarkDetails.sFeeIds += string.Format("{0},", iStudentFeeId);

                    if (oAmtLateFee.Text != Constants.S_ZERO)
                    {
                        oRemarkDetails.sLateFeeRemarks = oRemarkDetails.sLateFeeRemarks + "," + oAmtPayableFor.Text;
                        if (hidPaymentType.Value == Constants.FeePaymentType.PDC.ToInt().ToString() || !tblChequeGrid.Visible)
                            oRemarkDetails.iLateFee = 0;
                        else
                            oRemarkDetails.iLateFee = oRemarkDetails.iLateFee + txtLateFeeAmt.Text.ToInt();
                        oRemarkDetails.sDistribution = oRemarkDetails.sDistribution + "+" + oAmtLateFee.Text;
                    }
                }
                else
                    GenerateRemarkForExtraFeePayment(iStudentFeeId, oCurrentItem, oRemarkDetails);

            }
        }

        return oRemarkDetails;
    }

    /// <summary>
    /// This method is used to generate a remark for extra fee payments added from last 2 rows of a listview.
    /// </summary>
    /// <param name="aiStudentFeeId"></param>
    /// <param name="aoCurrentItem"></param>
    /// <param name="aoRemarkDetails"></param>
    private void GenerateRemarkForExtraFeePayment(int aiStudentFeeId, ListViewDataItem aoCurrentItem, RemarkDetails aoRemarkDetails)
    {
        var oAmtPayable = aoCurrentItem.FindControl("txtActualAmount") as TextBox;
        if (aiStudentFeeId == I_NEW_FEE_TYPE)
        {
            DropDownList cmbFeeType = aoCurrentItem.FindControl("cmbFeeType") as DropDownList;
            DropDownList cmbPayableFor = aoCurrentItem.FindControl("cmbPayableFor") as DropDownList;
            if (cmbPayableFor.SelectedItem.Text != Constants.S_SELECT && cmbFeeType.SelectedItem.Text != Constants.S_SELECT && oAmtPayable.Text != Constants.S_ZERO)
                aoRemarkDetails.sRemarks = string.Format("{0},{1} ({2} - Rs. {3} /-) ", aoRemarkDetails.sRemarks, cmbPayableFor.SelectedItem.Text, cmbFeeType.SelectedItem.Text, oAmtPayable.Text);
        }
        if (aiStudentFeeId == I_EXISTING_FEE_TYPE)
        {
            TextBox txtNewFeeType = aoCurrentItem.FindControl("txtNewFeeType") as TextBox;
            TextBox txtNewPayableFor = aoCurrentItem.FindControl("txtNewPayableFor") as TextBox;
            if (!txtNewFeeType.Text.IsNullOrEmpty() && !txtNewPayableFor.Text.IsNullOrEmpty() && oAmtPayable.Text != Constants.S_ZERO)
                aoRemarkDetails.sRemarks = string.Format("{0},{1} ({2} - Rs. {3} /-) ", aoRemarkDetails.sRemarks, txtNewPayableFor.Text, txtNewFeeType.Text, oAmtPayable.Text);
        }
    }

    /// <summary>
    /// This method is used to set concession.
    /// </summary>
    private void SetConcessionMessage()
    {
        if (moUserRole != Constants.UserRoles.Student)
        {
            string sConcessionMessage = moStudentFeeDetailsBL.GetConcessionMessage(hidStandardId.Value.ToInt());
            if (!string.IsNullOrEmpty(sConcessionMessage))
            {
                trConcesionMessage.Visible = true;
                lblConcessionMessage.Text = sConcessionMessage;
            }
            else
            {
                lblConcessionMessage.Text = string.Empty;
                trConcesionMessage.Visible = false;
            }
        }
    }

   /// <summary>
    /// This method is used to send notification to the parent
   /// </summary>
   /// <param name="sYearwiseStudentId"></param>
   /// <param name="sAmount"></param>
    public override void SendPushNotification(string sYearwiseStudentId, object sAmount)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {

            StudentBL oStudentBL = new StudentBL();
            int iUserId = oStudentBL.GetStudentUserId(miSchoolId, miAcademicYearId, Convert.ToInt32(sYearwiseStudentId));
            pushNotificationClient = new PushNotificationClient();
            int[] intArrayUserId = new int[1];
            intArrayUserId[0] = iUserId;
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_FEEAMOUNT, Convert.ToString(sAmount));
            pushNotificationClient.SendNotification(NotificationMessageHeadings.SchoolFeePaidAcknowledgement, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
            pushNotificationClient.Close();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                pushNotificationClient.Close();
        }
        }
    private void CheckLoginUser()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowPartialFee.ToInt() && ru.UserId == miUserId))
        {
            hidAllowPartialFee.Value = Constants.S_YES;
        } 
        else
        { 
            hidAllowPartialFee.Value = Constants.S_NO;
        }
    }

    private void CheckFinancialYearStatus()
    {
        if (hidBaseFinancialYearId.Value != string.Empty && hidBaseFinancialYearId.Value.ToInt() != 0 && hidBaseFinancialYearId.Value.ToInt() != miFinancialYearId)
        {
            string sFinancialYearString = CommonUtility.EncryptQuerystring("IsFinancialYearShared=Y&ShowLink=N");
            Response.Redirect("../Common/Error.aspx?" + sFinancialYearString, true);
        }
    }

    private void FollPaymentModes()
    {
        chkFeePayment.Items.Add(new ListItem { Text = "Cash", Value = "Cash" });
        chkFeePayment.Items.Add(new ListItem { Text = "Cheque", Value = "Cheque" });
        chkFeePayment.Items.Add(new ListItem { Text = "PDC", Value = "PDC" });
        chkFeePayment.Items.Add(new ListItem { Text = "Swipe Card", Value = "SwapCard" });
        chkFeePayment.Items.Add(new ListItem { Text = "Electronic (NEFT/RTGS/IMPS)", Value = "Electronic" });

        if (moSchool == Constants.SchoolId.PPSN || moSchool == Constants.SchoolId.PPSH)
        {
            chkFeePayment.Items.Add(new ListItem { Text = "Journal Voucher", Value = "JournalVoucher" });
            cstValCautionMoneyAdjst.Enabled = true;
        }
    }
        
    private string SaveFileToServer()
    {
        string sFile;
        if (flAttachment.HasFile)
        {           
            string sFileName = flAttachment.FileName;
            string sRenamedFileName = sFileName;
            string sFolderName = base.BasePath + S_ATTACHMENT_FOLDER_LOCATION;
            string sServerFilePath = sFolderName + sFileName;
            sFile = sFileName;

            if (File.Exists(sServerFilePath))
            {
                sRenamedFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                sFile = sRenamedFileName;
            }

            sServerFilePath = sFolderName + sRenamedFileName;
            flAttachment.SaveAs(sServerFilePath);
        }
        else
            sFile = hidFileUpload.Value;
        return sFile;
    }

    #endregion -- PRIVATE METHOD(s) --

    #region -- CLASS --
    /// <summary>
    /// This is a private class used for the local purpose to save the Remark details.
    /// </summary>
    private class RemarkDetails
    {
        public int iLateFee { get; set; }
        public string sRemarks{ get; set; }
        public string sFeeIds { get; set; }
        public string sDistribution{ get; set; }
        public string sLateFeeRemarks { get; set; }
    }

    #endregion -- CLASS --
}