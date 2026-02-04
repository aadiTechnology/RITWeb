/*
 * Created By:- Rohini
 * Date:- 28 Jun 2013
 * Description: This class is used to get and save user's education details and experience details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MasterEntities;
using SchoolEntities;

namespace DataCommunicator
{

	/// <summary>
	/// This class is used to save, update and delete user's experience and education details.
	/// </summary>
	public class UserDetailsDC
	{
		#region "Data Member"

		private int miSchoolId = 0;
		private int miAcademicYearId = 0;
		private int miUserId = 0;

		#endregion

		#region "Constructor"
		
		public UserDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
		{
			miSchoolId = aiSchoolId;
			miAcademicYearId = aiAcademicYearId;
			miUserId = aiUserId;
		}

		#endregion

		#region "Public Method"

		/// <summary>
		/// This method is used to get list of education details of user.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public static List<UserEducationDetails> GetEducationDetailsList(int aiUserId)
		{
			List<UserEducationDetails> lstEducationDetails = new List<UserEducationDetails>();
			Qualification oQualification;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", 0, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetEducationDetails"))
				{
					if (oSqlDataReader.HasRows)
					{
						while (oSqlDataReader.Read())
						{
							oQualification = new Qualification() { Id = Convert.ToInt32(oSqlDataReader["Qualification_Id"]), Name = oSqlDataReader["Qualification"].ToString() };
							lstEducationDetails.Add(ReadObjectFromReader(oQualification, oSqlDataReader));
						}
					}
				}
			}
			return lstEducationDetails;
		}

		/// <summary>
		/// This method is used to get list of experience details.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public List<UserExperienceDetails> GetExperienceDetailsList(int aiUserId)
		{
			
			List<UserExperienceDetails> lstExperienceDetails = new List<UserExperienceDetails>();			
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", 0, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExperienceDetails"))
				{
					if (oSqlDataReader.HasRows)
					{
						while (oSqlDataReader.Read())
						{
							lstExperienceDetails.Add(ReadObjectFromReader(oSqlDataReader));
						}
					}
				}
			}
			
			return lstExperienceDetails;
		}
		
		/// <summary>
		/// This methhod is uesd to get education details for update.
		/// </summary>
		/// <param name="aiEducationDetailsId"></param>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public static UserEducationDetails GetEducationDetails(int aiEducationDetailsId, int aiUserId)
		{
			UserEducationDetails oUserEducationDetails = null;
			Qualification oQualification;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", aiEducationDetailsId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetEducationDetails"))
				{
					if (oSqlDataReader.HasRows)
					{
						oSqlDataReader.Read();
						oQualification = new Qualification() { Id = Convert.ToInt32(oSqlDataReader["Qualification_Id"]), Name = oSqlDataReader["Qualification"].ToString() };
						oUserEducationDetails = ReadObjectFromReader(oQualification, oSqlDataReader);
					}
				}
			}
			return oUserEducationDetails;
		}
		
		/// <summary>
		/// This method is used to get experience details for update.
		/// </summary>
		/// <param name="aiExperienceDetailsId"></param>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public UserExperienceDetails GetExperienceDetails(int aiExperienceDetailsId, int aiUserId)
		{
			UserExperienceDetails oUserExperienceDetails =null;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", aiExperienceDetailsId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExperienceDetails"))
				{
					if (oSqlDataReader.HasRows)
					{
						oSqlDataReader.Read();
						oUserExperienceDetails = ReadObjectFromReader(oSqlDataReader);
					}
				}
			}
			
			return oUserExperienceDetails;
		}

		/// <summary>
		/// This method is used to get all applicable documents.
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
		public List<UserDocument> GetApplicableDocumentList(int aiUserId)
		{
			List<UserDocument> lstDocuments = new List<UserDocument>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetApplicableDocument"))
				{
					if (oSqlDataReader.HasRows)
					{
						while (oSqlDataReader.Read())
						{
							lstDocuments.Add(new UserDocument
							{
								Id = Convert.ToInt32(oSqlDataReader["Id"]),
								Name = oSqlDataReader["Name"].ToString(),
								DocumentCount = Convert.ToInt32(oSqlDataReader["Count"]),
								DocumentTypeId = Convert.ToInt32(oSqlDataReader["DocumentTypeId"])
							}
							);
						}
					}
				}
			}
			
			return lstDocuments;
		}

		/// <summary>
		/// This method is used to insert/update experience details.
		/// </summary>
		/// <param name="aoUserExperienceDetails"></param>
		public void SaveExperienceDetails(UserExperienceDetails aoUserExperienceDetails)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("Id",aoUserExperienceDetails.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("JoiningDate", aoUserExperienceDetails.JoiningDate, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("LeftDate", aoUserExperienceDetails.LeftDate, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SchoolName", aoUserExperienceDetails.Organization, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("UserId", aoUserExperienceDetails.UserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUserExperienceDetails");
			}
		}

		/// <summary>
		/// This method is used to insert or update education details.
		/// </summary>
		/// <param name="aoUserEducationDetails"></param>
		public void SaveEducationDetails(UserEducationDetails aoUserEducationDetails)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("Id", aoUserEducationDetails.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("QualificationId", aoUserEducationDetails.Qualification.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("YearOfPassing", aoUserEducationDetails.YearOfPassing, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ClassId", aoUserEducationDetails.PassClassId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserId", aoUserEducationDetails.UserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("InsertedById", aoUserEducationDetails.InsertedById, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("University", aoUserEducationDetails.University, SqlDbType.NVarChar);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUserEducationDetails");
			}
		}

		/// <summary>
		/// This method is used delete education details
		/// </summary>
		/// <param name="aiEducationDetailsId"></param>
		/// <param name="aiUserId"></param>
		public void DeleteEducationDetails(int aiEducationDetailsId, int aiUserId)
		{
			List<UserDocument> lstDocument = new List<UserDocument>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", aiEducationDetailsId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteEducationDetails");
				
			}
		}

		/// <summary>
		/// This method is delete experience details.
		/// </summary>
		/// <param name="aiExperienceDetailsId"></param>
		/// <param name="aiUserId"></param>
		public void DeleteExperienceDetails(int aiExperienceDetailsId, int aiUserId)
		{
			List<UserDocument> lstDocument = new List<UserDocument>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", aiExperienceDetailsId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteExperiencDetails");			
			}
		}

		#endregion

		#region"Private Method"

		/// <summary>
		/// This method is used to read data from sql reader and return object of UserExperienceDetails class.
		/// </summary>
		/// <param name="oSqlDataReader"></param>
		/// <returns></returns>
		private static UserExperienceDetails ReadObjectFromReader(SqlDataReader oSqlDataReader)
		{
			return new UserExperienceDetails()
			{
				Id = Convert.ToInt32(oSqlDataReader["ExperienceDetailsId"]),
				SchoolName = oSqlDataReader["SchoolName"].ToString(),
				JoiningDate = Convert.ToDateTime(oSqlDataReader["JoiningDate"]),
				LeftDate = Convert.ToDateTime(oSqlDataReader["leftDate"]),
				AttachmentCount = oSqlDataReader["Count"]!=DBNull.Value ?Convert.ToInt32(oSqlDataReader["Count"]):0
			};
		}

		/// <summary>
		/// This method is used to read data from sql reader and return object of UserEducationDetails class.
		/// </summary>
		/// <param name="oQualification"></param>
		/// <param name="oSqlDataReader"></param>
		/// <returns></returns>
		private static UserEducationDetails ReadObjectFromReader(Qualification oQualification, SqlDataReader oSqlDataReader)
		{
			return new UserEducationDetails()
			{
				Id = Convert.ToInt32(oSqlDataReader["Id"]),
				YearOfPassing = oSqlDataReader["YearOfPassing"].ToString(),
				Qualification = oQualification,
				University = oSqlDataReader["University"].ToString(),
				PassClassId = Convert.ToInt32(oSqlDataReader["Class_Id"]),
				Class = oSqlDataReader["Class"].ToString()
			};
		}

		#endregion
	}
}
