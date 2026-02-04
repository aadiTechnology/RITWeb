// Class Name       :- StockIssueDetailsBL
// Purpose          :- This class is used to manage StockIssueDetails details.
// Date Of creation :- 7/6/2009
// Author Name      :- Amit


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using SchoolEntities.Inventory;


namespace BusinessLogic
{
    public class StockIssueDetailsBL
    {
        #region " Constuctors "

        public StockIssueDetailsBL()
        {
            moStockIssueDetailsDC = new StockIssueDetailsDC();
        }

        public StockIssueDetailsBL(int miStockIssueDetailsID)
        {
            moStockIssueDetailsDC = new StockIssueDetailsDC(miStockIssueDetailsID);
            moStockIssueDetailsStruct = moStockIssueDetailsDC.StockIssueDetailsStructDetails;
        }
        #endregion " Constuctors "

        #region " Data Members And Properties "

        #region " Data Members "

        private StockIssueDetailsDC.StockIssueDetailsStruct moStockIssueDetailsStruct;

        private StockIssueDetailsDC moStockIssueDetailsDC;

        private int miRowCount;
        private int miRowCntAppReq;

        #endregion " Data Members "

        #region " Properties "

        public virtual int StockIssueDetailsID
        {
            get
            {
                return moStockIssueDetailsStruct.miStockIssueDetailsID;
            }
            set
            {
                moStockIssueDetailsStruct.miStockIssueDetailsID = value;
            }
        }

        public virtual int ItemID
        {
            get
            {
                return moStockIssueDetailsStruct.miItemID;
            }
            set
            {
                moStockIssueDetailsStruct.miItemID = value;
            }
        }

        public virtual double ItemQty
        {
            get
            {
                return moStockIssueDetailsStruct.mdItemQty;
            }
            set
            {
                moStockIssueDetailsStruct.mdItemQty = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moStockIssueDetailsStruct.mdtInsertDate;
            }
            set
            {
                moStockIssueDetailsStruct.mdtInsertDate = value;
            }
        }
        public virtual System.DateTime  ExpectedReturnDate   //expectedReturnDate
        {
            get
            {
                return moStockIssueDetailsStruct.mdExpectedReturnDate;
            }
            set
            {
                moStockIssueDetailsStruct.mdExpectedReturnDate= value;
            }
        }

        public virtual int Inserted_By_Id
        {
            get
            {
                return moStockIssueDetailsStruct.miInsertedById;
            }
            set
            {
                moStockIssueDetailsStruct.miInsertedById = value;
            }
        }
       
        public virtual System.DateTime Update_Date
        {
            get
            {
                return moStockIssueDetailsStruct.mdtUpdateDate;
            }
            set
            {
                moStockIssueDetailsStruct.mdtUpdateDate = value;
            }
        }

        public virtual int Updated_By_Id
        {
            get
            {
                return moStockIssueDetailsStruct.miUpdatedById;
            }
            set
            {
                moStockIssueDetailsStruct.miUpdatedById = value;
            }
        }

        public virtual bool Is_Deleted
        {
            get
            {
                return moStockIssueDetailsStruct.mblnIsDeleted;
            }
            set
            {
                moStockIssueDetailsStruct.mblnIsDeleted = value;
            }
        }

        public virtual int RequisitionID
        {
            get
            {
                return moStockIssueDetailsStruct.miRequisitionID;
            }
            set
            {
                moStockIssueDetailsStruct.miRequisitionID = value;
            }
        }

        public virtual string Comment
        {
            get
            {
                return moStockIssueDetailsStruct.msComment;
            }
            set
            {
                moStockIssueDetailsStruct.msComment = value;
            }
        }

        public virtual string IssuedItemIds
        {
            get
            {
                return moStockIssueDetailsStruct.msIssuedItemIds;
            }
            set
            {
                moStockIssueDetailsStruct.msIssuedItemIds = value;
            }
        }

