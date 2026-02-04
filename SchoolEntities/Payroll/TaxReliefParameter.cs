using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollEntities
{
    public class TaxReliefBaseField
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TaxReliefCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TaxReliefCalculationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
