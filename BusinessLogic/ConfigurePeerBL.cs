using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class ConfigurePeerBL
    {
        #region Data Member(s)

        private ConfigurePeerDC moConfigurePeer = null;

        #endregion

        #region Constructor(s)

        public ConfigurePeerBL()
        {
            moConfigurePeer = new ConfigurePeerDC();
        }

        public ConfigurePeerBL(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            moConfigurePeer = new ConfigurePeerDC(aiSchoolId, aiUserId, aiAcademicYearId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to fill peer dropdown in listview
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeerDetails()
        {
            return moConfigurePeer.GetPeerDetails();
        }

        /// <summary>
        /// This method is used to save details.
        /// </summary>
        /// <param name="aoCancellationForm"></param>
        public void Save(string asXML)
        {
            moConfigurePeer.Save(asXML);
        }

        /// <summary>
        /// This method is used to get student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<ConfigurePeerDetails> GetAll(int aiStandardId, int aiDivisionId)
        {
            List<ConfigurePeerDetails> lstConfigurePeerDetails = moConfigurePeer.GetAll(aiStandardId, aiDivisionId);
            return lstConfigurePeerDetails;
        }
        
        #endregion
    }
}
