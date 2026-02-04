using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using StaffPerformanceEntity;
using System.Data;

namespace BusinessLogic
{
    public class UserSurveyBL
    {
        #region Data Member(s)
        
        private UserSurveyDC moUserSurveyDC; 

        #endregion

        #region Constructor(s)
        
        public UserSurveyBL()
        {
            this.moUserSurveyDC = new UserSurveyDC();
        }

        public UserSurveyBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moUserSurveyDC = new UserSurveyDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Property(s)

        public List<SurveyQuestion> SurveyQuestions
        {
            get { return this.moUserSurveyDC.SurveyQuestions; }
        }

        public List<SurveyAnswer> SurveyAnswers
        {
            get { return this.moUserSurveyDC.SurveyAnswers; }
        }

        public SurveyUserDetails UserDetails
        {
            get { return this.moUserSurveyDC.UserDetails; }
        }

        public List<SurveyHeader> SurveyHeaders
        {
            get { return this.moUserSurveyDC.SurveyHeaders; }
        }

        public ButtonState ButtonStates
        {
            get { return this.moUserSurveyDC.ButtonStates; }
        } 

        #endregion

        #region Public Method(s)
       /// <summary>
       /// This method is used to return survey details.
       /// </summary>
       /// <param name="aiSurveyId"></param>
       /// <param name="aiUserId"></param>
       /// <returns></returns>
        public List<UserSurveyDetails> GetUserSurveyDetails(int aiSurveyId, int aiUserId)
        {
            return this.moUserSurveyDC.GetUserSurveyDetails(aiSurveyId, aiUserId);
        }

        /// <summary>
        /// This method is used to save survey details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, int aiSurveyId, string asXml)
        {
            this.moUserSurveyDC.Save(aiUserId, aiSurveyId, asXml);
        }

        /// <summary>
        /// This method is used to submit survey details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        public void Submit(int aiUserId, int aiSurveyId)
        {
            this.moUserSurveyDC.Submit(aiUserId, aiSurveyId);
        }

        /// <summary>
        /// This method is used to return all survey users.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="asFilter"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public static List<SurveyUserDetails> GetAllUsers(int aiSchoolId, int aiAcademicYearId, int aiSurveyId, string asFilter, int aiUserRoleId, int aiStartIndex, int aiEndIndex)
        {
            return UserSurveyDC.GetAllUsers(aiSchoolId, aiAcademicYearId, aiSurveyId, asFilter, aiUserRoleId, aiStartIndex, aiEndIndex);
        }

        /// <summary>
        /// This method is used to return all surveys.
        /// </summary>
        /// <returns></returns>
        public List<SurveyConfig> GetAllSurveys()
        {
            return this.moUserSurveyDC.GetAllSurveys();
        }

        /// <summary>
        /// This method is used to return all user roles.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUserRoles()
        {
            MasterDataCollectionDC oMasterDataCollectionDC = new MasterDataCollectionDC();
            DataTable oDT = oMasterDataCollectionDC.GetAllUserRoles();
            DataRow[] drArr = oDT.Select("User_Role_Id IN (1,2,3,6,7)");
            return drArr.CopyToDataTable();
        }

        #endregion
    }
}
