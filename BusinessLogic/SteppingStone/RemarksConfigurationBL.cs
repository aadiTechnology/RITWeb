/* --------------------------------------------------------------------------
 *	FileName	: RemarksConfigurationBL.cs
 *	Modified by	: Pravin
 *	Date		: 30 Mar 2012
 *	Description	: This class is used to Adding,Removing Remarks and Template
 * --------------------------------------------------------------------------
 */

using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using ProgressReportEntities;
using System.Data;
namespace BusinessLogic
{
    public class RemarksConfigurationBL
    {
        #region Data Member(s)

        private RemarksConfigurationDC moRemarksConfigurationDC;

        #endregion

        #region Constructor(s)
        /// <summary>
        /// empty constructor.
        /// </summary>
        public RemarksConfigurationBL()
        {
            moRemarksConfigurationDC = new RemarksConfigurationDC();
        }

        /// <summary>
        /// constructor with school id and Aacademic year id parameters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public RemarksConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moRemarksConfigurationDC = new RemarksConfigurationDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }
        #endregion


        public static List<RemarksConfig> GetConfig(int aiSchoolId, int aiAcademicYearId)
        {
            return RemarksConfigurationDC.GetAll(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This function is used to get the remark template notes.
        /// </summary>
        /// <returns></returns>
        public static List<RemarkTemplateKeyword> GetTemplateNotes()
        {
            return RemarksConfigurationDC.GetTemplateNotes();
        }

        public static void Save(RemarksConfig aoRemarksConfig)
        {
            RemarksConfigurationDC.Save(aoRemarksConfig);
        }

        public static void Update(RemarksConfig aoRemarksConfig)
        {
            RemarksConfigurationDC.Update(aoRemarksConfig);
        }

        public static void Delete(RemarksConfig aoRemarksConfig)
        {
            RemarksConfigurationDC.Delete(aoRemarksConfig);
        }

        public static RemarksConfig GetRemarkDetails(int aiSchoolId, int aiAcademicYearId, int aiRemarksConfigId)
        {
            return RemarksConfigurationDC.GetRemarkDetails(aiSchoolId, aiAcademicYearId, aiRemarksConfigId);
        }

        /// <summary>
        /// This methode is used to get Maximum Remark length of any student in Class or Standard.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="bFlag"></param>
        /// <returns></returns>
        public int GetMaxRemarkLength(int aiStandardId, int aiTermId)
        {
            return moRemarksConfigurationDC.GetMaxRemarkLength(aiStandardId, aiTermId);
        }

        /// <summary>
        /// This methode is used to get confogured Maximum Remark length of any student in Class or Standard.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="bFlag"></param>
        /// <returns></returns>
        public int GetConfiguredMaxRemarkLength(int aiStandardId, int aiTermId)
        {
            return moRemarksConfigurationDC.GetConfiguredMaxRemarkLength(aiStandardId, aiTermId);
        }

        /// <summary>
        /// This class is used to get Remark Length Configuration.
        /// </summary>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public List<StandardwiseRemarkLength> GetAllStandardwiseRemarkLengths()
        {
            return moRemarksConfigurationDC.GetAllStandardwiseRemarkLengths();
        }

        /// <summary>
        /// This method is used to get Remark Length congiruation for single record.
        /// </summary>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public StandardwiseRemarkLength GetRemarkConfiguration(int aiConfigId)
        {
            return this.moRemarksConfigurationDC.GetRemarkConfiguration(aiConfigId);
        }

        /// <summary>
        /// This methode is used to Insert Remark length configuration.
        /// </summary>
        /// <param name="oStandardwiseRemarkLength"></param>
        /// <param name="aiConfigId"></param>
        public void InsertRemarkLengthDetails(StandardwiseRemarkLength oStandardwiseRemarkLength)
        {
            moRemarksConfigurationDC.InsertRemarkLengthDetails(oStandardwiseRemarkLength);
        }

        /// <summary>
        /// This methode is used to delete Remark Length configuration.
        /// </summary>
        /// <param name="aiConfigId"></param>
        public void DeleteProgressRemarkLength(int aiConfigId)
        {
            moRemarksConfigurationDC.DeleteProgressRemarkLength(aiConfigId);
        }

        public int GetConfiguredMaxRemarkLength(int aiSubjectId, int aiTestId, int aiStandardId)
        {
           return moRemarksConfigurationDC.GetConfiguredMaxRemarkLength(aiSubjectId, aiTestId, aiStandardId);
        }
    }

    public class RemarksCategoryBL
    {
        RemarksCategoryDC oRemarksCategoryDC = new RemarksCategoryDC();

        public static List<RemarksCategory> GetConfig(int aiSchoolId, int aiAcademicYearId)
        {
            return RemarksCategoryDC.GetAll(aiSchoolId, aiAcademicYearId);
        }

        public static DataTable GetGrades(int aiSchoolId, int aiAcademicYearId)
        {
            return RemarksCategoryDC.GetGrades(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This function is used to get the remark template notes.
        /// </summary>
        /// <returns></returns>
        public static List<RemarkTemplateKeyword> GetTemplateNotes()
        {
            return RemarksCategoryDC.GetTemplateNotes();
        }

        public static void Save(RemarksConfig aoRemarksCategory, int aiRecordId)
        {
            RemarksCategoryDC.Save(aoRemarksCategory, aiRecordId);
        }

        public static void Delete(RemarksConfig aoRemarksCategory)
        {
            RemarksCategoryDC.Delete(aoRemarksCategory);
        }

        public static RemarksConfig GetRemarkDetails(int aiSchoolId, int aiAcademicYearId, int aiRemarksCategoryId)
        {
            return RemarksCategoryDC.GetRemarkDetails(aiSchoolId, aiAcademicYearId, aiRemarksCategoryId);
        }
    }
    public class RemarkTemplateBL
    {
        RemarkTemplateDC oTemplateConfigurationDC = new RemarkTemplateDC();
        /// <summary>
        /// This method is used to Save Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public void Save(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            oTemplateConfigurationDC.Save(oRemarkTemplateConfig);
        }

        /// <summary>
        /// This method is used to Check Remark Template is duplicated or not
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public bool IsDuplicate(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            return oTemplateConfigurationDC.IsDuplicate(oRemarkTemplateConfig);
        }

        /// <summary>
        /// This method is used to get Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public RemarkTemplateConfig Get(int aiSchoolId, int aiTemplateConfigId)
        {
            return oTemplateConfigurationDC.Get(aiSchoolId, aiTemplateConfigId);
        }

        /// <summary>
        /// This method is used to get Remark Template details for selected remark id
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public List<RemarkTemplateConfig> GetAll(int aiSchoolId, int aiRemarkId, string asSortExpression, string asSortDirection, string asFilter, int aiAcademicYearId, int aiMarks_Grades_Configuration_DetailsId, int aiStandardId)
        {
            return oTemplateConfigurationDC.GetAll(aiSchoolId, aiRemarkId, asSortExpression, asSortDirection, asFilter, aiAcademicYearId, aiMarks_Grades_Configuration_DetailsId, aiStandardId);
        }

        /// <summary>
        /// This method is used to Delete Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public void Delete(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            oTemplateConfigurationDC.Delete(oRemarkTemplateConfig);
        }

        public List<RemarkTypeCategory> GetAllRemarkTypeCategories(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiSubjectId)
        {
            return oTemplateConfigurationDC.GetAllRemarkTypeCategories(aiSchoolId, aiAcademicYearId, aiTestId, aiSubjectId);
        }
    }
}
