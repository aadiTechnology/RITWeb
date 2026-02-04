using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;

public partial class AdmissionFormReport : SchoolBase
{
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (QueryString.Count > 0)
			{
                if (QueryString["EnquiryId"] == null)
                {
                    if (QueryString["IsCurrentYearAdmission"] != null)
                    {
                        S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_YES;

                        if (QueryString["IsCurrentYearAdmission"] == "False")
                            S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_NO;
                    }
                    else if (miSchoolId == Constants.SchoolId.ZLSP.ToInt() && QueryString["ReceiptNo"] != null)
                    {
                        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentFeeReceiptForZLSP, GetFeeFilterString(), ExportFormatType.PortableDocFormat);
                        oReportDisplay.DisplayReport();
                    }
                    else
                        S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;

                    if (QueryString["iAdmissionId"] != null)
                    {
                        hidStudentAdmissionId.Value = QueryString["iAdmissionId"];

                        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPSH.ToInt())
                            DisplayReport();
                        else
                        {
                            tblButtons.Visible = true;
                        }
                    }

                    if (QueryString["StudentAdmissionId"] != null && QueryString["IsConfirmationForm"] == null)
                    {
                        hidStudentAdmissionId.Value = QueryString["StudentAdmissionId"];
                        DisplayAdmissionReport();
                    }
                    else if (QueryString["StudentAdmissionId"] != null && QueryString["IsConfirmationForm"] != null)
                    {
                        hidStudentAdmissionId.Value = QueryString["StudentAdmissionId"];
                        DisplayConfirmationReport();
                    }
                    else if ((QueryString["iEnquiryId"] != null && QueryString["iEnquiryId"].ToString() != string.Empty) || (QueryString["AdmissionId"] != null && QueryString["AdmissionId"] != string.Empty))
                    {
                        hidEnquiryId.Value = QueryString["iEnquiryId"];
                        hidStudentAdmissionId.Value = QueryString["AdmissionId"];
                        DisplayEnquiryFormReport();   
                    }
                }
                else
                {
                    if (QueryString["IsFromEnquiryList"] != null && QueryString["IsFromEnquiryList"] == "1")
                    {
                        DisplayRegistrationReceipt(QueryString["EnquiryId"].ToInt());
                    }
                    else
                    {
                        hidEnquiryId.Value = QueryString["EnquiryId"];
                        DisplayRegistrationForm();
                    }
                  }

                if (QueryString["ItemId"] != null && QueryString["ItemId"].ToString() != string.Empty)
                {                    
                    ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.MaterialwiseStockDetails, GetFeeFilterString(QueryString["ItemCategoryId"].ToInt(), QueryString["ItemId"].ToInt()), ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
                }
                if (QueryString["Id"] != null && QueryString["Id"].ToString() != string.Empty)
                {
                    ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.GSTInvoiceDetails, GetGSTFilterString(), ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
                }
                if (QueryString["POMasterId"] != null && QueryString["POMasterId"].ToString() != string.Empty)
                {
                    ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.PODetails, GetPOFilterString(), ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
                }
                if (QueryString["CancFormId"] != null && QueryString["CancFormId"].ToString() != string.Empty && QueryString["CancFormId"].ToString() != Constants.S_ZERO)
                {
                    ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.CancellationFormDetails, GetCancellationFormFilterString(QueryString["Standard_Id"].ToInt(), QueryString["Division_Id"].ToInt(), QueryString["Student_Id"].ToInt(), QueryString["SubmittedBy"].ToInt()), ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
                }
			}
		}
		catch (ThreadAbortException)
		{ }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    protected void btnAdminCopy_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, GetFilterStringForPPSH(0), ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnTeachersCopy_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, GetFilterStringForPPSH(1), ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GetGSTFilterString()
    {
        return "(usp_GetGSTInvoiceReport.School_Id}=" + miSchoolId + "AND usp_GetGSTInvoiceReport.Academic_Year_Id}=" + miAcademicYearId + " AND usp_GetGSTInvoiceReport.Invoice_Id}=" + QueryString["Id"] + ") @";
    }
    private string GetPOFilterString()
    {
        return "(usp_GetPODetailsForReport.SchoolId}=" + miSchoolId + "AND usp_GetPODetailsForReport.FinancialYearId}=" + miFinancialYearId + "AND usp_GetPODetailsForReport.PoMasterId}=" + QueryString["POMasterId"] + ") @";
    }
    private string GetFeeFilterString(int aiCategoryId, int aiItemID)
    {
        return "(usp_GetMaterialwiseStockDetails.School_Id}=" + miSchoolId + "AND usp_GetMaterialwiseStockDetails.Academic_Year_Id}=" + miAcademicYearId + " AND usp_GetMaterialwiseStockDetails.CategoryId}=" + aiCategoryId + " AND usp_GetMaterialwiseStockDetails.ItemIds}=" + aiItemID + ") @";
    }

    private string GetCancellationFormFilterString(int aiStandardId, int aiDivisionId, int aiSTudentId, int aiSubmittedId)
    {
        return "(usp_GetCancellationFormDetails.School_Id}=" + miSchoolId + "AND usp_GetCancellationFormDetails.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetCancellationFormDetails.Standard_Id}=" + QueryString["Standard_Id"] + "AND usp_GetCancellationFormDetails.Division_Id}=" + QueryString["Division_Id"] + "AND usp_GetCancellationFormDetails.Student_Id}=" + QueryString["Student_Id"] + "AND usp_GetCancellationFormDetails.SubmittedBy}=" + QueryString["SubmittedBy"] + QueryString["CancFormId"] + ") @";
    }

	private void DisplayReport()
	{
		ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, GetFilterString(), ExportFormatType.PortableDocFormat);
		oReportDisplay.DisplayReport();
	}

    private void DisplayAdmissionReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentAdmissionFormSPS, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    private void DisplayConfirmationReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentAdmissionConfirmation, GetFilterStringForConfirmationReport(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    private void DisplayEnquiryFormReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.EnquiryFormReport, GetFilterStringForEnquiryForm(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    private string GetFilterStringForPPSH(int aiIsTeachersCopy)
    {
        return "(usp_GetAdmmissionFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + hidStudentAdmissionId.Value + " AND usp_GetAdmmissionFormReport.IsTeachersCopy}=" + aiIsTeachersCopy + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR + ") @";
    }

	private string GetFilterString()
	{
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
            return "(usp_GetAdmmissionFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + hidStudentAdmissionId.Value + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR + ") @";
        else
            return "(usp_GetStudentDetailsForAdmissionForm_SPS.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetStudentDetailsForAdmissionForm_SPS.StudentAdmissionId}=" + hidStudentAdmissionId.Value + " AND usp_GetStudentDetailsForAdmissionForm_SPS.AcademicYearId}=" + miAcademicYearId + ") @";
	}

	/// <summary>
    /// These method is used to display enquiry form.
    /// </summary>
    /// <returns></returns>
    private string GetFilterStringForEnquiryForm()
    {
        return "(usp_GetEnquiryFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetEnquiryFormReport.StudentEnquiryId}=" + hidEnquiryId.Value + " AND usp_GetEnquiryFormReport.AdmissionId}=" + hidStudentAdmissionId.Value + ") @";
     }

    private string GetFeeFilterString()
    {
        return "(usp_GetStudentReceiptDetail_ZLSP.School_Id}=" + miSchoolId + "AND usp_GetStudentReceiptDetail_ZLSP.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentReceiptDetail_ZLSP.ReceiptNo}=" + QueryString["ReceiptNo"] + ") @";
    }
    
    private string GetFilterStringForConfirmationReport()
    {
        return "(usp_StudentAdmissionConfirmationDetails.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_StudentAdmissionConfirmationDetails.StudentAdmissionId}=" + hidStudentAdmissionId.Value + " AND usp_StudentAdmissionConfirmationDetails.AcademicYearId}=" + miAcademicYearId + ") @";
    }

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    /// <param name="asCompanyId"></param>
    /// <param name="asReportPath"></param>
    private void DisplayRegistrationForm()
    {
        ReportDisplay oReportDisplay = null;
        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentRegistrationForm, GetFilterStr(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to display Registration Receipt.
    /// </summary>
    /// <param name="iEnquiryId"></param>
    private void DisplayRegistrationReceipt(int iEnquiryId)
    {
        string sFilterString = string.Empty;
        sFilterString = "(usp_GetEnquiryDetailsForRegistartionReceipt.SchoolId}=" + miSchoolId + " AND  usp_GetEnquiryDetailsForRegistartionReceipt.AcademicYearId =" + miAcademicYearId + " AND  usp_GetEnquiryDetailsForRegistartionReceipt.EnquiryId} = " + iEnquiryId + ") @";
        ReportDisplay oReportDisplay = null;
        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentRegistrationFeeReceipt, sFilterString, ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// this method is for geting filter
    /// </summary>
    /// <returns></returns>
    private string GetFilterStr()
    {
        string sRecordSelectionFormula = string.Empty;
        sRecordSelectionFormula = "(usp_GetEnquiryDetailsForRegistartionForm.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND  usp_GetEnquiryDetailsForRegistartionForm.AcademicYearId =" + Constants.I_ZERO + " AND  usp_GetEnquiryDetailsForRegistartionForm.EnquiryId} = " + hidEnquiryId.Value + ") @";

        return sRecordSelectionFormula;
    }
   
}
