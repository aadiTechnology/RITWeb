
// Class Name       :- TaskManagementBL
// Purpose          :- This class is used to manage TaskMaster details.
// Date Of creation :- 6/9/2011
// Author Name      :- 


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using TaskManagementEntities;



namespace BusinessLogic
{

    public class TaskManagementBL
    {
        #region "Data Members"

        private TaskManagementDC moTaskManagementDC = null;
        private DesignationwiseUserTaskDetails moDesignationwiseUserTaskDetails = null;
        private UserAssignedTaskDetails moUserAssignedTaskDetails = null;

        #endregion "Data Members"

        #region "Constructors"

        public TaskManagementBL()
        {
            moDesignationwiseUserTaskDetails = new DesignationwiseUserTaskDetails();
            moUserAssignedTaskDetails = new UserAssignedTaskDetails();
            moTaskManagementDC = new TaskManagementDC();
        }

        public TaskManagementBL(int iSchoolId, int iAcademicYearId)
        {
            moDesignationwiseUserTaskDetails = new DesignationwiseUserTaskDetails();
            moUserAssignedTaskDetails = new UserAssignedTaskDetails();
            moTaskManagementDC = new TaskManagementDC(iSchoolId, iAcademicYearId);
        }

        #endregion "Constructors"

        #region "Properties"


        public DesignationwiseUserTaskDetails DesignationwiseUserTaskDetails
        {
            set { moTaskManagementDC.oDesignationwiseUserTaskDetails = value; }
            get { return moTaskManagementDC.oDesignationwiseUserTaskDetails; }
        }

        public UserAssignedTaskDetails UserAssignedTaskDetails
        {
            set { moTaskManagementDC.oUserAssignedTaskDetails = value; }
            get { return moTaskManagementDC.oUserAssignedTaskDetails; }
        }
        public List<UserAssignedTaskDetails> lstUserAssignedTaskDetails
        {
            set { moTaskManagementDC.olstUserAssignedTaskDetails = value; }
            get { return moTaskManagementDC.olstUserAssignedTaskDetails; }
        }       

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get designation details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiFilter"></param>
        /// <returns></returns>
        public DataTable GetDesignationsDetails(int aiUserId, int aiUserRoleId, int aiFilter)
        {
            return moTaskManagementDC.GetDesignationsDetails(aiUserId, aiUserRoleId, aiFilter);
        }

        /// <summary>
        /// This method is used to get task status details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTaskStatusList()
        {
            return moTaskManagementDC.GetTaskStatusList();
        }

        /// <summary>
        /// This method is used to get task status details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTaskStatusDetails(int aiTaskId, int aiTaskTypeId, int aiTaskDetailsId, int aiTaskAssignerUserId,int aiInsertedById,int aiFlag)
        {
            return moTaskManagementDC.GetTaskStatusDetails(aiTaskId, aiTaskTypeId, aiTaskDetailsId, aiTaskAssignerUserId, aiInsertedById, aiFlag);
        }

        /// <summary>
        /// This method is used to get designationwise user list.
        /// </summary>
        /// <param name="aiDesignationId"></param>
        /// <param name="aiTaskDetailId"></param>
        /// <param name="aiTaskId"></param>
        /// <param name="aiTaskTypeId"></param>
        /// <returns></returns>
        public List<DesignationwiseUserTaskDetails> GetDesignationwiseUserList(int aiDesignationId,int aiTaskDetailId,int aiTaskId,int aiTaskTypeId, int aiInsertedById,int aiAssignFlag)
        {
            return moTaskManagementDC.GetDesignationwiseUserList(aiDesignationId, aiTaskDetailId, aiTaskId, aiTaskTypeId, aiInsertedById, aiAssignFlag);
        }

        /// <summary>
        /// This method is used to save task details.
        /// </summary>
        /// <param name="sUserDetailXML"></param>
        public void SaveUserTaskDetails(string sUserDetailXML)
        {
            moTaskManagementDC.SaveUserTaskDetails(sUserDetailXML);
        }
        
        /****************Inner List View Methods*********/

        /// <summary>
        /// This method is used to get user task details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiDesignationId"></param>
        /// <returns></returns>
        public List<UserAssignedTaskDetails> GetUserTaskDetails(int aiUserId, int aiDesignationId)
        {
            return moTaskManagementDC.GetUserTaskDetails(aiUserId,aiDesignationId);
        }

        /// <summary>
        /// This method is used to get task details.
        /// </summary>
        /// <param name="aiTaskId"></param>
        /// <param name="aiTaskDetailId"></param>
        /// <param name="aiTaskAssignerUserId"></param>
        /// <param name="aiTaskTypeId"></param>
        /// <param name="aiInsertedById"></param>
        public void GetTaskDetails(int aiTaskId, int aiTaskDetailId, int aiTaskAssignerUserId, int aiTaskTypeId, int aiInsertedById)
        {
            moTaskManagementDC.GetTaskDetails(aiTaskId, aiTaskDetailId, aiTaskAssignerUserId, aiTaskTypeId, aiInsertedById);
        }

        public DataTable GetUserDetails(string asXML, string asFilter, int aiFlag)
        {
            return moTaskManagementDC.GetUserDetails(asXML, asFilter, aiFlag);
        }
        #endregion "Public Methods"

    }
}
