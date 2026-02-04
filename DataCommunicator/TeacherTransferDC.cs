using System.Data;

using Utility;

namespace DataCommunicator
{
    public class TeacherTransferDC
    {
        #region structure

        public struct TransferStruct
        {
            public int miSrcTeacherId;
            public int miTargetTeacherId;
            public int miSchoolId;
            public int miAcademicYearId;
        }

        #endregion
        private TransferStruct moTransferStruct;

        public TeacherTransferDC(int aiSchoolId, int aiAcadId, int aiSrcTeacherId, int aiTargetTeacherId)
        {
            moTransferStruct.miSchoolId = aiSchoolId;
            moTransferStruct.miAcademicYearId = aiAcadId;
            moTransferStruct.miSrcTeacherId = aiSrcTeacherId;
            moTransferStruct.miTargetTeacherId = aiTargetTeacherId;
        }

        public DataSet GetTeacherTransfer(int aiSchoolId, int aiAcadId, string isConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_AcadId", aiAcadId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_SrcTeacherId", moTransferStruct.miSrcTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_TargetTeacherId", moTransferStruct.miTargetTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_IsTTConfig", isConfig, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDefaulTransferResults");
            }
        }
        /// <summary>
        /// This method calls a stored procedure to retrive the data(with transfer status)for selected teachers. 
        /// </summary>
        /// <param name="asClassXML"></param>
        /// <param name="asSrcSubjectXML"></param>
        /// <param name="asTargetSubjectXML"></param>
        /// <param name="asTTStrXML"></param>
        public void SaveTeacherTransfer(string asClassXML, string asSrcSubjectXML, string asTargetSubjectXML, string asTTStrXML, string asAssemblyXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSrcTeacher_Id", moTransferStruct.miSrcTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTargetTeacher_Id", moTransferStruct.miTargetTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", moTransferStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sClassXML", asClassXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("sSrcSubjectXML", asSrcSubjectXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("sTargetSubjectXML", asTargetSubjectXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("sTTXML", asTTStrXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("sAssemblyXML", asAssemblyXML, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTeacherTransfer");
            }
        }
    }
}
