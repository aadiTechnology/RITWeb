using System;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using SchoolEntities;

namespace BusinessLogic
{

    public class SchoolTimeTableMasterBL
    {


        #region DataMembers and properties

        #region Data members

        private SchoolTimeTableMasterDC.SchoolTimeTableMasterStruct moSchoolTimeTableMasterStruct;
        private SchoolTimeTableMasterDC moSchoolTimeTableMasterDC = new SchoolTimeTableMasterDC();

        #endregion
        #region Properties

        public int SchoolTimeTableMasterId
        {

            get { return moSchoolTimeTableMasterStruct.miSchoolTimeTableMasterId; }
            set { moSchoolTimeTableMasterStruct.miSchoolTimeTableMasterId = value; }
        }

        public int SchoolId
        {

            get { return moSchoolTimeTableMasterStruct.miSchoolId; }
            set { moSchoolTimeTableMasterStruct.miSchoolId = value; }
        }

        public int AcademicYearId
        {

            get { return moSchoolTimeTableMasterStruct.miAcademicYearId; }
            set { moSchoolTimeTableMasterStruct.miAcademicYearId = value; }
        }

        public int StandardDivisionId
        {

            get { return moSchoolTimeTableMasterStruct.miStandardDivisionId; }
            set { moSchoolTimeTableMasterStruct.miStandardDivisionId = value; }
        }

        public int WeekdayId
        {

            get { return moSchoolTimeTableMasterStruct.miWeekdayId; }
            set { moSchoolTimeTableMasterStruct.miWeekdayId = value; }
        }

        public string IsDeleted
        {

            get { return moSchoolTimeTableMasterStruct.msIsDeleted; }
            set { moSchoolTimeTableMasterStruct.msIsDeleted = value; }
        }

        public int InsertedById
        {

            get { return moSchoolTimeTableMasterStruct.miInsertedById; }
            set { moSchoolTimeTableMasterStruct.miInsertedById = value; }
        }

        public DateTime InsertDate
        {

            get { return moSchoolTimeTableMasterStruct.mdtInsertDate; }
            set { moSchoolTimeTableMasterStruct.mdtInsertDate = value; }
        }

        public int UpdatedById
        {

            get { return moSchoolTimeTableMasterStruct.miUpdatedById; }
            set { moSchoolTimeTableMasterStruct.miUpdatedById = value; }
        }

