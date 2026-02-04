using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using Utility;
using ProgressReportEntities;

namespace BusinessLogic
{
    public class StudentSubjectMarksBL : BusinessLogicBaseBL
    {
        #region DataMembers & Properties

        #region DataMembers

        StudentSubjectMarksDC moStudentSubjectMarksDC;
        StudentSubjectMarksDC.StudentSubjectMarksStruct moStudentSubjectMarksStruct;

        #endregion

        #region Properties

        #region

        public Int32 TestWiseSubjectMarksId
        {
            get
            {
                return moStudentSubjectMarksStruct.miTestWiseSubjectMarksId;
            }
            set
            {
                moStudentSubjectMarksStruct.miTestWiseSubjectMarksId = value;
            }
        }

        public string StudentDetails
        {
            get
            {
                return moStudentSubjectMarksStruct.msStudentDetails;
            }
            set
            {
                moStudentSubjectMarksStruct.msStudentDetails = value;
            }
        }

        public string StudentMarkDetails
        {
            get
            {
                return moStudentSubjectMarksStruct.msStudentMarkDetails;
            }
            set
            {
                moStudentSubjectMarksStruct.msStudentMarkDetails = value;
            }
        }

        public string StudentTestSubmitStatus
        {
            get
            {
                return moStudentSubjectMarksStruct.msStudentTestSubmitStatus;
            }
            set
            {
                moStudentSubjectMarksStruct.msStudentTestSubmitStatus = value;
            }
        }

        public Int32 InsertedBYId
        {
            get
            {
                return moStudentSubjectMarksStruct.miInsertedByid;
            }
            set
            {
                moStudentSubjectMarksStruct.miInsertedByid = value;
            }
        }

        public string TestIds
        {
            get
            {
                return moStudentSubjectMarksDC.msTestIds;
            }
            set
            {
                moStudentSubjectMarksDC.msTestIds = value;
            }
        }

        public int StudentCount
        {
            get
            {
                return moStudentSubjectMarksDC.miStudentMarksTransferCount;
            }
            set
            {
                moStudentSubjectMarksDC.miStudentMarksTransferCount = value;
            }
        }

        public string RemarkXml
        {
            get { return moStudentSubjectMarksStruct.msRemarkXml; }
            set { moStudentSubjectMarksStruct.msRemarkXml = value; }
        }

        public bool HasRemarks
        {
            get { return moStudentSubjectMarksStruct.mbHasRemarks; }
            set { moStudentSubjectMarksStruct.mbHasRemarks = value; }
        }

        public int TestId
        {
            get { return moStudentSubjectMarksStruct.miTestId; }
            set { moStudentSubjectMarksStruct.miTestId = value; }
        }

        public int SubjectId
        {
            get { return moStudentSubjectMarksStruct.miSubjectId; }
            set { moStudentSubjectMarksStruct.miSubjectId = value; }
        }
        
        
        #endregion



        #endregion

        #endregion

        #region Constructors

        /// <summary>
        ///  Default constructor
        /// </summary>
        public StudentSubjectMarksBL()
        {
            moStudentSubjectMarksDC = new StudentSubjectMarksDC();
        }

        public StudentSubjectMarksBL(int aiSchoolId, int aiAcademicYearId)
        {
            moStudentSubjectMarksDC = new StudentSubjectMarksDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        #region Public Methods

        public void ManageStudentTestMarks(String asRemoveProgress, int aiSchoolId, int aiAcademicYearId)
        {
            moStudentSubjectMarksDC.StudentSubjectMarksStructDetails = moStudentSubjectMarksStruct;
            moStudentSubjectMarksDC.ManageStudentTestMarks(asRemoveProgress, aiSchoolId, aiAcademicYearId);
        }

        public void ManageTestWiseStudentMarks(String asRemoveProgress, string asMode, int aiStandardDivisionId, string asRoundMarksAtSubjectLevel)
        {
            moStudentSubjectMarksDC.StudentSubjectMarksStructDetails = moStudentSubjectMarksStruct;
            moStudentSubjectMarksDC.ManageTestWiseStudentMarks(asRemoveProgress, asMode, aiStandardDivisionId, asRoundMarksAtSubjectLevel);
        }

        public static void UpdateStudentTestMarks(int aiUpdated_By_id, string asStudentMarkDetails, char acUseAvarageFinalResult)
        {
            StudentSubjectMarksDC.UpdateStudentTestMarks(aiUpdated_By_id, asStudentMarkDetails, acUseAvarageFinalResult);
        }

        public static void UpdatePrePrimaryTestMarks(string asStudentMarkDetails, string sTestComment, int aiStudentId, int aiTestId, int aiUpdated_By_id)
        {
            StudentSubjectMarksDC.UpdatePrePrimaryTestMarks(asStudentMarkDetails, sTestComment, aiStudentId, aiTestId, aiUpdated_By_id);
        }

        public DataSet GetAllRelatedInformation(int aiSchoolId, int aiAcademicYrId, int aiSubjectId, int aiTestId, int aiStandardDivisionId)
        {
            return moStudentSubjectMarksDC.GetAllRelatedInformation(aiSchoolId, aiAcademicYrId, aiSubjectId, aiTestId, aiStandardDivisionId);
        }

        public StudentProgressReport GetStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId, int iUserId)
        {
            StudentProgressReport oStudentProgressReport = moStudentSubjectMarksDC.GetStudentProgressResult(aiSchoolId, aiAcademicYrId, aiStudentId, iUserId);
            if (oStudentProgressReport.StudentDetails.StudentName.IsNullOrEmpty() || oStudentProgressReport.SubjectDetails.Count <= 0)
                throw new Exceptions.MarksNotAvailableForResult(oStudentProgressReport.StudentDetails.YearWiseStudentId.ToString());
            return oStudentProgressReport;
        }

