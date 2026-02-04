// -----------------------------------------------------------------------
/* File Name - ProgressReportDC.cs
 * Created Date - 22-March-2013
 * Created by - Lakshman Shinde
 * Class Description - This class is used for  Block student progress report.
 */
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Utility;
using System.Data.SqlClient;
using System.Linq;
using ProgressReportEntities;
using SchoolEntities.Dashboard;
namespace DataCommunicator
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class ProgressReportDC
	{
	 
	  #region Datamember

		private int miStudentCount;
		private int miSchoolId;
		private int miAcademicYearId;
		private int miInsertedById;

	 #endregion

	  #region Constructors

		public ProgressReportDC()
		{
		}

		public ProgressReportDC(int aiSchoolId, int aiAcademicYearId,int aiInsertedById)
		{
			miSchoolId = aiSchoolId;
			miAcademicYearId = aiAcademicYearId;
			miInsertedById = aiInsertedById;
		}

		public int StudentCount
		{
			get
			{
				return miStudentCount;
			}
		}

	 #endregion

	  #region Public Methods

		/// <summary>
		/// This method is used to get the blocke unblocked student 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="baShowblocked"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asSearch"></param>
		/// <param name="sortExpression"></param>
		/// <param name="aistartRowIndex"></param>
		/// <param name="aiEndIndex"></param>
		/// <returns></returns>
		public List<BlockStudentsProgressReportDetails> GetAllBlockedUnBlockedStudents(int aiStdDivId, bool baShowblocked, int aiStudentId, string asSearch, string sortExpression, int aistartRowIndex, int aiEndIndex)
			{
				List<BlockStudentsProgressReportDetails> lstStudentInfo = new List<BlockStudentsProgressReportDetails>();
				if (sortExpression.IsNullOrEmpty())
				sortExpression="RollNo";
				using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				{
					oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("StandardDivId", aiStdDivId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("Showblocked", baShowblocked, SqlDbType.Bit);
					oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("Search", Utility.StringUtility.ReplaceSingleQuoteInString(asSearch, true), SqlDbType.NVarChar);
					oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
					oSQLServerDbUtility.AddParameter("StartIndex", aistartRowIndex, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBlockedUnBlockedStudents"))
                    {
                        GenericClass<BlockStudentsProgressReportDetails> oStudentInfo = new GenericClass<BlockStudentsProgressReportDetails>();
                        lstStudentInfo = oStudentInfo.GetFilledObjectList(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                        {
                            oSqlDataReader.Read();
                            miStudentCount = Convert.ToInt32(oSqlDataReader["Count"]);
                           
                        }
                    }
				}
				return lstStudentInfo;
			}

			/// <summary>
			/// This method is save the reason to block progress report
			/// </summary>
			/// <param name="asXml"></param>
			/// <param name="aiSchoolId"></param>
			/// <param name="aiAcademicYearId"></param>
			/// <param name="aiInsertedById"></param>
			/// <param name="abIsUpdateOrUnblock"></param>
		public void SaveBlockStudentDetails(string asXml, bool abIsUpdateOrUnblock)
			{
				using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				{
					oSQLServerDbUtility.AddParameter("StudentStatus", asXml, SqlDbType.Xml);
					oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("InsertedById", miInsertedById, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("IsBlocked", abIsUpdateOrUnblock, SqlDbType.Bit);
					oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveBlockedStudentProgressReportDetails");
				}
			}

			/// <summary>
			/// This methos is used to get Reason Progress report blocked reason.
			/// </summary>
			/// <param name="aiStudentId"></param>
			/// <param name="aiSchoolId"></param>
			/// <param name="aiAcademicYearId"></param>
			/// <returns></returns>
			public string GetBlockProgressReportReason(int aiStudentId)
			{
				using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				{
					oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
					oSQLServerDbUtility.AddParameter("AcadmicYearId", miAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("IsFromWebsite", Constants.I_ONE, SqlDbType.Int);
					SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("Reason",string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 300);
					oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetBlockProgressReportReason");
					return(oSqlParam.Value.ToString());
				}
			}

            /// <summary>
            /// This method is used to get exam wise student performance based on the exam id and standard Id.
            /// </summary>
            /// <param name="aiSchoolId"></param>
            /// <param name="aiAcademicYearId"></param>
            /// <param name="aiTestId"></param>
            /// <param name="aiStandardId"></param>
            /// <returns></returns>
            public static StandardwiseStudentPerformance GetStandardsPerformanceData(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiStandardId, bool abIsServiceCall = false)
            {
                    return GetStandardwiseExamPerformance(aiSchoolId, aiAcademicYearId, aiTestId, aiStandardId, abIsServiceCall);
               
            }
			#endregion

            #region  Private Methods

            /// <summary>
            /// This method is used to get standard wise exam performance based on the exam and standard pass for method.
            /// </summary>
            /// <param name="aiSchoolId"></param>
            /// <param name="aiAcademicYearId"></param>
            /// <param name="aiTestId"></param>
            /// <param name="aiStandardId"></param>
            /// <returns></returns>
            private static StandardwiseStudentPerformance GetStandardwiseExamPerformance(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiStandardId, bool abIsServiceCall = false)
            {
                List<StudentGradeDetails> lstPerformanceDetails = new List<StudentGradeDetails>();
                List<GradeStandardCountDetails> lstGradeStandardCount = GetGradeStandardDetails(aiSchoolId, aiAcademicYearId, aiTestId, aiStandardId, abIsServiceCall);

                //Get distinct standards list and convert to array
                IEnumerable<string> lstDistinctStandards = lstGradeStandardCount.Select(s => s.Standard).Distinct();

                List<string> lstStandards = new List<string>();
                foreach (var sItem in lstDistinctStandards)
                {
                    lstStandards.Add(sItem);
                }

                string[] arrStandards = lstStandards.ToArray();

                //Get distinct Grades list
                IEnumerable<string> lstDistinctGrades = lstGradeStandardCount.OrderBy(s => s.Grade).Select(s => s.Grade).Distinct();

                int iStudentCount;

                foreach (var sGrade in lstDistinctGrades)
                {
                    List<int> lstStudentCount = new List<int>();

                    foreach (var sStandard in lstStandards)
                    {
                        iStudentCount = 0;
                        iStudentCount = lstGradeStandardCount.Where(s => s.Standard == sStandard && s.Grade == sGrade).Select(s => s.StudentCount).FirstOrDefault();

                        lstStudentCount.Add(iStudentCount);
                    }

                    lstPerformanceDetails.Add(new StudentGradeDetails()
                    {
                        Grade = sGrade,
                        StudentCount = lstStudentCount.ToArray()
                    });
                }

                int iMaxStudentCount = 0;
                if (lstGradeStandardCount.Count > 0)
                {
                    iMaxStudentCount = lstGradeStandardCount.GroupBy(x => x.Standard).Select(lg =>
                                          new
                                          {
                                              Standard = lg.Key,
                                              StudentCount = lg.Sum(w => w.StudentCount),
                                          }).ToList().Max(a => a.StudentCount);
                }

                StandardwiseStudentPerformance oStandardwiseStudentPerformance = new StandardwiseStudentPerformance()
                {
                    GradeDetails = lstPerformanceDetails,
                    MaxStudentCount = iMaxStudentCount,
                    Standards = arrStandards
                };

                return oStandardwiseStudentPerformance;
            }

            /// <summary>
            /// This method is used to get grade of standard in the exam.
            /// </summary>
            /// <param name="aiSchoolId"></param>
            /// <param name="aiAcademicYearId"></param>
            /// <param name="aiTestId"></param>
            /// <param name="aiStandardId"></param>
            /// <returns></returns>
            private static List<GradeStandardCountDetails> GetGradeStandardDetails(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiStandardId, bool abIsServiceCall = false)
            {
                List<GradeStandardCountDetails> lstGradeStandardNameDetails = new List<GradeStandardCountDetails>();

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, Constants.I_ZERO, abIsServiceCall))
                {
                    oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GradeStandardDetails"))
                    {
                        GradeStandardCountDetails oGradeStandardNameDetails = null;
                        while (oSqlDataReader.Read())
                        {
                            oGradeStandardNameDetails = new GradeStandardCountDetails()
                            {
                                Grade = oSqlDataReader["GradeName"].ToString(),
                                Standard = oSqlDataReader["Standard_Name"].ToString(),
                                StudentCount = Convert.ToInt16(oSqlDataReader["StudentCount"]),
                            };

                            lstGradeStandardNameDetails.Add(oGradeStandardNameDetails);
                        }
                    }
                }

                return lstGradeStandardNameDetails;
            }

            #endregion
	}
}
