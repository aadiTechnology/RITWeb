/* ----------------------------------------------------------------------------
 *	FileName	: SchoolModule.cs
 *	Author		: Vishal B. Shah
 *	Date		: 7-May-2012
 * ----------------------------------------------------------------------------
 */

namespace SchoolEntities
{
	/// <summary>
	/// 	Represents a School module in the system.
	/// </summary>
	public class SchoolModule
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public bool IsScreenAccessRestricted { get; set; }
	}
}