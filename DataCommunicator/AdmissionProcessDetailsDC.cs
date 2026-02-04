// Class Name       :- AdmissionProcessDetailsDC
// Purpose          :- This class is used to manage student Admission Process details.
// Date Of creation :- 10/10/2015
// Author Name      :-

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Data;
namespace DataCommunicator
{
    public class AdmissionProcessDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public AdmissionProcessDetailsDC() { }
        public AdmissionProcessDetailsDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }
        public AdmissionProcessDetailsDC(int aiSchoolId, int aiUserId,int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all students Admission process details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<AdmissionProcessDetails> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAdmissionProcessDetails"))
                    return FillAdmissionProcessDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get Particular Admission details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="SchoolId"></param>
        /// <param name="AcademicYearId"></param>
        public AdmissionProcessDetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                AdmissionProcessDetails oAdmissionProcessDetails;
                oSQLServerDbUtility.AddParameter("AdmissionProcessId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oAdmissionProcessDetails = new AdmissionProcessDetails();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAdmissionProcessDetails"))
                    if (oSqlDataReader.Read())
                    {
                        oAdmissionProcessDetails.AdmissionProcessId = Convert.ToInt32(oSqlDataReader["AdmissionProcessId"]);
                        oAdmissionProcessDetails.StanderdId = Convert.ToInt32(oSqlDataReader["Standard_Id"]);
                        if (oSqlDataReader["TotalForms"] != DBNull.Value)
                            oAdmissionProcessDetails.TotalForms = Convert.ToInt32(oSqlDataReader["TotalForms"]);
                        if (oSqlDataReader["TotalOnlineForms"] != DBNull.Value)
                            oAdmissionProcessDetails.TotalOnlineForms = Convert.ToInt32(oSqlDataReader["TotalOnlineForms"]);
                        if (oSqlDataReader["FormOpenDate"] != DBNull.Value)
                            oAdmissionProcessDetails.FormOpenDate = Convert.ToDateTime(oSqlDataReader["FormOpenDate"]);
                        if (oSqlDataReader["FormCloseDate"] != DBNull.Value)
                            oAdmissionProcessDetails.FormCloseDate = Convert.ToDateTime(oSqlDataReader["FormCloseDate"]);
                        if (oSqlDataReader["LottoryDate"] != DBNull.Value)
                            oAdmissionProcessDetails.LottoryDate = Convert.ToDateTime(oSqlDataReader["LottoryDate"]);
                        if (oSqlDataReader["AdmissionConfirmLastDate"] != DBNull.Value)
                            oAdmissionProcessDetails.AdmissionConfirmLastDate = Convert.ToDateTime(oSqlDataReader["AdmissionConfirmLastDate"]);
                        oAdmissionProcessDetails.IsLotteryConfirmed = Convert.ToBoolean(oSqlDataReader["ISLotteryConfirmed"]);
                        oAdmissionProcessDetails.CanConfirmDirectly = Convert.ToBoolean(oSqlDataReader["CanConfirmDirectly"]);
                        if (oSqlDataReader["Amount"] != DBNull.Value)
                            oAdmissionProcessDetails.Amount = Convert.ToInt32(oSqlDataReader["Amount"]);
                        if (oSqlDataReader["DOBMax"] != DBNull.Value)
                            oAdmissionProcessDetails.DOBMax = Convert.ToDateTime(oSqlDataReader["DOBMax"]);
                        if (oSqlDataReader["DOBMin"] != DBNull.Value)
                            oAdmissionProcessDetails.DOBMin = Convert.ToDateTime(oSqlDataReader["DOBMin"]);
                        oAdmissionProcessDetails.EnableAdmissionFormFee = Convert.ToBoolean(oSqlDataReader["EnableAdmissionFormFee"]);
                        oAdmissionProcessDetails.IsInternalAdmission = Convert.ToBoolean(oSqlDataReader["IsInternalAdmission"]);
                        if (oSqlDataReader["EnableWaitingList"] != DBNull.Value)
                            oAdmissionProcessDetails.EnableWaitingList = Convert.ToBoolean(oSqlDataReader["EnableWaitingList"]);
                        else
                            oAdmissionProcessDetails.EnableWaitingList = false;
                        if (oSqlDataReader["WaitingListURL"] != DBNull.Value)
                            oAdmissionProcessDetails.WaitingListURL = Convert.ToString(oSqlDataReader["WaitingListURL"]);
                        else
                            oAdmissionProcessDetails.WaitingListURL = string.Empty;
                        // Read new fields from database
                        if (oSqlDataReader["EnableInternalLink"] != DBNull.Value)
                            oAdmissionProcessDetails.EnableInternalLink = Convert.ToBoolean(oSqlDataReader["EnableInternalLink"]);
                        else
                            oAdmissionProcessDetails.EnableInternalLink = false;
                        if (oSqlDataReader["ExternalSiteMessage"] != DBNull.Value)
                            oAdmissionProcessDetails.ExternalSiteMessage = Convert.ToString(oSqlDataReader["ExternalSiteMessage"]);
                        else
                            oAdmissionProcessDetails.ExternalSiteMessage = string.Empty;
                    }
                return oAdmissionProcessDetails;
            }
        }

        private List<AdmissionProcessDetails> FillAdmissionProcessDetails(SqlDataReader aoSqlDataReader)
        {
            List<AdmissionProcessDetails> olstAdmissionProcessDetails = new List<AdmissionProcessDetails>();
            while (aoSqlDataReader.Read())
            {
                AdmissionProcessDetails oAdmissionProcessDetails = new AdmissionProcessDetails();
                oAdmissionProcessDetails.AdmissionProcessId = Convert.ToInt32(aoSqlDataReader["AdmissionProcessId"]);
                oAdmissionProcessDetails.StandardName = Convert.ToString(aoSqlDataReader["Standard_Name"]);
                if (aoSqlDataReader["FormOpenDate"] != DBNull.Value)
                    oAdmissionProcessDetails.FormOpenDate = Convert.ToDateTime(aoSqlDataReader["FormOpenDate"]);
                if (aoSqlDataReader["FormCloseDate"] != DBNull.Value)
                    oAdmissionProcessDetails.FormCloseDate = Convert.ToDateTime(aoSqlDataReader["FormCloseDate"]);
                if (aoSqlDataReader["Amount"] != DBNull.Value)
                    oAdmissionProcessDetails.Amount = Convert.ToInt32(aoSqlDataReader["Amount"]);
                if (aoSqlDataReader["DOBMax"] != DBNull.Value)
                    oAdmissionProcessDetails.DOBMax = Convert.ToDateTime(aoSqlDataReader["DOBMax"]);
                if (aoSqlDataReader["DOBMin"] != DBNull.Value)
                    oAdmissionProcessDetails.DOBMin = Convert.ToDateTime(aoSqlDataReader["DOBMin"]);
                olstAdmissionProcessDetails.Add(oAdmissionProcessDetails);
            }
            return olstAdmissionProcessDetails;
        }

        /// <summary>
        /// This method is used to save Admission Process details.
        /// </summary>
        /// <param name="oAdmissionProcessDetails"></param>
        public void Save(AdmissionProcessDetails aoAdmissionProcessDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AdmissionProcessId", aoAdmissionProcessDetails.AdmissionProcessId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StanderdId", aoAdmissionProcessDetails.StanderdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TotalForms", aoAdmissionProcessDetails.TotalForms, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TotalOnlineForms", aoAdmissionProcessDetails.TotalOnlineForms, SqlDbType.Int);
                if (aoAdmissionProcessDetails.FormOpenDate.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("FormOpenDate", aoAdmissionProcessDetails.FormOpenDate, SqlDbType.DateTime);
                if (aoAdmissionProcessDetails.FormCloseDate.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("FormCloseDate", aoAdmissionProcessDetails.FormCloseDate, SqlDbType.DateTime);
                if (aoAdmissionProcessDetails.LottoryDate.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("LottoryDate", aoAdmissionProcessDetails.LottoryDate, SqlDbType.DateTime);
                if (aoAdmissionProcessDetails.AdmissionConfirmLastDate.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("AdmissionConfirmLastDate", aoAdmissionProcessDetails.AdmissionConfirmLastDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("IsLotteryConfirmed", aoAdmissionProcessDetails.IsLotteryConfirmed, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("CanConfirmDirectly", aoAdmissionProcessDetails.CanConfirmDirectly, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Amount", aoAdmissionProcessDetails.Amount, SqlDbType.Int);
                if (aoAdmissionProcessDetails.DOBMax.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("DOBMax", aoAdmissionProcessDetails.DOBMax, SqlDbType.DateTime);
                if (aoAdmissionProcessDetails.DOBMin.ToString(Constants.S_DATE_FORMAT) != "01-Jan-0001")
                    oSQLServerDbUtility.AddParameter("DOBMin", aoAdmissionProcessDetails.DOBMin, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EnableAdmissionFormFee", aoAdmissionProcessDetails.EnableAdmissionFormFee, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsInternalAdmission", aoAdmissionProcessDetails.IsInternalAdmission, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("EnableWaitingList", aoAdmissionProcessDetails.EnableWaitingList, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("WaitingListURL", aoAdmissionProcessDetails.WaitingListURL, SqlDbType.Text);
                // Add new fields to save operation
                oSQLServerDbUtility.AddParameter("EnableInternalLink", aoAdmissionProcessDetails.EnableInternalLink, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ExternalSiteMessage", aoAdmissionProcessDetails.ExternalSiteMessage, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveAdmissionProcessDetails");
            }
        }

        /// <summary>
        /// This method is used to save Student Location details.
        /// </summary>
        /// <param name="miId"></param>
        /// <param name="miLocation"></param>
        /// <param name="miUserId"></param>
        /// <param name="miSchoolId"></param>
        public void SaveStudentLocation(int aiId, string asLocation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LocationName", asLocation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentLocationDetails");
            }
        }

        /// <summary>
        /// This method will fetch all the Location from the database. It will simply convert the result into a List object and return it.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<StudentLivingLocation> GetAllLivingLocation()
        {
            List<StudentLivingLocation> lstLivingLocation = new List<StudentLivingLocation>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentLivingLocation"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstLivingLocation.Add(new StudentLivingLocation
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            LocationName = Convert.ToString(oSqlDataReader["Location"])
                        });
                    }
                }
            }
            return lstLivingLocation;
        }

        /// <summary>
        /// This method is used to delete Location.
        /// </summary>
        /// <param name="aiLocationId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiSchoolIdId"></param>
        public void DeleteLocation(int aiLocationId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiLocationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteLocationDetails");
            }
        }        

        /// <summary>
        /// This method is used to delete Admission Process details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AdmissionProcessId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteAdmissionProcessDetail");
            }
        }

        /// <summary>
        /// This method is used to Check Selected Standard is Already Exist or Not.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiStanderdId"></param>
        public bool IsConfigurationAlreadyExist(int aiId, int aiStanderdId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AdmissionProcessId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StanderdId", aiStanderdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("IsDulpicate", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IfAlreadyExistStandard");
                return OSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This method is used to get Internal Link Standards details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<InternalLinkStandardDetails> GetInternalLinkStandards(int aiSchoolId, int aiAcademicYearId)
        {
            List<InternalLinkStandardDetails> lstInternalLinkStandardDetails = new List<InternalLinkStandardDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInternalLinkStandards"))
                {
                    while (oSqlDataReader.Read())
                    {
                        InternalLinkStandardDetails oInternalLinkStandardDetails = new InternalLinkStandardDetails();
                        if (oSqlDataReader["Standard_Name"] != DBNull.Value)
                            oInternalLinkStandardDetails.StandardName = Convert.ToString(oSqlDataReader["Standard_Name"]);
                        else
                            oInternalLinkStandardDetails.StandardName = string.Empty;
                        if (oSqlDataReader["ExternalSiteMessage"] != DBNull.Value)
                            oInternalLinkStandardDetails.DisplayMessage = Convert.ToString(oSqlDataReader["ExternalSiteMessage"]);
                        else
                            oInternalLinkStandardDetails.DisplayMessage = string.Empty;
                        lstInternalLinkStandardDetails.Add(oInternalLinkStandardDetails);
                    }
                }
            }
            return lstInternalLinkStandardDetails;
        }
        #endregion "Public Methods"
    }
}
