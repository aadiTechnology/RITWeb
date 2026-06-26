using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaffPerformanceEntity;
using System.Data;
using System.Data.SqlClient;
using Utility;
using SchoolEntities;
using PayrollEntities;
using SchoolEntities.Payroll;

namespace DataCommunicator.PayrollDC
{
    public class UserApplyLeaveDetailsDC
    {
        private int miSchoolId;
        private int miInsertedById;
        private int miAcademicYearId;


        public UserApplyLeaveDetailsDC()
        {


        }
        public UserApplyLeaveDetailsDC(int aiSchoolId, int aiInsertedById, int aiAcademicYearId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miInsertedById = aiInsertedById;
            this.miAcademicYearId = aiAcademicYearId;
        }
        #region Data Member(s)



        #endregion

        public void Save(UserApplyLeaveDetails oUserApplyLeaveDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("Id", oUserApplyLeaveDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveId", oUserApplyLeaveDetails.LeaveId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", oUserApplyLeaveDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", oUserApplyLeaveDetails.StartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", oUserApplyLeaveDetails.EndDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("TotalDays", oUserApplyLeaveDetails.TotalDays, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("ChargeHandoverTo", oUserApplyLeaveDetails.ChargeHandoverTo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Description", oUserApplyLeaveDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Attachment", oUserApplyLeaveDetails.DocumnetPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertApplyLeaveDetails");
            }
        }


        public void SaveLeaveApprovalDetails(LeaveApprovalDetails oLeaveApprovalDetails, bool IsFromFinalApproval)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", oLeaveApprovalDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserLeaveDetailsId", oLeaveApprovalDetails.UserLeaveDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", oLeaveApprovalDetails.ReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Remark", oLeaveApprovalDetails.Remark, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StatusId", oLeaveApprovalDetails.StatusId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromFinalApproval", IsFromFinalApproval, SqlDbType.Bit);


                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLeaveApprovalDetails");
            }
        }


