
// Class Name       :- LectureTimingDC
// Purpose          :- This class is used to manage Notice Board details.
// Date Of creation :- 21/11/2008
// Author Name      :- Ashish


using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Utility;
namespace DataCommunicator
{
    public class LectureTimingDC : DataCommunicatorBaseDC
    {
        #region Constructor

        public LectureTimingDC()
        {
        }
        public LectureTimingDC(int aiLectureTimingDetailId)
        {
            LoadLectureTimingDetails(aiLectureTimingDetailId);
        }
        #endregion

        #region Data member and property

        private LectureTimingStructDetails moLectureTimingStructDetails;
        public LectureTimingStructDetails LectureTimingInfo
        {
            get
            {
                return moLectureTimingStructDetails;
            }
            set
            {
                moLectureTimingStructDetails = value;
            }
        }
        #endregion

        #region " structures "

        public struct LectureTimingStructDetails
        {
            public int miSchoolId;

            public int miAcademicYearId;

            public int miLectureTimingDetailsId;

            public int miLectureTimingId;

            public int miSectionId;

            public int miLectureNo;

            public System.DateTime mdtStartTime;

            public System.DateTime mdtEndTime;

            public string msDescription;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

        }
        #endregion

        #region Public methods

        /// <summary>
        /// This methos is used to retrive lecture timings data table.
        /// </summary>
        /// <returns></returns>
        public DataTable RetrieveLectureTimingDetails()
        {
            string sWhere = "";
            if (moLectureTimingStructDetails.miSectionId != 0)
            {
                sWhere = "AND [Section]=" + moLectureTimingStructDetails.miSectionId;
            }

            string sSelectStatement = "SELECT " +
                                        " [School_LectureTimings_Detail_Id]" +
                                        " ,[School_LectureTimings_Id]" +
                                        " ,[Lecture_No]" +
                                        " ,[Start_Time]" +
                                        " ,[End_Time]" +
                                        " ,[Description]" +
                                        " ,[Section]" +
                                        " ,sStart_Time" +
                                        " ,sEnd_Time" +
                                     " FROM " +
                                        " [vw_School_Lecture_Timing_Details]" +
                                     " WHERE " +
                                       " [School_Id]=" + moLectureTimingStructDetails.miSchoolId +
                                       " AND [Academic_Year_Id]=" + moLectureTimingStructDetails.miAcademicYearId +
                                       " AND [Is_Deleted]=N'" + Constants.C_NO + "'" + sWhere;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to retrive lecture number using user define function.
        /// </summary>
        /// <returns></returns>
        public int RetrieveLectureNumber()
        {
            string sSelectStatement = "SELECT [dbo].Udf_GetLectureNoAsPerSection ("
                                            + moLectureTimingStructDetails.miSchoolId +
                                            "," + moLectureTimingStructDetails.miAcademicYearId +
                                            "," + moLectureTimingStructDetails.miSectionId +
                                            ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get section and standard name as per school id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSectionAndStandardName()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moLectureTimingStructDetails.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moLectureTimingStructDetails.miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSectionAndStandardName");
            }
        }

