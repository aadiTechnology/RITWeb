// -----------------------------------------------------------------------
// File Name : RetirementNoticeDC.cs
// Creator : Sunny
// Created Date : 12-June-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollReportingUserEntities;
using MasterEntities;
using Utility;
namespace DataCommunicator
{

    /// <summary>
    ///This class is used to communicate with database to insert,update and select retirement notice configuration.
    /// </summary>
    public class ReportingUserConfigDC
    {
        #region Data Members 

        int miSchoolId;
        int miAcademicYearId;
        int miUserId;

        #endregion

        #region Constructors

        /// <summary>
        /// this is a default constructor.
        /// </summary>
        public ReportingUserConfigDC()
        {
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public ReportingUserConfigDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUserId = aiUserId;
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public ReportingUserConfigDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called for saving the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        /// <returns></returns>
        public void Save(ReportingUserConfiguration oReportingUserConfiguration, int aiConfigId)
        {
          
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", oReportingUserConfiguration.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingParameterId", oReportingUserConfiguration.ReportingTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", oReportingUserConfiguration.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);//reporting type id
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertReportingConfig");              
            }
        }

        /// <summary>
        /// This method is called to delete the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        public void Delete(int aiConfigId, int aiReportingTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);               
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingTypeId", aiReportingTypeId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteReportingDetails");               
            }
        }

        /// <summary>
        /// This mehotd is used to get the data for selected user.
        /// </summary>
        /// <param name="olstTempConfigDetails"></param>
        /// <returns></returns>
        public ReportingUserConfiguration Get(int aiConfigId)
        {
            ReportingUserConfiguration oReportingUserConfiguration = new ReportingUserConfiguration();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingId", aiConfigId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReportingUsers"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oReportingUserConfiguration.UserId = Convert.ToInt32(oSqlDataReader["Receiver_User_Id"]);
                        oReportingUserConfiguration.RoleId = Convert.ToInt32(oSqlDataReader["User_Role_Id"]);
                        oReportingUserConfiguration.ReportingParameterName = Convert.ToString(oSqlDataReader["ReportingParameterName"]);
                        oReportingUserConfiguration.ReportingPrameterId = Convert.ToInt32(oSqlDataReader["ReportingParameterId"]);
                    }
                }

                return oReportingUserConfiguration;
            }
        }

        /// <summary>
        /// This method is used to get all parameter. details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<ReportingUserConfiguration> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReportingUsers"))
                {
                    GenericClass<ReportingUserConfiguration> oGeneric = new GenericClass<ReportingUserConfiguration>();
                    return oGeneric.GetFilledObjectList(oSqlDataReader);
                }
            }
        }              

        /// <summary>
        /// This method is used to get all reporting parameters.
        /// </summary>
        /// <returns></returns>
        public List<ReportingParameter> GetAllReportingParameters()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReportingParameters"))
                return ReadAllReportParameters(oSqlDataReader);               
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<ReportingParameter> ReadAllReportParameters(SqlDataReader aoSqlDataReader)
        {
            List<ReportingParameter> lstReportingParameter = new List<ReportingParameter>();
            if (aoSqlDataReader != null && aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    ReportingParameter oReportingParameter = new ReportingParameter();
                    if (aoSqlDataReader["Id"] != DBNull.Value)
                        oReportingParameter.ReportingPrameterId = Convert.ToInt16(aoSqlDataReader["Id"]);
                    if (aoSqlDataReader["Name"] != DBNull.Value)
                        oReportingParameter.ReportingParameterName = Convert.ToString(aoSqlDataReader["Name"]);

                    lstReportingParameter.Add(oReportingParameter);
                }
                aoSqlDataReader.Close();
            }
            return lstReportingParameter;
        }

        #endregion
    }
}
