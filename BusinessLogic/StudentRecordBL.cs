using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{   
    public class StudentRecordBL
    {
        #region Data Member(s)
        
        private StudentRecordDC moStudentRecordDC;
        private int miRecordCount; 

        #endregion

        #region Constructor(s)

        public StudentRecordBL()
        {
            this.moStudentRecordDC = new StudentRecordDC();
        }

        public StudentRecordBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moStudentRecordDC = new StudentRecordDC(aiSchoolId, aiAcademicYearId, aiUserId);
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to return student record details.
        /// </summary>
        /// <param name="aiSchoowiseStudentId"></param>
        /// <param name="abIsReadMode"></param>
        /// <returns></returns>
        public StudentDataCollction GetAllStudentRecords(int aiSchoowiseStudentId, bool abIsReadMode)
        {
            return this.moStudentRecordDC.GetAllStudentRecords(aiSchoowiseStudentId, abIsReadMode);
        }

        /// <summary>
        /// This method is used to save student record.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="asData"></param>
        public void Save(int aiStudentId, string asData, DateTime adtDate)
        {
            this.moStudentRecordDC.Save(aiStudentId, asData, adtDate);
        }

        /// <summary>
        /// This method is used to get comment details.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <returns></returns>
        public StudentRecordComment GetCommentDetails(int aiSchoolwiseStudentId, int aiCommentId)
        {
            return this.moStudentRecordDC.GetCommentDetails(aiSchoolwiseStudentId, aiCommentId);
        }

        /// <summary>
        /// This method is used to save comment.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <param name="aoStudentRecordCommnet"></param>
        /// <param name="abAllowSubmit"></param>
        public void SaveComment(int aiSchoolwiseStudentId, int aiCommentId, StudentRecordComment aoStudentRecordCommnet, bool abAllowSubmit, string asStdDivId)
        {
            int aiStdDivId = Convert.ToInt32(asStdDivId);
            this.moStudentRecordDC.SaveComment(aiSchoolwiseStudentId, aiCommentId, aoStudentRecordCommnet, abAllowSubmit, aiStdDivId);
        }

        /// <summary>
        /// This method is used to delete comment
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        public void DeleteComment(int aiSchoolwiseStudentId, int aiCommentId)
        {
            this.moStudentRecordDC.DeleteComment(aiSchoolwiseStudentId, aiCommentId);
        }

        /// <summary>
        /// This method is used to submit comment.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <param name="abSubmitAllComments"></param>
        public void Submit(int aiSchoolwiseStudentId, int aiCommentId, bool abSubmitAllComments)
        {
            this.moStudentRecordDC.Submit(aiSchoolwiseStudentId, aiCommentId, abSubmitAllComments);
        }

        /// <summary>
        /// This method is used to  return status.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asShowSaved"></param>
        /// <param name="asHasEditAccess"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<StudentRecordStatus> GetAllStudentStatus(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, string asIncludeRiseAndShinde, string asFilter, string asShowSaved, string asHasEditAccess, int aiUserId, string asSortExpression, string asSortDirection, int startRowIndex, int maximumRows)
        {
            if (asFilter == null)
                asFilter = string.Empty;

            bool sShowSaved = asShowSaved == "1" ? true : false;
            bool abIncludeRiseAndShinde = asIncludeRiseAndShinde == "True" ? true : false;

            asSortExpression = asSortExpression.ToLower().Replace(Utility.Constants.S_ASCENDING, string.Empty).Replace(Utility.Constants.S_DESCENDING, string.Empty).Trim();

            if (asSortDirection == null)
                asSortDirection = "ASC";

            int iEndIndex = startRowIndex + maximumRows;
            List<StudentRecordStatus> lstStudentRecordStatus = this.moStudentRecordDC.GetAllStudentStatus(aiSchoolId, aiAcademicYearId, aiStdDivId, asFilter, sShowSaved,abIncludeRiseAndShinde, asHasEditAccess, aiUserId, asSortExpression, asSortDirection, startRowIndex, iEndIndex);
            if (lstStudentRecordStatus.Count > 0)
                this.miRecordCount = lstStudentRecordStatus[0].TotalRows;
            else
                this.miRecordCount = 0;

            return lstStudentRecordStatus;
        }

        /// <summary>
        /// This method is used to return count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asShowSaved"></param>
        /// <param name="asHasEditAccess"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public int GetAllStudentCount(int aiSchoolId, int aiAcademicYearId, int aiStdDivId,string asIncludeRiseAndShinde, string asFilter, string asShowSaved, string asHasEditAccess, int aiUserId, string asSortExpression, string asSortDirection, int startRowIndex, int maximumRows)
        {
            return this.miRecordCount;
        }

        /// <summary>
        /// This method is used to return teacher list.
        /// </summary>
        /// <param name="abHasFullAccess"></param>
        /// <returns></returns>
        public Tuple<bool, bool, int, List<AssociatedTeacher>, bool> GetTeacherList(bool abHasFullAccess)
        {
            return this.moStudentRecordDC.GetTeacherList(abHasFullAccess);
        }

        /// <summary>
        /// This method is used to mark comment as read.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        public void MarkAsRead(int aiSchoolwiseStudentId)
        {
            this.moStudentRecordDC.MarkAsRead(aiSchoolwiseStudentId);
        } 

        #endregion
    }
}
