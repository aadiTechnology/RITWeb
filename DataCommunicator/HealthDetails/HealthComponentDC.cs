// -----------------------------------------------------------------------
// File Name : HealthComponentDC.cs
// Creator : Sachin Wagh
// Created Date : 22-Nov-2018
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
    public class HealthComponentDC
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
        public HealthComponentDC()
        {

        }
         /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
        public HealthComponentDC(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;            
            this.miAcademicYearId = aiAcademicYearId;
			this.miUpdatedById = aiUpdatedById;
        }        
        #endregion

        #region Public Method(s)
        /// <summary>
        /// This method is used to insert/update Health Component details. 
        /// </summary>
        /// <param name="aoHealthComponent"></param>
        /// <returns>Entity list of Health Component details</returns>
        public List<HealthComponent> GetAll(int aiHealthComponentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiHealthComponentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllHealthComponent"))
                    return FillHealthComponent(oSqlDataReader);
            }
        }         
        /// <summary>
        /// This method is used to save Health Component details. 
        /// </summary>
        /// <param name="aoHealthComponent"></param>
        public void Save(HealthComponent aoHealthComponent)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aoHealthComponent.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoHealthComponent.ComponentName, SqlDbType.Text);
                oSQLServerDbUtility.AddParameter("SortOrder", aoHealthComponent.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFitnessComponent", aoHealthComponent.IsFitnessComponent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveHealthComponent");
            }
        }
        /// <summary>
        /// This method is used to delete Health Component details. 
        /// </summary>
        /// <param name="aiHealthComponentId"></param>
        public void Delete(int aiHealthComponentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiHealthComponentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHealthComponent");
            }
        }
        /// <summary>
        /// This method is used to fill Health Component details. 
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<HealthComponent> FillHealthComponent(SqlDataReader aoSqlDataReader)
        {
            List<HealthComponent> lstHealthComponent = new List<HealthComponent>();
            while (aoSqlDataReader.Read())
            {
                HealthComponent oHealthComponent = new HealthComponent
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    ComponentName = Convert.ToString(aoSqlDataReader["Name"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    IsFitnessComponent = Convert.ToBoolean(aoSqlDataReader["IsFitnessComponent"])                                       
                };
                lstHealthComponent.Add(oHealthComponent);
            }
            return lstHealthComponent;
        }
        #endregion
    }
}
