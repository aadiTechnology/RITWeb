using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    public class SurveyStudentDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private List<SurveySchool> mlstSchools;
        private List<SurveyStudentCategory> mlstCategories; 

        #endregion

        #region Property(s)

        public List<SurveySchool> Surveyschools
        {
            get { return mlstSchools; }
        }

        public List<SurveyStudentCategory> SurveyStudentCategories
        {
            get { return mlstCategories; }
        } 

        #endregion

        #region Constructor(s)

        public SurveyStudentDC()
        {
        }

        public SurveyStudentDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Method(s)

        public static List<SurveyStudentDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asSortDirection, int aiStartIndex, int aiEndIndex)
        {
            List<SurveyStudentDetails> lstStudents = new List<SurveyStudentDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSurveyStudents");
                while (oSqlDataReader.Read())
                {
                    lstStudents.Add
                        (
                            new SurveyStudentDetails
                            {
                                Category = Convert.ToString(oSqlDataReader["Category"]),
                                Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                MobileNo1 = Convert.ToString(oSqlDataReader["MobileNo1"]),
                                MobileNo2 = Convert.ToString(oSqlDataReader["MobileNo2"]),
                                Name = Convert.ToString(oSqlDataReader["Name"]),
                                RegNo = Convert.ToString(oSqlDataReader["RegNo"]),
                                School = Convert.ToString(oSqlDataReader["School"]),
                                Standard = Convert.ToString(oSqlDataReader["Standard"]),
                                GenderId = Convert.ToInt32(oSqlDataReader["GenderId"]),
                                Gender = Convert.ToString(oSqlDataReader["Gender"]),
                                IsInterested = Convert.ToInt32(oSqlDataReader["IsInterested"])
                            }
                        );
                }
            }
            return lstStudents;
        }

        public static int Count(int aiSchoolId, int aiAcademicYearId)
        {
            List<SurveyStudentDetails> lstStudents = new List<SurveyStudentDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetAllSurveyStudentsCount");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public List<Standard> GetAllEntities()
        {
            List<Standard> lstStandards = new List<Standard>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSurveyEntities");
                lstStandards = GetAllStandards(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillSchools(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillCategories(oSqlDataReader);

                return lstStandards;
            }
        }

        private void FillCategories(SqlDataReader aoSqlDataReader)
        {
            mlstCategories = new List<SurveyStudentCategory>();
            while (aoSqlDataReader.Read())
            {
                mlstCategories.Add
                    (
                        new SurveyStudentCategory
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        }
                    );
            }
        }

        private void FillSchools(SqlDataReader aoSqlDataReader)
        {
            mlstSchools = new List<SurveySchool>();
            while (aoSqlDataReader.Read())
            {
                mlstSchools.Add
                    (
                        new SurveySchool
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        }
                    );
            }
        }

        private List<Standard> GetAllStandards(SqlDataReader aoSqlDataReader)
        {
            List<Standard> lstStandards = new List<Standard>();
            while (aoSqlDataReader.Read())
            {
                lstStandards.Add
                    (
                        new Standard
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"])
                        }
                    );
            }
            return lstStandards;
        }

        public SurveyStudentDetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSurveyStudent");
                SurveyStudentDetails oSurveyStudentDetails = new SurveyStudentDetails();
                if (oSqlDataReader.Read())
                {
                    oSurveyStudentDetails.Category = Convert.ToString(oSqlDataReader["Category"]);
                    oSurveyStudentDetails.Id = Convert.ToInt32(oSqlDataReader["Id"]);
                    oSurveyStudentDetails.MobileNo1 = Convert.ToString(oSqlDataReader["MobileNo1"]);
                    oSurveyStudentDetails.MobileNo2 = Convert.ToString(oSqlDataReader["MobileNo2"]);
                    oSurveyStudentDetails.Name = Convert.ToString(oSqlDataReader["Name"]);
                    oSurveyStudentDetails.RegNo = Convert.ToString(oSqlDataReader["RegNo"]);
                    oSurveyStudentDetails.Standard = Convert.ToString(oSqlDataReader["Standard"]);
                    oSurveyStudentDetails.CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]);
                    oSurveyStudentDetails.SurveySchoolId = Convert.ToInt32(oSqlDataReader["SurveySchoolId"]);
                    oSurveyStudentDetails.StandardId = Convert.ToInt32(oSqlDataReader["StandardId"]);
                    oSurveyStudentDetails.GenderId = Convert.ToInt32(oSqlDataReader["GenderId"]);
                    oSurveyStudentDetails.Gender = Convert.ToString(oSqlDataReader["Gender"]);
                    oSurveyStudentDetails.IsInterested = Convert.ToInt32(oSqlDataReader["IsInterested"]);
                    oSurveyStudentDetails.Address = Convert.ToString(oSqlDataReader["Address"]);
                }
                return oSurveyStudentDetails;
            }
        }

        public string Save(SurveyStudentDetails aoSurveyStudentDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoSurveyStudentDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aoSurveyStudentDetails.CategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveySchoolId", aoSurveyStudentDetails.SurveySchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aoSurveyStudentDetails.StandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MobileNo1", aoSurveyStudentDetails.MobileNo1, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNo2", aoSurveyStudentDetails.MobileNo2, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Name", aoSurveyStudentDetails.Name, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("GenderId", aoSurveyStudentDetails.GenderId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInterested", aoSurveyStudentDetails.IsInterested, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Address", aoSurveyStudentDetails.Address, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("RegNo", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 30);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSurveyStudentDetails");
                return oSqlParameter.Value.ToString();
            }
        }

        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSurveyStudent");
            }
        }

        public DataTable GetStandardList(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardsList");
            }
        }

        public List<SurveyStudentDetails> GetAllStudents(int aiCategoryId, string asStandardList)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYEarId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardIds", asStandardList, SqlDbType.NVarChar);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSurveyStudentListForSMS");
                List<SurveyStudentDetails> lstStudents = new List<SurveyStudentDetails>();
                while (oSqlDataReader.Read())
                {
                    lstStudents.Add
                        (
                            new SurveyStudentDetails
                            {
                                Name = oSqlDataReader["Name"].ToString(),
                                MobileNo1 = oSqlDataReader["MobileNo1"].ToString(),
                                MobileNo2 = oSqlDataReader["MobileNo2"].ToString()
                            }
                        );
                }
                return lstStudents;
            }
        }

        #endregion
    }
}