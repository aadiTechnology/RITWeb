using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
/// <summary>
/// This class displays the preview of the changed settings made by the user.
/// </summary>
public partial class TransferPreview : TeacherTransferBaseUI
{
    #region constants
    const int I_DATAKEY_ROWSTATE = 1;
    const string S_CSS_ADDED = "ClsHilightBGB";
    const string S_CSS_TRANSFERED = "LblErrorMsg";
    const string S_CSS_DELETED = "SubDeleted";

    protected const int I_TBL_CLASSTEACHER = 0;
    protected const int I_TBL_SRCSUBJECTTEACHER = 1;
    protected const int I_TBL_TARGETSUBJECTTEACHER = 2;

    protected const int I_TBL_TTWEEKDAY = 4;
    protected const int I_TBL_TEACHERSTT = 3;
    protected const int I_TBL_MAINTT = 5;
    protected const int I_TBL_LECTURES = 6;

    private const int I_TBL_ASSEMBLY = 8;
    private const int I_TBL_STAYBACK = 9;

    protected const string S_DB_COL_ROWSTATE = "RowState";
    #endregion
    DataSet moDSTeacherTransfer;
    /// <summary>
    /// This event handler is called everytime the page loaded.
    /// It gets the dataset from session, and it stores the copy of this dataset into a member variable.
    /// And then calls the methods to display preview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {		
            if (!IsPostBack)
            {
                //get dataset from session
                DataSet oDsTransfer = (DataSet)Session[Constants.S_TEMP_SESSION_DS];
                moDSTeacherTransfer = oDsTransfer.Copy();
                //show teacher names.
                ShowTeachers();
                //display preview
                PrepareAndShowPreview();
            }

