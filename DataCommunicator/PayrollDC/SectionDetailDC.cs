// File Name - SectionDetailDC.cs
// Creator - Sachin
// Created Date - 

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for insert/delete/update/ display of sections.
    /// </summary>
    public class SectionDetailDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miFinYearId;
        private int miUpdatedById;
        private SectionDetails moSectionDetails; 

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SectionDetailDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public SectionDetailDC(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)

        /// <summary>
        /// Sets section details.
        /// </summary>
        public SectionDetails SectionDetails
        {
            set { this.moSectionDetails = value; }
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// THis method is used to return all the sections.
        /// </summary>
        /// <returns>Entity list of SectionDetails</returns>
        public List<SectionDetails> GetAll()
        {
            List<SectionDetails> lstSectionDetails = new List<SectionDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSections"))
                return FillSectionDetails(oSqlDataReader);
            }           
        }

        /// <summary>
        /// This method will be used to fill section details entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<SectionDetails> FillSectionDetails(SqlDataReader aoSqlDataReader)
        {
            List<SectionDetails> lstSectionDetails = new List<SectionDetails>();
            while (aoSqlDataReader.Read())
            {
                SectionDetails oSectionDetails = new SectionDetails
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    MaxAmount = Convert.ToDecimal(!aoSqlDataReader["MaxAmount"].ToString().IsNullOrEmpty() ? aoSqlDataReader["MaxAmount"] : 0),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    SectionGroupId = Convert.ToInt32(aoSqlDataReader["SectionGroupId"]),
                    SectionGroupName = Convert.ToString(aoSqlDataReader["SectionGroupName"]),
                    IsExemption = Convert.ToBoolean(aoSqlDataReader["IsExemption"]),
                    CategoryId = Convert.ToInt32(aoSqlDataReader["CategoryId"]),
                    GroupId = Convert.ToInt32(aoSqlDataReader["GroupId"]),
                    GroupMaxAmount = Convert.ToDecimal(aoSqlDataReader["GroupMaxAmount"])
                };
                lstSectionDetails.Add(oSectionDetails);
            }
            return lstSectionDetails;
        }

        /// <summary>
        /// This method is used to return all the section groups.
        /// </summary>
        /// <returns></returns>
        public List<SectionGroup> GetAllSectionGroups()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = "SELECT Id,Name,IsExemption FROM SectionGroups";
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    var oGenericClass = new GenericClass<SectionGroup>();
                    return oGenericClass.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to save configuration.
        /// </summary>
        public void Save()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", this.moSectionDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", this.moSectionDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", this.moSectionDetails.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SectionGroupId", this.moSectionDetails.SectionGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MaxAmount", this.moSectionDetails.MaxAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("CategoryId", this.moSectionDetails.CategoryId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSectionDetails");
            }
        }

        /// <summary>
        /// This method is used to delete configuration.
        /// </summary>
        /// <param name="aiSectionId"></param>
        public void Delete(int aiSectionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SectionId", aiSectionId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSectionDetails");
            }
        }

        #endregion
    }
}
