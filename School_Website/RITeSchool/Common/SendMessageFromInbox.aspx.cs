/*
 *  File Name : - SendMessageFromInbox.aspx.cs
 *  Purpose   : - This class is used to create new message to send multiple users.
 *  Date      : - 18-May-2007
 */

using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using BusinessLogic;
using Utility;
using System.Linq;
using SchoolEntities;
using StudentEntities;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.Text;
using PushNotificationService;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.UI.HtmlControls;
/// <summary>
/// This class is used to send the messge to the selected users.
/// It has 3 modes : 
/// 1. Reply
/// 2. Reply to all
/// 3. New message
/// In new message user selects different receipients from the user list.
/// While in case of reply 
/// </summary>
public partial class SendMessageFromInbox : SchoolBase
{
    #region Constants

    const int I_FILE_SIZE_LIMIT = 52428800;// for 5 MB

    const string S_COMMON_PENDING_FEE_MSG = "Your school fees are pending. Please pay the dues ASAP. For any query contact office. - Accounts Officer.";
    const string S_PENDING_FEE_SUBJECT = "Fee Payment Reminder";
    const string S_CHANGE_ACADEMIC_YEAR_PERIOD = "<p>Please change academic year period as per following details.</p><p>Standard :<br></p><p>Start Date&nbsp;<span _fcktemp=\"1\"></span>:<br></p><p>End Date&nbsp;<span _fcktemp=\"1\"></span>:<br></p><p>Re-Opening Date&nbsp;<span _fcktemp=\"1\"></span>:<br type=\"_moz\"></p>";
    const string S_SOFTWARE_COORDINATOR = "Software Coordinator";
    const string S_CHANGE_ACADEMIC_YEAR_PERIOD_SUBJECT = "Change Academic Year Period";
    const string S_SMS_TEMPLATE_ID = "13";
    const int I_SUPER_ADMIN_ID = 2771;
    const int I_SUPER_ADMIN_ROLE_ID = 11;

    #endregion

    #region Data Members

    MessageDetailsBL moMessageDetailsBL;
    private List<MessageReceiverDetailsBL> MessageReceiverDetailsBLList = new List<MessageReceiverDetailsBL>();
    string msForm = "";
    string sServerFilePath;
    string sFileName;
    
    #endregion

