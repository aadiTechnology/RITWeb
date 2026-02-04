/*
 * Creator:Rohini
 * Date:29 May 2012
 * Description : This class is used to insert/update  homework details.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MasterEntities;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	/// <summary>
	/// This class is used to get and upadate homework details.
	/// </summary>
	public class HomeworkDC
	{
		#region "Data Member"

		private int miSchoolId = 0;
		private int miAcademicYearId = 0;
		private int miUserId = 0;

		#endregion

		#region "Constructor"

		public HomeworkDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
		{
			miSchoolId = aiSchoolId;
			miAcademicYearId = aiAcademicYearId;
			miUserId = aiUserId;
		}

		#endregion

		#region "Public Method"
		/// <summary>
		/// This method is used to get homework details according to standard division.
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiSubjectId"></param>
		/// <param name="abFlag"></param>
		/// <returns></returns>
		public List<Homework> GetListForTeacher(int aiStdDivId, string asDate , string asHomeWorkStatus ,string asTitle)
		{
			List<Homework> lstHomework = new List<Homework>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StandarDivisionId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HomeWorkStatus", asHomeWorkStatus, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("AssignedDate", asDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("HomeworkTitle", asTitle, SqlDbType.NVarChar);
				using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHomeworkListForTeacher"))
				{
					if (oReader.HasRows)
					{
						while (oReader.Read())						
							lstHomework.Add(ReadObjectFromReader(oReader));						
					}

					
				}
			}

			return lstHomework;
		}
		
		/// <summary>
		/// This method is used to get homework details according to standard division.
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiTeacherId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiSubjectId"></param>
		/// <param name="abFlag"></param>
		/// <returns></returns>
		public List<Homework> GetListForStudent(int aiStdDivId, string asDate, string asHomeWorkStatus)
		{
			List<Homework> lstHomework = new List<Homework>();

			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StandarDivisionId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HomeWorkStatus", asHomeWorkStatus, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("AssignedDate", asDate, SqlDbType.NVarChar);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHomeworkListforStudent"))
				{
					if (oReader.HasRows)
					{
						while (oReader.Read())
							lstHomework.Add(ReadObjectFromReader(oReader));
					}

		
				}
			}

			return lstHomework;
		}

		/// <summary>
		/// This method is used to get homework details for provided id.
		/// </summary>
		/// <param name="aiHomeWorkId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public Homework Get(int aiHomeWorkId)
		{
			Homework oHomework = null;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("HomeworkId", aiHomeWorkId, SqlDbType.Int);
				using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHomeworkDetails"))
				{
					if (oReader.HasRows)
					{
						oReader.Read();
						oHomework = ReadObjectFromReader(oReader);                        
                        if (oReader.NextResult())
                        {
                            while (oReader.Read())
                            {
                                oHomework.LinkedDivisions.Add(oReader["Division_Id"].ToInt());
                            }
                        }
					}

			
				}
				return oHomework;
			}
		}

		/// <summary>
		/// This method is used to insert or update homework details.
		/// </summary>
		/// <param name="asXml"></param>
		/// <param name="abUpdateFlag"></param>
		/// <returns></returns>
		public void Save(Homework aoHomework,string asFileName)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("Id", aoHomework.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Title", aoHomework.Title, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SubjectId", aoHomework.Subject.SubjectId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StdDivId", aoHomework.StandardDivisionId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AttachmentPath", aoHomework.AttachmentPath, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("Details", aoHomework.Details, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("AssignDate", aoHomework.AssignedDate, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("CompleteByDate", aoHomework.CompleteByDate, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("InsertedById", aoHomework.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FileName", asFileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("DivisionIds", aoHomework.DivisionIds, SqlDbType.NVarChar);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_SaveHomeworkDetails]");			
			}			
		}

		/// <summary>
		/// This method is used to update IsPublished flag for homework.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
        public void Publish(string asHomeworkIds, bool abIsSMSSent)
		{
            using (SQLServerDbUtility oSQLServerDbUtility = LoadPublishUnpublishParameters(asHomeworkIds, string.Empty, true, abIsSMSSent))
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_PublishUnpublishHomework");			
		}

		/// <summary>
		/// This method is used to unpublish homework details.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public void UnPublish(string asHomeworkIds, string asReason)
		{
            using (SQLServerDbUtility oSQLServerDbUtility = LoadPublishUnpublishParameters(asHomeworkIds, asReason, false, false))
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_PublishUnpublishHomework");
		}

        /// <summary>
        /// This method is used to return homework sms status.
        /// </summary>
        /// <param name="aiStdDivId"></param>
        /// <param name="adtAssignedDate"></param>
        /// <returns></returns>
        public bool IsHomeworkSMSSent(int aiStdDivId, DateTime adtAssignedDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", adtAssignedDate, SqlDbType.Date);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Status", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsHomeworkSMSSent");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

		/// <summary>
		/// This method is used to delete homework.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
        public void Delete(int aiHomeworkId, string asDeleteFromAllClasses)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("HomeworkId", aiHomeworkId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DeleteFromAllClasses", asDeleteFromAllClasses, SqlDbType.NVarChar);                
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHomework");				
			}
		}

        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public string DeleteDocument(int aiHomeworkId, string asDeleteFromAllClasses)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HomeworkId", aiHomeworkId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DeleteFromAllClasses", asDeleteFromAllClasses, SqlDbType.Char);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("FileNameToDelete", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output,200);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHomeworkDocument");
                return OSqlParameter.Value.ToString();
            }
        }

        /// <summary>
        ///  This method is used to return Homework documents.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<Homework> GetDocuments(int aiHomeworkId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HomeWorkId", aiHomeworkId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
             
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllHomeworkDocuments"))
                {

                    List<Homework> lstHomework = new List<Homework>();
                    Homework oHomeworkDocument;
                    while (oSqlDataReader.Read())
                    {
                        oHomeworkDocument = new Homework
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            AttachmentsName = Convert.ToString(oSqlDataReader["AttachmentName"]),
                            HasLinkedHomework = Convert.ToBoolean(oSqlDataReader["HasLinkedHomework"])
                        };
                        lstHomework.Add(oHomeworkDocument);
                    }
                    return lstHomework;
                }
            }
        }
		#endregion

		#region "Private Method"
		/// <summary>
		/// This method is used to read values from reader and return homework class object.
		/// </summary>
		/// <param name="aoReader"></param>
		/// <returns></returns>
		private Homework ReadObjectFromReader(SqlDataReader aoReader)
		{
			return new Homework()
			{
				Id = aoReader["Id"].ToInt(),
				Title = aoReader["Title"].ToString(),
				Details = aoReader["Details"].ToString(),
				Subject = new SubjectMaster() { SubjectName = aoReader["Subject"].ToString(), SubjectId = aoReader["SubjectId"].ToInt() },
				IsPublished = aoReader["IsPublished"].ToBool(),
				AttachmentPath = aoReader["AttachmentPath"].ToString(),
				AssignedDate = Convert.ToDateTime(aoReader["AssignedDate"]),
				CompleteByDate = aoReader["CompleteByDate"].ToDateTime(),
                Flag=aoReader["flag"].ToInt(),
                HasLinkedHomework = aoReader["HasLinkedHomework"].ToBool(),
                LinkedDivisions = new List<int>()
			};
		}

		/// <summary>
		/// This method is used to set values to store procedure parameters.
		/// </summary>
		/// <param name="aiHomeworkId"></param>
		/// <param name="asReason"></param>
		/// <param name="aoSQLServerDbUtility"></param>
		private SQLServerDbUtility LoadPublishUnpublishParameters(string asHomeworkIds, string asReason, bool abIsPublish, bool abIsSMSSent = false)
		{
			SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility();
            oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("HomeworkIds", asHomeworkIds, SqlDbType.NVarChar);
			oSQLServerDbUtility.AddParameter("UnpublishReason", asReason, SqlDbType.NVarChar);
			oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
			oSQLServerDbUtility.AddParameter("IsPublished", abIsPublish, SqlDbType.Bit);
            oSQLServerDbUtility.AddParameter("IsSMSSent", abIsSMSSent, SqlDbType.Bit);
			return oSQLServerDbUtility;
		}
						
		#endregion 
	}
}
