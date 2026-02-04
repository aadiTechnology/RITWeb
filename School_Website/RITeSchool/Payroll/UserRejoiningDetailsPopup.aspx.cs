/* File Name :- UserRejoiningDetailsPopup.aspx.cs
 * Created Date :- 08-Nov-2019
 * Class Description :- This class is used to Rejoin Users in system. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using PayrollEntities;
using BusinessLogic.Exceptions;
using SchoolEntities.Payroll;
using System.Reflection;

public partial class UserRejoiningDetailsPopup : SchoolBase
{

    #region Constant(s)

    private const string S_DELETE_MESSAGE = "User Joinig details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "User Joinig details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "User Joinig details saved successfully !!!";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";    

    #endregion

    #region Datamember(s)

    UserRejoiningDetailsBL moUserRejoiningDetailsBL;

    #endregion

    #region Event's

    /// <summary>
    /// This method is used to load All default Controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserRejoiningDetailsBL = new UserRejoiningDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillUSerStaffCombo();
                FillUsersListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to user staff selected index changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserComboBox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to User name combobox seleceted index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserDetailsForRejoining();
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
            UserRejoiningDetails oUserRejoiningDetails = Populate();
            moUserRejoiningDetailsBL.Save(oUserRejoiningDetails);
            FillUsersListView();
            ClearFields();
            if (oUserRejoiningDetails.UserRejoinId == Constants.I_ZERO)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel button click event.
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
    /// This event is used to set values for liast view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblJoiningDate = e.Item.FindControl("lblJoiningDate") as Label;
                Label lblResignationDate = e.Item.FindControl("lblResignationDate") as Label;
                UserRejoiningDetails oUserRejoiningDetails = e.Item.DataItem as UserRejoiningDetails;

                if (oUserRejoiningDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblJoiningDate.Text = oUserRejoiningDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
                else
                    lblJoiningDate.Text = "-";

                if (oUserRejoiningDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblResignationDate.Text = oUserRejoiningDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT);
                else
                    lblResignationDate.Text = "-";

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
    /// This event is used to Item command event for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {   
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iReJoineUserId = Convert.ToInt32(lstvwUserDetails.DataKeys[e.Item.DisplayIndex]["UserRejoinId"]);
                int iUserId = Convert.ToInt32(lstvwUserDetails.DataKeys[e.Item.DisplayIndex]["UserId"]);

                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    UserRejoiningDetails oUserRejoinigDetails = moUserRejoiningDetailsBL.Get(Constants.I_ZERO, iUserId, iReJoineUserId);
                    hidUserReJoiningId.Value = iReJoineUserId.ToString();                    
                    cmbStaffGroup.SelectedValue = oUserRejoinigDetails.StaffGroupId.ToString();
                    cmbStaffGroup_SelectedIndexChanged(sender, e);
                    cmbUserName.SelectedValue = oUserRejoinigDetails.UserId.ToString();
                    txtEmployeeNo.Text = oUserRejoinigDetails.EmployeeNo;
                    txtAccountNo.Text = oUserRejoinigDetails.AccountNo;
                    txtPFNo.Text = oUserRejoinigDetails.PFNo;
                    txtUAN.Text = oUserRejoinigDetails.UAN;
                    txtPANNo.Text = oUserRejoinigDetails.PANNo;

                    if (oUserRejoinigDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtJoiningDate.Text = oUserRejoinigDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);

                    if (oUserRejoinigDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                        txtResignationDate.Text = oUserRejoinigDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT);
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moUserRejoiningDetailsBL.Delete(iReJoineUserId);
                    FillUsersListView();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidUserReJoiningId.Value) == iReJoineUserId)
                        ClearFields();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set pager of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUserDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwUserDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUserDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillUsersListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This Method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack, btnSearch });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This Method is used to fill staff group combobox.
    /// </summary>
    private void FillUSerStaffCombo()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_SELECT);

        FillUserComboBox();
    }

    /// <summary>
    /// This method is used to fill User combobox.
    /// </summary>
    private void FillUserComboBox()
    {   
        List<UserRejoiningDetails> lstUserRejoiningDetails = moUserRejoiningDetailsBL.GetAllUsers(cmbStaffGroup.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstUserRejoiningDetails, cmbUserName, "UserName", "UserId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to Fill User details for rejoining.
    /// </summary>
    private void FillUserDetailsForRejoining()
    {
        UserRejoiningDetails oUserRejoiningDetails = moUserRejoiningDetailsBL.Get(cmbStaffGroup.SelectedValue.ToInt(), cmbUserName.SelectedValue.ToInt(), Constants.I_ZERO);

        txtEmployeeNo.Text = oUserRejoiningDetails.EmployeeNo;
        txtAccountNo.Text = oUserRejoiningDetails.AccountNo;
        txtPFNo.Text = oUserRejoiningDetails.PFNo;
        txtUAN.Text = oUserRejoiningDetails.UAN;
        txtPANNo.Text = oUserRejoiningDetails.PANNo;
        if (oUserRejoiningDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
            txtJoiningDate.Text = oUserRejoiningDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
        if (oUserRejoiningDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
            txtResignationDate.Text = oUserRejoiningDetails.ResignationDate.ToString(Constants.S_DATE_FORMAT);
        if(oUserRejoiningDetails.OldJoiningDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
            hidOldJoiningDate.Value = oUserRejoiningDetails.OldJoiningDate.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to populate user details for save in to database.
    /// </summary>
    /// <returns></returns>
    private UserRejoiningDetails Populate()
    {
        UserRejoiningDetails oUserRejoiningDetails = new UserRejoiningDetails();

        oUserRejoiningDetails.UserRejoinId = hidUserReJoiningId.Value.ToInt();
        oUserRejoiningDetails.UserId = cmbUserName.SelectedValue.ToInt();
        oUserRejoiningDetails.StaffGroupId = cmbStaffGroup.SelectedValue.ToInt();
        oUserRejoiningDetails.EmployeeNo = txtEmployeeNo.Text.Trim();
        oUserRejoiningDetails.AccountNo = txtAccountNo.Text.Trim();
        oUserRejoiningDetails.PFNo = txtPFNo.Text.Trim();
        oUserRejoiningDetails.UAN = txtUAN.Text.Trim();
        oUserRejoiningDetails.PANNo = txtPANNo.Text.Trim();
        oUserRejoiningDetails.JoiningDate = txtJoiningDate.Text.ToDateTime();
        oUserRejoiningDetails.ResignationDate = txtResignationDate.Text.ToDateTime();

        return oUserRejoiningDetails;
    }

    /// <summary>
    /// This method is used to Fill users list view.
    /// </summary>
    private void FillUsersListView()
    {
        lstvwUserDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to clear all the fields.
    /// </summary>
    private void ClearFields()
    {
        cmbStaffGroup.ClearSelection();
        cmbUserName.ClearSelection();
        txtEmployeeNo.Text = string.Empty;
        txtAccountNo.Text = string.Empty;
        txtPFNo.Text = string.Empty;
        txtUAN.Text = string.Empty;
        txtPANNo.Text = string.Empty;
        txtJoiningDate.Text = string.Empty;
        txtResignationDate.Text = string.Empty;        
        hidUserReJoiningId.Value = Constants.S_ZERO;
        hidUserStaffGroupId.Value = Constants.S_ZERO;
    }

    #endregion
}