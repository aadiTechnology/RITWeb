using System;
using System.Collections.Generic;
using DataCommunicator;
using LessonPlanEntities;
using StaffPerformanceEntity;

namespace BusinessLogic
{
    public class LessonPlanDetailsBL
    {
        #region Data Member(s)

        private LessionPlanDetailsDC moLessionPlanDetailsDC; 

        #endregion

        #region Constructor(s)

        public LessonPlanDetailsBL()
        {
        }

        public LessonPlanDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moLessionPlanDetailsDC = new LessionPlanDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public List<LessonPlanParameters> Parameters
        {
            get
            {
                return this.moLessionPlanDetailsDC.Parameters;
            }
        }

        public List<LessonPlanConfig> PlanConfigs
        {
            get
            {
                return this.moLessionPlanDetailsDC.PlanConfigs;
            }
        }

        public List<LessonPlanReportingConfig> LessonPlanReportingUsers
        {
            get
            {
                return this.moLessionPlanDetailsDC.LessonPlanReportingUsers;
            }
        }

        public List<LessonPlanDetails> LessonPlanDetails
        {
            get
            {
                return this.moLessionPlanDetailsDC.LessonPlanDetails;
            }
        }

        public LessonPlanBasicDetails BasicDetails
        {
            get
            {
                return this.moLessionPlanDetailsDC.BasicDetails;
            }
        }

        public ButtonState ButtonState
        {
            get
            {
                return this.moLessionPlanDetailsDC.ButtonState;
            }
        }

        public LessonPlanStandardDivIds LessonPlanStandard
        {
            get
            {
                return this.moLessionPlanDetailsDC.LessonPlanStandard;
            }
        }

        public List<ApproverComment> ApproverComments
        {
            get { return this.moLessionPlanDetailsDC.ApproverComments; }
        }

        public List<LessonPlanPhrase> LessonPlanPhrases
        {
            get { return this.moLessionPlanDetailsDC.LessonPlanPhrases; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return lesson plan details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static List<LessonPlanConfig> GetAllConfigs(int aiSchoolId, int aiAcademicYearId, int aiReportingUserId, int aiUserId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex, string StartDate, string EndDate)
        {   
            int iEndIndex = startRowIndex + maximumRows;
            return LessionPlanDetailsDC.GetAllConfigs(aiSchoolId, aiAcademicYearId, aiReportingUserId, aiUserId, startRowIndex, iEndIndex, StartDate, EndDate);
        }

        /// <summary>
        /// This method is used to return lesson plan count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static int GetAllConfigsCount(int aiSchoolId, int aiAcademicYearId, int aiReportingUserId, int aiUserId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex, string StartDate, string EndDate)
        {
            return LessionPlanDetailsDC.GetAllConfigsCount(aiSchoolId, aiAcademicYearId, aiReportingUserId, aiUserId, StartDate, EndDate);
        }

        /// <summary>
        /// This method is used to save lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="asLessonPlanXml"></param>
        /// <param name="adtOldStartDate"></param>
        /// <param name="adtOldEndDate"></param>
        public void Save(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, string asLessonPlanXml, DateTime adtOldStartDate, DateTime adtOldEndDate)
        {
            this.moLessionPlanDetailsDC.Save(aiUserId, aiReportingUserId, adtStartDate, adtEndDate, asLessonPlanXml, adtOldStartDate, adtOldEndDate);
        }

        /// <summary>
        /// This method is used to submit lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        public void Submit(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate)
        {
            this.moLessionPlanDetailsDC.Submit(aiUserId, aiReportingUserId, adtStartDate, adtEndDate);
        }

        /// <summary>
        /// This method is used to return lesson plan details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiLessonPlanConfigId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        public void GetAll(int aiUserId, int aiReportingUserId, int aiLessonPlanConfigId, int aiStdDivId, int aiSubjectId)
        {
            this.moLessionPlanDetailsDC.GetAll(aiUserId, aiReportingUserId, aiLessonPlanConfigId, aiStdDivId, aiSubjectId);
        }

        /// <summary>
        /// This method is used to return all teachers.
        /// </summary>
        /// <returns></returns>
        public List<TeacherDetails> GetAllTeachers(string asFullAccess)
        {
            return this.moLessionPlanDetailsDC.GetAllTeachers(asFullAccess);
        }

        /// <summary>
        /// This method is used to return class subjects.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <returns></returns>
        public List<ClassSubjectDetails> GetAllClassSubjects(int aiUserId, int aiReportingUserId)
        {
            return this.moLessionPlanDetailsDC.GetAllClassSubjects(aiUserId, aiReportingUserId);
        }

        /// <summary>
        /// This method is used to delete configuration.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        public void DeleteConfiguration(int aiUserId, DateTime adtStartDate, DateTime adtEndDate)
        {
            this.moLessionPlanDetailsDC.DeleteConfiguration(aiUserId, adtStartDate, adtEndDate);
        }

        /// <summary>
        /// This method is used to update the read suggesition status.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public void UpdateReadSuggestion(int aiUserId, DateTime adtStartDate, DateTime adtEndDate, int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            LessionPlanDetailsDC oLessionPlanDetailsDC = new LessionPlanDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
            oLessionPlanDetailsDC.UpdateReadSuggestion(aiUserId, adtStartDate, adtEndDate);
        }

        /// <summary>
        /// This method is used to approve lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiConfigId"></param>
        public void Approve(int aiUserId, int aiConfigId)
        {
            this.moLessionPlanDetailsDC.Approve(aiUserId, aiConfigId);
        }

        /// <summary>
        /// This method is used to reject lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiConfigId"></param>
        /// <param name="asReason"></param>
        public void Reject(int aiUserId, int aiConfigId, string asReason)
        {
            this.moLessionPlanDetailsDC.Reject(aiUserId, aiConfigId, asReason);
        }

        /// <summary>
        /// This method is used to return last day of week.
        /// </summary>
        /// <returns></returns>
        public int GetLastDayOfWeek()
        {
            return this.moLessionPlanDetailsDC.GetLastDayOfWeek();
        }

        /// <summary>
        /// This method is used to return reporting user configuration.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LessonPlanReportingConfig> GetAllReportingConfigs(int aiUserId)
        {
            return this.moLessionPlanDetailsDC.GetAllReportingConfigs(aiUserId);
        }

        /// <summary>
        /// This method is used to return all lesson plans.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="abIsNewMode"></param>
        public void GetAll(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, bool abIsNewMode)
        {
            this.moLessionPlanDetailsDC.GetAll(aiUserId, aiReportingUserId, adtStartDate, adtEndDate, abIsNewMode);
        }

        /// <summary>
        /// This method is used to save comments saved by approver.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="asApproverComment"></param>
        public void SaveComment(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, string asApproverComment, DateTime adtOldStartDate, DateTime adtOldEndDate)
        {
            this.moLessionPlanDetailsDC.SaveComment(aiUserId, aiReportingUserId, adtStartDate, adtEndDate, asApproverComment, adtOldStartDate, adtOldEndDate);
        }

        /// <summary>
        /// This method is used to Update lesson plan Date.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="adtOldStartDate"></param>
        /// <param name="adtOldEndDate"></param>
        public void UpdateDate(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, DateTime adtOldStartDate, DateTime adtOldEndDate)
        {
            this.moLessionPlanDetailsDC.UpdateDate(aiUserId, aiReportingUserId, adtStartDate, adtEndDate, adtOldStartDate, adtOldEndDate);
        }
        #endregion
    }
}
