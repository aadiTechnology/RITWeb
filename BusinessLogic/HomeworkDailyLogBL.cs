using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class HomeworkDailyLogBL
    {
        #region "Data Members"

        private HomeworkDailyLogDC moHomeworkDailyLogDC;
        private int miTotalRows = 0;

        #endregion

        #region "Constructor"

        public HomeworkDailyLogBL()
        {
            moHomeworkDailyLogDC = new HomeworkDailyLogDC();
        }

        public HomeworkDailyLogBL(int aiSchoolId, int aiAcdemicYearId, int aiUserId)
        {
            moHomeworkDailyLogDC = new HomeworkDailyLogDC(aiSchoolId, aiAcdemicYearId, aiUserId);
        }

        #endregion

        #region "Public Methods"

        public bool ValidateHomeworkDailyLog(int aiSchoolId, int aiAcademicYearId, string aidate, int aiStdDivId, int aiId)
        {
            return moHomeworkDailyLogDC.ValidateHomeworkDailyLog(aiSchoolId, aiAcademicYearId, aidate, aiStdDivId, aiId);
        }

        /// <summary>
        /// This method is used to delete homework.
        /// </summary>
        /// <param name="aiHomeworkLogId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public void Delete(int aiHomeworkLogId)
        {
            moHomeworkDailyLogDC.Delete(aiHomeworkLogId);
        }

        public string Publish(string asHomeworkLogId, bool abIsPublish)
        {
            return moHomeworkDailyLogDC.Publish(asHomeworkLogId, abIsPublish);
        }

        /// <summary>
        /// This method is used to get homework details for provided id.
        /// </summary>
        /// <param name="aiHomeWorkId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public HomeworkDailyLog Get(int aiHomeWorkLogId)
        {
            return moHomeworkDailyLogDC.Get(aiHomeWorkLogId);
        }

        /// <summary>
        /// This method is used to insert or update homework details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="abUpdateFlag"></param>
        /// <returns></returns>
        public void Save(HomeworkDailyLog aoHomeworkLog, string fname, int aiStdDivId)
        {
            moHomeworkDailyLogDC.Save(aoHomeworkLog, fname, aiStdDivId);
        }
        
        public List<HomeworkDailyLog> GetAll(int aiSchoolId, int aiUserRoleId, string asFilter, string asStdDivId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            if (sortExpression == string.Empty)
                sortExpression = "Date Desc";
            if (asFilter == null)
                asFilter = string.Empty;

            maximumRows = startRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<HomeworkDailyLog> lstHomeworkDailyLog = moHomeworkDailyLogDC.GetAll(aiSchoolId, aiUserRoleId, asFilter, asStdDivId, sortExpression, startRowIndex, maximumRows);

            if (lstHomeworkDailyLog.Count > 0)
                miTotalRows = lstHomeworkDailyLog[0].TotalRows;

            return lstHomeworkDailyLog;
        }
        
        public int Count(int aiSchoolId, int aiUserRoleId, string asFilter, string asStdDivId, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            return miTotalRows;
        }

        #endregion
    }
}
