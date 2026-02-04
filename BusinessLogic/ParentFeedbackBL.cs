using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class ParentFeedbackBL
    {
        #region Data Member(s)
        ParentFeedbackDC moParentFeedbackDC;
        #endregion

        #region Constructor(s)

        public ParentFeedbackBL()
        {
        }

        public ParentFeedbackBL(int aiSchoolId, int aiUpdatedByid)
        {
            moParentFeedbackDC = new ParentFeedbackDC(aiSchoolId, aiUpdatedByid);
        }

        #endregion

        #region Property(s)

        public List<ParentFeedbackGrade> ParentFeedbackGrades
        {
            get { return moParentFeedbackDC.ParentFeedbackGrades; }
        }

        public List<ParentFeedbackDetails> ParentFeedbacks
        {
            get { return moParentFeedbackDC.ParentFeedbacks; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is sued to check feedback submit status.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFeedbackId"></param>
        /// <returns></returns>
        public bool CheckIsFeedbackSubmit(int aiUserId, int aiSchoolId, int aiFeedbackId)
        {
            return this.moParentFeedbackDC.CheckIsFeedbackSubmit(aiUserId, aiSchoolId, aiFeedbackId);
        }

        /// <summary>
        /// This method is sued to return feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiFeedbackId"></param>
        /// <returns></returns>
        public List<ParentFeedbackQuestion> GetAll(int aiUserId, int aiFeedbackId)
        {
            return moParentFeedbackDC.GetAll(aiUserId, aiFeedbackId);
        }


        /// <summary>
        /// This method is sued to save feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asFeedbackXml"></param>
        /// <param name="aiFeedbackId"></param>
        public void Save(int aiUserId, string asFeedbackXml, int aiFeedbackId)
        {
            moParentFeedbackDC.Save(aiUserId, asFeedbackXml, aiFeedbackId);
        }

        #endregion
    }
}
