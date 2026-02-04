using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;

namespace DataCommunicator
{
    public class ConfigurePeerDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miInsertedById;

        #endregion

        #region Constructor(s)

        public ConfigurePeerDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }

        public ConfigurePeerDC()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to fill peer dropdown in listview
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeerDetails()
        {
            string sStatement = " SELECT YSD.Roll_No," +
                                        " VBSD.StudentName" +
                                        " FROM vw_BaseStudentDetails VBSD" +
                                        " INNER JOIN YearWise_Student_Details YSD " +
                                        " ON VBSD.SchoolWise_Student_Id = YSD.Student_Id" +
                                        " WHERE vbsd.Is_Deleted = 'N'" +
                                        " AND YSD.Is_Deleted = 'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sStatement);
        }


        /// <summary>
        /// This method is used to save details.
        /// </summary>
        public void Save(string asXML)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentPeerDetails", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePeerStudentDetails");
            }
        }

        /// <summary>
        /// This method is used to get student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<ConfigurePeerDetails> GetAll(int aiStandardId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllConfigurePeerDetails"))
                {
                    List<ConfigurePeerDetails> lstConfigurePeerDetails = new List<ConfigurePeerDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstConfigurePeerDetails.Add(new ConfigurePeerDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            PeerName = Convert.ToString(oSqlDataReader["Peerstudentname"]),
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            YearwiseStudentId = Convert.ToInt32(oSqlDataReader["Yearwise_Student_Id"]),
                            PeerYrStudentId = Convert.ToInt32(oSqlDataReader["PeerStudentId"])                            
                        });
                    }
                    return lstConfigurePeerDetails;
                }
            }
        }
 
        #endregion
    }
}
