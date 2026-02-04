using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Data;


namespace DataCommunicator
{
   public class WorkinghrsDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public WorkinghrsDC() { }
        public WorkinghrsDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }
        public WorkinghrsDC(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region Method(s)
        /// <summary>
        /// This method is used to get all division as per the selected StandardId
       /// </summary>
       /// <param name="aiStandardId"></param>
       /// <returns></returns>
        public List<WorkinghrsDetails> GetAllDivisionsForStandard(int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDivision"))
                    return FillDivisionsForStandard(oSqlDataReader);
            }
        }

       /// <summary>
       /// THis method returns the all divisions.
       /// </summary>
       /// <param name="aoSqlDataReader"></param>
       /// <returns></returns>
        private List<WorkinghrsDetails> FillDivisionsForStandard(SqlDataReader aoSqlDataReader)
        {
            List<WorkinghrsDetails> olstWorkinghrsDetails = new List<WorkinghrsDetails>();
            while (aoSqlDataReader.Read())
            {
                WorkinghrsDetails oWorkinghrsDetails = new WorkinghrsDetails();
                oWorkinghrsDetails.DivisionId = Convert.ToInt32(aoSqlDataReader["Division_Id"]);
                oWorkinghrsDetails.DivisionName = Convert.ToString(aoSqlDataReader["Division_Name"]);
                olstWorkinghrsDetails.Add(oWorkinghrsDetails);
            }
            return olstWorkinghrsDetails;
        }
       /// <summary>
        /// This method is used insert the Working hours details and save of that details.
       /// </summary>
       /// <param name="asXml"></param>
       /// <param name="aiInsertedById"></param>

        public void InsertWorkingHrsDetails(int  aiStandardId,string asXml,int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GradeXML", asXml, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertWorkingHoursDetails");
            };
        }


    }
        #endregion
}
