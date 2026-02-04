using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utility;
using SchoolEntities.eStore;

namespace DataCommunicator.eStoreDC
{
    public class StoreItemStockDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public StoreItemStockDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public StoreItemStockDetailsDC()
        {
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save store item stock details.
        /// </summary>
        /// <param name="aoStoreItemStockDetails"></param>
        public void Save(StoreItemStockMaster aoStoreItemStockMaster)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AdjustedAmount", aoStoreItemStockMaster.AdjustedAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("TransportAmount", aoStoreItemStockMaster.TransportAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("TotalPrice", aoStoreItemStockMaster.TotalAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("StockDetails", aoStoreItemStockMaster.StockDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("NetPrice", aoStoreItemStockMaster.NetPrice, SqlDbType.Float);
                oSQLServerDbUtility.AddParameter("Date", aoStoreItemStockMaster.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Description", aoStoreItemStockMaster.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StockMasterId", aoStoreItemStockMaster.StockMasterId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStoreItemStockDetails");
            }
        }

        /// <summary>
        /// This method is used to Get All store item stock details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiItemMasterId"></param>
        /// <param name="aiItemVariationDetailId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public List<StoreItemStockMaster> GetAll(int aiSchoolId, int aiItemMasterId, int aiItemVariationDetailId, string asFilter, string asSortExpression, int iStartIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemMasterId", aiItemMasterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemVariationDetailId", aiItemVariationDetailId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                List<StoreItemStockMaster> lstStoreItemStockDetails = new List<StoreItemStockMaster>();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllNewStoreItemStockDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        StoreItemStockMaster oStoreItemStockDetails = new StoreItemStockMaster
                        {
                            StockMasterId = oSqlDataReader["Id"].ToInt(),
                            Date = oSqlDataReader["Date"].ToDateTime(),
                            TotalAmount = oSqlDataReader["TotalPrice"].ToDecimal(),
                            TransportAmount = oSqlDataReader["TransportAmount"].ToDecimal(),
                            AdjustedAmount = oSqlDataReader["AdjustedAmount"].ToDecimal(),
                            NetPrice = oSqlDataReader["NetPrice"].ToDecimal(),
                            TotalRows = oSqlDataReader["TotalRows"].ToInt(),
                        };

                        lstStoreItemStockDetails.Add(oStoreItemStockDetails);
                    }
                }
                return lstStoreItemStockDetails;
            }
        }

        /// <summary>
        /// This method is used to Get store item stock details.
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public StoreItemStock Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);

                StoreItemStock oStoreItemStock = new StoreItemStock();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStoreItemStockDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oStoreItemStock.StockMaster  = new StoreItemStockMaster
                        {
                            StockMasterId = oSqlDataReader["Id"].ToInt(),
                            TotalAmount = oSqlDataReader["TotalPrice"].ToDecimal(),
                            TransportAmount = oSqlDataReader["TransportAmount"].ToDecimal(),
                            AdjustedAmount = oSqlDataReader["AdjustedAmount"].ToDecimal(),
                            NetPrice = oSqlDataReader["NetPrice"].ToDecimal(),
                            Date = oSqlDataReader["Date"].ToDateTime(),
                            Description = oSqlDataReader["Description"].ToString()
                        };
                    }

                    oSqlDataReader.NextResult();

                    oStoreItemStock.StockDetails = new List<StoreItemStockDetails>();
                    StoreItemStockDetails oStoreItemStockDetails;
                    while (oSqlDataReader.Read())
                    {
                        oStoreItemStockDetails = new StoreItemStockDetails
                        {
                            Id = oSqlDataReader["Id"].ToInt(),
                            Color = oSqlDataReader["Color"].ToString(),
                            Discount = oSqlDataReader["Discount"].ToDecimal(),
                            GST = oSqlDataReader["GST"].ToString(),
                            GSTCategoryId = oSqlDataReader["GSTCategoryId"].ToInt(),
                            ItemCode = oSqlDataReader["ItemCode"].ToString(),
                            ItemMasterId = oSqlDataReader["ItemMasterId"].ToInt(),
                            ItemVariationDetailId = oSqlDataReader["ItemVariationDetailId"].ToInt(),
                            MRP = oSqlDataReader["MRP"].ToDecimal(),
                            NewQuantity = oSqlDataReader["NewQuantity"].ToInt(),
                            Price = oSqlDataReader["Price"].ToDecimal(),
                            Size = oSqlDataReader["Size"].ToString(),
                            Title = oSqlDataReader["Title"].ToString(),
                            UOM = oSqlDataReader["UOM"].ToString()                            
                        };

                        oStoreItemStock.StockDetails.Add(oStoreItemStockDetails);
                    }
                }
                return oStoreItemStock;
            }
        }

        /// <summary>
        /// This method is used to Delete store item stock details.
        /// </summary>
        /// <param name="iId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStoreItemStockDetails");
            }
        }

        #endregion
    }
}

