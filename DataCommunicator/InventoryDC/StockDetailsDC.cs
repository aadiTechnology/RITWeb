// File Name    : StockDetailsDC.cs
// Created By   : Sanket Bhujbal
// Crested Date : 26-Dec-2015 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NewStockDetails;
using Utility;

namespace DataCommunicator
{
   public class StockDetailsDC
   {
       #region "Data Members"

       private int iSchoolId;
       private int iUserId;

       #endregion

       #region "Constructors"

       /// <summary>
       /// Parameterized constructor.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiUserId"></param>
       public StockDetailsDC(int aiSchoolId,int aiUserId)
       {
           this.iSchoolId = aiSchoolId;
           this.iUserId = aiUserId;
       }

       /// <summary>
       /// Default constructor.
       /// </summary>
       public StockDetailsDC()
       { }

       #endregion

       #region "Methods"

       /// <summary>
       /// This method is used to return entity list of Stock Item Details.
       /// </summary>
       /// <param name="aiItemId"></param>
       /// <returns></returns>
       public StockItemDetails GetStockItemDetails(int aiItemId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStockItemDetails"))
                   return this.ReadStockItemDetails(oSqlDataReader);                 
           }
       }
       public DataTable GetAllVendor(int miSchoolId, int miAcademicYearId)
       {
           string sSelectStatement = " SELECT  " +
                                " Id AS VendorId " +
                                ", FirstName + ' ' + MiddleName + ' ' + LastName AS VendorName " +
                            " FROM " +
                                 " SchoolVendorDetails " +
                            " WHERE " +
                                 " IsDeleted = 0" +
                                 " AND SchoolId = " + miSchoolId;
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
               return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
       }
       /// <summary>
       /// This method is used to delete item details.
       /// </summary>
       /// <param name="aiId"></param>
       public void Delete(int aiId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("UserId", this.iUserId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteNewStockDetails");
           }
       }

       /// <summary>
       /// This method is used to save Item stock details.
       /// </summary>
       /// <param name="aStockDetails"></param>
       public void Save(StockDetails aoStockDetails, int aiId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("ItemId", aoStockDetails.ItemId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Quantity", aoStockDetails.ItemQuantity, SqlDbType.Decimal);
               oSQLServerDbUtility.AddParameter("Price", aoStockDetails.price, SqlDbType.Float);
               oSQLServerDbUtility.AddParameter("Date", aoStockDetails.Date, SqlDbType.DateTime);
               oSQLServerDbUtility.AddParameter("Description", aoStockDetails.Description, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("UserId", this.iUserId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ConsiderInUnitQuanity", aoStockDetails.ConsiderInUnitQuanity, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
               oSQLServerDbUtility .AddParameter("InvoiceNo" ,aoStockDetails .InvoiceNo , SqlDbType.NVarChar);  //invoiceno
               oSQLServerDbUtility.AddParameter ("VendorId", aoStockDetails .VendorId ,  SqlDbType.Int );  //vendor
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertNewStockDetails");
           }
       }

       /// <summary>
       /// This method is used to get all Item Stock Details.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public List<StockDetails> GetAll(int aiItemId,int aiSchoolId, string sortExpression, int iStartIndex, int iEndIndex)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ItemId", aiItemId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllNewStockDetails"))
                   return this.ReadAllStockDetails(oSqlDataReader);
           }
       }

