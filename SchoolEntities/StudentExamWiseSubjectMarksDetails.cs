namespace SchoolEntities
{
   public class StudentExamWiseSubjectMarksDetails
    {
       public int TestId { get; set; }
       public string TestName { get; set; }
       public decimal Marks { get; set; }
       public decimal OutOfMarks { get; set; }
       public string Grade { get; set;}
       public string SubjectName { get; set; }
       public string IsAbsentGrade { get; set; }
       public bool IsGradingSubject { get; set; }
    }
}
