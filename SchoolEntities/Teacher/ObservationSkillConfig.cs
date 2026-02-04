using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Teacher
{
   public class ObservationSkillConfig
    {
        //public ObservationSkillConfig();
        public int SubjectId { get; set; }
        public int StandardId { get; set; }
        public string SubjectName { get; set; }
        public string StandardName { get; set; }
        public int SortOrder { get; set; }
        public string Skill { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
