/*
 *  File Name : - SMSUI.aspx.cs
 *  Purpose   : - This class is used to create new SMS/ view SMS to send multiple users.
 *  Date      : - 22-Mar-2008
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;
using System.Web;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BusinessSMSBalanceService.Client;
using System.Xml;
using System.Configuration;
public partial class SMSUI : SchoolBase
{
    #region Constant's

    private const string S_SCHEDULED_SMS = "3";
    private const string S_SMS_BALANCE_COUNT_ERRORMSG = "SMS balance is low so you can not send SMS to selected staffs / students. Please communicate with software coordinator to recharge your SMS account.";

    #endregion

    #region Data Members

    Hashtable moHTUsersMobileNo = new Hashtable();
    Hashtable moManualMobileNo = new Hashtable();
    
    string msForm = "";
    #endregion

    #region Events

    /// <summary>
    /// This method is used to initialize page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetQuerystring();
                bool b = IsAllSentSMSbtnVisibility(miUserId.ToInt());
                if (b == true)
                {
                    btnAllSentItems.Visible = true;
                }
                else
                {
                    btnAllSentItems.Visible = false;
                }
                CheckPreConditions();

                hidResendUserName.Value = "0";
                CheckRoleAndAssignDisplayView();
                SetDefaultPageControls();
                valSum_SendMessage.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                SetSMSCount();
                SetClientScriptAttributes();
                GetSMSBalance();
                InitializeValues();
            }            
        }
        catch (NoUserOtherThanAdminExceptions ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            trMobileView.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// On this event all message details saving database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSendMessage_Click(object sender, EventArgs e)
    {
        try
        {
            string sQuerrystring = string.Empty;
            int iSMSCount = PrepareReceiversList();
            string sSendSMS = ConfigurationManager.AppSettings["SendSMS"].ToString();
            int iSMSLength = 0;
            if (txtMessage.Text.Length % 160 == 0)
                iSMSLength = txtMessage.Text.Length / 160;
            else
                iSMSLength = (txtMessage.Text.Length / 160) + 1;
            //if (sSendSMS == Constants.S_YES && (iSMSCount * iSMSLength) > hidSMSCountVal.Value.ToInt())
            //    Label1.Text = S_SMS_BALANCE_COUNT_ERRORMSG;
            //else
			
            int iCount = SendSMS();
            if (chkSendMessage.Checked)
                sQuerrystring = PrepareQueryString(false);
            ResetAllControls();
            RedirectToPage(iCount, sQuerrystring);
            //GetSMSBalance();
            
        }
        catch(SqlException ex)
        {
            Label1.Text = ex.Message;
        }
        catch (ValidMobileNumberExceptions ex)
        {
            Label1.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view all sent sms.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAllSentItems_Click(object sender, EventArgs e)
    {
        try
        {

            HidState.Value = Constants.S_ONE;

            SetQueryString(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This methos is used to  redirect to sent items
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSentItems_Click(object sender, EventArgs e)
    {
        try
        {
            HidState.Value = Constants.S_ONE;
            SetQueryString(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event it used to prepare and redirect to the scheduled sms screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnScheduledSMS_Click(object sender, EventArgs e)
    {
        try
        {
            HidState.Value = S_SCHEDULED_SMS;
            SetQueryString(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This methid is used to redirect to back to the listing page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
            btnBack.PostBackUrl = "SMSHistoryUI.aspx";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view Inbox messages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRecieved_Click(object sender, EventArgs e)
    {
        try
        {
            string sQuerrystring = string.Empty;
            ClearSMSSessionVariable();
            HidState.Value = Constants.S_ZERO;
            sQuerrystring = PrepareQueryString(false);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("SMSHistoryUI.aspx?" + sQuerrystring);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for handler to chkManualNumber changed 
    /// to enable or disable manual update text box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkManualNumber_CheckedChanged(object sender, EventArgs e)
    {
        txtManualNumbers.Enabled = chkManualNumber.Checked;       
        if (txtManualNumbers.Enabled)
        {
            txtManualNumbers.CssClass = "LrgMobileTxtBox";
            spnMandManualNos.Style.Add("visibility", "display");
        }
        else
        {
            txtManualNumbers.CssClass = "ClsReadOnly";
            spnMandManualNos.Style.Add("visibility", "hidden");
        }
        
    }

    /// <summary>
    /// This method is used to add decrypted querystring
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void opt_CheckedChanged(object sender, EventArgs e)
    {
        if (txtToUserId.Text == "EntireSchool")
            txtToUserId.Text = string.Empty;
			
        string UsersList = string.Empty;
        chkAdmin.Enabled = true;
        chkPrincipal.Enabled = true;
        lnkTeacherGroups.Enabled = true;
        if (optTeachers.Checked)
            UsersList = "Teacher";
        else if (optStudents.Checked)
            UsersList = "Student";
        else if (optSupervisor.Checked)
            UsersList = "Supervisor";
        else if (optOtherStaff.Checked)
            UsersList = "OtherStaff";
        else if (optParentTeacherAssociation.Checked)
            UsersList = "ParentTeacherAssociation";
        else if (optEntireSchool.Checked)
        {
            UsersList = "EntireSchool";
            HidStdDivId.Value = string.Empty;
            HidStdDivName.Value = string.Empty;
            HidStudentId.Value = string.Empty;
            HidStudentName.Value = string.Empty;
            HidSupervisorId.Value = string.Empty;
            HidSupervisorName.Value = string.Empty;
            HidTeacherId.Value = string.Empty;
            HidTeacherName.Value = string.Empty;
            HidOtherStaffId.Value = string.Empty;
            HidOtherStaffName.Value = string.Empty;
            hidUserId.Value = string.Empty;
            HidUserNames.Value = string.Empty;
            HidParentTeacherAssociationId.Value = string.Empty;
            HidParentTeacherAssociationName.Value = string.Empty;
            chkAdmin.Checked = false;
            chkAdmin.Enabled = false;
            chkPrincipal.Checked = false;
            chkPrincipal.Enabled = false;
            lnkTeacherGroups.Enabled = false;
            hidGroupId.Value = string.Empty;
            hidGroupName.Value = string.Empty;
        }
        else if (optLeftStudents.Checked)
            UsersList = "LeftStudents";
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=" + UsersList + "&sUserId=" + hidUserId.Value);        
    }

    #endregion

    #region Helping Methods

    /// <summary>
    /// This function is used to set appropriate query string.
    /// </summary>
    private void SetQueryString(bool abShowAllSentSMS)
    {
        string sQuerystring = string.Empty;
        ClearSMSSessionVariable();
        sQuerystring = PrepareQueryString(abShowAllSentSMS);
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage("SMSHistoryUI.aspx?" + sQuerystring);
    }

    private bool IsAllSentSMSbtnVisibility(int aiUserId)
    {
        return SchoolUserCollectionBL.IsAllSentSMSbtnVisibility(aiUserId);
    }

    /// <summary>
    /// This method is used to send sms.
    /// </summary>
    private int SendSMS()
    {
        SMS oSMS = new SMS();
        oSMS.Sender = txtFromMb.Text;
        oSMS.SMSCount = Convert.ToInt32(hidSMSCount.Value);
        oSMS.SMSText = txtMessage.Text.Replace("\\","\\\\").Trim();        
        
        if (!oSMS.SMSText.EndsWith("."))
            oSMS.SMSText = oSMS.SMSText + ".";

        if (hidResendUserName.Value == "0")
            oSMS.DisplayText = GetDisplayText();
        else
            oSMS.DisplayText = hidResendUserName.Value;
        oSMS.To = moHTUsersMobileNo;
        oSMS.ToManualNumbers = moManualMobileNo;
        oSMS.IsScheduled = chkScheduleSMS.Checked;
        if (oSMS.IsScheduled)
            oSMS.ScheduledDate = Convert.ToDateTime(txtPaymentDate.Text + ' ' + txtStartTime.Text);
        else
            oSMS.ScheduledDate = DateTime.Now;

        oSMS.IsUnicodeSMS = false;
        if (hidIsUnicodeSMS.Value == Constants.S_ONE)
            oSMS.IsUnicodeSMS = true;

        if (txtTemplateId.Text.Trim() != string.Empty)
            oSMS.TemplateRegistrationId = txtTemplateId.Text.Trim();

        if(!hidEditedSMSId.Value.IsNullOrEmpty() && hidEditedSMSId.Value != Constants.S_ZERO)
        {
            SMSMasterBL oSMSMasterBL = new SMSMasterBL();
            oSMSMasterBL.DeleteScheduledSMS(hidEditedSMSId.Value.ToInt(),miSchoolId,miAcademicYearId);
        }
        int iCount = oSMS.Send();
        return iCount;
    }

    /// <summary>
    /// This method is used to check if the login user is of superviser role and 
    /// check the access he have
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckPreConditions()
    {
        SchoolUserCollectionBL oSchoolUserCollectionBL = new SchoolUserCollectionBL();
        bool bresult = oSchoolUserCollectionBL.CheckThatIsAnyUserPresent(miSchoolId, Convert.ToInt32(Constants.UserRoles.Admin));
        if (!bresult)
        {
            throw new NoUserOtherThanAdminExceptions("In order to send SMS, you need to add users into system.");
        }
    }

    /// <summary>
    /// This method is used to set SMS count.
    /// </summary>
    private void SetSMSCount()
    {
        SMSMasterBL oSMSMasterBL = new SMSMasterBL();
        DataTable oDtMsgCnt = oSMSMasterBL.GetCountOfSentSMS(miSchoolId, miAcademicYearId);
        lblFreeSMSVal.Text = oDtMsgCnt.Rows[0]["AllowedSMS_Count"].ToString();
        lblSentSMSVal.Text = oDtMsgCnt.Rows[0]["SentSMS_Count"].ToString();
        checkIsSMSCountExceeded();
    }

    /// <summary>
    /// Check that if SMS count exceeded
    /// </summary>
    private void checkIsSMSCountExceeded()
    {
        int iExceededSmsCount = 0;
        int iFreeSmsCount = Convert.ToInt32(lblFreeSMSVal.Text);
        int iSentSmsCount = Convert.ToInt32(lblSentSMSVal.Text);
        if (iSentSmsCount > iFreeSmsCount)
        {
            iExceededSmsCount = iSentSmsCount - iFreeSmsCount;
            lblExceededSmsVal.Text = Convert.ToString(iExceededSmsCount);
            lblExceededSmsVal.Style.Add(HtmlTextWriterStyle.Color, "RED");
        }
    }

    /// <summary>
    /// This method is used to set javascript attribute.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        optSupervisor.Text = Constants.S_SUPERVISOR_ROLE_NAME;
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnClear, btnSendSMS, btnSent, btnRecievedSMS, btnScheduledSMS });
        btnSendSMS.Attributes.Add("onclick", "if(!ConfirmSendMessage()) {return false;}");
        chkScheduleSMS.Attributes.Add("onclick", "ScheduleSMS()");
        txtPaymentDate.Attributes.Add("onchange", "ScheduleSMS()");
        btnBack.PostBackUrl = "SMSHistoryUI.aspx";

        if (!chkManualNumber.Checked)
            txtManualNumbers.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to redirect to same page when SMS get sent.
    /// </summary>
    private void RedirectToPage(int aiCount,string asQueryString)
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        if (chkSendMessage.Checked)
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + asQueryString);
        else
        {
            if(chkScheduleSMS.Checked)
                oMasterPage.RedirectToNextPage("SMSUI.aspx?" + Utility.CommonUtility.EncryptQuerystring("MODE=Scheduled&COUNT=" + Convert.ToInt32(aiCount)));
            else
                oMasterPage.RedirectToNextPage("SMSUI.aspx?" + Utility.CommonUtility.EncryptQuerystring("MODE=Sent&COUNT=" + Convert.ToInt32(aiCount)));
        }
    }

    private string PrepareQueryString(bool abShowAllSentSMS)
    {
        Session["MessageText"] = txtMessage.Text;
        Session["TeacherNameList"] = HidTeacherName.Value;
        Session["TeacheIdList"] = HidTeacherId.Value;
        Session["StudentNameList"] = HidStudentName.Value;
        Session["StudentIdList"] = HidStudentId.Value;     
        Session["AdminstaffNameList"] = HidSupervisorName.Value;
        Session["AdminstaffIdList"] = HidSupervisorId.Value;       
        Session["UserNameList"] = HidUserNames.Value;
        Session["UserIdList"] = hidUserId.Value;        
        Session["IsAdminSelected"] = chkAdmin.Checked;
        Session["IsPrincipleSelected"] = chkPrincipal.Checked;
        Session["IsEntireSchoolSelected"] = optEntireSchool.Checked;
        Session["UserGroupId"] = hidGroupId.Value;
        Session["UserGroupName"] = hidGroupName.Value;
        string sQuerystring = "From=" + "SMSUI" + "&SMSId=" + hidSMSId.Value + "&Access=" + HidState.Value + "&ShowAllSentSMS=" + (abShowAllSentSMS ? Constants.S_ONE : Constants.S_ZERO);       
        sQuerystring = Utility.CommonUtility.EncryptQuerystring(sQuerystring);      
        return sQuerystring;
    }
    

    /// <summary>
    /// This method is used to reset all page controls.
    /// </summary>
    private void ResetAllControls()
    {
        lblFrom.Text = "";
        txtFrom.Text = "";
        txtFromMb.Text = "";
        txtMessage.Text = "";
        txtToUserId.Text = "";
        txtUser.Text = "";
        hidUserId.Value = "";
        HidUserNames.Value = "";
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>
    private void GetQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString().Length > 1)
            {    
                HidBackUrl.Value = Server.UrlDecode(Request.QueryString.ToString());
                if(!QueryString["From"].IsNull())
                    msForm = QueryString["From"];
                if (!QueryString["SMSId"].IsNull() && QueryString["SMSId"] != String.Empty)
                    hidSMSId.Value = QueryString["SMSId"];                
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to insert sms receiver details.
    /// </summary>
    private int PrepareReceiversList()
    {
        int iSMSCount = BuildReceiptantsObjects();
        if (chkManualNumber.Checked)
            foreach (string sMobileNo in txtManualNumbers.Text.Trim().Split(','))
                if (sMobileNo.Trim() != string.Empty && !moManualMobileNo.ContainsKey(sMobileNo.Trim()))
                    moManualMobileNo[sMobileNo.Trim()] = sMobileNo.Trim();
       if(String.IsNullOrEmpty(HidParentTeacherAssociationId.Value))
       {
        if (chkAdmin.Checked) 
            moHTUsersMobileNo[Convert.ToInt32(HidAdminUserID.Value)] = hidAdminMbNo.Value;

	       if (chkPrincipal.Checked)
	       {
			   HidId.Value = ";" + HidPrincipalUserID.Value + ";";
			   if (!hidUserId.Value.Contains(HidId.Value))
				   hidUserId.Value = hidUserId.Value + ";" + HidPrincipalUserID.Value;
			   if (hidUserId.Value.Trim().StartsWith(";"))
				   hidUserId.Value = hidUserId.Value.Substring(hidUserId.Value.IndexOf(';') + 1, hidUserId.Value.Length - 1);
	       }
       }
       else
       {
           if (chkAdmin.Checked && !moHTUsersMobileNo.ContainsKey(Convert.ToInt32(HidAdminUserID.Value)))
               moHTUsersMobileNo[HidAdminUserID.Value] = hidAdminMbNo.Value;

           if (chkPrincipal.Checked && !moHTUsersMobileNo.ContainsKey(Convert.ToInt32(HidPrincipalUserID.Value)))
               moHTUsersMobileNo[HidPrincipalUserID.Value] = hidPrincipalMbNo.Value;
       }
       return iSMSCount;
    }

    /// <summary>
    /// This method is used to insert sms Details into database by constructing a sms master objects.
    /// </summary>
    private string GetDisplayText()
    {
        string DisplayText = string.Empty;
        if (chkManualNumber.Checked == true)
        {
            SMSMasterBL oSMSMasterBL = new SMSMasterBL();
            oSMSMasterBL.ValidateMobileNos(txtManualNumbers.Text.Trim());
        }
        if (optEntireSchool.Checked == true)
            DisplayText = !chkManualNumber.Checked ? Constants.S_ENTIRE_SCHOOL : Constants.S_ENTIRE_SCHOOL + "," + txtManualNumbers.Text.Trim();
             else if (((chkManualNumber.Checked == false) && (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor ||
                        moUserRole == Constants.UserRoles.Teacher) && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES) ||
                                (HidUserNames.Value.Split(',').Length != hidUserId.Value.Split(';').Length))))
        {
            DisplayText = GetUserNameList();
        }
        else
            DisplayText = HidUserNames.Value + (HidUserNames.Value.Trim() != string.Empty && txtManualNumbers.Text.Trim() != string.Empty ? "," + txtManualNumbers.Text.Trim() : txtManualNumbers.Text.Trim());

        return DisplayText;
    }

  
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string GetUserNameList()
    {
        string sReturn = HidTeacherName.Value;

        if (chkPrincipal.Checked && !sReturn.Contains(HidPrincipleName.Value) && !HidParentTeacherAssociationName.Value.Contains(HidPrincipleName.Value.Substring(0, HidPrincipleName.Value.IndexOf("("))))
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidPrincipleName.Value : HidPrincipleName.Value;

        if (chkAdmin.Checked && !sReturn.Contains(HidAdminUserName.Value) && !HidParentTeacherAssociationName.Value.Contains(HidAdminUserName.Value.Substring(0, HidAdminUserName.Value.IndexOf("("))))
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidAdminUserName.Value : HidAdminUserName.Value;

        if (HidStdDivName.Value != Constants.S_EMPTY_STRING)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidStdDivName.Value : HidStdDivName.Value;

        if (HidStudentName.Value != Constants.S_EMPTY_STRING)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidStudentName.Value : HidStudentName.Value;

        if (HidSupervisorName.Value != Constants.S_EMPTY_STRING)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidSupervisorName.Value : HidSupervisorName.Value;

        if (HidOtherStaffName.Value != Constants.S_EMPTY_STRING)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidOtherStaffName.Value : HidOtherStaffName.Value;

        if (HidParentTeacherAssociationName.Value != Constants.S_EMPTY_STRING)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + HidParentTeacherAssociationName.Value : HidParentTeacherAssociationName.Value;

        if(hidGroupId.Value != string.Empty)
            sReturn = !String.IsNullOrEmpty(sReturn) ? sReturn + ", " + hidGroupName.Value : hidGroupName.Value;

        if (sReturn.Trim().EndsWith(","))
            sReturn = sReturn.Substring(0, sReturn.LastIndexOf(','));

        return sReturn;

    }

    /// <summary>
    /// This method is used to fill mobile number's hashtable for sending sms to them
    /// </summary>
    private int PopulateStudMobileNos()
    {
        //This function is used to get the list of the Users.        
        string SToUserList = string.Empty;
        int iValue = Constants.I_ZERO;
        if (!HidStdDivId.Value.Equals(string.Empty))
        {
            string sArrStdDivIds = HidStdDivId.Value.Replace(';', ',');
            SToUserList += RetrieveStdDivStudMobileNos(sArrStdDivIds);
            string[] arrMobile = SToUserList.Split(';');

            if (SToUserList.Trim().EndsWith(";"))
            {
                hidUserId.Value = SToUserList.Substring(0, SToUserList.LastIndexOf(';'));
                iValue = arrMobile.Length - 1;
            }
            else
                iValue = arrMobile.Length;
        }

        if (!String.IsNullOrEmpty(HidStudentId.Value))
        {
            SToUserList = RetrieveStudMobileNos(HidStudentId.Value);
            string[] arrMobile = SToUserList.Split(';');

            if (SToUserList.Trim().EndsWith(";"))
            {
                hidUserId.Value = SToUserList.Substring(0, SToUserList.LastIndexOf(';'));
                iValue = arrMobile.Length - 1;
            }
            else
                iValue = arrMobile.Length;
        }
        return iValue;
    }

    /// <summary>
    /// This method is used to set mobile nubmbers hashtable with teacher's.
    /// </summary>
    private int SetToTeachersIdList()
    {
        int iValue = Constants.I_ZERO;
        //This function is used to get the list of the Users.        
        string sUserIds = HidTeacherId.Value.Replace(";", ",");
        if (sUserIds.StartsWith(","))
            sUserIds = sUserIds.Substring(1);
        return iValue = SetTeachersMobileNostoHashTable(sUserIds);

    }

    /// <summary>
    /// This method is used to set mobile nubmbers hashtable with Parent and teacher's.
    /// </summary>
    private int SetToParentTeacherAssociationIdList()
    {
        //This function is used to get the list of the Users.  
        int iCount = Constants.I_ZERO;
        string sUserIds = HidParentTeacherAssociationId.Value.Replace(";", ",");
        return iCount = SetParentTeacherMobileNostoHashTable(sUserIds);

    }

    /// <summary>
    /// This method is used to build sms receiptants objects.
    /// </summary>
    private int BuildReceiptantsObjects()
    {
        int iSMSCount = Constants.I_ZERO;
        moHTUsersMobileNo.Clear();
         if (moUserRole == Constants.UserRoles.Admin ||
           ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.OtherStaff)
               && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))        
        {
            if (optEntireSchool.Checked)
                iSMSCount = SetToEntireSchool();
            else
            {
                if (!String.IsNullOrEmpty(HidStdDivId.Value) || !String.IsNullOrEmpty(HidStudentId.Value))
                    iSMSCount = PopulateStudMobileNos();
                if (!String.IsNullOrEmpty(HidTeacherId.Value))
                    iSMSCount = SetToTeachersIdList();
                if (!String.IsNullOrEmpty(HidSupervisorId.Value))
                    iSMSCount = SetToSupervisorIdList();
                if (!String.IsNullOrEmpty(HidOtherStaffId.Value))
                    iSMSCount = SetToOtherStaffIdIdList();
                if (!String.IsNullOrEmpty(HidParentTeacherAssociationId.Value))
                    iSMSCount = SetToParentTeacherAssociationIdList();

                if (!hidGroupId.Value.IsNullOrEmpty() && hidGroupId.Value != Constants.S_ZERO)
                {
                   SetGroupDetails();
                }
            }

        }
         return iSMSCount;
    }

    /// <summary>
    /// This function is used to get the list of the Users.        
    /// </summary>
    private void SetGroupDetails()
    {
        MailingGroupBL oMailingGroupBL = new MailingGroupBL(miSchoolId, miAcademicYearId, miUserId);
        string sUserIds = oMailingGroupBL.GetMailingGroupUsers(hidGroupId.Value, false);
        if (!sUserIds.IsNullOrEmpty())
            sUserIds = sUserIds.Replace(";", ",");
        else
            return;

        SetTeachersMobileNostoHashTable(sUserIds);
        SetSupervisorMobileNostoHashTable(sUserIds);
        SetStudentsMobileNostoHashTable(sUserIds);
        string []sArrUserId = sUserIds.Split(',');
        foreach (string sId in sArrUserId)
        {
            if (sId == HidAdminUserID.Value)
            {
                moHTUsersMobileNo[Convert.ToInt32(HidAdminUserID.Value)] = hidAdminMbNo.Value;
                break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private int SetToEntireSchool()
    {
        int iCount = Constants.I_ZERO;
        int iCount1 = Constants.I_ZERO;
        int iCount2 = Constants.I_ZERO;
        int iCount3 = Constants.I_ZERO;
        string sValue = RetrieveStudMobileNos();
        string[] Arr = sValue.Split(';');
        iCount = Arr.Length;
        iCount1 = SetTeachersMobileNostoHashTable(String.Empty);
        iCount2 = SetSupervisorMobileNostoHashTable(String.Empty);
        iCount3 = SetOtherStaffMobileNostoHashTable(String.Empty);     
        moHTUsersMobileNo[Convert.ToInt32(HidAdminUserID.Value)] = hidAdminMbNo.Value;

        return iCount + iCount1 + iCount2 + iCount3 + 1;
    }

    /// <summary>
    /// This method is used to set Teacher's mobile no's to hashtable
    /// </summary>
    /// <param name="iStdDiv"></param>
    /// <returns></returns>
    private int SetTeachersMobileNostoHashTable(String sUserIds)
    {
        DataTable oDtUserID = RetriveTeachersMobileNosDs(sUserIds);
        int iValue = oDtUserID.Rows.Count;
        if ((oDtUserID.Rows != null) && oDtUserID.Rows.Count >= 1)
        {
            foreach (DataRow oDataRow in oDtUserID.Rows)
            {
                if ((oDataRow["Mobile_Number"] != DBNull.Value) &&
                    (Convert.ToString(oDataRow["Mobile_Number"]).Length >= 10))
                {
                    moHTUsersMobileNo[Convert.ToInt32(oDataRow["User_Id"])] = Convert.ToString(oDataRow["Mobile_Number"]);
                }
            }
        }

        return iValue;
    }

    /// <summary>
    /// This method is used to set Teacher's mobile no's to hashtable
    /// </summary>
    /// <param name="iStdDiv"></param>
    /// <returns></returns>
    private int SetParentTeacherMobileNostoHashTable(String sUserIds)
    {
        DataTable oDtUserID = RetriveParentTeachersMobileNosDs(sUserIds);
        int iCount = oDtUserID.Rows.Count;
        string sUserId;
        if ((oDtUserID.Rows != null) && oDtUserID.Rows.Count >= 1)
        {
            foreach (DataRow oDataRow in oDtUserID.Rows)
            {
                sUserId = Convert.ToString(oDataRow["User_Id"]);
                if ((oDataRow["Mobile_Number"] != DBNull.Value) &&
                    (Convert.ToString(oDataRow["Mobile_Number"]).Length >= 10))
                {
                    if (moHTUsersMobileNo.ContainsKey(sUserId) && !moHTUsersMobileNo.ContainsKey(sUserId + "sm;"))
                    {
                        sUserId = sUserId + "sm;";
                        moHTUsersMobileNo[sUserId] = (oDataRow["Mobile_Number"]);
                    }
                    else if (!moHTUsersMobileNo.ContainsKey(sUserId))                
                        moHTUsersMobileNo[Convert.ToInt32(sUserId)] = (oDataRow["Mobile_Number"]);                
                }
                if (!(oDataRow["Mobile_Number2"]).ToString().IsNullOrEmpty() && (Convert.ToString(oDataRow["Mobile_Number2"]).Length >= 10))
                    moHTUsersMobileNo[sUserId + "sm;"] = (oDataRow["Mobile_Number2"]);
                
            }
        }
        return iCount;
    }

    /// <summary>
    /// This method is used to retrive Teachers dataset
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <returns></returns>
    private DataTable RetriveTeachersMobileNosDs(String sUserIds)
    {
        DataTable oDtUserID;
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        if (sUserIds != String.Empty)
            oDtUserID = oMasterDataCollectionBL.GetAllMobileNosForGivenTeacherUserID(miSchoolId, sUserIds, miAcademicYearId);
        else
            oDtUserID = oMasterDataCollectionBL.GetAllMobileNosForTeachers(miSchoolId, miAcademicYearId);

        return oDtUserID;
    }
    /// <summary>
    /// This method is used to retrive ParentTeacherAssociation dataset
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <returns></returns>
    private DataTable RetriveParentTeachersMobileNosDs(String sUserIds)
    {
        DataTable oDtUserID;
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        if (sUserIds != String.Empty)
            oDtUserID = oParentTeacherAssociationDetailsBL.GetAllMobileNosForParentTeachers(sUserIds);
        else
            oDtUserID = oParentTeacherAssociationDetailsBL.GetAllMobileNosForParentTeachers();

        return oDtUserID;
    }

    /// <summary>
    /// This method is used to set Supervisor's mobile no's to hashtable
    /// </summary>
    /// <param name="iStdDiv"></param>
    /// <returns></returns>
    private int SetSupervisorMobileNostoHashTable(String sUserIds)
    {
        string SToUserList = String.Empty;
        DataTable oDSUserID = RetriveSupervisorMobileNosDs(sUserIds);
        int iValue = oDSUserID.Rows.Count;
        if ((oDSUserID != null))
        {
            foreach (DataRow oDataRow in oDSUserID.Rows)
            {
                if ((oDataRow["Mobile_Number"] != DBNull.Value) &&
                    (Convert.ToString(oDataRow["Mobile_Number"]).Length >= 10))
                {
                    moHTUsersMobileNo[Convert.ToInt32(oDataRow["ID"])] = Convert.ToString(oDataRow["Mobile_Number"]);
                }
            }
        }

        return iValue;
    }

    /// <summary>
    /// This method is used for Set student mobile nos to hashtable.
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <returns></returns>
    private int SetStudentsMobileNostoHashTable(string sUserIds)
    {
        string SToUserList = String.Empty;
        DataTable oDSUserID = RetriveStudentMobileNosDs(sUserIds);
        int iValue = oDSUserID.Rows.Count;
        if ((oDSUserID != null))
        {
            foreach (DataRow oDataRow in oDSUserID.Rows)
            {
                if ((oDataRow["Mobile_Number"] != DBNull.Value) &&
                    (Convert.ToString(oDataRow["Mobile_Number"]).Length >= 10))
                {
                    moHTUsersMobileNo[Convert.ToInt32(oDataRow["User_Id"])] = Convert.ToString(oDataRow["Mobile_Number"]);
                }
            }
        }

        return iValue;
    }


    private int SetOtherStaffMobileNostoHashTable(String sUserIds)
    {
        string SToUserList = String.Empty;
        DataTable oDSUserID = RetriveOtherStaffMobileNosDs(sUserIds);
        int iCount = oDSUserID.Rows.Count;
        if ((oDSUserID != null))
        {
            foreach (DataRow oDataRow in oDSUserID.Rows)
            {
                if ((oDataRow["Mobile_Number"] != DBNull.Value) &&
                    (Convert.ToString(oDataRow["Mobile_Number"]).Length >= 10))
                {
                    moHTUsersMobileNo[Convert.ToInt32(oDataRow["ID"])] = Convert.ToString(oDataRow["Mobile_Number"]);
                }
            }
        }

        return iCount;
    }


    /// <summary>
    /// This method is used to retrive Supervisor dataset
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <returns></returns>
    private DataTable RetriveSupervisorMobileNosDs(String sUserIds)
    {
        DataTable oDttUserID;

        if (sUserIds != String.Empty)
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.GetAllMobileNosForGivenSupervisorUserID(miSchoolId, sUserIds, miAcademicYearId);
        else
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.GetSupervisorDetailsForMsging(miSchoolId, miAcademicYearId, moUserRole);
        return oDttUserID;
    }


    /// <summary>
    /// This method is used to retrive student dataset.
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <returns></returns>
    private DataTable RetriveStudentMobileNosDs(string sUserIds)
    {
        DataTable oDttUserID = new DataTable();

        if (sUserIds != string.Empty)
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.GetAllMobileNosForGivenStudentUserIds(sUserIds);
        else
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.GetStudentDetailsForMsging();
        return oDttUserID;
    }

    private DataTable RetriveOtherStaffMobileNosDs(String sUserIds)
    {
        DataTable oDttUserID;

        if (sUserIds != String.Empty)
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.GetAllMobileNosForGivenOtherStaffUserID(miSchoolId, sUserIds, miAcademicYearId);
        else
            oDttUserID = SchoolWiseSupervisorMasterCollectionBL.FetchSchoolWiseOtherStaffMasterDetails(miSchoolId, miAcademicYearId);
        return oDttUserID;
    }


    /// <summary>
    /// This method is used to set students mobile no's to hashtable
    /// </summary>
    /// <param name="iStdDiv"></param>
    /// <returns></returns>
    private string RetrieveStudMobileNos()
    {
        string SToUserList = String.Empty;
        DataTable oDtUserID = RetriveStudMobileNosDs();
        String iUserId;
        for (int jCount = 0; jCount < oDtUserID.Rows.Count; jCount++)
        {
            if ((oDtUserID.Rows[jCount]["Mobile_Number"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + ";";
                moHTUsersMobileNo[iUserId] = oDtUserID.Rows[jCount]["Mobile_Number"];
            }
            if ((oDtUserID.Rows[jCount]["Mobile_Number2"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number2"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + "sm;";
                moHTUsersMobileNo[iUserId + "sm;"] = oDtUserID.Rows[jCount]["Mobile_Number2"];
            }
        }
        return SToUserList;
    }

    /// <summary>
    /// This method is used to set students mobile no's to hashtable
    /// </summary>
    /// <param name="iStdDiv"></param>
    /// <returns></returns>
    private string RetrieveStdDivStudMobileNos(string sArrStdDiv)
    {
        string SToUserList = String.Empty;
        String iUserId;

        DataTable oDtUserID = StudentBL.GetAllStudentsByGivenStdDivs(miSchoolId, miAcademicYearId, sArrStdDiv, optLeftStudents.Checked);
        for (int jCount = 0; jCount < oDtUserID.Rows.Count; jCount++)
        {
            if ((oDtUserID.Rows[jCount]["Mobile_Number"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + ";";
                moHTUsersMobileNo[iUserId] = oDtUserID.Rows[jCount]["Mobile_Number"];
            }
            if ((oDtUserID.Rows[jCount]["Mobile_Number2"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number2"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + "sm;";
                moHTUsersMobileNo[iUserId + "sm;"] = oDtUserID.Rows[jCount]["Mobile_Number2"];
            }
        }
        return SToUserList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="asUserIDs"></param>
    /// <returns></returns>
    private string RetrieveStudMobileNos(string asUserIDs)
    {
        string SToUserList = String.Empty;
        DataTable oDtUserID = RetriveStudMobileNosDs(asUserIDs);
        String iUserId;
        for (int jCount = 0; jCount < oDtUserID.Rows.Count; jCount++)
        {
            if ((oDtUserID.Rows[jCount]["Mobile_Number"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + ";";
                moHTUsersMobileNo[iUserId] = oDtUserID.Rows[jCount]["Mobile_Number"];
            }
            if ((oDtUserID.Rows[jCount]["Mobile_Number2"] != DBNull.Value) &&
                Convert.ToString(oDtUserID.Rows[jCount]["Mobile_Number2"]).Length >= 10)
            {
                iUserId = Convert.ToString(oDtUserID.Rows[jCount]["Id"]);
                SToUserList += iUserId + "sm;";
                moHTUsersMobileNo[iUserId + "sm;"] = oDtUserID.Rows[jCount]["Mobile_Number2"];
            }
        }

        return SToUserList;
    }

    /// <summary>
    /// This method is used to retrive students dataset for given division ID
    /// </summary>
    /// <returns></returns>
    private DataTable RetriveStudMobileNosDs()
    {
        DataTable oDtUserID;
        oDtUserID = StudentBL.GetAllStudentsForMessageFacillity(miSchoolId, miAcademicYearId);
        return oDtUserID;
    }

    /// <summary>
    /// This method is used to retrive students dataset for given division ID
    /// </summary>
    /// <returns></returns>
    private DataTable RetriveStudMobileNosDs(string asUserIds)
    {
        DataTable oDtUserID = null;
        if (asUserIds != String.Empty)
            oDtUserID = StudentBL.GetAllStudentsByStdDivForMessageFacillity(miSchoolId, asUserIds.Replace(";", ","), miAcademicYearId);

        return oDtUserID;
    }

    /// <summary>
    /// This method is used to set mobile numbers hashtable with Supervisor details
    /// </summary>
    private int SetToSupervisorIdList()
    {
        int iValue = Constants.I_ZERO;
        string sUserIds = HidSupervisorId.Value.Replace(";", ",");
        iValue = SetSupervisorMobileNostoHashTable(sUserIds);

        return iValue;
    }
    private int SetToOtherStaffIdIdList()
    {
        int iCount = Constants.I_ZERO;
        string sUserIds = HidOtherStaffId.Value.Replace(";", ",");
        return iCount = SetOtherStaffMobileNostoHashTable(sUserIds);
    }

    /// <summary>
    /// This method is used to set defauks values and properties to page controls.
    /// </summary>
    private void SetDefaultPageControls()
    {
        txtManualNumbers.Enabled = false;
        txtManualNumbers.Attributes.Add("onkeyup", "extractPhNumbers(this)");
        txtManualNumbers.Attributes.Add("onkeypress", "return blockNonPhNumbers (this, event);");
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        txtFrom.Text = oSchoolBL.SMSSenderName;
        txtFromMb.Text = oSchoolBL.SMSSenderName;
         if (moUserRole == Constants.UserRoles.Admin ||
           ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
               && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))        
        {
            //check if we redirect here from Pending Fee Student List
            if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("Fee"))
                PendingFeeStudents();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("InternalFee"))
                PendingInternalFeeStudents();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("StudentPayables"))
               FeesPayableStudents();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("CopyFeeConfiguration"))
                FeesPayableStudentsForStds();
            else if (msForm != null && msForm != Constants.S_EMPTY_STRING && msForm.Equals("ResendSMS"))
                SetFields();
            else
            {
                txtToUserId.Text = string.Empty;
                optTeachers.Checked = true;
                hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Teacher" + "&sUserId=" + hidUserId.Value);
            }
        }

        txtMessage.Attributes.Add("onFocus", "poof3(this,'Type your SMS here...');");
        chkAdmin.Attributes.Add("onclick", "SetControlsForAdminDetails('Admin');");
        chkPrincipal.Attributes.Add("onclick", "SetControlsForAdminDetails('Principal');");
        optStudents.Attributes.Add("onclick", "SetControlsForAdminDetails('Student')");
        optTeachers.Attributes.Add("onclick", "SetControlsForAdminDetails('Teacher')");
        optSupervisor.Attributes.Add("onclick", "SetControlsForAdminDetails('Supervisor')");
        optOtherStaff.Attributes.Add("onclick", "SetControlsForAdminDetails('OtherStaff')");
        optParentTeacherAssociation.Attributes.Add("onclick", "SetControlsForAdminDetails('ParentTeacherAssociation')");
        optEntireSchool.Attributes.Add("onclick", "SetControlsForAdminDetails('EntireSchool')");
        btnClear.Attributes.Add("onclick", "return ClearTextFields();return false;");
        txtMessage.Attributes.Add("onkeyup", "alertMsgLength(event);");
        txtMessage.Attributes.Add("onblur", "poof3(this,'Type your SMS here...');");
        txtManualNumbers.Attributes.Add("onfocus", "fnTXTFocus('" + txtManualNumbers.ClientID + "')");
        txtManualNumbers.Attributes.Add("onblur", "fnTXTLostFocus('" + txtManualNumbers.ClientID + "')");
        SetControlsForViewMode();
        HidUserType.Value = moUserRole.ToString();
        FetchAdminUserId();
    }

    private void FeesPayableStudents()
    {
        int iStdId = Convert.ToInt32(QueryString["StandardId"]);
        int iDivId = Convert.ToInt32(QueryString["DivisionId"]);
        string sRegNo = Convert.ToString(QueryString["RegNo"]);
        string sDuedate = Convert.ToString(QueryString["DueDate"]);
        string sFeeType = Convert.ToString(QueryString["FeeType"]);
        string sPayableFor = Convert.ToString(QueryString["PayableFor"]);
        string sAmount = Convert.ToString(QueryString["Amount"]);
        bool bConsiderForRTEConcession = QueryString["ConsiderForRTEConcession"].ToBool();
        chkSendMessage.Checked = !string.IsNullOrEmpty(QueryString["SendMsg"])
                                && QueryString["SendMsg"].Equals(Constants.S_YES);
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

        if (!string.IsNullOrEmpty(sStudentName))
        {
            sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
            sUserId = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        }

        HidStudentId.Value = sUserId;
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        optStudents.Checked = true;
        txtMessage.Text = sSmsText;
        Session["MessageType"] = "Fee Updates";
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
        txtTemplateId.Text = Convert.ToString(oDTTemplate.Rows[0][4]);   //
    }

    private void FeesPayableStudentsForStds()
    {
        string sStdId = Convert.ToString(QueryString["StandardId"]);       
        string sDuedate = Convert.ToString(QueryString["DueDate"]);
        string sFeeType = Convert.ToString(QueryString["FeeType"]);
        string sPayableFor = Convert.ToString(QueryString["PayableFor"]);
        string sAmount = Convert.ToString(QueryString["Amount"]);
        bool bConsiderForRTEConcession = QueryString["ConsiderForRTEConcession"].ToBool();
        chkSendMessage.Checked = !string.IsNullOrEmpty(QueryString["SendMsg"])
                                && QueryString["SendMsg"].Equals(Constants.S_YES);
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
        txtMessage.Text = sSmsText;
        Session["MessageType"] = "Fee Updates";
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
        txtTemplateId.Text = Convert.ToString(oDTTemplate.Rows[0][4]);   //
    }

    /// <summary>
    /// This method is used to get students to whome message/sms will be sent.
    /// </summary>
    /// <param name="IsNew"></param>
    private List<StudentInfo> GetStudentList(int aiStdId, int aiDivId, string asRegNo, bool bConsiderForRTEConcession)
    {
        Hashtable oHashTable = new Hashtable();
        List<StudentInfo> oStudentList = StudentBL.GetStudentsForFeesUpdate(miSchoolId, miAcademicYearId, aiStdId, aiDivId, asRegNo, bConsiderForRTEConcession);
        return oStudentList;
    }

    /// <summary>
    /// 
    /// </summary>
    private void FetchAdminUserId()
    {
        //This function is used to set the Admin User Id's in the Hidden field.
        DataSet oDataSet = SchoolUserCollectionBL.GetAdminAndprincipalOfSchool(miSchoolId, miAcademicYearId, miUserId);
        HidAdminUserID.Value = oDataSet.Tables[0].Rows[0]["User_Id"].ToString();
        HidAdminUserName.Value = oDataSet.Tables[0].Rows[0]["username"].ToString();
        hidAdminMbNo.Value = oDataSet.Tables[0].Rows[0]["Mobile_Number"].ToString();

        if (oDataSet.Tables.Count > 1 && oDataSet.Tables[1].Rows.Count > 0)
        {            
            HidPrincipalUserID.Value = oDataSet.Tables[1].Rows[0]["User_Id"].ToString();
            HidPrincipleName.Value = oDataSet.Tables[1].Rows[0]["username"].ToString();
            hidPrincipalMbNo.Value = oDataSet.Tables[1].Rows[0]["Mobile_Number"].ToString();
        }
        else
        {
            chkPrincipal.Visible = false;
            chkPrincipal.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to check and set view mode for the screen
    /// </summary>
    private void SetControlsForViewMode()
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
	    if (moUserRole != Constants.UserRoles.Admin && QueryString.Count == 0 &&
				!((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher) && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
		    oMasterPage.RedirectToNextPage("SMSHistoryUI.aspx");
	    
		if (QueryString.Count > 0 && !QueryString["MODE"].IsNull())
        {
            if (QueryString["MODE"] == "View")
            {
                txtFrom.ReadOnly = true;
                btnSendSMS.Enabled = false;
                trReceivedDate.Visible = true;
                TDtxtCount.Visible = false;
                AddAtributeToBack("SMSHistoryUI.aspx?" + HidBackUrl.Value);
                btnSendSMS.Visible = false;
                btnClear.Visible = false;
                trMobileDisplay.Visible = false;
                tblNoteData.Visible = false;
                trPlaneDisplay.Visible = true;                
                if (moUserRole != Constants.UserRoles.Admin || !((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                    && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                {
                    oMasterPage.SetCurrentNodeText("View SMS", Convert.ToInt32(moUserRole), miSchoolId);
                    if (QueryString["Access"] == "3")
                        lblReceivedDate.Text = "Scheduled Date :";
                    else if (QueryString["Access"] == "1")
                        lblReceivedDate.Text = "Sent Date :";
                    else
                        lblReceivedDate.Text = "Received Date :";
                }

                oMasterPage.SetCurrentNodeText("View SMS", Convert.ToInt32(moUserRole), miSchoolId);
                SetSMSDetails();
            }
            else if (QueryString["MODE"] == "Sent")
            {
                if (Convert.ToInt32(QueryString["COUNT"]) == 0)
                    lblMessage.Text = "There is no user to sent SMS.";
                lblMessage.Text = "SMS sent successfully to " + QueryString["COUNT"] + " user(s).";
                MsgLbl.Visible = true;
            }
            else if (QueryString["MODE"] == "Scheduled")
            {
                if (Convert.ToInt32(QueryString["COUNT"]) == 0)
                    lblMessage.Text = "There is no user to schedule SMS.";                
                lblMessage.Text = "SMS scheduled successfully to " + QueryString["COUNT"] + " user(s).";               
                MsgLbl.Visible = true;
            }
            
        }
        else
        {
            oMasterPage.SetCurrentNodeText("SMS Center", Convert.ToInt32(moUserRole), miSchoolId);
              if (moUserRole == Constants.UserRoles.Admin ||((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
               && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                AddAtributeToBack("../Common/ControlPanel.aspx");
        }
    }

    /// <summary>
    /// This method is used to set navigation url to Back button.
    /// </summary>
    private void AddAtributeToBack(String asUrl)
    {
        btnBack.Attributes.Add("onclick", "window.open('" + asUrl + "' , '_self').focus(); return false;");
    }

    /// <summary>
    /// This method is used to display message details of selected message.
    /// </summary>
    private void SetSMSDetails()
    {
        int iSMSId = Convert.ToInt32(QueryString["SMSId"]);
        SMSMasterBL oSMSBL = new SMSMasterBL(iSMSId);
        txtFrom.Text = oSMSBL.Sender_Name;
        txtFromMb.Text = oSMSBL.Sender_Name;
        txtReceivedDate.Text = oSMSBL.Insert_Date.DayOfWeek
                               + ", " + oSMSBL.Insert_Date.ToString(Constants.S_STANDARD_DATE_FORMAT)
                               + " " + oSMSBL.Insert_Date.ToShortTimeString();
        txtMessage.Text = oSMSBL.SMS_Text;
        txtShowSMS.Text = oSMSBL.SMS_Text;
        trMandatoryMark.Visible = false;
        RetrieveReceiverList();
    }

    /// <summary>
    /// This methid is used to get receiver user ID
    /// </summary>
    private void RetrieveReceiverList()
    {
        int iSMS_Id = Convert.ToInt32(QueryString["SMSId"]);
        string sStudentName = string.Empty;
        string sUserStudentName = string.Empty;
        string sStudentNameList = string.Empty;
        SMSMasterBL oSMSMasterBL = new SMSMasterBL(iSMS_Id);
        DataTable oDtReceiverList = oSMSMasterBL.GetListOfReceiverName();

        for (int iRecordCount = 0; iRecordCount < oDtReceiverList.Rows.Count; iRecordCount++)
        {
            if (!txtToUserId.Text.Contains(oDtReceiverList.Rows[iRecordCount]["UserName"].ToString()))
                txtToUserId.Text += oDtReceiverList.Rows[iRecordCount]["UserName"].ToString() + ", ";
            if (!txtUser.Text.Contains(oDtReceiverList.Rows[iRecordCount]["UserName"].ToString()))
            {
                txtUser.Text += oDtReceiverList.Rows[iRecordCount]["UserName"].ToString() + ", ";
                sStudentNameList += oDtReceiverList.Rows[iRecordCount]["UserName"].ToString() + ", ";
            }
        }
        if (txtToUserId.Text.Length > 2)
        {
            txtToUserId.Text = txtToUserId.Text.Remove(txtToUserId.Text.Length - 2);
            txtUser.Text = txtUser.Text.Remove(txtUser.Text.Length - 2);
        }
        if (moUserRole == Constants.UserRoles.Student)
        {
            sUserStudentName = MessageDetailsBL.GetUserName(miUserId, Convert.ToInt32(moUserRole), miAcademicYearId);
            sStudentName = sUserStudentName.Replace(" ", "");
            sStudentNameList = sStudentNameList.Replace(" ", "");
            if (sStudentNameList.Contains(sStudentName))
                txtUser.Text = sUserStudentName;
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
        string sRegNo = Convert.ToString(QueryString["sRegNo"]);
        string sDuedate = Convert.ToString(QueryString["DueDate"]);
        bool bLfetStudent = Convert.ToBoolean(QueryString["bLeftStudent"]);
        bool bPDCStudent = Convert.ToBoolean(QueryString["bPDCStudent"]);
        int iFeeTypeId = Convert.ToInt32(QueryString["FeeTypeId"]);
        string sPayableFor = Convert.ToString(QueryString["PayableFor"]);
        string sOperator = Convert.ToString(QueryString["Operator"]);
        int iAmount = Convert.ToInt32(QueryString["Amount"]);
		string sPercentFilter = QueryString["PercentFilter"];
        string sStudentName = "";
        string sUserId = "";
        int iSmsId = Convert.ToInt32(QueryString["SMSId"]);
		DataTable OdtStudDetail = StudentBL.GetPendingFeeStudentList(miSchoolId, miAcademicYearId, iStdId, iDivId, sRegNo, sDuedate, bLfetStudent, bPDCStudent, iFeeTypeId, sPayableFor, sOperator, iAmount, Constants.S_EMPTY_STRING, Constants.I_ZERO, Constants.I_ZERO, sPercentFilter);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);

        for (int iCount = 0; iCount < OdtStudDetail.Rows.Count; iCount++)
        {
            sUserId += OdtStudDetail.Rows[iCount]["User_Id"].ToString() + "; ";
            sStudentName += OdtStudDetail.Rows[iCount]["SMSName"].ToString() + ", ";
        }

        sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
        HidStudentId.Value = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        optStudents.Checked = true;
        txtMessage.Text = Convert.ToString(oDTTemplate.Rows[0][2]);
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
        txtTemplateId.Text = Convert.ToString(oDTTemplate.Rows[0][4]);   //
    }

    private void PendingInternalFeeStudents()
    {
        string sFeeTypeID = Convert.ToString(QueryString["FeeTypeID"]);
        string sRegNo = Convert.ToString(QueryString["RegNo"]);
        string sFromDate = Convert.ToString(QueryString["FromDate"]);
        string sToDate = Convert.ToString(QueryString["ToDate"]);
        bool bIncludePaid = Convert.ToBoolean(QueryString["IncludePaid"]);
        bool bPayForNextYear = Convert.ToBoolean(QueryString["PayForNextYear"]);
        bool bIsRegNoFilter = Convert.ToBoolean(QueryString["IsRegNoFilter"]);
        string sStudentName = "";
        string sUserId = "";
        DataTable OdtStudDetail = InternalFeeDetailsBL.GetInternalFeesStudentForSMS(miSchoolId, miAcademicYearId, sRegNo, bIncludePaid, bPayForNextYear, sFromDate, sToDate, sFeeTypeID);

        for (int iCount = 0; iCount < OdtStudDetail.Rows.Count; iCount++)
        {
            sUserId += OdtStudDetail.Rows[iCount]["User_Id"].ToString() + "; ";
            sStudentName += OdtStudDetail.Rows[iCount]["SMSName"].ToString() + ", ";
        }

        sStudentName = sStudentName.Substring(0, sStudentName.LastIndexOf(','));
        HidStudentId.Value = sUserId.Substring(0, sUserId.LastIndexOf(';'));
        txtToUserId.Text = sStudentName;
        HidStudentName.Value = sStudentName;
        optStudents.Checked = true;
        hidQry.Value = CommonUtility.EncryptQuerystring("Mode=SMS&UsersList=Student" + "&sUserId=" + hidUserId.Value);
    }

    /// <summary>
    /// This method is used to clear send sms session variable.
    /// </summary>
    private void ClearSMSSessionVariable()
    {
        if (Session[Constants.S_SESSION_IS_SENT_SMS_LIST] != null)
            Session.Remove(Constants.S_SESSION_IS_SENT_SMS_LIST);
    }

    /// <summary>
    /// This method is used to reset the control in resend mode.
    /// </summary>
    public void SetFields()
    {
        int iSMS_Id = Convert.ToInt32(QueryString["SMSId"]);
        if (iSMS_Id != 0)
        {
            SMSMasterCollectionBL oSMSMasterBL = new SMSMasterCollectionBL();
            DataTable oDT = oSMSMasterBL.GetExistingGroup(iSMS_Id,miSchoolId,miAcademicYearId);
            txtToUserId.Text = oDT.Rows[0]["Display_Text"].ToString();
            txtUser.Text = oDT.Rows[0]["Display_Text"].ToString();
            hidResendUserName.Value = txtUser.Text;            
            txtMessage.Text = oDT.Rows[0]["SMS_Text"].ToString();
            for (int iIndex = 0; iIndex < oDT.Rows.Count; iIndex++)
            {
                int iRole = oDT.Rows[iIndex]["RoleId"].ToInt();
                if (iRole == Constants.UserRoles.Admin.ToInt())//if admin
                {
                    chkAdmin.Checked = true;
                    HidAdminUserID.Value = oDT.Rows[iIndex]["UserId"].ToString();                    
                }
                if (iRole == Constants.UserRoles.Teacher.ToInt())//if teacher
                {
                    SMSMasterCollectionBL oSMSMaster = new SMSMasterCollectionBL();
                    int iTeacherId = oDT.Rows[iIndex]["UserId"].ToInt();
                    DataTable isPrincipal = oSMSMaster.IsPrincipal(iTeacherId, miAcademicYearId);
                    if (isPrincipal.Rows.Count > 0)
                    {
                        chkPrincipal.Checked = true;
                        HidTeacherId.Value = HidTeacherId.Value + oDT.Rows[iIndex]["UserId"].ToString() + ";";                                           
                    }
                    else
                    {
                        HidTeacherId.Value = HidTeacherId.Value + oDT.Rows[iIndex]["UserId"].ToString() + ";";                
                    }
                }
                if (iRole == Constants.UserRoles.Student.ToInt())//if student
                {
                    HidStudentId.Value = HidStudentId.Value + oDT.Rows[iIndex]["UserId"].ToString() + ";";   
                }
                if (iRole == Constants.UserRoles.Supervisor.ToInt())// if admin staff
                {
                    HidSupervisorId.Value = HidSupervisorId.Value + oDT.Rows[iIndex]["UserId"].ToString() + ";";                  
                }
                if (iRole == Constants.UserRoles.OtherStaff.ToInt())//if other staff
                {
                    HidOtherStaffId.Value = HidOtherStaffId.Value + oDT.Rows[iIndex]["UserId"].ToString() + ";";                
                }
                if (iRole == Constants.S_ZERO.ToInt())
                {
                    txtManualNumbers.Text = oDT.Rows[iIndex]["Display_Text"].ToString() + ",";
                }
            }

            if (HidTeacherId.Value != string.Empty)
            {
                string sTeacher = HidTeacherId.Value;
                HidTeacherId.Value = sTeacher.Truncate(sTeacher.Length - 1);
                HidTeacherId.Value = HidTeacherId.Value.Replace(".", string.Empty);
            }
            if (HidStudentId.Value != string.Empty)
            {
                string sStudent = HidStudentId.Value;
                HidStudentId.Value = sStudent.Truncate(sStudent.Length - 1);
                HidStudentId.Value = HidStudentId.Value.Replace(".", string.Empty);
            }
            if (HidSupervisorId.Value != string.Empty)
            {
                string sSupervisor = HidSupervisorId.Value;
                HidSupervisorId.Value = sSupervisor.Truncate(sSupervisor.Length - 1);
                HidSupervisorId.Value = HidSupervisorId.Value.Replace(".", string.Empty);
            }
            if (HidOtherStaffId.Value != string.Empty)
            {
                string sOtherStaff = HidOtherStaffId.Value;
                HidOtherStaffId.Value = sOtherStaff.Truncate(sOtherStaff.Length - 1);
                HidOtherStaffId.Value = HidOtherStaffId.Value.Replace(".", string.Empty);
            }
            if (txtManualNumbers.Text != string.Empty)
            {
                string sManualNumbers = txtManualNumbers.Text;
                Regex oRegx = new Regex("[A-Za-z]");
                int iCount = 0;
                while (iCount < oDT.Rows.Count)
                {   
                    if(oRegx.IsMatch(sManualNumbers))
                    {
                        sManualNumbers = sManualNumbers.Remove(0,sManualNumbers.IndexOf(','));                        
                        sManualNumbers = sManualNumbers.TrimStart(',');                        
                    }
                    iCount = iCount + 1;
                }
                int iLength = txtManualNumbers.Text.Length;
                txtManualNumbers.Text = sManualNumbers.Truncate(sManualNumbers.Length - 1);
                txtManualNumbers.Text = txtManualNumbers.Text.Replace(".", string.Empty);
            }
           DisableControl();
        }
        else
        {
            EnableControl();
        }
    }
    /// <summary>
    /// This method is used to enable the control.
    /// </summary>
    public void EnableControl()
    {
        chkAdmin.Enabled = true;
        chkPrincipal.Enabled = true;
        optTeachers.Enabled = true;
        optStudents.Enabled = true;
        optSupervisor.Enabled = true;
        optOtherStaff.Enabled = true;
        optParentTeacherAssociation.Enabled = true;
        optEntireSchool.Enabled = true;
        chkManualNumber.Enabled = true;
        chkManualNumber.Checked = false;
        hlnkPersonalAddresses.Visible = true;
        txtManualNumbers.Enabled = true;        
        lnkTeacherGroups.Visible = true;
        HlnkSelectUser.Visible = true;
        txtToUserId.Enabled = true;
        chkSendMessage.Visible = true;
    }
    /// <summary>
    /// This method is used to Disble the control.
    /// </summary>
    public void DisableControl()
    {
        chkAdmin.Enabled = false;
        chkPrincipal.Enabled = false;
        optTeachers.Enabled = false;
        optStudents.Enabled = false;
        optSupervisor.Enabled = false;
        optOtherStaff.Enabled = false;
        optParentTeacherAssociation.Enabled = false;
        optEntireSchool.Enabled = false;
        chkManualNumber.Enabled = false;
        hlnkPersonalAddresses.Visible = false;
        if (txtManualNumbers.Text != string.Empty)
        {
            chkManualNumber.Checked = true;           
        }        
        txtManualNumbers.Enabled = false;
        lnkTeacherGroups.Visible = false;
        HlnkSelectUser.Visible = false;
        txtToUserId.Enabled = false;
        chkSendMessage.Visible = false;
    }

    /// <summary>
    /// This method is used to get the SMS Balance of School.
    /// </summary>
    private void GetSMSBalance()
    {
        try
        {
            if (ConfigurationManager.AppSettings["SendSMS"].Equals(Constants.C_YES.ToString()))
            {
                if (Settings.SMSProviderForWebsite.ToLower() == Constants.SMSProviders.BusinessSMS.ToString().ToLower())
                {
                    string sSMSSenderUName = ConfigurationManager.AppSettings["SMSSenderUName"];
                    string sSMSSenderUPwd = ConfigurationManager.AppSettings["SMSSenderUPwd"];

                    //BSWSSoapClient oClient = new BSWSSoapClient("BSWSSoap");
                    //XmlElement element = oClient.BSAcBalance(sSMSSenderUName, sSMSSenderUPwd);

                    //lblSMSBalance.Text = element.ChildNodes[0].ChildNodes[0].InnerText;
                    //hidSMSCountVal.Value = element.ChildNodes[0].ChildNodes[0].InnerText;

                    string sPostString = "ID=" + sSMSSenderUName + "&Pwd=" + sSMSSenderUPwd;
                    string sBalance = ControlUtility.GetWebRequestResult(sPostString, "https://messaging.charteredinfo.com/smsaspx");
                    lblSMSBalance.Text = sBalance;
                    hidSMSCountVal.Value=sBalance;
                }
                else if (Settings.SMSProviderForWebsite.ToLower() == Constants.SMSProviders.SoftSMS.ToString().ToLower())
                {
                    //chkScheduleSMS.Checked = false;
                    //chkScheduleSMS.Enabled = false;
                    imgNew.Visible = false;
                    WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["SMSSenderIPForSoftSMS"].ToString() + "/miscapi/" + ConfigurationManager.AppSettings["SMSSenderUPwdForSoftSMS"].ToString() + "/getBalance/true/");

                    // If required by the server, set the credentials.
                    request.Credentials = CredentialCache.DefaultCredentials;
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    // Get the stream containing content returned by the server.
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        lblSMSBalance.Text = responseFromServer;
                        hidSMSCountVal.Value = responseFromServer;

                        // Clean up the streams and the response.
                        reader.Close();
                    }
                    response.Close();
                }
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// This method is used to initialize values used throught the project.
    /// </summary>
    private void InitializeValues()
    {
        hidManualSMSCount.Value = Settings.SetDefaultSMSCount.ToString();
        hidIncreasedSMSLength.Value = Constants.S_ZERO;
        if (miSchoolId == Constants.SchoolId.SNS.ToInt() || miSchoolId == Constants.SchoolId.PPSH.ToInt())
            hidIncreasedSMSLength.Value = "459";
    }
    #endregion
    
}