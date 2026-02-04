// Class Name       :- ReportsDC
// Purpose          :- This class is used to manage Reports details.
// Date Of creation :- 1/8/2008
// Author Name      :- 


using System;
using System.Collections;
using System.Data;
using System.Linq;
using DataCommunicator;
using Utility;
using System.Collections.Generic;
using SchoolEntities;

namespace BusinessLogic
{
	public class ReportsBL
	{
		#region -- MEMBER(s) --

        private const string S_STUDENT_DETAILS = "StudentDetails";
        private const string S_STUDENT_MARKS_DETAILS = "StudentMarksDetails";
        private const string S_GRAPH_DETAILS = "GraphDetails";
        private const string S_GRADE_DETAILS = "GradeDetails";
        private const string S_HEADER_DETAILS = "HeaderDetails";
        private const string S_ATTITUDE_TOWARDS = "AttitudeTowards";
		private ReportsDC.ReportsStruct moReportsStruct;
		private ReportsDC moReportsDC;

		#endregion -- MEMBER(s) --

		#region -- CONSTRUCTOR(s) --

		/// <summary>
		///		Default constructor which initalizes the DC member object.
		/// </summary>
		public ReportsBL()
		{
			moReportsDC = new ReportsDC();
		}

		/// <summary>
		///		Initializes the ReportsDC class with a specific report.
		/// </summary>
		/// <param name="miReportId"></param>
		public ReportsBL(int miReportId)
		{
			moReportsDC = new ReportsDC(miReportId);
			moReportsStruct = moReportsDC.ReportsStructDetails;
		}

        /// <summary>
        ///		Initializes the ReportsDC class with a specific report.
        /// </summary>
        /// <param name="miReportId"></param>
        public ReportsBL(int aiSchoolId, int aiUserId)
        {
            moReportsDC = new ReportsDC(aiSchoolId, aiUserId);            
        }

		#endregion -- CONSTRUCTOR(s) --

		#region -- PROPERTIES --

		public virtual int Report_Id
		{
			get { return moReportsStruct.miReportId; }
			set { moReportsStruct.miReportId = value; }
		}

		public virtual string Report_Name
		{
			get { return moReportsStruct.msReportName; }
			set { moReportsStruct.msReportName = value; }
		}

		public virtual string Report_Display_Name
		{
			get { return moReportsStruct.msReportDisplayName; }
			set { moReportsStruct.msReportDisplayName = value; }
		}

		public virtual string Is_Deleted
		{
			get { return moReportsStruct.msIsDeleted; }
			set { moReportsStruct.msIsDeleted = value; }
		}

		#endregion -- PROPERTIES --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Loads the DataSet for a Report.
		/// </summary>
		/// <param name="msReportID"></param>
		/// <returns></returns>
		public static DataSet LoadReportsDataset(string msReportID)
		{
			return ReportsDC.LoadReportsDataset(msReportID);
		}

        /// <summary>
        /// This method is used to get academic year start date and end date.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetAcademicYearDate(int aiSchoolId, int aiAcademicYearId)
        {
            ReportsDC oReportsDC = new ReportsDC();
            return oReportsDC.GetAcademicYearDate(aiSchoolId, aiAcademicYearId);
        }
		/// <summary>
		/// 	This method is used to execute stored procedure.
		/// </summary>
		/// <param name="asSPName"> </param>
		/// <returns> </returns>
		public static DataSet RetrieveReportParameters(string asSPName)
		{
			return ReportsDC.RetrieveReportParameters(asSPName);
		}

		/// <summary>
		/// 	This method is used to get reports name.
		/// </summary>
		/// <returns> </returns>
		public static DataTable GetAllReportDetails(int aiUserRoleId)
		{
			return ReportsDC.GetAllReportDetails(aiUserRoleId);
		}

