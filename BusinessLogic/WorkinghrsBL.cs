using System;
using DataCommunicator;
using SchoolEntities;
using System.Collections.Generic;

namespace BusinessLogic
{
   public  class WorkinghrsBL
    {
       #region Data members

       private WorkinghrsDC moWorkinghrsDC;

       #endregion


        #region Constructors

        public WorkinghrsBL()
        {
            this.moWorkinghrsDC = new WorkinghrsDC();
        }


        public WorkinghrsBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moWorkinghrsDC = new WorkinghrsDC(aiSchoolId, aiAcademicYearId);
        }

        public WorkinghrsBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moWorkinghrsDC = new WorkinghrsDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion


        #region Method(s)

        /// <summary>
       /// This method is used to get all division as per the StandardId
       /// </summary>
       /// <returns></returns>
        public List<WorkinghrsDetails> GetAllDivisionsForStandard(int aiStandardId)
        {
            return this.moWorkinghrsDC.GetAllDivisionsForStandard(aiStandardId);
        }

       /// <summary>
       /// This method is used insert the Working hours details and save of that details.
       /// </summary>
       /// <param name="asXml"></param>
       /// <param name="aiInsertedById"></param>
        public void InsertWorkingHrsDetails(int aiStandardId,string asXml, int aiInsertedById)
        {
            moWorkinghrsDC.InsertWorkingHrsDetails(aiStandardId,asXml,aiInsertedById);
        }

    }
    #endregion
}
