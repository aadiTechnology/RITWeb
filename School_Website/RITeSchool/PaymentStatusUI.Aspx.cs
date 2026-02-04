// File Name  : PaymentStatusUI.aspx.cs
// Created By : Shankar 
// Date       : 17/11/2009
//Description : This class is used to handle response from payment gateway
/* Modified By : Pravin
 * Date        : 5 Jun 2013
 * Purpose     : To make the online transaction using a single entry in the database.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CCA.Util;
using COM;
using PushNotificationService;
using SchoolEntities;
using SchoolEntities.Accounts;
using Utility;
using FeeEntities;

public partial class PaymentStatusUI : SchoolBase
{
	#region -- CONSTANT(s) --

	// Response Array Indices
    
    private const string S_ADMISSION = "Admission";
    private const string S_SMS_TEMPLATE_TEXT = "SmsTemplateText";
    private const string S_STUDENTFEE = "StudentFee";
    private const string S_CAUTION_MONEY = "CautionMoney";
    private const string S_INTERNALFEE = "InternalFee";
    
	#endregion -- CONSTANT(s) --
    
	#region -- EVENT HANDLER(s) --

	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);

            AddLog("OnPreInit - Querystring :" + Request.Form.ToString(),true);

			InitializeMemberVariables();

			if (moUserRole == Constants.UserRoles.Student)
				this.Page.MasterPageFile = "./MasterPages/PopupMaster.master";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
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

            var swFile = new StreamWriter(sPath + "OnlineTransactionQueryString.log", true);
            swFile.WriteLine("\n" + sbContent);
            swFile.Flush();
            swFile.Close();
        }
    }

	protected void Page_Load(object sender, EventArgs e)
	{
        try
		{
            if (!IsPostBack)
            {
                AddLog("Page_Load");

                ProcessNetBankingTransaction();

                DisplayErrorMessage();
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    private void DisplayErrorMessage()
    {
        if (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION && (Session["IsInternalAdmission"] == null || Session["IsInternalAdmission"].ToString() != Constants.S_YES))
        {
            string sEmailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToString();

            if (sEmailAddress.Trim() == string.Empty)
                sEmailAddress = "helpdesk@regulusit.net";

            lblErrorMsg.Text = "Error occurred while processing your transaction. <BR /><BR />If amount is deducted from your bank account then no need to submit admission form again. In such case please send transaction details on <a href='mailto:" + sEmailAddress + "'>" + sEmailAddress+ "</a>.";
        }
        else
            lblErrorMsg.Text = "Error occurred while processing your transaction.<BR /><BR />If amount is deducted from your bank account then please wait for an hour and then send transaction details to Software Coordinator with Message Center facility.<BR /><BR />If amount is not deducted then please try again.";
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    private string SendSMS(int aiAdmissionId, NetBankingTransaction aoNetBankingTransaction)
	{
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();		
		var oStudentAdmissionsBL = new StudentAdmissionsBL();
		DataTable oDataTable = oStudentAdmissionsBL.GetStudentAdmissionDetails(aiAdmissionId, iSchoolId);
		if (oDataTable.Rows.Count > 0)
		{
			var moManualMobileNo = new Hashtable();
			string sMobileNumber = Convert.ToString(oDataTable.Rows[0]["MobileNumber"]);
			string sFormNumber = Convert.ToString(oDataTable.Rows[0]["Form_Number"]);		
			moManualMobileNo[sMobileNumber] = sMobileNumber;
            return String.Format("iAdmissionId={0}&Form_Number={1}&Mobile_Number={2}&Amount={3}&TxnId={4}&EnableAdmissionFormFee=true", aiAdmissionId, sFormNumber, sMobileNumber, aoNetBankingTransaction.TransactionAMT, aoNetBankingTransaction.PaymentReferenceNumber);
		}
		return string.Empty;
	}

    /// <summary>
    /// This method is used to process the trnasaction as per the status and type of transaction.
    /// </summary>
	private void ProcessNetBankingTransaction()
    {
        try
        {
            bool bIsChecksumMatched = false;                                    
            NetBankingTransaction oNetBankingTransaction=new NetBankingTransaction();
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;

            AddLog("ProcessNetBankingTransaction() - Before reading any data.");

            AddLog("Is Gateway value null in session ? - " + (Session[Constants.S_GATEWAY] == null? "Yes": "No"));

            string sGateway = Session[Constants.S_GATEWAY].ToString();

            AddLog("Gateway Name -" + sGateway);

            AddLog("Is sTransactionFrom value null in session ? - " + (Session[Constants.S_TRANSACTION_FROM] == null ? "Yes" : "No"));

            string sTransactionFrom = Session[Constants.S_TRANSACTION_FROM].ToString();

            AddLog("Transaction From -" + sTransactionFrom);

            if (sGateway == Constants.PaymentGateways.TPSL.ToString())
            {
                String sResponseMsg = Request["msg"] == null ? String.Empty : Request["msg"].Trim();
                SaveQueryString(sResponseMsg);
                TPSLPGResponseData oTPSLPGResponseData = new TPSLPGResponseData(sResponseMsg);
                oNetBankingTransaction = oTPSLPGResponseData.ReadNetBankingData(out bIsChecksumMatched);
            }
            else if (sGateway == Constants.PaymentGateways.AxisBank.ToString())
            {
                SaveQueryString(Page.Request.QueryString.ToString());
                SortedList oSplitResponse = SplitResponse(Page.Request.QueryString.ToString());
                AxisPGResponseData oAxisPGResponseData = new AxisPGResponseData(oSplitResponse);
                oNetBankingTransaction = oAxisPGResponseData.ReadNetBankingData(out bIsChecksumMatched);
            }

            else if (sGateway == Constants.PaymentGateways.PayU.ToString())
            {
                Hashtable oHashtable = new Hashtable();
            
                   string[] sArrKeys;

                // Here we checked if the query string is coming null. If it is not then we will access parameters through it. 
                // Else we will call verify_payment API to get the actual status.
               
                string sResultstring = string.Empty;
                if (!Request.Form.IsNull() && Request.Form.AllKeys.Count() > Constants.I_ONE)
                {
                    sArrKeys = Request.Form.AllKeys;
                    foreach (string key in sArrKeys)
                    {
                        oHashtable.Add(key, Request.Form[key]);
                        sResultstring = sResultstring + key + "=" + Request.Form[key].ToString() + " & ";
                    }

                    SaveQueryString(sResultstring);
                    PayUPGResponseData oPayUPGResponseData = new PayUPGResponseData(oHashtable);
                    oNetBankingTransaction = oPayUPGResponseData.ReadNetBankingData(out bIsChecksumMatched);
                }
                else if (Request.QueryString.Count > Constants.I_ZERO)
                {
                    string[] strForms = Request.QueryString.ToString().Split('&');

                    foreach (string str in strForms)
                    {   
                        string[] sData = str.Split('=');
                        if (sData.Length == 2)
                        {
                            oHashtable.Add(sData[0], sData[1]);                        
                        }                        
                    }

                    SaveQueryString(Request.QueryString.ToString());
                    PayUPGResponseData oPayUPGResponseData = new PayUPGResponseData(oHashtable);
                    oNetBankingTransaction = oPayUPGResponseData.ReadNetBankingData(out bIsChecksumMatched);
                }
                else
                {
                    PayUPGResponseData oPayUPGResponseData = new PayUPGResponseData(oHashtable);
                    oNetBankingTransaction = oPayUPGResponseData.VerifyNetBankingTransaction(out bIsChecksumMatched);
                }                
            }
            else if (sGateway == Constants.PaymentGateways.Atom.ToString())
            {
                NameValueCollection nvc = Request.Form;
                AddLog("In Atom condition querystring : " + Request.Form.ToString());
                AddLog("In Atom condition NameValueCollection : " + nvc.ToString());
                AddLog("Transaction id of session : " + Session["TransactionId"]);
                
                SaveQueryString(nvc.ToString());

                AddLog("query string save in table.");
                AtomPGResponseData oAtomPGResponseData = new AtomPGResponseData(nvc);
                oNetBankingTransaction = oAtomPGResponseData.ReadNetBankingData();                
            }
            else if (sGateway == Constants.PaymentGateways.PayUMoney.ToString())
            {
                Hashtable oHashtable = new Hashtable();
               // string[] sArrKeys;

                // Here we checked if the query string is coming null. If it is not then we will access parameters through it. 
                // Else we will call verify_payment API to get the actual status.
                string sResultstring = string.Empty;
                //if (!Request.Form.IsNull() && Request.Form.AllKeys.Count() > Constants.I_ONE)
                //{
                //    sArrKeys = Request.Form.AllKeys;
                //    foreach (string key in sArrKeys)
                //    {
                //        oHashtable.Add(key, Request.Form[key]);
                //        sResultstring = sResultstring + key + "=" + Request.Form[key].ToString() + " & ";
                //    }

                //    SaveQueryString(sResultstring);
                //    PayUMoneyPGResponse oPayUMoneyPGResponse = new PayUMoneyPGResponse(oHashtable);
                //    oNetBankingTransaction = oPayUMoneyPGResponse.ReadNetBankingData();
                //}

                if (Request.QueryString.Count > Constants.I_ZERO)
                {
                    foreach (string key in Request.QueryString.Keys)
                    {
                        oHashtable.Add(key, Request.QueryString[key]);
                        sResultstring = sResultstring + key + "=" + Request.QueryString[key].ToString() + "&";
                    }

                    sResultstring = sResultstring.Substring(0, sResultstring.Length - 1);
                    SaveQueryString(sResultstring);
                    PayUMoneyPGResponse oPayUMoneyPGResponse = new PayUMoneyPGResponse(oHashtable);
                    oNetBankingTransaction = oPayUMoneyPGResponse.ReadNetBankingData();
                }
            }
            else if (sGateway == Constants.PaymentGateways.AxisBankForAll.ToString())
            {
                // Here we checked if the query string is coming null. If it is not then we will access parameters through it. 
                // Else we will call verify_payment API to get the actual status.
                string sResultstring = string.Empty;
                if (!Request.QueryString.IsNull() && Request.QueryString.Count > Constants.I_ZERO)
                {   
                    AxisBankForAllPGResponse oAxisBankForAllPGResponse = new AxisBankForAllPGResponse();

                    PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
                    List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.AxisBankForAll);
                    string sEncryptionKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ENCRYPTION_KEY").FirstOrDefault().Value;

                    string sResponse = oAxisBankForAllPGResponse.Decrypt(Request.QueryString[0], sEncryptionKey);

                    string sReceivedHash = GetHash(sResponse);
                    SaveQueryString(sResponse);
                    NameValueCollection oNameValueCollection = HttpUtility.ParseQueryString(sResponse);
                    oNetBankingTransaction = oAxisBankForAllPGResponse.ReadNetBankingData(oNameValueCollection, sReceivedHash);
                }
            }
            else if (sGateway == Constants.PaymentGateways.EaseBuzz.ToString())
            {
                // Here we checked if the query string is coming null. If it is not then we will access parameters through it. 
                // Else we will call verify_payment API to get the actual status.
                string sResultstring = string.Empty;
                Hashtable oHashtable = new Hashtable();

                if (!Request.Form.IsNull() && Request.Form.AllKeys.Count() > Constants.I_ONE)
                {
                    EasebuzzPGResponse oEasebuzzPGResponse = new EasebuzzPGResponse();

                    string[] sArrKeys = Request.Form.AllKeys;
                    foreach (string key in sArrKeys)
                    {
                        oHashtable.Add(key, Request.Form[key]);
                        sResultstring = sResultstring + key + "=" + Request.Form[key].ToString() + " & ";
                    }

                    //NameValueCollection nvc = Request.Form;
                    SaveQueryString(sResultstring);
                    //NameValueCollection oNameValueCollection = HttpUtility.ParseQueryString(sResultstring);
                    oNetBankingTransaction = oEasebuzzPGResponse.ReadNetBankingData(oHashtable);
                }
            }
            else if (sGateway == Constants.PaymentGateways.Billdesk.ToString())
            {
                if (Request.QueryString.Count > Constants.I_ZERO && Request.QueryString["msg"] != null)
                {
                    BillDeskPGResponse oBillDeskPGResponse = new BillDeskPGResponse();
                    SaveQueryString(Request.QueryString["msg"].ToString());
                    oNetBankingTransaction = oBillDeskPGResponse.ReadNetBankingData(Request.QueryString["msg"].ToString());
                }
            }
            else if (sGateway == Constants.PaymentGateways.BilldeskDYP.ToString())
            {
                if (Request.QueryString.Count > Constants.I_ZERO && Request.QueryString["msg"] != null)
                {
                    BillDeskDYPPGResponse BillDeskDYPPGResponse = new BillDeskDYPPGResponse();
                    SaveQueryString(Request.QueryString["msg"].ToString());
                    oNetBankingTransaction = BillDeskDYPPGResponse.ReadNetBankingData(Request.QueryString["msg"].ToString());
                }
            }
            else if (sGateway == Constants.PaymentGateways.CCAvenue.ToString())
            {
                if (Request.QueryString.Count > Constants.I_ZERO && Request.QueryString["msg"] != null)
                {
                    CCAvenuePGResponse oCCAvenuePGResponse = new CCAvenuePGResponse();

                    PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();    
                    List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenue);
                    string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;


                    CCACrypto ccaCrypto = new CCACrypto();
                    string encResponse = ccaCrypto.Decrypt(Request.QueryString["msg"].ToString(), sChecksumKey);

                    SaveQueryString(encResponse);
                    oNetBankingTransaction = oCCAvenuePGResponse.ReadNetBankingData(encResponse);
                }
            }
            else if (sGateway == Constants.PaymentGateways.CCAvenueVPMCPS.ToString())
            {
                if (Request.QueryString.Count > Constants.I_ZERO && Request.QueryString["msg"] != null)
                {
                    CCAvenueVPMCPSPGResponse oCCAvenueVPMCPSPGResponse = new CCAvenueVPMCPSPGResponse();

                    bool bIsInternalFee = false;
                    if (sTransactionFrom == S_INTERNALFEE)
                        bIsInternalFee = true;

                    PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(oCCAvenueVPMCPSPGResponse.GetStudentFeeIds(), 0, string.Empty, 0, bIsInternalFee).Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt()).FirstOrDefault();

                    PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
                    List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenueVPMCPS);

                    string sChecksumKey = string.Empty;
                    if (oPaymentGateWayDetails.ProductInfo == Constants.VPMCPSProductInfo.VPMCPS_PP.ToString())
                        sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKeyPP").FirstOrDefault().Value;
                    else
                        sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;
                    
                    CCACrypto ccaCrypto = new CCACrypto();
                    string encResponse = ccaCrypto.Decrypt(Request.QueryString["msg"].ToString(), sChecksumKey);

                    SaveQueryString(encResponse);
                    oNetBankingTransaction = oCCAvenueVPMCPSPGResponse.ReadNetBankingData(encResponse);
                }
            }
            else if (sGateway == Constants.PaymentGateways.RazorPay.ToString())
            {
                Hashtable oHashtable = new Hashtable();
                string sResultstring = string.Empty;

                if (Request.QueryString.Count > Constants.I_ZERO)
                {
                    foreach (string key in QueryString.Keys)
                    {
                        oHashtable.Add(key, QueryString[key]);
                        sResultstring = sResultstring + key + "=" + QueryString[key].ToString() + "&";
                    }

                    sResultstring = sResultstring.Substring(0, sResultstring.Length - 1);
                    SaveQueryString(sResultstring);
                    RazorPayPGResponse oRazorPayPGResponse = new RazorPayPGResponse();
                    oNetBankingTransaction = oRazorPayPGResponse.ReadNetBankingData(oHashtable);
                }
            }
            //else if (sGateway == Constants.PaymentGateways.PhiCommerce.ToString())
            //{
            //    Hashtable oHashtable = new Hashtable();
            //    string[] sArrKeys;

            //    // Here we checked if the query string is coming null. If it is not then we will access parameters through it. 
            //    // Else we will call verify_payment API to get the actual status.
            //    string sResultstring = string.Empty;
            //    if (!Request.Form.IsNull() && Request.Form.AllKeys.Count() > Constants.I_ONE)
            //    {
            //        sArrKeys = Request.Form.AllKeys;
            //        foreach (string key in sArrKeys)
            //        {
            //            oHashtable.Add(key, Request.Form[key]);
            //            sResultstring = sResultstring + key + "=" + Request.Form[key].ToString() + " & ";
            //        }

            //        SaveQueryString(sResultstring);

            //        PhiCommercePGResponse oPhiCommercePGResponse = new PhiCommercePGResponse(oHashtable);
            //        oNetBankingTransaction = oPhiCommercePGResponse.ReadNetBankingData();

            //        //PayUMoneyPGResponse oPayUMoneyPGResponse = new PayUMoneyPGResponse(oHashtable);
            //        //oNetBankingTransaction = oPayUMoneyPGResponse.ReadNetBankingData();
            //    }
            //}

            AddLog("After gateway updation");

            if (!sTransactionFrom.IsNullOrEmpty())
            {
                //This statement will be execute when we got the response as Transaction failed from the returned URL.                
                if (oNetBankingTransaction.TransactionStatus == Constants.TransactionStatus.Failed && sGateway != Constants.PaymentGateways.RazorPay.ToString())
                    HandleFailedOnlineTransaction(oNetBankingTransaction, sTransactionFrom);
                //This statement will be execute when transaction is successful and it is for AdmissionForm.
                else if (sTransactionFrom == S_ADMISSION && bIsChecksumMatched)
                    CompleteOnlineAdmission(oNetBankingTransaction);
                //This statement will be execute when transaction is successful and it is for Student Fee.
                else if (sTransactionFrom == S_STUDENTFEE && bIsChecksumMatched)
                    CompleteStudentFeePayment(oNetBankingTransaction, Constants.OnlineFeeTypes.StudentFee);
                else if (sTransactionFrom == S_CAUTION_MONEY && bIsChecksumMatched)
                    CompleteStudentFeePayment(oNetBankingTransaction, Constants.OnlineFeeTypes.CautionMoney);
                else if (sGateway == Constants.PaymentGateways.RazorPay.ToString())
                {
                    if (sTransactionFrom == S_ADMISSION)
                    {
                        if (oNetBankingTransaction.TransactionStatus == Constants.TransactionStatus.Failed)
                            Session.Abandon();
                        else
                        {
                            this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";

                            int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();		
		                    var oStudentAdmissionsBL = new StudentAdmissionsBL();
                            DataTable oDT = oStudentAdmissionsBL.GetAdmissionDetails(iSchoolId, oNetBankingTransaction.PaymentReferenceNumber.ToInt());

                            string sMobileNumber = Convert.ToString(oDT.Rows[0]["MobileNumber"]);
                            string sFormNumber = Convert.ToString(oDT.Rows[0]["Form_Number"]);
                            int iAdmissionId = Convert.ToInt32(oDT.Rows[0]["Student_Admission_Id"]);
                            string sParams = String.Format("iAdmissionId={0}&Form_Number={1}&Mobile_Number={2}&Amount={3}&TxnId={4}&EnableAdmissionFormFee=true", iAdmissionId, sFormNumber, sMobileNumber, oNetBankingTransaction.TransactionAMT, oNetBankingTransaction.PaymentReferenceNumber);

                            string sQryString = CommonUtility.EncryptQuerystring(sParams);
                            Response.Redirect("Admission/AdmissionThankYouUI.aspx?" + sQryString, false);
                        }
                    }
                    else
                    {
                        string sQryString = string.Empty;
                        if (oNetBankingTransaction.TransactionStatus == Constants.TransactionStatus.Completed)
                            sQryString = CommonUtility.EncryptQuerystring("TransactionStatus=" + true.ToString() + "&Amount=" + oNetBankingTransaction.TransactionAMT + "&TxnId=" + oNetBankingTransaction.PaymentReferenceNumber);
                        else if (oNetBankingTransaction.TransactionStatus == Constants.TransactionStatus.Failed)
                            sQryString = CommonUtility.EncryptQuerystring("TransactionStatus=" + false.ToString() + "&StatusCode=" + oNetBankingTransaction.StatusCode);

                        Response.Redirect("Accountant/FeeThankYouUI.aspx?" + sQryString, false);
                    }
                }
                else if (sGateway == Constants.PaymentGateways.Atom.ToString()
                    || sGateway == Constants.PaymentGateways.TPSL.ToString() || sGateway == Constants.PaymentGateways.PayUMoney.ToString() || sGateway == Constants.PaymentGateways.AxisBankForAll.ToString() || sGateway == Constants.PaymentGateways.EaseBuzz.ToString() ||
                    sGateway == Constants.PaymentGateways.Billdesk.ToString() || sGateway == Constants.PaymentGateways.BilldeskDYP.ToString() || sGateway == Constants.PaymentGateways.CCAvenue.ToString() || sGateway == Constants.PaymentGateways.CCAvenueVPMCPS.ToString() || ((sGateway == Constants.PaymentGateways.AxisBank.ToString() || sGateway == Constants.PaymentGateways.PayU.ToString()) && sTransactionFrom == S_INTERNALFEE))
                {
                    if (sTransactionFrom == S_ADMISSION)
                        CompleteOnlineAdmission(oNetBankingTransaction);
                    else if (sTransactionFrom == S_CAUTION_MONEY)
                        CompleteStudentFeePayment(oNetBankingTransaction, Constants.OnlineFeeTypes.CautionMoney);
                    else if (sTransactionFrom == S_INTERNALFEE)
                        CompleteStudentFeePayment(oNetBankingTransaction, Constants.OnlineFeeTypes.InternalFee);
                    else
                        CompleteStudentFeePayment(oNetBankingTransaction, Constants.OnlineFeeTypes.StudentFee);
                }
                else
                {
                    var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();
                    oNetBankingPaymentTransactionsBL.UpdateOnlineTransactionStatus(oNetBankingTransaction, (oNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.CautionMoney.ToInt()), (oNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.InternalFee.ToInt()));
                }
            }
        }
        catch (SqlException ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            if (Session[Constants.S_TRANSACTION_FROM].ToString() == S_ADMISSION)
                this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";
            else
            {
                string sQryString = CommonUtility.EncryptQuerystring("TransactionStatus=" + false.ToString());
                Response.Redirect("Accountant/FeeThankYouUI.aspx?" + sQryString, false);
            }
        }
    }

    private string GetHash(string asResponse)
    {
        string sHash = string.Empty;
        string[] sArr = asResponse.Split('&');
        foreach (string sVal in sArr)
        {
            string[] sKeys = sVal.Split('=');
            if (sKeys.Length > 0)
            {
                if (sKeys[0] == "CKS")
                {
                    sHash = sKeys[1];
                    break;
                }
            }

            if (sHash != string.Empty)
                break;
        }
        return sHash.Trim();
    }

    /// <summary>
    /// This is a common method which is called from multiple location to save query string.
    /// </summary>
    /// <param name="asQueryString"></param>
    private void SaveQueryString(string asQueryString)
    {
        if (!Session["TransactionId"].IsNull())
            NetBankingPaymentTransactionsBL.SaveQueryString(Session["TransactionId"].ToInt(), asQueryString);
    }

    /// <summary>
    /// This method is used to generate the hash table depending on the query string received from Axis bank gateway. It is provided by axis bank gateway itself.
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    private SortedList SplitResponse(string asRawData)
    {
        SortedList oResponseData = new SortedList(new VPCStringComparer());
        if (asRawData.IndexOf("=") > 0)
        {
            foreach (string asPair in asRawData.Split('&'))
            {
                int iIndex = asPair.IndexOf("=");
                if (iIndex > 1 && asPair.Length > iIndex)
                {
                    string sParamKey = HttpUtility.UrlDecode(asPair.Substring(0, iIndex));
                    string sParamValue = HttpUtility.UrlDecode(asPair.Substring(iIndex + 1));
                    oResponseData.Add(sParamKey, sParamValue);
                }
            }
        }
        return oResponseData;
    }

    /// <summary>
    /// This method is used to handle the all types of failed transaction. We will redirect the page depends on type of transaction e.g. Fee/Admission 
    /// If merchent Id comes empty or anything different then we will redirect to show the errormessage.
    /// </summary>
    /// <param name="oNetBankingTransaction"></param>
    /// <param name="asMerchant"></param>
    private void HandleFailedOnlineTransaction(NetBankingTransaction oNetBankingTransaction, string asMerchant)
    {
        var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();
        oNetBankingPaymentTransactionsBL.UpdateOnlineTransactionStatus(oNetBankingTransaction, (oNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.CautionMoney.ToInt()), (oNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.InternalFee.ToInt()));
        if (asMerchant == S_ADMISSION)
            Session.Abandon();
        else
        {
            string sQryString = CommonUtility.EncryptQuerystring("TransactionStatus=" + false.ToString());
            Response.Redirect("Accountant/FeeThankYouUI.aspx?" + sQryString, false);
        }
    }

    /// <summary>
    /// This method is used to complete online admission transaction. 
    /// </summary>
    /// <param name="oNetBankingTransaction"></param>
    private void CompleteOnlineAdmission(NetBankingTransaction oNetBankingTransaction)
    {
        this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";
        AddLog("CompleteOnlineAdmission()");
        var oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();        
        oNetBankingTransaction.TransactionFor = Constants.OnlineFeeTypes.AdmissionFee.ToInt();
        oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Completed;
        DataTable oDataTable = oNetBankingPaymentTransactionsBL.UpdateOnlineTransactionStatus(oNetBankingTransaction,false, false);

        AddLog("After completion of transaction update.");

        string sParams = SendSMS(oDataTable.Rows[0]["AdmissionId"].ToInt(), oNetBankingTransaction);

        AddLog("After sending sms.");

        string sQryString = CommonUtility.EncryptQuerystring(sParams);
        Response.Redirect("Admission/AdmissionThankYouUI.aspx?" + sQryString, false);
    }

    /// <summary>
    /// This method is called to complete fee transaction of a student.After marking transaction we will redirect to the FeeThankyouUI to show the appropriate message.
    /// </summary>
    /// <param name="oNetBankingTransaction"></param>
    private void CompleteStudentFeePayment(NetBankingTransaction oNetBankingTransaction, Constants.OnlineFeeTypes aoOnlineFeeTypes)
    {
        oNetBankingTransaction.TransactionFor = aoOnlineFeeTypes.ToInt();
        oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Completed;

        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        if (Session["FinalAcademicYearId"] != null)
            miAcademicYearId = Session["FinalAcademicYearId"].ToInt();
        if (Session["FinalYearStudentId"] != null)
            iStudentId = Session["FinalYearStudentId"].ToInt();

        var oNetBankingPaymentTransactions = new NetBankingPaymentTransactionsBL(miSchoolId, miAcademicYearId, iStudentId);
        if (Session["IsForNextYear"] != null && Session["IsForNextYear"].ToString() == Constants.S_YES)
            oNetBankingTransaction.IsNextAcademicYear = true;

        bool bIsCautionMoneyPaymenty = false;
        bool bIsInternalFeePayment = false;
        bool bIsLastYearPayment = false;

        if (aoOnlineFeeTypes == Constants.OnlineFeeTypes.CautionMoney)
            bIsCautionMoneyPaymenty = true;

        if (aoOnlineFeeTypes == Constants.OnlineFeeTypes.InternalFee)
            bIsInternalFeePayment = true;


        if (Session["IsOldAcademicYearPayment"] != null && Session["IsOldAcademicYearPayment"].ToString() == Constants.S_ONE)
            bIsLastYearPayment = true;

        DataTable oDataTable = oNetBankingPaymentTransactions.UpdateOnlineTransactionStatus(oNetBankingTransaction, bIsCautionMoneyPaymenty, bIsInternalFeePayment, bIsLastYearPayment);

        Session[Constants.S_SESSION_PAYMENT_RECORD] = null;

        SendSMSOfFee(oDataTable.Rows[0]["PaidAmount"].ToInt(), oDataTable.Rows[0]["AdminUserId"].ToInt());
        string sQryString = CommonUtility.EncryptQuerystring("TransactionStatus=" + true.ToString() + "&Amount=" + oNetBankingTransaction.TransactionAMT + "&TxnId=" + oNetBankingTransaction.PaymentReferenceNumber);
        SendPushNotification(miUserId.ToString(), oDataTable.Rows[0]["PaidAmount"].ToString());
        Response.Redirect("Accountant/FeeThankYouUI.aspx?" + sQryString, false);
    }

   /// <summary>
   /// This method is used to send PushNotification After Transaction mark Completed.
   /// </summary>
   /// <param name="sUserId"></param>
   /// <param name="sAmount"></param>
    public override void SendPushNotification(string sUserId, object sAmount)
    {   
            PushNotificationClient pushNotificationClient = null;
            try
            {
                pushNotificationClient = new PushNotificationClient();
                int[] intArrayUserId = new int[1];
                intArrayUserId[0] = miUserId;
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

	private void SendSMSOfFee(int aiAmount, int aiAdminUserId)
	{
		string sFeeDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
		string sSmsSubject = string.Empty;
		int iSmsId = Constants.SMSTemplate.OnlineFeeDetailsSMS.ToInt();
		int iSMSType = 0;
		DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
		if (oDTSmsTemplate.Rows.Count != 0)
		{
			if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
			{
				sFeeDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

				sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
			}
			if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
				iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
		}

		var oSchoolBL = new SchoolBL(miSchoolId);
		DataTable oDataTable = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, miUserId.ToString(), miAcademicYearId);

		string sMobileNo = oDataTable.Rows[0]["Mobile_Number"].ToString();
		string sMobileNo2 = oDataTable.Rows[0]["Mobile_Number2"].ToString();

        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL();
        int iYearwiseStudentId = moStudentFeeDetailsBL.GetYearwiseStudentId(miUserId, miSchoolId, miAcademicYearId);
        FeeSMS oFeeSMS = moStudentFeeDetailsBL.GetPayableAmount(iYearwiseStudentId, miSchoolId, miAcademicYearId);
        string sHSPSMSText = "We have successfully received your fee payment(%TERMNAME%) of Rs. %AMOUNT% through online payment. Regards, HIS Pune.";
        sFeeDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[Constants.I_ZERO][S_SMS_TEMPLATE_TEXT]);
        if (SchoolBase.Settings.DisplayBalanceAmountInPaymentAcknowledgementSMS == true)
        {
            sFeeDetailsSmsText = sFeeDetailsSmsText.Replace("through online payment.", "through online payment. Balance Payment is Rs. " + oFeeSMS.PayableAmount.ToString() + "/-.").Replace("%Amount%", aiAmount.ToString() + "/-");
        }
        else
        {
        
            if (SchoolBase.Settings.DisplayBalanceAmountInPaymentAcknowledgementSMS == true)
                sFeeDetailsSmsText = sFeeDetailsSmsText.Replace("through online payment.", "through online payment. " + oFeeSMS.PayableAmount.ToString() + "/-.").Replace("%Amount%", aiAmount.ToString() + "/-");
            else if (miSchoolId == Constants.SchoolId.HSP.ToInt())

                sFeeDetailsSmsText = sHSPSMSText.Replace("%PAYMENTMODE%.", "through online payment. ").Replace("%AMOUNT%", aiAmount.ToString() + "/-").Replace("%TERMNAME%", oFeeSMS.Term);
               
            else
                sFeeDetailsSmsText = sFeeDetailsSmsText.Replace("%Amount%", aiAmount.ToString() + "/-");

                sFeeDetailsSmsText = sFeeDetailsSmsText.Replace("%Amount%", aiAmount.ToString() + "/-");

        }
		var oSMS = new SMS
					{
			            SenderID	   = aiAdminUserId,
			            SMSText		   = sFeeDetailsSmsText,
			            SMSType		   = iSMSType,
			            School_Name	   = oSchoolBL.SchoolName + "::" + sSmsSubject,
			            DisplayText	   = oDataTable.Rows[0]["Name"].ToString(),
			            SenderRoleID   = Constants.UserRoles.Admin.ToInt(),
                        TemplateRegistrationId = sTemplateRegistrationId,
			            Sender		   = oSchoolBL.SMSSenderName,
			            AcademicYearID = miAcademicYearId,
			            SchoolID	   = miSchoolId
		            };
		oSMS.To.Add(miUserId, sMobileNo);
		if (sMobileNo2 != string.Empty)
			oSMS.To.Add(miUserId + "sm;", sMobileNo2);
		oSMS.Send();
	}

	#endregion -- PRIVATE METHOD(s) --
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
        System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
        if (sStr1 != null && sStr2 != null)
        {
            // Compare using an Ordinal Comparison.
            return myComparer.Compare(sStr1, sStr2, System.Globalization.CompareOptions.Ordinal);
        }
        throw new ArgumentException("should be strings.");
    }
}

/// <summary>
/// This class will be used to read query string for TPSL payment gateway.
/// </summary>
public class TPSLPGResponseData : SchoolBase
{
    #region -- CONSTANT(s) --

    // Response Array Indices
    private const int MERCHANT_ID = 0;
    private const int TRANSACTION_ID = 1;
    private const int TPSL_TRANSACTION_ID = 2;
    private const int TXN_AMOUNT = 4;
    private const int CSTBANK_ID = 5;
    private const int AUTH_STATUS = 14;
    private const int CHECKSUM = 25;
    private const int TOTAL_LENGTH = 26;
    // Transaction Success Code.
    private const string S_TRANSACTIONSUCCESS = "0300";

    #endregion -- CONSTANT(s) --

    #region Data Member(s)

    private string msQueryString;

    #endregion

    #region -- CONSTRUCTOR(s) --

    public TPSLPGResponseData(string asQueryString)
    {
        this.msQueryString = asQueryString;
    }

    #endregion -- CONSTRUCTOR(s) --

    /// <summary>
    /// Here we intialize all the parameters read from returned url.Also we initialize transaction status from the parameters we got.
    /// </summary>
    /// <param name="asArrtoken"></param>
    /// <param name="oNetBankingTransaction"></param>
    /// <returns></returns>
    public NetBankingTransaction ReadNetBankingData(out bool abIsChecksumMatched)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();
        
        var oCheckSumResponseBean = new CheckSumResponseBean();
        var oTPSLUtil1 = new TPSLUtil1();        
        String sResponseMsg = msQueryString;
        String[] sArrtoken = sResponseMsg.Split('|');
        oCheckSumResponseBean.MSG = sResponseMsg;

        oCheckSumResponseBean.PropertyPath =
            Server.MapPath(Settings.AdmissionFormSubmerchantID == sArrtoken[MERCHANT_ID] ? "~/MerchantDetailsAdmission.property" : "~/MerchantDetailsFee.property");

        string sCheckSumValue = oTPSLUtil1.transactionResponseMessage(oCheckSumResponseBean);

        if (!sArrtoken[MERCHANT_ID].IsNullOrEmpty())
        {
            //oNetBankingTransaction.PaymentITCParameter = (Settings.AdmissionFormSubmerchantID == sArrtoken[MERCHANT_ID] ? "From$$Admission" : "From$$StudentFee");
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();
        }

        if (sArrtoken.Length > 1)
        {
            if (sArrtoken[TRANSACTION_ID] != null)
                oNetBankingTransaction.PaymentReferenceNumber = sArrtoken[TRANSACTION_ID];
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;
            if (sArrtoken[TXN_AMOUNT] != null)
                oNetBankingTransaction.TransactionAMT = sArrtoken[TXN_AMOUNT].ToDouble();

            if (sArrtoken[CSTBANK_ID] != null)
                oNetBankingTransaction.TransactionBankID = sArrtoken[CSTBANK_ID].ToString();

            if (sArrtoken[TPSL_TRANSACTION_ID] != null)
                oNetBankingTransaction.TPSLTransactionID = sArrtoken[TPSL_TRANSACTION_ID];

            if (sArrtoken[AUTH_STATUS] != null)
                oNetBankingTransaction.TransactionStatus = (sArrtoken[AUTH_STATUS] == S_TRANSACTIONSUCCESS ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
        }

        if (sArrtoken.Length == TOTAL_LENGTH && sArrtoken[CHECKSUM] == sCheckSumValue)
            abIsChecksumMatched = true;
        else
            abIsChecksumMatched = false;
        
        return oNetBankingTransaction;
    }
}


/// <summary>
/// This class will be used to read query string for TPSL payment gateway.
/// </summary>
public class AxisPGResponseData : SchoolBase
{
    #region Data Member(s)

    private SortedList msResponce;

    #endregion

    #region -- CONSTRUCTOR(s) --

    public AxisPGResponseData(SortedList asResponse)
    {
        this.msResponce = asResponse;
    }

    #endregion -- CONSTRUCTOR(s) --

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    /// <summary>
    /// Here we intialize all the parameters read from returned url.Also we initialize transaction status from the parameters we got.
    /// </summary>
    /// <param name="asArrtoken"></param>
    /// <param name="oNetBankingTransaction"></param>
    /// <returns></returns>
    public NetBankingTransaction ReadNetBankingData(out bool abIsChecksumMatched)
    {
        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment);
        PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBank.ToInt()).FirstOrDefault();
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();
        string sHashData = oPaymentGateWayDetails.Hash;                
        abIsChecksumMatched = false;

        if (msResponce.Count > 0)
        {
            foreach (var item in msResponce.Keys)
            {
                if (oPaymentGateWayDetails.Hash.Length > 0 && !item.Equals("vpc_SecureHash") && !item.Equals("vpc_SecureHashType"))
                {                    
                    sHashData += msResponce[item];
                }
            }
        }

        if (oPaymentGateWayDetails.Hash.Length > 0)
        {
            string asSignature = CreateSHA256Signature(oPaymentGateWayDetails.Hash);
            if (msResponce["vpc_SecureHash"].Equals(asSignature))
                abIsChecksumMatched = true;
            else
                abIsChecksumMatched = false;
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (msResponce.Count > 1)
        {
            if (msResponce["vpc_MerchTxnRef"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = msResponce["vpc_MerchTxnRef"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;
            if (msResponce["vpc_Amount"] != null)
                oNetBankingTransaction.TransactionAMT = (msResponce["vpc_Amount"].ToDecimal()/100).ToDouble();

            if (msResponce["vpc_AVS_PostCode"] != null)
                oNetBankingTransaction.TransactionBankID = msResponce["vpc_AVS_PostCode"].ToString();
            else
                oNetBankingTransaction.TransactionBankID = "DBTCRD";

            if (msResponce["vpc_TransactionNo"] != null)
                oNetBankingTransaction.TPSLTransactionID = msResponce["vpc_TransactionNo"].ToString();

            if (msResponce["vpc_TxnResponseCode"] != null)
                oNetBankingTransaction.TransactionStatus = ((msResponce["vpc_TxnResponseCode"].ToString() == oPaymentGateWayDetails.SuccessCode)? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
        }        
        return oNetBankingTransaction;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }

    private string CreateSHA256Signature(string asHash)
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
        //SortedList<String, String> list = (useRequest ? _requestFields : _responseFields);

        foreach (DictionaryEntry kvp in msResponce)
        {
            if (!kvp.Key.ToString().Equals("vpc_SecureHash") && !kvp.Key.ToString().Equals("vpc_SecureHashType"))
            {
                if (kvp.Key.ToString().StartsWith("vpc_") || kvp.Key.ToString().StartsWith("user_"))
                    sb.Append(kvp.Key.ToString() + "=" + kvp.Value.ToString() + "&");
            }
        }
        // remove trailing & from string
        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        // Create secureHash on string
        string hexHash = "";
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
    /// This method is used to create signature for Axis bank using MD5 algorthm.
    /// </summary>
    /// <param name="RawData"></param>
    /// <returns></returns>
    private string CreateMD5Signature(string asRawData)
    {
        MD5 oHasher = MD5CryptoServiceProvider.Create();
        byte[] sArrHashValue = oHasher.ComputeHash(Encoding.ASCII.GetBytes(asRawData));
        string sHex = "";
        foreach (byte bite in sArrHashValue)
        {
            sHex += bite.ToString("x2");
        }
        return sHex.ToUpper();
    }   
}

/// <summary>
/// This class will be used to read query string for TPSL payment gateway.
/// </summary>
public class PayUPGResponseData : SchoolBase
{
    #region Data Member(s)

    private Hashtable msResponce;

    #endregion

    #region -- CONSTRUCTOR(s) --

    public PayUPGResponseData(Hashtable asResponse)
    {
        this.msResponce = asResponse;
    }

    #endregion -- CONSTRUCTOR(s) --

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    /// <summary>
    /// Here we intialize all the parameters read from returned url.Also we initialize transaction status from the parameters we got.
    /// </summary>
    /// <param name="asArrtoken"></param>
    /// <param name="oNetBankingTransaction"></param>
    /// <returns></returns>
    public NetBankingTransaction ReadNetBankingData(out bool abIsChecksumMatched)
    {
        PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(),0,string.Empty,0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.PayU.ToInt()).FirstOrDefault();
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();
        abIsChecksumMatched = false;

        string[] sArrHashVars;
        string sHashTemp = string.Empty;
        string sHash = string.Empty;                

        if (msResponce["status"].ToString() == "success")
        {
            sArrHashVars = oPaymentGateWayDetails.Sequence.Split('|');
            Array.Reverse(sArrHashVars);
            sHashTemp = oPaymentGateWayDetails.Hash + "|" + msResponce["status"].ToString();

            foreach (string item in sArrHashVars)
            {
                sHashTemp += "|";
                sHashTemp = sHashTemp + (msResponce[item] != null ? msResponce[item] : "");
            }

            sHash = CreateHash(sHashTemp).ToLower();

            if (sHash != msResponce["cardhash"])
                abIsChecksumMatched = true;
            else
                abIsChecksumMatched = false;                
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (msResponce.Count > 1)
        {
            if (msResponce["txnid"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = msResponce["txnid"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (msResponce["amount"] != null)
                oNetBankingTransaction.TransactionAMT = (msResponce["amount"].ToDouble() + (!msResponce["additionalCharges"].IsNull() ? msResponce["additionalCharges"].ToDouble() : Constants.I_ZERO.ToDouble()));

            if (msResponce["mode"] != null && (msResponce["mode"].ToString() == "DC" || msResponce["mode"].ToString() == "CC"))
                oNetBankingTransaction.TransactionBankID = "DBTCRD";
            else
            {
                if (msResponce["bankcode"] != null)
                    oNetBankingTransaction.TransactionBankID = msResponce["bankcode"].ToString();
            }

            if (msResponce["mihpayid"] != null)
                oNetBankingTransaction.TPSLTransactionID = msResponce["mihpayid"].ToString();

            if (msResponce["error"] != null)
                oNetBankingTransaction.TransactionStatus = (msResponce["error"].ToString() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
        }
        return oNetBankingTransaction;
    }


    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }

    /// <summary>
    /// This function is used to verify the net banking transaction when there is no data in query string or query string came empty.
    /// </summary>
    /// <param name="abIsChecksumMatched"></param>
    /// <returns></returns>
    public NetBankingTransaction VerifyNetBankingTransaction(out bool abIsChecksumMatched)
    {
        PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0,IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.PayU.ToInt()).FirstOrDefault();
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();
        abIsChecksumMatched = false;

        if (!Session["TransactionId"].IsNull())
        {
            // Initialized variables.
            string sCommand = "verify_payment";
            string sTransactionId = Session["TransactionId"].ToString();
            string sParameters = oPaymentGateWayDetails.MerchantId + "|" + sCommand + "|" + sTransactionId + "|" + oPaymentGateWayDetails.Hash;
            string sHash = CreateHash(sParameters).ToLower();

            // Created a post string to be sent.
            ASCIIEncoding encoding = new ASCIIEncoding();
            string sPostString = "key=" + oPaymentGateWayDetails.MerchantId;
            sPostString += ("&hash=" + sHash);
            sPostString += ("&var1=" + sTransactionId);
            sPostString += ("&command=" + sCommand);
            byte[] ArrMessage = encoding.GetBytes(sPostString);

            // Web request to call the service is created.
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create("https://info.payu.in/merchant/postservice?form=1");
            oRequest.Method = "POST";
            oRequest.ContentType = "application/x-www-form-urlencoded";
            oRequest.ContentLength = ArrMessage.Length;
            Stream oRequestStream = oRequest.GetRequestStream();
            oRequestStream.Write(ArrMessage, 0, ArrMessage.Length);
            WebResponse oWebResponse = oRequest.GetResponse();
            Stream oResponseMessage = oWebResponse.GetResponseStream();
            using (StreamReader oStreamReader = new StreamReader(oResponseMessage))
            {
                var Result = oStreamReader.ReadToEnd();
                string[] sAllKeys = Result.Split('\n');

                bool bIsFound = false;
                Hashtable oHashtable = new Hashtable();

                // Here we collect all the data into hash table.
                foreach (string sItem in sAllKeys)
                {
                    if (sItem.IndexOf("mihpayid") != -1 || bIsFound)
                    {
                        bIsFound = true;
                        string sTemp = sItem.Replace('[', ' ').Replace(']', ' ').Replace('>', ' ');
                        var KeyValues = sTemp.Split('=');
                        if (KeyValues.Length == 2)
                            oHashtable.Add(KeyValues[0].Trim(), KeyValues[1].Trim());

                        if (sItem.IndexOf(")") != -1)
                            break;
                    }
                }

                // created objects are disposed.
                oResponseMessage.Dispose();
                oStreamReader.Dispose();

                if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
                    oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

                if (oHashtable.Count > 1)
                {
                    if (oHashtable["txnid"] != null)
                        oNetBankingTransaction.PaymentReferenceNumber = oHashtable["txnid"].ToString();
                    else
                        oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

                    if (oHashtable["amt"] != null)
                        oNetBankingTransaction.TransactionAMT = oHashtable["amt"].ToDouble();

                    if (oHashtable["bankcode"] != null)
                        oNetBankingTransaction.TransactionBankID = oHashtable["bankcode"].ToString();

                    if (oHashtable["mihpayid"] != null)
                        oNetBankingTransaction.TPSLTransactionID = oHashtable["mihpayid"].ToString();

                    if (oHashtable["status"] != null && oHashtable["unmappedstatus"] != null)
                        oNetBankingTransaction.TransactionStatus = ((oHashtable["status"].ToString() == "success" && oHashtable["unmappedstatus"].ToString() == "captured") ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                    else
                        oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;

                    if (oNetBankingTransaction.TransactionStatus == Constants.TransactionStatus.Completed)
                        abIsChecksumMatched = true;
                }
            }
        }
            return oNetBankingTransaction;
        }
    

    /// <summary>
    /// This method is used to create checksum using SHA512 algorithm.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string CreateHash(string asText)
    {
        byte[] ArrMessage = Encoding.UTF8.GetBytes(asText);
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] ArrHashValue;
        SHA512Managed hashString = new SHA512Managed();
        string sHex = "";
        ArrHashValue = hashString.ComputeHash(ArrMessage);
        foreach (byte bite in ArrHashValue)
        {
            sHex += String.Format("{0:x2}", bite);
        }
        return sHex;
    }
}


/// <summary>
/// This class will be used to read query string for TPSL payment gateway.
/// </summary>
public class AtomPGResponseData : SchoolBase
{
    #region Data Member(s)

    private NameValueCollection moCollection;

    #endregion

    #region -- CONSTRUCTOR(s) --

    public AtomPGResponseData(NameValueCollection aoNameValueCollection)
    {
        this.moCollection = aoNameValueCollection;
    }

    #endregion -- CONSTRUCTOR(s) --

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    /// <summary>
    /// This method is used to create signature for Axis bank using MD5 algorthm.
    /// </summary>
    /// <param name="RawData"></param>
    /// <returns></returns>
    private string CreateMD5Signature(string asRawData)
    {
        MD5 oHasher = MD5CryptoServiceProvider.Create();
        byte[] sArrHashValue = oHasher.ComputeHash(Encoding.ASCII.GetBytes(asRawData));
        string sHex = "";
        foreach (byte bite in sArrHashValue)
        {
            sHex += bite.ToString("x2");
        }
        return sHex.ToUpper();
    }

    internal NetBankingTransaction ReadNetBankingData()
    {
        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment);
        PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.Atom.ToInt()).FirstOrDefault();

        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

       string sBankName = moCollection["Bank_Name"].ToString();

       AddLog("Received bank name : " + sBankName);

       string sTransactionBankId = NetBankingPaymentTransactionsBL.GetTransactionBankCode(Constants.PaymentGateways.Atom.ToInt(), sBankName, moCollection["discriminator"]);

       AddLog("Fetched bank code from database : " + sTransactionBankId);

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (moCollection.Count > 1)
        {
            if (moCollection["mer_txn"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = moCollection["mer_txn"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;
            if (moCollection["amt"] != null)
                oNetBankingTransaction.TransactionAMT = moCollection["amt"].ToDouble() + moCollection["surcharge"].ToDouble();

            if (moCollection["Bank_Name"] != null)
                oNetBankingTransaction.TransactionBankID = sTransactionBankId;

            if (moCollection["mmp_txn"] != null)
                oNetBankingTransaction.TPSLTransactionID = moCollection["mmp_txn"].ToString();

            if (moCollection["f_code"] != null)
                oNetBankingTransaction.TransactionStatus = ((moCollection["f_code"].ToString() == oPaymentGateWayDetails.SuccessCode) ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);

            AddLog("F_Code status : " + moCollection["f_code"]);
        }

        return oNetBankingTransaction;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }

    private void AddLog(string asMessage)
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if (iSchoolId == Utility.Constants.SchoolId.DSK.ToInt())
        {
            if (ConfigurationManager.AppSettings["LogFilePath"] != null && ConfigurationManager.AppSettings["LogFilePath"].ToString() != string.Empty)
            {
                string sPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
                var sbContent = new StringBuilder();
                sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
                sbContent.AppendFormat("School Id   : {0}{1}", iSchoolId, Environment.NewLine);
                sbContent.AppendFormat("Message : {0}{1}", asMessage, Environment.NewLine);

                var swFile = new StreamWriter(sPath + "OnlineTransactionQueryString.log", true);
                swFile.WriteLine("\n" + sbContent);
                swFile.Flush();
                swFile.Close();
            }
        }
    }
}

public class PayUMoneyPGResponse : System.Web.UI.Page
{
    Hashtable moResponse;
    public PayUMoneyPGResponse(Hashtable aoResponse)
    {
        this.moResponse = aoResponse;
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData()
    {
        PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.PayUMoney.ToInt()).FirstOrDefault();
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();
        
        string[] sArrHashVarSequence = oPaymentGateWayDetails.Sequence.Split('|');
        Array.Reverse(sArrHashVarSequence);
        //string sHashString = this.moResponse["additionalCharges"].ToString() + "|" + oPaymentGateWayDetails.Hash + "|" + this.moResponse["status"].ToString();
        string sHashString = string.Empty;
        if (this.moResponse["additionalCharges"] != null)
           sHashString = this.moResponse["additionalCharges"].ToString() + "|" + oPaymentGateWayDetails.Hash + "|" + this.moResponse["status"].ToString();
        else
            sHashString = oPaymentGateWayDetails.Hash + "|" + this.moResponse["status"].ToString();

        foreach (string merc_hash_var in sArrHashVarSequence)
        {
            sHashString += "|";
            sHashString = sHashString + (this.moResponse[merc_hash_var] != null ? this.moResponse[merc_hash_var] : "");
        }

        string sHash = Generatehash512(sHashString).ToLower();

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (moResponse.Count > 0)
        {
            if (moResponse["txnid"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = moResponse["txnid"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (moResponse["amount"] != null)
                oNetBankingTransaction.TransactionAMT = (moResponse["amount"].ToDouble() + (!moResponse["additionalCharges"].IsNull() ? moResponse["additionalCharges"].ToDouble() : Constants.I_ZERO.ToDouble()));

            if (moResponse["bankcode"] != null)
            {
                if (moResponse["mode"].ToString() == Constants.PayUMoneyPaymentModes.CC.ToString())
                    oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.CC.ToString();
                else if (moResponse["mode"].ToString() == Constants.PayUMoneyPaymentModes.DC.ToString())
                    oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.DC.ToString();
                else
                    oNetBankingTransaction.TransactionBankID = moResponse["bankcode"].ToString();
            }

            if (moResponse["payuMoneyId"] != null)
                oNetBankingTransaction.TPSLTransactionID = moResponse["payuMoneyId"].ToString();

            if (moResponse["status"] != null)
            {
                if (sHash == this.moResponse["hash"].ToString())
                    oNetBankingTransaction.TransactionStatus = (moResponse["status"].ToString() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }
       
        return oNetBankingTransaction;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }

    private static string Generatehash512(string text)
    {
        byte[] message = Encoding.UTF8.GetBytes(text);

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
}

public class AxisBankForAllPGResponse : System.Web.UI.Page
{
    public AxisBankForAllPGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(NameValueCollection asResponse, string asReceivedHash)
    {
        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(string.Empty, 0, string.Empty, 0, IsInternalFeePayment);
        PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.AxisBankForAll.ToInt()).FirstOrDefault();

        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (asResponse.Count > 0)
        {
            if (asResponse["RID"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = asResponse["RID"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (asResponse["AMT"] != null)
                oNetBankingTransaction.TransactionAMT = asResponse["AMT"].ToDouble(); // (asResponse["AMT"].ToDouble() + (!asResponse["additionalCharges"].IsNull() ? asResponse["additionalCharges"].ToDouble() : Constants.I_ZERO.ToDouble()));

            if (asResponse["PMD"] != null)
            {
                if (asResponse["PMD"].ToString() == "CD" || asResponse["PMD"].ToString() == "OIB" || asResponse["PMD"].ToString() == "AIB")
                {
                    //if (Session[Constants.S_TRANSACTION_PAYMENT_METHOD] != null && Session[Constants.S_TRANSACTION_PAYMENT_METHOD].ToString() == Constants.PayUMoneyPaymentModes.CC.ToString())
                    //    oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.CC.ToString();
                    //else
                    //    oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.DC.ToString();

                    oNetBankingTransaction.TransactionBankID = asResponse["PMD"].ToString();
                }
                else
                    oNetBankingTransaction.TransactionBankID = Constants.S_TRANSACTION_UNKNOWN_BANK_ID;
            }

            if (asResponse["TRN"] != null)
                oNetBankingTransaction.TPSLTransactionID = asResponse["TRN"].ToString();

            if (asResponse["STC"] != null)
            {
                string StrCheckSumString = asResponse["CID"].Trim() + asResponse["RID"].Trim() + asResponse["CRN"].Trim() + asResponse["AMT"].Trim() + "n4p#";
                string sHash = sha256_hash(StrCheckSumString);

                if (sHash == asReceivedHash)
                    oNetBankingTransaction.TransactionStatus = (asResponse["STC"].ToString() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
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

    public string Decrypt(string input, string key)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = Convert.FromBase64String(input);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        //rDel.Padding = PaddingMode.None;
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}


public class EasebuzzPGResponse : System.Web.UI.Page
{
    public EasebuzzPGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(Hashtable asResponse)
    {
        PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.EaseBuzz.ToInt()).FirstOrDefault();

        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (asResponse.Count > 0)
        {
            if (asResponse["txnid"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = asResponse["txnid"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (asResponse["net_amount_debit"] != null)
                oNetBankingTransaction.TransactionAMT = asResponse["net_amount_debit"].ToDouble();

            if (asResponse["mode"] != null)
            {
                if (asResponse["mode"].ToString() == "DC" || asResponse["mode"].ToString() == "CC" || asResponse["mode"].ToString() == "NB" || asResponse["mode"].ToString() == "UPI")
                    oNetBankingTransaction.TransactionBankID = asResponse["mode"].ToString();
                else
                    oNetBankingTransaction.TransactionBankID = Constants.S_TRANSACTION_UNKNOWN_BANK_ID;
            }

            if (asResponse["easepayid"] != null)
                oNetBankingTransaction.TPSLTransactionID = asResponse["easepayid"].ToString();

            if (asResponse["status"] != null)
            {
                string[] merc_hash_vars_seq = oPaymentGateWayDetails.Sequence.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                string merc_hash_string = oPaymentGateWayDetails.Hash + "|" + asResponse["status"];

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (asResponse[merc_hash_var] != null ? asResponse[merc_hash_var] : "");

                }

                string merc_hash = Easebuzz_Generatehash512(merc_hash_string).ToLower();
                if (asResponse["hash"] != null && merc_hash == asResponse["hash"].ToString())
                    oNetBankingTransaction.TransactionStatus = (asResponse["status"].ToString().ToLower() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
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
}

public class BillDeskPGResponse : System.Web.UI.Page
{
    public BillDeskPGResponse()
    {
    }

    public NetBankingTransaction ReadNetBankingData(string asQueryString)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.Billdesk);
        string sResponseSequence = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ResponseSequence").FirstOrDefault().Value;
        string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ChecksumKey").FirstOrDefault().Value;

        string[] arrValues = asQueryString.Split('|');
        string[] sArrKeys = sResponseSequence.Split('|');

        bool bAppend = true;
        StringBuilder sb = new StringBuilder();
        Hashtable oResponse = new Hashtable();
        for (int iIndex = 0; iIndex < sArrKeys.Length; iIndex++)
        {
            oResponse.Add(sArrKeys[iIndex], arrValues[iIndex]);

            if (bAppend)
            {
                sb.Append("|" + arrValues[iIndex]);

                if (sArrKeys[iIndex].ToUpper() == "ERRORDESCRIPTION")
                    bAppend = false;
            }
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (oResponse.Count > 0)
        {
            if (oResponse["CustomerID"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = oResponse["CustomerID"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (oResponse["TxnAmount"] != null)
                oNetBankingTransaction.TransactionAMT = oResponse["TxnAmount"].ToDouble();

            if (oResponse["TxnType"] != null)
            {
                if (oResponse["TxnType"].ToString() == "03")
                    oNetBankingTransaction.TransactionBankID = "DC";
                else if (oResponse["TxnType"].ToString() == "02")
                    oNetBankingTransaction.TransactionBankID = "CC";
                else
                    oNetBankingTransaction.TransactionBankID = "OIB";
            }

            if (oResponse["TxnReferenceNo"] != null)
                oNetBankingTransaction.TPSLTransactionID = oResponse["TxnReferenceNo"].ToString();

            if (oResponse["AuthStatus"] != null)
            {
                //PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0).Where(a => a.GatewayId == Constants.PaymentGateways.Billdesk.ToInt()).FirstOrDefault();

                var sParam = sb.ToString().Substring(1);
                string merc_hash = GetHMACSHA256(sParam, sChecksumKey);
                if (oResponse["CheckSum"] != null && merc_hash == oResponse["CheckSum"].ToString())
                    oNetBankingTransaction.TransactionStatus = (oResponse["AuthStatus"].ToString().ToLower() == "0300" ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }


    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
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

public class BillDeskDYPPGResponse : System.Web.UI.Page
{
    public BillDeskDYPPGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(string asQueryString)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
        List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.BilldeskDYP);
        string sResponseSequence = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ResponseSequence").FirstOrDefault().Value;
        string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "ChecksumKey").FirstOrDefault().Value;

        string[] arrValues = asQueryString.Split('|');
        string[] sArrKeys = sResponseSequence.Split('|');

        bool bAppend = true;
        StringBuilder sb = new StringBuilder();
        Hashtable oResponse = new Hashtable();
        for (int iIndex = 0; iIndex < sArrKeys.Length; iIndex++)
        {
            oResponse.Add(sArrKeys[iIndex], arrValues[iIndex]);

            if (bAppend)
            {
                sb.Append("|" + arrValues[iIndex]);

                if (sArrKeys[iIndex].ToUpper() == "ERRORDESCRIPTION")
                    bAppend = false;
            }
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (oResponse.Count > 0)
        {
            if (oResponse["CustomerID"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = oResponse["CustomerID"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (oResponse["TxnAmount"] != null)
                oNetBankingTransaction.TransactionAMT = oResponse["TxnAmount"].ToDouble();

            if (oResponse["TxnType"] != null)
            {
                if (oResponse["TxnType"].ToString() == "03")
                    oNetBankingTransaction.TransactionBankID = "DC";
                else if (oResponse["TxnType"].ToString() == "02")
                    oNetBankingTransaction.TransactionBankID = "CC";
                else
                    oNetBankingTransaction.TransactionBankID = "OIB";
            }

            if (oResponse["TxnReferenceNo"] != null)
                oNetBankingTransaction.TPSLTransactionID = oResponse["TxnReferenceNo"].ToString();

            if (oResponse["AuthStatus"] != null)
            {
                PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.BilldeskDYP.ToInt()).FirstOrDefault();

                var sParam = sb.ToString().Substring(1);
                string merc_hash = GetHMACSHA256(sParam, sChecksumKey);
                if (oResponse["CheckSum"] != null && merc_hash == oResponse["CheckSum"].ToString().ToLower())
                    oNetBankingTransaction.TransactionStatus = (oResponse["AuthStatus"].ToString().ToLower() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }


    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
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

public class CCAvenuePGResponse : System.Web.UI.Page
{
    public CCAvenuePGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(string asQueryString)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        //PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();      
        //List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenue);
        //string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;


        //CCACrypto ccaCrypto = new CCACrypto();
        //string encResponse = ccaCrypto.Decrypt(asQueryString, sChecksumKey);

        NameValueCollection oResponse = new NameValueCollection();
        string[] segments = asQueryString.Split('&');
        foreach (string seg in segments)
        {
            string[] parts = seg.Split('=');
            if (parts.Length > 0)
            {
                string Key = parts[0].Trim();
                string Value = parts[1].Trim();
                oResponse.Add(Key, Value);
            }
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (oResponse.Count > 0)
        {
            if (oResponse["order_id"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = oResponse["order_id"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (oResponse["amount"] != null)
                oNetBankingTransaction.TransactionAMT = oResponse["amount"].ToDouble();

            if (oResponse["payment_mode"] != null)
            {
                if (oResponse["payment_mode"].ToString() == "Debit Card")
                    oNetBankingTransaction.TransactionBankID = "DC";
                else if (oResponse["payment_mode"].ToString() == "Credit Card")
                    oNetBankingTransaction.TransactionBankID = "CC";
                else if (oResponse["payment_mode"].ToString() == "Net Banking")
                    oNetBankingTransaction.TransactionBankID = "OIB";                    
                else
                    oNetBankingTransaction.TransactionBankID = "-9999";
            }

            if (oResponse["tracking_id"] != null)
                oNetBankingTransaction.TPSLTransactionID = oResponse["tracking_id"].ToString();

            if (oResponse["order_status"] != null)
            {
                PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(string.Empty, 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenue.ToInt()).FirstOrDefault();

                if (oResponse["order_status"] != null)
                {

                    if (Session[Constants.S_SESSION_PAYMENT_RECORD] != null)
                    {
                        Dictionary<string, string> dict = Session[Constants.S_SESSION_PAYMENT_RECORD] as Dictionary<string, string>;
                        if (dict["TxnId"] == oResponse["order_id"].ToString() && dict["TxnAmt"] == oResponse["amount"].ToString() && dict["TxnGuid"] == oResponse["merchant_param4"].ToString())
                        {
                            string sStatus = GetStatus(oPaymentGateWayDetails.AccessCode, oNetBankingTransaction.PaymentReferenceNumber, oNetBankingTransaction.TPSLTransactionID);
                            oNetBankingTransaction.TransactionStatus = (oResponse["order_status"].ToString().ToLower() == oPaymentGateWayDetails.SuccessCode && sStatus == Constants.S_ZERO ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                        }
                        else
                            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
                    }
                    else
                        oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;                    
                }
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }

    private string GetStatus(string AccessCode, string asTransactionId, string asTPSlTxnId)
    {
        string sResString = string.Empty;
        try
        {
            PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
            List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenue);
            string sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;
            string sStatusApiUrl = lstGatewayAdditionalDetails.Where(gt => gt.Name == "StatusApiUrl").FirstOrDefault().Value;


            string orderStatusQuery = asTPSlTxnId + "|"+asTransactionId+"|";
            string encQuery = "";

           // string queryUrl = "https://login.ccavenue.com/apis/servlet/DoWebTrans";

           // string queryUrl = "https://apitest.ccavenue.com/apis/servlet/DoWebTrans";
            
            CCACrypto ccaCrypto = new CCACrypto();
            encQuery = ccaCrypto.Encrypt(orderStatusQuery, sChecksumKey);

            // make query for the status of the order to ccAvenues change the command param as per your need
            string authQueryUrlParam = "enc_request=" + encQuery + "&access_code=" + AccessCode + "&command=orderStatusTracker&request_type=STRING&response_type=STRING";

            // Url Connection
            String message = postPaymentRequestToGateway(sStatusApiUrl, authQueryUrlParam);
            
            NameValueCollection param = getResponseMap(message);
            String status = "";
            String encRes = "";
            if (param != null && param.Count == 2)
            {
                for (int i = 0; i < param.Count; i++)
                {
                    if ("status".Equals(param.Keys[i]))
                    {
                        status = param[i];
                    }
                    if ("enc_response".Equals(param.Keys[i]))
                    {
                        encRes = param[i];                        
                    }
                }
                if (!"".Equals(status) && status.Equals("0"))
                {
                    //sResString = status;
                    string sResult = ccaCrypto.Decrypt(encRes, sChecksumKey);
                    string[] sArr = sResult.Split('|');
                    sResString = sArr[0].Trim();
                }               
            }

        }
        catch (Exception)
        {            
        }

        return sResString;
    }

    private string postPaymentRequestToGateway(String queryUrl, String urlParam)
    {

        String message = "";
     
        StreamWriter myWriter = null;// it will open a http connection with provided url
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest objRequest = WebRequest.Create(queryUrl);//send data using objxmlhttp object
        objRequest.Method = "POST";
        //objRequest.ContentLength = TranRequest.Length;
        objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
        myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
        myWriter.Write(urlParam);//send data
        myWriter.Close();//closed the myWriter object

        // Getting Response
        System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();//receive the responce from objxmlhttp object 
        using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
        {
            message = sr.ReadToEnd();
            //Response.Write(message);
        }
      
        return message;

    }

    private NameValueCollection getResponseMap(String message)
    {
        NameValueCollection Params = new NameValueCollection();
        if (message != null || !"".Equals(message))
        {
            string[] segments = message.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }
        }
        return Params;
    }
}

public class CCAvenueVPMCPSPGResponse : System.Web.UI.Page
{
    public CCAvenueVPMCPSPGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(string asQueryString)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        NameValueCollection oResponse = new NameValueCollection();
        string[] segments = asQueryString.Split('&');
        foreach (string seg in segments)
        {
            string[] parts = seg.Split('=');
            if (parts.Length > 0)
            {
                string Key = parts[0].Trim();
                string Value = parts[1].Trim();
                oResponse.Add(Key, Value);
            }
        }

        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

        if (oResponse.Count > 0)
        {
            if (oResponse["order_id"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = oResponse["order_id"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            if (oResponse["amount"] != null)
                oNetBankingTransaction.TransactionAMT = oResponse["amount"].ToDouble();

            if (oResponse["payment_mode"] != null)
            {
                if (oResponse["payment_mode"].ToString() == "Debit Card")
                    oNetBankingTransaction.TransactionBankID = "DC";
                else if (oResponse["payment_mode"].ToString() == "Credit Card")
                    oNetBankingTransaction.TransactionBankID = "CC";
                else if (oResponse["payment_mode"].ToString() == "Net Banking")
                    oNetBankingTransaction.TransactionBankID = "OIB";
                else if (oResponse["payment_mode"].ToString() == "UPI" || oResponse["payment_mode"].ToString() == "Unified Payments")
                    oNetBankingTransaction.TransactionBankID = "UPI";
                else if (oResponse["payment_mode"].ToString() == "Wallet")
                    oNetBankingTransaction.TransactionBankID = "Wallet";
                else
                    oNetBankingTransaction.TransactionBankID = "-9999";
            }

            if (oResponse["tracking_id"] != null)
                oNetBankingTransaction.TPSLTransactionID = oResponse["tracking_id"].ToString();

            if (oResponse["order_status"] != null)
            {
                PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.CCAvenueVPMCPS.ToInt()).FirstOrDefault();

                if (oResponse["order_status"] != null)
                {

                    if (Session[Constants.S_SESSION_PAYMENT_RECORD] != null)
                    {
                        Dictionary<string, string> dict = Session[Constants.S_SESSION_PAYMENT_RECORD] as Dictionary<string, string>;
                        if (dict["TxnId"] == oResponse["order_id"].ToString() && dict["TxnAmt"] == oResponse["mer_amount"].ToString() && dict["TxnGuid"] == oResponse["merchant_param4"].ToString())
                        {
                            if (oResponse["order_status"].ToString().ToLower() == oPaymentGateWayDetails.SuccessCode)
                            {
                                string sStatus = GetStatus(oPaymentGateWayDetails.AccessCode, oNetBankingTransaction.PaymentReferenceNumber, oNetBankingTransaction.TPSLTransactionID, oPaymentGateWayDetails.ProductInfo);
                                oNetBankingTransaction.TransactionStatus = (oResponse["order_status"].ToString().ToLower() == oPaymentGateWayDetails.SuccessCode && sStatus == Constants.S_ZERO ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
                            }
                            else
                                oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
                        }
                        else
                            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
                    }
                    else
                        oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
                }
                else
                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
            }
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }

    private string GetStatus(string AccessCode, string asTransactionId, string asTPSlTxnId, string asProductInfo)
    {
        string sResString = string.Empty;
        try
        {
            PaymentGatewayBL oPaymentGatewayBL = new PaymentGatewayBL();
            List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = oPaymentGatewayBL.GetGatewayDetails(Constants.PaymentGateways.CCAvenueVPMCPS);

            string sChecksumKey = string.Empty;
            if (asProductInfo == Constants.VPMCPSProductInfo.VPMCPS_PP.ToString())
                sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKeyPP").FirstOrDefault().Value;
            else
                sChecksumKey = lstGatewayAdditionalDetails.Where(gt => gt.Name == "EncryptionKey").FirstOrDefault().Value;

            string sStatusApiUrl = lstGatewayAdditionalDetails.Where(gt => gt.Name == "StatusApiUrl").FirstOrDefault().Value;

            string orderStatusQuery = "{'reference_no':'" + asTPSlTxnId + "','order_no':'"+asTransactionId+"'}";

            string encQuery = "";

            CCACrypto ccaCrypto = new CCACrypto();
            encQuery = ccaCrypto.Encrypt(orderStatusQuery, sChecksumKey);

            string authQueryUrlParam = "enc_request=" + encQuery + "&access_code=" + AccessCode + "&request_type=JSON&response_type=JSON&command=orderStatusTracker&version=1.2";

            // Url Connection
            String message = postPaymentRequestToGateway(sStatusApiUrl, authQueryUrlParam);

            NameValueCollection param = getResponseMap(message);
            String status = "";
            String encRes = "";
            if (param != null && param.Count >= 2)
            {
                for (int i = 0; i < param.Count; i++)
                {
                    if ("status".Equals(param.Keys[i]))
                    {
                        status = param[i];
                    }
                    if ("enc_response".Equals(param.Keys[i]))
                    {
                        encRes = param[i];
                    }
                }
                if (!"".Equals(status) && status.Equals("0"))
                {
                    string sResult = ccaCrypto.Decrypt(encRes, sChecksumKey);

                    var serializer = new JavaScriptSerializer();
                    var dict = serializer.Deserialize<Dictionary<string, object>>(sResult); // deserialize into dictionary
                    sResString = dict["status"].ToString();
                }
            }

        }
        catch (Exception)
        {
        }

        return sResString;
    }

    private string postPaymentRequestToGateway(String queryUrl, String urlParam)
    {

        String message = "";

        StreamWriter myWriter = null;// it will open a http connection with provided url
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest objRequest = WebRequest.Create(queryUrl);//send data using objxmlhttp object
        objRequest.Method = "POST";
        //objRequest.ContentLength = TranRequest.Length;
        objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
        myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
        myWriter.Write(urlParam);//send data
        myWriter.Close();//closed the myWriter object

        // Getting Response
        System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();//receive the responce from objxmlhttp object 
        using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
        {
            message = sr.ReadToEnd();
            //Response.Write(message);
        }

        return message;

    }

    private NameValueCollection getResponseMap(String message)
    {
        NameValueCollection Params = new NameValueCollection();
        if (message != null || !"".Equals(message))
        {
            string[] segments = message.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }
        }
        return Params;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    public string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }
}

public class RazorPayPGResponse : System.Web.UI.Page
{
    public RazorPayPGResponse()
    {
    }

    private bool IsInternalFeePayment
    {
        get
        {
            return Session[Constants.S_TRANSACTION_FROM] != null && Session[Constants.S_TRANSACTION_FROM].ToString() == Constants.S_TYPE_INTERNAL_FEE;
        }
    }

    public NetBankingTransaction ReadNetBankingData(Hashtable oResponse)
    {
        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

        if (oResponse.Count > 0 && oResponse["MerchantTxnId"] != null && !string.IsNullOrEmpty(oResponse["MerchantTxnId"].ToString()) && oResponse["MerchantTxnId"].ToString() != Constants.S_ZERO)
        {
            if (oResponse["MerchantTxnId"] != null)
                oNetBankingTransaction.PaymentReferenceNumber = oResponse["MerchantTxnId"].ToString();
            else
                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

            int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
            NetBankingPaymentTransactionsBL oNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL();
            TransactionStatusDetails oTransactionStatusDetails = oNetBankingPaymentTransactionsBL.GetTransactionStatus(iSchoolId, Constants.PaymentGateways.RazorPay.ToInt(), oResponse["OrderId"].ToString());
            
            PaymentGateWayDetails oPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(GetStudentFeeIds(), 0, string.Empty, 0, IsInternalFeePayment).Where(a => a.GatewayId == Constants.PaymentGateways.RazorPay.ToInt()).FirstOrDefault();
            oNetBankingTransaction.TransactionStatus = (oTransactionStatusDetails.StatusCode == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
            oNetBankingTransaction.StatusCode = oTransactionStatusDetails.StatusCode;
            oNetBankingTransaction.TransactionAMT = oTransactionStatusDetails.Amount;            
        }
        else
        {
            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
        }

        return oNetBankingTransaction;
    }

    /// <summary>
    /// This method is used to get return fee ids.
    /// </summary>
    /// <returns></returns>
    private string GetStudentFeeIds()
    {
        var oStudentFeeIds = new List<int>();
        if (Session["Schoolwise_Student_Fee_Id"] != null)
            oStudentFeeIds = Session["Schoolwise_Student_Fee_Id"] as List<int>;

        string sIds = Constants.S_ZERO;
        if (oStudentFeeIds.Count > 0)
        {
            sIds = string.Join(",", oStudentFeeIds);
            if (sIds.StartsWith(","))
                sIds = sIds.Substring(1);
        }
        return sIds;
    }
}

//public class PhiCommercePGResponse : System.Web.UI.Page
//{
//    private Hashtable msResponse;
//    public PhiCommercePGResponse()
//    {
//    }

//    public PhiCommercePGResponse(Hashtable asResponse)
//    {
//        this.msResponse = asResponse;
//    }

//    public NetBankingTransaction ReadNetBankingData()
//    {
//        List<PaymentGateWayDetails> lstPaymentGateWayDetails = NetBankingPaymentTransactionsBL.GetPaymentGatewayDetails(string.Empty);
//        PaymentGateWayDetails oPaymentGateWayDetails = lstPaymentGateWayDetails.Where(a => a.GatewayId == Constants.PaymentGateways.EaseBuzz.ToInt()).FirstOrDefault();

//        NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction();

//        if (!Session.IsNull() && !Session[Constants.S_TRANSACTION_FROM].IsNull())
//            oNetBankingTransaction.PaymentITCParameter = "From$$" + Session[Constants.S_TRANSACTION_FROM].ToString();

//        if (msResponse.Count > 0)
//        {



//            //// // Created a post string to be sent.
//            //ASCIIEncoding encoding = new ASCIIEncoding();
//            //string sPostString = "merchantID=" + msResponse["merchantId"].ToString() + "&merchantTxnNo=" + msResponse["merchantTxnNo"].ToString() + "&originalTxnNo" + msResponse["merchantTxnNo"].ToString() + "&transactionType=" + "SALE";
            
//            //byte[] ArrMessage = encoding.GetBytes(sPostString);

//            //// Web request to call the service is created.
//            //HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create("https://qa.phicommerce.com/pg/api/sale");
//            //oRequest.Method = "POST";
//            //oRequest.ContentType = "application/x-www-form-urlencoded";
//            //oRequest.ContentLength = ArrMessage.Length;
//            //Stream oRequestStream = oRequest.GetRequestStream();
//            //oRequestStream.Write(ArrMessage, 0, ArrMessage.Length);
//            //WebResponse oWebResponse = oRequest.GetResponse();
//            //Stream oResponseMessage = oWebResponse.GetResponseStream();


//            //XmlTextReader reader = new XmlTextReader(oResponseMessage);

//            //using (StreamReader oStreamReader = new StreamReader(oResponseMessage))
//            //{
//            //    var Result = oStreamReader.ReadToEnd();
//            //    string[] sAllKeys = Result.Split('\n');

//            //    bool bIsFound = false;
//            //    Hashtable oHashtable = new Hashtable();

//            //    // Here we collect all the data into hash table.
//            //    foreach (string sItem in sAllKeys)
//            //    {
//            //        if (sItem.IndexOf("mihpayid") != -1 || bIsFound)
//            //        {

//            //        }
//            //    }

//            //    // created objects are disposed.
//            //    oResponseMessage.Dispose();
//            //    oStreamReader.Dispose();
//            //}



//            //WebRequest oRequest = WebRequest.Create("https://qa.phicommerce.com/pg/api/sale");
//            //oRequest.Method = "POST";
//            //oRequest.ContentType = "application/x-www-form-urlencoded";
//            //oRequest.ContentLength = ArrMessage.Length;
//            //Stream oRequestStream = oRequest.GetRequestStream();
//            //oRequestStream.Write(ArrMessage, 0, ArrMessage.Length);
//            //// If required by the server, set the credentials.

//            ////request.Credentials = CredentialCache.DefaultCredentials;
//            //// Get the response.
//            //WebResponse response = oRequest.GetResponse();
//            //// Display the status.
//            ////Console.WriteLine(((HttpWebResponse)response).StatusDescription);
//            //// Get the stream containing content returned by the server.
//            //Stream dataStream = response.GetResponseStream();

//            //// Open the stream using a StreamReader for easy access.
//            //using (StreamReader reader = new StreamReader(dataStream))
//            //{
//            //    // Read the content.
//            //    string responseFromServer = reader.ReadToEnd();
//            //    // Display the content.

//            //    Console.WriteLine(responseFromServer);
//            //    // Clean up the streams and the response.
//            //    reader.Close();
//            //}
//            //response.Close();












//            if (msResponse["merchantTxnNo"] != null)
//                oNetBankingTransaction.PaymentReferenceNumber = msResponse["merchantTxnNo"].ToString();
//            else
//                oNetBankingTransaction.PaymentReferenceNumber = Constants.S_ZERO;

//            //if (msResponse["AMT"] != null)
//            //    oNetBankingTransaction.TransactionAMT = msResponse["AMT"].ToDouble();

//            // Need to remove below line with proper key/value.
//            oNetBankingTransaction.TransactionAMT = 1;

//            //if (msResponse["PMD"] != null)
//            //{
//            //    if (msResponse["PMD"].ToString() == "CD")
//            //        oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.CC.ToString();
//            //    else
//            //        oNetBankingTransaction.TransactionBankID = msResponse["bankcode"].ToString();
//            //}

//            oNetBankingTransaction.TransactionBankID = Constants.PayUMoneyPaymentModes.CC.ToString();

//            if (msResponse["txnID"] != null)
//                oNetBankingTransaction.TPSLTransactionID = msResponse["txnID"].ToString();

//            if (msResponse["responseCode"] != null)
//            {
//                string sHash = GetHash(oPaymentGateWayDetails, msResponse["merchantId"].ToString() + msResponse["merchantTxnNo"].ToString() + msResponse["paymentDateTime"].ToString() + msResponse["paymentID"].ToString() + msResponse["respDescription"].ToString() + msResponse["responseCode"].ToString() + msResponse["txnID"].ToString());
//                if (sHash == msResponse["secureHash"].ToString())
//                {
//                    oNetBankingTransaction.TransactionStatus = (msResponse["responseCode"].ToString() == oPaymentGateWayDetails.SuccessCode ? Constants.TransactionStatus.Completed : Constants.TransactionStatus.Failed);
//                }
//                else
//                    oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
//            }
//        }
//        else
//        {
//            oNetBankingTransaction.TransactionStatus = Constants.TransactionStatus.Failed;
//        }

//        return oNetBankingTransaction;
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
//}