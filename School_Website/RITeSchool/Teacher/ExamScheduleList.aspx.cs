/*  Created By :- Sachin
 *  Created Date :- 8-Sept-2009
 *  Class Description :- This class is used to display exam schedule of all the standards to teacher.
*/
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using eWorld.UI.Compatibility;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class ExamScheduleListUI : SchoolBase
{
    #region Constants

    const int I_EXAMDATE = 0;
    const int I_EXAMTIME = 1;
    const int I_INSTRUCTIONINDEX = 3;
    const string S_NOT_AVAILABLE = "Exam schedule is not available.";
    const string S_INSTRUCTION = "Instruction";    
    #endregion

    #region Member(s)
    ArrayList oArrExamDates = new ArrayList();
    DataTable moDataTable;
    #endregion

    #region Event(s)
    /// <summary>
    /// This event is used to generate collapsable panel and gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GenerateCollapsableGrids();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to modify grid cell values. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjectSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
            e.Row.Cells[2].Visible = false;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[I_EXAMDATE].Text = string.Empty;
                e.Row.Cells[I_EXAMTIME].Text = "Standards";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int iClumnCount = 0;
                string sExamDate = string.Empty;
                DateTime dtExamDate;
                string sDay = string.Empty;
                if (e.Row.Cells[I_EXAMDATE].Text.IndexOf(' ') != -1)
                {
                    sDay = e.Row.Cells[I_EXAMDATE].Text.Substring(0, e.Row.Cells[I_EXAMDATE].Text.IndexOf(' '));
                    dtExamDate = Convert.ToDateTime(e.Row.Cells[0].Text.Substring(e.Row.Cells[I_EXAMDATE].Text.IndexOf(' ')));
                    // "Instrcution" row - for that row only - 'default date' is bring from the database.
                    sExamDate = dtExamDate.ToString() == Constants.S_DEFAULT_DATE_4 ? "<B>" + S_INSTRUCTION + "</B>" : sDay + "<BR />" + dtExamDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
                    iClumnCount = e.Row.Cells.Count - 2;
                    if (!oArrExamDates.Contains(sExamDate))
                    {
                        e.Row.Cells[I_EXAMDATE].Text = sExamDate;
                        oArrExamDates.Add(sExamDate);
                    }
                    else
                        e.Row.Cells[I_EXAMDATE].Text = string.Empty;
                }
                TableCellCollection cells = e.Row.Cells;
                int iCount = 0;
                foreach (TableCell cell in cells)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {   
                        if (iCount == I_EXAMDATE)
                        {
                            cell.CssClass = "Clspadding";
                            cell.Width = Unit.Pixel(80);
                            cell.Wrap = false;
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            
                        }
                        else if (iCount == I_EXAMTIME)
                        {
                            cell.Width = Unit.Pixel(120);
                            cell.Wrap = false;
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            cell.CssClass = "GridDate";
                        }
                        else
                        {
                            cell.CssClass = "GridDate";
                            cell.Text = Server.HtmlDecode(cell.Text);
                            if (cell.Text == " ")
                                cell.Text = "--";                            
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            
                            // show tooltip to each cell of gridview.
                            if(e.Row.RowIndex != Constants.I_ZERO) 
                                cell.ToolTip ="Standard " +  moDataTable.Columns[iCount].ToString();
                            // Display instructions in BLUE color
                            if (sExamDate == "<B>Instruction</B>")
                                cell.ForeColor = Color.Blue;
                        }
                        if (e.Row.RowIndex == Constants.I_ZERO)
                        {
                            e.Row.CssClass = "ClsGridHeader";
                            e.Row.HorizontalAlign = HorizontalAlign .Center;
                            e.Row.Cells[I_EXAMDATE].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                        cell.HorizontalAlign = HorizontalAlign.Center;
                    iCount++;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set Instruction.
    /// </summary>
    /// <param name="aoDRInstruction"></param>
    /// <returns></returns>
    private string GenerateInstructionSet(DataRow[] aoDRInstruction)
    {
        string sInstruction = string.Empty;
        if (aoDRInstruction != null && aoDRInstruction.Length > 0)
        {
            foreach (DataRow oDataRow in aoDRInstruction)
                sInstruction += oDataRow["Instructions"] + "<BR />";
        }
        return sInstruction;
    }

    /// <summary>
    /// This method is used to generate collapsable panel.
    /// </summary>
    private void GenerateCollapsableGrids()
    {
        Boolean bIsCollapsedSet = false;
        Boolean bNextExamCollapsed = true;
        string spnlColor = string.Empty;
        DataSet odsExamSchedule = SchoolwiseStandardExamScheduleMasterBL.GetStandardwiseExamScheduleForTeacher(Session[Constants.S_SESSION_SCHOOL_ID].ToInt(), Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt());

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
                    // Don't delet this code.
                    string sInstructions = Convert.ToString(oDataRow["Instructions"]);
                    moDataTable = odsExamSchedule.Tables[iGridIndex];
                    CreateExamScheduleGrid(iGridIndex - 1, asTitle, sInstructions, bNextExamCollapsed);
                    bNextExamCollapsed = true;
                    oArrExamDates.Clear();
                }
                
            }
            ShowProgressSheetNote();
        }
        else
        {
            lblError.Text = S_NOT_AVAILABLE;
            pnlErrorMsg.Visible = true;
        }

    }

    /// <summary>
    /// This method is used to generate gridview.
    /// </summary>
    /// <param name="oDataTable"></param>
    /// <param name="aiGridCount"></param>
    /// <param name="asTitle"></param>
    /// <param name="asInstructions"></param>
    /// <param name="bIsCollapsed"></param>
    private void CreateExamScheduleGrid(int aiGridCount, string asTitle, string asInstructions, Boolean abIsCollapsed)
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
            oGridView.Columns.Add(oDataControlField);
        oGridView.DataSource = moDataTable;
        oGridView.DataBind();
        CollapsablePanel oCollapsablePanel = new CollapsablePanel();
        oCollapsablePanel.AllowSliding = colpnlSubjectSchedule.AllowSliding;
        oCollapsablePanel.AllowTitleExpandCollapse = colpnlSubjectSchedule.AllowTitleExpandCollapse;
        oCollapsablePanel.AllowTitleRowExpandCollapse = colpnlSubjectSchedule.AllowTitleRowExpandCollapse;
        oCollapsablePanel.BackColor = colpnlSubjectSchedule.BackColor;
        oCollapsablePanel.Collapsed = abIsCollapsed;
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

    /// <summary>
    /// This method is used to display progress sheet note.
    /// </summary>
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

    #endregion    
}
