/* File Name - StudentWiseProgressSheet.aspx.cs
 * Created Date - 22-Oct-2011
 * Created by - Vipul
 * Class Description - This class is used for displaying student progress report.
 
 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;

public partial class StudentWiseProgressSheet : SchoolBase
{
    int miStdDivId = 0;
    int miStudentId = 0;
    int miClassTacherID = 0;
    int miTeacherId = 0;
    string msTestId = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializeMembers();
            GetQueryString();
            ShowStudProgressSheet();
            ApplyMouseHoverEffect(new List<Button> { btnCancel, btnCancelUp });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to show progress sheet forselected criteria.
    /// </summary>
    private void ShowStudProgressSheet()
    {
        try
        {
            ProgressSheetBase oStudentProgress = (ProgressSheetBase)new StudentProgress();
            if (miStudentId != 0)
            {
                oStudentProgress = ProgressSheet.GetProgressSheet(GridViewScrollContainer, miSchoolId, miAcademicYearId, miStudentId, Constants.UserRoles.Student);
                oStudentProgress.mbViewStudnetwiseProgressReport = true;
                oStudentProgress.msTestId = string.Empty;
                oStudentProgress.msTestId = msTestId;
                oStudentProgress.ShowProgressSheet(miStudentId);
            }
            else
            {
                oStudentProgress = ProgressSheet.GetProgressSheet(GridViewScrollContainer, miSchoolId, miAcademicYearId, miTeacherId, Constants.UserRoles.Teacher);
                int iResult = oStudentProgress.ShowProgressSheet(miTeacherId, miStudentId);
                if (iResult > 1)
                    btnCancelUp.Visible = true;
            }
        }
        catch (MarksNotAvailableForResult Ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            tblProgress.Visible = false;
            lblErrorMsg.Text = "No Record Found.";
        }
    }

    /// <summary>
    /// This method is used to set values of session variable to member variables.
    /// </summary>
    private void InitializeMembers()
    {
        InitializeMemberVariables();
        if (Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] != null)
            miStdDivId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
        if (Session[Constants.S_SESSION_TEACHER_ID] != null)
            miTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count >0)
        {
            if (QueryString["ClassTeacherId"] != null)
                miClassTacherID = QueryString["ClassTeacherId"].ToInt();
            if (QueryString["StudentId"] != null)
                miStudentId = QueryString["StudentId"].ToInt();
            msTestId = string.Empty;
            if (QueryString["TestId"] != null)
                msTestId = QueryString["TestId"];
            btnCancel.PostBackUrl = btnCancel.PostBackUrl + "?" + Request.QueryString;
        }
    }
}
