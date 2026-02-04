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
    public class VehiclePUCDetailsBL
    {
        #region Data Member(s)

        private VehiclePUCDetailsDC moVehiclePUCDetailsDC;
        int miTotalRows = Constants.I_ZERO;

        #endregion

        #region Constructor(s)

        public VehiclePUCDetailsBL()
        {
            this.moVehiclePUCDetailsDC = new VehiclePUCDetailsDC();
        }

        public VehiclePUCDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moVehiclePUCDetailsDC = new VehiclePUCDetailsDC(aiSchoolId, aiAcademicYearId,aiInsertedById);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get vehicle details for fill Vehicle combobox.
        /// </summary>
        /// <returns></returns>
        public DataTable GetVehicalDetailsForComboBox()
        {
            return moVehiclePUCDetailsDC.GetVehicalDetailsForComboBox();
        }

        /// <summary>
        /// This method is used to save PUC detaiils.
        /// </summary>
        /// <param name="aoTransportPUCDetails"></param>
        public void Save(TransportPUCDetails aoTransportPUCDetails)
        {
            moVehiclePUCDetailsDC.Save(aoTransportPUCDetails);
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
        public List<TransportPUCDetails> GetAll(int aiSchoolId, bool abShowOldRecord, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {
            if (string.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "ExpiryDate";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_ASCENDING;
            }           
            asSortExpression = asSortExpression + " " + asSortDirection;

            if (asFilter == null)
                asFilter = string.Empty;
            int iEndIndex = startRowIndex + maximumRows;

            List<TransportPUCDetails> lstTransportPUCDetails =  moVehiclePUCDetailsDC.GetAll(aiSchoolId, abShowOldRecord, asSortExpression, startRowIndex, iEndIndex, asFilter);

            if (lstTransportPUCDetails.Count > Constants.I_ZERO)
                miTotalRows = lstTransportPUCDetails[0].TotalRows;
            else
                miTotalRows = Constants.I_ZERO;

            return lstTransportPUCDetails;
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
        /// This method is used to Delete vehicle PUC Details.
        /// </summary>
        /// <param name="aiVehiclePUCId"></param>
        public List<string> Delete(int aiVehiclePUCId)
        {
            return moVehiclePUCDetailsDC.Delete(aiVehiclePUCId);
        }

        /// <summary>
        /// This method is used to get Transport Option Images.
        /// </summary>
        /// <param name="aiTypeId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<TransportOptionImages> GetTransportOptionImages(int aiTypeId, int aiDetailsId, int aiVehicleId, int aiSchoolId)
        {
            return moVehiclePUCDetailsDC.GetTransportOptionImages(aiTypeId, aiDetailsId, aiVehicleId, aiSchoolId);
        }

        /// <summary>
        /// This method is used to delete Transport Option images.
        /// </summary>
        /// <param name="aiDetailsId"></param>
        /// <param name="aiTypeId"></param>
        public string DeleteTransportOptionImage(int aiDetailsId, int aiTypeId)
        {
            return moVehiclePUCDetailsDC.DeleteTransportOptionImage(aiDetailsId, aiTypeId);
        }

        /// <summary>
        /// This method is used to get vehicle details for edit.
        /// </summary>
        /// <param name="aiPUCId"></param>
        /// <returns></returns>
        public TransportPUCDetails Get(int aiPUCId)
        {
            return moVehiclePUCDetailsDC.Get(aiPUCId);
        }

        #endregion
    }
}
