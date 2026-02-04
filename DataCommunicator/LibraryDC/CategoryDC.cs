using System;
using System.Data;
using System.Data.SqlClient;
using Utility;


namespace DataCommunicator
{
    public class CategoryDC : DataCommunicatorBaseDC
    {

        public CategoryDC()
        {
        }

        public CategoryDC(int aiCatagoryID)
        {
            LoadCategoryDetails(aiCatagoryID);
        }

        /// <summary>
        /// This method is used to load category details and set category data object.
        /// </summary>
        /// <param name="aiCatagoryID"></param>
        private void LoadCategoryDetails(int aiCatagoryID)
        {
            string sSelectStatement = GetSelectStatement(aiCatagoryID);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader DR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (DR.Read())
                    {
                        if (DR["Category_Name"] != null)
                            moCategoryDetails.msCategoryName = Convert.ToString(DR["Category_Name"]);
                        if (DR["Category_Id"] != null)
                            moCategoryDetails.miCategoryId = Convert.ToInt32(DR["Category_Id"]);
                        if (DR["Is_Printable"] != null)
                            moCategoryDetails.miIsPrintable = Convert.ToInt16(DR["Is_Printable"]);
                        if (DR["Parent_Id"] != null)
                            moCategoryDetails.miParentId = Convert.ToInt32(DR["Parent_Id"]);
                        if (DR["Category_Level"] != null)
                            moCategoryDetails.miCategoryLevel = Convert.ToInt32(DR["Category_Level"]);
                        if (DR["School_Id"] != null)
                            moCategoryDetails.miSchoolId = Convert.ToInt32(DR["School_Id"]);
                    }
                }
            }
        }

        public CategoryDC(int aiCategory, int aiSchoolId, int aiAcademicYearId)
        {
            LoadCategoryDetails(aiCategory, aiSchoolId, aiAcademicYearId);
        }

        public struct CategoryStructDetails
        {
            public Int32 miSchoolId;

            public string msCategoryName;
            public Int32 miCategoryId;
            public string msSubCategoryName;
            public Int32 miSubCategoryId;
            public Int32 miCategoryLevel;
            public Int32 miParentId;
            public Int16 miIsPrintable;

            public Int32 miUserId;
            public char msIsDeleted;
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
        }

        private CategoryStructDetails moCategoryDetails;

        #region Property
        public CategoryStructDetails CategoryInfo
        {
            get
            {
                return moCategoryDetails;
            }
            set
            {
                moCategoryDetails = value;
            }

        }
        #endregion

        public void LoadCategoryDetails(int aiCategory, int aiSchoolId, int aiAcademicYearId)
        {

            string sSelectStatement = "SELECT" +
                                        " Category_Id" +
                                        ",Category_Name" +
                                      " FROM " +
                                        " Book_SubCategory" +
                                      " WHERE " +
                                           " School_Id =" + aiSchoolId +
                                           " AND Category_Id=" + aiCategory + "";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader DR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (DR.Read())
                    {
                        if (DR["Category_Name"] != null)
                            moCategoryDetails.msCategoryName = Convert.ToString(DR["Category_Name"]);
                        if (DR["Category_Id"] != null)
                            moCategoryDetails.miCategoryId = Convert.ToInt32(DR["Category_Id"]);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to get select statement to fill catagory details.
        /// </summary>
        /// <param name="aiCatagoryID"></param>
        /// <returns></returns>
        private string GetSelectStatement(int aiCatagoryID)
        {
            string sSelectStatement = "SELECT  " +
                                        " Category_Id" +
                                        ", Category_Name" +
                                        ", Is_Printable" +
                                        ", Parent_Id" +
                                        ", Category_Level" +
                                        ", School_Id" +
                                      " FROM " +
                                        " Book_Category " +
                                        " WHERE " +
                                        " (Is_Deleted = N'N') " +
                                        " AND Category_Id =" + aiCatagoryID;

            return sSelectStatement;
        }

        public DataSet RetriveCategoryList()
        {
            string sFetchStatement = "SELECT  * FROM vw_Book_Categary_Details" +
                                       " WHERE " +
                                           " School_Id =" + moCategoryDetails.miSchoolId +                                       
                                       " AND Is_Deleted=N'" + Constants.C_NO + "'"+
                                           " ORDER BY Is_Printable";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sFetchStatement);
        }

        public DataTable RetriveMainCategoryList()
        {
            string sWhere="";
            if (moCategoryDetails.miCategoryId != 0)
            {
                sWhere = " AND Category_Id= " + moCategoryDetails.miCategoryId;
            }

            string sSelect = "SELECT " +
                                        " Category_Id" +
                                        ",Category_Name" +
                                        ",Parent_Id" +
                                        ",Category_Level" +
                                        ",Is_Printable" +
                                      " FROM " +
                                        " Book_Category" +
                                      " WHERE " +
                                        " School_Id=" + moCategoryDetails.miSchoolId + " " +
                                      //" AND Is_Printable=" + moCategoryDetails.miIsPrintable +
                                        " AND Is_Deleted=N'" + Constants.C_NO + "'"
                                      + sWhere;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelect);
        }

        /// <summary>
        /// This methos is used to add category details in database.
        /// </summary> 
        public void AddCategory()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_CategoryName", StringUtility.ReplaceSingleQuoteInString(moCategoryDetails.msCategoryName, false), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_IsPrintable", moCategoryDetails.miIsPrintable, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_ParentId", moCategoryDetails.miParentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_School_Id", moCategoryDetails.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_UserId", moCategoryDetails.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertBookCategory");
            }
        }

        //Update Existeng Category
        public void UpdateCategory()
        {
            string sUpdateStatement = "UPDATE Book_Category SET " +
                " Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(moCategoryDetails.msCategoryName, false) + "' " +
                " , Updated_By_Id=" + moCategoryDetails.miUpdatedById +
                " , Update_Date=N'" + moCategoryDetails.mdtUpdatedDate.ToShortDateString() + "'" +
                "  WHERE " +
                " Category_Id=" + moCategoryDetails.miCategoryId +
                " AND School_Id= " + moCategoryDetails.miSchoolId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        //This method is usedt to Update Existeng SubCategory.
        public void UpdateSubCategory()
        {
            string sUpdateStatement = "UPDATE Book_Category " +
                " SET " +
                    " Category_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moCategoryDetails.msSubCategoryName, false) + "' " +
                    ",Updated_By_Id = " + moCategoryDetails.miUpdatedById +
                    ",Update_Date = N'" + moCategoryDetails.mdtUpdatedDate.ToShortDateString() + "'" +
                " WHERE " +
                    " Category_Id = " + moCategoryDetails.miSubCategoryId +
                    " AND School_Id = " + moCategoryDetails.miSchoolId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        //Delete Existeng Category
        public void DeleteCategory()
        {
            string sDeleteStatement = " UPDATE Book_Category SET " +
                                      " Is_Deleted=N'" + Constants.C_YES + "'" +
                                      " , Updated_By_Id=" + moCategoryDetails.miUpdatedById +
                                " WHERE " +
                                        " School_Id= " + moCategoryDetails.miSchoolId +
                                        " AND Category_Id = " + moCategoryDetails.miCategoryId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to check duplicate category name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateCategory()
        {
            string sWhere = "";

            if (moCategoryDetails.miCategoryId != 0)
            {
                sWhere = " AND Category_Id <> " + moCategoryDetails.miCategoryId;
            }
            string sSelectStatement = "SELECT COUNT(*) FROM Book_Category " +
                                                            " WHERE " +
                                                            " Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(moCategoryDetails.msCategoryName, false) + "' " +
                                                          "AND " +
                                                            " Is_Deleted=N'" + Constants.C_NO + "'" +
                                                            " AND Is_Printable =" + moCategoryDetails.miIsPrintable +
                                                            sWhere;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// This method is used to check duplicate sub category in database side.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateSubCategory()
        {
            string sWhere = "";

            if (moCategoryDetails.miSubCategoryId != 0)
            {
                sWhere = " AND Category_Id <> " + moCategoryDetails.miSubCategoryId;
            }
            string sSelectStatement = "SELECT COUNT(*) FROM Book_Category  " +
            " WHERE " +
                " Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(moCategoryDetails.msCategoryName, false) + "' " +
                " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                " AND Parent_Id=" + moCategoryDetails.miCategoryId +
                sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
