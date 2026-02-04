/*
 *  File Name : -- SchoolUserDC.cs
 *  Purpose   : -- This file is used to perform all database related operation of School user.
 *  Date      : -- 07-May-2007
 */


using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Utility;
using MasterEntities;
using System.Web;
using System.Configuration;
using SchoolEntities;
using SchoolEntities.Admin;
using SchoolEntities.Dashboard;

namespace DataCommunicator
{
    public class SchoolUserDC : DataCommunicatorBaseDC
    {
        #region Constants & Structrue

        public struct UserDetails
        {
            public Int32 UserId;
            public Int32 SchoolId;

            public string Login;
            public string Email;

            public string BloodGroup;

            public string DateOfJoining;
            public string Password;
            public string FirstName;
            public string MiddleName;
            public string LastName;
            public string Address;
            public string EmergencyContact;

            public string Phone_Number;
            public string Mobile_Number;
            public string Mobile_Number2;
            public Int32 Salutation_Id;
            public Char CanApproveRequisitions;
            public Char CanCreateGeneralRequisition;
            public Char CanSanctionLeave;
            public Char CanReceiveMail;
            public bool CanCreateVoucher;
            public bool CanPublishUnpublishExam;
            public bool CanApproveVoucher;
            public bool CanSelfApprove;
            public bool CanDeleteVoucher;
            public bool CanEditOldFinancialYear;

            public string AadharCard_Photo_Copy_Path;
            public string BirthCertificateScanCopyFileName;
            public string AadharCardNo;
            public int SchoolWise_Student_Id;
            public string StudentName;
            public string StudentNameOnAadharCard;
            public string MotherTongue;

            public string Family_Photo_Copy_Path;

            public Int32 UserRoleId;
            public int DesignationId;

            public string InsertedBy;
            public string UpdatedBy;

            public string SchoolName;
            public string User_Role_Name;
            public string msDOB;
            public string msPhotoFilePath;
            public byte[] msBinaryPhotoImage;
            public string mdtUpdateDate;

            public bool IsInternalUser;

            public bool ShowAllSentSMS;

        }

        #endregion

        #region Data Members & Propeties

        private UserDetails moUserDetails;

        public UserDetails SchoolUserInfo
        {
            get { return moUserDetails; }
            set { moUserDetails = value; }
        }

        public string ShowAllSentSMS
        {
            get { return (moUserDetails.ShowAllSentSMS ? Constants.S_ONE : Constants.S_ZERO); }
        }
        #endregion

        #region Overloaded Constructor

        public SchoolUserDC()
        {
            moUserDetails.UserId = 0;
        }

        // Overloaded contructor for edit.
        public SchoolUserDC(String asLogin)
        {
            if (PopulateSchoolUserDetails(asLogin) == false)
                moUserDetails.UserId = 0;
        }

        public SchoolUserDC(Int32 aiUserID)
        {
            if (PopulateSchoolUserDetails(aiUserID) == false)
                moUserDetails.UserId = 0;
        }

        public SchoolUserDC(int aiUserId, int aiSchoolId)
        {
            LoadSchoolwiseStudentMasterDetails(aiUserId, aiSchoolId);
        }

        public SchoolUserDC(Int32 aiUserID, int aiSchoolId, int aiAcademicYrId, bool abIsteacher)
        {
            if (PopulateSchoolUserDetails(aiUserID, aiSchoolId, aiAcademicYrId, abIsteacher) == false)
                moUserDetails.UserId = 0;
        }

        public SchoolUserDC(Int32 aiSchoolId, String asLogin)
        {
            if (PopulateSchoolUserDetails(aiSchoolId, asLogin) == false)
                moUserDetails.UserId = 0;
        }

        public SchoolUserDC(Int32 aiSchoolId, bool abIsFeeMessage)
        {
            if (PopulateSchoolUserDetails(aiSchoolId, abIsFeeMessage) == false)
                moUserDetails.UserId = 0;
        }

        public SchoolUserDC(String asLogin, string asMobileNo, DateTime adtBIrthDate)
        {
            if (PopulateSchoolUserDetails(asLogin, asMobileNo, adtBIrthDate) == false)
                moUserDetails.UserId = 0;
        }

        #endregion

        #region Public Methods



