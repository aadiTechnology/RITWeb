using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Linq;
using SchoolEntities;
using System.Web.UI.HtmlControls;
public partial class MessageViewUI : SchoolBase
{
    #region Constants

    const string S_QSTR_MSG_DETAILS_ID = "MessageDetailsId";
    const string S_QSTR_MSG_RECEIVER_DETAILS_ID = "MessageReceiverDetailsId";
    const string S_QSTR_MSG_MODE = "Mode";
    const string S_MODE_SHOW_INBOX = "ShowInbox";
    const string S_MODE_SHOW_SENT_ITEMS = "ShowSentItems";

    #endregion

    /// <summary>
    /// This method is used to hadle page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
          bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
          if (bIsUseSubmitBehavior)
          {
              GetQuerystring();
          }
          if (!IsPostBack)
          {
              if (QueryString["AcademicYearId"] != null && QueryString["AcademicYearId"].ToString() != string.Empty)
                  hidAccYearId.Value = QueryString["AcademicYearId"].ToString();

              if (hidDraftMessage.Value != Constants.S_ZERO)
              {
                  FillDraftDetails();
              }
              else
              {
                  InitializePage();
                  IsReadReceiptRequested();
                  base.SetDocType();
              }
              SetButtonState();
              RestrictFields();
          }         
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update Read Receipt status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateReadReceiptStatus(Constants.I_ONE);
            hidShowRequestMessage.Value = Constants.S_ZERO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update Read Receipt status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateReadReceiptStatus(Constants.I_ZERO);
            hidShowRequestMessage.Value = Constants.S_ZERO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This function is used to sending reply to the message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReply_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnReply.Text == "Edit")
            {
                int iId = hidDraftMessage.Value.ToInt();
                string sQueryString = "DraftMessageId=" + iId;
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sEncrypt);
            }
            else
            {
                int iMessageDetailsId = QueryString[S_QSTR_MSG_DETAILS_ID].ToInt();
                string sReplyToAll = "false", sReply = "true";
                string sQueryString = String.Format("MessageID={0}&ReplyToAll={1}&Reply={2}", iMessageDetailsId, sReplyToAll, sReply);
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sEncrypt);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to go back to the previous page which is Message Inbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void btnGoToInbox_Click(object sender, EventArgs e)
    {
        try
        {
            string[] sArrValue;
            string asQueryString = CommonUtility.DecryptQuerystring(HidBackUrl.Value);
            string[] sArrElements = asQueryString.Split('&');
            StringBuilder oStringBuilder = new StringBuilder();
            foreach (string key in sArrElements)
            {
                sArrValue = key.Split('=');
                if (sArrValue[0] == "Mode")
                    sArrValue[1] = "ShowInbox";

                if (sArrValue[0] == "SearchText")
                    sArrValue[1] = string.Empty;

                if (sArrValue[0] == "SearchDate")
                    sArrValue[1] = string.Empty;

                if (sArrValue[0] == "Operator")
                    sArrValue[1] = Constants.S_ZERO;
                   
                oStringBuilder.Append("&" + sArrValue[0] + "=" + sArrValue[1]);                   
            }

            if (oStringBuilder.Length > 0)
                asQueryString = oStringBuilder.ToString().Substring(1);
            HidBackUrl.Value = CommonUtility.EncryptQuerystring(asQueryString);

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Common/MessageInbox.aspx?" + HidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to return back to last page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Common/MessageInbox.aspx?" + HidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function is used to sending reply to the message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReplyToAll_Click(object sender, EventArgs e)
    {
        try
        {
            int iMessageDetailsId = QueryString[S_QSTR_MSG_DETAILS_ID].ToInt();
            string sReplyToAll = "true";
            string sQueryString = "MessageID=" + iMessageDetailsId + "&ReplyToAll=" + sReplyToAll;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sEncrypt);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to forword the message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnForward_Click(object sender, EventArgs e)
    {
        try
        {
            int iMessageDetailsId = QueryString[S_QSTR_MSG_DETAILS_ID].ToInt();
            string sReplyToAll = "true";
            string sQueryString = "MessageID=" + iMessageDetailsId + "&IsForward=" + sReplyToAll;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sEncrypt);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to initialize page.
    /// </summary>
    private void InitializePage()
    {
        UpdateReadMessageFlag();
        SetMessageDetailsToRespectiveControls();
        ApplyMouseHoverEffect(new List<Button> { btnForward, btnGoToInbox, btnReply, btnReplyToAll, btnBack });
        SetDefaultButton(btnGoToInbox);
        btnSavePopUp.Attributes.Add("Onclick", "HidePopup()");
        btnClosePopUp.Attributes.Add("Onclick", "HidePopup()");
    }

    /// <summary>
    /// This method is used to set attachment
    /// </summary>
    /// <param name="sAttachment"></param>
    private void SetAttachment(FileAttachment aoAttachment, HyperLink lnkAttachments)
    {
        string sAttachment = aoAttachment.FileName;
        string sAttachmentURL = sAttachment;
        sAttachment = sAttachment.Replace("'", "\\\'");
        sAttachment = sAttachment.Replace("%", "%25");
        sAttachment = sAttachment.Replace("#", "%23");        
        //string sServerFilePath = "../Uploads/" + sAttachment;
        string sServerFilePath = "../Common/DownloadFileUI.aspx?" + CommonUtility.EncryptQuerystring("FileTypeId=1&AttachmentId=" + aoAttachment.Id);

        // Since 10-Sept-2011, we are appending a Timestamp to every file attached with a message.
        // For files uploaded prior to that, there is no timestamp. Hence there is a special separator - $ added between
        // the Filename and Timestamp, to differentiate between non-timestamped & timestamped attachments.
        // If the Attachment containts that special separator, we remove it along with the actual timestamp.
        int iTimestampIndex = sAttachment.IndexOf("$");
        if (iTimestampIndex > -1)
            sAttachment = sAttachment.Remove(iTimestampIndex, 15);     

        int iIndex = sAttachmentURL.IndexOf("$");
        if (iIndex > -1)
            sAttachmentURL = sAttachmentURL.Remove(iIndex, 15);
        lnkAttachments.Text = sAttachmentURL;
        string sExtention = sAttachment.Substring(sAttachment.LastIndexOf(".") + 1).ToUpper();
        string sExtensionMap = "PDF,JPG";
        lnkAttachments.Attributes.Add("onclick",
                                     String.Format("window.open('{0}','{1}'); return false;",
                                                    sServerFilePath,
                                                    sExtensionMap.IndexOf(sExtention) > -1 ? "_blank" : "_self"));
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void GetQuerystring()
    {
        try
        {
            if (QueryString["MessageDraftId"].ToInt() != Constants.I_ZERO)
            {
                hidDraftMessage.Value = QueryString["MessageDraftId"].ToString();
            }
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
                string msQueryString = CommonUtility.DecryptQuerystring(sTestDecrypt);

                if (!QueryString["ReceiverUserId"].IsNull())
                {
                    if (QueryString["ReceiverUserId"].ToInt() != miUserId)
                        Response.Redirect("ControlPanel.aspx", false);

                    msQueryString = msQueryString.Substring(0, msQueryString.IndexOf("&ReceiverUserId"));
                    sTestDecrypt = CommonUtility.EncryptQuerystring(msQueryString);
                }
                HidBackUrl.Value = sTestDecrypt;

                if (QueryString["MessageReceiverDetailsId"].ToInt() == Constants.I_ZERO)
                    spReceivedDate.InnerText = "Sent Date :";
            }
        }
        catch
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to update read message flag.
    /// </summary>
    private void UpdateReadMessageFlag()
    {
        int iMessageReceiverDetailsId = QueryString[S_QSTR_MSG_RECEIVER_DETAILS_ID].ToInt();
        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
        oMessageReceiverDetailsBL.UpdateUnreadMessageFlag(miUserId, iMessageReceiverDetailsId);
    }

    /// <summary>
    /// This method is used to update read message flag.
    /// </summary>
    private void IsReadReceiptRequested()
    {
        int iMessageReceiverDetailsId = QueryString[S_QSTR_MSG_RECEIVER_DETAILS_ID].ToInt();
        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
        hidShowRequestMessage.Value = oMessageReceiverDetailsBL.IsReadReceiptRequested(miUserId, iMessageReceiverDetailsId).ToString();
    }

    /// <summary>
    /// This method is used to display message details of selected message.
    /// </summary>
    /// 

    //private List<string> GetFileAttachment()
    //{
    //    List<string> lstFiles = new List<string>();
    //    MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
    //    foreach (string file in oMessageDetailsBL.Attachments)
    //    {
    //        HyperLink hyper = new HyperLink();
    //        hyper.Target = "_blank";
    //        SetAttachment(file, hyper);
    //        pnl.Controls.Add(hyper);

    //        Literal lit = new Literal();
    //        lit.Text = "<br>  </br>";
    //        pnl.Controls.Add(lit);
    //    }
    //    return lstFiles;
    //}


    private void SetMessageDetailsToRespectiveControls()
    {
        int iMessageDetailsId = QueryString[S_QSTR_MSG_DETAILS_ID].ToInt();
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL(iMessageDetailsId);

        if (oMessageDetailsBL.Insert_Date > DateTime.Now)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Common/MessageInbox.aspx?" + HidBackUrl.Value);

        }

        lblFromUserName.Text = oMessageDetailsBL.UserName;
        lblReceivedDate.Text = oMessageDetailsBL.Insert_Date.DayOfWeek
                               + ", " + oMessageDetailsBL.Insert_Date.ToString(Constants.S_STANDARD_DATE_FORMAT)
                               + " " + oMessageDetailsBL.Insert_Date.ToShortTimeString();
      
        lblSubject.Text = oMessageDetailsBL.Subject;
       // GetFileAttachment();
        //FCKMessageBody.Text = HttpUtility.HtmlDecode(oMessageDetailsBL.Message_Body);
        //FCKMessageBody.Enabled = false;
        divData.InnerHtml = HttpUtility.HtmlDecode(oMessageDetailsBL.Message_Body);
     
        if (moUserRole == Constants.UserRoles.Student)
            SetReplyButtonEnabilityFalse(true, false);
        
        SetReceiverUserList(oMessageDetailsBL.Display_Text, iMessageDetailsId);
        SetReceiverCcUserList(oMessageDetailsBL.Cc_Display_Text, iMessageDetailsId);
        //when massage is sent to entire school
        if (lblToUserName.Text.Contains(Constants.S_ENTIRE_SCHOOL))
        {
            //if its a non-admin User reply to all button is disabled.
            if (moUserRole != Constants.UserRoles.Admin && moUserRole != Constants.UserRoles.Supervisor)
            {
                SetReplyButtonEnabilityFalse(true, false);
            }
        }
        if (lblCcUserName.Text.Contains(Constants.S_ENTIRE_SCHOOL))
        {
            //if its a non-admin User reply to all button is disabled.
            if (moUserRole != Constants.UserRoles.Admin && moUserRole != Constants.UserRoles.Supervisor)
            {
                SetReplyButtonEnabilityFalse(true, false);
            }
        }

        //if (oMessageDetailsBL.Attatchment != null && oMessageDetailsBL.Attatchment != "")
        //{
        //    tdAttachment.Visible = true;
        //    SetAttachment(oMessageDetailsBL.Attatchment,lnkAttachment);
        //}
        //else
        //    tdAttachment.Visible = false;
        //if (oMessageDetailsBL.Attatchment1 != null && oMessageDetailsBL.Attatchment1 != "")
        //{
        //    tdAttachment1.Visible = true;
        //    SetAttachment(oMessageDetailsBL.Attatchment1,lnkAttachment1);
        //}
        //else
        //    tdAttachment1.Visible = false;
        //if (oMessageDetailsBL.Attatchment2 != null && oMessageDetailsBL.Attatchment2 != "")
        //{
        //    tdAttachment2.Visible = true;
        //    SetAttachment(oMessageDetailsBL.Attatchment2,lnkAttachment2);
        //}
        //else
        //    tdAttachment2.Visible = false;

        HtmlTable oTable = new HtmlTable();
        foreach (FileAttachment file in oMessageDetailsBL.Attachments)
        {
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = new HtmlTableCell();

            HyperLink hyper = new HyperLink();
            hyper.Target = "_blank";
            SetAttachment(file, hyper);
            td.Controls.Add(hyper);

            tr.Controls.Add(td);
            oTable.Controls.Add(tr);
        }

        pnl.Controls.Add(oTable);
    }

    /// <summary>
    /// This function is used to set receivers list
    /// </summary>
    private void SetReceiverUserList(string sToNameList, int iMessageDetailsId)
    {        
        string sViewMode= string.Empty;
        if (QueryString.AllKeys.Contains(S_QSTR_MSG_MODE))
        sViewMode = QueryString[S_QSTR_MSG_MODE].ToString();
        /*Get the all receiver list when sToNameList list value is empty along with the following two cases:
         * 1. Logged in user will not be the student
         * 2. Logged in user will be student and user clicked on the message except than the inbox. Here mode empty case is like inbox*/
        if (
            ((moUserRole != Constants.UserRoles.Student) || 
            (moUserRole == Constants.UserRoles.Student && sViewMode != S_MODE_SHOW_INBOX && sViewMode != string.Empty)))
        {
            //MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
            //DataTable oDTReceiverList = oMessageDetailsBL.GetListOfReceiverName(iMessageDetailsId);
            ///* When display name is not empty the  we are retriving two tables
            // * first table conatin username and second table contain the list of receiver*/
            //for (int iRecordCount = 0; iRecordCount < oDTReceiverList.Rows.Count; iRecordCount++)
            //{
            //    string sUserName = oDTReceiverList.Rows[iRecordCount]["UserName"].ToString();
            //    /* If list of reciever not contain retriverd username then add append this other no need to do*/
            //    if (!sToNameList.Contains(sUserName))
            //        sToNameList += sUserName + ", ";
            //}

            /*If name of reciver is greated than the two character then remove last appended comma from the list and set text to the label*/
            sToNameList = sToNameList.TrimAll();
            if (sToNameList.EndsWith(","))
                sToNameList = sToNameList.Substring(0,sToNameList.Length - 1);
        }

       //Set to username to the lable.     
        lblToUserName.Text = sToNameList;

       /*This code is execute in case on except sent message becasue there is no facility to student include student into the to list*/
        if (moUserRole == Constants.UserRoles.Student && sViewMode != S_MODE_SHOW_SENT_ITEMS)
        {
            string sStudentName = string.Empty;
            /*Get current logged in user username*/
            string sUserName = MessageDetailsBL.GetUserName(miUserId, moUserRole.ToInt(), miAcademicYearId);
            sStudentName = sUserName.Replace(" ", string.Empty);
            sToNameList = sToNameList.Replace(" ", string.Empty);
           
            /* If current mode of view message is inbox then set username only name of current logged in user. 
             * Else check that the list of reciever having the name in reciever list then set name to the label*/
            if (sViewMode == S_MODE_SHOW_INBOX || sViewMode == string.Empty)
                lblToUserName.Text = sUserName;
            else if (sToNameList.Contains(sStudentName))
                lblToUserName.Text = sUserName;
            else
                lblToUserName.Text = string.Empty;
        }
    }

    /// <summary>
    /// This function is used to set Cc list.
    /// </summary>
    private void SetReceiverCcUserList(string sCcNameList, int iMessageDetailsId)
    {
        string sViewMode = string.Empty;
        if (QueryString.AllKeys.Contains(S_QSTR_MSG_MODE))
            sViewMode = QueryString[S_QSTR_MSG_MODE].ToString();
        /*Get the all receiver list when sToNameList list value is empty along with the following two cases:
         * 1. Logged in user will not be the student
         * 2. Logged in user will be student and user clicked on the message except than the inbox. Here mode empty case is like inbox*/
        if (sCcNameList == null)
            sCcNameList = "";

        if ((sCcNameList == string.Empty) &&
            ((moUserRole != Constants.UserRoles.Student) ||
            (moUserRole == Constants.UserRoles.Student && sViewMode != S_MODE_SHOW_INBOX && sViewMode != string.Empty)))
        {
            MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
            DataTable oDTCcReceiverList = oMessageDetailsBL.GetListOfCcReceiverName(iMessageDetailsId);
            /* When display name is not empty the  we are retriving two tables
             * first table conatin username and second table contain the list of receiver*/
            for (int iRecordCount = 0; iRecordCount < oDTCcReceiverList.Rows.Count; iRecordCount++)
            {
                string sUserName = oDTCcReceiverList.Rows[iRecordCount]["Cc_UserName"].ToString();
                /* If list of reciever not contain retriverd username then add append this other no need to do*/
                if (!sCcNameList.Contains(sUserName))
                    sCcNameList += sUserName + ", ";
            }

            /*If name of reciver is greated than the two character then remove last appended comma from the list and set text to the label*/
            if (sCcNameList.Length > 2)
                sCcNameList = sCcNameList.Remove(sCcNameList.Length - 2);
        }

        //Set to username to the lable.     
        lblCcUserName.Text = sCcNameList;

        /*This code is execute in case on except sent message becasue there is no facility to student include student into the to list*/
        if (moUserRole == Constants.UserRoles.Student && sViewMode != S_MODE_SHOW_SENT_ITEMS)
        {
            string sStudentName = string.Empty;
            /*Get current logged in user username*/
            MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
            DataTable oDTCcReceiverList = oMessageDetailsBL.GetListOfCcReceiverName(iMessageDetailsId);
            /* When display name is not empty the  we are retriving two tables
             * first table conatin username and second table contain the list of receiver*/
            for (int iRecordCount = 0; iRecordCount < oDTCcReceiverList.Rows.Count; iRecordCount++)
            {
                string sUserName = oDTCcReceiverList.Rows[iRecordCount]["Cc_UserName"].ToString();
                /* If list of reciever not contain retriverd username then add append this other no need to do*/
                if (!sCcNameList.Contains(sUserName))
                    sCcNameList += sUserName + ", ";
            }

            /*If name of reciver is greated than the two character then remove last appended comma from the list and set text to the label*/
            if (sCcNameList.Length > 2)
                sCcNameList = sCcNameList.Remove(sCcNameList.Length - 2);

            string sCcUserName = MessageDetailsBL.GetUserName(miUserId, moUserRole.ToInt(), miAcademicYearId);
            sStudentName = sCcUserName.Replace(" ", string.Empty);
            sCcNameList = sCcNameList.Replace(" ", string.Empty);

            /* If current mode of view message is inbox then set username only name of current logged in user. 
             * Else check that the list of reciever having the name in reciever list then set name to the label*/
            if ((sViewMode == S_MODE_SHOW_INBOX || sViewMode == string.Empty) && sCcNameList.Contains(sStudentName))
                lblCcUserName.Text = sCcUserName;
            else if (sCcNameList.Contains(sStudentName))
                lblCcUserName.Text = sCcUserName;
            else
                lblCcUserName.Text = string.Empty;
        }
    }

    /// <summary>
    /// This method enables/diables the  reply buttons
    /// </summary>
    /// <param name="abinReply">specifies Reply button enable/disable status</param>
    /// <param name="ablnReplyToAll"> specifies Reply To All button enable/disable status</param>
    private void SetReplyButtonEnabilityFalse(Boolean abinReply, Boolean ablnReplyToAll)
    {
        btnReply.Enabled = abinReply;
        btnReplyToAll.Enabled = ablnReplyToAll;
    }

    /// <summary>
    /// This method is used to Update Read Receipt status
    /// </summary>
    /// <param name="aiIsAccepted"></param>
    private void UpdateReadReceiptStatus(int aiIsAccepted)
    {
        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
        int iMessageReceiverDetailsId = QueryString[S_QSTR_MSG_RECEIVER_DETAILS_ID].ToInt();
        oMessageReceiverDetailsBL.MarkReadReceiptStatus(miSchoolId, miAcademicYearId, aiIsAccepted, iMessageReceiverDetailsId);
    }    

    /// <summary>
    /// This method is used fill the draft details to view.
    /// </summary>
    private void FillDraftDetails()
    {
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
        int iDraftId = hidDraftMessage.Value.ToInt();
        spReceivedDate.InnerText = "Draft Date";
        MessageDraftDetails oMessageDraftDetails = oMessageDetailsBL.GetMessageDetailsForDraft(iDraftId, miUserId, miSchoolId,miAcademicYearId);
        lblFromUserName.Text = oMessageDraftDetails.FromName;
        lblReceivedDate.Text = oMessageDraftDetails.DraftDate.ToString(Constants.S_DATE_FORMAT);
        lblSubject.Text = oMessageDraftDetails.Subject;
        //FCKMessageBody.Text = HttpUtility.HtmlDecode(oMessageDraftDetails.MessageBody);
        //FCKMessageBody.Enabled = false;
        divData.InnerHtml = HttpUtility.HtmlDecode(oMessageDraftDetails.MessageBody);        
        if (oMessageDraftDetails.DisplayText != string.Empty)
            lblToUserName.Text = oMessageDraftDetails.DisplayText;
        else
            lblToUserName.Text = "-";

        if (oMessageDraftDetails.DisplayText != string.Empty)
            lblCcUserName.Text = oMessageDraftDetails.CcDisplayText;
        else
            lblCcUserName.Text = "-";

        tdAttachment.Visible = false;
        tdAttachment1.Visible = false;
        tdAttachment2.Visible = false;
        btnForward.Visible = false;
        btnReplyToAll.Visible = false;
        btnReply.Text = "Edit";
    }

    /// <summary>
    /// This method is used to hide forword, reply and replytoall button for student login For only HIS school
    /// </summary>
    private void SetButtonState()
    {
        if (moUserRole == Constants.UserRoles.Student && Settings.EnableMessageCenterReadModeForStudent)
        {
            btnForward.Visible = false;
            btnReply.Visible = false;
            btnReplyToAll.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to restrict fields.
    /// </summary>
    private void RestrictFields()
    {
        if (Settings.RestrictCopyDataFromMessageCenter && moUserRole == Constants.UserRoles.Student)
            hidRestrictCopy.Value = Constants.S_ONE;
        else
            hidRestrictCopy.Value = Constants.S_ZERO;
    }
}
