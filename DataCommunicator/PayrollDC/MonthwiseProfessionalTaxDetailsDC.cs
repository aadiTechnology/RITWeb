// Class Name       :- MonthwiseProfessionalTaxDetailsDC
// Purpose          :- This class is used to manage MonthwiseProfessionalTaxDetails details.
// Date Of creation :- 4/5/2010
// Author Name      :- 

using System;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class MonthwiseProfessionalTaxDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;
        private MonthwiseProfessionalTaxDetails moMonthwiseProfessionalTaxDetails; 

        #endregion

        #region Constructor(s)

        public MonthwiseProfessionalTaxDetailsDC()
        {
            this.moMonthwiseProfessionalTaxDetails = new MonthwiseProfessionalTaxDetails();
        }

        public MonthwiseProfessionalTaxDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moMonthwiseProfessionalTaxDetails = new MonthwiseProfessionalTaxDetails();
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        } 

        #endregion

        #region Property(s)

        public MonthwiseProfessionalTaxDetails MonthwiseProfessionalTaxDetails
        {
            get { return this.moMonthwiseProfessionalTaxDetails; }
            set { this.moMonthwiseProfessionalTaxDetails = value; }
        } 

        #endregion

        #region Method(s)

        public static DataTable GetAllPTChallanDetails(int aiSchoolId, int aiFinancialYearId, string sortExpression, int iEndIndex, int startRowIndex)
        {
            if (sortExpression == string.Empty)
                sortExpression = "Year desc,MonthwiseProfessionalTaxDetails.MonthId desc";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinacialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedPtChallanDetails");
            }
        }

        public static int CountPTChallanDetails(int aiSchoolId,int aiFinancialYearId, string sortExpression, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinacialYearId", aiFinancialYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountPtChallanDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        // This function is used to insert the MonthwiseProfessionalTaxDetails Details
        public bool Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PTRegCertificateId", this.moMonthwiseProfessionalTaxDetails.PTRegCertificateId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeNo", this.moMonthwiseProfessionalTaxDetails.ChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CINNo", this.moMonthwiseProfessionalTaxDetails.CINNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PTRegCertificateNo", this.moMonthwiseProfessionalTaxDetails.PTRegCertificateNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", this.moMonthwiseProfessionalTaxDetails.BankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", this.moMonthwiseProfessionalTaxDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", this.moMonthwiseProfessionalTaxDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("IsInserted", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertMonthwiseProfessionalTaxDetails");
                return Convert.ToBoolean(oSqlParam.Value);
            }
        }

        // This function is used to update the MonthwiseProfessionalTaxDetails Details
        public bool Update()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MonthwiseProfessionalTaxDetailsId", this.moMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PTRegCertificateId", this.moMonthwiseProfessionalTaxDetails.PTRegCertificateId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeNo", this.moMonthwiseProfessionalTaxDetails.ChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CINNo", this.moMonthwiseProfessionalTaxDetails.CINNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PTRegCertificateNo", this.moMonthwiseProfessionalTaxDetails.PTRegCertificateNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", this.moMonthwiseProfessionalTaxDetails.BankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", this.moMonthwiseProfessionalTaxDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", this.moMonthwiseProfessionalTaxDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("IsInserted", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateMonthwiseProfessionalTaxDetails");
                return Convert.ToBoolean(oSqlParam.Value);
            }
        }

        // This function is used to delete the MonthwiseProfessionalTaxDetails Details
        public void Delete()
        {
            string sDeleteStatement = "DELETE MonthwiseProfessionalTaxDetails WHERE MonthwiseProfessionalTaxDetailsId=N'" + this.moMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        // This function is used to load the MonthwiseProfessionalTaxDetails Details
        public MonthwiseProfessionalTaxDetails Get(int aiMonthwiseProfessionalTaxDetailsId)
        {
            MonthwiseProfessionalTaxDetails oMonthwiseProfessionalTaxDetails = new MonthwiseProfessionalTaxDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = this.Fetch(aiMonthwiseProfessionalTaxDetailsId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["MonthwiseProfessionalTaxDetailsId"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId = Convert.ToInt32(oDR["MonthwiseProfessionalTaxDetailsId"]);
                            if (oDR["PTRegCertificateId"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.PTRegCertificateId = Convert.ToInt32(oDR["PTRegCertificateId"]);
                            if (oDR["MonthId"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.MonthId = Convert.ToInt32(oDR["MonthId"]);
                            if (oDR["Year"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.Year = Convert.ToInt32(oDR["Year"]);
                            if (oDR["BankId"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.BankId = Convert.ToInt32(oDR["BankId"]);
                            if (oDR["ChequeNo"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.ChequeNo = Convert.ToString(oDR["ChequeNo"]);
                            if (oDR["CINNo"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.CINNo = Convert.ToString(oDR["CINNo"]);
                            if (oDR["PTRegCertificateNo"] != DBNull.Value)
                                oMonthwiseProfessionalTaxDetails.PTRegCertificateNo = Convert.ToString(oDR["PTRegCertificateNo"]);
                        }

                    }
                }
            }

            return oMonthwiseProfessionalTaxDetails;
        }

        public bool IsDuplicate()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MonthwiseProfessionalTaxDetailsId", this.moMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PTRegCertificationNo", this.moMonthwiseProfessionalTaxDetails.PTRegCertificateNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ChequeNo", this.moMonthwiseProfessionalTaxDetails.ChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", this.moMonthwiseProfessionalTaxDetails.BankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", this.moMonthwiseProfessionalTaxDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", this.moMonthwiseProfessionalTaxDetails.Year, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_IsDuplicatePtChallanDetails");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to check CIN No. is duplicate or not.
        /// </summary>
        /// <returns></returns>
        public bool IsCINNoDuplicate()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MonthwiseProfessionalTaxDetailsId", this.moMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);   
                oSQLServerDbUtility.AddParameter("CINNo", this.moMonthwiseProfessionalTaxDetails.CINNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", this.moMonthwiseProfessionalTaxDetails.BankId, SqlDbType.Int);                
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_IsDuplicateCINNo");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }
        public DataSet GetBankNameMonthYear()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetBankYearAndMonth");
            }
        }

        public bool CheckPrecondition()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "SELECT TOP 1 Schoolwise_Bank_Id FROM Schoolwise_Bank_Master WHERE Is_Deleted=N'N' AND School_Id=" + this.miSchoolId;
                using(SqlDataReader oReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStatement))
                return oReader.HasRows;
            }
        }

        public bool IsSalaryPaid(int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "SELECT TOP 1 SalaryDetailsId FROM SalaryDetails WHERE SchoolId=" + this.miSchoolId +
                                    " AND MonthId=" + aiMonthId +
                                    " AND Year=" + aiYear +
                                    " AND Is_Deleted=N'N'";
                using(SqlDataReader oReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStatement))
                return oReader.HasRows;
            }
        }

        // This function is used to fetch the MonthwiseProfessionalTaxDetails Details
        private string Fetch(int aiMonthwiseProfessionalTaxDetailsId)
        {
            string sSelectStatement = "SELECT " +
           " MonthwiseProfessionalTaxDetailsId " +
           ",MonthwiseProfessionalTaxDetails.PTRegCertificateId " +
           ",PTRegistrationCertificateNoMaster.PTRegCertificateNo " +
           ",MonthId " +
           ",Year " +
           ",BankId " +
           ",ChequeNo " +
           ",CINNo " +
         " FROM " +
            " MonthwiseProfessionalTaxDetails   INNER JOIN PTRegistrationCertificateNoMaster ON " +
            " MonthwiseProfessionalTaxDetails.PTRegCertificateId=PTRegistrationCertificateNoMaster.PTRegCertificateId " +
         " WHERE " +
              " MonthwiseProfessionalTaxDetailsId= " + aiMonthwiseProfessionalTaxDetailsId +
         " AND " +
             " MonthwiseProfessionalTaxDetails.SchoolId=" + this.miSchoolId;
            return sSelectStatement;
        }

        #endregion
    }
}
