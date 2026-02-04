// Class Name       :- LateMarkConfigurationDC
// Purpose          :- This class is used to manage LateMarkConfigurationUI details.
// Date Of creation :- 11/10/2010
// Author Name      :- 

using System.Data;

namespace DataCommunicator
{
    public class LateMarkConfigurationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUserId; 

        #endregion

        #region Constructor(s)

        public LateMarkConfigurationDC()
        {
        }

        public LateMarkConfigurationDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUserId = aiUserId;
        } 

        #endregion

        #region Data Member(s)

        public DataSet GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLateMarkConfiguration");
            }
        }

        public void Save(string asLateMarkConfigurationXML, string asStaffLeavesSortOrderXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LateMarkConfigXml", asLateMarkConfigurationXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("StaffLeaveSortOrderXml", asStaffLeavesSortOrderXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertLateMarkConfiguration");
            }
        } 

        #endregion
    }
}
