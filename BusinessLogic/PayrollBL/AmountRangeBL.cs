// Class Name       :- AmountRangeBL
// Purpose          :- This class is used to manage AmountRange details.
// Date Of creation :- 11/4/2009
// Author Name      :- 

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class AmountRangeBL
    {
        #region Data Member(s)

        private AmountRangeDC moAmountRangeDC;
        private List<UsersFormulaAndRanges> mlstUsersFormulaeAndRanges;
        private List<EarningsDeductions> mlstEarningsDeductions;
        
        #endregion

        #region Constructor(s)

        public AmountRangeBL()
        {
            this.moAmountRangeDC = new AmountRangeDC();
        }

        public AmountRangeBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moAmountRangeDC = new AmountRangeDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Property(s)

        public List<AmountRange> AmountRanges
        {
            get { return this.moAmountRangeDC.AmountRanges; }
            set { this.moAmountRangeDC.AmountRanges = value; }
        }

        public List<MonthwiseAmount> MonthwiseAmounts
        {
            get { return this.moAmountRangeDC.MonthwiseAmounts;  }
            set { this.moAmountRangeDC.MonthwiseAmounts = value; }
        }

        public List<UsersFormulaAndRanges> UsersFormulaeAndRanges
        {   
            set { this.mlstUsersFormulaeAndRanges = value; }
        }

        public List<EarningsDeductions> EarningsDeductions
        {
            set { this.mlstEarningsDeductions = value; }
        }

        public AmountRange AmountRange
        {
            get { return this.moAmountRangeDC.AmountRange; }
            set { this.moAmountRangeDC.AmountRange = value; }
        }

        #endregion

        #region Methods

        public void Insert()
        {
            this.moAmountRangeDC.Insert();
        }

        public void IsDuplicateRangeName()
        {
            int iCount = 0;
            iCount = this.moAmountRangeDC.IsDuplicateRangeName();
            if (iCount > 0)
                throw new DuplicateName("Range name already exists.");
        }

        public void InsertMonthwiseAmount()
        {
            this.moAmountRangeDC.InsertMonthwiseAmount();
        }

        public void Update()
        {
            this.moAmountRangeDC.Update();
        }

        public void Delete(int aiAmountRangeId)
        {
            this.moAmountRangeDC.Delete(aiAmountRangeId);
        }

        public void DeleteAmountRange(int aiRangeId)
        {
            string sMessage = this.moAmountRangeDC.DeleteAmountRange(aiRangeId);
            if (sMessage != string.Empty)
                throw new Exceptions.ReferenceExceptions(sMessage);
        }

        public DataTable GetMonthwiseAmount(int aiAmountRangeId)
        {
            return this.moAmountRangeDC.GetMonthwiseAmount(aiAmountRangeId);
        }

        public DataSet GetAmountRanges(int aiEarningDeductionId)
        {
            return this.moAmountRangeDC.GetAll(aiEarningDeductionId);
        }

        public DataTable InsertRangeRow(int iAmountRangeId)
        {
            return this.moAmountRangeDC.InsertRangeRow(iAmountRangeId);
        }

        public void UpdateRangeRow(int iAmountRangeId)
        {
            this.moAmountRangeDC.UpdateRangeRow(iAmountRangeId);
        } 

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to return amount range details.
        /// </summary>
        /// <param name="aolstAmountRanges"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public dynamic GetAmountRangeDetails(int aiEarningsSum, int aiUserId, int aiMonthId, List<StaffGroupsEarningDeductionAssociation> alstStaffGroupsEarningDeductionAssociations, List<UsersSGAssociation> alstUsersSGAssociations)
        {
            List<AmountRange> olstAmountRanges = this.AmountRanges.Where(amountRange => amountRange.FromAmount <= aiEarningsSum && amountRange.UptoAmount >= aiEarningsSum && amountRange.IsDefault).ToList();

            return from amountRange in olstAmountRanges
                   join EarnDeduction in this.mlstEarningsDeductions
                   on amountRange.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                   join StaffGroupsEearnDeductionAsso in alstStaffGroupsEarningDeductionAssociations
                   on amountRange.EarningsDeductionsId equals StaffGroupsEearnDeductionAsso.EarningsDeductionsId
                   join UserStaffGroups in alstUsersSGAssociations
                   on StaffGroupsEearnDeductionAsso.StaffGroupsId equals UserStaffGroups.StaffGroupsId
                   join monthwiseAmount in this.MonthwiseAmounts
                   on amountRange.AmountRangeId equals monthwiseAmount.AmountRangeId
                   where UserStaffGroups.UserId == aiUserId
                         && monthwiseAmount.MonthId == aiMonthId
                   select new
                   {
                       EarningsDeductionsId = amountRange.EarningsDeductionsId,
                       FromAmount = amountRange.FromAmount,
                       UptoAmount = amountRange.UptoAmount,
                       Amount = monthwiseAmount.Amount,
                       ShortName = EarnDeduction.ShortName,
                       IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                       IsEarning = EarnDeduction.IsEarning,
                       HasFormula = EarnDeduction.HasFormula,
                       AmountRangeID = amountRange.AmountRangeId
                   };
        }

        /// <summary>
        /// This method is used to return users amount ranges.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiEarningsSum"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="iAmount"></param>
        /// <param name="EDRange"></param>
        /// <returns></returns>
        public int GetUserAmountRange(int aiUserId, int aiEarningsSum, int aiMonthId, int iAmount, dynamic oEDRange)
        {
            var oUsersRangeED = from UsersED in this.mlstUsersFormulaeAndRanges
                                join Range in this.AmountRanges
                                on UsersED.FormulaRangeId equals Range.RangeId
                                join monthwiseAmount in this.MonthwiseAmounts
                                     on Range.AmountRangeId equals monthwiseAmount.AmountRangeId
                                where Range.EarningsDeductionsId == oEDRange.EarningsDeductionsId &&
                                      monthwiseAmount.AmountRangeId == oEDRange.AmountRangeID &&
                                      UsersED.UserId == aiUserId &&
                                      UsersED.IsFormula == false &&
                                      Range.FromAmount <= aiEarningsSum
                                      && Range.UptoAmount >= aiEarningsSum
                                      && monthwiseAmount.MonthId == aiMonthId
                                select new
                                {
                                    FromAmount = Range.FromAmount,
                                    UptoAmount = Range.UptoAmount,
                                    Amount = monthwiseAmount.Amount
                                };

            if (oUsersRangeED.Count() == 0)
            {
                oUsersRangeED = from Range in this.AmountRanges
                                join monthwiseAmount in this.MonthwiseAmounts
                                     on Range.AmountRangeId equals monthwiseAmount.AmountRangeId
                                where Range.EarningsDeductionsId == oEDRange.EarningsDeductionsId
                                      && monthwiseAmount.AmountRangeId == oEDRange.AmountRangeID
                                      && monthwiseAmount.MonthId == aiMonthId
                                select new
                                {
                                    FromAmount = Range.FromAmount,
                                    UptoAmount = Range.UptoAmount,
                                    Amount = monthwiseAmount.Amount
                                };
            }

            // If any range is assigned to any user otherwise retrn default value.
            if (oUsersRangeED.Count() > 0)
                iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(oUsersRangeED.First().Amount)));
            else
                iAmount = Convert.ToInt32(Math.Round(Convert.ToDecimal(oEDRange.Amount)));
            return iAmount;
        } 

        #endregion

        internal List<PayrollEntities.EarningsDeductions> GetAmountRanges(int aiUserId, List<UsersSGAssociation> alstUsersSGAssociations, List<StaffGroupsEarningDeductionAssociation> alstStaffGroupsEarningDeductionAssociations)
        {
            return (from range in this.AmountRanges
                    join EarnDeduction in this.mlstEarningsDeductions
                    on range.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                    join SGEDAsso in alstStaffGroupsEarningDeductionAssociations
                    on range.EarningsDeductionsId equals SGEDAsso.EarningsDeductionsId
                    join UserSG in alstUsersSGAssociations
                    on SGEDAsso.StaffGroupsId equals UserSG.StaffGroupsId
                    where UserSG.UserId == aiUserId &&
                           range.IsDefault == true
                    orderby EarnDeduction.OriginalEarningsDeductionsId ascending
                    select new EarningsDeductions
                    {
                        EarningsDeductionsId = range.EarningsDeductionsId,
                        // Formula = range.Formula,
                        ShortName = EarnDeduction.ShortName,
                        IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                        IsEarning = EarnDeduction.IsEarning,
                        HasFormula = EarnDeduction.HasFormula
                    }).ToList();
        }
    }
}
