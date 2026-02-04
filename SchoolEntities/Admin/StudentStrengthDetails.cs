using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Admin
{
    public class StudentStrengthDetails
    {
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public int MaxStrength { get; set; }
        public bool IsExceeded { get; set; }
    }
}
