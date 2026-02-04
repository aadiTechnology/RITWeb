/* File Name = NoticeDetailsDC
 * Created Date - 27 Dec 2011
 * Created by - Poonam
 * Class Description - This class is defined to manage Notice Details.*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Collections;

namespace DataCommunicator
{
    public class NoticeDetailsDC
    {
        #region "Data Members"

        public NoticeDetails moNoticeDetails;
        public List<NoticeDetails> lstNoticeDetails = new List<NoticeDetails>();
        public Event oEvent;
        private List<Event> lstEventDetails = new List<Event>();
        #endregion "Data Members"

        #region "Constructors"

        public NoticeDetailsDC()
        {
            moNoticeDetails = new NoticeDetails();
        }

        public NoticeDetailsDC(int aiNoticeDetailsId)
        {
            moNoticeDetails = new NoticeDetails();
            Load(aiNoticeDetailsId);
        }

        #endregion "Constructors"

        #region "Properties"

        public List<Event> EventDetails
        {
            get { return lstEventDetails; }
            set { lstEventDetails = value; }
        }
        #endregion "Properties"
        #region "Public Methods"


        /// <summary>
        /// This method is used to ADD / Update Notice Details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeId"></param>
        ///  <param name="asNoticeName"></param>
        ///  <param name="asDisplayLocation"></param>
        ///  <param name="asStartDate"></param>
        ///  <param name="asEndDate"></param>
        ///  <param name="aiSortOrder"></param>
        ///  <param name="asFileName"></param>
        ///  <param name="aiSortOrderLocationChanged"></param>
        ///  <param name="aiUserId"></param>
        public static void Update(string asXml, string asUserRoleIds, string asClassIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("noticeXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("UserRoleIds", asUserRoleIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ClassIds", asClassIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Usp_AddNoticeDetails");

            }
        }


        /// <summary>
        /// This method is used to retrieve User Role Ids for selected menu.
        /// </summary>
        /// <param name="aiMenuId"></param>
        /// <returns></returns>
        public DataTable GetUserRolesForSelectedNoticeId(int aiNoticeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("NoticeId", aiNoticeId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetUserRolesForSelectedNoticeId]");
            }
        }


        public DataTable GetStandardDivisionsForSelectedNotice(int aiNoticeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("NoticeId", aiNoticeId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetStandardDivisionsForSelectedNoticeId]");
            }
        }

        /// <summary>
        /// This method is used to Delete Notice Details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeId"></param>
        ///  <param name="aiUserId"></param>
        /// <returns></returns>
        public static void Delete(int aiSchoolId, int aiNoticeId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("NoticeId", aiNoticeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Usp_DeleteNoticeDetails");
            }
        }

        /// <summary>
        /// This method is used to get Notice ID for Inputed Notice Name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeName"></param>
        /// <returns></returns>
        public static int GetIDByName(int aiSchoolId, string asNoticeName, string asStartDateTime, string asEndDateTime)
        {
            int iNoticeID = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NoticeName", asNoticeName, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("StartDate", asStartDateTime, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("EndDate ", asEndDateTime, SqlDbType.Char);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetNoticeIdForNotice"))
                {

                    if (oSqlDataReader != null)
                    {
                        oSqlDataReader.Read();
                        {
                            if (oSqlDataReader.HasRows)
                                iNoticeID = Convert.ToInt32(oSqlDataReader["NOticeID"]);
                        }
                    }
                }
            }
            return iNoticeID;
        }

        /// <summary>
        /// This method is used to get Notices for display on Home page and Control Pannel.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public int GetStandarDivisionId(int aiSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                int iStandardDivisionId = 0;

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardDivisionIdNotice"))
                {
                    if (oSqlDataReader != null && oSqlDataReader.Read())
                        iStandardDivisionId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Division_Id"]);
                }
                return iStandardDivisionId;
            }
        }

    /// <summary>
    /// This method is used to get Notices for display on Home page and Control Pannel.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asDisplayLocation"></param>
    /// <param name="aiShowAllNotices"></param>
    public void GetNotices(int aiSchoolId, string asDisplayLocation, int aiShowAllNotices, string asSortExpression,int aiEndRowIndex, int aiStartRowIndex, int aiUserRoleId)
    {
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("DisplayLocation", asDisplayLocation, SqlDbType.Char);
            oSQLServerDbUtility.AddParameter("ShowAllNotices", aiShowAllNotices, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("EndIndex", aiEndRowIndex, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("LoginUserRoleId", aiUserRoleId, SqlDbType.Int);

            using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetNoticeDetails"))
            {
                if (oSqlDataReader != null)
                {
                    while (oSqlDataReader.Read())
                    {
                        NoticeDetails oNoticeDetails = new NoticeDetails
                        {
                            ClassesIds = oSqlDataReader["ClassesIds"].ToString(),
                            NoticeId = Convert.ToInt32(oSqlDataReader["NoticeId"]),
                            NoticeName = Convert.ToString(oSqlDataReader["NoticeName"]),
                            SortOrder = Convert.ToInt32(oSqlDataReader["outSortOrder"]),
                            FileName = Convert.ToString(oSqlDataReader["FileName"]),
                            StartDate = oSqlDataReader["StartDate"].ToString(),
                            NoticeContent = oSqlDataReader["NoticeContent"].ToString(),
                            EndDate = oSqlDataReader["EndDate"].ToString()
                        };
                        lstNoticeDetails.Add(oNoticeDetails);
                    }
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            Event oEventDetails = new Event
                            {
                                EventId = Convert.ToInt32(oSqlDataReader["Event_Id"]),
                                EventDescription = oSqlDataReader["Event_Description"].ToString(),
                                StartDate = Convert.ToDateTime(oSqlDataReader["Event_Start_Date"]),
                            };
                            lstEventDetails.Add(oEventDetails);
                        }
                    }
                }
            }
        }
    }




    public int GetNoticesCount(int aiSchoolId, string asDisplayLocation, int aiShowAllNotices, int aiUserRoleId)
    {
        int iNoticeCount = 0;
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("DisplayLocation", asDisplayLocation, SqlDbType.Char);
            oSQLServerDbUtility.AddParameter("ShowAllNotices", aiShowAllNotices, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("LoginUserRoleId", aiUserRoleId, SqlDbType.Int);
            using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetNoticeListCount"))
            {
                if (oSqlDataReader != null)
                {
                    while (oSqlDataReader.Read())
                    {
                        iNoticeCount = oSqlDataReader["Count"].ToInt();
                    }
                }
            }
        }
        return iNoticeCount;
    }

        /// <summary>
    /// This method is used to get details of all Notices.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asDisplayLocation"></param>
    /// <param name="aiShowAllNotices"></param>
    /// <param name="asSortExpression"></param>
    /// <param name="aiEndRowIndex"></param>
    /// <param name="aiStartRowIndex"></param>
    public void GetAll(int aiSchoolId, string asDisplayLocation, int aiShowAllNotices, bool abText, string asSortExpression, int aiEndRowIndex, int aiStartRowIndex)
    {
        string sEndDate;
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("DisplayLocation", asDisplayLocation, SqlDbType.Char);
            oSQLServerDbUtility.AddParameter("ShowAllNotices", aiShowAllNotices, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + (string.IsNullOrEmpty(asSortExpression) ? "NoticeName" : asSortExpression), SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("EndIndex", aiEndRowIndex, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("IsText",abText,SqlDbType.Bit);
            using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetPagedNoticeDetails"))
            {
                if (oSqlDataReader != null)
                {
                    while (oSqlDataReader.Read())
                    {
                        sEndDate = oSqlDataReader["EndDate"].ToString();
                        if (sEndDate == Constants.S_DEFAULT_DATE_5)
                            sEndDate = string.Empty;
                        NoticeDetails oNoticeDetails = new NoticeDetails
                        {
                            NoticeId = Convert.ToInt32(oSqlDataReader["NoticeId"]),
                            NoticeName = Convert.ToString(oSqlDataReader["NoticeName"]),
                            DisplayLocation = Convert.ToString(oSqlDataReader["DisplayLocation"]),
                            StartDate = oSqlDataReader["StartDate"].ToString(),
                            EndDate = sEndDate.ToString(),
                            SortOrder = Convert.ToInt32(oSqlDataReader["dbSortOrder"]),
                            FileName = Convert.ToString(oSqlDataReader["FileName"]),
                            SchoolId = aiSchoolId,
                            IsSelected = Convert.ToBoolean(oSqlDataReader["IsSelected"]),
                            NoticeDescription = Convert.ToString(oSqlDataReader["NoticeDescription"]),
                            NoticeImage = Convert.ToString(oSqlDataReader["NoticeImage"])
                        };
                        lstNoticeDetails.Add(oNoticeDetails);
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is used to get total count of Notices.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asDisplayLocation"></param>
    /// <param name="aiShowAllNotices"></param>
    /// <returns></returns>
    public int GetCount(int aiSchoolId, string asDisplayLocation, int aiShowAllNotices, bool abText)
    {
        int iCount = 0;
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("DisplayLocation", asDisplayLocation, SqlDbType.Char);
            oSQLServerDbUtility.AddParameter("ShowAllNotices", aiShowAllNotices, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("IsText", abText, SqlDbType.Bit);
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCountNoticeDetails"))
            {
                if (oSqlDataReader != null)
                {
                    oSqlDataReader.Read();
                    {
                        iCount = Convert.ToInt32(oSqlDataReader[0]);
                        iCount = Convert.ToInt32(oSqlDataReader["CNT"]);
                    }
                }
            }
        }
        return iCount;
    }

    /// <summary>
    /// This method is used to return all top notices.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiTopCount"></param>
    /// <returns></returns>
    public List<NoticeDetails> GetAllTopNotices(int aiSchoolId, int aiTopCount)
    {
        List<NoticeDetails> lstNoticeDetails = new List<NoticeDetails>();
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("TopCount", aiTopCount, SqlDbType.Int);
            using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllTopNotices"))
            {
                if (oSqlReader.HasRows)
                {
                    Utility.GenericClass<NoticeDetails> oGenricClass = new GenericClass<NoticeDetails>();
                    lstNoticeDetails = oGenricClass.GetFilledObjectList(oSqlReader);
                }
            }
        }
        return lstNoticeDetails;
    }

    /// <summary>
    /// This method is used to get all Notices for external site.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    public List<NoticeDetails> GetAllNoticeDetails(int aiSchoolId)
    {
        List<NoticeDetails> lstNoticeDetails = new List<NoticeDetails>();
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllNoticesForDisplay"))
            {
                if (oSqlDataReader != null)
                {
                    while (oSqlDataReader.Read())
                    {
                        NoticeDetails oNoticeDetails = new NoticeDetails
                        {
                            NoticeId = Convert.ToInt32(oSqlDataReader["NoticeId"]),
                            NoticeName = Convert.ToString(oSqlDataReader["NoticeName"]),                            
                            StartDate = oSqlDataReader["StartDate"].ToString(),                                                        
                            FileName = Convert.ToString(oSqlDataReader["FileName"]),                  
                            NoticeContent = Convert.ToString(oSqlDataReader["NoticeContent"]),        
                            NoticeDescription = Convert.ToString(oSqlDataReader["NoticeDescription"]),
                            NoticeImage = Convert.ToString(oSqlDataReader["NoticeImage"]),
                            IsText = Convert.ToBoolean(oSqlDataReader["IsText"])
                        };
                        lstNoticeDetails.Add(oNoticeDetails);
                    }
                }
            }
            return lstNoticeDetails;
        }
    }

        #endregion "Public Methods"

        #region "Private Methods"

    /// <summary>
    /// This method is used to get details of single Notice.
    /// </summary>
    /// <param name="aiNoticeId"></param>
    private void Load(int aiNoticeId)
    {
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("NoticeId", aiNoticeId, SqlDbType.Int);
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetSingleNoticeDetails"))
            {
                if (oSqlDataReader != null)
                {
                    string sEndDate;
                    
                    oSqlDataReader.Read();
                    {
                        sEndDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(oSqlDataReader["EndDate"]));
                        if (sEndDate == Constants.S_DEFAULT_DATE_5)
                            sEndDate = string.Empty;
                        moNoticeDetails = new NoticeDetails
                        {
                            NoticeId = Convert.ToInt32(oSqlDataReader["NoticeId"]),
                            NoticeName = Convert.ToString(oSqlDataReader["NoticeName"]),
                            DisplayLocation = Convert.ToString(oSqlDataReader["DisplayLocation"]),
                            StartDate = oSqlDataReader["StartDate"].ToString(),
                            EndDate = oSqlDataReader["EndDate"].ToString(),
                            SortOrder = Convert.ToInt32(oSqlDataReader["dbSortOrder"]),
                            FileName = Convert.ToString(oSqlDataReader["FileName"]),
                            NoticeContent = oSqlDataReader["NoticeContent"].ToString(),
                            IsText = Convert.ToBoolean(oSqlDataReader["IsText"]),
                            NoticeDescription = Convert.ToString(oSqlDataReader["NoticeDescription"]),
                            NoticeImage = Convert.ToString(oSqlDataReader["NoticeImage"]),                            
                        };
                    }
                }
            }
        }
    }
        /// <summary>
        /// This method is used to save selected notice."
        /// </summary>
        /// <param name="asXML"></param>
    public static void SaveSelectedNotices(string asXML)
    {
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("NoticeXML", asXML, SqlDbType.Xml);
            oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateSelectedNotices");
        }
    }
        /// <summary>
        /// This method is used to get max end date of notice.
        /// </summary>
        /// <returns></returns>
    public static DateTime GetMaxEndDate()
    {
        string sSelectStmt = "SELECT MAX(EndDate) as EndDate from NoticeDetails where IsDeleted=0 and IsSelected=1 and (DisplayLocation='H' OR DisplayLocation='B')";
        DateTime dtMaxDate=new DateTime();
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
            {
                if (oSqlDataReader.HasRows)
                {
                    while (oSqlDataReader.Read())
                    {
                        if (oSqlDataReader["EndDate"] != DBNull.Value)
                            dtMaxDate = Convert.ToDateTime(oSqlDataReader["EndDate"]);
                    }
                }
            }            
        }
        return dtMaxDate;
    }

    public bool CompareDate(int aiSchoolId, DateTime adtCompareDate)
    {
        int iFlag = 0;
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("dtCompareDate", adtCompareDate, SqlDbType.DateTime);

            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDisplayNoticeResult"))
            {
                if (oSqlDataReader != null)
                {
                    oSqlDataReader.Read();
                    {
                        if (oSqlDataReader[0].ToInt() == 1)
                            iFlag = 1;
                    }
                }
            }
        }
        if (iFlag == 1)
            return true;
        else
            return false;
    }

    /// <summary>
    /// This method is used to delete the Notice Image.
    /// </summary>
    public void DeleteNoticeImage(int aiNoticeId, int bIsText)
    {
        string sDeleteNoticeImageStatement = "UPDATE NoticeDetails" +
                                               " SET NoticeImage = null" +
                                             " WHERE  " +
                                                  " NoticeId = " + aiNoticeId +
                                                " AND IsText =" + bIsText;
        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            oSQLServerDbUtility.ExecuteTransaction(sDeleteNoticeImageStatement);                                        
    }


    #endregion "Private Methods"
    }
}
