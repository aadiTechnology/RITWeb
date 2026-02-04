using System;
using System.Collections.Generic;
using System.Data;
using SchoolEntities;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class ResultDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public ResultDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public ResultDetailsDC()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// This msthod is used to get listview details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public List<ResultDetails> GetResultDetails(int aiStandardId, int aiDivisionId, int aiTermId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetResultDetails"))
                {
                    List<ResultDetails> lstResultDetails = new List<ResultDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstResultDetails.Add(new ResultDetails
                        {
                            StudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            ConductId = Convert.ToInt32(oSqlDataReader["ConductId"]),
                            PunctualityId = Convert.ToInt32(oSqlDataReader["PunctualityId"]),
                            ResultId = Convert.ToInt32(oSqlDataReader["ResultId"]),
                        });
                    }
                    return lstResultDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get conduct details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetConductList()
        {
            string sSelectStatement = " SELECT " +
                                        " Id " +
                                        " ,Name " +
                                        " FROM " +
                                        " ConductDetails " +
                                        " WHERE " +
                                        "IsDeleted =" + Constants.S_ZERO;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get punctuality details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPunctuationList()
        {
            string sSelectStatement = " SELECT " +
                                        " Id " +
                                        " ,Name " +
                                        " FROM " +
                                        " PunctualityDetails " +
                                        " WHERE " +
                                        "IsDeleted =" + Constants.S_ZERO;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get result details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultList()
        {
            string sSelectStatement = " SELECT " +
                                        " Id " +
                                        " ,Name " +
                                        " FROM " +
                                        " ResultDetails " +
                                        " WHERE " +
                                        "IsDeleted =" + Constants.S_ZERO;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to save result details.
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="aiTermId"></param>
        public void Save(string sXml, int aiTermId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ResultDetailxml", sXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveResultDetails");
            }
        }

        #endregion
    }
}
