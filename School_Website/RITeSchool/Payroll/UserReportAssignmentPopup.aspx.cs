using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

/// <summary>
/// This class is sued to set user report assignment.
/// </summary>
public partial class UserReportAssignmentPopup : SchoolBase
{
    #region Constant(s)

    private string S_SUCCESS_MESSAGE = "User Report Assignment saved successfully !!!";
    
    #endregion

    #region Data Member(s)

    ReportsBL moReportsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            base.AddSortImage(lstvwUsers, "UserName", hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill report and user role combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            base.InitializeMemberVariables();
            moReportsBL = new ReportsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillUserRoles();
                FillReports();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user list view according to filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hidPageNo.Value = Constants.S_ONE;
        DataPager oDataPager = lstvwUsers.FindControl("DtPgDropDown") as DataPager;
        if (oDataPager != null)
        {
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt != null && ddlCnt.Items.Count > 0 && ddlCnt.SelectedValue != Constants.S_ONE)
            {
                ddlCnt.SelectedValue = Constants.S_ONE;
                cmbPageCnt_SelectedIndexChanged(ddlCnt, null);
            }
            else
                FillUsers();
        }
        else
            FillUsers();
    }

    /// <summary>
    /// This event is used to handle pager settings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_DataBound(object sender, EventArgs e)
    {
        try
        {   
            if (lstvwUsers.Items.Count > Constants.I_ZERO)
            {
                ControlUtility.FillListViewPagerFooter(lstvwUsers, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUsers);
            DataPager oDataPager = lstvwUsers.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
            {
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                if (ddlCnt != null)
                    hidPageNo.Value = ddlCnt.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on list view controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                Report oReport = oCurrentItem.DataItem as Report;

                HiddenField hidIsViewApplicable = oCurrentItem.FindControl("hidIsViewApplicable") as HiddenField;
                hidIsViewApplicable.Value = oReport.IsViewApplicable ? Constants.S_ONE : Constants.S_ZERO;

                CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
                chkSelect.Checked = oReport.HasAccess;
                chkSelect.Attributes.Add("Onclick", "SetState(this," + oCurrentItem.DisplayIndex + ",0)");

                CheckBox chkHasFullAccess = oCurrentItem.FindControl("chkHasFullAccess") as CheckBox;
                chkHasFullAccess.Checked = oReport.HasFullAccess;
                chkHasFullAccess.Enabled = oReport.IsViewApplicable;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            RevertSortOrder(hidSortDirection);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<Report> lstReports = new List<Report>();
            lstReports = (from currentItem in lstvwUsers.Items
                          let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                          let chkHasFullAccess = currentItem.FindControl("chkHasFullAccess") as CheckBox
                          let iReportUserDetailId = lstvwUsers.DataKeys[currentItem.DisplayIndex]["ReportUserDetailId"].ToInt()
                          where chkSelect.Checked || iReportUserDetailId != 0
                          select new Report
                          {
                              UserId = lstvwUsers.DataKeys[currentItem.DisplayIndex]["UserId"].ToInt(),
                              HasFullAccess = chkHasFullAccess.Checked,
                              IsDeleted = chkSelect.Checked ? 0 : 1
                          }).ToList();

            if (lstReports.Count > 0)
            {
                string sAssignmentXml = GenerateXml(lstReports);
                moReportsBL.SaveUserReportAssignment(cmbReport.SelectedValue.ToInt(), sAssignmentXml);
            }
            lblMassage.Text = S_SUCCESS_MESSAGE;

            FillUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
        base.SetDefaultButton(btnSearch);
        base.ApplyMouseHoverEffect(new List<Button> { btnSearch, btnSave, btnClose });
        btnClose.Attributes.Add("onclick", "window.close();");
        btnSave.Attributes.Add("onclick", "SetMesageState()");
        btnSearch.Attributes.Add("onclick", "SetMesageState()");
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbReport.Focus();
    }

    /// <summary>
    /// This method is used to fill report combo box.
    /// </summary>
    private void FillReports()
    {
        List<Report> lstReports = moReportsBL.GetAll(QueryString["ReportFolderId"].ToInt());
        ListSource.FillDropDownList(lstReports, cmbReport, "ReportName", "ReportId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill user role combo box.
    /// </summary>
    private void FillUserRoles()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDtUserRoles = oMasterDataCollectionBL.GetAllUserRoles();
        DataRow[] odrRoles = oDtUserRoles.Select("User_Role_Id IN (" + Constants.UserRoles.Teacher.ToInt() + "," + Constants.UserRoles.Supervisor.ToInt() + "," + Constants.UserRoles.OtherStaff.ToInt() + ")");
        if (odrRoles.Length > 0)
            ListSource.FillDropDownList(odrRoles.CopyToDataTable(), cmbUserRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill user list view.
    /// </summary>
    private void FillUsers()
    {
        lstvwUsers.DataSourceID = objdsUsers.ID;
        lstvwUsers.DataBind();

        btnSave.Visible = lstvwUsers.Items.Count > 0;
    }

    #endregion
}