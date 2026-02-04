// Class Name       :- GradeMasterBL
// Purpose          :- This class is used to manage Grade Master details.
// Date Of creation :- 
// Author Name      :- Sachin
using System.Collections.Generic;
using DataCommunicator;
using StaffPerformanceEntity;
using System.Data;

namespace BusinessLogic
{
    public class PerformanceSkillBL
    {
        #region "Data Member"

        private PerformanceSkillDC moPerformanceSkillDC;

        #endregion

        #region "Constructors"

        public PerformanceSkillBL(int aiSchoolId, int aiUpdatedById)
        {
            this.moPerformanceSkillDC = new PerformanceSkillDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all skill details.
        /// </summary>
        /// <returns></returns> 
       public List<PerformanceSkill> GetAll()
        {
            return this.moPerformanceSkillDC.GetAll();
        }

       /// <summary>
       /// This method is used to get Input Types.
       /// </summary>
       /// <returns></returns>
       public List<InputType> GetInputTypes()
       {
           return this.moPerformanceSkillDC.GetInputTypes();
       }

       /// <summary>
       /// This method is used to get all performance form type.
       /// </summary>
       /// <returns></returns>
       public List<FormType> GetFormTypeDetails()
       {
           return this.moPerformanceSkillDC.GetFormTypeDetails();
       }

        /// <summary>
        /// This method is used to Insert and Update skill details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
       public void Insert(string asXml)
        {
            this.moPerformanceSkillDC.Insert(asXml);
        }

        #endregion "Public Method"
    }
}
