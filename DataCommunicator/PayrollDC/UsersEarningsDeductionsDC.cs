// Class Name       :- UsersEarningsDeductionsDC
// Purpose          :- This class is used to manage UsersEarningsDeductions details.
// Date Of creation :- 11/11/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class UsersEarningsDeductionsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;
        private UsersEarningsDeduction moUsersEarningsDeduction;        
        private List<UsersFormulaAndRanges> mlstUsersFormulaeAndRanges = new List<UsersFormulaAndRanges>();
        private List<UsersEarningsDeduction> mlstUsersEarningsDeductions = new List<UsersEarningsDeduction>();
        private List<UsersEarningsDeduction> mlstUsersSalDifferenceDetails = new List<UsersEarningsDeduction>();
        private List<SalaryDifferenceConfigDetails> mlstSalaryDifferenceConfigDetails = new List<SalaryDifferenceConfigDetails>();

        #endregion

        #region Constructor(s)

        public UsersEarningsDeductionsDC()
        {
            this.moUsersEarningsDeduction = new UsersEarningsDeduction();
        }

        public UsersEarningsDeductionsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
            this.moUsersEarningsDeduction = new UsersEarningsDeduction();
        } 

        #endregion

        #region Property(s)

        public UsersEarningsDeduction UsersEarningsDeduction
        {
            get { return this.moUsersEarningsDeduction; }
            set { this.moUsersEarningsDeduction = value; }
        }

        public List<UsersFormulaAndRanges> UsersFormulaeAndRanges
        {
            get { return this.mlstUsersFormulaeAndRanges; }
            set { this.mlstUsersFormulaeAndRanges = value; }
        }

        public List<UsersEarningsDeduction> UsersEarningsDeductions
        {
            get { return this.mlstUsersEarningsDeductions; }
            set { this.mlstUsersEarningsDeductions = value; }
        }

        public List<UsersEarningsDeduction> UsersSalDifferenceDetails
        {
            get { return this.mlstUsersSalDifferenceDetails; }
            set { this.mlstUsersSalDifferenceDetails = value; }
        }

        public List<SalaryDifferenceConfigDetails> SalaryDifferenceConfigDetails
        {
            get { return this.mlstSalaryDifferenceConfigDetails; }
            set { this.mlstSalaryDifferenceConfigDetails = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to save users earning deductions.
        /// </summary>
        public void Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.moUsersEarningsDeduction.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", this.moUsersEarningsDeduction.StaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PayScaleSettingsId", this.moUsersEarningsDeduction.PayScaleSettingId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Reason", this.moUsersEarningsDeduction.Reason, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EarningsDeductionsXml", this.moUsersEarningsDeduction.EarningsDeductionsXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("FormulaAndRangeXml", this.moUsersEarningsDeduction.FormulaAndRangeXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ApplyNewFormulaToAll", this.moUsersEarningsDeduction.ApplyFormulaToAllUsersOfStaffGroup.ToString(), SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("ApplyToAllUsersOfStaffGroup", this.moUsersEarningsDeduction.ApplyToAllUsersOfStaffGroup.ToString(), SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("IsActivePayScale", this.moUsersEarningsDeduction.IsActivePayScale, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("PayMatrixId", this.moUsersEarningsDeduction.PayMatrixId, SqlDbType.Int);
                if (this.moUsersEarningsDeduction.ApplyToAllUsersOfStaffGroup.ToString() == Constants.S_NO)
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUsersEarningsDeductions");
                else
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertAllUsersEarningsDeductions");
            }
        }

        /// <summary>
        /// This method is used to return all Earnings-Deductions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiPayScaleSettingsId"></param>
        /// <returns></returns>
        public DataSet GetAll(int aiUserId, int aiStaffGroupId, int aiPayScaleSettingsId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PayScaleSettingsId", aiPayScaleSettingsId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetUsersEarningsDeductions");
            }
        }

        /// <summary>
        /// This method is used to return pay scales. 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPayScaleSettings(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetPayScaleSettings");
            }
        }

        /// <summary>
        /// This method is used to return earning deduction details.
        /// </summary>
        /// <param name="aiFinYearId"></param>
        /// <param name="asUserIds"></param>
        /// <returns></returns>
        public List<EarningDeductionAmount> GetEarningDeductionDetails(int aiFinYearId, string asUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserIds", asUserIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetEarnDeductForITCalcuation"))
                    return this.FillEarningDeductionDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to return user age details.
        /// </summary>
        /// <param name="asUserIds"></param>
        /// <returns></returns>
        public List<UserAgeDetails> GetUserAgeDetails(string asUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserIds", asUserIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserAgeDetails"))
                    return this.FillUserAgeDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to fill up user age details into entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<UserAgeDetails> FillUserAgeDetails(SqlDataReader aoSqlDataReader)
        {
            List<UserAgeDetails> lstUserAgeDetails = new List<UserAgeDetails>();
            while (aoSqlDataReader.Read())
            {
                lstUserAgeDetails.Add(new UserAgeDetails
                {
                    Age = Convert.ToInt32(aoSqlDataReader["Age"]),
                    SalutationId = Convert.ToInt32(aoSqlDataReader["SalutationId"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"])
                });
            }
            return lstUserAgeDetails;
        }

        /// <summary>
        /// This method is used to fill up earning deduction details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<EarningDeductionAmount> FillEarningDeductionDetails(SqlDataReader aoSqlDataReader)
        {
            List<EarningDeductionAmount> lstEarningDeductionAmount = new List<EarningDeductionAmount>();
            while (aoSqlDataReader.Read())
            {
                lstEarningDeductionAmount.Add(new EarningDeductionAmount
                {
                    EarningDeductionId = Convert.ToInt32(aoSqlDataReader["AssociatedEarnDeductId"]),
                    InvestmentIncomeMethodId = Convert.ToInt32(aoSqlDataReader["InvestmentIncomeMethodId"]),
                    Amount = Convert.ToInt64(aoSqlDataReader["TotalAmount"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"])
                });
            }
            return lstEarningDeductionAmount;
        }

        /// <summary>
        /// This function is used to fetch the UsersEarningsDeductions Details.
        /// </summary>
        /// <param name="miUsersEarningsDeductionsId"></param>
        /// <returns></returns>
        private string FetchUsersEarningsDeductionsDetailsFromDatabase(int aiUsersEarningsDeductionsId)
        {
            string sSelectStatement = " SELECT  " +
            "UsersEarningsDeductionsId" +
            ",UserId" +
            ",EarningsDeductionsId" +
            ",EarningsDeductionsValue" +
            ",SchoolId" +
            ",Is_Deleted" +
            ",InsertDate" +
            ",InsertedById" +
            ",UpdateDate" +
            ",UpdatedById" +
            " FROM UsersEarningsDeductions" +
            " WHERE UsersEarningsDeductionsId=" + aiUsersEarningsDeductionsId;
            return sSelectStatement;
        } 

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill formula/range entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersFormulaAndRanges(SqlDataReader oSqlDataReader)
        {
            UsersFormulaAndRanges oUsersFormulaAndRanges;
            while (oSqlDataReader.Read())
            {
                oUsersFormulaAndRanges = new UsersFormulaAndRanges
                {
                    FormulaRangeId = Convert.ToInt32(oSqlDataReader["FormulaRangeId"]),
                    IsFormula = Convert.ToBoolean(oSqlDataReader["IsFormula"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"])
                };
                this.mlstUsersFormulaeAndRanges.Add(oUsersFormulaAndRanges);
            }
        }

        /// <summary>
        /// This method is used to fill formula/range entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersFormulaAndRangesToConfig(SqlDataReader oSqlDataReader)
        {
            UsersFormulaAndRanges oUsersFormulaAndRanges;
            while (oSqlDataReader.Read())
            {
                oUsersFormulaAndRanges = new UsersFormulaAndRanges
                {
                    FormulaRangeId = Convert.ToInt32(oSqlDataReader["FormulaRangeId"]),
                    IsFormula = Convert.ToBoolean(oSqlDataReader["IsFormula"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    UserName = Convert.ToString(oSqlDataReader["UserName"])
                };
                this.mlstUsersFormulaeAndRanges.Add(oUsersFormulaAndRanges);
            }
        }

        /// <summary>
        /// This method is used to fill users earning deduction entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersED(SqlDataReader oSqlDataReader)
        {
            UsersEarningsDeduction oUsersEarningsDeductionDC;
            while (oSqlDataReader.Read())
            {
                oUsersEarningsDeductionDC = new UsersEarningsDeduction
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsAttendanceDependent = Convert.ToBoolean(oSqlDataReader["IsAttendanceDependent"]),
                    IsEarning = Convert.ToBoolean(oSqlDataReader["IsEarning"]),
                    HasFormula = Convert.ToBoolean(oSqlDataReader["HasFormula"]),
                    EarningsDeductionsValue = Convert.ToDecimal(oSqlDataReader["EarningsDeductionsValue"]),
                    Reason = Convert.ToString(oSqlDataReader["Reason"])
                };
                this.mlstUsersEarningsDeductions.Add(oUsersEarningsDeductionDC);
            }
        }

        /// <summary>
        /// This method is used to fill users earning deduction entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersSalDifferenceDetails(SqlDataReader oSqlDataReader)
        {
            UsersEarningsDeduction oUsersEarningsDeductionDC;
            while (oSqlDataReader.Read())
            {
                oUsersEarningsDeductionDC = new UsersEarningsDeduction
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    EarningsDeductionsValue = Convert.ToDecimal(oSqlDataReader["EarningsDeductionsValue"]),
                    Type = Convert.ToString(oSqlDataReader["Type"])
                };
                this.mlstUsersSalDifferenceDetails.Add(oUsersEarningsDeductionDC);
            }
        }

        /// <summary>
        /// This method is used to fill users earning deduction entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersEDConfigDetails(SqlDataReader oSqlDataReader)
        {
            SalaryDifferenceConfigDetails oSalaryDifferenceConfigDetails;
            while (oSqlDataReader.Read())
            {
                oSalaryDifferenceConfigDetails = new SalaryDifferenceConfigDetails
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    FormulaRangeId = Convert.ToInt32(oSqlDataReader["FormulaRangeId"]),
                    IsFormula = Convert.ToBoolean(oSqlDataReader["IsFormula"]),
                    IsConfigured = Convert.ToBoolean(oSqlDataReader["IsConfigured"])
                };
                this.mlstSalaryDifferenceConfigDetails.Add(oSalaryDifferenceConfigDetails);
            }
        }

        #endregion
    }
}
