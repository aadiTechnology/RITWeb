// Class Name       :- SuperAdminBL
// Purpose          :- This class is used for l
// Date Of creation :- 05/12/2008
// Author Name      :- Ashish

using System;
using System.Data;
using DataCommunicator;
using System.Collections.Generic;
using SuperAdminEntities;
using Utility;

namespace BusinessLogic
{
   public class SuperAdminBL
   {
       #region Data members

       private SuperAdminDC.SuperAdminStructDetails moSuperAdminStructDetails;
       private SuperAdminDC moSuperAdminDC = new SuperAdminDC();

       #endregion
     
       #region Property
       
       public string SuperAdminPass
       {
           get { return moSuperAdminDC.SuperAdminPass; }
           set { moSuperAdminDC.SuperAdminPass = value; }           
       }

       public Int32 SuperAdminId
       {
           get { return moSuperAdminStructDetails.miSuperAdminId; }
           set { moSuperAdminStructDetails.miSuperAdminId = value; }
       }           

       public Int32 InsertedById
        {
            get { return moSuperAdminStructDetails.miInsertedById; }
            set { moSuperAdminStructDetails.miInsertedById = value; }
        }
        public System.DateTime InsertedDate
        {
            get { return moSuperAdminStructDetails.mdtInsertDate; }
            set { moSuperAdminStructDetails.mdtInsertDate = value; }
        }
        public Int32 UpdatedById
        {
            get { return moSuperAdminStructDetails.miUpdatedById; }
            set { moSuperAdminStructDetails.miUpdatedById = value; }
        }
        public System.DateTime UpdatedDate
        {
            get { return moSuperAdminStructDetails.mdtUpdateDate; }
            set { moSuperAdminStructDetails.mdtUpdateDate = value; }
        }

       #endregion

       #region Constructor

		public SuperAdminBL() {
		}
       
       #endregion
       
       public static DataTable GetCompAdmin(string asLogin, string asPassword, bool abIncludeSuperAdmin)
        {
            if (asPassword != "")
            {
                asPassword = Utility.CommonUtility.GetEncryptedPassword(asLogin, asPassword);
            }
            return SuperAdminDC.GetValidUserDetails(asLogin, asPassword, abIncludeSuperAdmin);
        }
       
       /// <summary>
       /// This method is used to get school admin user login name for proper login to the school.
       /// </summary>
       /// <param name="aiSchoolId"></param>
       /// <returns></returns>
       public static string GetSchoolAdminLoginName(int aiSchoolId)
       {
           return SuperAdminDC.GetSchoolAdminLoginName(aiSchoolId);          
       }

       public static void UpdateSuperAdminDetails(int aiUserId, string asLogin, string asPassword)
       {
           if (asPassword != "")
           {             
               asPassword = Utility.CommonUtility.GetEncryptedPassword(asLogin, asPassword);
           }
           SuperAdminDC.UpdateSuperAdminDetails(aiUserId,asPassword);
       }

       public string GetLoginName(int aiUserId)
       {
           return moSuperAdminDC.GetLoginName(aiUserId);
       }

       /// <summary>
       /// This method is used to get Admin notice to display into control panel.
       /// </summary>
       /// <param name="aiAdminUserId"></param>
       /// <param name="aiSchoolId"></param>
       /// <param name="aiAcademicYrId"></param>
       /// <returns></returns>
       public static DataSet GetAdminNoticeForControlPanel(int aiAdminUserId, int aiSchoolId, int aiAcademicYrId)
       {
           return SuperAdminDC.GetAdminNoticeForControlPanel(aiAdminUserId, aiSchoolId, aiAcademicYrId);
       }

       public static void Reset(int aiSchoolId, int aiAcademicYrId,char acResetSubjectTeacher, char acResetClassTeacher)
       {
           SuperAdminDC.Reset(aiSchoolId, aiAcademicYrId, acResetSubjectTeacher, acResetClassTeacher);
       }

       public static SuperAdminDetails GetSuperAdminSessionDetails(int aiSchoolId, string asLoginName)
       {
           return SuperAdminDC.GetSuperAdminSessionDetails(aiSchoolId, asLoginName);
       }
       public void PublishAllExams(int aiSchoolId, int aiAcademicYearId, int aiPublishById, string asReason)
       {
           moSuperAdminDC.PublishAllExams(aiSchoolId, aiAcademicYearId, aiPublishById, asReason);
       }
   }
}
