/*   Author		 : Yogesh
 *   Date		 : 15 May 2015
 *	 Description : This is the Entity class which is used in Functionality of Exam Types Screen.
 */

namespace SchoolEntities.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class YearWiseSubjectsDetails : SchoolEntity
    {
        public int SubjectId {get; set;}
        public string SubjectName {get; set;}
    }
}
