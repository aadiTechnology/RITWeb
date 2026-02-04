using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using MasterEntities;
using Utility;

public partial class ExportStudentMonthlyActivityDetailsUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to fill up Month and Category combo boxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillMonthCombo();
                FillNoteCategories();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to export students monthly activity details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            StudentBL oStudent = new StudentBL(iStudentId);

            string sStudentMonthlyDetails = string.Empty;
            sStudentMonthlyDetails = "(Usp_StudentsMonthlyStatusReport.School_Id}=" + miSchoolId + " AND  Usp_StudentsMonthlyStatusReport.Academic_Year_Id} =" + miAcademicYearId +
                                      " AND  Usp_StudentsMonthlyStatusReport.Standard_Id} =" + oStudent.StandardId + " AND  Usp_StudentsMonthlyStatusReport.Division_Id} =" + oStudent.DivisionId +
                                       "AND Usp_StudentsMonthlyStatusReport.MonthId}=" + cmbMonth.SelectedValue + " AND Usp_StudentsMonthlyStatusReport.CategoryId}=" + cmbCategory.SelectedValue +
                                         " AND  Usp_StudentsMonthlyStatusReport.StudentId} =" + iStudentId + ")" + "@ ";
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ExportStudentMonthlyDetails, sStudentMonthlyDetails, ExportFormatType.Excel);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill month combobox.
    /// </summary>
    private void FillMonthCombo()
    {
        List<MonthMaster> oLstMonths = SchoolWiseAcademicYearMasterBL.GetAllMonth();
        ListSource.FillDropDownList(oLstMonths, cmbMonth, "Month", "MonthID", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill note categories.
    /// </summary>
    private void FillNoteCategories()
    {
        StudentAchievementBL moStudentAchievementBL = new StudentAchievementBL();
        DataTable dtNoteCategory = moStudentAchievementBL.GetNoteCategories();
        cmbCategory.Bind(dtNoteCategory, "Id", "NoteCategory", Constants.S_SELECT);
    }


    #endregion
}