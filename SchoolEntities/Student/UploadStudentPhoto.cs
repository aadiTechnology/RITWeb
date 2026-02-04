using SchoolEntities;
using System;

namespace SchoolEntities
{
    public class StudentPhotoUploadDetails
    {
        public string StudentName { get; set; }
        public bool IsSaved { get; set; }
        public bool IsSubmitted { get; set; }
        public byte[] PhotoImage { get; set; }
        public bool IsOldPhotoExist { get; set; }
        public int SchoolwiseStudentId { get; set; }
    }

    public class SavePhotoFile
    {
        public byte[] PhotoFilePathInBinary { get; set; }
        public int StudentId { get; set; }       
    }
}
    