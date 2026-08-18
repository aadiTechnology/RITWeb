
// File Name   : InCompletedTransaction.aspx.cs
// Created By  : Milind
// Date        : 03 Dec 2009
// Description : This class is used to display the incomplete transaction of the online fee. And also 
//               mark that incomplete transction as complete and also delete the particular transaction.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Accounts;
using Utility;
using SchoolEntities;

public partial class InCompletedTransactionUI :SchoolBase
{

    #region Constants

    const string S_SHOW = "Show";
    const string S_CHANGE_INPUT = "Change Input";
    const string S_DEFUALT_SORT_EXPR = "TransactionDateTime";
    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_FAIL = "Fail";
    const string S_COMMAND_COMPLETE = "Complete";
    const string S_COMMAND_INCOMPLETE = "InComplete";
    NetBankingPaymentTransactionsBL moNetBankingPaymentTransactionsBL;
    #endregion

    #region Events

    /// <summary>
    /// This event is used to set all the controls for StudentFee.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {        
        try
        {
            moNetBankingPaymentTransactionsBL = new NetBankingPaymentTransactionsBL(miSchoolId);
            if (!IsPostBack)
            {
                
                SetSortVariables();
                valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;                
                btnOk.Attributes.Add("onclick", "VerifyClose()");
                SetAllControlsForStudentFee();
                ApplyMouseHoverEffect(new List<Button> { btnCancel, btnOk, btnShow });
                SetDefaultControls();
                //  GetPaymentGatewayURL();
                SetJavascriptAttributes();
                SetUrlToLinkButton();
                optRegNo.Checked = true;
                hidPaymentCategoryFeeId.Value = "1";
               optIncomplte.Checked = true;
               SetAllControlsForTransactionDate();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        
    }

    /// <summary>
    /// This event is used to Show/Hide the incomplete transaction according to show button text.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ToggleStatus();
              
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Complete transaction after click on Ok
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            int iTranscationId = Convert.ToInt32(hidTranscationId.Value);
            string sTPSLTransactionID = txtTPSLTransactionID.Text.Trim();            
            NetBankingTransaction oNetBankingTransaction=new NetBankingTransaction
            {
                NetBankingPaymentTransactionID = iTranscationId,
                TPSLTransactionID = sTPSLTransactionID,
                TransactionStatus = Constants.TransactionStatus.Completed,
                TransactionBankID = cmbBankName.SelectedValue,
                TransactionAMT = txtAmount.Text.ToDouble()
            };

            if (optRegNo.Checked || optCautionMoney.Checked || optInternalFee.Checked || optTransactionDate.Checked)
            {
                oNetBankingTransaction.TransactionFor = Constants.OnlineFeeTypes.StudentFee.ToInt();
                string sMessage = CompleteTransaction(lstDSobj.ID, oNetBankingTransaction, hidPaymentCategoryFeeId.Value.ToInt());                
                if (!sMessage.IsNullOrEmpty())
                    lblErr.Text = sMessage;
                
            }
            else if (optAdmission.Checked)
            {
                oNetBankingTransaction.TransactionFor = Constants.OnlineFeeTypes.AdmissionFee.ToInt();
                CompleteTransaction(objAdmission.ID, oNetBankingTransaction, Constants.I_ZERO);
            }
            if (hidSendSMS.Value == Constants.S_YES)
                SendSMS();
            txtTPSLTransactionID.Text = string.Empty;
            hidGatewayId.Value = Constants.S_ZERO;
                     
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// To set controls for student fees when optRegNo checked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRegNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optRegNo.Checked)
            {
                hidPaymentCategoryFeeId.Value = "1";
                SetAllControlsForStudentFee();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// To set controls for caution money when optCautionMoney checked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optCautionMoney_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optCautionMoney.Checked)
            {
                hidPaymentCategoryFeeId.Value = "2";
                SetAllControlsForStudentFee();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// To set controls for Internal Fee when optInternalFee checked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optInternalFee_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optInternalFee.Checked)
            {
              
                hidPaymentCategoryFeeId.Value = "3";
                lblDateMandatory.Visible = false;
                //SetAllControlsForStudentFee();                
            }
                
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// To set controls for admission when optAdmission checked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optAdmission_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optAdmission.Checked)
                SetAllControlsForAddmission();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// To set controls for admission when optTransactionDate checked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optTransactionDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {   
             SetAllControlsForTransactionDate();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region ListView Events

    /// <summary>
    /// This event is used To bound Item to Listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransaction_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool bRegNo = true;
                if (optAdmission.Checked)
                    bRegNo = false;
                System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableHeaderRow = (System.Web.UI.HtmlControls.HtmlTableRow)lstvwTransaction.FindControl("trHeader");
                
                System.Web.UI.HtmlControls.HtmlTableCell thFormNo = (System.Web.UI.HtmlControls.HtmlTableCell)oHtmlTableHeaderRow.FindControl("thFormNo");
                if (thFormNo != null)
                    thFormNo.Visible = !bRegNo; // Visible when optAdmission is checked
                
                System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = (System.Web.UI.HtmlControls.HtmlTableCell)oHtmlTableHeaderRow.FindControl("thReg");
                if (oHtmlTableCell != null)
                    oHtmlTableCell.Visible = bRegNo;
                System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell1 = (System.Web.UI.HtmlControls.HtmlTableCell)oHtmlTableHeaderRow.FindControl("thMob");

                if (oHtmlTableCell1 != null)
                    oHtmlTableCell1.Visible = !bRegNo;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                
                System.Web.UI.HtmlControls.HtmlTableCell tdFormNo = e.Item.FindControl("tdFormNo") as System.Web.UI.HtmlControls.HtmlTableCell;
                if (tdFormNo != null)
                {
                    if (bRegNo)
                    {
                        tdFormNo.Visible = false;
                    }
                    else
                    {
                        tdFormNo.Visible = true;
                        DataRowView drv = (DataRowView)e.Item.DataItem;
                        Label lblFormNo = e.Item.FindControl("lblFormNo") as Label;
                        lblFormNo.Text = drv["Form_Number"].ToString();
                    }
                }
                
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDeleteTran") as ImageButton;
                Button obtnComplete = e.Item.FindControl("btnComplete") as Button;
                Button obtnInComplete = e.Item.FindControl("btnInComplete") as Button;
                Button obtnFail = e.Item.FindControl("btnFail") as Button;
                Label Label1 = e.Item.FindControl("Label1") as Label;
                int iTranscationId = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["NetBankingPaymentTransactionID"]);
                int iAdmissionID = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["AdmissionID"]);
                int iAcedemicYearId = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["AcedemicYearId"]);
                int iStudentId = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["StudentId"]);
                int iAmount = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["TransactionAMT"]);
                int OrigionalFeeAmount = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["FeeAmount"]);
                hidAmountInDecimal.Value = String.Format("{0:0.00}", lstvwTransaction.DataKeys[iRowId]["TransactionAMT"].ToDecimal());
                Label1.Text = String.Format("{0:0.00}", lstvwTransaction.DataKeys[iRowId]["TransactionAMT"].ToDecimal());
                int iUserId = Convert.ToInt32(lstvwTransaction.DataKeys[iRowId]["UserId"]);
                string sGatewayId = lstvwTransaction.DataKeys[iRowId]["GatewayId"].ToString();
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                obtnComplete.Attributes.Add("onclick", "if (!ConfirmCompelte('" + iTranscationId.ToString() + "', '" + iAdmissionID.ToString() + "', '" + iAcedemicYearId.ToString() + "', '" + iStudentId.ToString() + "', '" + OrigionalFeeAmount.ToString() + "', '" + iUserId.ToString() + "', '" + sGatewayId + "')) {return false;}");
                obtnInComplete.Attributes.Add("onclick", "if (!ConfirmInCompelte()){return false;}");
                obtnFail.Attributes.Add("onclick", "if(!ConfirmFail()) {return false;}");
                ApplyMouseHoverEffect(new List<Button> { obtnComplete,obtnFail,obtnInComplete });

                System.Web.UI.HtmlControls.HtmlTableRow trData = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trData");
                System.Web.UI.HtmlControls.HtmlTableCell tdbtnComplete = (System.Web.UI.HtmlControls.HtmlTableCell)trData.FindControl("tdbtnComplete");
                System.Web.UI.HtmlControls.HtmlTableCell tdbtnFail = (System.Web.UI.HtmlControls.HtmlTableCell)trData.FindControl("tdbtnFail");
                System.Web.UI.HtmlControls.HtmlTableCell tdbtnInComplete = (System.Web.UI.HtmlControls.HtmlTableCell)trData.FindControl("tdbtnInComplete");
                System.Web.UI.HtmlControls.HtmlTableCell tdDelete = (System.Web.UI.HtmlControls.HtmlTableCell)trData.FindControl("tdDelete");


                if (optFail.Checked)
                {
                    if (tdbtnComplete != null)
                        tdbtnComplete.Visible = false;

                    if (tdbtnFail != null)
                        tdbtnFail.Visible = false;

                    if (tdbtnInComplete != null)
                        tdbtnInComplete.Visible = true;

                    if (tdDelete != null)
                        tdDelete.Visible = false;
                }
                else if (optSuccessful.Checked)
                {
                    if (tdbtnComplete != null)
                        tdbtnComplete.Visible = false;

                    if (tdbtnFail != null)
                        tdbtnFail.Visible = false;

                    if (tdbtnInComplete != null)
                        tdbtnInComplete.Visible = false;

                    if (tdDelete != null)
                        tdDelete.Visible = false;
                }
                else
                {
                    if (tdbtnComplete != null)
                        tdbtnComplete.Visible = true;

                    if (tdbtnFail != null)
                        tdbtnFail.Visible = true;

                    if (tdbtnInComplete != null)
                        tdbtnInComplete.Visible = false;

                    if (tdDelete != null)
                        tdDelete.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used set properties of controls in ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransaction_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTransaction.Items.Count > 0 && btnShow.Text == S_CHANGE_INPUT)
            {
                ControlUtility.FillListViewPagerFooter(lstvwTransaction, DtPgCount);
                SetHeader();
            }
            else
                DtPgCount.Visible = false;
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle delete command raised from the Existing Transactions grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransaction_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    int iTranscationId = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                    Constants.OnlineFeeTypes oOnlineFeeType;
                    if (optRegNo.Checked)
                        oOnlineFeeType = Constants.OnlineFeeTypes.AdmissionFee;
                    else
                        oOnlineFeeType = Constants.OnlineFeeTypes.StudentFee;


                    moNetBankingPaymentTransactionsBL.DeleteTransactionDetails(iTranscationId, miUserId, oOnlineFeeType);
                    SetDataSource();
                   
                }
                else if (e.CommandName == S_COMMAND_FAIL)
                {
                    int iTransactionId = Convert.ToInt32(lstvwTransaction.DataKeys[e.Item.DisplayIndex]["NetBankingPaymentTransactionID"]);                                                                                                                                                                                                                                                                                                                                                                                 
                    NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction
                    {
                        NetBankingPaymentTransactionID = iTransactionId,
                        TransactionStatus = Constants.TransactionStatus.Failed
                    };

                    moNetBankingPaymentTransactionsBL.CompleteTransactionDetails(oNetBankingTransaction, Constants.I_ZERO);
                    SetDataSource();
                }
                else if (e.CommandName == S_COMMAND_COMPLETE)
                {
                    Button btnComplete = oCurrentItem.FindControl("btnComplete") as Button;
                   
                    int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                    hidGatewayId.Value = lstvwTransaction.DataKeys[iRowId]["GatewayId"].ToString();
                    hidBankCode.Value = lstvwTransaction.DataKeys[iRowId]["BankCode"].ToString();
                    var Label1 = oCurrentItem.FindControl("Label1") as Label;
                    if (!Label1.IsNull() && Label1.Text.Trim() != string.Empty)
                        hidAmountInDecimal.Value = String.Format("{0:0.00}", Label1.Text.ToDecimal());
                    spnTransactionNumber.InnerText = hidTranscationId.Value;
                    spnName.InnerText = lstvwTransaction.DataKeys[iRowId]["FullName"].ToString();
                    FillBanks();
                    ScriptManager.RegisterClientScriptBlock(btnComplete, typeof(Page), "ShowPopup", "ShowPopup()", true);
                                 
                  }
                else if (e.CommandName == S_COMMAND_INCOMPLETE)
                {

                    int iTransactionId = Convert.ToInt32(lstvwTransaction.DataKeys[e.Item.DisplayIndex]["NetBankingPaymentTransactionID"]);
                    NetBankingTransaction oNetBankingTransaction = new NetBankingTransaction
                    {
                        NetBankingPaymentTransactionID = iTransactionId
                    };
                       moNetBankingPaymentTransactionsBL.MarkAsInComplete(iTransactionId);
                    
                    
                    SetDataSource();
                }
            }
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used To sort Items in listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransaction_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            HtmlTableRow oHtmlTableHeaderRow = lstvwTransaction.FindControl("trHeader") as HtmlTableRow;
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTransaction);
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This is a common method to complete transaction in both cases.
    /// </summary>
    /// <param name="iTransactionFor"></param>
    /// <param name="oObjectDataSource"></param>
    /// <returns></returns>
    private string CompleteTransaction(string asObjectDataSource, NetBankingTransaction aoNetBankingTransaction, int aiPaymentCategoryFeeId)
    {
        string sMessage = string.Empty;
        sMessage = moNetBankingPaymentTransactionsBL.CompleteTransactionDetails(aoNetBankingTransaction, aiPaymentCategoryFeeId);
        lstvwTransaction.DataSourceID = asObjectDataSource;
        //SetHeader();
        return sMessage;
    }

    private void SetHeader()
    {
        HtmlTableRow trHeader = lstvwTransaction.FindControl("trHeader") as HtmlTableRow;

        if (trHeader != null)
        {
            HtmlTableCell thComplete = lstvwTransaction.FindControl("thComplete") as HtmlTableCell;
            HtmlTableCell thIncomplete = lstvwTransaction.FindControl("thIncomplete") as HtmlTableCell;
            HtmlTableCell thFail = lstvwTransaction.FindControl("thFail") as HtmlTableCell;
            HtmlTableCell thDelete = lstvwTransaction.FindControl("thDelete") as HtmlTableCell;

            if (optFail.Checked)
            {
                if (thComplete != null)
                    thComplete.Visible = false;

                if (thFail != null)
                    thFail.Visible = false;

                if (thIncomplete != null)
                    thIncomplete.Visible = true;

                if (thDelete != null)
                    thDelete.Visible = false;
            }
            else if (optSuccessful.Checked)
            {
                if (thComplete != null)
                    thComplete.Visible = false;

                if (thFail != null)
                    thFail.Visible = false;

                if (thIncomplete != null)
                    thIncomplete.Visible = false;

                if (thDelete != null)
                    thDelete.Visible = false;
            }
            else
            {
                if (thComplete != null)
                    thComplete.Visible = true;

                if (thFail != null)
                    thFail.Visible = true;

                if (thIncomplete != null)
                    thIncomplete.Visible = false;

                if (thDelete != null)
                    thDelete.Visible = true;
            }
        }
    }

    /// <summary>
    /// This method is used to set the object datasource to appropriate condition.
    /// </summary>
    /// <param name="asObjectDataSource"></param>
    private void SetDataSource()
    {
        if (optRegNo.Checked || optCautionMoney.Checked || optInternalFee.Checked || optTransactionDate.Checked)
            lstvwTransaction.DataSourceID = lstDSobj.ID;
        else
            lstvwTransaction.DataSourceID = objAdmission.ID;
        
    }

    private void SetAllControlsForStudentFee()
    {
        lblDateMandatory.Visible = false;
        optAdmission.Checked = false;
        optTransactionDate.Checked = false;
        txtFromDate.Enabled = false;
        txtFromDate.Text = string.Empty;
        txtMobileNumber.Text = string.Empty;
        txtMobileNumber.Enabled = false;
        txtRegNo.Enabled = true;
        lblMobileMandMark.Visible = false;
        lblRegNoMandMark.Visible = true;
        lblErr.Text = "";
        txtRegNo.Focus();
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())

            lblRegNoMandMark.Visible = false;


        else
        {
            lblRegNoMandMark.Visible = true;
        }
        
       
    }

    private void EnabledDisabledControls(bool abFlag)
    {
        trLsttran.Visible = !abFlag;
        optRegNo.Enabled = abFlag;
        optAdmission.Enabled = abFlag;
        optIncomplte.Enabled = abFlag;
        optTransactionDate.Enabled = abFlag;
        optFail.Enabled = abFlag;
        optSuccessful.Enabled = abFlag;
        txtRegNo.Enabled = abFlag;
        txtMobileNumber.Enabled = abFlag;
        txtFromDate.Enabled = abFlag;
        txtFromDate.Enabled = abFlag;
        if (abFlag && optRegNo.Checked)
            SetAllControlsForStudentFee();
        else if (abFlag && optAdmission.Checked)
            SetAllControlsForAddmission();
        else if (abFlag && optTransactionDate.Checked)
            SetAllControlsForTransactionDate();
        lblErr.Text = "";
    }

    private void SetAllControlsForAddmission()
    {
        lblDateMandatory.Visible = false;
        optRegNo.Checked = false;
        optAdmission.Checked = true;
        optTransactionDate.Checked = false;
        txtFromDate.Enabled = false;
        txtFromDate.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtRegNo.Enabled = false;
        txtMobileNumber.Text = string.Empty;
        txtMobileNumber.Enabled = true;
        txtMobileNumber.Focus();
        lblMobileMandMark.Visible = true;
        lblRegNoMandMark.Visible = false;
        lblErr.Text = "";
        btnShow.Focus();
        System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableHeaderRow = (System.Web.UI.HtmlControls.HtmlTableRow)lstvwTransaction.FindControl("trHeader");
        System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = (System.Web.UI.HtmlControls.HtmlTableCell)lstvwTransaction.FindControl("thReg");
        if (oHtmlTableCell != null)
            oHtmlTableCell.Visible = false;

        oHtmlTableCell = (System.Web.UI.HtmlControls.HtmlTableCell)lstvwTransaction.FindControl("thMob");

        if (oHtmlTableCell != null)
            oHtmlTableCell.Visible = true;
    }
    /// <summary>
    /// SetAllControlsForTransactionDate
    /// </summary>
    private void SetAllControlsForTransactionDate()
    {   
        lblDateMandatory.Visible = true;        
        optAdmission.Checked = false;        
        txtMobileNumber.Text = string.Empty;
        txtMobileNumber.Enabled = false;        
        lblMobileMandMark.Visible = false;
        lblRegNoMandMark.Visible = false;
        lblErr.Text = "";
        txtRegNo.Focus();
        btnShow.Focus();       
        
        if(optTransactionDate.Checked)
            txtFromDate.Enabled = true;
        else
            txtFromDate.Enabled = false;
    }

    private void ToggleStatus()
    {
        if (btnShow.Text == S_SHOW)
        {
            EnabledDisabledControls(false);
            SetDataSource();            
            btnShow.Text = S_CHANGE_INPUT;
        }
        else
        {
            btnShow.Text = S_SHOW;
            lstvwTransaction.DataSourceID = null;
            EnabledDisabledControls(true);
        }
        AddSortImage();
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwTransaction.SortDirection.ToString() == "Ascending" || lstvwTransaction.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwTransaction.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwTransaction.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFUALT_SORT_EXPR;
        HtmlTableRow oHtmlTableHeaderRow = lstvwTransaction.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    private void SendSMS()
    {
        int aiAdmissionId = Convert.ToInt32(hidAdmissionId.Value);
        int iAcademicYearID = Convert.ToInt32(hidAcdYrId.Value);
		int iSchoolID = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        int iStudentId = Convert.ToInt32(hidStudentId.Value);
        int iAmount = Convert.ToInt32(hidAmount.Value);
        int iUserId = Convert.ToInt32(hidUserId.Value);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        int iSMSType = 0;
        int iSmsId = Convert.ToInt32((optRegNo.Checked) ? Constants.SMSTemplate.OnlineFeeDetailsSMS : Constants.SMSTemplate.OnlineFeeDetailsSMS);
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, iSchoolID);
        if (oDTSmsTemplate.Rows.Count != 0)
        {
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
        }
        DataTable oDataTable;
        if (optAdmission.Checked)
        {
            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
			oDataTable = oStudentAdmissionsBL.GetStudentAdmissionDetails(aiAdmissionId, Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));
        }
        else
        {
            StudentBL oStudentBL = new StudentBL();
            oDataTable = oStudentBL.RetriveStudentInfo(iSchoolID,  miAcademicYearId, iStudentId);
        }
        if (oDataTable.Rows.Count > 0)
        {
            Hashtable moManualMobileNo = new Hashtable();
            string sMobileNumber = string.Empty;
            string sMobileNumber2 = string.Empty;
            string sForm_Number = string.Empty;
            if (optAdmission.Checked)
            {
                sMobileNumber = Convert.ToString(oDataTable.Rows[0]["MobileNumber"]);
                sForm_Number = Convert.ToString(oDataTable.Rows[0]["Form_Number"]);
                //sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", sForm_Number).Replace("%PASSWORD%", sMobileNumber);
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%Amount%", hidAmountInDecimal.Value + "/-");
            }
            else
            {
                sMobileNumber = Convert.ToString(oDataTable.Rows[0]["Mobile_Number"]);
                sMobileNumber2 = oDataTable.Rows[0]["Mobile_Number2"].ToString();
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%Amount%", iAmount.ToString() + "/-");
            }
			int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
            SchoolBL oSchoolBL = new SchoolBL(iSchoolId);

            string sDisplayText = oDataTable.Rows[0]["Name"].ToString();
            
            SMS oSMS = new SMS();
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = miUserId;
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSType = iSMSType;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.AcademicYearID = iAcademicYearID;
            oSMS.SchoolID = iSchoolID;
            oSMS.DisplayText = sDisplayText;
        
            if (optRegNo.Checked)
            {
                oSMS.To.Add(iUserId, sMobileNumber);
                if (sMobileNumber2 != string.Empty)
                    oSMS.To.Add(iUserId + "sm;", sMobileNumber2);
            }
            else
            {
                moManualMobileNo[sMobileNumber] = sMobileNumber;
                oSMS.ToManualNumbers = moManualMobileNo;            
            }
            oSMS.Send();
        }
    }

    /// <summary>
    /// This Method is used to set default control & active payment URL list for incomplete transaction details.
    /// </summary>
    private void SetDefaultControls()
    {
        btnShow.Focus();    
        Form.DefaultButton = btnShow.UniqueID;
        optCautionMoney.Visible = Settings.EnableOnlinePaymentForCautionMoney;
        optInternalFee.Visible = Settings.EnableOnlinePaymentForInternalFee;
    }
    public void SetUrlToLinkButton()
    {
        //lnkbtnGatewayLinkss.Attributes.Add("onclick", "OpenGatewayPopup(); return false;");
        //lnkbtnGatewayLinkss.Visible = true;
        GetPaymentGatewayURL();
    }

    /// <summary>
    ///   This method is used for to show active payment gateway URLs.
    /// </summary>
    private void GetPaymentGatewayURL()
    {
        DataTable oDT = moNetBankingPaymentTransactionsBL.GetPaymentGatewayURL();
        for (int iRowIndex = 0; iRowIndex < oDT.Rows.Count; iRowIndex++)
        {

            //HyperLink hypLink = new HyperLink();
            divPaymentGatewayLoginURL.Visible = true;
            hlnkGatewayLinks.Text = Convert.ToString(oDT.Rows[iRowIndex]["PaymentGateway"]) + " Website";
            hlnkGatewayLinks.NavigateUrl = Convert.ToString(oDT.Rows[iRowIndex]["PaymentGatewayURL"]);
            hlnkGatewayLinks.Target = "_blank";
            divPaymentGatewayLoginURL.Attributes.Add("style", "text-align:right; padding-right:35px; padding-top:5px; font-size: large");
            //divPaymentGatewayLoginURL.Controls.Add(hypLink);


        }
    }


    private void SetJavascriptAttributes()
    {
        //lnkbtnGatewayLinkss.Attributes.Add("onclick", "OpenGatewayPopup(); return false;");        
    }

    /// <summary>
    /// This method is used to fill all the banks available for selected gateway.
    /// </summary>
    private void FillBanks()
    {
        List<Bank> lstBank = moNetBankingPaymentTransactionsBL.GetBanksForGateway(hidGatewayId.Value.ToInt(), optRegNo.Checked);
        ListSource.FillDropDownList(lstBank, cmbBankName, "Name", "BankCode", Constants.S_SELECT);
        if (lstBank.Count == Constants.I_ONE)
            cmbBankName.SelectedIndex = Constants.I_ONE;

        if (!hidBankCode.Value.IsNullOrEmpty())
            cmbBankName.SelectedValue = hidBankCode.Value;
        txtAmount.Text = hidAmountInDecimal.Value;


    #endregion
    }
    
}
