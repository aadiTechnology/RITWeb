// Class Name       :- TeacherAdditionalDetailsDC.cs
// Purpose          :- This class is used to Manage Teacher Additional Detials.
// Date Of creation :- 23/08/2018
// Author Name      :- Sonali Jatahr

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using SchoolEntities.Admin;
using System.Data.SqlClient;
using System.Data;


namespace DataCommunicator
{
    public class TeacherAdditionalDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public TeacherAdditionalDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUserId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to Get Teacher additional Details for Save.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public List<TeacherAdditionalDetails> Get(int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                List<TeacherAdditionalDetails> lstTeacherAdditionalDetails = new List<TeacherAdditionalDetails>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetALLTeacherAdditionalDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        TeacherAdditionalDetails oTeacherAdditionalDetails = oTeacherAdditionalDetails = new TeacherAdditionalDetails();
                        oTeacherAdditionalDetails.TeacherId = Convert.ToInt32(oSqlDataReader["TeacherId"]);
                        oTeacherAdditionalDetails.TeacherName = Convert.ToString(oSqlDataReader["TeacherName"]);
                        if (oSqlDataReader["AdditionDetailsId"] != DBNull.Value)
                            oTeacherAdditionalDetails.AdditionalDetailsId = Convert.ToInt32(oSqlDataReader["AdditionDetailsId"]);
                        if (oSqlDataReader["FieldDetailsId"] != DBNull.Value)
                            oTeacherAdditionalDetails.QuestionId = Convert.ToInt32(oSqlDataReader["FieldDetailsId"]);
                        if (oSqlDataReader["Value"] != DBNull.Value)
                            oTeacherAdditionalDetails.AnswerId = Convert.ToInt32(oSqlDataReader["Value"]);
                        if (oSqlDataReader["Description"] != DBNull.Value)
                            oTeacherAdditionalDetails.AnswerText = Convert.ToString(oSqlDataReader["Description"]);

                        lstTeacherAdditionalDetails.Add(oTeacherAdditionalDetails);
                    }
                    return lstTeacherAdditionalDetails;
                }
            }
        }

        /// <summary>
        /// This method is used Save Teacher Additional Details
        /// </summary>
        /// <param name="oTeacherAdditionalDetails"></param>
        public void save(int aiTeacherId, string asTeacheAdditionalDetailsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherAdditionDetailsXML", asTeacheAdditionalDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTeacherAdditionalDetails");
            }
        }

        /// <summary>
        /// This Method is used to get all master data for Filling combobox.
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMasterDetailsForUDISEForm()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllSupportingDetailsForUDISEForm");
            }
        }
    }

        #endregion
}
