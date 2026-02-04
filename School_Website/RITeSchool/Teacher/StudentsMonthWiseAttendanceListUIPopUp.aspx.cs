using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// "This class will display list of students and allows user to edit or add student information."
/// </summary>
public partial class StudentsMonthWiseAttendanceListUIPopUp : SchoolBase
{
    #region constants

    const string S_BLANK_GRID_MESSAGE = "No attendance records available.";

    #endregion

    #region event handlers


    /// <summary>
    /// This function is called when the page gets loaded
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
                InitialiseAttributes();
                SetControlsDefaultValues();
            }
            grdStudents.DataSourceID = GrdDSobj.ID;
            if (grdStudents.Rows.Count == 0)
            {
                grdStudents.Visible = false;
                trNoRecordFound.Visible = true;
            }
            else
                trNoRecordFound.Visible = false;
            ApplyMouseHoverEffect(new List<Button> { btnBack });
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


    #region grid events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            e.SortExpression = "[" + e.SortExpression + "]";
            hidSortExpression.Value = e.SortExpression;
            e.Cancel = true;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            //GridViewRow pagerRow = grdvwTeacherDetails.BottomPagerRow;
            GridViewRow pagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            grdStudents.DataSourceID = GrdDSobj.ID;

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Add a sort direction image to the appropriate column header
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);

                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    cell.Style.Add(HtmlTextWriterStyle.Padding, " 0 5px 0 5px");
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    TableCell cell = e.Row.Cells[i];
                    cell.Style.Add(HtmlTextWriterStyle.Padding, " 0 5px 0 5px");
                    if (i != 3)
                        cell.HorizontalAlign = HorizontalAlign.Center;
                    else
                        cell.HorizontalAlign = HorizontalAlign.Left;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {

                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdStudents.PageCount; i++)
                    {

                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdStudents.PageIndex)
                        {
                            item.Selected = true;
                        }

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);

                    }
                }
                if (pageLabel != null)
                {

                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = Resources.LocalizedResources.PageNo + " " + currentPage.ToString() + " " + Resources.LocalizedResources.Of + " " + grdStudents.PageCount.ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        trTotalRec.Visible = e.ReturnValue.ToString() != "0";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region grid commands

    protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStudents.PageIndex = e.NewPageIndex;
            grdStudents.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            EncodeHtmlCells(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    #endregion

    #region helper methods

    /// <summary>
    /// This function initialises the sort variables
    /// </summary>
    private void SetControlsDefaultValues()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This function is used to set sort variables
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    private void InitialiseAttributes()
    {
        btnBack.Attributes.Add("onclick", "refreshParent()");
    }

    /// <summary>
    /// This function is used to initialise field values
    /// </summary>
    private void InitializeFields()
    {
        InitializeMemberVariables();
		tblRecord.Visible = false;
        grdStudents.PageSize = 1000;
        grdStudents.EmptyDataText = Resources.LocalizedResources.NoAttendanceRecordsAvailable;
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        if (!IsPostBack)
        {
            SetControlsDefaultValues();
            hidStdDivId.Value = QueryString["iStandardDivisionId"] ?? "0";
        }
    }

    #endregion

    /// <summary>
    /// This method is used to encode the cell text of autogenerated coloums.
    /// </summary>
    /// <param name="e"></param>
    private void EncodeHtmlCells(GridViewRowEventArgs e)
    {
        TableCellCollection cells = e.Row.Cells;
        int iCnt = 0;
        foreach (TableCell cell in cells)
        {
            if (iCnt == 2)
                cell.HorizontalAlign = HorizontalAlign.Left;
            else if (iCnt == 3)
                cell.HorizontalAlign = HorizontalAlign.Center;
             if (iCnt == 0)   
                cell.Visible = false;
            else
                cell.Text = Server.HtmlDecode(cell.Text);              
            iCnt++;
        }
    }
}