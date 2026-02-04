
// -----------------------------------------------------------------------
// Class Name       :- TransportLateFeeSettingsBL
// Purpose          :- This class use to get & set transport late fee setting details
// Date Of creation :- 11/22/2013
// Author Name      :- Ashish Sonawane
// -----------------------------------------------------------------------
namespace BusinessLogic
{
    using System.Collections.Generic;
    using TransportDC;
    using TransportEntities;

    public class TransportLateFeeSettingsBL
    {

        #region Data Member(s)
        
        private TransportLateFeeSettingsDC moTransportLateFeeSettingsDC;
        
        #endregion

        #region Constructor (s)
        //Default contructor
        public TransportLateFeeSettingsBL()
        { 
        
        }
        //Define contructor with schoolis & academic year id 
        public TransportLateFeeSettingsBL(int aiSchoolId,int aiAcademicYearId)
        {
            moTransportLateFeeSettingsDC = new TransportLateFeeSettingsDC(aiSchoolId, aiAcademicYearId);
        }
        
        #endregion

        #region Public Method (s)
        /// <summary>
        /// This method use to get all transport late fee settings applicable for the school 
        /// </summary>
        /// <returns></returns>
        public List<TransportLateFeeDueDate> GetAll(out TransportLateFeeSetting aoTransportLateFeeSetting)
        {
            return this.moTransportLateFeeSettingsDC.GetAll(out aoTransportLateFeeSetting);
        }
     
        /// <summary>
        /// This method use to update /insert late fee settings for transport
        /// </summary>
        public void Insert(string asDueDateXml, TransportLateFeeSetting aoTransportLateFeeValue)
        {
            this.moTransportLateFeeSettingsDC.Insert(asDueDateXml, aoTransportLateFeeValue);
        }
        #endregion



    }
}
