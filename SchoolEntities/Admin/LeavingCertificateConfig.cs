/*   Author		 : Vishal Shah
 *   Date		 : 10 Sept 2011
 *	 Description : This is the Entity class which is used inthe Leaving Certificate Report Configuration screen.
 */

using System.Xml.Serialization;

namespace SchoolEntities
{

	public class LeavingCertificateConfig : SchoolEntity
	{

		#region -- PROPERTIES --

        [XmlIgnore]
		public int Id { get; set; }

		public string Name { get; set; }
        
		public int OriginalId { get; set; }

		[XmlIgnore]
		public string OriginalName { get; set; }

        public string SortOrder { get; set; }
        public string DefaultValue { get; set; }
        public bool IsDefaultValueApplicable { get; set; }

		#endregion -- PROPERTIES --

	}

}
