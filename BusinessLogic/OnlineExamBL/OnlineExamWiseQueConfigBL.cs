using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using System.Data;
using SchoolEntities.Admin;
namespace BusinessLogic
{
    public class OnlineExamWiseQueConfigBL
    {

        #region Data members

        private OnlineExamWiseQueConfigDC moOnlineExamWiseQueConfigDC;

        #endregion
        #region Constructors

        public OnlineExamWiseQueConfigBL()
        {
            this.moOnlineExamWiseQueConfigDC = new OnlineExamWiseQueConfigDC();
        }


        public OnlineExamWiseQueConfigBL(int aiSchoolId, int aiAcademicYearId)
        {
            moOnlineExamWiseQueConfigDC = new OnlineExamWiseQueConfigDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        public List<AnswerDetails> AnswerDetails
        {
            get { return moOnlineExamWiseQueConfigDC.AnswerDetails; }
        }

        public DataTable GetAllTestsForClass(int aiStandardDivId)
        {
            return moOnlineExamWiseQueConfigDC.GetAllTestsForClass(aiStandardDivId);
        }
        public DataTable GetAssociatedStandards(int aiSchoolId, int aiAcademicYearId)
        {
            return moOnlineExamWiseQueConfigDC.GetAssociatedStandards(aiSchoolId, aiAcademicYearId);
        }
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects()
        {
            return moOnlineExamWiseQueConfigDC.GetAllYearwiseSubjects();
        }

        public OnlineExamConfiguration OnlineExamConfiguration
        {
            get { return moOnlineExamWiseQueConfigDC.OnlineExamConfiguration; }
        }


        public void Save(string asStaffXML, OnlineExamWiseQueConfig oExamConfig)
        {
            moOnlineExamWiseQueConfigDC.Save(asStaffXML, oExamConfig);
        }

        public void SaveExamQuestion(string asQuestionXML, OnlineExamQuestConfig oExamConfig)  
        {
            moOnlineExamWiseQueConfigDC.SaveExamQuestion(asQuestionXML, oExamConfig);
        }

        public List<OnlineExamQuestConfig> GetAll(int aiSchoolId)  
        {
            return this.moOnlineExamWiseQueConfigDC.GetAll(aiSchoolId);
        }


        /// <summary>
        /// This method is used to return additional payment object.
        /// </summary>
        /// <param name="aiPaymentId"></param>
        /// <returns></returns>
        public List<OnlineExamQuestConfig> Get(int aiId, int aiSchoolid)  
        {
            return moOnlineExamWiseQueConfigDC.Get(aiId, aiSchoolid);
        }

        public static DataTable GetAllQuestions(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiSubjectId)
        {
            return OnlineExamWiseQueConfigDC.GetAllQuestions(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, aiSubjectId);
        }

        public static DataTable GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId)
        {

            return OnlineExamWiseQueConfigDC.GetAllExamQuestionConfiguration(aiSchoolId, aiAcademicYearId);
        }
        //public List<OnlineExamWiseQueConfig> GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, String sortExpression, int startRowIndex, int maximumRows, int aiStandardDivisionId, int aiSubjectId)
        //{
        //    int iStartIndex = startRowIndex;
        //    int iEndIndex = iStartIndex + maximumRows;
        //    return moOnlineExamWiseQueConfigDC.GetAllExamQuestionConfiguration(aiSchoolId, aiAcademicYearId, sortExpression, iEndIndex, startRowIndex, aiStandardDivisionId, aiSubjectId);
        //}
        //public static int CountTotalExamQuestionConfiguration(Int32 aiSchoolId, Int32 aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiStandardDivisionId, int aiSubjectId)
        //{
        //    return OnlineExamWiseQueConfigDC.CountTotalExamQuestionConfiguration(aiSchoolId, aiAcademicYearId, sortExpression, maximumRows, startRowIndex, aiStandardDivisionId, aiSubjectId);
        //}

        public static DataSet GetDetailsForUpdateQuestions(int aiVehicleId, int aiSchoolID, int aiAcademicYearId)
        {
            return OnlineExamWiseQueConfigDC.GetDetailsForUpdateQuestions(aiVehicleId, aiSchoolID, aiAcademicYearId);
        }

