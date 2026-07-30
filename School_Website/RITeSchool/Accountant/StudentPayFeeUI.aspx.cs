// File Name  : StudentPayFeeUI.aspx.cs
// Created By : Anugandha
// Date       : 23 Sep 2008
//Description :This class is used to provide ui for fee status of a particular student 
//             as well give facility to pay fee.
//Modified By : Milind
// Date       : 12 Sep 2009

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using FeeEntities;
using SchoolBusinessService;
using Utility;
using System.Resources;
using System.Web.UI.HtmlControls;
using StudentEntities;
using System.Xml;
using System.Web;
using System.IO;
using System.Linq;
using SchoolEntities;
using PayrollReportingUserEntities;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SchoolEntities.StudentFee;

public partial class StudentPayFeeUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #region -- CONSTANT(s) --

    private const int I_COLUMN_INDEX_DELETE = 11;
    private const int I_COLUMN_INDEX_STUDENTCHKBOX = 0;
    private const int I_COLUMN_INDEX_EDIT = 10;
    private const string S_STUDENT = "Student";
    private const string S_CHECK_BOX_PAY = "ChkBoxPay";
    private const string S_CHECK_BOX_STUDENTPAY = "ChkBoxStudentPay";
    private const string S_RADIO_PAY_FEE = "rdoPayFee";    
    private const int I_COLUMN_INDEX_CHEQUE_AMT = 2;
    private const int I_COLUMN_INDEX_PAY = 5;
    private const int I_COLUMN_INDEX_STATUS = 4;
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Fee Structure/";
    private const int I_COLUMN_INDEX_VIEW = 13;

    private const string S_RTESTUDENT_MESSAGE = "* RTE Student (100% Concession on school fees)";
    private const int I_COLUMN_INDEX_LEFT_DATE = 6;
    private const int I_YEARWISE_STUDENT_ID = 0;
    private const int I_ISRTE_STUDENT = 2;
    private static string msFromUrl = string.Empty;
    private const string S_SCREENS_URL = "StudentDetailsUI.aspx";
    #endregion -- CONSTANT(s) --

    #region -- MEMBER(s) --

    private bool mbIsOldFeeDetails;
    private DateTime odtToday;
    private bool mbIsPaidForNextYear;
    private bool mbIsInternalFeePaidForNextYear;
    private string msIsPendingForLastYear;    
    private bool mbIsLastYearInternalFeePending;
    private string msAcademicYear;
    private bool mbIsLeftStudent;
  

    #endregion -- MEMBER(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// 	Returns true if the Accounts Module is enabled for the school.
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    /// <summary>
    ///		Return true if the currently logged in user is a Student.
    /// </summary>
    private bool IsStudentLogin
    {
        get { return moUserRole == Constants.UserRoles.Student; }
    }

    private bool IsRTEStudent
    {
        get;
        set;
    }

    #endregion -- PROPERTIES --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// 	This event is used to set master page accoringly.
    /// </summary>
    /// <param name="e"> </param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            string sFromPage = string.Empty;

            if (Request.QueryString.ToString() != string.Empty)
            {
                if (QueryString["FromPage"] != null)
                    sFromPage = QueryString["FromPage"];
            }

            if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";

            if (sFromPage == S_SCREENS_URL)
                msFromUrl = sFromPage;

            //If mbIsOldFeeDetails is true then form has to show the information about previous years
            //And for that this form is open as pop up therefore change the master page of this form.
            mbIsOldFeeDetails = bIsOldFeeDetails();
            if (mbIsOldFeeDetails)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is handled to call the base class OnInit function.
    /// </summary>
    /// <param name="e"> </param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to set default values to controls and load the form for the login user according to checked the right of that logged in user.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckFinancialYearStatus();
            if (!IsPostBack)
            {               
                ChangeFeeStrutcureLinkStatus();
                DisplayNote();
                hidAPopupBlockerIsDetected.Value = Resources.LocalizedResources.APopupBlockerIsDetected;

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }

                hidBaseFinancialYearId.Value = miFinancialYearId.ToString();
                hidSearch.Value = "Show";
                DesignSettingAccordingLanguage();
                hidbaseUrl.Value = Request.Url.GetLeftPart(UriPartial.Authority);
                CheckRoleAndAssignDisplayView();

                // Hide the caution money button if it is not applicable.
                btnPayCautionMoney.Visible = Settings.IsCautionMoneyApplicable;
                tdResetRecipt.Visible = false;
                if ((CheckIfAdminUser() || Convert.ToChar(hidCanEdit.Value) == Constants.C_YES) && !mbIsOldFeeDetails && QueryString["IsStudntDtailsScrn"].IsNullOrEmpty())
                {
                    txtRegNumber.Focus();

                    btnPay.Attributes.Add("Onclick", string.Format("if(!(ConfirmAction('{0}','{1}'))){{return false;}}", grdFeesToBePaid.AllowPaging, Resources.LocalizedResources.AtLeastOneEntryShouldBeSelectForPayingFee));

                    hlnkBankDetails.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=650').focus(); return false;", hlnkBankDetails.NavigateUrl));

                    txtRegNumber.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnSearch.ClientID));

                    if (miSchoolId == Constants.SchoolId.SNS.ToInt() || moSchool == Constants.SchoolId.VPMCPS)
                    {
                        tdResetRecipt.Visible = true;
                        hlnkReceiptNo.Attributes.Add("onclick", "OpenReceiptResetPopup(); return false");                        
                    }

                    tdSms.Visible = true;
                    ReadQueryString();
                }
                else if ((Convert.ToChar(hidCanEdit.Value) == Constants.C_NO && (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)) && !mbIsOldFeeDetails && QueryString["IsStudntDtailsScrn"].IsNullOrEmpty())
                {
                    txtRegNumber.Focus();
                    hlnkBankDetails.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=650').focus(); return false;", hlnkBankDetails.NavigateUrl));

                    txtRegNumber.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnSearch.ClientID));
                    ReadQueryString();
                    tdBank.Visible = false;
                    tdSearch.ColSpan = 2;
                    tdSms.Visible = false;
                }
                else
                {
                    btnOnlinePayment.Attributes.Add("Onclick", string.Format("if(!(ConfirmActionForStudent('{0}','{1}'))){{return false;}}", grdFeesToBePaid.AllowPaging, Resources.LocalizedResources.AtLeastOneEntryShouldBeSelectForPayingFee));
                    ShowAndHideOldFeeRecordLink();
                    LoadFormForStudent();
                    if(hidSNSSchoolId.Value == Constants.S_YES)
                        btnOnlinePayment.Enabled = true;
                    else
                        btnOnlinePayment.Enabled = false;

                    hidRestrictFeePaymentForSequence.Value = (Settings.RestrictFeePaymentForSequence ? Constants.S_YES : Constants.S_NO);

                    trPay.Visible = true;
                    btnPayCautionMoney.Visible = false;
                    btnPay.Visible = false;
                    tdlnkRefund.Visible = false;
                    tdPDCOpen.Visible = false;
                    td1.Visible = false;
                    if (!QueryString["IsStudntDtailsScrn"].IsNullOrEmpty())
                    {
                        btnOnlinePayment.Visible = false;
                        btnOnlineCautionMoneyPayment.Visible = false;
                        hlnkFeestructure.Visible = false;
                        btnBack.Text = "Close";
                        hlnkNextYr.Visible = false;

                        btnOnlineInternalFeePayment.Visible = false;
                    }
                }
                SetJavaScriptAtributes();
                ReadQuerystring();
                
                ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
                List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
                if (moUserRole != Constants.UserRoles.Student && (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.RestrictUsersForFeeUpdation.ToInt() && ru.UserId == miUserId).Any()))
                {
                    tdBankChallan.Visible = false;
                    tdSms.Visible = false;
                    tdBank.Visible = false;
                    tdResetRecipt.Visible = false;
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
            SetDefaultButton(btnSearch);
           }
        catch (Exception ex)
        {
            if (moUserRole == Constants.UserRoles.Student)
            {
                tblMain.Visible = false;
                divMsg.Visible = true;
            }

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to search students by reg. no or name.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblLeaveMessage.Visible = false;
            lblStuError.Text = String.Empty;
            tdlnkRefund.Visible = false;
            lblLastPayment.Text = string.Empty;
            lblStudentAbsent.Text = string.Empty;
            lblLeft.Text = string.Empty;
            lblPDCDetails.Text = string.Empty;
            lblNextYearPayment.Text = string.Empty;

            if (txtRegNumber.Enabled)
            {
                SetStudentGridViewDateColumnProperties();
                grdStudents.PageIndex = 0;
                grdStudents.DataSourceID = GrdDSobj.ID;
                grdStudents.DataBind();
                txtRegNumber.Enabled = false;
                btnSearch.Text = Resources.LocalizedResources.ChangeInput;
                hidSearch.Value = "Change Input";
                if (grdStudents.Rows.Count == 1)
                {
                    int iStudentId = grdStudents.DataKeys[Constants.I_ZERO][I_YEARWISE_STUDENT_ID].ToInt();
                    ShowStudentForFeeEntry(iStudentId, 0);
                    trStudents.Visible = false;

                }
            }
            else
            {
                ShowHideFields(false);
                txtRegNumber.Enabled = true;
                btnSearch.Text = Resources.LocalizedResources.Show;
                hidSearch.Value = "Show";
                txtRegNumber.Focus();
                grdStudents.DataSourceID = null;
                grdFeesToBePaid.DataSourceID = null;
                grdPostdatedCheque.DataSourceID = null;
                trNote.Visible = false;
                trStudents.Visible = false;
                lblNoDebitEntry.Text = string.Empty;
                trNoDebit.Visible = false;
                hlnkOldFeeRecord.Visible = false;//This line is modify to make invisible hyperlink for old student.
            }
        }
        catch (NoRecordFoundException oEx)
        {
            trNoDebit.Visible = true;
            lblNoDebitEntry.Text = oEx.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to display fee details according to academic year.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void cmbAcademicYrId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowGridOnPopup();
            lblacademicYr.Visible = true;
            hlnkOldFeeRecord.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update internal fee link status.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void hidInternalFeeDetails_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            SetInternalFeeAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This is the Selected event of ObjectDataSource control.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != String.Empty && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdStudents.PageSize) - 1);
                if (e.ReturnValue.ToString() != String.Empty && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                        {
                            trTotalRec.Visible = false;
                            trStudents.Visible = false;
                            lblStuError.Text = Resources.LocalizedResources.StudentNotFound;
                            ShowHideFields(false);
                            txtRegNumber.Enabled = true;
                            btnSearch.Text = Resources.LocalizedResources.Show;
                            hidSearch.Value = "Show";
                            txtRegNumber.Focus();
                        }
                        else
                        {
                            trTotalRec.Visible = true;
                            trStudents.Visible = true;
                        }
                    }
                    if (lblTotal.Text != String.Empty)
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
    /// 	This event is used to display fee details of the student who is selected from student grid.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SELECT_STUDENT")
        {
            try
            {
                int iRowIndex = e.CommandArgument.ToInt();
                int iStudentId = grdStudents.DataKeys[iRowIndex]["Yearwise_Student_Id"].ToInt();
                ShowStudentForFeeEntry(iStudentId, iRowIndex);
                trStudents.Visible = false;
            }
            catch (NoRecordFoundException oEx)
            {
                trNoDebit.Visible = true;
                lblNoDebitEntry.Text = oEx.Message;
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format("RowIndex : {0}. StudentId : {1}", e.CommandArgument, grdStudents.DataKeys[e.CommandArgument.ToInt()][0]));
            }
        }
    }

    /// <summary>
    /// 	This is the btnPayCautionMoney Click event.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnPayCautionMoney_Click(object sender, EventArgs e)
    {
        try
        {
            var oStudentCautionMoneyDetailsBL = new StudentCautionMoneyDetailsBL();
            string sUploadURL = "~/StudentPayFeeUI.aspx";
            int iStudentID = hidStudentId.Value.ToInt();
            DataTable odt = oStudentCautionMoneyDetailsBL.GetStudentCautionMoneyDetails(iStudentID, miAcademicYearId, miSchoolId);
            string sStudentID = odt.Rows[0]["Schoolwise_Student_Id"].ToString();
            string sStudentCautionMoneyId = odt.Rows[0]["Student_Caution_Money_Id"].ToString();

            string sAmount = odt.Rows[0]["Amount"].ToString();
            var sQueryString = new StringBuilder();
            sQueryString.AppendFormat("&StudentId={0}&Amount={1}&StudentCautionMoneyId={2}&PostBackUrl={3}", sStudentID, sAmount, sStudentCautionMoneyId, sUploadURL);
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
            hidQueryString.Value = sEncrypt;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnInauguralCertificate_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.InauguralCertificate, GetInauguralFilterString(), ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to set query string and open pop-up for fee payment of a particular student.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            const int I_COLUMN_INDEX_AMOUNT = 4;

            int iAmt = Constants.I_ZERO;
            string sRemarks = String.Empty;
            string sStudentFeeId = String.Empty;
            
            for (int iRowCnt = 0; iRowCnt < grdFeesToBePaid.Rows.Count; iRowCnt++)
            {
                var oChkPay = grdFeesToBePaid.Rows[iRowCnt].Cells[Constants.I_ONE].FindControl(S_CHECK_BOX_PAY) as CheckBox;
                //Calculate total amount (checked checkbox)
                if (!oChkPay.Checked)
                    continue;
                int iRowAmt = grdFeesToBePaid.Rows[iRowCnt].Cells[I_COLUMN_INDEX_AMOUNT].Text.ToInt();
                int iStudentFeeId = grdFeesToBePaid.DataKeys[iRowCnt][Constants.I_ZERO].ToString().ToInt();
                sStudentFeeId = sStudentFeeId + "," + iStudentFeeId;
                iAmt = iAmt + iRowAmt;

                if (!sRemarks.Contains(grdFeesToBePaid.Rows[iRowCnt].Cells[3].Text))
                    sRemarks = string.Format("{0}, {1}({2} - Rs. {3} /-)", sRemarks, grdFeesToBePaid.Rows[iRowCnt].Cells[3].Text, grdFeesToBePaid.Rows[iRowCnt].Cells[2].Text, iRowAmt);
            }
            if (sStudentFeeId.StartsWith(","))
                sStudentFeeId = sStudentFeeId.Substring(1);
            if (sRemarks.StartsWith(","))
                sRemarks = sRemarks.Substring(1);
            int iTotalAmount = txtAmtPayable.Text.ToInt() + txtLateFee.Text.ToInt();

             
            //Set query string.
            string sQueryString = String.Format("PayBtn=Pay&StudentId={0}&AmtToBePaid={1}&StudentFeeId={2}&Remarks={3}&StandardId={4}&TotalAmt={5}&StudentName={6}", hidStudentId.Value, iAmt, sStudentFeeId, sRemarks, hidStandardId.Value, iTotalAmount, lblStudentName.Text);
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            hidQueryString.Value = sEncrypt;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnOnlinePayment_Click(object sender, EventArgs e)
    {
        try
        {   
            if (hidSNSSchoolId.Value == Constants.S_YES)
            {
                btnOnlinePayment.Visible = true;
                grdFeesToBePaid.HeaderRow.Cells[1].Text = "";
            }
            else
                btnOnlinePayment.Enabled = false;

           // const int I_COLUMN_INDEX_DUEDATE = 5;

            string sRemarks = String.Empty;
            string sDueDates = String.Empty;
            int iSchoolwiseStudentFeeId = 0;
           
            if (hidSNSSchoolId.Value == Constants.S_YES)
            {
                for (int iRowCnt = 0; iRowCnt < grdFeesToBePaid.Rows.Count; iRowCnt++)
                {                  
                    var oRdoPayFee = grdFeesToBePaid.Rows[iRowCnt].Cells[Constants.I_ZERO].FindControl(S_RADIO_PAY_FEE) as RadioButton;
                    //Calculate total amount (checked checkbox)
                    if (oRdoPayFee.Checked)
                    {
                        //DateTime dtDueDate = grdFeesToBePaid.Rows[iRowCnt].Cells[I_COLUMN_INDEX_DUEDATE].Text.ToDateTime();
                        DateTime dtDueDate = grdFeesToBePaid.DataKeys[iRowCnt]["Paid_Date"].ToDateTime();
                        sDueDates = sDueDates + "," + dtDueDate;
                        iSchoolwiseStudentFeeId = grdFeesToBePaid.DataKeys[iRowCnt]["Schoolwise_Student_Fee_Id"].ToInt();
                    }
                }
            }
            else
            {
                for (int iRowCnt = 0; iRowCnt < grdFeesToBePaid.Rows.Count; iRowCnt++)
                {
                    var oChkPay = grdFeesToBePaid.Rows[iRowCnt].Cells[Constants.I_ONE].FindControl(S_CHECK_BOX_STUDENTPAY) as CheckBox;
                    //Calculate total amount (checked checkbox)
                    if (oChkPay.Checked)
                    {
                        //DateTime dtDueDate = grdFeesToBePaid.Rows[iRowCnt].Cells[I_COLUMN_INDEX_DUEDATE].Text.ToDateTime();
                        DateTime dtDueDate = grdFeesToBePaid.DataKeys[iRowCnt]["Paid_Date"].ToDateTime();
                        sDueDates = sDueDates + "," + dtDueDate;                                             
                    }
                }
            }
            if (sDueDates.StartsWith(","))
                sDueDates = sDueDates.Substring(1);
            if (sRemarks.StartsWith(","))
                sRemarks = sRemarks.Substring(1);

            //Set query string.
            string sQueryString = string.Empty;
            sQueryString = string.Format("StudentId={0}&DueDates={1}&Remarks={2}&SchoolwiseStudentFeeId={3}", hidStudentId.Value, sDueDates, sRemarks, iSchoolwiseStudentFeeId);

            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            hidQueryString.Value = sEncrypt;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used select a sibling from given list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        try
        {
            for (int iRowCount = 0; iRowCount < lstvwSiblingsDetails.Items.Count; iRowCount++)
            {
                ListViewDataItem oListViewDataItem = lstvwSiblingsDetails.Items[iRowCount];
                RadioButton rdoSelect = oListViewDataItem.FindControl("rdoSelect") as RadioButton;
                Label lblEnrollmentNo = oListViewDataItem.FindControl("lblEnrollmentNo") as Label;
                Label lblSiblingName = oListViewDataItem.FindControl("lblSiblingName") as Label;
                int iStudentId = lstvwSiblingsDetails.DataKeys[iRowCount]["YearwiseStudentId"].ToInt();

                if (rdoSelect != null && rdoSelect.Checked)
                {
                    btnSearch_Click(sender, null);
                    var oStudentBL = new StudentBL(iStudentId);
                    SetStudentInfo(oStudentBL);
                    SetInternalFeeAttributes();
                    tblStudentInputFields.Visible = true;
                    txtRegNumber.Enabled = true;
                    txtRegNumber.Text = System.Text.RegularExpressions.Regex.Replace(lblEnrollmentNo.Text + " - " + lblSiblingName.Text, @"\s+", " ");
                    odtToday = DateTime.Now.Date;
                    btnSearch_Click(sender, null);

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to show or hide images and checkbox. Also this event is used to set the query string to opening the receipt for particular transcation.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdFeesToBePaid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string sQueryString = string.Empty;
            int iRowindex = e.Row.RowIndex.ToInt();
            const int I_LNK_COL_INDEX = 12;
            const int I_COLUMN_INDEX_PAYABLE_FOR = 3;
            const int I_COLUMN_INDEX_AMT = 4;
            const int I_COLUMN_INDEX_DUE_DATE = 5;
            const int I_COLUMN_INDEX_AMT_PAID = 7;
            const int I_COLUMN_INDEX_AMT_PAYABLE = 8;
            const int I_COLUMN_INDEX_LATE_FEE = 9;
            const int I_COLUMN_INDEX_EDIT = 10;
            const int I_COLUMN_INDEX_DELETE = 11;
            const int I_COLUMN_INDEX_VIEW = 13;
            int iLastEntryId = 0;
            if (iRowindex >= 0)
            {
                int iAmtPaid = grdFeesToBePaid.DataKeys[iRowindex][Constants.I_ONE].ToString().ToInt();
                string sIsChequeBounce = grdFeesToBePaid.DataKeys[iRowindex]["Is_Cheque_Bounce"].ToString();
                bool bIsTransactionCleared = grdFeesToBePaid.DataKeys[iRowindex]["IsTransactionCleared"].ToBool();
                string sIsConcession = grdFeesToBePaid.DataKeys[iRowindex]["Is_Concession"].ToString();
                int iRefundFeeDetailsID = grdFeesToBePaid.DataKeys[iRowindex]["RefundFeeDetailsID"].ToInt();
                bool bIsArrears = grdFeesToBePaid.DataKeys[iRowindex]["Is_Arrears"].ToBool();
                bool bIsLastRefund = grdFeesToBePaid.DataKeys[iRowindex]["Is_LastRefund"].ToBool();
                bool bIsPartialPayemnt = grdFeesToBePaid.DataKeys[iRowindex]["IsPartialPayemnt"].ToBool();
                int iAccountHeaderId = grdFeesToBePaid.DataKeys[iRowindex]["HeaderId"].ToInt();
                int iRefundReceiptNo = grdFeesToBePaid.DataKeys[iRowindex]["RefundReceiptNo"].ToInt();

                var imgDelete = e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO] as ImageButton;
                var imgEdit = e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[Constants.I_ZERO] as ImageButton;
                var chkPay = e.Row.Cells[1].FindControl(S_CHECK_BOX_PAY) as CheckBox;
                var chkStudentPay = e.Row.Cells[0].FindControl(S_CHECK_BOX_STUDENTPAY) as CheckBox;
                var rdoPayFee = e.Row.Cells[1].FindControl(S_RADIO_PAY_FEE) as RadioButton;                
                string sReceiptNo = grdFeesToBePaid.DataKeys[iRowindex][2].ToString();
                var imgView = e.Row.Cells[I_COLUMN_INDEX_VIEW].Controls[Constants.I_ZERO] as ImageButton;

                string sFileName = grdFeesToBePaid.DataKeys[iRowindex]["FileName"].ToString();
                if (sFileName == null || sFileName == string.Empty)
                    imgView.Visible = false;
                else
                {
                    imgView.Visible = true;
                    string sPath = "../uploads/Fees/PaymentDocuments/" + sFileName;
                    imgView.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");
                }

                imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                
                //string dtDueDate = e.Row.Cells[I_COLUMN_INDEX_DUE_DATE].Text;

                string dtDueDate = grdFeesToBePaid.DataKeys[iRowindex]["Paid_Date"].ToString();
                

                if (hidLastEntryId.Value != String.Empty)
                    iLastEntryId = hidLastEntryId.Value.ToInt();
                int iStudentFeeId = grdFeesToBePaid.DataKeys[iRowindex][Constants.I_ZERO].ToString().ToInt();

                //If current entry is credit entry then we lock payment of fee by hiding checkbox.
                if (iAmtPaid >= 0 && sReceiptNo != Constants.I_ZERO.ToString())
                {
                    chkPay.Visible = false;
                    if (bIsPartialPayemnt)
                        e.Row.Cells[I_COLUMN_INDEX_PAYABLE_FOR].Text += " - Partial";
                }
                    
                imgDelete.Visible = false;
                imgEdit.Visible = false;

                if ((Settings.IsAaryanSchool && iAmtPaid >= 0 && sReceiptNo != Constants.I_ZERO.ToString()) && (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Admin))
                {
                    imgEdit.Visible = true;
                    imgDelete.Visible = true;
                }
               
                //For last credit entry only , we give facility for deletion
                if (iRefundFeeDetailsID != 0 && bIsLastRefund)
                    imgDelete.Visible = true;

                if (iStudentFeeId == iLastEntryId && iAmtPaid > 0 && sIsChequeBounce == Constants.C_NO.ToString())
                {
                    imgDelete.Visible = true;
                    imgEdit.Visible = true;
                   
                }

                
                var oHyperLinkField = e.Row.Cells[I_LNK_COL_INDEX].FindControl("lnkMini") as HyperLink;
                var olnkRefundLink = e.Row.Cells[I_LNK_COL_INDEX].FindControl("lnkRefundRecpt") as HyperLink;

                sQueryString = string.Format("PayBtn=Edit&ReceiptNo={0}&StudentId={1}&StandardId={2}&StudentName={3}&PaidDate={4}&AccountHeaderId={5}", sReceiptNo, hidStudentId.Value, hidStandardId.Value, lblStudentName.Text, dtDueDate, iAccountHeaderId);
                string sEncryptedQueryString = CommonUtility.EncryptQuerystring(sQueryString);

                //Here we have modified this line to change the redirection of popup. Previously it was redirecting to EditFeePoup, no by fee changes it is redirecting to PayFeePopup.
                imgEdit.Attributes.Add("onclick", string.Format("window.open('PayFeePopUp.aspx?{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=650').focus(); return false;", sEncryptedQueryString));

                //If current entry is credit entry then we set receipt link.
                if (sReceiptNo != Constants.I_ZERO.ToString() && sIsChequeBounce == Constants.C_NO.ToString() && sIsConcession == Constants.C_NO.ToString() && iRefundFeeDetailsID == 0)
                {
                    if (mbIsOldFeeDetails)
                    {
                        sQueryString = string.Format("ReceiptNo={0}&AcademicYear={1}&AccountHeaderId={2}&StudentId={3}", sReceiptNo, cmbAcademicYrId.SelectedValue,iAccountHeaderId,hidStudentId.Value);
                        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                        oHyperLinkField.Visible = true;
                        if (miSchoolId != Constants.SchoolId.ZLSP.ToInt())
                        {
                            oHyperLinkField.NavigateUrl = oHyperLinkField.NavigateUrl + sEncrypt;
                            oHyperLinkField.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;", oHyperLinkField.NavigateUrl));
                        }
                        else
                        {
                            oHyperLinkField.NavigateUrl = "";
                            oHyperLinkField.NavigateUrl = "../Admission/AdmissionFormReport.aspx?" + sEncrypt;
                            oHyperLinkField.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=500,height=400').focus(); return false;", oHyperLinkField.NavigateUrl));
                        }                        
                        e.Row.ForeColor = Color.FromArgb(170, 170, 170);
                        imgDelete.Visible = false;
                        
                        imgEdit.Visible = false;
                        
                    }
                    else
                    {
                        sQueryString = string.Format("ReceiptNo={0}&AccountHeaderId={1}&StudentId={2}", sReceiptNo, iAccountHeaderId,hidStudentId.Value);
                        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                        oHyperLinkField.Visible = true;
                        if (miSchoolId != Constants.SchoolId.ZLSP.ToInt())
                        {
                            oHyperLinkField.NavigateUrl = oHyperLinkField.NavigateUrl + sEncrypt;
                            oHyperLinkField.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;", oHyperLinkField.NavigateUrl));
                        }
                        else
                        {
                            oHyperLinkField.NavigateUrl = "";
                            oHyperLinkField.NavigateUrl = "../Admission/AdmissionFormReport.aspx?" + sEncrypt;
                            oHyperLinkField.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=500,height=400').focus(); return false;", oHyperLinkField.NavigateUrl));
                        }                        
                        e.Row.ForeColor = Color.FromArgb(170, 170, 170);

                        if(Settings.ShowRefundOptionForAll ||(!Settings.ShowRefundOptionForAll && mbIsLeftStudent))
                            tdlnkRefund.Visible = true;
                    }
                }
                else
                    oHyperLinkField.Visible = false;

                if (iRefundReceiptNo != Constants.I_ZERO)
                {
                    olnkRefundLink.Visible = true;
                    sQueryString = string.Format("ReceiptNo={0}&StudentId={1}&IsRefundFee={2}&AccountHeaderId={3}", iRefundReceiptNo, hidStudentId.Value, 1, iAccountHeaderId);
                    string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                    olnkRefundLink.NavigateUrl = olnkRefundLink.NavigateUrl + sEncrypt;
                    olnkRefundLink.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;", olnkRefundLink.NavigateUrl));
                }
                else
                    olnkRefundLink.Visible = false;

                if (sIsChequeBounce == Constants.C_YES.ToString())
                {
                    e.Row.CssClass = "BounceCheque";
                    e.Row.ToolTip = Resources.LocalizedResources.BouncedChequeTransactions;
                }
                else if (iRefundFeeDetailsID != 0)
                {
                    e.Row.CssClass = "ClsGridNA";
                    chkPay.Visible = false;
                }
                else
                {
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAYABLE].BackColor = Color.PowderBlue;
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAYABLE].Style["padding-right"] = "5px";
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAID].BackColor = Color.FromArgb(179, 222, 242);
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAID].Style["padding-right"] = "5px";
                    e.Row.Cells[I_COLUMN_INDEX_AMT].Style["padding-right"] = "5px";
                }

                string sLateFee = e.Row.Cells[I_COLUMN_INDEX_LATE_FEE].Text;
                if (sLateFee == Constants.I_ZERO.ToString())
                    e.Row.Cells[I_COLUMN_INDEX_LATE_FEE].Text = "-";
                else
                    e.Row.Cells[I_COLUMN_INDEX_LATE_FEE].CssClass = "LateFee";

                if (dtDueDate.ToDateTime() < DateTime.Now.Date && sReceiptNo == Constants.I_ZERO.ToString() && (iRefundFeeDetailsID == 0) && sIsConcession == Constants.C_NO.ToString())
                {
                    e.Row.CssClass = "PendingFees";
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAYABLE].BackColor = Color.FromArgb(254, 234, 186);
                    e.Row.Cells[I_COLUMN_INDEX_AMT_PAID].BackColor = Color.FromArgb(254, 234, 186);
                }

                if (!IsStudentLogin && !bIsTransactionCleared)
                    e.Row.CssClass = "UnclearedChq";

                if (bIsArrears)
                    e.Row.Font.Bold = true;

                if (odtToday == null)
                    odtToday = DateTime.Now.Date;
                
                
                if (sReceiptNo == Constants.I_ZERO.ToString() && (iRefundFeeDetailsID == 0) && sIsConcession == Constants.C_NO.ToString() && !mbIsOldFeeDetails)
                {
                    if (dtDueDate.ToDateTime() != odtToday)
                    {
                        if (hidSNSSchoolId.Value == Constants.S_YES)
                        {
                            rdoPayFee.Visible = true;
                            chkPay.Visible = false;
                            grdFeesToBePaid.HeaderRow.Cells[1].Text = "";
                        }
                        else
                        {
                            chkStudentPay.Visible = true;
                            odtToday = dtDueDate.ToDateTime();
                        }
                    }
                    else
                        chkStudentPay.Visible = false;
                }
                else
                    chkStudentPay.Visible = false;

                if (mbIsPaidForNextYear && mbIsInternalFeePaidForNextYear)
                    lblNextYearPayment.Text = "*Student Fee and Internal Fee for next year is paid.";
                else if (mbIsPaidForNextYear && !mbIsInternalFeePaidForNextYear)
                    lblNextYearPayment.Text = "*Student Fee for next year is paid.";
                else if (!mbIsPaidForNextYear && mbIsInternalFeePaidForNextYear)
                    lblNextYearPayment.Text = "*Internal Fee for next year is paid.";
                else
                    lblNextYearPayment.Text = string.Empty;

                if (mbIsLastYearInternalFeePending && msIsPendingForLastYear != string.Empty)
                {
                    if (msAcademicYear == msIsPendingForLastYear)
                    {
                        lblLastPayment.Text = "*Student Fee and Internal Fee for " + msAcademicYear + " is pending.";
                    }
                    else
                        lblLastPayment.Text = "*Student Fee for " + msIsPendingForLastYear + " is pending and <br/>Internal Fee for  " + msAcademicYear + " is pending.";
                }
                else if (mbIsLastYearInternalFeePending)
                    lblLastPayment.Text = "*Internal Fee for " + msAcademicYear + " is pending.";
                else if (msIsPendingForLastYear != string.Empty)
                {
                    if (Settings.RestrictNewPaymentIfOldPaymentIsPending && moUserRole == Constants.UserRoles.Student)
                        hidRestrictCurrentYearPayment.Value = Constants.S_YES;
                   
                    lblLastPayment.Text = "*Student Fee for " + msIsPendingForLastYear + " is pending.";
                }

                if (msIsPendingForLastYear != string.Empty && ((moUserRole != Constants.UserRoles.Student && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES) || moUserRole == Constants.UserRoles.Admin))
                {
                    hlnkOldFeeRecord.Visible = true;
                    SetOldFeeDetailstUrl();

                }

                if ((moSchool == Constants.SchoolId.VPMCPS || moSchool == Constants.SchoolId.PPS) && e.Row.Cells[I_COLUMN_INDEX_AMT_PAYABLE].Text.ToString() == "1")
                {
                    chkPay.Visible = false;
                    chkStudentPay.Visible = false;
                }

                if (grdFeesToBePaid.DataKeys[iRowindex]["HideInstalment"].ToBool())
                    e.Row.Cells[I_COLUMN_INDEX_DUE_DATE].Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	Sets the Leave message for the student, if student is on leave.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdFeesToBePaid_DataBound(object sender, EventArgs e)
    {
        try
        {
            int iStudentId = hidStudentId.Value.ToInt();
            string sLeaveMessage = StudentFeeDetailsBL.IsOnLeave(iStudentId, miSchoolId, miAcademicYearId);
            if (sLeaveMessage != "0")
            {
                btnOnlinePayment.Enabled = false;
                btnOnlineCautionMoneyPayment.Enabled = false;
                hidIsOnLeave.Value = "Y";
                lblLeaveMessage.Visible = true;
                lblLeaveMessage.Text = sLeaveMessage;

                btnOnlineInternalFeePayment.Enabled = false;
            }
            else
            {

                btnOnlinePayment.Enabled = true;
                if (Settings.EnableOnlinePaymentForCautionMoney)
                    btnOnlineCautionMoneyPayment.Enabled = true;
                lblLeaveMessage.Visible = false;

                if (Settings.EnableOnlinePaymentForInternalFee && IsStudentLogin && Settings.EnabledOnlineFee)
                     btnOnlineInternalFeePayment.Enabled = true;
                else
                     btnOnlineInternalFeePayment.Visible = false;
                
            }

            if (IsStudentLogin)
            {
                tdUnclearedTransLegend.Visible = false;
                tdUnclearedTransLabel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to handle the selection/de-selection of fee type in the fee grid.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void ChkBoxStudentPay_Checked(object sender, EventArgs e)
    {
        try
        {
          //  const int I_COLUMN_INDEX_DUE_DATE = 5;
            const int I_COLUMN_INDEX_AMT_PAID = 6;
            const int I_COLUMN_INDEX_AMT_PAYABLE = 7;
            const int I_COLUMN_INDEX_CHECKBOX_PAY = 1;
            bool bIsEnable = false;

            var oChkBoxStudentPay = sender as CheckBox;
            var oGridViewRow = oChkBoxStudentPay.Parent.Parent as GridViewRow;
            int iRowIndex = oGridViewRow.RowIndex;
            
            //odtToday = grdFeesToBePaid.Rows[iRowIndex].Cells[I_COLUMN_INDEX_DUE_DATE].Text.ToDateTime();
            odtToday = grdFeesToBePaid.DataKeys[iRowIndex]["Paid_Date"].ToDateTime();

            for (int iCount = 0; iCount < grdFeesToBePaid.Rows.Count; iCount++)
            {
                string sIsConcession = grdFeesToBePaid.DataKeys[iCount]["Is_Concession"].ToString();
                int iRefundFeeDetailsID = grdFeesToBePaid.DataKeys[iCount]["RefundFeeDetailsID"].ToInt();
                string sReceiptNo = grdFeesToBePaid.DataKeys[iCount][2].ToString();

                var ChkBoxPay = grdFeesToBePaid.Rows[iCount].Cells[I_COLUMN_INDEX_CHECKBOX_PAY].FindControl("ChkBoxPay") as CheckBox;

                var oCheckBox = grdFeesToBePaid.Rows[iCount].Cells[I_COLUMN_INDEX_STUDENTCHKBOX].FindControl("ChkBoxStudentPay") as CheckBox;
                var oRdoPayFee = grdFeesToBePaid.Rows[iCount].Cells[I_COLUMN_INDEX_STUDENTCHKBOX].FindControl("rdoPayFee") as CheckBox;
                
                if (oCheckBox.Checked)
                  {
                      bIsEnable = true;
                      btnOnlinePayment.Enabled = true;

                  }               

                if (sReceiptNo != Constants.I_ZERO.ToString() || (iRefundFeeDetailsID != 0) || sIsConcession != Constants.C_NO.ToString())
                    continue;
                if (oChkBoxStudentPay.Checked)
                {
                    if (grdFeesToBePaid.DataKeys[iCount]["Paid_Date"].ToDateTime() == odtToday)
                    {
                        if (moUserRole == Constants.UserRoles.Student)
                            grdFeesToBePaid.Rows[iCount].ForeColor = Color.DarkOrange;
                        ChkBoxPay.Checked = true;
                    }
                    //btnOnlinePayment.Enabled = true;
                    bIsEnable = true;
                }
                else
                {
                    if (grdFeesToBePaid.DataKeys[iCount]["Paid_Date"].ToDateTime() == odtToday)
                        ChkBoxPay.Checked = false;
                    if (grdFeesToBePaid.DataKeys[iCount]["Paid_Date"].ToDateTime() == odtToday && odtToday >= DateTime.Now && moUserRole == Constants.UserRoles.Student)
                        grdFeesToBePaid.Rows[iCount].ForeColor = Color.Black;
                    else if (grdFeesToBePaid.DataKeys[iCount]["Paid_Date"].ToDateTime() == odtToday && odtToday < DateTime.Now && moUserRole == Constants.UserRoles.Student)
                    {
                        grdFeesToBePaid.Rows[iCount].CssClass = "PendingFees";
                        grdFeesToBePaid.Rows[iCount].Cells[I_COLUMN_INDEX_AMT_PAYABLE].BackColor = Color.FromArgb(254, 234, 186);
                        grdFeesToBePaid.Rows[iCount].Cells[I_COLUMN_INDEX_AMT_PAID].BackColor = Color.FromArgb(254, 234, 186);
                    }
                }                
            }
            if (!bIsEnable)
                btnOnlinePayment.Enabled = false;

            btnOnlinePayment.Enabled = hidIsOnLeave.Value != Constants.S_YES;

            //if(Settings.EnableOnlinePaymentForCautionMoney)
            //    btnOnlineCautionMoneyPayment.Visible = hidIsOnLeave.Value != Constants.S_YES;

            if (Settings.EnableOnlinePaymentForInternalFee)
            {
                if (IsStudentLogin && hidIsOnlineInternalFeeApplicable.Value == Constants.S_ONE && Settings.EnabledOnlineFee)
                    btnOnlineInternalFeePayment.Visible = hidIsOnLeave.Value != Constants.S_YES;
                else
                    btnOnlineInternalFeePayment.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to show or hide pay button depends on cheque is deposited or not.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdPostdatedCheque_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            const string S_BUTTON_PAY = "btnCheque";
            const string S_STATUS_PAID = "Paid";
            int iRowindex = e.Row.RowIndex;
            if (iRowindex >= 0)
            {
                int iAmt = e.Row.Cells[I_COLUMN_INDEX_CHEQUE_AMT].Text.ToInt();
                int iChequeId = grdPostdatedCheque.DataKeys[iRowindex][Constants.I_ZERO].ToString().ToInt();
                string sIsChequeBounce = grdPostdatedCheque.DataKeys[iRowindex]["Is_Cheque_Bounce"].ToString();
                int iTotalAmount = txtAmtPayable.Text.ToInt() + txtLateFee.Text.ToInt();

                //Set query string.
                string sQueryString = string.Format("PayBtn=GridPay&StudentId={0}&AmtToBePaid={1}&PDC_Id={2}&StandardId={3}&TotalAmt={4}", hidStudentId.Value, iAmt, iChequeId, hidStandardId.Value, iTotalAmount);
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                string sStatus = grdPostdatedCheque.DataKeys[iRowindex][Constants.I_ONE].ToString();

                //Check user role and show or hide pay button according to user.
                if (CheckIfAdminUser() || Convert.ToChar(hidCanEdit.Value) == Constants.C_YES)
                {
                    var btnPay = e.Row.Cells[I_COLUMN_INDEX_PAY].FindControl(S_BUTTON_PAY) as Button;
                    if (sStatus == S_STATUS_PAID || sIsChequeBounce == Constants.C_YES.ToString())
                    {
                        btnPay.Visible = false;
                        if (!lblPDCDetails.Visible)
                        {
                            lblPDCDetails.Visible = false;
                            lblPDCDetails.Text = String.Empty;
                        }
                    }
                    else
                    {
                        lblPDCDetails.Visible = true;
                        lblPDCDetails.Text = Resources.LocalizedResources.PostDatedChequeAreAvailable;
                        btnPay.Visible = true;
                        btnPay.Attributes.Add("onclick", string.Format("window.open('PayFeePopUp.aspx?{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=650').focus(); return false;", sEncrypt));
                    }
                }
                else
                {
                    if (sStatus != S_STATUS_PAID)
                        e.Row.ForeColor = Color.Blue;
                }
                if (sIsChequeBounce == Constants.C_YES.ToString())
                {
                    e.Row.ForeColor = Color.Red;
                    e.Row.ToolTip = "Cheque is bounced.";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to delete last credit entry.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdFeesToBePaid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            const string S_DELETE_COMMAND = "Delete_FeeDetails";
            const int I_COLUMN_INDEX_RECEIPTNO = 2;
            int iRowindex = e.CommandArgument.ToString().ToInt();
            int iStudentId = hidStudentId.Value.ToInt();
            int iRefundFeeDetailsID = grdFeesToBePaid.DataKeys[iRowindex]["RefundFeeDetailsID"].ToInt();
            bool bIsLastRefund = grdFeesToBePaid.DataKeys[iRowindex]["Is_LastRefund"].ToBool();
            int iAccountHeaderId = grdFeesToBePaid.DataKeys[iRowindex]["HeaderId"].ToInt();

            switch (e.CommandName)
            {
                case S_DELETE_COMMAND:

                    var oStudentFeeDetailsBL = new StudentFeeDetailsBL();

                    //If transcation is not refund transcation that time iRefundFeeDetailsID = 0
                    //given transcation is last transcation of selected student.
                    // Other wise refund delete transcation 
                    if (iRefundFeeDetailsID == 0 && !bIsLastRefund)
                    {
                        string sReceiptNo = grdFeesToBePaid.DataKeys[iRowindex][I_COLUMN_INDEX_RECEIPTNO].ToString();
                        string sStudentFeeIdsXML = String.Empty;

                        // We get the FeeVoucher particulars for the given Student and ReceiptNo.
                        // This needs to be performed now(before fee being delete in the db) becuase after deletion,
                        // It is difficult to get the correct particulars (since there could be multiple deleted entries).
                        if (IsAccountsModuleEnabled)
                        {
                            var oVoucherClient = new AccountVoucherClient();
                            try
                            {
                                oVoucherClient.Open();
                                List<FeeVoucherParticulars> lstFeeParticulars = oVoucherClient.GetFeePaymentParticulars(miSchoolId, miAcademicYearId, miFinancialYearId, iStudentId, sReceiptNo);
                                sStudentFeeIdsXML = CommonUtility.GetXMLForList(lstFeeParticulars);
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while getting FeeVoucher particulars. StudentId : {0}. ReceiptNo : {1}", iStudentId, sReceiptNo));
                            }
                            finally
                            {
                                if (oVoucherClient.State != CommunicationState.Faulted)
                                    oVoucherClient.Close();
                            }
                        }

                        oStudentFeeDetailsBL.DeleteLastCreditEntry(iStudentId, sReceiptNo, iAccountHeaderId, miUserId);

                        // Now we actually delete the previously collected particulars from the FeeVoucher.
                        if (IsAccountsModuleEnabled)
                        {
                            var oVoucherClient = new AccountVoucherClient();
                            try
                            {
                                oVoucherClient.Open();
                                oVoucherClient.DeleteFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, iStudentId, sReceiptNo, sStudentFeeIdsXML, miUserId, true);
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while deleting a fee payment. StudentId : {0}. ReceiptNo : {1}", iStudentId, sReceiptNo));
                            }
                            finally
                            {
                                if (oVoucherClient.State != CommunicationState.Faulted)
                                    oVoucherClient.Close();
                            }
                        }
                    }
                    else
                        oStudentFeeDetailsBL.DeleteRefundFeeDetails(iRefundFeeDetailsID);
                    FillAmtToBePaidGrid(hidStudentId.Value.ToInt());
                    FillPostdatedChequeGrid();
                    break;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to set the paging properties when page is changed.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            grdStudents.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used to fill the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                string sLeftDate = grdStudents.DataKeys[e.Row.RowIndex]["SchoolLeft_Date"].ToString();
                if (sLeftDate != Constants.S_EMPTY_STRING)
                    e.Row.Style.Add(HtmlTextWriterStyle.Color, "red");
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
                var pageLabel = pagerRow.Cells[0].FindControl("CurrentPageLabel") as Label;

                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdStudents.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        var item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdStudents.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }
                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + 1;
                    // Update the Label control with the current page information.
                    pageLabel.Text = (Resources.LocalizedResources.PageNo + " " + currentPage + Resources.LocalizedResources.Of + " " + grdStudents.PageCount + " " + Resources.LocalizedResources.OutOflst);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Here we directly onpen sibling details if there exists only one sibling for selected student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSibling_Click(object sender, EventArgs e)
    {
        try
        {
            if (!hidSiblingId.Value.IsNullOrEmpty())
            {
                btnSearch_Click(sender, null);
                var oStudentBL = new StudentBL(hidSiblingId.Value.ToInt());
                SetStudentInfo(oStudentBL);
                SetInternalFeeAttributes();
                tblStudentInputFields.Visible = true;
                txtRegNumber.Enabled = true;
                txtRegNumber.Text = System.Text.RegularExpressions.Regex.Replace(hidSearchDetails.Value, @"\s+", " ");
                odtToday = DateTime.Now.Date;
                btnSearch_Click(sender, null);
            }
        }
        catch (NoRecordFoundException oEx)
        {
            trNoDebit.Visible = true;
            lblNoDebitEntry.Text = oEx.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to pay caution money online.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOnlineCautionMoneyPayment_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = string.Format("StudentId={0}&DueDates={1}&Remarks={2}&SchoolwiseStudentFeeId={3}&IsOnlineCautionMoneyPayment=1", hidStudentId.Value, string.Empty, string.Empty, 0);
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            hidQueryString.Value = sEncrypt;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    private string GetInauguralFilterString()
    {
        return "(usp_GetDetsilsForInauguralCertificate.School_Id}=" + miSchoolId + "AND usp_GetDetsilsForInauguralCertificate.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetDetsilsForInauguralCertificate.Student_Id}=" + hidSchoolwiseStudentId.Value + "AND usp_GetDetsilsForInauguralCertificate.Standard_Id}=" + hidNewStdId.Value + " AND usp_GetDetsilsForInauguralCertificate.Division_Id}=" + hidStdDivId.Value.ToInt() + ") @";
    }
    
    /// <summary>
    /// This method is used to update Fee structure link status.
    /// </summary>
    private void ChangeFeeStrutcureLinkStatus()
    {
        FeeStructureLinkBL oFeeStructureLinkBL = new FeeStructureLinkBL(miSchoolId, miUserId, miAcademicYearId);
        bool bShowFeeStructureOfNextYear = SchoolBase.Settings.ShowFeeStructureOfNextYear;

        Dictionary<string, string> dirFeeLinkFileNames = oFeeStructureLinkBL.Get(miSchoolId, miAcademicYearId, miUserId, bShowFeeStructureOfNextYear);

        //code for current year fee structure pdf url.
        if (dirFeeLinkFileNames.ContainsKey("CurrentYearFeeStructureUrl"))
        {
            string sNewFileName = S_FOLDER_PATH + dirFeeLinkFileNames["CurrentYearFeeStructureUrl"];
            hlnkFeestructure.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            hlnkFeestructure.Visible = true;
        }
        else
        {
            hlnkFeestructure.Visible = false;
        }

        //code for next year fee structure pdf url.
        if (dirFeeLinkFileNames.ContainsKey("NextYearFeeStructureUrl") && bShowFeeStructureOfNextYear)
        {
            string sNewFileName = S_FOLDER_PATH + dirFeeLinkFileNames["NextYearFeeStructureUrl"];
            hlnkNextYearFeeStructure.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            hlnkNextYearFeeStructure.Visible = true;

            if(miSchoolId == Constants.SchoolId.PPS.ToInt())
                hlnkNextYearFeeStructure.Text = "Revised Fee Structure for 2026-2027";
        }
        else
        {
            hlnkNextYearFeeStructure.Visible = false;
        }
        //if (miSchoolId == Constants.SchoolId.PPS.ToInt() && moUserRole == Constants.UserRoles.Student)
        //{
        //    hlnkNextFeeStructure.Attributes.Add("onclick", "OpenWindow('" + "../DOWNLOADS/Fee Structure 2023-2024.pdf" + "'); return false;");
        //    hlnkNextFeeStructure.Visible = true;
        //}
        //else
        //{
        //    hlnkNextFeeStructure.Visible = false;
        //}

    }

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);

        }
        return sSourcePageUrl;
    }
    /// <summary>
    /// This method is used to set the internal fee popup attributes.
    /// </summary>
    private void SetInternalFeeAttributes()
    {
        if (!hidSchoolwiseStudentId.Value.IsNullOrEmpty())
        {
            InternalFeeDetailsBL oInternalFeeDetailsBL = new InternalFeeDetailsBL();
            List<InternalFeeDebitDetails> lstInternalFeeDebitDetails = oInternalFeeDetailsBL.GetInternalFeeDebitDetails(miSchoolId, miAcademicYearId, hidSchoolwiseStudentId.Value.ToInt(),false);
            if (lstInternalFeeDebitDetails.Count > 0)
            {
                for (int i = 0; i < lstInternalFeeDebitDetails.Count; i++)
                {
                    if (lstInternalFeeDebitDetails[i].DebitCredit.ToString().Trim() == "Debit" && lstInternalFeeDebitDetails[i].PaidDate.Date < System.DateTime.Today.Date && lstInternalFeeDebitDetails[i].IsDueDateApplicable)
                    {

                        hlnkInternalFee.Style.Add("color", "Red");
                        hlnkInternalFee.Style.Add("font-weight", "750");
                        break;
                    }
                    else
                        hlnkInternalFee.Style.Add("color", "#55713d");

                }

                string sQueryString = String.Format("StudentId={0}&StudentName={1}&Amount={2}&RegNo={3}&pIndex={4}&IsNextYearFeePayment={5}", hidSchoolwiseStudentId.Value, lblStudentName.Text, null, null, -9999, 0);
                hlnkInternalFee.Attributes.Add("onclick", "if(!OpenInternalFeePopup( 'PayInternalFeePopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");
            }
            else
                td1.Visible = false;
        }
    }

    /// <summary>
    /// 	This method is used to hide or show controls according to condition.
    /// </summary>
    /// <param name="abFlag"> </param>
    private void ShowHideFields(bool abFlag)
    {
        trCheque.Visible = abFlag;
        trAmtToBePaid.Visible = abFlag;
        trPay.Visible = abFlag;
        txtRegNumber.Enabled = !abFlag;
        tblStudentInfo.Visible = abFlag;
        tblLegend.Visible = abFlag;
        trTotalAmt.Visible = abFlag;
        trCheQueSummary.Visible = abFlag;
        grdPostdatedCheque.Columns[I_COLUMN_INDEX_STATUS].Visible = false;
        if (grdPostdatedCheque.Rows.Count == 0)
        {
            trCheQueSummary.Visible = false;
            trCheque.Visible = false;
            lblPDCDetails.Visible = false;
        }
        if (grdFeesToBePaid.Rows.Count == 0 && !txtRegNumber.Enabled)
        {
            trAmtToBePaid.Visible = true;
            trCheQueSummary.Visible = false;
            trCheque.Visible = false;
            trTotalAmt.Visible = false;
            trPay.Visible = false;
        }
    }

    /// <summary>
    /// 	This method is used to fill amount to be paid grid.
    /// </summary>
    private void FillAmtToBePaidGrid(int aiStudentId)
    {
        int iStudentId = aiStudentId;
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        FillSiblings();
        DateTime dtToday = DateTime.Today;
        DataSet oDsDebitDetails = oStudentFeeDetailsBL.GetStudentFeeDetails(iStudentId, dtToday, moUserRole.ToInt(), false);
        if (oDsDebitDetails.Tables[0].Rows.Count == 0 && !hidLeftDate.Value.IsNullOrEmpty())
            throw new NoRecordFoundException(Resources.LocalizedResources.FeeRecordsAreNotAvailableMessage.Replace("%DATE%", hidLeftDate.Value).Replace("%NAME%", lblStudentName.Text.Trim()));

        // throw new NoRecordFoundException(S_LEFT_STUDENT.Replace("%DATE%", hidLeftDate.Value).Replace("%NAME%", lblStudentName.Text.Trim()));            
        DataTable dtFeeStatus = oDsDebitDetails.Tables[2];
        mbIsPaidForNextYear = oDsDebitDetails.Tables[5].Rows[0][0].ToBool();
        mbIsInternalFeePaidForNextYear = oDsDebitDetails.Tables[5].Rows[0]["IsNextYearInternalFeePaid"].ToBool();
        bool bIsFinalYear = oDsDebitDetails.Tables[6].Rows[0][0].ToBool();
        string sAmtPaid = dtFeeStatus.Rows[Constants.I_ZERO][Constants.I_ZERO].ToString();
        string sAmtPayable = dtFeeStatus.Rows[Constants.I_ZERO][Constants.I_ONE].ToString();
        string sLateFee = dtFeeStatus.Rows[Constants.I_ZERO][2].ToString();
        string sRefundFee = dtFeeStatus.Rows[Constants.I_ZERO]["RefundAmt"].ToString();
        string sPaidLateFee = dtFeeStatus.Rows[Constants.I_ZERO]["PaidLatefeeAmount"].ToString();
        string sApplicableFee = dtFeeStatus.Rows[Constants.I_ZERO]["TotalApplicable"].ToString();
        mbIsLeftStudent = oDsDebitDetails.Tables[9].Rows[0][0].ToBool();

        if (oDsDebitDetails.Tables[8].Rows[0]["IsOnlineInternalPaymentApplicable"].ToBool())
            hidIsOnlineInternalFeeApplicable.Value = Constants.S_ONE;
        else
            hidIsOnlineInternalFeeApplicable.Value = Constants.S_ZERO;

        hidHideCautionMoneyButton.Value = (oDsDebitDetails.Tables[9].Rows[0]["HideOnlineCautionMoneyButton"].ToBool() ? Constants.S_YES : Constants.S_NO);

        msIsPendingForLastYear = StudentFeeDetailsBL.PreviousFeesPending(miSchoolId, miAcademicYearId, iStudentId);

        if (Settings.StudentAbsentCount > Constants.I_ZERO)
            lblStudentAbsent.Text = StudentFeeDetailsBL.IsStudentAbsent(miSchoolId, miAcademicYearId, iStudentId);

        hidIsCautionMoneyPaid.Value = (oDsDebitDetails.Tables[7].Rows[0][0].ToBool() ? Constants.S_ONE : Constants.S_ZERO);
        hidStudIdForCautionMoney.Value = oDsDebitDetails.Tables[7].Rows[0][1].ToString();

        mbIsLastYearInternalFeePending = StudentFeeDetailsBL.PreviousInternalFeesPending(miSchoolId, miAcademicYearId, iStudentId, out msAcademicYear);
        string sConcessionRule = String.Empty;
        hidIsRTEStudent.Value = dtFeeStatus.Rows[Constants.I_ZERO][Constants.I_SEVEN].ToString();
        hidStdDivId.Value = dtFeeStatus.Rows[Constants.I_ZERO]["StandardDivisionId"].ToString();
		hidNewStdId.Value = dtFeeStatus.Rows[Constants.I_ZERO]["NewStdId"].ToString();
        hidSchoolwiseStudentId.Value = oDsDebitDetails.Tables[7].Rows[Constants.I_ZERO]["SchoolwiseStudentId"].ToString();
		if (oDsDebitDetails.Tables[Constants.I_THREE].Rows.Count > Constants.I_ZERO)
            sConcessionRule = oDsDebitDetails.Tables[Constants.I_THREE].Rows[Constants.I_ZERO]["ConcessionRule"].ToString();


        txtAmtPaid.Text = sAmtPaid;
        txtAmtPayable.Text = sAmtPayable;
        txtLateFee.Text = sLateFee;
        lblTotalFee.Text = sApplicableFee;

        var dr = oDsDebitDetails.Tables[0].Select("(Fee_Type = 'Tuition Fee' or Fee_Type = 'Term Fee') and Amount_Paid = 0");
        if (dr.Length == 0)
            hidFeePayable.Value = "0";
        else
        {
            var amt = dr.Sum(dt => dt.Field<int>("Amount"));
            hidFeePayable.Value = amt.ToString();
        }

        if (Settings.IsCautionMoneyApplicable)
        {
            if (hidCautionMoneyButton.Value != Constants.S_YES && hidIsCautionMoneyExist.Value == Constants.S_YES && !(hidIsRTEStudent.Value == "True"))
                btnPayCautionMoney.Visible = true;
            else
                btnPayCautionMoney.Visible = false;

            if (hidIsRTEStudent.Value.ToLower() == "false" && hidIsCautionMoneyExist.Value == Constants.S_YES)
            {
                btnPayCautionMoney.Text = (hidIsCautionMoneyPaid.Value == Constants.S_ONE ? "Show Caution Money Receipt" : "Pay Caution Money");
                btnPayCautionMoney.Attributes.Remove("onclick");
                if (hidIsCautionMoneyPaid.Value == Constants.S_ONE)
                {
                    string sQueryStr = string.Format( "StudentId={0}&StudentCautionMoneyId={1}", hidStudIdForCautionMoney.Value,0 ); 
                    sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);
                    btnPayCautionMoney.Attributes.Add("onclick", string.Format("window.open('CautionMoneyReciept.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=470'); return false;", sQueryStr));
                }
            }
        }

        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (moUserRole != Constants.UserRoles.Student && (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.RestrictUsersForFeeUpdation.ToInt() && ru.UserId == miUserId).Any()))
        {
            btnPay.Visible = false;
            btnPayCautionMoney.Visible = false;
            tdPDCOpen.Visible = false;
            tdSPOpen.Visible = false;
            grdFeesToBePaid.Columns[10].Visible = false;
            grdFeesToBePaid.Columns[11].Visible = false;
            tdBankChallan.Visible = false;
            tdSms.Visible = false;
            tdBank.Visible = false;
            tdResetRecipt.Visible = false;
        }

        if (moUserRole != Constants.UserRoles.Student && (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowUsersonlyForPayFee.ToInt() && ru.UserId == miUserId).Any()))
        {            
            grdFeesToBePaid.Columns[10].Visible = false;
            grdFeesToBePaid.Columns[11].Visible = false;            
        }

        if (sRefundFee.ToInt() == 0)
        {
            tdrefundAmt.Visible = false;
            tdrefund.Visible = false;
        }
        else
        {
            tdrefundAmt.Visible = true;
            tdrefund.Visible = true;
        }

        lblRefund.Text = sRefundFee;
        SetGridViewDateColumnProperties();

        if (oDsDebitDetails.Tables[Constants.I_ONE].Rows.Count > 0)
            hidLastEntryId.Value = oDsDebitDetails.Tables[Constants.I_ONE].Rows[Constants.I_ZERO][Constants.I_ZERO].ToString();
        tdlnkRefund.Visible = false;

        DataView oDataView = oDsDebitDetails.Tables[Constants.I_ZERO].DefaultView;

        if (miSchoolId == Constants.SchoolId.PKIS.ToInt() && moUserRole == Constants.UserRoles.Student)
            oDataView = oDsDebitDetails.Tables[Constants.I_ZERO].Select("Fee_type<>'Transport Fees'").CopyToDataTable().DefaultView;

        grdFeesToBePaid.DataSource = oDataView;
        grdFeesToBePaid.DataBind();

        DataRow[] drArr = oDsDebitDetails.Tables[Constants.I_ZERO].Select("FileName<>''");
        if (drArr.Length == 0 && grdFeesToBePaid.Rows.Count > 0)
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_VIEW].Visible = false;
        else if(moUserRole != Constants.UserRoles.Student)
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_VIEW].Visible = true;

        /*if (miSchoolId == Constants.SchoolId.PPSH.ToInt() && oDataView.Count > 0 && moUserRole == Constants.UserRoles.Student)
            grdFeesToBePaid.Columns[5].Visible = false;*/

        int iExcudeLateAmt = sAmtPaid.ToInt() - sPaidLateFee.ToInt();
        if (sAmtPaid != String.Empty && sRefundFee.ToInt() == iExcudeLateAmt)
            tdlnkRefund.Visible = false;
        if (sConcessionRule != String.Empty)
        {
            lblConcessionRule.Visible = true;
            lblConcessionRule.Text = "* " + sConcessionRule;
        }
        else
            if (hidIsRTEStudent.Value == "True")
            {
                lblConcessionRule.Visible = true;
                lblConcessionRule.Text = S_RTESTUDENT_MESSAGE;
            }
            else
                lblConcessionRule.Visible = false;


        /**
         *	Logic to show/hide the link for paying fees for next academic year.
         */

        // First we hide the link directly. It is only shown if certain conditions are met.
        tdNextYearLink.Visible = false;
        tdNextYearInternalFee.Visible = false;

        // If EnableAdvanceFeePayment setting is true for the school.
        if (hidIsRTEStudent.Value == "False")
        {
            if (Settings.EnableAdvanceFeePayment)
            {
                /**
                 * We set the query string and show the link only if there is a higher standard configured in the next academic year.
                 * For e.g. if student is currently studying in 3rd, but in next academic year 4th std is not configured, then we do not show the link.
                 * "StandardID" is returned null in the following cases:
                 *	1. If final year is generated for the next academic year. 
                 *	2. If "Standardwise Fees" are not configured for the next academic year.
                 *	3. If "Stardardwise Fees" are not configured for the standard of the student in next academic year. (i.e. when student is in 3rd right now, and fees are not configured for 4th std.)
                 */
                if (oDsDebitDetails.Tables[Constants.I_FOUR] != null && oDsDebitDetails.Tables[Constants.I_FOUR].Rows.Count > 0 && oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["StandardID"] != null && oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["StandardID"].ToString() != string.Empty)
                {
                    if (moUserRole != Constants.UserRoles.Student || (moUserRole == Constants.UserRoles.Student && Settings.EnableAdvanceFeePaymentForStudent && oDsDebitDetails.Tables[9].Rows[0]["ShowMidYearPaymentOption"].ToBool() == true))
                    {
                        string sQueryString = string.Format("StudentId={0}&Academic_Year_ID={1}&StandardID={2}&StudentIdQurStr={3}",
                                                             oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["MasterStudentID"],
                                                             oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["Academic_Year_ID"],
                                                             oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["StandardID"],
                                                             hidStudentId.Value);
                        OpenNextYearPopUp(sQueryString);
                        tdNextYearLink.Visible = true;
                    }
                }
                /**
                 * If Online fee payment facility is enabled and if the currently logged in user is a student,
                 * and final year has been generated, then we show the link.
                 */
                else if (moUserRole == Constants.UserRoles.Student && bIsFinalYear && Settings.EnabledOnlineFee && Settings.EnableAdvanceFeePaymentForStudent)
                {
                    DataTable oDataTable = oStudentFeeDetailsBL.GetFinalAcademicYearDetails(miSchoolId, iStudentId);

                    string sQueryString = string.Format("StudentId={0}&IsFinalYear={1}&StandardID={2}&Academic_Year_ID={3}",


                                                         oDataTable.Rows[0]["MasterStudentId"],
                                                         "True",
                                                         oDataTable.Rows[0]["StandardId"],
                                                         oDataTable.Rows[0]["NextAcademicYearId"]);

                    OpenNextYearPopUp(sQueryString);
                    tdNextYearLink.Visible = true;
                }
            }
        }
        if (oDsDebitDetails.Tables[Constants.I_FOUR] != null && oDsDebitDetails.Tables[Constants.I_FOUR].Rows.Count > 0 && oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["StandardID"] != null && oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["StandardID"].ToString() != string.Empty)
        {
            if (Settings.AllowNextYearInternalFeePayment || Settings.AllowNextYearInternalFeePaymentForStudent)
                OpenNextYearIntFeePopUp(oDsDebitDetails.Tables[Constants.I_FOUR].Rows[0]["MasterStudentID"].ToInt());            
        }

        if (moSchool == Constants.SchoolId.DPIS)
        {
            int iSchoolwiseStudentId = oDsDebitDetails.Tables[9].Rows[0]["SchoolwiseStudentId"].ToInt();
            int iStandardId = oDsDebitDetails.Tables[9].Rows[0]["StandardId"].ToInt();
            int iStdDivId = oDsDebitDetails.Tables[9].Rows[0]["StdDivId"].ToInt();
            bool bShowOption = oStudentFeeDetailsBL.ShowInauguralCertificateOption(miSchoolId, miAcademicYearId, iSchoolwiseStudentId, iStandardId, iStdDivId);

            if (bShowOption == true)
                btnInauguralCertificate.Visible = true;
            else
                btnInauguralCertificate.Visible = false;
        }
        else
            btnInauguralCertificate.Visible = false;

        if (moSchool == Constants.SchoolId.SNS)
        {
            if (moUserRole != Constants.UserRoles.Admin && (!lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowUserToDeleteFee.ToInt() && ru.UserId == miUserId).Any()))
            {
                grdFeesToBePaid.Columns[10].Visible = false;
                grdFeesToBePaid.Columns[11].Visible = false;
            }
        }
    }

    /// <summary>
    /// This function is used to fill sibling details in the listview.
    /// </summary>
    private void FillSiblings()
    {
        StudentSiblingDetailsBL oStudentSiblingDetailsBL = new StudentSiblingDetailsBL();
        List<StudentDetails> lstStudentDetail = new List<StudentDetails>();
        lstStudentDetail = oStudentSiblingDetailsBL.GetSiblingDetails(miSchoolId, miAcademicYearId, hidStudentId.Value.IsNullOrEmpty() ? Constants.I_ZERO : hidStudentId.Value.ToInt());
        lstvwSiblingsDetails.DataSource = lstStudentDetail;
        lstvwSiblingsDetails.DataBind();
        btnSibling.Attributes.Add("onclick", "OpenSiblingPopup(); return false;");

        if (lstStudentDetail.Count > 0)
        {
            ListViewDataItem oListViewDataItem = lstvwSiblingsDetails.Items[0];
            RadioButton rdoSelect = oListViewDataItem.FindControl("rdoSelect") as RadioButton;
            rdoSelect.Checked = true;

            if (lstStudentDetail.Count == Constants.I_ONE)
            {
                btnSibling.Attributes.Remove("onclick");
                Label sRegNo = oListViewDataItem.FindControl("lblEnrollmentNo") as Label;
                Label lblSiblingName = oListViewDataItem.FindControl("lblSiblingName") as Label;
                hidSiblingId.Value = lstvwSiblingsDetails.DataKeys[0]["YearwiseStudentId"].ToString();
                hidSearchDetails.Value = sRegNo.Text + " - " + lblSiblingName.Text;
            }
        }

        if (Convert.ToChar(hidUserHasFullAccess.Value) == Constants.C_NO || moUserRole == Constants.UserRoles.Student || lstStudentDetail.Count == Constants.I_ZERO)
            btnSibling.Visible = false;
        else
        {
            if (lstStudentDetail.Count > 0)
                btnSibling.Visible = true;
        }
    }

    /// <summary>
    /// 	This function sets the date format for date column property.
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        const int I_COLUMN_INDEX_DUE_DATE = 5;
        const int I_COLUMN_INDEX_CHEQUE_DATE = 1;

        var oReceivedDate = grdFeesToBePaid.Columns[I_COLUMN_INDEX_DUE_DATE] as BoundField;
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;

        var oChequeDate = grdPostdatedCheque.Columns[I_COLUMN_INDEX_CHEQUE_DATE] as BoundField;
        oChequeDate.HtmlEncode = false;
        oChequeDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// 	This method is used to fill postdated cheque details of a particular student.
    /// </summary>
    private void FillPostdatedChequeGrid()
    {
        int iStudentId = hidStudentId.Value.ToInt();
        var oChequeDetails = new StudentPostDatedChequesBL();
        DataSet oDsChequeDetails = oChequeDetails.GetStudentChequeDetails(iStudentId);

        DataTable dtFeeStatus = oDsChequeDetails.Tables[2];
        string sAmtPaid = dtFeeStatus.Rows[Constants.I_ZERO][Constants.I_ZERO].ToString();
        string sAmtPayable = dtFeeStatus.Rows[Constants.I_ZERO][Constants.I_ONE].ToString();
        txtChequesDeposited.Text = sAmtPaid;
        txtChequeInHand.Text = sAmtPayable;

        SetGridViewDateColumnProperties();
        grdPostdatedCheque.DataSource = oDsChequeDetails.Tables[Constants.I_ZERO].DefaultView;
        grdPostdatedCheque.DataBind();
    }

    /// <summary>
    /// 	This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StudentId"] == null)
            return;

        hidStudentId.Value = QueryString["StudentId"];
        int iStudentId = hidStudentId.Value.ToInt();
        var oStudentBL = new StudentBL(iStudentId);
        SetStudentInfo(oStudentBL);
        SetInternalFeeAttributes();
    }

    /// <summary>
    /// 	This method is used to set student information to respected controls.
    /// </summary>
    private void SetStudentInfo(StudentBL oStudentBL)
    {
        if (oStudentBL.dLeftDate.ToString() != string.Empty && oStudentBL.dLeftDate != DateTime.MinValue)
        {
            lblLeft.Text = Resources.LocalizedResources.StudentLeftSchoolOn + " " + oStudentBL.dLeftDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
            lblLeft.Text = lblLeft.Text + " Cancellation Form Number - " + oStudentBL.CancellationFormNo.ToString();
            lblLeft.Visible = true;
            hidLeftDate.Value = oStudentBL.dLeftDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
            lblStudentName.Enabled = false;
        }
        else
        {
            lblLeft.Visible = false;
            lblStudentName.Enabled = true;
        }
       
        lblStudentName.Text = string.Format("{0} {1} {2} {3}", oStudentBL.SalutationName, oStudentBL.FirstName, oStudentBL.MiddleName, oStudentBL.LastName);
        string sFormNumber = oStudentBL.GetFormNumber(miSchoolId, oStudentBL.StudentId);
        
        if (sFormNumber != string.Empty && sFormNumber != null)
            lblStudentName.Text += " (" + Resources.LocalizedResources.FormNumber + " - " + sFormNumber + ")";

        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            lblStudentName.Text += " (" + oStudentBL.ResidenceTypeName + ")";
        
        lblRollNumber.Text = oStudentBL.RollNo.ToString();
        lblStandardDivision.Text = oStudentBL.StandardDivisionName;
        txtRegNumber.Text = oStudentBL.EnrolementNo.Equals(Constants.S_EMPTY_STRING) ? lblStudentName.Text : oStudentBL.EnrolementNo;
        hidSchoolwiseStudentId.Value = oStudentBL.StudentId.ToString();
        hidStandardId.Value = oStudentBL.StandardId.ToString();
        hidDivisionId.Value = oStudentBL.DivisionId.ToString();
        btnSearch.Text = Resources.LocalizedResources.ChangeInput;
        hidSearch.Value = "Change Input";
        FillAmtToBePaidGrid(oStudentBL.YearWiseStudentId);
        FillPostdatedChequeGrid();
        OpenPDCEntryPopUp();
        OpenRefundPopUp();
        OpenStudentPayablesScreen();
        ShowHideFields(true);
        SetStudentAccessURL();

        if (moUserRole != Constants.UserRoles.Student && mbIsOldFeeDetails == false && (hidCanEdit.Value == Constants.S_YES || moUserRole == Constants.UserRoles.Admin))
        {
            if (!oStudentBL.IsNewStudent)
            {
                hlnkOldFeeRecord.Visible = true;
                SetOldFeeDetailstUrl();
            }
            else
                hlnkOldFeeRecord.Visible = false;
        }

        if (!Settings.ShowNotes)
            return;

        if (Settings.IsCautionMoneyApplicable)
        {
            DisplayCautionMoneyDetails();
            trNote.Visible = true;
            trNote1.Visible = false;
            trNote2.Visible = false;
            Label14.Text = "Note :";
            ShowHideCautionMoneyDetails(true);
        }
        else
            ShowHideCautionMoneyDetails(false);
    }

    /// <summary>
    /// 	This method is used to add attribute to link to open pdc entry pop up.
    /// </summary>
    private void OpenPDCEntryPopUp()
    {
        string sQueryString = "StudentId=" + hidStudentId.Value;
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        lnkOpenPDC.NavigateUrl = string.Format("PostDated_Cheque_Entry_PopUp.aspx?{0}", sEncrypt);
        lnkOpenPDC.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500').focus(); return false;", lnkOpenPDC.NavigateUrl));
    }

    /// <summary>
    /// 	This method is used to add attribute to link to open refund fee pop up.
    /// </summary>
    private void OpenRefundPopUp()
    {
        string sQueryString = string.Format("StudentId={0}", hidStudentId.Value);
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        lnkRefund.NavigateUrl = string.Format("FeeRefundUI.aspx?{0}", sEncrypt);
        lnkRefund.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500').focus(); return false;", lnkRefund.NavigateUrl));
    }

    /// <summary>
    /// 	This method is used to add attribute to link to open Student Payables Screen.
    /// </summary>
    private void OpenStudentPayablesScreen()
    {
        string sQueryString = String.Format("RegNo={0}", txtRegNumber.Text);
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
        lnkOpenSP.NavigateUrl = string.Format("DebitEntryUI.aspx?{0}", sEncrypt);
    }

    /// <summary>
    /// 	This method is used to add attribute to link to open refund fee pop up.
    /// </summary>
    private void OpenNextYearPopUp(string sQueryString)
    {
        if (Settings.RestrictNewPaymentIfOldPaymentIsPending && hidFeePayable.Value.ToInt() > Constants.I_ZERO && moUserRole == Constants.UserRoles.Student)
        {
            if (hidRestrictCurrentYearPayment.Value == Constants.S_YES)
                hlnkNextYr.Attributes.Add("onclick", "ShowPendingFeeAlert('You cannot pay next year fee till the complete payment of last and current year fee.')");
            else
                hlnkNextYr.Attributes.Add("onclick", "ShowPendingFeeAlert('You cannot pay next year fee till the complete payment of current year fee.')");
        }
        else if (hidRestrictCurrentYearPayment.Value == Constants.S_YES)
            hlnkNextYr.Attributes.Add("onclick", "ShowPendingFeeAlert('You cannot pay next year fee till the complete payment of last year fee.')");
        else
        {
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            hlnkNextYr.NavigateUrl = string.Format("PayFeeForNextAcaYear.aspx?{0}", sEncrypt);
            hlnkNextYr.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650').focus(); return false;", hlnkNextYr.NavigateUrl));
        }
    }

    /// <summary>
    /// 	This method is used to add attribute to link to open refund fee pop up.
    /// </summary>
    private void OpenNextYearIntFeePopUp(int aiStudentId)
    {
        InternalFeeDetailsBL oInternalFeeDetailsBL = new InternalFeeDetailsBL();
        List<InternalFeeDebitDetails> lstInternalFeeDebitDetails = oInternalFeeDetailsBL.GetInternalFeeDebitDetails(miSchoolId, miAcademicYearId, hidSchoolwiseStudentId.Value.ToInt(),true);

        if (lstInternalFeeDebitDetails.Count > 0)
        {   
            int iNextAcademicYearId = lstInternalFeeDebitDetails.Select(ss => ss.NextAcademicYearId).FirstOrDefault();
            string sQueryString = string.Format("StudentId={0}&StudentName={1}&RegNo={2}&IsNextYearFeePayment=1&NextAcademicYearId={3}",
                                                                aiStudentId,
                                                                lblStudentName.Text,
                                                                txtRegNumber.Text, 
                                                                iNextAcademicYearId);
            string sEncrypt = string.Empty;
            if (moUserRole != Constants.UserRoles.Student && Settings.AllowNextYearInternalFeePayment)
            {
                sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                tdNextYearInternalFee.Visible = true;
                hlnkIntFeeNextYr.NavigateUrl = string.Format("PayInternalFeePopup.aspx?{0}", sEncrypt);
                hlnkIntFeeNextYr.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500').focus(); return false;", hlnkIntFeeNextYr.NavigateUrl));
            }
            else if (moUserRole == Constants.UserRoles.Student && Settings.AllowNextYearInternalFeePaymentForStudent)
            {   
                if (Settings.EnableOnlinePaymentForInternalFee && hidIsOnlineInternalFeeApplicable.Value == Constants.S_ONE && Settings.EnabledOnlineFee)
                    sQueryString += "&IsOnlinePayment=1";

                sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                tdNextYearInternalFee.Visible = true;
                hlnkIntFeeNextYr.NavigateUrl = string.Format("PayInternalFeeOnlinePopup.aspx?{0}", sEncrypt);
                hlnkIntFeeNextYr.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500').focus(); return false;", hlnkIntFeeNextYr.NavigateUrl));
            }            
        }
    }

    /// <summary>
    /// 	This method is used to visible or hide controls e.g. grid column(status) according to user.
    /// </summary>
    private void VisibleOrHideCtrlsForSupervisor()
    {
        const int I_COLUMN_INDEX_CHKBOX = 0;

        grdFeesToBePaid.Columns[I_COLUMN_INDEX_CHKBOX].Visible = false;
        grdFeesToBePaid.Columns[I_COLUMN_INDEX_DELETE].Visible = false;
        grdPostdatedCheque.Columns[I_COLUMN_INDEX_PAY].Visible = false;
        grdPostdatedCheque.Columns[I_COLUMN_INDEX_STATUS].Visible = true;
        grdFeesToBePaid.Columns[I_COLUMN_INDEX_EDIT].Visible = false;

        tblStudentInfo.Visible = true;
        tblLegend.Visible = true;
        lblMandatoryMark.Visible = false;
        trPay.Visible = false;
    }

    /// <summary>
    /// 	This method is used to visible or hide controls e.g. grid column(status) according to user.
    /// </summary>
    private void VisibleOrHideCtrlsAccordingUser()
    {
        const int I_COLUMN_INDEX_CHKBOX = 1;
        if (moUserRole == Constants.UserRoles.Student || !hidStudentId.Value.IsNullOrEmpty())
        {
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_STUDENTCHKBOX].Visible = false;
            if (hidSNSSchoolId.Value == Constants.S_YES)
                grdFeesToBePaid.Columns[I_COLUMN_INDEX_CHKBOX].Visible = true;            
            else
                grdFeesToBePaid.Columns[I_COLUMN_INDEX_CHKBOX].Visible = false;
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_DELETE].Visible = false;
            grdPostdatedCheque.Columns[I_COLUMN_INDEX_PAY].Visible = false;
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_EDIT].Visible = false;
            grdPostdatedCheque.Columns[I_COLUMN_INDEX_STATUS].Visible = true;
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_VIEW].Visible = false;
            tblStudentInputFields.Visible = false;
            tblStudentInfo.Visible = false;
            tblLegend.Visible = true;
            lblMandatoryMark.Visible = false;
            tdSPOpen.Visible = false;
            trPay.Visible = false;
            if (Settings.EnabledOnlineFee)
            {
                if (hidSNSSchoolId.Value == Constants.S_YES)
                    grdFeesToBePaid.Columns[I_COLUMN_INDEX_STUDENTCHKBOX].Visible = false;
                else
                    grdFeesToBePaid.Columns[I_COLUMN_INDEX_STUDENTCHKBOX].Visible = true;
                btnOnlinePayment.Visible = true;
                trOnlinePaymentWaitingMsg.Visible = true;

                if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                    trNEFTDetails.Visible = true;
                else
                    trNEFTDetails.Visible = false;
            }

			if (Settings.EnableOnlinePaymentForLastYearFee)
            {
                StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                if (oStudentFeeDetailsBL.IsLastYearPendingFeeExist(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt()))
                {
                    btnLastYearFee.Visible = true;
                    string sQueryString = CommonUtility.EncryptQuerystring("StudentId=" + hidStudentId.Value);
                    btnLastYearFee.Attributes.Add("onclick", string.Format("window.open('OldYearOnlineFeePaymentPopup.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=500'); return false;", sQueryString));
                }
            }
			
            if (Settings.EnableOnlinePaymentForCautionMoney &&  hidIsCautionMoneyExist.Value == Constants.S_YES)
            {
                btnOnlineCautionMoneyPayment.Visible = true;
                trOnlinePaymentWaitingMsg.Visible = true;
                btnOnlineCautionMoneyPayment.Text = (hidIsCautionMoneyPaid.Value == Constants.S_ONE ? "Show Caution Money Receipt" : "Pay Caution Money Online");

                if (hidIsCautionMoneyPaid.Value == Constants.S_ONE)
                {
                    int iWidth = 870, iHeight = 470;

                    if (moSchool == Constants.SchoolId.SNS && moUserRole == Constants.UserRoles.Student)
                    {
                        iWidth = 850;
                        iHeight = 580;
                    }

                    string sQueryStr = string.Format("StudentId={0}", hidStudIdForCautionMoney.Value);
                    sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);
                    btnOnlineCautionMoneyPayment.Attributes.Add("onclick", string.Format("window.open('CautionMoneyReciept.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=" + iWidth + ",height="+iHeight+"'); return false;", sQueryStr));
                }
            }

            if (hidHideCautionMoneyButton.Value == Constants.S_YES)
                btnOnlineCautionMoneyPayment.Visible = false;

            if (Settings.EnableOnlinePaymentForInternalFee && hidIsOnlineInternalFeeApplicable.Value == Constants.S_ONE && Settings.EnabledOnlineFee && moUserRole == Constants.UserRoles.Student)
            {
                btnOnlineInternalFeePayment.Visible = true;
                trOnlinePaymentWaitingMsg.Visible = true;

                string sQueryStr = string.Format("StudentId={0}&IsOnlinePayment=1", Session[Constants.S_SESSION_STUDENT_ID].ToString());
                sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);
                btnOnlineInternalFeePayment.Attributes.Add("onclick", string.Format("window.open('PayInternalFeeOnlinePopup.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=570'); return false;", sQueryStr));
            }
            else
                trOnlinePaymentWaitingMsg.Visible = false;

            if (Settings.ShowInternalFeeAtStudentLogin && moUserRole == Constants.UserRoles.Student)
            {
                btnShowInternalFee.Visible = true;
                trOnlinePaymentWaitingMsg.Visible = true;               
                string sQueryStr = string.Format("StudentId={0}&IsOnlinePayment=0", Session[Constants.S_SESSION_STUDENT_ID].ToString());
                sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);
                btnShowInternalFee.Attributes.Add("onclick", string.Format("window.open('PayInternalFeeOnlinePopup.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=570'); return false;", sQueryStr));
            }
            else
                trOnlinePaymentWaitingMsg.Visible = false;

            
            if (miSchoolId == Constants.SchoolId.SNS.ToInt() && moUserRole == Constants.UserRoles.Student)
            {
                hlnkStudentFeeChallan.Visible = true;

                string sUrl = "GenerateChallanPopUp.aspx?" + CommonUtility.EncryptQuerystring("StudentId=" + hidStudentId.Value);
                hlnkStudentFeeChallan.Attributes.Add("onclick", string.Format("OpenFeeChallanPopup('{0}');return false;", sUrl));
            }
            else
                hlnkStudentFeeChallan.Visible = false;
        }
        else
        {
            grdFeesToBePaid.Columns[I_COLUMN_INDEX_STUDENTCHKBOX].Visible = false;
        }
    }


    /// <summary>
    /// 	This method is used to check if the login user is of superviser role and check the access he have.
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        if (moUserRole == Constants.UserRoles.Admin)
        {
            lblStudentName.Enabled = true;
            hidUserHasFullAccess.Value = Constants.S_YES;
        }
        else if ((moUserRole == Constants.UserRoles.Supervisor) || moUserRole == Constants.UserRoles.Teacher)
        {
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Fees).ToString();
            if (hidCanEdit.Value == Constants.S_YES)
            {
                lblStudentName.Enabled = true;
                hidUserHasFullAccess.Value = Constants.S_YES;
            }
            else
                hidUserHasFullAccess.Value = Constants.S_NO;
        }
        else
        {
            hidCanEdit.Value = Constants.C_NO.ToString();
            hidUserHasFullAccess.Value = Constants.S_NO;

        }
    }

    /// <summary>
    /// 	This method is used to check user role.
    /// </summary>
    /// <returns> </returns>
    private bool CheckIfAdminUser()
    {
        return moUserRole == Constants.UserRoles.Admin;
    }

    /// <summary>
    /// 	This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAtributes()
    {
        string sIsEditMode = Constants.S_NO;
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnPay, btnBack, btnPayCautionMoney, btnOnlinePayment,btnOnlineCautionMoneyPayment, btnSelect, btnSibling, btnClosePopUp, btnOnlineInternalFeePayment });
        if (mbIsOldFeeDetails || msFromUrl == S_SCREENS_URL)
            btnBack.Attributes["onclick"] = "CloseWindow()";
        else
            btnBack.Visible = false;
        if (Settings.IsMiniSite)
            hlnkSendMessage.Text = Resources.LocalizedResources.FeeLinkName;
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            sIsEditMode = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.PendingFeeList).ToString();
        if (moUserRole == Constants.UserRoles.Admin ||
                  ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                      && sIsEditMode == Constants.S_YES))
        {
            tdSms.Visible = true;
        }
        else
        {
            tdSms.Visible = false;
        }
    }

    /// <summary>
    /// 	This method is used to set form for student user.
    /// </summary>
    private void LoadFormForStudent()
    {
        if (moUserRole == Constants.UserRoles.Student || !hidStudentId.Value.IsNullOrEmpty())
        {
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())            
                hidSNSSchoolId.Value = Constants.S_YES;            
            else
                hidSNSSchoolId.Value = Constants.S_NO;

            tdSms.Visible = false;
            if (mbIsOldFeeDetails)
            {
                trTitle.Visible = true;
                cmbAcademicYrId.Visible = true;
                lblOldAcademicYear.Text = CommonUtility.DisplayAcademicYear(cmbAcademicYrId.SelectedItem.Text);
                lblacademicYr.Visible = true;
                btnBack.Text = Resources.LocalizedResources.Close;
                ShowGridOnPopup();
                btnOnlinePayment.Visible = false;
                btnOnlineCautionMoneyPayment.Visible = false;
                hlnkFeestructure.Visible = false;
                tdNextYearLink.Visible = false;
                hlnkOldFeeRecord.Visible = false;
                lblLastPayment.Visible = false;
                btnOnlineInternalFeePayment.Visible = false;
                btnLastYearFee.Visible = false;
            }
            else
            {
                trTitle.Visible = false;
                cmbAcademicYrId.Visible = false;
                lblacademicYr.Visible = false;
                if ((Session[Constants.S_SESSION_IS_NEW_ADMISSION] != null && Session[Constants.S_SESSION_IS_NEW_ADMISSION].ToString() == "False") || (!QueryString["IsNewStudent"].IsNullOrEmpty() && QueryString["IsNewStudent"].ToInt() == Constants.I_ZERO))
                {
                    hlnkOldFeeRecord.Visible = true;
                    SetOldFeeDetailstUrl();
                }
                else
                    hlnkOldFeeRecord.Visible = false;
                btnBack.Text = Resources.LocalizedResources.Back;
                if (!Session[Constants.S_SESSION_STUDENT_ID].IsNull())
                    hidStudentId.Value = Session[Constants.S_SESSION_STUDENT_ID].ToString();
                ShowGridAccordingAcademicYear();
                lblacademicYr.Visible = false;
            }
        }
        else
            lblacademicYr.Visible = false;       
    }

    /// <summary>
    /// 	This method is used to display caution money details.
    /// </summary>
    private void DisplayCautionMoneyDetails()
    {
        lblVerifyNote4.Text = string.Empty;
        lblVerifyNote4.Visible = false;
        const string S_BACKGROUND_COLOR = "background-color";
        int iStudentId = moUserRole == Constants.UserRoles.Student ? Session[Constants.S_SESSION_STUDENT_ID].ToInt() : hidStudentId.Value.ToInt();

        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataTable oDTCautionMoney = oStudentFeeDetailsBL.GetStudentCautionMoneyDetails(iStudentId, miSchoolId);
        if (oDTCautionMoney.Rows.Count > 0)
        {
            DataRow oDRCautionMoney = oDTCautionMoney.Rows[0];
            string sAmount = string.Empty;

            string sConcessionAmount = string.Empty;
            if (oDRCautionMoney["ConcessionAmount"] != DBNull.Value && oDRCautionMoney["ConcessionAmount"].ToString().Trim() != Constants.S_ZERO)
                sConcessionAmount = ", Concession : " + oDRCautionMoney["ConcessionAmount"].ToString();

            if (moUserRole != Constants.UserRoles.Student)
                sAmount = string.Format(", Amount : {0}", oDRCautionMoney["Amount"].ToString());

            if (oDRCautionMoney["Paid_By_Student"].ToString() == "True")
            {
                string sPaymentDate = oDRCautionMoney["Payment_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                string sVerifyNote3 = string.Format("Caution Money paid by Cash on date {0}", sPaymentDate);
                string sVerifyNote4 = string.Format("Caution Money paid by " + oDRCautionMoney["PaymentType"] + "( Transaction No. - " + oDRCautionMoney["TransactionNumber"] + ")" + "Payment on date {0}", sPaymentDate);
                string sReceiptNumber = oDRCautionMoney["Receipt_Number"].ToString();
                int iReceiptNumber = sReceiptNumber.ToInt();
                sReceiptNumber = sReceiptNumber.Length >= Settings.ReceiptMinimumDigits ? sReceiptNumber : sReceiptNumber.PadLeft(Settings.ReceiptMinimumDigits, '0');
                string sReceiptNumberNode = string.Format(" Receipt No. : {0}", sReceiptNumber);

                if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && oDRCautionMoney["ConcessionAmount"] == DBNull.Value)
                    sConcessionAmount = string.Empty;

                if (oDRCautionMoney["Payment_Mode"].ToString() == "Q")
                {
                    string sMode = string.Format("Caution Money paid by Cheque on date {0}.", sPaymentDate);
                    if (Settings.ShowCautionMoneyClrDate && moUserRole != Constants.UserRoles.Student)
                    {
                        string sClearanceDate = " - ";
                        if (!oDRCautionMoney["ClearanceDate"].IsNull() && !oDRCautionMoney["ClearanceDate"].ToString().IsNullOrEmpty())
                            sClearanceDate = oDRCautionMoney["ClearanceDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                        sMode = string.Format("Caution Money is paid by Cheque on Date {0} and cleared on Date {1}.", sPaymentDate, sClearanceDate);
                    }

                    string sChequeDate = string.Format("Date: {0}", oDRCautionMoney["Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT));
                    string sChequeNumber = string.Format(", Number: {0}", Convert.ToString(oDRCautionMoney["Cheque_Number"]));
                    string sBankName = string.Format(", Bank Name: {0}", Convert.ToString(oDRCautionMoney["Bank_Name"]));
                    sVerifyNote3 = string.Format("{0} Cheque Details ({1}{2}{3})", sMode, sChequeDate, sChequeNumber, sBankName);
                    lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["ClearanceDate"] == DBNull.Value)
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "#f98972");
                    else
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "White");
                }
                else if (oDRCautionMoney["Payment_Mode"].ToString() == "N")
                {
                    string sMode = string.Format("Caution Money paid by online payment on date {0}.", sPaymentDate);
                    if (Settings.ShowCautionMoneyClrDate && moUserRole != Constants.UserRoles.Student)
                    {
                        string sClearanceDate = " - ";
                        if (!oDRCautionMoney["OnlineClearanceDate"].IsNull() && !oDRCautionMoney["OnlineClearanceDate"].ToString().IsNullOrEmpty())
                            sClearanceDate = oDRCautionMoney["OnlineClearanceDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                        sMode = string.Format("Caution Money is paid by online payment on Date {0} and cleared on Date {1}.", sPaymentDate, sClearanceDate);
                    }

                    string sTransactionDate = string.Format("Date: {0}", oDRCautionMoney["TransactionDateTime"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT));
                    string sTransNo = string.Format(", Transaction No. : {0}", Convert.ToString(oDRCautionMoney["TPSLTransactionID"]));
                    string sBankName = string.Format(", Bank Name: {0}", Convert.ToString(oDRCautionMoney["RegisterdBankName"]));
                    sVerifyNote3 = string.Format("{0} Transaction Details ({1}{2}{3})", sMode, sTransactionDate, sTransNo, sBankName);
                    lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["OnlineClearanceDate"] == DBNull.Value)
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "#f98972");
                    else
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "White");
                }
                else if (oDRCautionMoney["Payment_Mode"].ToString() == "E")
                {
                    if (!oDRCautionMoney["EClearance"].IsNull() && !oDRCautionMoney["EClearance"].ToString().IsNullOrEmpty())  //
                    {
                        string sClearanceDate = " - ";
                        sClearanceDate = oDRCautionMoney["EClearance"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);   //
                        string sMode = string.Format(" and cleared on Date {0}.", sClearanceDate);

                        // lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}.", sVerifyNote4, sReceiptNumberNode, sAmount) : string.Format("{0}.", sVerifyNote3);  //old line 
                        lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}{4}.", sVerifyNote4, sMode, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3); //
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "White");                                                                                                          // old line
                    }                                                                                                                         // 
                    else     //
                    {  //
                        lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote4, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);  //
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "White");     //
                    }   //
                }
                else
                {
                    lblVerifyNote3.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "White");
                }
                if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["Returned_By_School"].ToBool() == true)
                {
                    if (oDRCautionMoney["Return_Mode"].ToString() == "Q")
                    {
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "Yellow");
                        lblVerifyNote3.Text += string.Format(" <br/> Caution Money is returned by Cheque on Date: {0} (Details - Cheque Number: {1}, Bank Name: {2}, Amount: {3}).", oDRCautionMoney["Return_Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT), Convert.ToString(oDRCautionMoney["Return_Cheque_Number"]), Convert.ToString(oDRCautionMoney["Return_Bank_Name"]), oDRCautionMoney["ReturnAmount"].ToString());
                    }
                    else
                    {
                        tdVerifyNote3.Style.Add(S_BACKGROUND_COLOR, "Yellow");
                        lblVerifyNote3.Text += string.Format(" Caution Money returned by Cash on date {0} (Amount: {1}).", oDRCautionMoney["Return_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT), oDRCautionMoney["ReturnAmount"].ToString());
                    }
                }
            }
            else
                ShowHideCautionMoneyDetails(false);
        }
        else
            ShowHideCautionMoneyDetails(false);

        if (oDTCautionMoney.Rows.Count > 1)
        {
            DataRow oDRCautionMoney = oDTCautionMoney.Rows[1];
            string sAmount = string.Empty;

            string sConcessionAmount = string.Empty;
            if (oDRCautionMoney["ConcessionAmount"] != DBNull.Value && oDRCautionMoney["ConcessionAmount"].ToString().Trim() != Constants.S_ZERO)
                sConcessionAmount = ", Concession : " + oDRCautionMoney["ConcessionAmount"].ToString();

            if (moUserRole != Constants.UserRoles.Student)
                sAmount = string.Format(", Amount : {0}", oDRCautionMoney["Amount"].ToString());

            if (oDRCautionMoney["Paid_By_Student"].ToString() == "True")
            {
                string sPaymentDate = oDRCautionMoney["Payment_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                string sVerifyNote3 = string.Format("Caution Money paid by Cash on date {0}", sPaymentDate);
                string sVerifyNote4 = string.Format("Caution Money paid by " + oDRCautionMoney["PaymentType"] + "( Transaction No. - " + oDRCautionMoney["TransactionNumber"] + ")" + "Payment on date {0}", sPaymentDate);

                string sReceiptNumber = oDRCautionMoney["Receipt_Number"].ToString();
                int iReceiptNumber = sReceiptNumber.ToInt();
                sReceiptNumber = sReceiptNumber.Length >= Settings.ReceiptMinimumDigits ? sReceiptNumber : sReceiptNumber.PadLeft(Settings.ReceiptMinimumDigits, '0');

                string sReceiptNumberNode = string.Format(" Receipt No. : {0}", sReceiptNumber);

                if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && oDRCautionMoney["ConcessionAmount"] == DBNull.Value)
                    sConcessionAmount = string.Empty;

                if (oDRCautionMoney["Payment_Mode"].ToString() == "Q")
                {
                    string sMode = string.Format("Caution Money paid by Cheque on date {0}.", sPaymentDate);

                    if (Settings.ShowCautionMoneyClrDate && moUserRole != Constants.UserRoles.Student)
                    {
                        string sClearanceDate = " - ";
                        if (!oDRCautionMoney["ClearanceDate"].IsNull() && !oDRCautionMoney["ClearanceDate"].ToString().IsNullOrEmpty())
                            sClearanceDate = oDRCautionMoney["ClearanceDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                        sMode = string.Format("Caution Money is paid by Cheque on Date {0} and cleared on Date {1}.", sPaymentDate, sClearanceDate);
                    }

                    string sChequeDate = string.Format("Date: {0}", oDRCautionMoney["Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT));
                    string sChequeNumber = string.Format(", Number: {0}", Convert.ToString(oDRCautionMoney["Cheque_Number"]));
                    string sBankName = string.Format(", Bank Name: {0}", Convert.ToString(oDRCautionMoney["Bank_Name"]));
                    sVerifyNote3 = string.Format("{0} Cheque Details ({1}{2}{3})", sMode, sChequeDate, sChequeNumber, sBankName);
                    lblVerifyNote4.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["ClearanceDate"] == DBNull.Value)
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "#f98972");
                    else
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "White");
                }
                else if (oDRCautionMoney["Payment_Mode"].ToString() == "N")
                {
                    string sMode = string.Format("Caution Money paid by online payment on date {0}.", sPaymentDate);

                    if (Settings.ShowCautionMoneyClrDate && moUserRole != Constants.UserRoles.Student)
                    {
                        string sClearanceDate = " - ";
                        if (!oDRCautionMoney["OnlineClearanceDate"].IsNull() && !oDRCautionMoney["OnlineClearanceDate"].ToString().IsNullOrEmpty())
                            sClearanceDate = oDRCautionMoney["OnlineClearanceDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
                        sMode = string.Format("Caution Money is paid by online payment on Date {0} and cleared on Date {1}.", sPaymentDate, sClearanceDate);
                    }

                    string sTransactionDate = string.Format("Date: {0}", oDRCautionMoney["TransactionDateTime"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT));
                    string sTransNo = string.Format(", Transaction No. : {0}", Convert.ToString(oDRCautionMoney["TPSLTransactionID"]));
                    string sBankName = string.Format(", Bank Name: {0}", Convert.ToString(oDRCautionMoney["RegisterdBankName"]));
                    sVerifyNote3 = string.Format("{0} Transaction Details ({1}{2}{3})", sMode, sTransactionDate, sTransNo, sBankName);
                    lblVerifyNote4.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["OnlineClearanceDate"] == DBNull.Value)
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "#f98972");
                    else
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "White");
                }

                else if (oDRCautionMoney["Payment_Mode"].ToString() == "E")
                {
                    if (!oDRCautionMoney["EClearance"].IsNull() && !oDRCautionMoney["EClearance"].ToString().IsNullOrEmpty())  //
                    {
                        string sClearanceDate = " - ";
                        sClearanceDate = oDRCautionMoney["EClearance"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);   //
                        string sMode = string.Format(" and cleared on Date {0}.", sClearanceDate);

                        lblVerifyNote4.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}{4}.", sVerifyNote4, sMode, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3); //
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "White");
                    }
                    else
                    {
                        lblVerifyNote4.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "White");
                    }
                }
                else
                {
                    lblVerifyNote4.Text = iReceiptNumber != 0 ? string.Format("{0},{1}{2}{3}.", sVerifyNote3, sReceiptNumberNode, sAmount, sConcessionAmount) : string.Format("{0}.", sVerifyNote3);
                    tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "White");
                }

                if (moUserRole != Constants.UserRoles.Student && oDRCautionMoney["Returned_By_School"].ToBool())
                {
                    if (oDRCautionMoney["Return_Mode"].ToString() == "Q")
                    {
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "Yellow");
                        lblVerifyNote4.Text += string.Format(" <br/> Caution Money is returned by Cheque on Date: {0} (Details - Cheque Number: {1}, Bank Name: {2}, Amount: {3}).", oDRCautionMoney["Return_Cheque_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT), Convert.ToString(oDRCautionMoney["Return_Cheque_Number"]), Convert.ToString(oDRCautionMoney["Return_Bank_Name"]), oDRCautionMoney["ReturnAmount"].ToString());
                    }
                    else
                    {
                        tdVerifyNote4.Style.Add(S_BACKGROUND_COLOR, "Yellow");
                        lblVerifyNote4.Text += string.Format(" Caution Money returned by Cash on date {0} (Amount: {1}).", oDRCautionMoney["Return_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT), oDRCautionMoney["ReturnAmount"].ToString());
                    }
                }

                lblVerifyNote4.Visible = true;
            }

            else
            {
                lblVerifyNote4.Visible = false;
            }
        }
    }
    

    /// <summary>
    /// 	This method is used to show or hide caution money details.
    /// </summary>
    /// <param name="abFlag"> </param>
    private void ShowHideCautionMoneyDetails(bool abFlag)
    {
        trCautionMoneyNote.Visible = abFlag;
        trCautionMoneySpace.Visible = abFlag;
        if (lblVerifyNote3.Text != string.Empty)
            return;
        lblVerifyNote3.Visible = false;
        Label14.Visible = false;
    }

    /// <summary>
    /// 	This method is used to decrypt querystring passed to this page.
    /// </summary>
    private bool bIsOldFeeDetails()
    {
        return QueryString["IsOldProgressReport"] != null && QueryString["IsOldProgressReport"].ToBool();
    }

    /// <summary>
    /// 	This method is used to set old progress report hyperlink attribute.
    /// </summary>
    private void SetOldFeeDetailstUrl()
    {
        string sUrl = "../Accountant/StudentPayFeeUI.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=true&StudentId=" + hidStudentId.Value);

        hlnkOldFeeRecord.Attributes.Add("onclick", string.Format("ShowOldFeeRecord('{0}');return false;", sUrl));
    }

    /// <summary>
    /// This Method used to set Students Access path
    /// </summary>
    private void SetStudentAccessURL()
    {
        string sUrl = string.Format("../Teacher/StudentUI.aspx?StudentId={0}&amp;StudentName={1}&amp;ClassName={2}&amp;RegNo={3}", hidSchoolwiseStudentId.Value, lblStudentName.Text, lblStandardDivision.Text, txtRegNumber.Text);
        string sQueryString = string.Empty;
        sQueryString = sUrl.Substring(sUrl.IndexOf("?") + 1) + "&StandardId=" + hidStandardId.Value
                                                                + "&DivisionId=" + hidDivisionId.Value
                                                                + "&standardName=" + string.Empty
                                                                + "&DivisionName=" + string.Empty
                                                                + "&NewMode=" + "N"
                                                                + "&pIndex=" + grdStudents.PageIndex.ToString()
                                                                + "&pSortExp=" + ""
                                                                + "&pSortDirc=" + ""
                                                                + "&Is_Configured=" + ""
                                                                + "&DivSelectedValue=" + ""
                                                                + "&StdSelectedValue=" + ""
                                                                + "&NameOrRegNo=" + ""
                                                                + "&abIsExactMatch=" + "False"
                                                                + "&IsSchoolLeft=" + ""
                                                              + "&ClassName=" + lblStandardDivision.Text
                                                                + "&asOperator=" + ""
                                                                + "&asPrefix=" + ""
                                                                + "&asPostfix=" + ""
                                                                + "&SearchedNumber=" + txtRegNumber.Text.Trim()
                                                                + "&Is_SuperAdmin=" + "N"
                                                                + "&AccessModeFromFee=" + Constants.S_YES;
        string sStudnentInfo = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
        if (hidUserHasFullAccess.Value == Constants.S_YES)
            lblStudentName.Attributes.Add("onclick", string.Format("ShowStudentRecord('{0}');return false;", sStudnentInfo));
        else
        {
            lblStudentName.Enabled = false;
            lblStudentName.Attributes.Remove("onclick");

        }
    }

    /// <summary>
    /// 	This method is used to fill academic year combo on page load.
    /// </summary>
    private void ShowAndHideOldFeeRecordLink()
    {
        if (!QueryString["StudentId"].IsNullOrEmpty())
            hidStudentId.Value = QueryString["StudentId"];
        if (!Session[Constants.S_SESSION_STUDENT_ID].IsNull())
            hidStudentId.Value = Session[Constants.S_SESSION_STUDENT_ID].ToString();
        DataTable oDtYearInfo = SchoolWiseAcademicYearMasterBL.GetPassedAcademicYears(miSchoolId, hidStudentId.Value.ToInt(),false);
        if (oDtYearInfo != null && oDtYearInfo.Rows.Count > 0 && oDtYearInfo.Rows[0][0] != DBNull.Value)
        {
            cmbAcademicYrId.Bind(oDtYearInfo, "Value_Member", "Display_Member", String.Empty);
            if (cmbAcademicYrId.Items.Count == 1)
                cmbAcademicYrId.Enabled = false;
            hlnkOldFeeRecord.Visible = true;
        }
        else
            hlnkOldFeeRecord.Visible = false;
    }

    /// <summary>
    /// 	This method is used to fill grid(Fee) according to academic year selected in the combobox Only for student login.
    /// </summary>
    private void ShowGridAccordingAcademicYear()
    {
        FillAmtToBePaidGrid(hidStudentId.Value.ToInt());
        FillPostdatedChequeGrid();

        if (grdPostdatedCheque.Rows.Count == 0)
            trCheque.Visible = false;

        ShowHideFields(true);
        VisibleOrHideCtrlsAccordingUser();

        if (!Settings.ShowNotes)
            return;

        trNote.Visible = true;
        lblVerifyNote1.Text = Settings.VerifyNote1;
        lblVerifyNote2.Text = Settings.VerifyNote2;

        if (miSchoolId == 11 && moUserRole == Constants.UserRoles.Student && SchoolBase.Settings.DisplayLateFeeNote == true)
        {
            trNotePPSHStudent.Visible = true;
            Label14.Text = "Note 2 :";
        }
        if (Settings.IsCautionMoneyApplicable)
            DisplayCautionMoneyDetails();
        else
            ShowHideCautionMoneyDetails(false);
    }

    /// <summary>
    /// 	This method is used to show the grid (Fee) for student on the pop up for previous academic years.
    /// </summary>
    private void ShowGridOnPopup()
    {
        int iAcademicYrID = cmbAcademicYrId.SelectedValue.ToInt();
        int iStudentId = StudentBL.GetYearwiseStudentId(miSchoolId, iAcademicYrID, hidStudentId.Value.ToInt());
        hidStudentId.Value = iStudentId.ToString();
        ShowGridAccordingAcademicYear();
        lblOldAcademicYear.Text = CommonUtility.DisplayAcademicYear(cmbAcademicYrId.SelectedItem.Text);
    }

    /// <summary>
    /// 	This method is used to set date format to the gris column.
    /// </summary>
    private void SetStudentGridViewDateColumnProperties()
    {
        var oLeftDate = (BoundField)grdStudents.Columns[I_COLUMN_INDEX_LEFT_DATE];
        oLeftDate.HtmlEncode = false;
        oLeftDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// 	This method is used to display the fee details for selected student.
    /// </summary>
    private void ShowStudentForFeeEntry(int aiStudentId, int aiRowIndex)
    {
        hidCautionMoneyButton.Value = Constants.S_NO;
        if (grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"] != null && grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"].ToString() != string.Empty)
        {
            lblLeft.Text = string.Format("*Student left school on {0}.", grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT));
            lblLeft.Text = lblLeft.Text + "Cancellation Form Number - " + grdStudents.DataKeys[aiRowIndex]["CancellationFormNo"].ToString();
            lblLeft.Visible = true;
            if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                var iStudId = grdStudents.DataKeys[aiRowIndex]["Schoolwise_Student_Id"].ToInt();
                List<int> lstIds = new List<int> { 342, 875, 876, 345, 1845, 1538, 3377, 1403, 3436 };
                if (lstIds.Contains(iStudId))
                    hidCautionMoneyButton.Value = Constants.S_YES;
            }
         }
            
        else
            lblLeft.Visible = false;

        int iIsCautionMoneyExist = grdStudents.DataKeys[aiRowIndex]["IsCautionMoneyExist"].ToInt();
        hidIsCautionMoneyExist.Value = iIsCautionMoneyExist == 1 ? Constants.S_YES : Constants.S_NO;

        var oStudentBL = new StudentBL(aiStudentId);
        hidStudentId.Value = oStudentBL.YearWiseStudentId.ToString();
        hidStandardId.Value = oStudentBL.StandardId.ToString();
        hidDivisionId.Value = oStudentBL.DivisionId.ToString();
        hidSchoolwiseStudentId.Value = oStudentBL.StudentId.ToString();
        SetStudentInfo(oStudentBL);

      
        //if (Settings.ShowFormNumber && grdStudents.DataKeys[aiRowIndex]["Form_Number"] != null && grdStudents.DataKeys[aiRowIndex]["Form_Number"].ToString() != string.Empty)
        //  lblStudentName.Text += " (" + Resources.LocalizedResources.FormNumber + " - " + grdStudents.DataKeys[aiRowIndex]["Form_Number"].ToString() + ")";

        SetInternalFeeAttributes();
        if (Settings.ShowNotes)
        {
            if (Settings.IsCautionMoneyApplicable)
            {
                DisplayCautionMoneyDetails();
                trNote.Visible = true;
                trNote1.Visible = false;
                trNote2.Visible = false;
                Label14.Text = "Note :";
                if (lblVerifyNote3.Text == string.Empty)
                {
                    lblVerifyNote3.Visible = false;
                    Label14.Visible = false;
                }
            }
            else
                ShowHideCautionMoneyDetails(false);
        }

        if (Convert.ToChar(hidCanEdit.Value) == Constants.C_NO && (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher))
            VisibleOrHideCtrlsForSupervisor();
    }

    private void DesignSettingAccordingLanguage()
    {
        btnSearch.Text = oResourceManager.GetString(hidSearch.Value.Replace(" ", string.Empty));
        hidAreYouSureYouWantToDeleteThisRecords.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        valRegNumber.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["RegistrationNo"].IsNull())
        {   
            txtRegNumber.Text = QueryString["RegistrationNo"];
            btnSearch_Click(btnSearch, null);
        }
        else if (QueryString["RegistrationNo"].IsNull() && !QueryString["RegNo"].IsNull())
        {
            txtRegNumber.Text = QueryString["RegNo"];
            btnSearch_Click(btnSearch, null);
        }
    }

    private void CheckFinancialYearStatus()
    {
        if (hidBaseFinancialYearId.Value != string.Empty && hidBaseFinancialYearId.Value.ToInt() != 0 && hidBaseFinancialYearId.Value.ToInt() != miFinancialYearId)
        {
            string sFinancialYearString = CommonUtility.EncryptQuerystring("IsFinancialYearShared=Y&ShowLink=Y");
            Response.Redirect("../Common/Error.aspx?" + sFinancialYearString, true);
        }
    }

    private void DisplayNote()
    {
        if (moSchool == Constants.SchoolId.PPSH && miAcademicYearId >= 14)
            trCautionMoneyNewNote.Visible = true;
        else
            trCautionMoneyNewNote.Visible = false;
    }

    #endregion -- PRIVATE METHOD(s) --    
}