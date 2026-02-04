using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace DocumentEntity
{
    [Serializable]
    public class StandardwiseDocument
    {
        public int StandardwiseDocumentId { get; set; }
        public string DocumentName { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginalDocumentId { get; set; }
        public bool IsContinue { get; set; }
        public int Is_Deleted { get; set; }
        public int SchoolId { get; set; }
        public bool IsAppForExisStud { get; set; }
        public bool IsSubmit { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class StudentDocument 
    {
        public int StudentDocumentId { get; set; }
        public int StandardwiseDocumentId { get; set; }
        public string DocumentName { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApplicable { get; set; }
		public int DocumentCount { get; set; }
        public bool IsSubmissionMandatory { get; set; }
    }
}
