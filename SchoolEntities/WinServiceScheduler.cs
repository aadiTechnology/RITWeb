/* ------------------------------------------------------------------------------------------------
 *	FileName	: WinServiceScheduler.cs
 *	Author		: Vishal B. Shah
 *	Date		: 07-July-2012
 *	Purpose		: Defines entities which are used the in the School Win Service scheduler.
 * ------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Utility;

namespace SchoolEntities
{

	[Serializable]
	public class Job
	{
		public int JobId { get; set; }
		public string JobName { get; set; }
		public string RunTimeString { get; set; }
		public string JobMethodName { get; set; }
		public List<School> Schools { get; set; }
		
		[XmlIgnore]
		public TimeSpan RunTime
		{
			get
			{
				string[] arrRunTimeTokens = RunTimeString.Split(':');
				return arrRunTimeTokens.Length != 3 ? DateTime.Now.TimeOfDay : new TimeSpan(arrRunTimeTokens[0].ToInt(), arrRunTimeTokens[1].ToInt(), arrRunTimeTokens[2].ToInt());
			}
		}
	}

	[Serializable]
	public class School
	{
		public int SchoolId { get; set; }
		public string DbName { get; set; }
		public string SMSSenderIP { get; set; }
		public string SMSSenderUName { get; set; }
		public string SMSSenderUPwd { get; set; }
		public string AdminEmailId { get; set; }
		public int AdminUserId { get; set; }
		public string SendSMS { get; set; }
		public string SendMail { get; set; }
		public string SMTPIPAddress { get; set; }
		public int SMTPPort { get; set; }
        public string ScheduleAt { get; set; }
        public string SMSNotification { get; set; }        
        public string DeleteLogData { get; set; }
        public string SendPushNotification { get; set; }
        public int Day { get; set; }
        public string SMSProvider { get; set; }
	}
}
