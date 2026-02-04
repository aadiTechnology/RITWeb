using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database to insert,update and select Job.
    /// </summary>
    public class JobDetailsDC
    {
        #region Data Member(s)

		private int miSchoolId;		
		private int miUpdatedById;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public JobDetailsDC()
		{
		}

		/// <summary>
		/// Initializes member variables.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiUpdatedById"></param>
        public JobDetailsDC(int aiSchoolId, int aiUpdatedById)
		{
			miSchoolId = aiSchoolId;			
			miUpdatedById = aiUpdatedById;
		}

		#endregion

		#region Public Method(s)

		/// <summary>
		/// This method is used to return all job details.
		/// </summary>
		/// <returns></returns>
        public List<JobDetails> GetAll()
        {
            List<JobDetails> lstJobDetails = new List<JobDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllJobDetails"))
                {
                    while (oSqlDataReader.Read())
                        lstJobDetails.Add(ReadObjectFromReader(oSqlDataReader));
                }
            }
            return lstJobDetails;
        }

        /// <summary>
        /// This method is used to retrive job details for particular ID.
        /// </summary>
        public JobDetails Get(int aiJobId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JobId", aiJobId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSingleJobDetails"))
                {
                    if (oSqlDataReader.Read())
                        return ReadObjectFromReader(oSqlDataReader);
                }
            }
            return null;
        }

		/// <summary>
		/// This method is used to insert/update job details. 
		/// </summary>
        public void Save(JobDetails aoJobDetails)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JobId", aoJobDetails.JobId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("JobTitle", aoJobDetails.JobTitle, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Qualification", aoJobDetails.Qualification, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Description", aoJobDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", aoJobDetails.SortOrder, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Experience", aoJobDetails.Experience, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveJobDetails");
			}
		}

        /// <summary>
        /// This method is used to delete job details.
        /// </summary>
        public void Delete(int aiJobId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("JobId", aiJobId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteJobDetails");
            }
        }


        /// <summary>
        /// This method is used to populate object of job details.
        /// </summary>
        private JobDetails ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            JobDetails oJobDetails = new JobDetails
            {
                JobId = Convert.ToInt32(aoSqlDataReader["JobId"]),
                JobTitle = Convert.ToString(aoSqlDataReader["JobTitle"]),
                Qualification = Convert.ToString(aoSqlDataReader["Qualification"]),
                Description = Convert.ToString(aoSqlDataReader["Description"]),
                SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                IsSelected = Convert.ToBoolean(aoSqlDataReader["IsSelected"]),
                Experience = Convert.ToInt32(aoSqlDataReader["Experience"])
            };

            return oJobDetails;
        }

        /// <summary>
        /// This method is used to save selected job to be displayed on career page.
        /// </summary>
        public void SaveSelectedJob(string asXML)
        {
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("JobsXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSelectedJobs");
            }
        }

        /// <summary>
        /// This method is used to get selected job to be displayed on Career page.
        /// </summary>
        public List<JobDetails> GetSelectedJobDetails(int aiSchoolId)
        {
            List<JobDetails> lstSelectedJobDetails = new List<JobDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSelectedJobDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        JobDetails oJobDetails = new JobDetails
                        {
                            JobId = Convert.ToInt32(oSqlDataReader["JobId"]),
                            JobTitle = Convert.ToString(oSqlDataReader["JobTitle"]),
                            Qualification = Convert.ToString(oSqlDataReader["Qualification"]),
                            Description = Convert.ToString(oSqlDataReader["Description"]),
                            IsSelected = Convert.ToBoolean(oSqlDataReader["IsSelected"]),
                            SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]),
                            Experience = Convert.ToInt32(oSqlDataReader["Experience"])
                        };
                        lstSelectedJobDetails.Add(oJobDetails);
                    }
                }
            }
            return lstSelectedJobDetails;
        }        

		#endregion
    }
}
