// Class Name       :- NoticeBoardBL
// Purpose          :- This class is used to manage Notice Board details.
// Date Of creation :- 21/11/2008
// Author Name      :- Ashish


using System;
using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using Utility;
namespace BusinessLogic
{
   public class NoticeBoardBL
    {
       public NoticeBoardBL()
       {
       }

       #region Data members

       private NoticeBoardDC.NoticeBoardStructDetails moNoticeBoardStructDetails;
       private NoticeBoardDC moNoticeBoardDC = new NoticeBoardDC();
       
       #endregion

       #region Property 


       public Int32 SchoolId
       {
           get { return moNoticeBoardStructDetails.miSchoolId; }
           set { moNoticeBoardStructDetails.miSchoolId = value; }
       }

       public Int32 MessageId
       {
           get { return moNoticeBoardStructDetails.miMessageId; }
           set { moNoticeBoardStructDetails.miMessageId = value; }
       }

       public Int32 AcademicYearId
       {
           get { return moNoticeBoardStructDetails.miAcademicYearId; }
           set { moNoticeBoardStructDetails.miAcademicYearId = value; }
       }

       public string NoticeMessage
       {
           get { return moNoticeBoardStructDetails.msNoticeMessage; }
           set { moNoticeBoardStructDetails.msNoticeMessage = value; }
       }       

       public System.DateTime StartDate
       {
           get { return moNoticeBoardStructDetails.mdtStartDate; }
           set { moNoticeBoardStructDetails.mdtStartDate = value; }
       }

       public System.DateTime EndDate
       {
           get { return moNoticeBoardStructDetails.mdtEndDate; }
           set { moNoticeBoardStructDetails.mdtEndDate = value; }
       }

       public Int32 InsertedById
       {
           get { return moNoticeBoardStructDetails.miInsertedById; }
           set { moNoticeBoardStructDetails.miInsertedById = value; }
       }
       public System.DateTime InsertedDate
       {
           get { return moNoticeBoardStructDetails.mdtInsertDate; }
           set { moNoticeBoardStructDetails.mdtInsertDate = value; }
       }
       public Int32 UpdatedById
       {
           get { return moNoticeBoardStructDetails.miUpdatedById; }
           set { moNoticeBoardStructDetails.miUpdatedById = value; }
       }
       public System.DateTime UpdatedDate
       {
           get { return moNoticeBoardStructDetails.mdtUpdateDate; }
           set { moNoticeBoardStructDetails.mdtUpdateDate = value; }
       }
       public List<int> SelectedRoles
       {
           get { return moNoticeBoardStructDetails.oSelectedRoles; }
           set { moNoticeBoardStructDetails.oSelectedRoles = value; }
       }

       #endregion

       
       /// <summary>
       /// This method is used to add new notice board message.
       /// </summary>
       public void AddNoticeMessage()
       {
           moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           moNoticeBoardDC.AddNoticeMessage();          
       }

       /// <summary>
       /// This method is used to update existing notice board message .
       /// </summary>
       public void UpdateNoticeMessage()
       {
           moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           moNoticeBoardDC.UpdateNoticeMessage();    
       }

       /// <summary>
       /// This method is used to delete  notice board message.
       /// </summary>
       public void DeleteNoticeMessage()
       {
           moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           moNoticeBoardDC.DeleteNoticeMessage();
       }

       /// <summary>
       /// This method is used to update default notice dates according to academic year date change.
       /// </summary>
       public void UpdateDefaultNoticeDates()
       {
           moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           moNoticeBoardDC.UpdateDefaultNoticeDates();
       }

       /// <summary>
       /// This method is used to fill all roles into checkboxList from table UserRoleMaster.
       /// </summary>
       /// <returns></returns>
       public DataTable RetriveRolesFromUserRoleMaster()
       {
           moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           return moNoticeBoardDC.RetriveRolesFromUserRoleMaster();
       }
       /// <summary>
       /// This method is used to fill selected roles into checkboxList from table NoticeBoardRoles.
       /// </summary>
       /// <param name="iMessageId"></param>
       /// <returns></returns>
       public DataTable RetrieveRolesFromNoticeBoardRoles(int iMessageId)
       {
         //  moNoticeBoardDC.NoticeBoardInfo = moNoticeBoardStructDetails;
           return moNoticeBoardDC.RetrieveRolesFromNoticeBoardRoles(iMessageId);
       }
   }
   public class NoticeBoardCollectionBL
   {
       NoticeBoardCollectionDC moNoticeBoardCollectionDC = new NoticeBoardCollectionDC();

       public NoticeBoardCollectionBL()
       {

       }
       /// <summary>
       /// This method is used to fill notice board grid.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAccYrId"></param>
       /// <param name="sortExpression"></param>
       /// <param name="maximumRows"></param>
       /// <param name="startRowIndex"></param>
       /// <returns></returns>
       public DataTable GetNoticeBoardDetails(int aiSchoolId, int aiAccYrId, String sortExp, int maximumRows, int startRowIndex)
       {
           return moNoticeBoardCollectionDC.GetNoticeBoardDetails(Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]), Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]), sortExp, Constants.I_GRID_PAGE_COUNT, startRowIndex);
       }
       /// <summary>
       /// This method is used to count total records.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAccYrId"></param>
       /// <returns></returns>
       public int CountNoticeBoardDetails(int aiSchoolId, int aiAccYrId, String sortExp)
       {
           return moNoticeBoardCollectionDC.CountNoticeBoardDetails(aiSchoolId, aiAccYrId);
       }   
   }
}
