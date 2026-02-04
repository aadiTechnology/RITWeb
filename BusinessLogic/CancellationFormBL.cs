using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class CancellationFormBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private int miTotalRows;
        private CancellationFormDC moCancellationForm = null;

        #endregion

        #region Constructor(s)

        public CancellationFormBL()
        {
            moCancellationForm = new CancellationFormDC();
        }

        public CancellationFormBL(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            moCancellationForm = new CancellationFormDC(aiSchoolId, aiUserId, aiAcademicYearId);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to delete details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            moCancellationForm.Delete(aiId);
        }

        /// <summary>
        /// This method is used to save details.
        /// </summary>
        /// <param name="aoCancellationForm"></param>
        public void Save(CancellationForm aoCancellationForm)
        {
            moCancellationForm.Save(aoCancellationForm);
        }

        /// <summary>
        /// This method is used to get search student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<SearchStudentDetails> GetAllSearchStudents(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (SortExpression == string.Empty)
                SortExpression = "Roll_No desc";

            if (asFilter == null)
                asFilter = string.Empty;

            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<SearchStudentDetails> lstSearchStudentDetails = moCancellationForm.GetAllSearchStudents(aiSchoolId, aiAcademicYearId, asFilter, SortExpression, StartRowIndex, MaximumRows);

            if (lstSearchStudentDetails.Count > 0)
                miTotalRows = lstSearchStudentDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstSearchStudentDetails;
        }

        /// <summary>
        /// This method is used to count search student rows.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public int GetCountSearchStudent(int aiSchoolId, int aiAcademicYearId, string asFilter, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }

        /// <summary>
        /// This method is used to get student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<CancellationFormStudentDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {

            if (SortExpression == string.Empty)
                SortExpression = "Roll_No desc";

            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<CancellationFormStudentDetails> lstCancellationFormStudents = moCancellationForm.GetAllStudents(aiSchoolId, aiAcademicYearId, SortExpression, StartRowIndex, MaximumRows);

            if (lstCancellationFormStudents.Count > 0)
                miTotalRows = lstCancellationFormStudents[0].TotalRows;
            else
                miTotalRows = 0;

            return lstCancellationFormStudents;
        }

        /// <summary>
        /// This method is used to count rows. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public int GetCountStudents(int aiSchoolId, int aiAcademicYearId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }

        /// <summary>
        /// This method is used to return details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <returns></returns>
        public CancellationForm Get(int aiId, int aiSchoolwiseStudentId)
        {
            return moCancellationForm.Get(aiId, aiSchoolwiseStudentId);
        }

        /// <summary>
        /// This method is used to get control details.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public CancellationForm GetControlDetails(int aiSchoolwiseStudentId, int aiId)
        {
           return moCancellationForm.GetControlDetails(aiSchoolwiseStudentId, aiId);
        }

        public void ApplyConcessionFormFee(int aiSchoolId)
        {
            moCancellationForm.ApplyConcessionFormFee(aiSchoolId);
        }

        #endregion
    }
}
