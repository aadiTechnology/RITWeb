using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class JobDetails : SchoolEntity
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }       
        public string Qualification { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public int InertedById { get; set; }
        public int Experience { get; set; }
    }
}


