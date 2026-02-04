using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class AchievementDetails : SchoolEntity
    {
        public int Id { get; set; }
        public string AchievementTitle { get; set; }       
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public int InsertedById { get; set; }
        public int PhotoCount { get; set; }
    }

    public class Images 
    {
        public int Id { get; set; }
        public int achievementId { get; set; }
        public string ImagePath { get; set; }
        public int FieldIndex { get; set; }
    }
}




















