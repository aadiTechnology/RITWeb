// Class Name       :- UserLeavesYearwiseConfigurationBL
// Purpose          :- This class is used to manage UserLeavesYearwiseConfiguration details.
// Date Of creation :- 5 Jan 2010
// Author Name      :- Deepak

using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class UserLeavesYearwiseConfigurationBL
    {
        #region Data Member(s)

        private UserLeavesYearwiseConfigurationDC moUserLeavesYearwiseConfigurationDC; 

        #endregion

        #region Constructor(s)

        public UserLeavesYearwiseConfigurationBL()
        {
            this.moUserLeavesYearwiseConfigurationDC = new UserLeavesYearwiseConfigurationDC();
        }

        public UserLeavesYearwiseConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moUserLeavesYearwiseConfigurationDC = new UserLeavesYearwiseConfigurationDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Property(s)

        public List<UserLeaveConfiguration> UserLeaveConfiguration
        {
            get { return this.moUserLeavesYearwiseConfigurationDC.UserLeaveConfigurations; }
            set { this.moUserLeavesYearwiseConfigurationDC.UserLeaveConfigurations = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used get save leaves for staff groups member or for whole staff group.
        /// </summary>
        /// <param name="abApplytoAll"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void Save(bool abApplytoAll, int aiStaffGroupId, char acUpdateAll, UserLeaveConfiguration aoUserLeaveConfiguration)
        {   
            this.moUserLeavesYearwiseConfigurationDC.Save(abApplytoAll, aiStaffGroupId, acUpdateAll, aoUserLeaveConfiguration);
        }

        /// <summary>
        /// This method is used to get years.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetYears()
        {
            return this.moUserLeavesYearwiseConfigurationDC.GetYears();
        }

        /// <summary>
        /// This method is used get saved or default leaves for staff groups member.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public DataSet GetAllowedLeaves(int aiUserId, int aiYear)
        {
            return this.moUserLeavesYearwiseConfigurationDC.GetAllowedLeaves(aiUserId, aiYear);
        }

        /// <summary>
        /// This method is used to return users basic leave details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<BasicLeaveDetails> GetUsersBasicLeaves(int aiUserId, int aiStaffGroupId, int aiLeaveSeperatorDay)
        {
            return this.moUserLeavesYearwiseConfigurationDC.GetUsersBasicLeaves(aiUserId, aiStaffGroupId, aiLeaveSeperatorDay);
        } 

        #endregion

        public List<LeaveYear> GetLeaveYears()
        {
            return moUserLeavesYearwiseConfigurationDC.GetLeaveYears();
        }

        /// <summary>
        /// This method is used to get user leave details for leaves encashment.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public DataSet GetUserLeavesForEncashment(int aiUserId)
        {
            return moUserLeavesYearwiseConfigurationDC.GetUserLeavesForEncashment(aiUserId);
        }

        /// <summary>
        /// This method is used to get leave balance for encashment.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiLeaveID"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public decimal GetLeaveBalanceForEncashment(int aiUserId, int aiLeaveID, int aiYear)
        {
            return moUserLeavesYearwiseConfigurationDC.GetLeaveBalanceForEncashment(aiUserId, aiLeaveID, aiYear);
        }

        /// <summary>
        /// This method is used to save leave encashment details in database.
        /// </summary>
        /// <param name="moLeaveEncashmentDetails"></param>
        public void SaveEncashmentDetails(LeaveEncashmentDetails aoLeaveEncashmentDetails)
        {
            moUserLeavesYearwiseConfigurationDC.SaveEncashmentDetails(aoLeaveEncashmentDetails);
        }

        /// <summary>
        /// This method is used to get user encashed leave details for fill listview.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LeaveEncashmentDetails> GetUserAllEncashLeaveDetails(int aiUserId)
        {
            return moUserLeavesYearwiseConfigurationDC.GetUserAllEncashLeaveDetails(aiUserId);
        }

        /// <summary>
        /// This method is used to get user encash leave details for update.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiEncashLeaveId"></param>
        /// <returns></returns>
        public LeaveEncashmentDetails GetUserEncashLeaveDetails(int aiUserId, int aiEncashLeaveId)
        {
            return moUserLeavesYearwiseConfigurationDC.GetUserEncashLeaveDetails(aiUserId, aiEncashLeaveId);
        }

        /// <summary>
        /// This method is used to delete encash leave details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiEncashLeaveId"></param>
        /// <param name="aiLeaveId"></param>
        public void DeleteUserEncashLeave(int aiUserId, int aiEncashLeaveId)
        {
            moUserLeavesYearwiseConfigurationDC.DeleteUserEncashLeave(aiUserId, aiEncashLeaveId);
        }
        /// <summary>
        /// This method is used to get Amount .
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAmountForLeaveEncashment(int aiUserId, string Date,  int aiLeaveId, int miAcademicYearId, int miSchoolId)
        {
            return this.moUserLeavesYearwiseConfigurationDC.GetAmountForLeaveEncashment(aiUserId, Date,  aiLeaveId, miAcademicYearId, miSchoolId);
        }
    }
}