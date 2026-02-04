// Class Name       :- PerformanceGradeDC
// Purpose          :- This class is used to manage Performance Grade details.
// Date Of creation :- 15-Sept-2013
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StaffPerformanceEntity;

namespace DataCommunicator 
{    
    public class PerformanceGradeDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miUpdatedById;

        #endregion "Data Members"

        #region "Constructors"

        public PerformanceGradeDC()
        {            
        }

        public PerformanceGradeDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;            
        }

        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all grade details.
        /// </summary>
        /// <returns></returns>
        public List<PerformanceGrade> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPerformanceGrades"))
                    return this.SetGradetDetails(oSqlDataReader);
            }                         
        }

        /// <summary>
        /// This method is used to set values to property.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public List<PerformanceGrade> SetGradetDetails(SqlDataReader aoSqlDataReader)
        {
            List<PerformanceGrade> lstGradeDetails = new List<PerformanceGrade>();
            PerformanceGrade oPerformanceGrade = null;
            while (aoSqlDataReader.Read())
            {
                oPerformanceGrade = new PerformanceGrade
                {
                    GradeId = Convert.ToInt32(aoSqlDataReader["Id"]),
                    GradeName = Convert.ToString(aoSqlDataReader["Name"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    Description = Convert.ToString(aoSqlDataReader["Description"]),
                    OriginalGradeId = Convert.ToInt32(aoSqlDataReader["OriginalGradeId"]),
                    SchoolId = Convert.ToInt32(aoSqlDataReader["SchoolId"]),
                    IsDeleted = Convert.ToBoolean(aoSqlDataReader["IsDeleted"]),                    
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                };
                                
                lstGradeDetails.Add(oPerformanceGrade);
            }

            return lstGradeDetails;
        }

        /// <summary>
        /// This method is used to Insert and Update grade details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        public void Insert(string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GradeXML", asXml, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePerformanceGrades");
            };
        }
        
        #endregion
    }
}
