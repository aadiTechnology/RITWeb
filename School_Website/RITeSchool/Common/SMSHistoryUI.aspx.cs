/*
 *  File Name : - SMSHistoryUI.aspx.cs
 *  Purpose   : - This class is used to display all SMS in an inbox.
 *  Date      : - 15-May-2007
 */

using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Threading;
using System.Configuration;
public partial class SMSHistoryUI : ExportDataTable
{
    #region Constants

    const int I_SELECT_CHK_COLUMN_INDEX = 0;
    const int I_READ_FLAG_IMG_COLUMN_INDEX = 1;
    const int I_FROM_SENDER_NAME_COLUMN_INDEX = 2;
    const int I_FROM_USER_NAME_COLUMN_INDEX = 3;
    const int I_MOBILENO_COLUMN_INDEX = 4;
    const int I_SUBJECT_COLUMN_INDEX = 5;
    const int I_RECEIVED_DATE_COLUMN_INDEX = 6;
    const int I_SAMEGROUP_COLUMN_INDEX = 7;
    const int I_STATUS_COLUMN_INDEX = 8;
    const int I_DATAKEY_SMS_DETAILS_ID = 0;
    const int I_DATAKEY_SMS_RECEIVER_DETAILS_ID = 1;
    const int I_DATAKEY_SMS_STATUS_ID = 3;

    private const string S_SCHEDULED_SMS = "3";
    private const string S_SMS_PROCESSED = "2";
    private const string S_SMS_SCHEDULED = "1";

    const string S_CHECK_BOX_DELETE = "ChkBoxDelete";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_DELETE = "At least one SMS should be selected for deletion.";
    

    #endregion

    #region DataMembers

    public HttpRequest moHttprequest;
    string msQueryString;
    #endregion

    #region Events

    /// <summary>
    /// This method isused to fill grid of Sent/recieved Items
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           GetQuerryString();
            if (!IsPostBack)
            {
                CheckRoleAndAssignDisplayView();
                InitializeFields();
                GridViewEventHandlerAndProperties();
                SetDefaultSortGridArrow();
                DecryptQuerystring();
                FillGridAsPerViewMode();
                SetControlsDefaultValues();
                ClearControls();
            }
            else
            {
                AddViewState();
            }

