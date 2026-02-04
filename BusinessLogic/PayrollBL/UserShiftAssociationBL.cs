using System.Collections;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using XseedReportEntities;
using Utility;
using System;

namespace BusinessLogic
{
    public class UserShiftAssociationBL
    {
        #region Data members

        private UserShiftAssociationDC.UserShiftsAssociationDetailsStruct moUserShiftAssociationDetailStruct;
        private UserShiftAssociationDC moUserShiftAssociationDC = new UserShiftAssociationDC();

        #endregion

        private int miUserCount;

        public UserShiftAssociationBL()
        {
            moUserShiftAssociationDC = new UserShiftAssociationDC();
        }


        #region " Properties "

        public int UserShiftId
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miUserShiftId;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miUserShiftId = value;
            }
        }

        public int Shiftid
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miShiftId;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miShiftId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miSchoolId;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miSchoolId = value;
            }
        }

        public int UserId
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miUserId;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miUserId = value;
            }
        }


        public int AcademicYearId
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miAcademicYearId;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miAcademicYearId = value;
            }
        }

        public char IsDeleted
        {
            get
            {
                return moUserShiftAssociationDetailStruct.mbIs_Deleted;
            }
            set
            {
                moUserShiftAssociationDetailStruct.mbIs_Deleted = value;
            }
        }

        public int InsertedById
        {
            get
            {
                return moUserShiftAssociationDetailStruct.miInsertedByid;
            }
            set
            {
                moUserShiftAssociationDetailStruct.miInsertedByid = value;
            }
        }

        public DateTime InsertedDate
        {
            get
            {
                return moUserShiftAssociationDetailStruct.mdtInsertDate;
            }
            set
            {
                moUserShiftAssociationDetailStruct.mdtInsertDate = value;
            }
        }
        #endregion

        public string InsertUserShiftAssociationDetails()
        {
            // This Function is used to insert the record in to database. 
            moUserShiftAssociationDC.userShiftAssociationDetailStruct = moUserShiftAssociationDetailStruct;
            return moUserShiftAssociationDC.GetUserShiftAssociationInsertStatement();
        }


        public void InsertShiftAssociationDetailsForOtherAndAdminStaff()
        {
            // This Function is used to insert the record in to database. 
            moUserShiftAssociationDC.userShiftAssociationDetailStruct = moUserShiftAssociationDetailStruct;
            moUserShiftAssociationDC.InsertUserShiftAssociationDetailsForOtherStaff();
        }

        /// <summary>
        /// This function is used to get all shift details and bind to object data source. 
        /// </summary>
        public DataTable GetUserDetails(int aiSchoolId, int aiAcademicYearId, int aiShiftId, int aistaffGroupId, string sortExpression, string asSearchText, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + Constants.I_GRID_PAGE_COUNT;
            DataTable oDt = UserShiftAssociationDC.GetUserDetails(aiSchoolId, aiAcademicYearId, aiShiftId, aistaffGroupId, sortExpression, asSearchText, iEndIndex, startRowIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                miUserCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function is used to get total count of the Users. 
        /// </summary>
        public int CountTotalUserRecords(int aiSchoolId, int aiAcademicYearId, int aiShiftId, int aistaffGroupId, string sortExpression, string asSearchText)
        {
            return miUserCount;
        }

        public int GetDefaultShift(int aiSchoolId, int aiAcademicYrId)
        {
            return UserShiftAssociationDC.GetDefaultShift(aiSchoolId, aiAcademicYrId);
        }

        public static DataTable GetAllUsers(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int aishiftId)
        {
            return UserShiftAssociationDC.GetAllUsersDetails(aiSchoolId, aiAcademicYearId, aiStaffGroupId, aishiftId);
        }

        public static DataTable GetUsersforSearch(string asName, int aiSchoolId, int aiAcademicYrId)
        {
            return UserShiftAssociationDC.GetUsersforSearch(asName, aiSchoolId, aiAcademicYrId);
        }

        public void InsertUserShiftAssociationDetailsForUser(string asUserIdXML, int aiSchoolId, int aiAcademicYrId, int aiShiftId, int aiInsertedById)
        {
            moUserShiftAssociationDC.userShiftAssociationDetailStruct = moUserShiftAssociationDetailStruct;
            moUserShiftAssociationDC.InsertUserShiftAssociationDetailsForUser(asUserIdXML, aiSchoolId, aiAcademicYrId, aiShiftId, aiInsertedById);
        }        
    }
}
