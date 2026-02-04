using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DataCommunicator
{
    public class UsersPunchingDetailsDC : DataCommunicatorBaseDC
    {
        #region structure

        public struct UsersPunchingDetailsStruct
        {
            public int miSchoolId;
            public int miUserId;
            public int miAcademicYearId;
        }
        UsersPunchingDetailsStruct moUsersPunchingDetailsStruct;
        #endregion

        #region DataMembers and properties

        public UsersPunchingDetailsStruct usersPunchingDetailsStruct
        {

            get { return moUsersPunchingDetailsStruct; }
            set { moUsersPunchingDetailsStruct = value; }
        }

        #endregion

        /// <summary>
        /// This method is used to get connectionstring of CosecPP database.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        private static string GetConnectionstring(int aiSchoolId)
        {
            string sConncetionString = null;
            string sBiometricConncetionString = null;
            string SchoolLocationsDataSource = null;
            string SchoolLocationsDatabase = null;
            string SchoolLocationsUserId = null;
            string SchoolLocationsPassword = null;

            SchoolLocationsDataSource = ConfigurationManager.AppSettings["SchoolLocationsDataSource"];
            SchoolLocationsDatabase = ConfigurationManager.AppSettings["SchoolLocationsDatabase"];
            SchoolLocationsUserId = ConfigurationManager.AppSettings["SchoolLocationsUserId"];
            SchoolLocationsPassword = ConfigurationManager.AppSettings["SchoolLocationsPassword"];

            sConncetionString = "Data Source= " + SchoolLocationsDataSource + "; Database=" + SchoolLocationsDatabase + "; User ID=" + SchoolLocationsUserId + "; Password=" + SchoolLocationsPassword;

            using (SqlConnection oSqlConnection = new SqlConnection(sConncetionString))
            {
                string command = "SELECT BiometricUserID, BiometricPassword, BiometricDatabaseName,  BiometricDatabaseServer FROM BiometricSchools WHERE SchoolID = " + aiSchoolId + " AND IsDeleted = 0";
                
                SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                oSqlConnection.Open();

                SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader();
                while (oSqlDataReader.Read())
                {
                    string password = CommonUtility.GetDecryptedPassword((oSqlDataReader["BiometricUserID"]).ToString(), (oSqlDataReader["BiometricPassword"]).ToString());

                    sBiometricConncetionString = "Data Source= " + (oSqlDataReader["BiometricDatabaseServer"]).ToString() 
                        + "; Database=" + (oSqlDataReader["BiometricDatabaseName"]).ToString()
                        + "; User ID=" + (oSqlDataReader["BiometricUserID"]).ToString() + "; Password=" + password;
                }
                oSqlConnection.Close();
             }

            return sBiometricConncetionString;
        }

        /// <summary>
        /// Thsi method is used to get all users who have punched.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllUsersPunched(int aiSchoolId, String sSortExpression, int iEndIndex, int startRowIndex, string asSelectedDate, bool abChkGroupByUser)
        {
            string connection = GetConnectionstring(aiSchoolId);

            DateTime selectedDate = Convert.ToDateTime(asSelectedDate);
            using (SqlConnection oSqlConnection = new SqlConnection(connection))
            {
                string sEntitiesDetails = null;
                
                if (!abChkGroupByUser)
                {
                    sEntitiesDetails = "SELECT Employee_No, UserName, STUFF(RIGHT('0'+ LTRIM(RIGHT(CONVERT(varchar(8),EventDateTime,100),7)),7),6,0, ' ') as  EventDateTime, IndexNo, TotalRows " +
                                            " FROM (SELECT Employee_No,  " +
                                            " UserName , CAST (EventDateTime AS Time) as EventDateTime, IndexNo, ROW_NUMBER() OVER (Order By  " + sSortExpression + " ) as RowNo , TotalRows " +
                                            " FROM ( SELECT dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE AS Employee_No " +
                                            " ,dbo.Mx_VEW_APIUserAccessCtrlEvts.UserName AS UserName, " +
                                            " dbo.Mx_VEW_APIUserAccessCtrlEvts.EventDateTime_D AS EventDateTime, " +
                                            " dbo.Mx_VEW_APIUserAccessCtrlEvts.IndexNo AS IndexNo, " +
                                            " COUNT(1) OVER() AS TotalRows " +
                                            " FROM dbo.Mx_VEW_APIUserAccessCtrlEvts " +
                                            " WHERE   (dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE != NULL OR dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE != '')" +
                                            " AND CONVERT(Date, dbo.Mx_VEW_APIUserAccessCtrlEvts.EventDateTime_D) = " + "CONVERT(Date," + "'" + selectedDate + "')" + ") as A" +
                                            ") AS b" +
                                            " WHERE RowNo > " + "CAST( " + startRowIndex + " as NVARCHAR)" + " AND RowNo <= " + "CAST( " + iEndIndex + "as NVARCHAR)";
                }

                else
                {
                    if (sSortExpression == "IndexNo")
                        sSortExpression = "Employee_No";
                    else 
                        sSortExpression = "Employee_No, "+ sSortExpression;

                    sEntitiesDetails = "SELECT Employee_No, UserName, STUFF(RIGHT('0'+ LTRIM(RIGHT(CONVERT(varchar(8),EventDateTime,100),7)),7),6,0, ' ') as  EventDateTime, IndexNo, TotalRows " +
                                            " FROM (SELECT Employee_No, " +
                                            " UserName, CAST (EventDateTime AS Time) as EventDateTime, IndexNo, ROW_NUMBER() OVER (Order By " + sSortExpression + " ) as RowNo , TotalRows " +
                                            " FROM (SELECT dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE AS Employee_No " +
                                            " ,dbo.Mx_VEW_APIUserAccessCtrlEvts.UserName AS UserName, " +
                                            " dbo.Mx_VEW_APIUserAccessCtrlEvts.EventDateTime_D AS EventDateTime, " +
                                            " dbo.Mx_VEW_APIUserAccessCtrlEvts.IndexNo AS IndexNo, " +
                                            " COUNT(1) OVER() AS TotalRows " +
                                            " FROM dbo.Mx_VEW_APIUserAccessCtrlEvts " +
                                            " WHERE (dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE != NULL OR dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE != '')" +
                                            " AND CONVERT(Date, dbo.Mx_VEW_APIUserAccessCtrlEvts.EventDateTime_D) = " + "CONVERT(Date," + "'" + selectedDate + "')" + ") as A" +
                                            ") AS b" +
                                            " WHERE RowNo > " + "CAST( " + startRowIndex + " as NVARCHAR)" + " AND RowNo <= " + "CAST( " + iEndIndex + "as NVARCHAR)";
                }

                SqlCommand oSqlCommand = new SqlCommand(sEntitiesDetails, oSqlConnection);
                oSqlConnection.Open();

                var oDataTable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(oSqlCommand);

                da.Fill(oDataTable);

                oSqlConnection.Close();
                return oDataTable;
            }
        }

        /// <summary>
        /// This method is used to get all the users who haven't punched.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllUsersNotPunched(int aiSchoolId, String sSortExpression, int iEndIndex, int startRowIndex, string asSelectedDate)
        {
             string connection = GetConnectionstring(aiSchoolId);
             using (SqlConnection oSqlConnection = new SqlConnection(connection))
             {
                 string sEntitiesDetails = null;

                 DateTime selectedDate = Convert.ToDateTime(asSelectedDate);
                
                 sEntitiesDetails = "  SELECT DISTINCT Employee_No, UserName, TotalRows as  TotalRows " +
                                           " FROM (SELECT DISTINCT Employee_No,  " +
                                           " UserName, ROW_NUMBER() OVER (Order By  " + sSortExpression + " ) as RowNo , TotalRows " +
                                           " FROM ( SELECT DISTINCT dbo.Mx_UserMst.UserID AS Employee_No " +
                                           " ,dbo.Mx_UserMst.Name AS UserName, " +
                                           " COUNT(1) OVER() AS TotalRows " +
                                           " FROM dbo.Mx_UserMst " +
                                           " WHERE (dbo.Mx_UserMst.UserID != NULL OR dbo.Mx_UserMst.UserID != '')" +
                                           " AND dbo.Mx_UserMst.UserID NOT IN ( SELECT distinct dbo.Mx_VEW_APIUserAccessCtrlEvts.INTEGRATION_REFERENCE FROM Mx_VEW_APIUserAccessCtrlEvts" +
                                           " WHERE CONVERT(Date, dbo.Mx_VEW_APIUserAccessCtrlEvts.EventDateTime_D) = " + "CONVERT(Date," + "'" + selectedDate + "'))" + 
                                           ") as A" +
                                           ") AS b" +
                                           " WHERE RowNo > " + "CAST( " + startRowIndex + " as NVARCHAR)" + " AND RowNo <= " + "CAST( " + iEndIndex + "as NVARCHAR)";


                 SqlCommand oSqlCommand = new SqlCommand(sEntitiesDetails, oSqlConnection);
                 oSqlConnection.Open();

                 var oDataTable = new DataTable();
                 SqlDataAdapter da = new SqlDataAdapter(oSqlCommand);
                 
                 da.Fill(oDataTable);
                 oSqlConnection.Close();
                 
                 return oDataTable;
             }
        }
    }
}
