// Class Name       :- StudentLCUploadBL
// Purpose          :- This class is used to manage StudentLCUpload details.
// Date Of creation :- 28/3/2019
// Author Name      :- Sachin wagh

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
using LCUploadEntities;


namespace BusinessLogic
{
    public class StudentLCUploadBL
    {
        #region "Data Members"

        private StudentLCUploadDC moStudentLCUploadDC = null;        
        private int miStudentCount;
        private int miStudentLCDownloadCount;

        #endregion "Data Members"

        #region "Constructors"

        public StudentLCUploadBL()
        {
            moStudentLCUploadDC = new StudentLCUploadDC();         
        }

        public StudentLCUploadBL(int aiSchoolId, int aiAcademicYearId, int AiUpdatedById)
        {
            moStudentLCUploadDC = new StudentLCUploadDC(aiSchoolId, aiAcademicYearId, AiUpdatedById);            
        }

        #endregion "Constructors"


        #region "Public Methods"

        /// <summary>
        /// This methos is used to get the Student LC Upload details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        /// <param name="abChkUserWithPhotoFlag"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<StudentLCDetails> GetStudentLCUpload(int aiSchoolId, int aiAcademicYearId, string asUserName, bool abChkUserWithLCFlag, int aiStandardId, int aiDivisionId, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;            
            List<StudentLCDetails> lstStudentLCDetails = moStudentLCUploadDC.GetStudentLCUpload(aiSchoolId, aiAcademicYearId, asUserName, abChkUserWithLCFlag, aiStandardId, aiDivisionId, iEndIndex, iStartIndex);
            if (lstStudentLCDetails.Count > 0)
                miStudentCount = lstStudentLCDetails[0].TotalRows;
            return lstStudentLCDetails;
        }

        /// <summary>
        /// This methos is used to get the Student LC Upload Count detail.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        /// <param name="abChkUserWithPhotoFlag"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public int CountLCUplaod(int aiSchoolId, int aiAcademicYearId, string asUserName, bool abChkUserWithLCFlag, int aiStandardId, int aiDivisionId)
        {
            return miStudentCount;
        }

        /// <summary>
        /// This methos is used to get the Student LC Download details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<StudentLCDetails> GetStudentLCDownload(int aiSchoolId, string asSortExpression, string asSortDirection, string asFilter, int maximumRows, int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;           

            if (asFilter == null)
                asFilter = string.Empty;

            if (string.IsNullOrEmpty(asSortExpression))
            {
                asSortExpression = "SchoolLeft_Date";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }

            asSortExpression = asSortExpression + " " + asSortDirection;

            List<StudentLCDetails> lstStudentLCDetails = moStudentLCUploadDC.GetStudentLCDownload(aiSchoolId, iEndIndex, iStartIndex, asFilter,asSortExpression);
            if (lstStudentLCDetails.Count > 0)
                miStudentLCDownloadCount = lstStudentLCDetails[0].TotalRows;
            return lstStudentLCDetails;
        }

        public int CountLCDownload(int aiSchoolId, string asSortExpression, string asSortDirection, string asFilter)
        {
            return miStudentLCDownloadCount;
        }
   
        /// <summary>
        /// This method is used to add student into array for upload LC
        /// </summary>
        /// <param name="oStudent"></param>
        public void UploadStudentLC(string asXml)
        {
            moStudentLCUploadDC.UploadStudentLC(asXml);
        }

        public void DeleteLCFiles(string asIds)
        {
            moStudentLCUploadDC.DeleteLCFiles(asIds);
        }

        #endregion "Public Methods"
    }
}
