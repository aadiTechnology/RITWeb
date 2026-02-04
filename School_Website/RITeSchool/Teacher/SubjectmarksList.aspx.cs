using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class SubjectmarksList : SchoolBase
{
	#region Constant

    private const string S_FIRST_TOPPERS_GIF = "../images/Number1.gif";
    private const string S_SECOND_TOPPERS_GIF = "../images/Number2.gif";
    private const string S_THIRD_TOPPERS_GIF = "../images/Number3.gif";

    private const int I_ROLL_NUMBER_COLUMN_INDEX = 0;
    private const int I_ASSIGNED_GRADE_COLUMN_INDEX = 2;
    private const string S_DATAKEY_STUDENT_ID = "student_id";

    #endregion

	#region Members

    private int miSubjectID;
    private bool? mbTestTypeAdded = false;
    private int miStandardDivisionId;
    private int miTestID;
    private int miNoOfRecords = 15;
    private bool mbLegendAdded = false;
    private DataSet modsToppers;
    private DataTable modtExamStatusColors;

	#endregion

    #region Events

    /// <summary>
    /// This method is used to fill student attendance and set todays date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetQueryStringValues();
                DisplayData();
                FillStudentsGrid();
                ApplyMouseHoverEffect(new List<Button> { btnBack });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to lis page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            string sEncrypt = GetEncryptedTestQueryString();
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Teacher/ClassTeacherTestMarksUI.aspx?" + sEncrypt);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// Set hidden fields and class members with querystring values.
    /// </summary>
    private void SetQueryStringValues()
    {
        miSubjectID = QueryString["SubjectId"].ToInt();
        miStandardDivisionId = QueryString["StandardDivisionId"].ToInt();
        miTestID = QueryString["TestID"].ToInt();

        hidStandardDivisionId.Value = QueryString["StandardDivisionId"] ?? Constants.S_ZERO;
        hidSubjectId.Value = QueryString["SubjectId"] ?? Constants.S_ZERO;
        hidTestId.Value = QueryString["TestID"] ?? Constants.S_ZERO;
        hidTeacherId.Value = QueryString["TeacherId"] ?? Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to get encrypted 
    /// </summary>
    /// <returns></returns>
    private string GetEncryptedTestQueryString()
    {
		string sQuerystring = string.Format("TestId={0}&StandardDivisionId={1}", hidTestId.Value, hidStandardDivisionId.Value);
        string sEncryptedString = CommonUtility.EncryptQuerystring(sQuerystring);
        return sEncryptedString;
    }

    /// <summary>
    /// This method is used to display grades. 
    /// </summary>
    /// <param name="oGridView"></param>
    private void DisplayGrades(GridView oGridView)
    {
        oGridView.Columns[I_ASSIGNED_GRADE_COLUMN_INDEX].Visible = true;
        FillGradesCombobox(oGridView);
    }

    /// <summary>
    /// Fills all the students for current academic year for specified standard division.
    /// </summary>
    private void FillStudentsGrid()
    {
        int iTestId = Convert.ToInt32(hidTestId.Value);        
        DataSet oDSStudents = StudentBL.GetStudentsForSubjectMarkSheet(miSchoolId, miStandardDivisionId, miAcademicYearId, miNoOfRecords, iTestId, miSubjectID);
        modsToppers = StudentBL.GetFirstThreeToopers(miSchoolId, miStandardDivisionId, miAcademicYearId, miTestID, miSubjectID);
        int i = 1;
        foreach (DataTable oTable in oDSStudents.Tables)
        {
            ///Create saparated grid for each bulk of data partition.
            CreateNewGrid(oTable, i);
            i++;
        }
    }

    /// <summary>
    /// This method is used to create new grid schema.
    /// </summary>
    private void CreateNewGrid(DataTable oDataTable, int aiGridCount)
    {
        // We have one static gridview where we have defined all design and properties.
        // And here we need clone this grid with new one
        GridView oGridView = new GridView();
        oGridView.Visible = true;
        oGridView.EnableViewState = false;
        oGridView.CssClass = "GridBorder";
        oGridView.Width = grdStudentMarks.Width;
        oGridView.AutoGenerateColumns = grdStudentMarks.AutoGenerateColumns;
        oGridView.PageSize = grdStudentMarks.PageSize;
        oGridView.AllowPaging = grdStudentMarks.AllowPaging;
        oGridView.CellPadding = grdStudentMarks.CellPadding;
        oGridView.CellSpacing = grdStudentMarks.CellSpacing;
        oGridView.ForeColor = grdStudentMarks.ForeColor;
        oGridView.GridLines = grdStudentMarks.GridLines;
        oGridView.DataKeyNames = grdStudentMarks.DataKeyNames;
        oGridView.RowStyle.CssClass = grdStudentMarks.RowStyle.CssClass;
        oGridView.HeaderStyle.CssClass = grdStudentMarks.HeaderStyle.CssClass;
        oGridView.AlternatingRowStyle.CssClass = grdStudentMarks.AlternatingRowStyle.CssClass;
        oGridView.EmptyDataRowStyle.CssClass = grdStudentMarks.EmptyDataRowStyle.CssClass;
        foreach (DataControlField oDataControlField in grdStudentMarks.Columns.CloneFields())
            oGridView.Columns.Add(oDataControlField);
        DataSet oDS = AddTestTypeColumnsAndLegends(oGridView);
        oGridView.DataSource = oDataTable;
        oGridView.DataBind();

        // Ad this grid view into new table cell .
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = trGrid.Cells[0].Align;
        oHtmlTableCell.VAlign = trGrid.Cells[0].VAlign;
        oHtmlTableCell.Controls.Add(oGridView);
        trGrid.Cells.Add(oHtmlTableCell);

        // Display already assigned test marks for each student.
        DisplayStudentMarks(oGridView, oDS);
    }

    /// <summary>
    /// This method is used to add test type columns and legend of those columns
    /// </summary>
    /// <param name="oGridView"></param>
    /// <returns></returns>
    private DataSet AddTestTypeColumnsAndLegends(GridView oGridView)
    {
		string ShowTotalAsPerOutOfMarks = Settings.ShowTotalAsPerOutOfMarks ? Constants.S_YES : Constants.S_NO;
        DataSet oDSTestTypes = SubjectTestTypeConfigurationCollectionBL.GetAllTestTypesForStandardDivisionSubjectTest(miStandardDivisionId, miSubjectID, miTestID, miSchoolId, miAcademicYearId,ShowTotalAsPerOutOfMarks);
		modtExamStatusColors = oDSTestTypes.Tables[2];
        
		if (oDSTestTypes.Tables[3].Rows[0][0].ToString() == Constants.C_YES.ToString())
            tdFailStudentLegend.Visible = false;

		HtmlTableCell oHtmlTableCell;
		int iColoumnCnt = 2;

		// Add Legends dynamically
		if (!mbLegendAdded)
		{
			foreach (DataRow row in oDSTestTypes.Tables[2].Rows)
			{
				oHtmlTableCell = new HtmlTableCell();
				oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BackgroundColor, row["BackColor"].ToString());
				Label oLabel = new Label
				{
				    CssClass = "ClsLblLgnd",
				    Text = Convert.ToString(row["DisplayValue"]) + " : " + Convert.ToString(row["DisplayName"])
				};
			    oLabel.Style.Add(HtmlTextWriterStyle.Color, row["ForeColor"].ToString());
				oLabel.Style.Add(HtmlTextWriterStyle.Padding, "2px");
				oHtmlTableCell.Controls.Add(oLabel);
				oHtmlTableCell.Attributes.Add("class", "ClsBorderlight ");
				trLedgend.Cells.Insert(iColoumnCnt, oHtmlTableCell);
				iColoumnCnt++;
			}

			mbLegendAdded = true;
		}
		
		// if there are no test types then the grade is applicable for the current subject.
        if (oDSTestTypes.Tables[0].Rows.Count > 0 && oDSTestTypes.Tables[0].Rows[0]["Grade_Or_Marks"].ToString() != "G")
        {
            TemplateField customField;
			iColoumnCnt = 2;

            // Display only those columns which are applicable for the current test.
            foreach (DataRow oDataRow in oDSTestTypes.Tables[0].Rows)
            {
                string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]).Substring(0, 1);
                customField = new TemplateField();
                customField.ItemTemplate = new GridViewLabelTemplate(DataControlRowType.DataRow, sHeaderText, Convert.ToString(oDataRow["TestType_Name"]).Replace(" ", string.Empty) + "lblMarks");
                customField.HeaderTemplate = new GridViewLabelTemplate(DataControlRowType.Header, sHeaderText, Convert.ToString(oDataRow["TestType_Name"]).Replace(" ", string.Empty) + "lblMarks");
                customField.ControlStyle.CssClass = "ClsLbl";
                customField.ControlStyle.Width = Unit.Pixel(30);
                customField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                oGridView.Columns.Add(customField);

                // Add test type legend if not earlier.
                if (mbTestTypeAdded == false)
                {
					oHtmlTableCell = new HtmlTableCell();
                    Label oLabel = new Label();
                    oLabel.CssClass = "ClsLblLgnd";
					oLabel.Text = Convert.ToString(oDataRow["TestType_Name"]).Substring(0, 1) + " : " + Convert.ToString(oDataRow["TestType_Name"]);
					oLabel.Style.Add(HtmlTextWriterStyle.Padding, "2px");
                    oHtmlTableCell.Controls.Add(oLabel);
                    oHtmlTableCell.Attributes.Add("class", "ClsBorderlight ");
                    trLedgend.Cells.Insert(iColoumnCnt, oHtmlTableCell);
                    iColoumnCnt++;
                }
            }

            ///Add space after legends added
            if (mbTestTypeAdded == true)
            {
                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Style.Add(HtmlTextWriterStyle.Width, "5px");
                trLedgend.Cells.Insert(iColoumnCnt, oHtmlTableCell);
                iColoumnCnt++;
            }

            ///Add total column of test types
            customField = new TemplateField();
            customField.ItemTemplate = new GridViewLabelTemplate(DataControlRowType.DataRow, "Total", "lblTotalMarks");
            customField.HeaderTemplate = new GridViewLabelTemplate(DataControlRowType.Header, "Total", "lblTotalMarks");
            customField.ControlStyle.CssClass = "ClsLbl";
            customField.ControlStyle.Width = Unit.Pixel(30);
            customField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            oGridView.Columns.Add(customField);

            // Mark it as null so that next time this will not come into any check.
            mbTestTypeAdded = null;
        }

        return oDSTestTypes;
    }

    /// <summary>
    /// Fills combobox for each row if the grades are to be assigned to students.
    /// </summary>
    private void FillGradesCombobox(GridView oGridView)
    {
        DropDownList oddlHeaderGrades = (DropDownList)oGridView.HeaderRow.Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlHeaderGrade");
        oddlHeaderGrades.Attributes.Add("onchange", "SetSelectedGradeForAllRows();");
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillGradeListComboxForSelectedClass(miSchoolId, miAcademicYearId, miStandardDivisionId, ref oddlHeaderGrades);
    }

    /// <summary>
    /// Display data in respective controls.
    /// </summary>
    private void DisplayData()
    {
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        DataSet oDSInfo = oStudentSubjectMarksBL.GetAllRelatedInformation(miSchoolId, miAcademicYearId, miSubjectID, miTestID, miStandardDivisionId);

        // Dataset contains 3 tables - 1. standard-division name, 2. subject name, 3. test relation information.
        hidSchoolSubjectTestId.Value = oDSInfo.Tables[2].Rows[0]["TestWise_Subject_Marks_Id"].ToString();

        if (oDSInfo.Tables[2].Rows[0]["Grade_Or_Marks"].ToString() == "M")
            hidMarksOrGrades.Value = "M";
        else
            hidMarksOrGrades.Value = "G";

        // Standard division.
        lblDataStdDiv.Text = oDSInfo.Tables[0].Rows[0]["Standard_Name"].ToString() + " - " + oDSInfo.Tables[0].Rows[0]["Division_Name"].ToString();

        // Subject name
        lblDataSubjectName.Text = oDSInfo.Tables[1].Rows[0]["Subject_Name"].ToString();
        lblDataExam.Text = oDSInfo.Tables[2].Rows[0]["SchoolWise_Test_Name"].ToString();
    }

    /// <summary>
    /// Display already assigned marks for students.
    /// </summary>
    /// <param name="aoDTMarks"></param>
    private void DisplayStudentMarks(GridView oGridView, DataSet aoDataSet)
    {
        if (aoDataSet.Tables[0].Rows.Count == 0 || (aoDataSet.Tables[0].Rows.Count > 0 && aoDataSet.Tables[0].Rows[0]["Grade_Or_Marks"].ToString() == "G"))
            DisplayGrades(oGridView);
        
        for (int iRowIndex = 0; iRowIndex < oGridView.Rows.Count; iRowIndex++)
        {
            int iStudentId = Convert.ToInt32(oGridView.DataKeys[iRowIndex][S_DATAKEY_STUDENT_ID].ToString());
            DataRow[] oArrRows = aoDataSet.Tables[1].Select("Student_Id =" + iStudentId.ToString());
            decimal dTotalMarksScored = 0;
            // If datatable contains rows for current roll number then display marks in resp. cells.
            foreach (DataRow oDRStudentMarks in oArrRows)
            {
                if ((oDRStudentMarks["Assigned_Grade_Id"].ToString() == "0" || oDRStudentMarks["Assigned_Grade_Id"].ToString() == string.Empty) && oDRStudentMarks["Grade_Or_Marks"].ToString() != "G")
                {
                    DataRow[] oArrPassingRows = aoDataSet.Tables[0].Select("TestType_id =" + oDRStudentMarks["TestType_Id"].ToString());
                    string sHeaderText = Convert.ToString(oDRStudentMarks["TestType_Name"]);
                    SetMarksToLabel(oGridView, oDRStudentMarks["Marks_Scored"].ToString(), iRowIndex, sHeaderText.Replace(" ", string.Empty) + "lblMarks", oDRStudentMarks["Is_Absent"].ToString(), Convert.ToInt32(oArrPassingRows[0]["TestType_Passing_Marks"]));

                    Label oLabel = (Label)oGridView.Rows[iRowIndex].FindControl("lblTotalMarks");
                    if (Settings.ShowTotalAsPerOutOfMarks)
                        dTotalMarksScored = oDRStudentMarks["Total_Marks_Scored"].ToDecimal();
                    else
                        dTotalMarksScored = dTotalMarksScored + oDRStudentMarks["Marks_Scored"].ToDecimal();
                    oLabel.Text = dTotalMarksScored.ToString("0.#");
                }
                else
                {
                    Label oddlGrades = (Label)oGridView.Rows[iRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlGrade");
                    if (oDRStudentMarks["Is_Absent"].ToString() != Constants.C_NO.ToString())
                    {
                        oddlGrades.Font.Bold = true;
						string sForeColor;
						string sBackColor;
                        DataRow[] odr = aoDataSet.Tables[2].Select(string.Format("ShortName='{0}'", oDRStudentMarks["Is_Absent"].ToString()));
                        oddlGrades.Text = odr[0]["DisplayValue"].ToString();
						sForeColor = odr[0]["ForeColor"].ToString();
						sBackColor = odr[0]["BackColor"].ToString();
						oddlGrades.Style.Add(HtmlTextWriterStyle.Color, sForeColor);
						oGridView.Rows[iRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].Style.Add(HtmlTextWriterStyle.BackgroundColor, sBackColor);
                    }
                    else
                    {
                        DropDownList oddlHeaderGrades = (DropDownList)oGridView.HeaderRow.Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlHeaderGrade");
                        ListItem oListItem = oddlHeaderGrades.Items.FindByValue(oDRStudentMarks["Assigned_Grade_Id"].ToString());
                        if (oListItem != null)
                            oddlGrades.Text = oListItem.Text;
                        if (Convert.ToInt32(oDRStudentMarks["Assigned_Grade_Id"]) > Convert.ToInt32(oDRStudentMarks["Passing_Grade_Id"]))
                            oddlGrades.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

            if ((oArrRows.Length > 0) && (oArrRows[0]["Assigned_Grade_Id"] != DBNull.Value) && (oArrRows[0]["Assigned_Grade_Id"].ToString() == "0" || oArrRows[0]["Assigned_Grade_Id"].ToString() == string.Empty))
                CheckAndSetTopperOfSubject(oGridView, iRowIndex);
        }
    }

    /// <summary>
    /// This function is used to set first 3 toppers indication to the row.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    private void CheckAndSetTopperOfSubject(GridView oGridView, int aiRowIndex)
    {
        int iStudentId = Convert.ToInt32(oGridView.DataKeys[aiRowIndex][S_DATAKEY_STUDENT_ID].ToString());
        string sFilter = "Student_id = " + iStudentId;
        DataRow[] oArrRows = modsToppers.Tables[0].Select(sFilter);

        Label oLabel = new Label();
        oLabel.Text = oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Text;
        oLabel.BorderStyle = BorderStyle.None;
        oLabel.ToolTip = oGridView.DataKeys[aiRowIndex]["Name"].ToString();
        oLabel.Style.Add("cursor", "pointer");
        oLabel.CssClass = "class1";
       
        oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oLabel);

        if ((oArrRows != null) && (oArrRows.Length > 0) && (Convert.ToInt32(oArrRows[0][0]) == iStudentId))
        {
            Image oImage = new Image();
            oImage.ImageUrl = S_FIRST_TOPPERS_GIF;           
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oImage);
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oLabel);
        }

        oArrRows = modsToppers.Tables[1].Select(sFilter);
        if ((oArrRows != null) && (oArrRows.Length > 0) && (Convert.ToInt32(oArrRows[0][0]) == iStudentId))
        {
            Image oImage = new Image();
            oImage.ImageUrl = S_SECOND_TOPPERS_GIF;           
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oImage);
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oLabel);
        }

        oArrRows = modsToppers.Tables[2].Select(sFilter);
        if ((oArrRows != null) && (oArrRows.Length > 0) && (Convert.ToInt32(oArrRows[0][0]) == iStudentId))
        {
            Image oImage = new Image();
            oImage.ImageUrl = S_THIRD_TOPPERS_GIF;            
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oImage);
            oGridView.Rows[aiRowIndex].Cells[I_ROLL_NUMBER_COLUMN_INDEX].Controls.Add(oLabel);
        }
    }

    /// <summary>
    /// This function is used to set value to text box.
    /// </summary>
    /// <param name="oGridView"></param>
    /// <param name="asValue"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="asControlName"></param>
    /// <param name="asIsAbsent"></param>
    private void SetMarksToLabel(GridView oGridView, string asValue, int aiRowIndex, string asControlName, string asIsAbsent, int aiPassingMarks)
    {
        Label aoLabel = (Label)oGridView.Rows[aiRowIndex].FindControl(asControlName);
        aoLabel.Text = asValue.ToDecimal().ToString("0.#");
        
        if (asIsAbsent != Constants.C_NO.ToString())
        {
            DataRow[] odr = modtExamStatusColors.Select(string.Format("ShortName='{0}'", asIsAbsent));
            aoLabel.Text = odr[0]["DisplayValue"].ToString();
            aoLabel.Font.Bold = true;
			
			aoLabel.Style.Add(HtmlTextWriterStyle.BackgroundColor, odr[0]["BackColor"].ToString());
            aoLabel.ForeColor = System.Drawing.Color.FromName(odr[0]["ForeColor"].ToString());
        }
    }

    #endregion
}

