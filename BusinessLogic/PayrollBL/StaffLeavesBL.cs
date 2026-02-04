// Class Name       :- StaffLeavesBL
// Purpose          :- This class is used to manage StaffLeaves details.
// Date Of creation :- 11/7/2009
// Author Name      :- Sachin

using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class StaffLeavesBL
    {
        #region Data Member(s)

        private StaffLeavesDC moStaffLeavesDC; 

        #endregion

        #region Constructor(s)

        public StaffLeavesBL()
        {
            moStaffLeavesDC = new StaffLeavesDC();
        }

        public StaffLeavesBL(int aiSchoolId, int aiUpdatedById)
        {
            moStaffLeavesDC = new StaffLeavesDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public ConfiguredLeaves ConfiguredLeave
        {
            set { moStaffLeavesDC.ConfiguredLeave = value; }
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to save leave configuration.
        /// </summary>
        /// <param name="aiOriginalId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable Save(int aiOriginalId, int aiAcademicYearId)
        {
            return moStaffLeavesDC.Save(aiOriginalId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to return all the configured leave types.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetAll(int aiSchoolId)
        {
            return StaffLeavesDC.GetAll(aiSchoolId);
        }

        /// <summary>
        /// This method is used toreturn basic leave details.
        /// </summary>
        /// <returns></returns>
        public List<BasicLeaveDetails> GetBasicLeaveDetails()
        {
            return moStaffLeavesDC.GetBasicLeaveDetails();
        }

        /// <summary>
        /// This method is used to return basic leave configuration.
        /// </summary>
        /// <returns></returns>
        public List<BasicLeaveConfiguration> GetBasicLeaveConfigs(int aiBasicLeaveConfigId)
        {
            return moStaffLeavesDC.GetBasicLeaveConfigs(aiBasicLeaveConfigId);
        }

        /// <summary>
        /// This method is used to save basic leave configuration.
        /// </summary>
        /// <param name="aoBasicLeaveConfiguration"></param>
        public void SaveBasicLeaveConfig(BasicLeaveConfiguration aoBasicLeaveConfiguration)
        {
            moStaffLeavesDC.SaveBasicLeaveConfig(aoBasicLeaveConfiguration);
        }

        /// <summary>
        /// This method is used to delete basic leave config details.
        /// </summary>
        /// <param name="aiId"></param>
        public void DeleteBasicLeaveConfig(int aiId)
        {
            moStaffLeavesDC.DeleteBasicLeaveConfig(aiId);
        }

        /// <summary>
        /// This method is used to apply changes on all users of selected staff group and year.
        /// </summary>
        /// <param name="aiStaffGroupsId"></param>
        /// <param name="bUpdateExisting"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiLeaveSeperaterDay"></param>
        public void ApplyToAllUsers(int aiStaffGroupsId, bool abUpdateExisting, int aiLeaveSeperaterDay)
        {
            moStaffLeavesDC.ApplyToAllUsers(aiStaffGroupsId, abUpdateExisting, aiLeaveSeperaterDay);
        }

        #endregion
    }   
}
