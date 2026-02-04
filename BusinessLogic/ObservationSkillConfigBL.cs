using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    public class ObservationSkillConfigBL
    {
        #region Data Member(s)

        private ObservationSkillConfigDC moObservationSkillConfigDC;

        #endregion

        #region Constructor(s)

        public ObservationSkillConfigBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moObservationSkillConfigDC = new ObservationSkillConfigDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// this method is used to fill subject dropdown
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="miStandardid"></param>
        /// <returns></returns>

        public List<ObservationSkillConfig> GetAllSubjects(int miSchoolId, int miAcademicYearId, int miStandardid)
        {
            return moObservationSkillConfigDC.GetAllSubjects(miSchoolId, miAcademicYearId, miStandardid);
        }
        /// <summary>
        /// This method is used to return all available Skill.
        /// </summary>
        /// <param name="aiStandardid"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>

        public List<ObservationSkillConfig> GetAll(int aiStandardid, int aiSubjectId, string asFilter)
        {
            return moObservationSkillConfigDC.GetAll(aiStandardid, aiSubjectId, asFilter);
        }
        /// <summary>
        /// This method is used to save Skill details.
        /// </summary>
        /// <param name="oObservationSkillConfig"></param>

        public void Save(ObservationSkillConfig oObservationSkillConfig)
        {
            this.moObservationSkillConfigDC.Save(oObservationSkillConfig);
        }

        /// <summary>
        /// This method is used to delete Skill details
        /// </summary>
        /// <param name="aoObservationSkillConfig"></param>
        /// <param name="aiConfigId"></param>
        public void Delete(int aoObservationSkillConfig, int aiConfigId)
        {
            this.moObservationSkillConfigDC.Delete(aoObservationSkillConfig, aiConfigId);
        } 

        #endregion
    }
}
