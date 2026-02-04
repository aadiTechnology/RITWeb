using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class SurveyStudentDetails
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string School { get; set; }
        public int SurveySchoolId { get; set; }
        public string Standard { get; set; }
        public int StandardId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int IsInterested { get; set; }
    }

    public class Standard
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SurveySchool
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SurveyStudentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
