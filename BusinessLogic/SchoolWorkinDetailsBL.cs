// Class Name       :- SchoolWorkinDetailsBL
// Purpose          :- This class is used to manage School Working details.
// Date Of creation :- 29/11/2016
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
    public class SchoolWorkinDetailsBL
    {
        #region Data members

        private SchoolWorkingDetailsDC moSchoolWorkingDetailsDC;

        #endregion

        #region Constructors

        public SchoolWorkinDetailsBL()
        {
            this.moSchoolWorkingDetailsDC = new SchoolWorkingDetailsDC();
        }

        public SchoolWorkinDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moSchoolWorkingDetailsDC = new SchoolWorkingDetailsDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        }

        #endregion

        #region Property(s)

        public List<SchoolWorkinDivisionDetails> SchoolWorkinDivisionDetails
        {
            get { return this.moSchoolWorkingDetailsDC.SchoolWorkinDivisionDetails; }
        }

        public List<SchoolWorkingStdDivDetails> SchoolWorkingStdDivDetails
        {
            get { return this.moSchoolWorkingDetailsDC.SchoolWorkingStdDivDetails; }
        }

        public List<SchoolWorkingDetails> SchoolWorkingDetails
        {
            get { return this.moSchoolWorkingDetailsDC.SchoolWorkingDetails; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get All Standard Details Details
        /// </summary>
        /// <param name="dtHalfDayDate"></param>
        public List<SchoolWorkingStandardDetails> GetAll(DateTime adtHalfDayDate)
        {
            return moSchoolWorkingDetailsDC.GetAll(adtHalfDayDate);
        }

        /// <summary>
        /// This method is used to save class wise Half Day details.
        /// </summary>
        /// <param name="dtDate"></param>
        /// <param name="asStandDivIds"></param>
        public void Save(string asStandDivIds, DateTime adtDate, DateTime adtOldDate)
        {
            moSchoolWorkingDetailsDC.Save(asStandDivIds, adtDate, adtOldDate);
        }

        /// <summary>
        /// This method is used to get All datewise half day details.
        /// </summary>        
        public List<SchoolWorkingDetails> Get(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            if (string.IsNullOrEmpty(asSortExpression))
                asSortExpression = "Date";
            if (asSortDirection == "" || asSortDirection == null)
                asSortDirection = Constants.S_DESCENDING;

            int iEndIndx = startRowIndex + maximumRows;
            return moSchoolWorkingDetailsDC.Get(aiSchoolId, aiAcademicYearId, iEndIndx, startRowIndex, asSortDirection);
        }

        /// <summary>
        /// This method is used to get count of All datewise half day details.
        /// </summary>
        public int CountTotalConfiguration(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, int maximumRows, int startRowIndex)
        {
            return moSchoolWorkingDetailsDC.Count(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to delete class wise Half Day details.
        /// </summary>
        /// <param name="dtHalfDayDate"></param>
        public void Delete(DateTime adtHalfDayDate)
        {
            moSchoolWorkingDetailsDC.Delete(adtHalfDayDate);
        }

        #endregion
    }
}
