using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentAssessmentDetails : SchoolEntity
    {
        public int StudentId { get; set; }
        public int GradeId { get; set; }
        public int SerialNo { get; set; }
        public string Aspect { get; set; }
        public int ParameterId { get; set; }
        public string Parameter { get; set; }
        public string Comment { get; set; }
    }

    public class StudentFavouriteDetails
    {
        public string FavouriteColour { get; set; }
        public string FavouriteFood { get; set; }
        public string FavouriteSport { get; set; }
        public string FavouriteSubject { get; set; }
    }

    public class ButtonStatesForStudentAssessment
    {
        public bool IsSaved { get; set; }
        public bool IsSubmitted { get; set; }
    }

    public class StudentFavouriteListDetails
    {
        public int SerialNo { get; set; }
        public string Parameter { get; set; }
        public int ParameterId { get; set; }
        public string Comment { get; set; }
    }

    public class CategorywiseComment
    {
        public int SerialNo { get; set; }
        public string Parameter { get; set; }
        public int ParameterId { get; set; }
        public string CommentForCategory { get; set; }
        public string Comment { get; set; }
    }
}
