using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using MasterEntities;

namespace PayrollEntities
{
    public class ODDetails : SchoolEntity
    {
        public int ODId { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public int StaffGroupId { get; set; }
    }

    public class ODDateDetails : SchoolEntity
    {
        public DateTime Date { get; set; }
    }
    public class UserDetailsForOD : SchoolEntity
    {
        public int StaffGroupsId { get; set; }
        public int UserId { get; set; }
    }
}
