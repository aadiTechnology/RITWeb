using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utility;
using BusinessLogic.PayrollBL;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Payroll;
using System.Reflection;
using System.Web.UI.HtmlControls;
using PayrollReportingUserEntities;

public partial class LeaveDeatilsUI : SchoolBase
{
    #region Data Member(s)

    UserApplyLeaveDetailsBL moUserApplyLeaveDetailsBL;

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserApplyLeaveDetailsBL = new UserApplyLeaveDetailsBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                CheckFullAccess();
                FillAcademicYear();
                FillCategory();
                ReadQueryString();
                FillApprovalCategories();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage master = this.Master as MasterPage;
            string sQueryString = CommonUtility.EncryptQuerystring("Id=" + 0 + "&CategoryId=" + 1 + "&UserName=" + Session[Constants.S_SESSION_USER_FULLNAME].ToString()+"&UserId="+miUserId);
            master.RedirectToNextPage("ApplyLeaveUI.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                UserApplyLeaveDetails oUserApplyLeaveDetails = oCurrentItem.DataItem as UserApplyLeaveDetails;
                Label lblStartDate = oCurrentItem.FindControl("lblStartDate") as Label;
                lblStartDate.Text = oUserApplyLeaveDetails.StartDate.ToString(Constants.S_DATE_FORMAT);


                Label lblEndDate = oCurrentItem.FindControl("lblEndDate") as Label;
                lblEndDate.Text = oUserApplyLeaveDetails.EndDate.ToString(Constants.S_DATE_FORMAT);                
                ImageButton imgDelete = oCurrentItem.FindControl("imgDelete") as ImageButton;

                imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                bool bIsApprovedByApprover = Convert.ToBoolean(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["IsApprovedByApprover"]);

                if (cmbReportingRole.SelectedValue != "1")
                    imgDelete.Visible = false;
                else
                    imgDelete.Visible = true;

                if (bIsApprovedByApprover)
                    imgDelete.Visible = false;

                if (!oUserApplyLeaveDetails.IsLeaveUpdatedInPayroll && cmbReportingRole.SelectedValue == "4") // all approved leaves
                {
                    Label lblUserName = e.Item.FindControl("lblUserName") as Label;
                    Label lblDescription = e.Item.FindControl("lblDescription") as Label;
                    Label lblTotalDays = e.Item.FindControl("lblTotalDays") as Label;
                    Label lblStatus = e.Item.FindControl("lblStatus") as Label;
                    Label lblLeaveType = e.Item.FindControl("lblLeaveType") as Label;
                    Label lblLeaveBalance = e.Item.FindControl("lblLeaveBalance") as Label;

                    lblUserName.Style.Add("color","navy");
                    lblStartDate.Style.Add("color", "navy");
                    lblEndDate.Style.Add("color", "navy");
                    lblDescription.Style.Add("color", "navy");
                    lblTotalDays.Style.Add("color", "navy");
                    lblStatus.Style.Add("color", "navy");
                    lblLeaveType.Style.Add("color", "navy");
                    lblLeaveBalance.Style.Add("color", "navy");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfiguration_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iLeaveCOnfigId = Convert.ToInt32(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["Id"]);
                int iUserId = Convert.ToInt32(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["UserId"]);

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moUserApplyLeaveDetailsBL.Delete(iLeaveCOnfigId);
                    FillApprovalCategories();
                    lblMessage.Text = "Leave record deleted successfully !!!";
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    Label lblUserName = e.Item.FindControl("lblUserName") as Label;
                    string sUserName = HttpUtility.HtmlDecode(lblUserName.Text);

                    MasterPage master = this.Master as MasterPage;
                    string sQueryString = CommonUtility.EncryptQuerystring("Id=" + iLeaveCOnfigId + "&CategoryId=" + cmbReportingRole.SelectedValue + "&HasFullAccess=" + hidHasFullAccess.Value + "&UserName=" + sUserName + "&UserId=" + iUserId);
                    master.RedirectToNextPage("ApplyLeaveUI.aspx?" + sQueryString);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfiguration_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfiguration.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwConfiguration, DtPgCount);

            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwConfiguration);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwConfiguration_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbReportingRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillLeaveDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillApprovalCategories();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void chkNonLeave_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillApprovalCategories();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void chknonupdatedrecords_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillApprovalCategories();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    private void FillCategory()
    {
        DataTable dt = moUserApplyLeaveDetailsBL.GetCategory(miUserId);
        if (hidHasFullAccess.Value != Constants.S_YES)
        {
            DataRow[] dr = dt.Select("Id=4");
            if (dr.Length > 0)
            {
                dr[0].Delete();
                dt.AcceptChanges();
            }
        }

        bool bAllowUserToViewAllLeaves = moUserApplyLeaveDetailsBL.AllowUserToViewAllLeaves();
        if (!bAllowUserToViewAllLeaves)
        {
            DataRow[] dr = dt.Select("Id=5");
            if (dr.Length > 0)
            {
                dr[0].Delete();
                dt.AcceptChanges();
            }
        }

        ListSource.FillDropDownList(dt, cmbReportingRole, "Category", "Id", string.Empty);
        cmbReportingRole.SelectedIndex = Constants.I_ZERO;
    }

