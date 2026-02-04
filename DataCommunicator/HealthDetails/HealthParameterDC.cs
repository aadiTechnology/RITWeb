// -----------------------------------------------------------------------
// File Name : HealthParameterDC.cs
// Creator :  Sachin Wagh
// Created Date : 10-12-2018
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SchoolEntities;

namespace DataCommunicator
{
    public class HealthParameterDC
    {
         #region Data Member(s)

		private int miSchoolId;
		private int miFinYearId;		
		private int miAcademicYearId;
		private int miUpdatedById;
     
        #endregion

		#region Constructor(s)
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public HealthParameterDC()
        {

        }
         /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public HealthParameterDC(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;            
            this.miAcademicYearId = aiAcademicYearId;
			this.miUpdatedById = aiUpdatedById;
        }        
        #endregion

        #region Public Method(s)
        
        /// <Summary>
        ///This Methos is used to get the All the Health Component from the HealthComponents table
        ///</Summary>
        public List<HealthParameter> GetAll(int aiHealthParameterId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiHealthParameterId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllHealthParameter"))
                    return FillHealthParameter(oSqlDataReader);
            }
        }
        /// <Summary>
        ///This function is used to insert the health parameter details 
        ///</Summary> 
        public void Save(HealthParameter aoHealthParameter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aoHealthParameter.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoHealthParameter.ParameterName, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("ComponentId", aoHealthParameter.HealthComponentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test", aoHealthParameter.TestName, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("Measure", aoHealthParameter.Measure, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("SortOrder", aoHealthParameter.SortOrder, SqlDbType.Int);               
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveHealthParameter");
            }
        }
        /// <Summary>
        ///This function is used to delete the health parameter details 
        ///</Summary> 
        public void Delete(int aiHealthParameterId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiHealthParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHealthParameter");
            }
        }
        /// <Summary>
        ///This function is used to fill health parameter details 
        ///</Summary> 
        private List<HealthParameter> FillHealthParameter(SqlDataReader aoSqlDataReader)
        {
            List<HealthParameter> lstHealthParameter = new List<HealthParameter>();
            while (aoSqlDataReader.Read())
            {
                HealthParameter oHealthParameter = new HealthParameter
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    ParameterName = Convert.ToString(aoSqlDataReader["Name"]),
                    HealthComponentId = Convert.ToInt32(aoSqlDataReader["ComponentId"]),
                    TestName = Convert.ToString(aoSqlDataReader["Test"]),
                    Measure = Convert.ToString(aoSqlDataReader["Measure"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ComponentName = Convert.ToString(aoSqlDataReader["ComponentName"]),                                                           
                };
                lstHealthParameter.Add(oHealthParameter);  
            }
            return lstHealthParameter;
        }   
        #endregion
    }
}
