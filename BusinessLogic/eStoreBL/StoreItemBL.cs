using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using System.Data;
using SchoolEntities.eStore;

namespace BusinessLogic
{
    public class StoreItemBL
    {
        #region DataMember
     
        private StoreItemDC moStoreItemDC = null;

        private int miRowCount;

        #endregion

        #region Constructor

        public StoreItemBL()
        {
            moStoreItemDC = new StoreItemDC();
        }

        public StoreItemBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moStoreItemDC = new StoreItemDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }
            
        #endregion

        #region Methods

        public DataTable GetStoreCategories()
        {
            return moStoreItemDC.GetStoreCategories();
        }

        public int CountStoreItem(int aiSchoolId, int aiAcademicYearId, int aiStoreCategory, string asStandardIds, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            return miRowCount;
        }

        public DataTable GetStoreItemList(int aiSchoolId, int aiAcademicYearId, int aiStoreCategory, string asStandardIds, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Title";
            }
            if (asFilter == null)
                asFilter = string.Empty;

            if (asStandardIds == null)
                asStandardIds = string.Empty;

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            
            DataTable oDt = moStoreItemDC.GetStoreItemList(aiSchoolId, aiAcademicYearId, aiStoreCategory, asStandardIds, asFilter, sortExpression, startRowIndex, iEndIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                miRowCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }
        
        public void DeleteItem(int aiId)
        {
            moStoreItemDC.DeleteItem(aiId);
        }

        #endregion
    }
}
