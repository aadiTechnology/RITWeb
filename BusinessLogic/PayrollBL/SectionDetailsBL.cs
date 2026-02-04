// File Name - SectionDetailsBL.cs
// Creator3- Sachin
// Created Date - 

using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;
using System.Runtime.Caching;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class SectionDetailsBL
    {
        #region Constants

        private readonly string S_IT_SECTIONS_KEY;
        private const string S_IT_SECTION_GROUP_KEY = "ITSectionGroupKey";

        #endregion

        #region Data Member(s)

        private SectionDetailDC moSectionDetailDC;
        private InvestmentMethodBL moInvestmentMethodBL;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDetailsBL" /> class. 
        /// </summary>
        public SectionDetailsBL()
        {
            this.moSectionDetailDC = new SectionDetailDC();
            this.moInvestmentMethodBL = new InvestmentMethodBL();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDetailsBL" /> class. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public SectionDetailsBL(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.moSectionDetailDC = new SectionDetailDC(aiSchoolId, aiFinYearId, aiUpdatedById);
            this.moInvestmentMethodBL = new InvestmentMethodBL(aiSchoolId, aiFinYearId, aiUpdatedById,0);
            this.S_IT_SECTIONS_KEY = "ITSectionKey_" + aiFinYearId;
        }

        #endregion

        #region Property(s)
        
        /// <summary>
        /// Sets Section details.
        /// </summary>
        public SectionDetails SectionDetails
        {
            set { this.moSectionDetailDC.SectionDetails = value; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all the sections.
        /// </summary>
        /// <returns>Entity list of SectionDetails</returns>
        public List<SectionDetails> GetAll()
        {
            if (!CacheManager.HasValue(this.S_IT_SECTIONS_KEY))
                CacheManager.Insert(this.S_IT_SECTIONS_KEY, this.moSectionDetailDC.GetAll());

            return CacheManager.Get(this.S_IT_SECTIONS_KEY) as List<SectionDetails>;            
        }

        /// <summary>
        /// This method is used to refresh cache data.
        /// </summary>
        private void RefreshSectionCache()
        {
            CacheManager.Insert(S_IT_SECTIONS_KEY, this.moSectionDetailDC.GetAll());
            moInvestmentMethodBL.RefreshInvestmentMethod();
        }

        /// <summary>
        /// This method is used to return all the section groups.
        /// </summary>
        /// <returns></returns>
        public List<SectionGroup> GetAllSectionGroups()
        {
            if(!CacheManager.HasValue(S_IT_SECTION_GROUP_KEY))
                CacheManager.InsertStaticData(S_IT_SECTION_GROUP_KEY,this.moSectionDetailDC.GetAllSectionGroups());
            return CacheManager.Get(S_IT_SECTION_GROUP_KEY) as List<SectionGroup>;
        }

        /// <summary>
        /// This method is used to save configuration.
        /// </summary>
        public void Save()
        {
            this.moSectionDetailDC.Save();
            this.RefreshSectionCache();
        }

        /// <summary>
        /// This method is used to delete configuration.
        /// </summary>
        /// <param name="aiSectionId"></param>
        public void Delete(int aiSectionId)
        {
            this.moSectionDetailDC.Delete(aiSectionId);
            this.RefreshSectionCache();

        }

        #endregion
    }
}
