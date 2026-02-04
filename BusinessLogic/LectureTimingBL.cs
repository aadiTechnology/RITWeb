// Class Name       :- LectureTimingBL
// Purpose          :- This class is used to manage Lecture Timings details.
// Date Of creation :- 29/11/2008
// Author Name      :- Ashish


using System;
using System.Data;
using DataCommunicator;
namespace BusinessLogic
{
   public class LectureTimingBL
   {
       #region Constructors
      
       public LectureTimingBL()
       {
       }
       public LectureTimingBL(int iLectureTimingDetailId)
       {
           moLectureTimingDC =new LectureTimingDC(iLectureTimingDetailId);
           moLectureTimingStructDetails = moLectureTimingDC.LectureTimingInfo;
       }
       #endregion

       #region Data members

       private LectureTimingDC.LectureTimingStructDetails moLectureTimingStructDetails;
       private LectureTimingDC moLectureTimingDC = new LectureTimingDC();
      
       #endregion

       #region Property


       public Int32 SchoolId
       {
           get { return moLectureTimingStructDetails.miSchoolId; }
           set { moLectureTimingStructDetails.miSchoolId = value; }
       }

       public Int32 AcademicYearId
       {
           get { return moLectureTimingStructDetails.miAcademicYearId; }
           set { moLectureTimingStructDetails.miAcademicYearId = value; }
       }

       public Int32 LectureTimingDetailsId
       {
           get { return moLectureTimingStructDetails.miLectureTimingDetailsId; }
           set { moLectureTimingStructDetails.miLectureTimingDetailsId = value; }
       }

       public Int32 LectureTimingId
       {
           get { return moLectureTimingStructDetails.miLectureTimingId; }
           set { moLectureTimingStructDetails.miLectureTimingId = value; }
       }

       public Int32 Section
       {
           get { return moLectureTimingStructDetails.miSectionId; }
           set { moLectureTimingStructDetails.miSectionId = value; }
       }
      
       public int LectureNumber
       {
           get { return moLectureTimingStructDetails.miLectureNo; }
           set { moLectureTimingStructDetails.miLectureNo = value; }
       }

       public System.DateTime StartTime
       {
           get { return moLectureTimingStructDetails.mdtStartTime; }
           set { moLectureTimingStructDetails.mdtStartTime = value; }
       }

       public System.DateTime EndTime
       {
           get { return moLectureTimingStructDetails.mdtEndTime; }
           set { moLectureTimingStructDetails.mdtEndTime = value; }
       }

       public string Description
       {
           get { return moLectureTimingStructDetails.msDescription; }
           set { moLectureTimingStructDetails.msDescription = value; }
       }

       public Int32 InsertedById
       {
           get { return moLectureTimingStructDetails.miInsertedById; }
           set { moLectureTimingStructDetails.miInsertedById = value; }
       }
       public System.DateTime InsertedDate
       {
           get { return moLectureTimingStructDetails.mdtInsertDate; }
           set { moLectureTimingStructDetails.mdtInsertDate = value; }
       }
       public Int32 UpdatedById
       {
           get { return moLectureTimingStructDetails.miUpdatedById; }
           set { moLectureTimingStructDetails.miUpdatedById = value; }
       }
       public System.DateTime UpdatedDate
       {
           get { return moLectureTimingStructDetails.mdtUpdateDate; }
           set { moLectureTimingStructDetails.mdtUpdateDate = value; }
       }
       #endregion

       #region Public Methods
       
       /// <summary>
       /// This methos is used to retrive lecture timings data table.
       /// </summary>
       /// <returns></returns>
       public DataTable RetrieveLectureTimingDetails() 
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           return moLectureTimingDC.RetrieveLectureTimingDetails();
       }

       /// <summary>
       /// This method is used to retrive lecture number using user define function.
       /// </summary>
       /// <returns></returns>
       public int RetrieveLectureNumber()
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           return moLectureTimingDC.RetrieveLectureNumber();
       }

       /// <summary>
       /// This method is used to get section and standard name as per school id.
       /// </summary>
       /// <returns></returns>
       public DataTable GetSectionAndStandardName()
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           return moLectureTimingDC.GetSectionAndStandardName();
       }

       /// <summary>
       /// This method is used to add lecture timing details to the database.
       /// </summary>
       public void AddLectureTimingDetails()
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           moLectureTimingDC.AddLectureTimingDetails();
       }

       /// <summary>
       /// This method is used to update lecture timing details into database.
       /// </summary>
       public void UpdateLectureTimingDetails()
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           moLectureTimingDC.UpdateLectureTimingDetails();
       }

       /// <summary>
       /// This method is used to delete lecture timing from the database table.
       /// </summary>
       public void DeleteLectureTiming(Char sIsLastRecord)
       {
           moLectureTimingDC.LectureTimingInfo = moLectureTimingStructDetails;
           moLectureTimingDC.DeleteLectureTiming(sIsLastRecord);
       }
       
       #endregion
   }
}
