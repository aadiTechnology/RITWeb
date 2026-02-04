using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class ParentHealthDetails
    {
        public string FatherOrMother { get; set; }
        public string Name { get; set; }
        public string AadharCardNo { get; set; }
        public string BloodGroup { get; set; }
        public DateTime DOB { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
     }
}
