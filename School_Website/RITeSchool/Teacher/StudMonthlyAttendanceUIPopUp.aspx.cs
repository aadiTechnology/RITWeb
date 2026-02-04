using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// "This class will display list of students and allows user to edit or add student information."
/// </summary>

public partial class StudMonthlyAttendanceUIPopUp : SchoolBase
{
    #region Constant
    
    private const string S_SAVE_MESSAGE ="Attendance saved successfully !!!";    
    
    #endregion

    #region event handlers


    /// <summary>
    /// This function is called when the page gets  loaded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializeFields();
            if (!IsPostBack)
            {
                DisplayFormFieldsAccordingToUser();
                InitialiseAttributes();
                SetAttendanceCalendarColumnHeaders();
                BindStudentDropDown();
                if (cmbStudents.Items.Count > 0)
                {
                    DateTime oNow = Convert.ToDateTime(hidDate.Value);
                    AttendanceCalendar.VisibleDate = oNow;
                    GetAssignAttendenceToCalendar(oNow.Month, oNow.Year);
                }
                else
                {
                    trStudentCombo.Visible = false;
                    trChkAll.Visible = false;
                    trcalendar.Visible = false;
                    btnSave.Visible = false;
                    trNoRecordFound.Visible = true;
                    lblNoRecordFound.Text = Resources.LocalizedResources.ThereAreNoStudentsInThisClass;
                }

                SetJavascriptAttributes();               
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }     

    /// <summary>
    /// Function to get back to control panel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is occured when drop down list is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetAssignAttendenceToCalendar(AttendanceCalendar.VisibleDate.Month, AttendanceCalendar.VisibleDate.Year);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void AttendanceCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            GetAssignAttendenceToCalendar(e.NewDate.Month, e.NewDate.Year);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region helper methods


