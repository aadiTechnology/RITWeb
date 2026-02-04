/* File Name - LessonPlanUI.aspx.cs
 * Created By -Sachin
 * Created Date - 16 Jun 2015
 * Description - This class is used to display lesson plans..jm
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using LessonPlanEntities;
using Utility;
using System.Text;
using System.Web.UI;
using System.Web.Services;

public partial class LessonPlanUI : SchoolBase
{
    #region Data Member(s)

    private LessonPlanDetailsBL moLessonPlanDetailsBL;
    private List<LessonPlanReportingConfig> mlstReportingStatus; 

    #endregion

    #region Data Member(s)

    /// <summary>
    /// This event is used to read query string, fill teacher combo box and fill lesson plan list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moLessonPlanDetailsBL = new LessonPlanDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillTeacherDetails();
                SetJavascriptAttributes();
                ReadQueryString();
                FillLessonPlans();
                SetAddButtonStatus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill lesson plans in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeacher_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillLessonPlans();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill lesson plans in list view between selected Date span.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void calDates_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                FillLessonPlans();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwLessonPlan);
            DataPager oDataPager = lstvwLessonPlan.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
            {
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                if (ddlCnt != null)
                    hidPageNo.Value = ddlCnt.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set pager settings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLessonPlan_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwLessonPlan.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwLessonPlan, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to open lesson plan page to add new lesson plan.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = CommonUtility.EncryptQuerystring("UserId=" + cmbTeacher.SelectedValue + "&StartDate=" + DateTime.MinValue + "&EndDate=" + DateTime.MinValue);
            MasterPage oMaster = this.Master as MasterPage;
            oMaster.RedirectToNextPage("LessonPlanApprovalUI.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set visibility of controls in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLessonPlan_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LessonPlanConfig oLessonPlanConfig = e.Item.DataItem as LessonPlanConfig;

                Label lblStartDate = e.Item.FindControl("lblStartDate") as Label;
                lblStartDate.Text = oLessonPlanConfig.StartDate.ToString(Constants.S_DATE_FORMAT);

                Label lblEndDate = e.Item.FindControl("lblEndDate") as Label;
                lblEndDate.Text = oLessonPlanConfig.EndDate.ToString(Constants.S_DATE_FORMAT);

                HtmlTableCell tdView = e.Item.FindControl("tdView") as HtmlTableCell;

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                
                if (cmbTeacher.SelectedValue.ToInt() != miUserId)
                {
                    if (oLessonPlanConfig.IsSubmitedByReportingUser)
                        btnDelete.Visible = false;
                    else
                        btnDelete.Visible = true;

                    //HtmlTableCell oCell = e.Item.FindControl("tdDelete") as HtmlTableCell;
                    //if (oCell != null)
                    //    oCell.Visible = false;

                    HtmlTableCell tdEdit = e.Item.FindControl("tdEdit") as HtmlTableCell;
                    if (tdEdit != null)
                        tdEdit.Visible = false;

                    if (tdView != null)
                        tdView.Visible = true;
                }
                else
                {   
                    btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                    if (oLessonPlanConfig.IsSubmitted)
                        btnDelete.Visible = false;
                    else
                        btnDelete.Visible = true;

                    if (tdView != null)
                        tdView.Visible = false;
                }

                LinkButton lbtnExport = e.Item.FindControl("lbtnExport") as LinkButton;
                if (lbtnExport != null)
                    lbtnExport.Visible = oLessonPlanConfig.IsSubmitted;

                mlstReportingStatus = ViewState["ReportingStatus"] as List<LessonPlanReportingConfig>;

                ImageButton imgRemarkButton = e.Item.FindControl("btnViewRemarks") as ImageButton;
                Label lblRemarks = e.Item.FindControl("lblRemarks") as Label;

                if (mlstReportingStatus.Any(rs => rs.IsSubmitted && rs.ReportingUserId != oLessonPlanConfig.UserId && rs.StartDate == oLessonPlanConfig.StartDate && rs.EndDate == oLessonPlanConfig.EndDate))
                {
                    imgRemarkButton.Visible = true;
                    lblRemarks.Visible = false;                                        

                    if (oLessonPlanConfig.UserId == miUserId)
                    {
                        imgRemarkButton.Attributes.Add("onclick", "if(!CheckForSuggisition('" + oLessonPlanConfig.Remarks + "'" + ", " + "'" + oLessonPlanConfig.IsSuggestionAdded + "'" + ", '" + oLessonPlanConfig.IsSuggestionRead + "'" + ", '" + cmbTeacher.SelectedValue + "'" + ", '" + lblStartDate.Text + "'" + ", '" + lblEndDate.Text + "'" + ", '" + miSchoolId + "'" + ", '" + miAcademicYearId + "'" + ", '" + miUserId + "')) return false;");                                   
                        if (oLessonPlanConfig.IsSuggestionAdded && !oLessonPlanConfig.IsSuggestionRead)
                        {
                            lblStartDate.ForeColor = System.Drawing.Color.Blue;
                            lblStartDate.Font.Bold = true;
                            lblEndDate.ForeColor = System.Drawing.Color.Blue;
                            lblEndDate.Font.Bold = true;
                            lbtnExport.ForeColor = System.Drawing.Color.Blue;
                            lbtnExport.Font.Bold = true;
                        }
                    }
                    else
                        imgRemarkButton.Attributes.Add("onclick", "OpenPopup('" + oLessonPlanConfig.Remarks + "'); return false;");

                }
                else
                {
                    imgRemarkButton.Visible = false;
                    lblRemarks.Visible = true;                    
                }

                HtmlTableCell tdStatus = e.Item.FindControl("tdStatus") as HtmlTableCell;
                if (tdStatus != null)
                {
                    HtmlTable oHtmlTable = new HtmlTable();
                    HtmlTableRow trTable = new HtmlTableRow();
                    mlstReportingStatus.Where(usr => usr.StartDate == oLessonPlanConfig.StartDate && usr.EndDate == oLessonPlanConfig.EndDate).OrderBy(usr => usr.ApprovalSortOrder).ToList().ForEach(
                        usr =>
                        {
                            HtmlTableCell tdImageRow = new HtmlTableCell();
                            Image imgStatus = new Image();
                            Label lbl = new Label();
                                  
                            imgStatus.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";
                            if (usr.IsSubmitted)
                            {
                                imgStatus.ImageUrl = "../images/IconGrid_AssignTrue.gif";
                                tdImageRow.Controls.Add(imgStatus);
                            }
                            else
                            {
                                if (usr.MaxDate < oLessonPlanConfig.EndDate || usr.MinDate > oLessonPlanConfig.StartDate)
                                {                                    
                                    tdImageRow.Style.Add("Horizontal-align", "center");
                                    lbl.Style["text-align"] = "center";
                                    lbl.Text = "-";
                                    tdImageRow.Controls.Add(lbl);
                                }    
                                else
                                    tdImageRow.Controls.Add(imgStatus);
                            }
                            tdImageRow.Width = "20px";
                            tdImageRow.Style["text-align"] = "center";
                            imgStatus.ToolTip = usr.ReportingUserName;
                            lbl.ToolTip = usr.ReportingUserName;
                            trTable.Controls.Add(tdImageRow);                          
                        }

                      );                    
                    oHtmlTable.Rows.Add(trTable);
                    tdStatus.Controls.Add(oHtmlTable); 
                }

               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update, view and export lesson plan details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLessonPlan_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblStartDate = e.Item.FindControl("lblStartDate") as Label;
                Label lblEndDate = e.Item.FindControl("lblEndDate") as Label;
                int iId = lstvwLessonPlan.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moLessonPlanDetailsBL.DeleteConfiguration(cmbTeacher.SelectedValue.ToInt(), lblStartDate.Text.ToDateTime(), lblEndDate.Text.ToDateTime());
                    base.DisplayMessage("Lesson Plan deleted successfully !!!", false, tdMessage);
                    FillLessonPlans();
                }
                else if (e.CommandName == "VIEW" || e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    string sQueryString = CommonUtility.EncryptQuerystring("UserId=" + cmbTeacher.SelectedValue + "&StartDate=" + lblStartDate.Text + "&EndDate=" + lblEndDate.Text);
                    MasterPage oMaster = this.Master as MasterPage;
                    oMaster.RedirectToNextPage("LessonPlanApprovalUI.aspx?" + sQueryString);
                }
                else if (e.CommandName == "EXPORT")
                {
                    string sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + miSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + miAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + cmbTeacher.SelectedValue +
                    " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + lblStartDate.Text + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + lblEndDate.Text + ")" + "  AND  usp_GetLessonPlanDetailsForReport.StandardDivisionId}=0) AND  usp_GetLessonPlanDetailsForReport.SubjectId}=0)" + "@ ";

                    ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.LessonPlan, sRecordSelectionFormula, ExportFormatType.PortableDocFormat);
                    oReportDisplay.DisplayReport();
                }                
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    [WebMethod]
    public static void UpdateStatus(int aiTeacherId, DateTime adtStartDate, DateTime adtEndDate, int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
    {
        LessonPlanDetailsBL oLessonPlanDetailsBL = new LessonPlanDetailsBL();
        oLessonPlanDetailsBL.UpdateReadSuggestion(aiTeacherId, adtStartDate, adtEndDate, aiSchoolId, aiAcademicYearId, aiUpdatedById);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string sRecordSelectionFormula = string.Empty;
            if (txtStartDate.Text == string.Empty && txtEndDate.Text == string.Empty)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + miSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + miAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + cmbTeacher.SelectedValue + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=0" + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=0" +
                        " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
            }
            else if (txtStartDate.Text != string.Empty && txtEndDate.Text == string.Empty)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + miSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + miAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + cmbTeacher.SelectedValue + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=0" + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=0" +
                       " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + txtStartDate.Text + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
            }
            else if (txtStartDate.Text == string.Empty && txtEndDate.Text != string.Empty)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + miSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + miAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + cmbTeacher.SelectedValue + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=0" + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=0" +
                       " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + txtEndDate.Text + ")" + "@ ";
            }
            else
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + miSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + miAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + cmbTeacher.SelectedValue + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=0" + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=0" +
                       " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + txtStartDate.Text + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + txtEndDate.Text + ")" + "@ ";
            }

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.LessonPlan, sRecordSelectionFormula, ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["UserId"] != null)
        {
            cmbTeacher.SelectedValue = QueryString["UserId"].ToString();
            cmbTeacher_SelectedIndexChanged(cmbTeacher, null);
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnAdd });
    }

    /// <summary>
    /// This method is used to fill up teacher's details.
    /// </summary>
    private void FillTeacherDetails()
    {
        List<TeacherDetails> lstTeachers = new List<TeacherDetails>();
        string sFullAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.LessonPlanScreenFullAccess).ToString();


        if (moUserRole == Constants.UserRoles.Admin)
            sFullAccess = Constants.S_YES;

        lstTeachers = moLessonPlanDetailsBL.GetAllTeachers(sFullAccess);
        ListSource.FillDropDownList(lstTeachers, cmbTeacher, "Name", "UserId", Constants.S_SELECT);

        ListItem oListItem = cmbTeacher.Items.FindByValue(miUserId.ToString());
        if (oListItem != null)
        {
            oListItem.Selected = true;
            cmbTeacher_SelectedIndexChanged(cmbTeacher, null);

            if (lstTeachers.Count == 1)
            {
                tdTeacher.Visible = false;
                tdTeacherHeader.Visible = false;
            }
            else
            {
                tdTeacher.Visible = true;
                tdTeacherHeader.Visible = true;
            }
        }
    }

    /// <summary>
    /// This method is used to fill lesson plans.
    /// </summary>
    private void FillLessonPlans()
    {
        DateTime dtStartDate;
        DateTime dtEndDate;
        if (txtStartDate.Text != string.Empty && txtEndDate.Text != string.Empty)
        {
            dtStartDate = txtStartDate.Text.ToDateTime();
            dtEndDate = txtEndDate.Text.ToDateTime();
            if (dtStartDate > dtEndDate)
            {
                base.DisplayMessage("End Date should be greater than Start Date.", true, tdMessage);
                btnExport.Enabled = false;
            }
            else
                FillLessonPlanDetails();
        }
        else
            FillLessonPlanDetails();
    }

    private void FillLessonPlanDetails()
    {
        mlstReportingStatus = moLessonPlanDetailsBL.GetAllReportingConfigs(cmbTeacher.SelectedValue.ToInt());
        ViewState["ReportingStatus"] = mlstReportingStatus;
        lstvwLessonPlan.DataSourceID = objdsLessonPlan.ID;
        lstvwLessonPlan.DataBind();

        if (lstvwLessonPlan.Items.Count == Constants.I_ZERO)
            btnExport.Enabled = false;
        else
            btnExport.Enabled = true;

        if (cmbTeacher.SelectedValue.ToInt() != miUserId)
            SetTeacherView(false);
        else
            SetTeacherView(true);
    }

    /// <summary>
    /// This method is used to set teacher's view.
    /// </summary>
    /// <param name="abStatus"></param>
    private void SetTeacherView(bool abStatus)
    {
        HtmlTableRow tr = lstvwLessonPlan.FindControl("trHeader") as HtmlTableRow;
        if (tr != null)
        {
            HtmlTableCell thDelete = tr.FindControl("thDelete") as HtmlTableCell;
            HtmlTableCell thEdit = tr.FindControl("thEdit") as HtmlTableCell;
            HtmlTableCell thView = tr.FindControl("thView") as HtmlTableCell;
            
            if (thView != null)
                thView.Visible = !abStatus;

            if (thEdit != null)
                thEdit.Visible = abStatus;

            //if (thDelete != null)
            //    thDelete.Visible = abStatus;
        }

        btnAdd.Visible = abStatus;
    }

    /// <summary>
    /// This method is used to set add button status.
    /// </summary>
    private void SetAddButtonStatus()
    {
        btnAdd.Visible = false;
        if (cmbTeacher.SelectedValue.ToInt() == miUserId)
            btnAdd.Visible = true;
    } 

    #endregion    
}
