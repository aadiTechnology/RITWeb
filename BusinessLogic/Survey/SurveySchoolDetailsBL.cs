//// File Name  : SurveySchoolDetailsBL.cs
//// Created By : Yogesh
//// Date       : 31/10/2015
//// Description :This class is used to maintain business logic survey school record details functionality. 
////   

using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
namespace BusinessLogic
{
    public class SurveySchoolDetailsBL
    {
        #region Member(s)

        private SurveySchoolDetailsDC moSurveySchoolDetailsDC;

        #endregion

        #region Constructor

        public SurveySchoolDetailsBL()
        {

            moSurveySchoolDetailsDC = new SurveySchoolDetailsDC();
        }

        /// <summary>
        /// This method is used to get all school survey details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<SurveySchool> GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            return SurveySchoolDetailsDC.GetAll(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to save school survey details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aoSurveySchool"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static string Save(int aiSchoolId, int aiAcademicYearId, int aiSurveySchoolId, string asSurveySchoolName, int aiUserId)
        {
            return SurveySchoolDetailsDC.Save(aiSchoolId, aiAcademicYearId, aiSurveySchoolId, asSurveySchoolName, aiUserId);
        }

        /// <summary>
        /// This method is used to delete survey details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiServaySchoolId"></param>
        /// <param name="aiUserId"></param>
        public static void Delete(int aiSchoolId, int aiAcademicYearId, int aiServaySchoolId, int aiUserId)
        {
             SurveySchoolDetailsDC.Delete(aiSchoolId, aiAcademicYearId, aiServaySchoolId, aiUserId);
        }
        #endregion

    }
}
