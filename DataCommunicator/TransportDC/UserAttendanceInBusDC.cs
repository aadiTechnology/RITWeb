using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Transport;
using Utility;

namespace DataCommunicator.TransportDC
{
    public class UserAttendanceInBusDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public UserAttendanceInBusDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public UserAttendanceInBusDC()
        {
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// THis method is used to get user attendance details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiJourneyId"></param>
        /// <param name="adDate"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<UserAttendanceInBus> GetAll(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiJourneyId, DateTime adDate, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JourneyId", aiJourneyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", adDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBusPunchingDetailsForReport"))
                {
                    List<UserAttendanceInBus> lstUserAttendance = new List<UserAttendanceInBus>();
                    while (oSqlDataReader.Read())
                    {
                        lstUserAttendance.Add(new UserAttendanceInBus
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            Standard_Name = Convert.ToString(oSqlDataReader["Standard_Name"]),
                            Division_Name = Convert.ToString(oSqlDataReader["Division_Name"]),
                            RouteName = Convert.ToString(oSqlDataReader["RouteName"]),
                            JourneyName = Convert.ToString(oSqlDataReader["JourneyName"]),
                            VehicleNo = Convert.ToString(oSqlDataReader["VehicleNo"]),
                            JourneyType = Convert.ToString(oSqlDataReader["JourneyType"]),
                            PunchingDateTime = (oSqlDataReader["PunchingDateTime"] == DBNull.Value?"-" : Convert.ToDateTime(oSqlDataReader["PunchingDateTime"]).ToString("hh:mm tt")),
                            Location = Convert.ToString(oSqlDataReader["Location"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]),
                            LocationURL = Convert.ToString(oSqlDataReader["LocationURL"]),
                            IsOnBoardingNotificationSent = Convert.ToString(oSqlDataReader["IsOnBoardingNotificationSent"]),
                            IsGeofenceNotificationSent = Convert.ToString(oSqlDataReader["IsGeofenceNotificationSent"]),
                            IsOffBoardingNotificationSent = Convert.ToString(oSqlDataReader["IsOffBoardingNotificationSent"]),
                            Comment = Convert.ToString(oSqlDataReader["Comment"]),
                            IsJourneyChanged = oSqlDataReader["IsJourneyChanged"].ToBool(),
                            IsVehicleChanged = oSqlDataReader["IsVehicleChanged"].ToBool()
                        });
                    }
                    return lstUserAttendance;
                }
            }
        }

        /// <summary>
        /// This method is used to get vehicle nos.
        /// </summary>
        /// <returns></returns>
        public List<Vehicle> GetVehicleNumber()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetVehicleNumber"))
                {
                    List<Vehicle> lstVehicle = new List<Vehicle>();
                    while (oSqlDataReader.Read())
                    {
                        lstVehicle.Add
                            (
                                new Vehicle
                                {
                                    Value_Member = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                    Display_Member = Convert.ToString(oSqlDataReader["Display_Member"])
                                }
                             );
                    }
                    return lstVehicle;
                }
            }
        }

        /// <summary>
        /// This method is used to get journey.
        /// </summary>
        /// <param name="aiVehicleId"></param>
        /// <returns></returns>
        public List<Journey> GetJourney(int aiVehicleId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetJourney"))
                {
                    List<Journey> lstJourney = new List<Journey>();
                    while (oSqlDataReader.Read())
                    {
                        lstJourney.Add
                            (
                                new Journey
                                {
                                    Value_Member = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                    Display_Member = Convert.ToString(oSqlDataReader["Display_Member"])
                                }
                            );
                    }
                    return lstJourney;
                }
            }
        }

        #endregion
    }
}
