/* File Name :- ConfigureMenu.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 19-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to manipulate internal as well as external menu items.
*/

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;
using SchoolEntities.Admin;
using System.Linq;


public partial class ConfigureMenu : SchoolBase
{
    #region Event(s)
    List<ConfigMenuAssociatedClasses> molstConfigMenuAssociatedClasses = new List<ConfigMenuAssociatedClasses>();

    /// <summary>
    /// This event is used to fill parent combobox and menu controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Culture = "en";
            if (!Page.IsPostBack)
            {
				base.SetDocType();
                FCKMenuView.BasePath = "../ckeditor/";
                FCKNewMenu.BasePath = "../ckeditor/";
                FCKMenuView.ResizeEnabled = false;
                FCKNewMenu.ResizeEnabled = false;
                FillApplicableRoles();
                FillParentMenuCombobox();
                FillMenuControl(string.Empty);
                SetQueryString();
                SetJavascriptAttributes();
                ClearRoles();
             }
            SetDefaultValues();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This methodis used to display content of selected menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Menu_Configure_MenuItemClick(object sender, MenuEventArgs e)
    {        
        try
        {
            FillMenuItemContents();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change text of external menu item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void trvExternal_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
	    try
	    {
		    int iMenuId = Convert.ToInt32(e.Node.Value);
		    if (iMenuId == 12)
			    e.Node.Text = "Active : " + e.Node.Text;
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
	    }
	    catch (Exception ex)
	    {
		    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	    }
    }

    /// <summary>
    /// This event is used ti fill parent menu combobox and display content of selected menu item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Menu_Configure_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            TreeNode oSelectedNode;
            if (((System.Web.UI.WebControls.HierarchicalDataBoundControl)(sender)).ID == Menu_Configure.ID)
                oSelectedNode = trvExternal.SelectedNode;
            else
                oSelectedNode = Menu_Configure.SelectedNode;

            if (oSelectedNode != null)
                oSelectedNode.Selected = false;
              
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
            FillParentMenuCombobox();
            FillMenuItemContents();
            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
      
    /// <summary>
    /// This event is used to save detals of newly added menu item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ConfigureMenuBL oConfigureMenuBL = InitializeConfigureMenuBL(miSchoolId);
            string sMessage = oConfigureMenuBL.IsMenuNameAlreadyExists();
            if (sMessage == "False")
            {
                string sEndDate;

                sEndDate = Convert.ToString(txtEndDate.Text.Trim());
                int iReturnId = oConfigureMenuBL.InsertConfigureMenu(sEndDate, miAcademicYearId);
                lblMsg.Text = "Menu item saved successfully.";
                lblMsg.CssClass = "ClsHilightBGB";
                lblMsg.Visible = true;
                FillParentMenuCombobox();
                FillMenuControl(Convert.ToString(iReturnId));
                txtMenuName.Text = string.Empty;
                FCKNewMenu.Text = HttpUtility.HtmlDecode("<p><BR><p>");
                NmBoxPriority.Text = string.Empty;
                txtShowTopAdd.Text = string.Empty;
                chkAllsubMenuAdd.Text=string.Empty;
              
                ConfigureCollectionMenuBL oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
                Session[Constants.S_SESSION_SCHOOL_MENUS] = oConfigureCollectionMenuBL.FetchAllActiveInternalMenus(miSchoolId, moUserRole.ToInt());
                DisplayAddMode(false);
                Menu_Configure.SelectedNode.Value = iReturnId.ToString();
                MasterPage oMaster = (MasterPage)this.Master;
                oMaster.FillMenuControl();
                SetQueryString();
                FillStandardChkLstBox(iReturnId);
            }
            else
            {
                    DisplayAddMode(true);
                    lblErrorMsg.Text = sMessage;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to update the ontents of selected menu item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnUpdate_Click(object sender, EventArgs e)
    {        
        try
        {
            string sUpdateEndDate;
            if (txtDateUpdate.Text != Constants.S_EMPTY_STRING)
                sUpdateEndDate = txtDateUpdate.Text;
            else
                sUpdateEndDate = null;
          
            int iMenuId = 0;
            if (Menu_Configure.SelectedValue != string.Empty)
                iMenuId = Convert.ToInt32(Menu_Configure.SelectedValue);
            else
                iMenuId = Convert.ToInt32(trvExternal.SelectedValue);

            int iParentMenuId = cmbSubMenu.SelectedValue.ToInt() != 0 && !string.IsNullOrEmpty(cmbSubMenu.SelectedValue) ? cmbSubMenu.SelectedValue.ToInt() : (!string.IsNullOrEmpty(cmbParentMenu.SelectedValue) ? cmbParentMenu.SelectedValue.ToInt() : 0);

            ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
            int iMenuIdTest = oConfigureMenuBL.GetMenuIdByMenuName(txtMenuNameUpdate.Text.Trim(), iParentMenuId, miSchoolId);
            if (iMenuId == iMenuIdTest || iMenuIdTest == 0)
            {
                oConfigureMenuBL = UpdateMenuItem(oConfigureMenuBL, iMenuId, sUpdateEndDate);
                lblMsg.Text = "Menu item updated successfully.";
                lblMsg.CssClass = "ClsHilightBGB";
                lblMsg.Visible = true;
                FillParentMenuCombobox();
                FillMenuControl(Convert.ToString(iMenuId));
                ConfigureCollectionMenuBL oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
                Session[Constants.S_SESSION_SCHOOL_MENUS] = oConfigureCollectionMenuBL.FetchAllActiveInternalMenus(miSchoolId,moUserRole.ToInt());
				MasterPage oMaster = (MasterPage)this.Master;
				oMaster.FillMenuControl();
                DivAddNewMenu.Visible = false;
                DivViewMenues.Visible = true;
            }
            else
            {
                lblMsg.Text = "Menu item Name already exists.";
                lblMsg.CssClass = "LblErrorMsg";
                lblMsg.Visible = true;
                DivAddNewMenu.Visible = false;
                DivViewMenues.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add new menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
      
            NmBoxPriority.Text = SetDefaultPriority();
            chkAllsubMenuAdd.Checked = false;
            optAllAdd.Checked = true;
            HideControls();
            ClearControls();
            txtMenuName.Focus();
            ClearRoles();
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
            FillStandardChkLstBox(0);
            trAssociatedClasses.Style.Add("display","none");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to delete the selected menu Item.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int iMenuId = 0;

            if (Menu_Configure.SelectedValue != null && Menu_Configure.SelectedValue != string.Empty)
                iMenuId = Convert.ToInt32(Menu_Configure.SelectedValue);
            else
                iMenuId = Convert.ToInt32(trvExternal.SelectedValue);

            ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
            oConfigureMenuBL.ConfigureMenuId = iMenuId;
            oConfigureMenuBL.UpdatedById = miUserId;
            oConfigureMenuBL.DeleteConfigureMenu();
            lblMsg.Text = "Menu item deleted successfully.";
            lblMsg.CssClass = "ClsHilightBGB";
            lblMsg.Visible = true;
            FillParentMenuCombobox();
            FillMenuControl(string.Empty);
            SetQueryString();
            ConfigureCollectionMenuBL oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
            Session[Constants.S_SESSION_SCHOOL_MENUS] = oConfigureCollectionMenuBL.FetchAllActiveInternalMenus(miSchoolId, moUserRole.ToInt());
			MasterPage oMaster = (MasterPage)this.Master;
			oMaster.FillMenuControl();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method is used to display view mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            DisplayAddMode(false);
            txtMenuNameUpdate.Focus();
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Visible = false;
            valSumAdd.Visible = false;
            valsumEdit.Visible = true;
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to configuration menu screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgBtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration).ToString());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkOnPopUp_CheckedChanged(object sender, EventArgs e)
    {
	    try
	    {
		    if (chkOnPopUp.Checked)
		    {
			    chkApplicable.Checked = false;
			    chkApplicable.Enabled = false;
		    }
		    else
		    {
			    chkApplicable.Enabled = true;
			    if (Convert.ToChar(hidIsExternal.Value) == Constants.C_YES)
				    chkApplicable.Checked = true;
		    }
		    if (Convert.ToChar(hidIsChildMenu.Value) == Constants.C_YES)
			    chkApplicable.Enabled = false;
            
	    }
	    catch (Exception ex)
	    {
		    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	    }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbParentMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        if (cmbParentMenu != null && chkApplicable != null) {
            if (cmbParentMenu.SelectedValue == "0") {
                chkApplicable.Enabled = false;
            }
            else {
                chkApplicable.Enabled = true;
                chkApplicable.Checked = false;
            }
           
        }
        var oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
        DataTable oDT = oConfigureCollectionMenuBL.GetAllSubMenus(cmbParentMenu.SelectedValue.ToInt(), miSchoolId);
        cmbSubMenu.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);
       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbParentMenuAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbParentMenuAdd != null && chkApplicableAdd != null){
            if (cmbParentMenuAdd.SelectedValue == "0"){
                chkApplicableAdd.Enabled = false;
                trAddShowSubMenu.Visible = true;
            }
            else{
                chkApplicableAdd.Enabled = true;
                chkApplicableAdd.Checked = false;
                trAddShowSubMenu.Visible = false;
            }
        }

        
        FillMenuItemContents();
        var oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
        DataTable oDT = oConfigureCollectionMenuBL.GetAllSubMenus(cmbParentMenuAdd.SelectedValue.ToInt(), miSchoolId);
        cmbSubMenuAdd.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);   
    }

