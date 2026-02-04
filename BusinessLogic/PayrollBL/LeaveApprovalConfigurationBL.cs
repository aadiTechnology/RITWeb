using System.Collections.Generic;
using System.Data;
using DataCommunicator.PayrollDC;
using SchoolEntities.Payroll;

namespace BusinessLogic.PayrollBL
{
    public class LeaveApprovalConfigurationBL
    {
        #region Data Member(s)
        
        private LeaveApprovalConfigurationDC moLeaveApprovalConfigurationDC; 

        #endregion

        #region Constructor(s)

        public LeaveApprovalConfigurationBL()
        {
            moLeaveApprovalConfigurationDC = new LeaveApprovalConfigurationDC();

        }

        public LeaveApprovalConfigurationBL(int aiSchoolId, int aiAcademicYearId)
        {
            moLeaveApprovalConfigurationDC = new LeaveApprovalConfigurationDC(aiSchoolId, aiAcademicYearId);

        }
        
        public LeaveApprovalConfigurationBL(int miSchoolId, int miAcademicYearId, int miUserId)
        {
            moLeaveApprovalConfigurationDC = new LeaveApprovalConfigurationDC(miSchoolId, miAcademicYearId, miUserId);
        }

        #endregion
        
        #region Public Method(s)

        public void Save(LeaveApprovalConfiguration aoLeaveApprovalConfig)
        {
            this.moLeaveApprovalConfigurationDC.Save(aoLeaveApprovalConfig);
        }

        public List<LeaveApprovalConfiguration> GetAll(int aiUserId)
        {
            return this.moLeaveApprovalConfigurationDC.GetAll(aiUserId);
        }

        public void Submit(int aiUserId, bool abIsSubmit)
        {
            this.moLeaveApprovalConfigurationDC.Submit(aiUserId, abIsSubmit);
        }

        public DataTable GetStatus(int aiUserId)
        {
            return moLeaveApprovalConfigurationDC.GetStatus(aiUserId);
        }

        public LeaveApprovalConfiguration Get(int aiConfigId)
        {
            return this.moLeaveApprovalConfigurationDC.Get(aiConfigId);
        }

        public void Delete(int aiConfigId)
        {
            this.moLeaveApprovalConfigurationDC.Delete(aiConfigId);
        } 

        #endregion
    }
}
