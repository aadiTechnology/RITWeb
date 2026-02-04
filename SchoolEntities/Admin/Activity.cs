using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Admin
{
    public class Activity : SchoolEntity
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSaved { get; set; }
    }
}
