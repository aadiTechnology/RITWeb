using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class HouseConfigurationDetails :SchoolEntity
    {
        public int StandardId { get; set; }
        public bool AllowHouseConfiguration { get; set; }
        public string StandardName { get; set; }
    }
}
