// Class Name       :- AmountRangeDC
// Purpose          :- This class is used to manage AmountRange details.
// Date Of creation :- 11/4/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class AmountRangeDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;
        private AmountRange moAmountRange;
        private List<AmountRange> mlstAmountRanges = new List<AmountRange>();
        private List<MonthwiseAmount> mlstMonthwiseAmounts = new List<MonthwiseAmount>();

        #endregion

        #region Construstor(s)

        public AmountRangeDC()
        {
            this.moAmountRange = new AmountRange();
        }

        public AmountRangeDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }
        
        #endregion

        #region Property(s)

        public List<AmountRange> AmountRanges
        {
            get { return this.mlstAmountRanges; }
            set { this.mlstAmountRanges = value; }
        }

        public List<MonthwiseAmount> MonthwiseAmounts
        {
            get { return this.mlstMonthwiseAmounts; }
            set { this.mlstMonthwiseAmounts = value; }
        }

        public AmountRange AmountRange
        {
            get { return this.moAmountRange; }
            set { this.moAmountRange = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This function is used to insert the AmountRange Details.
        /// </summary>
        public void Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", this.moAmountRange.EarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdateMonthwiseAmount", this.moAmountRange.UpdateMonthwiseAmount, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("AmountRangeXml", this.moAmountRange.AmountRangeXml, SqlDbType.Xml);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertAmountRangeDetails");
            }
        }

        /// <summary>
        /// This method is used to check duplicate amount range name.
        /// </summary>
        /// <returns></returns>
        public int IsDuplicateRangeName()
        {
            string sSelectStatement = "SELECT " +
                                      "COUNT(RangeId)" +
                                      " FROM " +
                                      "AmountRange" +
                                      " WHERE " +
                                      " (SchoolId = " + this.miSchoolId + ")" +
                                      " AND (Is_Deleted = N'N')" +
                                      " AND (RangeName = N'" + StringUtility.ReplaceSingleQuoteInString(this.moAmountRange.RangeName, false) + "')" +
                                      " AND (EarningsDeductionsId = " + this.moAmountRange.EarningsDeductionsId + ")" +
                                      " AND (RangeId <> " + this.moAmountRange.RangeId + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to save month wise amount details.
        /// </summary>
        public void InsertMonthwiseAmount()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AmountRangeId", this.moAmountRange.AmountRangeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthXml", this.moAmountRange.MonthXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertMonthwiseAmount");
            }
        }

        /// <summary>
        /// This method is used to return amount ranges.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiEarningDeductionId"></param>
        /// <returns></returns>
        public DataSet GetAll(int aiEarningDeductionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("EarningDeductionId", aiEarningDeductionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAmountRanges");
            }
        }

        /// <summary>
        /// This function is used to update the AmountRange Details.
        /// </summary>
        public void Update()
        {
            string sUpdateStatement = "UPDATE AmountRange SET " +
            "EarningsDeductionsId= " + this.moAmountRange.EarningsDeductionsId +
            ",FromAmount= " + this.moAmountRange.FromAmount +
            ",UptoAmount= " + this.moAmountRange.UptoAmount +
            ",UpdateDate= N'" + DateTime.Now.ToShortDateString() + "' " +
            ",UpdatedById= " + this.miUserId +            
            " WHERE AmountRangeId=" + this.moAmountRange.AmountRangeId +
            " AND SchoolId= " + this.miSchoolId +
            " AND Is_Deleted = N'Y'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete range.
        /// </summary>
        public void Delete(int aiAmountRangeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AmountRangeId", aiAmountRangeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_DeleteRange",true);
            }
        }

        /// <summary>
        /// This function is used to delete the AmountRange Details.
        /// </summary>
        /// <returns></returns>
        public string DeleteAmountRange(int aiRangeId)
        {
            string sSelectSatement = string.Empty;
            sSelectSatement = "select [dbo].[Udf_CheckFormulaAndRangeDependency](" + this.miSchoolId + ",0," + aiRangeId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sMessage = oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectSatement);
                if (sMessage == string.Empty)
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("RangeId", aiRangeId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteEarningDeductionAmountRange");
                }

                return sMessage;
            }
        }

        /// <summary>
        /// This method is used to return month wise amount dtails.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAmountRangeId"></param>
        /// <returns></returns>
        public DataTable GetMonthwiseAmount(int aiAmountRangeId)
        {
            string sSelectStatement = "SELECT " +
                                      "MonthwiseAmountId" +
                                      ", AmountRangeId" +
                                      ", MonthwiseAmount.MonthId" +
                                      ", Month" +
                                      ", Amount" +
                                      " FROM " +
                                      "MonthwiseAmount" +
                                      " INNER JOIN " +
                                      "MonthsOfYear" +
                                      " ON MonthwiseAmount.MonthId = MonthsOfYear.MonthId" +
                                      " WHERE " +
                                      "Is_Deleted = N'N'" +
                                      " AND SchoolId = " + this.miSchoolId +
                                      " AND AmountRangeId = " + aiAmountRangeId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to insert single range.
        /// </summary>
        /// <param name="iAmountRangeId"></param>
        /// <returns></returns>
        public DataTable InsertRangeRow(int iAmountRangeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AmountRangeId", iAmountRangeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", this.moAmountRange.EarningsDeductionsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdateMonthwiseAmount", this.moAmountRange.UpdateMonthwiseAmount, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("FromAmount", this.moAmountRange.FromAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Amount", this.moAmountRange.Amount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("UptoAmount", this.moAmountRange.UptoAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Is_Deleted", this.moAmountRange.Is_Deleted == 0 ? Constants.C_NO : Constants.C_YES, SqlDbType.Char);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_InsertAmountRangeRow",true);
            }
        }

        /// <summary>
        /// THis method is used to update single range.
        /// </summary>
        /// <param name="iAmountRangeId"></param>
        public void UpdateRangeRow(int iAmountRangeId)
        {
            string[] sUpdateStatement = new string[2];

            sUpdateStatement[0] = "UPDATE AmountRange SET " +
            "EarningsDeductionsId= " + this.moAmountRange.EarningsDeductionsId +
            ",FromAmount= " + this.moAmountRange.FromAmount +
            ",UptoAmount= " + this.moAmountRange.UptoAmount +
            ",UpdateDate= N'" + DateTime.Now.ToShortDateString() + "' " +
            ",UpdatedById= " + this.miUserId +
            " WHERE AmountRangeId=" + iAmountRangeId +
            " AND SchoolId= " + this.miSchoolId +
            " AND Is_Deleted = N'N'";

            sUpdateStatement[1] = " UPDATE EarningDeductionFormula " +
                                     " SET    Is_Deleted = N'Y'" +
                                     ",UpdateDate= N'" + DateTime.Now.ToShortDateString() + "' " +
                                     ",UpdatedById= " + this.miUserId +
                                     " WHERE " +
                                                "SchoolId =" + +this.miSchoolId +
                                    " AND " +
                                               "EarningsDeductionsId =" + this.moAmountRange.EarningsDeductionsId +
                                    " AND " +
                                               "Is_Deleted = N'N' ;";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }  

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill amount range entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetAmountRanges(SqlDataReader oSqlDataReader)
        {
            AmountRange oAmountRangeDC;
            while (oSqlDataReader.Read())
            {
                oAmountRangeDC = new AmountRange
                {
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    AmountRangeId = Convert.ToInt32(oSqlDataReader["AmountRangeId"]),
                    FromAmount = Convert.ToDecimal(oSqlDataReader["FromAmount"]),
                    UptoAmount = Convert.ToDecimal(oSqlDataReader["UptoAmount"]),
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"]),
                    RangeId = Convert.ToInt32(oSqlDataReader["RangeId"])                    
                };
                this.mlstAmountRanges.Add(oAmountRangeDC);
            }
        }

        /// <summary>
        /// This method is used to fill amount range entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetAmountRangesToConfig(SqlDataReader oSqlDataReader)
        {
            AmountRange oAmountRangeDC;
            while (oSqlDataReader.Read())
            {
                oAmountRangeDC = new AmountRange
                {
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),                   
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"]),
                    RangeId = Convert.ToInt32(oSqlDataReader["RangeId"]),
                    RangeName = Convert.ToString(oSqlDataReader["RangeName"])
                };
                this.mlstAmountRanges.Add(oAmountRangeDC);
            }
        }
        
        /// <summary>
        /// This method is used to fill month wise amount entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetMonthwiseAmounts(SqlDataReader oSqlDataReader)
        {
            MonthwiseAmount oMonthwiseAmountDC;
            while (oSqlDataReader.Read())
            {
                oMonthwiseAmountDC = new MonthwiseAmount
                {
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    AmountRangeId = Convert.ToInt32(oSqlDataReader["AmountRangeId"]),
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"])
                };
                this.mlstMonthwiseAmounts.Add(oMonthwiseAmountDC);
            }
        }

        #endregion
    }
}
