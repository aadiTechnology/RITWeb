using DataCommunicator.TransportDC;

namespace BusinessLogic.TransportBL
{
    public class TransportServiceBL
    {
        TransportServiceDC moTransportServiceDC;
        public TransportServiceBL()
        {
            moTransportServiceDC = new TransportServiceDC();
        }

        public string SendPushNotification(string asRFID, string asLocation, string asDateTime, string asCode)
        {
            return moTransportServiceDC.SendPushNotification(asRFID, asLocation, asDateTime, asCode);
        }
    }
}
