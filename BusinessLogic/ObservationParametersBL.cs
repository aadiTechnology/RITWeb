using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    public class ObservationParametersBL
    {
        #region Data Member(s)
        
        private ObservationParametersDC moObservationParametersDC; 

        #endregion 

        #region Constructor(s)

        public ObservationParametersBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moObservationParametersDC = new ObservationParametersDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        } 

        #endregion
        
        #region Public Method(s)

        public List<ObservationParameters> GetAll(int aiSkillId)
        {
            return this.moObservationParametersDC.GetAll(aiSkillId, 0);
        }

        public List<ObservationParameters> GetSkills(int aiSchoolId, int aiStandardid, int aiAcademicYearId)
        {
            return this.moObservationParametersDC.GetSkills(aiSchoolId, aiStandardid, aiAcademicYearId);
        }

        public void Save(ObservationParameters oObservationParameters)
        {
            this.moObservationParametersDC.Save(oObservationParameters);
        }

        public ObservationParameters Get(int aiSkillId, int aiParameterId)
        {
            List<ObservationParameters> lstObservationParameters = this.moObservationParametersDC.GetAll(aiSkillId, aiParameterId);
            return lstObservationParameters[0];
        }

        public void Delete(int aiParamterId)
        {
            moObservationParametersDC.Delete(aiParamterId);
        }

        public void Submit(int aiStandardId, int aiSkillId, bool abIsSubmit)
        {
            moObservationParametersDC.Submit(aiStandardId, aiSkillId, abIsSubmit);
        } 

        #endregion
    }
}
