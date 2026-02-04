// Class Name       :- ExternalLecturesBL
// Purpose          :- This class is used to external lectures.
// Date Of creation :- 6/23/2011
// Author Name      :- Vipul Jadhav

using DataCommunicator;
using System.Collections.Generic;
using ExternalLectures;
using WeekDayNameDetails;


namespace BusinessLogic
{
    public class ExternalLecturesBL
    {
        #region " Data Members "

        ExternalLecturesDC moExternalLecturesDC;

        #endregion " Data Members "

        #region " Properties "

        public List<StandardDivisions> lstStandardDivisions
        {
            get { return moExternalLecturesDC.mlstStandardDivisions; }
            set { moExternalLecturesDC.mlstStandardDivisions = value; }
        }

        public List<WeekDays> lstWeekDays
        {
            get { return moExternalLecturesDC.mlstWeekDays; }
            set { moExternalLecturesDC.mlstWeekDays = value; }
        }

        public List<StayBackLectureDetails> lstStayBackLectureDetails
        {
            get { return moExternalLecturesDC.mlstStayBackLectureDetails; }
            set { moExternalLecturesDC.mlstStayBackLectureDetails = value; }
        }

        public StandardWeekDaywsieStayBackLectureDetails StandardWeekDaywsieStayBackLectureDetails
        {
            get { return moExternalLecturesDC.moStandardWeekDaywsieStayBackLectureDetails; }
            set { moExternalLecturesDC.moStandardWeekDaywsieStayBackLectureDetails = value; }
        }

        #endregion " Properties "

        #region " Constructors "

        public ExternalLecturesBL()
        {
            moExternalLecturesDC = new ExternalLecturesDC();
        }

        #endregion " Constructors "

        #region " Public Methods "

        /// <summary>
        /// This method is used to get paged teacher details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="asCriteria"></param>
        /// <returns></returns>
        public List<TeacherExternalLecturesDetails> GetPagedTeacherExternalLectureDetails(int aiSchoolId, int aiAcademicYearId, int startRowIndex, int maximumRows, string asCriteria)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moExternalLecturesDC.GetPagedTeacherExternalLectureDetails(aiSchoolId, aiAcademicYearId, startRowIndex, iEndIndex, asCriteria);
        }

        /// <summary>
        /// This method is used to get teacher count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="asCriteria"></param>
        /// <returns></returns>
        public int CountPagedTeacherExternalLectureDetails(int aiSchoolId, int aiAcademicYearId, int startRowIndex, int maximumRows, string asCriteria)
        {
            return moExternalLecturesDC.CountPagedTeacherExternalLectureDetails(aiSchoolId, aiAcademicYearId, asCriteria);
        }

        /// <summary>
        /// This method is used to get stay back lecture details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetStayBackLectureDetails(int aiSchoolId, int aiAcademicYearId,string asExmaType)
        {
            moExternalLecturesDC.GetStayBackLectureDetails(aiSchoolId, aiAcademicYearId, asExmaType);
        }

        /// <summary>
        /// This method is used to get week days name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<WeekdaysName> GetWeedDaysName(int aiSchoolId, int aiAcademicYearId)
        {
            return moExternalLecturesDC.GetWeedDaysName(aiSchoolId, aiAcademicYearId);
        }
        /// <summary>
        /// This method is used to get standard week daywise stay back lecture details.
        /// </summary>
        /// <param name="aiStandardDivisonId"></param>
        /// <param name="aiWeekDayId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetStandardWeekDaywiseStayBackLectureDetails(int aiStandardDivisonId, int aiWeekDayId, int aiSchoolId, int aiAcademicYearId,string asLectureType)
        {
            moExternalLecturesDC.GetStandardWeekDaywiseStayBackLectureDetails(aiStandardDivisonId, aiWeekDayId, aiSchoolId, aiAcademicYearId, asLectureType);
        }

        public List<StayBackLectureDetails> GetStayBackLecturesForStandardsAssociatedToTeachers(int aiTeacherId, string asWeekDay, int aiSchoolId, int aiAcademicYearId, string asLectureType)
        {
            return moExternalLecturesDC.GetStayBackLecturesForStandardsAssociatedToTeachers(aiTeacherId, asWeekDay, aiSchoolId, aiAcademicYearId, asLectureType);
        }

        /// <summary>
        /// This method is used to save external lecture details.
        /// </summary>
        /// <param name="asXmlExternalLectureDetails"></param>
        public void SaveTeacherExternalLectureDetails(string asXmlExternalLectureDetails)
        {
            moExternalLecturesDC.SaveTeacherExternalLectureDetails(asXmlExternalLectureDetails);
        }

        /// <summary>
        /// This method is used to save stay back lecture details.
        /// </summary>
        /// <param name="asXmlStayBackLectureDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asWeekDay"></param>
        /// <param name="aiStandardDivsionId"></param>
        public void SaveStayBackLectureDetails(string asXmlStayBackLectureDetails, int aiSchoolId, int aiAcademicYearId, int aiUserId, string asWeekDay, int aiStandardDivsionId,string asLectureType)
        {
            moExternalLecturesDC.SaveStayBackLectureDetails(asXmlStayBackLectureDetails, aiSchoolId, aiAcademicYearId, aiUserId, asWeekDay, aiStandardDivsionId, asLectureType);
        }

        #endregion " Public Methods "
    }
}
