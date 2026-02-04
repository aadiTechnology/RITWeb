using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class PaymentGroupUI : SchoolBase
{
    #region Data Member(s)
    
    PaymentGroupBL moPaymentGroupBL; 

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
            base.AddSortImage(lstvwGroups, "Name", hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up available payment groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPaymentGroupBL = new PaymentGroupBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillPaymentGroups();                
                FillEarningDeductions();
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel current operation.
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
    /// This event is used to save payment group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iPaymentGroupId = Convert.ToInt32(hidPaymentGroupId.Value);
            string sParameterXml = GetParameterXml();
            moPaymentGroupBL.Save(iPaymentGroupId, txtName.Text.Trim(), sParameterXml);

            if (iPaymentGroupId == 0)
                base.DisplayMessage(Resources.LocalizedResources.msgSavePaymentGroup, false, tdMessage);
            else
                base.DisplayMessage(Resources.LocalizedResources.msgUpdatePaymentGroup, false, tdMessage);

            ClearFields();
            FillPaymentGroups();

            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PaymentGroups));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set delete button attribute.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroups_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ShowConfirmation()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update  / delete payment group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroups_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iGroupId = lstvwGroups.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    PaymentGroup oPaymentGroup = moPaymentGroupBL.Get(iGroupId);
                    txtName.Text = oPaymentGroup.Name;
                    lblGrossSalary.Text = Constants.S_ZERO;
                    decimal dcGrossSalary = 0;
                    foreach (ListViewDataItem oItem in lstvwParameters.Items)
                    {
                        int iEarningDeductionId = Convert.ToInt32(lstvwParameters.DataKeys[oItem.DisplayIndex]["EarningsDeductionsId"]);
                        TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
                        var oDetails = oPaymentGroup.EarningDeductionGroups.Where(pgd => pgd.EarningDeductionId == iEarningDeductionId);
                        if (oDetails != null && oDetails.Count() > 0)
                        {
                            HiddenField hidIsEarning = oItem.FindControl("hidIsEarning") as HiddenField;
                            txtAmount.Text = oDetails.FirstOrDefault().Amount.ToString();

                            if (hidIsEarning.Value == "True")
                                dcGrossSalary = dcGrossSalary + Convert.ToDecimal(txtAmount.Text);
                            else
                                dcGrossSalary = dcGrossSalary - Convert.ToDecimal(txtAmount.Text);
                        }
                        else
                            txtAmount.Text = Constants.S_ZERO;
                    }

                    lblGrossSalary.Text = dcGrossSalary.ToString();

                    hidPaymentGroupId.Value = iGroupId.ToString();
                    btnSave.Text = Resources.LocalizedResources.Update;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moPaymentGroupBL.Delete(iGroupId);
                    base.DisplayMessage(Resources.LocalizedResources.msgDeletePaymentGroup, false, tdMessage);
                    FillPaymentGroups();
                    if (hidPaymentGroupId.Value == iGroupId.ToString())
                        ClearFields();

                    if (lstvwGroups.Items.Count == 0)
                        base.DeleteConfigDetails(Constants.SchoolConfigurations.PaymentGroups.ToInt());
                }
            }
        }
        catch (SqlException oSqlException)
        {
            base.DisplayMessage(oSqlException.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroups_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            base.RevertSortOrder(hidSortDirection);
            FillPaymentGroups();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on earning deduction gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameters_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                EarningsDeductions oEarningsDeductions = e.Item.DataItem as EarningsDeductions;
                Label lblName = e.Item.FindControl("lblEDName") as Label;
                lblName.Text = (oEarningsDeductions.IsEarning ? "(+) " : "(-) ") + oEarningsDeductions.EarningsDeductionsName;

                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                txtAmount.Attributes.Add("onchange", "UpdateGrossSalary();");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)
   
    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });        
        hidSortDirection.Value = Constants.S_ASCENDING;
        txtName.Focus();
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        btnSave.Attributes.Add("onclick","ResetMessage()");
    }

    /// <summary>
    /// This method is used to fill up payment group list view.
    /// </summary>
    private void FillPaymentGroups()
    {
        List<PaymentGroup> lstGroups = moPaymentGroupBL.GetAll();
        if (hidSortDirection.Value == Constants.S_ASCENDING || hidSortDirection.Value == string.Empty)
            lstGroups = lstGroups.OrderBy(gp => gp.Name).ToList();
        else
            lstGroups = lstGroups.OrderByDescending(gp => gp.Name).ToList();

        lstvwGroups.DataSource = lstGroups;
        lstvwGroups.DataBind();
    }

    /// <summary>
    /// This method is used to fill up earning deduction list view.
    /// </summary>
    private void FillEarningDeductions()
    {
        List<EarningsDeductions> lstEarningDeductions = EarningsDeductionsBL.GetAll(miSchoolId);
        lstEarningDeductions = lstEarningDeductions.Where(ed => ed.SchoolId == miSchoolId).OrderByDescending(ed => ed.IsEarning).ThenBy(ed => ed.OriginalEarningsDeductionsId).ToList();
        lstvwParameters.DataSource = lstEarningDeductions;
        lstvwParameters.DataBind();        
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtName.Text = string.Empty;
        hidPaymentGroupId.Value = Constants.S_ZERO;
        lblGrossSalary.Text = Constants.S_ZERO;
        foreach (ListViewDataItem oItem in lstvwParameters.Items)
        {
            TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
            txtAmount.Text = string.Empty;
        }
        btnSave.Text = Resources.LocalizedResources.Save;
    }

    /// <summary>
    /// This method is used to return earning deduction xml.
    /// </summary>
    /// <returns></returns>
    private string GetParameterXml()
    {
        List<EarningDeductionGroup> lstParameters = new List<EarningDeductionGroup>();
        foreach (ListViewDataItem oItem in lstvwParameters.Items)
        {
            TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
            if (txtAmount.Text != string.Empty)
            {   
                lstParameters.Add
                    (
                        new EarningDeductionGroup
                        {
                            PaymentGroupId = Convert.ToInt32(hidPaymentGroupId.Value),
                            EarningDeductionId = Convert.ToInt32(lstvwParameters.DataKeys[oItem.DisplayIndex]["EarningsDeductionsId"]),
                            Amount = Convert.ToInt32(txtAmount.Text)
                        }
                    );
            }
        }

        return base.GenerateXml(lstParameters);
    }

    /// <summary>
    /// This method is used to update controls value according to culture.
    /// </summary>
    private void RefreshValues()
    {
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidmsgConfirmDelete.Value = Resources.LocalizedResources.AlertDeleterecord;
    }

    #endregion       
}