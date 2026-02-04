using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;

/// <summary>
/// Summary description for PrePrimaryStudentProgress
/// </summary>
public class PrePrimaryStudentProgress : ProgressSheetBase
{
    #region class member

    protected Panel GridViewScrollContainer;
    protected int miStudentId = 0;
    protected Boolean bHeaderAdded = false;
    private Boolean mbEnabled = true;
    protected DataSet moProgressDetails;
    protected int I_DB_TABLE_INDEX_GRADE = 0;
    protected int I_DB_TABLE_INDEX_TEST = 1;
    protected int I_DB_TABLE_INDEX_HEADER = 2;
    protected int I_DB_TABLE_INDEX_PROGRESSENTRY = 3;
    private EventHandler cmbGrade_SelectedIndexChanged;
    protected int miSelectedAcademicYrId;

    #endregion class member   

    /// <summary>
    /// Used to get set Test Id
    /// </summary>    
    public Boolean ReadOnly
    {
        get
        {
            return !mbEnabled;
        }
        set
        {
            mbEnabled = !value;
        }
    }
    
    /// <summary>
    /// Event handler to attatch dropdownlist
    /// </summary>
    public EventHandler SelectedIndexChanged
    {
        set
        {
            cmbGrade_SelectedIndexChanged = value;
        }
    }

    #region cunstructor

    public PrePrimaryStudentProgress()
    {
        InitializeMemberVariables();
        SetSelectedAcademicYear();
    }

    public PrePrimaryStudentProgress(Panel oPanel)
    {
        InitializeMemberVariables();
        GridViewScrollContainer = oPanel;
        SetSelectedAcademicYear();
    }

    #endregion cunstructor  

    #region prgress sheet generation

    #region protected methods

    /// <summary>
    /// This method is used to show progress sheet.
    /// </summary>
    /// <param name="aiStudentId"></param>
    public override void ShowProgressSheet(int aiStudentId)
    {
        miStudentId = aiStudentId;
        GetProgressSheetDetailsResultSet();
        GenerateProgressSheet();
        SetSelectedAcademicYear();
    }

