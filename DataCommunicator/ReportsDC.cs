
// Class Name       :- ReportsDC
// Purpose          :- This class is used to manage Reports details.
// Date Of creation :- 1/8/2008
// Author Name      :- 


using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using SchoolEntities;

namespace DataCommunicator
{
	public class ReportsDC : DataCommunicatorBaseDC
	{
		#region -- STRUCTURE(s) --

		public struct ReportsStruct
		{
			public int miReportId;
			public string msReportName;
			public string msReportDisplayName;
			public string msIsDeleted;
		}

		#endregion -- STRUCTURE(s) --

		#region -- MEMBER(s) --

		private ReportsStruct moReportsStruct;
        private int miScholId;
        private int miUserId;
              
        #endregion -- MEMBER(s) --

		#region -- CONSTRUCTOR(s) --

		public ReportsDC()
		{
		}

		public ReportsDC(int miReportId)
		{
			LoadReportsDetails(miReportId);
		}               

        /// <summary>
        ///		Initializes member variables.
        /// </summary>
        /// <param name="miReportId"></param>
        public ReportsDC(int aiSchoolId, int aiUserId)
        {
            miScholId = aiSchoolId;
            miUserId = aiUserId;
        }

		#endregion -- CONSTRUCTOR(s) --

		#region -- PROPERTIES --

		public virtual ReportsStruct ReportsStructDetails
		{
			get { return moReportsStruct; }
			set { moReportsStruct = value; }
		}

		#endregion -- PROPERTIES --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// 	Loads the DataSet for a Report.
		/// </summary>
		/// <param name="sReportID"> </param>
		/// <returns> </returns>
		public static DataSet LoadReportsDataset(string sReportID)
		{
			string sSelectStatement = GetSelectStatementForReportFields(sReportID);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
		}

        /// <summary>
        /// This method is used to get academic year start date and end date.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetAcademicYearDate(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = String.Format("SELECT Start_date,End_Date FROM SchoolWise_Academic_Year_Master where Academic_Year_ID = {0} AND School_Id = {1} AND Is_Deleted = 'N'", aiAcademicYearId, aiSchoolId);
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

