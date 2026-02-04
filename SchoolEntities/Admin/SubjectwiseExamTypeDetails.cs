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
    public class SubjectwiseExamTypeDetails : SchoolEntity
    {
        public int TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public int Flag { get; set; }
        public bool ConsiderExamStatus { get; set; }
        public int SortOrder { get; set; }
    }
}