        public DataTable GetStaffName(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStaffName");
            }
        }

        public DataTable GetCategory(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
              oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);
              return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllLeaveCategory");
           }
        }

        //private List<UserApplyLeaveDetails> FillCategories(SqlDataReader aoSqlDataReader)
        //{
        //    List<UserApplyLeaveDetails> lstUserApplyLeaveDetails = new List<UserApplyLeaveDetails>();
        //    while (aoSqlDataReader.Read())
        //    {
        //        lstUserApplyLeaveDetails.Add(new UserApplyLeaveDetails
        //        { 
        //            UserName=aoSqlDataReader["UserName"].ToString(),
        //            StartDate = aoSqlDataReader["StartDate"].ToDateTime(),
        //            EndDate = aoSqlDataReader["EndDate"].ToDateTime(),
        //            Description = aoSqlDataReader["Description"].ToString(),
        //            Status=aoSqlDataReader["Status"].ToString(),


        //        });
        //    }

        //    return lstUserApplyLeaveDetails;
        //}




        //public List<UserApplyLeaveDetails> GetAllFillCategories(int aiCategoryId,int aiUserId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        //oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
        //        //oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

        //        using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLeaveApprovalCatgories"))
        //            return this.FillCategories(oSqlDataReader);


        //    }
        //}
        public void Delete(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_DeleteLeaveApprovalCatgories]");
            }
        }



        public List<UserApplyLeaveDetails> GetAll(int aiSchoolId, int aiUserId, int aiCategoryId,bool abShowOldNonUpdated, int aiAcademicYearId, bool abShowOnlyNonUpdated, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowOldNonUpdated", abShowOldNonUpdated, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ShowOnlyNonUpdated", abShowOnlyNonUpdated, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLeaveApprovalCatgories"))
                {
                    List<UserApplyLeaveDetails> lstUserApplyLeaveDetails = new List<UserApplyLeaveDetails>();
                    while (oSqlDataReader.Read())
                        lstUserApplyLeaveDetails.Add(SetLeaveDetails(oSqlDataReader));
                    return lstUserApplyLeaveDetails;
                }
            }
        }

        private UserApplyLeaveDetails SetLeaveDetails(SqlDataReader aoSqlDataReader)
        {
            return new UserApplyLeaveDetails
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                StartDate = Convert.ToDateTime(aoSqlDataReader["StartDate"]),
                EndDate = Convert.ToDateTime(aoSqlDataReader["EndDate"]),
                TotalDays = Convert.ToDecimal(aoSqlDataReader["TotalDays"]),
                Description = Convert.ToString(aoSqlDataReader["Description"]),
                UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                Status = Convert.ToString(aoSqlDataReader["Status"]),
                TotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"]),
                StatusId = Convert.ToInt32(aoSqlDataReader["StatusId"]),
                LeaveName = Convert.ToString(aoSqlDataReader["LeaveName"]),
                LeaveBalance = Convert.ToDecimal(aoSqlDataReader["LeaveBalance"]),
                IsApprovedByApprover = Convert.ToBoolean(aoSqlDataReader["IsApprovedByApprover"]),
                IsLeaveUpdatedInPayroll = aoSqlDataReader["IsLeaveUpdatedInPayroll"].ToBool()
            };
        }

        //public UserApplyLeaveDetails GetLeaveDetailsCategory(int aiId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //          oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

        //        oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
        //        SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveCategoryDetails");

        //        UserApplyLeaveDetails oUserApplyLeaveDetails;
        //        if(oSqlDataReader.Read())
        //        {
        //         oUserApplyLeaveDetails  = new UserApplyLeaveDetails
        //        {
        //            //Id = Convert.ToInt32(oSqlDataReader["Id"]),
        //            //UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
        //            //LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
        //            //StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
        //            //EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]),
        //            //TotalDays = Convert.ToInt32(oSqlDataReader["TotalDays"]),
        //            //// ChargeHandoverTo = Convert.ToInt32(aoSqlDataReader["ChargeHandoverTo"]),
        //            //Description = Convert.ToString(oSqlDataReader["Description"]),
        //            //UserName = Convert.ToString(oSqlDataReader["UserName"]),
        //            //Status = Convert.ToString(oSqlDataReader["Status"]),
        //            //TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"])
        //         
        //        };

        //        }
        //        return oUserApplyLeaveDetails;
        //    }

        //}


        public UserApplyLeaveDetails GetLeaveDetailsCategory(int aiId, int aiUserId, int aiLoginUserId)
        {
            UserApplyLeaveDetails oUserApplyLeaveDetails = new UserApplyLeaveDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveCategoryDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oUserApplyLeaveDetails.Id = (oSqlDataReader["Id"]).ToInt();
                        oUserApplyLeaveDetails.UserId = (oSqlDataReader["UserId"]).ToInt();
                        oUserApplyLeaveDetails.LeaveId = (oSqlDataReader["LeaveId"]).ToInt();
                        oUserApplyLeaveDetails.StartDate = (oSqlDataReader["StartDate"]).ToDateTime();
                        oUserApplyLeaveDetails.EndDate = (oSqlDataReader["EndDate"]).ToDateTime();
                        oUserApplyLeaveDetails.TotalDays = (oSqlDataReader["TotalDays"]).ToDecimal();
                        oUserApplyLeaveDetails.ChargeHandoverTo = Convert.ToInt32(oSqlDataReader["ChargeHandoverTo"]);
                        oUserApplyLeaveDetails.Description = (oSqlDataReader["Description"]).ToString();
                        oUserApplyLeaveDetails.UserName = (oSqlDataReader["UserName"]).ToString();
                        oUserApplyLeaveDetails.Status = (oSqlDataReader["Status"]).ToString();
                        oUserApplyLeaveDetails.IsFinalApprover = oSqlDataReader["IsFinalApprover"].ToBool();
                        oUserApplyLeaveDetails.LastApproverUserId = oSqlDataReader["LastApproverUserId"].ToInt();
                        oUserApplyLeaveDetails.ApproverRemark = oSqlDataReader["ApproverRemark"].ToString();
                        oUserApplyLeaveDetails.DocumnetPhoto = oSqlDataReader["Attachment"].ToString();
                   }
                    return oUserApplyLeaveDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get login user leave balance details.
        /// </summary>
        /// <param name="aiUSerId"></param>
        /// <returns></returns>
        public List<LeaveBalance> GetLeaveTypeWiseLeaveBalance(int aiUSerId)
        {
            List<LeaveBalance> lstLeaveBalance = new List<LeaveBalance>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUSerId, SqlDbType.Int);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserLeaveBalanceDetails");
                while (oSqlDataReader.Read())
                {
                    lstLeaveBalance.Add(new LeaveBalance {
                        Balance = oSqlDataReader["LeaveBalance"].ToDecimal(),
                        LeaveId = oSqlDataReader["LeaveId"].ToInt(),
                        LeaveName = oSqlDataReader["ShortName"].ToString(), 
                        IsUnpaid = oSqlDataReader["IsUnpaidLeave"].ToBool(),
                        AllowZeroBalance = oSqlDataReader["AllowZeroBalance"].ToBool()
                    });
                    
                }
            }
            return lstLeaveBalance;
        }

        public void UpdateLeaveRecordinPayroll(int aiLeaveConfigId, int aiLeaveTypeId, DateTime adtStartDate, DateTime adtEndDate, Decimal adTotalDays)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveConfigId", aiLeaveConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveTypeId", aiLeaveTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("TotalDays", adTotalDays, SqlDbType.Decimal);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateLeaveRecordInPayroll");
            }
        }

        public string ValidateDates(DateTime adtDate, int aiLeaveTypeId, int aiLeaveConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", adtDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("LeaveTypeId", aiLeaveTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveConfigId", aiLeaveConfigId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ValidateLeaveDate"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader["ErrorMessage"].ToString();
                }
            }
            return string.Empty;
        }

        public bool ValidateDateOverlapping(DateTime adtStartDate, DateTime adtEndDate, int aiUserId, int aiLeaveConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveConfigId", aiLeaveConfigId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsValid", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateLeaveDateOverlapping");
                return oSqlParameter.Value.ToBool();
            }
        }


        public bool AllowUserToViewAllLeaves()
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", this.miInsertedById, SqlDbType.Int);                
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AllowToViewAllLeave", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AllowUserToViewAllLeaves");
                return oSqlParameter.Value.ToBool();
            }
        }
        /// <summary>
        ///This method is used to count pending approval leaves.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static int CountRowsOfRequisition(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountWaitingApprovalLeaves");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }
      }
  }