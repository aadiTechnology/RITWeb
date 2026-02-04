/* File Name - SalaryDifferenceBL.cs
 * Created By - Sachin
 * Description - This class is used to calculate salary difference of selected month against current configuraation. 
 * Modified By-  Sachin
 * Modified Date - 14 August 2012
 * Descri[tion - Facility to calculate salary difference of selected month against selected month.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using DataCommunicator;
using PayrollEntities;
using Utility;
using System.Diagnostics;

namespace BusinessLogic
{
    public class SalaryDifferenceBL
    {
        #region Data Members

        private int miTotalPages;
        private int miTotalRecords;
        private int miDaysOfMonth = 0;
        private int miMonthId = 0;
        private int miYearId = 0;
        private string msMonthList = string.Empty;
        private SalaryDifferenceDC moSalaryDifferenceDC;
        private DataTable moDTSalaryDetails;
        private List<int> mlstUserIds = new List<int>();
        private List<int> mlstEarningsDeductions = new List<int>();
        private List<string> mlstAttendanceDependentColumns = new List<string>();
        private List<string> mlstTotalEarningsDeductions;
        private List<ConfiguredDefaultLeaves> mlstConfiguredLeaves = new List<ConfiguredDefaultLeaves>();
        private List<SalaryDifferenceClass> mlstSalaryDifferenceClassList = new List<SalaryDifferenceClass>();
        private List<EarningsDeductions> molstEarningsDeductions = new List<EarningsDeductions>();
        private Dictionary<int, int> mdictPaidSalary = new Dictionary<int, int>();

        private StaffLeaveDetailsBL moStaffLeaveDetailsBL;
        private StaffAttendanceBL moStaffAttendanceBL;
        private EarningsDeductionsBL moEarningsDeductionsBL;
        private AmountRangeBL moAmountRangeBL;
        private UsersEarningsDeductionsBL moUsersEarningsDeductionsBL;
        private StaffGroupsAndEarningsDeductionsAssociationBL moStaffGroupsAndEarningsDeductionsAssociationBL;
        private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;
        private EarningDeductionFormulaBL moEarningDeductionFormulaBL;
        private UserLeavesYearwiseConfigurationBL moUserLeavesYearwiseConfigurationBL;

        #endregion

        #region Constant

        private const int I_PAID_SALARY_DETAILS_TABLE_INDEX = 1;

        private const int I_GRID_PAGE_COUNT = 7;
        private const int I_BASE_CONFIGURATION_TABLE_INDEX = 2;
        private const string S_HOLIDAY_LEAVE = "Holiday Leaves";
        private const string S_NO_RECORD_FOUND_MESSAGE = "No Record Found.";
        private const string S_SALARY_DIFFERENCE_ROW_COLUMN = "IsSalaryDifferenceRow";
        private const string S_USER_ID = "UserId";
        private const string S_LEAVE_DEDUCTED = "Leave Deducted ";
        private const string S_ATTENDANCE = "Attendance";
        private const string S_UNPAID_LEAVES = "Unpaid Leaves";
        private const string S_EARNING_DEDUCTION_SEPARATOR = "_ED_";
        private const string S_LEAVE_DEDUCTED_EARNING_DEDUCTION_SEPARATOR = "_LD";
        private const string S_EARNING_DEDUCTION_NOT_APPLICABLE = "-1";

        #endregion

        #region Constructor

        public SalaryDifferenceBL()
        {
            moSalaryDifferenceDC = new SalaryDifferenceDC();
        }

        public SalaryDifferenceBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moSalaryDifferenceDC = new SalaryDifferenceDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public bool IsReadyToPaySalary
        {
            get { return moSalaryDifferenceDC.IsReadyToPay; }
        }

        public int TotalPages
        {
            get { return miTotalPages; }
            set { miTotalPages = value; }
        }

        public int TotalRecords
        {
            get { return miTotalRecords; }
            set { miTotalRecords = value; }
        }

        /// <summary>
        /// Gets attendance dependent column names.
        /// </summary>
        public List<string> AttendanceDependentColumns
        {
            get { return mlstAttendanceDependentColumns; }
        }

        /// <summary>
        /// Gets earning deduction details.
        /// </summary>
        public List<EarningsDeductions> EarningsDeductions
        {
            get { return moEarningsDeductionsBL.EarningsDeductions; }
        }

        /// <summary>
        /// Gets selected months salary difference.
        /// </summary>
        public List<SalaryDifference> SalaryDifferences
        {
            get { return moSalaryDifferenceDC.SalaryDifferences; }
        }

        /// <summary>
        /// Gets and Sets staff basic details.
        /// </summary>
        public StaffBaseDetails StaffBaseDetails
        {
            get { return moSalaryDifferenceDC.StaffBaseDetails; }
            set { moSalaryDifferenceDC.StaffBaseDetails = value; }
        }

        /// <summary>
        /// Gets list of SalaryDifferenceClass object.
        /// </summary>
        public List<SalaryDifferenceClass> SalaryDifferenceClassList
        {
            get { return mlstSalaryDifferenceClassList; }
            set { mlstSalaryDifferenceClassList = value; }
        }

        public List<EarningsDeductions> EarningsDeductionsToSave
        {
            set { molstEarningsDeductions = value; }
        }

        /// <summary>
        /// Gets staff basic details.
        /// </summary>
        public List<StaffBaseDetails> StaffBaseDetailsList
        {
            get { return moSalaryDifferenceDC.StaffBaseDetailsList; }
        }

        // --------- Accessed from salary difference config screen ----------

        public UsersEarningsDeductionsBL UsersEarningsDeductionsBL
        {
            get { return moUsersEarningsDeductionsBL; }
        }

        public EarningDeductionFormulaBL EarningDeductionFormulaBL
        {
            get { return moEarningDeductionFormulaBL; }
        }

        public AmountRangeBL AmountRangeBL
        {
            get { return moAmountRangeBL; }
        }

        // ------------------------------------------------------------------

        #endregion

        #region Methods

        #region Update Database Tables

        /// <summary>
        /// This method is used to save salary difference.
        /// </summary>
        public void Save(int aiUserid, DataTable aoDTSalaryDifference)
        {
            string sSalaryDifferenceXml = GenerateXml(aiUserid, aoDTSalaryDifference);
            moSalaryDifferenceDC.Save(sSalaryDifferenceXml);
        }

        /// <summary>
        /// This method is used to delete salary difference.
        /// </summary>
        public void Delete()
        {
            moSalaryDifferenceDC.Delete();
        }

        /// <summary>
        /// This method is used to return salary difference configuration details.
        /// </summary>
        /// <param name="abShowDefault"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        public void GetSalaryDifferenceConfigDetails(bool abShowDefault, int aiMonthId, int aiYear)
        {
            moUsersEarningsDeductionsBL = new UsersEarningsDeductionsBL();
            moEarningDeductionFormulaBL = new EarningDeductionFormulaBL();
            moAmountRangeBL = new BusinessLogic.AmountRangeBL();

            moSalaryDifferenceDC.GetSalaryDifferenceConfigDetails(abShowDefault, aiMonthId, aiYear);
            moUsersEarningsDeductionsBL.SalaryDifferenceConfigDetails = moSalaryDifferenceDC.UsersEarningsDeductionsDC.SalaryDifferenceConfigDetails;
            moUsersEarningsDeductionsBL.UsersFormulaAndRanges = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersFormulaeAndRanges;
            moEarningDeductionFormulaBL.EarningsDeductionsFormulae = moSalaryDifferenceDC.EarningDeductionFormulaDC.EarningsDeductionsFormulae;
            moAmountRangeBL.AmountRanges = moSalaryDifferenceDC.AmountRangeDC.AmountRanges;
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
            return moSalaryDifferenceDC.GetSavedSalaryDifferenceDetails(aiMonthId, aiYear, abShowPaid);
        }

        /// <summary>
        /// This method is used to save configuration details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiConfigId"></param>
        /// <param name="aiUserId"></param>
        public void SaveConfig(string asXml, int aiConfigId, int aiUserId)
        {
            moSalaryDifferenceDC.SaveConfig(asXml, aiConfigId, aiUserId);
        }

        /// <summary>
        /// This method is used to delete last transaction.
        /// </summary>
        /// <param name="aiSalaryDifferenceId"></param>
        /// <param name="aiUserId"></param>
        public void DeleteLastTransaction(int aiSalaryDifferenceId, int aiUserId)
        {
            moSalaryDifferenceDC.DeleteLastTransaction(aiSalaryDifferenceId, aiUserId);
        }

        #endregion

        #region Basic Details

        /// <summary>
        /// This method is used to set baic details.
        /// </summary>
        /// <param name="aoUserDetails"></param>
        /// <param name="aiRowIndex"></param>
        private void SetBasicDetails(UsersBasicDetails aoUserDetails, int aiRowIndex)
        {
            moDTSalaryDetails.Rows[aiRowIndex]["UserId"] = aoUserDetails.UserId;
            moDTSalaryDetails.Rows[aiRowIndex]["OriginalStaffGroupsId"] = aoUserDetails.OriginalStaffGroupId;
            moDTSalaryDetails.Rows[aiRowIndex]["SortOrder"] = 0;
            moDTSalaryDetails.Rows[aiRowIndex]["Sr No"] = (aiRowIndex + 1);
            moDTSalaryDetails.Rows[aiRowIndex]["Name"] = aoUserDetails.Name;
            moDTSalaryDetails.Rows[aiRowIndex]["Designation"] = aoUserDetails.Designation;
            moDTSalaryDetails.Rows[aiRowIndex]["DisplayControls"] = Constants.S_YES;
            moDTSalaryDetails.Rows[aiRowIndex]["TotalSortOrder"] = 0;
            moDTSalaryDetails.Rows[aiRowIndex]["StaffGroupId"] = aoUserDetails.StaffGroupId;
        }

        /// <summary>
        /// This method is used to set display of save button.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiUserId"></param>
        private void SetDisplayOfSaveButton(int aiRowIndex, int aiUserId)
        {
            moDTSalaryDetails.Rows[aiRowIndex]["DisplayControls"] = moUserLeavesYearwiseConfigurationBL.UserLeaveConfiguration.Where(user => user.UserId == aiUserId).Count() == 0 ? Constants.S_NO : Constants.S_YES;
        }

        /// <summary>
        /// This method is sued to populate salary difference class.
        /// </summary>
        /// <param name="asShortName"></param>
        /// <param name="aoType"></param>
        /// <param name="aiEarningsDeductionsId"></param>
        public void PopulateSalaryDifferenceClass(string asShortName, string aoType, int aiEarningsDeductionsId)
        {
            SalaryDifferenceClass oSalaryDifferenceClass = new SalaryDifferenceClass { ColumnName = asShortName, Type = aoType, Id = aiEarningsDeductionsId };
            int iRecordCount = mlstSalaryDifferenceClassList.Where(salDiff => salDiff.ColumnName == oSalaryDifferenceClass.ColumnName && salDiff.Id == oSalaryDifferenceClass.Id && salDiff.Type == oSalaryDifferenceClass.Type).Count();

            if (iRecordCount == 0)
                mlstSalaryDifferenceClassList.Add(oSalaryDifferenceClass);
        }

        #endregion

        #region Earning and Deductions

        /// <summary>
        /// This method is used to set default values to earning-deduction if ED is not assocaited.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiUserId"></param>
        private void SetDefaultEDValuesIfNotAssociated(int aiRowIndex, int aiUserId)
        {
            List<int> olstAvailableEDs = moUsersEarningsDeductionsBL.GetUsersEarningDeductionIds(aiUserId,moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);
            List<int> olstRemainingEarnDeducts1 = mlstEarningsDeductions.Except(olstAvailableEDs).ToList();
            moEarningsDeductionsBL.SetEarnDeductDefaultValues(aiRowIndex, olstRemainingEarnDeducts1);
        }

        /// <summary>
        /// This method is used to set earning deductions.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="adcTotalDays"></param>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiEarningsSum"></param>
        /// <param name="aiDeductionSum"></param>
        /// <returns></returns>
        private List<UsersEarnDeductDetails> SetEarningDeductions(int aiUserId, decimal adcTotalDays, int aiRowIndex, ref int aiEarningsSum, ref int aiDeductionSum)
        {
            List<UsersEarnDeductDetails> oUsersEarningsDeductions = moUsersEarningsDeductionsBL.GetUsersEarningDeductions(aiUserId, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);
            
            // Get all earnings and deductions associated with user if not found then set zero value for eah earning deduction.
            if (oUsersEarningsDeductions.Count() > 0)
                SetUsersEarningDeduction(adcTotalDays, aiRowIndex, ref aiEarningsSum, ref aiDeductionSum, oUsersEarningsDeductions);
            else
                moEarningsDeductionsBL.SetDefaultEDValuesIfNotAvail(aiRowIndex, aiUserId, moUsersStaffGroupsAssociationBL.UsersSGAssociations);

            return oUsersEarningsDeductions;
        }

        /// <summary>
        /// This method is used to set earning deductions.
        /// </summary>
        /// <param name="adcTotalDays"></param>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiEarningsSum"></param>
        /// <param name="aiDeductionSum"></param>
        /// <param name="oUsersEarningsDeductions"></param>
        private void SetUsersEarningDeduction(decimal adcTotalDays, int aiRowIndex, ref int aiEarningsSum, ref int aiDeductionSum, List<UsersEarnDeductDetails> aoUsersEarningsDeductions)
        {
            int EDValue = 0;
            int iLeaveDeductedValue = 0;

            // Add details of users earning deduction in base table.
            foreach (var usersED in aoUsersEarningsDeductions)
            {
                EDValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(usersED.EarningsDeductionsValue)));
                PopulateSalaryDifferenceClass(usersED.ShortName, PayrollConstants.ED, usersED.EarningsDeductionsId);

                moDTSalaryDetails.Rows[aiRowIndex][usersED.ShortName.ToString()] = EDValue;

                // if earning deduction is attendance dependent then value will be calculated according to present days.
                if (Convert.ToBoolean(usersED.IsAttendanceDependent) == true)
                {
                    iLeaveDeductedValue = Convert.ToInt32(Math.Round((adcTotalDays / miDaysOfMonth) * EDValue));

                    moDTSalaryDetails.Rows[aiRowIndex][String.Format("Leave Deducted {0}", usersED.ShortName)] = iLeaveDeductedValue;
                    PopulateSalaryDifferenceClass(usersED.ShortName, PayrollConstants.LD, usersED.EarningsDeductionsId);

                    if (Convert.ToBoolean(usersED.IsEarning))
                        aiEarningsSum = aiEarningsSum + iLeaveDeductedValue;
                    else
                        aiDeductionSum = aiDeductionSum + iLeaveDeductedValue;
                }
                else
                {
                    if (Convert.ToBoolean(usersED.IsEarning))
                        aiEarningsSum = aiEarningsSum + EDValue;
                    else
                        aiDeductionSum = aiDeductionSum + EDValue;
                }
            }
        }

        /// <summary>
        /// This method is used to ser earing deduction formula.
        /// </summary>
        /// <param name="aolstUsersEarningsDeductions"></param>
        /// <param name="aiUserId"></param>
        /// <param name="adcTotalDays"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiEarningsSum"></param>
        /// <param name="aiDeductionSum"></param>
        private void SetEarningDeductionFormula(List<UsersEarnDeductDetails> aolstUsersEarningsDeductions, int aiUserId, decimal adcTotalDays, int aiStaffGroupId, int aiRowIndex, ref int aiEarningsSum, ref int aiDeductionSum)
        {
            int iLeaveDeductedED = 0;
            int iEDFormulaValue = 0;

            // Get all earning deduction formulae.
            List<EarningsDeductions> oEarningsDeductionsFormulae = moEarningDeductionFormulaBL.GetEarningDeductionFormulae(aiUserId, moUsersStaffGroupsAssociationBL.UsersSGAssociations, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);
            List<UsersEarnDeductDetails> oUsersEDForFormulae = aolstUsersEarningsDeductions;

            // iterate each formula.
            foreach (var EDFormula in oEarningsDeductionsFormulae)
            {
                string sFormula = moEarningDeductionFormulaBL.GetFormula(EDFormula, aiUserId);
                oUsersEDForFormulae = aolstUsersEarningsDeductions;

                // Replace formula values woth actual amount.
                foreach (var usersED in oUsersEDForFormulae)
                {
                    iEDFormulaValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(usersED.EarningsDeductionsValue)));
                    if (Convert.ToBoolean(usersED.IsAttendanceDependent))
                        iEDFormulaValue = Convert.ToInt32(Math.Round((adcTotalDays / miDaysOfMonth) * iEDFormulaValue));

                    sFormula = sFormula.Replace("'" + usersED.EarningsDeductionsId + "'", iEDFormulaValue.ToString());
                }

                sFormula = moEarningDeductionFormulaBL.GetUpdatedEDFormulaForZeroIDs(aiStaffGroupId, sFormula, moEarningsDeductionsBL.EarningsDeductions);

                // Evaluate formula.
                int valueOfED = 0;
                MathsExpressionParser oMathsExpressionParser = new MathsExpressionParser();
                if (oMathsExpressionParser.Evaluate(sFormula))
                {
                    bool bIncludeInSalaryDifference = true;

                    int iEDValue = bIncludeInSalaryDifference ? Convert.ToInt32(Math.Round(oMathsExpressionParser.Result)) : -7777;
                    moDTSalaryDetails.Rows[aiRowIndex][EDFormula.ShortName.ToString()] = iEDValue;
                    PopulateSalaryDifferenceClass(EDFormula.ShortName, PayrollConstants.ED, EDFormula.EarningsDeductionsId);

                    // If earning deduction is attendance dependent then update amount value accordig to present days.
                    if (Convert.ToBoolean(EDFormula.IsAttendanceDependent) == true)
                    {
                        iLeaveDeductedED = Convert.ToInt32(Math.Round((adcTotalDays / miDaysOfMonth) * iEDValue));
                        moDTSalaryDetails.Rows[aiRowIndex][EDFormula.ShortName.ToString()] = iLeaveDeductedED;

                        moDTSalaryDetails.Rows[aiRowIndex][String.Format("Leave Deducted {0}", EDFormula.ShortName)] = iLeaveDeductedED;
                        PopulateSalaryDifferenceClass(EDFormula.ShortName, PayrollConstants.LD, EDFormula.EarningsDeductionsId);

                        if (Convert.ToBoolean(EDFormula.IsEarning))
                            aiEarningsSum = aiEarningsSum + iLeaveDeductedED;
                        else
                            aiDeductionSum = aiDeductionSum + iLeaveDeductedED;
                        valueOfED = iLeaveDeductedED;
                    }
                    else
                    {
                        if (Convert.ToBoolean(EDFormula.IsEarning))
                            aiEarningsSum = aiEarningsSum + iEDValue;
                        else
                            aiDeductionSum = aiDeductionSum + iEDValue;
                        valueOfED = iEDValue;
                    }
                }

                int iEDId = Convert.ToInt32(EDFormula.EarningsDeductionsId);

                List<UsersEarnDeductDetails> olstAppendEDs = moEarningsDeductionsBL.GetEarningDeductions(valueOfED, iEDId);

                aolstUsersEarningsDeductions = oUsersEDForFormulae.Union(olstAppendEDs).ToList();
            }
        }

        /// <summary>
        /// This method is used to set range values.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiEarningsSum"></param>
        /// <param name="aiDeductionSum"></param>
        /// <param name="adcTotalDays"></param>
        /// <param name="aiDaysOfMonth"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        private int SetEarningsDeductionsRange(int aiRowIndex, int aiUserId, int aiEarningsSum, ref int aiDeductionSum, decimal adcTotalDays, int aiDaysOfMonth, int aiMonthId)
        {
            dynamic oEarningsDeductionsRange = moAmountRangeBL.GetAmountRangeDetails(aiEarningsSum, aiUserId, aiMonthId, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations, moUsersStaffGroupsAssociationBL.UsersSGAssociations);

            int iAmount = 0;
            //  Iterate each earning deduction range.
            foreach (var EDRange in oEarningsDeductionsRange)
            {
                iAmount = moAmountRangeBL.GetUserAmountRange(aiUserId, aiEarningsSum, aiMonthId, iAmount, EDRange);

                bool bIncludeInSalaryDifference = true;
                if (!bIncludeInSalaryDifference)
                    iAmount = -7777;

                moDTSalaryDetails.Rows[aiRowIndex][EDRange.ShortName.ToString()] = iAmount;
                PopulateSalaryDifferenceClass(EDRange.ShortName, PayrollConstants.ED, EDRange.EarningsDeductionsId);

                // If range is attendance dependent then update amount value according to present days.
                if (Convert.ToBoolean(EDRange.IsAttendanceDependent))
                {
                    moDTSalaryDetails.Rows[aiRowIndex][EDRange.ShortName.ToString()] = Convert.ToInt32(Math.Round((adcTotalDays / aiDaysOfMonth) * iAmount));
                    PopulateSalaryDifferenceClass(EDRange.ShortName, PayrollConstants.LD, EDRange.EarningsDeductionsId);
                }

                if (Convert.ToBoolean(EDRange.IsEarning))
                    aiEarningsSum = aiEarningsSum + iAmount;
                else
                    aiDeductionSum = aiDeductionSum + iAmount;
            }
            return aiEarningsSum;
        }

        #endregion

        #region Input and Database call

        /// <summary>
        /// This method is used to return salary difference tables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiMOnthId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiBaseMonthId"></param>
        /// <param name="aiBaseYearId"></param>
        private void GetSalaryDifferenceEntities(int aiMonthId, int aiYear, int aiBaseMonthId, int aiBaseYearId)
        {
            miMonthId = aiMonthId;
            miYearId = aiYear;
            moSalaryDifferenceDC.GetSalaryDifferenceEntities(aiMonthId, aiYear, aiBaseMonthId, aiBaseYearId);            
            PopulatePayrollBLObjects();
        }

        /// <summary>
        /// This method is used to populate payroll objects.
        /// </summary>
        private void PopulatePayrollBLObjects()
        {
            moStaffLeaveDetailsBL = new StaffLeaveDetailsBL();
            moStaffAttendanceBL = new StaffAttendanceBL();
            moEarningsDeductionsBL = new EarningsDeductionsBL(moDTSalaryDetails);
            moAmountRangeBL = new AmountRangeBL();
            moUsersEarningsDeductionsBL = new UsersEarningsDeductionsBL();
            moStaffGroupsAndEarningsDeductionsAssociationBL = new StaffGroupsAndEarningsDeductionsAssociationBL();
            moUsersStaffGroupsAssociationBL = new UsersStaffGroupsAssociationBL();
            moEarningDeductionFormulaBL = new EarningDeductionFormulaBL();
            moUserLeavesYearwiseConfigurationBL = new UserLeavesYearwiseConfigurationBL();

            moEarningDeductionFormulaBL.UsersFormulaeAndRanges = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersFormulaeAndRanges;
            moEarningDeductionFormulaBL.EarningsDeductions = moSalaryDifferenceDC.EarningsDeductionsDC.EarningsDeductions;
            
            moStaffLeaveDetailsBL.ConfiguredLeaves = moSalaryDifferenceDC.StaffLeaveDetailsDC.ConfiguredLeaves;
            moStaffLeaveDetailsBL.UserLateMarkLeaves = moSalaryDifferenceDC.StaffLeaveDetailsDC.UserLateMarkLeaves;
            moStaffLeaveDetailsBL.LateMarkConfigurations = moSalaryDifferenceDC.StaffLeaveDetailsDC.LateMarkConfigurations;
            moStaffLeaveDetailsBL.StaffLeaveDetails = moSalaryDifferenceDC.StaffLeaveDetailsDC.StaffLeaveDetails;
            moStaffLeaveDetailsBL.UsersSalaryDeductions = moSalaryDifferenceDC.StaffLeaveDetailsDC.UsersSalaryDeductions;

            moStaffAttendanceBL.StaffAttendanceDetails = moSalaryDifferenceDC.StaffAttendanceDC.StaffAttendanceDetails;

            moEarningsDeductionsBL.EarningsDeductions = moSalaryDifferenceDC.EarningsDeductionsDC.EarningsDeductions;
            moEarningsDeductionsBL.StaffGroupsEarningDeductionAssociations = moSalaryDifferenceDC.StaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociations;
            moEarningsDeductionsBL.SalaryDifferenceBL = this;

            moAmountRangeBL.AmountRanges = moSalaryDifferenceDC.AmountRangeDC.AmountRanges;
            moAmountRangeBL.MonthwiseAmounts = moSalaryDifferenceDC.AmountRangeDC.MonthwiseAmounts;
            moAmountRangeBL.UsersFormulaeAndRanges = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersFormulaeAndRanges;
            moAmountRangeBL.EarningsDeductions = moSalaryDifferenceDC.EarningsDeductionsDC.EarningsDeductions;

            moUsersEarningsDeductionsBL.UsersFormulaAndRanges = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersFormulaeAndRanges;
            moUsersEarningsDeductionsBL.UsersEarningsDeductionDetails = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersEarningsDeductions;
            moUsersEarningsDeductionsBL.UsersSalDifferenceDetails = moSalaryDifferenceDC.UsersEarningsDeductionsDC.UsersSalDifferenceDetails;
            moUsersEarningsDeductionsBL.EarningsDeductions = moSalaryDifferenceDC.EarningsDeductionsDC.EarningsDeductions;
            moUsersEarningsDeductionsBL.UsersSGAssociations = moSalaryDifferenceDC.UsersStaffGroupsAssociationDC.UsersSGAssociations;

            moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations = moSalaryDifferenceDC.StaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociations;
            moUsersStaffGroupsAssociationBL.UsersSGAssociations = moSalaryDifferenceDC.UsersStaffGroupsAssociationDC.UsersSGAssociations;
            moEarningDeductionFormulaBL.EarningsDeductionsFormulae = moSalaryDifferenceDC.EarningDeductionFormulaDC.EarningsDeductionsFormulae;
            moUserLeavesYearwiseConfigurationBL.UserLeaveConfiguration = moSalaryDifferenceDC.UserLeavesYearwiseConfigurationDC.UserLeaveConfigurations;
        }

        /// <summary>
        /// This method is used to return paid salary details.
        /// </summary>
        public void GetPaidSalaryDifferenceDetails(int aiMOnthId, int aiYear)
        {
            moSalaryDifferenceDC.GetPaidSalaryDifferenceDetails(aiMOnthId, aiYear);
        }

        /// <summary>
        /// This method is used to return salary diffference dataset.
        /// </summary>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataSet GetSalaryDifferenceDataset(int aiMonthId, int aiYear, string asFilter, int aiPageIndex, int aiPageSize, int aiBaseMonthId, int aiBaseYearId)
        {
            moDTSalaryDetails = new DataTable();

            GetSalaryDifferenceEntities(aiMonthId, aiYear, aiBaseMonthId, aiBaseYearId);
            miDaysOfMonth = DateTime.DaysInMonth(aiBaseYearId, aiBaseMonthId);

            // If salary of selected month/ year is paid.
            if (moSalaryDifferenceDC.IsSalaryPaid)
            {
                DataTable ODTSalary = new DataTable();

                int iStartIndex = aiPageIndex * aiPageSize;

                List<UsersDetails> olstUsers = new List<UsersDetails>();
                List<UsersDetails> olstFilteredUsers = new List<UsersDetails>();

                // Select users according to filter and paging.
                if (!moSalaryDifferenceDC.IsBaseMonthsSalaryPaid)
                {
                    if (!string.IsNullOrEmpty(asFilter))
                    {
                        int iItemIndex = 1;
                        olstUsers = moSalaryDifferenceDC.UsersDetails.Where(user => user.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();
                        olstUsers.ForEach(user => { user.SerialNo = iItemIndex++; });
                        olstFilteredUsers = olstUsers.Where(user => user.SerialNo > iStartIndex && user.SerialNo <= iStartIndex + aiPageSize).ToList();
                    }
                    else
                        olstFilteredUsers = moSalaryDifferenceDC.UsersDetails.Where(user => user.SrNo > iStartIndex && user.SrNo <= iStartIndex + aiPageSize).ToList();                        

                    miTotalRecords = string.IsNullOrEmpty(asFilter) ? moSalaryDifferenceDC.UsersDetails.Count : olstUsers.Count;
                    mlstUserIds = olstFilteredUsers.Select(user => user.UserId).ToList();
                }
                else
                {
                   var olstFltUsers = (from ud in moSalaryDifferenceDC.UsersDetails
                                         join ubd in moSalaryDifferenceDC.BaseStaticSalaryDetails
                                         on ud.UserId equals ubd.UserId
                                         select ud).ToList();

                   if (string.IsNullOrEmpty(asFilter))
                       olstFilteredUsers = olstFltUsers.OrderBy(user => user.SrNo).Skip(iStartIndex).Take(aiPageSize).ToList();
                   else
                   {
                       int iItemIndex = 1;
                       olstUsers = olstFltUsers.Where(user => user.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();
                       olstUsers.ForEach(user => { user.SerialNo = iItemIndex++; });
                       olstFilteredUsers = olstUsers.Where(user => user.SerialNo > iStartIndex && user.SerialNo <= iStartIndex + aiPageSize).ToList();
                   }

                   miTotalRecords = string.IsNullOrEmpty(asFilter) ? olstFltUsers.Count : olstUsers.Count;

                   mlstUserIds = olstFilteredUsers.Select(user => user.UserId).ToList();
                }

                // Select user details order by serial no.
                var oUserDetails = olstFilteredUsers
                                  .OrderBy(user => user.SrNo)
                                  .Select(UserDetail =>
                                      new UsersBasicDetails
                                      {
                                          Name = UserDetail.Name,
                                          Designation = UserDetail.Designation,
                                          UserId = UserDetail.UserId,
                                          OriginalStaffGroupId = UserDetail.OriginalStaffGroupsId,
                                          StaffGroupId = UserDetail.StaffGroupsId
                                      });

                SetTotalPages(miTotalRecords);

                SetPaidSalaryMonthList();

                mlstAttendanceDependentColumns.AddRange(moEarningsDeductionsBL.GetAttendanceDependentColumns());

                decimal dcTotalDays = 0;

                // If base month's salary is not paid.
                if (!moSalaryDifferenceDC.IsBaseMonthsSalaryPaid)
                {
                    mlstTotalEarningsDeductions = new List<string>();

                    int iEarningsSum = 0;
                    int iDeductionSum = 0;

                    AddBasicColumns();
                    moStaffLeaveDetailsBL.AddAttendanceLeavesColumns(moDTSalaryDetails, mlstTotalEarningsDeductions);
                    AddEarningDeductionsColumns();
                    SetPaidSalaryDetails();
                    mlstEarningsDeductions = moEarningsDeductionsBL.GetEarningDeductionIDs();
                    mlstConfiguredLeaves = moStaffLeaveDetailsBL.SetDefaultLeaves();

                    int iRowIndex = 0;
                    decimal dcUnpaidLeaves = 0;

                    // Iterate loop for each user.
                    foreach (UsersBasicDetails userDetails in oUserDetails)
                    {
                        moDTSalaryDetails.Rows.Add();
                        SetBasicDetails(userDetails, iRowIndex);

                        dcUnpaidLeaves = moStaffLeaveDetailsBL.GetUnpaidLeavesCount(userDetails.UserId, iRowIndex, moDTSalaryDetails, moStaffAttendanceBL.StaffAttendanceDetails);

                        dcTotalDays = moStaffAttendanceBL.SetAttendanceDetails(iRowIndex, dcUnpaidLeaves, userDetails.UserId, moDTSalaryDetails, miDaysOfMonth, moStaffLeaveDetailsBL);

                        List<UsersEarnDeductDetails> UsersEarningsDeductions = SetEarningDeductions(userDetails.UserId, dcTotalDays, iRowIndex, ref iEarningsSum, ref iDeductionSum);

                        SetDefaultEDValuesIfNotAssociated(iRowIndex, userDetails.UserId);

                        SetEarningDeductionFormula(UsersEarningsDeductions, userDetails.UserId, dcTotalDays, userDetails.StaffGroupId, iRowIndex, ref iEarningsSum, ref iDeductionSum);

                        iEarningsSum = SetEarningsDeductionsRange(iRowIndex, userDetails.UserId, iEarningsSum, ref iDeductionSum, dcTotalDays, miDaysOfMonth, aiBaseMonthId);

                        SetDisplayOfSaveButton(iRowIndex, Convert.ToInt32(userDetails.UserId));

                        moDTSalaryDetails.Rows[iRowIndex][PayrollConstants.S_GROSS_SALARY] = iEarningsSum;
                        moDTSalaryDetails.Rows[iRowIndex][PayrollConstants.S_TOTAL_DEDUCTION] = iDeductionSum;
                        moDTSalaryDetails.Rows[iRowIndex][PayrollConstants.S_NET_SALARY] = iEarningsSum - iDeductionSum;

                        PopulateSalaryDifferenceClass();

                        iEarningsSum = 0;
                        iDeductionSum = 0;

                        iRowIndex++;
                    }

                    int iRowCounter = iRowIndex;

                    // return datarow collection by sorting details with original staff groups id and sortorder.
                    IEnumerable<DataRow> oSortedSalaryDetails = from SalDetails in moDTSalaryDetails.AsEnumerable()
                                                                orderby Convert.ToInt32(SalDetails["OriginalStaffGroupsId"]) ascending, Convert.ToInt32(SalDetails["SortOrder"]) ascending
                                                                select SalDetails;

                    if (oSortedSalaryDetails.Count() > 0)
                        ODTSalary = oSortedSalaryDetails.CopyToDataTable();
                }
                else
                {
                    // if base months salary is paid.
                    ODTSalary = GetBaseDetailsFromXml(ODTSalary);
                  
                    SetTotalPages(miTotalRecords);                    
                }

                if (mlstSalaryDifferenceClassList.Exists(diff => diff.ColumnName == PayrollConstants.S_SALARY_DIFFERENCE))
                    mlstSalaryDifferenceClassList.RemoveAll(diff => diff.ColumnName == PayrollConstants.S_SALARY_DIFFERENCE);

                DataSet oDS = new DataSet();
                oDS.Tables.Add(GetPaidSalaryDetails()); // Paid salary details
                oDS.Tables.Add(GetSalaryDetailsXml());  // Salary Details xml
                oDS.Tables.Add(ODTSalary); // Salary details as per current configuration
                return oDS;
            }
            else
            {
                // if salary of selected month is not paid.
                DataSet oDS = new DataSet();
                DataTable oDataTable = new DataTable();
                oDataTable.Columns.Add("IsSalaryPaid");

                oDataTable.Rows.Add();
                oDataTable.Rows[0]["IsSalaryPaid"] = Constants.S_NO;
                oDS.Tables.Add(oDataTable);
                return oDS;
            }
        }

        /// <summary>
        /// This method is used to return base details from xml format salary details.
        /// </summary>
        /// <param name="oDTSalary"></param>
        /// <returns></returns>
        private DataTable GetBaseDetailsFromXml(DataTable oDTSalary)
        {
            string sXml = string.Empty;
            string sTotalXml = string.Empty;

            // Convert xml entity list to a datatable.
            foreach (StaticSalaryDetails salDiff in moSalaryDifferenceDC.BaseStaticSalaryDetails.Where(user => user.UserId != -9999))
            {
                sXml = salDiff.SalaryDetailsXml;
                sXml = sXml.Replace("<SalaryDetails>", "");
                sXml = sXml.Replace("</SalaryDetails>", "");
                sTotalXml += sXml;
            }

            sTotalXml = "<SalaryDetailsXml>" + sTotalXml.Replace("<SalaryDetails ", "<SalaryDetailsXml ") + "</SalaryDetailsXml>";

            DataSet oDSPaidSalary = new DataSet();
            System.IO.StringReader oReader = new System.IO.StringReader(sTotalXml);
            oDSPaidSalary.ReadXml(oReader);


            int iIndex = 0;
            string sValue;
            DataTable oDTSalaryDetails = oDSPaidSalary.Tables[0];

            // Copy all columns.
            foreach (DataColumn column in oDTSalaryDetails.Columns)
                oDTSalary.Columns.Add(column.ColumnName);

            int iTotalColumnIndex = oDTSalaryDetails.Columns.IndexOf(PayrollConstants.S_TOTAL);
            
            if (oDTSalaryDetails.IsNonEmpty())
            {
                DataRow oDataRow = oDTSalaryDetails.Rows[0];
                List<string> olstColumns = new List<string>();
                for (int iColumnIndex = 0; iColumnIndex < oDTSalaryDetails.Columns.Count; iColumnIndex++)
                    olstColumns.Add(oDTSalaryDetails.Columns[iColumnIndex].ColumnName.Replace("_", " "));

                mlstAttendanceDependentColumns = olstColumns.Intersect(mlstAttendanceDependentColumns).ToList();

                sValue = string.Empty;

                // Convert data from xml format to numeric format.
                for (int iRowIndex = 0; iRowIndex < oDTSalaryDetails.Rows.Count; iRowIndex++)
                {
                    oDataRow = oDTSalaryDetails.Rows[iRowIndex];
                    for (int iColumnIndex = 0; iColumnIndex < oDTSalaryDetails.Columns.Count; iColumnIndex++)
                    {
                        sValue = oDataRow[iColumnIndex].ToString();

                        SalaryDifferenceClass oSalaryDifferenceClass;

                        iIndex = sValue.IndexOf("_");
                        if (iIndex >= 0)
                        {
                            oDataRow[iColumnIndex] = sValue.Substring(0, iIndex);

                            if (iRowIndex == 0 && iTotalColumnIndex < iColumnIndex)
                            {
                                sValue = sValue.Substring(iIndex + 1);
                                string sType = sValue.Substring(0, 2);
                                string sId = Constants.S_ZERO;
                                if (sValue.Length > 2)
                                {
                                    sValue = sValue.Substring(3);
                                    sId = sValue.Contains("_") ? sValue.Substring(0, sValue.IndexOf("_")) : Constants.S_ZERO;
                                }
                                oSalaryDifferenceClass = new SalaryDifferenceClass { ColumnName = oDTSalaryDetails.Columns[iColumnIndex].ColumnName.Replace("_", " "), Id = Convert.ToInt32(sId), Type = sType };
                                mlstSalaryDifferenceClassList.Add(oSalaryDifferenceClass);
                            }
                        }
                    }
                    oDTSalary.ImportRow(oDataRow);
                }

                foreach (DataColumn column in oDTSalary.Columns)
                    column.ColumnName = column.ColumnName.Replace("_", " ");

                oDTSalary = oDTSalary.AsEnumerable().OrderBy(user => Convert.ToInt32(user.Field<string>("Sr No"))).CopyToDataTable();

                PopulateSalaryDifferenceClass();
            }
            return oDTSalary;
        }

        /// <summary>
        /// This method is used to populate salary difference class.
        /// </summary>
        private void PopulateSalaryDifferenceClass()
        {
            PopulateSalaryDifferenceClass(PayrollConstants.S_GROSS_SALARY, PayrollConstants.GS, 0);
            PopulateSalaryDifferenceClass(PayrollConstants.S_TOTAL_DEDUCTION, PayrollConstants.TD, 0);
            PopulateSalaryDifferenceClass(PayrollConstants.S_NET_SALARY, PayrollConstants.NS, 0);
        }

        /// <summary>
        /// This method is used to set total pages.
        /// </summary>
        /// <param name="aiTotalRows"></param>
        private void SetTotalPages(int aiTotalRows)
        {
            if (aiTotalRows == Constants.I_GRID_PAGE_COUNT)
                miTotalPages = 1;
            else if (aiTotalRows % I_GRID_PAGE_COUNT == 0)
                miTotalPages = aiTotalRows / I_GRID_PAGE_COUNT;
            else
                miTotalPages = (aiTotalRows / I_GRID_PAGE_COUNT) + 1;
        }

        #endregion

        #region Supporting Information

        /// <summary>
        /// This method is used to add paid salary difference into dictionary to display it on salary details / pay salary screen.
        /// </summary>
        private void SetPaidSalaryDetails()
        {
            var oPaidSalary = moSalaryDifferenceDC.PaidSalaryDifferences.Select(salDiff => new { UserID = salDiff.UserId, Amount = Convert.ToInt32(Math.Round(salDiff.Amount)) }).ToList();
            if (oPaidSalary.Count() > 0)
                oPaidSalary.ForEach(user => mdictPaidSalary.Add(user.UserID, user.Amount));
        }

        /// <summary>
        /// This method is used to set paid salary difference month.
        /// </summary>
        private void SetPaidSalaryMonthList()
        {
            var oSalaryDifference = SalaryDifferences.Where(salDifference => salDifference.MonthId != 0 && salDifference.Year != 0).Select(salDifference => new { Month = salDifference.MonthId, Year = salDifference.Year });
            if (oSalaryDifference.Count() > 0)
            {
                StringBuilder oStringBuilder = new StringBuilder();
                var oUniqueSalaryDiff = oSalaryDifference.Distinct();

                foreach (var difference in oUniqueSalaryDiff)
                    oStringBuilder.Append(String.Format(", {0}", (String.Format("{0:MMMM}", Convert.ToDateTime("2010-" + difference.Month + "-02")) + " - " + difference.Year)));

                if (oStringBuilder.Length > 1)
                    msMonthList = oStringBuilder.ToString().Substring(1);
            }
        }

        /// <summary>
        /// This method is used to get salary details xml.
        /// </summary>
        /// <returns></returns>
        private DataTable GetSalaryDetailsXml()
        {
            DataTable oDataTable = new DataTable();
            oDataTable.Columns.Add("SalaryDetailsXml");

            int iRowIndex = 0;
            moSalaryDifferenceDC.StaticSalaryDetails.Where(user => mlstUserIds.Contains(user.UserId)).ToList().ForEach(xml => { oDataTable.Rows.Add(); oDataTable.Rows[iRowIndex++]["SalaryDetailsXml"] = xml.SalaryDetailsXml; });
            return oDataTable;
        }

        /// <summary>
        /// This method is used to return paid salary difference.
        /// </summary>
        /// <returns></returns>
        private DataTable GetPaidSalaryDetails()
        {
            DataTable oDTPaidSalary = new DataTable();
            oDTPaidSalary.AddColumns(new string[] { "IsSalaryPaid", "MonthList", "CurrentSalaryMonth" });

            oDTPaidSalary.Rows.Add();
            oDTPaidSalary.Rows[0]["IsSalaryPaid"] = Constants.S_YES;
            oDTPaidSalary.Rows[0]["MonthList"] = msMonthList;
            oDTPaidSalary.Rows[0]["CurrentSalaryMonth"] = moSalaryDifferenceDC.CurrentSalaryMonth;

            return oDTPaidSalary;
        }

        #endregion

        #region Table Design

        /// <summary>
        /// This method is used to add earning-deduction columns.
        /// </summary>
        private void AddEarningDeductionsColumns()
        {
            List<string> lstEarnings = moEarningsDeductionsBL.AddEarningDeductionColumns(true, null);
            mlstTotalEarningsDeductions.AddRange(lstEarnings);
            moDTSalaryDetails.Columns.Add(PayrollConstants.S_GROSS_SALARY);
            mlstTotalEarningsDeductions.Add(PayrollConstants.S_GROSS_SALARY);

            List<string> lstDeductions = moEarningsDeductionsBL.AddEarningDeductionColumns(false, null);
            mlstTotalEarningsDeductions.AddRange(lstDeductions);
            moDTSalaryDetails.AddColumns(new string[] { PayrollConstants.S_TOTAL_DEDUCTION, PayrollConstants.S_NET_SALARY });
            mlstTotalEarningsDeductions.AddRange(new string[] { PayrollConstants.S_TOTAL_DEDUCTION, PayrollConstants.S_NET_SALARY });
        }

        /// <summary>
        /// This method is used to add basic columns.
        /// </summary>
        private void AddBasicColumns()
        {
            moDTSalaryDetails.AddColumns(
                                            new string[] 
                                        { 
                                            "Sr No", "DisplayControls", "SortOrder", 
                                            "TotalSortOrder", "UserId", "OriginalStaffGroupsId",
                                            "StaffGroupId", "Name", "Designation", "Attendance"
                                        }
                                       );
        }

        #endregion

        #region Calculate Salary Difference

        /// <summary>
        /// This method is used to return paid salary table.
        /// </summary>
        /// <param name="aoDataSet"></param>
        /// <returns></returns>
        public DataTable GetPaidSalaryTable(DataSet aoDataSet)
        {
            // Get xml and convert it into table.
            DataTable oDTPaidSalaryXml = aoDataSet.Tables[I_PAID_SALARY_DETAILS_TABLE_INDEX];

            if (oDTPaidSalaryXml == null || oDTPaidSalaryXml.Rows.Count == 0)
                throw new NoRecordFoundException(S_NO_RECORD_FOUND_MESSAGE);

            string sXml = string.Empty;
            string sTotalXml = string.Empty;

            foreach (DataRow oDataRow in oDTPaidSalaryXml.Rows)
            {
                sXml = oDataRow[0].ToString();
                sXml = sXml.Replace("<SalaryDetails>", string.Empty);
                sXml = sXml.Replace("</SalaryDetails>", string.Empty);
                sTotalXml += sXml;
            }

            sTotalXml = "<SalaryDetailsXml>" + sTotalXml.Replace("<SalaryDetails ", "<SalaryDetailsXml ") + "</SalaryDetailsXml>";

            DataSet oDSPaidSalary = new DataSet();
            System.IO.StringReader oReader = new System.IO.StringReader(sTotalXml);
            oDSPaidSalary.ReadXml(oReader);
            oDTPaidSalaryXml = oDSPaidSalary.Tables[0];

            return oDTPaidSalaryXml;
        }

        /// <summary>
        /// This method is used to calculate net salary difference.
        /// </summary>
        /// <param name="aoDTSalaryDifference"></param>
        public void CalculateNetSalaryDifference(DataTable aoDTSalaryDifference)
        {
            int iUserId;
            decimal iEarnings;
            decimal iDeductions;

            // Iterate each row of datatable.
            for (int iRowIndex = 0; iRowIndex < aoDTSalaryDifference.Rows.Count; iRowIndex++)
            {
                iEarnings = 0;
                iDeductions = 0;
                if (Convert.ToInt32(aoDTSalaryDifference.Rows[iRowIndex][S_SALARY_DIFFERENCE_ROW_COLUMN]) == 1)
                {
                    iUserId = Convert.ToInt32(aoDTSalaryDifference.Rows[iRowIndex][S_USER_ID]);
                    var oSalDiffDetails = moSalaryDifferenceDC.CurrentMonthsPaidSalaryDifferences
                                                .Where(salDiff => salDiff.UserId == iUserId)
                                                .FirstOrDefault();


                    if (oSalDiffDetails == null)
                        oSalDiffDetails = new SalaryDifference { UserId = iUserId, Amount = 0, AmountToBePaid = 0 };


                    // Update gross salary and total deduction of salary difference row.
                    var oUsersEarningsDeduction = moUsersEarningsDeductionsBL.UsersSalDifferenceDetails.Where(user => user.UserId == iUserId).ToList();
                    moEarningsDeductionsBL.EarningsDeductions.ForEach
                        (
                            ED =>
                            {
                                string sShortName = ED.ShortName;
                                if (ED.IsAttendanceDependent)
                                    sShortName = S_LEAVE_DEDUCTED + sShortName;
                                if (aoDTSalaryDifference.Columns.Contains(sShortName) && aoDTSalaryDifference.Rows[iRowIndex][sShortName] != DBNull.Value
                                    && aoDTSalaryDifference.Rows[iRowIndex][sShortName].ToString().Trim() != string.Empty && Convert.ToInt32(aoDTSalaryDifference.Rows[iRowIndex][sShortName]) != 0)
                                {
                                    decimal iValue = Convert.ToDecimal(aoDTSalaryDifference.Rows[iRowIndex][sShortName]);
                                    if (ED.IsEarning)
                                        iEarnings = iEarnings + iValue;
                                    else
                                        iDeductions = iDeductions + iValue;
                                }
                            }
                        );

                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_GROSS_SALARY] = iEarnings;
                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_TOTAL_DEDUCTION] = iDeductions;
                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_NET_SALARY] = iEarnings - iDeductions;
                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_SAVED_DIFFERENCE] = Math.Round(oSalDiffDetails.Amount, 0);
                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_PAID_DIFFERENCE] = Math.Round(oSalDiffDetails.AmountToBePaid, 0);
                    aoDTSalaryDifference.Rows[iRowIndex][PayrollConstants.S_NET_DIFFERENCE] = iEarnings - iDeductions;
                }
            }
        }

        /// <summary>
        /// This method is used to update tables for salary difference.
        /// </summary>
        public void UpdateTablesForSalaryDifference(DataTable aoDTCurrentSalary)
        {
            if (aoDTCurrentSalary.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE) || aoDTCurrentSalary.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
            {
                string sSalaryDifference = aoDTCurrentSalary.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)) ? PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE) : PayrollConstants.S_SALARY_DIFFERENCE;
                var oRows = aoDTCurrentSalary.AsEnumerable().Where(user => Convert.ToInt32(user.Field<string>(sSalaryDifference)) != 0);

                // Update gross salary and net salary if there exists salary difference column.
                foreach (DataRow row in oRows)
                {
                    row[PayrollConstants.S_GROSS_SALARY] = Convert.ToInt32(row[PayrollConstants.S_GROSS_SALARY]) - Convert.ToInt32(row[sSalaryDifference]);
                    row[PayrollConstants.S_NET_SALARY] = Convert.ToInt32(row[PayrollConstants.S_NET_SALARY]) - Convert.ToInt32(row[sSalaryDifference]);
                    row[PayrollConstants.S_SALARY_DIFFERENCE] = 0;
                }
                aoDTCurrentSalary.Columns.Remove(sSalaryDifference);
            }
        }

        /// <summary>
        /// This method is used to calculate salary difference of selected month's configuration as per current configuration.
        /// </summary>
        /// <param name="aoDTSalaryDetails"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiBaseYear"></param>
        /// <param name="aiBaseMonthId"></param>
        /// <returns></returns>
        public DataTable CalculateSalaryDifference(DataTable aoDTSalaryDetails, int aiYear, int aiMonthId, int aiBaseYear, int aiBaseMonthId)
        {
            string sFifthPay;
            decimal dcFifthPay;
            decimal dcSixthPay;
            string sSixthPay;
            decimal dcFifthPayTotalDays;
            decimal dcSixthPayTotalDays;

            int iSalaryRecodCount = aoDTSalaryDetails.Rows.Count;
            int iRowCount = aoDTSalaryDetails.Rows.Count;
            int iColumnCount = aoDTSalaryDetails.Columns.Count - 1;
            int iTotalDays = DateTime.DaysInMonth(aiYear, aiMonthId);
            int iBaseTotalDays = DateTime.DaysInMonth(aiBaseYear, aiBaseMonthId);
            int iTotalColumnIndex = aoDTSalaryDetails.Columns.IndexOf(PayrollConstants.S_TOTAL);

            // Iterate each row.
            for (int iRowIndex = 1; iRowIndex < iRowCount; iRowIndex = iRowIndex + 2)
            {
                aoDTSalaryDetails.Rows.Add();
                dcFifthPayTotalDays = Convert.ToDecimal(aoDTSalaryDetails.Rows[iRowIndex - 1][PayrollConstants.S_TOTAL]);
                dcSixthPayTotalDays = Convert.ToDecimal(aoDTSalaryDetails.Rows[iRowIndex][PayrollConstants.S_TOTAL]);

                // Iterate each column.
                for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
                {
                    aoDTSalaryDetails.Rows[iRowIndex]["OriginalStaffGroupsId"] = aoDTSalaryDetails.Rows[iRowIndex - 1]["OriginalStaffGroupsId"];

                    // If earning deduction columns.
                    if (iColumnIndex > iTotalColumnIndex && aoDTSalaryDetails.Columns[iColumnIndex].ColumnName != PayrollConstants.S_LATE_MARK_LEAVES)
                    {
                        sFifthPay = aoDTSalaryDetails.Rows[iRowIndex - 1][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName].ToString();
                        sSixthPay = aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName].ToString();

                        dcFifthPay = string.IsNullOrEmpty(sFifthPay) ? 0 : Convert.ToDecimal(sFifthPay);
                        dcSixthPay = string.IsNullOrEmpty(sSixthPay) ? 0 : Convert.ToDecimal(sSixthPay);

                        if (dcFifthPay == (decimal)-1)
                        {
                            dcFifthPay = 0;
                            aoDTSalaryDetails.Rows[iRowIndex - 1][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName] = string.Empty;
                        }

                        if (dcSixthPay == (decimal)-1)
                        {
                            dcSixthPay = 0;
                            aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName] = string.Empty;
                        }

                        // if earning deduction is attendance dependent.
                        if (AttendanceDependentColumns.Contains(aoDTSalaryDetails.Columns[iColumnIndex].ColumnName))
                        {
                            if (!(aoDTSalaryDetails.Columns[iColumnIndex + 1] != null && aoDTSalaryDetails.Columns[iColumnIndex + 1].ColumnName.Contains(S_LEAVE_DEDUCTED.Trim())))
                            {
                                if (dcSixthPayTotalDays == 0)
                                    dcSixthPay = 0;
                                else
                                {
                                    if (aoDTSalaryDetails.Columns[iColumnIndex].ColumnName.Contains(S_LEAVE_DEDUCTED.Trim()))
                                    {
                                        if (aoDTSalaryDetails.Rows[iRowIndex][iColumnIndex - 1] == DBNull.Value || aoDTSalaryDetails.Rows[iRowIndex][iColumnIndex - 1].ToString() == string.Empty || aoDTSalaryDetails.Rows[iRowIndex][iColumnIndex - 1].ToString() == "-1")
                                            dcSixthPay = 0;                                        
                                    }
                                    aoDTSalaryDetails.Rows[iRowIndex][PayrollConstants.S_TOTAL] = dcFifthPayTotalDays;
                                }

                                if (aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName].ToString() != string.Empty)
                                    aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName] = dcSixthPay;
                            }
                        }

                        if (aoDTSalaryDetails.Rows[iRowIndex - 1][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName].ToString() == string.Empty && aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName].ToString() == string.Empty)
                            aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = string.Empty;
                        else
                        {   
                            string sColumnName = aoDTSalaryDetails.Columns[iColumnIndex].ColumnName;
                            EarningsDeductions oEarningsDeductions = moEarningsDeductionsBL.EarningsDeductions.Where(ED => ED.ShortName == sColumnName || ED.ShortName == sColumnName.Replace(S_LEAVE_DEDUCTED, string.Empty).Trim()).FirstOrDefault();
                            
                            // Check if any earning deduction is associated with user.
                            if (oEarningsDeductions != null)
                            {
                                var oUsersED = moUsersEarningsDeductionsBL.UsersSalDifferenceDetails.Where(user => user.UserId == Convert.ToInt32(aoDTSalaryDetails.Rows[iRowIndex]["UserId"]) && user.EarningsDeductionsId == oEarningsDeductions.EarningsDeductionsId);
                                decimal dcAmount = 0;
                                if (oUsersED.Count() > 0)
                                {
                                    if (oUsersED.Count() > 1)
                                    {
                                        if (sColumnName.Contains(S_LEAVE_DEDUCTED))
                                            dcAmount = oUsersED.Where(ed => ed.Type == PayrollConstants.LD).First().EarningsDeductionsValue;
                                        else
                                            dcAmount = oUsersED.Where(ed => ed.Type == PayrollConstants.ED).First().EarningsDeductionsValue;
                                    }
                                    else
                                        dcAmount = oUsersED.First().EarningsDeductionsValue;
                                }

                                // calculate salary difference by substracting fifth pay amount from sixth pay amount.
                                if (oUsersED != null)
                                    aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = (dcSixthPay - dcFifthPay - dcAmount).ToString();
                                else
                                    aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = (dcSixthPay - dcFifthPay).ToString();
                            }
                            else
                                aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = (dcSixthPay - dcFifthPay).ToString();
                        }
                    }
                    else
                    {
                        if (aoDTSalaryDetails.Columns[iColumnIndex].ColumnName == PayrollConstants.S_LATE_MARK_LEAVES)
                        {
                            aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = 0;
                            aoDTSalaryDetails.Rows[iRowIndex][iColumnIndex] = aoDTSalaryDetails.Rows[iRowIndex - 1][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName];
                        }
                        else if (iColumnIndex == iTotalColumnIndex)
                            aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = 0;
                        else
                            aoDTSalaryDetails.Rows[iSalaryRecodCount][iColumnIndex] = aoDTSalaryDetails.Rows[iRowIndex][aoDTSalaryDetails.Columns[iColumnIndex].ColumnName];
                    }
                }

                aoDTSalaryDetails.Rows[iSalaryRecodCount][S_SALARY_DIFFERENCE_ROW_COLUMN] = 1;
                aoDTSalaryDetails.Rows[iRowIndex][S_SALARY_DIFFERENCE_ROW_COLUMN] = 0;
                aoDTSalaryDetails.Rows[iRowIndex - 1][S_SALARY_DIFFERENCE_ROW_COLUMN] = 0;
                iSalaryRecodCount++;
            }

            // Return salary difference details order by serial no.
            IEnumerable<DataRow> SalaryDetails = from SalDetails in aoDTSalaryDetails.AsEnumerable()
                                                 where Convert.ToInt32(SalDetails[S_USER_ID]) != -9999
                                                 orderby Convert.ToInt32(SalDetails["Sr No"]) ascending
                                                 select SalDetails;

            //DataTable oDTSalaryDetailsTable = SalaryDetails.CopyToDataTable();

            DataTable oDTSalaryDetailsTable = aoDTSalaryDetails.Clone();
            if (SalaryDetails.Count() > 0)
                oDTSalaryDetailsTable = SalaryDetails.CopyToDataTable();

            return oDTSalaryDetailsTable;
        }

        /// <summary>
        /// This method is used to return current salary table.
        /// </summary>
        /// <param name="aoDataSet"></param>
        public DataTable GetCurrentSalaryDetailsTable(DataSet aoDataSet)
        {
            // Remove un-necessary columns.
            DataTable oDTCurrentSalary = aoDataSet.Tables[I_BASE_CONFIGURATION_TABLE_INDEX];

            if (oDTCurrentSalary == null || oDTCurrentSalary.Rows.Count == 0)
                throw new NoRecordFoundException(S_NO_RECORD_FOUND_MESSAGE);

            oDTCurrentSalary.Columns.RemoveAt(3);
            oDTCurrentSalary.Columns.RemoveAt(2);
            oDTCurrentSalary.Columns.RemoveAt(1);
            UpdateTablesForSalaryDifference(oDTCurrentSalary);

            if (mlstUserIds.Count > 0)
            {
                DataRow[] oDatarows = oDTCurrentSalary.Select("UserId IN (" + string.Join(",", mlstUserIds) + ")");

                if (oDatarows.Length > 0)
                    oDTCurrentSalary = oDatarows.CopyToDataTable();
            }

            UpdateTableToRemoveExtraColumns(oDTCurrentSalary);
            return oDTCurrentSalary;
        }

        /// <summary>
        /// This method is used to remove extra columns.
        /// </summary>
        /// <param name="aoDTCurrentSalary"></param>
        private void UpdateTableToRemoveExtraColumns(DataTable aoDTCurrentSalary)
        {
            List<EarningsDeductions> olstEarninDeductions = moEarningsDeductionsBL.EarningsDeductions.Where(earningDeduct => !earningDeduct.IncludeInSalaryDifference).ToList();

            // Iterate each earning deduction.
            olstEarninDeductions.ForEach
                (
                    earnDeduct =>
                    {
                        string sColumnName = earnDeduct.ShortName;

                        // If earning/deduction is attendance dependent then rename it.
                        if (!aoDTCurrentSalary.Columns.Contains(sColumnName))
                            sColumnName = sColumnName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE);

                        if (aoDTCurrentSalary.Columns.Contains(sColumnName))
                        {
                            // Remove leave deducted and baseearning deduction from table.
                            if (aoDTCurrentSalary.Columns.Contains(S_LEAVE_DEDUCTED + sColumnName))
                            {
                                aoDTCurrentSalary.Columns.Remove(sColumnName);
                                sColumnName = S_LEAVE_DEDUCTED + sColumnName;
                            }

                            if (!earnDeduct.IncludeInSalaryDifference)
                                aoDTCurrentSalary.Columns.Remove(sColumnName);
                        }
                    }
                );

            // Update summery details.
            List<int> oEarnings = new List<int>();
            List<int> oDeductions = new List<int>();
            olstEarninDeductions = moEarningsDeductionsBL.EarningsDeductions.Where(earningDeduct => earningDeduct.IncludeInSalaryDifference).ToList();
            for (int iRowIndex = 0; iRowIndex < aoDTCurrentSalary.Rows.Count; iRowIndex++)
            {
                olstEarninDeductions.ForEach
               (
                   earnDeduct =>
                   {
                       string sColumnName = earnDeduct.ShortName;

                       if (!aoDTCurrentSalary.Columns.Contains(sColumnName))
                           sColumnName = sColumnName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE);

                       if (aoDTCurrentSalary.Rows[iRowIndex][sColumnName] == DBNull.Value || aoDTCurrentSalary.Rows[iRowIndex][sColumnName].ToString() == string.Empty)
                           aoDTCurrentSalary.Rows[iRowIndex][sColumnName] = -1;

                       int iValue = Convert.ToInt32(aoDTCurrentSalary.Rows[iRowIndex][sColumnName]);
                       if (iValue == -1)
                           iValue = 0;

                       // calculate earning and deductions after removing some earningd and deductions from table.
                       if (earnDeduct.IsEarning)
                           oEarnings.Add(iValue);
                       else
                           oDeductions.Add(iValue);
                   });

                aoDTCurrentSalary.Rows[iRowIndex][PayrollConstants.S_GROSS_SALARY] = oEarnings.Sum();
                aoDTCurrentSalary.Rows[iRowIndex][PayrollConstants.S_TOTAL_DEDUCTION] = oDeductions.Sum();
                aoDTCurrentSalary.Rows[iRowIndex][PayrollConstants.S_NET_SALARY] = oEarnings.Sum() - oDeductions.Sum();
                oEarnings.Clear();
                oDeductions.Clear();
            }

        }

        /// <summary>
        /// This method is used to update paid salary table for current users.
        /// </summary>
        /// <param name="aoDTTempTable"></param>
        public void UpdatePaidSalaryTableForCurrentUsers(DataTable aoDTTempTable, DataTable aoDTCurrentSalary, DataTable aoDTPaidSalary)
        {
            List<int> lstUserIds = new List<int>();

            // collect user ids of all the users.
            foreach (DataRow oDataRow in aoDTCurrentSalary.Rows)
            {
                if (!string.IsNullOrEmpty(oDataRow[S_USER_ID].ToString()))
                    lstUserIds.Add(Convert.ToInt32(oDataRow[S_USER_ID]));
            }

            // select salary difference details for selected users.
            for (int iRowIndex = 0; iRowIndex < aoDTPaidSalary.Rows.Count; iRowIndex++)
            {
                DataRow oDataRow = aoDTPaidSalary.Rows[iRowIndex];
                if (!string.IsNullOrEmpty(oDataRow[S_USER_ID].ToString()))
                {
                    int iUserId = Convert.ToInt32(oDataRow[S_USER_ID]);
                    if (lstUserIds.Contains(iUserId))
                        aoDTTempTable.ImportRow(oDataRow);
                }
            }
        }

        /// <summary>
        /// This method is used to merge current configuration details into existing salary details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetMergedTable(DataTable aoDTPaidSalary, DataTable aoDTCurrentSalary)
        {
            int iUserId;
            int iSalaryRecodCount = aoDTPaidSalary.Rows.Count;
            int iRowCount = aoDTCurrentSalary.Rows.Count;
            int iColumnCount = aoDTCurrentSalary.Columns.Count;

            DataRow[] oDRPaid;
            DataRow[] oDataRows;

            // merge both the tables.
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                iUserId = Convert.ToInt32(aoDTCurrentSalary.Rows[iRowIndex][S_USER_ID]);
                oDRPaid = aoDTPaidSalary.Select("UserId='" + iUserId + "'");
                oDataRows = aoDTCurrentSalary.Select("UserId='" + iUserId + "'");
                if (oDataRows.Length > 0 && oDRPaid.Length > 0)
                {
                    aoDTPaidSalary.Rows.Add();
                    aoDTPaidSalary.Rows[iSalaryRecodCount]["Sr No"] = oDataRows[0]["Sr No"].ToString();
                    oDRPaid[0]["Sr No"] = oDataRows[0]["Sr No"].ToString();
                    aoDTPaidSalary.AcceptChanges();

                    for (int iColumnIndex = 1; iColumnIndex < iColumnCount; iColumnIndex++)
                        aoDTPaidSalary.Rows[iSalaryRecodCount][aoDTPaidSalary.Columns[iColumnIndex].ColumnName] = aoDTCurrentSalary.Rows[iRowIndex][aoDTPaidSalary.Columns[iColumnIndex].ColumnName];

                    iSalaryRecodCount++;
                }
            }

            IEnumerable<DataRow> SortedSalaryDetails = from SalDetails in aoDTPaidSalary.AsEnumerable()
                                                       where Convert.ToInt32(SalDetails[S_USER_ID]) != -9999
                                                       orderby Convert.ToInt32(SalDetails["OriginalStaffGroupsId"]) ascending, Convert.ToInt32(SalDetails["Sr No"]) ascending
                                                       select SalDetails;

            //DataTable oDTSalaryDetails = SortedSalaryDetails.CopyToDataTable();
            DataTable oDTSalaryDetails = aoDTPaidSalary.Clone();
            if (SortedSalaryDetails.Count() > 0)
                oDTSalaryDetails = SortedSalaryDetails.CopyToDataTable();

            oDTSalaryDetails.Columns.Add(S_SALARY_DIFFERENCE_ROW_COLUMN);

            // If there exists salary difference column then reomve it.
            if (oDTSalaryDetails.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                oDTSalaryDetails.Columns.Remove(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));

            RemoveNonSalaryDependentColumns(oDTSalaryDetails);

            return oDTSalaryDetails;
        }

        /// <summary>
        /// This method is used to remove non salary dependent columns.
        /// </summary>
        /// <param name="oDTSalaryDetails"></param>
        private void RemoveNonSalaryDependentColumns(DataTable aoDTSalaryDetails)
        {
            List<EarningsDeductions> lstEarningsDeductions = moEarningsDeductionsBL.EarningsDeductions.Where(earnDeduct => !earnDeduct.IncludeInSalaryDifference).ToList();

            // iterate each configured earnng deduction and remove all such earning deduction columns are not included in salary difference.
            lstEarningsDeductions.ForEach(earnDeduct =>
                {
                    if (aoDTSalaryDetails.Columns.Contains(earnDeduct.ShortName))
                        aoDTSalaryDetails.Columns.Remove(earnDeduct.ShortName);
                    else if (aoDTSalaryDetails.Columns.Contains(earnDeduct.ShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                        aoDTSalaryDetails.Columns.Remove(earnDeduct.ShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));

                    if (aoDTSalaryDetails.Columns.Contains(S_LEAVE_DEDUCTED + earnDeduct.ShortName))
                        aoDTSalaryDetails.Columns.Remove(S_LEAVE_DEDUCTED + earnDeduct.ShortName);
                    else if (aoDTSalaryDetails.Columns.Contains(S_LEAVE_DEDUCTED + earnDeduct.ShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                        aoDTSalaryDetails.Columns.Remove(S_LEAVE_DEDUCTED + earnDeduct.ShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));
                });
        }

        /// <summary>
        /// This method is used to calculate summary details.
        /// </summary>
        /// <param name="aoDTSalaryDifference"></param>
        public void CalculateSummaryOfDifference(DataTable aoDTSalaryDifference, List<string> alstColumnsNames)
        {
            alstColumnsNames.AddRange(new string[] { PayrollConstants.S_SAVED_DIFFERENCE, PayrollConstants.S_NET_DIFFERENCE, PayrollConstants.S_PAID_DIFFERENCE });
            if (alstColumnsNames.Contains(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                alstColumnsNames.Remove(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));

            RemoveExcludedColumns(alstColumnsNames);

            // Select only salary difference rows from table.
            var columnSum = from column in aoDTSalaryDifference.AsEnumerable()
                            where column.Field<string>(S_SALARY_DIFFERENCE_ROW_COLUMN) == Constants.S_ONE
                            select column;

            aoDTSalaryDifference.Rows.Add();
            int iLastRowIndex = aoDTSalaryDifference.Rows.Count - 1;
            aoDTSalaryDifference.Rows[iLastRowIndex][S_USER_ID] = 0;

            // Calculate total of all the salary difference details.
            foreach (string sColumnName in alstColumnsNames)
            {
                var UnpaidLeaves = columnSum
                                    .Where(salaryDiff => salaryDiff[sColumnName] != null && salaryDiff[sColumnName].ToString() != string.Empty)
                                   .GroupBy(salaryDiff => salaryDiff.Field<string>(S_SALARY_DIFFERENCE_ROW_COLUMN))
                                   .Select(sumDays => new
                                   {
                                       sumDays.Key,
                                       TotalDays = Convert.ToDecimal(sumDays.Sum(p => Convert.ToDecimal(p[sColumnName])))
                                   });
                aoDTSalaryDifference.Rows[iLastRowIndex][sColumnName] = UnpaidLeaves.Count() > 0 ? UnpaidLeaves.First().TotalDays.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// This method is used to remove excluded columns from column list.
        /// </summary>
        /// <param name="alstColumnsNames"></param>
        private void RemoveExcludedColumns(List<string> alstColumnsNames)
        {
            List<string> sColumns = moEarningsDeductionsBL.EarningsDeductions.Where(earnDeduct => !earnDeduct.IncludeInSalaryDifference).Select(earnDeduct => earnDeduct.ShortName).ToList();

            // Iterate for each ED and remove ecluded ED columns.
            sColumns.ForEach(column =>
            {
                if (alstColumnsNames.Contains(column))
                    alstColumnsNames.Remove(column);
                else if (alstColumnsNames.Contains(column.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                    alstColumnsNames.Remove(column.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));

                if (alstColumnsNames.Contains(S_LEAVE_DEDUCTED + column))
                    alstColumnsNames.Remove(S_LEAVE_DEDUCTED + column);
                else if (alstColumnsNames.Contains(S_LEAVE_DEDUCTED + column.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                    alstColumnsNames.Remove(S_LEAVE_DEDUCTED + column.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));
            });
        }

        /// <summary>
        /// Thisd method is used to remove supporting columns.
        /// </summary>
        /// <param name="aoDTSalaryDifference"></param>
        public void RemoveSupportingColumns(DataTable aoDTSalaryDifference)
        {
            int iTotalCoumnIndex = aoDTSalaryDifference.Columns.IndexOf(PayrollConstants.S_TOTAL);
            int iAttendanceColumnIndex = aoDTSalaryDifference.Columns.IndexOf(S_ATTENDANCE);

            for (int iColumnIndex = iTotalCoumnIndex - 1; iColumnIndex >= iAttendanceColumnIndex; iColumnIndex--)
                aoDTSalaryDifference.Columns.Remove(aoDTSalaryDifference.Columns[iColumnIndex]);
        }

        /// <summary>
        /// This method is used to generate salary difference xml.
        /// </summary>
        /// <returns></returns>
        private string GenerateXml(int aiUserid, DataTable aoDTSalaryDifference)
        {
            const string S_ELEMENT = "SalaryDifference";
            string sXml = string.Empty;

            if (aoDTSalaryDifference.IsNonEmpty())
            {
                var oSalaryCollection = from SalaryDiff in aoDTSalaryDifference.AsEnumerable()
                                        where SalaryDiff.Field<string>(S_SALARY_DIFFERENCE_ROW_COLUMN) == Constants.S_ONE
                                        select new
                                        {
                                            NetSalary = SalaryDiff.Field<string>(PayrollConstants.S_NET_SALARY),
                                            UserId = SalaryDiff.Field<string>(S_USER_ID)
                                        };
                if (aiUserid != 0)
                    oSalaryCollection = oSalaryCollection.Where(user => user.UserId == aiUserid.ToString());

                XmlDocument Doc = new XmlDocument();
                XmlElement root = Doc.CreateElement(S_ELEMENT);
                XmlNode oXmlRootNode = Doc.CreateNode("element", S_ELEMENT, string.Empty);
                int iAmount = 0;

                foreach (var salaryDifference in oSalaryCollection)
                {
                    if (salaryDifference.NetSalary != Constants.S_ZERO)
                    {
                        XmlNode oXmlNode = Doc.CreateNode("element", S_ELEMENT, string.Empty);

                        XmlAttribute attr = Doc.CreateAttribute(S_USER_ID);
                        attr.Value = salaryDifference.UserId;
                        oXmlNode.Attributes.Append(attr);

                        iAmount = Convert.ToInt32(salaryDifference.NetSalary);
                        attr = Doc.CreateAttribute("NetSalary");
                        attr.Value = iAmount.ToString();
                        oXmlNode.Attributes.Append(attr);

                        attr = Doc.CreateAttribute("SalaryDifferenceXml");
                        attr.Value = GenerateSalaryDifferenceXML(salaryDifference.UserId, aoDTSalaryDifference);
                        oXmlNode.Attributes.Append(attr);

                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                }
                root.AppendChild(oXmlRootNode);
                sXml = root.InnerXml;
            }
            return sXml;
        }

        /// <summary>
        /// This method is used to generate salary difference xml.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        private string GenerateSalaryDifferenceXML(string aiUserId, DataTable aoDTSalaryDifference)
        {
            string sXml = string.Empty;
            if (aoDTSalaryDifference.IsNonEmpty())
            {
                DataRow oDataRow = (from salDiff in aoDTSalaryDifference.AsEnumerable()
                                    where salDiff.Field<string>(S_USER_ID) == aiUserId
                                    && salDiff.Field<string>(S_SALARY_DIFFERENCE_ROW_COLUMN) == Constants.S_ONE
                                    select salDiff).FirstOrDefault();
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("User");
                XmlNode rootNode = doc.CreateNode("element", "User", string.Empty);

                molstEarningsDeductions.ForEach
                    (
                        earnDeduct =>
                        {
                            if (mlstSalaryDifferenceClassList.Where(salDiff => salDiff.Id == earnDeduct.EarningsDeductionsId && earnDeduct.IncludeInSalaryDifference).Count() == 0)
                            {
                                mlstSalaryDifferenceClassList.RemoveAll(salDiff => salDiff.ColumnName == S_LEAVE_DEDUCTED + earnDeduct.ShortName);
                                mlstSalaryDifferenceClassList.RemoveAll(salDiff => salDiff.Id == earnDeduct.EarningsDeductionsId);
                            }
                        }
                    );

                if (oDataRow != null)
                {
                    foreach (SalaryDifferenceClass salDiff in mlstSalaryDifferenceClassList)
                    {
                        XmlNode node = doc.CreateNode("element", "UsersSalaryDifference", string.Empty);

                        XmlAttribute attr = doc.CreateAttribute("Id");
                        attr.Value = salDiff.Id.ToString();
                        node.Attributes.Append(attr);

                        attr = doc.CreateAttribute("Type");
                        attr.Value = salDiff.Type;
                        node.Attributes.Append(attr);

                        string sColumnName = salDiff.ColumnName;
                        if (salDiff.Type == PayrollConstants.LD && !sColumnName.Contains(S_LEAVE_DEDUCTED))
                            sColumnName = S_LEAVE_DEDUCTED + sColumnName;

                        attr = doc.CreateAttribute("Value");
                        attr.Value = oDataRow[sColumnName] == null ? Constants.S_ZERO : oDataRow[sColumnName].ToString();
                        node.Attributes.Append(attr);

                        rootNode.AppendChild(node);
                    }
                    root.AppendChild(rootNode);
                    sXml = root.InnerXml;
                }
            }
            return sXml;
        }

        /// <summary>
        /// This method is used to return salary details with respective to current cnfiguration as well as seleced months configuration.
        /// </summary>
        /// <param name="ODataSet"></param>
        /// <param name="oDTCurrentSalary"></param>
        /// <param name="oDTPaidSalaryXml"></param>
        public DataTable GetPaidSalaryDetails(DataSet aoDataSet, out List<string> aolstColumnsNames, out DataTable aoDTCurrentSalary)
        {
            string sValue = string.Empty;
            bool bIsEarning = true;

            Stack<int> stkfifthPayEarnings = new Stack<int>();
            Stack<int> stkfifthPayDeductions = new Stack<int>();

            aolstColumnsNames = new List<string>();
            DataTable oDTPaidSalary = new DataTable();
            DataTable oDTTempTable = new DataTable();

            aoDTCurrentSalary = GetCurrentSalaryDetailsTable(aoDataSet);
            DataTable oDTPaidSalaryXml = GetPaidSalaryTable(aoDataSet);
            DataTable oDTTempPaidSalary = GetPaidSalaryTable(aoDataSet);

            RemoveNonSalaryDependentColumns(oDTPaidSalaryXml);

            // Add salary difference column in current salary detail tables if it is present in paid salary difference table.
            if (oDTPaidSalaryXml.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)) && !aoDTCurrentSalary.Columns.Contains(PayrollConstants.S_SALARY_DIFFERENCE))
                aoDTCurrentSalary.Columns.Add(PayrollConstants.S_SALARY_DIFFERENCE.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE));

            AddColumnsInPaidSalaryTable(aolstColumnsNames, aoDTCurrentSalary, oDTPaidSalary, oDTTempTable);

            bool bExcludeFromCalculation = false;
            int iRowCount = oDTPaidSalaryXml.Rows.Count;

            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                oDTPaidSalary.Rows.Add();
                if (oDTPaidSalaryXml.Columns.Contains("Sr_No"))
                    oDTPaidSalary.Rows[iRowIndex][0] = oDTPaidSalaryXml.Rows[iRowIndex]["Sr_No"].ToString();
                else
                    oDTPaidSalary.Rows[iRowIndex][0] = oDTPaidSalaryXml.Rows[iRowIndex]["SrNo"].ToString();

                bIsEarning = true;
                bExcludeFromCalculation = false;
                int iColumnCount = aoDTCurrentSalary.Columns.Count;
                for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
                {
                    sValue = string.Empty;
                    if (oDTPaidSalaryXml.Columns.Contains(aoDTCurrentSalary.Columns[iColumnIndex].ColumnName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)) && oDTPaidSalaryXml.Rows[iRowIndex][aoDTCurrentSalary.Columns[iColumnIndex].ColumnName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)] != DBNull.Value)
                        sValue = oDTPaidSalaryXml.Rows[iRowIndex][aoDTCurrentSalary.Columns[iColumnIndex].ColumnName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)].ToString();

                    if (sValue.Contains(S_EARNING_DEDUCTION_SEPARATOR) || sValue.Contains(S_LEAVE_DEDUCTED_EARNING_DEDUCTION_SEPARATOR))
                        UpdateEarningDeductionStack(aoDTCurrentSalary, ref sValue, bIsEarning, stkfifthPayEarnings, stkfifthPayDeductions, ref bExcludeFromCalculation, aoDTCurrentSalary.Columns[iColumnIndex].ColumnName.Contains(S_LEAVE_DEDUCTED.Trim()));

                    else if (aoDTCurrentSalary.Columns[iColumnIndex].ColumnName == PayrollConstants.S_GROSS_SALARY)
                    {
                        // If gross salary ccolumn.
                        sValue = stkfifthPayEarnings.Sum().ToString();
                        bIsEarning = false;
                    }
                    else if (aoDTCurrentSalary.Columns[iColumnIndex].ColumnName == PayrollConstants.S_TOTAL_DEDUCTION)
                        sValue = stkfifthPayDeductions.Sum().ToString();
                    else if (aoDTCurrentSalary.Columns[iColumnIndex].ColumnName == PayrollConstants.S_NET_SALARY)
                    {
                        sValue = (stkfifthPayEarnings.Sum() - stkfifthPayDeductions.Sum()).ToString();
                        stkfifthPayEarnings.Clear();
                        stkfifthPayDeductions.Clear();
                    }

                    if (sValue.Contains(PayrollConstants.S_UNDERSCORE))
                        sValue = sValue.Substring(0, sValue.IndexOf(PayrollConstants.S_UNDERSCORE));

                    oDTPaidSalary.Rows[iRowIndex][iColumnIndex] = sValue;
                }

                stkfifthPayEarnings.Clear();
                stkfifthPayDeductions.Clear();
            }

            UpdatePaidSalaryTableForCurrentUsers(oDTTempTable, aoDTCurrentSalary, oDTPaidSalary);

            oDTPaidSalary.Rows.Clear();
            oDTPaidSalary = oDTTempTable;

            UpdateCurrentSalaryDetails(ref aoDTCurrentSalary, oDTTempPaidSalary, oDTPaidSalary);

            return oDTPaidSalary;
        }

        /// <summary>
        /// This method is used to update base month salary difference according to salary difference configuration.
        /// </summary>
        /// <param name="aoDTCurrentSalary"></param>
        /// <param name="aoDTTempPaidSalary"></param>
        /// <param name="aiDTPaidSalary"></param>
        private void UpdateCurrentSalaryDetails(ref DataTable aoDTCurrentSalary, DataTable aoDTTempPaidSalary, DataTable aiDTPaidSalary)
        {
            DataTable oTempPaidSalary = aoDTTempPaidSalary.Clone();
            int iColumnCount = aoDTTempPaidSalary.Columns.Count;
            for (int iRowIndex = 0; iRowIndex < aoDTTempPaidSalary.Rows.Count; iRowIndex++)
            {
                DataRow oDataRow = aoDTTempPaidSalary.Rows[iRowIndex];
                for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
                {
                    string svalue = oDataRow[iColumnIndex].ToString();
                    if (svalue.IndexOf("_") != -1)
                        oDataRow[iColumnIndex] = svalue.Substring(0, svalue.IndexOf("_"));
                }
                oTempPaidSalary.ImportRow(oDataRow);
            }

            // import rows.
            DataTable oDTPaidSalary = aiDTPaidSalary.Clone();
            foreach (DataRow oDataRow in aiDTPaidSalary.Rows)
                oDTPaidSalary.ImportRow(oDataRow);

            DataTable oDTCurrentSalary = aoDTCurrentSalary.Clone();
            foreach (DataRow oDataRow in aoDTCurrentSalary.Rows)
                oDTCurrentSalary.ImportRow(oDataRow);

            for (int iRowIndex = 0; iRowIndex < oDTCurrentSalary.Rows.Count; iRowIndex++)
            {
                int iUserId = Convert.ToInt32(oDTCurrentSalary.Rows[iRowIndex]["UserId"]);
                int iStaffGroupId = Convert.ToInt32(oDTCurrentSalary.Rows[iRowIndex]["StaffGroupId"]);

                IEnumerable<DataRow> oDatarows = oDTPaidSalary.AsEnumerable().Where(user => user.Field<string>("UserId") == iUserId.ToString());
                IEnumerable<DataRow> oTempDataRow = oTempPaidSalary.AsEnumerable().Where(user => user.Field<string>("UserId") == iUserId.ToString());

                if (oDatarows.Count() > 0)
                {
                    DataRow oDataRow = oDatarows.FirstOrDefault();
                    
                    // Update all non formula fields from base monht paid salary to salary difference field month.
                    UpdateNonFormulaFields(oTempPaidSalary, oDTPaidSalary, oDTCurrentSalary, iRowIndex, oTempDataRow, oDataRow);

                    // Update all formula fields by calculating it on paid salary details.
                    CalculateFormula(oTempPaidSalary, oDTPaidSalary, iUserId, iStaffGroupId, oTempDataRow, oDataRow);

                    // Update summery details.
                    UpdateSummearyDetails(oDTPaidSalary, oDataRow);
                }
            }
            aoDTCurrentSalary = oDTPaidSalary;
        }

        /// <summary>
        /// This method is used to update non formula fields.
        /// </summary>
        /// <param name="oTempPaidSalary"></param>
        /// <param name="oDTPaidSalary"></param>
        /// <param name="oDTCurrentSalary"></param>
        /// <param name="iRowIndex"></param>
        /// <param name="oTempDataRow"></param>
        /// <param name="oDataRow"></param>
        private void UpdateNonFormulaFields(DataTable aoTempPaidSalary, DataTable aoDTPaidSalary, DataTable aoDTCurrentSalary, int aiRowIndex, IEnumerable<DataRow> aoTempDataRows, DataRow aoDataRow)
        {   
            moEarningsDeductionsBL.EarningsDeductions.Where(ED => !ED.HasFormula).ToList().ForEach
                (
                    ED =>
                    {
                        string sShortName = ED.ShortName;
                        bool bIsLeaveDeducted = false;

                        // iterate twice if earning deduction is attendance dependent.
                        do
                        {
                            // update non formula fields if salary difference for same is already paid.
                            if (aoDTPaidSalary.Columns.Contains(sShortName))
                            {
                                if (aoDTCurrentSalary.Columns.Contains(sShortName))
                                {
                                    int iPaidValue = aoDataRow[sShortName] != DBNull.Value ? Convert.ToInt32(aoDataRow[sShortName]) : -1;
                                    int iBaseValue = aoDTCurrentSalary.Rows[aiRowIndex][ED.ShortName] != DBNull.Value ? Convert.ToInt32(aoDTCurrentSalary.Rows[aiRowIndex][ED.ShortName]) : 0;

                                    if (iBaseValue == -1)
                                        iBaseValue = 0;

                                    if (iPaidValue != -1)
                                    {
                                        // calclate actual salary difference.
                                        if (bIsLeaveDeducted)
                                        {
                                            decimal iTotalDays = Convert.ToDecimal(aoDataRow[PayrollConstants.S_TOTAL]);
                                            int iTotalDaysofSalDiffMonth = DateTime.DaysInMonth(miYearId, miMonthId);
                                            iBaseValue = Convert.ToInt32(Math.Round(((iBaseValue / (decimal)iTotalDaysofSalDiffMonth) * iTotalDays), 0));
                                        }

                                        aoDataRow[sShortName] = iBaseValue;

                                        // update base salary details with latestd calculated one.
                                        if (aoTempDataRows.Count() > 0)
                                        {
                                            DataRow oTempRow = aoTempDataRows.First();

                                            if (aoTempPaidSalary.Columns.Contains(sShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                                                sShortName = sShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE);

                                            if (oTempRow[sShortName] != null)
                                                oTempRow[sShortName] = iBaseValue;
                                        }
                                    }
                                }
                            }

                            // rename earning deduction if it is dependent of attendance.
                            if (ED.IsAttendanceDependent && !bIsLeaveDeducted)
                            {
                                sShortName = S_LEAVE_DEDUCTED + ED.ShortName;
                                bIsLeaveDeducted = true;
                            }
                            else
                                bIsLeaveDeducted = false;
                        } while (bIsLeaveDeducted);
                    }
                );
        }

        /// <summary>
        /// This metod is used to update summery details.
        /// </summary>
        /// <param name="oDTPaidSalary"></param>
        /// <param name="oDataRow"></param>
        private void UpdateSummearyDetails(DataTable aoDTPaidSalary, DataRow aoDataRow)
        {
            List<int> lstEarnings = new List<int>();
            List<int> lstDeductions = new List<int>();

            moEarningsDeductionsBL.EarningsDeductions.ForEach(ED =>
            {
                string sShortName = ED.ShortName;

                if (ED.IsAttendanceDependent)
                    sShortName = S_LEAVE_DEDUCTED + sShortName;

                // Add all the earning and deduction amounts into two lists.
                if (aoDTPaidSalary.Columns.Contains(sShortName))
                {
                    int iValue = aoDataRow[sShortName] != DBNull.Value && aoDataRow[sShortName].ToString().Trim() != string.Empty ? Convert.ToInt32(aoDataRow[sShortName]) : 0;
                    if (iValue == -1)
                        iValue = 0;
                    if (ED.IsEarning)
                        lstEarnings.Add(iValue);
                    else
                        lstDeductions.Add(iValue);
                }
            }
            );

            // Update table with earning and deduction sum.
            aoDataRow[PayrollConstants.S_GROSS_SALARY] = lstEarnings.Sum();
            aoDataRow[PayrollConstants.S_TOTAL_DEDUCTION] = lstDeductions.Sum();
            aoDataRow[PayrollConstants.S_NET_SALARY] = lstEarnings.Sum() - lstDeductions.Sum();
            lstEarnings.Clear();
            lstDeductions.Clear();
        }

        /// <summary>
        /// This method is used to calculate formula details.
        /// </summary>
        /// <param name="oTempPaidSalary"></param>
        /// <param name="oDTPaidSalary"></param>
        /// <param name="iUserId"></param>
        /// <param name="iStaffGroupId"></param>
        /// <param name="oTempDataRow"></param>
        /// <param name="oDataRow"></param>
        private void CalculateFormula(DataTable aoTempPaidSalary, DataTable aoDTPaidSalary, int aiUserId, int aiStaffGroupId, IEnumerable<DataRow> aoTempDataRow, DataRow aoDataRow)
        {
            int iEDFormulaValue;
            List<EarningsDeductions> oEarningsDeductionsFormulae = moEarningDeductionFormulaBL.GetEarningDeductionFormulae(aiUserId,moUsersStaffGroupsAssociationBL.UsersSGAssociations, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);
            
            // Iterate each formula field.
            foreach (var EDFormula in oEarningsDeductionsFormulae)
            {
                if (aoDTPaidSalary.Columns.Contains(EDFormula.ShortName))
                {
                    string sFormula = moEarningDeductionFormulaBL.GetFormula(EDFormula, aiUserId);

                    // Iterate each earning deduction to calculate formula value.
                    moEarningsDeductionsBL.EarningsDeductions.ForEach(ED =>
                    {
                        string sShortName = ED.ShortName;
                        if (ED.IsAttendanceDependent)
                            sShortName = S_LEAVE_DEDUCTED + sShortName;

                        if (aoTempPaidSalary.Columns.Contains(sShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE)))
                            sShortName = sShortName.Replace(PayrollConstants.S_SINGLE_SPACE, PayrollConstants.S_UNDERSCORE);

                        if (aoTempPaidSalary.Columns.Contains(sShortName))
                        {
                            if (aoTempDataRow.Count() > 0)
                            {
                                DataRow oTempRow = aoTempDataRow.First();
                                iEDFormulaValue = oTempRow[sShortName] != null && Convert.ToInt32(oTempRow[sShortName]) != -1 ? Convert.ToInt32(oTempRow[sShortName]) : 0;
                                sFormula = sFormula.Replace("'" + ED.EarningsDeductionsId + "'", iEDFormulaValue.ToString());
                            }
                        }
                    });

                    sFormula = moEarningDeductionFormulaBL.GetUpdatedEDFormulaForZeroIDs(aiStaffGroupId, sFormula, moEarningsDeductionsBL.EarningsDeductions);

                    // Evaluate formula for result.
                    MathsExpressionParser oMathsExpressionParser = new MathsExpressionParser();
                    if (oMathsExpressionParser.Evaluate(sFormula))
                    {
                        int iEDValue = Convert.ToInt32(Math.Round(oMathsExpressionParser.Result));
                        aoDataRow[EDFormula.ShortName.ToString()] = iEDValue;

                        // Update table with result.
                        if (aoTempDataRow.Count() > 0)
                        {
                            DataRow oTempRow = aoTempDataRow.First();
                            if (oTempRow[EDFormula.ShortName.ToString()] != null)
                                oTempRow[EDFormula.ShortName.ToString()] = iEDValue;
                        }
                    }
                }
            }

            CalculateRange(aoTempPaidSalary, aoDTPaidSalary, aiUserId, aiStaffGroupId, aoTempDataRow, aoDataRow);
        }

        private void CalculateRange(DataTable aoTempPaidSalary, DataTable aoDTPaidSalary, int aiUserId, int aiStaffGroupId, IEnumerable<DataRow> aoTempDataRow, DataRow aoDataRow)
        {
           // int iEDFormulaValue;
            //List<EarningsDeductions> oEarningsDeductionsFormulae = moEarningDeductionFormulaBL.GetEarningDeductionFormulae(aiUserId, moUsersStaffGroupsAssociationBL.UsersSGAssociations, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);

            List<EarningsDeductions> oEarningsDeductionsFormulae = moAmountRangeBL.GetAmountRanges(aiUserId, moUsersStaffGroupsAssociationBL.UsersSGAssociations, moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociations);

            // Iterate each formula field.
            foreach (var range in oEarningsDeductionsFormulae)
            {
                if (aoDTPaidSalary.Columns.Contains(range.ShortName))
                {
                    if (aoTempPaidSalary.Columns.Contains(range.ShortName))
                    {
                        var iGrossSalary = Convert.ToInt32(aoDataRow["Gross Salary"]);

                        var iAmount = moAmountRangeBL.AmountRanges.Where(ar => ar.FromAmount <= iGrossSalary && iGrossSalary <= ar.UptoAmount).Select(ar => ar.Amount).FirstOrDefault();
                        if (iAmount != 0)
                        {
                            aoDataRow[range.ShortName.ToString()] = Convert.ToInt32(iAmount);

                            if (aoTempDataRow.Count() > 0)
                            {
                                DataRow oTempRow = aoTempDataRow.First();
                                if (oTempRow[range.ShortName.ToString()] != null)
                                    oTempRow[range.ShortName.ToString()] = Convert.ToInt32(iAmount);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to add columns in paid salary details table.
        /// </summary>
        /// <param name="aolstColumnsNames"></param>
        /// <param name="aoDTCurrentSalary"></param>
        /// <param name="aoDTPaidSalary"></param>
        /// <param name="aoDTTempTable"></param>
        private void AddColumnsInPaidSalaryTable(List<string> aolstColumnsNames, DataTable aoDTCurrentSalary, DataTable aoDTPaidSalary, DataTable aoDTTempTable)
        {
            string sColumnName;
            int iColumnCount = aoDTCurrentSalary.Columns.Count;
            for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
            {
                sColumnName = aoDTCurrentSalary.Columns[iColumnIndex].ColumnName;
                aoDTPaidSalary.Columns.Add(sColumnName);
                aoDTTempTable.Columns.Add(sColumnName);
                if (iColumnIndex > 6)
                    aolstColumnsNames.Add(sColumnName);
            }
        }

        /// <summary>
        /// This method is used to update earning deduction stack.
        /// </summary>
        /// <param name="aoDTCurrentSalary"></param>
        /// <param name="asValue"></param>
        /// <param name="abIsEarning"></param>
        /// <param name="astkfifthPayEarnings"></param>
        /// <param name="astkfifthPayDeductions"></param>
        /// <param name="abExcludeFromCalculation"></param>
        /// <param name="aiColumnIndex"></param>
        private void UpdateEarningDeductionStack(DataTable aoDTCurrentSalary, ref string asValue, bool abIsEarning, Stack<int> astkfifthPayEarnings, Stack<int> astkfifthPayDeductions, ref bool abExcludeFromCalculation, bool abIsLeaveDeductedcolumn)
        {
            if (abIsEarning)
                PopulateFifthPayStack(ref asValue, astkfifthPayEarnings, ref abExcludeFromCalculation, abIsLeaveDeductedcolumn);
            else
                PopulateFifthPayStack(ref asValue, astkfifthPayDeductions, ref abExcludeFromCalculation, abIsLeaveDeductedcolumn);
        }

        /// <summary>
        /// This method is used to populate fifth pay stack.
        /// </summary>
        /// <param name="asValue"></param>
        /// <param name="astkfifthPay"></param>
        /// <param name="abExcludeFromCalculation"></param>
        /// <param name="abIsLeaveDeductedcolumn"></param>
        private void PopulateFifthPayStack(ref string asValue, Stack<int> astkfifthPay, ref bool abExcludeFromCalculation, bool abIsLeaveDeductedcolumn)
        {
            // add each element in stact and pop it if earning deduction is attendance dependent and push attendance dependent ed value.
            if (abIsLeaveDeductedcolumn)
            {
                if (!abExcludeFromCalculation)
                    astkfifthPay.Pop();
            }

            if (asValue.Contains(PayrollConstants.S_UNDERSCORE))
            {
                asValue = asValue.Substring(0, asValue.IndexOf(PayrollConstants.S_UNDERSCORE));
                if (asValue != S_EARNING_DEDUCTION_NOT_APPLICABLE)
                {
                    astkfifthPay.Push(Convert.ToInt32(asValue));
                    abExcludeFromCalculation = false;
                }
                else
                    abExcludeFromCalculation = true;
            }
        }

        /// <summary>
        /// This method is used to return salary difference details to export.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSalaryDifferenceToExport(DataTable aoDTSalaryDifference, string asMonthList)
        {
            aoDTSalaryDifference.Columns.RemoveAt(aoDTSalaryDifference.Columns.Count - 4);
            aoDTSalaryDifference.Columns.RemoveAt(3);
            aoDTSalaryDifference.Columns.RemoveAt(2);
            aoDTSalaryDifference.Columns.RemoveAt(1);

            if (!string.IsNullOrEmpty(asMonthList))
            {
                int iRowIndex = aoDTSalaryDifference.Rows.Count;
                DataRow oDataRow = aoDTSalaryDifference.NewRow();
                aoDTSalaryDifference.Rows.InsertAt(oDataRow, iRowIndex);

                iRowIndex = aoDTSalaryDifference.Rows.Count;
                oDataRow = aoDTSalaryDifference.NewRow();
                aoDTSalaryDifference.Rows.InsertAt(oDataRow, iRowIndex);

                aoDTSalaryDifference.Rows[iRowIndex][0] = "Salary difference of this month has been paid in month(s):";
                aoDTSalaryDifference.Rows[iRowIndex][1] = asMonthList;
            }
            return aoDTSalaryDifference;
        }

        #endregion

        #endregion

        public void SaveAll(string asXml)
        {
            moSalaryDifferenceDC.Save(asXml);
        }
    }
}
