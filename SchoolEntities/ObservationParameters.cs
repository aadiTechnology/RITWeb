using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Teacher
{
    public class ObservationParameters
    {
        public int StandardId { get; set; }
        public int SkillId { get; set; }
        public bool IsSubmitted { get; set; }
        public int SortOrder { get; set; }
        public int Id { get; set; }
        public string Parameter { get; set; }
        public string SkillName { get; set; }
        public string Name { get; set; }
    }
}
