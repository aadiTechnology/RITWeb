
// Class Name       :- StockBalancesDetailsDC
// Purpose          :- This class is used to manage StockBalancesDetails details.
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
using Utility;




namespace DataCommunicator
{
    public class StockBalancesDetailsDC
    {
        #region " Constants Ans Structures "

        #region " Structures "

        public struct StockBalancesDetailsStruct
        {

            public int miStockBalancesDetailsID;

            public int miItemID;

            public double mdOrginalItemQty;

            public double mdBalencedItemQty;

            public string msReason;

            public int miSchoolId;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mblnIsDeleted;
        }

        #endregion " Structures "

        #endregion " Constants Ans Structures "

        #region " Constructors "

        public StockBalancesDetailsDC()
        {
        }

        public StockBalancesDetailsDC(int miStockBalancesDetailsID)
        {
            LoadStockBalancesDetailsDetails(miStockBalancesDetailsID);
        }

        #endregion " Constructors "

        #region " Data Members And Properties "

        #region " Data Members "

        private StockBalancesDetailsStruct moStockBalancesDetailsStruct;

        #endregion " Data Members "

        #region " Properties "

        public StockBalancesDetailsStruct StockBalancesDetailsStructDetails
        {
            get
            {
                return moStockBalancesDetailsStruct;
            }
            set
            {
                moStockBalancesDetailsStruct = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "

        // This function is used to insert the StockBalancesDetails Details
        public int InsertStockBalancesDetails()
        {

            StringBuilder sInsertStatement = new StringBuilder();
            sInsertStatement.Append(" INSERT INTO" +
                                             " StockBalancesDetails(" +
                                                 "ItemID" +
                                                 ",OrginalItemQty" +
                                                 ",BalencedItemQty" +
                                                 ",Reason" +
                                                 ",School_Id" +
                                                 ",Insert_Date" +
                                                 ",Inserted_By_Id" +
                                                 ",Update_Date" +
                                                 ",Updated_By_Id" +
                                                 ",Is_Deleted" +
                                         ")VALUES(" +
                                              " " + moStockBalancesDetailsStruct.miItemID +
                                              " , " + moStockBalancesDetailsStruct.mdOrginalItemQty +
                                              " , " + moStockBalancesDetailsStruct.mdBalencedItemQty +
                                              " , N'" + StringUtility.ReplaceSingleQuoteInString(moStockBalancesDetailsStruct.msReason, false) + "' " +
                                              " , " + moStockBalancesDetailsStruct.miSchoolId +
                                              " , N'" + System.DateTime.Now.ToShortDateString() + "' " +
                                              " , " + moStockBalancesDetailsStruct.miInsertedById +
                                              " , N'" + System.DateTime.Now.ToShortDateString() + "' " +
                                              " , " + moStockBalancesDetailsStruct.miUpdatedById +
                                              " , N'" + moStockBalancesDetailsStruct.mblnIsDeleted + "' " +
                                             ");");

            sInsertStatement.Append(" UPDATE" +
                                           " ItemsMaster" +
                                    " SET" +
                                           " ItemQty =" + moStockBalancesDetailsStruct.mdBalencedItemQty +
                                           ", Update_Date =N'" + System.DateTime.Now.ToShortDateString() + "' " +
                                        ", Updated_By_Id =N'" + moStockBalancesDetailsStruct.miUpdatedById + "' " +
                                    " WHERE" +
                                           " ItemID = " + moStockBalancesDetailsStruct.miItemID +
                                           " AND" +
                                           " School_Id = " + moStockBalancesDetailsStruct.miSchoolId);
                    
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement.ToString());
        }

        // This function is used to update the StockBalancesDetails Details
        public void UpdateStockBalancesDetails()
        {
            string sUpdateStatement = "UPDATE StockBalancesDetails SET " +
                                            "ItemID= " + moStockBalancesDetailsStruct.miItemID +
                                            ",OrginalItemQty= " + moStockBalancesDetailsStruct.mdOrginalItemQty +
                                            ",BalencedItemQty= " + moStockBalancesDetailsStruct.mdBalencedItemQty +
                                            ",Reason= N'" + StringUtility.ReplaceSingleQuoteInString(moStockBalancesDetailsStruct.msReason, false) + "' " +
                                            ",School_Id= " + moStockBalancesDetailsStruct.miSchoolId +
                                            ",Insert_Date= N'" + System.DateTime.Now.ToShortDateString() + "' " +
                                            ",Inserted_By_Id= " + moStockBalancesDetailsStruct.miInsertedById +
                                            ",Update_Date= N'" + System.DateTime.Now.ToShortDateString() + "' " +
                                            ",Updated_By_Id= " + moStockBalancesDetailsStruct.miUpdatedById +
                                            ",Is_Deleted= " + moStockBalancesDetailsStruct.mblnIsDeleted +
                                            "" +
                                        " WHERE StockBalancesDetailsID=" + moStockBalancesDetailsStruct.miStockBalancesDetailsID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the StockBalancesDetails Details
        public void DeleteStockBalancesDetails()
        {
            string sDeleteStatement = "DELETE StockBalancesDetails WHERE StockBalancesDetailsID=N'" + moStockBalancesDetailsStruct.miStockBalancesDetailsID + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }
        #endregion " Public Methods "

        #region " Private Methods "

        // This function is used to load the StockBalancesDetails Details
        private void LoadStockBalancesDetailsDetails(int miStockBalancesDetailsID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchStockBalancesDetailsDetailsFromDatabase(miStockBalancesDetailsID);
               using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
               {
                   if (oDR != null)
                   {
                       while (oDR.Read())
                       {
                           if (oDR["StockBalancesDetailsID"] != DBNull.Value)
                               moStockBalancesDetailsStruct.miStockBalancesDetailsID = Convert.ToInt32(oDR["StockBalancesDetailsID"]);
                           if (oDR["ItemID"] != DBNull.Value)
                               moStockBalancesDetailsStruct.miItemID = Convert.ToInt32(oDR["ItemID"]);
                           if (oDR["OrginalItemQty"] != DBNull.Value)
                               moStockBalancesDetailsStruct.mdOrginalItemQty = Convert.ToInt32(oDR["OrginalItemQty"]);
                           if (oDR["BalencedItemQty"] != DBNull.Value)
                               moStockBalancesDetailsStruct.mdBalencedItemQty = Convert.ToDouble(oDR["BalencedItemQty"]);
                           if (oDR["Reason"] != DBNull.Value)
                               moStockBalancesDetailsStruct.msReason = Convert.ToString(oDR["Reason"]);
                           if (oDR["School_Id"] != DBNull.Value)
                               moStockBalancesDetailsStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                           if (oDR["Insert_Date"] != DBNull.Value)
                               moStockBalancesDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                           if (oDR["Inserted_By_Id"] != DBNull.Value)
                               moStockBalancesDetailsStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                           if (oDR["Update_Date"] != DBNull.Value)
                               moStockBalancesDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                           if (oDR["Updated_By_Id"] != DBNull.Value)
                               moStockBalancesDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                           if (oDR["Is_Deleted"] != DBNull.Value)
                               moStockBalancesDetailsStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                       }
                   }
                }
            }
        }

        // This function is used to fetch the StockBalancesDetails Details
        private string FetchStockBalancesDetailsDetailsFromDatabase(int miStockBalancesDetailsID)
        {
            string sSelectStatement = " SELECT  " +
            "StockBalancesDetailsID" +
            ",ItemID" +
            ",OrginalItemQty" +
            ",BalencedItemQty" +
            ",Reason" +
            ",School_Id" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            ",Is_Deleted" +
            " FROM StockBalancesDetails" +
            " WHERE StockBalancesDetailsID=" + miStockBalancesDetailsID;
            return sSelectStatement;
        }

        #endregion " Private Methods" 
       
    }
}
