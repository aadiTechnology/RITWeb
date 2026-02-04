using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SchoolEntities;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class POUserDetailsDC
    {
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;

        public POUserDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        public POUserDetailsDC()
        {
            // TODO: Complete member initialization
        }

        public void Save(SchoolEntities.POUserDetails aoPOUserDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("Id", aoPOUserDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoPOUserDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Address", aoPOUserDetails.Address, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("City", aoPOUserDetails.City, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Pincode", aoPOUserDetails.Pincode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNumber", aoPOUserDetails.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GSTIN", aoPOUserDetails.GSTIN, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePOUserPopupDetails");
            }
        }

        public POUserDetails Get(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);

                POUserDetails oPOUserDetails = new POUserDetails();

                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPOUserPopupDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oPOUserDetails.Address = oSqlDataReader["Address"].ToString();
                        oPOUserDetails.Name = oSqlDataReader["Name"].ToString();
                        oPOUserDetails.City = oSqlDataReader["City"].ToString();
                        oPOUserDetails.Pincode = oSqlDataReader["Pincode"].ToString();
                        oPOUserDetails.MobileNo = oSqlDataReader["MobileNo"].ToString();
                        oPOUserDetails.GSTIN = oSqlDataReader["GSTIN"].ToString();
                    }
                    return oPOUserDetails;
                }
            }
         }

        public void Delete(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);              
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePOUserPopupDetails");
            }
        }

        public List<POUserDetails> GetAll(int aiSchoolId, string asFilter, string sortExpression, int startRowIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);

                List<POUserDetails> lstPOUserDetails = new List<POUserDetails>();
                using (SqlDataReader oSqlDataReader =oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPOUserPopupDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        POUserDetails oPOUserDetails = new POUserDetails();
                        oPOUserDetails.Address = oSqlDataReader["Address"].ToString();
                        oPOUserDetails.City = oSqlDataReader["City"].ToString();
                        oPOUserDetails.GSTIN = oSqlDataReader["GSTIN"].ToString();
                        oPOUserDetails.Id = oSqlDataReader["Id"].ToInt();
                        oPOUserDetails.MobileNo = oSqlDataReader["MobileNo"].ToString();
                        oPOUserDetails.Name = oSqlDataReader["Name"].ToString();
                        oPOUserDetails.Pincode = oSqlDataReader["Pincode"].ToString();
                        oPOUserDetails.TotalRows = oSqlDataReader["TotalRows"].ToInt();
                        lstPOUserDetails.Add(oPOUserDetails);
                    }

                    return lstPOUserDetails;
                }
            }
        }

        public bool CheckDependencies(int aiId)
        {
            string sStatement = "IF EXISTS(select TOP 1 1 from ExternalPOMaster where isdeleted = 0 and ExternalPOUserId = " + aiId + " and schoolid = " + this.miSchoolId + ") SELECT 1 ELSE SELECT 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);
                if (iCount == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool Validate(int aiTypeId, string asValue, int aiId)
        {
            string sFilter = string.Empty;
            if (aiTypeId == 1)
                sFilter = " Name='"+asValue+"'";
            else
                sFilter = " GSTIN='" + asValue + "'";

            string sStatement = "IF EXISTS(select TOP 1 1 from ExternalPOUsers where isdeleted = 0 and Id <> " + aiId + " and schoolid = " + this.miSchoolId + " and " + sFilter + ") SELECT 1 ELSE SELECT 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);
                if (iCount == 1)
                    return true;
                else
                    return false;
            }
        }
    }
}
