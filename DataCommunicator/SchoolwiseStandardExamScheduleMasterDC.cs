// File Name       :SchoolwiseStandardExamScheduleMasterDC
// Purpose         :This class is used to manage SchoolwiseStandardExamScheduleMaster details.
// Date Of creation:2/2/2008
// Author Name     :Anugandha 

using System;
using System.Data;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using BookEntities;
namespace DataCommunicator
{

    public class SchoolwiseStandardExamScheduleMasterDC : DataCommunicatorBaseDC
    {
        #region Data Members

        private SchoolwiseStandardExamScheduleMasterStruct moSchoolwiseStandardExamScheduleMasterStruct;
        
        #endregion

        #region Structure

        public struct SchoolwiseStandardExamScheduleMasterStruct
        {

            public int miSchoolwiseStandardExamScheduleId;

            public int miStandardId;

            public int miSchoolWiseTestId;

            public int miStandardTestId;

            public DateTime mdtExamStartDate;

            public DateTime mdtExamEndDate;

            public int miTotalExamDays;

            public string msInstructions;

            public int miSchoolId;

            public int miacademicYearId;

            public string msIsDeleted;

            public DateTime mdtInsertDate;

            public string msInsertedByid;

            public DateTime mdtUpdateDate;

            public string msUpdatedById;

            public string msExamDetails;

            public DateTime mdSubjectExamStartDate;
        }

        #endregion

        #region Properties

        public SchoolwiseStandardExamScheduleMasterStruct SchoolwiseStandardExamScheduleMasterStructDetails
        {
            get
            {
                return moSchoolwiseStandardExamScheduleMasterStruct;
            }
            set
            {
                moSchoolwiseStandardExamScheduleMasterStruct = value;
            }
        }

        #endregion

        #region Constructors

        public SchoolwiseStandardExamScheduleMasterDC()
        {
        }

        public SchoolwiseStandardExamScheduleMasterDC(int miSchoolwiseStandardExamScheduleId)
        {
            LoadSchoolwiseStandardExamScheduleMasterDetails(miSchoolwiseStandardExamScheduleId);
        }
        public SchoolwiseStandardExamScheduleMasterDC(int aiStandardId, int aiTestId)
        {
            LoadSchoolwiseStandardExamScheduleMasterDetails(aiStandardId, aiTestId);
        }

        public SchoolwiseStandardExamScheduleMasterDC(int aiStandardId, int aiTestId, int aiSubjectId)
        {
            LoadSchoolwiseStandardExamScheduleMasterDetails(aiStandardId, aiTestId, aiSubjectId);
        }

        #endregion

        #region Public Methods

