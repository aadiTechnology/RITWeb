using System;
using System.Collections;
using System.Data;
using System.Collections.ObjectModel;
using Utility;
using DataCommunicator;
using System.Collections.Generic;
namespace BusinessLogic
{
    public class UserWeekEndAssociationBL
    {

        #region Data members

        private UserWeekendAssociationDC.UserWeekendAssociationDetailsStruct moUserWeekendAssociationDetailStruct;
        private UserWeekendAssociationDC moUserWeekendAssociationDC = new UserWeekendAssociationDC();
        public Constants.Action eAction;

        #endregion

        public UserWeekEndAssociationBL()
        {
            moUserWeekendAssociationDC = new UserWeekendAssociationDC();
        }
       

        #region " Properties "

        public int WeekEndId
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miWeekEndId;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miWeekEndId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miSchoolId;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miSchoolId = value;
            }
        }

        public int UserId
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miUserId;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miUserId = value;
            }
        }

        public int AcademicYearId
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miAcademicYearId;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miAcademicYearId = value;
            }
        }

        public char IsDeleted
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.mbIs_Deleted;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.mbIs_Deleted = value;
            }
        }

        public int InsertedById
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miInsertedById;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miInsertedById = value;
            }
        }

        public DateTime InsertedDate
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.mdtInsertDate;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.mdtInsertDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.miUpdatedById;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.miUpdatedById = value;
            }
        }

        public DateTime UpdatedDate
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.mdtUpdateDate;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.mdtUpdateDate = value;
            }
        }

        public static int UsersCount
        {
            get
            {
                return UserWeekendAssociationDC.miUsersCount;
            }
            set
            {
                UserWeekendAssociationDC.miUsersCount = value;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get
            {
                return eAction;
            }
            set
            {
                eAction = value;
            }
        }

        public bool IsOtherStaffApplicable
        {
            get
            {
                return moUserWeekendAssociationDetailStruct.mbIsOtherStaffApplicable;
            }
            set
            {
                moUserWeekendAssociationDetailStruct.mbIsOtherStaffApplicable = value;
            }
        }

        #endregion

        /// <summary>
        /// This function is used to Insert Weekend association details for user when new user is added.
        /// </summary>
        public string InsertUserWeekendAssociationDetails()
        {
            // This Function is used to insert the record in to database. 
            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            return moUserWeekendAssociationDC.GetUserWeekndAssociationInsertStatement();
        }

        /// <summary>
        /// This function is used to insert weekend association details for Other and admin staff when newly added.
        /// </summary>
        public void InsertWeekendAssociationDetailsForOtherAndAdminStaff()
        {
            // This Function is used to insert the record in to database. 
            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            moUserWeekendAssociationDC.InsertUserWeekendAssociationDetailsForOtherStaff();
        }

        /// <summary>
        /// This function is used to get weekends which are applicable to staff.
        /// </summary>
        public List<int> GetWeekendsApplicableforStaff(int aiSchoolId, int aiAcademicYrId)
        {
            return UserWeekendAssociationDC.GetWeekendsApplicableforStaff(aiSchoolId, aiAcademicYrId);
        }

        /// <summary>
        /// This function is used to get all user details.
        /// </summary>
        public static DataTable GetAllUsersDetails(int aiSchoolId, int aiStaffGroupId, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "UserId";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = UserWeekendAssociationDC.GetAllUsersDetails(aiSchoolId, aiStaffGroupId, sortExpression, iStartIndex, iEndIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                UsersCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function is used to get total count of users. 
        /// </summary>
        public static int CountTotalUsers(Int32 aiSchoolId, Int32 aiStaffGroupId)
        {
            return UsersCount;
        }

        /// <summary>
        /// This function is used to get Weekends for specific user.
        /// </summary>
        public static DataTable GetWeekends(int aiUserId, int aiSchoolId, int aiAcademicYrId)
        {
            return UserWeekendAssociationDC.GetWeekends(aiUserId, aiSchoolId, aiAcademicYrId);
        }

        /// <summary>
        /// This function is used to search user by name.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUsersforSearch(string asName, int aiSchoolId)
        {
            return UserWeekendAssociationDC.GetUsersforSearch(asName, aiSchoolId);
        }

        /// <summary>
        /// This function is used to get All Weekends.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllWeekends(int aiSchoolId)
        {
            return UserWeekendAssociationDC.GetAllWeekends(aiSchoolId);
        }

        /// <summary>
        /// This function is used to Insert user weekend association details for user.
        /// </summary>
        public void InsertUserWeekEndAssociationDetailsForUser(int aiUserId, int aiSchoolId, int aiAcademicYearId, int asWeekendId)
        {
            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            moUserWeekendAssociationDC.InsertUserWeekEndAssociationDetailsForUser(aiUserId, aiSchoolId, aiAcademicYearId, asWeekendId);
        }

        /// <summary>
        /// This function is used to update user weekend association details for user.
        /// </summary>
        public void UpdateUserWeekendAssociationDetailsForUser(int aiUserId, int aiSchoolId, int aiAcademicYearId, int asWeekendId)
        {
            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            moUserWeekendAssociationDC.UpdateUserWeekendAssociationDetailsForUser(aiUserId, aiSchoolId, aiAcademicYearId, asWeekendId);
        }

        /// <summary>
        /// This method is used to retrieve insert statement.
        /// </summary>
        public string InsertStatmentUserWeekendAssociation()
        {
            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            return moUserWeekendAssociationDC.InsertStatmentUserWeekendAssociation();
        }

        /// <summary>
        /// This method is used to retrieve delete statement.
        /// </summary>
        public string DeleteStatmentUserWeekendAssociation()
        {

            moUserWeekendAssociationDC.userWeekendAssociationDetailStruct = moUserWeekendAssociationDetailStruct;
            return moUserWeekendAssociationDC.DeleteStatmentUserWeekendAssociation();
        }

    }

     /// <summary>
    /// This collection class is used to update all weekdays configuration details. 
    /// </summary>
    public class UserWeekEndConfigCollectionBL : IEnumerable
    {
        private Collection<UserWeekEndAssociationBL> moUserWeekendConfigListBL = null;
        UserWeekendMasterCollectionDC oUserWeekendMasterCollectionDC;

        public Collection<UserWeekEndAssociationBL> UserWeekendConfigListBL
        {
            get
            {
                return moUserWeekendConfigListBL;
            }
            set
            {
                moUserWeekendConfigListBL = value;
            }
        }

        public UserWeekEndConfigCollectionBL()
        {
            moUserWeekendConfigListBL = new Collection<UserWeekEndAssociationBL>();
            oUserWeekendMasterCollectionDC = new UserWeekendMasterCollectionDC();
        }

        /// <summary>
        /// This method is used to add collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Add(UserWeekEndAssociationBL aoWeekDaysMasterBL)
        {
            moUserWeekendConfigListBL.Add(aoWeekDaysMasterBL);
        }

        /// <summary>
        /// This method is used to remove collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Remove(UserWeekEndAssociationBL aoWeekDaysMasterBL)
        {
            moUserWeekendConfigListBL.Remove(aoWeekDaysMasterBL);

        }

        public IEnumerator GetEnumerator()
        {
            return new UserWeekEndCollectionEnumerator(this);
        }

        /// <summary>
        /// This method is used to update all weekend configuration details.
        /// </summary>
        public void UpdateAllUserWeekEndAssociationConfigurationDetails(int aiAcadYrId)
        {
            {
                IEnumerator oEnum = moUserWeekendConfigListBL.GetEnumerator();
                ArrayList oArrayListInsertWeekEnd = new ArrayList();
                while (oEnum.MoveNext())
                {
                    UserWeekEndAssociationBL oUserWeekEndAssociationMasterBL = (UserWeekEndAssociationBL)oEnum.Current;
                    switch (oUserWeekEndAssociationMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayListInsertWeekEnd.Add(((UserWeekEndAssociationBL)oEnum.Current).InsertStatmentUserWeekendAssociation());
                            break;
                        case Constants.Action.Delete:
                            oArrayListInsertWeekEnd.Add(((UserWeekEndAssociationBL)oEnum.Current).DeleteStatmentUserWeekendAssociation());
                            break;
                    }
                }
            }
        }

        private class UserWeekEndCollectionEnumerator : IEnumerator
        {
            #region DataMember
            private int position = -1;
            private UserWeekEndConfigCollectionBL moUserWeekEndAssociationCollection;
            #endregion

            #region Constructor
            public UserWeekEndCollectionEnumerator(UserWeekEndConfigCollectionBL aoWeekEndCollection)
            {
                moUserWeekEndAssociationCollection = aoWeekEndCollection;
            }
            #endregion

            #region Public Method
            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (position < moUserWeekEndAssociationCollection.moUserWeekendConfigListBL.Count - 1)
                {
                    position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                position = -1;
            }
            #endregion

            #region Property
            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return moUserWeekEndAssociationCollection.moUserWeekendConfigListBL[position];
                }
            }
            #endregion
        }
    }
        
}