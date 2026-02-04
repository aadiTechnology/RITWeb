using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using TaskManagementEntities;
using MasterEntities;

namespace BusinessLogic
{
   public class TaskListBL
   {
        #region "Data Member"
        private TaskListDC moTaskListDC =null;
        #endregion

        #region "Properties"

        public List<TaskTypeMaster> TaskTypeMaster
        {
            get { return moTaskListDC.mlstTaskType; }
            set { moTaskListDC.mlstTaskType = value; }
        }
        public List<TaskStatusMaster> TaskStatusMaster
        {
            get { return moTaskListDC.mlstStatus; }
            set { moTaskListDC.mlstStatus = value; }
        }
        public List<DesignationMaster> DesignationMaster
        {
            get { return moTaskListDC.mlstDesignation; }
            set { moTaskListDC.mlstDesignation = value; }
        }
        
        #endregion

        #region "Constructor"
        public TaskListBL(int iSchoolId, int iAcademicYearID)
        {
            moTaskListDC = new TaskListDC(iSchoolId, iAcademicYearID);

        }
        #endregion

        #region"Public methods"
        /// <summary>
        /// This method is usedto get task types,task status and designation.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiFilter"></param>
        public void GetTaskTypeStatusAndDesignation(int aiUserRoleId, int aiUserId, int aiFilter)
        {
             moTaskListDC.GetTaskTypeStatusAndDesignation(aiUserRoleId,aiUserId,aiFilter);
        }
       /// <summary>
       /// This method is used to Fill 
       /// </summary>
       /// <param name="cmbTaskType"></param>
       /// <param name="cmbStatus"></param>
       /// <param name="cmbDesignation"></param>
        public void FillTaskTypeStatusAndDesignationComboboxes(System.Web.UI.WebControls.DropDownList cmbTaskType, System.Web.UI.WebControls.DropDownList cmbStatus,System.Web.UI.WebControls.DropDownList cmbDesignation)
        {
            cmbTaskType.DataSource = TaskTypeMaster;
            cmbTaskType.DataTextField = "TaskType";
            cmbTaskType.DataValueField = "TaskTypeId";
            cmbTaskType.DataBind();
            cmbTaskType.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "-- Select --" });

            cmbStatus.DataSource = TaskStatusMaster;
            cmbStatus.DataTextField = "StatusName";
            cmbStatus.DataValueField = "TaskStatusId";
            cmbStatus.DataBind();
            cmbStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "-- Select --" });

            cmbDesignation.DataSource = DesignationMaster;
            cmbDesignation.DataTextField = "Designation";
            cmbDesignation.DataValueField = "DesignationId";
            cmbDesignation.DataBind();
            cmbDesignation.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "-- Select --" });


        }
        /// <summary>
        /// This method is used to get task list 
        /// </summary>
        /// <param name="AssignedToUserId"></param>
        /// <param name="aiFlag"></param>
        /// <param name="aiTaskTypeId"></param>
        /// <param name="aiTaskStatusId"></param>
        /// <param name="aiDesignationId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<UserTaskList> GetListViewDetails(string asXML, string asSortFilter, string asSortOrder)
        {
            return moTaskListDC.GetListViewDetails(asXML, asSortFilter, asSortOrder);
        }
        /// <summary>
        /// This method is used to get designation wise user list.
        /// </summary>
        /// <param name="aiDesignationId"></param>
        /// <returns></returns>
        public DataTable GetDesignationwiseResourceList(int aiDesignationId, int aiFlag)
        {
            return moTaskListDC.GetDesignationwiseResourceList(aiDesignationId, aiFlag);
        }
       /// <summary>
       /// This method is used to get all designations.
       /// </summary>
       /// <param name="aiUserId"></param>
       /// <param name="aiUserRoleId"></param>
       /// <param name="aiFilter"></param>
       /// <returns></returns>
        //public List<DesignationMaster> GetDesignationsDetails(int aiUserId, int aiUserRoleId, int aiFilter)
        //{
        //    return moTaskListDC.GetDesignationsDetails(aiUserId, aiUserRoleId, aiFilter);
        //}
        /// <summary>
       /// This method is used to delete task details.
       /// </summary>
       /// <param name="aiTaskId"></param>
       /// <param name="aiTaskStatusId"></param>
       /// <param name="aiAssignerToUserId"></param>
       /// <param name="aiTaskTypeId"></param>
       /// <returns></returns>
         public void DeleteTaskDetails(int aiTaskId, int aiTaskStatusId, int aiAssignerToUserId, int aiTaskTypeId,int aiTaskDetailsId)
        {
             moTaskListDC.DeleteTaskDetails(aiTaskId, aiTaskStatusId, aiAssignerToUserId, aiTaskTypeId,aiTaskDetailsId);
        }

        #endregion
    }
}
