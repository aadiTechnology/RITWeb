using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class TeacherAdditionalDetails 
    {
        public string TeacherName { get; set; }
        public int AdditionalDetailsId { get; set; }
        public int TeacherId {get; set;}
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
    }
}
