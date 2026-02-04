using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Utility;


/// <summary>
/// Summary description for Timetable
/// This class formats timetable data into desired format.
/// </summary>
public class Timetable : SchoolBase
{
    #region Data members
    private DataTable moDtTeacher;//Teacher id and name
    private DataTable moDtTimeTable;// Timetable
    private DataTable moDtWeekday;//Weekday and max lectures
    private DataTable moDtLectures;//Teacher, weekday,  maxlectures
    private DataTable moDtAssembly;//Assembly
    private DataTable moDtStayBack;//StayBack
    private HtmlTable tblTeacher;
    private HtmlGenericControl moDivTeacher;
    HtmlGenericControl moDivLectures;

    //private bool mbSrcAssembly;
    //private bool mbSrcMPT;
    //private bool mbTargetAssembly;
    //private bool mbTargetMPT;

    public HtmlTable tblTT;
    public Panel moPnl;

    #endregion
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

    private const int I_TEACHER_TBL_INDEX = 1;
    private const int I_WEEKDAY_TBL_INDEX = 0;
    private const int I_TT_TBL_INDEX = 2;
    private const int I_TEACHER_LECTURE_TBL_INDEX = 3;
    private const int I_TEACHER_CELL_INDEX = 0;
    #endregion
    public Timetable()
    {	
        //
        // TODO: Add constructor logic here
        //
    }
    public Timetable(DataTable aoDtTeacher, DataTable aoDtTimeTable, DataTable aoDtWeekday, DataTable aoDtLectures, DataTable aoDtAssembly,DataTable aoDtStayBack)
    {
        moDtTeacher = aoDtTeacher;//Teacher id and name
        moDtTimeTable = aoDtTimeTable;// Timetable
        moDtWeekday = aoDtWeekday;//Weekday and max lectures
        moDtLectures = aoDtLectures;
        moDtAssembly = aoDtAssembly;
        moDtStayBack = aoDtStayBack;
    }

