using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using Utility;


namespace DataCommunicator
{
   public class StudentTransportDetailsDC
    {
       private StudentTransportDetailsStruct moStudentTransportDetailsStruct;
       public StudentTransportDetailsDC()
       {
       }
       public struct StudentTransportDetailsStruct
       {
           public int miUserId;

           public int miSchoolId;

           public int miAcademicYearId;
       }
       public virtual StudentTransportDetailsStruct StudentTransportDetailsStructDetails
       {

           get
           {
               return moStudentTransportDetailsStruct;
           }
           set
           {
               moStudentTransportDetailsStruct = value;
           }
       }
       /// <summary>
       /// This Stored Procedure is used to get all Transport details from Database.
       /// </summary>
       /// <returns></returns>
       public virtual DataSet TransportDetails()
       {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
           {
               oSQLServerDbUtility.AddParameter("UserId", moStudentTransportDetailsStruct.miUserId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("SchoolId", moStudentTransportDetailsStruct.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentTransportDetailsStruct.miAcademicYearId, SqlDbType.Int);
               return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("Transport.usp_GetStudentTransportDetails");
           }
       }
    }
}
