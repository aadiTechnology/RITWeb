// Class Name       :- GradeMasterBL
// Purpose          :- This class is used to manage Grade Master details.
// Date Of creation :- 
// Author Name      :- Sachin
using System.Collections.Generic;
using DataCommunicator;
using StaffPerformanceEntity;

namespace BusinessLogic
{
    public class PerformanceGradeBL
    {
        #region "Data Member"

        private PerformanceGradeDC moPerformanceGradeDC;

        #endregion

        #region "Constructors"

        public PerformanceGradeBL()
        {
            this.moPerformanceGradeDC = new PerformanceGradeDC();
        }

        public PerformanceGradeBL(int miSchoolId,  int aiUpdatedById)
        {
            this.moPerformanceGradeDC = new PerformanceGradeDC(miSchoolId, aiUpdatedById);
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all grade details.
        /// </summary>
        /// <returns></returns>
        public List<PerformanceGrade> GetAll()
        {
            return this.moPerformanceGradeDC.GetAll();
        }

        /// <summary>
        /// This method is used to Insert and Update grade details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        public void Insert(string asXml)
        {
            this.moPerformanceGradeDC.Insert(asXml);
        }

        #endregion "Public Method"

    }
}
