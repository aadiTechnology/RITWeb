using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using BusinessLogic;
using System.Data;
using SchoolEntities;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class MissingAttendancePopup : SchoolBase
{
    #region Data Member(s)

    private string SMS_SENT = "SMS sent successfully to absent student(s)!!!";
    private string SMS_SENT_HALF_ATTENDANCE = "SMS sent successfully to 1/2 day present student(s)!!!";
    private string STUDENT_ABSENT_FROM_SMS = "StudentAbsentFromSMS";
    private string STUDENT_ABSENT_SMS = "StudentAbsentSMS";
    #endregion

    #region Event(s)

    /// <summary>
    /// This is page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQueryString();
                FillAbsentStudentListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to send SMS.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            int iIndex;
            Hashtable oHTUsersMobileNo = new Hashtable();
            string sMobileNo1 = string.Empty, sMobileNo2 = string.Empty, sName;
            for (iIndex = 0; iIndex < lstvwAbsentStudent.Items.Count; iIndex++)
            {
                CheckBox chkSelect = lstvwAbsentStudent.Items[iIndex].FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                {
                    sMobileNo1 = lstvwAbsentStudent.DataKeys[iIndex]["Mobile_Number"].ToString();
                    sMobileNo2 = lstvwAbsentStudent.DataKeys[iIndex]["Mobile_Number2"].ToString();
                    Label lblName = lstvwAbsentStudent.Items[iIndex].FindControl("lblName") as Label;
                    sName = lblName.Text;
                    if (sMobileNo1 != string.Empty)
                        oHTUsersMobileNo[lstvwAbsentStudent.DataKeys[iIndex]["User_Id"]] = sMobileNo1;
                    if (sMobileNo2 != string.Empty && sMobileNo2 != "0")
                    {
                        oHTUsersMobileNo[lstvwAbsentStudent.DataKeys[iIndex]["User_Id"] + "sm;"] = sMobileNo2;
                    }
                    SMS oSMS = new SMS();
                    SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                    oSMS.InsertedByID = -9999;
                    oSMS.Sender = oSchoolBL.SMSSenderName;
                    oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                    oSMS.SenderID = oSchoolBL.AdminId;
                    oSMS.School_Name = oSchoolBL.SchoolName;

                    DataTable oDTTemplate = new DataTable();
                    string sSMSTemplateName = string.Empty;
                    string sSmsText = string.Empty;
                    string sTemplateRegistrationId = string.Empty;
                    if (lstvwAbsentStudent.DataKeys[iIndex]["FromAbsentDate"].ToDateTime().ToString("dd-MMM-yyyy") != lblDateData.Text)                    
                        sSMSTemplateName = STUDENT_ABSENT_FROM_SMS;
                    else
                        sSMSTemplateName = STUDENT_ABSENT_SMS;

                    oDTTemplate = SmsTemplateBL.GetTemplate(sSMSTemplateName, miSchoolId);
                    sSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);

                    if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    if (lstvwAbsentStudent.DataKeys[iIndex]["FromAbsentDate"].ToDateTime().ToString("dd-MMM-yyyy") != lblDateData.Text)
                    {   
                        sSmsText = sSmsText.Replace("%NAME%", sName).Replace("%DATE%", lstvwAbsentStudent.DataKeys[iIndex]["FromAbsentDate"].ToDateTime().ToString("dd-MMM-yyyy"));
                        oSMS.SMSText = sSmsText;                        
                    }
                    else
                    {   
                        sSmsText = sSmsText.Replace("%NAME%", sName).Replace("%DATE%", lblDateData.Text);
                        oSMS.SMSText = sSmsText;                    
                    }
                    oSMS.AcademicYearID = miAcademicYearId;
                    oSMS.SchoolID = miSchoolId;
                    oSMS.DisplayText = sName;
                    oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                    oSMS.To = oHTUsersMobileNo;
                    oSMS.Send();
                    oHTUsersMobileNo.Clear();
                }
            }
            base.DisplayMessage(SMS_SENT, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is event is used to close popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'>window.close();window.opener.focus(); </Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSendSMSToHalfDayAbsentStudent_Click(object sender, EventArgs e)
    {
        try
        {
            int iIndex;
            Hashtable oHTUsersMobileNo = new Hashtable();
            string sMobileNo1 = string.Empty, sMobileNo2 = string.Empty, sName;
            for (iIndex = 0; iIndex < lstvwHalfDayAbsentStudentDetails.Items.Count; iIndex++)
            {
                CheckBox chkSelect = lstvwHalfDayAbsentStudentDetails.Items[iIndex].FindControl("chkSelectHalfDayStudent") as CheckBox;
                if (chkSelect.Checked)
                {
                    sMobileNo1 = lstvwHalfDayAbsentStudentDetails.DataKeys[iIndex]["Mobile_Number"].ToString();
                    sMobileNo2 = lstvwHalfDayAbsentStudentDetails.DataKeys[iIndex]["Mobile_Number2"].ToString();
                    Label lblName = lstvwHalfDayAbsentStudentDetails.Items[iIndex].FindControl("lblName") as Label;
                    sName = lblName.Text;
                    if (sMobileNo1 != string.Empty)
                        oHTUsersMobileNo[lstvwHalfDayAbsentStudentDetails.DataKeys[iIndex]["User_Id"]] = sMobileNo1;
                    if (sMobileNo2 != string.Empty && sMobileNo2 != "0")
                    {
                        oHTUsersMobileNo[lstvwHalfDayAbsentStudentDetails.DataKeys[iIndex]["User_Id"] + "sm;"] = sMobileNo2;
                    }
                    SMS oSMS = new SMS();
                    SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                    oSMS.InsertedByID = -9999;
                    oSMS.Sender = oSchoolBL.SMSSenderName;
                    oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                    oSMS.SenderID = oSchoolBL.AdminId;
                    oSMS.School_Name = oSchoolBL.SchoolName;
                    oSMS.SMSText = "This is to inform you that "+ sName +" was present for 1/2 day on " + lblDateData.Text + ".";
                    oSMS.AcademicYearID = miAcademicYearId;
                    oSMS.SchoolID = miSchoolId;
                    oSMS.DisplayText = sName;
                    oSMS.To = oHTUsersMobileNo;
                    oSMS.Send();
                    oHTUsersMobileNo.Clear();
                }
            }
            base.DisplayMessage(SMS_SENT_HALF_ATTENDANCE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwAbsentStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (!SchoolBase.Settings.SendSMSToAbsentStudent)
                {
                    HtmlTableRow trItemtemplate = e.Item.FindControl("trItemtemplate") as HtmlTableRow;
                    if (trItemtemplate != null)
                    {
                        HtmlTableCell tdChkSelect = trItemtemplate.FindControl("tdChkSelect") as HtmlTableCell;
                        if (tdChkSelect != null)
                            tdChkSelect.Visible = false;
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

    #region Method(s)

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillAbsentStudentListView()
    {
        string sStudentIds = hidStudentIds.Value;
        int iStandardDivisionId = Constants.S_ZERO.ToInt();
        iStandardDivisionId = Convert.ToInt32(hidStdDivId.Value);
        string sHalfDayAttendanceStudentIds = hidHalfDayPresentStudentId.Value;
        AttendanceDetailsBL oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();
        lblClassNameData.Text = oSchoolWiseAttendanceDetailsBL.GetClassName(miSchoolId, miAcademicYearId, iStandardDivisionId);
        sStudentIds = sStudentIds.Replace('"', ' ');
        List<AttendanceDetails> lstHalfDayStudentAttendanceDetails;
        List<AttendanceDetails> lstAttendanceDetails = oSchoolWiseAttendanceDetailsBL.GetAbsentStudentDetails(miSchoolId, miAcademicYearId, sStudentIds, iStandardDivisionId, lblDateData.Text.ToDateTime(), sHalfDayAttendanceStudentIds, out lstHalfDayStudentAttendanceDetails);
        lstvwAbsentStudent.DataSource = lstAttendanceDetails;
        lstvwAbsentStudent.DataBind();

        if (lstAttendanceDetails.Count > 0)
            btnSendSMS.Enabled = true;
        else
            btnSendSMS.Enabled = false;
        lstvwHalfDayAbsentStudentDetails.DataSource = lstHalfDayStudentAttendanceDetails;
        lstvwHalfDayAbsentStudentDetails.DataBind();
        if (lstHalfDayStudentAttendanceDetails.Count > 0)
            btnSendSMSToHalfDayAbsentStudent.Enabled = true;
        else
            btnSendSMSToHalfDayAbsentStudent.Enabled = false;

        if (!SchoolBase.Settings.SendSMSToAbsentStudent)
        {
            lblSendSMS.Text = "Absent Student Details.";
            trAbsentStudents.Visible = true;
            lblAbsentStudents.Text = "Student(s) is absent for " + SchoolBase.Settings.StudentAbsentCount.ToString() + " or more working days.";
            HtmlTableRow trHeader = lstvwAbsentStudent.FindControl("trHeader") as HtmlTableRow;
            btnSendSMS.Visible = false;

            if (trHeader != null)
            {
                HtmlTableCell thChkSelectAll = trHeader.FindControl("thChkSelectAll") as HtmlTableCell;
                if (thChkSelectAll != null)
                    thChkSelectAll.Visible = false;
                if (lstHalfDayStudentAttendanceDetails.Count == Constants.I_ZERO)
                {
                    tblHalfDayStudents.Visible = false;
                    btnSendSMSToHalfDayAbsentStudent.Visible = false;                    
                }
                else
                    tblHalfDayStudents.Visible = true;

                btnSendSMS.Visible = false;
            }
            else
            {
                tblHalfDayStudents.Visible = false;
                btnSendSMSToHalfDayAbsentStudent.Visible = false;
            }
        }
        else
        {
            lblSendSMS.Text = "Send SMS to absent student.";
            trAbsentStudents.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidStudentIds.Value = QueryString["StudentIdList"].ToString();
        hidStdDivId.Value = QueryString["StandardDivisionId"].ToString();
        lblDateData.Text = QueryString["SelectedDate"].ToString();
        hidHalfDayPresentStudentId.Value = QueryString["HalfDayPresentStudentIdList"].ToString();
    }

    #endregion    
}