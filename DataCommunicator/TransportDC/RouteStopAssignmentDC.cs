/*File Name - RouteStopAssignmentDC.cs
 * Created By - Pravin Shinde
 * Created Date - 30 Nov 2013
 * Description - This class is used to assign route stop to the user.
 */
namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;    
    using System.Data;
    using System.Data.SqlClient;
    using Utility;
    using SchoolEntities;
    using SchoolEntities.Transport;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RouteStopAssignmentDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;        
        private int miInsertedById;     
        private List<ShiftDetails> mlstShiftDetails;

        #endregion

        #region Construstor(s)

        public RouteStopAssignmentDC(int aiSchoolId,int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }
        
        #endregion

        #region Properties(s)

        public List<ShiftDetails> ShiftDetail
        {
            get { return mlstShiftDetails; }
            set { mlstShiftDetails = value; }
        }        

        #endregion

        #region Public Method(s)        

        /// <summary>
        /// This method is used to get all Stops & shifts.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<StopDetails> GetStopShiftDetails(int aiRouteId)
        {
            List<StopDetails> lstStopDetail = new List<StopDetails>();            

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RouteId", aiRouteId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Transport.usp_GetStopShiftDetails"))
                {
                    lstStopDetail = FillStopShifts(oSqlDataReader);
                }
            }
            
            return lstStopDetail;
        }
           
        /// <summary>
        /// This method is used to insert & update travelers details.
        /// </summary>
        /// <param name="asTransportDetailsXML"></param>
        public void Insert(string asTransportDetailsXML, DateTime aodtEffectiveFromDate, string asEndDate, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EffectiveFromDate", aodtEffectiveFromDate, SqlDbType.DateTime);
                if (asEndDate.TrimAll() != string.Empty)
                    oSQLServerDbUtility.AddParameter("EndDate", asEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("TransportDetailsXML", asTransportDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Transport.usp_InsertTravelersTransportDetails");
            }
        }      

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill the stops and shifts into the list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StopDetails> FillStopShifts(SqlDataReader aoSqlDataReader)
        {
            List<StopDetails> lstStopDetail = new List<StopDetails>();
            mlstShiftDetails = new List<ShiftDetails>();

            while (aoSqlDataReader.Read())
            {
                StopDetails oStopDetails = new StopDetails
                {
                    StopId = aoSqlDataReader["StopId"].ToInt(),
                    StopName = aoSqlDataReader["StopName"].ToString()
                };

                lstStopDetail.Add(oStopDetails);
            }

            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    ShiftDetails oShiftDetails = new ShiftDetails
                    {
                        ShiftId = aoSqlDataReader["ShiftId"].ToInt(),
                        ShiftName = aoSqlDataReader["ShiftName"].ToString(),
                        JourneyTypeId = aoSqlDataReader["JourneyTypeId"].ToInt()
                    };

                    mlstShiftDetails.Add(oShiftDetails);
                }
            }

            return lstStopDetail;
        }

        #endregion
    }
}
