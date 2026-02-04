#region Assembly SchoolEntities.dll, v4.0.30319
// D:\Assemblies\SchoolEntities.dll
#endregion

using System;
using SchoolEntities;

namespace LCUploadEntities
{   
    public class StudentLCDetails : SchoolEntity
    {
        public string EnrollmentNo { get; set; }
        public int RollNo { get; set; }
        public string LCFilePath { get; set; }                        
        public int StudentId { get; set; }
        public string StudentName { get; set; }        
        public int LCUploadStatus { get; set; }

        public int SrNo { get; set; }
        public int LCNo { get; set; }
        public string ClassName { get; set; }
        public int TotalRows { get; set; }
    }
}
