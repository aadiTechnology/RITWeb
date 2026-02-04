using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using SchoolEntities.Transport;

namespace BusinessLogic
{
    public class VehicleMaintenanceExpensesBL
    {
        #region Data Member(s)

        private VehicleMaintenanceExpensesDC oVehicleMaintenanceDC = new VehicleMaintenanceExpensesDC();

        #endregion

        /// <summary>
        /// This method is used to Get Vehicle Numbers details.
        /// </summary>
        /// <returns></returns>
        public static List<VehicleMaintenanceExpenses> GetVehicleNumbers(int aiAcademicYearId)
        {
            List<VehicleMaintenanceExpenses> lstVehicleMaintenanceExpenses = new List<VehicleMaintenanceExpenses>();
            return lstVehicleMaintenanceExpenses = VehicleMaintenanceExpensesDC.GetVehicleNumbers(aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to Get Vehicle Maintenance Expenses details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSortExpression"></param>
        /// <returns></returns>
        public DataSet GetAllVehicleExpensesDetails(int aiSchoolId, int aiAcademicYearId, string asFilter, int aiMaintenanceTypeId, string asStartDate, string asEndDate, String aiSortExpression)
        {
            if (!string.IsNullOrEmpty(aiSortExpression))
                aiSortExpression = "Order by " + aiSortExpression;
            else
                aiSortExpression = string.Empty;

            if(asStartDate == null)
                asStartDate = string.Empty;

            if(asEndDate == null)
                asEndDate = string.Empty;

            string sMainFilter="", sDateFilter="";

            if (asStartDate != string.Empty && asEndDate != string.Empty)
                sDateFilter = " AND CONVERT(DATE,MaintenanceDate) >= '" + asStartDate + "' AND CONVERT(DATE,MaintenanceDate) <= '" + asEndDate + "'";
            else if (asStartDate != string.Empty && asEndDate == string.Empty)
                sDateFilter = " AND CONVERT(DATE,MaintenanceDate) >= '" + asStartDate + "'";
            else if (asStartDate == string.Empty && asEndDate != string.Empty)
                sDateFilter = " AND CONVERT(DATE,MaintenanceDate) <= '" + asEndDate + "'";

            sMainFilter = " AND (VehicleNumber like '%" + asFilter + "%' OR WorkshopName like '%" + asFilter + "%') and (VME.MaintenanceTypeId = " + @aiMaintenanceTypeId + " OR " + @aiMaintenanceTypeId + "=0)" + sDateFilter;

            DataSet oListVehicleDetails = new DataSet();
            oListVehicleDetails = oVehicleMaintenanceDC.GetAllVehicleExpensesDetails(aiSchoolId, aiAcademicYearId, sMainFilter, aiSortExpression);

            return oListVehicleDetails;
        }

        /// <summary>
        /// This method is used to Get Vehicle Maintenance Expenses Parts Used details of the selected Vehicle Maintenance Expense.
        /// </summary>
        /// <param name="aiVehicleMaintenanceExpensesID"></param>
        /// <returns></returns>
        public DataSet GetAllVehicleExpensesPartsUsedDetails(int aiVehicleMaintenanceExpensesID)
        {
            DataSet oListVehicleDetails = new DataSet();
            oListVehicleDetails = oVehicleMaintenanceDC.GetAllVehicleExpensesPartsUsedDetails(aiVehicleMaintenanceExpensesID);

            return oListVehicleDetails;
        }        

        /// <summary>
        /// This method is used to save and update Vehicle Maintenance Expenses details and Vehicle Maintenance Expenses Parts Used detail.
        /// </summary>
        /// <param name="oVehicleMaintenanceExpenses"></param>
        public static void SaveUpdateVehicleMaintenanceExpenses(string sXml, string xXmlVehiclePartsUsed)
        {
            VehicleMaintenanceExpensesDC.SaveUpdateVehicleMaintenanceExpenses(sXml, xXmlVehiclePartsUsed);
        }

        /// <summary>
        /// This method is used to delete Vehicle Maintenance Expenses details and Vehicle Maintenance Expenses Parts Used details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiVehicleMaintenanceExpensesId"></param>
        /// <param name="aiUserId"></param>
        public static void Delete(int aiSchoolId, int aiVehicleMaintenanceExpensesId, int aiUserId, int aiAcademicYearId)
        {
            VehicleMaintenanceExpensesDC.Delete(aiSchoolId, aiVehicleMaintenanceExpensesId, aiUserId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get Maintenance type list.
        /// </summary>
        /// <returns></returns>
        public List<Maintanance> GetMaintenanceTypeList()
        {
            return this.oVehicleMaintenanceDC.GetMaintenanceTypeList();
        }
    }
}
