using System;
using DataCommunicator;
using SchoolEntities;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class WorkinghoursBL
    {
        #region Data members

        private WorkinghoursDC moWorkinghrsDC;

        #endregion

        #region Constructors

        public WorkinghoursBL()
        {
            this.moWorkinghrsDC = new WorkinghoursDC();
        }

        public WorkinghoursBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moWorkinghrsDC = new WorkinghoursDC(aiSchoolId, aiAcademicYearId);
        }

        public WorkinghoursBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moWorkinghrsDC = new WorkinghoursDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion 

        #region Property(s)

        public List<WorkinghrsDetails> WorkinghoursDetails
        {
            get { return this.moWorkinghrsDC.WorkinHoursDetails; }
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
        public void InsertWorkingHrsDetails(int aiStandardId, string asXml, int aiInsertedById)
        {
            moWorkinghrsDC.InsertWorkingHrsDetails(aiStandardId, asXml, aiInsertedById);
        }

        public List<WorkinghrsDetails> Get(int aiStandardId)
        {
            return this.moWorkinghrsDC.Get(aiStandardId);
        }

    }
        #endregion
}
