/* ---------------------------------------------------------------------------------------------------------------
 *	Filename	: MailingGroupDC.cs
 *	Author		: Pravin Shinde
 *	Date		: 25-07-2013
 *	Description	: This class is used to get the mailing group details while sending sms & messages. 
 * ---------------------------------------------------------------------------------------------------------------
 */
namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using SchoolEntities;
    using Utility;

    /// <summary>
    /// This class is used to get and set mailing groups. It communicates with the database.
    /// </summary>
    public class MailingGroupDC
    {
        #region -- MEMBER(s) --

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;                

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR --

        public MailingGroupDC()
        {
            
        }

        public MailingGroupDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
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
            List<MailingGroup> lstMailingGroup = new List<MailingGroup>();
            asRoleIds = string.Empty;
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("RoleId", aiRoleId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("UserId", miUserId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("IsCallFromWebsite", true, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("usp_GetMailingGroups"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstMailingGroup.Add(new MailingGroup
                        {
                            GroupId = oSqlDataReader["GroupId"].ToInt(),
                            Name = oSqlDataReader["GroupName"].ToString(),
                            Users = oSqlDataReader["Users"].ToString(),
                            IsDefault = oSqlDataReader["IsDefault"].ToBool(),
                            IsAllDeactivated = oSqlDataReader["IsAllDeactivated"].ToBool()
                        });
                    }
                    if (aiGroupId != Constants.I_ZERO && oSqlDataReader.NextResult() && oSqlDataReader.Read())
                        asRoleIds = oSqlDataReader["UserRoles"].ToString();
                }

                return lstMailingGroup;
            }
        }

        /// <summary>
        /// This method is used to delete the group.
        /// </summary>
        /// <param name="aiGroupId"></param>
        public void Delete(int aiGroupId)
        {
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);                
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);                
                oSqlDBUtility.ExecuteStoredProcedureOnServer("usp_DeleteMailingGroup");
            }
        }

        /// <summary>
        /// This method is used to delete the perticular group from the listview.
        /// </summary>
        /// <param name="aiGroupId"></param>
        public void DeleteMailingGroupUser(int aiGroupId, int aiUserId = 0)
        {
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSqlDBUtility.ExecuteStoredProcedureOnServer("usp_DeleteMailingGroupUser");
            }
        }

        /// <summary>
        /// This procedure is used to get the group users for selected group.
        /// </summary>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public List<UserInfo> GetGroupUsers(int aiGroupId)
        {
            List<UserInfo> lstUserInfo = new List<UserInfo>();
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("usp_GetUsers"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUserInfo.Add(new UserInfo
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            UserName = oSqlDataReader["UserName"].ToString(),
                            IsDeactivated = oSqlDataReader["IsDeactivated"].ToBool()
                        });
                    }
                }

                return lstUserInfo;
            }
        }        
        
        /// <summary>
        /// This method is used to insert mailing details for  the selected group.
        /// </summary>
        /// <param name="asMailingDetailsXML"></param>
        public void Insert(string asMailingGroupXML)
        {
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);                
                oSqlDBUtility.AddParameter("MailingGroupXML", asMailingGroupXML, SqlDbType.Xml);                
                oSqlDBUtility.ExecuteStoredProcedureOnServer("usp_InsertMailingGroupDetails");
            }
        }

       /// <summary>
       /// This method is used to get the Users id's for selected group.
       /// </summary>
       /// <param name="asGroupId"></param>
       /// <param name="abIsForUsers"></param>
       /// <returns></returns>
       public string GetMailingGroupUsers(string asGroupId, bool abIsForUsers)
       {
           string sUserId = string.Empty;
           using (var oSqlDBUtility = new SQLServerDbUtility())
           {
               oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
               oSqlDBUtility.AddParameter("IsForUsers", abIsForUsers, SqlDbType.Int);
               oSqlDBUtility.AddParameter("Groups", asGroupId, SqlDbType.NVarChar);
               using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("usp_GetMailingGroupUsers"))
               {
                   while (oSqlDataReader.Read())
                   {
                       sUserId = oSqlDataReader["Users"].ToString();
                   }
               }
           }
           return sUserId;
       }

       /// <summary>
       /// This method is used to create the default groups for the school.
       /// </summary>
       public void CreateDefaultGroups()
       {
           using (var oSqlDBUtility = new SQLServerDbUtility())
           {
               oSqlDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
               oSqlDBUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
               oSqlDBUtility.ExecuteStoredProcedureOnServer("usp_CreateDefaultMailingGroups");               
           }
       }

        #endregion -- PUBLIC METHODS --
    }
}
