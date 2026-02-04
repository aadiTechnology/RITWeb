// -----------------------------------------------------------------------
// <copyright file="TermwiseStudentHeightWeightMasterBL.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using DataCommunicator;
using StudentEntities;

namespace BusinessLogic
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TermwiseStudentHeightWeightMasterBL
    {

        #region DATA MEMBERS
        private TermwiseStudentHeightWeightMasterDC moTermwiseStudentHeightWeightMasterDC;
        #endregion

        #region CONSTURCTOR(S)
        public TermwiseStudentHeightWeightMasterBL()
        {
            moTermwiseStudentHeightWeightMasterDC = new TermwiseStudentHeightWeightMasterDC();
        }

        public TermwiseStudentHeightWeightMasterBL(int aiSchoolId, int aiAcademicYearId)
        {
            moTermwiseStudentHeightWeightMasterDC = new TermwiseStudentHeightWeightMasterDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        #region PUBLIC METHOD(S)
        /// <summary>
        /// This method is used to update student height weight details.
        /// </summary>
        /// <param name="sXML"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiLoginUserId"></param>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiTerm"></param>
        public void UpdateStudentDetailsForHeightWeight(string asStudentHeightWeight, int miSchoolId, int miAcademicYearId, int aiUserId, int aiStdDivId, int aiTermId)
        {
            moTermwiseStudentHeightWeightMasterDC.UpdateStudentDetailsForHeightWeight(asStudentHeightWeight, miSchoolId, miAcademicYearId, aiUserId, aiStdDivId, aiTermId);
        }

        /// <summary>
        /// This method is used to get all student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public List<StudentInfoForHeightWeight> GetStudentDetailsForHeightWeight(int aiSchoolId, int aiAcademicYearID, int aiStdDivId, int aiTermId)
        {
            return moTermwiseStudentHeightWeightMasterDC.GetStudentDetailsForHeightWeight(aiSchoolId, aiAcademicYearID, aiStdDivId, aiTermId);
        }
        #endregion

    }
 }
