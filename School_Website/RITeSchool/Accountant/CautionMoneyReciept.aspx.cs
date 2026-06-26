/*-------------------------------------------------------------------------------
 *	MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 21-Mar-2012
 *	Purpose		: Instead of using value from setting file, amount displayed on
 *				  receipt is now fetched from the database.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Data;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;
using CrystalDecisions.Shared;

public partial class CautionMoneyReciept : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string STUDENT_DATA = " RECEIVED WITH THANKS FROM ";
	private const string CHEQUE_NOTE = "*SUBJECT TO REALISATION OF CHEQUE";
	private const string REFUND_NOTE = "**NO REFUND OF DEPOSIT POSSIBLE IF RECEIPT IN ORIGINAL IS NOT SUBMITTED";

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
            //if (!IsPostBack)
            //{
				this.Page.Title = Constants.S_TITLE_FOR_PAGE;
				GetQueryString();
				//GetaDataForReciept();
				SetQueryString();

                if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                {
                    trPageSetting.Visible = false;
                    trLogo.Visible = true;
                }
                else
                {
                    trPageSetting.Visible = true;
                    trLogo.Visible = false;
                }

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
	/// This function sets the form fields according to the query string values.
	/// </summary>
	private void GetQueryString()
	{
		if (QueryString.Count > 0)
		{
			if (!QueryString["StudentId"].IsNullOrEmpty())
				hidStudentId.Value = QueryString["StudentId"];

			if (!QueryString["CautionMode"].IsNullOrEmpty())
				hidCautionMode.Value = QueryString["CautionMode"];

			if (!QueryString["PageIndex"].IsNullOrEmpty())
				hidPageIndex.Value = QueryString["PageIndex"];

			if (!QueryString["StudentRegNo"].IsNullOrEmpty())
				hidStudentRegNo.Value = QueryString["StudentRegNo"];
            if (!QueryString["StudentCautionMoneyId"].IsNullOrEmpty())
                hidStudentCautionMoneyId.Value = QueryString["StudentCautionMoneyId"];
			if (!QueryString["StudentRegNo"].IsNullOrEmpty())
				hidPostBackUrl.Value = QueryString["PostBackUrl"];

            if (!QueryString["IsReturnMode"].IsNullOrEmpty())
            {
                hidIsReturnMode.Value = QueryString["IsReturnMode"].ToString();
                trFeePaidBy.Visible = false;
            }
            else
                hidIsReturnMode.Value = Constants.S_ZERO;
		}
		else
			hidQueryString.Value = String.Empty;
	}

	/// <summary>
	/// This method is used to display receipt details. (set values to respective controls)
	/// </summary>
	private void GetaDataForReciept()
	{
		int iStudentId = hidStudentId.Value.ToInt();
		var oStudentCautionMoneyDetailsBL = new StudentCautionMoneyDetailsBL();
        DataTable odtRecieptData = oStudentCautionMoneyDetailsBL.GetDataForReciept(iStudentId, miSchoolId, hidIsReturnMode.Value);
		if (odtRecieptData != null && odtRecieptData.Rows.Count > 0)
			DisplayReciept(odtRecieptData);

        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        spnSchoolName.InnerText = oSchoolBL.SchoolName.ToString();
        spnAddress.InnerText = oSchoolBL.Address1.ToString();
        spnAddress1.InnerText = oSchoolBL.Address2.ToString() +", "+ oSchoolBL.City.ToString() + "-" + oSchoolBL.Pincode.ToString() + " Tel.:" + oSchoolBL.PhoneNumber.ToString() ;
        spnWebsite.InnerText = "Website:" + oSchoolBL.WebSite.ToString() + ", " + "Email Address:" +  oSchoolBL.Email.ToString();
        imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + Constants.SchoolLogos.SchoolLogo.ToInt().ToString();
	}

	private void DisplayReciept(DataTable aodtRecieptData)
	{
		lblDataPaymentDate.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Payment_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));

		int iReceiptMinimumDigits = Settings.ReceiptMinimumDigits;
		string sReceiptNumber = aodtRecieptData.Rows[Constants.I_ZERO]["Receipt_Number"].ToString();
		lblDataRcptNo.Text = sReceiptNumber.Length >= iReceiptMinimumDigits ? sReceiptNumber : sReceiptNumber.PadLeft(iReceiptMinimumDigits, '0');

        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            trPPSHFoMo.Visible = true;
            lblStudentName.Text = Resources.LocalizedResources.RecievedWithThanksFrom + "&nbsp;&nbsp;&nbsp;&nbsp;" + aodtRecieptData.Rows[Constants.I_ZERO]["PaidByName"].ToString();
            lblPPSHFoMo.Text = "FO / MO " + "&nbsp;&nbsp;&nbsp;&nbsp;" + aodtRecieptData.Rows[Constants.I_ZERO]["StudentName"].ToString();
            trFeePaidBy.Visible = false;
        }
        else
        {
            if (hidIsReturnMode.Value != Constants.S_ONE)
            {
                lblStudentName.Text = Resources.LocalizedResources.RecievedWithThanksFrom + "&nbsp;&nbsp;&nbsp;&nbsp;" + aodtRecieptData.Rows[Constants.I_ZERO]["StudentName"].ToString();
                trFeePaidBy.Visible = true;
            }
            else
            {
                lblStudentName.Text = "GIVEN TO : " + "&nbsp;&nbsp;" + aodtRecieptData.Rows[Constants.I_ZERO]["StudentName"].ToString();
            }
        }

		string sCautionMoneyAmt = aodtRecieptData.Rows[Constants.I_ZERO]["Amount"].ToString();
		string sCautionMoneyAmtInWord = CommonUtility.GetNumberInWords(sCautionMoneyAmt);
		lblAmount.Text = sCautionMoneyAmt + "/- ";

        if (aodtRecieptData.Rows[Constants.I_ZERO]["ConcessionAmount"].ToString() != Constants.S_ZERO)
        {
            trConcessionAmount.Visible = true;
            string sConcessionAmount = aodtRecieptData.Rows[Constants.I_ZERO]["ConcessionAmount"].ToString();
            lblConcession.Text = sConcessionAmount + "/- ";
        }
        else
            trConcessionAmount.Visible = false;

        lblMoney.Text = String.Format(Resources.LocalizedResources.Sum + "{0}/- ({1}) " + Resources.LocalizedResources.TowardsRefundable, sCautionMoneyAmt, sCautionMoneyAmtInWord);
        lblNote.Text = " " + Resources.LocalizedResources.CautionMoneyDuringAdmissionTo + " ";
		lblStandard.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Standard_Name"].ToString();
		if (aodtRecieptData.Rows[Constants.I_ZERO]["Form_Number"].ToString().IsNullOrEmpty())
			lblGRNumber.Text = "-";
		else
			lblGRNumber.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Form_Number"].ToString();

		if ((hidIsReturnMode.Value == Constants.S_ZERO && aodtRecieptData.Rows[Constants.I_ZERO]["Payment_Mode"].ToString() == "Q") ||
            (hidIsReturnMode.Value == Constants.S_ONE && aodtRecieptData.Rows[Constants.I_ZERO]["Return_Mode"].ToString() == "Q"))
		{
			lblChequeNo.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Cheque_Number"].ToString();
			lblChequeDate.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));
			lblBankName.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Bank_Name"].ToString();
		}
        else if ((hidIsReturnMode.Value == Constants.S_ZERO && aodtRecieptData.Rows[Constants.I_ZERO]["Payment_Mode"].ToString() == "E") ||
            (hidIsReturnMode.Value == Constants.S_ONE && aodtRecieptData.Rows[Constants.I_ZERO]["Return_Mode"].ToString() == "E"))
        {
            lblChequeNo.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Electronic_Payment_TranNo"].ToString();
            lblChequeDate.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));
            lblBankName.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Electronic_Payment_Bank"].ToString();
        }
        else if (aodtRecieptData.Rows[Constants.I_ZERO]["Payment_Mode"].ToString() == "N" && Settings.EnableOnlinePaymentForCautionMoney)
        {
            lblChequeNo.Text = aodtRecieptData.Rows[Constants.I_ZERO]["NetBankingPaymentTransactionID"].ToString();
            lblChequeDate.Text = aodtRecieptData.Rows[Constants.I_ZERO]["Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT, new CultureInfo("en"));
            lblBankName.Text = aodtRecieptData.Rows[Constants.I_ZERO]["RegisterdBankName"].ToString();

            if(moUserRole == Constants.UserRoles.Student)
                trReceiptDisplay.Visible = true;
        }
        else
        {
            lblChequeNo.Text = Resources.LocalizedResources.Cash;
            lblChequeDate.Text = "-";
            lblBankName.Text = "-";
        }
        lblRemark.Text = (hidIsReturnMode.Value == Constants.S_ONE ? aodtRecieptData.Rows[Constants.I_ZERO]["ReturnRemark"].ToString() : aodtRecieptData.Rows[Constants.I_ZERO]["PaymentRemark"].ToString());
        lblRefund.Text = Resources.LocalizedResources.TextSubmitOriginalReceipt;
        lblSub.Text = Resources.LocalizedResources.SubjectToRelaeseOfCheque;
        lblCreaterName.Text = Resources.LocalizedResources.Creator + aodtRecieptData.Rows[Constants.I_ZERO]["Generator"].ToString();
        lblFeePaidBy.Text = aodtRecieptData.Rows[Constants.I_ZERO]["PaidByName"].ToString();

	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		if (!hidCautionMode.Value.IsNullOrEmpty())
		{
			string sQueryString;
			if (hidPostBackUrl.Value != "~/StudentPayFeeUI.aspx")
				sQueryString = String.Format("StudentId={0}&CautionMode={1}&StudentRegNo={2}&PageIndex={3}&PageStatus=Close", hidStudentId.Value, hidCautionMode.Value, hidStudentRegNo.Value, hidPageIndex.Value);
			else
			{
				var oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
				int iYearwiseStudentId = oStudentCautionMoneyDetailsCollectionBL.GetStudentAcademicYearId(hidStudentId.Value.ToInt(), miSchoolId, miAcademicYearId);
				hidYearwiseStudentId.Value = Convert.ToString(iYearwiseStudentId);
				sQueryString = String.Format("StudentId={0}&CautionMode={1}&StudentRegNo={2}&PageIndex={3}&PageStatus=Close", hidYearwiseStudentId.Value, hidCautionMode.Value, hidStudentRegNo.Value, hidPageIndex.Value);
			}
			string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
			sQueryString = "'?" + sEncryptQueryString + "'";
			hidQueryString.Value = sQueryString;
		}
		else
			hidQueryString.Value = String.Empty;
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

            kvp.Add("SchoolId", miSchoolId.ToString());
            kvp.Add("StudentId", hidStudentId.Value);
            kvp.Add("StudentCautionMoneyId", hidStudentCautionMoneyId.Value);
            
            ReportDisplay oReportDisplay = new ReportDisplay();
            if (moSchool == Constants.SchoolId.SNS && moUserRole == Constants.UserRoles.Student)
                crReportDocument = oReportDisplay.GetReportDocument(Constants.ExportReports.StudentCautionMoneySNSForStudentLogin, kvp);
            else
            {
                kvp.Add("IsReturnMode", Constants.S_ZERO);
                crReportDocument = oReportDisplay.GetReportDocument(Constants.ExportReports.CautionMoneyReceipt, kvp);
            }

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
