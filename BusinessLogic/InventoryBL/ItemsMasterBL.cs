// Class Name       :- ItemsMasterBL
// Purpose          :- This class is used to manage ItemsMaster details.
// Date Of creation :- 6/24/2009
// Author Name      :- Amit Vernekar.


using System;
using System.Data;
using System.Collections;
using DataCommunicator;
using Utility;
using SchoolEntities.Inventory;
using System.Collections.Generic;


namespace BusinessLogic
{
    public class ItemsMasterBL
    {
        #region " Constants "

            private const string S_DUPLICATE_ITEM_NAME = "Item Name already exists.";
            private const string S_DUPLICATE_ITEM_CODE = "Item Code already exists.";

        #endregion " Constants "

        #region " Constructors "

        public ItemsMasterBL()
        {
            moItemsMasterDC = new ItemsMasterDC();
        }

        public ItemsMasterBL(int aiItemId, int aiSchoolId)
        {
            moItemsMasterDC = new ItemsMasterDC(aiItemId,aiSchoolId);
            moItemsMasterStruct = moItemsMasterDC.ItemsMasterStructDetails;
        }
        
        #endregion " Constructors "

        #region " Data Members and Properties "

        #region " Data Members "

        private ItemsMasterDC.ItemsMasterStruct moItemsMasterStruct;
        private ItemsMasterDC moItemsMasterDC;

        #endregion " Data Members "

        #region " Properties "

        public int ItemID
        {
            get
            {
                return moItemsMasterStruct.miItemID;
            }
            set
            {
                moItemsMasterStruct.miItemID = value;
            }
        }

        public string ItemCode
        {
            get
            {
                return moItemsMasterStruct.msItemCode;
            }
            set
            {
                moItemsMasterStruct.msItemCode = value;
            }
        }

        public string ItemName
        {
            get
            {
                return moItemsMasterStruct.msItemName;
            }
            set
            {
                moItemsMasterStruct.msItemName = value;
            }
        }

        public string CurrentStock
        {
            get
            {
                return moItemsMasterStruct.msCurrentStock;
            }
            set
            {
                moItemsMasterStruct.msCurrentStock = value;
            }
        }

        public decimal ItemQty
        {
            get
            {
                return moItemsMasterStruct.mdItemQty;
            }
            set
            {
                moItemsMasterStruct.mdItemQty = value;
            }
        }

        public decimal ItemPrice
        {
            get
            {
                return moItemsMasterStruct.mdItemPrice;
            }
            set
            {
                moItemsMasterStruct.mdItemPrice = value;
            }
        }

        public string Make
        {
            get
            {
                return moItemsMasterStruct.msMake;
            }
            set
            {
                moItemsMasterStruct.msMake = value;
            }
        }

        public decimal ItemReorderLevelQty
        {
            get
            {
                return moItemsMasterStruct.mdItemReorderLevelQty;
            }
            set
            {
                moItemsMasterStruct.mdItemReorderLevelQty = value;
            }
        }

        public bool IsConsiderForDetailLevel
        {
            get {
                return moItemsMasterStruct.mbIsConsiderForDetailLevel;
            }
            set {
                moItemsMasterStruct.mbIsConsiderForDetailLevel = value;
            }
        }

        public int UOMID
        {
            get
            {
                return moItemsMasterStruct.miUOMID;
            }
            set
            {
                moItemsMasterStruct.miUOMID = value;
            }
        }

        public int ItemCategoryID
        {
            get
            {
                return moItemsMasterStruct.miItemCategoryID;
            }
            set
            {
                moItemsMasterStruct.miItemCategoryID = value;
            }
        }

        public string RemoveReason
        {
            get
            {
                return moItemsMasterStruct.msRemoveReason;
            }
            set
            {
                moItemsMasterStruct.msRemoveReason = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moItemsMasterStruct.miSchoolId;
            }
            set
            {
                moItemsMasterStruct.miSchoolId = value;
            }
        }

        public System.DateTime InsertDate
        {
            get
            {
                return moItemsMasterStruct.mdtInsertDate;
            }
            set
            {
                moItemsMasterStruct.mdtInsertDate = value;
            }
        }

