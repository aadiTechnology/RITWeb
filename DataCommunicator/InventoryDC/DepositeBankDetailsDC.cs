using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class DepositeBankDetailsDC
    {
        #region Data MEmber(s)

        private int miSchoolId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public DepositeBankDetailsDC()
        {
        }

        public DepositeBankDetailsDC(int aiSchoolId, int aiUpdatedById)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to savebank details.
        /// </summary>
        /// <param name="aoDepositeBankDetails"></param>
        public void Save(SchoolEntities.DepositeBankDetails aoDepositeBankDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoDepositeBankDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aoDepositeBankDetails.CategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aoDepositeBankDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aoDepositeBankDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeNo", aoDepositeBankDetails.ChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Date", aoDepositeBankDetails.Date, SqlDbType.Date);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveDepositeBankDetails");
            }
        }

        /// <summary>
        /// This method is used to delete bank details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteDepositeBankDetails");
            }
        }

        /// <summary>
        /// This method is used to get bank details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public SchoolEntities.DepositeBankDetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DepositeBankDetails oDepositeBankDetails = new DepositeBankDetails();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDepositeBankDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oDepositeBankDetails.ChequeNo = oSqlDataReader["ChequeNo"].ToString();
                        oDepositeBankDetails.Year = oSqlDataReader["Year"].ToInt();
                        oDepositeBankDetails.MonthId = oSqlDataReader["MonthId"].ToInt();
                        oDepositeBankDetails.Date = oSqlDataReader["Date"].ToDateTime();
                        oDepositeBankDetails.Id = oSqlDataReader["Id"].ToInt();
                        oDepositeBankDetails.CategoryId = oSqlDataReader["CategoryId"].ToInt();
                    }
                }
                return oDepositeBankDetails;
            }
        }

        /// <summary>
        /// This method is used to get all bank details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDate"></param>
        /// <param name="asChequeNo"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <returns></returns>
        public List<DepositeBankDetails> GetAll(int aiSchoolId, string asDate, string asChequeNo, string asSortExpression, int aiStartRowIndex, int aiEndRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", asDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("ChequeNo", asChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", aiEndRowIndex, SqlDbType.Int);

                List<DepositeBankDetails> lstDepositeBankDetails = new List<DepositeBankDetails>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllDepositeBankDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        DepositeBankDetails oDepositeBankDetails = new DepositeBankDetails();
                        oDepositeBankDetails.ChequeNo = oSqlDataReader["ChequeNo"].ToString();
                        oDepositeBankDetails.Year = oSqlDataReader["Year"].ToInt();
                        oDepositeBankDetails.MonthId = oSqlDataReader["MonthId"].ToInt();
                        oDepositeBankDetails.Date = oSqlDataReader["Date"].ToDateTime();
                        oDepositeBankDetails.Id = oSqlDataReader["Id"].ToInt();
                        oDepositeBankDetails.TotalRows = oSqlDataReader["TotalRows"].ToInt();
                        oDepositeBankDetails.CategoryId = oSqlDataReader["CategoryId"].ToInt();
                        oDepositeBankDetails.Month = oSqlDataReader["Month"].ToString();
                        oDepositeBankDetails.Category = oSqlDataReader["Category"].ToString();
                        lstDepositeBankDetails.Add(oDepositeBankDetails);
                    }
                }

                return lstDepositeBankDetails;
            }
        }

        /// <summary>
        /// This method is used to validate cheque no.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="asChequeNo"></param>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public bool ValidateChequeNo(int aiId, string asChequeNo, int aiCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "IF EXISTS(SELECT TOP 1 1 FROM GSTInvoicePaymentDetails WHERE IsDeleted = 0 and Id <>" + aiId + " and CategoryId=" + aiCategoryId + " and ChequeNo='" + asChequeNo + "') SELECT 1 ELSE SELECT 0";
                int iValue = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);

                if (iValue == 1)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// This method is used to validate month.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public bool ValidateMonth(int aiId, int aiYear, int aiMonthId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "IF EXISTS(SELECT TOP 1 1 FROM GSTInvoicePaymentDetails WHERE IsDeleted = 0 and Id <>" + aiId + " and Year=" + aiYear + " and MonthId=" + aiMonthId + ") SELECT 1 ELSE SELECT 0";
                int iValue = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);

                if (iValue == 1)
                    return false;
                else
                    return true;
            }
        } 

        #endregion
    }
}
