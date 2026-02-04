
using System.Collections.Generic;
using DataCommunicator;
using XseedReportEntities;

namespace BusinessLogic
{
  public class XseedThemesBL
    {
        #region "Private Member"

        private XseedThemesDC moXseedThemesDC = null;
        
        #endregion

        #region "Constructor"
        public XseedThemesBL(int aiSchoolId)
        {
            moXseedThemesDC = new XseedThemesDC(aiSchoolId);
        }

        public XseedThemesBL(int aiSchoolId,int aiAcademicYearId)
        {
            moXseedThemesDC = new XseedThemesDC(aiSchoolId,aiAcademicYearId);
        }

        #endregion

        #region "Public method"
        /// <summary>
        /// This method is used to save Xseed theme details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiFlag"></param>
        public void Save(int aiStandardwiseAssessmentId, string asTheme, int aiSortOrder,int aiThemeId, int aiInsertedById)
        {
            moXseedThemesDC.Save(aiStandardwiseAssessmentId, asTheme, aiSortOrder,aiThemeId, aiInsertedById);
        }
        /// <summary>
        /// This method is used to get theme details. 
        /// </summary>
        /// <param name="asFilter"></param>
        /// <param name="asOrder"></param>
        /// <returns></returns>
        public static List<XseedTheme> GetAll(string asFilter, int aiStandardWiseAssessmentId, string asOrder, int aiSchoolId)
        {
            return XseedThemesDC.GetAll(asFilter,aiStandardWiseAssessmentId,asOrder,aiSchoolId);
        }
         /// <summary>
        /// This method is used to delete theme.
        /// </summary>
        /// <param name="aiThemeId"></param>
        public void Delete(int aiThemeId)
        {
            moXseedThemesDC.Delete(aiThemeId);
        }

        public int GetCount()
        {
            return moXseedThemesDC.GetCount();
        }
        #endregion
    }
}
