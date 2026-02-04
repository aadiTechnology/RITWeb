using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using Utility;
using SchoolEntities;
using TaskManagementEntities;

namespace TaskManagementEntities
{
    public class DesignationwiseUserTaskDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int IsSelected { get; set; }
        public int TaskStatusId { get; set; }
        public int IsLoggedUser { get; set; }
    }

    public class UserAssignedTaskDetails
    {
        public int TaskDetailId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDetails { get; set; }
        public string Comments { get; set; }
        public int DesignationId { get; set; }
        public int TaskAssignerId { get; set; }
        public string TaskAssignerName { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public DateTime BufferDate { get; set; }
        public string BufferTime { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStatusId { get; set; }
        public DateTime TaskCompletedDate { get; set; }
        public int TaskCompletedCount { get; set; }
        public string TaskCompletedStTime { get; set; }
        public string TaskCompletedEndTime { get; set; }
        public int IsLoggedUser { get; set; }

    }
    public class WorkFlowRoleConfigurationDetail
    {
        public int WorkFlowLevelId { get; set; }
        public int AssignedToDesignationId { get; set; }
        public int AssignedByDesignationId { get; set; }
        public int SchoolId { get; set; }
        public string Is_Deleted { get; set; }
        public int InsertedById { get; set; }
        public string Designation { get; set; }
        public Constants.Action Action { get; set; }
    }

    public class TaskTypeMaster
    {
        public int TaskTypeId { get; set; }
        public string TaskType { get; set; }
    }

    public class TaskStatusMaster
    {
        public int TaskStatusId { get; set; }
        public string StatusName { get; set; }
    }
    public class UserTaskList
    {
        public string UserName { get; set; }
        public string TaskName { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TaskType { get; set; }
        public string Comment { get; set; }

        public int AssignedToUserId { get; set; }
        public int TaskAssignerUserId { get; set; }
        public int TaskId { get; set; }
        public int TaskStatusId { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskDetailsId { get; set; }
    }
}
