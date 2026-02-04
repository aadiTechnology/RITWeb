// File Name - IncomeTaxDetailsDC.cs
// Creator - Pravin
// Created Date -

// Modifide  - Sachin
// Modifide Dae - 13 Nov 2013
// Reasion - For adding income tax calculation code.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Utility;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for publish,unpublish,display display of income tax details.
    /// </summary>    
    public class IncomeTaxDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miFinYearId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion
        
        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IncomeTaxDetailsDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public IncomeTaxDetailsDC(int aiSchoolId, int aiFinYearId, int aiUserId,int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;
            this.miUpdatedById = aiUserId;
            this.miAcademicYearId = aiAcademicYearId;
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
            /// This method is used to return all the income tax details according to selected page.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="aiStartRowIndex"></param>
            /// <returns>Entity list of income tax details</returns>
        public static List<IncomeTaxDetails> GetAll(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStaffGroupId, string asSearchName, int aiEndIndex, int aiStartRowIndex, out int aiTotalRows)
        {
            aiTotalRows = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId",aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowCnt", Constants.I_ZERO, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SearchName", asSearchName, SqlDbType.VarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStaffGroupUsers"))
                    return ReadFromDataReader(oSqlDataReader, out aiTotalRows);
                
            }
        }

        /// <summary>
        /// This method is used to publish income tax details.
        /// </summary>
        /// <param name="aoTaxDeductionDetails"></param>
        public void Publish(bool abIsPublish)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", abIsPublish, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishIncomeTaxDetails");
            }
        }

        /// <summary>
        /// This method is used to checked whether income tax details are published for this year.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public bool CheckIsPublished(int aiUserId=0)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                bool IsPublished = false;
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_CheckPublishUnPublish"))
                {
                    if (oSqlDataReader.Read())
                        IsPublished = Convert.ToBoolean(oSqlDataReader["IsPublished"]);

                    return IsPublished;
                }
            }         
        }

        #endregion

        #region Private Method(s)

        /// <summary>
            /// This method is used to fill income tax details in entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns>Entity List of InvestmentDeclaration</returns>
        private static List<IncomeTaxDetails> ReadFromDataReader(SqlDataReader aoSqlDataReader, out int aiTotalRows)
        {
            aiTotalRows = 0;
            List<IncomeTaxDetails> lstIncomeTaxDetails = new List<IncomeTaxDetails>();
            while (aoSqlDataReader.Read())
            {
                IncomeTaxDetails oIncomeTaxDetails = new IncomeTaxDetails
                {
                    Id = Convert.ToInt32(aoSqlDataReader["SrNo"]),
                    Designation = Convert.ToString(aoSqlDataReader["Designation"]),
                    UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    IsPublished=Convert.ToBoolean(aoSqlDataReader["IsPublished"])
                };
                lstIncomeTaxDetails.Add(oIncomeTaxDetails);

                if (aiTotalRows == 0)
                    aiTotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"]);
            }

            return lstIncomeTaxDetails;
        }

        #endregion     
    
        public List<TaxReliefParameters> GetAllTaxReliefDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaxReliefDetails"))
                {
                    List<TaxReliefParameters> lstParameters = new List<TaxReliefParameters>();
                    while (oSqlDataReader.Read())
                    {
                        lstParameters.Add
                            (
                                new TaxReliefParameters
                                {
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                                    FromAmount = Convert.ToDecimal(oSqlDataReader["FromAmount"]),
                                    ToAmount = Convert.ToDecimal(oSqlDataReader["ToAmount"]),
                                    SectionId = Convert.ToInt32(oSqlDataReader["SectionId"])
                                }
                            );
                    }
                    return lstParameters;
                }
            }
        }
    }
}
