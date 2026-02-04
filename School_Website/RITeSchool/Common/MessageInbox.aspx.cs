/*
 *  File Name : - MessageInbox.aspx.cs
 *  Purpose   : - This class is used to display all messsages in an inbox.
 *  Date      : - 15-May-2007
 */

using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;
using StudentEntities;
using SchoolEntities;
using System.Text;

public partial class MessageInbox : SchoolBase
{
    #region Constants

    const int I_READ_FLAG_IMG_COLUMN_INDEX = 1;
    const int I_FROM_USER_NAME_COLUMN_INDEX = 2;
    const int I_CC_USER_NAME_COLUMN_INDEX = 3;
    const int I_SUBJECT_COLUMN_INDEX = 4;
    const int I_READ_RECEIPT_COLUMN_INDEX = 5;
    const int I_RECEIVED_DATE_COLUMN_INDEX = 6;

    const int I_DATAKEY_MESSAGE_DETAILS_ID = 0;
    const int I_DATAKEY_MESSAGE_RECEIVER_DETAILS_ID = 1;
    const int I_DATAKEY_READ_FLAG = 2;
    const int I_DATAKEY_ITEM_TYPE = 3;
    const int I_DATAKEY_REPLY_FORWARD_FLAG = 9;
    const int I_SUPER_ADMIN_ID = 2771;
    const string S_UNREAD_MESSEGES = " Unread Messages";
    const string S_UNREAD_MESSEGE = " Unread Message";
    const string S_FLAG_SHOW_INBOX = "ShowInbox";
    const string S_FLAG_SHOW_SENT_ITEMS = "ShowSentItems";
    const string S_FLAG_SHOW_TRASH = "ShowTrash";
    const string S_FLAG_SHOW_DRAFT = "Draft";
    const string S_IMG_FOR_UNREAD_MSG = "~/RITeSchool/images/IconGrid_Mail.gif";
    const string S_IMG_FOR_READ_MSG = "~/RITeSchool/images/IconGrid_MailOpen.gif";
    const string S_IMG_FOR_FORWARD_MSG = "~/RITeSchool/images/IconGrid_MailFwd.gif";
    const string S_IMG_FOR_REPLY_MSG = "~/RITeSchool/images/IconGrid_MailReply.gif";
    const string S_CHECK_BOX_DELETE = "ChkBoxDelete";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_DELETE = "At least one message should be selected for deletion.";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_UNREAD = "At least one message should be selected to mark as unread.";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_READ = "At least one message should be selected to mark as read.";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_TRASH = "At least one message should be selected for trash.";
    const string S_SELECT_AT_LEAST_ONE_MESSAGE_UNDELETE = "At least one message should be selected.";

    const string S_IMG_FOR_HIGHLIGHT_DRAFT_ITEMS = "~/RITeSchool/images/IconBtn_DraftActive.gif";

    const string S_SCREENS_URL = "ScreensUI.aspx";
    static string msFromUrl = string.Empty;

    const string S_ACADEMIC_YEAR_ID = "Academic_Year_ID";
    const string S_YEAR_VALUE = "YearValue";
    #endregion

    #region DataMembers

    public HttpRequest moHttprequest;
    string msQueryString;
    private bool mbReadMessage = false, mbUnreadMessage = false;
    
    DataTable moDtAcademicAndYearInfo;

    #endregion

    #region Events

    /// <summary>
    /// This method is used set grid view properties, set controls as per view mode,fills grids as
    /// Inbox,Trash or Sent Items and sets unread message count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {        
            if (!IsPostBack)
            {
                FillOperators();
                FillAcademicYearCombo();
                RefreshValue();                    
                InitializeFields();
                UpdateNewMessageFlag();
                SetControlsDefaultValues();
				SetEmailAddressOnPopup();
                GetQuerystring();
                SetButtonState();
                GridViewEventHandlerAndProperties();
                FillGridAsPerViewMode();               
            }
			
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Click Events

