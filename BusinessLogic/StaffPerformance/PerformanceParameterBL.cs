/*File Name - PerformanceParameterBL.cs
 * Created Date - 17 Sept 2013
 * Created By - Sachin
 * Description - This class is used to communicate with data access layer.
 */
using System.Collections.Generic;
using DataCommunicator;
using StaffPerformanceEntity;

namespace BusinessLogic
{
    public class PerformanceParameterBL
    {
        #region Data Member(s)
        
        private PerformanceParameterDC moPerformanceParameterDC; 

        #endregion

        #region Constructor(s)
        
        public PerformanceParameterBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moPerformanceParameterDC = new PerformanceParameterDC(aiSchoolId, aiUpdatedById);
        } 

        #endregion

        #region Method(s)
        
        /// <summary>
        /// This method is used to return all available parameters.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="aiPerformanceParameterId"></param>
        /// <returns></returns>
        public List<PerformanceParameter> GetAll(int aiYear, int aiSkillId,int iFormTypeId, int aiPerformanceParameterId = 0)
        {
            return this.moPerformanceParameterDC.GetAll(aiYear, aiSkillId, iFormTypeId, aiPerformanceParameterId);
        }

        /// <summary>
        /// This method is used to save parametere details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
        public void Save(PerformanceParameter aoPerformanceParameter)
        {
            this.moPerformanceParameterDC.Save(aoPerformanceParameter);
        }

        /// <summary>
        /// This method is used to delete parameter details.
        /// </summary>
        /// <param name="aiPerformanceParameterId"></param>
        /// <param name="aiConfigId"></param>
        public void Delete(int aiPerformanceParameterId, int aiConfigId)
        {
            this.moPerformanceParameterDC.Delete(aiPerformanceParameterId, aiConfigId);
        }

        /// <summary>
        /// This method is used to submit / un submit parameters of selected year and skills.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="abIsSubmit"></param>
        public void Submit(int aiYear, int aiSkillId, bool abIsSubmit)
        {
            this.moPerformanceParameterDC.Submit(aiYear, aiSkillId, abIsSubmit);
        } 

        #endregion
    }
}
