/*File Name - RouteStopAssignmentBL.cs
 * Created By - Pravin Shinde
 * Created Date - 30 Nov 2013
 * Description - This class is used to assign route stop to the user.
 */
namespace BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataCommunicator;
    using SchoolEntities;
    using SchoolEntities.Transport;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RouteStopAssignmentBL
    {
        #region Data Member(s)

        private RouteStopAssignmentDC moRouteStopAssignmentDC;

        #endregion

        #region Constructor(s)

        public RouteStopAssignmentBL(int aiSchoolId,int aiAcademicYearId, int aiInsertedById)
        {
            this.moRouteStopAssignmentDC = new RouteStopAssignmentDC(aiSchoolId,aiAcademicYearId, aiInsertedById);
        } 

        #endregion

        #region Properties(s)

        public List<ShiftDetails> ShiftDetail
        {
            get { return moRouteStopAssignmentDC.ShiftDetail; }
            set { moRouteStopAssignmentDC.ShiftDetail = value; }
        }

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to get all Stops.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<StopDetails> GetStopShiftDetails(int aiRouteId)
        {
            return moRouteStopAssignmentDC.GetStopShiftDetails(aiRouteId);
        }        

        /// <summary>
        /// This method is used to insert & update travelers details.
        /// </summary>
        /// <param name="asTransportDetailsXML"></param>
        public void Insert(string asTransportDetailsXML, DateTime aodtEffectiveFromDate,string asEndDate, int aiUserId)
        {
            moRouteStopAssignmentDC.Insert(asTransportDetailsXML, aodtEffectiveFromDate, asEndDate, aiUserId);
        }
       
        #endregion
    }
}
