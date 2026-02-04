using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities 
{
    public class NoticeDetails 
    {
        public int NoticeId { get; set; }
        public string NoticeName { get; set; }
        public string DisplayLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SortOrder { get; set; }
        public string FileName { get; set; }
        public int UserId { get; set; }
        public int SortOrderLocationChanged { get; set; }
        public bool IsSelected { get; set; }
        public bool IsText { get; set; }
        public string NoticeContent { get; set; }
        public int InertedById { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public string Subject { get; set; }
        public string ImageFileName { get; set; }
        public string NoticeDescription { get; set; }
        public string NoticeImage { get; set; }
        public string ClassesIds { get; set; }
        public int StandardDivisionId { get; set; }
   
    }
}


