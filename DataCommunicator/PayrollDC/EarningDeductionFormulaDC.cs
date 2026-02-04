// Class Name       :- EarningDeductionFormulaDC
// Purpose          :- This class is used to manage EarningDeductionFormula details.
// Date Of creation :- 11/3/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class EarningDeductionFormulaDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;
        
        private List<EarningsDeductionsFormulae> mlstEarningsDeductionsFormulae = new List<EarningsDeductionsFormulae>();
        private EarningsDeductionsFormulae moEarningsDeductionsFormulae;

        #endregion

        #region Constructor(s)

        public EarningDeductionFormulaDC()
        {
            this.moEarningsDeductionsFormulae = new EarningsDeductionsFormulae();
        }

        public EarningDeductionFormulaDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moEarningsDeductionsFormulae = new EarningsDeductionsFormulae();
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        } 

        #endregion

        #region Property(s)

        public List<EarningsDeductionsFormulae> EarningsDeductionsFormulae
        {
            get { return this.mlstEarningsDeductionsFormulae; }
            set { this.mlstEarningsDeductionsFormulae = value;  }
        }

        public EarningsDeductionsFormulae EarningsDeductionsFormula
        {
            get { return this.moEarningsDeductionsFormulae; }
            set { this.moEarningsDeductionsFormulae = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This function is used to insert the EarningDeductionFormula Details.
        /// </summary> 
        public void Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", this.moEarningsDeductionsFormulae.EarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Formula", StringUtility.ReplaceSingleQuoteInString(this.moEarningsDeductionsFormulae.Formula, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ChildIds", this.moEarningsDeductionsFormulae.ChildIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FormulaName",this.moEarningsDeductionsFormulae.FormulaName ,SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("IsDefault", this.moEarningsDeductionsFormulae.IsDefault, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_InsertEarningDeductionFormula]");
            }
        }

        /// <summary>
        /// This function is used to update the EarningDeductionFormula Details.
        /// </summary>
        public void Update()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FormulaId", this.moEarningsDeductionsFormulae.FormulaId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", this.moEarningsDeductionsFormulae.EarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Formula", StringUtility.ReplaceSingleQuoteInString(this.moEarningsDeductionsFormulae.Formula, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FormulaName", this.moEarningsDeductionsFormulae.FormulaName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsDefault", this.moEarningsDeductionsFormulae.IsDefault, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_UpdateEarningDeductionFormula]");
            }
        }

        /// <summary>
        /// This function is used to delete the EarningDeductionFormula Details.
        /// </summary>
        /// <returns></returns>
        public string Delete(int aiFormulaId)
        {
            string sSelectSatement = "select [dbo].[Udf_CheckFormulaAndRangeDependency](" + this.miSchoolId + "," + aiFormulaId + ",0)";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sMessage = string.Empty;
                sMessage = oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectSatement);
                if (sMessage == string.Empty)
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FormulaId", aiFormulaId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteEarningDeductionFormula");
                }

                return sMessage;
            }
        }

        /// <summary>
        /// This method is used to delete formula and range.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiFormulaId"></param>
        /// <param name="aiAmountRangeId"></param>
        /// <param name="aiEarningsDeductionsId"></param>
        /// <param name="aiUserId"></param>
        public void DeleteFormulaAndRange(int aiFormulaId, int aiAmountRangeId, int aiEarningsDeductionsId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FormulaId", aiFormulaId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", aiEarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AmountRangeId", aiAmountRangeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteFormulaAndRange");
            }
        }

        /// <summary>
        /// This method is used to check duplicate formula name.
        /// </summary>
        /// <returns></returns>
        public int IsDuplicateFormulaName()
        {
            string sSelectStatement = "SELECT " +
                                      "COUNT(FormulaId)" +
                                      " FROM " +
                                      "EarningDeductionFormula" +
                                      " WHERE " +
                                      " (SchoolId = " + this.miSchoolId + ")" +
                                      " AND (Is_Deleted = N'N')" +
                                      " AND (FormulaName = N'" + StringUtility.ReplaceSingleQuoteInString(this.moEarningsDeductionsFormulae.FormulaName, false) + "')" +
                                      " AND (EarningsDeductionsId = " + this.moEarningsDeductionsFormulae.EarningsDeductionsId + ")" +
                                      " AND (FormulaId <> " + this.moEarningsDeductionsFormulae.FormulaId + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to check whether staff group and earning deduction is configured.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asIdList"></param>
        /// <returns></returns>
        public int AreConfigured(string asIdList)
        {
            string sSelectStatement = "SELECT " +
                                      "COUNT(DISTINCT EarningsDeductionsId)" +
                                      " FROM " +
                                      "StaffGroupsAndEarningsDeductionsAssociation" +
                                      " WHERE " +
                                      " Is_Deleted = N'N'" +
                                      " AND SchoolId = " + this.miSchoolId +
                                      " AND EarningsDeductionsId IN(" + asIdList + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to return recursive formula fields.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiEarningsDeductionsId"></param>
        /// <param name="asIdList"></param>
        /// <returns></returns>
        public DataTable GetRecursiveFieldsOfFormula(int aiEarningsDeductionsId, string asIdList)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", aiEarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IdList", asIdList, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetRecursiveFieldsOfFormula");
            }
        } 

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill formula entity list.
        /// </summary>
        public void SetEarningsDeductionsFormula(SqlDataReader oSqlDataReader)
        {
            EarningsDeductionsFormulae oEarningsDeductionsFormulaeDC;
            while (oSqlDataReader.Read())
            {
                oEarningsDeductionsFormulaeDC = new EarningsDeductionsFormulae
                {
                    FormulaId = Convert.ToInt32(oSqlDataReader["FormulaId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    Formula = Convert.ToString(oSqlDataReader["Formula"]),
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"])                    
                };
                this.mlstEarningsDeductionsFormulae.Add(oEarningsDeductionsFormulaeDC);
            }
        }

        /// <summary>
        /// This method is used to fill formula entity list.
        /// </summary>
        public void SetEarningsDeductionsFormulaToConfig(SqlDataReader oSqlDataReader)
        {
            EarningsDeductionsFormulae oEarningsDeductionsFormulaeDC;
            while (oSqlDataReader.Read())
            {
                oEarningsDeductionsFormulaeDC = new EarningsDeductionsFormulae
                {
                    FormulaId = Convert.ToInt32(oSqlDataReader["FormulaId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    Formula = Convert.ToString(oSqlDataReader["Formula"]),
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"]),
                    FormulaName = Convert.ToString(oSqlDataReader["FormulaName"])
                };
                this.mlstEarningsDeductionsFormulae.Add(oEarningsDeductionsFormulaeDC);
            }
        }

        #endregion
    }
}
