// Class Name       :- VendorDetailsDC
// Purpose          :- This class is used to Add vendors configurations.
// Date Of creation :- 12/01/2018
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    public class VendorDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;        
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public VendorDetailsDC() { }        
        public VendorDetailsDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;            
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region "Public Methods"


        public void Save(VendorDetails oVendorDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VendorId", oVendorDetails.VendorId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("SalutationId", oVendorDetails.SalutationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FirstName", oVendorDetails.FirstName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MiddleName", oVendorDetails.MiddleName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("LastName", oVendorDetails.LastName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CompanyName", oVendorDetails.CompanyName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Address", oVendorDetails.VendorAddress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Pincode", oVendorDetails.Pincode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PhoneNo", oVendorDetails.PhNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNo", oVendorDetails.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Fax", oVendorDetails.FaxNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EmailAddress", oVendorDetails.EmailId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GSTNo", oVendorDetails.GSTNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PANNo", oVendorDetails.PANNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccountHolderName", oVendorDetails.AccountHolderName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AccountNumber", oVendorDetails.AccountNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BranchName", oVendorDetails.BranchName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IFSCCode", oVendorDetails.IFSCCode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", oVendorDetails.BankId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveVendorDetails");
            }
        }

        public List<VendorDetails> GetAll(int aiSchoolId, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpr", " ORDER BY " + asSortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);                

                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllVendorDetails"))
                    return FillVendorDetails(oSqlDataReader);
            }
        }

        public VendorDetails Get(int aiVendorId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                VendorDetails oVendorDetails = new VendorDetails();
                oSQLServerDbUtility.AddParameter("VendorId", aiVendorId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetVendorDetails"))
                    if (oSqlDataReader.Read())
                    {
                        oVendorDetails.VendorId = Convert.ToInt32(oSqlDataReader["Id"]);
                        oVendorDetails.VendorNo = Convert.ToInt32(oSqlDataReader["VendorNo"]);
                        oVendorDetails.SalutationId = Convert.ToInt32(oSqlDataReader["SalutationId"]);
                        oVendorDetails.FirstName = Convert.ToString(oSqlDataReader["FirstName"]);
                        oVendorDetails.MiddleName = Convert.ToString(oSqlDataReader["MiddleName"]);
                        oVendorDetails.LastName = Convert.ToString(oSqlDataReader["LastName"]);
                        oVendorDetails.CompanyName = Convert.ToString(oSqlDataReader["CompanyName"]);
                        oVendorDetails.VendorAddress = Convert.ToString(oSqlDataReader["Address"]);
                        oVendorDetails.Pincode = Convert.ToString(oSqlDataReader["Pincode"]);
                        oVendorDetails.PhNumber = Convert.ToString(oSqlDataReader["PhoneNumber"]);
                        oVendorDetails.MobileNo = Convert.ToString(oSqlDataReader["MobileNo"]);
                        oVendorDetails.FaxNo = Convert.ToString(oSqlDataReader["FaxNo"]);
                        oVendorDetails.EmailId = Convert.ToString(oSqlDataReader["EmailId"]);
                        oVendorDetails.GSTNo = Convert.ToString(oSqlDataReader["GSTNo"]);
                        oVendorDetails.PANNo = Convert.ToString(oSqlDataReader["PANNo"]);
                        oVendorDetails.AccountHolderName = Convert.ToString(oSqlDataReader["AccountHolderName"]);
                        oVendorDetails.AccountNumber = Convert.ToString(oSqlDataReader["BankAccountNo"]);
                        oVendorDetails.BranchName = Convert.ToString(oSqlDataReader["BranchName"]);
                        oVendorDetails.IFSCCode = Convert.ToString(oSqlDataReader["IFSCCode"]);
                        oVendorDetails.BankId = Convert.ToInt32(oSqlDataReader["BankId"]);
                    }
                return oVendorDetails;
            }
        }

        public void Delete(int aiVendorId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VendorId", aiVendorId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteVendorDetails");
            }
        }

        public DataSet GetAllVendorsForCombo()
        {            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

               return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllVendorsForCombo");
            }
        }

        #endregion

        #region "Private Methods"

        private List<VendorDetails> FillVendorDetails(SqlDataReader oSqlDataReader)
        {
            List<VendorDetails> lstVendorDetails = new List<VendorDetails>();
            while(oSqlDataReader.Read())
            {
                VendorDetails oVendorDetails = new VendorDetails();
                oVendorDetails.VendorId = Convert.ToInt32(oSqlDataReader["Id"]);
                oVendorDetails.VendorNo = Convert.ToInt32(oSqlDataReader["VendorNo"]);
                oVendorDetails.VendorName = Convert.ToString(oSqlDataReader["VendorName"]);
                oVendorDetails.CompanyName = Convert.ToString(oSqlDataReader["CompanyName"]);                
                oVendorDetails.PhNumber = Convert.ToString(oSqlDataReader["PhoneNumber"]);
                oVendorDetails.MobileNo = Convert.ToString(oSqlDataReader["MobileNo"]);
                oVendorDetails.GSTNo = Convert.ToString(oSqlDataReader["GSTNo"]);
                oVendorDetails.TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]);
                
                lstVendorDetails.Add(oVendorDetails);
            }
            return lstVendorDetails;
        }


        #endregion
    }
}
