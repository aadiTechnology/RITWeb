// -----------------------------------------------------------------------
// File Name : ExternalStudentFeeDC.cs
// Creator :  Sachin Wagh
// Created Date : 03-14-2018
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SchoolEntities;
using Utility;


namespace DataCommunicator
{
    public class ExternalStudentFeeDC
    {
        #region Data Member(s)

		private int miSchoolId;				
		private int miAcademicYearId;
		private int miUserId;
     
        #endregion

		#region Constructor(s)
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExternalStudentFeeDC()
        {

        }
         /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>        
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public ExternalStudentFeeDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;                    
            this.miAcademicYearId = aiAcademicYearId;
			this.miUserId = aiUpdatedById;
        }        
        #endregion

        #region Public Method(s)
        
        /// <Summary>
        ///This Methos is used to get the All the Health Component from the HealthComponents table
        ///</Summary>
        public List<ExternalStudentFee> GetAll(int aiSchoolId, int aiAcademicYearId, string asSortExpression, int aiStartIndex, int aiEndIndex, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", " ORDER BY " + asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllExternalStudentFeeDetails"))
                    return FillExternalFeeDetails(oSqlDataReader);
            }
        }

        /// <Summary>
        /// This function is used to insert the health parameter details 
        /// </Summary> 
        public void Save(ExternalStudentFee aoExternalStudentFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aoExternalStudentFee.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaidDate", aoExternalStudentFee.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("StudentName", aoExternalStudentFee.StudentName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeTypeId", aoExternalStudentFee.FeeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Amount", aoExternalStudentFee.Amount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MobileNo", aoExternalStudentFee.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PaymentMode", aoExternalStudentFee.PaymentModeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeNo", aoExternalStudentFee.ChequeNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeDate", aoExternalStudentFee.ChequeDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("BankId", aoExternalStudentFee.BankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TypeId", aoExternalStudentFee.TypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransactionNo", aoExternalStudentFee.TransactionNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayStudentsExternalFee");
            }
        }

        /// <summary>
        /// This method is used to get all external fee types for fill combobox.
        /// </summary>
        /// <returns></returns>
        public DataTable GetExternalFeeTypesForCombo()
        {
            string sSQLStatement = string.Empty;
            sSQLStatement = "SELECT ID AS ExternalFeeId, FeeType, Amount FROM ExternalFeeDetails WHERE SchoolID = " + miSchoolId + " AND AcademicYearId = " + miAcademicYearId + " AND Is_Deleted = " + Constants.I_ZERO + " ORDER BY FeeType";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSQLStatement);
            }
        }

        /// <Summary>
        ///This function is used to delete the health parameter details 
        ///</Summary> 
        public void Delete(int aiExternalStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiExternalStudentId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteExternalStudentFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to get external Student fee details for edit.
        /// </summary>
        /// <param name="aiExternalStudentId"></param>
        /// <returns></returns>
        public ExternalStudentFee Get(int aiExternalStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                ExternalStudentFee oExternalStudentFee;
                oSQLServerDbUtility.AddParameter("Id", aiExternalStudentId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oExternalStudentFee = new ExternalStudentFee();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExternalStudentFeeDetails"))
                {
                    if (oSqlDataReader.Read())
                    {   
                        oExternalStudentFee.Id = Convert.ToInt32(oSqlDataReader["Id"]);
                        oExternalStudentFee.Date = Convert.ToDateTime(oSqlDataReader["PaymentDate"]);
                        oExternalStudentFee.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                        oExternalStudentFee.FeeId = Convert.ToInt32(oSqlDataReader["FeeTypeId"]);
                        oExternalStudentFee.Amount = Convert.ToInt32(oSqlDataReader["Amount"]);
                        oExternalStudentFee.MobileNo = Convert.ToString(oSqlDataReader["MobileNo"]);
                        oExternalStudentFee.PaymentModeId = Convert.ToInt32(oSqlDataReader["PaymentMode"]);
                        oExternalStudentFee.ChequeNo = Convert.ToInt32(oSqlDataReader["ChequeNo"]);
                        oExternalStudentFee.ChequeDate = Convert.ToDateTime(oSqlDataReader["ChequeDate"]);
                        oExternalStudentFee.BankId = Convert.ToInt32(oSqlDataReader["BankId"]);
                        oExternalStudentFee.ElectronicDetails = oSqlDataReader["ElectronicDetails"].ToString();
                    }
                }
                return oExternalStudentFee;
            }
        }

        /// <summary>
        /// This method is used to get external student details for display receipts.
        /// </summary>
        /// <param name="aiExternalStudentFeeId"></param>
        /// <param name="aiReceiptNo"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAccountHeaderId"></param>
        /// <returns></returns>
        public DataTable GetRecieptDetails(int aiExternalStudentFeeId, int aiReceiptNo, int aiAcademicYearId, int aiAccountHeaderId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ExternalStudentFeeId", aiExternalStudentFeeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNo", aiReceiptNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccountHeaderId", aiAccountHeaderId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetExternalStudetDetailsForReceipt");
            }
        }

        #endregion

        #region Private Method(s)

        /// <Summary>
        ///This function is used to fill health parameter details 
        ///</Summary> 
        private List<ExternalStudentFee> FillExternalFeeDetails(SqlDataReader aoSqlDataReader)
        {
            List<ExternalStudentFee> lstExternalStudentFee = new List<ExternalStudentFee>();
            while (aoSqlDataReader.Read())
            {
                ExternalStudentFee oExternalStudentFee = new ExternalStudentFee
                {
                    TotalRowCount = aoSqlDataReader["TotalRows"].ToInt(),
                    Id = aoSqlDataReader["Id"].ToInt(),
                    Date = aoSqlDataReader["PaymentDate"].ToDateTime(),
                    StudentName = aoSqlDataReader["StudentName"].ToString(),
                    MobileNo = aoSqlDataReader["MobileNo"].ToString(),
                    ReceiptNumber = aoSqlDataReader["ReceiptNumber"].ToInt(),
                    FeeType = aoSqlDataReader["FeeType"].ToString(),
                    Amount = aoSqlDataReader["Amount"].ToInt(),
                    PaymentMode = aoSqlDataReader["PaymentMode"].ToString(),
                    AccountHeaderId = aoSqlDataReader["AccountHeaderId"].ToInt()
                };
                lstExternalStudentFee.Add(oExternalStudentFee);
            }
            return lstExternalStudentFee;
        }

        #endregion
    }
}
