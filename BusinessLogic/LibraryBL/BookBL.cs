using System;
using System.Data;
using System.Collections;
using DataCommunicator;
using System.Collections.Generic;
using BookEntities;
using StudentEntities;

namespace BusinessLogic
{
    public class BookBL
    {
        #region Data members

        private BookDC.BookStructDetails moBookStructDetails;
        private BookDC moBookDC = new BookDC();

        public List<IssueBookUserMaster> LstIssueBookUserMaster
        {
            get
            {
                return moBookDC.LstIssueBookUserMaster;
            }
            set
            {
                moBookDC.LstIssueBookUserMaster = value;
            }
        }
        public List<AllBookDetails> LstAllBookDetails
        {
            get
            {
                return moBookDC.LstAllBookDetails;
            }
            set
            {
                moBookDC.LstAllBookDetails = value;
            }
        }
        public List<BookIssueRenewCountMaster> LstBookIssueRenewCountMaster
        {
            get
            {
                return moBookDC.LstBookIssueRenewCountMaster;
            }
            set
            {
                moBookDC.LstBookIssueRenewCountMaster = value;
            }
        }
        public List<UserRoles> LstUserRoles
        {
            get
            {
                return moBookDC.LstUserRoles;
            }
            set
            {
                moBookDC.LstUserRoles = value;
            }
        }
        public List<ClassDetails> LstClassDetails
        {
            get
            {
                return moBookDC.LstClassDetails;
            }
            set
            {
                moBookDC.LstClassDetails = value;
            }
        }
        public List<StudentInfo> LstStudentInfo
        {
            get
            {
                return moBookDC.LstStudentInfo;
            }
            set
            {
                moBookDC.LstStudentInfo = value;
            }
        }

        public int ReserveBookCount
        {

            get  {return moBookDC.ReserveBookCount ;}
            set {moBookDC.ReserveBookCount=value ;}
        }
       
        #endregion

        #region " Constant "

        private const string S_DUPLICATE_BOOK_NAME = "Book title already exists.";

        #endregion

        public BookBL()
        {
            moBookDC.BookInfo = moBookStructDetails;
        }

        public BookBL(int sBookId, int iSchoolId)
        {
            moBookDC = new BookDC(sBookId, iSchoolId);
            moBookStructDetails = moBookDC.BookInfo;
        }

        public DataTable GetImportBookList(Int32 aiSchoolId, string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, String asAuthorName, String asPublisher, String asDescription, String asAccessionNumber, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Book_Title";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moBookDC.GetImportBookList(aiSchoolId, asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDescription, asAccessionNumber, sortExpression, iEndIndex, iStartIndex);
        }

        public int CountImportRows(Int32 aiSchoolId, string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, String asAuthorName, String asPublisher, String asDescription, String asAccessionNumber)
        {
            return moBookDC.CountImportRows(aiSchoolId, asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDescription, asAccessionNumber);
        }

        ///// <summary>
        ///// This method is used to get user details to fill the BooksListView.
        ///// </summary>
        //public static List<LibaryUsers> GetAllUsers(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivisionId)
        //{

        //    return BookDC.GetAllUsers(aiSchoolId, aiAcademicYearId, asFilter, aiUserRoleId, aiStandardDivisionId);
        //}

        //Retrive Book details on BookUI.aspx page.
        public DataTable RetriveAllBooks(Int32 aiSchoolId, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Book_Title";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moBookDC.RetriveAllBooks(aiSchoolId, sortExpression, iEndIndex, iStartIndex);
        }
        public int CountRetriveRows(Int32 aiSchoolId)
        {
            return moBookDC.CountRetriveRows(aiSchoolId);
        }


        //Search the Remove Books which is not issed ie IssueStatus is "N"                                                   .
        public DataTable GetAllBooksForRemove(Int32 aiSchoolId, Int32 aiBookId, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Book_Title";
            }
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moBookDC.GetAllBooksForRemove(aiSchoolId, aiBookId, sortExpression, iEndIndex, iStartIndex);
        }

        public int CountRemoveBookRows(Int32 aiSchoolId, Int32 aiBookId)
        {
            return moBookDC.CountRemoveBookRows(aiSchoolId, aiBookId);
        }