		/// <summary>
		/// 	This method gives standard name of a particular standard_id.
		/// </summary>
		/// <param name="asStdID"> </param>
		/// <returns> </returns>
		public string GetStandardNameWithTheStandardID(string asStdID)
		{
			moReportsDC.ReportsStructDetails = moReportsStruct;
			return moReportsDC.GetStandardNameWithTheStandardID(asStdID);
		}

		/// <summary>
		/// 	This method is used to get division name of a paricular division_id.
		/// </summary>
		/// <param name="asDivID"> </param>
		/// <returns> </returns>
		public string GetDivisionNameWithDivisionID(string asDivID)
		{
			moReportsDC.ReportsStructDetails = moReportsStruct;
			return moReportsDC.GetDivisionNameWithDivisionID(asDivID);
		}

		/// <summary>
		/// 	This method is used to get particular report's description.
		/// </summary>
		/// <param name="aiReportId"> </param>
		/// <returns> </returns>
		public static string GetReportDescription(string aiReportId)
		{
			return ReportsDC.GetReportDescription(aiReportId);
		}

		/// <summary>
		/// 	This method is used to get reports folder name.
		/// </summary>
		/// <returns> </returns>
		public static DataTable GetReportFolderName(int aiUserRoleId)
		{
			return ReportsDC.GetReportFolderName(aiUserRoleId);
		}

		/// <summary>
		/// 	This method is used to get dataset for report data.
		/// </summary>
		/// <param name="sViewName"> </param>
		/// <param name="sParameters"> </param>
		/// <returns> </returns>
		public static int IsReportEmpty(string sViewName, string sParameters)
		{
			return ReportsDC.IsReportEmpty(sViewName, sParameters);
		}

		/// <summary>
		/// 	This method is used to get dataset for report data.
		/// </summary>
		/// <param name="sViewName"> </param>
		/// <param name="oHashFilterParameters"> </param>
		/// <returns> </returns>
		public static DataTable IsReportEmpty(string sViewName, Hashtable oHashFilterParameters)
		{
			return ReportsDC.IsReportEmpty(sViewName, oHashFilterParameters);
		}

        /// <summary>
        /// This Method is used to get report name from database for report screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiTermId"></param>
        public static string GetReportName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiReportId, int aiTermId)
        {
            return ReportsDC.GetReportName(aiSchoolId, aiAcademicYearId, aiStandardId, aiReportId, aiTermId);
        }

		/// <summary>
		///		Sets the default financial year date.
		/// </summary>
		/// <param name="asDate"></param>
		/// <returns></returns>
		public static int SetDefaultFinancialYear(string asDate)
		{
			return ReportsDC.SetDefaultFinancialYear(asDate);
		}

		/// <summary>
		/// 	This method is used to get datatable for filter parameters.
		/// </summary>
		/// <param name="asViewName"> </param>
		/// <param name="asOrderByField"> </param>
		/// <param name="oHashfilterParameters"> </param>
		/// <returns> DataTable </returns>
		public static DataSet RetrieveReportParameters(string asViewName, string asOrderByField, Hashtable oHashfilterParameters)
		{
			return ReportsDC.RetrieveReportParameters(asViewName, asOrderByField, oHashfilterParameters);
		}

		/// <summary>
		///		Retrieves parameters for a Report.
		/// </summary>
		/// <param name="asUSPName"></param>
		/// <param name="oHashTable"></param>
		/// <returns></returns>
		public static DataSet RetrieveReportParameters(string asUSPName, Hashtable oHashTable)
		{
			return ReportsDC.RetrieveReportParameters(asUSPName, oHashTable);
		}

        /// <summary>
        ///		Retrieves parameters for a Report.
        /// </summary>
        /// <param name="asUSPName"></param>
        /// <param name="oHashTable"></param>
        /// <returns></returns>
        public static DataSet RetrieveReportParameters(string asUSPName, Hashtable oHashTable, Dictionary<string, string> aoDictFiledDatatype)
        {
            return ReportsDC.RetrieveReportParameters(asUSPName, oHashTable, aoDictFiledDatatype);
        }

