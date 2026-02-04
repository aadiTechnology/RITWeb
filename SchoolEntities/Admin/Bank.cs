/* -----------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 7-Mar-2012
 *	Description	: This is the Entity class to represent a Bank in the system.
 * -----------------------------------------------------------------------
 */

using System;

namespace SchoolEntities
{
	[Serializable]
	public class Bank : SchoolEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public string BankCode { get; set; }
	}
}