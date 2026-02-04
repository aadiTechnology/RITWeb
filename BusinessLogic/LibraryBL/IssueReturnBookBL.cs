using System;
using System.Data;
using System.Collections.Generic;
using DataCommunicator;
using BookEntities;




namespace BusinessLogic
{
   public class IssueReturnBookBL
    {
       #region Data members

       private IssueReturnBookDC.IssueReturnBookStruct moIssueReturnBookStruct;
       private IssueReturnBookDC moIssueReturnBookDC = new IssueReturnBookDC();
       
       #endregion
       #region Properties
       public List<IssueReturnDateMaster> LstIssueReturnDateMaster
       {
           get
           {
               return moIssueReturnBookDC.LstIssueReturnDateMaster;
           }
           set
           {
               moIssueReturnBookDC.LstIssueReturnDateMaster = value;
           }
       }
       public List<UserBookRenewDetails> LstUserBookRenewDetails
       {
           get
           {
               return moIssueReturnBookDC.LstUserBookRenewDetails;
           }
           set
           {
               moIssueReturnBookDC.LstUserBookRenewDetails = value;
           }
       }
       #endregion
       /// <summary>
       /// This is constructor which is used to initialized IssueReturnBookDC class member.
       /// </summary>
       public IssueReturnBookBL()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
       }
      
       /// <summary>
       /// This method is used to Issue Books
       /// </summary>
       public void IssueBook()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           moIssueReturnBookDC.IssueBook();
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
       public void IssueBookToUser(int aiUserId, string aiReturnDate, int aiUserRoleId, string asAccessionNoBarcode, int miSchoolId, int miAcademicYearId, int aiInserttedById)
       {
           moIssueReturnBookDC.IssueBookToUser(aiUserId, aiReturnDate, aiUserRoleId, miSchoolId, miAcademicYearId, asAccessionNoBarcode, aiInserttedById);
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
           moIssueReturnBookDC.IssueBooksToUserInBulk(aiReturnDate, aiUserRoleId, aiSchoolId, aiAcademicYearId, aiInserttedById, asXMLBookIssueDetails, aiTypeId);
       }

       /// <summary>
       /// This method is used to return date to user. 
       /// </summary>
       /// <param name="aiUserId"></param>
       /// <param name="aiUserRoleId"></param>
       /// <param name="miSchoolId"></param>
       /// <param name="miAcademicYearId"></param>
       /// <param name="asAccessionNoBarcode"></param>
       public void RenewUserBook(int aiUserId, int aiUserRoleId, string asAccessionNoBarcode, int miSchoolId, int miAcademicYearId, int aiInserttedById)
       {
           moIssueReturnBookDC.RenewUserBook(aiUserId, aiUserRoleId, asAccessionNoBarcode, miSchoolId, miAcademicYearId, aiInserttedById);
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
           moIssueReturnBookDC.GetBookReturnDate(aiUserId, aiUserRoleId, asAccessionNoBarcode, miSchoolId, miAcademicYearId, aiInserttedById);
       }

