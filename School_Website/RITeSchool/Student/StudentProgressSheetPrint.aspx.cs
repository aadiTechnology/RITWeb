using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;

public partial class StudentProgressSheetPrint : SchoolBase
{
	PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL;

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			checkModeAndSetcontrols();
		}
		catch (ResultNotAvailableForOtherDiv ex)
		{
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
			lblErrorMsg.CssClass = "LblNoRecord";
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = ex.Message;
		}
		catch (NoResultFound ex)
		{
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
			lblErrorMsg.CssClass = "LblNoRecord";
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = ex.Message;
		}
		catch (System.Data.SqlClient.SqlException ex)
		{
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
			lblErrorMsg.CssClass = "LblNoRecord";
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = ex.Message;
		}
		catch (MarksNotAvailableForResult ex)
		{
			lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
			lblErrorMsg.CssClass = "LblNoRecord";
			lblErrorMsg.Visible = true;
			lblErrorMsg.Text = ex.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to set print mode if this screen is going to open for printing mode.
	/// </summary>
	/// <returns></returns>
	private Boolean checkModeAndSetcontrols()
	{
		if (QueryString != null)
		{
			int iStudentId = 0;
			int iTeacherId = 0;
			int iTestId = 0;
			int iStandardDivisionId = 0;

			if (QueryString["iAcademicYearId"] != null)
			{
				miAcademicYearId = QueryString["iAcademicYearId"].ToInt();
				Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] = miAcademicYearId.ToString();
			}

			if (!string.IsNullOrEmpty(QueryString["iStdDivId"]))
				iStandardDivisionId = QueryString["iStdDivId"].ToInt();
			else if (moUserRole == Constants.UserRoles.Teacher)
				iTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();

			if (!QueryString["iStudId"].IsNullOrEmpty())
			{
				iStudentId = QueryString["iStudId"].ToInt();
				if (miAcademicYearId != Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt())
				{
					miAcademicYearId = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
					iStudentId = StudentBL.GetYearwiseStudentId(miSchoolId, miAcademicYearId, iStudentId);
				}
			}
			if (QueryString["iTestId"] != null)
				iTestId = QueryString["iTestId"].ToInt();
			ProgressSheetBase oStudentProgress = new StudentProgress();
			

			if (moUserRole == Constants.UserRoles.Student)
			{
				iStandardDivisionId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
				if (miAcademicYearId != 0)
				{
					int iStudent_Id = QueryString["iStudId"].ToInt();
					iStandardDivisionId = SchoolWiseStanderedDivisionTestMasterBL.GetStandardDivisionIdOfYear(miSchoolId, miAcademicYearId, iStudent_Id);
				}
			}

			if (QueryString["IsTeacherView"] != null && QueryString["IsTeacherView"] == Constants.C_YES.ToString())
			{
				hidMode.Value = "TeacherView";
				TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
				SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, iStandardDivisionId, iTestId);
				oSWStdDivTestMasterBL.School_id = miSchoolId;
				oSWStdDivTestMasterBL.Acadmic_year_id = miAcademicYearId;
				oSWStdDivTestMasterBL.Standerd_division_Id = iStandardDivisionId;
				if (!oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(miSchoolId, miAcademicYearId, iStandardDivisionId,moUserRole.ToString()))
				{
					oSWStdDivTestMasterBL.CheckGradeConfigurations();
				}
			}
			if (miAcademicYearId != 0 || oStudentProgress.isTestPublishedForStdDivId(iStandardDivisionId) || (hidMode.Value == "TeacherView"))
			{
				DataTable odtTeachers = TeacherStandardDetailsCollectionBL.GetTeachersForPrePrimaryProgressReport(iStandardDivisionId, miSchoolId, miAcademicYearId);
				if (odtTeachers.Rows.Count > 0)
				{

					if (iStudentId == 0)
					{
						StudentProgress objStudentProgress = new StudentProgress();
						DataTable odtStudents = objStudentProgress.GetStudentDatset(iStandardDivisionId, false);
						for (int i = 0; i < odtStudents.Rows.Count; i++)
							DisplayProgresReport(odtStudents.Rows[i]["Student_Id"].ToInt(), 0);
					}
					else
					{
						DisplayProgresReport(iStudentId.ToInt(), 0);
					}
				}
				else
				{
					PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
					TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
					
					if (iStudentId != 0)
					{   
                        DataTable odtTeachersData = TeacherStandardDetailsCollectionBL.GetTeachersForPrePrimaryProgressReport(iStandardDivisionId,
                                                                                                                     miSchoolId,
                                                                                                                     miAcademicYearId);

						if (!oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(miSchoolId, miAcademicYearId, moUserRole==Constants.UserRoles.Student? iStudentId:iStandardDivisionId, moUserRole.ToString())
                            || odtTeachersData.Rows.Count == 0)
						{
							oStudentProgress = ProgressSheet.GetProgressSheet(GridViewContainer, miSchoolId, miAcademicYearId, iStudentId, Constants.UserRoles.Student);
							oStudentProgress.TestId = iTestId;
							SetDisplayMode(oStudentProgress);
							oStudentProgress.ShowProgressSheet(iStudentId);
						}
						else
							DisplayProgresReport(iStudentId.ToInt(), miAcademicYearId);
					}
					else
					{
						oStudentProgress = ProgressSheet.GetProgressSheet(GridViewContainer, miSchoolId, miAcademicYearId, iStandardDivisionId, Constants.UserRoles.Teacher);
						oStudentProgress.TestId = iTestId;
						SetDisplayMode(oStudentProgress);
						oStudentProgress.ShowProgressSheet(iStandardDivisionId, iStudentId);
					}
					if (iStudentId != 0)
					{
						if (oStudentProgress is StudentProgress)
                            GenerateResult(iStudentId, iStandardDivisionId);
					}
				}
			}
			else
			{
				throw new NoResultFound("No exam of this class has been published for the current academic year.");
			}
			return false;
		}
		return true;
	}

	/// <summary>
	/// This method is used to set mode to display
	/// </summary>
	/// <param name="oIStudentProgress"></param>
	private void SetDisplayMode(ProgressSheetBase oIStudentProgress)
	{
		if (oIStudentProgress is StudentProgress)
		{
			StudentProgress oStudentProgress = (StudentProgress)oIStudentProgress;
			oStudentProgress.PrintPrefix = "P";
			oStudentProgress.PageMode = Constants.PageMode.Print;
			if (hidMode.Value == "TeacherView")
			{

				oStudentProgress.PrintPrefix = " ";
				oStudentProgress.PageMode = Constants.PageMode.Normal;
				oStudentProgress.ResultType = StudentProgress.enumResultType.TeacherModeProgress;
				hidMode.Value = "TeacherView";
			}
		}
		else
		{
			GridViewContainer.Width = Unit.Pixel(842);
			if (QueryString["IsTeacherView"] == Constants.C_YES.ToString())
				hidMode.Value = "TeacherView";
		}
	}

	/// <summary>
	/// This method is used to create an result of a student
	/// </summary>
	/// <param name="iStudentId"></param>
    private void GenerateResult(int aiStudentId, int aiStandardDivisionId)
	{
		try
		{
			if (Settings.ShowAnnualInProgressSheet)
			{
                if (miAcademicYearId != 0 && CheckIsResultPublished(aiStandardDivisionId))
				{
					StudentResult oStudentResult = new StudentResult(ResultContainer);
					oStudentResult.SetRenderMode(Constants.PageMode.Print);
                    oStudentResult.FillProgressReport(aiStudentId);
				}
			}

		}
		catch (MarksNotAvailableForResult)
		{
		}
		catch (NoResultFound)
		{
		}
	}

	/// <summary>
	/// This method is used to check that is Result is published or not
	/// </summary>
    private Boolean CheckIsResultPublished(int aiStandardDivisionId)
	{
		//int iStandardDivisionId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
        SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
		if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == 0)
			return false;

		return true;
	}

	protected void grdWithOutSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			//DropDownList ocmbRemark;
			Label olblMonth;
			Label olblRemark;
			GridView grdWithOutSubjects = (GridView)sender;
			int cnt = 0;
			foreach (TableCell cell in e.Row.Cells)
			{
				cell.VerticalAlign = VerticalAlign.Middle;
				cell.HorizontalAlign = HorizontalAlign.Center;
				if (cnt > 0)
				{
					cell.Controls.Clear();
					olblMonth = new Label();
					if (e.Row.RowType == DataControlRowType.Header)
					{
						olblMonth.Text = " " + cell.Text + "<br />";
						cell.Controls.Add(olblMonth);
					}
					string sRemark = string.Empty;
					olblRemark = new Label();

					if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
					{
						if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 1].IsPublished)
						{
							PrePrimaryStudentsExamResult remarks = GetRemarkForMonth(e.Row.RowIndex, cnt);
							if (remarks.PrePrimaryRemarkId != 0)
								sRemark = GetRemarkName(remarks.PrePrimaryRemarkId);
							else
								sRemark = " N/A ";
						}
						else
							sRemark = " N/A ";
						olblRemark.Text = sRemark;
					}
					cell.Controls.Add(olblRemark);

					cell.Height = 30;
				}
				else
				{
					if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
					{
						cell.Font.Bold = true;
						cell.Font.Size = 10;
					}
				}
				cnt++;
			}
		}
		catch (Exception Ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(Ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	protected void grdWithSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			Label olblMonth;
			Label olblRemark;
			GridView grdWithSubjects = (GridView)sender;
			int cnt = 0;
			foreach (TableCell cell in e.Row.Cells)
			{
				cell.VerticalAlign = VerticalAlign.Middle;
				cell.HorizontalAlign = HorizontalAlign.Center;
				if (cnt > 1)
				{
					cell.Controls.Clear();
					olblMonth = new Label();

					if (e.Row.RowType == DataControlRowType.Header)
					{
						olblMonth.Text = " " + cell.Text + "<br />";
						cell.Controls.Add(olblMonth);
					}
					string sRemark = string.Empty;
					olblRemark = new Label();
					if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
					{
						if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[cnt - 2].IsPublished)
						{
							PrePrimaryStudentsExamResult remarks = GetRemarkForMonthWithSubject(e.Row.RowIndex, cnt);
							if (remarks.PrePrimaryRemarkId != 0)
								sRemark = GetRemarkName(remarks.PrePrimaryRemarkId);
							else
								sRemark = " N/A ";
						}
						else
							sRemark = " N/A ";
						olblRemark.Text = sRemark;
					}
					cell.Controls.Add(olblRemark);


					cell.Height = 30;
				}
				else if (cnt == 0)
				{
					if (hidSubName.Value != cell.Text)
					{
						if (hidRowNo.Value != "-1")
						{
							grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].RowSpan = hidRowSpan.Value.ToInt();
							grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].Text = hidSubName.Value;
						}
						hidSubName.Value = cell.Text;
						cell.Font.Bold = true;
						hidRowNo.Value = e.Row.RowIndex.ToString();
						hidRowSpan.Value = "1";
					}
					else
					{
						cell.Text = string.Empty;
						cell.Visible = false;
						hidRowSpan.Value = (hidRowSpan.Value.ToInt() + 1).ToString();

						if (oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count() == (e.Row.RowIndex + 1))
						{
							grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].RowSpan = hidRowSpan.Value.ToInt();
							grdWithSubjects.Rows[hidRowNo.Value.ToInt()].Cells[0].Text = hidSubName.Value;
						}
					}
				}
				else
				{
					if (e.Row.RowType != DataControlRowType.Header && e.Row.RowIndex >= 0)
					{
						cell.Font.Bold = true;
						cell.Font.Bold = true;
						cell.Font.Size = 10;
					}
				}
				cnt++;
			}
		}
		catch (Exception Ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(Ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	private void FillRemarkComboBox(DropDownList oDropDownList)
	{
		oDropDownList.Items.Add(new ListItem(Constants.S_SELECT, "0"));
		oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.ForEach(remark => oDropDownList.Items.Add(new ListItem(remark.PrePrimaryProgressReportRemarkName, remark.PrePrimaryProgressReportRemarkId.ToString())));
	}

	private PrePrimaryStudentsExamResult GetRemarkForMonthWithSubject(int airowno, int aicolno)
	{
		PrePrimaryStudentsExamResult remarks;
		PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aicolno - 2];
		PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects[airowno];
		remarks = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithSubjects.AsEnumerable()
				   where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
				   && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
				   && remark.PrePrimarySubjectId == oPrePrimaryProgressReportSubSubjects.SubjectID
				   select new PrePrimaryStudentsExamResult
				   {
					   PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
					   PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
					   PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,

				   }).FirstOrDefault();

		if (remarks != null)
			return remarks;
		else
		{
			remarks = new PrePrimaryStudentsExamResult
			{
				PrePrimaryRemarkId = 0,
				PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
				PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
			};
			return remarks;
		}
	}

	private string GetRemarkName(int aiRemarkId)
	{
		string sRmrkName = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig
							where remark.PrePrimaryProgressReportRemarkId == aiRemarkId
							select remark.PrePrimaryProgressReportRemarkName).FirstOrDefault();

		return sRmrkName;
	}

	private PrePrimaryStudentsExamResult GetRemarkForMonth(int airowno, int aicolno)
	{
		PrePrimaryStudentsExamResult remarks;
		PrePrimaryConfiguredMonthDetails oPrePrimaryConfiguredMonthDetails = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails[aicolno - 1];
		PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects[airowno];
		remarks = (from remark in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamResultWithoutSubjects.AsEnumerable()
				   where remark.PreprimaryExamConfigurationId == oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId
				   && remark.PrePrimaryProgressReportSubSubjectId == oPrePrimaryProgressReportSubSubjects.SubSubjectID
				   select new PrePrimaryStudentsExamResult
				   {
					   PrePrimaryRemarkId = remark.PrePrimaryRemarkId,
					   PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
					   PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,

				   }).FirstOrDefault();

		if (remarks != null)
			return remarks;
		else
		{
			remarks = new PrePrimaryStudentsExamResult
			{
				PrePrimaryRemarkId = 0,
				PrePrimaryProgressReportSubSubjectId = oPrePrimaryProgressReportSubSubjects.SubSubjectID,
				PreprimaryExamConfigurationId = oPrePrimaryConfiguredMonthDetails.PreprimaryExamConfigurationId,
			};
			return remarks;
		}
	}

	private void FillProgressReportTables(PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
	{
		DataTable oDtProgressreportWOSubject = new DataTable();
		DataTable oDtProgressreportWSubject = new DataTable();
		oDtProgressreportWOSubject.Columns.Add("Skills / Behaviour");
		oDtProgressreportWSubject.Columns.Add("Subjects");
		oDtProgressreportWSubject.Columns.Add("Skills / Behaviour");

		oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.ForEach
			(
				s =>
				{
					oDtProgressreportWOSubject.Columns.Add(s.MonthAbbreviation);
					oDtProgressreportWSubject.Columns.Add(s.MonthAbbreviation);
				}
			);

		oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.ForEach
			(
				examresult =>
				{
					DataRow oDataRow = oDtProgressreportWOSubject.NewRow();
					oDataRow[0] = examresult.SubSubjectName;
					oDtProgressreportWOSubject.Rows.Add(oDataRow);
				}
			);
		oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.ForEach
			(
				examresult =>
				{
					DataRow oDataRow = oDtProgressreportWSubject.NewRow();
					oDataRow[0] = examresult.SubjectName;
					oDataRow[1] = examresult.SubSubjectName;
					oDtProgressreportWSubject.Rows.Add(oDataRow);
				}
			);

		CreateStudentInfo(oPrePrimaryProgressSheetConfigBL);
		FillAllGrids(oDtProgressreportWOSubject, oDtProgressreportWSubject);
	}

	private void CreateStudentInfo(PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
	{
		HtmlTable HeaderHtmlTable = CreateHdTable();
		CreateHdSchoolName(HeaderHtmlTable, oPrePrimaryProgressSheetConfigBL);
		CreateHdProgressCard(HeaderHtmlTable);
		CreateHdStudentName(HeaderHtmlTable, oPrePrimaryProgressSheetConfigBL);
		CreateHdStudentAttendance(HeaderHtmlTable, oPrePrimaryProgressSheetConfigBL);
		GridViewContainer.Controls.Add(HeaderHtmlTable);
		HeaderHtmlTable.Dispose();
	}

	/// <summary>
	/// This methos is used to create not applicable ledgend.
	/// </summary>
	private HtmlTable CreateHdTable()
	{
		HtmlTable HeaderHtmlTable = new HtmlTable();
		HeaderHtmlTable.EnableViewState = false;
		HeaderHtmlTable.CellPadding = 0;
		HeaderHtmlTable.CellSpacing = 1;
		HeaderHtmlTable.Attributes.Add("class", "ClsBorderNoBg BGReport");
		HeaderHtmlTable.Width = "60%";
		HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
		HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
		HeaderHtmlTable.Align = "center";
		return HeaderHtmlTable;
	}
	/// <summary>
	/// This methos is used to create not Schooll Name header.
	/// </summary>
	private void CreateHdSchoolName(HtmlTable HeaderHtmlTable, PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
	{
		String sSchoolName = Convert.ToString(oPrePrimaryProgressSheetConfigBL.StudentDetails.School_Name);
		String sSchoolOrgnName = Convert.ToString(oPrePrimaryProgressSheetConfigBL.StudentDetails.School_Orgn_Name);
		HtmlTableRow oHtmlTableRow = new HtmlTableRow();
		CreateHtmlCell(oHtmlTableRow, sSchoolOrgnName, "SocietyName", 1, 8, HorizontalAlign.Center);
		HeaderHtmlTable.Rows.Add(oHtmlTableRow);
		oHtmlTableRow = new HtmlTableRow();
		CreateHtmlCell(oHtmlTableRow, sSchoolName, "ActualSchoolName", 1, 8, HorizontalAlign.Center);
		HeaderHtmlTable.Rows.Add(oHtmlTableRow);
		oHtmlTableRow.Dispose();
	}

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
	/// This methos is used to create not Student name.
	/// </summary>
	private void CreateHdStudentName(HtmlTable HeaderHtmlTable, PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
	{
		HtmlTableRow oHtmlTableRow = new HtmlTableRow();
		AddStudentInfo(oHtmlTableRow, "Roll No. ", oPrePrimaryProgressSheetConfigBL.StudentDetails.RollNo.ToString());
		AddStudentInfo(oHtmlTableRow, "Name ", oPrePrimaryProgressSheetConfigBL.StudentDetails.StudentName);
		AddStudentInfo(oHtmlTableRow, "Class ", oPrePrimaryProgressSheetConfigBL.StudentDetails.ClassName);
		AddStudentInfo(oHtmlTableRow, "Year ", oPrePrimaryProgressSheetConfigBL.StudentDetails.Academic_Year);
		HeaderHtmlTable.Rows.Add(oHtmlTableRow);
		oHtmlTableRow.Dispose();
	}

	/// <summary>
	/// This methos is used to create Student attendance.
	/// </summary>
	private void CreateHdStudentAttendance(HtmlTable HeaderHtmlTable, PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL)
	{
		HtmlTableRow oHtmlTableRow = new HtmlTableRow();
		CreateHtmlCell(oHtmlTableRow, "Term-I Attendance", "ClsBGWhite ClsBorderlight", 0, 2, HorizontalAlign.Right);
		CreateHtmlCell(oHtmlTableRow, oPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_PresentDay + " out of " + oPrePrimaryProgressSheetConfigBL.StudentDetails.First_Term_Total, "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
		CreateHtmlCell(oHtmlTableRow, "Term-II Attendance", "ClsBGWhite ClsBorderlight ", 0, 2, HorizontalAlign.Right);
		CreateHtmlCell(oHtmlTableRow, oPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_PresentDay + " out of " + oPrePrimaryProgressSheetConfigBL.StudentDetails.Final_Term_Total, "ClsHilightTextB ClspaddingR ClsBorderlight", 0, 2, HorizontalAlign.Left);
		HeaderHtmlTable.Rows.Add(oHtmlTableRow);
		oHtmlTableRow.Dispose();
	}

	/// This method is used to create cell
	/// </summary>
	/// <param name="sInnerText"></param>
	/// <param name="sClassName"></param>
	/// <param name="iRowSpan"></param>
	/// <param name="iColSpan"></param>
	private void CreateHtmlCell(HtmlTableRow oHtmlTableRow, String sInnerText, String sClassName, int iRowSpan, int iColSpan, HorizontalAlign sAlignment)
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
	/// This method is used to student info pair to html row.
	/// </summary>
	/// <param name="oHtmlTableRow"></param>
	/// <param name="asLblText"></param>
	/// <param name="asLblVal"></param>
	private void AddStudentInfo(HtmlTableRow oHtmlTableRow, String asLblText, String asLblVal)
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


	private void FillAllGrids(DataTable oDtProgressreportWOSubject, DataTable oDtProgressreportWSubject)
	{

		GridView grdWithSubjects = new GridView();
		GridView grdWithOutSubjects = new GridView();

		grdWithSubjects.Width = Unit.Percentage(60);
		grdWithOutSubjects.Width = Unit.Percentage(60);

		grdWithSubjects.HorizontalAlign = HorizontalAlign.Center;
		grdWithOutSubjects.HorizontalAlign = HorizontalAlign.Center;

		grdWithSubjects.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
		grdWithSubjects.RowStyle.Font.Size = FontUnit.Point(9);
		grdWithSubjects.RowStyle.Font.Bold = false;

		grdWithOutSubjects.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
		grdWithOutSubjects.RowStyle.Font.Size = FontUnit.Point(9);

		grdWithSubjects.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
		grdWithSubjects.HeaderStyle.Font.Size = FontUnit.Point(10);
		grdWithSubjects.HeaderStyle.Height = Unit.Pixel(40);

		grdWithOutSubjects.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
		grdWithOutSubjects.HeaderStyle.Font.Size = FontUnit.Point(10);
		grdWithOutSubjects.HeaderStyle.Height = Unit.Pixel(40);

		grdWithSubjects.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
		grdWithSubjects.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
		grdWithSubjects.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
		grdWithSubjects.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
		grdWithSubjects.AlternatingRowStyle.Font.Bold = false;

		grdWithOutSubjects.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
		grdWithOutSubjects.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
		grdWithOutSubjects.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
		grdWithOutSubjects.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
		grdWithOutSubjects.AlternatingRowStyle.Font.Bold = false;

		grdWithSubjects.GridLines = GridLines.None;
		grdWithSubjects.CellSpacing = 1;
		grdWithSubjects.ForeColor = System.Drawing.Color.Black;

		grdWithOutSubjects.GridLines = GridLines.None;
		grdWithOutSubjects.CellSpacing = 1;
		grdWithOutSubjects.ForeColor = System.Drawing.Color.Black;

		if (oDtProgressreportWOSubject.Rows.Count > 0)
		{
			grdWithOutSubjects.RowDataBound += grdWithOutSubjects_RowDataBound;
			grdWithOutSubjects.DataSource = oDtProgressreportWOSubject;
			grdWithOutSubjects.DataBind();

			GridViewContainer.Visible = true;

			if (grdWithOutSubjects.Rows.Count > 0)
			{
				HtmlTable HeaderHtmlTable = new HtmlTable();
				HeaderHtmlTable.EnableViewState = false;
				HeaderHtmlTable.Width = "100%";
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
				HeaderHtmlTable.Align = "center";

				HtmlTableRow oHtmlTableRow = new HtmlTableRow();
				oHtmlTableRow.Align = "center";

				Label lblModuleName = new Label();
				lblModuleName.Text = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[0].ModuleName;
				lblModuleName.CssClass = "HeadTxtBWOPadding";

				HtmlTableCell oHtmlTableCell = new HtmlTableCell();
				oHtmlTableCell.Controls.Add(lblModuleName);
				oHtmlTableCell.Align = "center";

				oHtmlTableRow.Cells.Add(oHtmlTableCell);

				HeaderHtmlTable.Rows.Add(oHtmlTableRow);

				GridViewContainer.Controls.Add(HeaderHtmlTable);
			}

			GridViewContainer.Controls.Add(grdWithOutSubjects);
		}

		if (oDtProgressreportWSubject.Rows.Count > 0)
		{
			grdWithSubjects.RowDataBound += grdWithSubjects_RowDataBound;
			grdWithSubjects.DataSource = oDtProgressreportWSubject;
			grdWithSubjects.DataBind();


			GridViewContainer.Visible = true;


			if (grdWithSubjects.Rows.Count > 0)
			{
				HtmlTable HeaderHtmlTable = new HtmlTable();
				HeaderHtmlTable.EnableViewState = false;
				HeaderHtmlTable.Width = "100%";
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
				HeaderHtmlTable.Align = "center";

				HtmlTableRow oHtmlTableRow = new HtmlTableRow();
				oHtmlTableRow.Align = "center";

				Label lblModuleNameWithSubject = new Label();
				lblModuleNameWithSubject.Text = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule[1].ModuleName;
				lblModuleNameWithSubject.CssClass = "HeadTxtBWOPadding";

				HtmlTableCell oHtmlTableCell = new HtmlTableCell();
				oHtmlTableCell.Controls.Add(lblModuleNameWithSubject);
				oHtmlTableCell.Align = "center";

				oHtmlTableRow.Cells.Add(oHtmlTableCell);

				HeaderHtmlTable.Rows.Add(oHtmlTableRow);

				GridViewContainer.Controls.Add(HeaderHtmlTable);
			}

			GridViewContainer.Controls.Add(grdWithSubjects);
		}
		if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Count != 0)
		{
			GridView grdViewRemarks = new GridView();
			grdViewRemarks.Width = Unit.Percentage(60);

			grdViewRemarks.RowStyle.CssClass = "Lbl10pt ConfigHeadBG";
			grdViewRemarks.RowStyle.Font.Size = FontUnit.Point(9);
			grdViewRemarks.RowStyle.Font.Bold = false;


			grdViewRemarks.HeaderStyle.CssClass = "ClsProgressGridTestHeader";
			grdViewRemarks.HeaderStyle.Font.Size = FontUnit.Point(10);
			grdViewRemarks.HeaderStyle.Height = Unit.Pixel(30);



			grdViewRemarks.AlternatingRowStyle.CssClass = "ClsProgressGridTestHeader";
			grdViewRemarks.AlternatingRowStyle.Font.Size = FontUnit.Point(9);
			grdViewRemarks.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#eef1ea");
			grdViewRemarks.AlternatingRowStyle.ForeColor = System.Drawing.Color.Black;
			grdViewRemarks.AlternatingRowStyle.Font.Bold = false;

			grdViewRemarks.RowStyle.Wrap = true;
			grdViewRemarks.HorizontalAlign = HorizontalAlign.Center;

			grdViewRemarks.GridLines = GridLines.None;
			grdViewRemarks.CellSpacing = 1;
			grdViewRemarks.ForeColor = System.Drawing.Color.Black;

			grdViewRemarks.DataSource = oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStudentsExamComment.Where(i => i.IsPublished == true).Select(i => new { Header = i.Header, Comments = i.Comment });
			grdViewRemarks.DataBind();

			if (grdViewRemarks.Rows.Count > 0)
			{
				HtmlTable HeaderHtmlTable = new HtmlTable();
				HeaderHtmlTable.EnableViewState = false;
				HeaderHtmlTable.Width = "100%";
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
				HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
				HeaderHtmlTable.Align = "center";

				HtmlTableRow oHtmlTableRow = new HtmlTableRow();
				oHtmlTableRow.Align = "center";

				Label lblRemark = new Label();
				lblRemark.Text = "Remarks";
				lblRemark.CssClass = "HeadTxtBWOPadding";

				HtmlTableCell oHtmlTableCell = new HtmlTableCell();
				oHtmlTableCell.Controls.Add(lblRemark);
				oHtmlTableCell.Align = "center";

				oHtmlTableRow.Cells.Add(oHtmlTableCell);

				HeaderHtmlTable.Rows.Add(oHtmlTableRow);

				GridViewContainer.Controls.Add(HeaderHtmlTable);
			}
			GridViewContainer.Controls.Add(grdViewRemarks);

		}
		int iStudentId = 0;
		if (QueryString != null && QueryString["iStudId"] != null)
			iStudentId = QueryString["iStudId"].ToInt();

		if (iStudentId == 0)
		{
			HtmlTable oHtmlTable = new HtmlTable();
			oHtmlTable.EnableViewState = false;
			oHtmlTable.Width = "100%";
			oHtmlTable.Height = "50px";
			oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
			oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
			oHtmlTable.Align = "center";

			HtmlTableRow oTableRow = new HtmlTableRow();
			oTableRow.Align = "center";

			Label lblLine = new Label();
			lblLine.Text = "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
			HtmlTableCell oTableCell = new HtmlTableCell();
			oTableCell.Controls.Add(lblLine);

			oTableRow.Cells.Add(oTableCell);

			oHtmlTable.Rows.Add(oTableRow);

			GridViewContainer.Controls.Add(oHtmlTable);
		}

	}


	private void DisplayProgresReport(int aiStudentId, int aiAcademicYearId)
	{
		int iAcademicYearId = aiAcademicYearId == 0 ? miAcademicYearId : aiAcademicYearId;
		oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
		oPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressSheetDetailsOfStudent(miSchoolId, iAcademicYearId, aiStudentId);

		if (oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails.Count == 0 ||
			oPrePrimaryProgressSheetConfigBL.LstPrePrimaryRemarkConfig.Count == 0 ||
			(oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithSubjects.Count == 0 && oPrePrimaryProgressSheetConfigBL.LstSubSubjectsWithoutSubjects.Count == 0))
		{
			if (GridViewContainer.FindControl("lblNotPublished") == null)
			{

				HtmlTable oHtmlTable = new HtmlTable();
				oHtmlTable.EnableViewState = false;
				oHtmlTable.Width = "60%";
				oHtmlTable.Height = "20px";
				oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
				oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
				oHtmlTable.Align = "center";

				HtmlTableRow oTableRow = new HtmlTableRow();
				oTableRow.Align = "center";

				Label oLabel = new Label();
				oLabel.ID = "lblNotPublished";
				oLabel.CssClass = "ClsConfigText";
				oLabel.Text = "Progress report is not published yet.";

				HtmlTableCell oTableCell = new HtmlTableCell();

				oTableCell.Controls.Add(oLabel);
				oTableCell.BgColor = "#E6EEFC";
				oTableRow.Cells.Add(oTableCell);

				oHtmlTable.Rows.Add(oTableRow);

				oTableCell.Style.Add("border", "1px solid #8FBC8F");

				GridViewContainer.Controls.Add(oHtmlTable);
			}

		}
		else
		{
			var IsMonthPublishCount = from IsPublishCount in oPrePrimaryProgressSheetConfigBL.LstPrePrimaryConfiguredMonthDetails
									  where IsPublishCount.IsPublished == true
									  select new PrePrimaryConfiguredMonthDetails { IsPublished = IsPublishCount.IsPublished };

			if ((IsMonthPublishCount.Count().ToInt() == 0))
			{
				if (GridViewContainer.FindControl("lblNotPublished") == null)
				{
					HtmlTable oHtmlTable = new HtmlTable();
					oHtmlTable.EnableViewState = false;
					oHtmlTable.Width = "60%";
					oHtmlTable.Height = "20px";
					oHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
					oHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
					oHtmlTable.Align = "center";

					HtmlTableRow oTableRow = new HtmlTableRow();
					oTableRow.Align = "center";

					Label oLabel = new Label();
					oLabel.ID = "lblNotPublished";
					oLabel.CssClass = "ClsConfigText";
					oLabel.Text = "Progress report is not published yet.";

					HtmlTableCell oTableCell = new HtmlTableCell();

					oTableCell.Controls.Add(oLabel);
					oTableCell.BgColor = "#E6EEFC";
					oTableRow.Cells.Add(oTableCell);

					oHtmlTable.Rows.Add(oTableRow);

					oTableCell.Style.Add("border", "1px solid #8FBC8F");

					GridViewContainer.Controls.Add(oHtmlTable);
				}
			}
			else
				FillProgressReportTables(oPrePrimaryProgressSheetConfigBL);
		}
	}
}
