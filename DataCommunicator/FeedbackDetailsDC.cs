using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using System.Linq;
using SchoolEntities.Dashboard;

namespace DataCommunicator
{
    public class FeedbackDetailsDC
    {
        private FeedbackStructDetails moFeedbackStructDetails;
        public List<FeedbackType> mlstFeedbackTypes=new List<FeedbackType>();

        public List<FeedbackType> FeedbackType
        {
            get { return mlstFeedbackTypes; }
            set { mlstFeedbackTypes = value; }
        } 

        public FeedbackStructDetails FeedbackInfo
        {
            get
            {
                return moFeedbackStructDetails;
            }
            set
            {
                moFeedbackStructDetails = value;
            }
        }

        #region Structure

        public struct FeedbackStructDetails
        {
            public int miUser_Id;

            public int miSchool_Id;

            public string msFeedbackDescription;

            public int miFeedback_Type_Id;

            public string msFeedbackFor;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public string msEmail;

            public int Feedback_Id;

            public string msUserName;

            public int IsSelected { get; set; }
        }

        #endregion Structure

        #region Helping Method

        /// <summary>
        /// This method is used to get data table to fill radio button in Feedback type.
        /// </summary>
        /// <returns></returns>   
        public List<FeedbackTemplate> RetriveFeedbackTypeFromFeedbackTypeMaster()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeedbackTypeAndTemplates"))
                {
                FillFeedbackTypes(oSqlDataReader);
                return FillFeedbackTemplates(oSqlDataReader);
            }
        }}

        /// <summary>
        /// This function is used to fill feed back types.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillFeedbackTypes(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                FeedbackType oFidbackType = new FeedbackType
                {
                    Id = aoSqlDataReader["Feedback_Type_Id"].ToInt(),
                    Type = aoSqlDataReader["Feedback_Type"].ToString()
                };
                mlstFeedbackTypes.Add(oFidbackType);
            }
        }

        /// <summary>
        /// This method is used to fill the templates.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<FeedbackTemplate> FillFeedbackTemplates(SqlDataReader aoSqlDataReader)
        {
            List<FeedbackTemplate> lstFeedbackTemplates = new List<FeedbackTemplate>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    FeedbackTemplate olstFeedbackTemplates = new FeedbackTemplate
                    {
                        FeedbackFor = aoSqlDataReader["FeedbackFor"].ToInt(),
                        FeedbackTypeId = aoSqlDataReader["Type"].ToInt(),
                        Name = aoSqlDataReader["FeedbackTemplate"].ToString()
                    };
                    lstFeedbackTemplates.Add(olstFeedbackTemplates);
                }
            }
            return lstFeedbackTemplates;
        }

        /// <summary>
        /// This method is used to add Feedback in database.
        /// </summary>        
        public void InsertFeedbackDetails()
        {
            string sInsertStatement = "INSERT INTO Feedback_Details ( " +
                                            " [User_Id] " +
                                            " , [School_Id] " +
                                            " , [Feedback_Description] " +
                                            " , [Feedback_Type_Id] " +
                                            " , [Inserted_By_id]" +
                                            ", Feedback_For " +
                                            " , Email_Address " +
                                            " , User_Name "+
                                        ") VALUES (" + moFeedbackStructDetails.miUser_Id
                                        + " , " + moFeedbackStructDetails.miSchool_Id
                                        + " , N'" + StringUtility.ReplaceSingleQuoteInString(moFeedbackStructDetails.msFeedbackDescription, false) + "' "
                                        + " , " + moFeedbackStructDetails.miFeedback_Type_Id
                                        + " , " + moFeedbackStructDetails.miInsertedById
                                        + " , N'" + StringUtility.ReplaceSingleQuoteInString(moFeedbackStructDetails.msFeedbackFor, false) + "' "
                                        + " , N'" + StringUtility.ReplaceSingleQuoteInString(moFeedbackStructDetails.msEmail, false) + "' "
                                        + " , N'"+StringUtility.ReplaceSingleQuoteInString(moFeedbackStructDetails.msUserName,false)+" ' "
                                        + " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }


        /// <summary>
        /// This method is used to get mail addresses for school.
        /// </summary>
        /// <returns></returns>
        public string GetMailAddressForSchool(int aiSchoolId)
        {
            string sFeedbackEmail = string.Empty;
            string sInsertStatement = "SELECT FeedbackEmail FROM School_Master WHERE School_Id=" + aiSchoolId + " AND Is_Deleted='N' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sInsertStatement))
                {
                    if (oSqlDataReader.Read())
                        sFeedbackEmail = oSqlDataReader["FeedbackEmail"].ToString();
                }
            }
            return sFeedbackEmail;
        }


        /// <summary>
        /// This method is used to delete(update Is_Deleted= 'Y') the feedback from database.
        /// </summary>
        public void DeleteFeedbackDetails(int iFeedbackID, int iUserId)
        {
            string sDeleteStatement = " UPDATE [Feedback_Details] SET " +
                                      "  Is_Deleted = '" + Constants.C_YES +
                                      "',  Updated_By_Id =" + iUserId +
                                      ",  Update_Date ='" + System.DateTime.Now +
                                      "'  WHERE " +
                                      "  Feedback_Id = " + iFeedbackID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to get given user role feedback details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortDirection"></param>
        /// <param name="sortExpression"></param>
        /// <param name="asUserName"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public DataTable GetUserFeedbackDetails(int aiSchoolId, string sortDirection, String sortExpression, string asUserName, int maximumRows, int startRowIndex, int iEndIndex)
        {
            string sSortExp;
            if (sortExpression == string.Empty)
                sSortExp = " ORDER BY Feedback_Date " + sortDirection;
            else
                sSortExp = " ORDER BY " + sortExpression + " " + sortDirection;

            string sFilter = CreateFilter(asUserName,string.Empty,0,0,"", "");
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExp, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetPagedUserFeedback]");
            }
        }

       /// <summary>
       /// Displays data on feedbackdetails screen in superadmin login.
       /// </summary>
       /// <param name="aiUserRoleId"></param>
       /// <param name="aiFeedbackTypeId"></param>
       /// <param name="asFeedBackFor"></param>
       /// <param name="aiSchoolId"></param>
       /// <param name="sortDirection"></param>
       /// <param name="asStartDate"></param>
       /// <param name="asEndDate"></param>
       /// <param name="sortExpression"></param>
       /// <param name="startRowIndex"></param>
       /// <param name="iEndIndex"></param>
       /// <param name="abIsServiceCall"></param>
       /// <param name="asDesignationId"></param>
       /// <param name="abIsAccountsCumAdminOfficer"></param>
       /// <returns></returns>
        public DataTable GetUserFeedbackDetails(int aiUserRoleId, int aiFeedbackTypeId, string asFeedBackFor, int aiSchoolId, string sortDirection, string asStartDate, string asEndDate, String sortExpression, int startRowIndex, int iEndIndex, bool abIsServiceCall = false, string asDesignationId = "", bool abIsAccountsCumAdminOfficer = false)
        {
            string sSortExp;
            if (sortExpression == string.Empty)
                sSortExp = " ORDER BY Feedback_Date " + sortDirection;
            else
                sSortExp = " ORDER BY " + sortExpression ;

        
            string sFilter = CreateFilter(string.Empty, asFeedBackFor, aiUserRoleId, aiFeedbackTypeId, asStartDate, asEndDate, abIsServiceCall, asDesignationId, abIsAccountsCumAdminOfficer);
            sFilter = sFilter + " and Is_Selected=1 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExp, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetPagedUserFeedback]");
            }
        }

        /// <summary>
        /// This method is used to save selected feedback.
        /// </summary>
        /// <param name="asXML"></param>
        /// <param name="aiFlag"></param>
        public void SaveSelectedFeedback(string asXML, int aiFlag)
        {
            using (SQLServerDbUtility oSQLServerDbUtility=new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("FeedbackXML",asXML,SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Flag", aiFlag, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateSelectedFeedback");
            }
        }

        /// <summary>
        /// This method is used to get feedback for edit.
        /// </summary>
        /// <param name="aiFeedbackId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public FeedbackStructDetails GetFeedbackToEdit(int aiFeedbackId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Feedback_Id", aiFeedbackId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserFeedback"))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            moFeedbackStructDetails.Feedback_Id = Convert.ToInt32(oSqlDataReader["Feedback_Id"]);
                            moFeedbackStructDetails.miFeedback_Type_Id = Convert.ToInt32(oSqlDataReader["Feedback_Type_Id"]);
                            moFeedbackStructDetails.miUser_Id = Convert.ToInt32(oSqlDataReader["User_Id"]);
                            moFeedbackStructDetails.msEmail = oSqlDataReader["Email_Address"].ToString();
                            moFeedbackStructDetails.msFeedbackDescription = oSqlDataReader["Feedback"].ToString();
                            moFeedbackStructDetails.msFeedbackFor = oSqlDataReader["Feedback_For"].ToString();
                            moFeedbackStructDetails.msUserName = oSqlDataReader["User_Name"].ToString();
                        }
                    }
                }
            }
            return moFeedbackStructDetails;
        }

        /// <summary>
        /// This method is used to get selected feedback display on school notice page.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<FeedbackStructDetails> GetSelectedFeedback( int aiSchoolId)
        {
            List<FeedbackStructDetails> lstDetails = new List<FeedbackStructDetails>();
            string sSelectStmt = " SELECT User_Name,Feedback,Feedback_Date,Email_Address FROM vw_User_Feedback_Details  " +
                                 " WHERE School_Id = " + aiSchoolId +
                                 " AND Is_Selected=1 " +
                                 " AND Is_Deleted='N'" +
                                 " ORDER BY Feedback_Date desc ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        FeedbackStructDetails structDetails = new FeedbackStructDetails();
                        while (oSqlDataReader.Read())
                        {
                            structDetails.msUserName = oSqlDataReader["User_Name"].ToString();
                            structDetails.msFeedbackDescription = oSqlDataReader["Feedback"].ToString();
                            structDetails.msEmail = oSqlDataReader["Email_Address"].ToString();
                            structDetails.mdtInsertDate = Convert.ToDateTime(oSqlDataReader["Feedback_Date"]);
                            lstDetails.Add(structDetails);
                        }
                    }    
                }
            };
            return lstDetails;
        }
       
        /// <summary>
        /// This method is used to save Other type of feedback.
        /// </summary>
        /// <param name="oFeedbackDetails"></param>
        public void InsertOtherFeedbackDetails(FeedbackDetails oFeedbackDetails)
        {
            string sInsertStmt = " INSERT INTO [dbo].[OtherAppreciationDetails] " +
                                 " ([LinkName] "+
                                 " ,[FilePath] "+
                                 " ,[IsDeleted] "+
                                 " ,[SchoolId] " +
                                 " ,[AcademicYearId] "+
                                 " ,[InsertedById] "+
                                 "  )"+
                                 " VALUES" +
                                 " ( N'"+oFeedbackDetails.LinkName + "'"+
                                 " , N'" +oFeedbackDetails.FilePath +"'" +
                                 " , 'N' "+
                                 " , " + oFeedbackDetails.SchoolId +
                                 " , "+oFeedbackDetails.AcademicYearId +
                                 " , "+oFeedbackDetails.InsertedById +
                                 ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sInsertStmt);
            };
        }

        /// <summary>
        /// This method is used to get Other type of feedback.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public static List<FeedbackDetails> GetOtherFeedback(int aiSchoolId,string asFilter)
        {
            if (string.IsNullOrEmpty(asFilter))
                asFilter = " order by InsertDate desc ";
            else if(!asFilter.Contains("AND IsSelected=1"))
                asFilter = "order by " + asFilter;

            string sSelectStmt = "SELECT Id,LinkName,FilePath,IsSelected, InsertDate FROM OtherAppreciationDetails "+
                                 "where IsDeleted='N' AND SchoolId=" + aiSchoolId + asFilter;
            List<FeedbackDetails> lstFeedback = new List<FeedbackDetails>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    FeedbackDetails oFeedbackDetails = null;
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oFeedbackDetails = new FeedbackDetails()
                            {
                                LinkName = oSqlDataReader["LinkName"].ToString(),
                                LinkId = Convert.ToInt32(oSqlDataReader["Id"]),
                                FilePath = oSqlDataReader["FilePath"].ToString(),
                                IsSelected = Convert.ToInt32(oSqlDataReader["IsSelected"]),
                                InsertDate = oSqlDataReader["InsertDate"].ToString()
                            };
                            lstFeedback.Add(oFeedbackDetails);
                        }
                    }
                }
            }
            return lstFeedback;
        }

        /// <summary>
        /// This method is used to update other feedback.
        /// </summary>
        /// <param name="oFeedbackDetails"></param>
        public void UpdateOtherFeedback(FeedbackDetails oFeedbackDetails)
        { 
            string sUpdateStmt="UPDATE OtherAppreciationDetails  "+
                               " SET IsDeleted =N'"+oFeedbackDetails.IsDeleted+"'"+
                               ", LinkName =N'"+oFeedbackDetails.LinkName+"'"+
                               ", FilePath =N'"+oFeedbackDetails.FilePath+"'"+
                               ", UpdateDate=dbo.GetLocalDate(DEFAULT)" +
                               ", UpdatedById="+oFeedbackDetails.InsertedById +
                               " WHERE Id="+oFeedbackDetails.LinkId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sUpdateStmt);
            }
        }

        /// <summary>
        /// This method is used to save other type of feedback.
        /// </summary>
        /// <param name="sXML"></param>
        public void SaveOtherFeedback(string sXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("FeedbackXML", sXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateSelectedOtherFeedback");
            }

        }
        /// <summary>
        /// This method use to get feedback details submited by user
        /// </summary>
        /// <returns></returns>
        public static List<UsersFeedbackDetails> GetFeedbackDetails()
        {
            List<UsersFeedbackDetails> lstUsersFeedbackDetails = new List<UsersFeedbackDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserFeedBackDetailsFromTable"))
                {
                    UsersFeedbackDetails oUsersFeedbackDetails = null;
                    while (oSqlDataReader.Read())
                    {
                        oUsersFeedbackDetails = new UsersFeedbackDetails()
                        {
                            UserName = oSqlDataReader["UserName"].ToString(),
                            FeedbackDescription = oSqlDataReader["Feedback"].ToString(),
                            FeedbackDate = Convert.ToString(oSqlDataReader["FeedbackDate"]),
                        };
                        lstUsersFeedbackDetails.Add(oUsersFeedbackDetails);
                    }
                }
               
            }
            return lstUsersFeedbackDetails;
        }

        /// <summary>
        /// This method is used to get top 10 feedbacks of users.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asDesignationId"></param>
        /// <param name="abIsServiceCall"></param>
        /// <param name="abIsAccountsCumAdminOfficer"></param>
        /// <returns></returns>
        public static List<UserFeedBack> GetUserFeedback(int aiSchoolId, int aiUserRoleId, string asDesignationId = null, bool abIsServiceCall = false, bool abIsAccountsCumAdminOfficer = false)
        {
            DataTable dt = new DataTable();
            List<UserFeedBack> lstUserFeedBackList = new List<UserFeedBack>();
            FeedbackDetailsDC oFeedbackDetailsDC = new FeedbackDetailsDC();
            DataTable dtUserFeedback = oFeedbackDetailsDC.GetUserFeedbackDetails(aiUserRoleId, 0, "School", aiSchoolId, "desc", "", "", "", 0, 10, abIsServiceCall, asDesignationId, abIsAccountsCumAdminOfficer);


            foreach (DataRow UFB in dtUserFeedback.Rows)
            {
                lstUserFeedBackList.Add(new UserFeedBack() { Date = Convert.ToDateTime(UFB[6]).ToString("dd MMM yyyy"), Text = UFB[5].ToString(), UserName = UFB[4] == null ? "Unkonown User" : UFB[4].ToString(), IsSelected = UFB[13].ToBool() });
            }

            return lstUserFeedBackList;
        }

        #endregion
        #region  Private Method
        /// <summary>
        /// This method is used to create the filter for usp.
        /// </summary>
        /// <param name="asUserName"></param>
        /// <param name="asFeedbackFor"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiFeedbackTypeId"></param>
        /// <param name="asStartDate"></param>
        /// <param name="asEndDate"></param>
        /// <param name="abIsServiceCall"></param>
        /// <param name="asDesignationId"></param>
        /// <param name="abIsAccountsCumAdminOfficer"></param>
        /// <returns></returns>
        private string CreateFilter(string asUserName, string asFeedbackFor, int aiUserRoleId, int aiFeedbackTypeId, string asStartDate, string asEndDate, bool abIsServiceCall = false, string asDesignationId = "", bool abIsAccountsCumAdminOfficer = false)
        {
            StringBuilder sFilter = new StringBuilder();
            if (!string.IsNullOrEmpty(StringUtility.ReplaceSingleQuoteInString(asUserName.Trim(),true)))
                sFilter.Append(" AND [User_Name] like '%" + asUserName + "%'");
            /*This code is used to append filetr in case only of when method not called from dashboard*/
            if (aiUserRoleId != 0 && abIsServiceCall == false)
                sFilter.Append(" AND User_Role_Id =" + aiUserRoleId + "");  
            if(aiFeedbackTypeId!=0)
                sFilter.Append(" AND Feedback_Type_Id =" + aiFeedbackTypeId + "");
            if(!string.IsNullOrEmpty(asFeedbackFor) && asFeedbackFor!="0")
                sFilter.Append(" AND Feedback_For =N'" + asFeedbackFor + "'");
            if (!string.IsNullOrEmpty(asStartDate) && !string.IsNullOrEmpty(asEndDate))
                sFilter.Append(" AND Feedback_Date between '" + asStartDate + "' and N'"+asEndDate+"'");

            //This filter is used to show only selected messages on dashboard for user except than admin, Principal and Accounts Cum Admin Officer.
            if (abIsServiceCall && aiUserRoleId != Constants.UserRoles.Admin.ToInt() 
                && asDesignationId != Constants.S_PRINCIPAL_DESIGNATION_ID
                && (aiUserRoleId != Constants.UserRoles.Supervisor.ToInt() && abIsAccountsCumAdminOfficer == false))
                sFilter.Append(" AND Is_Selected = 1");

            return sFilter.ToString();
        }

        #endregion
    }
}
