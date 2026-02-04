/*File Name - StudentRecordParameterBL.cs
 * Created Date - 6th June 2018
 * Created By - Sonali
 * Description - This class is used to communicate with data access layer.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class StudentRecordParameterBL
    {
        #region Data Member(s)

        private StudentRecordParameterDC moStudentRecordParameterDC;

        #endregion

        #region Constructor(s)

        public StudentRecordParameterBL()
        {
        }

        public StudentRecordParameterBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.moStudentRecordParameterDC = new StudentRecordParameterDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        #endregion

        #region Method(s)
  
        /// <summary>
        /// This method is used to save StudentRecordParameter details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
        public void Save(StudentRecordParameter aoStudentRecordParameter)
        {
            this.moStudentRecordParameterDC.Save(aoStudentRecordParameter);
        }

        /// <summary>
        /// This method is used to delete StudentRecordParameter details.
        /// </summary>
        /// <param name="aiParameterId"></param>
        public void Delete(int aiParameterId,int aiSchoolId)
        {
            this.moStudentRecordParameterDC.Delete(aiParameterId, aiSchoolId);
        }

        /// <summary>
        /// This method is used to return AllStudentRecordParameter.
        /// </summary>
        /// <returns></returns>
        public List<StudentRecordParameter> GetAll(int aiSectionId)
        {
            return moStudentRecordParameterDC.GetAll(aiSectionId);
        }
        /// <summary>
        /// This method is used to return AllStudentRecordSections.
        /// </summary>
        /// <returns></returns>
        public List<StudentRecordSection> GetAllSections()
        {
            return moStudentRecordParameterDC.GetAllSections();
        }

        #endregion
    }
}
