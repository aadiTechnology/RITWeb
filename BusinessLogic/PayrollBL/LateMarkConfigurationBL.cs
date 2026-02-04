// Class Name       :- LateMarkConfigurationBL
// Purpose          :- This class is used to manage LateMarkConfigurationUI details.
// Date Of creation :- 11/10/2010
// Author Name      :- 

using System.Data;
using DataCommunicator;

namespace BusinessLogic
{
    public class LateMarkConfigurationBL
    {
        #region Data Member(s)

        private LateMarkConfigurationDC moLateMarkConfigurationDC; 

        #endregion

        #region Constructor(s)

        public LateMarkConfigurationBL()
        {
            this.moLateMarkConfigurationDC = new LateMarkConfigurationDC();
        }

        public LateMarkConfigurationBL(int aiSchoolId, int aiUserId)
        {
            this.moLateMarkConfigurationDC = new LateMarkConfigurationDC(aiSchoolId, aiUserId);
        } 

        #endregion

        #region Method(s)

        public void Save(string asLateMarkConfigurationXML, string asStaffLeavesSortOrderXML)
        {
            this.moLateMarkConfigurationDC.Save(asLateMarkConfigurationXML, asStaffLeavesSortOrderXML);
        }

        public DataSet GetAll()
        {
            return this.moLateMarkConfigurationDC.GetAll();
        } 

        #endregion
    }
}
