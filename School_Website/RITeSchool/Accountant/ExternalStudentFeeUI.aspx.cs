// File Name - ExternalStudentInternalFeeUI.aspx.cs
// Creator - Sachin Wagh
// Created Date - 03-13-2018
// Description - This class is used to configure External Student Internal Fee.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using SchoolEntities.StudentFee;
using Utility;
using System.Data;
using System.Xml;

public partial class ExternalStudentFeeUI : SchoolBase
{
    #region Constants

    private const string S_SORT_ROW = "SortRow";

    #endregion

    #region Data Member(s)

    private ExternalStudentFeeBL moExternalStudentFeeBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// Thos event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty || hidSortDirection.Value == string.Empty)
            {
                hidSortExpression.Value = "PaymentDate";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            base.AddSortImage(lstvwExternalStudentFee, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill External Student Fee details in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moExternalStudentFeeBL = new ExternalStudentFeeBL(miSchoolId,miAcademicYearId,miUserId);
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();                
                FillFeeType();
                FillElectronicPaymentTypes();
                FillBankCombo();
                FillStudentsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            ExternalStudentFee oExternalStudentFee = Populate();
            moExternalStudentFeeBL.Save(oExternalStudentFee);

            if (btnPay.Text == Constants.ButtonText.Update.ToString())
                DisplayMessage(Constants.ItemState.updated, false);
            else
                DisplayMessage(Constants.ItemState.saved, false);                
            
            ResetFields();            
            FillStudentsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to controls in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExternalStudentFee_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ExternalStudentFee oExternalStudentFee = e.Item.DataItem as ExternalStudentFee;
                Label lblDate = e.Item.FindControl("lblDate") as Label;
                if (oExternalStudentFee.Date.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblDate.Text = oExternalStudentFee.Date.ToString(Constants.S_DATE_FORMAT);

                int iExternalStudentFeeId = Convert.ToInt32(lstvwExternalStudentFee.DataKeys[e.Item.DisplayIndex]["Id"]);
                int iReceiptNumber = Convert.ToInt32(lstvwExternalStudentFee.DataKeys[e.Item.DisplayIndex]["ReceiptNumber"]);
                int iAcountHeaderId = Convert.ToInt32(lstvwExternalStudentFee.DataKeys[e.Item.DisplayIndex]["AccountHeaderId"]);

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                
                HyperLink hlnkReceipt = e.Item.FindControl("hlnkReceipt") as HyperLink;
                string sRecieptQueryString = String.Format("ExternalStudentFeeId={0}&AcademicYear={1}&ReceiptNo={2}&AccountHeaderId={3}", iExternalStudentFeeId, miAcademicYearId, iReceiptNumber, iAcountHeaderId);
                hlnkReceipt.Attributes.Add("onclick", "if(!OpenRecieptPopup( '" + CommonUtility.EncryptQuerystring(sRecieptQueryString) + "' )) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound data for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExternalStudentFee_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwExternalStudentFee.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwExternalStudentFee, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used for listview command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExternalStudentFee_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iExternalStudentFeeId = Convert.ToInt32(lstvwExternalStudentFee.DataKeys[e.Item.DisplayIndex]["Id"]);

                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    ExternalStudentFee oExternalStudentFee = moExternalStudentFeeBL.Get(iExternalStudentFeeId);
                    if (oExternalStudentFee != null)
                    {
                        hidExternalStudentFeeId.Value = oExternalStudentFee.Id.ToString();
                        txtCalDt.Text = oExternalStudentFee.Date.ToString(Constants.S_DATE_FORMAT);
                        txtStudentName.Text = oExternalStudentFee.StudentName;
                        cmbFeeType.SelectedValue = oExternalStudentFee.FeeId.ToString();
                        txtAmount.Text = oExternalStudentFee.Amount.ToString();
                        txtMobileNo.Text = oExternalStudentFee.MobileNo.ToString();
                        if (oExternalStudentFee.PaymentModeId == Constants.I_ONE)
                        {
                            optCash.Checked = true;
                            optCheque.Checked = false;
                            optElectronic.Checked = false;
                        }
                        else if (oExternalStudentFee.PaymentModeId == Constants.I_TWO)
                        {
                            optCheque.Checked = true;
                            optCash.Checked = false;
                            optElectronic.Checked = false;
                            txtChequeNo.Text = oExternalStudentFee.ChequeNo.ToString();
                            cmbBankName.SelectedValue = oExternalStudentFee.BankId.ToString();
                            txtChequeDt.Text = oExternalStudentFee.ChequeDate != DateTime.MinValue ? oExternalStudentFee.ChequeDate.ToString(Constants.S_DATE_FORMAT): string.Empty;
                        }
                        else if (oExternalStudentFee.PaymentModeId == Constants.I_THREE)
                        {
                            optElectronic.Checked = true;
                            optCash.Checked = false;
                            optCheque.Checked = false;

                          if (!string.IsNullOrEmpty(oExternalStudentFee.ElectronicDetails))
                               {
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(oExternalStudentFee.ElectronicDetails);

                                    XmlNode node = xmlDoc.SelectSingleNode("/ElectronicDetails");
                                    if (node != null)
                                    {
                                        cmbBankName.SelectedValue = oExternalStudentFee.BankId.ToString();
                                        string typeId = node.SelectSingleNode("TypeId").InnerText;
                                        string transactionNo = node.SelectSingleNode("TransactionNo").InnerText;
                                         
                                        if (!string.IsNullOrEmpty(typeId))
                                            cmbElectronicTypes.SelectedValue = typeId;

                                        if (!string.IsNullOrEmpty(transactionNo))
                                            txtChequeNo.Text = transactionNo;
                                    }
                               }
                          }
                      }
                    btnPay.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moExternalStudentFeeBL.Delete(iExternalStudentFeeId);
                    DisplayMessage(Constants.ItemState.deleted, false);                    
                    ResetFields();
                    FillStudentsListView();
                }                
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillStudentsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwExternalStudentFee);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Search student name or Mobile number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate fields.
    /// </summary>
    /// <returns></returns>
    private ExternalStudentFee Populate()
    {
        ExternalStudentFee oExternalStudentFee = new ExternalStudentFee();

        oExternalStudentFee.Id = hidExternalStudentFeeId.Value.ToInt();
        oExternalStudentFee.Date = Convert.ToDateTime(txtCalDt.Text.Trim());
        oExternalStudentFee.StudentName = txtStudentName.Text.Trim();
        oExternalStudentFee.FeeId = cmbFeeType.SelectedValue.ToInt();
        oExternalStudentFee.Amount = Convert.ToInt32(txtAmount.Text.Trim());
        oExternalStudentFee.MobileNo = txtMobileNo.Text.Trim();

        if (optCash.Checked)
        {
            oExternalStudentFee.PaymentModeId = Constants.I_ONE;
            oExternalStudentFee.ChequeNo = Constants.I_ZERO;
            oExternalStudentFee.ChequeDate = DateTime.Now;
            oExternalStudentFee.BankId = Constants.I_ZERO;
        }
        else if (optCheque.Checked)
        {
            oExternalStudentFee.PaymentModeId = Constants.I_TWO;
            oExternalStudentFee.ChequeNo = Convert.ToInt32(txtChequeNo.Text);
            oExternalStudentFee.ChequeDate = Convert.ToDateTime(txtChequeDt.Text);
            oExternalStudentFee.BankId = Convert.ToInt32(cmbBankName.SelectedValue);
        }
        else if (optElectronic.Checked)
        {
            oExternalStudentFee.PaymentModeId = Constants.I_THREE;
            oExternalStudentFee.BankId = cmbBankName.SelectedValue.ToInt();
            oExternalStudentFee.TransactionNo = txtChequeNo.Text.Trim();
            oExternalStudentFee.TypeId = cmbElectronicTypes.SelectedValue.ToInt();
            oExternalStudentFee.ChequeNo = Constants.I_ZERO;
            oExternalStudentFee.ChequeDate = DateTime.Now;
        }
        return oExternalStudentFee;
    }
   
    /// <summary>
    /// This method is used to set javascriot attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        CalDtPopup.Focus();
        ApplyMouseHoverEffect(new List<Button> { btnClear, btnPay});
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        optCash.Checked = true;
        txtCalDt.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);

        cmbFeeType.Attributes.Add("onchange", "SetAmmount()");
        optCheque.Attributes.Add("onclick", "ShowChequeControls()");
        optCash.Attributes.Add("onclick", "ShowChequeControls()");
        optElectronic.Attributes.Add("onclick", "ShowChequeControls()");
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        hidExternalStudentFeeId.Value = Constants.S_ZERO;
        txtAmount.Text = string.Empty;
        txtCalDt.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtChequeDt.Text = string.Empty;
        txtChequeNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtStudentName.Text = string.Empty;
        txtSearch.Text = string.Empty;
        cmbBankName.ClearSelection();
        cmbFeeType.ClearSelection();
        cmbElectronicTypes.ClearSelection();
        CalDtPopup.Focus();
        optCash.Checked = true;
        optCheque.Checked = false;
        optElectronic.Checked = false;
        btnPay.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to display message. 
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "External Student Internal Fee " + aoItemState.ToString() + " successfully !!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This Method Is Used to Fill Fee Type Combobox.
    /// </summary>
    private void FillFeeType()
    {
        string sValue = string.Empty;
        DataTable dtBankList = moExternalStudentFeeBL.GetExternalFeeTypesForCombo();
        cmbFeeType.Bind(dtBankList, "ExternalFeeId", "FeeType", Constants.S_SELECT);
        int iExternalFeeTypeId = Constants.I_ZERO;
        int iAmount = Constants.I_ZERO;
        string sString = string.Empty;

        for (int iValue = 0; iValue < dtBankList.Rows.Count; iValue++)
        {
            iExternalFeeTypeId = Convert.ToInt32(dtBankList.Rows[iValue]["ExternalFeeId"]);
            iAmount = Convert.ToInt32(dtBankList.Rows[iValue]["Amount"]);

            sString = sString + "," + iExternalFeeTypeId + "$" + iAmount;            
        }

        if (sString.StartsWith(",") && sString != string.Empty)        
            hidFeeDetails.Value = sString.Substring(1);                   

    }

    /// <summary>
    /// This method is used to fill Bank Combobox.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        cmbBankName.Bind(dtBankList, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This event is used for fill external students listview.
    /// </summary>
    private void FillStudentsListView()
    {
        lstvwExternalStudentFee.DataSourceID = lstvwDSobj.ID;        
    }


    /// <summary>
    /// This method will be used to fill all the electronic types into the types dropdownlist.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, 0, 0, 0);
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbElectronicTypes, "Type", "TypeId", Constants.S_SELECT);
    }

    #endregion    
}