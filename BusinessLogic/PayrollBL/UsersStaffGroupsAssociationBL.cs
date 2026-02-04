// Class Name       :- OtherStaffGroupAssociationBL
// Purpose          :- This class is used to manage OtherStaffGroupAssociation details.
// Date Of creation :- 11/10/2009
// Author Name      :- 

using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class UsersStaffGroupsAssociationBL
    {
        #region "Data Members"
                
        private UsersStaffGroupsAssociationDC moUsersStaffGroupsAssociationDC;

        #endregion "Data Members"

        #region "Constructors"

        public UsersStaffGroupsAssociationBL()
        {
            moUsersStaffGroupsAssociationDC = new UsersStaffGroupsAssociationDC();
        }

        public UsersStaffGroupsAssociationBL(int aiSchoolId, int aiAcademicYearId)
        {
            moUsersStaffGroupsAssociationDC = new UsersStaffGroupsAssociationDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion "Constructors"

        #region "Properties"

        public UsersSGAssociation UsersSGAssociation
        {
            set { moUsersStaffGroupsAssociationDC.UsersSGAssociation = value; }
        }
              
        public List<UsersSGAssociation> UsersSGAssociations
        {
            get { return moUsersStaffGroupsAssociationDC.UsersSGAssociations;  }
            set { moUsersStaffGroupsAssociationDC.UsersSGAssociations = value;  }
        }

        public Insurance InsuranceAmountAndStatus
        {
            get { return moUsersStaffGroupsAssociationDC.Insurances; }
        }
        
        public List<UsersInsuranceDependent> DependentDetails
        {
            get { return moUsersStaffGroupsAssociationDC.InsuranceDependents; }
        }

        public List<Salutations> SalutationsForName
        {
            get { return moUsersStaffGroupsAssociationDC.SalutationsForName; }
        }

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to return tables of user role anf staff groups.
        /// </summary>
        public DataSet GetStaffGroupsAndRoles(int aiSchoolId)
        {
           return UsersStaffGroupsAssociationDC.GetStaffGroupsAndRoles(aiSchoolId);
        }

        /// <summary>
        /// This method is used to return user details.
        /// </summary>
        public DataTable GetUserDetails(int aiUserRoleId, string asUserName, int aiUserTypeId, bool abWithSalutation)
        {
            if (asUserName == null)
                asUserName = string.Empty;
            return moUsersStaffGroupsAssociationDC.GetUserDetails(aiUserRoleId, asUserName, aiUserTypeId, abWithSalutation); 
        }

        /// <summary>
        /// This method is used to insert/edit/delete association.
        /// </summary>
        public DataSet Save(int aiLeaveSeperaterDay)
        {
            return moUsersStaffGroupsAssociationDC.Save(aiLeaveSeperaterDay);
        }

        public void LockUnlocksalaryUser(int aiUserId, int aiSchoolId, bool aiIsLocked, int aiUpdatedById)
        {
            moUsersStaffGroupsAssociationDC.LockUnlocksalaryUser(aiUserId, aiSchoolId, aiIsLocked, aiUpdatedById);
        }

        public void GetUserInsuranceDetails(int UserId, int SchoolId)
        {
             moUsersStaffGroupsAssociationDC.GetUserInsuranceDetails(UserId, SchoolId);
        }

        public void SaveInsuranceDetails(decimal aiAmount, int Status, int UserId, string asInsuranceCardNumber, int aiUpdatedById)
        {
            UsersStaffGroupsAssociationDC.SaveInsuranceDetails(aiAmount, Status, UserId, asInsuranceCardNumber, aiUpdatedById);
        }

        public void RemoveOldInsuranceDetails(int UserId, int SchoolId, int aiUpdatedById)
        {
            UsersStaffGroupsAssociationDC.RemoveOldInsuranceDetails(UserId, SchoolId, aiUpdatedById);
        }

        public void InsertDependentDetails(UsersInsuranceDependent oUsersInsuranceDependent, int UserId, int SchoolId, int InsertedById)
        {
            CheckDuplicateDetails(oUsersInsuranceDependent);
			UsersStaffGroupsAssociationDC.InsertDependentDetails(oUsersInsuranceDependent, UserId, SchoolId, InsertedById);
        }

        public void UpdateDependentDetails(UsersInsuranceDependent oUsersInsuranceDependent, int InsertedById,  int UsersInsuranceDependentId)
        {
            CheckDuplicateDetails(oUsersInsuranceDependent);
			UsersStaffGroupsAssociationDC.UpdateDependentDetails(oUsersInsuranceDependent, InsertedById, UsersInsuranceDependentId);
        }

        public Insurance GetUserInsuranceAmountAndStatus(int UserId, int SchoolId)
        {
            return UsersStaffGroupsAssociationDC.GetUserInsuranceAmountAndStatus(UserId, SchoolId);
        }

        public List<UsersInsuranceDependent> GetUserDependentDetails(int UserId, int SchoolId)
        {
            return UsersStaffGroupsAssociationDC.GetUserDependentDetails(UserId, SchoolId);
        }

        public void DeleteDependentDetails(int UserId, int SchoolId, int aiUpdatedById)
        {
            UsersStaffGroupsAssociationDC.DeleteDependentDetails(UserId, SchoolId, aiUpdatedById);
        }

        public void DeleteDependent(int iUsersInsuranceDependentId, int iSchoolID, int aiUpdatedById)
        {
            UsersStaffGroupsAssociationDC.DeleteDependent(iUsersInsuranceDependentId, iSchoolID, aiUpdatedById);
        }

        public UsersInsuranceDependent GetDependent(int iUsersInsuranceDependentId, int iSchoolID)
        {
            return UsersStaffGroupsAssociationDC.GetDependent(iUsersInsuranceDependentId, iSchoolID);
        }
		
        /// <summary>
        /// This function is used to Get the User Basic details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
		public UserBasicDetails GetUserBasicDetails(int aiUserId,int aiSchoolId)
        {
            return UsersStaffGroupsAssociationDC.GetUserBasicDetails(aiUserId, aiSchoolId);
        }

        /// <summary>
        /// This function is used to save user basic details.
        /// </summary>
        /// <param name="aoUserBasicDetails"></param>
        /// <param name="aiAcademicYearId"></param>
        public void SaveBasicDetails(UserBasicDetails aoUserBasicDetails, int aiAcademicYearId, int aiLeaveSeperaterDay)
        {
            UsersStaffGroupsAssociationDC.SaveBasicDetails(aoUserBasicDetails, aiAcademicYearId, aiLeaveSeperaterDay);
        }

        /// <summary>
        /// This function is used to validate profile details for user before saving it.
        /// </summary>
        /// <param name="aoUserBasicDetails"></param>
        /// <param name="aiAcademicYearId"></param>
        public void ValidateProfileDetails(UserBasicDetails aoUserBasicDetails, int aiAcademicYearId)
        {
            UsersStaffGroupsAssociationDC.ValidateProfileDetails(aoUserBasicDetails, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to return all the users.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetPayrollUsers(int aiStaffGroupId)
        {
            return moUsersStaffGroupsAssociationDC.GetPayrollUsers(aiStaffGroupId);
        }
        public static DataTable GetAllBloodGroups()
        {
            return UsersStaffGroupsAssociationDC.GetAllBloodGroups();
        }
        #endregion "Public Methods"

		#region -- PRIVATE METHOD(s) --

		private static void CheckDuplicateDetails(UsersInsuranceDependent aoDependentDetails)
		{
			if (UsersStaffGroupsAssociationDC.CheckDuplicateDependantDetails(aoDependentDetails))
				throw new DuplicateUserException("Dependant details should not be duplicated.");
		}

		#endregion -- PRIVATE METHOD(s) --
    }
}