    /// <summary>
    /// This method is used to set selected academic year.
    /// </summary>
    public void SetSelectedAcademicYear()
    {
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] != null && Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString() != "0")
            miSelectedAcademicYrId = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
        else
            miSelectedAcademicYrId = miAcademicYearId;
    }

    /// <summary>
    /// This method is used to show progress sheet.
    /// </summary>
    /// <param name="miStudentId"></param>
    public override int ShowProgressSheet(int iTecherId, int iStudentId)
    {

        moProgressDetails = getAllStudentsProgressSheet(iTecherId);
        for (int iCnt = 1; iCnt < moProgressDetails.Tables.Count; iCnt = iCnt + 3)
        {
            GenerateProgressSheet();
            I_DB_TABLE_INDEX_TEST += 3;
            I_DB_TABLE_INDEX_HEADER += 3;
            I_DB_TABLE_INDEX_PROGRESSENTRY += 3;
            bHeaderAdded = false;
            if (moProgressDetails.Tables.Count > 3)
                CreatBlankTable();
        }

        SetSelectedAcademicYear();
        return moProgressDetails.Tables.Count;
    }

    /// <summary>
    /// This method is used to create blank table which can be placed between two progress sheets.
    /// </summary>
    protected void CreatBlankTable()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.Height = "30px";
        HeaderHtmlTable.Width = "100%";
        HeaderHtmlTable.Border = 0;
        //HeaderHtmlTable.Attributes.Add(HeaderHtmlTable.Align, "left");
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        //oHtmlTableCell.InnerHtml = "<hr class='Dottedhr'>";
        oHtmlTableCell.InnerHtml = "&nbsp;";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableCell.Attributes.Add("class", "Dottedhr");
        oHtmlTableCell.Attributes.Add("page-break-after", "always");
        oHtmlTableRow = new HtmlTableRow();
        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = "&nbsp;";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        LiteralControl oLiteralControl = new LiteralControl("<br />");
        GridViewScrollContainer.Controls.Add(oLiteralControl);
        GridViewScrollContainer.Controls.Add(HeaderHtmlTable);
    }

    /// <summary>
    /// This method is used to generate progress sheet
    /// </summary>
    protected void GenerateProgressSheet()
    {
        CreateStudentInfo();
        HtmlTable oMainHtmlTable = CreateHdTable(false);
        GridViewScrollContainer.Controls.Add(oMainHtmlTable);
        CreateAllControls(oMainHtmlTable);
        //ShowGradeInfo();
    }

    /// <summary>
    /// This method is used to Create all html controls reqd for progress sheet.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    protected void CreateAllControls(HtmlTable oMainHtmlTable)
    {
        GenerateHeaders(oMainHtmlTable);
        GenerateTestLevelComents(oMainHtmlTable);
    }

    /// <summary>
    /// This method is used to generate progress sheet record. 
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    /// <param name="aoArrDataRow"></param>
    /// <param name="sAlgnment"></param>
    protected void GenerateRecord(HtmlTable oMainHtmlTable, DataRow[] aoArrDataRow, HorizontalAlign sAlgnment)
    {
        //Swap cell aligment
        if (sAlgnment == HorizontalAlign.Left)
            sAlgnment = HorizontalAlign.Right;
        else
            sAlgnment = HorizontalAlign.Left;

        //Iterate through all record set.
        for (int iCnt = 0; iCnt < aoArrDataRow.Length; iCnt++)
        {
            DataRow oDataRow = aoArrDataRow[iCnt];

            //create html row for the record.
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            oMainHtmlTable.Rows.Add(oHtmlTableRow);

            //check is this entry have sub headers i.e child
            DataRow[] oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_PROGRESSENTRY].Select("Heading_Parent_Id=" + Convert.ToString(oDataRow["Heading_Id"]));
            if (oArrDataRow.Length > 0)
            {
                //add blank row before record insertion
                CreateHtmlCell(oHtmlTableRow, "&nbsp;", "BorderTB", 1, 2 + (GetTestEntryCount() * 2), sAlgnment);
                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);

                //If alignment is right the add description and image left side(before) the details list
                if (sAlgnment == HorizontalAlign.Right)
                {
                    CreateHtmlCell(oHtmlTableRow, "&nbsp;", " LblUsrNameHead", oArrDataRow.Length + 1, 1 + GetTestEntryCount(), sAlgnment);
                    GenerateRightSideDescriptionbox(oDataRow, oHtmlTableRow, iCnt);
                }
                //Show header record
                CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Heading_Text"]), "ClsTestHeader LblUsrNameHead", 1, 1, HorizontalAlign.Left);
                GenerateTestNamesHeading(oHtmlTableRow);
                //If alignment is right the add description and image right side(after) the details list
                if (sAlgnment == HorizontalAlign.Left)
                {
                    CreateHtmlCell(oHtmlTableRow, "&nbsp;", " LblUsrNameHead", oArrDataRow.Length + 1, 1 + GetTestEntryCount(), sAlgnment);
                    GenerateLeftSideDescriptionbox(oDataRow, oHtmlTableRow, iCnt);
                }
                //Swap cell aligment
                if (sAlgnment == HorizontalAlign.Left)
                    sAlgnment = HorizontalAlign.Right;
                else
                    sAlgnment = HorizontalAlign.Left;
                //recurrsion for the childs(sub header) of current record.
                GenerateRecord(oMainHtmlTable, oArrDataRow, sAlgnment);
            }
            else if (oDataRow["Heading_Text"] != DBNull.Value)
            {
                GenerateDisplayControls(oDataRow, oHtmlTableRow, sAlgnment);
            }
        }
    }

    /// <summary>
    /// This method is used to generate headers of progress sheet.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    protected virtual void GenerateHeaders(HtmlTable oMainHtmlTable)
    {
        //get the rowset of parent entries and generate the parent header records
        DataRow[] oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_PROGRESSENTRY].Select("Heading_Parent_Id=0");
        if (AllSettings[miSelectedAcademicYrId].IsPrePrimaryProgressSheetWithGrade)
        {
            //create html row for the record.
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            oMainHtmlTable.Rows.Add(oHtmlTableRow);
            CreateHtmlCell(oHtmlTableRow, "Set Default", "HilightBGGray LblUsrNameHead ", 1, 1, HorizontalAlign.Left);
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "HilightBGGray ConfigHeadBG", 1, 1, HorizontalAlign.Left);

            DropDownList oDropDownList = new DropDownList();
            oDropDownList.ID = "ddlDefault_Entry";
            ControlUtility.FillDropDownList(moProgressDetails.Tables[I_DB_TABLE_INDEX_GRADE], ref oDropDownList,
                                         "Grade_Name",
                                         "Grade_Name",
                                         Constants.S_SELECT);
            oDropDownList.Enabled = mbEnabled;
            oDropDownList.EnableViewState = true;
            oDropDownList.SelectedIndexChanged += new EventHandler(cmbGrade_SelectedIndexChanged);
            oDropDownList.AutoPostBack = true;
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oDropDownList);
        }

        GenerateRecord(oMainHtmlTable, oArrDataRow, HorizontalAlign.Right);
    }
    
    /// <summary>
    /// This methos is used to create not applicable ledgend.
    /// </summary>
    protected HtmlTable CreateHdTable(bool IsHeader)
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 0;
        HeaderHtmlTable.CellSpacing = 1;        
        if (!IsHeader)
        {
            HeaderHtmlTable.Attributes.Add("class", "ClsBorderNoBg BGReport");
            HeaderHtmlTable.ID = "tbl_" + moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["YearWise_Student_Id"].ToString();
            HeaderHtmlTable.Width = "100%";
        }
        else
        {
            HeaderHtmlTable.Attributes.Add("class", "ReportOuter");
            //HeaderHtmlTable.Width = "842px";
            HeaderHtmlTable.Width = "100%";
            HeaderHtmlTable.Border = 0;
        }
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        return HeaderHtmlTable;
    }

    /// <summary>
    /// This method is used to create cell
    /// </summary>
    /// <param name="sInnerText"></param>
    /// <param name="sClassName"></param>
    /// <param name="iRowSpan"></param>
    /// <param name="iColSpan"></param>
    protected void CreateHtmlCell(HtmlTableRow oHtmlTableRow, String sInnerText, String sClassName, int iRowSpan, int iColSpan, HorizontalAlign sAlignment)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Attributes.Add("style", "padding-" + sAlignment + ": 10px");
        oHtmlTableCell.Align = sAlignment.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to get resultset for the progress sheet
    /// </summary>
    protected void GetProgressSheetDetailsResultSet()
    {
        int iAcademicYrID;
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] != null && Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString() != "0")
            iAcademicYrID = Convert.ToInt32(Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID]);
        else
            iAcademicYrID = miAcademicYearId;
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        if (menmPagemode == Constants.PageMode.Normal)
            moProgressDetails = oPrePrimaryProgressSheetConfigBL.GetAllPrePrimaryProgressSheetDetails(miSchoolId, iAcademicYrID, 0, miTestId, miStudentId);
        else if (menmPagemode == Constants.PageMode.Edit)
            moProgressDetails = oPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressSheetDetails(miSchoolId, iAcademicYrID, miTestId, miStudentId);
        if (moProgressDetails.Tables.Count == 1 || moProgressDetails.Tables[1].Rows.Count <= 0)
            throw new BusinessLogic.Exceptions.MarksNotAvailableForResult(moProgressDetails.Tables[0].Rows[0][0].ToString());
    }
    
    /// <summary>
    /// This method is used to get all sutdents data.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    /// <returns></returns>
    protected DataSet getAllStudentsProgressSheet(int aiTeacherId)
    {
        DataSet oDataSet;
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oDataSet = oPrePrimaryProgressSheetConfigBL.GetAllPrePrimaryProgressSheetDetails(miSchoolId, miAcademicYearId, aiTeacherId, miTestId, 0);
        return oDataSet;
    }
        
    /// <summary>
    /// This function is used to get student dataset  for a given teacher ID
    /// </summary>
    /// <returns></returns>
    public DataTable getStudentDatset(int aiTeacherId)
    {
        int iAcademicYrID = miAcademicYearId;
        int iSchoolID = miSchoolId;
        StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL(iSchoolID, iAcademicYrID);
        DataTable oDSStudents = oStudentCollectionBL.GetStudentListOfGivenClassTeacher(aiTeacherId);
        return oDSStudents;
    }

    /// <summary>
    /// This methos is used to create not Student name.
    /// </summary>
    protected void CreateHdStudentName(HtmlTable HeaderHtmlTable)
    {
        DataTable oDTStudentInfo = moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER];
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        AddStudentInfo(oHtmlTableRow, "Roll No. ", oDTStudentInfo.Rows[0]["Roll_No"].ToString());
        AddStudentInfo(oHtmlTableRow, "Name ", oDTStudentInfo.Rows[0]["Student_Name"].ToString());
        AddStudentInfo(oHtmlTableRow, "Class ", oDTStudentInfo.Rows[0]["Standard_Name"].ToString() + " - " + oDTStudentInfo.Rows[0]["Division_Name"].ToString());
        AddStudentInfo(oHtmlTableRow, "Year ", oDTStudentInfo.Rows[0]["Academic_Year"].ToString());
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
        oDTStudentInfo.Dispose();
    }

    /// <summary>
    /// This methos is used to create not Schooll Name header.
    /// </summary>
    protected void CreateHdSchoolName(HtmlTable HeaderHtmlTable)
    {
        String sSchoolName = Convert.ToString(moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["School_Name"]);
        String sSchoolOrgnName = Convert.ToString(moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["School_Orgn_Name"]);
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolOrgnName, "SocietyName", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, sSchoolName, "ActualSchoolName", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to student info pair to html row.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="asLblText"></param>
    /// <param name="asLblVal"></param>
    protected void AddStudentInfo(HtmlTableRow oHtmlTableRow, String asLblText, String asLblVal)
    {
        Label oLabel = new Label();
        oLabel.Text = asLblText;
        oLabel.CssClass = "LblRht ClspaddingR";
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Controls.Add(oLabel);
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
        oHtmlTableCell.NoWrap = true;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        if (asLblVal != "")
        {
            oLabel = new Label();
            oLabel.Text = asLblVal;
            oLabel.CssClass = "ClsHilightTextB ClspaddingR";

            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.Controls.Add(oLabel);
            oHtmlTableCell.Align = "left";
            oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ClsBorderlight");
            oHtmlTableCell.NoWrap = true;
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
        }
    }

    #endregion protected methods

    #region protected virtual methods

    /// <summary>
    /// This method is used to generate test level comment.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    protected virtual void GenerateTestLevelComents(HtmlTable oMainHtmlTable)
    {
        //create html row for the record.
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "BorderTB", 1, 2 + (GetTestEntryCount() * 2), HorizontalAlign.Left);
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "Exam Comments", "ClsBorderlight LblUsrNameHead", 1, 1, HorizontalAlign.Left);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = VerticalAlign.Top.ToString();
        CreateHtmlCell(oHtmlTableRow, "", "", 1, 3, HorizontalAlign.Left);
        TextBox oTextBox = new TextBox();
        oTextBox.ID = "Test_" + moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["YearWise_Student_Id"].ToString();
        oTextBox.Attributes.Add("onkeyup", "javascript:ValidateMaxLength(this,300);");
        oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
        if (moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows.Count > 0 && moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows[0]["Description"] != DBNull.Value)
            oTextBox.Text = Convert.ToString(moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows[0]["Description"]);
        oTextBox.Rows = 10;
        oTextBox.CssClass = "LrgTxtBox";
        oTextBox.TextMode = TextBoxMode.MultiLine;
        oTextBox.Enabled = mbEnabled;
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oTextBox);
    }

    /// <summary>
    /// This method is used to generate the test names heading.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    protected virtual void GenerateTestNamesHeading(HtmlTableRow oHtmlTableRow)
    {
        CreateHtmlCell(oHtmlTableRow, Convert.ToString(moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows[0]["Test_Name"]), "ClsProgressGridHeader  LblUsrNameHead", 1, 1, HorizontalAlign.Left);
    }

    /// <summary>
    /// This method is Used to get included test count
    /// </summary>
    /// <returns></returns>
    protected virtual int GetTestEntryCount()
    {
        return 1;
    }

    /// <summary>
    /// This method is used to generate left aligned description and images.
    /// </summary>
    /// <param name="oDataRow"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="iCnt"></param>
    protected virtual void GenerateLeftSideDescriptionbox(DataRow oDataRow, HtmlTableRow aoHtmlTableRow, int iCnt)
    {
        HtmlTableCell oHtmlTableCell = aoHtmlTableRow.Cells[aoHtmlTableRow.Cells.Count - 1];
        HtmlTable oMainHtmlTable = new HtmlTable();
        oHtmlTableCell.Controls.Add(oMainHtmlTable);
        HtmlTableRow oHtmlTableRow;

        if (oDataRow["Heading_Text"] != DBNull.Value && (Convert.ToChar(oDataRow["Is_Description"]) == Constants.C_YES))
        {
            oHtmlTableRow = new HtmlTableRow();
            oMainHtmlTable.Rows.Add(oHtmlTableRow);

            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
            CreateCommentLbl(oHtmlTableRow);
        }
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);

        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
        Image oImage = new Image();
        oImage.ImageUrl = "~/RITeSchool/images/ProgressSheet" + iCnt + ".jpg";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oImage);

        CreateDescTextBox(oHtmlTableRow, oDataRow);
    }
        
    /// <summary>
    /// This method is used to generate right aligned description and images.
    /// </summary>
    /// <param name="oDataRow"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="iCnt"></param>
    protected virtual void GenerateRightSideDescriptionbox(DataRow oDataRow, HtmlTableRow aoHtmlTableRow, int iCnt)
    {
        HtmlTableCell oHtmlTableCell = aoHtmlTableRow.Cells[aoHtmlTableRow.Cells.Count - 1];
        HtmlTable oMainHtmlTable = new HtmlTable();
        oHtmlTableCell.Controls.Add(oMainHtmlTable);
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        if (oDataRow["Heading_Text"] != DBNull.Value && (Convert.ToChar(oDataRow["Is_Description"]) == Constants.C_YES))
        {
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
            CreateCommentLbl(oHtmlTableRow);
        }
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);

        CreateDescTextBox(oHtmlTableRow, oDataRow);
        
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
        Image oImage = new Image();
        oImage.ImageUrl = "~/RITeSchool/images/ProgressSheet" + iCnt + ".jpg";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oImage);
    }

    /// <summary>
    /// This method is used to create comment lebel.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    private void CreateCommentLbl(HtmlTableRow oHtmlTableRow)
    {
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        Label oLabel = new Label();
        oLabel.CssClass = "LblUsrNameHead";
        oLabel.Text = "Comments";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oLabel);
    }

    /// <summary>
    /// This method is used to create description textbox
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    private void CreateDescTextBox(HtmlTableRow oHtmlTableRow, DataRow oDataRow)
    {
        if (oDataRow["Heading_Text"] != DBNull.Value && (Convert.ToChar(oDataRow["Is_Description"]) == Constants.C_YES))
        {
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1, HorizontalAlign.Left);
            TextBox oTextBox = new TextBox();
            oTextBox.ID = "Txt_Desc_" + Convert.ToString(oDataRow["Heading_Id"]) + "_" + moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["YearWise_Student_Id"].ToString();
            oTextBox.Attributes.Add("onkeyup", "javascript:ValidateMaxLength(this,100);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            if (oDataRow["Description"] != DBNull.Value)
                oTextBox.Text = oDataRow["Description"].ToString();
            oTextBox.Rows = 5;
            oTextBox.CssClass = "LrgTxtBox";
            oTextBox.TextMode = TextBoxMode.MultiLine;
            oTextBox.Enabled = mbEnabled;
            oTextBox.ReadOnly = !mbEnabled;
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oTextBox);
        }
    }

    /// <summary>
    /// This method is used to generate desplay controls.
    /// </summary>
    /// <param name="oDataRow"></param>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="sAlgnment"></param>
    protected virtual void GenerateDisplayControls(DataRow oDataRow, HtmlTableRow oHtmlTableRow, HorizontalAlign sAlgnment)
    {
        //Check is this main header or sub header
        //There can be main headers witch are leaf.
        if ((oDataRow["Heading_Parent_Id"] != DBNull.Value) && (Convert.ToInt32(oDataRow["Heading_Parent_Id"]) != 0))
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Heading_Text"]), "ClsBorderlight ClsLbl", 1, 1, HorizontalAlign.Left);
        else
        {
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "BorderTB", 1, 2 + (GetTestEntryCount() * 2), HorizontalAlign.Left);
            HtmlTable oMainHtmlTable = (HtmlTable)oHtmlTableRow.Parent;
            oHtmlTableRow = new HtmlTableRow();
            oMainHtmlTable.Rows.Add(oHtmlTableRow);
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Heading_Text"]), "ClsBorderlight LblUsrNameHead", 1, 1, HorizontalAlign.Left);
        }

        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "ClsBorderlight", 1, 1, HorizontalAlign.Left);
        if (!AllSettings[miSelectedAcademicYrId].IsPrePrimaryProgressSheetWithGrade)
        {

            TextBox oTextBox = new TextBox();
            oTextBox.ID = "Txt_Entry_" + Convert.ToString(oDataRow["Heading_Id"]);
            if (oDataRow["Value"] != DBNull.Value)
                oTextBox.Text = oDataRow["Value"].ToString();
            oTextBox.CssClass = "LrgTxtBox";
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oTextBox);
            oTextBox.Enabled = mbEnabled;
            oTextBox.MaxLength = 50;
        }
        else
        {

            DropDownList oDropDownList = new DropDownList();
            oDropDownList.ID = "ddl_Entry_" + Convert.ToString(oDataRow["Heading_Id"]);
            ControlUtility.FillDropDownList(moProgressDetails.Tables[I_DB_TABLE_INDEX_GRADE], ref oDropDownList,
                                         "Grade_Name",
                                         "Grade_Name",
                                         Constants.S_SELECT);
            if (oDataRow["Value"] != DBNull.Value)
                oDropDownList.SelectedValue = oDataRow["Value"].ToString();
            oDropDownList.Enabled = mbEnabled;
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oDropDownList);

        }
        if ((oDataRow["Heading_Parent_Id"] != DBNull.Value) && (Convert.ToInt32(oDataRow["Heading_Parent_Id"]) == 0))
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1 + GetTestEntryCount(), HorizontalAlign.Left);
    }

    /// <summary>
    /// This method is used to create student's Header information.
    /// </summary>    
    protected virtual void CreateStudentInfo()
    {
        HtmlTable HeaderHtmlTable = CreateHdTable(true);
        CreateHdSchoolName(HeaderHtmlTable);
        CreateHdProgressCard(HeaderHtmlTable);
        CreateHdStudentName(HeaderHtmlTable);
        GridViewScrollContainer.Controls.Add(HeaderHtmlTable);
        HeaderHtmlTable.Dispose();
    }

    #endregion protected virtual methods

    #region private methods

    /// <summary>
    /// This method is used to create progress report header
    /// </summary>
    /// <param name="HeaderHtmlTable"></param>
    private void CreateHdProgressCard(HtmlTable HeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Progress Report", "ClsReportHead", 1, 8, HorizontalAlign.Center);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to generate grade legend.
    /// </summary>
    private void ShowGradeInfo()
    {
        DataTable oDataTable = moProgressDetails.Tables[I_DB_TABLE_INDEX_GRADE];
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 3;
        HeaderHtmlTable.CellSpacing = 1;
        HeaderHtmlTable.Border = 0;
        HeaderHtmlTable.Align = "left";
        HeaderHtmlTable.BgColor = "Black";
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        GridViewScrollContainer.Controls.Add(HeaderHtmlTable);

        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Grade", "Lbl10ptB ConfigHeadBG", 1, 1, HorizontalAlign.Left);
        if (oDataTable.Rows.Count > 1)
        {
            int iColCount = 1;
            int iRowCount = 0;
            for (; iRowCount <= oDataTable.Rows.Count - 1; iRowCount++)
            {
                DataRow oDataRow = oDataTable.Rows[iRowCount];
                CreateHtmlCell(oHtmlTableRow, oDataRow[iColCount].ToString(), "LblSmlV ClsBGWhite", 1, 1, HorizontalAlign.Left);

                if (iRowCount == oDataTable.Rows.Count - 1)
                {
                    HeaderHtmlTable.Rows.Add(oHtmlTableRow);
                    iColCount++;
                    oHtmlTableRow = new HtmlTableRow();
                    CreateHtmlCell(oHtmlTableRow, oDataTable.Columns[iColCount].ToString(), "LblSmlVB ConfigHeadBG", 1, 1, HorizontalAlign.Left);

                    iRowCount = -1;
                }
                if (iColCount == 3)
                    break;
            }
        }
    }

    #endregion private methods

    #endregion progress sheet generation
}

