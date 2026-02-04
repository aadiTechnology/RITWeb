
namespace SchoolEntities
{
    public class HealthComponent
    {
        public int Id { get; set; }
        public int FinancialYearId { get; set; }
        public int IsDeleted { get; set; }
        public string ComponentName { get; set; }
        public int SortOrder { get; set; }
        public bool IsFitnessComponent { get; set; }
    }
}