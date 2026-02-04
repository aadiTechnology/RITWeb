/* Class Name - StudentMarksExportUI
 * Created Date - 5-May-2021
 * Created By - Sachin
 * Description - This class is used to export student marks details.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SchoolEntities.ProgressReport;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class StudentMarksExportUI : ExportToExcel
{
    #region Data MEmber(s)

    private StudentMarksBL moStudentMarksBL;
    private int miIndex = 1;
    private StudentConsolidatedMarkDetails moStudentConsolidatedMarkDetails;
    private Dictionary<int, int> mSubjects = new Dictionary<int, int>(); 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill academic years.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentMarksBL = new StudentMarksBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillAcademicYears();
                SetDefaultValue();
                VAlSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to standard list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbAcademicYear.SelectedValue != Constants.S_ZERO)
            {
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());
                DataTable dtStandards = oStandardCollectionBL.GetAllStandards();
                ListSource.FillDropDownList(dtStandards, cmbStandard, "Standard_Name", "Standard_Id", Constants.S_SELECT);
            }
            else
            {
                FIllDefaultValue(cmbStandard, Constants.S_SELECT);
                FIllDefaultValue(cmbDivision, Constants.S_SELECT);
                FIllDefaultValue(cmbTest, Constants.S_ALL);
                FIllDefaultValue(cmbSubject, Constants.S_ALL);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbStandard.SelectedValue != Constants.S_ZERO)
            {
                DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());
                DataTable dtDivisions = oDivisionCollectionBL.GetAllDivisionsForStandard(cmbStandard.SelectedValue.ToInt());
                ListSource.FillDropDownList(dtDivisions, cmbDivision, "Division_Name", "SchoolWise_Standard_Division_Id", Constants.S_SELECT);
            }
            else
            {
                FIllDefaultValue(cmbDivision, Constants.S_SELECT);
                FIllDefaultValue(cmbTest, Constants.S_ALL);
                FIllDefaultValue(cmbSubject, Constants.S_ALL);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill test and subject list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());
            DataSet dsSubjects = oSubjectCollectionBL.GetAllSubjectsforDivision(cmbDivision.SelectedValue.ToInt());
            DataTable dtSubjects = dsSubjects.Tables[0];

            if (dsSubjects.Tables[0].Rows.Count > 0)
                dsSubjects.Tables[0].AsEnumerable().OrderBy(sb => sb.Field<int>("Sort_Order")).CopyToDataTable();

            ListSource.FillDropDownList(dtSubjects, cmbSubject, "Subject_Name", "Subject_Id", Constants.S_ALL);

            List<Test> lstTests = moStudentMarksBL.GetAllTestsForClassSUbject(cmbAcademicYear.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt(), cmbSubject.SelectedValue.ToInt());
            ListSource.FillDropDownList(lstTests, cmbTest, "Name", "TestId", Constants.S_ALL);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export student marks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            moStudentConsolidatedMarkDetails = moStudentMarksBL.GetAllDetails(cmbAcademicYear.SelectedValue.ToInt(), cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt(), cmbTest.SelectedValue.ToInt(), cmbSubject.SelectedValue.ToInt());

            string sFileName = "StudentMarksDetails_" + Guid.NewGuid() + ".xlsx";
            string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                CreateWorkBookPartForStudentPaidFeeReport(workbookPart);
            }

            Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill default values.
    /// </summary>
    private void SetDefaultValue()
    {
        FIllDefaultValue(cmbStandard, Constants.S_SELECT);
        FIllDefaultValue(cmbDivision, Constants.S_SELECT);
        FIllDefaultValue(cmbSubject, Constants.S_ALL);
        FIllDefaultValue(cmbTest, Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to default dropdown.
    /// </summary>
    /// <param name="aoDropDownList"></param>
    /// <param name="asTitleVal"></param>
    private void FIllDefaultValue(DropDownList aoDropDownList, string asTitleVal)
    {
        aoDropDownList.Items.Clear();
        ListItem oListItem = new ListItem { Text = asTitleVal, Value = Constants.S_ZERO };
        aoDropDownList.Items.Add(oListItem);
    }

    /// <summary>
    /// This method is used to fill academicyears.
    /// </summary>
    private void FillAcademicYears()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable dtYears = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId);
        ListSource.FillDropDownList(dtYears, cmbAcademicYear, "YearValue", "Academic_Year_ID", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to create work book part for marks.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookPartForStudentPaidFeeReport(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateMarksContent(worksheetPart1);

        base.GeneratePartContent(aoPart, cmbAcademicYear.SelectedItem.Text + " " + cmbStandard.SelectedItem.Text + "-" + cmbDivision.SelectedItem.Text);
    }

    /// <summary>
    /// This method is used to generate mark content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateMarksContent(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        SetWidth(worksheet1);
        AddSubjects(sheetData1);
        AddTests(sheetData1);
        AddStudents(sheetData1);
        worksheet1.Append(sheetData1);
        worksheet1.Append(MergeHeaderCells());

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used to merge header cells.
    /// </summary>
    /// <returns></returns>
    private MergeCells MergeHeaderCells()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        int iStartIndex = 3;
        int iEndIndex;
        string sCellStart, sCellEnd;

        MergeCell mergeCell3 = null;
        foreach (KeyValuePair<int, int> kvp in mSubjects)
        {
            iEndIndex = iStartIndex + kvp.Value;
            string sReference = string.Empty;

            sCellStart = base.GetReferenceName(iStartIndex);
            sCellEnd = base.GetReferenceName(iEndIndex);

            mergeCell3 = new MergeCell() { Reference = sCellStart + "1" + ":" + sCellEnd + "1" };
            mergeCells1.Append(mergeCell3);

            iStartIndex = iEndIndex + 1;
        }

        return mergeCells1;
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    private void SetWidth(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 40D, CustomWidth = true });

        int iLastINdex = 0;
        if (cmbTest.SelectedValue.ToInt() != 0)
            iLastINdex = 2 + moStudentConsolidatedMarkDetails.ExamConfigs.Count;
        else
            iLastINdex = 2 + moStudentConsolidatedMarkDetails.ExamConfigs.Count + 1;

        if (cmbSubject.SelectedValue.ToInt() == 0 && cmbTest.SelectedValue.ToInt() != 0)
            columns1.Append(new Column() { Min = (UInt32Value)3U, Max = Convert.ToUInt32(iLastINdex), Width = 12D, CustomWidth = true });
        else
            columns1.Append(new Column() { Min = (UInt32Value)3U, Max = Convert.ToUInt32(iLastINdex), Width = 7D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    /// <summary>
    /// This method is used to fill marks.
    /// </summary>
    /// <param name="sheetData1"></param>
    private void AddStudents(SheetData sheetData1)
    {
        moStudentConsolidatedMarkDetails.Students.OrderBy(st => st.RollNo).ToList().ForEach(stud =>
            {
                miIndex++;
                Row row = new Row { RowIndex = Convert.ToUInt32(miIndex), CustomHeight = true, Height = 15 };
                row.Append(AddCell(stud.RollNo.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(stud.Name, CellValues.String, StudentPaidFeeEnum.LeftData));

                moStudentConsolidatedMarkDetails.ExamConfigs.Select(sub => new { sub.SubjectId, sub.SubjectName, sub.SubjectSortOrder }).Distinct().OrderBy(sub => sub.SubjectSortOrder).ToList().ForEach
                  (
                      sub =>
                      {
                          moStudentConsolidatedMarkDetails.ExamConfigs.Where(ec => ec.SubjectId == sub.SubjectId).Select(ec => new { ec.SchoolWiseTestId, ec.SchoolWiseTestName, ec.TestSortOrder }).Distinct().OrderBy(ec => ec.TestSortOrder).ToList().ForEach(
                          tst =>
                          {
                              var oMarks = moStudentConsolidatedMarkDetails.Marks.Where(mk => mk.StudentId == stud.StudentId && mk.SubjectId == sub.SubjectId && mk.SchoolWiseTestId == tst.SchoolWiseTestId).FirstOrDefault();
                              if (oMarks != null)
                              {
                                  if (oMarks.IsAbsent != 'N')
                                  {
                                      string sDisplayVal = moStudentConsolidatedMarkDetails.ExamStatusConfigs.Where(esc => esc.ShortName == oMarks.IsAbsent.ToString()).FirstOrDefault().DisplayValue;
                                      row.Append(AddCell(sDisplayVal, CellValues.String, StudentPaidFeeEnum.CenterData));
                                  }
                                  else
                                      row.Append(AddCell(oMarks.TotalMarksScored.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                              }
                              else
                                  row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterData));
                          }
                          );

                          if (cmbTest.SelectedValue.ToInt() == 0)
                          {
                              int iTotal = moStudentConsolidatedMarkDetails.Marks.Where(mk => mk.StudentId == stud.StudentId && mk.SubjectId == sub.SubjectId).Sum(mk => mk.TotalMarksScored);
                              row.Append(AddCell(iTotal.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataGray));
                          }
                      }
                  );

                int iGrandTotal = moStudentConsolidatedMarkDetails.Marks.Where(mk => mk.StudentId == stud.StudentId).Sum(mk => mk.TotalMarksScored);
                row.Append(AddCell(iGrandTotal.ToString(), CellValues.String, StudentPaidFeeEnum.CenterHeader));

                sheetData1.Append(row);
            });
    }

    /// <summary>
    /// This method is used to fill subjects.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    private void AddSubjects(SheetData aoSheetData1)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miIndex), CustomHeight = true, Height = 15 };

        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        var result = moStudentConsolidatedMarkDetails.ExamConfigs.GroupBy(ec => ec.SubjectId).Select(ec => new { SubjectId = ec.Key, Count = ec.Count() });

        int iIndex = 1;
        moStudentConsolidatedMarkDetails.ExamConfigs.Select(sub => new { sub.SubjectId, sub.SubjectName, sub.SubjectSortOrder }).Distinct().OrderBy(sub => sub.SubjectSortOrder).ToList().ForEach
            (
                sub =>
                {
                    row.Append(AddCell(sub.SubjectName, CellValues.String, StudentPaidFeeEnum.CenterHeader));

                    int iCount = result.Where(sb => sb.SubjectId == sub.SubjectId).FirstOrDefault().Count;
                    for (int k = 0; k < iCount - 1; k++)
                        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));

                    if (cmbTest.SelectedValue.ToInt() == 0)
                    {
                        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));
                        mSubjects.Add(iIndex, iCount);
                    }
                    else
                        mSubjects.Add(iIndex, iCount - 1);

                    iIndex++;
                }
            );

        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is used to tests.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    private void AddTests(SheetData aoSheetData1)
    {
        miIndex++;
        Row row = new Row { RowIndex = Convert.ToUInt32(miIndex), CustomHeight = true, Height = 100 };

        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        int iGrandOutOfMarks = 0;
        moStudentConsolidatedMarkDetails.ExamConfigs.Select(sub => new { sub.SubjectId, sub.SubjectName, sub.SubjectSortOrder }).Distinct().OrderBy(sub => sub.SubjectSortOrder).ToList().ForEach
            (
                sub =>
                {
                    moStudentConsolidatedMarkDetails.ExamConfigs.Where(ec => ec.SubjectId == sub.SubjectId).Select(ec => new { ec.SchoolWiseTestId, ec.SchoolWiseTestName, ec.TestSortOrder }).Distinct().OrderBy(ec => ec.TestSortOrder).ToList().ForEach(
                    tst =>
                    {
                        int iMaxOutOfMarks = moStudentConsolidatedMarkDetails.ExamConfigs.Where(ec => ec.SubjectId == sub.SubjectId && ec.SchoolWiseTestId == tst.SchoolWiseTestId).Max(ec => ec.SubjectTotalMarks);
                        iGrandOutOfMarks += iMaxOutOfMarks;
                        row.Append(AddCell(tst.SchoolWiseTestName + " (" + iMaxOutOfMarks+")", CellValues.String, StudentPaidFeeEnum.CenterHeader90));
                    }
                    );

                    if (cmbTest.SelectedValue.ToInt() == 0)
                    {
                        int iOutOfMarks = moStudentConsolidatedMarkDetails.ExamConfigs.Where(ec => ec.SubjectId == sub.SubjectId).Sum(ec => ec.SubjectTotalMarks);
                        row.Append(AddCell("Total (" + iOutOfMarks+")", CellValues.String, StudentPaidFeeEnum.CenterHeader90));
                    }
                }
            );

        var iGrandTotal = moStudentConsolidatedMarkDetails.Marks.GroupBy(mk => mk.StudentId).Select(mk => new { StudentId = mk.Key, SubjectTotalMarks = mk.Sum(s => s.SubjectTotalMarks) }).Max(mk => mk.SubjectTotalMarks);
        row.Append(AddCell("Grand Total (" + iGrandTotal + ")", CellValues.String, StudentPaidFeeEnum.CenterHeader90));

        aoSheetData1.Append(row);
    } 

    #endregion
}