// Class Name       :- StaffLeavesDC
// Purpose          :- This class is used to manage StaffLeaves details.
// Date Of creation :- 11/7/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MasterEntities;
using PayrollEntities;

namespace DataCommunicator
{
    public class StaffLeavesDC
    {
        #region Data Member(s)

        private ConfiguredLeaves moConfiguredLeave;
        private int miSchoolId;
        public int miUpdatedById;
        
        #endregion

        #region Constructor(s)

        public StaffLeavesDC()
        {
        }

        public StaffLeavesDC(int aiSchoolId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)

        public ConfiguredLeaves ConfiguredLeave
        {
            set { moConfiguredLeave = value; }
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to save leave configuration.
        /// </summary>
        /// <param name="aiOriginalId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable Save(int aiOriginalId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moConfiguredLeave.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moConfiguredLeave.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveXML", moConfiguredLeave.LeaveXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("OriginalId", aiOriginalId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_InsertStaffLeavesDetails",true);
            }
        }
        
        /// <summary>
        /// This method is used to return all configured leaves.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetAll(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetConfiguredLeaves");
            }
        }

        /// <summary>
        /// This method is used to return basic leave details.
        /// </summary>
        /// <returns></returns>
        public List<BasicLeaveDetails> GetBasicLeaveDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBasicLeaveConfigDetails"))
                return GetBasicLeaveDetail(oSqlDataReader);                
            }
        }

        /// <summary>
        /// This method is used to return basic leave configuration.
        /// </summary>
        /// <returns></returns>
        public List<BasicLeaveConfiguration> GetBasicLeaveConfigs(int aiBasicLeaveConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BasicLeaveCOnfigId", aiBasicLeaveConfigId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBasicLeaveConfigDetails"))
                {
                    List<BasicLeaveDetails> lstBasicLeaveDetails = GetBasicLeaveDetail(oSqlDataReader);
                    oSqlDataReader.NextResult();
                    return FillBasicLeaveConfigDetails(oSqlDataReader, lstBasicLeaveDetails);
                }
            }
        }

        /// <summary>
        /// This method is used to return basic leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<BasicLeaveDetails> GetBasicLeaveDetail(SqlDataReader aoSqlDataReader)
        {
            List<BasicLeaveDetails> lstBasicLeaves = new List<BasicLeaveDetails>();
            while (aoSqlDataReader.Read())
            {
                BasicLeaveDetails oBasicLeaveConfiguration = new BasicLeaveDetails
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    BasicLeaveConfigId = Convert.ToInt32(aoSqlDataReader["BasicLeaveConfigId"]),
                    Leave = new ConfiguredLeaves
                    {
                        LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                        LeaveName = Convert.ToString(aoSqlDataReader["LeaveName"])
                    },
                    BasicLeaves = Convert.ToDecimal(aoSqlDataReader["BasicLeaves"]),
                    AccumulateLeaves = Convert.ToDecimal(aoSqlDataReader["AccumulateLeaves"])
                };
                lstBasicLeaves.Add(oBasicLeaveConfiguration);
            }
            return lstBasicLeaves;
        }

        /// <summary>
        /// This method is used to fill basic leave configuration in entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <param name="alstBasicLeaveDetails"></param>
        /// <returns></returns>
        private List<BasicLeaveConfiguration> FillBasicLeaveConfigDetails(SqlDataReader aoSqlDataReader, List<BasicLeaveDetails> alstBasicLeaveDetails)
        {
            List<BasicLeaveConfiguration> lstBasicLeaves = new List<BasicLeaveConfiguration>();
            while (aoSqlDataReader.Read())
            {
                BasicLeaveConfiguration oBasicLeaveConfiguration = new BasicLeaveConfiguration
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    IsAccumulationMonth = Convert.ToBoolean(aoSqlDataReader["IsAccumulationMonth"]),
                    StaffGroups = new StaffGroupsEntity
                    {
                        StaffGroupsId = Convert.ToInt32(aoSqlDataReader["StaffGroupsId"]),
                        StaffGroupsName = Convert.ToString(aoSqlDataReader["StaffGroupsName"])
                    },
                    Month = new MonthMaster
                    {
                        MonthId = Convert.ToInt32(aoSqlDataReader["MonthId"]),
                        MonthAbbreviation = Convert.ToString(aoSqlDataReader["MonthAbbreviation"]),
                    },                    
                    Leaves = alstBasicLeaveDetails.Where(bld => bld.BasicLeaveConfigId == Convert.ToInt32(aoSqlDataReader["Id"]) || bld.BasicLeaveConfigId == 0).ToList()
                };
                lstBasicLeaves.Add(oBasicLeaveConfiguration);
            }
            return lstBasicLeaves;
        }

        /// <summary>
        /// This method is used to delete configuration.
        /// </summary>
        /// <param name="aiId"></param>
        public void DeleteBasicLeaveConfig(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteBasicLeaveConfiguration");
            }
        }
        
        /// <summary>
        /// This method is used to save basic leave details.
        /// </summary>
        /// <param name="aoBasicLeaveConfiguration"></param>
        public void SaveBasicLeaveConfig(BasicLeaveConfiguration aoBasicLeaveConfiguration)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoBasicLeaveConfiguration.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aoBasicLeaveConfiguration.StaffGroups.StaffGroupsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aoBasicLeaveConfiguration.Month.MonthId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("IsAccumulationMonth", aoBasicLeaveConfiguration.IsAccumulationMonth, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("LeaveXml", aoBasicLeaveConfiguration.LeaveXml, SqlDbType.Xml);                
                oSQLServerDbUtility.AddParameter("UpdatedById", aoBasicLeaveConfiguration.UpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveBasicLeaveConfiguration");
            }
        }

        /// <summary>
        /// This method is used to apply changes for all the users of staff group for selected year.
        /// </summary>
        /// <param name="aiStaffGroupsId"></param>
        /// <param name="abUpdateExisting"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiLeaveSeperaterDay"></param>
        public void ApplyToAllUsers(int aiStaffGroupsId, bool abUpdateExisting, int aiLeaveSeperaterDay)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",miSchoolId,SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupsId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("LeaveSeperaterDay", aiLeaveSeperaterDay, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("UpdateExistng", abUpdateExisting, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ApplyBasicLeaveToAllUsers");
            }
        }

        #endregion
    }
}
