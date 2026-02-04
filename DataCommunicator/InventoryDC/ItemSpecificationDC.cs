/* -------------------------------------------------------------------------------
 *	DEVELOPMENT LOG
 * -------------------------------------------------------------------------------
 *	Author	: Yogesh Karne
 *	Date	: 1-Jan-2016
 *	Purpose	: Used to mark specific item as damaged.
 * -------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NewStockDetails;
using SchoolEntities.Inventory;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to mark specific item as damaged.
    /// </summary>
    public class ItemSpecificationDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public ItemSpecificationDC()
        {
        }

        public ItemSpecificationDC(int aiSchoolId, int aiUserId)
        {   
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
        } 

        #endregion

        #region "Public Method"

        /// <summary>
        /// This method is used to get homework details according to standard division.
        /// </summary>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="abFlag"></param>
        /// <returns></returns>
        public List<ItemSpecificationDetails> GetAll(int aiItemID, int aiSchoolId, string asSortExpression, int aiStartRowIndex, int aiEndRowIndex)
        {
            List<ItemSpecificationDetails> lstItemSpecification = new List<ItemSpecificationDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ItemID", aiItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + (string.IsNullOrEmpty(asSortExpression) ? "SpecificationCode" : asSortExpression), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndRowIndex, SqlDbType.Int);

                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedItemSpecificationDetails"))
                {
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                            lstItemSpecification.Add(ReadObjectFromReader(oReader));
                    }
                }
            }

            return lstItemSpecification;
        }
        
        /// <summary>
        /// This method is used to get specific item details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public ItemSpecificationDetails Get(int aiId)
        {
            ItemSpecificationDetails lstItemSpecificationDetails = new ItemSpecificationDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetItemSpecificationDetailsForEdit"))
                {
                    if (oReader.HasRows)
                    {
                            lstItemSpecificationDetails = LoadItemSpecificationData(oReader);
                    }
                }
            }

            return lstItemSpecificationDetails;
        }

        /// <summary>
        /// This method is used to get total count of Notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDisplayLocation"></param>
        /// <param name="aiShowAllNotices"></param>
        /// <returns></returns>
        public int GetCount(int aiItemID, int aiSchoolId)
        {
            int iCount = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ItemId", aiItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCountItemSpecificationDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        oSqlDataReader.Read();
                        {   
                            iCount = Convert.ToInt32(oSqlDataReader["CNT"]);
                        }
                    }
                }
            }
            return iCount;
        }

        /// <summary>
        /// This method is used to delete Item specification.
        /// </summary>
        /// <param name="aiHomeworkId"></param>
        /// <param name="asReason"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_DeleteItemSpecificationDetails]");
            }
        }

        #endregion

        #region "Private Method"

        /// <summary>
        /// This method is used read item code related details.
        /// </summary>
        /// <param name="aoReader"></param>
        /// <returns></returns>
        private StockItemDetails LoadData(SqlDataReader aoReader)
        {
            return new StockItemDetails()
            {
                ItemCode = aoReader["ItemCode"].ToString(),
                ItemName = aoReader["ItemName"].ToString()
            };
        }

        /// <summary>
        /// This method is used to read values from reader and return homework class object.
        /// </summary>
        /// <param name="aoReader"></param>
        /// <returns></returns>
        private ItemSpecificationDetails ReadObjectFromReader(SqlDataReader aoReader)
        {
            return new ItemSpecificationDetails()
            {
                Id = aoReader["Id"].ToInt(),
                ItemID = aoReader["ItemID"].ToInt(),
                SpecificationCode = aoReader["SpecificationCode"].ToString(),
                Description = aoReader["Description"].ToString(),
                IsDamaged = aoReader["IsDamaged"].ToBool(),
                DamagedDate = aoReader["DamagedDate"].Equals(DBNull.Value) ? string.Empty : Convert.ToDateTime(aoReader["DamagedDate"]).ToString(Constants.S_DATE_FORMAT),
                DamageDescription = aoReader["DamageDescription"].Equals(DBNull.Value) ? string.Empty : aoReader["DamageDescription"].ToString(),
                IsIssued = aoReader["IsIssued"].ToBool(),
                Price = aoReader["Price"].Equals(DBNull.Value) ? string.Empty : aoReader["Price"].ToString()
            };
        }

        /// <summary>
        /// This method is used to read values from reader and return homework class object.
        /// </summary>
        /// <param name="aoReader"></param>
        /// <returns></returns>
        private ItemSpecificationDetails LoadItemSpecificationData(SqlDataReader aoReader)
        {
            aoReader.Read();
            return new ItemSpecificationDetails()
            {
               Id = aoReader["Id"].ToInt(),
               SpecificationCode = aoReader["SpecificationCode"].ToString(),
               Description = aoReader["Description"].ToString(),
               IsDamaged = aoReader["IsDamaged"].ToBool(),
               DamagedDate = aoReader["DamagedDate"].Equals(DBNull.Value) ? string.Empty : Convert.ToDateTime(aoReader["DamagedDate"]).ToString(Constants.S_DATE_FORMAT),
               DamageDescription = aoReader["DamageDescription"].Equals(DBNull.Value) ? string.Empty : aoReader["DamageDescription"].ToString(),
               IsIssued = aoReader["IsIssued"].ToBool(),
               Price = aoReader["Price"].Equals(DBNull.Value) ? string.Empty : aoReader["Price"].ToString()
            };
        }

        /// <summary>
        /// This method is used to save item details.
        /// </summary>
        /// <param name="aoItemSpecificationDetails"></param>
        public void Save(ItemSpecificationDetails aoItemSpecificationDetails)
        { 
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aoItemSpecificationDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ItemId", aoItemSpecificationDetails.ItemID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SpecificationCode", aoItemSpecificationDetails.SpecificationCode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Description", aoItemSpecificationDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsDamaged", aoItemSpecificationDetails.IsDamaged, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("DamageDate", aoItemSpecificationDetails.DamagedDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("DamageDescription", aoItemSpecificationDetails.DamageDescription, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Price", aoItemSpecificationDetails.Price, SqlDbType.Decimal);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_AddItemSpecificationDetails]");
        }
        }
       
        #endregion
    }
}
