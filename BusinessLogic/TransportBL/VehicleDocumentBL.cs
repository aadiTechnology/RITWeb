// File Name - VehicleDocumentBL.cs
// Creator - Vishakha
// Created Date -

using System.Collections.Generic;
using DataCommunicator;
using TransportEntities;
using Utility;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used for processing business logic and communicate with data access layer.
    /// </summary>
    public class VehicleDocumentBL
    {
        #region Data Member(s)

        private VehicleDocumentDC moVehicleDocumentDC;
        private int miTotalRows;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
        public VehicleDocumentBL()
        {
            this.moVehicleDocumentDC = new VehicleDocumentDC();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDocumentBL" /> class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public VehicleDocumentBL(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.moVehicleDocumentDC = new VehicleDocumentDC(aiSchoolId, aiAcademicYearId, aiInsertedById);
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save document.
        /// </summary>
        /// <param name="oVehicleDocumentDetails"></param>
        /// <param name="asFileName"></param>
        public void SaveDocument(VehicleDocumentDetails oVehicleDocumentDetails)
        {
            moVehicleDocumentDC.SaveDocument(oVehicleDocumentDetails);
        }

        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public void DeleteDocument(int aiId)
        {
            moVehicleDocumentDC.DeleteDocument(aiId);
        }

        /// <summary>
        /// THis method is used to get document list.
        /// </summary>
        /// <returns></returns>
        public List<Documents> GetDocumentList()
        {
            return this.moVehicleDocumentDC.GetDocumentList();
        }

        /// <summary>
        /// This method is used to get vehicle document details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiDocumentId"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public List<GetVehicleDocumentDetails> GetAll(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiDocumentId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            if (SortExpression == string.Empty)
                SortExpression = "StartDate desc";

            MaximumRows = StartRowIndex + Constants.I_GRID_PAGE_COUNT;
            List<GetVehicleDocumentDetails> lstGetVehicleDocumentDetails = moVehicleDocumentDC.GetAll(aiSchoolId, aiAcademicYearId, aiVehicleId, aiDocumentId, SortExpression, StartRowIndex, MaximumRows);

            if (lstGetVehicleDocumentDetails.Count > 0)
                miTotalRows = lstGetVehicleDocumentDetails[0].TotalRows;
            else
                miTotalRows = 0;

            return lstGetVehicleDocumentDetails;
        }

        /// <summary>
        /// This method is used to count rows.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiDocumentId"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <param name="MaximumRows"></param>
        /// <param name="StartRowIndex"></param>
        /// <returns></returns>
        public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiDocumentId, string SortExpression, string SortDirection, int MaximumRows, int StartRowIndex)
        {
            return miTotalRows;
        }
        
        /// <summary>
        /// This method is used to read vehicle document details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public GetVehicleDocumentDetails Get(int aiId)
        {
            return moVehicleDocumentDC.Get(aiId);
        }

         #endregion


        public bool Validate(int aiDocumentId, int aiVehicleId, string asStartDate, string asEndDate, int aiId, string asTitle, string asPolicyNo, int aiCategoryId)
        {
            return moVehicleDocumentDC.Validate(aiDocumentId, aiVehicleId, asStartDate, asEndDate, aiId, asTitle,asPolicyNo, aiCategoryId);
        }
    }
}
