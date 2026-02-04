// File Name - IncomeDeclarationUC.ascx.cs
// Creator - Sachin
// Created Date - 7-Feb-2013
// Description - This class is used to set income  declarations.

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Linq;

public partial class IncomeDeclarationUC : InvestmentAndIncomeBase
{
    #region Constant(s)

    private const string S_SECTION_NAME = "SectionName";

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up staff group combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.InitializeMemberVariables();
            if (!IsPostBack)
            {
                this.SetDefaultValues();
                this.FillDeclarations();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMethods_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;

            hidSortExpression.Value = e.SortExpression;
            this.RevertSortOrder(hidSortDirection);
            this.FillDeclarations();
            this.AddSortImage(lstvwMethods, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is use to set attributes for controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMethods_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oIncomeDeclaration = e.Item.DataItem as IncomeDeclaration;
                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                txtAmount.Attributes.Add("onchange", "CheckIncomeValue(this)");

                Label lblRowNo = e.Item.FindControl("lblRowNo") as Label;
                lblRowNo.Text = (e.Item.DisplayIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to save income declarations.
    /// </summary>
    public override void Save()
    {
        string sXml = GenerateXml(this.Populate());
        IncomeDeclarationBL oInvestmentDeclarationBL = new IncomeDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
        oInvestmentDeclarationBL.Save(SelectedUserId, sXml, RegimId);
        this.FillDeclarations();
    }

    /// <summary>
    /// This method is used to fill up investment methods n list view.
    /// </summary>
    public override void FillDeclarations()
    {
        base.InitializeMemberVariables();
        string sHasFullAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.InvestmentDeclaration).ToString();        
        IncomeDeclarationBL oIncomeDeclarationBL = new IncomeDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
        if (sHasFullAccess == Constants.S_NO && moUserRole != Constants.UserRoles.Admin)
            SelectedUserId = miUserId;
        List<IncomeDeclaration> lstInvestmentDeclarations = oIncomeDeclarationBL.GetAll(SelectedUserId, SectionId, hidSortExpression.Value, hidSortDirection.Value);

        if (sHasFullAccess == Constants.S_NO && moUserRole != Constants.UserRoles.Admin)
        {
            SectionDetailsBL oSectionDetailsBL = new SectionDetailsBL(miSchoolId, miFinancialYearId, miUserId);
            List<SectionDetails> lstSectionDetails = oSectionDetailsBL.GetAll();
            var oSection = lstSectionDetails.Where(sd => (Constants.SectionGroups)sd.SectionGroupId == Constants.SectionGroups.OtherIncome);
            if(oSection.Count() > 0)
                lstInvestmentDeclarations = lstInvestmentDeclarations.Where(id => id.SectionId == oSection.FirstOrDefault().Id).ToList();
        }

        RecordCount = lstInvestmentDeclarations.Count;

        lstvwMethods.DataSource = lstInvestmentDeclarations;
        lstvwMethods.DataBind();

        trNote.Visible = lstInvestmentDeclarations.Count > 0;

        if (lstInvestmentDeclarations.Count > 0)
            RegimId = lstInvestmentDeclarations[0].RegimId;
    } 

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortExpression.Value = S_SECTION_NAME;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to populate investment declarations.
    /// </summary>
    /// <returns></returns>
    private List<IncomeDeclaration> Populate()
    {
        List<IncomeDeclaration> lstIncomeDeclaration = new List<IncomeDeclaration>();
        foreach (ListViewDataItem oItem in lstvwMethods.Items)
        {
            int iInvestmentDeclarationId = Convert.ToInt32(lstvwMethods.DataKeys[oItem.DisplayIndex]["Id"]);
            int iInvestmentMethodId = Convert.ToInt32(lstvwMethods.DataKeys[oItem.DisplayIndex]["InvestmentMethodId"]);

            TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
            FileUpload flAttachment = oItem.FindControl("flAttachment") as FileUpload;
            HiddenField hidAttachment = oItem.FindControl("hidAttachment") as HiddenField;

            CheckBox chkIsDocSubmitted = oItem.FindControl("chkIsSubmitted") as CheckBox;
            string sIsDeleted = Convert.ToDecimal(txtAmount.Text.Trim()) == 0 ? Constants.S_YES : Constants.S_NO;

            IncomeDeclaration oInvestmentDeclaration = new IncomeDeclaration
            {
                Id = iInvestmentDeclarationId,
                InvestmentMethodId = iInvestmentMethodId,
                Amount = Convert.ToDecimal(txtAmount.Text.Trim()),
                IsDeleted = sIsDeleted
            };
            lstIncomeDeclaration.Add(oInvestmentDeclaration);
        }

        return lstIncomeDeclaration;
    }

    #endregion
}