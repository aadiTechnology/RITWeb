// File Name    : SchoolwiseAttendanceDetails.aspx.cs
// Created By   : Ketan
// Crested Date : 6/12/2007  

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Collections;
using System.Configuration;
using SchoolEntities.Teacher;
using System.Linq;

/// <summary>
/// This Class is used to provides user interface to configure,save and update the student attendance.
/// </summary>
public partial class SchoolwiseAttendanceDetails : SchoolBase
{
    #region Constant

    const Int32 I_SCHOOL_ID_COLUMN_NUMBER = 0; //datakey
    const Int32 I_CHECKBOX_COLUMN_NUMBER = 2;
    const Int32 I_INDEX_GRID_ROLL_NO_COL = 0;
    const Int32 I_TABLE_PRESENT = 2;
    const Int32 I_TABLE_ABSENT = 3;
    const Int32 I_TABLE_TOTALPRESENT = 4;
    const int I_TABLE_AVERAGE = 6;
    const Int32 I_INDEX_HALF_VIEW_COL = 3;
    const string S_CHECKBOX_PRESENTORABSENT = "chkPresentOrAbsent";
    const string S_CHECKBOX_HALFDAYPRESENT= "ChkBoxHalfDayPresent";
    const string S_ERROR_MSG_FUTURE_DATE = "Future date attendance is not allowed.";
    const string S_ERROR_MSG_INVALID_DT = "Please select valid date.";
    const string S_ERROR_MSG_BLANK_DT = "Date should not be blank.";
    const string S_ERROR_MSG_NO_DATA = " There are no students in this class.";
    const string S_MESSAGE_NO_ATTENDANCE = "Attendance not yet marked.";
    const string S_MESSAGE_ACADEMIC_YEAR = " Attendance date should be within current academic year&nbsp;&nbsp;<br>(i.e. between ";
    private const string S_ERRROR_MSG_ACESS_DENIED = "Access denied.";
    private const string S_SAVE_MESSAGE = " Attendance saved successfully !!!";   

    #endregion

    DateTime odtStartDate;
    DateTime odtEndDate;

