// Class Name       :- SchoolWorkingDetailsDC
// Purpose          :- This class is used to manage school working details.
// Date Of creation :- 29/11/2016
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class SchoolWorkingDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miInsertedById;        
        private List<SchoolWorkinDivisionDetails> mlstDivisionDetails;
        private List<SchoolWorkingStdDivDetails> mlstStdDivDetails;
        private List<SchoolWorkingDetails> mlstWorkingDetails;

        #endregion

        #region " Constructor "

        public SchoolWorkingDetailsDC() { }       

        public SchoolWorkingDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }

        #endregion

        #region "Property(s)"

        public List<SchoolWorkinDivisionDetails> SchoolWorkinDivisionDetails
        {
            get { return this.mlstDivisionDetails; }
        }

        public List<SchoolWorkingStdDivDetails> SchoolWorkingStdDivDetails
        {
            get { return this.mlstStdDivDetails; }
        }

        public List<SchoolWorkingDetails> SchoolWorkingDetails
        {
            get { return this.mlstWorkingDetails; }
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all standard divisions to fill the first listview.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="dtHalfDayDate"></param>
        public List<SchoolWorkingStandardDetails> GetAll(DateTime adtHalfDayDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HalfDayDate", adtHalfDayDate, SqlDbType.DateTime);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStandardDivisionForHalfDay"))
                {
                    List <SchoolWorkingStandardDetails> lstStandardDetails =  FillStandardDivisions(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillDivisionDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStdDivDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillHalfDayStandardDivDetails(oSqlDataReader);
                    return lstStandardDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to save ll Half day configuration details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asStandDivIds"></param>
        /// <param name="dtDate"></param>
        public void Save(string asStandDivIds, DateTime adtDate, DateTime adtOldDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandDivIds", asStandDivIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("HalfDayDate", adtDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("HalfDayOldDate", adtOldDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveClasswiseHalfDayConfig");
            }
        }

        /// <summary>
        /// This method is used to get all Datewise half day details to fill the second listview.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="asSortDirection"></param>
        public List<SchoolWorkingDetails> Get(int aiSchoolId, int aiAcademicYearId, int aiEndIndex, int aiStartIndex, string asSortDirection)
        { 
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortDirestion", asSortDirection, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllDatewiseHalfDayDetails"))
                {
                    List<SchoolWorkingDetails> lstWorkingDetails = FillDatewiseWorkingDetails(oSqlDataReader);

                    return lstWorkingDetails;
                }
             }
        }

        /// <summary>
        /// This method is used to get count of all Datewise half day details to fill the listview.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        public int Count(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCountOfHalfDayDates");

                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to delete datewise Halfday details configuration.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void Delete(DateTime adtHalfDayDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HalfDayDate", adtHalfDayDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHalfDayConfiguration");
            }
        }

        /// <summary>
        /// This method is used to fill StandardDivision Detils.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<SchoolWorkingStandardDetails> FillStandardDivisions(SqlDataReader aoSqlDataReader)
        {
            List<SchoolWorkingStandardDetails> olstSchoolWorkingStandardDetails = new List<SchoolWorkingStandardDetails>();
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    SchoolWorkingStandardDetails oSchoolWorkingStandardDetails = new SchoolWorkingStandardDetails();
                    oSchoolWorkingStandardDetails.StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]);
                    oSchoolWorkingStandardDetails.StandardName = Convert.ToString(aoSqlDataReader["StandardName"]);

                    olstSchoolWorkingStandardDetails.Add(oSchoolWorkingStandardDetails);
                }
            }
            return olstSchoolWorkingStandardDetails;
        }

        /// <summary>
        /// This method is used to get all Division Details to fill.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillDivisionDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstDivisionDetails = new List<SchoolWorkinDivisionDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstDivisionDetails.Add
                    (
                        new SchoolWorkinDivisionDetails
                        {
                            DivisionId = Convert.ToInt32(aoSqlDataReader["DivisionId"]),
                            DivisionName = Convert.ToString(aoSqlDataReader["DivisionName"])
                            
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to get all students division details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStdDivDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstStdDivDetails = new List<SchoolWorkingStdDivDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstStdDivDetails.Add
                    (
                        new SchoolWorkingStdDivDetails
                        {
                            StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]),
                            StandardName = Convert.ToString(aoSqlDataReader["StandardName"]),
                            OriginalStandardId = Convert.ToInt32(aoSqlDataReader["OriginalStandardId"]),
                            DivisionID = Convert.ToInt32(aoSqlDataReader["DivisionId"]),
                            DivisionName = Convert.ToString(aoSqlDataReader["DivisionName"]),
                            OriginalDivisionId = Convert.ToInt32(aoSqlDataReader["OriginalDivisionId"]),
                            StandardDivisionId = Convert.ToInt32(aoSqlDataReader["StandardDivisionId"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill half day standard division details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillHalfDayStandardDivDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstWorkingDetails = new List<SchoolWorkingDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstWorkingDetails.Add
                (
                    new SchoolWorkingDetails
                    {
                        HalfDayDetailsId = Convert.ToInt32(aoSqlDataReader["Id"]),
                        HalfDayDate = Convert.ToDateTime(aoSqlDataReader["Date"]),
                        StandardId = Convert.ToInt32(aoSqlDataReader["Standard_Id"]),
                        OriginalStandardId = Convert.ToInt32(aoSqlDataReader["Original_Standard_Id"]),
                        DivisionID = Convert.ToInt32(aoSqlDataReader["Division_Id"]),
                        OriginalDivisionId = Convert.ToInt32(aoSqlDataReader["Original_Division_Id"]),
                    }
                );
            }
        }

        /// <summary>
        /// This method is used to fill datewise Half day configuration details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<SchoolWorkingDetails> FillDatewiseWorkingDetails(SqlDataReader aoSqlDataReader)
        {
            List<SchoolWorkingDetails> olstWorkingDetails = new List<SchoolWorkingDetails>();
            while (aoSqlDataReader.Read())
            {
                SchoolWorkingDetails oSchoolWorkingDetails = new SchoolWorkingDetails();
                oSchoolWorkingDetails.HalfDayDetailsId = Convert.ToInt32(aoSqlDataReader["Id"]);
                oSchoolWorkingDetails.HalfDayDate = Convert.ToDateTime(aoSqlDataReader["Date"]);
                oSchoolWorkingDetails.ClassName = Convert.ToString(aoSqlDataReader["ClassName"]);

                olstWorkingDetails.Add(oSchoolWorkingDetails);
            }
            return olstWorkingDetails;
        }

        #endregion
    }
}
