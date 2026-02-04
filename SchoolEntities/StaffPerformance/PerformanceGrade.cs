using System;
using SchoolEntities;
using Utility;

namespace StaffPerformanceEntity
{  
    [Serializable]
    public class PerformanceGrade : SchoolEntity
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int OriginalGradeId { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }
        public Constants.Action Action { get; set; }
        public int School_Id { get; set; }
    }
}
