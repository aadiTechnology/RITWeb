using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace SuperAdminEntities
{
    public class SuperAdminDetails : SchoolEntity
    {
        public int SuperAdminDetailsId { get; set; }
        public int SalutationId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int FinancialYearId { get; set; }
    }
    public class Studentdetails:SchoolEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int IsRTE { get; set; }
        public string EnrolmentNumber { get; set; }
    }
}
