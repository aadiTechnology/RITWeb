using System.Collections.Generic;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;

namespace BusinessLogic.TransportBL
{
    public class TransportCapacityDetailsBL
    {
        #region Data Member(s)
        private TransportCapacityDetailsDC moTransportCapacityDetailsDC = null;
        #endregion

        #region Constructor(s)
        public TransportCapacityDetailsBL()
        {
            moTransportCapacityDetailsDC = new TransportCapacityDetailsDC();
        }

        public TransportCapacityDetailsBL(int aiSchoolId, int aiAcademicYearId)
        {
            moTransportCapacityDetailsDC = new TransportCapacityDetailsDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        public List<StandardwiseCapacityDetails> StandardwiseCount
        {
            get { return this.moTransportCapacityDetailsDC.StandardwiseCount; }
        }

        #region Public Method(s)

        /// <summary>
        /// This method is used to get transport capacity details
        /// </summary>
        /// <returns></returns>
        public List<TransportCapacityDetails> GetTransportCapacityDetails()
        {
            return moTransportCapacityDetailsDC.GetTransportCapacityDetails();
        }

        #endregion

    }
}
