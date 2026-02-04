// Class Name       :- EarningDeductionFormulaBL
// Purpose          :- This class is used to manage EarningDeductionFormula details.
// Date Of creation :- 11/3/2009
// Author Name      :- Sachin
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    public class EarningDeductionFormulaBL
    {
        #region Data Member(s)

        private EarningDeductionFormulaDC moEarningDeductionFormulaDC;
        private List<UsersFormulaAndRanges> mlstUsersFormulaeAndRanges;
        private List<EarningsDeductions> mlstEarningsDeductions;
                
        #endregion

        #region Constructor(s)

        public EarningDeductionFormulaBL()
        {
            this.moEarningDeductionFormulaDC = new EarningDeductionFormulaDC();
        }

        public EarningDeductionFormulaBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moEarningDeductionFormulaDC = new EarningDeductionFormulaDC(aiSchoolId, aiAcademicYearId, aiUserId);            
        } 

        #endregion

        #region Property(s)

        public List<EarningsDeductionsFormulae> EarningsDeductionsFormulae
        {
            get { return this.moEarningDeductionFormulaDC.EarningsDeductionsFormulae; }
            set { this.moEarningDeductionFormulaDC.EarningsDeductionsFormulae = value; }
        }

        public List<UsersFormulaAndRanges> UsersFormulaeAndRanges
        {
            set { this.mlstUsersFormulaeAndRanges = value; }
        }

        public List<EarningsDeductions> EarningsDeductions
        {
            set { this.mlstEarningsDeductions = value; }
        }

        public EarningsDeductionsFormulae EarningsDeductionsFormula
        {
            get { return this.moEarningDeductionFormulaDC.EarningsDeductionsFormula; }
            set { this.moEarningDeductionFormulaDC.EarningsDeductionsFormula = value; }
        }

        #endregion

        #region Method(s)

        public void Insert()
        {   
            this.moEarningDeductionFormulaDC.Insert();
        }

        public void Update()
        {  
            this.moEarningDeductionFormulaDC.Update();
        }

        public void Delete(int aiFormulaId)
        {   
            string sMessage = this.moEarningDeductionFormulaDC.Delete(aiFormulaId);
            if (sMessage != string.Empty)
                throw new Exceptions.ReferenceExceptions(sMessage);
        }

        public void DeleteFormulaAndRange(int aiFormulaId, int aiAmountRangeId, int aiEarningsDeductionsId)
        {
            this.moEarningDeductionFormulaDC.DeleteFormulaAndRange(aiFormulaId, aiAmountRangeId, aiEarningsDeductionsId);
        }

        public bool AreConfigured(string asIdList)
        {
            int iReturnValue = this.moEarningDeductionFormulaDC.AreConfigured(asIdList);
            if (iReturnValue > 0)
                return true;
            return false;
        }

        public string GetRecursiveFieldsOfFormula(int aiEarningsDeductionsId, string asIdList)
        {
            DataTable oDataTable = this.moEarningDeductionFormulaDC.GetRecursiveFieldsOfFormula(aiEarningsDeductionsId, asIdList);
            if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value && oDataTable.Rows[0][0].ToString() != string.Empty)
                return oDataTable.Rows[0][0].ToString();
            else
                return string.Empty;
        }

        public void IsDuplicateFormulaName()
        {   
            int iCount = 0;
            iCount = this.moEarningDeductionFormulaDC.IsDuplicateFormulaName();
            if (iCount > 0)
                throw new DuplicateName("Formula name already exists.");
        } 

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to return formulae associated with user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<EarningsDeductions> GetEarningDeductionFormulae(int aiUserId, List<UsersSGAssociation> alstUsersSGAssociations, List<StaffGroupsEarningDeductionAssociation> alstStaffGroupsEarningDeductionAssociations)
        {
            return (from EDFormula in this.EarningsDeductionsFormulae
                    join EarnDeduction in this.mlstEarningsDeductions
                    on EDFormula.EarningsDeductionsId equals EarnDeduction.EarningsDeductionsId
                    join SGEDAsso in alstStaffGroupsEarningDeductionAssociations
                    on EDFormula.EarningsDeductionsId equals SGEDAsso.EarningsDeductionsId
                    join UserSG in alstUsersSGAssociations
                    on SGEDAsso.StaffGroupsId equals UserSG.StaffGroupsId
                    where UserSG.UserId == aiUserId &&
                           EDFormula.IsDefault == true
                    orderby EarnDeduction.OriginalEarningsDeductionsId ascending
                    select new EarningsDeductions
                    {
                        EarningsDeductionsId = EDFormula.EarningsDeductionsId,
                        Formula = EDFormula.Formula,
                        ShortName = EarnDeduction.ShortName,
                        IsAttendanceDependent = EarnDeduction.IsAttendanceDependent,
                        IsEarning = EarnDeduction.IsEarning,
                        HasFormula = EarnDeduction.HasFormula
                    }).ToList();
        }

        /// <summary>
        /// This method is sued to return formula associated with user.
        /// </summary>
        /// <param name="aoEDFormula"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public string GetFormula(EarningsDeductions aoEDFormula, int aiUserId)
        {
            // Check earning deduction associated with the respective user.
            List<string> lstUserEDFormula = (from UsersED in this.mlstUsersFormulaeAndRanges
                                           join Formula in this.EarningsDeductionsFormulae
                                           on UsersED.FormulaRangeId equals Formula.FormulaId
                                           where Formula.EarningsDeductionsId == aoEDFormula.EarningsDeductionsId &&
                                                 UsersED.UserId == aiUserId &&
                                                 UsersED.IsFormula == true
                                           select Formula.Formula).ToList();

            string sFormula = string.Empty;

            // if formula is associated ten use it otherwise use default formula.
            if (lstUserEDFormula.Count() > 0)
                sFormula = lstUserEDFormula.First();
            else
                sFormula = aoEDFormula.Formula.ToString();

            sFormula = sFormula.Replace(",", string.Empty);
            sFormula = sFormula.Replace("%", "/100");

            return sFormula;
        }

        /// <summary>
        /// This method is used to update formula with value zero.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="sFormula"></param>
        /// <returns></returns>
        public string GetUpdatedEDFormulaForZeroIDs(int aiStaffGroupId, string asFormula, List<EarningsDeductions> alstEarningDeduction)
        {
            List<int> lstEarningDeductions = alstEarningDeduction.Select(ed => ed.EarningsDeductionsId).ToList();

            // replace each earning deduction id with zero if respective ED is not associated with respective staff group.
            foreach (int iEarnDeductId in lstEarningDeductions)
                asFormula = asFormula.Replace("'" + iEarnDeductId + "'", "0");

            asFormula = asFormula.Replace("'", string.Empty);
            return asFormula;
        } 

        #endregion
    }
}
