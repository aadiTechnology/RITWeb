/*
 * File Name - UserPaymentCategoryAssoBL.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage association of user and payment category.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class UserPaymentCategoryAssoDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
       
        public UserPaymentCategoryAssoDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public UserPaymentCategoryAssoDC()
        {
            // TODO: Complete member initialization
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return all users for association.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<UserPaymentCategoryAssociation> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserPaymentCategoryAssociations"))
                {
                    List<UserPaymentCategoryAssociation> lstAssociations = new List<UserPaymentCategoryAssociation>();
                    while (oSqlDataReader.Read())
                    {
                        lstAssociations.Add
                            (
                                new UserPaymentCategoryAssociation
                                {
                                    Amount = Convert.ToInt32(oSqlDataReader["Amount"]),
                                    CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]),
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                    UserName = Convert.ToString(oSqlDataReader["UserName"]),
                                    SrNo = Convert.ToInt32(oSqlDataReader["SrNo"])
                                });
                    }
                    return lstAssociations;
                }
            }
        }

        /// <summary>
        /// This method is sued to return count of records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserPaymentCategoryAssoCount");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to save association.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserPaymentCategoryAsso");
            }
        } 

        #endregion
    }
}
