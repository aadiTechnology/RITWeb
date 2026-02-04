/*File Name - StudentRecordStatusUI.aspx.cs
 * Created By - Sachin
 * Created Date - 4-Jun-2018
 * Description - This class is used to showing student record status.
 * 
 */
using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentRecordStatusUI : SchoolBase
{   
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
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "className";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            base.AddSortImage(lstvwStudents, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill class teacher combobox and fill student listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillClassTeachers();
                ReadQueryString();
            }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudents);
            DataPager oDataPager = lstvwStudents.FindControl("DtPgDropDown") as DataPager;
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
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_Sorting(object sender, ListViewSortEventArgs e)
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

    /// <summary>
    /// This event is used to fill student list as per filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            var oDtPgDropDown = lstvwStudents.FindControl("DtPgDropDown") as DataPager;
            if (oDtPgDropDown != null)
                oDtPgDropDown.SetPageProperties(0, oDtPgDropDown.PageSize, true);

            hidStdDivId.Value = cmbClasses.SelectedValue;
            hidFilter.Value = txtSearch.Text.Trim();
            hidIncludeRiseAndShine.Value = Convert.ToString(chkIncludeRiseAndShine.Checked);

            if (cmbClasses.SelectedValue == hidAssociatedClassId.Value)
                hidShowOnlysavedRecords.Value = Constants.S_ZERO;
            else
                hidShowOnlysavedRecords.Value = Constants.S_ONE;

            lstvwStudents.DataSourceID = objdsStudents.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is to set the listview footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudents.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStudents, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set image button propperties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                StudentRecordStatus oStudentRecordStatus = e.Item.DataItem as StudentRecordStatus;
                HyperLink hlnkEdit = e.Item.FindControl("hlnkEdit") as HyperLink;

                Image imgPrincipalStatus = e.Item.FindControl("imgPrincipalStatus") as Image;
                Image imgCounsellorStatus = e.Item.FindControl("imgCounsellorStatus") as Image;
                Label lblAction = e.Item.FindControl("lblAction") as Label;

                string sMessage = "-";

                if (oStudentRecordStatus.ReadyToReadCount > 0 && oStudentRecordStatus.ReadyToSubmitCount > 0)
                    sMessage = "Unread : " + oStudentRecordStatus.ReadyToReadCount + " & Unsubmitted : " + oStudentRecordStatus.ReadyToSubmitCount;
                else if (oStudentRecordStatus.ReadyToReadCount > 0 && oStudentRecordStatus.ReadyToSubmitCount == 0)
                    sMessage = "Unread : " + oStudentRecordStatus.ReadyToReadCount;
                else if (oStudentRecordStatus.ReadyToReadCount == 0 && oStudentRecordStatus.ReadyToSubmitCount > 0)
                    sMessage = "Unsubmitted : " + oStudentRecordStatus.ReadyToSubmitCount;                

                lblAction.Text = sMessage;

                SetActionButton(oStudentRecordStatus, hlnkEdit);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill class teacher combobox.
    /// </summary>
    private void FillClassTeachers()
    {
        bool bHasFullAccess = false;
        if (moUserRole == Constants.UserRoles.Admin || hidUserHasEditAccess.Value == Constants.S_YES)
            bHasFullAccess = true;

        StudentRecordBL oStudentRecordBL = new StudentRecordBL(miSchoolId, miAcademicYearId, miUserId);
        var oStatus = oStudentRecordBL.GetTeacherList(bHasFullAccess);

        hidIsPrincipal.Value = (oStatus.Item1 ? Constants.S_ONE : Constants.S_ZERO);
        hidIsCounsellor.Value = (oStatus.Item2 ? Constants.S_ONE : Constants.S_ZERO);
        hidAssociatedClassId.Value = oStatus.Item3.ToString();
        hidIsSubjectTeacher.Value = (oStatus.Item5 ? Constants.S_ONE : Constants.S_ZERO);

        ListSource.FillDropDownList(oStatus.Item4, cmbClasses, "TeacherName", "StdDivId", Constants.S_ALL);

        if (oStatus.Item4.Count == 1)
        {
            cmbClasses.SelectedIndex = 1;
            cmbClasses.Enabled = false;
        }
    }

    /// <summary>
    ///  This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidUserHasEditAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentRecords).ToString();
        hidSortExpression.Value = "className";
        hidSortDirection.Value = "ASC";
        base.SetDefaultButton(btnSearch);
    }

    /// <summary>
    ///  This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StdDivId"] != null && QueryString["StdDivId"].ToString() != string.Empty)
            cmbClasses.SelectedValue = QueryString["StdDivId"].ToString();

        if (QueryString["Filter"] != null && QueryString["Filter"].ToString() != string.Empty)
            txtSearch.Text = QueryString["Filter"].ToString();

        if (QueryString["ShowOnlyRiseAndShine"] != null && QueryString["ShowOnlyRiseAndShine"].ToString() != string.Empty)        
            chkIncludeRiseAndShine.Checked = QueryString["ShowOnlyRiseAndShine"] == "True" ? true : false;        

        btnSearch_Click(btnSearch, null);
    }
    
    /// <summary>
    ///  This method is used to set action button mode.
    /// </summary>
    /// <param name="aoStudentRecordStatus"></param>
    /// <param name="ahlnkEdit"></param>
    private void SetActionButton(StudentRecordStatus aoStudentRecordStatus, HyperLink ahlnkEdit)
    {
        if (!aoStudentRecordStatus.IsRecordFound)
        {
            ahlnkEdit.ImageUrl = "~/RITeSchool/images/IconGrid_Edit.gif";
            ahlnkEdit.ToolTip = "Add";
        }
        else
        {
            ahlnkEdit.ImageUrl = "~/RITeSchool/images/iconGridSml_ViewGE.gif";
            ahlnkEdit.ToolTip = "View / Edit";
        }

        int iIsReadMode = 1;
        if (hidStdDivId.Value == hidAssociatedClassId.Value)
            iIsReadMode = 0;

        ahlnkEdit.NavigateUrl = "StudentRecordUI.aspx?" + CommonUtility.EncryptQuerystring("SchoolwiseStudentId=" + aoStudentRecordStatus.SchoolwiseStudentId + "&StdDivId=" + hidStdDivId.Value + "&Filter=" + hidFilter.Value + "&ShowOnlySavedRecord="
            + hidShowOnlysavedRecords.Value + "&IsReadMode=" + iIsReadMode + "&IsPrincipal=" + hidIsPrincipal.Value + "&IsCounsellor=" + hidIsCounsellor.Value + "&ShowOnlyRiseAndShine=" + chkIncludeRiseAndShine.Checked + "&IsSubjectTeacher=" + hidIsSubjectTeacher.Value + "&IsClassTeacher=" + hidAssociatedClassId.Value);
    } 
    
    #endregion
}