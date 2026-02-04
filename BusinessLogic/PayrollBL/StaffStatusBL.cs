/* 
   Class Name       :- StaffStatusBL
   Created By       :- Vinod  
   Created Date     :- 12-Sept-2011
   Class Description:- This class is used to manage staff status details.
*/
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
using PayrollEntities;

namespace BusinessLogic
{
    public class StaffStatusBL
    {
        #region Members
               
        public StaffStatusDC moStaffStatusDC;

        #endregion

        #region Constructors

        public StaffStatusBL() 
        { 
            moStaffStatusDC = new StaffStatusDC(); 
        }

        public StaffStatusBL(int aiSchoolId, int aiAcademicYrId)
        {
            moStaffStatusDC = new StaffStatusDC(aiSchoolId, aiAcademicYrId);
        }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// This method used to get Staff Stats type details.
        /// </summary>
        /// <returns></returns>
        public List<StaffStatusDetails> GetStaffStatusTypes()
        {
            return moStaffStatusDC.GetStaffStatusTypes();
        }

        /// <summary>
        /// This method is used to get paged staff status details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        ///// 
        public List<StaffStatusDetails> GetStaffStatusDetails(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiUserRoleId, string asStatusType, string asFilter, bool asLocked)  //asLocked filter added
        {   
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return moStaffStatusDC.GetStaffStatusDetails(aiSchoolId, aiAcademicYearId, sortExpression, iEndIndex, iStartIndex, aiUserRoleId, asStatusType, asFilter, asLocked);
            
        }


        /// <summary>
        /// This method is used to get count of total library vendor records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int CountTotalStaffStatusDetails(int aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiUserRoleId, string asStatusType, string asFilter, bool asLocked) //asLocked filter added
        {
            return StaffStatusDC.CountTotalStaffStatusDetails(aiSchoolId, aiAcademicYearId, maximumRows, startRowIndex, aiUserRoleId, asStatusType, asFilter, asLocked);
        }

        /// <summary>
        /// This method used to save and update Staff status details.
        /// </summary>
        /// <returns></returns>
        public void SaveStaffStatusDetails(string asStaffStatusDetailsXML, int aiInsertedById)
        {
            moStaffStatusDC.SaveStaffStatusDetails(asStaffStatusDetailsXML, aiInsertedById);
        }

        /// <summary>
        /// This method used to get Staff working status details.
        /// </summary>
        /// <returns></returns>
        public List<StaffWorkingStatus> GetStaffWorkingStatus()
        {
            return moStaffStatusDC.GetStaffWorkingStatus();
        }
        
        #endregion
    }
}