       /// <summary>
       /// This method is used to count number of records.
       /// </summary>
       /// <param name="aiItemId"></param>
       /// <param name="aiSchoolId"></param>
       /// <returns></returns>
       public int Count(int aiItemId, int aiSchoolId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ItemId",aiItemId , SqlDbType.Int);
               SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
               oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountAllNewStockDetails");
               return Convert.ToInt32(oSqlParameter.Value);
           }
       }       

       /// <summary>
       /// This method used to return specific item stock details.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
       public StockDetails Get(int aiId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetNewStockDetails"))
                   return this.ReadStockDetails(oSqlDataReader);
           }
       }

       /// <summary>
       /// This method used to return specific item stock details.
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
       public StockDetails ReadStockDetails(SqlDataReader aoSqlDataReader)
       {
           StockDetails oStockDetails = new StockDetails();
           if (aoSqlDataReader != null)
           {
               if(aoSqlDataReader.Read())
               {
                   if (aoSqlDataReader["StockBalancesDetailsID"] != DBNull.Value)
                       oStockDetails.Id = Convert.ToInt32(aoSqlDataReader["StockBalancesDetailsID"]);
                   if (aoSqlDataReader["ItemID"] != DBNull.Value)
                       oStockDetails.ItemId = Convert.ToInt32(aoSqlDataReader["ItemID"]);
                   if (aoSqlDataReader["NewQuantity"] != DBNull.Value)
                       oStockDetails.ItemQuantity = Convert.ToDecimal(aoSqlDataReader["NewQuantity"]);
                   if (aoSqlDataReader["Price"] != DBNull.Value)
                       oStockDetails.price = Convert.ToDecimal(aoSqlDataReader["Price"]);
                   if (aoSqlDataReader["NewStockDate"] != DBNull.Value)
                       oStockDetails.Date = Convert.ToDateTime(aoSqlDataReader["NewStockDate"]);
                   if (aoSqlDataReader["Reason"] != DBNull.Value)
                       oStockDetails.Description = aoSqlDataReader["Reason"].ToString();
                   if (aoSqlDataReader["ConsiderUnitQuantity"] != DBNull.Value)
                       oStockDetails.ConsiderInUnitQuanity = Convert.ToInt32(aoSqlDataReader["ConsiderUnitQuantity"]);
                   if (aoSqlDataReader["PieceCount"] != DBNull.Value)
                       oStockDetails.UOMPieceCount = Convert.ToInt32(aoSqlDataReader["PieceCount"]);
                   if (aoSqlDataReader["InvoiceNo"] != DBNull.Value)
                       oStockDetails.InvoiceNo = Convert.ToString(aoSqlDataReader["InvoiceNo"]);
                   if (aoSqlDataReader["VendorId"] != DBNull.Value)
                       oStockDetails.VendorId = Convert.ToInt32(aoSqlDataReader["VendorId"]);
               }
               aoSqlDataReader.Close();
           }
           return oStockDetails;
       }

       /// <summary>
       /// This method is used to get All Item Stock Details.
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
       private List<StockDetails> ReadAllStockDetails(SqlDataReader aoSqlDataReader)
       {
           List<StockDetails> lstStockDetails = new List<StockDetails>();
           if (aoSqlDataReader != null)
           {
               while (aoSqlDataReader.Read())
               {
                   StockDetails oStockDetails = new StockDetails();
                   if (aoSqlDataReader["StockBalancesDetailsID"] != DBNull.Value)
                       oStockDetails.Id = Convert.ToInt32(aoSqlDataReader["StockBalancesDetailsID"]);
                   if (aoSqlDataReader["ItemID"] != DBNull.Value)
                       oStockDetails.ItemId = Convert.ToInt32(aoSqlDataReader["ItemID"]);
                   if (aoSqlDataReader["NewQuantity"] != DBNull.Value)
                       oStockDetails.ItemQuantity = Convert.ToInt32(aoSqlDataReader["NewQuantity"]);
                   if (aoSqlDataReader["Price"] != DBNull.Value)
                       oStockDetails.price = Convert.ToDecimal(aoSqlDataReader["Price"]);
                   if (aoSqlDataReader["NewStockDate"] != DBNull.Value)
                       oStockDetails.Date = Convert.ToDateTime(aoSqlDataReader["NewStockDate"]);
                   if (aoSqlDataReader["NewQuantityWithUnits"] != DBNull.Value)
                       oStockDetails.ItemQuantityWithUnits = Convert.ToString(aoSqlDataReader["NewQuantityWithUnits"]);
                   if (aoSqlDataReader["InvoiceNo"] != DBNull.Value)
                       oStockDetails.InvoiceNo = Convert.ToString(aoSqlDataReader["InvoiceNo"]);
                   if (aoSqlDataReader["VendorId"] != DBNull.Value)
                       oStockDetails.VendorId = Convert.ToInt32(aoSqlDataReader["VendorId"]);
                   lstStockDetails.Add(oStockDetails);
               }
               aoSqlDataReader.Close();
           }
           return lstStockDetails;
       }

       /// <summary>
       /// This method is used to return entity list of Stock Item Details.
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
       private StockItemDetails ReadStockItemDetails(SqlDataReader aoSqlDataReader)
       {
           StockItemDetails oStockItemDetails = new StockItemDetails();
           if (aoSqlDataReader != null)
           {
               if (aoSqlDataReader.Read())
               {
                   if (aoSqlDataReader["ItemName"] != DBNull.Value)
                       oStockItemDetails.ItemName = aoSqlDataReader["ItemName"].ToString();
                   if (aoSqlDataReader["ItemCode"] != DBNull.Value)
                       oStockItemDetails.ItemCode = aoSqlDataReader["ItemCode"].ToString();
                   if (aoSqlDataReader["ItemQty"] != DBNull.Value)
                       oStockItemDetails.CurrentQuantity = Convert.ToDecimal(aoSqlDataReader["ItemQty"]);
                   if (aoSqlDataReader["UOMUnit"] != DBNull.Value)
                       oStockItemDetails.CurrentStockUOM = aoSqlDataReader["UOMUnit"].ToString();
                   if (aoSqlDataReader["NewQuantityWithUnits"] != DBNull.Value)
                       oStockItemDetails.ItemQuantityWithUnits = Convert.ToString(aoSqlDataReader["NewQuantityWithUnits"]);
               }
               aoSqlDataReader.Close();
           }
           return oStockItemDetails;
       }
       #endregion

   }
}
