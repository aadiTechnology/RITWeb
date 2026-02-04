using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using PayrollEntities;
using System.Threading;

public partial class StaffInOutDetailsPopup : SchoolBase
{
    #region Page Events

    /// <summary>
    /// This event is used to load data on page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillComboBox();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used change combobox value of staff group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserComboBox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used display staff In out details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>  
    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        try
        {
            SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
            List<StaffInOutDetails> lstStaffInOutDetails = new List<StaffInOutDetails>();
            List<EmployeeNoDetails> lstEmployeeNoDetails = new List<EmployeeNoDetails>();

            lstStaffInOutDetails = oSalaryDetailsBL.GetUserInOutDetails(miSchoolId, miAcademicYearId, cmbUserName.SelectedValue.ToString(), cmbStaffGroup.SelectedValue.ToInt(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), out lstEmployeeNoDetails);

            if (lstStaffInOutDetails.Count > Constants.I_ZERO)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=StaffInOutDetails.xls");
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");

                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
                HttpContext.Current.Response.Write("<TR>");

                AddHeader("Sr No.", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Employee No.", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Staff Name", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Date", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Timing", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Time Difference", "text-align:center; font-weight:bold; font-size:17px;");
                HttpContext.Current.Response.Write("</TR>");

                AddUserInoutDetails(lstStaffInOutDetails, lstEmployeeNoDetails);

                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
        catch( ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This Method is used to fill User role combobox.
    /// </summary>
    private void FillComboBox()
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffGroup, "StaffGroupsName", "StaffGroupsId", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This Method is used to fill User combobox.
    /// </summary>
    private void FillUserComboBox()
    {
        StaffLeaveDetailsBL oStaffLeaveDetailsBL = new StaffLeaveDetailsBL();
        List<UserBasicDetails> lstUserBasicDetails = oStaffLeaveDetailsBL.GetAllUsersForODDetails(cmbStaffGroup.SelectedValue.ToInt(), miSchoolId, miAcademicYearId, DateTime.Now.Year.ToInt(), true);
        ListSource.FillDropDownList(lstUserBasicDetails, cmbUserName, "StaffName", "EmployeeNo", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used for Adding the row Header in to Table for.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

    /// <summary>
    /// This method is used for Adding the users inout details.
    /// </summary>
    private void AddUserInoutDetails(List<StaffInOutDetails> lstStaffInOutDetails, List<EmployeeNoDetails> lstEmploeeNos)
    {
        List<DateTime> dtInDate = lstStaffInOutDetails.Select(dt => dt.InDateTime.Date).Distinct().ToList();

        var oDateDetails = (from emp in lstEmploeeNos
                            join tm in lstStaffInOutDetails
                            on emp.EmployeeNo equals tm.EmployeeNo
                            select new
                            {
                                SrNo = emp.SrNo,
                                EmployeeNo = emp.EmployeeNo,
                                UserName = tm.UserName,
                                Time = tm.InDateTime.ToString("hh:mm tt"),
                                ActualTime = tm.InDateTime,
                                Date = tm.InDateTime.Date
                            }).ToList();

        if (oDateDetails != null && oDateDetails.Count > 0)
        {
            dtInDate.OrderBy(date => date).ToList().ForEach
            (
                dt =>
                {
                    int aiSerialNo = Constants.I_ONE;
                    Dictionary<string, DateTime> lastTime = new Dictionary<string, DateTime>();

                    oDateDetails.Where(s => s.Date == dt)
                                .OrderBy(s => s.SrNo)
                                .ThenBy(s => s.ActualTime)
                                .ToList()
                                .ForEach
                    (
                        dtm =>
                        {
                            string timeDiff = "";
                            string key = dtm.EmployeeNo + "_" + dt.ToString("yyyyMMdd");

                            if (lastTime.ContainsKey(key))
                            {
                                TimeSpan diff = dtm.ActualTime - lastTime[key];
                                timeDiff = ((int)diff.TotalHours).ToString() + " Hr " + diff.Minutes + " Mn";
                                lastTime[key] = dtm.ActualTime;
                            }
                            else
                            {
                                lastTime[key] = dtm.ActualTime;
                            }

                            HttpContext.Current.Response.Write("<TR>");

                            AddTableRows(aiSerialNo.ToString(), "text-align:center");
                            AddTableRows(dtm.EmployeeNo, "text-align:left");
                            AddTableRows(dtm.UserName, "text-align:left; vertical-align:middle");
                            AddTableRows(dtm.Date.ToString(Constants.S_DATE_FORMAT), "text-align:center");
                            AddTableRows(dtm.Time, "text-align:center");
                            AddTableRows(timeDiff, "text-align:center");  
                            HttpContext.Current.Response.Write("</TR>");

                            aiSerialNo++;
                        }
                    );

                    HttpContext.Current.Response.Write("<TR>");
                    AddTableRows(string.Empty, string.Empty, 6);
                    HttpContext.Current.Response.Write("</TR>");
                }
            );
        }
    }

    /// <summary>
    /// This method is used for Adding the rows in to Table for exporting.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "", int aiColSpan=1)
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        string sColSpan = string.Empty;
        if (aiColSpan > 0)
            sColSpan = "colspan='" + aiColSpan + "' ";

        HttpContext.Current.Response.Write("<TD "+sColSpan+ sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }

    /// <summary>
    /// This Method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnDisplay, btnBack});

        txtStartDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);   
    }

    #endregion
}