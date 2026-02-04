using System.Collections.Generic;
using DataCommunicator;
using TaskManagementEntities;
using MasterEntities;
namespace BusinessLogic
{
    public class WorkFlowRoleConfigurationBL
    {
        #region "Data Member"
        private WorkFlowRoleConfigurationDC moWorkFlowRoleConfigurationDetailDC = null;
        #endregion

        #region "Constructor"

        public WorkFlowRoleConfigurationBL()
        {
            moWorkFlowRoleConfigurationDetailDC = new WorkFlowRoleConfigurationDC();
        }
        public WorkFlowRoleConfigurationBL(int aiSchoolId,int aiAcademicYearId)
        {
            moWorkFlowRoleConfigurationDetailDC = new WorkFlowRoleConfigurationDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// This method is used to get all designations 
        /// </summary>
        /// <returns></returns>
        public List<DesignationMaster> GetAllDesignations()
        {
            return moWorkFlowRoleConfigurationDetailDC.GetAllDesignations();
        }
        /// <summary>
        /// This method is used to get all work flow configuration details 
        /// </summary>
        /// <param name="iAssignedByDesgId"></param>
        /// <returns></returns>
        public List<WorkFlowRoleConfigurationDetail> GetAllAssigneeList(int iAssignedByDesgId)
        {
            return moWorkFlowRoleConfigurationDetailDC.GetAllAssigneeList(iAssignedByDesgId);
        }
        /// <summary>
        /// This method is used to insert work flow configuration detail
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        public void SaveWorkFlowConfigDetails(string asXml, int aiInsertedById)
        {
            moWorkFlowRoleConfigurationDetailDC.SaveWorkFlowConfigDetails(asXml, aiInsertedById);   
        }
        #endregion
    }
}
