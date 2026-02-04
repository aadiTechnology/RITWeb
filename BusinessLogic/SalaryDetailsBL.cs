// Class Name       :- SalaryDetailsBL
// Purpose          :- This class is used to manage SalaryDetails details.
// Date Of creation :- 11/18/2009
// Author Name      :- Sachin


using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataCommunicator;
using PayrollEntities;
using Utility;

using System.Web;
using System.IO;

namespace BusinessLogic
{
    public class SalaryDetailsBL
    {
        #region Salary Detail BL

        #region Data Member

        private SalaryDetailsDC moSalaryDetailsDC;
        private List<string> molstNonLeaveConfiguredUsers;
        private List<string> molstNonEDConfiguredUsers;
        private List<string> molstNonAttendanceUsers;

        int miCacheTimeout;
        int miSchoolId;

        #endregion

        #region Constants

        const string S_BASIC_DETAILS = "BasicDetails";
        const string S_SALARY_ENTITY_LIST = "SchoolEntityList";
        const string S_MONTH_AND_YEAR = "MonthAndYEar";
        const string S_ISDELETED = "IsDeleted";

        #endregion

        #region Constructor

        public SalaryDetailsBL()
        {
            moSalaryDetailsDC = new SalaryDetailsDC();
        }

        public SalaryDetailsBL(int iSchoolId, int iAcademicYearId)
        {
            miSchoolId = iSchoolId;
            moSalaryDetailsDC = new SalaryDetailsDC(iSchoolId, iAcademicYearId);
        }

        #endregion

        #region Properties

        public SalaryDetails SalaryDetails
        {
            get { return moSalaryDetailsDC.SalaryDetails; }
            set { moSalaryDetailsDC.SalaryDetails = value; }
        }

        public SalaryEntityList SalaryEntityLists
        {
            get { return moSalaryDetailsDC.SalaryEntityLists; }
            set { moSalaryDetailsDC.SalaryEntityLists = value; }
        }

        public List<SalaryMonth> Months
        {
            get { return moSalaryDetailsDC.Months; }
            set { moSalaryDetailsDC.Months = value; }
        }

        public List<SalaryYear> Years
        {
            get { return moSalaryDetailsDC.Years; }
            set { moSalaryDetailsDC.Years = value; }
        }

        public int MinUserStaffGroupId
        {
            get { return moSalaryDetailsDC.iMinUserStaffGroupId; }
            set { moSalaryDetailsDC.iMinUserStaffGroupId = value; }
        }

        public List<PaidSalaryDetails> UserSalaryDetails
        {
            get { return moSalaryDetailsDC.UserSalaryDetails; }
        }

        public List<PaidSalaryDetails> PaidSalaryDetails
        {
            get { return moSalaryDetailsDC.PaidSalaryDetails; }
        }

        public List<StaffAttendance> StaffAttendanceList
        {
            get { return moSalaryDetailsDC.moStaffAttendanceList; }
        }

        public List<StaffLeaveDetails> StaffLeaveDetailsList
        {
            get { return moSalaryDetailsDC.moStaffLeaveDetailsList; }
        }

        public List<string> NonLeaveConfiguredUsers
        {
            get { return molstNonLeaveConfiguredUsers; }
        }

        public List<string> NonEarnDeductConfiguredUsers
        {
            get { return molstNonEDConfiguredUsers; }
        }

        public List<string> NonAttendanceUsers
        {
            get { return molstNonAttendanceUsers; }
        }
        public char IsLeaveIntervalMonth
        {
            get { return moSalaryDetailsDC.moBasicDetails.IsLeaveIntervalMonth; }
        }
        
        public int CacheTimeout
        {
            get { return miCacheTimeout; }
            set { miCacheTimeout = value; }
        }

        #endregion

        #region Methods

        public void Insert()
        {
            moSalaryDetailsDC.Insert();
        }

        public static bool CheckPTChallanDetailsExists(int aiSchoolId, int aiMonthId, int aiYear)
        {
            return SalaryDetailsDC.CheckPTChallanDetailsExists(aiSchoolId, aiMonthId, aiYear);
        }

        public void InsertIndividualDetails()
        {
            moSalaryDetailsDC.InsertIndividualDetails();
            //UpdateSalaryEntityListCache();
        }

        private void UpdateSalaryEntityListCache()
        {
            if (HttpContext.Current.Cache[S_SALARY_ENTITY_LIST] != null)
            {
                SalaryEntityList oSalaryEntityList = HttpContext.Current.Cache[S_SALARY_ENTITY_LIST] as SalaryEntityList;
                oSalaryEntityList.lstStaffAttendance = SalaryEntityLists.lstStaffAttendance;
                oSalaryEntityList.lstStaffLeaveDetails = SalaryEntityLists.lstStaffLeaveDetails;
                oSalaryEntityList.lstUsersEarningsDeduction = SalaryEntityLists.lstUsersEarningsDeduction;
                oSalaryEntityList.lstUserLeaveConfiguration = SalaryEntityLists.lstUserLeaveConfiguration;
                oSalaryEntityList.lstUserLateMarkLeaves = SalaryEntityLists.lstUserLateMarkLeaves;
                HttpContext.Current.Cache[S_SALARY_ENTITY_LIST] = oSalaryEntityList;
            }
        }

        public void Save()
        {
            moSalaryDetailsDC.Save();
            //UpdateSalaryEntityListCache();
        }

        public static DataSet GetSalaryMonthAndYear(int aiSchoolId, int aiAcademicYEarId)
        {
            return SalaryDetailsDC.GetSalaryMonthAndYear(aiSchoolId, aiAcademicYEarId);
        }

        public static void DeleteSalary(int aiSchoolId, int aiAcademicYearId, int aiMonthid, int aiYear)
        {
            SalaryDetailsDC.DeleteSalary(aiSchoolId, aiAcademicYearId, aiMonthid, aiYear);
        }

        public void GetStaffGroupsAndMonths(int aiSchoolId, int aiAcademicYearId)
        {
            moSalaryDetailsDC.GetStaffGroupsAndMonths(aiSchoolId, aiAcademicYearId);
        }

        public static void Unpublish(int aiSchoolId, int aiAcademicYearId, int aiMonthId, int aiYear, int aiInsertedById, int aiLeaveTransferMonth)
        {
            SalaryDetailsDC.Unpublish(aiSchoolId, aiAcademicYearId, aiMonthId, aiYear, aiInsertedById, aiLeaveTransferMonth);
        }

        public static int GetStaffGroupId(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiMonthId, int aiYear)
        {
            return SalaryDetailsDC.GetStaffGroupId(aiSchoolId, aiAcademicYearId, aiUserId, aiMonthId, aiYear);
        }

        public static DataSet GetUsersStaffGroupDetais(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asStartDate, string asEndDate, int aiFinancialYearId)
        {
            return SalaryDetailsDC.GetUsersStaffGroupDetais(aiSchoolId, aiAcademicYearId, aiUserId, asStartDate, asEndDate, aiFinancialYearId);
        }

        public static DataTable GetUserDetails(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId)
        {
            return SalaryDetailsDC.GetUserDetails(aiSchoolId, aiAcademicYearId, aiStaffGroupId);
        }

        public void GetLeavesAndUsers(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int aiMonthId, int aiYear)
        {
            moSalaryDetailsDC.GetLeavesAndUsers(aiSchoolId, aiAcademicYearId, aiStaffGroupId, aiMonthId, aiYear);
        }

        public static DataSet GetSalaryDetailTables(int aiSchoolId, int aiAcademicYearId, int aiMOnthId, int aiYear, int aiStaffGroupsId)
        {
            return SalaryDetailsDC.GetSalaryDetailTables(aiSchoolId, aiAcademicYearId, aiMOnthId, aiYear, aiStaffGroupsId);
        }

         /// <summary>
        /// This method is used to return salary payment details.
        /// </summary>
        /// <returns></returns>
        public List<SalaryPaymentDetails> GetAllPaymentDetails()
        {
            return moSalaryDetailsDC.GetAllPaymentDetails();
        }

        #endregion

        #region Salary Details Entities

        public void GetSalaryTables(int aiMOnthId, int aiYear, int aiStaffGroupsId, bool abIsPageInit, bool abReLoad)
        {
            moSalaryDetailsDC.GetSalaryTables(aiMOnthId, aiYear, 0);
        }

        public BasicDetails BasicDetails
        {
            get { return moSalaryDetailsDC.moBasicDetails; }
            set { moSalaryDetailsDC.moBasicDetails = value; }
        }

        public MonthAndYear clsMonthAndYear
        {
            get { return moSalaryDetailsDC.oMonthAndYear; }
            set { moSalaryDetailsDC.oMonthAndYear = value; }
        }

        public SalaryCommonUtility SalaryMonthAndYear
        {
            get { return moSalaryDetailsDC.oSalaryMonthAndYear; }
            set { moSalaryDetailsDC.oSalaryMonthAndYear = value; }
        }

        #endregion

        #endregion

        #region Business Logic

        const string S_CONNECTOR = "_";
        const string S_LEAVE_DEDUCTED_CONNECTOR = "_LD";
        const string S_EARNING_DEDUCTION_CONNECTOR = "_ED_";
        const string S_HOLIDAY_LEAVES = "_HL_";
        const string S_NET_SALARY = "Net Salary";
        const string S_TOTAL_DEDUCTION = "Total Deduction";
        const string S_HOLIDAY_LEAVE_DEDUCTION = "Holiday Leaves Deduction";
        const string S_GROSS_SALARY = "Gross Salary";
        const string S_SALARY_DIFFERENCE = "Gross Salary Difference";
        const string S_SALARY_DIFFERENCE_PF = "Salary Difference of Deduction";
        const string S_HOLIDAY_LEAVE = "Holiday Leaves";

        const string S_EARNING_DEDUCTION = "_ED_";
        const string S_LEAVE_DEDUCTED = "_LD";

        DataTable moDTTempSalaryDetails;
        DataTable moDTSalaryDetails;
        List<string> olstTotalEarningsDeductions;
        string msMonthList = string.Empty;
        int miTotalPages;

        public bool IsInvalidLeaveExists { get; set; }

        public int TotalPages
        {
            get { return miTotalPages; }
        }

