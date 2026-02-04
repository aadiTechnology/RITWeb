using System;
using System.Collections.Generic;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;
using Utility;

namespace BusinessLogic.TransportBL
{
    public class TransportOverrideDetailsBL : BusinessLogicBaseBL
    {
        #region Data Member(s)

        private int miTotalRows;
        private TransportOverrideDetailsDC moTransportOverrideDetailsDC = null;

        #endregion

        #region Constructor(s)

        public TransportOverrideDetailsBL()
        {
            moTransportOverrideDetailsDC = new TransportOverrideDetailsDC();
        }

        public TransportOverrideDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moTransportOverrideDetailsDC = new TransportOverrideDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to delete transport override details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            moTransportOverrideDetailsDC.Delete(aiId);
        }

        /// <summary>
        /// This method is used to get transport override details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRouteNo"></param>
        /// <param name="asRouteName"></param>
        /// <param name="asVehicleNo"></param>
        /// <param name="asJourneyName"></param>
        /// <param name="asStudentName"></param>
        /// <param name="asStudentRegNo"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<OverrideDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asStudentName, string asStudentRegNo,string asOverrideName, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (asRouteNo == null)
                asRouteNo = string.Empty;
            if (asRouteName == null)
                asRouteName = string.Empty;
            if (asVehicleNo == null)
                asVehicleNo = string.Empty;
            if (asJourneyName == null)
                asJourneyName = string.Empty;
            if (asStudentName == null)
                asStudentName = string.Empty;
            if (asStudentRegNo == null)
                asStudentRegNo = string.Empty;
            if (asOverrideName == null)
                asOverrideName = string.Empty;
            
            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<OverrideDetails> lstOverrideDetails = moTransportOverrideDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, asRouteNo, asRouteName, asVehicleNo, asJourneyName, asStudentName, asStudentRegNo, asOverrideName, SortExpression, StartRowIndex, MaximumRows);

            if (lstOverrideDetails.Count > 0)
                miTotalRows = lstOverrideDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstOverrideDetails;
        }

        /// <summary>
        /// This method is used to count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRouteNo"></param>
        /// <param name="asRouteName"></param>
        /// <param name="asVehicleNo"></param>
        /// <param name="asJourneyName"></param>
        /// <param name="asStudentName"></param>
        /// <param name="asStudentRegNo"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public int GetCount(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asStudentName, string asStudentRegNo, string asOverrideName, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }

        /// <summary>
        /// This method is used to get override details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public TransportOverrideDetails Get(int aiId)
        {
            return moTransportOverrideDetailsDC.Get(aiId);
        }

        /// <summary>
        /// This method is used to return student list.
        /// </summary>
        /// <param name="aiRouteId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiJourneyId"></param>
        /// <returns></returns>
        public List<SchoolEntities.Student> GetStudentList(int aiRouteId, int aiVehicleId, int aiJourneyId)
        {
            return moTransportOverrideDetailsDC.GetStudentList(aiRouteId, aiVehicleId, aiJourneyId);
        }

        /// <summary>
        /// This method is used save override details.
        /// </summary>
        /// <param name="aoTransportOverrideDetails"></param>
        public void Save(TransportOverrideDetails aoTransportOverrideDetails)
        {
            moTransportOverrideDetailsDC.Save(aoTransportOverrideDetails);
        }

        /// <summary>
        /// This method is used to validate details.
        /// </summary>
        /// <param name="aiSourceRouteId"></param>
        /// <param name="aiSourceVehicleId"></param>
        /// <param name="aiSourceJourneyId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public string Validate(int aiSourceRouteId, int aiSourceVehicleId, int aiSourceJourneyId, DateTime adtStartDate, DateTime adtEndDate, int aiId, string asName)
        {
            return moTransportOverrideDetailsDC.Validate(aiSourceRouteId, aiSourceVehicleId, aiSourceJourneyId, adtStartDate, adtEndDate, aiId, asName);
        }
    }

        #endregion
}
