using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using System.Resources;
using Utility;

/// <summary>
/// This class displays the complete Timetable for school.
/// The format of TimeTable :
/// Timetable is divided into no. of panel each displaying the Timetable for fixed no. of teachers
/// ( "I_NO_RECS"  is a constant defined to define the panel size. )
/// </summary>
public partial class SchoolTimeTable : SchoolBase
{
    #region Constants

    private const string S_CSS_OUTERTBL = "ReportOuter";
    private const string S_CSS_DATACELL = "TTCellsTchr";
    private const string S_CSS_SEPROW = "TTSepRow";
    private const string S_CSS_NA = "UsrTTNA";
    private const string S_CSS_NOTCLASS_TEACHER = "TTNotAssignDark";
    private const string S_CSS_GRIDHEAD = "UsrGridHead";
    private const string S_CSS_WEEKDAY = "WeekDayHead";
    private const string S_CSS_ALT_WEEKDAY = "AltWeekdayHead";

    private const string S_CSS_WEEKDAY_CELL = "WeekDCell";
    private const string S_CSS_ALT_WEEKDAY_CELL = "AltWeekDCell";

    private const string S_FEILD_WEEKDAYNAME = "WeekDay_Name";
    private const string S_FEILD_TEACHERNAME = "TeacherName";
    private const string S_FEILD_MPT_APPLICABLE = "MPT_Applicable";
    private const string S_FEILD_ASSEMBLY_APPLICABLE = "Assembly_Applicable";
    private const string S_FEILD_STAYBACK_APPLICABLE = "Stayback_Applicable";
    private const string S_FIELD_WEEKLYTEST_APPLICABLE = "WeeklyTestApplicable";

    private const string S_HEADER_TEACHERNAME = "Teacher Name";
    private const int I_TEACHER_TBL_INDEX = 1;
    private const int I_WEEKDAY_TBL_INDEX = 0;
    private const int I_TT_TBL_INDEX = 2;
    private const int I_TEACHER_LECTURE_TBL_INDEX = 3;
    private const int I_TEACHER_CELL_INDEX = 0;
    private const int I_NO_RECS = 8; // block size. defines no. of of teachers in  1 one block.
    #endregion

    #region Data members
    //Dataset contains
    // table 0: Teacher id and name
    // table 1: Weekday and max lectures
    // table 2: Timetable
    // table 3: Teacher, weekday,  maxlectures
    private DataSet moDsTT;
    private DataTable moDtStaybackTable;
    private DataTable moDtAssemblyTable;
    private DataTable moDtMPTTable;
    private DataTable moDtAdditionalLectTable;
    private DataTable moDtWeeklyTest;

    private HtmlTable tblTT;
    private HtmlTable tblTeacher;
    private HtmlGenericControl moDivTeacher;
    private HtmlGenericControl moDivButton;

    HtmlGenericControl oDivSubject;
    Panel moPnl;

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #endregion

