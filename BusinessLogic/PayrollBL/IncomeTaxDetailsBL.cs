// File Name - IncomeTaxDetailsBL.cs
// Creator - Pravin
// Created Date -

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class IncomeTaxDetailsBL
    {
        #region Constant(s)

        private const string S_SECTION_80C = "Section 80C";
        private const int I_80C_GROUP = 1;
        private const string S_MALE = "Male";

        #endregion

        #region Data Member(s)

        private IncomeTaxDetailsDC moIncomeTaxDetailsDC;        
        private int miSchoolId;
        private int miFinYearId;
        private int miUserId;
        private int miAcademicYearId;

        private List<TaxDeduction> mlstTaxDeductions;
        private List<SectionDetails> mlstSectionDetails;
        private List<InvestmentMethod> mlstInvestmentMethods;
        private List<IncomeDeclaration> mlstIncomeDeclarations;
        private List<IncomeTaxSlab> mlstIncomeTaxSlabs;
        private List<EarningDeductionAmount> mlstEarningDeductionDetails;
        private List<InvestmentDeclaration> mlstInvestmentDeclaration;
        private List<ITSlabCategory> mlstITSlabCategories;
        private List<UserAgeDetails> mlstUserAgeDetails;
        private List<AdditionalPaymentDetails> mlstAdditionalPayments;
        private List<TaxReliefParameters> mlstTaxReliefParameters;
        private int miIncomeTaxDetailsCount;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        public IncomeTaxDetailsBL()
        {
            this.moIncomeTaxDetailsDC = new IncomeTaxDetailsDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public IncomeTaxDetailsBL(int aiSchoolId, int aiFinYearId, int aiUserId, int aiAcademicYearId)
        {
            this.moIncomeTaxDetailsDC = new IncomeTaxDetailsDC(aiSchoolId, aiFinYearId, aiUserId, aiAcademicYearId);
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miFinYearId = aiFinYearId;
            this.miUserId = aiUserId;
        }

        #endregion

        #region Enum

        public enum ChapterVIACategory
        {
            A = 1,
            B = 2
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all investment methods according to selected page.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns>Entity list of income tax details</returns>
        public List<IncomeTaxDetails> GetAll(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStaffGroupId, string asSearchName, int maximumRows, int startRowIndex)
        {   
            if (asSearchName == null)
                asSearchName = string.Empty;
            int iIncomeTaxDetailsCount = 0;
            List<IncomeTaxDetails> lst = IncomeTaxDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, aiFinancialYearId, aiStaffGroupId, asSearchName, 9999, startRowIndex, out iIncomeTaxDetailsCount);
            miIncomeTaxDetailsCount = iIncomeTaxDetailsCount;
            return lst;
        }

        /// <summary>
        /// This method is used to return total count of record.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns>Income tax count</returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStaffGroupId, string asSearchName, int maximumRows, int startRowIndex)
        {
            return miIncomeTaxDetailsCount;
        }

        /// <summary>
        /// This method is used to publish the income Tax details details.
        /// </summary>
        /// <param name="aoTaxDeductionDetails"></param>
        public void Publish(bool abIsPublish)
        {
            moIncomeTaxDetailsDC.Publish(abIsPublish);
        }

        /// <summary>
        /// This method rturns true if income tax details alreday get published.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public bool CheckIsPublished(int aiUserId = 0)
        {
            return moIncomeTaxDetailsDC.CheckIsPublished(aiUserId);
        }

        #endregion

        #region Income Tax Calculation Method(s)

        /// <summary>
        /// This method is used to calculate income tax amount.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public string GetIncomeTaxAmount(string asUserIds)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            ReadIncomeTaxRelatedDetails(asUserIds);            
            List<string> lstUserIds = asUserIds.Split(',').ToList();

            foreach (string sUserId in lstUserIds)
            {
                int iUserId = Convert.ToInt32(sUserId);
                UpdateInvestmentDeclaration(iUserId);
                decimal dcTotalIncome = GetTotalIncome(iUserId);
                decimal dcBasicDeduction = GetBasicDeductionAmount(iUserId);

                decimal dc80CMaxAmount = mlstSectionDetails.Where(sd => sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt() && sd.Name == S_SECTION_80C).Select(sd => sd.MaxAmount).FirstOrDefault();
                decimal dc80CGroupMaxAmount = mlstSectionDetails.Where(sd => sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt() && sd.GroupId == 1).Select(sd => sd.GroupMaxAmount).FirstOrDefault();
                decimal dc80CAmount = Get80CSectionAmount(dc80CMaxAmount, iUserId);
                decimal dcChapterVIAAmount = GetChapterVIAAmount(dc80CGroupMaxAmount, dc80CAmount, iUserId);
                decimal dcCategory1Total = Get80CCategoryAmount(iUserId);
                decimal oCategory2Total = GetNon80cCategoryAmount(iUserId);

                decimal dcNetIncome = dcTotalIncome - dcBasicDeduction - dcChapterVIAAmount - oCategory2Total - dcCategory1Total;

                decimal dcTaxPayable = CalculateIncomeTax(dcNetIncome, iUserId);

                decimal dcSection87Amount = GetSection87Amount(dcNetIncome);

                // deduct amount of section 87.
                if (dcTaxPayable - dcSection87Amount < 0)
                    dcTaxPayable = 0;
                else
                    dcTaxPayable = dcTaxPayable - dcSection87Amount;

                // add educational cess 3%.
                decimal dcEducationalCess = (decimal)0.03;
                dcTaxPayable = dcTaxPayable + Math.Round(dcTaxPayable * dcEducationalCess);

                decimal dcSection89Amount = GetSection89Amount(iUserId);
                
                // net tax
                dcTaxPayable = dcTaxPayable - dcSection89Amount;

                decimal dcTotalDeductedTax = GetTaxDeductionAmount(iUserId);

                // pending tax = tax payable - tax paid.
                oStringBuilder.Append("," + ((long)dcTaxPayable - dcTotalDeductedTax));                
            }

            return oStringBuilder.ToString();
        }

        /// <summary>
        /// This method is used to return section 87 amount.
        /// </summary>
        /// <param name="dcNetIncome"></param>
        /// <returns></returns>
        private decimal GetSection87Amount(decimal dcNetIncome)
        {
            var oParameter = mlstTaxReliefParameters.Where(prm => prm.FromAmount <= dcNetIncome && dcNetIncome <= prm.ToAmount).FirstOrDefault();
            if (oParameter != null)
                return oParameter.Amount;
            return (decimal)0;
        }

        /// <summary>
        /// This method is used to read income tax details from database.
        /// </summary>
        /// <param name="asUserIds"></param>
        private void ReadIncomeTaxRelatedDetails(string asUserIds)
        {
            TaxDeductionBL oTaxDeductionBL = new TaxDeductionBL(miSchoolId, miFinYearId, miUserId, miAcademicYearId);
            mlstTaxDeductions = oTaxDeductionBL.GetAll(-1, "UserId", "asc");

            SectionDetailsBL oSectionDetailsBL = new SectionDetailsBL(miSchoolId, miFinYearId, miUserId);
            mlstSectionDetails = oSectionDetailsBL.GetAll();

            InvestmentMethodBL oInvestmentMethodBL = new InvestmentMethodBL(miSchoolId, miFinYearId, miUserId, miAcademicYearId);
            mlstInvestmentMethods = oInvestmentMethodBL.GetAll();

            InvestmentDeclarationBL oInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinYearId, miUserId);
            mlstInvestmentDeclaration = oInvestmentDeclarationBL.GetAll(-1, 0, string.Empty, string.Empty);

            IncomeDeclarationBL oIncomeDeclarationBL = new IncomeDeclarationBL(miSchoolId,miFinYearId,miUserId);
            mlstIncomeDeclarations = oIncomeDeclarationBL.GetAll(-1, 0, string.Empty, string.Empty);

            IncomeTaxSlabsBL oIncomeTaxSlabsBL = new IncomeTaxSlabsBL(miSchoolId, miFinYearId, miAcademicYearId, miUserId);
            mlstIncomeTaxSlabs = oIncomeTaxSlabsBL.GetAll();
            mlstITSlabCategories = oIncomeTaxSlabsBL.GetAllCategories();

            UsersEarningsDeductionsBL oUsersEarningsDeductionsBL = new UsersEarningsDeductionsBL(miSchoolId, miAcademicYearId, miUserId);
            mlstEarningDeductionDetails = oUsersEarningsDeductionsBL.GetEarningDeductionDetails(miFinYearId,asUserIds);
            mlstUserAgeDetails = oUsersEarningsDeductionsBL.GetUserAgeDetails(asUserIds);

            AdditionalPaymentBL oAdditionalPaymentBL = new AdditionalPaymentBL(miSchoolId, miFinYearId, miUserId);
            mlstAdditionalPayments = oAdditionalPaymentBL.GetAll();

            mlstTaxReliefParameters = moIncomeTaxDetailsDC.GetAllTaxReliefDetails();
        }

        /// <summary>
        /// This method is used to return deducted tax amount.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        private decimal GetTaxDeductionAmount(int aiUserId)
        {   
            decimal dcAmount = 0;

            var user = mlstTaxDeductions.Where(td => td.UserId == aiUserId);
            var oAmount = mlstTaxDeductions.Where(td => td.UserId == aiUserId).GroupBy(td => td.UserId).Select(td => new {UserId = td.Key, Amount = td.Sum(tdx => tdx.TaxDeductionAmount)}).FirstOrDefault();
            if (oAmount != null)
                dcAmount = oAmount.Amount;
            return dcAmount;
        }

        /// <summary>
        /// This method is used to return section 89 amount.
        /// </summary>        
        /// <returns></returns>
        private decimal GetSection89Amount(int aiUserId)
        {   
            var oSection89Amount = (from itd in mlstInvestmentDeclaration
                                    join im in mlstInvestmentMethods
                                    on itd.InvestmentMethodId equals im.Id
                                    join sd in mlstSectionDetails
                                    on im.SectionId equals sd.Id
                                    where sd.SectionGroupId == Constants.SectionGroups.Group89.ToInt()
                                    && itd.UserId == aiUserId
                                    group itd by itd.UserId into itdm
                                    select new
                                    {
                                        Amount = itdm.Sum(itd1 => itd1.Amount)
                                    }

                                   ).FirstOrDefault();

            decimal dcSection89Amount = 0;
            if (oSection89Amount != null)
                dcSection89Amount = oSection89Amount.Amount;

            return dcSection89Amount;
        }

        /// <summary>
        /// This method is used to calculate income tax according to given amount.
        /// </summary>
        /// <param name="adcNetIncome"></param>
        /// <returns></returns>
        private decimal CalculateIncomeTax(decimal adcNetIncome, int aiUserId)
        {   
            UserAgeDetails oUserAgeDetails = mlstUserAgeDetails.Where(user => user.UserId == aiUserId).FirstOrDefault();
            List<ITSlabCategory> lstCategories = mlstITSlabCategories.Where(ct => ct.FromAge <= oUserAgeDetails.Age && ct.UptoAge >= oUserAgeDetails.Age).ToList();

            int iCategoryId = 0;
            if (lstCategories.Count == 1)
                iCategoryId = lstCategories.First().Id;
            else if (lstCategories.Count > 1)
            {
                if (oUserAgeDetails.SalutationId == Constants.Salutation.Mr.ToInt())
                    iCategoryId = lstCategories.Where(ct => ct.Name == S_MALE).FirstOrDefault().Id;
                else
                    iCategoryId = lstCategories.Where(ct => ct.Name != S_MALE).FirstOrDefault().Id;
            }

            IncomeTaxSlab oCurrentIncomeTaxSlab = mlstIncomeTaxSlabs.Where(itr => adcNetIncome >= itr.FromAmount && adcNetIncome <= itr.ToAmount && itr.Category.Id == iCategoryId).FirstOrDefault();
            decimal dcTaxPayable = oCurrentIncomeTaxSlab.FixedAmount + Math.Round((adcNetIncome - oCurrentIncomeTaxSlab.FromAmount + 1) * ((decimal)(oCurrentIncomeTaxSlab.Percentage / 100)));
            return dcTaxPayable;
        }

        /// <summary>
        /// This method is used to return non 80C category amount.
        /// </summary>        
        /// <returns></returns>
        private decimal GetNon80cCategoryAmount(int aiUserId)
        {            
            var oCategory2Details = (from itd in mlstInvestmentDeclaration
                                     join im in mlstInvestmentMethods
                                     on itd.InvestmentMethodId equals im.Id
                                     join sd in mlstSectionDetails
                                     on im.SectionId equals sd.Id
                                     where sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt()
                                     && sd.CategoryId == ChapterVIACategory.B.ToInt()
                                     && itd.UserId == aiUserId
                                     select new
                                     {
                                         UserId = itd.UserId,
                                         Section = sd.Id,
                                         Amount = itd.Amount > sd.GroupMaxAmount && sd.GroupMaxAmount != 0 ? sd.GroupMaxAmount : itd.Amount
                                     }

                                  ).ToList();

            var oCategory2Total = oCategory2Details.Sum(amt => amt.Amount);
            return oCategory2Total;
        }

        /// <summary>
        /// This method is used to calculate total of 80C category sections except 80C sections.
        /// </summary>        
        /// <returns></returns>
        private decimal Get80CCategoryAmount(int aiUserId)
        {            
            var oCategory1Total = (from itd in mlstInvestmentDeclaration
                                   join im in mlstInvestmentMethods
                                   on itd.InvestmentMethodId equals im.Id
                                   join sd in mlstSectionDetails
                                   on im.SectionId equals sd.Id
                                   where sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt()
                                   && sd.GroupId != I_80C_GROUP
                                   && sd.CategoryId == ChapterVIACategory.A.ToInt()
                                   && itd.UserId == aiUserId
                                   group itd by itd.UserId into itdm
                                   select new
                                   {
                                       Amount = itdm.Sum(itd1 => itd1.Amount)
                                   }

                       ).FirstOrDefault();

            decimal dcCategory1Total = 0;
            if (oCategory1Total != null)
                dcCategory1Total = oCategory1Total.Amount;
            return dcCategory1Total;
        }

        /// <summary>
        /// This cmethod is used to return chapter VIA total amount.
        /// </summary>        
        /// <param name="adc80CGroupMaxAmount"></param>
        /// <param name="adc80CAmount"></param>
        /// <returns></returns>
        private decimal GetChapterVIAAmount(decimal adc80CGroupMaxAmount, decimal adc80CAmount, int aiUserId)
        {   
            var o80CGroupAmount = (from itd in mlstInvestmentDeclaration
                                   join im in mlstInvestmentMethods
                                   on itd.InvestmentMethodId equals im.Id
                                   join sd in mlstSectionDetails
                                   on im.SectionId equals sd.Id
                                   where sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt()
                                   && sd.GroupId == I_80C_GROUP
                                   && sd.Name != S_SECTION_80C
                                   && itd.UserId == aiUserId
                                   group itd by itd.UserId into itdm
                                   select new
                                   {
                                       Amount = itdm.Sum(itd1 => itd1.Amount)
                                   }

                       ).FirstOrDefault();

            decimal dc80CGroupAmount = 0;
            if (o80CGroupAmount != null)
                dc80CGroupAmount = o80CGroupAmount.Amount;

            // If group total is greater than group max amount then consider group max amount.
            decimal dcChapterVIAAmount = adc80CAmount + dc80CGroupAmount;
            if (dcChapterVIAAmount > adc80CGroupMaxAmount)
                dcChapterVIAAmount = adc80CGroupMaxAmount;
            return dcChapterVIAAmount;
        }

        /// <summary>
        /// This method is used to return 80C section amount.
        /// </summary>        
        /// <param name="adc80CMaxAmount"></param>
        /// <returns></returns>
        private decimal Get80CSectionAmount(decimal adc80CMaxAmount, int aiUserId)
        {
            var o80CAmount = (from itd in mlstInvestmentDeclaration
                              join im in mlstInvestmentMethods
                              on itd.InvestmentMethodId equals im.Id
                              join sd in mlstSectionDetails
                              on im.SectionId equals sd.Id
                              where sd.SectionGroupId == Constants.SectionGroups.DeductionUnderChapterVIA.ToInt()
                              && sd.Name == S_SECTION_80C
                              && itd.UserId == aiUserId
                              group itd by itd.UserId into itdm
                              select new
                              {
                                  Amount = itdm.Sum(itd1 => itd1.Amount)
                              }

                              ).FirstOrDefault();

            decimal dc80CAmount = 0;
            if (o80CAmount != null)
                dc80CAmount = o80CAmount.Amount;

            // If 80C section amount is greater then max amount then consider max amount.
            if (dc80CAmount > adc80CMaxAmount)
                dc80CAmount = adc80CMaxAmount;
            return dc80CAmount;
        }

        /// <summary>
        /// This method is used to reutrn basic deduction amount.
        /// </summary>        
        /// <returns></returns>
        private decimal GetBasicDeductionAmount(int aiUserId)
        {
            var oUser = (from itd in mlstInvestmentDeclaration
                         join im in mlstInvestmentMethods
                         on itd.InvestmentMethodId equals im.Id
                         join sd in mlstSectionDetails
                         on im.SectionId equals sd.Id
                         where (sd.SectionGroupId == Constants.SectionGroups.Allowance.ToInt() ||
                         sd.SectionGroupId == Constants.SectionGroups.Deduction.ToInt())
                         && itd.UserId == aiUserId
                         select new
                         {
                             im.Name,
                             im.Id,
                             itd.Amount
                         }

                         ).ToList();

            var oBasicDeduction = (from itd in mlstInvestmentDeclaration
                                   join im in mlstInvestmentMethods
                                   on itd.InvestmentMethodId equals im.Id
                                   join sd in mlstSectionDetails
                                   on im.SectionId equals sd.Id
                                   where (sd.SectionGroupId == Constants.SectionGroups.Allowance.ToInt() ||
                                   sd.SectionGroupId == Constants.SectionGroups.Deduction.ToInt())
                                   && itd.UserId == aiUserId
                                   group itd by itd.UserId into itdm
                                   select new
                                   {
                                       Amount = itdm.Sum(itd1 => itd1.Amount)
                                   }

                                   ).FirstOrDefault();

            decimal dcBasicDeduction = 0;
            if (oBasicDeduction != null)
                dcBasicDeduction = oBasicDeduction.Amount;
            return dcBasicDeduction;
        }

        /// <summary>
        /// This method is used to update investment declaration details for future months.
        /// </summary>
        /// <returns></returns>
        private void UpdateInvestmentDeclaration(int aiUserId)
        {
            List<EarningDeductionAmount> lstEarningDeductions = mlstEarningDeductionDetails.Where(ed => ed.UserId == aiUserId).ToList();            
            lstEarningDeductions.ForEach(
                    ed =>
                    {
                        var oInvestmentDeclaration = mlstInvestmentDeclaration.Where(id => id.InvestmentMethodId == ed.InvestmentIncomeMethodId && ed.UserId == id.UserId).FirstOrDefault();
                        if (oInvestmentDeclaration != null)
                            oInvestmentDeclaration.Amount = oInvestmentDeclaration.Amount + ed.Amount;
                    });
        }

        /// <summary>
        /// this method is used to return total income amount.
        /// </summary>
        /// <returns></returns>
        private decimal GetTotalIncome(int aiUserId)
        {
            List<EarningDeductionAmount> lstEarningDeductions = mlstEarningDeductionDetails.Where(ed => ed.UserId == aiUserId).ToList();
            List<IncomeDeclaration> lstIncomeDeclarations = mlstIncomeDeclarations.Where(id => id.UserId == aiUserId).ToList();
            lstEarningDeductions.ForEach(
                    ed =>
                    {
                        var oIncomeDeclaration = lstIncomeDeclarations.Where(id => id.InvestmentMethodId == ed.InvestmentIncomeMethodId && id.UserId == ed.UserId).FirstOrDefault();
                        if (oIncomeDeclaration != null)
                            oIncomeDeclaration.Amount = oIncomeDeclaration.Amount + ed.Amount;
                    });

            decimal dcIncome = lstIncomeDeclarations.Sum(itd => itd.Amount);

            var oAdditionalPayments = mlstAdditionalPayments.Where(pt => pt.UserId == aiUserId).ToList();
            if (oAdditionalPayments != null)
                dcIncome = dcIncome + oAdditionalPayments.Sum(pt => pt.Amount);

            return dcIncome;
        }

        #endregion
    }
}
