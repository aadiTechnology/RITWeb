// File Name   : WeekDaysMasterDC.cs
// Created By  : Ketan     
//Created Date : 27/11/2007      

using System;
using System.Data;
using System.Collections;
using Utility;
using System.Data.SqlClient;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to handle all the database related operations on Weekdays_Master table. 
    /// </summary>
    public class WeekDaysMasterDC : DataCommunicatorBaseDC
    {
        #region structure

        public struct WeekDaysMasterStruct
        {
            public int miWeekDaysId;
            public int miSchoolId;
            public string msWeekDayName;
            public int miOriginalWeekDaysId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miAcademicYearId;
            public string msWeekDayShortName;
        }
        #endregion

        #region DataMember
        private WeekDaysMasterStruct moWeekDaysMasterStruct;
        #endregion

        #region Properties

        public WeekDaysMasterStruct WeekDaysMasterStructDetails
        {

            get { return moWeekDaysMasterStruct; }
            set { moWeekDaysMasterStruct = value; }
        }

        #endregion

        #region Constructors

        public WeekDaysMasterDC()
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to return insert statement.
        /// </summary>
        /// <returns></returns>
        public string InsertWeekDaysMaster()
        {
            string sInsertStatement = "INSERT INTO WeekDays_Master ( " +
                                        "   school_id " +
                                        " , Academic_Year_Id " +
                                        " , weekday_name" +
                                        " , original_weekdays_id" +
                                        " , inserted_by_id" +
                                        " , updated_by_id" +
                                        " , WeekDay_Short_Name" +

                                    ") VALUES (" + "  " +
                                        moWeekDaysMasterStruct.miSchoolId +
                                        " , " + moWeekDaysMasterStruct.miAcademicYearId +
                                        " , N'" + StringUtility.ReplaceSingleQuoteInString(moWeekDaysMasterStruct.msWeekDayName, false) + "' " +
                                        " , " + moWeekDaysMasterStruct.miOriginalWeekDaysId +
                                        " , " + moWeekDaysMasterStruct.miInsertedByid +
                                        " , " + moWeekDaysMasterStruct.miUpdatedById +
                                        " , N'" + StringUtility.ReplaceSingleQuoteInString(moWeekDaysMasterStruct.msWeekDayShortName, false) + "' " +
                                             " ) ";
            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to return delete statement.
        /// </summary>
        /// <returns></returns>
        public string DeleteWeekDaysMaster()
        {

            string sDeleteStatement = " DELETE FROM " +
                                      " WeekDays_Master" +
                                      " WHERE " +
                                      "(School_Id =" + moWeekDaysMasterStruct.miSchoolId +
                                      " AND Original_WeekDays_Id = " + moWeekDaysMasterStruct.miOriginalWeekDaysId +
                                      "AND Academic_Year_Id = " + moWeekDaysMasterStruct.miAcademicYearId + ")";

            return sDeleteStatement;
        }

        /// <summary>
        /// This method is used to return update statement.
        /// </summary>
        /// <returns></returns>
        public string UpdateWeekDaysMaster()
        {
            string sUpdateStatement = " UPDATE WeekDays_Master SET WeekDay_Short_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moWeekDaysMasterStruct.msWeekDayShortName, false) + "'" +
                                      " WHERE" +
                                      " School_Id =" + moWeekDaysMasterStruct.miSchoolId +
                                      " AND Original_WeekDays_Id = " + moWeekDaysMasterStruct.miOriginalWeekDaysId +
                                      " AND Academic_Year_Id = " + moWeekDaysMasterStruct.miAcademicYearId + "";
            return sUpdateStatement;
        }

        public bool CheckWeekdayConfigureOrNot(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                                    " COUNT(Weekday_Name)" +
                                    "FROM Weekdays_Master " +
                                    "WHERE " +
                                    " School_Id= " + aiSchoolId +
                                    " AND Is_Deleted= N'" + Constants.C_NO + "'" +
                                    " AND Academic_Year_Id=" + aiAcademicYearId + " ";
            Int32 iWeekdayCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iWeekdayCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iWeekdayCount == Constants.I_ZERO)
                return false;
            else
                return true;
        }
        /// <summary>
        /// This method is used to return datatable containing collection of all Weekdays details. 
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAllWeekDayConfiguration(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectQuery = " SELECT  -9999 as school_id " +
                                    ", Original_WeekDays_Id" +
                                    ",WeekDays_id " +
                                    ", WeekDay_name " +
                                    ", WeekDay_Short_Name" +
                              " FROM " +
                                    " WeekDays_master" +
                              " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id is null " +
                                    " AND WeekDays_id NOT IN " +
                                      "(SELECT " +
                                            "Original_WeekDays_Id" +
                                      " FROM " +
                                            "WeekDays_master" +
                                      " WHERE " +
                                            " is_deleted = N'" + Constants.C_NO + "'" +
                                            " AND school_id =" + aiSchoolId + "" +
                                            " AND Academic_Year_Id = " + aiAcademicYearId + ")" +
                              " UNION  SELECT " +
                                    " school_id  " +
                                    ", Original_WeekDays_Id " +
                                    ",WeekDays_id " +
                                    ", WeekDay_name " +
                                    ", WeekDay_Short_Name" +
                              " FROM " +
                                    "WeekDays_master " +
                              " WHERE " +
                                            " is_deleted = N'" + Constants.C_NO + "'" +
                                            " AND school_id =" + aiSchoolId + "" +
                                            " AND Academic_Year_Id = " + aiAcademicYearId +
                                            " ORDER BY Original_WeekDays_Id ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectQuery);
        }
        /// <summary>
        /// This method is used to return datatable containing collection of all Weekdays details. 
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetConfiguredWeekDays(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetConfiguredWeekDays");                   
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsDateWeekday(int aiSchoolId, int aiAcademicYearId, DateTime aoDt)
        {
            bool bReturn = true;
            string sSelectStatement = " SELECT COUNT(WeekDay_Name)" +
                                     " FROM WeekDays_Master " +
                                     " WHERE WeekDay_Name = " +
                                         " DATENAME(weekday,N'" + aoDt + "') " +
                                         " AND  School_Id= " + aiSchoolId + " " +
                                         " ANd Academic_Year_Id= " + aiAcademicYearId +
                                         " AND Is_Deleted= N'" + Constants.C_NO + "'";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iCount > 0)
                bReturn = true;
            else
                bReturn = false;
            return bReturn;
        }
        #endregion
    }

    /// <summary>
    /// This class is used to execute weekdays configuration transaction on Weekdays_Master table.
    /// </summary>
    public class WeekDaysMasterCollectionDC
    {
        #region PublicMethod
        /// <summary>
        /// This method update all Weekdays Configuration into Weekdays_Master table
        /// </summary>
        /// <param name="aoArrayListWeekDays"></param>
        public void UpdateWeekDaysConfiguration(ArrayList aoArrayListWeekDays)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListWeekDays.ToArray(typeof(string)));
        }
        #endregion
    }
}
