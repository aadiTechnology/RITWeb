using System;
using SchoolEntities;

namespace StaffPerformanceEntity
{
    [Serializable]
    public class PerformanceParameter : SchoolEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public int Year { get; set; }
        public int SkillId { get; set; }
        public int AppraisalFormTypeId { get; set; }
        public bool IsSubmitted { get; set; }
        public string FormType { get; set; }
    }
}