        public virtual int UOMUnits
        {
            get
            {
                return moStockIssueDetailsStruct.msUnits;
            }
            set
            {
                moStockIssueDetailsStruct.msUnits = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "

        public virtual void InsertStockIssueDetails(int aiSchoolID, string asCancelRemainingItems)
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            moStockIssueDetailsDC.InsertStockIssueDetails(aiSchoolID, asCancelRemainingItems);
        }

        public virtual void InsertStockReturnDetails(int aiSchoolID)
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            moStockIssueDetailsDC.InsertStockReturnDetails(aiSchoolID);
        }

        public virtual void UpdateStockIssueDetails()
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            moStockIssueDetailsDC.UpdateStockIssueDetails();
        }

        public virtual void DeleteStockIssueDetails()
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            moStockIssueDetailsDC.DeleteStockIssueDetails();
        }

        public DataTable GetAllUserRolesForItemIssue()
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            return moStockIssueDetailsDC.GetAllUserRolesForItemIssue();
        }

        public DataTable GetAllUsersList(int aiSchoolID, int aiUserRoleID, int aiAcadamicYearID)
        {
            int iMaximumRows = 1000;
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            return moStockIssueDetailsDC.GetAllUsersList(aiSchoolID, aiUserRoleID, aiAcadamicYearID, iMaximumRows);
        }

        public DataTable GetAllApprovedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID,int abIsGeneral, string sortExpression, int maximumRows, int startRowIndex)
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "ApprovedDate DESC";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = moStockIssueDetailsDC.GetAllApprovedRequisitions(aiSchoolID, asSenderDesgID, asSenderID, abIsGeneral, sortExpression, iEndIndex, startRowIndex);
            if (oDt != null && oDt.Rows.Count > 0)
                miRowCntAppReq = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }
         public DataTable GetAllIssuedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, string sortExpression, int maximumRows, int startRowIndex)  
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "IssuedDate DESC";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = moStockIssueDetailsDC.GetAllIssuedRequisitions(aiSchoolID, asSenderDesgID, asSenderID, abIsGeneral, sortExpression, iEndIndex, startRowIndex);  //
            if (oDt != null && oDt.Rows.Count > 0)
                miRowCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        public DataTable GetAllIssuedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, DateTime  asExpectedReturnDate, string sortExpression, int maximumRows, int startRowIndex)  //add expectedReturnDate
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "IssuedDate DESC";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = moStockIssueDetailsDC.GetAllIssuedRequisitions(aiSchoolID, asSenderDesgID, asSenderID, abIsGeneral, sortExpression, iEndIndex, startRowIndex, asExpectedReturnDate);  
            if (oDt != null && oDt.Rows.Count > 0)
                miRowCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        public int CountRequisitionRow(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral)
        {
            return miRowCntAppReq;
        }

        public int CountIssuedRequisition(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral)
        {
            return miRowCount;
        }
        public int CountIssuedRequisition(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, string asExpectedReturnDate) //
        {
            return miRowCount;
        }

        public DataTable GetItemsForRequisition(int aiRequisitionID)
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            return moStockIssueDetailsDC.GetItemsForRequisition(aiRequisitionID);
        }

        public DataTable GetIssuedItemsOfRequisition(int aiRequisitionID)
        {
            moStockIssueDetailsDC.StockIssueDetailsStructDetails = moStockIssueDetailsStruct;
            return moStockIssueDetailsDC.GetIssuedItemsOfRequisition(aiRequisitionID);
        }

        public List<ItemDetails> GetItemDetails(int aiSchoolId, int aiItemId)
        {
            return moStockIssueDetailsDC.GetItemDetails(aiSchoolId, aiItemId);
        }

        public List<ItemDetails> GetIssuedItemDetails(int aiSchoolId, int aiItemId, int aiRequisitionId)
        {
            return moStockIssueDetailsDC.GetIssuedItemDetails(aiSchoolId, aiItemId, aiRequisitionId);
        }

        public void CancelItemFromRequisition(int aiRequisitionId, int aiItemId, int aiCancelQty, int aiUpdatedById)
        {
            moStockIssueDetailsDC.CancelItemFromRequisition(aiRequisitionId, aiItemId, aiCancelQty, aiUpdatedById);
        }

        #endregion " Public Methods "

        
    }
}
