using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using PayrollEntities;
using System.Data;

public partial class RITeSchool_Payroll_UserWeekEndAssociation : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillStaffGroups();
                SetJavascriptAttributes();

                lstvwStaffGroupUsers.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show users.
    /// </summary>
    /// <param name="e"></param>
    protected void btnshow_Click(object sender, EventArgs e)
    {
        FillStaffGroupUsersGrid();
    }

    /// <summary>
    /// 
    /// </summary>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwStaffGroupUsers.Visible = false;
            btnSaveWeekends.Visible = false;
            DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set list view footer and sorting image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffGroupUsers_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStaffGroupUsers.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwStaffGroupUsers, DtPgCount);
                FillWeekendCheckboxes();
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
    /// 
    /// </summary>
    protected void lstvwStaffGroupUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                lstvwStaffGroupUsers.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to cancel the changes on screen.
    /// </summary>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lstvwStaffGroupUsers.Visible = false;
        DtPgCount.Visible = false;
        btnSaveWeekends.Visible = false;
        cmbStaffGroup.SelectedIndex = 0;

        if (txtUserName.Text != null)
            txtUserName.Text = string.Empty;
        if (lblmessage.Text != null)
            lblmessage.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to search by name.
    /// </summary>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lstvwStaffGroupUsers.DataSourceID = null;
        DataTable oDTStaffGroupUsers = UserWeekEndAssociationBL.GetUsersforSearch(txtUserName.Text.Trim(), miSchoolId);
        lstvwStaffGroupUsers.DataSource = oDTStaffGroupUsers;
        lstvwStaffGroupUsers.DataBind();

        DataTable odTStaffWeekendsforAll = UserWeekEndAssociationBL.GetAllWeekends(miSchoolId);

        int UserCount = 0;
        foreach (ListViewDataItem oCurrentItem in lstvwStaffGroupUsers.Items)
        {
            if (oCurrentItem.ItemType == ListViewItemType.DataItem)
            {
                CheckBoxList checkbox = (CheckBoxList)oCurrentItem.FindControl("ckhWeekendsList");
                {
                    checkbox.DataSource = odTStaffWeekendsforAll;
                    checkbox.DataTextField = "WeekDay_Name";
                    checkbox.DataValueField = "Original_WeekDays_Id";
                    checkbox.DataBind();
                }

                int UserId = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[UserCount]["UserId"]);
                DataTable odTStaffWeekends = UserWeekEndAssociationBL.GetWeekends(UserId, miSchoolId, miAcademicYearId);

                CheckBoxList checkboxList = (CheckBoxList)oCurrentItem.FindControl("ckhWeekendsList");
                int iWeekendCount = odTStaffWeekends.Rows.Count;
                int iItemCount = checkboxList.Items.Count;
                DataRow oDRRoles;

                for (int iIndex = 0; iIndex < iItemCount; iIndex++)
                {
                    if (iIndex < iWeekendCount)
                    {
                        oDRRoles = odTStaffWeekends.Rows[iIndex];

                        if (checkboxList.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
                        {
                            checkboxList.Items[iIndex].Selected = true;
                        }
                        else if (iIndex <= iItemCount)
                        {
                            if (checkboxList.Items[iIndex + 1].Value.ToString() == oDRRoles[0].ToString())
                            {
                                checkboxList.Items[iIndex + 1].Selected = true;
                            }
                        }
                        else
                            checkboxList.Items[iIndex].Selected = false;
                    }
                }
                UserCount++;
            }
        }

        if (oDTStaffGroupUsers.Rows.Count > 0)
            btnSaveWeekends.Visible = true;
        else
        {
            lblmessage.Text = "No Such User found !!";
            btnSaveWeekends.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to save Weekend details for users.
    /// </summary>
    /// <param name="e"></param>
    protected void btnSaveWeekends_Click(object sender, EventArgs e)
    {
        try
        {
            int UserCount = 0;
            foreach (ListViewDataItem Ldi in lstvwStaffGroupUsers.Items)
            {
                if (Ldi.ItemType == ListViewItemType.DataItem)
                {
                    CheckBoxList chkIsSelected = (CheckBoxList)Ldi.FindControl("ckhWeekendsList");
                    int UserID = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[UserCount]["UserId"]);

                    int iItemCount;
                    iItemCount = chkIsSelected.Items.Count;

                    for (int iIndex = 0; iIndex < iItemCount; iIndex++)
                    {
                        if (chkIsSelected.Items[iIndex].Selected == true)
                        {
                            UserWeekEndAssociationBL oWeekendDetailsBL = new UserWeekEndAssociationBL();
                            oWeekendDetailsBL.InsertUserWeekEndAssociationDetailsForUser(UserID, miSchoolId, miAcademicYearId, chkIsSelected.Items[iIndex].Value.ToInt());
                        }
                        else
                        {
                            UserWeekEndAssociationBL oWeekendDetailsBL = new UserWeekEndAssociationBL();
                            oWeekendDetailsBL.UpdateUserWeekendAssociationDetailsForUser(UserID, miSchoolId, miAcademicYearId, chkIsSelected.Items[iIndex].Value.ToInt());
                        }
                    }
                }
                UserCount++;
            }

            lblmessage.Text = "Weekend Details are saved Successfully !!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill StaffGroups dropdown.
    /// </summary>
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to fill Grid of Users.
    /// </summary>
    /// <param name="mistaffGroupID"></param>
    private void FillStaffGroupUsersGrid()
    {
        lstvwStaffGroupUsers.Visible = true;
        DataPager pager = lstvwStaffGroupUsers.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        DtPgCount.Visible = true;

        lstvwStaffGroupUsers.DataSourceID = ObjDSConfigureWeekend.ID;
        lstvwStaffGroupUsers.DataBind();

        ControlUtility.FillListViewPagerFooter(lstvwStaffGroupUsers, DtPgCount);
    }

    /// <summary>
    /// This method is used to fill checboxlist of weekends for all users.
    /// </summary>
    private void FillWeekendCheckboxes()
    {
        DataTable odTStaffWeekendsforAll = UserWeekEndAssociationBL.GetAllWeekends(miSchoolId);
        int UserCount = 0;
        foreach (ListViewDataItem oCurrentItem in lstvwStaffGroupUsers.Items)
        {
            if (oCurrentItem.ItemType == ListViewItemType.DataItem)
            {
                CheckBoxList checkbox = (CheckBoxList)oCurrentItem.FindControl("ckhWeekendsList");
                {
                    checkbox.DataSource = odTStaffWeekendsforAll;
                    checkbox.DataTextField = "WeekDay_Name";
                    checkbox.DataValueField = "Original_WeekDays_Id";
                    checkbox.DataBind();
                }
                
                int UserId = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[UserCount]["UserId"]);
                DataTable odTStaffWeekends = UserWeekEndAssociationBL.GetWeekends(UserId, miSchoolId, miAcademicYearId);

                CheckBoxList checkboxList = (CheckBoxList)oCurrentItem.FindControl("ckhWeekendsList");
                int iWeekendCount = odTStaffWeekends.Rows.Count;
                int iItemCount = checkboxList.Items.Count;
                DataRow oDRRoles;

                for (int iIndex = 0; iIndex < iItemCount; iIndex++)
                {
                    if (iIndex < iWeekendCount)
                    {
                        oDRRoles = odTStaffWeekends.Rows[iIndex];

                        if (checkboxList.Items[iIndex].Value.ToString() == oDRRoles[0].ToString())
                        {
                            checkboxList.Items[iIndex].Selected = true;
                        }
                        else if (iIndex <= iItemCount)
                        {
                            if (checkboxList.Items[iIndex + 1].Value.ToString() == oDRRoles[0].ToString())
                            {
                                checkboxList.Items[iIndex+1].Selected = true;
                            }
                        }
                        else
                            checkboxList.Items[iIndex].Selected = false;
                    }
                }
                UserCount++;
            }
        }
       
        btnSaveWeekends.Visible = true;
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStaffGroupUsers);
            if (lblmessage.Text != null)
                lblmessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
    }

}