        public int InsertedById
        {
            get
            {
                return moItemsMasterStruct.miInsertedById;
            }
            set
            {
                moItemsMasterStruct.miInsertedById = value;
            }
        }

        public System.DateTime UpdateDate
        {
            get
            {
                return moItemsMasterStruct.mdtUpdateDate;
            }
            set
            {
                moItemsMasterStruct.mdtUpdateDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moItemsMasterStruct.miUpdatedById;
            }
            set
            {
                moItemsMasterStruct.miUpdatedById = value;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return moItemsMasterStruct.mblnIsDeleted;
            }
            set
            {
                moItemsMasterStruct.mblnIsDeleted = value;
            }
        }

        public string Unit
        {
            get
            {
                return moItemsMasterStruct.msUnit;
            }
            set
            {
                moItemsMasterStruct.msUnit = value;
            }
        }

        public string ImageXml
        {
            get
            {
                return moItemsMasterStruct.msImageXml;
            }
            set
            {
                moItemsMasterStruct.msImageXml = value;
            }
        }

        public bool IsIssued
        {
            get
            {
                return moItemsMasterStruct.mbIsIssued;
            }
            set
            {
                moItemsMasterStruct.mbIsIssued = value;
            }
        }

        public int UOMPieceCount
        {
            get
            {
                return moItemsMasterStruct.miUOMPieceCount;
            }
            set
            {
                moItemsMasterStruct.miUOMPieceCount = value;
            }
        }

        public int GSTCategoryId
        {
            get
            {
                return moItemsMasterStruct.miGSTCategoryId;
            }
            set
            {
                moItemsMasterStruct.miGSTCategoryId = value;
            }
        }
        public int ConsiderUnitQuantity
        {
            get
            {
                return moItemsMasterStruct.miConsiderUnitQuantity;
            }
            set
            {
                moItemsMasterStruct.miConsiderUnitQuantity = value;
            }
        }

        public int ConsiderUnitReorderLevel
        {
            get
            {
                return moItemsMasterStruct.miConsiderUnitReorderLevel;
            }
            set
            {
                moItemsMasterStruct.miConsiderUnitReorderLevel = value;
            }
        }

        public int PieceCount
        {
            get
            {
                return moItemsMasterStruct.miPieceCount;
            }
            set
            {
                moItemsMasterStruct.miPieceCount = value;
            }
        
        }

        public decimal IssueQty
        {
            get
            {
                return moItemsMasterStruct.miIssueQty;
            }
            set
            {
                moItemsMasterStruct.miIssueQty = value;
            }
        }

        public decimal ReturnQty
        {
            get
            {
                return moItemsMasterStruct.miReturnQty;
            }
            set
            {
                moItemsMasterStruct.miReturnQty = value;
            }
        }

        public decimal CancelQty
        {
            get 
            {
                return moItemsMasterStruct.miCancelQty;
            }
            set
            {
                moItemsMasterStruct.miReturnQty = value;
            }
        }
        public string  Hall             //hall
        {
            get
            {
                return moItemsMasterStruct.msHall;
            }
            set
            {
                moItemsMasterStruct.msHall  = value;
            }
        }
        public string  RackNo             //rackno
        {
            get
            {
                return moItemsMasterStruct.msRackNo ;
            }
            set
            {
                moItemsMasterStruct.msRackNo  = value;
            }
        }

