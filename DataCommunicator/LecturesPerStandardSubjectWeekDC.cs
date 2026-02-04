// File Name        : LecturesPerStandardSubjectWeekDC
// Purpose          : This class is used to manage LecturesPerStandardSubjectWeek details.
// Date Of creation : 2/29/2008
// Author Name      : Anugandha


using System;
using System.Data;
using System.Data.SqlClient;
using Utility;


namespace DataCommunicator
{


    public class LecturesPerStandardSubjectWeekDC : DataCommunicatorBaseDC
    {
        #region Data Members

        private LecturesPerStandardSubjectWeekStruct moLecturesPerStandardSubjectWeekStruct;

        #endregion

        #region Constructor

        public LecturesPerStandardSubjectWeekDC()
        {
        }

        public LecturesPerStandardSubjectWeekDC(int miLecturesPerStandardSubjectWeekId)
        {
            LoadLecturesPerStandardSubjectWeekDetails(miLecturesPerStandardSubjectWeekId);
        }

        #endregion

        #region Structure

        public struct LecturesPerStandardSubjectWeekStruct
        {

            public int miLecturesPerStandardSubjectWeekId;

            public int miSchoolId;

            public int miAcademicYearId;

            public int miStandardSubjectId;

            public int miMaxLecturesPerStandardSubject;

            public string msIsDeleted;

            public int miInsertedById;

            public DateTime mdtInsertDate;

            public int miUpdatedById;

            public DateTime mdtUpdateDate;
        }

        #endregion

        #region Properties

        public virtual LecturesPerStandardSubjectWeekStruct LecturesPerStandardSubjectWeekStructDetails
        {
            get
            {
                return moLecturesPerStandardSubjectWeekStruct;
            }
            set
            {
                moLecturesPerStandardSubjectWeekStruct = value;
            }
        }

        #endregion

        #region Public Methods

        // This function is used to insert the LecturesPerStandardSubjectWeek Details
        public string InsertLecturesPerStandardSubjectWeek()
        {
            string sInsertStatement = "INSERT INTO Lectures_Per_Standard_Subject_Week(" +
                                        "School_Id" +
                                        ",Academic_Year_Id" +
                                        ",Division_Subject_Id" +
                                        ",Max_Lectures_Per_Standard_Subject" +
                                        ",Inserted_By_Id" +
                                        ",Is_Deleted" +
                                        ")VALUES(" +
                                        " " + moLecturesPerStandardSubjectWeekStruct.miSchoolId +
                                         " , " + moLecturesPerStandardSubjectWeekStruct.miAcademicYearId +
                                         " , " + moLecturesPerStandardSubjectWeekStruct.miStandardSubjectId +
                                         " , " + moLecturesPerStandardSubjectWeekStruct.miMaxLecturesPerStandardSubject +
                                         " , " + moLecturesPerStandardSubjectWeekStruct.miInsertedById +
                                         " , '" + moLecturesPerStandardSubjectWeekStruct.msIsDeleted +
                                        "')";
            return sInsertStatement;
        }

        // This function is used to update the LecturesPerStandardSubjectWeek Details
        public string UpdateLecturesPerStandardSubjectWeek()
        {
            string sUpdateStatement = "UPDATE Lectures_Per_Standard_Subject_Week SET " +
                                        "School_Id= " + moLecturesPerStandardSubjectWeekStruct.miSchoolId +
                                        ",Academic_Year_Id= " + moLecturesPerStandardSubjectWeekStruct.miAcademicYearId +
                                        ",Division_Subject_Id= " + moLecturesPerStandardSubjectWeekStruct.miStandardSubjectId +
                                        ",Max_Lectures_Per_Standard_Subject= " + moLecturesPerStandardSubjectWeekStruct.miMaxLecturesPerStandardSubject +
                                        ",Is_Deleted= '" + moLecturesPerStandardSubjectWeekStruct.msIsDeleted +
                                        "'" +
                                        " WHERE Lectures_Per_Standard_Subject_Week_Id=" + moLecturesPerStandardSubjectWeekStruct.miLecturesPerStandardSubjectWeekId;
            return sUpdateStatement;

        }

