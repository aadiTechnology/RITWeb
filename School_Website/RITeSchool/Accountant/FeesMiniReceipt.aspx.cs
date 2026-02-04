// File Name   : BankDetailsPopup.aspx.cs
// Created By  : -
// Date        : -
// Modified By : Milind
// Date        : 10 Sept 09
// Description : This class is used to display the student paid fee receipt.
// Modified by : Rohini
// Description : Created new  report.

using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Utility;
using System.Collections.Generic;

public partial class FeesMiniReceipt : SchoolBase
{
	#region -- MEMBER(s) --

	private ReportDocument crReportDocument;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to decrypt query string and display receipt.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			GetQueryString();            

			if (!IsPostBack)
				this.Page.Title = Constants.S_TITLE_FOR_PAGE;

            if (moSchool == Constants.SchoolId.VPMCPS)
                DisplayReport();
            else
                DisplayReport(GetFilterString());
		}
		catch (ThreadAbortException)
		{
			// Empty Catch block.
			// This exception is raised when Reponse.End is called.
			// We just swallow the exception here.
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    private void DisplayReport()
    {
        ReportDocument crReportDocument;
        try
        {   
            int iAcademicYearId;
            if (QueryString["Academic_Year_ID"] == null)
                iAcademicYearId = miAcademicYearId;
            else
                iAcademicYearId = QueryString["Academic_Year_ID"].ToInt();

            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            DataTable dtReceiptDetails = oStudentFeeDetailsBL.GetReceiptDetailsForStudent(miSchoolId, iAcademicYearId, QueryString["ReceiptNo"].ToString(), QueryString["StudentId"].ToInt());
            
            Dictionary<string, string> kvp = new Dictionary<string, string>();

            kvp.Add("School_Id", miSchoolId.ToString());
            kvp.Add("Academic_Year_ID", iAcademicYearId.ToString());
            kvp.Add("ReceiptNo", QueryString["ReceiptNo"] ?? Constants.S_ZERO);
            kvp.Add("Standard_Id", dtReceiptDetails.Rows[0]["Standard_Id"].ToString());
            kvp.Add("Division_id", dtReceiptDetails.Rows[0]["Division_id"].ToString());
            kvp.Add("StartDate", Convert.ToDateTime(dtReceiptDetails.Rows[0]["Paid_Date"]).ToString(Constants.S_DATE_FORMAT));
            kvp.Add("EndDate", Convert.ToDateTime(dtReceiptDetails.Rows[0]["Paid_Date"]).ToString(Constants.S_DATE_FORMAT));

            ReportDisplay oReportDisplay = new ReportDisplay();
            crReportDocument = oReportDisplay.GetReportDocument(Constants.ExportReports.StudentFeeReceipt, kvp);
            
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

	/// <summary>
	///		Disposes the ReportDocument object.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnUnload(EventArgs e)
	{
		try
		{
			base.OnUnload(e);
			
			if (crReportDocument != null)
			{
				crReportDocument.Close();
				crReportDocument.Dispose();
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
	/// This method is used to display report
	/// </summary>
	/// <param name="msFilter"></param>
	private void DisplayReport(string msFilter)
	{
		crReportDocument = new ReportDocument();
		var crConnectionInfo = new ConnectionInfo();
		var crtableLogoninfos = new TableLogOnInfos();
		var crtableLogoninfo = new TableLogOnInfo();

		crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
		crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
		crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
		crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];

        string sReportName = "FeeReciept.rpt";
        if(!Settings.IsMiniSite)
        {
            if(miSchoolId == Constants.SchoolId.SS.ToInt())
                sReportName = "FeeRecieptSS.rpt";
            else if(miSchoolId == Constants.SchoolId.PPS.ToInt() )
                sReportName = "FeeRecieptForPP.rpt";
            //else if (miSchoolId == Constants.SchoolId.JPS.ToInt())
            //    sReportName = "FeeRecieptJPS.rpt";
            else if(miSchoolId == Constants.SchoolId.SNS.ToInt() && hidIsRefundFee.Value == Constants.S_ZERO)
                sReportName = "FeeRecieptForSNS.rpt";
            else if (miSchoolId == Constants.SchoolId.SNS.ToInt() && hidIsRefundFee.Value == Constants.S_ONE)
                sReportName = "RefundFeeRecieptForSNS.rpt";
			else if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                sReportName = "FeeRecieptPPSN.rpt";
            else if (miSchoolId == Constants.SchoolId.BMFS.ToInt())
            {
                bool bIsPreprimaryStudent = false;
                
                StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                DataTable dt = oStudentFeeDetailsBL.CheckStudentsStandardDetails(hidReceiptNo.Value.ToInt(), miSchoolId, miAcademicYearId);
                if (dt.Rows.Count > Constants.I_ZERO)
                {
                    if (dt.Rows[0]["Is_PrePrimary"].ToString() == "Y")
                        bIsPreprimaryStudent = true;
                }                

                if (bIsPreprimaryStudent)
                    sReportName = "FeeRecieptBMFS_PrePrimary.rpt";
                else
                    sReportName = "FeeRecieptBMFS.rpt";
            }

        }

        string sPath = Server.MapPath("~") + @"\RITeSchool\Report\Fee\" + sReportName;

        crReportDocument.Load(sPath);

		SetFeeRecieptDatascource(msFilter);        

		//Restrict Export formats to PDF only.
		reportViewer.AllowedExportFormats = ViewerExportFormats.PdfFormat.ToInt();

		reportViewer.ReportSource = crReportDocument;

		crtableLogoninfo = null;
		crtableLogoninfos = null;
		crConnectionInfo = null;
	}

	/// <summary>
	/// This methd is used to get filter string.
	/// </summary>
	/// <returns></returns>
	private string GetFilterString()
	{
        if (miSchoolId != Constants.SchoolId.SNS.ToInt())
        {
            string sReceiptNumber = Constants.S_EXPORT_PENDING_FEE + ".ReceiptNumber}";
            string sAcademicYearID = Constants.S_EXPORT_PENDING_FEE + ".AcademicYearID}";
            return String.Format("({0}={1} AND {2}={3}",
                                  sReceiptNumber,
                                  QueryString["ReceiptNo"] ?? Constants.S_ZERO,
                                  sAcademicYearID,
                                  QueryString["AcademicYear"] ?? miAcademicYearId.ToString());
        }
        else
        {
            string sReceiptNumber = Constants.S_EXPORT_PENDING_FEE + ".ReceiptNumber}";
            string sAcademicYearID = Constants.S_EXPORT_PENDING_FEE + ".AcademicYearID}";
            string sAccountHeaderId = Constants.S_EXPORT_PENDING_FEE + ".AccountHeaderId}";
            string sIsRefundReport = Constants.S_EXPORT_PENDING_FEE + ".IsRefundReport}";
            return String.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                                  sReceiptNumber,
                                  QueryString["ReceiptNo"] ?? Constants.S_ZERO,
                                  sAcademicYearID,
                                  QueryString["AcademicYear"] ?? miAcademicYearId.ToString(),
                                  sAccountHeaderId,
                                  QueryString["AccountHeaderId"] ?? Constants.S_ZERO,
                                  sIsRefundReport,
                                  hidIsRefundFee.Value);
        }
    }

    // <summary>
	/// This method is used to set data source to report.
	/// </summary>
	/// <param name="asReportSelectionString"></param>
	private void SetFeeRecieptDatascource(string asReportSelectionString)
	{
		var dsFeeRecieptReportDetails = new DataSet();
		asReportSelectionString = FormatFilterString(asReportSelectionString);
		String[] sFilters = asReportSelectionString.Split('@');
		string sParameterValue;
		string sParameterField;
        string sReceiptNumber = Constants.S_ZERO;
		int sRecieptNo = 0;
		int iAcademicYearId = 0;
        int iAccountHeaderId = 0;
        int iIsRefundFee = 0;
		foreach (string filter in sFilters.Where(str => !str.IsNullOrEmpty()))
		{
			sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);
			sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                switch (sParameterField)
                {
                    case "ReceiptNumber":
                        sReceiptNumber = sParameterValue;
                        break;
                    case "AcademicYearID":
                        iAcademicYearId = sParameterValue.ToInt();
                        break;
                    case "AccountHeaderId":
                        iAccountHeaderId = sParameterValue.ToInt();
                        break;
                    case "IsRefundReport":
                        iIsRefundFee = sParameterValue.ToInt();
                        break;
                }
            }
            else
            {
                switch (sParameterField)
                {
                    case "ReceiptNumber":
                        sRecieptNo = sParameterValue.ToInt();
                        break;
                    case "AcademicYearID":
                        iAcademicYearId = sParameterValue.ToInt();
                        break;
                    case "AccountHeaderId":
                        iAccountHeaderId = sParameterValue.ToInt();
                        break;
                }
            }
		}

        if (hidSubmissionID.Value == Constants.S_ZERO && hidSerialNo.Value == Constants.S_ZERO)
        {
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                int iStudentId = 0;
                if (QueryString["StudentId"] != null && QueryString["StudentId"].ToString() != string.Empty)
                    iStudentId = QueryString["StudentId"].ToInt();

                bool bIsRefundFee = false;

                if (iIsRefundFee == Constants.I_ONE)
                    bIsRefundFee = true;

                dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetailsForSNS(sReceiptNumber, iAcademicYearId, iAccountHeaderId, iStudentId, bIsRefundFee);
            }
            else
                dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetails(sRecieptNo, iAcademicYearId);
        }
        else if (hidSubmissionID.Value != Constants.S_ZERO)
            dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetAdmissionReceiptDetails(hidSubmissionID.Value.ToInt(), iAcademicYearId);
        else if (hidSerialNo.Value != Constants.S_ZERO)
            dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetails(hidSerialNo.Value.ToInt());

		dsFeeRecieptReportDetails.Tables[0].TableName = "StudentDetails";
		dsFeeRecieptReportDetails.Tables[1].TableName = "PaymentmodeDetails";
		if (dsFeeRecieptReportDetails.Tables.Count == 3)
			dsFeeRecieptReportDetails.Tables[2].TableName = "FeeDetails";
		crReportDocument.SetDataSource(dsFeeRecieptReportDetails);
	}

	/// <summary>
	/// This method is used to convert string into particular format.
	/// </summary>
	/// <param name="asFilterString"> </param>
	/// <returns>string</returns>
	private string FormatFilterString(string asFilterString)
	{
		asFilterString = asFilterString.Replace("AND", "@");
		asFilterString = asFilterString.Replace("OR", "@");
		asFilterString = asFilterString.Replace("(", String.Empty);
		asFilterString = asFilterString.Replace(")", String.Empty);
		asFilterString = asFilterString.Replace("{", String.Empty);
		asFilterString = asFilterString.Replace("}", String.Empty);
		
		return asFilterString;
	}

	/// <summary>
	/// This function sets the form fields according to the query string values.
	/// </summary>
	private void GetQueryString()
	{
		if (QueryString.Count <= 0)
			return;
		
		if (!QueryString["iAdmissionId"].IsNull())
		{
			this.ErrorPage = "~/RITeSchool/Admission/Error.aspx";
			if (Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] == null && QueryString["FormNo"]==null && moUserRole != Constants.UserRoles.Student)
			{
				Response.Write("<Script language='Javascript'>");
				Response.Write("window.close();");
				Response.Write("</script>");
				Response.End();
			}

			hidSubmissionID.Value = QueryString["iAdmissionId"];				
		}
		else if (!QueryString["NewAcdYear"].IsNull())
			hidSerialNo.Value = QueryString["SerialNo"];
		else
		{
			if (!QueryString["ReceiptNo"].IsNullOrEmpty())
				hidReceiptNo.Value = QueryString["ReceiptNo"];
				
			if (!QueryString["StudentId"].IsNullOrEmpty())
				hidPostBackUrl.Value = QueryString["PostBackUrl"];
				
			if (!QueryString["StudentId"].IsNull() && !QueryString["PostBackUrl"].IsNull())
				hidStudentId.Value = QueryString["StudentId"];

            if(!QueryString["IsRefundFee"].IsNull())
                hidIsRefundFee.Value = QueryString["IsRefundFee"];
		}

		if (!QueryString["AcademicYear"].IsNullOrEmpty())
			hidAcaYear.Value = QueryString["AcademicYear"];

        if (!QueryString["AccountHeaderId"].IsNullOrEmpty())
            hidHeaderId.Value = QueryString["AccountHeaderId"];

	}

	#endregion -- PRIVATE METHOD(s) --
}
