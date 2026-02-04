using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator.TransportDC;
using System.Data;
using SchoolEntities.Transport;
using Utility;

namespace BusinessLogic.TransportBL
{
    public class VehicleServicingDetailsBL
    {
        #region Data Member(s)

        private VehicleServicingDetailsDC moVehicleServicingDetailsDC;
        int miTotalRows = Constants.I_ZERO;

        #endregion

        #region Constructor(s)

        public VehicleServicingDetailsBL()
        {
            this.moVehicleServicingDetailsDC = new VehicleServicingDetailsDC();
        }

        public VehicleServicingDetailsBL(int aiSchoolId, int aiAcademicYearId)
        {
            this.moVehicleServicingDetailsDC = new VehicleServicingDetailsDC(aiSchoolId, aiAcademicYearId);
        }

        public VehicleServicingDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moVehicleServicingDetailsDC = new VehicleServicingDetailsDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save Servicing details in database.
        /// </summary>
        /// <param name="moVehicleServicingDetails"></param>
        public void Save(VehicleServicingDetails moVehicleServicingDetails)
        {
            moVehicleServicingDetailsDC.Save(moVehicleServicingDetails);
        }

        /// <summary>
        /// This method is used to get PUC details for fill list view.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="asFilter"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<VehicleServicingDetails> GetAll(int aiSchoolId, bool abShowOldRecord, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {
            if (string.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "NextServicingDate";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_ASCENDING;
            }
            asSortExpression = asSortExpression + " " + asSortDirection;

            if (asFilter == null)
                asFilter = string.Empty;
            int iEndIndex = startRowIndex + maximumRows;

            List<VehicleServicingDetails> lstVehicleServicingDetails = moVehicleServicingDetailsDC.GetAll(aiSchoolId, abShowOldRecord, asSortExpression, startRowIndex, iEndIndex, asFilter);

            if (lstVehicleServicingDetails.Count > Constants.I_ZERO)
                miTotalRows = lstVehicleServicingDetails[0].TotalRows;
            else
                miTotalRows = Constants.I_ZERO;

            return lstVehicleServicingDetails;
        }

        /// <summary>
        /// This method is used to get count of vehiclePUC Details.
        /// </summary>        
        /// <param name="aiSchoolId"></param>        
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        public int Count(int aiSchoolId, bool abShowOldRecord, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {
            return miTotalRows;
        }

        /// <summary>
        /// This method is used to get vehicle details for edit.
        /// </summary>
        /// <param name="aiPUCId"></param>
        /// <returns></returns>
        public VehicleServicingDetails Get(int aiServicingId)
        {
            return moVehicleServicingDetailsDC.Get(aiServicingId);
        }

        /// <summary>
        /// This method is used to delete Vehicle Servicing details.
        /// </summary>
        /// <param name="aiServicingId"></param>
        public List<string> Delete(int aiServicingId)
        {
            return moVehicleServicingDetailsDC.Delete(aiServicingId);
        }

        public static void UpdateNotificationDetails(int aiSchoolId, string asNotificationDetails)
        {
            VehicleServicingDetailsDC.UpdateNotificationDetails(aiSchoolId, asNotificationDetails);
        }

        public static List<TransportNotificationDetails> GetTransportDetails(int aiSchoolId, string asNotificationUserIds)
        {
            return VehicleServicingDetailsDC.GetTransportDetails(aiSchoolId, asNotificationUserIds);
        }

        #endregion
    }
}