            //Hide back button for other users
            if (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                   && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                imgbtnBack.Visible = true;
            else
                imgbtnBack.Visible = false;

            ApplyMouseHoverEffect(new List<Button> { btnDelete, btnSearch, imgBtnComposeMessage, imgBtnNewMessage, imgbtnBack, btnExport });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// this method is used to navigate to send new SMS
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnComposeMessage_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Common/SMSUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Click Events

    /// <summary>
    /// This method is used to delete messages from grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteSelectedMessagesFromGrid();
            lblMobileNo.Visible = false;
            tdlblMobileNo.Visible = false;
        }
        catch (SqlException ex)
        {
            tblError.Visible = true;
            lblErrorMesage.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region GridView Events For Inbox and Trash

    /// <summary>
    /// This method is used to bound data to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int iSMSId = Convert.ToInt32(grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_SMS_DETAILS_ID].ToString());
                int iSendSMSId = Convert.ToInt32(grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_SMS_RECEIVER_DETAILS_ID].ToString());
                string sStatusId = grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_SMS_STATUS_ID].ToString();

                HyperLink olnkSubject = (HyperLink)e.Row.Cells[I_SUBJECT_COLUMN_INDEX].Controls[0];
                
                Image oStatusImage = (Image)e.Row.Cells[I_READ_FLAG_IMG_COLUMN_INDEX].Controls[0];

                string sQuerString = String.Format("MODE=View&SMSId={0}&SMSReceiverDetailsId={1}&pIndex={2}&pSortExp={3}&pSortDirc={4}&Name={5}&Content={6}&Access={7}"
                    , iSMSId, iSendSMSId, grdvwMessageInbox.PageIndex, hidSortExpression.Value, hidSortDirection.Value, txtName.Text, txtContent.Text, hidQuerryString.Value);
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);
                oStatusImage.Attributes.Add("onclick", String.Format("window.open('../Common/SMSUI.aspx?{0}' , '_self'); return false;", sEncrypt));
                olnkSubject.Attributes.Add("onclick", String.Format("window.open('../Common/SMSUI.aspx?{0}' , '_self'); return false;", sEncrypt));
                olnkSubject.NavigateUrl = "~/RITeSchool/Common/SMSUI.aspx?" + sEncrypt;

                if (e.Row.Cells[I_SAMEGROUP_COLUMN_INDEX].Controls.Count > 0)
                {
                    HyperLink olnkSameGroup = (HyperLink)e.Row.Cells[I_SAMEGROUP_COLUMN_INDEX].Controls[0];
                    string sQueryString = CommonUtility.EncryptQuerystring("SMSId="+iSMSId+"&From=ResendSMS");
                    olnkSameGroup.Attributes.Add("onclick", String.Format("window.open('SMSUI.aspx?" + sQueryString + "' , '_self'); return false;"));
                    olnkSameGroup.NavigateUrl = "~/RITeSchool/Common/SMSUI.aspx?";
                }

                if (e.Row.Cells[I_STATUS_COLUMN_INDEX].Controls.Count > 0)
                {
                    string sSMSShootId = grdvwMessageInbox.DataKeys[e.Row.RowIndex]["SMSShootId"].ToString();
                    HyperLink olnkSameGroup = (HyperLink)e.Row.Cells[I_STATUS_COLUMN_INDEX].Controls[0];
                    if (sSMSShootId.TrimAll() != string.Empty)
                    {
                        string sQueryString = CommonUtility.EncryptQuerystring("SMSShootId=" + sSMSShootId);
                        olnkSameGroup.Attributes.Add("onclick", String.Format("window.open('SMSStatusDetailsPopup.aspx?" + sQueryString + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=750,height=750').focus(); return false;"));
                        olnkSameGroup.NavigateUrl = "#";
                    }
                    else
                        olnkSameGroup.Visible = false;
                }

                
                CheckBox ochkDelete = (CheckBox)e.Row.FindControl(S_CHECK_BOX_DELETE);
                ochkDelete.Attributes.Add("onclick", "UpdateDeleteCount(this)");
                if (ViewState[S_CHECK_BOX_DELETE] != null)
                {
                    Hashtable oHtMessageDetailsId = (Hashtable)ViewState[S_CHECK_BOX_DELETE];
                    if (oHtMessageDetailsId.ContainsKey(grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_SMS_DETAILS_ID].ToString()))
                        ochkDelete.Checked = true;
                }

                if (sStatusId == S_SMS_PROCESSED)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                    ochkDelete.Visible = false;
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
                    for (int i = 0; i < grdvwMessageInbox.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control. 
                        //pageList.SelectedIndex = -1;
                        if (i == grdvwMessageInbox.PageIndex)
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
                    int currentPage = grdvwMessageInbox.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdvwMessageInbox.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method used to sort the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;

            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Export Detais in excel format.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int iShowAllSentSMS = (hidShowAllSendSMS.Value == Constants.S_ONE) ? 1 : 0;
            DataTable oDataTable = SMSMasterCollectionBL.GetSentItemsForUserForExport(miSchoolId, miUserId, moUserRole.ToInt(), miAcademicYearId, hidName.Value, hidContent.Value, string.Empty, 100000, 0, iShowAllSentSMS);
            oDataTable.Columns.Remove("TotalRows");
            ExportToExcel("SentSMSDetails.xls", oDataTable);
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
    /// This method used to set page index to the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwMessageInbox.PageIndex = e.NewPageIndex;
            FillGridAsPerViewMode();
            trMessage.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Use the RowType property to determine whether the 
    /// row being created is the header row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This method is used to navigate back.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            if (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                   && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                oMasterPage.RedirectToNextPage("../Common/SMSUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle paging and set index of changed page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdvwMessageInbox.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdvwMessageInbox.PageIndex = pageList.SelectedIndex;
            trMessage.Visible = false;
            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to handle paging and set index of changed page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
            {               
                    lblStartIndex.Text = Convert.ToString((grdvwMessageInbox.PageSize * grdvwMessageInbox.PageIndex) + 1);
                    lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwMessageInbox.PageSize) - 1);
                    if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
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
                    if (lblTotal.Text != "")
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
    /// This event is used to search SMS sent to perticular user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            hidName.Value = txtName.Text.Trim();
            hidContent.Value = txtContent.Text.Trim();
            grdvwMessageInbox.PageIndex = 0;
            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used when data is bound to grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_DataBound(object sender, EventArgs e)
    {
        try
        {
           if (grdvwMessageInbox.Rows.Count <= 0)
                btnDelete.Visible = false; 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Helping Methods
    /// <summary>
    /// This Method is used for retrieve querrystring to set page for Inbox mode or SentBox mode
    /// </summary>
    private void GetQuerryString()
    {
        try
        {
            if (Request.QueryString.ToString().Length > 1)
            {
                if (!QueryString["Access"].IsNull())
                    hidQuerryString.Value = QueryString["Access"];               
            }
            else if (!Session["Access"].IsNull())
            {
                hidQuerryString.Value = Session["Access"].ToString();               
            }

            if (hidQuerryString.Value.IsNullOrEmpty())
                hidQuerryString.Value = Constants.S_ZERO;

            if (Settings.SMSProviderForWebsite.ToLower() == Constants.SMSProviders.BusinessSMS.ToString().ToLower())
            {
                if ((moUserRole == Constants.UserRoles.Admin || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter) == Constants.C_YES) && (hidQuerryString.Value == Constants.S_ONE || hidShowAllSendSMS.Value == Constants.S_ONE))
                    trSMSStatus.Visible = true;
                else
                    trSMSStatus.Visible = false;
            }
            else
                trSMSStatus.Visible = false;
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }
    
    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        System.Web.UI.HtmlControls.HtmlForm oform = (System.Web.UI.HtmlControls.HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = btnSearch.UniqueID;
    }

    /// <summary>
    /// This method is used to fill grid as per view mode.
    /// </summary>
    private void FillGridAsPerViewMode()
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        SiteMapPath siteMap = (SiteMapPath)oMasterPage.FindControl("SiteMapPath1");

        ShowHideGridColumns();

        if ((moUserRole == Constants.UserRoles.Admin || (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)) && hidQuerryString.Value == Constants.S_ONE)
        {
            GetDsForFillSentItemsGrid();
            oMasterPage.NodeTitle = "Sent SMS";
            lblMobileNo.Visible = false;
            tdlblMobileNo.Visible = false;
            btnDelete.Visible = true;
           GrdDSobj.SelectCountMethod = "CountSentSMS";
            GrdDSobj.SelectMethod = "GetSentItemsForUser";
            grdvwMessageInbox.DataSourceID = GrdDSobj.ID;
            Session[Constants.S_SESSION_IS_SENT_SMS_LIST] = Constants.S_YES;
            btnExport.Visible = true;
        }
        else if ((moUserRole == Constants.UserRoles.Admin || (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Student)) 
            && (hidQuerryString.Value == Constants.S_ZERO))
        {
            GetDsForDisplaySMSInbox();
            oMasterPage.NodeTitle = "Received SMS";
            grdvwMessageInbox.DataSourceID = GrdDSobj.ID;
            imgBtnNewMessage.Visible = false;
            imgBtnComposeMessage.Visible = false;
            btnDelete.Visible = false;
            tblSearch.Visible = false;
            Session[Constants.S_SESSION_IS_SENT_SMS_LIST] = Constants.S_NO;
            grdvwMessageInbox.Columns[I_SELECT_CHK_COLUMN_INDEX].Visible = false;
        }
        else if (hidQuerryString.Value == S_SCHEDULED_SMS)
        {
            GetScheduledSMS();
            oMasterPage.NodeTitle = "Scheduled SMS";                        
            tblSearch.Visible = false;
            btnDelete.Visible = true;
            tblLegend.Visible = true;
            trHeaderMessage.Visible=false;
            tdlblUpdateMobile.Visible = false;
            trMobileSearch.Visible = false;
            GrdDSobj.SelectCountMethod = "CountScheduledSMS";
            GrdDSobj.SelectMethod = "GetScheduledSMS";
            grdvwMessageInbox.DataSourceID = GrdDSobj.ID;
            Session[Constants.S_SESSION_IS_SENT_SMS_LIST] = Constants.S_NO;
        }
    }

    /// <summary>
    /// This method is used to check if the login user is of superviser role and 
    /// check the access he have
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
        {
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter).ToString();
            if (Convert.ToChar(hidCanEdit.Value) != Constants.C_YES)
            {
                grdvwMessageInbox.Columns[2].ItemStyle.Width = Unit.Pixel(57);
                grdvwMessageInbox.Columns[3].ItemStyle.Width = Unit.Pixel(380);
                grdvwMessageInbox.Columns[2].SortExpression = "";
            }
        }
        else
        {
            if (moUserRole != Constants.UserRoles.Admin)
            {
                grdvwMessageInbox.Columns[2].ItemStyle.Width = Unit.Pixel(57);
                grdvwMessageInbox.Columns[3].ItemStyle.Width = Unit.Pixel(380);
                grdvwMessageInbox.Columns[2].SortExpression = "";
            }
        }
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void DecryptQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {

                string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
                msQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
                
                

                if (msQueryString == Constants.S_MESSAGE_SENT_SUCCESSFULLY)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = msQueryString;
                }
                else
                {
                    
                    hidQueryStrViewMode.Value = msQueryString;
                }
                moHttprequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                                Page.Request.Url.ToString(),
                                                msQueryString);

                if (moHttprequest.QueryString["pIndex"] != null)
                    grdvwMessageInbox.PageIndex = Convert.ToInt32(moHttprequest.QueryString["pIndex"]);
                if (moHttprequest.QueryString["pSortExp"] != null)
                    hidSortExpression.Value = moHttprequest.QueryString["pSortExp"];
                if (moHttprequest.QueryString["pSortDirc"] != null)
                    hidSortDirection.Value = moHttprequest.QueryString["pSortDirc"];
                if (moHttprequest.QueryString["Name"] != null)
                    txtName.Text = moHttprequest.QueryString["Name"];
                if (moHttprequest.QueryString["Content"] != null)
                    txtContent.Text = moHttprequest.QueryString["Content"];

                if (moHttprequest.QueryString["ShowAllSentSMS"] != null)
                    hidShowAllSendSMS.Value = moHttprequest.QueryString["ShowAllSentSMS"];

            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }

    }

    /// <summary>
    /// This method is used to add event handler and set properties of gridview.
    /// </summary>
    private void GridViewEventHandlerAndProperties()
    {
        grdvwMessageInbox.PageSize = Constants.I_GRID_PAGE_COUNT;
        grdvwMessageInbox.EmptyDataText = Constants.S_BLANK_GRID_MESSAGE;
    }

    /// <summary>
    /// This method is used to set controls value at the time of page load.
    /// </summary>    
    private void SetControlsDefaultValues()
    {
        lnkSMSStatus.Attributes.Add("onclick","OpenSMSStatusPopup(); return false;");
        btnDelete.Attributes.Add("Onclick", "if(!(ConfirmAction(" + grdvwMessageInbox.PageCount
                                                                   + ",'" + S_SELECT_AT_LEAST_ONE_MESSAGE_DELETE
                                                                   + "'))){return false;}");
        if ((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && (hidQuerryString.Value == Constants.S_ONE || Session[Constants.S_SESSION_IS_SENT_SMS_LIST].ToString() == Constants.S_YES))
        {
            lblMobileOne.Visible = false;
            lblMobileNo.Visible = false;
            tdlblUpdateMobile.Visible = false;
            tdlblMobileNo.Visible = false;
            tdlblMobile2.Visible = false;
            tdtxtMobile2.Visible = false;
            lblMob1.Text = "Mobile Number :";
        }
        else if ((moUserRole == Constants.UserRoles.Admin || (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Student)) && (hidQuerryString.Value == Constants.S_ZERO))
        {
            btnDelete.Visible = false;
            tblSearch.Visible = false;
            if(moUserRole != Constants.UserRoles.Student)
            {
                tdlblMobile2.Visible = false;
                tdtxtMobile2.Visible = false;
                lblMob1.Text = "Mobile Number :";
            }
            string sMobileNumber = RetrieveUserMobileNo();
            string[] sMobileNumbers = sMobileNumber.Split(';');
            lblMobileOne.Text = sMobileNumbers[0];
            if (moUserRole == Constants.UserRoles.Student)
            {
                if (sMobileNumbers[1] != string.Empty)
                    lblMobileTwo.Text = sMobileNumbers[1];
                else
                    lblMobileTwo.Text = "-";
            }           

        }
        if (hidShowAllSendSMS.Value == Constants.S_ONE)
            btnExport.Visible = true;
    }

    /// <summary>
    /// Set property of DataFormatString to bound field used to display dates.   
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        BoundField oReceivedDate = (BoundField)grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_TIME_FORMAT;
    }

    /// <summary>
    /// This method is used to fill sent items in the grid.
    /// </summary>    
    private void GetDsForFillSentItemsGrid()
    {
        SetGridViewDateColumnProperties();
        
            grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "To";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Send Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = false;
        grdvwMessageInbox.Columns[I_MOBILENO_COLUMN_INDEX].Visible = false;        
    }

    /// <summary>
    /// This method is used to show Received SMS to user.
    /// </summary>
    private void GetDsForDisplaySMSInbox()
    {
        SetGridViewDateColumnProperties();
        grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "From";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Received Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = false;
        if(moUserRole != Constants.UserRoles.Student)
            grdvwMessageInbox.Columns[I_MOBILENO_COLUMN_INDEX].Visible = false;
    }

    ///// <summary>
    ///// This method is used to show Received SMS to user.
    ///// </summary>
    private void GetScheduledSMS()
    {
        SetGridViewDateColumnProperties();
        grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "To";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Scheduled Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = false;
        if (moUserRole != Constants.UserRoles.Student)
            grdvwMessageInbox.Columns[I_MOBILENO_COLUMN_INDEX].Visible = false;

    }

    /// <summary>
    /// This method is used to delete the messages as per view mode from grid.
    /// </summary>   
    private void DeleteSelectedMessagesFromGrid()
    {
        if (hidQuerryString.Value == S_SCHEDULED_SMS)
            DeleteScheduledSMS();
        else
        {
            if (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                   && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                DeleteSentItemsFromGrid();
            else
                DeleteInboxSMSFromGrid();
        }
    }

    /// <summary>
    /// This method is used to archive messages from message inbox.
    /// </summary>    
    private void DeleteSentItemsFromGrid()
    {
        if (ViewState[S_CHECK_BOX_DELETE] != null)
        {
            Hashtable oHtMessageDetailsId = (Hashtable)ViewState[S_CHECK_BOX_DELETE];
            SMSMasterBL oSMSMasterBL = new SMSMasterBL();
            oSMSMasterBL.DeleteSentItems(oHtMessageDetailsId);
            FillPreviousPageData();
            FillGridAsPerViewMode();
        }
    }

    private void FillPreviousPageData()
    {
        //fill previous page index of grid if there is last index deleted.
        HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grdvwMessageInbox.HeaderRow.FindControl("ChkAllDel");
        GridViewRow oPagerRow = grdvwMessageInbox.BottomPagerRow;
        DropDownList pageList = (DropDownList)oPagerRow.Cells[0].FindControl("PageDropDownList");
        if (grdvwMessageInbox.PageIndex > 0 && (chkDelete.Checked == true))
            grdvwMessageInbox.PageIndex = pageList.SelectedIndex - 1;
        else
            grdvwMessageInbox.PageIndex = pageList.SelectedIndex;
        trMessage.Visible = false;
    }

    private void DeleteScheduledSMS()
    {
        if (ViewState[S_CHECK_BOX_DELETE] != null)
        {
            Hashtable oHtMessageDetailsId = (Hashtable)ViewState[S_CHECK_BOX_DELETE];
            string sSMSIds=string.Empty;
            foreach (var item in oHtMessageDetailsId.Values)
            {
                if (sSMSIds.IsNullOrEmpty())
                    sSMSIds = item.ToString();
                else
                    sSMSIds = sSMSIds+ "," +item.ToString();
            }

            SMSMasterBL oSMSMasterBL = new SMSMasterBL();
            oSMSMasterBL.DeleteScheduledSMS(sSMSIds, miSchoolId, miAcademicYearId);
            DeleteSentItemsFromGrid();            
        }
    }

    /// <summary>
    /// This method is used to add viewstate for a delete history.
    /// </summary>
    private void AddViewState()
    {
        CheckBox ochkDelete;
        Hashtable oHtMessageDetailsId;
        if (ViewState[S_CHECK_BOX_DELETE] != null)
            oHtMessageDetailsId = (Hashtable)ViewState[S_CHECK_BOX_DELETE];
        else
            oHtMessageDetailsId = new Hashtable();

        for (int iRowCount = 0; iRowCount < grdvwMessageInbox.Rows.Count; iRowCount++)
        {
            ochkDelete = (CheckBox)grdvwMessageInbox.Rows[iRowCount].FindControl(S_CHECK_BOX_DELETE);
            if (ochkDelete.Checked == true)
                oHtMessageDetailsId[grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_SMS_DETAILS_ID].ToString()] = grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_SMS_DETAILS_ID].ToString();
            else
                oHtMessageDetailsId.Remove(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_SMS_DETAILS_ID].ToString());
        }
        ViewState[S_CHECK_BOX_DELETE] = oHtMessageDetailsId;
       // hidDeleteCnt.Value = oHtMessageDetailsId.Count.ToString();
    }

    /// <summary>
    /// This method is used to delete Inbox / archived items from grid.
    /// </summary>
    private void DeleteInboxSMSFromGrid()
    {
        if (ViewState[S_CHECK_BOX_DELETE] != null)
        {
            Hashtable oHtMessageDetailsId = (Hashtable)ViewState[S_CHECK_BOX_DELETE];
            SMSMasterBL oSMSMasterBL = new SMSMasterBL();
            oSMSMasterBL.DeleteSMSFromInbox(oHtMessageDetailsId);
            FillGridAsPerViewMode();
        }
    }

    /// <summary>
    /// This method is used to set default sort arrow in grid.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        hidSortExpression.Value = grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to get login users mobile no.
    /// </summary>
    /// <returns></returns>
    private string RetrieveUserMobileNo()
    {
        String sMobileNo = string.Empty;
        switch (moUserRole)
        { 
            case Constants.UserRoles.Admin:
                SchoolBL oSchoolBL = new SchoolBL();
               sMobileNo = oSchoolBL.GetAdminMobileNo(miSchoolId, Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
                break;
            case Constants.UserRoles.Teacher:
                SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL(Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]));
                sMobileNo = oSchoolWiseTeacherMasterBL.MobileNumber;
                break;

            case Constants.UserRoles.Student:
                StudentBL oStudentBL = new StudentBL();
                sMobileNo = oStudentBL.GetMobileNo(miSchoolId, Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]));
                break;
            case Constants.UserRoles.Supervisor:
                SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL(miSchoolId, miUserId, miAcademicYearId);
                sMobileNo = oSchoolWiseSupervisorMasterBL.Mobile_Number;
                break;
        }
        if (sMobileNo == null)
            sMobileNo = string.Empty;
        return sMobileNo;
    }

    /// <summary>
    /// This methode is used to clear controls.
    /// </summary>
    private void ClearControls()
    {
        txtContent.Text = string.Empty;
        txtName.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to show or hide grid column based on the hidden field value.
    /// </summary>
    private void ShowHideGridColumns()
    {
        if (hidQuerryString.Value == Constants.S_ONE)
            grdvwMessageInbox.Columns[I_SAMEGROUP_COLUMN_INDEX].Visible = true;
        else
            grdvwMessageInbox.Columns[I_SAMEGROUP_COLUMN_INDEX].Visible = false;

        if (hidShowAllSendSMS.Value != Constants.S_ONE)
            grdvwMessageInbox.Columns[I_FROM_SENDER_NAME_COLUMN_INDEX].Visible = false;

        grdvwMessageInbox.Columns[I_STATUS_COLUMN_INDEX].Visible = false;
        if (Settings.SMSProviderForWebsite.ToLower() == Constants.SMSProviders.SoftSMS.ToString().ToLower())
        {
            if (hidQuerryString.Value == Constants.S_ONE)
                grdvwMessageInbox.Columns[I_STATUS_COLUMN_INDEX].Visible = true;
            else
                grdvwMessageInbox.Columns[I_STATUS_COLUMN_INDEX].Visible = false;
        }
    }

    #endregion  
}