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
    public class ServiceReceiverDetailsDC
    {
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;

        public ServiceReceiverDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        public ServiceReceiverDetailsDC()
        {
            // TODO: Complete member initialization
        }

        public void Save(SchoolEntities.ServiceReceiverDetails aoServiceReceiverDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("Id", aoServiceReceiverDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoServiceReceiverDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Address", aoServiceReceiverDetails.Address, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("City", aoServiceReceiverDetails.City, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Pincode", aoServiceReceiverDetails.Pincode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNumber", aoServiceReceiverDetails.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GSTIN", aoServiceReceiverDetails.GSTIN, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveServiceReceiverDetails");
            }
        }

        public ServiceReceiverDetails Get(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);

                 ServiceReceiverDetails oServiceReceiverDetails = new ServiceReceiverDetails();

                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetServiceReceiverDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oServiceReceiverDetails.Address = oSqlDataReader["Address"].ToString();
                        oServiceReceiverDetails.Name = oSqlDataReader["Name"].ToString();
                        oServiceReceiverDetails.City = oSqlDataReader["City"].ToString();
                        oServiceReceiverDetails.Pincode = oSqlDataReader["Pincode"].ToString();
                        oServiceReceiverDetails.MobileNo = oSqlDataReader["MobileNo"].ToString();
                        oServiceReceiverDetails.GSTIN = oSqlDataReader["GSTIN"].ToString();
                    }
                    return oServiceReceiverDetails;
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
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteServiceReceiverDetails");
            }
        }

        public List<ServiceReceiverDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asFilter, string sortExpression, int startRowIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);

                List<ServiceReceiverDetails> lstServiceReceiverDetails = new List<ServiceReceiverDetails>();
                using (SqlDataReader oSqlDataReader =oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllServiceReceiverDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ServiceReceiverDetails oServiceReceiverDetails = new ServiceReceiverDetails();
                        oServiceReceiverDetails.Address = oSqlDataReader["Address"].ToString();
                        oServiceReceiverDetails.City = oSqlDataReader["City"].ToString();
                        oServiceReceiverDetails.GSTIN = oSqlDataReader["GSTIN"].ToString();
                        oServiceReceiverDetails.Id = oSqlDataReader["Id"].ToInt();
                        oServiceReceiverDetails.MobileNo = oSqlDataReader["MobileNo"].ToString();
                        oServiceReceiverDetails.Name = oSqlDataReader["Name"].ToString();
                        oServiceReceiverDetails.Pincode = oSqlDataReader["Pincode"].ToString();
                        oServiceReceiverDetails.TotalRows = oSqlDataReader["TotalRows"].ToInt();
                        lstServiceReceiverDetails.Add(oServiceReceiverDetails);
                    }

                    return lstServiceReceiverDetails;
                }
            }
        }

        public bool CheckDependencies(int aiId)
        {
            string sStatement = "IF EXISTS(select TOP 1 1 from gstinvoicemaster where isdeleted = 0 and ServiceReceiverId = "+aiId+" and schoolid = "+this.miSchoolId+") SELECT 1 ELSE SELECT 0";
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

            string sStatement = "IF EXISTS(select TOP 1 1 from ServiceReceiverDetails where isdeleted = 0 and Id <> " + aiId + " and schoolid = " + this.miSchoolId + "and AcademicYearId = " + this.miAcademicYearId + "  and "+sFilter+") SELECT 1 ELSE SELECT 0";
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
