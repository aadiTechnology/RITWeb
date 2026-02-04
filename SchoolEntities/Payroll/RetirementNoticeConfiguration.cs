
using SchoolEntities;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;
using MasterEntities;

namespace PayrollEntities
{
	public class RetirementNoticeConfiguration : SchoolEntity
	{
		public int Id { get; set; }
		public int RetirementAge { get; set; }
		public int ReminderDays { get; set; }
		public UserRoleMaster UserRole { get; set; }  
	}

	public class StaffMemberRetirementNotice 
	{
		public int UserId { get; set; }		
		public string Name { get; set; }
		public int RemainingDays { get; set; }
		public DateTime RetirementDate { get; set; }
	}
}
