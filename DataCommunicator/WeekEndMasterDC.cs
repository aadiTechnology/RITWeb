using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Utility;
using System.Data.SqlClient;
using System.Collections;

namespace DataCommunicator
{
    public class WeekEndMasterDC : DataCommunicatorBaseDC
    {
        
        public struct WeekEndMasterStruct
        {
            public int miSchoolId;
            public int miOriginalWeekEndId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miAcademicYearId;
            public string msWeekEndShortName;
            public int miWeekEndId;
            public string msWeekEndName;
            public bool mbIsStaffApplicable;
        }

        private WeekEndMasterStruct moWeekEndMasterStruct;

        public WeekEndMasterStruct WeekEndMasterStructDetails
        {

            get { return moWeekEndMasterStruct; }
            set { moWeekEndMasterStruct = value; }
        }

        public WeekEndMasterDC()
        {

        }

        /// <summary>
        /// This method is used to return insert statement.
        /// </summary>
        /// <returns></returns>
        public string InsertWeekEndMaster()
        {
            bool isWeekendAvailable = chkIfWeekEndAvailable(moWeekEndMasterStruct.miSchoolId, moWeekEndMasterStruct.miAcademicYearId, moWeekEndMasterStruct.miOriginalWeekEndId);
            string sInsertStatement = null;

            if (isWeekendAvailable != true)
            {
                sInsertStatement = "INSERT INTO WeekEnd_Master ( " +
                                             "  AcademicYear_Id " +
                                             " , SchoolId " +
                                             " , WeekDay_name" +
                                             " , Original_Weekdays_Id" +
                                             " , Is_Deleted" +
                                             " , Insert_Date" +
                                             " , Inserted_By_Id" +
                                             " , WeekDay_Short_Name" +
                                             " , IsStaffApplicable" +
                                             " , Update_Date" +
                                             " , Updated_By_Id" +

                                        ") VALUES (" + "  " +
                                            moWeekEndMasterStruct.miAcademicYearId +
                                            " , " + moWeekEndMasterStruct.miSchoolId +
                                            " , N'" + StringUtility.ReplaceSingleQuoteInString(moWeekEndMasterStruct.msWeekEndName, false) + "' " +
                                            " , " + moWeekEndMasterStruct.miOriginalWeekEndId +
                                            " , N'" + Constants.C_NO + "' " +
                                            " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                                            " , " + moWeekEndMasterStruct.miInsertedByid +
                                            " , N'" + StringUtility.ReplaceSingleQuoteInString(moWeekEndMasterStruct.msWeekEndShortName, false) + "' " +
                                            " , " + Convert.ToInt32(moWeekEndMasterStruct.mbIsStaffApplicable) +
                                            " , N'" + DateTime.UtcNow.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                                            " , " + moWeekEndMasterStruct.miUpdatedById +
                                             " ) ";

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
            }

            else
            {
                sInsertStatement = "UPDATE WeekEnd_Master SET IsStaffApplicable =  " + moWeekEndMasterStruct.mbIsStaffApplicable.ToInt() +
                    ", WeekDay_Short_Name = N'" + moWeekEndMasterStruct.msWeekEndShortName + "'" +
                    " WHERE WeekEnd_Master.SchoolId = " + moWeekEndMasterStruct.miSchoolId +
                    " AND WeekEnd_Master.AcademicYear_Id = " + moWeekEndMasterStruct.miAcademicYearId +
                    " AND WeekEnd_Master.Original_WeekDays_Id = " + moWeekEndMasterStruct.miOriginalWeekEndId;
                    

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
            }

            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to return delete statement.
        /// </summary>
        /// <returns></returns>
        public string DeleteWeekEndMaster()
        {

            string sDeleteStatement = " DELETE FROM " +
                                      " WeekEnd_Master" +
                                      " WHERE " +
                                      "(SchoolId =" + moWeekEndMasterStruct.miSchoolId +
                                      " AND Original_WeekDays_Id = " + moWeekEndMasterStruct.miOriginalWeekEndId +
                                      "AND AcademicYear_Id = " + moWeekEndMasterStruct.miAcademicYearId + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);

            return sDeleteStatement;
        }

        /// <summary>
        /// This method is used to return update statement.
        /// </summary>
        /// <returns></returns>
        public string UpdateWeekEndMaster()
        {
            string sUpdateStatement = " UPDATE WeekEnd_Master SET WeekDay_Short_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moWeekEndMasterStruct.msWeekEndShortName, false) + "'" +
                                      " WHERE" +
                                      " School_Id =" + moWeekEndMasterStruct.miSchoolId +
                                      " AND Original_WeekDays_Id = " + moWeekEndMasterStruct.miOriginalWeekEndId +
                                      " AND Academic_Year_Id = " + moWeekEndMasterStruct.miAcademicYearId + "";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

            return sUpdateStatement;
        }

        /// <summary>
        /// This function is used to check whether weekend is available.
        /// </summary>
        public bool chkIfWeekEndAvailable(int aiSchoolId, int aiAcademicYearId, int aiOriginalWeekEndId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("WeekendId", aiOriginalWeekEndId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("IsWeekendAvailable", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IfAlreadyExistWeekend");
                return OSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This function is used to check whether the Weekends are applicable to all staff.
        /// </summary>
        public bool chkIfOtherStaffApplicable(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT COUNT(1) FROM " +
                                      " WeekEnd_Master" +
                                      " WHERE SchoolId = " + aiSchoolId +
                                      "AND AcademicYear_Id = " + aiAcademicYearId +
                                      " AND IsStaffApplicable = 1";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (iCount == 0)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// This function is used get all weekends.
        /// </summary>
        public List<int> GetAllWeekends(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetWeekendList"))
                    return this.GetAllWeekendList(oSqlDataReader);
            }
        }

        /// <summary>
        /// This function is used to get all weekends list.
        /// </summary>
        public List<int> GetAllWeekendList(SqlDataReader aoSqlDataReader)
        {
            List<int> lstWeekendId = new List<int>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    int iWeekendId = 0;
                    if (aoSqlDataReader["Original_WeekDays_Id"] != DBNull.Value)
                        iWeekendId = Convert.ToInt32(aoSqlDataReader["Original_WeekDays_Id"]);
                    lstWeekendId.Add(iWeekendId);
                }
                aoSqlDataReader.Close();
            }
            return lstWeekendId;
        }
    }

    public class WeekEndMasterCollectionDC
    {
        #region PublicMethod
        /// <summary>
        /// This method update all Weekdays Configuration into Weekdays_Master table
        /// </summary>
        /// <param name="aoArrayListWeekDays"></param>
        public void UpdateWeekEndConfiguration(ArrayList aoArrayListWeekDays)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListWeekDays.ToArray(typeof(string)));
        }
        #endregion
    }
       
}