    #region Events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStdDivId.Value = cmbTeachers.SelectedValue;
            string sErrMessage = CheckIfDateValid();
            if (sErrMessage.Equals(string.Empty))
            {
                setDefaultValues();
                DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
                GetAssignAttendenceToCalendar(oDSEvent);
                if (calTodaysDate.Text == "")
                    calTodaysDate.Text = DateTime.Now.ToShortDateString();
                AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
                AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);

            }
            else
            {
                DataSet oDSEvent = FillAttendenceDataSet();
                if (oDSEvent.Tables.Count > 0 && oDSEvent.Tables[0].Rows.Count > 0)
                    GetAssignAttendenceToCalendar(oDSEvent);
                lblErrorMsg.Text = sErrMessage;
                VisibleInvisibleFormFields(false);
            }
            AttendanceCalendar_SelectionChanged(sender, e);

            SetAbsentStudentVisibility();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill student attendance and set todays date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }

                if (CheckPreCondition())
                {
                    SetStandardDivisionAndDate();
                    SetSortingFieldDefaultValues();
                    SetAttendanceCalendarColumnHeaders();
                    if (!CheckUserRolesAndSetDisplay())
                    {
                        lblErrorMsg.Text = Resources.LocalizedResources.AccessDenied;
                        lblErrorMsg.CssClass = "ClsHilightErrorB";
                        lblErrorMsg.Visible = true;
                        pnlFields.Visible = false;
                    }
                }
               
                RefreshValue();
                SetJavascriptAttributes();
                setDefaultValues();
                SetDefaultDeleteStatus();
                SetAbsentStudentVisibility();
            }            

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                SetStandardDivisionAndDate();
                SetAttendancelinkAttributes();
                AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
                AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
            }
           
            calTodaysDate.ReadOnly = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to configure(save and update) student attendance.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sAttendanceXML = GenerateStudentXML();
            AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
            oSchoolWiseAttendanceDetailsBL.AttendanceDate = Convert.ToDateTime(calTodaysDate.Text);
            oSchoolWiseAttendanceDetailsBL.StandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
            oSchoolWiseAttendanceDetailsBL.InsertedByid = miUserId;
            oSchoolWiseAttendanceDetailsBL.MarkStudentAttendence(sAttendanceXML,chkboxSave.Checked);
            AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
            AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
            DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
            GetAssignAttendenceToCalendar(oDSEvent);
            lblUpdateSuccess.Text = Resources.LocalizedResources.AttendanceSavedSuccessfully;
            tdErrMsg.Visible = false;
            if (SchoolBase.Settings.StudentAbsentCount > Constants.I_ZERO)
            {
                SetAbsentStudentVisibility();
                List<int> aolstHalfDayPresentStudentId;
                List<int> oLstStudentId = oSchoolWiseAttendanceDetailsBL.GetAbsentStudentIds(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), cAttendDate.DateValue.ToString("yyy/MM/dd").ToDateTime(), SchoolBase.Settings.StudentAbsentCount, out aolstHalfDayPresentStudentId);
                if (oLstStudentId.Count > 0 || aolstHalfDayPresentStudentId.Count > 0)
                {
                    String QueryString = "StudentIdList=" + string.Join(",", oLstStudentId) + "&StandardDivisionId=" + cmbTeachers.SelectedValue + "&SelectedDate=" + calTodaysDate.Text + "&HalfDayPresentStudentIdList=" + string.Join(",", aolstHalfDayPresentStudentId);
                    QueryString = "../Teacher/MissingAttendancePopup.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPopupWindow", "OpenPopupWindow('" + QueryString + "')", true);
                }
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to change date and check for non working day.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAttendance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Boolean bBoolean = Convert.ToBoolean(grdStudentAttendanceManagement.DataKeys[e.Row.RowIndex]["isApplicable"]);
                    if (grdStudentAttendanceManagement.DataKeys[e.Row.RowIndex]["Joining_Date"] != DBNull.Value)
                    {
                        String sLbl = Convert.ToDateTime(grdStudentAttendanceManagement.DataKeys[e.Row.RowIndex]["Joining_Date"]).ToString("MMM dd");
                        if (!bBoolean)
                        {
                            Label oLabel = new Label();
                            oLabel.Text = Resources.LocalizedResources.LateJoining + "-<br/>" + sLbl;
                            e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].Controls.Add(oLabel);
                            Control oControl = e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].FindControl(S_CHECKBOX_PRESENTORABSENT);
                            e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].Controls.Remove(oControl);
                        }
                    }
                }
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAttendance_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillAttendanceOfStudentIntoGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to do paging to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudentAttendanceManagement_PageIndexChange(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStudentAttendanceManagement.PageIndex = e.NewPageIndex;
            FillAttendanceOfStudentIntoGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This Method is used to get attendence of selected date
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void calTodaysDate_DateChanged(object sender, EventArgs e)
    {
        try
        {
            if (calTodaysDate.Text != "")
            {
                if (cAttendDate.DateValue.Year != 1)
                {
                    SetSortingFieldDefaultValues();
                    AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
                    AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
                    DateTime dtTodaysDate = Convert.ToDateTime(calTodaysDate.Text);
                    //check if date is valid
                    string sErrMessage = CheckIfDateValid();
                    if (!sErrMessage.Equals(string.Empty))
                    {
                        lblErrorMsg.CssClass = "ClsHilightErrorB";
                        lblErrorMsg.Text = sErrMessage;
                        VisibleInvisibleFormFields(false);
                        DataSet oDSAttendanceManagement = FillAttendenceDataSet();
                        GetAssignAttendenceToCalendar(oDSAttendanceManagement);
                        lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
                        lnkAttendanceStatus.Enabled = false;
                        lnkTotalStudents.Attributes.Add("onclick", "return false;");
                        lnkTotalStudents.Enabled = false;
                        SetPresentyGrid(oDSAttendanceManagement);

                    }
                    else
                    {
                        lblErrorMsg.Text = string.Empty;
                        VisibleInvisibleFormFields(true);
                        DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
                        GetAssignAttendenceToCalendar(oDSEvent);
                        SetAttendancelinkAttributes();
                    }

                }
                else
                {
                    lblErrorMsg.Text = Resources.LocalizedResources.DateErrorMsg;
                    VisibleInvisibleFormFields(false);
                    lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
                    lnkAttendanceStatus.Enabled = false;
                    lnkTotalStudents.Attributes.Add("onclick", "return false;");
                    lnkTotalStudents.Enabled = false;
                }
            }
            else
            {
                lblErrorMsg.Text = Resources.LocalizedResources.DateShouldNotBeBlank;
                VisibleInvisibleFormFields(false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method event is used to get attendance details of the month changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AttendanceCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            if (e.NewDate.Month == DateTime.Now.Month)
            {
                calTodaysDate.Text = DateTime.Today.ToShortDateString();
                cAttendDate.DateValue = Convert.ToDateTime(DateTime.Today);
                AttendanceCalendar.SelectedDate = DateTime.Today;
            }
            else
            {
                calTodaysDate.Text = e.NewDate.ToShortDateString();
                cAttendDate.DateValue = Convert.ToDateTime(e.NewDate);
                AttendanceCalendar.SelectedDate = e.NewDate;
            }

            string sErrMessage = CheckIfDateValid();

            if (!sErrMessage.Equals(string.Empty))
            {
                lblErrorMsg.CssClass = "ClsHilightErrorB";
                lblErrorMsg.Text = sErrMessage;
                VisibleInvisibleFormFields(false);
                btnSave.Visible = false;
                btnSaveUp.Visible = false;
                btnDelete.Visible = false;
                DataSet oDSAttendanceManagement = FillAttendenceDataSet(cAttendDate.DateValue);
                GetAssignAttendenceToCalendar(oDSAttendanceManagement);
                SetPresentyGrid(oDSAttendanceManagement);
                lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
                lnkAttendanceStatus.Enabled = false;
                lnkTotalStudents.Attributes.Add("onclick", "return false;");
                lnkTotalStudents.Enabled = false;
                btnMonthwise.Visible = false;
                btnMarkMonthwiseAttendance.Visible = false;
                trMontwiseAttendanceNote.Visible = false;
            }
            else
            {
                lblErrorMsg.Text = string.Empty;
                VisibleInvisibleFormFields(true);
                btnSave.Visible = true;
                btnSaveUp.Visible = true;
                btnDelete.Visible = true;
                DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
                GetAssignAttendenceToCalendar(oDSEvent);
                SetAttendancelinkAttributes();
                if (Settings.MarkMonthwiseAttendance)
                {
                    btnMonthwise.Visible = true;
                    btnMarkMonthwiseAttendance.Visible = true;
                    trMontwiseAttendanceNote.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method event is used to show a attendence of a class for a selected date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AttendanceCalendar_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            SetSortingFieldDefaultValues();
            SelectedDatesCollection theDates = AttendanceCalendar.SelectedDates;
            DateTime oDateTime = theDates[0];
            calTodaysDate.Text = oDateTime.ToShortDateString();
            cAttendDate.DateValue = oDateTime;
            string sErrMessage = CheckIfDateValid();
            if (sErrMessage.Equals(string.Empty))
            {
                tdErrMsg.Visible = false;
                AttendanceCalendar.VisibleDate = AttendanceCalendar.SelectedDate;
                DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
                GetAssignAttendenceToCalendar(oDSEvent);
                SetAttendancelinkAttributes();
            }
            else
            {
                lblErrorMsg.CssClass = "ClsHilightErrorB";
                lblErrorMsg.Text = sErrMessage;
                VisibleInvisibleFormFields(false);
                btnSave.Visible = false;
                btnSaveUp.Visible = false;
                btnDelete.Visible = false;
                DataSet oDSAttendanceManagement = FillAttendenceDataSet();
                SetPresentyGrid(oDSAttendanceManagement);
                GetAssignAttendenceToCalendar(oDSAttendanceManagement);
                lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
                lnkAttendanceStatus.Enabled = false;
                lnkTotalStudents.Attributes.Add("onclick", "return false;");
                lnkTotalStudents.Enabled = false;
            }
            SetAbsentStudentVisibility();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This buton Delete event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int iStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
            string sAttendanceDate = cAttendDate.DateValue.ToString("yyyy-MM-dd");
            AttendanceDetailsBL.DeleteSchoolWiseAttendanceDetails(miSchoolId, miAcademicYearId, sAttendanceDate, iStdDivId);
            DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
            GetAssignAttendenceToCalendar(oDSEvent);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to Yes button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            MarkMonthwiseAttendance();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to No button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            MarkMonthwiseAttendance();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to Mark Attendance for current Month.
    /// </summary>
    private void MarkMonthwiseAttendance()
    {
        AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
        string sAttendanceDate = cAttendDate.DateValue.ToString("yyyy-MM-dd");
        bool bIsOverrite = hidSendNotification.Value.ToBool();
        oAttendanceDetailsBL.MarkClassMothwiseAttendance(miSchoolId, miAcademicYearId, cmbTeachers.SelectedValue.ToInt(), sAttendanceDate.ToDateTime(), miUserId, bIsOverrite);
        DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
        GetAssignAttendenceToCalendar(oDSEvent);
        lblUpdateSuccess.Text = Resources.LocalizedResources.AttendanceSavedSuccessfully;
    }

    /// <summary>
    /// This function is used to set sort variables
    /// </summary>
    private void SetSortVariables()
    {

        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set hidden field to default value for grid sorting.
    /// </summary>
    private void SetSortingFieldDefaultValues()
    {
        hidSortExpression.Value = hidSortExpression.Value = grdStudentAttendanceManagement.Columns[I_INDEX_GRID_ROLL_NO_COL].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }



    /// <summary>
    /// Generate XML for the students.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolWiseAttendance");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Attendance", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdStudentAttendanceManagement.Rows.Count; iRowCount++)
        {

            // This check is added to check is the attendance for student is applicable or not.
            // if the attendance is not applicable then their check boxes are disabled while binding.
            CheckBox chkSelectOrDeselectAll = ((CheckBox)grdStudentAttendanceManagement.Rows[iRowCount].FindControl(S_CHECKBOX_PRESENTORABSENT));
            if (chkSelectOrDeselectAll != null && chkSelectOrDeselectAll.Enabled)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseAttendance", "");
                GridViewRow oRow = grdStudentAttendanceManagement.Rows[iRowCount];

                string sAtrrName = "school_id"; // oRow.Cells[iColCount]
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = miSchoolId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "attendance_date";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = cAttendDate.DateValue.ToString("yyy/MM/dd");
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Student_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = grdStudentAttendanceManagement.DataKeys[iRowCount][I_SCHOOL_ID_COLUMN_NUMBER].ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "is_present";
                attr = oDoc.CreateAttribute(sAtrrName);
                Control oControl = grdStudentAttendanceManagement.Rows[iRowCount].FindControl(S_CHECKBOX_PRESENTORABSENT);
                if (oControl != null)
                {
                    CheckBox chkSelect = (CheckBox)oControl;
                    if (chkSelect.Checked == true)
                        attr.Value = Convert.ToString(Constants.C_YES);
                    else
                        attr.Value = Convert.ToString(Constants.C_NO);
                }
                oXmlNode.Attributes.Append(attr);
                
                sAtrrName = "is_halfdaypresent";
                attr = oDoc.CreateAttribute(sAtrrName);
                Control oAttendanceControl = grdStudentAttendanceManagement.Rows[iRowCount].FindControl(S_CHECKBOX_HALFDAYPRESENT);
                if (oAttendanceControl != null)
                {
                    CheckBox chkSelect = (CheckBox)oAttendanceControl;
                    if (chkSelect.Checked == true)
                        attr.Value = Constants.S_YES;
                    else
                        attr.Value = Constants.S_NO;
                }
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Standard_Division_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = cmbTeachers.SelectedValue;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Academic_Year_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = miAcademicYearId.ToString();
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }

        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }


    private DataSet FillAttendenceDataSet(DateTime dateTime)
    {
        DateTime dtTodaysDate = dateTime;
        int iStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
        AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
        DataSet oDSAttendanceManagement = oSchoolWiseAttendanceDetailsBL.FetchAttendenceDetails(miSchoolId, miAcademicYearId, iStandardDivisionId, dtTodaysDate, miUserId);
        setIdentitylinkURL();
        setIndiviAttendancelinkURL();
        SetStudentCount(oDSAttendanceManagement.Tables[5]);
        return oDSAttendanceManagement;
    }

    private void SetStandardDivisionAndDate()
    {
        cAttendDate.DateValue = Convert.ToDateTime(DateTime.Today);
        FillTeachersComboBox();

        if (QueryString.Count <= 0)
            return;

        if (QueryString["iStandardDivisionId"] != null)
        {
            hidStdDivId.Value = QueryString["iStandardDivisionId"];
            cmbTeachers.SelectedValue = hidStdDivId.Value;
        }
        else
            hidStdDivId.Value = "0";
        if (QueryString["dtDate"] != null)
        {
            calTodaysDate.Text = QueryString["dtDate"].ToDateTime().ToShortDateString();
            cAttendDate.DateValue = QueryString["dtDate"].ToDateTime();
            AttendanceCalendar.VisibleDate = cAttendDate.DateValue;
            AttendanceCalendar.SelectedDate = cAttendDate.DateValue;
            lblErrorMsg.Text = string.Empty;
            VisibleInvisibleFormFields(true);
        }
        string sErrMessage = CheckIfDateValid();
        if (sErrMessage == string.Empty)
        {
            DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
            GetAssignAttendenceToCalendar(oDSEvent);
        }
    }

    /// <summary>
    /// This method is used to fill student attendance.
    /// </summary>
    private DataSet FillAttendanceOfStudentIntoGrid()
    {
        DataSet oDSAttendanceManagement = FillAttendenceDataSet();
        if (oDSAttendanceManagement != null && oDSAttendanceManagement.Tables.Count > 0 &&
            oDSAttendanceManagement.Tables[0].Rows.Count > 0)
        {
            lblErrorMsg.Text = string.Empty;
            VisibleInvisibleFormFields(true);
            SetDataToAttendanceGrid(oDSAttendanceManagement);
            hidGridItemCount.Value = grdStudentAttendanceManagement.Rows.Count.ToString();
            SetPresentyGrid(oDSAttendanceManagement);
        }
        else
        {
            if (moUserRole == Constants.UserRoles.Teacher && !CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Attendance))
                VisibleAndHideControls();
            else
            {
                lblErrorMsg.Text = Resources.LocalizedResources.ThereAreNoStudentsInThisClass;
                lblErrorMsg.CssClass = "ClsHilightErrorB";
                VisibleInvisibleFormFields(false);
            }

        }

        return oDSAttendanceManagement;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oDSAttendanceManagement"></param>
    private void SetPresentyGrid(DataSet oDSAttendanceManagement)
    {
        tblPresentGrids.Visible = true;
        FillPresentStudentsGrid(oDSAttendanceManagement.Tables[I_TABLE_PRESENT]);
        FillAbsentStudentsGrid(oDSAttendanceManagement.Tables[I_TABLE_ABSENT]);
        FillTotalStudentsGrid(oDSAttendanceManagement.Tables[I_TABLE_TOTALPRESENT]);
        FillAverageDetails(oDSAttendanceManagement.Tables[I_TABLE_AVERAGE]);
    }

    private void FillAverageDetails(DataTable aDtAverage)
    {
        lstvwAverageDetails.DataSource = aDtAverage;
        lstvwAverageDetails.DataBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataTable"></param>
    private void FillTotalStudentsGrid(DataTable dataTable)
    {
        grdTotalPresent.DataSource = dataTable;
        grdTotalPresent.DataBind();
    }

    /// <summary>
    /// This method is used to set grid for present student count.
    /// </summary>
    /// <param name="dataTable"></param>
    private void FillPresentStudentsGrid(DataTable dataTable)
    {
        grdPresent.DataSource = dataTable;
        grdPresent.DataBind();
    }

    /// <summary>
    /// This method is used to set grid for absent student count.
    /// </summary>
    /// <param name="dataTable"></param>
    private void FillAbsentStudentsGrid(DataTable dataTable)
    {
        grdAbsent.DataSource = dataTable;
        grdAbsent.DataBind();
    }

    /// <summary>
    /// This function s used to set attendance dataset .
    /// </summary>
    /// <returns></returns>
    private DataSet FillAttendenceDataSet()
    {
        DataSet oDSAttendanceManagement = new DataSet();
        DateTime dtTodaysDate = DateTime.Now;
        if (DateTime.TryParse(calTodaysDate.Text, out dtTodaysDate))
        {
            dtTodaysDate = Convert.ToDateTime((calTodaysDate.Text == "" ? Convert.ToString(DateTime.Now) : calTodaysDate.Text));
            AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
            oDSAttendanceManagement = oSchoolWiseAttendanceDetailsBL.FetchAttendenceDetails(miSchoolId, miAcademicYearId, cmbTeachers.SelectedValue.ToInt(), dtTodaysDate, miUserId);
            setIdentitylinkURL();
            setIndiviAttendancelinkURL();
            SetStudentCount(oDSAttendanceManagement.Tables[5]);

        }
        return oDSAttendanceManagement;
    }

    /// <summary>
    /// This method is used to set data into grid.
    /// </summary>
    /// <param name="aoDSAttendanceManagement"></param>
    private void SetDataToAttendanceGrid(DataSet aoDSAttendanceManagement)
    {
        if (aoDSAttendanceManagement.Tables[0].Rows.Count > 0)
        {
            DataView oDataView = aoDSAttendanceManagement.Tables[0].DefaultView;
            grdStudentAttendanceManagement.DataSource = oDataView;
            grdStudentAttendanceManagement.DataBind();
            if (grdStudentAttendanceManagement.Rows.Count > 0)
            {
                CheckBox chkSelectOrDeselectAll = ((CheckBox)grdStudentAttendanceManagement.Rows[0].FindControl(S_CHECKBOX_PRESENTORABSENT));
                if (chkSelectOrDeselectAll != null)
                    chkSelectOrDeselectAll.Focus();
            }


            for (int iRowCount = 0; iRowCount < grdStudentAttendanceManagement.Rows.Count; iRowCount++)
            {
                CheckBox chkPresentOrAbsent = (CheckBox)grdStudentAttendanceManagement.Rows[iRowCount].FindControl(S_CHECKBOX_PRESENTORABSENT);
				if(chkPresentOrAbsent!=null)
                if (!chkPresentOrAbsent.Checked)
                {
                    if (iRowCount % 2 == 1)
                        grdStudentAttendanceManagement.Rows[iRowCount].CssClass = "ClsGridAltRowHighLight";
                    else
                        grdStudentAttendanceManagement.Rows[iRowCount].CssClass = "ClsGridRowHighLight";
                }
            }
            
            // This condition is used to show/hide halfday column.
            if (!SchoolBase.Settings.IsEnableHalfDayView)
            {
                grdStudentAttendanceManagement.Columns[I_INDEX_HALF_VIEW_COL].Visible = false;
            }

            VisibleInvisibleFormFields(true);
            DataRow[] oDataRow = aoDSAttendanceManagement.Tables[0].Select("SchoolWise_Attendance_Id IS NOT NULL");
            if (oDataRow.Length == 0)
            {
                lblErrorMsg.Text = Resources.LocalizedResources.AttendanceNotYetMarked;
                lblErrorMsg.CssClass = "ClsHilightErrorB";
                lblErrorMsg.Visible = true;
                tdErrMsg.Visible = true;
                btnDelete.Visible = false;
            }
            else
            {
                if (moUserRole == Constants.UserRoles.Admin || hidCanEdit.Value == Constants.C_YES.ToString() || hidCanEdit.Value == string.Empty)
                {
                    btnDelete.Visible = true;
                    btnSave.Visible = true;
                    btnSaveUp.Visible = true;
                    btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete('" + cAttendDate.DateValue.ToString("dd-MMM-yyyy") + "')) return false;");
                }
                else
                {
                    btnSave.Visible = false;
                    btnSaveUp.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }
        else
        {
            if (moUserRole == Constants.UserRoles.Teacher)
                VisibleAndHideControls();
            else
            {
                lblErrorMsg.Text = Resources.LocalizedResources.ThereAreNoStudentsInThisClass;
                lblErrorMsg.CssClass = "ClsHilightErrorB";
                VisibleInvisibleFormFields(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void VisibleInvisibleFormFields(bool abAction)
    {
        lblErrorMsg.Visible = !(abAction);
        tdErrMsg.Visible = !(abAction);
        grdStudentAttendanceManagement.Visible = abAction;
        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnSave.Visible = abAction;
            btnSaveUp.Visible = abAction;
            btnDelete.Visible = abAction;
        }
        else
        {
            if (hidCanEdit.Value == "Y" || hidCanEdit.Value == "")
            {
                btnSave.Visible = abAction;
                btnSaveUp.Visible = abAction;
                btnDelete.Visible = abAction;
            }
        }
    }

    /// <summary>+
    /// 
    /// 
    /// </summary>
    private void setDefaultValues()
    {
        int iStdDivID = 0;
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor
            || (moUserRole == Constants.UserRoles.Teacher
                && Boolean.Parse(hidUserHasFullAccess.Value)))
        {
            if (cmbTeachers.SelectedIndex >= 0)
                iStdDivID = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (!IsPostBack)
            {
                if (calTodaysDate.Text == "")
                {
                    calTodaysDate.Text = DateTime.Now.ToShortDateString();
                    cAttendDate.DateValue = DateTime.Now;
                }
            }

            hidStdDivId.Value = iStdDivID.ToString();
        }
        else if ((moUserRole == Constants.UserRoles.Teacher) && (Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Convert.ToChar(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) == Constants.C_YES))
        {
            iStdDivID = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (!IsPostBack)
            {
                if (calTodaysDate.Text == "")
                {
                    calTodaysDate.Text = DateTime.Now.ToShortDateString();
                    cAttendDate.DateValue = DateTime.Now;
                }
            }
            hidStdDivId.Value = iStdDivID.ToString();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool CheckUserRolesAndSetDisplay()
    {
        SetControlsDefaultValues();
        bool bReturn = true;
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor
            || (moUserRole == Constants.UserRoles.Teacher
            && Boolean.Parse(hidUserHasFullAccess.Value)))
        {
            string sErrMessage = CheckIfDateValid();
            if (sErrMessage != "")
            {
                btnSave.Visible = false;
                btnSaveUp.Visible = false;
                btnDelete.Visible = false;
            }

            if (cmbTeachers.Items.Count > 0)
            {
                if (moUserRole == Constants.UserRoles.Supervisor
                    || (moUserRole == Constants.UserRoles.Teacher
                    && Boolean.Parse(hidUserHasFullAccess.Value)))
                {
                    hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Attendance).ToString();

                    if (moUserRole == Constants.UserRoles.Teacher && cmbTeachers.Items.Count == 1 && cmbTeachers.Enabled == false)
                        hidCanEdit.Value = Constants.S_YES;

                    if (hidCanEdit.Value == "N")
                    {
                        btnSave.Visible = false;
                        btnSaveUp.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
            }
            else
            {
                tdErrMsg.Visible = true;
                lblErrorMsg.Text = Resources.LocalizedResources.TeachersAreNotAvailable;
                trgrdStudent.Visible = false;
                trButton.Visible = false;
                trIndiviAttendance.Visible = false;

                btnSave.Visible = false;
                btnSaveUp.Visible = false;
                AttendanceCalendar.Enabled = false;
            }
            if (cmbTeachers.Items.Count > 0)
            {
                DataSet oDSEvent = FillAttendenceDataSet();
                GetAssignAttendenceToCalendar(oDSEvent);
            }

            AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
            AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            setDefaultValues();
            string sErrMessage = CheckIfDateValid();
            if (sErrMessage.Equals(string.Empty))
            {
                DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
                if (oDSEvent != null && oDSEvent.Tables.Count > 0)
                    GetAssignAttendenceToCalendar(oDSEvent);
                AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
                AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
            }
            else
            {
                lblErrorMsg.Text = sErrMessage;
                VisibleInvisibleFormFields(false);
                btnSave.Visible = false;
                btnSaveUp.Visible = false;
                DataSet oDSEvent = FillAttendenceDataSet();
                GetAssignAttendenceToCalendar(oDSEvent);
                AttendanceCalendar.VisibleDate = Convert.ToDateTime(calTodaysDate.Text);
                AttendanceCalendar.SelectedDate = Convert.ToDateTime(calTodaysDate.Text);
            }

        }
        else
        {
            bReturn = false;
        }
        setIdentitylinkURL();
        setIndiviAttendancelinkURL();

        return bReturn;
    }

    /// <summary>
    /// If the staus of calender date is "Done" then  sets button delete visible to true else False.
    /// </summary>
    private void SetDefaultDeleteStatus()
    {
        DateTime oDt;
        if (DateTime.TryParse(calTodaysDate.Text.ToString(), out oDt))
        {
            DataRow[] oDR = AttendanceCalendar.EventSource.Select("Att_date='" + calTodaysDate.Text.ToDateTime().ToString("MM/dd/yyyy") + "'");
            if (oDR.Length > 0)
            {
                string sStatus =
					AttendanceCalendar.EventSource.Select("Att_date='" + calTodaysDate.Text.ToDateTime().ToString("MM/dd/yyyy") + "'")[0][
                        "Status"].ToString();
                btnDelete.Visible = !(sStatus.Contains("Not Done") ||
                    (sStatus.Contains("Holiday") || (sStatus.Contains("Weekend"))));
                string sErrMessage = CheckIfDateValid();
                btnDelete.Visible = !(sStatus.Contains("Not Done") || sStatus.Contains("Outside Academic Year") ||
                  (sStatus.Contains("Holiday") || (sStatus.Contains("Weekend"))));
                if (!sErrMessage.Equals(string.Empty))
                {
                    DataSet oDSAttendanceManagement = FillAttendenceDataSet();
                    SetPresentyGrid(oDSAttendanceManagement);
                    btnDelete.Visible = false;
                }
            }
        }
    }

    private void SetStudentCount(DataTable oDataTable)
    {
        if (oDataTable.IsNonEmpty())
        {
            DataRow oDataRow = oDataTable.Rows[0];
            lnkTotalStudents.Text = oDataRow["PresentStudents"] + "/" + oDataRow["TotalStudents"];
            lnkAttendanceStatus.Text = oDataRow["PresentDivisions"] + "/" + oDataRow["TotalDivisions"];            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDataTable = oMasterDataCollectionBL.GetClassTeachers(miSchoolId, miAcademicYearId);

        if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Attendance) != Constants.C_YES)
        {
            if (moSchool == Constants.SchoolId.PPSN)
            {
                AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
                List<CoordinateDetails> lstCoordinatorDetails = oAttendanceDetailsBL.GetCoordinatorDetails(miSchoolId, miAcademicYearId);
                List<int> lstStandardIds = lstCoordinatorDetails.Where(ct => ct.UserId == miUserId).Select(ct => ct.StandardId).ToList();

                DataRow[] dtArray = null;
                if (lstStandardIds.Count > 0)
                {
                    dtArray = oDataTable.Select("(Standard_Id IN (" + string.Join(",", lstStandardIds) + ") OR Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID] + ")");
                    if (dtArray.Length > 0)
                    {
                        var sortedRows = dtArray.AsEnumerable()
                        .OrderBy(row => row.Field<int>("Original_Standard_Id"))
                        .ThenBy(row => row.Field<int>("Original_Division_Id"));

                        oDataTable = sortedRows.CopyToDataTable();
                    }
                }
                else
                {
                    dtArray = oDataTable.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
                    oDataTable = dtArray.CopyToDataTable();
                }

                ControlUtility.FillDropDownList(oDataTable, ref cmbTeachers,
                                                   Constants.S_STANDARD_DIVISION_ID_FIELD,
                                                   Constants.S_TEACHER_NAME_FIELD,
                                                   string.Empty);
                if (oDataTable.Rows.Count == Constants.I_ONE)
                    cmbTeachers.Enabled = false;
            }
            else
            {
                DataRow[] oDataRow = oDataTable.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
                ControlUtility.FillDropDownList(oDataRow, ref cmbTeachers,
                                                   Constants.S_STANDARD_DIVISION_ID_FIELD,
                                                   Constants.S_TEACHER_NAME_FIELD,
                                                   string.Empty);
                if (oDataRow.Length == Constants.I_ONE)
                    cmbTeachers.Enabled = false;
            }
        }

        else
        {
            ControlUtility.FillDropDownList(oDataTable, ref cmbTeachers,
                                           Constants.S_STANDARD_DIVISION_ID_FIELD,
                                           Constants.S_TEACHER_NAME_FIELD,
                                          string.Empty);
        }

     
            
        if ((oDataTable != null) && (oDataTable.Rows.Count > 0))
        {
            int iStdDivID = Convert.ToInt32(cmbTeachers.SelectedValue);
            hidStdDivId.Value = iStdDivID.ToString();
        }
        string sErrMessage = CheckIfDateValid();
        if (sErrMessage.Equals(string.Empty))
        {
            setDefaultValues();
            DataSet oDSEvent = FillAttendanceOfStudentIntoGrid();
            GetAssignAttendenceToCalendar(oDSEvent);
        }
        else
        {
            lblErrorMsg.Text = sErrMessage;
            VisibleInvisibleFormFields(false);
        }
    }
    /// <summary>
    /// This method is used to visible and hide controls
    /// when class teacher and weekdays are not configured.
    /// </summary>
    private void VisibleAndHideControls()
    {
        AttendanceCalendar.Visible = false;
        btnSave.Visible = false;
        btnSaveUp.Visible = false;
        pnlFields.Visible = false;
        tdlbl.Visible = false;
        tdErrMsg.Visible = false;
        lblDate.Visible = false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetControlsDefaultValues()
    {
        hidSortExpression.Value = grdStudentAttendanceManagement.Columns[0].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
        if (calTodaysDate.Text == "")
        {
            calTodaysDate.Text = DateTime.Now.ToString(Constants.S_STANDARD_DATE_FORMAT).Replace(" ", "-");
            AttendanceCalendar.SelectedDate = DateTime.Now;
        }

        btnSave.Attributes.Add("onclick", "return (IsAllStudentPresentOrAbsent(true))");
        btnSaveUp.Attributes.Add("onclick", "return (IsAllStudentPresentOrAbsent(false))");
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        SetAcademicYearDates();
        if (moUserRole == Constants.UserRoles.Teacher)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Attendance).ToString();
    }

    /// <summary>
    /// This method is used to set javascript atrributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnDelete, btnSaveUp });
        SetAttendancelinkAttributes();
        if (Settings.MarkMonthwiseAttendance)
        {
            btnMonthwise.Visible = true;
            btnMarkMonthwiseAttendance.Visible = true;
            btnMonthwise.Attributes.Add("onclick", "OpenConfirmationPopup()");
            btnMarkMonthwiseAttendance.Attributes.Add("onclick", "OpenConfirmationPopup()");
            btnCancelOp.Attributes.Add("onclick", "HideConfirmationPopup(); return false;");
            trMontwiseAttendanceNote.Visible = true;
        }
        else
        {
            btnMonthwise.Visible = false;
            btnMarkMonthwiseAttendance.Visible = false;
            trMontwiseAttendanceNote.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set academic year start and end date.
    /// </summary>
    private void SetAcademicYearDates()
    {
        hidYearStartDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).ToShortDateString();
        hidYearEndDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).ToShortDateString();
    }

    /// <summary>
    /// This method check if the selected date is a valid date,
    /// and if not it returns the specific error message.
    /// </summary>
    /// <returns>
    /// Error message: if date is invalid 
    /// Blank string : if valid
    /// </returns>
    private string CheckIfDateValid()
    {

        if (!isValidDateForAcademicYear())
        {
            if (odtStartDate != null && odtEndDate != null && odtStartDate != DateTime.MinValue && odtEndDate != DateTime.MinValue)
            {
                return Resources.LocalizedResources.AttendanceDateShouldBeWithInCurrentAcademicYear + " "
                        + odtStartDate.ToString("d MMM yyyy") + " " + Resources.LocalizedResources.To + " "
                        + odtEndDate.ToString("d MMM yyyy") + ").";
            }
            return Resources.LocalizedResources.AttendanceNotYetMarked;
        }

        DateTime dtTodaysDate = Convert.ToDateTime(calTodaysDate.Text == "" ? Convert.ToString(DateTime.Now) : calTodaysDate.Text);
        if (dtTodaysDate > DateTime.Today) // check if selected date is future date
        {
            lblErrorMsg.CssClass = "ClsHilightErrorB";
            return Resources.LocalizedResources.FutureDateAttendanceIsNotAllowed;


        }
        int iStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);

        AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
        hidIsNonWorking.Value = oSchoolWiseAttendanceDetailsBL.CheckWeekEnd(dtTodaysDate, miSchoolId, miAcademicYearId, iStandardDivisionId);
        hidIsNonWorking.Value = hidIsNonWorking.Value.Replace("Selected date is holiday.", Resources.LocalizedResources.MsgSelectedDateHoliday);
        hidIsNonWorking.Value = hidIsNonWorking.Value.Replace("Selected date is weekend.", Resources.LocalizedResources.MsgSelectedDateWeekend);
        return string.Empty;
    }

    /// <summary>
    /// This method check if the selected date is a valid date,
    /// and if not it returns the specific error message.
    /// </summary>
    /// <returns>
    /// Error message: if date is invalid 
    /// Blank string : if valid
    /// </returns>
    private Boolean isValidDateForAcademicYear()
    {
        DateTime oDt;
        odtStartDate = DateTime.MinValue;
        odtEndDate = DateTime.MinValue;
        if (DateTime.TryParse(calTodaysDate.Text.ToString(), out oDt))
            oDt = Convert.ToDateTime(calTodaysDate.Text == "" ? Convert.ToString(DateTime.Now) : calTodaysDate.Text);
        if (!string.IsNullOrEmpty(hidStdDivId.Value))
        {
            DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandardDivision(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStdDivId.Value));
            if (oDT != null && oDT.Rows.Count > 0 && oDT.Rows[0]["StartDate"] != DBNull.Value && oDT.Rows[0]["EndDate"] != DBNull.Value)
            {
                odtStartDate = Convert.ToDateTime(oDT.Rows[0]["StartDate"].ToString());
                odtEndDate = Convert.ToDateTime(oDT.Rows[0]["EndDate"].ToString());

                btnSave.Enabled = true;
                btnSaveUp.Enabled = true;
                btnDelete.Visible = true;
            }
            else
            {
                trAcademicYr.Visible = true;
                lblAcademicYrErrorMsg.Text = Resources.LocalizedResources.ConfigureAcademicYearStartAndEndDates;
                    //"Academic Year Start & End dates have not been configured for this Standard. Please configure it.";
                lblAcademicYrErrorMsg.Visible = true;

                btnSave.Enabled = false;
                btnSaveUp.Enabled = false;
                btnDelete.Visible = false;
            }
        }
        else
        {
            odtStartDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
            odtEndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
        }

        if (odtStartDate <= DateTime.MinValue)
            odtStartDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
        else if (odtEndDate <= DateTime.MinValue)
            odtEndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
        
            if (oDt < odtStartDate || oDt > odtEndDate)
            return false;
        else
            return true;
    }

    #region Calender method

    /// <summary>
    /// This function is used to set the events to the calendar control.
    /// </summary>    
    private void GetAssignAttendenceToCalendar(DataSet aoDSEvent)
    {
        
        AttendanceCalendar.EventSource = aoDSEvent.Tables[1];
    }

    /// <summary>
    /// This function is used to set the columns of the event calendar.
    /// </summary>
    private void SetAttendanceCalendarColumnHeaders()
    {
        AttendanceCalendar.EventStartDateColumnName = "Att_date";
        AttendanceCalendar.EventEndDateColumnName = "Att_date";
        AttendanceCalendar.EventDescriptionColumnName = "Status_Desc";
        AttendanceCalendar.EventHeaderColumnName = "Status";
        AttendanceCalendar.EventBackColorName = "Status_BackColur";
        AttendanceCalendar.EventForeColorName = "Status_ForeColur";
    }

    #endregion calender method


    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void setIndiviAttendancelinkURL()
    {
        hlnkIndiviAttendance.Disabled = false;
        String QueryString = "iStandardDivisionId=" + hidStdDivId.Value + "&dtDate=" + Convert.ToDateTime((calTodaysDate.Text == "" ? Convert.ToString(DateTime.Now) : calTodaysDate.Text));
        QueryString = "../Teacher/StudMonthlyAttendanceUIPopUp.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
        hlnkIndiviAttendance.Attributes.Add("onclick", "ShowIdentities('" + QueryString + "');return false;");
    }

    private void SetAttendancelinkAttributes()
    {
        string sQueryString1 = string.Empty;
        string sQueryString2 = string.Empty;
        sQueryString1 = CommonUtility.EncryptQuerystring(String.Format("SelectedDate={0}&ShowCount=Y", calTodaysDate.Text));
        sQueryString2 = CommonUtility.EncryptQuerystring(String.Format("SelectedDate={0}&ShowCount=N", calTodaysDate.Text));
        lnkAttendanceStatus.Enabled = true;
        lnkTotalStudents.Enabled = true;

        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.S_YES && !Boolean.Parse(hidUserHasFullAccess.Value))
        {
            lnkTotalStudents.Attributes.Add("onclick", "return false;");
            lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
        }
        else
        {
            lnkAttendanceStatus.Attributes.Add("onclick", "if(!OpenPopUp('" + sQueryString1 + "')) {return false;}");
            lnkTotalStudents.Attributes.Add("onclick", "if(!OpenPopUp('" + sQueryString2 + "')) {return false;}");
        }
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void setIdentitylinkURL()
    {
        hlnkIdentity.Disabled = false;
        String QueryString = "iStandardDivisionId=" + hidStdDivId.Value;
        QueryString = "../Teacher/StudentsMonthWiseAttendanceListUIPopUp.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
        hlnkIdentity.Attributes.Add("onclick", "ShowIdentities('" + QueryString + "');return false;");
    }

    /// <summary>
    /// This method is ued to check Precondition
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.Attendance);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.Visible = true;
            divErr.InnerHtml = sLinks;
            HideControls(false);
        }
        return bReturn;
    }

    /// <summary>
    /// thi smetho dis used to hid controls
    /// </summary>
    /// <param name="abStatus"></param>
    private void HideControls(bool abStatus)
    {
        trAttendenceDetails.Visible = abStatus;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidAllStudentsMarkedAsAbsent.Value = Resources.LocalizedResources.AllMarkedAsAbsentWantToSave;
        hidAllStudentsMarkedAsPresent.Value = Resources.LocalizedResources.AllMarkedAsPresentWantToSave;
        hidAreYouSureYouWantToSaveTheAttendance.Value = Resources.LocalizedResources.AreYouSureYouWantToSaveTheAttendance;
        hidAreYouSureYouWantToDeleteAttendanceOfDate.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteAttendanceOfDate;
        hidDateShouldNotBeBlank.Value = Resources.LocalizedResources.DateShouldNotBeBlank;
       
    }

    /// <summary>
    /// This method used to set the value for Absent students link.
    /// </summary>
    private void SetAbsentStudentVisibility()
    {
        if (SchoolBase.Settings.StudentAbsentCount > Constants.I_ZERO)
        {
            List<int> aolstHalfDayPresentStudentId;
            hlnkAbsentStudents.Visible = true;
            AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
            List<int> oLstStudentId = oSchoolWiseAttendanceDetailsBL.GetAbsentStudentIds(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), cAttendDate.DateValue.ToString("yyy/MM/dd").ToDateTime(), SchoolBase.Settings.StudentAbsentCount, out aolstHalfDayPresentStudentId);
            String QueryString = "StudentIdList=" + string.Join(",", oLstStudentId) + "&StandardDivisionId=" + cmbTeachers.SelectedValue + "&SelectedDate=" + calTodaysDate.Text + "&HalfDayPresentStudentIdList=" + string.Join(",", aolstHalfDayPresentStudentId);
            QueryString = "../Teacher/MissingAttendancePopup.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
            hlnkAbsentStudents.Attributes.Add("onclick", "OpenPopupWindow('" + QueryString + "');return false;");
        }
    }

    /// <summary>
    /// This method is used to set field values.
    /// </summary>
    private void SetFields()
    {
        setDefaultValues();
        SetDefaultDeleteStatus();
        ShowMessage();
        SetAbsentStudentVisibility();
    }

    /// <summary>
    /// This method is used to show messages.
    /// </summary>
    private void ShowMessage()
    {
        string sErrMessage = CheckIfDateValid();
        if (sErrMessage != string.Empty)
        {
            lblErrorMsg.Text = sErrMessage;
            VisibleInvisibleFormFields(false);
            lnkAttendanceStatus.Attributes.Add("onclick", "return false;");
            lnkAttendanceStatus.Enabled = false;
            lnkTotalStudents.Attributes.Add("onclick", "return false;");
            lnkTotalStudents.Enabled = false;
        }
    }
    #endregion
}