        public DataSet GetSalaryDetailsDataset(int aiMonthId, int aiYear, int aiStaffGroupId, string asFilter, int aiPageIndex, int aiPageSize, bool abIsPageInit, bool abReLoad)
        {
            GetSalaryTables(aiMonthId, aiYear, aiStaffGroupId, abIsPageInit, abReLoad);
            int iTotalDaysOfMonth = DateTime.DaysInMonth(aiYear, aiMonthId);
            int iDaysOfMonth;

            var lstSalaryDifference = SalaryEntityLists.lstSalaryDifference.Select(salaryDifference => new { MonthId = salaryDifference.MonthId, Year = salaryDifference.Year }).ToList();
            StringBuilder oMonthList = new StringBuilder();
            if (BasicDetails.DisplaySalaryDifference)
            {
                if (lstSalaryDifference.Count() > 0)
                {
                    var SalaryDiff = lstSalaryDifference.ToList().Distinct().ToList();
                    foreach (var difference in SalaryDiff)
                        oMonthList.Append(", " + (String.Format("{0:MMMM}", Convert.ToDateTime("2010-" + difference.MonthId + "-02")) + " - " + difference.Year));

                    if (oMonthList.ToString().Trim().Length > 1)
                        msMonthList = oMonthList.ToString().Substring(1);
                }
            }
            else
                msMonthList = string.Empty;


            DataSet oDataSet = new DataSet();

            if (!BasicDetails.IsStaticData)
            {

                olstTotalEarningsDeductions = new List<string>();

                int iEarningsSum = 0;
                int iDeductionSum = 0;

                int iStartIndex = aiPageIndex * aiPageSize;

                moDTTempSalaryDetails = new DataTable();
                moDTSalaryDetails = new DataTable();

                AddBasicColumns();
                AddAttendanceLeavesColumns();
                AddEarningDeductionsColumns();

                moDTTempSalaryDetails.Columns.Add(S_ISDELETED);

                List<UsersDetails> filteredUsers = new List<UsersDetails>();
                List<UsersDetails> users = new List<UsersDetails>();
                if (!string.IsNullOrEmpty(asFilter))
                {
                    int iItemIndex = 1;
                    if (aiStaffGroupId == 0)
                        users = SalaryEntityLists.lstUsersDetails.Where(user => user.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();
                    else
                        users = SalaryEntityLists.lstUsersDetails.Where(user => user.StaffGroupsId == aiStaffGroupId && user.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();

                    users.ForEach(user => { user.SerialNo = iItemIndex++; });
                    filteredUsers = users.Where(user => user.SerialNo > iStartIndex && user.SerialNo <= iStartIndex + aiPageSize).ToList();
                }
                else
                {
                    if (aiStaffGroupId != 0)
                        filteredUsers = SalaryEntityLists.lstUsersDetails.Where(user => user.StaffGroupsId == aiStaffGroupId && user.SrNo > iStartIndex && user.SrNo <= iStartIndex + aiPageSize).ToList();
                    else
                        filteredUsers = SalaryEntityLists.lstUsersDetails.Where(user => user.SrNo > iStartIndex && user.SrNo <= iStartIndex + aiPageSize).ToList();
                }

                CheckNonConfiguredUsers(aiMonthId,aiYear);

                List<UsersBasicDetails> UserDetails = filteredUsers.Select(UserDetail =>
                                                                             new UsersBasicDetails
                                                                             {
                                                                                 Name = UserDetail.Name,
                                                                                 Designation = UserDetail.Designation,
                                                                                 UserId = UserDetail.UserId,
                                                                                 OriginalStaffGroupId = UserDetail.OriginalStaffGroupsId,
                                                                                 StaffGroupId = UserDetail.StaffGroupsId,
                                                                                 Is_Deleted = UserDetail.Is_Deleted,
                                                                                 Gender = UserDetail.Gender
                                                                             })
                                                                    .ToList();

                miTotalPages = asFilter == string.Empty ? (SalaryEntityLists.lstUsersDetails.Count / aiPageSize) + 1 : (users.Count / aiPageSize) + 1;

                int iSrNo = iStartIndex;
                int iRowIndex = 0;
                decimal dcUnpaidLeaves = 0;
                decimal dcTotalDays = 0;

                decimal dcHolidayLeaveDeductionAmount;

                foreach (UsersBasicDetails userDetails in UserDetails)
                {
                    iSrNo++;
                    dcHolidayLeaveDeductionAmount = 0;
                    moDTSalaryDetails.Rows.Add();
                    moDTTempSalaryDetails.Rows.Add();
                    SetBasicDetails(userDetails, iRowIndex, iSrNo);

                    iDaysOfMonth = DateTime.DaysInMonth(aiYear, aiMonthId);

                    //********************** Start - Attendance and Leaves ************************************************************

                    dcUnpaidLeaves = GetUnpaidLeavesCount(userDetails.UserId, iRowIndex);

                    dcTotalDays = SetAttendanceDetails(iRowIndex, dcUnpaidLeaves, userDetails.UserId, iDaysOfMonth);

                    //********************** Start - Users Earning Deduction ************************************************************

                    List<UsersEarnDeductDetails> UsersEarningsDeductions = GetUSersEarningDeductions(userDetails);

                    int iUsersEarningsDeductionsCount = UsersEarningsDeductions.Count();
                    if (iUsersEarningsDeductionsCount > 0)
                        SetUsersEarningsDeductions(iDaysOfMonth, ref iEarningsSum, ref iDeductionSum, iRowIndex, dcTotalDays, UsersEarningsDeductions);
                    else
                        SetDefaultEDValuesIfNotAvail(iRowIndex, userDetails.UserId);

                    SetDefaultEDValuesIfNotAssociated(iRowIndex, userDetails.UserId);

                    //********************** Start - Users Earning Deduction -  Formula Value ************************************************************

                    var EarningsDeductionsFormulae = from EDFormula in SalaryEntityLists.lstEarningsDeductionsFormulae.AsEnumerable()
                                                     join EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                                     on EDFormula.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                                                     join SGEDAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                                     on EDFormula.EarningsDeductionsId equals SGEDAsso.EarningsDeductionsId
                                                     join UserSG in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                                     on SGEDAsso.StaffGroupsId equals UserSG.StaffGroupsId
                                                     where UserSG.UserId == userDetails.UserId &&
                                                            EDFormula.IsDefault == true
                                                     orderby EarnDeduction.OriginalEarningsDeductionsId ascending
                                                     select new
                                                     {
                                                         EarningsDeductionsId = EDFormula.EarningsDeductionsId,
                                                         Formula = EDFormula.Formula,
                                                         ShortName = EarnDeduction.ShortName,
                                                         IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                                                         IsEarning = EarnDeduction.IsEarning,
                                                         HasFormula = EarnDeduction.HasFormula,
                                                         FormulaId = EDFormula.FormulaId
                                                     };

                    int iLeaveDeductedED = 0;
                    MathsExpressionParser oMathsExpressionParser = new MathsExpressionParser();
                    int iEDFormulaValue = 0;

                    List<UsersEarnDeductDetails> UsersEDForFormula = UsersEarningsDeductions;
                    Dictionary<int, decimal> dictEDValue = new Dictionary<int, decimal>();
                    foreach (var EDFormula in EarningsDeductionsFormulae)
                    {
                        List<string> UsersFormulaED = SalaryEntityLists.lstUsersFormulaAndRanges
                                                      .Join(SalaryEntityLists.lstEarningsDeductionsFormulae, UsersED => UsersED.FormulaRangeId, Formula => Formula.FormulaId, (UsersED, Formula) => new { UsersED = UsersED, Formula = Formula })
                                                      .Where(User => User.Formula.EarningsDeductionsId == EDFormula.EarningsDeductionsId && User.UsersED.UserId == userDetails.UserId && User.UsersED.IsFormula)
                                                      .Select(user => user.Formula.Formula).ToList();

                        string sFormula = string.Empty;
                        string sTempFormula = string.Empty;

                        if (UsersFormulaED.Count() > 0)
                            sFormula = UsersFormulaED.First();
                        else
                            sFormula = EDFormula.Formula.ToString();
                        sFormula = sFormula.Replace(",", "");
                        sFormula = sFormula.Replace("%", "/100");

                        sTempFormula = sFormula;
                        UsersEDForFormula = UsersEarningsDeductions.ToList();
                        decimal iTempEDValue = 0;

                        DesignFormula(iDaysOfMonth, dcTotalDays, ref iEDFormulaValue, UsersEDForFormula, dictEDValue, ref sFormula, ref sTempFormula, ref iTempEDValue);

                        List<int> UsersEarnDeduct = SalaryEntityLists.lstUsersEarningsDeduction.Where(UsersEarnDeduction => UsersEarnDeduction.UserId == userDetails.UserId)
                                                                       .Select(UsersEarnDeduction => UsersEarnDeduction.EarningsDeductionsId).ToList();

                        List<int> STEDAssociation = SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.Where(SGEDAsso => SGEDAsso.StaffGroupsId == userDetails.StaffGroupId)
                                                                        .Select(SGEDAsso => SGEDAsso.EarningsDeductionsId).ToList();

                        List<int> EDs = SalaryEntityLists.lstEarningsDeductions.Select(EarnDeduct => EarnDeduct.EarningsDeductionsId).ToList();

                        List<int> RemAsso = EDs.Except(STEDAssociation).ToList();
                        List<int> RemUsersEDs = EDs.Except(UsersEarnDeduct).ToList();
                        List<int> TotalEDs = RemUsersEDs.Union(RemAsso).ToList();

                        foreach (int ED in EDs)
                        {
                            sFormula = sFormula.Replace("'" + ED + "'", "0");
                            sTempFormula = sTempFormula.Replace("'" + ED + "'", "0");
                        }

                        sFormula = sFormula.Replace("'", "");
                        sTempFormula = sTempFormula.Replace("'", "");

                        int valueOfED = 0;
                        if (oMathsExpressionParser.Evaluate(sFormula))
                        {
                            int iEDValue = Convert.ToInt32(Math.Round(oMathsExpressionParser.Result));

                            oMathsExpressionParser.Evaluate(sTempFormula);
                            int iEDTempFormula = Convert.ToInt32(Math.Round(oMathsExpressionParser.Result));

                            dictEDValue.Add(EDFormula.EarningsDeductionsId, iEDTempFormula);
                            moDTSalaryDetails.Rows[iRowIndex][EDFormula.ShortName.ToString()] = iEDValue;
                            moDTTempSalaryDetails.Rows[iRowIndex][EDFormula.ShortName.ToString()] = iEDValue + S_EARNING_DEDUCTION + EDFormula.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(EDFormula.HasFormula);
                            if (Convert.ToBoolean(EDFormula.IsAttendanceDependent) == true)
                            {
                                iLeaveDeductedED = Convert.ToInt32(Math.Round((dcTotalDays / iDaysOfMonth) * iEDValue));
                                dcHolidayLeaveDeductionAmount = dcHolidayLeaveDeductionAmount + iEDTempFormula - iLeaveDeductedED;

                                moDTSalaryDetails.Rows[iRowIndex]["Leave Deducted " + EDFormula.ShortName.ToString()] = iLeaveDeductedED;
                                moDTTempSalaryDetails.Rows[iRowIndex]["Leave Deducted " + EDFormula.ShortName.ToString()] = iLeaveDeductedED + S_LEAVE_DEDUCTED;
                                if (Convert.ToBoolean(EDFormula.IsEarning))
                                    iEarningsSum = iEarningsSum + iLeaveDeductedED;
                                else
                                    iDeductionSum = iDeductionSum + iLeaveDeductedED;
                                valueOfED = iLeaveDeductedED;
                            }
                            else
                            {
                                if (Convert.ToBoolean(EDFormula.IsEarning))
                                {
                                    iEarningsSum = iEarningsSum + iEDValue;
                                    dcHolidayLeaveDeductionAmount = dcHolidayLeaveDeductionAmount + iEDTempFormula - iEDValue;
                                }
                                else
                                {
                                    iDeductionSum = iDeductionSum + iEDValue;
                                    dcHolidayLeaveDeductionAmount = dcHolidayLeaveDeductionAmount - (iEDTempFormula - iEDValue);
                                }
                                valueOfED = iEDValue;
                            }
                        }

                        int iEDId = Convert.ToInt32(EDFormula.EarningsDeductionsId);

                        List<UsersEarnDeductDetails> EDAppend = GetUsersEarningDeductionDetails(valueOfED, iEDId);

                        UsersEarningsDeductions = UsersEDForFormula.Union(EDAppend).ToList();
                    }
                    //********************** Start - Users Earning Deduction - Range value ************************************************************

                    iEarningsSum = SetEarningsDeductionsRange(iRowIndex, userDetails.UserId, iEarningsSum, ref iDeductionSum, dcTotalDays, iDaysOfMonth, aiMonthId, userDetails.Gender);

                    if (BasicDetails.DisplaySalaryDifference)
                    {   
                        var SalaryDifferenceList = SalaryEntityLists.lstSalaryDifference.Where(salaryDifference => salaryDifference.UserId == userDetails.UserId)
                                                                            .Select(salaryDifference => new { GrossSalary = Convert.ToInt32(Math.Round(salaryDifference.GrossSalary)), ProvidentFund = Convert.ToInt32(Math.Round(salaryDifference.ProvidentFund)) }).ToList();

                        int iDifferenceAmount = 0;
                        int iPFAmount = 0;
                        if (SalaryDifferenceList != null && SalaryDifferenceList.Count > 0)
                        {
                            iDifferenceAmount = SalaryDifferenceList.First().GrossSalary;
                            iEarningsSum = iEarningsSum + iDifferenceAmount;

                            iPFAmount = SalaryDifferenceList.First().ProvidentFund;
                            iDeductionSum = iDeductionSum + iPFAmount;
                        }

                        moDTSalaryDetails.Rows[iRowIndex][S_SALARY_DIFFERENCE] = iDifferenceAmount;
                        moDTTempSalaryDetails.Rows[iRowIndex][S_SALARY_DIFFERENCE] = iDifferenceAmount + S_EARNING_DEDUCTION + "00" + S_CONNECTOR + "1";

                        moDTSalaryDetails.Rows[iRowIndex][S_SALARY_DIFFERENCE_PF] = iPFAmount;
                        moDTTempSalaryDetails.Rows[iRowIndex][S_SALARY_DIFFERENCE_PF] = iPFAmount + S_EARNING_DEDUCTION + "00" + S_CONNECTOR + "1";
                    }

                    SetDisplayOfSaveButton(iRowIndex, Convert.ToInt32(userDetails.UserId));
                    moDTSalaryDetails.Rows[iRowIndex][S_GROSS_SALARY] = iEarningsSum;
                    moDTSalaryDetails.Rows[iRowIndex][S_TOTAL_DEDUCTION] = iDeductionSum;

                    moDTSalaryDetails.Rows[iRowIndex][S_NET_SALARY] = iEarningsSum - iDeductionSum;
                    moDTTempSalaryDetails.Rows[iRowIndex][S_GROSS_SALARY] = iEarningsSum;
                    moDTTempSalaryDetails.Rows[iRowIndex][S_TOTAL_DEDUCTION] = iDeductionSum;
                    moDTTempSalaryDetails.Rows[iRowIndex][S_NET_SALARY] = iEarningsSum - iDeductionSum;

                    moDTTempSalaryDetails.Rows[iRowIndex][S_ISDELETED] = userDetails.Is_Deleted;

                    iEarningsSum = 0;
                    iDeductionSum = 0;
                    iRowIndex++;
                }


                int iRowCounter = iRowIndex;

                int iNetTotalRowIndex = 0;

                SetSummaryDetails(iRowIndex, aiStaffGroupId, iRowIndex, ref iNetTotalRowIndex);

                IEnumerable<DataRow> SortedSalaryDetails = from SalDetails in moDTTempSalaryDetails.AsEnumerable()
                                                           orderby Convert.ToInt32(SalDetails["OriginalStaffGroupsId"]) ascending, Convert.ToInt32(SalDetails["SortOrder"]) ascending
                                                           select SalDetails;

                DataTable ODTSalary = new DataTable();
                DataTable oDTMonth = GetMonthTable();

                DataTable oDTNetSalSum = new DataTable();
                oDTNetSalSum.Columns.Add("NetSalarySum");

                if (SortedSalaryDetails.Count() > 0)
                {
                    ODTSalary = SortedSalaryDetails.CopyToDataTable();
                    oDTNetSalSum.Rows.Add();
                    oDTNetSalSum.Rows[0][0] = moDTSalaryDetails.Rows[iNetTotalRowIndex][S_NET_SALARY];
                    oDataSet.Tables.Add(ODTSalary);
                    oDataSet.Tables.Add(oDTNetSalSum);
                    oDataSet.Tables.Add(oDTMonth);
                }
            }
            else
            {
                DataTable oDataTable = GetStaticSalaryXML(aiStaffGroupId, asFilter);
                DataTable oDTUnpublishStatus = GetUnpublishStatus();

                oDataSet.Tables.Add(oDataTable);
                oDataSet.Tables.Add(oDTUnpublishStatus);
            }

            IsValidateLeaves(aiStaffGroupId);
            
            return oDataSet;
        }

        /// <summary>
        /// This method is used to check non configured users.
        /// </summary>
        private void CheckNonConfiguredUsers(int aiMonthId, int aiYear)
        {
            DateTime dtCurrentJoiningDate = new DateTime(aiYear, aiMonthId, DateTime.DaysInMonth(aiYear, aiMonthId));
            DateTime dtCurrentResignDate = new DateTime(aiYear, aiMonthId, 1);

            molstNonLeaveConfiguredUsers = new List<string>();
            molstNonLeaveConfiguredUsers.AddRange(SalaryEntityLists.lstUsersDetails.Where(user => SalaryEntityLists.lstUserLeaveConfiguration.FindAll(leave => leave.UserId == user.UserId).Count == 0).Select(user => user.Name).ToList());

            molstNonAttendanceUsers = new List<string>();
            molstNonAttendanceUsers.AddRange(SalaryEntityLists.lstUsersDetails.Where(user => SalaryEntityLists.lstStaffAttendance.FindAll(leave => leave.UserId == user.UserId).Count == 0 && (user.JoiningDate <= dtCurrentJoiningDate || user.JoiningDate == DateTime.MinValue) && (user.ResignDate >= dtCurrentResignDate || user.ResignDate == DateTime.MinValue)).Select(user => user.Name).ToList());

            var oUsers = from SGED in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation
                         join USGA in SalaryEntityLists.lstUsersSGAssociation
                         on SGED.StaffGroupsId equals USGA.StaffGroupsId
                         join usersED in SalaryEntityLists.lstUsersEarningsDeduction
                         on USGA.UserId equals usersED.UserId
                         select usersED.UserId;


            molstNonEDConfiguredUsers = new List<string>();
            molstNonEDConfiguredUsers.AddRange(SalaryEntityLists.lstUsersDetails.Where(user => !oUsers.Contains(user.UserId)).Select(user => user.Name).ToList());
        }

        private DataTable GetStaticSalaryXML(int aiStaffGroupId, string asFilter)
        {
            DataTable oDataTable = new DataTable();
            oDataTable.Columns.Add("SalaryDetailsXml");

            List<StaticSalaryDetails> salXml;
            if (aiStaffGroupId == 0)
                salXml = SalaryEntityLists.lstStaticSalaryDetails.Where(salaryXML => salaryXML.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();
            else
                salXml = SalaryEntityLists.lstStaticSalaryDetails.Where(salaryXML => salaryXML.StaffGroupId == aiStaffGroupId && salaryXML.Name.ToUpper().Contains(asFilter.ToUpper())).ToList();

            int iRowIndex = 0;
            StringBuilder oSalaryDetailsXml = new StringBuilder();
            foreach (StaticSalaryDetails xml in salXml)
                oSalaryDetailsXml.Append(xml.SalaryDetailsXml);

            if (oSalaryDetailsXml.Length > 0)
            {
                oDataTable.Rows.Add();
                oDataTable.Rows[iRowIndex++]["SalaryDetailsXml"] = "<SalaryDetailsXml>" + oSalaryDetailsXml.ToString() + "</SalaryDetailsXml>";
            }
            return oDataTable;
        }

        private void IsValidateLeaves(int aiStaffGroupId)
        {
            IsInvalidLeaveExists = false;
            var usersLeaves = (from userLeave in SalaryEntityLists.lstUserLeaveConfiguration
                               join configLeave in SalaryEntityLists.lstConfiguredLeaves
                               on userLeave.LeaveId equals configLeave.LeaveId
                               join userSG in SalaryEntityLists.lstUsersSGAssociation
                               on userLeave.UserId equals userSG.UserId
                               where userLeave.OriginalLeaveBalance < 0
                               && configLeave.IsUnpaidLeave == false
                               select new
                               {
                                   OriginalLeaveBalance = userLeave.OriginalLeaveBalance,
                                   IsUnpaidLeave = configLeave.IsUnpaidLeave,
                                   StaffGroupId = userSG.StaffGroupsId
                               }).ToList();

            if (aiStaffGroupId != 0)
                usersLeaves = usersLeaves.Where(leave => leave.StaffGroupId == aiStaffGroupId).ToList();

            if (usersLeaves.Count > 0)
                IsInvalidLeaveExists = true;
        }

        private DataTable GetUnpublishStatus()
        {
            DataTable oDTUnpublishStatus = new DataTable();
            oDTUnpublishStatus.Columns.Add("AllowUnpublish");
            oDTUnpublishStatus.Columns.Add("IsNextMonthAttendanceAvailable");
            oDTUnpublishStatus.Columns.Add("MonthList");

            oDTUnpublishStatus.Rows.Add();
            oDTUnpublishStatus.Rows[0]["AllowUnpublish"] = BasicDetails.UnpublishStatus;
            oDTUnpublishStatus.Rows[0]["IsNextMonthAttendanceAvailable"] = BasicDetails.IsNextMonthAttendanceAvailable;
            oDTUnpublishStatus.Rows[0]["MonthList"] = msMonthList;
            return oDTUnpublishStatus;
        }

        private DataTable GetMonthTable()
        {
            DataTable oDTMonth = new DataTable();
            oDTMonth.Columns.Add("MonthId");
            oDTMonth.Columns.Add("Year");
            oDTMonth.Columns.Add("MonthList");

            oDTMonth.Rows.Add();
            oDTMonth.Rows[0]["Monthid"] = clsMonthAndYear.MonthId;
            oDTMonth.Rows[0]["Year"] = clsMonthAndYear.Year;
            oDTMonth.Rows[0]["MonthList"] = msMonthList;
            return oDTMonth;
        }

        private List<UsersEarnDeductDetails> GetUsersEarningDeductionDetails(int valueOfED, int iEDId)
        {
            List<UsersEarnDeductDetails> EDAppend = (from AppendED in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                                     where AppendED.EarningsDeductionsId == iEDId
                                                     select new UsersEarnDeductDetails
                                                     {
                                                         EarningsDeductionsId = AppendED.EarningsDeductionsId,
                                                         ShortName = AppendED.ShortName,
                                                         EarningsDeductionsValue = valueOfED,
                                                         IsAttendanceDependent = AppendED.IsAttendanceDependent,
                                                         IsEarning = AppendED.IsEarning,
                                                         HasFormula = AppendED.HasFormula
                                                     }).ToList();
            return EDAppend;
        }

        private List<UsersEarnDeductDetails> GetUSersEarningDeductions(UsersBasicDetails userDetails)
        {
            List<UsersEarnDeductDetails> UsersEarningsDeductions = (from UsersEarnDeduction in SalaryEntityLists.lstUsersEarningsDeduction.AsEnumerable()
                                                                    join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                                                    on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                                                                    join EarnDeductions in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                                                    on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId

                                                                    join SGEDAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                                                    on UserSGAsso.StaffGroupsId equals SGEDAsso.StaffGroupsId

                                                                    where UsersEarnDeduction.UserId == userDetails.UserId
                                                                      && EarnDeductions.HasFormula == false
                                                                      && EarnDeductions.EarningsDeductionsId == SGEDAsso.EarningsDeductionsId

                                                                    select new UsersEarnDeductDetails
                                                                    {
                                                                        EarningsDeductionsId = UsersEarnDeduction.EarningsDeductionsId,
                                                                        ShortName = EarnDeductions.ShortName,
                                                                        EarningsDeductionsValue = UsersEarnDeduction.EarningsDeductionsValue,
                                                                        IsAttendanceDependent = EarnDeductions.IsAttendanceDependent,
                                                                        IsEarning = EarnDeductions.IsEarning,
                                                                        HasFormula = EarnDeductions.HasFormula
                                                                    }).ToList();
            return UsersEarningsDeductions;
        }

        private static void DesignFormula(int iDaysOfMonth, decimal dcTotalDays, ref int iEDFormulaValue, List<UsersEarnDeductDetails> UsersEDForFormula, Dictionary<int, decimal> dictEDValue, ref string sFormula, ref string sTempFormula, ref decimal iTempEDValue)
        {
            foreach (var UsersED in UsersEDForFormula)
            {
                iEDFormulaValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(UsersED.EarningsDeductionsValue)));
                iTempEDValue = iEDFormulaValue;

                var formula = from EDF in dictEDValue
                              where EDF.Key == UsersED.EarningsDeductionsId
                              select EDF;

                if (formula.Count() > 0)
                    iTempEDValue = formula.First().Value;

                if (Convert.ToBoolean(UsersED.IsAttendanceDependent))
                {
                    iEDFormulaValue = Convert.ToInt32(Math.Round((dcTotalDays / iDaysOfMonth) * iEDFormulaValue));
                    iTempEDValue = iEDFormulaValue;
                }
                sTempFormula = sTempFormula.Replace("'" + UsersED.EarningsDeductionsId + "'", iTempEDValue.ToString());
                sFormula = sFormula.Replace("'" + UsersED.EarningsDeductionsId + "'", iEDFormulaValue.ToString());
            }
        }

        private void SetUsersEarningsDeductions(int iDaysOfMonth, ref int iEarningsSum, ref int iDeductionSum, int iRowIndex, decimal dcTotalDays, List<UsersEarnDeductDetails> UsersEarningsDeductions)
        {
            int EDValue = 0;
            int iLeaveDeductedValue = 0;
            foreach (UsersEarnDeductDetails UsersED in UsersEarningsDeductions)
            {
                EDValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(UsersED.EarningsDeductionsValue)));
                moDTSalaryDetails.Rows[iRowIndex][UsersED.ShortName.ToString()] = EDValue;
                moDTTempSalaryDetails.Rows[iRowIndex][UsersED.ShortName.ToString()] = EDValue + S_EARNING_DEDUCTION + UsersED.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(UsersED.HasFormula);
                if (Convert.ToBoolean(UsersED.IsAttendanceDependent) == true)
                {
                    iLeaveDeductedValue = Convert.ToInt32(Math.Round((dcTotalDays / iDaysOfMonth) * EDValue));
                    moDTSalaryDetails.Rows[iRowIndex]["Leave Deducted " + UsersED.ShortName.ToString()] = iLeaveDeductedValue;
                    moDTTempSalaryDetails.Rows[iRowIndex]["Leave Deducted " + UsersED.ShortName.ToString()] = iLeaveDeductedValue + S_LEAVE_DEDUCTED;
                    if (Convert.ToBoolean(UsersED.IsEarning))
                        iEarningsSum = iEarningsSum + iLeaveDeductedValue;
                    else
                        iDeductionSum = iDeductionSum + iLeaveDeductedValue;
                }
                else
                {
                    if (Convert.ToBoolean(UsersED.IsEarning))
                        iEarningsSum = iEarningsSum + EDValue;
                    else
                        iDeductionSum = iDeductionSum + EDValue;
                }
            }
        }

