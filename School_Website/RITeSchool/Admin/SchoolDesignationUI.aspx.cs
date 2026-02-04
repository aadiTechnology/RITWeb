using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using MasterEntities;
using System.Data.SqlClient;
public partial class SchoolDesignationUI : SchoolBase
{
    #region Data Members

    private DesignationMasterBL moDesignationMasterBL;

    #endregion

    #region "CONSTANTS"

    const string S_DEFAULT_SORT_EXP = "Name";

    #endregion

    #region "EVENTS"


    /// <summary>
    /// This event is used to add the sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add Sort Image
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to fill existing Designation listView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moDesignationMasterBL = new DesignationMasterBL(miSchoolId, miAcademicYearId, miUserId);

        if (!IsPostBack)
        {
            SetJavascriptAttributes();
            FillUserRoleCombo();
            FillDesignations();
            btnAdd.Text = Constants.ButtonText.Save.ToString();
        }
    }

    /// <summary>
    /// This event is used to add attribute to existing StopName listviews item control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureDesignation_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DesignationMaster oDesignationMaster = oCurrentItem.DataItem as DesignationMaster;

                HtmlTableCell tIsAccountingScreen = e.Item.FindControl("tIsAccountingScreen") as HtmlTableCell;
                if (rdoPTADesig.Checked)
                    tIsAccountingScreen.Visible = false;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                Image oImage = oCurrentItem.FindControl("imgTickmark") as Image;
                oImage.Visible = oDesignationMaster.HasAccountAccess;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of existing Stop name listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureDesignation_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfigureDesignation.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwConfigureDesignation, DtPgCount);
                //AddSortImage();
            }
            else
            {
                DtPgCount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Edit or Delete Designation Names 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureDesignation_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iDesignationId = Convert.ToInt32(lstvwConfigureDesignation.DataKeys[iListIndex]["DesignationId"]);
                hidDesignationId.Value = iDesignationId.ToString();

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    Delete(iDesignationId);
                    DisplayMessage(Constants.ItemState.deleted, false);
                    FillDesignations();
                    if (lstvwConfigureDesignation.Items.Count == 0)
                        DeleteConfigDetails(Constants.SchoolConfigurations.Designations.ToInt());
                    ClearFields();
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnAdd.Text = Constants.ButtonText.Update.ToString();
                    DesignationMaster oDesignationMaster = moDesignationMasterBL.Get(iDesignationId, rdoPTADesig.Checked);
                    {
                        cmbUserRole.SelectedValue = oDesignationMaster.UserRoleId.ToString();
                        txtDesignationName.Text = oDesignationMaster.Designation;
                        txtSortOrder.Text = oDesignationMaster.SortOrder.ToString();
                        hidDesignationId.Value = iDesignationId.ToString();
                        if (oDesignationMaster.UserRoleId == Constants.UserRoles.Supervisor.ToInt())
                            chkIsAccountingScreenAvailable.Enabled = true;
                        else
                        {
                            chkIsAccountingScreenAvailable.Enabled = false;
                            chkIsAccountingScreenAvailable.Checked = false;
                        }
                        if (oDesignationMaster.HasAccountAccess)
                            chkIsAccountingScreenAvailable.Checked = true;
                        else
                            chkIsAccountingScreenAvailable.Checked = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message, true, tdMessage);
        }
    }

    /// <summary>
    /// To Insert Designation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAdd.Text == Constants.ButtonText.Update.ToString())
            {
                Update();
                DisplayMessage(Constants.ItemState.updated, false);
            }
            else if (btnAdd.Text == Constants.ButtonText.Save.ToString())
            {
                Save();
                DisplayMessage(Constants.ItemState.saved, false);
            }
            FillDesignations();

            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Constants.SchoolConfigurations.Designations.ToInt());
        }
        catch (DuplicateEntityException exx)
        {
            DisplayMessage(exx.Message, true, tdMessage);

        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// To clear fields..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    /// <summary>
    /// To go back to dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureDesignation_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise Designation Name list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwConfigureDesignation);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to check is select role is Adminstaff or not 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbUserRole.SelectedValue.ToInt() == Constants.UserRoles.Supervisor.ToInt())
            {

                chkIsAccountingScreenAvailable.Enabled = true;
                chkIsAccountingScreenAvailable.Checked = false;
            }
            else
            {
                chkIsAccountingScreenAvailable.Enabled = false;
                chkIsAccountingScreenAvailable.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This Event is used for Editing ListView Items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureDesignation_ItemEditing(object sender, ListViewEditEventArgs e) { }

    /// <summary>
    /// This event is used to set Designation type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoSchoolDesig_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserRoleCombo();
            FillDesignations();
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set Designation type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoPTADesig_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserRoleCombo();
            FillDesignations();
            chkIsAccountingScreenAvailable.Checked = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo()
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        cmbUserRole.Items.Clear();
        if (rdoPTADesig.Checked)
        {
            chkIsAccountingScreenAvailable.Enabled = false;
            cmbUserRole.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = "0" });
            cmbUserRole.Items.Add(new ListItem { Text = "School", Value = "1" });
            cmbUserRole.Items.Add(new ListItem { Text = "Parent", Value = "2" });
        }
        else
        {

            DataTable oDSDesignatiuon = oMasterDataCollectionBL.GetAllUserRoles();
            ControlUtility.FillDropDownList(oDSDesignatiuon.Select("User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Student) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Parent) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Admin) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.ExAdmin)), ref cmbUserRole,
                                            Constants.S_USER_ROLE_ID_FIELD,
                                            Constants.S_USER_ROLE_NAME_FIELD,
                                            Constants.S_SELECT);
        }
    }

    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        rdoSchoolDesig.Checked = true;
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnAdd, btnBack });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnAdd.Attributes.Add("Onclick", "ClearSuccessfulMessage()");
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwConfigureDesignation.SortDirection.ToString() == "Ascending" || lstvwConfigureDesignation.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwConfigureDesignation.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwConfigureDesignation.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwConfigureDesignation.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used set datasource  to ListView
    /// </summary>
    private void FillDesignations()
    {
        lstvwConfigureDesignation.DataSourceID = ObjDSConfigureDesignation.ID;
        lstvwConfigureDesignation.DataBind();
        SetListFields();

    }

    /// <summary>
    /// This method is used set ListView field.
    /// </summary>
    private void SetListFields()
    {
        HtmlTableRow tr = lstvwConfigureDesignation.FindControl("trHeader") as HtmlTableRow;
        if (tr != null)
        {
            HtmlTableCell thIsAccountScreen = tr.FindControl("thIsAccountScreen") as HtmlTableCell;
            if (thIsAccountScreen != null)
            {
                if (rdoPTADesig.Checked)
                    thIsAccountScreen.Visible = false;
                else
                    thIsAccountScreen.Visible = true;
            }
        }
    }

    /// <summary>
    /// This method is used to save Designation Details
    /// </summary>
    private void Save()
    {
        DesignationMaster oDesignationMaster = Populate();
        oDesignationMaster.HasAccountAccess = chkIsAccountingScreenAvailable.Checked;
        moDesignationMasterBL.Insert(oDesignationMaster, rdoPTADesig.Checked);
        FillDesignations();
        ClearFields();
    }

    /// <summary>
    /// This method is used to Update designation Details
    /// </summary>
    private void Update()
    {
        DesignationMaster oDesignationMaster = Populate();
        if (!string.IsNullOrEmpty(hidDesignationId.Value))
        {
            oDesignationMaster.DesignationId = Convert.ToInt32(hidDesignationId.Value);
            oDesignationMaster.HasAccountAccess = chkIsAccountingScreenAvailable.Checked;

            moDesignationMasterBL.Update(oDesignationMaster, rdoPTADesig.Checked);

            ClearFields();
        }
    }

    /// <summary>
    /// This method is used fill entities to add or update from screen.
    /// </summary>
    /// <returns></returns>
    private DesignationMaster Populate()
    {
        DesignationMaster oDesignationMaster = new DesignationMaster
        {
            Designation = txtDesignationName.Text.ToString(),
            UserRoleName = cmbUserRole.SelectedItem.ToString(),
            SortOrder = txtSortOrder.Text.Trim().ToInt(),
            UserRoleId = cmbUserRole.SelectedValue.ToInt()
        };
        return oDesignationMaster;
    }

    /// <summary>
    /// This Method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        txtDesignationName.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        cmbUserRole.SelectedIndex = 0;
        chkIsAccountingScreenAvailable.Enabled = false;
        chkIsAccountingScreenAvailable.Checked = false;
        btnAdd.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        //AddSortImage();
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = SortDirection.Ascending.ToString();
    }

    /// <summary>
    /// This method is used to check dependancy of designation and Delete Designation. 
    /// </summary>
    /// <returns></returns>
    private int Delete(int aiDesignationID)
    {
        moDesignationMasterBL = new DesignationMasterBL(miSchoolId, miAcademicYearId, miUserId);
        return moDesignationMasterBL.Delete(aiDesignationID, miSchoolId, miAcademicYearId, miUserId, rdoPTADesig.Checked);
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Designation details " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }
    #endregion



}