using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Survey
{
   public class GuestDetails
    {
        public int GuestId { get; set; }
        public int SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Area { get; set; }
        public bool IsSendSMS { get; set; }
        public int ReferenceGuestId { get; set; }
        public string FullName { get; set; }
        public string ReferenceGuestFullName { get; set; }
    }

    public class GuestReferenceDetails
    {
        public int GuestId { get; set; }
        public string GuestFullName { get; set; }
    }
}
