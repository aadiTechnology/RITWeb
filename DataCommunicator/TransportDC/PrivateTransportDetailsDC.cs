using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class PrivateTransportDetailsDC
    {


        public class PrivateTransportDetails
        {
            public int PrivateTransportDetailsId
            { get; set; }
            public int UserId
            { get; set; }
            public string UserName
            { get; set; }
            public string StopName
            { get; set; }
            public string VehicleNumber
            { get; set; }
            public string VehicleType
            { get; set; }
            public string TransportStaff1
            { get; set; }
            public string MobileNo1
            { get; set; }
            public string TransportStaff2
            { get; set; }
            public string MobileNo2
            { get; set; }
            public int SchoolId
            { get; set; }
            public int AcademicYearId
            { get; set; }
            public int Is_Deleted
            { get; set; }
            public int InsertedById
            { get; set; }
        }

        private PrivateTransportDetails moPrivateTransportDetails = new PrivateTransportDetails();

        public PrivateTransportDetailsDC()
        {
        }

        public PrivateTransportDetailsDC(int aiPrivateTransportDetailsId, int aiSchoolId, int aiAcademicYearId)
        {
            LoadPrivateTransportDetails(aiPrivateTransportDetailsId, aiSchoolId, aiAcademicYearId);
        }

        public PrivateTransportDetails TransportDetails
        {
            get
            {
                return moPrivateTransportDetails;
            }
            set
            {
                moPrivateTransportDetails = value;
            }
        }

        private void LoadPrivateTransportDetails(int aiPrivateTransportDetailsId, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                //List<SqlParameter> olstSqlParameter = new List<SqlParameter>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int );
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int );
                oSQLServerDbUtility.AddParameter("PrivateTransportDetailsId", aiPrivateTransportDetailsId, SqlDbType.Int );
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetPrivateTransportForUpdate]"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["PrivateTransportDetailsId"] != DBNull.Value)
                                moPrivateTransportDetails.PrivateTransportDetailsId = Convert.ToInt32(oDR["PrivateTransportDetailsId"]);
                            if (oDR["UserId"] != DBNull.Value)
                                moPrivateTransportDetails.UserId = Convert.ToInt32(oDR["UserId"]);
                            if (oDR["UserName"] != DBNull.Value)
                                moPrivateTransportDetails.UserName = Convert.ToString(oDR["UserName"]);
                            if (oDR["StopName"] != DBNull.Value)
                                moPrivateTransportDetails.StopName = Convert.ToString(oDR["StopName"]);
                            if (oDR["VehicleNumber"] != DBNull.Value)
                                moPrivateTransportDetails.VehicleNumber = Convert.ToString(oDR["VehicleNumber"]);
                            if (oDR["VehicleType"] != DBNull.Value)
                                moPrivateTransportDetails.VehicleType = Convert.ToString(oDR["VehicleType"]);
                            if (oDR["TransportStaff1"] != DBNull.Value)
                                moPrivateTransportDetails.TransportStaff1 = Convert.ToString(oDR["TransportStaff1"]);
                            if (oDR["TransportStaff2"] != DBNull.Value)
                                moPrivateTransportDetails.TransportStaff2 = Convert.ToString(oDR["TransportStaff2"]);
                            if (oDR["MobileNo1"] != DBNull.Value)
                                moPrivateTransportDetails.MobileNo1 = Convert.ToString(oDR["MobileNo1"]);
                            if (oDR["MobileNo2"] != DBNull.Value)
                                moPrivateTransportDetails.MobileNo2 = Convert.ToString(oDR["MobileNo2"]);

                        }
                    }
                }
            }
        }

        public static List<PrivateTransportDetails> GetTravelersList(int aiSchoolId, int aiAcademicYearId, int aiStandardId,
                                                                     int aiDivisonId, string asUserName, string sortExpression, int iEndIndex,
                                                                     int startRowIndex)
        {

            if (sortExpression == string.Empty)
                sortExpression = "UserName";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                List<PrivateTransportDetails> olstPrivateTransportDetails = new List<PrivateTransportDetails>();
                //List<SqlParameter> olstSqlParameter = new List<SqlParameter>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                if (aiStandardId != 0 && aiDivisonId != 0)
                {
                    oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("DivisionId", aiDivisonId, SqlDbType.Int);
                }
                if (asUserName != "0")
                    oSQLServerDbUtility.AddParameter("UserName", asUserName, SqlDbType.NVarChar);

                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetPrivateTransportDetails]"))
                {
                    PrivateTransportDetails oPrivateTransportDetails;
                    while (oSqlDataReader.Read())
                    {
                        oPrivateTransportDetails = new PrivateTransportDetails
                        {
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            PrivateTransportDetailsId = Convert.ToInt32(oSqlDataReader["PrivateTransportDetailsId"]),
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),

                        };
                        olstPrivateTransportDetails.Add(oPrivateTransportDetails);
                    }
                    return olstPrivateTransportDetails;
                }
            }
        }

        public static int GetTravelersListCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisonId, string asUserName, string sortExpression, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                if (aiStandardId != 0 && aiDivisonId != 0)
                {
                    oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("DivisionId", aiDivisonId, SqlDbType.Int);
                }
                if (asUserName != "0")
                    oSQLServerDbUtility.AddParameter("UserName", asUserName, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_GetPrivateTransportCount]");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public void Insert()
        {
            string sInsertStatement = "INSERT INTO " +
                                            " Transport.PrivateTransportDetails(" +
                                            " UserId," +
                                            " StopName," +
                                            " VehicleNumber," +
                                            " VehicleType," +
                                            " TransportStaff1," +
                                            " MobileNo1," +
                                            " TransportStaff2," +
                                            " MobileNo2," +
                                            " SchoolId," +
                                            " AcademicYearId," +
                                            " InsertedById," +
                                            " UpdateById )" +
                                      " VALUES (" +
                                                 moPrivateTransportDetails.UserId +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.StopName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.VehicleNumber, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.VehicleType, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.TransportStaff1, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.MobileNo1, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.TransportStaff2, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.MobileNo2, true) + "'" +
                                               " , " + moPrivateTransportDetails.SchoolId + " " +
                                               " , " + moPrivateTransportDetails.AcademicYearId + " " +
                                               " , " + moPrivateTransportDetails.InsertedById + " " +
                                               " , " + moPrivateTransportDetails.InsertedById + " " +
                                        " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);

        }

        public void Update()
        {
            string sUpdateStatement = "UPDATE  " +
                                            " Transport.PrivateTransportDetails " +
                                      " SET " +

                                            " StopName= N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.StopName, true) + "'" +
                                            " ,VehicleNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.VehicleNumber, true) + "'" +
                                            " ,VehicleType= N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.VehicleType, true) + "'" +
                                            " ,TransportStaff1=N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.TransportStaff1, true) + "'" +
                                            " ,MobileNo1=N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.MobileNo1, true) + "'" +
                                            " ,TransportStaff2=N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.TransportStaff2, true) + "'" +
                                            " ,MobileNo2=N'" + StringUtility.ReplaceSingleQuoteInString(moPrivateTransportDetails.MobileNo2, true) + "'" +
                                            " ,InsertedById=" + moPrivateTransportDetails.InsertedById + " " +
                                            " ,UpdateById =" + moPrivateTransportDetails.InsertedById + " " +
                                    " WHERE PrivateTransportDetailsId=" + moPrivateTransportDetails.PrivateTransportDetailsId +
                                    " AND " +
                                    " SchoolId=" + moPrivateTransportDetails.SchoolId +
                                    " AND " +
                                    " AcademicYearId=" + moPrivateTransportDetails.AcademicYearId +
                                    " AND Is_Deleted=0";


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        public static void Delete(int iPrivateTransportDetailsId)
        {
            string sDeleteStatement = "DELETE FROM  " +
                                      " Transport.PrivateTransportDetails " +
                                      " WHERE PrivateTransportDetailsId=" + iPrivateTransportDetailsId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }
    }
}
