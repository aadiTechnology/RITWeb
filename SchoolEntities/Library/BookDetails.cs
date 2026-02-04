using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace BookEntities
{
    public class SchoolBookDetails : SchoolEntity
    {
        public int Book_Id { get; set; }
        public int Is_Printable { get; set; }
        public string Book_Title { get; set; }
        public int Category_Id { get; set; }
        public string Author_Name { get; set; }
        public string Published_By { get; set; }
        public decimal Book_Price { get; set; }
        public string Decription { get; set; }
        public string RackNumber { get; set; }
        public string ShelfNumber { get; set; }
        public string Remark { get; set; }
        public string Book_No { get; set; }
        public string Category_Name { get; set; }
        public int Available_Books { get; set; }
        public int Total_Book_Quantity { get; set; }
        public int RowNo { get; set; }
        public int AccesNo { get; set; }
        //new fields

        public int TotalPages { get; set; }
        public string Classification { get; set; }
        public string Class { get; set; }
        public decimal LostPercentage { get; set; }
        public string Language { get; set; }
        public int IsGifted { get; set; }
        public string BillNo { get; set; }
        public DateTime DateOfPurchage { get; set; }
        public string ISBN { get; set; }
        public int VendorId { get; set; }
        public int IsForIssue { get; set; }
        public string Standards { get; set; }

    }

    [Serializable]
    public class BookDetails : SchoolEntity
    {
        public int Book_Detail_Id { get; set; }
        public int Book_Id { get; set; }
        public string Book_Title { get; set; }
        public string Book_No { get; set; }
        public int Is_Issue { get; set; }
        public bool IsBookLost { get; set; }
        public string Remove_Reason { get; set; }
        public bool IsWriteOffBook { get; set; }
        public DateTime WriteOff_Date { get; set; }
    }

    public class UserRoles : SchoolEntity
    {
        public int User_Role_Id { get; set; }
        public string User_Role_Name { get; set; }
    }
    public class ClassDetails : SchoolEntity
    {
        public int StandardDivisionId { get; set; }
        public string Classname { get; set; }
    }

    public class StandardDetails : SchoolEntity
    {
        public int Standard_Id { get; set; }
        public string Standard_Name { get; set; }
    }
    public class LibaryUsers : SchoolEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RegNo { get; set; }
        public int UserRoleId { get; set; }
        public string ClassNameDesignation { get; set; }
        public string Designation { get; set; }
        public int Book_Id { get; set; }
        public string Book_No { get; set; }
        public string Book_Title { get; set; }
        public DateTime Issue_Date { get; set; }
        public DateTime Return_Date { get; set; }
        public string IsActive { get; set; }
        public int RollNo { get; set; }
        public string EmployeeNo { get; set; }
        public bool IsForParent { get; set; }//for book reservation
        public string ReservationDate { get; set; }
        public int StandardDivisionId { get; set; }
        public string EnrollmentNo { get; set; } 
    }

    public class LibraryVendors : SchoolEntity
    {   
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
    }
    public class IssueBookUserMaster : SchoolEntity
    {        
        public int UserId { get; set; }
        public int RowNo { get; set; }
        public string UserName { get; set; }
        public int StandardDivisionId { get; set; }
        public string IsActive { get; set; }
        public IssueBookUserDetails IssueBookUserDetail { get; set; }
        public IssueBookUserRollNoDesig IssueBookUserRollNoDesig { get; set; }
        public string BookwiseRenewCount { get; set; }
        public int UserIssueBookCount { get; set; }
        public string EnrollOrEmpNo { get; set; }
        public bool HasLateEntry { get; set; }
        public string EnrollmentNo { get; set; } 
     }
    public class UserBookRenewDetails
    {
        public int UserId { get; set; }
        public string BookwiseRenewCount { get; set; }
    }
    public class IssueBookUserDetails : SchoolEntity
    {
        public int UserRoleId { get; set; }
        public string RollNoOrEmployeeNo { get; set; }
        public string EnrollmentNo { get; set; }
    }
    public class IssueBookUserRollNoDesig : SchoolEntity
    {
        public string ClassNameDesignation { get; set; }
        public string RollNoEmployeeNo { get; set; }
        public string EnrollmentNo { get; set; }
    }
    public class AllBookDetails
    {
        public string BookNo { get; set; }
        public string BookDetailsId { get; set; }
        public string BookName { get; set; }
    }
    public class BookIssueRenewCountMaster 
    {
        public int MaxIssueBookCount { get; set; }
        public int MaxRenewBookCount { get; set; }
        public int LateFeePerDay { get; set; }
        public int LateFeeEffectiveFrom { get; set; }
    }
    public class IssueReturnDateMaster
    {
        public string IssueDate { get; set; }
        public string ReturnDate { get; set; }
        public MailReserveBookUserMaster MailReserveBookUserMaster { get; set; }
    }
    public class MailReserveBookUserMaster
    {
        public string BookName { get; set; }
        public string BookReserveUserList { get; set; }
    }

    public class BulkBookDetails
    {
        public int UserId { get; set; }
        public string AccessionNo { get; set; }
    }
   
}
