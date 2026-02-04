using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Teacher
{
  public  class AssignSummaryGradesDetails
    {
      public int RollNo { get; set; }
      public string StudentName { get; set; }
      public int GradeId { get; set; }
      public int YearwiseStudentId { get; set; }
     }

  public class ButtonStatesforAssignSummaryGrades
  {
      public bool IsSaved { get; set; }
      public bool IsSubmitted { get; set; }
      public bool IsPublished { get; set; }
  }
}
