/* --------------------------------------------------------------------------------
 *	FileName	: Event.cs
 *	Author		: Vishal B. Shah
 *	Date		: 19-Jan-2012
 *	Purpose		: This class is used to represent an Event entity in the database.
 * --------------------------------------------------------------------------------
 */

using System;

namespace SchoolEntities
{
     [Serializable]
	public class Event : SchoolEntity
	{

		#region -- PROPERTIES --

		public int EventId { get; set; }
		public string EventDescription { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Standards { get; set; }
		public bool Display_On_Homepage { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }

		#endregion -- PROPERTIES --

	}
}