using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class StudentAchievement : SchoolEntity
    {
        public int AchievementId { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public string StudentClass { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime AchievementDate { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public int NoteCategoryId { get; set; }
    }
}
