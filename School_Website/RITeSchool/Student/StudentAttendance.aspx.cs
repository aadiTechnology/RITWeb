using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Collections.Generic;

public partial class StudentAttendance : SchoolBase
{
    #region Constants
    private static string msFromUrl = string.Empty;
    private const string S_SCREENS_URL = "StudentDetailsUI.aspx";
    #endregion
   
    #region "Events"
    /// <summary>
    /// This event is used to set masterpage according to login user.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            string sFromPage = string.Empty;

            if (Request.QueryString.ToString() != string.Empty)
            {
                if (QueryString["IsFrom"] != null)
                    sFromPage = QueryString["IsFrom"];
            }
            if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
            if (sFromPage == S_SCREENS_URL)
                msFromUrl = sFromPage;
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    
    /// <summary>
	/// This event is used to set java script attribute.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				SetAttendanceCalendarColumnHeaders();
				DateTime oNow = System.DateTime.Now;
				GetAssignAttendenceToCalendar(oNow.Month, oNow.Year);
				hlnkOldToppers.Visible = !Convert.ToBoolean(Session[Constants.S_SESSION_IS_NEW_ADMISSION]);

                if(!QueryString["IsFrom"].IsNullOrEmpty())
                    hlnkToppers.Visible = hlnkOldToppers.Visible = false;
                
                SetJavaScriptAttribute();
                
                //if (msFromUrl != S_SCREENS_URL)
                //    btnCancel.Visible = false;
			}            
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}

	}
	/// <summary>
	/// This event is used to show attendance.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void AttendanceCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
	{
		try
		{
			GetAssignAttendenceToCalendar(e.NewDate.Month, e.NewDate.Year);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}
	/// <summary>
	/// This method event is used to navigate to control panel when user press cancel button.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (msFromUrl == S_SCREENS_URL)
    //            ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
    //    }
    //}
	#endregion
	#region "Private methods"
    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
	/// This method is used to set javascript attribute.
	/// </summary>
	private void SetJavaScriptAttribute()
	{
		//ApplyMouseHoverEffect(new List<Button>() { btnCancel });
		hlnkToppers.Attributes.Add("onclick", "ShowToppers();return false;");        

        //if (msFromUrl == S_SCREENS_URL)
        //  btnCancel.Text = "Close";
        string sQueryString = "bIsOldTopperScreen= true";
		string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
		hlnkOldToppers.NavigateUrl = hlnkOldToppers.NavigateUrl + "?" + sEncrypt;
		hlnkOldToppers.Attributes.Add("onclick", "window.open('" + hlnkOldToppers.NavigateUrl
										+ "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=600'); return false;");
	}
	/// <summary>
	/// This function is used to set the events to the calendar control.
	/// </summary>
	/// <param name="p"></param>
	/// <param name="p_2"></param>
	private void GetAssignAttendenceToCalendar(Int32 aiMonth, Int32 aiYear)
	{
		Int32 iStudentID = 0;
		Int32 iStanderedID = 0;
		Int32 iDivisinID = 0;
        //This query string value come from student details screen
        if (!QueryString["StudentId"].IsNullOrEmpty())
            iStudentID = QueryString["StudentId"].ToInt();
        if (!QueryString["StandardId"].IsNullOrEmpty())
            iStanderedID = QueryString["StandardId"].ToInt();
        if (!QueryString["DivisionId"].IsNullOrEmpty())
            iDivisinID = QueryString["DivisionId"].ToInt();
       //This session value come when student login to this page 
        if (Session[Constants.S_SESSION_STUDENT_ID] != null)
			iStudentID = Int32.Parse(Session[Constants.S_SESSION_STUDENT_ID].ToString());
		if (Session[Constants.S_SESSION_STUDENT_STANDERED_ID] != null)
			iStanderedID = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
		if (Session[Constants.S_SESSION_STUDENT_DIVISION_ID] != null)
			iDivisinID = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);

		AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
		DataSet oDataSetEvents = oSchoolWiseAttendanceDetailsBL.FetchStudentAttendanceForCalender(miSchoolId, iStudentID, miAcademicYearId, iStanderedID, iDivisinID, aiMonth, aiYear);
		AttendanceCalendar.EventSource = oDataSetEvents.Tables[0];
		ShowAttendanceSummoryForMonth(oDataSetEvents, aiMonth, aiYear);
	}
	/// <summary>
	/// This method is used to show attendance summary.
	/// </summary>
	/// <param name="oDataSetEvents"></param>
	/// <param name="aiMonth"></param>
	/// <param name="aiYear"></param>
	private void ShowAttendanceSummoryForMonth(DataSet oDataSetEvents, int aiMonth, int aiYear)
	{
		Int32 iTotalDaysInMonth = 0;
		Int32 iTotalWorkingDaysInMonth = 0;
		Int32 iTotalPresentDaysInMonth = 0;
		Int32 iTotalAttendanceDaysInMonth = 0;

		DataTable oTotalPrsentDaysDataTable = oDataSetEvents.Tables[1];
		DataTable oTotalAttendanceDaysDataTable = oDataSetEvents.Tables[2];
		DataTable oTotalWorkingDaysDataTable = oDataSetEvents.Tables[3];
		DataTable oTotalDaysOfMonthDataTable = oDataSetEvents.Tables[4];

		if ((oTotalPrsentDaysDataTable != null) && (oTotalPrsentDaysDataTable.Rows.Count > 0) && (oTotalPrsentDaysDataTable.Rows[0][1] != DBNull.Value))
			iTotalPresentDaysInMonth = Convert.ToInt32(oTotalPrsentDaysDataTable.Rows[0][1]);
		if ((oTotalAttendanceDaysDataTable != null) && (oTotalAttendanceDaysDataTable.Rows.Count > 0) && (oTotalAttendanceDaysDataTable.Rows[0][0] != DBNull.Value))
			iTotalAttendanceDaysInMonth = Convert.ToInt32(oTotalAttendanceDaysDataTable.Rows[0][0]);
		if ((oTotalWorkingDaysDataTable != null) && (oTotalWorkingDaysDataTable.Rows.Count > 0) && (oTotalWorkingDaysDataTable.Rows[0][0] != DBNull.Value))
			iTotalWorkingDaysInMonth = Convert.ToInt32(oTotalWorkingDaysDataTable.Rows[0][0]);
		if ((oTotalDaysOfMonthDataTable != null) && (oTotalDaysOfMonthDataTable.Rows.Count > 0) && (oTotalDaysOfMonthDataTable.Rows[0][0] != DBNull.Value))
			iTotalDaysInMonth = Convert.ToInt32(oTotalDaysOfMonthDataTable.Rows[0][0]);

		lblWorkingDaysR.Text = iTotalAttendanceDaysInMonth.ToString() + " out of " + iTotalDaysInMonth.ToString();
		lblPresentDaysR.Text = iTotalPresentDaysInMonth.ToString() + " out of " + iTotalAttendanceDaysInMonth.ToString();
		lblAbsentDaysR.Text = Convert.ToString(iTotalAttendanceDaysInMonth - iTotalPresentDaysInMonth) + " out of " + iTotalAttendanceDaysInMonth.ToString();
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
	#endregion

}
