/*
 *  File Name :- DatewiseStaffLeavesPopup.aspx.cs
 *  Created By :- Sachin
 *  Created Date :-30-August-2010
 *  Class Description :- this class is used to display / save date wise leaves details as well as late mark details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Diagnostics;

public partial class DatewiseStaffLeavesPopup : SchoolBase
{
    #region Constants

    private const string S_LEAVE_TABLE = "Leave_Table";
    private const string S_LATE_MARK_CONFIG = "LateMarkConfiguration";
    private const string S_LATE_MARK_LEAVES = "LateMarkLeaves";
    private const string S_CONFIGURED_LEAVE_TYPE = "ConfiguredLeaveType";
    private const string S_JOINING_DATES = "Users_Joining_Dates";
    private const string S_CONFIGURED_LEAVE = "hidPartialLeaves";
    private const string S_PARTIAL_LEAVES = "PartialLEaves";

    #endregion

    #region Data Member

    private List<UsedLeaves> mlstLeaveTypeCount;
    private List<DaywiseLeaves> molstDaywiseLeaves;
    private DatewiseStaffLeavesBL moDatewiseStaffLeavesBL;
    private Dictionary<int, int> moLeaveDictionary;
    private int miOldHolidatId = 0;
    private List<StaffHoliday> mlstHolidayStartingDays = new List<StaffHoliday>();
    
    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill user combobox and set selected months leaves.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {  
            if (!IsPostBack)
            {
                ReadQuerystring();
                SetCalendarColumns();
                FillUserCombobox();
                SetJavascriptAttributes();
                DisplayPullBiometricData();

                // set leaves if at least one user is exists.
                if (cmbUsers.Items.Count > 0)
                {
                    GetStaffLeavesDetails(false);
                    cmbUsers.Focus();
                }
                SetQueryStringForOD();
            }
            miOldHolidatId = 0;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display leaves details as per selected month.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LeaveCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            hidPartialLeaves.Value = string.Empty;
            hidDeductHolidayLeaves.Value = Constants.S_NO;
            hidEnclosedDates.Value = string.Empty;
            hidAttEncConfig.Value = string.Empty;
            hidAttachedDates.Value = string.Empty;
            hidMonthId.Value = LeaveCalendar.VisibleDate.Month.ToString();
            hidYear.Value = LeaveCalendar.VisibleDate.Year.ToString();
            hidSelectedUserId.Value = cmbUsers.SelectedValue;

            FillUserCombobox();
            GetStaffLeavesDetails(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save leave details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidateLeaveBalance() && ValidateJoiningDate())
            {
                decimal dcLeaves = CalculateLateMarkLeaves();
                Dictionary<int, decimal> dictLateMarkLeaves = ApplyLateMarkLeaves(dcLeaves);
                DatewiseStaffLeavesBL oDatewiseStaffLeavesBL = new DatewiseStaffLeavesBL();
                DatewiseStaffLeave oDatewiseStaffLeave = new DatewiseStaffLeave
                {
                    SchoolId = miSchoolId,
                    AcademicYearId = miAcademicYearId,
                    UserId = Convert.ToInt32(cmbUsers.SelectedValue),
                    InsertedById = miUserId,
                    LeaveXml = GenerateXml(),
                    LateMarkLeaveXml = GenerateLateMarkLeaveXML(dictLateMarkLeaves),
                    MonthId = LeaveCalendar.VisibleDate.Month,
                    Year = LeaveCalendar.VisibleDate.Year,
                    StaffHolidayLeaveConfigIds = GetStaffHolidaysSalaryConfigIds(),
                    ExcludeFromSalaryDeduction = hidDeductHolidayLeaves.Value == Constants.S_YES ? false : true,
                    Holidays = GetHolidayLeaveList()
                };

                oDatewiseStaffLeavesBL.DatewiseStaffLeaves = oDatewiseStaffLeave;
                oDatewiseStaffLeavesBL.Insert();
                hidPartialLeaves.Value = string.Empty;
                hidEnclosedDates.Value = string.Empty;
                hidAttEncConfig.Value = string.Empty;
                hidAttachedDates.Value = string.Empty;

                //Refill leave details.
                GetStaffLeavesDetails(false);
                lblSuccessMessage.ForeColor = System.Drawing.Color.Blue;
                lblSuccessMessage.Text = "Leaves has been saved successfully !!!";
                hidUsedLeaveBkp.Value = Constants.S_YES;
            }
            else
                SetConfiguredLeaves(null);
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
            UpdateCalenderEventSource(molstDaywiseLeaves);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidPartialLeaves.Value = string.Empty;
            hidSelectedUserId.Value = Constants.S_ZERO;
            GetStaffLeavesDetails(true);
            SetQueryStringForOD();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close popup and refresh parent screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidClosePopup_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display leaves details as per selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidPartialLeaves.Value = string.Empty;
            hidEnclosedDates.Value = string.Empty;
            hidAttEncConfig.Value = string.Empty;
            hidAttachedDates.Value = string.Empty;
            hidDeductHolidayLeaves.Value = Constants.S_NO;
            GetStaffLeavesDetails(false);
            SetQueryStringForOD();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Search user name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            UserDetailsForLeave oUserDetailsForLeave = new UserDetailsForLeave();
            DatewiseStaffLeavesBL oDatewiseStaffLeavesBL = new DatewiseStaffLeavesBL();
            oUserDetailsForLeave = oDatewiseStaffLeavesBL.GetUserDetailsForLeave(txtName.Text.Trim(), miSchoolId);
            if (oUserDetailsForLeave.UserId != Constants.I_ZERO && oUserDetailsForLeave.StaffGroupsId != Constants.I_ZERO)
            {

                cmbStaffGroup.SelectedValue = oUserDetailsForLeave.StaffGroupsId.ToString();
                cmbStaffGroup_SelectedIndexChanged(cmbStaffGroup, null);

                cmbUsers.SelectedValue = oUserDetailsForLeave.UserId.ToString();
                cmbUsers_SelectedIndexChanged(cmbUsers, null);
            }
            else
                lblNoRecordMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Get attendance marked for staff through utility.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPullBiometricData_Click(object sender, EventArgs e)
    {
        try
        {
            //string filename = SchoolBase.Settings.BiometricUtilityPath;

            //ProcessStartInfo startproc = new ProcessStartInfo();
            //startproc.Arguments = miSchoolId.ToString();
            //startproc.FileName = filename;
            //startproc.WindowStyle = ProcessWindowStyle.Hidden;
            //startproc.CreateNoWindow = true;
            //int exitCode;

            //using (Process proc = Process.Start(startproc))
            //{
            //    TimeSpan runningTime = DateTime.Now - proc.StartTime;
            //    if (runningTime.TotalMinutes > 2)
            //        proc.Kill();

            //    proc.WaitForExit();
            //    exitCode = proc.ExitCode;
            //    cmbUsers_SelectedIndexChanged(null, null);
            //}

            //Response.Redirect(SchoolBase.Settings.BiometricUtilityPath+"?" + CommonUtility.EncryptQuerystring("CurrentDate=" + DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT) + "&SchoolId=" + miSchoolId));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// This method is used to validate joining as well as resign date.
    /// </summary>
    /// <returns></returns>
    private bool ValidateJoiningDate()
    {
        string sMessage = string.Empty;
        StaffBaseDetails oStaffBaseDetails = null;
        if (ViewState[S_JOINING_DATES] != null)
            oStaffBaseDetails = ViewState[S_JOINING_DATES] as StaffBaseDetails;

        if (oStaffBaseDetails != null)
        {
            string[] sArrDays = hidDay.Value.Split('$');
            string[] sArrLateMarks = hidLateMarkDays.Value.Split('$');

            string[] sDays = sArrDays.Where(day => day != string.Empty).Select(day => day).ToArray().Union(sArrLateMarks.Where(day => day != string.Empty).Select(day => day).ToArray()).ToArray();

            if (sDays.Length != 0 && !(sDays.Length == 1 && sDays[0] == string.Empty))
            {
                int iJoiningDateRecCount = 0;
                int iResignDateRecCount = 0;

                if (oStaffBaseDetails.JoiningDate != DateTime.MinValue)
                    iJoiningDateRecCount = sDays.Where(day => new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), Convert.ToInt32(day)) < oStaffBaseDetails.JoiningDate.Date).Count();

                if (oStaffBaseDetails.ResignDate != DateTime.MinValue)
                    iResignDateRecCount = sDays.Where(day => new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), Convert.ToInt32(day)) > oStaffBaseDetails.ResignDate.Date).Count();

                if (iJoiningDateRecCount > 0 && iResignDateRecCount > 0)
                    sMessage = "Leave(s) or Late Mark(s) should not be allowed before joining date and after the resign date.";
                else
                {
                    if (iJoiningDateRecCount > 0)
                        sMessage = "Leave(s) or Late Mark(s) should not be allowed before joining date.";
                    else if (iResignDateRecCount > 0)
                        sMessage = "Leave(s) or Late Mark(s) should not be allowed after the resign date.";
                }
            }
            else
            {
                DateTime dtStartDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), 1);
                DateTime dtEndDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), DateTime.DaysInMonth(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value)));

                if (oStaffBaseDetails.JoiningDate.Date != DateTime.MinValue && dtStartDate < oStaffBaseDetails.JoiningDate.Date)
                {
                    if (!(oStaffBaseDetails.JoiningDate.Date.IsBetween(dtStartDate, dtEndDate) && oStaffBaseDetails.JoiningDate.Date.Day != 1))
                        sMessage = "Full attendance will not be allowed before joining date.";
                }
                else if (oStaffBaseDetails.ResignDate.Date != DateTime.MinValue && dtEndDate > oStaffBaseDetails.ResignDate.Date)
                {
                    if (!(oStaffBaseDetails.ResignDate.Date.IsBetween(dtStartDate, dtEndDate) && oStaffBaseDetails.ResignDate.Date.Day != DateTime.DaysInMonth(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value))))
                        sMessage = "Full attendance will not be allowed after resignation date.";
                }
                else
                    sMessage = string.Empty;
            }
        }

        if (sMessage != string.Empty)
        {
            lblSuccessMessage.ForeColor = Color.Red;
            lblSuccessMessage.Text = sMessage;
            hidUsedLeaveBkp.Value = Constants.S_NO;
            UpdateCalenderEventSource(molstDaywiseLeaves);
            return false;
        }

        return true;
    }

    /// <summary>
    /// This method is used to return holiday leaves.
    /// </summary>
    /// <returns></returns>
    private string GetHolidayLeaveList()
    {
        string sConfigIds = string.Empty;
        string[] sArrDays = hidDay.Value.Split('$');
        string[] sArrLateMarks = hidLateMarkDays.Value.Split('$');

        if (hidHolidays.Value.StartsWith("$"))
            hidHolidays.Value = hidHolidays.Value.Substring(1);

        string[] sArrHolidays = hidHolidays.Value.Split('$');

        string[] dayList = sArrHolidays.Join(sArrDays, holiday => holiday, day => day, (holiday, day) => holiday).ToArray();
        string[] lateMarkList = sArrHolidays.Join(sArrLateMarks, holiday => holiday, lateMark => lateMark, (holiday, lateMark) => holiday).ToArray();

        string[] resultList = dayList.Union(lateMarkList).ToArray();

        if (resultList.Count() > 0)
            sConfigIds = string.Join(",", resultList);

        return sConfigIds;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!CalculateLeaves()) return false;");
        cmbAllLeaves.Attributes.Add("onchange", "SelectAll()");
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
        lblSuccessMessage.Text = string.Empty;
        hidDeductHolidayLeaves.Value = Constants.S_NO;
        lnkODDetails.Attributes.Add("onclick", "if(!OpenODDetailsPopup()) return false;");
        lnkUserInfo.Attributes.Add("onclick","ShowInfo(); return false;");

        hidBioData.Value = SchoolBase.Settings.BiometricUtilityPath + "?" + CommonUtility.EncryptQuerystring("CurrentDate=" + DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT) + "&SchoolId=" + miSchoolId);
        btnPullBioData.Attributes.Add("onclick","UpdateBioData(); return false;");
    }

    /// <summary>
    /// This method is sued to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = string.Format("MonthId={0}&Year={1}&StaffGroupId={2}&Filter={3}", hidMonthId.Value, hidYear.Value, hidStaffGroup.Value, hidFilter.Value);
        sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = string.Format("'?{0}'", sQuerystring);
        Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sQuerystring));
    }

    /// <summary>
    /// This method is used to return staff holidays salary coniguration ids.
    /// </summary>
    /// <returns></returns>
    private string GetStaffHolidaysSalaryConfigIds()
    {
        string sConfigIds = string.Empty;

        string[] sArrDays = hidDay.Value.Split('$');
        string[] sArrLeaves = hidLeave.Value.Split('$');
        string[] sArrHalfLeaves = hidHalfLeaves.Value.Split('$');

        string[] sArrHolidays1 = hidHolidayDates.Value.Split(',');
        string[] sArrConfigIds1 = hidConfigIds.Value.Split(',');
        string[] sArrConfAttEncDays = hidAttEncConfig.Value.Split(',');

        List<string> sArrHolidays = new List<string>();
        List<string> sArrConfigIds = new List<string>();
        
        if (sArrConfAttEncDays.Length > 0 && sArrConfAttEncDays[0] != string.Empty)
        {
            for (int iId = 0; iId < sArrHolidays1.Length; iId++)
            {
                if (sArrConfAttEncDays.Contains(sArrHolidays1[iId]))
                {
                    sArrHolidays.Add(sArrHolidays1[iId]);
                    sArrConfigIds.Add(sArrConfigIds1[iId]);
                }
            }
        }
      
        Dictionary<int, int> partialLeaveDictionary = new Dictionary<int, int>();
        if (ViewState[S_PARTIAL_LEAVES] != null)
            partialLeaveDictionary = ViewState[S_PARTIAL_LEAVES] as Dictionary<int, int>;

        Dictionary<string, string> dictHolidays = new Dictionary<string, string>();
        for (int iDay = 0; iDay < sArrDays.Length; iDay++)
        {
            if ((!sArrHalfLeaves.Contains(sArrDays[iDay]) || (sArrDays.Length > 0 && sArrDays[0] != string.Empty && sArrHalfLeaves.Contains(sArrDays[iDay]) && partialLeaveDictionary.ContainsKey(Convert.ToInt32(sArrDays[iDay])))) && sArrHolidays.Contains(sArrDays[iDay]))
                dictHolidays.Add(sArrDays[iDay], sArrLeaves[iDay]);
        }

        List<DaywiseLeaves> lstConfiguredLeaveTypes = null;
        if (ViewState[S_CONFIGURED_LEAVE_TYPE] != null)
            lstConfiguredLeaveTypes = (List<DaywiseLeaves>)ViewState[S_CONFIGURED_LEAVE_TYPE];

        if (lstConfiguredLeaveTypes != null)
        {
            List<string> lstHolidays = dictHolidays.Join(lstConfiguredLeaveTypes, holiday => Convert.ToInt32(holiday.Value), configuredLeave => configuredLeave.LeaveId,
                                                         (holiday, configuredLeave) => new { Day = holiday.Key, configuredLeave.ExcludeFromSalaryDeduction })
                .Where(holiday => holiday.ExcludeFromSalaryDeduction == false)
                .Select(holiday => holiday.Day)
                .ToList();

            List<StaffHoliday> lstDays = new List<StaffHoliday>();
            for (int iIndex = 0; iIndex < sArrHolidays.Count; iIndex++)
                lstDays.Add(new StaffHoliday { ConfigId = sArrConfigIds[iIndex].ToInt(), Day = sArrHolidays[iIndex].ToInt() });

            List<int> lstHolidaysSalaryConfigIds = new List<int>();

            if (hidEnclosedDates.Value == string.Empty)
                lstHolidays.ForEach(holiday => lstHolidaysSalaryConfigIds.AddRange(lstDays.Where(dt => dt.Day == holiday.ToInt()).Select(dt => dt.ConfigId)));
            else
            {
                if (hidIsPreEnclosed.Value != Constants.S_ZERO)
                    lstDays.Add(new StaffHoliday { Day = 0, ConfigId = Convert.ToInt32(hidIsPreEnclosed.Value) });

                if (hidIsPostEnclosed.Value != Constants.S_ZERO)
                    lstDays.Add(new StaffHoliday { Day = 0, ConfigId = Convert.ToInt32(hidIsPostEnclosed.Value) });

                string[] sGroups = hidEnclosedDates.Value.Split('$');
                for (int iIndex = 0; iIndex < sGroups.Length; iIndex++)
                {
                    string[] sPairString = sGroups[iIndex].Replace("#","0").Split(',');
                    if ((lstDays.FindAll(str => str.Day == sPairString[0].ToInt() || str.Day == 0).Any())
                        && (lstDays.FindAll(str => str.Day == sPairString[1].ToInt() || str.Day == 0).Any()))
                    {
                        var iConfigId = (from d1 in lstDays
                                         join d2 in lstDays
                                         on d1.ConfigId equals d2.ConfigId
                                         where d1.Day == sPairString[0].ToInt()
                                         && d2.Day == sPairString[1].ToInt()
                                         select d1.ConfigId
                                        ).FirstOrDefault();
                        lstHolidaysSalaryConfigIds.Add(iConfigId);
                    }
                }
            }

            //lstHolidays.ForEach(holiday => lstHolidaysSalaryConfigIds.Add(sArrConfigIds[sArrHolidays.ToList().IndexOf(holiday)]));

            if (lstHolidaysSalaryConfigIds.Count > 0)
                sConfigIds = string.Join(",", lstHolidaysSalaryConfigIds.Distinct().ToArray());
        }

        return sConfigIds;
    }

    /// <summary>
    /// This method is used to generate late mark xml.
    /// </summary>
    /// <param name="aDictLateMarkLeaveDictionary"></param>
    /// <returns></returns>
    private string GenerateLateMarkLeaveXML(Dictionary<int, decimal> aoDictLateMarkLeaves)
    {
        string sXml = string.Empty;
        if (aoDictLateMarkLeaves.Count > 0)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("LateMarkLeave");
            XmlNode oXmlNode = doc.CreateNode("element", "LateMarkLeave", string.Empty);

            foreach (KeyValuePair<int, decimal> kvp in aoDictLateMarkLeaves)
            {
                XmlNode oNode = doc.CreateNode("element", "LateMarkLeave", string.Empty);

                XmlAttribute attr = doc.CreateAttribute("LeaveId");
                attr.Value = kvp.Key.ToString();
                oNode.Attributes.Append(attr);

                attr = doc.CreateAttribute("Days");
                attr.Value = kvp.Value.ToString();
                oNode.Attributes.Append(attr);

                oXmlNode.AppendChild(oNode);
            }

            root.AppendChild(oXmlNode);
            sXml = root.InnerXml;
        }

        return sXml;
    }

    /// <summary>
    /// This method is used to calculate late mark leaves.
    /// </summary>
    /// <returns></returns>
    private decimal CalculateLateMarkLeaves()
    {
        string[] sArrLateMarks = hidLateMarkDays.Value.Split('$');
        decimal dcTotalLateMarkLeaves = 0;
        if (sArrLateMarks.Length > 0 && sArrLateMarks[0] != string.Empty)
        {
            const string S_ZERO = "0";

            var oLateMarks = sArrLateMarks.Where(lateMark => lateMark != S_ZERO);
            int iTotalLateMarks = oLateMarks.Count();
            int iLastLateMarkCount = 0;
            decimal dcLastConsideredLeaves = 0;
            int iRemainingLateMarks = iTotalLateMarks;

            List<LateMarkConfiguration> lstLateMarkConfigurations = ViewState[S_LATE_MARK_CONFIG] as List<LateMarkConfiguration>;
            foreach (LateMarkConfiguration config in lstLateMarkConfigurations)
            {
                iLastLateMarkCount = config.LateMarkCount;
                dcLastConsideredLeaves = config.ConsideredLeaves;
                if (iRemainingLateMarks >= config.LateMarkCount)
                {
                    dcTotalLateMarkLeaves += dcLastConsideredLeaves;
                    iRemainingLateMarks -= iLastLateMarkCount;
                }
                else
                    break;
            }

            if (iRemainingLateMarks >= iLastLateMarkCount && iRemainingLateMarks != iTotalLateMarks)
            {
                while (iRemainingLateMarks > 0)
                {
                    if (iRemainingLateMarks >= iLastLateMarkCount)
                        dcTotalLateMarkLeaves += dcLastConsideredLeaves;
                    iRemainingLateMarks -= iLastLateMarkCount;
                }
            }
        }

        return dcTotalLateMarkLeaves;
    }

    /// <summary>
    /// This method is used to fill user combobox.
    /// </summary>
    private void FillUserCombobox()
    {
        int iMonthId = Convert.ToInt32(hidMonthId.Value);
        int iYear = Convert.ToInt32(hidYear.Value);
        int iStaffGroupId = Convert.ToInt32(hidStaffGroup.Value);

        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetLeavesAndUsers(miSchoolId, miAcademicYearId, iStaffGroupId, iMonthId, iYear);
        SalaryEntityList olstSalaryEntityLists = oSalaryDetailsBL.SalaryEntityLists;

        if (olstSalaryEntityLists.lstStaffGroups.Count > 0)
        {
            ListSource.FillDropDownList(olstSalaryEntityLists.lstStaffGroups, cmbStaffGroup, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);
            cmbStaffGroup.FindByValue(hidStaffGroup.Value);
        }

        // Fill user combobox.
        SerUserDetails(olstSalaryEntityLists.lstSalaryCommonUtility);

        // Fill leave combobox.
        if (olstSalaryEntityLists.lstConfiguredLeaves.Count > 0)
            SetConfiguredLeaves(olstSalaryEntityLists.lstConfiguredLeaves);

        ViewState[S_CONFIGURED_LEAVE] = olstSalaryEntityLists.lstConfiguredLeaves;

        // Set salary month and year.
        hidSalaryMonthId.Value = oSalaryDetailsBL.SalaryMonthAndYear.MonthId.ToString();
        hidSalaryYear.Value = oSalaryDetailsBL.SalaryMonthAndYear.Year.ToString();
    }

    /// <summary>
    /// This method is used to set user details.
    /// </summary>
    /// <param name="lstSalaryCommonUtility"></param>
    private void SerUserDetails(List<SalaryCommonUtility> aolstSalaryCommonUtility)
    {
        ListSource.FillDropDownList(aolstSalaryCommonUtility, cmbUsers, "Name", "UserId", string.Empty);
        if (aolstSalaryCommonUtility.Count > 0)
        {
            cmbUsers.FindByValue(hidSelectedUserId.Value);
            trLeaveDetails.Visible = true;
            trNoLeaveConfig.Visible = false;
            hidIsLeaveConfigured.Value = Constants.S_YES;
        }
        else
        {
            trLeaveDetails.Visible = false;
            trNoLeaveConfig.Visible = true;
            hidIsLeaveConfigured.Value = Constants.S_NO;
        }
    }

    /// <summary>
    /// this method is used to fill leave combobox.
    /// </summary>
    /// <param name="oDTUserLeaves"></param>
    private void SetConfiguredLeaves(List<ConfiguredLeaves> alstConfiguredLeave)
    {
        const string S_LEAVES = "Leaves";
        StringBuilder oStringBuilder = new StringBuilder();
        if (alstConfiguredLeave != null)
        {                   
            List<int> lstLeaveIds = (from id in alstConfiguredLeave where id.AllowZeroBalance == true select id.LeaveId).ToList();
            hidAllowZeroBalance.Value = String.Join(",", lstLeaveIds);         
        }
        if (alstConfiguredLeave == null || alstConfiguredLeave.Count == 0)
            alstConfiguredLeave = ViewState[S_LEAVES] as List<ConfiguredLeaves>;
        cmbAllLeaves.Items.Clear();
        cmbAllLeaves.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        alstConfiguredLeave.ForEach
            (
                configuredLeave =>
                {
                    ListItem oListItem = new ListItem();
                    oListItem.Text = configuredLeave.ShortName;
                    oListItem.Value = configuredLeave.LeaveId.ToString();
                    oListItem.Attributes.Add("style", "background-color:" + configuredLeave.ColorCode + ";");
                    cmbAllLeaves.Items.Add(oListItem);
                }
            );

        ViewState[S_LEAVES] = alstConfiguredLeave;
    }

    /// <summary>
    /// This method is used to return late mark leave details.
    /// </summary>
    /// <param name="adcTotalLateMarkLeaves"></param>
    /// <returns></returns>
    private Dictionary<int, decimal> ApplyLateMarkLeaves(decimal adcTotalLateMarkLeaves)
    {
        Dictionary<int, decimal> dictLateMarkLeaves = new Dictionary<int, decimal>();
        if (adcTotalLateMarkLeaves != 0)
            return mlstLeaveTypeCount != null
                       ? GetLateMarkLeaves(adcTotalLateMarkLeaves)
                       : GetLateMarksWithoutLeaves(adcTotalLateMarkLeaves);

        return dictLateMarkLeaves;
    }

    /// <summary>
    /// This method is used to return late mark leave details with considering used leaves.
    /// </summary>
    /// <param name="adcTotalLateMarkLeaves"></param>
    /// <returns></returns>
    private Dictionary<int, decimal> GetLateMarkLeaves(decimal adcTotalLateMarkLeaves)
    {
        List<DaywiseLeaves> lstDaywiseLeaves = null;
        if (ViewState[S_LEAVE_TABLE] != null)
            lstDaywiseLeaves = (List<DaywiseLeaves>)ViewState[S_LEAVE_TABLE];

        List<LateMarkLeave> lstLateMarkLeaves = null;
        if (ViewState[S_LATE_MARK_LEAVES] != null)
            lstLateMarkLeaves = ViewState[S_LATE_MARK_LEAVES] as List<LateMarkLeave>;

        Dictionary<int, decimal> dictLateMarkLeaves = new Dictionary<int, decimal>();

        List<DaywiseLeaves> lstLeaves = (from balance in lstDaywiseLeaves
                                         join leave in mlstLeaveTypeCount
                                         on balance.LeaveId equals leave.LeaveId into usedLeaves
                                         from usedLeave in usedLeaves.DefaultIfEmpty(new UsedLeaves { LeaveId = 0, LeaveCount = 0 })
                                         orderby balance.SortOrder ascending, balance.OriginalLeaveId ascending
                                         where balance.SortOrder != 9999
                                         select new DaywiseLeaves
                                         {
                                             LeaveId = balance.LeaveId,
                                             LeaveBalance = balance.LeaveBalance - usedLeave.LeaveCount - balance.MinimumBalance
                                         }
                              ).ToList();

        lstLeaves = (from dayLeave in lstLeaves
                     join lateMarkLeave in lstLateMarkLeaves
                          on dayLeave.LeaveId equals lateMarkLeave.LeaveId into totalLeaves
                     from leave in totalLeaves.DefaultIfEmpty()
                     select new DaywiseLeaves
                     {
                         LeaveId = dayLeave.LeaveId,
                         LeaveBalance = dayLeave.LeaveBalance + (leave == null ? 0 : leave.Days)
                     }).ToList();

        lstLeaves.ForEach
            (
                leave =>
                {
                    if (leave.LeaveBalance > 0 && adcTotalLateMarkLeaves > 0)
                    {
                        if (leave.LeaveBalance < adcTotalLateMarkLeaves)
                        {
                            dictLateMarkLeaves.Add(leave.LeaveId, leave.LeaveBalance);
                            adcTotalLateMarkLeaves -= leave.LeaveBalance;
                        }
                        else
                        {
                            if (adcTotalLateMarkLeaves != 0)
                                dictLateMarkLeaves.Add(leave.LeaveId, adcTotalLateMarkLeaves);
                            adcTotalLateMarkLeaves = 0;
                        }
                    }
                }
            );

        if (dictLateMarkLeaves.Count == 0 || adcTotalLateMarkLeaves > 0)
        {
            int iLeaveId = GetUnpaidLeaveId();
            if (iLeaveId != 0)
                dictLateMarkLeaves.Add(iLeaveId, adcTotalLateMarkLeaves);
        }

        return dictLateMarkLeaves;
    }

    /// <summary>
    /// This method is used to return late mark leave details without considering used leaves.
    /// </summary>
    /// <param name="adcTotalLateMarkLeaves"></param>
    /// <returns></returns>
    private Dictionary<int, decimal> GetLateMarksWithoutLeaves(decimal adcTotalLateMarkLeaves)
    {
        List<DaywiseLeaves> lstDaywiseLeaves = null;
        if (ViewState[S_LEAVE_TABLE] != null)
            lstDaywiseLeaves = (List<DaywiseLeaves>)ViewState[S_LEAVE_TABLE];

        List<LateMarkLeave> lstLateMarkLeaves = null;
        if (ViewState[S_LATE_MARK_LEAVES] != null)
            lstLateMarkLeaves = ViewState[S_LATE_MARK_LEAVES] as List<LateMarkLeave>;

        Dictionary<int, decimal> dictLateMarkLeaves = new Dictionary<int, decimal>();

        List<DaywiseLeaves> lstLeaves = null;
        if (lstLateMarkLeaves != null)
        {
            if (lstLateMarkLeaves.Count > 0)
            {
                lstLeaves = (from dayLeaves in lstDaywiseLeaves
                             join lateMarkLeave in lstLateMarkLeaves
                                 on dayLeaves.LeaveId equals lateMarkLeave.LeaveId into totalLeaves
                             from leave in totalLeaves.DefaultIfEmpty()
                             select new DaywiseLeaves
                                        {
                                            LeaveId = dayLeaves.LeaveId,
                                            LeaveBalance =
                                                dayLeaves.LeaveBalance + (leave == null ? 0 : leave.Days) -
                                                dayLeaves.MinimumBalance
                                        }).ToList();
            }
            else
            {
                lstLeaves = lstDaywiseLeaves
                    .OrderBy(balance => balance.SortOrder)
                    .OrderBy(balance => balance.OriginalLeaveId)
                    .Select(balance => balance)
                    .ToList();
            }
        }

        lstLeaves.ForEach
            (
                leave =>
                {
                    if (leave.LeaveBalance > 0 && adcTotalLateMarkLeaves > 0)
                    {
                        if (leave.LeaveBalance < adcTotalLateMarkLeaves)
                        {
                            dictLateMarkLeaves.Add(leave.LeaveId, leave.LeaveBalance);
                            adcTotalLateMarkLeaves -= leave.LeaveBalance;
                        }
                        else
                        {
                            dictLateMarkLeaves.Add(leave.LeaveId, adcTotalLateMarkLeaves);
                            adcTotalLateMarkLeaves = 0;
                        }
                    }
                }
            );

        if (adcTotalLateMarkLeaves > 0)
        {
            int iLeaveId = GetUnpaidLeaveId();
            if (iLeaveId != 0)
                dictLateMarkLeaves.Add(iLeaveId, adcTotalLateMarkLeaves);
        }

        return dictLateMarkLeaves;
    }

    /// <summary>
    /// This method is used to return unpaid leave id.
    /// </summary>
    /// <returns></returns>
    private int GetUnpaidLeaveId()
    {
        int iLeaveId = 0;
        List<ConfiguredLeaves> lstConfiguredLeaves = null;
        if (ViewState[S_CONFIGURED_LEAVE] != null)
            lstConfiguredLeaves = ViewState[S_CONFIGURED_LEAVE] as List<ConfiguredLeaves>;
        if (lstConfiguredLeaves != null && lstConfiguredLeaves.Count > 0)
        {
            var oLeave = lstConfiguredLeaves.Where(leave => leave.IsUnpaidLeave).OrderBy(lv => lv.OriginalLeaveId).Select(leave => leave.LeaveId);
            if (oLeave != null && oLeave.Count() > 0)
                iLeaveId = oLeave.First();                
        }
        return iLeaveId;
    }

    /// <summary>
    /// This method is used to Validate leave balance.
    /// </summary>
    /// <returns></returns>
    private bool ValidateLeaveBalance()
    {
        List<DaywiseLeaves> lstDaywiseLeaveTypes = null;
        if (ViewState[S_LEAVE_TABLE] != null)
            lstDaywiseLeaveTypes = (List<DaywiseLeaves>)ViewState[S_LEAVE_TABLE];

        string[] sArrDays = hidDay.Value.Split('$');
        string[] sArrLeaves = hidLeave.Value.Split('$');
        string[] sArrHalfLeaves = hidHalfLeaves.Value.Split('$');
        string[] sArrLateMarks = hidLateMarkDays.Value.Split('$');
        string[] sArrColors = hidColorCodes.Value.Split('$');
        var AllowZero = hidAllowZeroBalance.Value;
        var sArrAllowZero = AllowZero.Split(',');

        int iDayInMonths = DateTime.DaysInMonth(LeaveCalendar.SelectedDate.Year, LeaveCalendar.SelectedDate.Month);
        List<int> lstLeaveIds = new List<int>();

        molstDaywiseLeaves = new List<DaywiseLeaves>();
        DaywiseLeaves daywiseLeave;

        for (int iCount = 1; iCount <= iDayInMonths; iCount++)
        {
            if (sArrDays.Contains(iCount.ToString()) || sArrLateMarks.Contains(iCount.ToString()))
            {
                daywiseLeave = new DaywiseLeaves();
                daywiseLeave.Day = iCount;
                if (sArrDays.Contains(iCount.ToString()))
                {
                    daywiseLeave.LeaveId = Convert.ToInt16(sArrLeaves[sArrDays.ToList().IndexOf(iCount.ToString())]);
                    daywiseLeave.LeaveCount = 1;
                    daywiseLeave.IsHalfLeave = false;
                    daywiseLeave.ColorCode = sArrColors[sArrDays.ToList().IndexOf(iCount.ToString())];
                    lstLeaveIds.Add(daywiseLeave.LeaveId);
                    if (sArrHalfLeaves.Contains(iCount.ToString()))
                    {
                        daywiseLeave.LeaveCount = 0.5;
                        daywiseLeave.IsHalfLeave = true;
                    }
                }
                else
                {
                    daywiseLeave.LeaveId = 0;
                    daywiseLeave.IsHalfLeave = false;
                    daywiseLeave.LeaveCount = 1;
                }

                daywiseLeave.IsLateMark = sArrLateMarks.Contains(daywiseLeave.Day.ToString());
                molstDaywiseLeaves.Add(daywiseLeave);
            }
        }

        // Start - Partial Leaves
        if (!string.IsNullOrEmpty(hidPartialLeaves.Value))
        {
            if (moLeaveDictionary == null)
                moLeaveDictionary = new Dictionary<int, int>();

            moLeaveDictionary.Clear();

            string[] leaves = hidPartialLeaves.Value.Split('$');
            string[] leaveDays;

            leaves.ToList().ForEach(
                leave =>
                {
                    leaveDays = leave.Split(',');
                    if (leaveDays[0] != string.Empty && leaveDays[1] != "0")
                        moLeaveDictionary.Add(Convert.ToInt32(leaveDays[0]), Convert.ToInt32(leaveDays[1]));
                });

            ViewState[S_PARTIAL_LEAVES] = moLeaveDictionary;

            foreach (KeyValuePair<int, int> kvp in moLeaveDictionary)
            {
                molstDaywiseLeaves.Add
                    (
                        new DaywiseLeaves
                        {
                            Day = kvp.Key,
                            LeaveId = kvp.Value,
                            LeaveCount = 0.5,
                            IsHalfLeave = true,
                            IsLateMark = false
                        }
                    );
            }
        }

        // End - parial leaves
        mlstLeaveTypeCount = (molstDaywiseLeaves
                                          .GroupBy(leave => leave.LeaveId)
                                          .Select(leaveGroups => new UsedLeaves
                                          {
                                              LeaveId = Convert.ToInt16(leaveGroups.Key),
                                              LeaveCount = Convert.ToDecimal(leaveGroups.Sum(leaveGroup => leaveGroup.LeaveCount))
                                          }
                                          )).ToList();

        var extraLeaves = lstDaywiseLeaveTypes
                                    .Join(
                                            mlstLeaveTypeCount,
                                            leaveBalance => leaveBalance.LeaveId,
                                            leaveCount => leaveCount.LeaveId,
                                            (leaveBalance, leaveCount) => new
                                            {
                                                leaveBalance.ShortName,
                                                leaveBalance.LeaveBalance,
                                                leaveCount.LeaveCount,
                                                leaveBalance.MinimumBalance,
                                                leaveBalance.LeaveId
                                            }
                                         )
                                    .Where(leaves => leaves.LeaveBalance - leaves.LeaveCount < leaves.MinimumBalance)
                                    .Select(leaves => new { leaves.ShortName, LeaveBalance = leaves.MinimumBalance + leaves.LeaveCount - leaves.LeaveBalance, LeaveId = leaves.LeaveId })
                                    .ToList();

        if (extraLeaves.Count() > 0)
        {
            string sLeaveNames = string.Empty;

            if (sArrAllowZero.Length > 0)
            {
                for (int iitem = 0; iitem < sArrAllowZero.Length;iitem++)
                {
                    if (sArrAllowZero[iitem].Trim() != string.Empty)
                    {
                        if (extraLeaves.Any(el => el.LeaveId == sArrAllowZero[iitem].ToInt()))
                        {
                            var s = extraLeaves.Where(el => el.LeaveId == sArrAllowZero[iitem].ToInt()).FirstOrDefault();
                            extraLeaves.Remove(s);
                        }
                    }
                }
            }

             extraLeaves.ForEach(sName => sLeaveNames = string.Format("{0}, {1}", sLeaveNames, sName.ShortName));

            if (sLeaveNames.StartsWith(", "))
                sLeaveNames = sLeaveNames.Substring(2);

            if (sLeaveNames != string.Empty)
            {
                lblSuccessMessage.ForeColor = System.Drawing.Color.Red;
                lblSuccessMessage.Text = string.Format("Leave balance of {0} is not sufficient. It should be greater than minimum balance.", sLeaveNames);
                hidUsedLeaveBkp.Value = Constants.S_NO;
                UpdateCalenderEventSource(molstDaywiseLeaves);

                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// This method is used to update calendar event source.
    /// </summary>
    /// <param name="alstDaywiseLeaves"></param>
    private void UpdateCalenderEventSource(List<DaywiseLeaves> alstDaywiseLeaves)
    {
        DataTable oDTLeaveDetails = LeaveCalendar.EventSource as DataTable;
        if (oDTLeaveDetails.IsNonEmpty())
        {
            int iRowCount = oDTLeaveDetails.Rows.Count;
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                DataRow oDRLeave = oDTLeaveDetails.Rows[iRowIndex];

                string sHtmlDesign = oDRLeave["Status_Desc"].ToString();
                int iDay = Convert.ToDateTime(oDRLeave["Leave_Date"]).Day;

                var oDaywiseLeaves = alstDaywiseLeaves
                                    .Where(leaveDays => leaveDays.Day == iDay)
                                    .Select(leaveDays => leaveDays);

                sHtmlDesign = sHtmlDesign.Replace("selected='selected'", string.Empty);
                sHtmlDesign = sHtmlDesign.Replace("checked='checked'", string.Empty);
                sHtmlDesign = sHtmlDesign.Replace("disabled='disabled'", string.Empty);
                sHtmlDesign = sHtmlDesign.Replace("id='halfLeavespan" + iDay + "' style='background-color:transparent'", "id='halfLeavespan" + iDay + "' style='background-color:transparent'");
                sHtmlDesign = sHtmlDesign.Replace("id='lateMarkSpan" + iDay + "' style='background-color:transparent'", "id='lateMarkSpan" + iDay + "' style='background-color:transparent'");

                Type tColors = typeof(Color);
                PropertyInfo[] oPropInfoArr = tColors.GetProperties(BindingFlags.Static | BindingFlags.Public);

                oPropInfoArr.ToList().ForEach
                    (
                        oProperty =>
                        {
                            if (oProperty.DeclaringType.Equals(typeof(Color)))
                                sHtmlDesign = sHtmlDesign.Replace(string.Format("style='font-size: X-Small;background-color:{0}'", oProperty.Name), string.Format("style='font-size: X-Small;background-color:white'"));
                        }
                );

                if (oDaywiseLeaves.Count() > 0)
                {
                    DaywiseLeaves oDaywiseLeave = oDaywiseLeaves.First();

                    sHtmlDesign = sHtmlDesign.Replace(string.Format("style='font-size: X-Small;background-color:white'"), string.Format("style='font-size: X-Small;background-color:{0}'", oDaywiseLeave.ColorCode));
                    sHtmlDesign = sHtmlDesign.Replace(string.Format("option value='{0}'", oDaywiseLeave.LeaveId), string.Format("option value='{0}' selected='selected' style= 'background-color:{1}'", oDaywiseLeave.LeaveId, oDaywiseLeave.ColorCode));

                    if (oDaywiseLeave.IsHalfLeave)
                    {
                        sHtmlDesign = sHtmlDesign.Replace("> H</input>", "checked='checked'> H</input>");
                        sHtmlDesign = sHtmlDesign.Replace("id='halfLeavespan" + iDay + "' style='background-color:transparent'", "style='background-color:#009B9B'");
                    }

                    if (oDaywiseLeave.IsLateMark)
                    {
                        sHtmlDesign = sHtmlDesign.Replace("> L</input>", "checked='checked'> L</input>");
                        sHtmlDesign = sHtmlDesign.Replace("id='lateMarkSpan" + iDay + "' style='background-color:transparent'", "style='background-color:#01A5EC'");
                    }
                }
                oDRLeave["Status_Desc"] = sHtmlDesign;
            }
            LeaveCalendar.EventSource = oDTLeaveDetails;
        }
    }

    /// <summary>
    /// This function is used to set the columns of the event calendar.
    /// </summary>
    private void SetCalendarColumns()
    {
        LeaveCalendar.EventStartDateColumnName = "Leave_Date";
        LeaveCalendar.EventEndDateColumnName = "Leave_Date";
        LeaveCalendar.EventDescriptionColumnName = "Status_Description";
        LeaveCalendar.EventHeaderColumnName = "Status_Desc";
        LeaveCalendar.EventBackColorName = "Status_BackColur";
        LeaveCalendar.EventForeColorName = "Status_ForeColur";
    }

    /// <summary>
    /// This function is used to set the events to the calendar control.
    /// </summary>
    private void GetStaffLeavesDetails(bool abIsStaffGroupChanged)
    {
        int iMonthId = Convert.ToInt32(hidMonthId.Value);
        int iYear = Convert.ToInt32(hidYear.Value);
        int iStaffGroupId = Convert.ToInt32(cmbStaffGroup.SelectedValue);
        hidStaffGroup.Value = cmbStaffGroup.SelectedValue;

        if (LeaveCalendar.VisibleDate == DateTime.MinValue)
            LeaveCalendar.VisibleDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), 1);
        else
        {
            iMonthId = LeaveCalendar.VisibleDate.Month;
            iYear = LeaveCalendar.VisibleDate.Year;
            hidMonthId.Value = iMonthId.ToString();
            hidYear.Value = iYear.ToString();
        }

        moDatewiseStaffLeavesBL = new DatewiseStaffLeavesBL();
        DatewiseStaffLeave oDatewiseStaffLeave = new DatewiseStaffLeave
        {
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            MonthId = iMonthId,
            Year = iYear,
            StaffGroupsId = iStaffGroupId,
            UserId = abIsStaffGroupChanged ? 0 : Convert.ToInt32(cmbUsers.SelectedValue)
        };

        moDatewiseStaffLeavesBL.DatewiseStaffLeaves = oDatewiseStaffLeave;
        moDatewiseStaffLeavesBL.GetUserLeavesDetails();

        hidIsPreEnclosed.Value = moDatewiseStaffLeavesBL.PreAttachedHolidayId.ToString();
        hidIsPostEnclosed.Value = moDatewiseStaffLeavesBL.PostAttachedHolidayId.ToString();

        SetLeaveDetails(abIsStaffGroupChanged);
        SetJoiningDate();
        ViewState[S_JOINING_DATES] = moDatewiseStaffLeavesBL.StaffBaseDetails;
        SetQueryString();
    }

    /// <summary>
    /// This method is used to set querystring.
    /// </summary>
    private void SetQueryString()
    {
        int iSalaryYear = Convert.ToInt32(hidSalaryYear.Value);
        int iSalaryMonthId = Convert.ToInt32(hidSalaryMonthId.Value);

        if (iSalaryYear == 0 && iSalaryMonthId == 0)
        {
            iSalaryYear = Convert.ToInt32(QueryString["MonthId"]);
            iSalaryMonthId = Convert.ToInt32(QueryString["Year"]);
            hidSalaryYear.Value = LeaveCalendar.VisibleDate.Year.ToString();
            hidSalaryMonthId.Value = LeaveCalendar.VisibleDate.Month.ToString();
        }

        int iYear = Convert.ToInt32(hidYear.Value);
        int iMonthId = Convert.ToInt32(hidMonthId.Value);

        bool bIsValid = (iYear < iSalaryYear || iYear > iSalaryYear) ? false : iMonthId >= iSalaryMonthId ? true : false;
        if (bIsValid)
        {
            Dictionary<int, int> partialLeaveDictionary = new Dictionary<int, int>();
            if (ViewState[S_PARTIAL_LEAVES] != null)
                partialLeaveDictionary = ViewState[S_PARTIAL_LEAVES] as Dictionary<int, int>;

            StringBuilder sLeaves = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in partialLeaveDictionary)
                sLeaves.Append("$" + kvp.Key + "," + kvp.Value);
            string sPartiaLeave = string.Empty;
            if (sLeaves.Length > 0)
                sPartiaLeave = sLeaves.ToString().Substring(1);
            string sQueryString = "MonthId=" + hidMonthId.Value +
                                  "&Year=" + hidYear.Value +
                                  "&UserId=" + cmbUsers.SelectedValue +
                                  "&StaffGroupId=" + hidStaffGroup.Value +
                                  "&PartialLeaves=" + sPartiaLeave +
                                  "&Filter=" + hidFilter.Value;
            lnkPartialLeave.NavigateUrl = "PartialLeavePopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        }
        else
            lnkPartialLeave.NavigateUrl = string.Empty;
    }

    /// <summary>
    /// This method is used to diplay joining and resign date.
    /// </summary>
    private void SetJoiningDate()
    {
        string sJoiningDate = "N/A";
        string sResignDate = "N/A";
        if (moDatewiseStaffLeavesBL.StaffBaseDetails != null)
        {
            if (moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate != DateTime.MinValue && moDatewiseStaffLeavesBL.StaffBaseDetails.ResignDate != DateTime.MinValue)
            {
                sJoiningDate = moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate.ToString("dd-MMM-yyyy");
                sResignDate = moDatewiseStaffLeavesBL.StaffBaseDetails.ResignDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                if (moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate != DateTime.MinValue)
                    sJoiningDate = moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate.ToString("dd-MMM-yyyy");
                else if (moDatewiseStaffLeavesBL.StaffBaseDetails.ResignDate != DateTime.MinValue)
                    sResignDate = moDatewiseStaffLeavesBL.StaffBaseDetails.ResignDate.ToString("dd-MMM-yyyy");
            }
        }
        lblDates.Text = "Joining Date = " + sJoiningDate + " and Resign Date = " + sResignDate;

        if (moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate != DateTime.MinValue)
            spnJoiningDate.InnerText = moDatewiseStaffLeavesBL.StaffBaseDetails.JoiningDate.ToString("dd-MMM-yyyy");
        else
            spnJoiningDate.InnerText = "-";

        if (moDatewiseStaffLeavesBL.StaffBaseDetails.PermanentDate != DateTime.MinValue)
            spnPermanentDate.InnerText = moDatewiseStaffLeavesBL.StaffBaseDetails.PermanentDate.ToString("dd-MMM-yyyy");
        else
            spnPermanentDate.InnerText = "-";

        spnName.InnerText = cmbUsers.SelectedItem.Text;
        imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + cmbUsers.SelectedValue;
    }

    /// <summary>
    /// This method is used to set leave details.
    /// </summary>
    private void SetLeaveDetails(bool abIsStaffGroupChanged)
    {
        if (abIsStaffGroupChanged)
            SerUserDetails(moDatewiseStaffLeavesBL.SalaryCommonUtilityList);
        SetPartialLeaves();
        SetEventSource();
        SetExcludedLeaveDetails();
        SetLeaveBalanceText();
        SetLateMarkConfigNote();
        SetLateMarkLeaveSortOrder();
        SetLateMarkLeaveText();
        SetSalaryDeductionCheckBoxState();
        SetConfiguredLeaves(null);
    }

    /// <summary>
    /// This method is used to check excluded leaves from salary deduction.
    /// </summary>
    private void SetExcludedLeaveDetails()
    {
        var oLeaves = moDatewiseStaffLeavesBL.StaffLeaves.Where(lv => lv.ExcludeFromSalaryDeduction).Select(lv => lv.LeaveId);
        if (oLeaves.Count() > 0)
            hidExcludedLeaves.Value = string.Join(",", oLeaves);
        else
            hidExcludedLeaves.Value = string.Empty;
    }

    /// <summary>
    /// This method is used to set partial leave of user if exists.
    /// </summary>
    private void SetPartialLeaves()
    {
        lblPartialLeaves.Text = string.Empty;
        StringBuilder sPartialLeaves = new StringBuilder();
        StringBuilder sPartialLeaveList = new StringBuilder();
        moLeaveDictionary = new Dictionary<int, int>();

        if (string.IsNullOrEmpty(hidPartialLeaves.Value))
        {
            if (moDatewiseStaffLeavesBL.PartialLeaveDetailsList.Count > 0)
            {
                moDatewiseStaffLeavesBL.PartialLeaveDetailsList.ForEach(
                    leave =>
                    {
                        sPartialLeaves.Append(", " + leave.LeaveDate.Day + "(" + leave.ShortName + ")");
                        moLeaveDictionary.Add(leave.LeaveDate.Day, leave.PartialLeaveId);
                        sPartialLeaveList.Append("$" + leave.LeaveDate.Day + "," + leave.PartialLeaveId);
                    });
                if (sPartialLeaveList.ToString().StartsWith("$"))
                    hidPartialLeaves.Value = sPartialLeaveList.ToString().Substring(1);
            }
        }
        else
        {
            List<ConfiguredLeaves> olstConfiguredLeavess = null;
            if (ViewState[S_CONFIGURED_LEAVE] != null)
                olstConfiguredLeavess = ViewState[S_CONFIGURED_LEAVE] as List<ConfiguredLeaves>;

            string[] leaves = hidPartialLeaves.Value.Split('$');
            string[] leaveDays;

            leaves.ToList().ForEach(
                leave =>
                {
                    leaveDays = leave.Split(',');
                    if (leaveDays[1] != "0")
                        moLeaveDictionary.Add(Convert.ToInt32(leaveDays[0]), Convert.ToInt32(leaveDays[1]));
                });

            moLeaveDictionary.Join(olstConfiguredLeavess, partialLeave => partialLeave.Value, configLeave => configLeave.LeaveId, (partialLeave, configLeave)
                            => new { Day = partialLeave.Key, configLeave.ShortName, partialLeave.Value }).Where(leave => leave.Value != 0).ToList()
                           .ForEach(leave => sPartialLeaves.Append(", " + leave.Day + "(" + leave.ShortName + ")"));
        }

        if (sPartialLeaves.ToString().StartsWith(", "))
            lblPartialLeaves.Text = sPartialLeaves.ToString().Substring(2);
        ViewState[S_PARTIAL_LEAVES] = moLeaveDictionary;
    }

    /// <summary>
    /// This method is used to set salary deduction checkbox state.
    /// </summary>
    private void SetSalaryDeductionCheckBoxState()
    {
        var lstUsersSalaryDeductions = moDatewiseStaffLeavesBL.UsersSalaryDeductions
                                        .Where(UsersED => UsersED.MonthId == Convert.ToInt32(hidMonthId.Value) &&
                                        UsersED.Year == Convert.ToInt32(hidYear.Value) && UsersED.UserId == Convert.ToInt32(cmbUsers.SelectedValue));

        if (lstUsersSalaryDeductions.Count() > 0)
        {
            tdDeductHolidayLeaves.Visible = true;
            chkDeductHolidayLeaves.Checked = true;
            chkDeductHolidayLeaves.Enabled = true;
        }
        else
        {
            chkDeductHolidayLeaves.Checked = false;
            chkDeductHolidayLeaves.Enabled = false;
            tdDeductHolidayLeaves.Visible = false;
            trHoliday.Visible = false;
        }

        if (moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations.Count > 0)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            List<StaffHolidaysSalaryDeduction> lstConfigs = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                                                .Where(config => !config.IsWeekend && (config.HolidayStartDate.Year == Convert.ToInt32(hidYear.Value) || config.HolidayEndDate.Year == Convert.ToInt32(hidYear.Value)) && (config.HolidayStartDate.Month == Convert.ToInt32(hidMonthId.Value) || config.HolidayEndDate.Month == Convert.ToInt32(hidMonthId.Value)))
                                                                .ToList();

            lstConfigs.ForEach
                (
                    config =>
                    {
                        oStringBuilder.Append(", " + config.HolidayName + "(" + config.HolidayStartDate.ToString("dd-MMM-yyyy") + " to " + config.HolidayEndDate.ToString("dd-MMM-yyyy") + ")");
                    }
                );

            if (oStringBuilder.Length > 0)
            {
                lblHoliday.Text = string.Format("<span style='color:red;font-Weight:bold;'>Holiday</span> - {0}", oStringBuilder.ToString().Substring(2));
                lblHolidayHeader.Text = moDatewiseStaffLeavesBL.LateMarkConfigurations.Count == 0 ? "Note5 :" : "Note7 :";
                trHoliday.Visible = true;
            }
            else
                trHoliday.Visible = false;

            // Show weekend
            List<StaffHolidaysSalaryDeduction> lstWeekends = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                                                .Where(config => config.IsWeekend && (config.HolidayStartDate.Year == Convert.ToInt32(hidYear.Value) || config.HolidayEndDate.Year == Convert.ToInt32(hidYear.Value)) && (config.HolidayStartDate.Month == Convert.ToInt32(hidMonthId.Value) || config.HolidayEndDate.Month == Convert.ToInt32(hidMonthId.Value)))
                                                                .ToList();
            if (lstWeekends.Count > 0)
            {
                var lstDays = lstWeekends.Select(wk => wk.HolidayStartDate.DayOfWeek).Union(lstWeekends.Select(wk => wk.HolidayEndDate.DayOfWeek)).Select(wk => wk).Distinct();
                lblWeekend.Text = string.Format("<span style='color:red;font-Weight:bold;'>Weekend</span> - {0}", string.Join(", ", lstDays));

                string sNote = moDatewiseStaffLeavesBL.LateMarkConfigurations.Count == 0 ? lstConfigs.Count == 0 ? "Note5 :" : "Note6 :" : lstConfigs.Count == 0 ? "Note7 :" : "Note8 :";
                lblWeekendHeader.Text = sNote;
                trWeekend.Visible = true;
            }
            else
                trWeekend.Visible = false;
        }
        else
        {
            trHoliday.Visible = false;
            trWeekend.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set late mark text.
    /// </summary>
    private void SetLateMarkLeaveText()
    {
        List<DaywiseLeaves> lstLeaves = moDatewiseStaffLeavesBL.StaffLeaves
                                .Join
                                (
                                    moDatewiseStaffLeavesBL.LateMarkLeaves,
                                    staffLeave => staffLeave.LeaveId,
                                    lateMarkLeave => lateMarkLeave.LeaveId,
                                    (staffLeave, lateMarkLeave) => new DaywiseLeaves
                                    {
                                        ShortName = staffLeave.ShortName,
                                        Days = lateMarkLeave == null ? 0 : lateMarkLeave.Days
                                    }
                                )
                                .Select(leave => new DaywiseLeaves { ShortName = leave.ShortName, Days = leave.Days })
                                .ToList();

        StringBuilder oLateMarkLeaves = new StringBuilder();
        lstLeaves.ForEach(leave => oLateMarkLeaves.Append(string.Format(", {0}({1})", leave.ShortName, Math.Round(leave.Days, 1))));

        lblLateMarkLeaves.Text = oLateMarkLeaves.ToString();

        if (lblLateMarkLeaves.Text.StartsWith(", "))
            lblLateMarkLeaves.Text = lblLateMarkLeaves.Text.Substring(2);

        int iLateMarkCount = 0;
        iLateMarkCount = lstLeaves.Where(lateMark => lateMark.IsLateMark == true).Count();
        lblLateMark.Text = iLateMarkCount.ToString();
    }

    /// <summary>
    /// This method is used to display late mark leave sort order.
    /// </summary>
    private void SetLateMarkLeaveSortOrder()
    {
        trLeaveSortOrder.Visible = false;
        if (moDatewiseStaffLeavesBL.LateMarkConfigurations.Count > 0)
        {
            trLeaveSortOrder.Visible = true;
            lblLeaveSortOrder.Text = string.Format("Leave deduction sort order is {0}.", string.Join(" -> ", moDatewiseStaffLeavesBL.StaffLeaveSortOrders.ToArray()));
        }
    }

    /// <summary>
    /// This method is used to set late mark configuration.
    /// </summary>
    private void SetLateMarkConfigNote()
    {
        lblLateMarkNote.Text = string.Empty;
        if (moDatewiseStaffLeavesBL.LateMarkConfigurations.Count > 0)
        {
            hidConsideredLeaves.Value = moDatewiseStaffLeavesBL.LateMarkConfigurations[0].ConsideredLeaves.ToString();
            hidMaxLateMarkCount.Value = moDatewiseStaffLeavesBL.LateMarkConfigurations[0].LateMarkCount.ToString();

            int iRowCount = 1;
            StringBuilder oLateMarkNote = new StringBuilder();

            moDatewiseStaffLeavesBL.LateMarkConfigurations.ForEach
                (
                    lateMarkConfig =>
                    {
                        oLateMarkNote.Append(string.Format(", {0} leave(s) will be considered for ", Math.Round(lateMarkConfig.ConsideredLeaves, 1)));
                        if (iRowCount == 1)
                            oLateMarkNote.Append(string.Format("first {0} late mark(s)", lateMarkConfig.LateMarkCount));
                        else if (iRowCount != moDatewiseStaffLeavesBL.LateMarkConfigurations.Count)
                            oLateMarkNote.Append(string.Format("next {0} late mark(s)", lateMarkConfig.LateMarkCount));
                        else
                            oLateMarkNote.Append("each remaining " + lateMarkConfig.LateMarkCount + " late mark(s)");
                        iRowCount++;
                    }
                );

            if (oLateMarkNote.ToString().Length > 2)
            {
                trLateMarkNote.Visible = true;
                lblLateMarkNote.Text = string.Format("{0}.", oLateMarkNote.ToString().Substring(2));
            }
        }
        else
            trLateMarkNote.Visible = false;
    }

    /// <summary>
    /// This method is used to set leave calendar event source.
    /// </summary>
    private void SetEventSource()
    {
        int iNoOfDays = DateTime.DaysInMonth(moDatewiseStaffLeavesBL.DatewiseStaffLeaves.Year, moDatewiseStaffLeavesBL.DatewiseStaffLeaves.MonthId);

        hidHolidayDates.Value = string.Empty;
        hidConfigIds.Value = string.Empty;
        hidHolidays.Value = string.Empty;

        using (DataTable oDTEventSource = new DataTable())
        {
            string[] columns = { "Leave_Date", "Status_Description", "Status_Desc", "Status_BackColur", "Status_ForeColur" };
            oDTEventSource.AddColumns(columns);
            int iDayNumber = 1;
            while (iDayNumber <= iNoOfDays)
            {
                List<DaywiseLeaves> lstDatewiseLeaves = moDatewiseStaffLeavesBL.DatewiseLeaves
                                                     .Where(dtLeave => dtLeave.Date.Day == iDayNumber)
                                                     .Select(dtLeave => dtLeave)
                                                     .ToList();

                string sColorCode;
                string sOptions = GetLeavesInHtmlFormat(lstDatewiseLeaves, moDatewiseStaffLeavesBL, out sColorCode);
                int iIsHalfLeave = -1;
                int iIsLateMark = -1;
                if (lstDatewiseLeaves.Count() > 0)
                {
                    iIsHalfLeave = lstDatewiseLeaves.First().IsHalfLeave ? 1 : 0;
                    iIsLateMark = lstDatewiseLeaves.First().IsLateMark ? 1 : 0;
                }
                string sSelect = GetComboboxHtmlFormat(iDayNumber, sOptions, iIsHalfLeave, iIsLateMark, sColorCode);
                DataRow oDataRow = oDTEventSource.NewRow();
                oDataRow["Leave_Date"] = new DateTime(moDatewiseStaffLeavesBL.DatewiseStaffLeaves.Year, moDatewiseStaffLeavesBL.DatewiseStaffLeaves.MonthId, iDayNumber);
                oDataRow["Status_Desc"] = sSelect;
                oDTEventSource.Rows.Add(oDataRow);
                iDayNumber++;
            }

            if (hidHolidayDates.Value.StartsWith(","))
                hidHolidayDates.Value = hidHolidayDates.Value.Substring(1);

            if (hidConfigIds.Value.StartsWith(","))
            {
                hidConfigIds.Value = hidConfigIds.Value.Substring(1);

                List<string> sIds = hidConfigIds.Value.Split(',').ToList().Where(id => id != string.Empty).ToList();

                List<string> lstIds = (from sh in moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                       join id in sIds
                                    on sh.StaffHolidaysSalaryDeductionId equals id.ToInt()
                                       where id != string.Empty
                                       orderby sh.HolidayStartDate
                                       select id).ToList();
                hidConfigIds.Value = string.Join(",", lstIds);
            }

            if (oDTEventSource.IsNonEmpty())
                LeaveCalendar.EventSource = oDTEventSource;
        }
    }

    /// <summary>
    /// This method is used to return combobox html strudture.
    /// </summary>
    /// <param name="aiDayNumber"></param>
    /// <param name="asOptions"></param>
    /// <param name="aiIsHalfLeave"></param>
    /// <param name="aiIsLateMark"></param>
    /// <param name="asColorCode"></param>
    /// <returns></returns>
    private string GetComboboxHtmlFormat(int aiDayNumber, string asOptions, int aiIsHalfLeave, int aiIsLateMark, string asColorCode)
    {
        string sSelect = "<span id = 'spn2' style= 'color:blue'><select  id='cmb_" + aiDayNumber + "' style='font-size: X-Small;background-color:" + asColorCode + "' name='cmbLeaves' onchange= 'DisableCheckbox(cmb_" + aiDayNumber + ",chk_" + aiDayNumber + ",chkLateMark_" + aiDayNumber + "," + aiDayNumber + ")'>" +
                "<option value='0' style= 'background-color:White'>Select</option>" + asOptions + "</select>" +
                "<span id='halfLeavespan" + aiDayNumber + "' style='background-color:transparent' onclick ='CheckHalfLeave(this," + aiDayNumber + ")'><input type='checkbox' name='chk2' runat='server' onclick= 'OnCheck(chk_" + aiDayNumber + ",chkLateMark_" + aiDayNumber + ")' id='chk_" + aiDayNumber + "'";
        if (aiIsHalfLeave == 1)
            sSelect = string.Format("{0} checked='checked'", sSelect);
        else if (aiIsHalfLeave != 0)
            sSelect = string.Format("{0} disabled='disabled'", sSelect);
        sSelect = sSelect + "> H</input></span>" +
            "&nbsp;<span id='lateMarkSpan" + aiDayNumber + "' style='background-color:transparent' onclick ='CheckLateMark(this," + aiDayNumber + ")'><input type='checkbox' name='chk3' runat='server' id='chkLateMark_" + aiDayNumber + "'";
        if (aiIsLateMark == 1)
        {
            sSelect = sSelect.Replace("id='lateMarkSpan" + aiDayNumber + "' style='background-color:transparent'", "id='lateMarkSpan" + aiDayNumber + "' style='background-color:#01A5EC'");
            sSelect = string.Format("{0} checked='checked'", sSelect);
        }

        SetHolidayLeavesConfigDetails(aiDayNumber, ref sSelect);

        if (asOptions.Contains("selected"))
            sSelect = aiIsHalfLeave == 0
                          ? string.Format("{0} disabled='disabled'", sSelect)
                          : sSelect.Replace(
                              "id='halfLeavespan" + aiDayNumber + "' style='background-color:transparent'",
                              "id='halfLeavespan" + aiDayNumber + "' style='background-color:#009B9B'");

        sSelect = string.Format("{0}> L</input></span>", sSelect);
        sSelect = string.Format("{0}</span>", sSelect);

        if (moLeaveDictionary.ContainsKey(aiDayNumber))
        {
            if (moLeaveDictionary.ContainsKey(aiDayNumber))
                sSelect += "&nbsp;&nbsp;<span id='lblpartialleave" + aiDayNumber + "' style='color:#066;font-weight:bold;'>P</span>";
        }

        return sSelect;
    }

    /// <summary>
    /// This method is used to set holiday configuration.
    /// </summary>
    /// <param name="aiDayNumber"></param>
    /// <param name="asSelect"></param>
    private void SetHolidayLeavesConfigDetails(int aiDayNumber, ref string asSelect)
    {
        DateTime dtCurrentDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), aiDayNumber);
        int iRecordCount = 0;

        var lstStaffHolidaysSalaryDeduction = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                                                     .Where(holiday => dtCurrentDate.IsBetween(holiday.HolidayStartDate, holiday.HolidayEndDate));
        if (lstStaffHolidaysSalaryDeduction.Count() == 0)
        {
            int iStartDay;
            int iEndDay;
            int iYear;
            int iMonth;

            iYear = Convert.ToInt32(hidYear.Value);
            iMonth = Convert.ToInt32(hidMonthId.Value);
            int iDaysInMonth = DateTime.DaysInMonth(iYear, iMonth);
            iStartDay = aiDayNumber + 1;
            iEndDay = iDaysInMonth;

            //Previous and next months and years.
            int iPreviousMonthId = iMonth - 1;
            int iPreviousMonthYear = iYear;
            int iNextMonthId = iMonth + 1;
            int iNextMonthYear = iYear;
            if (iPreviousMonthId == 0)
            {
                iPreviousMonthId = 12;
                iPreviousMonthYear = iPreviousMonthYear - 1;
            }

            if (iNextMonthId == 13)
            {
                iNextMonthId = 1;
                iNextMonthYear = iNextMonthYear + 1;
            }

            if (iStartDay == iDaysInMonth + 1)
            {
                iStartDay = 1;
                iEndDay = aiDayNumber - 1;
                if (iMonth == 12)
                {
                    iMonth = 1;
                    iYear = iYear + 1;
                }
                else
                    iMonth = iMonth + 1;
            }

            // Check whether next day is holiday or if current day is last working day then upcoming first working day is holiday.
            for (int iDay = iStartDay; iDay <= iEndDay; iDay++)
            {
                DateTime dtCurrDate = new DateTime(iYear, iMonth, iDay);
                bool bIsConfigWeekend = GetWeekendConfig(dtCurrDate);

                if (!bIsConfigWeekend)
                {
                    asSelect = SetHolidayMark(aiDayNumber, asSelect, dtCurrentDate, dtCurrDate, true);
                    break;
                }

                if (iDay == iDaysInMonth)
                {
                    if (iMonth == 12)
                    {
                        iMonth = 1;
                        iYear = iYear + 1;
                    }
                    else
                        iMonth = iMonth + 1;

                    iDaysInMonth = DateTime.DaysInMonth(iYear, iMonth);
                    iStartDay = 1;
                    iEndDay = aiDayNumber - 1;
                    iDay = iDaysInMonth - 1;
                }
            }

            iStartDay = aiDayNumber - 1;
            iEndDay = 1;
            iYear = Convert.ToInt32(hidYear.Value);
            iMonth = Convert.ToInt32(hidMonthId.Value);
           
            if (iStartDay == 0)
            {
                if (iMonth == 1)
                {
                    iMonth = 12;
                    iYear = iYear - 1;
                }
                else
                    iMonth = iMonth - 1;

                iDaysInMonth = DateTime.DaysInMonth(iYear, iMonth);
                iStartDay = iDaysInMonth;
                iEndDay = aiDayNumber + 1;
            }

            // Check whether previous day is holiday or if current day is first working day then last working day was holiday.            
            for (int iDay = iStartDay; iDay >= iEndDay; iDay--)
            {
                DateTime dtCurrDate = new DateTime(iYear, iMonth, iDay);
                bool bIsConfigWeekend = GetWeekendConfig(dtCurrDate);

                if (!bIsConfigWeekend)
                {
                    asSelect = SetHolidayMark(aiDayNumber, asSelect, dtCurrentDate, dtCurrDate, false);
                    break;
                }
                if (iDay == 1)
                {
                    if (iMonth == 1)
                    {
                        iMonth = 12;
                        iYear = iYear - 1;
                    }

                    iDaysInMonth = DateTime.DaysInMonth(iYear, iMonth);
                    iStartDay = iDaysInMonth;
                    iEndDay = aiDayNumber + 1;
                    iDay = iDaysInMonth + 1;
                }
            }





            bool bIsWkEnd = moDatewiseStaffLeavesBL.WeekDays
                                .Where(config => config.IsWeekend &&
                                                (config.OriginalWeekDaysId == Convert.ToInt32(dtCurrentDate.DayOfWeek) ||
                                                config.OriginalWeekDaysId - 7 == Convert.ToInt32(dtCurrentDate.DayOfWeek))
                                      )
                                .Count() > 0 ? true : false;

            bool bIsLastDayWeekend = moDatewiseStaffLeavesBL.WeekDays
                                .Where(config => config.IsWeekend &&
                                                (config.OriginalWeekDaysId == Convert.ToInt32(dtCurrentDate.AddDays(-1).DayOfWeek) ||
                                                config.OriginalWeekDaysId - 7 == Convert.ToInt32(dtCurrentDate.AddDays(-1).DayOfWeek))
                                      )
                                .Count() > 0 ? true : false;

            bool bIsNextWeekend = moDatewiseStaffLeavesBL.WeekDays
                                .Where(config => config.IsWeekend &&
                                                (config.OriginalWeekDaysId == Convert.ToInt32(dtCurrentDate.AddDays(1).DayOfWeek) ||
                                                config.OriginalWeekDaysId - 7 == Convert.ToInt32(dtCurrentDate.AddDays(1).DayOfWeek))
                                      )
                                .Count() > 0 ? true : false;

            if (bIsWkEnd || bIsLastDayWeekend || bIsNextWeekend)
            {
                if (aiDayNumber <= 3)
                {
                    iYear = dtCurrentDate.Month == 1? dtCurrentDate.Year - 1 : dtCurrentDate.Year;
                    iMonth = dtCurrentDate.Month == 1 ? 12:dtCurrentDate.Month - 1;
                    int iDaysInLastMonth = DateTime.DaysInMonth(iYear, iMonth);
                    for (int iDay = iDaysInLastMonth; iDay >= iDaysInLastMonth - 3; iDay--)
                    {
                        DateTime dtCurrDate = new DateTime(iYear, iMonth, iDay);
                        bool bIsConfigWeekend = GetWeekendConfig(dtCurrDate);

                        if (!bIsConfigWeekend)
                        {
                            asSelect = SetHolidayMark(aiDayNumber, asSelect, dtCurrentDate, dtCurrDate, false);
                            break;
                        }
                    }
                }

                int iLastDay = DateTime.DaysInMonth(dtCurrentDate.Year, dtCurrentDate.Month) - 3;
                if (aiDayNumber >= iLastDay)
                {
                    for (int iDay = 1; iDay <= 3; iDay++)
                    {
                        iYear = dtCurrentDate.Month == 12 ? dtCurrentDate.Year + 1 : dtCurrentDate.Year;
                        iMonth = dtCurrentDate.Month == 12 ? 1 : dtCurrentDate.Month + 1;
                        DateTime dtCurrDate = new DateTime(iYear, iMonth, iDay);
                        bool bIsConfigWeekend = GetWeekendConfig(dtCurrDate);

                        if (!bIsConfigWeekend)
                        {
                            asSelect = SetHolidayMark(aiDayNumber, asSelect, dtCurrentDate, dtCurrDate, false);
                            break;
                        }
                    }
                }
            }

        }

        iRecordCount = 0;
        iRecordCount = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                       .Where(config => dtCurrentDate.IsBetween(config.HolidayStartDate, config.HolidayEndDate))
                       .Count();

        if (iRecordCount != 0)
        {
            asSelect = asSelect.Replace("id = 'spn2' style= 'color:blue'", "id = 'spn2' style= 'color:red;font-weight:bold'");
            hidHolidays.Value = hidHolidays.Value + "$" + aiDayNumber;
        }
    }

   

    private string SetHolidayMark(int aiDayNumber, string asSelect, DateTime dtCurrentDate, DateTime dtCurrDate, bool abIsPrecondition)
    {
        List<StaffHolidaysSalaryDeduction> lstConfigs;
        if (abIsPrecondition)
        {
            lstConfigs = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                                .Where(config => dtCurrDate.IsBetween(config.HolidayStartDate, config.HolidayEndDate)
                                                && config.HolidayStartDate.Year == dtCurrDate.Year).ToList();
        }
        else
        {
            lstConfigs = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                     .Where(config => dtCurrDate.IsBetween(config.HolidayStartDate, config.HolidayEndDate)
                     && config.HolidayEndDate.Year == dtCurrDate.Year).ToList();
        }

        if (lstConfigs.Count() > 0)
        {
            bool bInclude = true;
            StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction = lstConfigs.FirstOrDefault();

            bInclude = CheckSalaryDeductionStatus(oStaffHolidaysSalaryDeduction, bInclude);

            if (bInclude)
            {
                asSelect = asSelect.Replace("id = 'spn2' style= 'color:blue'", "id = 'spn2' style= 'color:black;font-weight:bold'");
                hidHolidayDates.Value = hidHolidayDates.Value + "," + aiDayNumber;

                SetStafHolidayDates(aiDayNumber, dtCurrentDate, oStaffHolidaysSalaryDeduction, abIsPrecondition);
                miOldHolidatId = oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId;

                hidConfigIds.Value = hidConfigIds.Value + "," + oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId;
            }
        }
        return asSelect;
    }

    private bool GetWeekendConfig(DateTime adtCurrDate)
    {
        bool bIsConfigWeekend = moDatewiseStaffLeavesBL.WeekDays
                                .Where(config => config.IsWeekend &&
                                                (config.OriginalWeekDaysId == Convert.ToInt32(adtCurrDate.DayOfWeek) ||
                                                config.OriginalWeekDaysId - 7 == Convert.ToInt32(adtCurrDate.DayOfWeek))
                                      )
                                .Count() > 0 ? true : false;

        if (bIsConfigWeekend)
        {
            if (moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations.FindAll(shld => shld.IsWeekend).Count > 0)
                bIsConfigWeekend = false;

            var dtHolidayStartDate = moDatewiseStaffLeavesBL.StaffHolidayAndSalaryDeductionConfigurations
                                                    .Where(config => (config.HolidayStartDate.Day == adtCurrDate.Day || config.HolidayEndDate.Day == adtCurrDate.Day)
                                                    && config.HolidayStartDate.Month == adtCurrDate.Month
                                                    && config.HolidayStartDate.Year == adtCurrDate.Year);
            if (dtHolidayStartDate.Count() > 0)
                bIsConfigWeekend = false;
        }
        return bIsConfigWeekend;
    }

    private bool CheckSalaryDeductionStatus(StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction, bool bInclude)
    {
        if (oStaffHolidaysSalaryDeduction.Type != 2)
        {
            DateTime dtFirstDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), 1);
            DateTime dtLastDate = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), DateTime.DaysInMonth(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value)));

            var lstUsersSalaryDeductions = moDatewiseStaffLeavesBL.UsersSalaryDeductions
                                                 .Where(UsersSD => ((new DateTime(UsersSD.Year, UsersSD.MonthId, 1) < dtFirstDate) ||
                                                                     (new DateTime(UsersSD.Year, UsersSD.MonthId, 1) > dtLastDate)) &&
                                                                     UsersSD.StaffHolidayAndLeavesConfigurationId == oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId
                                                       );
            bInclude = lstUsersSalaryDeductions.Count() == 0;
        }
        return bInclude;
    }

    private void SetStafHolidayDates(int aiDayNumber, DateTime dtCurrentDate, StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction, bool abIsPreCondition)
    {
        if (oStaffHolidaysSalaryDeduction.Type == 2)
        {
            if (oStaffHolidaysSalaryDeduction.HolidayStartDate.Month == oStaffHolidaysSalaryDeduction.HolidayEndDate.Month && dtCurrentDate.Month == oStaffHolidaysSalaryDeduction.HolidayStartDate.Month)
            {
                StringBuilder oStringBuilder = new StringBuilder();

                if (oStaffHolidaysSalaryDeduction.HolidayEndDate.Day == DateTime.DaysInMonth(oStaffHolidaysSalaryDeduction.HolidayEndDate.Year, oStaffHolidaysSalaryDeduction.HolidayEndDate.Month))
                    hidEnclosedDates.Value = hidEnclosedDates.Value + "$" + aiDayNumber + ",#";
                else if (oStaffHolidaysSalaryDeduction.HolidayStartDate.Day == 1)
                    hidEnclosedDates.Value = hidEnclosedDates.Value + "$" + "#," + aiDayNumber;
                else
                {
                    if (abIsPreCondition)
                        mlstHolidayStartingDays.Add(new StaffHoliday{  ConfigId = oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId, Day = aiDayNumber });                     
                    else
                    {
                        if (dtCurrentDate < oStaffHolidaysSalaryDeduction.HolidayStartDate)
                            mlstHolidayStartingDays.Add(new StaffHoliday { ConfigId = oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId, Day = aiDayNumber });
                        else
                        {   
                            mlstHolidayStartingDays.Where(sh => sh.ConfigId == oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId).ToList().ForEach(stDay =>
                            {
                                oStringBuilder.Append("$" + stDay.Day + "," + aiDayNumber);
                            });

                            mlstHolidayStartingDays.RemoveAll(sh => sh.ConfigId == oStaffHolidaysSalaryDeduction.StaffHolidaysSalaryDeductionId);

                            if (oStringBuilder.ToString().StartsWith("$"))
                                hidEnclosedDates.Value = hidEnclosedDates.Value + "$" + oStringBuilder.ToString().Substring(1);
                            else
                                hidEnclosedDates.Value = hidEnclosedDates.Value + "$" + oStringBuilder.ToString();
                        }
                    }
                }
            }
            else
            {
                if (oStaffHolidaysSalaryDeduction.HolidayStartDate < dtCurrentDate)
                    hidEnclosedDates.Value = hidEnclosedDates.Value + "$"  +"#," + aiDayNumber;
                else
                    hidEnclosedDates.Value = hidEnclosedDates.Value + "$" + aiDayNumber + ",#";
            }
            if (hidEnclosedDates.Value.StartsWith("$"))
                hidEnclosedDates.Value = hidEnclosedDates.Value.Substring(1);
        }
        else
        {
            if (oStaffHolidaysSalaryDeduction.HolidayStartDate.Month == Convert.ToInt32(hidMonthId.Value) || oStaffHolidaysSalaryDeduction.HolidayEndDate.Month == Convert.ToInt32(hidMonthId.Value))
                hidAttachedDates.Value = hidAttachedDates.Value + "," + aiDayNumber;
            else if ((oStaffHolidaysSalaryDeduction.HolidayEndDate.AddDays(1) == dtCurrentDate && aiDayNumber == 1) || (oStaffHolidaysSalaryDeduction.HolidayStartDate.AddDays(-1) == dtCurrentDate && aiDayNumber == DateTime.DaysInMonth(dtCurrentDate.Year, dtCurrentDate.Month)))
                hidAttachedDates.Value = hidAttachedDates.Value + "," + aiDayNumber;
        }
    }

    /// <summary>
    /// This method is used to return combobox options html code.
    /// </summary>
    /// <param name="lstDatewiseLeaves"></param>
    /// <param name="oDatewiseStaffLeavesBL"></param>
    /// <returns></returns>
    private string GetLeavesInHtmlFormat(List<DaywiseLeaves> lstDatewiseLeaves, DatewiseStaffLeavesBL oDatewiseStaffLeavesBL, out string asColorCode)
    {
        string sColorCode = "White";
        List<DaywiseLeaves> lstLeaves = (from leave in oDatewiseStaffLeavesBL.StaffLeaves
                                         join datewiseLeave in lstDatewiseLeaves
                                         on leave.LeaveId equals datewiseLeave.LeaveId into DatewiseStaffLeaves
                                         from DatewiseStaffLeave in DatewiseStaffLeaves.DefaultIfEmpty()
                                         orderby leave.OriginalLeaveId ascending
                                         select new DaywiseLeaves
                                         {
                                             LeaveId = leave.LeaveId,
                                             ShortName = leave.ShortName,
                                             OriginalLeaveId = leave.OriginalLeaveId,
                                             IsSelected = (DatewiseStaffLeave == null ? false : true),
                                             IsUnPaidLeave = leave.IsUnPaidLeave,
                                             ColorCode = leave.ColorCode
                                         }).ToList();

        string sOption = string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();

        lstLeaves.ForEach(leave =>
        {
            sOption = string.Format("<option value='{0}' ", leave.LeaveId);
            if (leave.IsSelected)
            {
                sOption = string.Format("{0}selected='selected'", sOption);
                sColorCode = leave.ColorCode;
            }

            sOption = string.Format("{0}style= 'background-color:" + leave.ColorCode + "'", sOption);

            sOption = string.Format("{0}>{1}</option>", sOption, leave.ShortName);
            oStringBuilder.Append(sOption);
        });
        asColorCode = sColorCode;
        return oStringBuilder.ToString();
    }

    /// <summary>
    /// This method is used to set leave balance text;
    /// </summary>
    private void SetLeaveBalanceText()
    {
        ViewState[S_CONFIGURED_LEAVE_TYPE] = moDatewiseStaffLeavesBL.StaffLeaves;
        var lstLeaveBalance = from leave in moDatewiseStaffLeavesBL.StaffLeaves
                              join attendance in moDatewiseStaffLeavesBL.StaffLeaveDetails
                              on leave.LeaveId equals attendance.LeaveId into StaffLeaves
                              from StaffLeave in StaffLeaves.DefaultIfEmpty()
                              where leave.IsUnPaidLeave == false
                              select new DaywiseLeaves
                              {
                                  LeaveId = leave.LeaveId,
                                  ShortName = leave.ShortName,
                                  LeaveBalance = (StaffLeave == null ? 0 : StaffLeave.Days),
                                  SortOrder = leave.SortOrder,
                                  OriginalLeaveId = leave.OriginalLeaveId,
                                  IsUnPaidLeave = leave.IsUnPaidLeave,
                                  ExcludeFromSalaryDeduction = leave.ExcludeFromSalaryDeduction
                              };

        List<DaywiseLeaves> lstTotalLeaveBalance = (from leave in lstLeaveBalance
                                                    join balanceLeave in moDatewiseStaffLeavesBL.UserLeavesYearwiseConfigurations
                                                    on leave.LeaveId equals balanceLeave.LeaveId into BalanceLeaves
                                                    from BalanceLeave in BalanceLeaves.DefaultIfEmpty()
                                                    select new DaywiseLeaves
                                                    {
                                                        LeaveId = leave.LeaveId,
                                                        ShortName = leave.ShortName,
                                                        LeaveBalance = leave.LeaveBalance + (BalanceLeave == null ? 0 : BalanceLeave.LeaveBalance),
                                                        SortOrder = leave.SortOrder,
                                                        OriginalLeaveId = leave.OriginalLeaveId,
                                                        IsUnPaidLeave = leave.IsUnPaidLeave,
                                                        MinimumBalance = (BalanceLeave == null ? 0 : BalanceLeave.MinimumBalance),
                                                        ExcludeFromSalaryDeduction = leave.ExcludeFromSalaryDeduction
                                                    }).ToList();
        ViewState[S_LEAVE_TABLE] = lstTotalLeaveBalance;
        lstTotalLeaveBalance = (from balance in lstTotalLeaveBalance
                                join lateMarkLeave in moDatewiseStaffLeavesBL.LateMarkLeaves
                                on balance.LeaveId equals lateMarkLeave.LeaveId into totalLeaves
                                from leave in totalLeaves.DefaultIfEmpty()
                                select new DaywiseLeaves
                                {
                                    LeaveId = balance.LeaveId,
                                    ShortName = balance.ShortName,
                                    LeaveBalance = balance.LeaveBalance + (leave == null ? 0 : leave.Days),
                                    SortOrder = balance.SortOrder,
                                    OriginalLeaveId = balance.OriginalLeaveId,
                                    IsUnPaidLeave = balance.IsUnPaidLeave,
                                    MinimumBalance = balance.MinimumBalance
                                }).ToList();

        ViewState[S_LATE_MARK_LEAVES] = moDatewiseStaffLeavesBL.LateMarkLeaves;
        ViewState[S_LATE_MARK_CONFIG] = moDatewiseStaffLeavesBL.LateMarkConfigurations;

        StringBuilder oLeaveBalance = new StringBuilder();
        StringBuilder oUsedLeaves = new StringBuilder();
        StringBuilder oRequiredLeaves = new StringBuilder();

        if (moDatewiseStaffLeavesBL.UserLeavesYearwiseConfigurations.Count == 0)
            lstTotalLeaveBalance.ForEach(leave => leave.LeaveBalance = 0);

        lstTotalLeaveBalance.ForEach(leave => oLeaveBalance.Append(string.Format(", {0}({1})", leave.ShortName, Math.Round(leave.LeaveBalance, 1))));

        if (oLeaveBalance.ToString().StartsWith(", "))
            lblLeaveBalance.Text = oLeaveBalance.ToString().Substring(1);

        List<ConfiguredLeaves> olstConfiguredLeaves = null;
        if (ViewState[S_CONFIGURED_LEAVE] != null)
            olstConfiguredLeaves = ViewState[S_CONFIGURED_LEAVE] as List<ConfiguredLeaves>;

        olstConfiguredLeaves.ForEach
            (
                leave =>
                {
                    if (leave.LeaveId != 0)
                    {
                        oUsedLeaves.Append(string.Format(", {0}(0.0)", leave.ShortName));
                        if (!leave.IsUnpaidLeave)
                            oRequiredLeaves.Append(string.Format(", {0}({1})", leave.ShortName, Math.Round(Convert.ToDecimal(leave.MinimumBalance), 1)));
                    }
                }
            );

        if (oLeaveBalance.ToString().StartsWith(", "))
        {
            lblUsedLeaves.Text = oUsedLeaves.ToString().Substring(1);
            lblRequiredBalamce.Text = oRequiredLeaves.ToString().Substring(1);
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() != String.Empty)
        {
            hidMonthId.Value = QueryString["MonthId"];
            hidYear.Value = QueryString["Year"];
            hidStaffGroup.Value = QueryString["StaffGroupId"];
            if (QueryString["UserId"] != null)
                cmbUsers.SelectedValue = hidSelectedUserId.Value = hidUserId.Value = QueryString["UserId"];
            if (QueryString["PartialLeave"] != null)
                hidPartialLeaves.Value = QueryString["PartialLeave"];
            if (QueryString["Filter"] != null)
                hidFilter.Value = QueryString["Filter"];
        }
    }

    /// <summary>
    /// This method is used to generate leave xml.
    /// </summary>
    /// <returns></returns>
    private string GenerateXml()
    {
        string[] sArrDays = hidDay.Value.Split('$');
        string[] sArrLeaves = hidLeave.Value.Split('$');
        string[] sArrHalfLeaves = hidHalfLeaves.Value.Split('$');
        string[] sArrLateMarks = hidLateMarkDays.Value.Split('$');
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("UserLeaves");
        XmlNode rootNode = oDoc.CreateNode("element", "UserLeaves", string.Empty);

        if (sArrDays.Count() == 1 && sArrDays[0] == string.Empty && sArrLateMarks.Count() == 1 && sArrLateMarks[0] == string.Empty)
            return string.Empty;

        Dictionary<string, string> dictLeaves = new Dictionary<string, string>();
        for (int iDay = 0; iDay < sArrDays.Length; iDay++)
            dictLeaves.Add(sArrDays[iDay], sArrLeaves[iDay]);

        int iDayOfMonth = DateTime.DaysInMonth(LeaveCalendar.SelectedDate.Year, LeaveCalendar.SelectedDate.Month);
        string sLeaveId;
        string sIsHalfLeave;
        string sDate;
        for (int iDayIndex = 1; iDayIndex <= iDayOfMonth; iDayIndex++)
        {
            if (dictLeaves.ContainsKey(iDayIndex.ToString()) || sArrLateMarks.Contains(iDayIndex.ToString()))
            {
                XmlNode oNode = oDoc.CreateNode("element", "UserLeaves", string.Empty);

                sLeaveId = "0";
                sIsHalfLeave = "0";
                sDate = new DateTime(LeaveCalendar.VisibleDate.Year, LeaveCalendar.VisibleDate.Month, iDayIndex).ToString();
                if (dictLeaves.ContainsKey(iDayIndex.ToString()))
                {
                    sLeaveId = dictLeaves[iDayIndex.ToString()];
                    sIsHalfLeave = sArrHalfLeaves.Contains(iDayIndex.ToString()) ? "1" : "0";
                }

                XmlAttribute attr = oDoc.CreateAttribute("Date");
                attr.Value = sDate;
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("LeaveId");
                attr.Value = sLeaveId;
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("IsHalfLeave");
                attr.Value = sIsHalfLeave;
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("IsLateMark");
                attr.Value = sArrLateMarks.Contains(iDayIndex.ToString()) ? "1" : "0";
                oNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("IsPartialLeave");
                attr.Value = "0";
                oNode.Attributes.Append(attr);

                rootNode.AppendChild(oNode);
            }
        }

        Dictionary<int, int> partialLeaveDictionary = new Dictionary<int, int>();
        if (ViewState[S_PARTIAL_LEAVES] != null)
            partialLeaveDictionary = ViewState[S_PARTIAL_LEAVES] as Dictionary<int, int>;

        foreach (KeyValuePair<int, int> kvp in partialLeaveDictionary)
        {
            XmlNode oNode = oDoc.CreateNode("element", "UserLeaves", string.Empty);

            sDate = new DateTime(LeaveCalendar.VisibleDate.Year, LeaveCalendar.VisibleDate.Month, kvp.Key).ToString();

            XmlAttribute attr = oDoc.CreateAttribute("Date");
            attr.Value = sDate;
            oNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("LeaveId");
            attr.Value = kvp.Value.ToString();
            oNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsHalfLeave");
            attr.Value = "1";
            oNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsLateMark");
            attr.Value = "0";
            oNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsPartialLeave");
            attr.Value = "1";
            oNode.Attributes.Append(attr);

            rootNode.AppendChild(oNode);
        }

        root.AppendChild(rootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to set Query string for OD details.
    /// </summary>
    private void SetQueryStringForOD()
    {
        string sQueryString = "&StaffGroupId=" + Convert.ToInt32(cmbStaffGroup.SelectedValue) +
                              "&Year=" + hidYear.Value +
                              "&UserId=" + Convert.ToInt32(cmbUsers.SelectedValue);
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }

    /// <summary>
    /// This method is used to display Pull Biometric data Button.
    /// </summary>
    private void DisplayPullBiometricData()
    {
        if (SchoolBase.Settings.IsBiometriceEnabled == true)
        {
            btnPullBioData.Visible = true;
        }
        else
            btnPullBioData.Visible = false;
    }

    #endregion

    #region Inner Class

    class UsedLeaves
    {
        public int LeaveId { get; set; }
        public decimal LeaveCount { get; set; }
    }

    class StaffHoliday
    {
        public int Day { get; set; }
        public int ConfigId { get; set; }
    }

    #endregion
}