        public List<OnlineExamWiseQueConfig> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiExamId, int aiSubjectId)
        {
            List<OnlineExamWiseQueConfig> lstVendorDetails = new List<OnlineExamWiseQueConfig>();

            lstVendorDetails = this.moOnlineExamWiseQueConfigDC.GetAllExamQuestionConfiguration(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, aiExamId, aiSubjectId);

            return lstVendorDetails;
        }
        public DataTable DeleteExamDetails(int aiVehicleId, int aiSchoolID, int aiAcademicYearId, out int aiRowCount)
        {
            return moOnlineExamWiseQueConfigDC.DeleteExamDetails(aiVehicleId, aiSchoolID, aiAcademicYearId, out aiRowCount);
        }

        /// <summary>
        /// This method is used to get Questions for online exam.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<QuestionDetails> GetQuestionsForOnlineExam(int aiStandardId, int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiStudentId)
        {
            return moOnlineExamWiseQueConfigDC.GetQuestionsForOnlineExam(aiStandardId, aiStandardDivisionId, aiSubjectId, aiExamId, aiStudentId);
        }

        /// <summary>
        /// This method is used to get question wise answer details for exam.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        //public List<AnswerDetails> GetQuestionWiseAnswersForOnlineExam(int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiQuestionId, int aiStudentId) 
        //{
        //    return moOnlineExamWiseQueConfigDC.GetQuestionWiseAnswersForOnlineExam(aiStandardDivisionId, aiSubjectId, aiExamId, aiQuestionId, aiStudentId);
        //}

        /// <summary>
        /// This method is used to save student online exam details.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asQuestAnswerDetails"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiTotalMarks"></param>
        /// <param name="aiOutOfMarks"></param>
        public void SaveStudentQuestionAnswerDetails(int iStandardId, int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiStudentId, string asQuestAnswerDetails, int aiInsertedById, int aiTotalMarks, int aiOutOfMarks)
        {
            moOnlineExamWiseQueConfigDC.SaveStudentQuestionAnswerDetails(iStandardId, aiStandardDivisionId, aiSubjectId, aiExamId, aiStudentId, asQuestAnswerDetails, aiInsertedById, aiTotalMarks, aiOutOfMarks);
        }

        /// <summary>
        /// This method is used to Submit the Online exam.
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        public void SubmitStudentOnlineExam(int aiStdId, int aiStdDivId, int aiSubjectId, int aiExamId, int aiStudentId)
        {
            moOnlineExamWiseQueConfigDC.SubmitStudentOnlineExam(aiStdId,aiStdDivId, aiSubjectId, aiExamId, aiStudentId);
        }

        public static DataTable GetAllSubjectsForExam(int aiSchoolId, int aiAcademicYearId, int aiExamId, int aiStudentIUd)
        {
            return OnlineExamWiseQueConfigDC.GetAllSubjectsForExam(aiSchoolId, aiAcademicYearId, aiExamId, aiStudentIUd);
        }

        public static DataTable GetAllSubjectDetailsOfExam(int aiSchoolId, int aiAcademicYearId, int aiId)
        {
            return OnlineExamWiseQueConfigDC.GetAllSubjectDetailsOfExam(aiSchoolId, aiAcademicYearId, aiId);
        }

        public DataTable GetAllStudentList(int aiStdDivId, int aiSubjectId)
        {

            return moOnlineExamWiseQueConfigDC.GetAllStudentList(aiStdDivId, aiSubjectId);
        }

        public DataTable GetAllStudentQuestionList(int aistudentid, int aiSubjectId)
        {

            return moOnlineExamWiseQueConfigDC.GetAllStudentQuestionList(aistudentid, aiSubjectId);
        }
        
        public void SaveStudentsQuestionMarks(string sStudentMarkDetails, int aiUpdatedById)
        {
            moOnlineExamWiseQueConfigDC.SaveStudentsQuestionMarks(sStudentMarkDetails, aiUpdatedById);
        }
    }
}
