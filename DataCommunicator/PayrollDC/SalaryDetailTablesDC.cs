using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommunicator
{
    class SalaryDetailTablesDC
    {
    }

    public class UsersDetails
    {
        int miUserId;
        string msName;
        string msDesignation;
        int miOriginalStaffGroupId;
        int miStaffGroupId;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public string Name
        {
            get
            {
                return msName;
            }
            set
            {
                msName = value;
            }
        }

        public string Designation
        {
            get
            {
                return msDesignation;
            }
            set
            {
                msDesignation = value;
            }
        }

        public int OriginalStaffGroupId
        {
            get
            {
                return miOriginalStaffGroupId;
            }
            set
            {
                miOriginalStaffGroupId = value;
            }
        }

        public int StaffGroupId
        {
            get
            {
                return miStaffGroupId;
            }
            set
            {
                miStaffGroupId = value;
            }
        }
    }

    public class StaffAttendance
    {
        int miStaffAttendanceId;
        decimal mdcAttendance;
        int miUserId;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public int StaffAttendanceId
        {
            get
            {
                return miStaffAttendanceId;
            }
            set
            {
                miStaffAttendanceId = value;
            }
        }

        public decimal Attendance
        {
            get
            {
                return mdcAttendance;
            }
            set
            {
                mdcAttendance = value;
            }
        }

    }

    public class StaffLeaveDetails
    {
        string msShortName;
        decimal mdcDays;
        int miLeaveId;
        int miStaffAttendanceId;

        public string ShortName
        {
            get
            {
                return msShortName;
            }
            set
            {
                msShortName = value;
            }
        }

        public int LeaveId
        {
            get
            {
                return miLeaveId;
            }
            set
            {
                miLeaveId = value;
            }
        }

        public decimal Days
        {
            get
            {
                return mdcDays;
            }
            set
            {
                mdcDays = value;
            }
        }

        public int StaffAttendanceId
        {
            get
            {
                return miStaffAttendanceId;
            }
            set
            {
                miStaffAttendanceId = value;
            }
        }
    }

    public class ConfiguredLeaves
    {
        string msShortName;
        int miLeaveId;
        bool mbIsUnpaidLeave;
        int miOriginalLeaveId;

        public string ShortName
        {
            get
            {
                return msShortName;
            }
            set
            {
                msShortName = value;
            }
        }

        public int LeaveId
        {
            get
            {
                return miLeaveId;
            }
            set
            {
                miLeaveId = value;
            }
        }

        public bool IsUnpaidLeave
        {
            get
            {
                return mbIsUnpaidLeave;
            }
            set
            {
                mbIsUnpaidLeave = value;
            }
        }

        public int OriginalLeaveId
        {
            get
            {
                return miOriginalLeaveId;
            }
            set
            {
                miOriginalLeaveId = value;
            }
        }
    }

    public class StaffGroups
    {
        int miStaffGroupsId;
        string msStaffGroupsName;
        int miOriginalStaffGroupsId;

        public string StaffGroupsName
        {
            get
            {
                return msStaffGroupsName;
            }
            set
            {
                msStaffGroupsName = value;
            }
        }

        public int StaffGroupsId
        {
            get
            {
                return miStaffGroupsId;
            }
            set
            {
                miStaffGroupsId = value;
            }
        }

        public int OriginalStaffGroupsId
        {
            get
            {
                return miOriginalStaffGroupsId;
            }
            set
            {
                miOriginalStaffGroupsId = value;
            }
        }

    }

    public class StaffGroupsEarningDeductionAssociation
    {
        int miEarningsDeductionsId;
        int miStaffGroupsId;

        public int StaffGroupsId
        {
            get
            {
                return miStaffGroupsId;
            }
            set
            {
                miStaffGroupsId = value;
            }
        }

        public int EarningsDeductionsId
        {
            get
            {
                return miEarningsDeductionsId;
            }
            set
            {
                miEarningsDeductionsId = value;
            }
        }
    }

    public class UsersSGAssociation
    {
        int miUserId;
        int miStaffGroupsId;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public int StaffGroupsId
        {
            get
            {
                return miStaffGroupsId;
            }
            set
            {
                miStaffGroupsId = value;
            }
        }

    }

    public class UserLeaveConfiguration
    {
        int miUserId;
        int miLeaveId;
        decimal mdcLeaveBalance;
        decimal mdcOriginalLeaveBalance;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public int LeaveId
        {
            get
            {
                return miLeaveId;
            }
            set
            {
                miLeaveId = value;
            }
        }

        public decimal OriginalLeaveBalance
        {
            get
            {
                return mdcOriginalLeaveBalance;
            }
            set
            {
                mdcOriginalLeaveBalance = value;
            }
        }

        public decimal LeaveBalance
        {
            get
            {
                return mdcLeaveBalance;
            }
            set
            {
                mdcLeaveBalance = value;
            }
        }
    }

    public class EarningsDeductions
    {       
        int miEarningsDeductionsId;
        int miOriginalEarningsDeductionsId;
        string msShortName;
        bool mbIsAttendanceDependent;
        bool mbIsEarning;
        bool mbHasFormula;

        public int EarningsDeductionsId
        {
            get
            {
                return miEarningsDeductionsId;
            }
            set
            {
                miEarningsDeductionsId = value;
            }
        }

        public int OriginalEarningsDeductionsId
        {
            get
            {
                return miOriginalEarningsDeductionsId;
            }
            set
            {
                miOriginalEarningsDeductionsId = value;
            }
        }

        public string ShortName
        {
            get
            {
                return msShortName;
            }
            set
            {
                msShortName = value;
            }
        }

        public bool IsAttendanceDependent
        {
            get
            {
                return mbIsAttendanceDependent;
            }
            set
            {
                mbIsAttendanceDependent = value;
            }
        }

        public bool IsEarning
        {
            get
            {
                return mbIsEarning;
            }
            set
            {
                mbIsEarning = value;
            }
        }

        public bool HasFormula
        {
            get
            {
                return mbHasFormula;
            }
            set
            {
                mbHasFormula = value;
            }
        }
    }

    public class UsersEarningsDeduction
    {
        int miUserId;
        int miEarningDeductionId;
        decimal mdcEarningDeductionValue;
        string msShortName;
        bool mbIsAttendanceDependent;
        bool mbIsEarning;
        bool mbHasFormula;
        string msReason;

        public int EarningsDeductionsId
        {
            get
            {
                return miEarningDeductionId;
            }
            set
            {
                miEarningDeductionId = value;
            }
        }

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public decimal EarningsDeductionsValue
        {
            get
            {
                return mdcEarningDeductionValue;
            }
            set
            {
                mdcEarningDeductionValue = value;
            }
        }

        public string ShortName
        {
            get
            {
                return msShortName;
            }
            set
            {
                msShortName = value;
            }
        }

        public bool IsAttendanceDependent
        {
            get
            {
                return mbIsAttendanceDependent;
            }
            set
            {
                mbIsAttendanceDependent = value;
            }
        }

        public bool IsEarning
        {
            get
            {
                return mbIsEarning;
            }
            set
            {
                mbIsEarning = value;
            }
        }

        public bool HasFormula
        {
            get
            {
                return mbHasFormula;
            }
            set
            {
                mbHasFormula = value;
            }
        }

        public string Reason
        {
            get
            {
                return msReason;
            }
            set
            {
                msReason = value;
            }
        }
    }

    public class EarningsDeductionsFormulae
    {
        int miFormulaId;
        int miEarningsDeductionsId;
        string msFormula;
        bool mbIsDefault;

        public int FormulaId
        {
            get
            {
                return miFormulaId;
            }
            set
            {
                miFormulaId = value;
            }
        }

        public int EarningsDeductionsId
        {
            get
            {
                return miEarningsDeductionsId;
            }
            set
            {
                miEarningsDeductionsId = value;
            }
        }

        public string Formula
        {
            get
            {
                return msFormula;
            }
            set
            {
                msFormula = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return mbIsDefault;
            }
            set
            {
                mbIsDefault = value;
            }
        }
    }

    public class AmountRange
    {
        int miAmountRangeId;
        int miRangeId;
        int miEarningsDeductionsId;
        decimal mdcFromAmount;
        decimal mdcUptoAmount;
        decimal mdcAmount;
        bool mbIsDefault;

        public int AmountRangeId
        {
            get
            {
                return miAmountRangeId;
            }
            set
            {
                miAmountRangeId = value;
            }
        }

        public int RangeId
        {
            get
            {
                return miRangeId;
            }
            set
            {
                miRangeId = value;
            }
        }

        public int EarningsDeductionsId
        {
            get
            {
                return miEarningsDeductionsId;
            }
            set
            {
                miEarningsDeductionsId = value;
            }
        }

        public decimal FromAmount
        {
            get
            {
                return mdcFromAmount;
            }
            set
            {
                mdcFromAmount = value;
            }
        }

        public decimal UptoAmount
        {
            get
            {
                return mdcUptoAmount;
            }
            set
            {
                mdcUptoAmount = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return mdcAmount;
            }
            set
            {
                mdcAmount = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return mbIsDefault;
            }
            set
            {
                mbIsDefault = value;
            }
        }
    }

    public class MonthwiseAmount
    {
        int miAmountRangeId;
        int miMonthId;
        decimal mdcAmount;

        public int AmountRangeId
        {
            get
            {
                return miAmountRangeId;
            }
            set
            {
                miAmountRangeId = value;
            }
        }

        public int MonthId
        {
            get
            {
                return miMonthId;
            }
            set
            {
                miMonthId = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return mdcAmount;
            }
            set
            {
                mdcAmount = value;
            }
        }

    }

    public class MonthAndYear
    {
        int miMonthId;
        int miYear;

        public int Year
        {
            get
            {
                return miYear;
            }
            set
            {
                miYear = value;
            }
        }

        public int MonthId
        {
            get
            {
                return miMonthId;
            }
            set
            {
                miMonthId = value;
            }
        }

    }

    public class StaticSalaryDetails
    {
        string msSalaryDetailsXml;

        public string SalaryDetailsXml
        {
            get
            {
                return msSalaryDetailsXml;
            }
            set
            {
                msSalaryDetailsXml = value;
            }
        }
    }

    public class UnpublishStatus
    {
        char mcAllowUnpublish;

        public char AllowUnpublish
        {
            get
            {
                return mcAllowUnpublish;
            }
            set
            {
                mcAllowUnpublish = value;
            }
        }
    }

    public class UsersFormulaAndRanges
    {
        int miUserId;
        int miFormulaRangeId;
        bool mbIsFormula;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public int FormulaRangeId
        {
            get
            {
                return miFormulaRangeId;
            }
            set
            {
                miFormulaRangeId = value;
            }
        }

        public bool IsFormula
        {
            get
            {
                return mbIsFormula;
            }
            set
            {
                mbIsFormula = value;
            }
        }
    }

    public class SalaryDifference
    {
        int miUserId;
        int miMonthId;
        int miYear;
        decimal mdcAmount;
        string msName;
        string msDesignation;

        public int UserId
        {
            get
            {
                return miUserId;
            }
            set
            {
                miUserId = value;
            }
        }

        public int MonthId
        {
            get
            {
                return miMonthId;
            }
            set
            {
                miMonthId = value;
            }
        }

        public int Year
        {
            get
            {
                return miYear;
            }
            set
            {
                miYear = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return mdcAmount;
            }
            set
            {
                mdcAmount = value;
            }
        }

        public string Name
        {
            get
            {
                return msName;
            }
            set
            {
                msName = value;
            }
        }

        public string Designation
        {
            get
            {
                return msDesignation;
            }
            set
            {
                msDesignation = value;
            }
        }
    }

    [Serializable]
    public class UserLateMarkLeave
    {
        public int UserId { get; set; }
        public int LeaveId { get; set; }
        public decimal Days { get; set; }
        public bool IsUnPaidLeave { get; set; }
    }

}
