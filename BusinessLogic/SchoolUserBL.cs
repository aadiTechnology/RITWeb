/* File Name         :- BuyerBL.cs
 * Purpose           :- This Class is used as an interface between the UILayer and DCLayer.
 * Date of creation  :-16-apr-2007
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using BusinessLogic.Exceptions;
using DataCommunicator;
using MasterEntities;
using Utility;
using SchoolEntities;
using SchoolEntities.Admin;

namespace BusinessLogic
{
    public class SchoolUserBL
    {
        #region Data Members and Properties

        #region Data Members

        // Object of the data access class.
        private SchoolUserDC moSchoolUser;

        // Buyer's structure object.
        private SchoolUserDC.UserDetails moUserDetails;
        private SchoolWiseTeacherMasterBL moSchoolWiseTeacherMasterBL;
        const string S_DUPLICATE_REG_NO = "RegNoException";
        const string S_DUPLICATE_LOGIN_MSG = "MsgLoginNameExists";

        #endregion

        #region Properties

        // Property to get/set Id of Buyer.

        public SchoolUserDC.UserDetails UserInformation
        {
            get { return moUserDetails; }
            set { moUserDetails = value; }
        }

        public SchoolWiseTeacherMasterBL TeacherDetails
        {
            get { return moSchoolWiseTeacherMasterBL; }
            set { moSchoolWiseTeacherMasterBL = value; }
        }

        public Int32 SchoolId
        {
            get { return moUserDetails.SchoolId; }
            set { moUserDetails.SchoolId = value; }
        }

        public Int32 UserId
        {
            get { return moUserDetails.UserId; }
            set { moUserDetails.UserId = value; }
        }

        public string Login
        {
            get { return moUserDetails.Login; }
            set { moUserDetails.Login = value; }
        }

        public string FirstName
        {
            get { return moUserDetails.FirstName; }
            set { moUserDetails.FirstName = value; }
        }

        public string MiddleName
        {
            get { return moUserDetails.MiddleName; }
            set { moUserDetails.MiddleName = value; }
        }

        public string LastName
        {
            get { return moUserDetails.LastName; }
            set { moUserDetails.LastName = value; }
        }

        public string Address
        {
            get { return moUserDetails.Address; }
            set { moUserDetails.Address = value; }
        }

        public string EmergencyContact
        {
            get { return moUserDetails.EmergencyContact; }
            set { moUserDetails.EmergencyContact = value; }
        }

        public string Email
        {
            get { return moUserDetails.Email; }
            set { moUserDetails.Email = value; }
        }

        public string BloodGroup
        {
            get { return moUserDetails.BloodGroup; }
            set { moUserDetails.BloodGroup = value; }
        }

        public string DateOfJoining
        {
            get { return moUserDetails.DateOfJoining; }
            set { moUserDetails.DateOfJoining = value; }
        }

        public string Password
        {
            get {
                if (moUserDetails.Login.ToString() != string.Empty)
                    return CommonUtility.GetDecryptedPassword(moUserDetails.Login.ToLower(), moUserDetails.Password);
                else
                    return  string.Empty;
            }
            set { moUserDetails.Password = CommonUtility.GetEncryptedPassword(moUserDetails.Login.ToLower(), value); }
        }

        public Int32 SalutationId
        {
            get { return moUserDetails.Salutation_Id; }
            set { moUserDetails.Salutation_Id = value; }
        }

        public int DesignationId
        {
            get { return moUserDetails.DesignationId; }
            set { moUserDetails.DesignationId = value; }
        }

        public string Phone_Number
        {
            get { return moUserDetails.Phone_Number; }
            set { moUserDetails.Email = value; }
        }

        public string Mobile_Number
        {
            get { return moUserDetails.Mobile_Number; }
            set { moUserDetails.Mobile_Number = value; }
        }

        public string Mobile_Number2
        {
            get { return moUserDetails.Mobile_Number2; }
            set { moUserDetails.Mobile_Number2 = value; }
        }

        public char CanApproveRequisition
        {
            get { return moUserDetails.CanApproveRequisitions; }
            set { moUserDetails.CanApproveRequisitions = value; }
        }

        public char CanSanctionLeave
        {
            get { return moUserDetails.CanSanctionLeave; }
            set { moUserDetails.CanSanctionLeave = value; }
        }

        public char CanCreateGeneralRequisition
        {
            get { return moUserDetails.CanCreateGeneralRequisition; }
            set { moUserDetails.CanCreateGeneralRequisition = value; }
        }

        public char CanReceiveMail
        {
            get { return moUserDetails.CanReceiveMail; }
            set { moUserDetails.CanReceiveMail = value; }
        }

        public bool CanCreateVoucher
        {
            get { return moUserDetails.CanCreateVoucher; }
            set { moUserDetails.CanCreateVoucher = value; }
        }

        public bool CanPublishUnpublishExam
        {
            get { return moUserDetails.CanPublishUnpublishExam; }
            set { moUserDetails.CanPublishUnpublishExam = value; }
        }

        public bool CanApproveVoucher
        {
            get { return moUserDetails.CanApproveVoucher; }
            set { moUserDetails.CanApproveVoucher = value; }
        }

        public bool CanSelfApprove
        {
            get { return moUserDetails.CanSelfApprove; }
            set { moUserDetails.CanSelfApprove = value; }
        }

        public bool CanDeleteVoucher
        {
            get { return moUserDetails.CanDeleteVoucher; }
            set { moUserDetails.CanDeleteVoucher = value; }
        }

        public bool CanEditOldFinancialYear
        {
            get { return moUserDetails.CanEditOldFinancialYear; }
            set { moUserDetails.CanEditOldFinancialYear = value; }
        }

        public bool ShowAllSentSMS
        {
            get { return moUserDetails.ShowAllSentSMS; }
            set { moUserDetails.ShowAllSentSMS = value; }
        }

        public Int32 UserRoleId
        {
            get { return moUserDetails.UserRoleId; }
            set { moUserDetails.UserRoleId = value; }
        }

        public string InsertedBy
        {
            get { return moUserDetails.InsertedBy; }
            set { moUserDetails.InsertedBy = value; }
        }

        public string UpdatedBy
        {
            get { return moUserDetails.UpdatedBy; }
            set { moUserDetails.UpdatedBy = value; }
        }

        public string UpdatedDate
        {
            get { return moUserDetails.mdtUpdateDate; }
            set { moUserDetails.mdtUpdateDate = value; }
        }

        public string User_Role_Name
        {
            get { return moUserDetails.User_Role_Name; }
            set { moUserDetails.User_Role_Name = value; }
        }

        public string SchoolName
        {
            get { return moUserDetails.SchoolName; }
            set { moUserDetails.SchoolName = value; }
        }

        public string sDOB
        {
            get { return moUserDetails.msDOB; }
            set { moUserDetails.msDOB = value; }
        }

        public byte[] BinaryPhotoImage
        {
            get { return moUserDetails.msBinaryPhotoImage; }
            set { moUserDetails.msBinaryPhotoImage = value; }
        }

        public string PhotoFilePath
        {
            get { return moUserDetails.msPhotoFilePath; }
            set { moUserDetails.msPhotoFilePath = value; }
        }

        public bool InternalUser
        {
            get { return moUserDetails.IsInternalUser; }
            set { moUserDetails.IsInternalUser = value; }
        }

       public string AadharCard_Photo_Copy_Path
        {
            get { return moUserDetails.AadharCard_Photo_Copy_Path; }
            set { moUserDetails.AadharCard_Photo_Copy_Path = value; }
        }

       public string BirthCertificateScanCopyFileName
       {
           get { return moUserDetails.BirthCertificateScanCopyFileName; }
           set { moUserDetails.BirthCertificateScanCopyFileName = value; }
       }



        public string AadharCardNo
        {
            get { return moUserDetails.AadharCardNo; }
            set { moUserDetails.AadharCardNo = value; }
        }

        public int SchoolWise_Student_Id
        {
            get { return moUserDetails.SchoolWise_Student_Id; }
            set { moUserDetails.SchoolWise_Student_Id = value; }
        }

        public string StudentName
        {
            get { return moUserDetails.StudentName; }
            set { moUserDetails.StudentName = value; }
        }

        public string StudentNameOnAadharCard
        {
            get { return moUserDetails.StudentNameOnAadharCard; }
            set { moUserDetails.StudentNameOnAadharCard = value; }
        }

        public string MotherTongue
        {
            get { return moUserDetails.MotherTongue; }
            set { moUserDetails.MotherTongue = value; }
        }
       

        #endregion

        #endregion

        #region Overloaded Constructor

        public SchoolUserBL()
        {
            // Contructor for new Buyer
            moSchoolUser = new SchoolUserDC();
            moSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
        }

        public SchoolUserBL(string asLogin, string asPassword)
        {
            // Populate the Buyer's data. If passed Buyer's login is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(asLogin);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(int aiSchoolId, string asLogin, string asPassword)
        {
            // Populate the Buyer's data. If passed Buyer's login is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(aiSchoolId, asLogin);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(Int32 aiUserID)
        {
            // Populate the SchoolUser's data. If passed UserId's  is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(aiUserID);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(int aiUserId, int aiSchoolId)
        {
            // Populate the SchoolUser's data. If passed UserId's  is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(aiUserId, aiSchoolId);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(Int32 aiUserID, bool abIsFeeMessage)
        {
            // Populate the SchoolUser's data. If passed UserId's  is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(aiUserID, abIsFeeMessage);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(Int32 aiUserID, int aiSchoolId, int aiAcademicYearId, bool abIsteacher)
        {
            // Populate the SchoolUser's data. If passed UserId's  is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(aiUserID, aiSchoolId, aiAcademicYearId, abIsteacher);
            moUserDetails = moSchoolUser.SchoolUserInfo;

            if (moSchoolUser.SchoolUserInfo.UserId == 0)
                throw new BuyerNotFoundException("");
        }

        public SchoolUserBL(string asLogin)
        {
            // Populate the Buyer's data. If passed Buyer's login is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(asLogin);
            moUserDetails = moSchoolUser.SchoolUserInfo;
        }

        public SchoolUserBL(string asLogin, string asMobileNo, DateTime odtBirthDate)
        {
            if (!CommonUtility.IsAValidSqlDateTime(odtBirthDate))
                throw new InvalidSqlDateTimeException("Please select a valid date.");

            // Populate the Buyer's data. If passed Buyer's login is not found in the database then 
            // throw an exception. 
            moSchoolUser = new SchoolUserDC(asLogin, asMobileNo, odtBirthDate);
            moUserDetails = moSchoolUser.SchoolUserInfo;
        }

        #endregion

        #region Public Methods

        public string GetInsertSqlStatementForSchoolUser()
        {
            // This method adds new company user.
            moSchoolUser.SchoolUserInfo = moUserDetails;
            //  if (!IsUserLoginDuplicate())
            return moSchoolUser.GetInsertSqlStatementForSchoolUser();
            //else
            //    throw new DuplicateUserException(Constants.S_DUPLICATE_LOGIN_MSG);
        }

        public Int32 InsertSchoolUserDetails()
        {
            // This method adds new company user.
            moSchoolUser.SchoolUserInfo = moUserDetails;
            if (!IsUserLoginDuplicate())
                return moSchoolUser.InsertSchoolUserDetails();

            throw new DuplicateUserException(S_DUPLICATE_LOGIN_MSG);
        }

        public bool UpdateSchoolUser()
        {
            //This function is used to update the school user details.
            moSchoolUser.SchoolUserInfo = moUserDetails;
            if (!IsUserLoginDuplicate())
                return moSchoolUser.UpdateSchoolUser();

            throw new DuplicateUserException(S_DUPLICATE_LOGIN_MSG);
        }

        public bool UpdateOtherStaffSchoolUser(byte[] abImageBinaryData, int aiOtherStaffId, string asPhotoFilePath, string asUserName, string asPassword)
        {
            //This function is used to update the school user details.

            asPassword = CommonUtility.GetEncryptedPassword(asUserName, asPassword);
            moSchoolUser.SchoolUserInfo = moUserDetails;
            //if (!IsUserLoginDuplicate())
            return moSchoolUser.UpdateOtherStaffSchoolUser(abImageBinaryData, aiOtherStaffId, asPhotoFilePath, asUserName, asPassword);
            //else
            //{   
            //    string sErrorMessage = GetErrorMessage();
            //    throw new DuplicateUserException(sErrorMessage);
            //}
        }

        /// <summary>
        /// This method is used to update CanReceiveMail flag in user master table.
        /// </summary>
        public void UpdateSchoolUserReceiveMailFlag()
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolUser.UpdateSchoolUserReceiveMailFlag();
        }

        public bool UpdateSchoolUserPassword()
        {
            //This function is used to update current user's password.
            moSchoolUser.SchoolUserInfo = moUserDetails;
            return moSchoolUser.UpdateSchoolUserPassword();
        }

        public string UpdateUserDetails()
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            return moSchoolUser.UpdateUserDetails();
        }

        public void UpdateFamilyPhotoNumber (int aiUserRoleId)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolUser.UpdateFamilyPhotoNumber(aiUserRoleId);
        }

        public void UpdateStudentAadharNumber(int aiUserRoleId)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolUser.UpdateStudentAadharNumber(aiUserRoleId);
        }

        public Int32 InsertUserDetailsAsTeacher()
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolWiseTeacherMasterBL.UserId = moSchoolUser.GetPrimaryKeyOfInsertedUser();
            UserId = moSchoolWiseTeacherMasterBL.UserId;
            return moSchoolWiseTeacherMasterBL.InsertTeacherDetails();
            //return true;
        }

        /// <summary>
        /// This method is used to get login details list details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public List<UserLoginDetails> GetLoginDetails(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId)
        {
            return moSchoolUser.GetLoginDetails(aiSchoolId, aiAcademicYearId, aiUserRoleId);
        }

        public Int32 UpdateUserDetailsAsTeacher()
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            var oArrayListUpdateStatements = new ArrayList
			                                 	{
			                                 		moSchoolUser.UpdateUserDetails()
			                                 	};
            moSchoolWiseTeacherMasterBL.UserId = moUserDetails.UserId;
            return moSchoolWiseTeacherMasterBL.UpdateTeacherDetails(oArrayListUpdateStatements);
        }

        public void LockParticularUser(int aiUserId, int aiSchoolId, int aiUpdatedById, string asDeactivationReason, int aiConsideredForSMS, int aiUserRoleId, int aiRemoveReferances)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolUser.LockParticularUser(aiUserId, aiSchoolId, aiUpdatedById, asDeactivationReason, aiConsideredForSMS,aiUserRoleId,aiRemoveReferances);
        }

        public void UnLockParticularUser(int aiUserId, int aiSchoolId, int aiUpdatedById, int aiConsideredForSMS)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            moSchoolUser.UnLockParticularUser(aiUserId, aiSchoolId, aiUpdatedById, aiConsideredForSMS);
        }

        public bool IsUserLoginDuplicate()
        {
            // This method calls the DC method to check if the current Buyer login is duplicate or not.
            moSchoolUser.SchoolUserInfo = moUserDetails;
            return moSchoolUser.IsUserLoginDuplicate();
        }

        public static DataSet GetValidUser(int aiSchoolId, string asLogin, string asPassword, string asIPAddress)
        {
            if (asPassword != "")
                asPassword = CommonUtility.GetEncryptedPassword(asLogin, asPassword);
            return SchoolUserDC.GetValidUser(aiSchoolId, asLogin, asPassword, asIPAddress);
        }
        /// <summary>
        /// This is Used To GetUserEnrolmentNumbe
        /// </summary>
        /// <param name="lstUserLoginDetails"></param>        

        public void UpdateStudentLoginDetails(List<UserLoginDetails> lstUserLoginDetails)
        {
              SchoolUserDC oSchoolUserDC = new SchoolUserDC();
              oSchoolUserDC.UpdateStudentLoginDetails(lstUserLoginDetails);
        }

        public static void AddRemoveUserFromSmsMessageList(int aiUserId, bool abRemove, int aiSchoolId, int aiUpdatedById)
        {
            SchoolUserDC.AddRemoveUserFromSmsMessageList(aiUserId, abRemove, aiSchoolId, aiUpdatedById);
        }

        public static DataSet GetValidRITUser(int aiSchoolId, string asLogin, string asPassword, string asIPAddress)
        {
            return SchoolUserDC.GetValidUser(aiSchoolId, asLogin, asPassword, asIPAddress);
        }

        public static void AcceptTerms(int aiUserId, int aiSchoolId)
        {
            SchoolUserDC.AcceptTerms(aiUserId, aiSchoolId);
        }

        public DataTable GetStaffBirthday(int aiSchoolId, int aiAcademicYrId, String sortExpression, int maximumRows, int startRowIndex)
        {
            var oSchoolUserDC = new SchoolUserDC();
            return oSchoolUserDC.GetStaffBirthday(aiSchoolId, aiAcademicYrId, maximumRows, startRowIndex);
        }

        public int GetBirthdayCount(int aiSchoolId, int aiAcademicYrId)
        {
            int iRet = 0;
            var oSchoolUserDC = new SchoolUserDC();

            DataTable oDt = oSchoolUserDC.GetStaffBirthday(aiSchoolId, aiAcademicYrId, 20, 0);
            if (oDt.Rows.Count > 0)
                iRet = oDt.Rows[0]["TotalRows"].ToInt();
            return iRet;
        }

        /// <summary>
        /// 	This method used to update student registration number.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiUserId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="asRegNumber"> </param>
        /// <param name="asXmlStudentsRegNos"> </param>
        public DataTable UpdateStudentRegNoAndLoginPassword(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId, int aiDivisionId, string asRegNumber, string asXmlStudentsRegNos)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            return moSchoolUser.UpdateStudentRegNoAndLoginPassword(aiSchoolId, aiAcademicYearId, aiUserId, aiStandardId, aiDivisionId, asRegNumber, asXmlStudentsRegNos);
        }

        /// <summary>
        /// 	This method is used to get all designation of school users.
        /// </summary>
        public DataTable GetAllDesgnation(int aiSchoolId, string asDesignationIDs, int aiRequisitionByDesignationID)
        {
            moSchoolUser.SchoolUserInfo = moUserDetails;
            return moSchoolUser.GetAllDesgnation(aiSchoolId, asDesignationIDs, aiRequisitionByDesignationID);
        }

        public static DataTable GetAcademicYearForUser(int UserId, int SchoolId, int AcademicYearId)
        {
            return SchoolUserDC.GetAcademicYearForUser(UserId, SchoolId, AcademicYearId);
        }

        /// <summary>
        /// 	Returns the fullname of a user including salutation.
        /// </summary>
        /// <returns> </returns>
        public string GetFullName()
        {
            return moSchoolUser.IsNull() ? null : moSchoolUser.GetFullName();
        }

        /// <summary>
        /// This function is used to get email addresses for forgot password request.
        /// </summary>
        /// <returns></returns>
        public string GetEmailsForForgotPassword(int aiSchoolId)
        {
            return moSchoolUser.GetEmailsForForgotPassword(aiSchoolId);
        }

        /// <summary>
        /// This method is used to get all the teachers into the listview with their associating status.It is called from Object data source
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiGroupId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="asSortDirection"></param>
        /// <returns></returns>
        public List<UserInfo> GetUsersforMailingGroups(int aiSchoolId, int aiAcademicYearId, int aiRoleId, int aiStandardDivId, int maximumRows, int startRowIndex, string sortDirection, String sortExpression,string asFilter)
        {
            if (asFilter == null)
                asFilter = string.Empty;
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moSchoolUser.GetUsersforMailingGroups(aiSchoolId, aiAcademicYearId, aiRoleId,aiStandardDivId, iStartIndex, iEndIndex, sortDirection,asFilter);
        }

        /// <summary>
        /// This method is used to return a teachers count to Object data source. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public int GetUserCountForMailingGroups(int aiSchoolId, int aiAcademicYearId, int aiRoleId, int aiStandardDivId, string sortDirection, String sortExpression,string asFilter)
        {
            if (asFilter == null)
                asFilter = string.Empty;
            return moSchoolUser.GetUserCountForMailingGroups(aiSchoolId, aiAcademicYearId, aiRoleId, aiStandardDivId,asFilter);
        }

        /// <summary>
        /// This method is used to get the user Details for Sending Login details SMS. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public UserDetailsForLoginSMS GetUserDetailsForLogin(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            SchoolUserDC oSchoolUserDC = new SchoolUserDC();
            return oSchoolUserDC.GetUserDetailsForLogin(aiSchoolId,aiAcademicYearId,aiUserId);
        }


        #endregion

        #region private methods

        private string GetErrorMessage()
        {
            return moUserDetails.UserRoleId == Constants.UserRoles.Student.ToInt() ? S_DUPLICATE_REG_NO : S_DUPLICATE_LOGIN_MSG;
        }

        #endregion

        /// <summary>
        /// 	This method is used to check for SuperAdmin user and set the control panal according to that.
        /// </summary>
        /// <param name="iSuperAdminId"> </param>
        /// <param name="aiSchoolId"> </param>
        /// <returns> </returns>
        public int GetSuperAdmin(int iSuperAdminId, int aiSchoolId)
        {
            var oSchoolUserDC = new SchoolUserDC();
            return oSchoolUserDC.GetSuperAdmin(iSuperAdminId, aiSchoolId);
        }

        //public void GetOtherStaffSchoolUserDetails(int aiUserId)
        //{
        //    SchoolUserDC oSchoolUserDC = new SchoolUserDC();
        //    DataTable oDT = oSchoolUserDC.GetOtherStaffSchoolUserDetails(aiUserId);
        //    moUserDetails = oSchoolUserDC.SetOtherStaffUserInformationDetails(oDT); 

        //}

        /// <summary>
        /// This method gives us the count of staff birthday.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public int GetCountOfSchoolStaffBirthDay(int aiSchoolId, int aiAcademicYearId)
        {
            SchoolUserDC oSchoolUserDC = new SchoolUserDC();
            int iTotalBirthdayCount = oSchoolUserDC.GetCountOfSchoolStaffBirthDay(aiSchoolId, aiAcademicYearId);
            return iTotalBirthdayCount;
        }

        /// <summary>
        /// This method is used to return all admin details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<Admin> GetAllAdmins(int aiSchoolId, int aiUserId = 0)
        {
            SchoolUserDC oSchoolUserDC = new SchoolUserDC();
            return oSchoolUserDC.GetAllAdmins(aiSchoolId, aiUserId);
        }

        /// <summary>
        /// this method is used to delete admin details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteAdminDetails(int aiSchoolId, int aiUserId, int aiUpdatedById)
        {
            SchoolUserDC oSchoolUserDC = new SchoolUserDC();
            oSchoolUserDC.DeleteAdminDetails(aiSchoolId, aiUserId, aiUpdatedById);
        }

        /// <summary>
        /// This is used to UserLoginDate & time.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void UpdateLogOutDate(int aiUserId)
        {
            SchoolUserDC oSchoolUserDC = new SchoolUserDC();
            oSchoolUserDC.UpdateLogOutDate(aiUserId);
        }
		
		public DataTable GetAllUsers(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            //return SchoolUserCollectionDC.GetAllUsers(aiUserRoleId);
           SchoolUserDC oSchoolUserDC = new SchoolUserDC();
           return oSchoolUserDC.GetAllUsers(aiUserRoleId, aiSchoolId, aiAcademicYearId);

        }
    }

    public class SchoolUserCollectionBL
    {
        private SchoolUserCollectionDC moSchoolUserCollectionDC;
        private int miUserCount;

        // This method is used to delete Librarian.
        public static string DeleteLibrarian(int aiUserId)
        {
            return DeleteUserDetails(aiUserId);
        }

        // This method is used to delete Supervisor.
        public static string DeleteSupervisor(int aiUserId)
        {
            return DeleteUserDetails(aiUserId);
        }

        /// <summary>
        /// 	This method is used to delete user datails.
        /// </summary>
        /// <param name="aiUserId"> </param>
        /// <returns> </returns>
        private static string DeleteUserDetails(int aiUserId)
        {
            string sDeleteuser = SchoolUserCollectionDC.DeleteUserDetails(aiUserId);
            return sDeleteuser;
        }

        public SchoolUserCollectionBL()
        {
            moSchoolUserCollectionDC = new SchoolUserCollectionDC();
        }

        public static DataSet GetAdminAndprincipalOfSchool(int aiSchoolId, int aiAcadYrId, int aiUserId)
        {
            return SchoolUserCollectionDC.GetAdminUserForTheSchool(aiSchoolId, aiAcadYrId, aiUserId);
        }

        public static Boolean IsAllSentSMSbtnVisibility(int aiUserId)
        {
            return SchoolUserCollectionDC.IsAllSentSMSbtnVisibility(aiUserId);
        }
        public static DataTable GetPasswordRecoveryDetails(int aiUserId, int aiSchoolId)
        {
            return SchoolUserCollectionDC.GetPasswordRecoveryDetails(aiUserId, aiSchoolId);
        }

        public static DataTable GetUserDetails(int aiSchoolId, string asName, int aiUserRolId, int aiAcademicYrId)
        {
            return SchoolUserCollectionDC.GetUserDetails(aiSchoolId, asName, aiUserRolId, aiAcademicYrId);
        }

        public DataTable GetUserAsTeacherDetails(int aiSchoolId, int aiAcademicYearId, string asFilter, string asUserType, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Teacher_Designation_Master.SortOrder ASC ";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moSchoolUserCollectionDC.GetUserAsTeacherDetails(aiSchoolId, aiAcademicYearId, asFilter,asUserType.ToInt(), sortExpression, iEndIndex, iStartIndex);
        }

        public int CountTeachers(int aiSchoolId, int aiAcademicYearId, string asFilter, string asUserType)
        {
            int iCnt = moSchoolUserCollectionDC.CountTeachers(aiSchoolId, aiAcademicYearId, asFilter, asUserType.ToInt());
            return iCnt;
        }

        public DataTable GetTeacherDetailsForPhotoUplaod(int aiSchoolId, int aiAcademicYearId, string asName, bool abPhotoFilePath, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Designation_Id ASC ";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moSchoolUserCollectionDC.GetTeacherDetailsForPhotoUplaod(aiSchoolId, aiAcademicYearId, asName, abPhotoFilePath, sortExpression, iEndIndex, iStartIndex);
        }

        public int CountTeachersForPhotoUplaod(int aiSchoolId, int aiAcademicYearId, string asName, bool abPhotoFilePath)
        {
            int iCnt = moSchoolUserCollectionDC.CountTeachersForPhotoUplaod(aiSchoolId, aiAcademicYearId, asName, abPhotoFilePath);
            return iCnt;
        }

        public bool DeleteMultipleUserWhoAreTeacher(ArrayList aoArrDeleteTeacherIds)
        {
            moSchoolUserCollectionDC.DeleteMultipleUserWhoAreTeacher(aoArrDeleteTeacherIds);
            return true;
        }

        public bool DeleteUserWhoAreTeacher(int aiTeacherId)
        {
            moSchoolUserCollectionDC.DeleteUserWhoAreTeacher(aiTeacherId);
            return true;
        }

        /// <summary>
        /// 	This method is used to get user details.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiUserRoleId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="sortDirection"> </param>
        /// <param name="sortExpression"> </param>
        /// <param name="asCriteria"> </param>
        /// <param name="maximumRows"> </param>
        /// <param name="startRowIndex"> </param>
        /// <returns> DataSet </returns>
        public DataTable GetUserDetails(int aiSchoolId, int aiUserRoleId, int aiUserTypeId, int aiAcademicYearId, string sortDirection, String sortExpression, string asCriteria, int maximumRows, int startRowIndex)
        {
            if ((aiUserRoleId == 2 || aiUserRoleId == 6 || aiUserRoleId == 7) && sortExpression.Contains("First_Name"))
                sortExpression = "Name";

            if (aiUserRoleId != 3 && sortExpression.Contains("Roll_No"))
                sortExpression = "Name";

            DataTable oDt = moSchoolUserCollectionDC.GetUserDetails(aiSchoolId, aiUserRoleId, aiUserTypeId, aiAcademicYearId, sortDirection, sortExpression.Replace("DESC", "").Replace("ASC", ""), asCriteria, maximumRows, startRowIndex);
            if (oDt.Rows.Count > 0)
                miUserCount = oDt.Rows[0]["TotalRows"].ToInt();
            return oDt;
        }

        public int GetCountUsers(int aiSchoolId, int aiUserRoleId, int aiUserTypeId, int aiAcademicYearId, string sortDirection, string asCriteria)
        {
            //int iRet = 0;
            //DataTable oDt = moSchoolUserCollectionDC.GetUserDetails(aiSchoolId, aiUserRoleId, aiAcademicYearId, sortDirection, "", asCriteria, 3, 0);
            //if (oDt.Rows.Count > 0)
            //    iRet = oDt.Rows[0]["TotalRows"].ToInt();
            //return iRet;

            return miUserCount;
        }

        /// <summary>
        /// 	This method get count of user other than admin
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAdminRoleId"> </param>
        /// <returns> </returns>
        public Boolean CheckThatIsAnyUserPresent(int aiSchoolId, int aiAdminRoleId)
        {
            int iCount = moSchoolUserCollectionDC.GetNotAdminUsersCount(aiSchoolId, aiAdminRoleId);
            return iCount > 0;
        }        

        /// <summary>
        /// 	This method is used to get standard and divisionwise user details i.e.student.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiUserRoleId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <returns> DataSet </returns>
        public DataTable GetUserDetails(int aiSchoolId, int aiUserRoleId, int aiStandardId, int aiDivisionId)
        {
            return moSchoolUserCollectionDC.GetUserDetails(aiSchoolId, aiUserRoleId, aiStandardId, aiDivisionId);
        }

        public static DataTable GetAllUsers(int aiSchoolID, int aiAcademicYearId)
        {
            return SchoolUserCollectionDC.GetAllUsers(aiSchoolID, aiAcademicYearId);
        }

        public static DataTable GetAllStudentUsers(int aiSchoolId)
        {
            return SchoolUserCollectionDC.GetAllStudentUsers(aiSchoolId);
        }

        public static DataTable UpdateStudentPasswordsWithRegNo(string asXML)
        {
            return SchoolUserCollectionDC.UpdateStudentPasswordsWithRegNo(asXML);
        }

        public static DataTable GetAllStaffByName(int aiSchoolId, int aiAcademicYrId, string asRegNumbers, int aiMonthId, int aiYear, DateTime adtStartDate, DateTime adtEndDate, int aiFinancialYearId)
        {
            return SchoolUserCollectionDC.GetAllStaffByName(aiSchoolId, aiAcademicYrId, asRegNumbers, aiMonthId, aiYear, adtStartDate, adtEndDate, aiFinancialYearId);
        }

        public static DataTable GetTransportTravellerDetails(int aiSchoolId, int aiAcademicYrId, string asFilter, int aiVehicleId, int aiShift, int aiRouteId, int aiStopId)
        {
            return SchoolUserCollectionDC.GetTransportTravellerDetails(aiSchoolId, aiAcademicYrId, asFilter, aiVehicleId, aiShift, aiRouteId, aiStopId);
        }

        public static DataTable GetAllTravellerDetails(int aiSchoolId, int aiAcademicYrId, string asFilter, int aiUserRoleId, int aiStandard_Id, int aiDivision_Id, int aiUser_Id)
        {
            return SchoolUserCollectionDC.GetAllTravellerDetails(aiSchoolId, aiAcademicYrId, asFilter, aiUserRoleId, aiStandard_Id, aiDivision_Id, aiUser_Id);
        }

        public static DataTable GetAllStaffForInvestmentDeclaration(int aiSchoolId, int aiAcademicYearId, string asSearchCriteria, int aiStaffGroupId)
        {
            return SchoolUserCollectionDC.GetAllStaffForInvestmentDeclaration(aiSchoolId, aiAcademicYearId, asSearchCriteria, aiStaffGroupId);
        }

        public static List<UserSMS> GetUserLoginDetails(int aiSchoolId, int aiAcademicYearId, int aiFlag)
        {
            return SchoolUserCollectionDC.GetUserLoginDetails(aiSchoolId, aiAcademicYearId, aiFlag);
        }

        /// <summary>
        /// This method is used to get teacher information.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetAllTeacherDetails(int aiSchoolId, int aiAcademicYearId, string asFilter, string sortExpression, int maximumRows, int startRowIndex,int aiuserType)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "TDM.SortOrder ASC ";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moSchoolUserCollectionDC.GetAllTeacherDetails(aiSchoolId, aiAcademicYearId, asFilter, sortExpression, iEndIndex, iStartIndex, aiuserType);
        }
    }

    // This exception is thrown when the password of the Buyer is not correct. 
    public class InvalidBuyerLoginException : InvalidLoginException
    {
        private string msMessage = "";

        public override string Message
        {
            get { return msMessage; }
        }

        public InvalidBuyerLoginException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }

    // This exception is thrown when the Buyer login is not found.
    public class BuyerNotFoundException : LoginNotFoundException
    {
        private string msMessage = "";

        public override string Message
        {
            get { return msMessage; }
        }

        public BuyerNotFoundException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }
}