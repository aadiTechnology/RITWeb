// Class Name       :- ItemsMasterDC
// Purpose          :- This class is used to manage ItemsMaster details.
// Date Of creation :- 6/24/2009


using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using SchoolEntities.Inventory;
using System.Collections.Generic;


namespace DataCommunicator
{
    public class ItemsMasterDC
    {
        #region " Constants and Structures "

        #region " Structures "

        public struct ItemsMasterStruct
        {

            public int miItemID;

            public string msItemCode;

            public string msItemName;

            public string msCurrentStock;

            public decimal mdItemQty;

            public decimal mdItemPrice;

            public string msMake;

            public decimal mdItemReorderLevelQty;

            public int miUOMID;

            public int miItemCategoryID;

            public string msRemoveReason;

            public bool mbIsConsiderForDetailLevel;

            public int miSchoolId;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mblnIsDeleted;

            public string msUnit;

            public string msImageXml;

            public bool mbIsIssued;

            public int miUOMPieceCount;

            public int miConsiderUnitQuantity;

            public int miConsiderUnitReorderLevel;

            public int miPieceCount;

            public decimal miIssueQty;

            public decimal miReturnQty;

            public int miGSTCategoryId;

            public decimal miCancelQty;
            public string msHall;  //hall
            public string msRackNo; //RackNo
            public string msShelfNo; // shelf no
            public string msInvoiceNo; //invoice no
            public int MsVendor; //vendor
        }

        #endregion " Structures "

        #endregion " Constants and Structures "

        #region " Constuctors "

        public ItemsMasterDC() { }

        public ItemsMasterDC(int aiItemId, int aiSchoolId)
        {
            LoadItemsMasterDetails(aiItemId, aiSchoolId);
        }
        #endregion " Constuctors "

        #region " Data Members and Properties "

        #region " Data Members "
        private ItemsMasterStruct moItemsMasterStruct;

        #endregion " Data Members "

        #region " Properties "

        public ItemsMasterStruct ItemsMasterStructDetails
        {
            get
            {
                return moItemsMasterStruct;
            }
            set
            {
                moItemsMasterStruct = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members and Properties "

        #region " Public Methods "

        // This function is used to insert the ItemsMaster Details
        public int InsertItemsMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ItemCode", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemCode, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemName", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemName, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemQty", moItemsMasterStruct.mdItemQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("ItemPrice", moItemsMasterStruct.mdItemPrice, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Make", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msMake, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemReorderLevelQty", moItemsMasterStruct.mdItemReorderLevelQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("UOMID", moItemsMasterStruct.miUOMID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemCategoryID", moItemsMasterStruct.miItemCategoryID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsConsiderForDetailLevel", moItemsMasterStruct.mbIsConsiderForDetailLevel, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("School_Id", moItemsMasterStruct.miSchoolId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moItemsMasterStruct.miInsertedById, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Is_Deleted", moItemsMasterStruct.mblnIsDeleted, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Updated_By_Id", moItemsMasterStruct.miUpdatedById, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ImageXml", moItemsMasterStruct.msImageXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ConsiderUnitQuantity", moItemsMasterStruct.miConsiderUnitQuantity, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConsiderUnitReorderLevel", moItemsMasterStruct.miConsiderUnitReorderLevel, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GSTCategoryId", moItemsMasterStruct.miGSTCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Hall", moItemsMasterStruct.msHall, SqlDbType.NVarChar);  //hall
                oSQLServerDbUtility.AddParameter("RackNo", moItemsMasterStruct.msRackNo, SqlDbType.NVarChar);   //rackno
                oSQLServerDbUtility.AddParameter("ShelfNo", moItemsMasterStruct.msShelfNo, SqlDbType.NVarChar);   //shelf no
                oSQLServerDbUtility.AddParameter("InvoiceNo", moItemsMasterStruct.msInvoiceNo, SqlDbType.NVarChar); //InvoiceNo
                oSQLServerDbUtility .AddParameter ("VendorId" , moItemsMasterStruct.MsVendor ,  SqlDbType.Int ); //vendor
                SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("ItemId", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertItem");

                return Convert.ToInt32(oSqlParam.Value);
            }
        }

