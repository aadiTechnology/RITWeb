/* ------------------------------------------------------------------------
 *  FileName	: EventOverview.aspx.cs
 *  Author		: Vishal B. Shah
 *  Date		: 19-Jan-2012
 *  Purpose		: Displays an overview of the Annual events of the School.
 * ------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities;
using Utility;

/// <summary>
/// Displays an overview of the Annual events of the School.
/// </summary>
public partial class EventOverview : SchoolBase
{
	#region -- EVENT(s) --
	
	/// <summary>
	/// This event is used to Initialize controls on Page load.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{			            
			if (!IsPostBack)
			{
				FillAcademicYearList();
				FillStandardsList();
                SetStudentView();
				FillMonthsList();
				DisplayEvents();
				Initialize();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

	/// <summary>
	/// This event is used process the Events grid to set rowspan for table cells.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwEvents_DataBound(object sender, EventArgs e)
	{
		try
		{
			ProcessGrid("lblMonth", Constants.I_ZERO, true);
			ProcessGrid("lblDay", Constants.I_ONE, false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// Common event handler to rebind the Events grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			// If the Control that raised this event is ddlAcademicYears dropdown list,
			// Then we need to rebind the Standards dropdown list, since the Standard_Id will be difference for the newly selected AcademicYear.
			DropDownList ddlSender = sender as DropDownList;
			if (ddlSender.ID == ddlAcademicYears.ID)
				FillStandardsList();

			DisplayEvents();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT(s) --

	#region -- METHOD(s) --

	/// <summary>
	/// Populates the Standards dropdown list.
	/// </summary>
	private void FillStandardsList()
	{
		int iAcademicYearId = Convert.ToInt32(ddlAcademicYears.SelectedValue);
		StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, iAcademicYearId);
		DataTable oDtStandards = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandards, ref ddlStandards, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT_ALL);		
	}

    private void SetStudentView()
    {
        if (moUserRole == Constants.UserRoles.Student && miAcademicYearId == Convert.ToInt32(ddlAcademicYears.SelectedValue))
            ddlStandards.SelectedValue = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString();
        else
            ddlStandards.SelectedIndex = 0;
    }

	/// <summary>
	/// Populates the Months dropdown list.
	/// </summary>
	private void FillMonthsList()
	{
        List<MonthMaster> olstMonths = SchoolWiseAcademicYearMasterBL.GetAllMonth();
        ListSource.FillDropDownList(olstMonths, ddlMonths, "Month", "MonthID", Constants.S_SELECT_ALL);
	}

	/// <summary>
	/// Populates the Academic Year drop down list.
	/// </summary>
	private void FillAcademicYearList()
	{
		SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId, miUserId, moUserRole.ToInt());
        ControlUtility.FillDropDownList(oDtYearInfo, ref ddlAcademicYears, "Academic_Year_ID", "YearValue", String.Empty);
		ddlAcademicYears.SelectedValue = miAcademicYearId.ToString();
	}

	/// <summary>
	/// Binds the events ListView.
	/// </summary>
	private void DisplayEvents()
	{
		int iStandardId = Convert.ToInt32(ddlStandards.SelectedValue);
		int iMonthId = Convert.ToInt32(ddlMonths.SelectedValue);
		int iAcademicYearId = 0;
		if (tdAcademicYearlbl.Visible && tdAcademicYearsddl.Visible)
			iAcademicYearId = Convert.ToInt32(ddlAcademicYears.SelectedValue);
		else
			iAcademicYearId = miAcademicYearId;

		List<Event> lstEvents = SchoolEventBL.GetAllEvents(miSchoolId, iAcademicYearId, iStandardId, iMonthId);
		
		// We create a new anonymous type using LINQ.
		// This is done to avoid implementing the ItemDataBound event.
        var olstEvents = (from _event in lstEvents
                          select new
                          {
                              EventId = _event.EventId,
                              EventDescription = _event.EventDescription,
                              Month = String.Format("{0:MMMM} {0:yyyy}", _event.StartDate),
                              Day = (_event.StartDate != _event.EndDate) ? String.Format("{0} " + Resources.LocalizedResources.To + " {1}", _event.StartDate.ToString("ddd. d MMM"), _event.EndDate.ToString("ddd. d MMM")).Replace("..",".")
                                                                         : _event.StartDate.ToString("ddd. d").Replace("..", "."),
                              Standards = _event.Standards
                          }).ToList();

		lstvwEvents.DataSource = olstEvents;
		lstvwEvents.DataBind();
	}

	/// <summary>
	/// Common method to process the ListView rows to set rowspan for repeating items.
	/// </summary>
	/// <param name="sControlId">Id of the control to check repetition.</param>
	/// <param name="iIndex">Index of the cell in the row, to set rowspan.</param>
	private void ProcessGrid(string sControlId, int iIndex, bool bSetClass)
	{
		string sContent = String.Empty;
		int iCount = 0;
		ListViewDataItem oCurrent = null;
		string sClassName = "ClsGridAltRow";
			
		foreach (ListViewDataItem item in lstvwEvents.Items)
		{
			Label lblLabel = item.FindControl(sControlId) as Label;
			if (lblLabel.Text == sContent)
			{
				iCount++;
				HtmlTableRow oHTMLCurrentRow = item.FindControl("trGridRow") as HtmlTableRow;
				oHTMLCurrentRow.Cells[iIndex].Style["display"] = "none";
				if (bSetClass)
					oHTMLCurrentRow.Attributes["class"] = sClassName;
				lblLabel.Text = String.Empty;
				continue;
			}
			else
			{
				if (iCount != 0)
				{
					HtmlTableRow oHTMLCurrentRow = oCurrent.FindControl("trGridRow") as HtmlTableRow;
					oHTMLCurrentRow.Cells[iIndex].Attributes["rowspan"] = (iCount + 1).ToString();
					iCount = 0;
				}

				sClassName = (sClassName == "ClsGridRow") ? "ClsGridAltRow" : "ClsGridRow";
				
				if (bSetClass)
				{
					HtmlTableRow oHTMLTableRow = item.FindControl("trGridRow") as HtmlTableRow;
					oHTMLTableRow.Attributes["class"] = sClassName;
				}

				oCurrent = item;
				sContent = lblLabel.Text;
			}
		}

		if (oCurrent != null)
		{
			HtmlTableRow oTableRow = oCurrent.FindControl("trGridRow") as HtmlTableRow;
			if (iCount != 0)
				oTableRow.Cells[iIndex].Attributes["rowspan"] = (iCount + 1).ToString();
			if (bSetClass)
				oTableRow.Attributes["class"] = sClassName;  
		}
	}

	/// <summary>
	/// Sets the hover effect for buttons on the page.
	/// </summary>
	private void Initialize()
	{
        ApplyMouseHoverEffect(new List<Button> { btnClose });        
	}

	#endregion -- METHOD(s) --

}