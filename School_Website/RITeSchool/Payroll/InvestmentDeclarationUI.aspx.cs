// File Name - InvestmentDeclarationUI.aspx.cs
// Creator - Sachin
// Created Date - 
// Description - This class is used to set investment declarations.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class InvestmentDeclarationUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "%TYPE% Declaration saved successfully!!!"; 

    #endregion

    #region Data Member(s)

    private InvestmentAndIncomeBase moInvestmentAndIncomeBase;
    private bool mbIsPublished;

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
            if (!IsPostBack)
            {
                SetDefaultValues();
                SetDeclarationView();
                FillControls();
            }

            InitUserControl();
			if (!IsPostBack)
				ReadQueryString();
            InitUserFields();            
            SetUserDetails();

            if (!IsPostBack)
                SetButtonState();
            if (QueryString["UserId"] == null)
                btnBack.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill users in user combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers();
            moInvestmentAndIncomeBase.SelectedUserId = 0;
            moInvestmentAndIncomeBase.SectionId = 0;
            SetInvestmentAndIncomeDeclarationView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill investment declarations in list view on change of sections.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetInvestmentAndIncomeDeclarationView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up investment declarations according to selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetInvestmentAndIncomeDeclarationView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display  investment declarations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optInvestment_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillSections();
            SetDeclarationView();
            moInvestmentAndIncomeBase.SectionId = 0;
            SetInvestmentAndIncomeDeclarationView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display income declarations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optIncome_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillSections();
            SetDeclarationView();
            moInvestmentAndIncomeBase.SectionId = 0;
            SetInvestmentAndIncomeDeclarationView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save investment and income declarations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moInvestmentAndIncomeBase.RegimId = ddlRegime.SelectedValue.ToInt();
            moInvestmentAndIncomeBase.Save();
            string sMessage = "Income";
            if (optInvestment.Checked)
                sMessage = "Investment";
            DisplayMessage(S_SAVE_MESSAGE.Replace("%TYPE%", sMessage), false, tdMessage);            
        }
        catch (ApplicationException ae)
        {
            DisplayMessage(ae.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to return back to last page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = this.Master as MasterPage;
            if (QueryString["UserId"] != null)
                oMasterPage.RedirectToNextPage("IncomeTaxDetailsUI.aspx?" + Request.QueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update investment declaration listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void HidItemCount_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            moInvestmentAndIncomeBase.UpdateDocumentCount(hidItemCount.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This function is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["StaffGroupId"].IsNullOrEmpty())
            cmbStaffGroup.SelectedValue = QueryString["StaffGroupId"];
        if (!QueryString["UserId"].IsNullOrEmpty())
        {
            cmbUser.SelectedValue = QueryString["UserId"];
            moInvestmentAndIncomeBase.SelectedUserId = QueryString["UserId"].ToInt();
            cmbUser_SelectedIndexChanged(cmbUser, null);

            IncomeTaxDetailsBL oIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
            if (oIncomeTaxDetailsBL.CheckIsPublished(QueryString["UserId"].ToInt()))
            {
                trPublishMessage.Visible = true;
                cmbStaffGroup.Enabled = false;
                cmbUser.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                trPublishMessage.Visible = false;
                cmbStaffGroup.Enabled = false;
                cmbUser.Enabled = false;
                btnSave.Enabled = true;
            }
        }       
    }

    /// <summary>
    /// This method is used to fill fields.
    /// </summary>
    private void FillControls()
    {
        FillSections();
        FillStaffGroups();
        FillUsers();
        FillRegimeDropdown();
    }

    /// <summary>
    /// This method is used to fill up staff group combo box.
    /// </summary>
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();

        // This is a existing method.
        DataTable dtAllStaffGroups = StaffGroupsBL.GetAll(miSchoolId);
        DataRow[] drStaffGroups = dtAllStaffGroups.Select("SchoolId=" + miSchoolId);
        DataTable dtStaffGroups = dtAllStaffGroups.Clone();
        if (drStaffGroups.Length > 0)
            dtStaffGroups = drStaffGroups.CopyToDataTable();
        ControlUtility.FillDropDownList(dtStaffGroups, ref cmbStaffGroup, "StaffGroupsId", "StaffGroupsName", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to fill up sections.
    /// </summary>
    private void FillSections()
    {
        SectionDetailsBL oSectionDetailsBL = new SectionDetailsBL(miSchoolId, miFinancialYearId, miUserId);
        List<SectionDetails> lstSectionDetails = oSectionDetailsBL.GetAll();
        lstSectionDetails = lstSectionDetails.Where(sd => sd.IsExemption == optInvestment.Checked).ToList();

        if (!HasFullAccess && optIncome.Checked)
            lstSectionDetails = lstSectionDetails.Where(sd => ((Constants.SectionGroups)sd.SectionGroupId) == Constants.SectionGroups.OtherIncome).ToList();

        ListSource.FillDropDownList(lstSectionDetails, cmbSection, "Name", "Id", Constants.S_ALL);        
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        SetFieldState();
        lnkInvestmentMethods.Focus();
        optInvestment.Checked = true;        
        btnSave.Attributes.Add("onclick", "ResetFields()");

        if (moSchool == Constants.SchoolId.PPSN)
        {
            starspan.Visible = true;
            ReqRegime.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    private void SetFieldState()
    {
        hidHasFullAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.InvestmentDeclaration).ToString();
        trInvMethod.Visible = HasFullAccess;
    }

    /// <summary>
    /// This method is used to fill up user combo box.
    /// </summary>
    private void FillUsers()
    {
        TaxDeductionBL oTaxDeductionBL = new TaxDeductionBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        List<UserBasicDetails> lstUserBasicDetails = oTaxDeductionBL.GetPayrollUsers(Convert.ToInt32(cmbStaffGroup.SelectedValue));
         ListSource.FillDropDownList(lstUserBasicDetails, cmbUser, "StaffName", "UserId", Constants.S_SELECT);
        SetViewMode();
    }

    /// <summary>
    /// This method is used to set view mode.
    /// </summary>
    private void SetViewMode()
    {
        if (!HasFullAccess)                             
        {
            ListItem oListItem = cmbUser.Items.FindByValue(miUserId.ToString());
            if (oListItem != null)
                oListItem.Selected = true;
            tdUser.Visible = false;
            tdCmbUser.Visible = false;
            tdStaffGroupCombo.Visible = false;
            tdStaffGroup.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to initialize user control fields.
    /// </summary>
    private void InitUserFields()
    {
        moInvestmentAndIncomeBase.SelectedUserId = Convert.ToInt32(cmbUser.SelectedValue);
        moInvestmentAndIncomeBase.SectionId = Convert.ToInt32(cmbSection.SelectedValue);
    }

    /// <summary>
    /// This method is used to set view.
    /// </summary>
    private void SetDeclarationView()
    {
        trInvestmentDeclarations.Visible = optInvestment.Checked;
        trIncomeDeclarations.Visible = !optInvestment.Checked;
    }

    /// <summary>
    /// This method is used to initialize user control.
    /// </summary>
    private void InitUserControl()
    {
        if (optInvestment.Checked)
            moInvestmentAndIncomeBase = ucInvestmentDeclarations;
        else
            moInvestmentAndIncomeBase = ucIncomeDeclarations;
    }

    /// <summary>
    /// This method is used to set user details.
    /// </summary>
    private void SetUserDetails()
    {
        IncomeTaxDetailsBL oIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        if (oIncomeTaxDetailsBL.CheckIsPublished())
        {
            trPublishMessage.Visible = true;
            btnSave.Enabled = false;
            mbIsPublished = true;
        }
        else
        {
            trPublishMessage.Visible = false;
            btnSave.Enabled = true;
            mbIsPublished = false;
        }

    }

    /// <summary>
    /// This method is used to display investment / income method declarations.
    /// </summary>
    private void SetInvestmentAndIncomeDeclarationView()
    {
        moInvestmentAndIncomeBase.FillDeclarations();

        if (moInvestmentAndIncomeBase.RecordCount > 0)
        {
            trRegim.Visible = true;
            ddlRegime.SelectedValue = moInvestmentAndIncomeBase.RegimId.ToString();
        }
        else
            trRegim.Visible = false;

        SetButtonState();
    }

    /// <summary>
    /// This metod is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        if (!mbIsPublished)
        {
            if (moInvestmentAndIncomeBase.RecordCount == 0)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
    }


    private void FillRegimeDropdown()
    {
        InvestmentDeclarationBL oInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
        List<UserDetails> lstRegime = oInvestmentDeclarationBL.GetRegimeDetails();
        ListSource.FillDropDownList(lstRegime, ddlRegime, "Name", "Id", Constants.S_SELECT);
    }


    #endregion
}