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
    public class GRNDetailsDC
    {
        #region " Constants And Structures "

        #region " Structures "

        public struct GRNDetailsStruct
        {

            public int miGRNDetailsID;

            public int miGRNID;

            public int miItemID;

            public double mdReceivedItemQty;

            public double mdRejectedQty;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mblnIsDeleted;

            public string msUMOName;

            public int iPieceCount;
        }

        #endregion " Structures "

        #endregion " Constants And Structures "
        
        #region " Constructors "

        public GRNDetailsDC()
        {
        }

        public GRNDetailsDC(int miGRNDetailsID)
        {
            LoadGRNDetailsDetails(miGRNDetailsID);
        }

        #endregion " Constructors "

        #region " Data Members And Properties "

        #region " Data Members "

        private GRNDetailsStruct moGRNDetailsStruct;


        #endregion " Data Members "

        #region " Properties "

        public virtual GRNDetailsStruct GRNDetailsStructDetails
        {
            get
            {return moGRNDetailsStruct;}
            set
            {moGRNDetailsStruct = value;}
        }

        #endregion " Properties "

        #endregion " Data Members And Properties "

        #region " Public Methods "

        // This function is used to insert the GRNDetails Details
        public virtual int InsertGRNDetails()
        {
            string sInsertStatement = "INSERT INTO GRNDetails(" +
            "GRNID" +
            ",ItemID" +
            ",ReceivedItemQty" +
            ",RejectedQty" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            ",Is_Deleted" +
            ")VALUES(" +
            " " + moGRNDetailsStruct.miGRNID +
             " , " + moGRNDetailsStruct.miItemID +
             " , " + moGRNDetailsStruct.mdReceivedItemQty +
             " , " + moGRNDetailsStruct.mdRejectedQty +
             " , N'" + moGRNDetailsStruct.mdtInsertDate + "' " +
             " , " + moGRNDetailsStruct.miInsertedById +
             " , N'" + moGRNDetailsStruct.mdtUpdateDate + "' " +
             " , " + moGRNDetailsStruct.miUpdatedById +
             " , " + moGRNDetailsStruct.mblnIsDeleted +
            ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return  oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        // This function is used to update the GRNDetails Details
        public virtual void UpdateGRNDetails()
        {
            string sUpdateStatement = "UPDATE GRNDetails SET " +
            "GRNID= " + moGRNDetailsStruct.miGRNID +
            ",ItemID= " + moGRNDetailsStruct.miItemID +
            ",ReceivedItemQty= " + moGRNDetailsStruct.mdReceivedItemQty +
            ",RejectedQty= " + moGRNDetailsStruct.mdRejectedQty +
            ",Insert_Date= N'" + moGRNDetailsStruct.mdtInsertDate + "' " +
            ",Inserted_By_Id= " + moGRNDetailsStruct.miInsertedById +
            ",Update_Date= N'" + moGRNDetailsStruct.mdtUpdateDate + "' " +
            ",Updated_By_Id= " + moGRNDetailsStruct.miUpdatedById +
            ",Is_Deleted= " + moGRNDetailsStruct.mblnIsDeleted +
            "" +
            " WHERE GRNDetailsID=" + moGRNDetailsStruct.miGRNDetailsID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the GRNDetails Details
        public virtual void DeleteGRNDetails()
        {
            string sDeleteStatement = "DELETE GRNDetails WHERE GRNDetailsID=N'" + moGRNDetailsStruct.miGRNDetailsID + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        #endregion " Public Methods "

        #region " Private Methods " 

        // This function is used to load the GRNDetails Details
        private void LoadGRNDetailsDetails(int miGRNDetailsID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchGRNDetailsDetailsFromDatabase(miGRNDetailsID);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["GRNDetailsID"] != DBNull.Value)
                                moGRNDetailsStruct.miGRNDetailsID = Convert.ToInt32(oDR["GRNDetailsID"]);
                            if (oDR["GRNID"] != DBNull.Value)
                                moGRNDetailsStruct.miGRNID = Convert.ToInt32(oDR["GRNID"]);
                            if (oDR["ItemID"] != DBNull.Value)
                                moGRNDetailsStruct.miItemID = Convert.ToInt32(oDR["ItemID"]);
                            if (oDR["ReceivedItemQty"] != DBNull.Value)
                                moGRNDetailsStruct.mdReceivedItemQty = Convert.ToDouble(oDR["ReceivedItemQty"]);
                            if (oDR["RejectedQty"] != DBNull.Value)
                                moGRNDetailsStruct.mdRejectedQty = Convert.ToDouble(oDR["RejectedQty"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moGRNDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_Id"] != DBNull.Value)
                                moGRNDetailsStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moGRNDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moGRNDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moGRNDetailsStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the GRNDetails Details
        private string FetchGRNDetailsDetailsFromDatabase(int miGRNDetailsID)
        {
            string sSelectStatement = " SELECT  " +
            "GRNDetailsID" +
            ",GRNID" +
            ",ItemID" +
            ",ReceivedItemQty" +
            ",RejectedQty" +
            ",Insert_Date" +
            ",Inserted_By_Id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            ",Is_Deleted" +
            " FROM GRNDetails" +
            " WHERE GRNDetailsID=" + miGRNDetailsID;
            return sSelectStatement;
        }

        #endregion " Private Methods "

        public DataTable GetPODetails(int aiSchoolId, string asGRNId, bool abItemWise, bool abPOWise, string sortExpression, int iStartIndex, int iEndIndex)
        { 
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sDisplayMode", GetDisplayMode(abItemWise, abPOWise), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sGRNId", asGRNId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", String.Format(" ORDER BY {0}", sortExpression), SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedItemsInPurchaseOrder");
            }   
        }

        private string GetDisplayMode(bool abItemWise, bool abPOWise)
        {
            string sDisplayMode = string.Empty;

            if (abItemWise)
                sDisplayMode = "ItemWise";
            else if (abPOWise)
                sDisplayMode = "POWise";

            return sDisplayMode;
        }

        public int CountItemsInPO(int aiSchoolId, string asGRNId, bool abItemWise, bool abPOWise)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sGRNId", asGRNId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sDisplayMode", GetDisplayMode(abItemWise, abPOWise), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountItemsInPurchaseOrder");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public void InsertGRNDetails(int aiSchoolId, int aiUserId, string asGRNName, string asGRNDesc, string asGRNPOItems, string asGRNItems, int aiGRNId, string sIsModify)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GRNName", asGRNName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GRNDesc", asGRNDesc, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GRNItemDetails", asGRNItems, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GRNPOItemDetails", asGRNPOItems, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GRNId", aiGRNId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsModify", sIsModify, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertGRNDetails");
            }   
        }

        public static DataTable GetGRNList(int aiSchoolId, int aiUserId, String sortExpression, int iEndIndex, int startRowIndex)
        {
            Constants.ParameterNameValuePair[] oArrParameterNameValuePair = new Constants.ParameterNameValuePair[5];
            string sSortExpression = "";
            if (sortExpression != "")
                sSortExpression = String.Format(" ORDER BY {0}", sortExpression);
            else
                sSortExpression = " ORDER BY GRNCode";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedGRN");
            }
        }

        public static int CountTotalGRN(Int32 aiSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountGRN");
                return Convert.ToInt32(oSqlParameter.Value);
            }   
        }

        public DataSet GetGRNItemsDetails(int aiGRNId, int aiSchoolId)
        {   
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iGRNId", aiGRNId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetGRNItemsDetails");
            }
        }

        public void DeleteGRNDetails(int aiGRNId, int aiSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iGRNId", aiGRNId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iUserId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteGRNDetails");
            }
        }
    }
}
