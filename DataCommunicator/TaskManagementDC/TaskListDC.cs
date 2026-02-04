using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TaskManagementEntities;
using Utility;
using SchoolEntities;
using MasterEntities;


namespace DataCommunicator
{
    public class TaskListDC
    {
        #region "Data member"
        int miSchoolId = 0;
        int miAcademicYearId = 0;
        
        public List<TaskTypeMaster> mlstTaskType = new List<TaskTypeMaster>();
        public List<TaskStatusMaster> mlstStatus = new List<TaskStatusMaster>();
        public List<DesignationMaster> mlstDesignation = new List<DesignationMaster>();
        
        #endregion

        #region "Constructor"
        public TaskListDC()
        { 
        
        }
        public TaskListDC(int iSchoolId, int iAcademicYearID)
        {
            miSchoolId = iSchoolId;
            miAcademicYearId = iAcademicYearID;
        }
        #endregion
        #region "Public methods"
        /// <summary>
        /// This method is usedto get task types,task status and designation.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiFilter"></param>
        public void GetTaskTypeStatusAndDesignation(int aiUserRoleId, int aiUserId, int aiFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", aiFilter, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaskTypeStatusAndDesignationDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        SetTaskStatusList(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetAllTaskTyes(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetDesignation(oSqlDataReader);
                    }
                }
            }
        }
        /// <summary>
        /// This method is used to set properties.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<TaskTypeMaster> SetAllTaskTyes(SqlDataReader aoSqlDataReader)
        {
            TaskTypeMaster oTaskTypeMaster = null; 
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oTaskTypeMaster = new TaskTypeMaster
                    {
                        TaskTypeId =Convert.ToInt32(aoSqlDataReader["TaskTypeId"]),
                        TaskType = aoSqlDataReader["TaskType"].ToString()
                    };
                    mlstTaskType.Add(oTaskTypeMaster);
                }
            }
            return mlstTaskType;
        }
        /// <summary>
       /// This method is used to set  properties.
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
        public List<TaskStatusMaster> SetTaskStatusList(SqlDataReader aoSqlDataReader)
        {
            TaskStatusMaster oTaskStatusMaster = null;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oTaskStatusMaster = new TaskStatusMaster
                    {
                        TaskStatusId = Convert.ToInt32(aoSqlDataReader["TaskStatusId"]),
                        StatusName = aoSqlDataReader["StatusName"].ToString()
                    };
                    mlstStatus.Add(oTaskStatusMaster);
                }
            }
            return mlstStatus;
        }
        /// <summary>
        /// This method is used to get users.
        /// </summary>
        /// <param name="aiDesignationId"></param>
        /// <param name="aiFlag"></param>
        /// <returns></returns>
        public DataTable GetDesignationwiseResourceList(int aiDesignationId, int aiFlag)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DesignationId", aiDesignationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", aiFlag, SqlDbType.Int);
                DataTable oDT = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserTaskDetailsAsPerDesignation");
                return oDT;
            };
        }
        /// <summary>
        /// This method is used to set values to properties.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public List<UserTaskList> SetListViewDetails(SqlDataReader aoSqlDataReader)
        {
            UserTaskList oUserTaskList = null;
            List<UserTaskList> lstTaskDetails = new List<UserTaskList>();
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oUserTaskList = new UserTaskList
                    {
                        UserName = aoSqlDataReader["UserName"].ToString(),
                        TaskName = aoSqlDataReader["TaskName"].ToString(),
                        StatusName = aoSqlDataReader["StatusName"].ToString(),
                        TaskId = Convert.ToInt32(aoSqlDataReader["TaskId"]),
                        TaskStatusId = Convert.ToInt32(aoSqlDataReader["TaskStatusId"]),
                        AssignedToUserId=Convert.ToInt32(aoSqlDataReader["TashAssignedToId"]),
                        TaskAssignerUserId = Convert.ToInt32(aoSqlDataReader["TaskAssignerUserId"]),
                        TaskDetailsId = Convert.ToInt32(aoSqlDataReader["TaskDetailsId"]),
                        TaskTypeId = Convert.ToInt32(aoSqlDataReader["TaskTypeId"]),
                       
                        TaskType = aoSqlDataReader["TaskType"].ToString(),
                        StartTime = aoSqlDataReader["StartTime"].ToString(),
                        StartDate = Convert.ToDateTime(aoSqlDataReader["StartDate"]),
                        EndDate = Convert.ToDateTime(aoSqlDataReader["EndDate"]),
                        EndTime = aoSqlDataReader["EndTime"].ToString()

                    };
                    lstTaskDetails.Add(oUserTaskList);
                }
            }
            return lstTaskDetails;
        }
        /// <summary>
        /// This method is used to get list view details.
        /// </summary>
        /// <param name="asXML"></param>
        /// <returns></returns>
        public List<UserTaskList> GetListViewDetails(string asXML,string asSortFilter,string asSortOrder)
        {
            List<UserTaskList> lstTaskDetails = new List<UserTaskList>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FilterXMl", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SortFilter",asSortFilter,SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder",asSortOrder,SqlDbType.NVarChar);
                using(SqlDataReader SqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaskDetails"))
                return SetListViewDetails(SqlDataReader);
            }
        }
        /// <summary>
        /// This method is used to delete task details.
        /// </summary>
        /// <param name="aiTaskId"></param>
        /// <param name="aiTaskStatusId"></param>
        /// <param name="aiAssignedToUserId"></param>
        /// <param name="aiTaskTypeId"></param>
        /// <returns></returns>
        public void DeleteTaskDetails(int aiTaskId, int aiTaskStatusId, int aiAssignedToUserId, int aiTaskTypeId, int aiTaskDetailsId)
        {
           
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TaskTypeId",aiTaskTypeId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TaskStatusId",aiTaskStatusId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TaskId",aiTaskId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssignedToUserId", aiAssignedToUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TaskDetailsId", aiTaskDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteTaskDetails");               
            }

        }
        /// <summary>
        /// This method is used to set properties.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<DesignationMaster> SetDesignation(SqlDataReader aoSqlDataReader)
        {
            DesignationMaster oDesignationMaster = null;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oDesignationMaster = new DesignationMaster
                    {
                        DesignationId = Convert.ToInt32(aoSqlDataReader["Value_Member"]),
                        Designation = aoSqlDataReader["Display_Member"].ToString()
                    };
                    mlstDesignation.Add(oDesignationMaster);
                }
            }
            return mlstDesignation;
        }
        #endregion
    }
}
