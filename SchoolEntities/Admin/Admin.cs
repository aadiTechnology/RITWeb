using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Admin
{
    public class Admin
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string EmergencyContact { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool CanApproveRequisition { get; set; }
        public bool CanCreateGeneralRequisition { get; set; }
        public bool CanSanctionLeave { get; set; }
        public bool CanApproveVoucher { get; set; }
        public bool CanCreateVoucher { get; set; }
        public bool CanPublishUnpublishExam { get; set; }
        public bool CanSelfApprove { get; set; }
        public bool CanDeleteVoucher { get; set; }

        public bool CanEditOldFinancialYear { get; set; }
        public bool ShowAllSentSMS { get; set; }
        public int SalutationId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DOB { get; set; }
        public string PhotoFilePath { get; set; }

        public byte[] BinaryPhotoImage { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public int UserRoleId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
    }

    public class SMSReceiverDetails
    {
        public string SMS_Text { get; set; }
        public string Display_Text { get; set; }
        public int UserId { get; set; }
        public string MobileNo { get; set; }
    }
}
