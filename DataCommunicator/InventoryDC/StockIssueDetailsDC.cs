// Class Name       :- StockIssueDetailsDC
// Purpose          :- This class is used to manage stock issue details.
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
using Utility;
using SchoolEntities.Inventory;

namespace DataCommunicator
{
    public class StockIssueDetailsDC
    {
        #region " Constants And Structures "

        #region " Structures "

        public struct StockIssueDetailsStruct
        {
            public int miStockIssueDetailsID;

            public int miItemID;

            public double mdItemQty;

            public System.DateTime mdtInsertDate;
            public System.DateTime mdExpectedReturnDate;  //Expected return date
            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mblnIsDeleted;

            public int miRequisitionID;

            public string msComment;

            public string msIssuedItemIds;

            public int msUnits;
        }

        #endregion " Structures "

        #endregion " Constants And Structures "

        #region " Constuctors "

        public StockIssueDetailsDC()
        {
        }

        public StockIssueDetailsDC(int miStockIssueDetailsID)
        {
            LoadStockIssueDetailsDetails(miStockIssueDetailsID);
        }

        #endregion " Constructors "

        #region " Data Members And Properties "

        #region " Data Members "

        private StockIssueDetailsStruct moStockIssueDetailsStruct;

        #endregion " Data Members "

        #region " Properties "

        public virtual StockIssueDetailsStruct StockIssueDetailsStructDetails
        {
            get
            {
                return moStockIssueDetailsStruct;
            }
            set
            {
                moStockIssueDetailsStruct = value;
            }
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "



        public virtual void InsertStockIssueDetails(int aiSchoolID, string asCancelRemainingItems)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolID", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iItemID", moStockIssueDetailsStruct.miItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("dItemQty", moStockIssueDetailsStruct.mdItemQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("iRequisitionID", moStockIssueDetailsStruct.miRequisitionID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iInsertedByID", moStockIssueDetailsStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sComment", moStockIssueDetailsStruct.msComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IssuedItemIds", moStockIssueDetailsStruct.msIssuedItemIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UOMUnits", moStockIssueDetailsStruct.msUnits, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CancelRemainingItems", asCancelRemainingItems, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ExpectedReturnDate", moStockIssueDetailsStruct.mdExpectedReturnDate, SqlDbType.DateTime); //Expected return date 

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_IssueItemsInRequisition",true);
            }
        }

