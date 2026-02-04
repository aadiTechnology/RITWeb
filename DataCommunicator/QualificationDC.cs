// -----------------------------------------------------------------------
/* File Name = QualificationDC
 * Created Date - 12 March 2015
 * Created by - Yogesh
 * Class Description - This class used to manage business logic about qualification details*/
//// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using SchoolEntities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualificationDC
    {
         #region Public Method(s)

        /// <summary>
        /// This method is used to save qualification details
        /// </summary>
        /// <param name="aoQualificatoinDetails"></param>
        /// <param name="aiInsertedById"></param>
        /// <returns></returns>
        public static string Save(QualificatoinDetails aoQualificatoinDetails, int aiInsertedById, int aiAcademicYearId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Qualification", aoQualificatoinDetails.Qualification, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("QualificationId", aoQualificatoinDetails.QualificationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("DuplicationErr", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveQualificationDetails");
                return Convert.ToString(oSqlParam.Value);
            }
        }

        /// <summary>
        /// This method is to delete qualification details.
        /// </summary>
        /// <param name="aiQualificationId"></param>
        public static void Delete(int aiQualificationId,int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("QualificationId", aiQualificationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteQualificationDetails");
            }
        }

        /// <summary>
        /// This method is used to Get Qualification details.
        /// </summary>
        /// <returns></returns>
        public static List<QualificatoinDetails> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllQualificationDetails"))
                return LoadQualificationDetails(oSqlDataReader);
            }
        }

        #endregion

         #region Private Method(s)

        /// <summary>
        /// This method is used to Load collection qualification.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<QualificatoinDetails> LoadQualificationDetails(SqlDataReader aoSqlDataReader)
        {
            List<QualificatoinDetails> lstQualificatoinDetails = new List<QualificatoinDetails>();
            QualificatoinDetails oQualificatoinDetails;
            while (aoSqlDataReader.Read())
            {
                oQualificatoinDetails = new QualificatoinDetails()
                {
                    QualificationId = Convert.ToInt32(aoSqlDataReader["Qualification_Id"]),
                    Qualification = Convert.ToString(aoSqlDataReader["Qualification_Name"]),
                    IsUsedByTeacher = Convert.ToInt32(aoSqlDataReader["IsUsedByTeacher"])
                };
                lstQualificatoinDetails.Add(oQualificatoinDetails);
            }

            return lstQualificatoinDetails;
        }

        #endregion
    }
    }
