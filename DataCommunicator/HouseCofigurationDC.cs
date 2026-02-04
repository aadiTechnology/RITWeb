// -----------------------------------------------------------------------
// <copyright file="HouseMasterDC.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.SqlClient;
    using House;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HouseCofigurationDC
    {
        # region constants
        public int miSchoolId = 0;
        public int miAcademicYearId = 0;
        public int miUpdatedById = 0;
        #endregion

        #region constructors

        public HouseCofigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        }

        public HouseCofigurationDC()
        {
           
        }
        #endregion

        /// <summary>
        /// This method is used to get all House Details.
        /// </summary>
        /// <returns></returns>
        public List<HouseConfiguration> GetAll(String asSortExpression, int aiSchoolId, int aiAcademicYearId)
        {
            if (asSortExpression == string.Empty || asSortExpression == "Name" || asSortExpression == "Name ASC")
                asSortExpression = "Order By Name";
            else if (asSortExpression == "Name DESC")
                asSortExpression = "Order By Name DESC";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.NVarChar);                
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHouseDetails"))
                return ReadAllHouses(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get count of Houses.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_CountHouseDetails]");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<HouseConfiguration> ReadAllHouses(SqlDataReader aoSqlDataReader)
        {
            List<HouseConfiguration> lstHouseConfiguration = new List<HouseConfiguration>();
            if (aoSqlDataReader != null && aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    HouseConfiguration oHouseConfiguration = new HouseConfiguration();
                    if (aoSqlDataReader["Id"] != DBNull.Value)
                        oHouseConfiguration.Id = Convert.ToInt16(aoSqlDataReader["Id"]);
                    if (aoSqlDataReader["Name"] != DBNull.Value)
                        oHouseConfiguration.Name = Convert.ToString(aoSqlDataReader["Name"]);
                    if (aoSqlDataReader["Color"] != DBNull.Value)
                        oHouseConfiguration.Color = Convert.ToString(aoSqlDataReader["Color"]);
                    if (aoSqlDataReader["Motto"] != DBNull.Value)
                        oHouseConfiguration.Motto = Convert.ToString(aoSqlDataReader["Motto"]);

                    lstHouseConfiguration.Add(oHouseConfiguration);
                }
                aoSqlDataReader.Close();
            }
            return lstHouseConfiguration;
        }

        /// <summary>
        /// This method is used to Insert House Information.
        /// </summary>
        /// <param name="oHouseConfiguration"></param>
        public void Insert(HouseConfiguration oHouseConfiguration)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Name", oHouseConfiguration.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Color", oHouseConfiguration.Color, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Motto", oHouseConfiguration.Motto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertHouseConfiguration");
            }
        }

        /// <summary>
        /// This method is used to get Single House Related Information.
        /// </summary>
        /// <param name="aiHouseId"></param>
        /// <returns></returns>
        public HouseConfiguration Get(int aiHouseId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("HouseId", aiHouseId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSingleHouseRecord"))
                {
                    List<HouseConfiguration> lstHouseCnofigs = ReadAllHouses(oSqlDataReader);
                    return lstHouseCnofigs[0];
                }
            }
          
        }
       

        /// <summary>
        /// This method is used to Update House Cofiguration Details.
        /// </summary>
        /// <param name="aoHouseConfiguration"></param>
        public void Update(HouseConfiguration aoHouseConfiguration)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Name", aoHouseConfiguration.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Id", aoHouseConfiguration.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Color", aoHouseConfiguration.Color, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Motto", aoHouseConfiguration.Motto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateHouseConfiguration");
            }
        }

        /// <summary>
        /// This method is used to Delete House Details.
        /// </summary>
        /// <param name="aiHouseId"></param>
        /// <param name="amiSchoolId"></param>
        /// <param name="amiAcademicYearId"></param>
        /// <returns></returns>
        public int Delete(int aiHouseId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("HouseId", aiHouseId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteHouseConfiguration");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// 	This method is used to update student House details.
        /// </summary>
        /// <param name="asXml"> </param>
        /// <param name="aiUpadatedBy"> </param>
        public void UpdateStudentHouseInformation(string asXml)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedBy", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentsHouseInformation");
            }
        }
    }
}
