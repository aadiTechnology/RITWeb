// Class Name       :- VendorDetailsDC
// Purpose          :- This class is used to Add vendors configurations.
// Date Of creation :- 12/01/2018
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace SchoolEntities
{
    public class VendorDetails : SchoolEntity
    {
        public int VendorId { get; set; }
        public int VendorNo { get; set; }
        public int SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string VendorAddress { get; set; }
        public string Pincode { get; set; }
        public string PhNumber { get; set; }
        public string MobileNo { get; set; }        
        public string FaxNo { get; set; }
        public string EmailId { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string VendorName { get; set; }
        public int TotalRows { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public int BankId { get; set; }
    }

    public class PODetailsForApprove : SchoolEntity
    {
        public int POId { get; set; }
        public int UserId { get; set; }
        public string RequesterName { get; set; }
        public DateTime PODate { get; set; }
        public string POCode { get; set; }
        public int RequesterId { get; set; }
    }
}
