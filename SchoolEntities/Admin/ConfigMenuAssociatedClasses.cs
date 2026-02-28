using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Admin
{
   public class ConfigMenuAssociatedClasses
    {
        public int StandardwiseDivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DivisionId { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginaDivisionId { get; set; }
        public int SavedStandardDivisionId { get; set; }
       public bool   IsRecordSaved { get; set; }
    }
}
