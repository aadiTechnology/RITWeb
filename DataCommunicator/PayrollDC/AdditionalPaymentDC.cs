/* File Name - AdditionalPaymentDC.cs
 * Created By - Sachin
 * Created Date - 29 Oct 2013
 * Description - This class is used to manage additional payment details. 
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class AdditionalPaymentDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miFinancialYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public AdditionalPaymentDC()
        {
        }

        public AdditionalPaymentDC(int aiSchoolId, int aiFinancialYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinancialYearId = aiFinancialYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Public Method((s)

        /// <summary>
        /// This method is used to return all additional payment details according to given filter.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<AdditionalPaymentDetails> GetAll(int aiSchoolId, int aiFinancialYearId, string asFilter, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAdditionalPayments"))
                {
                    List<AdditionalPaymentDetails> lstAdditionalPayments = new List<AdditionalPaymentDetails>();
                    while (oSqlDataReader.Read())
                        lstAdditionalPayments.Add(SetPaymentDetails(oSqlDataReader));
                    return lstAdditionalPayments;
                }
            }
        }

        /// <summary>
        /// This method is used to count additional payments.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiFinancialYearId, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAdditionalPaymentCount");
                return Convert.ToInt32(oSqlParameter.Value);                
            }
        }

        /// <summary>
        /// This method is used to return all additional payments.
        /// </summary>
        /// <returns></returns>
        public List<AdditionalPaymentDetails> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", string.Empty, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", "PaymentDate Desc", SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", 0, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", 99999 , SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAdditionalPayments"))
                {
                    List<AdditionalPaymentDetails> lstAdditionalPayments = new List<AdditionalPaymentDetails>();
                    while (oSqlDataReader.Read())
                        lstAdditionalPayments.Add(SetPaymentDetails(oSqlDataReader));
                    return lstAdditionalPayments;
                }
            }
        }

        /// <summary>
        /// This method is used to return additional payment object.
        /// </summary>
        /// <param name="aiPaymentId"></param>
        /// <returns></returns>
        public AdditionalPaymentDetails Get(int aiPaymentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentId", aiPaymentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAdditionalPaymentDetails"))
                {
                    oSqlDataReader.Read();
                    return SetPaymentDetails(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to delete additional payment according to given id.
        /// </summary>
        /// <param name="aiPaymentId"></param>
        public void Delete(int aiPaymentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentId", aiPaymentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteAdditionalPaymentDetails");
            }
        }

        /// <summary>
        /// This method is used to save additional payment details.
        /// </summary>
        /// <param name="asPaymentXml"></param>
        public void Save(string asPaymentXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentXml", asPaymentXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveAdditionalPaymentDetails");
            }
        } 

        #endregion

        #region Private Method(s)
        
        /// <summary>
        /// This method is used to fill up additional payment entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private AdditionalPaymentDetails SetPaymentDetails(SqlDataReader aoSqlDataReader)
        {
            return new AdditionalPaymentDetails
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                ParameterId = Convert.ToInt32(aoSqlDataReader["ParameterId"]),
                Parameter = Convert.ToString(aoSqlDataReader["Parameter"]),
                Amount = Convert.ToInt64(aoSqlDataReader["Amount"]),
                PaymentDate = Convert.ToDateTime(aoSqlDataReader["PaymentDate"]),
                UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                StaffGroupId = Convert.ToInt32(aoSqlDataReader["StaffGroupsId"]),
                BankId = Convert.ToInt32(aoSqlDataReader["BankId"]),
                BankDetailsId = Convert.ToInt32(aoSqlDataReader["BankDetailsId"])
            };
        } 

        #endregion
    }
}
