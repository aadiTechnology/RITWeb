/* File Name :- LeaveEnCashmentPopup.aspx.cs
 * Created Date :- 11-Oct-2019
 * Class Description :- This class is used to manage encash leave Details.
 * Created By :- Dnyaneshwar
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using PayrollEntities;
using BusinessLogic;
using System.Data;
using Utility;

public partial class LeaveEnCashmentPopup : SchoolBase
{
    #region Constant(s)

    private const string S_DELETE_MESSAGE = "Leave encashment details deleted successfully!!!";
    private const string S_UPDATE_MESSAGE = "Leave encashment details updated successfully!!!";
    private const string S_SAVE_MESSAGE = "Leave encashment details saved successfully!!!";    
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";

    #endregion

    #region Data Member(s)
    
    private UserLeavesYearwiseConfigurationBL moUserLeavesYearwiseConfigurationBL;

    #endregion

    #region Event's

    /// <summary>
    /// This event is used ti initialise all controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserLeavesYearwiseConfigurationBL = new UserLeavesYearwiseConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillYearCombo();
                FillLeaveCombo();
                FillUserEncashLeaveDetailsList();
                SetJavascriptAttributes();
                SetLeaveDetails();
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to save button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            LeaveEncashmentDetails oLeaveEncashmentDetails = Pupulate();
            moUserLeavesYearwiseConfigurationBL.SaveEncashmentDetails(oLeaveEncashmentDetails);            
            if (oLeaveEncashmentDetails.Id == 0)            
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);            
            else            
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);

            ClearFields();
            FillUserEncashLeaveDetailsList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Item data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEncashLeaveDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LeaveEncashmentDetails oLeaveEncashmentDetails = e.Item.DataItem as LeaveEncashmentDetails;

                Label lblDate = e.Item.FindControl("lblDate") as Label;
                lblDate.Text = oLeaveEncashmentDetails.Date.ToString(Constants.S_DATE_FORMAT);

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
    /// This event is used to item command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwEncashLeaveDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iEncashLeaveId = Convert.ToInt32(lstvwEncashLeaveDetails.DataKeys[e.Item.DisplayIndex]["Id"]);                
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    hidId.Value = iEncashLeaveId.ToString();
                    LeaveEncashmentDetails oLeaveEncashmentDetails = moUserLeavesYearwiseConfigurationBL.GetUserEncashLeaveDetails(hidUserId.Value.ToInt(), iEncashLeaveId);
                    cmbYear.SelectedValue = oLeaveEncashmentDetails.Year.ToString();
                    txtDate.Text = oLeaveEncashmentDetails.Date.ToString(Constants.S_DATE_FORMAT);
                    cmbLeaveType.SelectedValue = oLeaveEncashmentDetails.LeaveId.ToString();
                    cmbLeaveType_SelectedIndexChanged(sender, e);
                    txtLeaveCount.Text = oLeaveEncashmentDetails.EncashCount.ToString();
                    txtAmount.Text = oLeaveEncashmentDetails.Amount.ToString();
                    txtDescription.Text = oLeaveEncashmentDetails.Description;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moUserLeavesYearwiseConfigurationBL.DeleteUserEncashLeave(hidUserId.Value.ToInt(), iEncashLeaveId);
                    FillUserEncashLeaveDetailsList();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    ClearFields();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to leave type selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbLeaveType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillLeaveBalaceCount();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to clear all the fields.
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

    #endregion

    #region Private methods

    /// <summary>
    /// This method is used to fill year combobox.
    /// </summary>
    private void FillYearCombo()
    {
        List<LeaveYear> lstYears = moUserLeavesYearwiseConfigurationBL.GetLeaveYears();
        ListSource.FillDropDownList(lstYears, cmbYear, "Year", "Id", string.Empty);
        var oYear = lstYears.Where(yr => yr.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= yr.EndDate.Date).FirstOrDefault();
        if (oYear != null)
            cmbYear.SelectedValue = oYear.Id.ToString();
    }

    /// <summary>
    /// This method is used to fill leave combobox.
    /// </summary>
    private void FillLeaveCombo()
    {
        DataSet dt = moUserLeavesYearwiseConfigurationBL.GetUserLeavesForEncashment(Convert.ToInt32(hidUserId.Value));
        DataTable dtLeaveDetails = dt.Tables[0];
        ListSource.FillDropDownList(dtLeaveDetails, cmbLeaveType, "LeaveName", "LeaveId", Constants.S_SELECT);
        lblUserName.Text = dt.Tables[1].Rows[0]["UserName"].ToString();
    }

    /// <summary>
    /// This method is used to fill User encash leave details list view.
    /// </summary>
    private void FillUserEncashLeaveDetailsList()
    {
        List<LeaveEncashmentDetails> lstLeaveEncashmentDetails = moUserLeavesYearwiseConfigurationBL.GetUserAllEncashLeaveDetails(hidUserId.Value.ToInt());
        lstvwEncashLeaveDetails.DataSource = lstLeaveEncashmentDetails;
        lstvwEncashLeaveDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill leave balance count.
    /// </summary>
    private void FillLeaveBalaceCount()
    {   
        decimal dLeaveBalance = moUserLeavesYearwiseConfigurationBL.GetLeaveBalanceForEncashment(hidUserId.Value.ToInt(), cmbLeaveType.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt());
        txtLeaveBalance.Text = dLeaveBalance.ToString();
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    /// <returns></returns>
    private void ReadQuerystring()
    {
        hidUserId.Value = QueryString["UserId"];
        hidUserRoleId.Value = QueryString["UserRoleId"];
        hidFilter.Value = QueryString["Filter"];        
    }

    /// <summary>
    /// This method is used to populate encash leave details for save.
    /// </summary>
    /// <returns></returns>
    private LeaveEncashmentDetails Pupulate()
    {
        LeaveEncashmentDetails oLeaveEncashmentDetails = new LeaveEncashmentDetails();
        oLeaveEncashmentDetails.Id = hidId.Value.ToInt();
        oLeaveEncashmentDetails.UserId = hidUserId.Value.ToInt();
        oLeaveEncashmentDetails.Year = cmbYear.SelectedValue.ToInt();
        oLeaveEncashmentDetails.Date = txtDate.Text.ToDateTime();
        oLeaveEncashmentDetails.LeaveId = cmbLeaveType.SelectedValue.ToInt();
        oLeaveEncashmentDetails.EncashCount = txtLeaveCount.Text.ToDecimal();
        if(txtAmount.Text != string.Empty)
            oLeaveEncashmentDetails.Amount = txtAmount.Text.ToDecimal();
        if(txtDescription.Text != string.Empty)
            oLeaveEncashmentDetails.Description = txtDescription.Text.Trim();

        return oLeaveEncashmentDetails;
    }

    /// <summary>
    /// This method is used to clear all fields.
    /// </summary>
    private void ClearFields()
    {
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        cmbLeaveType.SelectedValue = Constants.S_ZERO;
        txtLeaveBalance.Text = string.Empty;
        txtLeaveCount.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnSave.Text = S_SAVE_TEXT;
        hidId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to set leave amount.
    /// </summary>
    private void SetLeaveDetails()
    {
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {
            DataTable dt = moUserLeavesYearwiseConfigurationBL.GetAmountForLeaveEncashment(hidUserId.Value.ToInt(), txtDate.Text, cmbLeaveType.SelectedValue.ToInt(), miAcademicYearId, miSchoolId);
            hidAmount.Value = dt.Rows[0][0].ToString();
            txtLeaveCount.Attributes.Add("onchange", "javascript:CountAmount()");
        }
    }

    #endregion   
}