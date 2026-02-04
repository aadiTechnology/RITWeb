using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TaskManagementEntities;
using MasterEntities;

namespace DataCommunicator
{
    public class WorkFlowRoleConfigurationDC
    {
        #region "Data Members"
  
        int miSchoolId = 0;
        int miAcademicYearId = 0;
        WorkFlowRoleConfigurationDetail moWorkFlowRoleConfigurationDetail = null;
        private List<DesignationMaster> lstGradeDetails = new List<DesignationMaster>();
        DesignationMaster oDesignationMaster=null;
        
        #endregion

        #region "Constructor"

        public WorkFlowRoleConfigurationDC()
        {
            moWorkFlowRoleConfigurationDetail = new WorkFlowRoleConfigurationDetail();
        }
        
        public WorkFlowRoleConfigurationDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            moWorkFlowRoleConfigurationDetail = new WorkFlowRoleConfigurationDetail();
        }
       #endregion

        #region "public Methods"
        /// <summary>
        /// This method is used to get all designation
        /// </summary>
        /// <returns></returns>
        public List<DesignationMaster> GetAllDesignations()
        {
            List<WorkFlowRoleConfigurationDetail> oDesgList = new List<WorkFlowRoleConfigurationDetail>();
            string sSelectStatement = "SELECT Distinct DesignationId,UserRoleId,Designation " +
                                           "FROM udf_GetAllDesignationwiseUserDetails " +
                                           "(" +miSchoolId+ "," +miAcademicYearId+ ",0,null)"+
                                           "  ORDER BY UserRoleId,DesignationId ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                return SetDesignationDetails(oSqlDataReader);
            }   
        }
        /// <summary>
        /// This method is used to set properties.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<DesignationMaster> SetDesignationDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                oDesignationMaster = new DesignationMaster
                {
                    DesignationId = Convert.ToInt32(aoSqlDataReader["DesignationId"]),
                    Designation = Convert.ToString(aoSqlDataReader["Designation"])
                };
                lstGradeDetails.Add(oDesignationMaster);
            }
            return lstGradeDetails;

        }
        /// <summary>
        /// This method is used to set ListView details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<WorkFlowRoleConfigurationDetail> SetListViewDetails(SqlDataReader aoSqlDataReader)
        {
           List<WorkFlowRoleConfigurationDetail> oDesgList=new List<WorkFlowRoleConfigurationDetail>();
           while (aoSqlDataReader.Read())
            {
                WorkFlowRoleConfigurationDetail oWorkFlowConfigurationDetail =new WorkFlowRoleConfigurationDetail
                {
                    WorkFlowLevelId = Convert.ToInt32(aoSqlDataReader["WorkFlowLevelId"]),
                    AssignedByDesignationId = Convert.ToInt32(aoSqlDataReader["AssignedByDesignationId"]),
                    AssignedToDesignationId = Convert.ToInt32(aoSqlDataReader["AssignedToDesignationId"]),
                    Designation = aoSqlDataReader["Designation"].ToString(),
                    Is_Deleted = aoSqlDataReader["Is_Deleted"].ToString()
                };
                oDesgList.Add(oWorkFlowConfigurationDetail);
            }
            return oDesgList;
        }
        /// <summary>
        /// This method is used to get designations of assignee. 
        /// </summary>
        /// <param name="iAssignedByDesgId"></param>
        /// <returns></returns>
        public List<WorkFlowRoleConfigurationDetail> GetAllAssigneeList(int iAssignedByDesgId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssignByDesignationId",iAssignedByDesgId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId",miAcademicYearId,SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetWorkFlowConfigurationDetails"))
                return SetListViewDetails(oSqlDataReader);
            }  
        }
        /// <summary>
        /// This method is used to set proprties.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public void SaveWorkFlowConfigDetails(string asXml, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WorkFlowConfigurationXML", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertWorkFlowConfigurationDetails");
            };
        }
        #endregion
    }
}