       /// <summary>
       /// This method is used to Renew Books
       /// </summary>
       /// <param name="iBookNo"></param>
       /// <param name="iReturnBy"></param>
       /// <param name="dtRenewDate"></param>
       /// <param name="iRenewBookAttempt"></param>
       public void RenewBook()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           moIssueReturnBookDC.RenewBook();
       }

       /// <summary>
       /// 
       /// </summary>
       public void ReturnUserBook()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           moIssueReturnBookDC.ReturnUserBook();
       }

       /// <summary>
       /// This method is used to Return Books
       /// </summary>
       /// <param name="iBookNo"></param>
       /// <param name="iReturnBy"></param>
       public void ReturnBook()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           moIssueReturnBookDC.ReturnBook();
       }

       /// <summary>
       /// This method is used in ReturnRenewUI.aspx, to Get All Issue Book detail table.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="asUserId"></param>
       /// <param name="asBookNo"></param>
       /// <param name="sortExpression"></param>
       /// <param name="maximumRows"></param>
       /// <param name="startRowIndex"></param>
       /// <returns></returns>
       public DataTable GetAllIssueBooks(Int32 aiSchoolId, int aiAcademicYearId, String asUserName, String asBookNo, Int32 aiBookDetailsID, int aiStdDivId, String sortExpression, int maximumRows, int startRowIndex, String aiReturnBookID, int aiDeactivatedUser)
       {
           if (aiReturnBookID != string.Empty)
               asBookNo = aiReturnBookID;
           if (String.IsNullOrEmpty(sortExpression))
           {
               sortExpression = "Book_Title";
           }
           int iStartIndex = startRowIndex;
           int iEndIndex = iStartIndex + maximumRows;
           return moIssueReturnBookDC.GetAllIsseuBooks(aiSchoolId, aiAcademicYearId, asUserName, asBookNo, aiBookDetailsID,aiStdDivId, sortExpression, iEndIndex, iStartIndex,aiDeactivatedUser);
       }

       public DataTable GetAllIssuedBookHistory(Int32 aiSchoolId, int aiAcademicYearId, String asBookName, String asUserName,String asCategoryName ,String asStartDate ,String asEndDate ,String asAccessionNumber, String sortExpression, int maximumRows, int startRowIndex)
       {//aiSchoolId aiAcademicYearId asBookName asUserName asCategoryName asStartDate asEndDate asAccessionNumber
           if (String.IsNullOrEmpty(sortExpression))
           {
               sortExpression = "Book_Title";
           }
           int iStartIndex = startRowIndex;
           int iEndIndex = iStartIndex + maximumRows;
           return moIssueReturnBookDC.GetAllIssuedBookHistory(aiSchoolId, aiAcademicYearId, asBookName, asUserName, asCategoryName, asStartDate, asEndDate, asAccessionNumber, sortExpression, iEndIndex, iStartIndex);
       }

       /// <summary>
       /// This method is used to get count for Issued Book history details.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="asUserId"></param>
       /// <param name="asBookNo"></param>
       /// <returns></returns>
       public int CountIssuedBookHistoryRows(Int32 aiSchoolId, int aiAcademicYearId, String asBookName, String asUserName, String asCategoryName, String asStartDate, String asEndDate, String asAccessionNumber)
       {
           return moIssueReturnBookDC.CountIssuedBookHistoryRows(aiSchoolId, aiAcademicYearId, asBookName, asUserName, asCategoryName, asStartDate, asEndDate, asAccessionNumber);
       }
      
       /// <summary>
       /// This method is used to get count for Issue Book detail.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="asUserId"></param>
       /// <param name="asBookNo"></param>
       /// <returns></returns>
       public int CountRows(Int32 aiSchoolId, int aiAcademicYearId, String asUserName, String asBookNo, Int32 aiBookDetailsID, int aiStdDivId, String aiReturnBookID, int aiDeactivatedUser)
       {
           if (aiReturnBookID != string.Empty)
               asBookNo = aiReturnBookID;
           return moIssueReturnBookDC.CountRows(aiSchoolId, aiAcademicYearId, asUserName, asBookNo, aiBookDetailsID, aiStdDivId,aiDeactivatedUser);
       }

       /// <summary>
       /// This method is used in IssueBookUI.aspx page which is used to get all Books which is not Issued.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiBookId"></param>
       /// <param name="sortExpression"></param>
       /// <param name="maximumRows"></param>
       /// <param name="startRowIndex"></param>
       /// <returns></returns>
       public DataTable GetSelectedIssueBooks(Int32 aiSchoolId, Int32 aiBookId, String sortExpression, int maximumRows, int startRowIndex)
       {
           if (String.IsNullOrEmpty(sortExpression))
           {
               sortExpression = "Book_Title";
           }
           int iStartIndex = startRowIndex;
           int iEndIndex = iStartIndex + maximumRows;
           return moIssueReturnBookDC.GetSelectedIssueBooks(aiSchoolId, aiBookId, sortExpression, iEndIndex, iStartIndex);
       }

       /// <summary>
       /// This method is used to get count for (IssueBookUI.aspx) Book detail.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiBookId"></param>
       /// <returns></returns>
       public int CountIssuedRows(Int32 aiSchoolId, Int32 aiBookId)
       {
           return moIssueReturnBookDC.CountIssuedRows(aiSchoolId, aiBookId);
       }

       #region Property & Data Member

       public Int32 SchoolId
       {
           get { return moIssueReturnBookStruct.miSchoolId; }
           set { moIssueReturnBookStruct.miSchoolId = value; }
       }

       public Int32 AcademicYearId
       {
           get { return moIssueReturnBookStruct.miAcademicYearId; }
           set { moIssueReturnBookStruct.miAcademicYearId = value; }
       }

       public Int32 BookDetailsId
       {
           get { return moIssueReturnBookStruct.miBookDetailsId; }
           set { moIssueReturnBookStruct.miBookDetailsId = value; }
       }
       public Int32 BookIssueId
       {
           get { return moIssueReturnBookStruct.miBookIssueId; }
           set { moIssueReturnBookStruct.miBookIssueId = value; }
       }
       
       public string BookName
       {
           get { return moIssueReturnBookStruct.msBookName; }
           set { moIssueReturnBookStruct.msBookName = value; }
       }

       public Int32 BookId
       {
           get { return moIssueReturnBookStruct.miBookId; }
           set { moIssueReturnBookStruct.miBookId = value; }
       }

       public string BookNo
       {
           get { return moIssueReturnBookStruct.msBookNo; }
           set { moIssueReturnBookStruct.msBookNo = value; }
       }

       public string AuthorName
       {
           get { return moIssueReturnBookStruct.msAuthorName; }
           set { moIssueReturnBookStruct.msAuthorName = value; }
       }

       public string CategoryName
       {
           get { return moIssueReturnBookStruct.msCategoryName; }
           set { moIssueReturnBookStruct.msCategoryName = value; }
       }

       public Int32 CategoryId
       {
           get { return moIssueReturnBookStruct.miCategoryId; }
           set { moIssueReturnBookStruct.miCategoryId = value; }
       }

       public string PublishedBy
       {
           get { return moIssueReturnBookStruct.msPublishedBy; }
           set { moIssueReturnBookStruct.msPublishedBy = value; }
       }

       public Int32 Price
       {
           get { return moIssueReturnBookStruct.miPrice; }
           set { moIssueReturnBookStruct.miPrice = value; }
       }
              
       public Int32 IssueId
       {
           get { return moIssueReturnBookStruct.miIssueID; }
           set { moIssueReturnBookStruct.miIssueID = value; }
       }
       
       public System.DateTime IssueDate
       {
           get { return moIssueReturnBookStruct.mdtIssueDate; }
           set { moIssueReturnBookStruct.mdtIssueDate = value; }
       }
       public string ReturnDate
       {
           get { return moIssueReturnBookStruct.mdtReturnDate; }
           set { moIssueReturnBookStruct.mdtReturnDate = value; }
       }
       public string RenewDate
       {
           get { return moIssueReturnBookStruct.mdtRenewDate ; }
           set { moIssueReturnBookStruct.mdtRenewDate = value; }
       }
       public Int32 UserId
       {
           get { return moIssueReturnBookStruct.miUser_Id; }
           set { moIssueReturnBookStruct.miUser_Id = value; }
       }

       public Int32 RenewAttempts
       {
           get { return moIssueReturnBookStruct.miRenewAttempts; }
           set { moIssueReturnBookStruct.miRenewAttempts = value; }
       }
       public char IsDeleted
       {
           get { return moIssueReturnBookStruct.msIsDeleted; }
           set { moIssueReturnBookStruct.msIsDeleted = value; }
       }

       public Int32 InsertedById
       {
           get { return moIssueReturnBookStruct.miInsertedById; }
           set { moIssueReturnBookStruct.miInsertedById = value; }
       }

       public System.DateTime InsertedDate
       {
           get { return moIssueReturnBookStruct.mdtInsertedDate; }
           set { moIssueReturnBookStruct.mdtInsertedDate = value; }
       }


       public Int32 UpdatedById
       {
           get { return moIssueReturnBookStruct.miUpdatedById; }
           set { moIssueReturnBookStruct.miUpdatedById = value; }
       }

       public System.DateTime UpdatedDate
       {
           get { return moIssueReturnBookStruct.mdtUpdatedDate; }
           set { moIssueReturnBookStruct.mdtUpdatedDate = value; }
       }

       public string ActualReturnDate
       {
           get { return moIssueReturnBookStruct.mdtActualReturnDate; }
           set { moIssueReturnBookStruct.mdtActualReturnDate = value; }
       }
       public Int32 LateFee
       {
           get { return moIssueReturnBookStruct.miLateFee; }
           set { moIssueReturnBookStruct.miLateFee = value; }
       }

       public Int32 BookIssuedTo
       {
           get { return moIssueReturnBookStruct.miBookIssuedTo; }
           set { moIssueReturnBookStruct.miBookIssuedTo = value; }
       }

       public Int32 IsForParent
       {
           get { return moIssueReturnBookStruct.miIsForParent; }
           set { moIssueReturnBookStruct.miIsForParent = value; }
       }
       #endregion


       /// <summary>
       /// This method is used to get number of book Allowed as per user role.
       /// </summary>
       /// <returns></returns>
       public static int NumberOfBookAllowed(int iUserRoleId,int iSchoolId,int iAcademicYearId)
       {

           return IssueReturnBookDC.NumberOfBookAllowed(iUserRoleId, iSchoolId, iAcademicYearId);
       }

       /// <summary>
       /// This method is used to get number of attempt book issued.
       /// </summary>
       /// <param name="iBookNo"></param>
       /// <param name="iSchoolID"></param>
       /// <param name="iAcacemicYearID"></param>
       /// <returns></returns>
       public string NoOfRenewBookAttempt(string iBookNo, int iSchoolID, int iAcacemicYearID, int iUserRoleId)
       {
           return moIssueReturnBookDC.NoOfRenewBookAttempt(iBookNo, iSchoolID, iAcacemicYearID, iUserRoleId);
       }

       /// <summary>
       /// This method is used to ger issue period in days.
       /// </summary>
       /// <param name="iUserRoleId"></param>
       /// <returns></returns>
       public int GetIssuePeried(int aiUserRoleId)
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           return moIssueReturnBookDC.GetIssuePeried(aiUserRoleId);
       }


       public DataTable GetConfiguredUsers()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           return moIssueReturnBookDC.GetConfiguredUsers();
       }

       public DataTable GetBookDetails(int aischoolId, int aiBookId)
       {
           return moIssueReturnBookDC.GetBookDetails(aischoolId, aiBookId);
       }

       public DataTable GetBookDetailsByBarcode(int aischoolId, string asBookId)
       {
           return moIssueReturnBookDC.GetBookDetailsByBarcode(aischoolId, asBookId);
       }

       public DataTable GetIssuedBookDetailsofUser(int aischoolId, int aiacademicyaerID, int aiUserId)
       {
           return moIssueReturnBookDC.GetIssuedBookDetailsofUser(aischoolId,aiacademicyaerID, aiUserId);
       }

       public static List<LibaryUsers> GetUser(string sUserRole, string asEnrollOrEmpNo, int iSchoolId, int iAcademicYearId)
       {
           return IssueReturnBookDC.GetUser(sUserRole, asEnrollOrEmpNo, iSchoolId, iAcademicYearId);
       }

       public static List<SchoolBookDetails> GetBook(int iBookId, int iSchoolId, int iAcademicYearId)
       {
           return IssueReturnBookDC.GetBook(iBookId, iSchoolId, iAcademicYearId);
       }

       public static int NumberOfBookIssued(short iUserRoleId, short iUserId, int iSchoolId, int iAcademicYearId,bool abForParent)
       {
           return IssueReturnBookDC.NumberOfBookIssued(iUserRoleId, iUserId, iSchoolId, iAcademicYearId,abForParent);
       }
       public void SaveLateFee()
       {
           moIssueReturnBookDC.IssueReturnBookInfo = moIssueReturnBookStruct;
           moIssueReturnBookDC.SaveLateFee();
       }

       public static void GetUsersForRervedBook(int aiBookId, int aiSchoolId, int aiAcademicYearId)
       {
           IssueReturnBookDC.GetUsersForRervedBook(aiBookId, aiSchoolId, aiAcademicYearId);
       }
       /// <summary>
       /// This method is used to get all issued book details for report.
       /// </summary>
       /// <param name="adictIssueBookFilters"></param>
       /// <param name="aiOptValue"></param>
       /// <returns></returns>
       public static DataTable GetAllIssueBookDetails(Dictionary<string, string> adictIssueBookFilters, int aiOptValue)
       {
           return IssueReturnBookDC.GetAllIssueBookDetails(adictIssueBookFilters, aiOptValue);
       }
       public static int GetIssuedCntForParent(int aiUserId, int aiBookId)
       {
           return IssueReturnBookDC.GetIssuedCntForParent(aiUserId, aiBookId);
       }
    }
}
