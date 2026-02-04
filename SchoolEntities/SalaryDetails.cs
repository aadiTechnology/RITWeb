/* Class Name :- SalaryDetails.cs
 * Created By :- Shobha
 * Created Date :- 03-Dec-2010
 * Description :- This class is used create basic objects related to payroll .
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using SchoolEntities;

namespace PayrollEntities
{
    public class SalaryDetails : StaffBaseDetails
    {
        public int SalaryDetailsId { get; set; }        
        public int StaffGroupsId { get; set; }
        public string SalaryDetailsXml { get; set; }
        public string IndividualXml { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
        public int LeaveTransferMonthId { get; set; }
        public bool IsPreviewDisplayed { get; set; }
        public string SalayDifferenceXml { get; set; }
        public int SchoolWiseBankAccountDetailsId { get; set; }
        public bool IsOnlineTransaction { get; set; }
    }

    public class UsersDetails : StaffBaseDetails
    {   
        public int OriginalStaffGroupsId { get; set; }
        public int StaffGroupsId { get; set; }
        public int SerialNo { get; set; }
    }

    public class StaffAttendance : StaffBaseDetails
    {
        public int StaffAttendanceId { get; set; }        
        public decimal PresentDays { get; set; }
        public int StaffGroupsId { get; set; }
        public string UserIdsXML { get; set; }
    }

    public class StaffGroupsEntity : SchoolEntity
    {
        public string StaffGroupsName { get; set; }
        public int StaffGroupsId { get; set; }
        public int OriginalStaffGroupsId { get; set; }
        public Constants.Action Action { get; set; }
        public int UserCount { get; set; }
    }

    public class StaffGroupsEarningDeductionAssociation : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public int StaffGroupsAndEarningDeductionAssociationId { get; set; }
        public int StaffGroupsId { get; set; }
        public string AssociationXML { get; set; }
    }

    public class UsersSGAssociation : SchoolEntity
    {
        public int UserId { get; set; }
        public int StaffGroupsId { get; set; }
        public string UserXml { get; set; }
    }

    [Serializable]
    public class EarningsDeductions : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public string EarningsDeductionsName { get; set; }
        public int OriginalEarningsDeductionsId { get; set; }
        public string ShortName { get; set; }
        public bool IsAttendanceDependent { get; set; }
        public bool IsEarning { get; set; }
        public bool HasFormula { get; set; }
        public bool IsBasic { get; set; }
        public bool IncludeInSalaryDifference { get; set; }
        public string Formula { get; set; }
        public bool IsModified { get; set; }
        public Constants.Action Action { get; set; }
        public string FormulaOrRange { get; set; }
    }

    [Serializable]
    public class UsersEarningsDeduction : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public int UsersEarningsDeductionsId { get; set; }        
        public int UserId { get; set; }
        public decimal EarningsDeductionsValue { get; set; }
        public string ShortName { get; set; }
        public bool IsAttendanceDependent { get; set; }
        public bool IsEarning { get; set; }
        public bool HasFormula { get; set; }
        public string Reason { get; set; }
        public int PayScaleSettingId { get; set; }
        public string Type { get; set; }
        public int StaffGroupId { get; set; }
        public string EarningsDeductionsXml { get; set; }
        public string FormulaAndRangeXml { get; set; }
        public char ApplyFormulaToAllUsersOfStaffGroup { get; set; }
        public char ApplyToAllUsersOfStaffGroup { get; set; }
    }

    [Serializable]
    public class EarningsDeductionsFormulae : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public int FormulaId { get; set; }        
        public string Formula { get; set; }
        public string FormulaName { get; set; }
        public bool IsDefault { get; set; }
        public string ChildIds { get; set; }
    }

    [Serializable]
    public class AmountRange : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public int AmountRangeId { get; set; }
        public int RangeId { get; set; }        
        public decimal FromAmount { get; set; }
        public decimal UptoAmount { get; set; }
        public decimal Amount { get; set; }
        public bool IsDefault { get; set; }
        public string RangeName { get; set; }
        public char UpdateMonthwiseAmount { get; set; }
        public string AmountRangeXml { get; set; }
        public string MonthXml { get; set; }
    }

    [Serializable]
    public class MonthwiseAmount : SchoolEntity
    {
        public int MounthwiseAmountId { get; set; }
        public int AmountRangeId { get; set; }
        public int MonthId { get; set; }
        public decimal Amount { get; set; }
    }

    [Serializable]
    public class MonthAndYear : StaffBaseDetails
    {
    }

    public class StaticSalaryDetails : SchoolEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string SalaryDetailsXml { get; set; }
        public int StaffGroupId { get; set; }
    }

    public class UnpublishStatus : SchoolEntity
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

    // Table Name-UsersEarningsDeductionsFormula
    [Serializable]
    public class UsersFormulaAndRanges : SchoolEntity
    {
        public int UsersFormulaRangeId { get; set; }
        public int UserId { get; set; }
        public int FormulaRangeId { get; set; }
        public bool IsFormula { get; set; }
        public string UserName { get; set; }
    }

    [Serializable]
    public class SalaryDifference : StaffBaseDetails
    {
        public int SalaryDifferenceId { get; set; }       
        public decimal Amount { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal ProvidentFund { get; set; }   
        public int PaidMonthId { get; set; }
        public int PaidYearId { get; set; }
        public decimal AmountToBePaid { get; set; }        
    }

    [Serializable]
    public class SavedSalaryDifference
    {
        public int SalaryDifferenceId { get; set; }
        public int EarningDeductionId { get; set; }
        public string EarningDeductionName { get; set; }
        public decimal Amount { get; set; }
        public bool IsLastTransaction { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
    }

    public class SalaryCommonUtility : StaffBaseDetails
    {   
        public int StaffGroupsId { get; set; }
        public string StaffGroupsName { get; set; }
    }

    public class SchoolWiseBankAccountDetails : SchoolEntity
    {
        public int SchoolWiseBankAccountDetailsId { get; set; }
        public string BankName { get; set; }
        public int BankAssociationCount { get; set; }
        public string AccountNo { get; set; }
        public int BankId{ get; set; } 
    }

    public class SalaryYear
    {   
        public int Year { get; set; }
    }

    public class SalaryMonth
    {   
        public string Month { get; set; }
        public int MonthId { get; set; }     
    }

    public class SalaryEntityList
    {
        public List<UsersDetails> lstUsersDetails = new List<UsersDetails>();
        public List<StaffGroupsEntity> lstStaffGroups = new List<StaffGroupsEntity>();
        public List<EarningsDeductions> lstEarningsDeductions = new List<EarningsDeductions>();
        public List<EarningsDeductionsFormulae> lstEarningsDeductionsFormulae  = new List<EarningsDeductionsFormulae>();
        public List<AmountRange> lstAmountRange  = new List<AmountRange>();
        public List<MonthwiseAmount> lstMonthwiseAmount  = new List<MonthwiseAmount>();
        public List<ConfiguredLeaves> lstConfiguredLeaves  = new List<ConfiguredLeaves>();
        public List<StaffAttendance> lstStaffAttendance  = new List<StaffAttendance>();
        public List<StaffLeaveDetails> lstStaffLeaveDetails  = new List<StaffLeaveDetails>();
        public List<UsersSGAssociation> lstUsersSGAssociation  = new List<UsersSGAssociation>();
        public List<UsersEarningsDeduction> lstUsersEarningsDeduction  = new List<UsersEarningsDeduction>();
        public List<StaffGroupsEarningDeductionAssociation> lstStaffGroupsEarningDeductionAssociation =  new List<StaffGroupsEarningDeductionAssociation>();
        public List<UserLeaveConfiguration> lstUserLeaveConfiguration  = new List<UserLeaveConfiguration>();
        public List<UsersFormulaAndRanges> lstUsersFormulaAndRanges  = new List<UsersFormulaAndRanges>();
        public List<StaticSalaryDetails> lstStaticSalaryDetails  = new List<StaticSalaryDetails>();
        public List<SalaryDifference> lstSalaryDifference  = new List<SalaryDifference>();
        public List<string> lstSalaryDifferenceMonths  = new List<string>();
        public List<UserLateMarkLeave> lstUserLateMarkLeaves  = new List<UserLateMarkLeave>();
        public List<LateMarkConfiguration> lstLateMarkConfigurations  = new List<LateMarkConfiguration>();
        public List<UsersSalaryDeduction> lstUsersSalaryDeductions  = new List<UsersSalaryDeduction>();
        public List<SalaryCommonUtility> lstSalaryCommonUtility = new List<SalaryCommonUtility>();
        public List<StaffBaseDetails> lstStaffBaseDetails = new List<StaffBaseDetails>();
    }

    [Serializable]
    public class SalaryDifferenceClass
    {
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
    }

    [Serializable]
    public class StaffBaseDetails : SchoolEntity
    {
        public int SrNo { get; set; }
        public int UserId { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ResignDate { get; set; }
        public char Gender { get; set; }
    }

    public class PaidSalaryDetails : StaffBaseDetails
    {
        public string MobileNo { get; set; }
        public string Month { get; set; }
        public int AdminId { get; set; }
        public decimal NetSalary { get; set; }
        public string EarnDeductName { get; set; }
        public decimal Amount { get; set; }        
    }

    public class Insurance : StaffBaseDetails
    {
        public decimal InsuranceAmount { get; set; }
        public int UserStatus { get; set; }
        public string InsuranceCardNumber { get; set; }
    }

    public class UsersInsuranceDependent : SchoolEntity
    {
        public int UsersInsuranceDependentId { get; set; }
        public int UserId { get; set; }
        public int SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string InsuranceCardNumber { get; set; }
        public string ChildName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Relation { get; set; }
        public string Name { get; set; }

    }

    [Serializable]
    public class Salutations : SchoolEntity
    {
        public int SalutationId { get; set; }
        public string SalutationName { get; set; }
    }

    [Serializable]
    public class StaffStatusDetails : SchoolEntity
    {
        public int StaffStatusDetailsId { get; set; }
        public int UserId { get; set; }
        public int DesignationId { get; set; }
        public int StatusId { get; set; }
        public string UserName { get; set; }
        public string DesignationName { get; set; }
        public string StatusName { get; set; }
        public string IsDeleted { get; set; }
        public string IsLocked { get; set; }
    }

    [Serializable]
    public class SalaryDifferenceConfigDetails : SchoolEntity
    {
        public int EarningsDeductionsId { get; set; }
        public string ShortName { get; set; }
        public int UserId { get; set; }
        public int FormulaRangeId { get; set; }
        public bool IsFormula { get; set; }
        public bool IsConfigured { get; set; }
    }

    public class MonthwiseProfessionalTaxDetails : SchoolEntity
    {
        public int MonthwiseProfessionalTaxDetailsId { get; set; }
        public int PTRegCertificateId { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public int BankId { get; set; }
        public string ChequeNo { get; set; }        
        public string PTRegCertificateNo { get; set; }
        public string CINNo { get; set; }
    }

    public class OtherStaff : SchoolEntity
    {
        public int OtherStaffId { get; set; }
        public int SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmergencyNo { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfJoining { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int DesignationId { get; set; }
        public int UserId { get; set; }
        public string PhotoFilePath { get; set; }
        public byte[] BinaryFormatPhoto { get; set; }
    }

    public class SalaryPaymentDetails
    {
        public int MonthId { get; set; }
        public int Year { get; set; }
        public bool IsOnlineTransaction { get; set; }
        public string TransactionNumber { get; set; }
        public string Month { get; set; }
        public bool IsLastRecord { get; set; }
        public string IsOnlineTransactionText { get; set; }
    }

    public class StaffWorkingStatus
    {
        public int StatusId { get; set; }
        public string WorkingStatus { get; set; }
    }

    public class GrossSalaryDetails
    {
        public int UserId { get; set; }
        public int Amount { get; set; }
    }
}