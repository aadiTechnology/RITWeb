// Class Name       :- LeftStudentsDetailsBL
// Purpose          :- This class is used to manage academic yearwise left student details.
// Date Of creation :- 8/10/2015
// Author Name      :- Yogesh

using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities.AcademicYearwiseLeftStudentDetailsMaster;
using System.Data;

namespace BusinessLogic
{
    public class LeftStudentsDetailsBL
    {

        #region MEMBER(S)

        private LeftStudentsDetailsDC moLeftStudentsDetailsDC;

        #endregion

        #region CONSTRUCTOR(S)

        public LeftStudentsDetailsBL()
        {
            this.moLeftStudentsDetailsDC = new LeftStudentsDetailsDC();
        }

        #endregion

        #region PUBLIC METHOD(S)

        /// <summary>
        /// This method is used to get academic yearwise left student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <returns></returns>
        public static List<AcademicYearwiseLeftStudentDetails> Get(int aiSchoolId, int aiAcademicYearId,int aiStandardId, string asNameFilter, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex)
        {
            return LeftStudentsDetailsDC.Get(aiSchoolId, aiAcademicYearId,aiStandardId, asNameFilter, asSortDirection, aiStartRowIndex, aiEndRowIndex);
        }

        /// <summary>
        /// This method is used to count records for grid paging.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <returns></returns>
        public static int GetCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId, string asNameFilter)
        {
            return LeftStudentsDetailsDC.GetCount(aiSchoolId, aiAcademicYearId, aiStandardId,asNameFilter);
        }

        /// <summary>
        /// This method is used to get Mobile number of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataTable GetMobileNumber(string asStudentIds)
        {
            LeftStudentsDetailsDC oLeftStudentsDetailsDC = new LeftStudentsDetailsDC();
            return oLeftStudentsDetailsDC.GetMobileNumber(asStudentIds);
        }

        /// <summary>
        /// This method is used for get data for filling standard combo box.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetDataForStandardCombo(int aiSchoolId, int aiAcademicYearId)
        {
            LeftStudentsDetailsDC oLeftStudentsDetailsDC = new LeftStudentsDetailsDC();
            return oLeftStudentsDetailsDC.GetDataForStandardCombo(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get Mobile number of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataTable ReadmissionLeftStudent(int aiSchoolId, int aiAcademicYearId, string aiStudentId)
        {
            LeftStudentsDetailsDC oLeftStudentsDetailsDC = new LeftStudentsDetailsDC();
            return oLeftStudentsDetailsDC.ReadmissionLeftStudent(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        #endregion
    }
}
