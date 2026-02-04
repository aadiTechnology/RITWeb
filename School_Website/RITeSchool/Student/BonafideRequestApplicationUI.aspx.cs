using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using CrystalDecisions.Shared;

public partial class BonafideRequestApplicationUI : SchoolBase
{    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.BonafideRequestApplication, GetBonafideRequestApplicationFilterString(), ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GetBonafideRequestApplicationFilterString()
    {
        return "(usp_GetDetailsForBonafideRequestApplication.School_Id}=" + miSchoolId + "AND usp_GetDetailsForBonafideRequestApplication.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetDetailsForBonafideRequestApplication.Student_Id}="+ Session[Constants.S_SESSION_STUDENT_ID].ToString() + ") @";
    }    
}