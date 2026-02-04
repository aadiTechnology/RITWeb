using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using LessonPlanEntities;
using StaffPerformanceEntity;

namespace BusinessLogic
{
    public class LessonPlanReportingConfigBL
    {
        #region Data Member(s)

        private LessonPlanReportingConfigDC mLessonPlanReportingConfigDC;

        #endregion

        public LessonPlanReportingConfigBL()
        {
        }

        public LessonPlanReportingConfigBL(int aiSchooLId, int aiAcademicYearId, int aiUpdatedById)
        {
            mLessonPlanReportingConfigDC = new LessonPlanReportingConfigDC(aiSchooLId, aiAcademicYearId, aiUpdatedById);
        }

        #region Method(s)

        /// <summary>
        /// This method is used to get all the reporting configuration for selected user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LessonPlanReportingConfig> GetAll(int aiUserId)
        {
            return this.mLessonPlanReportingConfigDC.GetAll(aiUserId);
        }

        /// <summary>
        /// This method is used to get all the reporting configuration for selected reporting user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public LessonPlanReportingConfig Get(int aiUserId, int aiReportingConfigId)
        {
            return this.mLessonPlanReportingConfigDC.Get(aiUserId, aiReportingConfigId);
        }

        /// <summary>
        /// This method is used to save the reporting configuration for selected user.
        /// </summary>
        /// <param name="aoLessonPlanReportingConfig"></param>
        public void Save(LessonPlanReportingConfig aoLessonPlanReportingConfig)
        {
            this.mLessonPlanReportingConfigDC.Save(aoLessonPlanReportingConfig);
        }

        /// <summary>
        /// This method is used to delete the reporting configuation for selected user.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingConfigId"></param>
        public void Delete(int aiUserId, int aiReportingConfigId)
        {
            this.mLessonPlanReportingConfigDC.Delete(aiUserId, aiReportingConfigId);
        }

        /// <summary>
        /// This method is used to submit all the configuration to database.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void SubmitUnsubmitConfig(int aiUserId, bool abIsSubmit)
        {
            this.mLessonPlanReportingConfigDC.SubmitUnsubmitConfig(aiUserId, abIsSubmit);
        }

        /// <summary>
        /// This method is used to retrieve staff reporting config for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        //public List<LessonPlanReportingConfig> GetAllReportingConfigs(int aiUserID, int aiYear)
        //{
        //    return this.mLessonPlanReportingConfigDC.GetAllReportingConfigs(aiUserID, aiYear);
        //}

        /// <summary>
        /// This method is used to retrieve invitee of for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        //public List<LessonPlanReportingConfig> GetAllInviteeOfGivenUser(int aiUserID, int aiYear)
        //{
        //    return this.mLessonPlanReportingConfigDC.GetAllInviteeOfGivenUser(aiUserID, aiYear);
        //}

        /// <summary>
        /// This method is used to send request to invitee.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        //public void SendRequestToInvitee(string asInviteeUserIds, int aiReportingUserId, int aiYear)
        //{
        //    this.mLessonPlanReportingConfigDC.SendRequestToInvitee(asInviteeUserIds, aiReportingUserId, aiYear);
        //}

         #endregion

        //public List<ReportingStaff> GetAllUsers(int aiUserId, int aiUserRoleId)
        //{
        //    return mLessonPlanReportingConfigDC.GetAllUsers(aiUserId, aiUserRoleId);
        //}

        public void Copy(int aiUserId, int aiUserRoleId, int aiYear, string asSelectedUserIds)
        {
            mLessonPlanReportingConfigDC.Copy(aiUserId, aiUserRoleId, aiYear, asSelectedUserIds);
        }
    }
}