    #region Events


    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            
            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSendMessage.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSendMessageUp.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET"))
                 )
                    FillAttachments();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    //SetControlsIfReplyMsg()

    /// <summary>
    /// This method is used to set default page controls and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetQuerystring();
            if (!IsPostBack)
            {
                if (miSchoolId == Constants.SchoolId.PPSN.ToInt() || miSchoolId == Constants.SchoolId.PPS.ToInt() || miSchoolId == Constants.SchoolId.PPSH.ToInt())
                {
                    chkAdmin.Visible = false;
                    chkAdminCC.Visible = false;
                    HideSwCordinatorOption();
                }
                else
                    chkAdmin.Visible = true;

                    ReadQueryString();
                if (hidDraftId.Value != Constants.S_ZERO)
                {
                    FillDraftDetails();
                    SetDefaultPageControls();                    
                }
                else
                {
                    base.SetDocType();
                    SetDefaultPageControls();
                    SetControlsIfReplyMsg();
                    valSum_SendMessage.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                    SetClientScriptAttributes();
                }

                hidIsPTAMember.Value = Constants.S_NO;
                if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    if (moUserRole != Constants.UserRoles.Teacher)
                    {
                        optParentTeacherAssociation.Visible = true;
                        optCCParentTeacherAssociation.Visible = true;
                    }

                    MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
                    bool bIsPTAMrmber = oMessageDetailsBL.IsPTAMember(miSchoolId,miAcademicYearId,miUserId);
                    if (bIsPTAMrmber)
                    {
                        optStudents.Visible = true;
                        optCCStudents.Visible = true;
                        hidIsPTAMember.Value = Constants.S_YES;
                    }
                }

                btnSendMessage.Attributes.Add("onclick", "SendCKEditorMessage();");
                btnSendMessageUp.Attributes.Add("onclick", "SendCKEditorMessage();");
                btnDraft.Attributes.Add("onclick", "SendCKEditorMessage();");

                SetDefaultFields();
                RestrictFields();
                SetViewAsPerDesingation();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
        

    /// <summary>
    /// This method sets the radio buttons and the query string value.
    /// </summary>
    /// <param name="aiSenderUserRoleId"></param>
    /// <param name="abIsReplyToAll"></param>
    /// <param name="aoMsgDetailsBL"></param>
    private void SetRadioButtons(int aiSenderUserRoleId, string abIsReplyToAll, MessageDetailsBL aoMsgDetailsBL)
    {
        CheckUncheckOptions(false);
        string sIsStudentLevel = string.Empty;
        if (!Boolean.Parse(hidUserHasFullAccess.Value) && ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher) && (optStudents.Checked))
            sIsStudentLevel = "&IsStudentLevel=Y";
        //This logic is for setting fields for sending reply for the massage. 
        if (abIsReplyToAll == "ReplyToAll")
        {
            AddSenderToReplyList(aoMsgDetailsBL);

            if (string.IsNullOrEmpty(HidTeacherId.Value) && aoMsgDetailsBL.Sender_User_Role_Id != Convert.ToInt32(Constants.UserRoles.Teacher))
            {
                if (!string.IsNullOrEmpty(HidStudentId.Value) || !string.IsNullOrEmpty(HidStdDivId.Value))
                {
                    optStudents.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                }
                else if (!string.IsNullOrEmpty(HidSupervisorId.Value))
                {
                    optSupervisor.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                }
                else if (!string.IsNullOrEmpty(HidPTAId.Value))/////////////////////////////
                {
                    optParentTeacherAssociation.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=ParentTeacherAssociation" + "&sUserId=" + hidUserId.Value+"&IsPTAMember=Y" + sIsStudentLevel);
                }
                else if (!string.IsNullOrEmpty(HidTeacherId.Value))
                {
                    optTeachers.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);                    
                }
                else
                {
                    optStudents.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                }              
            }
            else
            {
                optTeachers.Checked = true;
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
            }

            if (string.IsNullOrEmpty(HidTeacherIdCC.Value) && aoMsgDetailsBL.Sender_User_Role_Id != Convert.ToInt32(Constants.UserRoles.Teacher))
            {
                if (!string.IsNullOrEmpty(HidStudentIdCC.Value) || !string.IsNullOrEmpty(HidStdDivIdCC.Value))
                {
                    optCCStudents.Checked = true;
                    hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel);
                }

                else if (!string.IsNullOrEmpty(HidSupervisorIdCC.Value))
                {
                    optCCSupervisor.Checked = true;
                    hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel);
                }

                else if (!string.IsNullOrEmpty(HidPTAIdCC.Value))/////////////////////////////
                {
                    optCCParentTeacherAssociation.Checked = true;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=ParentTeacherAssociation" + "&sUserId=" + hidUserId.Value + "&IsPTAMember=Y" + sIsStudentLevel);
                }


                else if (!string.IsNullOrEmpty(HidTeacherIdCC.Value))
                {
                    optCCTeachers.Checked = true;
                    hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel);                    
                }
                else
                {
                    optCCStudents.Checked = true;
                    hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel);
                }
            }
            else
            {
                optCCTeachers.Checked = true;
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel);
            }
        }
        else if (abIsReplyToAll == "Reply")
        {
            //If this is normal massage send then set radio buttons depending upon login role.
            switch (aoMsgDetailsBL.Sender_User_Role_Id)
            {
                case 1:
                    optTeachers.Checked = true;
                    HidAdminReplyName.Value = aoMsgDetailsBL.UserName;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                    break;
                case 2:
                    optTeachers.Checked = true;
                    HidTeacherId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                    HidTeacherName.Value = aoMsgDetailsBL.UserName;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                    break;

                //case 4:
                //    optParentTeacherAssociation.Checked = true;
                //    HidPTAId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();//////////////////////////
                //    HidPTAName.Value = aoMsgDetailsBL.UserName;
                //    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=ParentTeacherAssociation" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                //    break;

                case 3:
                    optStudents.Checked = true;
                    HidStudentId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                    HidStudentName.Value = aoMsgDetailsBL.UserName;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                    break;
                case 6:
                    optSupervisor.Checked = true;
                    HidSupervisorId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                    HidSupervisorName.Value = aoMsgDetailsBL.UserName;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                    break;
                default:
                    optTeachers.Checked = true;
                    HidTeacherId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                    HidTeacherName.Value = aoMsgDetailsBL.UserName;
                    hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
                    break;
            }
        }
        else if (abIsReplyToAll == "Forward")
        {
            optTeachers.Checked = true;
            HidTeacherId.Value = "";
            HidTeacherName.Value = "";
            hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + "0" + sIsStudentLevel);
        }
    }

    /// <summary>
    /// This method is used to add sender to the reply list.
    /// </summary>
    /// <param name="aoMsgDetailsBL"></param>
    protected void AddSenderToReplyList(MessageDetailsBL aoMsgDetailsBL)
    {
        switch (aoMsgDetailsBL.Sender_User_Role_Id)
        {
            case 1:
                HidAdminReplyName.Value = aoMsgDetailsBL.UserName;
                HidAdminReplyNameCC.Value = aoMsgDetailsBL.UserName;
                break;

            case 2:

                if (string.IsNullOrEmpty(HidTeacherId.Value))
                    HidTeacherId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                else
                    HidTeacherId.Value += ";" + aoMsgDetailsBL.Sender_User_Id.ToString();

                if (string.IsNullOrEmpty(HidTeacherName.Value))
                    HidTeacherName.Value = aoMsgDetailsBL.UserName;
                else
                    HidTeacherName.Value += ", " + aoMsgDetailsBL.UserName;

                break;

            case 3:
                if (string.IsNullOrEmpty(HidStudentId.Value))
                    HidStudentId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                else
                    HidStudentId.Value += ";" + aoMsgDetailsBL.Sender_User_Id.ToString();

                if (string.IsNullOrEmpty(HidStudentName.Value))
                    HidStudentName.Value = aoMsgDetailsBL.UserName;
                else
                    HidStudentName.Value += ", " + aoMsgDetailsBL.UserName;

                break;

            case 6:
                if (string.IsNullOrEmpty(HidSupervisorId.Value))
                    HidSupervisorId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                else
                    HidSupervisorId.Value += ";" + aoMsgDetailsBL.Sender_User_Id.ToString();

                if (string.IsNullOrEmpty(HidSupervisorName.Value))
                    HidSupervisorName.Value = aoMsgDetailsBL.UserName;
                else
                    HidSupervisorName.Value += ", " + aoMsgDetailsBL.UserName;

                break;
            //case 4:
            //    if (string.IsNullOrEmpty(HidPTAId.Value))
            //        HidPTAId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
            //    else
            //        HidPTAId.Value += ";" + aoMsgDetailsBL.Sender_User_Id.ToString();

            //    if (string.IsNullOrEmpty(HidSupervisorName.Value))
            //        HidPTAName.Value = aoMsgDetailsBL.UserName;
            //    else
            //        HidPTAName.Value += ", " + aoMsgDetailsBL.UserName;

            //    break;

            default:
                if (string.IsNullOrEmpty(HidTeacherId.Value))
                    HidTeacherId.Value = aoMsgDetailsBL.Sender_User_Id.ToString();
                else
                    HidTeacherId.Value += ";" + aoMsgDetailsBL.Sender_User_Id.ToString();

                if (string.IsNullOrEmpty(HidTeacherName.Value))
                    HidTeacherName.Value = aoMsgDetailsBL.UserName;
                else
                    HidTeacherName.Value += ", " + aoMsgDetailsBL.UserName;

                break;
        }
    }

    
    /// <summary>
    /// On this event all message details saving database.
    /// 1. Collect receiver details.
    /// 2. Set object for message details.
    /// 3. Save the information.
    /// 4. Redirect to inbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSendMessage_Click(object sender, EventArgs e)
    {
        try
        {    
            CollectReceiverDetails();
            InsertMessageDetails();
            string sMessage = Constants.S_MESSAGE_SENT_SUCCESSFULLY;
            string sEncrypt = CommonUtility.EncryptQuerystring(sMessage);
            string sSubject = txtSubject.Text.Trim();

            if(!chkScheduleMessages.Checked)
                SendPushNotification(hidUserId.Value.ToString(), sSubject);
            
            if (hidDraftId.Value != Constants.S_ZERO)
                RemoveMessaegFromDraft();
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Common/MessageInbox.aspx?" + sEncrypt);
            
        }
        catch (UploadFileExceptions ex)
        {
            lblErr.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the attached file and upload new file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChanegAttachment_Click(object sender, EventArgs e)
    {
        try
        {
            txtToUserId.Text = HidReciepents.Value;
            trfileupload.Visible = true;
            tdAttachment.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the second attached file and upload new file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChanegAttachment1_Click(object sender, EventArgs e)
    {
        try
        {
            txtToUserId.Text = HidReciepents1.Value;
            trfileupload1.Visible = true;
            tdAttachment1.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the third attached file and upload new file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChanegAttachment2_Click(object sender, EventArgs e)
    {
        try
        {
            txtToUserId.Text = HidReciepents2.Value;
            trfileupload2.Visible = true;
            tdAttachment2.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to refresh the session after specific time interval.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void timer_Tick(object sender, EventArgs e)
    {
        try
        {
            
            btnDraft_Click(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// On this event page redirect to message inbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnGoToInbox_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Common/MessageInbox.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used for save the message as Draft.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDraft_Click(object sender, EventArgs e)
    {
        try
        {
            SetToUserIdList();
            SetCCUserIdList();
            SaveMessageAsDraft();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Helping Methods

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void GetQuerystring()
    {
        try
        {
            if (!QueryString["From"].IsNull())
                msForm = QueryString["From"];
            if (!QueryString["SMSId"].IsNull() && QueryString["SMSId"] == S_SMS_TEMPLATE_ID)
                txtSubject.Text = S_PENDING_FEE_SUBJECT;
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This is a wrapper method to collect the receiver massage records.
    /// </summary>
    private void CollectReceiverDetails()
    {
        GetReceiversInArray();
    }

    /// <summary>
    /// This method is used to validate file to upload.
    /// </summary>
    /// <returns></returns>
    private bool IsFileUploaded()
    {
        bool bIsValid = false;

        if (trfileupload.Visible == true)
        { 
            HttpFileCollection oCollection = Request.Files;
            for (int iCount = 0; iCount < oCollection.Count; iCount++)
            {
                HttpPostedFile aoAttachment = oCollection[iCount];

                string sFileName = aoAttachment.FileName;
            }

            if (File_attatchment.FileName != "")
            {
                if (File_attatchment.HasFile)
                {
                    if (File_attatchment.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                    {
                        txtToUserId.Text = GetUserNameList();
                        bIsValid = false;
                        throw new UploadFileExceptions("File size should not be greater than 5 MB.");
                    }
                    else
                    {
                        sFileName = File_attatchment.FileName;
                        sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                        //sServerFilePath = Server.MapPath("..") + "/Uploads/" + sFileName;
                        sServerFilePath = base.BasePath+"/RITeSchool/Uploads/" + sFileName;
                        File_attatchment.SaveAs(sServerFilePath);
                        bIsValid = true;
                    }
                }
                else
                {
                    throw new UploadFileExceptions("File Not Found.");
                }
            }
        }
        else if (!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
        {
            sFileName = HidAttachment.Value;
            bIsValid = true;
        }
        return bIsValid;
    }

    /// <summary>
    /// This method is used to validate file to upload in 2nd attachment.
    /// </summary>
    /// <returns></returns>
    private bool IsFileUploaded1()
    {
        bool bIsValid = false;

        if (trfileupload1.Visible == true)
        {
            if (File_attatchment1.FileName != "")
            {
                if (File_attatchment1.HasFile)
                {
                    if (File_attatchment1.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                    {
                        txtToUserId.Text = GetUserNameList();
                        bIsValid = false;
                        throw new UploadFileExceptions("File size should not be greater than 5 MB.");
                    }
                    else
                    {
                        sFileName = File_attatchment1.FileName;
                        sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                        //sServerFilePath = Server.MapPath("..") + "/Uploads/" + sFileName;
                        sServerFilePath = base.BasePath + "/RITeSchool/Uploads/" + sFileName;
                        File_attatchment1.SaveAs(sServerFilePath);
                        bIsValid = true;
                    }
                }
                else
                {
                    throw new UploadFileExceptions("File Not Found.");
                }
            }
        }
        else if (!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
        {
            sFileName = HidAttachment1.Value;
            bIsValid = true;
        }
        return bIsValid;
    }

    /// <summary>
    /// This method is used to validate file to upload in 3nd attachment.
    /// </summary>
    /// <returns></returns>
    private bool IsFileUploaded2()
    {
        bool bIsValid = false;

        if (trfileupload2.Visible == true)
        {
            if (File_attatchment2.FileName != "")
            {
                if (File_attatchment2.HasFile)
                {
                    if (File_attatchment2.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                    {
                        txtToUserId.Text = GetUserNameList();
                        bIsValid = false;
                        throw new UploadFileExceptions("File size should not be greater than 5 MB.");
                    }
                    else
                    {
                        sFileName = File_attatchment2.FileName;
                        sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                        //sServerFilePath = Server.MapPath("..") + "/Uploads/" + sFileName;\
                        sServerFilePath = base.BasePath + "/RITeSchool/Uploads/" + sFileName;
                        File_attatchment2.SaveAs(sServerFilePath);
                        bIsValid = true;
                    }
                }
                else
                {
                    throw new UploadFileExceptions("File Not Found.");
                }
            }
        }
        else if (!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
        {
            sFileName = HidAttachment2.Value;
            bIsValid = true;
        }
        return bIsValid;
    }

    /// <summary>
    /// This method creates an object for message details.
    /// </summary>
    private void InsertMessageDetails()
   {
        //This function is used to insert the Message details.
        try
        {
            moMessageDetailsBL = new MessageDetailsBL();
            string sFileAttachment = string.Empty, sFileAttachment2 = string.Empty, sFileAttachment3 = string.Empty;

            List<string> lstFiles = GetFileNames();

            int iIndex = 1;
            var arrDeletedIds = hidDeleteedIds.Value.Split(',');
            foreach (HtmlTableRow tr in pnl.Rows)
            {
                if (tr != null)
                {
                    HtmlTableCell td = tr.FindControl("td_" + iIndex) as HtmlTableCell;
                    if (td != null)
                    {
                        HyperLink hyperLink = td.FindControl("hyper_" + iIndex) as HyperLink;
                        HiddenField hidden = td.FindControl("hidden_" + iIndex) as HiddenField;
                        HiddenField hiddenLinkValue = td.FindControl("hiddenLinkValue_" + iIndex) as HiddenField;

                        if (hyperLink != null && !arrDeletedIds.Contains(iIndex.ToString()) && hiddenLinkValue.Value.Trim() != string.Empty)
                            lstFiles.Add(hiddenLinkValue.Value);
                        iIndex++;
                    }
                }
            }

            if (txtSubject.Text.Length > 200)
                moMessageDetailsBL.Subject = txtSubject.Text.Substring(0, 200);
            else
                moMessageDetailsBL.Subject = txtSubject.Text;

            moMessageDetailsBL.Message_Body = HttpUtility.HtmlEncode(hidData.Value);

            moMessageDetailsBL.Display_Text = "";
            moMessageDetailsBL.Cc_Display_Text = "";

            if ((HidUserNames.Value.Split(',').Length != hidUserId.Value.Split(';').Length) || (!hidUserId.Value.IsNullOrEmpty()) || (!hidUserGroupName.Value.IsNullOrEmpty()))
            {
                moMessageDetailsBL.Display_Text = GetUserNameList();
            }
            if (optAll.Checked == true)
            {
                if (HidUserNames.Value != "")
                    moMessageDetailsBL.Display_Text = Constants.S_ENTIRE_SCHOOL;
                else
                    moMessageDetailsBL.Display_Text = HidReplyUserNames.Value;
            }

            if (chkSuperAdmin.Checked || chkSuperAdminCC.Checked)
             {
                 HidTO.Value = moMessageDetailsBL.Display_Text;
            }

            if ((HidUserNamesCC.Value.Split(',').Length != hidUserIdCC.Value.Split(';').Length) || (!hidUserId.Value.IsNullOrEmpty()) || (!hidUserGroupName.Value.IsNullOrEmpty()))
            {
                moMessageDetailsBL.Cc_Display_Text = GetCcUserNameList();
            }
          
            if (optAll.Checked == true)
            {
                if (HidUserNames.Value != "")
                    moMessageDetailsBL.Cc_Display_Text = Constants.S_ENTIRE_SCHOOL;
                else
                    moMessageDetailsBL.Cc_Display_Text = HidReplyUserNamesCC.Value;
            }

            if (chkSuperAdminCC.Checked || chkSuperAdmin.Checked)
            {
                HidCC.Value = moMessageDetailsBL.Cc_Display_Text;
            }
            
            moMessageDetailsBL.Inserted_By_Id = miUserId;
            moMessageDetailsBL.Is_Deleted = "N";
            moMessageDetailsBL.Is_DeletedFromUser = "N";
            int iSuperAdminId = Convert.ToInt32(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID]);
            moMessageDetailsBL.Sender_User_Id = miUserId;
            moMessageDetailsBL.Sender_User_Role_Id = Convert.ToInt32(moUserRole);
            moMessageDetailsBL.Updated_By_Id = miUserId;
            moMessageDetailsBL.AcademicYrId = miAcademicYearId;
            moMessageDetailsBL.RequestReadReceipt = chkReadReceipt.Checked;
            int iMessageId = 0;

            if (chkScheduleMessages.Checked)
            {
                moMessageDetailsBL.Insert_Date = Convert.ToDateTime(txtDate.Text + ' ' + txtStartTime.Text);
            }
            else
            {
                moMessageDetailsBL.Insert_Date = DateTime.Now;
            }
            if (MessageReceiverDetailsBLList.Count > 0)
                iMessageId = moMessageDetailsBL.InsertMessageDetails(MessageReceiverDetailsBLList, lstFiles);
            MessageReceiverDetailsCollectionBL oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();

            List<MessageDetails> lstMessageDetails = oMessageReceiverDetailsCollectionBL.GetEmailAddressAndReceiverUserId(miSchoolId, iMessageId);
            if (lstMessageDetails != null && lstMessageDetails.Count > 0)
            {
                foreach (var item in lstMessageDetails)
                {

                    try
                    {
                        EncryptMessageAndSendEmail(item.EmailAddress, iMessageId, item.MessageReceiverDetailsId);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            
            if (chkSuperAdmin.Checked || chkSuperAdminCC.Checked)
            {
                int iMessageReceiverDetailsId = MessageReceiverDetailsBL.GetMessageReceiverDetailsIdForSWCoordinator(iMessageId, miSchoolId);
                string sCoordinatorEmails = ConfigurationManager.AppSettings["SoftwareCoordinatorEmailAddresses"];
                EncryptMessageAndSendEmail(sCoordinatorEmails, iMessageId, iMessageReceiverDetailsId);
            }
        }
        catch (UploadFileExceptions ex)
        {
            throw new UploadFileExceptions(ex.Message);
        }
    }

    private List<string> GetFileNames()
    {   
        List<string> lstFiles = new List<string>();
        HttpFileCollection oCollection = Request.Files;
        for (int iCount = 0; iCount < oCollection.Count; iCount++)
        {
            HttpPostedFile aoAttachment = oCollection[iCount];

            string sFileName = aoAttachment.FileName;

            if (sFileName.Trim()!= string.Empty)
            {
                sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                //sServerFilePath = Server.MapPath("..") + "/Uploads/" + sFileName;
                sServerFilePath = base.BasePath + "/RITeSchool/Uploads/" + sFileName;
                aoAttachment.SaveAs(sServerFilePath);
                lstFiles.Add(sFileName);
            }
        }

        return lstFiles;

        //return base.GenerateXml(lstFiles);

    }

    private void EncryptMessageAndSendEmail(string asEmailAddress, int iMessageId, int iMessageReceiverDetailsId)
    {

        MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL(iMessageReceiverDetailsId);

        string sUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.AbsolutePath)) + "/RITeSchool/Common/MessageViewUI.aspx?";
        string sQuerString = "MessageDetailsId=" + iMessageId
                     + "&MessageReceiverDetailsId=" + iMessageReceiverDetailsId
                     + "&pIndex=" + 0
                     + "&pSortExp=" + "Insert_Date"
                     + "&pSortDirc=" + "Desc"
                     + "&ReceiverUserId=" + oMessageReceiverDetailsBL.Receiver_User_Id;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);
        sUrl = sUrl + sEncrypt;
        string sFrom = MessageDetailsBL.GetUserName(miUserId, Convert.ToInt32(moUserRole), miAcademicYearId).Trim();
        string sTo = HidTO.Value;
        string sCc = HidCC.Value;
        string sSubject = txtSubject.Text.Trim();
        string sSignature = "RITeSchool - " + Session[Utility.Constants.S_SESSION_SCHOOL_NAME] + ", " + SchoolBase.Settings.Location;
        string sMessageBody = HttpUtility.HtmlEncode(hidData.Value);
        var sbContent = new StringBuilder();

        int iAttachmentCount = 0;
        if (File_attatchment.HasFile)
            iAttachmentCount = iAttachmentCount + 1;
        if (File_attatchment1.HasFile)
            iAttachmentCount = iAttachmentCount + 1;
        if (File_attatchment2.HasFile)
            iAttachmentCount = iAttachmentCount + 1;


        string sDetailsForSoftwareCordinator;

        if (chkSuperAdmin.Checked || chkSuperAdminCC.Checked)
            sDetailsForSoftwareCordinator = "  <tr style=\"color: #003366;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                           "     <div>  <span style=\"font-weight:bold;margin-left:15px;\">To : </span> </div>" +
                            "  </td>" +
                            "  <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" +
                               "  <div>  <span>" + "<font face=verdana>" + sTo + "</span> <div>" +
                            "  </td>" +
                        "  </tr>" + "<div></div>" +
                        "  <tr style=\"color: #003366;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                           "     <div>  <span style=\"font-weight:bold;margin-left:15px;\">Cc : </span> </div>" +
                            "  </td>" +
                            "  <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" +
                               "  <div>  <span>" + "<font face=verdana>" + sCc + "</span> <div>" +
                            "  </td>" +
                        "  </tr>" + "<div></div>" +
                        " <tr style=\"color: #003366; font-style:verdana;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                 " <div> <span style=\"font-weight:bold;margin-left:15px;\">Message Body:</span> <div>" +
                            "  </td>" +
                           "   <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" +
                                 "  <div><span>" + "<font face=verdana>" + HttpUtility.HtmlDecode(sMessageBody) + "</span> <div>" +
                             " </td>" +
                          "</tr>" +
                          "  <tr style=\"color: #003366;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                  "<span style=\"font-weight:bold;margin-left:15px;\">Attachment Count:</span>" +
                             " </td>" +
                            "  <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                 " <a href=" + sUrl + ">" + iAttachmentCount + "</a>" +
                             " </td>" +
                        "  </tr>";
        else
            sDetailsForSoftwareCordinator = string.Empty;

        sbContent.Append("<pre>");
        sUrl = " <table width=100% style=\" font-style:Cambria\"><tr width=100%><td><hr/></td></tr>" +
             " <tr >" +
              "    <td style=\"font-size:14\">" + "<font face=verdana>" +
                   "   Dear User," +
                 " </td>" +
            "  </tr>" +
           "   <tr>" +
                 " <td>" +
                 " </td>" +
            "  </tr>" +
             " <tr>" +
               "   <td colspan=\"4\" style=\"font-size:14\">" + "<font face=verdana>" +
                    "  You have received a message in RITeSchool Message Center." + " </td>" + "  </tr>" + "<tr><td></td></tr>" + "<tr><td></td></tr>" +
                       " <tr>" +
               "   <td colspan=\"4\" style=\"font-size:14\">" + "<font face=verdana>" +
                    " Following are the details." +
                 " </td>" +
             " </tr>" +
            "  <tr style=\"height: 20px;\">" +
                 " <td>" +
                  "</td>" +
              "</tr>" +
              "<tr>" +
                  "<td>" +
                     " <table width=\"150%\" style=\"background-color:white;border: 1px;\">" +
                        "  <tr style=\"color: #003366;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                           "     <div>  <span style=\"font-weight:bold;margin-left:15px;\">From : </span> </div>" +
                            "  </td>" +
                            "  <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" +
                               "  <div>  <span>" + "<font face=verdana>" + sFrom + "</span> <div>" +
                            "  </td>" +
                        "  </tr>" + "<div></div>" +
                         " <tr style=\"color: #003366; font-style:verdana;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                 " <div> <span style=\"font-weight:bold;margin-left:15px;\">Subject:</span> <div>" +
                            "  </td>" +
                           "   <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" +
                                 "  <div><span>" + "<font face=verdana>" + sSubject + "</span> <div>" +
                             " </td>" +
                          "</tr>" + sDetailsForSoftwareCordinator +
                        "  <tr style=\"color: #003366;border: 1px;border-bottom-color: white;margin-bottom: 2px;padding-bottom: 2px;\">" +
                             " <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                  "<span style=\"font-weight:bold;margin-left:15px;\">Message:</span>" +
                             " </td>" +
                            "  <td style=\"background:#E6EDF4;padding:0in 0in 0in 0in;height:.75pt;font-Size:14\">" + "<font face=verdana>" +
                                 " <a href=" + sUrl + ">Click Here</a>" +
                             " </td>" +
                        "  </tr>" +
                    "  </table>" +
                 " </td>" +
             " </tr>" +
            "  <tr>" +
                 " <td>" +
                 " </td>" +
              "</tr>" + "<tr><td></td></tr>" +
             " <tr>" +
                 " <td style=\" Font-Size:14\">" + "<font face=verdana>" +
                    "  Regards," +
                 "</td>" +
            "  </tr>" +
             " <tr>" +
                "  <td style=\"Font-Size:14\">" + "<font face=verdana>" +
                    sSignature +
                "  </td>" +
             " </tr>" + "<tr><td></td></tr>" +
            "  <tr>" +
                 " <td>" +
                  "</td>" +
            "  </tr>" +
            "  <tr>" +
              "   <td colspan=\"4\"  style=\"Font-Size:12\">" + "<font face=verdana>" +
                     " <b>Note </b>:If this message is unexpected and don't want to receive in future;" +
                     " login to your RITeSchool account, go to <b>Dashboard >> Message Center >> E-mail Settings</b>" +
                  "    and deselect \"Yes, I want to receive messages on below email address\" option and Save." +
                "  </td>" +
            "  </tr>" +
             " <tr><td></td></tr>" + "<tr><td></td></tr>" + "<font face=verdana>" +
               " <tr><td colspan=\"4\" style=\"Font-Size:12;color:blue;\">This is an auto-generated email and need not to be replied back.This mailbox is not getting monitored.</td></tr><tr><td></td></tr><tr width=100%><td><hr/></td></tr>" +
         " </table>" +
               sbContent.Append("<pre>");

        if (chkSuperAdmin.Checked || chkSuperAdminCC.Checked)
        {
            AddLog("S/w coordinator mail to => " + asEmailAddress);
        }

        if (!asEmailAddress.IsNullOrEmpty())
            CommonUtility.SendE_Mail(asEmailAddress, Constants.S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN, "Message received from " + Settings.SiteName, sUrl);
    }


    private void AddLog(string asMessage, bool abIsStartingMessage = false)
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
        if (ConfigurationManager.AppSettings["LogFilePath"] != null && ConfigurationManager.AppSettings["LogFilePath"].ToString() != string.Empty)
        {
            string sPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            var sbContent = new StringBuilder();

            if (abIsStartingMessage)
                sbContent.AppendFormat("{0}{0}", Environment.NewLine, Environment.NewLine);

            sbContent.AppendFormat("School Id    : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            sbContent.AppendFormat("School Id   : {0}{1}", iSchoolId, Environment.NewLine);
            sbContent.AppendFormat("Message : {0}{1}", asMessage, Environment.NewLine);

            var swFile = new StreamWriter(sPath + "OnlineTransactionQueryString.log", true);
            swFile.WriteLine("\n" + sbContent);
            swFile.Flush();
            swFile.Close();
        }
    }


    private string StripHTML(string source)
    {
        string result;

        // Remove HTML Development formatting
        // Replace line breaks with space
        // because browsers inserts space
        result = source.Replace("\r", " ");
        // Replace line breaks with space
        // because browsers inserts space
        result = result.Replace("\n", " ");
        // Remove step-formatting
        result = result.Replace("\t", string.Empty);
        // Remove repeating spaces because browsers ignore them
        result = System.Text.RegularExpressions.Regex.Replace(result,
                                                              @"( )+", " ");

        // Remove the header (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*head([^>])*>", "<head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*head( )*>)", "</head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<head>).*(</head>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all scripts (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*script([^>])*>", "<script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*script( )*>)", "</script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<script>).*(</script>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all styles (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*style([^>])*>", "<style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*style( )*>)", "</style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<style>).*(</style>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert tabs in spaces of <td> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*td([^>])*>", "\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line breaks in places of <BR> and <LI> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*br( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*li( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line paragraphs (double line breaks) in place
        // if <P>, <DIV> and <TR> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*div([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*tr([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*p([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove remaining tags like <a>, links, images,
        // comments etc - anything that's enclosed inside < >
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[^>]*>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // replace special characters:
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @" ", " ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&bull;", " * ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lsaquo;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&rsaquo;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&trade;", "(tm)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&frasl;", "/",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lt;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&gt;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&copy;", "(c)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&reg;", "(r)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // Remove all others. More can be added, see
        // http://hotwired.lycos.com/webmonkey/reference/special_characters/
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&(.{2,6});", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // make line breaking consistent
        result = result.Replace("\n", "\r");

        // Remove extra line breaks and tabs:
        // replace over 2 breaks with 2 and over 4 tabs with 4.
        // Prepare first to remove any whitespaces in between
        // the escaped characters and remove redundant tabs in between line breaks
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\t)", "\t\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\r)", "\t\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\t)", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // Remove redundant tabs
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // Remove multiple tabs following a line break with just one tab
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // Initial replacement target string for line breaks
        string breaks = "\r\r\r";
        // Initial replacement target string for tabs
        string tabs = "\t\t\t\t\t";
        for (int index = 0; index < result.Length; index++)
        {
            result = result.Replace(breaks, "\r\r");
            result = result.Replace(tabs, "\t\t\t\t");
            breaks = breaks + "\r";
            tabs = tabs + "\t";
        }

        return result;
    }

    /// <summary>
    /// Get comma separated list of user names selected by user.
    /// </summary>
    /// <returns></returns>
    private string GetUserNameList()
    {
        string sReturn = String.Empty;
        if (!String.IsNullOrEmpty(HidTeacherName.Value))
            sReturn = HidTeacherName.Value;

        if (!String.IsNullOrEmpty(HidStdDivName.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidStdDivName.Value;
            else
                sReturn = HidStdDivName.Value;
        }
        if (!String.IsNullOrEmpty(sReturn))
        {
            if (hidDraftId.Value == Constants.S_ZERO)
            {
                if (HidStudentName.Value != string.Empty)
                    sReturn = sReturn + ", " + HidStudentName.Value;
            }
        }
        else
            sReturn = HidStudentName.Value;

        if (!String.IsNullOrEmpty(HidSupervisorName.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidSupervisorName.Value;
            else
                sReturn = HidSupervisorName.Value;
        }

        if (!String.IsNullOrEmpty(HidPTAName.Value))
        {
            StringBuilder sb = new StringBuilder();

            bool abNotFound = true;
            HidPTAName.Value.Split(',').ToList().ForEach(id =>
            {
                abNotFound = true;
                string[] sUserIds = sReturn.Split(',');
                for (int k = 0; k < sUserIds.Length; k++)
                {
                    if (sUserIds[k].Trim() == id.Trim())
                    {
                        abNotFound = false;
                        break;
                    }
                }

                if (abNotFound && id.Trim() != string.Empty)
                    sb.Append("," + id);

            });

            string sNewIds = string.Empty;
            if (sb.Length > 0)
                sNewIds = sb.ToString().Substring(1);

            if (sNewIds.Trim() != string.Empty)
            {
                if (!String.IsNullOrEmpty(sReturn))
                    sReturn = sReturn + ", " + sNewIds;
                else
                    sReturn = sNewIds;
            }
        }


        if (chkSuperAdmin.Checked)
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidSuperAdminName.Value;
            else
                sReturn = HidSuperAdminName.Value;
        }
        if (chkAdmin.Checked)
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidAdminUserName.Value;
            else
                sReturn = HidAdminUserName.Value;
        }
        if (chkPrincipal.Checked && !sReturn.Contains(HidPrincipleName.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidPrincipleName.Value;
            else
                sReturn = HidPrincipleName.Value;
        }
        if (!String.IsNullOrEmpty(hidUserGroupName.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + hidUserGroupName.Value;
            else
                sReturn = hidUserGroupName.Value;
        }

        return sReturn;
    }

    /// <summary>
    /// Get comma separated list of user names selected by user.
    /// </summary>
    /// <returns></returns>
    private string GetCcUserNameList()
    {
        string sReturn = String.Empty;
        if (!String.IsNullOrEmpty(HidTeacherNameCC.Value))
            sReturn = HidTeacherNameCC.Value;

        if (!String.IsNullOrEmpty(HidStdDivNameCC.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidStdDivNameCC.Value;
            else
                sReturn = HidStdDivNameCC.Value;
        }
        if (!String.IsNullOrEmpty(sReturn))
        {
            if (hidDraftId.Value == Constants.S_ZERO)
            {
                if (HidStudentNameCC.Value != string.Empty)
                    sReturn = sReturn + ", " + HidStudentNameCC.Value;
            }
        }
        else
            sReturn = HidStudentNameCC.Value;

        if (!String.IsNullOrEmpty(HidSupervisorNameCC.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidSupervisorNameCC.Value;
            else
                sReturn = HidSupervisorNameCC.Value;
        }

        if (!String.IsNullOrEmpty(HidPTANameCC.Value))
        {


            StringBuilder sb = new StringBuilder();

            bool abNotFound = true;
            HidPTANameCC.Value.Split(',').ToList().ForEach(id =>
            {
                abNotFound = true;
                string[] sUserIds = sReturn.Split(',');
                for (int k = 0; k < sUserIds.Length; k++)
                {
                    if (sUserIds[k].Trim() == id.Trim())
                    {
                        abNotFound = false;
                        break;
                    }
                }

                if (abNotFound && id.Trim() != string.Empty)
                    sb.Append("," + id);

            });

            string sNewIds = string.Empty;
            if (sb.Length > 0)
                sNewIds = sb.ToString().Substring(1);

            if (sNewIds.Trim() != string.Empty)
            {
                if (sNewIds.Trim() != string.Empty)
                    sReturn = sReturn + ", " + sNewIds.Trim();
                else
                    sReturn = sNewIds.Trim();
            }
        }
        
        if (chkSuperAdminCC.Checked)
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidSuperAdminNameCC.Value;
            else
                sReturn = HidSuperAdminNameCC.Value;
        }
        if (chkAdminCC.Checked)
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidAdminUserNameCC.Value;
            else
                sReturn = HidAdminUserNameCC.Value;
        }
        if (chkPrincipleCC.Checked && !sReturn.Contains(HidPrincipleNameCC.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + HidPrincipleNameCC.Value;
            else
                sReturn = HidPrincipleNameCC.Value;
        }
        if (!String.IsNullOrEmpty(hidUserGroupNameCC.Value))
        {
            if (!String.IsNullOrEmpty(sReturn))
                sReturn = sReturn + ", " + hidUserGroupNameCC.Value;
            else
                sReturn = hidUserGroupNameCC.Value;
        }
        return sReturn;
    }

    /// <summary>
    /// This method collects the user ids of receiptants into a comma separated list.
    /// This list is then stored in hidUserId.
    /// if massage is being sent to the entire school - The ids of all the users in school are collected 
    /// else , ids of selected users are collected .
    /// </summary>
    private void SetToUserIdList()
    {
        //This function is used to get the list of the Users.
        string SToUserList = string.Empty;
        DataTable odsUserID;
        int iCount;
        //Entire school
        if (txtToUserId.Text.Contains(Constants.S_ENTIRE_SCHOOL))
            optAll.Checked = true;
        if (((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && optAll.Visible == true && optAll.Checked == true) || (txtToUserId.Text.Contains(Constants.S_ENTIRE_SCHOOL)))
        {
            odsUserID = SchoolUserCollectionBL.GetAllUsers(miSchoolId, miAcademicYearId);
            for (int jCount = 0; jCount < odsUserID.Rows.Count; jCount++)
                SToUserList += Convert.ToString(odsUserID.Rows[jCount]["Id"]) + ";";
            hidUserId.Value = SToUserList;
        }
        else
        {
            hidUserId.Value = HidTeacherId.Value;
            if (HidStdDivId.Value != "")
            {
                string[] sArrStdDivIds = HidStdDivId.Value.Split(';');
                for (iCount = 0; iCount < sArrStdDivIds.Length; iCount++)
                {
                    odsUserID = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, Convert.ToInt32(sArrStdDivIds[iCount]), miAcademicYearId, string.Empty, Constants.I_ZERO, false);
                    for (int jCount = 0; jCount < odsUserID.Rows.Count; jCount++)
                        SToUserList += Convert.ToString(odsUserID.Rows[jCount]["Id"]) + ";";
                }
                if (SToUserList.EndsWith(";"))
                    SToUserList = SToUserList.Substring(0, SToUserList.LastIndexOf(';'));

                if (String.IsNullOrEmpty(hidUserId.Value))
                    hidUserId.Value = SToUserList;
                else
                    hidUserId.Value = hidUserId.Value + ";" + SToUserList;
            }
            //students
            if (!String.IsNullOrEmpty(HidStudentId.Value))
            {
                if (String.IsNullOrEmpty(hidUserId.Value))
                    hidUserId.Value = HidStudentId.Value;
                else
                    hidUserId.Value = hidUserId.Value + ";" + HidStudentId.Value;
            }
            //Supervisor
            if (!String.IsNullOrEmpty(HidSupervisorId.Value))
            {
                if (String.IsNullOrEmpty(hidUserId.Value))
                    hidUserId.Value = HidSupervisorId.Value;
                else
                    hidUserId.Value = hidUserId.Value + ";" + HidSupervisorId.Value;
            }

            if (!String.IsNullOrEmpty(HidPTAId.Value))
            {
                //string[] sIds = hidUserId.Value.Split(';');

                StringBuilder sb = new StringBuilder();

                bool abNotFound = true;
                HidPTAId.Value.Split(';').ToList().ForEach(id =>
                {
                    abNotFound = true;
                    string[] sUserIds = hidUserId.Value.Split(';');
                    for (int k = 0; k < sUserIds.Length; k++)
                    {
                        if (sUserIds[k].Trim() == id.Trim())
                        {
                            abNotFound = false;
                            break;
                        }
                    }

                    if (abNotFound && id.Trim() != string.Empty)
                        sb.Append(";" + id);

                });

                string sNewIds = string.Empty;
                if (sb.Length > 0)
                    sNewIds = sb.ToString().Substring(1);

                if (sNewIds != string.Empty)
                {
                    if (String.IsNullOrEmpty(hidUserId.Value))
                        hidUserId.Value = sNewIds;
                    else
                        hidUserId.Value = hidUserId.Value + ";" + sNewIds;
                }
            }

            //if (String.IsNullOrEmpty(hidUserId.Value))
            //    hidUserId.Value = HidReplyUserID.Value;
            //else
            //{
            //    // Check if same user id is not being included in the reply list.
            //    if (hidUserId.Value == HidReplyUserID.Value)
            //        hidUserId.Value = hidUserId.Value + ";" + HidReplyUserID.Value;
            //}
            if (chkAdmin.Checked)
            {
                HidId.Value = ";" + HidAdminUserID.Value + ";";
                if (!hidUserId.Value.Contains(HidId.Value))
                    hidUserId.Value = hidUserId.Value + ";" + HidAdminUserID.Value;
                if (hidUserId.Value.Trim().StartsWith(";"))
                    hidUserId.Value = hidUserId.Value.Substring(hidUserId.Value.IndexOf(';') + 1, hidUserId.Value.Length - 1);
            }
            if (chkPrincipal.Checked)
            {
                HidId.Value = ";" + HidPrincipalUserID.Value + ";";
                if (!hidUserId.Value.Contains(HidId.Value))
                    hidUserId.Value = hidUserId.Value + ";" + HidPrincipalUserID.Value;
                if (hidUserId.Value.Trim().StartsWith(";"))
                    hidUserId.Value = hidUserId.Value.Substring(hidUserId.Value.IndexOf(';') + 1, hidUserId.Value.Length - 1);
            }
            if (chkSuperAdmin.Checked)
            {
                HidId.Value = ";" + HidSuperAdminUserId.Value + ";";
                if (!hidUserId.Value.Contains(HidId.Value))
                    hidUserId.Value = hidUserId.Value + ";" + HidSuperAdminUserId.Value;
                if (hidUserId.Value.Trim().StartsWith(";"))
                    hidUserId.Value = hidUserId.Value.Substring(hidUserId.Value.IndexOf(';') + 1, hidUserId.Value.Length - 1);
            }

            if (!hidUserGroupId.Value.IsNullOrEmpty() && hidUserGroupId.Value != Constants.S_ZERO)
            {
                MailingGroupBL oMailingGroupBL = new MailingGroupBL(miSchoolId, miAcademicYearId, miUserId);
                string sUserIds = oMailingGroupBL.GetMailingGroupUsers(hidUserGroupId.Value, false);
                if (!sUserIds.IsNullOrEmpty())
                    sUserIds = sUserIds.Replace(",", ";");
                if (!hidUserId.Value.IsNullOrEmpty())
                    hidUserId.Value = hidUserId.Value + ";" + sUserIds;
                else
                    hidUserId.Value = sUserIds;
            }
        }
        if (hidUserId.Value.EndsWith(";"))
            hidUserId.Value = hidUserId.Value.Substring(0, hidUserId.Value.LastIndexOf(';'));
    }

    /// <summary>
    /// This method collects the user ids of receiptants into a comma separated list.
    /// This list is then stored in hidUserId.
    /// if massage is being sent to the entire school - The ids of all the users in school are collected 
    /// else , ids of selected users are collected .
    /// </summary>
    private void SetCCUserIdList()
    {
        //This function is used to get the list of the Users.
        string SToUserList = string.Empty;
        DataTable odsUserID;
        int iCount;
        //Entire school
        if (txtCCUserId.Text.Contains(Constants.S_ENTIRE_SCHOOL))
            optCCAll.Checked = true;
        if (((moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor) && optCCAll.Checked == true) || (txtCCUserId.Text.Contains(Constants.S_ENTIRE_SCHOOL)))
        {
            odsUserID = SchoolUserCollectionBL.GetAllUsers(miSchoolId, miAcademicYearId);
            for (int jCount = 0; jCount < odsUserID.Rows.Count; jCount++)
                SToUserList += Convert.ToString(odsUserID.Rows[jCount]["Id"]) + ";";
            hidUserIdCC.Value = SToUserList;
        }
        else
        {
            hidUserIdCC.Value = HidTeacherIdCC.Value;
            if (HidStdDivIdCC.Value != "")
            {
                string[] sArrStdDivIds = HidStdDivIdCC.Value.Split(';');
                for (iCount = 0; iCount < sArrStdDivIds.Length; iCount++)
                {
                    odsUserID = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, Convert.ToInt32(sArrStdDivIds[iCount]), miAcademicYearId, string.Empty, Constants.I_ZERO, false);
                    for (int jCount = 0; jCount < odsUserID.Rows.Count; jCount++)
                        SToUserList += Convert.ToString(odsUserID.Rows[jCount]["Id"]) + ";";
                }
                if (SToUserList.EndsWith(";"))
                    SToUserList = SToUserList.Substring(0, SToUserList.LastIndexOf(';'));

                if (String.IsNullOrEmpty(hidUserIdCC.Value))
                    hidUserIdCC.Value = SToUserList;
                else
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + SToUserList;
            }
            //students
            if (!String.IsNullOrEmpty(HidStudentIdCC.Value))
            {
                if (String.IsNullOrEmpty(hidUserIdCC.Value))
                    hidUserIdCC.Value = HidStudentIdCC.Value;
                else
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + HidStudentIdCC.Value;
            }
            //Supervisor
            if (!String.IsNullOrEmpty(HidSupervisorIdCC.Value))
            {
                if (String.IsNullOrEmpty(hidUserIdCC.Value))
                    hidUserIdCC.Value = HidSupervisorIdCC.Value;
                else
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + HidSupervisorIdCC.Value;
            }

            if (!String.IsNullOrEmpty(HidPTAIdCC.Value))
            {
                StringBuilder sb = new StringBuilder();

                bool abNotFound = true;
                HidPTAIdCC.Value.Split(';').ToList().ForEach(id =>
                {
                    abNotFound = true;
                    string[] sUserIds = hidUserIdCC.Value.Split(';');
                    for (int k = 0; k < sUserIds.Length; k++)
                    {
                        if (sUserIds[k].Trim() == id.Trim())
                        {
                            abNotFound = false;
                            break;
                        }
                    }

                    if (abNotFound && id.Trim() != string.Empty)
                        sb.Append("," + id);

                });

                string sNewIds = string.Empty;
                if (sb.Length > 0)
                    sNewIds = sb.ToString().Substring(1);

                if (sNewIds.Trim() != string.Empty)
                {
                    if (String.IsNullOrEmpty(hidUserIdCC.Value))
                        hidUserIdCC.Value = sNewIds;
                    else
                        hidUserIdCC.Value = hidUserIdCC.Value + ";" + sNewIds;
                }
            }

            if (chkAdminCC.Checked)
            {
                HidId.Value = ";" + HidAdminUserID.Value + ";";
                if (!hidUserIdCC.Value.Contains(HidId.Value))
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + HidAdminUserID.Value;
                if (hidUserIdCC.Value.Trim().StartsWith(";"))
                    hidUserIdCC.Value = hidUserIdCC.Value.Substring(hidUserIdCC.Value.IndexOf(';') + 1, hidUserIdCC.Value.Length - 1);
            }
            if (chkPrincipleCC.Checked)
            {
                HidId.Value = ";" + HidPrincipalUserIDCC.Value + ";";
                if (!hidUserIdCC.Value.Contains(HidId.Value))
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + HidPrincipalUserIDCC.Value;
                if (hidUserIdCC.Value.Trim().StartsWith(";"))
                    hidUserIdCC.Value = hidUserIdCC.Value.Substring(hidUserIdCC.Value.IndexOf(';') + 1, hidUserIdCC.Value.Length - 1);
            }
            if (chkSuperAdminCC.Checked)
            {
                HidId.Value = ";" + HidSuperAdminUserIdCC.Value + ";";
                if (!hidUserIdCC.Value.Contains(HidId.Value))
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + HidSuperAdminUserIdCC.Value;
                if (hidUserIdCC.Value.Trim().StartsWith(";"))
                    hidUserIdCC.Value = hidUserIdCC.Value.Substring(hidUserIdCC.Value.IndexOf(';') + 1, hidUserIdCC.Value.Length - 1);
            }

            if (!hidUserGroupIdCC.Value.IsNullOrEmpty() && hidUserGroupIdCC.Value != Constants.S_ZERO)
            {
                MailingGroupBL oMailingGroupBL = new MailingGroupBL(miSchoolId, miAcademicYearId, miUserId);
                string sUserIds = oMailingGroupBL.GetMailingGroupUsers(hidUserGroupIdCC.Value, false);
                if (!sUserIds.IsNullOrEmpty())
                    sUserIds = sUserIds.Replace(",", ";");
                if (!hidUserIdCC.Value.IsNullOrEmpty())
                    hidUserIdCC.Value = hidUserIdCC.Value + ";" + sUserIds;
                else
                    hidUserIdCC.Value = sUserIds;
            }
        }
        if (hidUserIdCC.Value.EndsWith(";"))
            hidUserIdCC.Value = hidUserIdCC.Value.Substring(0, hidUserIdCC.Value.LastIndexOf(';'));
    }

    /// <summary>
    /// This  method to collect the receiver details in array.
    /// It gets the records as comma separated list of ids into hidden variabel(hidUserId).
    /// Splits the ids and makes 1 object for each id. And all these objects are collected as an array.
    /// </summary>
    private void GetReceiversInArray()
    {
        SetToUserIdList();
        SetCCUserIdList();

        hidUserId.Value = hidUserId.Value.Replace("; ", ";").Replace(";;", ";");
        hidUserIdCC.Value = hidUserIdCC.Value.Replace("; ", ";").Replace(";;", ";");

        string[] sArrUserIds = hidUserId.Value.Split(';');
        Hashtable moHTUsersMobileNo = new Hashtable();

        string[] sArrCcUserIds = hidUserIdCC.Value.Split(';');
        Hashtable moHTUsersMobileNoCc = new Hashtable();

        if (hidUserId.Value.Trim() != string.Empty)
        {
            int iUserId = 0;
            for (int iCount = 0; iCount < sArrUserIds.Length; iCount++)
                if (Int32.TryParse(sArrUserIds[iCount], out iUserId))
                    moHTUsersMobileNo[Convert.ToInt32(sArrUserIds[iCount])] = iUserId;
            MessageReceiverDetailsBL aoMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
            if (!Settings.ReceiveAllMsgToDefaultUser)
            {
                if (moUserRole == Constants.UserRoles.Teacher)
                {                    
                    string sUserId = hidUserId.Value.Replace(";", ",");
                    bool bIsStudentPresent = aoMessageReceiverDetailsBL.IsStudentPresentInReceipantList(miSchoolId, sUserId);
                    if (bIsStudentPresent == true)
                    {
                        AddDefaultMessageReceiver(moHTUsersMobileNo, aoMessageReceiverDetailsBL);
                    }
                }
                else if (moUserRole == Constants.UserRoles.Student)
                {
                    AddDefaultMessageReceiver(moHTUsersMobileNo, aoMessageReceiverDetailsBL);
                }
            }
            else            
                AddDefaultMessageReceiver(moHTUsersMobileNo, aoMessageReceiverDetailsBL);
            

            foreach (DictionaryEntry oDE in moHTUsersMobileNo)
            {
                int iMessageID = Convert.ToInt32(QueryString["MessageID"]);
                MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
                oMessageReceiverDetailsBL.New_Message_Flag = "Y";
                oMessageReceiverDetailsBL.Message_Details_Id = iMessageID;
                oMessageReceiverDetailsBL.Read_Message_Flag = "N";
                oMessageReceiverDetailsBL.Receiver_User_Id = Convert.ToInt32(oDE.Key);
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(Convert.ToInt32(oDE.Key));
                oMessageReceiverDetailsBL.Receiver_User_Role_Id = oSchoolUserBL.UserRoleId;
                oMessageReceiverDetailsBL.Updated_By_Id = miUserId;
                oMessageReceiverDetailsBL.Inserted_By_Id = miUserId;
                oMessageReceiverDetailsBL.Is_Archive = "N";
                oMessageReceiverDetailsBL.Is_Deleted = "N";
                oMessageReceiverDetailsBL.Is_DeletedFromReceiver = "N";
                oMessageReceiverDetailsBL.IsCc = 0;

                if (chkScheduleMessages.Checked)
                {
                    oMessageReceiverDetailsBL.Insert_Date = Convert.ToDateTime(txtDate.Text + ' ' + txtStartTime.Text);
                }
                else
                    oMessageReceiverDetailsBL.Insert_Date = DateTime.Now;

                if(!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
                    oMessageReceiverDetailsBL.IsForwardReply = "Y";
                else if ((!QueryString["Reply"].IsNull() && QueryString["Reply"] == "true") || (!QueryString["ReplyToAll"].IsNull() && QueryString["ReplyToAll"] == "true"))
                    oMessageReceiverDetailsBL.IsForwardReply = "N";

                MessageReceiverDetailsBLList.Add(oMessageReceiverDetailsBL);

                if ((!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true") || (!QueryString["Reply"].IsNull() && QueryString["Reply"] == "true") || (!QueryString["ReplyToAll"].IsNull() && QueryString["ReplyToAll"] == "true"))
                oMessageReceiverDetailsBL.UpdateMessageReceiverDetails(true, miUserId);
            }
        }

        if (moUserRole == Constants.UserRoles.Teacher)
        {
            MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
            string sUserId = hidUserId.Value.Replace(";", ",");
            bool bIsStudentPresent = oMessageReceiverDetailsBL.IsStudentPresentInReceipantList(miSchoolId, sUserId);
            if (bIsStudentPresent == true)
                AddDefaultMessageReceiverCC(moHTUsersMobileNoCc, moHTUsersMobileNo);
        }
        else if (moUserRole == Constants.UserRoles.Student)
        {
            AddDefaultMessageReceiverCC(moHTUsersMobileNoCc, moHTUsersMobileNo);
        }

        if (hidUserIdCC.Value.Trim() != string.Empty || moHTUsersMobileNoCc.Count > 0)
        {
            int iCcUserId = 0;
            if (sArrCcUserIds.Length > 0 && sArrCcUserIds[0].Trim() != string.Empty)
            {
                for (int iCount = 0; iCount < sArrCcUserIds.Length; iCount++)
                    if (Int32.TryParse(sArrCcUserIds[iCount], out iCcUserId))
                        moHTUsersMobileNoCc[Convert.ToInt32(sArrCcUserIds[iCount])] = iCcUserId;
            }

            foreach (DictionaryEntry oDE1 in moHTUsersMobileNoCc)
            {
                MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
                oMessageReceiverDetailsBL.New_Message_Flag = "Y";
                oMessageReceiverDetailsBL.Read_Message_Flag = "N";
                oMessageReceiverDetailsBL.Receiver_User_Id = Convert.ToInt32(oDE1.Key);
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(Convert.ToInt32(oDE1.Key));
                oMessageReceiverDetailsBL.Receiver_User_Role_Id = oSchoolUserBL.UserRoleId;
                oMessageReceiverDetailsBL.Updated_By_Id = miUserId;
                oMessageReceiverDetailsBL.Inserted_By_Id = miUserId;
                oMessageReceiverDetailsBL.Is_Archive = "N";
                oMessageReceiverDetailsBL.Is_Deleted = "N";
                oMessageReceiverDetailsBL.Is_DeletedFromReceiver = "N";
                oMessageReceiverDetailsBL.IsCc = 1;

                if (chkScheduleMessages.Checked)
                    oMessageReceiverDetailsBL.Insert_Date = Convert.ToDateTime(txtDate.Text + ' ' + txtStartTime.Text);
                else
                    oMessageReceiverDetailsBL.Insert_Date = DateTime.Now;

                if (!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
                    oMessageReceiverDetailsBL.IsForwardReply = "Y";
                else if ((!QueryString["Reply"].IsNull() && QueryString["Reply"] == "true") || (!QueryString["ReplyToAll"].IsNull() && QueryString["ReplyToAll"] == "true"))
                    oMessageReceiverDetailsBL.IsForwardReply = "N";

                MessageReceiverDetailsBLList.Add(oMessageReceiverDetailsBL);

                if ((!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true") || (!QueryString["Reply"].IsNull() && QueryString["Reply"] == "true") || (!QueryString["ReplyToAll"].IsNull() && QueryString["ReplyToAll"] == "true"))
                oMessageReceiverDetailsBL.UpdateMessageReceiverDetails(true, miUserId);
            }
        }
    }

    /// <summary>
    ///  This method is used to Add Default Message receivers.
    /// </summary>
    private void AddDefaultMessageReceiver(Hashtable moHTUsersMobileNo, MessageReceiverDetailsBL oMessageReceiverDetailsBL)
    {
        if (miSchoolId != Constants.SchoolId.SNS.ToInt())
        {
            int iUserIdOfDefaultUser;
            List<int> lstReceiveUserDetails = oMessageReceiverDetailsBL.GetDefaultUserId(miSchoolId);
            for (int iUserCount = 0; iUserCount < lstReceiveUserDetails.Count; iUserCount++)
            {
                iUserIdOfDefaultUser = lstReceiveUserDetails[iUserCount];
                if (miUserId != iUserIdOfDefaultUser)
                    moHTUsersMobileNo[iUserIdOfDefaultUser] = iUserIdOfDefaultUser;
            }
        }
    }

    private void AddDefaultMessageReceiverCC(Hashtable moHTUsersMobileNoCc, Hashtable moHTUsersMobileNo)
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && txtSubject.Text.Trim().Contains("PTA"))
        {
            int iUserIdOfDefaultUser;
            MessageReceiverDetailsBL oMessageReceiverDetailsBL = new MessageReceiverDetailsBL();
            List<int> lstReceiveUserDetails = oMessageReceiverDetailsBL.GetDefaultUserId(miSchoolId);
            for (int iUserCount = 0; iUserCount < lstReceiveUserDetails.Count; iUserCount++)
            {
                iUserIdOfDefaultUser = lstReceiveUserDetails[iUserCount];
                if (miUserId != iUserIdOfDefaultUser && !moHTUsersMobileNo.Contains(iUserIdOfDefaultUser))
                    moHTUsersMobileNoCc[iUserIdOfDefaultUser] = iUserIdOfDefaultUser;
            }
        }
    }

    /// <summary>
    ///  This method sets the display according to user role and gets admin user details.
    /// </summary>
    private void SetDefaultPageControls()
    {
        FetchAdminUserId();
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
        DataTable oDt = oMessageDetailsBL.GetAcademicYearDetails(miAcademicYearId);
        if (oDt.Rows[0]["Is_Current_Year"].ToString() == "N" && oDt.Rows[0]["Is_Close_Year"].ToString() == "N"
            && oDt.Rows[0]["Is_NewlyCreated"].ToString() == "Y" && oDt.Rows[0]["Is_FinalYear_Generated"].ToString() == "N")
        {
            chkPrincipal.Visible = false;
            chkSuperAdmin.Visible = false;
            chkPrincipleCC.Visible = true;
            chkSuperAdminCC.Visible = false;
        }

        if (moUserRole == Constants.UserRoles.Student && !Boolean.Parse(hidUserHasFullAccess.Value))
        {
            chkAdmin.Visible = false;
            chkAdminCC.Visible = false;
            HideSwCordinatorOption();

            if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
            {
                chkPrincipal.Visible = false;
                chkPrincipleCC.Visible = false;
            }

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                optParentTeacherAssociation.Visible = true;
        }

        if (moUserRole == Constants.UserRoles.Teacher)
        {
            if (hidViewAllStudents.Value == Constants.S_ZERO)
            {
                optCCStudents.Visible = false;
                optStudents.Visible = false;
                optTeachers.Checked = true;
                optCCTeachers.Checked = true;

                //if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                //{
                //    optParentTeacherAssociation.Visible = true;
                //    optCCParentTeacherAssociation.Visible = true;
                //}
            }
        }

        HidUserType.Value = moUserRole.ToString();
        txtSubject.Focus();
        chkAdmin.Attributes.Add("onclick", "SetControlsForAdminDetails(this, 'Admin');");
        chkPrincipal.Attributes.Add("onclick", "SetControlsForAdminDetails(this, 'Principal');");
        chkSuperAdmin.Attributes.Add("onclick", "SetControlsForAdminDetails(this, 'Software Coordinator');");
        optStudents.Attributes.Add("onclick", "SetControlsForAdminDetails(this,'Student');");
        optTeachers.Attributes.Add("onclick", "SetControlsForAdminDetails(this,'Teacher');");
        optSupervisor.Attributes.Add("onclick", "SetControlsForAdminDetails(this,'Supervisor');");
        optAll.Attributes.Add("onclick", "SetControlsForAdminDetails(this,'EntireSchool');");
        optParentTeacherAssociation.Attributes.Add("onclick", "SetControlsForAdminDetails(this,'ParentTeacherAssociation')");/////////////new

        if (Constants.S_SUPERVISOR_ROLE_NAME == string.Empty)
            Constants.S_SUPERVISOR_ROLE_NAME = Settings.SupervisorRoleName;

        optSupervisor.Text = Constants.S_SUPERVISOR_ROLE_NAME;
        
        lblFrom.Text = MessageDetailsBL.GetUserName(miUserId, Convert.ToInt32(moUserRole), miAcademicYearId);
        tdAttachment.Visible = false;
        tdAttachment1.Visible = false;
        tdAttachment2.Visible = false;
        
        chkAdminCC.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this, 'Admin');");
        chkPrincipleCC.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this, 'Principal');");
        chkSuperAdminCC.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this, 'Software Coordinator');");
        optCCStudents.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this,'Student');");
        optCCTeachers.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this,'Teacher');");
        optCCSupervisor.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this,'Supervisor');");
        optCCAll.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this,'EntireSchool');");
        optCCParentTeacherAssociation.Attributes.Add("onclick", "SetControlsForAdminCCDetails(this,'ParentTeacherAssociation')");////////new

        optCCSupervisor.Text = Constants.S_SUPERVISOR_ROLE_NAME;
       
        if (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Student)
        {
            //check if we redirect here from Pending Fee Student List
            if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("Fee"))
                PendingFeeStudents();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("AcademicPeriod"))
                ChangeAcademicPeriod();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("SMSUI"))
                RecieversListFromSMSUI();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("StudentPayables"))
                RecieversListFromStudentPayable();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("CopyFeeConfiguration"))
                FeesPayableStudentsForStds();
            else
            {
                if (hidDraftId.Value == Constants.S_ZERO)
                    txtToUserId.Text = string.Empty;
                optTeachers.Checked = true;
                optCCTeachers.Checked = true;
            }
            if (moUserRole == Constants.UserRoles.Admin)
                hidUserHasFullAccess.Value = "True";

            if (moUserRole == Constants.UserRoles.Student)
            {
                optStudents.Visible = false;
                optCCStudents.Visible = false;
                trNote.Visible = true;

                if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("Subject_Teacher_Screen"))
                {
                    string sTeacherName = "";
                    int iTeacherUserId = Convert.ToInt32(QueryString["TeacherUserId"]);
                    sTeacherName = Convert.ToString(QueryString["Teacher_Name"]);
                    HidTeacherId.Value = iTeacherUserId.ToString();
                    txtToUserId.Text = sTeacherName;
                    HidTeacherName.Value = sTeacherName;
                    hidUserId.Value = iTeacherUserId.ToString();
                }
            }
        }
        if (moUserRole != Constants.UserRoles.Admin)
        {
            optAll.Visible = false;
            optCCAll.Visible = false;
        }
        if (moUserRole == Constants.UserRoles.Supervisor)
        {
            if (QueryString.Count > 0)
            {
                string sFrom = QueryString["From"];
                if (sFrom != null && sFrom.Equals("Fee"))
                    PendingFeeStudents();
                else if (msForm != null && msForm.Equals("AcademicPeriod"))
                    ChangeAcademicPeriod();
                else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("SMSUI"))
                    RecieversListFromSMSUI();
                else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("StudentPayables"))
                    RecieversListFromStudentPayable();
            }
            else
            {
                txtToUserId.Text = string.Empty;
                optTeachers.Checked = true;
                txtCCUserId.Text = string.Empty;
                optCCTeachers.Checked = true;
            }
            optAll.Visible = true;
            optCCAll.Visible = true;
        }
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.MessageCenter).ToString();
            if (Boolean.Parse(hidUserHasFullAccess.Value))
            {
                optAll.Visible = true;
                optCCAll.Visible = true;
            }
            if (hidDraftId.Value == Constants.S_ZERO)
            {
                optStudents.Checked = true;
                optCCStudents.Checked = true;
            }
            else
            {
                optTeachers.Checked = true;
                optCCTeachers.Checked = true;
            }
            if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("SMSUI"))
                RecieversListFromSMSUI();
        }
    }

    private void FeesPayableStudentsForStds()
    {
        string sStdId = QueryString["StandardId"];
        string sDuedate = QueryString["DueDate"];
        string sFeeType = QueryString["FeeType"];
        string sPayableFor = QueryString["PayableFor"];
        string sAmount = QueryString["Amount"];
        bool bConsiderForRTEConcession = QueryString["ConsiderForRTEConcession"].ToBool();
        string sStudentName = string.Empty;
        string sUserId = string.Empty;
        int iSmsId = Convert.ToInt32(QueryString["SmsId"]);

        string[] stdList = sStdId.Split(',');
        for (int i = 0; i < stdList.Length; i++)
        {
            List<StudentInfo> oStudentList = GetStudentList(Convert.ToInt32(stdList[i]), 0, "", bConsiderForRTEConcession);
            foreach (StudentInfo student in oStudentList)
            {
                sUserId += student.UserId + "; ";
                sStudentName += student.StudentName + ", ";
            }
        }

        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        string sSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);
        sSmsText = sSmsText.Replace("%FEE TYPE%", sFeeType).Replace("%AMOUNT%", sAmount).Replace("%DUEDATE%", sDuedate).Replace("%PAYABLEFOR%", sPayableFor);

        sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
        sUserId = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        HidStudentId.Value = sUserId;
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        optStudents.Checked = true;
        hidData.Value = HttpUtility.HtmlDecode(sSmsText);
        txtSubject.Text = "Fee Updates";
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
    }


    /// <summary>
    /// This method is used to check or un check radio options
    /// </summary>
    /// <param name="bIsCkecked"></param>
    private void CheckUncheckOptions(Boolean bIsCkecked)
    {
        optStudents.Checked = bIsCkecked;
        optTeachers.Checked = bIsCkecked;
        optSupervisor.Checked = bIsCkecked;
        optAll.Checked = bIsCkecked;
    }

    /// <summary>
    /// This method is used to set client side javascript.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        btnSendMessage.Attributes["onclick"] = "ResetLabel();";
        ApplyMouseHoverEffect(new List<Button> { imgBtnGoToInbox, btnSendMessage, btnSendMessageUp, btnChanegAttachment, btnChanegAttachment1, btnChanegAttachment2 });
        chkScheduleMessages.Attributes.Add("onclick", "ScheduleMessage()");
        txtDate.Attributes.Add("onchange", "ScheduleMessage()");
    }

    /// <summary>
    /// Get User id of admin
    /// </summary>
    private void FetchAdminUserId()
    {
        //This function is used to set the Admin User Id's in the Hidden field.
        DataSet oDataSet = SchoolUserCollectionBL.GetAdminAndprincipalOfSchool(miSchoolId, miAcademicYearId, miUserId);
        HidAdminUserID.Value = oDataSet.Tables[0].Rows[0]["User_Id"].ToString();
        HidAdminUserIDCC.Value = oDataSet.Tables[0].Rows[0]["User_Id"].ToString();
        HidAdminUserName.Value = oDataSet.Tables[0].Rows[0]["username"].ToString();
        HidAdminUserNameCC.Value = oDataSet.Tables[0].Rows[0]["username"].ToString();
        HidSuperAdminUserId.Value = oDataSet.Tables[2].Rows[0]["User_Id"].ToString();
        HidSuperAdminUserIdCC.Value = oDataSet.Tables[2].Rows[0]["User_Id"].ToString();
        HidSuperAdminName.Value = oDataSet.Tables[2].Rows[0]["username"].ToString();
        HidSuperAdminNameCC.Value = oDataSet.Tables[2].Rows[0]["username"].ToString();
        hidViewAllStudents.Value = oDataSet.Tables[3].Rows[0]["IsStudentsVisibleForMessage"].ToString();
        if (oDataSet.Tables[1].Rows.Count > 0)
        {
            HidPrincipalUserID.Value = oDataSet.Tables[1].Rows[0]["User_Id"].ToString();
            HidPrincipalUserIDCC.Value = oDataSet.Tables[1].Rows[0]["User_Id"].ToString();
            HidPrincipleName.Value = oDataSet.Tables[1].Rows[0]["username"].ToString();
            HidPrincipleNameCC.Value = oDataSet.Tables[1].Rows[0]["username"].ToString();
        }
    }

    /// <summary>
    /// This method is used to get the Teacher group mailing details.
    /// </summary>
    //private void GetTeacherGroupNames()
    //{
    //    //This function is used to get the list of the Users.        
    //    MailingGroupBL oMailingGroupBL = new MailingGroupBL(miSchoolId, miAcademicYearId, miUserId);
    //    string sGroupName = oMailingGroupBL.GetGroupUserList(hidTeacherGroupId.Value,true);
    //    hidTeacherGroupName.Value = sGroupName;
    //}

    /// <summary>
    /// This method is used to set the attachment.
    /// </summary>
    /// <param name="sAttachment"></param>
    private void SetAttachment(FileAttachment aoAttachment, HyperLink lnkAttach)
    {
        string sAttachment = aoAttachment.FileName;
        string sAttachmentURL = sAttachment;
        sAttachment = sAttachment.Replace("'","\\\'");
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
        if (iTimestampIndex > -1)
            sAttachmentURL = sAttachmentURL.Remove(iIndex, 15);
        lnkAttach.Text = sAttachmentURL;
        string sExtention = sAttachment.Substring(sAttachment.LastIndexOf(".") + 1).ToUpper();
        string sExtensionMap = "PDF,JPG";
        lnkAttach.Attributes.Add("onclick",
                                     String.Format("window.open('{0}','{1}').focus(); return false;",
                                                    sServerFilePath,
                                                    sExtensionMap.IndexOf(sExtention) > -1 ? "_blank" : "_self"));
    }


    private void FillAttachments()
    {
        int iMessageID = Convert.ToInt32(QueryString["MessageID"]);
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL(iMessageID);

        int iIndex = 1;
        foreach (FileAttachment file in oMessageDetailsBL.Attachments)
        {
            HtmlTableRow tr = new HtmlTableRow();

            HtmlTableCell td = new HtmlTableCell();
            td.ID = "td_" + iIndex;
            HyperLink hyper = new HyperLink();
            hyper.ID = "hyper_" + iIndex;
            hyper.Target = "_blank";
            SetAttachment(file, hyper);
            td.Controls.Add(hyper);
            tr.Controls.Add(td);

            HtmlTableCell tdAction = new HtmlTableCell();
            ImageButton img = new ImageButton();
            img.ID = "img_" + iIndex;
            img.ImageUrl = "../images/IconGrid_Delete.gif";
            img.Attributes.Add("onclick", "HideAttachment('" + iIndex + "'); return false;");
            tdAction.Controls.Add(img);

            HiddenField hf = new HiddenField();
            hf.ID = "hidden_" + iIndex;
            hf.Value = Constants.S_ZERO;
            tdAction.Controls.Add(hf);

            HiddenField hfLinkValue = new HiddenField();
            hfLinkValue.ID = "hiddenLinkValue_" + iIndex;
            hfLinkValue.Value = file.FileName;
            tdAction.Controls.Add(hfLinkValue);

            tr.Controls.Add(tdAction);

            pnl.Rows.Add(tr);

            iIndex++;
        }               
    }

    /// <summary>
    /// Set controls if user on this page replying to massage.
    /// </summary>
    private void SetControlsIfReplyMsg()
    {
        //This function is used to check whether the mail is going to reply for previously sended mail.
        //If Yes then check attribute of reply or reply to all.
        //Depend on that set the controls.
        if (Request.QueryString != null && Request.QueryString.ToString() != "" && (msForm == null || msForm == ""))
        {
            int iMessageID = Convert.ToInt32(QueryString["MessageID"]);
            MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL(iMessageID);
            DataSet oDsReceiverUserDetails = MessageDetailsCollectionBL.GetUserIDDetailsByMessageID(iMessageID);
            DataTable oDTReceiverUserDetails = oDsReceiverUserDetails.Tables[0];
            DataTable oDTCcReceiverUserDetails = oDsReceiverUserDetails.Tables[2];

            DataTable oDTStdDivDetails = oDsReceiverUserDetails.Tables[1];
            DataTable oDTCcStdDivDetails = oDsReceiverUserDetails.Tables[3];

            string sprefix = string.Empty;
            string bIfReplyToAll = string.Empty;
            if (!QueryString["IsForward"].IsNull() && QueryString["IsForward"] == "true")
            {
                
                //trfileupload.Visible = false;
                //trfileupload1.Visible = false;
                //trfileupload2.Visible = false;

                sprefix = "FW: ";
                HidReplyUserID.Value = string.Empty;
                bIfReplyToAll = "Forward";

                if (oMessageDetailsBL.Attachments.Count > 0)
                    trAttachments.Visible = true;

                //if (oMessageDetailsBL.Attatchment != null && oMessageDetailsBL.Attatchment != "")
                //{
                //    tdAttachment.Visible = true;
                //    HidAttachment.Value = oMessageDetailsBL.Attatchment;
                //    SetAttachment(oMessageDetailsBL.Attatchment, lnkAttachment);
                //    btnChanegAttachment.Visible = true;
                //}
                //else
                //{
                //    tdAttachment.Visible = false;
                //    trfileupload.Visible = true;
                //    btnChanegAttachment.Visible = false;
                //}

                //if (oMessageDetailsBL.Attatchment1 != null && oMessageDetailsBL.Attatchment1 != "")
                //{
                //    tdAttachment1.Visible = true;
                //    HidAttachment1.Value = oMessageDetailsBL.Attatchment1;
                //    SetAttachment(oMessageDetailsBL.Attatchment1, lnkAttachment1);
                //    btnChanegAttachment1.Visible = true;
                //}
                //else
                //{
                //    tdAttachment1.Visible = false;
                //    trfileupload1.Visible = true;
                //    btnChanegAttachment1.Visible = false;
                //}
                //if (oMessageDetailsBL.Attatchment2 != null && oMessageDetailsBL.Attatchment2 != "")
                //{
                //    tdAttachment2.Visible = true;
                //    HidAttachment2.Value = oMessageDetailsBL.Attatchment2;
                //    SetAttachment(oMessageDetailsBL.Attatchment2, lnkAttachment2);
                //    btnChanegAttachment2.Visible = true;
                //}
                //else
                //{
                //    tdAttachment2.Visible = false;
                //    trfileupload2.Visible = true;
                //    btnChanegAttachment2.Visible = false;
                //}

                int iIndex= 1;

                foreach (FileAttachment file in oMessageDetailsBL.Attachments)
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    
                    HtmlTableCell td = new HtmlTableCell();
                    td.ID = "td_" + iIndex;
                    HyperLink hyper = new HyperLink();
                    hyper.ID = "hyper_" + iIndex;
                    hyper.Target = "_blank";
                    SetAttachment(file, hyper);
                    td.Controls.Add(hyper);
                    tr.Controls.Add(td);

                    HtmlTableCell tdAction = new HtmlTableCell();
                    ImageButton img = new ImageButton();
                    img.ID = "img_" + iIndex;
                    img.ImageUrl = "../images/IconGrid_Delete.gif";
                    img.Attributes.Add("onclick", "HideAttachment('" + iIndex + "'); return false;");
                    tdAction.Controls.Add(img);
                    tr.Controls.Add(tdAction);
                
                    pnl.Rows.Add(tr);
    
                    iIndex++;                    
                }               
            }
            else
            {
                trfileupload.Visible = true;
                trfileupload1.Visible = true;
                trfileupload2.Visible = true;
                tdAttachment.Visible = false;
                tdAttachment1.Visible = false;
                tdAttachment2.Visible = false;
                sprefix = "RE: ";
                HidReplyUserID.Value = oMessageDetailsBL.Sender_User_Id + ";";
                HidReplyUserNames.Value = oMessageDetailsBL.UserName + ", ";
                bIfReplyToAll = "Reply";
                DataRow[] oDrRec = oDTReceiverUserDetails.Select("Receiver_User_Id=" + oMessageDetailsBL.Sender_User_Id);
                if (oMessageDetailsBL.Display_Text == null)
                    oMessageDetailsBL.Display_Text = "";

                if (oMessageDetailsBL.Cc_Display_Text == null)
                    oMessageDetailsBL.Cc_Display_Text = "";

                if (oDrRec.Length > 0)
                {
                    oDTReceiverUserDetails.Rows.Remove(oDrRec[0]);
                    if (oMessageDetailsBL.Display_Text.Contains(HidReplyUserNames.Value))
                    {
                        oMessageDetailsBL.Display_Text = oMessageDetailsBL.Display_Text.Replace(HidReplyUserNames.Value, "");
                    }
                }

                DataRow[] oDrRecCc = oDTCcReceiverUserDetails.Select("Cc_Receiver_User_Id=" + oMessageDetailsBL.Sender_User_Id);

                if (oDrRecCc.Length > 0)
                {
                    oDTCcReceiverUserDetails.Rows.Remove(oDrRecCc[0]);
                    if (!oMessageDetailsBL.Cc_Display_Text.Equals(""))
                    {
                        string[] str = HidReplyUserNames.Value.Split(',');
                        if (!str.Contains(oMessageDetailsBL.Cc_Display_Text) && HidReplyUserNamesCC.Value != string.Empty)
                            oMessageDetailsBL.Cc_Display_Text = oMessageDetailsBL.Cc_Display_Text.Replace(HidReplyUserNamesCC.Value, "");
                    }
                }
                if (oMessageDetailsBL.Display_Text.Contains(HidReplyUserNames.Value.Substring(0, HidReplyUserNames.Value.Length - 2)))
                {
                    oMessageDetailsBL.Display_Text = oMessageDetailsBL.Display_Text.Replace(HidReplyUserNames.Value.Substring(0, HidReplyUserNames.Value.Length - 2), "");
                }
                if (oMessageDetailsBL.Display_Text.Contains(lblFrom.Text))
                {
                    oMessageDetailsBL.Display_Text = oMessageDetailsBL.Display_Text.Replace(lblFrom.Text, "").Replace(",,", "").Replace(", ,", ",");
                    oMessageDetailsBL.Display_Text = oMessageDetailsBL.Display_Text.TrimStart(',');
                }
                if (QueryString["ReplyToAll"] == "true")
                {
                    bIfReplyToAll = "ReplyToAll";
                    if (!oMessageDetailsBL.Display_Text.Contains(Constants.S_ENTIRE_SCHOOL))
                    {
                        {
                            for (int iCount = 0; iCount < oDTReceiverUserDetails.Rows.Count; iCount++)
                            {
                                if (miUserId != Convert.ToInt32(oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"]))
                                {
                                    if (oMessageDetailsBL.Display_Text.Equals(""))
                                    {
                                        HidReplyUserID.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";
                                        HidReplyUserNames.Value += oDTReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    else
                                        HidReplyUserID.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";

                                    if (oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Role_Id"].ToString().Equals("2"))
                                    {
                                        HidTeacherId.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";
                                        HidTeacherName.Value += oDTReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    if (oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Role_Id"].ToString().Equals("3"))
                                    {
                                        HidStudentId.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";
                                        HidStudentName.Value += oDTReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    if (oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Role_Id"].ToString().Equals("6"))
                                    {
                                        HidSupervisorId.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";
                                        HidSupervisorName.Value += oDTReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                }
                            }
                            //if (!oDsReceiverUserDetails.Tables[I_TABLE_GROUP_DETAILS].IsNull() && oDsReceiverUserDetails.Tables[I_TABLE_GROUP_DETAILS].Rows.Count > 0)
                            //{
                            //    for (int iRowCnt = 0; iRowCnt < oDsReceiverUserDetails.Tables[I_TABLE_GROUP_DETAILS].Rows.Count; iRowCnt++)
                            //    {
                            //        hidTeacherGroupId.Value += oDsReceiverUserDetails.Tables[I_TABLE_GROUP_DETAILS].Rows[iRowCnt]["Id"] + ",";
                            //        hidTeacherGroupName.Value += oDsReceiverUserDetails.Tables[I_TABLE_GROUP_DETAILS].Rows[iRowCnt]["Name"] + ",";
                            //    }
                            //}
                            if (oMessageDetailsBL.Sender_User_Id == Convert.ToInt32(HidAdminUserID.Value))
                            {
                                if (moUserRole == Constants.UserRoles.Admin && moUserRole == Constants.UserRoles.Supervisor)
                                {
                                    for (int i = 0; i < oDTStdDivDetails.Rows.Count; i++)
                                    {
                                        HidStdDivId.Value += oDTStdDivDetails.Rows[i]["Receiver_User_Id"].ToString() + ";";
                                        HidStdDivName.Value += oDTStdDivDetails.Rows[i]["UserName"].ToString() + ", ";
                                    }
                                }
                            }
                            TrimTrailingChar();
                            if (!oMessageDetailsBL.Display_Text.Equals(""))
                            {
                               if (!oMessageDetailsBL.Display_Text.Contains(HidReplyUserNames.Value.Replace(",", "")))                                
                                    HidReplyUserNames.Value = HidReplyUserNames.Value + oMessageDetailsBL.Display_Text;                                     
                                else
                                    HidReplyUserNames.Value = oMessageDetailsBL.Display_Text;
                            }
                        }
                    }
                    else
                    {
                        HidReplyUserNames.Value += Constants.S_ENTIRE_SCHOOL + ",";
                        for (int iCount = 0; iCount < oDTReceiverUserDetails.Rows.Count; iCount++)
                        {
                            if (miUserId != Convert.ToInt32(oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"]))
                            {
                                HidReplyUserID.Value += oDTReceiverUserDetails.Rows[iCount]["Receiver_User_Id"].ToString() + ";";
                            }
                        }
                    }
                    if (HidReplyUserNames.Value.Contains(lblFrom.Text))
                    {
                        HidReplyUserNames.Value = HidReplyUserNames.Value.Replace(lblFrom.Text, "").Replace(",,", "").Replace(", ,", ",");
                        HidReplyUserNames.Value = HidReplyUserNames.Value.TrimStart(',');                        
                    }

                    if (HidReplyUserNames.Value.Contains(lblFrom.Text))
                    {
                        HidReplyUserNames.Value = HidReplyUserNames.Value.Replace(lblFrom.Text, "").Replace(",,", "").Replace(", ,", ",");
                        HidReplyUserNames.Value = HidReplyUserNames.Value.TrimStart(',');
                    }

                    if (oMessageDetailsBL.Cc_Display_Text.Contains(lblFrom.Text))
                    {
                        oMessageDetailsBL.Cc_Display_Text = oMessageDetailsBL.Cc_Display_Text.Replace(lblFrom.Text, "").Replace(",,", "").Replace(", ,", ",");
                        oMessageDetailsBL.Cc_Display_Text = oMessageDetailsBL.Cc_Display_Text.TrimStart(',');
                    }

                    if (oMessageDetailsBL.Cc_Display_Text == null)
                    {
                        oMessageDetailsBL.Cc_Display_Text = "";
                    }
                       if (!oMessageDetailsBL.Cc_Display_Text.Contains(Constants.S_ENTIRE_SCHOOL))
                       {
                            for (int iCount = 0; iCount < oDTCcReceiverUserDetails.Rows.Count; iCount++)
                            {
                                if (miUserId != Convert.ToInt32(oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"]))
                                {
                                    if (oMessageDetailsBL.Cc_Display_Text.Equals(""))
                                    {
                                        HidReplyUserIDCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";
                                        HidReplyUserNamesCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    else
                                        HidReplyUserIDCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";

                                    if (oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Role_Id"].ToString().Equals("2"))
                                    {
                                        HidTeacherIdCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";
                                        HidTeacherNameCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    if (oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Role_Id"].ToString().Equals("3"))
                                    {
                                        HidStudentIdCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";
                                        HidStudentNameCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                    if (oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Role_Id"].ToString().Equals("6"))
                                    {
                                        HidSupervisorIdCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";
                                        HidSupervisorNameCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["UserName"].ToString() + ", ";
                                    }
                                }
                            }
                            if (oMessageDetailsBL.Sender_User_Id == Convert.ToInt32(HidAdminUserIDCC.Value))
                            {
                                if (moUserRole == Constants.UserRoles.Admin && moUserRole == Constants.UserRoles.Supervisor)
                                {
                                    for (int i = 0; i < oDTCcStdDivDetails.Rows.Count; i++)
                                    {
                                        HidStdDivIdCC.Value += oDTCcStdDivDetails.Rows[i]["Receiver_User_Id"].ToString() + ";";
                                        HidStdDivNameCC.Value += oDTCcStdDivDetails.Rows[i]["UserName"].ToString() + ", ";
                                    }
                                }
                            }
                            TrimTrailingChar();
                            if (!oMessageDetailsBL.Cc_Display_Text.Equals(""))
                            {
                                if (!oMessageDetailsBL.Cc_Display_Text.Contains(HidReplyUserNamesCC.Value.Replace(",", "")))
                                    HidReplyUserNamesCC.Value = HidReplyUserNamesCC.Value + oMessageDetailsBL.Cc_Display_Text;
                                else
                                    HidReplyUserNamesCC.Value = oMessageDetailsBL.Cc_Display_Text;
                            }
                    }
                    else
                    {
                        HidReplyUserNamesCC.Value += Constants.S_ENTIRE_SCHOOL + ",";
                        for (int iCount = 0; iCount < oDTCcReceiverUserDetails.Rows.Count; iCount++)
                        {
                            if (miUserId != Convert.ToInt32(oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"]))
                            {
                                HidReplyUserIDCC.Value += oDTCcReceiverUserDetails.Rows[iCount]["Cc_Receiver_User_Id"].ToString() + ";";
                            }
                        }
                    }
                   
                }

                if (HidPTAId.Value == string.Empty && miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    HidPTAId.Value = HidReplyUserID.Value;
                    HidPTAIdCC.Value = HidReplyUserIDCC.Value;

                    HidPTAName.Value = HidReplyUserNames.Value;
                    HidPTANameCC.Value = HidReplyUserNamesCC.Value;
                }

                hidUserId.Value = HidReplyUserID.Value;
                hidUserIdCC.Value = HidReplyUserIDCC.Value;

                string[] strarr = new string[HidReplyUserID.Value.Split(';').Length];
                strarr = HidReplyUserID.Value.Split(';');

                if (strarr.Contains(HidAdminUserID.Value))
                {
                    if (chkAdmin.Visible == true)
                    {
                        chkAdmin.Checked = true;
                    }
                }
                if (strarr.Contains(HidSuperAdminUserId.Value))
                {
                    if (chkSuperAdmin.Visible == true)
                    {
                        if (HidSuperAdminUserId.Value.Trim() == miUserId.ToString())
                            chkSuperAdmin.Checked = false;
                        else
                            chkSuperAdmin.Checked = true;                        
                    }
                }
                if (HidReplyUserID.Value.Contains(HidPrincipalUserID.Value + ";") && HidPrincipalUserID.Value.Trim() != string.Empty)
                {
                    if (chkPrincipal.Visible == true)
                    {
                        if (HidPrincipalUserID.Value.Trim() == miUserId.ToString())
                            chkPrincipal.Checked = false;
                        else
                            chkPrincipal.Checked = true;   
                    }
                   
                }
                if (hidUserId.Value.EndsWith(";"))
                {
                    hidUserId.Value = hidUserId.Value.Substring(0, hidUserId.Value.LastIndexOf(";"));
                }
                if (HidReplyUserNames.Value.Trim().EndsWith(","))
                {
                    txtToUserId.Text = HidReplyUserNames.Value.Substring(0, HidReplyUserNames.Value.LastIndexOf(","));
                }
                else
                {
                    txtToUserId.Text = HidReplyUserNames.Value;
                }

                string[] strCCArr = new string[HidReplyUserIDCC.Value.Split(';').Length];
                strCCArr = HidReplyUserIDCC.Value.Split(';');

                if (strCCArr.Contains(HidAdminUserIDCC.Value))
                {
                    if (chkAdminCC.Visible == true)
                    {
                        chkAdminCC.Checked = true;
                    }
                }
                if (strCCArr.Contains(HidSuperAdminUserIdCC.Value))
                {
                    if (chkSuperAdminCC.Visible == true)
                    {
                        chkSuperAdminCC.Checked = true;
                    }
                }
                if (HidReplyUserIDCC.Value.Contains(HidPrincipalUserIDCC.Value + ";") && HidPrincipalUserIDCC.Value.Trim() != string.Empty)
                {
                    if (chkPrincipleCC.Visible == true)
                    {
                        chkPrincipleCC.Checked = true;
                    }
                }
                if (hidUserIdCC.Value.EndsWith(";"))
                {
                    hidUserIdCC.Value = hidUserIdCC.Value.Substring(0, hidUserIdCC.Value.LastIndexOf(";"));
                }
                if (HidReplyUserNamesCC.Value.Trim().EndsWith(","))
                {
                    txtCCUserId.Text = HidReplyUserNamesCC.Value.Substring(0, HidReplyUserNamesCC.Value.LastIndexOf(","));
                }
                else
                {
                    txtCCUserId.Text = HidReplyUserNamesCC.Value;
                }
                HidReplyUserIDCC.Value = oMessageDetailsBL.Sender_User_Id.ToString();
            }

            HidReplyUserNames.Value = txtToUserId.Text;
            HidReplyUserIDCC.Value = txtCCUserId.Text;

            txtSubject.Text = sprefix + oMessageDetailsBL.Subject;

            //hidData.Value = "<p><br><br><br><br><br></p>" + HttpUtility.HtmlEncode("------------ Original message ------------") + "<br>Sent On : " + oMessageDetailsBL.Insert_Date.ToString(Constants.S_DATE_FORMAT + " hh:mm tt") + "<br><br>" +
            //                        HttpUtility.HtmlDecode(Convert.ToString(oMessageDetailsBL.Message_Body));

            
            divData.Visible = true;
            divData.InnerHtml = "<p><br><br><br></p>" + HttpUtility.HtmlEncode("------------ Original message ------------") + "<br>Sent On : " + oMessageDetailsBL.Insert_Date.ToString(Constants.S_DATE_FORMAT + " hh:mm tt") + "<br><br>" +
                                    HttpUtility.HtmlDecode(Convert.ToString(oMessageDetailsBL.Message_Body));
            hidOldData.Value = divData.InnerHtml;

            SetRadioButtons(oMessageDetailsBL.Sender_User_Role_Id, bIfReplyToAll, oMessageDetailsBL);
        }
        else
        {
            string sIsStudentLevel = string.Empty;
            if (!Boolean.Parse(hidUserHasFullAccess.Value) && (moUserRole == Constants.UserRoles.Teacher) && (optStudents.Checked))
                sIsStudentLevel = "&IsStudentLevel=Y";
            if (HidStudentId.Value != "")
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
            else if (optStudents.Checked)
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);

            else if (optParentTeacherAssociation.Checked)/////
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=ParentTeacherAssociation" + "&sUserId=" + hidUserId.Value + "&IsPTAMember=Y" + sIsStudentLevel);

            else if (optSupervisor.Checked)
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);
            else
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value + sIsStudentLevel);

            string sIsCc = "true";
            if (HidStudentIdCC.Value != "")
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel + "&IsCc=" + sIsCc);
            else if (optCCStudents.Checked)
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Student" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel + "&IsCc=" + sIsCc);

            else if (optCCParentTeacherAssociation.Checked)////////
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=ParentTeacherAssociation" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel + "&IsCc=" + sIsCc + "&IsPTAMember=Y");

              else if (optCCSupervisor.Checked)
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Supervisor" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel + "&IsCc=" + sIsCc);

            else
                hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserIdCC.Value + sIsStudentLevel + "&IsCc=" + sIsCc);
        }
    }

    /// <summary>
    /// This method removes unnecessary trailing characters
    /// Basically it removes ";" from id fields and "," from name fields.
    /// </summary>
    private void TrimTrailingChar()
    {
        if (HidTeacherName.Value.EndsWith(", "))
        {
            HidTeacherName.Value = HidTeacherName.Value.Substring(0, HidTeacherName.Value.LastIndexOf(","));
        }
        if (HidSupervisorName.Value.EndsWith(", "))
        {
            HidSupervisorName.Value = HidSupervisorName.Value.Substring(0, HidSupervisorName.Value.LastIndexOf(", "));
        }

        if (HidPTAName.Value.EndsWith(", "))
        {
            HidPTAName.Value = HidPTAName.Value.Substring(0, HidPTAName.Value.LastIndexOf(", "));
        }

        if (HidStdDivName.Value.EndsWith(", "))
        {
            HidStdDivName.Value = HidStdDivName.Value.Substring(0, HidStdDivName.Value.LastIndexOf(","));
        }
        if (HidStudentName.Value.EndsWith(", "))
        {
            HidStudentName.Value = HidStudentName.Value.Substring(0, HidStudentName.Value.LastIndexOf(","));
        }
        if (HidTeacherId.Value.EndsWith(";"))
        {
            HidTeacherId.Value = HidTeacherId.Value.Substring(0, HidTeacherId.Value.LastIndexOf(";"));
        }
        if (HidSupervisorId.Value.EndsWith(";"))
        {
            HidSupervisorId.Value = HidSupervisorId.Value.Substring(0, HidSupervisorId.Value.LastIndexOf(";"));
        }

        if (HidPTAId.Value.EndsWith(";"))
        {
            HidPTAId.Value = HidPTAId.Value.Substring(0, HidPTAId.Value.LastIndexOf(";"));
        }
        
        if (HidStdDivId.Value.EndsWith(";"))
        {
            HidStdDivId.Value = HidStdDivId.Value.Substring(0, HidStdDivId.Value.LastIndexOf(";"));
        }
        if (HidStudentId.Value.EndsWith(";"))
        {
            HidStudentId.Value = HidStudentId.Value.Substring(0, HidStudentId.Value.LastIndexOf(";"));
        }        
    }

    /// <summary>
    /// This method is used initialise this form with search criteria we selected 
    /// when we redirect here from Pending Fee Student List.
    /// </summary>
    private void PendingFeeStudents()
    {
        int iStdId = Convert.ToInt32(QueryString["Standard_Id"]);
        int iDivId = Convert.ToInt32(QueryString["Division_Id"]);
        string sRegNo = QueryString["sRegNo"];
        string sDuedate = QueryString["DueDate"];
        bool bLfetStudent = true;
        bool bPDCStudent = Convert.ToBoolean(QueryString["bPDCStudent"]);
        int iFeeTypeId = Convert.ToInt32(QueryString["FeeTypeId"]);
        string sPayableFor = Convert.ToString(QueryString["PayableFor"]);
        string sOperator = QueryString["Operator"];
        int iAmount = Convert.ToInt32(QueryString["Amount"]);
        string sPercentFilter = QueryString["PercentFilter"];
        string sStudentName = "";
        string sUserId = "";
        DataTable OdtStudDetail = StudentBL.GetPendingFeeStudentList(miSchoolId, miAcademicYearId, iStdId, iDivId, sRegNo, sDuedate, bLfetStudent, bPDCStudent, iFeeTypeId,sPayableFor, sOperator, iAmount, Constants.S_EMPTY_STRING, Constants.I_ZERO, Constants.I_ZERO, sPercentFilter);
        for (int iCount = 0; iCount < OdtStudDetail.Rows.Count; iCount++)
        {
            sUserId += OdtStudDetail.Rows[iCount]["User_Id"].ToString() + "; ";
            sStudentName += OdtStudDetail.Rows[iCount]["SMSName"].ToString() + ", ";
        }
        sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
        HidStudentId.Value = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        chkAdmin.Checked = false;
        chkAdmin.Visible = false;
        HideSwCordinatorOption();
        optStudents.Checked = true;
        hidData.Value = HttpUtility.HtmlDecode(S_COMMON_PENDING_FEE_MSG);
        txtSubject.Text = S_PENDING_FEE_SUBJECT;
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
    }

    /// <summary>
    /// This method is used retrive student list for whome the new fee is added.   /// 
    /// </summary>
    private void RecieversListFromStudentPayable()
    {
        int iStdId = Convert.ToInt32(QueryString["StandardId"]);
        int iDivId = Convert.ToInt32(QueryString["DivisionId"]);
        string sRegNo = Convert.ToString(QueryString["RegNo"]);
        string sDuedate = QueryString["DueDate"];
        string sFeeType = QueryString["FeeType"];
        string sPayableFor = QueryString["PayableFor"];
        string sAmount = QueryString["Amount"];
        bool bConsiderForRTEConcession = QueryString["ConsiderForRTEConcession"].ToBool();
        string sStudentName = string.Empty;
        string sUserId = string.Empty;
        int iSmsId = Convert.ToInt32(QueryString["SmsId"]);

        List<StudentInfo> oStudentList = GetStudentList(iStdId, iDivId, sRegNo, bConsiderForRTEConcession);

        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        string sSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);
        sSmsText = sSmsText.Replace("%FEE TYPE%", sFeeType).Replace("%AMOUNT%", sAmount).Replace("%DUEDATE%", sDuedate).Replace("%PAYABLEFOR%", sPayableFor);

        foreach (StudentInfo student in oStudentList)
        {
            sUserId += student.UserId + "; ";
            sStudentName += student.StudentName + ", ";
        }

        sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
        sUserId = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        HidStudentId.Value = sUserId;
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        optStudents.Checked = true;
        hidData.Value = HttpUtility.HtmlDecode(sSmsText);
        txtSubject.Text = "Fee Updates";
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
    }

    /// <summary>
    /// This method is used to get students to whome message/sms will be sent.
    /// </summary>
    /// <param name="IsNew"></param>
    private List<StudentInfo> GetStudentList(int aiStdId, int aiDivId, string asRegNo, bool abConsiderForRTEConcession)
    {
        Hashtable oHashTable = new Hashtable();
        List<StudentInfo> oStudentList = StudentBL.GetStudentsForFeesUpdate(miSchoolId, miAcademicYearId, aiStdId, aiDivId, asRegNo, abConsiderForRTEConcession);
        return oStudentList;
    }

    /// <summary>
    /// This method is used retrive user list to whome Sms is sent.
    /// </summary>
    private void RecieversListFromSMSUI()
    {
        txtToUserId.Text = PrepareRecieversList();
        SetRadioButtons();
        if (Session["MessageType"] != null)
            txtSubject.Text = Convert.ToString(Session["MessageType"]);
        hidData.Value = HttpUtility.HtmlDecode(Convert.ToString(Session["MessageText"]));
        RemoveSessionVariables();
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
    }

    /// <summary>
    /// This method is used set option buttons.
    /// </summary>
    private void SetRadioButtons()
    {
        if (HidUserNames.Value.Equals("Entire School"))
        {
            optAll.Checked = true;
            chkAdmin.Enabled = false;
            chkPrincipal.Enabled = false;
            chkSuperAdmin.Enabled = false;
        }
        else
        {
            optStudents.Checked = true;
        }
    }

    /// <summary>
    /// This method is used to set recievers list to whome sms is sent.
    /// </summary>
    private string PrepareRecieversList()
    {
        HidTeacherName.Value = Convert.ToString(Session["TeacherNameList"]);
        HidTeacherId.Value = Convert.ToString(Session["TeacheIdList"]);

        HidStudentName.Value = Convert.ToString(Session["StudentNameList"]);
        HidStudentId.Value = Convert.ToString(Session["StudentIdList"]);

        HidSupervisorName.Value = Convert.ToString(Session["AdminstaffNameList"]);
        HidSupervisorId.Value = Convert.ToString(Session["AdminstaffIdList"]);

        HidPTAName.Value = Convert.ToString(Session["PTANameList"]);
        HidPTAId.Value = Convert.ToString(Session["PTAIdList"]);

        chkAdmin.Checked = Convert.ToBoolean(Session["IsAdminSelected"]);
        chkPrincipal.Checked = Convert.ToBoolean(Session["IsPrincipleSelected"]);

        if (!Session["UserGroupId"].IsNull() && !Session["UserGroupName"].IsNull())
        {
            hidUserGroupId.Value = Session["UserGroupId"].ToString();
            hidUserGroupName.Value = Session["UserGroupName"].ToString();
        }

        string sUsers = string.Empty;
        if (Session["IsEntireSchoolSelected"].ToBool())
        {
            optAll.Checked = true;
            sUsers = optAll.Text + ", ";
        }

        if (!string.IsNullOrEmpty(hidUserGroupName.Value))
            sUsers += hidUserGroupName.Value + ", ";
        if (chkAdmin.Checked)
            sUsers += HidAdminUserName.Value + ", ";
        if (chkPrincipal.Checked)
            sUsers += HidPrincipleName.Value + ", ";
        if (!string.IsNullOrEmpty(HidTeacherName.Value))
            sUsers += HidTeacherName.Value + ", ";
        if (!string.IsNullOrEmpty(HidSupervisorName.Value))
            sUsers += HidSupervisorName.Value + ", ";
        if (!string.IsNullOrEmpty(HidPTAName.Value))
            sUsers += HidPTAName.Value + ", ";
        if (!string.IsNullOrEmpty(HidStudentName.Value))
            sUsers += HidStudentName.Value;

        return !sUsers.IsNullOrEmpty() ? sUsers.Substring(0, sUsers.Length - 2) : sUsers;
    }

    /// <summary>
    /// This method is used to remove session variables.
    /// </summary>
    private void RemoveSessionVariables()
    {
        Session.Remove("MessageText");
        Session.Remove("UserIdList");
        Session.Remove("UserNameList");
        Session.Remove("IsAdminSelected");
        Session.Remove("IsPrincipleSelected");
        Session.Remove("MessageText");
        Session.Remove("TeacherNameList");
        Session.Remove("TeacheIdList");
        Session.Remove("StudentNameList");
        Session.Remove("StudentIdList");
        Session.Remove("AdminstaffNameList");
        Session.Remove("AdminstaffIdList");

        Session.Remove("PTANameList");
        Session.Remove("PTAIdList");
        
        Session.Remove("UserNameList");
        Session.Remove("UserIdList");
        Session.Remove("IsAdminSelected");
        Session.Remove("IsPrincipleSelected");
        Session.Remove("MessageType");
        Session.Remove("UserGroupId");
        Session.Remove("UserGroupName");
    }

    private void ChangeAcademicPeriod()
    {
        chkSuperAdmin.Checked = true;
        txtToUserId.Text = S_SOFTWARE_COORDINATOR;
        txtSubject.Text = S_CHANGE_ACADEMIC_YEAR_PERIOD_SUBJECT;
        hidData.Value = HttpUtility.HtmlDecode(S_CHANGE_ACADEMIC_YEAR_PERIOD);
    }

    /// <summary>
    /// Add entry to notification table so service send notifiction to users selected for message.
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <param name="sSubjectName"></param>
    public override void SendPushNotification(string sUserIds, object sSubjectName)
    {
        if (sUserIds != string.Empty)
        {
            PushNotificationClient pushNotificationClient = null;
            try
            {
                pushNotificationClient = new PushNotificationClient();
                string[] strArrayUserid = sUserIds.Split(';');
                int[] intArrayUserId = Array.ConvertAll(strArrayUserid, userId => int.Parse(userId));
                string messageSubject = txtSubject.Text.Trim();
                Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
                dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_FULLNAME, Session[Constants.S_SESSION_USER_FULLNAME].ToString());
                dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_MESSAGE_SUBJECT, Convert.ToString(sSubjectName));
                pushNotificationClient.SendNotification(NotificationMessageHeadings.NewMessageArrived, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
                pushNotificationClient.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
            finally
            {
                if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                    pushNotificationClient.Close();
            }
        }
    }

    /// <summary>
    /// This Method is Used for Save The Message as Draft Message in Draft.
    /// </summary>
    private void SaveMessageAsDraft()
    {        
        string sMessageBody = HttpUtility.HtmlEncode(hidData.Value);
        if (sMessageBody != string.Empty)
        {
            moMessageDetailsBL = new MessageDetailsBL();
            string ReceipantUserIds = hidUserId.Value;
            string Subjects = txtSubject.Text;
            string sUserNames = GetUserNameList();
            if (sUserNames == string.Empty && hidUserId.Value.Length >= 2000)
                sUserNames = "Entire School";

            string CcReciepientUserIds = hidUserIdCC.Value;
            string sCcUserNames = GetCcUserNameList();
            if (sCcUserNames == string.Empty && hidUserIdCC.Value.Length >= 2000)
                sCcUserNames = "Entire School";

            int iDraftId = hidDraftId.Value.ToInt();
            int iValue = moMessageDetailsBL.SaveMessageAsDraft(iDraftId, ReceipantUserIds, sUserNames, Subjects, sMessageBody, miUserId, miSchoolId, miAcademicYearId, CcReciepientUserIds, sCcUserNames);
            if (iValue != iDraftId)
                lblSaveMsg.Text = "Message saved successfully!!!";
            else
                lblSaveMsg.Text = "Message updated successfully!!!";

            hidDraftId.Value = iValue.ToString();
        }
    }

    /// <summary>
    /// This Method is Used for Read Query String For Draft.
    /// </summary>
    private void ReadQueryString()
    {
        if (!QueryString["DraftMessageId"].IsNull())
            hidDraftId.Value = QueryString["DraftMessageId"].ToString();

        hidDeleteedIds.Value = string.Empty;
    }

    /// <summary>
    /// This method is Used for Get the Message Details Of Draft Message.
    /// </summary>
    private void FillDraftDetails()
    {
        List<MessageDraftUserDetails> lstMessageDraftUserDetails;
        List<StandardDivisionDetails> lstStandardDivisionDetails;
        List<MessageDraftUserDetails> lstMessageDraftCCUserDetails;
        moMessageDetailsBL = new MessageDetailsBL();

        MessageDraftDetails oMessageDraftDetails = moMessageDetailsBL.GetMessageDetailsForDraftEdit(hidDraftId.Value.ToInt(), miUserId, miSchoolId, miAcademicYearId, out lstStandardDivisionDetails, out lstMessageDraftUserDetails, out lstMessageDraftCCUserDetails);

        var std = (from lstMd in lstMessageDraftUserDetails 
                   join lstSD in lstStandardDivisionDetails 
                     on lstMd.UserId equals lstSD.StandardDivisionId
                   where lstMd.UserRoleId == 0
                   select new { lstMd.UserId ,lstMd.UserName}).ToList();

        var CCstd = (from lstMd in lstMessageDraftCCUserDetails
                     join lstSD in lstStandardDivisionDetails
                       on lstMd.UserId equals lstSD.StandardDivisionId
                     where lstMd.UserRoleId == 0
                     select new { lstMd.UserId, lstMd.UserName }).ToList();

        if (std.Count > Constants.I_ZERO)
        {
            foreach (var div in std)
            {
                HidStdDivId.Value = HidStdDivId.Value + ";" + div.UserId.ToString();
                HidStdDivName.Value = HidStdDivName.Value + ";" + div.UserName.ToString();
            }
        }

        if (CCstd.Count > Constants.I_ZERO)
        {
            foreach (var div in CCstd)
            {
                HidStdDivIdCC.Value = HidStdDivIdCC.Value + ";" + div.UserId.ToString();
                HidStdDivNameCC.Value = HidStdDivNameCC.Value + ";" + div.UserName.ToString();
            }
        }

        if (lstMessageDraftUserDetails.Count > Constants.I_ZERO)
        {
            foreach (var lst in lstMessageDraftUserDetails)
            {
                if (lst.UserRoleId == Constants.UserRoles.Teacher.ToInt())
                {
                    HidTeacherId.Value = HidTeacherId.Value + ";" + lst.UserId.ToString();
                    HidTeacherName.Value = HidTeacherName.Value + ";" + lst.UserName.ToString();
                }

                if (lst.UserRoleId == Constants.UserRoles.Student.ToInt())
                {
                    HidStudentId.Value = HidStudentId.Value + ";" + lst.UserId.ToString();
                    HidStudentName.Value = HidStudentName.Value + ";" + lst.UserName.ToString();
                }

                if (lst.UserRoleId == Constants.UserRoles.Supervisor.ToInt())
                {
                    HidSupervisorId.Value = HidSupervisorId.Value + ";" + lst.UserId.ToString();
                    HidSupervisorName.Value = HidSupervisorName.Value + ";" + lst.UserName.ToString();
                }
            }
        }

        if (lstMessageDraftCCUserDetails.Count > Constants.I_ZERO)
        {
            foreach (var lst in lstMessageDraftCCUserDetails)
            {
                if (lst.UserRoleId == Constants.UserRoles.Teacher.ToInt())
                {
                    HidTeacherIdCC.Value = HidTeacherIdCC.Value + ";" + lst.UserId.ToString();
                    HidTeacherNameCC.Value = HidTeacherNameCC.Value + ";" + lst.UserName.ToString();
                }

                if (lst.UserRoleId == Constants.UserRoles.Student.ToInt())
                {
                    HidStudentIdCC.Value = HidStudentIdCC.Value + ";" + lst.UserId.ToString();
                    HidStudentNameCC.Value = HidStudentNameCC.Value + ";" + lst.UserName.ToString();
                }

                if (lst.UserRoleId == Constants.UserRoles.Supervisor.ToInt())
                {
                    HidSupervisorIdCC.Value = HidSupervisorIdCC.Value + ";" + lst.UserId.ToString();
                    HidSupervisorNameCC.Value = HidSupervisorNameCC.Value + ";" + lst.UserName.ToString();
                }
            }
        }

        if (HidStdDivId.Value != string.Empty)
        {
            HidStdDivId.Value = HidStdDivId.Value.Substring(1);
            HidStdDivId.Value = HidStdDivId.Value.Trim();
            HidStdDivName.Value = HidStdDivName.Value.Substring(1);
            HidStdDivName.Value = HidStdDivName.Value.Trim();
        }

        if (HidStdDivIdCC.Value != string.Empty)
        {
            HidStdDivIdCC.Value = HidStdDivIdCC.Value.Substring(1);
            HidStdDivIdCC.Value = HidStdDivIdCC.Value.Trim();
            HidStdDivNameCC.Value = HidStdDivNameCC.Value.Substring(1);
            HidStdDivNameCC.Value = HidStdDivNameCC.Value.Trim();
        }

        if (HidStudentId.Value != string.Empty)
        {
            HidStudentId.Value = HidStudentId.Value.Substring(1);
            HidStudentId.Value = HidStudentId.Value.Trim();
            HidStudentName.Value = HidStudentName.Value.Substring(1);
            HidStudentName.Value = HidStudentName.Value.Trim();
        }

        if (HidStudentIdCC.Value != string.Empty)
        {
            HidStudentIdCC.Value = HidStudentIdCC.Value.Substring(1);
            HidStudentIdCC.Value = HidStudentIdCC.Value.Trim();
            HidStudentNameCC.Value = HidStudentNameCC.Value.Substring(1);
            HidStudentNameCC.Value = HidStudentNameCC.Value.Trim();
        }

        if (HidTeacherId.Value != string.Empty)
        {
            HidTeacherId.Value = HidTeacherId.Value.Substring(1);
            HidTeacherId.Value = HidTeacherId.Value.Trim();
            HidTeacherName.Value = HidTeacherName.Value.Substring(1);
            HidTeacherName.Value = HidTeacherName.Value.Trim();
        }

        if (HidTeacherIdCC.Value != string.Empty)
        {
            HidTeacherIdCC.Value = HidTeacherIdCC.Value.Substring(1);
            HidTeacherIdCC.Value = HidTeacherIdCC.Value.Trim();
            HidTeacherNameCC.Value = HidTeacherNameCC.Value.Substring(1);
            HidTeacherNameCC.Value = HidTeacherNameCC.Value.Trim();
        }

        if (HidSupervisorId.Value != string.Empty)
        {
            HidSupervisorId.Value = HidSupervisorId.Value.Substring(1);
            HidSupervisorId.Value = HidSupervisorId.Value.Trim();
            HidSupervisorName.Value = HidSupervisorName.Value.Substring(1);
            HidSupervisorName.Value = HidSupervisorName.Value.Trim();
        }

        if (HidSupervisorIdCC.Value != string.Empty)
        {
            HidSupervisorIdCC.Value = HidSupervisorIdCC.Value.Substring(1);
            HidSupervisorIdCC.Value = HidSupervisorIdCC.Value.Trim();
            HidSupervisorNameCC.Value = HidSupervisorNameCC.Value.Substring(1);
            HidSupervisorNameCC.Value = HidSupervisorNameCC.Value.Trim();
        }

        txtToUserId.Text = oMessageDraftDetails.DisplayText;
        txtCCUserId.Text = oMessageDraftDetails.CcDisplayText;

        txtSubject.Text = oMessageDraftDetails.Subject;
        hidData.Value = HttpUtility.HtmlDecode(oMessageDraftDetails.MessageBody);
               
        HidReplyUserID.Value = oMessageDraftDetails.ReceipantList;
        HidReplyUserIDCC.Value = oMessageDraftDetails.CcReciepientList;
        lblFrom.Text = oMessageDraftDetails.FromName;

        if (oMessageDraftDetails.DisplayText.Contains("Software Coordinator") && oMessageDraftDetails.ReceipantList != string.Empty)
            chkSuperAdmin.Checked = true;

        if (oMessageDraftDetails.DisplayText.Contains("Chief Administrative Officer") && oMessageDraftDetails.ReceipantList != string.Empty)
            chkAdmin.Checked = true;

        if (oMessageDraftDetails.DisplayText.Contains("Principal") && oMessageDraftDetails.ReceipantList != string.Empty)
            chkPrincipal.Checked = true;

        optTeachers.Checked = true;
              
        if(optTeachers.Checked)
            hidQry.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value);


        if (oMessageDraftDetails.CcDisplayText.Contains("Software Coordinator") && oMessageDraftDetails.CcReciepientList != string.Empty)
            chkSuperAdminCC.Checked = true;

        if (oMessageDraftDetails.CcDisplayText.Contains("Chief Administrative Officer") && oMessageDraftDetails.CcReciepientList != string.Empty)
            chkAdminCC.Checked = true;

        if (oMessageDraftDetails.CcDisplayText.Contains("Principal") && oMessageDraftDetails.CcReciepientList != string.Empty)
            chkPrincipleCC.Checked = true;

        optCCTeachers.Checked = true;

        if (optCCTeachers.Checked)
            hidQryCC.Value = CommonUtility.EncryptQuerystring("Mode=Message&UsersList=Teacher" + "&sUserId=" + hidUserId.Value);

    }

    /// <summary>
    /// This Method is Used For Remove/Delete The Message from Draft.
    /// </summary>
    private void RemoveMessaegFromDraft()
    {
        moMessageDetailsBL = new MessageDetailsBL();
        moMessageDetailsBL.DeleteMessageFromDraft(hidDraftId.Value.ToInt(),miUserId,miSchoolId,miAcademicYearId);
    }

    /// <summary>
    /// This method is used to set default fields.
    /// </summary>
    private void SetDefaultFields()
    {
        hidShowOnlyCoordinators.Value = (Settings.ShowOnlyCoOrdinators ? Constants.S_ONE : Constants.S_ZERO);
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        hidLoginUserId.Value = miUserId.ToString();

        if (!hidData.Value.Contains("Thanks and Regards"))
            hidData.Value = hidData.Value + "</br></br>Thanks and Regards,</br>" + Session[Constants.S_SESSION_USER_FULLNAME];
    }

    /// <summary>
    /// This method is used to set restriction.
    /// </summary>
    private void RestrictFields()
    {
        if (Settings.RestrictCopyDataFromMessageCenter && moUserRole ==  Constants.UserRoles.Student)
            hidRestrictCopy.Value = Constants.S_ONE;
        else
            hidRestrictCopy.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is sued to hide s/w coordinator option.
    /// </summary>
    private void HideSwCordinatorOption()
    {
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt() && moUserRole == Constants.UserRoles.Student)
        {
            chkSuperAdmin.Visible = false;
            chkSuperAdminCC.Visible = false;
        }
    }

    private void SetViewAsPerDesingation()
    {
        moMessageDetailsBL = new MessageDetailsBL();
        string sDesignation = moMessageDetailsBL.GetDesignation(miUserId, miSchoolId);
        
        if (moSchool == Constants.SchoolId.PPSN && sDesignation == Constants.S_MD_DESIGNATION)
            SetFieldVisibility(false);
        //else
        //    SetFieldVisibility(true);
    }

    private void SetFieldVisibility(bool abVisible)
    {
        lnkTeacherGroups.Visible = abVisible;
        optStudents.Visible = abVisible;
        optAll.Visible = abVisible;

        lnkTeacherGroupsCC.Visible = abVisible;
        optCCStudents.Visible = abVisible;
        optCCAll.Visible = abVisible;
    }

    [WebMethod]
    public static UserData GetUserDetails(string asUserName, string asSchoolId, string asAcademicYearId)
    {
        MessageDetailsBL oMessageDetailsBL = new MessageDetailsBL();
        UserData oUserData = oMessageDetailsBL.GetUserData(asUserName, asSchoolId, asAcademicYearId);

        return oUserData;
    }

    #endregion
   
}
