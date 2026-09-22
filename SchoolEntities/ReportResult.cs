using System.Runtime.Serialization;

namespace SchoolEntities
{
    [DataContract]
    public class ReportResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string ContentType { get; set; }
    }
}
