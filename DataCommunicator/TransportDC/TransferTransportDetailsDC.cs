using SchoolEntities.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utility;

namespace DataCommunicator.TransportDC
{
    public class TransferTransportDetailsDC
    {
        string S_TRANSPORT_CONNECTION_STRING = "";

        public TransferTransportDetailsDC(int aiSchoolId, string asDBName, string asTransportDBName)
        {
            S_TRANSPORT_CONNECTION_STRING = Constants.S_CONNECTION_STRING.Replace(asDBName, asTransportDBName);
        }

        public bool AllowTransportDataUpdation
        {
            get { return !string.IsNullOrEmpty(S_TRANSPORT_CONNECTION_STRING); }
        }

        public void UpdateRFIDDetails(int aiUserId)
        {
            if (AllowTransportDataUpdation)
            {
                string sRFIDDetails = string.Empty;
                using (SQLServerDbUtility OSQLServerDbUtility = new SQLServerDbUtility())
                {
                    OSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = OSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRFIDOfUser"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            sRFIDDetails = oSqlDataReader["RFIDDetails"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sRFIDDetails))
                {
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(S_TRANSPORT_CONNECTION_STRING))
                    {
                        oSQLServerDbUtility.AddParameter("RFIDDetails", sRFIDDetails, SqlDbType.NVarChar);
                        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateRFID");
                    }
                }
            }
        }

        public void UpdateJourneyDetails()
        {
            if (AllowTransportDataUpdation)
            {
                string sJourenyDetails = string.Empty;
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetUpdatedJourneyDetails]"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            sJourenyDetails = oSqlDataReader["JourneyDetails"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sJourenyDetails))
                {
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(S_TRANSPORT_CONNECTION_STRING))
                    {
                        oSQLServerDbUtility.AddParameter("JourenyDetails", sJourenyDetails, SqlDbType.Xml);
                        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_UpdateJourneyDetails]");
                    }
                }
            }
        }

        public void UpdateJourneyOverrideDetails()
        {
            if (AllowTransportDataUpdation)
            {
                string sJourneyOverrideDetails = string.Empty;
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetJourneyOverrideDetailsToTransfer]"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            sJourneyOverrideDetails = oSqlDataReader["JourneyOverrideDetails"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sJourneyOverrideDetails))
                {
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(S_TRANSPORT_CONNECTION_STRING))
                    {
                        oSQLServerDbUtility.AddParameter("JourneyOverrideDetails", sJourneyOverrideDetails, SqlDbType.Xml);
                        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_UpdateJourneyOverrideDetails]");
                    }
                }
            }
        }

        public void UpdateBusAttendanceDetails()
        {
            if (AllowTransportDataUpdation)
            {
                int iLastId = 0;
                string sStatement = "select ISNULL(MAX(Id),0) from Transport.BusPunchingDetails";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    iLastId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);

                string asBusPunchingDetails = string.Empty;
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(S_TRANSPORT_CONNECTION_STRING))
                {
                    oSQLServerDbUtility.AddParameter("LastId", iLastId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetLastBusPunchingDetails]"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            asBusPunchingDetails = oSqlDataReader["BusPunchingDetails"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(asBusPunchingDetails))
                {
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    {
                        oSQLServerDbUtility.AddParameter("BusPunchingDetails", asBusPunchingDetails, SqlDbType.Xml);
                        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_CopyBusPunchingDetails]");
                    }
                }
            }
        }

        public void UpdateAttendantRFIDDetails(string asVehicleNumber, string asAttendantRFIDRFID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(S_TRANSPORT_CONNECTION_STRING))
            {
                oSQLServerDbUtility.AddParameter("VehicleNumber", asVehicleNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AttendantRFID", asAttendantRFIDRFID, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_UpdateAttendantRFID]");
            }
        }
    }
}
