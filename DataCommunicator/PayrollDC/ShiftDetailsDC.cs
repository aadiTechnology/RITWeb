using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using PayrollEntities;

namespace DataCommunicator
{
    public class ShiftDetailsDC
    {
        #region "Constructer"
        private ShiftDetailsStruct moShiftDetailsStruct;
        public ShiftDetailsDC() 
        {

        }

        public ShiftDetailsDC(int miShiftId, int miSchoolId, int miAcademicYearId)
        {
            LoadShiftDetails(miShiftId, miSchoolId, miAcademicYearId);
        }
        #endregion

        #region " Properties "
        public virtual ShiftDetailsStruct ShiftDetailsStructDetails
        {
            get
            {
                return moShiftDetailsStruct;
            }
            set
            {
                moShiftDetailsStruct = value;
            }
        }
        #endregion " Properties "

        #region "struct"
        public struct ShiftDetailsStruct
        {
            public int miShiftId;

            public string msShiftName;

            public string msShiftStartTime;

            public string msShiftEndTime;

            public string msHalfDayTime;

            public string msLateMarkTime;

            public int miSchoolId;

            public int miAcademicYearId;

            public char mcIs_Deleted;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public bool mbIsDefault;
        }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// This function is used to duplicate entry of Stop Name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateShiftName()
        {
            string sWhere = "";
            bool bFlag = true;
            if (moShiftDetailsStruct.miShiftId != 0)
            {
                sWhere = " AND ShiftId<> N'" + moShiftDetailsStruct.miShiftId + "'";
            }
            string sSelectStatement = "SELECT COUNT(*) " +
                " FROM SchoolShifts " +
                "  WHERE " +
                " SchoolId = " + moShiftDetailsStruct.miSchoolId +
                " AND AcademicYearId = " + moShiftDetailsStruct.miAcademicYearId +
                " AND Is_Deleted=N'N' " +
                " AND ShiftName=N'" + StringUtility.ReplaceSingleQuoteInString(moShiftDetailsStruct.msShiftName, false) + "'" +
                  sWhere;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int i = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (i > 0)
                    bFlag = false;
            }
            return bFlag;
        }

        ///<summary>
        /// This function is used to insert the ShiftMaster Details
        ///</summary>
        public virtual void InsertShiftDetails(string asType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                if (asType == "Update")
                    oSQLServerDbUtility.AddParameter("ShiftId", moShiftDetailsStruct.miShiftId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", moShiftDetailsStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShiftName", moShiftDetailsStruct.msShiftName, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("ShiftStartTime", moShiftDetailsStruct.msShiftStartTime, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("ShiftEndTime", moShiftDetailsStruct.msShiftEndTime, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("HalfDayTime", moShiftDetailsStruct.msHalfDayTime, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("LateMarkTime", moShiftDetailsStruct.msLateMarkTime, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moShiftDetailsStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moShiftDetailsStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDefault", moShiftDetailsStruct.mbIsDefault, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertShiftDetails");
            }
        }

        /// <summary>
        /// This function is used to get all Shift details and bind to object data source. 
        /// </summary>
        public static DataTable GetAll(int aiSchoolId, int aiAcademicYearId, String sSortExpression, int iEndIndex, int startRowIndex)
        {
            if (sSortExpression == string.Empty || sSortExpression == "Name" || sSortExpression == "Name ASC")
                sSortExpression = "ShiftName";
            else if (sSortExpression == "Name DESC")
                sSortExpression = "ShiftName DESC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sSortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedShiftDetails");
            }
        }

        /// <summary>
        /// This function is used to get total count, used to Configure Screen.
        /// </summary>
        public static DataTable GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            string sStatement = " SELECT  * " +

              " FROM SchoolShifts" +
              "  WHERE " +
              " SchoolId = " + aiSchoolId +
              " AND AcademicYearId = " + aiAcademicYearId +
              " AND Is_Deleted=N'N'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sStatement);

        }