// Create a template class to represent a dynamic textbox template column.
public class GridViewLabelTemplate : ITemplate
{
    private DataControlRowType templateType;
    private string columnName;
    private string sCntrlName;

    public GridViewLabelTemplate(DataControlRowType type, string colname, string sControlName)
    {
        templateType = type;
        columnName = colname;
        sCntrlName = sControlName;
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {
        // Create the content for the different row types.
        switch (templateType)
        {
            case DataControlRowType.Header:
                // Create the controls to put in the header
                // section and set their properties.
                Literal lc = new Literal();
                lc.Text = "<b>" + columnName + "</b>";

                // Add the controls to the Controls collection
                // of the container.
                container.Controls.Add(lc);
                break;
            case DataControlRowType.DataRow:
                // Create the controls to put in a data row
                // section and set their properties.
                Label oLabel = new Label();

                // To support data binding, register the event-handling methods
                // to perform the data binding. Each control needs its own event
                // handler.
                oLabel.DataBinding += new EventHandler(this.Marks_DataBinding);
                oLabel.ID = sCntrlName;

                // Add the controls to the Controls collection
                // of the container.
                container.Controls.Add(oLabel);
                break;

            // Insert cases to create the content for the other 
            // row types, if desired.
            default:
                // Insert code to handle unexpected values.
                break;
        }
    }

    private void Marks_DataBinding(object sender, EventArgs e)
    {
        // Get the Label control to bind the value. The Label control
        // is contained in the object that raised the DataBinding 
        // event (the sender parameter).
        Label oLabel = (Label)sender;

        // Get the GridViewRow object that contains the Label control.
        GridViewRow row = (GridViewRow)oLabel.NamingContainer;

        // Get the field value from the GridViewRow object and 
        // assign it to the Text property of the Label control.
        // oLabel.Text = DataBinder.Eval(row.DataItem, "Marks_Scored").ToString();
    }
}
