using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExternalLectures
{
    public class TeacherExternalLecturesDetails
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public bool IsMPT { get; set; }
        public bool IsAssembly { get; set; }
        public bool IsStayBack { get; set; }
        public bool WeeklyTestApplicable { get; set; }
    }

    public class StandardDivisions
    {
        public int StandardwiseDivisionId { get; set; }
        public string StandardDivision { get; set; }
        public int StandardId { get; set; }
    }

    public class WeekDays
    {
        public int WeekDayId { get; set; }
        public string WeekDay { get; set; }
        public bool IsConfigured { get; set; }
    }

    public class StayBackLectureDetails
    {
        [XmlIgnore]public int StandardwiseDivisionId { get; set; }
        [XmlIgnore]public int WeekDayId { get; set; }
        [XmlIgnore]public string WeekDay { get; set; }
        public int LectureNo { get; set; }
        public int StayBackDetailsId { get; set; }
        public bool IsStayBackLecture { get; set; }
    }

    public class StandardWeekDaywsieStayBackLectureDetails
    {
        public string StandardName { get; set; }
        public string DivisionName { get; set; }
        public string WeekDay { get; set; }
        public int MaxNoOfLecturesForStandard { get; set; }
        public string WeekdayShortName { get; set; }
    }
}
