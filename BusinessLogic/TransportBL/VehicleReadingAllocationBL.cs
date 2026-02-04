using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using SchoolEntities.Transport;
using DataCommunicator.TransportDC;
using System.Data;
using Utility;




namespace BusinessLogic.TransportBL
{
  public class VehicleReadingAllocationBL
    {

      VehicleReadingAllocationDC moVehicleReadingAllocationDC = new VehicleReadingAllocationDC();

      /// <summary>
      /// this method is used to save transport reading allocation details.
      /// </summary>
      /// <param name="asXml"></param>

      public static void SaveTransportReadingAllocationDetails(string asXml)
      {
          VehicleReadingAllocationDC.SaveTransportReadingAllocationDetails(asXml);
      }

      /// <summary>
      /// this method is used to get all vehicle number
      /// </summary>
      /// <returns></returns>
      public List<TransportReadingAllocationDetails> GetAllVehicleNumbers(int aiAcademicYearId)
      {
          return moVehicleReadingAllocationDC.GetAllVehicleNumbers(aiAcademicYearId);
      }

      /// <summary>
      /// this method is used to get vehicle reading allocation details.
      /// </summary>
      /// <param name="aiSchoolId"></param>
      /// <param name="aiAcademicYearId"></param>
      /// <param name="sortExpression"></param>
      /// <param name="maximumRows"></param>
      /// <param name="startRowIndex"></param>
      /// <param name="asFilter"></param>
      /// <returns></returns>
      public DataSet GetAllVehicleReadingAllocationDetails(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asFilter, string asFilterDate, string asIncludeAllDates)
      {
          if (asIncludeAllDates == Constants.S_ONE)
              asFilterDate = null;         

          DataSet oDS = moVehicleReadingAllocationDC.GetAllVehicleReadingAllocationDetails(aiSchoolId, aiAcademicYearId,asSortExpression, asFilter, asFilterDate);
          return oDS;
      }

      /// <summary>
      /// this method is used to delete vehicle reading allocation details.
      /// </summary>
      /// <param name="aiSchoolId"></param>
      /// <param name="aiNoticeId"></param>
      /// <param name="aiUserId"></param>

      public static void DeleteVehicleAllocationDetails(int aiVehicleReadingAllocationId, int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
      {
          VehicleReadingAllocationDC.DeleteVehicleAllocationDetails(aiVehicleReadingAllocationId, aiSchoolId, aiAcademicYearId, aiUpdatedById);
      }

      public void InsertAllocationDetails(string asAllocationDetails,int aiSchoolId, int aiAcademicYearId, int aiUserId)
      {
          moVehicleReadingAllocationDC.InsertAllocationDetails(asAllocationDetails, aiSchoolId, aiAcademicYearId, aiUserId);
      }

      public void InsertMaintenanceDetails(string asMaintenanceDetails, int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
      {
          moVehicleReadingAllocationDC.InsertMaintenanceDetails(asMaintenanceDetails, aiSchoolId, aiAcademicYearId, aiInsertedById);
      }

      public List<string> GetVehicleNumbers(int aiSchoolId, int aiAcademicYearId)
      {
          return moVehicleReadingAllocationDC.GetVehicleNumbers(aiSchoolId, aiAcademicYearId);
      }


    }
}
