using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.eStore
{
    public class ItemList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
    }

    public class AllStandardDetails
    {
        public int Standard_Id { get; set; }
        public string Standard_Name { get; set; }
    }
}
