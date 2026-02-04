// Class Name       :- StandardWiseExamConfigurationDC
// Purpose          :- This class use to communicate with database to set standrdwise exam configuration and exam status and also read the information
// Date Of creation :- 22/10/2008
// Author Name      :- 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StandardWiseExamConfigurationEntities;
using Utility;


namespace DataCommunicator
{
    public class StandardWiseExamConfigurationDC
    {
        #region -- MEMBER(s) --

        private int miSchoolId;
        private int miAcademicYearId;

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR(s) --
       
        public StandardWiseExamConfigurationDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --
        
        /// <summary>
        /// This method communicate with database & return list that contain statndardwise exam
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public List<StandardWiseExamConfiguration> GetExamsForStandard(int aiStandardId)
        {
            List<StandardWiseExamConfiguration> lstConsiderMarksOutOfConfiguration = new List<StandardWiseExamConfiguration>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardWiseExamDetails"))
                {
                    GenericClass<StandardWiseExamConfiguration> oConsiderMarksOutOfConfiguration = new GenericClass<StandardWiseExamConfiguration>();
                    lstConsiderMarksOutOfConfiguration = oConsiderMarksOutOfConfiguration.GetFilledObjectList(oSqlDataReader);
                }
            }
            return lstConsiderMarksOutOfConfiguration;
        }

        /// <summary>
        /// This method use to save standardwise exam
        /// </summary>
        /// <param name="asStandardWiseExamDetailsXml"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiInsertedById"></param>
        public void Save(string asStandardWiseExamDetailsXml, int aiStandardId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardWiseExamDetailsXml", asStandardWiseExamDetailsXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ManageStandardWiseExams");
            }
        }

        /// <summary>
        /// This method return list that contain schoolwise exam status applicable for that school
        /// </summary>
        /// <returns>List<ExamStatusConfiguration></returns>
        public List<ExamStatusConfiguration> GetSchoolwiseExamStatusConfiguration()
        {
            List<ExamStatusConfiguration> lstExamStatusConfiguration = new List<ExamStatusConfiguration>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolWiseExamStatusConfiguration"))
                {
                    while (oSqlDataReader.Read())
                        lstExamStatusConfiguration.Add(ReadObjectFromReader(oSqlDataReader));
                }
                return lstExamStatusConfiguration;
            }
        }
       
        /// <summary>
        /// This method communicate with database and read status information in List
        /// </summary>
        /// <param name="aiStatusId"></param>
        /// <returns>ExamStatusConfiguration</returns>
        public ExamStatusConfiguration GetExamStatusForSelectedStatusName(int aiStatusId)
        {
           ExamStatusConfiguration oExamStatusConfiguration = new ExamStatusConfiguration();
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ExamStatusId", aiStatusId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
               using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolWiseExamStatusConfiguration"))
               {
                   if (oSqlDataReader.Read())
                       oExamStatusConfiguration=ReadObjectFromReader(oSqlDataReader);	
               } 
            }
           return oExamStatusConfiguration;
        }
        
        /// <summary>
        /// This method use to update exam status information 
        /// </summary>
        /// <param name="oExamStatusConfiguration"></param>
        public void UpdateExamStatusConfiguration(ExamStatusConfiguration oExamStatusConfiguration)
        { 
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               oSQLServerDbUtility.AddParameter("ExamStatusId",oExamStatusConfiguration.ExamStatusId,SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("ForeColor", oExamStatusConfiguration.ForeColor, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("ConsiderInTotal", oExamStatusConfiguration.ConsiderInTotal, SqlDbType.NChar);
               oSQLServerDbUtility.AddParameter("DisplayTotal", oExamStatusConfiguration.DisplayTotal, SqlDbType.NChar);
               oSQLServerDbUtility.AddParameter("ConsiderAsPresent", oExamStatusConfiguration.ConsiderAsPresent, SqlDbType.NChar);
               oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateExamStatusConfiguration");
            }   
        }

        #endregion -- PUBLIC METHOD(s) --

        #region -- PRIVATE METHOD(s) --
        
        /// <summary>
        /// This method is used to populate object of exam status configuration.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private ExamStatusConfiguration ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            ExamStatusConfiguration oExamStatusConfiguration = new ExamStatusConfiguration
            {
                DisplayName = Convert.ToString(aoSqlDataReader["DisplayName"]),
                ExamStatusId = Convert.ToInt32(aoSqlDataReader["ExamStatusId"]),
                ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                DisplayValue = Convert.ToString(aoSqlDataReader["DisplayValue"]),
                ForeColor = Convert.ToString(aoSqlDataReader["ForeColor"]),
                BackColor = Convert.ToString(aoSqlDataReader["BackColor"]),
                ConsiderInTotal = Convert.ToChar(aoSqlDataReader["ConsiderInTotal"]),
                DisplayTotal = Convert.ToChar(aoSqlDataReader["DisplayTotal"]),
                ConsiderAsPresent = Convert.ToChar(aoSqlDataReader["ConsiderAsPresent"]),
            };
            return oExamStatusConfiguration;
        }

        #endregion -- PRIVATE METHOD(s) --

    }
 }