        /// <summary>
        /// This function is used to get total count of Shift details and bind to object data source. 
        /// </summary>
        public static int CountTotalShiftRecords(Int32 aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountShiftDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        ///<summary>
        /// This function is used to load the Shift Details
        ///</summary>
        private void LoadShiftDetails(int miShiftId, int miSchoolId, int miAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchShiftDetailsFromDatabase(miShiftId, miSchoolId, miAcademicYearId);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["ShiftId"] != DBNull.Value)
                                moShiftDetailsStruct.miShiftId = Convert.ToInt32(oDR["ShiftId"]);
                            if (oDR["ShiftName"] != DBNull.Value)
                                moShiftDetailsStruct.msShiftName = Convert.ToString(oDR["ShiftName"]);
                            if (oDR["ShiftStartTime"] != DBNull.Value)
                                moShiftDetailsStruct.msShiftStartTime = Convert.ToString(oDR["ShiftStartTime"]);
                            if (oDR["ShiftEndTime"] != DBNull.Value)
                                moShiftDetailsStruct.msShiftEndTime = Convert.ToString(oDR["ShiftEndTime"]);
                            if (oDR["HalfDayTime"] != DBNull.Value)
                                moShiftDetailsStruct.msHalfDayTime = Convert.ToString(oDR["HalfDayTime"]);
                            if (oDR["LateMarkTime"] != DBNull.Value)
                                moShiftDetailsStruct.msLateMarkTime = Convert.ToString(oDR["LateMarkTime"]);
                            if (oDR["SchoolId"] != DBNull.Value)
                                moShiftDetailsStruct.miSchoolId = Convert.ToInt32(oDR["SchoolId"]);
                            if (oDR["IsDefault"] != DBNull.Value)
                                moShiftDetailsStruct.mbIsDefault = Convert.ToBoolean(oDR["IsDefault"]);
                            if (oDR["AcademicYearId"] != DBNull.Value)
                                moShiftDetailsStruct.miAcademicYearId = Convert.ToInt32(oDR["AcademicYearId"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moShiftDetailsStruct.mcIs_Deleted = Convert.ToChar(oDR["Is_Deleted"]);
                            if (oDR["InsertedDate"] != DBNull.Value)
                                moShiftDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["InsertedDate"]);
                            if (oDR["InsertedById"] != DBNull.Value)
                                moShiftDetailsStruct.miInsertedById = Convert.ToInt32(oDR["InsertedById"]);
                            if (oDR["UpdatedDate"] != DBNull.Value)
                                moShiftDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["UpdatedDate"]);
                            if (oDR["UpdatedById"] != DBNull.Value)
                                moShiftDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["UpdatedById"]);
                        }
                    }
                }
            }
        }

        ///<summary>
        ///This function is used to fetch the Shift Details.
        ///</summary>
        private String FetchShiftDetailsFromDatabase(int miShiftId, int miSchoolId, int miAcademicYearId)
        {
            string sSelectStatement = " SELECT  " +
            "ShiftId" +
            ",ShiftName" +
            ",ShiftStartTime" +
            ",ShiftEndTime" +
            ",HalfDayTime" +
            ",LateMarkTime" +
            ",SchoolId" +
            ",AcademicYearId" +
            ",IsDefault" +
            ",Is_Deleted" +
            ",InsertedDate" +
            ",InsertedById" +
            ",UpdatedDate" +
            ",UpdatedById" +
            " FROM SchoolShifts" +
            " WHERE ShiftId=" + miShiftId +
            " AND SchoolId=" + miSchoolId +
            " AND AcademicYearId=" + miAcademicYearId;
            return sSelectStatement;
        }

        /// <summary>
        /// This method is used to check whether any user is associated with shift or not.
        /// </summary>
        /// <param name="aiShiftId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public int CheckDependencyForShift(int aiShiftId, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ShiftId", aiShiftId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CheckDependencyForShiftName");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        ///<summary>
        /// This function is used to delete the Shift Details
        ///</summary>
        public virtual void DeleteShiftDetails(int aiShiftId, int aiSchoolId, int aiAcademicYearId)
        {
            string sDeleteStatement = "UPDATE SchoolShifts" +
                                     " SET " +
                                     "Is_Deleted=N'Y'" +
                                     " WHERE " +
                                     " ShiftId=" + aiShiftId +
                                     " AND SchoolId=" + aiSchoolId +
                                     " AND AcademicYearId=" + aiAcademicYearId +
                                     " AND Is_Deleted = N'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This function is used to get shift details.
        /// </summary>
        public List<SchoolShifts> GetAllShifts(int aiSchoolId, int aiAcademicYearId)
        {
            List<SchoolShifts> olstSchoolShifts = new List<SchoolShifts>();

            string sStatement = " SELECT  * " +
              " FROM SchoolShifts" +
              "  WHERE " +
              " SchoolId = " + aiSchoolId +
              " AND AcademicYearId = " + aiAcademicYearId +
              " AND Is_Deleted=N'N'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStatement))
                {
                    if (oSqlDataReader != null)
                    {
                        SchoolShifts oSchoolShiftEntity;
                        while (oSqlDataReader.Read())
                        {
                            oSchoolShiftEntity = new SchoolShifts
                            {
                                ShiftId = Convert.ToInt32(oSqlDataReader["ShiftId"]),
                                ShiftName = oSqlDataReader["ShiftName"].ToString(),
                            };
                            olstSchoolShifts.Add(oSchoolShiftEntity);
                        }
                    }
                }
                return olstSchoolShifts;
            }

        }
        #endregion
    }
}
