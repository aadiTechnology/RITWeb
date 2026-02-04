using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities; 

namespace ProductDemoEntities
{

    [Serializable]
    public class ProductDemoDetailsInfo : SchoolEntity
    {
        public Int32 ProductDemoDetailsID { get; set; }
        public String Name { get; set; }
        public String Designation { get; set; }
        public String NameOfTheInstitute { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String State { get; set; }
        public Int32 Country { get; set; }
        public String PhoneNo { get; set; }
        public String MobileNo { get; set; }
        public String WebSite { get; set; }
        public Boolean IsActive { get; set; }
    }

    [Serializable]
    public class FranchiseRequestDetails
    {
        public int FranchiseRequestDetailsID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string NameOfTheFirm { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }     
        public string MobileNo { get; set; }
        public string WebSite { get; set; }        
    }
}
