// File Name  : FeedbackDetailsUI.aspx.cs
// Created By : Milind
// Date       : 23/4/2009
// Description :This class is used to show list of all feedback.

using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;

public partial class FeedbackListUI :SchoolBase
{
    #region "Const"

    private const string S_DELETE_COMMAND = "Delete_FeedbackDetails";
    private const int I_COLUMN_INDEX__FEEDBACK_ID = 0;
    private const int I_COLUMN_INDEX_DELETE = 3;   

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill all control related to user and feedback type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FillUserRoleCombo();
                FillFeedbackTypeCombo();
                ddlUserRole.Focus();
                SetDefaultControlProperties();
                SetJavaScriptAttribute();
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used show the feedback details grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            grdUsersFeedback.PageIndex = 0;   
            FillUserDetailGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Event
    /// <summary>
    /// This event is used to sort data according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsersFeedback_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsersFeedback_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                ImageButton imgDelete = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO];
                imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }

            SetGridPaging(e.Row);
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete particular feedback details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsersFeedback_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_DELETE_COMMAND)
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iFeedbackID = Convert.ToInt32(grdUsersFeedback.DataKeys[iRowIndex][I_COLUMN_INDEX__FEEDBACK_ID].ToString());
                FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
                oFeedbackDetailsBL.DeleteFeedbackDetails(iFeedbackID, miUserId);
                lblDelete.Visible = true;
                lblDelete.Text = "Feedback deleted successfully!!!";
                FillUserDetailGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// this event is used to show data pager according to records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdUsersFeedback.PageSize * grdUsersFeedback.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdUsersFeedback.PageSize) - 1);
                if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }

                    if (lblTotal.Text != string.Empty)
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event i used to set the page according to page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsersFeedback_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdUsersFeedback.PageIndex = e.NewPageIndex;
            FillUserDetailGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set page dropdown according to selected index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdUsersFeedback.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdUsersFeedback.PageIndex = pageList.SelectedIndex;
            FillUserDetailGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event sets sortimaege.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsersFeedback_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = (System.Web.UI.WebControls.GridView)sender;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex;
                if (ddlUserRole.SelectedValue == Constants.UserRoles.Student.ToString())
                    sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                else
                    sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                else
                    CommonUtility.AddSortImage(1, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo()
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSStateCollection = oMasterDataCollectionBL.GetAllUserRoles();       
        ControlUtility.FillDropDownList(oDSStateCollection.Select("User_Role_Name<>'Parent'"), ref ddlUserRole, Constants.S_USER_ROLE_ID_FIELD, Constants.S_USER_ROLE_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill feed back type combo.
    /// </summary>
    private void FillFeedbackTypeCombo()
    {
        //const string S_FEEDBACK_TYPE_ID_FIELD = "Feedback_Type_Id";
        //const string S_FEEDBACK_TYPE_NAME_FIELD = "Feedback_Type";   
        //FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
        //DataTable oDSFeedbacktype = oFeedbackDetailsBL.RetriveFeedbackTypeFromFeedbackTypeMaster();
        //ControlUtility.FillDropDownList(oDSFeedbacktype, ref ddlFeedbackType, S_FEEDBACK_TYPE_ID_FIELD, S_FEEDBACK_TYPE_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set grid view paging.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetGridPaging(GridViewRow gridViewRow)
    {
        if (gridViewRow.RowType == DataControlRowType.Pager)
        {
            GridViewRow pagerRow = gridViewRow;
            // Retrieve the DropDownList and Label controls from the row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");
            if (pageList != null)
            {
                // Create the values for the DropDownList control based on 
                // the  total number of pages required to display the data
                // source.
                for (int i = 0; i < grdUsersFeedback.PageCount; i++)
                {
                    // Create a ListItem object to represent a page.
                    int pageNumber = i + 1;
                    ListItem item = new ListItem(pageNumber.ToString());
                    // If the ListItem object matches the currently selected
                    // page, flag the ListItem object as being selected. Because
                    // the DropDownList control is recreated each time the pager
                    // row gets created, this will persist the selected item in
                    // the DropDownList control.                        
                    if (i == grdUsersFeedback.PageIndex)
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
                int currentPage = grdUsersFeedback.PageIndex + 1;
                // Update the Label control with the current page information.
                pageLabel.Text = "Page " + currentPage.ToString() +
                  " of " + grdUsersFeedback.PageCount.ToString();
            }
        }
    }

    /// <summary>
    /// This method is used to fill userdetail's grid of a particular user.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    private void FillUserDetailGrid()
    {
        grdUsersFeedback.DataSourceID = GrdDSobj.ID;
        grdUsersFeedback.DataBind();
    }

    /// <summary>
    /// This method is used to set default control on the grid.
    /// </summary>
    /// <param name="aiUserRoleId"></param>
    /// <param name="sSortExpression"></param>
    private void SetDefaultControlProperties()
    {
        txtFromDate.Text = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).ToString("dd-MMM-yyyy");
        txtToDate.Text = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).ToString("dd-MMM-yyyy");
        grdUsersFeedback.PageIndex = 0;
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = grdUsersFeedback.Columns[Constants.I_ZERO].SortExpression;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnShow });
        btnBack.PostBackUrl = Constants.S_PAGE_SUPERADMIN_DASHBOARD;
    }

    #endregion
   
}
