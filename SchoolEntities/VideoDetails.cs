// Class Name       :- VideoDetails.cs
// Purpose          :- This class is used to manage VideoGallery details.
// Date Of creation :- 3/4/2009
// Author Name      :- Yogesh Karne
namespace SchoolEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class VideoDetails
    {
        #region PROPERTIES

        public int SchoolId { get; set; }
        public int VideoId { get; set; }
        public int VideoDetailsId { get; set; }
        public string sVideoName { get; set; }
        public string sVideoUrl { get; set; }
        public string sVideoComment { get; set; }      
        public string VideoDetailsXML { get; set; }
        public int InsertedById { get; set; }
        public int MoreVideo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserRoleIds { get; set; }
        public string StandardDivisionIds { get; set; }
        public int SubjectId { get; set; }
        public bool ShowOnExternalWebsite { get; set; }
        public int OldSubjectId { get; set; }
        public int UrlSourceId { get; set; }
        #endregion
    }

    public class SaveVideoDetails
    {
        public int VideoId { get; set; }
        public string Comment { get; set; }
        public string VideoURL { get; set; }       
    }


}
