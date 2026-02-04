/* Class Name :- StudentDetails.cs
 * Created By :- Shobha
 * Created Date :- 03-Dec-2010
 * Description :- This class is used create basic objects related to students.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace StudentEntities
{
    [Serializable]
    public class StudentInfo : SchoolEntity
    {
        public string RegNo { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public int YearwiseStudentId { get; set; }
        public int UserId { get; set; }
        public int RollNo { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Academic_Year { get; set; }
        public string School_Name { get; set; }
        public string School_Orgn_Name { get; set; }
        public int First_Term_PresentDay { get; set; }
        public int Final_Term_PresentDay { get; set; }
        public int First_Term_Total { get; set; }
        public int Final_Term_Total { get; set; }
        public int OptionalSubjectId { get; set; }
        public int IsLeftStudent { get; set; }
        public int IsNewStudent { get; set; }
        public int StudentSiblingId { get; set; }
		public int ProgresSheetID { get; set; }
        public int Standard_Division_Id { get; set; }
        public int StandardId { get; set; }
        public string ProgressReportType { get; set; }
        public string ShowProgressReport { get; set; }
        public int EditStatus { get; set; }
        public int HouseId { get; set; }
        public string HouseColor { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int SecondLanguageSubjectId { get; set; }
        public int ThirdLanguageSubjectId { get; set; }
        public bool ShowDeleteButton { get; set; }
    }
    [Serializable]
    public class StudentInfoForHeightWeight : SchoolEntity
    {
        public int RollNo { get; set; }
        public int YearWiseStudentId { get; set; }
        public string StudentName { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int IsLeftStudent { get; set; }
       
    }

    public class StudentDetails
    {
        public string Name { get; set; }
        public int RollNo { get; set; }
        public int SchoolwiseStandardDivisionId { get; set; }
        public string IsLock { get; set; }
        public string MobileNo1 { get; set; }
        public int YearwiseStudentId { get; set; }
        public int IsNewStudent { get; set; }
        public DateTime DOB { get; set; }
        public string EnrollmentNo { get; set; }
        public int UserId { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public string StandrdDivision { get; set; }
        public string IsLeave { get; set; }
        public string PhotoFilePath { get; set; }
        public int StandrdId { get; set; }
        public int DivisionId { get; set; }
        public bool HasDebitEntries { get; set; }
    }
    public class SiblingInfo : SchoolEntity
    {
        //Sibling Info of student
        public string CommonFieldName { get; set; }
        public int CommonFieldId { get; set; }
    }
    public class ClassDetailsForExam : SchoolEntity
    {
        public int StandardDivisionId { get; set; }
        public string Classname { get; set; }
    }
      
    public class Operator
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class StudentDetailsForSMS
    {   
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string StudentName { get; set; }
        public int UserId { get; set; }
    }
}