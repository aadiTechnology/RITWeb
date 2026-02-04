using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class ResultDetailsBL
    {
        #region Data member(s)

        private ResultDetailsDC moResultDetailsDC;

        #endregion

        #region Constructor(s)

        public ResultDetailsBL()
       {
           moResultDetailsDC = new ResultDetailsDC();
       }

        public ResultDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moResultDetailsDC = new ResultDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to get listview details. 
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public List<ResultDetails> GetResultDetails(int aiStandardId, int aiDivisionId, int aiTermId)
        {
            return moResultDetailsDC.GetResultDetails(aiStandardId, aiDivisionId, aiTermId);
        }

        /// <summary>
        /// THis method is used to get conduct details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetConductList()
        {
            return moResultDetailsDC.GetConductList();
        }

        /// <summary>
        /// This method is used to get punctuality details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPunctuationList()
        {
            return moResultDetailsDC.GetPunctuationList();
        }

        /// <summary>
        /// This method is used to get result details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultList()
        {
            return moResultDetailsDC.GetResultList();
        }

        /// <summary>
        /// This method is used to save details.
        /// </summary>
        /// <param name="asxml"></param>
        /// <param name="aiTermId"></param>
        public void Save(String asxml, int aiTermId)
        {
            moResultDetailsDC.Save(asxml, aiTermId);
        }

        #endregion
    }
}
