using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Utility;
using MasterEntities;

namespace SchoolEntities.Payroll
{
   public class LeaveApprovalConfiguration :SchoolEntity
    {
        public int Id { get; set; }
        public bool IsFinalApprover { get; set; }
       
        public bool IsSubmitted { get; set; }
        public int ReportingUserId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }


        public bool IsDeleted { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public int InsertedById { get; set; }
        public int ApproverSortOrder { get; set; }
        public int UserRoleId { get; set; }

    }
}
