using System;
using SchoolEntities;
using Utility;

namespace StaffPerformanceEntity
{  
    [Serializable]
    public class PerformanceSkill : SchoolEntity
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int OriginalSkillId { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }
        public Constants.Action Action { get; set; }
        public int School_Id { get; set; }
        public int InputTypeId { get; set; }
        public bool IsEditableToAll { get; set; }
    }
}
