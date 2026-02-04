/* --------------------------------------------------------------------------------
 *	FileName	: ImageGallery.cs
 *	Author		: Sunny P. Chavan
 *	Date		: 23-Jan-2014
 *	Purpose		: This class is used to represent an Image gallery entity in the database.
 * --------------------------------------------------------------------------------
 */

using System;

namespace SchoolEntities
{
    [Serializable]
    public class ImageGallery : SchoolEntity
    {

        #region -- PROPERTIES --

        public int SectionId { get; set; }
        public string SectionName { get; set; } 

        #endregion -- PROPERTIES --

    }

    [Serializable]
    public class PhotoGallerySectionAssociation
    {
        public string GalleryName { get; set; }
        public string ImagePath { get; set; }
        public string Comment { get; set; }
        public int SectionId { get; set; }
        public int Id { get; set; }
        
    }
    public class StandardDivisions 
    {
        public string ClassesIds { get; set; }
        public string Standards { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int StandardDivisionId { get; set; }
        public string DivisionName { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginalDivisionId { get; set; }
        public string classes { get; set; }

    }
    
}