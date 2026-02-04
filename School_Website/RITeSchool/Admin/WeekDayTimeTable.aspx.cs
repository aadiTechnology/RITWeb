using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Xml;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

/// <summary>
/// This class displays the timetable for all the teachers for  selected weekday.
/// Flow:
/// 1. Select Week day
/// 2. Click on "Show"
/// 3. Select the lecture for teachers.
/// 4. Click on "Save".
/// </summary>

public partial class WeekDayTimeTable :SchoolBase
{
    #region Constants
    private const Int32 I_COLUMN_NUMBERS = 1;

    private const Int32 I_TEACHERID_COLUMN_NO = 0;


    private const String S_STD_DIV_ID_FIELD = "Standard_Division_Id";
    private const String S_STD_ID_FIELD = "Standard_Id";
    private const string S_STD_DIV_NAME_FIELD = "StandardDivision";
    private const string S_CLASS_SUBJECT_ID_FIELD = "Schoolwise_Division_Subject_Id";
    private const string S_STD_SUBJECT_MAXLECTURES_PERWEEK_FIELD = "StdSubjectMaxLecturesPerWeek";
    private const string S_MAX_TEACHER_LECTURES_FOR_STD_PER_DAY = "MaxTeacherLecturesForStdandard";
    private const string S_MAX_STDTEACHER_LECTURES_PER_WEEK = "StdTeacherMaxLecturesPerWeek";
    private const string S_TEACHER_STD_ASSIGNED_LECTURES = "TeacherstdLecturesinWeek";
    private const string S_TEACHER_SUBJECT_ID_FIELD = "Teacher_Subject_Id";
    private const string S_MAXLECTURE_STD_FIELD = "MaxLecturesStdandard";
    private const string S_LECTURES_ASSIGNED = "LecturesAssigned";
    private const string S_CLASSSUBJECT_NAME_FIELD = "classSubjectName";
    private const string S_LECTURE = "Lecture";

    private const string S_CSS_NA = "TTNotAssignDark";
    private const string S_CSS_NOTCLASS_TEACHER = "TTNotClassTchr";
    private const string S_IMG_GRD_HEAD = "~/RITeSchool/images/GridHead_TeachLectTT.gif";

    const string S_HEADER_MSG_DUPLICATE_CLASS = "Duplicate class for: ";
    const string S_HEADER_MSG_CLASSSUBJECT_WEEK = "Max. limit of lectures per week exceeds for the following subjects: ";
    const string S_HEADER_MSG_TEACHER_STD = "Max. lectures per day exceeds: ";
    const string S_HEADER_MSG_TEACHER_STD_WEEK = "Max. lectures per week  exceeds for the following teachers: ";

    const string S_HEADER_MSG_TEACHER_DAY = "Max. lectures per day exceeds for the following teachers:<BR> ";
    const string S_HEADER_MSG_TEACHER_WEEK = "Max. lectures per week exceeds for the following teachers:<BR>";

    private const string S_CSSCLASS_COMBO = "TTCombo";
    private const string S_CSSCLASS_COMBO_SELECTED = "TTComboSelect";


    private const string S_SHOW = "Show";
    private const string S_CHANGE = "Change Weekday";

    const string S_TXTCLASSTEACHER = "Not a Class Teacher";
    const string S_TXTLECTURENA = "Lecture Not Applicable";
    #endregion

