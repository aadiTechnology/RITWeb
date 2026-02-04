using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using PhotoUploadEntities;
using System.Data;
namespace BusinessLogic
{
   public class UploadUserDocumentBL
   {
       #region "Data Members"

       private UploadUserDocumentDC moUploadUserDocumentDC = null;
       private int miUserCount;

       #endregion

       #region "Constructors"

       public UploadUserDocumentBL()
       {
           moUploadUserDocumentDC = new UploadUserDocumentDC();
       }

       public UploadUserDocumentBL(int aiSchoolId, int aiAcademicYearId)
       {
           moUploadUserDocumentDC = new UploadUserDocumentDC(aiSchoolId, aiAcademicYearId);
       }       

       #endregion

       #region "Public Methods"

       /// <summary>
       /// This methos is used to get the User Rolewise Photo Upload details.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiDocumentTypeId"></param>
       /// <param name="aiUserRoleId"></param>
       /// <param name="asUserName"></param>       
       /// <param name="maximumRows"></param>
       /// <param name="startRowIndex"></param>       
       /// <returns></returns>
       public List<UserRolewiseDocumentDetails> GetUserDetailsForDocumentUpload(int aiSchoolId, int aiAcademicYearId, int aiDocumentTypeId, int aiUserRoleId, string asUserName, 
           string sortExpression, string sortDirection, int maximumRows, int startRowIndex, int aiUser,bool asLeftStudent, int aiStandardDivisionId)  //
       {
           int iStartIndex = startRowIndex;
           int iEndIndex = iStartIndex + maximumRows;
           int iUserCount = 0;
           List<UserRolewiseDocumentDetails> lstUserDocumentDetails = moUploadUserDocumentDC.GetUserDetailsForDocumentUpload(aiSchoolId, aiAcademicYearId, aiDocumentTypeId, aiUserRoleId, asUserName, iEndIndex, iStartIndex, out iUserCount, aiUser,asLeftStudent, aiStandardDivisionId);
           miUserCount = iUserCount;
           return lstUserDocumentDetails;
       }

       /// <summary>
       /// This method is used to count no. of users to upload photo.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYearId"></param>
       /// <param name="aiDocumentTypeId"></param>
       /// <param name="aiUserRoleId"></param>
       /// <param name="asUserName"></param>      
       /// <returns></returns>
       public int CountUserForDocumentUplaod(int aiSchoolId, int aiAcademicYearId, int aiDocumentTypeId, int aiUserRoleId, string asUserName, string sortExpression, string sortDirection,int maximumRows, int startRowIndex, int aiUser,bool asLeftStudent, int aiStandardDivisionId)
       {   
           return miUserCount;
       }

       /// <summary>
       /// This method is used to save the details in to DB.
       /// </summary>
       /// <param name="aiUserRoleId"></param>
       /// <param name="asUserDocumentDetails"></param>
       /// <param name="aiInsertedById"></param>
       public void Save(int aiDocumentTypeId ,int aiUserRoleId, string asUserDocumentDetails, int aiInsertedById)
       {
           moUploadUserDocumentDC.Save(aiDocumentTypeId, aiUserRoleId, asUserDocumentDetails, aiInsertedById);
       }
       
       /// <summary>
       /// This method is used to delete the user document details.
       /// </summary>
       /// <param name="aiDocumentId"></param>
       /// <param name="aiDocumentTypeId"></param>
       /// <param name="aiUserId"></param>
       /// <param name="aiUpdatedById"></param>
       public void Delete(int aiDocumentId, int aiDocumentTypeId, int aiUserId, int aiUpdatedById)
       {
           moUploadUserDocumentDC.Delete(aiDocumentId, aiDocumentTypeId, aiUserId, aiUpdatedById);
       }
       

       /// <summary>
       /// This method is used to get user document details.
       /// </summary>
       /// <param name="aiUserId"></param>
       /// <param name="aiFinancialYearId"></param>
       /// <returns></returns>
       public List<UserRolewiseDocumentDetails> GetUserDocumentDetails(int aiUserId, int aiFinancialYearId)
       {
           return moUploadUserDocumentDC.GetUserDocumentDetails(aiUserId, aiFinancialYearId);
       }
      
      
       

      
      

       /// <summary>
       /// This method is used to get users .
       /// </summary>
       /// <returns></returns>
       public DataTable GetUsers(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStdDivId=0)  ////
       {
           return moUploadUserDocumentDC.GetUsers(aiSchoolId, aiAcademicYearId, aiUserId, aiStdDivId);
       }

       /// <summary>
       /// This method is used to get document types .
       /// </summary>
       /// <returns></returns>
       public DataTable GetDocumentTypes(int aiSchoolId)//
       {
           return moUploadUserDocumentDC.GetDocumentTypes(aiSchoolId);
       }
       public DataTable GetUserWisePanNo(int aiSchoolId,int aiAcademicYearId,string aiUserId)//
       {
           return moUploadUserDocumentDC.GetUserWisePanNo(aiSchoolId, aiAcademicYearId, aiUserId);
       }

       public DataTable GetStudentLCFileName(int aischoolId, int aiUserId)
       {
           return moUploadUserDocumentDC.GetStudentLCFileName(aischoolId, aiUserId);
       }
       #endregion
   }
}
