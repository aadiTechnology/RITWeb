using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;

namespace BusinessLogic
{
    class SalaryDetailTableBL
    {
    }
    public class UsersBasicDetails :SchoolEntities.SchoolEntity
    {
        int miUserId;
        string msName;
        string msDesignation;
        int miOriginalStaffGroupId;
        int miStaffGroupId;
        char mcGender;

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

        public char Gender
        {
            get
            {
                return mcGender;
            }
            set
            {
                mcGender = value;
            }
        }
    }

    public class UsersEarnDeductDetails
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

    public class ConfiguredDefaultLeaves
    {
        string msShortName;
        int miLeaveId;
        decimal mdcDays;

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

    }
}