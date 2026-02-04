/* File Name - CopyReportingConfigPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 25 Feb 2015
 * Class Description - This class is used to copy reporting staff configuration.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StaffPerformanceEntity;
using Utility;

public partial class CopyReportingConfigPopup : SchoolBase
{
    #region Data Member(s)
    
    private ReportingConfigurationBL moReportingConfigurationBL; 

    #endregion

    #region Property(s)

    public int UserRoleId
    {
        get
        {
            if (QueryString["IsLessonPlanScreen"] != null && QueryString["IsLessonPlanScreen"].ToInt() == 1)
                return Constants.UserRoles.Teacher.ToInt();
            else
                return QueryString["UserRoleId"].ToInt();
        }
    }

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill up user list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moReportingConfigurationBL = new ReportingConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillUserDetails();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to copy reporting staff configuration to selected users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder oStringBuilder = new StringBuilder();
            foreach (ListViewDataItem oItem in lstvwUsers.Items)
            {
                if (oItem.ItemType == ListViewItemType.DataItem)
                {
                    CheckBox chkSelect = oItem.FindControl("ChkSelect") as CheckBox;
                    if (chkSelect.Checked)
                    {
                        int iUserId = lstvwUsers.DataKeys[oItem.DisplayIndex]["ReportingUserId"].ToInt();
                        oStringBuilder.Append("," + iUserId);
                    }
                }
            }


            string sUserIds = string.Empty;

            if (oStringBuilder.Length > 0)
                sUserIds = oStringBuilder.ToString().Substring(1);

            Constants.ReportingUserScreen oReportingUserScreen = GetScreenType();

            moReportingConfigurationBL.Copy(QueryString["UserId"].ToInt(), UserRoleId, QueryString["Year"].ToInt(), sUserIds, oReportingUserScreen);

            Response.Write(string.Format("<Script language='Javascript'>window.close();</Script>"));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)
   
    /// <summary>
    /// This method is used to set java scripts attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCopy, btnClose });
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblName.Text = QueryString["UserName"].ToString();

        if (QueryString["UserRole"] == null || QueryString["UserRole"].ToString() == string.Empty)
            trCopyFromRole.Visible = false;
        else
        {
            trCopyFromRole.Visible = true;
            lblUserRole.Text = QueryString["UserRole"].ToString();
        }

    }

    /// <summary>
    /// This method is used to fill up user details.
    /// </summary>
    private void FillUserDetails()
    {
        Constants.ReportingUserScreen oReportingUserScreen = GetScreenType();

         List<ReportingStaff> lstUsers = moReportingConfigurationBL.GetAllUsers(QueryString["UserId"].ToInt(), UserRoleId, QueryString["Year"].ToInt(), oReportingUserScreen);
        lstvwUsers.DataSource = lstUsers;
        lstvwUsers.DataBind();

        btnCopy.Enabled = lstUsers.Count > 0;
    }

    private Constants.ReportingUserScreen GetScreenType()
    {
        Constants.ReportingUserScreen oReportingUserScreen = Constants.ReportingUserScreen.StaffPerformanceEval;
        if (QueryString["IsLessonPlanScreen"] != null && QueryString["IsLessonPlanScreen"].ToInt() == 1)
            oReportingUserScreen = Constants.ReportingUserScreen.LessonPlan;
        return oReportingUserScreen;
    }

    #endregion
}