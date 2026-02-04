
// Class Name       :- SchoolwiseTermConfigurationMasterDC
// Purpose          :- This class is used to manage SchoolwiseTermConfigurationMaster details.
// Date Of creation :- 2/15/2011
// Author Name      :- Vinod

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using SchoolEntities;
using TermEntities;
//using StudentTestwiseAttendanceEntities;

namespace DataCommunicator
{
    public class SchoolwiseTermConfigurationMasterDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
      
      
     
        private SchoolwiseTermConfigurationDetails moSchoolwiseTermConfigurationDetails = null;
        public List<SchoolwiseTermConfigurationDetails> olstSchoolwiseTermConfigurationDetails = new List<SchoolwiseTermConfigurationDetails>();
        public List<StandardwiseAcademicYearDates> olstStandardwiseAcademicYearDates = new List<StandardwiseAcademicYearDates>();
        public List<EvaluationPeriodDetails> olstEvaluationPeriodDetails = new List<EvaluationPeriodDetails>();
    
        #endregion "Data Members"

        #region "Constructors"

        public SchoolwiseTermConfigurationMasterDC()
        {
            moSchoolwiseTermConfigurationDetails = new SchoolwiseTermConfigurationDetails();
        }

        public SchoolwiseTermConfigurationMasterDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion "Constructors"

        #region "Properties"

        public SchoolwiseTermConfigurationDetails SchoolwiseTermConfigurationDetails
        {
            get { return moSchoolwiseTermConfigurationDetails; }
            set { moSchoolwiseTermConfigurationDetails = value; }
        }

        #endregion "Properties"

