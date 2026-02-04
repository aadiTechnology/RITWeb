/*   Author		 : Yogesh
 *   Date		 : 8-10-2015
 *	 Description : This is the Entity class which is used in get academic yearwise left student Details.
 */

namespace SchoolEntities.AcademicYearwiseLeftStudentDetailsMaster
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AcademicYearwiseLeftStudentDetails
    {
        #region PROPERTIES

        public int StudentId {get; set;} 
		public string YearValue {get; set;}
		public string ClassName{get; set;} 
		public string RegNo {get; set;} 
		public string Name {get; set;} 
		public string SchoolLeftDate {get; set;}
        public int StandardId { get; set; }
        public int DivisionId { get; set; }
        public int AcademicYearId { get; set; }
        public int TotalRowCount { get; set; }
                         
        #endregion
    }
}