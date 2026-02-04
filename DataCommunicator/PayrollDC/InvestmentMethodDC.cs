// File Name - InvestmentMethodDC.cs
// Creator - Sachin
// Created Date - 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// class is used to communicate with database for insert/delete/update/ display of investment declarations.
    /// </summary>
    public class InvestmentMethodDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miFinYearId;
        private int miUpdatedById;
        private int miAcademicYearId;        
        private InvestmentMethod moInvestmentMethod;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public InvestmentMethodDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public InvestmentMethodDC(int aiSchoolId, int aiFinYearId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Property(s)

        public InvestmentMethod InvestmentMethod
        {
            set { this.moInvestmentMethod = value; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all the investment methods according to selected page.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <returns>Entity list of investment method</returns>
        public List<InvestmentMethod> GetAll(int aiSchoolId, int aiFinYearId, string asSortExpression, int aiEndIndex, int aiStartRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.NVarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInvestmentMethods"))
                return FillInvestmentMethods(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to return all the investment methods.
        /// </summary>
        /// <returns>Entity list of investment method</returns>
        public List<InvestmentMethod> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInvestmentMethods"))
                return FillInvestmentMethods(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to update configuration.
        /// </summary>
        public void Update()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InvestmentMethodId", this.moInvestmentMethod.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", this.moInvestmentMethod.Name, SqlDbType.NVarChar);                
                oSQLServerDbUtility.AddParameter("AssociatedEarnDeductId", this.moInvestmentMethod.AssociatedEarnDeductId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MaxLimit",this.moInvestmentMethod.MaxLimit,SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("SectionId", this.moInvestmentMethod.SectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DeleteEntry", this.moInvestmentMethod.Is_Deleted, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ApplyToAllUsers", this.moInvestmentMethod.ApplyToAllUsers, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsReset", this.moInvestmentMethod.IsReset, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("ConfigId", Constants.SchoolConfigurations.InvestmentMethod,SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateInvestmentMethod");
            }
        }

        /// <summary>
        /// This method is used to fill all investment methods in entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns>Entity list of investment method</returns>
        public List<InvestmentMethod> FillInvestmentMethods(SqlDataReader aoSqlDataReader)
        {
            List<InvestmentMethod> lstInvestmentMethods = new List<InvestmentMethod>();
            while (aoSqlDataReader.Read())
            {
                InvestmentMethod oInvestmentMethod = new InvestmentMethod
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"]),
                    SectionName = Convert.ToString(aoSqlDataReader["SectionName"]),                    
                    AssociatedEarnDeductId = Convert.ToInt32(aoSqlDataReader["AssociatedEarnDeductId"]),
                    MaxLimit = Convert.ToInt32(aoSqlDataReader["MaxLimit"]),
                    AssociatedEarnDeductName = ((aoSqlDataReader["AssociatedEarnDeductName"] == null || aoSqlDataReader["AssociatedEarnDeductName"].ToString() == string.Empty) ? "-" : aoSqlDataReader["AssociatedEarnDeductName"].ToString())

                };
                lstInvestmentMethods.Add(oInvestmentMethod);
            }

            return lstInvestmentMethods;
        } 

        #endregion
    }
}