    #region events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (CheckPreCondition())
            {
                InitializeMemberVariables();
                if (!IsPostBack)
                {
                    optTeacher.Checked = true;                    
                }
                DisplayTimeTable();
                hlnkTTSchedule.Attributes.Add("onclick", "window.open('../Student/TimeTable.aspx','_blank','scrollbars=yes,statusbar=no,width=650,height=630'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optTeacher_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optTeacher.Checked)
            {
                hidIs_TeachersTT.Value = "Y";
                pnlContainer.Controls.Clear();
                ResetDataMember();
                DisplayTimeTable();
            }
            else
            {
                optClass.Checked = true;
                hidIs_TeachersTT.Value = "N";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optClass_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optClass.Checked)
            {
                hidIs_TeachersTT.Value = "N";
                pnlContainer.Controls.Clear();
                ResetDataMember();
                DisplayTimeTable();
            }
            else
            {
                optTeacher.Checked = true;
                hidIs_TeachersTT.Value = "Y";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
    #region private methods
    /// <summary>
    /// This is a wrapper method to call all the functions to display Timetable.
    /// </summary>
    private void DisplayTimeTable()
    {
        //Dataset contains
        // table 0: Teacher id and name
        // table 1: Weekday and max lectures
        // table 2: Timetable
        // table 3: Teacher, weekday,  maxlectures
        moDsTT = null;
        if (hidIs_TeachersTT.Value == "Y")
            moDsTT = SchoolTimeTableMasterBL.GetSchoolTimeTable(miSchoolId, miAcademicYearId);
        else
            moDsTT = SchoolTimeTableMasterBL.GetWeeklyClassTimeTable(miSchoolId, miAcademicYearId);
        moDtStaybackTable = null;
        moDtWeeklyTest = null;
        moDtStaybackTable = moDsTT.Tables[4];
        moDtAssemblyTable = moDsTT.Tables[5];
        moDtMPTTable = moDsTT.Tables[6];
        moDtAdditionalLectTable = null;
        moDtAdditionalLectTable = moDsTT.Tables[7];
        moDtWeeklyTest = moDsTT.Tables[8];
        int iNoPnl = moDsTT.Tables[I_TEACHER_TBL_INDEX].Rows.Count / I_NO_RECS;
        int iMod = moDsTT.Tables[I_TEACHER_TBL_INDEX].Rows.Count % I_NO_RECS;
        if (iMod != 0)
        {
            iNoPnl = iNoPnl + 1;
        }
        int iBtnCnt = iNoPnl - 1;
        for (int i = 0; i < iNoPnl; i++)
        {
            GeneratePanel();
            GenerateTable(i);
        }
    }

    /// <summary>
    /// This method adds new panel to the main panel.
    /// The  generated panel should contain:
    /// 1. div for teacher
    /// 2. div for weekday lectures.
    /// 3. a separator div.
    /// </summary>
    private void GeneratePanel()
    {
        // Panel to be added to the main container panel
        moPnl = new Panel();
        moPnl.Controls.Clear();
        moPnl.Attributes.Add("style", "width: 100%; align:center ");
        // div for teacher
        moDivTeacher = new HtmlGenericControl("div");
        moDivTeacher.Controls.Clear();
        moDivTeacher.Attributes.Add("style", " float: left; width: 18%; border: 1px");
        //div for weekday lectures.
        oDivSubject = new HtmlGenericControl("div");
        oDivSubject.Controls.Clear();
        oDivSubject.Attributes.Add("style", " float: left; width: 82%; overflow:scroll; border: 1px");

        moDivButton = new HtmlGenericControl("div");
        moDivButton.Controls.Clear();
        // moDivButton.Attributes.Add("style", "align:center");

        moPnl.Controls.Add(moDivTeacher);
        moPnl.Controls.Add(oDivSubject);
        moPnl.Controls.Add(moDivButton);

        pnlContainer.Controls.Add(moPnl);
        //separator div
        HtmlGenericControl oDiv = new HtmlGenericControl("div");
        oDiv.Controls.Clear();
        oDiv.Attributes.Add("style", " float: left; width: 100% ; height: 10px; ");
        //button div

        pnlContainer.Controls.Add(oDiv);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aiBlock"></param>
    private void GenerateTable(int aiBlock)
    {
        AddTables();
        AddHeaderRows();
        AddContentRows(aiBlock);
    }

    #region Header row functions

    private void AddHeaderRows()
    {
        AddTeacherCellToHeader();
        AddWeekDayHeaderRow();
    }

    private void AddToLecturesHeaderRow(int aiTotLecturesCnt, string asCssClass)
    {
        for (int i = 1; i <= aiTotLecturesCnt; i++)
        {
            HtmlTableCell oCell = new HtmlTableCell();
            oCell.Controls.Clear();
            oCell.InnerHtml = i.ToString();
            oCell.Align = "center";
            oCell.Height = "20px";
            oCell.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");
            oCell.Attributes.Add("class", asCssClass);
            tblTT.Rows[1].Cells.Add(oCell);
        }

    }
    private void AddTeacherCellToHeader()
    {
        HtmlTableRow oWeekDayRow = new HtmlTableRow();
        oWeekDayRow.Controls.Clear();
        tblTeacher.Rows.Add(oWeekDayRow);
        HtmlTableRow oLecturesRow = new HtmlTableRow();
        oLecturesRow.Controls.Clear();
        tblTeacher.Rows.Add(oLecturesRow);

        HtmlTableCell oCell = new HtmlTableCell();
        oCell.Controls.Clear();

        Image OImage = new Image();
        if (hidIs_TeachersTT.Value == "Y")
            OImage.ImageUrl = "~/RITeSchool/images/GridHead_TeachWday.gif";
        else
            OImage.ImageUrl = "~/RITeSchool/images/GridHead_ClassesWday.gif";

        oCell.Controls.Add(OImage);
        oCell.Attributes.Add("class", S_CSS_GRIDHEAD);
        tblTeacher.Rows[0].Cells.Add(oCell);

        oCell = new HtmlTableCell();
        oCell.Controls.Clear();
        oCell.InnerHtml = Resources.LocalizedResources.LectureNo +">>";
        oCell.Attributes.Add("class", S_CSS_SEPROW);
        oCell.Align = "right";
        oCell.Height = "20px";
        tblTeacher.Rows[1].Cells.Add(oCell);
    }
    private void AddWeekDayHeaderRow()
    {

        HtmlTableRow oWeekDayRow = new HtmlTableRow();
        oWeekDayRow.Controls.Clear();
        tblTT.Rows.Add(oWeekDayRow);
        HtmlTableRow oLecturesRow = new HtmlTableRow();
        oLecturesRow.Controls.Clear();
        tblTT.Rows.Add(oLecturesRow);
        DataTable oDtTeacher = moDsTT.Tables[I_TEACHER_TBL_INDEX];
        int iRowSpan = oDtTeacher.Rows.Count + 2;
        //get weekday and resp. max lectures cnt
        DataTable oDt = moDsTT.Tables[I_WEEKDAY_TBL_INDEX];
        int iRowCnt = oDt.Rows.Count;
        string sCssHead = S_CSS_WEEKDAY;
        for (int iRowIndex = 0; iRowIndex < iRowCnt; iRowIndex++)
        {
            HtmlTableCell oCell = new HtmlTableCell();
            oCell.Controls.Clear();
            oCell.InnerHtml = oDt.Rows[iRowIndex][S_FEILD_WEEKDAYNAME].ToString().Replace(" ",string.Empty);
            oCell.ColSpan = Convert.ToInt32(oDt.Rows[iRowIndex]["LecturesCnt"]);
            oCell.Align = "center";
            oCell.Height = "26px";
            oCell.Attributes.Add("class", sCssHead);
            oWeekDayRow.Cells.Add(oCell);
            AddToLecturesHeaderRow(oCell.ColSpan, sCssHead);
            sCssHead = ToggleClass(sCssHead);
            AddSeparatorRow(iRowSpan);
        }
    }

    private void AddSeparatorRow(int aiRowSpan)
    {
        HtmlTableCell oSepCell = new HtmlTableCell();
        oSepCell.Controls.Clear();
        oSepCell.RowSpan = aiRowSpan;
        oSepCell.Attributes.Add("class", S_CSS_SEPROW);
        oSepCell.Width = "2px";
        tblTT.Rows[0].Cells.Add(oSepCell);

    }
    #endregion
    #region Content row functions
    private void AddContentRows(int aiBlock)
    {
        int iStart = 0;
        iStart = aiBlock * I_NO_RECS;
        DataTable oDt = moDsTT.Tables[I_TEACHER_TBL_INDEX];
        int iRowCnt = oDt.Rows.Count;
        int iEnd = iStart + I_NO_RECS;
        if (iEnd < iRowCnt)
            iRowCnt = iEnd;
        for (int iRowIndex = iStart; iRowIndex < iRowCnt; iRowIndex++)
        {
            int iTeacherID;
            string sTeacherName;
            string sMptAvailable = "N";
            string sAssblAvailable = "N";
            bool bStayBackApp = false;
            string sWeeklyTestAvailable = "N";

            if (hidIs_TeachersTT.Value == "Y")
            {
                iTeacherID = Convert.ToInt32(oDt.Rows[iRowIndex]["Teacher_ID"]);
                sTeacherName = oDt.Rows[iRowIndex][S_FEILD_TEACHERNAME].ToString();
                sMptAvailable = oDt.Rows[iRowIndex][S_FEILD_MPT_APPLICABLE].ToString();
                sAssblAvailable = oDt.Rows[iRowIndex][S_FEILD_ASSEMBLY_APPLICABLE].ToString();
                sWeeklyTestAvailable = oDt.Rows[iRowIndex][S_FIELD_WEEKLYTEST_APPLICABLE].ToString();
                if (oDt.Rows[iRowIndex][S_FEILD_STAYBACK_APPLICABLE].ToString() != null && oDt.Rows[iRowIndex][S_FEILD_STAYBACK_APPLICABLE].ToString() != "")
                    bStayBackApp = Convert.ToBoolean(oDt.Rows[iRowIndex][S_FEILD_STAYBACK_APPLICABLE]);
            }
            else
            {
                iTeacherID = Convert.ToInt32(oDt.Rows[iRowIndex]["StdDivId"]);
                sTeacherName = oDt.Rows[iRowIndex]["Class"].ToString();

                DataRow[] oArrDataRow = oDt.Select("StdDivId = " + iTeacherID.ToString());
                if (oArrDataRow.Length > 1)
                {
                    iRowIndex = iRowIndex + (oArrDataRow.Length - 1);
                    foreach (DataRow oDataRow in oArrDataRow)
                    {
                        if (oDataRow[S_FEILD_MPT_APPLICABLE].ToString() == "Y")
                            sMptAvailable = oDataRow[S_FEILD_MPT_APPLICABLE].ToString();
                        if (oDataRow[S_FEILD_ASSEMBLY_APPLICABLE].ToString() == "Y")
                            sAssblAvailable = oDataRow[S_FEILD_ASSEMBLY_APPLICABLE].ToString();
                        if (oDataRow[S_FEILD_STAYBACK_APPLICABLE].ToString() != null && oDataRow[S_FEILD_STAYBACK_APPLICABLE].ToString() != Constants.S_EMPTY_STRING)
                        {
                            if (Convert.ToBoolean(oDataRow[S_FEILD_STAYBACK_APPLICABLE]) == true)
                                bStayBackApp = Convert.ToBoolean(oDataRow[S_FEILD_STAYBACK_APPLICABLE]);
                        }
                        if(oDataRow[S_FIELD_WEEKLYTEST_APPLICABLE].ToString() == "Y")
                            sWeeklyTestAvailable = oDataRow[S_FIELD_WEEKLYTEST_APPLICABLE].ToString();
                    }
                }
                else
                {
                    sMptAvailable = oDt.Rows[iRowIndex][S_FEILD_MPT_APPLICABLE].ToString();
                    sAssblAvailable = oDt.Rows[iRowIndex][S_FEILD_ASSEMBLY_APPLICABLE].ToString();
                    bStayBackApp = Convert.ToBoolean(oDt.Rows[iRowIndex][S_FEILD_STAYBACK_APPLICABLE]);
                    sWeeklyTestAvailable = oDt.Rows[iRowIndex][S_FIELD_WEEKLYTEST_APPLICABLE].ToString();
                }
            }
            AddRowForTeacher(iTeacherID, sTeacherName, sMptAvailable, sAssblAvailable, bStayBackApp.ToString(), sWeeklyTestAvailable);
        }
    }

    private void AddRowForTeacher(int aiTeacherID, string asTeacherName, string asMPTApplicable, string asAssemblyApplicable, string asStaybackApplicable, string asWeeklyTestApplicable)
    {   
        HtmlTableRow oTeacherRow = new HtmlTableRow();
        oTeacherRow.Controls.Clear();
        tblTeacher.Rows.Add(oTeacherRow);

        DataTable oDtWeekDay = moDsTT.Tables[I_WEEKDAY_TBL_INDEX];
        int iWeekDayCnt = oDtWeekDay.Rows.Count;
        HtmlTableCell oTeacherCell = new HtmlTableCell();
        Label oName = new Label();
        oName.Text = asTeacherName;
        oTeacherCell.Controls.Add(oName);
        oTeacherCell.Attributes.Add("class", S_CSS_DATACELL);
        oTeacherRow.Cells.Add(oTeacherCell);

        DataRow[] oArrDataRow;
        if (hidIs_TeachersTT.Value == "Y")
            oArrDataRow = moDtAdditionalLectTable.Select("Teacher_Id = " + aiTeacherID.ToString());
        else
            oArrDataRow = moDtAdditionalLectTable.Select("SchoolWise_Standard_Division_Id = " + aiTeacherID.ToString());

        if (oArrDataRow.Length > 0)
            oTeacherCell.Height = "72px";
        else
            oTeacherCell.Height = "36px";
        oTeacherCell.Align = "left";
        //Time table

        HtmlTableRow oLectures = new HtmlTableRow();
        tblTT.Rows.Add(oLectures);
        string sCssClass = S_CSS_WEEKDAY_CELL;

        // Get assembly/MPT configuration
        string sAssembly = Settings.AssemblyName;
        string sMPT = Settings.MPTName;
        string sStayback = Settings.StaybackName;
        string sWeeklyTest = Settings.WeeklyTestName;

        int iAssemblyLectNo = Settings.AssemblyLectNo;
        string sAssemblyWeekday = Settings.AssemblyWeekday;
        int iMPTLectNo = Settings.MPTLectNo;
        string sMPTWeekday = Settings.MPTWeekday;

        //loop through weekdays
        for (int iWeekDay = 0; iWeekDay < iWeekDayCnt; iWeekDay++)
        {
            int iLectureCnt = Convert.ToInt32(oDtWeekDay.Rows[iWeekDay]["LecturesCnt"]);
            string sWeekDayId = oDtWeekDay.Rows[iWeekDay]["Weekdays_Id"].ToString();
            string sWeekDayName = oDtWeekDay.Rows[iWeekDay]["Weekday_Name"].ToString();

            //loop through Lectures
            for (int i = 1; i <= iLectureCnt; i++)
            {
                string sDesc = sDesc = GetLectureDesc(aiTeacherID.ToString(), sWeekDayId, i.ToString());
                if (hidIs_TeachersTT.Value == "Y")
                {
                    if (Settings.IsStaybackApplicable)
                    {
                        if (asStaybackApplicable.ToLower() == true.ToString().ToLower())
                        {
                            DataRow[] oArrRows = moDtStaybackTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i);
                            if (oArrRows.Length > 0)
                                sDesc = "<b>" + sStayback + "</b>";
                        }
                    }
                    if (Settings.IsAssemblyApplicable)
                    {
                        if (asAssemblyApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                        {
                            DataRow[] oArrRows = moDtAssemblyTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i);
                            if (oArrRows.Length > 0)
                                sDesc = "<b>" + sAssembly + "</b>";
                        }
                    }
                    if (Settings.IsMPTApplicable)
                    {
                        if (asMPTApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                        {
                            DataRow[] oArrRows = moDtMPTTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i);
                            if (oArrRows.Length > 0)
                                sDesc = "<b>" + sMPT + "</b>";
                        }
                    }
                    if (Settings.IsWeeklyTestApplicable)
                    {
                        if (asWeeklyTestApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                        {
                            DataRow[] oArrRows = moDtWeeklyTest.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i);
                            if (oArrRows.Length > 0)
                                sDesc = "<b>" + sWeeklyTest + "</b>";
                        }
                    }
                 }
                else
                {
                    if (Settings.IsStaybackApplicable)
                    {
                        DataRow[] oArrRows = moDtStaybackTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i + "AND StandardDivision_Id=" + aiTeacherID);
                        if (oArrRows.Length > 0)
                            sDesc = "<b>" + sStayback + "</b>";
                    }
                    if (Settings.IsAssemblyApplicable)
                    {
                        DataRow[] oArrRows = moDtAssemblyTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i + "AND StandardDivision_Id=" + aiTeacherID);
                        if (oArrRows.Length > 0)
                            sDesc = "<b>" + sAssembly + "</b>";
                    }
                    if (Settings.IsMPTApplicable)
                    {
                        DataRow[] oArrRows = moDtMPTTable.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i + "AND StandardDivision_Id=" + aiTeacherID);
                        if (oArrRows.Length > 0)
                            sDesc = "<b>" + sMPT + "</b>";
                    }
                    if (Settings.IsWeeklyTestApplicable)
                    {
                        DataRow[] oArrRows = moDtWeeklyTest.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i + "AND StandardDivision_Id=" + aiTeacherID);
                        if (oArrRows.Length > 0)
                            sDesc = "<b>" + sWeeklyTest + "</b>";
                    }
                }
                HtmlTableCell oCell = new HtmlTableCell();
                oCell.InnerHtml = sDesc;
                oCell.Attributes.Add("class", sCssClass);
                oCell.Attributes.Add("nowrap", "true");
                if (oArrDataRow.Length > 0)
                    oCell.Height = "72px";
                else
                    oCell.Height = "36px";
                oCell.Attributes.Add("title", oResourceManager.GetString(sWeekDayName.Replace(" ",string.Empty)) + "[Lect. " + i + "]");
                oCell.Align = "center";
                if (sDesc.Equals("N/A"))
                {
                    oCell.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");

                }
                else if (sDesc.Equals("Off"))
                {
                    oCell.Style.Add(HtmlTextWriterStyle.Color, "black");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");
                }
                oLectures.Cells.Add(oCell);
            }

            sCssClass = ToggleClass(sCssClass);
        }
    }

    private string GetLectureDesc(string asTeacherId, string asWeekdayId, string asLectureNo)
    {
        string sReturn = "Off";
        DataTable oDtTT = moDsTT.Tables[I_TT_TBL_INDEX];
        DataTable oDtTeacherLectures = moDsTT.Tables[I_TEACHER_LECTURE_TBL_INDEX];
        DataRow[] oDtRows;
        DataRow[] oDrLectures;

        if (hidIs_TeachersTT.Value == "Y")
        {
            oDtRows = oDtTT.Select("Teacher_Id= " + asTeacherId + " AND Weekday_Id=" + asWeekdayId + " AND Lecture_Number=" + asLectureNo);
            oDrLectures = oDtTeacherLectures.Select("Weekdays_Id=" + asWeekdayId + " AND Teacher_Id=" + asTeacherId);
        }
        else
        {
            oDtRows = oDtTT.Select("SchoolWise_Standard_Division_Id= " + asTeacherId + " AND Weekday_Id=" + asWeekdayId + " AND Lecture_Number=" + asLectureNo);
            oDrLectures = oDtTeacherLectures.Select("Weekdays_Id=" + asWeekdayId + " AND SchoolWise_Standard_Division_Id=" + asTeacherId);
        }
        int iMaxTeacherLectureCnt = 0;
        if (oDrLectures.Length > 0)
        {
            iMaxTeacherLectureCnt = Convert.ToInt32(oDrLectures[0]["max_lectures"]);

        }
        if (Convert.ToInt32(asLectureNo) <= iMaxTeacherLectureCnt)
        {
            if (oDtRows.Length == 1)
                sReturn = oDtRows[0]["description"].ToString();
            else if (oDtRows.Length > 1)
            {
                sReturn = "";
                foreach (DataRow oDataRow in oDtRows)
                    sReturn += oDataRow["description"].ToString() + "<br>";
                sReturn = sReturn.Substring(0, sReturn.LastIndexOf("<br>"));
            }
        }
        else
        {
            sReturn = "N/A";

        }
        return sReturn;

    }
    #endregion
    private void AddTables()
    {
        tblTT = new HtmlTable();
        tblTT.Controls.Clear();
        tblTT.EnableViewState = true;
        tblTT.BgColor = "#6394D6";
        tblTT.CellPadding = 0;
        tblTT.CellSpacing = 1;
        tblTT.Border = 0;
        tblTT.Width = "98%";
        tblTT.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        tblTT.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        tblTT.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#6394D6");
        oDivSubject.Controls.Add(tblTT);

        tblTeacher = new HtmlTable();
        tblTeacher.Controls.Clear();
        tblTeacher.EnableViewState = true;
        tblTeacher.BgColor = "#6394D6";
        tblTeacher.CellPadding = 0;
        tblTeacher.CellSpacing = 1;
        tblTeacher.Border = 0;
        tblTeacher.Width = "98%";
        tblTeacher.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        tblTeacher.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        tblTeacher.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#6394D6");
        moDivTeacher.Controls.Add(tblTeacher);

    }
    private string ToggleClass(string asCssHead)
    {
        string sReturn = "";
        switch (asCssHead)
        {
            case S_CSS_WEEKDAY:
                sReturn = S_CSS_ALT_WEEKDAY;
                break;
            case S_CSS_ALT_WEEKDAY:
                sReturn = S_CSS_WEEKDAY;
                break;
            case S_CSS_WEEKDAY_CELL:
                sReturn = S_CSS_ALT_WEEKDAY_CELL;
                break;
            case S_CSS_ALT_WEEKDAY_CELL:
                sReturn = S_CSS_WEEKDAY_CELL;
                break;

        }
        return sReturn;
    }
    /// <summary>
    /// This function checks the preconditons of Teachertimetable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {

        bool bReturn = false;
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);

        string sLinks = ReferenceBL.CheckPrecondition(iSchoolId, iAcademicYearId, Convert.ToInt32(Constants.SchoolConfigurations.TeacherTimeTable), Convert.ToInt32((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]));

        if (!sLinks.Equals(""))
        {
            divErr.InnerHtml = sLinks;
            divSTT.Visible = false;
            divLink.Visible = false;
            trClassTeacher.Visible = false;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;

    }
    private void ResetDataMember()
    {
        moDsTT = null;
        moDtStaybackTable = null;
        moDtAdditionalLectTable = null;
        moDtWeeklyTest = null;

        tblTT = null;
        tblTeacher = null;
        moDivTeacher = null;
        moDivButton = null;
        moPnl = null;
        oDivSubject = null;
    }

    #endregion
}
