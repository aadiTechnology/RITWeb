using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{

    public class StudentRecordParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SectionName { get; set; }
        public int SectionId { get; set; }
        public int SortOrder { get; set; }
        public int ControlId { get; set; }

       

    }
}
