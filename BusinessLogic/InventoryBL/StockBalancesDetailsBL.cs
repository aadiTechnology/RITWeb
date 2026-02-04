
// Class Name       :- StockBalancesDetailsBL
// Purpose          :- This class is used to manage Stock Balance Details details.
// Date Of creation :- 7/1/2009
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




namespace BusinessLogic
{
    public class StockBalancesDetailsBL
    {
        #region " Constants "
        #endregion " Constants "

        #region " Constructors "

        public StockBalancesDetailsBL()
        {
            moStockBalancesDetailsDC = new StockBalancesDetailsDC();
        }

        public StockBalancesDetailsBL(int miStockBalancesDetailsID)
        {
            moStockBalancesDetailsDC = new StockBalancesDetailsDC(miStockBalancesDetailsID);
            moStockBalancesDetailsStruct = moStockBalancesDetailsDC.StockBalancesDetailsStructDetails;
        }

        #endregion " Constructors "

        #region " Data Members And Properties "

        #region " Data Members "

        private StockBalancesDetailsDC.StockBalancesDetailsStruct moStockBalancesDetailsStruct;
        private StockBalancesDetailsDC moStockBalancesDetailsDC;

        #endregion " Data Members"

        #region " Properties "

        public int StockBalancesDetailsID
        {
            get
            {
                return moStockBalancesDetailsStruct.miStockBalancesDetailsID;
            }
            set
            {
                moStockBalancesDetailsStruct.miStockBalancesDetailsID = value;
            }
        }

        public int ItemID
        {
            get
            {
                return moStockBalancesDetailsStruct.miItemID;
            }
            set
            {
                moStockBalancesDetailsStruct.miItemID = value;
            }
        }

        public double OrginalItemQty
        {
            get
            {
                return moStockBalancesDetailsStruct.mdOrginalItemQty;
            }
            set
            {
                moStockBalancesDetailsStruct.mdOrginalItemQty = value;
            }
        }

        public double BalencedItemQty
        {
            get
            {
                return moStockBalancesDetailsStruct.mdBalencedItemQty;
            }
            set
            {
                moStockBalancesDetailsStruct.mdBalencedItemQty = value;
            }
        }

        public string Reason
        {
            get
            {
                return moStockBalancesDetailsStruct.msReason;
            }
            set
            {
                moStockBalancesDetailsStruct.msReason = value;
            }
        }

        public int School_Id
        {
            get
            {
                return moStockBalancesDetailsStruct.miSchoolId;
            }
            set
            {
                moStockBalancesDetailsStruct.miSchoolId = value;
            }
        }

        public System.DateTime Insert_Date
        {
            get
            {
                return moStockBalancesDetailsStruct.mdtInsertDate;
            }
            set
            {
                moStockBalancesDetailsStruct.mdtInsertDate = value;
            }
        }

        public int Inserted_By_Id
        {
            get
            {
                return moStockBalancesDetailsStruct.miInsertedById;
            }
            set
            {
                moStockBalancesDetailsStruct.miInsertedById = value;
            }
        }

        public System.DateTime Update_Date
        {
            get
            {
                return moStockBalancesDetailsStruct.mdtUpdateDate;
            }
            set
            {
                moStockBalancesDetailsStruct.mdtUpdateDate = value;
            }
        }

        public int Updated_By_Id
        {
            get
            {
                return moStockBalancesDetailsStruct.miUpdatedById;
            }
            set
            {
                moStockBalancesDetailsStruct.miUpdatedById = value;
            }
        }

        public bool Is_Deleted
        {
            get
            {
                return moStockBalancesDetailsStruct.mblnIsDeleted;
            }
            set
            {
                moStockBalancesDetailsStruct.mblnIsDeleted = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "

        public int InsertStockBalancesDetails()
        {
            moStockBalancesDetailsDC.StockBalancesDetailsStructDetails = moStockBalancesDetailsStruct;
            return moStockBalancesDetailsDC.InsertStockBalancesDetails();
        }

        public  void UpdateStockBalancesDetails()
        {
            moStockBalancesDetailsDC.StockBalancesDetailsStructDetails = moStockBalancesDetailsStruct;
            moStockBalancesDetailsDC.UpdateStockBalancesDetails();
        }

        public  void DeleteStockBalancesDetails()
        {
            moStockBalancesDetailsDC.StockBalancesDetailsStructDetails = moStockBalancesDetailsStruct;
            moStockBalancesDetailsDC.DeleteStockBalancesDetails();
        }

        #endregion " Public Methods " 
    }
}
