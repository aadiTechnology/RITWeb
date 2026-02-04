// Class Name       :- UsersEarningsDeductionsBL
// Purpose          :- This class is used to manage UsersEarningsDeductions details.
// Date Of creation :- 11/11/2009
// Author Name      :- Sachin

using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class UsersEarningsDeductionsBL
    {
        #region Data Member(s)

        private UsersEarningsDeductionsDC moUsersEarningsDeductionsDC;
        private List<UsersSGAssociation> mlstUsersSGAssociations;
        private List<EarningsDeductions> mlstEarningsDeductions;
        
        #endregion

        #region Constructor(s)

        public UsersEarningsDeductionsBL()
        {
            this.moUsersEarningsDeductionsDC = new UsersEarningsDeductionsDC();
        }

        public UsersEarningsDeductionsBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moUsersEarningsDeductionsDC = new UsersEarningsDeductionsDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        public List<SalaryDifferenceConfigDetails> SalaryDifferenceConfigDetails
        {
            get { return this.moUsersEarningsDeductionsDC.SalaryDifferenceConfigDetails; }
            set { this.moUsersEarningsDeductionsDC.SalaryDifferenceConfigDetails = value; }
        }

        #endregion

        #region Property(s)

        public UsersEarningsDeduction UsersEarningsDeduction
        {
            get { return this.moUsersEarningsDeductionsDC.UsersEarningsDeduction; }
            set { this.moUsersEarningsDeductionsDC.UsersEarningsDeduction = value; }
        }

        public List<UsersFormulaAndRanges> UsersFormulaAndRanges
        {
            get { return this.moUsersEarningsDeductionsDC.UsersFormulaeAndRanges; }
            set { this.moUsersEarningsDeductionsDC.UsersFormulaeAndRanges = value; }
        }

        public List<UsersEarningsDeduction> UsersEarningsDeductionDetails
        {
            get { return this.moUsersEarningsDeductionsDC.UsersEarningsDeductions; }
            set { this.moUsersEarningsDeductionsDC.UsersEarningsDeductions = value; }
        }
       
        public List<UsersEarningsDeduction> UsersSalDifferenceDetails
        {
            get { return this.moUsersEarningsDeductionsDC.UsersSalDifferenceDetails; }
            set { this.moUsersEarningsDeductionsDC.UsersSalDifferenceDetails = value; }
        }

        public List<UsersSGAssociation> UsersSGAssociations
        {
            set { this.mlstUsersSGAssociations = value; }
        }

        public List<EarningsDeductions> EarningsDeductions
        {
            set { this.mlstEarningsDeductions = value; }
        }

        #endregion

        #region Methods(s)

        /// <summary>
        /// This method is used to save user's earning deduction details.
        /// </summary>
        public void Insert()
        {
            this.moUsersEarningsDeductionsDC.Insert();
        }

        /// <summary>
        /// This method is used to return user's earning deduction details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiPayScaleSettingsId"></param>
        /// <returns></returns>
        public DataSet GetAll(int aiUserId, int aiStaffGroupId, int aiPayScaleSettingsId)
        {
            return this.moUsersEarningsDeductionsDC.GetAll(aiUserId, aiStaffGroupId, aiPayScaleSettingsId);
        }

        /// <summary>
        /// This method is used to return pay scale setting details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public DataSet GetPayScaleSettings(int aiUserId)
        {
            return this.moUsersEarningsDeductionsDC.GetPayScaleSettings(aiUserId);
        }

        /// <summary>
        /// This method is used to return user earning deduction details that are release to investment methods.
        /// </summary>
        /// <param name="aiFinYearId"></param>
        /// <param name="asUserIds"></param>
        /// <returns></returns>
        public List<EarningDeductionAmount> GetEarningDeductionDetails(int aiFinYearId, string asUserIds)
        {
            return this.moUsersEarningsDeductionsDC.GetEarningDeductionDetails(aiFinYearId, asUserIds);
        }

        /// <summary>
        /// This method is used to return user's age details.
        /// </summary>
        /// <param name="asUserIds"></param>
        /// <returns></returns>
        public List<UserAgeDetails> GetUserAgeDetails(string asUserIds)
        {
            return this.moUsersEarningsDeductionsDC.GetUserAgeDetails(asUserIds);
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to return users earning deduction.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<int> GetUsersEarningDeductionIds(int aiUserId, List<StaffGroupsEarningDeductionAssociation> alstStaffGroupsEarningDeductionAssociations)
        {
            List<int> olstAvailableED = (from UsersEarnDeduction in this.UsersEarningsDeductionDetails
                                         join UserSGAsso in this.mlstUsersSGAssociations
                                         on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                                         join EarnDeductions in this.mlstEarningsDeductions
                                         on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId
                                         where UsersEarnDeduction.UserId == aiUserId
                                                   && EarnDeductions.HasFormula == false
                                         select UsersEarnDeduction.EarningsDeductionsId).ToList();

            if (olstAvailableED.Count() == 0)
            {
                olstAvailableED = (from StaffGroupEarnDeductAsso in alstStaffGroupsEarningDeductionAssociations
                                   join UserSGAsso in this.mlstUsersSGAssociations
                                   on StaffGroupEarnDeductAsso.StaffGroupsId equals UserSGAsso.StaffGroupsId
                                   where UserSGAsso.UserId == aiUserId
                                   select StaffGroupEarnDeductAsso.EarningsDeductionsId).ToList();
            }

            return olstAvailableED;
        }

        /// <summary>
        /// This method is used to return users earning deductions.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aoSalaryDifferenceDC"></param>
        /// <returns></returns>
        public List<UsersEarnDeductDetails> GetUsersEarningDeductions(int aiUserId, List<StaffGroupsEarningDeductionAssociation> alstStaffGroupsEarningDeductionAssociations)
        {
            List<UsersEarnDeductDetails> oUsersEarningsDeductions = (from UsersEarnDeduction in this.UsersEarningsDeductionDetails
                                                                     join UserSGAsso in this.mlstUsersSGAssociations
                                                                     on UsersEarnDeduction.UserId equals UserSGAsso.UserId
                                                                     join EarnDeductions in this.mlstEarningsDeductions
                                                                     on UsersEarnDeduction.EarningsDeductionsId equals EarnDeductions.EarningsDeductionsId
                                                                     join SGEDAsso in alstStaffGroupsEarningDeductionAssociations
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
                                                                         HasFormula = EarnDeductions.HasFormula,
                                                                         Reason = UsersEarnDeduction.Reason
                                                                     }).ToList();
            return oUsersEarningsDeductions;
        } 

        #endregion
    }
}
