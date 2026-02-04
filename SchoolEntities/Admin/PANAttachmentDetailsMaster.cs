/*   Author		 : Yogesh
 *   Date		 : 1-9-2014
 *	 Description : This is the Entity class which is used in funtionality of PAN Attachment Details.
 */

namespace SchoolEntities.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
   
    public class PANAttachmentDetails
    {
        #region PROPERTIES
        public int UserId { get; set; }

        public string PanNo { get; set; }

        public string Name { get; set; }

        public string PanAttachment { get; set; }

        public string NameonAadharCard { get; set; }
        public int RollNo { get; set; }
        public string ClassName { get; set; }
        #endregion
    }
}
