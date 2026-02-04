// Class Name       :- SupportBL
// Purpose          :- This class is used to manage Support Details.
// Date Of creation :- 10/21/2013
// Author Name      :- Ashish

namespace BusinessLogic
{
    using System.Collections.Generic;
    using DataCommunicator;
    using SchoolEntities;

    /// <summary>
    /// TODO: This calll use to communicate with DC and give data to user interface
    /// </summary>
    public class SupportBL
    {
        #region -- MEMBER(s) --
        
        private SupportDC moSupportDC=null;

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR(s) --

        public SupportBL(int aiSchoolId, int aiAcademicYearId)
        { 
            this.moSupportDC=new SupportDC(aiSchoolId,aiAcademicYearId);
        }

        #endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --
        
        /// <summary>
        /// This method use to get all support details send via mail
        /// </summary>
        /// <returns></returns>
        public List<SupportDetails> GetAll()
        {
            return this.moSupportDC.GetAll();
        }
        
       /// <summary>
       /// This method use to save support details of user
       /// </summary>
       /// <param name="aoSupportDetails"></param>
        public void Save(SupportDetails aoSupportDetails)
        {
            this.moSupportDC.Save(aoSupportDetails);
        }
        
        /// <summary>
        /// This method use to get support details for selected support id
        /// </summary>
        /// <param name="aiSupportId"></param>
        /// <returns></returns>
        public SupportDetails Get(int aiSupportId)
        {
           return this.moSupportDC.Get(aiSupportId);
        }

        /// <summary>
        /// This method use to get Name with Enrolment Number or Designation for User.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public string GetStudentDetails(int aiUserId, int aiUserRoleId)
        {
            return this.moSupportDC.GetStudentDetails(aiUserId, aiUserRoleId);
        }
        
        #endregion -- PUBLIC METHOD(s) --
    }
}
