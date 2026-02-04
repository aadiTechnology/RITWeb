using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace SchoolTransportService.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransportService" in both code and config file together.
    [ServiceContract]
    public interface ITransportService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "SendPushNotification", Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendPushNotification(string asRFID, string asLocation, string asDateTime, string asCode);
    }
}