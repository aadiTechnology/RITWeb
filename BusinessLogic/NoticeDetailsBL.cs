/* File Name = NoticeDetailsBL
 * Created Date - 27 Dec 2011
 * Created by - Poonam
 * Class Description - This class is defined to manage Notice Details.*/

using System;
using System.Collections.Generic;
using SchoolEntities;
using DataCommunicator;
using Utility;
using System.Data;
using System.Collections;
namespace BusinessLogic
{
    public class NoticeDetailsBL
    {
        #region "Data Members"

        NoticeDetailsDC moNoticeDetailsDC;

        #endregion "Data Members"

        #region "Constructors"

        public NoticeDetailsBL()
        {
            moNoticeDetailsDC = new NoticeDetailsDC();
        }

        public NoticeDetailsBL(int aiNoticeDetailsId)
        {
            moNoticeDetailsDC = new NoticeDetailsDC(aiNoticeDetailsId);
        }

        #endregion "Constructors"

        #region "Properties"

        public NoticeDetails NoticeDetails
        {
            get { return moNoticeDetailsDC.moNoticeDetails; }
            set { moNoticeDetailsDC.moNoticeDetails = value; }
        }

        public List<NoticeDetails> lstNoticeDetails
        {
            get { return moNoticeDetailsDC.lstNoticeDetails; }
            set { moNoticeDetailsDC.lstNoticeDetails = value; }
        }

        public List<Event> lstEventDetails
        {
            get{return moNoticeDetailsDC.EventDetails;}
            set{moNoticeDetailsDC.EventDetails=value;}
        }
        
        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method returns Notice ID for supplied notice Name
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asNoticeName"></param> 
        public static int GetIDByName(int aiSchoolId, string asNoticeName, string asStartDateTime, string asEndDateTime)
        {
            return NoticeDetailsDC.GetIDByName(aiSchoolId, asNoticeName, asStartDateTime, asEndDateTime);
        }

        /// <summary>
        /// This method is used to Update Notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiNoticeId"></param>
        /// <param name="asNoticeName"></param>
        /// <param name="asDisplayLocation"></param>
        /// <param name="asStartDate"></param>
        /// <param name="asEndDate"></param>
        /// <param name="aiSortOrder"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiSortOrderLocationChanged"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static void Update(string sXml, string asUserRoleIds, string asClassIds)
        {
            NoticeDetailsDC.Update(sXml, asUserRoleIds, asClassIds);
        }

        public DataTable GetUserRolesForSelectedNoticeId(int aiNoticeId)
        {
            return moNoticeDetailsDC.GetUserRolesForSelectedNoticeId(aiNoticeId);
        }

        public DataTable GetStandardDivisionsForSelectedNotice(int aiNoticeId)
        {
            return moNoticeDetailsDC.GetStandardDivisionsForSelectedNotice(aiNoticeId);
        }

        /// <summary>
        /// This method is used to Delete Notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiNoticeId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static void Delete(int aiSchoolId, int aiNoticeId, int aiUserId)
        {
            NoticeDetailsDC.Delete(aiSchoolId, aiNoticeId, aiUserId);
        }

      

        /// <summary>
        /// This method is used to get details of all Notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDisplayLocation"></param>
        /// <param name="aiShowAllNotices"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiMaximumRows"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <returns></returns>
        public List<NoticeDetails> GetAll(int aiSchoolId, string asDisplayLocation, bool abShowAllNotices, bool abText, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex)
        {
            int iStartRowIndex = StartRowIndex;
            if (StartRowIndex != 0)
                iStartRowIndex = StartRowIndex + 1;
            int iEndRowIndex = iStartRowIndex + MaximumRows;
            int iShowAllNotices = abShowAllNotices ? 1 : 0;
            if (asSortExpression == "" || asSortExpression == null)
            {
                asSortExpression = "StartDate";
                if (asSortDirection == "" || asSortDirection == null)
                    asSortDirection = Constants.S_DESCENDING;
            }
            asSortExpression = asSortExpression + " " + asSortDirection;
            moNoticeDetailsDC.GetAll(aiSchoolId, asDisplayLocation, iShowAllNotices,abText, asSortExpression, iEndRowIndex, iStartRowIndex);
             return moNoticeDetailsDC.lstNoticeDetails;
        }

