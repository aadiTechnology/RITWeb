// File Name  : StandardwiseExamScheduleList.aspx.cs
// Created By : Anugandha
// Date       : 4/2/2008 
//Description : This class is used to view exam dates i.e. exam schedule.  

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using eWorld.UI.Compatibility;
using Utility;

public partial class StandardwiseExamScheduleList : SchoolBase
{
    #region Constant

    const Int32 I_SCHOOLWISE_STANDARD_EXAM_SCHEDULE_ID = 0;
    const string S_ERR_MSG_ADMIN = "Exams not yet configured.";
    const string S_ERR_MSG = "Exam Schedule not yet declared.";
    const int I_COLUMN_INDEX_START_DATE = 0;
    const int I_COLUMN_INDEX_START_TIME = 3;
    const int I_COLUMN_INDEX_END_TIME = 4;
    const int I_COLUMN_INDEX_TOTAL_TIME = 5;
   
    #endregion

    #region Data Members

    DataSet odsExamSchedule;

    #endregion

    #region Events
    /// <summary>
    /// This method use to aply master page if user is Admin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);
            if (moUserRole == Constants.UserRoles.Admin ||moUserRole==Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Supervisor)
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill grid view.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Supervisor)
                {
                    hidIsAdmin.Value = true.ToString();
                    lblStandard.Visible = true;
                    ddlStandard.Visible = true;
                    pnlTittle.Visible = true;
                    btnClose.Visible = true;
                    fillStandardComboBox();
                }
                else
                {
                    if (CheckPreCondition())
                    {
                        SetStandardAsPerLogin();
                        FillSubjectwiseExamScheduleGridview();
                        GenerateColapsableGrids();
                    }
                   // ApplyMouseHoverEffect(new List<Button>() { btnBack , btnClose });
                    
                }
            }
            ApplyMouseHoverEffect(new List<Button>() { btnClose });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is sused to generate colapsable panel.
    /// </summary>
    private void GenerateColapsableGrids()
    {
        Boolean bIsCollapsedSet = false;
        Boolean bNextExamCollapsed = true;
        string spnlColor = string.Empty;
        SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL();
        oSchoolwiseStandardExamScheduleMasterBL.School_Id = miSchoolId;
        oSchoolwiseStandardExamScheduleMasterBL.academic_Year_Id = miAcademicYearId;
        odsExamSchedule = oSchoolwiseStandardExamScheduleMasterBL.GetStandardwiseExamSchedule(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidDivisionID.Value));
        if ((odsExamSchedule != null) && odsExamSchedule.Tables.Count > 0 && odsExamSchedule.Tables[0].Rows.Count > 0)
        {
            int iGridIndex = 0;
            DataRow[] odrCollapsed = odsExamSchedule.Tables[0].Select("IsCollapsed='false'");
            foreach (DataRow oDataRow in odsExamSchedule.Tables[0].Rows)
            {
                iGridIndex++;
                if (odsExamSchedule.Tables[iGridIndex].Rows.Count > 0)
                {
                    spnlColor = "#ebedd9";
                    if (!bIsCollapsedSet)
                    {
                        if (odrCollapsed.Length > 0)
                        {
                            if (Convert.ToDateTime(oDataRow["Exam_Start_Date"]).DayOfYear <= DateTime.Today.DayOfYear &&
                                Convert.ToDateTime(oDataRow["Exam_End_Date"]).DayOfYear >= DateTime.Today.DayOfYear)
                            {
                                bNextExamCollapsed = false;
                                bIsCollapsedSet = true;
                                spnlColor = "#ccddb3";
                            }
                        }
                        else
                        {
                            if (((Convert.ToDateTime(oDataRow["Exam_Start_Date"]) - DateTime.Today).Days >= 1))
                            {
                                bNextExamCollapsed = false;
                                bIsCollapsedSet = true;
                                spnlColor = "#ebedd9";
                            }
                        }
                    }
                    string asTitle = "<b><font color='maroon' size='2'>" + Convert.ToString(oDataRow["SchoolWise_Test_Name"]) + " : " + "</font></b><font size='2'><B>" +
                        Convert.ToDateTime(oDataRow["Exam_Start_Date"]).ToString("dd MMM yyyy") + "</B> To <B>" + Convert.ToDateTime(oDataRow["Exam_End_Date"]).ToString("dd MMM yyyy") + "</B></font>";
                    string asInstructions = Convert.ToString(oDataRow["Instructions"]);
                    createNewGrid(odsExamSchedule.Tables[iGridIndex], iGridIndex - 1, asTitle, asInstructions, bNextExamCollapsed);
                    bNextExamCollapsed = true;
                }
            }
            ShowProgressSheetNote();
            pnlErrorMsg.Visible = false;
        }
        else
        {
            pnlErrorMsg.Visible = true;
            lblError.Text = "Exam schedule is not available.";
        }
    }

    public void ShowProgressSheetNote()
    {
        if (Settings.ShowProgressSheetNote)
        {
            String sProgressSheetNote = Settings.ProgressSheetNote;
            HtmlTable HeaderHtmlTable = new HtmlTable();
            HeaderHtmlTable.EnableViewState = false;
            HeaderHtmlTable.Width = "100%";
            HeaderHtmlTable.Border = 0;

            HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
            HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            HtmlTableCell oHtmlTableCell = new HtmlTableCell();

            Label oLabel = new Label();
            oLabel.Text = sProgressSheetNote;
            oLabel.CssClass = "LblSmlGray";
            oLabel.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
            oHtmlTableCell.Controls.Add(oLabel);
            oHtmlTableCell.Attributes.Add("class", "ClsBorderlight");

            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            HeaderHtmlTable.Rows.Add(oHtmlTableRow);
            oHtmlTableRow = new HtmlTableRow();
            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.InnerHtml = "&nbsp;";
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            HeaderHtmlTable.Rows.Add(oHtmlTableRow);
            LiteralControl oLiteralControl = new LiteralControl("<br />");
            oHtmlTableRow = new HtmlTableRow();
            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.Controls.Add(HeaderHtmlTable);
            oHtmlTableCell.Height = "6px";
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            tblGridsubjects.Rows.Add(oHtmlTableRow);
        }
    }
    private bool SetStandardAsPerLogin()
    {
        bool bReturn = false;
        if (moUserRole == Constants.UserRoles.Student ||
           (moUserRole == Constants.UserRoles.Teacher &&
           (Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString().Equals(Constants.C_YES.ToString()))))
        {
            if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Student)
            {
                hidStandardId.Value = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString();
                hidDivisionID.Value = Session[Constants.S_SESSION_STUDENT_DIVISION_ID].ToString();
                bReturn = true;
            }
            else// get teachers class
            {
                TeacherStandardDetailsBL oTeacher = new TeacherStandardDetailsBL();
                DataTable oDT = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetStandardDivisionOfTeacher(Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]), Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]));
                if (oDT.Rows.Count > 0)
                {
                    hidStandardId.Value = oDT.Rows[0]["Standard_Id"].ToString();
                    hidDivisionID.Value = oDT.Rows[0]["Division_ID"].ToString();
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }

        }

        if (!bReturn)
        {
            lblError.Visible = true;
            lblError.Text = "Access denied !! ";
        }
        return bReturn;
    }

    protected void grdSubjectSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[I_COLUMN_INDEX_START_TIME].Text == "12:00 AM" || e.Row.Cells[I_COLUMN_INDEX_START_TIME].Text == "08:18 AM")
                {
                    e.Row.Cells[I_COLUMN_INDEX_START_TIME].Text = "--";
                }
                if (e.Row.Cells[I_COLUMN_INDEX_END_TIME].Text == "12:00 AM" || e.Row.Cells[I_COLUMN_INDEX_END_TIME].Text == "08:18 AM")
                {
                    e.Row.Cells[I_COLUMN_INDEX_END_TIME].Text = "--";
                }
                if (e.Row.Cells[I_COLUMN_INDEX_START_TIME].Text == "--" &&
                    e.Row.Cells[I_COLUMN_INDEX_END_TIME].Text == "--")
                {
                    e.Row.Cells[I_COLUMN_INDEX_TOTAL_TIME].Text = "--";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        if (ddlStandard.SelectedIndex != 0)
        {
            hidStandardId.Value = ddlStandard.SelectedValue;
            FillSubjectwiseExamScheduleGridview();
            GenerateColapsableGrids();
        }
        else
        {
            hidStandardId.Value = "0";
            hidDivisionID.Value = "0";
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This function checks the preconditons for Standardwise Exam Schedule List.aspx configuration.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {

        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseExamScheduleConfig);

        if (!sLinks.Equals(""))
        {
            divErr.InnerHtml = sLinks;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;

    }
    private void fillStandardComboBox()
    {
        
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
    }
    #endregion

    #region Schedule

    /// <summary>
    ///  This method is used to fill subjectwise exam schedule.
    /// </summary>
    private void FillSubjectwiseExamScheduleGridview()
    {
        ShowDefaultGridFormat();
    }

    /// <summary>
    /// This method is used to create new grid scheema.
    /// </summary>
    private void createNewGrid(DataTable oDataTable, int aiGridCount, string asTitle, string asInstructions, Boolean bIsCollapsed)
    {
        GridView oGridView = new GridView();
        oGridView.Visible = true;
        oGridView.EnableViewState = false;
        oGridView.CssClass = "GridBorder";
        oGridView.Width = grdSubjectSchedule.Width;
        oGridView.AutoGenerateColumns = grdSubjectSchedule.AutoGenerateColumns;
        oGridView.PageSize = grdSubjectSchedule.PageSize;
        oGridView.AllowPaging = grdSubjectSchedule.AllowPaging;
        oGridView.CellPadding = grdSubjectSchedule.CellPadding;
        oGridView.CellSpacing = grdSubjectSchedule.CellSpacing;
        oGridView.ForeColor = grdSubjectSchedule.ForeColor;
        oGridView.GridLines = grdSubjectSchedule.GridLines;
        oGridView.DataKeyNames = grdSubjectSchedule.DataKeyNames;
        oGridView.RowStyle.CssClass = grdSubjectSchedule.RowStyle.CssClass;
        oGridView.HeaderStyle.CssClass = grdSubjectSchedule.HeaderStyle.CssClass;
        oGridView.AlternatingRowStyle.CssClass = grdSubjectSchedule.AlternatingRowStyle.CssClass;
        oGridView.EmptyDataRowStyle.CssClass = grdSubjectSchedule.EmptyDataRowStyle.CssClass;
        oGridView.RowDataBound += new GridViewRowEventHandler(grdSubjectSchedule_RowDataBound);
        foreach (DataControlField oDataControlField in grdSubjectSchedule.Columns.CloneFields())
        {
            oGridView.Columns.Add(oDataControlField);
        }
        oGridView.DataSource = oDataTable;
        oGridView.DataBind();
        CollapsablePanel oCollapsablePanel = new CollapsablePanel();
        oCollapsablePanel.AllowSliding = colpnlSubjectSchedule.AllowSliding;
        oCollapsablePanel.AllowTitleExpandCollapse = colpnlSubjectSchedule.AllowTitleExpandCollapse;
        oCollapsablePanel.AllowTitleRowExpandCollapse = colpnlSubjectSchedule.AllowTitleRowExpandCollapse;
        oCollapsablePanel.BackColor = colpnlSubjectSchedule.BackColor;
        oCollapsablePanel.Collapsed = bIsCollapsed;
        oCollapsablePanel.CollapsedTitleStyle = colpnlSubjectSchedule.CollapsedTitleStyle;
        oCollapsablePanel.Collapsable = colpnlSubjectSchedule.Collapsable;
        oCollapsablePanel.TitleStyle.CssClass = colpnlSubjectSchedule.TitleStyle.CssClass;
        oCollapsablePanel.CollapseImageUrl = colpnlSubjectSchedule.CollapseImageUrl;
        oCollapsablePanel.CollapserAlign = colpnlSubjectSchedule.CollapserAlign;
        oCollapsablePanel.CollapseText = colpnlSubjectSchedule.CollapseText;
        oCollapsablePanel.CssClass = colpnlSubjectSchedule.CssClass;
        oCollapsablePanel.DefaultButton = colpnlSubjectSchedule.DefaultButton;
        oCollapsablePanel.Direction = colpnlSubjectSchedule.Direction;
        oCollapsablePanel.Enabled = colpnlSubjectSchedule.Enabled;
        oCollapsablePanel.ExpandImageUrl = colpnlSubjectSchedule.ExpandImageUrl;
        oCollapsablePanel.ExpandText = colpnlSubjectSchedule.ExpandText;
        oCollapsablePanel.ForeColor = colpnlSubjectSchedule.ForeColor;
        oCollapsablePanel.HorizontalAlign = colpnlSubjectSchedule.HorizontalAlign;
        oCollapsablePanel.ScrollBars = colpnlSubjectSchedule.ScrollBars;
        oCollapsablePanel.ShowLinkOrImage = colpnlSubjectSchedule.ShowLinkOrImage;
        oCollapsablePanel.SlideLines = colpnlSubjectSchedule.SlideLines;
        oCollapsablePanel.SlideSpeed = colpnlSubjectSchedule.SlideSpeed;
        oCollapsablePanel.TitleText = asTitle;
        oCollapsablePanel.Controls.Add(oGridView);
        HtmlTable oHtmlTable = new HtmlTable();
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = asInstructions.Replace("\r\n", "<BR/>");
        oHtmlTableCell.Attributes.Add("class", "LblNormal");
        oHtmlTableCell.NoWrap = false;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTable.Rows.Add(oHtmlTableRow);
        oCollapsablePanel.Controls.Add(oHtmlTable);
        oHtmlTableRow = new HtmlTableRow();
        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Controls.Add(oCollapsablePanel);
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        tblGridsubjects.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Height = "6px";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        tblGridsubjects.Rows.Add(oHtmlTableRow);
    }

    private void ShowDefaultGridFormat()
    {
        BoundField oStartDate = (BoundField)grdSubjectSchedule.Columns[I_COLUMN_INDEX_START_DATE];
        oStartDate.HtmlEncode = false;
        oStartDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        oStartDate = (BoundField)grdSubjectSchedule.Columns[I_COLUMN_INDEX_START_TIME];
        oStartDate.HtmlEncode = false;
        oStartDate.DataFormatString = Constants.S_STANDARD_GRID_TIME_FORMAT;
        oStartDate = (BoundField)grdSubjectSchedule.Columns[I_COLUMN_INDEX_END_TIME];
        oStartDate.HtmlEncode = false;
        oStartDate.DataFormatString = Constants.S_STANDARD_GRID_TIME_FORMAT;
    }
    #endregion schedule
   
}