        public bool UpdateSchoolUser()
        {
            int sReturnValue;
            string sUpdateUserSql;
            var sSalutation = new StringBuilder();
            string sPassword = "";

            if (moUserDetails.msDOB != "")
                sSalutation.Append(", DOB = N'" + moUserDetails.msDOB + "'");
            else
                sSalutation.Append(", DOB = NULL");

            if (moUserDetails.Salutation_Id != 0)
                sSalutation.Append(" , Salutation_Id = N'" + moUserDetails.Salutation_Id + "'");

            if (moUserDetails.UserRoleId != (int)Constants.UserRoles.Student || (moUserDetails.UserRoleId == (int)Constants.UserRoles.Student && moUserDetails.Password != null))
                sPassword = " , user_password = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'";

            if (!String.IsNullOrEmpty(moUserDetails.msPhotoFilePath) || (HttpContext.Current.Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && HttpContext.Current.Session[Constants.S_SESSION_IS_BUTTON_CLOSE] != null))
            {
                sSalutation.Append(", PhotoFilePath = N'" + moUserDetails.msPhotoFilePath + "'");
                //", BinaryPhotoImage = '" + moUserDetails.msBinaryPhotoImage + "'");          

                // This method UpdateSchool users details.
                sUpdateUserSql = " UPDATE user_master " +
                                 " SET " +
                                 " user_role_id = " + moUserDetails.UserRoleId +
                                 " , user_login = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                 " , user_first_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, false) + "'" +
                                 " , user_middle_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                 " , user_last_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, false) + "'" +
                                 " , Address =N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, false) + "'" +
                                 sPassword +
                                 " , email_address = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                 " , Mobile_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Mobile_Number, true) + "'" +
                                 " , EmergencyContactNumber = N'" + moUserDetails.EmergencyContact + "'" +
                                 " , DesignationId =  " + moUserDetails.DesignationId +
                                 " , CanApproveRequisition = N'" + moUserDetails.CanApproveRequisitions + "'" +
                                 " , CanCreateGeneralRequisition = N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                 " , CanSanctionLeave = N'" + moUserDetails.CanSanctionLeave + "'" +
                                 " , CanReceiveMail = N'" + moUserDetails.CanReceiveMail + "'" +
                                 " , CanCreateVoucher = N'" + moUserDetails.CanCreateVoucher + "'" +
                                 " , CanApproveVoucher = N'" + moUserDetails.CanApproveVoucher + "'" +
                                 " , CanSelfApprove = N'" + moUserDetails.CanSelfApprove + "'" +
                                 " , CanDeleteVoucher = N'" + moUserDetails.CanDeleteVoucher + "'" +
                                 " , CanEditOldFinancialYear = N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                 " , CanPublishUnpublishExam = N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                 " , updated_by_id = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                 " , update_date = N'" + moUserDetails.mdtUpdateDate + "'" +
                                 " , IsInternalUser = N'" + moUserDetails.IsInternalUser + "'" +
                                 sSalutation +
                                 ", BinaryPhotoImage = @Image " +
                                 ", ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                                 ", ShowAllSentSMS = " + ShowAllSentSMS + "" +
                                 " WHERE " +
                                 " user_id = " + moUserDetails.UserId +
                                 " AND is_deleted= N'" + Constants.C_NO + "'";

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    sReturnValue = oSQLServerDbUtility.ExecuteTransaction(moUserDetails.msBinaryPhotoImage, sUpdateUserSql);
            }
            else
            {
                // This method UpdateSchool users details.
                sUpdateUserSql = " UPDATE user_master " +
                                 " SET " +
                                 " user_role_id = " + moUserDetails.UserRoleId +
                                 " , user_login = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                 " , user_first_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, false) + "'" +
                                 " , user_middle_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                 " , user_last_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, false) + "'" +
                                 " , Address =N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, false) + "'" +
                                 sPassword +
                                 " , email_address = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                 " , Mobile_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Mobile_Number, true) + "'" +
                                 " , EmergencyContactNumber = N'" + moUserDetails.EmergencyContact + "'" +
                                 " , DesignationId =  " + moUserDetails.DesignationId +
                                 " , CanApproveRequisition = N'" + moUserDetails.CanApproveRequisitions + "'" +
                                 " , CanCreateGeneralRequisition = N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                 " , CanSanctionLeave = N'" + moUserDetails.CanSanctionLeave + "'" +
                                 " , CanReceiveMail = N'" + moUserDetails.CanReceiveMail + "'" +
                                 " , CanCreateVoucher = N'" + moUserDetails.CanCreateVoucher + "'" +
                                 " , CanApproveVoucher = N'" + moUserDetails.CanApproveVoucher + "'" +
                                 " , CanSelfApprove = N'" + moUserDetails.CanSelfApprove + "'" +
                                 " , CanDeleteVoucher = N'" + moUserDetails.CanDeleteVoucher + "'" +
                                 " , CanEditOldFinancialYear = N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                 " , CanPublishUnpublishExam = N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                 " , updated_by_id = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                 " , update_date = N'" + moUserDetails.mdtUpdateDate + "'" +
                                 " , IsInternalUser = N'" + moUserDetails.IsInternalUser + "'" +
                                 sSalutation +
                                 " , ShowAllSentSMS = " + ShowAllSentSMS + "" +
                                   " WHERE " +
                                 " user_id = " + moUserDetails.UserId +
                                 " AND is_deleted= N'" + Constants.C_NO + "'";

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    sReturnValue = oSQLServerDbUtility.ExecuteTransaction(sUpdateUserSql);
            }
            UpdateAdminDesignation(moUserDetails.DesignationId, moUserDetails.UserId);
            return sReturnValue != 0;
        }

        // This function is used to load the Schoolwise_Student_Master Details
        private void LoadSchoolwiseStudentMasterDetails(int miEventId, int SchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", miEventId, SqlDbType.Int);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForAadharCardNo"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["AadharCard_Photo_Copy_Path"] != DBNull.Value)
                                moUserDetails.AadharCard_Photo_Copy_Path = Convert.ToString(oDR["AadharCard_Photo_Copy_Path"]);
                            if (oDR["AadharCardNo"] != DBNull.Value)
                                moUserDetails.AadharCardNo = Convert.ToString(oDR["AadharCardNo"]);
                            if (oDR["User_Id"] != DBNull.Value)
                                moUserDetails.UserId = Convert.ToInt32(oDR["User_Id"]);
                            if (oDR["SchoolWise_Student_Id"] != DBNull.Value)
                                moUserDetails.SchoolWise_Student_Id = Convert.ToInt32(oDR["SchoolWise_Student_Id"]);
                            if (oDR["StudentFullName"] != DBNull.Value)
                                moUserDetails.StudentName = Convert.ToString(oDR["StudentFullName"]);
                            if (oDR["NameOnAadharCard"] != DBNull.Value)
                                moUserDetails.StudentNameOnAadharCard = Convert.ToString(oDR["NameOnAadharCard"]);
                            if (oDR["Mother_Tongue"] != DBNull.Value)
                                moUserDetails.MotherTongue = Convert.ToString(oDR["Mother_Tongue"]);
                            if (oDR["Email_Address"] != DBNull.Value)
                                moUserDetails.Email = Convert.ToString(oDR["Email_Address"]);
                            if (oDR["Blood_Group"] != DBNull.Value)
                                moUserDetails.BloodGroup = Convert.ToString(oDR["Blood_Group"]);

                            if (SchoolId == Constants.SchoolId.SNS.ToInt())
                            {
                                if (oDR["BirthCertificateScanCopyFileName"] != DBNull.Value)
                                    moUserDetails.BirthCertificateScanCopyFileName = Convert.ToString(oDR["BirthCertificateScanCopyFileName"]);
                            }
                        }
                    }
                }
            }
        }

        // This function is used to fetch the Schoolwise_Student_Master Details
        private string FetchSchoolwiseStudentMaster(int aiUserId, int aiSchoolId)
        {
            string sSelectStatement = " SELECT  " +
            "School_Id, SchoolWise_Student_Id, User_Id, AadharCardNo, AadharCard_Photo_Copy_Path, NameOnAadharCard, Mother_Tongue, Blood_Group" +
            "SM.Salutation_Name + N' ' + SchoolWise_Student_Master.First_Name + N' ' + SchoolWise_Student_Master.Middle_Name + N' ' + SchoolWise_Student_Master.Last_Name AS StudentFullName" +
            " FROM SchoolWise_Student_Master" +
            "  INNER JOIN Salutation_Master AS SM ON SchoolWise_Student_Master.Salutation_Id = SM.Salutation_Id " +
            " WHERE User_Id = " + aiUserId + " AND School_Id = " + aiSchoolId;
            return sSelectStatement;
        }

        private void UpdateAdminDesignation(int aiDesignationId, int aiUserId)
        {
            if (aiDesignationId == Convert.ToInt32(Constants.AdminDesignations.ChiefAdministratorOfficer))
            {
                string sUpdateStatement = "UPDATE user_master" +
                                           " SET " +
                                           "DesignationId = " + Constants.AdminDesignations.ExAdmin.ToInt() +
                                           ",User_Role_Id = " + Constants.UserRoles.ExAdmin.ToInt() +
                                           ", Is_Locked = 'Y'" +
                                           " WHERE " +
                                           " User_Role_Id = " + Constants.UserRoles.Admin.ToInt() +
                                           " AND IsSuperAdmin = 0" +
                                           " AND User_Id <> " + aiUserId;
                sUpdateStatement = sUpdateStatement + ";" +
                                            "UPDATE user_master" +
                                           " SET " +
                                           " Is_Locked = 'N'" +
                                           " WHERE " +
                                           " User_Role_Id = " + Constants.UserRoles.Admin.ToInt() +
                                           " AND IsSuperAdmin = 0" +
                                           " AND User_Id = " + aiUserId;
                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
            }
            else if (aiDesignationId == Convert.ToInt32(Constants.AdminDesignations.ExAdmin))
            {
                string sUpdateStatement = "UPDATE user_master" +
                                           " SET " +
                                           " Is_Locked = 'Y'" +
                                           " WHERE " +
                                           " IsSuperAdmin = 0" +
                                           " AND User_Id = " + aiUserId;
                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
            }
            else
            {
                string sUpdateStatement = "UPDATE user_master" +
                                              " SET " +
                                              " Is_Locked = 'N'" +
                                              " WHERE " +
                                              " IsSuperAdmin = 0" +
                                              " AND User_Id = " + aiUserId;
                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
            }
        }

        public bool UpdateOtherStaffSchoolUser(Byte[] imageBinaryData, int aiOtherStaffId, string asPhotoFilePath, string asUserName, string asPassword)
        {
            var sSalutation = new StringBuilder();
            //string sPassword = "";

            if (moUserDetails.msDOB != "")
                sSalutation.Append(", DOB = N'" + moUserDetails.msDOB + "'");
            else
                sSalutation.Append(", DOB = NULL");

            if (moUserDetails.Salutation_Id != 0)
                sSalutation.Append(" , Salutation_Id = N'" + moUserDetails.Salutation_Id + "'");

            string sUpdateUserSql = " UPDATE user_master " +
                                    " SET " +
                                    " user_role_id = " + moUserDetails.UserRoleId +
                                    " , user_first_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, false) + "'" +
                                    " , user_middle_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                    " , user_last_name = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, false) + "'" +
                                    " , Address =N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, false) + "'" +
                                    " , user_login =N'" + StringUtility.ReplaceSingleQuoteInString(asUserName, false) + "'" +
                                    " , user_password =N'" + StringUtility.ReplaceSingleQuoteInString(asPassword, false) + "'" +
                                    " , email_address = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                    " , Mobile_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Mobile_Number, true) + "'" +
                                    " , EmergencyContactNumber =N'" + moUserDetails.EmergencyContact + "'" +
                                    " , DesignationId =  " + moUserDetails.DesignationId +
                                    " , updated_by_id = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                    " , update_date = N'" + moUserDetails.mdtUpdateDate + "'" +
                                    sSalutation +
                                    " WHERE " +
                                    " user_id = " + moUserDetails.UserId +
                                    " AND is_deleted= N'" + Constants.C_NO + "'";
            string sUpdateStatement;
            int sReturnValue;
            if (!String.IsNullOrEmpty(asPhotoFilePath) || HttpContext.Current.Session["UserImageData"] != null)
            {
                string sPhotoUpdate = ", PhotoFilePath= N'" + asPhotoFilePath + "'";
                //", BinaryPhotoImage = '" + ImageBinaryData + "'";            

                sUpdateStatement = " UPDATE OtherStaff SET " +
                                   " SalutationId = " + moUserDetails.Salutation_Id +
                                   ",FirstName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, false) + "'" +
                                   ",MiddleName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, false) + "'" +
                                   ",LastName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, false) + "'" +
                                   " , Address =N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, false) + "'" +
                                   ",DateOfBirth = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.msDOB, false) + "'" +
                                   ",MobileNo = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Mobile_Number, false) + "'" +
                                   ",EmailId = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                   ",DesignationId = " + moUserDetails.DesignationId +
                                   ",UpdatedDate = N'" + DateTime.Now.ToShortDateString() + "' " +
                                   ",UpdatedById = " + moUserDetails.InsertedBy +
                                   sPhotoUpdate +
                                   ",BinaryPhotoImage = @Image " +
                                   ",ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                                   " WHERE OtherStaffId =" + aiOtherStaffId +
                                   " AND SchoolId = " + moUserDetails.SchoolId +
                                   " AND Is_Deleted = N'" + Constants.C_NO + "'";
                sUpdateUserSql = sUpdateUserSql + "; " + sUpdateStatement;

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    sReturnValue = oSQLServerDbUtility.ExecuteTransaction(imageBinaryData, sUpdateUserSql);
            }
            else
            {
                sUpdateStatement = " UPDATE OtherStaff SET " +
                                   " SalutationId = " + moUserDetails.Salutation_Id +
                                   ",FirstName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, false) + "'" +
                                   ",MiddleName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, false) + "'" +
                                   ",LastName = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, false) + "'" +
                                   ",Address =N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, false) + "'" +
                                   ",DateOfBirth = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.msDOB, false) + "'" +
                                   ",MobileNo = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Mobile_Number, false) + "'" +
                                   ",EmailId = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                   ",DesignationId = " + moUserDetails.DesignationId +
                                   ",UpdatedDate = N'" + DateTime.Now.ToString("MM/dd/yyyy") + "' " +
                                   ",UpdatedById = " + moUserDetails.InsertedBy +
                                   " WHERE OtherStaffId =" + aiOtherStaffId +
                                   " AND SchoolId = " + moUserDetails.SchoolId +
                                   " AND Is_Deleted = N'" + Constants.C_NO + "'";

                sUpdateUserSql = sUpdateUserSql + "; " + sUpdateStatement;

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    sReturnValue = oSQLServerDbUtility.ExecuteTransaction(sUpdateUserSql);
            }

            return sReturnValue != 0;
        }

        /// <summary>
        /// This method is used to update CanReceiveMail flag in user master table.
        /// </summary>
        public void UpdateSchoolUserReceiveMailFlag()
        {
            //This method Updates password of current user.
            string sUpdateUserSql = " UPDATE user_master " +
                                    " SET " +
                                    " CanReceiveMail = N'" + moUserDetails.CanReceiveMail + "'" +
                                    " ,Email_Address = N'" + moUserDetails.Email + "'" +
                                    " WHERE " +
                                    " user_id = " + moUserDetails.UserId +
                                    " AND is_deleted= N'" + Constants.C_NO + "'";


            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateUserSql);

        }


        public bool UpdateSchoolUserPassword()
        {
            //This method Updates password of current user.
            string sUpdateUserSql = " UPDATE user_master " +
                                    " SET " +
                                    " user_password = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'" +
                                    " , update_date =N'" + moUserDetails.mdtUpdateDate + "'" +
                                    ", LastPasswordChangeDate = N'" + DateTime.Now + "'" +
                                    " WHERE " +
                                    " user_id = " + moUserDetails.UserId +
                                    " AND is_deleted= N'" + Constants.C_NO + "'";

            int sReturnValue;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                sReturnValue = oSQLServerDbUtility.ExecuteTransaction(sUpdateUserSql);
            return sReturnValue != 0;
        }

        public string UpdateUserDetails()
        {
            string sUpdateUserSql = " UPDATE user_master " +
                                    " SET " +
                                    " user_role_id = " + moUserDetails.UserRoleId +
                                    " , user_login = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                    " , user_password = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'" +
                                    " , EmergencyContactNumber = N'" + moUserDetails.EmergencyContact + "' " +
                                    " , CanApproveRequisition = N'" + moUserDetails.CanApproveRequisitions + "'" +
                                    " , CanCreateGeneralRequisition = N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                    " , CanSanctionLeave = N'" + moUserDetails.CanSanctionLeave + "'" +
                                    " , CanCreateVoucher = N'" + moUserDetails.CanCreateVoucher + "'" +
                                    " , CanApproveVoucher = N'" + moUserDetails.CanApproveVoucher + "'" +
                                    " , CanSelfApprove = N'" + moUserDetails.CanSelfApprove + "'" +
                                    " , CanDeleteVoucher = N'" + moUserDetails.CanDeleteVoucher + "'" +
                                    " , CanEditOldFinancialYear = N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                    " , CanPublishUnpublishExam = N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                    " , email_address = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                    " , updated_by_Id = N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                    " , IsInternalUser = N'" + moUserDetails.IsInternalUser + "'" +
                                    " , ShowAllSentSMS = " + ShowAllSentSMS + "" +


                                    " WHERE " +
                                    " user_id = " + moUserDetails.UserId +
                                    " AND is_deleted= N'" + Constants.C_NO + "'";
            return sUpdateUserSql;
        }

        public string GetInsertSqlStatementForSchoolUser()
        {
            // This method returns the insert sql statement for School user.This method is used
            // while default user is created for newly regiestered School.
            return GetSchoolUserInsertStatement();
        }

        public int GetPrimaryKeyOfInsertedUser()
        {
            string sInsertQuery = GetSchoolUserInsertStatement();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertQuery);
        }

        public bool IsUserLoginDuplicate()
        {
            // This method checks if the speicified Buyer login is duplicate or not.
            var sSelectStatement = new StringBuilder();
            string sWhereClause;

            // Actual select statement.
            string sLogin = StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login.Trim(), false);

            sSelectStatement.Append(" SELECT " +
                                    " count(*) " +
                                    " FROM " +
                                    " User_Master " +
                                    " WHERE " +
                                    " User_Login =N'" + sLogin + "'");

            /* If the Buyer id is not zero that means duplication is being checked for existing Buyer. 
             * Thus, the same Buyer login should be excluded from the duplicate check. 
             */
            if (moUserDetails.SchoolId != 0)
            {
                // not a new Buyer
                sWhereClause = " AND School_Id =" + moUserDetails.SchoolId;
                sSelectStatement.Append(sWhereClause);
            }

            if (moUserDetails.UserId != 0)
            {
                // not a new Buyer
                sWhereClause = " AND User_Id <> " + moUserDetails.UserId;
                sSelectStatement.Append(sWhereClause);
            }
            // Perform the stetement on server.
            int iCount;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement.ToString());

            if (iCount == 0)
            {
                sSelectStatement.Clear();
                sSelectStatement.AppendFormat("SELECT COUNT(User_Id) FROM dbo.Super_Admin where Login_Name = '{0}' and Is_Deleted = 0 {1}",
                                               sLogin,
                                               moUserDetails.UserId != 0 ? " AND User_Id <> " + moUserDetails.UserId : String.Empty);

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                    iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement.ToString());

                return iCount != 0;
            }

            // If the count is zero there is no duplication of Buyer login. 
            return iCount != 0;
        }

        public static DataSet GetValidUser(int aiSchoolId, string asLogin, string asPassword, string asIPAddress)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("User_Name", asLogin, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Password", asPassword, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IPAddress", asIPAddress, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLoginDetailsOfUser");
            }
        }

        /// <summary>
        /// This method is used to UpdateStudentUserName&Password
        /// </summary>
        /// <param name="lstUserLoginDetails"></param>        
        public void UpdateStudentLoginDetails(List<UserLoginDetails> lstUserLoginDetails)
        {
            ArrayList arrStudentDetails = new ArrayList();

            foreach (var std in lstUserLoginDetails)
            {
                arrStudentDetails.Add("UPDATE User_Master SET User_Login = '" + std.UserLogin + "', User_Password = '" + std.Password + "' WHERE User_Id = " + std.UserId + "AND Is_Deleted = 'N' AND User_Role_Id = 3");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])arrStudentDetails.ToArray(typeof(string)));
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
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAcademicYearwiseLoginDetails"))
                    return LoadDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to add login details to list object.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<UserLoginDetails> LoadDetails(SqlDataReader aoSqlDataReader)
        {
            List<UserLoginDetails> lstUserLoginDetails = new List<UserLoginDetails>();
            UserLoginDetails oUserLoginDetails;
            while (aoSqlDataReader.Read())
            {
                oUserLoginDetails = new UserLoginDetails()
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                    MobileNumber1 = Convert.ToString(aoSqlDataReader["MobileNumber1"]),
                    MobileNumber2 = Convert.ToString(aoSqlDataReader["MobileNumber2"]),
                    UserLogin = Convert.ToString(aoSqlDataReader["UserLogin"]),
                    Password = CommonUtility.GetDecryptedPassword(Convert.ToString(aoSqlDataReader["UserLogin"]), Convert.ToString(aoSqlDataReader["UserPassword"])),
                    ClassName = Convert.ToString(aoSqlDataReader["ClassName"])
                };
                lstUserLoginDetails.Add(oUserLoginDetails);
            }
            return lstUserLoginDetails;
        }

        public void LockParticularUser(int aiUserId, int aiSchoolId, int aiUpdatedById, string asDeactivationReason, int aiConsideredForSMS, int aiUserRoleId, int aiRemoveReferances)
        {
            string sQuery = "UPDATE User_Master SET " +
                            " Is_Locked = N'" + Constants.C_YES + "'" +
                            " ,Deactivation_Reason = N'" + StringUtility.ReplaceSingleQuoteInString(asDeactivationReason, true) + "'" +
                            " ,Updated_By_Id = " + aiUpdatedById +
                            " ,Update_Date = N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                            " ,IsConsideredForMessage=" + aiConsideredForSMS + " " +
                            " WHERE " +
                            " school_id = " + aiSchoolId +
                            " AND is_deleted = N'" + Constants.C_NO + "'" +
                            " AND User_Id=" + aiUserId;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteTransaction(sQuery);
                if (aiUserRoleId == Constants.I_TWO && aiRemoveReferances == Constants.I_ONE)
                {
                    oSQLServerDbUtility.AddParameter("User_Id", aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_RemoveTeacherAllReferances");
                }
            }
        }

        public void UnLockParticularUser(int aiUserId, int aiSchoolId, int aiUpdatedById, int aiConsideredForSMS)
        {
            string sQuery = "UPDATE User_Master SET " +
                            " Is_Locked = N'" + Constants.C_NO + "'" +
                            " ,Updated_By_Id = " + aiUpdatedById +
                            " ,Update_Date = N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                            " ,IsConsideredForMessage=N'" + aiConsideredForSMS + "'" +
                            " WHERE " +
                            " school_id = " + aiSchoolId +
                            " AND is_deleted = N'" + Constants.C_NO + "'" +
                            " AND User_Id=" + aiUserId;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery);
        }

        public static void AddRemoveUserFromSmsMessageList(int aiUserId, bool abRemove, int aiSchoolId, int aiUpdatedById)
        {
            string sSqlStatement = String.Format("UPDATE User_Master SET IsConsideredForMessage = {0}, Updated_By_Id = {3} WHERE User_Id = {1} AND School_Id = {2}",
                                                 abRemove.ToInt(),
                                                 aiUserId,
                                                 aiSchoolId,
                                                 aiUpdatedById);
            using (var oSQLDbUtility = new SQLServerDbUtility())
                oSQLDbUtility.ExecuteTransaction(sSqlStatement);
        }

        public static void AcceptTerms(int aiUserId, int aiSchoolId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_AcceptTermsOfUse");
            }
        }

        public DataTable GetStaffBirthday(int aiSchoolId, int aiAcademicYrId, int maximumRows, int startRowIndex)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PazeSize", maximumRows, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_SchoolStaffBirthDay");
            }
        }

        /// <summary>
        /// 	This method is used to get all designation of school users.
        /// </summary>
        /// <returns> </returns>
        public DataTable GetAllDesgnation(int aiSchoolId, string asDesignationIDs, int aiRequisitionByDesignationID)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchool_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sDesignationIDs", asDesignationIDs, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("iRequisitionByDesignationID", aiRequisitionByDesignationID, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllDesignations");
            }
        }

        public static DataTable GetAcademicYearForUser(int UserId, int SchoolId, int AcademicYearId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", AcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicYearForUser");
            }
        }

        /// <summary>
        /// 	Returns the fullname of a user including salutation.
        /// </summary>
        /// <returns> </returns>
        public string GetFullName()
        {
            if (moUserDetails.UserId == 0 || moUserDetails.UserRoleId == 0)
                return null;

            string sSqlStatement = String.Format("SELECT dbo.Udf_GetUserName({0}, {1}) [Name]", moUserDetails.UserId, moUserDetails.UserRoleId);

            using (var oSqlDbUtility = new SQLServerDbUtility())
                return oSqlDbUtility.PerformStringQueryOnSqlServer(sSqlStatement);
        }

        /// <summary>
        /// This function is used to get email addresses for forgot password request.
        /// </summary>
        /// <returns></returns>
        public string GetEmailsForForgotPassword(int aiSchoolId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Emails", Constants.I_ZERO, SqlDbType.NVarChar, ParameterDirection.Output, 500);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetEmailsForForgotPassword");
                if (oSqlParameter.Value != DBNull.Value)
                    return oSqlParameter.Value.ToString();
                return string.Empty;
            }
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
        public List<UserInfo> GetUsersforMailingGroups(int aiSchoolId, int aiAcademicYearId, int aiRoleId, int aiStandardDivId, int aiStartIndex, int aiEndIndex, string asSortDirection,string asFilter)
        {
            List<UserInfo> lstUserInfo = new List<UserInfo>();
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("RoleId", aiRoleId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSqlDBUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSqlDBUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSqlDBUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("usp_GetUsers"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUserInfo.Add(new UserInfo
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            UserName = oSqlDataReader["UserName"].ToString(),
                            IsInGroup = oSqlDataReader["IsInGroup"].ToBool(),
                            IsDeactivated = oSqlDataReader["IsDeactivated"].ToBool()
                        });
                    }
                }
                return lstUserInfo;
            }
        }

        /// <summary>
        /// This method is used to return a teachers count to Object data source. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public int GetUserCountForMailingGroups(int aiSchoolId, int aiAcademicYearId, int aiRoleId, int aiStandardDivId,string asFilter)
        {
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("RoleId", aiRoleId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParam = oSqlDBUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSqlDBUtility.ExecuteStoredProcedureOnServer("usp_CountUsers");
                return Convert.ToInt32(oSqlParam.Value);
            }
        }

        /// <summary>
        /// this method is used to get upcoming staff birthday list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asView"></param>
        /// <returns></returns>
        public static List<StaffDetails> GetUpcomingStaffBdayList(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, string asView, bool abIsServiceCall = false)
        {
            List<StaffDetails> lstStaffBirthdayDetailsList = new List<StaffDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Duration", asView, SqlDbType.VarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllBirthdays"))
                {
                    StaffDetails oStaffBirthdayDetailsList = null;
                    while (oSqlDataReader.Read())
                    {
                        oStaffBirthdayDetailsList = new StaffDetails()
                        {
                            Date = oSqlDataReader["DOB"].ToString(),
                            UserName = oSqlDataReader["Name"].ToString(),
                            PhotoPath = Convert.ToBase64String(oSqlDataReader["BinaryPhotoImage"] as byte[])

                        };

                        lstStaffBirthdayDetailsList.Add(oStaffBirthdayDetailsList);
                    }
                }
            }

            return lstStaffBirthdayDetailsList;
        }

        /// <summary>
        /// Get albums details to show in photo gallery.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiMonth"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public static List<PhotoGalley> GetAlbumsList(int aiSchoolId, int aiMonth, int aiYear, int aiUserId, bool iFirstLoad, bool abIsServiceCall = false)
        {
            List<PhotoGalley> lstAlbums = new List<PhotoGalley>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month", aiMonth, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SetPreviousMonth", Convert.ToInt32(iFirstLoad), SqlDbType.Int);


                string sGallaryName = string.Empty;
                int iGallaryId = 0;
                int PreviousMonth = aiMonth;
                int PreviousYear = aiYear;
                List<ImageDetails> lstImageDetails = null;
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMonthwisePhotoGallery"))
                {
                    while (oSqlDataReader.Read())
                    {
                        if (sGallaryName != oSqlDataReader["GalleryName"].ToString())
                        {
                            if (!string.IsNullOrEmpty(sGallaryName))
                            {
                                lstAlbums.Add(new PhotoGalley() { Id = iGallaryId, Name = sGallaryName, ImageList = lstImageDetails }); ;
                            }

                            sGallaryName = oSqlDataReader["GalleryName"].ToString();
                            iGallaryId = Convert.ToInt32(oSqlDataReader["Id"]);
                            lstImageDetails = new List<ImageDetails>();

                        }

                        lstImageDetails.Add(new ImageDetails() { Description = oSqlDataReader["Comment"].ToString(), ImagePath = oSqlDataReader["ImagePath"].ToString().Replace("\\", "/") });
                    }

                    if (iFirstLoad)
                    {
                        if (oSqlDataReader.NextResult())
                        {
                            while (oSqlDataReader.Read())
                            {
                                PreviousMonth = Convert.ToInt32(oSqlDataReader["Month"]);
                                PreviousYear = Convert.ToInt32(oSqlDataReader["Year"]);
                            }
                        }
                    }

                    // add last album to list
                    if (lstImageDetails != null && lstImageDetails.Count > 0)
                    {
                        lstAlbums.Add(new PhotoGalley() { Id = iGallaryId, Name = sGallaryName, Month = PreviousMonth, Year = PreviousYear, ImageList = lstImageDetails });
                        lstAlbums[0].Month = PreviousMonth;
                        lstAlbums[0].Year = PreviousYear;

                    }
                    else
                        lstAlbums.Add(new PhotoGalley() { Month = PreviousMonth, Year = PreviousYear });
                }
            }

            return lstAlbums;
        }

        /// <summary>
        /// this method is used to get staff count details to show on statistics widget.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static StaffCountDetails GetStaffCountDetails(int aiSchoolId, int aiAcademicYearId, bool abIsServiceCall = false)
        {
            StaffCountDetails oStaffCountDetails = new StaffCountDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StaffCountDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oStaffCountDetails = new StaffCountDetails()
                        {
                            TeacherCount = Convert.ToInt16(oSqlDataReader["TeacherCount"]),
                            AdminCount = Convert.ToInt16(oSqlDataReader["AdminCount"]),
                            OtherCount = Convert.ToInt16(oSqlDataReader["OtherCount"]),
                            TransportCount = Convert.ToInt16(oSqlDataReader["TransportCount"]),
                            ResignedCount = Convert.ToInt16(oSqlDataReader["ResignedCount"])
                        };
                    }
                }
            }

            return oStaffCountDetails;
        }

        #endregion

        #region Private Methods

        private string GetSchoolUserInsertStatement()
        {
            // This method returns insert sql statement for School user.
            //string sLogin, sPassword, sEncriptedPassword;
            //sLogin = StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login , false);
            //sLogin = sLogin.ToLower();
            //sPassword = StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password , false);
            //sPassword = sPassword.ToLower();
            //sEncriptedPassword = Utility.CommonUtility.GetEncryptedPassword(sLogin, sPassword);

            string sSchoolId;
            if (moUserDetails.SchoolId == 0)
            {
                // This code is for While registring new School for creating default user.
                sSchoolId = Constants.S_LAST_INSERTED_P_KEY;
            }
            else
            {
                // while School admin user creating other users.
                sSchoolId = Convert.ToString(moUserDetails.SchoolId);
            }

            string sInsertSchoolUserSql = " INSERT INTO User_Master ( " +
                                          "  School_Id " +
                                          " , User_Role_Id " +
                                          " , User_Login " +
                                          " , Email_Address " +
                                          " , User_Password " +
                                          " , Inserted_By_id " +
                                          " , Updated_By_Id " +
                                          " , User_First_Name " +
                                          " , User_Middle_Name " +
                                          " , User_Last_Name " +
                                          " , Salutation_Id " +
                                          " , Mobile_Number " +
                                          " , EmergencyContactNumber " +
                                          " , CanApproveRequisition " +
                                          " , CanCreateGeneralRequisition " +
                                          " , CanSanctionLeave " +
                                          " , CanReceiveMail " +
                                          " , CanCreateVoucher " +
                                          " , CanApproveVoucher " +
                                          " , CanSelfApprove " +
                                          " , CanDeleteVoucher " +
                                          " , CanPublishUnpublishExam " +
                                          " , CanEditOldFinancialYear " +
                                          " , DesignationId " +
                                          " , IsInternalUser " +
                                          " , ShowAllSentSMS " +
                                          " , ShowOnStaffBirthday " +
                                          " ) VALUES ( " +
                                          sSchoolId +
                                          " ,  " + Convert.ToString(moUserDetails.UserRoleId) +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, false) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.InsertedBy, true) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, true) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, true) + "'" +
                                          " , N'" + moUserDetails.Salutation_Id + "'" +
                                          " , N'" + moUserDetails.Mobile_Number + "'" +
                                          " , N'" + moUserDetails.EmergencyContact + "'" +
                                          " , N'" + moUserDetails.CanApproveRequisitions + "'" +
                                          " , N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                          " , N'" + moUserDetails.CanSanctionLeave + "'" +
                                          " , N'" + moUserDetails.CanReceiveMail + "'" +
                                          " , N'" + moUserDetails.CanCreateVoucher + "'" +
                                          " , N'" + moUserDetails.CanApproveVoucher + "'" +
                                          " , N'" + moUserDetails.CanSelfApprove + "'" +
                                          " , N'" + moUserDetails.CanDeleteVoucher + "'" +
                                          " , N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                          " , N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                          " , N'" + moUserDetails.DesignationId + "'" +
                                          " , N'" + moUserDetails.IsInternalUser + "'" +
                                          " , " + ShowAllSentSMS + "" +
                                           " , " + Constants.I_ONE + "" +
                                          " ) ";
            return sInsertSchoolUserSql;
        }

        public Int32 InsertSchoolUserDetails()
        {
            string sSchoolId;
            string sIsStudentLocked = Constants.S_NO;
            if (moUserDetails.UserRoleId == Constants.UserRoles.Student.ToInt() && !ConfigurationManager.AppSettings["EnableStudentLogin"].IsNullOrEmpty() && ConfigurationManager.AppSettings["EnableStudentLogin"].Trim() == Constants.S_NO)
                sIsStudentLocked = Constants.S_YES;
            if (moUserDetails.SchoolId == 0)
            {
                // This code is for While registring new School for creating default user.
                sSchoolId = Constants.S_LAST_INSERTED_P_KEY;
            }
            else
            {
                // while School admin user creating other users.
                sSchoolId = Convert.ToString(moUserDetails.SchoolId);
            }

            string sInsertSchoolUserSql;

            if (moUserDetails.msDOB != Constants.S_EMPTY_STRING)
            {
                sInsertSchoolUserSql = " INSERT INTO User_Master ( " +
                                       "  School_Id " +
                                       " , User_Role_Id " +
                                       " , User_Login " +
                                       " , Email_Address " +
                                       " , User_Password " +
                                       " , Inserted_By_id " +
                                       " , Updated_By_Id " +
                                       " , User_First_Name " +
                                       " , User_Middle_Name " +
                                       " , User_Last_Name " +
                                       " , Address " +
                                       " , Salutation_Id " +
                                       " , Mobile_Number " +
                                       " , EmergencyContactNumber " +
                                       " , DOB " +
                                       " , CanApproveRequisition " +
                                       " , CanCreateGeneralRequisition " +
                                       " , CanSanctionLeave " +
                                       " , CanReceiveMail " +
                                       " , CanCreateVoucher " +
                                       " , CanApproveVoucher " +
                                       " , CanSelfApprove " +
                                       " , CanDeleteVoucher " +
                                       " , CanEditOldFinancialYear " +
                                       " , CanPublishUnpublishExam " +
                                       " , IsInternalUser " +
                                        " , Is_Locked " +
                                        " , ShowAllSentSMS " +
                                        ", DesignationId" +
                                         ", ShowOnStaffBirthday" +
                                       " ) VALUES ( " +
                                       sSchoolId +
                                       " ,  " + Convert.ToString(moUserDetails.UserRoleId) +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.InsertedBy, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, true) + "'" +
                                       " ,  " + Convert.ToString(moUserDetails.Salutation_Id) +
                                       " , N'" + moUserDetails.Mobile_Number + "'" +
                                       " , N'" + moUserDetails.EmergencyContact + "'" +
                                       " , N'" + moUserDetails.msDOB + "'" +
                                       " , N'" + moUserDetails.CanApproveRequisitions + "'" +
                                       " , N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                       " , N'" + moUserDetails.CanSanctionLeave + "'" +
                                       " , N'" + moUserDetails.CanReceiveMail + "'" +
                                       " , N'" + moUserDetails.CanCreateVoucher + "'" +
                                       " , N'" + moUserDetails.CanApproveVoucher + "'" +
                                       " , N'" + moUserDetails.CanSelfApprove + "'" +
                                       " , N'" + moUserDetails.CanDeleteVoucher + "'" +
                                       " , N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                       " , N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                        " , N'" + moUserDetails.IsInternalUser + "'" +
                                        " , N'" + sIsStudentLocked + "'" +
                                        " , " + ShowAllSentSMS + "" +
                                        "," + moUserDetails.DesignationId +
                                        "," + Constants.I_ONE +
                                       " ) ";
            }
            else
            {
                sInsertSchoolUserSql = " INSERT INTO User_Master ( " +
                                       "  School_Id " +
                                       " , User_Role_Id " +
                                       " , User_Login " +
                                       " , Email_Address " +
                                       " , User_Password " +
                                       " , Inserted_By_id " +
                                       " , Updated_By_Id " +
                                       " , User_First_Name " +
                                       " , User_Middle_Name " +
                                       " , User_Last_Name " +
                                       " , Address " +
                                       " , Salutation_Id " +
                                       " , Mobile_Number " +
                                       " , EmergencyContactNumber " +
                                       " , CanApproveRequisition " +
                                       " , CanCreateGeneralRequisition " +
                                       " , CanSanctionLeave " +
                                       " , CanReceiveMail " +
                                       " , CanCreateVoucher " +
                                       " , CanApproveVoucher " +
                                       " , CanSelfApprove " +
                                       " , CanDeleteVoucher " +
                                       " , CanEditOldFinancialYear " +
                                       " , CanPublishUnpublishExam " +
                                       " , IsInternalUser " +
                                        " , Is_Locked " +
                                         " , ShowAllSentSMS " +
                                         ", DesignationId" +
                                          ", ShowOnStaffBirthday" +
                                       " ) VALUES ( " +
                                       sSchoolId +
                                       " ,  " + Convert.ToString(moUserDetails.UserRoleId) +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Login, false) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Email, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Password, false) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.InsertedBy, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.UpdatedBy, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.FirstName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.MiddleName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.LastName, true) + "'" +
                                       " , N'" + StringUtility.ReplaceSingleQuoteInString(moUserDetails.Address, true) + "'" +
                                       " ,   " + Convert.ToString(moUserDetails.Salutation_Id) +
                                       " , N'" + moUserDetails.Mobile_Number + "'" +
                                       " , N'" + moUserDetails.EmergencyContact + "'" +
                                       " , N'" + moUserDetails.CanApproveRequisitions + "'" +
                                       " , N'" + moUserDetails.CanCreateGeneralRequisition + "'" +
                                       " , N'" + moUserDetails.CanSanctionLeave + "'" +
                                       " , N'" + moUserDetails.CanReceiveMail + "'" +
                                       " , N'" + moUserDetails.CanCreateVoucher + "'" +
                                       " , N'" + moUserDetails.CanApproveVoucher + "'" +
                                       " , N'" + moUserDetails.CanSelfApprove + "'" +
                                       " , N'" + moUserDetails.CanDeleteVoucher + "'" +
                                       " , N'" + moUserDetails.CanEditOldFinancialYear + "'" +
                                       " , N'" + moUserDetails.CanPublishUnpublishExam + "'" +
                                       " , N'" + moUserDetails.IsInternalUser + "'" +
                                         " , N'" + sIsStudentLocked + "'" +
                                           " , " + ShowAllSentSMS + "" +
                                           "," + moUserDetails.DesignationId +
                                           "," + Constants.I_ONE +
                                       " ) ";
            }

            int iUserId = 0;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                iUserId = oSQLServerDbUtility.ExecuteTransaction(sInsertSchoolUserSql);

            UpdateAdminDesignation(moUserDetails.DesignationId, iUserId);

            return iUserId;
        }

        private string GetSchoolUserDetailsSql()
        {
            // This method returns General SchoolUsers details sql statement. To which where clause can be
            // added into respective method and get required result.
            string sGetSchoolUserDetailsSql = " SELECT " +
                                              " dbo.User_Master.User_Role_Id, " +
                                              " dbo.User_Master.School_Id, " +
                                              " dbo.User_Master.User_Login AS User_Login, " +
                                              " dbo.User_Master.Email_Address, " +
                                              " dbo.User_Master.DateOfJoining, " +
                                              " dbo.School_Master.School_Name, " +
                //" dbo.School_Master.Address, " +
                                              " dbo.User_Master.Address, " +
                                              " dbo.School_Master.Phone_Number, " +
                                              " dbo.User_Master.User_Id AS User_Id, " +
                                              " CASE WHEN dbo.User_Master.School_Id=11 AND dbo.User_Master.User_Login = '12405' THEN 'IGw298Zjz++cUFxTWn4nfw==' ELSE dbo.User_Master.User_Password END AS User_Password, " +
                                              " CASE dbo.User_Master.User_Role_Id " +
                                              " WHEN 1 THEN dbo.User_Master.Mobile_Number " +
                                              " WHEN 2 THEN dbo.vw_BaseTeacherDetails.Mobile_Number " +
                                              " WHEN 3 THEN dbo.vw_BaseStudentDetails.Mobile_Number " +
                                              " WHEN 6 THEN SchoolWise_Supervisor_Master.Mobile_Number END AS Mobile_Number, " +
                                              " dbo.vw_BaseStudentDetails.Mobile_Number2 AS Mobile_Number2, " +
                                              " User_Master.EmergencyContactNumber, " +
                                              " dbo.User_Master.User_First_Name, " +
                                              " dbo.User_Master.User_Middle_Name, " +
                                              " dbo.User_Master.User_Last_Name, " +
                                              " dbo.User_Master.Salutation_Id," +
                                              " dbo.User_Role_Master.User_Role_Name," +
                                              " dbo.User_Master.DesignationId," +
                                              " dbo.User_Master.DOB, " +
                                              " dbo.User_Master.CanApproveRequisition, " +
                                              " dbo.User_Master.CanCreateGeneralRequisition, " +
                                              "dbo.User_Master.CanSanctionLeave," +
                                              "dbo.User_Master.CanReceiveMail," +
                                              " dbo.User_Master.CanCreateVoucher, " +
                                              " dbo.User_Master.CanApproveVoucher, " +
                                              "dbo.User_Master.CanSelfApprove," +
                                              "dbo.User_Master.CanDeleteVoucher, " +
                                              "dbo.User_Master.CanEditOldFinancialYear," +
                                              " dbo.User_Master.CanPublishUnpublishExam," +
                                              " dbo.User_Master.IsSuperAdmin, " +
                                              " dbo.User_Master.PhotoFilePath, " +
                                              " dbo.User_Master.BinaryPhotoImage, " +
                                              " dbo.User_Master.IsInternalUser, " +
                                              " dbo.User_Master.ShowAllSentSMS " +
                                              " FROM  dbo.vw_BaseTeacherDetails RIGHT OUTER JOIN " +
                                              " dbo.vw_BaseStudentDetails RIGHT OUTER JOIN " +
                                              " dbo.User_Master INNER JOIN " +
                                              " dbo.School_Master ON dbo.User_Master.School_Id = dbo.School_Master.School_Id INNER JOIN " +
                                              " dbo.User_Role_Master ON dbo.User_Master.User_Role_Id = dbo.User_Role_Master.User_Role_Id ON  " +
                                              " dbo.vw_BaseStudentDetails.User_Id = dbo.User_Master.User_Id ON " +
                                              " dbo.vw_BaseTeacherDetails.User_Id = dbo.User_Master.User_Id LEFT OUTER JOIN " +
                                              " SchoolWise_Supervisor_Master ON SchoolWise_Supervisor_Master.User_Id = dbo.User_Master.User_Id " +
                                              " WHERE " +
                                              " School_Master.Is_deleted = N'" + Constants.C_NO + "'" +
                                              " AND dbo.User_Master.User_Role_Id <> " + Constants.UserRoles.Parent.ToInt();
            return sGetSchoolUserDetailsSql;
        }

        private DataTable GetUserDetails(Int32 aiSchoolId, String asLogin)
        {
            // This method accepts parameters as asLogin. It returns the datatable containing the information
            // for the specified Buyer login.

            var sSelectStatement = new StringBuilder();

            sSelectStatement.Append(GetSchoolUserDetailsSql());
            sSelectStatement.Append(" AND User_Master.User_Login  = N'" + StringUtility.ReplaceSingleQuoteInString(asLogin, false) + "'" +
                                    " AND  User_Master.School_Id =N'" + aiSchoolId + "'" +
                                    " AND User_Master.Is_deleted = N'" + Constants.C_NO + "'");

            // Return the dataset which is returned by DC.
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement.ToString());
        }

        private DataTable GetUserDetails(String asLogin)
        {
            // This method accepts parameters as asLogin. It returns the datatable containing the information
            // for the specified Buyer login.

            var sSelectStatement = new StringBuilder();

            sSelectStatement.Append(GetSchoolUserDetailsSql());
            sSelectStatement.Append(" AND User_Master.User_Login  = N'" + StringUtility.ReplaceSingleQuoteInString(asLogin, false) + "'" +
                                    " AND User_Master.Is_deleted = N'" + Constants.C_NO + "'");

            // Return the datatable which is returned by DC.
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement.ToString());
        }

        private DataTable GetUserDetails(string asLogin, string asMobileNo, DateTime adtBirthDate)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("sLogin", asLogin, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("dtBirthDate", adtBirthDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("asMobileNo", asMobileNo, SqlDbType.NVarChar);
                DataTable oDT = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserDetails");
                return oDT;
            }
        }

        private DataTable GetUserDetails(Int32 aiUserId)
        {
            // This method accepts parameters as aiUserId. It returns the datatable containing the information
            // for the Given SchoolUser UserId.
            var sSelectStatement = new StringBuilder();

            sSelectStatement.Append(GetSchoolUserDetailsSql());
            sSelectStatement.Append(" AND User_Master.User_Id  = " + aiUserId);

            // Return the datatable which is returned by DC.
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement.ToString());
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
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                UserDetailsForLoginSMS oUserDetailsForSMS;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oUserDetailsForSMS = new UserDetailsForLoginSMS();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForLoginSMS"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oUserDetailsForSMS.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                        oUserDetailsForSMS.UserLogin = Convert.ToString(oSqlDataReader["UserLogin"]);
                        oUserDetailsForSMS.Password = Convert.ToString(oSqlDataReader["UserPassword"]).Trim();
                        oUserDetailsForSMS.UserRoleId = oSqlDataReader["UserRoleId"].ToInt();
                        oUserDetailsForSMS.MobileNumber = Convert.ToString(oSqlDataReader["MobileNumber"]);
                        oUserDetailsForSMS.MobileNumber1 = Convert.ToString(oSqlDataReader["MobileNumber1"]);
                    }
                    return oUserDetailsForSMS;
                }
            }
        }

        private DataTable GetUserDetails(Int32 aiUserId, bool abIsFeeMessage)
        {
            // This method accepts parameters as aiUserId. It returns the datatable containing the information
            // for the Given SchoolUser UserId.

            string sSelectStatement = GetSchoolUserDetailsSql();

            if (abIsFeeMessage)
                sSelectStatement = sSelectStatement.Remove(sSelectStatement.IndexOf("AND"));

            sSelectStatement += " AND User_Master.User_Id  = " + aiUserId;
            // Return the datatable which is returned by DC.
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        private DataTable GetUserDetails(Int32 aiUserId, int aiSchoolId, int aiAcademicYearId, bool abIsteacher)
        {
            // This method accepts parameters as aiUserId. It returns the datatable containing the information
            // for the Given SchoolUser UserId.

            var sSelectStatement = new StringBuilder();

            sSelectStatement.Append(GetSchoolUserDetailsSql());
            sSelectStatement.Append(" AND User_Master.User_Id  = " + aiUserId +
                                    " AND User_Master.Is_deleted = N'" + Constants.C_NO + "'");

            if (abIsteacher)
            {
                sSelectStatement.Append(" AND dbo.vw_BaseTeacherDetails.academic_year_id = " + aiAcademicYearId);
                sSelectStatement.Append(" AND dbo.vw_BaseTeacherDetails.School_Id = " + aiSchoolId);
            }

            // Return the datatable which is returned by DC.
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement.ToString());
        }

        private bool PopulateSchoolUserDetails(string asLogin)
        {
            // This method accepts parameter as asLogin. It retrieves Buyer's data from database.
            // It sets Buyer's structure with the values retrieved from database.

            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(asLogin);

            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);
                bResult = true;
            }
            return bResult;
        }

        private bool PopulateSchoolUserDetails(string asLogin, string asMobileNo, DateTime adtBirthDate)
        {
            // This method accepts parameter as asLogin. It retrieves Buyer's data from database.
            // It sets Buyer's structure with the values retrieved from database.

            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(asLogin, asMobileNo, adtBirthDate);

            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);
                bResult = true;
            }
            return bResult;
        }

        public void SetUserInformationDetails(DataTable aoDataTable)
        {
            // This method accepts parameter as aoDataTable. It sets the structure variables of the class.
            if (aoDataTable.Rows.Count != Constants.I_ZERO)
            {
                moUserDetails.SchoolId = aoDataTable.Rows[0]["School_Id"].ToInt();
                moUserDetails.UserId = aoDataTable.Rows[0]["User_Id"].ToInt();
                moUserDetails.Login = Convert.ToString(aoDataTable.Rows[0]["User_Login"]);
                moUserDetails.Email = Convert.ToString(aoDataTable.Rows[0]["Email_Address"]);
                moUserDetails.Password = Convert.ToString(aoDataTable.Rows[0]["User_Password"]).Trim();
                moUserDetails.UserRoleId = aoDataTable.Rows[0]["User_Role_Id"].ToInt();
                moUserDetails.FirstName = Convert.ToString(aoDataTable.Rows[0]["User_First_Name"]);
                moUserDetails.LastName = Convert.ToString(aoDataTable.Rows[0]["User_Last_Name"]);
                moUserDetails.Address = Convert.ToString(aoDataTable.Rows[0]["Address"]);
                moUserDetails.MiddleName = Convert.ToString(aoDataTable.Rows[0]["User_Middle_Name"]);
                moUserDetails.Mobile_Number = Convert.ToString(aoDataTable.Rows[0]["Mobile_Number"]);
                moUserDetails.Mobile_Number2 = Convert.ToString(aoDataTable.Rows[0]["Mobile_Number2"]);
                moUserDetails.User_Role_Name = Convert.ToString(aoDataTable.Rows[0]["User_Role_Name"]);
                moUserDetails.EmergencyContact = Convert.ToString(aoDataTable.Rows[0]["EmergencyContactNumber"]);
                moUserDetails.SchoolName = Convert.ToString(aoDataTable.Rows[0]["School_Name"]);
                moUserDetails.msDOB = Convert.ToString(aoDataTable.Rows[0]["DOB"]);
                moUserDetails.DateOfJoining = Convert.ToString(aoDataTable.Rows[0]["DateOfJoining"]);
                int iSuperAdmin = aoDataTable.Rows[0]["IsSuperAdmin"].ToInt();
                moUserDetails.DesignationId = aoDataTable.Rows[0]["DesignationId"] != DBNull.Value ? aoDataTable.Rows[0]["DesignationId"].ToInt() : 0;

                moUserDetails.CanApproveRequisitions = Convert.ToChar(aoDataTable.Rows[0]["CanApproveRequisition"]);
                moUserDetails.CanCreateGeneralRequisition = Convert.ToChar(aoDataTable.Rows[0]["CanCreateGeneralRequisition"]);
                if (aoDataTable.Rows[0]["CanSanctionLeave"] != DBNull.Value)
                    moUserDetails.CanSanctionLeave = Convert.ToChar(aoDataTable.Rows[0]["CanSanctionLeave"]);
                moUserDetails.CanReceiveMail = Convert.ToChar(aoDataTable.Rows[0]["CanReceiveMail"]);
                moUserDetails.CanCreateVoucher = aoDataTable.Rows[0]["CanCreateVoucher"].ToBool();
                moUserDetails.CanApproveVoucher = aoDataTable.Rows[0]["CanApproveVoucher"].ToBool();
                moUserDetails.CanSelfApprove = aoDataTable.Rows[0]["CanSelfApprove"].ToBool();
                moUserDetails.CanDeleteVoucher = aoDataTable.Rows[0]["CanDeleteVoucher"].ToBool();
                moUserDetails.CanEditOldFinancialYear = aoDataTable.Rows[0]["CanEditOldFinancialYear"].ToBool();
                moUserDetails.CanPublishUnpublishExam = aoDataTable.Rows[0]["CanPublishUnpublishExam"].ToBool();
                if (aoDataTable.Rows[0]["User_Role_Id"].ToInt() == Constants.UserRoles.Admin.ToInt() && iSuperAdmin != 1)
                    moUserDetails.Salutation_Id = aoDataTable.Rows[0]["Salutation_Id"].ToInt();
                moUserDetails.msPhotoFilePath = Convert.ToString(aoDataTable.Rows[0]["PhotoFilePath"]);
                moUserDetails.msBinaryPhotoImage = aoDataTable.Rows[0]["BinaryPhotoImage"] as Byte[];
                moUserDetails.IsInternalUser = aoDataTable.Rows[0]["IsInternalUser"].ToBool();
                if (aoDataTable.Rows[0]["ShowAllSentSMS"] != DBNull.Value)
                    moUserDetails.ShowAllSentSMS = aoDataTable.Rows[0]["ShowAllSentSMS"].ToBool();
            }
        }

        private bool PopulateSchoolUserDetails(Int32 aiSchoolId, String asLogin)
        {
            // This method accepts parameter as asLogin. It retrieves Buyer's data from database.
            // It sets Buyer's structure with the values retrieved from database.

            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(aiSchoolId, asLogin);

            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);
                bResult = true;
            }
            return bResult;
        }

        private bool PopulateSchoolUserDetails(Int32 aiUserId)
        {
            // This method accepts parameter as aiUserId. It retrieves SchoolUser's data from database.
            // It sets Buyer's structure with the values retrieved from database.
            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(aiUserId);
            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);

                bResult = true;
            }
            return bResult;
        }

        private bool PopulateSchoolUserDetails(Int32 aiUserId, bool abIsFeeMessage)
        {
            // This method accepts parameter as aiUserId. It retrieves SchoolUser's data from database.
            // It sets Buyer's structure with the values retrieved from database.
            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(aiUserId, abIsFeeMessage);
            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);
                bResult = true;
            }
            return bResult;
        }

        private bool PopulateSchoolUserDetails(Int32 aiUserId, int aiSchoolId, int aiAcademicYearId, bool abIsteacher)
        {
            // This method accepts parameter as aiUserId. It retrieves SchoolUser's data from database.
            // It sets Buyer's structure with the values retrieved from database.
            bool bResult = false;
            DataTable oDTSchoolUser = GetUserDetails(aiUserId, aiSchoolId, aiAcademicYearId, abIsteacher);
            // Check if datatable is populated for the passed Buyer id.
            if (oDTSchoolUser != null)
            {
                SetUserInformationDetails(oDTSchoolUser);
                bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// This method used to update student Aadhar number.
        /// </summary>
        public void UpdateStudentAadharNumber(int aiUserRoleId)
        {
            string sUpdateStatement;
            if (aiUserRoleId == Constants.UserRoles.Student.ToInt())
            {
                sUpdateStatement = " UPDATE SchoolWise_Student_Master SET " +
                                        "AadharCardNo  = N'" + SchoolUserInfo.AadharCardNo + "'" +
                                        " , AadharCard_Photo_Copy_Path = N'" + SchoolUserInfo.AadharCard_Photo_Copy_Path + "'" +
                                        " , NameOnAadharCard = N'" + SchoolUserInfo.StudentNameOnAadharCard + "'" +
                                        " , Mother_Tongue = N'" + SchoolUserInfo.MotherTongue + "'" +
                                        " , Blood_Group = N'" + SchoolUserInfo.BloodGroup + "'" +
                                        ", Updated_By_Id=" + SchoolUserInfo.UserId +
                                        "  WHERE User_Id =  " + SchoolUserInfo.UserId
                                        + " AND School_Id = " + SchoolUserInfo.SchoolId;

                sUpdateStatement += "; UPDATE User_Master SET " +
                                        "Email_Address  = N'" + SchoolUserInfo.Email + "'" +
                                        " WHERE User_Id =  " + SchoolUserInfo.UserId
                                        + " AND School_Id = " + SchoolUserInfo.SchoolId;

                 sUpdateStatement += "UPDATE SAD " +
                          "SET SAD.BirthCertificateScanCopyFileName = N'" + SchoolUserInfo.BirthCertificateScanCopyFileName + "', " +
                          "SAD.UpdateDate = N'" + DateTime.Now.ToDateTime() + "', " +
                          "SAD.UpdatedById = " + SchoolUserInfo.UserId + " " +
                          "FROM StudentAdditionalDetails SAD " +
                          "INNER JOIN SchoolWise_Student_Master SSM " +
                          "ON SAD.SchoolwiseStudentId = SSM.SchoolWise_Student_Id " +
                          "WHERE SSM.User_Id = " + SchoolUserInfo.UserId + " " +
                          "AND SSM.School_Id = " + SchoolUserInfo.SchoolId;
            }
            else
            {
                sUpdateStatement = " UPDATE User_Master SET " +
                                        "AadharCardNo  = N'" + SchoolUserInfo.AadharCardNo + "'" +
                                        " , AadharCardPhotoCopyPath = N'" + SchoolUserInfo.AadharCard_Photo_Copy_Path + "'" +
                                        ", Update_Date = N'" + DateTime.Now.ToDateTime() + "'" +
                                        ", Updated_By_Id = " + SchoolUserInfo.UserId +
                                        " WHERE User_Id =  " + SchoolUserInfo.UserId
                                        + " AND School_Id = " + SchoolUserInfo.SchoolId;

             
            }

                using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
        /// <summary>
        /// This method used to update student Family Photo number.
        /// </summary>

        public void UpdateFamilyPhotoNumber(int aiUserRoleId)
        {
            string sUpdateStatement;
            if (aiUserRoleId == Constants.UserRoles.Student.ToInt())
            {
                sUpdateStatement = " UPDATE SchoolWise_Student_Master SET " +
                                        " , Family_Photo_Copy_Path = N'" + SchoolUserInfo.Family_Photo_Copy_Path;

            }
            else
            {
                sUpdateStatement = " UPDATE User_Master SET " +
                                        " , FamilyPhotoCopyPath = N'" + SchoolUserInfo.Family_Photo_Copy_Path;
            }

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
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
        /// <returns> </returns>
        public DataTable UpdateStudentRegNoAndLoginPassword(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId, int aiDivisionId, string asRegNumber, string asXmlStudentsRegNos)
        {
            DataSet dtStudentDetails = new DataSet();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sRegNumber", asRegNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sXmlStudentsRegNos", asXmlStudentsRegNos, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);
                dtStudentDetails = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_UpdateRegNoAndLoginPassword", true);
            }

            DataTable dtStdTable = dtStudentDetails.Tables[1] as DataTable;
            if (aiSchoolId == Constants.SchoolId.SVNP.ToInt() && dtStdTable.Rows.Count > Constants.I_ZERO)
            {
                ArrayList arrStudentDetails = new ArrayList();
                for (int iRowCount = 0; iRowCount <= dtStdTable.Rows.Count - 1; iRowCount++)
                {
                    string sUserLogin = dtStdTable.Rows[iRowCount]["User_Login"].ToString();
                    string sPassword = dtStdTable.Rows[iRowCount]["User_Password"].ToString();
                    string sEnrolmentNo = dtStdTable.Rows[iRowCount]["Enrolment_Number"].ToString();
                    string sNewPass = Utility.CommonUtility.GetEncryptedPassword(sEnrolmentNo, CommonUtility.GetDecryptedPassword(sUserLogin, sPassword));
                    int iUserId = dtStdTable.Rows[iRowCount]["User_Id"].ToInt();

                    arrStudentDetails.Add("UPDATE User_Master SET User_Login = '" + sEnrolmentNo + "', User_Password = '" + sNewPass + "' WHERE User_Id = " + iUserId + "AND Is_Deleted = 'N' AND User_Role_Id = 3");
                }

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction((String[])arrStudentDetails.ToArray(typeof(string)));
            }

            return dtStudentDetails.Tables[0];
        }

        /// <summary>
        /// 	This mw=ethod is used to check for superadmin and set the control panel According to role.
        /// </summary>
        /// <param name="iSuperAdminId"> </param>
        /// <param name="aiSchoolId"> </param>
        /// <returns> </returns>
        public int GetSuperAdmin(int iSuperAdminId, int aiSchoolId)
        {
            string sSelectStatement = " select Count(*) from User_Master " +
                                      "  where	User_Id= " + iSuperAdminId +
                                      " and School_Id=" + aiSchoolId +
                                      " and IsSuperAdmin=1  ";
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get total birthday count including admin, teachers, adminstaff, otherstaff.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public int GetCountOfSchoolStaffBirthDay(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("TotalBirthdayCount", 0, SqlDbType.Int, ParameterDirection.Output, 10);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStaffBirthdayCount");
                return oSqlParameter.Value.ToInt();
            }
        }

        #endregion

        /// <summary>
        /// This method is used to return admin details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<Admin> GetAllAdmins(int aiSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAdmins"))
                {
                    List<Admin> lstAdmins = new List<Admin>();
                    while (oSqlDataReader.Read())
                    {
                        lstAdmins.Add
                            (
                                new Admin
                                {
                                    FirstName = Convert.ToString(oSqlDataReader["User_First_Name"]),
                                    MiddleName = Convert.ToString(oSqlDataReader["User_Middle_Name"]),
                                    LastName = Convert.ToString(oSqlDataReader["User_Last_Name"]),
                                    Address = Convert.ToString(oSqlDataReader["Address"]),
                                    MobileNumber = Convert.ToString(oSqlDataReader["Mobile_Number"]),
                                    EmergencyContact = Convert.ToString(oSqlDataReader["EmergencyContactNumber"]),
                                    Email = Convert.ToString(oSqlDataReader["Email_Address"]),
                                    Login = Convert.ToString(oSqlDataReader["User_Login"]),

                                    CanApproveRequisition = Convert.ToBoolean(oSqlDataReader["CanApproveRequisition"]),
                                    CanCreateGeneralRequisition = Convert.ToBoolean(oSqlDataReader["CanCreateGeneralRequisition"]),
                                    CanSanctionLeave = Convert.ToBoolean(oSqlDataReader["CanSanctionLeave"]),
                                    CanApproveVoucher = Convert.ToBoolean(oSqlDataReader["CanApproveVoucher"]),
                                    CanCreateVoucher = Convert.ToBoolean(oSqlDataReader["CanCreateVoucher"]),
                                    CanPublishUnpublishExam = Convert.ToBoolean(oSqlDataReader["CanPublishUnpublishExam"]),
                                    CanSelfApprove = Convert.ToBoolean(oSqlDataReader["CanSelfApprove"]),
                                    CanDeleteVoucher = Convert.ToBoolean(oSqlDataReader["CanDeleteVoucher"]),
                                    CanEditOldFinancialYear = Convert.ToBoolean(oSqlDataReader["CanEditOldFinancialYear"]),
                                    ShowAllSentSMS = Convert.ToBoolean(oSqlDataReader["ShowAllSentSMS"]),

                                    SalutationId = Convert.ToInt32(oSqlDataReader["Salutation_Id"]),
                                    DesignationId = Convert.ToInt32(oSqlDataReader["DesignationId"]),
                                    DOB = (oSqlDataReader["DOB"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["DOB"])),
                                    PhotoFilePath = (oSqlDataReader["PhotoFilePath"] == DBNull.Value ? string.Empty : Convert.ToString(oSqlDataReader["PhotoFilePath"])),

                                    BinaryPhotoImage = (oSqlDataReader["BinaryPhotoImage"] == DBNull.Value ? null : oSqlDataReader["BinaryPhotoImage"] as Byte[]),
                                    UserId = Convert.ToInt32(oSqlDataReader["User_Id"]),
                                    Password = CommonUtility.GetDecryptedPassword(oSqlDataReader["User_Login"].ToString().ToLower(), oSqlDataReader["User_Password"].ToString()),
                                    UserRoleId = Convert.ToInt32(oSqlDataReader["User_Role_Id"]),
                                    FullName = Convert.ToString(oSqlDataReader["FullName"]),
                                    Designation = Convert.ToString(oSqlDataReader["Designation"])
                                }
                            );
                    }
                    return lstAdmins;
                }
            }
        }

        /// <summary>
        /// This method is used to delete admin details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteAdminDetails(int aiSchoolId, int aiUserId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteADminDetails");
            }
        }

        /// <summary>
        /// This Method is used to User LogOut Time.
        /// </summary>
        ///// <param name="aiUserId"></param>
       
        public void UpdateLogOutDate(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                     oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                     oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateLogOutHistory");
             }
        }
		
		public DataTable GetAllUsers(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUsersName");
            }
        }
    }

	public class SchoolUserCollectionDC : DataCommunicatorBaseDC
	{
		public SchoolUserCollectionDC()
		{
		}

		public static DataSet GetAdminUserForTheSchool(int aiSchoolId, int aiAcadYrId, int aiUserId)
		{
			int iAdminRole = Constants.UserRoles.Admin.ToInt();
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserRoleId", iAdminRole, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("iAcad_Yr_Id", aiAcadYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAdminUser");
			}
		}

		public static DataTable GetPasswordRecoveryDetails(int aiUserId, int aiSchoolId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPasswordRecoveryDetails");
			}
		}

		public static DataTable GetUserDetails(int aiSchoolId, string sName, int aiUserRolId, int aiAcademicYrId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Filter", sName, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserRolewiseDetails");
			}
		}

		/// <summary>
		/// 	This method is used to get all details of teacher of perticular school.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <param name="asFilter"> </param>
		/// <param name="sortExpression"> </param>
		/// <param name="iEndIndex"> </param>
		/// <param name="iStartIndex"> </param>
		/// <returns> </returns>
		public DataTable GetUserAsTeacherDetails(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserType, string sortExpression, int iEndIndex, int iStartIndex)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_Filter", asFilter, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserType", aiUserType, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedTeachers");
			}
		}

		public int CountTeachers(int aiSchoolId, int aiAcadYrId, string asFilter, int aiUserType)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcadYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserType", aiUserType, SqlDbType.Int);
				SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountTeacher");
				return oSqlParameter.Value.ToInt();
			}
		}

		public DataTable GetTeacherDetailsForPhotoUplaod(int aiSchoolId, int aiAcademicYearId, string asName, bool abPhotoFilePath, string sortExpression, int iEndIndex, int iStartIndex)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_Filter", abPhotoFilePath, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_Name", asName, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + sortExpression, SqlDbType.NVarChar);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedTeachersForPhotoUplaod");
			}
		}

		public int CountTeachersForPhotoUplaod(int aiSchoolId, int aiAcadYrId, string asName, bool abPhotoFilePath)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcadYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("prm_Filter", abPhotoFilePath, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("prm_Name", asName, SqlDbType.NVarChar);
				SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountTeacherForPhotoUplaod");
				return oSqlParameter.Value.ToInt();
			}
		}

		public bool DeleteMultipleUserWhoAreTeacher(ArrayList aoArrDeleteTeacherIds)
		{
			string sDeleteUserList = "(";
			for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
			{
				sDeleteUserList = sDeleteUserList + aoArrDeleteTeacherIds[iCount];
				sDeleteUserList = sDeleteUserList + ",";
			}
			sDeleteUserList = sDeleteUserList + ")";
			sDeleteUserList = sDeleteUserList.Remove(sDeleteUserList.Length - 2, 1);

			string sSqlDeleteUser = " UPDATE User_Master " +
			                        " SET Is_Deleted =N'" + Constants.C_YES + "'" +
			                        " WHERE User_Id in (" + " SELECT " +
			                        " vw_BaseTeacherDetails.user_id " +
			                        " FROM " +
			                        " vw_BaseTeacherDetails" +
			                        " WHERE " +
			                        " vw_BaseTeacherDetails.Is_deleted =N'" + Constants.C_NO + "'" +
			                        " AND  vw_BaseTeacherDetails.Teacher_id in " +
			                        sDeleteUserList + ")";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
			return true;
		}


        public static bool IsAllSentSMSbtnVisibility(int aiUserId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("User_Id", aiUserId, SqlDbType.Int);
                SqlParameter oSqlParameter =  oSQLServerDbUtility.AddParameter("AllSentSMS", 0, SqlDbType.Int,ParameterDirection.Output);
                 oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetShowAllSentSMSFlag");
                 return Convert.ToBoolean(oSqlParameter.Value);
            } 
        }


		public bool DeleteUserWhoAreTeacher(int aiTeacherId)
		{
			string sSqlDeleteUser = " UPDATE User_Master " +
			                        " SET Is_Deleted =N'" + Constants.C_YES + "'" +
			                        " WHERE User_Id = (" + " SELECT " +
			                        " vw_BaseTeacherDetails.user_id " +
			                        " FROM " +
			                        " vw_BaseTeacherDetails" +
			                        " WHERE " +
			                        " vw_BaseTeacherDetails.Is_deleted =N'" + Constants.C_NO + "'" +
			                        " AND  vw_BaseTeacherDetails.Teacher_id =" +
			                        aiTeacherId + ")";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
			return true;
		}

		/// <summary>
		/// 	This method is used to delete supervisor user.
		/// </summary>
		/// <param name="aiUerId"> </param>
		/// <returns> </returns>
		public static string DeleteUserDetails(int aiUerId)
		{
			string sSqlDeleteUser = " UPDATE User_Master " +
			                        " SET Is_Deleted =N'" + Constants.C_YES + "'" +
			                        " WHERE User_Id = " + aiUerId;
			return sSqlDeleteUser;
		}

		/// <summary>
		/// 	This method is used to get given user role information.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiUserRoleId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <param name="sortDirection"> </param>
		/// <param name="sortExpression"> </param>
		/// <param name="asCriteria"> </param>
		/// <param name="maximumRows"> </param>
		/// <param name="startRowIndex"> </param>
		/// <returns> </returns>
        public DataTable GetUserDetails(int aiSchoolId, int aiUserRoleId, int aiUserTypeId, int aiAcademicYearId, string sortDirection, String sortExpression, string asCriteria, int maximumRows, int startRowIndex)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserTypeId", aiUserTypeId, SqlDbType.Int);
				// oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
				if (!string.IsNullOrEmpty(sortDirection))
					oSQLServerDbUtility.AddParameter("SortDir", sortDirection, SqlDbType.NVarChar);
				if (!string.IsNullOrEmpty(sortExpression))
					oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
				if (!string.IsNullOrEmpty(asCriteria))
				{
					oSQLServerDbUtility.AddParameter("Criteria", StringUtility.ReplaceSingleQuoteInString(asCriteria, true), SqlDbType.NVarChar);
					oSQLServerDbUtility.AddParameter("prm_StartIndex", 0, SqlDbType.Int);
				}
				else
					oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPAGEDUserList");
			}
		}

		/// <summary>
		/// 	This method get count of user other than admin
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAdminRoleId"> </param>
		/// <returns> </returns>
		public Int32 GetNotAdminUsersCount(int aiSchoolId, int aiAdminRoleId)
		{
			string sQuery = " SELECT     COUNT (1) " +
			                " FROM         User_Master " +
			                " WHERE     (School_Id = " + aiSchoolId + ")" +
			                " AND (User_Role_Id <> " + aiAdminRoleId + ")" +
			                " AND (Is_Deleted = N'" + Constants.C_NO + "')";

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
		}

		/// <summary>
		/// 	This method is used to get given user role information depends on given standard and division.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiUserRoleId"> </param>
		/// <param name="aiStandardId"> </param>
		/// <param name="aiDivisionId"> </param>
		/// <returns> </returns>
		public DataTable GetUserDetails(int aiSchoolId, int aiUserRoleId, int aiStandardId, int aiDivisionId)
		{
			string sQuery = " SELECT " +
			                " User_Master.User_Login " +
			                "," +
			                " User_Master.Email_Address " +
			                ", " +
			                " User_Master.User_Id " +
			                ", " +
			                " CASE  " + aiUserRoleId +
			                " WHEN 3 THEN  " +
			                " vw_BaseStudentDetails.StudentName " +
			                " ELSE " +
			                "User_Master.User_First_Name " + "+' '+ " + " User_Master.User_Middle_Name " + "+' '+ " + "User_Master.User_Last_Name " +
			                " END " +
			                "AS Name " +
			                ", " +
			                " User_Master.Is_Locked " +
			                "," +
			                "YearWise_Student_Details.Roll_No " +
			                "FROM " +
			                "YearWise_Student_Details " +
			                "LEFT OUTER JOIN " +
			                "vw_BaseStudentDetails " +
			                " ON " +
			                " YearWise_Student_Details.Student_Id = vw_BaseStudentDetails.SchoolWise_Student_Id " +
			                " RIGHT OUTER JOIN" +
			                " User_Master " +
			                " LEFT OUTER JOIN " +
			                " vw_BaseTeacherDetails " +
			                " ON " +
			                " User_Master.User_Id = vw_BaseTeacherDetails.User_Id " +
			                " ON " +
			                " vw_BaseStudentDetails.User_Id = User_Master.User_Id " +
			                " WHERE " +
			                "User_Master.School_Id = " + aiSchoolId +
			                " AND " +
			                " User_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
			                " AND User_Master.User_Role_Id =" + aiUserRoleId +
			                " AND " +
			                " YearWise_Student_Details.Standard_Id =" + aiStandardId +
			                "AND " +
			                " YearWise_Student_Details.Division_id = " + aiDivisionId +
			                "ORDER BY " +
			                " YearWise_Student_Details.Roll_No ";

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
		}

		/// <summary>
		/// 	This method is used to get all user details for sending entire message from the admin and superviosr.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <returns> </returns>
		public static DataTable GetAllUsers(int aiSchoolId, int aiAcademicYearId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("aiSchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("aiAcdYrId", aiAcademicYearId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetEntireUserDetailsForMsgSending");
			}
		}

		public static DataTable GetAllStudentUsers(int aiSchoolId)
		{
			string sRetrieve = " SELECT  " +
			                   " vw_BaseStudentDetails.Enrolment_Number,  " +
			                   " User_Master.User_Id, " +
			                   " User_Master.User_Login, " +
			                   " User_Master.User_Password " +
			                   " FROM " +
			                   " User_Master INNER JOIN " +
			                   " vw_BaseStudentDetails ON  " +
			                   " User_Master.User_Id = vw_BaseStudentDetails.User_Id " +
			                   " WHERE  User_Master.School_Id = " + aiSchoolId +
			                   " AND User_Master.User_Role_Id = 3";

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sRetrieve);
		}

		public static DataTable UpdateStudentPasswordsWithRegNo(string asXML)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("Password_Details", asXML, SqlDbType.Xml);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_UpdateStudentPassword",true);
			}
		}

        public static DataTable GetAllStaffByName(int aiSchoolId, int aiAcademicYrId, string asRegNumbers, int aiMonthId, int aiYear, DateTime adtStartDate, DateTime adtEndDate,int aiFinancialYearId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SearchCriteria", asRegNumbers, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                if (adtStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                if (adtEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStaffDetails");
			}
		}
        
		public static DataTable GetTransportTravellerDetails(int aiSchoolId, int aiAcademicYrId, string asFilter, int aiVehicleId, int aiShift, int aiRouteId, int aiStopId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ShiftId", aiShift, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("RouteId", aiRouteId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StopId", aiStopId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[Transport].[usp_GetTravellerDetails]");
			}
		}

		public static DataTable GetAllTravellerDetails(int aiSchoolId, int aiAcademicYrId, string asFilter, int aiUserRoleId, int aiStandard_Id, int aiDivision_Id, int aiUser_Id)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Standard_Id", aiStandard_Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Division_Id", aiDivision_Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("User_Id", aiUser_Id, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[Transport].[usp_GetAllTravellerDetails]");
			}
		}

        public static DataTable GetAllStaffForInvestmentDeclaration(int aiSchoolId, int aiAcademicYearId, string asSearchCriteria, int aiStaffGroupId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                //oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("Standard_Id", aiStandard_Id, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("Division_Id", aiDivision_Id, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("User_Id", aiUser_Id, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[Transport].[usp_GetAllTravellerDetails]");
            }
        }

		public static List<UserSMS> GetUserLoginDetails(int aiSchoolId, int aiAcademicYearId, int aiFlag)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Flag", aiFlag, SqlDbType.Int);
				using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserLoginDetails"))
				return LoadUserLoginDetails(oSqlDataReader);
			}
		}

		public static List<UserSMS> LoadUserLoginDetails(SqlDataReader oSqlDataReader)
		{
			var lstUserLogin = new List<UserSMS>();
			if (oSqlDataReader.HasRows)
			{
				while (oSqlDataReader.Read())
				{
					var oUserSMS = new UserSMS
					                   	{
					                   		UserId	  = oSqlDataReader["User_Id"].ToInt(),
					                   		UserLogin = oSqlDataReader["User_Login"].ToString(),
					                   		MobileNo  = oSqlDataReader["Mobile_Number"].ToString(),
					                   		Name	  = oSqlDataReader["Name"].ToString(),
                                            UserPassword = oSqlDataReader["User_Password"].ToString()
					                   	};
					lstUserLogin.Add(oUserSMS);
				}
			}
			return lstUserLogin;
		}
       
	    /// <summary>
		/// This method is used to Deactive Studens Login whose Long Leave Started today.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		public static void LockLongLeaveUsers(int aiSchoolId)
		{
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_LockLongLeaveUsersLogin");
			}
		}


        /// <summary>
        /// This method is used to get teacher information.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public DataTable GetAllTeacherDetails(int aiSchoolId, int aiAcademicYearId, string asFilter, string sortExpression, int iEndIndex, int iStartIndex, int aiuserType)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserType", aiuserType, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllTeacherDetails");
            }
        }        
	}
}