		/// <summary>
		/// 	This method is used to check wheteer at least one annual result is published or not.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <returns> </returns>
		public static bool IsAnnualResultPublished(int aiSchoolId, int aiAcademicYearId)
		{
			return ReportsDC.IsAnnualResultPublished(aiSchoolId, aiAcademicYearId) > 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAccYearId"></param>
		/// <param name="aiStandardId"></param>
		/// <param name="aidivisionId"></param>
		/// <returns></returns>
		public static DataTable GetStandardDivisionId(int aiSchoolId, int aiAccYearId, int aiStandardId, int aidivisionId)
		{
			return ReportsDC.GetStandardDivisionId(aiSchoolId, aiAccYearId, aiStandardId, aidivisionId);
		}

        /// <summary>
        /// This method is used to return all reports of given report folder.
        /// </summary>
        /// <param name="aiReportFolderId"></param>
        /// <returns></returns>
        public List<Report> GetAll(int aiReportFolderId)
        {
            return moReportsDC.GetAll(aiReportFolderId);
        }

        /// <summary>
        /// This method is used to return user report assignment details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static List<Report> GetUserReportDetails(int aiSchoolId, int aiAcademicYearId, int aiReportId, int aiUserRoleId, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            if (asFilter == null)
                asFilter = "";

            if (sortDirection == null)
                sortDirection = Constants.S_ASCENDING;

            return ReportsDC.GetUserReportDetails(aiSchoolId, aiAcademicYearId, aiReportId, aiUserRoleId, asFilter, sortDirection, maximumRows, startRowIndex);
        }

        /// <summary>
        /// This method is used to return count of user report assignment details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static int GetUserReportCount(int aiSchoolId, int aiAcademicYearId, int aiReportId, int aiUserRoleId, string asFilter, string sortExpression, string sortDirection, int maximumRows, int startRowIndex)
        {
            if (asFilter == null)
                asFilter = "";

            return ReportsDC.GetUserReportCount(aiSchoolId, aiAcademicYearId, aiReportId, aiUserRoleId, asFilter);
        }

        /// <summary>
        /// This method is used to save user report assignment details.
        /// </summary>
        /// <param name="aiReportId"></param>
        /// <param name="asAssignmentXml"></param>
        public void SaveUserReportAssignment(int aiReportId, string asAssignmentXml)
        {
            moReportsDC.SaveUserReportAssignment(aiReportId, asAssignmentXml);
        }

        /// <summary>
        /// This method is useed to get Report file Name of Bonaafide Certificate
        /// </summary>
        /// <returns></returns>
        public static string GetBonafideReportFileName()
        {
            return ReportsDC.GetBonafideReportFileName();
        }

		#region -- PROGRESS REPORT RELATED --
		
        /// <summary>
		///		Returns a DataSet to be bound to the Final Progress Report.
		///		It contains four DataTables:
		///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
		///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			2. CoCurricularSubjects	- contains co-curricular subject details for each student in the StudentDetails table.
        ///			4. GraphDetails			- contains details required for displaying graph for each student.
		///			5. GradeDetails			- contains grade details for the given standard.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiStandardId"></param>
		/// <param name="aiDivisionId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asNote"></param>
        /// <param name="aiTermId"></param>
		/// <returns></returns>
        public static DataSet GetGradingProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetGradingProgressReportDataSet(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId, aiIsFromReportScreen);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = "StudentMarkDetails";
            dsResultSet.Tables[2].TableName = "CoCurricularSubjects";

            if (aiSchoolId == Constants.SchoolId.DSK.ToInt())
            {
                dsResultSet.Tables[3].TableName = "AttitudeSubjects";
                dsResultSet.Tables[4].TableName = "ProgressGraph";
                dsResultSet.Tables[5].TableName = S_GRADE_DETAILS;
                dsResultSet.Tables[6].TableName = "SchoolDetails";
            }
            else
            {   
                dsResultSet.Tables[3].TableName = "ProgressGraph";
                dsResultSet.Tables[4].TableName = S_GRADE_DETAILS;
                dsResultSet.Tables[5].TableName = "SchoolDetails";
            }

