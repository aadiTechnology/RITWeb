using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using DataCommunicator.TransportDC;
using SchoolEntities.Transport;

namespace BusinessLogic.TransportBL
{
    public class TransportOverrideConfigDetailsBL :  BusinessLogicBaseBL
    {
        private int miTotalRows;
        private TransportOverrideConfigDetailsDC moTransportOverrideConfigDetailsDC = null;

        public TransportOverrideConfigDetailsBL()
        {
            moTransportOverrideConfigDetailsDC = new TransportOverrideConfigDetailsDC();
        }

        public TransportOverrideConfigDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            moTransportOverrideConfigDetailsDC = new TransportOverrideConfigDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }

        public void Delete(int aiId)
        {
            moTransportOverrideConfigDetailsDC.Delete(aiId);
        }

        public List<TransportOverrideConfigDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asName, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (asRouteNo == null)
                asRouteNo = string.Empty;
            if (asRouteName == null)
                asRouteName = string.Empty;
            if (asVehicleNo == null)
                asVehicleNo = string.Empty;
            if (asJourneyName == null)
                asJourneyName = string.Empty;
            if (asName == null)
                asName = string.Empty;

            if (SortExpression == null || SortExpression == string.Empty)
                SortExpression = "StartDate desc";
            else
            {
                SortExpression = SortExpression.Replace("asc", "").Replace("desc", "").Replace("ASC","").Replace("DESC", "").Trim();
                SortExpression = SortExpression + " " + SortDirection;
            }
            
            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<TransportOverrideConfigDetails> lstTransportOverrideConfigDetails = moTransportOverrideConfigDetailsDC.GetAll(aiSchoolId, aiAcademicYearId, asRouteNo, asRouteName, asVehicleNo, asJourneyName, asName, SortExpression, StartRowIndex, MaximumRows);

            if (lstTransportOverrideConfigDetails.Count > 0)
                miTotalRows = lstTransportOverrideConfigDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstTransportOverrideConfigDetails;
        }

        public int GetCount(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asName, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }
    }
}
