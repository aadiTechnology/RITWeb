// -----------------------------------------------------------------------
// <copyright file="PanAttachmentBL.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

// Class Name       :- PanAttachmentBL
// Purpose          :- This class is used to manage PAN Attachment details.
// Date Of creation :- 2/7/2008
// Author Name      :- Yogesh

namespace BusinessLogic
{
    using System.Collections.Generic;
    using DataCommunicator;
    using SchoolEntities.Admin;
   
    public class PanAttachmentBL
    {
        #region MEMBER(S)
        private PanAttachmentDC moPanAttachmentDC;
        #endregion

        #region CONSTRUCTOR(S)
        public PanAttachmentBL()
        {
            moPanAttachmentDC = new PanAttachmentDC();
        }
        #endregion

        #region PUBLIC METHOD(S)

        /// <summary>
        /// This method is used to return PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="aiShowAllDetails"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asSortExpression"></param>
        /// <returns></returns>
        public static List<PANAttachmentDetails> GetAllPanAttachmentDetails(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, string asNameFilter, int aiShowAllDetails, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex, int aiCategoryId, int aiStdDivId, string asSortExpression, bool asIncludeLeftStudents)
        {
            return PanAttachmentDC.GetAllPanAttachmentDetails(aiUserRoleId, aiSchoolId, aiAcademicYearId, asNameFilter, aiShowAllDetails, asSortDirection, aiStartRowIndex, aiEndRowIndex, aiCategoryId, aiStdDivId, asSortExpression, asIncludeLeftStudents);
        }

        /// <summary>
        /// This method is used to return record count.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="aiShowAllDetails"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStdDivId"></param>
        /// <returns></returns>
        public static int GetCountAllPanAttachmentDetails(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, string asNameFilter, int aiShowAllDetails, int aiCategoryId, int aiStdDivId, bool asIncludeLeftStudents)
        {
            return PanAttachmentDC.GetCountAllPanAttachmentDetails(aiUserRoleId, aiSchoolId, aiAcademicYearId, asNameFilter, aiShowAllDetails, aiCategoryId, aiStdDivId, asIncludeLeftStudents);
        }
        
        /// <summary>
        /// This method is used to return PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiDocumentId"></param>
        /// <returns></returns>
        public PANAttachmentDetails Get(int aiUserId, int aiDocumentId)
        {
            return moPanAttachmentDC.Get(aiUserId, aiDocumentId);
        }

        /// <summary>
        /// This method is used to save PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asPANNo"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiUpdatedById"></param>
        public void Save(int aiDocumentId, int aiUserId, string asPANNo, string asNameonAadharCard, string asFileName, int aiUpdatedById)
        {
            moPanAttachmentDC.Save(aiDocumentId, aiUserId, asPANNo, asNameonAadharCard, asFileName, aiUpdatedById);
        }

        /// <summary>
        /// This method is used to delete PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUpdatedById"></param>
        public void Delete(int aiDocumentId, int aiUserId, int aiUpdatedById)
        {
            moPanAttachmentDC.Delete(aiDocumentId, aiUserId, aiUpdatedById);
        }

        #endregion
    }
}
