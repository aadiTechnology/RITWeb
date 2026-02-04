/* --------------------------------------------------------------------------
 *	FileName	: RemarksConfig.cs
 *	Author		: Vishal B. Shah
 *	Date		: 3-Dec-2011
 *	Description	: This is the entity class for Remarks Configuration screen.
 * --------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolEntities
{
	public class RemarksConfig : SchoolEntity
	{
		#region -- PROPERTIES --
		
		public int Id { get; set; }
		public string Name { get; set; }
		public int SortOrder { get; set; }
		
		#endregion -- PROPERTIES --
	}

    public class RemarksCategory : SchoolEntity
    {
        #region -- PROPERTIES --

        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        
        #endregion -- PROPERTIES --
    }

    public class RemarkTemplateConfig : SchoolEntity
    {
        public int RemarkId { get; set; }
        public string Template { get; set; }
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
    
    [Serializable]
    public class RemarkTemplateKeyword : SchoolEntity
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Male { get; set; }
        public string Female { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }

    public class RemarkTypeCategory
    {
        public int RemarkConfigId { get; set; }
        public int CategoryId { get; set; }
    }

}