    private void FillAcademicYear()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable dtAcademicYears = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId);
        
        var oDataItem = dtAcademicYears.AsEnumerable().Where(ay => ay.Field<string>("YearValue") == "2022-2023").Select(ay => ay.Field<int>("Academic_Year_Id")).FirstOrDefault();
        if (oDataItem != null)
            dtAcademicYears = dtAcademicYears.Select("Academic_Year_Id>=" + oDataItem.ToInt()).OrderByDescending(ay => ay.Field<int>("Academic_Year_Id")).CopyToDataTable();
        
        ListSource.FillDropDownList(dtAcademicYears, cmbAcademicYear, "YearValue", "Academic_Year_Id", Constants.S_SELECT);

        var iId = dtAcademicYears.AsEnumerable().Where(yr => yr.Field<string>("Is_Current_Year") == "Y").Select(ay => ay.Field<int>("Academic_Year_Id")).FirstOrDefault();
        if (iId != null)
        {
            ListItem oItem = cmbAcademicYear.Items.FindByValue(iId.ToString());
            if (oItem != null)
                oItem.Selected = true;
        }
    }

    private void ReadQueryString()
    {
        if (QueryString["CategoryId"] != null)
        {
            cmbReportingRole.SelectedValue = QueryString["CategoryId"];
            cmbReportingRole_SelectedIndexChanged(cmbReportingRole, null);
        }

        if (cmbReportingRole.SelectedValue == Constants.S_ONE)
            btnAdd.Visible = true;
        else
            btnAdd.Visible = false;
    }

    private void FillApprovalCategories()
    {
        lstvwConfiguration.DataSourceID = objdsPayments.ID;
        lstvwConfiguration.DataBind();

        if (lstvwConfiguration.Items.Count > 0)
        {
            HtmlTableRow tr = lstvwConfiguration.FindControl("trHeader") as HtmlTableRow;
            if (tr != null)
            {
                HtmlTableCell th = tr.FindControl("thDelete") as HtmlTableCell;
                if (th != null)
                {
                    if (cmbReportingRole.SelectedValue != "1")
                        th.Visible = false;
                    else
                        th.Visible = true;
                }
            }
        }
    }

    private void CheckFullAccess()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (moUserRole == Constants.UserRoles.Admin || lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.LeaveApprovalRejection.ToInt() && ru.UserId == miUserId))
            hidHasFullAccess.Value = Constants.S_YES;
        else
            hidHasFullAccess.Value = Constants.S_NO;
    }

    private void FillLeaveDetails()
    {
        if (cmbReportingRole.SelectedValue == Constants.S_ONE)
            btnAdd.Visible = true;
        else
            btnAdd.Visible = false;
        
        FillApprovalCategories();

        if (cmbReportingRole.SelectedValue == "4")
        {
            trLegend.Visible = true;
            trShowUpdated.Visible = true;
            chkNonLeave.Checked = true;
            chknonupdatedrecords.Checked = false;
        }
        else
        {
            trLegend.Visible = false;
            trShowUpdated.Visible = false;
            chkNonLeave.Checked = false;
            chknonupdatedrecords.Checked = true;
        }
    }

    #endregion    
}