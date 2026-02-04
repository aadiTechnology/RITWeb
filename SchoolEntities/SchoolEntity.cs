/* Class Name :- SchoolEntity.cs
 * Created By :- Sachin
 * Created Date :- 03-Dec-2010
 * Description :- This class is used create basic School Entities.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SchoolEntities
{
    [Serializable]    
    public class SchoolEntity
    {
        [XmlIgnore] public int SchoolId { get; set; }
        [XmlIgnore] public int AcademicYearId { get; set; }
        [XmlIgnore] public int InsertedById { get; set; }
        [XmlIgnore] public int UpdatedById { get; set; }
        [XmlIgnore] public string InsertDate { get; set; }
        [XmlIgnore] public string UpdateDate { get; set; }
        [XmlIgnore] public int Is_Deleted { get; set; }
        [XmlIgnore] public string OrganizationName { get; set; }
        [XmlIgnore] public string SchoolName { get; set; }
        [XmlIgnore] public string Address { get; set; }
    }

    public class AcademicYear
    {
        public int Id { get; set; }
        public string Year { get; set; }
    }

    public class UserDetailsForLoginSMS
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public int UserRoleId { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber1 { get; set; }
   
    }



    public class ConcessionDetails
    {
        public int Rule_Id { get; set; }
        public string RuleName {get; set; }
        public string Description { get; set;}
        public int FeeTypeId  {get; set; }
        public string FeeSubType { get; set; }
        public int PercentageConcession { get; set; }
    }

    public class AccountHeaderConfig
    {
        public int AccountHeaderId { get; set; }
        public int SchoolwiseBankId { get; set; }
        public string AccountHeaderName { get; set; }
        public string SchoolCurrentAccountNumber { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
    
    }
    public class ResetFeeReceipt
    {
        public DateTime Date { get; set; }
        public int AccountHeaderId { get; set; }
        public string AccountHeaderName { get; set; }
        public string FeeType { get; set; }
        public string OrderBy { get; set; }
        public int OriginalFeeTypeId { get; set; }
    
    
    }

    public class SchoolFolder
    {
        public int SchoolId { get; set; }
        public string FolderName { get; set; }
    }

  
}