    protected void lstvwStandardDivisions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;            
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkStandard = oCurrentItem.FindControl("chkStandard") as CheckBox;
                CheckBoxList chkStandardDivLst = oCurrentItem.FindControl("chkStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwStandardDivisions.DataKeys[iRowId]["StandardId"].ToInt();

                var oDivision = molstConfigMenuAssociatedClasses.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.StandardwiseDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();
                chkStandard.Attributes.Add("onclick", "CheckAllAdd(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheckAdd('" + chkStandard + "','" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwUpdateStandardDivision_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            ConfigMenuAssociatedClasses oConfigMenuAssociatedClasses = oCurrentItem.DataItem as ConfigMenuAssociatedClasses;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkStandard = oCurrentItem.FindControl("chkStandard") as CheckBox;
                CheckBoxList chkStandardDivLst = oCurrentItem.FindControl("chkStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwUpdateStandardDivision.DataKeys[iRowId]["StandardId"].ToInt();

                var oDivision = molstConfigMenuAssociatedClasses.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.StandardwiseDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();
                var AssociatedDivisions = molstConfigMenuAssociatedClasses
                    .Where(sd => sd.StandardId == iStandardId && sd.IsRecordSaved == Constants.I_ONE.ToBool())
                    .Select(sd => sd.StandardwiseDivisionId)
                    .ToList();
               foreach (ListItem item in chkStandardDivLst.Items)
                {
                    item.Selected = AssociatedDivisions.Contains(item.Value.ToInt());
                } 
                chkStandard.Attributes.Add("onclick", "CheckAllUpdate(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheckUpdate('" + chkStandard + "','" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    
    #endregion//Event(s)

    #region Method(s)

    /// <summary>
    /// This method is used to hide controls.
    /// </summary>
    private void HideControls()
    {
        DivAddNewMenu.Visible = true;
        DivViewMenues.Visible = false;
        chkApplicableAdd.Checked = false;
        chkAddIsActive.Checked = false;
        lblErrorMsg.Visible = false;
        lblMsg.Visible = false;
        valSumAdd.Visible = true;
        valsumEdit.Visible = false;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnPreview, imgBtnDelete, imgbtnCancel, imgbtnSave, imgbtnAdd, imgbtnUpdate });
        
        imgBtnDelete.Attributes.Add("OnClick", "return window.confirm('Are you sure you want to delete the menu?');");
        btnPreview.Attributes.Add("OnClick", "ShowPreview();return false;");
        optAllAdd.Attributes.Add("onclick", "ShowAllCountAdd()");
        optTopAdd.Attributes.Add("onclick", "ShowAllCountAdd()");
        optAllUpdate.Attributes.Add("onclick", "ShowAllCountUpdate()");
        optTopUpdate.Attributes.Add("onclick", "ShowAllCountUpdate()");
        chkAddListRoles.Attributes.Add("onclick", "ShowAssociatedClassAdd()");
        chkListRoles.Attributes.Add("onclick", "ShowAssociatedClassUpdate()");
        imgbtnAdd.Attributes.Add("onclick", "ShowHideClasses(false);");        
    }

    /// <summary>
    /// This method is used to update menu item.
    /// </summary>
    /// <param name="aoConfigureMenuBL"></param>
    /// <param name="aiMenuId"></param>
    /// <returns></returns>
    private ConfigureMenuBL UpdateMenuItem(ConfigureMenuBL aoConfigureMenuBL, int aiMenuId, string sUpdateEndDate)
    {
        aoConfigureMenuBL.ConfigureMenuContent = HttpUtility.HtmlEncode(FCKMenuView.Text);
        aoConfigureMenuBL.ConfigureMenuName = txtMenuNameUpdate.Text;
        aoConfigureMenuBL.ConfigureMenuId = aiMenuId;
        aoConfigureMenuBL.Priority = Convert.ToInt32(NmcBxPriorityUpdate.Text);

        if (optTopUpdate.Checked)
            aoConfigureMenuBL.SubMenuCount = Convert.ToInt32(txtShowTopUpdate.Text);
        else
            aoConfigureMenuBL.SubMenuCount = 0;

             if (chkAllsubMenu.Checked)
            aoConfigureMenuBL.ApplyAllSubMenu = true;
        else
            aoConfigureMenuBL.ApplyAllSubMenu = false;
        aoConfigureMenuBL.SchoolId = miSchoolId;
        aoConfigureMenuBL.ParentMenuId = cmbSubMenu.SelectedValue.ToInt() != 0 && !string.IsNullOrEmpty(cmbSubMenu.SelectedValue) ? Convert.ToInt32(cmbSubMenu.SelectedValue) : Convert.ToInt32(cmbParentMenu.SelectedValue);
        if (txtDateUpdate.Text != Constants.S_EMPTY_STRING)
            aoConfigureMenuBL.End_Date = txtDateUpdate.Text;
        else
            aoConfigureMenuBL.End_Date =null;
        if (chkIsDefault.Checked)
            aoConfigureMenuBL.IsDefault = Constants.C_YES;
        else
            aoConfigureMenuBL.IsDefault = Constants.C_NO;

        if (cmbParentMenu.SelectedIndex != 0)
            aoConfigureMenuBL.IsExternal = CheckIfParentMenuIsApplicbleToAllUsers(Convert.ToInt32(cmbParentMenu.SelectedValue));
        else
        {
            if (chkApplicable.Checked)
                aoConfigureMenuBL.IsExternal = Constants.C_YES;
            else
                aoConfigureMenuBL.IsExternal = Constants.C_NO;
        }

        if (chkIsActive.Checked)
            aoConfigureMenuBL.IsActive = Constants.C_YES;
        else
            aoConfigureMenuBL.IsActive = Constants.C_NO;

        if (chkOnPopUp.Checked)
            aoConfigureMenuBL.IsOnPopUp = Constants.C_YES;
        else
            aoConfigureMenuBL.IsOnPopUp = Constants.C_NO;

        aoConfigureMenuBL.UpdatedById = miUserId;

        aoConfigureMenuBL.UserRoleIds = string.Join(",", GetSelectedRolesforUpdate());
        aoConfigureMenuBL.AssoiciatedStandards = GetSelectedStandardDivisionsForUpdate();
       
        if (Menu_Configure.SelectedValue != string.Empty)
            aoConfigureMenuBL.UpdateConfigureMenu(true, sUpdateEndDate,miAcademicYearId);
        else
            aoConfigureMenuBL.UpdateConfigureMenu(false, sUpdateEndDate, miAcademicYearId);

        if (Menu_Configure.SelectedNode != null || Menu_Configure.SelectedNode.Parent != null || Menu_Configure.SelectedNode.Parent.Parent != null)
            aoConfigureMenuBL.UpdateChildNodes(sUpdateEndDate);
   
        return aoConfigureMenuBL;
    }

   /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valsumEdit.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumAdd.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblErrorMsg.Text = string.Empty;
        lblMsg.Text = string.Empty;
        lblMsg.Visible = false;
    }

    /// <summary>
    /// This method is used to fill roles into checkBoxList.
    /// </summary>
    private void FillApplicableRoles()
    {
        NoticeBoardBL oNoticeBoardBL = InitializeNoticeBoardBL();
        DataTable oDTRole = oNoticeBoardBL.RetriveRolesFromUserRoleMaster();

        if (!Settings.EnableOtherStaffLogin)
        {
            DataRow[] dr = oDTRole.Select("User_Role_Id=" + Constants.UserRoles.OtherStaff.ToInt());
            if (dr.Length > 0)
            {
                dr[0].Delete();
                oDTRole.AcceptChanges();
            }
        }

        chkListRoles.DataSource = chkAddListRoles.DataSource = oDTRole;
        chkListRoles.DataTextField = chkAddListRoles.DataTextField = "User_Role_Name";
        chkListRoles.DataValueField  = chkAddListRoles.DataValueField = "User_Role_Id";
        chkListRoles.DataBind();
        chkAddListRoles.DataBind();
    }

    /// <summary>
    /// This method is used to Initialized Notice Board BL class property.
    /// </summary>
    private NoticeBoardBL InitializeNoticeBoardBL()
    {
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        oNoticeBoardBL.SchoolId = miSchoolId;
        oNoticeBoardBL.AcademicYearId = miAcademicYearId;
        oNoticeBoardBL.InsertedById = miUserId;
        oNoticeBoardBL.UpdatedById = miUserId;
        oNoticeBoardBL.UpdatedDate = Convert.ToDateTime(System.DateTime.Today);

        return oNoticeBoardBL;
    }

    /// <summary>
    /// This method is used to clear controls.
    /// </summary>
    private void ClearControls()
    {
        txtMenuName.Text = string.Empty;
        FCKNewMenu.Text= string.Empty;
        lblErrorMsg.Text = string.Empty;
        lblMsg.Text = string.Empty;
        cmbParentMenuAdd.SelectedIndex = 0;
        ClearRoles();
        
    }

    /// <summary>
    /// This method is used to display add mode controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisplayAddMode(bool abAction)
    {
        DivAddNewMenu.Visible = abAction;
        DivViewMenues.Visible = !abAction;        
    }  

    /// <summary>
    /// This method is used to fill parent menu items comboboxes of add and view mode.
    /// </summary>
    private void FillParentMenuCombobox()
    {
        int iMenuId = 0, iParentMenuId = 0;
        if (Menu_Configure.SelectedValue != string.Empty)
            iMenuId = Menu_Configure.SelectedValue.ToInt();
        else if (trvExternal.SelectedValue != string.Empty)
            iMenuId = trvExternal.SelectedValue.ToInt();

        if (Menu_Configure.SelectedValue != string.Empty)
        {
            if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                iParentMenuId = Menu_Configure.SelectedValue.ToInt();
            else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                iParentMenuId = Menu_Configure.SelectedNode.Parent != null ? Menu_Configure.SelectedNode.Parent.Value.ToInt() : Menu_Configure.SelectedValue.ToInt();
            else if (CheckIfCurrentMenuIsSecondChildMenu())
                iParentMenuId = Menu_Configure.SelectedNode.Parent != null && Menu_Configure.SelectedNode.Parent.Parent != null ? Menu_Configure.SelectedNode.Parent.Parent.Value.ToInt() : Menu_Configure.SelectedValue.ToInt();
        }
        else if (trvExternal.SelectedValue != string.Empty)
        {
            if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                iParentMenuId = trvExternal.SelectedValue.ToInt();
            else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                iParentMenuId = trvExternal.SelectedNode.Parent != null ? trvExternal.SelectedNode.Parent.Value.ToInt() : trvExternal.SelectedValue.ToInt();
            else if (CheckIfCurrentMenuIsSecondChildMenu())
                iParentMenuId = trvExternal.SelectedNode.Parent != null && trvExternal.SelectedNode.Parent.Parent != null ? trvExternal.SelectedNode.Parent.Parent.ToInt() : trvExternal.SelectedValue.ToInt();
        }
        else
            iParentMenuId = hidParentMenuAddId.Value.ToInt();

        var oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
        DataTable oDT = oConfigureCollectionMenuBL.GetAllParentMenus(iMenuId, miSchoolId);
        cmbParentMenu.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);

        oDT = oConfigureCollectionMenuBL.GetAllParentMenus(0, miSchoolId);
        cmbParentMenuAdd.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);