    /// <summary>
    /// This method is used to view Sent Items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSentItems_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lstDraftMessageDetails.Visible = false;
            txtSearch.Text = string.Empty;
            txtSearchDate.Text = string.Empty;
            cmbOperation.SelectedIndex = Constants.I_ZERO;
            hidQueryStrViewMode.Value = S_FLAG_SHOW_SENT_ITEMS;
            FillGridAsPerViewMode();
            grdvwMessageInbox.PageIndex = 0;
            btnUnread.Visible = false;
            btnRead.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view Trash messages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnShowTrash_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lstDraftMessageDetails.Visible = false;
            txtSearch.Text = string.Empty;
            txtSearchDate.Text = string.Empty;
            cmbOperation.SelectedIndex = Constants.I_ZERO;
            hidQueryStrViewMode.Value = S_FLAG_SHOW_TRASH;
            FillGridAsPerViewMode();
            grdvwMessageInbox.PageIndex = 0;
            btnUnread.Visible = true;
            btnRead.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view inbox messages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnInbox_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lstDraftMessageDetails.Visible = false;
            txtSearch.Text = string.Empty;
            txtSearchDate.Text = string.Empty;
            cmbOperation.SelectedIndex = Constants.I_ZERO;
            hidQueryStrViewMode.Value = S_FLAG_SHOW_INBOX;
            FillGridAsPerViewMode();
            grdvwMessageInbox.PageIndex = 0;
            btnUnread.Visible = true;
            btnRead.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view Draft messages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnDraft_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            imgBtnDraft.ImageUrl = S_IMG_FOR_HIGHLIGHT_DRAFT_ITEMS;
            lstDraftMessageDetails.Visible = true;
            FillUsersDraftMessage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to redirect page to compose new messages.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnComposeMessage_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx");
        }
        catch (Exception ex)
        {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to delete messages from grid based on Item Type.
    /// Item type indicates whether it is inbox item or Sent Item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox ochkDelete;
            ArrayList oArrMessageReceiverId = new ArrayList();
            ArrayList oArrMessageSentId = new ArrayList();

            for (int iRowCount = 0; iRowCount < grdvwMessageInbox.Rows.Count; iRowCount++)
            {
                ochkDelete = (CheckBox)grdvwMessageInbox.Rows[iRowCount].FindControl(S_CHECK_BOX_DELETE);
                if (ochkDelete.Checked == true)
                {
                    int iItemType = Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_ITEM_TYPE]);
                    if (iItemType == 0)
                    {
                        //this array list stores Inbox message IDs
                        int iMessageReceiverDetailsId =
                                         Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_MESSAGE_RECEIVER_DETAILS_ID].ToString());
                        oArrMessageReceiverId.Add(iMessageReceiverDetailsId);
                    }
                    else if (iItemType == 1)
                    {
                        //this array list stores sent message IDs
                        int iMessageDetailsId = Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_MESSAGE_DETAILS_ID]);
                        oArrMessageSentId.Add(iMessageDetailsId);
                    }
                }
            }
            if (oArrMessageReceiverId.Count != 0)
            {
                MessageReceiverDetailsCollectionBL oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();
                oMessageReceiverDetailsCollectionBL.DeleteInboxAndArchivedMessages(oArrMessageReceiverId);
            }
            if (oArrMessageSentId.Count != 0)
            {
                MessageDetailsCollectionBL oMessageDetailsCollectionBL = new MessageDetailsCollectionBL();
                oMessageDetailsCollectionBL.DeleteSentItems(oArrMessageSentId);
            }

            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
             ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to trash messages from grid based on Item Type.
    /// Item type indicates whether it is inbox item or Sent Item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnArchive_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox ochkDelete;
            MessageReceiverDetailsCollectionBL oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();
            ArrayList oArrMessageReceiverId = new ArrayList();
            ArrayList oArrMessageSentId = new ArrayList();
            if (hidQueryStrViewMode.Value != S_FLAG_SHOW_SENT_ITEMS)
            {
                for (int iRowCount = 0; iRowCount < grdvwMessageInbox.Rows.Count; iRowCount++)
                {
                    ochkDelete = (CheckBox)grdvwMessageInbox.Rows[iRowCount].FindControl(S_CHECK_BOX_DELETE);
                    if (ochkDelete.Checked == true)
                    {
                        //this array list stores Inbox message IDs
                        int iItemType = Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_ITEM_TYPE]);
                        if (iItemType == 0)
                        {
                            int iMessageReceiverDetailsId = Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_MESSAGE_RECEIVER_DETAILS_ID].ToString());
                            oArrMessageReceiverId.Add(iMessageReceiverDetailsId);
                        }
                        else if (iItemType == 1)
                        {
                            //this array list stores sent message IDs
                            int iMessageDetailsId = Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_MESSAGE_DETAILS_ID]);
                            oArrMessageSentId.Add(iMessageDetailsId);
                        }
                    }
                }
                if (hidQueryStrViewMode.Value == "Message sent successfully !!!")
                    hidQueryStrViewMode.Value = "";

                if (hidQueryStrViewMode.Value == S_FLAG_SHOW_INBOX || hidQueryStrViewMode.Value == "")
                {
                    oMessageReceiverDetailsCollectionBL.ArchiveMessagesFromInbox(oArrMessageReceiverId);
                }
                else if (hidQueryStrViewMode.Value == S_FLAG_SHOW_TRASH)
                {
                    if (oArrMessageReceiverId.Count != 0)
                        oMessageReceiverDetailsCollectionBL.DeArchiveMessagesFromArchivedMessages(oArrMessageReceiverId);
                    if (oArrMessageSentId.Count != 0)
                        oMessageReceiverDetailsCollectionBL.DeArchiveSentMessagesFromArchivedMessages(oArrMessageSentId);
                }
            }
            else
            {
                for (int iRowCount = 0; iRowCount < grdvwMessageInbox.Rows.Count; iRowCount++)
                {
                    ochkDelete = (CheckBox)grdvwMessageInbox.Rows[iRowCount].FindControl(S_CHECK_BOX_DELETE);
                    if (ochkDelete.Checked == true)
                    {
                        int iMessageReceiverDetailsId =
                                         Convert.ToInt32(grdvwMessageInbox.DataKeys[iRowCount][I_DATAKEY_MESSAGE_DETAILS_ID]);
                        oArrMessageReceiverId.Add(iMessageReceiverDetailsId);
                    }
                }
                oMessageReceiverDetailsCollectionBL.ArchiveMessagesFromSentItems(oArrMessageReceiverId);
            }
            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set message as unread
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnread_Click(object sender, EventArgs e)
    {
        try
        {
            MarkMessageAsUnread(true);
        }
        catch (Exception ex)
        {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set message as read.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRead_Click(object sender, EventArgs e)
    {
        try
        {
            MarkMessageAsUnread(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to save receive mail or not details and update flag accordingly.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSavePopUp_Click(object sender, EventArgs e)
	{
		try
		{
			SchoolUserBL oSchoolUserBL = new SchoolUserBL();
			oSchoolUserBL.UserId = miUserId;
			oSchoolUserBL.CanReceiveMail = chkReceiveMail.Checked ? Constants.C_YES : Constants.C_NO;
            oSchoolUserBL.Email = txtEmailId.Text.Trim();
			oSchoolUserBL.UpdateSchoolUserReceiveMailFlag();
            hidEmailAddress.Value = oSchoolUserBL.Email;
            hidCanReceiveMail.Value = oSchoolUserBL.CanReceiveMail == Constants.C_YES ? Constants.S_ONE : Constants.S_ZERO;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to update status and redirect to next page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidIsReadReceiptAccepted_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
            string[] sValues = hidIsReadReceiptAccepted.Value.Split(',');

            //if (sValues.Length == 4)
            //{
            //    int iIsAccepted = Convert.ToInt32(sValues[3]);
            //    oMessageReceiverDetailsBL.MarkReadReceiptStatus(miSchoolId, miAcademicYearId, iIsAccepted, sValues[1].ToInt());
            //}

            string sQuerString = "MessageDetailsId=" + sValues[0]
                                + "&MessageReceiverDetailsId=" + sValues[1]
                                + "&pIndex=" + sValues[2]
                                + "&pSortExp=" + hidSortExpression.Value
                                + "&pSortDirc=" + hidSortDirection.Value
                                + "&Mode=" + hidQueryStrViewMode.Value;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);
            Response.Redirect("~/RITeSchool/Common/MessageViewUI.aspx?" + sEncrypt, false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete messages permanently.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteFromEveryOne_Click(object sender, EventArgs e)
    {
        try
        {
            bool bIsSuperAdmin = false;
            if ((Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null && Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID].ToString() != string.Empty) || moUserRole == Constants.UserRoles.Admin)
                bIsSuperAdmin = true;
            string sIds = GetSeletedMessages();
            MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
            oMessageDetailsBL.DeleteMessagePermanently(miSchoolId, miAcademicYearId, miUserId, sIds, bIsSuperAdmin);
            lblMessage.Visible = true;
            lblMessage.Text = "Message(s) deleted successfully!!!";
            imgBtnSentItems_Click(imgBtnSentItems, null);
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
                int iMessageId = Convert.ToInt32(grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_MESSAGE_DETAILS_ID].ToString());
                int iSendMessageId = Convert.ToInt32(grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_MESSAGE_RECEIVER_DETAILS_ID].ToString());
                string sAttachment = Convert.ToString(grdvwMessageInbox.DataKeys[e.Row.RowIndex]["Attatchment"]);
                
                HyperLink olnkSubject = (HyperLink)e.Row.Cells[I_SUBJECT_COLUMN_INDEX].Controls[0];
                Image oStatusImage = (Image)e.Row.Cells[I_READ_FLAG_IMG_COLUMN_INDEX].Controls[0];

                string sQuerString = "MessageDetailsId=" + iMessageId
                             + "&MessageReceiverDetailsId=" + iSendMessageId
                             + "&pIndex=" + grdvwMessageInbox.PageIndex.ToString()
                             + "&pSortExp=" + hidSortExpression.Value
                             + "&pSortDirc=" + hidSortDirection.Value
                             + "&Mode=" + hidQueryStrViewMode.Value
                             + "&IsReadReceiptAccepted=" + hidIsReadReceiptAccepted.Value
                             + "&SearchText=" + txtSearch.Text
                             + "&SearchDate=" + txtSearchDate.Text
                             + "&Operator=" + cmbOperation.SelectedIndex
                             +"&AcademicYearId="+cmbAcademicYear.SelectedValue;
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);

                oStatusImage.Attributes.Add("onclick", "window.open('../Common/MessageViewUI.aspx?" + sEncrypt
                                                                    + "' , '_self'); return false;");

                olnkSubject.Attributes.Add("onclick", "window.open('../Common/MessageViewUI.aspx?" + sEncrypt + "' , '_self'); return false;");
                olnkSubject.NavigateUrl = "~/RITeSchool/Common/MessageViewUI.aspx?" + sEncrypt;

                if (hidQueryStrViewMode.Value != S_FLAG_SHOW_SENT_ITEMS)
                {
                    if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_READ_FLAG].ToString() == Constants.C_NO.ToString())
                    {
                        oStatusImage.ImageUrl = S_IMG_FOR_UNREAD_MSG;
                        e.Row.Font.Bold = true;
                        mbReadMessage = true;
                    }
                    else if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_READ_FLAG].ToString() == Constants.C_YES.ToString())
                    {
                        oStatusImage.ImageUrl = S_IMG_FOR_READ_MSG;
                        e.Row.Font.Bold = false;
                        mbUnreadMessage = true;
                    }
                    if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_REPLY_FORWARD_FLAG].ToString() == Constants.C_YES.ToString())
                    {
                        oStatusImage.ImageUrl = S_IMG_FOR_FORWARD_MSG;
                        if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_READ_FLAG].ToString() == Constants.C_NO.ToString())
                            e.Row.Font.Bold = true;
                        else
                            e.Row.Font.Bold = false;
                        oStatusImage.ToolTip = "Forwarded";
                    }
                    if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_REPLY_FORWARD_FLAG].ToString() == Constants.C_NO.ToString())
                    {
                        oStatusImage.ImageUrl = S_IMG_FOR_REPLY_MSG;
                        if (grdvwMessageInbox.DataKeys[e.Row.RowIndex][I_DATAKEY_READ_FLAG].ToString() == Constants.C_NO.ToString())
                            e.Row.Font.Bold = true;
                        else
                            e.Row.Font.Bold = false;
                        oStatusImage.ToolTip = "Replied";
                    }
                }
                else
                {
                    DateTime dt = grdvwMessageInbox.DataKeys[e.Row.RowIndex]["Insert_Date"].ToDateTime();

                    if (dt > DateTime.Now)
                    {
                        e.Row.Font.Bold = true;
                        e.Row.ForeColor = System.Drawing.Color.Navy;
                        e.Row.ToolTip = "Scheduled message.";
                    }
                }

                bool bRequestReadReceipt = Convert.ToBoolean(grdvwMessageInbox.DataKeys[e.Row.RowIndex]["RequestReadReceipt"]);               
                                    
                //bool bIsFirstTime = false;
                //if (grdvwMessageInbox.DataKeys[e.Row.RowIndex]["ReadingDateTime"] == null || grdvwMessageInbox.DataKeys[e.Row.RowIndex]["ReadingDateTime"] == DBNull.Value)
                //    bIsFirstTime = true;

                //int iShowMessage = 0;
                //if (bRequestReadReceipt && bIsFirstTime && (hidQueryStrViewMode.Value.Trim() != S_FLAG_SHOW_TRASH && hidQueryStrViewMode.Value.Trim() != S_FLAG_SHOW_SENT_ITEMS))
                //    iShowMessage = 1;

                //olnkSubject.Attributes.Add("onclick", "ShowReadReceiptConfirmation(" + iMessageId + "," + iSendMessageId + "," + grdvwMessageInbox.PageIndex + "," + iShowMessage + ")");
                

                if (e.Row.Cells[I_READ_RECEIPT_COLUMN_INDEX].Controls.Count > 0)
                {
                    HyperLink lnkReadReceipt = (HyperLink)e.Row.Cells[I_READ_RECEIPT_COLUMN_INDEX].Controls[0];
                    
                    if (bRequestReadReceipt)
                    {
                        lnkReadReceipt.Visible = true;
                        bool bHasReadReceipt = Convert.ToBoolean(grdvwMessageInbox.DataKeys[e.Row.RowIndex]["HasReadReceipt"]);
                        if (bHasReadReceipt)
                        {
                            lnkReadReceipt.Text = "View";
                            lnkReadReceipt.Enabled = true;
                            string sEncryptedQueryString = Utility.CommonUtility.EncryptQuerystring("MessageDetailId=" + iMessageId);                            
                            lnkReadReceipt.Attributes.Add("onclick", "DisplayReadReceiptDetails('" + sEncryptedQueryString + "')");
                        }
                        else
                        {
                            lnkReadReceipt.Text = "Requested";
                            lnkReadReceipt.Enabled = false;
                        }
                    }
                    else
                        lnkReadReceipt.Visible = false;

                    if(hidQueryStrViewMode.Value != S_FLAG_SHOW_SENT_ITEMS)
                        lnkReadReceipt.Visible = false;
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
    /// This method is used to handle paging changed
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
            FillGridAsPerViewMode();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle Selected event of datasource object
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdvwMessageInbox.PageSize * grdvwMessageInbox.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwMessageInbox.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty) 
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0" || grdvwMessageInbox.PageCount == 0)
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
    /// This method is used to handle gridview databound events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwMessageInbox_DataBound(object sender, EventArgs e)
    {
        try
        {
            SetControlsPropertyAtPageLoad();
            if (mbReadMessage == true && mbUnreadMessage == false)
            {
                btnRead.Enabled = true;
                btnUnread.Enabled = false;
            }
            if (mbReadMessage == false && mbUnreadMessage == true)
            {
                btnRead.Enabled = false;
                btnUnread.Enabled = true;
            }
            if (mbReadMessage == true && mbUnreadMessage == true)
            {
                btnRead.Enabled = true;
                btnUnread.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to search Name & message text in messages.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {        
        grdvwMessageInbox.PageIndex = Constants.I_ZERO;
        FillGridAsPerViewMode();
    }
    #endregion

    #region Draft ListViewEvents

    /// <summary>
    /// This event is used to handle list view Item databound events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstDraftMessageDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                MessageDraftDetails oMessageDraftDetails = e.Item.DataItem as MessageDraftDetails;
                ImageButton imgBtnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("onclick", "if(!DeleteDraftMessage()) return false;");
                Label lblDraftDate = e.Item.FindControl("lblDraftDate") as Label;
                lblDraftDate.Text = oMessageDraftDetails.DraftDate.ToString(Constants.S_DATE_FORMAT);
                HyperLink hlnkSubject = e.Item.FindControl("hlnkSubject") as HyperLink;
                hlnkSubject.Text = oMessageDraftDetails.Subject;
                HyperLink hlnkMessageBody = e.Item.FindControl("hlnkMessageBody") as HyperLink;
                hlnkMessageBody.Text = HttpUtility.HtmlDecode(oMessageDraftDetails.MessageBody);
                
                string sQuerString = "MessageDraftId=" + oMessageDraftDetails.DraftId
                                   + "&MessageReceiverDetailsId=" + 0
                                   + "&pIndex=" + 0
                                   + "&pSortExp=" + hidSortExpression.Value
                                   + "&pSortDirc=" + hidSortDirection.Value
                                   + "&Mode=" + "Draft"
                                   + "&IsReadReceiptAccepted=" + hidIsReadReceiptAccepted.Value
                                   + "&SearchText=" + txtSearch.Text
                                   + "&SearchDate=" + txtSearchDate.Text
                                   + "&Operator=" + cmbOperation.SelectedIndex;
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);

                hlnkSubject.Attributes.Add("onclick", "window.open('../Common/MessageViewUI.aspx?" + sEncrypt + "' , '_self'); return false;");
                hlnkMessageBody.Attributes.Add("onclick", "window.open('../Common/MessageViewUI.aspx?" + sEncrypt + "' , '_self'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle listview Item Command events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstDraftMessageDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iDraftId = Convert.ToInt32(lstDraftMessageDetails.DataKeys[e.Item.DisplayIndex]["DraftId"]);
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
                    oMessageDetailsBL.DeleteMessageFromDraft(iDraftId, miUserId, miSchoolId, miAcademicYearId);
                    lblMessage.Visible = true;
                    lblMessage.Text = "Message deleted successfully!!!";
                    FillUsersDraftMessage();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle list view Item Deleting events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstDraftMessageDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e) {  }

    /// <summary>
    /// This method is used to handle list view Item Editing events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstDraftMessageDetails_ItemEditing(object sender, ListViewEditEventArgs e) {  }

    #endregion

    #region Helping Methods

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        trTotalRec.Visible = false;     

        ApplyMouseHoverEffect(new List<Button> { btnArchive, btnDelete, btnUnread, imgBtnNewMessage, imgBtnComposeMessage,btnClosePopUp,btnSavePopUp, btnRead });
        btnUnread.Attributes.Add("Onclick", "if(!(ConfirmUnread('" + grdvwMessageInbox.AllowPaging
                                                                  + "','" + S_SELECT_AT_LEAST_ONE_MESSAGE_UNREAD
                                                                  + "'))){return false;}");
        btnRead.Attributes.Add("Onclick", "if(!(ConfirmRead('" + grdvwMessageInbox.AllowPaging
                                                                  + "','" + S_SELECT_AT_LEAST_ONE_MESSAGE_READ
                                                                  + "'))){return false;}");

        btnDeleteFromEveryOne.Attributes.Add("onclick", "if(!ConfirmTotalDelete()) return false;");

        base.SetDefaultButton(btnSearch);
    }

    /// <summary>
    /// This method is used to fill grid as per view mode.
    /// </summary>
    private void FillGridAsPerViewMode()
    {
        //related to image urls
        const string S_IMG_FOR_HIGHLIGHT_INBOX = "~/RITeSchool/images/IconBtn_InboxActive.gif";
        const string S_IMG_FOR_HIGHLIGHT_TRASH = "~/RITeSchool/images/IconBtn_ArchivedMsgsActive.gif";
        const string S_IMG_FOR_HIGHLIGHT_SENT_ITEMS = "~/RITeSchool/images/IconBtn_SentMsgsActive.gif";        
        btnDelete.Visible = true;
        btnDeleteFromEveryOne.Visible = false;
        grdvwMessageInbox.DataSourceID = GrdDSobj.ID;
        switch (hidQueryStrViewMode.Value)
        {
            case S_FLAG_SHOW_INBOX:
                trSearchDetails.Visible = true;
                grdvwMessageInbox.Visible = true;
                imgBtnInbox.ImageUrl = S_IMG_FOR_HIGHLIGHT_INBOX;
                btnArchive.Text = "Trash";
                btnArchive.CssClass = "ClsBtnSml";
                DisplayMessageInbox();
                btnDelete.Visible = false;
                break;
            case S_FLAG_SHOW_TRASH:
                trSearchDetails.Visible = true;
                grdvwMessageInbox.Visible = true;
                imgBtnShowTrash.ImageUrl = S_IMG_FOR_HIGHLIGHT_TRASH;
                btnArchive.Text = "Un-Delete";
                btnArchive.CssClass = "ClsBtnMid";
                FillTrashMessagesGrid();
                break;
            case S_FLAG_SHOW_SENT_ITEMS:
                trSearchDetails.Visible = true;
                grdvwMessageInbox.Visible = true;
                imgBtnSentItems.ImageUrl = S_IMG_FOR_HIGHLIGHT_SENT_ITEMS;
                btnArchive.Text = "Trash";
                btnDelete.Visible = false;
                btnDeleteFromEveryOne.Visible = true;
                FillSentItemsGrid();
                break;
            case S_FLAG_SHOW_DRAFT:
                lstDraftMessageDetails.Visible = true;
                grdvwMessageInbox.Visible = false;
                imgBtnDraft.ImageUrl = S_IMG_FOR_HIGHLIGHT_DRAFT_ITEMS;
                FillUsersDraftMessage();
                break;
            default:
                imgBtnInbox.ImageUrl = S_IMG_FOR_HIGHLIGHT_INBOX;
                btnArchive.Text = "Trash";
                btnArchive.CssClass = "ClsBtnSml";
                DisplayMessageInbox();
                btnDelete.Visible = false;
                break;
        }
        DisplayUnreadMessageCount();
    }

    /// <summary>
    /// This method is used to hide controls if grid is absent and if 
    /// grid is present set attributes property at page load.
    /// </summary>
    private void SetControlsPropertyAtPageLoad()
    {
        if (grdvwMessageInbox.Rows.Count == 0)
        {
            btnArchive.Visible = false;
            btnDelete.Visible = false;
            btnUnread.Visible = false;
            btnRead.Visible = false;
        }
        else
        {
            btnArchive.Visible = true;
            ConfirmationForDeleteOrTrashMessages();
        }
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void GetQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());

                msQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
                if (msQueryString == Constants.S_MESSAGE_SENT_SUCCESSFULLY)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = msQueryString;
                }
                moHttprequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                                Page.Request.Url.ToString(),
                                                msQueryString);
                if (moHttprequest.QueryString["Mode"] != null)
                    hidQueryStrViewMode.Value = Convert.ToString(moHttprequest.QueryString["Mode"]);
                if (moHttprequest.QueryString["pIndex"] != null)
                    grdvwMessageInbox.PageIndex = Convert.ToInt32(moHttprequest.QueryString["pIndex"]);
                if (moHttprequest.QueryString["pSortExp"] != null)
                    hidSortExpression.Value = moHttprequest.QueryString["pSortExp"];
                if (moHttprequest.QueryString["pSortDirc"] != null)
                {
                    hidSortDirection.Value = moHttprequest.QueryString["pSortDirc"];
                    if (!IsPostBack)
                    {
                        if (moHttprequest.QueryString["SearchText"] != null)
                        txtSearch.Text = moHttprequest.QueryString["SearchText"];
                        if (moHttprequest.QueryString["SearchDate"] != null)
                        txtSearchDate.Text = moHttprequest.QueryString["SearchDate"];
                        if(moHttprequest.QueryString["Operator"] != null && moHttprequest.QueryString["Operator"].ToString() != string.Empty)
                            cmbOperation.SelectedIndex = moHttprequest.QueryString["Operator"].ToInt();
                    }
                }
                else
                    hidQueryStrViewMode.Value = msQueryString;              
                    
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
        hidSortExpression.Value = grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_DESCENDING;
        hidCanReceiveMail.Value = string.Empty;
        hidEmailAddress.Value = string.Empty;
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
    /// This method is used to display message details in grid.
    /// </summary>
    private void DisplayMessageInbox()
    {
        SetGridViewDateColumnProperties();
        InitializeFields();
        grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "From";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Received Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = true;
        grdvwMessageInbox.Columns[I_READ_RECEIPT_COLUMN_INDEX].Visible = false;
        grdvwMessageInbox.Columns[I_CC_USER_NAME_COLUMN_INDEX].Visible = false;
    }

    /// <summary>
    /// This method is used to fill archived messages from the grid.
    /// </summary>
    private void FillTrashMessagesGrid()
    {
        SetGridViewDateColumnProperties();
        InitializeFields();
        grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "From / To";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Received Date/ Sent Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = true;
        grdvwMessageInbox.Columns[I_READ_RECEIPT_COLUMN_INDEX].Visible = false;
        grdvwMessageInbox.Columns[I_CC_USER_NAME_COLUMN_INDEX].Visible = true;
    }

    /// <summary>
    /// This method is used to fill sent items in the grid.
    /// </summary>
    private void FillSentItemsGrid()
    {
        SetGridViewDateColumnProperties();
        InitializeFields();
        grdvwMessageInbox.Columns[I_FROM_USER_NAME_COLUMN_INDEX].HeaderText = "To";
        grdvwMessageInbox.Columns[I_RECEIVED_DATE_COLUMN_INDEX].HeaderText = "Sent Date";
        grdvwMessageInbox.Columns[I_READ_FLAG_IMG_COLUMN_INDEX].Visible = false;
        grdvwMessageInbox.Columns[I_READ_RECEIPT_COLUMN_INDEX].Visible = true;
        grdvwMessageInbox.Columns[I_CC_USER_NAME_COLUMN_INDEX].Visible = true;
    }

    /// <summary>
    /// This method is used to show confirmation messages for delete, archive actions.
    /// </summary>
    private void ConfirmationForDeleteOrTrashMessages()
    {
        if (hidQueryStrViewMode.Value == "Message sent successfully !!!")
            hidQueryStrViewMode.Value = "";

        btnDelete.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdvwMessageInbox.AllowPaging
                                                                   + "','" + S_SELECT_AT_LEAST_ONE_MESSAGE_DELETE
                                                                   + "'))){return false;}");
        if (hidQueryStrViewMode.Value == S_FLAG_SHOW_INBOX || hidQueryStrViewMode.Value == "" || hidQueryStrViewMode.Value == S_FLAG_SHOW_SENT_ITEMS)
        {
            btnArchive.Attributes.Add("Onclick", "if(!(ConfirmDeArchive('" + grdvwMessageInbox.AllowPaging
                                                                     + "','" + S_SELECT_AT_LEAST_ONE_MESSAGE_TRASH
                                                                     + "'))){return false;}");
        }
        else if (hidQueryStrViewMode.Value == S_FLAG_SHOW_TRASH)
        {
            btnArchive.Attributes.Add("Onclick", "if(!(ConfirmDeArchive('" + grdvwMessageInbox.AllowPaging
                                                                        + "','" + S_SELECT_AT_LEAST_ONE_MESSAGE_UNDELETE
                                                                        + "'))){return false;}");
        }
    }

    /// <summary>
    /// This method is used to update "New_Message_Flag" 'N' 
    /// from 'Y' for user at each page load of the inbox.
    /// </summary>
    private void UpdateNewMessageFlag()
    {        
        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
        oMessageReceiverDetailsBL.UpdateNewMessageFlag(miUserId, Convert.ToInt32(cmbAcademicYear.SelectedValue));
    }

    /// <summary>
    /// This method is used to set resources values to hidden field.
    /// </summary>
    private void RefreshValue()
    {        
        hidEmailShouldNotBlank.Value = Resources.LocalizedResources.EmailShouldNotBlank;
        hidEmailValidation.Value = Resources.LocalizedResources.EmailValidation;      
    }

    /// <summary>
    /// This method is used to display count of unread messages of inbox as well as trash messages.
    /// </summary>
    private void DisplayUnreadMessageCount()
    {             
        MessageReceiverDetailsCollectionBL oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();
        int iUnreadMessageCount = 0;
        int iAcademicYearId = Convert.ToInt32(cmbAcademicYear.SelectedValue);
        if (hidQueryStrViewMode.Value == S_FLAG_SHOW_INBOX || hidQueryStrViewMode.Value == Constants.S_MESSAGE_SENT_SUCCESSFULLY || hidQueryStrViewMode.Value == "")
        {
            iUnreadMessageCount = oMessageReceiverDetailsCollectionBL.GetCountOfUnreadInboxMessageForUser(miUserId, iAcademicYearId, txtSearch.Text, txtSearchDate.Text, cmbOperation.SelectedValue);
        }
        else if (hidQueryStrViewMode.Value == S_FLAG_SHOW_TRASH)
        {
            iUnreadMessageCount = oMessageReceiverDetailsCollectionBL.GetCountOfUnreadArchivedMessageForUser(miUserId, iAcademicYearId, txtSearch.Text, txtSearchDate.Text, cmbOperation.SelectedValue);
        }
        if (iUnreadMessageCount > 0)
        {
            lblUnreadMessage.Visible = true;
            tdUnreadMessage.Visible = true;
            if (iUnreadMessageCount == 1)
                lblUnreadMessage.Text = iUnreadMessageCount.ToString() + " " + S_UNREAD_MESSEGE;
            else
                lblUnreadMessage.Text = iUnreadMessageCount.ToString() + " " + S_UNREAD_MESSEGES;
        }
        else
        {
            lblUnreadMessage.Visible = false;
            tdUnreadMessage.Visible = false;
        }
    }

	/// <summary>
	/// This method is used to set email address to textbox on popup.
	/// </summary>
	private void SetEmailAddressOnPopup()
	{		
		SchoolUserBL oSchoolBL = new SchoolUserBL(miUserId);
		hidEmailAddress.Value = txtEmailId.Text = oSchoolBL.Email;
        hidCanReceiveMail.Value = oSchoolBL.CanReceiveMail == Constants.C_YES ? Constants.S_ONE : Constants.S_ZERO;
        chkReceiveMail.Checked = hidCanReceiveMail.Value.ToInt().ToBool();
	}


    /// <summary>
    /// Thi method is used to mark the message as unread.
    /// </summary>
    private void MarkMessageAsUnread(bool abMarkAsUnread)
    {
        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
        oMessageReceiverDetailsBL.SetMessageFlagToUnread(GenerateXML() , abMarkAsUnread);
        grdvwMessageInbox.DataSourceID = GrdDSobj.ID;
        DisplayUnreadMessageCount();
    }

    /// <summary>
    /// This method is used to generate xml for message details id.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("MessageId");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "MessageId", "");
        for (int i = 0; i < grdvwMessageInbox.Rows.Count; i++)
        {
            CheckBox oChkSelect = grdvwMessageInbox.Rows[i].FindControl("ChkBoxDelete") as CheckBox;
            if (oChkSelect.Checked)
            {
                XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "MessageId", "");

                sAttribute = "MesssageDetailsId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdvwMessageInbox.DataKeys[i]["Message_Details_Id"].ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "MessageReceiverDetailsId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdvwMessageInbox.DataKeys[i]["Message_Receiver_Details_Id"].ToString();
                oXMLNode.Attributes.Append(oAttr);

                oXmlRootNode.AppendChild(oXMLNode);
            }
            oElement.AppendChild(oXmlRootNode);
        }
        return oElement.InnerXml;
    }

    private void FillOperators()
    {
        List<Operator> olstOperators = StudentBL.GetOperators();
        ListSource.FillDropDownList(olstOperators, cmbOperation, "Text", "Value", string.Empty);
        cmbOperation.SelectedIndex = Constants.I_ZERO;
    }

    /// <summary>
    /// This mwthod is used to used to fill academic year combobox.
    /// </summary>
    /// <returns></returns>
    private void FillAcademicYearCombo()
    {
        if (Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] != null)
            miAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();

        var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId, miUserId, moUserRole.ToInt());
        moDtAcademicAndYearInfo = oDtYearInfo;
        cmbAcademicYear.Bind(moDtAcademicAndYearInfo, S_ACADEMIC_YEAR_ID, S_YEAR_VALUE, string.Empty);

        if (QueryString["AcademicYearId"] != null && QueryString["AcademicYearId"].ToString() != string.Empty)
        {
            cmbAcademicYear.SelectedValue = QueryString["AcademicYearId"].ToString();
            btnSearch_Click(btnSearch,null);
        }
        else
            cmbAcademicYear.SelectedValue = Convert.ToString(miAcademicYearId);
    }

    /// <summary>
    /// This method is used to fill Draft messge listview.
    /// </summary>
    private void FillUsersDraftMessage()
    {
        SetDraftMode();

        hidQueryStrViewMode.Value = S_FLAG_SHOW_DRAFT;
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
        List<MessageDraftDetails> lstMessageDraftDetails = oMessageDetailsBL.GetDraftDetails(miSchoolId, Convert.ToInt32(cmbAcademicYear.SelectedValue.ToString()), miUserId);
        lstDraftMessageDetails.DataSource = lstMessageDraftDetails;
        lstDraftMessageDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set the visibility of the controls for Draft.
    /// </summary>
    private void SetDraftMode()
    {
        trTotalRec.Visible = false;
        tdUnreadMessage.Visible = false;
        grdvwMessageInbox.Visible = false;
        btnRead.Visible = false;
        btnUnread.Visible = false;        
        btnDelete.Visible = false;
        btnArchive.Visible = false;
        trSearchDetails.Visible = false;
    }

    /// <summary>
    /// This method is used to hide draft and sent button for student login For only HIS school
    /// </summary>
    private void SetButtonState()
    {
        if (moUserRole == Constants.UserRoles.Student && Settings.EnableMessageCenterReadModeForStudent)
        {
            imgBtnDraft.Visible = false;
            imgBtnSentItems.Visible = false;
            imgBtnComposeMessage.Visible = false;
            imgBtnNewMessage.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to return selected messaGES IDS.
    /// </summary>
    /// <returns></returns>
    private string GetSeletedMessages()
    {
        StringBuilder sb = new StringBuilder();
        for (int iIndex = 0; iIndex < grdvwMessageInbox.Rows.Count; iIndex++)
        {
            CheckBox chkSelect = grdvwMessageInbox.Rows[iIndex].FindControl("ChkBoxDelete") as CheckBox;
            if (chkSelect.Checked)
                sb.Append("," + grdvwMessageInbox.DataKeys[iIndex]["Message_Details_Id"].ToString());
        }

        string sIds = sb.ToString().Substring(1);
        return sIds;
    }

    #endregion      

}


