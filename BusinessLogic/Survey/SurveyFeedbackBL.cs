using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using SchoolEntities.Survey;

namespace BusinessLogic
{
    public class SurveyFeedbackBL
    {
        #region Data Member(s)

        private SurveyFeedbackDC moSurveyFeedbackDC; 

        #endregion

        #region Constructor(s)

        public SurveyFeedbackBL()
        {
            this.moSurveyFeedbackDC = new SurveyFeedbackDC();
        }

        public SurveyFeedbackBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moSurveyFeedbackDC = new SurveyFeedbackDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Property(s)

        public List<FeedbackGrade> FeedbackGrades
        {
            get { return this.moSurveyFeedbackDC.FeedbackGrades; }
        }

        public List<FeedbackCategory> FeedbackCategories
        {
            get { return this.moSurveyFeedbackDC.FeedbackCategories; }
        }

        public List<FeedbackParameter> FeedbackParameters
        {
            get { return this.moSurveyFeedbackDC.FeedbackParameters; }
        }

        public SchoolEntity SchoolInfo
        {
            get { return this.moSurveyFeedbackDC.SchoolInfo; }
        }

        public bool IsFeedbackSubmitted
        {
            get { return this.moSurveyFeedbackDC.IsFeedbackSubmitted; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return feedback details.
        /// </summary>
        /// <param name="aiSurveyId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<SurveyFeedbackDetails> GetFeedbackDetails(int aiSurveyId, int aiUserId)
        {
            return this.moSurveyFeedbackDC.GetFeedbackDetails(aiSurveyId, aiUserId);
        }

        /// <summary>
        /// This method is used to save feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, int aiSurveyId, string asXml)
        {
            this.moSurveyFeedbackDC.Save(aiUserId, aiSurveyId, asXml);
        }

        /// <summary>
        /// This method is submit to return feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="abIsSubmitted"></param>
        public void Submit(int aiUserId, int aiSurveyId, bool abIsSubmitted)
        {
            this.moSurveyFeedbackDC.Submit(aiUserId, aiSurveyId, abIsSubmitted);
        }

        #endregion
    }
}