        public DataTable GetStandardSubjectId(int aiStandardId, int aiSubjectId, int aiSchoolId)
        {
            string sQuery = " SELECT  " +
                                " SchoolWise_Standard_Subject_Id " +
                            " FROM " +
                                " SchoolWise_Standard_Subject_Master " +
                            " WHERE " +
                                " Standard_Id = " + aiStandardId +
                                " AND Subject_Id = " + aiSubjectId +
                                " AND School_Id = " + aiSchoolId +
                                " AND Is_Deleted = N'" + Constants.C_NO + " ' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        public DataTable GetLectureCount(int aiStandardSubjectId, int aiSchoolId)
        {
            string sQuery = " SELECT " +
                            "  Lectures_Per_Standard_Subject_Week_Id " +
                            " , " +
                              " Max_Lectures_Per_Standard_Subject " +
                              " , " +
                              " Division_Subject_Id " +
                              " FROM " +
                              " Lectures_Per_Standard_Subject_Week " +
                              " WHERE " +
                              " Division_Subject_Id = " + aiStandardSubjectId +
                              " AND " +
                              " School_Id = " + aiSchoolId +
                              " AND " +
                              " Is_Deleted =N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }
		/// <summary>
		/// This method is used to check if lecture count is changed to lower.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicyearId"></param>
		/// <param name="asXml"></param>
		/// <returns></returns>
		public static DataTable CheckValidUpdatedCount(int aiSchoolId, int aiAcademicyearId, string asXml)
		{
			string sResult = string.Empty;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicyearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("LectureCountDetails", asXml, SqlDbType.Xml);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CheckForValidUpdatedLectureCount");
			}
		}

        #endregion

        #region Private Methods

        // This function is used to load the LecturesPerStandardSubjectWeek Details
        private void LoadLecturesPerStandardSubjectWeekDetails(int miLecturesPerStandardSubjectWeekId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchLecturesPerStandardSubjectWeekDetailsFromDatabase(miLecturesPerStandardSubjectWeekId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Lectures_Per_Standard_Subject_Week_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miLecturesPerStandardSubjectWeekId = Convert.ToInt32(oDR["Lectures_Per_Standard_Subject_Week_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["Academic_Year_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miAcademicYearId = Convert.ToInt32(oDR["Academic_Year_Id"]);
                            if (oDR["Division_Subject_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miStandardSubjectId = Convert.ToInt32(oDR["Division_Subject_Id"]);
                            if (oDR["Max_Lectures_Per_Standard_Subject"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miMaxLecturesPerStandardSubject = Convert.ToInt32(oDR["Max_Lectures_Per_Standard_Subject"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Inserted_By_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moLecturesPerStandardSubjectWeekStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the LecturesPerStandardSubjectWeek Details
        private string FetchLecturesPerStandardSubjectWeekDetailsFromDatabase(int miLecturesPerStandardSubjectWeekId)
        {
            string sSelectStatement = " SELECT  " +
            "Lectures_Per_Standard_Subject_Week_Id" +
            ",School_Id" +
            ",Academic_Year_Id" +
            ",Division_Subject_Id" +
            ",Max_Lectures_Per_Standard_Subject" +
            ",Is_Deleted" +
            ",Inserted_By_Id" +
            ",Insert_Date" +
            ",Updated_By_Id" +
            ",Update_Date" +
            " FROM Lectures_Per_Standard_Subject_Week" +
            " WHERE Lectures_Per_Standard_Subject_Week_Id=" + miLecturesPerStandardSubjectWeekId;
            return sSelectStatement;
        }

        #endregion


    }

    public class LecturesPerStandardSubjectWeekCollectionDC
    {
        #region Public Methods

        /// <summary>
        /// This method is used to get all subjects assignend for particular subject.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public DataTable GetAllSubjectsforStandard(int aiSchoolId, int aiStandardId)
        {
            string sSelectStatement = " SELECT " +
                                        " SchoolWise_Standard_Subject_Master.Subject_Id " +

                                    " FROM " +
                                        " Subject_Master " +
                                    " INNER JOIN " +
                                        " SchoolWise_Standard_Subject_Master " +
                                    " ON " +
                                          " SchoolWise_Standard_Subject_Master.Subject_Id = Subject_Master.Subject_Id " +
                                    " INNER JOIN " +
                                        " Standard_Master " +
                                    " ON  " +
                                     " Standard_Master.Standard_Id = SchoolWise_Standard_Subject_Master.Standard_Id " +
                                    " WHERE " +
                                        " SchoolWise_Standard_Subject_Master.School_Id= " + aiSchoolId +
                                        " AND SchoolWise_Standard_Subject_Master.Standard_Id= " + aiStandardId +
                                        " AND SchoolWise_Standard_Subject_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Subject_Master.Is_Deleted= N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public DataSet GetStdSubjectLectures(int aiSchoolId, int aiAcadId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcadId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStdSubjectLectures");
            }
        }
        #endregion
    }
}
