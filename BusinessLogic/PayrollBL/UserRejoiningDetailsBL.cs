// Class Name       :- UserRejoiningDetailsBL
// Purpose          :- This class is used to manage user rejoining details.
// Date Of creation :- 08/11/2019
// Author Name      :- Dnyaneshwar Shinde

using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using PayrollEntities;
using Utility;
using SchoolEntities.Payroll;

namespace BusinessLogic
{
    public class UserRejoiningDetailsBL
    {
        #region Data member(s)

        private UserRejoinigDetailsDC moUserRejoinigDetailsDC;
        private int miTotalRowCount;

        #endregion

        #region Constructor(s)

        public UserRejoiningDetailsBL()
        {
            this.moUserRejoinigDetailsDC = new UserRejoinigDetailsDC();
        }
        
        public UserRejoiningDetailsBL(int aiSchoolId, int aiUserId)
        {
            this.moUserRejoinigDetailsDC = new UserRejoinigDetailsDC(aiSchoolId, aiUserId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get all User details for fill User combobox.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public List<UserRejoiningDetails> GetAllUsers(int aiStaffGroupId)
        {
            return this.moUserRejoinigDetailsDC.GetAllUsers(aiStaffGroupId);
        }

        /// <summary>
        /// This method is used to get selected user details for Rejoinig.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRejoingId"></param>
        /// <returns></returns>
        public UserRejoiningDetails Get(int aiStaffGroupId, int aiUserId, int aiUserRejoiningId)
        {
            return this.moUserRejoinigDetailsDC.Get(aiStaffGroupId, aiUserId, aiUserRejoiningId);
        }

        /// <summary>
        /// This method is used to Save user rejoining details.
        /// </summary>
        /// <param name="aoUserRejoiningDetails"></param>
        public void Save(UserRejoiningDetails aoUserRejoiningDetails)
        {
            this.moUserRejoinigDetailsDC.Save(aoUserRejoiningDetails);
        }

        /// <summary>
        /// This method is used to get all users for fill list view.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <param name="aistartRowIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<UserRejoiningDetails> GetAll(int aiSchoolId, string asFilter, int maximumRows, int startRowIndex)
        {
            if (asFilter == null)
                asFilter = string.Empty;

            int iEndIndex = startRowIndex + maximumRows;
            List<UserRejoiningDetails> lstUserRejoiningDetails = this.moUserRejoinigDetailsDC.GetAll(aiSchoolId, asFilter, startRowIndex, iEndIndex);
            if (lstUserRejoiningDetails.Count > Constants.I_ZERO)
                miTotalRowCount = lstUserRejoiningDetails[0].TotalRowCount.ToInt();

            return lstUserRejoiningDetails;
        }

        /// <summary>
        /// This method is used to return total users count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, string asFilter)
        {
            return miTotalRowCount;
        }

        /// <summary>
        /// This method is used to delete user rejoing details.
        /// </summary>
        /// <param name="aiUserRejoinId"></param>
        /// <param name="aiUserId"></param>
        public void Delete(int aiUserRejoinId)
        {
            moUserRejoinigDetailsDC.Delete(aiUserRejoinId);
        }

        #endregion
    }
}
