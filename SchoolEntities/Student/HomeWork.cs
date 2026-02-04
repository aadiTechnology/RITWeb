using System;
using MasterEntities;
using Utility;
using System.Collections.Generic;
namespace SchoolEntities
{
	/// <summary>
	/// This class is used to  get or set homework details.
	/// </summary>
	public class Homework : SchoolEntity
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime AssignedDate { get; set; }
		public DateTime CompleteByDate { get; set; }
		public string Details { get; set; }
		public string AttachmentPath { get; set; }
		public SubjectMaster Subject { get; set; }
		public int StandardDivisionId { get; set; }
		public string Class { get; set; }
		public bool IsPublished { get; set; }
		public int InsertedById { get; set; }
		public string UnpublishedReason { get; set; }
		public Constants.Action Action { get;set; }
        public string AttachmentsName { get; set; }
        public int Flag { get; set; }
        public string DivisionIds { get; set; }
        public bool HasLinkedHomework { get; set; }
        public List<int> LinkedDivisions { get; set; }
	}
}
