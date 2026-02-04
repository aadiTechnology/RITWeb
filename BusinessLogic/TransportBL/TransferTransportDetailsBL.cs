using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator.TransportDC;

namespace BusinessLogic.TransportBL
{
    public class TransferTransportDetailsBL
    {
        TransferTransportDetailsDC moTransferTransportDetailsDC;

        public TransferTransportDetailsBL(int aiSchoolId, string asDBName, string asTransportDBName)
        {
            moTransferTransportDetailsDC = new TransferTransportDetailsDC(aiSchoolId, asDBName, asTransportDBName);
        }

        public void UpdateRFIDDetails(int aiUserId)
        {
            moTransferTransportDetailsDC.UpdateRFIDDetails(aiUserId);
        }

        public void UpdateJourneyDetails()
        {
            moTransferTransportDetailsDC.UpdateJourneyDetails();
        }

        public void UpdateJourneyOverrideDetails()
        {
            moTransferTransportDetailsDC.UpdateJourneyOverrideDetails();
        }

        public void UpdateBusAttendanceDetails()
        {
            moTransferTransportDetailsDC.UpdateBusAttendanceDetails();
        }

        public void UpdateAttendantRFIDDetails(string asVehicleNumber, string asAttendantRFIDRFID)
        {
            moTransferTransportDetailsDC.UpdateAttendantRFIDDetails(asVehicleNumber, asAttendantRFIDRFID);
        }
    }
}
