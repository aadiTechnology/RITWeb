using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using SchoolEntities.Admin;
using Utility;

namespace BusinessLogic
{
    public class UpdateStudentDetailsInBulkBL
    {
        #region Data Member(s)

        private UpdateStudentDetailsInBulkDC moUpdateStudentDetailsInBulkDC;
        private int miTotalRecords = 0;

        #endregion

        #region Constructor(s)

        public UpdateStudentDetailsInBulkBL()
        {
            this.moUpdateStudentDetailsInBulkDC = new UpdateStudentDetailsInBulkDC();
        }

        public UpdateStudentDetailsInBulkBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moUpdateStudentDetailsInBulkDC = new UpdateStudentDetailsInBulkDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Pubic Method(s)

        public DataTable GetFillCategory()
        {
            return moUpdateStudentDetailsInBulkDC.GetFillCategoy();
        }

        public List<UpdateStudentDetailsInBulk> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiCategoryId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix, string asIsResetCall, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            if (asIsResetCall == Constants.S_ONE)
            {
                miTotalRecords = 0;
                return new List<UpdateStudentDetailsInBulk>();
            }

            if (asEnrolmentNumber == null)
                asEnrolmentNumber = string.Empty;

            if (asRegNo == null)
                asRegNo = string.Empty;

            if (sortDirection == null)
                sortDirection = string.Empty;

            if (string.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";
            else
                sortExpression = sortExpression.Replace(" ASC", "").Replace(" DESC", "");

            if (string.IsNullOrEmpty(sortDirection) || sortDirection.ToUpper() == "ASC")
                sortExpression = sortExpression + " ASC";
            else
                sortExpression = sortExpression + " DESC";

            int iEndIndex = startRowIndex + maximumRows;

            List<UpdateStudentDetailsInBulk> lstUpdateStudentDetailsInBulk = moUpdateStudentDetailsInBulkDC.GetAll(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiCategoryId, asEnrolmentNumber, abIsStudBlankRegNo, asRegNo, abIsExact, asOperator, asPrefix, startRowIndex, iEndIndex, sortExpression);

            if (lstUpdateStudentDetailsInBulk.Count > 0)
                miTotalRecords = lstUpdateStudentDetailsInBulk[0].TotalRecords;

            return lstUpdateStudentDetailsInBulk;
        }

        public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiCategoryId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix, string asIsResetCall, string sortExpression, string sortDirection, int startRowIndex, int maximumRows)
        {
            return miTotalRecords;
        }

        public void Save(string asUpdateStudentDetailsInBulkXML, int aiStandardId, int aiDivisionId, int aiCatgegoryId)
        {
            moUpdateStudentDetailsInBulkDC.Save(asUpdateStudentDetailsInBulkXML, aiStandardId, aiDivisionId, aiCatgegoryId);
        }

        #endregion
    }
}