        public string ShelfNo             //Shelf
        {
            get
            {
                return moItemsMasterStruct.msShelfNo;
            }
            set
            {
                moItemsMasterStruct.msShelfNo = value;
            }
        }
        public string InvoiceNo   //invoice no
        {
            get
            {
                return moItemsMasterStruct.msInvoiceNo;
            }
            set
            {
                moItemsMasterStruct.msInvoiceNo = value;
            }
        }
        public int VendorId             //vendor
        { 
            get
            {
                return moItemsMasterStruct.MsVendor;
            }
            set
            {
                moItemsMasterStruct.MsVendor = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members and Properties "

        #region " Public Methods "

        /// <summary>
        /// This function is used to insert the ItemsMaster Details.
        /// </summary>
        /// <returns></returns>
        public int InsertItemsMaster()
        {
            moItemsMasterDC.ItemsMasterStructDetails = moItemsMasterStruct;
            return moItemsMasterDC.InsertItemsMaster();
         }

        /// <summary>
        /// This function is used to update the ItemsMaster Details.
        /// </summary>
        public void UpdateItemsMaster()
        {
            moItemsMasterDC.ItemsMasterStructDetails = moItemsMasterStruct;
            moItemsMasterDC.UpdateItemsMaster();
        }

        /// <summary>
        /// This function is used to delete the ItemsMaster Details
        /// </summary>
        public void DeleteItemDetails()
        {
            moItemsMasterDC.ItemsMasterStructDetails = moItemsMasterStruct;
            moItemsMasterDC.DeleteItemDetails();
        }

        /// <summary>
        /// This function used to get highest item id .
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public int GetHighestPriority(int aiSchoolId)
        {
            return moItemsMasterDC.GetHighestPriority(aiSchoolId);
        }
        /// <summary>
        /// This function used to update next number .
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        //public int GetUpdateNextNumber()
        //{
        //    return moItemsMasterDC.GetUpdateNextNumber();
        //}
        /// <summary>
        /// This function used to get Unit Of Measurement and Item Category details.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public DataSet GetAddItemDetails(int aiSchoolID)
        {
            return moItemsMasterDC.GetAddItemDetails(aiSchoolID);
        }

        /// <summary>
        /// This method used to get Item Category details.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <returns></returns>
        public DataTable GetInventoryCategories(int aiSchoolID)
        {
            return moItemsMasterDC.GetInventoryCategories(aiSchoolID);
        }

        /// <summary>
        /// This function used to check duplicate item name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemName()
        {
            moItemsMasterDC.ItemsMasterStructDetails = moItemsMasterStruct;
            bool bIsDuplicate = moItemsMasterDC.IsDuplicateItemName();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_ITEM_NAME);
            return bIsDuplicate;
        }

        /// <summary>
        /// This function used to check duplicate item code.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemCode()
        {
            moItemsMasterDC.ItemsMasterStructDetails = moItemsMasterStruct;
            bool bIsDuplicate = moItemsMasterDC.IsDuplicateItemCode();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_ITEM_CODE);
            return bIsDuplicate;
        }

