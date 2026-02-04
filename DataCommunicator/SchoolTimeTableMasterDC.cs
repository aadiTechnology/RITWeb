using System;
using System.Data;
using Utility;
using System.Collections.Generic;
using SchoolEntities;
using System.Data.SqlClient;

namespace DataCommunicator
{

    public class SchoolTimeTableMasterDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolTimeTableMasterStruct
        {
            public int miSchoolTimeTableMasterId;
            public int miSchoolId;
            public int miAcademicYearId;
            public int miStandardDivisionId;
            public int miWeekdayId;
            public bool mbIsAdditionalClass;
            public string msIsDeleted;
            public int miInsertedById;
            public DateTime mdtInsertDate;
            public int miUpdatedById;
            public DateTime mdtUpdateDate;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SchoolTimeTableMasterStruct moSchoolTimeTableMasterStruct;

        #endregion
        #region Properties

        public SchoolTimeTableMasterStruct SchoolTimeTableMasterStructDetails
        {

            get { return moSchoolTimeTableMasterStruct; }
            set { moSchoolTimeTableMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolTimeTableMasterDC()
        {
        }

        #endregion

        #region Public Methods

        public static DataSet GetTimeTableForClass(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_TimeTable_ForClass");
            }
        }
        public static DataSet GetTimeTableDisplayForStudent(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Display_Student_TimeTable");
            }
        }
        public static DataSet GetTimeTableForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_TimeTable_ForTeacher");
            }
        }

        public static DataSet GetTeacherTimeTableForAdditionalClasses(int aiSchoolId, int aiAcademicYearId, int aiTeacherId, int aiStandardDivId) 
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStandardDivId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDataForAdditionalClasses");
            }
        }
        public static DataSet GetTimeTableDisplayForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Display_Teacher_TimeTable");
            }
        }

        public static DataSet GetTeacherLectureDetails(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Weekday_Id", aiWeekDayId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTeacherLecturesDeatails");
            }
        }
        public static DataTable GetWeekDayTimeTable(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Weekday_Id", aiWeekDayId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_TimeTable_ForWeekday").Tables[0];
            }
        }
        public static DataSet GetSchoolTimeTable(int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_Id", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_School_TimeTable");
            }
        }

        public static DataSet GetWeeklyClassTimeTable(int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_Id", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_WeeklyClass_TimeTable");
            }
        }

        public void ManageDayTimeTable(string asMasterXml, string asDetailXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moSchoolTimeTableMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Weekday_Id", moSchoolTimeTableMasterStruct.miWeekdayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableMaster", asMasterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ManageDayTimeTable");
            }
        }
        public DataSet ValidateClassTimeTable(string asDetailXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDiv_Id", moSchoolTimeTableMasterStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ValidateClassTimeTable");
            }
        }

        public DataSet ValidateTeacherTimeTable(int aiTeacherId, string asDetailXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ValidateTeacherTimeTable");
            }
        }


        public DataSet ManageClassTimeTable(string asMasterXml, string asDetailXml, bool abIsAdditionalClass,int aiIncNt)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moSchoolTimeTableMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDiv_Id", moSchoolTimeTableMasterStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableMaster", asMasterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
				oSQLServerDbUtility.AddParameter("IsAdditionalClass", abIsAdditionalClass, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsCountInceased", aiIncNt, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageClassTimeTable");
            }
        }

		public DataSet ManageClassTimeTable(string asMasterXml, string asDetailXml, string asAdditionalLect,int aiLectCnt)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moSchoolTimeTableMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDiv_Id", moSchoolTimeTableMasterStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableMaster", asMasterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("AdditionalLectDetails", asAdditionalLect, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IsCountInceased", aiLectCnt, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageClassTimeTable");
            }
        }

        public DataSet ManageTeacherTimeTable(int aiTeacherId, string asMasterXml, string asDetailXml, bool abIsAdditionalClass, int aiIncCnt)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moSchoolTimeTableMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableMaster", asMasterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IsAdditionalClass", abIsAdditionalClass, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IncCntByOne", aiIncCnt, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageTeacherTimeTable");
            }
        }

        public DataSet ManageTeacherTimeTable(int aiTeacherId, string asMasterXml, string asDetailXml, string asTeacherXML,int iaIncCnt)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolTimeTableMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSchoolTimeTableMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", moSchoolTimeTableMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableMaster", asMasterXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", asDetailXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("TeacherDetails", asTeacherXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IncCntByOne", iaIncCnt, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageTeacherTimeTable");
            }
        }

        /// <summary>
        /// This method is used to reset timetable.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        public static void ResetTimetable(int aiSchoolId, int aiAcademicYrId, int aiTeacherId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Yr_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Usp_ResetTimetable");
            }
        }
        /// <summary>
        /// Method returns datatable contining the number of lectures associated to specified teacher.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public DataTable GetLectureCountsForTeachers(int aiTeacherId, string acConsiderAssembly, string acConsiderMPT, string asConsiderStayback, string asConsiderWeeklyTest)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iTeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConsiderAssembly", acConsiderAssembly, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ConsiderMPT", acConsiderMPT, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ConsiderStayback", asConsiderStayback, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ConsiderWeeklyTest", asConsiderWeeklyTest, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetLectureCountsForTeachers");
            }
        }

        public static void DeleteAdditionalLecture(int aiDetailID)
        {
            string sDeleteStatement = " DELETE FROM School_TimeTable_Details WHERE School_TimeTable_Detail_Id = " + aiDetailID.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }
		public static void DeleteAdditionalLecture(int aiTeacherId, int aiMptLect, int aiAssemblyLecNo, string asAssemblyDay, string asMptDay, bool abIsStayback, int aiSchoolId, int aiAcadeicYearId)
		{
			 using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcadeicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StayBack", abIsStayback, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("MPTLectNo", aiMptLect, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AssemblyLecNo", aiAssemblyLecNo, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AssemblyDay", asAssemblyDay, SqlDbType.VarChar);
				oSQLServerDbUtility.AddParameter("MPTweekday", asMptDay, SqlDbType.VarChar);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteAdditionalLecture",true);
            }
		}

        public static bool IsMid(int aiSchool_Id,int aiAcademic_Year_Id)
        {
            string sSelectStatement = " Select [dbo].[udf_IsMidGenerated] (" + aiSchool_Id.ToString() + "," + aiAcademic_Year_Id.ToString()+")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             return Convert.ToBoolean(oSQLServerDbUtility.ExecuteTransaction(sSelectStatement));
        }

        public static void GenerateSchoolTimeTable(int aiSchool_Id, int aiAcademic_Year_Id, int Standard_Division_Id, int WeekDay_Id, int Inserted_By_Id, string DayTimeTableDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchool_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademic_Year_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", Standard_Division_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDay_Id", WeekDay_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", Inserted_By_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DayTimeTableDetails", DayTimeTableDetails, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GenerateSchoolTimeTable");
            }
        }

        /// <summary>
        /// This method is used to check weather lecure is already configured in time table or not.(avoid duplicate additional lectures).
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
			string sResult = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureNo", aiLectureNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDayId", aiweekDayId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Result", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 300);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_CheckDuplicateLectures"))
                {
                    if (oSqlParameter.Value != DBNull.Value)
                        sResult = oSqlParameter.Value.ToString();
                }
            }
			return sResult;			            
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
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentGroupId", aiParentGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectGroupId", aiSubjectGroupId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader =oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGroupwiseOptionalSubject"))
                return LoadOptionalSubjectDetails( oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to set values to properties.
        /// </summary>
        /// <param name="oTimeTableDetails"></param>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private static List<TimeTableDetails> LoadOptionalSubjectDetails(SqlDataReader oSqlDataReader)
        {
            TimeTableDetails oTimeTableDetails = null;
            List<TimeTableDetails> lstTimeTableDetails = new List<TimeTableDetails>();
            while (oSqlDataReader.Read())
            {
                oTimeTableDetails = new TimeTableDetails
                {
                    StandardDivisionID = oSqlDataReader["Standard_Division_Id"].ToInt(),
                    SubjectID = oSqlDataReader["Subject_Id"].ToInt(),
                    Class = oSqlDataReader["classSubjectName"].ToString(),
					ParentGroupId = oSqlDataReader["ParentGrpId"].ToInt(),
                    TeacherID = oSqlDataReader["Teacher_Id"].ToInt(),
                    Teacher = oSqlDataReader["SubjectTeacher"].ToString()
                };
                lstTimeTableDetails.Add(oTimeTableDetails);
            }
            return lstTimeTableDetails;
        }
        #endregion
    }

}
