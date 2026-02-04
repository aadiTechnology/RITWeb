/*  Author		: Vishal Shah
 *  Date		: 20-Sept-2011
 *  Description : This is the entity class which holds information about a single Database Activity Log
 */

using System;

namespace SchoolEntities
{
	[Serializable]
	public class ActivityLog : SchoolEntity
	{
		public PageRequestLog PageRequest { get; set; }
		public string SQLStatement { get; set;}
		public string Parameters { get; set; }
		public bool IsSproc { get; set; }
		public long ExecutionTime { get; set; }
		public new DateTime InsertDate { get; set; }
	}
}
