/*
 * File Name - EarningDeductionPercentagePopup.aspx.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage payment categories.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class PaymentCategoryDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miUpdatedById;
        private List<EarningDeductionPercentage> mlstPercentages = new List<EarningDeductionPercentage>();

        #endregion

        #region Constructor(s)

        public PaymentCategoryDC()
        {
        }

        public PaymentCategoryDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)
        
        public List<EarningDeductionPercentage> EarningDeductionPercentages
        {
            get { return this.mlstPercentages; }
        } 

        #endregion

        #region Public Method(s)
       
        /// <summary>
        /// This method is used to save category.
        /// </summary>
        /// <param name="aiCategoryID"></param>
        /// <param name="asName"></param>
        /// <param name="asEarnDeductXml"></param>
        public void Save(int aiCategoryID, string asName, string asEarnDeductXml, string asUpdateExistingData)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EarnDeductXml", asEarnDeductXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdateExistingData", asUpdateExistingData, SqlDbType.NVarChar);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePaymentCategory");
            }
        }

        /// <summary>
        /// This method is used to return details of selected category.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public PaymentCategory Get(int aiCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPaymentCategory"))
                {
                    PaymentCategory oPaymentCategory = new PaymentCategory();
                    if (oSqlDataReader.Read())
                    {
                        oPaymentCategory = new PaymentCategory
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            Name = Convert.ToString(oSqlDataReader["Name"])
                        };
                    }

                    oSqlDataReader.NextResult();
                    LoadEarningDeductionPencentage(oSqlDataReader);

                    return oPaymentCategory;
                }
            }
        }

        /// <summary>
        /// This method is used to delete category.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        public void Delete(int aiCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePaymentCategory");
            }
        }

        /// <summary>
        /// This method is used to return all categories.
        /// </summary>
        /// <returns></returns>
        public List<PaymentCategory> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPaymentCategories"))
                {
                    List<PaymentCategory> lstCategories = new List<PaymentCategory>();
                    while (oSqlDataReader.Read())
                    {
                        lstCategories.Add
                            (
                                new PaymentCategory
                                {
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    Name = Convert.ToString(oSqlDataReader["Name"])
                                });
                    }

                    return lstCategories;
                }
            }
        } 

        #endregion

        #region Private Method(s)
      
        /// <summary>
        /// This method is used to load percentage details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadEarningDeductionPencentage(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                this.mlstPercentages.Add
                    (
                        new EarningDeductionPercentage
                        {
                            EarningDeductionId = Convert.ToInt32(aoSqlDataReader["EarningsDeductionsId"]),
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Percentage = Convert.ToInt32(aoSqlDataReader["Percentage"]),
                            EarnDeduct = new EarningsDeductions
                            {
                                ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                                IsEarning = Convert.ToBoolean(aoSqlDataReader["IsEarning"])
                            }
                        });
            }
        } 

        #endregion
    }
}
