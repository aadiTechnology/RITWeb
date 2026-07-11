using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// Data communicator class for student mandatory details.
    /// </summary>
    public class StudentMandatoryDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentMandatoryDetailsDC"/> class.
        /// </summary>
        public StudentMandatoryDetailsDC()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentMandatoryDetailsDC"/> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        public StudentMandatoryDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUserId = aiUserId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get student mandatory details.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public StudentMandatoryDetails GetStudentMandatoryDetails(int aiYearwiseStudentId)
        {
            StudentMandatoryDetails oStudentMandatoryDetails = new StudentMandatoryDetails();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentMandatoryDetails"))
                {
                    if (oSqlDataReader != null && oSqlDataReader.Read())
                    {
                        oStudentMandatoryDetails.FatherMobileNumber = oSqlDataReader["FatherMobileNumber"].ToString();
                        oStudentMandatoryDetails.MotherMobileNumber = oSqlDataReader["MotherMobileNumber"].ToString();
                        oStudentMandatoryDetails.EmergencyContact = oSqlDataReader["EmergencyContact"].ToString();
                        oStudentMandatoryDetails.BloodGroup = oSqlDataReader["BloodGroup"].ToString();

                        oStudentMandatoryDetails.TransportMode = oSqlDataReader["TransportModeId"].ToInt();
                        oStudentMandatoryDetails.RouteNo = oSqlDataReader["RouteNo"].ToString();
                        oStudentMandatoryDetails.StopName = oSqlDataReader["StopName"].ToString();
                        oStudentMandatoryDetails.ContractorName = oSqlDataReader["ContractorName"].ToString();
                        oStudentMandatoryDetails.ContractorContactNo = oSqlDataReader["ContractorContactNo"].ToString();

                        oStudentMandatoryDetails.IsSaved = oSqlDataReader["IsSaved"].ToBool();
                        oStudentMandatoryDetails.IsSubmitted = oSqlDataReader["IsSubmitted"].ToBool();
                    }
                }
            }

            return oStudentMandatoryDetails;
        }

        /// <summary>
        /// This method is used to save student mandatory details.
        /// </summary>
        /// <param name="aoStudentMandatoryDetails"></param>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public bool SaveStudentMandatoryDetails(StudentMandatoryDetails aoStudentMandatoryDetails, int aiYearwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("FatherMobileNumber", aoStudentMandatoryDetails.FatherMobileNumber, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("MotherMobileNumber", aoStudentMandatoryDetails.MotherMobileNumber, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("EmergencyContact", aoStudentMandatoryDetails.EmergencyContact, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("BloodGroup", aoStudentMandatoryDetails.BloodGroup, SqlDbType.VarChar);

                oSQLServerDbUtility.AddParameter("TransportMode", aoStudentMandatoryDetails.TransportMode, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RouteNo", aoStudentMandatoryDetails.RouteNo, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("StopName", aoStudentMandatoryDetails.StopName, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("ContractorName", aoStudentMandatoryDetails.ContractorName, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("ContractorContactNo", aoStudentMandatoryDetails.ContractorContactNo, SqlDbType.VarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentMandatoryDetails");
            }

            return true;
        }

        /// <summary>
        /// This method is used to submit student mandatory details.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <returns></returns>
        public bool SubmitStudentMandatoryDetails(int aiYearwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentMandatoryDetails");
            }

            return true;
        }

       /// <summary>
        /// This method is used to get transport mode details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTransportModeDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTransportModeDetails");
            }
        }

        #endregion
    }
}

