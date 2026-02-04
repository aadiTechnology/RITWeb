using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using SchoolEntities.Transport;
using Utility;
using System.Linq;

namespace DataCommunicator.TransportDC
{
    public class TransportNotificationDC
    {
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        public TransportNotificationDC()
        {
        }

        public TransportNotificationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public void CopyTransportNotification(int aiSchoolId, string asBaseDatabaseName)
        {
           int iMessageId = 12;

           int iLastIndex = 0;
           string sSelectStatement = "SELECT LastIndex FROM [dbo].[LastRecordDetails] WHERE TypeId = 1 and IsDeleted = 0";
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
              iLastIndex = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
           }
           
           string connectionString = Constants.S_CONNECTION_STRING.Replace(asBaseDatabaseName, "RITeSchool");

           using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
           {
               string command = "select Id, UserId,MessageString, CreateDate from Mobile.PushNotifications where MessageId = " + iMessageId + " and schoolid = " + aiSchoolId + " and IsDeleted = 0 and Id >" + iLastIndex;

               SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
               oSqlConnection.Open();

               List<NotificationDetails> lstNotificationDetails = new List<NotificationDetails>();
               string sConnectionString = string.Empty;
               using (SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader())
               {
                   while (oSqlDataReader.Read())
                   {
                       lstNotificationDetails.Add(new NotificationDetails {
                           CreateDate = oSqlDataReader["CreateDate"].ToDateTime(),
                           MessageString = oSqlDataReader["MessageString"].ToString(),
                           UserId = oSqlDataReader["UserId"].ToInt(),
                           Id = oSqlDataReader["Id"].ToInt()
                       });
                   }
               }

               if (lstNotificationDetails.Count > 0)
               {
                   string sData = GenerateXml(lstNotificationDetails);

                   using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                   {
                       oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                       oSQLServerDbUtility.AddParameter("Details", sData, SqlDbType.Xml);
                       oSQLServerDbUtility.AddParameter("MessageId", iMessageId, SqlDbType.Int); // transport
                       oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyTransportNotification");
                   }
               }

               if (lstNotificationDetails.Count > 0)
               {
                   int iMaxIndex = 0;

                   if (DateTime.Now > Convert.ToDateTime(DateTime.Now.Date.AddHours(6)))
                       iMaxIndex = lstNotificationDetails.Max(ntd => ntd.Id);

                   string sUpdateStatement = "UPDATE [dbo].[LastRecordDetails] SET LastIndex = " + iMaxIndex + " WHERE TypeId = 1 and IsDeleted = 0";
                   using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                   {
                       oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
                   }
               }
           }
          
        }

        public string GenerateXml(Object alstGenerateXML)
        {
            var oStrwrtr = new StringWriter();
            new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
            string sXml = oStrwrtr.ToString();
            return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        }
    }
}
