/*File Name - ExternalPOPaymentDetailsUI.aspx.cs
 * Created By - Sachin
 * Created Date - 22-Dec-2023
 * Description - This class is used to manage PO payments.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using SchoolEntities.StudentFee;
using Utility;

public partial class ExternalPOPaymentDetailsUI : SchoolBase
{
    #region Constant(s)
    
    private PODetailsBL moPODetailsBL; 

    #endregion

    #region Event(s)
   
    /// <summary>
    /// This event is used to fill payments, banks, types etc.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPODetailsBL = new PODetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                ReadQueryString();
                SetDefaultValues();
                SetPoDetails();
                FillPaymentModes();
                FillBankCombo();
                FillTypes();
                FillPayments();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                POFeePayment oPOFeePayment = e.Item.DataItem as POFeePayment;

                Label lblPaymentMode = e.Item.FindControl("lblPaymentMode") as Label;
                lblPaymentMode.Text = ((Constants.PaymentMode)oPOFeePayment.PaymentModeId).ToString();

                Label lblPaymentDate = e.Item.FindControl("lblPaymentDate") as Label;
                lblPaymentDate.Text = oPOFeePayment.PaymentDate.ToString(Constants.S_DATE_FORMAT);

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle edit/delete action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwPayments.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                hidId.Value = iId.ToString();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    SetFieldsForUpdation(iId);
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moPODetailsBL.DeletePaymentDetails(hidPOMasterId.Value.ToInt(), iId);
                    lblMessage.Text = "Payment details deleted successfully !!!";
                    ClearFields();
                    FillPayments();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show/hide fields as per selected payment mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optLstModes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (optLstModes.SelectedValue == Constants.PaymentMode.Cheque.ToInt().ToString())
            {
                trChequeDate.Visible = true;
                trChequeNo.Visible = true;
                trTxnNo.Visible = false;
                trType.Visible = false;
            }
            else
            {
                trChequeDate.Visible = false;
                trChequeNo.Visible = false;
                trTxnNo.Visible = true;
                trType.Visible = true;
                FillTypes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields.
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save payment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            POFeePayment oPOFeePayment = Populate();
            moPODetailsBL.SavePayment(oPOFeePayment);

            if (btnSave.Text == Constants.ButtonText.Save.ToString())
                lblMessage.Text = "Payment details saved successfully !!!";
            else
                lblMessage.Text = "Payment details updated successfully !!!";

            ClearFields();
            FillPayments();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill types.
    /// </summary>
    private void FillTypes()
    {
        if (optLstModes.SelectedValue == Constants.PaymentMode.Electronic.ToInt().ToString())
            FillElectronicPaymentTypes();
        else if (optLstModes.SelectedValue == Constants.PaymentMode.Card.ToInt().ToString())
            FillCardTypes();
        else
        {
            cmbTypes.Items.Clear();
            cmbTypes.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        }
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["POMasterId"] != null)
            hidPOMasterId.Value = QueryString["POMasterId"].ToString();
        else
            hidPOMasterId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to fill payment listview.
    /// </summary>
    private void FillPayments()
    {
        List<POFeePayment> lstPOFeePayment = moPODetailsBL.GetAllPayments(hidPOMasterId.Value.ToInt());
        lstvwPayments.DataSource = lstPOFeePayment; ;
        lstvwPayments.DataBind();
    }

    /// <summary>
    /// This method is used to fill payment modes.
    /// </summary>
    private void FillPaymentModes()
    {
        optLstModes.Items.Add(new ListItem { Text = "Cheque", Value = Constants.PaymentMode.Cheque.ToInt().ToString() });
        optLstModes.Items.Add(new ListItem { Text = "Card", Value = Constants.PaymentMode.Card.ToInt().ToString() });
        optLstModes.Items.Add(new ListItem { Text = "Electronic", Value = Constants.PaymentMode.Electronic.ToInt().ToString() });
        
        optLstModes.SelectedValue = Constants.PaymentMode.Cheque.ToInt().ToString();
    }

    /// <summary>
    /// This method will be used to fill all the electronic types into the types dropdownlist.
    /// </summary>
    private void FillElectronicPaymentTypes()
    {
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
        List<ElectronicPaymentType> lstElectronicTypes = moStudentFeeDetailsBL.GetElectronicPaymentTypes();
        ListSource.FillDropDownList(lstElectronicTypes, cmbTypes, "Type", "TypeId", Constants.S_SELECT);
    }

    /// <summary>
    /// Populates the Card type dropdown list.
    /// </summary>
    private void FillCardTypes()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtCardTypeList = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(miSchoolId);
        ControlUtility.FillDropDownList(dtCardTypeList, ref cmbTypes, "CardTypeId", "CardType", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill banks.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref cmbBanks, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to clar fields.
    /// </summary>
    private void ClearFields()
    {   
        txtPaymentDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtChequeDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtPayableAmount.Text = string.Empty;
        txtChequeNumber.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtTxnNumber.Text = string.Empty;

        hidId.Value = Constants.S_ZERO;

        cmbBanks.ClearSelection();
        cmbTypes.ClearSelection();

        optLstModes.SelectedValue = Constants.PaymentMode.Cheque.ToInt().ToString();
        optLstModes_SelectedIndexChanged(optLstModes, null);

        btnSave.Text = Constants.ButtonText.Save.ToString();
        SetPoDetails();
    }

    /// <summary>
    /// This method is used to populate entiry class properties.
    /// </summary>
    /// <returns></returns>
    private POFeePayment Populate()
    {
        POFeePayment oPOFeePayment = new POFeePayment();
        oPOFeePayment.Amount = txtPayableAmount.Text.ToDecimal();
        oPOFeePayment.BankId = cmbBanks.SelectedValue.ToInt();
        oPOFeePayment.PaymentDate = txtPaymentDate.Text.ToDateTime();
        oPOFeePayment.PaymentModeId = optLstModes.SelectedValue.ToInt();

        if (optLstModes.SelectedValue.ToInt() == Constants.PaymentMode.Cheque.ToInt())
        {
            oPOFeePayment.TxnNo = txtChequeNumber.Text;
            oPOFeePayment.ChequeDate = txtChequeDate.Text.ToDateTime();
            oPOFeePayment.TypeId = 0;
        }
        else
        {
            oPOFeePayment.TxnNo = txtTxnNumber.Text;
            oPOFeePayment.ChequeDate = DateTime.Now.Date;
            oPOFeePayment.TypeId = cmbTypes.SelectedValue.ToInt();
        }

        oPOFeePayment.Id = hidId.Value.ToInt();
        oPOFeePayment.POMasterId = hidPOMasterId.Value.ToInt();
        oPOFeePayment.Remark = txtRemark.Text.Trim();

        return oPOFeePayment;
    }

    /// <summary>
    /// This method is used to set values for fields.
    /// </summary>
    /// <param name="aiId"></param>
    private void SetFieldsForUpdation(int aiId)
    {
        POFeePayment oPOFeePayment = moPODetailsBL.GetPaymentDetails(hidPOMasterId.Value.ToInt(), aiId);
        optLstModes.SelectedValue = oPOFeePayment.PaymentModeId.ToString();

        cmbBanks.SelectedValue = oPOFeePayment.BankId.ToString();

        txtPaymentDate.Text = oPOFeePayment.PaymentDate.ToString(Constants.S_DATE_FORMAT);
        txtPayableAmount.Text = oPOFeePayment.Amount.ToString();
        txtRemark.Text = oPOFeePayment.Remark;

        if (oPOFeePayment.PaymentModeId == Constants.PaymentMode.Cheque.ToInt())
        {
            txtChequeNumber.Text = oPOFeePayment.TxnNo;
            txtChequeDate.Text = oPOFeePayment.ChequeDate.ToString(Constants.S_DATE_FORMAT);
            trChequeDate.Visible = true;
            trChequeNo.Visible = true;
            trTxnNo.Visible = false;
            trType.Visible = false;
        }
        else
        {
            txtTxnNumber.Text = oPOFeePayment.TxnNo;

            if (oPOFeePayment.PaymentModeId == Constants.PaymentMode.Card.ToInt())
                FillCardTypes();
            else
                FillElectronicPaymentTypes();

            cmbTypes.SelectedValue = oPOFeePayment.TypeId.ToString();

            trChequeDate.Visible = false;
            trChequeNo.Visible = false;
            trTxnNo.Visible = true;
            trType.Visible = true;
        }

        txtPendingAmount.Text = (txtPendingAmount.Text.ToDecimal() + oPOFeePayment.Amount).ToString();
        btnSave.Text = Constants.ButtonText.Update.ToString();
    }

    /// <summary>
    /// This method is used to PO details.
    /// </summary>
    private void SetPoDetails()
    {
        ExternalOrderPrefix oExternalOrderPrefix = moPODetailsBL.GetPrefixes();
        ExternalPODetails oExternalPODetails = moPODetailsBL.Get(hidPOMasterId.Value.ToInt());
        txtTotalAmount.Text = oExternalPODetails.TotalAmount.ToString();
        lblPONo.Text = (oExternalPODetails.IsPO ? oExternalOrderPrefix.POPrefix : oExternalOrderPrefix.WOPrefix) + oExternalPODetails.PONo;
        spnPONo.InnerText = (oExternalPODetails.IsPO ? "PO NO." : "WO NO.");
        txtPendingAmount.Text = (oExternalPODetails.TotalAmount - oExternalPODetails.TotalPaidAmount).ToString();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        txtPaymentDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtChequeDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = "PODetailsUI.aspx?" + CommonUtility.EncryptQuerystring("StatusId=" + QueryString["StatusId"].ToString() + "&Filter=" + QueryString["Filter"].ToString());
        btnSave.Attributes.Add("onclick", "ResetLabel()");
    } 

    #endregion
    
}