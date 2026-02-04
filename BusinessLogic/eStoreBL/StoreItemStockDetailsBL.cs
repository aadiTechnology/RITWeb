using System.Collections.Generic;
using DataCommunicator.eStoreDC;
using Utility;
using SchoolEntities.eStore;

namespace BusinessLogic.eStoreBL
{
    public class StoreItemStockDetailsBL
    {
        #region Data member(s)

        private int miTotalRows;
        private StoreItemStockDetailsDC moStoreItemStockDetailsDC;

        #endregion

        #region Constructor(s)

        public StoreItemStockDetailsBL()
        {
            moStoreItemStockDetailsDC = new StoreItemStockDetailsDC();
        }

        public StoreItemStockDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moStoreItemStockDetailsDC = new StoreItemStockDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save store item stock details.
        /// </summary>
        /// <param name="aoStoreItemStockDetails"></param>
        public void Save(StoreItemStockMaster aoStoreItemStockMaster)
        {
            moStoreItemStockDetailsDC.Save(aoStoreItemStockMaster);
        }

        /// <summary>
        /// This method is used to Get All store item stock details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiItemMasterId"></param>
        /// <param name="aiItemVariationDetailId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<StoreItemStockMaster> GetAll(int aiSchoolId, int aiItemMasterId, int aiItemVariationDetailId, string asFilter, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (asFilter == null || asFilter == "default")
                asFilter = string.Empty;
            
            if (asSortExpression == null || asSortExpression == string.Empty)
                asSortExpression = "Date DESC";
            else
            {
                asSortExpression = asSortExpression.ToLower().Replace(" desc", string.Empty).Replace(" asc", string.Empty);
                asSortExpression = asSortExpression + " " + asSortDirection;
            }

            maximumRows = startRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<StoreItemStockMaster> lstStoreItemStockDetails = moStoreItemStockDetailsDC.GetAll(aiSchoolId, aiItemMasterId, aiItemVariationDetailId, asFilter, asSortExpression, startRowIndex, iEndIndex);

            if (lstStoreItemStockDetails.Count > 0)
                miTotalRows = lstStoreItemStockDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstStoreItemStockDetails;
        }

        /// <summary>
        /// This method is used to get count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiItemMasterId"></param>
        /// <param name="aiItemVariationDetailId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiItemMasterId, int aiItemVariationDetailId,string asFilter, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }

        /// <summary>
        /// This method is used to get details.
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public StoreItemStock Get(int aiId)
        {
            return moStoreItemStockDetailsDC.Get(aiId);
        }

        /// <summary>
        /// This method is used to delete store item stock details.
        /// </summary>
        /// <param name="iId"></param>
        public void Delete(int aiId)
        {
            moStoreItemStockDetailsDC.Delete(aiId);
        }

        #endregion
    }
}
