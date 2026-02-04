// File Name - TaxDeductionBL.cs
// Creator - Pravin
// Created Date -

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class TaxDeductionBL
    {
        #region Data Member(s)

        private TaxDeductionDC moTaxDeductionDC;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        public TaxDeductionBL()
        {
            this.moTaxDeductionDC = new TaxDeductionDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentDeclarationBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public TaxDeductionBL(int aiSchoolId, int aiFinYearId, int aiUserId,int aiAcademicYearId)
        {
            this.moTaxDeductionDC = new TaxDeductionDC(aiSchoolId, aiFinYearId, aiUserId,aiAcademicYearId);
        } 

        #endregion

        #region Public Method(s)        

        /// <summary>
        /// This method is used to return all the Tax deduction of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSectionId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <returns>List<InvestmentDeclaration></returns>
        public List<TaxDeduction> GetAll(int aiUserId,string asSortExpression,string asSortDirection)
        {
             return this.moTaxDeductionDC.GetAll(aiUserId,asSortExpression,asSortDirection);
        }

        /// <summary>
        /// This method is used to save the Tax deduction details.
        /// </summary>
        /// <param name="aoTaxDeductionDetails"></param>
        public void Save(TaxDeduction aoTaxDeductionDetails)
        {
            this.moTaxDeductionDC.Save(aoTaxDeductionDetails);
        }

        /// <summary>
        /// This method is used to get all the quarters.
        /// </summary>
        /// <returns></returns>
        public List<Quarter> GetAllQuarters()
        {
            return this.moTaxDeductionDC.GetAllQuarters();
        }

        /// <summary>
        /// This method is used to get Tax deducotr details.
        /// </summary>
        /// <returns></returns>
        public TaxDeductorDetails GetTaxDeductorDetails()
        {
            return this.moTaxDeductionDC.GetTaxDeductorDetails();
        }

        /// <summary>
        /// This method is used to return all the users.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetPayrollUsers(int aiStaffGroupId)
        {
            return this.moTaxDeductionDC.GetPayrollUsers(aiStaffGroupId);
        }

        /// <summary>
        /// This method is used to get the CIT details.
        /// </summary>
        /// <returns></returns>
        public ITCommissionerDetails GetCITDetails()
        {
            return this.moTaxDeductionDC.GetCITDetails();
        }

        /// <summary>
        /// This method is used to save CIT details.
        /// </summary>
        /// <param name="oITCommissionerDetails"></param>
        public void SaveCITDetails(ITCommissionerDetails oITCommissionerDetails)
        {
            this.moTaxDeductionDC.SaveCITDetails(oITCommissionerDetails);
        }

        /// <summary>
        /// This method is used to save quarter details.
        /// </summary>
        /// <param name="saQuarterXML"></param>
        public void SaveQuarters(string asQuarterXML)
        {
            this.moTaxDeductionDC.SaveQuarters(asQuarterXML);
        }

        /// <summary>
        /// This method is used to tax deductor details.
        /// </summary>
        /// <param name="aoTaxDeductorDetails"></param>
        public void SaveTaxDeductorDetails(TaxDeductorDetails aoTaxDeductorDetails)
        {
            this.moTaxDeductionDC.SaveTaxDeductorDetails(aoTaxDeductorDetails);
        }

        #endregion

    }
}
