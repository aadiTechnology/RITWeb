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
using Utility;
using System.Web.UI;
using System.Web;

namespace BusinessLogic
{
    public class AlumniStudentBL
    {
        #region Data Member(s)

        private AlumniStudentDC oAlumniStudentDC = new AlumniStudentDC();

        #endregion

        /// <summary>
        /// This method is used to save Alumni Student details.
        /// </summary>
        /// <param name="oVehicleMaintenanceExpenses"></param>
        public void SaveAlumniStudentDetails(string sAlumniDetails, int aiSchoolId)
        {
            oAlumniStudentDC.SaveAlumniStudentDetails(sAlumniDetails, aiSchoolId);
        }
        
        /// <summary>
        /// This method is used to Get all Alumni Student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSortExpression"></param>
        /// <returns></returns>
        public DataSet GetAllAumniStudentDetails(int aiSchoolId, String aiSortExpression)
        {
            DataSet oListAlumniDetails = new DataSet();

            if (!string.IsNullOrEmpty(aiSortExpression))
                aiSortExpression = "Order by " + aiSortExpression;
            else
                aiSortExpression = string.Empty;

            return oListAlumniDetails = oAlumniStudentDC.GetAllAumniStudentDetails(aiSchoolId, aiSortExpression);
        }

        /// <summary>
        /// This method is used to Get Alumni Student details of selected criteria to Export.
        /// </summary>
        /// <param name="aiVehicleMaintenanceExpensesID"></param>
        /// <returns></returns>
        public DataTable GetAlumniStudentDetailsToExport(int aiPassoutYear, int aiSchoolId)
        {
            DataTable oListAlumniDetails = new DataTable();
            return oListAlumniDetails = oAlumniStudentDC.GetAlumniStudentDetailsToExport(aiPassoutYear, aiSchoolId);
        }        
    }
}
