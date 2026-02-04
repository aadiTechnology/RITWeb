using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Admin
{
    public class SchoolGuestDetails : SchoolEntity
    {
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public DateTime Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string MobileNum { get; set; }
        public string AadharCardNo { get; set; }
        public string PanCardNo { get; set; }
        public string PurposeOfVisit { get; set; }
        public string OrganisationName { get; set; }
        public string WhomToMeet { get; set; }
        public string Designation { get; set; }
        public byte[] GuestPhoto { get; set; }
        public int SalutaionId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}
