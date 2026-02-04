using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using SchoolEntities.eStore;

namespace DataCommunicator.eStoreDC
{
   public class StoreItemDetailsDC : DataCommunicatorBaseDC
   {
       #region Data Member(s)

       private int miSchoolId;
       private int miUpdatedById;
       private int miAcademicYearId;

       #endregion

       #region Constructor(s)

       public StoreItemDetailsDC(int aiSchoolId, int aiUserId, int aiAcademicYearId)
       {
           this.miSchoolId = aiSchoolId;
           this.miUpdatedById = aiUserId;
           this.miAcademicYearId = aiAcademicYearId;
       }

       public StoreItemDetailsDC()
       {

       }

       #endregion

       #region Public Method(s)

       /// <summary>
       /// This method is used to get store category to fill dropdown.
       /// </summary>
       /// <returns></returns>
       public List<StoreItemCategory> GetStoreItemCategories()
       {
           List<StoreItemCategory> lstStoreItemCategory = new List<StoreItemCategory>();
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_FillStoreCategory"))
               {
                   while (oSqlDataReader.Read())
                   {
                       StoreItemCategory oStoreItemCategory = new StoreItemCategory();
                       oStoreItemCategory.Id = oSqlDataReader["Id"].ToInt();
                       oStoreItemCategory.Name = oSqlDataReader["Name"].ToString();

                       lstStoreItemCategory.Add(oStoreItemCategory);
                   }
               }
               return lstStoreItemCategory;
           }
       }

