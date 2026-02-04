using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Data;


namespace DataCommunicator
{
   public class WorkinghoursDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private List<WorkinghrsDetails> mlstWorkinHoursDetails;

        #endregion

        #region " Constructor "

        public WorkinghoursDC() { }
        public WorkinghoursDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }
        public WorkinghoursDC(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region "Property(s)"

        public List<WorkinghrsDetails> WorkinHoursDetails
        {
            get { return this.mlstWorkinHoursDetails; }
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
                {
                 List<WorkinghrsDetails> lstWorkingHrsDetails =  FillDivisionsForStandard(oSqlDataReader);
                 if (oSqlDataReader.NextResult())
                     FillWorkinHoursDetails(oSqlDataReader);
                 return lstWorkingHrsDetails;
                }
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

        private void FillWorkinHoursDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstWorkinHoursDetails = new List<WorkinghrsDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstWorkinHoursDetails.Add
                    (
                        new WorkinghrsDetails
                        {
                            WeekdayNumber = Convert.ToInt32(aoSqlDataReader["WeekdayNumber"]),
                            FullHours = Convert.ToDecimal(aoSqlDataReader["FullHours"]),
                            HalfHours = Convert.ToDecimal(aoSqlDataReader["HalfHours"]),
                            DivisionId = Convert.ToInt32(aoSqlDataReader["Division_Id"])
                        }
                    );
            }
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

        public  List<WorkinghrsDetails> Get(int aiStandardId)
        {
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDivision"))
                    return GetAll(oSqlDataReader);
            }
        }


        private List<WorkinghrsDetails> GetAll(SqlDataReader aoSqlDataReader)
        {
            List<WorkinghrsDetails> olstWorkinghrsDetails = new List<WorkinghrsDetails>();
            while (aoSqlDataReader.Read())
            {
                WorkinghrsDetails oWorkinghrsDetails = new WorkinghrsDetails();
                oWorkinghrsDetails.DivisionId=Convert.ToInt32(aoSqlDataReader["Division_Id"]);
                oWorkinghrsDetails.WeekdayNumber=Convert.ToInt32(aoSqlDataReader["WeekdayNumber"]);
                oWorkinghrsDetails.FullHours = Convert.ToInt32(aoSqlDataReader["FullHours"]);
                oWorkinghrsDetails.HalfHours = Convert.ToInt32(aoSqlDataReader["HalfHours"]);
                olstWorkinghrsDetails.Add(oWorkinghrsDetails);
            }
            return olstWorkinghrsDetails;
        }

            
        

    }
        #endregion
}
