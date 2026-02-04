/*File Name - PaymentParameterDC.cs
 * Created By - Pravin Shinde
 * Created Date - 29 Oct 2013
 * Description - This class is used to manage payroll parameters.
 */
namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PayrollEntities;
    using System.Data;
    using System.Data.SqlClient;
    using Utility;

    /// <summary>
    /// This class contains the methods that will be used to manage payroll parameters.
    /// </summary>
    public class PaymentParameterDC
    {
        #region Data Member(s)

        private int miSchoolId;        
        private int miInsertedById;       

        #endregion

        #region Construstor(s)

        public PaymentParameterDC(int aiSchoolId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;            
            this.miInsertedById = aiInsertedById;
        }
        
        #endregion

         #region Public Method(s)

        /// <summary>
        /// This method is used to get all/selected the payment parameters.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<PaymentParameter> GetAll(int aiParameterId)
        {
            List<PaymentParameter> lstPaymentParameters = new List<PaymentParameter>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                                
                oSQLServerDbUtility.AddParameter("ParameterId", aiParameterId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPaymentParameters"))
                {
                    while(oSqlDataReader.Read())
                    {
                        PaymentParameter oPaymentParameter=new PaymentParameter
                        {
                            Id = oSqlDataReader["Id"].ToInt(),
                            Parameter = oSqlDataReader["Parameter"].ToString()
                        };

                        lstPaymentParameters.Add(oPaymentParameter);
                    }
                }                
            }

            return lstPaymentParameters;
        }        
        
        /// <summary>
        /// This method is used to save/update the existing payment parameter.
        /// </summary>
        /// <param name="aiParameterId"></param>
        /// <param name="asParameter"></param>
        public void Save(int aiParameterId,string asParameter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                                
                oSQLServerDbUtility.AddParameter("ParameterId", aiParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Parameter", asParameter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePaymentParameter");
            }
        }

        /// <summary>
        /// This method is used to delete the parameter from the given list view.
        /// </summary>
        /// <param name="aiParameterId"></param>
        public void Delete(int aiParameterId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                                
                oSQLServerDbUtility.AddParameter("ParameterId", aiParameterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePaymentParameter");
            }
        }

        #endregion
    }
}
