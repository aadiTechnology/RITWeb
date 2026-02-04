using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
namespace BusinessLogic
{
    public class ExportReportBL
    {
        #region Data Member(s)
        
        private ExportReportDC moExportReportDC;
        private List<StudentMarkDetails> mlstStudentMarkDetails; 
        private string msHost;

        #endregion

        #region Constructor(s)

        public ExportReportBL()
        {
            this.moExportReportDC = new ExportReportDC();
        }

        public ExportReportBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moExportReportDC = new ExportReportDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Property(s)

        public List<SubjectInfo> Subjects
        {
            get
            {
                return this.moExportReportDC.Subjects;
            }
        }


        public List<TestDetails> TestDetails
        {
            get
            {
                return this.moExportReportDC.TestDetails;
            }
        }

        public List<StudentInfoForExam> StudentInfos
        {
            get
            {
                return this.moExportReportDC.StudentInfos;
            }
        }

        public BasicInfo BasicInfo
        {
            get
            {
                return this.moExportReportDC.BasicInfo;
            }
        }

        public List<StudentMarkSummary> StudentMarkSummary
        {
            get
            {
                return this.moExportReportDC.StudentMarkSummary;
            }
        }

        public string Host
        {
            get
            {
                return msHost;
            }
            set
            {
                msHost = value;
            }
        }

        #endregion

        #region Public Method(s)
       
        /// <summary>
        /// This method is used to return data to export result sheet details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentMarkDetails> GetResultSheetDetailsForExcelInterop(int aiStandardId, int aiDivisionId, int aiTestId, int aiTermId)
        {
            this.mlstStudentMarkDetails = this.moExportReportDC.GetResultSheetDetails(aiStandardId, aiDivisionId, aiTestId, false, aiTermId);
            StringBuilder obj = new StringBuilder();
            //obj.Append(AddHeader(false));
            //obj.Append(AddStudentMarkDetails());
            //obj.Append("</Table>");
            return this.mlstStudentMarkDetails;
        }

        /// <summary>
        /// This method is used to return data to export result sheet details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentMarkDetails> GetResultSheetDetailsForPrelimReport(int aiStandardId, int aiDivisionId, int aiTestId)
        {
            this.mlstStudentMarkDetails = this.moExportReportDC.GetResultSheetDetails(aiStandardId, aiDivisionId, aiTestId, true,0);
            return this.mlstStudentMarkDetails;
        }

        public StringBuilder GetResultSheetDetails(int aiStandardId, int aiDivisionId, int aiTestId)
        {
            this.mlstStudentMarkDetails = this.moExportReportDC.GetResultSheetDetails(aiStandardId, aiDivisionId, aiTestId, false,0);
            StringBuilder obj = new StringBuilder();
            obj.Append(AddHeader(false));
            obj.Append(AddStudentMarkDetails());
            obj.Append("</Table>");
            return obj;
        }

