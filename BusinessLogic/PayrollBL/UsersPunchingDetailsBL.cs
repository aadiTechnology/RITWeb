using System.Collections;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using XseedReportEntities;
using Utility;
using System;

namespace BusinessLogic
{
    public class UsersPunchingDetailsBL
    {
        #region Data members

        private UsersPunchingDetailsDC.UsersPunchingDetailsStruct moUsersPunchingDetailsStruct;
        private UsersPunchingDetailsDC moUsersPunchingDetailsDC = new UsersPunchingDetailsDC();

        #endregion

        private int miPunchedUsersCount;
        private int miNotPunchedUsersCount;

        public UsersPunchingDetailsBL()
        {
            moUsersPunchingDetailsDC = new UsersPunchingDetailsDC();
        }

        #region " Properties "

        public int UserId
        {
            get
            {
                return moUsersPunchingDetailsStruct.miUserId;
            }
            set
            {
                moUsersPunchingDetailsStruct.miUserId = value;
            }
        }

        
        public int AcademicYearId
        {
            get
            {
                return moUsersPunchingDetailsStruct.miAcademicYearId;
            }
            set
            {
                moUsersPunchingDetailsStruct.miAcademicYearId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moUsersPunchingDetailsStruct.miSchoolId;
            }
            set
            {
                moUsersPunchingDetailsStruct.miSchoolId = value;
            }
        }
        #endregion

        /// <summary>
        /// This function is used to get all users who have punched in the biometric machine.
        /// </summary>
        public DataTable GetAllUsersPunched(int aiSchoolId, string asSelectedDate, bool abChkGroupByUser, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "IndexNo";
            
            if (sortExpression == "IndexNo")
                sortExpression = " IndexNo DESC";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;

            DataTable oDt = UsersPunchingDetailsDC.GetAllUsersPunched(aiSchoolId, sortExpression, iEndIndex, startRowIndex, asSelectedDate, abChkGroupByUser);
            if (oDt != null && oDt.Rows.Count > 0)
                miPunchedUsersCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function is used to get total count of the Punched Users. 
        /// </summary>
        public int CountTotalUsersPunched(int aiSchoolId, string asSelectedDate, bool abChkGroupByUser)
        {
            return miPunchedUsersCount;
        }

        /// <summary>
        /// This function is used to get all users who have not yet punched in the biometric machine.
        /// </summary>
        public DataTable GetAllUsersNotPunched(int aiSchoolId, string asSelectedDate, bool abChkGroupByUser, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Employee_No";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;

            DataTable oDt = UsersPunchingDetailsDC.GetAllUsersNotPunched(aiSchoolId, sortExpression, iEndIndex, startRowIndex, asSelectedDate);

            if (oDt != null && oDt.Rows.Count > 0)
                miNotPunchedUsersCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function is used to get total count of the non punched Users. 
        /// </summary>
        public int CountTotalUsersNotPunched(int aiSchoolId, string asSelectedDate, bool abChkGroupByUser)
        {
            return miNotPunchedUsersCount;
        }
    }
}
