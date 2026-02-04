// Class Name       :- CareerDetailsInfo
// Purpose          :- This class is used to manage Carrer details.
// Date Of creation :- 1 Decemeber 2012
// Author Name      :- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace CareerEntities
{

    [Serializable]
    public class CareerDetailsInfo : SchoolEntity
    {
        public Int32 CareerDetailsID { get; set; }
        public String Name { get; set; }
        public DateTime DOB {get; set; }
        public String Address { get; set; }
        public String Email { get; set; }
        public String MobileNo { get; set; }
        public decimal YearOfExperience { get; set; }
        public String Post { get; set; }
        public String LastOrganisationName { get; set; }
        public String AreaOfSpecialization { get; set; }
        public String Resume { get; set; }
        public Boolean IsActive { get; set; }
        public String Education { get; set; }
    }
}
