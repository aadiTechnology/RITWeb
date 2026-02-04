using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Globalization;

public partial class AdditionalPaymentsUI : SchoolBase
{
    #region Data Member(s)

    AdditionalPaymentBL moAdditionalPaymentBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "PaymentDate";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }
            
            AddSortImage(lstvwPayments, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moAdditionalPaymentBL = new AdditionalPaymentBL(miSchoolId, miFinancialYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttrinutes();
                FillParameterCombo();
                FillStaffGroupCombo();
                FillPaymentDetails();
                FillBankCombo();
            }
            RefreshValue();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStaffNameCombo();
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwPayments);
            DataPager oDataPager = lstvwPayments.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
            {
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                if (ddlCnt != null)
                    hidPageNo.Value = ddlCnt.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AdditionalPaymentDetails oAdditionalPaymentDetails = new AdditionalPaymentDetails
            {
                UserId = cmbStaffName.SelectedValue.ToInt(),
                PaymentDate = Convert.ToDateTime(txtPaymentDate.Text),
                ParameterId = cmbParameter.SelectedValue.ToInt(),
                Amount = Convert.ToInt64(txtAmount.Text),
                Id = hidPaymentId.Value.ToInt(),                
                BankDetailsId = Convert.ToInt32(cmbAccountNo.SelectedValue)
            };

            moAdditionalPaymentBL.Save(base.GenerateXml(oAdditionalPaymentDetails));

            if (btnSave.Text == Resources.LocalizedResources.Save)
                lblMessage.Text = Resources.LocalizedResources.msgPaymentDetailsSaved;
            else
                lblMessage.Text = Resources.LocalizedResources.msgPaymentDetailsUpdated;

            ClearFields();
            FillPaymentDetails();
            RefreshValue();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwPayments_DataBound(object sender, EventArgs e)
    {
        if (lstvwPayments.Items.Count > Constants.I_ZERO)
            ControlUtility.FillListViewPagerFooter(lstvwPayments, DtPgCount);
        else
            DtPgCount.Visible = false;
    }

    protected void lstvwPayments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                AdditionalPaymentDetails oAdditionalPaymentDetails = oCurrentItem.DataItem as AdditionalPaymentDetails;
                Label lblPaymentDate = oCurrentItem.FindControl("lblPaymentDate") as Label;
                lblPaymentDate.Text = oAdditionalPaymentDetails.PaymentDate.ToString(Constants.S_DATE_FORMAT);


                ImageButton btnDelete = oCurrentItem.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillPaymentDetails();
            RefreshValue();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwPayments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iPaymentId = Convert.ToInt32(lstvwPayments.DataKeys[e.Item.DisplayIndex]["Id"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                    FillControls(iPaymentId);
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moAdditionalPaymentBL.Delete(iPaymentId);
                    lblMessage.Text = Resources.LocalizedResources.msgPaymentDetailsDeleted;
                    FillPaymentDetails();
                    ClearFields();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

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

    protected void lstvwPayments_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillBankAccountCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void FillStaffGroupCombo()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> lstStaffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(lstStaffGroups, cmbStaffGroup, "StaffGroupsName", "StaffGroupsId", Constants.S_SELECT);
    }

    private void FillParameterCombo()
    {
        PaymentParameterBL oPaymentParameterBL = new PaymentParameterBL(miSchoolId, miUserId);
        List<PaymentParameter> lstParameters = oPaymentParameterBL.GetAll(0);
        lstParameters = lstParameters.OrderBy(param => param.Parameter).ToList();
        ListSource.FillDropDownList(lstParameters, cmbParameter, "Parameter", "Id", Constants.S_SELECT);
    }

    private void FillStaffNameCombo()
    {
        TaxDeductionBL oTaxDeductionBL = new TaxDeductionBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        List<UserBasicDetails> lstUserBasicDetails = oTaxDeductionBL.GetPayrollUsers(Convert.ToInt32(cmbStaffGroup.SelectedValue));
        ListSource.FillDropDownList(lstUserBasicDetails, cmbStaffName, "StaffName", "UserId", Constants.S_SELECT);
    }

    private void SetJavascriptAttrinutes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnSearch});
        cmbStaffName.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        base.SetDefaultButton(btnSearch);
        lnkPaymentParameter.Attributes.Add("onclick", "OpenPopup(); return false;");
        hidSortDirection.Value = Constants.S_ASCENDING;
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidServerDate.Value = DateTime.Today.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        btnSave.Attributes.Add("onclick","ClearMessage();");
        txtPaymentDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
    }

    private void FillPaymentDetails()
    {
        lstvwPayments.DataSourceID = objdsPayments.ID;
        lstvwPayments.DataBind();
    }

    private void ClearFields()
    {
        txtAmount.Text = string.Empty;
        txtPaymentDate.Text = string.Empty;
        cmbParameter.ClearSelection();
        cmbStaffGroup.ClearSelection();
        cmbStaffName.ClearSelection();
        cmbBank.ClearSelection();
        cmbAccountNo.ClearSelection();
        hidPaymentId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
        txtPaymentDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
    }

    private void FillControls(int aiPaymentId)
    {
        AdditionalPaymentDetails oAdditionalPaymentDetails = moAdditionalPaymentBL.Get(aiPaymentId);
        txtAmount.Text = oAdditionalPaymentDetails.Amount.ToString();
        txtPaymentDate.Text = oAdditionalPaymentDetails.PaymentDate.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        hidPaymentId.Value = oAdditionalPaymentDetails.Id.ToString();
        cmbParameter.SelectedValue = oAdditionalPaymentDetails.ParameterId.ToString();
        cmbStaffGroup.SelectedValue = oAdditionalPaymentDetails.StaffGroupId.ToString();
        cmbStaffGroup_SelectedIndexChanged(null, null);
        cmbStaffName.SelectedValue = oAdditionalPaymentDetails.UserId.ToString();
        cmbBank.SelectedValue = oAdditionalPaymentDetails.BankId.ToString();
        cmbBank_SelectedIndexChanged(null,null);
        cmbAccountNo.SelectedValue = oAdditionalPaymentDetails.BankDetailsId.ToString();
        btnSave.Text = Resources.LocalizedResources.Update;
    }

    private void RefreshValue()
    {
        lblSearch.Text = Resources.LocalizedResources.UserName + " / " + Resources.LocalizedResources.PaymentParameter;

        var obj = lstvwPayments.FindControl("trHeader");
        if (obj != null)
        {
            LinkButton lnkUserName = obj.FindControl("lnkUserName") as LinkButton;
            lnkUserName.Text = Resources.LocalizedResources.UserName + " (" + Resources.LocalizedResources.Designation + ")";
        }

        hidmsgConfirmDelete.Value = Resources.LocalizedResources.AlertDeleterecord;
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidvalnonZeroAmount.Value = Resources.LocalizedResources.valnonZeroAmount;
        hidvalBlankPaymentDate.Value = Resources.LocalizedResources.valBlankPaymentDate;
        hidvalFuturePaymentDate.Value = Resources.LocalizedResources.valFuturePaymentDate;
    }

    /// <summary>
    /// This method is used to fill bank combobox.
    /// </summary>
    private void FillBankCombo()
    {
        SchoolwiseBankAccountDetailsBL oSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();
        List<SchoolWiseBankAccountDetails> lstSchoolWiseBankAccountDetails = oSchoolwiseBankAccountDetailsBL.GetSchoolwiseBankList(miSchoolId);
        ListSource.FillDropDownList(lstSchoolWiseBankAccountDetails, cmbBank, "BankName", "BankId", Constants.S_SELECT);
        cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
    }
    /// <summary>
    /// This method is used to fill bank account combobox.
    /// </summary>
    private void FillBankAccountCombo()
    {
        int iBankId = Convert.ToInt32(cmbBank.SelectedValue);
        if (iBankId != 0)
        {
            SchoolwiseBankAccountDetailsBL oSchoolwiseBankAccountDetailsBL = new SchoolwiseBankAccountDetailsBL();
            List<SchoolWiseBankAccountDetails> lstSchoolWiseBankAccountDetails = oSchoolwiseBankAccountDetailsBL.GetBankwiseAccountList(miSchoolId, iBankId);
            cmbAccountNo.Items.Clear();
            cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            lstSchoolWiseBankAccountDetails.ForEach(account => cmbAccountNo.Items.Add(new ListItem(Convert.ToString(account.AccountNo), Convert.ToString(account.SchoolWiseBankAccountDetailsId))));
        }
        else
        {
            cmbAccountNo.Items.Clear();
            cmbAccountNo.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));
        }
    }

    #endregion    
}