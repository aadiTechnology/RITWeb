using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class AskMeQuestionMasterBL
    {
        #region Data Member(s)
        
        AskMeQuestionMasterDC moAskMeQuestionMasterDC; 

        #endregion

        #region Constructor(s)
        
        public AskMeQuestionMasterBL()
        {
            moAskMeQuestionMasterDC = new AskMeQuestionMasterDC();
        }

        public AskMeQuestionMasterBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moAskMeQuestionMasterDC = new AskMeQuestionMasterDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Public Method(s)
      
        /// <summary>
        /// This method is used to return all available questions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStatusId"></param>
        /// <param name="aiLoginUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="abOnlyShowPublishedQueries"></param>
        /// <returns></returns>
        public static List<AskMeQuestionMaster> GetAllQuestions(int aiSchoolId, int aiAcademicYearId, int aiStatusId, int aiLoginUserId, string asSortExpression, string asSortDirection, int aiStartIndex, int aiEndIndex, bool abOnlyShowPublishedQueries, string asFilter, string asCategories)
        {
            return AskMeQuestionMasterDC.GetAllQuestions(aiSchoolId, aiAcademicYearId, aiStatusId, aiLoginUserId, asSortExpression, asSortDirection, aiStartIndex, aiEndIndex, abOnlyShowPublishedQueries, asFilter, asCategories);
        }

        /// <summary>
        /// This method is used to return uestion count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStatusId"></param>
        /// <param name="aiLoginUserId"></param>
        /// <param name="abOnlyShowPublishedQueries"></param>
        /// <returns></returns>
        //public static int GetCountOfQuestionDetails(int aiSchoolId, int aiAcademicYearId, int aiStatusId, int aiLoginUserId, bool abOnlyShowPublishedQueries, string asFilter, string asCategories)
        //{
        //    return AskMeQuestionMasterDC.GetCountOfQuestionDetails(aiSchoolId, aiAcademicYearId, aiStatusId, aiLoginUserId, abOnlyShowPublishedQueries, asFilter, asCategories);
        //}

        /// <summary>
        /// This method is used to return all communications of given question id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public static List<AskMeQuestionDetails> GetAllQuestionCommunications(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, string asSortExpression, string asSortDirection, int aiStartIndex, int aiEndIndex, int aiLoginUserId, bool abShowOnPublishedQuery = false)
        {
            return AskMeQuestionMasterDC.GetAllQuestionCommunications(aiSchoolId, aiAcademicYearId, aiQuestionId, asSortExpression, asSortDirection, aiStartIndex, aiEndIndex, aiLoginUserId, abShowOnPublishedQuery);
        }

        /// <summary>
        /// This method is used to return count of communications of given question id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public static int GetCountOfQuestionCommunications(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiLoginUserId)
        {
            return AskMeQuestionMasterDC.GetCountOfQuestionCommunications(aiSchoolId, aiAcademicYearId, aiQuestionId, aiLoginUserId);
        }

        /// <summary>
        /// This method is used to return all statuses.
        /// </summary>
        /// <returns></returns>
        public List<AskMeStatusMaster> GetAllStatuses()
        {
            return moAskMeQuestionMasterDC.GetAllStatuses();
        }

        /// <summary>
        /// This method is used to delete communication.
        /// </summary>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiUpdatedById"></param>
        public static void DeleteQuestionDetails(int aiQuestionDetailsId, int aiUpdatedById)
        {
            AskMeQuestionMasterDC.DeleteQuestionDetails(aiQuestionDetailsId, aiUpdatedById);
        }

        /// <summary>
        /// This method is used to return communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public static AskMeQuestionMaster GetQuestionDetails(int aiSchoolId, int aiAcademicYearId, int aiQuestionDetailsId, int aiQuestionId, int aiLoginUserId)
        {
            return AskMeQuestionMasterDC.GetQuestionDetails(aiSchoolId, aiAcademicYearId, aiQuestionDetailsId, aiQuestionId, aiLoginUserId);
        }

        /// <summary>
        /// This method is used to save communication details.
        /// </summary>
        /// <param name="aoAskMeQuestionMaster"></param>
        public void SaveCommunicationDetails(AskMeQuestionMaster aoAskMeQuestionMaster)
        {
            moAskMeQuestionMasterDC.SaveCommunicationDetails(aoAskMeQuestionMaster);
        }

  /// <summary>
        /// This method is used to get details about ask me communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public static List<AskMeCommunicationDetails> GetAskMeCommunication(int aiSchoolId, int aiAcademicYearId, int aiQuestionId)
        {
            return AskMeQuestionMasterDC.GetAskMeCommunication(aiSchoolId, aiAcademicYearId, aiQuestionId);
        }
        /// <summary>
        /// This method is used to publish communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsPublish"></param>
        public static void PublishCommunication(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiUpdatedById, bool abIsPublish)
        {
            AskMeQuestionMasterDC.PublishCommunication(aiSchoolId, aiAcademicYearId, aiQuestionId, aiUpdatedById, abIsPublish);
        } 

        /// <summary>
 /// This method is used to publish communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsPublish"></param>
        public  void PublishCommunicationDetails(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiUpdatedById, bool abIsPublish)
        {
            AskMeQuestionMasterDC.PublishCommunication(aiSchoolId, aiAcademicYearId, aiQuestionId, aiUpdatedById, abIsPublish);
        } 

        /// <summary>
        /// This method is used to submit communications.
        /// </summary>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsSubmitted"></param>
        public static void SubmitCommunication(int aiSchoolId, int aiQuestionDetailsId, int aiUpdatedById, bool abIsSubmitted)
        {
            AskMeQuestionMasterDC.SubmitCommunication(aiSchoolId, aiQuestionDetailsId, aiUpdatedById, abIsSubmitted);
        }

        /// <summary>
        /// This method is used to mark query as invalid.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsInvalid"></param>
        public void MarkValidityStatus(int aiQuestionId, int aiSchoolId, int aiAcademicYearId, int aiUpdatedById, bool abIsInvalidAction)
        {
            AskMeQuestionMasterDC.MarkValidityStatus(aiQuestionId, aiSchoolId, aiAcademicYearId, aiUpdatedById, abIsInvalidAction);
        }

        /// <summary>
        /// This method is used to return all categories.
        /// </summary>
        /// <returns></returns>
        public List<AskMeCategory> GetAllCategories()
        {
            return moAskMeQuestionMasterDC.GetAllCategories();
        }

        /// <summary>
        /// This method is used to set owner assignment.
        /// </summary>
        /// <param name="aoAskMeOwnerAssignment"></param>
        public void SetOwnerAssignment(AskMeOwnerAssignment aoAskMeOwnerAssignment)
        {
            moAskMeQuestionMasterDC.SetOwnerAssignment(aoAskMeOwnerAssignment);
        }

        #endregion

        public static void AssignCommunication(int aiSchoolId, int aiQuestionId, int aiUpdatedById, bool abIsForward, int aiAcademicYearId)
        {
            AskMeQuestionMasterDC.AssignCommunication(aiSchoolId, aiQuestionId, aiUpdatedById, abIsForward, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to Get Teacher to Set As a Subject Expert For Ask ME.
        /// </summary>
        /// <param name="aoAskMeQuestionMaster"></param>
        public List<SubjectExperts> GetSubjectExperts(int aiSubjectId)
        {
            List<SubjectExperts> lstSubjectExperts = moAskMeQuestionMasterDC.GetSubjectExperts(aiSubjectId);
            return lstSubjectExperts;
        }

        /// <summary>
        /// This method is used to Save Teacher as a Subject Expert For Ask ME.
        /// </summary>
        /// <param name="aoAskMeQuestionMaster"></param>
        public void SaveSubjectExperts(int aiSchoolId,int aiAcademicYearId, int aiSubjectId, string asTeacherIds)
        {
            moAskMeQuestionMasterDC.SaveSubjectExperts(aiSchoolId,aiAcademicYearId,aiSubjectId, asTeacherIds);
        }

        /// <summary>
        /// This method is used to get the Count of Unread Question to Display on Contrrol Panel.
        /// </summary>
        /// <param name="aiUserId"></param>
        public int GetCountOfUnreadQuestion(int aiUserId)
        {
            return moAskMeQuestionMasterDC.GetCountOfUnreadQuestion(aiUserId);
        }

        /// <summary>
        /// This method is used to get the Read Receipt Details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiQuestionId"></param>
        /// <pparam name="aiAcademicYearId"></param>
        public List<AskMeReadReceiptDetails> GetReadReceiptDetails(int aiSchoolId, int aiQuestionId, int aiAcademicYearId, int aiLoginUserId)
        {
            List<AskMeReadReceiptDetails> lstAskMeReadReceiptDetails = moAskMeQuestionMasterDC.GetReadReceiptDetails(aiSchoolId, aiQuestionId, aiAcademicYearId, aiLoginUserId);
            return lstAskMeReadReceiptDetails;
        }

        /// <summary>
        /// This method is used to return all owners.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public List<AskMeOwnerAssignment> GetAllOwners(int aiQuestionId)
        {
            return moAskMeQuestionMasterDC.GetAllOwners(aiQuestionId);
        }

        /// <summary>
        /// This method is used to return subject teachers.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public List<AskMeOwnerAssignment> GetAllSubjectTeachers(int aiUserRoleId)
        {
            return moAskMeQuestionMasterDC.GetAllSubjectTeachers(aiUserRoleId);
        }

        /// <summary>
        /// This method is used to submit owner assignment.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <param name="abIsSubmit"></param>
        public void SubmitOwnerAssignment(int aiQuestionId, bool abIsSubmit)
        {
            moAskMeQuestionMasterDC.SubmitOwnerAssignment(aiQuestionId, abIsSubmit);
        }

        /// <summary>
        /// This method is used to delete owner assignment.
        /// </summary>
        /// <param name="aiId"></param>
        public void DeleteOwnerAssignment(int aiId)
        {
            moAskMeQuestionMasterDC.DeleteOwnerAssignment(aiId);
        }

        /// <summary>
        /// This method is used to save communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSelectedQuestionIds"></param>
        /// <param name="aiMasterQuestionId"></param>
        /// <param name="aiUserId"></param>
        public static void SaveSelection(int aiSchoolId, string asSelectedQuestionIds, int aiMasterQuestionId, int aiUserId)
        {
            AskMeQuestionMasterDC.SaveSelection(aiSchoolId, asSelectedQuestionIds, aiMasterQuestionId, aiUserId);
        }

    }
}
