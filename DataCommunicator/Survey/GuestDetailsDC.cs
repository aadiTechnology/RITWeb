using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.Survey;
using System.Data;
using System.Data.SqlClient;
namespace DataCommunicator.Survey
{
    public class GuestDetailsDC
    {

        #region DataMember(s)

        private int iSchoolId, iAcademicYearId, iUserId;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GuestDetailsDC()
        {
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        public GuestDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.iSchoolId = aiSchoolId;
            this.iAcademicYearId = aiAcademicYearId;
            this.iUserId = aiUserId;
        }

        #endregion

        #region Method(S)

        /// <summary>
        /// This method is used to get reference guest name.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public List<GuestReferenceDetails> GetReferenceGuestName()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", iAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetReferenceGuestName"))
                    return this.ReadReferenceGuestName(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to read reference guest name.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<GuestReferenceDetails> ReadReferenceGuestName(SqlDataReader aoSqlDataReader)
        {
            List<GuestReferenceDetails> lstGuestReferenceDetails = new List<GuestReferenceDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    GuestReferenceDetails oGuestReferenceDetails = new GuestReferenceDetails();
                    if (aoSqlDataReader["Guest_Id"] != DBNull.Value)
                        oGuestReferenceDetails.GuestId = Convert.ToInt32(aoSqlDataReader["Guest_Id"]);
                    if (aoSqlDataReader["FullName"] != DBNull.Value)
                        oGuestReferenceDetails.GuestFullName = aoSqlDataReader["FullName"].ToString();
                    lstGuestReferenceDetails.Add(oGuestReferenceDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstGuestReferenceDetails;
        }

        /// <summary>
        /// This method is used to save guest details.
        /// </summary>
        /// <param name="moGuestDetails"></param>
        public void Save(GuestDetails moGuestDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Salutation_Id", moGuestDetails.SalutationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FirstName", moGuestDetails.FirstName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MiddleName", moGuestDetails.MiddleName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("LastName", moGuestDetails.LastName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNumber", moGuestDetails.MobileNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Area", moGuestDetails.Area, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsSentSMS", moGuestDetails.IsSendSMS, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ReferenceGuestId", moGuestDetails.ReferenceGuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.iUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveGuestDetails");
            }
        }

        /// <summary>
        /// This method is used to get all Guest Details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<GuestDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string sortExpression, int iStartIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllGuestDetails"))
                    return this.ReadAllGuestDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to read all guest Details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<GuestDetails> ReadAllGuestDetails(SqlDataReader aoSqlDataReader)
        {
            List<GuestDetails> lstGuestDetails = new List<GuestDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    GuestDetails oGuestDetails = new GuestDetails();
                    if (aoSqlDataReader["Guest_Id"] != DBNull.Value)
                        oGuestDetails.GuestId = Convert.ToInt32(aoSqlDataReader["Guest_Id"]);
                    if (aoSqlDataReader["FullName"] != DBNull.Value)
                        oGuestDetails.FullName = Convert.ToString(aoSqlDataReader["FullName"]);
                    if (aoSqlDataReader["MobileNumber"] != DBNull.Value)
                        oGuestDetails.MobileNumber = Convert.ToString(aoSqlDataReader["MobileNumber"]);
                    if (aoSqlDataReader["Area"] != DBNull.Value)
                        oGuestDetails.Area = Convert.ToString(aoSqlDataReader["Area"]);
                    if (aoSqlDataReader["IsSentSMS"] != DBNull.Value)
                        oGuestDetails.IsSendSMS = Convert.ToBoolean(aoSqlDataReader["IsSentSMS"]);
                    if (aoSqlDataReader["ReferenceGuestName"] != DBNull.Value)
                        oGuestDetails.ReferenceGuestFullName = Convert.ToString(aoSqlDataReader["ReferenceGuestName"]);
                    lstGuestDetails.Add(oGuestDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstGuestDetails;
        }

        /// <summary>
        /// This method is used to count number of records.
        /// </summary>
        /// <param name="aiItemId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountOfgGuestDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get Guest Deatils.
        /// </summary>
        /// <param name="aiGuestId"></param>
        /// <returns></returns>
        public GuestDetails Get(int aiGuestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GuestId", aiGuestId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGuestDetails"))
                    return this.ReadGuestDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to read guest details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private GuestDetails ReadGuestDetails(SqlDataReader aoSqlDataReader)
        {
            GuestDetails oGuestDetails = new GuestDetails();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    if (aoSqlDataReader["Salutation_Id"] != DBNull.Value)
                        oGuestDetails.SalutationId = Convert.ToInt32(aoSqlDataReader["Salutation_Id"]);
                    if (aoSqlDataReader["FirstName"] != DBNull.Value)
                        oGuestDetails.FirstName = Convert.ToString(aoSqlDataReader["FirstName"]);
                    if (aoSqlDataReader["MiddleName"] != DBNull.Value)
                        oGuestDetails.MiddleName = Convert.ToString(aoSqlDataReader["MiddleName"]);
                    if (aoSqlDataReader["LastName"] != DBNull.Value)
                        oGuestDetails.LastName = Convert.ToString(aoSqlDataReader["LastName"]);
                    if (aoSqlDataReader["MobileNumber"] != DBNull.Value)
                        oGuestDetails.MobileNumber = Convert.ToString(aoSqlDataReader["MobileNumber"]);
                    if (aoSqlDataReader["Area"] != DBNull.Value)
                        oGuestDetails.Area = Convert.ToString(aoSqlDataReader["Area"]);
                    if (aoSqlDataReader["IsSentSMS"] != DBNull.Value)
                        oGuestDetails.IsSendSMS = Convert.ToBoolean(aoSqlDataReader["IsSentSMS"]);
                    if (aoSqlDataReader["ReferenceGuestId"] != DBNull.Value)
                        oGuestDetails.ReferenceGuestId = Convert.ToInt32(aoSqlDataReader["ReferenceGuestId"]);
                }
                aoSqlDataReader.Close();
                return oGuestDetails;
            }
            return oGuestDetails;
        }

        /// <summary>
        /// This method is used to update guest details.
        /// </summary>
        /// <param name="aiGuestId"></param>
        /// <param name="moGuestDetails"></param>
        public void Update(int aiGuestId, GuestDetails moGuestDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("GuestId", aiGuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Salutation_Id", moGuestDetails.SalutationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FirstName", moGuestDetails.FirstName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MiddleName", moGuestDetails.MiddleName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("LastName", moGuestDetails.LastName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNumber", moGuestDetails.MobileNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Area", moGuestDetails.Area, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsSentSMS", moGuestDetails.IsSendSMS, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ReferenceGuestId", moGuestDetails.ReferenceGuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.iUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateGuestDetails");
            }
        }

        /// <summary>
        /// This method is used to delete guest details.
        /// </summary>
        /// <param name="aiGuestId"></param>
        public void Delete(int aiGuestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("GuestId", aiGuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.iUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteGuestDetails");
            }
        }

        #endregion

    }
}
