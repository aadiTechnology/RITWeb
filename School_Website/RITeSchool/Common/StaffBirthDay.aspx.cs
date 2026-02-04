using System;
using System.Data;
using System.Web.UI.WebControls;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class StaffBirthDay : SchoolBase
{
    #region  Constants

    const int I_SORT_ORDER=1;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill school staff birthday grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                grdStaffBirthday.PageSize = Constants.I_GRID_PAGE_COUNT;
                SetGridViewDateColumnProperties();
                FillStaffBirthDayGrid();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the color to  upcoming birthday.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStaffBirthday_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 1 && hidRowCnt.Value!=string.Empty)
            {                
                if (grdStaffBirthday.PageIndex == Constants.I_ZERO && Convert.ToInt32(hidRowCnt.Value) - 1 > e.Row.RowIndex)
                {
                    int iSort = Convert.ToInt32(grdStaffBirthday.DataKeys[Constants.I_ZERO][I_SORT_ORDER].ToString());
                    int iSortOrderId = Convert.ToInt32(grdStaffBirthday.DataKeys[e.Row.RowIndex - 1][I_SORT_ORDER].ToString());
                    if (iSortOrderId == iSort)
                    {
                        grdStaffBirthday.Rows[e.Row.RowIndex - 1].Font.Bold = true;
                        //grdStaffBirthday.Rows[e.Row.RowIndex - 1].BackColor = System.Drawing.Color.FromArgb(239, 220, 201);
                        grdStaffBirthday.Rows[e.Row.RowIndex - 1].Style.Add("background-color", "#EFDCC9 !important");
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager)
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
                    for (int i = 0; i < grdStaffBirthday.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.                        
                        if (i == grdStaffBirthday.PageIndex)
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
                    int currentPage = grdStaffBirthday.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdStaffBirthday.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used when gridview is attached to database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStaffBirthday.PageSize * grdStaffBirthday.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStaffBirthday.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0" || grdStaffBirthday.PageCount == 0)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    hidRowCnt.Value = Convert.ToString(Convert.ToInt32(e.ReturnValue)-1);
                }
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display if we change index of page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStaffBirthday_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStaffBirthday.PageIndex = e.NewPageIndex;
            SetGridViewDateColumnProperties();
            FillStaffBirthDayGrid();
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
            GridViewRow pagerRow = grdStaffBirthday.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStaffBirthday.PageIndex = pageList.SelectedIndex;
            SetGridViewDateColumnProperties();
            FillStaffBirthDayGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to fill school staff birthday grid.
    /// </summary>
    private void FillStaffBirthDayGrid()
    {
        grdStaffBirthday.DataSourceID = GrdDSobj.ID;       
    }

    /// <summary>
    /// This function is used to set the date format for date column property 
    /// </summary>    
    private void SetGridViewDateColumnProperties()
    {
        const int I_DATE_COLUMN = 1;

        BoundField oReceivedDate = (BoundField)grdStaffBirthday.Columns[I_DATE_COLUMN];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = "{0:dd MMM}";
    }

    #endregion

}