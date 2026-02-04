using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MasterEntities;
using StudentEntities;
using Utility;

namespace DataCommunicator
{
	public class SecondLanguageDC
	{
		#region "Data Member"

		public int miSchoolId;
		public int miAcademicYearId;
        
		#endregion

		#region "Constructor"

		public SecondLanguageDC(int aiSchoolId, int aiAcademicYearId)
		{
			miAcademicYearId = aiAcademicYearId;
			miSchoolId = aiSchoolId;
		}

		#endregion

		#region "Public Methods"

		/// <summary>
		///		This method is used to get second language.
		/// </summary>
		/// <param name="aiStandardDivisionId"></param>
		/// <returns></returns>
		public List<SubjectMaster> GetAll(int aiStandardDivisionId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolID", miSchoolId, SqlDbType.Int);
				using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSecondLanguage"))
				return SetSecondLanguageSubjects(oSqlDataReader);
			}
		}

		/// <summary>
		/// 	This method is used to get second language.
		/// </summary>
		/// <param name="aiStandardId"> </param>
		/// <param name="aiDivisionId"> </param>
		/// <returns> </returns>
		public List<SubjectMaster> GetAll(int aiStandardId, int aiDivisionId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolID", miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSecondLanguage"))
                    return SetSecondLanguageSubjects(oSqlDataReader);
			}
		}

        /// <summary>
        /// This method is used to check whether any exam is published.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public bool IsAnyExamPublished(int aiStandardId, int aiDivisionId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolID", miSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsExamPublished", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsAnyExamPublished");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

		/// <summary>
		/// 	This method is used set properties.
		/// </summary>
		/// <param name="oSqlDataReader"> </param>
		/// <returns> </returns>
		private List<SubjectMaster> SetSecondLanguageSubjects(SqlDataReader oSqlDataReader)
		{
			var lstSecondLanguage = new List<SubjectMaster>();
			if (oSqlDataReader.HasRows)
			{
				while (oSqlDataReader.Read())
				{
					lstSecondLanguage.Add(new SubjectMaster {
							SubjectName = oSqlDataReader["Subject_Name"].ToString(),
							SubjectId = oSqlDataReader["Subject_Id"].ToInt(),	// This is acutally the Original Subject Id
							Original_Subject_Id = oSqlDataReader["SubjectId"].ToInt(),	// And this is the Yearwise Subject Id.
                            LanguageGroupId = Convert.ToInt32(oSqlDataReader["LanguageGroupId"]),
                            SubjectGroupId = Convert.ToInt32(oSqlDataReader["SubjectGroupId"]),
                            SecondThirdId = Convert.ToInt32(oSqlDataReader["SecondThirdId"])
						});
				}
			}
			return lstSecondLanguage;
		}

		/// <summary>
		/// 	This method is used to update student details.
		/// </summary>
		/// <param name="asXml"> </param>
		/// <param name="aiUpadatedBy"> </param>
		public void Update(string asXml, int aiUpadatedBy, int aiStandardId, int aiDivisionId, int aiYearwiseStudentID = 0)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedBy", aiUpadatedBy, SqlDbType.Int);

                if (asXml != string.Empty)
				    oSQLServerDbUtility.AddParameter("sXml", asXml, SqlDbType.Xml);

                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YrStudentID", aiYearwiseStudentID, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateSecondLanguage");
			}
		}

		#endregion
	}
}