        public static DataSet GetStdExamSchedule(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardExamSchedule");
            }
        }
        // This function is used to insert the SchoolwiseStandardExamScheduleMaster Details
        public int InsertSchoolwiseStandardExamScheduleMaster()
        {
            string sInsertStatement = " INSERT INTO " +
                                      " Schoolwise_Standard_Exam_Schedule_Master(" +
                                      " Standard_Id" +
                                      " ,SchoolWise_Test_Id" +
                                      " , Standard_Test_Id " +
                                      " ,Exam_Start_Date" +
                                      " ,Exam_End_Date" +
                                      " ,Total_Exam_Days" +
                                      " ,Instructions" +
                                      " ,School_Id" +
                                      " ,academic_Year_Id" +
                                      " ,Inserted_By_id" +
                                      ") VALUES(" +
                                          " " + moSchoolwiseStandardExamScheduleMasterStruct.miStandardId +
                                          " , " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolWiseTestId +
                                          " , " + moSchoolwiseStandardExamScheduleMasterStruct.miStandardTestId +
                                          " , N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate + "' " +
                                          " , N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate + "' " +
                                          " , " + moSchoolwiseStandardExamScheduleMasterStruct.miTotalExamDays +
                                          " , N'" + moSchoolwiseStandardExamScheduleMasterStruct.msInstructions + "' " +
                                          " , " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId +
                                          " , " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                          " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardExamScheduleMasterStruct.msInsertedByid, false) + "' " +
                                      ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        // This function is used to update the SchoolwiseStandardExamScheduleMaster Details
        public void UpdateSchoolwiseStandardExamScheduleMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardwiseExamScheduleId", moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Updated_By_id", moSchoolwiseStandardExamScheduleMasterStruct.msUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStandardWiseExamSchedule");
            }
        }

        // This function is used to update the SchoolwiseStandardExamScheduleMaster Details
        public void UpdateExamScheduleInstruction()
        {
            string sUpdateStatement = " UPDATE " +
                                      " Schoolwise_Standard_Exam_Schedule_Master " +
                                      " SET " +
                                      "  Instructions= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardExamScheduleMasterStruct.msInstructions, true) + "' " +
                                      " ,Updated_By_id= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStandardExamScheduleMasterStruct.msUpdatedById, false) + "' " +
                                      " " +
                                      " WHERE " +
                                      " Schoolwise_Standard_Exam_Schedule_Id=" + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId;


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        //this finction use to get standrds to which given exam applicablr
        public static List<ClassDetails> GetStandards(int aiSchoolId, int aiAcademicYearId, int aiExamId)
        {
            List<ClassDetails> lstClassDetails = new List<ClassDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseExamId", aiExamId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardsForExamCopy"))
                {
                    ClassDetails oClassDetails;
                    while (oSqlDataReader.Read())
                    {
                        oClassDetails = new ClassDetails
                        {
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["Standard_Id"]),
                            Classname = Convert.ToString(oSqlDataReader["Standard_Name"]),
                        };
                        lstClassDetails.Add(oClassDetails);
                    }
                }
            }
            return lstClassDetails;
        }


        // This function is used to delete the SchoolwiseStandardExamScheduleMaster Details
        public void DeleteSchoolwiseStandardExamScheduleMaster()
        {
            string sDeleteStatement = " DELETE " +
                                      " Schoolwise_Standard_Exam_Schedule_Master " +
                                      " WHERE " +
                                      " Schoolwise_Standard_Exam_Schedule_Id='" + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method check whether start and end date is persent in 
        /// Schoolwise_Standard_Exam_Schedule_Master for particular standard or not.
        /// </summary>
        /// <param name="Standard_Id"></param>
        /// <returns>Int32</returns>
        public Int32 IsExamStartAndEndDatePredefined(int aiStandardId)
        {
            string sFilter = " WHERE " +
                                   "( ( CONVERT(DATETIME,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate.ToShortDateString() + "',101) " +
                                   "BETWEEN CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_Start_Date,101),101) AND CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_End_Date,101),101) ) " +
                                        " OR " +
                                        "( CONVERT(DATETIME,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate.ToShortDateString() + "',101) " +
                                   "BETWEEN CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_Start_Date,101),101) AND CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_End_Date,101),101)) " +
                                        " OR " +
                                        "( CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_Start_Date,101),101) BETWEEN CONVERT(DATETIME,CONVERT(NVARCHAR(10),N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate.ToShortDateString() + "',101),101) " +
                                   " AND  CONVERT(DATETIME,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate.ToShortDateString() + "',101))" +
                                        " OR " +
                                        "( CONVERT(DATETIME,CONVERT(NVARCHAR(10),Exam_End_Date,101),101) BETWEEN CONVERT(DATETIME,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate.ToShortDateString() + "',101) " +
                                   " AND  CONVERT(DATETIME,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate.ToShortDateString() + "',101)) )" +
                                   " AND ( School_Id = " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId + " ) " +
                                   " AND ( Academic_Year_Id = " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId + ")" +
                                   " AND Is_Deleted= N'" + Constants.C_NO + "'" +
                                   " AND Standard_Id= " + aiStandardId;

            if (moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId != Constants.I_ZERO)
            {
                sFilter += " AND Schoolwise_Standard_Exam_Schedule_Id <> " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId + "";
            }

            string sSelectStatment = "SELECT " +
                                         " COUNT(Schoolwise_Standard_Exam_Schedule_Id) " +
                                         " FROM Schoolwise_Standard_Exam_Schedule_Master " +
                                         sFilter;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment);
        }

        /// <summary>
        /// This method check whether start and end date inbetween nonworking day.
        /// </summary>
        /// <returns>Dataset</returns>
        public DataSet IsWorkingWeekday()
        {
            string sSelectStatement = " SELECT COUNT(WeekDay_Name)" +
                                     " FROM WeekDays_Master " +
                                     " WHERE WeekDay_Name = " +
                                         " DATENAME(weekday,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate + "') " +
                                         " AND  School_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId + " " +
                                         " ANd Academic_Year_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                         " AND Is_Deleted= N'" + Constants.C_NO + "'" +
                                     " SELECT COUNT(WeekDay_Name)" +
                                      " FROM WeekDays_Master " +
                                      " WHERE WeekDay_Name = " +
                                             " DATENAME(weekday,N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate + "') " +
                                             " AND  School_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId + " " +
                                             " AND Academic_Year_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                             " AND Is_Deleted= N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
        }

        /// <summary>
        /// This method check whether start and end date is holiday or not.
        /// </summary>
        /// <returns>Dataset</returns>
        public DataSet IsHoliday()
        {
            string sSelectStatement = "SELECT COUNT(Holiday_Name) " +
                                          " FROM " +
                                          " Holidays_Master " +
                                          " WHERE " +
                                          " N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate + "' " +
                                          "BETWEEN Holiday_Start_Date AND Holiday_Start_Date  " +
                                              " AND  School_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId + " " +
                                              " ANd Academic_Year_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                              " AND Is_Deleted= N'" + Constants.C_NO + "'" +
                                      " SELECT COUNT(Holiday_Name)" +
                                      " FROM Holidays_Master " +
                                      " WHERE " +
                                      " N'" + moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate + "' " +
                                          "BETWEEN Holiday_Start_Date AND Holiday_Start_Date  " +
                                             " AND  School_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId + " " +
                                             " AND Academic_Year_Id= " + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                             " AND Is_Deleted= N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
        }

        public DataTable FillStandardwiseExamScheduleGrid(int aiStandardId)
        {
            string sSelectStatement = " SELECT " +
                                     " SchoolWise_Test_Master.SchoolWise_Test_Name " +
                                     "," +
                                     " SchoolWise_Test_Master.SchoolWise_Test_Id " +
                                     "," +
                                     " Schoolwise_Standard_Exam_Schedule_Master.Exam_Start_Date " +
                                     "," +
                                     " Schoolwise_Standard_Exam_Schedule_Master.Exam_End_Date " +
                                     "," +
                                     " Schoolwise_Standard_Exam_Schedule_Master.Total_Exam_Days " +
                                     "," +
                                     " Schoolwise_Standard_Exam_Schedule_Id " +
                                     " FROM " +
                                     " SchoolWise_Test_Master " +
                                     " INNER JOIN " +
                                     " Schoolwise_Standard_Exam_Schedule_Master " +
                                     " ON " +
                                     " SchoolWise_Test_Master.SchoolWise_Test_Id = Schoolwise_Standard_Exam_Schedule_Master.SchoolWise_Test_Id " +
                                     " WHERE " +
                                     " Schoolwise_Standard_Exam_Schedule_Master.School_Id = " + moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId +
                                     " AND " +
                                     " Schoolwise_Standard_Exam_Schedule_Master.academic_Year_Id =" + moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId +
                                     " AND " +
                                     " Schoolwise_Standard_Exam_Schedule_Master.Standard_Id =" + aiStandardId +
                                     " AND " +
                                     " Schoolwise_Standard_Exam_Schedule_Master.Is_Deleted= N'" + Constants.C_NO + "'" +
                                     " ORDER BY Schoolwise_Standard_Exam_Schedule_Master.Exam_Start_Date , Schoolwise_Standard_Exam_Schedule_Master.Exam_End_Date ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataSet InsertExamScheduleDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_Id", moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", moSchoolwiseStandardExamScheduleMasterStruct.miStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolWise_Test_Id", moSchoolwiseStandardExamScheduleMasterStruct.miSchoolWiseTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Test_Id", moSchoolwiseStandardExamScheduleMasterStruct.miStandardTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("inserted_By_Id", moSchoolwiseStandardExamScheduleMasterStruct.msInsertedByid, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ScreenId",Constants.SchoolConfigurations.StandardwiseExamScheduleConfig, SqlDbType.Int);				
                oSQLServerDbUtility.AddParameter("Schoolwise_Standard_Exam_Schedule_Id", moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Exam_Details", moSchoolwiseStandardExamScheduleMasterStruct.msExamDetails, SqlDbType.Xml);
               return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("Usp_InsertStandardExamSchedule",true);
            }
        }

        /// <summary>
        /// This method is used to submit or Unsubmit the exam Schedule.
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="abIsUnsubmit"></param>
        /// <param name="aiSchoolwiseTestId"></param>
        /// </summary>
        public void SubmitExamSchedule(int aiSchoolId, int aiAcademicYearId, int aiUserId,int aiStandardId, bool abIsUnsubmit, int aiSchoolwiseTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsUnSubmit", abIsUnsubmit, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SchoolwiseTestId", aiSchoolwiseTestId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SumbitExamSchedule");
            }
        }


        #endregion

        #region Private Methods

        // This function is used to load the SchoolwiseStandardExamScheduleMaster Details
        private void LoadSchoolwiseStandardExamScheduleMasterDetails(int miSchoolwiseStandardExamScheduleId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardExamScheduleMasterDetailsFromDatabase(miSchoolwiseStandardExamScheduleId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                SetObjectValues(oDR);
               
            }
        }
        private void LoadSchoolwiseStandardExamScheduleMasterDetails(int aiStandardId, int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStandardExamScheduleMasterDetailsFromDatabase(aiStandardId, aiTestId);
               using( SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                SetObjectValues(oDR);
                
            }
        }

        private void LoadSchoolwiseStandardExamScheduleMasterDetails(int aiStandardId, int aiTestId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExamSchedule"))
                {
                    if (oSqlDataReader.Read())
                    {
                        if (oSqlDataReader["Schoolwise_Standard_Exam_Schedule_Id"] != DBNull.Value)
                            moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId = Convert.ToInt32(oSqlDataReader["Schoolwise_Standard_Exam_Schedule_Id"]);

                        if (oSqlDataReader["Exam_Start_Date"] != DBNull.Value)
                            moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate = Convert.ToDateTime(oSqlDataReader["Exam_Start_Date"]);

                        if (oSqlDataReader["Exam_End_Date"] != DBNull.Value)
                            moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate = Convert.ToDateTime(oSqlDataReader["Exam_End_Date"]);

                        if(oSqlDataReader["SubjectExamStartDate"] != DBNull.Value)
                            moSchoolwiseStandardExamScheduleMasterStruct.mdSubjectExamStartDate = Convert.ToDateTime(oSqlDataReader["SubjectExamStartDate"]);
                    }
                }

            }
        }

        private void SetObjectValues(SqlDataReader aoDR)
        {
            if (aoDR != null)
            {
                while (aoDR.Read())
                {
                    if (aoDR["Schoolwise_Standard_Exam_Schedule_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miSchoolwiseStandardExamScheduleId = Convert.ToInt32(aoDR["Schoolwise_Standard_Exam_Schedule_Id"]);
                    if (aoDR["Standard_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miStandardId = Convert.ToInt32(aoDR["Standard_Id"]);
                    if (aoDR["SchoolWise_Test_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miSchoolWiseTestId = Convert.ToInt32(aoDR["SchoolWise_Test_Id"]);
                    if (aoDR["Exam_Start_Date"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.mdtExamStartDate = Convert.ToDateTime(aoDR["Exam_Start_Date"]);
                    if (aoDR["Exam_End_Date"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.mdtExamEndDate = Convert.ToDateTime(aoDR["Exam_End_Date"]);
                    if (aoDR["Total_Exam_Days"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miTotalExamDays = Convert.ToInt32(aoDR["Total_Exam_Days"]);
                    if (aoDR["Total_Exam_Days"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miTotalExamDays = Convert.ToInt32(aoDR["Total_Exam_Days"]);
                    if (aoDR["Instructions"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.msInstructions = Convert.ToString(aoDR["Instructions"]);
                    if (aoDR["School_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId = Convert.ToInt32(aoDR["School_Id"]);
                    if (aoDR["academic_Year_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId = Convert.ToInt32(aoDR["academic_Year_Id"]);
                    if (aoDR["Is_Deleted"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.msIsDeleted = Convert.ToString(aoDR["Is_Deleted"]);
                    if (aoDR["Insert_Date"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.mdtInsertDate = Convert.ToDateTime(aoDR["Insert_Date"]);
                    if (aoDR["Inserted_By_id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.msInsertedByid = Convert.ToString(aoDR["Inserted_By_id"]);
                    if (aoDR["Update_Date"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.mdtUpdateDate = Convert.ToDateTime(aoDR["Update_Date"]);
                    if (aoDR["Updated_By_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.msUpdatedById = Convert.ToString(aoDR["Updated_By_Id"]);
                    if (aoDR["Standard_Test_Id"] != DBNull.Value)
                        moSchoolwiseStandardExamScheduleMasterStruct.miStandardTestId = Convert.ToInt32(aoDR["Standard_Test_Id"]);

                }
            }
        }

        // This function is used to fetch the SchoolwiseStandardExamScheduleMaster Details
        private string FetchSchoolwiseStandardExamScheduleMasterDetailsFromDatabase(int miSchoolwiseStandardExamScheduleId)
        {
            string sSelectStatement = " SELECT  " +
                                      " Schoolwise_Standard_Exam_Schedule_Id" +
                                      " ,Standard_Id" +
                                      " ,SchoolWise_Test_Id" +
                                      " ,Exam_Start_Date" +
                                      " ,Exam_End_Date" +
                                      " ,Total_Exam_Days" +
                                      " ,Standard_Test_Id" +
                                      " ,Instructions" +
                                      " ,School_Id" +
                                      " ,academic_Year_Id" +
                                      " ,Is_Deleted" +
                                      " ,Insert_Date" +
                                      " ,Inserted_By_id" +
                                      " ,Update_Date" +
                                      " ,Updated_By_Id" +
                                      " FROM " +
                                      " Schoolwise_Standard_Exam_Schedule_Master" +
            " WHERE Schoolwise_Standard_Exam_Schedule_Id=" + miSchoolwiseStandardExamScheduleId +
            " AND Is_Deleted='N'";
            return sSelectStatement;

        }
        private string FetchSchoolwiseStandardExamScheduleMasterDetailsFromDatabase(int aiStandardId, int aiTestId)
        {
            string sSelectStatement = " SELECT  " +
                                      " Schoolwise_Standard_Exam_Schedule_Id" +
                                      " ,Standard_Id" +
                                      " ,SchoolWise_Test_Id" +
                                      " ,Exam_Start_Date" +
                                      " ,Exam_End_Date" +
                                      " ,Total_Exam_Days" +
                                      " ,Standard_Test_Id" +
                                      " ,Instructions" +
                                      " ,School_Id" +
                                      " ,academic_Year_Id" +
                                      " ,Is_Deleted" +
                                      " ,Insert_Date" +
                                      " ,Inserted_By_id" +
                                      " ,Update_Date" +
                                      " ,Updated_By_Id" +
                                      " FROM " +
                                      " Schoolwise_Standard_Exam_Schedule_Master" +
                                      " WHERE " +
                                      " Standard_Id=" + aiStandardId +
                                      " AND SchoolWise_Test_Id =" + aiTestId +
                                      " AND Is_Deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;

        }
        #endregion

        public DataSet GetStandardwiseExamSchedule(int aiStandardId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolwiseStandardExamScheduleMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("academic_Year_Id", moSchoolwiseStandardExamScheduleMasterStruct.miacademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardSubjectsExamSchedule");
            }
        }

        public static DataSet GetStandardwiseExamScheduleForTeacher(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetExamScheduleDetails");
            }
        }

        public void CopyExamScheduleToSelectedStandards(int aiSchoolId, int aiAcademicYearId, int aiSourceStandardId, int aiSourceStandardTestId, string asTargetStandardXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiSourceStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DestinationStandards", asTargetStandardXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SourceStandardTestId", aiSourceStandardTestId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyExamSchedule");
            }
        }
    }
}
