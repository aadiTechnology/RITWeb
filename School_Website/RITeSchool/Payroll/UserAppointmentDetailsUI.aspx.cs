using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using MasterEntities;
using PayrollEntities;
using Utility;
using System.Web.Services;

public partial class UserAppointmentDetailsUI : SchoolBase
{
    #region Data Member(s)
    
    UserAppointmentDetailsBL moUserAppointmentDetailsBL; 

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
            base.AddSortImage(lstvwAppointentDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up available appointment details in list view, fill up job type, designation and payment group combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserAppointmentDetailsBL = new UserAppointmentDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillDesignations();
                FillPaymentGroups();
                SetDefaultValues();
                FillEarningDeductions(0);
                FillJobTypes();                
                FillSalutation();
                FillAppointmentDetails();
                RefreshValue();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAppointentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAppointentDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwAppointentDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwAppointentDetails);           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes for delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAppointentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ShowConfirmation()) return false;");

                int iAppointmentId = Convert.ToInt32(lstvwAppointentDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
                
                ImageButton btnAppointmentLetter = e.Item.FindControl("btnAppointmentLetter") as ImageButton;                
                btnAppointmentLetter.Attributes.Add("onclick", "OpenPopup('" + "AppointmentId=" + iAppointmentId+"&ReportNo="+Constants.ExportReports.AppointmentLetter.ToInt() + "'); return false;");

                ImageButton btnServiceContract = e.Item.FindControl("btnServiceContract") as ImageButton;                
                btnServiceContract.Attributes.Add("onclick", "OpenPopup('" + "AppointmentId=" + iAppointmentId + "&ReportNo=" + Constants.ExportReports.ServiceContract.ToInt() + "'); return false;");

                Label lblJoiningDate = e.Item.FindControl("lblJoiningdate") as Label;
                lblJoiningDate.Text = Convert.ToDateTime(lblJoiningDate.Text).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));

                Label lblAgreementDate = e.Item.FindControl("lblAgreementDate") as Label;
                lblAgreementDate.Text = Convert.ToDateTime(lblAgreementDate.Text).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit / delete selected record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAppointentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iAppointmentId = Convert.ToInt32(lstvwAppointentDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    UpdateFields(iAppointmentId);
                    btnSave.Text = Resources.LocalizedResources.Update;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moUserAppointmentDetailsBL.Delete(iAppointmentId);
                    base.DisplayMessage(Resources.LocalizedResources.msgAppointmentDeleted, false, tdMessage);

                    if (hidAppointmentId.Value == iAppointmentId.ToString())
                        ClearFields();

                    FillAppointmentDetails();
                }                
            }
        }
        catch (ThreadAbortException)
        {
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
    protected void lstvwAppointentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            base.RevertSortOrder(hidSortDirection);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save appointment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moUserAppointmentDetailsBL.Save(Populate());
            if (hidAppointmentId.Value == Constants.S_ZERO)
                base.DisplayMessage(Resources.LocalizedResources.msgAppointmentSaved, false, tdMessage);
            else
                base.DisplayMessage(Resources.LocalizedResources.msgAppointmentUpdated, false, tdMessage);
            FillAppointmentDetails();
            ClearFields();
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
    /// This event is used to fill up earning deduction list view according to selected payment group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPaymentGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillEarningDeductions(Convert.ToInt32(cmbPaymentGroup.SelectedValue));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to attributes for amount textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameters_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool bIsEarning = Convert.ToBoolean(lstvwParameters.DataKeys[e.Item.DisplayIndex]["IsEarning"]);
                Label lblEDName = e.Item.FindControl("lblEDName") as Label;
                lblEDName.Text = (bIsEarning ? "(+) " : "(-) ") + lblEDName.Text;

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
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnShow });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortExpression.Value = "Name";
        hidSortDirection.Value = Constants.S_ASCENDING;
        cmbPaymentGroup.Attributes.Add("onchange", "if(!ConfirmChange()) return false;");
        btnSave.Attributes.Add("onclick", "ResetMessage()");

        txtAgreementDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtJoiningDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtPaymentStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));

        cmbSalutation.Focus();
    }

    /// <summary>
    /// This method is used to fill up earning deduction list view.
    /// </summary>
    /// <param name="aiPaymentGroupId"></param>
    private void FillEarningDeductions(int aiPaymentGroupId)
    {
        List<EarningDeductionGroup> lstEarningDeductionGroups = new List<EarningDeductionGroup>();
        if (aiPaymentGroupId != 0)
        {
            PaymentGroupBL oPaymentGroupBL = new PaymentGroupBL(miSchoolId, miUserId);
            PaymentGroup oPaymentGroup = oPaymentGroupBL.Get(aiPaymentGroupId);
            lstEarningDeductionGroups = oPaymentGroup.EarningDeductionGroups;
        }
        else
        {
            List<EarningsDeductions> lstEarningDeductions = EarningsDeductionsBL.GetAll(miSchoolId);
            lstEarningDeductions = lstEarningDeductions.Where(ed => ed.SchoolId == miSchoolId).OrderByDescending(ed => ed.IsEarning).ThenBy(ed => ed.OriginalEarningsDeductionsId).ToList();            
            lstEarningDeductions.ForEach
                (
                    ed =>
                    {
                        lstEarningDeductionGroups.Add
                            (
                                new EarningDeductionGroup
                                {
                                    EarningDeductionId = ed.EarningsDeductionsId,
                                    ShortName = ed.ShortName,
                                    Amount = 0,
                                    PaymentGroupId = aiPaymentGroupId,
                                    IsEarning = ed.IsEarning
                                }

                            );
                    }

                );
        }
        
        lstvwParameters.DataSource = lstEarningDeductionGroups;
        lstvwParameters.DataBind();

        var dcEarnings = lstEarningDeductionGroups.Where(ed => ed.IsEarning).GroupBy(pg => pg.PaymentGroupId).Select(pg => new { Key = pg.Key, Amount = pg.Sum(pgd => pgd.Amount) }).FirstOrDefault();
        var dcDeductions = lstEarningDeductionGroups.Where(ed => !ed.IsEarning).GroupBy(pg => pg.PaymentGroupId).Select(pg => new { Key = pg.Key, Amount = pg.Sum(pgd => pgd.Amount) }).FirstOrDefault();
        lblGrossSalary.Text = ((dcEarnings == null ? (decimal)0 : dcEarnings.Amount) - (dcDeductions == null ? (decimal)0 : dcDeductions.Amount)).ToString();
    }

    /// <summary>
    /// This method is used to fill up appointment list view.
    /// </summary>
    private void FillAppointmentDetails()
    {
        lstvwAppointentDetails.DataSourceID = objdsAppointments.ID;
        lstvwAppointentDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set control values according to culture.
    /// </summary>
    private void RefreshValue()
    {
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidmsgConfirmDelete.Value = Resources.LocalizedResources.AlertDeleterecord;
        hidValBlankAddress.Value = Resources.LocalizedResources.AddressBlank;
        hidValAddressLength.Value = Resources.LocalizedResources.LengthOfAddress;
        hidValBlankJoiningDate.Value = Resources.LocalizedResources.ValJoiningDateNull;
        hidValJoiningDateFormat.Value = Resources.LocalizedResources.valJoiningDate;
        hidvalBlankPaymentStartDate.Value = Resources.LocalizedResources.valBlankPaymentStartDate;
        hidvalPaymentStartDateFormat.Value = Resources.LocalizedResources.valPaymentStartDateFormat;
        hidvalBlankAgreementdate.Value = Resources.LocalizedResources.valBlankAfreementDate;
        hidvalAgreementDateFormat.Value = Resources.LocalizedResources.valAfreementDateFormat;
        hidPaymentGroupMsg.Value = Resources.LocalizedResources.msgPaymentGroupChange;
        hidPaymentStartDateAD.Value = Resources.LocalizedResources.valPaymentDateOverAgreementDate;
        hidJoiningDateValPSD.Value = Resources.LocalizedResources.valJoiningDateOverPaymentDate;
        hidJoiningDateValAD.Value = Resources.LocalizedResources.valJoiningDateOverAgreementDate;
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtAddress.Text = string.Empty;
        txtAgreementDate.Text = string.Empty;
        txtJoiningDate.Text = string.Empty;
        txtName.Text = string.Empty;
        txtPaymentStartDate.Text = string.Empty;
        cmbDesignation.ClearSelection();
        cmbJobType.ClearSelection();
        cmbPaymentGroup.ClearSelection();
        hidAppointmentId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
        FillEarningDeductions(0);
        txtAgreementDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtJoiningDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtPaymentStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtEmpNo.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to populate user appointment details object.
    /// </summary>
    /// <returns></returns>
    private UserAppointmentDetails Populate()
    {
        return new UserAppointmentDetails
        {
            SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue),
            Address = txtAddress.Text.Trim(),
            AgreementDate = Convert.ToDateTime(txtAgreementDate.Text.Trim()),
            DesignationId = Convert.ToInt32(cmbDesignation.SelectedValue),
            EarningDeductionXml = GetEarningDeductionXml(),
            Id = Convert.ToInt32(hidAppointmentId.Value),
            JoiningDate = Convert.ToDateTime(txtJoiningDate.Text.Trim()),
            Name = txtName.Text.Trim(),
            PaymentStartDate = Convert.ToDateTime(txtPaymentStartDate.Text.Trim()),
            Status = new StaffStatusDetails { StatusId = Convert.ToInt32(cmbJobType.SelectedValue) },
            PaymentGroupId = Convert.ToInt32(cmbPaymentGroup.SelectedValue),
            EmployeeNo = txtEmpNoPrefix.Text.Trim() + txtEmpNo.Text.Trim(),
        };
    }

    /// <summary>
    /// This method is used to return earning deduction xml.
    /// </summary>
    /// <returns></returns>
    private string GetEarningDeductionXml()
    {
        List<EarningDeductionGroup> lstEarningDeductionGroup = new List<EarningDeductionGroup>();
        foreach (ListViewDataItem oCurrentItem in lstvwParameters.Items)
        {
            int iEarningDeductionId = Convert.ToInt32(lstvwParameters.DataKeys[oCurrentItem.DisplayIndex]["EarningDeductionId"]);
            TextBox txtAmount = oCurrentItem.FindControl("txtAmount") as TextBox;
            lstEarningDeductionGroup.Add
                (
                    new EarningDeductionGroup
                    {
                        PaymentGroupId = Convert.ToInt32(cmbPaymentGroup.SelectedValue),
                        EarningDeductionId = iEarningDeductionId,
                        Amount = Convert.ToInt32(txtAmount.Text)
                    }

                );
        }

        return base.GenerateXml(lstEarningDeductionGroup);
    }

    /// <summary>
    /// This method is used to update fields according to retrieved values.
    /// </summary>
    /// <param name="aiAppointmentId"></param>
    private void UpdateFields(int aiAppointmentId)
    {
        hidAppointmentId.Value = aiAppointmentId.ToString();
        UserAppointmentDetails oUserAppointmentDetails = moUserAppointmentDetailsBL.Get(aiAppointmentId);
        cmbSalutation.SelectedValue = oUserAppointmentDetails.SalutationId.ToString();
        txtAddress.Text = oUserAppointmentDetails.Address;
        txtAgreementDate.Text = oUserAppointmentDetails.AgreementDate.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        cmbDesignation.SelectedValue = oUserAppointmentDetails.DesignationId.ToString();        
        hidAppointmentId.Value = oUserAppointmentDetails.Id.ToString();
        txtJoiningDate.Text = oUserAppointmentDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        txtName.Text = oUserAppointmentDetails.Name;
        txtPaymentStartDate.Text = oUserAppointmentDetails.PaymentStartDate.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
        cmbJobType.SelectedValue = oUserAppointmentDetails.Status.StatusId.ToString();
        cmbPaymentGroup.SelectedValue = oUserAppointmentDetails.PaymentGroupId.ToString();
        cmbPaymentGroup_SelectedIndexChanged(cmbPaymentGroup, null);
        txtEmpNo.Text = oUserAppointmentDetails.EmployeeNo.Replace(Settings.EmployeeNoPrefix, string.Empty);

        decimal iGrossSalary = 0;
        foreach (ListViewDataItem oCurrentItem in lstvwParameters.Items)
        {
            int iEarningDeductionId = Convert.ToInt32(lstvwParameters.DataKeys[oCurrentItem.DisplayIndex]["EarningDeductionId"]);
            TextBox txtAmount = oCurrentItem.FindControl("txtAmount") as TextBox;
            EarningDeductionGroup oEarningDeductionGroup = oUserAppointmentDetails.EarningDeductions.Where(ed => ed.EarningDeductionId == iEarningDeductionId).FirstOrDefault();
            if (oEarningDeductionGroup != null)
            {
                txtAmount.Text = oEarningDeductionGroup.Amount.ToString();

                HiddenField hidIsEarning = oCurrentItem.FindControl("hidIsEarning") as HiddenField;

                if (hidIsEarning.Value == "True")
                    iGrossSalary = iGrossSalary + oEarningDeductionGroup.Amount;
                else
                    iGrossSalary = iGrossSalary - oEarningDeductionGroup.Amount;
            }
        }
        lblGrossSalary.Text = iGrossSalary.ToString();
    }

    /// <summary>
    /// This method is used to fill up designation combo box.
    /// </summary>
    private void FillDesignations()
    {
        DesignationMasterBL oDesignationMasterBL = new DesignationMasterBL();
        List<DesignationMaster> lstDesgnations = oDesignationMasterBL.GetAll();
        lstDesgnations = lstDesgnations.Where(dg => dg.UserRoleId != Convert.ToInt32(Constants.UserRoles.ExAdmin)).ToList();
        ListSource.FillDropDownList(lstDesgnations, cmbDesignation, "Designation", "DesignationId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill pup job type combo box.
    /// </summary>
    private void FillJobTypes()
    {
        StaffStatusBL oStaffStatusBL = new StaffStatusBL();
        List<StaffStatusDetails> lstStaffStatusDetails = oStaffStatusBL.GetStaffStatusTypes();
        ListSource.FillDropDownList(lstStaffStatusDetails, cmbJobType, "StatusName", "StatusId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill up payment group combo box.
    /// </summary>
    private void FillPaymentGroups()
    {
        PaymentGroupBL oPaymentGroupBL = new PaymentGroupBL(miSchoolId, miUserId);
        List<PaymentGroup> lstPaymentGroups = oPaymentGroupBL.GetAll();
        ListSource.FillDropDownList(lstPaymentGroups, cmbPaymentGroup, "Name", "Id", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to fill up payment group combo box.
    /// </summary>
    private void FillSalutation()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);        
    }

    /// <summary>
    /// This method is used to return encrypted query string.
    /// </summary>
    /// <param name="asQueryString"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(string asQueryString)
    {
        return "PayrollReportUI.aspx?" + CommonUtility.EncryptQuerystring(asQueryString);
    }

    private void SetDefaultValues()
    {
        txtEmpNoPrefix.Text = Settings.EmployeeNoPrefix;
    }
    #endregion
}