using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using System.Data;

namespace BusinessLogic
{
    public class DescriptiveIndicatorBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private DescriptiveIndicatorDC moDescriptiveIndicatorDC;

        #endregion

        #region Property(s)

        public int StudentCount
        {
            get
            {
                return moDescriptiveIndicatorDC.miStudentCount;
            }
            set
            {
                moDescriptiveIndicatorDC.miStudentCount = value;
            }
        }

        #endregion
        
        #region Constructor(s)

        public DescriptiveIndicatorBL()
        {
            moDescriptiveIndicatorDC = new DescriptiveIndicatorDC();
        }

        public DescriptiveIndicatorBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moDescriptiveIndicatorDC = new DescriptiveIndicatorDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Method(s)

        public DataTable GetAllGradeDetails()
        {
            return moDescriptiveIndicatorDC.GetAllGradeDetails();
        }
        /// <summary>
        /// This method is used to return marks and remarks.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiSkillId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public DescriptiveIndicator GetAll(int aiYearwiseStudentId, int aiSkillId, int aiTermId)
        {
            return moDescriptiveIndicatorDC.GetAll(aiYearwiseStudentId, aiSkillId, aiTermId);
        }

        /// <summary>
        /// This method is used to return all parent skills.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<DescriptiveSkill> GetAllSections(int aiStandardId)
        {
            return moDescriptiveIndicatorDC.GetAllSections(aiStandardId);
        }

        /// <summary>
        /// This method is used to save marks and remarks.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="asObservationXml"></param>
        /// <param name="asMarks"></param>
        public void Save(int aiYearwiseStudentId, int aiTermId, string asObservationXml, string asMarks)
        {
            moDescriptiveIndicatorDC.Save(aiYearwiseStudentId, aiTermId, asObservationXml, asMarks);
        }

        /// <summary>
        /// This method is used to get Standardwise All Student Details for Descriptive Indecators.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        public List<StudentDetailsForDescriptiveIndicators> GetAllStudentDetails(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiTermId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            int iEndIndex = startRowIndex + maximumRows;

            if (asSortExpression != string.Empty && asSortExpression != null)
                asSortExpression = "ORDER BY" + " " + asSortExpression + " " + asSortDirection;

            return moDescriptiveIndicatorDC.GetAllStudentDetails(aiSchoolId, aiAcademicYearId, aiStdDivId, aiTermId, asSortExpression, asSortDirection, startRowIndex, iEndIndex);
        }

        /// <summary>
        /// This method is used to get standardwise count of Students for Descriptive Indecators.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiTermId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            return StudentCount;
        }

        /// <summary>
        /// This method is used to Publish The Descriptive Indicators.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="aiPublish"></param>
        public void PublishDescriptiveIndecators(int aiYearwiseStudentId, int aiTermId, int aiPublish, int aiStandardDivId)
        {
            moDescriptiveIndicatorDC.PublishDescriptiveIndecators(aiYearwiseStudentId, aiTermId, aiPublish, aiStandardDivId);
        }

        /// <summary>
        /// This method is used to check the Publish status of Descriptive Indicators.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="aiPublishStatus"></param>
        /// <param name="aiPublished"></param>
        public void CheckPublishStatus(int aiStandardDivId, int aiTermId, out int aiPublishStatus, out int aiPublished)
        {
            moDescriptiveIndicatorDC.CheckPublishStatus(aiStandardDivId, aiTermId, out aiPublishStatus, out aiPublished);
        } 

        #endregion
    }
}