        /// <summary>
        /// This function used to get item code for new item.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public string GetItemCode(int aiSchoolId)
        {
            return moItemsMasterDC.GetItemCode(aiSchoolId);
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
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, bool abIsNonMoveItem, string asFromDate, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ItemName";
            }
            if (asFromDate == null)
                asFromDate = "";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moItemsMasterDC.GetAllItemDetails(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, abIsNonMoveItem, asFromDate, sortExpression, iStartIndex, iEndIndex);
        }

        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string sortExpression, int maximumRows, int startRowIndex)  //executes
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ItemName";
            }
            
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moItemsMasterDC.GetAllItemDetails(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, sortExpression, iStartIndex, iEndIndex);
        }

        
        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string sortExpression, int maximumRows, int startRowIndex, string asHall, string asRack, string asShelf, string asFromDate, bool abIsNonMoveItem)  //HRS added
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ItemName";                
            }
            if (asFromDate == null)
                asFromDate = "";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moItemsMasterDC.GetAllItemDetails(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, sortExpression, iStartIndex, iEndIndex, asHall, asRack, asShelf, asFromDate, abIsNonMoveItem);
        }

        public DataTable GetAllItemDetails(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, string sortExpression, int maximumRows, int startRowIndex, string asHall, string asRack, string asShelf)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ItemName";
            }
         
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moItemsMasterDC.GetAllItemDetails(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, sortExpression, iStartIndex, iEndIndex, asHall, asRack, asShelf, "", false);
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
            if (asFromDate == null)
                asFromDate = "";
            return moItemsMasterDC.CountItemRows(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, abIsNonMoveItem, asFromDate);
        }
        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder, bool abIsNonMoveItem, string asFromDate, string asHall, string asRack , string asShelf)//hrs & IsNon move
        {
            if (asFromDate == null)
                asFromDate = "";
            return moItemsMasterDC.CountItemRows(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, abIsNonMoveItem, asFromDate, asHall , asRack , asShelf);
        }
        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder,  string asHall, string asRack, string asShelf)//hrs & Is move
        {
            //if (asFromDate == null)
            //    asFromDate = "";
            return moItemsMasterDC.CountItemRows(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder, asHall, asRack, asShelf);
        }

        public int CountItemRows(int aiSchoolId, string asItemName, string asItemCode, string asItemCategory, bool abIsBelowReoder)
        {
            return moItemsMasterDC.CountItemRows(aiSchoolId, asItemName, asItemCode, asItemCategory, abIsBelowReoder);
        }
        /// <summary>
        /// This function used to RI check in case of item delete.
        /// </summary>
        /// <param name="aiItemID"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetDependancyForItemRemove(int aiItemID, int aiSchoolID, int aiAcademicYearId )
        {
            ArrayList oErrorMsg = new ArrayList();

            string sMessage = "";
            string sMsg = "";
            string sFlag = "False";
            int iParentId = Convert.ToInt32(Constants.ReferenceId.Inventory);
            string sCategoryName = string.Empty;
            sMessage = ItemsMasterDC.CheckDependenciesAndGetErrorMessages(iParentId, aiItemID, sCategoryName, aiAcademicYearId, sFlag);
            if (!sMessage.Equals(""))
            {
                oErrorMsg.Add(sMessage);
            }

            if (oErrorMsg.Count != 0)
            {
                IEnumerator ie = oErrorMsg.GetEnumerator();
                while (ie.MoveNext())
                {
                    sMsg = sMsg + Convert.ToString(ie.Current) + "<BR>";
                }
                throw new BusinessLogic.Exceptions.ReferenceExceptions(sMsg);
            }
        }

        public DataTable GetAllItemDetails(int aiSchoolId, string sortExpression, int maximumRows, int startRowIndex)
        {
            string sItemName = string.Empty;
            string sItemCode = string.Empty;
            string sItemCategory = "0";
            bool bIsBelowReoder = false;
            bool bIsNonMoveItem = false;
            string sFromDate = string.Empty;
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ItemName";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moItemsMasterDC.GetAllItemDetails(aiSchoolId, sItemName, sItemCode, sItemCategory, bIsBelowReoder, bIsNonMoveItem, sFromDate, sortExpression, iStartIndex, iEndIndex);
        }

        public int CountItemRows(int aiSchoolId)
        {
            string sItemName = string.Empty;
            string sItemCode = string.Empty;
            string sItemCategory = "0";
            bool bIsBelowReoder = false;
            bool bIsNonMoveItem = false;
            string sFromDate = string.Empty;

            return moItemsMasterDC.CountItemRows(aiSchoolId, sItemName, sItemCode, sItemCategory, bIsBelowReoder, bIsNonMoveItem, sFromDate);
        }

        /// <summary>
        /// This method is used to get images URL 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ItemImageDetails> GetImagesUrl(int aiItemid)
        {
            return moItemsMasterDC.GetImagesUrl(aiItemid);
        }

        /// <summary>
        /// This method is used to delete images URL 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteFileDetails(int aiItemid,int aiControlId )
        {
            moItemsMasterDC.DeleteFileDetails(aiItemid,aiControlId);
        }


       

        #endregion " Public Methods "

        public DataTable GetAllVendor(int miSchoolId, int miAcademicYearId)
        {
            return moItemsMasterDC.GetAllVendor(miSchoolId, miAcademicYearId);
        }
    }

    public class ItemCollectionBL
    {
        #region " Data Members "

        private ItemCollectionDC moItemCollectionDC = null;

        #endregion " Data Members "

        #region " Constructors "

        public ItemCollectionBL()
        {
            moItemCollectionDC = new ItemCollectionDC();
        }
        public ItemCollectionBL(int aiSchoolId, int aiAcademicId, int aiInsertById)
        {
            moItemCollectionDC = new ItemCollectionDC(aiSchoolId, aiAcademicId, aiInsertById);
        }

        #endregion " Constructors "

        #region " Public Methods "

        public void InsertMultipleItems(string asItemDetails, bool abSetAutoCode)
        {
            moItemCollectionDC.InsertMultipleItems(asItemDetails, abSetAutoCode);
        }

        #endregion " Public Methods "
    }

}
