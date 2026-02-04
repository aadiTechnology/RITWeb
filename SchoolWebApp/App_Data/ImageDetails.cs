using System.Web.UI;
using PhotoUploadEntities;

namespace SchoolWebApp
{
    public class ImageDetails : Page
    {
        const string S_IMAGE_DATA = "IMAGE_DATA";

        public void SetData(ImageData aoImageData)
        {
            Session[S_IMAGE_DATA] = aoImageData;
        }

        public ImageData GetData()
        {
            return Session[S_IMAGE_DATA] as ImageData;
        }

        public void ClearImage()
        {
            Session[S_IMAGE_DATA] = null;
        }
    }
}