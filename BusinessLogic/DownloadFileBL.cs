using DataCommunicator;

namespace BusinessLogic
{
    public class DownloadFileBL
    {
        #region Data Member(s)

        private DownloadFileDC moDownloadFileDC;

        #endregion

        #region Constructor(s)

        public DownloadFileBL()
        {
            moDownloadFileDC = new DownloadFileDC();
        }

        public DownloadFileBL(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId)
        {
            moDownloadFileDC = new DownloadFileDC(aiSchoolId, aiAcademicYearId, aiLoginUserId);
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
            return moDownloadFileDC.GetFilePathAndName(aiFileTypeId, aiAttachmentId);
        }

        #endregion
    }
}