    /// <summary>
    /// This methosd is used to get javascript attributes.
    /// /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack });
    }

    private void BindStudentDropDown()
    {
        int iStdDiv = Convert.ToInt32(hidStdDivId.Value);
        StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStudents = oStudentCollectionBL.GetStudentListOfGivenStdDiv(iStdDiv);
        FillStudentsComboBox(oDSStudents);
    }

    /// <summary>
    /// This function is used to fill student's combo
    /// </summary>
    private void FillStudentsComboBox(DataTable aoDtStudent)
    {
        //get all class teachers
        ControlUtility.FillDropDownList(aoDtStudent, ref cmbStudents,
                                                 "Student_Id",
                                                 "Student_Name",
                                                 string.Empty);

    }

    /// <summary>
    /// This method is used to display fields according to user role.
    /// </summary>
    private void DisplayFormFieldsAccordingToUser()
    {
        string sFullAccess = "N";
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
        {
            sFullAccess = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Attendance).ToString();
            string cCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Attendance).ToString();
            
            if (sFullAccess == "True" && cCanEdit == "N")
                btnSave.Visible = false;
        }
    }

    /// <summary>
    /// This function is used to set the events to the calendar control.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p_2"></param>
    private void GetAssignAttendenceToCalendar(Int32 aiMonth, Int32 aiYear)
    {
	    int iStudentID = Convert.ToInt32(cmbStudents.SelectedValue);
        AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
        DataSet oDataSetEvents = oSchoolWiseAttendanceDetailsBL.FetchStudentMonthlyAttendance(miSchoolId, iStudentID, miAcademicYearId, aiMonth, aiYear);
        AttendanceCalendar.EventSource = oDataSetEvents.Tables[0];
    }


    /// <summary>
    /// This function is used to set the columns of the event calendar.
    /// </summary>
    private void SetAttendanceCalendarColumnHeaders()
    {
        AttendanceCalendar.EventStartDateColumnName = "Att_date";
        AttendanceCalendar.EventEndDateColumnName = "Att_date";
        AttendanceCalendar.EventDescriptionColumnName = "Status_Description";
        AttendanceCalendar.EventHeaderColumnName = "Status_Desc";
        AttendanceCalendar.EventBackColorName = "Status_BackColur";
        AttendanceCalendar.EventForeColorName = "Status_ForeColur";
    }

    private void InitialiseAttributes()
    {
        btnBack.Attributes.Add("onclick", "refreshParent('"+Request.QueryString+"')");
        btnSave.Attributes.Add("onclick", "CalculateAttendance();");
        cmbStudents.Attributes.Add("onchange", "UncheckSelectAll()");
    }

    /// <summary>
    /// This function is used to initialise field values
    /// </summary>
    private void InitializeFields()
    {
		InitializeMemberVariables();
		hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        if (!IsPostBack)
        {
            try
            {
                hidStdDivId.Value = QueryString["iStandardDivisionId"] ?? "0";
                hidDate.Value = Convert.ToDateTime(QueryString["dtDate"]).ToShortDateString() ?? DateTime.Today.ToShortDateString();
            }
            catch (Exception)
            {
                PopupMaster oMasterPage = (PopupMaster)Master;
				oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
            }
        }
    }

    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 iStudentID = 0;

            iStudentID = Convert.ToInt32(cmbStudents.SelectedValue);
            if (hidDays.Value != string.Empty || hidAbsentDays.Value != string.Empty)
            {
                string[] sArrDays = hidDays.Value.Split('$');
                string[] sArrAnsentDays = hidAbsentDays.Value.Split('$');
                string sAttendanceXML = GenerateStudentXML(sArrDays, sArrAnsentDays);
                AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
                oSchoolWiseAttendanceDetailsBL.StudentId = iStudentID;
                oSchoolWiseAttendanceDetailsBL.SchoolId = miSchoolId;
                oSchoolWiseAttendanceDetailsBL.AcademicYearId = miAcademicYearId;
                oSchoolWiseAttendanceDetailsBL.InsertedByid = miUserId;
                oSchoolWiseAttendanceDetailsBL.MarkStudentMonthlyAttendence(sAttendanceXML, AttendanceCalendar.VisibleDate.Year, AttendanceCalendar.VisibleDate.Month);
            }
            GetAssignAttendenceToCalendar(AttendanceCalendar.VisibleDate.Month, AttendanceCalendar.VisibleDate.Year);
            lblSaveSuccess.Text = Resources.LocalizedResources.AttendanceSavedSuccessfully;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// Generate XML for the students.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentXML(string[] sArrDays, string[] sArrAnsentDays)
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolWiseAttendance");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Attendance", "");

        if (hidDays.Value != string.Empty)
        {
            foreach (string sDay in sArrDays)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseAttendance", "");
                string sAtrrName = "Attendance_Date"; //oRow.Cells[iColCount]
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                DateTime odate = new DateTime(AttendanceCalendar.VisibleDate.Year, AttendanceCalendar.VisibleDate.Month, Convert.ToInt32(sDay));
                attr.Value = odate.ToString("MM/dd/yyyy");
                oXmlNode.Attributes.Append(attr);
                
                sAtrrName = "Is_Present"; //oRow.Cells[iColCount]
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = Constants.C_YES.ToString();
                oXmlNode.Attributes.Append(attr);

                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        if (hidAbsentDays.Value != string.Empty)
        {
            foreach (string sAnsentDay in sArrAnsentDays)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseAttendance", "");
                string sAtrrName = "Attendance_Date"; //oRow.Cells[iColCount]
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                DateTime odate = new DateTime(AttendanceCalendar.VisibleDate.Year, AttendanceCalendar.VisibleDate.Month, Convert.ToInt32(sAnsentDay));
                attr.Value = odate.ToString("MM/dd/yyyy");
                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                sAtrrName = "Is_Present"; //oRow.Cells[iColCount]
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = Constants.C_NO.ToString();
                oXmlNode.Attributes.Append(attr);

                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }
   
}
