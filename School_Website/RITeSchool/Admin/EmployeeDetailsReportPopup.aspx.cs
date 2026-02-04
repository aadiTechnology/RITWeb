using System;
using System.Reflection;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;


public partial class EmployeeDetailsReportPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReportDisplay oReportDisplay = null;
                if (SchoolBase.Settings.IsAaryanSchool)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.EmployeeDetailsReport, GetFilterString(), ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        string sFilterStr = string.Empty;
        sFilterStr = "(usp_EmployeeDetails.School_Id}=" + miSchoolId + "AND usp_EmployeeDetails.Academic_Year_Id}=" + miAcademicYearId + "AND usp_EmployeeDetails.UserRoleId}=" + QueryString["UserRoleId"] +
               "AND usp_EmployeeDetails.UserId}=" +QueryString["UserId"] + "AND usp_EmployeeDetails.IncludeDeactivatUser}=1" + ") @";
        return sFilterStr;
    }
    

}