using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class PaymentGroupDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public PaymentGroupDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)
       
        /// <summary>
        /// This method is used to return all payment groups.
        /// </summary>
        /// <returns></returns>
        public List<PaymentGroup> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPaymentGroups"))
                {
                    List<PaymentGroup> lstGroups = new List<PaymentGroup>();
                    while (oSqlDataReader.Read())
                    {
                        lstGroups.Add
                            (
                                new PaymentGroup
                                {
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    Name = Convert.ToString(oSqlDataReader["Name"])
                                }
                            );
                    }

                    return lstGroups;
                }
            }
        }

        /// <summary>
        /// This method is used to return payment group according to given payment group id.
        /// </summary>
        /// <param name="aiPaymentGroupId"></param>
        /// <returns></returns>
        public PaymentGroup Get(int aiPaymentGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentGroupId", aiPaymentGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPaymentGroups"))
                {
                    List<EarningDeductionGroup> lstGroupDetails = GetPaymentGroupdetails(oSqlDataReader);
                    oSqlDataReader.NextResult();
                    oSqlDataReader.Read();
                    PaymentGroup oPaymentGroup = new PaymentGroup
                    {
                        Id = Convert.ToInt32(oSqlDataReader["Id"]),
                        Name = Convert.ToString(oSqlDataReader["Name"]),
                        EarningDeductionGroups = lstGroupDetails
                    };

                    return oPaymentGroup;
                }
            }
        }

        /// <summary>
        /// This method is used to delete payment group according to given payment group id.
        /// </summary>
        /// <param name="aiPaymentGroupId"></param>
        public void Delete(int aiPaymentGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentGroupId", aiPaymentGroupId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePaymentGroup");
            }
        }

        /// <summary>
        /// This method is used to save payment group details.
        /// </summary>
        /// <param name="aiGroupId"></param>
        /// <param name="asName"></param>
        /// <param name="asParameterXml"></param>
        public void Save(int aiGroupId, string asName, string asParameterXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentGroupId", aiGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ParameterXml", asParameterXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePaymentGroup");
            }
        } 

        #endregion

        #region Private method(s)
        /// <summary>
        /// This method is used to fill up payment group entity list and return it.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<EarningDeductionGroup> GetPaymentGroupdetails(SqlDataReader aoSqlDataReader)
        {
            List<EarningDeductionGroup> lstGroupParameters = new List<EarningDeductionGroup>();
            while (aoSqlDataReader.Read())
            {
                lstGroupParameters.Add
                    (
                        new EarningDeductionGroup
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            EarningDeductionId = Convert.ToInt32(aoSqlDataReader["EarningDeductionId"]),
                            PaymentGroupId = Convert.ToInt32(aoSqlDataReader["PaymentGroupId"]),
                            Amount = Convert.ToDecimal(aoSqlDataReader["Amount"]),
                            ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                            IsEarning = Convert.ToBoolean(aoSqlDataReader["IsEarning"]),
                        }
                    );
            }
            return lstGroupParameters;
        } 

        #endregion
    }
}
