using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;



namespace BusinessLogic
{
    public class GRNDetailsBL
    {
        #region " Constuctors "
        public GRNDetailsBL()
        {
            moGRNDetailsDC = new GRNDetailsDC();
        }

        public GRNDetailsBL(int miGRNDetailsID)
        {
            moGRNDetailsDC = new GRNDetailsDC(miGRNDetailsID);
            moGRNDetailsStruct = moGRNDetailsDC.GRNDetailsStructDetails;
        }
        #endregion " Constructors "

        #region " Data Members And Properties "

        #region " Data Members "

        private GRNDetailsDC.GRNDetailsStruct moGRNDetailsStruct;
        private GRNDetailsDC moGRNDetailsDC;

        #endregion " Data Members "

        #region " Properties "

        public virtual int GRNDetailsID
        {
            get
            { return moGRNDetailsStruct.miGRNDetailsID;}
            set
            { moGRNDetailsStruct.miGRNDetailsID = value;}
        }

        public virtual int GRNID
        {
            get
            {return moGRNDetailsStruct.miGRNID;}
            set
            {moGRNDetailsStruct.miGRNID = value;}
        }

        public virtual int ItemID
        {
            get
            {return moGRNDetailsStruct.miItemID;}
            set
            {moGRNDetailsStruct.miItemID = value;}
        }

        public virtual double ReceivedItemQty
        {
            get
            {return moGRNDetailsStruct.mdReceivedItemQty;}
            set
            {moGRNDetailsStruct.mdReceivedItemQty = value;}
        }

        public virtual double RejectedQty
        {
            get
            {return moGRNDetailsStruct.mdRejectedQty;}
            set
            {moGRNDetailsStruct.mdRejectedQty = value;}
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {return moGRNDetailsStruct.mdtInsertDate;}
            set
            {moGRNDetailsStruct.mdtInsertDate = value;}
        }

        public virtual int Inserted_By_Id
        {
            get
            {return moGRNDetailsStruct.miInsertedById;}
            set
            {moGRNDetailsStruct.miInsertedById = value;}
        }

        public virtual System.DateTime Update_Date
        {
            get
            {return moGRNDetailsStruct.mdtUpdateDate;}
            set
            {moGRNDetailsStruct.mdtUpdateDate = value;}
        }

        public virtual int Updated_By_Id
        {
            get
            {return moGRNDetailsStruct.miUpdatedById;}
            set
            {moGRNDetailsStruct.miUpdatedById = value;}
        }

        public virtual bool Is_Deleted
        {
            get
            {return moGRNDetailsStruct.mblnIsDeleted;}
            set
            {moGRNDetailsStruct.mblnIsDeleted = value;}
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "

        public virtual void InsertGRNDetails(int aiSchoolId, int aiUserId, string sGRNName, string sGRNDesc, string sGRNPOItems, string sGRNItems, int iGRNId, string sIsModify)
        {
            moGRNDetailsDC.GRNDetailsStructDetails = moGRNDetailsStruct;
            moGRNDetailsDC.InsertGRNDetails(aiSchoolId, aiUserId, sGRNName, sGRNDesc, sGRNPOItems, sGRNItems, iGRNId, sIsModify);
        }

        public virtual void UpdateGRNDetails()
        {
            moGRNDetailsDC.GRNDetailsStructDetails = moGRNDetailsStruct;
            moGRNDetailsDC.UpdateGRNDetails();
        }

        public virtual void DeleteGRNDetails()
        {
            moGRNDetailsDC.GRNDetailsStructDetails = moGRNDetailsStruct;
            moGRNDetailsDC.DeleteGRNDetails();
        }

        public DataTable GetPODetails(int aiSchoolId, string asGRNId, bool abItemWise, bool abPOWise, string sortExpression, int maximumRows, int startRowIndex)
        {
            moGRNDetailsDC.GRNDetailsStructDetails = moGRNDetailsStruct;
            if (String.IsNullOrEmpty(sortExpression))
            {
                if(abItemWise)
                    sortExpression = "ItemName";
                else
                    sortExpression = "PurchaseOrderCode";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moGRNDetailsDC.GetPODetails(aiSchoolId, asGRNId, abItemWise, abPOWise, sortExpression, iStartIndex, iEndIndex);
        }

        public int CountItemsInPO(int aiSchoolId, string asGRNId, bool abItemWise, bool abPOWise)
        {
            moGRNDetailsDC.GRNDetailsStructDetails = moGRNDetailsStruct;
            return moGRNDetailsDC.CountItemsInPO(aiSchoolId, asGRNId, abItemWise, abPOWise);
        }


        public static DataTable GetGRNList(int aiSchoolId, int aiUserId, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;

            return GRNDetailsDC.GetGRNList(aiSchoolId, aiUserId, sortExpression, iEndIndex, startRowIndex);
        }

        public static int CountTotalGRN(Int32 aiSchoolId, int aiUserId)
        {
            return GRNDetailsDC.CountTotalGRN(aiSchoolId, aiUserId);
        }


        public DataSet GetGRNItemsDetails(int aiGRNId, int aiSchoolId)
        {
            GRNDetailsDC oGRNDetailsDC = new GRNDetailsDC();
            return oGRNDetailsDC.GetGRNItemsDetails(aiGRNId, aiSchoolId);
        }

        public void DeleteGRNDetails(int aiGRNID, int aiSchoolID, int aiUserId)
        {
            GRNDetailsDC oGRNDetailsDC = new GRNDetailsDC();
            oGRNDetailsDC.DeleteGRNDetails(aiGRNID, aiSchoolID, aiUserId);
        }

        #endregion " Public Methods "
    }
}
