/*File Name - StudentRecordParameterDC.cs
 * Created Date - 2 June 2018
 * Created By - Sonali
 * Description - This class is used to communicate with database for managing StudentRecord parameter details.
 */
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
    public class StudentRecordParameterDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;

        #endregion
       
        #region Constructor(s)

        public StudentRecordParameterDC()
        {
        }

        public StudentRecordParameterDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return all Student Record Sections Details .
        /// </summary>
        /// <param name="aisectionId"></param>
        /// <returns></returns>
        public List<StudentRecordParameter> GetAll(int aiSectionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SectionId", aiSectionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentRecordSections"))
                    return this.FillStudentRecordParameter(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to save Student Record Parameters details.
        /// </summary>
        /// <param name="aoPerformanceParameter"></param>
        public void Save(StudentRecordParameter aoStudentRecordParameter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SectionId", aoStudentRecordParameter.SectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", aoStudentRecordParameter.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParameterId", aoStudentRecordParameter.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortOrder", aoStudentRecordParameter.SortOrder, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentRecordParameters");

            }
        }

        /// <summary>
        /// This method is used to Delete Student Record Parameters details.
        /// </summary>
        /// <param name="aiPerformanceParameterId"></param>
        /// <param name="aiConfigId"></param>
        public  void Delete(int aiParameterId,int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParameterId", aiParameterId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStudentRecordSection");
                
            }
        }
           
        /// <summary>
          /// This method is used to return all combobox fill Student Record Sectios Details
          /// </summary>
          /// <returns></returns>
        public List<StudentRecordSection> GetAllSections()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StudentRecordSectionsCombo"))
                    return this.FillStudentRecordSections(oSqlDataReader);
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill Student Record Parameter entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentRecordParameter> FillStudentRecordParameter(SqlDataReader aoSqlDataReader)
        {
            List<StudentRecordParameter> lstPerformanceParameters = new List<StudentRecordParameter>();
            while (aoSqlDataReader.Read())
            {
                StudentRecordParameter oStudentRecordParameter = new StudentRecordParameter();                
                oStudentRecordParameter.SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"]);
                oStudentRecordParameter.SectionName = Convert.ToString(aoSqlDataReader["SectionName"]);
                oStudentRecordParameter.Name = Convert.ToString(aoSqlDataReader["Parameter"]);
                oStudentRecordParameter.SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]);
                oStudentRecordParameter.Id = Convert.ToInt32(aoSqlDataReader["ParameterId"]);
                lstPerformanceParameters.Add(oStudentRecordParameter);
            }
            return lstPerformanceParameters;
        }
        /// <summary>
        /// This method is used to fill StudentRecordSection entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentRecordSection> FillStudentRecordSections(SqlDataReader aoSqlDataReader)
        {
            List<StudentRecordSection> lstStudentRecordSection = new List<StudentRecordSection>();
            while (aoSqlDataReader.Read())
            {
                StudentRecordSection oStudentRecordSection = new StudentRecordSection();
                oStudentRecordSection.Id = Convert.ToInt32(aoSqlDataReader["Id"]);
                oStudentRecordSection.Name = Convert.ToString(aoSqlDataReader["Name"]);
                lstStudentRecordSection.Add(oStudentRecordSection);
            }
            return lstStudentRecordSection;
        } 

        #endregion
    }
}




