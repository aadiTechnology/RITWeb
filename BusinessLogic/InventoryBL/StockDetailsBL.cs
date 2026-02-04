// File Name    : StockDetailsDC.cs
// Created By   : Sanket Bhujbal
// Crested Date : 26-Dec-2015 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using NewStockDetails;
using Utility;
using System.Data;
namespace BusinessLogic
{    
    public class StockDetailsBL
    {
        #region "Data Members"

        private StockDetailsDC moStockDetailsDC;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockDetailsBL()
        { }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        public StockDetailsBL(int aiSchoolId,int aiUserId)
        {
            moStockDetailsDC = new StockDetailsDC(aiSchoolId,aiUserId);
        }

        #endregion
        private StockDetailsDC moItemsMasterDC = new StockDetailsDC();
        #region "Methods"

        /// <summary>
        /// This Method is used to save item stock details.
        /// </summary>
        /// <param name="aStockDetails"></param>
        public void Save(StockDetails aoStockDetails, int aiId)
        {
            moStockDetailsDC.Save(aoStockDetails, aiId);
        }
        public DataTable GetAllVendor(int miSchoolId, int miAcademicYearId)
        {
            return moItemsMasterDC.GetAllVendor(miSchoolId, miAcademicYearId);
        }
        /// <summary>
        /// This method is used to return entity list of Stock Item Details.
        /// </summary>
        /// <param name="aiItemId"></param>
        /// <returns></returns>
        public StockItemDetails GetStockItemDetails(int aiItemId)
        {
            return moStockDetailsDC.GetStockItemDetails(aiItemId);
        }

        /// <summary>
        /// This method isused to get all stock details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiItemId"></param>
        /// <returns></returns>
        public List<StockDetails> GetAll(string aiItemId, int aiSchoolId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "NewStockDate";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }
            asSortExpression = asSortExpression + " " + asSortDirection;       
            int iEndIndex = startRowIndex + maximumRows;        
            StockDetailsDC moStockDetailsDC = new StockDetailsDC();
            return moStockDetailsDC.GetAll(Convert.ToInt32(aiItemId), aiSchoolId, asSortExpression, startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is used to count number of items record.
        /// </summary>
        /// <param name="aiItemId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <returns></returns>
        public int Count(string aiItemId, int aiSchoolId, string asSortExpression, string asSortDirection)
        {
            StockDetailsDC moStockDetailsDC = new StockDetailsDC();
            return moStockDetailsDC.Count(Convert.ToInt32(aiItemId), aiSchoolId);
        }

        /// <summary>
        /// This method is used to get stock details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public StockDetails Get(int aiId)
        {
            return moStockDetailsDC.Get(aiId);
        }

        /// <summary>
        /// This method is used to delete item details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            moStockDetailsDC.Delete(aiId);
        }

        #endregion
    }
}