		/// <summary>
		/// 	Returns the StandardDivisionId for a class.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAccYearId"> </param>
		/// <param name="aiStandardId"> </param>
		/// <param name="aidivisionId"> </param>
		/// <returns> </returns>
		public static DataTable GetStandardDivisionId(int aiSchoolId, int aiAccYearId, int aiStandardId, int aidivisionId)
		{
			string sSelectStatement = String.Format("SELECT SchoolWise_Standard_Division_Id FROM dbo.SchoolWise_Standard_Division_Master " + "WHERE SchoolWise_Standard_Division_Master.Standard_Id={0} AND SchoolWise_Standard_Division_Master.Division_Id={1} " + "AND SchoolWise_Standard_Division_Master.School_Id={2} AND SchoolWise_Standard_Division_Master.academic_year_id={3} " + "AND SchoolWise_Standard_Division_Master.Is_Deleted=\'N\'", aiStandardId, aidivisionId, aiSchoolId, aiAccYearId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		/// <summary>
		/// 	This method is used to execute stored procedure.
		/// </summary>
		/// <param name="asSPName"> </param>
		/// <returns> </returns>
		public static DataSet RetrieveReportParameters(string asSPName)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(asSPName);
		}

		/// <summary>
		/// 	This method is used to get reports name.
		/// </summary>
		/// <returns> </returns>
		public static DataTable GetAllReportDetails(int aiUserRoleId)
		{
			string sSelectStatment = string.Format("SELECT Reports.[Report_Id],[Report_Name],[Report_Display_Name],Reports.Report_Folder_Id,[Sort_Order],Report_Folder_Name.Report_Folder_Name,Reports.IsSearchGridConsidered " + "  FROM Report_Folder_Name INNER JOIN Reports ON Report_Folder_Name.Report_Folder_Id = Reports.Report_Folder_Id INNER JOIN  Report_UserRole_Details ON Reports.Report_Id = Report_UserRole_Details.Report_Id " + " WHERE Reports.Is_Deleted=\'{0}\' AND Report_UserRole_Details.Is_Deleted =\'{0}\' AND Report_UserRole_Details.User_Role_Id ={1} ORDER BY Reports.Report_Folder_Id ,Sort_Order ", Constants.C_NO, aiUserRoleId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatment);
		}

		/// <summary>
		/// 	This method is used to get reports folder name.
		/// </summary>
		/// <returns> </returns>
		public static DataTable GetReportFolderName(int aiUserRoleId)
		{
			string sSelectStatment = string.Format("SELECT DISTINCT Report_Folder_Name.Report_Folder_Id,Report_Folder_Name.Report_Folder_Name,CASE WHEN SchoolModules.IsActive IS NULL THEN 1 ELSE SchoolModules.IsActive END AS IsActive " + "FROM SchoolModules RIGHT OUTER JOIN ModulewiseReportFolders ON SchoolModules.SchoolModulesId = ModulewiseReportFolders.SchoolModulesId RIGHT OUTER JOIN Reports INNER JOIN Report_Folder_Name " + "ON Reports.Report_Folder_Id = Report_Folder_Name.Report_Folder_Id INNER JOIN Report_UserRole_Details ON Reports.Report_Id = Report_UserRole_Details.Report_Id ON ModulewiseReportFolders.Report_Folder_Id = Report_Folder_Name.Report_Folder_Id " + "WHERE Reports.Is_Deleted = \'N\'  AND  Report_Folder_Name.Is_Deleted = \'N\' AND Report_UserRole_Details.Is_Deleted = \'N\' AND Report_UserRole_Details.User_Role_Id = {0}", aiUserRoleId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatment);
		}

		/// <summary>
		/// 	This method gives standard name of a particular standard_id.
		/// </summary>
		/// <param name="asStdID"> </param>
		/// <returns> </returns>
		public string GetStandardNameWithTheStandardID(string asStdID)
		{
			string sSelect = string.Format(" SELECT Display_Member  FROM  vw_Standard  WHERE Value_Member = \'{0}\'", asStdID);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelect);
		}

		/// <summary>
		/// 	This method is used to get division name of a paricular division_id.
		/// </summary>
		/// <param name="asDivID"> </param>
		/// <returns> </returns>
		public string GetDivisionNameWithDivisionID(string asDivID)
		{
			string sSelect = string.Format(" SELECT Display_Member  FROM  vw_Division  WHERE Value_Member = \'{0}\'", asDivID);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelect);
		}

		/// <summary>
		/// 	This method is used to get particular report's description.
		/// </summary>
		/// <param name="aiReportId"> </param>
		/// <returns> </returns>
		public static string GetReportDescription(string aiReportId)
		{
			string sSelect = string.Format(" SELECT  Report_Description  FROM  Reports  WHERE  report_Id = {0}", aiReportId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelect);
		}

		/// <summary>
		/// 	This method is used to get dataset for report data.
		/// </summary>
		/// <param name="sViewName"> </param>
		/// <param name="sParameters"> </param>
		/// <returns> </returns>
		public static int IsReportEmpty(string sViewName, string sParameters)
		{
			string sQuery = " SELECT " + "COUNT(*)" + " FROM " + sViewName + " WHERE " + sParameters;
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
		}

