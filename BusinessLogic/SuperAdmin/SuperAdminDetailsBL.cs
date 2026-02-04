
// Class Name       :- SuperAdminDetailsBL
// Purpose          :- This class is used to manage super admin details.
// Date Of creation :- 8/17/2011
// Author Name      :- Vipul Jadhav


using System.Collections.Generic;
using DataCommunicator;
using SuperAdminEntities;
using BusinessLogic;

namespace BusinessLogic
{
    public class SuperAdminDetailsBL
    {
        #region "Data Members"

        SuperAdminDetailsDC moSuperAdminDetailsDC;

        #endregion "Data Members"

        #region "Properties"

        public SuperAdminDetails SuperAdminDetails
        {
            get { return moSuperAdminDetailsDC.moSuperAdminDetails; }
            set { moSuperAdminDetailsDC.moSuperAdminDetails = value; }
        }

        #endregion "Properties"

        #region "Constructor"

        public SuperAdminDetailsBL()
        {
            moSuperAdminDetailsDC = new SuperAdminDetailsDC();
        }

        public SuperAdminDetailsBL(int aiSuperAdminDetailsId)
        {
            moSuperAdminDetailsDC = new SuperAdminDetailsDC(aiSuperAdminDetailsId);
        }

        #endregion "Constructor"

        /// <summary>
        /// This method is used to insert super admin details.
        /// </summary>
        public void Insert()
        {
            moSuperAdminDetailsDC.Insert();
        }

        public bool IsDuplicate()
        {
            //return moSuperAdminDetailsDC.IsDuplicate();
			var oSchoolUserBL = new SchoolUserBL
				{
					UserId = SuperAdminDetails.UserId,
					Login  = SuperAdminDetails.UserName
				};

			return oSchoolUserBL.IsUserLoginDuplicate();
        }

        /// <summary>
        /// This method is used to update super admin details.
        /// </summary>
        public void Update()
        {
            moSuperAdminDetailsDC.Update();
        }

        /// <summary>
        /// This method is used to delete super admin details.
        /// </summary>
        public void Delete()
        {
            moSuperAdminDetailsDC.Delete();
        }

        /// <summary>
        /// This method is used to get super admin details.
        /// </summary>
        /// <returns></returns>
        public List<SuperAdminDetails> GetAll()
        {
            return moSuperAdminDetailsDC.GetAll();
        }

       /// <summary>
       /// This method is used for getting RTE/NONRTE Students.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiStandardId"></param>
       /// <param name="aiDivisionId"></param>
       /// <param name="isRTE"></param>
       /// <param name="asSearchText"></param>
       /// <param name="maximumRows"></param>
       /// <param name="startRowIndex"></param>
       /// <returns></returns>
        public List<Studentdetails> GetAllStudent(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, bool abIsRTEStudent, string asSearchText, int startRowIndex, int maximumRows)
        {
            SuperAdminDetailsDC oSuperAdminDetailsDC = new SuperAdminDetailsDC();
            int iEndIndex = startRowIndex + maximumRows;
            return oSuperAdminDetailsDC.GetAllStudent(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, abIsRTEStudent, asSearchText, startRowIndex, iEndIndex);
        }
        /// <summary>
        /// This method is used for Count purpose of the RTE/NONRTE List.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="isRTE"></param>
        /// <param name="asSearchText"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, bool abIsRTEStudent, string asSearchText, int startRowIndex, int maximumRows)
        {

            SuperAdminDetailsDC oSuperAdminDetailsDC = new SuperAdminDetailsDC();
            return oSuperAdminDetailsDC.Count(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, abIsRTEStudent, asSearchText);
        }


        /// <summary>
        /// This method is used for saving RTE/NONRTE students
        /// </summary>
        /// <param name="asStudentId"></param>
        /// <param name="miSchoolId"></param>

        public void Save(string asStudentId, int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsRTEStudent)
        {
            SuperAdminDetailsDC oSuperAdminDetailsDC = new SuperAdminDetailsDC();
            oSuperAdminDetailsDC.Save(asStudentId, aiSchoolId, aiAcademicYearId, aiUserId, abIsRTEStudent);
        }
    }
}
