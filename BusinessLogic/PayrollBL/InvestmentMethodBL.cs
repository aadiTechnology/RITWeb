// File Name - InvestmentMethodBL.cs
// Creator - Sachin
// Ceated Date - 

using System;
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;
using System.Linq;
using Utility;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class InvestmentMethodBL
    {
        #region Constants

        private readonly string S_IT_INVESTMENT_METHOD_KEY;

        #endregion

        #region Data Member(s)

        private InvestmentMethodDC moInvestmentMethodDC;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentMethodBL" /> class. 
        /// </summary>
        public InvestmentMethodBL()
        {
            this.moInvestmentMethodDC = new InvestmentMethodDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentMethodBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public InvestmentMethodBL(int aiSchoolId, int aiFinYearId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moInvestmentMethodDC = new InvestmentMethodDC(aiSchoolId, aiFinYearId, aiUpdatedById, aiAcademicYearId);            
            this.S_IT_INVESTMENT_METHOD_KEY = "InvestmentMethodKey" + aiFinYearId;
        }

        #endregion

        #region Property(s)

        /// <summary>
        /// Sets Investment Method.
        /// </summary>
        public InvestmentMethod InvestmentMethod
        {
            set { this.moInvestmentMethodDC.InvestmentMethod = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to return all investment methods according to selected page.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns>Entity list of Investment Method</returns>
        public List<InvestmentMethod> GetAll(int aiSchoolId, int aiFinYearId, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            if (!CacheManager.HasValue("InvestmentMethodKey"+aiFinYearId))
                CacheManager.Insert("InvestmentMethodKey" + aiFinYearId, this.moInvestmentMethodDC.GetAll(aiSchoolId, aiFinYearId, sortExpression, 9999, 0));

            List<InvestmentMethod> lstInvestmentMethods = CacheManager.Get("InvestmentMethodKey" + aiFinYearId) as List<InvestmentMethod>;
            if (sortExpression.Contains("MethodName"))
            {
                if (sortDirection == Constants.S_ASCENDING)
                    lstInvestmentMethods = lstInvestmentMethods.OrderBy(im => im.Name).ToList();
                else
                    lstInvestmentMethods = lstInvestmentMethods.OrderByDescending(im => im.Name).ToList();
            }
            else if (sortExpression.Contains("SectionName") || sortExpression == string.Empty)
            {
                if (sortDirection == Constants.S_ASCENDING)
                    lstInvestmentMethods = lstInvestmentMethods.OrderBy(im => im.SectionName).ToList();
                else
                    lstInvestmentMethods = lstInvestmentMethods.OrderByDescending(im => im.SectionName).ToList();
            }

            return lstInvestmentMethods;
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
        /// <returns>Investment method count</returns>
        public int Count(int aiSchoolId, int aiFinYearId, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {   
            return CacheManager.HasValue("InvestmentMethodKey" + aiFinYearId) ? (CacheManager.Get("InvestmentMethodKey" + aiFinYearId) as List<InvestmentMethod>).Count : 0;
        }

        /// <summary>
        /// This method is used to return all investment methods.
        /// </summary>
        /// <returns>Entity list of Investment Method</returns>
        public List<InvestmentMethod> GetAll()
        {
            if (!CacheManager.HasValue(this.S_IT_INVESTMENT_METHOD_KEY))
                CacheManager.Insert(this.S_IT_INVESTMENT_METHOD_KEY, this.moInvestmentMethodDC.GetAll());

            return CacheManager.Get(this.S_IT_INVESTMENT_METHOD_KEY) as List<InvestmentMethod>;
        }

        /// <summary>
        /// This method is used to refresh investment method cache.
        /// </summary>
        public void RefreshInvestmentMethod()
        {
            CacheManager.Insert(this.S_IT_INVESTMENT_METHOD_KEY, this.moInvestmentMethodDC.GetAll());
        }

        /// <summary>
        /// This method is used to update configuration.
        /// </summary>
        public void Update()
        {
            this.moInvestmentMethodDC.Update();
            this.RefreshInvestmentMethod();
        }

        #endregion
    }
}