        public StudentProgressReport GetStudentTestProgressResult(int iSchoolID, int iAcademicYrID, int iStudentId, int iTestId)
        {
            StudentProgressReport oStudentProgressReport = moStudentSubjectMarksDC.GetStudentTestProgressResult(iSchoolID, iAcademicYrID, iStudentId, iTestId);
            if ((oStudentProgressReport.StudentDetails.StudentName.IsNullOrEmpty() || oStudentProgressReport.SubjectDetails.Count <= 0) && (TestIds.IsNullOrEmpty() || oStudentProgressReport.SubjectDetails.Count <= 0))
                throw new Exceptions.MarksNotAvailableForResult(oStudentProgressReport.StudentDetails.YearWiseStudentId.ToString());
            return oStudentProgressReport;
        }

		public DataSet GetAllStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId)
        {
			return moStudentSubjectMarksDC.GetAllStudentProgressResult(aiSchoolId, aiAcademicYrId, aiStdDivId);
        }

		public DataSet GetAllStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId, int aiStartIndex, int aiPageCount, int aiTestID)
        {
			return moStudentSubjectMarksDC.GetAllStudentProgressResult(aiSchoolId, aiAcademicYrId, aiStdDivId, aiStartIndex, aiPageCount, aiTestID);
        }

        public StudentProgressReport GetStudentResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            StudentProgressReport oStudentProgressReport = moStudentSubjectMarksDC.GetStudentGraceResult(aiSchoolId, aiAcademicYrId, aiStudentId);
            if (oStudentProgressReport.SubjectDetails.Count == Constants.I_ZERO)
                throw new Exceptions.NoResultFound("Result not generated for this student : " + oStudentProgressReport.StudentDetails.RollNo + " - " + oStudentProgressReport.StudentDetails.StudentName);
            return oStudentProgressReport;
        }

        public StudentProgressReport GetStudentGraceResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            StudentProgressReport oStudentProgressReport = moStudentSubjectMarksDC.GetStudentResult(aiSchoolId, aiAcademicYrId, aiStudentId);
            if (oStudentProgressReport.SubjectDetails.Count < 1)
                throw new Exceptions.NoResultFound("Result not generated for this student : " + oStudentProgressReport.StudentDetails.RollNo + " - " + oStudentProgressReport.StudentDetails.StudentName);
            return oStudentProgressReport;
        }


        public DataSet GenerateAllStudentsResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId, int aiUserId, char acUseAvarageFinalResult)
        {
            return moStudentSubjectMarksDC.GenerateAllStudentsResult(aiSchoolId, aiAcademicYrId, aiStdDivId, aiUserId, acUseAvarageFinalResult);
        }

        public static void UpdateAnnualResultGraceMarks(int iStudentId, int aiUpdated_By_id, string xmlStudentMarkDetails)
        {
            StudentSubjectMarksDC.UpdateAnnualResultGraceMarks(iStudentId, aiUpdated_By_id, xmlStudentMarkDetails);
        }

        public StudentProgressReport GetMarksDetailsForExamwiseStudentMarksAssignment(int aiSchoolID, int aiAcademicYrID, int aiStudentId, int aiUserId)
        {
            return moStudentSubjectMarksDC.GetMarksDetailsForExamwiseStudentMarksAssignment(aiSchoolID, aiAcademicYrID, aiStudentId, aiUserId);
        }

        /// <summary>
        /// This method is used to get students list for transfering the optinal subject marks.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="asName"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<TransferStudentSubjectsMarkDetails> GetStudentsToTransferMarks(int aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId, string asName, int maximumRows, int startRowIndex)
        {
            List<TransferStudentSubjectsMarkDetails> lstAllTransferStudentSubjectsMarkDetails = moStudentSubjectMarksDC.GetStudentsToTransferMarks(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, asName, startRowIndex + maximumRows, startRowIndex);
            List<TransferStudentSubjectsMarkDetails> lstTransferStudentSubjectsMarkDetails = new List<TransferStudentSubjectsMarkDetails>();
            int iStudentId = 0;
            lstAllTransferStudentSubjectsMarkDetails.ForEach(
                                                                StudentSubjectsMark =>
                                                                {
                                                                    if (iStudentId != StudentSubjectsMark.YearwiseStudentId)
                                                                    {
                                                                        lstTransferStudentSubjectsMarkDetails.Add(GetSubjectsMarkDetails(lstAllTransferStudentSubjectsMarkDetails.Where(SubjectMarks => SubjectMarks.YearwiseStudentId == StudentSubjectsMark.YearwiseStudentId).ToList()));
                                                                        iStudentId = StudentSubjectsMark.YearwiseStudentId;
                                                                    }
                                                                }
                                                            );
            return lstTransferStudentSubjectsMarkDetails;
        }

        /// <summary>
        /// This method is used to get a single object for multiple records of single student.
        /// </summary>
        /// <param name="alstTransferStudentSubjectsMarkDetails"></param>
        /// <returns></returns>
        private TransferStudentSubjectsMarkDetails GetSubjectsMarkDetails(List<TransferStudentSubjectsMarkDetails> alstTransferStudentSubjectsMarkDetails)
        {
            TransferStudentSubjectsMarkDetails oTransferStudentSubjectsMarkDetails = new TransferStudentSubjectsMarkDetails();

            if (alstTransferStudentSubjectsMarkDetails.Count > 0)
            {
                alstTransferStudentSubjectsMarkDetails.ForEach
                (
                    StudentSubjectsMark =>
                    {
                        oTransferStudentSubjectsMarkDetails.ClassName = StudentSubjectsMark.ClassName;
                        oTransferStudentSubjectsMarkDetails.RegNo = StudentSubjectsMark.RegNo;
                        oTransferStudentSubjectsMarkDetails.RollNo = StudentSubjectsMark.RollNo;
                        oTransferStudentSubjectsMarkDetails.Standard_Division_Id = StudentSubjectsMark.Standard_Division_Id;
                        oTransferStudentSubjectsMarkDetails.StudentName = StudentSubjectsMark.StudentName;
                        oTransferStudentSubjectsMarkDetails.YearwiseStudentId = StudentSubjectsMark.YearwiseStudentId;
                        
                        if(StudentSubjectsMark.TransferFromSubjectName != null && StudentSubjectsMark.TransferFromSubjectName.Trim() != string.Empty)
                            oTransferStudentSubjectsMarkDetails.TransferFromSubjectName += StudentSubjectsMark.TransferFromSubjectName + " ,";
                    }
                );

                if (oTransferStudentSubjectsMarkDetails.TransferFromSubjectName != null && oTransferStudentSubjectsMarkDetails.TransferFromSubjectName.Trim().StartsWith(","))
                    oTransferStudentSubjectsMarkDetails.TransferFromSubjectName = oTransferStudentSubjectsMarkDetails.TransferFromSubjectName.Trim().Substring(1);

                if (oTransferStudentSubjectsMarkDetails.TransferFromSubjectName != null && oTransferStudentSubjectsMarkDetails.TransferFromSubjectName.Trim().EndsWith(","))
                    oTransferStudentSubjectsMarkDetails.TransferFromSubjectName = oTransferStudentSubjectsMarkDetails.TransferFromSubjectName.Trim().Substring(0, oTransferStudentSubjectsMarkDetails.TransferFromSubjectName.Length - 1);
            }

            return oTransferStudentSubjectsMarkDetails;
        }

        /// <summary>
        /// This method is used to get count of students for transfering the optinal subject marks.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="asName"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int GetStudentsCountToTransferMarks(int aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId, string asName, int maximumRows, int startRowIndex)
        {
            return StudentCount;
        }

        public DataTable Transfer(string asStudentTransferMarksXml, int aiUserId)
        {
            return moStudentSubjectMarksDC.Transfer(asStudentTransferMarksXml, aiUserId);
        }
        #endregion

        public void SaveStudentwiseRemarks(int aiSchoolId, int aiAcademicYearId)
        {
            moStudentSubjectMarksDC.StudentSubjectMarksStructDetails = moStudentSubjectMarksStruct;
            moStudentSubjectMarksDC.SaveStudentwiseRemarks(aiSchoolId, aiAcademicYearId);
        }
    }
}
