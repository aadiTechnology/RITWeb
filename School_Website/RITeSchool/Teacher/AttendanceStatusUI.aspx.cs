// File Name  : StandardDivisionAttendanceStatus.aspx.cs
// Created By : Pravin
// Date       : 23 MAy 2012
// Description: This class is used to getting the Attendance status for standard divisions on selected date.

using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;
using System.Globalization;

/// <summary>
/// This class is used to getting the Attendance status for standard divisions on selected date.
/// </summary>
public partial class AttendanceStatusUI : SchoolBase
{
    #region -- Costant(s) --

    private const string S_STANDARD = "Standard";
    private const string S_TOTAL = "Total";
    private const string S_ERROR_MESSAGE="Please select valid date.";    
    
    #endregion -- Costant(s) --

    #region -- Member(s) --

    private DayDetails moDayDetails;
    private List<ClasswiseAttendanceStatus> molstClasswiseAttendanceInfo;
		
    #endregion -- Member(s) --

    #region --Event(s)--

    /// <summary>
    /// This is used to fill the grid on pageload.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
			if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    SetDefaultDate();
                    FillAttendanceStatusGrid();
					tdimgAttendanceDone.Visible = tdlblAttendanceDone.Visible = hidShowCount.Value == Constants.S_YES;
                }                                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This is called upon selection changed of date control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cAttendDate_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (calTodaysDate.Text.IsValidDate())
            {
                if (Convert.ToDateTime(calTodaysDate.Text) < DateTime.Now)
                {
                    
                    SetControlsVisibility(true);
                    FillAttendanceStatusGrid();
                }
                else
                {
                    SetControlsVisibility(false);
                    lblError.Visible = true;
                    lblError.Text = Resources.LocalizedResources.AttendanceDateShouldNotBeAFutureDate;
                }
            }
            else
            {
                SetControlsVisibility(false);
                lblError.Text = Resources.LocalizedResources.DateErrorMsg;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called for binding cells and data to gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string sValue=string.Empty;
        try
        {
            TableCellCollection cells = e.Row.Cells;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in cells)                
                    cell.Style["width"] = "90px";
                
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string sHolidayName = string.Empty;
                bool bIsNonHeader = true;

                Dictionary<int, string> dctHolidays=new Dictionary<int,string>();
                var oHolidays = molstClasswiseAttendanceInfo.Where(Holiday => Holiday.HolidayName != string.Empty && Holiday.StandardName == e.Row.Cells[0].Text);
                   
                int iCellIndex = 0;
                if (oHolidays.Count() != 0)
                {
                    
                    foreach (TableCell cell in cells)
                    {                        
                        string sDivisionName = grdStandards.HeaderRow.Cells[iCellIndex].Text;			
						
                        sHolidayName = oHolidays.Where(holiday => holiday.DivisionName == sDivisionName).Select(Holiday => Holiday.HolidayName).FirstOrDefault();
                        dctHolidays[iCellIndex] =string.Empty;

						int iCellIndex1 = 0;
						if (hidShowCount.Value == Constants.S_YES)
							iCellIndex1 = cells.Count - 1;
						else
							iCellIndex1 = cells.Count ;
							int number;
						if (iCellIndex != 0 && iCellIndex != iCellIndex1 && cell.Text != "-2" && !sHolidayName.IsNullOrEmpty() && Int32.TryParse(cell.Text,out number))
                        {
                            cell.Text = "<font color='#9F0050'>Holiday</font>";                            
                            cell.ToolTip = sHolidayName;

                            dctHolidays[iCellIndex] = sHolidayName;
                            cell.CssClass = "class1";
                            if (IsPostBack)
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "showtooltip", "showtooltip()", true);
                            
                        }
                        iCellIndex++;                        
                    }       
             
                }
                bIsNonHeader = true;
                iCellIndex = 0;
                foreach (TableCell cell in cells)
                {
                    if (bIsNonHeader)
                    {
                        bIsNonHeader = false;
                        iCellIndex++;
                        continue;
                    }
					int number;
					Constants.AttendanceStatus status = Constants.AttendanceStatus.NoClassAvailable;
                    if (dctHolidays.Count!=0 && dctHolidays[iCellIndex] != string.Empty && cell.Text != Constants.AttendanceStatus.AtteandanceTaken.ToInt().ToString() && cell.Text != Constants.AttendanceStatus.NoClassAvailable.ToInt().ToString())
							status = Constants.AttendanceStatus.Other;
					else if (hidShowCount.Value == Constants.S_NO && ((cell.Text != Constants.AttendanceStatus.AtteandanceTaken.ToInt().ToString() && cell.Text != Constants.AttendanceStatus.NoClassAvailable.ToInt().ToString() && cell.Text != Constants.AttendanceStatus.AtteandanceNotTaken.ToInt().ToString()
                        && cell.Text != Constants.AttendanceStatus.OutsideAcademicYear.ToInt().ToString()
                        && !HttpUtility.HtmlDecode(cell.Text.Trim()).Trim().IsNullOrEmpty()) ||
								((!Int32.TryParse(cell.Text, out number) && !HttpUtility.HtmlDecode(cell.Text.Trim()).Trim().IsNullOrEmpty()) || cell.Text == Constants.AttendanceStatus.AtteandanceTaken.ToInt().ToString())))
						status = Constants.AttendanceStatus.AtteandanceTaken;
					else if (!HttpUtility.HtmlDecode(cell.Text.Trim()).Trim().IsNullOrEmpty())
								status = (Constants.AttendanceStatus)cell.Text.ToInt();
					   
                    switch (status)
                    {
						case Constants.AttendanceStatus.AtteandanceTaken:
							if (hidShowCount.Value == Constants.S_YES)
							{
								Image img = new Image();
								img.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
								img.ImageAlign = ImageAlign.Middle;
								cell.HorizontalAlign = HorizontalAlign.Center;
								cell.Controls.Add(img);
							}
							else
							{
								cell.ForeColor = System.Drawing.Color.Black;
								cell.Style.Add("font-weight", "bold");
							}


							break;

                        case Constants.AttendanceStatus.AtteandanceNotTaken:                            
                            if (moDayDetails.IsWeekDay != Constants.S_NO)
                            {
                                cell.Text = "<font color='#201B7C'>Weekend</font>";
                                trHoliday.Visible = false;
                            }
                            else
                            {
                                Image imgDelete = new Image();
                                imgDelete.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";
                                imgDelete.ImageAlign = ImageAlign.Middle;
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.Controls.Add(imgDelete);
                                trHoliday.Visible = false;
                            }
                            break;
                       
                        case Constants.AttendanceStatus.OutsideAcademicYear:
                            cell.Style.Add("Background-color", "#FFCCFF");
                            cell.Text = "-";
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            if (moDayDetails.HolidayName != string.Empty)
                            {
                                //lblHoliday.Text = moDayDetails.HolidayName.ToString();
                                trHoliday.Visible = true;
                            }
                            else
                                trHoliday.Visible = false;
                            break;

                        case Constants.AttendanceStatus.NoClassAvailable:
                            cell.Text = string.Empty;
                            break;
                    }
                    cell.HorizontalAlign = HorizontalAlign.Center;

                    iCellIndex++;
                }

				if (hidShowCount.Value == Constants.S_YES)
				{
					e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
					e.Row.Cells[0].Style.Add("padding-left", "10px");
					e.Row.Cells[e.Row.Cells.Count - 1].Style.Add("font-weight", "bold");
					e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "TotalCount";				
				}
					
				
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion --Event(s)--

    #region --Private Method(s)--
  
    /// <summary>
    /// This method is used to fill Gridview according to selected date.
    /// </summary>
    private void FillAttendanceStatusGrid()
    {
        GetClasswiseAttendanceStatus();
        if (moDayDetails.OutSideAcademicYear == String.Empty)
        {
            int iDivisionCount;
            pnlGrid.Visible = true;
            lblError.Visible = false;   
            grdStandards.DataSource = GetClasswiseAttendanceDetails(out iDivisionCount);
            grdStandards.DataBind();
            SetTotalCountRow(iDivisionCount);
        }
        else
        {
            lblError.Text = moDayDetails.OutSideAcademicYear.Replace("Attendance date should be within current academic year  \r\n\t\t\t\t\t\t\t\t(i.e. between", Resources.LocalizedResources.ValAttendanceCurrentDateOutSide).Replace("to",Resources.LocalizedResources.To);
            lblError.Visible = true;                
            SetControlsVisibility(false);
        }
    }

    /// <summary>
    /// This method is used to return attendance details datatable.
    /// </summary>
    /// <param name="iDivisionCount"></param>
    /// <returns></returns>
    private DataTable GetClasswiseAttendanceDetails(out int aiDivisionCount)
    {
        int iIsAttendanceTaken;

        //Here we will get all the distinct divisions for school.
        var oDivisions = molstClasswiseAttendanceInfo.Select(division => new { division.DivisionId, division.DivisionName }).Distinct().ToList();
        aiDivisionCount = oDivisions.Count;

        //Here add a column for each standard as a division name.
        DataTable odtAttendance = new DataTable();
        odtAttendance.Columns.Add(S_STANDARD);
        oDivisions.ForEach(division => odtAttendance.Columns.Add(division.DivisionName));
		if (hidShowCount.Value == Constants.S_YES)
		    odtAttendance.Columns.Add(S_TOTAL);
		
			

        //Here we will get all the standnards present in school.
        var oStandards = molstClasswiseAttendanceInfo.Select(standard => new { standard.StandardId, standard.StandardName }).Distinct().ToList();

        //In this loop we are adding all standards-divisions with there attendnace status.
        foreach (var standard in oStandards)
        {
            DataRow oDataRow = odtAttendance.NewRow();
            oDataRow[S_STANDARD] = standard.StandardName;

            foreach (var division in oDivisions)
            {
                var oAttendance = molstClasswiseAttendanceInfo.Where(attendance => attendance.StandardId == standard.StandardId && attendance.DivisionId == division.DivisionId)
                                                            .Select(attendance => attendance.AttendanceTaken);
                
                iIsAttendanceTaken = Constants.AttendanceStatus.NoClassAvailable.ToInt();
                if (oAttendance.Count() != Constants.I_ZERO)
                    iIsAttendanceTaken = oAttendance.First();

				if (hidShowCount.Value == Constants.S_NO)
				{
					string sPresentStudentTotal = string.Empty;
					if (iIsAttendanceTaken == Constants.AttendanceStatus.AtteandanceTaken.ToInt() || iIsAttendanceTaken == Constants.AttendanceStatus.AtteandanceNotTaken.ToInt())
					{
						sPresentStudentTotal = molstClasswiseAttendanceInfo.Where(attendance => attendance.StandardId == standard.StandardId && attendance.DivisionId == division.DivisionId).First().PresentStudentWithTotal.ToString();
						oDataRow[division.DivisionName] = sPresentStudentTotal;
					}
					else 
                        oDataRow[division.DivisionName] = iIsAttendanceTaken;
				}
				else
					oDataRow[division.DivisionName] = iIsAttendanceTaken;

              
            }
			if (hidShowCount.Value == Constants.S_YES)
			{
				var oStandardCount = molstClasswiseAttendanceInfo.Where(attendance => attendance.StandardId == standard.StandardId && attendance.AttendanceTaken == Constants.AttendanceStatus.AtteandanceTaken.ToInt());
				oDataRow[S_TOTAL] = oStandardCount.Count();
		   }
            odtAttendance.Rows.Add(oDataRow);
        }

        DataRow odrAttendance = odtAttendance.NewRow();
        oDivisions.ForEach(division =>
        {
            var oDivisionCount = molstClasswiseAttendanceInfo.Where(attendance => attendance.DivisionId == division.DivisionId && attendance.AttendanceTaken == Constants.AttendanceStatus.AtteandanceTaken.ToInt());
            odrAttendance[division.DivisionName] = oDivisionCount.Count();
        });

		if (hidShowCount.Value == Constants.S_YES)
		{
			odrAttendance[S_STANDARD] = S_TOTAL;
			var oPresentCount = molstClasswiseAttendanceInfo.Where(attendance => attendance.AttendanceTaken == Constants.AttendanceStatus.AtteandanceTaken.ToInt());
			odrAttendance[S_TOTAL] = oPresentCount.Count();
		}
        odtAttendance.Rows.Add(odrAttendance);

        return odtAttendance;
    }
    
    /// <summary>
    /// This method is used to set total count row.
    /// </summary>
    /// <param name="aiDivisionCount"></param>
    private void SetTotalCountRow(int aiDivisionCount)
    {
        grdStandards.HeaderRow.Cells[0].Controls.Add(new Image { ImageUrl = Resources.LocalizedResources.HeaderURL });
        if (grdStandards.Rows.Count > 0)
        {
            int iStandardCount = grdStandards.Rows.Count - 1;
            trLegend.Visible = true;
			if (hidShowCount.Value == Constants.S_YES)
				grdStandards.Rows[iStandardCount].CssClass = "TotalCount";
			else
				grdStandards.Rows[iStandardCount].Visible = false;		
            grdStandards.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
            grdStandards.Rows[iStandardCount].Style.Add("font-weight", "bold");
			if (hidShowCount.Value == Constants.S_YES)
				grdStandards.Rows[iStandardCount].Cells[aiDivisionCount + 1].CssClass = "ClsHilightBG";
			else
				grdStandards.Rows[iStandardCount].Cells[aiDivisionCount ].CssClass = "ClsHilightBG";
        }
        else
            trLegend.Visible = false;
    }

    /// <summary>
    /// This function is used to Get the Attendance status.
	/// 
    /// </summary>
    private void GetClasswiseAttendanceStatus()
    {
        DateTime dtSelectedDate = calTodaysDate.Text.ToDateTime();
        string sDate = dtSelectedDate.ToString(Constants.S_DATE_FORMAT_MARATHI );
        calTodaysDate.Text = dtSelectedDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
        AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
        molstClasswiseAttendanceInfo = oAttendanceDetailsBL.Get(miSchoolId, miAcademicYearId, sDate);
        moDayDetails = oAttendanceDetailsBL.DayDetails;
    }

    /// <summary>
    /// This function is used to set default date for the page.
    /// </summary>
    private void SetDefaultDate()    
	{
		if (QueryString.Count > 0 && QueryString["SelectedDate"] != null && QueryString["ShowCount"] != null)
		{
		     calTodaysDate.Text = QueryString["SelectedDate"];	           
             hidShowCount.Value = QueryString["ShowCount"];
		}

        ApplyMouseHoverEffect(new List<Button>() { btnBack   });
        HtmlForm iForm = (HtmlForm)this.Master.FindControl("form1");
        iForm.DefaultButton = btnBack.UniqueID;
        btnBack.Focus();
    }

    /// <summary>
    /// This function checks the preconditons of RemarkTemplates.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AttendanceStatus);

        if (string.IsNullOrEmpty(sLinks))
        {
            divErr.Visible = false;
            bReturn = true;
            trDate.Visible = true;
            grdStandards.Visible = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            grdStandards.Visible = false;
            trDate.Visible = false;
        }
        return bReturn;
    }

    /// <summary>
    /// THis method is used to set controls visibility.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetControlsVisibility(bool abAction)
    {
        pnlGrid.Visible = abAction;
        trHoliday.Visible = abAction;
        trLegend.Visible = abAction;
    }

    #endregion --Private Methods--
}