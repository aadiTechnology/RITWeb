// Class Name       :- StudentAchievementBL
// Purpose          :- This class is used to manage Students All Achievement details.
// Date Of creation :- 17/11/2015
// Author Name      :- 


using System;
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using System.Data;

namespace BusinessLogic
{
    public class StudentAchievementBL
    {

        #region Data members

        private StudentAchievementDC moStudentAchievementDC;

        #endregion

        #region Constructors

        public StudentAchievementBL()
        {
           this.moStudentAchievementDC = new StudentAchievementDC();
        }

        public StudentAchievementBL(int aiSchoolId, int aiAcademicYearId,int aiUpdatedById)
        {
            this.moStudentAchievementDC = new StudentAchievementDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get Students All Achievement Details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiYearWiseStudentId"></param>

        public List<StudentAchievement> GetAll(int aiYearWiseStudentId,int aiNoteCategoryId)
        {
            return this.moStudentAchievementDC.GetAll(aiYearWiseStudentId, aiNoteCategoryId);
        }

        /// <summary>
        /// This method is used to get Achievement details
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiAchievementId"></param>

        public StudentAchievement Get(int aiAchievementId, int aiId)
        {
            return this.moStudentAchievementDC.Get(aiAchievementId, aiId);
        }

        /// <summary>
        /// This method is used to Save Students Achievement details
        /// </summary>
        /// <param name="aoStudentAchievement"></param>

        public void Save(StudentAchievement aoStudentAchievement)
        {
            this.moStudentAchievementDC.Save(aoStudentAchievement);
        }

        /// <summary>
        /// This method is used to Delete Admission Process details
        /// </summary>
        /// <param name="aiId"></param>
        ///  <param name="aiAchievementId"></param>

        public void Delete(int aiAchievementId,int aiId)
        {
            this.moStudentAchievementDC.Delete(aiAchievementId,aiId);
        }

        /// <summary>
        /// This method is used to get Name & Registration Number of Student.
        /// </summary>
        /// <param name="aiStudentId"></param>

        public StudentAchievement GetStudentDetails(int aiStudentId)
        {
            return this.moStudentAchievementDC.GetStudentDetails(aiStudentId);
        }

        /// <summary>
        /// this method is used to save achievement deatils.
        /// </summary>
        /// <param name="Xml"></param>
        /// <param name="ImageXml"></param>
        public void SaveAchievementDetails(string Xml , string ImageXml)
        {
            moStudentAchievementDC.SaveAchievementDetails(Xml, ImageXml);

        }
        /// <summary>
        /// this method is used to get achievement deatils.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="lstImagePath"></param>
        /// <returns></returns>
        public List<AchievementDetails> GetAchievementDetails(int aiSchoolId, out List<Images> lstImagePath, int aiAchievementId = 0)
        {
            return moStudentAchievementDC.GetAchievementDetails(aiSchoolId, out lstImagePath, aiAchievementId);
        }
        /// <summary>
        /// this method is used to delete achievement deatils.
        /// </summary>
        /// <param name="aiAchievementId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteAchievementDetails(int aiAchievementId)
        {
            moStudentAchievementDC.DeleteAchievementDetails(aiAchievementId);
        }

        public DataTable GetNoteCategories()
        {
            return moStudentAchievementDC.GetNoteCategories();
        }

        #endregion
    }
}
