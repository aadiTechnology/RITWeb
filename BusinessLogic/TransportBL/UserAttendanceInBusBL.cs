using System;
using System.Collections.Generic;
using Utility;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;

namespace BusinessLogic.TransportBL
{
   public class UserAttendanceInBusBL : BusinessLogicBaseBL
   {
       #region Data Member(s)

       private int miTotalRows;
       private UserAttendanceInBusDC moUserAttendanceInBusDC = null;

       #endregion

       #region Constructor(s)

       public UserAttendanceInBusBL()
       {
           moUserAttendanceInBusDC = new UserAttendanceInBusDC();
       }

       public UserAttendanceInBusBL(int aiSchoolId, int aiAcademicYearId)
       {
           moUserAttendanceInBusDC = new UserAttendanceInBusDC(aiSchoolId, aiAcademicYearId);
       }

       #endregion

       #region Public Method(s)

       /// <summary>
       /// This method is used to get user attendance details.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiVehicleId"></param>
       /// <param name="aiJourneyId"></param>
       /// <param name="adDate"></param>
       /// <param name="SortExpression"></param>
       /// <param name="SortDirection"></param>
       /// <param name="MaximumRows"></param>
       /// <param name="StartRowIndex"></param>
       /// <returns></returns>
       public List<UserAttendanceInBus> GetAll(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiJourneyId, DateTime adDate, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
       {
           MaximumRows = StartRowIndex + MaximumRows;
           List<UserAttendanceInBus> lstUserAttendance = moUserAttendanceInBusDC.GetAll(aiSchoolId, aiAcademicYearId, aiVehicleId, aiJourneyId, adDate, SortExpression, StartRowIndex, MaximumRows);

           if (lstUserAttendance.Count > 0)
               miTotalRows = lstUserAttendance[0].TotalRows;
           else
               miTotalRows = 0;

           return lstUserAttendance;
       }

       /// <summary>
       /// This method is used to count rows.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiVehicleId"></param>
       /// <param name="aiJourneyId"></param>
       /// <param name="adDate"></param>
       /// <param name="SortExpression"></param>
       /// <param name="SortDirection"></param>
       /// <param name="MaximumRows"></param>
       /// <param name="StartRowIndex"></param>
       /// <returns></returns>
       public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiJourneyId, DateTime adDate, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
       {
           return miTotalRows;
       }

       /// <summary>
       /// This method is used to get vehicle nos.
       /// </summary>
       /// <returns></returns>
       public List<Vehicle> GetVehicleNumber()
       {
           return moUserAttendanceInBusDC.GetVehicleNumber();
       }

       /// <summary>
       /// This method is used to get Journey.
       /// </summary>
       /// <param name="aiVehicleId"></param>
       /// <returns></returns>
       public List<Journey> GetJourney(int aiVehicleId)
       {
           return moUserAttendanceInBusDC.GetJourney(aiVehicleId);
       }

       #endregion
   }
}
