using System;
using System.Collections.Generic;

namespace SchoolEntities
{
	
	/// <summary>
	/// Represents an entry in the PageRequestLog table.
	/// </summary>
	public class PageRequestLog : SchoolEntity
	{
		public int Id { get; set; }
        public int RequestSchoolId { get; set; }
        public int RequestAcademicYearId { get; set; }
		public string SessionId { get; set; }
		public int UserId { get; set; }
		public string IPAddress { get; set; }
		public string Browser { get; set; }
		public string BrowserVersion { get; set; }
		public string Page { get; set; }
		public string QueryString { get; set; }
		public bool IsPostBack { get; set; }
		public List<KeyValuePair<string, string>> RequestData { get; set; }
		public long ExecutionTime
		{
		    get { return InsertDate > DateTime.MinValue ? (long)(DateTime.Now - InsertDate).TotalMilliseconds : 0; }
		}
		public new DateTime InsertDate { get; set; }

		// A List of ActivityLog entity objects, which represent all the db activity that happend during the PageRequest.
		public List<ActivityLog> ActivityLog { get; set; }
	}

}