        /// <summary>
        /// This methid is used to return no of days as respective to user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiTotalDaysOfMonth"></param>
        /// <returns></returns>
        //private int GetDaysOfMonth(int aiUserId, int aiTotalDaysOfMonth)
        //{
        //    int iDaysOfMonth = aiTotalDaysOfMonth;
        //    SalaryEntityLists.lstStaffBaseDetails
        //                     .Where(user => user.UserId == aiUserId)
        //                     .ToList()
        //                     .ForEach
        //                        (
        //                            user =>
        //                            {
        //                                if (user.JoiningDate != DateTime.MinValue && user.ResignDate != DateTime.MinValue)
        //                                    iDaysOfMonth = user.ResignDate.Day - user.JoiningDate.Day + 1;
        //                                else
        //                                {
        //                                    if (user.JoiningDate != DateTime.MinValue)
        //                                        iDaysOfMonth = aiTotalDaysOfMonth - user.JoiningDate.Day + 1;
        //                                    else if (user.ResignDate != DateTime.MinValue)
        //                                        iDaysOfMonth = user.ResignDate.Day;
        //                                }
        //                            }
        //                        );
        //    return iDaysOfMonth;
        //}

        private decimal GetStaffHolidayLeaveDeductions(int aiUserId, int iDaysOfMonth)
        {
            decimal dcTotalAmount = 0;
            if (SalaryEntityLists.lstUsersSalaryDeductions.Count > 0)
            {
                List<UsersSalaryDeduction> UsersSalaryDeductions = SalaryEntityLists.lstUsersSalaryDeductions
                                                                    .Where(config => config.UserId == aiUserId)
                                                                    .ToList();
                UsersSalaryDeductions.ForEach
                    (
                        config => dcTotalAmount = dcTotalAmount + config.Days * (config.PercentageToDeduct / 100)
                    );
            }
            return Math.Round(dcTotalAmount, 2);
        }

