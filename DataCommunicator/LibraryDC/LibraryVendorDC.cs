using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Utility;
using BookEntities;

namespace DataCommunicator
{
    public class LibraryVendorDC
    {
        #region "Data Members"
        public LibraryVendors moLibraryVendor;
        #endregion "Data Members"

        #region "Constructors"
        public LibraryVendorDC()
        {
            moLibraryVendor = new LibraryVendors();
        }

        public LibraryVendorDC(int iSchoolId, int iVendorId)
        {
            moLibraryVendor = new LibraryVendors();
            GetLibraryVendorDetail(iSchoolId,iVendorId);
        }
        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to insert library vendor details.
        /// </summary>
        public void InsertLibraryVendorDC()
        {
            string sInsertStatement = "INSERT INTO" +
                                      " VendorDetails(" +
                                      "Vendor_Name" +
                                      ",MobileNumber" +
                                      ",Address" +
                                      ",SchoolId" +
                                      ",Is_Deleted" +
                                      ",InsertDate" +
                                      ",InsertedById)" +
                                      "VALUES(" +
                                      "N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.VendorName, false) +
                                      "',N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.MobileNumber, false) +
                                      "',N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.Address, false) +
                                      "'," + moLibraryVendor.SchoolId +
                                      ",N'0'" +
                                      ",N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(moLibraryVendor.InsertDate), false) +
                                      "'," + moLibraryVendor.UserId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sInsertStatement);
        }

