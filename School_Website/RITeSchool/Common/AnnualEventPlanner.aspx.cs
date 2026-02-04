/* File Name    :- EventPlanner.aspx.cs
 * Purpose      :- This class is used to display annual event planner of a school.
 * Created Date :- 6/18/2008
 * Created By   :- Anu
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;
using System.Globalization;

public partial class EventPlanner : SchoolBase
{
    #region Constants

   //const string S_MESSAGE_ACADEMIC_YEAR = "Event date should be within current academic year (i.e. between ";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Event Planner\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Event Planner/";
    private const string S_FILE_NOT_FOUND = "File does not exists.";
    private const int I_FILE_SIZE_LIMIT = 2097152; 
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    #endregion

    #region Data Members

    DateTime odtStartDate;
    DateTime odtEndDate;
    private SchoolEventBL oSchoolEventBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill standard combobox and set calendar properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            oSchoolEventBL = new SchoolEventBL(miSchoolId, miUserId, miAcademicYearId);
            SetJavascriptAttributes();
            if (!IsPostBack)
            {
				if(CheckPreCondition()) {
					InitializeScreen();
                    ChangeAnnualPlannerLinkStatus();
                    FillYearCombo();
                    FillMonthCombo();
					SetEventCalendarProperties();
					GetQueryString();
					DisableAddEventForOtherUsers();
					SetAnnualPlanner();					
					ddlStandard.Focus();
				}
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill event calendar when user navigates through month.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void EventCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            hidCurrentDate.Value = e.NewDate.Month + "/" + "1" + "/" + e.NewDate.Year;
            FillEventCalendar(e.NewDate.Month, e.NewDate.Year);
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
    protected void EventCalendar_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
    {
        try
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
                e.Cell.Height = 0;
            }
            else
                e.Cell.Height = 46;
            e.Cell.BorderColor = System.Drawing.Color.Silver;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to open a popup to add/update event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void EventCalendar_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtEventDate = EventCalendar.SelectedDate;
            if (!IsDateOutsideAcademicYear(dtEventDate))
            {
                DataTable oDT = (DataTable)EventCalendar.EventSource;
                if (oDT.Rows.Count != 0)
                {
                    foreach (DataRow oDR in oDT.Rows)
                    {
                        if (Convert.ToString(oDR["Event_Date"]) == Convert.ToString(dtEventDate))
                        {
                            string EventName = Convert.ToString(oDR["Event_Title"]);                            
                            string sQueryString = "EventDate=" + dtEventDate.ToString(Constants.S_DATE_FORMAT_MARATHI,new CultureInfo("en-US")) +
                                                  "&" +
                                                  "Standard_Id=" + ddlStandard.SelectedValue + "&" + "DivisionId=" + ddlDivision.SelectedValue;
                            string sEncryptEventDate = Utility.CommonUtility.EncryptQuerystring(sQueryString);

                            // This will transfer Event_Date in encrypeted format to Schoolwise_Events_Pop_Up page.
                            Response.Write("<Script language='javascript'>window.open('../Admin/Schoolwise_Events_Pop_Up.aspx?" + sEncryptEventDate +
                             "','_new','left=0, top=0, height=510, width=700, status=no, resizable= no, scrollbars= yes')</Script>");
                            EventCalendar.SelectedDate = Convert.ToDateTime("1/1/0001 12:00:00 AM",new CultureInfo("en"));
                            break;                            
                        }
                    }
                }
            }
            else
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = Resources.LocalizedResources.MsgAnnualEventPlanner1
                    + odtStartDate.ToString("d MMM yyyy") + " " + Resources.LocalizedResources.To + " "
                    + odtEndDate.ToString("d MMM yyyy") + ").";
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill annual planeer according to selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblPlanner.Visible = true;
            FillDivisionCombo();
            EventCalendar.VisibleDate = Convert.ToDateTime(hidCurrentDate.Value,new CultureInfo("en"));
            FillEventCalendar(Convert.ToDateTime(hidCurrentDate.Value).Month, Convert.ToDateTime(hidCurrentDate.Value).Year);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblPlanner.Visible = true;
            EventCalendar.VisibleDate = Convert.ToDateTime(hidCurrentDate.Value, new CultureInfo("en"));
            FillEventCalendar(Convert.ToDateTime(hidCurrentDate.Value).Month, Convert.ToDateTime(hidCurrentDate.Value).Year);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill annual planner according to month.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetControlState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill annual planner according to year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetControlState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete annual planner file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {
            oSchoolEventBL.DeleteFileDetails();
            lblSuccess.Text = "Annual planner file Deleted Successfully!!!";
            ChangeAnnualPlannerLinkStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save and upload Anual Planner link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sLinkName;
            string sFileUploadErr = UploadNoticeFile(out sLinkName);
            if (string.IsNullOrEmpty(sFileUploadErr))
            {
                oSchoolEventBL.SaveFileDetails(sLinkName);
                lblSuccess.Text = "File Uploaded Successfully!!!";
                ChangeAnnualPlannerLinkStatus();
            }
            else
            {
                lblError.Text = sFileUploadErr;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to update Annual Planner link status.
    /// </summary>
    private void ChangeAnnualPlannerLinkStatus()
    {
        lnkbtnAnnualPlanner.Visible = false;
        lnkbtnAnnualPlannerread.Visible = true;
        string sCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AnnualEventPlanner).ToString();
        if (moUserRole == Constants.UserRoles.Admin || (moUserRole == Constants.UserRoles.Teacher && sCanEdit == Constants.S_YES) || (moUserRole == Constants.UserRoles.Supervisor && sCanEdit == Constants.S_YES))
        {
            lnkbtnAnnualPlanner.Visible = true;
            lnkbtnAnnualPlannerread.Visible = false;
        }
        lnkbtnAnnualPlanner.Attributes.Add("onclick", "ShowAnnualPlannerPopup(); return false;");
        SchoolEventBL oSchoolEventBL = new SchoolEventBL(miSchoolId, miUserId, miAcademicYearId);
        string sCurrentFeeLinkFileName = oSchoolEventBL.GetFileDetails();
        if (string.IsNullOrEmpty(sCurrentFeeLinkFileName))
        {
            btnView.Visible = false;
            btnDelete.Visible = false;
            lnkbtnAnnualPlannerread.Visible = false;
        }
        else
        {
            string sNewFileName = S_FOLDER_PATH + sCurrentFeeLinkFileName;
            btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            lnkbtnAnnualPlannerread.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            btnView.Visible = true;
            btnDelete.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadNoticeFile(out string asFileName)
    {
        asFileName = string.Empty;
        if (fileUploadItems.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName.ToString());
            if (fileUploadItems.HasFile)
            {
                if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                {
                    string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                    fileUploadItems.SaveAs(sLinkPath);
                    asFileName = sLinkName;
                }
                else
                {
                    sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
            }
            else
            {
                sReturnErrorMsg = S_FILE_NOT_FOUND;
                throw new System.IO.FileNotFoundException();
            }
            return sReturnErrorMsg;
        }
        return string.Empty;
    }

	/// <summary>
	/// This method checks the preconditons 
	/// </summary>
	/// <returns></returns>
	private bool CheckPreCondition() {
		bool bReturn = false;
		string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AnnualEventPlanner);
		if(sLinks.Equals(string.Empty)) {
			divErr.Visible = false;
			bReturn = true;
		}
		else {
			divErr.InnerHtml = sLinks;
			HideControls();
		}
		return bReturn;
	}
	
    /// <summary>
    /// This Method Hides controls
    /// </summary>
	private void HideControls() 
    {
		trLegend.Visible = false;
		trPlanner.Visible = false;
		trNotes.Visible = false;
	}

    /// <summary>
    /// This method is used to fill standard dropdownlist.
    /// </summary>
    private void FillStandardCombo()
    {      
        DataTable oDtStandardCollection;
        if (moUserRole == Constants.UserRoles.Teacher&& !Boolean.Parse(hidUserHasFullAccess.Value))
        {            
            oDtStandardCollection = SchoolWiseTeacherMasterCollectionBL.GetAssociatedStdLstForTeacher(miUserId, miAcademicYearId);
        }
        else
        {
            StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
            oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        }
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       string.Empty);
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            if (Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            {
                DataSet oDS = SchoolWiseTeacherMasterBL.GetTeacherDetailsForControlPanel(Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]),
                                                                           miSchoolId, miAcademicYearId);
                string sStandardId = oDS.Tables[0].Rows[0]["Standard_Id"].ToString();
                ddlStandard.Items.FindByValue(sStandardId).Selected = true;
            }
        }
        else
            ddlStandard.SelectedIndex = 0;
    }

    /// <summary>
    /// This method is used to fill standard dropdownlist.
    /// </summary>
    private void FillDivisionCombo()
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection;
        if(hidStandardId.Value != string.Empty)
            oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(hidStandardId.Value.ToInt());
        else
            oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(ddlStandard.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
    }

    /// <summary>
    /// This method is used to fill year combo box.
    /// </summary>
    private void FillYearCombo()
    {       
        List<string> oLstYear = SchoolWiseAcademicYearMasterBL.GetYearsForAnnualPalanner(miSchoolId);
        ListSource.FillDropDownList(oLstYear,cmbYears,
                                     string.Empty,
                                     string.Empty,
                                    string.Empty);
    }

    /// <summary>
    /// This method is used to fill month combobox.
    /// </summary>
    private void FillMonthCombo()
    {
        List<MonthMaster> oLstMonths = SchoolWiseAcademicYearMasterBL.GetAllMonth();
        ListSource.FillDropDownList(oLstMonths, cmbMonth, "Month", "MonthID", string.Empty);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
           ApplyMouseHoverEffect( new List<Button>{btnSave});
    }
   
    /// <summary>
    /// This method check that given date is outside academic year or not.
    /// </summary>
    /// <param name="aoDtCurrentDate"></param>
    /// <returns>Boolean</returns>
    private Boolean IsDateOutsideAcademicYear(DateTime aoDtCurrentDate)
    {
        if (ddlStandard.Visible && !string.IsNullOrEmpty(ddlStandard.SelectedValue) && Convert.ToInt32(ddlStandard.SelectedValue) > 0)
        {
            DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId, miAcademicYearId, Convert.ToInt32(ddlStandard.SelectedValue));
            if (oDT.Rows.Count > 0)
            {
                odtStartDate = (DateTime)oDT.Rows[0]["StartDate"];
                odtEndDate = (DateTime)oDT.Rows[0]["EndDate"];
            }
            else
            {
                SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, miAcademicYearId);
                odtStartDate = oSchoolWiseAcademicYearMasterBL.StartDate;
                odtEndDate = oSchoolWiseAcademicYearMasterBL.EndDate;
            }
        }
        else
        {
            SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, miAcademicYearId);
            odtStartDate = oSchoolWiseAcademicYearMasterBL.StartDate;
            odtEndDate = oSchoolWiseAcademicYearMasterBL.EndDate;
        }        
        
        if (aoDtCurrentDate < odtStartDate || aoDtCurrentDate > odtEndDate)
            return true;
        else
            return false;
    }

    /// <summary>
    /// This method is used to set the column headers of event calendar.
    /// </summary>
    private void SetEventCalendarProperties()
    {
        EventCalendar.EventStartDateColumnName = "Event_Date";
        EventCalendar.EventEndDateColumnName = "Event_Date";
        EventCalendar.EventDescriptionColumnName = "Event_Title";
        EventCalendar.EventHeaderColumnName = "Event_Desc";
        EventCalendar.EventBackColorName = "Event_BackColor";
        EventCalendar.EventForeColorName = "Event_ForeColor";
    }

    /// <summary>
    /// This method is used to set the events to the calendar control.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p_2"></param>
    private void FillEventCalendar(int aiMonth, int aiYear)
    {       
        int iStandardId = 0;
        int iDivisionId = 0;
        if (moUserRole != Constants.UserRoles.Student)
            iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);

        if (moUserRole != Constants.UserRoles.Student)
            iDivisionId = Convert.ToInt32(ddlDivision.SelectedValue);

        SchoolEventBL oEventDescriptionBL = new SchoolEventBL();
        DataTable oDTEventsData = null;
        cmbMonth.SelectedValue = aiMonth.ToString();

        ListItem lstItem = cmbYears.Items.FindByValue(aiYear.ToString());
        if (lstItem != null)
            cmbYears.SelectedValue = aiYear.ToString();

        if (moUserRole == Constants.UserRoles.Student)
        {
            iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
            iDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);
            int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID].ToString());
            oDTEventsData = oEventDescriptionBL.GetEventsDataForStudent(miSchoolId, miAcademicYearId, aiMonth, aiYear, iStudentId);
        }
        else
            oDTEventsData = oEventDescriptionBL.GetEventsData(miSchoolId, miAcademicYearId, aiMonth, aiYear, iStandardId, iDivisionId);

        Char cCanEdit = Constants.C_NO;
        cCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AnnualEventPlanner);

        if (moUserRole != Constants.UserRoles.Admin && (moUserRole == Constants.UserRoles.Student|| cCanEdit == Constants.C_NO))
        {
            DataTable oDTEvents1=
            (from event1 in oDTEventsData.AsEnumerable()
             where (Convert.ToInt32(event1["Standard_Id"]) != 0 || Convert.ToInt32(event1["Event_Id"]) == 0)
            select event1).CopyToDataTable();            
            EventCalendar.EventSource = oDTEvents1;
        }
        else
        {
            List<int> eventDetailIds = (from event1 in oDTEventsData.AsEnumerable()
                                        join event2 in oDTEventsData.AsEnumerable()
                                        on event1["Event_Id"] equals event2["Event_Id"]
                                        where Convert.ToInt32(event1["Standard_Id"]) != 0
                                        && Convert.ToInt32(event2["Standard_Id"]) == 0
                                        select Convert.ToInt32(event2["Schoolwise_Event_Detail_Id"])).Distinct().ToList();

            DataTable oDTEvents;
            if (eventDetailIds.Count == 0)
                oDTEvents = oDTEventsData;
            else
            {
                oDTEvents =
                ((from event1 in oDTEventsData.AsEnumerable()
                  select event1)
                                             .Except
                                             (
                                              from event1 in oDTEventsData.AsEnumerable()
                                              join ids in eventDetailIds.AsEnumerable()
                                              on Convert.ToInt32(event1["Schoolwise_Event_Detail_Id"]) equals ids
                                              where Convert.ToInt32(event1["Standard_Id"]) == 0
                                              select event1
                                             )).CopyToDataTable();

            }

            EventCalendar.EventSource = oDTEvents;
        }
    }

    /// <summary>
    /// This method is used to disable event of calender control, if user role is other than admin.
    /// </summary>
    private void DisableAddEventForOtherUsers()
    {
        if (moUserRole != Constants.UserRoles.Admin)
        {
            Char cCanEdit = Constants.C_NO;
            if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            {
                SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
                cCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AnnualEventPlanner);
            }
            if (cCanEdit == Constants.C_NO)
            {
                EventCalendar.SelectionChanged -= EventCalendar_SelectionChanged;
                EventCalendar.Controls.IsReadOnly.Equals(true);
                EventCalendar.SelectionMode = CalendarSelectionMode.None;
            }
            if (moUserRole == Constants.UserRoles.Student)
            {
                tblLegend.Visible = true;
                tdStd.Visible = false;
                tdCmbStd.Visible = false;
                tdDiv.Visible = false;
                tdCmbDiv.Visible = false;
            }
        }

        if (moUserRole != Constants.UserRoles.Student)
        {
            FillStandardCombo();
            FillDivisionCombo();
        }

        FillEventCalendar(Convert.ToDateTime(hidEventDate.Value).Month, Convert.ToDateTime(hidEventDate.Value).Year);
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void GetQueryString()
    {
        if (!QueryString["EventDate"].IsNull())
            hidEventDate.Value = QueryString["EventDate"];
        else
            hidEventDate.Value = System.DateTime.Now.ToString();

        if (!QueryString["Standard_Id"].IsNull())
            hidStandardId.Value = QueryString["Standard_Id"];

        if (!QueryString["DivisionId"].IsNull())
            hidDivisionId.Value = QueryString["DivisionId"];

        if (hidStandardId.Value == String.Empty)
            hidCurrentDate.Value = DateTime.Now.ToString();
        else
            hidCurrentDate.Value = hidEventDate.Value;
    }

    /// <summary>
    /// This method is used to set annual planner for a particular standard.
    /// </summary>
    private void SetAnnualPlanner()
    {
        if (hidStandardId.Value != string.Empty)
        {
            if (hidDivisionId.Value != string.Empty)
                ddlDivision.SelectedValue = hidDivisionId.Value;

            ddlStandard.SelectedValue = hidStandardId.Value;
            EventCalendar.VisibleDate = Convert.ToDateTime(hidEventDate.Value,new CultureInfo("en"));
            FillEventCalendar(Convert.ToDateTime(hidEventDate.Value).Month, Convert.ToDateTime(hidEventDate.Value).Year);
        }
    }

    /// <summary>
    /// Function used to initialze screen content
    /// </summary>
    private void InitializeScreen()
    {
        if (moUserRole == Constants.UserRoles.Teacher)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.AnnualEventPlanner).ToString();
    }

    /// <summary>
    /// This method is used to set values of month and year.
    /// </summary>
    private void SetControlState()
    {
        tblPlanner.Visible = true;
        int year = cmbYears.SelectedValue != Constants.S_ZERO ? Convert.ToInt32(cmbYears.SelectedItem.Text) : Convert.ToDateTime(hidCurrentDate.Value).Year;
        int month = cmbMonth.SelectedValue != Constants.S_ZERO ? Convert.ToInt32(cmbMonth.SelectedValue) : Convert.ToDateTime(hidCurrentDate.Value).Month;
        hidCurrentDate.Value = (month + "/1" + "/" + year);
        EventCalendar.VisibleDate = Convert.ToDateTime(hidCurrentDate.Value,new CultureInfo("en"));
        FillEventCalendar(Convert.ToDateTime(Convert.ToDateTime(hidCurrentDate.Value).ToString("MM-dd-yyyy")).Month, Convert.ToDateTime(hidCurrentDate.Value).Year);
    }

    #endregion
}