       /// <summary>
       /// This method is used to get standards to fill checkbox list.
       /// </summary>
       /// <returns></returns>
       public List<StandardList> GetStandardList()
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               List<StandardList> lstStandard = new List<StandardList>();
               oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_FillStoreStandardChkBoxList"))
               {
                   while (oSqlDataReader.Read())
                   {
                       lstStandard.Add
                       (
                           new StandardList
                           {
                               Original_Standard_Id = Convert.ToInt32(oSqlDataReader["Original_Standard_Id"]),
                               Standard_Name = Convert.ToString(oSqlDataReader["Standard_Name"])
                           }
                       );
                   }
                   return lstStandard;
               }
           }
       }

       /// <summary>
       /// This method is used to save item details.
       /// </summary>
       /// <param name="aoStoreItemDetails"></param>
       public int Save(StoreItemDetails aoStoreItemDetails)
       {
           using (var oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("StoreCategoryId", aoStoreItemDetails.StoreCategoryId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Title", aoStoreItemDetails.Title, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("Description", aoStoreItemDetails.Description, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("Gender", aoStoreItemDetails.Gender, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("AssociatedStandard", aoStoreItemDetails.AssociatedStandards, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("AvailabilitySetting", aoStoreItemDetails.AvailabilitySetting, SqlDbType.Bit);

               if (aoStoreItemDetails.StartDate != DateTime.MinValue)
                oSQLServerDbUtility.AddParameter("StartDate", aoStoreItemDetails.StartDate, SqlDbType.DateTime);

               if(aoStoreItemDetails.EndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("EndDate", aoStoreItemDetails.EndDate, SqlDbType.DateTime);

               oSQLServerDbUtility.AddParameter("Price", aoStoreItemDetails.Price, SqlDbType.Decimal);
               oSQLServerDbUtility.AddParameter("Quantity", aoStoreItemDetails.Quantity, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ReorderQuantity", aoStoreItemDetails.ReOrderQuantity, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Id", aoStoreItemDetails.Id, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("IsVariation", aoStoreItemDetails.IsVariation, SqlDbType.Bit);
               oSQLServerDbUtility.AddParameter("ImageFileNames", aoStoreItemDetails.ImageFileNames, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("FileIdsToDelete", aoStoreItemDetails.FileIdsToDelete, SqlDbType.NVarChar);

               oSQLServerDbUtility.AddParameter("UOMId", aoStoreItemDetails.UOMId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("GSTCategoryId", aoStoreItemDetails.GSTCategoryId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("MRP", aoStoreItemDetails.MRP, SqlDbType.Decimal);
               oSQLServerDbUtility.AddParameter("Discount", aoStoreItemDetails.Discount, SqlDbType.Decimal);
               oSQLServerDbUtility.AddParameter("ItemCode", aoStoreItemDetails.ItemCode, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("HSNCode", aoStoreItemDetails.HSNCode, SqlDbType.NVarChar);

               SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("StoreItemMasterId", 0, SqlDbType.Int, ParameterDirection.Output);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStoreItemDetails");
               return oSqlParameter.Value.ToInt();
           }
       }

       /// <summary>
       /// This method is used to get item details.
       /// </summary>
       /// <param name="aiId"></param>
       /// <returns></returns>
       public StoreItemDetails GetStoreItemDetails(int aiId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               StoreItemDetails oStoreItemDetails = new StoreItemDetails();
                             
               oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);

               List<int> lstStandard = new List<int>();
               
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStoreItemDetails"))
               {
                   List<Attachment> lstAttachmentDetails = new List<Attachment>();
                   Attachment oAttachmentsDetails;
                   while (oSqlDataReader.Read())
                   {
                       oAttachmentsDetails = new Attachment();
                       oAttachmentsDetails.Id = oSqlDataReader["Id"].ToInt();
                       oAttachmentsDetails.ImageFileName = oSqlDataReader["ImageFileName"].ToString();

                       lstAttachmentDetails.Add(oAttachmentsDetails);
                   }
                   oSqlDataReader.NextResult();

                   while (oSqlDataReader.Read())
                   {
                       lstStandard.Add(oSqlDataReader["OriginalStandardId"].ToInt());
                   }

                   oSqlDataReader.NextResult();
                   if (oSqlDataReader.Read())
                   {
                       oStoreItemDetails.StoreCategoryId = oSqlDataReader["StoreCategoryId"].ToInt();
                       oStoreItemDetails.Title = oSqlDataReader["Title"].ToString();
                       oStoreItemDetails.Description = oSqlDataReader["Description"].ToString();
                       oStoreItemDetails.AvailabilitySetting = oSqlDataReader["SetAvailabilitySetting"].ToBool();

                       if (oSqlDataReader["SetAvailabilitySetting"].ToBool())
                       {
                           oStoreItemDetails.StartDate = oSqlDataReader["StartDate"].ToDateTime();
                           oStoreItemDetails.EndDate = oSqlDataReader["EndDate"].ToDateTime();
                       }
                       else
                       {
                           oStoreItemDetails.StartDate = DateTime.MinValue;
                           oStoreItemDetails.EndDate = DateTime.MinValue;
                       }

                       oStoreItemDetails.Price = oSqlDataReader["Price"].ToDecimal();
                       oStoreItemDetails.Quantity = oSqlDataReader["Quantity"].ToInt();
                       oStoreItemDetails.ReOrderQuantity = oSqlDataReader["ReorderQuantity"].ToInt();
                       oStoreItemDetails.Gender = oSqlDataReader["Gender"].ToString();
                       oStoreItemDetails.IsVariation = oSqlDataReader["IsVariationAvailable"].ToBool();
                       oStoreItemDetails.AreVariationExists = oSqlDataReader["AreVariationExists"].ToBool();
                       oStoreItemDetails.UOMId = oSqlDataReader["UOMId"].ToInt();
                       oStoreItemDetails.GSTCategoryId = oSqlDataReader["GSTCategoryId"].ToInt();
                       oStoreItemDetails.MRP = oSqlDataReader["MRP"].ToDecimal();
                       oStoreItemDetails.Discount = oSqlDataReader["Discount"].ToDecimal();
                       oStoreItemDetails.ItemCode = oSqlDataReader["ItemCode"].ToString();
                       oStoreItemDetails.HSNCode = oSqlDataReader["HSNCode"].ToString();
                       oStoreItemDetails.StoreItemVariationId = oSqlDataReader["StoreItemVariationId"].ToInt();
                   }
                   oStoreItemDetails.StandardList = lstStandard;
                   oStoreItemDetails.AttachmentsDetails = lstAttachmentDetails;
               }
               return oStoreItemDetails;
           }
       }

       public string Validate(string asTitle, int aiId, int aiSchoolId, int aiAcademicYearId, int aiTypeId, string asItemCode)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("Title", asTitle, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("ItemCode", asItemCode, SqlDbType.NVarChar);
               SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 500);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateStoreItem");
               return oSqlParameter.Value.ToString();
           }
       }

       #endregion
   }
}
