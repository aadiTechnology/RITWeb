using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using System.Data;
using Utility;
using XseedReportEntities;
using System.Web.UI.HtmlControls;
using System.Reflection;

public partial class DescriptiveIndicatorsUI : SchoolBase
{
    #region Constant's

    const string S_DEFAULT_SORT_EXP = "Roll_No";
    private const string S_SORT_ROW = "SortRow";

    private const string S_COMMAND_PUBLISH = "Publish";
    private const string S_TEXT_PUBLISH = "Publish";
    private const string S_TEXT_UNPUBLISH = "UnPublish";
    private const string S_PUBLISH_MESSAGE = "Descriptive Indicators published successfully !!!";
    private const string S_UNPUBLISH_MESSAGE = "Descriptive Indicators un-published successfully !!!";

    #endregion

    #region DataMember

    private DescriptiveIndicatorBL moDescriptiveIndicatorBL;

    #endregion

    #region Event's

    // <summary>
    // This event is used to add sort image.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param>    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = S_DEFAULT_SORT_EXP;
                hidSortDirection.Value = Constants.S_ASCENDING;
            }
            base.AddSortImage(lstvwStudentDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used load the default controls.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moDescriptiveIndicatorBL = new DescriptiveIndicatorBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillClassTeacherCombo();
                FillTermComboBox();
                SetJavascriptAttributes();
                ReadQueryString();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used to change the value of Class teacher combobox.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void cmbClassTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LegendTable.Visible = true;
            hidTeacherId.Value = cmbClassTeachers.SelectedValue;
            FillStudentsListView();
            CheckPublishStatus();
        }   
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used to change the term combobox value.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void CmbTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LegendTable.Visible = true;
            hidTermId.Value = CmbTerm.SelectedValue;
            FillStudentsListView();
            CheckPublishStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used load the data to listview
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string sQueryString = string.Empty;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                HyperLink ohyplnkEdit = ((HyperLink)(oCurrentItem.FindControl("hyplnkEdit")));
                Button btnPublish = ((Button)(oCurrentItem.FindControl("btnPublish")));
                Label lblPublishStatus = ((Label)(oCurrentItem.FindControl("lblPublishStatus")));
                int iEditStatus = lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["EditStatus"].ToInt();
                int iPublish = lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["IsPublished"].ToInt();

                if (iEditStatus == Constants.I_THREE)
                {
                    btnPublish.Visible = true;
                    if (iPublish == Constants.I_ONE)
                    {
                        btnPublish.Text = S_TEXT_UNPUBLISH;
                        lblPublishStatus.Visible = false;
                    }
                    else
                    {
                        btnPublish.Text = S_TEXT_PUBLISH;
                        lblPublishStatus.Visible = false;
                    }
                }
                else
                {
                    btnPublish.Visible = false;
                    lblPublishStatus.Visible = true;
                }
             
                
                ohyplnkEdit.ImageUrl = "../images/IconGrid_Edit.GIF";

                SetEditStatus(iEditStatus, ohyplnkEdit);
                sQueryString = "&StudentId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString() +
                               "&StandardId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString() + 
                               "&TermId=" + CmbTerm.SelectedValue.ToString() +
                               "&StdDivId=" + cmbClassTeachers.SelectedValue.ToString();

                ohyplnkEdit.NavigateUrl = "~/RITeSchool/DescriptiveIndicators/AssignDescriptiveIndicatorMarksUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used Item command of listview.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void lstvwStudentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iYearwiseStudentId = Convert.ToInt32(lstvwStudentDetails.DataKeys[e.Item.DisplayIndex]["YearwiseStudentId"]);
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                Button btnPublish = ((Button)(oCurrentItem.FindControl("btnPublish")));
                if (e.CommandName == S_COMMAND_PUBLISH)
                {
                    if (btnPublish.Text == S_TEXT_PUBLISH)
                    {
                        PublishMarks(iYearwiseStudentId, Constants.I_ONE);
                        FillStudentsListView();
                    }
                    else
                    {
                        PublishMarks(iYearwiseStudentId, Constants.I_ZERO);
                        FillStudentsListView();
                    }
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillStudentsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }    

    // <summary>
    // This event is used to DataBound.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void lstvwStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStudentDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    // <summary>
    // This event is used for listview sorting.
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param> 
    protected void lstvwStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if(hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillStudentsListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            LegendTable.Visible = true;
            if (btnPublish.Text == S_TEXT_PUBLISH)
            {
                PublishMarks(Constants.I_ZERO, Constants.I_ONE);
                FillStudentsListView();
                btnPublish.Text = S_TEXT_UNPUBLISH;                
            }
            else
            {
                PublishMarks(Constants.I_ZERO, Constants.I_ZERO);
                btnPublish.Text = S_TEXT_PUBLISH;
                FillStudentsListView();                
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    // <summary>
    // This method is used for Fill Student listview.
    // </summary>
    private void FillStudentsListView()
    {        
        lstvwStudentDetails.DataSourceID = ObjDSStudentDetails.ID;
        lstvwStudentDetails.DataBind();
    }

    // <summary>
    // This method is used for Fill Class Teacher combobox.
    // </summary>
    private void FillClassTeacherCombo()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDtClassTeacher = oMasterDataCollectionBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);

        char cHadEditAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.DescriptiveIndicators);

        if (moUserRole == Constants.UserRoles.Teacher && cHadEditAccess != 'Y')
        {
            DataRow[] oDataRow = oDtClassTeacher.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID]);
            ControlUtility.FillDropDownList(
                       oDataRow,
                       ref cmbClassTeachers,
                       Constants.S_STANDARD_DIVISION_ID_FIELD,
                       Constants.S_TEACHER_NAME_FIELD,
                       Constants.S_SELECT);
            if (oDataRow.Length == 1)
            {
                cmbClassTeachers.SelectedIndex = 1;
                cmbClassTeachers.Enabled = false;
                cmbClassTeachers_SelectedIndexChanged(cmbClassTeachers, null);
            }
        }
        else
        {
            ListSource.FillDropDownList(oDtClassTeacher, cmbClassTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_SELECT);
        }

        //ListSource.FillDropDownList(oDtClassTeacher, cmbClassTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_SELECT);
    }

    // <summary>
    // This method is used for Fill term Combobox.
    // </summary>
    private void FillTermComboBox()
    {        
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref CmbTerm, "Value_Member", "Display_Member", string.Empty);
        hidTermId.Value = CmbTerm.SelectedValue;
    }

    // <summary>
    // This method is used for Add sort image.
    // </summary>
    private void AddSortImage()
    {
        hidSortDirection.Value = (lstvwStudentDetails.SortDirection.ToString() == "Ascending" || lstvwStudentDetails.SortDirection.ToString() == string.Empty) ? Constants.S_ASCENDING : Constants.S_DESCENDING;
        hidSortExpression.Value = (lstvwStudentDetails.SortExpression != string.Empty) ? lstvwStudentDetails.SortExpression.ToString() : S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    // <summary>
    // This method is used for set java script attributes to the control.
    // </summary>
    private void SetJavascriptAttributes()
    {
        btnPublish.Enabled = false;
        base.ApplyMouseHoverEffect(new List<Button> { btnPublish });
    }

    // <summary>
    // This method is used for set Edit Status for control.
    // </summary>
    private void SetEditStatus(int iCanEdit, HyperLink hyplnkEdit)
    {
        switch ((Constants.MarkAssignmentStatus)iCanEdit)
        {
            case Constants.MarkAssignmentStatus.NotAssigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_NONE_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryNotStarted;
                break;
            case Constants.MarkAssignmentStatus.PartiallyAssigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_PARTIAL_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryPartiallyDone;
                break;
            case Constants.MarkAssignmentStatus.Assigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_COMPLETE_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
                break;
        }
    }

    // <summary>
    // This method is used to publish the Descriptive Indicators Marks.
    // </summary>
    private void PublishMarks(int iYearwiseStudentId, int iPublish)
    {
        moDescriptiveIndicatorBL.PublishDescriptiveIndecators(iYearwiseStudentId, CmbTerm.SelectedValue.ToInt(), iPublish, cmbClassTeachers.SelectedValue.ToInt());
    }

    // <summary>
    // This method is used to check the Publish status.
    // </summary>
    private void CheckPublishStatus()
    { 
        int iPublishStatus = 0;
        int iPublished = 0;
        moDescriptiveIndicatorBL.CheckPublishStatus(cmbClassTeachers.SelectedValue.ToInt(), CmbTerm.SelectedValue.ToInt(), out iPublishStatus, out iPublished);

        if (iPublishStatus == Constants.I_ONE)
            btnPublish.Enabled = true;
        else
            btnPublish.Enabled = false;

        if (iPublished == Constants.I_ONE)
            btnPublish.Text = S_TEXT_UNPUBLISH;
        else
            btnPublish.Text = S_TEXT_PUBLISH;
    }

    // <summary>
    // This method is used to Read the query string.
    // </summary>
    private void ReadQueryString()
    {
        if (QueryString["StdDivId"] != null)
            cmbClassTeachers.SelectedValue = QueryString["StdDivId"].ToString();
        if (QueryString["TermId"] != null)
        {
            CmbTerm.SelectedValue = QueryString["TermId"].ToString();
            hidTermId.Value = CmbTerm.SelectedValue;
            cmbClassTeachers_SelectedIndexChanged(cmbClassTeachers, null);
        }
        else
            FillStudentsListView();
    }

#endregion   
}