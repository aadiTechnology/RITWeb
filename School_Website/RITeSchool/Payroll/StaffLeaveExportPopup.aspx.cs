/* File Name - StaffLeaveExportPopup.aspx.cs
 * Created By -Sachin
 * Created Date - 13 Aug 2015
 * Description - This class is used to export leave details.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Threading;
using System.Web;
using System.Text;
using System.Configuration;

public partial class StaffLeaveExportPopup : ExportDataTable
{
    #region Constant(s)

    private const string S_PREFIX = "Day ";
    private const string S_TOTAL_LEAVES = "Total Leaves";
    private const string S_LATE_MARK = "Late Mark";
    private const string S_TOTAL_LATE_MARK_COUNT = "Total Late Mark Count";
    private const string S_LEAVE_DEDUCTED_FOR_LATE_MARK = "Leave Deducted for Late Mark";
    private const string S_TOTAL_HOURS = "Total Hours";
    private const string S_PRESENT_DAYS = "Present Days";
    private const string S_BOLD_START = "<B>";
    private const string S_BOLD_END = "</B>";
    private const string S_LEAVE_START = "<span style='color:%COLOR%;font-weight:bold'>";
    private const string S_COLOR = "%COLOR%";
    private const string S_LEAVE_END = "</span>";

    private const string S_NAME = "Name";
    private const string S_YEAR = "Year";
    private const string S_MONTH = "Month";
    private const string S_AVERAGE = "Average";

    private const string S_SR_NO = "Sr. No.";
    private const string S_START_DATE = "Start Date";
    private const string S_END_DATE = "End Date";

    #endregion

    #region Data Member(s)

    private StaffLeaveDetailsBL moStaffLeaveDetailsBL;
    private StaffLeaveDetailsBL moStaffLeavesBL;
    private int miUsersTotalWorkingHours = 0;
    private int miTotalWorkingHours = 0;

    #endregion

    #region Property(s)

    public int NoOfDays
    {
        get { return DateTime.DaysInMonth(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt()); }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill combo boxes and set java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStaffLeaveDetailsBL = new StaffLeaveDetailsBL();
            if (!IsPostBack)
            {
                FillComboboxes();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up user combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers(cmbStaffGroups.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), cmbUser);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up user combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers(cmbStaffGroups.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), cmbUser);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ExportLeaves(false);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export leave details in detail.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMIS_Click(object sender, EventArgs e)
    {
        try
        {
            ExportLeaves(true);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to export leave balance details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportBalance_Click(object sender, EventArgs e)
    {
        try
        {
            List<LeaveBalanceDetails> lstLeaveBalanceDetails = moStaffLeaveDetailsBL.GetLeaveBalanceToExport(miSchoolId, cmbStaffgroupForBalance.SelectedValue.ToInt(), cmbUserForBalance.SelectedValue.ToInt());

            DataTable oDt = AddLeaveColumns(lstLeaveBalanceDetails);
            
            lstLeaveBalanceDetails.OrderBy(usr => usr.RowNo).Select(usr => usr.UserId).Distinct().ToList()
                .ForEach
                (
                    userId =>
                    {
                        DataRow dr = oDt.NewRow();

                        var leaveDetails = lstLeaveBalanceDetails.Where(lv => lv.UserId == userId).OrderBy(lv => lv.RowNo).ToList();

                        dr[S_NAME] = leaveDetails[0].UserName;

                        foreach (var leave in leaveDetails)
                        {
                            dr[leave.LeaveName] = leave.LeaveBalance;
                        }

                        decimal dcTotalLeaves = leaveDetails.Sum(lv => lv.LeaveBalance);
                        dr[S_TOTAL_LEAVES] = S_BOLD_START + dcTotalLeaves + S_BOLD_END;

                        oDt.Rows.Add(dr);
                    }
                );

            ExportToExcel(GetFileName(), oDt);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This even it used to fill user combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffgroupForBalance_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers(cmbStaffgroupForBalance.SelectedValue.ToInt(), DateTime.Now.Year, cmbUserForBalance);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export staff leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportStaffLeave_Click(object sender, EventArgs e)
    {
        try
        {
            ExportStaffLeaves();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user combo according to selected staff group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers(cmbGroups.SelectedValue.ToInt(), DateTime.Now.Year, cmbStaff);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to export staff attendance.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStaffAttendance_Click(object sender, EventArgs e)
    {
        try
        {
            List<UserLeaveDetails> lstUserLeaveDetails = moStaffLeaveDetailsBL.GetLeaveDetailsToExport(cmbStaffGroups.SelectedValue.ToInt(), miSchoolId, cmbUser.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt());
            ExportStaffAttendance(lstUserLeaveDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to return file name.
    /// </summary>
    /// <returns></returns>
    private string GetFileName()
    {
        int iYear = cmbYear.SelectedValue.ToInt();
        int iMonthId = cmbMonth.SelectedValue.ToInt();
        int iStaffGroupId = cmbStaffGroups.SelectedValue.ToInt();

        string sFileName;
        if (iStaffGroupId == 0)
            sFileName = "StaffLeaves_" + cmbMonth.SelectedItem.Text;
        else
            sFileName = "StaffLeaves_" + cmbStaffGroups.SelectedItem.Text.Replace(" ", string.Empty) + "_" + cmbMonth.SelectedItem.Text;

        return sFileName + ".xls";
    }

    /// <summary>
    /// This method is used to add columns in data table.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <returns></returns>
    private DataTable AddColumns(List<UserLeaveDetails> alstUserLeaveDetails, bool abIsMisReport)
    {
        DataTable oDt = new DataTable();
        oDt.Columns.Add(S_YEAR);
        oDt.Columns.Add(S_MONTH);
        oDt.Columns.Add(S_NAME);

        if (abIsMisReport)
        {
            for (int iDay = 1; iDay <= NoOfDays; iDay++)
            {
                oDt.Columns.Add(S_PREFIX + iDay.ToString());
            }

            oDt.Columns.Add(S_TOTAL_HOURS);
            oDt.Columns.Add(S_AVERAGE);

            moStaffLeaveDetailsBL.ConfiguredLeaves.OrderBy(lv => lv.OriginalLeaveId).ToList()
                .ForEach(
                lv =>
                {
                    oDt.Columns.Add(lv.ShortName);
                }
                );
            oDt.Columns.Add(S_TOTAL_LATE_MARK_COUNT);
            oDt.Columns.Add(S_LEAVE_DEDUCTED_FOR_LATE_MARK);
        }
        else
        {
            alstUserLeaveDetails.Select(uld => uld.Day).Distinct().OrderBy(day => day).ToList().ForEach
                (
                    uld =>
                    {
                        oDt.Columns.Add(S_PREFIX + uld.ToString());
                    }

                );
        }


        oDt.Columns.Add(S_TOTAL_LEAVES);

        if(abIsMisReport)
            oDt.Columns.Add(S_PRESENT_DAYS);
        return oDt;
    }

    /// <summary>
    /// This method is used to add empty row.
    /// </summary>
    /// <param name="aoDt"></param>
    private void AddEmptyRow(DataTable aoDt)
    {
        DataRow dr1 = aoDt.NewRow();
        aoDt.Rows.Add(dr1);
    }

    /// <summary>
    /// This method is used to add basic details.
    /// </summary>
    /// <param name="aoDt"></param>
    /// <param name="adrBasicDetails"></param>
    /// <param name="aoUser"></param>
    private void AddBasicDetails(DataTable aoDt, DataRow adrBasicDetails, int aiUserId)
    {
        var oUser = moStaffLeaveDetailsBL.DaywiseStaffAttendances.Where(usr => usr.UserId == aiUserId).FirstOrDefault();
        if (oUser != null)
        {
            adrBasicDetails[S_NAME] = S_BOLD_START + oUser.Name + S_BOLD_END;
            adrBasicDetails[S_YEAR] = S_BOLD_START + cmbYear.SelectedItem.Text + S_BOLD_END;
            adrBasicDetails[S_MONTH] = S_BOLD_START + (new DateTime(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt(), 1).ToString("MMM")) + S_BOLD_END;
            aoDt.Rows.Add(adrBasicDetails);
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnExport, btnClose, btnExportBalance, btnExportStaffLeave });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill all combo boxes.
    /// </summary>
    private void FillComboboxes()
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);

        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffGroups, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);

        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbGroups, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);
        FillUsers(cmbGroups.SelectedValue.ToInt(), DateTime.Now.Year, cmbStaff);

        ListSource.FillDropDownList(oSalaryDetailsBL.Months, cmbMonth, "Month", "MonthId", Constants.S_SELECT);
        ListSource.FillDropDownList(oSalaryDetailsBL.Years, cmbYear, "Year", "Year", Constants.S_SELECT);
        FillUsers(cmbStaffGroups.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), cmbUser);

        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffgroupForBalance, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);
        FillUsers(cmbStaffgroupForBalance.SelectedValue.ToInt(), DateTime.Now.Year, cmbUserForBalance);

        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to fill up user combo box.
    /// </summary>
    private void FillUsers(int aiStaffGroupId, int aiYear, DropDownList aoDropDownList)
    {
        List<UserBasicDetails> lstUserBasicDetails = moStaffLeaveDetailsBL.GetAllUsers(aiStaffGroupId, miSchoolId, miAcademicYearId, aiYear);
        ListSource.FillDropDownList(lstUserBasicDetails, aoDropDownList, "StaffName", "UserId", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to add final total.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <param name="aoDt"></param>
    /// <param name="aiRowIndex"></param>
    private void AddFinalTotal(List<UserLeaveDetails> alstUserLeaveDetails, DataTable aoDt, int aiRowIndex, bool abIsMisReport)
    {
        decimal dcTotalFullLeave = alstUserLeaveDetails.Where(lv => lv.IsHalfLeave == false).Count();
        decimal dcTotalHalfLeave = alstUserLeaveDetails.Where(lv => lv.IsHalfLeave).Count();
        decimal dcTotalPartialLeave = alstUserLeaveDetails.Where(lv => lv.PartialLeaveId != 0).Count();

        decimal dcLateMarkCountCount = moStaffLeaveDetailsBL.DatewiseStaffLeaves.Select(dsl => new { UserId = dsl.UserId, dsl.LateMarkLeaveCount }).Distinct().Sum(dsl => dsl.LateMarkLeaveCount);

        aoDt.Rows[aiRowIndex][S_TOTAL_LEAVES] = S_BOLD_START + (dcTotalFullLeave + (dcTotalHalfLeave * (decimal)0.5) + (dcTotalPartialLeave * (decimal)0.5) + dcLateMarkCountCount) + S_BOLD_END;
              
        if (abIsMisReport)
        {
            aoDt.Rows[aiRowIndex][S_PRESENT_DAYS] = S_BOLD_START + ((decimal)miTotalWorkingHours / 8) + S_BOLD_END;
            for (int iIndex = 2; iIndex <= NoOfDays; iIndex++)
            {
                if (!alstUserLeaveDetails.Any(lv => lv.Day == iIndex))
                    aoDt.Rows[aiRowIndex][S_PREFIX + iIndex.ToString()] = Constants.S_ZERO;
            }
        }


    }

    /// <summary>
    /// This method is used to add row total.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <param name="aoDt"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiMonthId"></param>
    /// <param name="aiUserId"></param>
    private void AddRowTotal(List<UserLeaveDetails> alstUserLeaveDetails, DataTable aoDt, int aiRowIndex, int aiMonthId, int aiUserId)
    {
        decimal dcFullLeave = alstUserLeaveDetails.Where(lv1 => lv1.UserId == aiUserId && lv1.MonthId == aiMonthId && lv1.IsHalfLeave == false).Count();
        decimal dcHalfLeave = alstUserLeaveDetails.Where(lv1 => lv1.UserId == aiUserId && lv1.MonthId == aiMonthId && lv1.IsHalfLeave).Count();
        decimal dcPartialLeave = alstUserLeaveDetails.Where(lv1 => lv1.UserId == aiUserId && lv1.MonthId == aiMonthId && lv1.PartialLeaveId != 0).Count();

        aoDt.Rows[aiRowIndex][S_TOTAL_LEAVES] = S_BOLD_START + (dcFullLeave + (dcHalfLeave * (decimal)0.5) + (dcPartialLeave * (decimal)0.5)) + S_BOLD_END;
    }

    /// <summary>
    /// This method is used to add column total.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <param name="aoDt"></param>
    private void AddColumnTotal(List<UserLeaveDetails> alstUserLeaveDetails, DataTable aoDt, bool abIsMisReport)
    {
        DataRow drRowTotal = aoDt.NewRow();
        drRowTotal[S_NAME] = S_BOLD_START + S_TOTAL_LEAVES + S_BOLD_END;
        alstUserLeaveDetails.Select(day => day.Day).Distinct().ToList().ForEach
        (
            day =>
            {
                decimal dcFullLeave = alstUserLeaveDetails.Where(lv => lv.Day == day && lv.IsHalfLeave == false).Count();
                decimal dcHalfLeave = alstUserLeaveDetails.Where(lv => lv.Day == day && lv.IsHalfLeave).Count();
                decimal dcPartialLeave = alstUserLeaveDetails.Where(lv => lv.Day == day && lv.PartialLeaveId != 0).Count();
                drRowTotal[S_PREFIX + day.ToString()] = S_BOLD_START + (dcFullLeave + (dcHalfLeave * (decimal)0.5) + (dcPartialLeave * (decimal)0.5)) + S_BOLD_END;
                dcFullLeave = 0;
                dcHalfLeave = 0;
                dcPartialLeave = 0;
            }

        );

        if (abIsMisReport)
        {
            AddLeaveColumnsTotal(alstUserLeaveDetails, drRowTotal);
            AddLateMarkColumnTotal(drRowTotal);
            AddTotalHoursTotal(alstUserLeaveDetails, drRowTotal);
        }

        aoDt.Rows.Add(drRowTotal);
    }

    /// <summary>
    /// This method is used to add total hour's total.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <param name="adrRowTotal"></param>
    private static void AddTotalHoursTotal(List<UserLeaveDetails> alstUserLeaveDetails, DataRow adrRowTotal)
    {
        decimal dcTotalFullLeave1 = alstUserLeaveDetails.Where(lv => lv.IsHalfLeave == false).Count();
        decimal dcTotalHalfLeave1 = alstUserLeaveDetails.Where(lv => lv.IsHalfLeave).Count();
        decimal dcTotalPartialLeave1 = alstUserLeaveDetails.Where(lv => lv.PartialLeaveId != 0).Count();

        decimal dcTotal = dcTotalFullLeave1 + (dcTotalHalfLeave1 * (decimal)0.5) + (dcTotalPartialLeave1 * (decimal)0.5);
        //adrRowTotal[S_TOTAL_HOURS] = dcTotal * 8;
    }

    /// <summary>
    /// THis method is used to add late mark total.
    /// </summary>
    /// <param name="adrRowTotal"></param>
    private void AddLateMarkColumnTotal(DataRow adrRowTotal)
    {
        if (moStaffLeaveDetailsBL.DatewiseStaffLeaves.Count() > 0)
        {
            adrRowTotal[S_TOTAL_LATE_MARK_COUNT] = S_BOLD_START + moStaffLeaveDetailsBL.DatewiseStaffLeaves.Count() + S_BOLD_END;
            adrRowTotal[S_LEAVE_DEDUCTED_FOR_LATE_MARK] = S_BOLD_START + moStaffLeaveDetailsBL.DatewiseStaffLeaves.Select(dsl => new { UserId = dsl.UserId, dsl.LateMarkLeaveCount }).Distinct().Sum(dsl => dsl.LateMarkLeaveCount) + S_BOLD_END;
        }
    }

    /// <summary>
    /// This method is used to add leave total.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    /// <param name="drRowTotal"></param>
    private void AddLeaveColumnsTotal(List<UserLeaveDetails> alstUserLeaveDetails, DataRow drRowTotal)
    {
        moStaffLeaveDetailsBL.ConfiguredLeaves.ToList().ForEach
        (
            clv =>
            {
                decimal dcFullLeave = alstUserLeaveDetails.Where(lv => lv.LeaveId == clv.LeaveId && lv.IsHalfLeave == false).Count();
                decimal dcHalfLeave = alstUserLeaveDetails.Where(lv => lv.LeaveId == clv.LeaveId && lv.IsHalfLeave).Count();
                decimal dcPartialLeave = alstUserLeaveDetails.Where(lv => lv.LeaveId == clv.LeaveId && lv.PartialLeaveId != 0).Count();
                drRowTotal[clv.ShortName] = S_BOLD_START + (dcFullLeave + (dcHalfLeave * (decimal)0.5) + (dcPartialLeave * (decimal)0.5)) + S_BOLD_END;
                dcFullLeave = 0;
                dcHalfLeave = 0;
                dcPartialLeave = 0;
            }

        );
    }

    /// <summary>
    /// This method is used to add leave balance columns.
    /// </summary>
    /// <param name="alstLeaveBalanceDetails"></param>
    /// <returns></returns>
    private static DataTable AddLeaveColumns(List<LeaveBalanceDetails> alstLeaveBalanceDetails)
    {
        DataTable oDt = new DataTable();

        oDt.Columns.Add(S_NAME);

        alstLeaveBalanceDetails.OrderBy(usr => usr.RowNo).Select(usr => usr.LeaveName).Distinct().ToList()
            .ForEach
            (
                leave =>
                {
                    oDt.Columns.Add(leave);
                }
            );

        oDt.Columns.Add(S_TOTAL_LEAVES);
        return oDt;
    }

    /// <summary>
    /// This method is used to export leave details.
    /// </summary>
    /// <param name="abIsMisReport"></param>
    private void ExportLeaves(bool abIsMisReport)
    {
        List<UserLeaveDetails> lstUserLeaveDetails = moStaffLeaveDetailsBL.GetLeaveDetailsToExport(cmbStaffGroups.SelectedValue.ToInt(), miSchoolId, cmbUser.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt());

        DataTable oDt = AddColumns(lstUserLeaveDetails, abIsMisReport);

        int iRowIndex = 0;
        miTotalWorkingHours = 0;

        int iMonthId = cmbMonth.SelectedValue.ToInt();

            moStaffLeaveDetailsBL.DaywiseStaffAttendances.OrderBy(usr => usr.SrNo).Select(uld => uld.UserId).Distinct().ToList().ForEach
                    (
                        userId =>
                        {
                            DataRow dr = oDt.NewRow();

                            AddBasicDetails(oDt, dr, userId);

                            List<UserLeaveDetails> lstLeaves = lstUserLeaveDetails.Where(usr => usr.UserId == userId && usr.MonthId == iMonthId).ToList();

                            lstLeaves.ToList().ForEach
                            (
                                lv =>
                                {
                                    if (lv.IsHalfLeave)
                                    {
                                        if (lv.PartialLeaveId == 0)
                                        {
                                            if (lv.IsLateMark)
                                                oDt.Rows[iRowIndex][S_PREFIX + lv.Day.ToString()] = S_LEAVE_START.Replace(S_COLOR, lv.LeaveColor) + lv.LeaveName + "(H) / " + S_LATE_MARK + S_LEAVE_END;
                                            else
                                                oDt.Rows[iRowIndex][S_PREFIX + lv.Day.ToString()] = S_LEAVE_START.Replace(S_COLOR, lv.LeaveColor) + lv.LeaveName + "(H)" + S_LEAVE_END;
                                        }
                                        else
                                        {
                                            var partialLeave = moStaffLeaveDetailsBL.ConfiguredLeaves.Where(configLeave => configLeave.LeaveId == lv.PartialLeaveId).Select(configLeave => configLeave.ShortName).FirstOrDefault();
                                            oDt.Rows[iRowIndex][S_PREFIX + lv.Day.ToString()] = S_LEAVE_START.Replace(S_COLOR, lv.LeaveColor) + lv.LeaveName + "(H)" + " / " + partialLeave + "(H)" + S_LEAVE_END;
                                        }
                                    }
                                    else
                                    {
                                        oDt.Rows[iRowIndex][S_PREFIX + lv.Day.ToString()] = S_LEAVE_START.Replace(S_COLOR, lv.LeaveColor) + lv.LeaveName + S_LEAVE_END;
                                    }

                                    if (!abIsMisReport)
                                        AddRowTotal(lstUserLeaveDetails, oDt, iRowIndex, iMonthId, userId);
                                }

                            );

                            if (abIsMisReport)
                            {
                                for (int iIndex = 1; iIndex <= NoOfDays; iIndex++)
                                {
                                    if (!lstLeaves.Any(lv => lv.Day == iIndex))
                                    {
                                        if (moStaffLeaveDetailsBL.DatewiseStaffLeaves.Any(dsl => dsl.UserId == userId && dsl.Date.Day == iIndex))
                                            oDt.Rows[iRowIndex][S_PREFIX + iIndex.ToString()] = "<span style='color:purple'>" + "P / " + S_LATE_MARK + "</span>";
                                        else
                                            oDt.Rows[iRowIndex][S_PREFIX + iIndex.ToString()] = "P";
                                    }
                                }

                                DataRow drHour = oDt.NewRow();
                                drHour[S_NAME] = "Hour Details";
                                for (int iIndex = 1; iIndex <= NoOfDays; iIndex++)
                                {
                                    if (!lstLeaves.Any(lv => lv.Day == iIndex))
                                    {
                                        drHour[S_PREFIX + iIndex.ToString()] = 8; //Settings.FullWorkingHours;
                                        miUsersTotalWorkingHours += 8;
                                    }
                                    else if (lstLeaves.Any(lv => lv.Day == iIndex && lv.IsHalfLeave && lv.PartialLeaveId == 0))
                                    {
                                        drHour[S_PREFIX + iIndex.ToString()] = Settings.HalfWorkingHours;
                                        miUsersTotalWorkingHours += Settings.HalfWorkingHours;
                                    }
                                    else
                                        drHour[S_PREFIX + iIndex.ToString()] = Constants.S_ZERO;
                                }

                                AddLeaveTotal(drHour, lstLeaves);

                                SetHolidays(drHour, lstLeaves, oDt.Rows[iRowIndex]);
                                SetWeekend(drHour, lstLeaves, oDt.Rows[iRowIndex]);

                                drHour[S_TOTAL_HOURS] = miUsersTotalWorkingHours;
                                
                                drHour[S_PRESENT_DAYS] = (decimal)miUsersTotalWorkingHours / 8;
                                drHour[S_AVERAGE] = Math.Round(miUsersTotalWorkingHours / (drHour[S_PRESENT_DAYS].ToDecimal() + drHour[S_TOTAL_LEAVES].ToDecimal()), 2);

                                SetLateMarkDetails(drHour, userId);

                                oDt.Rows.Add(drHour);
                                iRowIndex = iRowIndex + 2;


                            }
                            else
                                iRowIndex++;

                            miTotalWorkingHours += miUsersTotalWorkingHours;

                            miUsersTotalWorkingHours = 0;
                        }

                    );

                AddEmptyRow(oDt);

                iRowIndex++;

        AddColumnTotal(lstUserLeaveDetails, oDt, abIsMisReport);

        AddFinalTotal(lstUserLeaveDetails, oDt, iRowIndex, abIsMisReport);

        ExportToExcel(GetFileName(), oDt);
    }

    /// <summary>
    /// This method is sued to set late mark details.
    /// </summary>
    /// <param name="adrHour"></param>
    /// <param name="aiUserId"></param>
    private void SetLateMarkDetails(DataRow adrHour, int aiUserId)
    {
        var oUser = moStaffLeaveDetailsBL.DatewiseStaffLeaves.Where(dsl => dsl.UserId == aiUserId);
        if (oUser != null && oUser.Count() > 0)
        {
            adrHour[S_TOTAL_LATE_MARK_COUNT] = oUser.Count();
            adrHour[S_LEAVE_DEDUCTED_FOR_LATE_MARK] = oUser.FirstOrDefault().LateMarkLeaveCount;

            adrHour[S_TOTAL_LEAVES] = oUser.FirstOrDefault().LateMarkLeaveCount + adrHour[S_TOTAL_LEAVES].ToDecimal();
        }
    }

    /// <summary>
    /// This method is used to set weekend.
    /// </summary>
    /// <param name="adrHour"></param>
    /// <param name="alstLeaves"></param>
    /// <param name="adrWeekend"></param>
    private void SetWeekend(DataRow adrHour, List<UserLeaveDetails> alstLeaves, DataRow adrWeekend)
    {
        for (int iDay = 1; iDay <= NoOfDays; iDay++)
        {
            if (!alstLeaves.Any(lv => lv.Day == iDay))
            {
                DayOfWeek oDayOfWeek = (new DateTime(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt(), iDay).DayOfWeek);
                int iWeekDay = ((int)oDayOfWeek);

                if (iWeekDay == 0)
                    iWeekDay = 7;

                if (moStaffLeaveDetailsBL.WeekendDays.Contains(iWeekDay))
                {
                    if (adrWeekend[S_PREFIX + iDay].ToString() == "P")
                        miUsersTotalWorkingHours = miUsersTotalWorkingHours - 8; //Settings.FullWorkingHours;

                    adrWeekend[S_PREFIX + iDay] = "<span style='color:blue'>" + oDayOfWeek.ToString() + "</span>";
                    adrHour[S_PREFIX + iDay] = "<span style='color:blue'>" + Constants.S_ZERO + "</span>";
                }
            }
        }
    }

    /// <summary>
    /// THis method is used to set holiday details.
    /// </summary>
    /// <param name="adrHour"></param>
    /// <param name="alstLeaves"></param>
    /// <param name="adrHoliday"></param>
    private void SetHolidays(DataRow adrHour, List<UserLeaveDetails> alstLeaves, DataRow adrHoliday)
    {
        moStaffLeaveDetailsBL.Holidays.ForEach(
            holiday =>
            {
                // If holiday start Date & holiday end date is in same Month(Same Year).
                if (holiday.StatDate.Day <= holiday.EndDate.Day && holiday.StatDate.Month == holiday.EndDate.Month)
                {
                    for (int iDay = holiday.StatDate.Day; iDay <= holiday.EndDate.Day; iDay++)
                    {
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }

                // If Hoiday starts in Current month & end in next Month in Same Year.
                else if (holiday.StatDate.Month < holiday.EndDate.Month && cmbMonth.SelectedValue.ToInt() == holiday.StatDate.Month)
                {
                    for (int iDay = holiday.StatDate.Day; iDay <= NoOfDays; iDay++)
                    {
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }

                //If Hoiday starts in Previous month & end in Current Month in Same Year.
                else if (holiday.StatDate.Month < holiday.EndDate.Month && cmbMonth.SelectedValue.ToInt() == holiday.EndDate.Month)
                {
                    for (int iDay = 1; iDay <= holiday.StatDate.Day; iDay++)
                    {
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }

                // If Hoiday starts in Previous month & end in next Month in Same Year.
                else if (holiday.StatDate.Month < holiday.EndDate.Month && cmbMonth.SelectedValue.ToInt() != holiday.EndDate.Month && cmbMonth.SelectedValue.ToInt() != holiday.StatDate.Month)
                {
                    for (int iDay = 1; iDay <= holiday.StatDate.Day; iDay++)
                    {
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }
                // If Hoiday starts in current month  & end in next Month in next Year.
                else if (holiday.StatDate.Month == cmbMonth.SelectedValue.ToInt() && holiday.EndDate.Year == cmbYear.SelectedValue.ToInt() + 1)
                {
                    for (int iDay = holiday.StatDate.Day; iDay <= NoOfDays; iDay++ )
                    {                        
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }
                // If Hoiday starts in previous month (previous year) & end in current Month.
                else if (holiday.EndDate.Month == cmbMonth.SelectedValue.ToInt() && holiday.StatDate.Year == cmbYear.SelectedValue.ToInt() - 1)
                {
                    for (int iDay = 1; iDay <= holiday.EndDate.Day; iDay++)
                    {
                        SetHolidayHours(adrHour, alstLeaves, adrHoliday, holiday, iDay);
                    }
                }
            }
            );
    }

    /// <summary>
    /// This method is used to set holiday hours.
    /// </summary>
    /// <param name="adrHour"></param>
    /// <param name="alstLeaves"></param>
    /// <param name="adrHoliday"></param>
    /// <param name="oHoliday"></param>
    /// <param name="aiDay"></param>
    private void SetHolidayHours(DataRow adrHour, List<UserLeaveDetails> alstLeaves, DataRow adrHoliday, HolidayMaster oHoliday, int aiDay)
    {
        if (!alstLeaves.Any(lv => lv.Day == aiDay))
        {
            if (adrHoliday[S_PREFIX + aiDay].ToString() == "P")
                miUsersTotalWorkingHours = miUsersTotalWorkingHours - 8; // Settings.FullWorkingHours;
            
            adrHoliday[S_PREFIX + aiDay] = "<span style='color:maroon'>" + oHoliday.HolidayName + "</span>";
            adrHour[S_PREFIX + aiDay] = "<span style='color:maroon'>" + Constants.S_ZERO + "</span>";
        }
    }

    /// <summary>
    /// This method is used to set leave total.
    /// </summary>
    /// <param name="aoDR"></param>
    /// <param name="alstLeaves"></param>
    private void AddLeaveTotal(DataRow aoDR, List<UserLeaveDetails> alstLeaves)
    {
        var oLeaves = (from lv in moStaffLeaveDetailsBL.ConfiguredLeaves
                       join uld in alstLeaves
                       on lv.LeaveId equals uld.LeaveId
                       select lv).Distinct().ToList();

        oLeaves.ForEach(
            clv =>
            {
                decimal dcTotalFullLeave = alstLeaves.Where(lv => lv.IsHalfLeave == false && clv.LeaveId == lv.LeaveId).Count();
                decimal dcTotalHalfLeave = alstLeaves.Where(lv => lv.IsHalfLeave && clv.LeaveId == lv.LeaveId).Count();
                decimal dcTotalPartialLeave = alstLeaves.Where(lv => lv.PartialLeaveId != 0 && clv.LeaveId == lv.LeaveId).Count();

                aoDR[clv.ShortName] = dcTotalFullLeave + (dcTotalHalfLeave * (decimal)0.5) + (dcTotalPartialLeave * (decimal)0.5);
            }
            );

        moStaffLeaveDetailsBL.ConfiguredLeaves.ForEach(
            clv =>
            {
                if (!alstLeaves.Any(lv => lv.LeaveId == clv.LeaveId))
                    aoDR[clv.ShortName] = Constants.S_ZERO;
            }
            );

        decimal dcTotalFullLeave1 = alstLeaves.Where(lv => lv.IsHalfLeave == false).Count();
        decimal dcTotalHalfLeave1 = alstLeaves.Where(lv => lv.IsHalfLeave).Count();
        decimal dcTotalPartialLeave1 = alstLeaves.Where(lv => lv.PartialLeaveId != 0).Count();

        decimal dcTotal = dcTotalFullLeave1 + (dcTotalHalfLeave1 * (decimal)0.5) + (dcTotalPartialLeave1 * (decimal)0.5);
        aoDR[S_TOTAL_LEAVES] = dcTotal;
    }

    /// <summary>
    /// This method is used to export staff leaves according to selected staff group and date range.
    /// </summary>
    private void ExportStaffLeaves()
    {
        moStaffLeavesBL = new StaffLeaveDetailsBL(miSchoolId);
        List<DateWiseStaffLeaves> lstLeaves = moStaffLeavesBL.GetStaffwiseLeaves(cmbStaff.SelectedValue.ToInt(), cmbGroups.SelectedValue.ToInt(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime());
        
        DataTable dtLeaves = new DataTable();
        dtLeaves.AddColumns(new string[] { S_SR_NO, S_NAME, S_START_DATE, S_END_DATE });
        GenerateColumns(dtLeaves);
        FillUsers(dtLeaves, lstLeaves);

        ExportToExcel("StaffLeaves.xls", dtLeaves);
    }

    /// <summary>
    /// This method is used to add user leave details.
    /// </summary>
    /// <param name="adtLeaves"></param>
    /// <param name="alstLeaves"></param>
    private void FillUsers(DataTable adtLeaves, List<DateWiseStaffLeaves> alstLeaves)
    {
        int iSrNo = 1;
        moStaffLeavesBL.UserDetails.OrderBy(usr => usr.SrNo).ToList().ForEach
            (
             usr =>
             {
                 if (alstLeaves.Any(lv => lv.DateWiseStaffUserId == usr.UserId))
                 {
                     var oUserLeaves = alstLeaves.Where(lv => lv.DateWiseStaffUserId == usr.UserId).ToList();

                     DateTime dtStartDate = oUserLeaves.Min(lv => lv.LeaveDate);
                     DateTime dtEndDate = oUserLeaves.Max(lv => lv.LeaveDate);

                     int iLeaveYearId = moStaffLeavesBL.LeaveYears.Where(ly => ly.StartDate <= dtStartDate && dtStartDate <= ly.EndDate).FirstOrDefault().Id;

                     while (dtStartDate <= dtEndDate)
                     {
                         if (oUserLeaves.Any(lv => lv.LeaveDate == dtStartDate))
                         {
                             Dictionary<int, double> dictLeaves = new Dictionary<int, double>();
                             DataRow dr = adtLeaves.NewRow();
                             dr[S_SR_NO] = iSrNo++;
                             dr[S_NAME] = usr.StaffName;

                             DateTime dtNextDate = dtStartDate;
                             while (oUserLeaves.Any(lv => lv.LeaveDate == dtNextDate))
                             {
                                 var oLeave = oUserLeaves.Where(lv => lv.LeaveDate == dtNextDate).FirstOrDefault();

                                 if (oLeave != null)
                                 {
                                     double iLeaveCount = (oLeave.IsHalfLeave ? 0.5 : 1);
                                     dictLeaves[oLeave.LeaveId] = (dictLeaves.Keys.Contains(oLeave.LeaveId) ? dictLeaves[oLeave.LeaveId] + iLeaveCount : iLeaveCount);

                                     var oPartialLeave = oUserLeaves.Where(lv => lv.LeaveDate == dtNextDate && lv.LeaveId != oLeave.LeaveId && lv.IsPartialLeave).FirstOrDefault();
                                     if (oPartialLeave != null)
                                         dictLeaves[oPartialLeave.LeaveId] = (dictLeaves.Keys.Contains(oPartialLeave.LeaveId) ? dictLeaves[oPartialLeave.LeaveId] + 0.5 : 0.5);

                                     dtNextDate = dtNextDate.AddDays(1);
                                 }
                             }

                             moStaffLeavesBL.ConfiguredLeaves.ForEach
                                 (
                                    lv =>
                                    {
                                        if (dictLeaves.Keys.Contains(lv.LeaveId))
                                            dr[lv.ShortName] = dictLeaves[lv.LeaveId];
                                    }
                                 );


                             dtNextDate = dtNextDate.AddDays(-1);

                             dr[S_START_DATE] = dtStartDate.ToString(Constants.S_DATE_FORMAT);
                             dr[S_END_DATE] = dtNextDate.ToString(Constants.S_DATE_FORMAT);

                             adtLeaves.Rows.Add(dr);

                             dtStartDate = dtNextDate;
                             dictLeaves.Clear();
                         }

                         dtStartDate = dtStartDate.AddDays(1);

                         int iNewLeaveYearId = moStaffLeavesBL.LeaveYears.Where(ly => ly.StartDate <= dtStartDate && dtStartDate <= ly.EndDate).FirstOrDefault().Id;

                         if (iLeaveYearId != iNewLeaveYearId)
                         {
                             AddLateMarkLeaveCount(adtLeaves, usr.UserId, iLeaveYearId);

                             AddBlankRow(adtLeaves);
                             AddTotalRow(adtLeaves, oUserLeaves, iLeaveYearId, moStaffLeavesBL.UserLateMarks, usr.UserId);
                             AddLeaveBalance(adtLeaves, usr.UserId, iLeaveYearId);
                             iLeaveYearId = iNewLeaveYearId;
                         }
                     }

                     AddLateMarkLeaveCount(adtLeaves, usr.UserId, iLeaveYearId);
                     AddBlankRow(adtLeaves);
                     AddTotalRow(adtLeaves, oUserLeaves, iLeaveYearId, moStaffLeavesBL.UserLateMarks, usr.UserId);
                     AddLeaveBalance(adtLeaves, usr.UserId, iLeaveYearId);

                     AddBlankRow(adtLeaves);
                 }
             }
            );
    }

    /// <summary>
    /// This method is sued to add late mark leave count.
    /// </summary>
    /// <param name="adtLeaves"></param>
    /// <param name="aiUserId"></param>
    /// <param name="iLeaveYearId"></param>
    private void AddLateMarkLeaveCount(DataTable adtLeaves, int aiUserId, int iLeaveYearId)
    {
        var oLateMarks = moStaffLeavesBL.UserLateMarks.Where(s => s.UserId == aiUserId && s.Year == iLeaveYearId).ToList();
        if (oLateMarks != null && oLateMarks.Count > 0)
        {
            DataRow drLateMark = adtLeaves.NewRow();
            drLateMark[S_SR_NO] = string.Empty;
            drLateMark[S_NAME] = "Leave(s) deduced for late mark";

            moStaffLeavesBL.ConfiguredLeaves.ForEach
           (
              lv =>
              {
                  var oLateMark = moStaffLeavesBL.UserLateMarks.Where(s => s.UserId == aiUserId && s.LeaveId == lv.LeaveId && s.Year == iLeaveYearId).FirstOrDefault();
                  if (oLateMark != null)
                      drLateMark[lv.ShortName] = oLateMark.Days;
              }
           );

            adtLeaves.Rows.Add(drLateMark);
        }
    }

    /// <summary>
    /// This method is used to add leave balance row.
    /// </summary>
    /// <param name="adtLeaves"></param>
    /// <param name="aiUserId"></param>
    /// <param name="adtStartDate"></param>
    private void AddLeaveBalance(DataTable adtLeaves, int aiUserId, int aiLeaveYearId)
    {
        DataRow dr1 = adtLeaves.NewRow();

        dr1[S_NAME] = "<span style='font-weight:Bold;color:Navy'>" + "Leave Balance" + "</span>";
        moStaffLeavesBL.LeaveBalanceDetails.Where(lb => lb.UserId == aiUserId && lb.LeaveYear == aiLeaveYearId).ToList().ForEach(
               lbd =>
               {
                   string sShortName = moStaffLeavesBL.ConfiguredLeaves.Where(lv => lv.LeaveId == lbd.LeaveId).Select(lv => lv.ShortName).FirstOrDefault();
                   dr1[sShortName] = "<span style='font-weight:Bold;color:Navy'>" + (lbd.LeaveBalance < 0 ? (decimal)0 : lbd.LeaveBalance) + "</span>";
               }
            );

        adtLeaves.Rows.Add(dr1);
    }

    /// <summary>
    /// This method is used to add total leaves row.
    /// </summary>
    /// <param name="adtLeaves"></param>
    /// <param name="alstUserLeaves"></param>
    /// <param name="adtStartDate"></param>
    private void AddTotalRow(DataTable adtLeaves, List<DateWiseStaffLeaves> alstUserLeaves, int aiLeaveYearId, List<UserLateMarkLeave> alstoUserLateMarkLeave, int aiUserId)
    {
        var oUserLateMarks = alstoUserLateMarkLeave.Where(um => um.UserId == aiUserId);
        var oLeaveYear = moStaffLeavesBL.LeaveYears.Where(lv => lv.Id == aiLeaveYearId).FirstOrDefault();
        DataRow dr1 = adtLeaves.NewRow();
        dr1[S_NAME] = "<span style='font-weight:Bold;color:maroon'>" + "Total Leaves" + "</span>";
        moStaffLeavesBL.ConfiguredLeaves.ForEach
            (
               lv =>
               {   
                   decimal iHalfLeaveCount = alstUserLeaves.Where(ul => ul.LeaveId == lv.LeaveId && ul.IsHalfLeave && ul.LeaveDate.IsBetween(oLeaveYear.StartDate,oLeaveYear.EndDate)).Count() * (0.5).ToDecimal();
                   decimal iFullLeaveCount = alstUserLeaves.Where(ul => ul.LeaveId == lv.LeaveId && !ul.IsHalfLeave && ul.LeaveDate.IsBetween(oLeaveYear.StartDate, oLeaveYear.EndDate)).Count();
                   
                   decimal dbLateMarkCount = 0;

                   if (oUserLateMarks.Count() > 0)
                   {
                       if (oUserLateMarks.Any(ul => ul.LeaveId == lv.LeaveId && ul.Year == aiLeaveYearId))
                           dbLateMarkCount = oUserLateMarks.Where(ul => ul.LeaveId == lv.LeaveId && ul.Year == aiLeaveYearId).FirstOrDefault().Days;
                   }

                   dr1[lv.ShortName] = "<span style='font-weight:Bold;color:maroon'>" + (iHalfLeaveCount + iFullLeaveCount + dbLateMarkCount) + "</span>";
               }
            );

        adtLeaves.Rows.Add(dr1);
    }

    /// <summary>
    /// This method is used to add blank row.
    /// </summary>
    /// <param name="adtLeaves"></param>
    private void AddBlankRow(DataTable adtLeaves)
    {
        DataRow dr = adtLeaves.NewRow();
        adtLeaves.Rows.Add(dr);
    }

    /// <summary>
    /// This method is used to generate leave columns.
    /// </summary>
    /// <param name="adtLeaves"></param>
    private void GenerateColumns(DataTable adtLeaves)
    {
        moStaffLeavesBL.ConfiguredLeaves.OrderBy(lv => lv.OriginalLeaveId).ToList().ForEach
            (
                lv =>
                {
                    adtLeaves.Columns.Add(lv.ShortName);
                }
            );
    }

    /// <summary>
    /// This method is used to export staff attendance details.
    /// </summary>
    /// <param name="alstUserLeaveDetails"></param>
    private void ExportStaffAttendance(List<UserLeaveDetails> alstUserLeaveDetails)
    {
        SetBasicHTTPResponse();

        StringBuilder obj = new StringBuilder();
        StringBuilder oDays = new StringBuilder();
        obj.Append("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");


        AddHeaders(obj);
        AddColumns(obj, oDays);

        int iSrNo = 1;
        StringBuilder sb = new StringBuilder();

        moStaffLeaveDetailsBL.DaywiseStaffAttendances.OrderBy(usr => usr.SrNo).Select(uld => uld.UserId).Distinct().ToList().ForEach
        (
            userId =>
            {
                sb.Append("<TR>");
                var oUser = moStaffLeaveDetailsBL.DaywiseStaffAttendances.Where(usr => usr.UserId == userId).FirstOrDefault();
                if (oUser != null)
                {
                    sb.Append(AddNewCell(iSrNo.ToString(), "text-align:center"));
                    sb.Append(AddNewCell(oUser.EmployeeNo, "text-align:left;padding-left:5px"));
                    sb.Append(AddNewCell(oUser.Name, "padding-left:5px"));

                    string sData = string.Empty;
                    decimal dcPresentTotal = 0,
                            dcAbsentTotal = 0;
                    for (int iDay = 1; iDay <= NoOfDays; iDay++)
                    {
                        DateTime dt = new DateTime(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt(), iDay);

                        List<UserLeaveDetails> lstLeaves = alstUserLeaveDetails.Where(usr => usr.UserId == userId && usr.MonthId == cmbMonth.SelectedValue.ToInt()).ToList();
                        if (!lstLeaves.Any(lv => lv.Day == iDay && lv.LeaveId != 0))
                        {
                            //if (!alstUserLeaveDetails.Any(usr => usr.Day == iDay && usr.MonthId == cmbMonth.SelectedValue.ToInt() && usr.LeaveId != 0))
                            //{
                                if (moStaffLeaveDetailsBL.Holidays.Any(hd => dt.IsBetween(hd.StatDate, hd.EndDate)))
                                    sData = "W";

                                DayOfWeek oDayOfWeek = (new DateTime(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt(), iDay).DayOfWeek);
                                int iWeekDay = ((int)oDayOfWeek);

                                if (iWeekDay == 0)
                                    iWeekDay = 7;

                                if (moStaffLeaveDetailsBL.WeekendDays.Contains(iWeekDay))
                                    sData = "W";
                            //}

                            if (sData == string.Empty)
                            {
                                if (moStaffLeaveDetailsBL.IsAttendanceMarked)
                                {
                                    sData = "P";
                                    dcPresentTotal++;
                                }
                                else
                                    sData = "NA";
                            }
                        }
                        else
                        {
                            if (lstLeaves.Any(lv => lv.Day == iDay && lv.PartialLeaveId == 0 && lv.IsHalfLeave && lv.LeaveId != 0))
                            {
                                sData = "H";
                                dcAbsentTotal = dcAbsentTotal + Convert.ToDecimal(0.5);
                                dcPresentTotal = dcPresentTotal + Convert.ToDecimal(0.5);
                            }
                            else if (lstLeaves.Any(lv => lv.Day == iDay && lv.PartialLeaveId == 0 && !lv.IsHalfLeave && lv.LeaveId != 0) ||
                                lstLeaves.Any(lv => lv.Day == iDay && lv.PartialLeaveId != 0 && lv.IsHalfLeave && lv.LeaveId != 0))
                            {
                                sData = "A";
                                dcAbsentTotal++;
                            }
                        }

                        sb.Append(AddNewCell(sData, "text-align:center"));
                        sData = string.Empty;
                    }

                    sb.Append(AddNewCell(dcPresentTotal.ToString(), "text-align:center"));
                    sb.Append(AddNewCell(dcAbsentTotal.ToString(), "text-align:center"));

                    dcPresentTotal = 0;
                    dcAbsentTotal = 0;
                }

                sb.Append("</TR>");
                iSrNo++;
            }
        );

        AddLegend(sb);

        obj.Append(sb.ToString());
        obj.Append("</Table>");
        HttpContext.Current.Response.Write(obj.ToString());
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    /// <summary>
    /// This method is used to set basic http details.
    /// </summary>
    private static void SetBasicHTTPResponse()
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=StaffAttendance.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
    }

    private void AddLegend(StringBuilder aoStringBuilder)
    {
        aoStringBuilder.Append("<TR>");
        aoStringBuilder.Append(AddNewCell("Legend : W: Weekly Off / Holiday, H : Halfday, P : Present, A : Absent, NA : Not Available", string.Empty, "colspan='" + (NoOfDays + 5) + "'"));
        aoStringBuilder.Append("</TR>");
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aiStringBuilder"></param>
    /// <param name="aoDays"></param>
    private void AddColumns(StringBuilder aiStringBuilder, StringBuilder aoDays)
    {
        aiStringBuilder.Append("<TR>");

        aoDays.Append("<TR>");

        aiStringBuilder.Append(AddNewCell("Sr. No."));
        aiStringBuilder.Append(AddNewCell("Emp. No.","padding-left:5px"));
        aiStringBuilder.Append(AddNewCell("Employee Name", "padding-left:5px"));

        aoDays.Append(AddNewCell(string.Empty));
        aoDays.Append(AddNewCell(string.Empty));
        aoDays.Append(AddNewCell(string.Empty));

        for (int iDay = 1; iDay <= NoOfDays; iDay++)
        {
            aiStringBuilder.Append(AddNewCell(iDay.ToString(), "width:40px;text-align:center"));
            DateTime dt = new DateTime(cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt(), iDay);
            aoDays.Append(AddNewCell(dt.DayOfWeek.ToString().Substring(0, 3), "text-align:center"));
        }

        aiStringBuilder.Append(AddNewCell("Present", "text-align:center"));
        aiStringBuilder.Append(AddNewCell("Absent", "text-align:center"));

        aoDays.Append("</TR>");
        aiStringBuilder.Append("</TR>");
        aiStringBuilder.Append(aoDays.ToString());
    }

    /// <summary>
    /// This method is used to add headers
    /// </summary>
    /// <param name="aoStringBuilder"></param>
    private void AddHeaders(StringBuilder aoStringBuilder)
    {
        aoStringBuilder.Append("<TR>");
        aoStringBuilder.Append(AddNewCell(moStaffLeaveDetailsBL.SchoolName, "text-align:center;font-weight:bold;font-size:20px", "colspan='" + (NoOfDays + 5) + "'"));
        aoStringBuilder.Append("</TR>");

        aoStringBuilder.Append("<TR>");
        aoStringBuilder.Append(AddNewCell("Staff Attendance Report", "text-align:center;font-size:18px", "colspan='" + (NoOfDays + 5) + "'"));
        aoStringBuilder.Append("</TR>");

        aoStringBuilder.Append("<TR>");
        aoStringBuilder.Append(AddNewCell("Month : " + cmbMonth.SelectedItem.Text + " - " + cmbYear.SelectedItem.Text, string.Empty, "colspan='" + (NoOfDays + 5) + "'"));
        aoStringBuilder.Append("</TR>");

        aoStringBuilder.Append("<TR>");
        aoStringBuilder.Append(AddNewCell("Department : " + cmbStaffGroups.SelectedItem.Text, string.Empty, "colspan='" + (NoOfDays + 5) + "'"));
        aoStringBuilder.Append("</TR>");
    }

    /// <summary>
    /// This method is used to add new cell.
    /// </summary>
    /// <param name="asData"></param>
    /// <param name="asStyle"></param>
    /// <param name="astdStyle"></param>
    /// <returns></returns>
    private string AddNewCell(string asData, string asStyle="", string astdStyle = "")
    {
        StringBuilder obj = new StringBuilder();
        obj.Append("<TD " + astdStyle + " style='" + asStyle + "'>");
        obj.Append(asData);
        obj.Append("</TD>");
        return obj.ToString();
    }

    #endregion
}