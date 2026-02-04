
// Class Name       :- PurchaseOrderBL
// Purpose          :- This class is used to manage PurchaseOrder details.
// Date Of creation :- 7/13/2009
// Author Name      :- 


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using SchoolEntities;




namespace BusinessLogic
{


    public class PurchaseOrderBL
    {

        private PurchaseOrderDC.PurchaseOrderStruct moPurchaseOrderStruct;

        private PurchaseOrderDC moPurchaseOrderDC;

        public PurchaseOrderBL()
        {
            moPurchaseOrderDC = new PurchaseOrderDC();
        }

        public PurchaseOrderBL(int miPurchaseOrderID)
        {
            moPurchaseOrderDC = new PurchaseOrderDC(miPurchaseOrderID);
            moPurchaseOrderStruct = moPurchaseOrderDC.PurchaseOrderStructDetails;
        }

        public virtual int PurchaseOrderID
        {
            get
            {
                return moPurchaseOrderStruct.miPurchaseOrderID;
            }
            set
            {
                moPurchaseOrderStruct.miPurchaseOrderID = value;
            }
        }

        public virtual string PurchaseOrderCode
        {
            get
            {
                return moPurchaseOrderStruct.msPurchaseOrderCode;
            }
            set
            {
                moPurchaseOrderStruct.msPurchaseOrderCode = value;
            }
        }

        public virtual string PurchaseOrderName
        {
            get
            {
                return moPurchaseOrderStruct.msPurchaseOrderName;
            }
            set
            {
                moPurchaseOrderStruct.msPurchaseOrderName = value;
            }
        }

        public virtual string PurchaseOrderDesc
        {
            get
            {
                return moPurchaseOrderStruct.msPurchaseOrderDesc;
            }
            set
            {
                moPurchaseOrderStruct.msPurchaseOrderDesc = value;
            }
        }

        public virtual int School_Id
        {
            get
            {
                return moPurchaseOrderStruct.miSchoolId;
            }
            set
            {
                moPurchaseOrderStruct.miSchoolId = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moPurchaseOrderStruct.mdtInsertDate;
            }
            set
            {
                moPurchaseOrderStruct.mdtInsertDate = value;
            }
        }

        public virtual int Inserted_By_Id
        {
            get
            {
                return moPurchaseOrderStruct.miInsertedById;
            }
            set
            {
                moPurchaseOrderStruct.miInsertedById = value;
            }
        }

        public virtual System.DateTime Update_Date
        {
            get
            {
                return moPurchaseOrderStruct.mdtUpdateDate;
            }
            set
            {
                moPurchaseOrderStruct.mdtUpdateDate = value;
            }
        }

        public virtual int Updated_By_Id
        {
            get
            {
                return moPurchaseOrderStruct.miUpdatedById;
            }
            set
            {
                moPurchaseOrderStruct.miUpdatedById = value;
            }
        }

        public virtual bool Is_Deleted
        {
            get
            {
                return moPurchaseOrderStruct.mblnIsDeleted;
            }
            set
            {
                moPurchaseOrderStruct.mblnIsDeleted = value;
            }
        }

        public virtual int InsertPurchaseOrder()
        {
            moPurchaseOrderDC.PurchaseOrderStructDetails = moPurchaseOrderStruct;
            return moPurchaseOrderDC.InsertPurchaseOrder();
        }

        public virtual void UpdatePurchaseOrder()
        {
            moPurchaseOrderDC.PurchaseOrderStructDetails = moPurchaseOrderStruct;
            moPurchaseOrderDC.UpdatePurchaseOrder();
        }

        public virtual void DeletePurchaseOrder()
        {
            moPurchaseOrderDC.PurchaseOrderStructDetails = moPurchaseOrderStruct;
            moPurchaseOrderDC.DeletePurchaseOrder();
        }

        public void InsertPurchaseOrderDetails(int aiSchoolId, int aiUserId, string asPOName, string asPODesc, string asPOReqItems, string asPOItems, int aiPOId, bool abOrderType, int aiVendorId, int aiHeaderId,DateTime adtPODeliveryDate, string asNote, int aiDiscount, out int aiPOIdForSubmit)
        {
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            if (asPOReqItems != null)
                oPurchaseOrderDC.InsertPurchaseOrderDetails(aiSchoolId, aiUserId, asPOName, asPODesc, asPOReqItems, asPOItems, aiPOId, abOrderType, aiVendorId, aiHeaderId, adtPODeliveryDate, asNote, aiDiscount, out aiPOIdForSubmit);
            else
                oPurchaseOrderDC.InsertPurchaseOrderDetails(aiSchoolId, aiUserId, asPOName, asPODesc, asPOItems, aiPOId, abOrderType, aiVendorId, aiHeaderId, adtPODeliveryDate, asNote, aiDiscount, out aiPOIdForSubmit);
        }

        public static DataTable GetPOList(int aiSchoolId, int aiUserId, string asPOId, string asRequesterId, string sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;

            return PurchaseOrderDC.GetPOList(aiSchoolId, aiUserId, Convert.ToInt32(asPOId), Convert.ToInt32(asRequesterId), sortExpression, iEndIndex, startRowIndex);
        }

        public static int CountRowsOfPO(Int32 aiSchoolId, int aiUserId, string asPOId, string asRequesterId)
        {
            return PurchaseOrderDC.CountRowsOfPO(aiSchoolId, aiUserId, asPOId);
        }

        public void DeletePurchaseOrderDetails(int aiPOId, int aiSchoolId, int aiUserId)
        {
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            oPurchaseOrderDC.DeletePurchaseOrderDetails(aiPOId, aiSchoolId, aiUserId);
        }

        public DataSet GetPOItemsDetails(int aiPOId, int aiSchoolId, int aiUserId)
        {
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            return oPurchaseOrderDC.GetPOItemsDetails(aiPOId, aiSchoolId, aiUserId);
        }

        public DataTable GetPOsForItem(int aiSchoolId, string asGRNCreateMode, int aiItemId, int aiPOId, int aiGRNId)
        {
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            return oPurchaseOrderDC.GetPOsForItem(aiSchoolId, asGRNCreateMode, aiItemId, aiPOId, aiGRNId);
        }

        public List<PODetailsForApprove> GetAllPODetailsForApprove(int aiSchoolId, int aiUserId, int aiStatusId)
        { 
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            List<PODetailsForApprove> lstPODetailsForApprove = oPurchaseOrderDC.GetAllPODetailsForApprove(aiSchoolId, aiUserId, aiStatusId);
            return lstPODetailsForApprove;
        }

        public void ApprovePurchaseOrder(int aiSchoolId, int aiPOId, int aiUserId)
        {
            PurchaseOrderDC oPurchaseOrderDC = new PurchaseOrderDC();
            oPurchaseOrderDC.ApprovePurchaseOrder(aiSchoolId, aiPOId, aiUserId);
        }
    }
}