    #region datamembers
    private static int miWeekDayId;
    #endregion
    #region events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bool bIsUseSubmitBehavior = true;
            if (IsPostBack)
            {
                bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
                if (bIsUseSubmitBehavior == true)
                {
                    InitialiseDatamembers();
                    FillGrid();
                }
            }
            else
            {
                InitialiseDatamembers();
                if (ValidateUser() && CheckPreCondition())
                {
                    FillWeekdayCombo();
                    grdTemp.Columns[0].HeaderImageUrl = S_IMG_GRD_HEAD;
                    grdTemp.Columns[0].HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                    cmbWeekDay.Focus();
                    btnSave.Attributes["onclick"] = "javascript:DisableButtons(this)";
                    btnSave.Visible = false;
                }
            }
            ApplyMouseHoverEffect(new List<Button> { btnSave, btnShow });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (ToggleStatus())
            {
                btnSave.Visible = true;
                divTimeTable.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                divTimeTable.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidateData())
            {
                string[] strXml = new string[2];
                strXml = GetXMLForTimeTable();
                SchoolTimeTableMasterBL oTimeTable = GetObject();
                oTimeTable.ManageDayTimeTable(strXml[0], strXml[1]);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            btnSave.Enabled = true;
        }

    }

    #endregion
    #region private methods


    /// <summary>
    /// This function checks the preconditons of WeekdayTimeTable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {

        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.WeekDayTimeTable);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            HideAllFields();
        }
        return bReturn;

    }
    private SchoolTimeTableMasterBL GetObject()
    {
        SchoolTimeTableMasterBL obj = new SchoolTimeTableMasterBL();
        obj.AcademicYearId = miAcademicYearId;
        obj.SchoolId = miSchoolId;
        obj.InsertedById = miUserId;
        obj.WeekdayId = miWeekDayId;
        return obj;
    }
    private void InitialiseDatamembers()
    {
        btnSave.Attributes.Add("onclick", "GoToTop()");
    }
    /// <summary>
    /// This method fills the timetable grid.
    /// </summary>
    private void FillGrid()
    {
        miWeekDayId = Convert.ToInt32(cmbWeekDay.SelectedValue);
        DataTable oDt = SchoolTimeTableMasterBL.GetWeekDayTimeTable(miSchoolId, miAcademicYearId, miWeekDayId);
        grdTemp.DataSource = oDt.DefaultView;
        grdTemp.DataBind();
        GenerateColumns(oDt);
    }
    /// <summary>
    /// This method adds the columns to the grid.
    /// </summary>
    /// <param name="aoDs"></param>
    private void GenerateColumns(DataTable aoDs)
    {
        GenerateHeaderRowCols(aoDs);
        GenerateOtherColumns(aoDs);

    }
    /// <summary>
    /// This method adds lecture No. columns to the header row of the grid.
    /// </summary>
    /// <param name="aoDs"></param>
    private void GenerateHeaderRowCols(DataTable aoDs)
    {
        int iCount = aoDs.Columns.Count - 1;
        //Loop to add Divisions in Header ROw 
        for (int iColIndex = 1; iColIndex < iCount; iColIndex++)
        {
            TableCell oTableCell1 = new TableCell();
            oTableCell1.HorizontalAlign = HorizontalAlign.Center;
            //oTableCell1.Width = System.Web.UI.WebControls.Unit.Point(900);
            oTableCell1.Wrap = false;
            oTableCell1.Style.Add(HtmlTextWriterStyle.Padding, "2");
            int iColName = iColIndex - I_COLUMN_NUMBERS + 1;
            oTableCell1.Text = S_LECTURE + " " + iColName.ToString();
            grdTemp.HeaderRow.Cells.Add(oTableCell1);
        }

    }
    /// <summary>
    /// This method adds lecture No. columns to other rows of the grid.
    /// </summary>
    /// <param name="aoDs"></param>
    private void GenerateOtherColumns(DataTable aoDs)
    {
        TeacherSubjectAssignmentCollectionBL oTeacherSubject = new TeacherSubjectAssignmentCollectionBL();
        miWeekDayId = Convert.ToInt32(cmbWeekDay.SelectedValue);
        DataSet oDsTeacherSubjects = oTeacherSubject.RetriveTeacherClassSubjectsForTT(miSchoolId, miAcademicYearId, miWeekDayId);
        ViewState[Constants.S_SESSION_TEACHER_SUBJECT_DS] = oDsTeacherSubjects;
        DataTable oDtClassTeachers;
        DataRow[] oDrClassTeachers;
        DropDownList oDr;
        int iColCount = aoDs.Columns.Count - 2;
        int iRowCount = aoDs.Rows.Count;
        for (int i = 0; i < iRowCount; i++)
        {
            for (int j = I_COLUMN_NUMBERS; j <= iColCount; j++)
            {
                TableCell oTableCell = new TableCell();
                oTableCell.HorizontalAlign = HorizontalAlign.Center;
                int iLectureNo = j - I_COLUMN_NUMBERS + 1;
                oTableCell.Wrap = false;
                oTableCell.Style.Add(HtmlTextWriterStyle.Padding, "2");
                string sFieldName = S_LECTURE + j.ToString();
                oTableCell.Text = aoDs.Rows[i][sFieldName].ToString();
                int iCellIndex = grdTemp.Rows[i].Cells.Add(oTableCell);
                oTableCell.Attributes.Add("title", grdTemp.Rows[i].Cells[0].Text + "[ Lect " + j.ToString() + "]");
                oDr = new DropDownList();
                oDr.CssClass = S_CSSCLASS_COMBO;
                oDr.ID = "dr" + i.ToString() + "_" + j.ToString();
                grdTemp.Rows[i].Cells[iCellIndex].Controls.Add(oDr);
                DataRow[] oDrLectures = oDsTeacherSubjects.Tables[i].Select(S_MAXLECTURE_STD_FIELD + " >=" + iLectureNo);
                if (j != I_COLUMN_NUMBERS)
                {
                    ControlUtility.FillDropDownList(oDrLectures, ref oDr, S_TEACHER_SUBJECT_ID_FIELD, S_CLASSSUBJECT_NAME_FIELD, Constants.S_SELECT);
                    if (oDr.Items.Count < 2)
                    {
                        oTableCell.CssClass = S_CSS_NA;
                        oDr.Visible = false;
                        oTableCell.Text = S_TXTLECTURENA;
                        oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
                    }
                }
                else
                {
                    oDtClassTeachers = oDsTeacherSubjects.Tables[i].Clone();
                    oDtClassTeachers.Rows.Clear();
                    oDrClassTeachers = oDsTeacherSubjects.Tables[i].Select("Is_ClassTeacher = '" + Constants.C_YES.ToString() + "'");
                    ControlUtility.FillDropDownList(oDrClassTeachers, ref oDr, S_TEACHER_SUBJECT_ID_FIELD, S_CLASSSUBJECT_NAME_FIELD, Constants.S_SELECT);
                    if (oDr.Items.Count < 2)
                    {
                        oTableCell.CssClass = S_CSS_NOTCLASS_TEACHER;
                        oTableCell.Font.Bold = false;
                        oDr.Visible = false;
                        oTableCell.Text = S_TXTCLASSTEACHER;
                        oTableCell.Style.Add(HtmlTextWriterStyle.Color, "SlateGray");
                        oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "8pt");
                    }
                    oDr.Attributes.Add("title", "For - " + j.ToString() + " : " + grdTemp.Rows[i].Cells[0].Text);
                }
                if (Convert.ToInt32(aoDs.Rows[i][sFieldName]) != 0)
                {
                    oDr.CssClass = S_CSSCLASS_COMBO_SELECTED;
                }
                else
                {
                    oDr.CssClass = S_CSSCLASS_COMBO;
                }
                oDr.SelectedValue = oTableCell.Text;

            }
        }
    }

    /// <summary>
    /// This method fills the combo with weekdays.
    /// </summary>
    private void FillWeekdayCombo()
    {
        WeekDaysMasterBL obj = new WeekDaysMasterBL();
        DataTable oDs = obj.GetConfiguredWeekDays(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oDs, ref cmbWeekDay, "WeekDays_id", "WeekDay_name", Constants.S_SELECT);
    }

    /// <summary>
    /// This method creates an XML for Time table
    /// The XML format: 
    /// </summary>
    /// <returns>
    /// Arraay of xml strings.
    // 1: for master: <DaywiseTimeTableMaster><DaywiseTimeTable Standard_division_id ="1"/><DaywiseTimeTable Standard_division_id ="2"/></DaywiseTimeTableMaster>'
    // 2: for details:  '<DaywiseTimeTableDetails><DaywiseTimeTableDetail Standard_division_id ="1" Lecture_Number ="1" Teacher_ID ="1" Subject_Id ="1257" />
    //<DaywiseTimeTableDetail Standard_division_id ="2" Lecture_Number ="2" Teacher_ID ="1" Subject_Id ="1257" />
    //</DaywiseTimeTableDetails>
    /// </returns>
    private string[] GetXMLForTimeTable()
    {
        string[] sArrStrXml = new string[2];
        int iRowCount = grdTemp.Rows.Count;
        int iColumnCount = grdTemp.Rows[0].Cells.Count;
        ArrayList arrIncludedStdDivs = new ArrayList();
        const string S_ELEMENT = "element";

        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("DaywiseTimeTableMaster");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTableMaster", "");
        XmlDocument oDocDetail = new XmlDocument();
        XmlElement rootDetail = oDocDetail.CreateElement("DaywiseTimeTableDetails");
        XmlElement DetailRootNode = oDocDetail.CreateElement("DaywiseTimeTableDetails");
        //create root level node.
        DataSet oDs = (DataSet)ViewState[Constants.S_SESSION_TEACHER_SUBJECT_DS];
        int iStdDivId = 0;
        for (int i = 0; i < iRowCount; i++)
        {
            DataTable oDt = oDs.Tables[i];
            string sAtrrName;
            XmlAttribute attr;
            XmlNode oXmlNode;
            XmlNode oXmlDetailNode;
            for (int j = I_COLUMN_NUMBERS; j < iColumnCount; j++)
            {
                if (grdTemp.Rows[i].Cells[j].Controls.Count > 0)
                {
                    DropDownList oCmb = (DropDownList)grdTemp.Rows[i].Cells[j].Controls[0];
                    if (!oCmb.SelectedValue.Equals("0"))
                    {
                        oXmlDetailNode = oDocDetail.CreateNode(S_ELEMENT, "DaywiseTimeTableDetail", "");

                        DataRow[] oDr = oDt.Select("Teacher_Subject_Id = " + oCmb.SelectedValue);
                        if (oDr.Length > 0)
                        {
                            iStdDivId = Convert.ToInt32(oDr[0][S_STD_DIV_ID_FIELD]);
                        }
                        if (!arrIncludedStdDivs.Contains(iStdDivId))
                        {
                            arrIncludedStdDivs.Add(iStdDivId);
                            oXmlNode = oDoc.CreateNode(S_ELEMENT, "DaywiseTimeTable", "");

                            sAtrrName = "Standard_division_id"; //oRow.Cells[iColCount]
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = iStdDivId.ToString();
                            oXmlNode.Attributes.Append(attr);

                            oXmlRootNode.AppendChild(oXmlNode);
                            root.AppendChild(oXmlRootNode);
                        }
                        sAtrrName = "Standard_division_id"; //oRow.Cells[iColCount]
                        attr = oDocDetail.CreateAttribute(sAtrrName);
                        attr.Value = iStdDivId.ToString();
                        oXmlDetailNode.Attributes.Append(attr);

                        string sSubjectId = oDr[0]["Subject_Id"].ToString();
                        sAtrrName = "Subject_Id"; //oRow.Cells[iColCount]
                        attr = oDocDetail.CreateAttribute(sAtrrName);
                        attr.Value = sSubjectId;
                        oXmlDetailNode.Attributes.Append(attr);


                        int iLectureNo = j - I_COLUMN_NUMBERS + 1;
                        sAtrrName = "Lecture_Number"; //oRow.Cells[iColCount]
                        attr = oDocDetail.CreateAttribute(sAtrrName);
                        attr.Value = iLectureNo.ToString();
                        oXmlDetailNode.Attributes.Append(attr);

                        sAtrrName = "Teacher_ID"; //oRow.Cells[iColCount]
                        attr = oDocDetail.CreateAttribute(sAtrrName);
                        attr.Value = grdTemp.DataKeys[i][0].ToString();//grdTemp.Rows[i].Cells[0].Text;
                        oXmlDetailNode.Attributes.Append(attr);

                        DetailRootNode.AppendChild(oXmlDetailNode);
                        rootDetail.AppendChild(DetailRootNode);
                    }
                }
            }
        }
        sArrStrXml[0] = root.InnerXml;
        sArrStrXml[1] = rootDetail.InnerXml;
        return sArrStrXml;
    }
    /// <summary>
    /// This method is called from click event handler of search button.
    /// It changes the caption of the button.
    /// And changes read only status of the registration no. text box.
    /// </summary>
    /// <returns>
    /// True: 
    /// False:
    /// </returns>
    private bool ToggleStatus()
    {
        bool bReturn = true;
        if (btnShow.Text.Equals(S_SHOW) && cmbWeekDay.SelectedIndex != 0)
        {
            btnShow.Text = S_CHANGE;
            cmbWeekDay.Enabled = false;
            bReturn = true;
        }
        else
        {
            btnShow.Text = S_SHOW;
            cmbWeekDay.Enabled = true;
            bReturn = false;
        }
        return bReturn;
    }
    /// <summary>
    /// This method is called when precondition  is not satified OR
    /// When logged in user doesnt have rights to set the timetable.
    /// It hides all the fields except error labels.
    /// </summary>
    private void HideAllFields()
    {
        divTimeTable.Visible = false;
        tblInputFields.Visible = false;
        btnSave.Visible = false;
        LegendTable.Visible = false;
    }
    #region Validations

    /// <summary>
    /// This method checks if the logged in user is valid user
    /// i.e. admin 
    /// </summary>
    /// <returns>
    /// true: if admin
    /// false: non admin
    /// </returns>

    private bool ValidateUser()
    {
        bool bIsValidUser = false;
        if (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor)
        {
            bIsValidUser = true;
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "Access Denied !!";
            HideAllFields();
        }
        return bIsValidUser;

    }

    /// <summary>
    /// This is a wrapper procedure:
    /// 1. calls other validation procedures.
    /// 2. Prints the errors if any
    /// </summary>
    /// <returns>
    /// true: if valid TT
    /// false: otherwise
    /// </returns>
    private bool ValidateData()
    {
        bool bReturn = true;
        string sErrMsgForTeachers = TeacherValidations();
        string sErrMsg = ValidateForDuplicateClassForLecture();

        if (!sErrMsg.Equals("") || !sErrMsgForTeachers.Equals(""))
        {
            lblError.Visible = true;
            lblError.Text = sErrMsg + sErrMsgForTeachers;
            bReturn = false;

        }
        return bReturn;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// Error message if any or blank string
    /// </returns>
    private string TeacherValidations()
    {
        string sErrMsgDay = "";
        string sErrMsgForWeek = "";
        string sReturnMsg = "";
        miWeekDayId = Convert.ToInt32(cmbWeekDay.SelectedValue);
        DataSet oDsLectures = SchoolTimeTableMasterBL.GetTeacherLectureDetails(miSchoolId, miAcademicYearId, miWeekDayId);
        DataSet oDsGlobal = (DataSet)ViewState[Constants.S_SESSION_TEACHER_SUBJECT_DS];
        int iRowCount = grdTemp.Rows.Count;
        int iColCount = grdTemp.Rows[0].Cells.Count;
        const string S_MAX_WEEKLY_LECTURES = "LecturesPerWeek";
        const string S_MAX_LECTURES_PERDAY = "LecturesInaday";
        const string S_LECTURES_ASSIGNED = "LecturesAssigned";
        //SELECT @Teacher_Id as Teacher_Id ,@maxLecturesPerWeek as LecturesPerWeek,@maxLecturesInWeekDay as LecturesInaday,@LecturesAssignedInWeek as lecturesAssigned
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            string sTeacherName = grdTemp.Rows[iRowIndex].Cells[I_TEACHERID_COLUMN_NO].Text;
            int iTeacherId = Convert.ToInt32(grdTemp.DataKeys[iRowIndex][0]);
            int iLecturesAssigned = 0;
            int iMaxLecturesPerWeek = 0;
            int iMaxLecturesPerDay = 0;
            int iCount = 0;

            DataRow[] oDr = oDsLectures.Tables[0].Select("Teacher_id =" + iTeacherId.ToString());
            if (oDr.Length > 0)
            {
                iLecturesAssigned = Convert.ToInt32(oDr[0][S_LECTURES_ASSIGNED]);
                iMaxLecturesPerWeek = Convert.ToInt32(oDr[0][S_MAX_WEEKLY_LECTURES]);
                iMaxLecturesPerDay = Convert.ToInt32(oDr[0][S_MAX_LECTURES_PERDAY]);
            }
            for (int iColIndex = I_COLUMN_NUMBERS; iColIndex < iColCount; iColIndex++)
            {
                if (grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls.Count > 0)
                {
                    DropDownList oCmb = (DropDownList)grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls[0];
                    if (oCmb.SelectedIndex != 0)
                    {
                        iCount++;
                    }
                }
            }
            int iTot = iLecturesAssigned + iCount;
            if (iCount > iMaxLecturesPerWeek)
            {
                sErrMsgForWeek = sErrMsgForWeek + sTeacherName + " (" + iMaxLecturesPerWeek + " )" + "<BR>";
            }
        }
        if (!sErrMsgForWeek.Equals(""))
        {
            sErrMsgForWeek = S_HEADER_MSG_TEACHER_WEEK + sErrMsgForWeek;
        }
        sReturnMsg = FormatMessage(sErrMsgDay, sErrMsgForWeek);
        return sReturnMsg;
    }

    /// <summary>
    /// This method formats the errormessages in such a format that can be displayed on the label.
    /// </summary>
    /// <param name="asErrMsg1"></param>
    /// <param name="asErrMsg2"></param>
    /// <returns>
    /// Well formatted message.
    /// </returns>
    private string FormatMessage(string asErrMsg1, string asErrMsg2)
    {
        string sReturnMsg = "";
        if (!asErrMsg1.Equals("") && !asErrMsg2.Equals(""))
        {
            sReturnMsg = asErrMsg1 + "<BR>" + asErrMsg2;
        }
        else
        {
            sReturnMsg = asErrMsg1 + asErrMsg2;
        }
        return sReturnMsg;
    }

    /// <summary>
    /// This method checks: 
    /// 1. if the 2 techers are associated to the same class, for the same lecture.
    ///     i.e. Check if the same class (std-div) appears in the  1 column.
    /// 2. If max lecture count for class-subject is exceeded.
    /// </summary>
    /// <returns>
    /// Error message if any or blank string
    /// </returns>
    private string ValidateForDuplicateClassForLecture()
    {
        //string sReturn = "";
        int iRowCount = grdTemp.Rows.Count;
        string sErrMsg = "";
        string sErrMaxLimiExceededMsg = "";
        //S_STD_DIV_NAME_FIELD
        if (iRowCount > 0)
        {
            int iColCount = grdTemp.Rows[0].Cells.Count;
            Hashtable oHash = GetClassSubjectCount();
            DataSet oDs = (DataSet)ViewState[Constants.S_SESSION_TEACHER_SUBJECT_DS];
            ArrayList arrStdDivId = new ArrayList();
            ArrayList arrDupStdDivId = new ArrayList();
            ArrayList arrClassSubjectId = new ArrayList();

            for (int iColIndex = I_COLUMN_NUMBERS; iColIndex < iColCount; iColIndex++)
            {
                arrStdDivId.Clear();
                arrDupStdDivId.Clear();
                for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
                {
                    DataTable oDt = oDs.Tables[iRowIndex];
                    if (grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls.Count > 0)
                    {
                        DropDownList oCmb = (DropDownList)grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls[0];
                        DataRow[] oDr = oDt.Select("Teacher_Subject_Id = " + oCmb.SelectedValue);
                        if (oDr.Length > 0)
                        {
                            //duplicate check 
                            if (!arrStdDivId.Contains(oDr[0][S_STD_DIV_ID_FIELD].ToString()))
                            {
                                arrStdDivId.Add(oDr[0][S_STD_DIV_ID_FIELD].ToString());
                            }
                            else
                            {
                                if (!arrDupStdDivId.Contains(oDr[0][S_STD_DIV_NAME_FIELD].ToString()))
                                    arrDupStdDivId.Add(oDr[0][S_STD_DIV_NAME_FIELD].ToString());
                            }
                            string sClassSubjectName = oDr[0][S_CLASSSUBJECT_NAME_FIELD].ToString();
                            int iClassSubjectId = Convert.ToInt32(oDr[0][S_CLASS_SUBJECT_ID_FIELD]);
                            int iLecturesAssigned = Convert.ToInt32(oDr[0][S_LECTURES_ASSIGNED]);
                            int iMaxLecturesPerWeek = Convert.ToInt32(oDr[0][S_STD_SUBJECT_MAXLECTURES_PERWEEK_FIELD]);
                            int iTotLecturesAssigned = Convert.ToInt32(oHash[iClassSubjectId]) + iLecturesAssigned;

                            if (iTotLecturesAssigned > iMaxLecturesPerWeek)
                            {
                                if (!arrClassSubjectId.Contains(iClassSubjectId))
                                {
                                    arrClassSubjectId.Add(iClassSubjectId);
                                    sErrMaxLimiExceededMsg = sErrMaxLimiExceededMsg + "<BR>" + sClassSubjectName + ".";
                                }
                            }
                        }
                    }
                }
                int iDupCount = arrDupStdDivId.Count;
                //check if duplicate class for this column(Lecture)
                //and create message
                if (iDupCount > 0)
                {
                    iColIndex = iColIndex - I_COLUMN_NUMBERS + 1;
                    string sStdDivList = "";
                    for (int i = 0; i < iDupCount; i++)
                    {

                        if (i != 0)
                        {
                            sStdDivList = sStdDivList + "," + arrDupStdDivId[i].ToString();
                        }
                        else
                        {
                            sStdDivList = arrDupStdDivId[i].ToString();
                        }
                    }
                    sErrMsg = sErrMsg + S_LECTURE + " " + iColIndex.ToString() + " :  " + sStdDivList + "<BR>";
                }
            }
        }
        if (!sErrMsg.Equals(""))
        {
            sErrMsg = S_HEADER_MSG_DUPLICATE_CLASS + "<BR> " + sErrMsg + "<BR> ";
        }
        if (!sErrMaxLimiExceededMsg.Equals(""))
        {
            sErrMaxLimiExceededMsg = S_HEADER_MSG_CLASSSUBJECT_WEEK + "<BR> " + sErrMaxLimiExceededMsg + "<BR>";
        }
        return sErrMsg + sErrMaxLimiExceededMsg;
    }

    /// <summary>
    /// This method gets the max lectures count for each class subject.
    /// </summary>
    /// <returns>
    /// Hashtable:
    /// Class-Subject_Id as key
    /// Max lectures limit as value.
    /// </returns>
    private Hashtable GetClassSubjectCount()
    {
        Hashtable oHashReturn = new Hashtable();
        int iRowCount = grdTemp.Rows.Count;

        //S_STD_DIV_NAME_FIELD
        if (iRowCount > 0)
        {
            DataSet oDs = (DataSet)ViewState[Constants.S_SESSION_TEACHER_SUBJECT_DS];
            int iColCount = grdTemp.Rows[0].Cells.Count;
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                for (int iColIndex = I_COLUMN_NUMBERS; iColIndex < iColCount; iColIndex++)
                {
                    DataTable oDt = oDs.Tables[iRowIndex];
                    if (grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls.Count > 0)
                    {
                        DropDownList oCmb = (DropDownList)grdTemp.Rows[iRowIndex].Cells[iColIndex].Controls[0];
                        DataRow[] oDr = oDt.Select("Teacher_Subject_Id = " + oCmb.SelectedValue);
                        if (oDr.Length > 0)
                        {
                            int iClassSubjectId = Convert.ToInt32(oDr[0][S_CLASS_SUBJECT_ID_FIELD]);
                            if (!oHashReturn.ContainsKey(iClassSubjectId))
                            {
                                oHashReturn.Add(iClassSubjectId, 1);
                            }
                            else
                            {
                                oHashReturn[iClassSubjectId] = Convert.ToInt32(oHashReturn[iClassSubjectId]) + 1;
                            }
                        }
                    }
                }
            }
        }
        return oHashReturn;
    }
    #endregion
    #endregion
}
