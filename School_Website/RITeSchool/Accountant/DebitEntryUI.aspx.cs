/* -------------------------------------------------------------------------------
 *	FileName	: DebitEntryUI.aspx.cs
 *	Author		: ?
 *	Date		: ?
 * -------------------------------------------------------------------------------
 */

/* -------------------------------------------------------------------------------
 *  MODIFICATION LOG
 * -------------------------------------------------------------------------------
 *  Author		: Milind Y
 *  Date		: 14-Sept-2009
 *  Purpose		: This class is used to update/add/delete existing or new fee
 *				  type to the individual student as well as for the particular
 *				  standard division. For existing fee type(Standard Fee Type)
 *				  we can modify the amount as well as its due date only.
 * -------------------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 31-Jan-2012
 *	Purpose		: Modified to create a Ledger for new fee types added.
 * -------------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using System.Resources;
using SchoolEntities;

public partial class DebitEntryUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region -- CONSTANT(s) --
    private const string S_CMD_NAME_DELETE_DEBIT = "Delete_Debit_Entry";
    private const string S_CMD_NAME_EDIT_DEBIT = "Edit_Debit_Entry";

    private const int I_COLUMN_INDEX_DOB = 4;
    private const int I_COLUMN_INDEX_COPY = 6;
    private const int I_COLUMN_INDEX_SELECT = 5;
    private const int I_COLUMN_INDEX_DUE_DATE = 2;
    private const string S_ELEMENT = "element";
    private const string S_VW_CHEQUE = "CreditedChequeDT";
    private const string S_DT_FEETYPE = "FeeTypeDetails";
    private const string S_EDIT_IMAGE_URL = "~/RITeSchool/images/DisableSelect.gif";
    private const int I_COLUMN_INDEX_LEFT_DATE = 5;
    private const int I_IS_NEW_INTERNAL_FEE_ENTRY = 1;
    private const int I_IS_BOUNCED_CHEQUE_INTERNAL_FEE_ENTRY = 0;
    private const int I_COLUMN_INDEX_RTE = 7;
    private const string S_DELETE_MSG = "Unpaid fee deleted successfully !!!";
    private const string S_DISABLE_MSG = "Unpaid fee disabled successfully !!!";
    //private const int S_VW_INTERNALCHEQUE = "InternalFeeCheques";

    #endregion -- CONSTANT(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Indicates if the Accounts module is enabled for the school.
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    public bool IsInternalFee
    {
        get { return rdlstDisplayFeeType.SelectedValue.ToInt() == FeeModes.InternalFee.ToInt(); }
    }

    private bool UpdateInternalFeeinDayBook
    {
        get
        {
            if (moSchool == Constants.SchoolId.PPS)
                return true;
            else
                return rdlstDisplayFeeType.Items[0].Selected;
        }
    }

    #endregion -- PROPERTIES --

    public enum FeeModes
    {
        StudentFee = 1,
        InternalFee = 2
    }

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to fill standard and division combo boxes as well to set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tdSMSLabel.Visible = true;
           
            if (!IsPostBack)
            {
                hidShow.Value = "Show";
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DisableValidator();
                DesignSettingAccordingLanguage();
                txtRegNumber.Focus();
                chkSendSMS.Checked = false;
                chkSendMessage.Checked = false;
                chkRTEStudent.Enabled = true;
                FillAllComboBoxes();
                SetAcademicYearDates();
                SetSortingFieldDefaultValues();
                SetGridViewDateColumnProperties();
                hidSendSms.Value = "N";
                ApplyMouseHoverEffect(new List<Button> { btnShow, btnBackUp, btnSave, btnDelete, btnCancel });

                SetDefaultButton(btnShow);
                ReadQuerystring();

                if (QueryString["RegNo"].IsNull())
                    btnBackUp.Visible = false;
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

    private void FillAccountHeaderComboBox(bool bIsForInternal)
    {
        if (rdoFeeType.Items[0].Selected && rdlstDisplayFeeType.Items[0].Selected && !rdoFeeType.Items[2].Selected)
        {   
            AccountHeaderStatus(true);
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            ListSource.FillDropDownList(oStudentFeeDetailsBL.GetAccountHeaderDetails(miSchoolId, bIsForInternal), cmbAccountHeader, "AccountHeaderName", "AccountHeaderId", Constants.S_SELECT);
        }
        else
            AccountHeaderStatus(false);
    }

    /// <summary>
    /// This event is used to fill division combox according to selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtRegNumber.Text = string.Empty;
            hidStandardId.Value = ddlStandard.SelectedValue;
            FillDivisionCombobox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display fee details of the stardard division as well as for displaying student
    /// list grid or details of particular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            tblMsg.Visible = true;
            btnSave.Enabled = true;
            btnDelete.Visible = false;
            lblError.Text = string.Empty;
            ClearAllControls();
            chkRTEStudent.Enabled = (txtRegNumber.Text == string.Empty) ? true : false;
            if (txtRegNumber.Enabled)
            {
                // If there is no text enter in the textbox that means we need to display
                // fee details of selected standard division.
                if (txtRegNumber.Text.Trim() != string.Empty)
                {
                    SetStudentGridViewDateColumnProperties();
                    grdStudents.PageIndex = 0;
                    grdStudents.DataSourceID = GrdDSobj.ID;
                    grdStudents.DataBind();
                    btnShow.Text = Resources.LocalizedResources.ChangeInput;
                    hidShow.Value = "Change Input";
                    txtRegNumber.Enabled = false;
                    ddlStandard.Enabled = false;
                    ddlDivision.Enabled = false;
                    
                    // If text entered in the textbox matches exactly to the one student then diplay that  
                    // student fee details instead of displaying the student list grid.
                    if (grdStudents.Rows.Count == 1)
                    {
                        int iStudentId = grdStudents.DataKeys[0][0].ToInt();
                        ShowStudentForFeeEntry(iStudentId, 0);
                        trStudents.Visible = false;
                    }

                    grdDebitInfo.Columns[6].Visible = false;
                }
                else
                {
                    rdoFeeType.Items[2].Enabled = false;
                    btnShow.Text = Resources.LocalizedResources.ChangeInput;
                    hidShow.Value = "Change Input";
                    lblError.Visible = false;
                    tblStudentInfo.Visible = false;
                    ShowHideControls(true);
                    FillDebitGrid();
                    FillOtherFeeTypes();
                    trLeftStudent.Visible = false;
                    trConcession.Visible = false;
                }
            }
            else
            {
                btnShow.Text = Resources.LocalizedResources.Show;
                hidShow.Value = "Show";
                ShowHideControls(false);
                grdStudents.DataSourceID = null;
                grdDebitInfo.DataSource = null;
                grdDebitInfo.DataBind();
                trStudents.Visible = false;
            }

            chkSendMessage.Checked = false;
            chkSendSMS.Checked = false;

            string sMessage = SetAlertMessage();
            sMessage = sMessage.Replace("'", "\\'");
            btnSave.Attributes.Add("onclick", string.Empty);
            btnSave.Attributes.Add("onclick", "if(!SendSms_Message('" + sMessage + "')) {return false;}");

            if (IsInternalFee)
                rdoFeeType.Items[1].Enabled = false;
            else
                rdoFeeType.Items[1].Enabled = true;

            SetDueDateStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is sued to set due date check box status.
    /// </summary>
    private void SetDueDateStatus()
    {
        chkNotApplicable.Checked = false;
        chkNotApplicable.Enabled = false;
        txtDueDate.Enabled = true;
        spnMandatory.Visible = true;
        if (txtRegNumber.Text.Trim() == string.Empty && btnShow.Text == Resources.LocalizedResources.ChangeInput && IsInternalFee)
            chkNotApplicable.Enabled = true;
    }

    /// <summary>
    /// This event is used to set PostBackUrl to Back button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        string sQueryString = CommonUtility.EncryptQuerystring(String.Format("RegistrationNo={0}", txtRegNumber.Text));
        Response.Redirect("~/RITeSchool/Accountant/StudentPayFeeUI.aspx?" + sQueryString);
    }

    /// <summary>
    /// This event is used to save fee details of the selected standard division as well as student..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = PopulateFeeDetailsBL();
            ArrayList arrStdDivLst;
            lblError.Text = " ";
       

            // This transaction is for new mode.
            if (hidMode.Value == Constants.S_NEW_MODE)
            {
                // If textbox text is empty that means debit entry is not for particular student.
                if (txtRegNumber.Text.Trim() == string.Empty)
                {
                    // Get standard divids arraylist.
                    arrStdDivLst = GetStdDivIdLst();
                    if (!IsInternalFee)
                    {
                        // Entry is added at school level.
                        if (ddlStandard.SelectedIndex == 0)
                            oStudentFeeDetailsBL.InsertStudentFeeDetails(arrStdDivLst);
                        else
                        {
                            int iStandardId = ddlStandard.SelectedValue.ToInt();

                            // Entry is added at Standard level.
                            if (ddlStandard.SelectedIndex != 0 && ddlDivision.SelectedIndex == 0)
                                oStudentFeeDetailsBL.InsertStudentFeeDetails(arrStdDivLst, iStandardId);

                            // Entry is added at class level.
                            else
                            {
                                int iDivisionId = ddlDivision.SelectedValue.ToInt();
                                oStudentFeeDetailsBL.InsertStudentFeeDetails(arrStdDivLst, iStandardId, iDivisionId);
                            }
                        }
                    }
                    else
                    {
                        if (ddlStandard.SelectedIndex == 0)
                            oStudentFeeDetailsBL.InsertStudentInternalFeeDetails(arrStdDivLst);
                        else if (ddlStandard.SelectedIndex != 0 && ddlDivision.SelectedIndex == 0)
                            oStudentFeeDetailsBL.InsertStudentInternalFeeDetails(arrStdDivLst, ddlStandard.SelectedValue.ToInt());
                        else
                            oStudentFeeDetailsBL.InsertStudentInternalFeeDetails(arrStdDivLst, ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt());
                    }

                }

                // Entry is added for particular student.
                else
                {
                    // If user want to add debit entry for cheque bounce then
                    if (rdoFeeType.Items[2].Selected && ddlChequeNo.Visible && ddlMode.Visible)
                    {
                        // Get xml string for bounce cheque.
                        string sBounceChequeDetails = GetXMLForBouncedChequeDetails();
                        int iStudentId = hidStudentId.Value.ToInt();
                        int iPDCId = ddlChequeNo.SelectedValue.ToInt();

                        var oStudentFeeBL = new StudentFeeDetailsBL();
                        int iReceiptNo = oStudentFeeBL.GetReceiptNoForPDCPayment(iStudentId, iPDCId,ddlMode.SelectedValue.ToInt());

                        string sStudentFeeIdsXML = String.Empty;

                        if (IsInternalFee)
                        {
                            oStudentFeeDetailsBL.InsertStudentInternalFeeDetails(hidStudentId.Value.ToInt(), 1, I_IS_BOUNCED_CHEQUE_INTERNAL_FEE_ENTRY, iPDCId);
                        }
                        else
                        {
                            // We get the FeeVoucher particulars for the given Student and ReceiptNo.
                            // This needs to be performed now(before fee being delete in the db) becuase after deletion,
                            // It is difficult to get the correct particulars (since there could be multiple deleted entries).
                            if (IsAccountsModuleEnabled)
                            {
                                var oVoucherClient = new AccountVoucherClient();
                                try
                                {
                                    oVoucherClient.Open();
                                    List<FeeVoucherParticulars> lstFeeParticulars = oVoucherClient.GetFeePaymentParticulars(miSchoolId, miAcademicYearId, miFinancialYearId, iStudentId, iReceiptNo.ToString());
                                    sStudentFeeIdsXML = CommonUtility.GetXMLForList(lstFeeParticulars);
                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), Resources.LocalizedResources.AnExceptionOccuredWhileGettingFeeVoucherParticulars.Replace("%studentId%", iStudentId.ToString()).Replace("%receiptNo%", iReceiptNo.ToString()));
                                }
                                finally
                                {
                                    if (oVoucherClient.State != CommunicationState.Faulted)
                                        oVoucherClient.Close();
                                }
                            }

                            oStudentFeeDetailsBL.RollBackIfChequeIsBounce(iStudentId, iPDCId, sBounceChequeDetails, Convert.ToInt32(ddlMode.SelectedValue));

                            // Now we actually delete the previously collected particulars from the FeeVoucher.
                            if (IsAccountsModuleEnabled)
                            {
                                var oVoucherClient = new AccountVoucherClient();
                                try
                                {
                                    oVoucherClient.Open();
                                    oVoucherClient.DeleteFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, iStudentId, iReceiptNo.ToString(), sStudentFeeIdsXML, miUserId,true);
                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), Resources.LocalizedResources.AnExceptionOccuredWhileDeletingFeePayment.Replace("%studentid%", iStudentId.ToString()).Replace("%receiptno%", iReceiptNo.ToString()));
                                }
                                finally
                                {
                                    if (oVoucherClient.State != CommunicationState.Faulted)
                                        oVoucherClient.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        oStudentFeeDetailsBL.Standard_Div_Id = hidStandardDivId.Value.ToInt();
                        if (!IsInternalFee)
                            oStudentFeeDetailsBL.InsertStudentFeeDetails();
                        else
                            oStudentFeeDetailsBL.InsertStudentInternalFeeDetails(hidStudentId.Value.ToInt(), 1, I_IS_NEW_INTERNAL_FEE_ENTRY, 0);
                    }

                    if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
                    {
                        AccountLedgerClient oLedgerClient = null;
                        try
                        {
                            oLedgerClient = new AccountLedgerClient();
                            oLedgerClient.Open();
                            oLedgerClient.CreateLedgerForNewFeeType(miSchoolId, miFinancialYearId, oStudentFeeDetailsBL.FeeType, miUserId);
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),Resources.LocalizedResources.ThereWasAnErrorCreatingLedgerForNewFeeType.Replace("%feetype%",oStudentFeeDetailsBL.FeeType));
                        }
                        finally
                        {
                            if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
                                oLedgerClient.Close();
                        }
                    }
                }
                if (!(rdoFeeType.Items[2].Selected && ddlChequeNo.Visible && ddlMode.Visible))
                    SendMessage(hidMode.Value, (txtFeeType.Visible ? (txtFeeType.Text != string.Empty ? txtFeeType.Text : ddlOtherFeeTypes.SelectedItem.Text) : ddlFeeType.SelectedItem.Text),
                        txtDueDate.Text, txtAmt.Text, (txtPayableFor.Visible ? txtPayableFor.Text : ddlPayableFor.SelectedItem.Text));

            }

            // for edit mode.
            else
            {
                oStudentFeeDetailsBL.Updated_By_Id = miUserId;
                int iSerialNo = hidSerialNo.Value.ToInt();
                int iDebitId = hidDebitId.Value.ToInt();
                if (iSerialNo != 0 && txtRegNumber.Text.Trim() == string.Empty)
                {
                    oStudentFeeDetailsBL.SerialNumber = iSerialNo;
                    arrStdDivLst = GetStdDivIdLst();

                    // Check entry level i.e. School, Standard, Class or Student as well check entry level of current transaction
                    // if it is same then update log entry.
                    string sIsUpdate = hidCurrentLevel.Value == hidDebitEntryLevel.Value ? "true" : "false";

                    if (!IsInternalFee)
                        oStudentFeeDetailsBL.UpdateStudentFeeDetails(arrStdDivLst, sIsUpdate);
                    else
                        oStudentFeeDetailsBL.UpdateStudentInternalFeeDetails(arrStdDivLst, sIsUpdate);
                }
                else
                {
                    if (!IsInternalFee)
                        oStudentFeeDetailsBL.UpdateStudentFeeDetails(iDebitId);
                    else
                        oStudentFeeDetailsBL.UpdateStudentInternalFeeDetails(iDebitId);

                    if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
                    {
                        AccountLedgerClient oLedgerClient = null;
                        try
                        {
                            oLedgerClient = new AccountLedgerClient();
                            oLedgerClient.Open();
                            oLedgerClient.CreateLedgerForNewFeeType(miSchoolId, miFinancialYearId, oStudentFeeDetailsBL.FeeType, miUserId);
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),Resources.LocalizedResources.ThereWasAnErrorCreatingLedgerForNewFeeType.Replace("%feetype%",oStudentFeeDetailsBL.FeeType));
                        }
                        finally
                        {
                            if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
                                oLedgerClient.Close();
                        }
                    }
                }

                if (!(rdoFeeType.Items[2].Selected && ddlChequeNo.Visible && ddlMode.Visible))
                    SendMessage(hidMode.Value, (txtFeeType.Visible ? (txtFeeType.Text != string.Empty ? txtFeeType.Text : ddlOtherFeeTypes.SelectedItem.Text) : ddlFeeType.SelectedItem.Text),
                        txtDueDate.Text, txtAmt.Text, (txtPayableFor.Visible ? txtPayableFor.Text : ddlPayableFor.SelectedItem.Text));

                hidMode.Value = Constants.S_NEW_MODE;
            }

            if (rdoFeeType.Items[2].Selected && ddlChequeNo.Visible && ddlMode.Visible && hidSendSms.Value == "Y")
                SendChequeBounceSMS();
            ClearAllControls();
            FillDebitGrid();
            FillOtherFeeTypes();
            btnSave.Attributes.Add("onclick", string.Empty);
            chkSendSMS.Checked = false;
            chkSendMessage.Checked = false;
            tblMsg.Visible = true;
            btnDelete.Visible = false;

            // If the Accounts module is enabled, create a new ledger for newly saved fee type.
            if (IsAccountsModuleEnabled && UpdateInternalFeeinDayBook)
            {
                AccountLedgerClient oLedgerClient = null;
                try
                {
                    oLedgerClient = new AccountLedgerClient();

                    oLedgerClient.Open();
                    oLedgerClient.CreateLedgersForStudentPayables(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), Resources.LocalizedResources.ThereWasErrorCreatingLedgeForStudentPayables);
                }
                finally
                {
                    if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
                        oLedgerClient.Close();
                }
            }

            SetDueDateStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all data entry controls as well set mode of operation i.e. new .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            ClearAllControls();
            chkSendMessage.Checked = false;
            chkSendSMS.Checked = false;
            FillDebitGrid();
            FillOtherFeeTypes();
            btnDelete.Visible = false;            
            btnSave.Enabled = true;
            SetDueDateStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default values to feetype and payable for textbox accodring to selected 
    /// cheque number from the combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlChequeNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlChequeNo.SelectedValue != Constants.S_ZERO)
            {
                if (ddlMode.SelectedValue.ToInt() == 1)
                {
                    txtFeeType.Text = Resources.LocalizedResources.BounceChequeFine;
                    txtPayableFor.Text = Resources.LocalizedResources.BouncedChqNo + ddlChequeNo.SelectedItem.Text;
                    txtRemarks.Text = Resources.LocalizedResources.FineForBouncedChqNo.Replace("%checkno%", ddlChequeNo.SelectedItem.Text);
                }
                else
                {
                    txtFeeType.Text = "Bounce Transaction Fine";
                    txtPayableFor.Text = "Bounced Txn. No." + ddlChequeNo.SelectedItem.Text;
                    txtRemarks.Text = "Fine for bounced Txn. No. " + ddlChequeNo.SelectedItem.Text;
                }

                const int I_CHEQUE_CLEAREANCE_DATE_COL = 2;
                //txtFeeType.Text = Resources.LocalizedResources.BounceChequeFine;
                //txtPayableFor.Text = Resources.LocalizedResources.BouncedChqNo + ddlChequeNo.SelectedItem.Text;
                //txtRemarks.Text = Resources.LocalizedResources.FineForBouncedChqNo.Replace("%checkno%", ddlChequeNo.SelectedItem.Text);
                var dtCreditedChequeDetails = ViewState[S_VW_CHEQUE] as DataTable;
                DataRow[] oDataRow = dtCreditedChequeDetails.Select("Id = " + ddlChequeNo.SelectedValue);
                if (oDataRow.Length > 0 && oDataRow[Constants.I_ZERO][I_CHEQUE_CLEAREANCE_DATE_COL].ToString() != null
                    && oDataRow[Constants.I_ZERO][I_CHEQUE_CLEAREANCE_DATE_COL].ToString() != string.Empty)

                    if (ddlMode.SelectedValue.ToInt() == 1)
                    {
                        lblMsgBounce.Text = Resources.LocalizedResources.ThisChequeIsAlreadyCleared +
                                            oDataRow[Constants.I_ZERO][I_CHEQUE_CLEAREANCE_DATE_COL].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT) + ")";
                    }
                    else
                    {
                        lblMsgBounce.Text = "This transaction is already cleared.(Transaction Clearance Date :" + 
                                           oDataRow[Constants.I_ZERO][I_CHEQUE_CLEAREANCE_DATE_COL].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT) + ")";
                    }
                else
                    lblMsgBounce.Text = string.Empty;

                FillDebitGrid();

                string sLoginDetailsSmsText = GetSMSTemplate(Constants.SMSTemplate.ChequeBounceSMS.ToInt());
                btnSave.Attributes.Add("onclick", "if(!SendMessage('" + sLoginDetailsSmsText + "')) {return false;}");
            }
            else
            {
                txtAmt.Text = Constants.S_ZERO;
                txtFeeType.Text = string.Empty;
                txtPayableFor.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                lblMsgBounce.Text = string.Empty;
                txtDueDate.Text = string.Empty;
                chkSendSMS.Checked = false;
                ddlChequeNo.Focus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var oStudentFeeDetailsBL = new StudentFeeDetailsBL();

            DataTable dtFeeDetails = oStudentFeeDetailsBL.GetFeeDetails(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt(), Convert.ToInt32(ddlMode.SelectedValue), (rdlstDisplayFeeType.SelectedValue.ToInt() == 2));
            ddlChequeNo.Bind(dtFeeDetails, "Id", "TxnNo", Constants.S_SELECT);
            ViewState[S_VW_CHEQUE] = dtFeeDetails;
        }        
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
            
     /// <summary>
    /// This event is used to delete fees.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteFee();
            SetDueDateStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnDisableOnlinePayment_Click(object sender, EventArgs e)
    {
        try
        {
            DisableOrDeleteUnpaidFee(true);
            lblMessage.Text = S_DISABLE_MSG;
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnDelUnpaidFee_Click(object sender, EventArgs e)
    {
        try
        {
            DisableOrDeleteUnpaidFee(false);
            lblMessage.Text = S_DELETE_MSG;
            btnDelUnpaidFee.Visible = false;
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    protected void rdlstDisplayFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoFeeType.Items[2].Selected)
                FillModes();

            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            if (!IsInternalFee)
            {
                tdInternalFeeOnlinePayment.Visible = false;

                rdoFeeType.Items[1].Enabled = true;
                FillDebitGrid();
                //cmbAccountHeader.Visible = true;
                AccountHeaderStatus(true);                
                ListSource.FillDropDownList(oStudentFeeDetailsBL.GetAccountHeaderDetails(miSchoolId, false), cmbAccountHeader, "AccountHeaderName", "AccountHeaderId", Constants.S_SELECT);
            }
            else
            {
                tdInternalFeeOnlinePayment.Visible = true;
                FillDebitGrid();

                if (rdoFeeType.Items[1].Selected)
                {
                    rdoFeeType.Items[1].Selected = false;
                    rdoFeeType.Items[0].Selected = true;
                    rdoFeeType_SelectedIndexChanged(rdoFeeType, null);
                }

                rdoFeeType.Items[1].Enabled = false;

                //cmbAccountHeader.Visible = false;
                if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    AccountHeaderStatus(true);                    
                    ListSource.FillDropDownList(oStudentFeeDetailsBL.GetAccountHeaderDetails(miSchoolId, true), cmbAccountHeader, "AccountHeaderName", "AccountHeaderId", Constants.S_SELECT);
                }
                else
                    AccountHeaderStatus(false);
            }

            FillOtherFeeTypes();

            //var dtCreditedChequeDetails = ViewState[S_VW_CHEQUE] as DataTable;
            //ddlChequeNo.Bind(dtCreditedChequeDetails, "PostDated_Cheque_Id", "Cheque_Number", Constants.S_SELECT);

            SetDueDateStatus();

            btnSave.Visible = true;
            ClearAllControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set due date field status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkNotApplicable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkNotApplicable.Checked)
            {
                txtDueDate.Text = string.Empty;
                txtDueDate.Enabled = false;
                spnMandatory.Visible = false;
            }
            else
            {
                spnMandatory.Visible = true;
                txtDueDate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region -- GRID EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to delete particular data entry as well
    /// to set values to controls while clicking on edit button of a particular data entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDebitInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() != "SORT")
            {
                int iSerialNo = Constants.I_ZERO;
                int iRowindex = e.CommandArgument.ToInt();
                int iDebitId = grdDebitInfo.DataKeys[iRowindex][Constants.I_ZERO].ToInt();
                bool bIsInternalFee = grdDebitInfo.DataKeys[iRowindex]["IsInternalFee"].ToBool();
                if (grdDebitInfo.DataKeys[iRowindex][2].ToString() != string.Empty)
                    iSerialNo = grdDebitInfo.DataKeys[iRowindex][2].ToInt();
                string sDebitLevel = grdDebitInfo.DataKeys[iRowindex][3].ToString();
                bool bIsPaid = grdDebitInfo.DataKeys[iRowindex]["IsPaid"].ToBool();
                string sSerialNumber = grdDebitInfo.DataKeys[iRowindex]["Serial_Number"].ToString();
                hidSerialNo.Value = sSerialNumber.ToString();

                switch (e.CommandName)
                {
                    case S_CMD_NAME_EDIT_DEBIT:
                        ddlOtherFeeTypes.Visible = true;
                        chkRTEStudent.Enabled = false;
                        EditFee(iSerialNo, iRowindex, iDebitId, bIsInternalFee, sDebitLevel);

                        if (grdDebitInfo.DataKeys[iRowindex]["ShowUnPaidDisableButton"].ToBool() == true)
                            btnDisableOnlinePayment.Visible = true;
                        else
                            btnDisableOnlinePayment.Visible = false;

                        if (grdDebitInfo.DataKeys[iRowindex]["ShowUnPaidDeleteButton"].ToBool() == true)
                            btnDelUnpaidFee.Visible = true;
                        else
                            btnDelUnpaidFee.Visible = false;

                        if (bIsPaid)
                        {
                            btnSave.Visible = false;
                            btnDelete.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = true;
                            btnDelete.Visible = true;
                        }

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
    /// This event is used to set sorting image as per sorting direction
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDebitInfo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            var sGridviewName = sender as GridView;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                // Call the AddSortImage helper method to add a sort direction image to the appropriate column header. 
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
    /// This event is used to sort data according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDebitInfo_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
            FillDebitGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show or hide image and delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDebitInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= 0)
            {

                string sSerialNumber = grdDebitInfo.DataKeys[iRowIndex]["Serial_Number"].ToString();

                var imgSelect = e.Row.Cells[I_COLUMN_INDEX_SELECT].Controls[Constants.I_ZERO] as ImageButton;
                var imgCopy = e.Row.Cells[I_COLUMN_INDEX_COPY].Controls[Constants.I_ZERO] as ImageButton;

                int iVal = Constants.I_ZERO;
                if (IsInternalFee)
                    iVal = Constants.I_ONE;
                else
                    iVal = Constants.I_ZERO;

                string sQueryString = "SerialNumber=" + sSerialNumber + "&IsInternalFee=" + iVal;
                imgCopy.Attributes.Add("onclick", "if(!OpenPopup( '" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");

                bool bIsPaid = grdDebitInfo.DataKeys[iRowIndex]["IsPaid"].ToBool();
                if (bIsPaid)
                {
                    //imgSelect.ImageUrl = S_EDIT_IMAGE_URL;
                    //imgSelect.Enabled = false;
                    imgSelect.ToolTip = Resources.LocalizedResources.IndicatesYouCanNotModifyThisEntry;
                }
                string sIsBounceChequeEntry = grdDebitInfo.DataKeys[iRowIndex]["Is_Cheque_Bounce"].ToString();
                if (sIsBounceChequeEntry == Constants.C_YES.ToString())
                {
                    e.Row.CssClass = "BounceCheque";
                }
                Image imgConsiderRTE = e.Row.Cells[I_COLUMN_INDEX_RTE].Controls[Constants.I_ZERO] as Image;
                imgConsiderRTE.Attributes.Add("onclick", "{return false;}");
                imgConsiderRTE.Style.Add("cursor", "default");
                    bool bConsiderForRTEConcession = !grdDebitInfo.DataKeys[iRowIndex]["IsConsiderForRTEStudent"].ToBool();
                    if (bConsiderForRTEConcession)
                    {
                        imgConsiderRTE.Visible = true;
                    }
                    else
                    {
                        imgConsiderRTE.Visible = false;
                    }

                    DataRowView oDataRowView = e.Row.DataItem as DataRowView;
                    bool bIsDueDateApplicable = oDataRowView["IsDueDateApplicable"].ToBool();
                    if (!bIsDueDateApplicable)
                    {   
                        TableCell cell = e.Row.Cells[2];
                        cell.Text = "-";
                    }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset standard and division combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtRegNumber_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRegNumber.Text.Trim() != string.Empty)
            {
                ddlStandard.SelectedIndex = 0;
                ddlDivision.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to decide whether existing fee type is used or new fee type is used.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidMode.Value = Constants.S_NEW_MODE;
            btnSave.Enabled = true;
            btnDelete.Visible = false;
            if (rdoFeeType.Items[Constants.I_ZERO].Selected)
            {
                ClearAllControls();
                VisibleDisableControls(false);
                feetypeSeparator.Visible = true;
                ddlOtherFeeTypes.Visible = true;
                if (ddlOtherFeeTypes.Items.Count > 0)
                    ddlOtherFeeTypes.SelectedIndex = 0;
                else
                {
                    ddlOtherFeeTypes.Visible = false;
                    feetypeSeparator.Visible = false;
                }
                trChequeNo.Visible = false;
                trMode.Visible = false;
                chkSendSMS.Checked = false;
                chkSendMessage.Checked = false;
                tblMsg.Visible = true;
                grdDebitInfo.Columns[6].Visible = true;
                btnSave.Attributes.Add("onclick", string.Empty);
            }
            else if (rdoFeeType.Items[Constants.I_ONE].Selected)
            {
                VisibleDisableControls(true);
                feetypeSeparator.Visible = false;
                ddlOtherFeeTypes.Visible = false;
                var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                int iStandardId = hidStandardId.Value.ToInt();

                DataTable dtStdFeeType = oStudentFeeDetailsBL.GetStandardFeeType(miSchoolId, miAcademicYearId, iStandardId);
                ddlFeeType.Bind(dtStdFeeType, "SchoolWise_Standard_FeeType_Id", "Fee_Type", Constants.S_SELECT);

                ddlPayableFor.Bind(null, "", "", Constants.S_SELECT);
                trChequeNo.Visible = false;
                trMode.Visible = false;
                chkSendSMS.Checked = false;
                chkSendMessage.Checked = false;
                tblMsg.Visible = true;
                grdDebitInfo.Columns[6].Visible = false;
                btnSave.Attributes.Add("onclick", string.Empty);
            }
            else
            {
                FillModes();
                chkSendSMS.Checked = false;
                chkSendMessage.Checked = false;
                tblMsg.Visible = false;
                trChequeNo.Visible = true;
                trMode.Visible = true;
                VisibleDisableControls(false);
                feetypeSeparator.Visible = false;
                ddlOtherFeeTypes.Visible = false;
                var dtCreditedChequeDetails = ViewState[S_VW_CHEQUE] as DataTable;
                ddlChequeNo.Bind(dtCreditedChequeDetails, "Id", "Cheque_Number", Constants.S_SELECT);
                txtAmt.Text = "0";
                grdDebitInfo.Columns[6].Visible = false;
            }

            if (!rdoFeeType.Items[2].Selected)
            {
                string sMessage = SetAlertMessage();
                sMessage = sMessage.Replace("'", "\\'");
                btnSave.Attributes.Add("onclick", string.Empty);
                btnSave.Attributes.Add("onclick", "if(!SendSms_Message('" + sMessage + "')) {return false;}");
            }

            FillDebitGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillModes()
    {
        bool bIsInternalFee = false;
        if (rdlstDisplayFeeType.SelectedValue.ToInt() == 2)
            bIsInternalFee = true;
        List<PaymentMode> lstPaymentMode = GetModes(bIsInternalFee);

        ListSource.FillDropDownList(lstPaymentMode, ddlMode, "Name", "Id", Constants.S_SELECT);

        ddlChequeNo.Items.Clear();
        ddlChequeNo.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });

        txtAmt.Text = string.Empty;
        txtDueDate.Text = string.Empty;
        txtFeeType.Text = string.Empty;
        txtPayableFor.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }

    /// <summary>
    /// This event is used to check that SMS has to send or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSendSMS_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            hidSendSms.Value = chkSendSMS.Checked ? Constants.S_YES : Constants.S_NO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check that Message has to send or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSendMessage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            hidSendMsg.Value = chkSendMessage.Checked ? Constants.S_YES : Constants.S_NO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill payable for dropdownlist according to selected fee type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            int iStdFeeTypeId = ddlFeeType.SelectedValue.ToInt();
            int iStudentId = ((hidStudentId.Value == string.Empty) ? "0" : hidStudentId.Value).ToInt();
            DataTable dtStdFeeType = oStudentFeeDetailsBL.GetIntervalsWithAmount(miSchoolId, miAcademicYearId, iStdFeeTypeId, iStudentId);
            ViewState[S_DT_FEETYPE] = dtStdFeeType;
            ddlPayableFor.Bind(dtStdFeeType, "PayableFor", "PayableFor", Constants.S_SELECT);
            txtAmt.Text = dtStdFeeType.Rows.Count > 0 ? dtStdFeeType.Rows[Constants.I_ZERO]["Amount"].ToString() : string.Empty;
            txtDueDate.Text = String.Empty;
            txtRemarks.Text = String.Empty;
            if (ddlStandard.SelectedIndex == Constants.I_ZERO)
                cmbAccountHeader.SelectedValue = oStudentFeeDetailsBL.GetAccountHeaderIdByFeeType(miSchoolId, hidStandardId.Value.ToInt(), iStdFeeTypeId).ToString(); 
            else
                cmbAccountHeader.SelectedValue = oStudentFeeDetailsBL.GetAccountHeaderIdByFeeType(miSchoolId, ddlStandard.SelectedValue.ToInt(), iStdFeeTypeId).ToString(); 
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set due date of selected payable for.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPayableFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPayableFor.SelectedIndex != 0)
            {
                var dtFeeType = ViewState[S_DT_FEETYPE] as DataTable;
                DataRow[] drPayableFor = dtFeeType.Select("PayableFor='" + ddlPayableFor.SelectedItem.Text + "'");
                if (drPayableFor.Length > 0
                      && drPayableFor[0]["PaidDate"] != null
                        && drPayableFor[0]["PaidDate"].ToString() != String.Empty)
                {
                    cal_DueDate.DateValue = drPayableFor[0]["PaidDate"].ToDateTime();
                    txtAmt.Text = drPayableFor[0]["Amount"].ToString();
                }
                else
                {
                    txtDueDate.Text = String.Empty;
                    txtAmt.Text = string.Empty;
                }
            }
            else
                txtDueDate.Text = String.Empty;
            txtRemarks.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

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

    protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                string sLeftDate = grdStudents.DataKeys[e.Row.RowIndex]["SchoolLeft_Date"].ToString();
                if (sLeftDate != null && sLeftDate != Constants.S_EMPTY_STRING)
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
                    pageLabel.Text = Resources.LocalizedResources.PageNo + " "+ currentPage + Resources.LocalizedResources.Of + " " + grdStudents.PageCount + " " + Resources.LocalizedResources.OutOflst;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "PAY_FEE":
                    int iRowIndex = e.CommandArgument.ToInt();
                    int iStudentId = grdStudents.DataKeys[iRowIndex][0].ToInt();

                    ShowStudentForFeeEntry(iStudentId, iRowIndex);
                    trStudents.Visible = false;
                    break;
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
            if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdStudents.PageSize) - 1);
                if (e.ReturnValue.ToString() != string.Empty && e.ReturnValue != null)
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
                            lblError.Text = Resources.LocalizedResources.StudentNotFound;
                            ShowHideControls(false);
                            txtRegNumber.Enabled = true;
                            txtRegNumber.Focus();
                        }
                        else
                        {
                            trTotalRec.Visible = true;
                            trStudents.Visible = true;
                        }
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

    #endregion -- GRID EVENT HANDLER(s) --

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

        /// <summary>
    /// This method is used to delete fees.
    /// </summary>
    private void DeleteFee()
    {

        int iSerialNo = hidSerialNo.Value.ToInt();
        int iDebitId = hidDebitId.Value.ToInt();
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        if (iSerialNo != 0 && txtRegNumber.Text.Trim() == string.Empty)
        {
            ArrayList arrStdDivLst = GetStdDivIdLst();
            oStudentFeeDetailsBL.DeleteDebitFeeDetails(iSerialNo, arrStdDivLst, miUserId, IsInternalFee, miSchoolId, miAcademicYearId);
        }
        else
        {
            if (hidIsChequeBounc.Value == Constants.C_YES.ToString())
                oStudentFeeDetailsBL.DeleteStudentBounceChequeFeeDetails(iDebitId, IsInternalFee);
            else
            {
                if(!IsInternalFee)
                    oStudentFeeDetailsBL.DeleteStudentFeeDetails(iDebitId, miUserId);
                else
                    oStudentFeeDetailsBL.DeleteStudentInternalFeeDetails(iDebitId,miUserId);
            }
        }
        SendMessage(Constants.S_DELETED, (txtFeeType.Visible ? (txtFeeType.Text != string.Empty ? txtFeeType.Text : ddlOtherFeeTypes.SelectedItem.Text) : ddlFeeType.SelectedItem.Text),
                        txtDueDate.Text, txtAmt.Text, (txtPayableFor.Visible ? txtPayableFor.Text : ddlPayableFor.SelectedItem.Text));

        ClearAllControls();
        FillOtherFeeTypes();
        FillDebitGrid();

        chkSendMessage.Checked = false;
        chkSendSMS.Checked = false;
        btnSave.Enabled = true;
        btnDelete.Visible = false;
    }

    /// <summary>
    /// This method is used to edit fees.
    /// </summary>
    /// <param name="iSerialNo"></param>
    /// <param name="iRowindex"></param>
    /// <param name="iDebitId"></param>
    /// <param name="bIsInternalFee"></param>
    /// <param name="sDebitLevel"></param>
    private void EditFee(int iSerialNo, int iRowindex, int iDebitId, bool bIsInternalFee, string sDebitLevel)
    {
        trChequeNo.Visible = false;
        trMode.Visible = false;

        tblMsg.Visible = true;

        string sMessage = SetAlertMessage();
        sMessage = sMessage.Replace("'", "\\'");
        btnSave.Enabled = true;
        btnSave.Attributes.Add("onclick", string.Empty);
        btnSave.Attributes.Add("onclick", "if(!SendSms_Message('" + sMessage + "')) {return false;}");

        btnDelete.Visible = true;
        btnDelete.Enabled = true;
        btnDelete.Attributes.Add("onclick", string.Empty);
        bool bIsPaid = grdDebitInfo.DataKeys[iRowindex]["IsPaid"].ToBool();

        if (!bIsPaid)
            btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete('" + Constants.C_NO + "' , '" + sMessage + "')) {return false;}");
        else
        {
            btnDelete.Attributes.Add("onclick", "if(!SendSms_Message('" + sMessage + "')) {return false;}");
        }

        btnDelUnpaidFee.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        btnDisableOnlinePayment.Attributes.Add("onclick", "if(!ConfirmDisable()) return false;");
        
        hidCurrentLevel.Value = sDebitLevel;
        hidMode.Value = "Edit";
        hidDebitId.Value = iDebitId.ToString();
        hidSerialNo.Value = iSerialNo.ToString();
        
        if (txtRegNumber.Text.Trim() != string.Empty)
            SetDebitDetails(iDebitId);
        else
        {
            int iStdDivId = grdDebitInfo.DataKeys[iRowindex][Constants.I_ONE].ToInt();
            hidStdDivId.Value = iStdDivId.ToString();
            SetDebitDetails(iRowindex);
        }

        string sIsBounceChequeEntry = grdDebitInfo.DataKeys[iRowindex]["Is_Cheque_Bounce"].ToString();
        if (sIsBounceChequeEntry == Constants.C_YES.ToString())
        {
            btnSave.Enabled = false;
            hidIsChequeBounc.Value = Constants.C_YES.ToString();
            chkSendMessage.Checked = false;
            chkSendSMS.Checked = false;
            tdSMSLabel.Visible = false;
            btnDelete.Attributes.Add("onclick", string.Empty);
        }
        else
        {
            hidIsChequeBounc.Value = Constants.C_NO.ToString();
            tdSMSLabel.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to fill all combo boxes.
    /// </summary>
    private void FillAllComboBoxes()
    {
        FillStandardCombobox();
        ddlDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL));
        if (SchoolBase.Settings.DisplayAccountHeaders)
            FillAccountHeaderComboBox(false);
        else
            cmbAccountHeader.Visible = false;

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
        ddlDivision.Bind(dtDivision, S_STDDIV_ID_FLD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to show or hide controls.
    /// </summary>
    /// <param name="abFlag"></param>
    private void ShowHideControls(bool abFlag)
    {
        tblDebitEntry.Visible = abFlag;
        txtRegNumber.Enabled = !abFlag;
        ddlStandard.Enabled = !abFlag;
        ddlDivision.Enabled = !abFlag;
    }

    private void FillDebitInternalGrid()
    { 
    
    }

    /// <summary>
    /// This method is used to fill debit details frid.
    /// </summary>
    private void FillDebitGrid()
    {
        DataSet dsDebitDetails;
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();

        if (txtRegNumber.Text.Trim() != string.Empty)
        {
            hidDebitEntryLevel.Value = "Student";

            if (!IsInternalFee)
            {   
                dsDebitDetails = oStudentFeeDetailsBL.GetStudentDebitDetails(hidStudentId.Value.ToInt());
                ViewState[S_VW_CHEQUE] = dsDebitDetails.Tables[Constants.I_TWO];
                
                trFeeType.Visible = true;

                string sConcessionRule = string.Empty;

                if (dsDebitDetails.Tables[Constants.I_THREE].Rows.Count > Constants.I_ZERO)
                    sConcessionRule = dsDebitDetails.Tables[Constants.I_THREE].Rows[Constants.I_ZERO]["ConcessionRule"].ToString();
                trConcession.Visible = true;

                if (sConcessionRule != string.Empty)
                {
                    trConcession.Visible = true;
                    lblConcessionRule.Text = "* " + sConcessionRule;
                }
                else
                    trConcession.Visible = false;

                grdDebitInfo.Columns[6].Visible = false;

                if (dsDebitDetails.Tables[Constants.I_ZERO].Rows.Count > 0)
                {
                    hidStandardDivId.Value = dsDebitDetails.Tables[Constants.I_ZERO].Rows[0]["Standard_Div_Id"].ToString();
                }
                else if (dsDebitDetails.Tables[Constants.I_FOUR] != null && dsDebitDetails.Tables[Constants.I_FOUR].Rows.Count > 0)
                {
                    DataRow[] dtRows = dsDebitDetails.Tables[Constants.I_FOUR].Select("AcademicYearId = " + miAcademicYearId);
                    if (dtRows.Length > 0)
                        hidStandardDivId.Value = dtRows[0]["Standard_Div_Id"].ToString();
                }
                else
                {
                    var oDivCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
                    DataTable dtStdDivision = oDivCollectionBL.GetStdDivIdForClass(hidStandardId.Value.ToInt(), hidDivisionId.Value.ToInt());
                    if (dtStdDivision.Rows.Count > 0)
                        hidStandardDivId.Value = dtStdDivision.Rows[0]["SchoolWise_Standard_Division_Id"].ToString();
                }
            }
            else
            {   
                int iStudentId = hidStudentId.Value.ToInt();
                dsDebitDetails = oStudentFeeDetailsBL.GetDebitDetails(miSchoolId, miAcademicYearId, iStudentId);
                grdDebitInfo.Columns[6].Visible = false;

                GetInernalFeeCheckNos();

                if (dsDebitDetails.Tables[Constants.I_ZERO].Rows.Count > 0)
                {
                    hidStandardDivId.Value = dsDebitDetails.Tables[Constants.I_ZERO].Rows[0]["Standard_Div_Id"].ToString();
                }
            }
        }
        else
        {
            if (ddlStandard.SelectedIndex == 0)
            {
                hidDebitEntryLevel.Value = "School";
                trFeeType.Visible = false;
                dsDebitDetails = oStudentFeeDetailsBL.GetDebitDetails(miSchoolId, miAcademicYearId, IsInternalFee);
                grdDebitInfo.Columns[6].Visible = false;
            }
            else
            {
                int iStandardId = ddlStandard.SelectedValue.ToInt();
                trFeeType.Visible = true;
                if (ddlDivision.SelectedIndex == 0)
                {
                    hidDebitEntryLevel.Value = "Standard";
                    dsDebitDetails = oStudentFeeDetailsBL.GetDebitDetails(miSchoolId, miAcademicYearId, iStandardId, IsInternalFee);
                    grdDebitInfo.Columns[6].Visible = rdoFeeType.Items[Constants.I_ZERO].Selected;
                }
                else
                {
                    int iDivisionId = ddlDivision.SelectedValue.ToInt();
                    trFeeType.Visible = true;
                    hidDebitEntryLevel.Value = "Class";
                    dsDebitDetails = oStudentFeeDetailsBL.GetDebitDetails(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, IsInternalFee);
                    grdDebitInfo.Columns[6].Visible = false;
                }
            }
        }

        dsDebitDetails.Tables[Constants.I_ZERO].DefaultView.Sort = "Is_Cheque_Bounce DESC" + (hidSortExpression.Value != string.Empty ? ("," + hidSortExpression.Value + " " + hidSortDirection.Value) : string.Empty);

        grdDebitInfo.DataSource = dsDebitDetails.Tables[Constants.I_ZERO].DefaultView;
        grdDebitInfo.DataBind();
      

        if (grdDebitInfo.Rows.Count > 0)
        {
            trTotalAmt.Visible = true;
            txtAmtPaid.Text = dsDebitDetails.Tables[Constants.I_ONE].Rows[Constants.I_ZERO][Constants.I_ZERO].ToString();
        }
        else
        {
            trTotalAmt.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to populate StudentFeeDetailsBL and returns its object.
    /// </summary>
    /// <returns></returns>
    private StudentFeeDetailsBL PopulateFeeDetailsBL()
    {
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL
            {
                Academic_Year_Id = miAcademicYearId,
                School_Id = miSchoolId,
                Inserted_By_id = miUserId,
                Insert_Date = DateTime.Now,
                Amount = txtAmt.Text.ToInt(),
                DebitOrCredit = "Debit",
                Paid_Date = cal_DueDate.DateValue,
                Remarks = txtRemarks.Text.Trim()
            };

        oStudentFeeDetailsBL.IsDueDateApplicable = true;
        if (chkNotApplicable.Checked)
        {
            oStudentFeeDetailsBL.IsDueDateApplicable = false;
            oStudentFeeDetailsBL.Paid_Date = DateTime.Now.Date;
        }

        if (chkConsiderForOnline.Checked)
            oStudentFeeDetailsBL.IsConsiderForOnlinePayment = true;
        else
            oStudentFeeDetailsBL.IsConsiderForOnlinePayment = false;

        if (ddlFeeType.Visible)
        {
            oStudentFeeDetailsBL.Payable_For = ddlPayableFor.SelectedItem.Text;
            
            oStudentFeeDetailsBL.FeeType = ddlFeeType.SelectedItem.Text;
            oStudentFeeDetailsBL.Std_FeeType_Id = ddlFeeType.SelectedValue.ToInt();
        }
        else
        {
            oStudentFeeDetailsBL.Payable_For = txtPayableFor.Text.Trim();

           

            if (txtFeeType.Text.Trim() != string.Empty)
                oStudentFeeDetailsBL.FeeType = txtFeeType.Text.Trim();
            else
                oStudentFeeDetailsBL.FeeType = ddlOtherFeeTypes.SelectedValue;

        }

        if (txtRegNumber.Text.Trim() != string.Empty)
            oStudentFeeDetailsBL.Student_Id = hidStudentId.Value.ToInt();
        
        oStudentFeeDetailsBL.ConsiderRTEStudent = (chkRTEStudent.Checked == true) ? false : true;
        
        if (SchoolBase.Settings.DisplayAccountHeaders)
        {
            if (cmbAccountHeader.SelectedIndex.ToString() == Constants.S_ZERO)
                oStudentFeeDetailsBL.AccountHeaderId = Constants.I_ZERO;
            else
                oStudentFeeDetailsBL.AccountHeaderId = cmbAccountHeader.SelectedValue.ToInt();
        }

        return oStudentFeeDetailsBL;
    }

    /// <summary>
    /// This method is used to get stddivid for selected standard-division 
    /// and add it to arralist and return arraylist.
    /// </summary>
    /// <returns></returns>
    private ArrayList GetStdDivIdLst()
    {
        DataTable odtStdDivId;
        var oarrStdDivIdLst = new ArrayList();
        var oDivisionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        if (ddlStandard.SelectedIndex == 0)
            odtStdDivId = oDivisionBL.GettStdDivIdForSchool();
        else
        {
            int iStandardId = ddlStandard.SelectedValue.ToInt();
            if (ddlStandard.SelectedIndex != 0 && ddlDivision.SelectedIndex == 0)
                odtStdDivId = oDivisionBL.GetAllDivisionsForStandard(iStandardId);
            else
            {
                int iDivisionId = ddlDivision.SelectedValue.ToInt();
                odtStdDivId = oDivisionBL.GetStdDivIdForClass(iStandardId, iDivisionId);
            }
        }

        for (int iRowCnt = 0; iRowCnt < odtStdDivId.Rows.Count; iRowCnt++)
        {
            int iStdDivId = odtStdDivId.Rows[iRowCnt][Constants.I_ZERO].ToInt();
            oarrStdDivIdLst.Add(iStdDivId);
        }

        return oarrStdDivIdLst;
    }

    /// <summary>
    /// This method is used to clear all controls.
    /// </summary>
    private void ClearAllControls()
    {
        hidMode.Value = Constants.S_NEW_MODE;
        txtFeeType.Enabled = true;
        txtPayableFor.ReadOnly = false;
        if(SchoolBase.Settings.DisplayAccountHeaders)
        cmbAccountHeader.SelectedIndex = Constants.I_ZERO;

        txtAmt.Text = string.Empty;
        txtDueDate.Text = string.Empty;
        txtFeeType.Text = string.Empty;
        txtPayableFor.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        trChequeNo.Visible = false;
        trMode.Visible = false;
        rdoFeeType.Items[Constants.I_ZERO].Selected = true;
        rdoFeeType.Items[Constants.I_ONE].Selected = false;
        ddlPayableFor.Items.Clear();
        ddlChequeNo.Items.Clear();
        rdoFeeType.Items[2].Selected = false;
        VisibleDisableControls(false);
        ddlOtherFeeTypes.Visible = true;
        feetypeSeparator.Visible = true;
        chkRTEStudent.Checked = false;
        chkRTEStudent.Enabled = (txtRegNumber.Text == string.Empty) ? true : false;
        SetDueDateStatus();

        chkConsiderForOnline.Checked = false;

        btnDelUnpaidFee.Visible = false;
        btnDisableOnlinePayment.Visible = false;
        hidSerialNo.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetAcademicYearDates()
    {
        DataTable oDT;
        if (!ddlStandard.SelectedValue.IsNullOrEmpty() && ddlStandard.SelectedValue.ToInt() > 0)
        {
            oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId, miAcademicYearId, ddlStandard.SelectedValue.ToInt());
            if (oDT.Rows.Count > 0)
            {
                hidYearStartDate.Value = oDT.Rows[0]["StartDate"].ToString();
                hidYearEndDate.Value = oDT.Rows[0]["EndDate"].ToString();
            }
            else
            {
                hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
                hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
            }
        }
        else if (txtRegNumber.Text != string.Empty)
        {
            oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStudent(miSchoolId, miAcademicYearId, txtRegNumber.Text);
            if (oDT.Rows.Count > 0)
            {
                hidYearStartDate.Value = oDT.Rows[0]["StartDate"].ToString();
                hidYearEndDate.Value = oDT.Rows[0]["EndDate"].ToString();
            }
            else
            {
                hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
                hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
            }
        }
        else
        {
            hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
            hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
        }

        cal_DueDate.From.Date = hidYearStartDate.Value.ToDateTime();
        cal_DueDate.To.Date = hidYearEndDate.Value.ToDateTime();
        hidServerDate.Value = Convert.ToString(DateTime.Today);
    }

    /// <summary>
    /// This function sets the date format for date column property 
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        var oReceivedDate = grdDebitInfo.Columns[I_COLUMN_INDEX_DUE_DATE] as BoundField;
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        ddlStandard.Focus();
    }

    /// <summary>
    /// This method is used to set hidden field to default value for grid sorting.
    /// </summary>
    private void SetSortingFieldDefaultValues()
    {
        hidSortExpression.Value = grdDebitInfo.Columns[2].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to set values to controls.
    /// </summary>
    /// <param name="aiDebitId"></param>
    private void SetDebitDetails(int aiDebitId)
    {
        rdoFeeType.Items[Constants.I_ZERO].Selected = true;
        rdoFeeType.Items[Constants.I_ONE].Selected = false;
        rdoFeeType.Items[2].Selected = false;
        VisibleDisableControls(false);
        feetypeSeparator.Visible = true;
        if (txtRegNumber.Text.Trim() != string.Empty)
        {   
            StudentFeeDetailsBL oStudentFeeDetailsBL;
            oStudentFeeDetailsBL = new StudentFeeDetailsBL(aiDebitId, IsInternalFee);
            
            txtAmt.Text = oStudentFeeDetailsBL.Amount.ToString();
            if (ddlOtherFeeTypes.Items.Contains(new ListItem(oStudentFeeDetailsBL.FeeType)))
            {
                ddlOtherFeeTypes.SelectedValue = oStudentFeeDetailsBL.FeeType;
                txtFeeType.Enabled = false;
            }
            else
            {
                txtFeeType.Enabled = true;
                txtFeeType.Text = oStudentFeeDetailsBL.FeeType;
                ddlOtherFeeTypes.Visible = false;
                feetypeSeparator.Visible = false;
           } 


            txtPayableFor.Text = oStudentFeeDetailsBL.Payable_For;
            cmbAccountHeader.SelectedValue = oStudentFeeDetailsBL.AccountHeaderId.ToString();
            txtRemarks.Text = oStudentFeeDetailsBL.Remarks;
            cal_DueDate.DateValue = oStudentFeeDetailsBL.Paid_Date.ToDateTime();
            if (oStudentFeeDetailsBL.Std_FeeType_Id != 0)
            {
                txtFeeType.ReadOnly = true;
                txtPayableFor.ReadOnly = true;
            }
            else
            {
                txtFeeType.ReadOnly = false;
                txtPayableFor.ReadOnly = false;
            }

            chkConsiderForOnline.Checked = oStudentFeeDetailsBL.IsConsiderForOnlinePayment;
        }
        else
        {

            txtAmt.Text = grdDebitInfo.Rows[aiDebitId].Cells[3].Text;

            bool bIsDueDateApplicable = grdDebitInfo.DataKeys[aiDebitId]["IsDueDateApplicable"].ToBool();
            if (bIsDueDateApplicable)
            {
                cal_DueDate.DateValue = grdDebitInfo.Rows[aiDebitId].Cells[2].Text.ToDateTime();
                chkNotApplicable.Checked = false;
            }
            else
            {
                txtDueDate.Text = string.Empty;
                txtDueDate.Enabled = false;
                chkNotApplicable.Checked = true;
            }

            if (ddlOtherFeeTypes.Items.Contains(new ListItem(System.Web.HttpUtility.HtmlDecode(grdDebitInfo.Rows[aiDebitId].Cells[0].Text))))
                ddlOtherFeeTypes.SelectedValue = System.Web.HttpUtility.HtmlDecode(grdDebitInfo.Rows[aiDebitId].Cells[0].Text);
            else
            {
                txtFeeType.Enabled = true;
                txtFeeType.Text = System.Web.HttpUtility.HtmlDecode(grdDebitInfo.Rows[aiDebitId].Cells[0].Text);
                ddlOtherFeeTypes.Visible = false;
                feetypeSeparator.Visible = false;
            }
            txtFeeType.Enabled = false;
            txtPayableFor.Text = System.Web.HttpUtility.HtmlDecode(grdDebitInfo.Rows[aiDebitId].Cells[1].Text);
           
            string sRemark = System.Web.HttpUtility.HtmlDecode(grdDebitInfo.Rows[aiDebitId].Cells[4].Text);

            if (sRemark.Trim() == "&nbsp;" || sRemark.Trim() == string.Empty)
                txtRemarks.Text = string.Empty;
            else
                txtRemarks.Text = sRemark.Trim();

            chkRTEStudent.Checked = !Convert.ToBoolean(grdDebitInfo.DataKeys[aiDebitId]["IsConsiderForRTEStudent"]);
            cmbAccountHeader.SelectedValue = grdDebitInfo.DataKeys[aiDebitId]["AccountHeaderId"].ToString();

            chkConsiderForOnline.Checked = Convert.ToBoolean(grdDebitInfo.DataKeys[aiDebitId]["IsOnlinePaymentApplicable"]);

        }
    }

    /// <summary>
    /// This method is used to check date valiadation.
    /// </summary>
    /// <returns>string</returns>
    private string CheckIfDateValid()
    {
        var sReturnErrMsg = new StringBuilder();

        // check for holiday
        sReturnErrMsg.Append(CheckForHolidayAndSetErrMsg());
        return sReturnErrMsg.ToString();
    }

    /// <summary>
    /// This method is used to check selected date is holiday or weekday and give message if yes.
    /// </summary>
    /// <returns>string</returns>
    private string CheckForHolidayAndSetErrMsg()
    {
        string sReturnErrMsg = string.Empty;
        DataSet oDSnonWorkingdayStartDate;
        var oSchoolWiseAttendanceDetailsBL = new AttendanceDetailsBL();

        if (ddlStandard.SelectedIndex == 0)
            oDSnonWorkingdayStartDate = oSchoolWiseAttendanceDetailsBL.IsDayNonWorking(cal_DueDate.DateValue, miSchoolId, miAcademicYearId, 0);
        else
        {
            int iStandardId = ddlStandard.SelectedValue.ToInt();
            oDSnonWorkingdayStartDate = oSchoolWiseAttendanceDetailsBL.IsDayNonWorking(cal_DueDate.DateValue, miSchoolId, miAcademicYearId, iStandardId);
        }

        if (oDSnonWorkingdayStartDate.Tables[1].Rows[0][0].ToString() != Convert.ToString(Constants.I_ZERO))
        {
            sReturnErrMsg = Constants.S_VALIDATION_SUMMARY_HEADER + "</Br>";
            sReturnErrMsg = sReturnErrMsg + "</br>" + Resources.LocalizedResources.DueDateShouldNotBeHoliday;
            return sReturnErrMsg;
        }

        if (oDSnonWorkingdayStartDate.Tables[0].Rows[0][0].ToString() == Convert.ToString(Constants.I_ZERO))
        {
            sReturnErrMsg = Constants.S_VALIDATION_SUMMARY_HEADER + "</Br>";
            sReturnErrMsg = sReturnErrMsg + "</Br>" + Resources.LocalizedResources.DueDateShouldBeWorkingDay;
            return sReturnErrMsg;
        }

        return sReturnErrMsg;
    }

    /// <summary>
    /// This method creates an XML for standard-division ids list.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForStdDivIds()
    {
        const string S_STD_DIV_ID = "StdDivIdsList";
        const string S_STD_DIV = "StdDiv";

        var oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement(S_STD_DIV_ID);
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_STD_DIV_ID, string.Empty);

        ArrayList arrStdDivId = GetStdDivIdLst();
        for (int iCnt = 0; iCnt < arrStdDivId.Count; iCnt++)
        {
            int iStdDivId = arrStdDivId[iCnt].ToString().ToInt();

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STD_DIV, string.Empty);

            string sAtrrName = "Std_Div_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = iStdDivId.ToString();
            oXmlNode.Attributes.Append(attr);
            oXmlRootNode.AppendChild(oXmlNode);
        }

        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to visible or hide controls on the basis of fee type(existing or new) selection.
    /// </summary>
    /// <param name="abFlag"></param>
    private void VisibleDisableControls(bool abFlag)
    {
        txtFeeType.Visible = !abFlag;
        txtPayableFor.Visible = !abFlag;
        ddlFeeType.Visible = abFlag;
        ddlPayableFor.Visible = abFlag;
        txtAmt.Text = string.Empty;
        txtDueDate.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtFeeType.Text = string.Empty;
        txtPayableFor.Text = string.Empty;
    }

    /// <summary>
    /// This method creates an XML for bounce cheque debit entry details.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForBouncedChequeDetails()
    {
        const string S_STUDENT = "Student";
        var oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("BounceChequeDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "BounceChequeDetails", string.Empty);

        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_STUDENT, string.Empty);

        string sAtrrName1 = "DueDate";
        XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
        attr1.Value = txtDueDate.Text;
        oXmlNode.Attributes.Append(attr1);

        string sAtrrName2 = "PayableFor";
        XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
        attr2.Value = txtPayableFor.Text;
        oXmlNode.Attributes.Append(attr2);

        string sAtrrName3 = "FeeType";
        XmlAttribute attr3 = oDoc.CreateAttribute(sAtrrName3);
        attr3.Value = txtFeeType.Text;
        oXmlNode.Attributes.Append(attr3);

        string sAtrrName4 = "Remarks";
        XmlAttribute attr4 = oDoc.CreateAttribute(sAtrrName4);
        attr4.Value = txtRemarks.Text;
        oXmlNode.Attributes.Append(attr4);

        string sAtrrName5 = "Amount";
        XmlAttribute attr5 = oDoc.CreateAttribute(sAtrrName5);
        attr5.Value = txtAmt.Text;
        oXmlNode.Attributes.Append(attr5);

        string sAtrrName6 = "AccountHeaderId";
        XmlAttribute attr6 = oDoc.CreateAttribute(sAtrrName6);
        attr6.Value = (cmbAccountHeader.Visible ? cmbAccountHeader.SelectedValue : Constants.S_ZERO);
        oXmlNode.Attributes.Append(attr6);

        oXmlRootNode.AppendChild(oXmlNode);

        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    private void ShowStudentForFeeEntry(int aiStudentId, int aiRowIndex)
    {
        if (grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"] != null && grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"].ToString() != string.Empty)
        {
            lblLeft.Text = "Student left on " + grdStudents.DataKeys[aiRowIndex]["SchoolLeft_Date"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
            trLeftStudent.Visible = true;
        }
        else
            trLeftStudent.Visible = false;

        var oStudentBL = new StudentBL(aiStudentId);

        hidStudentId.Value = oStudentBL.YearWiseStudentId.ToString();
        hidStandardId.Value = oStudentBL.StandardId.ToString();
        hidDivisionId.Value = oStudentBL.DivisionId.ToString();
        SetStudentInfo(oStudentBL);
    }

    private void SetStudentInfo(StudentBL aoStudentBL)
    {
        btnShow.Text = Resources.LocalizedResources.ChangeInput;
        hidShow.Value = "Change Input";
        if (aoStudentBL.EnrolementNo != Constants.S_EMPTY_STRING)
            txtRegNumber.Text = aoStudentBL.EnrolementNo;
        else
            txtRegNumber.Text = aoStudentBL.SalutationName + " " + aoStudentBL.FirstName + " " + aoStudentBL.MiddleName + " " + aoStudentBL.LastName;
        lblError.Visible = false;

        FillOtherFeeTypes();

        ShowHideControls(true);
        hidStudentId.Value = aoStudentBL.YearWiseStudentId.ToString();
        hidStandardId.Value = aoStudentBL.StandardId.ToString();
        hidStudentName.Value = aoStudentBL.SalutationName + " " + aoStudentBL.FirstName + " " + aoStudentBL.MiddleName + " " + aoStudentBL.LastName;
        rdoFeeType.Items[2].Enabled = true;
        tblStudentInfo.Visible = true;
        lblStudentName.Text = aoStudentBL.SalutationName + " " + aoStudentBL.FirstName + " " + aoStudentBL.MiddleName + " " + aoStudentBL.LastName;
        lblRollNumber.Text = aoStudentBL.RollNo.ToString();
        lblStandardDivision.Text = aoStudentBL.StandardDivisionName;
        if (Session[Constants.S_SESSION_IS_NEW_ADMISSION] != null && Session[Constants.S_SESSION_IS_NEW_ADMISSION].ToString() == "False")
        {
            VisibleJoinDateControl(true);
            lblJoiningDate.Text = aoStudentBL.JoiningDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
        }
        else
            VisibleJoinDateControl(false);
        FillDebitGrid();
    }

    private void SetStudentGridViewDateColumnProperties()
    {
        var oDOB = grdStudents.Columns[I_COLUMN_INDEX_DOB] as BoundField;
        oDOB.HtmlEncode = false;
        oDOB.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        var oLeftDate = grdStudents.Columns[I_COLUMN_INDEX_LEFT_DATE] as BoundField;
        oLeftDate.HtmlEncode = false;
        oLeftDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method is used to visible or hide  JoinDate Controls.
    /// </summary>
    /// <returns></returns>
    private void VisibleJoinDateControl(bool abFlag)
    {
        tdJoiningDate.Visible = abFlag;
        tdJoinDate.Visible = abFlag;
    }

    /// <Summary>
    /// This method is used to set alert message on click of save.
    /// </Summary>   
    private string SetAlertMessage()
    {
        if (!txtRegNumber.Text.IsNullOrEmpty())
            return " selected student";

        if (ddlStandard.SelectedItem.Text.Equals(Constants.S_SELECT_ALL))
            return " all students";

        return !ddlDivision.SelectedItem.Text.Equals(Constants.S_SELECT_ALL) ? " students of class " + ddlStandard.SelectedItem.Text + " " + ddlDivision.SelectedItem.Text : " students of standard " + ddlStandard.SelectedItem.Text;
    }

    private string GetSMSTemplate(int iSmsId)
    {
        string sLoginDetailsSmsText = string.Empty;
        string sChequeNo = ddlChequeNo.SelectedItem.Text;
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
                sLoginDetailsSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);
        }

        DataTable oDataTable = SMSReceiverDetailsBL.GetBouncedChequeDetails(miSchoolId, miAcademicYearId, txtRegNumber.Text.Trim(), sChequeNo, IsInternalFee);
        if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
        {
            int iChequeAmount = oDataTable.Rows[0]["Cheque_Amount"].ToInt();
            string sChequeDate = oDataTable.Rows[0]["Cheque_Date"].ToDateTime().ToString("dd MMM yyyy");
            sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%CHEQUENO%", sChequeNo).Replace("%CHEQUEDATE%", sChequeDate).Replace("%AMOUNT%", iChequeAmount.ToString());
        }

        return sLoginDetailsSmsText;
    }

    /// <summary>
    /// This method is used to send login details sms to parent.
    /// </summary>
    private string SendChequeBounceSMS()
    {
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        if (Convert.ToString(hidSendSms.Value) == "Y")
        {
            string sChequeNo = ddlChequeNo.SelectedItem.Text;
            int iSmsId = Constants.SMSTemplate.ChequeBounceSMS.ToInt();
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDTSmsTemplate.Rows.Count != 0)
            {
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                {
                    sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                    if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
                }
            }

            DataTable oDataTable = SMSReceiverDetailsBL.GetBouncedChequeDetails(miSchoolId, miAcademicYearId, txtRegNumber.Text.Trim(), sChequeNo, IsInternalFee);
            if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
            {
                string sMobileNo = oDataTable.Rows[0]["Mobile_Number"].ToString();
                string sMobileNo2 = oDataTable.Rows[0]["Mobile_Number2"].ToString();
                int iChequeAmount = oDataTable.Rows[0]["Cheque_Amount"].ToInt();
                string sChequeDate = oDataTable.Rows[0]["Cheque_Date"].ToDateTime().ToString("dd MMM yyyy");

                int iUserId = oDataTable.Rows[0]["User_Id"].ToInt();
                var oSchoolBL = new SchoolBL(miSchoolId);
                string sDisplayText = lblStudentName.Text + " (" + lblStandardDivision.Text + " - " + txtRegNumber.Text.Trim() + ")";
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%CHEQUENO%", sChequeNo).Replace("%CHEQUEDATE%", sChequeDate).Replace("%AMOUNT%", iChequeAmount.ToString());
                if (Convert.ToString(hidSendSms.Value) == Constants.S_YES)
                {
                    var oSMS = new SMS
                                   {
                                       Sender = oSchoolBL.SMSSenderName,
                                       SMSText = sLoginDetailsSmsText,
                                       School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject,
                                       TemplateRegistrationId = sTemplateRegistrationId,
                                       DisplayText = sDisplayText
                                   };
                    oSMS.To.Add(iUserId, sMobileNo);
                    if (sMobileNo2 != string.Empty)
                        oSMS.To.Add(iUserId + "sm;", sMobileNo2);
                    oSMS.Send();
                }
            }

            hidSendSms.Value = "N";
        }

        return sLoginDetailsSmsText;
    }

    /// <summary>
    ///		This method is used to send a sms for new fee type added/updated.
    /// </summary>
    /// <param name="asIsNew"></param>
    /// <param name="asFeeType"></param>
    /// <param name="asDueDate"></param>
    /// <param name="asAmount"></param>
    /// <param name="asPayableFor"></param>
    private void SendMessage(string asIsNew, string asFeeType, string asDueDate, string asAmount, string asPayableFor)
    {
        var oMasterPage = this.Master as MasterPage;
        int iSmsId;
        if (asIsNew.Equals(Constants.S_NEW_MODE))
            iSmsId = Constants.SMSTemplate.NewFeesSMS.ToInt();
        else if (asIsNew.Equals(Constants.S_EDIT_MODE))
            iSmsId = Constants.SMSTemplate.NewFeesUpdateSMS.ToInt();
        else
            iSmsId = Constants.SMSTemplate.FeesDeletedSMS.ToInt();

        string sQueryString;

        if (hidSendSms.Value.Equals(Constants.S_YES) && hidSendMsg.Value.Equals(Constants.S_YES))
        {
            // Redirect to Sms center with a flag that will further redirect to message center.
            sQueryString = PrepareQueryString(true, iSmsId, asFeeType, asDueDate, asAmount, asPayableFor);
            oMasterPage.RedirectToNextPage("~/Common/SMSUI.aspx?" + sQueryString);
            hidSendSms.Value = "N";
            hidSendMsg.Value = "N";
        }
        else if (hidSendSms.Value.Equals(Constants.S_YES))
        {
            // Redirect to Sms center with a flag that will not redirect to message center.
            sQueryString = PrepareQueryString(false, iSmsId, asFeeType, asDueDate, asAmount, asPayableFor);
            oMasterPage.RedirectToNextPage("~/Common/SMSUI.aspx?" + sQueryString);
            hidSendSms.Value = "N";
        }
        else if (hidSendMsg.Value.Equals(Constants.S_YES))
        {
            // Redirect to message center.
            sQueryString = PrepareQueryString(false, iSmsId, asFeeType, asDueDate, asAmount, asPayableFor);
            oMasterPage.RedirectToNextPage("~/Common/SendMessageFromInbox.aspx?" + sQueryString);
            hidSendMsg.Value = "N";
        }
    }

    /// <summary>
    /// This method is used to prepare Query Strings.
    /// </summary>
    private string PrepareQueryString(bool abIsRedirectToMsgCenter, int aiSmsId, string sFeeType, string sDueDate, string sAmount, string asPayableFor)
    {
        const string S_PAGE = "StudentPayables";
        string sQuerystring = "From=" + S_PAGE + (abIsRedirectToMsgCenter ? "&SendMsg=Y" : string.Empty) + "&SmsId=" + aiSmsId
                              + "&StandardId=" + (ddlStandard.SelectedIndex != 0 ? ddlStandard.SelectedValue : "0")
                              + "&DivisionId=" + (ddlDivision.SelectedIndex != 0 ? ddlDivision.SelectedValue : "0")
                              + "&RegNo=" + txtRegNumber.Text
                              + "&FeeType=" + sFeeType
                              + "&DueDate=" + sDueDate
                              + "&Amount=" + sAmount
                              + "&PayableFor=" + asPayableFor
                              + "&ConsiderForRTEConcession=" + chkRTEStudent.Checked;

        string sQueryString = CommonUtility.EncryptQuerystring(sQuerystring);

        return sQueryString;
    }

    /// <summary>
    /// This method is used to set display text for a message/sms.
    /// </summary>
    private string SetDisplayTextToMsg()
    {
        if (!hidStudentName.Value.IsNullOrEmpty() && !txtRegNumber.Text.IsNullOrEmpty())
            return hidStudentName.Value;

        if (ddlStandard.SelectedItem.Text.Equals(Constants.S_SELECT_ALL))
            return "All students";

        return !ddlDivision.SelectedItem.Text.Equals(Constants.S_SELECT_ALL) ? "Students of class" + ddlStandard.SelectedItem.Text + " " + ddlDivision.SelectedItem.Text : "Students of standard" + ddlStandard.SelectedItem.Text;
    }

    private void FillOtherFeeTypes()
    {
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        List<string> lstOtherFeeTypes = oStudentFeeDetailsBL.GetOtherFeeTypes(miSchoolId, IsInternalFee);
        ddlOtherFeeTypes.Items.Clear();
        if (lstOtherFeeTypes != null && lstOtherFeeTypes.Count > 0)
        {
            ddlOtherFeeTypes.DataSource = lstOtherFeeTypes;
            ddlOtherFeeTypes.DataBind();
            ddlOtherFeeTypes.Items.Insert(0, new ListItem(Constants.S_SELECT));
            ddlOtherFeeTypes.SelectedIndex = 0;
        }
        else
        {
            ddlOtherFeeTypes.Visible = false;
            feetypeSeparator.Visible = false;
        }

    }

    /// <summary>
    /// This method is used to set design according to the selected language
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        btnShow.Text = oResourceManager.GetString(hidShow.Value.Replace(" ", string.Empty));
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valErrMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAmountShouldNotBeBlank.Value = Resources.LocalizedResources.AmountShouldNotBeBlank;
        hidAmountShouldNotBeZero.Value = Resources.LocalizedResources.AmountShouldNotBeZero;
        hidChequeNumberShouldBeSelected.Value = Resources.LocalizedResources.ChequeNumberShouldBeSelected;
        hidFeeTypeShouldNotBeBlank.Value = Resources.LocalizedResources.FeeTypeShouldNotBeBlank;
        hidFeeTypeShouldBeSelected.Value = Resources.LocalizedResources.FeeTypeShouldBeSelected;
        hidPayableForShouldNotBeBlank.Value = Resources.LocalizedResources.PayableForShouldNotBeBlank;
        hidPayableForShouldBeSelected.Value = Resources.LocalizedResources.PayableForShouldBeSelected;
        hidDueDateShouldNotBeBlank.Value = Resources.LocalizedResources.DueDateShouldNotBeBlank;
        hidDoYouWantToSendFollowingSMSMessage.Value = Resources.LocalizedResources.DoYouWantToSendFollowingSMSMessage;
        hidAreYouSureYouWantToDeleteThisBounceChequeTransaction.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisBounceChequeTransaction;
        hidAreYouSureYouWantToDeleteThisDebitDetails.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisDebitDetails;
        hidDoYouWantToSendSMSTo.Value = Resources.LocalizedResources.DoYouWantToSendSMSTo;
        hidDoYouWantToSendMessageTo.Value = Resources.LocalizedResources.DoYouWantToSendMessageTo;
    }

    /// <summary>
    /// This method is used to set account header status.
    /// </summary>
    /// <param name="abAction"></param>
    private void AccountHeaderStatus(bool abAction)
    {
        if (!SchoolBase.Settings.DisplayAccountHeaders)
            cmbAccountHeader.Visible = false;
        else        
            cmbAccountHeader.Visible = abAction;            
    }


    private void GetInernalFeeCheckNos()
    {
        var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataSet dsChequeDetails = oStudentFeeDetailsBL.GetInternalFeesChequeDetails(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt());
        ViewState[S_VW_CHEQUE] = dsChequeDetails.Tables[0];
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["RegNo"].IsNull())
        { 
            txtRegNumber.Text = QueryString["RegNo"];
            btnShow_Click(btnShow, null);
        }
       
    }

    private List<PaymentMode> GetModes(bool abIsInternalFee)
    {
        List<PaymentMode> lstPaymentMode = new List<PaymentMode>();
        if (abIsInternalFee)
        {
            lstPaymentMode.Add(new PaymentMode { Id = 1, Name = "Cheque" });
        }
        else
        {
            lstPaymentMode.Add(new PaymentMode { Id = 1, Name = "Cheque" });
            lstPaymentMode.Add(new PaymentMode { Id = 2, Name = "Swipe Card" });
            lstPaymentMode.Add(new PaymentMode { Id = 3, Name = "Electronic" });
            lstPaymentMode.Add(new PaymentMode { Id = 4, Name = "Online Transaction" });
        }

        return lstPaymentMode;
    }

    private void DisableValidator()
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            ReqAccountHeader.Enabled = true;
            spnStar.Visible = true;
        }
        else
        {
            ReqAccountHeader.Enabled = false;
            spnStar.Visible = false;
        }
    }

    private void DisableOrDeleteUnpaidFee(bool abIsDisable)
    {
        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        oStudentFeeDetailsBL.DisableOrDeleteUnpaidFee(miSchoolId, miAcademicYearId, ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), hidSerialNo.Value, abIsDisable, miUserId);
    }
    
    private void ResetFields()
    {        
        ClearAllControls();
        chkSendMessage.Checked = false;
        chkSendSMS.Checked = false;
        btnSave.Enabled = true;
        btnDelete.Visible = false;
        FillDebitGrid();
    }

     #endregion -- PRIVATE METHOD(s) --

    public class PaymentMode
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
