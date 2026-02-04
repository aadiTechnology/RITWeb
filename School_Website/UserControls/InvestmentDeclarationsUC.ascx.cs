// File Name - InvestmentDeclarationsUC.ascx.cs
// Creator - Sachin
// Created Date - 7-Feb-2013
// Description - This class is used to set investment declarations.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class InvestmentDeclarationsUC : InvestmentAndIncomeBase
{
    #region Constant(s)

    private const string S_SECTION_NAME = "SectionName";
    
    #endregion
            
    #region Property(s)

    /// <summary>
    /// returns true if login user is admin or has edit access of screen.
    /// </summary>
    private bool HasFullAccess
    {
        get { return hidHasFullAccess.Value == Constants.S_YES || moUserRole == Constants.UserRoles.Admin; }
    }

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
                this.SetFieldState();
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
                var oInvestmentDeclaration = e.Item.DataItem as InvestmentDeclaration;
                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                txtAmount.Attributes.Add("onchange", "CheckValue(this)");

                Label lblRowNo = e.Item.FindControl("lblRowNo") as Label;
                lblRowNo.Text = (e.Item.DisplayIndex + 1).ToString();

                CheckBox chkIsSubmitted = e.Item.FindControl("chkIsSubmitted") as CheckBox;
                if (!this.HasFullAccess)
                {
                    System.Web.UI.WebControls.Image imgConfirm = e.Item.FindControl("imgConfirm") as System.Web.UI.WebControls.Image;
                    chkIsSubmitted.Visible = false;

                    if (chkIsSubmitted.Checked)
                    {
                        imgConfirm.Visible = true;
                        txtAmount.Enabled = false;                       
                    }
                }
                
                int iInvestmentMethodId = Convert.ToInt32(lstvwMethods.DataKeys[e.Item.DisplayIndex]["InvestmentMethodId"]);
                string sQueryString = "UserId=" + SelectedUserId +
                                      "&DocumentId=" + iInvestmentMethodId +
                                      "&IsSubmitted=" + (chkIsSubmitted.Checked ? Constants.S_YES : Constants.S_NO) +
                                      "&DocumentTypeId=" + Constants.DocumentTypes.InvestmentDocuments.ToInt();
                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                LinkButton oLinkButton = e.Item.FindControl("lnkAttachment") as LinkButton;
                oLinkButton.Attributes.Add("onclick", "if(!OpenPopup('" + sQueryString + "')) return false;");
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
    /// This method is used to update listview for updating attachment count.
    /// </summary>
    /// <param name="sender"></param>
    public override void UpdateDocumentCount(string asValue)
    {
        int iInvestmentMethodId = 0;
        const int I_DOCUMENT_COUNT = 0;
        const int I_INVESTMENT_METHOD_ID = 1;
        const int I_USER_ID = 2;

        string[] sArrayIds = asValue.Split('$');
        if (sArrayIds[I_DOCUMENT_COUNT] != string.Empty && sArrayIds[I_USER_ID] == SelectedUserId.ToString())
        {
            foreach (ListViewDataItem oCurrentItem in lstvwMethods.Items)
            {
                iInvestmentMethodId = Convert.ToInt32(lstvwMethods.DataKeys[oCurrentItem.DisplayIndex]["InvestmentMethodId"]);
                if (iInvestmentMethodId == sArrayIds[I_INVESTMENT_METHOD_ID].ToInt())
                {
                    LinkButton lnkAttachment = oCurrentItem.FindControl("lnkAttachment") as LinkButton;
                    lnkAttachment.Text = sArrayIds[I_DOCUMENT_COUNT];
                }
            }
        }
    }

    /// <summary>
    /// his method is used to save investment details.
    /// </summary>
    public override void Save()
    {
        string sXml = this.GenerateXml(this.Populate());
        InvestmentDeclarationBL oInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
        oInvestmentDeclarationBL.Save(SelectedUserId, sXml, RegimId);
        this.FillDeclarations();
    }

    /// <summary>
    /// This method is used to fill up investment methods n list view.
    /// </summary>
    public override void FillDeclarations()
    {
        base.InitializeMemberVariables();
        InvestmentDeclarationBL amoInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
        if (hidHasFullAccess.Value == Constants.S_NO && moUserRole != Constants.UserRoles.Admin)
            SelectedUserId = miUserId;
        List<InvestmentDeclaration> lstInvestmentDeclarations = amoInvestmentDeclarationBL.GetAll(SelectedUserId, SectionId, hidSortExpression.Value, hidSortDirection.Value);

        RecordCount = lstInvestmentDeclarations.Count;

        lstvwMethods.DataSource = lstInvestmentDeclarations;
        lstvwMethods.DataBind();

        bool bRecordFound = lstInvestmentDeclarations.Count > 0;
        trNote.Visible = bRecordFound;
        trFilefFormat.Visible = bRecordFound;

        if (lstInvestmentDeclarations.Count > 0)
            RegimId = lstInvestmentDeclarations[0].RegimId;
    }

    #endregion
        
    #region Private Method(s)

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    private void SetFieldState()
    {
        hidHasFullAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.InvestmentDeclaration).ToString();
        trViewAccess.Visible = !this.HasFullAccess;
        trFullAccess.Visible = this.HasFullAccess;
    }

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
    private List<InvestmentDeclaration> Populate()
    {
        List<InvestmentDeclaration> lstInvestmentDeclaration = new List<InvestmentDeclaration>();
        foreach (ListViewDataItem oItem in lstvwMethods.Items)
        {
            int iInvestmentDeclarationId = Convert.ToInt32(lstvwMethods.DataKeys[oItem.DisplayIndex]["Id"]);
            int iInvestmentMethodId = Convert.ToInt32(lstvwMethods.DataKeys[oItem.DisplayIndex]["InvestmentMethodId"]);
            TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
            CheckBox chkIsDocSubmitted = oItem.FindControl("chkIsSubmitted") as CheckBox;
            string sIsDeleted = Convert.ToDecimal(txtAmount.Text.Trim()) == 0 ? Constants.S_YES : Constants.S_NO;

            InvestmentDeclaration oInvestmentDeclaration = new InvestmentDeclaration
            {
                Id = iInvestmentDeclarationId,
                InvestmentMethodId = iInvestmentMethodId,
                Amount = Convert.ToDecimal(txtAmount.Text.Trim()),                
                IsDocSubmitted = chkIsDocSubmitted.Checked,
                IsDeleted = sIsDeleted
            };
            lstInvestmentDeclaration.Add(oInvestmentDeclaration);
        }

        return lstInvestmentDeclaration;
    }

    #endregion   
}