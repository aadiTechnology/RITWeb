// -----------------------------------------------------------------------
// <copyright file="QualificatoinDetails.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
   

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualificatoinDetails
    {
       

        public int QualificationId {get; set;}
        public string Qualification {get; set;}
        public int IsUsedByTeacher { get; set; }
    }
}