        public virtual void InsertStockReturnDetails(int aiSchoolID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolID", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iItemID", moStockIssueDetailsStruct.miItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("dItemQty", moStockIssueDetailsStruct.mdItemQty, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("iRequisitionID", moStockIssueDetailsStruct.miRequisitionID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iInsertedByID", moStockIssueDetailsStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sComment", moStockIssueDetailsStruct.msComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IssuedItemIds", moStockIssueDetailsStruct.msIssuedItemIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UOMUnits", moStockIssueDetailsStruct.msUnits, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_ReturnItemsInStock", true);
            }
        }

        // This function is used to update the StockIssueDetails Details
        public virtual void UpdateStockIssueDetails()
        {
            string sUpdateStatement = "UPDATE StockIssueDetails SET " +
            "ItemID= " + moStockIssueDetailsStruct.miItemID +
            ",ItemQty= " + moStockIssueDetailsStruct.mdItemQty +
            ",Insert_Date= N'" + moStockIssueDetailsStruct.mdtInsertDate + "' " +
            ",Inserted_By_Id= " + moStockIssueDetailsStruct.miInsertedById +
            ",Update_Date= N'" + moStockIssueDetailsStruct.mdtUpdateDate + "' " +
            ",Updated_By_Id= " + moStockIssueDetailsStruct.miUpdatedById +
            ",Is_Deleted= " + moStockIssueDetailsStruct.mblnIsDeleted +
            ",RequisitionID= " + moStockIssueDetailsStruct.miRequisitionID +
            "" +
            " WHERE StockIssueDetailsID=" + moStockIssueDetailsStruct.miStockIssueDetailsID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the StockIssueDetails Details
        public virtual void DeleteStockIssueDetails()
        {
            string sDeleteStatement = "DELETE StockIssueDetails WHERE StockIssueDetailsID=N'" + moStockIssueDetailsStruct.miStockIssueDetailsID + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method returns datatable populated with 'Teacher' and 'Admin Staff' user roles from databse.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUserRolesForItemIssue()
        {
            string sSelectStatement = " SELECT  " +
                                               " User_Role_Id " +
                                               " , User_Role_Name" +
                                           " FROM " +
                                                " User_Role_Master " +
                                           " WHERE " +
                                                " Is_Deleted = N'" + Constants.C_NO + "'" +
                                                " AND " +
                                                "User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Student) +
                                                " AND " +
                                                "User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.OtherStaff) +
                                                " AND " +
                                                "User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.TransportStaff) +
                                                "AND " +
                                                "User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Parent) +
                                           " ORDER BY " +
                                                " User_Role_Id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        #endregion " Public Methods "

        #region " Private Methods "

        // This function is used to load the StockIssueDetails Details
        private void LoadStockIssueDetailsDetails(int miStockIssueDetailsID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchStockIssueDetailsDetailsFromDatabase(miStockIssueDetailsID);
               using( SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
               {
                   if (oDR != null)
                   {
                       while (oDR.Read())
                       {
                           if (oDR["StockIssueDetailsID"] != DBNull.Value)
                               moStockIssueDetailsStruct.miStockIssueDetailsID = Convert.ToInt32(oDR["StockIssueDetailsID"]);
                           if (oDR["ItemID"] != DBNull.Value)
                               moStockIssueDetailsStruct.miItemID = Convert.ToInt32(oDR["ItemID"]);
                           if (oDR["ItemQty"] != DBNull.Value)
                               moStockIssueDetailsStruct.mdItemQty = Convert.ToDouble(oDR["ItemQty"]);
                           if (oDR["Insert_Date"] != DBNull.Value)
                               moStockIssueDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                           if (oDR["Inserted_By_Id"] != DBNull.Value)
                               moStockIssueDetailsStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                           if (oDR["Update_Date"] != DBNull.Value)
                               moStockIssueDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                           if (oDR["Updated_By_Id"] != DBNull.Value)
                               moStockIssueDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                           if (oDR["Is_Deleted"] != DBNull.Value)
                               moStockIssueDetailsStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                           if (oDR["RequisitionID"] != DBNull.Value)
                               moStockIssueDetailsStruct.miRequisitionID = Convert.ToInt32(oDR["RequisitionID"]);
                       }
                   }
                }
            }
        }

        // This function is used to fetch the StockIssueDetails Details
        private string FetchStockIssueDetailsDetailsFromDatabase(int miStockIssueDetailsID)
        {
            string sSelectStatement = " SELECT  " +
            "StockIssueDetailsID" +
            ",ItemID" +
            ",ItemQty" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            ",Is_Deleted" +
            ",RequisitionID" +
            " FROM StockIssueDetails" +
            " WHERE StockIssueDetailsID=" + miStockIssueDetailsID;
            return sSelectStatement;
        }

        #endregion " Private Methods "

        public DataTable GetAllUsersList(int aiSchoolId, int aiUserRoleId, int aiAcademicYearId, int maximumRows)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPAGEDUserList");
            }
        }

        public DataTable GetAllApprovedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, string sortExpression, int iEndIndex, int iStartIndex)
        {
            if (sortExpression == "ApprovedDate")
                sortExpression = "ReqAct.Insert_Date";
            else if (sortExpression == "ApprovedDate DESC")
                sortExpression = "ReqAct.Insert_Date DESC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asSenderDesgID, asSenderID, abIsGeneral), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedApproveRequisition");
            }
        }

