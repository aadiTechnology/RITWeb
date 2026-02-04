/* File Name :- MonthiseStaffAttendancePopup.aspx.cs
 * Created Date :- 06-Jan-2016
 * Class Description :- This class is used to manage Month wise Staff Attendance. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using SchoolEntities;
using Utility;

public partial class MonthwiseStaffAttendancePopup : SchoolBase
{
    #region Constant(s)
    
    private const string S_SR_NO = "Sr. No.";
    private const string S_NAME = "Name";
    private const string S_DESIGNATION = "Designation";
    private const string S_PRESENT_DAYS = "Present Days";
    private const string S_TOTAL_DAYS = "Total Days";
    private const string S_PERCENTAGE = "Percentage"; 

    #endregion

    #region DataMember

    private DatewiseStaffLeavesBL moDatewiseStaffLeavesBL;

    #endregion

    #region Events

    /// <summary>
    /// This Event is used to fill the data in controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moDatewiseStaffLeavesBL = new DatewiseStaffLeavesBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                ReadQueryString();
                SetJavascriptAttributes();
                FillStaffGroupCombobox();
                FillStaffAttendance();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to change selected index of combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStaffAttendance();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to formating grid views row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStaffAttendance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int iCount = 0;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (iCount == 1 || iCount == 2)
                        cell.HorizontalAlign = HorizontalAlign.Left;
                    else
                        cell.HorizontalAlign = HorizontalAlign.Center;

                    cell.Style.Add(HtmlTextWriterStyle.Padding, "0 5px 0 5px");

                    cell.Wrap = false;

                    iCount++;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {   
                for (int iCellIndex = 0; iCellIndex < e.Row.Cells.Count; iCellIndex++)
                {
                    TableCell cell = e.Row.Cells[iCellIndex];
                    cell.Style.Add(HtmlTextWriterStyle.Padding, " 0 5px 0 5px");

                    if (iCellIndex == 1 || iCellIndex == 2)
                        cell.HorizontalAlign = HorizontalAlign.Left;
                    else
                        cell.HorizontalAlign = HorizontalAlign.Center;

                    cell.Wrap = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set dash in empty cell.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStaffAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {   
                TableCellCollection cells = e.Row.Cells;
                foreach (TableCell cell in cells)
                {
                    if (cell.Text.Trim() == "&nbsp;")
                        cell.Text = "-";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region PrivateMethod(s)

    /// <summary>
    /// This Method is used to fill staffAttendance gridview.
    /// </summary>
    private void FillStaffAttendance()
    {   
        List<StaffDetails> lstStaffDetails = moDatewiseStaffLeavesBL.FillMonthWiseStaffAttendance(hidYear.Value.ToInt(), cmbStaffGroup.SelectedValue.ToInt());
        DataTable dtStaff = AddColumns();
        DataTable dtStaffAttendance = SetUserAttendance(lstStaffDetails, dtStaff);
        grdStaffAttendance.DataSource = dtStaffAttendance;
        grdStaffAttendance.DataBind();
    }

    /// <summary>
    /// This Method is used to Add datatable columns.
    /// </summary>
    private DataTable AddColumns()
    {
        DataTable dtStaff = new DataTable();
        dtStaff.AddColumns(new string[] { S_SR_NO, S_NAME, S_DESIGNATION });

        moDatewiseStaffLeavesBL.MonthDetails.ForEach(
            month =>
            {
                dtStaff.Columns.Add(month.Month);
            }
            );

        dtStaff.AddColumns(new string[] { S_PRESENT_DAYS, S_TOTAL_DAYS, S_PERCENTAGE });
        return dtStaff;
    }

    /// <summary>
    /// This Method is used to Add datatable rows and add data on gridview.
    /// </summary>
    private DataTable SetUserAttendance(List<StaffDetails> alstStaffDetails, DataTable adtStaff)
    {
        List<int> lstWeekendIds = moDatewiseStaffLeavesBL.WeekDayDetailsList.Select(days => days.OriginalWeekDayId).ToList();

        // Used for getting all Users from staff details list.
        alstStaffDetails.ForEach(
           user =>
           {
               DataRow drStaff = SetBasicDetails(adtStaff, user);
               decimal dcTotalDays = 0;
               decimal dcTotalPresentDays = 0;
               decimal dcWorkingDays = 0;

               // Used for getting all months from list.
               moDatewiseStaffLeavesBL.MonthDetails.ForEach(
                   month =>
                   {
                       int iDayOfMonth = DateTime.DaysInMonth(hidYear.Value.ToInt(), month.MonthId);
                       decimal dcAttendance = 0;

                       var oAttendance = moDatewiseStaffLeavesBL.MonthwiseStaffLeaveDetailsList.Where(att => att.StaffAttendanceUserId == user.StaffUserId && att.MonthId == month.MonthId);                       
                       if (oAttendance != null && oAttendance.Count() > 0)
                       {
                           dcAttendance = oAttendance.FirstOrDefault().PresentDays;

                           decimal HolidayCount = GetHolidayCount(month.MonthId, iDayOfMonth);
                           decimal iLeaveCount = GetLeaveCount(user.StaffUserId, month.MonthId);
                           decimal dcHolidayLeaveCount = GetHolidayLeaveCount(user.StaffUserId, month.MonthId);

                           int iWeekendCount = 0, iWeekendLeaveCount = 0;

                           bool bIsHolidayFound = false;
                           for (int iDay = 1; iDay <= iDayOfMonth; iDay++)
                           {
                               DateTime dt = new DateTime(hidYear.Value.ToInt(), month.MonthId, iDay);
                               if (lstWeekendIds.Contains(dt.DayOfWeek.ToInt()))
                               {
                                   // Check whetehre exist holiday on weekend.
                                   if (moDatewiseStaffLeavesBL.HolidayDetailsList.Any(day => dt.IsBetween(day.HolidayStartDate, day.HolidayEndDate)))
                                       bIsHolidayFound = true;

                                   // consider leave only of there is weekend but not holiday.
                                   if (!bIsHolidayFound)
                                   {
                                       if (moDatewiseStaffLeavesBL.DateWiseStaffLeavesList.Any(leaveday => leaveday.LeaveDate == dt && leaveday.DateWiseStaffUserId == user.StaffUserId && leaveday.LeaveId != 0))
                                       {
                                           iLeaveCount++;
                                           iWeekendLeaveCount++;
                                       }
                                   }

                                   if (!bIsHolidayFound)
                                       iWeekendCount++;
                               }

                               bIsHolidayFound = false;
                           }

                           if (!user.IsAdminStaff)
                           {
                               dcAttendance = dcAttendance - HolidayCount - iWeekendCount + dcHolidayLeaveCount + iWeekendLeaveCount; // +iLeaveCount;
                               dcWorkingDays = iDayOfMonth - HolidayCount - iWeekendCount;
                           }
                           else
                           {
                               //dcAttendance = dcAttendance + iLeaveCount;
                               dcWorkingDays = iDayOfMonth;
                           }

                           
                           drStaff[month.Month] = dcAttendance + "/" + dcWorkingDays;
                           HolidayCount = 0;
                       }

                       dcTotalDays = dcTotalDays + dcAttendance;
                       dcTotalPresentDays = dcTotalPresentDays + dcWorkingDays;
                       dcWorkingDays = 0;
                   }

                   );

               // Set summary details.
               drStaff[S_PRESENT_DAYS] = dcTotalDays;
               drStaff[S_TOTAL_DAYS] = dcTotalPresentDays;
               if (dcTotalPresentDays != 0)
                   drStaff[S_PERCENTAGE] = Math.Round(((dcTotalDays / dcTotalPresentDays) * 100), 2) + "%";
               else
                   drStaff[S_PERCENTAGE] = "0%";
               adtStaff.Rows.Add(drStaff);
               dcTotalPresentDays = 0;
               dcTotalDays = 0;
           }

           );
        return adtStaff;
    }

    /// <summary>
    /// This method is used to set basic details.
    /// </summary>
    /// <param name="adtStaff"></param>
    /// <param name="aoUser"></param>
    /// <returns></returns>
    private DataRow SetBasicDetails(DataTable adtStaff, StaffDetails aoUser)
    {
        DataRow drStaff = adtStaff.NewRow();
        drStaff[S_SR_NO] = aoUser.RowNo;
        drStaff[S_NAME] = aoUser.StaffUserName;
        drStaff[S_DESIGNATION] = aoUser.StaffDesignation;
        return drStaff;
    }

    /// <summary>
    /// This method is used to return leave count.
    /// </summary>
    /// <param name="aiUserId"></param>
    /// <param name="aiMonthId"></param>
    /// <returns></returns>
    private decimal GetLeaveCount(int aiUserId, int aiMonthId)
    {
        decimal iLeaveCount = 0;
        moDatewiseStaffLeavesBL.DateWiseStaffLeavesList.Where(leave => leave.DateWiseStaffUserId == aiUserId && leave.LeaveId != 0 && leave.LeaveDate.Month == aiMonthId && leave.LeaveDate.Year == hidYear.Value.ToInt()).ToList().ForEach(
            leavedetails =>
            {
                // check whether there exist leave on holiday.
                if (moDatewiseStaffLeavesBL.HolidayDetailsList.Any(day => leavedetails.LeaveDate.IsBetween(day.HolidayStartDate, day.HolidayEndDate)))
                {
                    if (leavedetails.IsHalfLeave)
                        iLeaveCount = iLeaveCount + (decimal)0.5;
                    else
                        iLeaveCount++;
                }
                else
                    iLeaveCount++;
            }
            );
        return iLeaveCount;
    }


    /// <summary>
    /// This method is used to return leave count.
    /// </summary>
    /// <param name="aiUserId"></param>
    /// <param name="aiMonthId"></param>
    /// <returns></returns>
    private decimal GetHolidayLeaveCount(int aiUserId, int aiMonthId)
    {
        decimal iLeaveCount = 0;
        moDatewiseStaffLeavesBL.DateWiseStaffLeavesList.Where(leave => leave.DateWiseStaffUserId == aiUserId && leave.LeaveId != 0 && leave.LeaveDate.Month == aiMonthId && leave.LeaveDate.Year == hidYear.Value.ToInt()).ToList().ForEach(
            leavedetails =>
            {
                // check whether there exist leave on holiday.
                if (moDatewiseStaffLeavesBL.HolidayDetailsList.Any(day => leavedetails.LeaveDate.IsBetween(day.HolidayStartDate, day.HolidayEndDate)))
                {
                    if (leavedetails.IsHalfLeave)
                        iLeaveCount = iLeaveCount + (decimal)0.5;
                    else
                        iLeaveCount++;
                }               
            }
            );
        return iLeaveCount;
    }

    /// <summary>
    /// This method is used to retutn holiday count.
    /// </summary>
    /// <param name="aiMonthId"></param>
    /// <param name="iDayOfMonth"></param>
    /// <returns></returns>
    private int GetHolidayCount(int aiMonthId, int iDayOfMonth)
    {
        int iHolidayCount = 0;
        
        // Used for getting all holidays from holiday list.
        moDatewiseStaffLeavesBL.HolidayDetailsList.ForEach(
           holiday =>
           {
               // Checking is holiday start date & holiday end date in same month and in same year.
               if (holiday.HolidayStartDate.Month == aiMonthId && holiday.HolidayEndDate.Month == aiMonthId && holiday.HolidayStartDate.Year == hidYear.Value.ToInt() && holiday.HolidayEndDate.Year == hidYear.Value.ToInt())
                   iHolidayCount = iHolidayCount + holiday.HolidayEndDate.Day - holiday.HolidayStartDate.Day + 1;

               // Checking is holiday start date in previous month & holiday end date in same month and in same year.
               if (holiday.HolidayStartDate.Month < aiMonthId && holiday.HolidayEndDate.Month == aiMonthId && holiday.HolidayStartDate.Year == hidYear.Value.ToInt() && holiday.HolidayEndDate.Year == hidYear.Value.ToInt())
                   iHolidayCount = iHolidayCount + holiday.HolidayEndDate.Day;

               // Checking is holiday start date in previous month & holiday end date in same month and in same year.
               if (holiday.HolidayEndDate.Month == aiMonthId && holiday.HolidayStartDate.Year < hidYear.Value.ToInt() && holiday.HolidayEndDate.Year == hidYear.Value.ToInt())
                   iHolidayCount = iHolidayCount + holiday.HolidayEndDate.Day;

               // Checking is holiday start date in same month & holiday end date in next month or in next year.
               if (holiday.HolidayStartDate.Month == aiMonthId && holiday.HolidayStartDate.Year == hidYear.Value.ToInt() && ((holiday.HolidayEndDate.Month > aiMonthId && holiday.HolidayEndDate.Year == hidYear.Value.ToInt()) || (holiday.HolidayEndDate.Month < aiMonthId && holiday.HolidayEndDate.Year == hidYear.Value.ToInt() + 1)))
                   iHolidayCount = iHolidayCount + (iDayOfMonth - holiday.HolidayStartDate.Day) + 1;

               // Checking is holiday start date in previous month & holiday end date in next month or in same year.
               if (holiday.HolidayStartDate.Month < aiMonthId && holiday.HolidayEndDate.Month > aiMonthId && holiday.HolidayStartDate.Year == hidYear.Value.ToInt() && holiday.HolidayEndDate.Year == hidYear.Value.ToInt())
                   iHolidayCount = iHolidayCount + iDayOfMonth;
           }

           );
        return iHolidayCount;
    }

    /// <summary>
    /// This Method is used to read query string and assign hiddenfields value.
    /// </summary>
    private void ReadQueryString()
    {   
        hidYear.Value = QueryString["Year"];
        cmbStaffGroup.SelectedValue = QueryString["StaffGroupId"];
        lblYear.Text = hidYear.Value;
    }

    /// <summary>
    /// This Method is used to Fill StaffGroup Combobox.
    /// </summary>
    private void FillStaffGroupCombobox()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_ALL);
    }

    /// <summary>
    /// This Method is used to set Javascript Attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnBack });
    }

    #endregion
}