		/// <summary>
		/// 	This method is used to check wheteer at least one annual result is published or not.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <returns> </returns>
		public static int IsAnnualResultPublished(int aiSchoolId, int aiAcademicYearId)
		{
			string sSelectStatement = string.Format("SELECT COUNT(*) FROM SchoolWise_AnnualResult_Publish WHERE School_Id = {0} AND Academic_Year_Id = {1} AND Is_Deleted = \'N\' AND Is_Published = \'Y\'", aiSchoolId, aiAcademicYearId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
		}

		/// <summary>
		/// 	This method is used to get dataset for report data.
		/// </summary>
		/// <param name="sViewName"> </param>
		/// <param name="oHashFilterParameters"> </param>
		/// <returns> </returns>
		public static DataTable IsReportEmpty(string sViewName, Hashtable oHashFilterParameters)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				foreach (DictionaryEntry oParameter in oHashFilterParameters)
					oSQLServerDbUtility.AddParameter(oParameter.Key.ToString(), oParameter.Value.ToString(), SqlDbType.NVarChar);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable(sViewName);
			}
		}

        /// <summary>
        /// This Method is used to get report name from database for report screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public static string GetReportName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiReportId, int aiTermId)
        {
            string sReportName = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportId", aiReportId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetReportNameForProgressReport"))
                {
                    if (oSqlDataReader.Read())
                        sReportName = oSqlDataReader["ReportName"].ToString();
                }
            }
            return sReportName;
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
			if (asOrderByField.Trim() != string.Empty)
				asOrderByField = " ORDER BY " + asOrderByField;
			string sSelectStatement = "SELECT " + " * " + " FROM " + asViewName;
			string sWhereClause = string.Empty;
			foreach (DictionaryEntry oParameter in oHashfilterParameters)
			{
				string sFieldName = oParameter.Key.ToString();
				int iFieldValue = oParameter.Value.ToString().ToInt();
				sWhereClause = sWhereClause + sFieldName + "= " + iFieldValue + " AND ";
			}
			if (sWhereClause != string.Empty)
				sWhereClause = " WHERE " + sWhereClause.Substring(0, (sWhereClause.Length) - 4);
			sSelectStatement = sSelectStatement + sWhereClause + asOrderByField;
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
		}

		/// <summary>
		/// 	Retrieves the parameters for a report.
		/// </summary>
		/// <param name="asUSPName"> </param>
		/// <param name="oHashTable"> </param>
		/// <returns> </returns>
		public static DataSet RetrieveReportParameters(string asUSPName, Hashtable oHashTable)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				foreach (DictionaryEntry oParameter in oHashTable)
					oSQLServerDbUtility.AddParameter(oParameter.Key.ToString(), oParameter.Value, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(asUSPName);
			}
		}

        /// <summary>
        /// 	Retrieves the parameters for a report.
        /// </summary>
        /// <param name="asUSPName"> </param>
        /// <param name="oHashTable"> </param>
        /// <returns> </returns>
        public static DataSet RetrieveReportParameters(string asUSPName, Hashtable oHashTable, Dictionary<string, string> aoDictFiledDatatype)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                foreach (DictionaryEntry oParameter in oHashTable)
                    oSQLServerDbUtility.AddParameter(oParameter.Key.ToString(), oParameter.Value, GetDatatype(oParameter.Key.ToString(), aoDictFiledDatatype));
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(asUSPName);
            }
        }

        /// <summary>
        /// This method is used to return sql data type;
        /// </summary>
        /// <param name="asKey"></param>
        /// <param name="aoDictFiledDatatype"></param>
        /// <returns></returns>
        private static SqlDbType GetDatatype(string asKey,Dictionary<string, string> aoDictFiledDatatype)
        {
            SqlDbType oSqlDbType = SqlDbType.Int;
            if (aoDictFiledDatatype.ContainsKey(asKey))
            {
                switch(aoDictFiledDatatype[asKey])
                {
                    case "DropDownList": oSqlDbType = SqlDbType.Int; break;
                    case "datetime": oSqlDbType = SqlDbType.DateTime; break;
                    case "textbox": oSqlDbType = SqlDbType.NVarChar; break;
                }
            }
            return oSqlDbType;
        }

		/// <summary>
		/// 	Sets the default financial year date.
		/// </summary>
		/// <param name="asDate"> </param>
		/// <returns> </returns>
		public static int SetDefaultFinancialYear(string asDate)
		{
			string sSelectStatement = "SELECT [dbo].[Udf_GetCurrentFinancialYear](N'" + asDate + "')";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
		}

        /// <summary>
        /// This method is used to return all reports of given report folder.
        /// </summary>
        /// <param name="aiReportFolderId"></param>
        /// <returns></returns>
        public List<SchoolEntities.Report> GetAll(int aiReportFolderId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReportFolderId", aiReportFolderId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReports"))
                {
                    List<Report> lstReports = new List<Report>();
                    while (oSqlDataReader.Read())
                        lstReports.Add(new Report { ReportId = Convert.ToInt32(oSqlDataReader["Report_Id"]), ReportName = oSqlDataReader["Report_Display_Name"].ToString() });
                    return lstReports;
                }
            }
        }

        /// <summary>
        /// This method is used to return user report assignment details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asFilter"></param>
        /// <param name="sortDirection"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static List<Report> GetUserReportDetails(int aiSchoolId, int aiAcademicYearId, int aiReportId, int aiUserRoleId, string asFilter, string sortDirection, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportId", aiReportId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", sortDirection, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("maximumRows", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("startRowIndex", startRowIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetUserReportAssignments"))
                {
                    List<Report> lstReports = new List<Report>();

                    while (oSqlDataReader.Read())
                    {
                        lstReports.Add(
                                        new Report
                                        {
                                            ReportUserDetailId = Convert.ToInt32(oSqlDataReader["ReportUserDetailId"]),
                                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                                            HasAccess = Convert.ToBoolean(oSqlDataReader["HasAccess"]),
                                            HasFullAccess = Convert.ToBoolean(oSqlDataReader["HasFullAccess"]),
                                            IsViewApplicable = Convert.ToBoolean(oSqlDataReader["IsViewApplicable"])
                                        }
                                      );
                    }
                    return lstReports;
                }
            }
        }

        /// <summary>
        /// This method is used to return count of user report assignment details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public static int GetUserReportCount(int aiSchoolId, int aiAcademicYearId, int aiReportId, int aiUserRoleId, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportId", aiReportId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.VarChar);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("RecordCount", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetUserReportAssignments");
                return OSqlParameter.Value.ToInt();
            }
        }

        /// <summary>
        /// This method is used to save user report assignment details.
        /// </summary>
        /// <param name="aiReportId"></param>
        /// <param name="asAssignmentXml"></param>
        public void SaveUserReportAssignment(int aiReportId, string asAssignmentXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miScholId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportId", aiReportId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssignmentXml", asAssignmentXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserReportAssignment");
            }
        }

        /// <summary>
        /// This method is useed to get Report file Name of Bonaafide Certificate
        /// </summary>
        /// <returns></returns>
        public static string GetBonafideReportFileName()
        {
            string sFileNane = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBonafideCertificateReportFileName"))
                {
                    if (oSqlDataReader.Read())
                        sFileNane = oSqlDataReader["Report_Name"].ToString();
                }
            }
            return sFileNane;
        }

		#region -- FINAL PROGRESS REPORT RELATED --

		/// <summary>
		/// 	Returns the DataSet for Final Progress Report.
		/// </summary>
		/// <param name="aiSchoolId"> </param>
		/// <param name="aiAcademicYearId"> </param>
		/// <param name="aiStandardId"> </param>
		/// <param name="aiDivisionId"> </param>
		/// <param name="aiStudentId"> </param>
		/// <param name="asNote"> </param>
		/// <returns> </returns>
        public static DataSet GetProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFinalProgressReport");
			}
		}

        public static DataSet GetProgressReportDataSetForPP(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TermId", 2, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFinalProgressReportDetailsForPP");
            }
        }

        public static DataSet GetProgressReportDataSetForVPMCPS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TermId", 2, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFinalProgressReportDetailsForVPMCPS");
            }
        }

        /// <summary>
        /// 	Returns the DataSet for Final Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="aiStudentId"> </param>
        /// <param name="asNote"> </param>
        /// <returns> </returns>
        public static DataSet GetProgressReportDataSetForPPSN(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFinalProgressReportPPSN");
            }
        }

        /// <summary>
        /// 	Returns the DataSet for Final Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="aiStudentId"> </param>
        /// <param name="asNote"> </param>
        /// <returns> </returns>
        public static DataSet GetProgressReportDataSetForMCPS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFinalProgressReportMCPS");
            }
        }

        /// <summary>
        /// 	Returns the DataSet for Grading System Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="aiStudentId"> </param>
        /// <param name="asNote"> </param>
        /// <returns> </returns>
        public static DataSet GetGradingProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", aiIsFromReportScreen, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetGradingSystemProgressReport");
            }
        }

        /// <summary>
        /// 	Returns the DataSet for Grading System Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="aiStudentId"> </param>
        /// <param name="asNote"> </param>
        /// <returns> </returns>
        public static DataSet GetGradingProgressReportDataSetForFBS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetGradingSystemProgressReportForFBS");
            }
        }


        /// <summary>
        /// 	Returns the DataSet for Grading System Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"> </param>
        /// <param name="aiAcademicYearId"> </param>
        /// <param name="aiStandardId"> </param>
        /// <param name="aiDivisionId"> </param>
        /// <param name="aiStudentId"> </param>
        /// <param name="asNote"> </param>
        /// <returns> </returns>
        public static DataSet GetGradingProgressReportDataSetForPPSN(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetGradingSystemProgressReportForPPSN");
            }
        }

        /// <summary>
        /// Returns the DataSet for Marking System Progress Report.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asNote"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public static DataSet GetMarkingSystemProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                if (aiTermId != Constants.I_ZERO)
                    oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", aiIsFromReportScreen, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetMarkingSystemProgressReport");
            }
        }

        public static DataSet GetPreliminaryExaminationProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, bool abIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetPreliminaryExaminationProgressReport");
            }
        }

		#endregion -- FINAL PROGRESS REPORT RELATED --


        public AttendanceReportEntity.StudentAttendanceReport GetStudentAttendanceDetails(int aiSchooolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiYear, int aiMonthId)
        {
            AttendanceReportEntity.StudentAttendanceReport oStudentAttendanceReport = new AttendanceReportEntity.StudentAttendanceReport();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchooolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLecturewiseAttendanceForReport"))
                {
                    LoadAttendanceDetails(oSqlDataReader, oStudentAttendanceReport);

                    if (oSqlDataReader.NextResult())
                        LoadStudentInfo(oSqlDataReader, oStudentAttendanceReport);
                }
            }

            return oStudentAttendanceReport;
        }

        private void LoadStudentInfo(SqlDataReader aoSqlDataReader, AttendanceReportEntity.StudentAttendanceReport obj)
        {
            obj.StudentDetails = new List<AttendanceReportEntity.StudentInfo>();
            while (aoSqlDataReader.Read())
            {
                obj.StudentDetails.Add(new AttendanceReportEntity.StudentInfo
                {
                    EnrolmentNumber = aoSqlDataReader["Enrolment_Number"].ToString(),
                    className = aoSqlDataReader["className"].ToString(),
                    RollNo = aoSqlDataReader["Roll_No"].ToInt(),
                    StudentName = aoSqlDataReader["StudentName"].ToString(),
                    YearWiseStudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt(),
                    TermCount = aoSqlDataReader["TermCount"].ToString(),
                    TermPercentage = aoSqlDataReader["TermPercentage"].ToDecimal()
                });
            }
        }

        private void LoadAttendanceDetails(SqlDataReader aoSqlDataReader, AttendanceReportEntity.StudentAttendanceReport obj)
        {
            obj.AttendanceDetails = new List<AttendanceReportEntity.AttendanceDetails>();
            while (aoSqlDataReader.Read())
            {
                obj.AttendanceDetails.Add(new AttendanceReportEntity.AttendanceDetails
                {
                    Date = aoSqlDataReader["Attendance_Date"].ToDateTime(),
                    IsPresent = aoSqlDataReader["Is_Present"].ToString(),
                    LectureNo = aoSqlDataReader["Lecture_No"].ToInt(),
                    StudentId = aoSqlDataReader["Student_Id"].ToInt()                    
                });
            }
        }


		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		/// 	This function is used to load the Reports Details.
		/// </summary>
		/// <param name="miReportId"> </param>
		private void LoadReportsDetails(int miReportId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				string sSelectStatement = FetchReportsDetailsFromDatabase(miReportId);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR == null)
                        return;
                    while (oDR.Read())
                    {
                        if (oDR["Report_Id"] != DBNull.Value)
                            moReportsStruct.miReportId = oDR["Report_Id"].ToInt();
                        if (oDR["Report_Name"] != DBNull.Value)
                            moReportsStruct.msReportName = Convert.ToString(oDR["Report_Name"]);
                        if (oDR["Report_Display_Name"] != DBNull.Value)
                            moReportsStruct.msReportDisplayName = Convert.ToString(oDR["Report_Display_Name"]);
                        if (oDR["Is_Deleted"] != DBNull.Value)
                            moReportsStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                    }
                }
			}
		}

		/// <summary>
		/// 	This function is used to fetch the Reports Details.
		/// </summary>
		/// <param name="miReportId"> </param>
		/// <returns> </returns>
		private string FetchReportsDetailsFromDatabase(int miReportId)
		{
			string sSelectStatement = String.Format(" SELECT  Report_Id,Report_Name,Report_Display_Name,Is_Deleted FROM Reports WHERE Report_Id={0} AND Is_Deleted = \'{1}\'", miReportId, Constants.C_NO);
			return sSelectStatement;
		}

		/// <summary>
		/// 	Returns the sql select statement for Report fields.
		/// </summary>
		/// <param name="sReportID"> </param>
		/// <returns> </returns>
		private static string GetSelectStatementForReportFields(string sReportID)
		{
			return string.Format(" SELECT   *  FROM   vw_ReportFieldsForSchool WHERE  vw_ReportFieldsForSchool.Report_Id = \'{0}\' ORDER BY Display_Order ", sReportID);
		}

		#endregion -- PRIVATE METHOD(s) --        
    
        public static DataSet GetTermwiseProgressReportDataSet(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, bool abIsFromReportScreen)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTerm1ProgressReportDetailsForPP");
            }
        }

        public static DataSet GetPrelimProgressReportDataSetForPP(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, bool abIsFromReportScreen)
        {
            string sUSPName = "usp_GetPrelimProgressReportForPP";
            if (aiSchoolId == Constants.SchoolId.VPMCPS.ToInt())
                sUSPName = "usp_GetPrelimProgressReportForVP";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sUSPName);
            }
        }

        public static DataSet GetProgressReportDataSetForVPMCPS(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiStudentId, string asNote, int aiTermId, int aiIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Note", asNote, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", aiIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTerm1ProgressReportForVPMCPS");
            }
        }

        public static DataSet GetDetailsForHolisticReport(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiStdDivId, int aiStudentId, int aiTestId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Schoolwise_Standard_Division_Id", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDetailsForHolisticProgressReport");
            }
        }

        public static DataSet GetDetailsForPrePrimaryTerm1Report(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiStudentId,int aiTermId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDetailsForPrePrimaryReport");
            }
        }

        public static DataSet GetDetailsForPrePrimaryTerm1Report(int aiSchoolId, int aiAcademicYearId, int aiStdId, int aiDivId, int aiStudentId, int aiTermId, bool abIsFromReportScreen)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFromReportScreen", abIsFromReportScreen, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDetailsForHolisticReportFor3To5Std");
            }
        }
    }
}