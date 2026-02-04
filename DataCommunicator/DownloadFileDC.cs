using System.Data.SqlClient;

namespace DataCommunicator
{
    public class DownloadFileDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private int miLoginUserId; 

        #endregion

        #region Constructor(s)
        
        public DownloadFileDC()
        {
            // TODO: Complete member initialization
        }

        public DownloadFileDC(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miLoginUserId = aiLoginUserId;
        } 

        #endregion


        #region Public Method(s)

        /// <summary>
        /// This method is used to return file name with path.
        /// </summary>
        /// <param name="aiFileTypeId"></param>
        /// <param name="aiAttachmentId"></param>
        /// <returns></returns>
        public string GetFilePathAndName(int aiFileTypeId, int aiAttachmentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, System.Data.SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, System.Data.SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", miLoginUserId, System.Data.SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FileTypeId", aiFileTypeId, System.Data.SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttachmentId", aiAttachmentId, System.Data.SqlDbType.Int);

                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("FilePath", string.Empty, System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Output, 1000);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetDownloadFilePathAndName");
                return oSqlParameter.Value.ToString();
            }
        } 

        #endregion
    }
}
