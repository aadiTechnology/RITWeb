using System;
using System.Collections.Generic;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;

namespace BusinessLogic.TransportBL
{
   public class TransportNotificationDetailsBL
   {
       #region Data Member(s)

       private TransportNotificationDetailsDC moTransportNotificationDetailsDC = null;

       #endregion

       #region Constructor(s)

       public TransportNotificationDetailsBL()
       {
           moTransportNotificationDetailsDC = new TransportNotificationDetailsDC();
       }

       public TransportNotificationDetailsBL(int aiSchoolId, int aiAcademicYearId)
       {
           moTransportNotificationDetailsDC = new TransportNotificationDetailsDC(aiSchoolId, aiAcademicYearId);
       }

       #endregion

       #region Public Method(s)

       /// <summary>
       /// THis method is used to get transport notification details.
       /// </summary>
       /// <param name="asStudentName"></param>
       /// <param name="adStartDate"></param>
       /// <param name="adEndDate"></param>
       /// <param name="aiTypeId"></param>
       /// <param name="aiVehicleId"></param>
       /// <param name="aiRouteId"></param>
       /// <param name="aiJourneyId"></param>
       /// <returns></returns>
       public List<NotificationDetailsForScreen> GetTransportNotificationDetails(string asStudentName, DateTime adStartDate, DateTime adEndDate, int aiTypeId, string asVehicleNo, int aiRouteId, int aiJourneyId)
       {
           return moTransportNotificationDetailsDC.GetTransportNotificationDetails(asStudentName, adStartDate, adEndDate, aiTypeId, asVehicleNo, aiRouteId, aiJourneyId);
       }

       /// <summary>
       /// This method is used to get route.
       /// </summary>
       /// <returns></returns>
       public List<Route> GetRoute()
       {
           return moTransportNotificationDetailsDC.GetRoute();
       }

       /// <summary>
       /// This msthod is used to get Journey.
       /// </summary>
       /// <param name="aiRouteId"></param>
       /// <returns></returns>
       public List<JourneyDetails> GetJourney(int aiRouteId)
       {
           return moTransportNotificationDetailsDC.GetJourney(aiRouteId);
       }

       /// <summary>
       /// This msthod is used to get Vehicle numbers.
       /// </summary>
       /// <returns></returns>
       public List<VehicleDetails> GetVehicleNumber(int aiJourneyId)
       {
           return moTransportNotificationDetailsDC.GetVehicleNumber(aiJourneyId);
       }

       #endregion
   }
}