        private void SetBasicDetails(UsersBasicDetails userDetails, int iRowIndex, int iSrNo)
        {
            moDTSalaryDetails.Rows[iRowIndex]["UserId"] = userDetails.UserId;
            moDTSalaryDetails.Rows[iRowIndex]["OriginalStaffGroupsId"] = userDetails.OriginalStaffGroupId;
            moDTSalaryDetails.Rows[iRowIndex]["SortOrder"] = 0;
            moDTSalaryDetails.Rows[iRowIndex]["Sr No"] = iSrNo;
            moDTSalaryDetails.Rows[iRowIndex]["Name"] = userDetails.Name;
            moDTSalaryDetails.Rows[iRowIndex]["Designation"] = userDetails.Designation;
            moDTSalaryDetails.Rows[iRowIndex]["DisplayControls"] = "Y";
            moDTSalaryDetails.Rows[iRowIndex]["TotalSortOrder"] = 0;
            moDTSalaryDetails.Rows[iRowIndex]["StaffGroupId"] = userDetails.StaffGroupId;

            moDTTempSalaryDetails.Rows[iRowIndex]["UserId"] = userDetails.UserId;
            moDTTempSalaryDetails.Rows[iRowIndex]["OriginalStaffGroupsId"] = userDetails.OriginalStaffGroupId;
            moDTTempSalaryDetails.Rows[iRowIndex]["SortOrder"] = 0;
            moDTTempSalaryDetails.Rows[iRowIndex]["Sr No"] = iSrNo;
            moDTTempSalaryDetails.Rows[iRowIndex]["Name"] = userDetails.Name;
            moDTTempSalaryDetails.Rows[iRowIndex]["Designation"] = userDetails.Designation;
            moDTTempSalaryDetails.Rows[iRowIndex]["DisplayControls"] = "Y";
            moDTTempSalaryDetails.Rows[iRowIndex]["TotalSortOrder"] = 0;
            moDTTempSalaryDetails.Rows[iRowIndex]["StaffGroupId"] = userDetails.StaffGroupId;
        }

        private void SetSummaryDetails(int aiRowIndex, int aiStaffGroupId, int aiRowCounter, ref int aiNetTotalRowIndex)
        {
            int iLoopCounter = 0;
            decimal iNetSum = 0;

            foreach (string Columns in olstTotalEarningsDeductions)
            {
                var SalaryDetailsSum = (from Salary in moDTSalaryDetails.AsEnumerable()
                                        where Salary["OriginalStaffGroupsId"] != DBNull.Value
                                           && (Columns == S_NET_SALARY || Columns == S_GROSS_SALARY || Columns == S_TOTAL_DEDUCTION || Salary[Columns].ToString() != "-1")
                                           && Salary["UserId"].ToString() != "-9999"
                                        group Salary by Salary["OriginalStaffGroupsId"] into EDSum
                                        select new
                                        {
                                            OriginalStaffGroupId = Convert.ToInt32(EDSum.Key),
                                            NetEDSum = Convert.ToDecimal(EDSum.Sum(p => Convert.ToDecimal(p[Columns])))
                                        });


                var StGroups = SalaryEntityLists.lstUsersDetails
                                                  .Join(SalaryEntityLists.lstStaffGroups, UserDetails => UserDetails.StaffGroupsId, StaffGroups => StaffGroups.StaffGroupsId, (UserDetails, StaffGroups) => new { StaffGroupsName = StaffGroups.StaffGroupsName, OriginalStaffGroupsId = StaffGroups.OriginalStaffGroupsId, StaffGroupsId = StaffGroups.StaffGroupsId })
                                                  .Distinct();


                decimal dcTotal;
                bool bIsStaffGroupAvailable = false;
                foreach (var StaffGroups in StGroups)
                {
                    List<decimal> SalaryDetails = SalaryDetailsSum.Where(SalarySum => SalarySum.OriginalStaffGroupId == StaffGroups.OriginalStaffGroupsId).Select(SalarySum => SalarySum.NetEDSum).ToList();

                    if (SalaryDetails.Count() > 0)
                    {
                        if (iLoopCounter == 0)
                            SetSummaryRowHeaderDetails(StaffGroups.StaffGroupsName.ToString(), Convert.ToInt32(StaffGroups.OriginalStaffGroupsId), aiRowIndex, StaffGroups.StaffGroupsId);

                        dcTotal = 0;
                        if (SalaryDetails != null && SalaryDetails.Count() > 0)
                            dcTotal = SalaryDetails.First();

                        moDTTempSalaryDetails.Rows[aiRowIndex][Columns] = dcTotal;
                        moDTSalaryDetails.Rows[aiRowIndex][Columns] = dcTotal;
                        iNetSum = iNetSum + dcTotal;
                        bIsStaffGroupAvailable = true;                        
                    }
                    aiRowIndex++;
                }

                if (bIsStaffGroupAvailable)
                {
                    if (iLoopCounter == 0 && aiStaffGroupId == 0)
                        SetNetSummaryRowDetails(aiRowIndex, ref aiNetTotalRowIndex);

                    if (aiStaffGroupId == 0)
                    {
                        moDTSalaryDetails.Rows[aiNetTotalRowIndex][Columns] = iNetSum;
                        moDTTempSalaryDetails.Rows[aiNetTotalRowIndex][Columns] = iNetSum;
                    }
                }

                iNetSum = 0;
                iLoopCounter++;
                aiRowIndex = aiRowCounter;
                bIsStaffGroupAvailable = true;
            }
        }

        private void SetSummaryRowHeaderDetails(string asStaffGroupsName, int aiOriginalStaffGroupsId, int iRowIndex, int aiStaffGroupId)
        {
            moDTSalaryDetails.Rows.Add();
            moDTSalaryDetails.Rows[iRowIndex]["Name"] = asStaffGroupsName + " Total";
            moDTSalaryDetails.Rows[iRowIndex]["OriginalStaffGroupsId"] = aiOriginalStaffGroupsId;
            moDTSalaryDetails.Rows[iRowIndex]["StaffGroupId"] = aiStaffGroupId;
            moDTSalaryDetails.Rows[iRowIndex]["UserId"] = -9999;
            moDTSalaryDetails.Rows[iRowIndex]["SortOrder"] = 1;

            moDTTempSalaryDetails.Rows.Add();
            moDTTempSalaryDetails.Rows[iRowIndex]["Name"] = asStaffGroupsName + " Total";
            moDTTempSalaryDetails.Rows[iRowIndex]["OriginalStaffGroupsId"] = aiOriginalStaffGroupsId;
            moDTTempSalaryDetails.Rows[iRowIndex]["StaffGroupId"] = aiStaffGroupId;
            moDTTempSalaryDetails.Rows[iRowIndex]["UserId"] = -9999;
            moDTTempSalaryDetails.Rows[iRowIndex]["SortOrder"] = 1;
        }

        private void SetNetSummaryRowDetails(int aiRowIndex, ref int aiNetTotalRowIndex)
        {
            moDTSalaryDetails.Rows.Add();
            aiNetTotalRowIndex = aiRowIndex;
            moDTSalaryDetails.Rows[aiRowIndex]["Name"] = "Net Total";
            moDTSalaryDetails.Rows[aiRowIndex]["UserId"] = -9999;
            moDTSalaryDetails.Rows[aiRowIndex]["OriginalStaffGroupsId"] = 9999;
            moDTSalaryDetails.Rows[aiRowIndex]["SortOrder"] = 1;


            moDTTempSalaryDetails.Rows.Add();
            moDTTempSalaryDetails.Rows[aiRowIndex]["Name"] = "Net Total";
            moDTTempSalaryDetails.Rows[aiRowIndex]["UserId"] = -9999;
            moDTTempSalaryDetails.Rows[aiRowIndex]["OriginalStaffGroupsId"] = 9999;
            moDTTempSalaryDetails.Rows[aiRowIndex]["SortOrder"] = 1;
        }

        private void SetDisplayOfSaveButton(int aiRowIndex, int aiUserId)
        {
            int LeaveCount = (from UsersLeaveBank in SalaryEntityLists.lstUserLeaveConfiguration.AsEnumerable()
                              where UsersLeaveBank.UserId == aiUserId
                              select UsersLeaveBank).Count();

            if (LeaveCount == 0)
            {
                moDTSalaryDetails.Rows[aiRowIndex]["DisplayControls"] = "N";
                moDTTempSalaryDetails.Rows[aiRowIndex]["DisplayControls"] = "N";
            }
        }

