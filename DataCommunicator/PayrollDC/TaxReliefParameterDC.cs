using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class TaxReliefParameterDC
    {
        #region Data Member(s)
        
        int miSchoolId;
        int miFinancialYearId;
        int miUpdatedById;
        List<TaxReliefCategory> mlstCategories;
        List<TaxReliefCalculationType> mlstTypes;

        #endregion

        #region Constructor(s)
        
        public TaxReliefParameterDC()
        {
        }

        public TaxReliefParameterDC(int aiSchoolId, int aiFinancialYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinancialYearId = aiFinancialYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)
        
        public List<TaxReliefCategory> Categories
        {
            get { return mlstCategories; }
        }

        public List<TaxReliefCalculationType> Types
        {
            get { return mlstTypes; }
        } 

        #endregion

        #region Public Method(s)
        
        public List<TaxReliefBaseField> GetAllBaseFields()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaxReliefFields");
                List<TaxReliefBaseField> lstBaseFields = GetBaseFields(oSqlDataReader);
                GetCategories(oSqlDataReader);
                GetCalcultionType(oSqlDataReader);
                return lstBaseFields;
            }
        } 

        #endregion

        #region Private Method(s)
        
        private List<TaxReliefBaseField> GetBaseFields(SqlDataReader aoSqlDataReader)
        {
            List<TaxReliefBaseField> lstBaseFields = new List<TaxReliefBaseField>();
            while (aoSqlDataReader.Read())
            {
                lstBaseFields.Add
                    (
                        new TaxReliefBaseField
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        };
                    );
            }
        }

        private List<TaxReliefCategory> GetCategories(SqlDataReader aoSqlDataReader)
        {
            List<TaxReliefCategory> lstCategories = new List<TaxReliefCategory>();
            while (aoSqlDataReader.Read())
            {
                lstCategories.Add
                    (
                        new TaxReliefCategory
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        };
                    );
            }
        }

        private List<TaxReliefCalculationType> GetCalcultionType(SqlDataReader aoSqlDataReader)
        {
            List<TaxReliefCalculationType> lstTypes = new List<TaxReliefCalculationType>();
            while (aoSqlDataReader.Read())
            {
                lstTypes.Add
                    (
                        new TaxReliefCalculationType
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        };
                    );
            }
        } 

        #endregion
    }
}
