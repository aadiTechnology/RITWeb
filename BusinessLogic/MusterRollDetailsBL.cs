using System.Collections.Generic;
using DataCommunicator.MusterRollDetails;
using SchoolEntities.MusterRollDetails;

namespace BusinessLogic.MusterRollDetails
{
    public class MusterRollDetailsBL
    {
        #region Data Member(s)
        
        private MusterRollDetailsDC moMusterRollDetailsDC; 

        #endregion

        #region Constructor(s)

        public MusterRollDetailsBL()
        {
            moMusterRollDetailsDC = new MusterRollDetailsDC();
        }

        public MusterRollDetailsBL(int aiSchoolId, int aiAcademicYearId)
        {
            moMusterRollDetailsDC = new MusterRollDetailsDC(aiSchoolId, aiAcademicYearId);
        } 

        #endregion

        #region Property(s)

        public List<StudentDetails> StudentDetails
        {
            get { return this.moMusterRollDetailsDC.StudentDetails; }
        }

        public List<HolidayDetails> HolidayDetails
        {
            get { return this.moMusterRollDetailsDC.HolidayDetails; }
        }

        public List<AttendanceSummaryDetails> AttendanceSummaryDetails
        {
            get { return this.moMusterRollDetailsDC.AttendanceSummaryDetails; }
        }

        public SchoolDetails SchoolDetails
        {
            get { return this.moMusterRollDetailsDC.SchoolDetails; }
        }

        public List<int> Weekends
        {
            get { return this.moMusterRollDetailsDC.Weekends; }
        }

        public List<GenderwiseAttendanceSummary> GenderwiseAttendanceSummary
        {
            get { return this.moMusterRollDetailsDC.GenderwiseAttendanceSummary; }
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return muster roll report related details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public List<AttendanceDetails> GetAttendanceDetailsForMusterRoll(int aiStandardId, int aiDivisionId, int aiYear, int aiMonthId)
        {
            return this.moMusterRollDetailsDC.GetAttendanceDetailsForMusterRoll(aiStandardId, aiDivisionId, aiYear, aiMonthId);
        } 

        #endregion
    }
}
