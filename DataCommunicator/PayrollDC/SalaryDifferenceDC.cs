using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class SalaryDifferenceDC
    {
        #region Data Member(s)

        private List<UsersDetails> mlstUsersDetails = new List<UsersDetails>();
        private List<StaticSalaryDetails> mlstStaticSalaryDetails = new List<StaticSalaryDetails>();
        private List<StaticSalaryDetails> mlstBaseStaticSalaryDetails = new List<StaticSalaryDetails>();
        private List<SalaryDifference> mlstSalaryDifferences = new List<SalaryDifference>();
        private List<SalaryDifference> mlstPaidSalaryDifferences = new List<SalaryDifference>();
        private List<StaffBaseDetails> mlstStaffBaseDetailsList = new List<StaffBaseDetails>();
        private List<SalaryDifference> mlstCurrentMonthsPaidSalaryDifferences = new List<SalaryDifference>();

        private bool mbIsSalaryPaid;
        private bool mbIsBaseMonthsSalaryPaid;
        private string msCurrentSalaryMonth = string.Empty;
        private bool mbIsReadyToPay = false;
        private StaffBaseDetails moStaffBaseDetails;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        private StaffLeaveDetailsDC moStaffLeaveDetailsDC = new StaffLeaveDetailsDC();
        private StaffAttendanceDC moStaffAttendanceDC = new StaffAttendanceDC();
        private EarningsDeductionsDC moEarningsDeductionsDC = new EarningsDeductionsDC();
        private AmountRangeDC moAmountRangeDC = new AmountRangeDC();
        private UsersEarningsDeductionsDC moUsersEarningsDeductionsDC = new UsersEarningsDeductionsDC();
        private StaffGroupsAndEarningsDeductionsAssociationDC moStaffGroupsAndEarningsDeductionsAssociationDC = new StaffGroupsAndEarningsDeductionsAssociationDC();
        private UsersStaffGroupsAssociationDC moUsersStaffGroupsAssociationDC = new UsersStaffGroupsAssociationDC();
        private EarningDeductionFormulaDC moEarningDeductionFormulaDC = new EarningDeductionFormulaDC();
        private StaffGroupsDC moStaffGroupsDC = new StaffGroupsDC();
        private UserLeavesYearwiseConfigurationDC moUserLeavesYearwiseConfigurationDC = new UserLeavesYearwiseConfigurationDC();

        #endregion

        #region Constructor(s)

        public SalaryDifferenceDC()
        {
        }

        public SalaryDifferenceDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)

        public bool IsSalaryPaid
        {
            get { return mbIsSalaryPaid; }
        }

        public bool IsBaseMonthsSalaryPaid
        {
            get { return mbIsBaseMonthsSalaryPaid; }
        }

        public string CurrentSalaryMonth
        {
            get { return msCurrentSalaryMonth; }
        }

        public bool IsReadyToPay
        {
            get { return mbIsReadyToPay; }
        }

        public List<UsersDetails> UsersDetails
        {
            get { return mlstUsersDetails; }
        }

        public List<StaticSalaryDetails> StaticSalaryDetails
        {
            get { return mlstStaticSalaryDetails; }
        }

        public List<StaticSalaryDetails> BaseStaticSalaryDetails
        {
            get { return mlstBaseStaticSalaryDetails; }
        }

        public List<SalaryDifference> SalaryDifferences
        {
            get { return mlstSalaryDifferences; }
        }

        public List<SalaryDifference> PaidSalaryDifferences
        {
            get { return mlstPaidSalaryDifferences; }
        }

        public List<StaffBaseDetails> StaffBaseDetailsList
        {
            get { return mlstStaffBaseDetailsList; }
        }

        public List<SalaryDifference> CurrentMonthsPaidSalaryDifferences
        {
            get { return mlstCurrentMonthsPaidSalaryDifferences; }
        }

        public StaffLeaveDetailsDC StaffLeaveDetailsDC
        {
            get { return moStaffLeaveDetailsDC; }
        }

        public StaffAttendanceDC StaffAttendanceDC
        {
            get { return moStaffAttendanceDC; }
        }

        public EarningsDeductionsDC EarningsDeductionsDC
        {
            get { return moEarningsDeductionsDC; }
        }

        public AmountRangeDC AmountRangeDC
        {
            get { return moAmountRangeDC; }
        }

        public UsersEarningsDeductionsDC UsersEarningsDeductionsDC
        {
            get { return moUsersEarningsDeductionsDC; }
        }

        public StaffGroupsAndEarningsDeductionsAssociationDC StaffGroupsAndEarningsDeductionsAssociationDC
        {
            get { return moStaffGroupsAndEarningsDeductionsAssociationDC; }
        }

        public UsersStaffGroupsAssociationDC UsersStaffGroupsAssociationDC
        {
            get { return moUsersStaffGroupsAssociationDC; }
        }

        public EarningDeductionFormulaDC EarningDeductionFormulaDC
        {
            get { return moEarningDeductionFormulaDC; }
        }

        public StaffGroupsDC StaffGroupsDC
        {
            get { return moStaffGroupsDC; }
        }

        public UserLeavesYearwiseConfigurationDC UserLeavesYearwiseConfigurationDC
        {
            get { return moUserLeavesYearwiseConfigurationDC; }
        }

        public StaffBaseDetails StaffBaseDetails
        {
            get { return moStaffBaseDetails; }
            set { moStaffBaseDetails = value; }
        }

        public void GetPaidSalaryDifferenceDetails(int aiMOnthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMOnthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPaidSalaryDifferenceDetails"))
                {

                    if (oSqlDataReader != null)
                    {
                        SalaryDifference oSalaryDifference;
                        while (oSqlDataReader.Read())
                        {
                            oSalaryDifference = new SalaryDifference
                            {
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                MonthId = Convert.ToInt32(oSqlDataReader["PaidMonthId"]),
                                Year = Convert.ToInt32(oSqlDataReader["PaidYearId"]),
                                Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                                Name = Convert.ToString(oSqlDataReader["Name"]),
                                Designation = Convert.ToString(oSqlDataReader["Designation"])
                            };
                            mlstSalaryDifferences.Add(oSalaryDifference);
                        }
                    }
                }
            }
        }

        public void Save(string asSalaryDifferenceXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", StaffBaseDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", StaffBaseDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", StaffBaseDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SalaryDifferenceXml", asSalaryDifferenceXml, SqlDbType.Xml);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_InsertSalaryDifferenceDetails"))
                FillUserDetails(oSqlDataReader);
            }
        }

        public void Delete()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", moStaffBaseDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moStaffBaseDetails.Year, SqlDbType.Int);
                if (moStaffBaseDetails.UserId != 0)
                    oSQLServerDbUtility.AddParameter("UserId", moStaffBaseDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSalaryDifference");
            }
        }

        /// <summary>
        /// This method is used to return salary difference configuration details.
        /// </summary>
        /// <param name="abShowDefault"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        public void GetSalaryDifferenceConfigDetails(bool abShowDefault, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowDefault", abShowDefault, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryDifferenceConfiguration"))
                {
                    if (oSqlDataReader != null)
                    {
                        moUsersEarningsDeductionsDC.SetUsersEDConfigDetails(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            moEarningDeductionFormulaDC.SetEarningsDeductionsFormulaToConfig(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            moAmountRangeDC.SetAmountRangesToConfig(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            moUsersEarningsDeductionsDC.SetUsersFormulaAndRangesToConfig(oSqlDataReader);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to return saved salary difference.
        /// </summary>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <param name="abShowPaid"></param>
        /// <returns></returns>
        public List<SavedSalaryDifference> GetSavedSalaryDifferenceDetails(int aiMonthId, int aiYear, bool abShowPaid)
        {
            List<SavedSalaryDifference> lstSalaryDifferenceInDetails = new List<SavedSalaryDifference>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowPaid", abShowPaid, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryDifferenceInDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        SavedSalaryDifference oSalaryDifferenceInDetails = new SavedSalaryDifference
                        {
                            SalaryDifferenceId = Convert.ToInt32(oSqlDataReader["SalaryDifferenceId"]),
                            EarningDeductionName = Convert.ToString(oSqlDataReader["EarningDeductionName"]),
                            Amount = Convert.ToInt32(oSqlDataReader["Amount"]),
                            IsLastTransaction = Convert.ToBoolean(oSqlDataReader["IsLastTransaction"]),
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            Designation = Convert.ToString(oSqlDataReader["Designation"]),
                            EarningDeductionId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"])
                        };
                        lstSalaryDifferenceInDetails.Add(oSalaryDifferenceInDetails);
                    }
                    return lstSalaryDifferenceInDetails;
                }
            }
        }
        
        /// <summary>
        /// This method is used to save configuration.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiConfigId"></param>
        /// <param name="aiUserId"></param>
        public void SaveConfig(string asXml, int aiConfigId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSalaryDifferenceConfig");
            }
        }

        /// <summary>
        /// This method is used to delete last transaction.
        /// </summary>
        /// <param name="aiSalaryDifferenceId"></param>
        /// <param name="aiUserId"></param>
        public void DeleteLastTransaction(int aiSalaryDifferenceId, int aiUserId)
        {
            string sUpdateStatement = "UPDATE SalaryDifference" +
                                      " SET Is_Deleted = N'Y'," +
                                      " UpdateDate = GETDATE()," +
                                      " UpdatedById = " + aiUserId +
                                      " WHERE SalaryDifferenceId = " + aiSalaryDifferenceId +
                                      " AND SchoolId = " + miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        #endregion

        #region Get Salary Detail for difference

        public void GetSalaryDifferenceEntities(int aiMonthId, int aiYear, int aiBaseMonthId, int aiBaseYearId)
        {
            mlstUsersDetails.Clear();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttendanceMonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttendanceYear", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BaseMonthId", aiBaseMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BaseYearId", aiBaseYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryDifferenceDetails"))
                {
                    oSqlDataReader.Read();
                    mbIsSalaryPaid = Convert.ToBoolean(oSqlDataReader["IsSalaryPaid"]);
                    msCurrentSalaryMonth = Convert.ToString(oSqlDataReader["CurrentSalaryMonth"]);
                    mbIsBaseMonthsSalaryPaid = Convert.ToBoolean(oSqlDataReader["IsBaseMonthsSalaryPaid"]);

                    if (mbIsSalaryPaid)
                    {
                        if (oSqlDataReader.NextResult())
                            SetSalaryDetails(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            SetUsersDetails(oSqlDataReader);

                        if (mbIsBaseMonthsSalaryPaid)
                        {
                            if (oSqlDataReader.NextResult())
                                moEarningsDeductionsDC.SetEarningsDeductions(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetBaseSalaryDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetSalaryDiference(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                            {
                                if (oSqlDataReader.Read())
                                    mbIsReadyToPay = Convert.ToBoolean(oSqlDataReader["IsReadyToPay"]);
                            }

                            if (oSqlDataReader.NextResult())
                                FillUserDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetCurrentMonthsPaidSalaryDifference(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersEarningsDeductionsDC.SetUsersSalDifferenceDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moEarningDeductionFormulaDC.SetEarningsDeductionsFormula(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffGroupsAndEarningsDeductionsAssociationDC.SetSGEDAssociation(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersStaffGroupsAssociationDC.SetUsersSGAssociation(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersEarningsDeductionsDC.SetUsersFormulaAndRanges(oSqlDataReader);
                        }
                        else
                        {
                            if (oSqlDataReader.NextResult())
                                moStaffGroupsDC.SetStaffGroups(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moEarningsDeductionsDC.SetEarningsDeductions(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moEarningDeductionFormulaDC.SetEarningsDeductionsFormula(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moAmountRangeDC.SetAmountRanges(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moAmountRangeDC.SetMonthwiseAmounts(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffLeaveDetailsDC.SetLeaves(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffAttendanceDC.SetAttendance(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffLeaveDetailsDC.SetStaffLeavesDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersStaffGroupsAssociationDC.SetUsersSGAssociation(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersEarningsDeductionsDC.SetUsersED(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                UserLeavesYearwiseConfigurationDC.SetUsersLeaves(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffGroupsAndEarningsDeductionsAssociationDC.SetSGEDAssociation(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersEarningsDeductionsDC.SetUsersFormulaAndRanges(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetSalaryDiference(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetPaidSalaryDiference(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffLeaveDetailsDC.SetLateMarkConfig(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffLeaveDetailsDC.SetLateMarkDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moStaffLeaveDetailsDC.SetStaffHolidayConfiguration(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                            {
                                if (oSqlDataReader.Read())
                                    mbIsReadyToPay = Convert.ToBoolean(oSqlDataReader["IsReadyToPay"]);
                            }

                            if (oSqlDataReader.NextResult())
                                FillUserDetails(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                SetCurrentMonthsPaidSalaryDifference(oSqlDataReader);

                            if (oSqlDataReader.NextResult())
                                moUsersEarningsDeductionsDC.SetUsersSalDifferenceDetails(oSqlDataReader);
                        }
                    }
                }
            }
        }

        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            StaffBaseDetails oStaffBaseDetails = null;
            while (aoSqlDataReader.Read())
            {
                oStaffBaseDetails = new StaffBaseDetails
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"])
                };
                mlstStaffBaseDetailsList.Add(oStaffBaseDetails);
            }
        }

        private void SetSalaryDiference(SqlDataReader oSqlDataReader)
        {
            SalaryDifference oSalaryDifference;
            while (oSqlDataReader.Read())
            {
                oSalaryDifference = new SalaryDifference
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                    Year = Convert.ToInt32(oSqlDataReader["Year"])
                };
                mlstSalaryDifferences.Add(oSalaryDifference);
            }
        }

        private void SetPaidSalaryDiference(SqlDataReader oSqlDataReader)
        {
            SalaryDifference oSalaryDifference;
            while (oSqlDataReader.Read())
            {
                oSalaryDifference = new SalaryDifference
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"])
                };
                mlstPaidSalaryDifferences.Add(oSalaryDifference);
            }
        }

        private void SetCurrentMonthsPaidSalaryDifference(SqlDataReader oSqlDataReader)
        {
            SalaryDifference oSalaryDifference;
            while (oSqlDataReader.Read())
            {
                oSalaryDifference = new SalaryDifference
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    AmountToBePaid = Convert.ToDecimal(oSqlDataReader["AmountToBePaid"])
                };
                mlstCurrentMonthsPaidSalaryDifferences.Add(oSalaryDifference);
            }
        }

        private void SetSalaryDetails(SqlDataReader oSqlDataReader)
        {
            StaticSalaryDetails oStaticSalaryDetails;
            while (oSqlDataReader.Read())
            {
                oStaticSalaryDetails = new StaticSalaryDetails
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    SalaryDetailsXml = Convert.ToString(oSqlDataReader["SalaryDetailsXml"])
                };
                mlstStaticSalaryDetails.Add(oStaticSalaryDetails);
            }
        }

        private void SetBaseSalaryDetails(SqlDataReader oSqlDataReader)
        {
            StaticSalaryDetails oStaticSalaryDetails;
            while (oSqlDataReader.Read())
            {
                oStaticSalaryDetails = new StaticSalaryDetails
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    SalaryDetailsXml = Convert.ToString(oSqlDataReader["SalaryDetailsXml"])
                };
                mlstBaseStaticSalaryDetails.Add(oStaticSalaryDetails);
            }
        }

        private void SetUsersDetails(SqlDataReader oSqlDataReader)
        {
            UsersDetails oUsersDetails;
            while (oSqlDataReader.Read())
            {
                oUsersDetails = new UsersDetails
                {
                    SrNo = Convert.ToInt32(oSqlDataReader["SrNo"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Name = Convert.ToString(oSqlDataReader["Name"]),
                    Designation = Convert.ToString(oSqlDataReader["Designation"]),
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupId"]),
                    OriginalStaffGroupsId = Convert.ToInt32(oSqlDataReader["OriginalStaffGroupId"]),
                };
                mlstUsersDetails.Add(oUsersDetails);
            }
        }

        #endregion
    }
}
