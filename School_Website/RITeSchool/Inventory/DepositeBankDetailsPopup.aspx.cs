using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities;
using Utility;

public partial class DepositeBankDetailsPopup : SchoolBase
{
    #region Data Mewmber(s)
    
    private DepositeBankDetailsBL moDepositeBankDetailsBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used set listview image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Month";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwPayments, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill years, months and bank details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moDepositeBankDetailsBL = new DepositeBankDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillYears();
                FillMonths();
                SetDefaultValues();
                FillBankDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show record in listview as per selected page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwPayments);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    /// <summary>
    /// This event is used to save deposite bank details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            DepositeBankDetails oDepositeBankDetails = new DepositeBankDetails();
            oDepositeBankDetails.ChequeNo = txtChequeNo.Text.Trim();
            oDepositeBankDetails.Year = cmbYear.SelectedValue.ToInt();
            oDepositeBankDetails.MonthId = cmbMonth.SelectedValue.ToInt();
            oDepositeBankDetails.Date = txtDate.Text.ToDateTime();
            oDepositeBankDetails.Id = hidId.Value.ToInt();
            oDepositeBankDetails.CategoryId = cmbCategory.SelectedValue.ToInt();

            moDepositeBankDetailsBL.Save(oDepositeBankDetails);
            if (hidId.Value == Constants.S_ZERO)
                lblMessage.Text = "Bank details saved successfully !!!";
            else
                lblMessage.Text = "Bank details updated successfully !!!";

            ClearFields();
            FillBankDetails();
        }
    }

    /// <summary>
    /// This event is used to handle edit and delete action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            int iId = lstvwPayments.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
            if (e.CommandName == "RemoveCommand")
            {
                moDepositeBankDetailsBL.Delete(iId);
                lblMessage.Text = "Bank details deleted successfully !!!";
                ClearFields();
                FillBankDetails();
            }
            else if (e.CommandName == "UpdateCommand")
            {
                DepositeBankDetails oDepositeBankDetails = moDepositeBankDetailsBL.Get(iId);
                cmbYear.SelectedValue = oDepositeBankDetails.Year.ToString();
                cmbMonth.SelectedValue = oDepositeBankDetails.MonthId.ToString();
                txtChequeNo.Text = oDepositeBankDetails.ChequeNo;
                txtDate.Text = oDepositeBankDetails.Date.ToString(Constants.S_DATE_FORMAT);
                cmbCategory.SelectedValue = oDepositeBankDetails.CategoryId.ToString();
                hidId.Value = iId.ToString();
                btnSave.Text = Constants.ButtonText.Update.ToString();
            }
        }
    }

    /// <summary>
    /// This event is used to set pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_DataBound(object sender, EventArgs e)
    {
        if (lstvwPayments.Items.Count > Constants.I_ZERO)
            ControlUtility.FillListViewPagerFooter(lstvwPayments, DtPgCount);
        else
            DtPgCount.Visible = false;
    }

    /// <summary>
    /// This event is used to set attribute on controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DepositeBankDetails oDepositeBankDetails = e.Item.DataItem as DepositeBankDetails;
            Label lblPaymentDate = e.Item.FindControl("lblPaymentDate") as Label;
            lblPaymentDate.Text = oDepositeBankDetails.Date.ToString(Constants.S_DATE_FORMAT);

            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        }
    }

    /// <summary>
    /// This event is used to show record in listivew as per filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillBankDetails();
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPayments_Sorting(object sender, ListViewSortEventArgs e)
    {
        if (hidSortExpression.Value != e.SortExpression)
            hidSortDirection.Value = Constants.S_DESCENDING;
        base.RevertSortOrder(hidSortDirection);
        hidSortExpression.Value = e.SortExpression;
    }

    /// <summary>
    /// This event is used to validate month.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Month_Validate(object sender, ServerValidateEventArgs e)
    {
        bool bIsValid = moDepositeBankDetailsBL.ValidateMonth(hidId.Value.ToInt(), cmbYear.SelectedValue.ToInt(), cmbMonth.SelectedValue.ToInt());
        e.IsValid = bIsValid;
    }

    /// <summary>
    /// This event is used to validate cheque No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChequeNo_Validate(object sender, ServerValidateEventArgs e)
    {
        if (txtChequeNo.Text.Trim() != string.Empty)
        {
            bool bIsValid = moDepositeBankDetailsBL.ValidateChequeNo(hidId.Value.ToInt(), txtChequeNo.Text.Trim(), cmbCategory.SelectedValue.ToInt());
            e.IsValid = bIsValid;
        }
        else
            e.IsValid = true;
    }
    
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill years, months and bank details.
    /// </summary>
    private void FillBankDetails()
    {
        lstvwPayments.DataSourceID = objdsPayments.ID;
        lstvwPayments.DataBind();
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbYear.ClearSelection();
        cmbCategory.ClearSelection();
        cmbMonth.ClearSelection();
        txtChequeNo.Text = string.Empty;
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        btnSave.Text = Constants.ButtonText.Save.ToString();
        hidId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbCategory.Attributes.Add("onchange", "SetText(this)");
    }

    /// <summary>
    /// This method is used to fill months.
    /// </summary>
    private void FillMonths()
    {
        SchoolBL oSchoolBL = new SchoolBL();
        List<MonthMaster> lstMonthMaster = oSchoolBL.GetAllMonths();
        ListSource.FillDropDownList(lstMonthMaster, cmbMonth, "Month", "MonthId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill years.
    /// </summary>
    private void FillYears()
    {
        cmbYear.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        for (int k = 2022; k < 2099; k++)
            cmbYear.Items.Add(new ListItem { Text = k.ToString(), Value = k.ToString() });
    } 

    #endregion

}