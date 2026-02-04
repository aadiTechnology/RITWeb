// Class Name       :- LearningOutcomeConfigMasterBL
// Purpose          :- This class is used to manage learnig outcomes.
// Date Of creation :- 5/24/2011
// Author Name      :- Vipul Jadhav
using System.Collections.Generic;
using DataCommunicator;
using XseedReportEntities;
using MasterEntities;

namespace BusinessLogic
{
    public class LearningOutcomeConfigMasterBL
    {
        #region "Data Members"

        private LearningOutcomeConfigMasterDC moLearningOutcomeConfigMasterDC;

        #endregion "Data Members"

        #region "Properties"

        public LearningOutcomeConfigMaster LearningOutcomeConfigMaster
        {
            get { return moLearningOutcomeConfigMasterDC.moLearningOutcomeConfigMaster; }
            set { moLearningOutcomeConfigMasterDC.moLearningOutcomeConfigMaster = value; }
        }

        public bool IsSubmitted
        {
            get { return moLearningOutcomeConfigMasterDC.bIsSubmitted; }
            set { moLearningOutcomeConfigMasterDC.bIsSubmitted = value; }
        }

        public bool GradeSubmitStatus
        {
            get { return moLearningOutcomeConfigMasterDC.bGradeSubmitStatus; }
            set { moLearningOutcomeConfigMasterDC.bGradeSubmitStatus = value; }
        }

        public LearningOutcomesSubmitStatus LearningOutcomesSubmitStatus
        {
            get { return moLearningOutcomeConfigMasterDC.moLearningOutcomesSubmitStatus; }
            set { moLearningOutcomeConfigMasterDC.moLearningOutcomesSubmitStatus = value; }
        }

        #endregion "Properties"

        #region "Constructors"

        public LearningOutcomeConfigMasterBL()
        {
            moLearningOutcomeConfigMasterDC = new LearningOutcomeConfigMasterDC();
        }

        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to insert the learning outcome details.
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            return moLearningOutcomeConfigMasterDC.Insert();
        }

        /// <summary>
        /// This method is used to update the learning outcome details.
        /// </summary>
        public void Update()
        {
            moLearningOutcomeConfigMasterDC.Update();
        }

        /// <summary>
        /// This method is used to get learning outcome details.
        /// </summary>
        /// <param name="asSortOrder"></param>
        /// <returns></returns>
        public List<LearningOutcomeConfigMaster> GetAll(string asSortOrder)
        {
            return moLearningOutcomeConfigMasterDC.GetAll(asSortOrder);
        }

        /// <summary>
        /// This function is used to delete the learning outcome details.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        public void Delete(int aiLearningOutcomeConfigId, int aiUserId)
        {
            moLearningOutcomeConfigMasterDC.Delete(aiLearningOutcomeConfigId,aiUserId);
        }

        /// <summary>
        ///  This method is used to load learning outcome details.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        public void Load(int aiLearningOutcomeConfigId)
        {
            moLearningOutcomeConfigMasterDC.Load(aiLearningOutcomeConfigId);
        }

        /// <summary>
        /// This method is used to save the learning outcome submit status.
        /// </summary>
        public void SaveLearningOutcomesSubmitStatus()
        {
            moLearningOutcomeConfigMasterDC.SaveLearningOutcomesSubmitStatus();
        }

        /// <summary>
        /// This method is used to copy learning outcomes.
        /// </summary>
        /// <param name="aiTargetAssessmentId"></param>
        /// <param name="aiTargetSubjectSectionId"></param>
        public void Copy(int aiTargetAssessmentId, int aiTargetSubjectSectionId)
        {
            moLearningOutcomeConfigMasterDC.Copy(aiTargetAssessmentId, aiTargetSubjectSectionId);
        }

        /// <summary>
        /// This method is used to get teacher associated standards.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<StandardMaster> GetTeacherAssociatedStandards(int aiTeacherId, int aiAcademicYearId, int aiSchoolId)
        {
            return moLearningOutcomeConfigMasterDC.GetTeacherAssociatedStandards(aiTeacherId, aiAcademicYearId, aiSchoolId);
        }

        /// <summary>
        /// This method is used to check dependency of learning outcome with grade.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
        public static bool Dependent(int aiLearningOutcomeConfigId, int aiSchoolID, int aiAcademicYearID)
        {
            return LearningOutcomeConfigMasterDC.Dependent(aiLearningOutcomeConfigId, aiSchoolID, aiAcademicYearID);
        }

        #endregion "Public Methods"

    }
}
