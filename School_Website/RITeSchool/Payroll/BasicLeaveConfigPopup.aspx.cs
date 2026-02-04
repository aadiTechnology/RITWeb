/*File Name - BasicLeaveConfigPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 24 Dec 2012
 * Description - This class is used to configure basic leave details.
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using PayrollEntities;
using Utility;
using System.Data;

public partial class BasicLeaveConfigPopup : SchoolBase
{   
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Configuration Saved Successfully!!!";
    private const string S_DELETE_MESSAGE = "Configuration Deleted Successfully!!!";
    private const string S_EDIT_COMMAND = "EDIT";
    private const string S_DELETE_COMMAND = "DELETE";

    #endregion

    #region Data Member(s)

    private StaffLeavesBL moStaffLeavesBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill staff group and month combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {         
            moStaffLeavesBL = new StaffLeavesBL(miSchoolId, miUserId);            
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    FillControls();
                    SetDefaultValues();                    
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ValidateConfiguration();
            Save();
            lblMessage.Text = S_SAVE_MESSAGE;
            ResetFields();
            FillBasicLeaveConfigs();
        }
        catch (DuplicateEntityException dex)
        {
            lblMessage.Text = dex.ErrorMessage;
            lblMessage.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes for leave textboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguredLeaves_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TextBox txtBasicLeave = e.Item.FindControl("txtBasicLeave") as TextBox;
                TextBox txtAccLeave = e.Item.FindControl("txtAccLeave") as TextBox;
                HiddenField hidBasicLeave = e.Item.FindControl("hidBasicLeave") as HiddenField;
                HiddenField hidAccLeave = e.Item.FindControl("hidAccLeave") as HiddenField;

                txtBasicLeave.Attributes.Add("onchange", "CheckValue(this," + e.Item.DisplayIndex + ")");
                txtAccLeave.Attributes.Add("onchange", "CheckValue(this," + e.Item.DisplayIndex + ")");
                txtAccLeave.Enabled = chkAccumulateLeave.Checked;
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fillup controls on edit and delete configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBasicLeaveDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iId = Convert.ToInt32(lstvwBasicLeaveDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
            if (e.CommandName == S_EDIT_COMMAND)
            {
                hidConfigId.Value = iId.ToString();
                FillControlsToUpdate(iId);
                //SetFieldState();
            }
            else if (e.CommandName == S_DELETE_COMMAND)
            {
                moStaffLeavesBL.DeleteBasicLeaveConfig(iId);
                lblMessage.Text = S_DELETE_MESSAGE;
                FillBasicLeaveConfigs();
                if (hidConfigId.Value == iId.ToString())
                    ResetFields();
            }
        }
        catch (SqlException se)
        {
            lblMessage.Text = se.Message;
            lblMessage.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for handling item editing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBasicLeaveDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    {
    }

    /// <summary>
    /// This event is used for handling item deleting event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBasicLeaveDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
    }

    /// <summary>
    /// This event is used to reset controls.
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (hidConfigId.Value == Constants.S_ZERO)
            {
                if (cmbStaffGroup.SelectedValue != Constants.S_ZERO)
                {
                    List<BasicLeaveConfiguration> lstConfigLeaves = moStaffLeavesBL.GetBasicLeaveConfigs(0);
                    if (lstConfigLeaves.FindAll(lv => lv.StaffGroups.StaffGroupsId == Convert.ToInt32(cmbStaffGroup.SelectedValue)).Count == 0)
                    {
                        cmbMonth.SelectedValue = Constants.S_ONE;
                        SetFieldState();
                    }
                    else
                    {
                        cmbMonth.Enabled = true;                        
                        chkAccumulateLeave.Enabled = true;
                        chkAccumulateLeave.Checked = false;                        
                    }
                }
                else
                {
                    chkAccumulateLeave.Enabled = true;
                    chkAccumulateLeave.Checked = false;
                    cmbMonth.Enabled = true;
                }

                DisableAccLeaveColumn();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attribute for delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBasicLeaveDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event i used to fill up configuration listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbConfigStaffGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillBasicLeaveConfigs();
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to apply configuration of selcted year to all the users of selected staff group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApplyToAll_Click(object sender, EventArgs e)
    {
        try
        {
            int iStaffGroupsId = Convert.ToInt32(cmbConfigStaffGroups.SelectedValue);
            bool bUpdateExisting = hidUpdateExisting.Value == Constants.S_YES;            
            int iLeaveSeperaterDay = Settings.LeaveSeperaterDay;
            moStaffLeavesBL.ApplyToAllUsers(iStaffGroupsId, bUpdateExisting, iLeaveSeperaterDay);
            lblMessage.Text = "Configuration is applied for all the users successfully!!!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to disable accumulated leave controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkAccumulateLeave_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DisableAccLeaveColumn();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    private void DisableAccLeaveColumn()
    {
        foreach (ListViewDataItem oItem in lstvwConfiguredLeaves.Items)
        {
            TextBox txtAccLeave = oItem.FindControl("txtAccLeave") as TextBox;
            txtAccLeave.Text = Constants.S_ZERO;
            txtAccLeave.Enabled = chkAccumulateLeave.Checked;
        }
    }	

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, BtnCancel, btnClose, btnApplyToAll });
        BtnSave.Attributes.Add("onclick", "ResetFields()");
        btnApplyToAll.Attributes.Add("onclick", "if(!ConfirmUpdateExisting()) return false;");
    }

    /// <summary>
    /// This method is used to fill controls.
    /// </summary>
    private void FillControls()
    {
        FillStaffGroupCombo();
        FillMonthCombo();
        FillConfiguredLeaves();
        FillBasicLeaveConfigs();
        setStaffGroup();
    }

    /// <summary>
    /// This method is used to set default staff group.
    /// </summary>
    private void setStaffGroup()
    {
        if (QueryString["StaffGroupId"] != null)
        {
            cmbStaffGroup.SelectedValue = QueryString["StaffGroupId"];
            cmbStaffGroup_SelectedIndexChanged(cmbStaffGroup, null);
            cmbStaffGroup.Focus();
        }
    }

    /// <summary>
    /// This method is used to fill up basic leave config listview.
    /// </summary>
    private void FillBasicLeaveConfigs()
    {
        List<BasicLeaveConfiguration> lstConfigLeaves = moStaffLeavesBL.GetBasicLeaveConfigs(0);

        if (cmbConfigStaffGroups.SelectedValue != Constants.S_ZERO)
            lstConfigLeaves = lstConfigLeaves.Where(cl => cl.StaffGroups.StaffGroupsId == Convert.ToInt32(cmbConfigStaffGroups.SelectedValue)).ToList();

        var oConfigDetails = lstConfigLeaves.Select(lv => new { lv.Id, lv.StaffGroups.StaffGroupsName, Month = lv.Month.MonthAbbreviation, lv.IsAccumulationMonth });

        lstvwBasicLeaveDetails.DataSource = oConfigDetails;
        lstvwBasicLeaveDetails.DataBind();

        btnApplyToAll.Visible = tr1.Visible = oConfigDetails.Count() != 0;
    }

    /// <summary>
    /// This method is used to fill up configured leave listview.
    /// </summary>
    private void FillConfiguredLeaves()
    {
        List<BasicLeaveDetails> lstConfigLeaves = moStaffLeavesBL.GetBasicLeaveDetails();
        var oLeaves = lstConfigLeaves.Select(cl => new { Id = 0, cl.Leave.LeaveId, cl.Leave.LeaveName, BasicLeaves = 0, AccumulateLeaves = 0 }).Distinct();
        if (lstConfigLeaves.Count > 0)
        {
            lstvwConfiguredLeaves.DataSource = oLeaves;
            lstvwConfiguredLeaves.DataBind();
        }
    }

    /// <summary>
    /// This method is used to fill monh combobox.
    /// </summary>
    private void FillMonthCombo()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        List<MonthMaster> lstMonths = SchoolWiseAcademicYearMasterBL.GetAllMonth();
        ListSource.FillDropDownList(lstMonths, cmbMonth, "MonthAbbreviation", "MonthId", string.Empty);
    }

    /// <summary>
    /// This method is used to fill staff group combobox.
    /// </summary>
    private void FillStaffGroupCombo()
    {        
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        DataTable oDTStaffGroups = StaffGroupsBL.GetAll(miSchoolId);
        DataTable oDT = oDTStaffGroups.Select("SchoolId=" + miSchoolId).CopyToDataTable();
        ControlUtility.FillDropDownList(oDT, ref cmbStaffGroup, "StaffGroupsId", "StaffGroupsName", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDT, ref cmbConfigStaffGroups, "StaffGroupsId", "StaffGroupsName", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to validate configuration.
    /// </summary>
    private void ValidateConfiguration()
    {
        List<BasicLeaveConfiguration> lstConfigLeaves = moStaffLeavesBL.GetBasicLeaveConfigs(0);
        if (lstConfigLeaves.FindAll(lv => lv.StaffGroups.StaffGroupsId == Convert.ToInt32(cmbStaffGroup.SelectedValue) && lv.Month.MonthId == Convert.ToInt32(cmbMonth.SelectedValue) && lv.Id != Convert.ToInt32(hidConfigId.Value)).Count > 0)
            throw new DuplicateEntityException("Configuration is already exist for selected Staff Group and Month.");
    }

    /// <summary>
    /// This method is used to populate leave object.
    /// </summary>
    /// <returns></returns>
    private List<BasicLeaveDetails> PopulateLeaves()
    {
        List<BasicLeaveDetails> lstLeaves = new List<BasicLeaveDetails>();
        foreach (ListViewDataItem oItem in lstvwConfiguredLeaves.Items)
        {
            int iLeaveId = Convert.ToInt32(lstvwConfiguredLeaves.DataKeys[oItem.DisplayIndex]["LeaveId"]);
            int iId = Convert.ToInt32(lstvwConfiguredLeaves.DataKeys[oItem.DisplayIndex]["Id"]);
            TextBox txtBasicLeave = oItem.FindControl("txtBasicLeave") as TextBox;
            TextBox txtAccLeave = oItem.FindControl("txtAccLeave") as TextBox;

            BasicLeaveDetails oBasicLeaveDetails = new BasicLeaveDetails
            {
                Id = iId,
                BasicLeaveConfigId = Convert.ToInt32(hidConfigId.Value),
                BasicLeaves = Convert.ToDecimal(txtBasicLeave.Text),
                AccumulateLeaves = Convert.ToDecimal(txtAccLeave.Text),
                LeaveId = iLeaveId
            };
            lstLeaves.Add(oBasicLeaveDetails);
        }

        return lstLeaves;
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        cmbStaffGroup.ClearSelection();        
        cmbMonth.Enabled = true;
        chkAccumulateLeave.Checked = false;
        cmbMonth.SelectedValue = Constants.S_ONE;
        chkAccumulateLeave.Enabled = true;        
        hidConfigId.Value = Constants.S_ZERO;
        FillConfiguredLeaves();
        if (lstvwBasicLeaveDetails.Items.Count == 0)
            SetFieldState();
        DisableAccLeaveColumn();
    }

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    private void SetFieldState()
    {
        //if (cmbMonth.SelectedValue == Constants.S_ONE)
        //{
        //    cmbMonth.Enabled = false;
        //    chkAccumulateLeave.Checked = true;
        //    chkAccumulateLeave.Enabled = false;
        //}
        //else
        //{
        //    cmbMonth.Enabled = true;
        //    chkAccumulateLeave.Enabled = true;
        //}

        //chkAccumulateLeave.Checked = true;
        //chkAccumulateLeave.Enabled = false;        
    }

    /// <summary>
    /// This method is used to save configuration.
    /// </summary>
    private void Save()
    {
        BasicLeaveConfiguration oBasicLeaveConfiguration = new BasicLeaveConfiguration
        {
            Id = Convert.ToInt32(hidConfigId.Value),
            IsAccumulationMonth = chkAccumulateLeave.Checked,
            StaffGroups = new StaffGroupsEntity
            {
                StaffGroupsId = Convert.ToInt32(cmbStaffGroup.SelectedValue)
            },
            Month = new MonthMaster
            {
                MonthId = Convert.ToInt32(cmbMonth.SelectedValue)
            },
            LeaveXml = GenerateXml(PopulateLeaves()),            
            UpdatedById = miUserId            
        };
        moStaffLeavesBL.SaveBasicLeaveConfig(oBasicLeaveConfiguration);
    }

    /// <summary>
    /// This method is used to fill controls to update.
    /// </summary>
    /// <param name="iId"></param>
    private void FillControlsToUpdate(int aiId)
    {
        List<BasicLeaveConfiguration> lstConfigLeaves = moStaffLeavesBL.GetBasicLeaveConfigs(aiId);
        BasicLeaveConfiguration oBasicLeaveConfiguration = lstConfigLeaves.Where(blc => blc.Id == aiId || blc.Id == 0).FirstOrDefault();
        cmbMonth.SelectedValue = oBasicLeaveConfiguration.Month.MonthId.ToString();
        cmbStaffGroup.SelectedValue = oBasicLeaveConfiguration.StaffGroups.StaffGroupsId.ToString();
        chkAccumulateLeave.Checked = oBasicLeaveConfiguration.IsAccumulationMonth;        
        lstvwConfiguredLeaves.DataSource = oBasicLeaveConfiguration.Leaves.Select(lv => new { lv.Id, lv.Leave.LeaveName, lv.Leave.LeaveId, lv.BasicLeaves, lv.AccumulateLeaves });
        lstvwConfiguredLeaves.DataBind();
    }

    /// <summary>
    /// This method is used to check pre-condition.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.BasicLeaveConfiguration);

        if (!sLinks.Equals(string.Empty))
        {
            divErr.InnerHtml = sLinks;
            trDetails.Visible = false;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }

        return bReturn;
    }

    #endregion
}