        oDT = oConfigureCollectionMenuBL.GetAllSubMenus(iParentMenuId, miSchoolId);
        cmbSubMenu.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);

        oDT = oConfigureCollectionMenuBL.GetAllSubMenus(iParentMenuId, miSchoolId);
        cmbSubMenuAdd.Bind(oDT, "ConfigureMenuId", "ConfigureMenuName", Constants.S_SELECT);

        Session["S_PARENT_MENUS"] = oDT;

    }

    /// <summary>
    /// This method is used to check whether current menu items has child menu items or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuHasChildMenus()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count > 0)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count > 0)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether current menu item is Parent menu item or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsParentMenu()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count > 0 && Menu_Configure.SelectedNode.Parent == null)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count > 0 && trvExternal.SelectedNode.Parent == null)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether current menu item is Sub menu item or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsSubMenu()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count > 0 && Menu_Configure.SelectedNode.Parent != null)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count > 0 && trvExternal.SelectedNode.Parent != null)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether current menu item is second level child menu item or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsFirstChildMenu()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count == 0 && Menu_Configure.SelectedNode.Parent != null && Menu_Configure.SelectedNode.Parent.Parent == null)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count == 0 && trvExternal.SelectedNode.Parent != null && trvExternal.SelectedNode.Parent.Parent == null)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether current menu item is third level child menu item or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsSecondChildMenu()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count == 0 && Menu_Configure.SelectedNode.Parent != null && Menu_Configure.SelectedNode.Parent.Parent != null)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count == 0 && trvExternal.SelectedNode.Parent != null && trvExternal.SelectedNode.Parent.Parent != null)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether the current menu item does not have any Parent menu as well as any Child menu or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsTopMenu()
    {
        bool bResult = false;
        if (Menu_Configure.SelectedNode != null)
        {
            if (Menu_Configure.SelectedNode.ChildNodes.Count == 0 && Menu_Configure.SelectedNode.Parent == null)
                bResult = true;
        }
        else if (trvExternal.SelectedNode != null)
        {
            if (trvExternal.SelectedNode.ChildNodes.Count == 0 && trvExternal.SelectedNode.Parent == null)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to check whether current menu items is child menu of another menu item.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCurrentMenuIsChildMenu()
    {
        int iMenuId = Convert.ToInt32(trvExternal.SelectedValue);
        return ConfigureMenuBL.IsChildMenu(miSchoolId, iMenuId);
    }

    /// <summary>
    /// This method is used to fill internal as well as extrnal treeview with menu items.
    /// </summary>
    /// <param name="asSelectedValue"></param>
    private void FillMenuControl(string asSelectedValue)
    {
        string sSelectedMenuItemValue = asSelectedValue;
        ConfigureCollectionMenuBL oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
        DataTable oDataTable = oConfigureCollectionMenuBL.FetchAllInternalMenus(miSchoolId, string.Empty);
        AddMenuItems(ref Menu_Configure, oDataTable, sSelectedMenuItemValue, true);
        if (Menu_Configure.Nodes.Count == 0)
        {
            pnlMenuConfigure.Visible = false;
            trInternalMenus.Visible = false;
            pnlExternal.Height = Unit.Pixel(380);
        }
        else
        {
            pnlMenuConfigure.Visible = true;
            trInternalMenus.Visible = true;
            pnlMenuConfigure.Height = Unit.Pixel(100);
            pnlExternal.Height = Unit.Pixel(100);
        }

        DataTable oExternalDataTable = oConfigureCollectionMenuBL.FetchAllExternalMenus(miSchoolId);
        AddMenuItems(ref trvExternal, oExternalDataTable, sSelectedMenuItemValue, false);
        if (trvExternal.Nodes.Count == 0)
        {
            pnlExternal.Visible = false;
            trMenuSeparator.Visible=false;
            trExternalMenus.Visible = false;
            pnlMenuConfigure.Height = Unit.Pixel(410);
        }
        else
        {
            if (Menu_Configure.Nodes.Count != 0)
                trMenuSeparator.Visible = true;
            else
                trMenuSeparator.Visible = false;
            trExternalMenus.Visible = true;
            
            pnlExternal.Visible = true;
            pnlMenuConfigure.Height = Unit.Pixel(185);
            pnlExternal.Height = Unit.Pixel(180);
        }

        if (oDataTable.Rows.Count == 0)
            SetButtonVisibility(false);
        else
            SetButtonVisibility(true);
      
        FillMenuItemContents();
    }

    /// <summary>
    /// This method is used to set default priority to menu.
    /// </summary>
    /// <returns></returns>
    /// 
    private string SetDefaultPriority()
    {       
        ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
        return (oConfigureMenuBL.GetHighestPriority(miSchoolId).ToString());
    }

    /// <summary>
    /// This method is used to set query string.
    /// </summary>
    private void SetQueryString()
    {
        int iMenuId = 0;
        if (Menu_Configure.SelectedNode != null)
            iMenuId = Convert.ToInt32(Menu_Configure.SelectedValue);
        else if (trvExternal.SelectedNode != null)
            iMenuId = Convert.ToInt32(trvExternal.SelectedValue);
        hidMenuId.Value = "../Common/DisplayMenuContents.aspx?" + CommonUtility.EncryptQuerystring("MenuId=" + iMenuId + "&IsPreview=true");
    }

    /// <summary>
    /// This method is used to initialize ConfigureMenuBL;
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    private ConfigureMenuBL InitializeConfigureMenuBL(int aiSchoolId)
    {
        ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
        oConfigureMenuBL.AssoiciatedStandards = GetSelectedStandardDivList();
        oConfigureMenuBL.ConfigureMenuName = txtMenuName.Text;
        oConfigureMenuBL.ConfigureMenuContent = HttpUtility.HtmlEncode(FCKNewMenu.Text);
        oConfigureMenuBL.Priority = Convert.ToInt32(NmBoxPriority.Text);
        optAllAdd.Checked = true;
        if (optAllAdd.Checked)

             oConfigureMenuBL.SubMenuCount = 0;  
        else
            oConfigureMenuBL.SubMenuCount = Convert.ToInt32(txtShowTopAdd.Text);
      
        if (chkAllsubMenuAdd.Checked)
            oConfigureMenuBL.ApplyAllSubMenu = true;
        else
            oConfigureMenuBL.ApplyAllSubMenu = false;
        oConfigureMenuBL.SchoolId = aiSchoolId;
        oConfigureMenuBL.ParentMenuId = !string.IsNullOrEmpty(cmbSubMenuAdd.SelectedValue) && cmbSubMenuAdd.SelectedValue.ToInt() != 0 ? Convert.ToInt32(cmbSubMenuAdd.SelectedValue) : Convert.ToInt32(cmbParentMenuAdd.SelectedValue);
        if (cmbParentMenuAdd.SelectedIndex != 0)
            oConfigureMenuBL.IsExternal = CheckIfParentMenuIsApplicbleToAllUsers(Convert.ToInt32(cmbParentMenuAdd.SelectedValue));
        else
        {
            if (chkApplicableAdd.Checked)
                oConfigureMenuBL.IsExternal = Constants.C_YES;
            else
                oConfigureMenuBL.IsExternal = Constants.C_NO;
        }
        oConfigureMenuBL.IsDefault = Constants.C_NO;

        if (chkAddIsActive.Checked)
            oConfigureMenuBL.IsActive = Constants.C_YES;
        else
            oConfigureMenuBL.IsActive = Constants.C_NO;

        if (chkOnPopUp.Checked)
            oConfigureMenuBL.IsOnPopUp = Constants.C_YES;
        else
            oConfigureMenuBL.IsOnPopUp = Constants.C_NO;

        oConfigureMenuBL.InsertedById = miUserId;

        oConfigureMenuBL.UpdatedById = miUserId;

        oConfigureMenuBL.UserRoleIds = string.Join(",", GetSelectedRolesforInsert());

        hidParentMenuAddId.Value = cmbParentMenuAdd.SelectedValue.ToInt() != 0 ? cmbParentMenuAdd.SelectedValue : Constants.S_ZERO;
       

        return oConfigureMenuBL;
    }

    /// <summary>
    /// This method is used to collect selected roles into List collection.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedRolesforInsert()
    {
        int iTotalRoles = chkAddListRoles.Items.Count;
        List<int> RoleValues = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
        {
            if (chkAddListRoles.Items[iListIndex].Selected == true)
                RoleValues.Add(Convert.ToInt32(chkAddListRoles.Items[iListIndex].Value));
        }
        return RoleValues;
    }

    /// <summary>
    /// This method is used to collect selected roles into List collection.
    /// </summary>
    /// <returns></returns>
    private List<int> GetSelectedRolesforUpdate()
    {
        int iTotalRoles = chkListRoles.Items.Count;
        List<int> RoleValues = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalRoles; iListIndex++)
        {
            if (chkListRoles.Items[iListIndex].Selected == true)
                RoleValues.Add(Convert.ToInt32(chkListRoles.Items[iListIndex].Value));
        }
        return RoleValues;
    }

    /// <summary>
    /// This method is used to add menu items into treeview.
    /// </summary>
    /// <param name="aoTrv"></param>
    /// <param name="aoDT"></param>
    /// <param name="asSelectedMenuItemValue"></param>
    /// <param name="abSetFirstNodeSelected"></param>
    private void AddMenuItems(ref TreeView aoTrv, DataTable aoDT, string asSelectedMenuItemValue, bool abSetFirstNodeSelected)
    {
        aoTrv.Nodes.Clear();
        DataRow[] oArrParentMenus = aoDT.Select("Parent_Menu_Id=0 OR Is_External='Y'" );
        int iCount = 0;
        char cIsActive = 'N';
        foreach (DataRow oDRParent in oArrParentMenus)
        {
            string sText = oDRParent["ConfigureMenuName"].ToString();
            string sValue = oDRParent["ConfigureMenuId"].ToString();            

            TreeNode oMenuItem = new TreeNode(sText, sValue);

            if (abSetFirstNodeSelected)
            {
                if (asSelectedMenuItemValue == string.Empty && iCount == 0 || asSelectedMenuItemValue == sValue)
                    oMenuItem.Selected = true;
            }
            else
            {
                if (asSelectedMenuItemValue == sValue)
                    oMenuItem.Selected = true;
                cIsActive = Convert.ToChar(oDRParent["Is_Active"]);
                if (cIsActive == 'Y')
                    oMenuItem.Text = sText + ":Active";                    
            }
            iCount++;

            // Add menus which are Sub menus.
            DataRow[] oArrRows = aoDT.Select("Parent_Menu_Id =" + sValue + " AND Is_External<>'Y'");
            foreach (DataRow oDR in oArrRows)
            {
                sText = "      " + oDR["ConfigureMenuName"].ToString();
                sValue = oDR["ConfigureMenuId"].ToString();

                TreeNode oMenusubItem = new TreeNode(sText, sValue);
                oMenuItem.ChildNodes.Add(oMenusubItem);

                // Add menus which are child menus of Sub Menu.
                DataRow[] oArrSubMenuRows = aoDT.Select("Parent_Menu_Id =" + sValue + " AND Is_External<>'Y'");
                foreach (DataRow oDRSubMenu in oArrSubMenuRows)
                {
                    sText = "      " + oDRSubMenu["ConfigureMenuName"].ToString();
                    sValue = oDRSubMenu["ConfigureMenuId"].ToString();

                    TreeNode trvSubMenuItem = new TreeNode(sText, sValue);
                    oMenusubItem.ChildNodes.Add(trvSubMenuItem);

                    DataRow[] oArrChildMenuRows = aoDT.Select("Parent_Menu_Id =" + sValue + " AND Is_External<>'Y'");
                    if (oArrChildMenuRows.Length > Constants.I_ZERO)
                    {
                        foreach (DataRow oDRChildMenu in oArrChildMenuRows)
                        {
                            sText = "      " + oDRChildMenu["ConfigureMenuName"].ToString();
                            sValue = oDRChildMenu["ConfigureMenuId"].ToString();

                            TreeNode trvChildMenuItem = new TreeNode(sText, sValue);
                            trvSubMenuItem.ChildNodes.Add(trvChildMenuItem);

                            if (aoTrv.SelectedNode == null)
                            {
                                if (asSelectedMenuItemValue == sValue)
                                    trvChildMenuItem.Selected = true;
                            }
                        }
                    }

                    if (aoTrv.SelectedNode == null)
                    {
                        if (asSelectedMenuItemValue == sValue)
                            trvSubMenuItem.Selected = true;
                    }
                }

                if (aoTrv.SelectedNode == null)
                {
                    if (asSelectedMenuItemValue == sValue)
                        oMenusubItem.Selected = true;
                }
            }
            aoTrv.Nodes.Add(oMenuItem);
        }
        aoTrv.ExpandAll();
    }

    /// <summary>
    /// This function is used to set the visibility of the btn Update and Delete.
    /// </summary>
    /// <param name="ablnValue"></param>
    private void SetButtonVisibility(bool ablnValue)
    {
        DivAddNewMenu.Visible = !ablnValue;
        DivViewMenues.Visible = ablnValue;
    }

    /// <summary>
    /// This method is used to fill menu item contents.
    /// </summary>
    private void FillMenuItemContents()
    {
        int iMenuId = 0;
        string sMenuContent = string.Empty;
        string sMenuPriority = string.Empty;
        string sMenuName = string.Empty;

        if (Menu_Configure.SelectedNode != null)
            iMenuId = Convert.ToInt32(Menu_Configure.SelectedValue);            
        else if (trvExternal.SelectedNode != null)
            iMenuId = Convert.ToInt32(trvExternal.SelectedValue);            
        else
            SetDefaultControlsReadonlyProperty(true);

        if (iMenuId != 0)
        {
            SetDefaultControlsReadonlyProperty(false);

            ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL(iMenuId);            
            FCKMenuView.Text= HttpUtility.HtmlDecode(oConfigureMenuBL.ConfigureMenuContent);
            txtMenuNameUpdate.Text = oConfigureMenuBL.ConfigureMenuName;
            if (oConfigureMenuBL.End_Date == null)
                txtDateUpdate.Text = string.Empty;
            else
            {
                DateTime odtEndDate = Convert.ToDateTime(oConfigureMenuBL.End_Date);
                txtDateUpdate.Text = odtEndDate.ToString("dd-MMM-yyyy");
            }
            NmcBxPriorityUpdate.Text = oConfigureMenuBL.Priority.ToString();
            txtShowTopUpdate.Text = oConfigureMenuBL.SubMenuCount.ToString();

            if ( oConfigureMenuBL.SubMenuCount >0 )
                optTopUpdate.Checked=true;
            else
                optAllUpdate.Checked=true;
         
            cmbParentMenu.SelectedValue = null;
            hidIsExternal.Value = oConfigureMenuBL.IsExternal.ToString();
            if (oConfigureMenuBL.IsExternal == Constants.C_NO)
                chkApplicable.Checked = false;
            else
                chkApplicable.Checked = true;

            txtMenuNameUpdate.Focus();

            if (oConfigureMenuBL.ApplyAllSubMenu)
                chkAllsubMenu.Checked = true;
            else
                chkAllsubMenu.Checked = false;
                       
           if (CheckIfCurrentMenuIsParentMenu())
            {
                cmbParentMenu.Enabled = false;
                cmbSubMenu.Enabled = false; 
            }
            else
            {
                cmbParentMenu.Enabled = true;
                cmbSubMenu.Enabled = true;
            }

            if (oConfigureMenuBL.IsActive == Constants.C_YES)
                chkIsActive.Checked = true;
            else
                chkIsActive.Checked = false;

            if (oConfigureMenuBL.IsOnPopUp == Constants.C_YES)
            {
                chkOnPopUp.Checked = true;
                chkApplicable.Checked = false;
                chkApplicable.Enabled = false;
            }
            else
            {
                chkOnPopUp.Checked = false;
                chkApplicable.Enabled = true;
            }

            FetchRoles(iMenuId);

            DisableDeleteButtonForDefaultMenu(oConfigureMenuBL.IsDefault);

            if (Menu_Configure.SelectedNode != null )
            {
                string sMainMenu = "", sSubMenu = "";

                if (Menu_Configure.SelectedValue != string.Empty)
                {
                   if (sMainMenu == string.Empty)
                   {
                       if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                           sMainMenu = Menu_Configure.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                           sMainMenu = Menu_Configure.SelectedNode.Parent != null ? Menu_Configure.SelectedNode.Parent.Value.ToString() : Menu_Configure.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsSecondChildMenu())
                           sMainMenu = Menu_Configure.SelectedNode.Parent != null && Menu_Configure.SelectedNode.Parent.Parent != null ? Menu_Configure.SelectedNode.Parent.Parent.Value.ToString() : Menu_Configure.SelectedValue.ToString();
                   }

                   if (sSubMenu == string.Empty)
                   {
                       if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                           sSubMenu = Menu_Configure.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                           sSubMenu = Menu_Configure.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsSecondChildMenu())
                           sSubMenu = Menu_Configure.SelectedNode.Parent != null ? Menu_Configure.SelectedNode.Parent.Value.ToString() : Constants.S_ZERO;
                   }
               }
               else if (trvExternal.SelectedValue != string.Empty)
               {
                   if (sMainMenu == string.Empty)
                   {
                       if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                           sMainMenu = trvExternal.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                           sMainMenu = trvExternal.SelectedNode.Parent != null ? trvExternal.SelectedNode.Parent.Value.ToString() : trvExternal.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsSecondChildMenu())
                           sMainMenu = trvExternal.SelectedNode.Parent != null && trvExternal.SelectedNode.Parent.Parent != null ? trvExternal.SelectedNode.Parent.Parent.Value.ToString() : trvExternal.SelectedValue.ToString();
                   }

                   if (sSubMenu == string.Empty)
                   {
                       if (CheckIfCurrentMenuIsParentMenu() || CheckIfCurrentMenuIsTopMenu())
                           sSubMenu = trvExternal.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsFirstChildMenu() || CheckIfCurrentMenuIsSubMenu())
                           sSubMenu = trvExternal.SelectedValue.ToString();
                       else if (CheckIfCurrentMenuIsSecondChildMenu())
                       {
                           sSubMenu = trvExternal.SelectedNode.Parent != null ? trvExternal.SelectedNode.Parent.Value.ToString() : Constants.S_ZERO;
                       }
                   }
               }

                if (CheckIfCurrentMenuIsSecondChildMenu())
                {
                    ListItem oSubMenuItem = cmbSubMenu.Items.FindByValue(sSubMenu);
                    if (oSubMenuItem != null)
                        oSubMenuItem.Selected = true;
                }
                else
                {
                    ListItem oSubMenuItem = cmbSubMenu.Items.FindByValue(Constants.S_ZERO);
                    if (oSubMenuItem != null)
                        oSubMenuItem.Selected = true;
                }

                ListItem oItem = cmbParentMenu.Items.FindByValue(sMainMenu);

                if (oItem != null)
                {
                    oItem.Selected = true;
                    if (oConfigureMenuBL.ParentMenuId != 0)
                    {
                        chkApplicable.Enabled = false;
                        hidIsChildMenu.Value = Constants.C_YES.ToString();
                    }
                    else
                        hidIsChildMenu.Value = Constants.C_NO.ToString();
                }
            }
            else if (trvExternal.SelectedNode != null )
            {
                if (CheckIfCurrentMenuIsChildMenu())
                    chkApplicable.Enabled = false;
                else
                    chkApplicable.Enabled = true;
                cmbParentMenu.Enabled = false;
                cmbSubMenu.Enabled = false;
            }

            // Remove current menu from parent combobox if present.
            ListItem oItemCurrent = cmbParentMenu.Items.FindByText(oConfigureMenuBL.ConfigureMenuName);

            if (oItemCurrent != null)
                cmbParentMenu.Items.Remove(oItemCurrent);
            txtMenuNameUpdate.Focus();
    
        }

        if (chkListRoles.Items.FindByValue("3") != null && chkListRoles.Items.FindByValue("3").Selected)
        {
            FillStandardChkLstBox(iMenuId);
            trAssociatedClasses.Style.Add("display", "");
            trUpdateAssociatedClasses.Style.Add("display", "");
        }
        else
        {
            FillStandardChkLstBox(0);
            trAssociatedClasses.Style.Add("display", "none");
            trUpdateAssociatedClasses.Style.Add("display", "none");
        }
    }

    /// <summary>
    /// This method is used to fetch roles from table MenusRoles according to MenuId.
    /// </summary>
    private void ClearRoles()
    {
        chkAddAll.Checked = false;
        for (int iIndex = 0; iIndex < chkListRoles.Items.Count; iIndex++)
                chkListRoles.Items[iIndex].Selected = false;

        for (int iIndex = 0; iIndex < chkAddListRoles.Items.Count; iIndex++)
            chkAddListRoles.Items[iIndex].Selected = false;
    }

    /// <summary>
    /// This method is used to fetch roles from table MenusRoles according to MenuId.
    /// </summary>
    private void FetchRoles(int aiNoticeId)
    {
        int iItemCount, iRowCount, iRowIndex;
        ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL(aiNoticeId);

        DataTable ODTNoticeBoardRoles = oConfigureMenuBL.GetUserRolesForSelectedMenuId(aiNoticeId);
        if (ODTNoticeBoardRoles.Rows.Count > 0)
        {
            iItemCount = chkListRoles.Items.Count;
            iRowCount = ODTNoticeBoardRoles.Rows.Count - 1;
            DataRow oDRRoles;
            iRowIndex = 0;
            for (int iIndex = 0; iIndex < iItemCount; iIndex++)
            {
                oDRRoles = ODTNoticeBoardRoles.Rows[iRowIndex];
                if (chkListRoles.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
                {
                    chkListRoles.Items[iIndex].Selected = true;
                    if (iRowIndex < iRowCount)
                        iRowIndex++;
                }
                else
                    chkListRoles.Items[iIndex].Selected = false;
            }
        }
    }

    /// <summary>
    /// This method is used to check whether parent menu is applicable to all users or not.
    /// </summary>
    /// <param name="aiParentMenuID"></param>
    /// <returns></returns>
    private char CheckIfParentMenuIsApplicbleToAllUsers(int aiParentMenuID)
    {
        DataTable oDT = (DataTable)Session["S_PARENT_MENUS"];
        DataRow[] oArrRows = oDT.Select("configuremenuid =" + aiParentMenuID);

        char cApplicable = Constants.C_NO;
        if (oArrRows.Length > 0)
            cApplicable = Convert.ToChar(oArrRows[0]["Is_External"].ToString());
        return cApplicable;
    }

    /// <summary>
    /// This method is used to disable delete button for default menu.
    /// </summary>
    /// <param name="acIsDefault"></param>
    private void DisableDeleteButtonForDefaultMenu(char acIsDefault)
    {
        if (acIsDefault == Constants.C_YES)
            VisibilityForDefaultMenu(true);
        else
            VisibilityForDefaultMenu(false);
        chkIsDefault.Enabled = false;
    }

    /// <summary>
    /// This method is used to set visibility for default menu.
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibilityForDefaultMenu(bool abAction)
    {
        imgBtnDelete.Enabled = !abAction;
        chkIsDefault.Checked = abAction;
        chkApplicable.Enabled = !abAction;
        chkIsDefault.Enabled = !abAction;
    }

    /// <summary>
    /// This method is used to set readonly property for default menu.
    /// </summary>
    /// <param name="ablnValue"></param>
    private void SetDefaultControlsReadonlyProperty(bool ablnValue)
    {
        txtMenuNameUpdate.ReadOnly = ablnValue;
        NmcBxPriorityUpdate.ReadOnly = ablnValue;
        txtShowTopUpdate.ReadOnly = ablnValue;
    }
    /// <summary>
    /// this method is used to collect selected standard division list
    /// </summary>
    /// <returns></returns>
    private string GetSelectedStandardDivList()
    {
        if (chkAddListRoles.Items.FindByValue("3") != null && chkAddListRoles.Items.FindByValue("3").Selected)
        {
            StringBuilder oStandards = new StringBuilder();

            foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
            {
                CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
                for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
                {
                    if (chkStandardDivLst.Items[iCount].Selected)
                        oStandards.Append(chkStandardDivLst.Items[iCount].Value + ",");
                }
            }

            return oStandards.ToString();
        }
        else
            return string.Empty;
    }

    /// <summary>
   /// thise method is collect selected standard division list for update 
    /// </summary>
    /// <returns></returns>
  private string GetSelectedStandardDivisionsForUpdate()
  {
      List<string> selectedIds = new List<string>();

      if (chkListRoles.Items.FindByValue("3") != null && chkListRoles.Items.FindByValue("3").Selected)
      {
          foreach (ListViewDataItem item in lstvwUpdateStandardDivision.Items)
          {
              CheckBoxList chkList = item.FindControl("chkStandardDivLst") as CheckBoxList;
              if (chkList != null)
              {
                  foreach (ListItem li in chkList.Items)
                  {
                      if (li.Selected)
                          selectedIds.Add(li.Value);
                  }
              }
          }

          return string.Join(",", selectedIds);
      }
      else
          return string.Empty;
  }
    /// <summary>
    /// This method is used to fill standard check box list.
    /// </summary>
  private void FillStandardChkLstBox(int aiMenuId)
  {
      ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
      molstConfigMenuAssociatedClasses = oConfigureMenuBL.GetConfigMenuAssociatedClasses(miSchoolId, miAcademicYearId, aiMenuId);
      var oStandards = molstConfigMenuAssociatedClasses.Select(sd => new { StandardName = sd.StandardName, StandardId = sd.StandardId }).Distinct().ToList();

      lstvwStandardDivisions.DataSource = oStandards;
      lstvwStandardDivisions.DataBind();
      lstvwUpdateStandardDivision.DataSource = oStandards;
      lstvwUpdateStandardDivision.DataBind();
  }

#endregion//Method(s)

}
