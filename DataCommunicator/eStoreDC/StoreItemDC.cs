using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.eStore;
using System.Data;
using System.Data.SqlClient;
using BookEntities;
using Utility;

namespace DataCommunicator
{
    public class StoreItemDC
    {
        #region Data Member

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor

        public StoreItemDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public StoreItemDC()
        {
        }

        #endregion

        #region Public Methods

        public DataTable GetStoreCategories()
        {
            string sSelectStatement = " SELECT" +
                                            " Id" +
                                            ", Name" +
                                      " FROM" +
                                            " StoreItemCategories" +
                                      " WHERE" +
                                            " IsDeleted = 0 " +
                                            "ORDER BY Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param   essxdwdee44a>
        /// <param name="asFilter"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetStoreItemList(int aiSchoolId, int aiAcademicYearId, int aiStoreCategory, string asStandardIds, string asFilter, string sortExpression, int iStartIndex, int iEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StoreCategoryId", aiStoreCategory, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardIds", asStandardIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetStoreItemList]");
            }
        }

        public void DeleteItem(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_DeleteStoreItem");
            }
        }

        #endregion
    }
}
