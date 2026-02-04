/* File Name :- ODDetailsPopup.aspx.cs
 * Created Date :- 13-Jan-2016
 * Class Description :- This class is used to manage staff members O.D Details. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities;
using PayrollEntities;
using Utility;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class ODLeaveDetailsPopup : SchoolBase
{

    #region Constants

    private const string S_DELETE_MESSAGE = "O.D. details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "O.D. details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "O.D. details saved successfully !!!";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";
    private const string S_SORT_ROW = "SortRow";
    private const string S_TIME_FORMAT = "hh:mm tt";

    #endregion

    #region DataMember

    private ODDetailsBL moODDetailsBL;

    #endregion

    #region Page Events

    /// <summary>
    /// Thos event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty || hidSortDirection.Value == string.Empty)
            {
                hidSortExpression.Value = "Date";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            base.AddSortImage(lstvwODDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moODDetailsBL = new ODDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillComboBox();
                ReadQueryString();
                SetJavascriptAttributes();
                FillODDetailsList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save OD Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ODDetails oODDetails = Populate();
            moODDetailsBL.SaveODDetails(oODDetails);
            FillODDetailsList();
            ClearFields();
            if (oODDetails.ODId == 0)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to call ClearFields method for clear all fields.
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the  listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwODDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iODId = Convert.ToInt32(lstvwODDetails.DataKeys[e.Item.DisplayIndex]["ODId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    ODDetails oODDetails = moODDetailsBL.GetODDetail(iODId);
                    hidODId.Value = oODDetails.ODId.ToString();
                    cmbStaffGroup.SelectedValue = oODDetails.StaffGroupId.ToString();
                    FillUserComboBox();
                    cmbUserName.SelectedValue = oODDetails.UserId.ToString();
                    cmbStaffGroup.Enabled = false;
                    cmbUserName.Enabled = false;
                    txtStartDate.Text = oODDetails.Date.ToString(Constants.S_DATE_FORMAT);
                    txtEndDate.Text = oODDetails.EndDate.ToString(Constants.S_DATE_FORMAT);
                    txtStartTime.Text = oODDetails.Date.ToString("hh:mm tt");
                    txtEndTime.Text = oODDetails.EndDate.ToString("hh:mm tt");
                    txtLocation.Text = oODDetails.Location.ToString();
                    txtDescription.Text = oODDetails.Description.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moODDetailsBL.DeleteODDetails(iODId);
                    FillODDetailsList();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidODId.Value) == iODId)
                        ClearFields();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillODDetailsList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Bound Data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwODDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                ODDetails oODDetails = e.Item.DataItem as ODDetails;
                Label lblLeaveDate = e.Item.FindControl("lblLeaveDate") as Label;
                Label lblEndDate = e.Item.FindControl("lblEndDate") as Label;
                lblLeaveDate.Text = oODDetails.Date.ToString(Constants.S_DATE_FORMAT) + " " + oODDetails.Date.ToString("hh:mm tt");
                lblEndDate.Text = oODDetails.EndDate.ToString(Constants.S_DATE_FORMAT) + " " + oODDetails.EndDate.ToString("hh:mm tt");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound data for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwODDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwODDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwODDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sorting data in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwODDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillODDetailsList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used to Deleting Item From the listview.
    // </summary>
    protected void lstvwODDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

    // <summary>
    // This event is used to Editing Item From the listview.
    // </summary>
    protected void lstvwODDetails_ItemEditing(object sender, ListViewEditEventArgs e) { }

    /// <summary>
    /// This event is used to set values on users combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserComboBox();
            SetQueryStringForOD();
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwODDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search the members to view or set OD Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillODDetailsList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get all dates of OD.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillODDetailsList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This Method is used to Read query string.
    /// </summary>
    private void ReadQueryString()
    {
        cmbStaffGroup.SelectedValue = QueryString["StaffGroupId"];
        hidCmbValue.Value = cmbStaffGroup.SelectedValue.ToString();
        hidYearId.Value = QueryString["Year"];
        txtStartTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        txtEndTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        txtStartDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        FillUserComboBox();
        if (QueryString["UserId"] == null || QueryString["UserId"] == Constants.S_ZERO)
            hidUserId.Value = Constants.S_ZERO;
        else
        {
            hidUserId.Value = QueryString["UserId"];
            cmbUserName.SelectedValue = QueryString["UserId"].ToString();
        }
    }

    /// <summary>
    /// This Method is used to fill staff group combobox.
    /// </summary>
    private void FillComboBox()
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffGroup, "StaffGroupsName", "StaffGroupsId", Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to fill User combobox.
    /// </summary>
    private void FillUserComboBox()
    {
        StaffLeaveDetailsBL oStaffLeaveDetailsBL = new StaffLeaveDetailsBL();
        int iStaffGroupId = cmbStaffGroup.SelectedValue.ToInt();
        int iYear = hidYearId.Value.ToInt();
        List<UserBasicDetails> lstUserBasicDetails = oStaffLeaveDetailsBL.GetAllUsersForODDetails(iStaffGroupId, miSchoolId, miAcademicYearId, iYear, false);
        ListSource.FillDropDownList(lstUserBasicDetails, cmbUserName, "StaffName", "UserId", Constants.S_SELECT);
    }   

    /// <summary>
    /// This Method is used to fill OD Details Listview.
    /// </summary>
    private void FillODDetailsList()
    {
        lstvwODDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This Method is used to populate controls after save in database.
    /// </summary>
    public ODDetails Populate()
    {
        ODDetails oODDetails = new ODDetails();
        oODDetails.ODId = hidODId.Value.ToInt();
        oODDetails.Date = Convert.ToDateTime(txtStartDate.Text + " " + txtStartTime.Text.ToString());
        oODDetails.EndDate = Convert.ToDateTime(txtEndDate.Text + " " + txtEndTime.Text.ToString());
        oODDetails.Location = txtLocation.Text;
        oODDetails.Description = txtDescription.Text;
        oODDetails.UserId = cmbUserName.SelectedValue.ToInt();
        return oODDetails;
    }

    /// <summary>
    /// This Method is used to clear all fields.
    /// </summary>
    private void ClearFields()
    {
        cmbStaffGroup.Enabled = true;
        cmbUserName.Enabled = true;
        hidODId.Value = Constants.S_ZERO;
        txtStartDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtStartTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        txtEndTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        cmbStaffGroup.ClearSelection();
        cmbUserName.ClearSelection();
        txtLocation.Text = string.Empty;
        txtDescription.Text = string.Empty;
        btnSave.Text = S_SAVE_TEXT;
        hidSortDirection.Value = string.Empty;
        cmbUserName_SelectedIndexChanged(cmbUserName, null);
    }

    /// <summary>
    /// This Method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack, btnSearch });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This Method is used to set items in Date drop down list.
    /// </summary>
    private void SetcmbODDatesItem(string sName, string sValue)
    {
        ListItem item = new ListItem();
        item.Text = sName;
        item.Value = sValue;
    }

    /// <summary>
    /// This event is used to Search user name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchOD_Click(object sender, EventArgs e)
    {
        try
        {
            UserDetailsForOD oUserDetailsForOD = new UserDetailsForOD();
            ODDetailsBL oODDetailsBL  = new ODDetailsBL();
            oUserDetailsForOD = oODDetailsBL.GetUserDetailsForOD(txtName.Text.Trim(), miSchoolId, miAcademicYearId);
            if (oUserDetailsForOD.UserId != Constants.I_ZERO && oUserDetailsForOD.StaffGroupsId != Constants.I_ZERO)
            {

                cmbStaffGroup.SelectedValue = oUserDetailsForOD.StaffGroupsId.ToString();
                cmbStaffGroup_SelectedIndexChanged(cmbStaffGroup, null);

                cmbUserName.SelectedValue = oUserDetailsForOD.UserId.ToString();
                cmbUsers_SelectedIndexChanged(cmbUserName, null);
            }
            else
                lblNoRecordMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }



    /// <summary>
    /// This event is used to display leaves details as per selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetQueryStringForOD();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set Query string for OD details.
    /// </summary>
    private void SetQueryStringForOD()
    {
        string sQueryString = "&StaffGroupId=" + Convert.ToInt32(cmbStaffGroup.SelectedValue) + " " +
                              "&Year=" + hidYearId.Value +
                              "&UserId=" + Convert.ToInt32(cmbUserName.SelectedValue);
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }



    #endregion
}