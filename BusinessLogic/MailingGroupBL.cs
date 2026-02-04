/* ---------------------------------------------------------------------------------------------------------------
 *	Filename	: MailingGroupBL.cs
 *	Author		: Pravin Shinde
 *	Date		: 25-07-2013
 *	Description	: This class is used to get the mailing group details while sending sms & messages. 
 * ---------------------------------------------------------------------------------------------------------------
 */
namespace BusinessLogic
{
    using System.Collections.Generic;
    using System.Data;
    using DataCommunicator;
    using SchoolEntities;
    /// <summary>
    /// This class is used to communicate with data access layer.
    /// </summary>
    public class MailingGroupBL
    {
        #region -- MEMBER(s) --

        private MailingGroupDC moMailingGroupDC;        

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR --

        public MailingGroupBL()
        {
            moMailingGroupDC = new MailingGroupDC();
        }

        public MailingGroupBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moMailingGroupDC = new MailingGroupDC(aiSchoolId,aiAcademicYearId,aiUserId);
        }

        #endregion -- CONSTRUCTOR --        

        #region -- PUBLIC METHODS --

        /// <summary>
        /// This method is used to get all the mailing groups esisting for current year.
        /// </summary>
        /// <param name="aiRoleId"></param>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public List<MailingGroup> GetAll(int aiRoleId, out string asRoleIds, int aiGroupId = 0)
        {
            return moMailingGroupDC.GetAll(aiRoleId,out asRoleIds, aiGroupId);
        }

        /// <summary>
        /// This method is used to delete the group.
        /// </summary>
        /// <param name="aiGroupId"></param>
        public void Delete(int aiGroupId)
        {
            moMailingGroupDC.Delete(aiGroupId);
        }

        /// <summary>
        /// This method is used to delete the perticular group from the listview.
        /// </summary>
        /// <param name="aiGroupId"></param>
        public void DeleteMailingGroupUser(int aiGroupId, int aiDeleteUserId = 0)
        {
            moMailingGroupDC.DeleteMailingGroupUser(aiGroupId, aiDeleteUserId);
        }        

        /// <summary>
        /// This procedure is used to get the group users for selected group.
        /// </summary>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public List<UserInfo> GetGroupUsers(int aiGroupId)
        {
            return moMailingGroupDC.GetGroupUsers(aiGroupId);
        }
        
        /// <summary>
        /// This method is used to insert mailing details for  the selected group.
        /// </summary>
        /// <param name="asMailingDetailsXML"></param>
        public void Insert(string asMailingGroupXML)
        {
            moMailingGroupDC.Insert(asMailingGroupXML);
        }

        /// <summary>
        /// This method is used to get the Users id's for selected group.
        /// </summary>
        /// <param name="asGroupId"></param>
        /// <returns></returns>
        public string GetMailingGroupUsers(string asGroupId, bool abIsForUsers)
        {
            return moMailingGroupDC.GetMailingGroupUsers(asGroupId, abIsForUsers);
        }

        /// <summary>
        /// This method is used to create the default groups for the school.
        /// </summary>
        public void CreateDefaultGroups()
        {
            moMailingGroupDC.CreateDefaultGroups();
        }

        #endregion -- PUBLIC METHODS --
    }
}
