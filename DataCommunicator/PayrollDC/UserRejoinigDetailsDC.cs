// Class Name       :- UserRejoinigDetailsDC
// Purpose          :- This class is used to manage user rejoining details.
// Date Of creation :- 08/11/2019
// Author Name      :- Dnyaneshwar Shinde

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Payroll;

namespace DataCommunicator
{
    public class UserRejoinigDetailsDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUserId;

        #endregion

        #region  Constructor(s)

        public UserRejoinigDetailsDC()
        { }        

        public UserRejoinigDetailsDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUserId = aiUserId;
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
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUsersForRejoiningDetails"))
                {
                    List<UserRejoiningDetails> lstUserRejoiningDetails = new List<UserRejoiningDetails>();

                    while (oSqlDataReader.Read())
                    {
                        lstUserRejoiningDetails.Add
                            (
                                new UserRejoiningDetails
                                {
                                    UserId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                    UserName = Convert.ToString(oSqlDataReader["Display_Member"])
                                }
                            );
                    }
                    return lstUserRejoiningDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get selected user details for Rejoinig.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRejoingId"></param>
        /// <returns></returns>
        public UserRejoiningDetails Get(int aiStaffGroupId, int aiUserId, int aiUserRejoingId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRejoinigId", aiUserRejoingId, SqlDbType.Int);
                UserRejoiningDetails oUserRejoiningDetails = new UserRejoiningDetails();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserBasicDetailsForRejoinig"))
                    return GetUserRejoinigDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to Save user rejoining details.
        /// </summary>
        /// <param name="aoUserRejoiningDetails"></param>
        public void Save(UserRejoiningDetails aoUserRejoiningDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoUserRejoiningDetails.UserRejoinId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aoUserRejoiningDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aoUserRejoiningDetails.StaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmployeeNo", aoUserRejoiningDetails.EmployeeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AccountNo", aoUserRejoiningDetails.AccountNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PFNo", aoUserRejoiningDetails.PFNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UAN", aoUserRejoiningDetails.UAN, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PANNo", aoUserRejoiningDetails.PANNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("JoiningDate", aoUserRejoiningDetails.JoiningDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ResignationDate", aoUserRejoiningDetails.ResignationDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserRejoiningDetails");
            }
        }

        /// <summary>
        /// This method is used to get all users for fill list view.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <param name="aistartRowIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<UserRejoiningDetails> GetAll(int aiSchoolId, string asFilter, int aistartRowIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);                
                oSQLServerDbUtility.AddParameter("StartIndex", aistartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                List<UserRejoiningDetails> mlstUserRejoiningDetails = new List<UserRejoiningDetails>();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUserDetailsForReJoining"))
                {
                    while (oSqlDataReader.Read())
                    {
                        UserRejoiningDetails oUserRejoiningDetails = new UserRejoiningDetails();
                        oUserRejoiningDetails.UserRejoinId = Convert.ToInt32(oSqlDataReader["Id"]);
                        oUserRejoiningDetails.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                        oUserRejoiningDetails.UserName = Convert.ToString(oSqlDataReader["UserName"]);
                        if (oSqlDataReader["JoiningDate"] != DBNull.Value)
                            oUserRejoiningDetails.JoiningDate = Convert.ToDateTime(oSqlDataReader["JoiningDate"]);
                        if (oSqlDataReader["ResignationDate"] != DBNull.Value)
                            oUserRejoiningDetails.ResignationDate = Convert.ToDateTime(oSqlDataReader["ResignationDate"]);
                        oUserRejoiningDetails.TotalRowCount = Convert.ToInt32(oSqlDataReader["TotalRowCount"]);

                        mlstUserRejoiningDetails.Add(oUserRejoiningDetails);
                    }
                }
                return mlstUserRejoiningDetails;
            }
        }

        /// <summary>
        /// This method is used to delete user rejoing details.
        /// </summary>
        /// <param name="aiUserRejoinId"></param>
        /// <param name="aiUserId"></param>
        public void Delete(int aiUserRejoinId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiUserRejoinId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteUserRejoinDetails");
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to populate details to entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private UserRejoiningDetails GetUserRejoinigDetails(SqlDataReader aoSqlDataReader)
        {
            UserRejoiningDetails oUserRejoiningDetails = new UserRejoiningDetails();
            if (aoSqlDataReader.Read())
            {
                oUserRejoiningDetails.StaffGroupId = Convert.ToInt32(aoSqlDataReader["StaffGroupId"]);
                oUserRejoiningDetails.UserId = Convert.ToInt32(aoSqlDataReader["UserId"]);
                oUserRejoiningDetails.EmployeeNo = Convert.ToString(aoSqlDataReader["EmployeeNo"]);
                oUserRejoiningDetails.AccountNo = Convert.ToString(aoSqlDataReader["AccountNo"]);
                oUserRejoiningDetails.PFNo = Convert.ToString(aoSqlDataReader["ProvidentFundNo"]);
                oUserRejoiningDetails.UAN = Convert.ToString(aoSqlDataReader["UAN"]);
                oUserRejoiningDetails.PANNo = Convert.ToString(aoSqlDataReader["PanNo"]);
                if (aoSqlDataReader["JoiningDate"] != DBNull.Value)
                    oUserRejoiningDetails.JoiningDate = Convert.ToDateTime(aoSqlDataReader["JoiningDate"]);
                if (aoSqlDataReader["ResignationDate"] != DBNull.Value)
                    oUserRejoiningDetails.ResignationDate = Convert.ToDateTime(aoSqlDataReader["ResignationDate"]);
                if (aoSqlDataReader["OldJoiningDate"] != DBNull.Value)
                    oUserRejoiningDetails.OldJoiningDate = Convert.ToDateTime(aoSqlDataReader["OldJoiningDate"]);
            }
            return oUserRejoiningDetails;
        }

        #endregion
    }
}