        /// <summary>
        /// This function used to update statement to enter new item code number.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateNextNumberStmt()
        {
            string sItemCode = StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemCode, false);
            string sUpdateStatement = string.Empty;
            bool Result;
            int iNextNumber;
            if (sItemCode.StartsWith("I"))
            {
                string sNextNumber = sItemCode.Substring(sItemCode.LastIndexOf('I') + 1, sItemCode.Length - 1);
                Result = int.TryParse(sNextNumber, out iNextNumber);
                if (Result)
                {
                    iNextNumber = iNextNumber + 1;
                    sUpdateStatement = "UPDATE" +
                                            " NextAutoCode" +
                                      " SET" +
                                            " NextNumber = " + iNextNumber +
                                      " WHERE" +
                                            " NextAutoCodeId = " + Constants.I_ONE +
                                            " AND School_Id = " + moItemsMasterStruct.miSchoolId +
                                            " AND NextNumber < " + iNextNumber;
                }
            }
            return sUpdateStatement;
        }

        // This function is used to update the ItemsMaster Details
        public void UpdateItemsMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ItemCode", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemCode, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemName", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemName, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemQty", moItemsMasterStruct.mdItemQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("ItemPrice", moItemsMasterStruct.mdItemPrice, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Make", StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msMake, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemReorderLevelQty", moItemsMasterStruct.mdItemReorderLevelQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("UOMID", moItemsMasterStruct.miUOMID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemCategoryID", moItemsMasterStruct.miItemCategoryID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsConsiderForDetailLevel", moItemsMasterStruct.mbIsConsiderForDetailLevel, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("School_Id", moItemsMasterStruct.miSchoolId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Updated_By_Id", moItemsMasterStruct.miUpdatedById, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moItemsMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Is_Deleted", moItemsMasterStruct.mblnIsDeleted, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ItemId", moItemsMasterStruct.miItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ImageXml", moItemsMasterStruct.msImageXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ConsiderUnitQuantity", moItemsMasterStruct.miConsiderUnitQuantity, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GSTCategoryId", moItemsMasterStruct.miGSTCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConsiderUnitReorderLevel", moItemsMasterStruct.miConsiderUnitReorderLevel, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Hall", moItemsMasterStruct.msHall, SqlDbType.NVarChar);  //hall
                oSQLServerDbUtility.AddParameter("RackNo", moItemsMasterStruct.msRackNo, SqlDbType.NVarChar);   //rackno
                oSQLServerDbUtility.AddParameter("ShelfNo", moItemsMasterStruct.msShelfNo, SqlDbType.NVarChar);   //shelf no
                oSQLServerDbUtility.AddParameter("InvoiceNo", moItemsMasterStruct.msInvoiceNo, SqlDbType.NVarChar);  //invoice
                oSQLServerDbUtility.AddParameter("VendorId", moItemsMasterStruct.MsVendor, SqlDbType.Int); //vendor
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertItem");
            }
        }

        // This function is used to delete the ItemsMaster Details
        public void DeleteItemDetails()
        {
            string sDeleteStatement = " Update" +
                                            " ItemsMaster" +
                                      " SET" +
                                           " Update_Date= N'" + System.DateTime.Now.ToShortDateString() + "'" +
                                           ", Updated_By_Id= " + moItemsMasterStruct.miUpdatedById +
                                           ", Is_Deleted = N'True'" +
                                      " WHERE" +
                                           " ItemID=" + moItemsMasterStruct.miItemID +
                                           "AND School_Id=" + moItemsMasterStruct.miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This function used to get heighest item id.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public int GetHighestPriority(int aiSchoolId)
        {
            string sSelectStatement = "SELECT " +
                                     "MAX(NextNumber)" +
                                      " FROM " +
                                      "NextAutoCode" +
                                     " WHERE" +
                                           " NextAutoCodeId = " + Constants.I_ONE;

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);


        }
        
        /// <summary>
        /// This function used to get Unit Of Measurement and Item Category details.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public DataSet GetAddItemDetails(int aiSchoolID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetUOMItemCategory");
            }


        }

        public DataTable GetInventoryCategories(int aiSchoolID)
        {
            string sSelectStatement = " SELECT" +
                                            " ItemCategoryID" +
                                            ", ItemCategoryName" +
                                      " FROM" +
                                            " ItemCategoryMaster" +
                                      " WHERE" +
                                            " School_Id = " + aiSchoolID +
                                            " AND " +
                                            " Is_Deleted =0 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This function get all item details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asItemName"></param>
        /// <param name="asItemCode"></param>
        /// <param name="asItemCategory"></param>
        /// <param name="abIsBelowReoder"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, bool abIsNonMoveItem, string asFromDate, string sortExpression, int iStartIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_FromDays", asFromDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedItems");
            }
        }

        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string sortExpression, int iStartIndex, int iEndIndex) //executes
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedItems");
            }
        }

        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string sortExpression, int iStartIndex, int iEndIndex, string asHall, string asRack, string asShelf, string asFromDate, bool abIsNonMoveItem) //HRS added
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder, asHall, asRack, asShelf), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_FromDays", asFromDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedItems");
            }
        }

        /// <summary>
        /// This function used create filter.
        /// </summary>
        /// <param name="asItemName"></param>
        /// <param name="asItemCode"></param>
        /// <param name="asItemCategory"></param>
        /// <param name="abIsBelowReoder"></param>
        /// <returns></returns>
        private string CreateFilter(string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder)//executes
        {
            string sFilter = string.Empty;

            if (!String.IsNullOrEmpty(asItemName))
                sFilter = sFilter + " AND ItemName LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asItemName), false) + "%'  ";
            if (!String.IsNullOrEmpty(asItemCode))
                sFilter = sFilter + " AND ItemCode LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asItemCode), false) + "%'  ";
            if (asItemCategory != "0")
                sFilter = sFilter + " AND ItemCategoryID =" + asItemCategory;
            if (abIsBelowReoder)
                sFilter = sFilter + "AND ItemQty < ItemReorderLevelQty";
            return sFilter;
        }
        private string CreateFilter(string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string asHall, string asRack, string asShelf) //HRS add
        {
            string sFilter = string.Empty;

            if (!String.IsNullOrEmpty(asItemName))
                sFilter = sFilter + " AND ItemName LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asItemName), false) + "%'  ";
            if (!String.IsNullOrEmpty(asItemCode))
                sFilter = sFilter + " AND ItemCode LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asItemCode), false) + "%'  ";
            if (asItemCategory != "0")
                sFilter = sFilter + " AND ItemCategoryID =" + asItemCategory;
            if (abIsBelowReoder)
                sFilter = sFilter + "AND ItemQty < ItemReorderLevelQty";
            if (!String.IsNullOrEmpty(asHall))
                sFilter = sFilter + " AND Hall LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asHall), false) + "%'  "; //hall
            if (!String.IsNullOrEmpty(asRack))
                sFilter = sFilter + " AND RackNo LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asRack), false) + "%'  ";//rack
            if (!String.IsNullOrEmpty(asShelf))
                sFilter = sFilter + " AND ShelfNo LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asShelf), false) + "%'  ";//shelf
            return sFilter;
        }

        /// <summary>
        /// This function used count total items in ItemMaster table. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asItemName"></param>
        /// <param name="asItemCode"></param>
        /// <param name="asItemCategory"></param>
        /// <param name="abIsBelowReoder"></param>
        /// <returns></returns>
        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, bool abIsNonMoveItem, string asFromDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_FromDays", asFromDate, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountItems");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, bool abIsNonMoveItem, string asFromDate , string asHall ,string asRack , string asShelf)  //hrs 
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder, asHall, asRack, asShelf), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_FromDays", asFromDate, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountItems");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder)  //executes
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountItems");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }
        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string asHall, string asRack, string asShelf)  //HRS added
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asItemName, asItemCode, asItemCategory, abIsBelowReoder, asHall, asRack, asShelf), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountItems");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This function used to check duplicate item name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemName()
        {
            string sWhere = "";
            bool bFlag = true;
            if (moItemsMasterStruct.miItemID != 0)
            {
                sWhere = " AND ItemID <> N'" + moItemsMasterStruct.miItemID + "'";
            }
            string sSelectStatement = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " ItemsMaster " +
                                         " WHERE " +
                                            " ItemName=N'" + StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemName, false) + "'" +
                                            " AND Is_Deleted = N'False'" +
                                            sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    bFlag = false;
            }
            return bFlag;
        }

        /// <summary>
        /// This function used to check duplicate item name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemName(string asItemName)
        {
            string sWhere = "";
            bool bFlag = true;

            string sSelectStatement = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " ItemsMaster " +
                                         " WHERE " +
                                            " ItemName=N'" + StringUtility.ReplaceSingleQuoteInString(asItemName, false) + "'" +
                                            " AND Is_Deleted = N'False'" +
                                            sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    bFlag = false;
            }
            return bFlag;
        }

        /// <summary>
        /// This function used to check duplicate item code.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemCode()
        {
            string sWhere = "";
            bool bFlag = true;
            if (moItemsMasterStruct.miItemID != 0)
            {
                sWhere = " AND ItemID <> N'" + moItemsMasterStruct.miItemID + "'";
            }
            string sSelectStatement = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " ItemsMaster " +
                                         " WHERE " +
                                            " ItemCode =N'" + StringUtility.ReplaceSingleQuoteInString(moItemsMasterStruct.msItemCode, false) + "'" +
                                            " AND Is_Deleted = N'False' " +
                                            sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    bFlag = false;
            }
            return bFlag;
        }

        /// <summary>
        /// This function used to check duplicate item code.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemCode(string asItemCode)
        {
            string sWhere = "";
            bool bFlag = true;
            string sSelectStatement = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " ItemsMaster " +
                                         " WHERE " +
                                            " ItemCode =N'" + StringUtility.ReplaceSingleQuoteInString(asItemCode, false) + "'" +
                                            " AND Is_Deleted = N'False' " +
                                            sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    bFlag = false;
            }
            return bFlag;
        }

        /// <summary>
        /// This function used to get item code for new item.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public string GetItemCode(int aiSchoolID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "SELECT dbo.[Udf_GetItemCode](" + aiSchoolID + ")";
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sStatement);
            }
        }

        /// <summary>
        /// This function used to RI check in case of item delete.
        /// </summary>
        /// <param name="aiParentId"></param>
        /// <param name="aiParentIdValue"></param>
        /// <param name="aiName"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFlag"></param>
        /// <returns></returns>
        public static string CheckDependenciesAndGetErrorMessages(int aiParentId, int aiParentIdValue, string aiName, int aiAcademicYearId, string asFlag)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_intReference_Id", aiParentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intRecord_Id", aiParentIdValue, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_strRecord_Name", aiName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_sFlag", asFlag, SqlDbType.NVarChar);
                DataSet oDs = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetReferencesAcademicYear");
                string sMessage = "";
                if (oDs.Tables[oDs.Tables.Count - 1].Rows.Count > 0)
                {
                    sMessage = oDs.Tables[oDs.Tables.Count - 1].Rows[0]["Reference"].ToString();
                }
                return sMessage;
            }

        }

        #endregion " Public Methods "

        #region " Private Methods "

        // This function is used to load the ItemsMaster Details
        private void LoadItemsMasterDetails(int aiItemId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetItemDetailsToUpdate"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["ItemID"] != DBNull.Value)
                                moItemsMasterStruct.miItemID = Convert.ToInt32(oDR["ItemID"]);
                            if (oDR["ItemCode"] != DBNull.Value)
                                moItemsMasterStruct.msItemCode = Convert.ToString(oDR["ItemCode"]);
                            if (oDR["ItemName"] != DBNull.Value)
                                moItemsMasterStruct.msItemName = Convert.ToString(oDR["ItemName"]);
                            if (oDR["ItemQty"] != DBNull.Value)
                                moItemsMasterStruct.mdItemQty = Convert.ToDecimal(oDR["ItemQty"]);
                            if (oDR["ItemPrice"] != DBNull.Value)
                                moItemsMasterStruct.mdItemPrice = Convert.ToDecimal(oDR["ItemPrice"]);
                            if (oDR["Make"] != DBNull.Value)
                                moItemsMasterStruct.msMake = Convert.ToString(oDR["Make"]);
                            if (oDR["ItemReorderLevelQty"] != DBNull.Value)
                                moItemsMasterStruct.mdItemReorderLevelQty = Convert.ToDecimal(oDR["ItemReorderLevelQty"]);
                            if (oDR["UOMID"] != DBNull.Value)
                                moItemsMasterStruct.miUOMID = Convert.ToInt32(oDR["UOMID"]);
                            if (oDR["ItemCategoryID"] != DBNull.Value)
                                moItemsMasterStruct.miItemCategoryID = Convert.ToInt32(oDR["ItemCategoryID"]);
                            if (oDR["Remove_Reason"] != DBNull.Value)
                                moItemsMasterStruct.msRemoveReason = Convert.ToString(oDR["Remove_Reason"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moItemsMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["IsConsiderForDetailLevel"] != DBNull.Value)
                                moItemsMasterStruct.mbIsConsiderForDetailLevel = Convert.ToBoolean(oDR["IsConsiderForDetailLevel"]);
                            if (oDR["ConsiderUnitQuantity"] != DBNull.Value)
                                moItemsMasterStruct.miConsiderUnitQuantity = Convert.ToInt32(oDR["ConsiderUnitQuantity"]);
                            if (oDR["ConsiderUnitReorderLevel"] != DBNull.Value)
                                moItemsMasterStruct.miConsiderUnitReorderLevel = Convert.ToInt32(oDR["ConsiderUnitReorderLevel"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moItemsMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_Id"] != DBNull.Value)
                                moItemsMasterStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moItemsMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moItemsMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moItemsMasterStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                            if (oDR["IsIssued"] != DBNull.Value)
                                moItemsMasterStruct.mbIsIssued = Convert.ToBoolean(oDR["IsIssued"]);
                            if (oDR["PieceCount"] != DBNull.Value)
                                moItemsMasterStruct.miPieceCount = Convert.ToInt32(oDR["PieceCount"]);
                            if (oDR["GSTCategoryId"] != DBNull.Value)
                                moItemsMasterStruct.miGSTCategoryId = Convert.ToInt32(oDR["GSTCategoryId"]);
                            if (oDR["Hall"] != DBNull.Value)                        //hall
                                moItemsMasterStruct.msHall = Convert.ToString(oDR["Hall"]);
                            if (oDR["RackNo"] != DBNull.Value)                        //Rack
                                moItemsMasterStruct.msRackNo = Convert.ToString(oDR["RackNo"]);
                            if (oDR["ShelfNo"] != DBNull.Value)                        //Shelf
                                moItemsMasterStruct.msShelfNo = Convert.ToString(oDR["ShelfNo"]);
                            if (oDR["InvoiceNo"] != DBNull.Value)                        //InvoiceNo
                                moItemsMasterStruct.msInvoiceNo  = Convert.ToString(oDR["InvoiceNo"]);
                            if (oDR["VendorId"] != DBNull.Value)                        //Vendor
                                moItemsMasterStruct.MsVendor = Convert.ToInt32(oDR["VendorId"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the ItemsMaster Details
        private string FetchItemsMasterDetailsFromDatabase(int aiItemId, int aiSchoolId)
        {
            string sSelectStatement = " SELECT  " +
                                            "ItemID" +
                                            ",ItemCode" +
                                            ",ItemName" +
                                            ",ItemQty" +
                                            ",ItemPrice" +
                                            ",Make" +
                                            ",ItemReorderLevelQty" +
                                            ",UOMID" +
                                            ",ItemCategoryID" +
                                            ",Remove_Reason" +
                                            ",School_Id" +
                                            ",Insert_Date" +
                                            ",Inserted_By_Id" +
                                            ",Update_Date" +
                                            ",Updated_By_Id" +
                                            ",Is_Deleted" +
                                    " FROM ItemsMaster" +
                                    " WHERE ItemID=" + aiItemId +
                                            " AND School_Id=" + aiSchoolId;
            return sSelectStatement;
        }

        /// <summary>
        /// This method is use to get Image URL
        /// </summary>
        /// <param name="aiTestTypeId"></param>
        /// <returns></returns>
        public List<ItemImageDetails> GetImagesUrl(int aiTestTypeId)
        {
            string sSelectStatementForImage = " SELECT  " +
               "ControlId" +
               ",ImageUrl" +
               " FROM  " +
               "ItemImage WITH(NOLOCK)" +
               " WHERE  " +
               "ItemId = " + aiTestTypeId +
               " AND IsDeleted =0";
            List<ItemImageDetails> oItemImage = new List<ItemImageDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatementForImage))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            ItemImageDetails oItemImageDetails = new ItemImageDetails();
                            if (oDR["ControlId"] != DBNull.Value)
                                oItemImageDetails.ControlId = Convert.ToInt32(oDR["ControlId"]);
                            if (oDR["ImageUrl"] != DBNull.Value)
                                oItemImageDetails.ImageUrl = oDR["ImageUrl"].ToString();
                            oItemImage.Add(oItemImageDetails);
                        }
                        oDR.Close();
                    }
                }
            }
            return oItemImage;
        }

        /// <summary>
        /// This method is used to delete image file
        /// </summary>
        public void DeleteFileDetails(int aiItemId, int aiControlId)
        {
            string sSelectStatement = " Update ItemImage  " +
                "Set IsDeleted = 1" +
                " WHERE  " +
                "ItemId = " + aiItemId +
                " AND ControlId =" + aiControlId;

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSelectStatement);
        }

        #endregion " Private Methods "

        public DataTable  GetAllVendor(int miSchoolId, int miAcademicYearId)
        {
            string sSelectStatement = " SELECT  " +
                                 " Id AS VendorId " +
                                 ", FirstName + ' ' + MiddleName + ' ' + LastName AS VendorName " +
                                 ", CompanyName"+
                             " FROM " +
                                  " SchoolVendorDetails " +
                             " WHERE " +
                                  " IsDeleted = 0" +
                                  " AND SchoolId = " + miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

    }
        public class ItemCollectionDC
        {
            #region " Data Members "

            private int miSchoolId;
            private int miAcadamicId;
            private int miInsertById;

            #endregion " Data Members "

            # region " Constructors "

            public ItemCollectionDC()
            {
            }

            public ItemCollectionDC(int aiSchoolId, int aiAcademicId, int aiInsertById)
            {
                miSchoolId = aiSchoolId;
                miAcadamicId = aiAcademicId;
                miInsertById = aiInsertById;
            }

            # endregion " Constructors "

            #region " Public Methods "

            /// <summary>
            /// This function is used to insert multiple items in case of item import.
            /// </summary>
            /// <param name="asItemDetails"></param>
            /// <param name="abSetAutoCode"></param>
            public void InsertMultipleItems(string asItemDetails, bool abSetAutoCode)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Inserted_By_Id", miInsertById, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("ItemDetails", asItemDetails, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("SetAutoCode", abSetAutoCode, SqlDbType.NVarChar);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddMultpleItemDetails");
                }
            }
            #endregion " Public Methods "
        }
    }





