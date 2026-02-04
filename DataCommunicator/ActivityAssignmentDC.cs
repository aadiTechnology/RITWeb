// Class Name       :- ActivityAssignmentDC.cs
// Purpose          :- This class is used to manage Activity Details for the Staff.
// Date Of creation :- 13/09/2016
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using SchoolEntities.Admin;
using System.Data.SqlClient;
using System.Data;

namespace DataCommunicator
{
    public class ActivityAssignmentDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miUpdatedById;

        #endregion    
        
        #region " Constructor's "

        public ActivityAssignmentDC(int aiSchoolid, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolid;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get All Activities.
        /// </summary>    
        public List<Activity> GetAllActivities()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllActivities"))
                {
                    List<Activity> lstActivities = new List<Activity>();
                    while (oSqlDataReader.Read())
                        lstActivities.Add(new Activity { Id = Convert.ToInt32(oSqlDataReader["Id"]), ActivityName = oSqlDataReader["ActivityName"].ToString() });
                    return lstActivities;
                }
            }
        }

        /// <summary>
        /// This method is used to get All teachers for Activity assignment.
        /// </summary>
        /// <param name="aiActivityId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        public List<Activity> GetAllTeachersForActivityAssignment(int aiUserRoleId, string asUserName, int aiActivityId)
        {
            List<Activity> lstActivityUsers = new List<Activity>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UsrName", asUserName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ActivityId", aiActivityId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForActivityAssignment"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstActivityUsers.Add(new Activity
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            IsSaved = Convert.ToBoolean(oSqlDataReader["IsSaved"])
                        });
                    }
                }
            }
            return lstActivityUsers;
        }

        /// <summary>
        /// This method is used to save the Users Activity.
        /// </summary>
        /// <param name="aiActivityId"></param>
        /// <param name="asUsersId"></param>
        public void SaveUsersActivity(int aiActivityId, string asCheckUserIds, string asUncheckUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ActivityId", aiActivityId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CheckUserIds", asCheckUserIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UncheckUserIds", asUncheckUserIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveActivityDetails");
            }
        }
        #endregion
    }
}
