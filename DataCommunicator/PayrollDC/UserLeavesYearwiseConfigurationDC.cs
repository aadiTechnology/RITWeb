// Class Name       :- UserLeavesYearwiseConfigurationDC
// Purpose          :- This class is used to manage UserLeavesYearwiseConfiguration details.
// Date Of creation :-  5 Jan 2010
// Author Name      :- Deepak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class UserLeavesYearwiseConfigurationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUserId;
        private int miAcademicYearId;
        private List<UserLeaveConfiguration> mlstUserLeaveConfigurations = new List<UserLeaveConfiguration>();

        #endregion

        #region Constructor(s)

        public UserLeavesYearwiseConfigurationDC()
        {
        }

        public UserLeavesYearwiseConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        #endregion

        #region Property(s)

        public List<UserLeaveConfiguration> UserLeaveConfigurations
        {
            get { return this.mlstUserLeaveConfigurations; }
            set { this.mlstUserLeaveConfigurations = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used get save leaves for staff groups member or for whole staff group.
        /// </summary>
        /// <param name="abApplytoAll"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void Save(bool abApplytoAll, int aiStaffGroupId, char acUpdateAll, UserLeaveConfiguration aoUserLeaveConfiguration)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aoUserLeaveConfiguration.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aoUserLeaveConfiguration.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Is_Deleted", aoUserLeaveConfiguration.Is_Deleted == 1 ? Constants.S_YES : Constants.S_NO, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AllowedLeavesXML", aoUserLeaveConfiguration.AllowedLeaveXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("BasicLeavesXML", aoUserLeaveConfiguration.BasicLeaveXml, SqlDbType.Xml);
                if (abApplytoAll)
                {
                    oSQLServerDbUtility.AddParameter("ApplyToAllUsersOfStaffGroup", acUpdateAll, SqlDbType.Char);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertAllowedLeavesForStaffGroup");
                }
                else
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertAllowedLeavesForStaffMember");
            }
        }

        /// <summary>
        /// This method is used to get years.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetYears()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetYearsForPayroll");
            }
        }

        /// <summary>
        /// This method is used get saved or default leaves for staff groups member.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public DataSet GetAllowedLeaves(int aiUserId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetYearwiseLeaves");
            }
        }

        public List<BasicLeaveDetails> GetUsersBasicLeaves(int aiUserId, int aiStaffGroupId, int aiLeaveSeperatorDay)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveSeperatorDay", aiLeaveSeperatorDay, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserBasicLeaveDetails"))
                {
                    List<BasicLeaveDetails> lstBasicLeaveDetails = new List<BasicLeaveDetails>();
                    while (oSqlDataReader.Read())
                    {
                        BasicLeaveDetails oBasicLeaveDetails = new BasicLeaveDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            BasicLeaveConfigId = Convert.ToInt32(oSqlDataReader["BasicLeaveConfigId"]),
                            LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                            BasicLeaves = Convert.ToDecimal(oSqlDataReader["BasicLeaves"]),
                            Month = new MasterEntities.MonthMaster
                            {
                                MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                                MonthAbbreviation = Convert.ToString(oSqlDataReader["MonthAbbreviation"])
                            }
                        };
                        lstBasicLeaveDetails.Add(oBasicLeaveDetails);
                    }

                    return lstBasicLeaveDetails;
                }
            }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill user leave entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersLeaves(SqlDataReader oSqlDataReader)
        {
            UserLeaveConfiguration oUserLeaveConfigurationDC;
            while (oSqlDataReader.Read())
            {
                oUserLeaveConfigurationDC = new UserLeaveConfiguration
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    LeaveBalance = Convert.ToDecimal(oSqlDataReader["LeaveBalance"])
                };
                this.mlstUserLeaveConfigurations.Add(oUserLeaveConfigurationDC);
            }
        }

        #endregion

        public List<LeaveYear> GetLeaveYears()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveYears"))
                {
                    List<LeaveYear> lstYears = new List<LeaveYear>();
                    while (oSqlDataReader.Read())
                    {
                        lstYears.Add
                            (
                             new LeaveYear
                             {
                                 Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                 Year = Convert.ToString(oSqlDataReader["Year"]),
                                 StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                                 EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"])
                             }
                            );
                    }
                    return lstYears;
                }
            }
        }

        /// <summary>
        /// This method is used to get user leave details for leaves encashment.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public DataSet GetUserLeavesForEncashment(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetUserLeaveDetailsForEnCashment");
            }
        }

        /// <summary>
        /// This method is used to get leave balance for encashment.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiLeaveID"></param>
        /// <param name="aiYear"></param>        
        public decimal GetLeaveBalanceForEncashment(int aiUserId, int aiLeaveID, int aiYear)
        {
            decimal dLeaveBalanceCount = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveId", aiLeaveID, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveBalanceForEncashment");
                if (oSqlDataReader.Read())
                    dLeaveBalanceCount = Convert.ToDecimal(oSqlDataReader["LeaveBalance"]);

                return dLeaveBalanceCount;
            }
        }

        /// <summary>
        /// This method is used to save leave encashment details in database.
        /// </summary>
        /// <param name="moLeaveEncashmentDetails"></param>
        public void SaveEncashmentDetails(LeaveEncashmentDetails aoLeaveEncashmentDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aoLeaveEncashmentDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aoLeaveEncashmentDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveId", aoLeaveEncashmentDetails.LeaveId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EnCashCount", aoLeaveEncashmentDetails.EncashCount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Amount", aoLeaveEncashmentDetails.Amount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Description", aoLeaveEncashmentDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Date", aoLeaveEncashmentDetails.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoLeaveEncashmentDetails.Id, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserLeaveEncashDetails");
            }
        }

        /// <summary>
        /// This method is used to get user encashed leave details for fill listview.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LeaveEncashmentDetails> GetUserAllEncashLeaveDetails(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<LeaveEncashmentDetails> lstLeaveEncashmentDetails = new List<LeaveEncashmentDetails>();
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserEncashLeaveDetails");
                
                while (oSqlDataReader.Read())
                {
                    LeaveEncashmentDetails oLeaveEncashmentDetails = new LeaveEncashmentDetails();
                    oLeaveEncashmentDetails.Id = Convert.ToInt32(oSqlDataReader["Id"]);
                    oLeaveEncashmentDetails.LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]);
                    oLeaveEncashmentDetails.LeaveType = Convert.ToString(oSqlDataReader["LeaveName"]);
                    oLeaveEncashmentDetails.Date = Convert.ToDateTime(oSqlDataReader["EncashDate"]);
                    oLeaveEncashmentDetails.EncashCount = Convert.ToDecimal(oSqlDataReader["EnCashCount"]);
                    oLeaveEncashmentDetails.Amount = Convert.ToDecimal(oSqlDataReader["Amount"]);

                    lstLeaveEncashmentDetails.Add(oLeaveEncashmentDetails);
                }
                return lstLeaveEncashmentDetails;
            }            
        }

        /// <summary>
        /// This method is used to get user encash leave details for update.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiEncashLeaveId"></param>
        /// <returns></returns>
        public LeaveEncashmentDetails GetUserEncashLeaveDetails(int aiUserId, int aiEncashLeaveId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                LeaveEncashmentDetails oLeaveEncashmentDetails = new LeaveEncashmentDetails();

                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EncashLeaveId", aiEncashLeaveId, SqlDbType.Int);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserEncashLeaveDetailsForUpdate");
                if (oSqlDataReader.Read())
                {
                    oLeaveEncashmentDetails.Year = Convert.ToInt32(oSqlDataReader["Year"]);
                    oLeaveEncashmentDetails.Date = Convert.ToDateTime(oSqlDataReader["EncashDate"]);
                    oLeaveEncashmentDetails.LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]);
                    oLeaveEncashmentDetails.EncashCount = Convert.ToDecimal(oSqlDataReader["EnCashCount"]);
                    oLeaveEncashmentDetails.Amount = Convert.ToDecimal(oSqlDataReader["Amount"]);
                    if (oSqlDataReader["Description"] != DBNull.Value)
                        oLeaveEncashmentDetails.Description = Convert.ToString(oSqlDataReader["Description"]);
                }

                return oLeaveEncashmentDetails;
            }
        }

        /// <summary>
        /// This method is used to delete encash leave details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiEncashLeaveId"></param>
        /// <param name="aiLeaveId"></param>
        public void DeleteUserEncashLeave(int aiUserId, int aiEncashLeaveId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EncashLeaveId", aiEncashLeaveId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteUserEncashLeaveDetails");
            }
        }

        /// <summary>
        /// This method is used to get Amount .
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAmountForLeaveEncashment(int aiUserId, string Date,  int aiLeaveId, int miAcademicYearId, int miSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", Date, SqlDbType.DateTime);
               // oSQLServerDbUtility.AddParameter("LeaveCount", aiEncashCount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EncashLeaveId", aiLeaveId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAmountForLeaveEncashment");
            }
        }
    }
}
