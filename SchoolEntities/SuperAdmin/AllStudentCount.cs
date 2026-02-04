using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class AllStudentCount : SchoolEntity
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Total { get; set; }
        public string Girls { get; set; }
        public string Boys { get; set; }
    }

    public class ConnectionDetails : SchoolEntity
    {
        public int SchoolId { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string UserId {get; set;}
        public string Password { get; set; }
    }
}