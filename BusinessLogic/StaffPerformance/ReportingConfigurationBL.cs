/*File Name - ReportingConfigurationDC.cs
 * Created By - Pravin Shinde
 * Created Date - 25 Sept 2013
 * Description - This class is used to manage performance reporting configuration.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using StaffPerformanceEntity;
using Utility;

namespace BusinessLogic
{   
    public class ReportingConfigurationBL
    {
        #region Data Member(s)

        private ReportingConfigurationDC moReportingConfigurationDC;

        #endregion

        #region Constructor(s)

        public ReportingConfigurationBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moReportingConfigurationDC = new ReportingConfigurationDC(aiSchoolId, aiUpdatedById);
        }

        public ReportingConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moReportingConfigurationDC = new ReportingConfigurationDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to get all the reporting configuration for selected user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<PerformanceReportingConfig> GetAll(int aiUserId, int aiYear)
        {
            return this.moReportingConfigurationDC.GetAll(aiUserId, aiYear);
        }

        /// <summary>
        /// This method is used to get all the reporting configuration for selected reporting user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public PerformanceReportingConfig Get(int aiUserId, int aiYear, int aiReportingConfigId)
        {
            return this.moReportingConfigurationDC.Get(aiUserId, aiYear, aiReportingConfigId);
        }

        /// <summary>
        /// This method is used to save the reporting configuration for selected user.
        /// </summary>
        /// <param name="aoPerformanceReportingConfig"></param>
        public void Save(PerformanceReportingConfig aoPerformanceReportingConfig)
        {
            this.moReportingConfigurationDC.Save(aoPerformanceReportingConfig);
        }

        /// <summary>
        /// This method is used to delete the reporting configuation for selected user.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingConfigId"></param>
        public void Delete(int aiYear, int aiUserId, int aiReportingConfigId)
        {
            this.moReportingConfigurationDC.Delete(aiYear,aiUserId,aiReportingConfigId);
        }

        /// <summary>
        /// This method is used to submit all the configuration to database.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void SubmitUnsubmitConfig(int aiUserId,int aiYear, bool abIsSubmit)
        {
            this.moReportingConfigurationDC.SubmitUnsubmitConfig(aiUserId, aiYear, abIsSubmit);
        }

        /// <summary>
        /// This method is used to retrieve staff reporting config for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public List<PerformanceReportingConfig> GetAllReportingConfigs(int aiUserID, int aiYear, bool abShowPending)
        {
            return this.moReportingConfigurationDC.GetAllReportingConfigs(aiUserID, aiYear, abShowPending);
        }

        /// <summary>
        /// This method is used to retrieve invitee of for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public List<PerformanceReportingConfig> GetAllInviteeOfGivenUser(int aiUserID, int aiYear)
        {
            return this.moReportingConfigurationDC.GetAllInviteeOfGivenUser(aiUserID, aiYear);
        }

        /// <summary>
        /// This method is used to send request to invitee.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SendRequestToInvitee(string asInviteeUserIds, int aiReportingUserId, int aiYear)
        {
            this.moReportingConfigurationDC.SendRequestToInvitee(asInviteeUserIds, aiReportingUserId, aiYear);
        }

         #endregion

        public List<ReportingStaff> GetAllUsers(int aiUserId, int aiUserRoleId, int aiYear, Constants.ReportingUserScreen aoReportingUserScreen)
        {
            return moReportingConfigurationDC.GetAllUsers(aiUserId, aiUserRoleId, aiYear, aoReportingUserScreen);
        }

        public void Copy(int aiUserId, int aiUserRoleId, int aiYear, string asSelectedUserIds, Constants.ReportingUserScreen aoReportingUserScreen)
        {
            moReportingConfigurationDC.Copy(aiUserId, aiUserRoleId, aiYear, asSelectedUserIds, aoReportingUserScreen);
        }
    }
}