        public DataTable GetAllIssuedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, string sortExpression, int iEndIndex, int iStartIndex)  
        {
            if (sortExpression == "ApprovedDate")
                sortExpression = "ReqAct.Insert_Date";
            else if (sortExpression == "ApprovedDate DESC")
                sortExpression = "ReqAct.Insert_Date DESC";

            if (sortExpression == "IssuedDate")
                sortExpression = "Issued.Insert_Date";
            else if (sortExpression == "IssuedDate DESC")
                sortExpression = "Issued.Insert_Date DESC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asSenderDesgID, asSenderID, abIsGeneral), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedIssuedRequisition");
            }
        }

        public DataTable GetAllIssuedRequisitions(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, string sortExpression, int iEndIndex, int iStartIndex, DateTime asExpectedReturnDate)  //add ExpectedReturnDate
        {
            if (sortExpression == "ApprovedDate")
                sortExpression = "ReqAct.Insert_Date";
            else if (sortExpression == "ApprovedDate DESC")
                sortExpression = "ReqAct.Insert_Date DESC";

            if (sortExpression == "IssuedDate")
                sortExpression = "Issued.Insert_Date";
            else if (sortExpression == "IssuedDate DESC")
                sortExpression = "Issued.Insert_Date DESC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asSenderDesgID, asSenderID, abIsGeneral, asExpectedReturnDate), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedIssuedRequisition");
            }
        }

        private string CreateFilter(string asSenderDesgID, string asSenderID, int abIsGeneral, DateTime asExpectedReturnDate)  //add ExpectedReturnDate
        {
            string sFilter = string.Empty; 

            if (asSenderDesgID != "0")
                sFilter = String.Format("{0} AND User_Role_Id ={1}", string.Empty, asSenderDesgID);
            else 
                sFilter = string.Empty;
            if (asSenderID != "0")
                sFilter = String.Format("{0} AND User_Id ={1}", sFilter, asSenderID);
            
                sFilter = String.Format("{0} AND Is_General ={1}", sFilter, abIsGeneral);

                if (asExpectedReturnDate != Constants.S_DEFAULT_DATE_2.ToDateTime())
                    sFilter = String.Format( "{0} AND ExpectedReturnDate = '{1}'",sFilter, asExpectedReturnDate);
                    
            return sFilter;
        }
        private string CreateFilter(string asSenderDesgID, string asSenderID, int abIsGeneral)  
        {
            string sFilter = string.Empty;

            if (asSenderDesgID != "0")
                sFilter = String.Format("{0} AND User_Role_Id ={1}", string.Empty, asSenderDesgID);
            else
                sFilter = string.Empty;
            if (asSenderID != "0")
                sFilter = String.Format("{0} AND User_Id ={1}", sFilter, asSenderID);

            sFilter = String.Format("{0} AND Is_General ={1}", sFilter, abIsGeneral);

         
            return sFilter;
        }

        public int CountRequisitionRow(int aiSchoolID, string asSenderDesgID, string asSenderID, int abIsGeneral, DateTime asExpectedReturnDate)        //add ExpectedReturnDate
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asSenderDesgID, asSenderID, abIsGeneral, asExpectedReturnDate), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountApprovedRequisition");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public DataTable GetItemsForRequisition(int aiRequisitionID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("RequisitionId", aiRequisitionID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetApprovedRequisitionItems");
            }
        }

        public DataTable GetIssuedItemsOfRequisition(int aiRequisitionID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("RequisitionId", aiRequisitionID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetIssuedRequisitionItems");
            }
        }

        public List<ItemDetails> GetItemDetails(int aiSchoolId, int aiItemId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetItemDetails"))
                {
                    List<ItemDetails> lstItemDetails = new List<ItemDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstItemDetails.Add
                            (
                                 new ItemDetails
                                 {
                                     Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                     SpecificationCode = Convert.ToString(oSqlDataReader["Code"]),
                                     Description = Convert.ToString(oSqlDataReader["Description"])
                                 }
                            );
                    }

                    return lstItemDetails;
                }
            }
        }

        public List<ItemDetails> GetIssuedItemDetails(int aiSchoolId, int aiItemId, int aiRequisitionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RequisitionId", aiRequisitionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetIssuedItemDetails"))
                {
                    List<ItemDetails> lstItemDetails = new List<ItemDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstItemDetails.Add
                            (
                                 new ItemDetails
                                 {
                                     Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                     SpecificationCode = Convert.ToString(oSqlDataReader["Code"]),
                                     Description = Convert.ToString(oSqlDataReader["Description"])
                                 }
                            );
                    }

                    return lstItemDetails;
                }
            }
        }

        public void CancelItemFromRequisition(int aiRequisitionId, int aiItemId, int aiCancelQty, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("RequisitionId", aiRequisitionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CancelQty", aiCancelQty, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CancelItemFromRequisition");
            }
        }


    }
}
