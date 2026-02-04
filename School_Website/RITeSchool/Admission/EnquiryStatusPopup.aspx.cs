/*
 * File Name - AdmissionStatusPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 2 Jan 215
 * Class Descriptin - This class is sued to add and display admission status and comments.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;
using System.Collections;
using SchoolEntities;
using Utility;
using System.Data;
using System.Web;
public partial class EnquiryStatusPopup : ExportDataTable
{
    #region Data Member(s)

    private StudentAdmissionsBL moStudentAdmissionsBL;

    #endregion
    private const string S_SMS_SEND = "SMS sent successfully !!!";
    private const string S_SMS_TEXT = "Your admission enquiry status is %Status%.  For any query contact office. - Accounts Officer.";
    #region Event(s)

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moStudentAdmissionsBL = new StudentAdmissionsBL(miSchoolId, 0, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnCancel.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    DisplayPreviousComments();  //
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill status combo box and fill display previous comments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillStatusCombo();
                DisplayPreviousComments();  //
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    

    /// <summary>
    /// This event is used to cancel current operation and clear controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save admission status details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        { 
            int  Id = Convert.ToInt32(hidAdmissionStatusDetailsId.Value);
            string EnquiryId = Convert.ToString(hidStudentEnquiryId.Value);              
            string  Comment = txtComment.Text.Trim();
            DateTime Date = Convert.ToDateTime(txtDate.Text);
            DateTime   FollowUpDate = Convert.ToDateTime(txtFollowupDate.Text);
            int StatusId = Convert.ToInt32(cmbStatus.SelectedValue);
           
            DataTable oDataTable = moStudentAdmissionsBL.SaveEnquiryStatusDetails(Id, EnquiryId, Comment, Date, FollowUpDate, StatusId);
            lblMessage.Text = "Enquiry status details saved successfully!!!";           
            ClearFields();
            DisplayPreviousComments();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {           
            SendSMS();
            lblMessage.Text = "SMS sent successfully!!!";         
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
            DataTable dtStudentEnquiry = oSchoolEnquiryBL.GetStudEnquiryStatusDetailsForExport(miSchoolId, hidNextAcademiYearId.Value.ToInt(), hidStudentEnquiryId.Value.ToInt());

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=StudentEnquiryStatusDetails.XLS");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
            HttpContext.Current.Response.Write("<TR>");

            AddHeader("Enquiry No", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Student Name", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Status", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Update Date", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Updated By", "text-align:left; font-weight:bold; font-size:17px;");   
            AddHeader("Comment", "text-align:left; font-weight:bold; font-size:17px; width:500px;");
            AddHeader("FollowUp Date", "text-align:center; font-weight:bold; font-size:17px;");            

            foreach (DataRow row in dtStudentEnquiry.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");

                AddTableRows(row["Enquiry_No"].ToString(), "text-align:left");
                AddTableRows(row["StudentName"].ToString(), "text-align:left");
                AddTableRows(row["Name"].ToString(), "text-align:left");
                AddTableRows(row["CurrentDate"].ToString(), "text-align:center");
                AddTableRows(row["UserName"].ToString(), "text-align:left");
                AddTableRows(row["Comment"].ToString(), "text-align:left");
                AddTableRows(row["FollowUpDate"].ToString(), "text-align:center");                
                
                HttpContext.Current.Response.Write("</TR>");
            }

            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End(); 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used for Adding the row Header.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

    /// <summary>
    /// 	This method is used for Adding the rows in to Table.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<TD " + sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }

    private void SendSMS()
    {   
        string sSmsSubject = string.Empty;

        int Id = Convert.ToInt32(hidStudentEnquiryId.Value);
        string sMobileNo1 = string.Empty;
        string sTemplateRegistrationId = string.Empty;  ////  
        //DataTable dtValues = StudentAdmissionsBL.GetEnquiryStudentForSMS(miSchoolId, miAcademicYearId, Id);
        Hashtable oHTUsersMobileNo = new Hashtable();
        if (oHTUsersMobileNo["TemplateRegistrationId"] != DBNull.Value)
            sTemplateRegistrationId = oHTUsersMobileNo["TemplateRegistrationId"].ToString();  
        int iSMSType = 0;

        if (hidMobileNumber.Value != string.Empty)
        {
            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            string sSMSSenderName = oSchoolBL.SMSSenderName;
            sMobileNo1 = hidMobileNumber.Value;
            oHTUsersMobileNo[sMobileNo1.Trim()] = sMobileNo1.Trim();
            sMobileNo1 = Convert.ToString(hidMobileNumber.Value);
            string sDisplayText = sMobileNo1;
            SMS oSMS = new SMS();
            oSMS.Sender = sSMSSenderName;
            oSMS.SMSText = txtSMS.Text.Trim();
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.DisplayText = sDisplayText;
            oSMS.SMSType = iSMSType;
            oSMS.SchoolID = miSchoolId;
            oSMS.AcademicYearID = Convert.ToInt32(miAcademicYearId);
            oSMS.ToManualNumbers = oHTUsersMobileNo;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;  ////
            oSMS.Send();
        }
    }
    /// <summary>
    /// This method is used to fill status combo.
    /// </summary>
    private void FillStatusCombo()
    {
        List<EnquiryStatus> lstStatuses = moStudentAdmissionsBL.GetAllEnquiryStatuses();
        ListSource.FillDropDownList(lstStatuses, cmbStatus, "Name", "Id", Constants.S_SELECT);
        ViewState["EnquiryStatuses"] = lstStatuses;
    }

    /// <summary>
    /// This method is used to display previous comments.
    /// </summary>
    private void DisplayPreviousComments()
    {
        btnExport.Visible = true;
        EnquiryDetails oAdmissionDetails;

        if (hidStudentEnquiryId.Value.Trim() == string.Empty)
            hidStudentEnquiryId.Value = QueryString["EnquiryId"].ToString();

        int iStudentAdmissionId = Convert.ToInt32(hidStudentEnquiryId.Value);
        List<EnquiryStatusDetails> lstDetails = moStudentAdmissionsBL.GetAllEnquiryComments(iStudentAdmissionId, out oAdmissionDetails).OrderByDescending(st => st.Id).ToList();

        lblStudentName.Text = oAdmissionDetails.StudentName;
        lblFormNo.Text = oAdmissionDetails.FormNumber;
        lblCurrentStatus.Text = oAdmissionDetails.CurrentStatus;
        hidLastCommentId.Value = Constants.S_ZERO;

        EnquiryStatusDetails oAdmissionStatusDetails = lstDetails.OrderByDescending(st => st.Id).FirstOrDefault();
        if (oAdmissionStatusDetails != null)
            hidLastCommentId.Value = oAdmissionStatusDetails.Id.ToString();

        bool bIsAlternetRow = false;

        List<EnquiryStatus> lstStatuses = new List<EnquiryStatus>();
        if (ViewState["EnquiryStatuses"] != null)
            lstStatuses = ViewState["EnquiryStatuses"] as List<EnquiryStatus>;

        lstDetails.ForEach
            (
                status =>
                {
                    string sHeaderClassName = "ClsProgressGridTestHeader";
                    string sCellClassName = "ClsMarksCell";
                    if (bIsAlternetRow)
                    {
                        sHeaderClassName = "ClsReceiverHeader";
                        sCellClassName = "ClsReceiverCell";
                        bIsAlternetRow = false;
                    }
                    else
                        bIsAlternetRow = true;

                    HtmlTableRow trSubHeader = new HtmlTableRow();
                    base.AddCell(trSubHeader, "Date : " + status.Date.ToString(Constants.S_DATE_FORMAT + " hh:mm tt"), sHeaderClassName, "left", 1, "width:50%");
                    base.AddCell(trSubHeader, "Updated By : " + status.UpdatedBy, sHeaderClassName, "left");

                    tblComments.Rows.Add(trSubHeader);

                    HtmlTableRow trStatus = new HtmlTableRow();
                    base.AddCell(trStatus,"Status : "+lstStatuses.Where(st=>st.Id == status.StatusId).FirstOrDefault().Name, sHeaderClassName, "left", 1);
                    base.AddCell(trStatus, "Follow-up Date : " + status.FollowUpDate.ToString(Constants.S_DATE_FORMAT), sHeaderClassName, "left", 1);
                    tblComments.Rows.Add(trStatus);


                    HtmlTableRow trContent = new HtmlTableRow();
                    base.AddCell(trContent, status.Comment, sCellClassName, "left", 2);
                    tblComments.Rows.Add(trContent);

                    AddEmptyRow();
                }

            );

        if (lstDetails.Count == 0)
        {
            lblPreviousComments.Visible = false;
            btnExport.Visible = false;
        }
        else
        {
            lblPreviousComments.Visible = true;
            btnExport.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        hidStudentEnquiryId.Value = QueryString["EnquiryId"].ToString();
        hidMobileNumber.Value = QueryString["MobileNumber"].ToString();
        hidNextAcademiYearId.Value = QueryString["NextAcademiYearId"].ToString();
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose, btnExport });
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
        cmbStatus.Focus();        
    }

    /// <summary>
    /// This method is used to add empty row.
    /// </summary>
    private void AddEmptyRow()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        AddCell(oHtmlTableRow, "<BR />", "ClsMarksCell", "Left", 3, "background-color:white");
        tblComments.Rows.Add(oHtmlTableRow);
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbStatus.ClearSelection();
        txtComment.Text = string.Empty;
        txtFollowupDate.Text = string.Empty;
        hidAdmissionStatusDetailsId.Value = Constants.S_ZERO;
    }

    #endregion    
}