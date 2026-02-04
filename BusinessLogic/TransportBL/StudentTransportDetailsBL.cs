using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using DataCommunicator;
using Utility;


namespace BusinessLogic
{
   public class StudentTransportDetailsBL
    {
       private StudentTransportDetailsDC.StudentTransportDetailsStruct moStudentTransportDetailsStruct;
       private StudentTransportDetailsDC moStudentTransportDetailsDC;
       public StudentTransportDetailsBL()
       {
           moStudentTransportDetailsDC = new StudentTransportDetailsDC();
       }
       public virtual int UserId
       {
           get
           {
               return moStudentTransportDetailsStruct.miUserId;
           }
           set
           {
               moStudentTransportDetailsStruct.miUserId = value;
           }
       }
       public virtual int SchoolId
       {
           get
           {
               return moStudentTransportDetailsStruct.miSchoolId;
           }
           set
           {
               moStudentTransportDetailsStruct.miSchoolId = value;
           }
       }
       public virtual int AcademicYearId
       {
           get
           {
               return moStudentTransportDetailsStruct.miAcademicYearId;
           }
           set
           {
               moStudentTransportDetailsStruct.miAcademicYearId = value;
           }
       }
       /// <summary>
       /// This Function is used to get all Transport details from Database.
       /// </summary>
       /// <returns></returns>
       public virtual DataSet TransportDetails()
       {
           moStudentTransportDetailsDC.StudentTransportDetailsStructDetails = moStudentTransportDetailsStruct;
           return moStudentTransportDetailsDC.TransportDetails();
       }
    }
}
