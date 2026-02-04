/* File Name - PayrollReportUI.aspc.cs
 * Created By - Sachin
 * Created Date - 27 Jan 2014
 * Description - This class is used to display report according to report no.
 */

using System;
using System.Threading;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;

public partial class PayrollReportUI : SchoolBase
{
    ExportFormatType oExportFormatType = ExportFormatType.PortableDocFormat;

    #region event(s)

    /// <summary>
    /// This event is used to open reports.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (QueryString["FileType"] != null)
                    oExportFormatType = (ExportFormatType)(QueryString["FileType"].ToInt());

                if (QueryString["ReportNo"].ToInt() == Constants.ExportReports.AppointmentLetter.ToInt())
                    DisplayAppointmentLetter();
                else if (QueryString["ReportNo"].ToInt() == Constants.ExportReports.ServiceContract.ToInt())
                    DisplayServiceContract();
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    private void DisplayServiceContract()
    {
        int iAppointmentId = QueryString["AppointmentId"].ToInt();
        string sRecordSelectionFormula = "(usp_GetServiceContractDetails.School_Id}=" + miSchoolId + " AND usp_GetServiceContractDetails.AppointmentId}=" + iAppointmentId + ")" + "@ ";
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ServiceContract, sRecordSelectionFormula, oExportFormatType);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to open applointment letter report.
    /// </summary>
    /// <param name="aiAppointmentId"></param>
    private void DisplayAppointmentLetter()
    {
        int iAppointmentId = QueryString["AppointmentId"].ToInt();
        string sRecordSelectionFormula = "(usp_GetAppointmentDetailsForReport.School_Id}=" + miSchoolId + " AND usp_GetAppointmentDetailsForReport.Academic_Year_Id}=" + miAcademicYearId + " AND usp_GetAppointmentDetailsForReport.AppointmentId}=" + iAppointmentId + ")" + "@ ";
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AppointmentLetter, sRecordSelectionFormula, oExportFormatType);
        oReportDisplay.DisplayReport();
    } 

    #endregion
}