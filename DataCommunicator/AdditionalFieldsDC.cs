using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;
using ControlEntities;

namespace DataCommunicator
{
    public class AdditionalFieldsDC
    {
        public List<AdditionalFields> GetAdditionalFields(int aiScreenId)
        {
            string sSelectStatement = " SELECT ADF.DisplayText," +
                                      " Controls.Control," +
                                      " ADF.IsMandatory," +
                                      " ADF.MaxLength," +
                                      " ADF.AdditionalFieldId" +
                                      " FROM AdditionalFieldDetails ADF" +
                                      " INNER JOIN Controls " +
                                      " ON ADF.ControlId = Controls.ControlId" +
                                      " WHERE ADF.ScreenId = " + aiScreenId +
                                      " AND ADF.Is_Deleted = 'N'" +
                                      " AND Controls.Is_Deleted = 'N'";
            List<AdditionalFields> lstAdditionalFields = new List<AdditionalFields>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    GenericClass<AdditionalFields> oAdditionalFields = new GenericClass<AdditionalFields>();
                    lstAdditionalFields = oAdditionalFields.GetFilledObjectList(oSqlDataReader);
                }
            }
            return lstAdditionalFields;
        }
    }
}
