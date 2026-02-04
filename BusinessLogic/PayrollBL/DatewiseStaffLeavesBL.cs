// Class Name       :- DatewiseStaffLeavesBL
// Purpose          :- This class is used to manage DatewiseStaffLeaves details.
// Date Of creation :- 28-Aug-10
// Created By      :- Sachin


using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;
using SchoolEntities;

namespace BusinessLogic
{
    public class DatewiseStaffLeavesBL
    {
        #region Data Members

        private DatewiseStaffLeavesDC moDatewiseStaffLeavesDC;

        #endregion

        #region Constructors

        public DatewiseStaffLeavesBL()
        {
            moDatewiseStaffLeavesDC = new DatewiseStaffLeavesDC();
        }

        public DatewiseStaffLeavesBL(int miDatewiseStaffLeavesId)
        {
            moDatewiseStaffLeavesDC = new DatewiseStaffLeavesDC(miDatewiseStaffLeavesId);
        }

        public DatewiseStaffLeavesBL(int aiSchoolId, int aiAcademicYearId)
        {
            moDatewiseStaffLeavesDC = new DatewiseStaffLeavesDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        #region Proerties

        public DatewiseStaffLeave DatewiseStaffLeaves
        {
            get { return moDatewiseStaffLeavesDC.DatewiseStaffLeaves; }
            set { moDatewiseStaffLeavesDC.DatewiseStaffLeaves = value; }
        }

        public List<DaywiseLeaves> StaffLeaves
        {
            get { return moDatewiseStaffLeavesDC.StaffLeaves; }
        }

        public List<DaywiseLeaves> DatewiseLeaves
        {
            get { return moDatewiseStaffLeavesDC.DatewiseLeaves; }
        }

        public List<DaywiseLeaves> StaffLeaveDetails
        {
            get { return moDatewiseStaffLeavesDC.StaffLeaveDetails; }
        }

        public List<DaywiseLeaves> UserLeavesYearwiseConfigurations
        {
            get { return moDatewiseStaffLeavesDC.UserLeavesYearwiseConfigurations; }
        }

        public List<LateMarkConfiguration> LateMarkConfigurations
        {
            get { return moDatewiseStaffLeavesDC.LateMarkConfigurations; }
        }

        public List<string> StaffLeaveSortOrders
        {
            get { return moDatewiseStaffLeavesDC.StaffLeaveSortOrders; }
        }

        public List<LateMarkLeave> LateMarkLeaves
        {
            get { return moDatewiseStaffLeavesDC.LateMarkLeaves; }
        }

        public List<UsersSalaryDeduction> UsersSalaryDeductions
        {
            get { return moDatewiseStaffLeavesDC.UsersSalaryDeductions; }
        }

        public List<StaffHolidaysSalaryDeduction> StaffHolidayAndSalaryDeductionConfigurations
        {
            get { return moDatewiseStaffLeavesDC.StaffHolidayAndSalaryDeductionConfigurations; }
        }

        public List<WeekDay> WeekDays
        {
            get { return moDatewiseStaffLeavesDC.weekDays; }
        }

        public List<SalaryCommonUtility> SalaryCommonUtilityList
        {
            get { return moDatewiseStaffLeavesDC.SalaryCommonUtilityList; }
        }

        public List<PartialLeaveDetails> PartialLeaveDetailsList
        {
            get { return moDatewiseStaffLeavesDC.PartialLeaveDetailsList; }
        }

        public List<UsersLeaveBalance> UsersLeaveBalanceList
        {
            get { return moDatewiseStaffLeavesDC.UsersLeaveBalanceList; }
        }

        public List<ConfiguredLeaves> ConfiguredLeavesList
        {
            get { return moDatewiseStaffLeavesDC.ConfiguredLeavesList; }
        }

        public List<DateWiseStaffLeaves> DateWiseStaffLeavesList
        {
            get { return moDatewiseStaffLeavesDC.DateWiseStaffLeavesList; }
        }

        public List<WeekDayDetails> WeekDayDetailsList
        {
            get { return moDatewiseStaffLeavesDC.WeekDayDetailsList; }
        }

        public List<HolidayDetails> HolidayDetailsList
        {
            get { return moDatewiseStaffLeavesDC.HolidayDetailsList; }
        }

        public List<MonthDetails> MonthDetails
        {
            get { return moDatewiseStaffLeavesDC.MonthDetailsList; }
        }

        public List<MonthwiseStaffLeaveDetails> MonthwiseStaffLeaveDetailsList
        {
            get { return moDatewiseStaffLeavesDC.MonthwiseStaffLeaveDetailsList; }
        }

        public StaffBaseDetails StaffBaseDetails
        {
            get { return moDatewiseStaffLeavesDC.moStaffBaseDetails; }
        }

        public SalaryCommonUtility SalaryCommonUtility
        {
            get { return moDatewiseStaffLeavesDC.SalaryCommonUtility; }
        }

        public int PreAttachedHolidayId
        {
            get { return moDatewiseStaffLeavesDC.PreAttachedHolidayId; }
        }

        public int PostAttachedHolidayId
        {
            get { return moDatewiseStaffLeavesDC.PostAttachedHolidayId; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to save datewise leaves details.
        /// </summary>
        public void Insert()
        {
            moDatewiseStaffLeavesDC.Insert();
        }

        /// <summary>
        /// This method is used to return leave details.
        /// </summary>
        public void GetUserLeavesDetails()
        {
            moDatewiseStaffLeavesDC.GetUserLeavesDetails();
        }

        /// <summary>
        /// This method is used to return partial leave details.
        /// </summary>
        public void GetUsersPartialLeaveDetails()
        {
            moDatewiseStaffLeavesDC.GetUsersPartialLeaveDetails();
        }

        /// <summary>
        /// This method is used to return monthwise staff attendance details.
        /// </summary>
        public List<StaffDetails> FillMonthWiseStaffAttendance(int miYearId, int miStaffGroupId)
        {
            return moDatewiseStaffLeavesDC.GetAllMonthwiseStaffAttendance(miYearId, miStaffGroupId);
        }

        /// <summary>
        /// This method is used to return UserStaff Group Id and UserId For Search.
        /// </summary>
        public UserDetailsForLeave GetUserDetailsForLeave(string asName,int aiSchoolId)
        {
            return this.moDatewiseStaffLeavesDC.GetUserDetailsForLeave(asName,aiSchoolId);
        }      

        #endregion
    }
}
