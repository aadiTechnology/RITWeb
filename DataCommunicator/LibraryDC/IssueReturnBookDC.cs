using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Utility;
using BookEntities;


namespace DataCommunicator
{
    public class IssueReturnBookDC : DataCommunicatorBaseDC
    {
        public List<IssueReturnDateMaster> LstIssueReturnDateMaster = new List<IssueReturnDateMaster>();
        public List<UserBookRenewDetails> LstUserBookRenewDetails = new List<UserBookRenewDetails>();

        public struct IssueReturnBookStruct
        {
            public Int32 miSchoolId;
            public Int32 miAcademicYearId;
            public string msBookName;
            public Int32 miBookId;
            public string msBookNo;
            public string msAuthorName;
            public string msCategoryName;
            public Int32 miCategoryId;
            public string msPublishedBy;
            public Int32 miPrice;
            public Int32 miQuantityAvailable;
            public Int32 miQuantityRemoved;
            public Int32 miUser_Id;
            public char msIsDeleted;
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
            public DateTime mdtIssueDate;
            public string mdtReturnDate;
            public string mdtRenewDate;
            public Int32 miIssueID;
            public string mdtActualReturnDate;
            public Int32 miBookDetailsId;
            public Int32 miBookIssueId;
            public Int32 miRenewAttempts;
            public Int32 miLateFee;
            public Int32 miBookIssuedTo;
            public Int32 miIsForParent;
        }

        private IssueReturnBookStruct moIssueReturnBookStruct;

        #region Property
        public IssueReturnBookStruct IssueReturnBookInfo
        {
            get
            {
                return moIssueReturnBookStruct;
            }
            set
            {
                moIssueReturnBookStruct = value;
            }

        }
        #endregion

        /// <summary>
        /// This method is used to issue book with respect to the  user id
        /// </summary>
        public void IssueBook()
        {
            string sStatement = "";
            sStatement = "INSERT INTO Library_Book_Issue_Details (" +
                                             "Book_ID" +
                                             ",Book_Detail_Id" +
                                             ",Book_No" +
                                             ",Issue_Date" +
                                             ",Book_Issued_To" +
                                             ",Book_Issue_Status" +
                                             ",Issued_By_id" +
                                             ",School_Id" +
                                             ",Academic_Year_Id" +
                                             ",Return_Date" +
                                             ",IsForParent" +
                                             ")" +
                                        " VALUES(" +
                                            "" + moIssueReturnBookStruct.miBookId +
                                            "," + moIssueReturnBookStruct.miBookDetailsId +
                                            ",N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(moIssueReturnBookStruct.msBookNo), false) + "'" +
                                            ",CAST (N'" + moIssueReturnBookStruct.mdtIssueDate + "' AS SMALLDATETIME)" +
                                            "," + moIssueReturnBookStruct.miIssueID +
                                            ",N'" + Constants.C_YES + "'" +
                                            "," + moIssueReturnBookStruct.miInsertedById +
                                            "," + moIssueReturnBookStruct.miSchoolId +
                                            "," + moIssueReturnBookStruct.miAcademicYearId +
                                            ",CAST (N'" + moIssueReturnBookStruct.mdtReturnDate + "' AS SMALLDATETIME)" +
                                            "," + moIssueReturnBookStruct.miIsForParent +
                                            ");";

            sStatement += "UPDATE library_Book_Reservation " +
                                      " SET IsIssued=1,UpdatedDate=GETDATE(),UpdatedById=" + moIssueReturnBookStruct.miInsertedById +
                                      " WHERE BookId=" + moIssueReturnBookStruct.miBookId +
                                      " AND UserId=" + moIssueReturnBookStruct.miIssueID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sStatement);
        }

