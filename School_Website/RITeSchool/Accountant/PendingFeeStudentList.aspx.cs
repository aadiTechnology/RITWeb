// File Name  : PendingFeeStudentList.aspx.cs
// Created By : Milind
// Start Date  : 2 May 2009
// End Date    :    ?
// Description :This class is used to provide UI for students list whose due date is passed and send SMS and 
//             Message to that students.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;
using System.Collections;
using SchoolEntities.Common;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using dr = System.Drawing;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Linq;

public partial class PendingFeeStudentList : ExportToExcel
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #region -- Data Members(s) --

    StudentBL moStudentBL;

    #endregion

    #region -- CONSTANT(s) --

    private const string S_SMS_TEMPLATE_ID = "13";
    private const string S_SMS_TEXT = "Your school fee (Rs.%AMOUNT%) is pending till Date %DATE%. Please pay the dues ASAP. For any query contact office. - Accounts Officer.";
    private const string S_SMS_SEND = "SMS sent successfully !!!";

    #endregion -- CONSTANT(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to fill all controls data like standards,divisions and date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hidShow.Value = "Show";
                if (CheckPreCondition())
                {
                    if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    {
                        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    }
                    DesignSettingAccordingLanguage();
                    grdFeesToBePaid.PageSize = Constants.I_GRID_PAGE_COUNT;

                    InitializeForm();
                    SetDefaultProperties();
                    spnAmountMandetory.Attributes.Add("style", "display:none;");
                    btnSendSMS.Attributes.Add("onclick", "if(!ConfirmSMS()) return false;");
                    //spnAmountMandetory.Visible = cmbOperator.SelectedIndex != Constants.I_ZERO;	
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combox according to selected division.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStandardId.Value = ddlStandard.SelectedValue;
            FillDivisionCombobox();
            FillFeeTypeCombo();            

            if (cmbOperator.SelectedIndex != Constants.I_ZERO)
            {
                spnAmountMandetory.Attributes.CssStyle.Add("display", "");
                txtAmount.Enabled = true;
            }
            else
            {
                spnAmountMandetory.Attributes.CssStyle.Add("display", "none");
                txtAmount.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used show the grid according to the search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidShow.Value == "Show")
            {
                grdFeesToBePaid.PageIndex = 0;
                hidServerDate.Value = cal_DueDate.DateValue.ToString("yyyy-MM-dd");
                btnShow.Text = Resources.LocalizedResources.ChangeInput;
                hidShow.Value = "Change Input";
                grdFeesToBePaid.DataSourceID = GrdDSobj.ID;
                ShowHideControls(false);
            }
            else
            {
                btnShow.Text = Resources.LocalizedResources.Show;
                hidShow.Value = "Show";
                ShowHideControls(true);
                grdFeesToBePaid.DataSourceID = null;
                tblTotalAmount.Visible = false;
            }
        }       
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to resolve conflict in pending fee screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnResolveConflict_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            SchoolEntities.Common.TransactionResult oResult = oStudentBL.ResolveConflict(miSchoolId, miAcademicYearId, miUserId);

            if (oResult.IsSuccess)
            {
                if (oResult.Message == string.Empty)
                    lblMessage.Text = "Conflict is resolved successfully !!!";
                else
                    lblMessage.Text = oResult.Message;

                lblMessage.ForeColor = System.Drawing.Color.Blue;
                lblMessage.Font.Bold = true;                
            }
            else
            {
                lblMessage.Text = oResult.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Font.Bold = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            lblMessage.Text = "An error occurred while resolving conflict.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Font.Bold = false;
        }
    }

    /// <summary>
    /// This event is used to export report in exel format
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            //ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.PendingFeeReminder, GetFilterString());
            //oReportDisplay.DisplayReport();

            string sFileName = "StudentPendingFeeDetailsReport_" + Guid.NewGuid() + ".xlsx";

            //string filePath = Server.MapPath("..") + @"\UPLOADS\PendingFee\" + sFileName;
            string filePath = base.BasePath + @"\RITeSchool\UPLOADS\PendingFee\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookPartForStudentPaidFeeReport(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/PendingFee/" + sFileName + "')</Script>"));
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
    /// This event is used to go to SMS Center(SMSUI.aspx) page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = PrepareQueryString();
            var oMasterPage = this.Master as MasterPage;
            oMasterPage.RedirectToNextPage("~/Common/SMSUI.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go to Send Message(SendMessageFromInbox.aspx) page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = PrepareQueryString();
            var oMasterPage = this.Master as MasterPage;
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go to Send Message of Pending fee.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {            
            int iStandardId = ddlStandard.SelectedValue.ToInt();
            int iDivisionId = ddlDivision.SelectedValue.ToInt();
            string sRegistrationNo = txtRegNumber.Text;
            string odtStartDate = txtDueDate.Text;
            bool bLeftStudent = chkleftStu.Checked;
            bool bPDCStudent = chkPDCStud.Checked;
            int iFeeTypeId = cmbFeeType.SelectedValue.ToInt();
            string sOperator = cmbOperator.SelectedItem.Text;
            int iAmount = Convert.ToInt32(txtAmount.Text == string.Empty ? "0" : txtAmount.Text);
            string sPercentFilter = ddlFilter.SelectedValue;
            DataTable dtValues = StudentBL.GetPendingFeeStudentList(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, sRegistrationNo, odtStartDate, bLeftStudent, bPDCStudent, iFeeTypeId, cmbPayableFor.SelectedValue.ToString(), sOperator, iAmount, null, 9999, 0, sPercentFilter);
            foreach (DataRow row in dtValues.Rows)
            {
                SendSMS(row);              
            }
            base.DisplayMessage(S_SMS_SEND, false, tdMessage);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get Payable for as per selected fee type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillPayableForCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill Payable For Combobox for respective fee type.
    /// </summary>
    private void FillPayableForCombo()
    {
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        int iStandardId = Constants.I_ZERO; 
        iStandardId = ddlStandard.SelectedValue.ToInt();
        DataTable dtPayableFor = oStudentFeeDetailsBL.GetFeeTypewisePayableFor(miSchoolId, miAcademicYearId, cmbFeeType.SelectedValue.ToInt(), iStandardId);

        cmbPayableFor.Bind(dtPayableFor, "Value_Member", "Display_Member", Constants.S_SELECT_ALL);
    }

    #region -- GRID EVENT(s) --

    /// <summary>
    /// This event is used to sort data according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdFeesToBePaid_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void grdFeesToBePaid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdFeesToBePaid.PageIndex = e.NewPageIndex;
            grdFeesToBePaid.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdFeesToBePaid.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;

            // Set the PageIndex property to display that page selected by the user.
            grdFeesToBePaid.PageIndex = pageList.SelectedIndex;
            grdFeesToBePaid.DataSourceID = GrdDSobj.ID;
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
    /// 
    protected void grdFeesToBePaid_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            var sGridviewName = sender as GridView;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
                CommonUtility.AddSortImage(sortColumnIndex != -1 ? sortColumnIndex : 1, e.Row, sGridviewName.SortDirection);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                if (e.ReturnValue.ToInt() == 0)
                {
                    tblTotalAmount.Visible = false;
                    EnableDisbleSMSMessageControl(false);
                }
                else
                {
                    tblTotalAmount.Visible = true;
                    EnableDisbleSMSMessageControl(true);
                }

                lblStartIndex.Text = Convert.ToString((grdFeesToBePaid.PageSize * grdFeesToBePaid.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdFeesToBePaid.PageSize) - 1);
                if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        trTotalRec.Visible = e.ReturnValue.ToString() != "0";
                    }

                    if (lblTotal.Text != string.Empty)
                        trTotalRec.Visible = lblTotal.Text.ToInt() > Constants.I_GRID_PAGE_COUNT;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdFeesToBePaid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                string sLeftDate = grdFeesToBePaid.DataKeys[e.Row.RowIndex]["SchoolLeft_Date"].ToString();
                if (!sLeftDate.IsNullOrEmpty())
                    e.Row.Style.Add(HtmlTextWriterStyle.Color, "red");
                lblTotalAmount.Text = grdFeesToBePaid.DataKeys[e.Row.RowIndex]["TotalAmount"].ToString();
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                if (grdFeesToBePaid.Rows.Count == Constants.I_ONE)
                {
                    string sLeftDate = grdFeesToBePaid.DataKeys[Constants.I_ZERO]["SchoolLeft_Date"].ToString();
                    if (!sLeftDate.IsNullOrEmpty())
                        btnMessage.Enabled = false;
                }

                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
                var pageLabel = pagerRow.Cells[0].FindControl("CurrentPageLabel") as Label;

                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdFeesToBePaid.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        var item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdFeesToBePaid.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdFeesToBePaid.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = Resources.LocalizedResources.PageNo + " " + currentPage + Resources.LocalizedResources.Of + " " + grdFeesToBePaid.PageCount + " " + Resources.LocalizedResources.OutOflst;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- GRID EVENT(s) --

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method used to send SMS about Pending fee details.
    /// </summary>
    private void SendSMS(DataRow row)
    {
        string sTemplateRegistrationId = string.Empty; //
        string sMessage = S_SMS_TEXT.Replace("%AMOUNT%", row["Amount"].ToString()).Replace("%DATE%", txtDueDate.Text);
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        Hashtable oHTUsersMobileNo = new Hashtable();

        oHTUsersMobileNo[row["User_Id"].ToInt()] = row["Mobile_Number"];

        if (row["Mobile_Number2"].ToString() != string.Empty && row["Mobile_Number2"].ToString() != Constants.S_ZERO)
        {
            oHTUsersMobileNo[row["User_Id"].ToInt() + "sm;"] = row["Mobile_Number2"].ToString();
            if (oHTUsersMobileNo["TemplateRegistrationId"] != DBNull.Value)     
                sTemplateRegistrationId = oHTUsersMobileNo["TemplateRegistrationId"].ToString();   
        }

        SMS oSMS = new SMS();
        oSMS.InsertedByID = -9999;
        oSMS.Sender = oSchoolBL.SMSSenderName;
        oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
        oSMS.SenderID = oSchoolBL.AdminId;
        oSMS.School_Name = oSchoolBL.SchoolName + "::" + "PendingFeeSMS";
        oSMS.SMSText = sMessage;
        oSMS.AcademicYearID = miAcademicYearId;
        oSMS.SchoolID = miSchoolId;
        oSMS.DisplayText = row["SMSName"].ToString();
        oSMS.TemplateRegistrationId = sTemplateRegistrationId;   /////
        oSMS.To = oHTUsersMobileNo;
        oSMS.Send();
        oHTUsersMobileNo.Clear();
    }

    /// <summary>
    /// This method fills combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ddlStandard.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    private void FillDivisionCombobox()
    {
        const string S_STDDIV_ID_FLD = "division_Id";
        var oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);

        DataTable dtDivision = hidStandardId.Value != string.Empty ? oDiv.GetAllDivisionsForStandard(hidStandardId.Value.ToInt()) : oDiv.GetAllSchoolDivisions();

        // This method is used to fill current division's combo.
        if (miSchoolId == Constants.SchoolId.PPS.ToInt() && ddlStandard.SelectedItem.Text == "10")
        {
            DataRow[] dr = dtDivision.Select("Division_Name='G'");

            if (dr.Length > 0)
            {
                dr[0].Delete();
                dtDivision.AcceptChanges();
            }
        }
        ddlDivision.Bind(dtDivision, S_STDDIV_ID_FLD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <Summary>
    /// This method is used to set default properties to controls.
    /// </Summary>   
    private void SetDefaultProperties()
    {
        SetDefaultButton(btnShow);
        txtDueDate.Focus();
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSMSText.Value = S_SMS_TEXT;
    }

    /// <summary>
    /// This method is used to hide and show the control.
    /// </summary>
    private void ShowHideControls(bool bFlag)
    {
        ddlFilter.Enabled = bFlag;
        btnExport.Visible = !bFlag;
        pnlFeesToBePaidGrid.Visible = !bFlag;
        btnSMS.Visible = !bFlag;
        btnSendSMS.Visible = !bFlag;
        btnMessage.Visible = !bFlag;
        ddlDivision.Enabled = bFlag;
        ddlStandard.Enabled = bFlag;
        txtRegNumber.Enabled = bFlag;
        txtDueDate.Enabled = bFlag;
        cal_DueDate.Enabled = bFlag;
        chkleftStu.Enabled = bFlag;
        chkPDCStud.Enabled = bFlag;
        cmbOperator.Enabled = bFlag;
        cmbFeeType.Enabled = bFlag;
        cmbPayableFor.Enabled = bFlag;
        txtAmount.Enabled = cmbOperator.SelectedValue.ToInt() != 0 && hidShow.Value != "Change Input";
        btnResolveConflict.Enabled = bFlag;

        if (miSchoolId == Constants.SchoolId.LORDDS.ToInt() )
            trTotalAmount.Visible = false;
    }

    /// <summary>
    /// This method is used to check if the login user is of superviser role and 
    /// check the access he have for SMS
    /// </summary>
    private void SetSMSAccessRights()
    {
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
        {
            hidIsSMSAccessEnabled.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter).ToString();
            hidIsMsgAccessEnabled.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.MessageCenter).ToString();
        }
    }

    /// <summary>
    /// This method is used set javascripts atributes.
    /// </summary>
    private void SetJavaScriptAtributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnSMS, btnMessage, btnShow, btnExport, btnSendSMS });
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used enable or disable SMS and Message buttons.
    /// </summary>
    private void EnableDisbleSMSMessageControl(bool bFlag)
    {
        btnExport.Enabled = bFlag;
        btnMessage.Enabled = bFlag;
        btnSMS.Enabled = bFlag;
        btnSendSMS.Visible = bFlag;
        if (moUserRole == Constants.UserRoles.Supervisor && (hidIsSMSAccessEnabled.Value == Constants.S_EMPTY_STRING || Convert.ToChar(hidIsSMSAccessEnabled.Value) == Constants.C_NO))
            btnSMS.Enabled = false;
        if (moUserRole == Constants.UserRoles.Supervisor && (hidIsMsgAccessEnabled.Value == Constants.S_EMPTY_STRING || Convert.ToChar(hidIsMsgAccessEnabled.Value) == Constants.C_NO))
            btnMessage.Enabled = false;
    }

    /// <summary>
    /// This method is used to prepare Query Strings.
    /// </summary>
    private string PrepareQueryString()
    {
        const string S_PAGE = "Fee";
        string sQuerystring = string.Format("From={0}&DueDate={1}&Standard_Id={2}&Division_Id={3}&sRegNo={4}&bLeftStudent={5}&bPDCStudent={6}&FeeTypeId={7}&PayableFor={8}&Operator={9}&Amount={10}&SMSId={11}&PercentFilter={12}",
                                             S_PAGE,
                                             txtDueDate.Text,
                                             ddlStandard.SelectedValue,
                                             ddlDivision.SelectedValue,
                                             txtRegNumber.Text,
                                             chkleftStu.Checked,
                                             chkPDCStud.Checked,
                                             cmbFeeType.SelectedValue,
                                             cmbPayableFor.SelectedValue,
                                             cmbOperator.SelectedItem.Text,
                                             txtAmount.Text == string.Empty ? "0" : txtAmount.Text,
                                             S_SMS_TEMPLATE_ID,
                                             ddlFilter.SelectedValue);
        string sQueryString = CommonUtility.EncryptQuerystring(sQuerystring);

        return sQueryString;
    }

    /// <summary>
    /// This function is used to initialise form.
    /// </summary>
    private void InitializeForm()
    {
        SetJavaScriptAtributes();
        SetSMSAccessRights();
        FillStandardCombobox();
        ddlDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL, "0"));
        cal_DueDate.DateValue = DateTime.Today;
        ShowHideControls(true);
        hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
        hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
        chkleftStu.Checked = true;
        chkPDCStud.Checked = true;
        txtAmount.Enabled = false;
        FillFeeTypeCombo();
        FillPayableForCombo();
    }

    private void FillFeeTypeCombo()
    {
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        int iStandardId = ddlStandard.SelectedValue.ToInt();
        DataTable dtStdFeeType = oStudentFeeDetailsBL.GetStandardFeeType(miSchoolId, miAcademicYearId, iStandardId);
        DataRow oDR = dtStdFeeType.NewRow();
        oDR["Schoolwise_Standard_FeeType_Id"] = -1;
        oDR["Fee_Type"] = "Others";
        dtStdFeeType.Rows.Add(oDR);
        cmbFeeType.Bind(dtStdFeeType, "SchoolWise_Standard_FeeType_Id", "Fee_Type", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.PendingFeeList);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            tblInputFields.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        string sSchoolYearFilter = string.Empty;
        string sSchoolId = Constants.S_EXPORT_PENDING_FEE + ".iSchoolId}";
        string sAcdYearId = Constants.S_EXPORT_PENDING_FEE + ".AcadmicYearId}";
        string sRegNo = Constants.S_EXPORT_PENDING_FEE + ".sFilterRegAndName}";
        string sDue_Date = Constants.S_EXPORT_PENDING_FEE + ".Due_Date}";
        string sStdDivFilter = Constants.S_EXPORT_PENDING_FEE + ".sStdDivFilter}";
        string sFeeTypeId = Constants.S_EXPORT_PENDING_FEE + ".FeeTypeId}";
        string sIsIgnorePDCStudent = Constants.S_EXPORT_PENDING_FEE + ".IsIgnorePDCStudent}";
        string sAmountFilter = Constants.S_EXPORT_PENDING_FEE + ".sAmountFilter}";
        string sSortExp = Constants.S_EXPORT_PENDING_FEE + ".sortExp}";
        string sStartIndex = Constants.S_EXPORT_PENDING_FEE + ".StartIndex}";
        string sPazeSize = Constants.S_EXPORT_PENDING_FEE + ".PazeSize}";
        string sPercentFilter = Constants.S_EXPORT_PENDING_FEE + ".PercentFilter}";
        string sOperator = Constants.S_EXPORT_PENDING_FEE + ".Oprater}";
        string sSortDirection = grdFeesToBePaid.SortDirection.ToString() == Constants.S_ASCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
        string sStdDiv = "and SchoolLeft_Date IS NULL ";

        if (ddlStandard.SelectedValue != "0" && ddlDivision.SelectedValue == "0")
            sStdDiv += "and Standard_Master.Standard_Id=" + ddlStandard.SelectedValue;
        else if (ddlStandard.SelectedValue != "0" && ddlDivision.SelectedValue != "0")
            sStdDiv += "and Standard_Master.Standard_Id=" + ddlStandard.SelectedValue + " and Division_Master.Division_Id=" + ddlDivision.SelectedValue;

        sSchoolYearFilter = string.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7} AND {8}={9} AND {10}={11} AND {12}={13} AND {14}={15}) AND {16}={17} AND {18}={19} AND {20}={21} AND {22}={23} AND {24}={25} AND {26}={27}",
                                                sSchoolId,
                                                miSchoolId,
                                                sAcdYearId,
                                                miAcademicYearId,
                                                sDue_Date,
                                                txtDueDate.Text.Trim(),
                                                sRegNo,
                                                txtRegNumber.Text.Trim(),
                                                sStdDivFilter,
                                                sStdDiv,
                                                sFeeTypeId,
                                                cmbFeeType.SelectedValue,
                                                sAmountFilter,
                                                txtAmount.Text.Trim() == string.Empty ? "0" : txtAmount.Text.Trim(),
                                                sIsIgnorePDCStudent,
                                                chkPDCStud.Checked == true ? Constants.S_YES : Constants.S_NO,
                                                sSortExp,
                                                hidSortExpression.Value == string.Empty ? "Std_Div_ID" : hidSortExpression.Value + " " + sSortDirection,
                                                sStartIndex,
                                                Constants.S_ZERO,
                                                sPazeSize,
                                                3000,
                                                sPercentFilter,
                                                ddlFilter.SelectedValue == "2" ? txtAmount.Text.Trim() : string.Empty,
                                                sOperator,
                                                cmbOperator.SelectedItem.Text == Constants.S_SELECT ? ">" : cmbOperator.SelectedItem.Text,
                                                "usp_PendingFeeStudentList.PayableFor}",
                                                cmbPayableFor.SelectedItem.Text
                                                );

        return sSchoolYearFilter + "@ ";
    }

    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        btnShow.Text = oResourceManager.GetString(hidShow.Value.Replace(" ", string.Empty));
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidDueDateShouldNotBeBlank.Value = Resources.LocalizedResources.DueDateShouldNotBeBlank;
    }

    #region Export

    ///// <summary>
    ///// This method is used to create work book part for student paid fee details report.
    ///// </summary>
    ///// <param name="aoPart"></param>
    private void CreateWorkBookPartForStudentPaidFeeReport(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentPaidFeeDetailsReportContent(worksheetPart1);

        GeneratePartContent(aoPart, "Pending Fees");
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentPaidFeeDetailsReportContent(WorksheetPart aoWorksheetPart1)
    {
        //   int iColCount = moStudentPaidFeeDetailsBL.PayableForDetails.Count;
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        SetPendingFeeColumnWidth(worksheet1);
        AddPaidFeeHeader(sheetData1);
        AddPendingFeeData(sheetData1);

        worksheet1.Append(sheetData1);

        AddPrintOptions(worksheet1);
        SetPageMargin(worksheet1, 0.2);
        SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used add column header to excel file.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPaidFeeHeader(SheetData aoSheetData1)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(1), CustomHeight = true, Height = 15 };
        row.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Mobile No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Pending Amt", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Late Fee", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        if (moSchool == Constants.SchoolId.PPSH)
        {
            row.Append(AddCell("Tuition Fee", CellValues.String, StudentPaidFeeEnum.LeftHeader));
            row.Append(AddCell("Term Fee", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        }
        else
        {
            row.Append(AddCell("Payable For", CellValues.String, StudentPaidFeeEnum.LeftHeader));
            row.Append(AddCell("Applicable Fee", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        }
        if (moSchool == Constants.SchoolId.PPSN)
            row.Append(AddCell("Residence Type", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        if (moSchool == Constants.SchoolId.SNS)
            row.Append(AddCell("Pickup Journey", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        aoSheetData1.Append(row);
    }

    private void AddPendingFeeData(SheetData aoSheetData)
    {
        moStudentBL = new StudentBL();

        string sStdDiv = string.Empty;

        if(chkleftStu.Checked)
            sStdDiv = "and SchoolLeft_Date IS NULL ";

        if (ddlStandard.SelectedValue != "0" && ddlDivision.SelectedValue == "0")
            sStdDiv += "and Standard_Master.Standard_Id=" + ddlStandard.SelectedValue;
        else if (ddlStandard.SelectedValue != "0" && ddlDivision.SelectedValue != "0")
            sStdDiv += "and Standard_Master.Standard_Id=" + ddlStandard.SelectedValue + " and Division_Master.Division_Id=" + ddlDivision.SelectedValue;

        string sSortDirection = grdFeesToBePaid.SortDirection.ToString() == Constants.S_ASCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;

        DataSet dsFee = moStudentBL.GetAllDetailsOfPendingFee(miSchoolId, miAcademicYearId, txtDueDate.Text.ToDateTime(), txtRegNumber.Text == string.Empty ? "" : txtRegNumber.Text.Trim(), sStdDiv, txtAmount.Text == string.Empty ? "0" : txtAmount.Text.Trim(), cmbFeeType.SelectedValue.ToInt(), chkPDCStud.Checked == true ? Constants.S_YES : Constants.S_NO, hidSortExpression.Value == string.Empty ? "Std_Div_ID" : hidSortExpression.Value + " " + sSortDirection, Constants.I_ZERO, 3000, cmbOperator.SelectedItem.Text == Constants.S_SELECT ? ">" : cmbOperator.SelectedItem.Text, cmbPayableFor.SelectedItem.Text);

        DataTable dtFeeDetails = dsFee.Tables[0];

        int iRowCount = Constants.I_TWO;
        foreach (DataRow dtPending in dtFeeDetails.Rows)
        {
            Row row1 = new Row { RowIndex = Convert.ToUInt32(iRowCount), CustomHeight = true, Height = 15 };
            row1.Append(AddCell(dtPending["Enrolment_Number"].ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
            row1.Append(AddCell(dtPending["Class"].ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
            row1.Append(AddCell(dtPending["Roll_No"].ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            row1.Append(AddCell(dtPending["StudentName"].ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
            row1.Append(AddCell(dtPending["Mobile_Number"].ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
            row1.Append(AddCell(dtPending["Amount"].ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            row1.Append(AddCell(dtPending["Late_Fee_Amt"].ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            if (moSchool == Constants.SchoolId.PPSH)
            {
                foreach (DataRow dr in dsFee.Tables[2].Rows)
                {
                    var amount = dsFee.Tables[1].AsEnumerable().Where(dt => dt.Field<int>("Student_Id") == dtPending["Student_Id"].ToInt() && dt.Field<string>("Fee_Type") == dr["FeeType"].ToString()).Select(dt => dt.Field<int>("Amount")).FirstOrDefault();
                    if (amount != null)
                        row1.Append(AddCell(amount.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
            }
            else
            {
                row1.Append(AddCell(dtPending["PaybleFor"].ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
                row1.Append(AddCell(dtPending["FeesApplicable"].ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            }
            if (moSchool == Constants.SchoolId.PPSN)
                row1.Append(AddCell(dtPending["Name"].ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
            if (moSchool == Constants.SchoolId.SNS)
            {
				var ShiftName = dsFee.Tables[3].AsEnumerable().Where(dt => dt.Field<int>("YearWise_Student_Id") == dtPending["Student_Id"].ToInt())
					.Select(dt => dt.Field<string>("TransportShiftName")).FirstOrDefault();
				if (ShiftName != null)
				{
					row1.Append(AddCell(ShiftName.ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
				}
				else
					row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
            }
            iRowCount++;
            aoSheetData.Append(row1);
        }
    }

    /// <summary>
    /// This method is used to set column width for pending fee report.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetPendingFeeColumnWidth(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 12D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 12D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 40D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 13D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = (UInt32Value)6U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)7U, Max = (UInt32Value)7U, Width = 10D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)8U, Max = (UInt32Value)8U, Width = 25D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)9U, Max = (UInt32Value)9U, Width = 16D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)10U, Max = (UInt32Value)10U, Width = 40D, CustomWidth = true });
        
        aoWorksheet1.Append(columns1);
    }
    
    #endregion
        
    #endregion -- PRIVATE METHOD(s) --   
}