        /// <summary>
        /// This method is used to add lecture timing details to the database.
        /// </summary>
        public void AddLectureTimingDetails()
        {
            string sSelectStatement = "SELECT " +
                                        " [School_LectureTimings_Id]" +
                                        " FROM " +
                                        " [School_LectureTimings_Master] " +
                                    " WHERE " +
                                        " [School_Id]=" + moLectureTimingStructDetails.miSchoolId +
                                        " AND [Section]=" + moLectureTimingStructDetails.miSectionId +
                                        " AND [Academic_Year_Id]=" + moLectureTimingStructDetails.miAcademicYearId +
                                        " AND [Is_Deleted]=N'" + Constants.C_NO + "'";

            int iLectureTimings_Id;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iLectureTimings_Id = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iLectureTimings_Id == Constants.I_ZERO)
            {
                ArrayList sArrInsert = new ArrayList();
                string sInsertStatement = "INSERT INTO [School_LectureTimings_Master]" +
                                                "([Section]" +
                                                " ,[School_Id]" +
                                                " ,[Academic_Year_Id]" +
                                                " ,[Inserted_By_Id]" +
                                            " ) VALUES (" +
                                                "" + moLectureTimingStructDetails.miSectionId +
                                                "," + moLectureTimingStructDetails.miSchoolId +
                                                "," + moLectureTimingStructDetails.miAcademicYearId +
                                                "," + moLectureTimingStructDetails.miInsertedById +
                                                ")";

                sArrInsert.Add(sInsertStatement);
                sInsertStatement = GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
                sArrInsert.Add(sInsertStatement);
                sInsertStatement = GetLectTimeInsertStatement(iLectureTimings_Id);
                sArrInsert.Add(sInsertStatement);
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction((string[])sArrInsert.ToArray(typeof(string)));
            }
            else
            {
                string sInsertLectTimeDetails = GetLectTimeInsertStatement(iLectureTimings_Id);
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sInsertLectTimeDetails);
            }
        }

        private string GetLectTimeInsertStatement(int iLectureTimings_Id)
        {
            string sInsertStatement = string.Empty;
            string sLectureId = string.Empty;
            string sDescription = string.Empty;
            string sDescriptionColName = string.Empty;
            if (iLectureTimings_Id != Constants.I_ZERO)
            {
                sLectureId = "" + iLectureTimings_Id;
            }
            else
            {
                sLectureId = "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'";
            }
            if (moLectureTimingStructDetails.msDescription != "")
            {
                sDescriptionColName = ",[Description]";
                sDescription = ",N'" + StringUtility.ReplaceSingleQuoteInString(moLectureTimingStructDetails.msDescription, false) + "' ";
            }
            else
            {
                sDescriptionColName = "";
                sDescription = "";
            }

            sInsertStatement = "INSERT INTO School_LectureTimings_Details (" +
                                           " [School_LectureTimings_Id]" +
                                           " ,[Lecture_No]" +
                                           " ,[Start_Time]" +
                                           " ,[End_Time]" +
                                           sDescriptionColName +
                                           " ,[Is_Deleted]" +
                                           " ,[Inserted_By_Id]" +
                                       " ) VALUES ( " +
                                           sLectureId +
                                           "," + moLectureTimingStructDetails.miLectureNo +
                                           ",N'" + moLectureTimingStructDetails.mdtStartTime + "'" +
                                           ",N'" + moLectureTimingStructDetails.mdtEndTime + "'" +
                                           sDescription +
                                           ",N'" + Constants.C_NO + "'" +
                                           "," + moLectureTimingStructDetails.miInsertedById +
                                       ")";
            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to update lecture timing details into database.
        /// </summary>
        public void UpdateLectureTimingDetails()
        {
            string sUpdateStatement = "UPDATE School_LectureTimings_Details SET  " +
                                                "[Lecture_No] = " + moLectureTimingStructDetails.miLectureNo +
                                                " ,[Start_Time] = N'" + moLectureTimingStructDetails.mdtStartTime + "'" +
                                                " ,[End_Time] = N'" + moLectureTimingStructDetails.mdtEndTime + "'" +
                                                " ,[Description] = N'" + StringUtility.ReplaceSingleQuoteInString(moLectureTimingStructDetails.msDescription, true) + "' " +
                                                " ,[Updated_By_Id] = " + moLectureTimingStructDetails.miUpdatedById +
                                                " ,[Update_Date] = N'" + moLectureTimingStructDetails.mdtUpdateDate.ToShortDateString() + "'" +
                                            " WHERE " +
                                                " School_LectureTimings_Detail_Id=" + moLectureTimingStructDetails.miLectureTimingDetailsId +
                                                " AND [Is_Deleted] = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete lecture timing from the database table.
        /// </summary>
        public void DeleteLectureTiming(Char asIsLastRecord)
        {
            string sDeleteDetailTblEmtry = GetDeleteStatementForLectureTimingForDetailsTbl();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteDetailTblEmtry);

            if (asIsLastRecord == Constants.C_YES)
            {
                string sDeleteMasterEmtry = GetDeleteStatementForLectureTimingMasterTbl();
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sDeleteMasterEmtry);
            }
        }



        /// <summary>
        /// This method is used to ger lecture timing details.
        /// </summary>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiSection"></param>
        /// <returns></returns>
        public static DataSet GetAllLectureTimings(int aiSchoolID, int aiAcademicYrId, int aiSection)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("aiSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("aiAcdYrId", aiAcademicYrId, SqlDbType.Int);
                if (aiSection != 0)
                    oSQLServerDbUtility.AddParameter("aiSectionId", aiSection, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLectureTimings");
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method is used to get lecture timing details.
        /// </summary>
        /// <param name="aiLectureTimingDetailId"></param>
        private void LoadLectureTimingDetails(int aiLectureTimingDetailId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchLectureTimingDetailFromDatabase(aiLectureTimingDetailId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Lecture_No"] != DBNull.Value)
                                moLectureTimingStructDetails.miLectureNo = Convert.ToInt32(oDR["Lecture_No"]);
                            if (oDR["Start_Time"] != DBNull.Value)
                                moLectureTimingStructDetails.mdtStartTime = Convert.ToDateTime(oDR["Start_Time"]);
                            if (oDR["End_Time"] != DBNull.Value)
                                moLectureTimingStructDetails.mdtEndTime = Convert.ToDateTime(oDR["End_Time"]);
                            if (oDR["Description"] != DBNull.Value)
                                moLectureTimingStructDetails.msDescription = Convert.ToString(oDR["Description"]);
                            if (oDR["Section"] != DBNull.Value)
                                moLectureTimingStructDetails.miSectionId = Convert.ToInt32(oDR["Section"]);
                        }
                    }
                }
            }
        }

        private string FetchLectureTimingDetailFromDatabase(int aiLectureTimingDetailId)
        {
            string sSelectStatement = "SELECT " +
                                        " [Lecture_No]" +
                                        " ,[Start_Time]" +
                                        " ,[End_Time]" +
                                        " ,[Description]" +
                                        " ,[Section]" +
                                     " FROM " +
                                        " [vw_School_Lecture_Timing_Details]" +
                                     " WHERE " +
                                       " [School_LectureTimings_Detail_Id]=" + aiLectureTimingDetailId +
                                       " AND [Is_Deleted]=N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        /// <summary>
        /// This method is used to get delete statement for deleting lecture timing detail entry from details table.
        /// </summary>
        /// <returns></returns>
        private string GetDeleteStatementForLectureTimingForDetailsTbl()
        {
            string sDeleteStatement = "UPDATE School_LectureTimings_Details SET " +
                                            " [Is_Deleted]=N'" + Constants.C_YES + "'" +
                                            " , [Updated_By_Id]=" + moLectureTimingStructDetails.miUpdatedById +
                                            " , [Update_Date]=N'" + moLectureTimingStructDetails.mdtUpdateDate.ToShortDateString() + "'" +
                                         "  WHERE " +
                                            " School_LectureTimings_Detail_Id=" + moLectureTimingStructDetails.miLectureTimingDetailsId +
                                            " AND [School_LectureTimings_Id] =" + moLectureTimingStructDetails.miLectureTimingId;

            return sDeleteStatement;
        }

        /// <summary>
        /// This method is used to get delete statement for deleting lecture timing master entry from master table.
        /// </summary>
        /// <returns></returns>
        private string GetDeleteStatementForLectureTimingMasterTbl()
        {
            string sDeleteStatement = "UPDATE School_LectureTimings_Master SET " +
                                             " [Is_Deleted]=N'" + Constants.C_YES + "'" +
                                             " , [Updated_By_Id]=" + moLectureTimingStructDetails.miUpdatedById +
                                             " , [Update_Date]=N'" + moLectureTimingStructDetails.mdtUpdateDate.ToShortDateString() + "'" +
                                          "  WHERE " +
                                             " School_Id=" + moLectureTimingStructDetails.miSchoolId +
                                             " AND Academic_Year_Id=" + moLectureTimingStructDetails.miAcademicYearId +
                                             " AND [School_LectureTimings_Id] =" + moLectureTimingStructDetails.miLectureTimingId;

            return sDeleteStatement;
        }

        #endregion

    }
}