        public List<StudentMarkDetails> GetAnnualConsolDetailsForHSP(int aiStandardId, int aiDivisionId)
        {
            return this.moExportReportDC.GetAnnualConsolDetailsForHSP(aiStandardId, aiDivisionId);                  
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to add headers.
        /// </summary>
        /// <param name="abAddSeparator"></param>
        /// <returns></returns>
        private string AddHeader(bool abAddSeparator)
        {
            StringBuilder obj = new StringBuilder();


            int iGroupCount = Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

            int iSubjectCount = 0;
            if (BasicInfo.ShowGrades)
                iSubjectCount = (Subjects.Count * 2) + iGroupCount + 7;
            else
                iSubjectCount = Subjects.Count + iGroupCount + 6;

            if (!abAddSeparator)
            {
                obj.Append("<Table width='100%' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");

                obj.Append("<TR>");
                obj.Append(AddCell(string.Empty));

                string sImage = "<div style='height:100px;width:100px;'><img style='object-fit:cover;width:100%;height:100%' src='" + msHost + "/RITeSchool/images/Logos/School_Logo_Small.jpg'></img><div>";

                obj.Append(AddCell(string.Empty, string.Empty, 1, 2, sImage));

                obj.Append(AddCell(BasicInfo.SchoolName + ", " + BasicInfo.Location, "text-align:center;font-weight:bold;font-size:14pt;font-family:SHREE-ENG-0252", iSubjectCount - 5));

                obj.Append("</TR>");

                obj.Append("<TR>");
                obj.Append(AddCell(string.Empty));
                obj.Append(AddCell("<U>RESULT OF " + BasicInfo.TestName + "   " + BasicInfo.AcademicYear + "</U>", "text-align:center;font-weight:bold;font-size:14pt;", iSubjectCount - 5));
                obj.Append("</TR>");

                obj.Append("<TR style='height:8px;'>");
                obj.Append("</TR>");

                obj.Append("<TR>");
                obj.Append(AddCell("CLASS : " + BasicInfo.ClassName, "text-align:right;font-weight:bold;", iSubjectCount));
                obj.Append("</TR>");
                obj.Append("</TABLE>");
            }
            else
            {
                obj.Append("<Table width='100%' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
                obj.Append(AddBlankRow());
                obj.Append("</TABLE>");
            }

            obj.Append("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
            obj.Append(AddGroupSubjectRow());
            obj.Append(AddSubjectRow());

            obj.Append("<TR>");
            obj.Append(AddOutOfMarks(false));

            if (StudentMarkSummary.Any())
            {
                var iTotal = StudentMarkSummary.Max(sms => sms.OutOfMarks);
                obj.Append(AddCell(iTotal.ToString(), "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                obj.Append(AddCell("100", "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                if (BasicInfo.ShowGrades)
                    obj.Append(AddCell("G", "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                obj.Append(AddOutOfMarks(true));
            }

            obj.Append("</TR>");
            
            return obj.ToString();
        }

        /// <summary>
        /// This method is used to add group subject row.
        /// </summary>
        /// <returns></returns>
        private string AddGroupSubjectRow()
        {
            StringBuilder obj = new StringBuilder();
            if (Subjects.Any(sb => sb.ParentSubject != string.Empty))
            {
                if (!Subjects.Any(sb => sb.ParentSubject != string.Empty))
                {
                    obj.Append("<TR>");
                    obj.Append(AddCell(string.Empty, "max-width:50px;background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append(AddGroupSubjects(false));
                    obj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;"));
                    obj.Append("</TR>");
                }
                else
                {
                    obj.Append("<TR>");
                    obj.Append(AddCell("ROLL NO.", "padding-top:10px;max-width:50px;text-align:center;background-color:#a6a6a6;font-weight:bold;vertical-align:middle;font-size:12pt;word-wrap:break-word", 1, 3));
                    obj.Append(AddCell("STUDENT NAME", "width:250px;background-color:#a6a6a6;font-weight:bold;text-align:center;vertical-align:middle;font-size:12pt;", 1, 3));
                    obj.Append(AddCell("HOUSE NAME", "width:100px;background-color:#a6a6a6;font-weight:bold;text-align:center;vertical-align:middle;font-size:12pt;", 1, 3));
                    obj.Append(AddGroupSubjects(false));
                    obj.Append(AddCell("<p>GRAND</p>TOTAL", "background-color:#a6a6a6;font-weight:bold;text-align:center;font-size:12pt;vertical-align:middle;", 1, 2));
                    obj.Append(AddCell("PER (%)", "background-color:#a6a6a6;font-weight:bold;text-align:center;font-size:12pt;vertical-align:middle;", 1, 2));
                    obj.Append(AddGroupSubjects(true));
                    obj.Append(AddCell("RANK", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;vertical-align:middle;font-size:12pt;", 1, 3));
                    obj.Append("</TR>");
                }
            }
            return obj.ToString();
        }

        /// <summary>
        /// This method is used to add subject row.
        /// </summary>
        /// <returns></returns>
        private string AddSubjectRow()
        {
            StringBuilder obj = new StringBuilder();
            obj.Append("<TR style='position:fixed;'>");

            if (!Subjects.Any(sb => sb.ParentSubject != string.Empty))
            {
                obj.Append(AddCell("ROLL NO.", "max-width:50px;text-align:center;background-color:#a6a6a6;font-weight:bold;vertical-align:middle;font-size:12pt;word-wrap:break-word", 1, 2));
                obj.Append(AddCell("STUDENT NAME", "width:250px;background-color:#a6a6a6;font-weight:bold;text-align:center;vertical-align:middle;font-size:12pt;", 1, 2));
                obj.Append(AddCell("HOUSE NAME", "width:100px;background-color:#a6a6a6;font-weight:bold;text-align:center;vertical-align:middle;font-size:12pt;", 1, 2));

                obj.Append(AddSubjects(false));

                obj.Append(AddCell("GRAND TOTAL", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                obj.Append(AddCell("PER (%)", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;"));

                if (BasicInfo.ShowGrades)
                    obj.Append(AddCell("GRADE", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;"));

                obj.Append(AddSubjects(true));
                obj.Append(AddCell("RANK", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;vertical-align:middle;font-size:12pt;", 1, 2));
            }
            else
            {
                obj.Append(AddSubjects(false));

                //obj.Append(AddCell("GRAND TOTAL", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));                
                //obj.Append(AddCell("PER (%)", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;"));
                if (BasicInfo.ShowGrades)
                    obj.Append(AddCell("GRADE", "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));

                obj.Append(AddSubjects(true));
            }
            obj.Append("</TR>");
            return obj.ToString();
        }

        /// <summary>
        /// This method is used to add signature section.
        /// </summary>
        /// <returns></returns>
        private string AddSignatures()
        {
            int iGroupCount = Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

            int iSubjectCount = 0;
            if (BasicInfo.ShowGrades)
                iSubjectCount = (Subjects.Count * 2) + iGroupCount + 2;
            else
                iSubjectCount = Subjects.Count + iGroupCount + 1;

            StringBuilder obj = new StringBuilder();
            obj.Append("<Table bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
            obj.Append(AddBlankRow());
            obj.Append(AddBlankRow()); ;
            obj.Append(AddBlankRow());
            obj.Append("<TR>");
            obj.Append(AddCell("CLASS TEACHER", "text-align:center;font-weight:bold;", 2));
            obj.Append(AddCell("COORDINATOR", "text-align:center;font-weight:bold;", iSubjectCount));
            obj.Append(AddCell("PRINCIPAL", "text-align:center;font-weight:bold;", 3));
            obj.Append("</TR>");

            obj.Append(AddBlankRow());
            return obj.ToString();
        }

        /// <summary>
        /// This method is used to add blank row.
        /// </summary>
        /// <returns></returns>
        private string AddBlankRow()
        {
            StringBuilder obj = new StringBuilder();
            obj.Append("<TR>");
            obj.Append("</TR>");
            return obj.ToString();
        }

        /// <summary>
        /// This method is used to add group subjects.
        /// </summary>
        /// <returns></returns>
        private string AddGroupSubjects(bool abIsCoCurriSubject)
        {
            StringBuilder sbObj = new StringBuilder();
            string sOldParentSubejct = string.Empty;
            int iColSpan = 1;
            Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                (
                    sb =>
                    {
                        if (sb.ParentSubject != string.Empty)
                        {
                            iColSpan++;

                            if (sOldParentSubejct != string.Empty && sOldParentSubejct != sb.ParentSubject)
                            {
                                sbObj.Append(AddCell(sOldParentSubejct, "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;", iColSpan));
                                iColSpan = 1;
                            }
                        }
                        else
                        {
                            if (sOldParentSubejct != string.Empty)
                                sbObj.Append(AddCell(sOldParentSubejct, "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;", iColSpan + 1));

                            sbObj.Append(AddCell(sb.SubjectName, "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;",1,2));
                            //sbObj.Append(AddCell(string.Empty, "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                            iColSpan = 0;
                        }

                        sOldParentSubejct = sb.ParentSubject;
                    }
                );

            if (sOldParentSubejct != string.Empty)
                sbObj.Append(AddCell(sOldParentSubejct, "width:100px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;", iColSpan + 1));
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to add student mark details.
        /// </summary>
        /// <returns></returns>
        private string AddStudentMarkDetails()
        {
            StringBuilder sbObj = new StringBuilder();
         //   int iCounter = 1;

            StudentInfos.OrderBy(si => si.RollNo).ToList().ForEach(
                stud =>
                {
                    sbObj.Append("<TR style='height:22pt;'>");
                    sbObj.Append(AddCell(stud.RollNo.ToString(), "text-align:center;font-size:12pt;vertical-align:middle;"));
                    sbObj.Append(AddCell(stud.StudentName, "text-align:left;padding-left:10px;font-size:12pt;vertical-align:middle;"));
                    sbObj.Append(AddCell(stud.HouseName, "text-align:center;font-size:12pt;vertical-align:middle;"));

                    sbObj.Append(SetSubjectMarks(stud.StudentId));

                    var oTotal = StudentMarkSummary.Where(ss => ss.StudentId == stud.StudentId).FirstOrDefault();
                    sbObj.Append(SetSummaryFields(oTotal));
                    sbObj.Append(SetCoCurriSubjectMarks(stud.StudentId));

                    if (oTotal != null)
                        sbObj.Append(AddCell(oTotal.Rank.ToString(), "text-align:center;font-size:12pt;vertical-align:middle;"));
                    else
                        sbObj.Append(AddCell(string.Empty));

                    sbObj.Append("</TR>");

                    //if (iCounter % 20 == 0)
                    //{
                    //    sbObj.Append("</TABLE>");
                    //    sbObj.Append(AddHeader(true));
                    //}

                  //  iCounter++;
                }
                );

            sbObj.Append(AddSignatures());
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to set summary fields.
        /// </summary>
        /// <param name="aoTotal"></param>
        /// <returns></returns>
        private string SetSummaryFields(StudentMarkSummary aoTotal)
        {
            StringBuilder sbObj = new StringBuilder();
            if (aoTotal != null)
            {
                sbObj.Append(AddCell(aoTotal.TotalScoredMarks.ToString(), "text-align:center;font-size:12pt;vertical-align:middle;"));
                sbObj.Append(AddCell(aoTotal.Percentage.ToString(), "text-align:center;font-size:12pt;vertical-align:middle;"));

                if (BasicInfo.ShowGrades)
                    sbObj.Append(AddCell(aoTotal.Grade, "text-align:center;font-size:12pt;vertical-align:middle;"));
            }
            else
            {
                sbObj.Append(AddCell(string.Empty));
                sbObj.Append(AddCell(string.Empty));
                if (BasicInfo.ShowGrades)
                    sbObj.Append(AddCell(string.Empty));
            }
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to set co-curricular subject marks/grades.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        private string SetCoCurriSubjectMarks(int aiStudentId)
        {
            StringBuilder sbObj = new StringBuilder();
            Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                var oMarks = this.mlstStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
                if (oMarks != null)
                {
                    sbObj.Append(AddCell(oMarks.ScoredMarks.ToString(), "text-align:center;font-size:12pt;vertical-align:middle;"));
                    if (BasicInfo.ShowGrades)
                        sbObj.Append(AddCell(oMarks.Grade, "text-align:center;font-size:12pt;vertical-align:middle;"));
                }
                else
                    sbObj.Append(AddCell(string.Empty));
            }
            );
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to set subject marks.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        private string SetSubjectMarks(int aiStudentId)
        {
            StringBuilder sbObj = new StringBuilder();
            int iCnt = 0;
            var oGroupSubjectIds = Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
            Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                var oMarks = this.mlstStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
                if (oMarks != null)
                {
                    if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
                        sbObj.Append(AddCell(oMarks.ScoredMarks.ToString(), "text-align:center;vertical-align:middle;"));
                    else
                        sbObj.Append(AddCell(oMarks.ExamStatus, "text-align:center;vertical-align:middle;"));

                    if (BasicInfo.ShowGrades)
                    {
                        if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
                            sbObj.Append(AddCell(oMarks.Grade, "text-align:center;vertical-align:middle;"));
                        else
                            sbObj.Append(AddCell(oMarks.ExamStatus, "text-align:center;vertical-align:middle;"));
                    }
                }
                else
                    sbObj.Append(AddCell(string.Empty));

                if (oGroupSubjectIds.Contains(sb.SubjectId))
                {
                    iCnt++;

                    if (iCnt == 2)
                    {
                        var oGroupMarks = this.mlstStudentMarkDetails.Where(st => st.StudentId == aiStudentId).ToList();
                        string sParentSubejct = Subjects.Where(sbs => sbs.SubjectId == sb.SubjectId).Select(sbs => sbs.ParentSubject).FirstOrDefault();
                        var oTotalMk = Subjects.Where(sbs => sbs.ParentSubject == sParentSubejct).Select(sbs => sbs.SubjectId).ToList();

                        var s = (from mm in oGroupMarks
                                 join tt in oTotalMk
                                 on mm.SubjectId equals tt
                                 select mm).ToList();
                        var sTotalMarks = s.Sum(sbs => sbs.ScoredMarks);

                        sbObj.Append(AddCell(sTotalMarks.ToString(), "text-align:center;vertical-align:middle;"));
                        iCnt = 0;
                    }
                }
            }
            );
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to add out of marks.
        /// </summary>
        /// <param name="abIsCoCurriSubject"></param>
        /// <returns></returns>
        private string AddOutOfMarks(bool abIsCoCurriSubject)
        {
            StringBuilder sbObj = new StringBuilder();
            var oGroupSubjectIds = Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
            var oMaxMarks = this.mlstStudentMarkDetails.GroupBy(sm => sm.SubjectId).Select(sm => new { SubjectId = sm.Key, OutOFMarks = sm.Max(smd => smd.OutOfMarks) });
            int iCounter = 0;
            Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                (
                    sb =>
                    {
                        var oMarks = oMaxMarks.Where(mx => mx.SubjectId == sb.SubjectId).FirstOrDefault();
                        if (oMarks != null)
                        {
                            sbObj.Append(AddCell("&nbsp;&nbsp;" + oMarks.OutOFMarks.ToString() + "&nbsp;&nbsp;", "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));

                            if (BasicInfo.ShowGrades)
                                sbObj.Append(AddCell("&nbsp;&nbsp;G&nbsp;&nbsp;", "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                        }
                        else
                            sbObj.Append(AddCell(string.Empty, "background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));

                        if (oGroupSubjectIds.Contains(sb.SubjectId))
                        {
                            iCounter++;

                            if (iCounter == 2)
                            {
                                string sParentSubejct = Subjects.Where(sbs => sbs.SubjectId == sb.SubjectId).Select(sbs => sbs.ParentSubject).FirstOrDefault();
                                var oTotal = Subjects.Where(sbs => sbs.ParentSubject == sParentSubejct).Select(sbs => sbs.SubjectId).ToList();

                                var s = (from mm in oMaxMarks
                                         join tt in oTotal
                                         on mm.SubjectId equals tt
                                         select mm).ToList();
                                var sTotalMarks = s.Sum(sbs => sbs.OutOFMarks);

                                sbObj.Append(AddCell(sTotalMarks.ToString(), "text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;"));
                                iCounter = 0;
                            }
                        }
                    }
                );
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to add subjects.
        /// </summary>
        /// <param name="abShowCoCurriSubject"></param>
        /// <returns></returns>
        private string AddSubjects(bool abShowCoCurriSubject)
        {
            StringBuilder sbObj = new StringBuilder();

            int iCounter = 0;
            var oGroupSubjectIds = Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
            Subjects.Where(sb => sb.IsCoCurricularSubject == abShowCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                (
                    sb =>
                    {
                        if (oGroupSubjectIds.Count == 0)
                        {
                            if (BasicInfo.ShowGrades)
                                sbObj.Append(AddCell("&nbsp;" + sb.SubjectName + "&nbsp;", "width:130px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;", 2));
                            else
                                sbObj.Append(AddCell("&nbsp;" + sb.SubjectName + "&nbsp;", "width:130px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;", 1));
                        }
                        else
                        {
                            if (sb.ParentSubject != string.Empty)
                            {
                                if (BasicInfo.ShowGrades)
                                    sbObj.Append(AddCell("&nbsp;" + sb.SubjectName + "&nbsp;", "width:130px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;", 2));
                                else
                                    sbObj.Append(AddCell("&nbsp;" + sb.SubjectName + "&nbsp;", "width:130px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;", 1));
                            }
                        }

                        if (oGroupSubjectIds.Contains(sb.SubjectId))
                        {
                            iCounter++;

                            if (iCounter == 2)
                            {
                                sbObj.Append(AddCell("TOTAL", "width:130px;text-align:center;background-color:#a6a6a6;font-weight:bold;font-size:12pt;vertical-align:middle;"));
                                iCounter = 0;
                            }
                        }
                    }
                );
            return sbObj.ToString();
        }

        /// <summary>
        /// This method is used to add new cell.
        /// </summary>
        /// <param name="asData"></param>
        /// <param name="asStyle"></param>
        /// <param name="aiColSpan"></param>
        /// <param name="aiRowSpan"></param>
        /// <param name="asControlString"></param>
        /// <returns></returns>
        private string AddCell(string asData, string asStyle = "", int aiColSpan = 1, int aiRowSpan = 1, string asControlString = "")
        {
            string sStyle = string.Empty;
            if (asStyle != string.Empty)
                sStyle = "style='" + asStyle + "'";

            StringBuilder obj = new StringBuilder();
            obj.Append("<TD colspan='" + aiColSpan + "' rowspan='" + aiRowSpan + "'" + sStyle + ">");
            obj.Append(asData);

            if (asControlString != string.Empty)
                obj.Append(asControlString);

            obj.Append("</TD>");
            return obj.ToString();
        }

        #endregion
    }   
}
