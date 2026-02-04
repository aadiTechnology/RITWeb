/* Class Name :- UserBasicDetails.cs
 * Created By :- Pravin
 * Created Date :- 06-Jun-2012
 * Description :- This class is used create basic details for User.
*/

namespace PayrollEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SchoolEntities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserBasicDetails
    {
        public int UserId { get; set; }
        public string StaffName { get; set; }
        public string PanNo { get; set; }
        public string JoiningDate { get; set; }
        public string PermanentDate { get; set; }
        public string ResignationDate { get; set; }
        public string FilePath { get; set; }
        public int SchoolId { get; set; }
        public int IsDeleted { get; set; }
        public int InsertedById { get; set; }
        public int JobTypeId{ get; set; }
        public string JobTypeName { get; set; }
        public int SrNo{ get; set; }
        public int WorkingStatusId { get; set; }
        public string EmployeeNo { get; set; }
        public string AadharNo { get; set; }
        public string AadharFileUpload { get; set; }
        public string TransferDate { get; set; }
        public int GradePay { get; set; }
        public bool IsOnCHB { get; set; }
        public int BloogGroupId { get; set; }
    }

    public class UserShiftAssociationDetails
    {
        public int UserId { get; set; }
        public int Action { get; set; }
    }
}
