using System;
using System.Collections.Generic;
using SchoolEntities.Transport;
using System.Data.SqlClient;
using System.Data;

namespace DataCommunicator.TransportDC
{
   public class TransportNotificationDetailsDC
   {
       #region Data Member(s)

       private int miSchoolId;
       private int miAcademicYearId;

       #endregion

       #region Constructor(s)

       public TransportNotificationDetailsDC(int aiSchoolId, int aiAcademicYearId)
       {
           this.miSchoolId = aiSchoolId;
           this.miAcademicYearId = aiAcademicYearId;
       }

       public TransportNotificationDetailsDC()
       {
       }

       #endregion

       #region Public Method(s)

       /// <summary>
       /// This method is used to get transport notification details.
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
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("StudentName", asStudentName, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("StartDate", adStartDate, SqlDbType.DateTime);
               oSQLServerDbUtility.AddParameter("EndDate", adEndDate, SqlDbType.DateTime);
               oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("VehicleNo", asVehicleNo, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("RouteId", aiRouteId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("JourneyId", aiJourneyId, SqlDbType.Int);

               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportNotificationDetailsForScreen]"))
               {
                   List<NotificationDetailsForScreen> lstNotificationDetailsForScreen = new List<NotificationDetailsForScreen>();
                   while (oSqlDataReader.Read())
                   {
                       lstNotificationDetailsForScreen.Add(new NotificationDetailsForScreen
                       {
                           Id = Convert.ToInt32(oSqlDataReader["SrNo"]),
                           StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                           Standard_Name = Convert.ToString(oSqlDataReader["Standard_Name"]),
                           Division_Name = Convert.ToString(oSqlDataReader["Division_Name"]),
                           VehicleNumber = Convert.ToString(oSqlDataReader["VehicleNumber"]),
                           CreateDate = Convert.ToDateTime(oSqlDataReader["CreateDate"]),
                           MessageString = Convert.ToString(oSqlDataReader["MessageString"]),
                       });
                   }
                   return lstNotificationDetailsForScreen;
               }
           }
       }

       /// <summary>
       /// This method is used to get route.
       /// </summary>
       /// <returns></returns>
       public List<Route> GetRoute()
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);

               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetRoutes]"))
               {
                   List<Route> lstRoute = new List<Route>();
                   while (oSqlDataReader.Read())
                   {
                       lstRoute.Add
                       (
                            new Route
                            {
                                RouteId = Convert.ToInt32(oSqlDataReader["RouteId"]),
                                RouteName = Convert.ToString(oSqlDataReader["RouteName"])
                            }
                       );
                   }
                   return lstRoute;
               }
           }
       }

       /// <summary>
       /// This method is used to get journey.
       /// </summary>
       /// <param name="aiRouteId"></param>
       /// <returns></returns>
       public List<JourneyDetails> GetJourney(int aiRouteId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("RouteId", aiRouteId, SqlDbType.Int);

               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetJourney]"))
               {
                   List<JourneyDetails> lstJourneyDetails = new List<JourneyDetails>();
                   while (oSqlDataReader.Read())
                   {
                       lstJourneyDetails.Add
                           (
                               new JourneyDetails
                               {
                                   TransportShiftId = Convert.ToInt32(oSqlDataReader["TransportShiftId"]),
                                   TransportShiftName = Convert.ToString(oSqlDataReader["TransportShiftName"]),
                               }
                           );
                   }
                   return lstJourneyDetails;
               }
           }
       }

       /// <summary>
       /// This method is used to get Vehicle numbers.
       /// </summary>
       /// <returns></returns>
       public List<VehicleDetails> GetVehicleNumber(int aiJourneyId)
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {

               oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("JourneyId", aiJourneyId, SqlDbType.Int);

               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehicleNumbersAsPerJourney]"))
               {
                   List<VehicleDetails> lstVehicle = new List<VehicleDetails>();
                   while (oSqlDataReader.Read())
                   {
                       lstVehicle.Add
                           (
                               new VehicleDetails
                               {
                                   VehicleId = Convert.ToInt32(oSqlDataReader["VehicleId"]),
                                   VehicleNumber = Convert.ToString(oSqlDataReader["VehicleNumber"])
                               }
                            );
                   }
                   return lstVehicle;
               }
           }
       }

       #endregion
   }
}
