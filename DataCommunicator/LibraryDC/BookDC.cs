using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Utility;
using BookEntities;
using StudentEntities;
using SchoolEntities.Dashboard;
 


namespace DataCommunicator
{
    public class BookDC : DataCommunicatorBaseDC
    {
        public BookDC()
        {
        }

        public BookDC(int asBookId, int aiSchoolId)
        {
            LoadBookDetails(asBookId, aiSchoolId);
        }

        public List<UserRoles> LstUserRoles = new List<UserRoles>();
        public List<ClassDetails> LstClassDetails = new List<ClassDetails>();
        public List<StudentInfo> LstStudentInfo = new List<StudentInfo>();
        public List<IssueBookUserMaster> LstIssueBookUserMaster = new List<IssueBookUserMaster>();
        public List<AllBookDetails> LstAllBookDetails = new List<AllBookDetails>();
        public List<BookIssueRenewCountMaster> LstBookIssueRenewCountMaster = new List<BookIssueRenewCountMaster>();
        public int ReservedByParent { get; set; }

        public struct BookStructDetails
        {
            public Int32 miSchoolId;
            public Int32 miAcademicYearId;
            public string msBookName;
            public Int32 miBookId;
            public Int32 miBookSrNo;
            public Int32 miBookDetailsId;
            public string msBookNumber;
            public string msAuthorName;
            public Int16 miMediaType;
            public string msMainCategoryName;

            public Int32 miMainCategoryId;
            public string msPublishedBy;
            public Int32 miUser_Id;
            public char msIsDeleted;
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
            public string msDescription;
            public string msBookRemoveReason;
            public Boolean mbIsBookLost;
            public string msRackNumber;
            public string msShelfNumber;
            public string msRemark;
            public string msISBN;
            public Boolean mbIsWriteOffBook;
            public DateTime mdtWriteOffDate;
            public string msClassification;
            public string msClass;
            public Decimal miLostPercentage;
            public string msLanguage;
            public Int16 miIsForIssue;
            public List<int> lstSelectedClasses;
            public string msBookEdition;
            public string msBookYear;


            public string miCallNumber;
            public string miSeries;
            public string msStatus;
            public DateTime msPublicationDate;
          
        }

        private BookStructDetails moBookDetails;
        public SchoolBookDetails moBookInfoEntity = null;

        #region Property

        public BookStructDetails BookInfo
        {
            get
            {
                return moBookDetails;
            }
            set
            {
                moBookDetails = value;
            }
        }

        public int ReserveBookCount{get;set;}

