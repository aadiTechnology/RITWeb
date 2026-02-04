// File Name  : FeedbackDetailsUI.aspx.cs
// Created By : Milind
// Date       : 23/4/2009
// Description :This class is used to show list of all feedback.
// Modified By: Rohini
// Date: 22 Feb 2012
// Description :Now admin can add, edit and delete the feedback.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Reflection;

public partial class FeedbackListUI : SchoolBase
{
    #region constants
    
    private const string S_SUCCESS_MSG = "Feedback saved successfully !!!";
    private const string S_DELETE_MSG = "Feedback deleted successfully !!!";
    private const string S_FILE_PATH = "\\RITeSchool\\downloads\\Feedbacks\\";
    private const string S_UPDATE_FEEDBACK = "Feedback updated successfully !!!";
    private const string S_REMOVE_COMMAND = "RemoveCommand";
    private const string S_EDIT_COMMAND = "EditCommand";
    private const string S_SAVE_MESSAGE = "Selected feedback(s) saved successfully !!!";
    private const string S_ServerPath = "../downloads/Feedbacks/";    
    private const string S_SortExpression = "InsertDate";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to add the sort image for the Ledger list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            // Add Sort Image
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

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
                Initializefields();
                SetJavaScriptAttribute();
                FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
                FillUserDetailGrid();
                hlnkAddNew.Attributes.Add("onclick", "ShowPopup2();");                
            }
            
            FeedbackDetails1.FillGrid += new EventHandler(btnShow_Click);
            FeedbackDetails1.ClearUserSearch += new EventHandler(btnCancelFeedback_Click);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to cancel the action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelFeedback_Click(object sender, EventArgs e)
    {
        try
        {
            txtuser.Text = string.Empty;
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

    /// <summary>
    /// This method is used to upload a file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidOtherMode.Value == Constants.S_NEW_MODE)
            {
                if (FileUpload1.HasFile)
                {                    
                    string sServerPath = Server.MapPath("~");
                    string sFolderName = sServerPath + S_FILE_PATH;
                    string sFileName=FileUpload1.FileName;
                    string asFileName=sFileName;
                    string sServerFilePath = sFolderName + sFileName;
                    if (File.Exists(sServerFilePath))
                    asFileName=GetFileNameForRenaming(sFileName);
                    sServerFilePath=sFolderName + asFileName;
                        FileUpload1.SaveAs(sServerFilePath);
                        SchoolEntities.FeedbackDetails oFeedbackDetails = PopulateOtherFeedback(asFileName);
                        FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
                        oFeedbackDetailsBL.InsertOtherFeedbackDetails(oFeedbackDetails);
                        FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
                        ShowSubmitMessage(S_SUCCESS_MSG, true);                   
                    
                }
            }
            else if (hidOtherMode.Value == Constants.S_EDIT_MODE)
                EditFeedbackDetails();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to populate Feed back information.
    /// </summary>
    protected void btnDisplayToUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbUserFeedback.Checked)
            {               
                SaveSelectedFeedackDetails();
                ShowMessage(S_SAVE_MESSAGE);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to cancel the action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            reqFile.Enabled = true;
            txtLinkName.Text = string.Empty;
            lblUpdateOther.Visible = false;
            hidOtherMode.Value = Constants.S_NEW_MODE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save selected feedbacks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<SchoolEntities.FeedbackDetails> oLstFeedbackId = new List<SchoolEntities.FeedbackDetails>();
            for (int iCount = 0; iCount < lstvwOtherFeedback.Items.Count; iCount++)
            {
                SchoolEntities.FeedbackDetails oFeedbackDetails = new SchoolEntities.FeedbackDetails();
                CheckBox oChkSelect = lstvwOtherFeedback.Items[iCount].FindControl("chkSelect") as CheckBox;
                oFeedbackDetails.LinkId = Convert.ToInt32(lstvwOtherFeedback.DataKeys[iCount]["LinkId"]);
                oFeedbackDetails.IsSelected = oChkSelect.Checked ? Constants.I_ONE : Constants.I_ZERO;
                oLstFeedbackId.Add(oFeedbackDetails);
            }

            string sXml = GenerateXml(oLstFeedbackId);
            FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
            oFeedbackDetailsBL.SaveOtherFeedback(sXml);
            ShowSubmitMessage(S_SAVE_MESSAGE, true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to search the feedback by user name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUserSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblDelete.Visible = false;
            FillUserDetailGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display user feedback.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdbUserFeedback_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserDetailGrid();
            SetFiledVisibility(true);
            HideNewFeedbackLink(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   

    /// <summary>
    /// This event is used to display other feedback.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdbOtherFeedback_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
            SetFiledVisibility(false);
            HideNewFeedbackLink(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Event

    /// <summary>
    /// This event is used to show or hide data pager.
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
                if (e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == Constants.S_ZERO)
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
    /// This method is used to set page according to index.
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
    /// 
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
            if (string.IsNullOrEmpty(hidSortDirection.Value))
                hidSortDirection.Value = Constants.S_DESCENDING;
            GridView sGridviewName = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                if (iSortColumnIndex != -1)
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
                else
                    CommonUtility.AddSortImage(1, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

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
            const int I_COLUMN_INDEX_DELETE = 6;
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                ImageButton imgDelete = (ImageButton)e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO];
                CheckBox oChkSelect = e.Row.Cells[Constants.I_ZERO].FindControl("ChkBoxSelect") as CheckBox;
                imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                oChkSelect.Checked = Convert.ToBoolean(grdUsersFeedback.DataKeys[e.Row.RowIndex]["Is_Selected"]);
            }

            SetGridPaging(e.Row);
            btnDisplayToUser.Visible = Convert.ToBoolean(grdUsersFeedback.Rows.Count);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            const string S_DELETE_COMMAND = "Delete_FeedbackDetails";
            const string S_EDIT_FEEDBACK_DETAILS = "Edit_FeedbackDetails";
            const int I_COLUMN_INDEX_FEEDBACK_ID = 0;

            if (e.CommandName == S_DELETE_COMMAND)
            {
                hidMode.Value = Constants.S_NEW_MODE;
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iFeedbackID = Convert.ToInt32(grdUsersFeedback.DataKeys[iRowIndex][I_COLUMN_INDEX_FEEDBACK_ID].ToString());
               
                FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
                oFeedbackDetailsBL.DeleteFeedbackDetails(iFeedbackID, miUserId);
                if (grdUsersFeedback.Rows.Count == Constants.I_ONE)
                    grdUsersFeedback.PageIndex = Constants.I_ZERO;
                FeedbackDetails1.ClearFeedbackControls();
                ShowMessage(S_DELETE_MSG);
                FillUserDetailGrid();
            }
            else if (e.CommandName == S_EDIT_FEEDBACK_DETAILS)
            {
                hidMode.Value = Constants.S_EDIT_MODE;
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iFeedbackId = Convert.ToInt32(grdUsersFeedback.DataKeys[iRowIndex][I_COLUMN_INDEX_FEEDBACK_ID].ToString());
                FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
                oFeedbackDetailsBL.Feedback_Id = iFeedbackId;
                oFeedbackDetailsBL.School_Id = miSchoolId;
                FeedbackDetails1.FillControls(iFeedbackId, miSchoolId);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "openPopup", "ShowPopup2();", true);
                AddSortImage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show delete message.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowMessage(string asMessage)
    {
        lblDelete.Visible = true;
        lblDelete.Text = asMessage;
    }

    #endregion

    #region "Listview  Events"

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    protected void lstvwOtherFeedback_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            if (oCurrentItem != null)
            {
                Label lblDate = oCurrentItem.FindControl("lblDate") as Label;
                ImageButton oImageButton = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                HyperLink oHyperLink = oCurrentItem.FindControl("lnkName") as HyperLink;
                DateTime dtFeedbackDate = Convert.ToDateTime(((SchoolEntities.FeedbackDetails)oCurrentItem.DataItem).InsertDate);
                string sOldFilePath = lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();

                oImageButton.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                oHyperLink.NavigateUrl = S_ServerPath + sOldFilePath;
                oHyperLink.Attributes.Add("onclick", "window.open('" + oHyperLink.NavigateUrl
                                               + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
                lblDate.Text = dtFeedbackDate.ToString(Constants.S_STANDARD_DATE_FORMAT);

                if (lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["IsSelected"].ToString() == Constants.S_ONE)
                {
                    CheckBox oChkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                    oChkSelect.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to delete feedback.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherFeedback_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (e.CommandName == S_REMOVE_COMMAND)
                {
                    if (oCurrentItem != null)
                    {
                        string sOldFilePath = lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
                        string sServerPath = Server.MapPath("~");
                        sServerPath += S_FILE_PATH;
                        if (File.Exists(sServerPath + sOldFilePath))
                        {
                            File.Delete(sServerPath + sOldFilePath);
                            SchoolEntities.FeedbackDetails oFeedbackDetails = new SchoolEntities.FeedbackDetails();
                            oFeedbackDetails.LinkId = Convert.ToInt32(lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["LinkId"]);
                            HyperLink oHyperLink = oCurrentItem.FindControl("lnkName") as HyperLink;
                            oFeedbackDetails.LinkName = StringUtility.ReplaceSingleQuoteInString(oHyperLink.Text.Trim(), true);
                            oFeedbackDetails.SchoolId = miSchoolId;
                            oFeedbackDetails.InsertedById = miUserId;
                            oFeedbackDetails.IsDeleted = Constants.S_YES;
                            oFeedbackDetails.FilePath = sOldFilePath;
                            FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
                            oFeedbackDetailsBL.UpdateOtherFeedback(oFeedbackDetails);
                        }
                        else
                            throw new FileNotFoundException();
                        FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
                        txtLinkName.Text = string.Empty;
                        ShowSubmitMessage(S_DELETE_MSG, true);
                    }
                }
                else if (e.CommandName == S_EDIT_COMMAND)
                {
                    if (oCurrentItem != null)
                    {
                        HyperLink oHyperLink = oCurrentItem.FindControl("lnkName") as HyperLink;
                        txtLinkName.Text = oHyperLink.Text;
                        hidOtherMode.Value = Constants.S_EDIT_MODE;
                        hidLinkId.Value = lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["LinkId"].ToString();
                        hidFilePath.Value = lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
                        reqFile.Enabled = false;
                        
                    }
                }
               
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == "SortRow")
            {
                if (hidSortDirection.Value == Constants.S_DESCENDING)
                    hidSortDirection.Value = Constants.S_ASCENDING;
                else
                    hidSortDirection.Value = Constants.S_DESCENDING;
                hidSortexpressionOther.Value = e.CommandArgument.ToString();
                FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
            }
        }
        catch (FileNotFoundException ex)
        {
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods    

    /// <summary>
    ///  This method is used to return  file name with appending time.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private  string GetFileNameForRenaming(string asFileName)
    {
        // This method modifies the file name as it is duplicate on the server.
        // Current time:monutes:seconds are appended to the file name and same is then returned.
        string sFileName;

        // Remove the extension from the file name.
        sFileName = asFileName.Substring(0, asFileName.LastIndexOf("."));

        // Append the time format to the file name.
        sFileName = sFileName +"."+DateTime.Now.ToString("yyyyMMddhhmmss");

        // Again append the original extension of the file.
        sFileName = sFileName + asFileName.Substring(asFileName.LastIndexOf("."));

        // Return the file name.
        return sFileName;
    }

    /// <summary>
    /// This method is used to populate other feedback details.
    /// </summary>
    /// <returns></returns>
    private SchoolEntities.FeedbackDetails PopulateOtherFeedback(string asFileName)
    {
        SchoolEntities.FeedbackDetails oFeedbackDetails = new SchoolEntities.FeedbackDetails();
        oFeedbackDetails.LinkName = StringUtility.ReplaceSingleQuoteInString(txtLinkName.Text.Trim(), true);
        oFeedbackDetails.FilePath = asFileName;
        oFeedbackDetails.SchoolId = miSchoolId;
        oFeedbackDetails.AcademicYearId = miAcademicYearId;
        oFeedbackDetails.InsertedById = miUserId;
        return oFeedbackDetails;
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
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnDisplayToUser, btnUpload, btnUserSearch });
        btnDisplayToUser.Attributes.Add("onclick", "if(!SelectedCount()){return false;}");
        btnSave.Attributes.Add("onclick", "if(!SelectedCountOther()) {return false;}");        
    }

    /// <summary>
    /// This method is used to show feedback submitted successufully message.
    /// </summary>
    private void ShowSubmitMessage(string asMesg, bool abFlag)
    {
        txtLinkName.Text = string.Empty;
        lblUpdateOther.Visible = abFlag;
        lblUpdateOther.Text = asMesg;
    }

    /// <summary>
    /// This method is used to fill other appreciation list.
    /// </summary>
    private void FillOtherAppreciationListView(string asSortExpression)
    {
        List<SchoolEntities.FeedbackDetails> lstDetails = FeedbackDetailsBL.GetOtherFeedback(miSchoolId, asSortExpression);
        lstvwOtherFeedback.DataSource = lstDetails;
        lstvwOtherFeedback.DataBind();
    }

    /// <summary>
    /// This method is used to save selected feedback.
    /// </summary>
    private void SaveSelectedFeedackDetails()
    {
        List<FeedbackDetailsBL> oLstFeedbackID = new List<FeedbackDetailsBL>();
        for (int iCnt = 0; iCnt < grdUsersFeedback.Rows.Count; iCnt++)
        {
            FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
            CheckBox oChkSelect = grdUsersFeedback.Rows[iCnt].FindControl("ChkBoxSelect") as CheckBox;
            oFeedbackDetailsBL.Feedback_Id = Convert.ToInt32(grdUsersFeedback.DataKeys[iCnt]["Feedback_Id"]);
            oFeedbackDetailsBL.UpdatedById = miUserId;
            oFeedbackDetailsBL.IsSelected = oChkSelect.Checked ? Constants.I_ONE : Constants.I_ZERO;
            oLstFeedbackID.Add(oFeedbackDetailsBL);
        }

        string sXml = GenerateXml(oLstFeedbackID);
        FeedbackDetailsBL oFeedbackDetails = new FeedbackDetailsBL();
        oFeedbackDetails.SaveSelectedFeedback(sXml, 0);
    }

    /// <summary>
    /// This method is used to initialize the controls.
    /// </summary>
    private void Initializefields()
    {
        hidSortexpressionOther.Value = S_SortExpression;
        hidSortDirection.Value = Constants.S_DESCENDING;
        tdOther.Visible = false;
        hidMode.Value = Constants.S_NEW_MODE;
        hidOtherMode.Value = Constants.S_NEW_MODE;
        grdUsersFeedback.PageIndex = 0;
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = grdUsersFeedback.Columns[Constants.I_ZERO].SortExpression;        
    }

    /// <summary>
    /// This method is used to set sort image.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortexpressionOther.Value))
            hidSortExpression.Value = S_SortExpression;
        HtmlTableRow oHtmlTableHeaderRow = lstvwOtherFeedback.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortexpressionOther.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to edit feedback details.
    /// </summary>
    private void EditFeedbackDetails()
    {
        string sFileName = hidFilePath.Value;
        if (FileUpload1.HasFile)
        {
            string sServerPath = Server.MapPath("~");
            sServerPath += S_FILE_PATH;
           
            string asFileName = FileUpload1.FileName;
            if (File.Exists(sServerPath + asFileName))
                asFileName = GetFileNameForRenaming(sFileName);
            FileUpload1.SaveAs(sServerPath + asFileName);
                SaveUpdatedFiles(asFileName);           
            
        }
        else
            SaveUpdatedFiles(sFileName);

        FillOtherAppreciationListView(hidSortexpressionOther.Value + " " + hidSortDirection.Value);
        SetNewMode();
    }

    /// <summary>
    /// This method is used to set new mode.
    /// </summary>
    private void SetNewMode()
    {
        txtLinkName.Text = string.Empty;
        hidOtherMode.Value = Constants.S_NEW_MODE;
        reqFile.Enabled = true;
        ShowSubmitMessage(S_UPDATE_FEEDBACK, true);
    }

    /// <summary>
    /// This method is used to save updated file details.
    /// </summary>
    private void SaveUpdatedFiles(string asFileName)
    {
        SchoolEntities.FeedbackDetails oFeedbackDetails = new SchoolEntities.FeedbackDetails();
        oFeedbackDetails.LinkId = Convert.ToInt32(hidLinkId.Value);
        oFeedbackDetails.LinkName = StringUtility.ReplaceSingleQuoteInString(txtLinkName.Text.Trim(), true);
        oFeedbackDetails.FilePath = FileUpload1.HasFile ? asFileName : hidFilePath.Value;
        oFeedbackDetails.SchoolId = miSchoolId;
        oFeedbackDetails.AcademicYearId = miAcademicYearId;
        oFeedbackDetails.InsertedById = miUserId;
        oFeedbackDetails.IsDeleted = Constants.S_NO;
        FeedbackDetailsBL oFeedbackDetailsBL = new FeedbackDetailsBL();
        oFeedbackDetailsBL.UpdateOtherFeedback(oFeedbackDetails);
    }

    /// <summary>
    /// This method is used to set control visibility.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetFiledVisibility(bool abFlag)
    {
        tblFeedbackUser.Visible = abFlag;
        tdOther.Visible = !abFlag;
        txtuser.Visible = abFlag;
        lblUser.Visible = abFlag;
        btnUserSearch.Visible = abFlag;
        trUserName.Visible = abFlag;
    }

    /// <summary>
    /// This method is used to hide add new feedback link.
    /// </summary>
    /// <param name="abflag"></param>
    private void HideNewFeedbackLink(bool abflag)
    {
        txtuser.Text = string.Empty;
        txtLinkName.Text = string.Empty;
        hlnkAddNew.Visible = abflag;
    }
    #endregion
}
