// File Name  : InternalFeePaymentReceipt.aspx.cs
// Created By : Deepak
// Date       : 07/11/2009
//Description :This class is used print reciept for internal fees. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Globalization;
using System.Xml;

public partial class ExternalStudentsFeePaymentReceipt : SchoolBase
{  

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used decrypt query string and generate reciept.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			GetQueryString();
			if (!IsPostBack)
			{
				this.Page.Title = Constants.S_TITLE_FOR_PAGE;
				GetReceiptDetails();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		This method get reciept data and and set controls.
	/// </summary>
	private void GetReceiptDetails()
	{
        ExternalStudentFeeBL oExternalStudentFeeBL = new ExternalStudentFeeBL();
		int iReceiptNo = hidReceiptNo.Value.ToInt();
		int iExternalStudentFeeDetailsId = hidExternalStudentFeeId.Value.ToInt();
		int iAcademicYear = miAcademicYearId;
        DataTable oDTReceiptDetails = oExternalStudentFeeBL.GetRecieptDetails(iExternalStudentFeeDetailsId, iReceiptNo, miAcademicYearId, hidAccountHeaderId.Value.ToInt());
            
        if (oDTReceiptDetails.Rows.Count > 0)
        {
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                LblReceiptHeader.Text = Convert.ToString(oDTReceiptDetails.Rows[Constants.I_ZERO]["AccountHeader"]);
                trSchoolTelephone.Visible = true;
                trSchoolAddress.Visible = true;
                lblRefundableNote.Visible = true;                
            }
            else
            {
                trSchoolTelephone.Visible = false;
                trSchoolAddress.Visible = false;
                trRemarkDetails.Visible = true;
               
                lblRefundableNote.Visible = false;
                trFeeType.Visible = true;
            }
            lblDataRcptNo.Text = Convert.ToString(oDTReceiptDetails.Rows[Constants.I_ZERO]["ReceiptNumber"]);
            DateTime oDt = oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentDate"].ToString().ToDateTime();
            lblDataPaymentDate.Text = oDt.ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));
            lblDataStudentName.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["StudentName"].ToString();
            lblFeeType.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["FeeType"].ToString();                     

            lblRemarks.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["Remark"].ToString();
            string sAmount = oDTReceiptDetails.Rows[Constants.I_ZERO]["Amount"].ToString();
            lblDataAmount.Text = sAmount;
            lblCreaterName.Text = Resources.LocalizedResources.Creator + oDTReceiptDetails.Rows[Constants.I_ZERO]["Generator"].ToString();
            string strAmount = CommonUtility.GetNumberInWords(sAmount);
            lblRsInWords.Text = strAmount;            

            trChequeDetails.Visible = false;            
            lblChequeNote.Visible = false;

            if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentMode"].ToInt() == Constants.I_TWO)
            {
                DataTable dtChequeDetails = oDTReceiptDetails;
                if (dtChequeDetails.IsNonEmpty())
                {
                    trChequeDetails.Visible = true;
                    lblCheckNo.Text = dtChequeDetails.Rows[0]["ChequeNo"].ToString();
                    lblChequeDate.Text = dtChequeDetails.Rows[0]["ChequeDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    lblBankName.Text = dtChequeDetails.Rows[0]["Bank_Name"].ToString();
                    lblAmount.Text = sAmount;          
                    
                    lblPaymentType.Text = "By Cheque";                    

                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                        tdSNSChequeDetails.Visible = true;
                    else
                    {
                        tdSNSChequeDetails.Visible = false;
                        lblChequeNote.Visible = true;
                        lblRefundableNote.Visible = true;
                    }
                }
            }
            else if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentMode"].ToInt() == Constants.I_ONE)
            {
                trChequeDetails.Visible = false;
                lblPaymentType.Text = "By Cash";
            }
            else if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentMode"].ToInt() == Constants.I_THREE)
            {
                trElectronicDetails.Visible = true;
                lblPaymentType.Text = "By Electronic";

                lblTransactionNo.Text = oDTReceiptDetails.Rows[0]["TransactionNo"].ToString();
                lblTransactionType.Text = oDTReceiptDetails.Rows[0]["ElectronicTypeName"].ToString();
                lblTransactionBank.Text = oDTReceiptDetails.Rows[0]["Bank_Name"].ToString();
                lblTransactionAmount.Text = sAmount;
             }
           
        }
	}

	/// <summary>
	///		This method get value from querystring parameter and set it to hidden variables.
	/// </summary>
	private void GetQueryString()
	{

        if (!QueryString["ExternalStudentFeeId"].IsNullOrEmpty())
            hidExternalStudentFeeId.Value = QueryString["ExternalStudentFeeId"];

        if (!QueryString["AcademicYear"].IsNullOrEmpty())
            hidAcaYear.Value = QueryString["AcademicYear"];

        if (!QueryString["ReceiptNo"].IsNullOrEmpty())
            hidReceiptNo.Value = QueryString["ReceiptNo"];

        if (!QueryString["AccountHeaderId"].IsNullOrEmpty())
            hidAccountHeaderId.Value = QueryString["AccountHeaderId"];
	}

	#endregion -- PRIVATE METHOD(s) --

}
