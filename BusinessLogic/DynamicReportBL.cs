/* Class - DynamicReportBL.cs
 * Author - Yogesh Karne
 * Date - 10 Jun 2016.
 * Description - This business logic class used to handle business logics related to dynamic field export.
 */
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities;
namespace BusinessLogic
{
    public class DynamicReportBL
    {
        #region Data Member(s)

        private DynamicReportDC moDynamicReportDC;
        
        #endregion

        #region Constructor(s)
        
        public DynamicReportBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moDynamicReportDC = new DynamicReportDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }
        
        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to save Dynamic Report Details.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml, int aiStandardId)
        {
            moDynamicReportDC.Save(asXml, aiStandardId);
        }

        /// <summary>
        /// This method is used to get dataset about dynamic student export.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public DataSet GetStudentDataForExport(int aiStandardId, int aiDivisionId, bool abIncludeWithLeft)
        {
            return moDynamicReportDC.GetStudentDataForExport(aiStandardId, aiDivisionId, abIncludeWithLeft);
        }

        /// <summary>
        /// This method is used to get Dynamic Report Field list.
        /// </summary>
        /// <returns></returns>
        public List<DynamicFieldDetails> GetDynamicReportFieldMasterDetails(int aiUserId, bool abIsAdditional)
        {
            return moDynamicReportDC.GetDynamicReportFieldMasterDetails(abIsAdditional);
        }

        #endregion
    }
}
