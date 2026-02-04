// Class Name       :- AssemblyDetailsBL
// Purpose          :- This class is used to manage Assembly details.
// Date Of creation :- 13/02/2016
// Author Name      :- Dnyaneshwar Shinde.

using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using StaffPerformanceEntity;
using System.Data;
using System;

namespace BusinessLogic
{
    public class AssemblyDetailsBL
    {
        #region Data Member(s)

        private AssemblyDetailsDC moAssemblyDetailsDC;

        #endregion

        #region Constructor(s)
        
        public AssemblyDetailsBL()
        {
            this.moAssemblyDetailsDC = new AssemblyDetailsDC();
        }

        public AssemblyDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moAssemblyDetailsDC = new AssemblyDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Property(s)

        public List<AssemblyQuestions> AssemblyQuestions
        {
            get { return this.moAssemblyDetailsDC.AssemblyQuestions; }
        }

        public List<AssemblyAnswers> AssemblyAnswers
        {
            get { return this.moAssemblyDetailsDC.AssemblyAnswers; }
        }

        public List<StandardDetails> StandardDetails
        {
            get { return this.moAssemblyDetailsDC.StandardDetails; }
        }

        public ButtonState ButtonStates
        {
            get { return this.moAssemblyDetailsDC.ButtonStates; }
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to return All Assembly details.
        /// </summary>    
        /// <param name="sDate"></param>
        /// <returns></returns>
        public List<AssemblyDetails> GetAllAssemblyDetails(DateTime asDate)
        {
            return this.moAssemblyDetailsDC.GetAllAssemblyDetails(asDate);
        }

        /// <summary>
        /// This method is used to save Assembly details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="dtDate"></param>
        /// <param name="aiAssemblyId"></param>
        public void Save(string asXml, DateTime asDate, int aiAssemblyId)
        {
            this.moAssemblyDetailsDC.Save(asXml, asDate, aiAssemblyId);
        }

        /// <summary>
        /// This method is used to Submit Assembly details.
        /// </summary>
        /// <param name="dtDate"></param>
        /// <param name="IsSubmited"></param>
        /// <param name="aiAssemblyId"></param>
        public void Submit(DateTime asDate, bool aIsSubmited, int aiAssemblyId)
        {
            this.moAssemblyDetailsDC.Submit(asDate, aIsSubmited, aiAssemblyId);
        }

        /// <summary>
        /// This method is used to Publish Assembly details.
        /// </summary>
        /// <param name="dtDate"></param>
        /// <param name="IsPublished"></param>
        /// <param name="aiAssemblyId"></param>
        public void Publish(DateTime asDate, bool aIsSubmited, int aiAssemblyId)
        {
            this.moAssemblyDetailsDC.Publish(asDate, aIsSubmited, aiAssemblyId);
        }

        /// <summary>
        /// This method is used to Get All Assembly List.
        /// </summary>
        public List<AssemblyDetails> GetAllAssemblyDetailsList()
        {
            List<AssemblyDetails> lstAssemblyDetails = moAssemblyDetailsDC.GetAllAssemblyList();
            return lstAssemblyDetails;
        }

        /// <summary>
        /// This method is used to Delete Assembly details.
        /// </summary>
        /// <param name="aiAssemblyId"></param>
        public void DeleteAssemblyDetails(int aiAssemblyId)
        {
            moAssemblyDetailsDC.DeleteAssembly(aiAssemblyId);
        }

        public DataTable GetAllAssemblyQuestionsForConfiguration()
        { 
            return  moAssemblyDetailsDC.GetAllAssemblyQuestionsForConfiguration();            
        }

        public DataTable GetAllAssemblyParentQuestions()
        {
            return moAssemblyDetailsDC.GetAllAssemblyParentQuestions();
        }

        #endregion
    }
}
