// Class Name       :- ImportFeeByChallanDC.cs
// Purpose          :- This class is used to import challan details.
// Date Of creation :- 5 Jul 2016
// Author Name      :- Yogesh

using System;
using System.Data;
using System.Data.SqlClient;
namespace DataCommunicator
{
    public class ImportFeeByChallanDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;
        private int miFinancialYearId;

#endregion

        #region Constructor(s)

        public ImportFeeByChallanDC()
        {
        }

        public ImportFeeByChallanDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUserId;            
        }
        
        public ImportFeeByChallanDC(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiFinancialYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUserId;
            miFinancialYearId = aiFinancialYearId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to Insert Fee by challan.
        /// </summary>
        /// <param name="asChallanDetails"></param>
        public void InsertFeeByChallan(string asChallanDetails, int aiOriginalFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OriginalFeeTypeId", aiOriginalFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChallanDetailsXml", asChallanDetails, SqlDbType.Xml);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ImportChallanDetails");
            }
        }

        /// <summary>
        /// This method is used to check given challan number is invalid or not.
        /// </summary>
        /// <param name="aiChallanNo"></param>
        /// <returns></returns>
        public bool InvalidChallanNo(int aiChallanNo, int aiOriginalFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChallanNo", aiChallanNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OriginalFeeTypeId", aiOriginalFeeTypeId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("InvalidChallanNo", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInvalideChallanDetails");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

        #endregion
    }
}