        public DataTable GetMainCategoryDetails()
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.GetMainCategoryDetails();
        }
        public static List<string> GetLanguages(int aiSchoolId)
        {
            return BookDC.GetLanguages(aiSchoolId);
        }

        public bool IsDuplicateBookTitle()
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.IsDuplicateBook();
        }

        public void IsDuplicateBook()
        {
            moBookDC.BookInfo = moBookStructDetails;
            bool bIsDuplicate = moBookDC.IsDuplicateBook();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_BOOK_NAME);
        }

        /// <summary>
        /// This method is used to store the book details, book copy details and standardwise book details in database.
        /// </summary>
        public void AddBook(List<SchoolBookDetails> lstAccessionDetails)
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.AddBook(lstAccessionDetails);
        }

        /// <summary>
        /// This method is used to save the book copy details.
        /// </summary>
        public void AddAccessionDetails(List<SchoolBookDetails> lstAccessionDetails, int iBookId)
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.AddAccessionDetails(lstAccessionDetails, iBookId);
        }

        /// <summary>
        /// This method is used to update Existing Book details, book copy details and standardwise book details.
        /// </summary>
        /// <param name="iBookId"></param>
        /// <param name="sArrUpdate"></param>
        public void UpdateBook(int aiBookId, ArrayList aoArrayList)
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.UpdateBook(aiBookId, aoArrayList);
        }

        #region Property & Data Member

        public Int32 AcademicYearId
        {
            get { return moBookStructDetails.miAcademicYearId; }
            set { moBookStructDetails.miAcademicYearId = value; }
        }
        public Int32 SchoolId
        {
            get { return moBookStructDetails.miSchoolId; }
            set { moBookStructDetails.miSchoolId = value; }
        }
        public string BookName
        {
            get { return moBookStructDetails.msBookName; }
            set { moBookStructDetails.msBookName = value; }
        }
        public Int32 BookId
        {
            get { return moBookStructDetails.miBookId; }
            set { moBookStructDetails.miBookId = value; }
        }
        public Int32 BookSrNo
        {
            get { return moBookStructDetails.miBookSrNo; }
            set { moBookStructDetails.miBookSrNo = value; }
        }
        public string BookNumber
        {
            get { return moBookStructDetails.msBookNumber; }
            set { moBookStructDetails.msBookNumber = value; }
        }
        public string AuthorName
        {
            get { return moBookStructDetails.msAuthorName; }
            set { moBookStructDetails.msAuthorName = value; }
        }
        public Int16 MediaType
        {
            get { return moBookStructDetails.miMediaType; }
            set { moBookStructDetails.miMediaType = value; }
        }
       
        public string MainCategoryName
        {
            get { return moBookStructDetails.msMainCategoryName; }
            set { moBookStructDetails.msMainCategoryName = value; }
        }
        public Int32 MainCategoryId
        {
            get { return moBookStructDetails.miMainCategoryId; }
            set { moBookStructDetails.miMainCategoryId = value; }
        }
        public string PublishedBy
        {
            get { return moBookStructDetails.msPublishedBy; }
            set { moBookStructDetails.msPublishedBy = value; }
        }
        public Int32 UserId
        {
            get { return moBookStructDetails.miUser_Id; }
            set { moBookStructDetails.miUser_Id = value; }
        }
        public char IsDeleted
        {
            get { return moBookStructDetails.msIsDeleted; }
            set { moBookStructDetails.msIsDeleted = value; }
        }
        public Int32 InsertedById
        {
            get { return moBookStructDetails.miInsertedById; }
            set { moBookStructDetails.miInsertedById = value; }
        }
        public System.DateTime InsertedDate
        {
            get { return moBookStructDetails.mdtInsertedDate; }
            set { moBookStructDetails.mdtInsertedDate = value; }
        }
        public Int32 UpdatedById
        {
            get { return moBookStructDetails.miUpdatedById; }
            set { moBookStructDetails.miUpdatedById = value; }
        }
        public System.DateTime UpdatedDate
        {
            get { return moBookStructDetails.mdtUpdatedDate; }
            set { moBookStructDetails.mdtUpdatedDate = value; }
        }

        public string Description
        {
            get { return moBookStructDetails.msDescription; }
            set { moBookStructDetails.msDescription = value; }
        }

    
        public string BookRemoveReason
        {
            get { return moBookStructDetails.msBookRemoveReason; }
            set { moBookStructDetails.msBookRemoveReason = value; }
        }

        public string RackNumber
        {
            get { return moBookStructDetails.msRackNumber; }
            set { moBookStructDetails.msRackNumber = value; }
        }

        public string ShelfNumber
        {
            get { return moBookStructDetails.msShelfNumber; }
            set { moBookStructDetails.msShelfNumber = value; }
        }
        public string Remark
        {
            get { return moBookStructDetails.msRemark; }
            set { moBookStructDetails.msRemark = value; }
        }

        public string ISBN
        {
            get { return moBookStructDetails.msISBN; }
            set { moBookStructDetails.msISBN = value; }
        }

        public string BookEdition
        {
            get { return moBookStructDetails.msBookEdition; }
            set { moBookStructDetails.msBookEdition = value; }
        }
        public string CallNumber
        {
            get { return moBookStructDetails.miCallNumber; }
            set { moBookStructDetails.miCallNumber = value; }
        }
        public string Series
        {
            get { return moBookStructDetails.miSeries; }
            set { moBookStructDetails.miSeries = value; }
        }
        public string Status
        {
            get { return moBookStructDetails.msStatus; }
            set { moBookStructDetails.msStatus = value; }
        }
        public DateTime PublicationDate
        {
            get { return moBookStructDetails.msPublicationDate; }
            set { moBookStructDetails.msPublicationDate = value; }
        }



        public string BookYear
        {
            get { return moBookStructDetails.msBookYear; }
            set { moBookStructDetails.msBookYear = value; }
        }

        public Boolean IsBookLost
        {
            get { return moBookStructDetails.mbIsBookLost; }
            set { moBookStructDetails.mbIsBookLost = value; }
        }

        public Boolean IsWriteOffBook
        {
            get { return moBookStructDetails.mbIsWriteOffBook; }
            set { moBookStructDetails.mbIsWriteOffBook = value; }
        }

        public System.DateTime WriteOffDate
        {
            get { return moBookStructDetails.mdtWriteOffDate; }
            set { moBookStructDetails.mdtWriteOffDate = value; }
        }
        public Int32 BookDetailsId
        {
            get { return moBookStructDetails.miBookDetailsId; }
            set { moBookStructDetails.miBookDetailsId = value; }
        }
        public string Classification
        {
            get { return moBookStructDetails.msClassification; }
            set { moBookStructDetails.msClassification = value; }
        }
        public string Class
        {
            get { return moBookStructDetails.msClass; }
            set { moBookStructDetails.msClass = value; }
        }
        public Decimal LostPercentage
        {
            get { return moBookStructDetails.miLostPercentage; }
            set { moBookStructDetails.miLostPercentage = value; }
        }
        public string Language
        {
            get { return moBookStructDetails.msLanguage; }
            set { moBookStructDetails.msLanguage = value; }
        }
        public Int16 IsForIssue
        {
            get { return moBookStructDetails.miIsForIssue; }
            set { moBookStructDetails.miIsForIssue = value; }
        }

        public List<int> SelectedClasses
        {
            get { return moBookStructDetails.lstSelectedClasses; }
            set { moBookStructDetails.lstSelectedClasses = value; }
        }

        public int ReservedByParent
        {
            get{return moBookDC.ReservedByParent;}
            set { moBookDC.ReservedByParent = value; }
        }
        #endregion

        /// <summary>
        /// This method is uwed to delete book copy details.
        /// </summary>
        /// <param name="iBookId"></param>
        public void DeleteBook()
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.RemoveBooks();
        }

        public bool IsDuplicateBookNumber(string BookNo)
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.IsDuplicateBookNo(BookNo);
        }

        /// <summary>
        /// This method is used to check duplicate book number(Accession number).
        /// </summary>
        /// <param name="BookNo"></param>
        public void IsDuplicateBookNo(string BookNo)
        {
            moBookDC.BookInfo = moBookStructDetails;
            bool bIsDuplicate = moBookDC.IsDuplicateBookNo(BookNo);
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException("Accession number already exists.");
        }

        /// <summary>
        /// This method is used to get book copy details.
        /// </summary>
        public DataTable GetBookNoDetails(int iBookId, int iSchoolId)
        {
            return moBookDC.GetBookNoDetails(iBookId, iSchoolId);
        }

        /// <summary>
        /// This method is used to update book number(Book Copy) Details.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatementForBookNo(int iBookSrNo, string sBookNo, int iBookId, int aiIsGifted, decimal adBookPrice, int aiTotalPages, string aiBillNo, int aiVendorId, string asPurchaseDate)
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.GetUpdateStatementForBookNo(iBookSrNo, sBookNo, iBookId, aiIsGifted, adBookPrice, aiTotalPages, aiBillNo, aiVendorId, asPurchaseDate);
        }

        public DataTable FetchUserDetails(int iUserId, int iSchoolId)
        {
            return moBookDC.FetchUserDetails(iUserId, iSchoolId);
        }

        public void WriteOffBookCopy()
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.WriteOffBookCopy();
        }

        /// <summary>
        /// This method is uwed to delete book details.
        /// </summary>
        /// <param name="iBookId"></param>
        public void Delete(int iBookId)
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.DeleteBooks(iBookId);
        }
        public int GetCount(int iBookId)
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.GetCopyCount(iBookId);
        }

        /// <summary>
        /// This method is used get the list of user roles to fill the user role combo box.
        /// </summary>
        public void GetUserRoles()
        {
            moBookDC.GetUserRoles();
        }

        /// <summary>
        /// This method is used to get student's class and devision list.
        /// </summary>
        public void GetStudentClassNames(int aiSchoolId, int aiAcademicYearId)
        {
            moBookDC.GetStudentClassNames(aiSchoolId, aiAcademicYearId);
        }
        /// <summary>
        /// This method is used to get standards list.
        /// </summary>
        public static List<ClassDetails> GetStandards(int aiSchoolId, int aiAcademicYearId)
        {
            return BookDC.GetStandards(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get saved standards list for book.
        /// </summary>
        public List<ClassDetails> GetSavedStandards(int aiSchoolId, int aiBookId, int aiAcademicYearId)
        {
            return moBookDC.GetSavedStandards(aiSchoolId, aiBookId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get book details to fill the BooksListView with sorting.
        /// </summary>
        public static List<BookDetails> GetBookDetails(Int32 aiSchoolId, string asBookName, Int32 aiMediaType, Int32 aiMainCategoryId, String asAuthorName, String asPublisher, String asDescription, String asAccessionNumber, Int32 aiStandardId, string sSortExpression, string aiAccessionFromNumber, string aiAccessionTo, string asPrefix)
        {
            return BookDC.GetBookDetails(aiSchoolId, asBookName, aiMediaType, aiMainCategoryId, asAuthorName, asPublisher, asDescription, asAccessionNumber, aiStandardId, sSortExpression, aiAccessionFromNumber, aiAccessionTo,asPrefix);
        }

        public List<SchoolBookDetails> GetPagedBookList(Int32 aiSchoolId, string asBookName, string asAccessionNumber, string asAuthorName, 
            string asPublisher, string asLanguage, int aiStandardId, int aiMediaType, int aiBookId, int aiParentStaffId, int maximumRows, 
            int startRowIndex, string sortExpression, string sortDirection)
        {
            if (sortExpression != string.Empty && sortExpression!=null)
                sortExpression =sortExpression + " " + sortDirection;
            else
                sortExpression = "Book_Title asc ";
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            if (aiParentStaffId == 1)
                aiParentStaffId = 100;
            return moBookDC.GetPagedBookList(aiSchoolId, asBookName, asAccessionNumber, asAuthorName, asPublisher, asLanguage, aiStandardId, aiMediaType, aiBookId,aiParentStaffId, iEndIndex, iStartIndex, sortExpression);
        }

        public int GetCount(Int32 aiSchoolId, string asBookName, string asAccessionNumber, string asAuthorName, string asPublisher, 
            string asLanguage, int aiStandardId, int aiMediaType, int aiBookId, int maximumRows, int startRowIndex, 
            string sortExpression,string sortDirection,int aiParentStaffId)
        {
            sortExpression = " " + sortDirection;
            if (aiParentStaffId == 1)
                aiParentStaffId = 100;
            return moBookDC.GetCount(aiSchoolId, asBookName, asAccessionNumber, asAuthorName, asPublisher, asLanguage, aiStandardId, aiMediaType, aiBookId,aiParentStaffId);
        }
        /// <summary>
        /// This method is used to get user details to fill the BooksListView with sorting.
        /// </summary>
        public static List<LibaryUsers> GetAllUsers(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivisionId, int aiRollNo, string sortExpression, int maximumRows, int startRowIndex, string asEmployeNo)
        {
            asEmployeNo = asEmployeNo == null ? string.Empty : asEmployeNo;
			int iEndIndex = startRowIndex + maximumRows;
			// Increment the startRowIndex to prevent returning the last record of the previous page.
			if(startRowIndex != 0) startRowIndex++;
			if(sortExpression.Contains("ClassNameDesignation"))
			{
                if (aiUserRoleId == 3 || aiUserRoleId == 9)
					sortExpression = sortExpression.Replace("ClassNameDesignation", "StandardDivisionId");
				else
					sortExpression = sortExpression.Replace("ClassNameDesignation", "SortOrder");
			}
			return BookDC.GetAllUsers(aiSchoolId, aiAcademicYearId, asFilter, aiUserRoleId, aiStandardDivisionId, aiRollNo, sortExpression, startRowIndex, iEndIndex, asEmployeNo);
        }

        public static int GetAllUsersCount(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiUserRoleId, int aiStandardDivisionId, int aiRollNo, string sortExpression, int maximumRows, int startRowIndex, string asEmployeNo)
        {
            asEmployeNo = asEmployeNo == null ? string.Empty : asEmployeNo;
			return BookDC.GetAllUsersCount(aiSchoolId, aiAcademicYearId, asFilter, aiUserRoleId, aiStandardDivisionId, aiRollNo,asEmployeNo);
        }

        /// <summary>
        /// This method is used to get the vendor details of books to fill vendor combobox.
        /// </summary>
        ///// <param name="aiSchoolId"></param>
        public static List<LibraryVendors> GetLibraryVendors(int aiSchoolId)
        {
            return BookDC.GetLibraryVendors(aiSchoolId);
        }


        public bool IsAssignedCategory(string sCategory, int iMediaType, string sSubCategory)
        {
            moBookDC.BookInfo = moBookStructDetails;
            return moBookDC.IsAssignedCategory(sCategory, iMediaType, sSubCategory);
        }

        public static List<BookDetails> GetBookDetailsForBarcodes(int aiSchoolId, int aiAcademicYearId)
        {
            return BookDC.GetBookDetailsForBarcodes(aiSchoolId, aiAcademicYearId);
        }
        public void SaveReserveBook()
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.SaveReserveBook();
        }
        public static int GetReserveCount(int aiSchoolId, int aiAcademicYearId, int aiUserId,int aiBookId,int aiFlag)
        {
            return BookDC.GetReserveBookCount(aiSchoolId, aiAcademicYearId, aiUserId,aiBookId,aiFlag);
        }
        public static int GetReserveBooksPerPerson(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId)
        {
            return BookDC.GetReserveBooksPerPerson(aiSchoolId,aiAcademicYearId,aiUserRoleId);
        }
        public List<LibaryUsers> GetReservedBookDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asBookTitle, string asUserName, string sortExpression, string sortDirection, int startRowIndex, int maximumRows,int aiAllUser)
        {
            if (sortExpression == "Class")
                sortExpression = "Order by StandardId " + sortDirection +" , "+ " DivisionId "+sortDirection;
            else
                sortExpression = "Order by " + sortExpression + " " + sortDirection;
            int iEndIndex = startRowIndex + maximumRows;
           
            
            return moBookDC.GetReservedBookDetails(aiSchoolId, aiAcademicYearId, aiUserId,asBookTitle,asUserName,startRowIndex,iEndIndex,sortExpression,aiAllUser);
        }
        public int GetReservedBookCount(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asBookTitle, string asUserName, string sortExpression, string sortDirection, int startRowIndex, int maximumRows, int aiAllUser)
        {
            return moBookDC.ReserveBookCount;
        }
        public void CancelBookReservation(int aiUserId, int aiBookid, int aiSchoolId, int aiAcademicYearId)
        {
            moBookDC.CancelBookReservation(aiUserId, aiBookid, aiSchoolId, aiAcademicYearId); ;
        }
         
        /// <summary>
        /// This method is used to get all user to issue book.
        /// </summary>
        /// <param name="asFilterXml"></param>
        public void GetAllUsersForIssueBook(string asFilterXml)
        {
            moBookDC.BookInfo = moBookStructDetails;
            moBookDC.GetAllUsersForIssueBook(asFilterXml);  
        }
    }

    public class BookCollectionBL
    {
        private BookCollectionDC moBookCollectionDC = null;

        public BookCollectionBL()
        {
            moBookCollectionDC = new BookCollectionDC();
        }
        public BookCollectionBL(int aiSchoolId, int aiAcademicId, int aiInsertById)
        {
            moBookCollectionDC = new BookCollectionDC(aiSchoolId, aiAcademicId, aiInsertById);
        }

        /// <summary>
        /// This method is used to insert multiple books while importing books.
        /// </summary>
        /// <param name="asBookDetails"></param>
        /// <param name="asBookNoDetails"></param>
        public void InsertMultipleBooks(string asBookDetails, string asBookNoDetails, int aiOriginalConfigId)
        {
            moBookCollectionDC.InsertMultipleBooks(asBookDetails, asBookNoDetails, aiOriginalConfigId);
        }
    }
}

