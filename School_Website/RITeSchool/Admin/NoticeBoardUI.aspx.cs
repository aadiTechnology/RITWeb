// File Name  : NoticeBoardUI.aspx.cs
// Created By : Ashish
// Date       : 21/11/2008
// Description: This class is used to add, edit or delete the Notice Board information.

using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;


public partial class NoticeBoardUI : SchoolBase
{

    #region " Constant "

    const int I_START_DATE_COLUMN = 1;
    const int I_END_DATE_COLUMN = 2;
    const string S_MESSAGE_DATAKEYNAME_ID = "Message_Id";
    const string S_DEFAULT_MSG_DATAKEYNAME_ID = "Is_Default_Msg";
    const string S_CMD_NAME_EDIT_MESSAGE = "EDIT_MESSAGE";
    const string S_CMD_NAME_DELETE_MESSAGE = "DELETE_MESSAGE";
    const string S_VIOLET_COLOR = "MediumVioletRed";
    const string S_BROWN_COLOR = "darkGreen";

    #endregion

    #region " Event "

    /// <summary>
    /// This event is used to fill grid and set java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultProperties();
                LoadNoticeBoardData();
                SetAcademicYearDates();
                SetClientScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Add/Update notice board message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!txtMessage.Text.Equals("") && !txtStartDate.Text.Equals("") && !txtEndDate.Text.Equals(""))
            {
                if (hidIsNewMessage.Value == "true")
                    AddMessage();
                else
                    UpdateMessage();
                ResetControl();
                LoadNoticeBoardData();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to cleare all input control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetControl();
            SelectAllRoles(true);
            hidIsDefaultMsg.Value = "false";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Grid Event "

    /// <summary>
    /// This event is used edit/delete notice board message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdNoticeBoard_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case S_CMD_NAME_EDIT_MESSAGE:
                    {
                        Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                        EditMessage(iRowIndex);
                        LoadNoticeBoardData();
                        FetchRolesFromNoticeBoardRoles();
                        txtMessage.Focus();
                    }
                    break;
                case S_CMD_NAME_DELETE_MESSAGE:
                    {
                        Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                        DeleteMessage(iRowIndex);
                        LoadNoticeBoardData();
                        ResetControl();
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event used to set properties to grid's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdNoticeBoard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetRowData(e.Row);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string sStartDate = (string)e.Row.Cells[I_START_DATE_COLUMN].Text;
                string sEndDate = (string)e.Row.Cells[I_END_DATE_COLUMN].Text;
                int iRowIndex = ((GridViewRow)e.Row).RowIndex;
                Boolean bIsDefalutMsg = Convert.ToBoolean(grdNoticeBoard.DataKeys[iRowIndex][S_DEFAULT_MSG_DATAKEYNAME_ID]);            
                DateTime dtToday = DateTime.Today;

                if (Convert.ToDateTime(sStartDate) <= dtToday && dtToday <= Convert.ToDateTime(sEndDate) && bIsDefalutMsg==false)
                {
                    //e.Row.BackColor = System.Drawing.Color.LightBlue;

                    e.Row.Style.Add("background-color", "LightBlue !important");
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
                    for (int i = 0; i < grdNoticeBoard.PageCount; i++)
                    {

                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        if (i == grdNoticeBoard.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the DropDownList.
                        pageList.Items.Add(item);
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
    /// This event is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdNoticeBoard_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;
            hidSortExpression.Value = hidSortExpression.Value + " " + hidSortDirection.Value;
            FillNoticeBoardGridview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdNoticeBoard.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdNoticeBoard.PageIndex = pageList.SelectedIndex;
            FillNoticeBoardGridview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set sortImage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdNoticeBoard_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set page number of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ObjectDataSet_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != "")
            {
                lblStartIndex.Text = Convert.ToString((grdNoticeBoard.PageSize * grdNoticeBoard.PageIndex) + 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdNoticeBoard.PageSize) - 1);
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (e.ReturnValue.ToString() == "0")
                            tblRowCounts.Visible = false;
                        else
                            tblRowCounts.Visible = true;

                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            tblRowCounts.Visible = false;
                        else
                            tblRowCounts.Visible = true;
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

    #region " Private Method "

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>  
    private void SetDefaultProperties()
    {
        valsumNoticeBoard.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdNoticeBoard.PageSize = Constants.I_GRID_PAGE_COUNT;

        hidSortExpression.Value = grdNoticeBoard.Columns[I_START_DATE_COLUMN].SortExpression;
        hidSortDirection.Value = Constants.S_DESCENDING;
        hidServerDate.Value = Convert.ToString(DateTime.Today);
        hidIsNewMessage.Value = "true";
        hidIsDefaultMsg.Value = "false";
        hidMessageId.Value = "0";
        hidRowIndex.Value = "-1";
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        txtMessage.Focus();
        btnCancel.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        grdNoticeBoard.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");        
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnCancel });
    }

    /// <summary>
    /// This method is used to fill grid view.
    /// </summary>
    private void LoadNoticeBoardData()
    {
        FillNoticeBoardGridview();
        ShowNoticeBoardMessage();
        FillNoticeBoardRoles();
        SelectAllRoles(true);
    }

    /// <summary>
    /// This method is used to fill roles into checkBoxList.
    /// </summary>
    private void FillNoticeBoardRoles()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBL();
        DataTable oDTRole = oNoticeBoardBL.RetriveRolesFromUserRoleMaster();

        if (!Settings.EnableOtherStaffLogin)
        {
          DataRow[] dr = oDTRole.Select("User_Role_Id="+Constants.UserRoles.OtherStaff.ToInt());
          if (dr.Length > 0)
          {
              dr[0].Delete();
              oDTRole.AcceptChanges();
          }
        }

        chkListRoles.DataSource = oDTRole;
        chkListRoles.DataTextField = "User_Role_Name";
        chkListRoles.DataValueField = "User_Role_Id";
        chkListRoles.DataBind();
    }

    /// <summary>
    /// This method is used to select all roles.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SelectAllRoles(bool abFlag)
    {
        int iListCount = chkListRoles.Items.Count;
        if (iListCount != 0)
        {
            for (int iIndex = 0; iIndex < iListCount; iIndex++)
                chkListRoles.Items[iIndex].Selected = abFlag;
        }
    }

    /// <summary>
    /// This method is used to fill grid view by using object datasource.
    /// </summary>
    private void FillNoticeBoardGridview()
    {
        grdNoticeBoard.DataSource = ObjectDataSet;
        grdNoticeBoard.DataBind();
    }

    /// <summary>
    /// This method initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetAcademicYearDates()
    {
        hidAcademicYrStartDt.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
        hidAcademicYrEndDt.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
    }

    /// <summary>
    /// This method is used to add message in database.
    /// </summary>
    private void AddMessage()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBLForSave();
        oNoticeBoardBL.AddNoticeMessage();
    }

    /// <summary>
    /// This method is used to update message in database.
    /// </summary>
    private void UpdateMessage()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBLForSave();
        oNoticeBoardBL.MessageId = Convert.ToInt32(hidMessageId.Value);
        oNoticeBoardBL.UpdateNoticeMessage();
        hidUpdateMode.Value = "False";
    }

    /// <summary>
    /// This method is used to reset all the control.
    /// </summary>
    private void ResetControl()
    {
        txtMessage.Text = string.Empty;
        txtStartDate.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        btnAdd.Text = "Add";
        hidIsNewMessage.Value = "true";
        txtStartDate.Enabled = true;
        txtEndDate.Enabled = true;
        hidRowIndex.Value = "-1";
        hidUpdateMode.Value = "False";
        txtMessage.Focus();
        SetAcademicYearDates();
    }

    /// <summary>
    /// This method is used to Initialized Notice Board BL class property.
    /// </summary>
    private NoticeBoardBL InitializeNoticeBoardBL()
    {
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        oNoticeBoardBL.SchoolId = miSchoolId;
        oNoticeBoardBL.AcademicYearId =miAcademicYearId;
        oNoticeBoardBL.InsertedById = miUserId;
        oNoticeBoardBL.UpdatedById = miUserId;
        oNoticeBoardBL.UpdatedDate = Convert.ToDateTime(System.DateTime.Today);

        return oNoticeBoardBL;
    }

    /// <summary>
    /// This method is used to Initialized Notice Board text value.
    /// </summary>
    private NoticeBoardBL InitializeNoticeBoardBLForSave()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBL();
        oNoticeBoardBL.NoticeMessage = Convert.ToString(txtMessage.Text);
        oNoticeBoardBL.StartDate = Convert.ToDateTime(txtStartDate.Text);
        oNoticeBoardBL.EndDate = Convert.ToDateTime(txtEndDate.Text);

        oNoticeBoardBL.SelectedRoles = GetSelectedRoles();

        return oNoticeBoardBL;
    }

    /// <summary>
    /// This method is used to collect selected roles into List collection.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedRoles()
    {
        int iTotalRoles = chkListRoles.Items.Count;
        List<int> RoleValues = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
        {
            if (chkListRoles.Items[iListIndex].Selected == true)
                RoleValues.Add(Convert.ToInt32(chkListRoles.Items[iListIndex].Value));
        }
        return RoleValues;
    }

    /// <summary>
    /// This method is used to hide image button when default message (Is_Default_Msg) is true.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetRowData(GridViewRow gridViewRow)
    {
        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= 0)
        {
            Boolean bIsDefalutMsg = Convert.ToBoolean(grdNoticeBoard.DataKeys[iRowIndex][S_DEFAULT_MSG_DATAKEYNAME_ID]);
            if (bIsDefalutMsg == true)
            {
                ImageButton oIsDefault = (ImageButton)gridViewRow.FindControl("btnDelete");
                oIsDefault.Visible = false;
                //gridViewRow.BackColor = System.Drawing.Color.Aqua;
                gridViewRow.Style.Add("background-color", "Aqua !important");

            }
            else
            {
                ImageButton oDeleteMessage = (ImageButton)gridViewRow.FindControl("btnDelete");
                oDeleteMessage.Attributes.Add("Onclick", "if(!ConfirmDelete()){return false;}");
            }
        }
    }

    /// <summary>
    /// This method is used to show notice board message on control panel page.
    /// </summary>
    private void ShowNoticeBoardMessage()
    {
        DataView oDViewNoticeMsg = (DataView)((System.Web.UI.WebControls.ObjectDataSource)(grdNoticeBoard.DataSource)).Select();
        DataTable oDTNoticeMsg = oDViewNoticeMsg.Table;
        string sNoticeMsg = string.Empty;
        DataRow[] oDRMessages = oDTNoticeMsg.Select("Is_Default_Msg = False");

        if (oDRMessages.Length > 0)
            sNoticeMsg = DisplayMessages(oDRMessages);
        if (sNoticeMsg == "")
        {
            oDRMessages = oDTNoticeMsg.Select("Is_Default_Msg=True");
            sNoticeMsg = DisplayDefaultMessage(oDRMessages);
        }
        lblNoticeBoardMsg.Text = sNoticeMsg;
    }

    /// <summary>
    /// This method is used to display notice board messages.
    /// </summary>
    /// <param name="oDRMessages"></param>
    /// <returns></returns>
    private string DisplayMessages(DataRow[] oDRMessages)
    {
        string sNoticeMsg = "";
        DateTime dtToday = System.DateTime.Today;
        DateTime dtStartDate;
        DateTime dtEndDate;
        string sMessageColor = S_VIOLET_COLOR;

        for (int iCount = 0; iCount < oDRMessages.Length; iCount++)
        {
            dtStartDate = Convert.ToDateTime(oDRMessages[iCount]["Start_Date"]);
            dtEndDate = Convert.ToDateTime(oDRMessages[iCount]["End_Date"]);
            if (dtToday >= dtStartDate && dtToday <= dtEndDate)
            {
                if (sNoticeMsg.Equals(string.Empty))
                    sNoticeMsg = Convert.ToString(oDRMessages[iCount]["Message"]);
                else
                    sNoticeMsg = sNoticeMsg + " &nbsp;&nbsp;&nbsp;&nbsp;<font color='black'>&sect;</font><font color='" + sMessageColor + "'>&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToString(oDRMessages[iCount]["Message"]) + "</font>";

                if (sMessageColor.Equals(S_BROWN_COLOR))
                    sMessageColor = S_VIOLET_COLOR;
                else
                    sMessageColor = S_BROWN_COLOR;
            }
        }
        return sNoticeMsg;
    }

    /// <summary>
    /// This method is used to display default message.
    /// </summary>
    /// <param name="oDRMessages"></param>
    private string DisplayDefaultMessage(DataRow[] oDRMessages)
    {
        string sNoticeMsg = "";
        for (int iCount = 0; iCount < oDRMessages.Length; iCount++)
        {
            sNoticeMsg = Convert.ToString(oDRMessages[iCount]["Message"]);
        }
        return sNoticeMsg;
    }

    /// <summary>
    /// This method is used to delete existing message.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void DeleteMessage(int iRowIndex)
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBL();
        int iMessageId = Convert.ToInt32(grdNoticeBoard.DataKeys[iRowIndex][S_MESSAGE_DATAKEYNAME_ID].ToString());
        oNoticeBoardBL.MessageId = iMessageId;
        oNoticeBoardBL.DeleteNoticeMessage();
        grdNoticeBoard.PageIndex = 0;
    }

    /// <summary>
    /// This method is used to update existing message.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void EditMessage(int iRowIndex)
    {
        txtMessage.Text = Convert.ToString(grdNoticeBoard.Rows[iRowIndex].Cells[Constants.I_ZERO].Text);
        DateTime odtStartDate = Convert.ToDateTime(grdNoticeBoard.Rows[iRowIndex].Cells[I_START_DATE_COLUMN].Text);
        DateTime odtEndDate = Convert.ToDateTime(grdNoticeBoard.Rows[iRowIndex].Cells[I_END_DATE_COLUMN].Text);
        txtStartDate.Text = odtStartDate.ToString("dd-MMM-yyyy");
        txtEndDate.Text = odtEndDate.ToString("dd-MMM-yyyy");
        btnAdd.Text = "Update";
        hidIsNewMessage.Value = "false";
        hidRowIndex.Value = Convert.ToString(iRowIndex);
        hidMessageId.Value = grdNoticeBoard.DataKeys[iRowIndex][S_MESSAGE_DATAKEYNAME_ID].ToString();

        Boolean bIsDefalutMsg = Convert.ToBoolean(grdNoticeBoard.DataKeys[iRowIndex][S_DEFAULT_MSG_DATAKEYNAME_ID]);
        if (bIsDefalutMsg == true)
        {
            txtStartDate.Enabled = false;
            txtEndDate.Enabled = false;
            hidIsDefaultMsg.Value = "true";
        }
        else
        {
            txtStartDate.Enabled = true;
            txtEndDate.Enabled = true;
            hidIsDefaultMsg.Value = "false";
        }
        hidUpdateMode.Value = "True";
    }

    /// <summary>
    /// This method is used to fetch roles from table NoticeBoardRoles according to messageId.
    /// </summary>
    private void FetchRolesFromNoticeBoardRoles()
    {
        int iItemCount, iNoticeRoleId, iRowCount, iRowIndex;
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        iNoticeRoleId = Convert.ToInt32(hidMessageId.Value);
        DataTable ODTNoticeBoardRoles = oNoticeBoardBL.RetrieveRolesFromNoticeBoardRoles(iNoticeRoleId);
        iItemCount = chkListRoles.Items.Count;
        iRowCount = ODTNoticeBoardRoles.Rows.Count - 1;
        DataRow oDRRoles;
        iRowIndex = 0;
        for (int iIndex = 0; iIndex < iItemCount; iIndex++)
        {
            oDRRoles = ODTNoticeBoardRoles.Rows[iRowIndex];
            if (chkListRoles.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
            {
                chkListRoles.Items[iIndex].Selected = true;
                if (iRowIndex < iRowCount)
                    iRowIndex++;
            }
            else
                chkListRoles.Items[iIndex].Selected = false;
        }
    }

    #endregion

}
