// -----------------------------------------------------------------------
// class  : OnlinePaymentTermsDC.cs
// Author : Yogesh
// Date   : 7 Aug 2015
// Description  : This class is used to write business logic about online payment term.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;

namespace DataCommunicator
{
    public class OnlinePaymentTermsDC
    {
        #region Data Member(s)
        
            private int miSchoolId = 0;
            private int miAcademicYearId = 0;
            private int miUserId = 0;
        
        #endregion

        #region Constructor(s)
            
            public OnlinePaymentTermsDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
            {
                miSchoolId = aiSchoolId;
                miAcademicYearId = aiAcademicYearId;
                miUserId = aiUserId;
            }

        #endregion
        
        #region Public Method(s)

            /// <summary>
            /// This method is used to return data table to fill term category combo box.
            /// </summary>
            /// <returns></returns>
            public DataTable GetOnlineTermsCatagory()
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                  return  oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable("usp_GetOnlinePaymentCatagories");
                }
            }

            public List<OnlinePaymentTermsDetails> Get(int aiCategoryId)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlinePaymentTerms"))
                        return this.FillLessonPlanParameters(oSqlDataReader);
                }
            }

            /// <summary>
            /// This method is used to fill Lesson Plan parameter entity list.
            /// </summary>
            /// <param name="aoSqlDataReader"></param>
            /// <returns></returns>
            private List<OnlinePaymentTermsDetails> FillLessonPlanParameters(SqlDataReader aoSqlDataReader)
            {
                List<OnlinePaymentTermsDetails> lstPerformanceParameters = new List<OnlinePaymentTermsDetails>();
                while (aoSqlDataReader.Read())
                {
                    lstPerformanceParameters.Add(new OnlinePaymentTermsDetails
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                        Discription = Convert.ToString(aoSqlDataReader["Discription"]),
                        TermsCatagoryId = Convert.ToInt32(aoSqlDataReader["TermsCatagoryId"])

                    });
                }

                return lstPerformanceParameters;
            }

            /// <summary>
            /// This method is used to save Description.
            /// </summary>
            /// <param name="aiId"></param>
            /// <param name="asDiscription"></param>
            /// <param name="aiTermsCatagoryId"></param>
            public void Save(int aiId, string asDiscription, int aiTermsCatagoryId)
            {

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {

                    oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Discription", asDiscription, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("TermsCatagoryId", aiTermsCatagoryId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);

                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveOnlinePaymentTerms");

                }
            }

            /// <summary>
            /// This mehtod is used to delete online payment term.
            /// </summary>
            /// <param name="aiId"></param>
            public void Delete(int aiId)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                { 
                oSQLServerDbUtility.AddParameter("Id", aiId,SqlDbType.Int);
                    
                    oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);

                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteOnlinePaymentTerms");
                }
            }

        #endregion
    }
}
