// File Name : IncomeTaxSlabsBL.cs
// Creator : Sunny
// Created Date : 15-Mar-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;
using System.Linq;

namespace BusinessLogic
{
	/// <summary>
	///  This class is used for processing business logic and communicate with data access layer.
	/// </summary>
	public class IncomeTaxSlabsBL
	{
        #region Constant(s)
        
        private readonly string S_IT_SLABS_KEY;
        private const string S_IT_SLABS_CATEGORY_KEY = "ITSlabCategoryKey"; 

        #endregion

	    #region Data Member(s)

		private IncomeTaxSlabsDC moIncomeTaxSlabsDC;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeTaxSlabsBL" /> class. 
        /// </summary>
        public IncomeTaxSlabsBL()
        {
			this.moIncomeTaxSlabsDC = new IncomeTaxSlabsDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeTaxSlabsBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
		public IncomeTaxSlabsBL(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
        {
			this.moIncomeTaxSlabsDC = new IncomeTaxSlabsDC(aiSchoolId, aiFinYearId, aiAcademicYearId,aiUpdatedById);
            this.S_IT_SLABS_KEY = "ITSlabKey" + aiFinYearId;
        }

        #endregion        

        #region Method(s)

        /// <summary>
		/// This method is used to return all categories for ITSlab.
        /// </summary>        
		public List<ITSlabCategory> GetAllCategories()
        {
            if (!CacheManager.HasValue(S_IT_SLABS_CATEGORY_KEY))
                CacheManager.InsertStaticData(S_IT_SLABS_CATEGORY_KEY, this.moIncomeTaxSlabsDC.GetAllCategories());
            return CacheManager.Get(S_IT_SLABS_CATEGORY_KEY) as List<ITSlabCategory>;
        }

		/// <summary>
		/// This method is used to return all income tax slabs details.
		/// </summary>
		/// <returns></returns>
		public List<IncomeTaxSlab> GetAll()
		{
            if (!CacheManager.HasValue(S_IT_SLABS_KEY))
                CacheManager.Insert(S_IT_SLABS_KEY, this.moIncomeTaxSlabsDC.GetAll());
            return CacheManager.Get(S_IT_SLABS_KEY) as List<IncomeTaxSlab>;
		}

        /// <summary>
        /// This method is used to refresh IT slab cache.
        /// </summary>
        private void RefreshITSlabCache()
        {
            CacheManager.Insert(S_IT_SLABS_KEY, this.moIncomeTaxSlabsDC.GetAll());
        }

		/// <summary>
		/// This method is used to retrive income tax slab details for particular ID.
		/// </summary>
		/// <param name="aiIncomeTaxRangeId"></param>
		/// <returns></returns>
		public IncomeTaxSlab Get(int aiIncomeTaxRangeId)
		{
            List<IncomeTaxSlab> lstIncomeTaxSlabs = GetAll();
            IncomeTaxSlab oIncomeTaxSlab = lstIncomeTaxSlabs.Where(its => its.Id == aiIncomeTaxRangeId).FirstOrDefault();
            return oIncomeTaxSlab;
		}

		/// <summary>
		/// This method is used to insert/update income tax slab details. 
		/// </summary>		
		public void Save(IncomeTaxSlab aoIncomeTaxSlab)
		{
		    this.moIncomeTaxSlabsDC.Save(aoIncomeTaxSlab);
            this.RefreshITSlabCache();
		}

		/// <summary>
		/// This method is used to delete income tax slab details.
		/// </summary>		
		public void Delete(int aiIncomeTaxRangeId)
		{
		   this.moIncomeTaxSlabsDC.Delete(aiIncomeTaxRangeId);
           this.RefreshITSlabCache();
		}

		/// <summary>
		/// This method is used to get maximum To amount for given category.
		/// </summary>
		/// <param name="aiCategoryId"></param>
		/// <returns></returns>
		public int GetMaxToAmount(int aiCategoryId)
		{
		  return this.moIncomeTaxSlabsDC.GetMaxToAmount(aiCategoryId);
		}

		 #endregion    
	}
}
