using System.Configuration;
using BusinessLogic.TransportBL;

namespace SchoolTransportService.Service
{
    public class TransportService : ITransportService
    {
        public string SendPushNotification(string asRFID, string asLocation, string asDateTime, string asCode)
        {
            if (ConfigurationManager.AppSettings["TransportAPICode"] != null && ConfigurationManager.AppSettings["TransportAPICode"].ToString() == asCode)
            {
                TransportServiceBL oTransportServiceBL = new TransportServiceBL();
                return oTransportServiceBL.SendPushNotification(asRFID, asLocation, asDateTime, asCode);
            }
            else
            {
                return "Invalid code.";
            }
        }
    }
}