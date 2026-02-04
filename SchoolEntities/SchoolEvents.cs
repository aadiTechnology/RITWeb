using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{

        public class EventDetails
        {
        public string Event_Description { get; set; }
        public int Inserted_By_id { get; set; }
        public int Display_On_Homepage { get; set; }
        public string Event_Image { get; set; }
        public int Event_Id { get; set; }
        public string Event_Start_Date { get; set; }
        public string Event_End_Date { get; set; }
        public string Event_Comment { get; set; }
        public string AssociatedStandards { get; set; }
    }
}