        public DateTime UpdateDate
        {

            get { return moSchoolTimeTableMasterStruct.mdtUpdateDate; }
            set { moSchoolTimeTableMasterStruct.mdtUpdateDate = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolTimeTableMasterBL()
        {
        }
        #endregion

        #region Public Methods

        public static DataTable GetWeekDayTimeTable(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
       {
          return SchoolTimeTableMasterDC.GetWeekDayTimeTable(aiSchoolId, aiAcademicYearId, aiWeekDayId);
       }
        public static DataSet GetTimeTableForClass(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            return SchoolTimeTableMasterDC.GetTimeTableForClass(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
        }
        public static DataSet GetTimeTableForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            return SchoolTimeTableMasterDC.GetTimeTableForTeacher(aiSchoolId, aiAcademicYearId, aiTeacherId);
        }
        public static DataSet GetTeacherTimeTableForAdditionalClasses(int aiSchoolId, int aiAcademicYearId, int aiTeacherId, int aiStandardDivId) 
        {
            return SchoolTimeTableMasterDC.GetTeacherTimeTableForAdditionalClasses(aiSchoolId, aiAcademicYearId, aiTeacherId, aiStandardDivId);
        }
        public static DataSet GetTeacherLectureDetails(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
        {
            return SchoolTimeTableMasterDC.GetTeacherLectureDetails(aiSchoolId, aiAcademicYearId, aiWeekDayId);
        }
        public static DataSet GetTimeTableDisplayForStudent(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            return SchoolTimeTableMasterDC.GetTimeTableDisplayForStudent(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
        }
        public static DataSet GetTimeTableDisplayForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            return SchoolTimeTableMasterDC.GetTimeTableDisplayForTeacher(aiSchoolId, aiAcademicYearId, aiTeacherId);
        }
        public void ManageDayTimeTable(string asMasterXml, string asDetailXml)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            moSchoolTimeTableMasterDC.ManageDayTimeTable(asMasterXml, asDetailXml);
        }
        public DataSet ManageClassTimeTable(string asMasterXml, string asDetailXml,bool abIsAdditionalClass,int aiIncCnt)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ManageClassTimeTable(asMasterXml, asDetailXml, abIsAdditionalClass, aiIncCnt);
        }
		public DataSet ManageClassTimeTable(string asMasterXml, string asDetailXml, string asAdditionalLect,int aiNcCnt)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ManageClassTimeTable(asMasterXml, asDetailXml, asAdditionalLect, aiNcCnt);
        }
        public DataSet ManageTeacherTimeTable(int aiTeacherId, string asMasterXml, string asDetailXml, string asTeacherXML,int aiIncrCnt)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ManageTeacherTimeTable(aiTeacherId, asMasterXml, asDetailXml, asTeacherXML, aiIncrCnt);
        }
        public DataSet ManageTeacherTimeTable(int aiTeacherId, string asMasterXml, string asDetailXml, bool abIsAdditionalClass,int aiIncCnt)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ManageTeacherTimeTable(aiTeacherId, asMasterXml, asDetailXml, abIsAdditionalClass, aiIncCnt);
        }
        public DataSet ValidateClassTimeTable(string asDetailXml)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ValidateClassTimeTable(asDetailXml);
        }
        public DataSet ValidateTeacherTimeTable(int aiTeacherId, string asDetailXml)
        {
            moSchoolTimeTableMasterDC.SchoolTimeTableMasterStructDetails = moSchoolTimeTableMasterStruct;
            return moSchoolTimeTableMasterDC.ValidateTeacherTimeTable(aiTeacherId, asDetailXml);
        }
        /// <summary>
        /// This method is used to reset timetable.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        public static void ResetTimetable(int aiSchoolId,int aiAcademicYrId,int aiTeacherId,int aiStandardDivisionId)
        {
            SchoolTimeTableMasterDC.ResetTimetable(aiSchoolId, aiAcademicYrId,aiTeacherId, aiStandardDivisionId);
        }
        public static DataSet GetSchoolTimeTable(int aiSchoolId, int aiAcademicYrId)
        {
            return SchoolTimeTableMasterDC.GetSchoolTimeTable(aiSchoolId, aiAcademicYrId);
        }
        public static DataSet GetWeeklyClassTimeTable(int aiSchoolId, int aiAcademicYrId)
        {
            return SchoolTimeTableMasterDC.GetWeeklyClassTimeTable(aiSchoolId, aiAcademicYrId);
        }
        public DataTable GetLectureCountsForTeachers(int aiTeacherId, string acConsiderAssembly, string acConsiderMPT, string asConsiderStayback, string asConsiderWeeklyTest)
        {
            return moSchoolTimeTableMasterDC.GetLectureCountsForTeachers(aiTeacherId, acConsiderAssembly, acConsiderMPT, asConsiderStayback, asConsiderWeeklyTest);
        }
		public static void DeleteAdditionalLecture(int aiTeacherId, int aiMptLect, int aiAssemblyLecNo, string asAssemblyDay, string asMptDay, bool abIsStayback, int aiSchoolId, int aiAcadeicYearId)
		{
			SchoolTimeTableMasterDC.DeleteAdditionalLecture(aiTeacherId,aiMptLect, aiAssemblyLecNo,asAssemblyDay,asMptDay,abIsStayback,aiSchoolId,aiAcadeicYearId); 
		}

        public static void DeleteAdditionalLecture(int aiDetailID)
        {
            SchoolTimeTableMasterDC.DeleteAdditionalLecture(aiDetailID); 
        }

        public static bool IsMid(int aiSchool_Id, int aiAcademic_Year_Id)
        {
            return SchoolTimeTableMasterDC.IsMid(aiSchool_Id, aiAcademic_Year_Id);
        }

        public static void GenerateSchoolTimeTable(int aiSchool_Id, int aiAcademic_Year_Id, int Standard_Division_Id, int WeekDay_Id, int Inserted_By_Id, string DayTimeTableDetails)
        {
            SchoolTimeTableMasterDC.GenerateSchoolTimeTable(aiSchool_Id, aiAcademic_Year_Id, Standard_Division_Id, WeekDay_Id, Inserted_By_Id, DayTimeTableDetails);
        }

        /// <summary>
        /// This method is used to check weather lecure is already configured in time table or not.(avoid duplicate additional lectures)
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiLectureNo"></param>
        /// <returns></returns>
		public string CheckDuplicateLecture(int aiSubjectId, int aiTeacherId, int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiLectureNo, int aiweekDayId)
        {
            return moSchoolTimeTableMasterDC.CheckDuplicateLecture(aiSubjectId, aiTeacherId, aiSchoolId, aiAcademicYearId, aiStdDivId, aiLectureNo,aiweekDayId);
        }

        /// <summary>
        /// This method is used to get optional subjects and optional subject groups for class 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiParentGroupId"></param>
        /// <param name="iSubjectGroupId"></param>
        /// <returns></returns>
        public static List<TimeTableDetails> GetGroupwiseOptionalSubjectForTimeTable(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiParentGroupId, int aiSubjectGroupId)
        {
            return SchoolTimeTableMasterDC.GetGroupwiseOptionalSubjectForTimeTable(aiSchoolId, aiAcademicYearId, aiStdDivId, aiParentGroupId, aiSubjectGroupId);
        }

        #endregion

    }

}
