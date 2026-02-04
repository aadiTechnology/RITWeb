// -----------------------------------------------------------------------
// <copyright file="Activation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Activation
    {
        public string GetActivationKey()
        {
            string sActivationKey = string.Empty;            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetActivationKey"))
                {
                    if (oSqlDataReader.Read())
                        sActivationKey = oSqlDataReader["ActivationKey"].ToString();
                }
            }
            return sActivationKey;
        }

        public void SaveActivationKey(string asKey)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                oSQLServerDbUtility.AddParameter("ActivationKey", asKey, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveActivationKey");
			}
        }

    }
}
