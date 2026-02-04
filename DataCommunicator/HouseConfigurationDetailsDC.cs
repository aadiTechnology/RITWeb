// Class Name       :- HouseConfigurationDetailsDC
// Purpose          :- This class is used to get all Standard for house configuration.
// Date Of creation :- 03/11/2015
// Author Name      :-


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Data;
namespace DataCommunicator
{
    public class HouseConfigurationDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public HouseConfigurationDetailsDC() { }
        public HouseConfigurationDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get all standards for house configuration.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<HouseConfigurationDetails> GetAll()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<HouseConfigurationDetails> lstHouseConfigurationDetails = new List<HouseConfigurationDetails>();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllHouseConfigStandards"))
                {
                    while (oSqlDataReader.Read())
                    {
                        HouseConfigurationDetails oHouseConfigurationDetails = new HouseConfigurationDetails();
                        oHouseConfigurationDetails.StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]);
                        oHouseConfigurationDetails.StandardName = Convert.ToString(oSqlDataReader["Standard_Name"]);
                        if (oSqlDataReader["AllowHouseConfiguration"] != DBNull.Value)
                            oHouseConfigurationDetails.AllowHouseConfiguration = Convert.ToBoolean(oSqlDataReader["AllowHouseConfiguration"]);
                        lstHouseConfigurationDetails.Add(oHouseConfigurationDetails);
                    }
                }
                return lstHouseConfigurationDetails;
            }
        }

        /// <summary>
        /// This method is used to save standard wise house configuration.
        /// </summary>
        /// <param name="asStandardIds"></param>
        public void Save(string asStandardIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardIds", asStandardIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId",this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveHouseConfigStandards");
            }
        }

        #endregion
    }
}