        /// <summary>
        /// This method is used to update library vendor details.
        /// </summary>
        /// <param name="aiVendorId"></param>
        public void UpdateLibraryVendorDC(int aiVendorId)
        {
            string sUpdateStatement = "Update" +
                                      " VendorDetails SET" +
                                      " Vendor_Name =N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.VendorName, false) +
                                      "',MobileNumber =N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.MobileNumber, false) +
                                      "',Address =N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.Address, false) +
                                      "',UpdateDate =N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.UpdateDate, false) +
                                      "',UpdatedById =" + moLibraryVendor.UserId +
                                      " WHERE Vendor_Id =" + aiVendorId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete library vendor details.
        /// </summary>
        /// <param name="iVendorId"></param>
        public void DeleteLibraryVendorDC(int aiVendorId)
        {
            string sDeleteStatement = "Update" +
                                      " VendorDetails SET" +
                                      " Is_Deleted =N'1'" +
                                      "WHERE Vendor_Id=" + aiVendorId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to get paged library vendor details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<LibraryVendors> GetLibraryVendorDetailsDC(int aiSchoolId, String sSortExpression, int aiEndIndex, int startRowIndex)
        {

            List<LibraryVendors> olstLibraryVendor = new List<LibraryVendors>();
            string sFilter = (sSortExpression != string.Empty && sSortExpression != null) ? " ORDER BY " + sSortExpression : " ORDER BY Vendor_Name";
            //string sSelectStatement = GetLibraryVendorDetailsFromDatabase(aiSchoolId) + sSortExpression;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sFilter, SqlDbType.NVarChar);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetPagedVendorDetails]"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            LibraryVendors oLibraryVendor = new LibraryVendors();
                            if (oDR["Vendor_Id"] != DBNull.Value)
                                oLibraryVendor.VendorId = Convert.ToInt32(oDR["Vendor_Id"]);
                            if (oDR["Vendor_Name"] != DBNull.Value)
                                oLibraryVendor.VendorName = Convert.ToString(oDR["Vendor_Name"]);
                            if (oDR["MobileNumber"] != DBNull.Value)
                                oLibraryVendor.MobileNumber = Convert.ToString(oDR["MobileNumber"]);
                            if (oDR["Address"] != DBNull.Value)
                                oLibraryVendor.Address = Convert.ToString(oDR["Address"]);
                            olstLibraryVendor.Add(oLibraryVendor);
                        }
                    }
                }
                return olstLibraryVendor;
            }
        }

        /// <summary>
        /// This method is used to get count of total library vendor records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static int CountTotalLibraryVendorDC(int aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {

            string sSelectStatement = GetCountLibraryVendor(aiSchoolId);
            int iCount = 0;
            using (SQLServerDbUtility moSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oDR = moSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["COUNT"] != DBNull.Value)
                                iCount = Convert.ToInt32(oDR["COUNT"]);
                        }
                    }
                    return iCount;
                }
            }
        }

        /// <summary>
        /// This method is used to check whether the vendor is associated with any book or not.
        /// </summary>
        /// <param name="aiVendorId"></param>
        /// <returns></returns>
        public int GetCountAssociatedLibraryVendorDC(int aiVendorId)
        {
          
            string sSelectStatement = GetCountAssociatedLibraryVendor(aiVendorId);
            int iCount = 0;
            using (SQLServerDbUtility moSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oDR = moSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["COUNT"] != DBNull.Value)
                                iCount = Convert.ToInt32(oDR["COUNT"]);
                        }
                    
                    }
                    return iCount;
                }
            }
        }

        /// <summary>
        /// This method is used to get select statement to get details of all library vendors.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public string GetLibraryVendorDetailsFromDatabase(int aiSchoolId)
        {
            string sSelectStatement = "SELECT " +
                                      " Vendor_Id," +
                                      " Vendor_Name," +
                                      " MobileNumber," +
                                      " Address" +
                                      " FROM VendorDetails" +
                                      " WHERE Is_Deleted=0" +
                                      " AND SchoolId =" + aiSchoolId;
            return sSelectStatement;
        }

        /// <summary>
        /// This method is used to get select statement to count total library vendors.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static string GetCountLibraryVendor(int aiSchoolId)
        {
            string sSelectStatement = "SELECT " +
                                      "COUNT(Vendor_Id) AS COUNT" +
                                      " FROM VendorDetails" +
                                      " WHERE Is_Deleted=0" +
                                      " AND SchoolId =" + aiSchoolId;
            return sSelectStatement;
        }

        /// <summary>
        /// This method is used to get select statement to count library vendors associated with books.
        /// </summary>
        /// <param name="aiVendorId"></param>
        /// <returns></returns>
        public static string GetCountAssociatedLibraryVendor(int aiVendorId)
        {
            string sSelectStatement = "SELECT " +
                                      "COUNT(Book_Detail_Id) AS COUNT" +
                                      " FROM Book_Details" +
                                      " WHERE Is_Deleted=N'N'" +
                                      " AND VendorId=" + aiVendorId;
            return sSelectStatement;
        }

        #endregion "Public Methods"

        #region "Private Methods"

        /// <summary>
        /// This method is used to get library vendor details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiVendorId"></param>
        private void GetLibraryVendorDetail(int aiSchoolId, int aiVendorId)
        {
           
            List<LibraryVendors> olstLibraryVendor = new List<LibraryVendors>();
            string sFilter = " AND Vendor_Id =" + aiVendorId;
            string sSelectStatement = GetLibraryVendorDetailsFromDatabase(aiSchoolId) + sFilter;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               using( SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                if (oDR != null)
                {
                    while (oDR.Read())
                    {
                        if (oDR["Vendor_Id"] != DBNull.Value)
                            moLibraryVendor.VendorId = Convert.ToInt32(oDR["Vendor_Id"]);
                        if (oDR["Vendor_Name"] != DBNull.Value)
                            moLibraryVendor.VendorName = Convert.ToString(oDR["Vendor_Name"]);
                        if (oDR["MobileNumber"] != DBNull.Value)
                            moLibraryVendor.MobileNumber = Convert.ToString(oDR["MobileNumber"]);
                        if (oDR["Address"] != DBNull.Value)
                            moLibraryVendor.Address = Convert.ToString(oDR["Address"]);
                    }
                    
                }
            }
        }

        #endregion "Private Methods"

        public int IsVendorDuplicateDC()
        {
            string sFilter = string.Empty;
            if (moLibraryVendor.VendorId != 0)
            {
                sFilter = " AND Vendor_Id <> " + moLibraryVendor.VendorId;
            }
            string sSelectStatement = "SELECT" +
                                      " COUNT(Vendor_Id) AS COUNT" +
                                      " FROM VendorDetails" +
                                      " WHERE Is_Deleted=0" +
                                      " AND Vendor_Name =" +
                                      "N'" + StringUtility.ReplaceSingleQuoteInString(moLibraryVendor.VendorName, false) + "'" + sFilter;

            int iCount = 0;
            using (SQLServerDbUtility moSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oDR = moSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["COUNT"] != DBNull.Value)
                                iCount = Convert.ToInt32(oDR["COUNT"]);
                        }
                 
                    }
                }
                return iCount;
            }
        }
    }
}
