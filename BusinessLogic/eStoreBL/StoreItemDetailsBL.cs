using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;
using DataCommunicator.eStoreDC;
using SchoolEntities.eStore;

namespace BusinessLogic.eStoreBL
{
    public class StoreItemDetailsBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private StoreItemDetailsDC moStoreItemDetails = null;

        #endregion

        #region Constructor(s)

        public StoreItemDetailsBL()
        {
            moStoreItemDetails = new StoreItemDetailsDC();
        }

        public StoreItemDetailsBL(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            moStoreItemDetails = new StoreItemDetailsDC(aiSchoolId, aiUserId, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get store item categories in dropdown.
        /// </summary>
        /// <returns></returns>
        public List<StoreItemCategory> GetStoreItemCategories()
        {
            return moStoreItemDetails.GetStoreItemCategories();
        }

        /// <summary>
        /// This method is used get standards to fill checkbox list.
        /// </summary>
        /// <returns></returns>
        public List<StandardList> GetStandardList()
        {
            return moStoreItemDetails.GetStandardList();
        }

        /// <summary>
        /// This method is used to save item details.
        /// </summary>
        /// <param name="aoStoreItemDetails"></param>
        public int Save(StoreItemDetails aoStoreItemDetails)
        {
            return moStoreItemDetails.Save(aoStoreItemDetails);
        }

        /// <summary>
        /// This method is used to get store item details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public StoreItemDetails GetStoreItemDetails(int aiId)
        {
            return moStoreItemDetails.GetStoreItemDetails(aiId);
        }

        public string Validate(string asTitle, int aiId, int aiSchoolId, int aiAcademicYearId, int aiTypeId, string asItemCode)
        {
            return moStoreItemDetails.Validate(asTitle, aiId, aiSchoolId, aiAcademicYearId, aiTypeId, asItemCode);
        } 

        #endregion
    }
}
