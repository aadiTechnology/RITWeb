using DataCommunicator.TransportDC;

namespace BusinessLogic.TransportBL
{
    public class TransportNotificationBL
    {
        private TransportNotificationDC moTransportNotificationDC;
        public TransportNotificationBL()
        {
            moTransportNotificationDC = new TransportNotificationDC();
        }

        public TransportNotificationBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moTransportNotificationDC = new TransportNotificationDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        public void CopyTransportNotification(int aiSchoolId, string asBaseDatabaseName)
        {
            moTransportNotificationDC.CopyTransportNotification(aiSchoolId, asBaseDatabaseName);
        }
    }
}
