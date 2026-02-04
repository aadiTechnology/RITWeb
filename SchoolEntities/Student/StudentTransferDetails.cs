using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentTransferDetails
    {
    }

    public class TransferStudent
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int FromBranchId { get; set; }

    }

    [Serializable]
    public class SchoolBranchDetails
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string ReportingServer { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SchoolName { get; set; }
        public int GroupId { get; set; }
    }
    public class DivisionDetails
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

    }
}
