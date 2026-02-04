// Class Name       :- FeeStructureLinkBL
// Purpose          :- This class is used for manage fee structure link upload
// Date Of creation :- 13 Apr 2015
// Author Name      :- Yogesh

namespace BusinessLogic
{
    using DataCommunicator;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FeeStructureLinkBL
    {

        #region Data Member
        private FeeStructureLinkDC moFeeStructureLinkDC;
        #endregion

        #region Constructors

        public FeeStructureLinkBL()
        {
            this.moFeeStructureLinkDC = new FeeStructureLinkDC();
        }

        public FeeStructureLinkBL(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.moFeeStructureLinkDC = new FeeStructureLinkDC(aiSchoolId, aiUpdatedById, aiAcademicYearId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to insert/update news details. 
        /// </summary>		
        public void Save(string asLinkUrl)
        {
            this.moFeeStructureLinkDC.Save(asLinkUrl);
        }

         /// <summary>
        /// This method is used to Link url for Inputed filters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeName"></param>
        /// <returns></returns>
        public Dictionary<string, string> Get(int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abShowFeeStructureForNextYear)
        {
            return this.moFeeStructureLinkDC.Get(aiSchoolId, aiAcademicYearId, aiUserId, abShowFeeStructureForNextYear);
        }

        /// <summary>
        /// This method is used to delete fee structure for current year
        /// </summary>
        public void Delete()
        {
            this.moFeeStructureLinkDC.Delete();
        }

        #endregion

    }
}