        /// <summary>
        /// This method is used to issue book to user. 
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReturnDate"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="asAccessionNoBarcode"></param>
        public void IssueBookToUser(int aiUserId, string aiReturnDate, int aiUserRoleId, int miSchoolId, int miAcademicYearId, string asAccessionNoBarcode, int aiInserttedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReturnDate", Convert.ToDateTime(aiReturnDate), SqlDbType.SmallDateTime);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InserttedById", aiInserttedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccesionNoOrBarcode", asAccessionNoBarcode, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_IssueBookToUser"))
                {
                    if (oSqlDataReader != null)
                        FillBookRenewUserDetails(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to pass the parameters to database for bulk issue, renew and retuen books.
        /// </summary>
        /// <param name="aiReturnDate"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="aiInserttedById"></param>
        /// <param name="sXMLBookIssueDetails"></param>
        /// <param name="aiTypeId"></param>
        public void IssueBooksToUserInBulk(string aiReturnDate, int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, int aiInserttedById, string asXMLBookIssueDetails, int aiTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReturnDate", Convert.ToDateTime(aiReturnDate), SqlDbType.SmallDateTime);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InserttedById", aiInserttedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BookIssueDetailsXML", asXMLBookIssueDetails, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_IssueBookToUserInBulk"))
                {
                    if(aiTypeId != Constants.I_THREE)
                    {
                        if (oSqlDataReader != null)
                            FillBookRenewUserDetails(oSqlDataReader);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to set book renew details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void FillBookRenewUserDetails(SqlDataReader aoSqlDataReader)
        {
            UserBookRenewDetails oUserBookRenewDetails = new UserBookRenewDetails();
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oUserBookRenewDetails.UserId = Convert.ToInt32(aoSqlDataReader["UserId"].ToString());
                    if (aoSqlDataReader["BookwiseRenewCount"].ToString() != null)
                        oUserBookRenewDetails.BookwiseRenewCount = aoSqlDataReader["BookwiseRenewCount"].ToString();

                    LstUserBookRenewDetails.Add(oUserBookRenewDetails);
                }
            }
        }

        /// <summary>
        /// This method is used to issue book to user. 
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="asAccessionNoBarcode"></param>
        public void RenewUserBook(int aiUserId, int aiUserRoleId, string asAccessionNoBarcode, int miSchoolId, int miAcademicYearId, int aiInserttedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InserttedById", aiInserttedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccesionNoOrBarcode", asAccessionNoBarcode, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_RenewBookToUser"))
                {
                    if (oSqlDataReader != null)
                        FillBookRenewUserDetails(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to return date to user. 
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="asAccessionNoBarcode"></param>
        public void GetBookReturnDate(int aiUserId, int aiUserRoleId, string asAccessionNoBarcode, int miSchoolId, int miAcademicYearId, int aiInserttedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InserttedById", aiInserttedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccesionNoOrBarcode", asAccessionNoBarcode, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBookReturnDate"))
                {
                    if (oSqlDataReader != null)
                        SetBookIssueReturnDates(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to return dateof book.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void SetBookIssueReturnDates(SqlDataReader aoSqlDataReader)
        {
            IssueReturnDateMaster oIssueReturnDateMaster;
           
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oIssueReturnDateMaster = new IssueReturnDateMaster()
                    {  
                        MailReserveBookUserMaster = new MailReserveBookUserMaster()
                        {
                            BookName = aoSqlDataReader["Book_Title"].ToString(),
                            BookReserveUserList = aoSqlDataReader["BookReserveUserList"].ToString(),
                        },
                        IssueDate = aoSqlDataReader["IssueDate"].ToString(),
                        ReturnDate = aoSqlDataReader["ReturnDate"].ToString(),
                    };
                    LstIssueReturnDateMaster.Add(oIssueReturnDateMaster);
                }
            }
        }

        /// <summary>
        /// This method is used to get all issued book from database.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asUserName"></param>
        /// <param name="asBookNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public DataTable GetAllIsseuBooks(Int32 aiSchoolId, Int32 aiAcademicYearId, String asUserName, String asBookNo, Int32 aiBookDetailsID, int aiStdDivId, String sortExpression, int iEndIndex, int iStartIndex, int aiDeactivatedUser)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asUserName, asBookNo), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_BookDetailsId", aiBookDetailsID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Flag", aiDeactivatedUser, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", String.Format(" ORDER BY {0}", sortExpression), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllIsseuBooks");
            }
        }

        /// <summary>
        /// This method is used to count rows from database which is used to set grid veiw page index.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asUserName"></param>
        /// <param name="asBookNo"></param>
        /// <returns></returns>
        public int CountRows(Int32 aiSchoolId, int aiAcademicYearId, String asUserName, String asBookNo, Int32 aiBookDetailsID, int aiStdDivId, int aiDeactivatedUser)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateFilter(asUserName, asBookNo), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_BookDetailsId", aiBookDetailsID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Flag", aiDeactivatedUser, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountIssueBooks");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to filter out data which is send by user. 
        /// </summary>
        /// <param name="asUserName"></param>
        /// <param name="asBookNo"></param>
        /// <returns></returns>
        private string CreateFilter(String asUserName, String asBookNo)
        {
            string sFilter = string.Empty;
            if (!String.IsNullOrEmpty(asUserName))
                sFilter = String.Format("{0} AND [Issued_Name] LIKE N'%{1}%'", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asUserName), false));
            if (!String.IsNullOrEmpty(asBookNo))
                sFilter = String.Format("{0} AND [Book_No]=N'{1}'", sFilter, StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asBookNo), false));

            return sFilter;
        }

        /// <summary>
        /// This method is used to get all issued book form database.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiBookId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public DataTable GetSelectedIssueBooks(int aiSchoolId, int aiBookId, string sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateIssueFilter(aiBookId), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", String.Format(" ORDER BY {0}", sortExpression), SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSelectedIssueBooks");
            }
        }

        /// <summary>
        /// This method is used to find count number of row to set grid view pagig.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiBookId"></param>
        /// <returns></returns>
        public int CountIssuedRows(int aiSchoolId, int aiBookId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateIssueFilter(aiBookId), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountSelectedIssueBooks");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to filter out data.
        /// </summary>
        /// <param name="aiBookId"></param>
        /// <returns></returns>
        private string CreateIssueFilter(Int32 aiBookId)
        {
            string sFilter = "";
            if (aiBookId != 0)
                sFilter = sFilter + " AND [Book_Id] =" + aiBookId + "";
            return sFilter;
        }

        /// <summary>
        /// This method is used to return book (update in database).
        /// </summary>
        /// <param name="iBookNo"></param>
        /// <param name="iReturnBy"></param>
        public void ReturnBook()
        {
            string sStatement = "";
            string sBookNo = "";
            if (moIssueReturnBookStruct.msBookNo.Substring(0, 1) == "B" && moIssueReturnBookStruct.msBookNo.Substring(moIssueReturnBookStruct.msBookNo.Length - Math.Min(3, moIssueReturnBookStruct.msBookNo.Length)) == "P" + moIssueReturnBookStruct.miSchoolId.ToString())
                sBookNo = StringUtility.ReplaceSingleQuoteInString(moIssueReturnBookStruct.msBookNo.Substring(1, moIssueReturnBookStruct.msBookNo.Length - 4), false);
            else
                sBookNo = StringUtility.ReplaceSingleQuoteInString(moIssueReturnBookStruct.msBookNo, false);
                sStatement = "UPDATE Library_Book_Issue_Details SET " +
                                                               " Book_Issue_Status=N'" + Constants.C_NO + "' " +
                                                               " ,Return_Renew_By_Id=" + moIssueReturnBookStruct.miUpdatedById +
                                                               " ,Return_Date=CAST (N'" + moIssueReturnBookStruct.mdtActualReturnDate + "'AS SMALLDATETIME)" +
                                                             " WHERE " +
                                                               " Book_No =N'" + sBookNo + "'";
            
            


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sStatement);
        }
        /// <summary>
        /// This method is used to return book (update in database).
        /// </summary>
        /// <param name="iBookNo"></param>
        /// <param name="iReturnBy"></param>
        public void ReturnUserBook()
        {
             string sBookNo = "";
             if (moIssueReturnBookStruct.msBookNo.Substring(0, 1) == "B" && moIssueReturnBookStruct.msBookNo.Substring(moIssueReturnBookStruct.msBookNo.Length - Math.Min(3, moIssueReturnBookStruct.msBookNo.Length)) == "P" + moIssueReturnBookStruct.miSchoolId.ToString())
                  sBookNo =moIssueReturnBookStruct.msBookNo.Substring(1, moIssueReturnBookStruct.msBookNo.Length - 4);
             else
                  sBookNo =moIssueReturnBookStruct.msBookNo;

             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 oSQLServerDbUtility.AddParameter("UserId", moIssueReturnBookStruct.miUser_Id, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("ReturnDate", Convert.ToDateTime(moIssueReturnBookStruct.mdtActualReturnDate), SqlDbType.SmallDateTime);
                 oSQLServerDbUtility.AddParameter("SchoolId", moIssueReturnBookStruct.miSchoolId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("AcademicYearId", moIssueReturnBookStruct.miAcademicYearId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("InsertedById", moIssueReturnBookStruct.miUpdatedById, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("BookNo", sBookNo, SqlDbType.NVarChar);
                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ReturnUserBook"))
                 {
                     if (oSqlDataReader != null)
                         FillBookRenewUserDetails(oSqlDataReader);
                 }
             }
        }
        /// <summary>
        /// This method is used to renew book, ie increse date in issue date.
        /// </summary>       
        public void RenewBook()
        {
            StringBuilder sStatement = new StringBuilder();

            sStatement.Append("UPDATE Library_Book_Issue_Details SET " +
                                                             " Return_Date=CAST (N'" + moIssueReturnBookStruct.mdtRenewDate + "'AS SMALLDATETIME)" +
                                                             " ,Return_Renew_By_Id=" + moIssueReturnBookStruct.miUser_Id +
                                                             " ,No_Of_Attempt_Renew=" + moIssueReturnBookStruct.miRenewAttempts +
                                                           " WHERE " +
                                                             " Book_No =N'" + StringUtility.ReplaceSingleQuoteInString(moIssueReturnBookStruct.msBookNo, false) + "'" +
                                                             " AND IsForParent=" + moIssueReturnBookStruct.miIsForParent +
                                                             " AND Book_Issue_Status=N'" + Constants.C_YES + "'");

            sStatement.Append(" INSERT INTO [IssuedBookHistory] " +
                                                             "([IssueBook_Id],[Renew_Date]) " +
                                                             " VALUES ("
                                             + moIssueReturnBookStruct.miBookIssueId +
                                             ",CAST (N'" + moIssueReturnBookStruct.mdtRenewDate + "'AS SMALLDATETIME))");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sStatement.ToString());
        }

        /// <summary>
        /// This method is used to ruturn number of book allowed as per user role.
        /// </summary>
        /// <returns></returns>
        public static int NumberOfBookAllowed(int iUserRoleId, int iSchoolId, int iAcademicYearId)
        {
            string sStatement = " SELECT " +
                              " ISNULL(No_Of_Book_Per_Person,0) " +
                              " FROM  " +
                              " Library_Configuration_Master  " +
                              " WHERE  " +
                              " School_Id = " + iSchoolId +
                               " AND User_Role_Id =" + iUserRoleId +
                              " AND  Academic_Year_Id =" + iAcademicYearId;

            //string sStatement = "SELECT dbo.[Udf_GetNumberOfBookAllowed]("
            //                                    + moIssueReturnBookStruct.miIssueID +
            //                                   "," + moIssueReturnBookStruct.miSchoolId +
            //                                   "," + moIssueReturnBookStruct.miAcademicYearId +
            //                                   "," + iUserRoleId +
            //                                   ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);
        }

        /// <summary>
        /// This method is used to return (Yes/No).
        /// Renewed attempt book for user particular books related to the library settings.
        /// </summary>
        /// <param name="iBookNo"></param>
        /// <param name="iSchoolID"></param>
        /// <param name="iAcacemicYearID"></param>
        /// <param name="iUserRoleId"></param>
        /// <returns></returns>
        public string NoOfRenewBookAttempt(string iBookNo, int iSchoolID, int iAcacemicYearID, int iUserRoleId)
        {
            string sStatement = "SELECT dbo.[Udf_GetNoOfAttemptBookRenew](" +
                                              "N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(iBookNo), false) + "'" +
                                              "," + iSchoolID +
                                              "," + iAcacemicYearID +
                                              "," + iUserRoleId +
                                              ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sStatement);
        }

        /// <summary>
        /// This method is used to get issue period for particular user.
        /// </summary>
        /// <param name="iUserRoleId"></param>
        /// <returns></returns>
        public int GetIssuePeried(int aiUserRoleId)
        {
            string sSelectStatement = " SELECT " +
                                         " Return_Days " +
                                      " FROM " +
                                        " Library_Configuration_Master" +
                                      " WHERE " +
                                        " User_Role_Id=" + aiUserRoleId +
                                        " AND School_Id=" + moIssueReturnBookStruct.miSchoolId +
                                        " AND Academic_Year_Id=" + moIssueReturnBookStruct.miAcademicYearId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        public DataTable GetAllIssuedBookHistory(int aiSchoolId, int aiAcademicYearId, string asBookName, string asUserName, string asCategoryName, string asStartDate, string asEndDate, string asAccessionNumber, string sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_AcademicYear_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateBookHistoryFilter(asBookName, asAccessionNumber, asUserName, asCategoryName, asStartDate, asEndDate), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", String.Format(" ORDER BY {0}", sortExpression), SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetBookIssuedHistory");
            }
        }

        private string CreateBookHistoryFilter(string asBookName, string asAccessionNumber, string asUserName, string asCategoryName,
            string asStartDate, string asEndDate)
        {
            string sFilter = string.Empty;

            if (!String.IsNullOrEmpty(asBookName))
                sFilter = sFilter + " AND [Book_Title] LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asBookName), false) + "%'";

            if (!String.IsNullOrEmpty(asAccessionNumber))
                sFilter = sFilter + " AND [Book_No] LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asAccessionNumber), false) + "%'";

            if (!String.IsNullOrEmpty(asUserName))
                sFilter = sFilter + " AND [User_Name] LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asUserName), false) + "%'";

            if (!String.IsNullOrEmpty(asCategoryName))
                sFilter = sFilter + " AND [Category_Name] LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asCategoryName), false) + "%'";

            if (!String.IsNullOrEmpty(asStartDate))
                sFilter = sFilter + " AND [Issue_Date] > N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asStartDate), false) + "'";

            if (!String.IsNullOrEmpty(asEndDate))
                sFilter = sFilter + " AND [Issue_Date] < N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(asEndDate), false) + "'";

            return sFilter;
        }

        /// <summary>
        /// This method is used to get total row count and selected row.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asBookName"></param>
        /// <param name="asUserName"></param>
        /// <param name="asCategoryName"></param>
        /// <param name="asStartDate"></param>
        /// <param name="asEndDate"></param>
        /// <param name="asAccessionNumber"></param>
        /// <returns></returns>
        public int CountIssuedBookHistoryRows(int aiSchoolId, int aiAcademicYearId, string asBookName, string asUserName, string asCategoryName, string asStartDate, string asEndDate, string asAccessionNumber)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", CreateBookHistoryFilter(asBookName, asAccessionNumber, asUserName, asCategoryName, asStartDate, asEndDate), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountIssuedBookHistory");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public DataTable GetConfiguredUsers()
        {
            string sSelectStatement = " SELECT " +
                                         " User_Role_Id " +
                                      " FROM " +
                                        " Library_Configuration_Master" +
                                      " WHERE " +
                                        " School_Id=" + moIssueReturnBookStruct.miSchoolId +
                                        " AND Academic_Year_Id=" + moIssueReturnBookStruct.miAcademicYearId +
                                      " ORDER BY" +
                                        " User_Role_Id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetBookDetails(int aischoolId, int aiBookId)
        {
            string sSelect = " SELECT " +
                             " Book_Id " +
                             " ,Book_No " +
                             " ,RackShelfNo " +
                             " ,Book_Detail_Id" +
                             " ,IsBookLost" +
                            " FROM " +
                             " vw_GetLibraryBookDetails " +
                             " WHERE " +
                             " School_Id= " + aischoolId +
                             " AND Book_Id=" + aiBookId +
                             " AND Book_Issue_Status= N'N'" +
                             " AND Is_Deleted=N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelect);
        }

        public DataTable GetBookDetailsByBarcode(int aischoolId, string asBookId)
        {
            string sSelect = string.Empty;
            if (aischoolId == Constants.SchoolId.SNS.ToInt())
            {
                sSelect = " SELECT " +
                          " Book_Id " +
                          " ,Book_No " +
                          " ,RackShelfNo " +
                          " ,Book_Detail_Id" +
                          " FROM " +
                          " vw_GetLibraryBookDetails " +
                          " WHERE " +
                          " School_Id= " + aischoolId +
                          " AND Book_No = " + "'" + asBookId + "'" +
                          " AND Is_Deleted=N'" + Constants.C_NO + "'";
            }
            else
            {
                sSelect = " SELECT " +
                          " Book_Id " +
                          " ,Book_No " +
                          " ,RackShelfNo " +
                          " ,Book_Detail_Id" +
                          " FROM " +
                          " vw_GetLibraryBookDetails " +
                          " WHERE " +
                          " School_Id= " + aischoolId +
                          " AND Book_Detail_Id=" + asBookId +
                          " AND Is_Deleted=N'" + Constants.C_NO + "'";
            }


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelect);
        }

        public DataTable GetIssuedBookDetailsofUser(int aischoolId, int aiacademicyaerID, int aiUserId)
        {
            string sSelect = " SELECT " +
                             " Library_Book_Issue_Details.Book_Issued_To," +
                             " Library_Book_Issue_Details.Return_Date," +
                             " Library_Book_Issue_Details.Book_Detail_Id," +
                             " Library_Book_Issue_Details.Book_No," +
                             " Library_Book_Issue_Details.Book_Id," +
                             " SchoolWise_Book_Master.Book_Title," +
                             " Library_Book_Issue_Details.Issue_Date," +
                             " IsForParent " +
                             " FROM " +
                             " Library_Book_Issue_Details INNER JOIN " +
                             " SchoolWise_Book_Master ON Library_Book_Issue_Details.Book_Id = SchoolWise_Book_Master.Book_Id INNER JOIN " +
                             " Book_Details ON Library_Book_Issue_Details.Book_Detail_Id = Book_Details.Book_Detail_Id " +
                             " WHERE " +
                              " Library_Book_Issue_Details.School_Id= " + aischoolId +
                              " AND Book_Issued_To= " + aiUserId +
                              " AND Academic_Year_Id=" + aiacademicyaerID +
                              " AND Library_Book_Issue_Details.Is_Deleted=N'N'" +
                              " AND Library_Book_Issue_Details.Book_Issue_Status=N'Y'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelect);
        }
        /// <summary>
        /// This method is user to get user details.
        /// </summary>
        /// <param name="sUserRole"></param>
        /// <param name="asEnrollOrEmpNo"></param>
        /// <param name="iSchoolId"></param>
        /// <param name="iAcademicYearId"></param>
        /// <returns></returns>
        public static List<LibaryUsers> GetUser(string sUserRole, string asEnrollOrEmpNo, int iSchoolId, int iAcademicYearId)
        {
            List<LibaryUsers> lstLibaryUsers = new List<LibaryUsers>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRole", sUserRole, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EnrollOrEmpNo", asEnrollOrEmpNo, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserForBarcode"))
                {
                    LibaryUsers oLibaryUsers;
                    while (oSqlDataReader.Read())
                    {
                        oLibaryUsers = new LibaryUsers
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            Book_Id = Convert.ToInt32(oSqlDataReader["Book_Id"]),
                            RegNo = Convert.ToString(oSqlDataReader["RegNo"]),
                            UserName = Convert.ToString(oSqlDataReader["FullName"]),
                            ClassNameDesignation = Convert.ToString(oSqlDataReader["ClassNameDesignation"]),
                            EmployeeNo = oSqlDataReader["EmployeeNo"].ToString(),
                            RollNo = oSqlDataReader["RollNo"] != DBNull.Value ? Convert.ToInt32(oSqlDataReader["RollNo"]) : 0,
                            IsActive = oSqlDataReader["IsActive"].ToString()
                        };
                        lstLibaryUsers.Add(oLibaryUsers);
                    }
                    return lstLibaryUsers;
                }
            }
        }

        public static List<SchoolBookDetails> GetBook(int iBookId, int iSchoolId, int iAcademicYearId)
        {
            List<SchoolBookDetails> lstSchoolBookDetails = new List<SchoolBookDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BookId", iBookId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", iAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBookByBarcode"))
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
                            Book_Price = Convert.ToInt32(oSqlDataReader["Book_Price"]),
                            IsForIssue = Convert.ToInt32(oSqlDataReader["IsForIssue"])
                        };
                        lstSchoolBookDetails.Add(oSchoolBookDetails);
                    }
                }
                return lstSchoolBookDetails;
            }
        }

        public static int NumberOfBookIssued(short iUserRoleId, short iUserId, int iSchoolId, int iAcademicYearId, bool abForParent)
        {

            string sStatement = " SELECT " +
                              " count(*) " +
                              " FROM  " +
                              " vw_BookIssueDetails " +
                              " WHERE  " +
                              " School_Id = " + iSchoolId +
                              " AND  Academic_Year_Id = " + iAcademicYearId +
                              " AND Book_Issued_To =" + iUserId +
                              " AND Book_Issue_Status = N'Y' " +
                              " AND User_Role_Id = " + iUserRoleId;

            if (abForParent == true)
                sStatement += " AND IsForParent=1";
            else
                sStatement += " AND IsForParent=0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);
        }
        public void SaveLateFee()
        {
            string sInsertStmt = "INSERT INTO [dbo].[Library_Late_Fee] " +
                                 "([BookNo],[UserId],[Amount],[SchoolId],[AcademicYearId],[InsertedBy])" +
                                 " VALUES (N'" + moIssueReturnBookStruct.msBookNo + "'" +
                                 "," + moIssueReturnBookStruct.miBookIssuedTo + "," +
                                 "" + moIssueReturnBookStruct.miLateFee + "," +
                                 " " + moIssueReturnBookStruct.miSchoolId + "" +
                                 "," + moIssueReturnBookStruct.miAcademicYearId +
                                 "," + moIssueReturnBookStruct.miInsertedById +
                                 ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sInsertStmt);
            };
        }

        public static void GetUsersForRervedBook(int aiBookId, int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStmt = " SELECT UserId " +
                                    " FROM library_Book_Reservation " +
                                    " WHERE BookId=" + aiBookId +
                                            " AND IsIssued=0 " +
                                            " AND IsCanceled=0 " +
                                            " AND IsDeleted=0 " +
                                            " AND AcademicYearId=" + aiAcademicYearId +
                                            " AND SchoolId=" + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt);
            };
        }

        /// <summary>
        /// This method is used to get all issued book or book issued user details for report.
        /// </summary>
        /// <param name="adictIssueBookFilters"></param>
        /// <param name="aiOptValue"></param>
        /// <returns></returns>
        public static DataTable GetAllIssueBookDetails(Dictionary<string, string> adictIssueBookFilters, int aiOptValue)
        {
            //string sFilterParameter = string.Empty;
            //foreach(KeyValuePair<string,string> sValue in adictIssueBookFilters)
            //    sFilterParameter=sFilterParameter != string.Empty ? sValue.Value: "," + sValue.Value;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                foreach (KeyValuePair<string, string> sDictValue in adictIssueBookFilters)
                    oSQLServerDbUtility.AddParameter(sDictValue.Key, sDictValue.Value, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SearchFilter", aiOptValue, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetIssuedBookOrBookIssuedUserDetails");
            }
        }

        public static int GetIssuedCntForParent(int aiUserId, int aiBookId)
        {
            int iCnt = 0;
            string sSelectStmt = " SELECT No_Of_Attempt_Renew " +
                                 " FROM Library_Book_Issue_Details " +
                                 " WHERE Book_Issue_Status=N'Y' " +
                                 " AND IsForParent=1 " +
                                 " and Book_Id=" + aiBookId +
                                 " AND Book_Issued_To=" + aiUserId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                            if (oSqlDataReader["No_Of_Attempt_Renew"] != DBNull.Value)
                                iCnt = Convert.ToInt32(oSqlDataReader["No_Of_Attempt_Renew"]);
                    }
                }
                return iCnt;
            }
        }
    }
}