            btnCancel.Attributes["onmouseover"] = "javascript:fnover('" + btnCancel.ClientID + "');";
            btnCancel.Attributes["onmouseout"] = "javascript:fnout('" + btnCancel.ClientID + "');";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method displays the source and desination teacher names.
    /// </summary>
    private void ShowTeachers()
    {
        DataTable oDtTeachers = moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT];
        lblSubjectHeader.Text += oDtTeachers.Rows[0]["TeacherName"].ToString();
        lblTargetSubjectHeader.Text += oDtTeachers.Rows[1]["TeacherName"].ToString();
        lblSrcTeacher.Text += oDtTeachers.Rows[0]["TeacherName"].ToString();
        lblTargetTeacher.Text += oDtTeachers.Rows[1]["TeacherName"].ToString();
    }
    /// <summary>
    /// This is row databound event for src teacher's subjects grid.
    /// it  changes the the style of the row according to the row state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdTeacher_Rowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iRowIndex = e.Row.RowIndex;
            string sTransferStyle = "<font color=\"blue\" style=\"text-decoration:line-through\">";
            string sEndTransferStyle = "</font>";
            if (iRowIndex >= Constants.I_ZERO)
            {
                string sRowState = grdSrcTeacher.DataKeys[iRowIndex][I_DATAKEY_ROWSTATE].ToString();
                if (sRowState.Equals(Constants.S_UPDATED))
                {
                    e.Row.CssClass = S_CSS_DELETED;
                    e.Row.Cells[0].Text = sTransferStyle + e.Row.Cells[0].Text + sEndTransferStyle;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This is row databound event for target teacher's subjects grid.
    /// it  changes the the style of the row according to the row state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdTargetTeacher_Rowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string sTransferStyle = "<font color=\"blue\" style=\"text-decoration:line-through\">";
            string sEndTransferStyle = "</font>";
            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= Constants.I_ZERO)
            {
                string sRowState = grdTargetTeacher.DataKeys[iRowIndex][I_DATAKEY_ROWSTATE].ToString();
                if (sRowState.Equals(Constants.S_ADDED))
                    e.Row.CssClass = S_CSS_ADDED;
                else if (sRowState.Equals(Constants.S_DELETED))
                {
                    e.Row.CssClass = S_CSS_DELETED;
                    e.Row.Cells[0].Text = sTransferStyle + e.Row.Cells[0].Text + sEndTransferStyle;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    /// <summary>
    /// This method calls the BL method to prepare dataset for prevw display.
    /// And other member methods to display the preview.
    /// </summary>
    private void PrepareAndShowPreview()
    {
        TeacherTransferBL obj = GetObject();
        ShowMsg(obj);
        obj.PreparePreview();
        moDSTeacherTransfer = obj.TeacherTransferDS;
        //show class teacher assignment
        ShowClassteacherAssignment();
        //show src teacher subjects
        grdSrcTeacher.DataSource = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
        grdSrcTeacher.DataBind();
        //show target teacher subjects
        grdTargetTeacher.DataSource = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER];
        grdTargetTeacher.DataBind();
        //show timetable
        ShowTimetable();
    }
    /// <summary>
    /// This method displays the messages indicating the changes done by the user.
    /// </summary>
    /// <param name="obj"></param>
    private void ShowMsg(TeacherTransferBL obj)
    {
        obj.PrepareMsgsForPreview();
        lblClassteacherMsg.Text = obj.SrcClassTeacherTransferMsg;
        lblTargetClassteacherMsg.Text = obj.TargetClassTeacherTransferMsg;
        lblTransferSubjectMsg.Text = obj.SrcSubjectTransferMsg;
        lblDeletedSubjectMsg.Text = obj.TargetSubjectTransferMsg;
        lblTTStatus.Text = obj.TransferTTMsg;
    }
    /// <summary>
    /// This method displays the time table according to the changes made by user.
    /// </summary>
    private void ShowTimetable()
    {
        if (moDSTeacherTransfer.Tables.Count > I_TBL_TTWEEKDAY)
        {
            //set base class members
            Timetable oTT = new Timetable(moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT], moDSTeacherTransfer.Tables[I_TBL_MAINTT], moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY], moDSTeacherTransfer.Tables[I_TBL_LECTURES], moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY], moDSTeacherTransfer.Tables[I_TBL_STAYBACK]);
            //base.oTransferTT = oTT;
            //DataTable oDtTeachers = moDsTransfer.Tables[I_TBL_CLASSTEACHER];
            int iSrcTeacherId = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].Rows[0]["Teacher_Id"]);
            int iTargetTeacherId = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER].Rows[1]["Teacher_Id"]);
            oTT.DisplayTT();
            //call the method to display timetable preview
            FormatPreVw(iSrcTeacherId, iTargetTeacherId, oTT);
            //clear pnlContainer
            pnlContainer.Controls.Clear();
            //Add the TT panel into container panel.
            pnlContainer.Controls.Add(oTT.moPnl);
        }
    }
    /// <summary>
    ///  This method displays the class teacher assignment according to the changes made by user.
    /// </summary>
    private void ShowClassteacherAssignment()
    {
        DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
        const string S_FLD_CLASSNMAE = "StdDiv";
        lblSrc.Text = oDtClassTeacher.Rows[0][S_FLD_CLASSNMAE].ToString();
        lblTarget.Text = oDtClassTeacher.Rows[1][S_FLD_CLASSNMAE].ToString();
    }
    /// <summary>
    /// This method creates,  initialises  and returns a TransferBL object. 
    /// </summary>
    /// <returns></returns>
    private TeacherTransferBL GetObject()
    {
        DataTable oDtTeachers = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
        int iSrcTeacherId = Convert.ToInt32(oDtTeachers.Rows[0]["Teacher_Id"]);
        int iTargetTeacherId = Convert.ToInt32(oDtTeachers.Rows[1]["Teacher_Id"]);
        TeacherTransferBL obj = new TeacherTransferBL(iSrcTeacherId, iTargetTeacherId, moDSTeacherTransfer);
        return obj;
    }
    /// <summary>
    /// This method formats timetable display by applying Style to each of the timetable cells.
    /// The style for each cell is chosen as per its rowstate.
    /// </summary>
    /// <param name="aiSrcTeacherId"></param>
    /// <param name="aiTargetTeacherId"></param>
    private void FormatPreVw(int aiSrcTeacherId, int aiTargetTeacherId, Timetable oTT)
    {
        oTransferTT = oTT;
        HtmlTable oTbl = oTransferTT.tblTT;
        int iWeekDayCnt = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows.Count;
        const string S_CSS_ADDED = "ClsHilightBGB";
        const string S_CSS_TRANSFERED = "TTNotClassTchr";
        const string S_CSS_DELETED = "SubDeleted";
        int iTeacherId = 0;
        //loop through teachers 
        for (int iTeacherIndex = 2; iTeacherIndex < 4; iTeacherIndex++)
        {
            switch (iTeacherIndex)
            {
                case 2://row for src teacher
                    iTeacherId = aiSrcTeacherId;
                    break;
                case 3://row for target teacher
                    iTeacherId = aiTargetTeacherId;
                    break;
            }

            int iCellIndex = 0;
            //loop through weekdays
            for (int iWeekDay = 0; iWeekDay < iWeekDayCnt; iWeekDay++)
            {
                int iLectureCnt = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["LecturesCnt"]);
                string sWeekDayId = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["Weekdays_Id"].ToString();
                string sWeekDayName = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["Weekday_Name"].ToString();
                string sStayback = Resources.LocalizedResources.StayBack;
                string sAssembly = Resources.LocalizedResources.Assembly;
                string sMPT = Settings.MPTName;
                //loop through lectures on the current weekday.
                for (int i = 1; i <= iLectureCnt; i++)
                {
                    HtmlTableCell oCell = oTbl.Rows[iTeacherIndex].Cells[iCellIndex];

                    DataRow[] oDtRows = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select("Teacher_Id= " + iTeacherId.ToString() + " AND Weekday_Id=" + sWeekDayId + " AND Lecture_Number=" + i.ToString());
                    string sRowState = Constants.S_ORIGINAL;//Constants.S_ORIGINAL;
                    if (oDtRows.Length > 0)
                    {
                        sRowState = oDtRows[0][S_DB_COL_ROWSTATE].ToString();
                    }
                    //if src teacher
                    if (iTeacherId == aiSrcTeacherId)
                    {
                        string sSrcSubject = oTbl.Rows[iTeacherIndex].Cells[iCellIndex].InnerHtml;
                        string sTrgSubject = oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].InnerHtml;

                        if ((!sSrcSubject.Equals("<b>" + sStayback + "</b>") && !sSrcSubject.Equals("<b>" + sAssembly + "</b>") && !sSrcSubject.Equals("<b>" + sMPT + "</b>")) && 
                            (!sTrgSubject.Equals("<b>" + sStayback + "</b>") && !sTrgSubject.Equals("<b>" + sAssembly + "</b>") && !sTrgSubject.Equals("<b>" + sMPT + "</b>")))
                        {
                            switch (sRowState)
                            {
                                case Constants.S_UPDATED: //transferred
                                    oCell.Attributes.Remove("class");
                                    oCell.Attributes.Add("class", S_CSS_TRANSFERED);
                                    break;
                                case Constants.S_DELETED: //removed
                                    oCell.Attributes.Remove("class");
                                    oCell.Attributes.Add("class", S_CSS_DELETED);
                                    break;
                            }
                        }
                    }
                    else //target
                    {
                        string sTrgSubject = oTbl.Rows[iTeacherIndex].Cells[iCellIndex].InnerHtml;
                        if (!sTrgSubject.Equals("<b>" + sStayback + "</b>") && !sTrgSubject.Equals("<b>" + sAssembly + "</b>") && !sTrgSubject.Equals("<b>" + sMPT + "</b>"))
                        {
                            switch (sRowState)
                            {
                                case Constants.S_ADDED://transferred from src
                                    oCell.Attributes.Remove("class");
                                    oCell.Attributes.Add("class", S_CSS_ADDED);
                                    break;
                                case Constants.S_DELETED://removed
                                    oCell.Attributes.Remove("class");
                                    oCell.Attributes.Add("class", S_CSS_DELETED);
                                    break;
                            }
                        }
                    }
                    iCellIndex = iCellIndex + 1;
                }
            }
        }
    }

}
