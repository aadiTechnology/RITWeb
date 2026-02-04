using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator.TransportDC
{
    public class TransportServiceDC
    {
        public TransportServiceDC()
        {
        }

        public string SendPushNotification(string asRFID, string asLocation, string asDateTime, string asCode)
        {
            DataTable dt = new DataTable();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("RFID", asRFID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Location", asLocation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PunchingDateTime", asDateTime, SqlDbType.NVarChar);                
                dt = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[Transport].[usp_AddTransportNotificationDetails]");
            }

            if (dt.Rows.Count > 0)
            {
                string connectionString = "Data Source= " + ConfigurationManager.AppSettings["ReportingDataSource"] + "; Database=" + "RITeSchool"
                                + "; User ID=" + ConfigurationManager.AppSettings["ReportingUserId"] + "; Password=" + ConfigurationManager.AppSettings["ReportingPassword"];

                using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
                {
                    DataRow dr = dt.Rows[0];
                    string command = "INSERT INTO mobile.PushNotifications SELECT " + dr["UserId"].ToString() + "," + dr["SchoolId"].ToString() + ",'" + dr["MessageString"].ToString() + "'," + dr["MessageId"].ToString() + ",1,dbo.getLocalDate(default),0,0";

                    SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                    oSqlConnection.Open();

                    oSqlCommand.ExecuteNonQuery();
                }

                return "Welcome in Transport Service";
            }
            else
                return "RFID doesn't match.";
        }
    }
}
