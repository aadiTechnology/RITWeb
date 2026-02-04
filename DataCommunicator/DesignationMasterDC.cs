

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MasterEntities;

namespace DataCommunicator
{
    public class DesignationMasterDC
    {

        #region DataMembers
        public int miSchooId = 0;
        public int miAcademicYearId = 0;
        public DesignationMaster moDesignationMaster;
        private int aiSchoolId;
        private int aiAcademicYearId;
        private int aiUserId;
        #endregion

        #region " Constructors "
        public DesignationMasterDC()
        {
            moDesignationMaster = new DesignationMaster();
        }

        public DesignationMasterDC(int miTeacherDesignationId)
        {
            moDesignationMaster = new DesignationMaster();
        }

        public DesignationMasterDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.aiSchoolId = aiSchoolId;
            this.aiAcademicYearId = aiAcademicYearId;
            this.aiUserId = aiUserId;
        }
        #endregion " Constructors "

        #region Public Methods

        /// <summary>
        /// This function is used to get all Designation Name details and bind to object data source. 
        /// </summary>
        public List<DesignationMaster> GetAll(String asSortExpression, int aiEndIndex, int astartRowIndex, bool abIsPTADesignation)
        {
            if (abIsPTADesignation == false)
            {
                if (asSortExpression == string.Empty || asSortExpression == "Name" || asSortExpression == "Name ASC")
                    asSortExpression = "Order By T.User_Role_Id,Teacher_Designation_Name";
                else if (asSortExpression == "Name DESC")
                    asSortExpression = "Order By T.User_Role_Id desc,Teacher_Designation_Name DESC";
                if (asSortExpression == string.Empty || asSortExpression == "SortOrder" || asSortExpression == "SortOrder ASC")
                    asSortExpression = "Order By SortOrder";
                else if (asSortExpression == "SortOrder DESC")
                    asSortExpression = "Order By SortOrder DESC";

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("StartIndex", astartRowIndex, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.Text);
                    using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedDesignatonMaster"))
                        return ReadAllDesignations(oDR);

                }
            }
            else
            {
                if (asSortExpression == string.Empty || asSortExpression == "Name" || asSortExpression == "Name ASC")
                    asSortExpression = "Order By  Name";
                else if (asSortExpression == "Name DESC")
                    asSortExpression = "Order By  Name DESC";
                if (asSortExpression == string.Empty || asSortExpression == "SortOrder" || asSortExpression == "SortOrder ASC")
                    asSortExpression = "Order By SortOrder";
                else if (asSortExpression == "SortOrder DESC")
                    asSortExpression = "Order By SortOrder DESC";

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("StartIndex", astartRowIndex, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.Text);
                    using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPTADEsignationMaster"))
                        return ReadAllDesignations(oDR);

                }
            }
        }


        public List<DesignationMaster> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllDesignatons"))
                    return ReadAllDesignations(oSqlDataReader);
            }
        }

        public List<DesignationMaster> ReadAllDesignations(SqlDataReader aoSqlDataReader)
        {
            List<DesignationMaster> lstDesignations = new List<DesignationMaster>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    DesignationMaster oDesignationMaster = new DesignationMaster();
                    if (aoSqlDataReader["Designation"] != DBNull.Value)
                        oDesignationMaster.Designation = Convert.ToString(aoSqlDataReader["Designation"]);
                    if (aoSqlDataReader["DesignationId"] != DBNull.Value)
                        oDesignationMaster.DesignationId = Convert.ToInt32(aoSqlDataReader["DesignationId"]);
                    if (aoSqlDataReader["Name"] != DBNull.Value)
                        oDesignationMaster.UserRoleName = Convert.ToString(aoSqlDataReader["Name"]);
                    if (aoSqlDataReader["Id"] != DBNull.Value)
                        oDesignationMaster.UserRoleId = Convert.ToInt32(aoSqlDataReader["Id"]);
                    if (aoSqlDataReader["SortOrder"] != DBNull.Value)
                        oDesignationMaster.SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]);
                    if (aoSqlDataReader["HasAccountsAccess"] != DBNull.Value)
                        oDesignationMaster.HasAccountAccess = Convert.ToBoolean(aoSqlDataReader["HasAccountsAccess"]);
                    lstDesignations.Add(oDesignationMaster);
                }
                aoSqlDataReader.Close();
            }
            return lstDesignations;
        }

        /// <summary>
        /// This method is used to get total count of designation details records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abIsPTADesignation"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, bool abIsPTADesignation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                if (abIsPTADesignation == false)
                {
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_CountDesignations]");
                }
                else
                {
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_CountPTADesignation]");
                }
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// To Insert Designation Information.
        /// </summary>
        /// <param name="aoDesignationMaster"></param>
        /// <param name="rdoPTADesignation"></param>

        public void Insert(DesignationMaster aoDesignationMaster, bool abIsPTADesignation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Designation", aoDesignationMaster.Designation, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("Name", aoDesignationMaster.UserRoleName, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("User_Role_Id", aoDesignationMaster.UserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortOrder", aoDesignationMaster.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", this.aiUserId, SqlDbType.Int);
                if (abIsPTADesignation == false)
                {
                    oSQLServerDbUtility.AddParameter("HasAccountAccess", aoDesignationMaster.HasAccountAccess, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertDesignations");
                }
                else
                {
                    oSQLServerDbUtility.AddParameter("DesignationId", 0, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertPTADesignation");
                }
            }
        }

        /// <summary>
        /// To Insert Designation Information.
        /// </summary>
        /// <param name="aoDesignationMaster"></param>
        /// <param name="abIsPTADesignation"></param>

        public void Update(DesignationMaster aoDesignationMaster, bool abIsPTADesignation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Designation", aoDesignationMaster.Designation, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("DesignationId", aoDesignationMaster.DesignationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoDesignationMaster.UserRoleName, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("User_Role_Id", aoDesignationMaster.UserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortOrder", aoDesignationMaster.SortOrder, SqlDbType.Int);
                if (abIsPTADesignation == false)
                {
                    oSQLServerDbUtility.AddParameter("Updated_By_Id", this.aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("HasAccountAccess", aoDesignationMaster.HasAccountAccess, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateDesignations");
                }
                else
                {
                    oSQLServerDbUtility.AddParameter("Inserted_By_Id", this.aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertPTADesignation");
                }

            }
        }

        /// <summary>
        /// To get Designation related single record to Updat.
        /// </summary>
        /// <param name="aiDesignationId"></param>
        /// <param name="abIsPTADesignation"></param>
        /// <returns></returns>


        public DesignationMaster Get(int aiDesignationId, bool abIsPTADesignation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("DesignationId", aiDesignationId, SqlDbType.Int);
                if (abIsPTADesignation == false)
                {
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSingleDesignationRecord"))
                    {
                        if (oSqlDataReader.Read())
                            return ReadObjectFromReader(oSqlDataReader);
                    }
                }

                else
                {
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSinglePTADesignationRecords"))
                    {
                        if (oSqlDataReader.Read())
                            return ReadObjectFromReader(oSqlDataReader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method is used to populate object of designation.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>

        private DesignationMaster ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            DesignationMaster oDesignationMaster = new DesignationMaster
            {
                DesignationId = Convert.ToInt32(aoSqlDataReader["DesignationId"]),
                Designation = Convert.ToString(aoSqlDataReader["Designation"]),
                UserRoleId = Convert.ToInt32(aoSqlDataReader["Id"]),
                UserRoleName = Convert.ToString(aoSqlDataReader["Name"]),
                SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                HasAccountAccess = Convert.ToBoolean(aoSqlDataReader["HasAccountAccess"])
            };
            return oDesignationMaster;
        }

        /// <summary>
        /// This method is used to check dependancy of designatin name and Delete designation.
        /// </summary>
        /// <param name="aiDesignationID"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="abIsPTADesignation"></param>
        /// <returns></returns>

        public int Delete(int aiDesignationID, int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsPTADesignation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("DesignationId", aiDesignationID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                if (abIsPTADesignation == false)
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteDesignation");
                else
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeletePTADesignation");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get Admistaff records whose Accounting screen is Access permission.
        /// </summary>
        /// <returns></returns>

        public List<int> GetAccountDesignations()
        {
            List<int> olstInt = new List<int>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAccountDesignations"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Designation"] != DBNull.Value)
                                olstInt.Add(Convert.ToInt32(oDR["Designation"]));
                        }
                    }
                }
            }
            return olstInt;
        }
        #endregion
    }
}
