// Class Name       :- ODDetailsBL
// Purpose          :- This class is used to manage OD details.
// Date Of creation :- 13/1/2016
// Author Name      :- Dnyaneshwar Shinde.


using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class ODDetailsBL
    {
        #region Data members

        private ODDetailsDC moODDetailsDC;

        #endregion

        #region Constructors

        public ODDetailsBL()
        {
            this.moODDetailsDC = new ODDetailsDC();
        }

        public ODDetailsBL(int aiSchoolId, int aiUserId)
        {
            this.moODDetailsDC = new ODDetailsDC(aiSchoolId, aiUserId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get All staff members OD Details
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="asFilter"></param>
        public List<ODDetails> GetAllODDetails(int aiStaffGroupId, int aiUserId, int aiSchoolId, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {
            if (string.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "Date";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }
            
            if (asSortExpression.Contains("UserName"))
                asSortExpression = "FirstName" + " " + asSortDirection + " ," + "MiddleName" + " " + asSortDirection + " ," + "LastName" + " " + asSortDirection;
            else
                asSortExpression = asSortExpression + " " + asSortDirection;

            if (asFilter == null)
                asFilter = string.Empty;
            int iEndIndex = startRowIndex + maximumRows;
            return this.moODDetailsDC.GetAllODDetails(aiStaffGroupId, aiUserId, aiSchoolId, asSortExpression, startRowIndex, iEndIndex, asFilter);
        }

        /// <summary>
        /// This method is used to get count of OD Details
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        public int Count(int aiStaffGroupId, int aiSchoolId, int aiUserId, string asSortExpression, string asSortDirection, string asFilter)
        {
            if (asFilter == null)
                asFilter = string.Empty;
            return moODDetailsDC.Count(aiSchoolId, asFilter);
        }

        /// <summary>
        /// This method is used to get OD details
        /// </summary>
        /// <param name="aiId"></param>
        public ODDetails GetODDetail(int aiId)
        {
            return this.moODDetailsDC.GetODDetail(aiId);
        }

        /// <summary>
        /// This method is used to get OD Dates.
        /// </summary>
        /// <param name="aiUserId"></param>
        public List<ODDateDetails> GetDates(int aiUserId)
        {
            return this.moODDetailsDC.GetODDates(aiUserId);
        }

        /// <summary>
        /// This method is used to Save OD details
        /// </summary>
        /// <param name="oODDetails"></param>
        public void SaveODDetails(ODDetails aoODDetails)
        {
            this.moODDetailsDC.SaveODDetails(aoODDetails);
        }

        /// <summary>
        /// This method is used to Delete OD details
        /// </summary>
        /// <param name="aiId"></param>
        public void DeleteODDetails(int aiId)
        {
            this.moODDetailsDC.DeleteODDetails(aiId);
        }
        
        /// <summary>
        /// This method is used to return UserStaff Group Id and UserId For Search.
        /// </summary>
        public UserDetailsForOD GetUserDetailsForOD(string asName, int aiSchoolId, int aiAcademicYearId)
        {
            return this.moODDetailsDC.GetUserDetailsForOD(asName, aiSchoolId, aiAcademicYearId);
        }      

        #endregion
    }
}