        /// <summary>
        /// This method is used to get all term details.
        /// </summary>   
        public void GetAllTermDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSchoolwiseTermDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        LoadTermDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            LoadStandardwiseAcademicYearDates(oSqlDataReader);
                    }
                }
            }
            
        }
        /// <summary>
        /// This method is used to Get All Evaluation Periods.
        /// </summary>   
        public List<EvaluationPeriodDetails> GetAllEvaluationPeriods(int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllEvaluationPeriods"))
                    return this.fillEvaluationPeriods(oSqlDataReader);
                
            }

        }
          ///// <summary>
        ///// This method is used to fill Evaluation Periods.
          ///// </summary>
          ///// <param name="aoSqlDataReader"></param>
          ///// <returns></returns>
        private List<EvaluationPeriodDetails> fillEvaluationPeriods(SqlDataReader aoSqlDataReader)
        {

            List<EvaluationPeriodDetails> lstEvaluationPeriodDetails = new List<EvaluationPeriodDetails>();
            while (aoSqlDataReader.Read())
            {
                EvaluationPeriodDetails oEvaluationPeriodDetails = new EvaluationPeriodDetails();

                oEvaluationPeriodDetails.StandardId = Convert.ToInt32(aoSqlDataReader["Standard_Id"]);
                oEvaluationPeriodDetails.StandardName = Convert.ToString(aoSqlDataReader["Standard_Name"]);

                if (aoSqlDataReader["TestStartDate"] != DBNull.Value)
                    oEvaluationPeriodDetails.TestStartDate = Convert.ToDateTime(aoSqlDataReader["TestStartDate"]);
                else
                    oEvaluationPeriodDetails.TestStartDate = Convert.ToDateTime("1/1/1900");

                if (aoSqlDataReader["TestEndDate"] != DBNull.Value)
                    oEvaluationPeriodDetails.TestEndDate = Convert.ToDateTime(aoSqlDataReader["TestEndDate"]);
                else
                    oEvaluationPeriodDetails.TestEndDate = Convert.ToDateTime("1/1/1900");

                lstEvaluationPeriodDetails.Add(oEvaluationPeriodDetails);
            }

            return lstEvaluationPeriodDetails;
        }
      
      
        /// <summary>
        /// This method is used to load term details
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void LoadTermDetails(SqlDataReader aoSqlDataReader)
        {
            TermConfigurationDetails oTerm1ConfigurationDetails = null;
            TermConfigurationDetails oTerm2ConfigurationDetails = null;
            SchoolwiseTermConfigurationDetails oSchoolwiseTermConfigurationDetails = null;
           
            while (aoSqlDataReader.Read())
            {
                oTerm1ConfigurationDetails = new TermConfigurationDetails
                {
                    TermId = Convert.ToInt32(aoSqlDataReader["Term_Id"]),
                    SchoolwiseTermId = Convert.ToInt32(aoSqlDataReader["SchoolwiseTermId"]),
                    TermStartDate = Convert.ToDateTime(aoSqlDataReader["TermStartDate"] != DBNull.Value ?
                            aoSqlDataReader["TermStartDate"] : "1900-01-01"),
                    TermEndDate = Convert.ToDateTime(aoSqlDataReader["TermEndDate"] != DBNull.Value ?
                            aoSqlDataReader["TermEndDate"] : "1900-01-01"),
                };
                if (aoSqlDataReader.Read())
                {
                    oTerm2ConfigurationDetails = new TermConfigurationDetails
                    {
                        TermId = Convert.ToInt32(aoSqlDataReader["Term_Id"]),
                        SchoolwiseTermId = Convert.ToInt32(aoSqlDataReader["SchoolwiseTermId"]),
                        TermStartDate = Convert.ToDateTime(aoSqlDataReader["TermStartDate"] != DBNull.Value ?
                                aoSqlDataReader["TermStartDate"] : "1900-01-01"),
                        TermEndDate = Convert.ToDateTime(aoSqlDataReader["TermEndDate"] != DBNull.Value ?
                                aoSqlDataReader["TermEndDate"] : "1900-01-01"),
                    };
                }
                oSchoolwiseTermConfigurationDetails = new SchoolwiseTermConfigurationDetails
                {
                    StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]),
                    StandardName = Convert.ToString(aoSqlDataReader["StandardName"]),
                    TermIInfo = oTerm1ConfigurationDetails,
                    TermIIInfo = oTerm2ConfigurationDetails
                };
                olstSchoolwiseTermConfigurationDetails.Add(oSchoolwiseTermConfigurationDetails);
            }
        }

        /// <summary>
        /// This method is used to load standardwise academic year dates.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void LoadStandardwiseAcademicYearDates(SqlDataReader aoSqlDataReader)
        {
            StandardwiseAcademicYearDates oStandardwiseAcademicYearDates=null;
            while (aoSqlDataReader.Read())
            {
                oStandardwiseAcademicYearDates = new StandardwiseAcademicYearDates
                {
                    StandardId =Convert.ToInt32(aoSqlDataReader["StandardID"]),
                    StartDate =Convert.ToDateTime(aoSqlDataReader["StartDate"]),
                    EndDate =Convert.ToDateTime(aoSqlDataReader["EndDate"]),
                };
                olstStandardwiseAcademicYearDates.Add(oStandardwiseAcademicYearDates);
            }            
        }

       /// <summary>
       /// This method is used to Save, Update Term Configuraion details.
       /// </summary>
       /// <param name="asTermXML"></param>
       /// <param name="aiInsertedById"></param>
       /// <returns></returns>
        public int SaveSchoolwiseTermDetails(string asTermXML, int aiInsertedById, int aiOrigConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolwiseTermXML", asTermXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OrigConfigId", aiOrigConfigId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertSchoolwiseTermConfigurationDetails");
                
                return Convert.ToInt32(oSqlParameter.Value);
            };
        }
        /// <summary>
        /// This method is used to Save, Update details.
        /// </summary>
        /// <param name="asXML"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiTestId"></param>
        public void InsertEvatualtionPeriodDetails(string asXML, int aiInsertedById, int aiTestId)

        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())

            {
                oSQLServerDbUtility.AddParameter("AttendanceXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertSchoolwiseEvatualtionPeriodDeatils");
            };
        }
        /// <summary>
        /// his method is used to Copy Evaluation Periods
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="asTargetTestIds"></param>
        /// <param name="aiInsertedById"></param>
        public void CopyEvaluationPeriods(int aiSchoolId, int aiAcademicYearId, int aiTestId, string asTargetTestIds, int aiInsertedById)
          {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
          {  
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TargetTestIds", asTargetTestIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyEvaluationPeriodDetails");
            };
        }

    }
}