            SetCoCurricularSubjectsFlag(dsResultSet.Tables["CoCurricularSubjects"], dsResultSet.Tables[S_STUDENT_DETAILS], " AND Test_Id <> -1");

            return dsResultSet;
        }

        /// <summary>
        ///		Returns a DataSet to be bound to the Final Progress Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			2. CoCurricularSubjects	- contains co-curricular subject details for each student in the StudentDetails table.
        ///			4. GraphDetails			- contains details required for displaying graph for each student.
        ///			5. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public static DataSet GetGradingProgressReportDataSetForFBS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId)
        {
            DataSet dsResultSet = ReportsDC.GetGradingProgressReportDataSetForFBS(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = "StudentMarkDetails";
            dsResultSet.Tables[2].TableName = "CoCurricularSubjects";
            dsResultSet.Tables[3].TableName = "ProgressGraph";
            dsResultSet.Tables[4].TableName = S_GRADE_DETAILS;

            SetCoCurricularSubjectsFlag(dsResultSet.Tables["CoCurricularSubjects"], dsResultSet.Tables[S_STUDENT_DETAILS], " AND Test_Id <> -1");

            return dsResultSet;
        }


        /// <summary>
        ///		Returns a DataSet to be bound to the Final Progress Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			2. CoCurricularSubjects	- contains co-curricular subject details for each student in the StudentDetails table.
        ///			4. GraphDetails			- contains details required for displaying graph for each student.
        ///			5. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public static DataSet GetGradingProgressReportDataSetForPPSN(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId)
        {
            DataSet dsResultSet = ReportsDC.GetGradingProgressReportDataSetForPPSN(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = "StudentMarkDetails";
            dsResultSet.Tables[2].TableName = "CoCurricularSubjects";
            dsResultSet.Tables[3].TableName = "ProgressGraph";
            dsResultSet.Tables[4].TableName = S_GRADE_DETAILS;

            SetCoCurricularSubjectsFlag(dsResultSet.Tables["CoCurricularSubjects"], dsResultSet.Tables[S_STUDENT_DETAILS], " AND Test_Id <> -1");

            return dsResultSet;
        }

        private static void SetCoCurricularSubjectsFlag(DataTable adtCoCurricularSubjects, DataTable adtStudentDetails, string sFilter)
        {
            foreach(DataRow oDRStudnetDetails in adtStudentDetails.Rows)
            {
                if (adtCoCurricularSubjects.Select("Student_Id = " + oDRStudnetDetails["Student_Id"] + sFilter).Count() > Constants.I_ZERO)
                    oDRStudnetDetails["ShowCoCurricularSubjects"] = Constants.S_YES;
                else
                    oDRStudnetDetails["ShowCoCurricularSubjects"] = Constants.S_NO;
            }
        }

		/// <summary>
		///		Returns a DataSet to be bound to the Final Progress Report.
		///		It contains four DataTables:
		///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
		///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
		///			3. GraphDetails			- contains details required for displaying graph for each student.
		///			4. GradeDetails			- contains grade details for the given standard.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiStandardId"></param>
		/// <param name="aiDivisionId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asNote"></param>
		/// <returns></returns>
		public static DataSet GetProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen = true)
		{
            DataSet dsResultSet;
            bool bIsGradingstandard = StandardMasterBL.IsGradingStandard(aiSchoolId, aiAcademicYearId, aiStandardId);
            if (aiSchoolId == Constants.SchoolId.PPS.ToInt() && aiAcademicYearId >= 53 && !bIsGradingstandard)
            {
                dsResultSet = ReportsDC.GetProgressReportDataSetForPP(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, abIsFromReportScreen);
                dsResultSet.Tables[0].TableName = "StudentDetails";
                dsResultSet.Tables[1].TableName = "StudentMarks";
                dsResultSet.Tables[2].TableName = "StudentGrades";

                DataRow[] dr = dsResultSet.Tables[1].Select("Marks LIKE '%[*]%'");
                if (dr.Length > 0)
                    SetStarLegendForPP(dr, dsResultSet.Tables[0], "* Marks are not out of 100.", "StarLegend");

                return dsResultSet;
            }
            else if (aiSchoolId == Constants.SchoolId.VPMCPS.ToInt() && !bIsGradingstandard)
            {
                dsResultSet = ReportsDC.GetProgressReportDataSetForVPMCPS(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, abIsFromReportScreen);
                dsResultSet.Tables[0].TableName = "StudentDetails";
                dsResultSet.Tables[1].TableName = "StudentMarks";
                dsResultSet.Tables[2].TableName = "StudentGrades";

                DataRow[] dr = dsResultSet.Tables[1].Select("Marks LIKE '%[*]%'");
                if (dr.Length > 0)
                    SetStarLegendForPP(dr, dsResultSet.Tables[0], "* Marks are not out of 100.", "StarLegend");

                return dsResultSet;
            }
            else
            {
                dsResultSet = ReportsDC.GetProgressReportDataSet(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, abIsFromReportScreen);

                // Give the tables an appropriate name.
                dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
                dsResultSet.Tables[1].TableName = S_STUDENT_MARKS_DETAILS;
                dsResultSet.Tables[2].TableName = S_GRAPH_DETAILS;
                dsResultSet.Tables[3].TableName = S_GRADE_DETAILS;
                dsResultSet.Tables[4].TableName = S_ATTITUDE_TOWARDS;

                // If the StudentMarksDetails table contains marks with a * or **, we update the StarLegend flag in StudentDetails table.
                DataTable dtStudentMarksDetails = dsResultSet.Tables[S_STUDENT_MARKS_DETAILS];
                DataRow[] dtRows;

                if (aiSchoolId != Constants.SchoolId.PPS.ToInt())
                {
                    dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]'");
                    SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*  Marks are out of 100.", "StarLegend");
                }

                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*][*]'");
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "**  Marks are out of 200.", "StarLegend");

                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[^]%'");
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "^  Marks are out of 80.", "StarLegend");

                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[^][^]%'");
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "^^  Marks are out of 160.", "StarLegend");

                if (aiSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]%'");
                    if (dtRows.Length > 0)
                        SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "* Marks are not out of given marks.", "StarLegend");
                }
                else
                {
                    dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[#]%'");
                    if (dtRows.Length > 0)
                        SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "# Marks are not out of given marks.", "StarLegend");
                }
            }
			return dsResultSet;
		}

        /// <summary>
        ///		Returns a DataSet to be bound to the Final Progress Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			3. GraphDetails			- contains details required for displaying graph for each student.
        ///			4. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <returns></returns>
        public static DataSet GetProgressReportDataSetForPPSN(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote)
        {
            DataSet dsResultSet = ReportsDC.GetProgressReportDataSetForPPSN(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = S_STUDENT_MARKS_DETAILS;
            dsResultSet.Tables[2].TableName = S_GRAPH_DETAILS;
            dsResultSet.Tables[3].TableName = S_GRADE_DETAILS;

            // If the StudentMarksDetails table contains marks with a * or **, we update the StarLegend flag in StudentDetails table.
            DataTable dtStudentMarksDetails = dsResultSet.Tables[S_STUDENT_MARKS_DETAILS];
            DataRow[] dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*  Marks are out of 100.", "StarLegend");

            dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*][*]'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "**  Marks are out of 200.", "StarLegend");

            return dsResultSet;
        }

        /// <summary>
        ///		Returns a DataSet to be bound to the Final Progress Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			3. GraphDetails			- contains details required for displaying graph for each student.
        ///			4. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <returns></returns>
        public static DataSet GetProgressReportDataSetForMCPS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote)
        {
            DataSet dsResultSet = ReportsDC.GetProgressReportDataSetForMCPS(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = S_STUDENT_MARKS_DETAILS;
            dsResultSet.Tables[2].TableName = S_GRAPH_DETAILS;
            dsResultSet.Tables[3].TableName = S_GRADE_DETAILS;
            dsResultSet.Tables[4].TableName = S_HEADER_DETAILS;

            // If the StudentMarksDetails table contains marks with a * or **, we update the StarLegend flag in StudentDetails table.
            DataTable dtStudentMarksDetails = dsResultSet.Tables[S_STUDENT_MARKS_DETAILS];
            DataRow[] dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*  Marks are out of 100.", "StarLegend");

            dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*][*]'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "**  Marks are out of 200.", "StarLegend");

            return dsResultSet;
        }

        /// <summary>
        ///		Returns a DataSet to be bound to the Final Progress Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			3. GraphDetails			- contains details required for displaying graph for each student.
        ///			4. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <returns></returns>
        public static DataSet GetMarkingSystemProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetMarkingSystemProgressReportDataSet(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId, aiIsFromReportScreen);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = S_STUDENT_MARKS_DETAILS;
            dsResultSet.Tables[2].TableName = "ProgressGraph";
            dsResultSet.Tables[3].TableName = S_GRADE_DETAILS;
            dsResultSet.Tables[4].TableName = "SchoolDetails";

            // If the StudentMarksDetails table contains marks with a * or **, we update the StarLegend flag in StudentDetails table.
            DataTable dtStudentMarksDetails = dsResultSet.Tables[S_STUDENT_MARKS_DETAILS];
            DataRow[] dtRows;
            if (aiSchoolId != Constants.SchoolId.PPS.ToInt() || (aiSchoolId == Constants.SchoolId.PPS.ToInt() && aiAcademicYearId < 48))
            {
                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]%'");
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*  Marks are out of 100.", "StarLegend");
            }

            dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*][*]'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "**  Marks are out of 200.", "StarLegend");

            dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[^]%'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "^  Marks are out of 80.", "StarLegend");

            dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[^][^]%'");
            SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "^^  Marks are out of 120.", "StarLegend");

            if (aiSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]%'");
                if (dtRows.Length > 0)
                {
                    if(aiSchoolId == Constants.SchoolId.PPS.ToInt() && aiAcademicYearId >= 51)
                        SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "* Marks are not out of 125 & 80.", "StarLegend");
                    else
                        SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "* Marks are not out of " + dtRows[0]["TotalMArks"] + ".", "StarLegend");
                }

                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[#]%'");
                if (dtRows.Length > 0)
                {
                    string sTotalMarks = dtRows[0]["TotalMArks"].ToString();
                    if (sTotalMarks.Trim() == Constants.S_ZERO)
                        sTotalMarks = "150";

                    SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "# Marks are not out of " + sTotalMarks + ".", "StarLegend");
                }
            }
            else
            {
                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%#%'");
                if (dtRows.Length > 0)
                    SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "# Marks are not out of " + dtRows[0]["TotalMArks"] + ".", "StarLegend");
            }
            SetCoCurricularSubjectsFlag(dsResultSet.Tables[S_STUDENT_MARKS_DETAILS], dsResultSet.Tables[S_STUDENT_DETAILS], " AND GradeSubjectName <> ''");


            return dsResultSet;
        }

        public static DataSet GetTerm1ProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetProgressReportDataSetForVPMCPS(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId, aiIsFromReportScreen);
            dsResultSet.Tables[0].TableName = "StudentDetails";
            dsResultSet.Tables[1].TableName = "StudentMarks";
            dsResultSet.Tables[2].TableName = "StudentGrades";

            DataRow[] dr = dsResultSet.Tables[1].Select("Marks LIKE '%[*]%'");
            if (dr.Length > 0)
                SetStarLegendForPP(dr, dsResultSet.Tables[0], "* Marks are not out of 100.", "StarLegend");

            return dsResultSet;
        }
        /// <summary>
        ///		Returns a DataSet to be bound to the Preliminary Examination Report.
        ///		It contains four DataTables:
        ///			1. StudentDetails		- contains details about each student such as name, rollno, reg no, class, dob, etc.
        ///			2. StudentMarksDetails	- contains marks details for each student in the StudentDetails table.
        ///			3. GradeDetails			- contains grade details for the given standard.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <returns></returns>
        public static DataSet GetPreliminaryExaminationProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetPreliminaryExaminationProgressReportDataSet(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, abIsFromReportScreen);

            // Give the tables an appropriate name.
            dsResultSet.Tables[0].TableName = S_STUDENT_DETAILS;
            dsResultSet.Tables[1].TableName = S_STUDENT_MARKS_DETAILS;
            dsResultSet.Tables[2].TableName = S_GRADE_DETAILS;

            // If the StudentMarksDetails table contains marks with a * or **, we update the StarLegend flag in StudentDetails table.
            DataTable dtStudentMarksDetails = dsResultSet.Tables[S_STUDENT_MARKS_DETAILS];
            DataRow[] dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*]%'");

            if (aiSchoolId == 18 && aiAcademicYearId >= 52)
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "* Given marks are out of 100.", "StarLegend");
            else if(aiSchoolId == 18 && aiAcademicYearId == 51)
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*- Marks are not out of given marks.", "StarLegend");
            else
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "*- Marks are out of 100.", "StarLegend");

            if (!(aiSchoolId == 18 && aiAcademicYearId >= 52))
            {
                dtRows = dtStudentMarksDetails.Select("Marks LIKE '%[*][*]%'");
                SetStarLegend(dtRows, dsResultSet.Tables[S_STUDENT_DETAILS], "**- Marks are out of 300.", "StarLegend");
            }

            return dsResultSet;
        }

		#endregion -- FINAL PROGRESS REPORT RELATED --

        public AttendanceReportEntity.StudentAttendanceReport GetStudentAttendanceDetails(int aiSchooolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiYear, int aiMonthId)
        {
            return moReportsDC.GetStudentAttendanceDetails(aiSchooolId, aiAcademicYearId, aiStdId, aiDivId, aiYear, aiMonthId);
        }

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		#region -- FINAL PROGRESS REPORT RELATED --
		
		/// <summary>
		///		Sets the text for StarLegend column in the StudentDetails table.
		/// </summary>
		/// <param name="adtRows">DataRow array which contain marks with single or double star.</param>
		/// <param name="adtStudentDetails">StudentDetails DataTable, which will be updated with the Legend text.</param>
		/// <param name="asLegendText">The text to be set as Legend.</param>
        private static void SetStarLegend(DataRow[] adtRows, DataTable adtStudentDetails, string asLegendText, string asFieldName)
		{
			if (adtRows.Length <= 0)
				return;

			var lstStudentIds = (from r in adtRows.AsEnumerable() select r["Student_Id"].ToInt()).Distinct().ToList();

			lstStudentIds.ForEach(stud =>
				{
					adtRows = adtStudentDetails.Select(String.Format("Student_Id = {0}", stud));
					if (adtRows.Length <= 0)
						return;
                    foreach (var row in adtRows)
                        row[asFieldName] += (row[asFieldName].ToString().IsNullOrEmpty() ? string.Empty : " ") + asLegendText;
				});
		}
		
		#endregion -- FINAL PROGRESS REPORT RELATED --

		#endregion -- PRIVATE METHOD(s) --        
    
        public static DataSet GetTermwiseProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, bool abIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetTermwiseProgressReportDataSet(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId, abIsFromReportScreen);

            dsResultSet.Tables[0].TableName = "StudentDetails";
            dsResultSet.Tables[1].TableName = "StudentMarks";
            dsResultSet.Tables[2].TableName = "StudentGrades";

            DataRow[] dr = dsResultSet.Tables[1].Select("Marks LIKE '%[*]%'");
            if (dr.Length > 0)
                SetStarLegendForPP(dr, dsResultSet.Tables[0], "* Marks are not out of 100.", "StarLegend");

            return dsResultSet;
        }

        public static DataSet GetPrelimProgressReportDataSetForPP(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, bool abIsFromReportScreen)
        {
            DataSet dsResultSet = ReportsDC.GetPrelimProgressReportDataSetForPP(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStudentId, asNote, aiTermId, abIsFromReportScreen);

            dsResultSet.Tables[0].TableName = "StudentDetails";
            dsResultSet.Tables[1].TableName = "StudentMarks";
            dsResultSet.Tables[2].TableName = "StudentGrades";

            if (aiSchoolId == Constants.SchoolId.VPMCPS.ToInt())
            {
                dsResultSet.Tables[3].TableName = "OtherDetails";
            }

            DataRow[] dr = dsResultSet.Tables[1].Select("Marks LIKE '%[*]%'");
            if (dr.Length > 0)
                SetStarLegendForPP(dr, dsResultSet.Tables[0], "* Marks are out of 100.", "StarLegend");

            return dsResultSet;
        }

        private static void SetStarLegendForPP(DataRow[] adtRows, DataTable adtStudentDetails, string asLegendText, string asFieldName)
        {
            if (adtRows.Length <= 0)
                return;

            var lstStudentIds = (from r in adtRows.AsEnumerable() select r["Yearwise_Student_Id"].ToInt()).Distinct().ToList();

            lstStudentIds.ForEach(stud =>
            {
                adtRows = adtStudentDetails.Select(String.Format("Yearwise_Student_Id = {0}", stud));
                if (adtRows.Length <= 0)
                    return;
                foreach (var row in adtRows)
                    row[asFieldName] += (row[asFieldName].ToString().IsNullOrEmpty() ? string.Empty : " ") + asLegendText;
            });
        }

        public static DataSet GetDetailsForHolisticReport(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiSydDivId, int aiStudentId, int aiTestId)
        {
            DataSet oDS = ReportsDC.GetDetailsForHolisticReport(aiSchoolId, aiAcademicYearId, aiStdId, aiSydDivId, aiStudentId, aiTestId);
            oDS.Tables[0].TableName = "StudentDetails";
            oDS.Tables[1].TableName = "AllAboutMe";
            oDS.Tables[2].TableName = "AssessmentDetails";
            oDS.Tables[3].TableName = "SubjectwiseAssessmentDetails";
            oDS.Tables[4].TableName = "SubjectwiseCoCurriAssessmentDetails";
            return oDS;
        }

        public static DataSet GetDetailsForPrePrimaryTerm1Report(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiStudentId)
        {
            DataSet oDS = ReportsDC.GetDetailsForPrePrimaryTerm1Report(aiSchoolId, aiAcademicYearId, aiStdId, aiDivId, aiStudentId);
            oDS.Tables[0].TableName = "StudentDetails";
            oDS.Tables[1].TableName = "StudentSkillDetails";
            oDS.Tables[2].TableName = "StudentMarkDetails";
            oDS.Tables[3].TableName = "GradeDetails";
            return oDS;
        }

        public static DataSet GetDetailsForHolisticReportForPPSH(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiStudentId, int aiTermId, bool abIsFromReportScreen)
        {
            DataSet oDS = ReportsDC.GetDetailsForPrePrimaryTerm1Report(aiSchoolId, aiAcademicYearId, aiStdId, aiDivId, aiStudentId, aiTermId, abIsFromReportScreen);
            oDS.Tables[0].TableName = "StudentDetails";
            oDS.Tables[1].TableName = "StudentSkillDetails";
            oDS.Tables[2].TableName = "StudentSkillSummary";
            return oDS;
        }
    }
}