        private int SetEarningsDeductionsRange(int iRowIndex, int aiUserId, int iEarningsSum, ref int iDeductionSum, decimal dcTotalDays, int iDaysOfMonth, int aiMonthId, char acGender)
        {
            var EarningsDeductionsRange = from AmountRange in SalaryEntityLists.lstAmountRange.AsEnumerable()
                                          join EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                          on AmountRange.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                                          join StaffGroupsEearnDeductionAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                          on AmountRange.EarningsDeductionsId equals StaffGroupsEearnDeductionAsso.EarningsDeductionsId
                                          join UserStaffGroups in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                          on StaffGroupsEearnDeductionAsso.StaffGroupsId equals UserStaffGroups.StaffGroupsId
                                          join MonthwiseAmount in SalaryEntityLists.lstMonthwiseAmount.AsEnumerable()
                                          on AmountRange.AmountRangeId equals MonthwiseAmount.AmountRangeId
                                          where UserStaffGroups.UserId == aiUserId
                                                && AmountRange.FromAmount <= iEarningsSum
                                                && AmountRange.UptoAmount >= iEarningsSum
                                                && MonthwiseAmount.MonthId == aiMonthId
                                                && AmountRange.IsDefault == true
                                          select new
                                          {
                                              EarningsDeductionsId = AmountRange.EarningsDeductionsId,
                                              FromAmount = AmountRange.FromAmount,
                                              UptoAmount = AmountRange.UptoAmount,
                                              Amount = MonthwiseAmount.Amount,
                                              ShortName = EarnDeduction.ShortName,
                                              IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                                              IsEarning = EarnDeduction.IsEarning,
                                              HasFormula = EarnDeduction.HasFormula,
                                              AmountRangeID = AmountRange.AmountRangeId
                                          };

            int iAmount = 0;
            foreach (var EDRange in EarningsDeductionsRange)
            {
                List<AmountRange> UsersRangeED = (from UsersED in SalaryEntityLists.lstUsersFormulaAndRanges.AsEnumerable()
                                                  join Range in SalaryEntityLists.lstAmountRange.AsEnumerable()
                                                  on UsersED.FormulaRangeId equals Range.RangeId
                                                  join MonthwiseAmount in SalaryEntityLists.lstMonthwiseAmount.AsEnumerable()
                                                       on Range.AmountRangeId equals MonthwiseAmount.AmountRangeId
                                                  where Range.EarningsDeductionsId == EDRange.EarningsDeductionsId &&                                                      
                                                        Range.IsDefault == false &&
                                                        UsersED.UserId == aiUserId &&
                                                        UsersED.IsFormula == false &&
                                                        Range.FromAmount <= iEarningsSum
                                                        && Range.UptoAmount >= iEarningsSum
                                                        && MonthwiseAmount.MonthId == aiMonthId
                                                  select new AmountRange
                                                  {
                                                      FromAmount = Range.FromAmount,
                                                      UptoAmount = Range.UptoAmount,
                                                      Amount = MonthwiseAmount.Amount
                                                  }).ToList();


                if (UsersRangeED.Count() == 0)
                {
                    UsersRangeED = SalaryEntityLists.lstAmountRange
                                    .Join(SalaryEntityLists.lstMonthwiseAmount, Range => Range.AmountRangeId, MonthwiseAmount => MonthwiseAmount.AmountRangeId,
                                    (Range, MonthwiseAmount) => new { Range = Range, MonthwiseAmount = MonthwiseAmount })
                                    .Where(amountRange => amountRange.Range.EarningsDeductionsId == EDRange.EarningsDeductionsId &&
                                           amountRange.MonthwiseAmount.AmountRangeId == EDRange.AmountRangeID &&
                                           amountRange.MonthwiseAmount.MonthId == aiMonthId)
                                    .Select(amountRange =>
                                            new AmountRange
                                            {
                                                FromAmount = amountRange.Range.FromAmount,
                                                UptoAmount = amountRange.Range.UptoAmount,
                                                Amount = amountRange.MonthwiseAmount.Amount
                                            })
                                    .ToList();

                }

                if (UsersRangeED.Count() > 0)
                    iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(UsersRangeED.First().Amount)));
                else
                    iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(EDRange.Amount)));

                if (iAmount == 175 && acGender == 'F')
                    iAmount = 0;

                // This is to set 0 P.T. for PPS staff Rekha Mohan..This is temporary solution and need to remove nce made available this screen from screen.
                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && aiUserId == 3772)
                    iAmount = 0;

                moDTSalaryDetails.Rows[iRowIndex][EDRange.ShortName.ToString()] = iAmount;
                moDTTempSalaryDetails.Rows[iRowIndex][EDRange.ShortName.ToString()] = iAmount + S_EARNING_DEDUCTION_CONNECTOR + EDRange.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(EDRange.HasFormula);

                if (Convert.ToBoolean(EDRange.IsAttendanceDependent))
                {
                    moDTSalaryDetails.Rows[iRowIndex][EDRange.ShortName.ToString()] = Convert.ToInt32(Math.Round((dcTotalDays / iDaysOfMonth) * iAmount));
                    moDTTempSalaryDetails.Rows[iRowIndex][EDRange.ShortName.ToString()] = Convert.ToInt32(Math.Round((dcTotalDays / iDaysOfMonth) * iAmount)) + S_EARNING_DEDUCTION_CONNECTOR + EDRange.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(EDRange.HasFormula);
                }

                if (Convert.ToBoolean(EDRange.IsEarning))
                    iEarningsSum = iEarningsSum + iAmount;
                else
                    iDeductionSum = iDeductionSum + iAmount;
            }
            return iEarningsSum;
        }

        private void SetDefaultEDValuesIfNotAssociated(int iRowIndex, int aiUserId)
        {
            var AvailableED = from UsersEarnDeduction in SalaryEntityLists.lstUsersEarningsDeduction.AsEnumerable()
                              join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                              on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                              join EarnDeductions in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                              on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId
                              join SGED in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation
                              on UserSGAsso.StaffGroupsId equals SGED.StaffGroupsId
                              where UsersEarnDeduction.UserId == aiUserId
                                        && EarnDeductions.HasFormula == false
                                        && EarnDeductions.EarningsDeductionsId == SGED.EarningsDeductionsId
                              select new
                              {
                                  EarningsDeductionsId = UsersEarnDeduction.EarningsDeductionsId
                              };

            if (AvailableED.Count() == 0)
            {
                AvailableED = from StaffGroupEarnDeductAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                              join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                              on StaffGroupEarnDeductAsso.StaffGroupsId equals UserSGAsso.StaffGroupsId
                              where UserSGAsso.UserId == aiUserId
                              select new
                              {
                                  EarningsDeductionsId = StaffGroupEarnDeductAsso.EarningsDeductionsId
                              };
            }

            var EarningsDeductions = SalaryEntityLists.lstEarningsDeductions.Select(EarnDeduct => new { EarningsDeductionsId = EarnDeduct.EarningsDeductionsId }).ToList();

            var RemainingEarnDeduct1 = EarningsDeductions.Except(AvailableED);

            var EDToRem = from EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                          join RemED in RemainingEarnDeduct1.AsEnumerable()
                          on EarnDeduction.EarningsDeductionsId equals RemED.EarningsDeductionsId
                          select new
                          {
                              ShortName = Convert.ToString(EarnDeduction.ShortName),
                              Value = -1,
                              EarningsDeductionsId = EarnDeduction.EarningsDeductionsId,
                              HasFormula = EarnDeduction.HasFormula
                          };

            var AttendanceDependentED = from EarningsDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                        join RemED in RemainingEarnDeduct1.AsEnumerable()
                                        on EarningsDeduction.EarningsDeductionsId equals RemED.EarningsDeductionsId
                                        where EarningsDeduction.IsAttendanceDependent == true
                                        select new
                                        {
                                            ShortName = "Leave Deducted " + EarningsDeduction.ShortName,
                                            Value = -1,
                                            EarningsDeductionsId = EarningsDeduction.EarningsDeductionsId,
                                            HasFormula = EarningsDeduction.HasFormula
                                        };

            foreach (var EarnDeduction in EDToRem)
            {
                moDTSalaryDetails.Rows[iRowIndex][EarnDeduction.ShortName.ToString()] = EarnDeduction.Value;
                moDTTempSalaryDetails.Rows[iRowIndex][EarnDeduction.ShortName.ToString()] = EarnDeduction.Value + S_EARNING_DEDUCTION_CONNECTOR + EarnDeduction.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(EarnDeduction.HasFormula);
            }

            foreach (var EarnDeduction in AttendanceDependentED)
            {
                moDTSalaryDetails.Rows[iRowIndex][EarnDeduction.ShortName.ToString()] = EarnDeduction.Value;
                moDTTempSalaryDetails.Rows[iRowIndex][EarnDeduction.ShortName.ToString()] = EarnDeduction.Value + S_LEAVE_DEDUCTED_CONNECTOR;
            }
        }

        private void SetDefaultEDValuesIfNotAvail(int iRowIndex, int aiUserId)
        {

            var AssociatedED = from StaffGroupEarnDeductAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                               join EarnDeduct in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                               on StaffGroupEarnDeductAsso.EarningsDeductionsId equals EarnDeduct.EarningsDeductionsId
                               join UsersSG in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                               on StaffGroupEarnDeductAsso.StaffGroupsId equals UsersSG.StaffGroupsId
                               where UsersSG.UserId == aiUserId
                               select new
                               {
                                   ShortName = EarnDeduct.ShortName,
                                   EarningsDeductionsId = StaffGroupEarnDeductAsso.EarningsDeductionsId,
                                   Value = 0,
                                   HasFormula = EarnDeduct.HasFormula,
                                   IsAttendanceDependent = EarnDeduct.IsAttendanceDependent
                               };

            foreach (var StaffGroupEarnDeductAsso in AssociatedED)
            {
                moDTSalaryDetails.Rows[iRowIndex][StaffGroupEarnDeductAsso.ShortName.ToString()] = StaffGroupEarnDeductAsso.Value;
                moDTTempSalaryDetails.Rows[iRowIndex][StaffGroupEarnDeductAsso.ShortName.ToString()] = StaffGroupEarnDeductAsso.Value + S_EARNING_DEDUCTION_CONNECTOR + StaffGroupEarnDeductAsso.EarningsDeductionsId + S_CONNECTOR + Convert.ToInt32(StaffGroupEarnDeductAsso.HasFormula);
                if (Convert.ToBoolean(StaffGroupEarnDeductAsso.IsAttendanceDependent) == true)
                {
                    moDTSalaryDetails.Rows[iRowIndex]["Leave Deducted " + StaffGroupEarnDeductAsso.ShortName] = StaffGroupEarnDeductAsso.Value;
                    moDTTempSalaryDetails.Rows[iRowIndex]["Leave Deducted " + StaffGroupEarnDeductAsso.ShortName] = StaffGroupEarnDeductAsso.Value + S_LEAVE_DEDUCTED_CONNECTOR;
                }
            }
        }

        private decimal SetAttendanceDetails(int iRowIndex, decimal dcUnpaidLeaves, int aiUserId, int aiDaysOfMonth)
        {
            const string S_ATTENDANCE_CONNERCTOR = "_AT_";
            const string S_LB = "_LB";
            const string S_LEAVE_CONNECTOR = "_LV_";

            decimal dcLMLeaves;
            string sLateMarkLeave = GetLateMarkLeave(aiUserId, out dcLMLeaves);

            decimal dcTotalDays = 0;

            int AttendanceCount = SalaryEntityLists.lstStaffAttendance.Where(attendance => attendance.UserId == aiUserId).Count();

            decimal dcAttendance = 0;
            int iAttendanceId = 0;
            if (AttendanceCount > 0)
                SetAttendanceDetails(iRowIndex, dcUnpaidLeaves, aiUserId, S_LEAVE_CONNECTOR, ref dcTotalDays, ref dcAttendance, ref iAttendanceId);
            else
            {
                dcAttendance = 0;
                dcTotalDays = 0;
                iAttendanceId = 0;

                var Leaves = SalaryEntityLists.lstConfiguredLeaves.Select(leave => new { ShortName = leave.ShortName, Days = 0, LeaveId = leave.LeaveId });

                foreach (var leave in Leaves)
                {
                    moDTSalaryDetails.Rows[iRowIndex][leave.ShortName.ToString()] = leave.Days;
                    moDTTempSalaryDetails.Rows[iRowIndex][leave.ShortName.ToString()] = leave.Days + S_LEAVE_CONNECTOR + leave.LeaveId;
                }
            }

            SetLeaveBalance(iRowIndex, aiUserId, S_LB);

            decimal dcHolidayLeaves = GetStaffHolidayLeaveDeductions(aiUserId, aiDaysOfMonth);
            dcTotalDays = dcTotalDays - dcHolidayLeaves;

            moDTSalaryDetails.Rows[iRowIndex]["Attendance"] = dcAttendance;
            moDTSalaryDetails.Rows[iRowIndex]["Late Mark Leaves"] = dcLMLeaves;
            moDTSalaryDetails.Rows[iRowIndex][S_HOLIDAY_LEAVE] = dcHolidayLeaves;
            moDTSalaryDetails.Rows[iRowIndex]["Total"] = dcTotalDays;

            moDTTempSalaryDetails.Rows[iRowIndex]["Attendance"] = dcAttendance + S_ATTENDANCE_CONNERCTOR + iAttendanceId;
            moDTTempSalaryDetails.Rows[iRowIndex]["Late Mark Leaves"] = sLateMarkLeave;
            moDTTempSalaryDetails.Rows[iRowIndex][S_HOLIDAY_LEAVE] = dcHolidayLeaves;
            moDTTempSalaryDetails.Rows[iRowIndex]["Total"] = dcTotalDays;
            moDTSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = Convert.ToDecimal(moDTSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"]) + dcHolidayLeaves;
            moDTTempSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = Convert.ToDecimal(moDTTempSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"]) + dcHolidayLeaves;

            return dcTotalDays;
        }

        private void SetAttendanceDetails(int iRowIndex, decimal dcUnpaidLeaves, int aiUserId, string S_LEAVE_CONNECTOR, ref decimal dcTotalDays, ref decimal dcAttendance, ref int iAttendanceId)
        {
            StaffAttendance UsersAttendance = SalaryEntityLists.lstStaffAttendance.Where(attendance => attendance.UserId == aiUserId).First();

            //StaffAttendance

            dcAttendance = UsersAttendance.PresentDays;
            iAttendanceId = UsersAttendance.StaffAttendanceId;

            List<StaffLeaveDetails> StaffLeavesDetails = SalaryEntityLists.lstStaffLeaveDetails
                                                         .Join(SalaryEntityLists.lstConfiguredLeaves, StaffLeavesDtl => StaffLeavesDtl.LeaveId, Leave => Leave.LeaveId, (StaffLeavesDtl, Leave) => StaffLeavesDtl)
                                                         .Where(StaffLeavesDtl => StaffLeavesDtl.StaffAttendanceId == UsersAttendance.StaffAttendanceId)
                                                         .Select(StaffLeavesDtl => StaffLeavesDtl)
                                                         .ToList();

            if (StaffLeavesDetails.Count() > 0)
            {
                decimal dcTotalLeaves = 0;
                decimal dcLeaveDays;
                StaffLeavesDetails.ForEach
                (
                    StaffLeaves =>
                    {
                        dcLeaveDays = StaffLeaves.Days;
                        moDTSalaryDetails.Rows[iRowIndex][StaffLeaves.ShortName.ToString()] = dcLeaveDays;
                        moDTTempSalaryDetails.Rows[iRowIndex][StaffLeaves.ShortName.ToString()] = dcLeaveDays + S_LEAVE_CONNECTOR + StaffLeaves.LeaveId;
                        dcTotalLeaves = dcTotalLeaves + dcLeaveDays;
                    }
                 );
                dcTotalDays = Convert.ToDecimal(dcAttendance) + dcTotalLeaves + dcUnpaidLeaves; ;
            }
        }

        private void SetLeaveBalance(int iRowIndex, int aiUserId, string S_LB)
        {
            var LeaveBalanceEntries = from LeaveBalance in SalaryEntityLists.lstUserLeaveConfiguration.AsEnumerable()
                                      join Leave in SalaryEntityLists.lstConfiguredLeaves.AsEnumerable()
                                      on LeaveBalance.LeaveId equals Leave.LeaveId
                                      where LeaveBalance.UserId == aiUserId && Leave.IsUnpaidLeave == false
                                      select new
                                      {
                                          LeaveId = Leave.LeaveId,
                                          ShortName = Leave.ShortName,
                                          Balance = Convert.ToDecimal(LeaveBalance.LeaveBalance),
                                          OriginalLeaveBalance = Convert.ToDecimal(LeaveBalance.OriginalLeaveBalance)
                                      };

            List<UserLateMarkLeave> LateMarkLeaves = SalaryEntityLists.lstUserLateMarkLeaves
                                                                        .Where(lateMarkLeave => lateMarkLeave.UserId == aiUserId)
                                                                        .Select(lateMarkLeave => new UserLateMarkLeave { LeaveId = lateMarkLeave.LeaveId, Days = lateMarkLeave.Days })
                                                                        .ToList();

            decimal dcLateMarkLeaves;
            foreach (var BalanceEntry in LeaveBalanceEntries)
            {
                dcLateMarkLeaves = 0;
                if (SalaryEntityLists.lstUserLeaveConfiguration.Count > 0)
                    dcLateMarkLeaves = LateMarkLeaves.Where(leave => leave.LeaveId == BalanceEntry.LeaveId).Select(leave => leave.Days).FirstOrDefault();
                if (BalanceEntry.OriginalLeaveBalance < 0)
                {
                    moDTSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.OriginalLeaveBalance;
                    moDTTempSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.OriginalLeaveBalance + S_LB;
                }
                else
                {
                    moDTSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.Balance + Math.Round(dcLateMarkLeaves, 1);
                    moDTTempSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.Balance + Math.Round(dcLateMarkLeaves, 1) + S_LB;
                }
            }

            var UnconfigLeaves = (SalaryEntityLists.lstConfiguredLeaves.Where(Leave => !Leave.IsUnpaidLeave).Select(Leave => new { LeaveId = Leave.LeaveId, ShortName = Leave.ShortName, Balance = Convert.ToDecimal(0.0) })
                                .Except(LeaveBalanceEntries.Select(LeaveBalance => new { LeaveId = LeaveBalance.LeaveId, ShortName = LeaveBalance.ShortName, Balance = Convert.ToDecimal(0.0) })))
                                .Distinct();

            foreach (var BalanceEntry in UnconfigLeaves)
            {
                dcLateMarkLeaves = 0;
                if (SalaryEntityLists.lstUserLeaveConfiguration.Count > 0)
                    dcLateMarkLeaves = LateMarkLeaves.Where(leave => leave.LeaveId == BalanceEntry.LeaveId).Select(leave => leave.Days).FirstOrDefault();
                moDTSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.Balance + Math.Round(dcLateMarkLeaves, 1);
                moDTTempSalaryDetails.Rows[iRowIndex][BalanceEntry.ShortName + " Balance"] = BalanceEntry.Balance + Math.Round(dcLateMarkLeaves, 1) + S_LB;
            }
        }

        private string GetLateMarkLeave(int aiUserId, out decimal adcLateMarkLeaves)
        {
            string sLateMarkLeave = string.Empty;
            adcLateMarkLeaves = 0;
            decimal dcLateMarkLeaves = 0;
            if (SalaryEntityLists.lstLateMarkConfigurations != null)
            {
                var lateMarkLeaves = (from leave in SalaryEntityLists.lstConfiguredLeaves.AsEnumerable()
                                      join lateMark in SalaryEntityLists.lstUserLateMarkLeaves.AsEnumerable()
                                      on leave.LeaveId equals lateMark.LeaveId
                                      where leave.LeaveId == lateMark.LeaveId
                                      && lateMark.UserId == aiUserId
                                      select new { ShortName = leave.ShortName + "(" + Math.Round(lateMark.Days, 1) + ")", Days = Math.Round(lateMark.Days, 1) }).ToList();

                lateMarkLeaves.ForEach(lateMark => dcLateMarkLeaves = dcLateMarkLeaves + lateMark.Days);
                lateMarkLeaves.ForEach(lateMark => sLateMarkLeave = sLateMarkLeave + ", " + lateMark.ShortName);
            }
            if (sLateMarkLeave == string.Empty || sLateMarkLeave.Length == 2)
                sLateMarkLeave = "0";
            else
                sLateMarkLeave = sLateMarkLeave.Substring(2);

            adcLateMarkLeaves = dcLateMarkLeaves;
            return sLateMarkLeave;
        }

        private decimal GetUnpaidLeavesCount(int aiUserId, int iRowIndex)
        {
            decimal dcUnpaidLeaves = 0;
            int UnpaidLeavesCount = (from UsersLeaveBank in SalaryEntityLists.lstUserLeaveConfiguration.AsEnumerable()
                                     where UsersLeaveBank.UserId == aiUserId
                                     && UsersLeaveBank.LeaveBalance < 0
                                     select UsersLeaveBank).Count();

            if (UnpaidLeavesCount > 0)
            {
                var UnpaidLeaves = (from UsersLeaveBank in SalaryEntityLists.lstUserLeaveConfiguration.AsEnumerable()
                                    where UsersLeaveBank.UserId == aiUserId
                                            && UsersLeaveBank.LeaveBalance < 0
                                    group UsersLeaveBank by UsersLeaveBank.UserId into sumDays
                                    select new
                                    {
                                        sumDays.Key,
                                        TotalDays = sumDays.Sum(p => Convert.ToDecimal(p.LeaveBalance))
                                    }).First();

                moDTSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = UnpaidLeaves.TotalDays * -1;
                moDTTempSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = UnpaidLeaves.TotalDays * -1;
                dcUnpaidLeaves = Convert.ToDecimal(UnpaidLeaves.TotalDays);
            }
            else
            {
                moDTSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = 0.00;
                moDTTempSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = 0.00;
            }

            decimal dcUnpaidLeavesCount2 = 0;
            dcUnpaidLeavesCount2 = SalaryEntityLists.lstUserLateMarkLeaves
                                                      .Where(UsersLeaveBank => UsersLeaveBank.UserId == aiUserId && UsersLeaveBank.IsUnPaidLeave)
                                                      .Select(UsersLeaveBank => UsersLeaveBank.Days).FirstOrDefault();

            if (dcUnpaidLeavesCount2 != 0)
            {
                dcUnpaidLeaves = dcUnpaidLeaves - Math.Round(dcUnpaidLeavesCount2, 1);
                moDTSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = dcUnpaidLeaves * -1;
                moDTTempSalaryDetails.Rows[iRowIndex]["Unpaid Leaves"] = dcUnpaidLeaves * -1;
            }

            return dcUnpaidLeaves;
        }

        private void AddEarningDeductionsColumns()
        {
            EarningsDeductionsBL oEarningsDeductionsBL = new EarningsDeductionsBL();
            //Earnings - Deductions
            {
                List<EarningsDeductions> olstEarningsDeductions = SalaryEntityLists.lstEarningsDeductions.Where(earnDeduct => earnDeduct.IsEarning).ToList();
                AddEarningDeductionColumns(olstEarningsDeductions, moDTSalaryDetails, olstTotalEarningsDeductions, moDTTempSalaryDetails);

                if (BasicDetails.DisplaySalaryDifference)
                {
                    moDTSalaryDetails.Columns.Add(S_SALARY_DIFFERENCE);
                    moDTTempSalaryDetails.Columns.Add(S_SALARY_DIFFERENCE);
                    olstTotalEarningsDeductions.Add(S_SALARY_DIFFERENCE);
                }

                moDTSalaryDetails.Columns.Add(S_GROSS_SALARY);
                moDTTempSalaryDetails.Columns.Add(S_GROSS_SALARY);
                olstTotalEarningsDeductions.Add(S_GROSS_SALARY);
            }
            {
                List<EarningsDeductions> olstEarningsDeductions = SalaryEntityLists.lstEarningsDeductions.Where(earnDeduct => !earnDeduct.IsEarning).ToList();
                AddEarningDeductionColumns(olstEarningsDeductions, moDTSalaryDetails, olstTotalEarningsDeductions, moDTTempSalaryDetails);

                if (BasicDetails.DisplaySalaryDifference)
                {  
                    moDTSalaryDetails.Columns.Add(S_SALARY_DIFFERENCE_PF);
                    moDTTempSalaryDetails.Columns.Add(S_SALARY_DIFFERENCE_PF);
                    olstTotalEarningsDeductions.Add(S_SALARY_DIFFERENCE_PF);
                }

                moDTSalaryDetails.AddColumns(new string[]{ S_TOTAL_DEDUCTION, S_NET_SALARY });
                moDTTempSalaryDetails.AddColumns(new string[] { S_TOTAL_DEDUCTION, S_NET_SALARY });
                
                olstTotalEarningsDeductions.Add(S_TOTAL_DEDUCTION);
                olstTotalEarningsDeductions.Add(S_NET_SALARY);
            }
        }

        /// <summary>
        /// This method is used to add earning deduction columns.
        /// </summary>
        /// <param name="aoEarningsDeductions"></param>
        public void AddEarningDeductionColumns(List<EarningsDeductions> aolstEarningsDeductions, DataTable aoDTSalaryDetails, List<string> aolstTotalEarningsDeductions, DataTable aoDTTempSalaryDetails)
        {
            aolstEarningsDeductions.ForEach
                (
                    earningDeduction =>
                    {
                        aoDTSalaryDetails.Columns.Add(earningDeduction.ShortName);
                        aolstTotalEarningsDeductions.Add(earningDeduction.ShortName);
                        if (aoDTTempSalaryDetails != null)
                            aoDTTempSalaryDetails.Columns.Add(earningDeduction.ShortName);
                        if (Convert.ToBoolean(earningDeduction.IsAttendanceDependent) == true)
                        {
                            aoDTSalaryDetails.Columns.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                            aolstTotalEarningsDeductions.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                            if (aoDTTempSalaryDetails != null)
                                aoDTTempSalaryDetails.Columns.Add(String.Format("Leave Deducted {0}", earningDeduction.ShortName));
                        }
                    }
                );
        }

        private void AddBasicColumns()
        {
            moDTSalaryDetails.AddColumns(new string[] { "Sr No", "DisplayControls", "SortOrder", "TotalSortOrder", "UserId", "OriginalStaffGroupsId", "StaffGroupId", "Name", "Designation", "Attendance" });
            moDTTempSalaryDetails.AddColumns(new string[] { "Sr No", "DisplayControls", "SortOrder", "TotalSortOrder", "UserId", "OriginalStaffGroupsId", "StaffGroupId", "Name", "Designation", "Attendance" });
        }

        private void AddAttendanceLeavesColumns()
        {
            //Leaves           
            SalaryEntityLists.lstConfiguredLeaves.ForEach
            (
                leave =>
                {
                    if (leave.IsUnpaidLeave == false)
                    {
                        moDTSalaryDetails.Columns.Add(leave.ShortName + " Balance");
                        moDTTempSalaryDetails.Columns.Add(leave.ShortName + " Balance");
                        olstTotalEarningsDeductions.Add(leave.ShortName + " Balance");
                    }
                    moDTSalaryDetails.Columns.Add(leave.ShortName);
                    moDTTempSalaryDetails.Columns.Add(leave.ShortName);
                    olstTotalEarningsDeductions.Add(leave.ShortName);
                }
            );

            //Leave Total

            moDTSalaryDetails.AddColumns(new string[] { "Unpaid Leaves", "Late Mark Leaves", S_HOLIDAY_LEAVE, "Total" });
            moDTTempSalaryDetails.AddColumns(new string[] { "Unpaid Leaves", "Late Mark Leaves", S_HOLIDAY_LEAVE, "Total" });
            olstTotalEarningsDeductions.AddRange(new string[] { "Unpaid Leaves", "Late Mark Leaves", "Total", "Attendance", S_HOLIDAY_LEAVE });
        }

        #endregion

        /// <summary>
        /// This method is used to set salary details.
        /// </summary>
        /// <param name="aoDictionary"></param>
        public void SetSalaryDetails(Dictionary<string, string> aoDictionary)
        {
            string sValue = string.Empty;
            string sColumnName = string.Empty;
            string sIsEarningDeduction;
            string sName;
            string sDesignation;
            int iUserId;

            DataTable oDTSalryDetails;
            List<string> lstFields;

            DataTable oDataTable = GetSalaryDataTable(aoDictionary);
            GetTableColumns(out oDTSalryDetails, out lstFields);

            for (int iRowIndex = 0; iRowIndex < oDataTable.Rows.Count; iRowIndex++)
            {
                sIsEarningDeduction = Constants.S_ZERO;
                sName = oDataTable.Rows[iRowIndex]["Name"].ToString();
                iUserId = oDataTable.Rows[iRowIndex]["UserId"].ToInt();
                sDesignation = oDataTable.Rows[iRowIndex]["Designation"].ToString();

                for (int iColumnIndex = 0; iColumnIndex < oDataTable.Columns.Count; iColumnIndex++)
                {
                    sColumnName = (oDataTable.Columns[iColumnIndex] as DataColumn).ColumnName;
                    if (lstFields.FindAll(fl => fl == sColumnName).Count == 0)
                    {
                        sValue = oDataTable.Rows[iRowIndex][iColumnIndex].ToString();
                        DataRow oDataRow = oDTSalryDetails.NewRow();
                        oDataRow["SerialNo"] = (iRowIndex + 1);
                        oDataRow["UserId"] = iUserId;
                        oDataRow["Name"] = sName;
                        oDataRow["SortOrder"] = iColumnIndex;
                        oDataRow["Designation"] = sDesignation;
                        oDataRow["FieldName"] = sColumnName.Replace("_", " ");

                        if (sValue.Contains("_ED_"))
                            sIsEarningDeduction = Constants.S_ONE;

                        if (iUserId == -9999 && iColumnIndex >= oDataTable.Columns.IndexOf(PayrollConstants.S_TOTAL) && aoDictionary["ShowAllDetails"].Trim() == Constants.S_ZERO)
                            sIsEarningDeduction = "2";

                        oDataRow["IsEarningDeduction"] = sIsEarningDeduction;

                        if (sValue.IndexOf('_') > 0)
                            sValue = sValue.Substring(0, sValue.IndexOf('_'));
                        oDataRow["Value"] = sValue;
                        if (sValue == "-1")
                            oDataRow["Value"] = string.Empty;

                        oDTSalryDetails.Rows.Add(oDataRow);
                    }
                }

                DataRow oEmptyDatarow = oDTSalryDetails.NewRow();
                oEmptyDatarow["SerialNo"] = (iRowIndex + 1);
                oEmptyDatarow["UserId"] = iUserId;
                oEmptyDatarow["Name"] = sName;
                oEmptyDatarow["SortOrder"] = oDataTable.Columns.Count;
                oEmptyDatarow["Designation"] = sDesignation;
                oEmptyDatarow["FieldName"] = "Employee Sign";
                oEmptyDatarow["Value"] = string.Empty;
                oEmptyDatarow["IsEarningDeduction"] = aoDictionary["ShowAllDetails"].Trim() == Constants.S_ZERO ? 2 : 0;
                oDTSalryDetails.Rows.Add(oEmptyDatarow);

            }

            if (aoDictionary["ShowAllDetails"].Trim() == Constants.S_ZERO)
            {
                for (int iRowIndex = 0; iRowIndex < oDTSalryDetails.Rows.Count; iRowIndex++)
                {
                    DataRow oDataRow = oDTSalryDetails.Rows[iRowIndex];

                    if (oDataRow["IsEarningDeduction"].ToString() == Constants.S_ZERO)
                    {
                        if (oDataRow["FieldName"].ToString() != PayrollConstants.S_TOTAL)
                        {
                            oDataRow.Delete();
                            oDTSalryDetails.AcceptChanges();
                            iRowIndex--;
                        }
                        else
                        {
                            oDataRow["FieldName"] = "Attendance Total";
                            oDTSalryDetails.AcceptChanges();
                        }
                    }
                    else if (oDataRow["FieldName"].ToString().Contains("Leave Deducted"))
                    {
                        oDataRow = oDTSalryDetails.Rows[iRowIndex - 1];
                        oDataRow.Delete();
                        oDTSalryDetails.AcceptChanges();
                        iRowIndex--;
                    }
                    else if (oDataRow["IsEarningDeduction"].ToString() == "2")
                    {
                        if (oDataRow["FieldName"].ToString() == PayrollConstants.S_TOTAL)
                            oDataRow["FieldName"] = "Attendance Total";
                        oDataRow["IsEarningDeduction"] = Constants.S_ZERO;
                        oDTSalryDetails.AcceptChanges();
                    }
                }
            }

            string sXml;
            using (StringWriter sw = new StringWriter())
            {
                oDTSalryDetails.TableName = "SalaryDetails";
                oDTSalryDetails.WriteXml(sw);
                sXml = sw.ToString();
            }

            SalaryDetailsDC.SetSalaryDetails(sXml);
        }

        /// <summary>
        /// This method is used to add columns into table and list.
        /// </summary>
        /// <param name="oDTSalryDetails"></param>
        /// <param name="lstFields"></param>
        private static void GetTableColumns(out DataTable oDTSalryDetails, out List<string> lstFields)
        {
            oDTSalryDetails = new DataTable();
            oDTSalryDetails.AddColumns(new string[] { "SerialNo", "UserId", "Name", "Designation", "FieldName", "Value", "SortOrder", "IsEarningDeduction" });

            lstFields = new List<string>();
            lstFields.AddRange(new[] { "DisplayControls", "SortOrder", "TotalSortOrder", "OriginalStaffGroupsId", "StaffGroupId", "UserId", "Name", "Designation", "Sr No", "Sr_No" });
        }

        /// <summary>
        /// This method is used to return salary datatables.
        /// </summary>
        /// <param name="kvp"></param>
        /// <returns></returns>
        private DataTable GetSalaryDataTable(Dictionary<string, string> aoDictionary)
        {
            int aiMonthId = Convert.ToInt32(aoDictionary["MonthId"]);
            int aiYear = Convert.ToInt32(aoDictionary["Year"]);
            int aiStaffGroupId = Convert.ToInt32(aoDictionary["StaffGroupsId"]);
            int aiUserId = Convert.ToInt32(aoDictionary["UserId"]);

            DataSet oDataSet = GetSalaryDetailsDataset(aiMonthId, aiYear, aiStaffGroupId, string.Empty, 0, 1000, true, true);
            DataTable oDataTable = oDataSet.Tables[0];

            if (oDataTable.IsNonEmpty())
            {
                if (oDataTable.Columns.Count == 1)
                {
                    oDataTable = GetPaidSalaryTable(oDataTable);

                    if (oDataTable.IsNonEmpty())
                        oDataTable = GetPaidSalary(aiStaffGroupId, 0, 1000, oDataTable, aiUserId);
                }
                else
                    oDataTable = GetFilteredUnPaidSalary(aiStaffGroupId, aiUserId, oDataTable);
            }
            return oDataTable;
        }

        /// <summary>
        /// This method is used to return filtered paid salary.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="oDataTable"></param>
        /// <returns></returns>
        private static DataTable GetFilteredUnPaidSalary(int aiStaffGroupId, int aiUserId, DataTable oDataTable)
        {
            IEnumerable<DataRow> SortedSalaryDetails2;
            if (aiStaffGroupId != 0)
            {
                if (aiUserId == 0)
                {
                    SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                           where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                           select SalDetails;
                }
                else
                {
                    SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                           where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                            && SalDetails.Field<string>("UserId") == aiUserId.ToString()
                                           select SalDetails;
                }
            }
            else
            {
                SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                       select SalDetails;
            }

            oDataTable = SortedSalaryDetails2.CopyToDataTable();

            SortedSalaryDetails2 = ((from SalDetails in oDataTable.AsEnumerable()
                                     where SalDetails.Field<string>("UserId") != "-9999"
                                     select SalDetails)
                                   .Union
                                   (
                                    from SalDetails in oDataTable.AsEnumerable()
                                    where SalDetails.Field<string>("UserId") == "-9999"
                                    select SalDetails
                                   ));

            oDataTable = SortedSalaryDetails2.CopyToDataTable();
            return oDataTable;
        }

        /// <summary>
        /// This method is used to return paid salary details.
        /// </summary>
        /// <param name="oDTSalaryDetails"></param>
        /// <returns></returns>
        private DataTable GetPaidSalaryTable(DataTable oDTSalaryDetails)
        {
            string sXml = oDTSalaryDetails.Rows[0][0].ToString();
            sXml = sXml.Replace("<SalaryDetails>", "");
            sXml = sXml.Replace("</SalaryDetails>", "");
            sXml = sXml.Replace("<SalaryDetails ", "<SalaryDetailsXml ");

            DataSet oDataSet = new DataSet();
            using (System.IO.StringReader oReader = new System.IO.StringReader(sXml))
                oDataSet.ReadXml(oReader);

            DataTable oDataTable = oDataSet.Tables[0];
            return oDataTable;
        }

        /// <summary>
        /// This method isused to return paid salary.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="oDataTable"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        private DataTable GetPaidSalary(int aiStaffGroupId, int iStartIndex, int iEndIndex, DataTable oDataTable, int aiUserId)
        {
            IEnumerable<DataRow> SortedSalaryDetails2;
            if (aiStaffGroupId != 0)
            {
                if (aiUserId == 0)
                {
                    SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                           where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                           select SalDetails;
                }
                else
                {
                    SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                           where Convert.ToString(SalDetails.Field<string>("StaffGroupId")) == aiStaffGroupId.ToString()
                                            && SalDetails.Field<string>("UserId") == aiUserId.ToString()
                                           select SalDetails;
                }
            }
            else
            {
                SortedSalaryDetails2 = from SalDetails in oDataTable.AsEnumerable()
                                       select SalDetails;
            }

            int iTotalRows = 0;
            foreach (DataRow dr in SortedSalaryDetails2)
                dr["Sr_No"] = ++iTotalRows;

            if (iTotalRows != 0)
            {
                SortedSalaryDetails2 = from SalDetails in SortedSalaryDetails2.CopyToDataTable().AsEnumerable()
                                       where Convert.ToInt32(SalDetails.Field<string>("Sr_No")) > iStartIndex && Convert.ToInt32(SalDetails.Field<string>("Sr_No")) <= iEndIndex
                                       select SalDetails;

                bool bIsFound = false;
                foreach (DataRow dr in SortedSalaryDetails2)
                {
                    bIsFound = true;
                    break;
                }

                oDataTable = bIsFound ? SortedSalaryDetails2.CopyToDataTable() : new DataTable();
            }
            else
                oDataTable = new DataTable();
            return oDataTable;
        }

        /// <summary>
        /// This method is used to return salary structure of a user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<UsersEarningsDeduction> GetSalaryStructureOfUser(int aiUserId)
        {
            Dictionary<int, int> EdFormulaValues = new Dictionary<int, int>();

            moSalaryDetailsDC.GetSalaryStructureOfUser(aiUserId);
            List<UsersEarnDeductDetails> UsersEarningsDeductions = GetUsersEDDetails(aiUserId);

            if (UsersEarningsDeductions.Count > 0)
            {
                int EDValue = 0;                
                foreach (UsersEarnDeductDetails UsersED in UsersEarningsDeductions)
                {
                    EDValue = Convert.ToInt32(Math.Round(Convert.ToDecimal(UsersED.EarningsDeductionsValue)));
                    EdFormulaValues[UsersED.EarningsDeductionsId] = EDValue;
                }
            }
            else
                SetDefaultEarnDeducts(aiUserId, EdFormulaValues);

            SetDefaultValueForEDs(aiUserId, EdFormulaValues);

            var EarningsDeductionsFormulae = from EDFormula in SalaryEntityLists.lstEarningsDeductionsFormulae.AsEnumerable()
                                             join EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                             on EDFormula.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                                             join SGEDAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                             on EDFormula.EarningsDeductionsId equals SGEDAsso.EarningsDeductionsId
                                             join UserSG in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                             on SGEDAsso.StaffGroupsId equals UserSG.StaffGroupsId
                                             where UserSG.UserId == aiUserId &&
                                                    EDFormula.IsDefault == true
                                             orderby EarnDeduction.OriginalEarningsDeductionsId ascending
                                             select new
                                             {
                                                 EarningsDeductionsId = EDFormula.EarningsDeductionsId,
                                                 Formula = EDFormula.Formula,
                                                 ShortName = EarnDeduction.ShortName,
                                                 IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                                                 IsEarning = EarnDeduction.IsEarning,
                                                 HasFormula = EarnDeduction.HasFormula,
                                                 FormulaId = EDFormula.FormulaId
                                             };

            MathsExpressionParser oMathsExpressionParser = new MathsExpressionParser();
            int iEDFormulaValue = 0;

            int iDaysOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            List<UsersEarnDeductDetails> UsersEDForFormula = UsersEarningsDeductions;
            Dictionary<int, decimal> dictEDValue = new Dictionary<int, decimal>();
            foreach (var EDFormula in EarningsDeductionsFormulae)
            {
                List<string> UsersFormulaED = SalaryEntityLists.lstUsersFormulaAndRanges
                                              .Join(SalaryEntityLists.lstEarningsDeductionsFormulae, UsersED => UsersED.FormulaRangeId, Formula => Formula.FormulaId, (UsersED, Formula) => new { UsersED = UsersED, Formula = Formula })
                                              .Where(User => User.Formula.EarningsDeductionsId == EDFormula.EarningsDeductionsId && User.UsersED.UserId == aiUserId && User.UsersED.IsFormula)
                                              .Select(user => user.Formula.Formula).ToList();

                string sFormula = string.Empty;
                string sTempFormula = string.Empty;

                if (UsersFormulaED.Count() > 0)
                    sFormula = UsersFormulaED.First();
                else
                    sFormula = EDFormula.Formula.ToString();
                sFormula = sFormula.Replace(",", "");
                sFormula = sFormula.Replace("%", "/100");

                sTempFormula = sFormula;
                UsersEDForFormula = UsersEarningsDeductions.ToList();
                decimal iTempEDValue = 0;

                DesignFormula(iDaysOfMonth, iDaysOfMonth, ref iEDFormulaValue, UsersEDForFormula, dictEDValue, ref sFormula, ref sTempFormula, ref iTempEDValue);

                List<int> UsersEarnDeduct = SalaryEntityLists.lstUsersEarningsDeduction.Where(UsersEarnDeduction => UsersEarnDeduction.UserId == aiUserId)
                                                               .Select(UsersEarnDeduction => UsersEarnDeduction.EarningsDeductionsId).ToList();

                List<int> STEDAssociation = SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation
                                                                .Select(SGEDAsso => SGEDAsso.EarningsDeductionsId).ToList();

                List<int> EDs = SalaryEntityLists.lstEarningsDeductions.Select(EarnDeduct => EarnDeduct.EarningsDeductionsId).ToList();

                List<int> RemAsso = EDs.Except(STEDAssociation).ToList();
                List<int> RemUsersEDs = EDs.Except(UsersEarnDeduct).ToList();
                List<int> TotalEDs = RemUsersEDs.Union(RemAsso).ToList();

                foreach (int ED in EDs)
                {
                    sFormula = sFormula.Replace("'" + ED + "'", "0");
                    sTempFormula = sTempFormula.Replace("'" + ED + "'", "0");
                }

                sFormula = sFormula.Replace("'", "");
                sTempFormula = sTempFormula.Replace("'", "");

                int valueOfED = 0;
                if (oMathsExpressionParser.Evaluate(sFormula))
                {
                    int iEDValue = Convert.ToInt32(Math.Round(oMathsExpressionParser.Result));

                    oMathsExpressionParser.Evaluate(sTempFormula);
                    int iEDTempFormula = Convert.ToInt32(Math.Round(oMathsExpressionParser.Result));

                    dictEDValue.Add(EDFormula.EarningsDeductionsId, iEDTempFormula);

                    EdFormulaValues[EDFormula.EarningsDeductionsId] = iEDTempFormula;                   
                }

                int iEDId = Convert.ToInt32(EDFormula.EarningsDeductionsId);

                List<UsersEarnDeductDetails> EDAppend = GetUsersEarningDeductionDetails(valueOfED, iEDId);

                UsersEarningsDeductions = UsersEDForFormula.Union(EDAppend).ToList();
            }

            int iEarningsSum = GetEarningTotal(EdFormulaValues);

            CalculateAmountRange(aiUserId, EdFormulaValues, iEarningsSum);

            int iDeductionSum = GetDeductionTotal(EdFormulaValues);

            List<UsersEarningsDeduction> lstUsersEarningsDeduction = new List<UsersEarningsDeduction>();

            var earningDeductions = (from ed in SalaryEntityLists.lstEarningsDeductions
                                     join sged in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation
                                     on ed.EarningsDeductionsId equals sged.EarningsDeductionsId
                                     join usga in SalaryEntityLists.lstUsersSGAssociation
                                     on sged.StaffGroupsId equals usga.StaffGroupsId
                                     where usga.UserId == aiUserId
                                     select ed).ToList();

            bool bIsChanged = false;
            earningDeductions.OrderByDescending(ed => ed.IsEarning).OrderBy(ed => ed.OriginalEarningsDeductionsId).ToList().ForEach(ed => 
            {
                if (!ed.IsEarning && !bIsChanged)                
                {
                    lstUsersEarningsDeduction.Add(PopulateUsersED(-999, iEarningsSum, true, "Total Earnings"));
                    lstUsersEarningsDeduction.Add(PopulateUsersED(-998, 0, true, string.Empty));
                    bIsChanged = true;
                }
                
                lstUsersEarningsDeduction.Add(PopulateUsersED(ed.EarningsDeductionsId, EdFormulaValues.ContainsKey(ed.EarningsDeductionsId) ? EdFormulaValues[ed.EarningsDeductionsId] : 0, ed.IsEarning, ed.ShortName));               
            });

            lstUsersEarningsDeduction.Add(PopulateUsersED(-997, iDeductionSum, false, "Total Deductions"));
            lstUsersEarningsDeduction.Add(PopulateUsersED(-996, 0, true, string.Empty));
            lstUsersEarningsDeduction.Add(PopulateUsersED(-995, (iEarningsSum - iDeductionSum), false, "Net Salary"));

            return lstUsersEarningsDeduction;
        }

        /// <summary>
        /// This method is used to set default Eds.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="EdFormulaValues"></param>
        private void SetDefaultValueForEDs(int aiUserId, Dictionary<int, int> aoEdFormulaValues)
        {
            var AvailableED = from UsersEarnDeduction in SalaryEntityLists.lstUsersEarningsDeduction.AsEnumerable()
                              join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                              on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                              join EarnDeductions in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                              on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId
                              join SGED in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation
                              on UserSGAsso.StaffGroupsId equals SGED.StaffGroupsId
                              where UsersEarnDeduction.UserId == aiUserId
                                        && EarnDeductions.HasFormula == false
                                        && EarnDeductions.EarningsDeductionsId == SGED.EarningsDeductionsId
                              select new
                              {
                                  UsersEarnDeduction.EarningsDeductionsId
                              };

            if (AvailableED.Count() == 0)
            {
                AvailableED = from StaffGroupEarnDeductAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                              join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                              on StaffGroupEarnDeductAsso.StaffGroupsId equals UserSGAsso.StaffGroupsId
                              where UserSGAsso.UserId == aiUserId
                              select new
                              {
                                  StaffGroupEarnDeductAsso.EarningsDeductionsId
                              };
            }

            var EarningsDeductions = SalaryEntityLists.lstEarningsDeductions.Select(EarnDeduct => new { EarningsDeductionsId = EarnDeduct.EarningsDeductionsId }).ToList();

            var RemainingEarnDeduct1 = EarningsDeductions.Except(AvailableED);

            var EDToRem = from EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                          join RemED in RemainingEarnDeduct1.AsEnumerable()
                          on EarnDeduction.EarningsDeductionsId equals RemED.EarningsDeductionsId
                          select new
                          {
                              ShortName = Convert.ToString(EarnDeduction.ShortName),
                              Value = -1,
                              EarningsDeductionsId = EarnDeduction.EarningsDeductionsId,
                              HasFormula = EarnDeduction.HasFormula
                          };

            foreach (var EarnDeduction in EDToRem)
                aoEdFormulaValues[EarnDeduction.EarningsDeductionsId] = EarnDeduction.Value;
        }

        /// <summary>
        /// This method is used to set default EDs.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="EdFormulaValues"></param>
        private void SetDefaultEarnDeducts(int aiUserId, Dictionary<int, int> aoEdFormulaValues)
        {
            var AssociatedED = from StaffGroupEarnDeductAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                               join EarnDeduct in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                               on StaffGroupEarnDeductAsso.EarningsDeductionsId equals EarnDeduct.EarningsDeductionsId
                               join UsersSG in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                               on StaffGroupEarnDeductAsso.StaffGroupsId equals UsersSG.StaffGroupsId
                               where UsersSG.UserId == aiUserId
                               select new
                               {
                                   ShortName = EarnDeduct.ShortName,
                                   EarningsDeductionsId = StaffGroupEarnDeductAsso.EarningsDeductionsId,
                                   Value = 0,
                                   HasFormula = EarnDeduct.HasFormula,
                                   IsAttendanceDependent = EarnDeduct.IsAttendanceDependent
                               };

            foreach (var StaffGroupEarnDeductAsso in AssociatedED)
                aoEdFormulaValues[StaffGroupEarnDeductAsso.EarningsDeductionsId] = StaffGroupEarnDeductAsso.Value;
        }

        /// <summary>
        /// This method is used to return user's EDs.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        private List<UsersEarnDeductDetails> GetUsersEDDetails(int aiUserId)
        {
            List<UsersEarnDeductDetails> UsersEarningsDeductions = (from UsersEarnDeduction in SalaryEntityLists.lstUsersEarningsDeduction.AsEnumerable()
                                                                    join UserSGAsso in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                                                    on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                                                                    join EarnDeductions in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                                                    on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId

                                                                    join SGEDAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                                                    on UserSGAsso.StaffGroupsId equals SGEDAsso.StaffGroupsId

                                                                    where UsersEarnDeduction.UserId == aiUserId
                                                                      && EarnDeductions.HasFormula == false
                                                                      && EarnDeductions.EarningsDeductionsId == SGEDAsso.EarningsDeductionsId

                                                                    select new UsersEarnDeductDetails
                                                                    {
                                                                        EarningsDeductionsId = UsersEarnDeduction.EarningsDeductionsId,
                                                                        ShortName = EarnDeductions.ShortName,
                                                                        EarningsDeductionsValue = UsersEarnDeduction.EarningsDeductionsValue,
                                                                        IsAttendanceDependent = EarnDeductions.IsAttendanceDependent,
                                                                        IsEarning = EarnDeductions.IsEarning,
                                                                        HasFormula = EarnDeductions.HasFormula
                                                                    }).ToList();
            return UsersEarningsDeductions;
        }

        /// <summary>
        /// This method is used to return earnings total.
        /// </summary>
        /// <param name="aoEdFormulaValues"></param>
        /// <returns></returns>
        private int GetEarningTotal(Dictionary<int, int> aoEdFormulaValues)
        {
            var EDSum = (from ed in SalaryEntityLists.lstEarningsDeductions
                         join kvp in aoEdFormulaValues.AsEnumerable()
                         on ed.EarningsDeductionsId equals kvp.Key
                         where kvp.Value != -1 && ed.IsEarning
                         group kvp by ed.IsEarning into edValue
                         select new
                         {
                             IsEarning = edValue.Key,
                             TotalAmount = edValue.Sum(ed => ed.Value)
                         }
                                ).FirstOrDefault();

            int iEarningsSum = EDSum.TotalAmount;
            return iEarningsSum;
        }

        /// <summary>
        /// This method is used to calculate amount range amount.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aoEdFormulaValues"></param>
        /// <param name="aiEarningsSum"></param>
        private void CalculateAmountRange(int aiUserId, Dictionary<int, int> aoEdFormulaValues, int aiEarningsSum)
        {
            var EarningsDeductionsRange = from AmountRange in SalaryEntityLists.lstAmountRange.AsEnumerable()
                                          join EarnDeduction in SalaryEntityLists.lstEarningsDeductions.AsEnumerable()
                                          on AmountRange.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                                          join StaffGroupsEearnDeductionAsso in SalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.AsEnumerable()
                                          on AmountRange.EarningsDeductionsId equals StaffGroupsEearnDeductionAsso.EarningsDeductionsId
                                          join UserStaffGroups in SalaryEntityLists.lstUsersSGAssociation.AsEnumerable()
                                          on StaffGroupsEearnDeductionAsso.StaffGroupsId equals UserStaffGroups.StaffGroupsId
                                          join MonthwiseAmount in SalaryEntityLists.lstMonthwiseAmount.AsEnumerable()
                                          on AmountRange.AmountRangeId equals MonthwiseAmount.AmountRangeId
                                          where UserStaffGroups.UserId == aiUserId
                                                && AmountRange.FromAmount <= aiEarningsSum
                                                && AmountRange.UptoAmount >= aiEarningsSum
                                                && MonthwiseAmount.MonthId == DateTime.Now.Month
                                                && AmountRange.IsDefault == true
                                          select new
                                          {
                                              EarningsDeductionsId = AmountRange.EarningsDeductionsId,
                                              FromAmount = AmountRange.FromAmount,
                                              UptoAmount = AmountRange.UptoAmount,
                                              Amount = MonthwiseAmount.Amount,
                                              ShortName = EarnDeduction.ShortName,
                                              IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                                              IsEarning = EarnDeduction.IsEarning,
                                              HasFormula = EarnDeduction.HasFormula,
                                              AmountRangeID = AmountRange.AmountRangeId
                                          };

            int iAmount = 0;
            foreach (var EDRange in EarningsDeductionsRange)
            {
                List<AmountRange> UsersRangeED = (from UsersED in SalaryEntityLists.lstUsersFormulaAndRanges.AsEnumerable()
                                                  join Range in SalaryEntityLists.lstAmountRange.AsEnumerable()
                                                  on UsersED.FormulaRangeId equals Range.RangeId
                                                  join MonthwiseAmount in SalaryEntityLists.lstMonthwiseAmount.AsEnumerable()
                                                       on Range.AmountRangeId equals MonthwiseAmount.AmountRangeId
                                                  where Range.EarningsDeductionsId == EDRange.EarningsDeductionsId &&
                                                        Range.IsDefault == false &&
                                                        UsersED.UserId == aiUserId &&
                                                        UsersED.IsFormula == false &&
                                                        Range.FromAmount <= aiEarningsSum
                                                        && Range.UptoAmount >= aiEarningsSum
                                                        && MonthwiseAmount.MonthId == DateTime.Now.Month
                                                  select new AmountRange
                                                  {
                                                      FromAmount = Range.FromAmount,
                                                      UptoAmount = Range.UptoAmount,
                                                      Amount = MonthwiseAmount.Amount
                                                  }).ToList();


                if (UsersRangeED.Count() == 0)
                {
                    UsersRangeED = SalaryEntityLists.lstAmountRange
                                    .Join(SalaryEntityLists.lstMonthwiseAmount, Range => Range.AmountRangeId, MonthwiseAmount => MonthwiseAmount.AmountRangeId,
                                    (Range, MonthwiseAmount) => new { Range = Range, MonthwiseAmount = MonthwiseAmount })
                                    .Where(amountRange => amountRange.Range.EarningsDeductionsId == EDRange.EarningsDeductionsId &&
                                           amountRange.MonthwiseAmount.AmountRangeId == EDRange.AmountRangeID &&
                                           amountRange.MonthwiseAmount.MonthId == DateTime.Now.Month)
                                    .Select(amountRange =>
                                            new AmountRange
                                            {
                                                FromAmount = amountRange.Range.FromAmount,
                                                UptoAmount = amountRange.Range.UptoAmount,
                                                Amount = amountRange.MonthwiseAmount.Amount
                                            })
                                    .ToList();

                }

                if (UsersRangeED.Count() > 0)
                    iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(UsersRangeED.First().Amount)));
                else
                    iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(EDRange.Amount)));

                aoEdFormulaValues[EDRange.EarningsDeductionsId] = iAmount;
            }
        }

        /// <summary>
        /// This method is used to return deeduction's total.
        /// </summary>
        /// <param name="EdFormulaValues"></param>
        /// <returns></returns>
        private int GetDeductionTotal(Dictionary<int, int> aoEdFormulaValues)
        {
            var EDSum1 = (from ed in SalaryEntityLists.lstEarningsDeductions
                          join kvp in aoEdFormulaValues.AsEnumerable()
                          on ed.EarningsDeductionsId equals kvp.Key
                          where kvp.Value != -1 && !ed.IsEarning
                          group kvp by ed.IsEarning into edValue
                          select new
                          {
                              IsEarning = edValue.Key,
                              TotalAmount = edValue.Sum(ed => ed.Value)
                          }
                                ).FirstOrDefault();

            int iDeductionSum = EDSum1.TotalAmount;
            return iDeductionSum;
        }

        /// <summary>
        /// This method is used to populate user's ED object.
        /// </summary>
        /// <param name="aiEarningsDeductionsId"></param>
        /// <param name="dcAmount"></param>
        /// <param name="abIsEarning"></param>
        /// <param name="asShortName"></param>
        /// <returns></returns>
        private UsersEarningsDeduction PopulateUsersED(int aiEarningsDeductionsId, decimal dcAmount, bool abIsEarning, string asShortName)
        {
           return new UsersEarningsDeduction
            {
                EarningsDeductionsId = aiEarningsDeductionsId,
                EarningsDeductionsValue = dcAmount,
                IsEarning = abIsEarning,
                ShortName = asShortName
            };
        }

        public void SavePaymentDetails(bool abIsOnlineTransaction, string asTransactionNumber, int aiSchoolId, int aiMonthId, int aiYear)
        {
            moSalaryDetailsDC.SavePaymentDetails(abIsOnlineTransaction, asTransactionNumber, aiSchoolId, aiMonthId, aiYear);
        }

        public List<GrossSalaryDetails> GetGrossSalary(int aiSchoolId, int aiMonthId, int aiYear)
        {
           return moSalaryDetailsDC.GetGrossSalary(aiSchoolId, aiMonthId, aiYear);
        }
    }
}