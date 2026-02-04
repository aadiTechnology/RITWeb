// File Name - InvestmentMethodUI.aspx.cs
// Creator - Sachin
// Created Date - 
// Description - This class is used to configure investment method.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class InvestmentMethodUI : SchoolBase
{
    #region Constants

    private string S_EARNING_DEDUCTION = "EarningDeduction";

    #endregion

    #region Data Member(s)

    InvestmentMethodBL moInvestmentMethodBL;
    SectionDetailsBL moSectionDetailsBL;

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
            AddSortImage(lstvwMethods, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill investment methods in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moInvestmentMethodBL = new InvestmentMethodBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            moSectionDetailsBL = new SectionDetailsBL(miSchoolId, miFinancialYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillSections();
                FillEarnDeductionCombo();
                FillInvestmentMethods();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is  used to save configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ValidateInvestmentMethod();
            PopulateInvestmentMethod(false);
            moInvestmentMethodBL.Update();
            DisplayMessage(hidInvestmentMethodId.Value == Constants.S_ZERO ? Constants.ItemState.saved : Constants.ItemState.updated, false);
            ResetFields();
            FillInvestmentMethods();
            if (lstvwMethods.Items.Count == 0)
                DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.InvestmentMethod));
        }
        catch (DuplicateName dn)
        {
            DisplayMessage(dn.Message, true, tdMessage);
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to payroll dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add attribute on delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMethods_ItemDataBound(object sender, ListViewItemEventArgs e)
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

    /// <summary>
    /// This event is used to update configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMethods_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                int iInvestmentId = Convert.ToInt32(lstvwMethods.DataKeys[e.Item.DisplayIndex]["Id"]);
                string sAssociatedEarnDeductId = lstvwMethods.DataKeys[e.Item.DisplayIndex]["AssociatedEarnDeductId"].ToString();
                hidAssociatedEDId.Value = sAssociatedEarnDeductId;
                List<InvestmentMethod> lstInvestmentMethod = moInvestmentMethodBL.GetAll();
                InvestmentMethod oInvestmentMethod = lstInvestmentMethod.Where(invst => invst.Id == iInvestmentId).FirstOrDefault();
                cmbEarningDeduction.Enabled = true;
                txtMethod.Text = oInvestmentMethod.Name;
                cmbSection.SelectedValue = oInvestmentMethod.SectionId.ToString();
                txtMaxLimit.Text = oInvestmentMethod.MaxLimit.ToString();

                cmbSection_SelectedIndexChanged(cmbSection, null);

                hidInvestmentMethodId.Value = iInvestmentId.ToString();
                BtnSave.Text = Constants.ButtonText.Update.ToString();

                if (cmbEarningDeduction.Items.FindByValue(sAssociatedEarnDeductId.ToString()) != null)
                    cmbEarningDeduction.SelectedValue = sAssociatedEarnDeductId;
                else
                    cmbEarningDeduction.ClearSelection();
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                int iInvestmentId = Convert.ToInt32(lstvwMethods.DataKeys[e.Item.DisplayIndex]["Id"]);
                hidInvestmentMethodId.Value = iInvestmentId.ToString();
                PopulateInvestmentMethod(true);
                moInvestmentMethodBL.Update();
                FillInvestmentMethods();

                if (lstvwMethods.Items.Count == 0)
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.InvestmentMethod));

                if (hidInvestmentMethodId.Value == iInvestmentId.ToString())
                    ResetFields();

                DisplayMessage(Constants.ItemState.deleted, false);
            }
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort expression.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMethods_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;

            RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set earning deduction combo state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<EarningsDeductions> lstEarningsDeductions = new List<EarningsDeductions>();
            if (cmbSection.SelectedValue != Constants.S_ZERO)
            {
                List<SectionDetails> lstSectionDetails = moSectionDetailsBL.GetAll();
                SectionDetails oSectionDetails = lstSectionDetails.FindAll(sd => sd.Id == Convert.ToInt32(cmbSection.SelectedValue)).FirstOrDefault();
                cmbEarningDeduction.ClearSelection();

                if (oSectionDetails.IsExemption)
                {
                    if (ViewState[S_EARNING_DEDUCTION] != null)
                        lstEarningsDeductions = ViewState[S_EARNING_DEDUCTION] as List<EarningsDeductions>;
                }
                else
                {
                    if ((Constants.SectionGroups)oSectionDetails.SectionGroupId == Constants.SectionGroups.GrossSalary)
                        lstEarningsDeductions.Add(new EarningsDeductions { ShortName = "Gross Salary", OriginalEarningsDeductionsId = -1 });
                }
            }
            ListSource.FillDropDownList(lstEarningsDeductions, cmbEarningDeduction, "ShortName", "OriginalEarningsDeductionsId", Constants.S_SELECT);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to display investment methods in list view.
    /// </summary>
    private void FillInvestmentMethods()
    {
        lstvwMethods.DataSourceID = objdsInvestmentMethods.ID;
        lstvwMethods.DataBind();
    }

    /// <summary>
    /// This method is used to display earnings/deductions combo.
    /// </summary>
    private void FillEarnDeductionCombo()
    {
        List<EarningsDeductions> lstEarningsDeductions = EarningsDeductionsBL.GetAll(miSchoolId);
        lstEarningsDeductions = lstEarningsDeductions.Where(ed => ed.SchoolId == miSchoolId).ToList();
        ViewState[S_EARNING_DEDUCTION] = lstEarningsDeductions;

        ListSource.FillDropDownList(null, cmbEarningDeduction, "ShortName", "OriginalEarningsDeductionsId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill section combo box.
    /// </summary>
    private void FillSections()
    {
        List<SectionDetails> lstSectionDetails = moSectionDetailsBL.GetAll();
        ListSource.FillDropDownList(lstSectionDetails, cmbSection, "Name", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { BtnSave, BtnCancel, btnBack });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lnkSectionDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "SectionName";
        cmbSection.Focus();
        cmbEarningDeduction.Attributes.Add("onchange", "SetState()");
        BtnSave.Attributes.Add("onclick", "if(!DisplayConfirmation()) return false;");
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        cmbSection.ClearSelection();
        cmbEarningDeduction.ClearSelection();
        txtMethod.Text = string.Empty;
        hidInvestmentMethodId.Value = Constants.S_ZERO;
        hidAssociatedEDId.Value = Constants.S_ZERO;
        BtnSave.Text = Constants.ButtonText.Save.ToString();
        chkApplyToUsers.Checked = false;
        txtMaxLimit.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to populate object.
    /// </summary>
    private void PopulateInvestmentMethod(bool abIsDeleted)
    {
        moInvestmentMethodBL.InvestmentMethod = new InvestmentMethod
        {
            Id = Convert.ToInt32(hidInvestmentMethodId.Value),
            Name = txtMethod.Text.Trim(),
            SectionId = Convert.ToInt32(cmbSection.SelectedValue),
            Is_Deleted = Convert.ToInt32(abIsDeleted),
            AssociatedEarnDeductId = (cmbEarningDeduction.SelectedValue == string.Empty ? Constants.I_ZERO : cmbEarningDeduction.SelectedValue.ToInt()),
            MaxLimit = Convert.ToInt32(txtMaxLimit.Text.Trim() == string.Empty ? Constants.S_ZERO : txtMaxLimit.Text.Trim()),
            ApplyToAllUsers = chkApplyToUsers.Checked,
            IsReset = hidIsConfirmed.Value
        };
    }

    /// <summary>
    /// This method is used to validate investment methods.
    /// </summary>
    private void ValidateInvestmentMethod()
    {
        List<InvestmentMethod> lstInvestmentMethod = moInvestmentMethodBL.GetAll();
        if (lstInvestmentMethod.FindAll(im => im.Id != Convert.ToInt32(hidInvestmentMethodId.Value) && im.Name.ToUpper() == txtMethod.Text.Trim().ToUpper()).Count > 0)
            throw new DuplicateName("Investment Method already exists.");
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Investment Method " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    #endregion

}