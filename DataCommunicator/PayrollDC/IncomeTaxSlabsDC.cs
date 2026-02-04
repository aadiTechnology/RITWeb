// -----------------------------------------------------------------------
// File Name : IncomeTaxSlabsDC.cs
// Creator : Sunny
// Created Date : 15-Mar-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{	

	/// <summary>
	///This class is used to communicate with database to insert,update,delete and select income tax slabs.
	/// </summary>
	public class IncomeTaxSlabsDC
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
        public IncomeTaxSlabsDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="aiAcademicYearId"></param>
		public IncomeTaxSlabsDC(int aiSchoolId, int aiFinYearId, int aiAcademicYearId,int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;            
            this.miAcademicYearId = aiAcademicYearId;
			this.miUpdatedById = aiUpdatedById;
        }

        #endregion		

		#region Public Method(s)

		/// <summary>
		/// This method is used to return all categories for ITSlab.
		/// </summary>
        public List<ITSlabCategory> GetAllCategories()
        {
            List<ITSlabCategory> lstITSlabCategories = new List<ITSlabCategory>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCategoriesForITRanges"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ITSlabCategory oITSlabCategories = new ITSlabCategory
                         {
                             Id = Convert.ToInt32(oSqlDataReader["Id"]),
                             Name = Convert.ToString(oSqlDataReader["Name"]),
                             UptoAge = Convert.ToInt32(oSqlDataReader["UptoAge"]),
                             FromAge = Convert.ToInt32(oSqlDataReader["FromAge"])
                         };
                        lstITSlabCategories.Add(oITSlabCategories);
                    }
                }
                return lstITSlabCategories;
            }
        }

		/// <summary>
		/// This method is used to return all income tax slabs details.
		/// </summary>
		/// <returns></returns>
		public List<IncomeTaxSlab> GetAll()
		{			
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
				using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllIncomeTaxRanges"))
                return FillIncomeTaxSlabs(oSqlDataReader);
			}			
		}

		/// <summary>
		/// This method is used to insert/update income tax slab details. 
		/// </summary>
		/// <param name="aoIncomeTaxSlab"></param>
		public void Save(IncomeTaxSlab aoIncomeTaxSlab)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id",aoIncomeTaxSlab.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("CategoryId", aoIncomeTaxSlab.Category.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("FromAmount", aoIncomeTaxSlab.FromAmount, SqlDbType.Decimal);
				oSQLServerDbUtility.AddParameter("ToAmount", aoIncomeTaxSlab.ToAmount, SqlDbType.Decimal);				
				oSQLServerDbUtility.AddParameter("Percentage", aoIncomeTaxSlab.Percentage, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveIncomeTaxRange");
			}
		}

		/// <summary>
		/// This method is used to delete income tax slab details.
		/// </summary>
		/// <param name="aiIncomeTaxRangeId"></param>
		public void Delete(int aiIncomeTaxRangeId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Id", aiIncomeTaxRangeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);			    
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteIncomeTaxRange");
			}
		}

		/// <summary>
		/// This method is used to get maximum To amount for given category.
		/// </summary>
		/// <param name="aiCategoryId"></param>
		/// <returns></returns>
		public int GetMaxToAmount(int aiCategoryId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
				SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("MaxToAmount", 0, SqlDbType.Int, ParameterDirection.Output);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetMaxIncomeTaxRangeAmount");
				return Convert.ToInt32(oSqlParameter.Value);
				
			}
		}

        /// <summary>
        /// This method is used to fill up income tax slab entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<IncomeTaxSlab> FillIncomeTaxSlabs(SqlDataReader aoSqlDataReader)
        {
            List<IncomeTaxSlab> lstIncomeTaxSlab = new List<IncomeTaxSlab>();
            while (aoSqlDataReader.Read())
            {
                IncomeTaxSlab oIncomeTaxSlab = new IncomeTaxSlab
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Category = new ITSlabCategory
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["CategoryId"]),
                        Name = Convert.ToString(aoSqlDataReader["CategoryName"])
                    },

                    FromAmount = Convert.ToInt32(aoSqlDataReader["FromAmount"]),
                    ToAmount = Convert.ToInt32(aoSqlDataReader["ToAmount"]),
                    FixedAmount = Convert.ToInt32(aoSqlDataReader["FixedAmount"]),
                    Percentage = Convert.ToDouble(aoSqlDataReader["Percentage"])
                };
                lstIncomeTaxSlab.Add(oIncomeTaxSlab);
            }
            return lstIncomeTaxSlab;
        }

		#endregion

	}
}
