// File Name  : HolidaysMasterDC.cs
// Created By : Ketan
// Date       : 28/11/2007   
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using SchoolEntities;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to handle all the database related operations on Weekdays_Master table. 
    /// </summary>
    public class HolidaysMasterDC : DataCommunicatorBaseDC
    {
        #region structure

        public struct HolidaysMasterStruct
        {
            public int miHolidayId;
            public string msHolidayName;
            public DateTime mdtHolidayStartDate;
            public DateTime mdtHolidayEndDate;
            public string AssociatedStandards;
            public string msRemarks;
            public string msIsDeleted;
            public bool mbAllowOverLapping;
            public DateTime mdtInsertDate;
            public DateTime mdtUpdateDate;
            public int miSchoolId;
            public int miAcademicYearId;
            public int miInsertedById;
        }

        #endregion

        #region Data members

        private HolidaysMasterStruct moHolidaysMasterStruct;

        #endregion

        #region Properties

        public HolidaysMasterStruct HolidaysMasterStructDetails
        {
            get
            {
                return moHolidaysMasterStruct;
            }
            set
            {
                moHolidaysMasterStruct = value;
            }
        }

        #endregion

        #region Constructors

        public HolidaysMasterDC()
        {
        }
        public HolidaysMasterDC(int aiHolidayId, int aiSchoolId, int aiAcademicYearId) 
        {
            LoadHolidaysMasterDetails(aiHolidayId,aiSchoolId,aiAcademicYearId); 
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to load holiday configuration data from Holidays_Master details.
        /// </summary>
        /// <param name="aiId"></param>
        public void LoadHolidaysMasterDetails(int aiHolidayId,int aiSchoolId,int aiAcademicYearId) 
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("HolidayId", aiHolidayId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId",aiAcademicYearId, SqlDbType.Int);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandrdwiseHolidayDetails"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Holiday_Id"] != DBNull.Value)
                                moHolidaysMasterStruct.miHolidayId = Convert.ToInt32(oDR["Holiday_Id"].ToString());
                            if (oDR["Holiday_Name"] != DBNull.Value)
                                moHolidaysMasterStruct.msHolidayName = oDR["Holiday_Name"].ToString();
                            if (oDR["Holiday_Start_Date"] != DBNull.Value)
                                moHolidaysMasterStruct.mdtHolidayStartDate = Convert.ToDateTime(oDR["Holiday_Start_Date"].ToString());
                            if (oDR["Holiday_End_Date"] != DBNull.Value)
                                moHolidaysMasterStruct.mdtHolidayEndDate = Convert.ToDateTime(oDR["Holiday_End_Date"].ToString());
                            if (oDR["AssociatedStandard"] != DBNull.Value)
                                moHolidaysMasterStruct.AssociatedStandards = oDR["AssociatedStandard"].ToString();
                            if (oDR["Remarks"] != DBNull.Value)
                                moHolidaysMasterStruct.msRemarks = oDR["Remarks"].ToString();
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moHolidaysMasterStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["AllowOverLapping"] != DBNull.Value)
                                moHolidaysMasterStruct.mbAllowOverLapping = oDR["AllowOverLapping"].ToBool();
                            moHolidaysMasterStruct.miSchoolId = aiSchoolId;
                            moHolidaysMasterStruct.miAcademicYearId = aiAcademicYearId;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to fetch holiday configuration data from Holidays_Master details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public string FetchHolidaysMasterDataFromDatabase(int aiHolidayId) 
        {
            string sSelectStatement = " SELECT  " +
                "   holiday_id  " +
                " , holiday_name" +
                " , holiday_start_date" +
                " , holiday_end_date" +
                " , AssociatedStandard" +
                " , remarks" +
                " , is_deleted" +
            " FROM  " +
                "Holidays_Master " +
            " WHERE  " +
                 "holiday_id = " + aiHolidayId +
                " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to insert holiday configuration data into Holidays_Master details.
        /// </summary>
        public void InsertHolidaysMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("HolidayName", moHolidaysMasterStruct.msHolidayName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Remarks", moHolidaysMasterStruct.msRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartDate", moHolidaysMasterStruct.mdtHolidayStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", moHolidaysMasterStruct.mdtHolidayEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("SchoolId", moHolidaysMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moHolidaysMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moHolidaysMasterStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssociatedStandard", moHolidaysMasterStruct.AssociatedStandards, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("AllowOverLapping", moHolidaysMasterStruct.mbAllowOverLapping, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertHolidays");
            }
        }
        /// <summary>
        /// This method is used to get Holiday 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetHolidayDetails(int aiSchoolId, int aiAccYrId, String sortExpression, int maximumRows, int startRowIndex,int aiStandardId,int iDivisionId)
        {
            if (!string.IsNullOrEmpty(sortExpression))
            {
                if (sortExpression == "Holiday_Start_Date")
                    sortExpression = " ORDER BY " + sortExpression;
                else
                    sortExpression = " ORDER BY " + sortExpression;
            }
            else
                sortExpression = " ORDER BY Holiday_Start_Date ASC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAccYrId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId",iDivisionId,SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetHolidayDetails");
            }
        }

        public List<Holiday> GetHolidayDetails(int aiSchoolId, int aiAccYrId)
        {
            List<Holiday> lstHolidays = new List<Holiday>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAccYrId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHolidayDetailsforStaffLeaveDeduction"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            Holiday oHolidays = new Holiday
                                                        {
                                                            Holiday_Name = oSqlDataReader["Holiday_Name"].ToString(),
                                                            Holiday_Start_Date = oSqlDataReader["Holiday_Start_Date"].ToDateTime(),
                                                            Holiday_End_Date = oSqlDataReader["Holiday_End_Date"].ToDateTime()
                                                        };
                            lstHolidays.Add(oHolidays);
                        }
                    }
                    return lstHolidays;
                }
            }
        }

        /// <summary>
        /// This method is used to calculate record count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <returns></returns>
        public int GetHolidayCount(int aiSchoolId, int aiAccYrId,int aiStandardId,int iDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAccYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId",iDivisionId,SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountHoliday");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method Check whether holiday is persent in Holidays_Master or not
        /// </summary>
        /// <returns></returns>
        public Int32 CheckForDuplicateHolidayName()
        {
            string sStandards = moHolidaysMasterStruct.AssociatedStandards;
            sStandards = sStandards.Remove(sStandards.Length-1);
            StringBuilder sFilter = new StringBuilder();
            sFilter.Append(" WHERE " +
                                    " Holiday_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moHolidaysMasterStruct.msHolidayName, false) + "' " + //asHolidayName
                                    " AND School_Id = " + moHolidaysMasterStruct.miSchoolId + " " +
                                    " AND Academic_Year_Id = " + moHolidaysMasterStruct.miAcademicYearId +
                                    " AND StandardwiseHoidaysMaster.StandardId IN( " + sStandards+") "+
                                    " AND Convert(Date,"+"'" + moHolidaysMasterStruct.mdtHolidayStartDate +"'"+ ") = Holiday_Start_Date" +
                                    " AND Convert(Date,"+"'" + moHolidaysMasterStruct.mdtHolidayEndDate +"'"+ ") = Holiday_End_Date " +
                                    " AND StandardwiseHoidaysMaster.Is_Deleted =N'"+Constants.I_ZERO+"' "+
                                    " AND Holidays_Master.Is_Deleted= N'" + Constants.C_NO + "'");
                                    
            if (moHolidaysMasterStruct.miHolidayId != Constants.I_ZERO)
            {
                sFilter.Append(" AND Holiday_Id <> " + moHolidaysMasterStruct.miHolidayId + " ");
            }
            string sSelectStatment = " SELECT " +
                                        " COUNT(Holiday_Id)" +
                                     " FROM Holidays_Master INNER JOIN StandardwiseHoidaysMaster ON Holidays_Master.Holiday_Id=StandardwiseHoidaysMaster.HolidayId" +
                                        sFilter.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return (oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment));
        }

        /// <summary>
        /// This method check whether start and end date is persent in Holidays_Master or not
        /// </summary>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <returns></returns>
        public Int32 IsHolidayStartAndEndDatePredefined()
        {
            string sStandards = moHolidaysMasterStruct.AssociatedStandards;
            sStandards = sStandards.Remove(sStandards.Length - 1);
            StringBuilder sFilter = new StringBuilder();
            sFilter.Append(" WHERE " +
                                "( ( N'" + moHolidaysMasterStruct.mdtHolidayStartDate.ToString("MM/dd/yyyy") + "' " +
                           "BETWEEN Holiday_Start_Date AND Holiday_End_Date ) " +
                                " OR " +
                                "( N'" + moHolidaysMasterStruct.mdtHolidayEndDate.ToString("MM/dd/yyyy") + "' " +
                           "BETWEEN Holiday_Start_Date AND Holiday_End_Date) " +
                                " OR " +
                                "( Holiday_Start_Date BETWEEN N'" + moHolidaysMasterStruct.mdtHolidayStartDate.ToString("MM/dd/yyyy") + "' " +
                           " AND  '" + moHolidaysMasterStruct.mdtHolidayEndDate.ToString("MM/dd/yyyy") + "')" +
                                " OR " +
                                "( Holiday_End_Date BETWEEN N'" + moHolidaysMasterStruct.mdtHolidayStartDate.ToString("MM/dd/yyyy") + "' " +
                           " AND  '" + moHolidaysMasterStruct.mdtHolidayEndDate.ToString("MM/dd/yyyy") + "') )" +
                           " AND ( School_Id = " + moHolidaysMasterStruct.miSchoolId + " ) " +
                           " AND ( Academic_Year_Id = " + moHolidaysMasterStruct.miAcademicYearId + ")" +
                           " AND StandardwiseHoidaysMaster.StandardId IN( " + sStandards + ") " +
                           " AND StandardwiseHoidaysMaster.Is_Deleted =N'" + Constants.I_ZERO + "' " +
                           " AND Holidays_Master.Is_Deleted= N'" + Constants.C_NO + "'");

            if (moHolidaysMasterStruct.miHolidayId != Constants.I_ZERO)
            {
                sFilter.Append(" AND Holiday_Id <> " + moHolidaysMasterStruct.miHolidayId + "");
            }

            string sSelectStatment = "SELECT " +
                                         " COUNT(Holiday_Id) " +
                                     " FROM Holidays_Master INNER JOIN StandardwiseHoidaysMaster ON Holidays_Master.Holiday_Id=StandardwiseHoidaysMaster.HolidayId" +
                                     sFilter.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment);
        }

        /// <summary>
        /// This method is used to update Holiday_Master details.
        /// </summary>
        public void UpdateHolidaysMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("HolidayName",moHolidaysMasterStruct.msHolidayName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Remarks", moHolidaysMasterStruct.msRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartDate", moHolidaysMasterStruct.mdtHolidayStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", moHolidaysMasterStruct.mdtHolidayEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("SchoolId", moHolidaysMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moHolidaysMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById",moHolidaysMasterStruct.miInsertedById , SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssociatedStandard", moHolidaysMasterStruct.AssociatedStandards, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("HolidaysId", moHolidaysMasterStruct.miHolidayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllowOverLapping", moHolidaysMasterStruct.mbAllowOverLapping, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertHolidays");
            }
        }

        /// <summary>
        /// This method is used to delete a particular holiday from Holidays_Master.
        /// </summary>
        public void DeleteHolidaysMaster()
        {
            string sDelteStatement = " DELETE " +
                                     " FROM Holidays_Master " +
                                     " WHERE Holiday_Id = " + moHolidaysMasterStruct.miHolidayId + " " +
                                     "AND School_Id = " + moHolidaysMasterStruct.miSchoolId + " " +
                                     " AND Academic_Year_Id = " + moHolidaysMasterStruct.miAcademicYearId + " ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDelteStatement);

            string sDelteStatement1 = " DELETE " +
                                     " FROM StandardwiseHoidaysMaster " +
                                     " WHERE HolidayId = " + moHolidaysMasterStruct.miHolidayId + " " +
                                     " AND SchoolId = " + moHolidaysMasterStruct.miSchoolId + " " +
                                     " AND AcedemicYearId = " + moHolidaysMasterStruct.miAcademicYearId + " ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDelteStatement1);
        }

        /// <summary>
        /// This method is used to select all holiday configuration, total days and return datatable.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllHolidayConfiguration(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatment = "SELECT " +
                                        " Holiday_Id " +
                                        ", Holiday_Name " +
                                        ", Holiday_Start_Date " +
                                        ", Holiday_End_Date " +
                                        ", Remarks " +
                                        ", (datediff(day,Holiday_Start_Date,Holiday_End_Date)+1) AS TotalDays " +
                                   " FROM " +
                                        " HoliDays_Master " +
                                   " WHERE " +
                                        " Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Academic_Year_Id= " + aiAcademicYearId + " " +
                                        " AND School_Id= " + aiSchoolId + " " +
                                        " ORDER BY Holiday_Start_Date ASC ";
            //" ORDER BY Holiday_Name ASC ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatment);
        }

        /// <summary>
        /// This method is used to get upcoming holiday date.
        /// </summary>
        /// <param name="aiSchooId"></param>
        /// <param name="aiAccYearId"></param>
        /// <returns></returns>

        public static DataTable GetUpcomingHolidayDate(int aiSchooId, int aiAccYearId,int aiStdId,int aiDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchooId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccYearId", aiAccYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUpcomingHoliday");
            }
        }


        /// <summary>
        /// This function checks if the given date is holiday for school.
        /// </summary>
        /// <returns></returns>
        public static bool IsDateHoliday(int aiSchoolId, int aiAcademicYearId, DateTime aoDt)
        {
            bool bReturn;
            string sSelectStatment = "SELECT COUNT(*) " +
                            " FROM Holidays_Master" +
                            " WHERE " +
                                "  N'" + aoDt + "' " +
                           "BETWEEN Holiday_Start_Date AND Holiday_End_Date  " +
                           " AND School_Id = " + aiSchoolId + "  " +
                           " AND Academic_Year_Id = " + aiAcademicYearId + "" +
                           " AND Is_Deleted= N'" + Constants.C_NO + "'";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment);
            if (iCount > 0)
                bReturn = true;
            else
                bReturn = false;
            return bReturn;
        }
        #endregion
    }
}
