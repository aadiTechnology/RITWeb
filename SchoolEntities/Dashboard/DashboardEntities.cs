using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Dashboard
{
    
    // class to get/set data to fee status widget.
    public class FeeSummary
    {
        public string AmountExpectedToReceive { get; set; }
        public string TodaysCollection { get; set; }
        public string Concession { get; set; }
        public string DuesTillDate { get; set; }
        public string TotalPaidFees { get; set; }
    }

    // class to get/set data to attendance widget.
    public class AttendanceSummary
    {
        public int TotalClasses { get; set; }
        public int TotalStudent { get; set; }
        public int AttendanceMarkedClassCount { get; set; }
        public int AttendanceMarkedStudentCount { get; set; }
        public string Students { get; set; }
        public string Classes { get; set; }
        public List<MissingAttendance> MissingAttendance { get; set; }

        public AttendanceSummary()
        {
            MissingAttendance = new List<MissingAttendance>();
        }
    }

    // class to get/set data to student performance widget.
    public class StandardwiseStudentPerformance
    {
        public List<StudentGradeDetails> GradeDetails { get; set; }
        public string[] Standards { get; set; }
        public int MaxStudentCount { get; set; }
    }

    // class to get/set data to Exam student Performance widget.
    public class Exam
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
    }

    // class to get/set data to missing attendance widget.
    public class MissingAttendance
    {
        public string ClassNames { get; set; }
        public float MissingPercentage { get; set; }
    }

    // class to get/set student grade details.
    public class StudentGradeDetails
    {
        public int[] StudentCount { get; set; }
        public string Grade { get; set; }
        public string Color { get; set; }
    }

    // class to get/set standard wise grade.
    public class GradeStandardCountDetails
    {
        public string Grade { get; set; }
        public string Standard { get; set; }
        public int StudentCount { get; set; }
    }

    // class to get/set data to accounts widget.
    public class InflowOutflowSummary
    {
        public double[] MonthwiseOutflowAmount { get; set; }
        public double[] MonthwiseInflowAmount { get; set; }
        public double MaxSalaryAmount { get; set; }

        public InflowOutflowSummary()
        {
            MonthwiseOutflowAmount = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            MonthwiseInflowAmount = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
    }

    // class to get/set data to payroll widget.
    public class PayrollSummary
    {
        //public string[] Months { get; set; }
        public double[] MonthWiseSalaryAmount { get; set; }
        public string IncomeTaxAmount { get; set; }
        public string PreviousMonthPaidSalary { get; set; }
        public double MaxPaidSalaryAmount { get; set; }
    }

    // class to get/set data to birthday widget.
    public class StaffDetails
    {
        public string UserName { get; set; }
        public string PhotoPath { get; set; }
        public string Date { get; set; }
        public string Classes { get; set; }
    }

    // class to get/set image related data to photo gallery widget.
    public class ImageDetails
    {
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }

    // class to get/set data to photo gallery widget.
    public class PhotoGalley
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ImageDetails> ImageList { get; set; }
		public int Month { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
    }

    // class to get/set student count data  to stats widget.
    public class StudentCountDetails
    {
        public int GirlsCount { get; set; }
        public int BoysCount { get; set; }
        public int TotalCount { get; set; }
        public int LeftCount { get; set; }
        public int NewJoinCount { get; set; }
        public int RteCount { get; set; }
    }

    // class to get/set staff count data  to stats widget.
    public class StaffCountDetails
    {
        public int TeacherCount { get; set; }
        public int AdminCount { get; set; }
        public int OtherCount { get; set; }
        public int TransportCount { get; set; }
        public int ResignedCount { get; set; }
    }

    // class to get/set library related data  to stats widget.
    public class LibraryCountDetails
    {
        public int TotalCount { get; set; }
        public int ReceivedCount { get; set; }
        public int PurchasedCount { get; set; }
        public int LostCount { get; set; }
    }

    // class to get/set to user feedback details to feedback widget.
    public class UserFeedBack
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public bool IsSelected { get; set; }
    }
 
	// class to get/set  data to unread message popup.
    public class UnreadMessage
    {
        public string SenderUserId { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public int MessageCount { get; set; }
        public string ReturnUrl { get; set; }
    }

    // class is use to get user's photo path
    public class SenderPhoto 
    {
        public string Id { get; set; }
        public string Photo { get; set; }
    }

    // class to get/set unread messages and count to unread message popup.
    public class UnreadMessageDetails
    {
        public List<UnreadMessage> UnreadMessages { get; set; }
        public List<SenderPhoto> SenderPhoto { get; set; }
        public int UnreadMessageCount { get; set; }
        public List<UserProfileData> UserProfilePicData { get; set; }
    }

    // class to get/set upcoming event on upcoming event widget.
    public class UpcomingEvents
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EndDateUniversal { get; set; }
        public string EventDescription { get; set; }
        public string EventTitle { get; set; }
        public string StandardName { get; set; }
        public string EventType { get; set; }

    }

    public class ClasswiseAttendanceSummary
    {
        public int[] StudentCount { get; set; }
        public string[] AttendanceDays { get; set; }
        public int MaxCountOfStudent { get; set; }
    }

    public class UserProfileData 
    {
        public string UpdateDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}
