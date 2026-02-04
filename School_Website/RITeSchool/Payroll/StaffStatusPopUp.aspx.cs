/* 
   Created By       :- Vinod  
   Created Date     :- 12-Sept-2011
   Class Description:- This class is used to manage staff status details.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class StaffStatusPopUp : SchoolBase
{
    #region Constants

    const int I_USER_ROLE_TABLE = 0;
    const string S_STAFF_STATUS_TYPE = "UserStaffType";
    const string S_SAVE_SUCCESSFUL_MESSAGE = "Service Type details saved successfully !!!";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set master page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (QueryString["IsFromStaffGroupAssociation"] == Constants.S_YES)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set javascript attribute, set default values, fill combobox and checkboxlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetConrolsProperty();
                SetJavascriptAttributes();
                SetDefaultVisibility(false);
                trNorecordFoundSearch.Visible = false;
                FillUserRoleCombobox();
                FillStaffStatusCheckboxList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetConrolsProperty()
    {
        if (QueryString["IsFromStaffGroupAssociation"] == Constants.S_YES)
        {
            btnClose.Visible = true;
            trPopupIdentity.Visible = true;
        }
        else
        {
            btnBack.Visible = true;
            trPopupIdentity.Visible = false;
        }
    }

    /// <summary>
    /// This event is used to search user as per the filter applied.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
            FillStaffStatusListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill rowwise status dropdownlist from listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffStatusType_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            DropDownList ddlStaffStatus = oCurrentItem.FindControl("cmbStaffStatusType") as DropDownList;
            List<StaffStatusDetails> olstStaffStatusDetails = (List<StaffStatusDetails>)ViewState[S_STAFF_STATUS_TYPE];
            ListSource.FillDropDownList(olstStaffStatusDetails, ddlStaffStatus, "StatusName", "StatusId", Constants.S_SELECT);
            ddlStaffStatus.SelectedValue = ((PayrollEntities.StaffStatusDetails)(oCurrentItem.DataItem)).StatusId.ToString();
            string sIsDeleted = ((PayrollEntities.StaffStatusDetails)(oCurrentItem.DataItem)).IsDeleted.ToString();
            string sIsLocked = ((PayrollEntities.StaffStatusDetails)(oCurrentItem.DataItem)).IsLocked.ToString();
            HiddenField hidIsDeleted = oCurrentItem.FindControl("hidIsDeleted") as HiddenField;
            HiddenField hidIsLocked = oCurrentItem.FindControl("hidIsLocked") as HiddenField;
            if (sIsDeleted == Constants.C_YES.ToString())
            {
                HtmlTableRow oHtmlTableRow = e.Item.FindControl("trItem") as HtmlTableRow;
                //oHtmlTableRow.Style.Add("color", "Red");
                oHtmlTableRow.Style.Add("color", "#FC0A2E !important");
                ddlStaffStatus.Enabled = false;
                hidIsDeleted.Value = Constants.I_ONE.ToString();
            }
            else
                hidIsDeleted.Value = Constants.I_ZERO.ToString();
            if (sIsLocked == Constants.C_YES.ToString())
            {
                HtmlTableRow oHtmlTableRow = e.Item.FindControl("trItem") as HtmlTableRow;
                //oHtmlTableRow.Style.Add("background-color", "Gainsboro");
                oHtmlTableRow.Style.Add("background-color", "#c3c3c3 !important");
                ddlStaffStatus.Enabled = false;
                hidIsLocked.Value = Constants.I_ONE.ToString();
            }
            else
                hidIsLocked.Value = Constants.I_ZERO.ToString();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager value and listview status header dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffStatusType_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStaffStatusType.Items.Count > Constants.I_ZERO)
            {
                SetConfirmationMessage();
                hidRowCnt.Value = Convert.ToString(lstvwStaffStatusType.Items.Count);
                ControlUtility.FillListViewPagerFooter(lstvwStaffStatusType, DtPgCount);
                DataPager oDataPager = lstvwStaffStatusType.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
                FillHeaderCombobox();
                SetDefaultVisibility(true);
            }
            else
            {
                SetDefaultVisibility(false);
                btnSave.Enabled = false;
                trNorecordFoundSearch.Visible = true;
                trPagerStaffStatus.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Staff Status details.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveStaffStatusDetails();
            trErrorMessage.Visible = true;
            lblMessage.Text = S_SAVE_SUCCESSFUL_MESSAGE;
            DataPager oDataPager = lstvwStaffStatusType.FindControl("DtPgDropDown") as DataPager;
            DropDownList ocmbDataPager = oDataPager.Controls[Constants.I_ZERO].FindControl("ddlCnt") as DropDownList;
            int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
            FillStaffStatusListView();
            //When Page size is greater than 1 and after deleting all the records from curr. page, that time curr. page contaon no record( show message 'No record found'.)
            if (iCurrentPage != Constants.I_ONE && lstvwStaffStatusType.Items.Count == Constants.I_ZERO)
                FillStaffStatusListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise Staff status list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStaffStatusType);
            FillStaffStatusListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to fill user role combobox.
    /// </summary>
    private void FillUserRoleCombobox()
    {
        UsersStaffGroupsAssociationBL oUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
        DataSet oDataSet = oUsersStaffGroupsAssociationBL.GetStaffGroupsAndRoles(miSchoolId);
        ControlUtility.FillDropDownList(oDataSet.Tables[I_USER_ROLE_TABLE], ref cmbUserRoles, "User_Role_Id", "User_Role_Name", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill header staff group combobox.
    /// </summary>
    private void FillHeaderCombobox()
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStaffStatusType.FindControl("trHeaderContol");
        DropDownList cmbAllStatusType = (DropDownList)oHtmlTableRow.FindControl("cmbAllStatusType");
        List<StaffStatusDetails> olstStaffStatusDetails = (List<StaffStatusDetails>)ViewState[S_STAFF_STATUS_TYPE];
        ListSource.FillDropDownList(olstStaffStatusDetails, cmbAllStatusType, "StatusName", "StatusId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill Staff status listview.
    /// </summary>
    private void FillStaffStatusListView()
    {
        StaffStatusBL oStaffStatusBL = new StaffStatusBL();
        string sStaffStatusList = string.Empty;
        hidChkStaffStatus.Value = string.Empty;

        foreach (ListItem oListItem in chkStaffStatus.Items)
            if (oListItem.Selected)
                hidChkStaffStatus.Value = hidChkStaffStatus.Value == string.Empty ? chkStaffStatus.SelectedValue : (hidChkStaffStatus.Value + ", " + oListItem.Value);

        btnSave.Enabled = true;
        trNorecordFoundSearch.Visible = false;
        lstvwStaffStatusType.DataSourceID = ObjDSStaffStatus.ID;
        lstvwStaffStatusType.DataBind();
    }

    /// <summary>
    /// This method is used to fill checkbox list.
    /// </summary>
    private void FillStaffStatusCheckboxList()
    {
        StaffStatusBL oStaffStatusBL = new StaffStatusBL();
        List<StaffStatusDetails> olstStaffStatusDetails = oStaffStatusBL.GetStaffStatusTypes();
        chkStaffStatus.Items.Clear();
        ListSource.FillCheckBoxList(olstStaffStatusDetails, chkStaffStatus, "StatusName", "StatusId");
        ViewState.Add(S_STAFF_STATUS_TYPE, olstStaffStatusDetails);
    }

    /// <summary>
    /// This method is used to save Staff Status Details
    /// </summary>
    private void SaveStaffStatusDetails()
    {   
        StaffStatusBL oStaffStatusBL = new StaffStatusBL(miSchoolId, miAcademicYearId);
        oStaffStatusBL.SaveStaffStatusDetails(GenerateXml(PopulateStatffStatusList()), miUserId);
    }

    /// <summary>
    /// This method is used to populate staffStatus list. 
    /// </summary>
    /// <returns></returns>
    private List<StaffStatusDetails> PopulateStatffStatusList()
    {
        StaffStatusDetails oStaffStatusDetails = null;
        List<StaffStatusDetails> lstStaffStatusDetails = new List<StaffStatusDetails>();
        int iRowId = 0;
        foreach (ListViewDataItem oListViewDataItem in lstvwStaffStatusType.Items)
        {            
            iRowId = Convert.ToInt32(oListViewDataItem.DisplayIndex);
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStaffStatusType.Items[iRowId];
            DropDownList cmbStaffStatusType = oCurrentItem.FindControl("cmbStaffStatusType") as DropDownList;
            oStaffStatusDetails = new StaffStatusDetails
            {
                UserId = Convert.ToInt32(lstvwStaffStatusType.DataKeys[iRowId]["UserId"]),
                DesignationId = Convert.ToInt32(lstvwStaffStatusType.DataKeys[iRowId]["DesignationId"]),
                StaffStatusDetailsId = Convert.ToInt32(lstvwStaffStatusType.DataKeys[iRowId]["StaffStatusDetailsId"]),
                StatusId = Convert.ToInt32(cmbStaffStatusType.SelectedValue)
            };
            lstStaffStatusDetails.Add(oStaffStatusDetails);
        }
        return lstStaffStatusDetails;
    }

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwStaffStatusType.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");
    }

    /// <summary>
    /// This method is used to set visibility.
    /// </summary>
    private void SetDefaultVisibility(bool abFlag)
    {
        trlistview.Visible = abFlag;
        btnSave.Enabled = abFlag;
        trPagerStaffStatus.Visible = abFlag;
        trNorecordFoundSearch.Visible = !abFlag;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose, btnSearch });
        btnClose.Attributes.Add("onclick", "window.close();");
        SetDefaultButton(btnSearch);
        cmbUserRoles.Focus();
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Other_User_Related));
    }

    /// <summary>
    /// This method is used to set sor direction.
    /// </summary>
    private void SetSortVariable()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    #endregion
}
