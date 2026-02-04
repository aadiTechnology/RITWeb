// Class Name       :- EarningsDeductionsDC
// Purpose          :- This class is used to manage EarningsDeductions details.
// Date Of creation :- 11/2/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class EarningsDeductionsDC
    {
        #region Data Member(s)

        private EarningsDeductions moEarningsDeduction = new EarningsDeductions();
        private List<EarningsDeductions> mlstEarningsDeductions = new List<EarningsDeductions>();

        #endregion

        #region Property(s)

        public EarningsDeductions EarningsDeduction
        {
            get { return moEarningsDeduction; }
            set { moEarningsDeduction = value; }
        }

        public List<EarningsDeductions> EarningsDeductions
        {
            get { return mlstEarningsDeductions; }
            set { mlstEarningsDeductions = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to validate earnig deduction short name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asShortNameList"></param>
        /// <param name="abIsEarningDeduction"></param>
        /// <returns></returns>
        public static string ValidateShortName(int aiSchoolId, int aiAcademicYearId, string asShortNameList, bool abIsEarningDeduction)
        {
            string sSelectStatement = "SELECT [dbo].[udf_ValidateShortName](" + aiSchoolId + ",N'" + StringUtility.ReplaceSingleQuoteInString(asShortNameList, false) + "'," + Convert.ToInt32(abIsEarningDeduction) + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to return a datatable of EarningsDeductions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<EarningsDeductions> GetAll(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllEarningsDeductions"))
                {
                    var oGenericClass = new GenericClass<EarningsDeductions>();
                    return oGenericClass.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to check whether salary is published for selected earning deduction or not.
        /// </summary>
        /// <param name="aiEarningDeductionId"></param>
        /// <returns></returns>
        public static void ValidateEarningsDeductions(List<int> alstEarningDeductions, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sEarningDeductions = string.Join(",", alstEarningDeductions);                
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningDeductionList", sEarningDeductions, SqlDbType.VarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateEarningsDeductions");
            }
        }
        /// <summary>
        /// This method is used to configure earnings and deductions.
        /// </summary>
        /// <param name="asEDXml"></param>
        /// <param name="aiSchoolId"></param>
        public void Update(string asEDXml, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EDXml", asEDXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertEarningDeductions");
            }
        }

        /// <summary>
        /// This method is used to return earnings and deductions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetFormulaDetails(int aiSchoolId, int aiAcadsemicYearId, int aiEarningDeductionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsId", aiEarningDeductionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetEarningDeductionFormulaDetails");
            }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill  earning deduction entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetEarningsDeductions(SqlDataReader oSqlDataReader)
        {
            EarningsDeductions oEarningsDeductions;
            while (oSqlDataReader.Read())
            {
                oEarningsDeductions = new EarningsDeductions
                {
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsAttendanceDependent = Convert.ToBoolean(oSqlDataReader["IsAttendanceDependent"]),
                    IsEarning = Convert.ToBoolean(oSqlDataReader["IsEarning"]),
                    HasFormula = Convert.ToBoolean(oSqlDataReader["HasFormula"]),
                    OriginalEarningsDeductionsId = Convert.ToInt32(oSqlDataReader["OriginalEarningsDeductionsId"]),
                    IncludeInSalaryDifference = Convert.ToBoolean(oSqlDataReader["IncludeInSalaryDifference"])
                };
                mlstEarningsDeductions.Add(oEarningsDeductions);
            }
        }

        #endregion
    }
}