public class PrePrimaryStudentProgressDisplay : PrePrimaryStudentProgress
{
    /// <summary>
    /// Counstructor - SET default panel where progress sheet to be rendered
    /// </summary>
    /// <param name="oPanel"></param>
    public PrePrimaryStudentProgressDisplay(Panel oPanel)
    {
        GridViewScrollContainer = oPanel;
        ReadOnly = true;
    }

    /// <summary>
    /// This method is used to generate display controls
    /// </summary>
    /// <param name="oDataRow"></param>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="sAlgnment"></param>
    protected override void GenerateDisplayControls(DataRow oDataRow, HtmlTableRow oHtmlTableRow, HorizontalAlign sAlgnment)
    {
        DataRow[] oArrDataRow;
        //Check is this main header or sub header
        //There can be main headers witch are leaf.
        if ((oDataRow["Heading_Parent_Id"] != DBNull.Value) && (Convert.ToInt32(oDataRow["Heading_Parent_Id"]) != 0))
        {
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Heading_Text"]), "ClsBorderlight ClsLbl", 1, 1, HorizontalAlign.Left);
        }
        else
        {
            //Create a blank row with the calculated colspan.
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "BorderTB", 1, 2 + (GetTestEntryCount() * 2), sAlgnment);
            HtmlTable oMainHtmlTable = (HtmlTable)oHtmlTableRow.Parent;
            //Check if test header is added or not and add if not added 
            if (!bHeaderAdded)
            {
                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);
                CreateHtmlCell(oHtmlTableRow, "&nbsp;", "ClsProgressGridTestHeader", 1, 1, HorizontalAlign.Left);
                oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Attributes.Add("style", "padding-left: 20px");
                oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Select("Is_Published <> 'X'");
                //Add test name cells
                for (int icnt = 0; icnt < oArrDataRow.Length; icnt++)
                {
                    DataRow oTestDataRow = oArrDataRow[icnt];
                    CreateHtmlCell(oHtmlTableRow, Convert.ToString(oTestDataRow["Test_Name"]), "ClsProgressGridTestHeader", 1, 1, HorizontalAlign.Left);
                    oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Attributes.Add("style", "padding-left: 20px");
                }
                CreateHtmlCell(oHtmlTableRow, "&nbsp;", "ClsProgressGridTestHeader", 1, 2, HorizontalAlign.Left);
                oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Attributes.Add("style", "padding-left: 20px");
                bHeaderAdded = true;
                //Add blank row
                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);
                CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1 + GetTestEntryCount(), HorizontalAlign.Left);
            }
            //Add the header text cell
            oHtmlTableRow = new HtmlTableRow();
            oMainHtmlTable.Rows.Add(oHtmlTableRow);
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Heading_Text"]), "ClsBorderlight LblUsrNameHead WeekDCell", 1, 1, HorizontalAlign.Left);

        }
        //Now Show the progress report header with their values.
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
        
        oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Select("Is_Published <> 'X'");
        //Show all values for all tests.
        for (int icnt = 0; icnt < oArrDataRow.Length; icnt++)
        {
            DataRow oTestDataRow = oArrDataRow[icnt];
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "ClsBorderlight", 1, 1, HorizontalAlign.Left);
            Label oLabel = new Label();
            if (oDataRow[Convert.ToString(oTestDataRow["Test_Id"])] != DBNull.Value)
            {
                //Get a value and remove a grade name from the test while displaying
                string sValue = Convert.ToString(oDataRow[Convert.ToString(oTestDataRow["Test_Id"])]);
                if (AllSettings[miSelectedAcademicYrId].IsPrePrimaryProgressSheetWithGrade)
                {
                    if (sValue.Length > 3)
                        sValue = sValue.Substring(sValue.IndexOf(" - ") + 3);
                }
                oLabel.Text = sValue;
            }
            else //No value show dash
                oLabel.Text = "-";
            oLabel.CssClass = "LblUsrNameHead";
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oLabel);
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].BgColor = System.Drawing.Color.WhiteSmoke.ToString();
        }
        //Show the blank row having calculated colspan between two headers.
        if ((oDataRow["Heading_Parent_Id"] != DBNull.Value) && (Convert.ToInt32(oDataRow["Heading_Parent_Id"]) == 0))
            CreateHtmlCell(oHtmlTableRow, "&nbsp;", "", 1, 1 + GetTestEntryCount(), HorizontalAlign.Left);
    }

    /// <summary>
    /// This method is used to generate headers of progress sheet.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    protected override void GenerateHeaders(HtmlTable oMainHtmlTable)
    {
        //get the rowset of parent entries and generate the parent header records
        DataRow[] oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_PROGRESSENTRY].Select("Heading_Parent_Id=0");
        GenerateRecord(oMainHtmlTable, oArrDataRow, HorizontalAlign.Right);
    }

    /// <summary>
    /// Method used to get test entry count.
    /// </summary>
    /// <returns></returns>
    protected override int GetTestEntryCount()
    {
        DataRow[] oDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Select("Is_Published <> 'X'");
        return oDataRow.Length;
    }
   
    /// <summary>
    /// This method is used to generate test level comment.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    protected override void GenerateTestLevelComents(HtmlTable aoMainHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        aoMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "BorderTB", 1, 2 + (GetTestEntryCount() * 2), HorizontalAlign.Left);

        HtmlTable oMainHtmlTable = new HtmlTable();
        oMainHtmlTable.EnableViewState = false;
        oMainHtmlTable.CellPadding = 3;
        oMainHtmlTable.CellSpacing = 1;
        oMainHtmlTable.Border = 0;
        oMainHtmlTable.Align = "left";
        oMainHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        oMainHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());

        oHtmlTableRow = new HtmlTableRow();
        aoMainHtmlTable.Rows.Add(oHtmlTableRow);

        CreateHtmlCell(oHtmlTableRow, "", "", 1, 2 + (GetTestEntryCount() * 2), HorizontalAlign.Left);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oMainHtmlTable);

        //create html row for the record.
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "Exam Comments", "LblUsrNameHead", 1, 2 + (GetTestEntryCount() * 2), HorizontalAlign.Left);

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);

        HtmlTableRow oCommentHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oCommentHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", "LblUsrNameHead", 1, 1, HorizontalAlign.Left);
        CreateHtmlCell(oCommentHtmlTableRow, "&nbsp;", "LblUsrNameHead", 1, 1, HorizontalAlign.Left);

        DataRow[] oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Select("Is_Published <> 'X'");
        for (int icnt = 0; icnt < oArrDataRow.Length; icnt++)
        {
            DataRow oTestDataRow = oArrDataRow[icnt];
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oTestDataRow["Test_Name"]), "LblUsrNameHead", 1, 1, HorizontalAlign.Left);
            CreateHtmlCell(oCommentHtmlTableRow, "&nbsp;", "LblUsrNameHead", 1, 1, HorizontalAlign.Left);
            TextBox oTextBox = new TextBox();
            oTextBox.ID = "Test_" + Convert.ToString(oTestDataRow["Test_Id"]) + moProgressDetails.Tables[I_DB_TABLE_INDEX_HEADER].Rows[0]["YearWise_Student_Id"].ToString();
            oTextBox.Attributes.Add("onkeyup", "javascript:ValidateMaxLength(this,300);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            if (moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows.Count > 0 && moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Rows[0]["Description"] != DBNull.Value)
                oTextBox.Text = Convert.ToString(oTestDataRow["Description"]);
            oTextBox.ReadOnly = true;
            oTextBox.Rows = 10;
            oTextBox.TextMode = TextBoxMode.MultiLine;
            oTextBox.CssClass = "LrgTxtBox";
            oCommentHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Controls.Add(oTextBox);
        }
    }

    /// <summary>
    /// This method is used to generate the test names heading.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    protected override void GenerateTestNamesHeading(HtmlTableRow oHtmlTableRow)
    {
        DataRow[] oArrDataRow = moProgressDetails.Tables[I_DB_TABLE_INDEX_TEST].Select("Is_Published <> 'X'");
        for (int icnt = 0; icnt < oArrDataRow.Length; icnt++)
        {
            DataRow oTestDataRow = oArrDataRow[icnt];
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oTestDataRow["Test_Name"]), "ClsProgressGridHeader  LblUsrNameHead", 1, 1, HorizontalAlign.Left);
        }
    }
}