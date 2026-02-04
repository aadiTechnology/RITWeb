using System;
namespace SchoolEntities
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class MessageDetails
	{
		public int MessageReceiverDetailsId { get; set;}
		public string EmailAddress { get; set;}
	}

    public class ReadReceiptDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }
        public string ReadingDatetime { get; set; }
        public string UserRole { get; set; }  
    }

    public class MessageDraftDetails
    {
        public int DraftId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime DraftDate { get; set; }
        public string ReceipantList { get; set; }
        public string DisplayText { get; set; }
        public string FromName { get; set; }
        public string CcReciepientList { get; set; }
        public string CcDisplayText { get; set; }
    }

    public class MessageDraftUserDetails
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserName { get; set; }
    }

    public class StandardDivisionDetails
    {
        public int StandardDivisionId { get; set; }
    }

    public class FileAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}
