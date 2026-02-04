
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities;
using Utility;
using System.Web.UI.HtmlControls;
using System.Web;
using MasterEntities;

/// <summary>
/// This class is used to add,update & delete mailing groups from the screen. 
/// </summary>
public partial class MailingGroupPopup : SchoolBase
{
    #region -- CONSTANTS(s) --

    private const string S_SAVE_MESSAGE = "Group details added successfully!!!";
    private const string S_UPDATE_MESSAGE = "Group details updated successfully!!!";
    private const string S_DELETE_MESSAGE = "Group details deleted successfully!!!";
    private const string S_DELETE_USER_MESSAGE = "User deleted successfully!!!";
    private const string S_ADD_BUTTON = "Add";
    private const string S_UPDATE_BUTTON = "Update";
    private const string S_DELETED = Constants.S_ONE;
    private const string S_USER_DETAILS = "lstUsers";

    #endregion -- CONSTANTS(s) --

    #region -- MEMBER(s) --

    private MailingGroupBL moMailingGroupBL;
    private Constants.UserRoles moLoginUserRole;
    private string msUserRoles;

    #endregion -- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            base.AddSortImage(lstvwContacts, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display all the default details on the pageload.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            RegisterPostbackControls();
            if (!IsPostBack)
            {
                hidStdDivId.Value = Constants.S_ZERO;
                if (!QueryString["IsCc"].IsNull())
                {
                    hidIsCc.Value = "1";
                }
                SetUserAccess();
                Initialize();
                CreateDefaultGroups();
                FillGroupDetails();                              
                if (moLoginUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value.ToBool())
                {
                    FillUsers();
                    FillRoleDetails();
                }
                else
                {
                    trContacts.Visible = false;
                    trCreateGroup.Visible = false;
                    trUserRole.Visible = false;
                    tblLegend.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the item databound of groups. Here we deside the controls enability depend on the access of the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroup_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                HiddenField hidUsers = oCurrentItem.FindControl("hidUsers") as HiddenField;
                Label lblName = oCurrentItem.FindControl("lblName") as Label;
                CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                var bIsDefault = lstvwGroup.DataKeys[oCurrentItem.DataItemIndex]["IsDefault"].ToBool();
                var bIsAllDeactivated = lstvwGroup.DataKeys[oCurrentItem.DataItemIndex]["IsAllDeactivated"].ToBool();
                var tdEdit = oCurrentItem.FindControl("tdEdit") as HtmlTableCell;
                var tdDelete = oCurrentItem.FindControl("tdDelete") as HtmlTableCell;

                if (moLoginUserRole != Constants.UserRoles.Admin && !hidHasFullAccess.Value.ToBool())
                {
                    tdEdit.Visible = false;
                    tdDelete.Visible = false;
                }

                if (hidUsers != null)
                {
                    lblName.ToolTip = hidUsers.Value.Replace(",", ",<BR />");
                    lblName.CssClass = "class1";

                    if (IsPostBack)
                        ScriptManager.RegisterClientScriptBlock(lblName, typeof(Page), "showtooltip", "showtooltip()", true);
                }

                if (bIsAllDeactivated && !chkSelect.IsNull())
                    chkSelect.Enabled = false;

                if (bIsDefault && tdDelete.Visible && tdEdit.Visible)
                {
                    var imgEdit = oCurrentItem.FindControl("imgEdit") as ImageButton;
                    var imgDelete = oCurrentItem.FindControl("imgDelete") as ImageButton;
                    imgDelete.Visible = false;
                    imgEdit.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide the edit and delete button depending on access to the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroup_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwGroup.Items.Count > 0 && moLoginUserRole != Constants.UserRoles.Admin && !hidHasFullAccess.Value.ToBool())
            {
                var othEdit = lstvwGroup.FindControl("thEdit") as HtmlTableCell;
                var othDelete = lstvwGroup.FindControl("thDelete") as HtmlTableCell;
                othEdit.Visible = false;
                othDelete.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the command events like Edit,Delete of groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroup_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var iGroupId = e.CommandArgument;
                var oCurrentItem = e.Item as ListViewDataItem;
                hidGroupId.Value = iGroupId.ToString();
                Label lblName = oCurrentItem.FindControl("lblName") as Label;

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moMailingGroupBL.Delete(iGroupId.ToInt());
                    FillGroupDetails();
                    FillUsers();
                    ClearFields();
                    CheckUserRoles();
                    lblUpdateMessage.Text = S_DELETE_MESSAGE;
                    hidIsGroupDeleted.Value = S_DELETED;
                    ViewState[S_USER_DETAILS] = null;
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    txtGroupName.Text = lblName.Text;
                    btnAdd.Text = S_UPDATE_BUTTON;
                    FillGroupDetails(iGroupId.ToInt());
                    FillGroupUsers();
                    CheckUserRoles();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the group details for the current academic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sMailingGroupXML = GetXMLForMailingGroup();
            moMailingGroupBL.Insert(sMailingGroupXML);

            if (!hidGroupId.Value.IsNullOrEmpty() && hidGroupId.Value != Constants.S_ZERO)
                lblUpdateMessage.Text = S_UPDATE_MESSAGE;
            else
                lblUpdateMessage.Text = S_SAVE_MESSAGE;

            ClearFields();
            FillGroupDetails();
            FillUsers();
            FillRoleDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get the selected groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            string sGroupId = string.Empty;
            string sGroupName = string.Empty;
            for (int iRowCount = 0; iRowCount < lstvwGroup.Items.Count; iRowCount++)
            {
                ListViewDataItem oListViewDataItem = lstvwGroup.Items[iRowCount];
                CheckBox oCheckBox = oListViewDataItem.FindControl("chkSelect") as CheckBox;
                Label lblName = oListViewDataItem.FindControl("lblName") as Label;
                if (oCheckBox != null && oCheckBox.Checked)
                {
                    string strGroupId = lstvwGroup.DataKeys[iRowCount]["GroupId"].ToString();
                    sGroupId = sGroupId + strGroupId + ",";
                    sGroupName = sGroupName + lblName.Text + ", ";
                }
            }

            if (!sGroupId.IsNullOrEmpty())
            {
                sGroupId = sGroupId.Remove(sGroupId.LastIndexOf(','), 1);
                sGroupName = sGroupName.Remove(sGroupName.LastIndexOf(','), 1);
                sGroupName = sGroupName.Replace("'", "\\'");
            }

            if (QueryString["IsCc"].IsNull())
                Response.Write(String.Format("<Script  type='text/javascript'>window.opener.SetToUserId('" + sGroupName + "','" + sGroupId + "','G');</Script>"));
            else
                Response.Write(String.Format("<Script  type='text/javascript'>window.opener.SetCcUserId('" + sGroupName + "','" + sGroupId + "','G');</Script>"));

            Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check if any group is deleted. If yes then we will refresh the base screen contacts.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidIsGroupDeleted.Value == S_DELETED)
                btnOk_Click(sender, e);
            else
                Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used hanfle select paging list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetSelectedUsers();
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwContacts);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort order in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwContacts_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            base.RevertSortOrder(hidSortDirection);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to fill page dropdown if item count is more than 20. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwContacts_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwContacts.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwContacts, DtPgCount);
            else
                DtPgCount.Visible = false;
            SelectUsers();
            CheckSelectAll();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to handle the command event of sorting the teachers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwContacts_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.EmptyItem)
            {
                if (e.CommandName == Constants.S_COMMAND_SORT)
                {
                    ClearFields();
                    lstvwContacts.DataSourceID = lstvwDSobj.ID;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the item databound of users. Here we deside the controls enability depend on Is Deleted user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwContacts_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                var bIsDeactivated = lstvwContacts.DataKeys[oCurrentItem.DisplayIndex]["IsDeactivated"].ToBool();
                var chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                var trHtmlTableRow = oCurrentItem.FindControl("trItem") as HtmlTableRow;

                if (bIsDeactivated && !trHtmlTableRow.IsNull())
                {
                    trHtmlTableRow.Style.Add("background-color", "Gainsboro");
                    chkSelect.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the item databound of users. Here we deside the controls enability depend on Is Deleted user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstGroupUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                var bIsDeactivated = lstGroupUsers.DataKeys[oCurrentItem.DataItemIndex]["IsDeactivated"].ToBool();
                var trHtmlTableRow = oCurrentItem.FindControl("trItem") as HtmlTableRow;

                if (bIsDeactivated && !trHtmlTableRow.IsNull())
                    trHtmlTableRow.Style.Add("background-color", "Gainsboro");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called on changing role selection. We will fill users according to the role selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetSelectedUsers();

            if (cmbRoles.SelectedValue == "3")
            {
                trClass.Visible = true;
                FillClassComboBox();
            }
            else
                trClass.Visible = false;

            FillUsers();
            CheckSelectAll();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called on changing class selection. We will fill students(Users) according to the class selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStdDivId.Value = cmbClass.SelectedValue;
            FillUsers();
            CheckSelectAll();
            GetSelectedUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the command event for the listview. Here we delete the perticular user from a group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstGroupUsers_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var iUserId = e.CommandArgument;
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    if (lstGroupUsers.Items.Count == Constants.I_ONE)
                    {
                        lblError.Visible = true;
                        lblError.Text = "At least one user should be present in group.";
                        return;
                    }
                    else
                    {
                        moMailingGroupBL.DeleteMailingGroupUser(hidGroupId.Value.ToInt(), iUserId.ToInt());
                        lblUpdateMessage.Text = S_DELETE_USER_MESSAGE;
                        FillGroupUsers();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel the edited operation & used to clear the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            lstGroupUsers.DataSource = null;
            lstGroupUsers.DataBind();
            GridViewScrollContainer.Visible = false;
            ViewState[S_USER_DETAILS] = null;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --
    /// <summary>
    /// Following method is used to fill the roles checkbox list. As this functionality is not available to students 
    /// we will exclude that role from the list.
    /// </summary>
    private void FillRoleDetails()
    {
        List<UserRoles> lstUserRoles = MasterDataCollectionBL.GetUserRolesForMailingList();
        chkListRoles.DataSource = lstUserRoles;
        chkListRoles.DataBind();
        //lstUserRoles = lstUserRoles.Where(a => a.User_Role_Id != Constants.UserRoles.Student.ToInt()).ToList();
        ListSource.FillDropDownList(lstUserRoles, cmbRoles, "User_Role_Name", "User_Role_Id", string.Empty);

        trClass.Visible = false;
    }

    /// <summary>
    /// This method is used to create the default groups for the school.
    /// </summary>
    private void CreateDefaultGroups()
    {
        moMailingGroupBL.CreateDefaultGroups();
    }

    /// <summary>
    /// This method is used to fill the group users for selected group.
    /// </summary>
    private void FillGroupUsers()
    {
        List<UserInfo> lstUserInfo = moMailingGroupBL.GetGroupUsers(hidGroupId.Value.ToInt());
        lstGroupUsers.DataSource = lstUserInfo;
        lstGroupUsers.DataBind();

        if (lstGroupUsers.Items.Count > 0)
        {
            hidEditedUserCount.Value = lstGroupUsers.Items.Count.ToString();
            GridViewScrollContainer.Visible = true;
        }
        else
        {
            hidEditedUserCount.Value = Constants.S_ZERO;
            GridViewScrollContainer.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to initialize the member variables on pageload.
    /// </summary>
    private void SetUserAccess()
    {
        if (!Request.QueryString.IsNull() && QueryString["Mode"] == "SMS")
            hidHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.SMSCenter).ToString();
        else
            hidHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.MessageCenter).ToString();
    }

    /// <summary>
    /// This method is used to register postback controls.
    /// </summary>
    private void RegisterPostbackControls()
    {
        moMailingGroupBL = new MailingGroupBL(miSchoolId, miAcademicYearId, miUserId);
        moLoginUserRole = (Constants.UserRoles)Enum.Parse(typeof(Constants.UserRoles), Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToString());

        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnClose);
        scriptManager.RegisterPostBackControl(this.btnOk);
        btnOk.Attributes.Add("Onclick", "if(!(CheckValidOK())){return false;}");
    }

    /// <summary>
    /// This method is used to check the user roles on editing  any group.
    /// </summary>
    private void CheckUserRoles()
    {
        chkListRoles.ClearSelection();
        string sUserRoles = msUserRoles;
        string[] sArrRoles = sUserRoles.Split(',');
        foreach (string role in sArrRoles)
        {
            foreach (ListItem item in chkListRoles.Items)
            {
                if (item.Value == role.Trim())
                    item.Selected = true;
            }
        }
    }

    /// <summary>
    /// This method is used to collect all the user details for selected group.
    /// </summary>
    private void GetSelectedUsers()
    {
        List<int> lstUsers = (from currentItem in lstvwContacts.Items
                              let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                              where chkSelect != null && chkSelect.Checked
                              select lstvwContacts.DataKeys[currentItem.DisplayIndex]["UserId"].ToInt()
                                                    ).ToList();

        if (!ViewState[S_USER_DETAILS].IsNull())
        {
            List<int> lstUserMailingLst = new List<int>();
            lstUserMailingLst = ViewState[S_USER_DETAILS] as List<int>;
            lstUserMailingLst.AddRange(lstUsers.Except(lstUserMailingLst));
            ViewState[S_USER_DETAILS] = lstUserMailingLst;
        }
        else
            ViewState[S_USER_DETAILS] = lstUsers;
    }

    /// <summary>
    /// This function is used to select all checkbox after page change & role change.
    /// </summary>
    private void CheckSelectAll()
    {
        List<int> lstUsers = (from currentItem in lstvwContacts.Items
                              let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                              where chkSelect != null && chkSelect.Checked
                              select lstvwContacts.DataKeys[currentItem.DisplayIndex]["UserId"].ToInt()
                                                    ).ToList();

        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwContacts.FindControl("trHeader");
        CheckBox oCheckBox = null;
        if (oHtmlTableRow != null)
            oCheckBox = (CheckBox)oHtmlTableRow.FindControl("chkSelectAll");
        if (oCheckBox != null)
        {
            if (lstUsers.Count == lstvwContacts.Items.Count && lstUsers.Count > 0)
                oCheckBox.Checked = true;
            else
                oCheckBox.Checked = false;
        }
    }

    /// <summary>
    /// This method is used to return the XML to insert all the details.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForMailingGroup()
    {

        List<UserRoles> lstUserRoles = new List<UserRoles>();
        foreach (ListItem item in chkListRoles.Items)
        {
            UserRoles oUserRoles = new UserRoles();
            if (item.Selected)
            {
                oUserRoles.User_Role_Id = item.Value.ToInt();
                lstUserRoles.Add(oUserRoles);
            }
        }

        GetSelectedUsers();

        List<int> lstUsers = ViewState[S_USER_DETAILS] as List<int>;
        string sUsers = string.Join(",", lstUsers.Select(n => n.ToString()).ToArray());
        ViewState[S_USER_DETAILS] = null;

        MailingGroup oMailingGroup = new MailingGroup
        {
            GroupId = hidGroupId.Value.ToInt(),
            Name = txtGroupName.Text,
            Users = sUsers,
            lstUserRoles = lstUserRoles
        };

        return base.GenerateXml(oMailingGroup);
    }

    /// <summary>
    /// This is a private method and is used to clear the fields.
    /// </summary>
    private void ClearFields()
    {
        txtGroupName.Text = string.Empty;
        hidGroupId.Value = Constants.S_ZERO;
        btnAdd.Text = S_ADD_BUTTON;
        GridViewScrollContainer.Visible = false;
    }

    /// <summary>
    /// This method is used to fill all the groups into the listview.
    /// </summary>
    /// <param name="aiGroupId"></param>
    private void FillGroupDetails(int aiGroupId = 0)
    {
        string asRoleIds;
        List<MailingGroup> lstMailingGroup = null;
        if (moLoginUserRole == Constants.UserRoles.Admin || (hidHasFullAccess.Value.ToBool()))
            lstMailingGroup = moMailingGroupBL.GetAll(0, out asRoleIds, aiGroupId);
        else
            lstMailingGroup = moMailingGroupBL.GetAll(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt(), out asRoleIds, aiGroupId);

        lstvwGroup.DataSource = lstMailingGroup;
        msUserRoles = asRoleIds;

        lstvwGroup.DataBind();
        if (lstvwGroup.Items.Count == 0)
        {
            btnOk.Enabled = false;
            tblLegend.Visible = false;
        }
        else
            btnOk.Enabled = true;
    }

    /// <summary>
    /// This method is used to fill the teachers with their associating status in the listview.
    /// </summary>
    /// <param name="aiGroupId"></param>
    private void FillUsers()
    {
        if (lstvwContacts.Items.Count > 0)
        {
            DataPager pager = lstvwContacts.FindControl("DtPgDropDown") as DataPager;
            pager.SetPageProperties(0, pager.PageSize, true);
        }
    }

    /// <summary>
    /// This function is used to check the selected users after the page change and role change.
    /// </summary>
    private void SelectUsers()
    {
        if (!ViewState[S_USER_DETAILS].IsNull())
        {
            List<int> lstUsers = ViewState[S_USER_DETAILS] as List<int>;
            foreach (ListViewDataItem oListViewDataItem in lstvwContacts.Items)
            {

                CheckBox chkSelect = oListViewDataItem.FindControl("chkSelect") as CheckBox;
                int iUserId = lstvwContacts.DataKeys[oListViewDataItem.DisplayIndex]["UserId"].ToInt();
                if (lstUsers != null)
                {
                    if (lstUsers.Contains(iUserId) && chkSelect != null)
                        chkSelect.Checked = true;
                }
            }
        }
    }

    /// <summary>
    /// This function is used to initialize controls to their default values.
    /// </summary>
    private void Initialize()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnOk, btnAdd, btnCancel, btnClose });
        base.SetDefaultButton(btnClose);
        hidSortDirection.Value = Constants.S_ASCENDING;
        btnAdd.Text = S_ADD_BUTTON;
        if (lstvwGroup.Items.Count == 0)
            btnOk.Enabled = false;
        else
            btnOk.Enabled = true;
    }

    /// <summary>
    /// This method is used to get the Standard list.
    /// </summary>
    //private void GetStandardDivisionList()
    //{
    //    StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
    //    List<StandardDivisionMaster> lstStandardDivisionMaster = oStandardDivisionCollectionBL.GetStandardDivisionList();
    //    Session.Add("StandardDivisionList", lstStandardDivisionMaster);
    //}

    private void FillClassComboBox()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oClass = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ControlUtility.FillDropDownList(oClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
        cmbClass.SelectedIndex = Constants.I_ONE;
        hidStdDivId.Value = cmbClass.SelectedValue;
    }

    /// <summary>
    /// This method is used to search Staff.
    /// </summary>
    /// <param name="asError"></param>
    protected void btnSearch_Click(object sender, EventArgs e)  //
    {
        try
        {
            FillUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion -- PRIVATE METHOD(s) --    
}