using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class AlumniStudentDC
    {
        /// <summary>
        /// This method is used to save Alumni Student details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="xXmlVehiclePartsUsed"></param>
        public void SaveAlumniStudentDetails(string xAlumniDetails, int aiSchoolId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AlumniDetailsXML", xAlumniDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveAlumniDetails");
            }
        }

        /// <summary>
        /// This method is used to Get all Alumni Student details.
        /// </summary>
        /// <param name="aiVehicleMaintenanceExpensesID"></param>
        /// <returns></returns>
        public DataSet GetAllAumniStudentDetails(int aiSchoolId, String aiSortExpression)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", aiSortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllAumniStudentDetails");
            }
        }

        /// <summary>
        /// This method is used to Get Alumni Student details of selected criteria to Export.
        /// </summary>
        /// <param name="aiPassoutYear"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAlumniStudentDetailsToExport(int aiPassoutYear, int aiSchoolId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("PassoutYear", aiPassoutYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAlumniStudentDetailsToExport");
            }
        }
    }
}