        /// <summary>
        /// This method is used to get all Notices for external site.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public List<NoticeDetails> GetAllNoticeDetails(int aiSchoolId)
        {
            return this.moNoticeDetailsDC.GetAllNoticeDetails(aiSchoolId);            
        }

        /// <summary>
        /// This method is used to get Notices for display on Home page and Control Pannel.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDisplayLocation"></param>
        /// <param name="aiShowAllNotices"></param>
        /// <returns></returns>
        public List<NoticeDetails> GetNotices(int aiSchoolId, string asDisplayLocation, bool abShowAllNotices, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex, int aiUserRoleId)
        {

            if (!string.IsNullOrEmpty(asSortExpression))
                asSortExpression = "Order by " + asSortExpression + " " + asSortDirection;
            else
                asSortExpression = string.Empty;
            int iShowAllNotices = abShowAllNotices ? 1 : 0;
            int iEndRowIndex = StartRowIndex + MaximumRows;
            moNoticeDetailsDC.GetNotices(aiSchoolId, asDisplayLocation, iShowAllNotices, asSortExpression, iEndRowIndex, StartRowIndex, aiUserRoleId);
            return moNoticeDetailsDC.lstNoticeDetails;           
        }

        public int GetStandarDivisionId(int aiSchoolId, int aiUserId)
        {
            return moNoticeDetailsDC.GetStandarDivisionId(aiSchoolId, aiUserId);            
        }


        public int GetNoticesCount(int aiSchoolId, string asDisplayLocation, bool abShowAllNotices, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex, int aiUserRoleId)
        {
            int iShowAllNotices = abShowAllNotices ? 1 : 0;
            return moNoticeDetailsDC.GetNoticesCount(aiSchoolId, asDisplayLocation, iShowAllNotices, aiUserRoleId);
        }

        /// <summary>
        /// This method is used to get Count of all Notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asDisplayLocation"></param>
        /// <param name="aiShowAllNotices"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public int GetCount(int aiSchoolId, string asDisplayLocation, bool abShowAllNotices,bool abText, string asSortExpression, string asSortDirection, int MaximumRows, int StartRowIndex)
        {
            int iShowAllNotices = abShowAllNotices ? 1 : 0;
            return moNoticeDetailsDC.GetCount(aiSchoolId, asDisplayLocation, iShowAllNotices, abText);
        }
        public static void SaveSelectedNotices(string asXML)
        {
            NoticeDetailsDC.SaveSelectedNotices(asXML);
        }

        public static DateTime GetMaxEndDate()
        {
            return NoticeDetailsDC.GetMaxEndDate();
        }

        public bool CompareDate(int aiSchoolId,DateTime adtCompareDate)
        {
            NoticeDetailsDC oNoticeDetailsDC = new NoticeDetailsDC();
            return oNoticeDetailsDC.CompareDate(aiSchoolId,adtCompareDate);
        }

        /// <summary>
        /// THis method is used to return top notices.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTopCount"></param>
        /// <returns></returns>
        public List<NoticeDetails> GetAllTopNotices(int aiSchoolId, int aiTopCount)
        {
            return moNoticeDetailsDC.GetAllTopNotices(aiSchoolId, aiTopCount);
        }

        /// <summary>
        /// THis method is used to delete 
        /// </summary>
        /// <param name="aiNoticeId"></param>
        /// <param name="bIsText"></param>
        public void DeleteNoticeImage(int aiNoticeId, int bIsText)
        {
            NoticeDetailsDC oNoticeDetailsDC = new NoticeDetailsDC();
            oNoticeDetailsDC.DeleteNoticeImage(aiNoticeId, bIsText);
        }        

        #endregion "Public Methods"
    }
}
