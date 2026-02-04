// -----------------------------------------------------------------------
// <copyright file="FeedbackDetails.cs" company="RegulusIT">
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
    public class FeedbackDetails:SchoolEntity
    {
       public int LinkId { get; set; }
       public string LinkName { get; set; }
       public string FilePath { get; set; }
       public int IsSelected { get; set; }
       public string IsDeleted { get; set; }
    }

    [Serializable]
    public class FeedbackTemplate : SchoolEntity
    {
        public int FeedbackFor { get; set; }
        public int FeedbackTypeId { get; set; }
        public string Name{get;set;}
    }

    public class FeedbackType : SchoolEntity
    {
        public string Type { get; set; }
        public int Id { get; set; } 
    }

    public class UsersFeedbackDetails : SchoolEntity
    {
        public string UserName { get; set; }
        public string FeedbackDescription { get; set; }
        public string FeedbackDate { get; set; }
    }
}
