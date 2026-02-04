// Class Name       :- ActivityAssignmentBL.cs
// Purpose          :- This class is used to manage Activity Details for the Staff.
// Date Of creation :- 13/09/2016
// Author Name      :- Dnyaneshwar Shinde.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities.Admin;

namespace BusinessLogic
{
    public class ActivityAssignmentBL
    {
        #region Data members

        private ActivityAssignmentDC moActivityAssignmentDC;

        #endregion

        #region Constructor's

        public ActivityAssignmentBL(int aiSchoolId, int aiUpdatedById)
        {
            moActivityAssignmentDC = new ActivityAssignmentDC(aiSchoolId, aiUpdatedById);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get All Activities.
        /// </summary>        
        public List<Activity> GetAllActivities()
        {
            return this.moActivityAssignmentDC.GetAllActivities();
        }

        /// <summary>
        /// This method is used to get All teachers for Activity assignment.
        /// </summary>
        /// <param name="aiActivityId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        public List<Activity> GetAllTeachersForActivityAssignment(int aiUserRoleId, string asUserName, int aiActivityId)
        {
            List<Activity> lstActivityUsers = moActivityAssignmentDC.GetAllTeachersForActivityAssignment(aiUserRoleId, asUserName, aiActivityId);
            return lstActivityUsers;
        }

        /// <summary>
        /// This method is used to save the Users Activity.
        /// </summary>
        /// <param name="aiActivityId"></param>
        /// <param name="asUsersId"></param>
        public void SaveUsersActivity(int aiActivityId, string asCheckUserIds, string asUnCheckUserIds)
        {
            moActivityAssignmentDC.SaveUsersActivity(aiActivityId, asCheckUserIds, asUnCheckUserIds);
        }

        #endregion
    }
}
