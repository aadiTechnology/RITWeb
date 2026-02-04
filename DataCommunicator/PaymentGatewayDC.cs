using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data.SqlClient;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class PaymentGatewayDC
    {
        public PaymentGatewayDC()
        {
        }

        public List<AtomGatewayDetails> GetAtomGatewayDetails(int aiAtomCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<AtomGatewayDetails> lstGatewayDetails = new List<AtomGatewayDetails>();
                oSQLServerDbUtility.AddParameter("AtomCategoryId", aiAtomCategoryId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAtomGatewayDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstGatewayDetails.Add
                            (
                                new AtomGatewayDetails
                                {
                                    Name = Convert.ToString(oSqlDataReader["Field"]),
                                    Value = Convert.ToString(oSqlDataReader["Value"])
                                }
                            );
                    }
                }
                return lstGatewayDetails;
            }
        }

        public List<GatewayAdditionalDetails> GetGatewayDetails(Utility.Constants.PaymentGateways aoPaymentGateway)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<GatewayAdditionalDetails> lstGatewayAdditionalDetails = new List<GatewayAdditionalDetails>();
                oSQLServerDbUtility.AddParameter("PaymentGatewayId", aoPaymentGateway.ToInt(), SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGatewayAdditionalDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstGatewayAdditionalDetails.Add
                            (
                                new GatewayAdditionalDetails
                                {
                                    Name = Convert.ToString(oSqlDataReader["Name"]),
                                    Value = Convert.ToString(oSqlDataReader["Value"])
                                }
                            );
                    }
                }
                return lstGatewayAdditionalDetails;
            }
        }

        public DataTable GetPaymentGateway(int aiSchoolId)
        {
            string sQuery = " SELECT DISTINCT " +
                                "Id, " +
                                " case when PaymentGateway = 'AxisBankForAll' then 'Axis Bank' else PaymentGateway end as PaymentGateway" +
                            " FROM " +
                                " PaymentGatewayMaster " +
                            " WHERE " +
                                " SchoolId = " + aiSchoolId +
                                " AND Id IN (6,11)"+
                                " ORDER BY PaymentGateway";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }
    }
}
