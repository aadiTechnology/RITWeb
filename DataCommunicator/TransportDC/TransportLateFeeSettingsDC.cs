// -----------------------------------------------------------------------
// Class Name       :- TransportLateFeeSettingsBL
// Purpose          :- This class use to get & set transport late fee setting details
// Date Of creation :- 11/22/2013
// Author Name      :- Ashish Sonawane
// -----------------------------------------------------------------------
namespace TransportDC
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using DataCommunicator;
    using TransportEntities;
    using Utility;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransportLateFeeSettingsDC
    {
       #region Data Member(s)
        private int miSchoolId;
        private int miAcademicYearId;
       #endregion

       #region Constructor (s)
        //Default contructor
        public TransportLateFeeSettingsDC()
        { 
        
        }
        //Define contructor with schoolis & academic year id 
        public TransportLateFeeSettingsDC(int aiSchoolId, int aiAcademicYearId)
        {
           this.miSchoolId = aiSchoolId;
           this.miAcademicYearId = aiAcademicYearId;
        }
       #endregion
      
       #region Public Method(s)
        /// <summary>
        /// This methos return list containing transport late fee for the school
        /// </summary>
        /// <returns></returns>
        public List<TransportLateFeeDueDate> GetAll(out TransportLateFeeSetting aoTransportLateFeeSetting)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId",this.miAcademicYearId,SqlDbType.Int);
                using (SqlDataReader aoSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Transport.USP_GetLateFeeSettings"))
                {
                    aoTransportLateFeeSetting = new TransportLateFeeSetting();
                    List<TransportLateFeeDueDate> lstTransportLateFeeSetting = FillDueDate(aoSqlDataReader);
                    if (aoSqlDataReader.NextResult() && aoSqlDataReader.Read())
                    {
                        if (aoSqlDataReader["ValueForType"] != DBNull.Value)
                            aoTransportLateFeeSetting.ValueForType = aoSqlDataReader["ValueForType"].ToInt();
                        if (aoSqlDataReader["LateFeeTypeId"] != DBNull.Value)
                            aoTransportLateFeeSetting.LateFeePerTypeId = aoSqlDataReader["LateFeeTypeId"].ToInt();
                        if (aoSqlDataReader["Amount"] != DBNull.Value)
                            aoTransportLateFeeSetting.LateFeeAmount = aoSqlDataReader["Amount"].ToInt();
                        if (aoSqlDataReader["TransportStartDate"] != DBNull.Value)
                            aoTransportLateFeeSetting.TransportStartDate = aoSqlDataReader["TransportStartDate"].ToDateTime();
                        if (aoSqlDataReader["TransportEndDate"] != DBNull.Value)
                            aoTransportLateFeeSetting.TransportEndDate = aoSqlDataReader["TransportEndDate"].ToDateTime();
                    }
                    return lstTransportLateFeeSetting;
                }
            }
        }
        /// <summary>
        /// This method communicate with database and update late fee settings applicable for transport
        /// </summary>
        /// <param name="asDueDateXml"></param>
        /// <param name="aoTransportLateFeeValue"></param>
        public void Insert(string asLateFeeSettingsXML, TransportLateFeeSetting aoTransportLateFeeValue)
        {
            using (var aoSQLServerDbUtility = new SQLServerDbUtility())
            {
                aoSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                aoSQLServerDbUtility.AddParameter("AcademicYearId",this.miAcademicYearId, SqlDbType.Int);
                aoSQLServerDbUtility.AddParameter("LateFeeSettingsXML", asLateFeeSettingsXML, SqlDbType.Xml);
                aoSQLServerDbUtility.AddParameter("LateFeeAmount", aoTransportLateFeeValue.LateFeeAmount, SqlDbType.Int);
                aoSQLServerDbUtility.AddParameter("LateFeePerTypeId", aoTransportLateFeeValue.LateFeePerTypeId, SqlDbType.Int);
                aoSQLServerDbUtility.AddParameter("ValueForType", aoTransportLateFeeValue.ValueForType, SqlDbType.Int);
                aoSQLServerDbUtility.AddParameter("InsertedById", aoTransportLateFeeValue.InsertedById, SqlDbType.Int);
                aoSQLServerDbUtility.ExecuteStoredProcedureOnServer("Transport.usp_InsertLateFeeSettings");
            }
        }
       #endregion
       
       #region Private Method (s)
        /// <summary>
        /// This method use to fill due date entity
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<TransportLateFeeDueDate> FillDueDate(SqlDataReader aoSqlDataReader)
        {
            List<TransportLateFeeDueDate> lstTransportLateFeeSetting = new List<TransportLateFeeDueDate>();
            while (aoSqlDataReader.Read())
            {
                TransportLateFeeDueDate oTransportLateFeeSetting = new TransportLateFeeDueDate();
                if (aoSqlDataReader["Id"] != DBNull.Value)
                    oTransportLateFeeSetting.Id = Convert.ToInt16(aoSqlDataReader["Id"]);
                if (aoSqlDataReader["Month"] != DBNull.Value)
                    oTransportLateFeeSetting.Month = aoSqlDataReader["Month"].ToString();
                if (aoSqlDataReader["DueDate"] != DBNull.Value)
                    oTransportLateFeeSetting.DueDate = Convert.ToDateTime(aoSqlDataReader["DueDate"].ToString());
                lstTransportLateFeeSetting.Add(oTransportLateFeeSetting);
            }
            return lstTransportLateFeeSetting;
        }
       #endregion Private Method (s)
    }
}
