using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;

public partial class TeacherSubjectListUI : SchoolBase
{

    const int I_COLUMN_INDEX_TEACHER_NAME = 0;

    #region Events
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                DisplayClassTeacher();
                SetControlsDefaultValues();
                FillTeacherSubjectListGrid();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region GridView Events

    /// <summary>
    /// This event is used to bind row command even.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwTeacherSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int iRowIndex = e.CommandArgument.ToInt();
            int iTeacherUserId = grdvwTeacherSubjects.DataKeys[iRowIndex]["TeacherUserId"].ToInt();
            string sTeacherName = grdvwTeacherSubjects.DataKeys[iRowIndex]["TeacherName"].ToString();
            string sQueryString = PrepareQueryString(iTeacherUserId, sTeacherName);
            var oMasterPage = this.Master as MasterPage;
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void grdvwTeacherSubjects_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }

    protected void grdvwTeacherSubjects_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillTeacherSubjectListGrid();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    private void DisplayClassTeacher()
    {     
        int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        int iDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);

        SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
        DataTable oDtClassTeacher = oSchoolWiseTeacherMasterBL.GetAssignedClassTeacher(miSchoolId, iStandardId, iDivisionId);
        if (oDtClassTeacher.Rows.Count > 0)
        {
            StringBuilder sTeacherNm = new StringBuilder();
            foreach (DataRow oRow in oDtClassTeacher.Rows)
                sTeacherNm.AppendFormat("{0}, ", oRow["TeacherName"]);

            lbClassTeacherName.Text = sTeacherNm.ToString().Substring(0, sTeacherNm.ToString().LastIndexOf(", "));
        }
        else
            trClassTeacher.Visible = false;                 
    }

    private void FillTeacherSubjectListGrid()
    {
        int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);        

        DataTable oDtTeacherSubject;

        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();

        oDtTeacherSubject =
            oTeacherSubjectAssignmentBL.GetListOfTeacherSubjectsforStudent(iUserId, miSchoolId, miAcademicYearId);

        DataView oDataView = oDtTeacherSubject.DefaultView;
        hidSortExpression.Value = hidSortExpression.Value.Replace(",", " " + hidSortDirection.Value + ",");
        if (hidSortExpression.Value != "")
            oDataView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
        grdvwTeacherSubjects.DataSource = oDataView;
        grdvwTeacherSubjects.DataBind();        
    }

    /// <summary>
    /// This method is used to set controls value at the time of page load.
    /// </summary>

    private void SetControlsDefaultValues()
    {
        hidSortExpression.Value = grdvwTeacherSubjects.Columns[I_COLUMN_INDEX_TEACHER_NAME].SortExpression ;
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }


    /// <summary>
    /// This method is used to prepare Query Strings.
    /// </summary>
    private string PrepareQueryString(int aiTeacherUserId,string saTeacherName)
    {
        const string S_PAGE = "Subject_Teacher_Screen";
        string sQuerystring = string.Format("From={0}&TeacherUserId={1}&Teacher_Name={2}",
                                           S_PAGE,
                                           aiTeacherUserId,
                                           saTeacherName);
        string sQueryString = CommonUtility.EncryptQuerystring(sQuerystring);
        return sQueryString;
    }

    #endregion
}
