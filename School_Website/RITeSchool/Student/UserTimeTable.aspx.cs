using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;

public partial class UserTimeTable : SchoolBase
{
    #region constants
    private const string S_IMG_GRD_HEAD = "~/RITeSchool/images/GridHead_LectWdayTT.gif";
    private const string S_CSS_NA = "UsrTTNA";
    private const string S_ERR_MSG = "Timetable not yet configured.";
    #endregion

    private const int I_STATICBOUND_COLS_COUNT = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
                FillGrid();
            
            hlnkTTSchedule.Attributes.Add("onclick", "window.open('../Student/TimeTable.aspx','_blank','scrollbars=no,statusbar=no,width=650,height=630'); return false;");
        }
        catch (Exception ex)
        {
			BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    private void FillGrid()
    {
        DataSet oDs = null;
        Image oImg = new Image();
        oImg.ImageUrl = S_IMG_GRD_HEAD;
        oImg.ImageAlign = ImageAlign.Top;
        oImg.ImageAlign = ImageAlign.Left;

        if (moUserRole == Constants.UserRoles.Student)
        {
            int iStdDivId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
            oDs = SchoolTimeTableMasterBL.GetTimeTableDisplayForStudent(miSchoolId, miAcademicYearId, iStdDivId);
            grdTT.DataSource = oDs;
            grdTT.DataBind();
        }
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            oDs = SchoolTimeTableMasterBL.GetTimeTableDisplayForTeacher(miSchoolId, miAcademicYearId, iTeacherId);
            grdTT.DataSource = oDs;
            grdTT.DataBind();
        }
        GenerateColumns();
        if (grdTT.HeaderRow != null)
        {
            if (grdTT.HeaderRow.Cells.Count > 0)
                grdTT.HeaderRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            
        }
		
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            GetLectureCountsForTeachers();
            grdAdditionalClasses.DataSource = oDs.Tables[5];
            grdAdditionalClasses.DataBind();
            divSubjectLect.Visible = true;
            divAdditionalLect.Visible = true;
        }
        else
        {
            divSubjectLect.Visible = false;
            divAdditionalLect.Visible = false;
            grdSubjectLect.Visible = false;
            grdAdditionalClasses.Visible = false;
        }
        if (lblError.Text != "")
        {
            divSubjectLect.Visible = false;
            divAdditionalLect.Visible = false;
            grdAdditionalClasses.Visible = false;
            grdSubjectLect.Visible = false;
            grdTT.Visible = false;
            tdLegend.Visible = false;
        }
    }


    private void GetLectureCountsForTeachers()
    {
        int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
		string sConsiderAssembly = Settings.IsAssemblyApplicable ? Constants.S_YES : Constants.S_NO;
		string sConsiderMPT = Settings.IsMPTApplicable ? Constants.S_YES : Constants.S_NO;
		string sConsiderStayback = Settings.IsStaybackApplicable ? Constants.S_YES : Constants.S_NO;
        string sConsiderWeeklyTest = Settings.IsWeeklyTestApplicable ? Constants.S_YES : Constants.S_NO;
        SchoolTimeTableMasterBL oSchoolTimeTableMasterBL = new SchoolTimeTableMasterBL();
        DataTable oDt = oSchoolTimeTableMasterBL.GetLectureCountsForTeachers(iTeacherId, sConsiderAssembly, sConsiderMPT, sConsiderStayback, sConsiderWeeklyTest);

        grdSubjectLect.DataSource = oDt;
        grdSubjectLect.DataBind();

        grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].CssClass = S_CSS_NA;
        grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].Font.Size = new FontUnit(8);
        grdSubjectLect.Rows[grdSubjectLect.Rows.Count - 1].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E6EEFC");
    }
   
    private void GenerateColumns()
    {
        GenerateHeaderRowCols();
        GenerateOtherColumns();
    }
    
    private void GenerateHeaderRowCols()
    {
        //already bound columns = 1
        DataSet oDs = (DataSet)grdTT.DataSource;
        int iCount = oDs.Tables[0].Columns.Count - (I_STATICBOUND_COLS_COUNT - 1);

        if (grdTT.Rows.Count > 0)
            //Loop to add Divisions in Header ROw 
            for (int iColIndex = I_STATICBOUND_COLS_COUNT; iColIndex <= iCount; iColIndex++)
            {
                TableCell oTableCell1 = new TableCell();
                oTableCell1.EnableViewState = false;
                oTableCell1.HorizontalAlign = HorizontalAlign.Center;

                oTableCell1.Width = System.Web.UI.WebControls.Unit.Point(900);
                oTableCell1.Wrap = false;
                oTableCell1.Style.Add(HtmlTextWriterStyle.Padding, "2");
                string sColNAme = oDs.Tables[0].Columns[iColIndex].ColumnName;
                oTableCell1.Text = sColNAme;
                int iCellIndex = grdTT.HeaderRow.Cells.Add(oTableCell1);
            }
    }

    protected void grdTT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > 0)
        {
            if (e.Row.Cells[0].Text == "Total Lectures")
            {
                e.Row.Cells[0].Font.Size = new FontUnit(8);
                e.Row.Cells[0].CssClass = S_CSS_NA;
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E6EEFC");
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Color, "Black");
            }
        }
    }

    private void GenerateOtherColumns()
    {

        //dataset for comboes
        DataSet oDs = (DataSet)grdTT.DataSource;
        int iRowCount = oDs.Tables[0].Rows.Count;
        int iColCount = oDs.Tables[0].Columns.Count - (I_STATICBOUND_COLS_COUNT - 1);
        bool bIsConfig = false;
        const string S_OFF = "N/C";
        TableCell oTableCell = null;
        Constants.UserRoles eUserRoles = (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID];
        for (int i = 0; i < iRowCount; i++)//lectures
           for (int j = I_STATICBOUND_COLS_COUNT; j <= iColCount; j++) //weekdays  
            {
                int iWeekdayIndex = j - I_STATICBOUND_COLS_COUNT;
                oTableCell = new TableCell();
                oTableCell.EnableViewState = false;
                oTableCell.HorizontalAlign = HorizontalAlign.Center;
                oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
                oTableCell.Wrap = false;
                oTableCell.Height = 35;
                string sColName = oDs.Tables[0].Columns[j].ColumnName;
                oTableCell.Text = oDs.Tables[0].Rows[i][sColName].ToString();

                if (!oTableCell.Text.Equals("N/A"))
                    bIsConfig = true;
                else
                {
                    int iMaxCnt = Convert.ToInt32(oDs.Tables[1].Rows[iWeekdayIndex]["MaxLectures"]);
                    if (i < iMaxCnt)
                    {
                        oTableCell.Text = "<b><font color=\"#000\" face=\"Verdana\" size=\"2\">" + S_OFF + "</font></b>";
                    }
                    oTableCell.CssClass = S_CSS_NA;
                }

				string sText = string.Empty;
                DataTable  oDtExtraLecture = oDs.Tables.Count > 6 ? oDs.Tables[6] : null;
                DataTable oDtAssembly = oDs.Tables[3]; //For Adssembly student
                DataTable oDtMPT = oDs.Tables[4];//For Stayback student
                DataTable oDtWeeklyTest = oDs.Tables[6];
                sText = GetAssemblyMPTstaybackText(i + 1, sColName, oDs.Tables[2], oDtExtraLecture, oDtAssembly, oDtMPT, oDtWeeklyTest);
                if (sText != "")
                {
                    oTableCell.Text = sText;
                    oTableCell.Style.Add(HtmlTextWriterStyle.Color, "#017df6");
                    oTableCell.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
                    oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "10pt");
                }
                else
                    if (eUserRoles == Constants.UserRoles.Student)
                    {
                        DataTable oDtAdditional = oDs.Tables[5];
                        DataRow[] oArrRows = oDtAdditional.Select("Lecture_Number = " + (i + 1) + " AND WeekDay_Name ='" + sColName + "'");
                        if (oArrRows.Length > 0)
                        {
                            string sSubjectName = "";
                            if (oTableCell.Text.Contains(S_OFF))
                            {
                                oTableCell.Text = "";
                                oTableCell.Style.Add(HtmlTextWriterStyle.Color, "#017df6");
                                oTableCell.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
                                oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "10pt");
                                if (oArrRows.Length > 1)
                                {
                                    for (int iCnt = 0; iCnt < oArrRows.Length; iCnt++)
                                        sSubjectName += iCnt == 0 ? "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + oArrRows[iCnt]["Subject_Name"].ToString() + "</font></b>" : "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">/" + oArrRows[iCnt]["Subject_Name"].ToString() + "</font></b>";
                                }
                                else
                                {
                                    sSubjectName = "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + oArrRows[0]["Subject_Name"].ToString() + "</font></b>";
                                }
                            }
                            else
                            {
                              if (oArrRows.Length > 1)
                                {
                                    for (int iCnt = 0; iCnt < oArrRows.Length; iCnt++)
                                        sSubjectName += "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">/" + oArrRows[iCnt]["Subject_Name"].ToString()+"</font></b>";
                                }
                                else
                                {
                                    sSubjectName = "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">/" + oArrRows[0]["Subject_Name"].ToString() + "</font></b>";                                    
                                }
                            }

                            oTableCell.Text = oTableCell.Text + sSubjectName;
                        }
                    }
                

                int iCellIndex = grdTT.Rows[i].Cells.Add(oTableCell);
                if (i == iRowCount - 1 && (Constants.UserRoles) Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher)
                {
                    oTableCell.CssClass = S_CSS_NA;
                    oTableCell.Style.Add(HtmlTextWriterStyle.FontSize, "9pt");
                    oTableCell.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E6EEFC");
                    oTableCell.Style.Add(HtmlTextWriterStyle.Color, "Black");
                }
            }
        
        if (eUserRoles == Constants.UserRoles.Teacher)
            DisplayLectureCount();
        
        if (!bIsConfig)
        {
            tdError.Visible = true;
            lblError.Text = S_ERR_MSG;
            divAdditionalLect.Visible = false;
        }
    }

    private void DisplayLectureCount()
    {
        int iRowCount = grdTT.Rows.Count;
        int iColCount = grdTT.HeaderRow.Cells.Count - 1;
        string sAssembly = Settings.AssemblyName;
        string sMPT = Settings.MPTName;
        string sStayback = Settings.StaybackName;
        for (int j = 1; j <= iColCount; j++)
        {
            int iColLectCount = 0;
            try { iColLectCount = Convert.ToInt32(grdTT.Rows[iRowCount - 1].Cells[j].Text); }
            catch { }
            for (int i = 0; i < iRowCount - 1; i++)
            {
                if (grdTT.Rows[i].Cells[j].Text.Contains(sAssembly))
                    iColLectCount += 1;

                if (grdTT.Rows[i].Cells[j].Text.Contains(sMPT))
                    iColLectCount += 1;

                if (grdTT.Rows[i].Cells[j].Text.Contains(sStayback))
                    iColLectCount += 1;
                
            }
            grdTT.Rows[grdTT.Rows.Count - 1].Cells[j].Text = iColLectCount.ToString();
        }
    }

    private string GetAssemblyMPTstaybackText(int aiLectNo, string asWeekdayName, DataTable aoDtStayback, DataTable aoMptAssemblyApplicable, DataTable aoAssemblyLecture, DataTable aoDtMPT, DataTable aoDtWeeklyTest)
    {
        string sDesc = "";
		if (Settings.IsAssemblyApplicable ||
           Settings.IsMPTApplicable||
            Settings.IsStaybackApplicable || Settings.IsWeeklyTestApplicable)
        {
            string sAssembly = Settings.AssemblyName;
            string sMPT = Settings.MPTName;
            string sStayback = Settings.StaybackName;
            string sWeeklyTest = Settings.WeeklyTestName;

            int iAssemblyLectNo = Settings.AssemblyLectNo;
            string sAssemblyWeekday = Settings.AssemblyWeekday;
            int iMPTLectNo = Settings.MPTLectNo;
            string sMPTWeekday =Settings.MPTWeekday;
            int iWeeklyLectNo = Settings.WeeklyTestLectNo;
            string sWeeklyTestDay = Settings.WeeklyTestWeekDay;

            string sAssemblyApplicable = "";
            string sMPTApplicable = "";
            string sStaybackApplicable = "";
            string sWeeklyTestApplicable = "";

            if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher)
            {
				if (aoMptAssemblyApplicable != null && aoMptAssemblyApplicable.Rows.Count>0)
				{
					sAssemblyApplicable = aoMptAssemblyApplicable.Rows[0]["Assembly_Applicable"].ToString();
					sMPTApplicable = aoMptAssemblyApplicable.Rows[0]["MPT_Applicable"].ToString();
					sStaybackApplicable = aoMptAssemblyApplicable.Rows[0]["Stayback_Applicable"].ToString();                    
				}
            }
            else if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Student)
            {
                //if ((Settings.IsAssemblyApplicable &&
                //    aiLectNo == iAssemblyLectNo && asWeekdayName.ToLower() == sAssemblyWeekday.ToLower()) || 
                //    (Settings.IsMPTApplicable
                //    && aiLectNo == iMPTLectNo && asWeekdayName.ToLower() == sMPTWeekday.ToLower()))
                if (Settings.IsAssemblyApplicable && Settings.IsMPTApplicable) 
                {
                    int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
                    int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
                    int iDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);

                    SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
                    DataTable oDtClassTeacher = oSchoolWiseTeacherMasterBL.GetAssignedClassTeacher(iSchoolId, iStandardId, iDivisionId);
                    if (oDtClassTeacher.Rows.Count > 0)
                    {
                        sAssemblyApplicable = oDtClassTeacher.Rows[0]["Assembly_Applicable"].ToString();
                        sMPTApplicable = oDtClassTeacher.Rows[0]["MPT_Applicable"].ToString();
                    }
                }
                sStaybackApplicable = Settings.IsStaybackApplicable? true.ToString() : false.ToString();
                sWeeklyTestApplicable = Settings.IsWeeklyTestApplicable ? true.ToString() : false.ToString();
            }
            if (Settings.IsAssemblyApplicable)
                if (sAssemblyApplicable.ToLower() == Constants.C_YES.ToString().ToLower())
                    //sDesc = (aiLectNo == iAssemblyLectNo && asWeekdayName.ToLower() == sAssemblyWeekday.ToLower()) ? "<b>" + sAssembly + "</b>" : "";
                 {
                     DataRow[] oArrRowsAssembly = aoAssemblyLecture.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                     sDesc = oArrRowsAssembly.Length > 0 ? "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + sAssembly + "</font></b>" : "";
                 }
            if (Settings.IsMPTApplicable)
                if (sMPTApplicable.ToLower() == Constants.C_YES.ToString().ToLower() && sDesc == "") //sDesc = (aiLectNo == iMPTLectNo && asWeekdayName.ToLower() == sMPTWeekday.ToLower()) ? "<b>" + sMPT + "</b>" : "";
                {
                    DataRow[] oArrRowsMPT = aoDtMPT.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                    sDesc = oArrRowsMPT.Length > 0 ? "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + sMPT + "</font></b>" : "";
                }

            if (Settings.IsStaybackApplicable)
                if (sStaybackApplicable.ToLower() == true.ToString().ToLower() && sDesc == "")
                {
                    DataRow[] oArrRows = aoDtStayback.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                    sDesc = oArrRows.Length > 0 ? "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + sStayback + "</font></b>" : "";
                }

            if (Settings.IsWeeklyTestApplicable)
                if (sWeeklyTestApplicable.ToLower() == true.ToString().ToLower() && sDesc == "")
                {
                    DataRow[] oArrRows = aoDtWeeklyTest.Select("WeekDay_Name='" + asWeekdayName + "' AND Lecture_Number=" + aiLectNo);
                    sDesc = oArrRows.Length > 0 ? "<b><font color=\"#017df6\" face=\"Verdana\" size=\"2\">" + sWeeklyTest + "</font></b>" : "";
                }
        }
        return sDesc;
    }
}
