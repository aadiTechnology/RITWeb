
namespace SchoolEntities
{
    public class HealthParameter
    {
        public int Id { get; set; }
        public int FinancialYearId { get; set; }
        public int IsDeleted { get; set; }
        public string ComponentName { get; set; }
        public int HealthComponentId { get; set; }
        public string ParameterName { get; set; }
        public string TestName { get; set; }
        public string Measure { get; set; }
        public int SortOrder { get; set; }
    }   
}
