using System;
using System.Data;
using System.Collections;
using Utility;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace DataCommunicator
{
    /// <summary>
    /// This class is used to handle all the database related operations on Weekdays_Master table. 
    /// </summary>
    public class UserWeekendAssociationDC : DataCommunicatorBaseDC
    {
        #region structure
        public struct UserWeekendAssociationDetailsStruct
        {
          
            public int miUserId;
            public int miWeekEndId;
            public int miSchoolId;
            public int miAcademicYearId;
            public char mbIs_Deleted;
            public DateTime mdtInsertDate;
            public int miInsertedById;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public bool mbIsOtherStaffApplicable;
        }

        UserWeekendAssociationDetailsStruct moUserWeekendAssociationDetailStruct;
        #endregion

        #region DataMember
        public static int miUsersCount;
        #endregion

        #region Properties
        public UserWeekendAssociationDetailsStruct userWeekendAssociationDetailStruct
        {

            get { return moUserWeekendAssociationDetailStruct; }
            set { moUserWeekendAssociationDetailStruct = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This function is used to get user weekend association details..
        /// </summary>
        /// <returns></returns>
        public string GetUserWeekndAssociationInsertStatement()
        {
            string sTeacherId;
            if (userWeekendAssociationDetailStruct.miUserId != 0)
                sTeacherId = "   " + moUserWeekendAssociationDetailStruct.miUserId;
            else
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY;

            string sInsertStatement = "INSERT INTO User_Weekend_Association (" +
                                      " UserId " +
                                      ",WeekendId" +
                                      ",SchoolId" +
                                      ",AcademicYearId" +
                                      ",Is_Deleted" +
                                      ",InsertedDate" +
                                      ",InsertedById" +

                " ) VALUES ( " +
                            sTeacherId +
                    ",   " + moUserWeekendAssociationDetailStruct.miWeekEndId +
                    " ,  " + moUserWeekendAssociationDetailStruct.miSchoolId +
                    " ,  " + moUserWeekendAssociationDetailStruct.miAcademicYearId +
                    " , N'" + Constants.C_NO + "' " +
                    " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                    " , " + moUserWeekendAssociationDetailStruct.miInsertedById +
            " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This function is used to associate weekends to users.
        /// </summary>
        public void InsertUserWeekendAssociationDetailsForOtherStaff()
        {
            string sInsertStatement = "INSERT INTO User_Weekend_Association (" +
                                     " UserId " +
                                     ",WeekendId" +
                                     ",SchoolId" +
                                     ",AcademicYearId" +
                                     ",Is_Deleted" +
                                     ",InsertedDate" +
                                     ",InsertedById" +

               " ) VALUES ( " +
                            moUserWeekendAssociationDetailStruct.miUserId +
                   ",   " + moUserWeekendAssociationDetailStruct.miWeekEndId +
                   " ,  " + moUserWeekendAssociationDetailStruct.miSchoolId +
                   " ,  " + moUserWeekendAssociationDetailStruct.miAcademicYearId +
                   " , N'" + Constants.C_NO + "' " +
                   " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                   " , " + moUserWeekendAssociationDetailStruct.miInsertedById +
           " ) ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        /// <summary>
        /// This function is used to get weekends applicable for staff.
        /// </summary>
        public static List<int> GetWeekendsApplicableforStaff(int aiSchoolId, int aiAcademicYrId)
        {
            List<int> iweekendIdList = new List<int>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetWeekEndsApplicableforStaff");
                if (oSqlDataReader.HasRows)
                {
                    while (oSqlDataReader.Read())
                    {
                        iweekendIdList.Add(Convert.ToInt32(oSqlDataReader["Original_WeekDays_Id"]));
                    }
                }
                oSqlDataReader.Close();
            }

            return iweekendIdList;
        }

        /// <summary>
        /// This function is used to Insert/Update User Weekend association details.
        /// </summary>
        /// <returns></returns>
        public string InsertStatmentUserWeekendAssociation()
        {
            List<int> UsersList = new List<int>();
            UsersList = GetAllUsers(moUserWeekendAssociationDetailStruct.miSchoolId);

            string sInsertStatement = "";
            foreach (int userId in UsersList)
            {
                bool isWeekendAvailable = GetUserWeekendDetails(userId, moUserWeekendAssociationDetailStruct.miSchoolId, moUserWeekendAssociationDetailStruct.miAcademicYearId, moUserWeekendAssociationDetailStruct.miWeekEndId);

                if (isWeekendAvailable != true)
                {
                    sInsertStatement = "INSERT INTO User_Weekend_Association (" +
                                        " UserId " +
                                        ",WeekendId" +
                                        ",SchoolId" +
                                        ",AcademicYearId" +
                                        ",Is_Deleted" +
                                        ",InsertedDate" +
                                        ",InsertedById" +

                  " ) VALUES ( " +
                               userId +
                      ",   " + moUserWeekendAssociationDetailStruct.miWeekEndId +
                      " ,  " + moUserWeekendAssociationDetailStruct.miSchoolId +
                      " ,  " + moUserWeekendAssociationDetailStruct.miAcademicYearId +
                      " , N'" + Constants.C_NO + "' " +
                      " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                      " , " + moUserWeekendAssociationDetailStruct.miInsertedById +
                     " ) ";

                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
                }
                else if (moUserWeekendAssociationDetailStruct.mbIsOtherStaffApplicable == true)
                {
                    sInsertStatement = "UPDATE User_Weekend_Association SET Is_Deleted = N'" + Constants.C_NO + "' " +
                        "WHERE User_Weekend_Association.SchoolId = " + moUserWeekendAssociationDetailStruct.miSchoolId +
                        " AND User_Weekend_Association.UserId = " + userId +
                        " AND User_Weekend_Association.WeekendId = " + moUserWeekendAssociationDetailStruct.miWeekEndId;

                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
                }
            }
            return sInsertStatement;
        }

        /// <summary>
        /// This function is used to delete user weekend association details.
        /// </summary>
        /// <returns></returns>
        public string DeleteStatmentUserWeekendAssociation()
        {
            string sDeleteStatement = "DELETE FROM User_Weekend_Association WHERE WeekendId = " + moUserWeekendAssociationDetailStruct.miWeekEndId +
                "AND SchoolId = " + moUserWeekendAssociationDetailStruct.miSchoolId +
                "AND AcademicYearId = " + moUserWeekendAssociationDetailStruct.miAcademicYearId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);

            return sDeleteStatement;
        }

        /// <summary>
        /// This function is used to get all user details.
        /// </summary>
        public static List<int> GetAllUsers(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsers");
                List<int> lstUserDetails = new List<int>();
                if (oSqlDataReader.HasRows)
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUserDetails.Add(Convert.ToInt32(oSqlDataReader["UserId"]));
                    }
                }
                oSqlDataReader.Close();
                return lstUserDetails;
            }
        }

        /// <summary>
        /// This function is used get all user details.
        /// </summary>
        public static DataTable GetAllUsersDetails(int aiSchoolId, int aiStaffGroupId, string sortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllUserDetails");
            }
        }

        /// <summary>
        /// This function is used to get Weekends for specific user.
        /// </summary>
        public static DataTable GetWeekends(int aiUserId, int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetWeekendsForUser");
            }
        }

        /// <summary>
        ///  This function is used to search user by name.
        /// </summary>
        public static DataTable GetUsersforSearch(string asName, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asName, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUsersforSearch");
            }
        }

        /// <summary>
        /// This function is used to get All Weekends.
        /// </summary>
        public static DataTable GetAllWeekends(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllWeekends");
            }
        }

        /// <summary>
        /// This function is used to get User Weekend details.
        /// </summary>
        public bool GetUserWeekendDetails(int aiUserId, int aiSchoolId, int aiAcademicYrId, int asWeekendId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekendId", asWeekendId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("IsWeekendAssociationAvailable", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IfAlreadyExistUserWeekendAssociation");
                return OSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This function is used to Insert user weekend association details for user.
        /// </summary>
        public void InsertUserWeekEndAssociationDetailsForUser(int aiUserId, int aiSchoolId, int aiAcademicYearId, int asWeekendId)
        {
            bool IsRecordAvailable = GetUserWeekendDetails(aiUserId, aiSchoolId, aiAcademicYearId, asWeekendId);
            string sInsertStatement;

            if (IsRecordAvailable != true)
            {
                sInsertStatement = "INSERT INTO User_Weekend_Association (" +
                                     " UserId " +
                                     ",WeekendId" +
                                     ",SchoolId" +
                                     ",AcademicYearId" +
                                     ",Is_Deleted" +
                                     ",InsertedDate" +
                                     ",InsertedById" +

                   " ) VALUES ( " +
                            aiUserId +
                       ",   " + asWeekendId +
                       " ,  " + aiSchoolId +
                       " ,  " + aiAcademicYearId +
                       " , N'" + Constants.C_NO + "' " +
                       " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                       " , " + moUserWeekendAssociationDetailStruct.miInsertedById +
                      " ) ";

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
            }
            else
            {
                sInsertStatement = "UPDATE User_Weekend_Association " +
                    "SET User_Weekend_Association.Is_Deleted = N'" + Constants.C_NO + "' " +
                    "WHERE  User_Weekend_Association.UserId = " + aiUserId + " " +
                    "AND User_Weekend_Association.WeekendId = " + asWeekendId + " " +
                    "AND User_Weekend_Association.SchoolId = " + aiSchoolId;

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
            }
        }

        /// <summary>
        ///  This function is used to update user weekend association details for user.
        /// </summary>
        public void UpdateUserWeekendAssociationDetailsForUser(int aiUserId, int aiSchoolId, int aiAcademicYearId, int asWeekendId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                bool IsRecordAvailable = GetUserWeekendDetails(aiUserId, aiSchoolId, aiAcademicYearId, asWeekendId);

                if (IsRecordAvailable == true)
                {
                    oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("WeekendId", asWeekendId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteUserWeekEndAssociation");
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// This class is used to execute Weekends configuration transaction on User Weekend Association table.
    /// </summary>
    public class UserWeekendMasterCollectionDC
    {
        #region PublicMethod
        /// <summary>
        /// This method update all Weekdays Configuration into Weekdays_Master table
        /// </summary>
        /// <param name="aoArrayListWeekDays"></param>
        public void UpdateUserWeekendConfiguration(ArrayList aoArrayListWeekDays)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListWeekDays.ToArray(typeof(string)));
        }
        #endregion
    }

}
