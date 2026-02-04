using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Payroll
{
    public class UserRejoiningDetails
    {
        public int UserId { get; set; }
        public int UserRejoinId { get; set; }
        public string UserName { get; set; }
        public int StaffGroupId { get; set; }
        public string EmployeeNo { get; set; }
        public string AccountNo { get; set; }
        public string PFNo { get; set; }
        public string UAN { get; set; }
        public string PANNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ResignationDate { get; set; }
        public int TotalRowCount { get; set; }
        public DateTime OldJoiningDate { get; set; }
    }
}
