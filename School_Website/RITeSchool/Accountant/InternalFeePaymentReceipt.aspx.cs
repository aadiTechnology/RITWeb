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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class InternalFeePaymentReceipt : SchoolBase
{
    private bool IsNextYearPayment
    {
        get { return (hidIsNextYearFeePayment.Value == Constants.S_ZERO ? false : true); }
    }

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
            //if (!IsPostBack)
            //{
				this.Page.Title = Constants.S_TITLE_FOR_PAGE;
				//GetReceiptDetails();
                DisplayReport();               
            //}
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
		int iReceiptNo = hidReceiptNo.Value.ToInt();
		int iInternalFeeDetailsId = hidInternalFeeDetailsId.Value.ToInt();
		int iAcademicYear = miAcademicYearId;
		int iStudentId = hidStudentId.Value.ToInt();
        DataSet oDS = InternalFeeDetailsBL.GetRecieptDetails(miSchoolId, iReceiptNo, iInternalFeeDetailsId, miAcademicYearId, iAcademicYear, iStudentId, hidDuplicateInternalFeeDetailsId.Value.ToInt(), IsNextYearPayment, hidSerialNo.Value.ToInt());
        DataTable oDTReceiptDetails = oDS.Tables[0];
        if (oDTReceiptDetails.Rows.Count > 0)
        {
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                //trReceiptHeader.Visible = true;
                LblReceiptHeader.Text = Convert.ToString(oDTReceiptDetails.Rows[Constants.I_ZERO]["AccountHeaderName"]);
                trSchoolTelephone.Visible = true;
                trSchoolAddress.Visible = true;
                //trAddressLine.Visible = true;
                trRemarkDetails.Visible = false;
                tdRegNo.Visible = true;
                tdRegNoLbl.Visible = true;
                lblStudentRegNo.Text = Convert.ToString(oDTReceiptDetails.Rows[Constants.I_ZERO]["StudentRegNo"]);
                lblRefundableNote.Visible = true;
                trFeeType.Visible = false;
            }
            else
            {
                //trReceiptHeader.Visible = false;
                trSchoolTelephone.Visible = false;
                trSchoolAddress.Visible = false;
                //trAddressLine.Visible = false;
                trRemarkDetails.Visible = true;
                tdRegNo.Visible = false;
                tdRegNoLbl.Visible = false;
                lblRefundableNote.Visible = false;
                trFeeType.Visible = true;
            }
            lblDataRcptNo.Text = Convert.ToString(oDTReceiptDetails.Rows[Constants.I_ZERO]["ReceiptNo"]);
            DateTime oDt = oDTReceiptDetails.Rows[Constants.I_ZERO]["PaidDate"].ToString().ToDateTime();
            lblDataPaymentDate.Text = oDt.ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));
            lblDataStudentName.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["StudentName"].ToString();
            lblFeeType.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["Fee_Type"].ToString();
            lblPayableFor.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["Payable_For"].ToString();
            if (oDTReceiptDetails.Rows[Constants.I_ZERO]["Class"].ToString() != String.Empty)
            {
                lblDataClass.Visible = true;
                lblDataClass.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["Class"].ToString();
            }
            else
            {
                lblDataClass.Visible = true;
                lblDataClass.Text = "-";
            }

            lblRemarks.Text = oDTReceiptDetails.Rows[Constants.I_ZERO]["Remark"].ToString();
            string sAmount = oDTReceiptDetails.Rows[Constants.I_ZERO]["Amount"].ToString();
            lblDataAmount.Text = sAmount;
            lblCreaterName.Text = Resources.LocalizedResources.Creator + oDTReceiptDetails.Rows[Constants.I_ZERO]["Generator"].ToString();
            string strAmount = CommonUtility.GetNumberInWords(sAmount);
            lblRsInWords.Text = strAmount;
            string sSex = oDTReceiptDetails.Rows[Constants.I_ZERO]["Sex"].ToString();
            if (sSex.Trim() == "F")
            {
                lblMaster.Style.Add(HtmlTextWriterStyle.TextDecoration, "line-through");
                lblMiss.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
            }
            else
            {
                lblMiss.Style.Add(HtmlTextWriterStyle.TextDecoration, "line-through");
                lblMaster.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
            }

            trChequeDetails.Visible = false;
            trNetBankingDetails.Visible = false;
            lblChequeNote.Visible = false;
            trElectroniDetails.Visible = false;

            if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentTypeId"].ToInt() == Constants.PaymentMode.Cheque.ToInt())
            {
                DataTable dtChequeDetails = oDS.Tables[1];
                if (dtChequeDetails.IsNonEmpty())
                {
                    trChequeDetails.Visible = true;
                    lblCheckNo.Text = dtChequeDetails.Rows[0]["ChequeNumber"].ToString();
                    lblChequeDate.Text = dtChequeDetails.Rows[0]["ChequeDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    lblBankName.Text = dtChequeDetails.Rows[0]["BankName"].ToString();
                    lblAmount.Text = dtChequeDetails.Rows[0]["ChequeAmount"].ToString();

                    if (miSchoolId != Constants.SchoolId.PPS.ToInt())
                        lblPaymentType.Text = "By Cheque";
                    else
                        lblPaymentType.Text = "By Cheque / DD";

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
           else if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentTypeId"].ToInt() == 3)//NetBanking 
            {
                DataTable dtNetBankingDetails = oDS.Tables[2];
                if (dtNetBankingDetails.IsNonEmpty())
                {
                    trNetBankingDetails.Visible = true;
                    lblTransactionNo.Text = dtNetBankingDetails.Rows[0]["TPSLTransactionID"].ToString();
                    lblTransactionDate.Text = dtNetBankingDetails.Rows[0]["TransactionDateTime"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    lblTransactionBankName.Text = dtNetBankingDetails.Rows[0]["RegisterdBankName"].ToString();
                    lblTransactionAmount.Text = dtNetBankingDetails.Rows[0]["TransactionAMT"].ToString();

                    lblPaymentType.Text = "By Netbanking";

                }
            }
            else if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentTypeId"].ToInt() == Constants.PaymentMode.Cash.ToInt())
            {
                lblPaymentType.Text = "By Cash";
            }
            else if (oDTReceiptDetails.Rows[Constants.I_ZERO]["PaymentTypeId"].ToInt() == Constants.PaymentMode.Electronic.ToInt())
            {
                DataTable dtElectronic = oDS.Tables[3];
                if (dtElectronic.IsNonEmpty())
                {
                    trElectroniDetails.Visible = true;
                    lblETransactionNo.Text = dtElectronic.Rows[0]["TransactionNumber"].ToString();
                    lblEPaymentType.Text = dtElectronic.Rows[0]["Type"].ToString();
                    lblEBankName.Text = dtElectronic.Rows[0]["Bank_Name"].ToString();
                    lblEAmount.Text = dtElectronic.Rows[0]["Amount"].ToString();

                    lblPaymentType.Text = "By Electronic Payment";
                }
            }
        }
	}

	/// <summary>
	///		This method get value from querystring parameter and set it to hidden variables.
	/// </summary>
	private void GetQueryString()
	{
        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            trExtraSpace.Visible = true;
        else
            trExtraSpace.Visible = false;

		if (!QueryString["ReceiptNo"].IsNullOrEmpty())
			hidReceiptNo.Value = QueryString["ReceiptNo"];

        hidSerialNo.Value = Constants.S_ZERO;
        if (!QueryString["SerialNumber"].IsNullOrEmpty())
			hidSerialNo.Value = QueryString["SerialNumber"];

		if (!QueryString["AcademicYear"].IsNullOrEmpty())
			hidAcaYear.Value = QueryString["AcademicYear"];
		
		if (!QueryString["StudentId"].IsNullOrEmpty())
			hidStudentId.Value = QueryString["StudentId"];
		
		if (!QueryString["InternalFeeDetailsId"].IsNullOrEmpty())
			hidInternalFeeDetailsId.Value = QueryString["InternalFeeDetailsId"];
		
		if (!QueryString["RegNo"].IsNull())
			hidRegNo.Value = QueryString["RegNo"];
		
		if (!QueryString["FromDate"].IsNull())
			hidFromDate.Value = QueryString["FromDate"];
		
		if (!QueryString["ToDate"].IsNull())
			hidToDate.Value = QueryString["ToDate"];
		
		if (!QueryString["ToDate"].IsNull())
			hidIncludePaid.Value = QueryString["IncludePaid"];
		
		if (!QueryString["ToDate"].IsNull())
			hidPayForNextYear.Value = QueryString["PayForNextYear"];
		
		if (!QueryString["ToDate"].IsNull())
			hidIsRegNoFilter.Value = QueryString["IsRegNoFilter"];
		
		if (!QueryString["ToDate"].IsNull())
			hidStandardID.Value = QueryString["StandardID"];
		
		if (!QueryString["ToDate"].IsNull())
			hidDivisionID.Value = QueryString["DivisionID"];
		
		if (!QueryString["ToDate"].IsNull())
			hidFeeTypeID.Value = QueryString["FeeTypeID"];
		
		if (!QueryString["ToDate"].IsNull())
			hidPageIndex.Value = QueryString["pIndex"];

        if (!QueryString["Date"].IsNull())
            hidDate.Value = QueryString["Date"].ToString();

        if (!QueryString["AccountHeaderId"].IsNull())
            hidAccountHeaderId.Value = QueryString["AccountHeaderId"].ToString();
        
        if (!QueryString["DuplicateInternalFeeDetailsId"].IsNullOrEmpty())
            hidDuplicateInternalFeeDetailsId.Value = QueryString["DuplicateInternalFeeDetailsId"];
        else hidDuplicateInternalFeeDetailsId.Value = Constants.S_ZERO;

        hidIsNextYearFeePayment.Value = Constants.S_ZERO;
        if (!QueryString["IsNextYearFeePayment"].IsNull())
            hidIsNextYearFeePayment.Value = QueryString["IsNextYearFeePayment"].ToString();

		hidQueryString.Value = "?" + CommonUtility.EncryptQuerystring(String.Format("RegNo={0}&StandardID={1}&DivisionID={2}&FeeTypeID={3}",
																					 hidRegNo.Value, 																					 
																					 hidStandardID.Value, 
																					 hidDivisionID.Value, 
																					 hidFeeTypeID.Value 
																					 ));
	}

    /// <summary>
    /// This method is used to display report in viewer.
    /// </summary>
    private void DisplayReport()
    {
        ReportDocument crReportDocument;
        try
        {
            Dictionary<string, string> kvp = new Dictionary<string, string>();

            if (moSchool == Constants.SchoolId.SNS)
            {
                kvp.Add("school_id", miSchoolId.ToString());
                kvp.Add("academic_Year_Id", miAcademicYearId.ToString());
                kvp.Add("FromDate", hidDate.Value);
                kvp.Add("ToDate", hidDate.Value);
                kvp.Add("AccountHeaderId", hidAccountHeaderId.Value);
                kvp.Add("Standard_Id", string.Empty);
                kvp.Add("ReceiptNumber", hidReceiptNo.Value);
            }
            else
            {
                kvp.Add("SchoolId", miSchoolId.ToString());
                kvp.Add("AcademicYearId", miAcademicYearId.ToString());
                kvp.Add("CurrAcadYerId", miAcademicYearId.ToString());
                kvp.Add("ReceiptNo", hidReceiptNo.Value);
                kvp.Add("InternalFeeDetailsId", hidInternalFeeDetailsId.Value);
                kvp.Add("Schoolwise_Student_Id", hidStudentId.Value);
                kvp.Add("DuplicateInternalFeeDetailsId", hidDuplicateInternalFeeDetailsId.Value);
                kvp.Add("IsNextYearPayment", IsNextYearPayment.ToString());
                kvp.Add("SerialNumber", hidSerialNo.Value);
            }

            ReportDisplay oReportDisplay = new ReportDisplay();

            if(moSchool == Constants.SchoolId.SNS)
                crReportDocument = oReportDisplay.GetReportDocument(Constants.ExportReports.InternalFeeReceiptSNS, kvp);
            else
                crReportDocument = oReportDisplay.GetReportDocument(Constants.ExportReports.InternalFeeReceipt, kvp);

            reportViewer.AllowedExportFormats = ViewerExportFormats.PdfFormat.ToInt();
            reportViewer.ReportSource = crReportDocument;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            crReportDocument = null;
        }
    }

	#endregion -- PRIVATE METHOD(s) --

}
