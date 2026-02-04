// Class Name       :- FeeStructureLinkDC
// Purpose          :- This class is used for manage fee structure link upload.
// Date Of creation :- 13 Apr 2015
// Author Name      :- Yogesh

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FeeStructureLinkDC
    {
        #region Data Member(s)

        private  int miSchoolId;
        private  int miUpdatedById;
        private  int miAcademicYearId;

        #endregion

        #region Constructor
        public FeeStructureLinkDC()
        {
        }

        public FeeStructureLinkDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miUpdatedById = aiUpdatedById;
            miAcademicYearId = aiAcademicYearId;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to insert/update retirement notice configuration. 
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void Save(string asLinkUrl)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LinkUrl", asLinkUrl, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveYearwiseFeeStructureDetails");
            }
        }


        /// <summary>
        /// This method is used to Link url for Inputed filters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeName"></param>
        /// <returns></returns>
        public Dictionary<string, string>  Get(int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abShowFeeStructureForNextYear)
        {
            Dictionary<string, string> feeStructure = new Dictionary<string, string>();

            string sLinkUrl = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowFeeStructureForNextYear", abShowFeeStructureForNextYear, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeStructure"))
                {
                    if (oSqlDataReader != null)
                    {
                        oSqlDataReader.Read();
                        {                       
                            if (oSqlDataReader["CurrentYearFeeStructureUrl"] != DBNull.Value)
                                feeStructure.Add("CurrentYearFeeStructureUrl", oSqlDataReader["CurrentYearFeeStructureUrl"].ToString());

                            if (oSqlDataReader["NextYearFeeStructureUrl"] != DBNull.Value)
                                feeStructure.Add("NextYearFeeStructureUrl", oSqlDataReader["NextYearFeeStructureUrl"].ToString());
                        }
                    }
                }
            }
            return feeStructure;
        }

        /// <summary>
        /// This method is used to delete fees structure for current year.
        /// </summary>
        public void Delete()
        {
            string sSelectStatement = " Update YearwiseFeeStructureDetails  " +
                "Set IsDeleted = 1" +
                " WHERE  " +
                "AcademicYearId = " + this.miAcademicYearId +
                " AND SchoolId =" + this.miSchoolId;

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSelectStatement);
        }

        #endregion
    }
   
}
