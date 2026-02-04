using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class NewsDetails:SchoolEntity
    {
        public int NewsId { get; set; }
        public string NewsHeading { get; set; }       
        public string NewsDate { get; set; }
        public int SortOrder { get; set; }
        public string FileName { get; set; }
        public int IsText { get; set; }
        public string NewsContent { get; set; }
        public bool IsSelected { get; set; }
        public int InertedById { get; set; }
    }
}


