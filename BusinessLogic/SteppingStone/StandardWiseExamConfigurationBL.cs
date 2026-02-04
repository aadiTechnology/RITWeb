using System.Collections.Generic;
using DataCommunicator;
using StandardWiseExamConfigurationEntities;

namespace BusinessLogic
{
    public class StandardWiseExamConfigurationBL
    {
        #region -- MEMBER(s) --
       
        private StandardWiseExamConfigurationDC moStandardWiseExamConfigurationDC;

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR(s) --
       
        public StandardWiseExamConfigurationBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moStandardWiseExamConfigurationDC = new StandardWiseExamConfigurationDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --

        /// <summary>
        /// This method return List that contain StandardWiseExamConfiguration object 
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<StandardWiseExamConfiguration> GetExamsForStandard(int aiStandardId)
        {
            return this.moStandardWiseExamConfigurationDC.GetExamsForStandard(aiStandardId);
        }

        /// <summary>
        /// This method use to save Standard wise exam details
        /// </summary>
        /// <param name="asStandardWiseExamDetailsXml"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiInsertedById"></param>
        public void Save(string asStandardWiseExamDetailsXml, int aiStandardId, int aiInsertedById)
        {
            this.moStandardWiseExamConfigurationDC.Save(asStandardWiseExamDetailsXml, aiStandardId, aiInsertedById);
        }
       
        /// <summary>
        /// This method return list containing exam status for that school
        /// </summary>
        /// <returns>List<ExamStatusConfiguration></returns>
        public List<ExamStatusConfiguration> GetSchoolwiseExamStatusConfiguration()
        {
            return this.moStandardWiseExamConfigurationDC.GetSchoolwiseExamStatusConfiguration();
        }
        
        /// <summary>
        /// This method return ExamStatusConfiguration object 
        /// </summary>
        /// <param name="aiExamStatusId"></param>
        /// <returns></returns>
        public ExamStatusConfiguration GetExamStatusForSelectedStatusName(int aiExamStatusId)
        {
            return this.moStandardWiseExamConfigurationDC.GetExamStatusForSelectedStatusName(aiExamStatusId);
        }
       
        /// <summary>
       /// This method use to save exam status configuartion for selected status name
       /// </summary>
       /// <param name="oExamStatusConfiguration"></param>
        public void UpdateExamStatusConfiguration(ExamStatusConfiguration aoExamStatusConfiguration)
        {
            this.moStandardWiseExamConfigurationDC.UpdateExamStatusConfiguration(aoExamStatusConfiguration);
        }

        #endregion -- PUBLIC METHOD(s) --

    }
}
