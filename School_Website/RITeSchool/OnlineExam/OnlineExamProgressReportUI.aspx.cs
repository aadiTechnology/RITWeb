using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using Utility;
using BusinessLogic.OnlineExamBL;
using SchoolEntities.OnlineExam;
using System.Web.UI.HtmlControls;

public partial class OnlineExamProgressReportUI : SchoolBase
{
    OnlineExamProgressReportBL moOnlineExamProgressReportBL;
    OnlineExamProgressReportDetails moOnlineExamProgressReportDetails;
    protected void Page_Load(object sender, EventArgs e)
    {
        moOnlineExamProgressReportBL = new OnlineExamProgressReportBL(miSchoolId, miAcademicYearId);
        if (!IsPostBack)
        {
            SetDefaultValue();
            FillClassList();

            if (moUserRole == Constants.UserRoles.Student)
            {
                cmbClass.SelectedValue = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToString();
                cmbClass_SelectedIndexChanged(cmbClass, null);

                cmbStudent.SelectedValue = Session[Constants.S_SESSION_STUDENT_ID].ToString();
                btnShow_Click(btnShow, null);
                trFilter.Visible = false;
            }
        }
    }

    private void SetDefaultValue()
    {
        cmbStudent.Items.Add(new ListItem { Text = Constants.S_ALL, Value = Constants.S_ZERO });
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.OnlineExamProgressReport).ToString();
    }

    private void FillClassList()
    {
        // get all class teachers
        DataTable oDt = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);

        if (moUserRole == Constants.UserRoles.Teacher && hidUserHasFullAccess.Value != Constants.S_YES)
        {
            DataRow[] oDataRow = oDt.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID]);
            ControlUtility.FillDropDownList(
                       oDataRow,
                       ref cmbClass,
                       Constants.S_STANDARD_DIVISION_ID_FIELD,
                       Constants.S_TEACHER_NAME_FIELD,
                       Constants.S_SELECT);
            if (oDataRow.Length == 1)
            {
                cmbClass.SelectedIndex = 1;
                cmbClass.Enabled = false;
            }
        }
        else
        {
            ControlUtility.FillDropDownList(
                           oDt,
                           ref cmbClass,
                           Constants.S_STANDARD_DIVISION_ID_FIELD,
                           Constants.S_TEACHER_NAME_FIELD,
                           Constants.S_SELECT);
        }
    }

    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        DataTable oDtStudents = oStudentwiseRemarkMasterBL.GetStudentListOfGivenClassTeacher(cmbClass.SelectedValue.ToInt(), miAcademicYearId, miSchoolId, Constants.I_ZERO);
        ListSource.FillDropDownList(oDtStudents, cmbStudent, "Student_Name", "Student_Id", Constants.S_ALL);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillProgressReportDetails();
    }

    private void FillProgressReportDetails()
    {
        moOnlineExamProgressReportDetails = moOnlineExamProgressReportBL.GetDetails(cmbClass.SelectedValue.ToInt(), cmbStudent.SelectedValue.ToInt());

        if (moOnlineExamProgressReportDetails.Students.Count == 0)
        {
            trMessage.Visible = true;
        }
        else
        {
            trMessage.Visible = false;
            int iIndex = 1;
            moOnlineExamProgressReportDetails.Students.ForEach(stud =>
            {
                DisplayBasicInfo(stud);
                DisplayMarks(stud.StudentId);

                if (moOnlineExamProgressReportDetails.Students.Count > 1 && moOnlineExamProgressReportDetails.Students.Count != iIndex)
                {
                    HtmlTableRow trBlank = new HtmlTableRow { Height = "25px" };
                    HtmlTableCell td = new HtmlTableCell();
                    td.InnerHtml = "<hr style='border: thin dashed #C0C0C0' />";
                    trBlank.Cells.Add(td);
                    tblMainTable.Rows.Add(trBlank);
                }

                iIndex++;
            });
        }
    }

    private void DisplayMarks(int aiStudentId)
    {
        HtmlTable oMarkDetails = new HtmlTable { Align = "left" };

        HtmlTableRow trSubjects = new HtmlTableRow();
        base.AddCell(trSubjects, @"Exam\Subject", "clsExamsAndSubjects", "left", 1, "width:150px");
        moOnlineExamProgressReportDetails.Subjects.ForEach(sub =>
            {
                base.AddCell(trSubjects, sub.Name, "clsExamsAndSubjects", "left", 1);
            });
        oMarkDetails.Rows.Add(trSubjects);

        base.AddCell(trSubjects, "Total", "clsExamsAndSubjects", "left", 1, "width:75px");
        base.AddCell(trSubjects, "Percentage", "clsExamsAndSubjects", "left", 1, "width:75px");

        moOnlineExamProgressReportDetails.OnlineExams.ForEach(exam =>
            {
                HtmlTableRow trExam = new HtmlTableRow();
                base.AddCell(trExam, exam.Name, "clsExamsAndSubjects", "left", 1);
                oMarkDetails.Rows.Add(trExam);

                moOnlineExamProgressReportDetails.Subjects.ForEach(sub =>
                {
                    var oMarks =  moOnlineExamProgressReportDetails.MarkInformation.Where(mk => mk.ExamId == exam.Id && mk.SubjectId == sub.SubjectId && mk.StudentId == aiStudentId).FirstOrDefault();

                    if(oMarks != null)
                        base.AddCell(trExam, oMarks.Marks + "/" + oMarks.OutOfMarks, "clsStudentActualMarks", "left", 1);
                    else
                        base.AddCell(trExam, "-", "clsStudentActualMarks", "left", 1);
                });

                var iTotalMarks = moOnlineExamProgressReportDetails.MarkInformation.Where(mk => mk.ExamId == exam.Id && mk.StudentId == aiStudentId).Sum(mk => mk.Marks);
                var iOutOfMarks = moOnlineExamProgressReportDetails.MarkInformation.Where(mk => mk.ExamId == exam.Id && mk.StudentId == aiStudentId).Sum(mk => mk.OutOfMarks);

                base.AddCell(trExam, iTotalMarks + "/" + iOutOfMarks, "clsStudentActualMarks", "left", 1);
                base.AddCell(trExam, Math.Round((iTotalMarks.ToDecimal()/iOutOfMarks)*100,2).ToString() , "clsStudentActualMarks", "left", 1);
            });
                
        AddInMainTable(oMarkDetails);        
    }

    private void DisplayBasicInfo(StudentInfo aoStudentInfo)
    {
        HtmlTable oBasicDetails = new HtmlTable { Width = "100%" };

        HtmlTableRow trSchoolName = new HtmlTableRow();
        base.AddCell(trSchoolName, moOnlineExamProgressReportDetails.SchoolInformation.OrgName, "clsSchoolName", "Center", 8);
        oBasicDetails.Rows.Add(trSchoolName);

        HtmlTableRow trOrgName = new HtmlTableRow();
        base.AddCell(trOrgName, moOnlineExamProgressReportDetails.SchoolInformation.SchoolName, "clsOrgName", "Center", 8);
        oBasicDetails.Rows.Add(trOrgName);

        HtmlTableRow trHeader = new HtmlTableRow();
        base.AddCell(trHeader, "Online Exam Progress Report", "clsHeader1", "Center", 8);
        oBasicDetails.Rows.Add(trHeader);

        HtmlTableRow trStudentInfo = new HtmlTableRow();
        base.AddCell(trStudentInfo, "Roll No.", "clsStudentInfo", "left", 1, "width:75px");
        base.AddCell(trStudentInfo, aoStudentInfo.RollNo.ToString(), "clsStudentInfoData", "left", 1, "width:75px");
        base.AddCell(trStudentInfo, "Name", "clsStudentInfo", "left", 1, "width:75px");
        base.AddCell(trStudentInfo, aoStudentInfo.StudentName, "clsStudentInfoData", "left", 1);
        base.AddCell(trStudentInfo, "Class", "clsStudentInfo", "left", 1, "width:50x");
        base.AddCell(trStudentInfo, aoStudentInfo.ClassName, "clsStudentInfoData", "left", 1, "width:200px");
        base.AddCell(trStudentInfo, "Year", "clsStudentInfo", "left", 1, "width:75px");
        base.AddCell(trStudentInfo, aoStudentInfo.AcademicYear, "clsStudentInfoData", "left", 1, "width:100px");
        oBasicDetails.Rows.Add(trStudentInfo);

        AddInMainTable(oBasicDetails);
    }

    private void AddInMainTable(Control aoControl)
    {
        HtmlTableRow tr = new HtmlTableRow();
        HtmlTableCell td = new HtmlTableCell();
        td.Controls.Add(aoControl);
        tr.Cells.Add(td);
        tblMainTable.Rows.Add(tr);
    }


}