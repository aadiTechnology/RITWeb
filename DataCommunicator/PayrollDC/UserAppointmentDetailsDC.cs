using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class UserAppointmentDetailsDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public UserAppointmentDetailsDC()
        {
        }

        public UserAppointmentDetailsDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all available appointmnet details.
        /// </summary>
        /// <returns></returns>
        public List<UserAppointmentDetails> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAppointmentDetails"))
                    return FillAppointmentDetails(oSqlDataReader);

            }
        }

        /// <summary>
        /// This method is used to return all available appointmnet details according to given page index.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<UserAppointmentDetails> GetAll(int aiSchoolId, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedAppointmentDetails"))
                    return FillAppointmentDetails(oSqlDataReader);

            }
        }

        /// <summary>
        /// This method is used to return appointmnet details count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAppointmentDetailsCount"))
                    return Convert.ToInt32(oSqlParameter.Value);

            }
        }

        /// <summary>
        /// This method is used to save appointment details.
        /// </summary>
        /// <param name="aoUserAppointmentDetails"></param>
        public void Save(UserAppointmentDetails aoUserAppointmentDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AppointmentId", aoUserAppointmentDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DesignationId", aoUserAppointmentDetails.DesignationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JobStatusId", aoUserAppointmentDetails.Status.StatusId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SalutationId", aoUserAppointmentDetails.SalutationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoUserAppointmentDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Address", aoUserAppointmentDetails.Address, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("JoiningDate", aoUserAppointmentDetails.JoiningDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("PaymentStartdate", aoUserAppointmentDetails.PaymentStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("AgreementDate", aoUserAppointmentDetails.AgreementDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ParameterXmL", aoUserAppointmentDetails.EarningDeductionXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("PaymentGroupId", aoUserAppointmentDetails.PaymentGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmployeeNo", aoUserAppointmentDetails.EmployeeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("LetterNoPostfix",string.IsNullOrEmpty(aoUserAppointmentDetails.LetterNoPostfix) ? (object)DBNull.Value : aoUserAppointmentDetails.LetterNoPostfix,SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserAppointmentDetails");
            }
        }

        /// <summary>
        /// This method is used to return appointment details.
        /// </summary>
        /// <param name="aiAppointmentId"></param>
        /// <returns></returns>
        public UserAppointmentDetails Get(int aiAppointmentId)
        {
            UserAppointmentDetails oUserAppointmentDetails;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AppointmentId", aiAppointmentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAppointmentDetails"))
                {
                    oUserAppointmentDetails = FillAppointmentDetails(oSqlDataReader)[0];
                    oSqlDataReader.NextResult();
                    oUserAppointmentDetails.EarningDeductions = GetEarningDeductions(oSqlDataReader);
                }
            }
            return oUserAppointmentDetails;
        }

        /// <summary>
        /// This method is used to delete appointment details.
        /// </summary>
        /// <param name="aiAppointmentId"></param>
        public void Delete(int aiAppointmentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AppointmentId", aiAppointmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteAppointmentDetails");
            }
        } 

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to to fill up appointment details entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<UserAppointmentDetails> FillAppointmentDetails(SqlDataReader aoSqlDataReader)
        {
            List<UserAppointmentDetails> lstUserAppointmentDetails = new List<UserAppointmentDetails>();
            while (aoSqlDataReader.Read())
            {
                lstUserAppointmentDetails.Add
                    (
                        new UserAppointmentDetails
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Address = Convert.ToString(aoSqlDataReader["Address"]),
                            AgreementDate = Convert.ToDateTime(aoSqlDataReader["AgreementDate"]),
                            DesignationId = Convert.ToInt32(aoSqlDataReader["DesignationId"]),
                            Designation = Convert.ToString(aoSqlDataReader["Designation"]),
                            JoiningDate = Convert.ToDateTime(aoSqlDataReader["JoiningDate"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            PaymentStartDate = Convert.ToDateTime(aoSqlDataReader["PaymentStartDate"]),
                            SalutationId = Convert.ToInt32(aoSqlDataReader["SalutationId"]),
                            PaymentGroupId = Convert.ToInt32(aoSqlDataReader["PaymentGroupId"]),
                            Status = new StaffStatusDetails { StatusId = Convert.ToInt32(aoSqlDataReader["StatusId"]), StatusName = Convert.ToString(aoSqlDataReader["StatusName"]) },
                            EmployeeNo = Convert.ToString(aoSqlDataReader["EmployeeNo"]),
                            LetterNoPostfix = aoSqlDataReader["LetterNoPostfix"] == DBNull.Value? string.Empty: Convert.ToString(aoSqlDataReader["LetterNoPostfix"])
                        }
                    );
            }
            return lstUserAppointmentDetails;
        }

        /// <summary>
        /// This method is used to return all available earning deductions groups.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<EarningDeductionGroup> GetEarningDeductions(SqlDataReader aoSqlDataReader)
        {
            List<EarningDeductionGroup> lstEarningDeductionGroups = new List<EarningDeductionGroup>();
            while (aoSqlDataReader.Read())
            {
                lstEarningDeductionGroups.Add
                    (
                    new EarningDeductionGroup
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                        EarningDeductionId = Convert.ToInt32(aoSqlDataReader["EarningDeductionId"]),
                        Amount = Convert.ToDecimal(aoSqlDataReader["Amount"])
                    }
                    );
            }
            return lstEarningDeductionGroups;
        } 

        #endregion        
    }
}
