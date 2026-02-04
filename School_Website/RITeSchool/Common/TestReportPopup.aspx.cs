using System;
using System.Reflection;
using System.Threading;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;

public partial class TestReportPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string sFilterString = "(usp_StudentwiseTestReportSPS.School_Id}=" + miSchoolId + "AND usp_StudentwiseTestReportSPS.Academic_Year_Id}=" + miAcademicYearId + "AND usp_StudentwiseTestReportSPS.StudentId}=" + QueryString["StudentId"] + "AND usp_StudentwiseTestReportSPS.Standard_Id}=" + QueryString["StandardId"] + " AND usp_StudentwiseTestReportSPS.Division_Id}=" + QueryString["StdDivId"] + "AND usp_StudentwiseTestReportSPS.TestId}=" + QueryString["TestId"] + " AND usp_StudentwiseTestReportSPS.IsAccessedFromScreen}=1" + ") @";
                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.TestwiseReport, sFilterString, ExportFormatType.PortableDocFormat);
                oReportDisplay.DisplayReport();
            }
        }
        catch (ThreadAbortException)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ClosePopup();", true);
           //Response.Write("<script type='text/javascript' language='javascript'>álert(''Test2')</script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}