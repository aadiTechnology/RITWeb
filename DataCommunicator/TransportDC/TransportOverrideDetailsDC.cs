using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using SchoolEntities.Transport;
using Utility;

namespace DataCommunicator.TransportDC
{
    public class TransportOverrideDetailsDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public TransportOverrideDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public TransportOverrideDetailsDC()
        {
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to delete transport override details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteTransportOverrideDetails]");
            }
        }

        /// <summary>
        /// This method is used to get transport override details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRouteNo"></param>
        /// <param name="asRouteName"></param>
        /// <param name="asVehicleNo"></param>
        /// <param name="asJourneyName"></param>
        /// <param name="asStudentName"></param>
        /// <param name="asStudentRegNo"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<OverrideDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asStudentName, string asStudentRegNo, string asOverrideName, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RouteNo", asRouteNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RouteName", asRouteName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("VehicleNo", asVehicleNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("JourneyName", asJourneyName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentName", asStudentName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentRegNo", asStudentRegNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OverrideName", asOverrideName, SqlDbType.NVarChar);                

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetAllTransportOverrideDetails]"))
                {
                    List<OverrideDetails> lstOverrideDetails = new List<OverrideDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstOverrideDetails.Add(new OverrideDetails
                        {
                            Name = Convert.ToString(oSqlDataReader["Name"]),
                            StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                            EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]),
                            SourceRoute = Convert.ToString(oSqlDataReader["SourceRoute"]),
                            SourceVehicle = Convert.ToString(oSqlDataReader["SourceVehicle"]),
                            SourceJourney = Convert.ToString(oSqlDataReader["SourceJourney"]),
                            TargetRoute = Convert.ToString(oSqlDataReader["TargetRoute"]),
                            TargetVehicle = Convert.ToString(oSqlDataReader["TargetVehicle"]),
                            TargetJourney = Convert.ToString(oSqlDataReader["TargetJourney"]),
                            Category = Convert.ToString(oSqlDataReader["Category"]),
                            //RowNo = Convert.ToInt32(oSqlDataReader["RowNo"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]),
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                        });
                    }
                    return lstOverrideDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get override details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public TransportOverrideDetails Get(int aiId)
        {
            TransportOverrideDetails oTransportOverrideDetails = new TransportOverrideDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportOverrideDetails]"))
                {
                    if (oSqlDataReader.Read())
                    {
                        // oTransportOverrideDetails.Id = oSqlDataReader["Id"].ToInt();

                        oTransportOverrideDetails.SourceJourneyId = oSqlDataReader["SourceJourneyId"].ToInt();
                        oTransportOverrideDetails.SourceRouteId = oSqlDataReader["SourceRouteId"].ToInt();
                        oTransportOverrideDetails.SourceVehicleId = oSqlDataReader["SourceVehicleId"].ToInt();

                        oTransportOverrideDetails.TargetJourneyId = oSqlDataReader["TargetJourneyId"].ToInt();
                        oTransportOverrideDetails.TargetRouteId = oSqlDataReader["TargetRouteId"].ToInt();
                        oTransportOverrideDetails.TargetVehicleId = oSqlDataReader["TargetVehicleId"].ToInt();

                        oTransportOverrideDetails.StartDate = oSqlDataReader["StartDate"].ToDateTime();
                        oTransportOverrideDetails.EndDate = oSqlDataReader["EndDate"].ToDateTime();

                        oTransportOverrideDetails.Name = oSqlDataReader["Name"].ToString();
                        oTransportOverrideDetails.UserIds = oSqlDataReader["UserIds"].ToString();
                        oTransportOverrideDetails.CategoryId = oSqlDataReader["CategoryId"].ToInt();
                    }
                }
                return oTransportOverrideDetails;
            }
        }

        /// <summary>
        /// This method is used to return student list.
        /// </summary>
        /// <param name="aiRouteId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiJourneyId"></param>
        /// <returns></returns>
        public List<Student> GetStudentList(int aiRouteId, int aiVehicleId, int aiJourneyId)
        {
            List<Student> lstStudents = new List<Student>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RouteId", aiRouteId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JourneyId", aiJourneyId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetStudentListForOverride]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstStudents.Add(new Student
                        {
                            RegistraionNo = oSqlDataReader["Enrolment_Number"].ToString(),
                            ClassName = oSqlDataReader["className"].ToString(),
                            Name = oSqlDataReader["StudentName"].ToString(),
                            UserId = oSqlDataReader["UserId"].ToInt()
                        });
                    }
                }
                return lstStudents;
            }
        }

        /// <summary>
        /// This method is used save override details.
        /// </summary>
        /// <param name="aoTransportOverrideDetails"></param>
        public void Save(TransportOverrideDetails aoTransportOverrideDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoTransportOverrideDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceJourneyId", aoTransportOverrideDetails.SourceJourneyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceRouteId", aoTransportOverrideDetails.SourceRouteId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceVehicleId", aoTransportOverrideDetails.SourceVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TargetJourneyId", aoTransportOverrideDetails.TargetJourneyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TargetRouteId", aoTransportOverrideDetails.TargetRouteId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TargetVehicleId", aoTransportOverrideDetails.TargetVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoTransportOverrideDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndDate", aoTransportOverrideDetails.EndDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("StartDate", aoTransportOverrideDetails.StartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("UserIds", aoTransportOverrideDetails.UserIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CategoryId", aoTransportOverrideDetails.CategoryId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveTransportOverrideDetails]");
            }
        }
        
        /// <summary>
        /// This method is used to validate details.
        /// </summary>
        /// <param name="aiSourceRouteId"></param>
        /// <param name="aiSourceVehicleId"></param>
        /// <param name="aiSourceJourneyId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public string Validate(int aiSourceRouteId, int aiSourceVehicleId, int aiSourceJourneyId, DateTime adtStartDate, DateTime adtEndDate, int aiId, string asName)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceRouteId", aiSourceRouteId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceVehicleId", aiSourceVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SourceJourneyId", aiSourceJourneyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_ValidateTransportOverrideDetails]"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader["msg"].ToString();
                }
                return string.Empty;
            }
        }

        #endregion
    }
}
