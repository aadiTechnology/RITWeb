// File Name  : UserLoginUI.aspx.cs
// Created By : Ashish
// Date       : 28/12/2008
//Description : This class is used to the login user account for super admin as per user role.

using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class UserLoginUI :SchoolBase
{
    #region Constants

    const int I_USERROLE_STUDENT = 3;
    const int I_USERROLE_SUPERVISOR = 6;
    const int I_USER_ID = 0;
    const int I_COLUMN_INDEX_NAME = 1;
    const int I_COLUMN_INDEX_RNO = 0;
    const int I_LOGIN_NAME_COLUMN_INDEX = 2;
    const int I_LOGIN_COLUMN_INDEX = 3;

    #endregion

    #region Events

    /// <summary>
    /// This event is handled while loading this page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {           
            if (!IsPostBack)
            {
                SetDefaultProperties();                
                if (CheckPreCondition())
                {
                    InitialiseFields();
                    FillUserRoleCombo();
                }
            }
            SetClientScriptAttributes();           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///<Summary>
    ///This method is used to cancel the transaction.
    ///</Summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// This event is used to lock or unlock user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("LOGIN"))
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                string sLoginName = grdUsers.Rows[iRowIndex].Cells[I_LOGIN_NAME_COLUMN_INDEX].Text;
                UpdateSessionVariableAndRedirectToNextPage(miSchoolId, sLoginName);

                Response.Write("<Script language ='javascript'>window.open('../Common/ControlPanel.aspx','_blank'); </Script>");
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
            }
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
    protected void grdUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlUserRole.SelectedValue) == I_USERROLE_STUDENT && e.SortExpression == "Name")
            {
                e.SortExpression = e.SortExpression.Replace("Name", "First_Name " + hidSortDirection.Value);
            }
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event sets properties to grid's column.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                int iRowindex = e.Row.RowIndex;
                System.Web.UI.WebControls.Button obtnLogin = (System.Web.UI.WebControls.Button)e.Row.Cells[I_LOGIN_COLUMN_INDEX].Controls[0];
                ApplyMouseHoverEffect(new List<Button> { obtnLogin });
            }
            SetGridPaging(e.Row);
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event sets sortimaege.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdUsers_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex;
                if (ddlUserRole.SelectedValue == Constants.UserRoles.Student.ToString())
                    sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                else
                    sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
                else
                {
                    CommonUtility.AddSortImage(1, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void grdUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdUsers.PageIndex = e.NewPageIndex;
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdUsers.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdUsers.PageIndex = pageList.SelectedIndex;
            FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);        
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdUsers.PageSize * grdUsers.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdUsers.PageSize) - 1);
                if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " ComboBox Event "

    /// <summary>
    /// This event is used to fill standard combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {   
            SetDefaultDDLProperty();
            SetDefaultSortGridArrowOfGrid();
            if (ddlUserRole.SelectedIndex == 0)
            {
                pnlUserGrid.Visible = false;               
            }
            else if (Convert.ToInt32(ddlUserRole.SelectedValue) == I_USERROLE_STUDENT)
            {
                pnlForStudent.Visible = true;
                ddlStandard.Visible = true;
                FillStandardCombo();
            }
            else
            {
                pnlUserGrid.Visible = true;
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);              
            }
            if (grdUsers.PageCount > 0)
                grdUsers.PageIndex = 0;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    
    /// <summary>
    /// This event is used to fill division's combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdUsers.Visible = false;
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            ddlDivision.Visible = true;
            trTotalRec.Visible = false;
            FillDivisionCombobox(iStandardId);
       
             if (ddlStandard.SelectedIndex != 0)
            {
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
                grdUsers.Visible = true;
                pnlUserGrid.Visible = true;
            }
            else
            {
                trTotalRec.Visible = false;
                grdUsers.Visible = false;
                ListItem olstDivision = new ListItem();
                olstDivision.Text = Constants.S_SELECT;
                ddlDivision.Items.Add(olstDivision);
            }
             if (grdUsers.PageCount > 0)
                 grdUsers.PageIndex = 0;
    }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to display grid with respected standard and division.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdUsers.Visible = true;
            pnlUserGrid.Visible = true;
            SetDefaultSortGridArrowOfGrid();
            if (Convert.ToInt32(ddlDivision.SelectedValue) != 0)
                FillUserDetailGrid(Convert.ToInt32(ddlUserRole.SelectedValue), null);
            else
            {
                trTotalRec.Visible = false;
                grdUsers.Visible = false;
            }
            if (grdUsers.PageCount > 0)
                grdUsers.PageIndex = 0;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>  
    private void SetDefaultProperties()
    {
        ddlUserRole.Focus();
        trTotalRec.Visible = false;
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitialiseFields()
    {
        grdUsers.Columns[0].Visible = false;
        lblTo.Text = Constants.S_TO;
        lblOutOf.Text = Constants.S_OUT_OF;
        lblRecords.Text = Constants.S_RECORDS;
        Constants.S_SUPERVISOR_ROLE_NAME = Settings.SupervisorRoleName;
    }

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo()
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSStateCollection = oMasterDataCollectionBL.GetAllUserRolesExceptAdmin();
        DataRow[] oDataRow = new DataRow[0];
        oDataRow = oDSStateCollection.Select("User_Role_Id=6");
        if (oDataRow.Length > 0)
        {
            oDataRow[0][Constants.S_USER_ROLE_NAME_FIELD] = Convert.ToString(Constants.S_SUPERVISOR_ROLE_NAME);
        }
        ControlUtility.FillDropDownList(oDSStateCollection, ref ddlUserRole,
                                        Constants.S_USER_ROLE_ID_FIELD,
                                       Constants.S_USER_ROLE_NAME_FIELD,
                                       Constants.S_SELECT);        
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel });
    }

    /// <summary>
    /// This method is used to set control/property on drop down list change event.
    /// </summary>
    private void SetDefaultDDLProperty()
    {
        grdUsers.PageIndex = 0;
        grdUsers.Columns[0].Visible = false;
        pnlUserGrid.Visible = false;
        pnlForStudent.Visible = false;
        ddlDivision.Items.Clear();
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        ListItem olstDivision = new ListItem();
        olstDivision.Text = "--Select--";
        ddlDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
    }

    /// <summary>
    /// This method is used to fill userdetail's grid of a particular user.
    /// </summary>
    /// <param name="aiUserRoleId"></param>
    /// <param name="sSortExpression"></param>
    private void FillUserDetailGrid(int aiUserRoleId, String sSortExpression)
    {
        SchoolUserCollectionBL oSchoolUserCollectionBL = new SchoolUserCollectionBL();
        SetEmptyDataText();
        if (aiUserRoleId == I_USERROLE_STUDENT)
        {
            if (grdUsers.SortExpression.Contains("Name"))
            {
                grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression = "First_Name";
                hidSortExpression.Value = "First_Name";
            }
            grdUsers.Columns[0].Visible = true;
            grdUsers.DataSourceID = GrdODStudent.ID;

        }
        else
        {
            if (grdUsers.SortExpression.Contains("First_Name"))
            {
                grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression = "Name";
                hidSortExpression.Value = "Name";
            }
            grdUsers.DataSourceID = GrdDSobj.ID;
        }
    }

    /// <summary>
    /// This method is used to set empty data text to grid.
    /// </summary>
    private void SetEmptyDataText()
    {
        if (ddlUserRole.SelectedValue == Convert.ToString(I_USERROLE_STUDENT))
        {
            grdUsers.EmptyDataText = "No student available.";
        }
        else if (ddlUserRole.SelectedValue == Convert.ToString(I_USERROLE_SUPERVISOR))
        {
            grdUsers.EmptyDataText = "No " + Constants.S_SUPERVISOR_ROLE_NAME + " available.";
        }
        else
        {
            grdUsers.EmptyDataText = "No teacher available.";
        }
    }

    /// <summary>
    /// This function is used to set initial values of sort variables
    /// </summary>
    private void SetDefaultSortGridArrowOfGrid()
    {
        if (ddlUserRole.SelectedValue == Convert.ToString(I_USERROLE_STUDENT))
        {
            grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression = "Name";
            grdUsers.Columns[I_COLUMN_INDEX_NAME].HeaderText = "Name";
            hidSortExpression.Value = grdUsers.Columns[I_COLUMN_INDEX_RNO].SortExpression;
        }
        else
        {
            if (Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Teacher) ||
                Convert.ToInt32(ddlUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Supervisor) )
            {
                grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression = "Name";
                grdUsers.Columns[I_COLUMN_INDEX_NAME].HeaderText = "Name (Designation)";
            }
            else
            {
                grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression = "Name";
                grdUsers.Columns[I_COLUMN_INDEX_NAME].HeaderText = "Name";
            }
            hidSortExpression.Value = grdUsers.Columns[I_COLUMN_INDEX_NAME].SortExpression;
        }
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {

        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set grid view paging.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetGridPaging(GridViewRow gridViewRow)
    {
        if (gridViewRow.RowType == DataControlRowType.Pager)
        {
            GridViewRow pagerRow = gridViewRow;
            // Retrieve the DropDownList and Label controls from the row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

            if (pageList != null)
            {
                // Create the values for the DropDownList control based on 
                // the  total number of pages required to display the data
                // source.
                for (int i = 0; i < grdUsers.PageCount; i++)
                {
                    // Create a ListItem object to represent a page.
                    int pageNumber = i + 1;
                    ListItem item = new ListItem(pageNumber.ToString());
                    // If the ListItem object matches the currently selected
                    // page, flag the ListItem object as being selected. Because
                    // the DropDownList control is recreated each time the pager
                    // row gets created, this will persist the selected item in
                    // the DropDownList control.                        
                    if (i == grdUsers.PageIndex)
                    {
                        item.Selected = true;
                    }
                    // Add the ListItem object to the Items collection of the 
                    // DropDownList.
                    pageList.Items.Add(item);
                }
            }
            if (pageLabel != null)
            {
                // Calculate the current page number.
                int currentPage = grdUsers.PageIndex + 1;
                // Update the Label control with the current page information.
                pageLabel.Text = "Page " + currentPage.ToString() +
                  " of " + grdUsers.PageCount.ToString();
            }
        }
    }

    /// <summary>
    /// This function checks the preconditons of WeekdayTimeTable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.UserMangement);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            trPrecondition.Visible = true;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls depends configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        pnlForStudent.Visible = false;
        pnlUserGrid.Visible = false;
        btnCancel.Text = "Back";
        trUserRole.Visible = false;
    }

    #endregion
}