        #endregion
        public List<SchoolBookDetails> GetPagedBookList(Int32 aiSchoolId, string asBookName, string asAccessionNumber, string asAuthorName, string asPublisher, string asLanguage, int aiStandardId, int aiMediaType, int aiBookId,int aiParentStaffId, int aiEndIndex, int aiStartRowIndex, string asSortExpression)
        {
            List<SchoolBookDetails> lstSchoolBookDetails = new List<SchoolBookDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateBookFilter(asBookName, aiMediaType, aiBookId, asAuthorName, asPublisher, asLanguage), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookNo", GetBookNumFilter(asAccessionNumber), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_iStandardId", aiStandardId, SqlDbType.Int);
                if (!string.IsNullOrEmpty(asSortExpression))
                    oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iParentStaffId", aiParentStaffId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedBooksDetails"))
                {
                    SchoolBookDetails oSchoolBookDetails;
                    while (oSqlDataReader.Read())
                    {
                        oSchoolBookDetails = new SchoolBookDetails
                        {
                            Book_Id = Convert.ToInt32(oSqlDataReader["Book_Id"]),
                            Book_Title = Convert.ToString(oSqlDataReader["Book_Title"]),
                            Is_Printable = Convert.ToInt32(oSqlDataReader["Is_Printable"]),
                            Category_Id = Convert.ToInt32(oSqlDataReader["Category_id"]),
                            Author_Name = Convert.ToString(oSqlDataReader["Author_Name"]),
                            Published_By = Convert.ToString(oSqlDataReader["Published_By"]),
                            Category_Name = Convert.ToString(oSqlDataReader["Category_Name"]),
                            Available_Books = Convert.ToInt32(oSqlDataReader["Available_Books"]),
                            Total_Book_Quantity = Convert.ToInt32(oSqlDataReader["Total_Book_Quantity"]),
                            IsForIssue = Convert.ToInt32(oSqlDataReader["IsForIssue"]),
                            Decription = Convert.ToString(oSqlDataReader["Decription"]),
                            Standards = Convert.ToString(oSqlDataReader["Standards"]),
                            Language = Convert.ToString(oSqlDataReader["Language"]),
                            Book_No = Convert.ToString(oSqlDataReader["Book_No"]),
                            AccesNo = Convert.ToInt32(oSqlDataReader["AccesNo"]) 

                        };
                        lstSchoolBookDetails.Add(oSchoolBookDetails);
                    }
                    return lstSchoolBookDetails;
                }
            }
        }

        public int GetCount(Int32 aiSchoolId, string asBookName, string asAccessionNumber, string asAuthorName, string asPublisher, string asLanguage, int aiStandardId, int aiMediaType, int aiBookId,int aiParentStaffId)
        {
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateBookFilter(asBookName, aiMediaType, aiBookId, asAuthorName, asPublisher, asLanguage), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookNo", GetBookNumFilter(asAccessionNumber), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_iStandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iParentStaffId", aiParentStaffId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetBooksCount]");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public static List<BookDetails> GetBookDetails(int aiSchoolId, string asBookName, int aiMediaType, int aiMainCategoryId, string asAuthorName, string asPublisher, string asDescription, string asAccessionNumber, int aiStandardId, string sSortExpression, string aiAccessionFromNumber, string aiAccessionTo, string asPrefix)
        {
            List<BookDetails> lstBookDetails = new List<BookDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateBookDetailsFilter(asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDescription, aiAccessionFromNumber, aiAccessionTo, asPrefix), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookNo", GetBookNumFilter(asAccessionNumber), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_iStandardId", aiStandardId, SqlDbType.Int);
                if (!string.IsNullOrEmpty(sSortExpression))
                    oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sSortExpression, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBookDetails"))
                {
                    BookDetails oBookDetails;
                    while (oSqlDataReader.Read())
                    {
                        oBookDetails = new BookDetails
                        {
                            Book_Detail_Id = Convert.ToInt32(oSqlDataReader["Book_Detail_Id"]),
                            Book_No = Convert.ToString(oSqlDataReader["Book_No"]),
                            Book_Title = Convert.ToString(oSqlDataReader["Book_Title"]),
                        };
                        lstBookDetails.Add(oBookDetails);
                    }
                }

                return lstBookDetails;
            }
        }

        //public static List<LibaryUsers> GetAllUsers(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivId)
        //{
        //    string sFilter = string.Empty;
        //        sFilter = GetFilter(asFilter, aiUserRoleId, aiStandardDivId);
        //     List<LibaryUsers> lstLibaryUsers = new List<LibaryUsers>();
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
        //        oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
        //        SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedUsersForBookIssue");
        //        LibaryUsers oLibaryUsers;
        //        while (oSqlDataReader.Read())
        //        {
        //            oLibaryUsers = new LibaryUsers
        //            {
        //                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
        //                UserRoleId = Convert.ToInt32(oSqlDataReader["UserRoleId"]),
        //                Book_Id = Convert.ToInt32(oSqlDataReader["Book_Id"]),                        
        //                RegNo = Convert.ToString(oSqlDataReader["RegNo"]),                       
        //                UserName = Convert.ToString(oSqlDataReader["FullName"]),
        //                ClassNameDesignation = Convert.ToString(oSqlDataReader["ClassNameDesignation"]),

        //            };
        //            lstLibaryUsers.Add(oLibaryUsers);
        //        }
        //        return lstLibaryUsers;
        //    }
        //}

        public static List<LibaryUsers> GetAllUsers(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivId, int aiRollNo, string sortExpression, int aiStartIndex, int aiEndIndex, string asEmployeNo)
        {
            string sFilter = GetFilter(asFilter, aiUserRoleId, aiStandardDivId);
            List<LibaryUsers> lstLibaryUsers = new List<LibaryUsers>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RollNo", aiRollNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EmployeeNo", asEmployeNo, SqlDbType.NVarChar);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedUsersForBookIssue"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstLibaryUsers.Add(new LibaryUsers
                                            {
                                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                                UserRoleId = Convert.ToInt32(oSqlDataReader["UserRoleId"]),
                                                Book_Id = Convert.ToInt32(oSqlDataReader["Book_Id"]),
                                                RegNo = Convert.ToString(oSqlDataReader["RegNo"]),
                                                UserName = Convert.ToString(oSqlDataReader["FullName"]),
                                                ClassNameDesignation = Convert.ToString(oSqlDataReader["ClassNameDesignation"]),
                                                IsActive = Convert.ToString(oSqlDataReader["IsActive"]),
                                                RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                                                EmployeeNo = oSqlDataReader["EmployeeNo"].ToString(),
                                                EnrollmentNo = oSqlDataReader["EnrolmentNo"].ToString()

                                            });
                    }
                    return lstLibaryUsers;
                }
            }
        }
        
        /// <summary>
        /// This function is used to fetch the total number of users based on the input parameters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiRollNo"></param>
        /// <returns></returns>
        public static int GetAllUsersCount(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivisionId, int aiRollNo, string asEmployeNo)
        {            
            if (aiUserRoleId == 9)
                aiUserRoleId = 3;
			string sSqlStatement = String.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RollNo", aiRollNo, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("EmployeeNo", asEmployeNo, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountUsersForBookIssue");
                return Convert.ToInt32(oSqlParameter.Value);
            }           
        }

        private static string GetBookNumFilter(string asAccessionNumber)
        {
            string sFilter = "";
            if (!String.IsNullOrEmpty(asAccessionNumber))
                sFilter = StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAccessionNumber), false);
            return sFilter;
        }


        public static string CreateBookDetailsFilter(string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, string asAuthorName, string asPublisher, string asDecription, string aiAccessionFromNumber, string aiAccessionTo, string asPrefix)
        {
            string sFilter = "";

            if (!String.IsNullOrEmpty(asBookName))
            {
                sFilter = String.Format("{0} AND Book_Title LIKE N'%{1}%'  ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asBookName), false));
            }
            if (aiMediaType != 2)
            {
                sFilter = String.Format("{0} AND SchoolWise_Book_Master.[Is_Printable] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMediaType);
            }
            if (aiMainCategoryId != 0)
            {
                sFilter = String.Format("{0} AND SchoolWise_Book_Master.[Category_Id] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMainCategoryId);
            }
            if (!String.IsNullOrEmpty(asAuthorName))
            {
                sFilter = String.Format("{0} AND [Author_Name] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAuthorName), false));
            }
            if (!String.IsNullOrEmpty(asPublisher))
            {
                sFilter = String.Format("{0} AND [Published_By] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asPublisher), false));
            }
            if (!String.IsNullOrEmpty(asDecription))
            {
                sFilter = String.Format("{0} AND [Decription] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asDecription), false));
            }
         
            if (asPrefix != string.Empty)
            {
                sFilter = sFilter + " AND Book_No LIKE '%" + asPrefix + "%'";
            }
            if (aiAccessionFromNumber != string.Empty && aiAccessionTo != string.Empty)
            {
                sFilter = sFilter + " AND Book_No IS NOT NULL AND LTRIM(RTRIM(Book_No)) <> '' AND CONVERT(INT,[DBO].[udf_GetNumericValue](Book_No))>=" + aiAccessionFromNumber + " AND CONVERT(INT,[DBO].[udf_GetNumericValue](Book_No))<=" + aiAccessionTo;
            }
            if (aiAccessionFromNumber != string.Empty && aiAccessionTo == string.Empty)
            {
                sFilter = sFilter + " AND Book_No IS NOT NULL AND LTRIM(RTRIM(Book_No)) <> '' AND CONVERT(INT,[DBO].[udf_GetNumericValue](Book_No))>=" + aiAccessionFromNumber;
            }
            if (aiAccessionFromNumber == string.Empty && aiAccessionTo != string.Empty)
            {
                sFilter = sFilter + " AND Book_No IS NOT NULL AND LTRIM(RTRIM(Book_No)) <> '' AND CONVERT(INT,[DBO].[udf_GetNumericValue](Book_No))<=" + aiAccessionTo;
            }
            return sFilter; 
        }

        public static string CreateBookFilter(string asBookName, Int32 aiMediaType, int aiBookId,  string asAuthorName, string asPublisher, string asLanguage)
        {
            string sFilter = "";

            if (!String.IsNullOrEmpty(asBookName))
            {
                sFilter = String.Format("{0} AND Book_Title LIKE N'%{1}%'  ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asBookName), false));
            }
            if (aiMediaType != 2)
            {
                sFilter = String.Format("{0} AND [Is_Printable] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMediaType);
            }

            if (aiBookId != 0)
            {
                sFilter = String.Format("{0} AND [Book_Id] =+ CAST({1}AS VARCHAR(15))", sFilter, aiBookId);
            }

            //if (aiMainCategoryId != 0)
            //{
            //    sFilter = String.Format("{0} AND [Category_Id] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMainCategoryId);
            //}
            if (!String.IsNullOrEmpty(asAuthorName))
            {
                sFilter = String.Format("{0} AND [Author_Name] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAuthorName), false));
            }
            if (!String.IsNullOrEmpty(asPublisher))
            {
                sFilter = String.Format("{0} AND [Published_By] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asPublisher), false));
            }
            if (asLanguage!="0")
            {
                sFilter = String.Format("{0} AND [Language] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asLanguage), false));
            }
            return sFilter;
        }

        /// <summary>
        /// This method is used to get the all imported Book details.
        /// </summary>
        public int CountImportRows(Int32 aiSchoolId, string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, String asAuthorName, String asPublisher, String asDecription, String asAccessionNumber)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDecription), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookNo", GetBookNoFilter(asAccessionNumber), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountImportBooks");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get count of all imported Book details.
        /// </summary>
        public DataTable GetImportBookList(Int32 aiSchoolId, String asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, String asAuthorName, String asPublisher, String asDescription, String asAccessionNumber, String sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDescription), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookNo", GetBookNoFilter(asAccessionNumber), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedImportBooks");
            }
        }

        /// <summary>
        /// This method is used to get the filetr string of applied book details filter.
        /// </summary>
        public string CreateFilter(string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, string asAuthorName, string asPublisher, string asDecription)
        {
            string sFilter = "";

            if (!String.IsNullOrEmpty(asBookName))
            {
                sFilter = String.Format("{0} AND Book_Title LIKE N'%{1}%'  ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asBookName), false));
            }
            if (aiMediaType != 2)
            {
                sFilter = String.Format("{0} AND [Is_Printable] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMediaType);
            }
            if (aiMainCategoryId != 0)
            {
                sFilter = String.Format("{0} AND [Category_Id] =+ CAST({1}AS VARCHAR(15))", sFilter, aiMainCategoryId);
            }
            if (!String.IsNullOrEmpty(asAuthorName))
            {
                sFilter = String.Format("{0} AND [Author_Name] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAuthorName), false));
            }
            if (!String.IsNullOrEmpty(asPublisher))
            {
                sFilter = String.Format("{0} AND [Published_By] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asPublisher), false));
            }
            if (!String.IsNullOrEmpty(asDecription))
            {
                sFilter = String.Format("{0} AND [Decription] LIKE N'%{1}%' ", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asDecription), false));
            }
            return sFilter;
        }


        /// <summary>
        /// This method is used to get book number as a filter book details.
        /// </summary>
        /// <param name="asAccessionNumber"></param>
        /// <returns></returns>
        private string GetBookNoFilter(string asAccessionNumber)
        {
            string sFilter = "";
            if (!String.IsNullOrEmpty(asAccessionNumber))
                sFilter = StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAccessionNumber), false);
            return sFilter;
        }

        /// <summary>
        /// This method is used to load book details.
        /// </summary>
        /// <param name="asBookId"></param>
        /// <param name="aiSchoolId"></param>
        public void LoadBookDetails(int asBookId, int aiSchoolId)  //
        {
            string sSelectStatement = "SELECT " +
                                        "Book_Id" +
                                        " , Book_Title" +
                                        " , Category_Id" +
                                        " , Category_Name" +
                                        " , Is_Printable" +
                                        " , Author_Name" +
                                        " , Published_By" +
                                        " , Language" +
                                        " , Classification" +
                                        " , LostPercentage" +
                                        " , IsForIssue" +
                                        " , Total_Book_Quantity" +
                                        " , Decription" +
                                        " , Remark" +
                                        " , ISBN" +
                                        " , RackNumber" +
                                        " , ShelfNumber" +
                                        " , BookEdition" +
                                        " , BookYear" +
                                        " , CallNumber" +
                                        " , Series" +
                                        " , Status" +
                                        " , PublicationDate" +
                                         " , Book_No" +
                                     " FROM " +
                                        " vw_Library_Total_Books" +
                                     " WHERE " +
                                           " Book_Id=" + asBookId +
                                           " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                           " AND School_Id =" + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader DR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (DR.Read())
                    {
                        if (DR["Book_Id"] != DBNull.Value)
                            moBookDetails.miBookId = Convert.ToInt32(DR["Book_Id"]);
                        if (DR["Book_Title"] != DBNull.Value)
                            moBookDetails.msBookName = Convert.ToString(DR["Book_Title"]);
                        if (DR["Category_Id"] != DBNull.Value)
                            moBookDetails.miMainCategoryId = Convert.ToInt32(DR["Category_Id"]);
                        if (DR["Category_Name"] != DBNull.Value)
                            moBookDetails.msMainCategoryName = Convert.ToString(DR["Category_Name"]);
                        if (DR["Is_Printable"] != DBNull.Value)
                            moBookDetails.miMediaType = Convert.ToInt16(DR["Is_Printable"]);
                        if (DR["Author_Name"] != DBNull.Value)
                            moBookDetails.msAuthorName = Convert.ToString(DR["Author_Name"]);
                        if (DR["Published_By"] != DBNull.Value)
                            moBookDetails.msPublishedBy = Convert.ToString(DR["Published_By"]);
                        if (DR["Language"] != DBNull.Value)
                            moBookDetails.msLanguage = Convert.ToString(DR["Language"]);
                        if (DR["Classification"] != DBNull.Value)
                            moBookDetails.msClassification = Convert.ToString(DR["Classification"]);
                        if (DR["LostPercentage"] != DBNull.Value)
                            moBookDetails.miLostPercentage = Convert.ToDecimal(DR["LostPercentage"]);
                        if (DR["IsForIssue"] != DBNull.Value)
                            moBookDetails.miIsForIssue = Convert.ToInt16(DR["IsForIssue"]);
                        if (DR["Decription"] != DBNull.Value)
                            moBookDetails.msDescription = Convert.ToString(DR["Decription"]);
                        if (DR["Remark"] != DBNull.Value)
                            moBookDetails.msRemark = Convert.ToString(DR["Remark"]);
                        if (DR["ISBN"] != DBNull.Value)
                            moBookDetails.msISBN = Convert.ToString(DR["ISBN"]);
                        if (DR["RackNumber"] != DBNull.Value)
                            moBookDetails.msRackNumber = Convert.ToString(DR["RackNumber"]);
                        if (DR["ShelfNumber"] != DBNull.Value)
                            moBookDetails.msShelfNumber = Convert.ToString(DR["ShelfNumber"]);
                        if (DR["BookEdition"] != DBNull.Value)
                            moBookDetails.msBookEdition = Convert.ToString(DR["BookEdition"]);
                        if (DR["BookYear"] != DBNull.Value)
                            moBookDetails.msBookYear = Convert.ToString(DR["BookYear"]);

                        if (DR["CallNumber"] != DBNull.Value)
                            moBookDetails.miCallNumber = Convert.ToString(DR["CallNumber"]);
                        if (DR["Series"] != DBNull.Value)
                            moBookDetails.miSeries = Convert.ToString(DR["Series"]);
                        if (DR["Status"] != DBNull.Value)
                            moBookDetails.msStatus = Convert.ToString(DR["Status"]);
                        if (DR["PublicationDate"] != DBNull.Value)
                            moBookDetails.msPublicationDate = Convert.ToDateTime(DR["PublicationDate"]);
                        if (DR["Book_No"] != DBNull.Value)
                            moBookDetails.msBookNumber = Convert.ToString(DR["Book_No"]);  //
                     }
                }
            }
        }

        /// <summary>
        /// This method is used to store the book details, book copy details and standardwise book details in database.
        /// </summary>
        /// <param name="LstSchoolBookDetails"></param>
        public void AddBook(List<SchoolBookDetails> lstAccessionDetails)
        {
            ArrayList sArrInsert = new ArrayList();
            string sInsertStatement = "INSERT INTO SchoolWise_Book_Master ( " +
               " Book_Title" +
               " , Is_Printable" +
               " , Category_Id" +
               " , Author_Name" +
               " , Published_By" +
               " , Language" +
               " , Classification" +
                " ,IsForIssue" +
               " , LostPercentage" +
               " , Decription" +
               " , Remark" +
               " , ISBN" +
               " , RackNumber" +
               " , ShelfNumber" +
               " , CallNumber" +
               " , Series" +
               " , Status" +
               " , PublicationDate" +
               " , School_Id" +
               " , Inserted_By_id" +
           ") VALUES (" +
                "  N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookName, false) + "' " +
                " , " + moBookDetails.miMediaType +
                " , " + moBookDetails.miMainCategoryId +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msAuthorName, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msPublishedBy, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msLanguage, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msClassification, false) + "' " +
                 " , " + moBookDetails.miIsForIssue +
                " , " + moBookDetails.miLostPercentage +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msDescription, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msRemark, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msISBN, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msRackNumber, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msShelfNumber, false) + "' " +
                  " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.miCallNumber, false) + "' " +
                   " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.miSeries, false) + "' " +
                " , N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msStatus, false) + "' " +
                " , N'" + moBookDetails.msPublicationDate.ToString("yyyy-MM-dd") + "'" +
                " , " + moBookDetails.miSchoolId +
                " , " + moBookDetails.miInsertedById +
           " ) ";

            sArrInsert.Add(sInsertStatement);
            sInsertStatement = GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
            sArrInsert.Add(sInsertStatement);
            string sInsertBookNo = string.Empty;
            string sInsertStandardBookDetails = string.Empty;
            if (lstAccessionDetails.Count > 0)
            {
                foreach (SchoolBookDetails oSchoolBookDetails in lstAccessionDetails)
                {
                    if (oSchoolBookDetails.DateOfPurchage != System.DateTime.MinValue)
                        sInsertBookNo = "INSERT INTO Book_Details ( " +
                                                    " Book_Id" +
                                                    " ,Book_No" +
                                                    " ,Inserted_By_id" +
                                                     " , Book_Price" +
                                                     " , TotalPages" +
                                                    " ,IsGifted" +
                                                    " ,BillNo" +
                                                    " ,PurchaseDate" +
                                                    " ,VendorId" +
                                                    " ,School_Id" +
                                                ") VALUES (" +
                                                     "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                                     " ,N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(oSchoolBookDetails.Book_No), false) + "' " +
                                                     " , " + moBookDetails.miInsertedById +
                                                     " , " + oSchoolBookDetails.Book_Price +
                                                     " , " + oSchoolBookDetails.TotalPages +
                                                      " , " + oSchoolBookDetails.IsGifted +
                                                      " , N'" + StringUtility.ReplaceSingleQuoteInString(oSchoolBookDetails.BillNo, false) + "' " +
                                                      " , N'" + oSchoolBookDetails.DateOfPurchage + "' " +
                                                       " , " + oSchoolBookDetails.VendorId +
                                                     " , " + moBookDetails.miSchoolId +
                                                      ")";


                    else
                        sInsertBookNo = "INSERT INTO Book_Details ( " +
                                               " Book_Id" +
                                               " ,Book_No" +
                                               " ,Inserted_By_id" +
                                               " , Book_Price" +
                                               " , TotalPages" +
                                               " ,IsGifted" +
                                               " ,BillNo" +
                                               " ,VendorId" +
                                               " ,School_Id" +
                                           ") VALUES (" +
                                                "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                                " ,N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(oSchoolBookDetails.Book_No), false) + "' " +
                                                " , " + moBookDetails.miInsertedById +
                                                 " , " + oSchoolBookDetails.Book_Price +
                                                 " , " + oSchoolBookDetails.TotalPages +
                                                 " , " + oSchoolBookDetails.IsGifted +
                                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(oSchoolBookDetails.BillNo, false) + "' " +
                                                 " , " + oSchoolBookDetails.VendorId +
                                                " , " + moBookDetails.miSchoolId +
                                                 ")";
                    sArrInsert.Add(sInsertBookNo);
                }
            }
            if (moBookDetails.lstSelectedClasses.Count > 0)
            {
                foreach (int iStandardValue in moBookDetails.lstSelectedClasses)
                {
                    sInsertStandardBookDetails = "INSERT INTO StandardwiseBookDetails ( " +
                                           " Book_Id" +
                                           " ,StandardId" +
                                           " ,Inserted_By_id" +
                                           " ,School_Id" +
                                           " ,AcademicYearId" +
                                       ") VALUES (" +
                                            "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                            " , " + iStandardValue +
                                            " , " + moBookDetails.miInsertedById +
                                            " , " + moBookDetails.miSchoolId +
                                            " , " + moBookDetails.miAcademicYearId +
                                             ")";
                    sArrInsert.Add(sInsertStandardBookDetails);

                }
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])sArrInsert.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to save the book copy details.
        /// </summary>
        /// <param name="LstSchoolBookDetails"></param>
        /// <param name="iBookId"></param>
        public void AddAccessionDetails(List<SchoolBookDetails> lstAccessionDetails, int iBookId)
        {
            ArrayList sArrInsert = new ArrayList();
            string sInsertBookNo = string.Empty;
            if (lstAccessionDetails.Count > 0)
            {
                foreach (SchoolBookDetails oSchoolBookDetails in lstAccessionDetails)
                {
                    if (oSchoolBookDetails.DateOfPurchage != System.DateTime.MinValue)
                        sInsertBookNo = "INSERT INTO Book_Details ( " +
                                                    " Book_Id" +
                                                    " ,Book_No" +
                                                    " ,Inserted_By_id" +
                                                     " , Book_Price" +
                                                    " , TotalPages" +
                                                    " ,IsGifted" +
                                                    " ,BillNo" +
                                                    " ,PurchaseDate" +
                                                    " ,VendorId" +
                                                    " ,School_Id" +
                                                ") VALUES (" +
                                                     "  " + iBookId +
                                                     " ,N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(oSchoolBookDetails.Book_No), false) + "' " +
                                                     " , " + moBookDetails.miInsertedById +
                                                      " , " + oSchoolBookDetails.Book_Price +
                                                    " , " + oSchoolBookDetails.TotalPages +
                                                      " , " + oSchoolBookDetails.IsGifted +
                                                      " , N'" + oSchoolBookDetails.BillNo + "' " +
                                                      " , N'" + oSchoolBookDetails.DateOfPurchage + "' " +
                                                      " , " + oSchoolBookDetails.VendorId +
                                                     " , " + moBookDetails.miSchoolId +
                                                      ")";
                    else
                        sInsertBookNo = "INSERT INTO Book_Details ( " +
                                                   " Book_Id" +
                                                   " ,Book_No" +
                                                   " ,Inserted_By_id" +
                                                    " , Book_Price" +
                                                    " , TotalPages" +
                                                   " ,IsGifted" +
                                                   " ,BillNo" +
                                                   " ,VendorId" +
                                                   " ,School_Id" +
                                               ") VALUES (" +
                                                    "  " + iBookId +
                                                     " ,N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(oSchoolBookDetails.Book_No), false) + "' " +
                                                     " , " + moBookDetails.miInsertedById +
                                                      " , " + oSchoolBookDetails.Book_Price +
                                                    " , " + oSchoolBookDetails.TotalPages +
                                                      " , " + oSchoolBookDetails.IsGifted +
                                                      " , N'" + oSchoolBookDetails.BillNo + "' " +
                                                      " , " + oSchoolBookDetails.VendorId +
                                                     " , " + moBookDetails.miSchoolId +
                                                      ")";

                    sArrInsert.Add(sInsertBookNo);
                }
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])sArrInsert.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to update Existing Book details, book copy details and standardwise book details.
        /// </summary>
        /// <param name="iBookId"></param>
        /// <param name="sArrUpdate"></param>
        public void UpdateBook(int iBookId, ArrayList sArrUpdate)
        {
            string sUpdateStatement = "UPDATE SchoolWise_Book_Master SET " +
                " Book_Title=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookName, false) + "' " +
                " , Is_Printable=" + moBookDetails.miMediaType +
                " , Category_Id=" + moBookDetails.miMainCategoryId +
                " , Author_Name=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msAuthorName, false) + "' " +
                " , Published_By=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msPublishedBy, false) + "' " +
                " , Classification=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msClassification, false) + "' " +
                " , LostPercentage=" + moBookDetails.miLostPercentage +
                " , IsForIssue=" + moBookDetails.miIsForIssue +
                " , Language=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msLanguage, false) + "' " +
                " , Decription=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msDescription, false) + "' " +
                " , Remark=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msRemark, false) + "' " +
                " , ISBN=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msISBN, false) + "' " +
                " , BookEdition=N'" + moBookDetails.msBookEdition + "' " +
                " , BookYear=N'" + moBookDetails.msBookYear + "' " +
                " , RackNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msRackNumber, false) + "' " +
                " , ShelfNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msShelfNumber, false) + "' " +
                " , CallNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.miCallNumber, false) + "' " +
                " , Series=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.miSeries, false) + "' " +
                " , Status=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msStatus, false) + "' " +
                " , PublicationDate=N'" + moBookDetails.msPublicationDate.ToString("yyyy-MM-dd") + "'" +
                " , Updated_By_Id=" + moBookDetails.miUpdatedById +
                " , Update_Date=N'" + moBookDetails.mdtUpdatedDate + "'" +
            "  WHERE " +
                " Book_Id= " + iBookId +
                " AND School_Id= " + moBookDetails.miSchoolId;
            sArrUpdate.Add(sUpdateStatement);

            if (moBookDetails.lstSelectedClasses.Count > 0)
            {
                string sDeleteClassDetails = " UPDATE StandardwiseBookDetails SET " +
                                      " Is_Deleted=N'" + Constants.C_YES + "'" +
                                      " WHERE " +
                                      " Book_Id= " + iBookId +
                                       " AND School_Id= " + moBookDetails.miSchoolId;
                sArrUpdate.Add(sDeleteClassDetails);
                foreach (int iStandardValue in moBookDetails.lstSelectedClasses)
                {

                    string sInsertClassDetails = "INSERT INTO StandardwiseBookDetails ( " +
                                             " Book_Id" +
                                             " ,StandardId" +
                                             " ,Inserted_By_id" +
                                             " ,School_Id" +
                                             " ,AcademicYearId" +
                                         ") VALUES (" +
                                              "  N'" + iBookId + "'" +
                                              " , " + iStandardValue +
                                              " , " + moBookDetails.miInsertedById +
                                              " , " + moBookDetails.miSchoolId +
                                              " , " + moBookDetails.miAcademicYearId +
                                               ")";
                    sArrUpdate.Add(sInsertClassDetails);

                }
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction((string[])sArrUpdate.ToArray(typeof(string)));
                
            }
            else
            {
                string sDeleteClassDetails = " UPDATE StandardwiseBookDetails SET " +
                                  " Is_Deleted=N'" + Constants.C_YES + "'" +
                                  " WHERE " +
                                  " Book_Id= " + iBookId +
                                   " AND School_Id= " + moBookDetails.miSchoolId;
                sArrUpdate.Add(sDeleteClassDetails);

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction((string[])sArrUpdate.ToArray(typeof(string)));
            }

           
        }

        /// <summary>
        /// This mathod is used to get the total count of books.
        /// </summary>
        /// <param name="iBookId"></param>
        /// <returns></returns>
        public int GetCopyCount(int iBookId)
        {
            string iCount = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " Book_Details " +
                                         " WHERE " +
                                         " Is_Deleted = N'" + Constants.C_NO + "' " +
                                         " AND Book_Id= " + iBookId +
                                         " AND School_Id= " + moBookDetails.miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(iCount);
            }
        }

        //Delete Existeng Book
        public void RemoveBooks()
        {
            string sWhere = string.Empty;
            if (moBookDetails.mbIsBookLost == true)
                sWhere = ", IsBookLost=N'" + moBookDetails.mbIsBookLost + "'";
            else
                sWhere = ", Is_Deleted=N'" + Constants.C_YES + "'";

            string sUpdateStatement = "UPDATE Book_Details SET " +                                            
                                            "Remove_Reason=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookRemoveReason, false) + "'" +
                                            ", Update_Date =N'" + System.DateTime.Now.ToShortDateString() + "'" +
                                            sWhere +
                                       " WHERE " +
                                            " Book_No=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookNumber, false) + "'" +
                                            " AND School_Id=" + moBookDetails.miSchoolId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is uwed to delete book details.
        /// </summary>
        public void DeleteBooks(int iBookId)
        {
            string sWhere = string.Empty;
            if (moBookDetails.mbIsBookLost == true)
                sWhere = ", IsBookLost=N'" + moBookDetails.mbIsBookLost + "'";
            else
                sWhere = ", Is_Deleted=N'" + Constants.C_YES + "'";

            string[] sQueryString = new string[3];
            sQueryString[0] = "UPDATE Book_Details SET " +                                            
                                            "Remove_Reason=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookRemoveReason, false) + "'" +
                                            ", Update_Date =N'" + System.DateTime.Now.ToShortDateString() + "'" +
                                            sWhere +
                                       " WHERE " +
                                            " Book_No=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookNumber, false) + "'" +
                                            " AND School_Id=" + moBookDetails.miSchoolId;
            sQueryString[1] = " UPDATE SchoolWise_Book_Master SET " +
                                        " Is_Deleted=N'" + (moBookDetails.mbIsBookLost? Constants.C_NO: Constants.C_YES) + "'" +
                                        ", Update_Date =N'" + System.DateTime.Now.ToShortDateString() + "'" +
                                         " WHERE " +
                                          " Book_Id= " + iBookId +
                                          " AND School_Id= " + moBookDetails.miSchoolId;
            sQueryString[2] = " UPDATE StandardwiseBookDetails SET " +
                                      " Is_Deleted=N'" + (moBookDetails.mbIsBookLost ? Constants.C_NO : Constants.C_YES) + "'" +
                                      " WHERE " +
                                      " Book_Id= " + iBookId +
                                       " AND School_Id= " + moBookDetails.miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQueryString);
        }

        /// <summary>
        /// This method is used to get main category details as per media type.
        /// </summary>
        /// <returns></returns>
        public DataTable GetMainCategoryDetails()
        {
            string sWhere = "";
            if (moBookDetails.miMediaType != 2)
            {
                sWhere = " AND [Is_Printable]=" + moBookDetails.miMediaType;
            }
            string sSelect = " SELECT Category_Id, Category_Name " +
                " FROM Book_Category " +
                " WHERE Is_Deleted = N'N' " +
                " " + sWhere +
                " AND School_Id = " + moBookDetails.miSchoolId +
                " AND Category_Id NOT IN (SELECT ISNULL(Parent_Id,0) FROM Book_Category " +
                    " WHERE Is_Deleted = N'N' " +
                    " " + sWhere +
                    " AND School_Id =" + moBookDetails.miSchoolId +
                    ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelect);
        }

        public static List<string> GetLanguages(int aiSchoolId)
        {
            List<string> lstLanguage = new List<string>();
            string sSelectStmt = "SELECT distinct Language " +
                                 " FROM SchoolWise_Book_Master " +
                                 " WHERE Is_Deleted = N'N' " +
                                 " AND School_Id = " + aiSchoolId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstLanguage.Add(oSqlDataReader["Language"].ToString());
                        }

                    }
                }
                return lstLanguage;
            }
        }

        public DataTable GetAllBooksForRemove(int aiSchoolId, int aiBookId, string sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateRemoveBookFilter(aiBookId), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetRemoveBooks");
            }
        }

        public int CountRemoveBookRows(int aiSchoolId, int aiBookId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateRemoveBookFilter(aiBookId), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountRemoveBooks");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public string CreateRemoveBookFilter(int aiBookId)
        {
            string sFilter = "";

            if (aiBookId != 0)
            {
                sFilter = String.Format("{0} AND Book_Id =+ CAST({1} AS VARCHAR(15))", sFilter, aiBookId);
            }

            return sFilter;
        }


        public bool IsDuplicateBookNo(string BookNo)
        {
            string sWhere = "";
            if (moBookDetails.miBookSrNo != 0)
            {
                sWhere = " AND Book_Detail_Id <> " + moBookDetails.miBookSrNo;
            }
            string sSelectStatement = "SELECT COUNT(*)  " +
                                            " FROM " +
                                            " Book_Details " +
                                         " WHERE " +
                                            " Book_No=N'" + StringUtility.ReplaceSingleQuoteInString(BookNo, false) + "'" +
                                            " AND (Is_Deleted = N'" + Constants.C_NO + "') " + " "
                //" OR  (Is_Deleted = '" + Constants.C_YES + "' AND IsBookLost = 1))"
                                            + sWhere;

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
        /// This method is used to get book copy details.
        /// </summary>
        /// <param name="aiBookId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetBookNoDetails(int aiBookId, int aiSchoolId)
        {
            string sSelectStatement = " SELECT " +
                    " CASE WHEN IsGifted = 1 THEN N'Yes' ELSE N'No' END AS IsGifted " +
                    " ,conveRT(NVARCHAR(50),CASE WHEN BillNo = N'0' THEN NULL ELSE BillNo END )AS BillNo" +
                    " ,ISNULL(VendorId,0) AS VendorId " +
                    " ,conveRT(NVARCHAR(25),PurchaseDate) AS PurchaseDate," +
                    "  Book_Price, (CASE WHEN TotalPages=0 THEN NULL ELSE TotalPages END) AS TotalPages ," +
                    " (SELECT  Vendor_Name " +
                        " FROM  VendorDetails  " +
                        " WHERE SchoolId =" + aiSchoolId +
                        " AND Is_Deleted = 0 " +
                        " AND (Vendor_Id = Book_Details.VendorId)) AS VendorName ," +
                    " Book_No, Book_Id, Book_Detail_Id ," +
                    " (SELECT  Book_Issue_Status " +
                        " FROM  Library_Book_Issue_Details " +
                        " WHERE School_Id =" + aiSchoolId +
                        " AND Is_Deleted = N'N' AND (Book_No = Book_Details.Book_No) " +
                        " AND (Book_Issue_Status = N'Y')) AS Book_Issue_Status ," +
                        "(SELECT ISBN FROM SchoolWise_Book_Master SBM INNER JOIN Book_Details BD ON SBM.Book_Id=BD.Book_Id WHERE BD.Book_Id=" + aiBookId + " AND BD.School_Id=" + aiSchoolId + " GROUP BY ISBN) AS ISBN" +
                " FROM " +
                    " Book_Details " +
                " WHERE " +
                    " Book_Id =" + aiBookId +
                    " AND School_Id = " + aiSchoolId +
                    " AND Is_Deleted = N'N'" +
                    " ORDER BY Book_Details.Book_No";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to update book number(Book Copy) Details.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatementForBookNo(int iBookSrNo, string sBookNo, int iBookId, int aiIsGifted, decimal adBookPrice, int aiTotalPages, string aiBillNo, int aiVendorId, string asPurchaseDate)
        {
            string sDate = "";
            if (asPurchaseDate == string.Empty)
                sDate = " , PurchaseDate=null ";
            else
                sDate = " , PurchaseDate=N'" + asPurchaseDate + "' ";
            string sUpdateBookNo = "UPDATE Book_Details SET " +
                                               " Book_No=N'" + StringUtility.ReplaceSingleQuoteInString(sBookNo, false) + "' " +
                                               " ,Updated_By_Id=" + moBookDetails.miUpdatedById +
                                               " , IsGifted=" + aiIsGifted +
                                               " , TotalPages=" + aiTotalPages +
                                               " , Book_Price=" + adBookPrice +
                                               " , BillNo=N'" + aiBillNo + "' " +
                                                sDate +
                                               " , VendorId=" + aiVendorId +
                                               " ,Update_Date=N'" + moBookDetails.mdtUpdatedDate.ToShortDateString() + "'" +
                                           " WHERE  Book_Id=" + iBookId +
                                               " AND Book_Detail_Id=" + iBookSrNo +
                                               " AND School_Id=" + moBookDetails.miSchoolId;
            return sUpdateBookNo;
        }

        public DataTable RetriveAllBooks(int aiSchoolId, string sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetBookDetails");
            }
        }

        public int CountRetriveRows(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountBooksDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public DataTable FetchUserDetails(int iUserId, int iSchoolId)
        {
            string sSelectStatement = "SELECT " +
                                    " Book_No" +
                                    " ,Book_Title" +
                                    " ,Author_Name" +
                                    " ,Issue_Date" +
                                    " ,Return_Date" +
                                  " FROM " +
                                    " vw_BookIssueDetails" +
                                  " WHERE " +
                                    " Book_Issued_To=" + iUserId +
                                    " AND  School_Id=" + iSchoolId +
                                    " AND Book_Issue_Status=N'" + Constants.C_YES + "'" +
                                    " AND Is_Deleted=N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to write off book copy from the database.
        /// </summary>
        public void WriteOffBookCopy()
        {
            string sUpdateStatement = " UPDATE [Book_Details] " +
                " SET " +
                    "[Remove_Reason] = N'" + moBookDetails.msBookRemoveReason + "'" +
                    ",[IsWriteOffBook] = N'" + moBookDetails.mbIsWriteOffBook + "'" +
                    ",[WriteOff_Date] =N'" + moBookDetails.mdtWriteOffDate + "'" +
                    ",[Update_Date] = N'" + System.DateTime.Now + "'" +
                    ",[Updated_By_Id] =" + moBookDetails.miUpdatedById +
                " WHERE " +
                    " Book_Detail_Id =" + moBookDetails.miBookDetailsId +
                    " and [School_Id] =" + moBookDetails.miSchoolId +
                    " AND Is_Deleted=N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        private static string GetFilter(string asFilter, int aiUserRoleId, int StandardDivId)
        {
			asFilter = asFilter != null ? asFilter : String.Empty;
            return String.Format(" AND (FirstName LIKE N'%{0}%' OR MiddleName LIKE N'%{0}%' OR LastName LIKE N'%{0}%' OR FullName LIKE N'%{0}%') ", StringUtility.ReplaceSingleQuoteInString(asFilter, false));
        }

        public void GetUserRoles()
        {
            string sSelectStatement = "SELECT User_Role_Id " +
                                      ",User_Role_Name " +
                                      " FROM User_Role_Master " +
                                      " WHERE User_Role_Id <>8" +
                                      " AND User_Role_Id <> 10";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    UserRoles oUserRoles;
                    while (oSqlDataReader.Read())
                    {
                        oUserRoles = new UserRoles
                        {
                            User_Role_Id = Convert.ToInt32(oSqlDataReader["User_Role_Id"]),
                            User_Role_Name = Convert.ToString(oSqlDataReader["User_Role_Name"]),
                        };
                        LstUserRoles.Add(oUserRoles);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to get student's class and devision list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetStudentClassNames(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                                      " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id  AS Value_Member" +
                                      " , Standard_Master.Standard_Name+N' - '+Division_Master.Division_Name AS Display_Member" +
                                      " FROM " +
                                      " SchoolWise_Standard_Division_Master INNER JOIN " +
                                      "  Standard_Master ON " +
                                      " SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id INNER JOIN " +
                                      " Division_Master ON " +
                                      " SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id " +
                                      " WHERE " +
                                      " SchoolWise_Standard_Division_Master.School_Id = " + aiSchoolId +
                                      " AND " +
                                      " SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " AND " +
                                      " SchoolWise_Standard_Division_Master.academic_year_id =" + aiAcademicYearId +
                                      " ORDER BY " +
                                      " Standard_Master.Standard_Id,Division_Master.Division_Id ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    ClassDetails oClassDetails;
                    while (oSqlDataReader.Read())
                    {
                        oClassDetails = new ClassDetails
                        {
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                            Classname = Convert.ToString(oSqlDataReader["Display_Member"]),
                        };
                        LstClassDetails.Add(oClassDetails);
                    }
                }
            }
        }
               
        /// <summary>
        /// This method is used to get saved standards list for book.
        /// </summary>
        public List<ClassDetails> GetSavedStandards(int aiSchoolId, int aiBookId, int aiAcademicYearId)
        {
            List<ClassDetails> LstClassDetails = new List<ClassDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = " SELECT " +
                                        " StandardId, Book_Id " +
                                        " FROM StandardwiseBookDetails " +
                                        " WHERE " +
                                        " School_Id=" + aiSchoolId +
                                        " AND AcademicYearId=" + aiAcademicYearId +
                                        " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                        " AND Book_Id=" + aiBookId;
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    ClassDetails oClassDetails;
                    while (oSqlDataReader.Read())
                    {
                        oClassDetails = new ClassDetails
                        {
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["StandardId"]),
                        };
                        LstClassDetails.Add(oClassDetails);
                    }
                }
                return LstClassDetails;
            }
        }

        /// <summary>
        /// This method is used to get standards list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public static List<ClassDetails> GetStandards(int aiSchoolId, int aiAcademicYearId)
        {
            List<ClassDetails> lstClassDetails = new List<ClassDetails>();
            string sSelectStatement = " SELECT " +
                                               " Standard_Id  AS Value_Member" +
                                               " , Standard_Name AS Display_Member" +
                                               " FROM " +
                                               " Standard_Master  " +
                                               " WHERE " +
                                               " Standard_Master.School_Id = " + aiSchoolId +
                                               " AND " +
                                               " Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                               " AND " +
                                               " Standard_Master.academic_year_id =" + aiAcademicYearId +
                                               " ORDER BY " +
                                               " Standard_Master.Original_Standard_Id ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    ClassDetails oClassDetails;
                    while (oSqlDataReader.Read())
                    {
                        oClassDetails = new ClassDetails
                        {
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                            Classname = Convert.ToString(oSqlDataReader["Display_Member"]),
                        };
                        lstClassDetails.Add(oClassDetails);
                    }
                }
                return lstClassDetails;
            }
        }
        /// <summary>
        /// This method is used to get library Vendors list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static List<LibraryVendors> GetLibraryVendors(int aiSchoolId)
        {
            List<LibraryVendors> lstLibraryVendors = new List<LibraryVendors>();
            string sSelectStatement = "SELECT " +
                                       " Vendor_Id" +
                                       " ,Vendor_Name" +
                                    " FROM " +
                                       " VendorDetails" +
                                    " WHERE " +
                                    " SchoolId =" + aiSchoolId +
                                       " AND Is_Deleted =0 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    LibraryVendors oLibraryVendor;
                    while (oSqlDataReader.Read())
                    {
                        oLibraryVendor = new LibraryVendors
                        {
                            VendorId = Convert.ToInt32(oSqlDataReader["Vendor_Id"]),
                            VendorName = Convert.ToString(oSqlDataReader["Vendor_Name"]),
                        };
                        lstLibraryVendors.Add(oLibraryVendor);
                    }
                }
                return lstLibraryVendors;
            }
        }
        public bool IsDuplicateBook()
        {
            string sWhere = "";
            if (moBookDetails.miBookId != 0)
            {
                sWhere = " AND Book_ID <>" + moBookDetails.miBookId;
            }
            string sSelectStatement = "SELECT COUNT(*) FROM vw_GetLibraryBookDetails " +
                                                      " WHERE " +
                                                          " Book_Title=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msBookName, false) + "' " +
                                                          " AND Author_Name=N'" + StringUtility.ReplaceSingleQuoteInString(moBookDetails.msAuthorName, false) + "' " +
                                                           " AND Category_Id=" + moBookDetails.miMainCategoryId +
                                                          " AND Is_Printable=" + moBookDetails.miMediaType +
                                                          " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                                          sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (iCount > 0)
                    return false;
                else
                    return true;
            }
        }

        public bool IsAssignedCategory(string sCategoryName, int iMediaType, string sSubCategory)
        {
            string sSelectStatement=string.Empty;
            if(sSubCategory!=string.Empty)
                sSelectStatement = " select COUNT(*)" +
                                            " FROM Book_Category " +
                                            " WHERE " +
                                            " Category_Id IN(SELECT Category_Id " +
                                                            " FROM SchoolWise_Book_Master " +
                                                            " WHERE Is_Deleted=N'" + Constants.C_NO + "'" +
                                                            ")" +
                                            " AND Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(sCategoryName, false) + "' " +
                                            " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                            " AND Is_Printable=" + iMediaType+
                                            " AND Parent_Id=0";
            else
                sSelectStatement = " select COUNT(*)" +
                                                    " FROM Book_Category " +
                                                    " WHERE " +
                                                    " Category_Id  IN(SELECT Parent_Id " +
                                                                   " FROM Book_Category " +
                                                                   " WHERE Is_Deleted=N'" + Constants.C_NO + "'" +
                                                                   ")" +
                                                    " AND Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(sCategoryName, false) + "' " +
                                                    " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                                    " AND Is_Printable=" + iMediaType;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (iCount > 0)
                    return false;
                else
                    return true;
            }
        }

        public static List<BookDetails> GetBookDetailsForBarcodes(int aiSchoolId, int aiAcademicYearId)
        {
            List<BookDetails> lstBookDetails = new List<BookDetails>();
            string sSelectStatement = "SELECT * FROM Book_Details INNER JOIN SchoolWise_Book_Master ON Book_Details.Book_Id=SchoolWise_Book_Master.Book_Id  WHERE Book_Details.Is_Deleted=N'N' AND Book_Details.School_Id=" + aiSchoolId;


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    BookDetails oBookDetails;
                    while (oSqlDataReader.Read())
                    {
                        oBookDetails = new BookDetails
                        {
                            Book_Id = Convert.ToInt32(oSqlDataReader["Book_Id"]),
                            Book_Detail_Id = Convert.ToInt32(oSqlDataReader["Book_Detail_Id"]),
                            Book_No = Convert.ToString(oSqlDataReader["Book_No"]),
                            Book_Title = Convert.ToString(oSqlDataReader["Book_Title"]),
                            SchoolId = Convert.ToInt32(oSqlDataReader["School_Id"]),

                        };
                        lstBookDetails.Add(oBookDetails);
                    }

                }

                return lstBookDetails;
            }
        }
        public void SaveReserveBook()
        {
            string sInsertStmt = "INSERT INTO [dbo].[library_Book_Reservation] "+
                                "("+
                                " [BookId]"+
                                ",[UserId]"+
                                ",[IsIssued]"+
                                ",[IsCanceled]"+
                                ",[ReservedByParent]"+
                                ",[SchoolId]"+
                                ",[AcademicYearId]"+
                                ",[IsDeleted]"+
                                ",[InsertedById]"+
                                " ) "+
                                "VALUES( "+
                                 "  "+moBookDetails.miBookId+
                                 "  ,"+moBookDetails.miUser_Id +
                                 "  , 0 "+
                                 "  , 0 "+
                                 "  , "+ReservedByParent +
                                 "  ,"+moBookDetails.miSchoolId+" "+
                                 "  ,"+moBookDetails.miAcademicYearId+ " "+
                                 "  , 0"+
                                 ","+ moBookDetails.miInsertedById+")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sInsertStmt);
            };
        }

        public static int GetReserveBookCount(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiBookId, int aiFlag)
        {
            int cnt = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BookId", aiBookId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Flag", aiFlag, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetReserveBooksCount"))
                {
                    ///////////
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            if (oSqlDataReader["Count"] != DBNull.Value)
                                cnt = Convert.ToInt32(oSqlDataReader["Count"]);
                        }
                    }
                };
                return cnt;
            }
        }

        public static int GetReserveBooksPerPerson(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId)
        {
            string sSelectStmt = "SELECT Reserve_Books_Per_Person " +
                               "FROM Library_Configuration_Master " +
                               "WHERE User_Role_Id=" +aiUserRoleId +"" +
                               "AND Is_Deleted=N'N' " +
                               "AND School_Id=" + aiSchoolId + " " +
                               "AND Academic_Year_Id=" + aiAcademicYearId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStmt);
            };
        }

        public List<LibaryUsers> GetReservedBookDetails(int aiSchoolId, int aiAcademicYearId,
           int aiUserId, string asBookTitle, string asUserName, int aiStartIndex, int aiEndIndex, string asSortExpression, int aiAllUser)
        {
            if (asBookTitle != string.Empty)
                asBookTitle = StringUtility.ReplaceSingleQuoteInString(asBookTitle, true);
            List<LibaryUsers> lstReservedBooks = new List<LibaryUsers>();
            LibaryUsers oLibaryUsers;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserID", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BookTitle", asBookTitle, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserName", asUserName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AllUserFlag", aiAllUser, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetReserveBookDetails"))
                {

                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oLibaryUsers = new LibaryUsers()
                            {
                                Book_Id = Convert.ToInt32(oSqlDataReader["BookId"]),
                                Book_Title = oSqlDataReader["Book_Title"].ToString(),
                                UserName = oSqlDataReader["Name"].ToString(),
                                ClassNameDesignation = oSqlDataReader["Class"].ToString(),
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                IsForParent = Convert.ToBoolean(oSqlDataReader["ReservedByParent"]),
                                ReservationDate = oSqlDataReader["ReservationDate"].ToString(),
                                Designation = oSqlDataReader["Designation"].ToString()
                            };
                            lstReservedBooks.Add(oLibaryUsers);
                        }
                    }
                    if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                        ReserveBookCount = Convert.ToInt32(oSqlDataReader["Count"]);
                };
                return lstReservedBooks;
            }
        }

        public void CancelBookReservation(int aiUserId,int aiBookid,int aiSchoolId,int aiAcademicYearId)
        { 
           
                                
            string sUpdateStmt= " UPDATE library_Book_Reservation "+
                                " SET IsCanceled=1 "+
                                ",UpdatedById="+aiUserId+ 
                                ",UpdatedDate=GETDATE()" +
                                " WHERE BookId="+aiBookid +
                                " AND SchoolId= "+aiSchoolId+
                                " AND AcademicYearId="+aiAcademicYearId;
            if (aiUserId != 0)
                sUpdateStmt += "AND UserId=" + aiUserId;
                                
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sUpdateStmt);
            }
        }

        /// <summary>
        /// This nethod is used to get all usersfor issue book. 
        /// </summary>
        /// <param name="asFilterXml"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetAllUsersForIssueBook(string asFilterXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("FilterXml", asFilterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", BookInfo.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", BookInfo.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetIssueRenewReturnUserDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillUserDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetLibraryConfigDetails(oSqlDataReader);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to set user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            IssueBookUserMaster oIssueBookUserMaster;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {

                    oIssueBookUserMaster = new IssueBookUserMaster()
                    {
                       IssueBookUserRollNoDesig = new IssueBookUserRollNoDesig()
                        {
                            RollNoEmployeeNo = aoSqlDataReader["RollNoEmployeeNo"].ToString(),
                            ClassNameDesignation = aoSqlDataReader["ClassNameDesignation"].ToString(),
                            EnrollmentNo = aoSqlDataReader["EnrollmentNo"].ToString()

                        },
                        RowNo = Convert.ToInt32(aoSqlDataReader["RowNo"].ToString()),
                        UserId = Convert.ToInt32(aoSqlDataReader["UserId"].ToString()),
                        UserName = aoSqlDataReader["UserName"].ToString(),
                        StandardDivisionId = Convert.ToInt32(aoSqlDataReader["StandardDivisionId"].ToString()),
                        IsActive = aoSqlDataReader["IsActive"].ToString(),
                        BookwiseRenewCount =aoSqlDataReader["BookwiseRenewCount"].ToString().Replace("'","&"),
                        UserIssueBookCount = Convert.ToInt32(aoSqlDataReader["UserIssueBookCount"].ToString()),
                        HasLateEntry = Convert.ToBoolean(aoSqlDataReader["HasLateEntry"]),
                       EnrollmentNo = aoSqlDataReader["EnrollmentNo"].ToString()

                    };
                   LstIssueBookUserMaster.Add(oIssueBookUserMaster);
                }
            }
        }

        public void SetAllBookDetailsInString(SqlDataReader aoSqlDataReader)
        {
            StringBuilder sBookNo = new StringBuilder();
            StringBuilder sBookDetailsId = new StringBuilder();
            StringBuilder sBookName = new StringBuilder();
            AllBookDetails oAllBookDetails;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    sBookNo.Append(',' + aoSqlDataReader["BookNo"].ToString());
                    sBookDetailsId.Append(',' + aoSqlDataReader["BookDetailsId"].ToString());
                    sBookName.Append(',' + aoSqlDataReader["BookName"].ToString());
                }
                oAllBookDetails = new AllBookDetails()
                {
                    BookNo = sBookNo.ToString().Remove(0, 1),
                    BookDetailsId = sBookDetailsId.ToString().Remove(0, 1),
                    BookName = sBookName.ToString().Remove(0, 1),
                };
                LstAllBookDetails.Add(oAllBookDetails);
            }
        }

        public void SetLibraryConfigDetails(SqlDataReader aoSqlDataReader)
        {
            BookIssueRenewCountMaster oBookIssueRenewCountMaster;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oBookIssueRenewCountMaster = new BookIssueRenewCountMaster()
                    {
                        MaxIssueBookCount = Convert.ToInt32(aoSqlDataReader["MaxIssueBookCount"].ToString()),
                        MaxRenewBookCount = Convert.ToInt32(aoSqlDataReader["MaxRenewBookCount"].ToString()),
                        LateFeePerDay = Convert.ToInt32(aoSqlDataReader["LateFeePerDay"].ToString()),
                        LateFeeEffectiveFrom = Convert.ToInt32(aoSqlDataReader["LateFeeEffectiveFrom"].ToString())
                    };
                    LstBookIssueRenewCountMaster.Add(oBookIssueRenewCountMaster);
                }
            }
        }

    }


    public class BookCollectionDC
    {
        private int miSchoolId;
        private int miAcadamicId;
        private int miInsertById;

        public BookCollectionDC()
        {
        }

        public BookCollectionDC(int aiSchoolId, int aiAcademicId, int aiInsertById)
        {
            miSchoolId = aiSchoolId;
            miAcadamicId = aiAcademicId;
            miInsertById = aiInsertById;
        }

        /// <summary>
        /// This method is used to insert multiple books while importing books.
        /// </summary>
        public void InsertMultipleBooks(string asBookDetails, string asBookNoDetails, int aiOriginalConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcadamicId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", miInsertById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BookDetails", asBookDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("StandardDetails", asBookNoDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("OriginalConfigId", aiOriginalConfigId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ImportMultipleBooks");
            }
        }

        /// <summary>
        /// this method is used to get library count details to show on statistics widget.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static LibraryCountDetails GetLibraryCountDetails(int aiSchoolId, bool abIsServiceCall = false)
        {
            LibraryCountDetails oLibraryCountDetails = new LibraryCountDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLibraryStatistics"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oLibraryCountDetails = new LibraryCountDetails()
                        {
                            TotalCount = Convert.ToInt16(oSqlDataReader["TotalCount"]),
                            ReceivedCount = Convert.ToInt16(oSqlDataReader["ReceivedCount"]),
                            PurchasedCount = Convert.ToInt16(oSqlDataReader["PurchasedCount"]),
                            LostCount = Convert.ToInt16(oSqlDataReader["LostCount"]),
                        };
                    }
                }
            }

            return oLibraryCountDetails;
        }


    }
}
