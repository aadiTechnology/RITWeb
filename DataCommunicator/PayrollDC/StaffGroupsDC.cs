// Class Name       :- StaffGroupsDC
// Purpose          :- This class is used to manage Staff Groups details.
// Date Of creation :- 11/2/2009
// Author Name      :- Sachin

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class StaffGroupsDC
    {
        #region Data Member(s)

        private StaffGroupsEntity moStaffGroupsEntity;
        private List<StaffGroupsEntity> mlstStaffGroups = new List<StaffGroupsEntity>(); 

        #endregion
               
        #region Property(s)

        public StaffGroupsEntity StaffGroupsEntity
        {
            get { return moStaffGroupsEntity; }
            set { moStaffGroupsEntity = value; }
        }

        public List<StaffGroupsEntity> StaffGroups
        {
            get { return mlstStaffGroups; }
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This emthod is used to return insert statement.
        /// </summary>
        /// <returns></returns>
        public string GetInsertStatement()
        {
            string sInsertStatement = "INSERT INTO StaffGroups ( " +
                " StaffGroupsName" +
                " , OriginalStaffGroupsId" +
                " , schoolid" +
                " , InsertDate" +
                " , InsertedById" +
                " , UpdateDate" +
                " , UpdatedById" +
            ") VALUES (" +
                 " N'" + StringUtility.ReplaceSingleQuoteInString(moStaffGroupsEntity.StaffGroupsName, false) + "' " +
                 " , " + moStaffGroupsEntity.OriginalStaffGroupsId +
                 " , " + moStaffGroupsEntity.SchoolId +
                 " ,N'" + DateTime.Now.ToShortDateString() + "' " +
                 " , " + moStaffGroupsEntity.InsertedById +
                 " ,N'" + DateTime.Now.ToShortDateString() + "' " +
                 " , " + moStaffGroupsEntity.UpdatedById +
                 " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This emthod is used to return update statement.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatement()
        {

            string sUpdateStatement = " UPDATE StaffGroups SET " +
                " StaffGroupsName =  N'" + StringUtility.ReplaceSingleQuoteInString(moStaffGroupsEntity.StaffGroupsName, false) + "' " +
                " , OriginalStaffGroupsId =  " + moStaffGroupsEntity.OriginalStaffGroupsId +
                " , UpdatedById =  " + moStaffGroupsEntity.UpdatedById +
                " , UpdateDate =  N'" + DateTime.Now.ToShortDateString() + "' " +
             " WHERE " +
                " Is_Deleted = N'" + Constants.C_NO + "'" +
                " AND Schoolid =  " + moStaffGroupsEntity.SchoolId +
                " AND StaffGroupsId =  " + moStaffGroupsEntity.StaffGroupsId;

            return sUpdateStatement;
        }

        /// <summary>
        /// This emthod is used to return delete statement.
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatement()
        {
            string sDeleteStatement = " UPDATE StaffGroups" +
                                      " SET Is_Deleted = N'" + Constants.C_YES + "'" +
                                      " , UpdatedById =  " + moStaffGroupsEntity.UpdatedById +
                                      " , UpdateDate =  N'" + DateTime.Now.ToShortDateString() + "' " +
                                      " WHERE " +
                                      " Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " AND Schoolid =  " + moStaffGroupsEntity.SchoolId +
                                      " AND StaffGroupsId =  " + moStaffGroupsEntity.StaffGroupsId;
            return sDeleteStatement;
        }

        /// <summary>
        /// This method is used to return  a datatable of staff groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetAll(int aiSchoolId)
        {
            string sSelectStatement;
            sSelectStatement = " SELECT " +
                                   " -9999 as SchoolId" +
                                   " , OriginalStaffGroupsId " +
                                   " , StaffGroupsId " +
                                   " , StaffGroupsName " +
                               " FROM " +
                                    " StaffGroups " +
                               " WHERE " +
                                    " Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND SchoolId is null " +
                                    " AND StaffGroupsId NOT IN " +
                                    " ( " +
                                     " SELECT  " +
                                           " OriginalStaffGroupsId " +
                                       " FROM " +
                                            " StaffGroups " +
                                       " WHERE " +
                                            " Is_Deleted = N'" + Constants.C_NO + "'" +
                                            " AND SchoolId = " + aiSchoolId +
                                       " )" +
                               " UNION " +
                                " SELECT  " +
                                   " SchoolId " +
                                   " , OriginalStaffGroupsId " +
                                   " , StaffGroupsId " +
                                   " , StaffGroupsName " +
                               " FROM " +
                                    " StaffGroups " +
                               " WHERE " +
                                    " Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND SchoolId = " + aiSchoolId +
                                " ORDER BY " +
                                     " OriginalStaffGroupsId";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to return all staff groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<StaffGroupsEntity> GetAllStaffGroups(int aiSchoolId)
        {
            List<StaffGroupsEntity> olstStaffGroups = new List<StaffGroupsEntity>();
            string sSelectStatement = "SELECT" +
                                     " StaffGroups.StaffGroupsId " +
                                     " ,StaffGroupsName  " +
                                     " ,OriginalStaffGroupsId  " +
                                     "FROM" +
                                     " StaffGroups " +
                                     "INNER JOIN" +
                                     " (" +
                                     " SELECT DISTINCT" +
                                     " StaffGroupsId " +
                                     " FROM " +
                                     "UsersStaffGroupsAssociation" +
                                     " WHERE " +
                                     " SchoolId = " + aiSchoolId +
                                     " AND Is_Deleted = N'N'" +
                                     " AND UsersStaffGroupsAssociation.Is_Locked = 0" +
                                     " )S" +
                                     " ON StaffGroups.StaffGroupsId = S.StaffGroupsId" +
                                     " WHERE " +
                                     " SchoolId =   " + aiSchoolId +
                                     " AND Is_Deleted = N'N'" +
                                     " ORDER BY OriginalStaffGroupsId";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oSqlDataReader != null)
                    {
                        StaffGroupsEntity oStaffGroupsEntity;
                        while (oSqlDataReader.Read())
                        {
                            oStaffGroupsEntity = new StaffGroupsEntity
                            {
                                StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]),
                                StaffGroupsName = oSqlDataReader["StaffGroupsName"].ToString(),
                                OriginalStaffGroupsId = Convert.ToInt32(oSqlDataReader["OriginalStaffGroupsId"])
                            };
                            olstStaffGroups.Add(oStaffGroupsEntity);
                        }
                    }
                }
                return olstStaffGroups;
            }
        }

        /// <summary>
        /// This method is used to update staff groups.
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void Update(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to update staff groups-Earning deductions association.
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UpdateStaffGroupsAndEarningsDeductionsAssociation(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        } 

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill staff groups entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetStaffGroups(SqlDataReader oSqlDataReader)
        {
            StaffGroupsEntity oStaffGroupsDC;
            while (oSqlDataReader.Read())
            {
                oStaffGroupsDC = new StaffGroupsEntity
                {
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]),
                    OriginalStaffGroupsId = Convert.ToInt32(oSqlDataReader["OriginalStaffGroupsId"]),
                    StaffGroupsName = Convert.ToString(oSqlDataReader["StaffGroupsName"])
                };
                mlstStaffGroups.Add(oStaffGroupsDC);
            }
        }

        #endregion
    }
}
