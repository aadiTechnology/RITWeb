/* File Name :- ActivityAssignmentUI.aspx.cs
 * Created Date :- 13-Sept-2016
 * Class Description :- This class is used to Assign the Different Activities to the Users. 
 * Created By :- Dnyaneshwar Shinde
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using System.Data;
using Utility;
using SchoolEntities.Admin;
using System.Web.Services;
using Kendo.DynamicLinq;

public partial class ActivityAssignmentUI : SchoolBase
{
    #region Data Member(s)

    private ActivityAssignmentBL moActivityAssignmentBL;

    #endregion

    #region Event's

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {                
                SetJavascriptAttributes();
                FillActivityDetails();
                ReadQueryString();
                FillUserRoles();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Get Subject Teacher List for expert assignment.
    /// </summary>
    /// <param name="aiUserRoleId"></param>
    /// <param name="aiSchoolId></param>
    /// <param name = "asUserName"></param>
    /// <param name = "aiActivityId"></param>
    [WebMethod]
    public static DataSourceResult GetTeachersForActivityAssignment(int aiUserRoleId, int aiSchoolId, string asUserName, int aiActivityId)
    {
        try
        {
            ActivityAssignmentBL oActivityAssignmentBL = new ActivityAssignmentBL(aiSchoolId, 1);
            List<Activity> lstActivityUsers = oActivityAssignmentBL.GetAllTeachersForActivityAssignment(aiUserRoleId, asUserName, aiActivityId);

            var result = new DataSourceResult()
            {
                Data = lstActivityUsers,
                Total = lstActivityUsers.Count
            };

            return result;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            return null;
        }
    }

    /// <summary>
    /// This Event is used to Get Subject Teacher List for expert assignment.
    /// </summary>
    /// <param name="aiActivityId"></param>
    /// <param name="asUserIds></param>
    /// <param name = "aiSchoolId"></param>
    /// <param name = "aiUpdatedById"></param>
    [WebMethod]
    public static void SaveUsersActivity(int aiActivityId, string asCheckUserIds, string asUnCheckUserIds, int aiSchoolId, int aiUpdatedById)
    {
        try
        {
            ActivityAssignmentBL oActivityAssignmentBL = new ActivityAssignmentBL(aiSchoolId, aiUpdatedById);
            oActivityAssignmentBL.SaveUsersActivity(aiActivityId, asCheckUserIds, asUnCheckUserIds);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This Method is used to set Javascript Attributes..
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel,btnSearch });

        hidSchoolId.Value = miSchoolId.ToString();
        hidUpdatedById.Value = miUserId.ToString();
    }

    /// <summary>
    /// This Method is used to fill the user role Combobox.
    /// </summary>
    private void FillUserRoles()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDtUserRoles = oMasterDataCollectionBL.GetAllUserRoles();
        DataRow[] odrRoles = oDtUserRoles.Select("User_Role_Id IN (" + Constants.UserRoles.Teacher.ToInt() + "," + Constants.UserRoles.Supervisor.ToInt() + "," + Constants.UserRoles.OtherStaff.ToInt() + ")");
        if (odrRoles.Length > 0)
            ListSource.FillDropDownList(odrRoles.CopyToDataTable(), cmbUserRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to fill the Avtivity Combobox.
    /// </summary>
    private void FillActivityDetails()
    {
        moActivityAssignmentBL = new ActivityAssignmentBL(miSchoolId,miUserId);
        List<Activity> lstActivities = moActivityAssignmentBL.GetAllActivities();
        ListSource.FillDropDownList(lstActivities, cmbActivity, "ActivityName", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to set the Query String.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["ActivityId"] != null)
        {
            cmbActivity.SelectedValue = QueryString["ActivityId"];
            cmbActivity.Enabled = false;
        }
    }

    #endregion
}