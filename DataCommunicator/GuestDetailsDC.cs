using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Data;
using SchoolEntities.Admin;

namespace DataCommunicator
{
    public class GuestDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public GuestDetailsDC() { }        
        public GuestDetailsDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region " Public Method "

        /// <summary>
        /// This method is used for Save the Guest Details in database.
        /// </summary>
        public void Save(SchoolGuestDetails aoSchoolGuestDetails, out int aiGuestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                aiGuestId = Constants.I_ZERO;

                oSQLServerDbUtility.AddParameter("GuestId", aoSchoolGuestDetails.GuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aoSchoolGuestDetails.CategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SalutationId", aoSchoolGuestDetails.SalutaionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GuestName", aoSchoolGuestDetails.GuestName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Date", aoSchoolGuestDetails.Date, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InTime", aoSchoolGuestDetails.InTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("OutTime", aoSchoolGuestDetails.OutTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobilNo", aoSchoolGuestDetails.MobileNum, SqlDbType.NVarChar);

                if(aoSchoolGuestDetails.AadharCardNo != null)
                    oSQLServerDbUtility.AddParameter("AadharNo", aoSchoolGuestDetails.AadharCardNo, SqlDbType.NVarChar);
                if(aoSchoolGuestDetails.PanCardNo != null)
                    oSQLServerDbUtility.AddParameter("PanCardNo", aoSchoolGuestDetails.PanCardNo, SqlDbType.NVarChar);

                oSQLServerDbUtility.AddParameter("PurposeOfVisit", aoSchoolGuestDetails.PurposeOfVisit, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("OrganisationName", aoSchoolGuestDetails.OrganisationName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("WhomToMeet", aoSchoolGuestDetails.WhomToMeet, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Designation", aoSchoolGuestDetails.Designation, SqlDbType.NVarChar);
                if (aoSchoolGuestDetails.GuestPhoto != null)
                    oSQLServerDbUtility.AddParameter("GuestPhoto", aoSchoolGuestDetails.GuestPhoto, SqlDbType.Binary);

                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUpdatedById, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_SaveSchoolGuestDetails"))
                {
                    if (oSqlDataReader.Read())
                        aiGuestId = Convert.ToInt32(oSqlDataReader["GuestId"]);
                }
            }
        }

        /// <summary>
        /// This method is used for get all the details of Guest for filling the list view..
        /// </summary>
        public List<SchoolGuestDetails> GetAll(int aiSchoolId, string asFilter, string asGuestType, int istartRowIndex, int aiEndIndex, int aiCategoryType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("asGuestType", asGuestType, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", istartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("asCategoryType", aiCategoryType, SqlDbType.Int);
                
             
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolAllGuestDetails"))
                    return FillGuestDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used for Getting the count of all Guests.
        /// </summary>
        public int GetCount(int aiSchoolId, string asFilter, string asGuestType, int aiCategoryType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("asGuestType", asGuestType, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("asCategoryType", aiCategoryType, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CountSchoolGuestDetails");
                return Convert.ToInt32(oSqlParameter.Value); 
            }
        }

        /// <summary>
        /// This method is used for getting the designation of staff for filling the designation text box on screen.
        /// </summary>
        public void GetDesignationForGuestStaff(string asUserName, int aiSchoolId, out string asDesignation)
        {
            asDesignation = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asUserName, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDesignationForGuestStaff"))
                {
                    if (oSqlDataReader.Read())
                        asDesignation = Convert.ToString(oSqlDataReader["Designation"]);
                }                
            }
        }

        /// <summary>
        /// This method is used geting the guest details for edit mode.
        /// </summary>
        public SchoolGuestDetails Get(int aiGuestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                SchoolGuestDetails oSchoolGuestDetails;
                oSQLServerDbUtility.AddParameter("GuestId", aiGuestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSchoolGuestDetails = new SchoolGuestDetails();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGuestDetailsForEditMode"))
                    if (oSqlDataReader.Read())
                    {
                        oSchoolGuestDetails.GuestId = Convert.ToInt32(oSqlDataReader["Id"]);
                        oSchoolGuestDetails.SalutaionId = Convert.ToInt32(oSqlDataReader["SalutationId"]);
                        oSchoolGuestDetails.GuestName = Convert.ToString(oSqlDataReader["Name"]);
                        oSchoolGuestDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);
                        oSchoolGuestDetails.InTime = Convert.ToString(oSqlDataReader["InTime"]);
                        if (oSqlDataReader["OutTime"] != DBNull.Value)
                            oSchoolGuestDetails.OutTime = Convert.ToString(oSqlDataReader["OutTime"]);
                        oSchoolGuestDetails.MobileNum = Convert.ToString(oSqlDataReader["MobileNo"]);
                        if (oSqlDataReader["AadharCardNo"] != DBNull.Value)
                            oSchoolGuestDetails.AadharCardNo = Convert.ToString(oSqlDataReader["AadharCardNo"]);
                        if (oSqlDataReader["PanCardNo"] != DBNull.Value)
                            oSchoolGuestDetails.PanCardNo = Convert.ToString(oSqlDataReader["PanCardNo"]);
                        oSchoolGuestDetails.PurposeOfVisit = Convert.ToString(oSqlDataReader["PurposeOfVisit"]);
                        oSchoolGuestDetails.OrganisationName = Convert.ToString(oSqlDataReader["WhereUComeFrom"]);
                        oSchoolGuestDetails.WhomToMeet = Convert.ToString(oSqlDataReader["WhomeUMeet"]); 
                        oSchoolGuestDetails.Designation = Convert.ToString(oSqlDataReader["Designation"]);
                        oSchoolGuestDetails.CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]);
                        oSchoolGuestDetails.GuestPhoto = oSqlDataReader["BinaryPhotoImage"] as byte[];
                        
                    }
                return oSchoolGuestDetails;
            }
        }

        /// <summary>
        /// This method is used deleting the data for perticular guest.
        /// </summary>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSchoolGuestDetails");
            }
        }
        /// <summary>
        /// This method is used geting the Category Type.
        /// </summary>
        /// <returns></returns>
        public List<SchoolGuestDetails> GetCategoryType()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
              
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCategoryTypeName"))
                    return this.FillCategoryType(oSqlDataReader);
            }
            
        }
        /// <summary>
        /// This method is used Filling the Category Type.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<SchoolGuestDetails> FillCategoryType(SqlDataReader aoSqlDataReader)
        {
            List<SchoolGuestDetails> lstCategory = new List<SchoolGuestDetails>();
            while (aoSqlDataReader.Read())
            {
                SchoolGuestDetails oSchoolGuestDetails = new SchoolGuestDetails();
                oSchoolGuestDetails.CategoryId = Convert.ToInt32(aoSqlDataReader["Id"]);
                oSchoolGuestDetails.CategoryName = Convert.ToString(aoSqlDataReader["Name"]);
                
                lstCategory.Add(oSchoolGuestDetails);
            }
            return lstCategory;
        } 

        #endregion

        #region " Private Method "

        /// <summary>
        /// This method is used for fill guest details.
        /// </summary>
        private List<SchoolGuestDetails> FillGuestDetails(SqlDataReader oSqlDataReader)
        {
            List<SchoolGuestDetails> olstSchoolGuestDetails = new List<SchoolGuestDetails>();
            while (oSqlDataReader.Read())
            {
                SchoolGuestDetails oSchoolGuestDetails = new SchoolGuestDetails();

                oSchoolGuestDetails.GuestId = Convert.ToInt32(oSqlDataReader["GuestId"]);
                oSchoolGuestDetails.SalutaionId = Convert.ToInt32(oSqlDataReader["SalutationId"]);
                oSchoolGuestDetails.GuestName = Convert.ToString(oSqlDataReader["Name"]);
                oSchoolGuestDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);
                oSchoolGuestDetails.InTime = Convert.ToString(oSqlDataReader["InTime"]);
                oSchoolGuestDetails.OutTime = Convert.ToString(oSqlDataReader["OutTime"]);
                oSchoolGuestDetails.MobileNum = Convert.ToString(oSqlDataReader["MobileNo"]);
                oSchoolGuestDetails.AadharCardNo = Convert.ToString(oSqlDataReader["AadharCardNo"]);
                oSchoolGuestDetails.PanCardNo = Convert.ToString(oSqlDataReader["PanCardNo"]);
                oSchoolGuestDetails.WhomToMeet = Convert.ToString(oSqlDataReader["WhomUMeet"]);
                oSchoolGuestDetails.CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]);
                oSchoolGuestDetails.CategoryName = Convert.ToString(oSqlDataReader["CategoryName"]);
                olstSchoolGuestDetails.Add(oSchoolGuestDetails);
            }
            return olstSchoolGuestDetails;
        }

        #endregion
    }
}
