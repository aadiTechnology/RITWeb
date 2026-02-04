using DataCommunicator;

namespace BusinessLogic
{
    public class PublisherBL
    {
        #region Data members

        private PublisherDC.PublisherStructDetails moPublisherStructDetails;
        private PublisherDC moPublisherDC = new PublisherDC();

        #endregion


        public PublisherBL()
        {
            moPublisherDC.PublisherInfo = moPublisherStructDetails;            
        }
    }

}
