namespace SchoolEntities
{
    public class Report
    {
        public int ReportUserDetailId { get; set; }  
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public bool HasAccess { get; set; }
        public bool HasFullAccess { get; set; }
        public bool IsViewApplicable { get; set; }
        public int IsDeleted { get; set; }
    }
}
