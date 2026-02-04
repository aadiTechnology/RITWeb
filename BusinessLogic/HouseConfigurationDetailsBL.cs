// Class Name       :- HouseConfigurationDetailsBL
// Purpose          :- This class is used to assign standards for house configuration.
// Date Of creation :- 03/11/2015
// Author Name      :- 


using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class HouseConfigurationDetailsBL
    {
        #region Data members

        private HouseConfigurationDetailsDC moHouseConfigurationDetailsDC;

        #endregion

        #region Constructor

        public HouseConfigurationDetailsBL() 
        { 
        }

        public HouseConfigurationDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moHouseConfigurationDetailsDC = new HouseConfigurationDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get All Standards for house Configuration.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public List<HouseConfigurationDetails> GetAll()
        {
            return this.moHouseConfigurationDetailsDC.GetAll();
        }

        /// <summary>
        /// This method is used to Save Standards for house Configuration.
        /// </summary>
        /// <param name="asStandardIds"></param>
        public void Save(string asStandardIds)
        {
            this.moHouseConfigurationDetailsDC.Save(asStandardIds);
        }

        #endregion
    }
}