    #region methods
    /// <summary>
    /// This is awrapper method to format the timetable.
    /// </summary>
    public void DisplayTT()
    {
        GeneratePanel();
        GenerateTable();
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
        moPnl.ID = "pnlTT";
        moPnl.Attributes.Add("style", "width: 100%; ");
        // div for teacher
        moDivTeacher = new HtmlGenericControl("div");
        moDivTeacher.Attributes.Add("style", " float: left; width: 18%; border: 1px");
        //div for weekday lectures.
        moDivLectures = new HtmlGenericControl("div");
        moDivLectures.Attributes.Add("style", " float: left; width: 82%; overflow:scroll; border: 1px");
        moPnl.Controls.Add(moDivTeacher);
        moPnl.Controls.Add(moDivLectures);
        moDivLectures.ID = "divLectures";
        //separator div
        HtmlGenericControl oDiv = new HtmlGenericControl("div");
        oDiv.Attributes.Add("style", " float: left; width: 100% ; height: 10px; ");
    }
    /// <summary>
    /// This is a wrapper method which calls other methods to format inner tables in timetable.
    /// </summary>
    private void GenerateTable()
    {
        AddTables();
        AddHeaderRows();
        AddContentRows();


    }
    /// <summary>
    /// This method adds table in the teachers and lectures divs.
    /// </summary>
    private void AddTables()
    {
        tblTT = new HtmlTable();
        tblTT.ID = "tbl_TTS";
        tblTT.EnableViewState = true;
        //tblTT.Attributes.Add("class",  S_CSS_OUTERTBL);
        tblTT.BgColor = "#6394D6";
        tblTT.CellPadding = 0;
        tblTT.CellSpacing = 1;
        tblTT.Border = 0;
        tblTT.Width = "98%";
        tblTT.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        tblTT.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        tblTT.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#6394D6");
        moDivLectures.Controls.Add(tblTT);

        tblTeacher = new HtmlTable();
        tblTeacher.ID = "tbl_Teachers";
        tblTeacher.EnableViewState = true;
        //tblTeacher.Attributes.Add("class",  S_CSS_OUTERTBL);
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
    /// <summary>
    /// This is a wrapper method to call the methods to add header rows in both the tables(teachers and lectures)
    /// </summary>
    private void AddHeaderRows()
    {
        AddTeacherCellToHeader();
        AddWeekDayHeaderRow();
    }
    /// <summary>
    /// This method adds the header row in the lectures table.
    /// The header row is made up of 2 rows.
    /// 1. for weekdays. (each cell in this row spans the columns = the no. of lectures for that weekday)
    /// 2. For the lectures in the weekday.
    /// </summary>
    private void AddWeekDayHeaderRow()
    {

        HtmlTableRow oWeekDayRow = new HtmlTableRow();
        tblTT.Rows.Add(oWeekDayRow);
        HtmlTableRow oLecturesRow = new HtmlTableRow();
        tblTT.Rows.Add(oLecturesRow);
        DataTable oDtTeacher = moDtTeacher;
        int iRowSpan = oDtTeacher.Rows.Count + 2;

        int iRowCnt = moDtWeekday.Rows.Count;
        string sCssHead = S_CSS_WEEKDAY;
        //loop through weekdays 
        for (int iRowIndex = 0; iRowIndex < iRowCnt; iRowIndex++)
        {
            HtmlTableCell oCell = new HtmlTableCell();
            oCell.InnerHtml = moDtWeekday.Rows[iRowIndex][S_FEILD_WEEKDAYNAME].ToString();
            //get. max lectures cnt for current weekday and set the colspan
            oCell.ColSpan = Convert.ToInt32(moDtWeekday.Rows[iRowIndex]["LecturesCnt"]);
            //set the style
            oCell.Align = "center";
            oCell.Height = "26px";
            oCell.Attributes.Add("class", sCssHead);
            oWeekDayRow.Cells.Add(oCell);
            AddToLecturesHeaderRow(oCell.ColSpan, sCssHead);
            //get next style
            sCssHead = ToggleClass(sCssHead);
            //add separator row.
            AddSeparatorRow(iRowSpan);
        }


    }
    /// <summary>
    /// The colspan of the weekday header row is set to lecture count .
    /// This method adds the cell for each lecture (for the weekday.)
    /// </summary>
    /// <param name="aiTotLecturesCnt">No of lectures</param>
    /// <param name="asCssClass">css class to be applied</param>
    private void AddToLecturesHeaderRow(int aiTotLecturesCnt, string asCssClass)
    {
        for (int i = 1; i <= aiTotLecturesCnt; i++)
        {
            HtmlTableCell oCell = new HtmlTableCell();
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
        tblTeacher.Rows.Add(oWeekDayRow);
        HtmlTableRow oLecturesRow = new HtmlTableRow();
        tblTeacher.Rows.Add(oLecturesRow);

        // const string S_IMG_GRD_HEAD = "~/RITeSchool/images/GridHead_LectWdayTT.gif";
        HtmlTableCell oCell = new HtmlTableCell();
        Image OImage = new Image();
        OImage.ImageUrl = "~/RITeSchool/images/GridHead_TeachWday.gif";
        oCell.Controls.Add(OImage);
        oCell.Align = "left";
        oCell.Attributes.Add("class", S_CSS_GRIDHEAD);
        tblTeacher.Rows[0].Cells.Add(oCell);

        oCell = new HtmlTableCell();
        oCell.InnerHtml = "Lecture No.>>";
        oCell.Attributes.Add("class", S_CSS_SEPROW);
        oCell.Align = "right";
        oCell.Height = "20px";
        tblTeacher.Rows[1].Cells.Add(oCell);
    }
    private void AddSeparatorRow(int aiRowSpan)
    {
        HtmlTableCell oSepCell = new HtmlTableCell();
        oSepCell.RowSpan = aiRowSpan;
        oSepCell.Attributes.Add("class", S_CSS_SEPROW);
        oSepCell.Width = "2px";
        tblTT.Rows[0].Cells.Add(oSepCell);

    }
    #region Content row functions
    /// <summary>
    /// This method adds the rows for the data. i.e actual lecture description for each teacher.
    /// </summary>
    private void AddContentRows()
    {
        DataTable oDt = moDtTeacher;
        int iRowCnt = oDt.Rows.Count;
        //loop through all the teachers
        for (int iRowIndex = 0; iRowIndex < iRowCnt; iRowIndex++)
        {
            int iTeacherID = Convert.ToInt32(oDt.Rows[iRowIndex]["Teacher_ID"]);
            string sTeacherName = oDt.Rows[iRowIndex][S_FEILD_TEACHERNAME].ToString();
            //add row for the teacher
            AddRowForTeacher(iTeacherID, sTeacherName, moDtAssembly.Rows[iRowIndex][S_FEILD_MPT_APPLICABLE].ToString(), moDtAssembly.Rows[iRowIndex][S_FEILD_ASSEMBLY_APPLICABLE].ToString(),moDtAssembly.Rows[iRowIndex]["Stayback_Applicable"].ToString());
        }
    }
    /// <summary>
    ///  This is a wrapper method to add the content row for each teacher.
    /// </summary>
    /// <param name="aiTeacherID"></param>
    /// <param name="asTeacherName"></param>
    private void AddRowForTeacher(int aiTeacherID, string asTeacherName, string asMPTApplicable, string asAssemblyApplicable,string asStayBackApplicable)
    {
        //Add row in teachers table
        //AddRowInTeacherTable(asTeacherName);
        //Add a row for the lectures of given teacher
        AddRowInLecturesTable(aiTeacherID, asTeacherName, asMPTApplicable, asAssemblyApplicable, asStayBackApplicable);
    }
    /// <summary>
    ///  This method adds a row tn lectures's table. And dsplays the lectures for the given teacher in the row. 
    /// </summary>
    /// <param name="aiTeacherID"></param>
    private void AddRowInLecturesTable(int aiTeacherID, string asTeacherName, string asMPTApplicable, string asAssemblyApplicable, string asStayBackApplicable)
    {
        HtmlTableRow oTeacherRow = new HtmlTableRow();
        tblTeacher.Rows.Add(oTeacherRow);
        int iTotCellCnt = tblTeacher.Rows[1].Cells.Count;

        HtmlTableCell oTeacherCell = new HtmlTableCell();
        oTeacherCell.InnerHtml = asTeacherName;
        oTeacherCell.Attributes.Add("class", S_CSS_DATACELL);
        oTeacherCell.Height = "36px";
        oTeacherCell.Align = "left";


        //Add row in lectures table for teacher's lectures 
        DataTable oDtWeekDay;
        int iWeekDayCnt;
        oDtWeekDay = moDtWeekday;
        iWeekDayCnt = oDtWeekDay.Rows.Count;

        HtmlTableRow oLectures = new HtmlTableRow();
        tblTT.Rows.Add(oLectures);
        string sCssClass = S_CSS_WEEKDAY_CELL;
        // Get assembly/MPT configuration
        string sAssembly = Settings.AssemblyName;
        string sMPT = Settings.MPTName;
        int iAssemblyLectNo = Settings.AssemblyLectNo;
        string sAssemblyWeekday = Settings.AssemblyWeekday;
        int iMPTLectNo = Settings.MPTLectNo;
        string sMPTWeekday = Settings.MPTWeekday;
        string sStayback = Settings.StaybackName;
        //loop through weekdays
        for (int iWeekDay = 0; iWeekDay < iWeekDayCnt; iWeekDay++)
        {
            int iLectureCnt = Convert.ToInt32(oDtWeekDay.Rows[iWeekDay]["LecturesCnt"]);
            string sWeekDayId = moDtWeekday.Rows[iWeekDay]["Weekdays_Id"].ToString();
            string sWeekDayName = moDtWeekday.Rows[iWeekDay]["Weekday_Name"].ToString();

            //loop through Lectures
            for (int i = 1; i <= iLectureCnt; i++)
            {
                //get the lecture description
                string sDesc = GetLectureDesc(aiTeacherID.ToString(), sWeekDayId, i.ToString());
				
				if (Settings.IsAssemblyApplicable)
                {
                    if (asAssemblyApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                    {
                        if (i == iAssemblyLectNo && sWeekDayName.ToLower() == sAssemblyWeekday.ToLower())
                        {
                            sDesc = "<b>" + sAssembly + "</b>";
                        }
                    }
                }
                if (Settings.IsMPTApplicable)
                {
                    if (asMPTApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                    {
                        if (i == iMPTLectNo && sWeekDayName.ToLower() == sMPTWeekday.ToLower())
                        {
                            sDesc = "<b>" + sMPT + "</b>";
                        }
                    }
                }

                if (Settings.IsStaybackApplicable)
                    if (asStayBackApplicable.ToLower() == true.ToString().ToLower())
                    {
                        DataRow[] oArrRows = moDtStayBack.Select("WeekDay_Name='" + sWeekDayName + "' AND Lecture_Number=" + i);
                        if (oArrRows.Length > 0)
                            sDesc = "<b>" + sStayback + "</b>";
                    }


                HtmlTableCell oCell = new HtmlTableCell();
                oCell.InnerHtml = sDesc;
                oCell.Attributes.Add("class", sCssClass);
                oCell.Attributes.Add("nowrap", "true");
                if (sDesc.Contains("<br>"))
                {
                    oTeacherCell.Height = "72px";
                    oCell.Height = "72px";
                }
                else
                    oCell.Height = "36px";

                oCell.Attributes.Add("title", sWeekDayName + "[Lect. " + i + "]");
                oCell.Align = "center";
                //apply styles according to lecture description
                if (sDesc.Equals("N/A"))
                {
                    oCell.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");

                }
                else if (sDesc.Equals("Off"))
                {
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
                    oCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");
                }
                oTeacherRow.Cells.Add(oTeacherCell);
                oLectures.Cells.Add(oCell);

            }
            //change the css class for next weekday
            sCssClass = ToggleClass(sCssClass);
        }
    }
    /// <summary>
    /// This method adds a row tn teacher's table. And dsplays the name of the teacher in the row. 
    /// </summary>
    /// <param name="asTeacherName"></param>
    private void AddRowInTeacherTable(string asTeacherName)
    {
        HtmlTableRow oTeacherRow = new HtmlTableRow();
        tblTeacher.Rows.Add(oTeacherRow);
        int iTotCellCnt = tblTeacher.Rows[1].Cells.Count;

        HtmlTableCell oTeacherCell = new HtmlTableCell();
        oTeacherCell.InnerHtml = asTeacherName;
        oTeacherCell.Attributes.Add("class", S_CSS_DATACELL);
        oTeacherCell.Height = "36px";
        oTeacherCell.Align = "left";
        oTeacherRow.Cells.Add(oTeacherCell);
    }
    /// <summary>
    /// This method retrives the description of the lecture.
    /// If the datatable doent contain the record for the given lecture, Off or N/A is returned.
    /// </summary>
    /// <param name="asTeacherId"></param>
    /// <param name="asWeekdayId"></param>
    /// <param name="asLectureNo"></param>
    /// <returns></returns>
    private string GetLectureDesc(string asTeacherId, string asWeekdayId, string asLectureNo)
    {
        string sReturn = "Off";
        DataTable oDtTT = moDtTimeTable;
        DataTable oDtTeacherLectures = moDtLectures;
        DataRow[] oDtRows = moDtTimeTable.Select("Teacher_Id= " + asTeacherId + " AND Weekday_Id=" + asWeekdayId + " AND Lecture_Number=" + asLectureNo);
        DataRow[] oDrLectures = oDtTeacherLectures.Select("Weekdays_Id=" + asWeekdayId + " AND Teacher_Id=" + asTeacherId);
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
    /// <summary>
    /// The css class (styles) for the alternate weekday blocks are different.
    /// This function returns the alternate style to be applied to  the block.
    /// </summary>
    /// <param name="asCssHead"> current style name</param>
    /// <returns></returns>
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
    #endregion




}
