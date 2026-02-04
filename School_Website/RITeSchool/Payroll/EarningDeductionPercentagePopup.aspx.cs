/*
 * File Name - EarningDeductionPercentagePopup.aspx.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage payment categories.
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class EarningDeductionPercentagePopup : SchoolBase
{
    #region Data Member(s)

    private PaymentCategoryBL moPaymentCategoryBL;

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
                hidSortExpression.Value = "Name";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwCategory, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill earning deduction and category list views.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPaymentCategoryBL = new PaymentCategoryBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillEarningDeductions();
                FillCategories();
            }
            RefreshValue();
            trMessage.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            FillCategories();
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
    /// This event is used to set attributes for controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEarningsDeductions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oEarningDeductionPercentage = e.Item.DataItem as EarningDeductionPercentage;
                Label lblShortName = e.Item.FindControl("lblShortName") as Label;
                lblShortName.Text = (oEarningDeductionPercentage.EarnDeduct.IsEarning ? "(+) " : "(-) ") + lblShortName.Text;

                TextBox txtPercentage = e.Item.FindControl("txtPercentage") as TextBox;
                txtPercentage.Attributes.Add("onchange", "RoundValue(this,100)");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit and delete operations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCategory_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iCategoryId = Convert.ToInt32(lstvwCategory.DataKeys[e.Item.DisplayIndex]["Id"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    hidCategoryId.Value = iCategoryId.ToString();
                    PaymentCategory oPaymentCategory = moPaymentCategoryBL.Get(iCategoryId);
                    txtName.Text = oPaymentCategory.Name;

                    lstvwEarningsDeductions.DataSource = moPaymentCategoryBL.EarningDeductionPercentages;
                    lstvwEarningsDeductions.DataBind();

                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moPaymentCategoryBL.Delete(iCategoryId);                    
                    base.DisplayMessage(Resources.LocalizedResources.msgCategoryDelete,false,tdMessage);
                    trMessage.Visible = true;
                    FillCategories();

                    if (hidCategoryId.Value == iCategoryId.ToString())
                        ClearFields();
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Message.StartsWith("MSG:"))
            {
                trMessage.Visible = true;
                base.DisplayMessage(ex.Message.Substring(4), true, tdMessage);
            }
            else
                ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to manage sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCategory_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillCategories(hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes for controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
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
    /// This method is used to fill category combo box.
    /// </summary>
    /// <param name="asSortDirection"></param>
    private void FillCategories(string asSortDirection = "")
    {
        List<PaymentCategory> lstCategories = moPaymentCategoryBL.GetAll();

        if (asSortDirection == string.Empty || asSortDirection == Constants.S_ASCENDING)
            lstCategories = lstCategories.OrderBy(ct => ct.Name).ToList();
        else
            lstCategories = lstCategories.OrderByDescending(ct => ct.Name).ToList();

        lstvwCategory.DataSource = lstCategories;
        lstvwCategory.DataBind();
    }

    /// <summary>
    /// This method is used to fill earning deduction list view.
    /// </summary>
    private void FillEarningDeductions()
    {
        List<EarningsDeductions> lstEDs = EarningsDeductionsBL.GetAll(miSchoolId);
        lstEDs = lstEDs.Where(ed => !ed.HasFormula && ed.SchoolId == miSchoolId).OrderByDescending(ed => ed.IsEarning).OrderBy(ed => ed.OriginalEarningsDeductionsId).ToList();

        List<EarningDeductionPercentage> lstPercentages = new List<EarningDeductionPercentage>();
        lstEDs.ForEach
            (
                ed =>
                {
                    lstPercentages.Add
                    (
                        new EarningDeductionPercentage
                        {
                            CategoryId = 0,
                            EarningDeductionId = ed.EarningsDeductionsId,
                            Id = 0,
                            Percentage = (decimal)0,                            
                            EarnDeduct = new EarningsDeductions
                            {
                                IsEarning = ed.IsEarning,
                                ShortName = ed.ShortName
                            }
                        });
                }
            );

        lstvwEarningsDeductions.DataSource = lstPercentages;
        lstvwEarningsDeductions.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose });        
        hidSortExpression.Value = "Name";
        hidSortDirection.Value = Constants.S_ASCENDING;
        btnSave.Attributes.Add("onclick","if(!ShowConfirmationMessage()) return false;");
        txtName.Focus();
    }

    /// <summary>
    /// This method is used to save category.
    /// </summary>
    private void Save()
    {
        int iCategoryID = Convert.ToInt32(hidCategoryId.Value);
        string sName = txtName.Text.Trim();
        string sEarnDeductXml = base.GenerateXml(PopulateEarningDedcutions());
        moPaymentCategoryBL.Save(iCategoryID, sName, sEarnDeductXml, hidUpdateUserData.Value);

        base.DisplayMessage((iCategoryID == 0 ? Resources.LocalizedResources.msgCategorySave : Resources.LocalizedResources.msgCategoryUpdate), false, tdMessage);

        trMessage.Visible = true;
        ClearFields();
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtName.Text = string.Empty;
        hidCategoryId.Value = Constants.S_ZERO;
        FillEarningDeductions();
        btnSave.Text = Constants.ButtonText.Save.ToString();
        hidUpdateUserData.Value = Constants.S_NO;
    }

    /// <summary>
    /// This method is used to populate earning deductions.
    /// </summary>
    /// <returns></returns>
    private List<EarningDeductionPercentage> PopulateEarningDedcutions()
    {
        List<EarningDeductionPercentage> lstEDs = new List<EarningDeductionPercentage>();
        foreach (var oItem in lstvwEarningsDeductions.Items)
        {
            if (oItem.ItemType == ListViewItemType.DataItem)
            {
                int iCategoryId = Convert.ToInt32(lstvwEarningsDeductions.DataKeys[oItem.DisplayIndex]["Id"]);
                int iEarnDeductId = Convert.ToInt32(lstvwEarningsDeductions.DataKeys[oItem.DisplayIndex]["EarningDeductionId"]);
                TextBox txtPercentage = oItem.FindControl("txtPercentage") as TextBox;
                if (txtPercentage.Text != string.Empty && Convert.ToDecimal(txtPercentage.Text) != (decimal)0)
                    lstEDs.Add(new EarningDeductionPercentage { Id = iCategoryId, EarningDeductionId = iEarnDeductId, Percentage = Convert.ToDecimal(txtPercentage.Text) });
            }
        }
        return lstEDs;
    }

    /// <summary>
    /// This method is used to refresh values according to culture.
    /// </summary>
    private void RefreshValue()
    {   
        hidmsgConfirmDelete.Value = Resources.LocalizedResources.AlertDeleterecord;
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hiddupvalCategoryName.Value = Resources.LocalizedResources.valDupCategoryName;
        hidvalPercentage.Value = Resources.LocalizedResources.valPercentage;
        hidUpdateConfirmMsg.Value = Resources.LocalizedResources.msgCategoryUpdateConfirm;
    }

    #endregion
}