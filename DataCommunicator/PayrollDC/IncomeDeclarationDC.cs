using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class IncomeDeclarationDC
    {

        #region Data Member(s)

        private int miSchoolId;
        private int miFinYearId;
        private int miUpdatedById;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IncomeDeclarationDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public IncomeDeclarationDC(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)

       /// <summary>
        /// This method is used to return all the investment declrations of respective user.
       /// </summary>
       /// <param name="aiUserId"></param>
       /// <param name="aiSectionId"></param>
       /// <param name="asSortExpression"></param>
       /// <param name="asSortDirection"></param>
        /// <returns>List<InvestmentDeclaration></returns>
        public List<IncomeDeclaration> GetAll(int aiUserId, int aiSectionId, string asSortExpression, string asSortDirection)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SectionId", aiSectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllIncomeDeclarations"))
                {
                    List<IncomeDeclaration> lstIncomeDeclaration = this.ReadFromDataReader(oSqlDataReader);
                    return lstIncomeDeclaration;
                }
            }
        }

        /// <summary>
        /// This method is used to save investment declarations of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, string asXml, int aiRegimId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Xml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("RegimeId", aiRegimId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveIncomeDeclaration");
            }
        } 

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill investment declaration in entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns>Entity List of InvestmentDeclaration</returns>
        private List<IncomeDeclaration> ReadFromDataReader(SqlDataReader aoSqlDataReader)
        {
            List<IncomeDeclaration> lstIncomeDeclarations = new List<IncomeDeclaration>();
            while (aoSqlDataReader.Read())
            {
                IncomeDeclaration oInvestmentDeclaration = new IncomeDeclaration
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    InvestmentMethodId = Convert.ToInt32(aoSqlDataReader["InvestmentMethodId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    Amount = Convert.ToDecimal(aoSqlDataReader["Amount"]),                   
                    SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"]),
                    SectionName = Convert.ToString(aoSqlDataReader["SectionName"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    RegimId = Convert.ToInt32(aoSqlDataReader["RegimId"])
                };
                lstIncomeDeclarations.Add(oInvestmentDeclaration);
            }

            return lstIncomeDeclarations;
        } 

        #endregion
    }
}
