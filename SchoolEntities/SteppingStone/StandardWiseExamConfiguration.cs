using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using Utility;
namespace StandardWiseExamConfigurationEntities
{
    public class StandardWiseExamConfiguration
    {
        public int SchoolwiseTestId { get; set; }
        public string SchoolwiseTestName { get; set; }
        public int OutOfMarks { get; set; }
        public int SchoolwiseStandardTestId { get; set; }
        public Constants.Action Action { get; set; }
        public string IsPublished { get; set; }
    }
    public class StandardGradeConfiguration 
    {
        public int StandardId { get; set; }
        public string IsSubjectConfigure { get; set; }
        public string IsCocoricularConfigure { get; set; }
        public string IsFailCriteriaNotConfigure { get; set; }
    }
    public class ExamStatusConfiguration
    {
        public int ExamStatusId { get; set; }
        public string DisplayName { get; set; }
        public string ShortName { get; set; }
        public string DisplayValue { get; set; }
        public string ForeColor { get; set; }
        public string BackColor { get; set; }
        public char ConsiderInTotal { get; set; }
        public char DisplayTotal { get; set; }
        public char ConsiderAsPresent { get; set; }
    
   }
}
