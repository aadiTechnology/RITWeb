using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoUploadEntities
{
    [Serializable]
    public class UserRolewisePhotoDetails
    {
        public int RowNo { get; set; }
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PhotoFilePath { get; set; }
        public bool RemovePhoto { get; set; }
        public byte[] BinaryPhotoImage { get; set; }
        public string ClassName { get; set; }
        public string UserRoleName { get; set; }
    }

    [Serializable]
	public class ImageData
	{
		public int UserID { get; set; }
		public byte[] ImagesData { get; set; }
	}

    public class UserRolewiseDocumentDetails
    {   
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DocumentFilePath { get; set; }                
        public string UserRoleName { get; set; }
        public int RowNo { get; set; }
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string Year { get; set; }
        public string PanNo { get; set; }
        public string EmployeeNo { get; set; }
    }
}
